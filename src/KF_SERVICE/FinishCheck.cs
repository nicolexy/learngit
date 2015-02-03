using System;

using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.C2C.Finance.BankLib;

namespace TENCENT.OSS.CFT.KF.KF_Service
{
	/// <summary>
	/// FinishCheck ��ժҪ˵����
	/// </summary>
	public class FinishCheck
	{
		public FinishCheck()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}



		/// <summary>
		/// ִ������(������������ִ������)
		/// </summary>
		/// <param name="strCheckID">����ID</param>
		/// <param name="myHeader">SOAPͷ</param>
		/// <param name="da">���ݷ��ʶ���</param>
		/// <returns>�Ƿ�ɹ�</returns>
		public static bool ExecuteCheck(string strCheckID, Finance_Header myHeader, MySqlAccess da)
		{
			myHeader.UserName = myHeader.UserName.Trim().ToLower();

			string strCheckType = GetCheckType(strCheckID);

			if(!PublicRes.IgnoreLimitCheck)
			{
				//furion 20060208 ��Ҫ����һ��Ԥ�ȼ��,�������ó�ִ����״̬�Ա����ظ�ִ��.
				if(!PriorCheck(strCheckID,da))
				{				
					throw new LogicException("������״̬����ȷ,��ȷ���Ƿ����ظ�ִ��");
				}
			}
			//�������

			bool flag = false;
			string errmsg = "";

			try
			{
				flag = CheckFactory.ReturnHandler(strCheckType).DoAction(strCheckID,myHeader) ;
			}
			catch(Exception err)
			{
				errmsg = err.Message;
				flag = false;
			}

			if(!flag)
			{
				LastCheck(strCheckID,da);
				throw new LogicException("ִ��������ɺ�Ķ���ʱʧ�ܣ�" + errmsg);
			}
			else
			{
				if(FinishedCheck(strCheckID,da))
				{
					return true;
				}
				else
				{
					throw new LogicException("ִ��������ɺ�Ķ���ʱ�ɹ���������ִ�и�������״̬ʱʧ�ܣ�");
				}
			}
		}


		/// <summary>
		/// ���ִ��������ɶ������ɹ����ٸĻ�ԭ״̬
		/// </summary>
		/// <param name="strCheckID"></param>
		/// <param name="da"></param>
		private static void LastCheck(string strCheckID, MySqlAccess da)
		{
			
			//da.ExecSql("update c2c_fmdb.t_check_main set FState='finish' where FID=" + strCheckID);
			da.ExecSql("update c2c_fmdb.t_check_main set FNewState=2 where FID=" + strCheckID);
			
		}



		/// <summary>
		/// ��������Ϊ��ִ��״̬
		/// </summary>
		/// <param name="strCheckID">����ID</param>
		/// <param name="da">���ݷ��ʶ���</param>
		/// <returns>�Ƿ�ɹ�</returns>
		public static bool FinishedCheck(string strCheckID,MySqlAccess da)
		{
			//return da.ExecSql("update c2c_fmdb.t_check_main set FState='finished' where FID=" + strCheckID);
			return da.ExecSql("update c2c_fmdb.t_check_main set FNewState=3 where FID=" + strCheckID);
		}



		/// <summary>
		/// Ϊ�˲����ִ�У��Ȱ�״̬�ĳ�һ��ִ���С�����
		/// </summary>
		/// <param name="strCheckID"></param>
		/// <param name="da"></param>
		/// <returns></returns>
		private static bool PriorCheck(string strCheckID, MySqlAccess da)
		{
			//string strSql = "select FState from c2c_fmdb.t_check_main where FID=" + strCheckID;
			string strSql = "select FNewState from c2c_fmdb.t_check_main where FID=" + strCheckID;
			string state = da.GetOneResult(strSql).Trim();
			if(state != "2")
			{
				return false;
			}
			else
			{
				//strSql = "update c2c_fmdb.t_check_main set FState='execute' where FID=" + strCheckID;
				strSql = "update c2c_fmdb.t_check_main set FNewState=9 where FID=" + strCheckID;
				return da.ExecSqlNum(strSql) == 1;
			}
		}


