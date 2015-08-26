using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Text;
using System.Web.Services.Protocols;
using System.EnterpriseServices;
using System.Configuration;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.C2C.Finance.Common.CommLib;


namespace TENCENT.OSS.CFT.KF.KF_Service
{
	/// <summary>
	/// FinanceManage 的摘要说明。
	/// </summary>
	public class Finance_Manage : System.Web.Services.WebService
	{
		public Finance_Header myHeader;

		//static 字段用来保存一些需要的共用的状态
		public  static int j;           
		public  static int ErrorNo;     //默认为“未找到匹配数据的记录数”
		public  static int TypeOfErrorDetail; //对帐结果的三种表现形式，根据不同的值，跳转到不同的页面加以展现
		private static string BatchNo; //设置对帐任务执行时候的批次号
		private static string bankType; //银行类型
		private static long iMaxTaskID; 
		private static int serialNo;    //对帐中跟“对帐类型”一起唯一确定一种对帐情况的标识
	

		public Finance_Manage()
		{
			//CODEGEN: 该调用是 ASP.NET Web 服务设计器所必需的
			InitializeComponent();
		}     

		//根据SQL语句返回DataSet
		public DataSet getDataSet(string selectStr,int istr,int imax,string dbStr)
		{
			try
			{
				return QueryInfo.GetTable(selectStr,istr,imax,dbStr);
			}
			catch(Exception e)
			{
				throw new Exception("KF_Service-Finance_Manage:getDataSet Error! " + e.Message.ToString().Replace("'","’"));
			}
		}

		#region 组件设计器生成的代码
		
		//Web 服务设计器所必需的
		private IContainer components = null;
				
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion
		

		
		#region	用户信息查询和修改页面 2012/5/4 添加

		[WebMethod(Description="验证用户的QQ和姓名是否相符")]
		public bool CheckOldName(string qqid,string name,out string Msg)
		{
			Msg = null;
			qqid = qqid.Trim();
			name = name.Trim();

			string uName = null;

			//参数校验
			if (qqid == null || name == null)
			{
				Msg = "传入的参数不完整！";
				return false;
			}

			try
			{
				// TODO1: 客户信息资料外移
				//furion 20061116 email登录修改。
				string strID = PublicRes.ConvertToFuid(qqid);  //先转换成fuid

				/*
				//string selectStr = "Select FtrueName from " + PublicRes.GetTableName("t_user_info",qqid) + " where fqqid = '" + qqid + "'";
				string selectStr = "Select FtrueName from " + PublicRes.GetTName("t_user_info",strID) + " where fuid =" + strID;

				//uName = PublicRes.ExecuteOne(selectStr,"YW_30");	
				uName = PublicRes.ExecuteOne(selectStr,"ZL");	
				*/
				string selectStr = "uid=" + strID;
				uName = CommQuery.GetOneResultFromICE(selectStr,CommQuery.QUERY_USERINFO,"FtrueName",out Msg);
			}
			catch(Exception e)
			{
				Msg = "查询用户帐号和姓名是否一致失败！[" + e.Message.ToString().Replace("'","’") + "]";
				return false;
			}
			

			//检验是否注册
			if (uName == null || uName == "")
			{
				Msg = "对不起，该帐号没有注册！";
				return false;
			}

			//检验一致性
			if (uName.Trim() != name)
			{
				Msg = "对不起，该帐号注册的姓名和申诉的姓名不符合！";
				return false;
			}

			return true;
		}


		

		
		// 2012/5/4 将账务系统相关功能移动到KF系统
		[WebMethod(Description="修改姓名和公司名")]
		[SoapHeader("myHeader",Direction=SoapHeaderDirection.In)]
		public bool modifyName(string QQID,string changedName,string cCompany)  //传入修改后的姓名和公司名称
		{
			ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP,PublicRes.ICEPort);
			try
			{
				/*
				string companyStr;

				if (cCompany != null && cCompany != "")
					companyStr = "', Fcompany_name = '" + cCompany;
				else
					companyStr = "";

				// TODO1: 客户信息资料外移

				//furion 20061117 email登录修改
				string fuid = PublicRes.ConvertToFuid(QQID);
				//先同时修改3张表彰的姓名 t_user_info,t_user,t_bank_user 分别为表1，2，3
				//string strCmd1 = "update " + PublicRes.GetTableName("t_user_info",QQID) + " SET Ftruename = '" + changedName +  companyStr +"' where Fqqid ='" +  QQID + "'";  //+PublicRes.GetSqlFromQQ(QQID,"fuid");
				string strCmd1 = "update " + PublicRes.GetTName("t_user_info",fuid) + " SET Ftruename = '" + changedName +  companyStr +"' where Fuid =" +  fuid ;  //+PublicRes.GetSqlFromQQ(QQID,"fuid");
				string strCmd2 = "update " + PublicRes.GetTName("t_user",fuid)      + " SET Ftruename = '" + changedName +  companyStr +"' where Fuid =" +  fuid ;
				string strCmd3 = "update " + PublicRes.GetTName("t_bank_user",fuid) + " SET Ftruename = '" + changedName +  companyStr +"' where Fuid =" +  fuid ;
			
				ArrayList al = new ArrayList();
			
				al.Add(strCmd1);
				//al.Add(strCmd2);
				al.Add(strCmd3);
				
				//清除cache
				PublicRes.ReleaseCache(QQID,"qqid");
				*/


				string fuid = PublicRes.ConvertToFuid(QQID);
				string strSql = "uid=" + fuid;
				strSql += "&modify_time=" + PublicRes.strNowTimeStander;

				strSql += "&truename=" + changedName;

				if (cCompany != null && cCompany != "")
					strSql += "&company_name=" + cCompany;

				string errMsg = "";
				int iresult = CommQuery.ExecSqlFromICE(strSql,CommQuery.UPDATE_USERINFO,out errMsg);

				if(iresult != 1)
				{
					throw new LogicException("更新了非一条记录:" + errMsg);
				}

				//				strSql += "&curtype=1";   (t_fetch_bank 表中已经不存在 truename 和 company_name) 20110125 rowenawu
				//
				//				iresult = CommQuery.ExecSqlFromICE(strSql,CommQuery.UPDATE_BANKUSER,out errMsg);

				if(iresult != 1)
				{
					throw new LogicException("更新了非一条记录:" + errMsg);
				}

				//furion V30_FURION改动 20090310 改动核心时调ICE
				//if(PublicRes.ExecuteSqlNum(strCmd2,"YW_30") != 1)
				//return false;

				ice.OpenConn();
				string strwhere = "where=" + ICEAccess.URLEncode("fcurtype=1&");
				strwhere += ICEAccess.URLEncode("fuid=" + fuid + "&");

				string strUpdate = "data=" + ICEAccess.URLEncode("ftruename=" + ICEAccess.URLEncode(changedName));

				if (cCompany != null && cCompany != "")
					strUpdate += ICEAccess.URLEncode("&fcompany_name=" + ICEAccess.URLEncode(cCompany));

				strUpdate += ICEAccess.ICEEncode("&fmodify_time=" + PublicRes.strNowTimeStander);

				string strResp = "";

				//3.0接口测试需要 furion 20090708
				if(ice.InvokeQuery_Exec(YWSourceType.用户资源,YWCommandCode.修改用户信息,fuid,strwhere + "&" + strUpdate,out strResp) != 0)
				{
					throw new Exception("修改个人姓名出错！" + strResp);
				}

				//return PublicRes.Execute(al,"YW_30");  //执行并返回结果
				//return PublicRes.Execute(al,"ZL");  //执行并返回结果
				return true;
			}
			catch(Exception e)
			{
				ice.CloseConn();
				throw new Exception(e.Message.ToString().Replace("'","’"));
				return false;
			}
			finally
			{
				ice.Dispose();
			}
		}

