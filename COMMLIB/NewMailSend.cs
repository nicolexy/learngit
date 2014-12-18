using System;
using System.Data; 
using System.Configuration;

namespace TENCENT.OSS.C2C.Finance.Common.CommLib
{
	/// <summary>
	/// NewMailSend 的摘要说明。
	/// </summary>
	public class NewMailSend
	{
		private string subject;//主题 
		public string Subject
		{
			set{this.subject = value;}
		}

		private string body;//文本内容
		public string Body
		{
			set{this.body = value;}
		}

		private string htmlbody;//超文本内容 
		public string HtmlBody
		{
			get{return htmlbody;}
			set{this.htmlbody = value;}
		}

		private string from;//发件人地址 
		//		public string From
		//		{
		//			set{this.from = value;}
		//		}

		private string to;//收件人地址 
		public string To
		{
			set{this.to = value;}
		}

		private string cc;//抄送人地址 
		public string CC
		{
			set{this.cc = value;}
		}
		private string bcc;//暗（密）送 
		public string BCC
		{
			set{this.cc = value;}
		}
		private string content_type;//邮件类型
		public string Content_Type
		{
			set{this.content_type = value;}
		}

		public DataTable filelist;//附件列表　

		private int priority;//邮件优先级
		public int Priority
		{
			set{this.priority = value;}
		}


		CDO.ConfigurationClass conf=new CDO.ConfigurationClass();
		

		string sendPassword="";
		string SMTPServer="";
		int serverPort=25 ;

		public NewMailSend()
		{
			try
			{
				filelist=new DataTable();//已定义变量，初始化操作

				filelist.Columns.Add(new DataColumn("URL",typeof(string)));//文件名 
				filelist.Columns.Add(new DataColumn("UserName",typeof(string)));//文件名 
				filelist.Columns.Add(new DataColumn("Password",typeof(string)));//文件名 

				from=ConfigurationManager.AppSettings["EmailAddress"].ToString();
				sendPassword=ConfigurationManager.AppSettings["EmailPassword"].ToString();
				SMTPServer=ConfigurationManager.AppSettings["SMTPServer"].ToString();
				serverPort=Convert.ToInt32(ConfigurationManager.AppSettings["SMTPServerPort"].ToString());

				conf.Fields[CDO.CdoConfiguration.cdoSendUsingMethod].Value =CDO.CdoSendUsing.cdoSendUsingPort;
				conf.Fields[CDO.CdoConfiguration.cdoSendEmailAddress].Value=from;
				conf.Fields[CDO.CdoConfiguration.cdoSendPassword].Value=sendPassword;
				conf.Fields[CDO.CdoConfiguration.cdoSMTPServer].Value=SMTPServer;
				conf.Fields[CDO.CdoConfiguration.cdoSendUserName].Value=from;
				conf.Fields[CDO.CdoConfiguration.cdoSMTPServerPort].Value=serverPort;
				conf.Fields[CDO.CdoConfiguration.cdoSMTPAuthenticate].Value=CDO.CdoProtocolsAuthentication.cdoBasic;
				conf.Fields.Update();
			}
			catch(Exception ex)
			{
				log4net.ILog log = log4net.LogManager.GetLogger("初始化NewMailSend异常");
				if(log.IsErrorEnabled) log.Error(ex.Message);
			}
		}

		public void Send()
		{
			try
			{
				CDO.MessageClass mail=new CDO.MessageClass();
				mail.Configuration=conf;

				
				mail.To=to;
				mail.From=from;
				mail.Subject=subject;

				if(body!=null&&body!="")
				{
					mail.TextBody=body;
					mail.TextBodyPart.Charset="UTF-8";
				}
				if(cc!=null&&cc!="")
				{
					mail.CC=cc;
				}
				if(bcc!=null&&bcc!="")
				{
					mail.BCC=bcc;
				}
				if(htmlbody!=null&&htmlbody!="")
				{
					mail.HTMLBody=htmlbody;
					mail.HTMLBodyPart.Charset="UTF-8";
				}
				mail.Charset="UTF-8";

				foreach(DataRow dr in filelist.Rows)
				{
					mail.AddAttachment(dr["URL"].ToString(),dr["UserName"].ToString(),dr["Password"].ToString());
				}
				
				mail.Send();
			}
			catch(Exception ex)
			{
				log4net.ILog log = log4net.LogManager.GetLogger("通过CDO发送邮件异常");
				if(log.IsErrorEnabled) log.Error(ex.Message);
			}
		}


		public  void SendMail(string mailToStr,string mailccStr,string mailSubject,string mailBody,bool htmlFormat,string[] fileAttachment )
		{
			try
			{

				string newMailTo="";
				if(mailToStr!=null&&mailToStr!="")
				{
					string[] mainsTo=mailToStr.Split(';');
					foreach(string oneMailTo in mainsTo)
					{
						if(oneMailTo.Trim()!=""&&oneMailTo.Trim().IndexOf("@")<0)
						{
							newMailTo+=oneMailTo.Trim()+"@tencent.com;";
						}
						else if(oneMailTo.Trim()!="")
						{
							newMailTo+=oneMailTo+";";
						}
					}
				}
				to=newMailTo;

				if(mailccStr!=null&&mailccStr.Trim()!="")
				{
					string newMailCC="";
				
					string[] mainsCC=mailccStr.Split(';');
					foreach(string oneMailCC in mainsCC)
					{
						if(oneMailCC.Trim()!=""&&oneMailCC.Trim().IndexOf("@")<0)
						{
							newMailCC+=oneMailCC.Trim()+"@tencent.com;";
						}
						else if(oneMailCC.Trim()!="")
						{
							newMailCC+=oneMailCC+";";
						}
					}
					cc=newMailCC;
				}
			
				subject=mailSubject;
				if(htmlFormat)
				{
					htmlbody=mailBody;
				}
				else
				{
					body=mailBody;
				}

				if(fileAttachment!=null&&fileAttachment.Length>0)
				{
					filelist.Clear();
					foreach(string oneFile in fileAttachment)
					{
						DataRow newDr=filelist.NewRow();
						newDr["URL"]=oneFile;
						filelist.Rows.Add(newDr);

					}
				}

				Send();
			}
			catch(Exception ex)
			{
				log4net.ILog log = log4net.LogManager.GetLogger("通过CDO的SendMail发送邮件异常");
				if(log.IsErrorEnabled) log.Error(ex.Message);
			}
		}
	}
}
