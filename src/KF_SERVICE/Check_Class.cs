using System;

using System.Data;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.C2C.Finance.BankLib;

namespace TENCENT.OSS.CFT.KF.KF_Service
{
	/// <summary>
	/// Check_Class 的摘要说明。
	/// </summary>
	public class Check_Class
	{
		public Check_Class()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}



		/// <summary>
		/// 创建出一个新的审核。有几个要点要处理，第一：加入参数，第二：判断级别。
		/// 新需求，如果有相同类型相同ID的未处理完成的审批存在，异常退出。
		/// </summary>
		/// <param name="strUserID">发起审批人</param>
		/// <param name="strMainID">审批主ID</param>
		/// <param name="strCheckType">审批类型</param>
		/// <param name="strMemo">审批说明</param>
		/// <param name="strLevelValue">审批判断值(如果为金额请用元)</param>
		/// <param name="myParams">参数</param>
		/// <returns>发起是否成功</returns>
		public static bool CreateCheck(string strUserID, string strMainID, string strCheckType, string strMemo, string strLevelValue, Param[] myParams,Finance_Header myheader)
		{
			strUserID = strUserID.Trim().ToLower();
			

			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
			try
			{
				da.OpenConn();
				da.StartTran();

				//判断审批是否能够启动，针对同一个审批类型，同一个主体ID，不能同时处于审批状态
				string strSql = "select count(1) from c2c_fmdb.t_check_main where FObjID='" + strMainID
					//+ "' and FCheckType='" + strCheckType + "' and (FState<>'finish' and FState<>'finished')";
					//furion 20051226 
					//+ "' and FCheckType='" + strCheckType + "'  and FState<>'finished' ";
					+ "' and FCheckType='" + strCheckType + "'  and FNewState<3 ";

				if(da.GetOneResult(strSql) == "1")  //已经存在
				{
					//return false;
					throw new LogicException("你已发起一个相同审批类型的审批！");
				}
                //理财通余额强赎，同一个财付通账号不能多次发起强赎 v_yqyqguo
                if (strCheckType == "LCTBalanceRedeem")
                {
                    string uin = myParams[0].ParamValue.ToString().Trim();
                    strSql = "select FObjID from c2c_fmdb.t_check_main where FCheckType='" + strCheckType + "' and FCheckMemo like 'LCTFund:true|uin:" + uin + "|total_fee:%'  and FNewState<3 ";
                    string FCheckMemo = da.GetOneResult(strSql);
                    if (!string.IsNullOrEmpty(FCheckMemo))
                    {
                        throw new LogicException("该财付通账号已经有待审批的理财通余额强赎！");
                    }
                }

				strSql = "Insert c2c_fmdb.t_check_main(FObjID) values('" + strMainID + "')";
                
				da.ExecSql(strSql);

				strSql = "select Max(FID) from c2c_fmdb.t_check_main where FObjID='" + strMainID + "'";
				string ID = da.GetOneResult(strSql);

				AddParam(ID,myParams,da);

				int iLevel = GetCheckLevel(strCheckType, strLevelValue,da);

				strSql = "update c2c_fmdb.t_check_main set FCheckType='" + strCheckType + "',FStartUser='"
					//+ strUserID + "',FCheckMemo='" + strMemo + "',FState='start',FCheckLevel=" + iLevel.ToString()
					+ strUserID + "',FCheckMemo='" + strMemo + "',FNewState=0,FCheckLevel=" + iLevel.ToString()
					+ ",FCurrLevel=0,FCheckCount=0,FCheckResult=-1,FStartTime=" + PublicRes.strNowTime + ",FCheckMoney='" + strLevelValue + "' where FID=" + ID ;
				da.ExecSql(strSql); 

				//furion 20060106 为了支持发起即通过,进行特殊处理.
				if(iLevel == 0 && (strCheckType.ToLower() == "fmfk03" || strCheckType.ToLower() == "fmtk04"
					|| strCheckType.ToLower() == "batchpay"|| strCheckType.ToLower() == "batchrefund"))//增加付款和退款发起即通过 andrew 20111117
				{
					strSql = "update c2c_fmdb.t_check_main set FCheckResult=0 where FID=" + ID;
					da.ExecSql(strSql);

					da.Commit();

					ValidCheck(ID,myheader,da);
					return true;
				}
				else
				{
					if(iLevel <= 0)
					{
						throw new LogicException("你发起的审批无人处理，请核对审批类型和配置是否正确！");
					}
					//每次向下走，都要判断条件，所以加上一个新函数。 furion 20050910

					GetNextCheckLevel(ID,da);
					//furion end

					//				//审批发起成功。发送邮件
					//给下一级审批人发送邮件(初次发起为0+1级)
					sendCheckMail(ID,da);
					
					da.Commit();
					return true;
				}

			}
			catch(Exception err)
			{
				da.RollBack();
				log4net.ILog log = log4net.LogManager.GetLogger("Check_Class.CreateCheck");
				if(log.IsErrorEnabled) log.Error(strMainID,err);
				throw;
			}
			finally
			{
				da.Dispose();
			}
		}


		
		/// <summary>
		/// 获取此条件对应于此审批类型的级别，其实不一定要是数值，这样适应性更广。
		/// </summary>
		/// <param name="strCheckType"></param>
		/// <param name="strLevelValue"></param>
		/// <param name="da"></param>
		/// <returns></returns>
		private static int GetCheckLevel(string strCheckType, string strLevelValue, MySqlAccess da)
		{
			for(int i=10; i>0; i--)
			{
				string strSql = "select FCheck" + i.ToString() + " from c2c_fmdb.t_check_type where FTypeID='"
					+ strCheckType + "'";

				string tmp = da.GetOneResult(strSql);
				if(tmp != null && tmp.Trim() != "")
				{
					tmp = String.Format(tmp,strLevelValue);
					tmp = da.GetOneResult("select " + tmp);
					if(tmp != null && tmp.Trim() != "")
					{
						if(tmp == "1") 
						{
							return i;
						}
					}                    
				}
			}
			return 0;
		}