        [WebMethod(Description = "返回用户的帐户类型")]
        public bool getUserType(string qqid, out string userType, out string Msg)
        {
            userType = null;
            Msg = null;

            if (qqid == null)
            {
                Msg = "传入的参数不完整！";
                return false;
            }

            try
            {
                string strID = PublicRes.ConvertToFuid(qqid);  //先转换成fuid

                /*
                //string str = "select Fuser_type from " + PublicRes.GetTName("t_user",strID) + " where fuid = '" + strID + "'";
                string str = "select Fuser_type from " + PublicRes.GetTName("t_user_info",strID) + " where fuid = '" + strID + "'";
                //userType = PublicRes.ExecuteOne(str,"YW_30");
                userType = PublicRes.ExecuteOne(str,"ZL");

                Msg = "获取用户帐户类型成功！";
                return true;
                */

                string strSql = "uid=" + strID;
                userType = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fuser_type", out Msg);

                if (userType != null && userType.Trim() != "")
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                Msg = "获取帐户类型失败！[" + e.Message.ToString().Replace("'", "’") + "]";
                return false;
            }
        }


		/// <summary>
		/// 密码取回参数
		/// </summary>
		/// <param name="qqid">用户的QQ号</param>
		/// <param name="mail">取回的邮箱</param>
		/// <param name="cleanMimao">是否清空密保</param>
		/// <param name="reason">取回的原因</param>
		/// <param name="pathUinfo">用户基本信息图片地址</param>
		/// <param name="AccInfo">用户帐户信息图片地址</param>
		/// <param name="IDCardInfo">身份证图片地址</param>
		/// <param name="BankCardInfo">银行卡图片地址</param>
		/// <returns></returns>
		[WebMethod(Description="密码取回")]
		[SoapHeader("myHeader", Direction=SoapHeaderDirection.In)]
		public bool changePwdInfo(string qqid,string mail,string cleanMimao,string reason,string pathUinfo,
			string AccInfo,string IDCardInfo,string BankCardInfo,out string Msg)
		{
			Msg = null;
			RightAndLog rl = new RightAndLog();
			try
			{
				if(myHeader == null)
				{
					throw new Exception("不正确的调用方法！");
				}

				//				rl.actionType = "密码取回";
				//				rl.ID = SPID;
				//				rl.OperID = myHeader.OperID;
				//				rl.sign = 1;
				//				rl.strRightCode = "ChangePassword";
				//				rl.RightString = myHeader.RightString;
				//				rl.SzKey = myHeader.SzKey;
				//				rl.type = "帐户密码修改";
				//				rl.UserID = myHeader.UserName;
				//				rl.UserIP = myHeader.UserIP;				
				//				if(!rl.CheckRight())
				//				{
				//					Msg = "用户权限不足，不能进行“密码取回操作”";
				//					return false;
				//				}

				//写业务表：用户执行了密码取回的操作。将来可以提供查询


				if (!PublicRes.ReleaseCache(qqid,"qqid"))
				{
					TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.sendLog4Log("FiannceManage.ModifyPwd"
						,"修改用户密码时，预清除用户"+ qqid + "cache失败！请检查！");
					return false;
				}

				//取回密码操作,参数同上
				bool pwdSign = FinanceManage.modifyPwd(qqid, mail,reason,cleanMimao, pathUinfo,AccInfo, 
					IDCardInfo, BankCardInfo,myHeader.UserName,myHeader.UserIP,out Msg);
				
				if (pwdSign == false)
				{
					//写本地日志
					PublicRes.WriteFile("修改密码（密保）失败！[" + qqid +"]" + Msg);
					//写失败的业务日志
					return false;
				}
				else if (pwdSign == true)
				{
					//写本地日志
					PublicRes.WriteFile("修改密码（密保）成功！[" + qqid +"]" + Msg);
					//写成功的业务日志
					

					return true;	
				}

				return true;
				
			}
			catch(Exception e)
			{
				Msg = "执行出错！[" + e.Message.ToString().Replace("'","’") + "]";
				return false;
			}

		}



		
		[WebMethod(Description="插入仲裁表相关的仲裁信息")]
		public bool insertMediation(string qqid,string mail,string cleanMimao,string reason,string uid,string fetchNO,string commTime,
			string pathAcc,string pathBase,string pathIDCard,string pathBank,string commitTime,string fnameNew,string FIDCardNew,out string Msg)
		{
			string fName  = null;
			string fbankNo= null;
			Msg = null;
 
			//获取相关的信息
			try
			{
				bool exeSign = getAccInfo(qqid,out fName,out fbankNo,out Msg);		
				if (exeSign == false)
				{
					return false;
				}
			}
			catch(Exception e)
			{
				Msg = Msg + "[" +e.Message.ToString().Replace("'","’") + "]";
				return false;
			}
			
			//插入数据库
			try
			{
				string insertStr = "Insert c2c_fmdb.t_mediation (FfetchID,FName,Fqqid,FbankNO,fCleanMibao,FfetchMail,Freason,FBasePath,FaccPath,FIDCardPath,FbankCardPath,FNameNew,FIDCardNew,FcommitTime,FlastModifyTime) VALUES ( '" 
					+ fetchNO + "','"
					+ fName   + "','"
					+ qqid   + "','"
					+ fbankNo + "','"
					+ cleanMimao + "','"
					+ mail    + "','"
					+ reason  + "','"
					+ pathBase+ "','"
					+ pathAcc + "','"
					+ pathIDCard + "','"
					+ pathBank   + "','"
					+ fnameNew   + "','"
					+ FIDCardNew + "','"
					+ commitTime + "',"
					+ PublicRes.strNowTime
					+")";
				PublicRes.ExecuteSql(insertStr,"ht");
				return true;
			}
			catch(Exception eStr)
			{
				string strMsg = "插入仲裁表错误失败！";
				Msg = strMsg + "[" + eStr.Message.ToString().Replace("'","’") + "]";
				return false;
			}	
		}
		



		[WebMethod(Description="根据QQ号获取姓名和银行帐号")]
		public bool getAccInfo(string qqid,out string name,out string bankNo,out string Msg)
		{
			name   = null;
			bankNo = null;
			Msg    = null;

			try
			{
				string fuid = PublicRes.ConvertToFuid(qqid);
				if(fuid == null)
					fuid = "0";

				/*
				// TODO1: 客户信息资料外移
				//furion 20061116 email登录修改
				//string str = "Select FtrueName,FbankID From " + PublicRes.GetTableName("t_bank_user",qqid) + " where fqqid = '" + qqid + "'";
				string str = "Select FtrueName,FbankID From " + PublicRes.GetTName("t_bank_user",fuid) + " where fuid =" + fuid ;

				*/
				string [] ar = new string[1];
				ar[0] = "FbankID";
				

				/*
				//ar = PublicRes.returnDrData(str,ar,"YW_30");
				ar = PublicRes.returnDrData(str,ar,"ZL");
				*/
				string strSql = "uid=" + fuid;
				strSql += "&curtype=1";

				ar = CommQuery.GetdrDataFromICE(strSql,CommQuery.QUERY_BANKUSER,ar,out Msg);

				string strSql1 = "uid=" + fuid;
				name = CommQuery.GetOneResultFromICE(strSql1,CommQuery.QUERY_USERINFO,"Ftruename",out Msg);

				if(ar!=null && ar.Length>0)
				{
					bankNo = ar[0].ToString().Trim();	
				}
				
				Msg = "获取姓名和银行帐号成功！";
				return true;
			}
			catch(Exception e)
			{
				Msg = "根据QQ号码获取姓名和银行帐号失败！" + e.Message.ToString().Replace("'","’");
				return false;
			}
		}




