using System;
using System.Web.UI;
using System.Configuration;
using System.Collections;
using TENCENT.OSS.CFT.KF.Common;
using SensitiveVerifyService;

namespace TENCENT.OSS.CFT.KF.Common
{

	/// <summary>
	/// SensitivePowerOperaLib 的摘要说明。
	/// </summary>
	public class SensitivePowerOperaCommonLib
	{
		public SensitivePowerOperaCommonLib()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}


		private static bool IsOpenSensitiveCheck()
		{
			return GetData.IsNewSensitiveMode;
		}



		public static Result CheckSession(string ticket,string url,string ip,string sessionID,string loginName,string szKey)
		{
			if(!IsOpenSensitiveCheck())
				return null;

			SensitiveVerifyService.auth authService = new SensitiveVerifyService.auth();

			SensitiveVerifyService.Request rq = new SensitiveVerifyService.Request();

			rq.system_id = 62;		// 固定的值
			rq.system_idSpecified = true;
			rq.auth_cm_com_ticket = ticket;
			rq.auth_cm_com_session_key = szKey;
			rq.user_url = url;
			rq.user_ip = ip;
			rq.user_name = loginName;
			rq.local_session_id = sessionID;

			return authService.CheckSession(rq);
		}


		

		public static Result CheckAuth(string opName,string userName,string sessionKey,string url,string ip,string sessionID)
		{
			if(!IsOpenSensitiveCheck())
				return null;

			int opID = GetPowerID(opName);

			Result ret_checkSession = CheckSession(sessionKey,url,ip,sessionID,userName,sessionKey);

			if(ret_checkSession == null || ret_checkSession.status != 0)
				return null;

			SensitiveVerifyService.auth authService = new SensitiveVerifyService.auth();

			SensitiveVerifyService.Request rq = new SensitiveVerifyService.Request();

			rq.system_id = 62;		// 固定的值
			rq.system_idSpecified = true;
			rq.operation_id = opID;
			rq.operation_idSpecified = true;
			rq.user_name = userName;
			rq.auth_cm_com_session_key = sessionKey;
			rq.user_url = url;
			rq.user_ip = ip;
			rq.local_session_id = sessionID;

			Result retResult = authService.CheckAuth(rq);

			return retResult;
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



		

		
		public static Result WriteOperationRecord(string opName,string userName,string sessionKey,string url,string ip,string sessionID
			,string log)
		{
			if(!IsOpenSensitiveCheck())
				return null;

			int opID = GetPowerID(opName);

			SensitiveVerifyService.auth authService = new SensitiveVerifyService.auth();

			SensitiveVerifyService.Request rq = new SensitiveVerifyService.Request();

			rq.system_id = 62;		// 固定的值
			rq.system_idSpecified = true;
			rq.operation_id = opID;
			rq.operation_idSpecified = true;
			rq.user_name = userName;
			rq.auth_cm_com_session_key = sessionKey;
			rq.user_url = url;
			rq.user_ip = ip;
			rq.local_session_id = sessionID;
//			byte[] byteArr = new byte[log.Length];
//			System.Text.Encoding.UTF8.GetBytes(log,0,log.Length,byteArr,0);
			byte[] byteArr = System.Text.Encoding.UTF8.GetBytes(log);
			rq.operation_log = System.Text.Encoding.UTF8.GetString(byteArr);

			Result ret = authService.WriteOperationRecord(rq);

			return ret;
		}




		public static Result LogOut(string userName,string sessionKey,string url,string ip,string sessionID,Page page)
		{
			if(!IsOpenSensitiveCheck())
				return null;

			SensitiveVerifyService.auth authService = new SensitiveVerifyService.auth();

			SensitiveVerifyService.Request rq = new SensitiveVerifyService.Request();

			rq.system_id = 62;		// 固定的值
			rq.system_idSpecified = true;
			rq.user_name = userName;
			rq.auth_cm_com_session_key = sessionKey;
			rq.user_url = url;
			rq.user_ip = ip;
			rq.local_session_id = sessionID;

			Result ret = authService.LogOut(rq);

			return ret;
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


                rights[181] = new OneRight("BankClassifyInfo", 181, "银行分类信息");
                rights[182] = new OneRight("FCXGAccountPassWordReset", 182, "外币账户重置密码");
                rights[176] = new OneRight("InfoCenter", 176, "基础信息管理");
                rights[177] = new OneRight("PayManagement", 177, "支付管理 ");
                rights[178] = new OneRight("TradeManagement", 178, "交易管理");
                rights[179] = new OneRight("SPInfoManagement", 179, "商户信息管理");
                rights[180] = new OneRight("SystemManagement", 180, "系统管理");
                rights[196] = new OneRight("ShowIDCrad", 196, "身份证查询");
				rights[18] = new OneRight("UserReport",18,"意见投诉");
				rights[19] = new OneRight("HistoryModify",19,"资料修改历史");
				rights[81] = new OneRight("FreezeUser",81,"冻结帐户按钮");
				rights[82] = new OneRight("FreezeBalance",82,"冻结可用余额按钮");
				rights[85] = new OneRight("LockTradeList",85,"锁定交易单按钮");
				rights[183] = new OneRight("UnFreezeUser",183,"解冻帐户按钮");
				rights[184] = new OneRight("UnLockTradeList",184,"解锁交易单按钮");
                rights[195] = new OneRight("CFTUserPick", 195, "财付通自助申诉处理");
                rights[192] = new OneRight("CFTUserPickQuer", 192, "财付通自助申诉处理查询");//二级权限 查询记录及1000元金额以下审批

                rights[206] = new OneRight("NameAbnormalCheck", 206, "姓名异常审批");
                rights[208] = new OneRight("ClearMobileNumber", 208, "手机号码清理");
                rights[191] = new OneRight("DrawAndApprove", 191, "自助商户领单和审核");
                rights[199] = new OneRight("DeleteCrt", 199, "删除个人证书");
                rights[150] = new OneRight("MobileConfig", 150, "手机分流配置");
                rights[151] = new OneRight("BussComplain", 151, "商户投诉新增修改");
                rights[152] = new OneRight("LCTQuery", 152, "理财通查询");
                rights[204] = new OneRight("BalanceControl", 204, "理财通余额控制");
                
                //lxl 20140731
                rights[205] = new OneRight("UnFinanceControl", 205, "资金受控解控");

                rights[185] = new OneRight("UnFreezeChannelFK", 185, "解冻风控冻结渠道");
                rights[186] = new OneRight("UnFreezeChannelPP", 186, "解冻拍拍冻结渠道");
                rights[187] = new OneRight("UnFreezeChannelYH", 187, "解冻用户冻结渠道");
                rights[188] = new OneRight("UnFreezeChannelSH", 188, "解冻商户冻结渠道");
                rights[189] = new OneRight("UnFreezeChannelBG", 189, "解冻BG接口冻结渠道");

                rights[200] = new OneRight("ModifyFundPayCard", 200, "理财通安全卡解绑");
                rights[201] = new OneRight("ModifyFundPayCardByCS", 201, "理财通安全卡解绑(客服)");
                rights[202] = new OneRight("InternetBankRefund", 202, "网银提交账务退款");
                rights[162] = new OneRight("PayBusinessCMD", 162, "网银提交账务退款");
                rights[193] = new OneRight("RefundCheck", 193, "退款登记");

                rights[203] = new OneRight("RefundMerchantCheck", 203, "退款商户录入");
                rights[194] = new OneRight("BatchCancellation", 194, "批量注销");
                //增加代扣调整按钮权限
                rights[198] = new OneRight("DKAdjust", 198, "代扣调整状态");

                rights[169] = new OneRight("RealNameCertification", 169, "实名认证白名单设置");

                rights[170] = new OneRight("IceOutPPSecurityMoney", 170, "解冻拍拍保证金订单");
                rights[171] = new OneRight("LCTMenu", 171, "理财通菜单");
                rights[172] = new OneRight("HandQMenu", 172, "手Q支付菜单");
                rights[173] = new OneRight("CreditQueryMenu", 173, "信用卡还款菜单");
                rights[174] = new OneRight("FastPayMenu", 174, "快捷支付菜单");
                rights[175] = new OneRight("TencentbusinessMenu", 175, "购买公司业务菜单");

                rights[196] = new OneRight("SensitiveRole", 196, "敏感权限");
                rights[206] = new OneRight("IDCardManualReview_ReviewCount", 206, "身份证影印件批量领单权限");
                rights[210] = new OneRight("IDCardManualReview_SeeDetail", 210, "身份证影印件查看详情权限");
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
				catch (System.Exception ex)
				{
					return -1;
				}
			}

		}

		#endregion



	}


	

}
