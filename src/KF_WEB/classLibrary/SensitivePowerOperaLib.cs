using System;
using System.Web.UI;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.KF_Web;
//using TENCENT.OSS.CFT.KF.KF_Web.SensitivePowerSystem_Service;
using System.Configuration;
using System.Collections;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.classLibrary
{
	/// <summary>
	/// SensitivePowerOperaLib 的摘要说明。
	/// </summary>
	public class SensitivePowerOperaLib
	{
		public SensitivePowerOperaLib()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}


		private static bool IsOpenSensitiveCheck()
		{
			return getData.IsNewSensitivePowerMode;
		}

		private static bool IsOutOfTime(TemplateControl control)
		{
			//return (control.Page.Session["OperID"] == null || control.Page.Session["SzKey"] == null || control.Page.Session["Ticket"] == null);
		
			bool b1 = control.Page.Session["OperID"] == null;
			bool b2 = control.Page.Session["SzKey"] == null;
			//bool b3 = control.Page.Session["Ticket"] == null;
			bool b4 = control.Page.Session["uid"] == null;

			return (b1 || b2 || b4);
		}



		// 如果没禁用Javascript，这个是可行的。
		private static void HandleSPResult(Page page,string msg,bool isRedirect,string url)
		{
			string strJs = "<script language=javascript>alert('" + setConfig.replaceHtmlStr(msg) + "');";

			if(isRedirect)
			{
				strJs += "location.href='" + url + "'";
				//strJs += "location.href='http://www.baidu.com'";
			}

			strJs += "</script>";

			page.Response.Write(strJs);
			page.Response.Flush();
		}
	


		public static bool CheckSPReturnResult(SensitiveVerifyService.Result result,Page page)
		{
			if(!IsOpenSensitiveCheck())
				return true;

			if(result.status == 0)
			{
				return true;
			}
			else if(result.status >= 100 && result.status <= 199)
			{
				//WebUtils.ShowMessage(page.Page,"调用验证权限出错,请重试，或联系系统维护人员。" + result.status_info);

				//HandleSPResult(page,"调用验证权限出错,请重试，或联系系统维护人员。" + result.status_info,false,"");

				return false;	
			}
			else if(result.status >= 200 && result.status <= 299)
			{
				//WebUtils.ShowMessage(page.Page,"您没有敏感登录状态或需要更高级别的敏感登录状态。" + result.status_info);

				//page.Page.Response.Redirect(result.login_url);

				//page.Response.Write("<script language=javascript>alert('您没有敏感登录状态或需要更高级别的敏感登录状态');location.href='" + result.login_url + "'</script>");

				//page.Response.Flush();

				//HandleSPResult(page,"您没有敏感登录状态或需要更高级别的敏感登录状态。" + result.status_info,true,result.login_url);

                //page.Page.Response.Redirect(result.login_url);

               // page.Page.Response.Write("<script>window.parent.location.href = '" + result.login_url + "';</script>");

                var script = "<script type='text/javascript'>"+
                                 "var win = window.parent.window || window;" +
                                 "win.location.href = '" + result.login_url + "';" +
                             "</script>";
                page.Page.Response.Write(script);
                page.Page.Response.Cookies.Clear(); // 清空Cookie;
                page.Page.Session.Clear();          // 清空Session;
               
				return false;
			}
			else if(result.status >= 300 && result.status <= 399)
			{
				//WebUtils.ShowMessage(page.Page,"您没有当前操作的权限," + result.status_info + "  开通权限请访问:" + result.auth_apply_url);
				
				//HandleSPResult(page,"您没有当前操作的权限," + result.status_info + "  开通权限请访问:" + result.auth_apply_url,false,"");

				return false;
			}

			return false;
		}


		public static bool CheckSPReturnResult(SensitiveVerifyService.Result result,UserControl page)
		{
			if(!IsOpenSensitiveCheck())
				return true;

			if(result.status == 0)
			{
				return true;
			}
			else if(result.status >= 100 && result.status <= 199)
			{
				//WebUtils.ShowMessage(page,"调用验证权限出错,请重试，或联系系统维护人员。" + result.status_info);
				//ClassLib.ShowMessage(page,"调用验证权限出错,请重试，或联系系统维护人员。" + result.status_info);
				return false;	
			}
			else if(result.status >= 200 && result.status <= 299)
			{
				//WebUtils.ShowMessage(page,"您没有敏感登录状态或需要更高级别的敏感登录状态。" + result.status_info);
				//ClassLib.ShowMessage(page,"您没有敏感登录状态或需要更高级别的敏感登录状态。" + result.status_info);
				page.Response.Redirect(result.login_url);
				return false;
			}
			else if(result.status >= 300 && result.status <= 399)
			{
				//WebUtils.ShowMessage(page,"您没有当前操作的权限," + result.status_info + "  开通权限请访问:" + result.auth_apply_url);
				//ClassLib.ShowMessage(page,"您没有当前操作的权限," + result.status_info + "  开通权限请访问:" + result.auth_apply_url);
				return false;
			}

			return false;
		}


		public static bool CheckSession(string ticket,Page page)
		{
			string loginName = "",szKey = "";

			if(page.Page.Session["uid"] != null)
				loginName = page.Page.Session["uid"].ToString();

			if(page.Page.Session["SzKey"] != null)
				szKey = page.Page.Session["SzKey"].ToString();

			return CheckSession(ticket,page.Page.Request.Url.ToString(),page.Page.Request.UserHostAddress,page.Page.Session.SessionID,loginName,szKey,page);
		}

		public static bool CheckSession(string ticket,UserControl page)
		{
			string loginName = "",szKey = "";

			if(page.Session["uid"] != null)
				loginName = page.Session["uid"].ToString();

			if(page.Session["SzKey"] != null)
				szKey = page.Session["SzKey"].ToString();

			return CheckSession(ticket,page.Request.Url.ToString(),page.Request.UserHostAddress,page.Session.SessionID,loginName,szKey,page);
		}

		private static bool CheckSession(string ticket,string url,string ip,string sessionID,string loginName,string szKey,Page page)
		{
			if(!IsOpenSensitiveCheck())
				return true;

			SensitiveVerifyService.Result ret = Common.AllUserRight.CheckSession(ticket,url,ip,sessionID,loginName,szKey);

			if(CheckSPReturnResult(ret,page))
			{	
				// 新敏感权限系统没有OperaID的获取了，应该如何和旧系统兼容？
				if(page.Page.Session["OperID"] == null)
					page.Page.Session["OperID"] = "0";

				if(page.Page.Session["SzKey"] == null)
					page.Page.Session["SzKey"] = ret.auth_cm_com_session_key;

				if(page.Page.Session["uid"] == null)
					page.Page.Session["uid"] = ret.user_name;

				return true;
			}
			else
			{
				return false;
			}
		}

		private static bool CheckSession(string ticket,string url,string ip,string sessionID,string loginName,string szKey,UserControl page)
		{
			if(!IsOpenSensitiveCheck())
				return true;

			SensitiveVerifyService.Result ret = Common.AllUserRight.CheckSession(ticket,url,ip,sessionID,loginName,szKey);

			if(CheckSPReturnResult(ret,page))
			{
				// 新敏感权限系统没有OperaID的获取了，应该如何和旧系统兼容？
				if(page.Session["OperID"] == null)
					page.Session["OperID"] = "0";

				if(page.Session["SzKey"] == null)
					page.Session["SzKey"] = ret.auth_cm_com_session_key;

				if(page.Session["uid"] == null)
					page.Session["uid"] = ret.user_name;

				return true;
			}
			else
			{
				return false;
			}
		}
	
		public static bool CheckAuth(string opName,UserControl control)
		{
			// 敏感权限文档上写每个页面都需要调用checkSession，所以在CheckAuth里边调用checkSession
			//if(IsOutOfTime(control) || !CheckSession(control.Page.Session["Ticket"].ToString(),control))
            if (IsOutOfTime(control))
            {
                control.Page.Response.Redirect("../login.aspx?wh=1");
                return false;
            }

			return CheckAuth(opName,control.Page.Session["uid"].ToString(),control.Page.Session["SzKey"].ToString(),
				control.Page.Request.Url.ToString(),control.Page.Request.UserHostAddress,control.Page.Session.SessionID,control);
		}

		public static bool CheckAuth(string opName,Page page)
		{
			// 敏感权限文档上写每个页面都需要调用checkSession，所以在CheckAuth里边调用checkSession
			//if(IsOutOfTime(page) || !CheckSession(page.Session["Ticket"].ToString(),page))
            if (IsOutOfTime(page))
            {
                page.Response.Redirect("../login.aspx?wh=1");
                return false;
            }

			return CheckAuth(opName,page.Session["uid"].ToString(),page.Session["SzKey"].ToString(),page.Request.Url.ToString(),
				page.Request.UserHostAddress,page.Session.SessionID,page);
		}

		private static bool CheckAuth(string opName,string userName,string sessionKey,string url,string ip,string sessionID,Page page)
		{
			if(!IsOpenSensitiveCheck())
				return true;

			SensitiveVerifyService.Result retResult = AllUserRight.CheckAuth(opName,userName,sessionKey,url,ip,sessionID);
			return CheckSPReturnResult(retResult,page);
		}

		private static bool CheckAuth(string opName,string userName,string sessionKey,string url,string ip,string sessionID,UserControl page)
		{
			if(!IsOpenSensitiveCheck())
				return true;

			SensitiveVerifyService.Result retResult = AllUserRight.CheckAuth(opName,userName,sessionKey,url,ip,sessionID);

			return CheckSPReturnResult(retResult,page);
		}

		public static string MakeLog(string opType,string targetQQID,string editDesc,params string[] strParams)
		{
			if(!IsOpenSensitiveCheck())
				return "";

			string log = "|" + opType + "|";

			foreach(string str in strParams)
			{
				log += str + ",";
			}

			log += "|" + targetQQID + "|" + editDesc + "|";

			return log;
		}

		public static bool WriteOperationRecord(string opPowerName,string log,Page page)
		{
			if(IsOutOfTime(page))
				page.Page.Response.Redirect("../login.aspx?wh=1");

			return WriteOperationRecord(opPowerName,page.Page.Session["uid"].ToString(),page.Page.Session["SzKey"].ToString(),page.Page.Request.Url.ToString(),
				page.Page.Request.UserHostAddress,page.Page.Session.SessionID,log,page);
		}

		private static bool WriteOperationRecord(string opName,string userName,string sessionKey,string url,string ip,string sessionID
			,string log,Page page)
		{
			if(!IsOpenSensitiveCheck())
				return true;

			SensitiveVerifyService.Result ret = AllUserRight.WriteLog(opName,userName,sessionKey,url,ip,sessionID,log);

			return CheckSPReturnResult(ret,page);
		}

		public static bool LogOut(Page page)
		{
			if(IsOutOfTime(page))
				page.Page.Response.Redirect("../login.aspx?wh=1");

			return LogOut(page.Page.Session["uid"].ToString(),page.Page.Session["SzKey"].ToString(),page.Page.Request.Url.ToString(),
				page.Page.Request.UserHostAddress,page.Page.Session.SessionID,page);
		}

		private static bool LogOut(string userName,string sessionKey,string url,string ip,string sessionID,Page page)
		{
			if(!IsOpenSensitiveCheck())
				return true;

			SensitiveVerifyService.auth authService = new SensitiveVerifyService.auth();

			SensitiveVerifyService.Request rq = new SensitiveVerifyService.Request();

			rq.system_id = 62;		// 固定的值
			rq.system_idSpecified = true;
			rq.user_name = userName;
			rq.auth_cm_com_session_key = sessionKey;
			rq.user_url = url;
			rq.user_ip = ip;
			rq.local_session_id = sessionID;

			SensitiveVerifyService.Result ret = authService.LogOut(rq);

			return CheckSPReturnResult(ret,page);
		}

		public static string Echo(string echoStr)
		{
			SensitiveVerifyService.auth authService = new SensitiveVerifyService.auth();

			string strResult = authService.Echo(echoStr);

			return strResult;
		}

		public static int GetPowerID(string powerName)
		{
			return PowerLib.GetPowerID(powerName);
		}


		#region PowerLib


		class PowerLib
		{
			const int RIGHTCOUNT = 255;
			public static OneRight[] rights;

			private static string serverIP;
			private static int serverPort;

			private static Hashtable ht;

			public static bool IgnoreLimitCheck = false;

			private PowerLib() { }

			static PowerLib() 
			{
				serverIP = ConfigurationManager.AppSettings["ServerIP"];
				serverPort = Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]);

				rights = new OneRight[RIGHTCOUNT];
				//加入所有的权限。


                rights[80] = new OneRight("BankClassifyInfo", 80, "银行分类信息");
                rights[86] = new OneRight("FCXGAccountPassWordReset", 86, "外币账户重置密码");
                rights[23] = new OneRight("InfoCenter", 23, "基础信息管理");
                rights[24] = new OneRight("PayManagement", 24, "支付管理 ");
                rights[25] = new OneRight("TradeManagement", 25, "交易管理");
                rights[26] = new OneRight("SPInfoManagement", 26, "商户信息管理");
                rights[27] = new OneRight("SystemManagement", 27, "系统管理");
                rights[28] = new OneRight("ShowIDCrad", 28, "身份证查询");
				rights[18] = new OneRight("UserReport",18,"意见投诉");
				rights[19] = new OneRight("HistoryModify",19,"资料修改历史");
				rights[81] = new OneRight("FreezeUser",81,"冻结帐户按钮");
				rights[82] = new OneRight("FreezeBalance",82,"冻结可用余额按钮");
				rights[85] = new OneRight("LockTradeList",85,"锁定交易单按钮");
				rights[181] = new OneRight("UnFreezeUser",87,"解冻帐户按钮");
				rights[185] = new OneRight("UnLockTradeList",88,"解锁交易单按钮");
				rights[21] = new OneRight("CFTUserPick",21,"财付通自助申诉处理");
                rights[155] = new OneRight("CFTUserPickQuer", 155, "财付通自助申诉处理查询");//二级权限 查询记录及1000元金额以下审批
                rights[161] = new OneRight("NameAbnormalCheck", 161, "姓名异常审批");
                rights[165] = new OneRight("ClearMobileNumber", 165, "手机号码清理");
				rights[110] = new OneRight("DrawAndApprove",110,"自助商户领单和审核");
				rights[90] = new OneRight("DeleteCrt",90,"删除个人证书");
                rights[150] = new OneRight("MobileConfig", 150, "手机分流配置");
                //add by yinhuang 2013/12/10
                rights[151] = new OneRight("BussComplain", 151, "商户投诉新增修改");
                rights[152] = new OneRight("LCTQuery", 152, "理财通查询");
                rights[153] = new OneRight("BalanceControl", 153, "理财通余额控制");
                
                //lxl 20140731
                rights[154] = new OneRight("UnFinanceControl", 154, "资金受控解控");

                rights[91] = new OneRight("UnFreezeChannelFK", 91, "解冻风控冻结渠道");
                rights[92] = new OneRight("UnFreezeChannelPP", 92, "解冻拍拍冻结渠道");
                rights[93] = new OneRight("UnFreezeChannelYH", 93, "解冻用户冻结渠道");
                rights[94] = new OneRight("UnFreezeChannelSH", 94, "解冻商户冻结渠道");
                rights[95] = new OneRight("UnFreezeChannelBG", 95, "解冻BG接口冻结渠道");

                rights[96] = new OneRight("ModifyFundPayCard", 96, "理财通安全卡解绑");
                rights[97] = new OneRight("ModifyFundPayCardByCS", 97, "理财通安全卡解绑(客服)");
                rights[99] = new OneRight("InternetBankRefund", 99, "网银提交账务退款");
                rights[156] = new OneRight("PayBusinessCMD", 162, "网银提交账务退款");
                rights[166] = new OneRight("RefundCheck", 166, "退款登记");

                rights[102] = new OneRight("RefundMerchantCheck", 102, "退款商户录入");
                rights[20] = new OneRight("BatchCancellation", 20, "批量注销");
                //增加代扣调整按钮权限
                rights[31] = new OneRight("DKAdjust", 31, "代扣调整状态");

                rights[169] = new OneRight("RealNameCertification", 169, "实名认证白名单设置");

				ht = new Hashtable(RIGHTCOUNT);
				for(int i=0 ; i< RIGHTCOUNT; i++)
				{
					if(rights[i] != null)
					{
						ht.Add(rights[i].RightItem.Trim().ToUpper(),i);
					}
				}
			}

			public static int GetPowerID(string powerName)
			{
				try
				{
					if(powerName == null || powerName.Trim() == "")
						return -1;

					int iindex = (int)ht[powerName.Trim().ToUpper()];

					int iRightID = rights[iindex].RightID;

					return iRightID;
				}
				catch
				{
					return -1;
				}
			}

		}

		#endregion

	}

}
