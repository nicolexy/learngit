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


            rights[181] = new OneRight("BankClassifyInfo", 181, "���з�����Ϣ");
            rights[182] = new OneRight("FCXGAccountPassWordReset", 182, "����˻���������");
            rights[176] = new OneRight("InfoCenter", 176, "������Ϣ����");//ԭ��23
            rights[177] = new OneRight("PayManagement", 177, "֧������ ");
            rights[178] = new OneRight("TradeManagement", 178, "���׹���");
            rights[179] = new OneRight("SPInfoManagement", 179, "�̻���Ϣ����");
            rights[180] = new OneRight("SystemManagement", 180, "ϵͳ����");
            rights[196] = new OneRight("ShowIDCrad", 196, "��ʾ���֤����");
			rights[18] = new OneRight("UserReport",18,"���Ͷ��");
			rights[19] = new OneRight("HistoryModify",19,"�����޸���ʷ");
			rights[81] = new OneRight("FreezeUser",81,"�����ʻ���ť");
			rights[82] = new OneRight("FreezeBalance",82,"���������ť");
			rights[85] = new OneRight("LockTradeList",85,"�������׵���ť");
			rights[183] = new OneRight("UnFreezeUser",183,"�ⶳ�ʻ���ť");
			rights[184] = new OneRight("UnLockTradeList",184,"�������׵���ť");
            rights[195] = new OneRight("CFTUserPick", 195, "�Ƹ�ͨ�������ߴ���");//һ��Ȩ�� �κν������
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
            rights[212] = new OneRight("UnFreezeChannelBDBP", 212, "������ƭ����");
            rights[213] = new OneRight("UnFreezeChannelTX", 213, "���ֶ���");
            rights[214] = new OneRight("UnFreezeChannelSD", 214, "��Ķ���");

            rights[200] = new OneRight("ModifyFundPayCard", 200, "���ͨ��ȫ�����");
            rights[201] = new OneRight("ModifyFundPayCardByCS", 201, "���ͨ��ȫ�����(�ͷ�)");
            rights[202] = new OneRight("InternetBankRefund", 202, "�����ύ�����˿�");
            rights[207] = new OneRight("PayBusinessCMD", 207, "�޸���ϵ���ֻ�");
            rights[193] = new OneRight("RefundCheck", 193, "�˿�Ǽ�");

            rights[203] = new OneRight("RefundMerchantCheck", 203, "�˿��̻�¼��");
            rights[194] = new OneRight("BatchCancellation", 194, "����ע��");
			//���Ӵ��۵�����ťȨ��
            rights[198] = new OneRight("DKAdjust", 198, "���۵���״̬");

            rights[169] = new OneRight("RealNameCertification",169,"ʵ����֤����������");

            rights[170] = new OneRight("IceOutPPSecurityMoney", 170, "�ⶳ���ı�֤�𶩵�");
            rights[171] = new OneRight("LCTMenu", 171, "���ͨ�˵�");
            rights[172] = new OneRight("HandQMenu", 172, "��Q֧���˵�");
            rights[173] = new OneRight("CreditQueryMenu", 173, "���ÿ�����˵�");
            rights[174] = new OneRight("FastPayMenu", 174, "���֧���˵�");
            rights[175] = new OneRight("TencentbusinessMenu", 175, "����˾ҵ��˵�");


            rights[196] = new OneRight("SensitiveRole", 196, "����Ȩ��");
            rights[206] = new OneRight("IDCardManualReview_ReviewCount", 206, "���֤Ӱӡ�������쵥Ȩ��");
            rights[210] = new OneRight("IDCardManualReview_SeeDetail", 210, "���֤Ӱӡ���鿴����Ȩ��");
            rights[211] = new OneRight("ChangeUserInfo", 211, "������Ϣ");

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
