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


            rights[181] = new OneRight("BankClassifyInfo", 181, "银行分类信息");
            rights[182] = new OneRight("FCXGAccountPassWordReset", 182, "外币账户重置密码");
            rights[176] = new OneRight("InfoCenter", 176, "基础信息管理");//原：23
            rights[177] = new OneRight("PayManagement", 177, "支付管理 ");
            rights[178] = new OneRight("TradeManagement", 178, "交易管理");
            rights[179] = new OneRight("SPInfoManagement", 179, "商户信息管理");
            rights[180] = new OneRight("SystemManagement", 180, "系统管理");
            rights[196] = new OneRight("ShowIDCrad", 196, "显示身份证号码");
			rights[18] = new OneRight("UserReport",18,"意见投诉");
			rights[19] = new OneRight("HistoryModify",19,"资料修改历史");
			rights[81] = new OneRight("FreezeUser",81,"冻结帐户按钮");
			rights[82] = new OneRight("FreezeBalance",82,"冻结可用余额按钮");
			rights[85] = new OneRight("LockTradeList",85,"锁定交易单按钮");
			rights[183] = new OneRight("UnFreezeUser",183,"解冻帐户按钮");
			rights[184] = new OneRight("UnLockTradeList",184,"解锁交易单按钮");
            rights[195] = new OneRight("CFTUserPick", 195, "财付通自助申诉处理");//一级权限 任何金额审批
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
            rights[212] = new OneRight("UnFreezeChannelBDBP", 212, "被盗被骗冻结");
            rights[213] = new OneRight("UnFreezeChannelTX", 213, "套现冻结");
            rights[214] = new OneRight("UnFreezeChannelSD", 214, "涉赌冻结");

            rights[200] = new OneRight("ModifyFundPayCard", 200, "理财通安全卡解绑");
            rights[201] = new OneRight("ModifyFundPayCardByCS", 201, "理财通安全卡解绑(客服)");
            rights[202] = new OneRight("InternetBankRefund", 202, "网银提交账务退款");
            rights[207] = new OneRight("PayBusinessCMD", 207, "修改联系人手机");
            rights[193] = new OneRight("RefundCheck", 193, "退款登记");

            rights[203] = new OneRight("RefundMerchantCheck", 203, "退款商户录入");
            rights[194] = new OneRight("BatchCancellation", 194, "批量注销");
			//增加代扣调整按钮权限
            rights[198] = new OneRight("DKAdjust", 198, "代扣调整状态");

            rights[169] = new OneRight("RealNameCertification",169,"实名认证白名单设置");

            rights[170] = new OneRight("IceOutPPSecurityMoney", 170, "解冻拍拍保证金订单");
            rights[171] = new OneRight("LCTMenu", 171, "理财通菜单");
            rights[172] = new OneRight("HandQMenu", 172, "手Q支付菜单");
            rights[173] = new OneRight("CreditQueryMenu", 173, "信用卡还款菜单");
            rights[174] = new OneRight("FastPayMenu", 174, "快捷支付菜单");
            rights[175] = new OneRight("TencentbusinessMenu", 175, "购买公司业务菜单");


            rights[196] = new OneRight("SensitiveRole", 196, "敏感权限");
            rights[206] = new OneRight("IDCardManualReview_ReviewCount", 206, "身份证影印件批量领单权限");
            rights[210] = new OneRight("IDCardManualReview_SeeDetail", 210, "身份证影印件查看详情权限");
            rights[211] = new OneRight("ChangeUserInfo", 211, "个人信息");

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