		/// <summary>
		/// 得到确切的下一级审批人是谁。 
		/// </summary>
		/// <param name="strMainID">审批ID</param>
		/// <param name="da">数据访问对象</param>
		private static void GetNextCheckLevel(string strMainID,MySqlAccess da)
		{
			string strSql = "select * from c2c_fmdb.t_check_main where Fid=" + strMainID;

			DataTable dt= da.GetTable(strSql);
			if(dt == null || dt.Rows.Count == 0)
			{
				throw new LogicException("查找不到指定的审批记录！");
			}
				
			int currlevel = Int32.Parse(dt.Rows[0]["FCurrLevel"].ToString());
			int maxlevel = Int32.Parse(dt.Rows[0]["FCheckLevel"].ToString());
			string checkvalue = dt.Rows[0]["FCheckMoney"].ToString();
			string strCheckType = dt.Rows[0]["FCheckType"].ToString();

			int newcurrlevel = currlevel;

			for(int i=currlevel+1; i<=maxlevel; i++)
			{
				strSql = "select FCheck" + i.ToString() + " from c2c_fmdb.t_check_type where FTypeID='"
					+ strCheckType + "'";
				string tmp = da.GetOneResult(strSql);
				if(tmp != null && tmp.Trim() != "")
				{
					tmp = String.Format(tmp,checkvalue);
					tmp = da.GetOneResult("select " + tmp);
					if(tmp != null && tmp.Trim() != "")
					{
						if(tmp == "1") 
						{
							newcurrlevel = i-1;
							break;
						}
					}                    
				}
			}

			strSql = "update c2c_fmdb.t_check_main set FCurrLevel=" + newcurrlevel.ToString() + " where fid=" + strMainID;
			da.ExecSql(strSql);		
		}



