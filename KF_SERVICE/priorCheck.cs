using System;
using System.Data;
using System.Collections;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.C2C.Finance.BankLib;
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.CFT.KF.DataAccess;

namespace TENCENT.OSS.CFT.KF.KF_Service
{
	/// <summary>
	/// 定义一个前置的检查权限的接口
	/// </summary>
	public interface IpriorCheck
	{
		bool checkRight(Param [] pa,out string Msg);
	}

	/// <summary>
	/// 实现最后操作的接口。
	/// </summary>
	public interface IBaseDoCheck
	{
		bool DoAction(string strCheckID, Finance_Header myHeader);
	}

	/// <summary>
	/// priorCheck 的摘要说明。
	/// </summary>
	public class priorCheck
	{
		public priorCheck()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		/// <summary>
		/// 根据每一种不同类型的审批，分别处理不同的权限检查。
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static IpriorCheck GetHandler(string str)
		{
			switch(str)
			{
				case "ChangeQQ":                  //furion 20051030 加入修改QQ
					return new ChangeQQClass();
				case "DKAdjustFail" : //客服系统发起的代扣调整状态审批
					return new DKAdjustFailClass();
				default:
					return null;
			}

			return null;

			/*
			switch(str)
			{
				case "mcTrans":
					return new mcTransClass();
					break;
				case "ChangeQQ":                  //furion 20051030 加入修改QQ
					return new ChangeQQClass();
				case "NewMedi":
					return new NewMediClass();
				case "ModifyMedi":
					return new NewMediClass();
				case "NewCoinPub" :
					return new NewCoinPubClass();
				case "ModifyCoinPub" :
					return new NewCoinPubClass();
				case "settle":
					return new settleClass();
				case "returnticket":
					return new returnticket();
				case "mannalFetch" :
					return new mannalFetch();

				case "mannalRefund" :
					return new mannalRefund();

				case "fmfk03" :
					return new fmfk03();
				case "fmtk04" :
					return new fmtk04();
				case "tokenAdjust" :
					return new tokenAdjust();
				case "HaoYe" :
					return new HaoYeCheck();
				case "proxyFetch" :
					return new proxyFetch();
				case "manualTransfer" :
					return new manualTransfer();
				case "batchsettle":
					return new batchsettleClass();
				case "mannallist" :
					return new MannalListClass();
				case "directpayfail" :
					return new directpayfailClass();
				case "ConfigExportTaskComm":
					return new ConfigExportTaskCommClass();//新增、修改通用汇总任务配置表
				case "ConfigExportBank":
					return new ConfigExportBankClass();//新增、修改通用汇总任务配置表
				case "ConfigExportTaskTmp":
					return new ConfigExportTaskTmpClass();//新增、修改临时汇总任务配置表
				case "ConfigExportBankRest":
					return new ConfigExportBankRestClass();//新增、修改非付款人临时表
				case "ConfigBankType":
					return new ConfigBankTypeClass();//银行编码配置
				case "CopyRights":
					return new CopyRightsClass();//银行编码配置
				default:
					return null; 
			}
			*/
		}