		[WebMethod(Description="检查用户是否注册或者作废")]
		public bool checkUserReg(string qqid,out string Msg)
		{
			Msg = null;

			try
			{
				//furion 20061115 email登录相关
				if(qqid == null || qqid.Trim().Length < 3)
				{
					Msg = "帐号不能为空";
					return false;
				}

				//start
				string qq = qqid.Trim();
				string uid3 = "";
				try
				{
					long itmp = long.Parse(qq);
					uid3 = qq;
				}
				catch
				{
					if(!Common.DESCode.GetEmailUid(qq,out uid3))
					{
						Msg = "解析EMAIL时出错。" + uid3;
						return false;
					}
				}
				//end	
				/*
				//如果用户注册了，但是处于作废状态，也不能够执行任何操作
				string ralationStr = "select Fsign from " + PublicRes.GetTName("t_relation",uid3) + " where Fqqid='" + qq +"'";
				
				//t_relation移到用户资料库。furion 20090302
				//string relaSign    = PublicRes.ExecuteOne(ralationStr,"YW_30");
				string relaSign    = PublicRes.ExecuteOne(ralationStr,"ZL");

				*/
						
				string strSql = "uin=" + qq;		
				string relaSign = CommQuery.GetOneResultFromICE(strSql,CommQuery.QUERY_RELATION,"Fsign",out Msg);

				if (relaSign == null || relaSign.Trim() == "")
				{
					Msg = "该帐号没有注册！";
					return false;
				}
 
				if (relaSign == "2")
				{
					Msg = "该帐户状态为作废状态，不能够进行任何操作！";
					return false;
				}
				else if (relaSign != "1")
				{
					Msg = "该账户状态标志("+ relaSign +")错误！请立即联系管理员察看！";
					return false;
				}

				/*
				string uidStr = "select Fuid from " + PublicRes.GetTName("t_relation",uid3) + " where Fqqid='" + qq +"'";
				
				//furion 20090302 t_relation移到用户资料库里。
				//Msg           = PublicRes.ExecuteOne(uidStr,"YW_30").Trim();
				Msg           = PublicRes.ExecuteOne(uidStr,"ZL").Trim();
				*/

				string errMsg = "";
				strSql = "uin=" + qq;		
				Msg = CommQuery.GetOneResultFromICE(strSql,CommQuery.QUERY_RELATION,"Fuid",out errMsg);

				return true;	
			}
			catch(Exception e)
			{
				Msg = "检查用户是否注册或者作废失败！" + PublicRes.replaceHtmlStr(e.Message.ToString());
				return false;
			}
		}

        [WebMethod(Description = "检查Uid是否存在 在userinfo中，不在t_relation中")]
        public bool CheckRecoverUid(string uid, string recoverQQid, out string Msg, out string type)
        {
            Msg = null;
            type = "";

            try
            {

                if (uid == null || uid.Trim().Length < 3)
                {
                    Msg = "帐号不能为空";
                    return false;
                }



                //				string errMsg = "";
                //				string strSql = "uid=" + uid;
                //				string qqid = CommQuery.GetOneResultFromICE(strSql,CommQuery.QUERY_USERINFO,"Fqqid",out errMsg);
                //
                //				if(qqid==null||qqid=="")
                //				{
                //					Msg="未查询到"+uid+"对应的QQId";
                //					return false;
                //				}

                //改为支持邮箱等 20111229
                string errMsg = "";
                string strSql = "uid=" + uid;
                DataSet ds = CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_USERINFO, out errMsg);

                string qqid_query = "";
                string mobile_query = "";
                string email_query = "";
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 1)
                {
                    Msg = "未查询到" + uid + "对应的QQId";
                    return false;
                }
                else
                {
                    qqid_query = ds.Tables[0].Rows[0]["Fqqid"].ToString();
                    mobile_query = ds.Tables[0].Rows[0]["fmobile"].ToString();
                    email_query = ds.Tables[0].Rows[0]["femail"].ToString();
                    if (qqid_query == recoverQQid ||
                        mobile_query == recoverQQid ||
                        email_query == recoverQQid)
                    {
                        if (mobile_query == recoverQQid && recoverQQid.Length == 11 && qqid_query == recoverQQid)
                        {
                            type = "mobile";
                            return true;
                        }
                        else if (email_query == recoverQQid && recoverQQid.IndexOf("@") > 0)
                        {
                            type = "email";
                            return true;
                        }
                        else if (qqid_query == recoverQQid)
                        {
                            type = "qq";
                            return true;
                        }
                        Msg = "需要恢复的帐号" + recoverQQid + "帐号类型未知！";
                        return false;
                    }
                    else
                    {
                        Msg = "需要恢复的帐号" + recoverQQid + "内部ID不为：" + uid;
                        return false;
                    }
                }


                //				string strSql = "uin=" + qqid;		
                //				string fuid = CommQuery.GetOneResultFromICE(strSql,CommQuery.QUERY_RELATION,"Fuid",out Msg);
                //
                //				if (fuid!=null&&fuid!="")
                //				{
                //					Msg=fuid+"已经存在RELATION表中！";
                //					return false;
                //				}

