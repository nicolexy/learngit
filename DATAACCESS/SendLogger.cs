using System;
using System.Configuration;
using System.Web.Mail;

namespace TENCENT.OSS.CFT.KF.DataAccess
{
	/// <summary>
	/// SendLogger 的摘要说明。
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
					log.Error(operID + "调用发送日志邮件功能");
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
				log.Error("停止调用发日志送邮件功能");

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
//							log.Error("发送客服日志邮件失败:" + errMsg);
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
//					log.Error("发送客服日志邮件失败:" + ex.Message);
//
//				return false;
//			}
//			/*
//			finally
//			{
//				log4net.ILog log = log4net.LogManager.GetLogger("DataAccess.SQL");
//				if(log.IsErrorEnabled)
//					log.Error("调用发日志送邮件功能");
//			}
//			*/
//		}


//		public static bool sendMail(string mailToStr,string mailFromStr,string subject,string content,string type,out string Msg)  //发送邮件
//		{
//			Msg = null;
//			try
//			{
//				TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend newMail=new TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend();
//				newMail.SendMail(mailToStr,"",subject,content,true,null);
////				MailMessage mail = new MailMessage();
////				mail.From = mailFromStr;        //发件人
////				mail.To   = mailToStr;          //收件人
////				//mail.BodyEncoding = System.Text.Encoding.Unicode;
////				mail.BodyFormat = MailFormat.Html;
////				mail.Body = content; //邮件内容
////				mail.Priority = MailPriority.High; //优先级
////				mail.Subject  = subject;           //邮件主题
////
////				//SmtpMail.SmtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString(); //"192.168.1.27";  邮件服务器地址
////			
////				if (type == "inner")
////				{
////					SmtpMail.SmtpServer = null;
////					//SmtpMail.SmtpServer.Insert(0,ConfigurationManager.AppSettings["smtpServer"].ToString().Trim());	
////					SmtpMail.SmtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString(); //"192.168.1.27";  邮件服务器地址
////					SmtpMail.Send(mail);
////					SmtpMail.SmtpServer = null;
////					
////				}
////				else if(type == "out")  //外部邮箱
////				{
////					SmtpMail.SmtpServer = null;
////					//SmtpMail.SmtpServer.Insert(0,ConfigurationManager.AppSettings["OutSmtpServer"].ToString().Trim());	
////					SmtpMail.SmtpServer = ConfigurationManager.AppSettings["OutSmtpServer"].ToString();
////					SmtpMail.Send(mail);
////					SmtpMail.SmtpServer = null;
////				}
////				else
////				{
////					Msg  = "邮件类型错误！ 请检查！只能为inner或out。";
////					return false;
////				}
//
//				return true;
//			}
//			catch(Exception er)
//			{
//				Msg = er.Message.ToString().Replace("'","’");
//				return false;
//			}
//		}
	}
}