		public class ChangeQQClass : IpriorCheck,IBaseDoCheck
		{
			//逻辑：如果同种类型的话可以更改。
			//如果不同类型的话，需要判断新的帐号是否是旧帐号的相关属性信息。
			private bool CheckTwoQQ(string oldqq, string newqq, out string Msg)
			{			
				Msg = "";

				if(oldqq == null || oldqq.Trim() == "")
				{
					Msg = "请给出原帐户！";
					return false;
				}

				if(newqq == null || newqq.Trim() == "")
				{
					Msg = "请给出新帐户号码！";
					return false;
				}

				Common.enmQQType oldtype = Common.DESCode.GetQQType(oldqq.Trim());
				Common.enmQQType newtype = Common.DESCode.GetQQType(newqq.Trim());

				//furion 20061117 email登录修改
				if(oldtype == Common.enmQQType.UNKNOWN || newtype == Common.enmQQType.UNKNOWN)
				{
					Msg = "请给出正确的帐户号码";
					return false;
				}

				string olduid3 = "";
				if(oldtype == Common.enmQQType.EMAIL)
				{
					if(!Common.DESCode.GetEmailUid(oldqq,out olduid3))
					{
						Msg = "解析EMAIL时出现错误！" + olduid3;
						return false;
					}
				}
				else
					olduid3 = oldqq.Trim();

				string newuid3 = "";
				if(newtype == Common.enmQQType.EMAIL)
				{
					if(!Common.DESCode.GetEmailUid(newqq,out newuid3))
					{
						Msg = "解析EMAIL时出现错误！" + newuid3;
						return false;
					}
				}
				else
					newuid3 = newqq.Trim();
		    
				//MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YWB_30"));
				//MySqlAccess da_zl = new MySqlAccess(PublicRes.GetConnString("ZL"));
				try
				{
					//da.OpenConn();
					//da_zl.OpenConn();

					/*
					//furion 20061117 email登录修改。由于有可能UID为空，所以这个地方语句还要改。
					//string strSql = "select count(*) from " + PublicRes.GetTName("t_relation",oldqq) + " where fqqid='" + oldqq + "' and FSign=1";				
					string strSql = "select count(*) from " + PublicRes.GetTName("t_relation",olduid3) + " where fqqid='" + oldqq 
						+ "' and FSign=1 and ifnull(fuid,0) > 0 ";

				

					//if(da.GetOneResult(strSql) == "0")
					if(da_zl.GetOneResult(strSql) == "0")
					{
						Msg = "原帐户不存在或已作废！";
						return false;
					}
					*/

					string strSql = "uin=" + oldqq + "&wheresign=1";		
					string struid = CommQuery.GetOneResultFromICE(strSql,CommQuery.QUERY_RELATION,"fuid",out Msg);

					if(struid == null)
					{
						Msg = "原帐户不存在或已作废！";
						return false;
					}

					/*
					//strSql = "select count(*) from " + PublicRes.GetTName("t_relation",newqq) + " where fqqid='" + newqq + "' ";
					strSql = "select count(*) from " + PublicRes.GetTName("t_relation",newuid3) + " where fqqid='" + newqq + "' ";
				
					//if(da.GetOneResult(strSql) == "1")
					if(da_zl.GetOneResult(strSql) == "1")
					{
						//furion 20061117 email登录修改
						//furion 20061031 还要判断是否是快速交易.
						//strSql = "select count(*) from " + PublicRes.GetTableName("t_user",newqq) + " where Fqqid='" + newqq + "' ";
						string fuid = PublicRes.ConvertToFuid(newqq);
						//strSql = "select count(*) from " + PublicRes.GetTName("t_user",fuid) + " where Fuid=" + fuid;
						strSql = "select count(*) from " + PublicRes.GetTName("t_user_info",fuid) + " where Fuid=" + fuid;

						//if(da.GetOneResult(strSql) == "1")
						if(da_zl.GetOneResult(strSql) == "1")
						{
							Msg = "新帐号已经是财付通帐户，请重新选择帐号！";
							return false;
						}
					}
					*/

					strSql = "uin=" + newqq ;		
					struid = CommQuery.GetOneResultFromICE(strSql,CommQuery.QUERY_RELATION,"fuid",out Msg);
					if(struid != null)
					{
						Msg = "新帐号已经是财付通帐户，请重新选择帐号！";
						return false;
					}

					//furion 新加判断，同类切换允许，不同类切换时原号的新属性必须未绑定。
					if(newtype != oldtype)
					{
						// TODO: 1客户信息资料外移
						string fuid = PublicRes.ConvertToFuid(oldqq);

						/*
						strSql = "select ifnull(Fqqid,''),ifnull(Femail,''),ifnull(Fmobile,'') from " 
							+ PublicRes.GetTName("t_user_info",fuid) + " where Fuid=" + fuid;

					

						//DataSet ds = da.dsGetTotalData(strSql);
						DataSet ds = da_zl.dsGetTotalData(strSql);

						if(ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null || ds.Tables[0].Rows.Count != 1)
						{
							Msg = "读取已有帐号信息时出错。";
							return false;
						}

						string oldotherid = "";
						if(newtype == enmQQType.QQ)
						{
							oldotherid = ds.Tables[0].Rows[0][0].ToString().Trim().ToLower();
						}
						else if(newtype == enmQQType.EMAIL)
						{
							oldotherid = ds.Tables[0].Rows[0][1].ToString().Trim().ToLower();
						}
						else if(newtype == enmQQType.MOBILE)
						{
							oldotherid = ds.Tables[0].Rows[0][2].ToString().Trim().ToLower();
						}
						*/

						strSql = "uid=" + fuid;
						string fieldstr = "";

						if(newtype == Common.enmQQType.QQ)
						{
							fieldstr = "Fqqid";
						}
						else if(newtype == Common.enmQQType.EMAIL)
						{
							fieldstr = "Femail";
						}
						else if(newtype == Common.enmQQType.MOBILE)
						{
							fieldstr = "FMobile";
						}
						string oldotherid = CommQuery.GetOneResultFromICE(strSql,CommQuery.QUERY_USERINFO,fieldstr,out Msg);

						if(oldotherid != "")
						{
							string oldotheruid3 = oldotherid;
							/*
							if(newtype == enmQQType.EMAIL)
							{
								if(!Common.DESCode.GetEmailUid(oldotherid,out oldotheruid3))
								{
									Msg = "解析EMAIL时出现错误！" + oldotheruid3;
									return false;
								}
							}

						
							strSql = " select count(*) from " + PublicRes.GetTName("t_relation",oldotheruid3)
								+ " where Fqqid='" + oldotherid + "' and Fsign=1 and fuid>0";

							//if(da.GetOneResult(strSql) != "0")
							if(da_zl.GetOneResult(strSql) != "0")
							*/

							strSql = "uin=" + oldotheruid3 + "&wheresign=1" ;		
							struid = CommQuery.GetOneResultFromICE(strSql,CommQuery.QUERY_RELATION,"fuid",out Msg);
							if(struid != null)
							{
								string strmsg = "";
								if(newtype == Common.enmQQType.EMAIL)
								{
									strmsg = "EMAIL";
								}
								else if(newtype == Common.enmQQType.MOBILE)
								{
									strmsg = "手机号";
								}
								else if(newtype == Common.enmQQType.QQ)
								{
									strmsg = "QQ号";
								}

								Msg = "旧帐号的" + strmsg + "已绑定为帐号类型，无法修改为新帐号。";
								return false;
							}
						}
					}

					return true;
				}
				finally
				{
					//da.Dispose();
					//da_zl.Dispose();
				}
			}

