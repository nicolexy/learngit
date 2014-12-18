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
            rights[21] = new OneRight("CFTUserPick", 21, "�Ƹ�ͨ�������ߴ���");//һ��Ȩ�� �κν������
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

		

		



		// �����¾�����Ȩ�޵���֤
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




		// ������Ȩ�޵���֤
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


		// ������Ȩ�޵���֤
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
			// ��������µ�Ȩ����֤ģʽ���Ͳ�����UpDateSession��
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
		/// У��Ȩ�ޣ�Ȩ���ַ����ж���
		/// </summary>
		/// <param name="strRightItem">Ȩ�ޱ�ʶ</param>
		/// <param name="strRight">Ȩ���ַ���</param>
		/// <returns>�Ƿ�ɹ�</returns>
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
		public string RightItem = "" ; //Ȩ��Ӣ������
		public int RightID = 0;        //Ȩ��ID��
		public string RightMemo = "";  //Ȩ���������ơ�

		

		public OneRight(string rightItem, int rightID, string rightMemo)
		{
			RightItem = rightItem;
			RightID = rightID;
			RightMemo = rightMemo;
		}	

		/// <summary>
		/// ��Ȩ��У�飬by krola 2011-10-13
		/// </summary>
		/// <param name="strRight">Ȩ���ַ���</param>
		/// <param name="rightID">Ȩ��ID</param>
		/// <returns>�Ƿ�ͨ��</returns>
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
