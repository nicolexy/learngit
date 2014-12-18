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
            rights[21] = new OneRight("CFTUserPick", 21, "财付通自助申诉处理");//一级权限 任何金额审批
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