			public bool checkRight(Param [] pa,out string Msg)
			{
				Msg = "";

			
				//获得已有参数.
				string oldqq = "";
				string newqq = "";

				foreach(Param param in pa)
				{
					if(param.ParamName.ToLower().Trim() == "oldqq")
					{
						oldqq = param.ParamValue.Trim();
					}
					else if(param.ParamName.ToLower().Trim() == "newqq")
					{
						newqq = param.ParamValue.Trim();
					}
				}
			
				return CheckTwoQQ(oldqq,newqq,out Msg);
			
			}

			#region IBaseDoCheck 成员

			public bool DoAction(string strCheckID, Finance_Header myHeader)
			{
				//已改动 furion V30_FURION核心查询需改动 用接口实现修改
				//MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YW"));
				MySqlAccess daht = new MySqlAccess(PublicRes.GetConnString("HT"));

				//MySqlAccess da_zl = new MySqlAccess(PublicRes.GetConnString("ZL"));
		
				
				ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP,PublicRes.ICEPort);
				try
				{
					string strSql = "Select * from c2c_fmdb.t_check_param where fcheckid =" + strCheckID;

					daht.OpenConn();				

					DataTable dt = daht.GetTable(strSql);				

					if(dt == null || dt.Rows.Count == 0)
					{
						throw new LogicException("审批参数已丢失！");
					}

					string oldqq = "";
					string newqq = "";
					string memo = "";

					foreach(DataRow dr in dt.Rows)
					{
						if(dr["FKey"].ToString().Trim().ToLower() == "oldqq")
						{
							oldqq = dr["FValue"].ToString().Trim();
						}
						else if(dr["FKey"].ToString().Trim().ToLower() == "newqq")
						{
							newqq = dr["FValue"].ToString().Trim();
						}
						else if(dr["FKey"].ToString().Trim().ToLower() == "memo")
						{
							memo = dr["FValue"].ToString().Trim();
						}
					}
	
					//改为调用接口实现

					string inmsg1 = "&uin=" + oldqq ;
					inmsg1 += "&new_uin=" + newqq;			
					inmsg1 += "&memo=" + ICEAccess.ICEEncode(memo);
					inmsg1 += "&op_type=3";
					inmsg1 += "&watch_word=" + PublicRes.GetWatchWord("ui_acc_replacement_service");

					string reply = "";
					string msg = "";
					short result = -1;

					if(commRes.middleInvoke("ui_acc_replacement_service",inmsg1,true,out reply,out result,out msg))
					{
						if(result != 0)
						{
							msg = "转换接口ui_acc_replacement_service返回失败：result=" + result + "，msg=" + msg;
							return false;
						}
						else
						{	
							if(reply.IndexOf("result=0") > -1)
							{
								return true;
							}
							else
							{
								msg = "转换接口ui_acc_replacement_service返回失败：reply=" + reply;
								return false;
							}
						}
					}
					else
					{
						return false;
					}


					/*
					string Msg = "";
					if(!CheckTwoQQ(oldqq,newqq,out Msg))
					{
						throw new LogicException(Msg);
					}

					//da.OpenConn();
					//da.StartTran();

					//da_zl.OpenConn();
					//da_zl.StartTran();
					//furion 20061120 email登录修改
					//start
					string newuid3 = "";
					try
					{
						long itmp = long.Parse(newqq);
						newuid3 = newqq;
					}
					catch
					{
						if(!Common.DESCode.GetEmailUid(newqq,out newuid3))
							throw new LogicException("解析EMAIL有误" + newuid3);
					}
					//end	

					//start
					string olduid3 = "";
					try
					{
						long itmp = long.Parse(oldqq);
						olduid3 = oldqq;
					}
					catch
					{
						if(!Common.DESCode.GetEmailUid(oldqq,out olduid3))
							throw new LogicException("解析EMAIL有误" + olduid3);
					}
					//end

					Common.enmQQType oldtype = Common.DESCode.GetQQType(oldqq.Trim());
					Common.enmQQType newtype = Common.DESCode.GetQQType(newqq.Trim());

					strSql = "uin=" + newqq ;		
					string struid = CommQuery.GetOneResultFromICE(strSql,CommQuery.QUERY_RELATION,"fuid",out Msg);
					if(struid != null)
					{
						Msg = "新帐号已经是财付通帐户，请重新选择帐号！";
						return false;
					}

					/*
					//开始处理。
					strSql = "select Fuid from " + PublicRes.GetTName("t_relation",olduid3) + " where fqqid='" + oldqq + "'";
					//string fuid = da.GetOneResult(strSql);
					string fuid = da_zl.GetOneResult(strSql);

					//第一步，把原有QQ设成作废。//furion 20051031 把原有关系删除掉.
					//strSql = "update " + PublicRes.GetTName("t_relation",oldqq) + " set Fsign=2 where Fqqid='" + oldqq + "' and FSign=1";
					strSql = "delete from " + PublicRes.GetTName("t_relation",olduid3) + " where Fuid=" + fuid + " and FSign=1";
					//if(da.ExecSqlNum(strSql) != 1)
					if(da_zl.ExecSqlNum(strSql) != 1)
					{
						throw new LogicException("删除原帐户关系时出错！");
					}
					*/

					/*
					strSql = "uin=" + oldqq ;		
					string fuid = CommQuery.GetOneResultFromICE(strSql,CommQuery.QUERY_RELATION,"fuid",out Msg);
					if(fuid == null)
					{
						Msg = "旧帐号已经不是财付通帐户，请重新选择帐号！";
						return false;
					}

					string errMsg = "";
					strSql = "uin=" + oldqq;
					CommQuery.ExecSqlFromICE(strSql,CommQuery.DELETE_RELATION,out errMsg);

					//第二步，插入新的对应关系。			

					string newpwd = ""; //遗留问题，获取新支付密码后如何通知用户。
					string telluserpwd = "";
					if(newtype == enmQQType.QQ)
					{
						newpwd = "NULL";
					}
					else
					{
						telluserpwd =  FinanceManage.makePwd();
						newpwd = "'" +System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(telluserpwd,"md5").ToLower() + "'";
					}
					*/
					/*
									strSql = "Insert " + PublicRes.GetTName("t_relation",newuid3) + "(Fqqid,Fuid,Fsign,Flogin_type,Flogin_passwd) "
										+ " values('" + newqq + "'," + fuid + ",1," + (int)newtype + "," + newpwd + ")";
									//furion 20061031 还要判断是否是快速交易.
				

				
									//string strSqlx = "select count(*) from " + PublicRes.GetTName("t_relation",newqq) + " where fqqid='" + newqq + "' ";
									string strSqlx = "select count(*) from " + PublicRes.GetTName("t_relation",newuid3) + " where fqqid='" + newqq + "' ";

									//if(da.GetOneResult(strSqlx) == "1")
									if(da_zl.GetOneResult(strSqlx) == "1")
									{
										string tmpuid = PublicRes.ConvertToFuid(newqq);

										//strSqlx = "select count(*) from " + PublicRes.GetTableName("t_user",newqq) + " where Fqqid='" + newqq + "' ";
										strSqlx = "select count(*) from " + PublicRes.GetTName("t_user_info",tmpuid) + " where Fuid=" + tmpuid;

										//if(da.GetOneResult(strSqlx) == "0")
										if(da_zl.GetOneResult(strSqlx) == "0")
										{
											//strSql = " update " + PublicRes.GetTName("t_relation",newqq) + " set Fuid=" + fuid + " where fqqid='" + newqq + "' ";
											//strSql = " update " + PublicRes.GetTName("t_relation",newqq) + " set Fuid=" + fuid + ",FSign=1 where fqqid='" + newqq + "' ";
											strSql = " update " + PublicRes.GetTName("t_relation",newuid3) + " set Fuid=" + fuid + ",FSign=1 where fqqid='" + newqq + "' ";
										}
										else
										{
											throw new LogicException("新QQ号已经是财付通帐户，请重新选择QQ号！");
										}
									}

									//if(da.ExecSqlNum(strSql) != 1)
									if(da_zl.ExecSqlNum(strSql) != 1)
									{
										throw new LogicException("新增帐户关系时出错！");
									}
				
					*/

					/*
					//insert into c2c_db_$XX$.t_relation_$Y$(Fqqid,Fuid,Fsign,Flogin_type,Flogin_passwd) values('$uin$',$uid$,$sign$,$login_type$,'$login_passwd$');
					strSql = "uin=" + newqq + "&uid=" + fuid + "&sign=1&login_type="+(int)newtype+"&login_passwd="+newpwd;
					CommQuery.ExecSqlFromICE(strSql,CommQuery.INSERT_RELATION,out errMsg);

					//furion 20061117 email登录修改 这里遇到新问题,要从新旧帐号来判断是否允许更换.
					//第三步，修改用户帐户表。

					string strUpdate = "data=" ;

					if(newtype == enmQQType.QQ)
					{
	//					strSql = "Update " + PublicRes.GetTName("t_user",fuid) + " set Fqqid='" + newqq + "' where fuid=" + fuid;
	//					if(da.ExecSqlNum(strSql) != 1)
	//					{
	//						throw new LogicException("修改用户帐户表时出错！");
	//					}

						strUpdate += ICEAccess.URLEncode("fqqid=" + newqq);

						/*
						// TODO: 1客户信息资料外移
						//第四步，修改用户绑定银行帐户表
						strSql = "Update " + PublicRes.GetTName("t_bank_user",fuid) + " set Fqqid='" + newqq + "' where fuid=" + fuid ;
						//if(da.ExecSqlNum(strSql) != 1)
						if(da_zl.ExecSqlNum(strSql) != 1)
						{
							throw new LogicException("修改用户绑定银行帐户表时出错！");
						}
						*/

					/*
						strSql = "uid=" + fuid;
						strSql += "&curtype=1";
						strSql += "&modify_time=" + PublicRes.strNowTimeStander;
						strSql += "&qqid=" + newqq;

						int iresult = CommQuery.ExecSqlFromICE(strSql,CommQuery.UPDATE_BANKUSER,out errMsg);

						if(iresult != 1)
						{
							throw new LogicException("修改用户绑定银行帐户表时出错！");
						}
					}
					else
					{
	//					strSql = "Update " + PublicRes.GetTName("t_user",fuid) + " set Fqqid='' where fuid=" + fuid;
	//					if(da.ExecSqlNum(strSql) != 1)
	//					{
	//						throw new LogicException("修改用户帐户表时出错！");
	//					}

						strUpdate += ICEAccess.URLEncode("fqqid=");

						/*
						// TODO: 1客户信息资料外移
						//第四步，修改用户绑定银行帐户表
						strSql = "Update " + PublicRes.GetTName("t_bank_user",fuid) + " set Fqqid='' where fuid=" + fuid ;
						//if(da.ExecSqlNum(strSql) != 1)
						if(da_zl.ExecSqlNum(strSql) != 1)
						{
							throw new LogicException("修改用户绑定银行帐户表时出错！");
						}
						*/

					/*
						strSql = "uid=" + fuid;
						strSql += "&curtype=1";
						strSql += "&modify_time=" + PublicRes.strNowTimeStander;
						strSql += "&qqid=" ;

						int iresult = CommQuery.ExecSqlFromICE(strSql,CommQuery.UPDATE_BANKUSER,out errMsg);

						if(iresult != 1)
						{
							throw new LogicException("修改用户绑定银行帐户表时出错！");
						}
					}

					ice.OpenConn();
					string strwhere = "where=" + ICEAccess.URLEncode("fcurtype=1&");
					strwhere += ICEAccess.URLEncode("fuid=" + fuid + "&");
				
					strUpdate += ICEAccess.ICEEncode("&fmodify_time=" + PublicRes.strNowTimeStander);

					string strResp = "";

					//3.0接口测试需要 furion 20090708
					if(ice.InvokeQuery_Exec(YWSourceType.用户资源,YWCommandCode.修改用户信息,fuid,strwhere + "&" + strUpdate,out strResp) != 0)
					{
						throw new Exception("修改QQ出错！" + strResp);
					}

					/*
					//第四步，修改用户资料表
					string strset = "";
					if(newtype == oldtype)
					{
						if(newtype == enmQQType.QQ)
						{
							strset = " set Fqqid='" + newqq + "' ";					
						}
						else if(newtype == enmQQType.EMAIL)
						{
							strset = " set Femail='" + newqq + "' ";
						}
						else if(newtype == enmQQType.MOBILE)
						{
							strset = " set Fmobile='" + newqq + "' ";
						}
					}
					else
					{
						if(newtype == enmQQType.QQ)
						{
							strset = " set Fqqid='" + newqq + "' ";					
						}
						else if(newtype == enmQQType.EMAIL)
						{
							strset = " set Femail='" + newqq + "' ";
						}
						else if(newtype == enmQQType.MOBILE)
						{
							strset = " set Fmobile='" + newqq + "' ";
						}

						if(oldtype == enmQQType.QQ)
						{
							strset += ",Fqqid='' ";					
						}
						else if(oldtype == enmQQType.EMAIL)
						{
							strset += ",Femail='' ";
						}
						else if(oldtype == enmQQType.MOBILE)
						{
							strset += ",Fmobile='' ";
						}
					}

					// TODO: 1客户信息资料外移
					strSql = "Update " + PublicRes.GetTName("t_user_info",fuid) + strset + " where fuid=" + fuid;
					//if(da.ExecSqlNum(strSql) != 1)
					if(da_zl.ExecSqlNum(strSql) != 1)
					{
						throw new LogicException("修改用户资料表时出错！");
					}

					*/

					/*
					string strset = "uid=" + fuid;
					strset += "&modify_time=" + PublicRes.strNowTimeStander;

					if(newtype == oldtype)
					{
						if(newtype == enmQQType.QQ)
						{
							strset += "&qqid=" + newqq;					
						}
						else if(newtype == enmQQType.EMAIL)
						{
							strset += "&email=" + newqq;	
						}
						else if(newtype == enmQQType.MOBILE)
						{
							strset += "&mobile=" + newqq;	
						}
					}
					else
					{
						if(newtype == enmQQType.QQ)
						{
							strset += "&qqid=" + newqq;				
						}
						else if(newtype == enmQQType.EMAIL)
						{
							strset += "&email=" + newqq;	
						}
						else if(newtype == enmQQType.MOBILE)
						{
							strset += "&mobile=" + newqq;	
						}

						if(oldtype == enmQQType.QQ)
						{
							strset += "&qqid=" ;				
						}
						else if(oldtype == enmQQType.EMAIL)
						{
							strset += "&email=" ;
						}
						else if(oldtype == enmQQType.MOBILE)
						{
							strset += "&mobile=" ;
						}
					}

					int isqlnum = CommQuery.ExecSqlFromICE(strSql,CommQuery.UPDATE_USERINFO,out errMsg);

					if(isqlnum != 1) 
					{
						throw new LogicException("修改用户资料表时出错！");
					}

					//第五步，修改用户取回密码表表
					strSql = "Update " + PublicRes.GetTName("t_get_passwd",fuid) + " set Fqqid='" + newqq + "' where fuid=" + fuid ;
	//				if(da.ExecSqlNum(strSql) != 1)
	//				{
	//					throw new LogicException("修改用户取回密码表时出错！");
	//				}

					//第六步,后台帐务系统中加入一条记录供查询.
					strSql = "insert c2c_fmdb.t_changeqq_list(FUserID,FActionTime,FOldQQ,FNewQQ,FMemo)"
						+ " values('{0}','{1}','{2}','{3}','{4}')";

					strSql = String.Format(strSql,myHeader.UserName,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),oldqq,newqq,memo);

					if(daht.ExecSqlNum(strSql) != 1)
					{
						throw new LogicException("在后台帐务系统增加修改记录时出错！");
					}

					PublicRes.ReleaseCache(oldqq,"qqid");

					//da.Commit();
					//da_zl.Commit();

					return true;
					*/
				
				}
				catch(Exception err)
				{
					//da.RollBack();
					//da_zl.RollBack();

					ice.CloseConn();

					log4net.ILog log = log4net.LogManager.GetLogger("ChangeQQClass.DoAction");
					if(log.IsErrorEnabled) log.Error(strCheckID,err);

					throw;
				}
				finally
				{
					//da.Dispose();
					daht.Dispose();
					//da_zl.Dispose();
					ice.Dispose();
				}
			}

