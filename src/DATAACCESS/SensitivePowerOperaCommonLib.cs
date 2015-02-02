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

				rights[10] = new OneRight("BaseAccount",10,"�����˻�����");
				rights[11] = new OneRight("InfoCenter",11,"�����˻���ѯ");
				rights[12] = new OneRight("UserBankInfoQuery",12,"�����˺���Ϣ");
				rights[13] = new OneRight("ChangeUserInfo",13,"���ϲ�ѯ�޸�");
				rights[17] = new OneRight("FreezeList",17,"���������ѯ");
				rights[18] = new OneRight("UserReport",18,"���Ͷ��");
				rights[19] = new OneRight("HistoryModify",19,"�����޸���ʷ");

				rights[60] = new OneRight("TradeManage",60,"���׹���");
				rights[61] = new OneRight("TradeLogQuery",61,"���׼�¼��ѯ");
				rights[100] = new OneRight("TradeLogList",100,"�̻������嵥");

				rights[81] = new OneRight("FreezeUser",81,"�����ʻ���ť");
				rights[82] = new OneRight("FreezeBalance",82,"���������ť");
				//rights[83] = new OneRight("UnFreezeBalance",83,"�ⶳ�����ť");
				//rights[84] = new OneRight("ChangeUser",84,"�༭�û����ϰ�ť");
				rights[85] = new OneRight("LockTradeList",85,"�������׵���ť");

				rights[62] = new OneRight("FundQuery",62,"��ֵ��¼��ѯ");
				rights[162] = new OneRight("GetFundList",62,"��ֵ��¼��ѯ");
			
				rights[181] = new OneRight("UnFreezeUser",87,"�ⶳ�ʻ���ť");
				rights[185] = new OneRight("UnLockTradeList",88,"�������׵���ť");
				rights[121] = new OneRight("CFTUserAppeal",11,"�Ƹ�ͨ�������߲�ѯ");
				rights[21] = new OneRight("CFTUserPick",21,"�Ƹ�ͨ�������ߴ���");
				rights[22] = new OneRight("CFTUserPickTJ",22,"�Ƹ�ͨ��������ͳ��");
                rights[155] = new OneRight("CFTUserPickQuer", 155, "�Ƹ�ͨ�������ߴ����ѯ");//����Ȩ�� ��ѯ��¼��1000Ԫ�����������

                rights[161] = new OneRight("NameAbnormalCheck", 161, "�����쳣����");
				//2006-10-17 Edwinyang��������
				rights[122] = new OneRight("CancelAccount",11,"�ʻ�������¼");
				rights[123] = new OneRight("UpdateAccountQQ",11,"�ʻ�QQ�޸�");
				//rights[124] = new OneRight("GetFreezeQQ",11,"�ʻ�QQ�޸Ĳ�ѯ");
				rights[125] = new OneRight("QueryQQ",11,"QQ�����ѯ");
				rights[110] = new OneRight("DrawAndApprove",110,"�����̻��쵥�����");
				rights[90] = new OneRight("DeleteCrt",90,"ɾ������֤��");

				//rights[70] = new OneRight("OverseasPayQuery",70,"����֧����ѯ");

                rights[150] = new OneRight("MobileConfig", 150, "�ֻ���������");
                rights[151] = new OneRight("BussComplain", 151, "�̻�Ͷ�������޸�");
                rights[152] = new OneRight("LCTQuery", 152, "���ͨ��ѯ");
                rights[153] = new OneRight("BalanceControl", 153, "���ͨ������");
                
                //lxl 20140731
                rights[154] = new OneRight("UnFinanceControl", 154, "�ʽ��ܿؽ��");

                rights[91] = new OneRight("UnFreezeChannelFK", 91, "�ⶳ��ض�������");
                rights[92] = new OneRight("UnFreezeChannelPP", 92, "�ⶳ���Ķ�������");
                rights[93] = new OneRight("UnFreezeChannelYH", 93, "�ⶳ�û���������");
                rights[94] = new OneRight("UnFreezeChannelSH", 94, "�ⶳ�̻���������");
                rights[95] = new OneRight("UnFreezeChannelBG", 95, "�ⶳBG�ӿڶ�������");

                rights[96] = new OneRight("ModifyFundPayCard", 96, "���ͨ��ȫ�����");
                rights[97] = new OneRight("ModifyFundPayCardByCS", 97, "���ͨ��ȫ�����(�ͷ�)");
                rights[99] = new OneRight("InternetBankRefund", 99, "�����ύ�����˿�");
                rights[156] = new OneRight("PayBusinessCMD", 162, "�����ύ�����˿�");

                //���Ӵ��۵�����ťȨ��
                rights[31] = new OneRight("DKAdjust", 31, "���۵���״̬");

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
