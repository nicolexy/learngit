using System;
using System.Configuration;
using System.Collections;
using SensitiveVerifyService;

namespace TENCENT.OSS.CFT.KF.Common
{
	public class AllUserRight
	{
		const int RIGHTCOUNT = 255;
		public static OneRight[] rights;

		private static string serverIP;
		private static int serverPort;

		private static Hashtable ht;

		public static bool IgnoreLimitCheck = false;

		public AllUserRight()
		{
		}

		static AllUserRight()
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
            rights[28] = new OneRight("ShowIDCrad", 28, "显示身份证号码");
			rights[18] = new OneRight("UserReport",18,"意见投诉");
			rights[19] = new OneRight("HistoryModify",19,"资料修改历史");
			rights[81] = new OneRight("FreezeUser",81,"冻结帐户按钮");
			rights[82] = new OneRight("FreezeBalance",82,"冻结可用余额按钮");
			rights[85] = new OneRight("LockTradeList",85,"锁定交易单按钮");
			rights[181] = new OneRight("UnFreezeUser",87,"解冻帐户按钮");
			rights[185] = new OneRight("UnLockTradeList",88,"解锁交易单按钮");
            rights[21] = new OneRight("CFTUserPick", 21, "财付通自助申诉处理");//一级权限 任何金额审批
            rights[155] = new OneRight("CFTUserPickQuer", 155, "财付通自助申诉处理查询");//二级权限 查询记录及1000元金额以下审批         
            rights[161] = new OneRight("NameAbnormalCheck", 161, "姓名异常审批");
            rights[165] = new OneRight("ClearMobileNumber", 165, "手机号码清理");
			rights[110] = new OneRight("DrawAndApprove",110,"自助商户领单和审核");
			rights[90] = new OneRight("DeleteCrt",90,"删除个人证书");
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
            rights[166] = new OneRight("RefundCheck", 166, "退款登记");

            rights[102] = new OneRight("RefundMerchantCheck", 102, "退款商户录入");
            rights[20] = new OneRight("BatchCancellation", 20, "批量注销");
			//增加代扣调整按钮权限
			rights[31] = new OneRight("DKAdjust", 31, "代扣调整状态");

            rights[169] = new OneRight("RealNameCertification",169,"实名认证白名单设置");

            rights[170] = new OneRight("IceOutPPSecurityMoney", 170, "解冻拍拍保证金订单");

            rights[196] = new OneRight("SensitiveRole", 196, "敏感权限");

			ht = new Hashtable(RIGHTCOUNT);
			for(int i=0 ; i< RIGHTCOUNT; i++)
			{
				if(rights[i] != null)
				{
					ht.Add(rights[i].RightItem.Trim().ToUpper(),i);
				}
			}
                        
		}

		public static int GetServiceID(string strRightItem)
		{
			if(strRightItem == null || strRightItem.Trim() == "")
				return -1;

			try
			{
				int iindex = (int)ht[strRightItem.Trim().ToUpper()];

				int iRightID = rights[iindex].RightID;
				return iRightID;
			}
			catch
			{
				return -1;
			}
		}

		

		



		// 兼容新旧敏感权限的验证
		public static bool ValidRight(string strSzKey,string strSzID,int igroupid,string strRightItem,
			string userIP,string sessionID,string url)
		{
			if(GetData.IsTestMode)
				return true;

			try
			{
				if(GetData.IsNewSensitiveMode)
				{
					return (ValidRight_New(strSzKey,strSzID,igroupid,strRightItem,userIP,sessionID,url).status == 0);
				}
				else
				{
					return ValidRight(strSzKey,int.Parse(strSzID),igroupid,strRightItem);
				}
			}
			catch (System.Exception ex)
			{
				return false;
			}	
		}




		// 旧敏感权限的验证
		private static bool ValidRight(string strSzkey, int iOperId, int groupid, string strRightItem)
		{
			try
			{
				if(GetData.IsTestMode)
					return true;

				int iindex = (int)ht[strRightItem.Trim().ToUpper()];

				int iRightID = rights[iindex].RightID;

				return UserRight.ValidateRight(strSzkey,iOperId,groupid,iRightID,serverIP,serverPort);
			}
			catch
			{
				return false;
			}
		}


		// 新敏感权限的验证
		public static Result ValidRight_New(string strSzKey,string strSzID,int igroupid,string strRightItem,
			string userIP,string sessionID,string url)
		{
			try
			{
				if(GetData.IsNewSensitiveMode)
				{
					return SensitivePowerOperaCommonLib.CheckAuth(strRightItem,strSzID,strSzKey,url,userIP,sessionID);
					//throw new Exception("UnFin");
				}
				else
				{
					return null;
				}
			}
			catch (System.Exception ex)
			{
				return null;
			}			
		}


		public static Result CheckSession(string ticket,string url,string ip,string sessionID,string loginName,string szKey)
		{
			return SensitivePowerOperaCommonLib.CheckSession(ticket,url,ip,sessionID,loginName,szKey);
		}



		public static Result CheckAuth(string opName,string userName,string sessionKey,string url,string ip,string sessionID)
		{
			return SensitivePowerOperaCommonLib.CheckAuth(opName,userName,sessionKey,url,ip,sessionID);
		}



		public static string MakeLog(string opType,string targetQQID,string editDesc,params string[] strParams)
		{
			return SensitivePowerOperaCommonLib.MakeLog(opType,targetQQID,editDesc,strParams);
		}



		public static Result WriteLog(string opName,string userName,string sessionKey,string url,string ip,string sessionID
			,string log)
		{
			return SensitivePowerOperaCommonLib.WriteOperationRecord(opName,userName,sessionKey,url,ip,sessionID,log);
		}		



		public static void UpdateSession(string strSzkey, int iOperID,int iGroupID, int iServiceID,string struserdata,string content)
		{
			// 如果采用新的权限验证模式，就不采用UpDateSession了
			if(GetData.IsNewSensitiveMode)
				return;

			try
			{
				UserRight.UpdateSession(strSzkey,iOperID,iGroupID,iServiceID,struserdata,content,serverIP,serverPort);
			}
			catch(Exception err)
			{
				
			}
		}

		/// <summary>
		/// 校验权限（权限字符串判定）
		/// </summary>
		/// <param name="strRightItem">权限标识</param>
		/// <param name="strRight">权限字符串</param>
		/// <returns>是否成功</returns>
		public static bool GetOneRightState(string strRightItem,  string strRight)
		{
			try
			{
				int iindex = (int)ht[strRightItem.Trim().ToUpper()];

				return OneRight.ValidRight(strRight, rights[iindex].RightID);
			}
			catch(Exception err)
			{
				string tmp = err.Message;
				return false;
			}
		}
	}




	public class OneRight
	{
		public string RightItem = "" ; //权限英文名。
		public int RightID = 0;        //权限ID。
		public string RightMemo = "";  //权限中文名称。

		

		public OneRight(string rightItem, int rightID, string rightMemo)
		{
			RightItem = rightItem;
			RightID = rightID;
			RightMemo = rightMemo;
		}	

		/// <summary>
		/// 新权限校验，by krola 2011-10-13
		/// </summary>
		/// <param name="strRight">权限字符串</param>
		/// <param name="rightID">权限ID</param>
		/// <returns>是否通过</returns>
		public static bool ValidRight(string strRight, int rightID)
		{
			if(AllUserRight.IgnoreLimitCheck)
				return true;
		 
#if QUXIANDEBUG
		 			return true;
#else
			return Tencent.CFT.LimitManage.LimitCommonLib.ValidRight(strRight, rightID);


#endif
		}

	}


}