			#endregion
		}



		public class DKAdjustFailClass : IpriorCheck
		{


			#region IpriorCheck 成员

			public bool checkRight(Param[] pa, out string Msg)
			{
				Msg = "";

				string batchid = "";
				string checkmemo = "";

				foreach(Param dr in pa)
				{
					if(dr.ParamName == "batchid")
					{
						batchid = dr.ParamValue.Trim();
					}

					if(dr.ParamName == "checkmemo")
					{
						checkmemo = dr.ParamValue.Trim();
					}
				}

				if(batchid == "")
				{
					Msg = "此审批单参数有误";
					return false;
				}

                MySqlAccess dainc = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
				try
				{
					dainc.OpenConn();
					//0初始标记　１已发起审批　２审批成功并执行　３撤消审批（允许再发起）
					//7 已发送银行   8 交易成功   9 交易失败

					string strSql = "update cft_cep_db.t_cep_adjust set Fcheckstate=1,Fmodify_time=now(),Fendstate=9 where FCheckBatchID ='"+batchid+"' and Fcheckstate=0"; 

					dainc.ExecSqlNum(strSql);
				
					return true;
				}
				catch(Exception err)
				{
					Msg = err.Message;

					log4net.ILog log = log4net.LogManager.GetLogger("AdjustData_DirClass.checkRight");
					if(log.IsErrorEnabled) log.Error(Msg,err);
				
					return false;
				}
				finally
				{
					dainc.Dispose();
				}
			}

			#endregion

			
		}
	}
}