                return true;
            }
            catch (Exception e)
            {
                Msg = e.Message;
                return false;
            }
        }

		#endregion



		[WebMethod(Description="交易管理_交易单冻结管理")]
		[SoapHeader("myHeader",Direction=SoapHeaderDirection.In)] 
		public bool freezeTrade(string listID,string flstate) //传入交易单号和要修改的状态 1:锁定 2 正常 3 作废
		{
			//furion 20051227 客服系统只能锁定
			if(flstate != "1")
			{
				//throw new Exception("不支持此功能");
				//furion 20080604 现在可以了
			}

			if(myHeader == null)
			{
				throw new Exception("不正确的调用方法！");
			}
			string strUserID = myHeader.UserName;
			string strIP = myHeader.UserIP;
			string strRightCode = "ModifyPayList";
			//首先根据listID分别取到买家和卖家的交易表名
			ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP,PublicRes.ICEPort);
			try
			{ 
				// TODO1: furion 数据库优化 20080111
				//furion need ****20070509 定单是否需要冻结
				//如果交易单本身为作废状态，不允许进行冻结管理操作
				//				string strLst = "select flstate from " + PublicRes.GetTName("t_tran_list",listID) + " WHERE FlistID = '" + listID +"'";
				//				string lstSign= PublicRes.ExecuteOne(strLst,"yw_30");
				
				/*
				string strLst = "select flstate from " + PublicRes.GetTName("t_order",listID) + " WHERE FlistID = '" + listID +"'";
				string lstSign= PublicRes.ExecuteOne(strLst,"ZJ");
				*/

				string errMsg = "";
				string strLst = "listid=" + listID;
				string lstSign = CommQuery.GetOneResultFromICE(strLst,CommQuery.QUERY_ORDER,"flstate",out errMsg);

				if (lstSign == "3") //作废状态
				{
					throw new Exception("交易单作废状态，不允许进行冻结管理操作!");
					return false;
				}


				//注意要同时更新三个处表的交易单的状态 1:锁定 2 正常 3 作废  fbuy_uid/fsale_uid
				//				string updateStr1 = "UPDATE " + PublicRes.GetTName("t_tran_list",listID) + "     SET flstate = '" + flstate + "' WHERE FlistID = '" + listID +"'";   //总表
				//				string updateStr2 = "UPDATE " + PublicRes.getTableNameFromLsd("fbuy_uid",listID, "t_pay_list") + "  SET flstate = '" + flstate + "' WHERE FlistID = '" + listID +"'";      //买家分库分表
				//				string updateStr3 = "UPDATE " + PublicRes.getTableNameFromLsd("fsale_uid",listID,"t_pay_list") + "  SET flstate = '" + flstate + "' WHERE FlistID = '" + listID +"'";      //卖家分库分表

				//				string strSelectBuyID  = "SELECT fbuy_uid  FROM " + PublicRes.GetTName("t_tran_list",listID) + " WHERE flistid ='" + listID + "'" ;
				//				string buyuid = PublicRes.ExecuteOne(strSelectBuyID,"yw_30");
				//
				//				strSelectBuyID  = "SELECT fsale_uid  FROM " + PublicRes.GetTName("t_tran_list",listID) + " WHERE flistid ='" + listID + "'" ;
				//				string saleuid = PublicRes.ExecuteOne(strSelectBuyID,"yw_30");

				/*
				string strSelectBuyID  = "SELECT fbuy_uid  FROM " + PublicRes.GetTName("t_order",listID) + " WHERE flistid ='" + listID + "'" ;
				string buyuid = PublicRes.ExecuteOne(strSelectBuyID,"ZJ");

				strSelectBuyID  = "SELECT fsale_uid  FROM " + PublicRes.GetTName("t_order",listID) + " WHERE flistid ='" + listID + "'" ;
				string saleuid = PublicRes.ExecuteOne(strSelectBuyID,"ZJ");
				*/

				string sqllst = "listid=" + listID;
				string buyuid = CommQuery.GetOneResultFromICE(sqllst,CommQuery.QUERY_ORDER,"fbuy_uid",out errMsg);
				string saleuid = CommQuery.GetOneResultFromICE(sqllst,CommQuery.QUERY_ORDER,"fsale_uid",out errMsg);

				//				string updateStr1 = "UPDATE " + PublicRes.GetTName("t_tran_list",listID) + "     SET flstate = '" + flstate + "',Fmodify_time=now() WHERE FlistID = '" + listID +"'";   //总表
				//				string updateStr2 = "UPDATE " + PublicRes.GetTName("t_pay_list",buyuid) + "  SET flstate = '" + flstate + "',Fmodify_time=now() WHERE FlistID = '" + listID +"'";      //买家分库分表
				//				string updateStr3 = "UPDATE " + PublicRes.GetTName("t_pay_list",saleuid) + "  SET flstate = '" + flstate + "',Fmodify_time=now() WHERE FlistID = '" + listID +"'";      //卖家分库分表
			
				//				ArrayList al = new ArrayList();
				//				al.Add(updateStr1);
				//al.Add(updateStr2);
				//al.Add(updateStr3);
           
				//PublicRes.Execute(al,"yw_30"); //事务执行

				ice.OpenConn();
				string strwhere = "where=" + ICEAccess.URLEncode("flistid=" + listID + "&fcurtype=1&");

				string strUpdate = "data=" + ICEAccess.URLEncode("flstate=" + flstate);
				strUpdate += ICEAccess.URLEncode("&fmodify_time=" + PublicRes.strNowTimeStander);

				string strResp = "";

				//3.0接口测试需要 furion 20090708
				int iresult = ice.InvokeQuery_Exec(YWSourceType.交易单资源,YWCommandCode.修改交易单信息,listID,strwhere + "&" + strUpdate,out strResp);
				if(iresult != 0 && iresult !=60120101)
				{
					throw new Exception("锁定交易单时出错！" + strResp);	
				}


				/*
				
				//furion 同时更新定单,不判断影响条数.因为有可能不存在定单.
				string updateStr1 = "UPDATE " + PublicRes.GetTName("t_order",listID) + "     SET flstate = '" + flstate + "',Fmodify_time=now() WHERE FlistID = '" + listID +"'";   //总表
				string updateStr2 = "UPDATE " + PublicRes.GetTName("t_user_order",buyuid) + "  SET flstate = '" + flstate + "',Fmodify_time=now() WHERE FlistID = '" + listID +"'";      //买家分库分表
				string updateStr3 = "UPDATE " + PublicRes.GetTName("t_user_order",saleuid) + "  SET flstate = '" + flstate + "',Fmodify_time=now() WHERE FlistID = '" + listID +"'";      //卖家分库分表

				updateStr1 += " and fbuy_uid='" + buyuid + "' and fsale_uid='" + saleuid + "' ";
				//furion 20081015 修改买卖家订单,现在也不一定存在了.因为没判断影响函数,不用处理. 
				PublicRes.ExecuteSql(updateStr1,"ZJ");
				//PublicRes.ExecuteSql(updateStr2,"BS");
				//PublicRes.ExecuteSql(updateStr3,"BS");
				*/

				string inmsg = "MSG_NO=" + listID + flstate;
				inmsg += "&transaction_id=" + listID;
				inmsg += "&flstate=" + flstate;
				inmsg += "&fmodify_time=" + PublicRes.strNowTimeStander;

				string reply;
				short sresult;
				string Msg = "";

				if(commRes.middleInvoke("order_update_service",inmsg,true,out reply,out sresult,out Msg))
				{
					if(sresult != 0)
					{
						Msg =  "order_update_service接口失败：result=" + sresult + "，msg=" + Msg + "&reply=" + reply;
						throw new LogicException(Msg);
					}
					else
					{
						if(reply.IndexOf("result=0") > -1)
						{
						}
						else
						{
							Msg =  "order_update_service接口失败：result=" + sresult + "，msg=" + Msg + "&reply=" + reply;
							throw new LogicException(Msg);
						}
					}
				}
				else
				{
					Msg = "order_update_service接口失败：result=" + sresult + "，msg=" + Msg + "&reply=" + reply;
					throw new LogicException(Msg);
				}

				PublicRes.writeSysLog(strUserID,strIP,"dj","交易解冻",1,listID,"状态："+flstate);

				return true;
			}	
			catch(Exception e)
			{
				throw new Exception("交易单冻结错误!" + e.Message.Replace("'","’"));
				return false;
			}
		}



		[WebMethod(Description="返回当前的static字段值")]  
		[SoapHeader("myHeader",Direction=SoapHeaderDirection.In)]
		public string [] returnStaticStr()
		{
			try
			{
				//返回静态字段值，供程序调用，反映当前的状态值
				string [] ar = new string[3];  //可扩充
				ar[0] = BatchNo; //设置对帐任务执行时候的批次号
				ar[1] = TypeOfErrorDetail.ToString(); //对帐结果的三种表现形式，根据不同的值，跳转到不同的页面加以展现
				ar[2] = ErrorNo.ToString();

				return ar;
			}
			catch(Exception e)
			{
				//throw new Exception("KF_Service-Insert_Service:AdjustTradeRequest Error! " + e.Message.ToString().Replace("'","’"));
				throw new Exception("Service调用出错，请联系管理员");
				return null;
			}
		}
		
	

		[WebMethod(Description="冻结账户")]
		[SoapHeader("myHeader",Direction=SoapHeaderDirection.In)]
		public bool freezeAccount(string uid,int type)  
		{
			try
			{
				string freezeStr=null;

				//furion 20070119
				string fuid = PublicRes.ConvertToFuid(uid);
			
				string errMsg = "";
				string strSql = "uid=" + fuid;		
				strSql += "&modify_time=" + PublicRes.strNowTimeStander;
				strSql += "&curtype=1";
				if (type == 1)  //传入正常，需要冻结
				{
					strSql += "&state=2";
				}
				else if (type == 2)  //传入冻结 需要解冻 正常化
				{
					strSql += "&state=1";
				}

				//int iresult = CommQuery.ExecSqlFromICE(strSql,CommQuery.UPDATE_BANKUSER,out errMsg);
				CommQuery.ExecSqlFromICE(strSql,CommQuery.UPDATE_BANKUSER,out errMsg);

//				if(iresult != 1)
//				{
//					throw new LogicException("更新了非一条记录:" + errMsg);
//				}

				//清除cache
				PublicRes.ReleaseCache(uid,"qqid");
				return true;

				/*
				// TODO1: 客户信息资料外移
				if (type == 1)  //传入正常，需要冻结
				{
					//freezeStr = "UPDATE " + PublicRes.GetTableName("t_bank_user",uid) + " SET fstate = 2 WHERE fqqid=" + uid;   //1 正常 2 冻结
					freezeStr = "UPDATE " + PublicRes.GetTName("t_bank_user",fuid) + " SET fstate = 2 WHERE fuid=" + fuid;   //1 正常 2 冻结
				}
				else if (type == 2)  //传入冻结 需要解冻 正常化
				{
					//freezeStr = "UPDATE " + PublicRes.GetTableName("t_bank_user",uid) + " SET fstate = 1 WHERE fqqid=" + uid;   //1 正常 2 冻结
					freezeStr = "UPDATE " + PublicRes.GetTName("t_bank_user",fuid) + " SET fstate = 1 WHERE fuid=" + fuid;   //1 正常 2 冻结
				}

				//清除cache
				PublicRes.ReleaseCache(uid,"qqid");

				//return PublicRes.ExecuteSql(freezeStr,"YW_30");
				return PublicRes.ExecuteSql(freezeStr,"ZL");
				*/
			}	
			catch(Exception e)
			{
				//throw new Exception("Finance_Service-Insert_Service:AdjustTradeRequest Error! " + e.Message.ToString().Replace("'","’"));
				throw new Exception("冻结账户出错！" + e.Message.ToString().Replace("'","’"));
				return false;		
			}
		}

		//freezePerAccount
		[WebMethod(Description="冻结个人账户")]
		[SoapHeader("myHeader",Direction=SoapHeaderDirection.In)]
        public bool freezePerAccount(string uid, int type, string username)
		{
			string mediflag = "false";

			if(myHeader == null)
			{
				throw new Exception("不正确的调用方法！");
			}
			string strUserID = myHeader.UserName;
			string strIP = myHeader.UserIP;
			
			//MySqlAccess dazl = new MySqlAccess(PublicRes.GetConnString("ZL"));
			ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP,PublicRes.ICEPort);
			try
			{
				//dazl.OpenConn();
				//dazl.StartTran();
			
				//furion 20060331 先判断用户是否是商户.
				if(mediflag.ToLower() == "false")
				{
					/*
					// TODO1: 客户信息资料外移
					//int itmp = Int32.Parse(PublicRes.ExecuteOne("select count(*) from c2c_db.t_merchant_info where Fspecial='" + uid + "' ","YW_30"));
					//int itmp = Int32.Parse(PublicRes.ExecuteOne("select count(*) from c2c_db.t_merchant_info where Fspecial='" + uid + "' ","ZL"));
					int itmp = Int32.Parse(dazl.GetOneResult("select count(*) from c2c_db.t_merchant_info where Fspecial='" + uid + "' "));
					
					if(itmp > 0)
					{
						throw new LogicException("不允许冻结或解冻商户所绑定的QQ号码");
					}
					*/

					//新增判断，29813和 10***@mch.tenpay.com类的不允许
					if(uid.StartsWith("29813"))
					{
						throw new LogicException("不允许冻结或解冻内部使用账号");
					}

					if(uid.StartsWith("10") && uid.ToLower().EndsWith("@mch.tenpay.com"))
					{
						throw new LogicException("不允许冻结或解冻内部使用账号");
					}

					string Msg = "";
					string selectStr = "special=" + uid;
					string Fspecial = CommQuery.GetOneResultFromICE(selectStr,CommQuery.QUERY_MERCHANTINFO,"Fspecial",out Msg);

					if(Fspecial != null && Fspecial.Trim() != "")
					{
						throw new LogicException("不允许冻结或解冻商户所绑定的QQ号码");
					}
					
					
				}

				//furion 20061116 email登录相关
				string fuid = PublicRes.ConvertToFuid(uid);

				if(fuid == null || fuid.Length<3)
				{
					throw new Exception("冻结解冻个人账户出错！内部ＩＤ不正确。" );
				}

				string strSql = "uid=" + fuid;
				strSql += "&modify_time=" + PublicRes.strNowTimeStander;
					
				int newtype = 0;

				if (type == 1)  //传入正常，需要冻结
				{
					strSql += "&state=2";
					newtype = 2;
				}
				else if (type == 2)  //传入冻结 需要解冻 正常化
				{				
					strSql += "&state=1";
					newtype = 1;
				}

                //echo 20141211 解冻ui_common_update_service接口全部换成ui_unfreeze_user_service
                if (type == 1)//冻结走之前的流程
                {
                    string errMsg = "";
                    int iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERINFO, out errMsg);

                    if (iresult != 1)
                    {
                        throw new LogicException("更新了非一条记录:" + errMsg);
                    }

                    strSql += "&curtype=1";

                    //iresult = 
                    CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_BANKUSER, out errMsg);

                }
                else if (type == 2) //解冻ui_common_update_service接口换成ui_unfreeze_user_service
                {
                    string errMsg = "";
                    string sql = "uid=" + fuid;
                    string fcredit = CommQuery.GetOneResultFromICE(sql, CommQuery.QUERY_USERINFO, "Fcreid", out errMsg);
                    string fcretype = CommQuery.GetOneResultFromICE(sql, CommQuery.QUERY_USERINFO, "Fcre_type", out errMsg);

                    string Msg = "";
                    string req = "uin=" + uid + "&cre_id=" + fcredit + "&cre_type=" + fcretype + "&caller=" + myHeader.UserName;
                    req += "&source=2&client_ip=" + myHeader.UserIP + "&name=" + username + "&modify_time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    req += "&op_id=" + myHeader.OperID + "&op_name=" + myHeader.UserName;
                    CommQuery.GetDSForServiceFromICE(req, "ui_unfreeze_user_service", true, out Msg);
                    if (Msg != "")
                    {
                        throw new Exception("调ui_unfreeze_user_service解冻异常："+Msg);
                    }
                }


