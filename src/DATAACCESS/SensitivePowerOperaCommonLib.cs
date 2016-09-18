using System;
using System.Web.UI;
using System.Configuration;
using System.Collections;
using TENCENT.OSS.CFT.KF.Common;
using SensitiveVerifyService;

namespace TENCENT.OSS.CFT.KF.Common
{

	/// <summary>
	/// SensitivePowerOperaLib ��ժҪ˵����
	/// </summary>
	public class SensitivePowerOperaCommonLib
	{
		public SensitivePowerOperaCommonLib()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
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

			rq.system_id = 62;		// �̶���ֵ
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

			rq.system_id = 62;		// �̶���ֵ
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

			rq.system_id = 62;		// �̶���ֵ
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

			rq.system_id = 62;		// �̶���ֵ
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
				//�������е�Ȩ�ޡ�


                rights[181] = new OneRight("BankClassifyInfo", 181, "���з�����Ϣ");
                rights[182] = new OneRight("FCXGAccountPassWordReset", 182, "����˻���������");
                rights[176] = new OneRight("InfoCenter", 176, "������Ϣ����");
                rights[177] = new OneRight("PayManagement", 177, "֧������ ");
                rights[178] = new OneRight("TradeManagement", 178, "���׹���");
                rights[179] = new OneRight("SPInfoManagement", 179, "�̻���Ϣ����");
                rights[180] = new OneRight("SystemManagement", 180, "ϵͳ����");
                rights[196] = new OneRight("ShowIDCrad", 196, "���֤��ѯ");
				rights[18] = new OneRight("UserReport",18,"���Ͷ��");
				rights[19] = new OneRight("HistoryModify",19,"�����޸���ʷ");
				rights[81] = new OneRight("FreezeUser",81,"�����ʻ���ť");
				rights[82] = new OneRight("FreezeBalance",82,"���������ť");
				rights[85] = new OneRight("LockTradeList",85,"�������׵���ť");
				rights[183] = new OneRight("UnFreezeUser",183,"�ⶳ�ʻ���ť");
				rights[184] = new OneRight("UnLockTradeList",184,"�������׵���ť");
                rights[195] = new OneRight("CFTUserPick", 195, "�Ƹ�ͨ�������ߴ���");
                rights[192] = new OneRight("CFTUserPickQuer", 192, "�Ƹ�ͨ�������ߴ����ѯ");//����Ȩ�� ��ѯ��¼��1000Ԫ�����������

                rights[206] = new OneRight("NameAbnormalCheck", 206, "�����쳣����");
                rights[208] = new OneRight("ClearMobileNumber", 208, "�ֻ���������");
                rights[191] = new OneRight("DrawAndApprove", 191, "�����̻��쵥�����");
                rights[199] = new OneRight("DeleteCrt", 199, "ɾ������֤��");
                rights[150] = new OneRight("MobileConfig", 150, "�ֻ���������");
                rights[151] = new OneRight("BussComplain", 151, "�̻�Ͷ�������޸�");
                rights[152] = new OneRight("LCTQuery", 152, "���ͨ��ѯ");
                rights[204] = new OneRight("BalanceControl", 204, "���ͨ������");
                
                //lxl 20140731
                rights[205] = new OneRight("UnFinanceControl", 205, "�ʽ��ܿؽ��");

                rights[185] = new OneRight("UnFreezeChannelFK", 185, "�ⶳ��ض�������");
                rights[186] = new OneRight("UnFreezeChannelPP", 186, "�ⶳ���Ķ�������");
                rights[187] = new OneRight("UnFreezeChannelYH", 187, "�ⶳ�û���������");
                rights[188] = new OneRight("UnFreezeChannelSH", 188, "�ⶳ�̻���������");
                rights[189] = new OneRight("UnFreezeChannelBG", 189, "�ⶳBG�ӿڶ�������");

                rights[200] = new OneRight("ModifyFundPayCard", 200, "���ͨ��ȫ�����");
                rights[201] = new OneRight("ModifyFundPayCardByCS", 201, "���ͨ��ȫ�����(�ͷ�)");
                rights[202] = new OneRight("InternetBankRefund", 202, "�����ύ�����˿�");
                rights[162] = new OneRight("PayBusinessCMD", 162, "�����ύ�����˿�");
                rights[193] = new OneRight("RefundCheck", 193, "�˿�Ǽ�");

                rights[203] = new OneRight("RefundMerchantCheck", 203, "�˿��̻�¼��");
                rights[194] = new OneRight("BatchCancellation", 194, "����ע��");
                //���Ӵ��۵�����ťȨ��
                rights[198] = new OneRight("DKAdjust", 198, "���۵���״̬");

                rights[169] = new OneRight("RealNameCertification", 169, "ʵ����֤����������");

                rights[170] = new OneRight("IceOutPPSecurityMoney", 170, "�ⶳ���ı�֤�𶩵�");
                rights[171] = new OneRight("LCTMenu", 171, "���ͨ�˵�");
                rights[172] = new OneRight("HandQMenu", 172, "��Q֧���˵�");
                rights[173] = new OneRight("CreditQueryMenu", 173, "���ÿ�����˵�");
                rights[174] = new OneRight("FastPayMenu", 174, "���֧���˵�");
                rights[175] = new OneRight("TencentbusinessMenu", 175, "����˾ҵ��˵�");

                rights[196] = new OneRight("SensitiveRole", 196, "����Ȩ��");
                rights[206] = new OneRight("IDCardManualReview_ReviewCount", 206, "���֤Ӱӡ�������쵥Ȩ��");
                rights[210] = new OneRight("IDCardManualReview_SeeDetail", 210, "���֤Ӱӡ���鿴����Ȩ��");
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
