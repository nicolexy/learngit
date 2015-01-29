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

				rights[10] = new OneRight("BaseAccount",10,"基本账户功能");
				rights[11] = new OneRight("InfoCenter",11,"个人账户查询");
				rights[12] = new OneRight("UserBankInfoQuery",12,"银行账号信息");
				rights[13] = new OneRight("ChangeUserInfo",13,"资料查询修改");
				rights[17] = new OneRight("FreezeList",17,"冻结操作查询");
				rights[18] = new OneRight("UserReport",18,"意见投诉");
				rights[19] = new OneRight("HistoryModify",19,"资料修改历史");

				rights[60] = new OneRight("TradeManage",60,"交易管理");
				rights[61] = new OneRight("TradeLogQuery",61,"交易记录查询");
				rights[100] = new OneRight("TradeLogList",100,"商户交易清单");

				rights[81] = new OneRight("FreezeUser",81,"冻结帐户按钮");
				rights[82] = new OneRight("FreezeBalance",82,"冻结可用余额按钮");
				//rights[83] = new OneRight("UnFreezeBalance",83,"解冻冻结金额按钮");
				//rights[84] = new OneRight("ChangeUser",84,"编辑用户资料按钮");
				rights[85] = new OneRight("LockTradeList",85,"锁定交易单按钮");

				rights[62] = new OneRight("FundQuery",62,"充值记录查询");
				rights[162] = new OneRight("GetFundList",62,"充值记录查询");
			
				rights[181] = new OneRight("UnFreezeUser",87,"解冻帐户按钮");
				rights[185] = new OneRight("UnLockTradeList",88,"解锁交易单按钮");
				rights[121] = new OneRight("CFTUserAppeal",11,"财付通自助申诉查询");
				rights[21] = new OneRight("CFTUserPick",21,"财付通自助申诉处理");
				rights[22] = new OneRight("CFTUserPickTJ",22,"财付通自助申诉统计");
                rights[155] = new OneRight("CFTUserPickQuer", 155, "财付通自助申诉处理查询");//二级权限 查询记录及1000元金额以下审批

                rights[161] = new OneRight("NameAbnormalCheck", 161, "姓名异常审批");
				//2006-10-17 Edwinyang新增功能
				rights[122] = new OneRight("CancelAccount",11,"帐户销户记录");
				rights[123] = new OneRight("UpdateAccountQQ",11,"帐户QQ修改");
				//rights[124] = new OneRight("GetFreezeQQ",11,"帐户QQ修改查询");
				rights[125] = new OneRight("QueryQQ",11,"QQ号码查询");
				rights[110] = new OneRight("DrawAndApprove",110,"自助商户领单和审核");
				rights[90] = new OneRight("DeleteCrt",90,"删除个人证书");

				//rights[70] = new OneRight("OverseasPayQuery",70,"国际支付查询");

                rights[150] = new OneRight("MobileConfig", 150, "手机分流配置");
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

                //增加代扣调整按钮权限
                rights[31] = new OneRight("DKAdjust", 31, "代扣调整状态");

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