		private static string GetCheckType(string strCheckID)
		{
			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
			try
			{
				da.OpenConn();
				return da.GetOneResult("select FCheckType from c2c_fmdb.t_check_main where FID=" + strCheckID);
			}
			finally
			{
				da.Dispose();
			}
		}


		/// <summary>
		/// ʵ���������Ľӿڡ�
		/// </summary>
		public interface IBaseDoCheck
		{
			bool DoAction(string strCheckID, Finance_Header myHeader);
		}

		
		
		/// <summary>
		/// ���������ࡣ
		/// </summary>
		public class CheckFactory
		{
			//		public enum ActionShow
			//		{
			//			allHide = 0,
			//			czcz01  = 1, //��ֵ����
			//			czjy02  = 2, //֧���ɹ�����
			//			czfk03  = 3, //���ȷ�ϳ���.
			//			cztk04  = 4, //�˿����.
			//			czcz05  = 5, //��ֵ��������.
			//			cztx06  = 6, //���ֳ�������.
			//			czhd07  = 7, //�������ص�����
			//			czzz08  = 8, //ת�ʳ�����
			//			allshow = 9
			//		}

			/// <summary>
			/// �����������ͻ�ȡ������
			/// </summary>
			/// <param name="strCheckType">��������</param>
			/// <returns>������</returns>
			public static IBaseDoCheck ReturnHandler(string strCheckType)
			{
				switch(strCheckType)
				{
						//���µ���������ʱ���Ѵ�����������д�ã�Ȼ����뵽����ķ�֧���С�
						/*
					case "batchpay" :
						return new PayCheck();  //û�漰���ʽ���ˮ��
						break;
					case "batchrefund" :
						return new BatchRefundCheck();  //û�漰���ʽ���ˮ��
						break;
					case "fmcz01":
						return new fmCheck();  //FinanceAdjust Check    //�ʽ���ˮ(�׳���)
						break;
					case "fmjy02":
						return new fmCheck();  //FinanceAdjust Check    //�ʽ���ˮ(�׳���)
						break;
					case "fmfk03":
						return new fmCheck();  //FinanceAdjust Check    //�ʽ���ˮ(�׳���)
						break;
					case "fmtk04":
						return new fmCheck();  //FinanceAdjust Check    //�ʽ���ˮ(�׳���)
						break;
					case "fmhd05":  //�ص�������ʧ��-���ɹ���  ���ɹ�-��ʧ�ܣ� ��δ���
						return new fmCheck();    //�ʽ���ˮ(�׳���)
						break;
					case "czcz01": //��ֵ������
						return new czCheck();   //û�漰���ʽ���ˮ��
						break;
					case "czjy02": //֧���ɹ�������
						return new czCheck();   //û�漰���ʽ���ˮ��
						break;
					case "czfk03": //���ȷ�ϳ�����
						return new czCheck();   //û�漰���ʽ���ˮ��
						break;
					case "cztk04": //�˿����
						return new czCheck();   //û�漰���ʽ���ˮ��
						break;
					case "czcz05": //���ڳ�ֵ����
						return new czCheck();   //û�漰���ʽ���ˮ��
						break;
					case "cztx06": //�������ֳ�����
						return new czCheck();   //û�漰���ʽ���ˮ��
						break;
					case "czhd07": //�������ص�������
						return new czCheck();   //û�漰���ʽ���ˮ��
						break;
					case "czzz08": //ת�ʳ�����
						return new czCheck();   //û�漰���ʽ���ˮ��
						break;
						*/
					case "Mediation": //�ٲ��ж�(�޸�������Ϣ)
						return new Mediation();   //û�漰���ʽ���ˮ��
						/*
					case "mcTrans":
						return new mcTrans();    //�����ʽ���ˮ���޸Ľ�С
					case "ChangeQQ" :
						return new ChangeQQClass();   //û�漰���ʽ���ˮ��
					case "NewMedi" :
						return new NewMediClass();    //û�漰���ʽ���ˮ��
					case "ModifyMedi" :
						return new NewMediClass();    //û�漰���ʽ���ˮ��
					case "NewCoinPub" :
						return new NewCoinPubClass();   //û�漰���ʽ���ˮ��
					case "ModifyCoinPub" :
						return new NewCoinPubClass();   //û�漰���ʽ���ˮ��
					case "settle" :
						return new SettleClass();   //�����ʽ���ˮ���޸Ľ�С
					case "customSave" :                   //�ֹ���ֵ
						return new customSaveClass();   //�����ʽ���ˮ���޸Ľ�С
					case "returnticket" :                   //��Ʊ
						return new returnticketClass();
					case "otherAdjust" :
						return new otherAdjustClass();

						//Ϊ�˺;ɰ汾������Ӧ,��Ҫ�½������µ���������,�����ٰ����ɾ����.
					case "batchexception" :    //�����쳣ֱ�Ӵ���
					case "Subbatchexception" :    //���ʻ������쳣ֱ�Ӵ���
						return new batchexceptionClass();
					case "refundexception" :
						return new refundexceptionClass();//�����쳣ֱ���˿�
					case "twoexception" :
						return new twoexceptionClass();  //��������
					case "Subacctwoexception" :
						return new SubacctwoexceptionClass();//���ʻ���������   rowenawu   20101023
					case "bankexception" :
						return new bankexceptionClass();

					case "tokenAdjust" :
						return new tokenAdjustClass();     //����ȯ����
					case "realTimeAdjust" :
						return new realTimeAdjustClass(); 
					case "mannalFetch" :
						return new mannalFetchClass();    //�ֹ�����

					case "mannalRefund" :
						return new mannalRefundClass();    //�ֹ��˵�

					case "HaoYe" :
						return new HaoYeCheck();
					case "proxyFetch" :
						return new proxyFetchClass();
					case "manualTransfer" :
						return new manualTransferClass();
					case "C2CmanualTransfer":
						return new manualTransferClass(); //C2C�ֹ�ת��
					case "b2creturn" :
						return new B2CReturnClass();
					case "logonUser" :
						return new logonUserClass();
					case "batchsettle":
						return new batchsettleClass();
					case "b2ctoerror" :
						return new b2ctoerrorClass();
					case "mannallist" :
						return new MannalListClass();
					case "changebankinfo" :
						return new ChangeBankInfoClass();
					case "directpayfail" :
						return new directpayfailClass();

						//�¼�����ֱ���˿�ʧ������
					case "banktofault" :
						return new banktofaultClass();

					case "givebackcheck" :
						return new givebackcheckClass();
					case "quicktraderefund":
						return new QuickTradeClass();
					case "sporderrefund":
						return new SporderRefundClass();
					case "spordercancel":
						return new SporderCancel();

					case "remitfetch"://�ʴ������������
						return new RemitFetchClass();
					case "remitrefund"://�ʴ�����˿�����
						return new RemitRefundClass();
					case "remitmodify"://�ʴ�������״̬����
						return new RemitModifyClass();
					case "remitcancel"://�ʴ���������������
						return new RemitCancelClass();
					case "tobanksucc":
						return new RefundBankSucc();
					case "tomanualsucc":
						return new RefundManualSucc();
					case "topaysucc":
						return new RefundPaySucc();
					case "failtowait":
						return new RefundFailToWait();//�˿�ʧ�ܵ����˿�״̬
					case "batmanualcz":
						return new BatManualCZ();  //���˺������ֹ���ֵ

					case "BankTreasurer"://�����ʽ����  rowena  20080821
						return new BankTreasurerClass();

					case "CZToRefund"://��ֵת�˿�  rowena  20090921
						return new CZToRefundClass();

					case "RefundToQuickTrade"://�ᶨ���������˿�ת��ֵ֧��  rowena  20100909
						return new RefundToQuickTradeClass();

					case "batchRefundAdjust"://�����˿����
						return new BatchadRefundAdjust();

					case "Autotwoexception":
						return new AutotwoexceptionClass(); //ϵͳ�Զ���������
					case "RefundFailHand":
						return new RefundFailHandClass(); //�˵��쳣����
					case "AdjustOtherRefund":
						return new AdjustOtherRefundClass(); //�ֹ������˿��쳣��������״̬
					case "AdjustRefundRecord":
						return new AdjustRefundRecordClass(); //�˿���ܱ���״̬����
					case "batchc2ctransfer":
						return new BatchC2CTransfer();//C2C����ת��
					case "BatchLogonUser":
						return new BatchLogonUser();//�����û�ע��
					case "ConfigExportTaskComm":
						return new ConfigExportTaskCommClass();//�������޸�ͨ�û����������ñ�
					case "ConfigExportBank":
						return new ConfigExportBankClass();//�������޸ĸ��������������
					case "ConfigExportTaskTmp":
						return new ConfigExportTaskTmpClass();//�������޸���ʱ����������ʱ��
					case "ConfigExportBankRest":
						return new ConfigExportBankRestClass();//�������޸ķǸ�������ʱ��
					case "subaccczpayexception":
						return new subaccczpayexception();//���˻���ֵ����
				
					case "subacctxpayexception":
						return new subacctxpayexception();//���˻����ֵ���
					case "ConfigBankType":
						return new ConfigBankTypeClass();//���б�������
					case "batchrefundapply":
						return new BatchRefundApplyClass();//b2c/fastpay/���ʻ������˿�
					case "CopyRights":
						return new CopyRightsClass();//��������ϵͳȨ�� Andrew
					case "RecoverQQ":
						return new RecoverQQClass();//�ָ�QQ�ʺ� Andrew

					case "directPayBankConfig":
						return new DirectPayBankConfigClass();//ֱ��ϵͳ��������
					case "directPayTaskCheck":
						return new DirectPayTaskCheckClass();//ֱ��ϵͳԼ������
					case "directPayTaskModel":
						return new DirectPayTaskModelClass();//ֱ��ϵͳ����ģ��
					case "directPayTaskExec":
						return new DirectPayTaskExecClass();//ֱ��ϵͳִ������

					case "SpecialTaskExec":
						return new SpecialTaskExecClass();//������������
					case "CNBankTreasurer":
						return new CNBankTreasurerClass();//�����ʽ����  rowenawu
					case "codcancel":
						return new CODCancelClass();//��������������� Andrew
					case "codtransfer":
						return new CODTransferClass();//��������ת�� Andrew
					case "HongBaoSend":
						return new HongBaoSendClass();//�������
					case "PosResultCheck":
					case "PosResultRefund":
						return new PosResultCheckClass();//POS���˽������
					case "ConfigBankBulletin":
						return new ConfigBankBulletinClass();//����ά����������  rowenawu
					case "gwqSaveOper":
						return new GWQSaveOperClass();//����ȯ��ֵ����
					case "gwqFetchOper":
						return new GWQFetchOperClass();//����ȯ������������
					case "gwqCancelOper":
						return new GWQCancelOperClass();//����ȯ��������
					case "SubRefund":
						return new SubRefundClass();//���ʻ��˿�  rowenawu
					case "UploadCancel": 
						return new UploadCancelClass();//�������� rowenawu
					case "ConfigYDTBankType":
						return new ConfigYDTBankTypeClass();//����һ��ͨ������Ϣ rowenawu
						*/
					default :
						return null;
						break;
				}
			}
		}


