using System;
using System.Configuration;
using System.Web.Mail;

namespace TENCENT.OSS.CFT.KF.DataAccess
{
	/// <summary>
	/// SendLogger ��ժҪ˵����
	/// </summary>
	public class SendLogger
	{
		private SendLogger(){}

		private static string[] PowerUser = new string[]{"alexguan","1100000000"};
		private static string[] loggerReceiver = new string[]{ "alexguan@tencent.com" };
		private static string[] loggerSender = new string[] {"alexguan@tencent.com"};

		private static string strMailContent = "";
		
		private static SendLogger m_instance = null;


		private static bool CheckUserPower(string operID)
		{
			foreach(string str in PowerUser)
			{
				if(operID.Trim() == str)
					return true;
			}

			return false;
		}


		public static bool InitInstance(string operID)
		{
			try
			{
				if(operID == null || operID.Trim() == "")
					return false;

				operID = operID.Trim();

				if(CheckUserPower(operID))
				{
					if(m_instance == null)
						m_instance = new SendLogger();
				}
				else
				{
					return false;
				}

				m_instance.m_isCatchLog = true;

				return true;
			}
			catch (System.Exception ex)
			{
				return false;
			}
			finally
			{
				log4net.ILog log = log4net.LogManager.GetLogger("DataAccess.SQL");
				if(log.IsErrorEnabled)
					log.Error(operID + "���÷�����־�ʼ�����");
			}
			
		}

		public static SendLogger GetInstance()
		{
			return m_instance;
		}
		

		private bool m_isCatchLog = false;

		public bool IsCatchLog
		{
			get { return m_isCatchLog; }
			set { m_isCatchLog = value; }
		}


		public bool AddLog(string logContent)
		{
			if(!m_isCatchLog)
				return false;

			strMailContent += "<br/>" + logContent;

			return true;
		}


		public bool EndSendLog()
		{
			m_isCatchLog = false;
			m_instance = null;
			strMailContent = "";

			log4net.ILog log = log4net.LogManager.GetLogger("DataAccess.SQL");
			if(log.IsErrorEnabled)
				log.Error("ֹͣ���÷���־���ʼ�����");

			return true;
		}


//		public bool SendLog(bool stopCatchLog)
//		{
//			if(!m_isCatchLog)
//				return false;
//
//			string errMsg = "";
//			try
//			{
//				foreach(string strReceiver in loggerReceiver)
//				{
//					bool bResult = sendMail(strReceiver,loggerSender[0],"KF Log",strMailContent,"inner",out errMsg);
//					if(!bResult)
//					{
//						log4net.ILog log = log4net.LogManager.GetLogger("DataAccess.SQL");
//						if(log.IsErrorEnabled)
//							log.Error("���Ϳͷ���־�ʼ�ʧ��:" + errMsg);
//					}
//				}
//
//				strMailContent = "";
//
//				if(stopCatchLog)
//				{
//					EndSendLog();
//				}
//				return true;
//			}
//			catch (System.Exception ex)
//			{
//				log4net.ILog log = log4net.LogManager.GetLogger("DataAccess.SQL");
//				if(log.IsErrorEnabled)
//					log.Error("���Ϳͷ���־�ʼ�ʧ��:" + ex.Message);
//
//				return false;
//			}
//			/*
//			finally
//			{
//				log4net.ILog log = log4net.LogManager.GetLogger("DataAccess.SQL");
//				if(log.IsErrorEnabled)
//					log.Error("���÷���־���ʼ�����");
//			}
//			*/
//		}


//		public static bool sendMail(string mailToStr,string mailFromStr,string subject,string content,string type,out string Msg)  //�����ʼ�
//		{
//			Msg = null;
//			try
//			{
//				TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend newMail=new TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend();
//				newMail.SendMail(mailToStr,"",subject,content,true,null);
////				MailMessage mail = new MailMessage();
////				mail.From = mailFromStr;        //������
////				mail.To   = mailToStr;          //�ռ���
////				//mail.BodyEncoding = System.Text.Encoding.Unicode;
////				mail.BodyFormat = MailFormat.Html;
////				mail.Body = content; //�ʼ�����
////				mail.Priority = MailPriority.High; //���ȼ�
////				mail.Subject  = subject;           //�ʼ�����
////
////				//SmtpMail.SmtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString(); //"192.168.1.27";  �ʼ���������ַ
////			
////				if (type == "inner")
////				{
////					SmtpMail.SmtpServer = null;
////					//SmtpMail.SmtpServer.Insert(0,ConfigurationManager.AppSettings["smtpServer"].ToString().Trim());	
////					SmtpMail.SmtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString(); //"192.168.1.27";  �ʼ���������ַ
////					SmtpMail.Send(mail);
////					SmtpMail.SmtpServer = null;
////					
////				}
////				else if(type == "out")  //�ⲿ����
////				{
////					SmtpMail.SmtpServer = null;
////					//SmtpMail.SmtpServer.Insert(0,ConfigurationManager.AppSettings["OutSmtpServer"].ToString().Trim());	
////					SmtpMail.SmtpServer = ConfigurationManager.AppSettings["OutSmtpServer"].ToString();
////					SmtpMail.Send(mail);
////					SmtpMail.SmtpServer = null;
////				}
////				else
////				{
////					Msg  = "�ʼ����ʹ��� ���飡ֻ��Ϊinner��out��";
////					return false;
////				}
//
//				return true;
//			}
//			catch(Exception er)
//			{
//				Msg = er.Message.ToString().Replace("'","��");
//				return false;
//			}
//		}
	}
}