		/// <summary>
		/// 发送审批通知邮件
		/// </summary>
		/// <param name="fid">传入审批ID</param>
		/// <param name="da">数据访问对象</param>
		public static void sendCheckMail(string fid, MySqlAccess da)  
		{
			if(PublicRes.IgnoreLimitCheck)
				return ;

			try
			{
				string selectStr = "Select FNewstate,FcheckLevel,FcurrLevel,FcheckType,FstartUser,FcheckMemo,FCheckResult from c2c_fmdb.t_check_main where fid='" + fid + "'";
				string [] ar = new string[7]; //returnDrData
				ar[0] = "FNewstate";
				ar[1] = "FcheckLevel";
				ar[2] = "FcurrLevel";
				ar[3] = "FcheckType";
				ar[4] = "FstartUser";
				ar[5] = "FcheckMemo";

				//furion 20050910 加入新的
				ar[6] = "FCheckResult";

				//ar = PublicRes.returnDrData(selectStr,ar,"ht"); //furion 20050914 加入事务的处理。
				ar = da.drData(selectStr, ar);
					
				string checkState = ar[0].ToString().Trim();
				string checkLevel = ar[1].ToString().Trim();
				int    currLevel  = Int32.Parse(ar[2].ToString().Trim()) + 1;  //当前需要的审批级别
				string checkType  = ar[3].ToString().Trim();
				string startUser  = ar[4].ToString().Trim();
				string checkMemo  = ar[5].ToString().Trim();


				string checkResult = ar[6].Trim();
				string Msg = null;

				string mailFrom  = System.Configuration.ConfigurationManager.AppSettings["mailFrom"].ToString().Trim();


				//如果同意，并且没有结束
				if (checkState != "2" && checkState != "3")  //说明审批还没有结束,需要发送邮件给下一级审批人
				{		
					try
					{
						string cmdStr = "Select FuserID from c2c_fmdb.t_check_user where fcheckType = '" + checkType +"' and flevel = '" + currLevel + "'";
						string mailToStr = da.GetOneResult(cmdStr);

						string sqlStr = "Select fTypeName from c2c_fmdb.t_check_type where ftypeID = '" + checkType + "'";
						string TypeStr = da.GetOneResult(sqlStr);

						string webPath = System.Configuration.ConfigurationManager.AppSettings["webPath"].ToString();
						//					string content = "<a href ='" + webPath + "/login.aspx?name=" + mailToStr + "'>审批任务类型：" + TypeStr + "，审批发起人：" + startUser + "。</a><br> 审批请求描述：" + checkMemo;
						string content = "<html><style type='text/css'>.check {font-size: 14px;color: #000000;} a:hover {font-size: 14px;color: #0099FF;text-decoration: underline;}</style><body><div align='center'><table width='90%' height='118' border='1' cellpadding='1' cellspacing='0' class = 'check'><tr bgcolor='#eeeeee'><td height='28' colspan='2'><p>您有新的任务待审批:</p></td></tr><tr><td width='223' height='22'><p>审批类型: " + TypeStr + " </p></td><td width='251'> <p>审批发起人: " + startUser + " </p></td></tr><tr><td colspan='2'><a href ='" + webPath + "/login.aspx?name=" + mailToStr + "'>审批描述: " + checkMemo + " </a></td></tr></table></div></body></html>";
						PublicRes.sendMail(mailToStr,mailFrom,"[财付通帐务管理系统]您有新的审批待处理",content,"inner",out Msg);
						
					}
					catch
					{
						throw new LogicException("邮件发送失败！");
					}		
				}	

				if(checkState == "2")
				{
					if(checkResult == "0")
					{
						string mailToStr = startUser;

						string sqlStr = "Select fTypeName from c2c_fmdb.t_check_type where ftypeID = '" + checkType + "'";
						string TypeStr = da.GetOneResult(sqlStr);

						string webPath = System.Configuration.ConfigurationManager.AppSettings["webPath"].ToString();
						//					string content = "<a href ='" + webPath + "/login.aspx?name=" + mailToStr + "'>审批任务类型：" + TypeStr + "，审批发起人：" + startUser + "。</a><br> 审批请求描述：" + checkMemo;
						string content = "<html><style type='text/css'>.check {font-size: 14px;color: #000000;} a:hover {font-size: 14px;color: #0099FF;text-decoration: underline;}</style><body><div align='center'><table width='90%' height='118' border='1' cellpadding='1' cellspacing='0' class = 'check'><tr bgcolor='#eeeeee'><td height='28' colspan='2'><p>您的审批已结束:</p></td></tr><tr><td width='223' height='22'><p>审批类型: " + TypeStr + " </p></td><td width='251'> <p>审批已被同意，请去执行审批内容 </p></td></tr><tr><td colspan='2'><a href ='" + webPath + "/login.aspx?name=" + mailToStr + "'>审批描述: " + checkMemo + " </a></td></tr></table></div></body></html>";
						PublicRes.sendMail(mailToStr,mailFrom,"[财付通帐务管理系统]您的审批已被同意",content,"inner",out Msg);
					}
				}

				if(checkState == "3")
				{
					if(checkResult == "0")
					{
						string mailToStr = startUser;

						string sqlStr = "Select fTypeName from c2c_fmdb.t_check_type where ftypeID = '" + checkType + "'";
						string TypeStr = da.GetOneResult(sqlStr);

						string webPath = System.Configuration.ConfigurationManager.AppSettings["webPath"].ToString();
						//					string content = "<a href ='" + webPath + "/login.aspx?name=" + mailToStr + "'>审批任务类型：" + TypeStr + "，审批发起人：" + startUser + "。</a><br> 审批请求描述：" + checkMemo;
						string content = "<html><style type='text/css'>.check {font-size: 14px;color: #000000;} a:hover {font-size: 14px;color: #0099FF;text-decoration: underline;}</style><body><div align='center'><table width='90%' height='118' border='1' cellpadding='1' cellspacing='0' class = 'check'><tr bgcolor='#eeeeee'><td height='28' colspan='2'><p>您的审批已结束:</p></td></tr><tr><td width='223' height='22'><p>审批类型: " + TypeStr + " </p></td><td width='251'> <p>审批已执行 </p></td></tr><tr><td colspan='2'><a href ='" + webPath + "/login.aspx?name=" + mailToStr + "'>审批描述: " + checkMemo + " </a></td></tr></table></div></body></html>";
						PublicRes.sendMail(mailToStr,mailFrom,"[财付通帐务管理系统]您的审批已被处理",content,"inner",out Msg);
					}
				}
			}
			catch(Exception err)
			{
				log4net.ILog log = log4net.LogManager.GetLogger("Check_Class.SendCheckMail");
				if(log.IsErrorEnabled) log.Error(fid,err);
				throw;
			}
		}


