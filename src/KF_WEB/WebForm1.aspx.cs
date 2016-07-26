using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using System.Web.Mail;
using System.IO;

namespace TENCENT.OSS.CFT.KF.KF_Web
{
	/// <summary>
	/// WebForm1 的摘要说明。
	/// </summary>
    public partial class WebForm1 : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string pp ="appealtime=1354086510&bargainor_id=1000000000&chv=9&cmd=update&newmobile=15013490149&sp_id=1000000000&status=0&uid=112407122&uin=2760807&key=Adf^#KK12D";
			string kk = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pp,"md5").ToLower();
			string tt = kk;
//			string time_stamp = TimeTransfer.GetTickFromTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
//			string inmsg = "uin=" + "123456789";
//			inmsg += "&optype=" + "1";
//			inmsg += "&uid=" + "987654321";
//			inmsg += "&time_stamp=" + time_stamp;
//			inmsg += "&sign=" + System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("9876543211" + time_stamp + "123456","md5").ToLower();
//
//			string reply;
//			short sresult;
//
//			string msg="";
//
//			if(TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("mbtoken_unbind_service",inmsg,true,out reply,out sresult,out msg))
//			{
//				if(sresult != 0)
//				{
//					msg =  "mbtoken_unbind_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
//				
//				}
//				else
//				{
//					if(reply.StartsWith("result=0"))
//					{
//					}
//					else
//					{
//						msg =  "mbtoken_unbind_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
//					
//					}
//				}
//			}
//			else
//			{
//				msg = "mbtoken_unbind_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
//				
//			}
//			// 在此处放置用户代码以初始化页面
		}
		private string getCgiString(string instr)
		{
			if(instr == null || instr.Trim() == "")
				return "";
			
			//System.Text.Encoding enc = System.Text.Encoding.GetEncoding("GB2312");
			return System.Web.HttpContext.Current.Server.UrlDecode(instr).Replace("\r\n","").Trim()
				.Replace("%3d","=").Replace("%20"," ").Replace("%26","&").Replace("%7c","|");
		}


		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion


		protected void Button1_Click(object sender, System.EventArgs e)
		{
			
				try
				{
					//外发邮件
//					System.Web.Mail.MailMessage mail = new System.Web.Mail.MailMessage();
//					mail.From = System.Configuration.ConfigurationManager.AppSettings["OutMailFrom"].ToString();        
//					mail.To   = "bruceliao@tencent.com;bruceliao@tencent.com;";
//					mail.BodyFormat = System.Web.Mail.MailFormat.Html;
//					mail.Body =GetEmalContentFK(2536,"申请关闭商户支付权限","bruceliao"); 
//					mail.Subject  = "申请关闭商户支付权限，请风控审核";  
//					System.Web.Mail.SmtpMail.SmtpServer = System.Configuration.ConfigurationManager.AppSettings["OutSmtpServer"].ToString(); 
//					System.Web.Mail.SmtpMail.Send(mail);

					
				}
				catch
				{
				
				}			

			}

		//获取银行信息修改邮件模板
		private string GetEmalContentFK(int maskid,string Type,string UserName)
		{
			string filename = System.Configuration.ConfigurationManager.AppSettings["ServicePath"].Trim();
			if(!filename.EndsWith("\\"))
				filename += "\\email_amend_fk.htm";　
			System.IO.StreamReader sr = new System.IO.StreamReader(filename,System.Text.Encoding.GetEncoding("GB2312"));
			try
			{
				string content = sr.ReadToEnd();

				return String.Format(content,"bruceliao",DateTime.Now.ToString("yyyy-MM-dd"),"申请关闭商户支付权限","http://cft-spoa.oss.com" + "/spoaweb/self/amend_msp_check.aspx?mode=check_fk&id=2536").Replace("[","{").Replace("]","}");
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
			finally
			{
				sr.Close();
			}
		}

		private bool SendEmail(string email)
		{
			try
			{
//				MailMessage mail = new MailMessage();
//				mail.From = System.Configuration.ConfigurationManager.AppSettings["OutMailFrom"].ToString();
//
//				mail.To = email;   
//				mail.BodyFormat = MailFormat.Html;
//				mail.Body = GetOutMailContent(); 
//				mail.Subject  = "您的QQ帐户回收申请资料已经通过";     
//			
//				SmtpMail.SmtpServer = System.Configuration.ConfigurationManager.AppSettings["OutSmtpServer"].ToString();
//				SmtpMail.Send(mail);

				return true;
			}
			catch(Exception err)
			{
				throw new Exception(err.Message);
			}			
		}

		private string GetOutMailContent()
		{
			string pattern = "";
			StreamReader sr = new StreamReader(Server.MapPath(Request.ApplicationPath).Trim().ToLower() 
				+ "\\Email\\QQReclaim.htm",System.Text.Encoding.GetEncoding("GB2312"));
			try
			{
				pattern = sr.ReadToEnd();
			}
			finally
			{
				sr.Close();
			}

			string TimeToTick,md5str,md5value,URL;

			TimeToTick = GetTickFromTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			md5str = "uin=12345&uid=67890&tmstmp="+TimeToTick+"&md5_mask=CREPLACERETRIEVEACC_MD5_MASK";
			md5value = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(md5str,"MD5").ToLower();
			URL = "https://www.tenpay.com/cgi-bin/v1.0/replace_retrieved_acc_pre.cgi?uin=12345&uid=67890&tmstmp=" + TimeToTick + "&skey=" + md5value;

		
			pattern = String.Format(pattern,"12345",URL,URL);
			return pattern;
		}

		private string GetTickFromTime(string time)
		{
			if(time == null || time.Trim() == "")
			{
				return "";
			}

			long ltick = 0;
			try
			{
				if(time.Length == 8)
					time = time.Substring(0,4)+"-"+time.Substring(4,2)+"-"+time.Substring(6,2);
				ltick = DateTime.Parse(time).Ticks;
			}
			catch
			{
				return "";
			}

			ltick = ltick - DateTime.Parse("1970-01-01").Ticks;
			ltick = ltick / 10000000;
			return ltick.ToString();
		}
	}
}