		public class Mediation : IBaseDoCheck
		{
			#region IBaseDoCheck ��Ա
		
			public bool DoAction(string strCheckID, Finance_Header myHeader)
			{
				return false;

				Finance_Manage fm = new Finance_Manage();
				Finance_Header fh = new Finance_Header();
				fh.UserName = myHeader.UserName;
				fh.UserIP   = myHeader.UserIP;
			
				fm.myHeader = fh;

				//�޸�����service
				string Msg = null;

				string strFetchNo = "select fobjID from c2c_fmdb.t_check_main where fid = '" + strCheckID + "'";
				string fetchID = PublicRes.ExecuteOne(strFetchNo,"ht");

				string [] br = new string[3]; 
				string strCheckInfo = "select FcheckUser,FcheckMemo,FcheckTime from c2c_fmdb.t_check_list where fcheckid = '" + strCheckID + "'";
				br[0] = "FcheckUser";
				br[1] = "FcheckMemo";
				br[2] = "FcheckTime";
				br = PublicRes.returnDrData(strCheckInfo,br,"ht");

				string checkUsr = br[0];
				string checkMemo= br[1];
				string checkTime= br[2];

				string [] ar = new string [9];
				string str = "Select * from c2c_fmdb.t_mediation where FfetchID='" + fetchID + "'";
				ar[0] = "Fqqid";
				ar[1] = "FfetchMail";
				ar[2] = "fcleanMibao";
				ar[3] = "freason";
				ar[4] = "FBasePath";
				ar[5] = "FAccPath";
				ar[6] = "FIDCardPath";
				ar[7] = "FbankCardPath";
				ar[8] = "FNameNew";

				ar = PublicRes.returnDrData(str,ar,"ht");

				string qqid = ar[0];
				string mail = ar[1];
				string cleanMimao = ar[2];
				string reason   = ar[3];
				string pathBase = ar[4];
				string pathAcc  = ar[5];
				string pathIDCard = ar[6];
				string pathBank   = ar[7];
				string changedName= ar[8];


				bool opSign;

				//�˴�Ӧ���ýӿ���ʵ�֣��ṹ��������
				if(fetchID.Substring(0,3) == "101")  //�޸�����
				{
					//�޸�������service
					opSign = fm.modifyName(qqid,changedName,"");
				
					string mailFrom = null;
					try
					{
						mailFrom = System.Configuration.ConfigurationManager.AppSettings["OutMailFrom"].ToString().Trim();	
					}
					catch
					{
						Msg = "��ȡ�ʼ�������ʧ�ܣ� ����Service��Webconfig�ļ��� mailFrom�Ƿ���ڣ�";
						return false;
					}

					bool mailSign = PublicRes.sendMail(mail,mailFrom,"�Ƹ�ͨ���������Ƹ�ͨ�����޸ĳɹ���","���ã� ��������޸������Ѿ�ͨ���� ������Ϊ" + changedName +" .��������½�Ƹ�ͨ���в鿴��","out",out Msg);
				
					if (mailSign == false)
					{
						throw new Exception(Msg);
					}

				}
				else
				{
					opSign = fm.changePwdInfo(qqid,mail,cleanMimao,reason,pathBase,pathAcc,pathIDCard,pathBank,out Msg); 	
				}

				if (opSign == false)
				{
					throw new Exception(Msg);
					return false;	
				}
				else //������ĳɹ���������ٲñ�������Ϣ
				{	
					MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));
					da.OpenConn();
					da.ExecSql("START TRANSACTION;");  //��ʼһ������

					try
					{
						string upStr = "update c2c_fmdb.t_mediation set FcheckUid ='" + checkUsr + "',FcheckTime = '" + checkTime + "',FcheckMemo = '" + checkMemo + "' where FfetchID='" + fetchID + "'";  //update c2c_fmdb.t_check_main set FState='finished' where FID=" + strCheckID
				
						da.ExecSql(upStr);
						//concat(FcleanMimao, "1212")
						string upParam = "update c2c_fmdb.t_check_param set Fvalue = concat(Fvalue, " + "'&checkUsr="
							+ checkUsr + "&FcheckTime=" + checkTime + "&FcheckMemo=" + checkMemo +"')" + "  where FcheckID ='" + strCheckID + "' and Fkey = 'returnUrl'";

						da.ExecSql(upParam);

						da.ExecSql("COMMIT;");	

						return true;
					}
					catch(Exception e)
					{
						da.ExecSql("ROLLBACK;");
						return false;
					}
				}
			}


			#endregion
		}



	}
}