		/// <summary>
		/// 加入参数。
		/// </summary>
		/// <param name="strCheckID"></param>
		/// <param name="myParams"></param>
		/// <param name="da"></param>
		private static void AddParam(string strCheckID, Param[] myParams, MySqlAccess da)
		{
			if(myParams == null || myParams.Length == 0) return;
			
						
			foreach(Param aparam in myParams)
			{
				string strSql = "insert c2c_fmdb.t_check_param(FCheckID,FKEY,FVALUE) values(" + strCheckID
					+ ",'" + aparam.ParamName + "','" + aparam.ParamValue +"')";
				da.ExecSql(strSql);
			}

		}


		/// <summary>
		/// 检验当前审批到哪个阶段了，每加一次审批动作就进行此检验。
		/// </summary>
		/// <param name="strCheckID"></param>
		private static void ValidCheck(string strCheckID, Finance_Header myHeader, MySqlAccess da)
		{
			myHeader.UserName = myHeader.UserName.Trim().ToLower();

			//第一步，如果审批是不同意，审批全部结束。   
			//furion 20050831 加入语句 FCurrLevel=FCheckLevel
			//string strSql = "Update c2c_fmdb.t_check_main set FState='finished',FEndTime=" + PublicRes.strNowTime + ",FCurrLevel=FCheckLevel where FID=" 
			string strSql = "Update c2c_fmdb.t_check_main set FNewState=3,FEndTime=" + PublicRes.strNowTime + ",FCurrLevel=FCheckLevel where FID=" 
				+ strCheckID + " and FCheckResult=1";
			da.ExecSql(strSql);

			//第二步，如果是按级审批，而且都审批过了。结束。1代表按级审批
			//strSql = "Update c2c_fmdb.t_check_main A, c2c_fmdb.t_check_type B set A.FState='finish',A.FEndTime=" + PublicRes.strNowTime + ""
			strSql = "Update c2c_fmdb.t_check_main A, c2c_fmdb.t_check_type B set A.FNewState=2,A.FEndTime=" + PublicRes.strNowTime + ""
				//+ " where A.FID=" + strCheckID + " and A.FCheckType=B.FTypeID and B.FRoadType=1 and A.FCurrLevel=A.FCheckLevel and A.FState<>'finished'";
				+ " where A.FID=" + strCheckID + " and A.FCheckType=B.FTypeID and B.FRoadType=1 and A.FCurrLevel=A.FCheckLevel and A.FNewState<3";
			da.ExecSql(strSql);

			//第三步，如果是并行审批，而且最后一个人审批过了，结束。
			strSql = "select FCheckType from c2c_fmdb.t_check_main where FID=" + strCheckID;
			string checktype = da.GetOneResult(strSql);

			strSql = "select FRoadType from c2c_fmdb.t_check_type where FTypeID='" + checktype + "'";
			if(da.GetOneResult(strSql) == "2")
			{
				strSql = "select FCheckLevel from c2c_fmdb.t_check_main where FID=" + strCheckID;
				string checklevel = da.GetOneResult(strSql);

				strSql = "select count(1) from c2c_fmdb.t_check_user where FCheckType='" + checktype 
					+ "' and FLevel <=" + checklevel;
 
				string ineedcount = da.GetOneResult(strSql);

				//strSql = "Update c2c_fmdb.t_check_main set FState='finish',FEndTime=" + PublicRes.strNowTime + " where FID=" 
				strSql = "Update c2c_fmdb.t_check_main set FNewState=2,FEndTime=" + PublicRes.strNowTime + " where FID=" 
					//+ strCheckID + " and FCheckCount=" + ineedcount + " and FState<>'finished'";
					+ strCheckID + " and FCheckCount=" + ineedcount + " and FNewState<3";
				da.ExecSql(strSql);
			}

			//da.ExecSqls_Trans(al);

			//第四步，如果是审批结束而且同意，如果是直接执行，调用函数。B.FActionType=2为直接执行
			strSql = "select count(1) from c2c_fmdb.t_check_main A,c2c_fmdb.t_check_type B where A.FID=" + strCheckID 
				//+ " and A.FState='finish' and A.FcheckType=B.FTypeID and B.FActionType=2 and A.FCheckResult=0"; //判断条件  A.FCheckResult=0 表示通过审批的
				+ " and A.FNewState=2 and A.FcheckType=B.FTypeID and B.FActionType=2 and A.FCheckResult=0"; //判断条件  A.FCheckResult=0 表示通过审批的

			if(da.GetOneResult(strSql) == "1")//此处1表示表示 有审批的执行类别FActionType=2立即执行存在，执行立即执行。//否则不执行，由另外的按钮调用执行。
			{
				if(!FinishCheck.ExecuteCheck(strCheckID,myHeader,da))
				{
					//这时候要引发异常来导致外面rollback
					throw new LogicException("执行审批任务时出现了错误！");
				}
			}
		}
	}
}