//				if(iresult != 1)
//				{
//					throw new LogicException("更新了非一条记录:" + errMsg);
//				}

				/*
				string freezeStr1 = "";
				string freezeStr2 = "";
				int newtype = 0;

				if (type == 1)  //传入正常，需要冻结
				{
					newtype = 2;
				}
				else if (type == 2)  //传入冻结 需要解冻 正常化
				{				
					newtype = 1;
				}

				freezeStr1 = "UPDATE " + PublicRes.GetTName("t_user_info",fuid) + " SET fstate = " + newtype + " WHERE fuid=" + fuid;   //1 正常 2 冻结
				freezeStr2 = "UPDATE " + PublicRes.GetTName("t_bank_user",fuid) + " SET fstate = " + newtype + " WHERE fuid=" + fuid;   //1 正常 2 冻结

				//清除cache
				PublicRes.ReleaseCache(uid,"qqid");

				if(dazl.ExecSqlNum(freezeStr1) != 1)
				{
					throw new Exception("冻结解冻个人账户出错！更新t_user_info出错。" );
				}

				if(dazl.ExecSqlNum(freezeStr2) != 1)
				{
					throw new Exception("冻结解冻个人账户出错！更新t_bank_user出错。" );
				}
				
				

				//return PublicRes.ExecuteSql(freezeStr,"YW_30");

				//furion 20090304 现在的冻结需要冻三个地方，一个在核心，还有t_bank_user/t_user_info
				//核心t_user的冻结属于资金止付的冻结，外围是真正的冻结。
				//需要先冻结外围再处理核心。
				dazl.Commit();
*/
				ice.OpenConn();
				string strwhere = "where=" + ICEAccess.URLEncode("fcurtype=1&");
				strwhere += ICEAccess.URLEncode("fuid=" + fuid + "&");
                strwhere += ICEAccess.URLEncode("fstate=" + type + "&");

				string strUpdate = "data=" + ICEAccess.URLEncode("fstate=" + newtype);
				strUpdate += ICEAccess.URLEncode("&fmodify_time=" + PublicRes.strNowTimeStander);

				string strResp = "";

				//3.0接口测试需要 furion 20090708
				if(ice.InvokeQuery_Exec(YWSourceType.用户资源,YWCommandCode.修改用户信息,fuid,strwhere + "&" + strUpdate,out strResp) != 0)
				{
					throw new Exception("冻结解冻个人账户出错！" + strResp);
				}

				return true;
			}	
			catch(Exception e)
			{
				ice.CloseConn();
				//dazl.RollBack();
				//throw new Exception("Finance_Service-Insert_Service:AdjustTradeRequest Error! " + e.Message.ToString().Replace("'","’"));
				throw new Exception("冻结个人账户出错！" + e.Message.ToString().Replace("'","’"));
				return false;		
			}
			finally
			{
				ice.Dispose();
				//dazl.Dispose();
				try
				{
					PublicRes.writeSysLog(strUserID,strIP,"dj","冻结个人账户",1,uid,"");	
				}
				catch
				{
					throw new Exception("写日志失败！");
				}
			}	
		}

        //add by yinhuang 2013/8/15
        [WebMethod(Description = "冻结个人账户wechat")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool FreezePerAccountWechat(string uid, int type)
        {
            string mediflag = "false";

            if (myHeader == null)
            {
                throw new Exception("不正确的调用方法！");
            }
            string strUserID = myHeader.UserName;
            string strIP = myHeader.UserIP;

            try
            {
                //furion 20060331 先判断用户是否是商户.
                if (mediflag.ToLower() == "false")
                {     
                    //新增判断，29813和 10***@mch.tenpay.com类的不允许
                    if (uid.StartsWith("29813"))
                    {
                        throw new LogicException("不允许冻结或解冻内部使用账号");
                    }

                    if (uid.StartsWith("10") && uid.ToLower().EndsWith("@mch.tenpay.com"))
                    {
                        throw new LogicException("不允许冻结或解冻内部使用账号");
                    }

                    string Msg = "";
                    string selectStr = "special=" + uid;
                    string Fspecial = CommQuery.GetOneResultFromICE(selectStr, CommQuery.QUERY_MERCHANTINFO, "Fspecial", out Msg);

                    if (Fspecial != null && Fspecial.Trim() != "")
                    {
                        throw new LogicException("不允许冻结或解冻商户所绑定的QQ号码");
                    }
                }

                //furion 20061116 email登录相关
                string fuid = PublicRes.ConvertToFuid(uid);

                if (fuid == null || fuid.Length < 3)
                {
                    throw new Exception("冻结解冻个人账户出错！内部ＩＤ不正确。");
                }

                string strSql = "uid=" + fuid;
                strSql += "&modify_time=" + PublicRes.strNowTimeStander;

                if (type == 1)  //传入正常，需要冻结
                {
                    strSql += "&state=2";
                }
                else if (type == 2)  //传入冻结 需要解冻 正常化
                {
                    strSql += "&state=1";
                }

                string errMsg = "";
                int iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERINFO, out errMsg);

                if (iresult != 1)
                {
                    throw new LogicException("更新了非一条记录:" + errMsg);
                }

                return true;
            }
            catch (Exception e)
            {
                throw new Exception("冻结个人账户wechat出错！" + e.Message.ToString().Replace("'", "’"));
                return false;
            }
            finally
            {
                try
                {
                    PublicRes.writeSysLog(strUserID, strIP, "dj", "冻结个人账户", 1, uid, "");
                }
                catch
                {
                    throw new Exception("写日志失败！");
                }
            }
        }

        [WebMethod(Description = "冻结个人账户webchat_new")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool FreezePerAccountWechat_New(string uin, string username, string channel) 
        {
            if (myHeader == null)
            {
                throw new Exception("不正确的调用方法！");
            }

            try
            {
                if (string.IsNullOrEmpty(uin)) 
                {
                    throw new Exception("账号为空");
                }
                uin = uin.Trim();
                string uid = PublicRes.ConvertToFuid(uin);
                if (uid == null || uid.Length < 3)
                {
                    throw new Exception(uin + "账号不存在");
                }
                string errMsg = "";
                string strSql = "uid=" + uid;
                string fcredit = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fcreid", out errMsg);
                string fcretype = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fcre_type", out errMsg);
                string Msg = "";
                //client_ip,modify_time;name;caller uin source=2 watch_word cre_id
                string req = "uin=" + uin + "&cre_id=" + fcredit + "&cre_type=" + fcretype + "&caller=" + myHeader.UserName;
                req += "&source=2&client_ip=" + myHeader.UserIP + "&channel=" + channel + "&name=" + username + "&modify_time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                CommQuery.GetDSForServiceFromICE(req, "ui_freeze_user_service", true, out Msg);
                if (Msg != "") {
                    throw new Exception(Msg);
                }

                return true;
            }
            catch (Exception e) 
            {
                throw new LogicException("service处理失败:" + e.Message);
                return false;
            }
        }

        [WebMethod(Description = "解冻个人账户webchat_new")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool UnFreezePerAccountWechat_New(string uin, string username)
        {
            if (myHeader == null)
            {
                throw new Exception("不正确的调用方法！");
            }

            try
            {
                if (string.IsNullOrEmpty(uin))
                {
                    throw new Exception("账号为空");
                }
                uin = uin.Trim();
                string uid = PublicRes.ConvertToFuid(uin);
                if (uid == null || uid.Length < 3)
                {
                    throw new Exception(uin + "账号不存在");
                }
                string errMsg = "";
                string strSql = "uid=" + uid;
                string fcredit = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fcreid", out errMsg);
                string fcretype = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fcre_type", out errMsg);

                string Msg = "";
                //client_ip,modify_time;name;caller uin source=2 watch_word cre_id
                string req = "uin=" + uin + "&cre_id=" + fcredit + "&cre_type=" + fcretype + "&caller=" + myHeader.UserName;
                req += "&source=2&client_ip=" + myHeader.UserIP + "&name=" + username + "&modify_time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                CommQuery.GetDSForServiceFromICE(req, "ui_unfreeze_user_service", true, out Msg);
                if (Msg != "")
                {
                    throw new Exception(Msg);
                }

                return true;
            }
            catch (Exception e)
            {
                throw new LogicException("service处理失败:" + e.Message);
                return false;
            }
        }

        //[WebMethod(Description = "付款单ID到交易单ID")] //2015-8-11 改接口 v_yqyqguo
        //public string TdeToID(string tdeid)  //传入付款单ID
        //{
        //    try
        //    {
        //        //先同时修改3张表彰的姓名 t_user_info,t_user,t_bank_user 分别为表1，2，3	
        //        string tmp = PublicRes.ExecuteOne("select Flistid from c2c_db.t_tcpay_list where ftde_id='" + tdeid + "' and Fsubject<>4 " ,"ywb");  //执行并返回结果
        //        if(tmp == null || tmp.ToString().Trim() == "")
        //        {
        //            tmp = PublicRes.ExecuteOne("select Flistid from c2c_db.t_refund_list where Frlistid = (select FListID from c2c_db.t_tcpay_list where ftde_id='" + tdeid + "' and Fsubject=4 )" ,"ywb");  //执行并返回结果
        //            //return "0";
        //        }
        //        return tmp.ToString().Trim();
        //    }
        //    catch(Exception e)
        //    {
        //        throw new Exception(e.Message.ToString().Replace("'","’"));
        //        return "0";
        //    }
        //}

		/// <summary>
		/// 根据传入的交易单数组，在帐务系统中取到审批交易单列表
		/// </summary>
		/// <param name="ar">交易单数组</param>
		/// <param name="time">帐务时间（YYYYMMDD），用来确定数据库</param>
		/// <returns>返回含数据的交易单信息的DataSet</returns>
		[WebMethod(Description="根据交易单号获取到审批明细单")]
		public DataSet dsReBatCheck(string [] ar,string time)
		{
			try
			{
				StringBuilder sb = new StringBuilder("");
				foreach(string s in ar)
				{
					sb.Append("Fbank_list=");
					sb.Append("'"+s+"'");
					sb.Append(" || ");
				}

				int n = sb.Length;
				string lstStr = sb.ToString().Trim().Substring(0,n-3);  //去处最后的||；

				string bankrollTable = "c2c_zwdb_" + time + ".t_bankroll_result ";
				//察看最新状态
				string selectStr = "Select * from (Select * from " + bankrollTable + "order by FbatchNO DESC) A where " + lstStr + " group by Fbank_list";
				
				return PublicRes.returnDSAll(selectStr,"zw");
			}
			catch(Exception e)
			{
				throw new Exception("通过交易单号获取到审批明细单错误[" + e.Message.ToString().Replace("'","’") + "]");
			}
		}
		
		[WebMethod(Description="销户历史查询")]
		[SoapHeader("myHeader", Direction=SoapHeaderDirection.In)]
		public DataSet logOnUserHistory(DateTime startDate,DateTime endDate,string qqid,string handid,int startIndex,int lenth,out string Msg)
		{
			Msg = null;
			string whereStr = " where 1=1 ";
			
			//格式化时间
			string strBgDateTime;
			string strEdDateTime;
 
			try
			{
				strBgDateTime = startDate.ToString("yyyy-MM-dd 00:00:00");
				strEdDateTime = endDate.ToString("yyyy-MM-dd 23:59:59");		
			}
			catch
			{
				Msg = "销户历史查询时间不正确！请检查！";
				return null;
			}
		


			//查询
			if (strBgDateTime != null && strBgDateTime.Trim() != "" && strEdDateTime != null && strEdDateTime.Trim() != "")
			{
				whereStr += " and flastModifyTime >= '" +strBgDateTime + "' and flastModifyTime <= '" +strEdDateTime + "'";
			}

			if (qqid != null && qqid.Trim() != "")
			{
				whereStr += " and fqqid = '" + qqid + "' ";
			}

			if (handid != null && handid.Trim() != "")
			{
				whereStr += " and handid = '" + handid + "' ";
			}

			//furion 20061122 对下面语句改一下.
			//得到总记录数
			//string fstrSql_count = "select *   from  c2c_fmdb.t_logon_history " + " " + whereStr;
			//string fstrSql_count = "select count(*)   from  c2c_fmdb.t_logon_history " + " " + whereStr;
    		//DataSet dsTmp = PublicRes.returnDSAll(fstrSql_count,"ht");
			//int count =  int.Parse(dsTmp.Tables[0].Rows.Count.ToString());
			int count =  10000;//int.Parse(dsTmp.Tables[0].Rows[0][0].ToString());

			string str = "select Fid, Fqqid, Fquid, Freason, handid, handip, FlastModifyTime,  " + count + " as icount from c2c_fmdb.t_logon_history " + whereStr + " order by fid DESC limit " + startIndex + "," + lenth; 
			return PublicRes.returnDSAll(str,"ht");
		}


		/// <summary>
		/// 销户功能删除用户对应关系表，记录历史即可。这样核心系统便不会去找到该用户的相关资料。（06.02.20与richard确认 rayguo）
		/// 以后如果想查询被销户的帐户的相关信息。则提供一个按找内部ID查询的功能即可从后台帐务系统中获取相关需要的信息。
		/// </summary>
		/// <param name="qqid"></param>
		/// <returns></returns>
		[WebMethod(Description="销户功能")]
		[SoapHeader("myHeader", Direction=SoapHeaderDirection.In)]
		public bool logOnUser(string qqid,string reason,string user, string userIP,out string Msg)
		{
			Msg = null;
			//MySqlAccess da = null;
			MySqlAccess fmda = null;
			
			//2.0直接注释了，这里有风险，为了并跑，先也暂时注释 furion 20091225
			/*
			if(myHeader == null)
			{
				throw new Exception("不正确的调用方法！");
			}
			
			RightAndLog rl = new RightAndLog();
			rl.actionType = "销户功能";
			rl.ID = qqid;
			rl.OperID = myHeader.OperID;
			rl.sign = 1;
			rl.strRightCode = "logonUser";
			rl.RightString = myHeader.RightString;
			rl.SzKey = myHeader.SzKey;
			rl.type = "销户功能";
			rl.UserID = myHeader.UserName;
			rl.UserIP = myHeader.UserIP;				
			if(!rl.CheckRight())
			{
				throw new LogicException("用户无权执行此操作！");
			}		
			*/

			//da   = new MySqlAccess(PublicRes.GetConnString("YW_30"));
			fmda = new MySqlAccess(PublicRes.GetConnString("ht"));

			//MySqlAccess da_zl = new MySqlAccess(PublicRes.GetConnString("ZL"));
			try
			{				
				//furion 20080522 删除实名认证.
				string inmsg = "uin=" + qqid;
				inmsg += "&operator=" + user;
				inmsg += "&memo=帐务销户删除" ;

				string reply;
				short sresult;

				//3.0接口测试需要 furion 20090708
				if(commRes.middleInvoke("au_del_authen_service",inmsg,true,out reply,out sresult,out Msg))
				{
					if(sresult != 0)
					{
						Msg =  "au_del_authen_service接口失败：result=" + sresult + "，msg=" + Msg + "&reply=" + reply;
						return false;
					}
					else
					{
						if(reply.IndexOf("result=0") > -1)
						{
						}
						else if(reply.IndexOf("result=22520101") > -1)
						{
						}
						else
						{
							Msg =  "au_del_authen_service接口失败：result=" + sresult + "，msg=" + Msg + "&reply=" + reply;
							return false;
						}
					}
				}
				else
				{
					Msg = "au_del_authen_service接口失败：result=" + sresult + "，msg=" + Msg + "&reply=" + reply;
					return false;
				}

				//end 删除实名认证
				//da.OpenConn();
				//da.StartTran();

				//da_zl.OpenConn();
				//da_zl.StartTran();

				fmda.OpenConn();
				fmda.StartTran();

				//销户判断。如果用户有未结束的交易或者是由余额，冻结余额等，不允许操作。 目前做最简单金额判断
				string uid = PublicRes.ConvertToFuid(qqid);
				if (uid == null || uid.Trim() == "" || uid.Trim() == "0")
				{
					Msg = "获取QQ" + qqid + "号码对应的UID失败!"; 
					//return false;
					//furion 20070130 如果是已销户的,就返回真,这样可重复调用.
					return true;
				}

				//				string str = "select fbalance,fcon from " + PublicRes.GetTName("t_user",uid) + " where fuid = '" + uid + "'";
				//				DataTable dt = PublicRes.returnDSAll(str,"YW_30").Tables[0];
				//				
				//				if (dt != null && dt.Rows.Count !=0 && dt.Rows[0]["fbalance"] != null && dt.Rows[0]["fcon"] != null)
				//				{
				//					string banlance = dt.Rows[0]["fbalance"].ToString().Trim();
				//					string fcon     = dt.Rows[0]["fcon"].ToString().Trim();
				//
				////					if (banlance != "0" || fcon != "0")  //取消判断。用户销户审批通过后，即可以进行销户。
				////					{
				////						Msg = "销户帐户" + qqid + "的余额[" + banlance + "]或者冻结余额[" + fcon + "]不为０.不允许进行销户操作．";
				////						commRes.sendLog4Log("Finance_Manage.logOnUser",Msg);
				////						return false;
				////					}	
				// 				}


				//插入历史备份数据库
				string nowTime = PublicRes.strNowTimeStander;
				string insertStr = "insert into c2c_fmdb.t_logon_history (fqqid,fquid,freason,handid,handip,flastMOdifyTime) values ('" 
					+ qqid + "','" + PublicRes.ConvertToFuid(qqid) + "','" + commRes.replaceSqlStr(reason) + "','" + user + "','" + userIP + "','" + nowTime + "')";
				if (!fmda.ExecSql(insertStr))
				{
					//da.RollBack();
					fmda.RollBack();

					Msg = "销户时插入历史备份数据库出错。";
					commRes.sendLog4Log("FinanceManage.LogOnUser",Msg);
					return false;
				}

				/*
				// TODO1: 客户信息资料外移
				//furion 20070206 如果销户,就全消,所以要查询出来所有的绑定的帐号.
				string strSql = "select ifnull(fqqid,''),ifnull(Femail,''),ifnull(Fmobile,'') from " 
					+ PublicRes.GetTName("t_user_info",uid) + " where fuid= '" + uid + "'";
				//DataSet ds = da.dsGetTotalData(strSql);
				DataSet ds = da_zl.dsGetTotalData(strSql);
				
				*/

				string strSql = "uid=" + uid;
				DataSet ds = CommQuery.GetDataSetFromICE(strSql,CommQuery.QUERY_USERINFO,out Msg);

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 1)
				{
					fmda.RollBack();
					Msg = "获取用户资料时出错." + Msg;
					return false;
				}
				
				string strtmp = QueryInfo.GetString( ds.Tables[0].Rows[0]["fqqid"]);
				if(strtmp != "")
				{
					string fuid = PublicRes.ConvertToFuid(strtmp);
					if(fuid != null && fuid == uid)
					{
						/*
						//清空QQ号对应的对应关系表
						string delStr = "delete from " + PublicRes.GetTName("t_relation",strtmp) + " where fqqid ='" + strtmp + "'";

						//furion 20090302 t_relation移到用户资料库里。
						//int i = da.ExecSqlNum(delStr);
						int i = da_zl.ExecSqlNum(delStr);
						*/

						strSql = "uin=" + strtmp;		
						int i = CommQuery.ExecSqlFromICE(strSql,CommQuery.DELETE_RELATION,out Msg);
						
				
						if (i < 0)
						{
							//da.RollBack();
							fmda.RollBack();
							//da_zl.RollBack();

							Msg = "删除错误！原因：" + Msg;
							commRes.sendLog4Log("FinanceManage.LogOnUser",Msg);
							return false;
						}
					}
				}

				strtmp = QueryInfo.GetString(ds.Tables[0].Rows[0]["Femail"]);
				if(strtmp != "")
				{
					string fuid = PublicRes.ConvertToFuid(strtmp);
					if(fuid != null && fuid == uid)
					{
						//清空email对应的对应关系表
						string uid3 = "";
						if(!Common.DESCode.GetEmailUid(strtmp,out uid3))
						{
							//da.RollBack();
							fmda.RollBack();
							//da_zl.RollBack();

							Msg = "获取内部ID时出错." + uid3;
							return false;
						}

						/*
						string delStr = "delete from " + PublicRes.GetTName("t_relation",uid3) + " where fqqid ='" + strtmp + "'";

						//furion 20090302 t_relation移到用户资料库里。
						//int i = da.ExecSqlNum(delStr);
						int i = da_zl.ExecSqlNum(delStr);
						*/

						strSql = "uin=" + strtmp;		
						int i = CommQuery.ExecSqlFromICE(strSql,CommQuery.DELETE_RELATION,out Msg);
				
						
						if (i < 0)
						{
							//da.RollBack();
							fmda.RollBack();
							//da_zl.RollBack();

							Msg = "删除错误！删除条数： " + i + " 条。";
							commRes.sendLog4Log("FinanceManage.LogOnUser",Msg);
							return false;
						}
						
					}
				}

				strtmp = QueryInfo.GetString(ds.Tables[0].Rows[0]["Fmobile"]);
				if(strtmp != "")
				{
					string fuid = PublicRes.ConvertToFuid(strtmp);
					if(fuid != null && fuid == uid)
					{
						/*
						//清空手机号对应的对应关系表
						string delStr = "delete from " + PublicRes.GetTName("t_relation",strtmp) + " where fqqid ='" + strtmp + "'";

						//furion 20090302 t_relation移到用户资料库里。
						//int i = da.ExecSqlNum(delStr);
						int i = da_zl.ExecSqlNum(delStr);
						*/
						strSql = "uin=" + strtmp;		
						int i = CommQuery.ExecSqlFromICE(strSql,CommQuery.DELETE_RELATION,out Msg);
				
						if (i < 0)
						{
							//da.RollBack();
							fmda.RollBack();
							//da_zl.RollBack();

							Msg = "删除错误！删除条数： " + i + " 条。";
							commRes.sendLog4Log("FinanceManage.LogOnUser",Msg);
							return false;
						}
					}
				}


				//da.Commit();
				fmda.Commit();
				//da_zl.Commit();
				return true;
			}
			catch(Exception e)
			{
				//da.RollBack();
				fmda.RollBack();
				//da_zl.RollBack();

				Msg = "删除用户关系对应表记录失败！请检查！" + commRes.replaceHtmlStr(e.Message);
				commRes.sendLog4Log("FinanceManage.LogOnUser",Msg);
				return false;
			}
			finally
			{
				//da.Dispose();
				fmda.Dispose();
				//da_zl.Dispose();
			}


		}

	}
}