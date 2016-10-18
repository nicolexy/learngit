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
using System.Configuration;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using SunLibrary;


namespace TENCENT.OSS.CFT.KF.KF_Web
{
	/// <summary>
	/// login ��ժҪ˵����
	/// </summary>

	public partial class login : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label Label1;
		string position;

		protected void Page_Load(object sender, System.EventArgs e)
		{            
            if (Request.QueryString["oaAuthentication"] =="1")
            {   //Oa ��Ȩ ticket  ��ȡ
                Session["oa_ticket"] = Request.QueryString["ticket"];                
                return;
            }           

			if(classLibrary.getData.IsNewSensitivePowerMode)
			{                
				NewPowerCheck();
			}
			else
			{
               
				if(Request.QueryString["key"] != null)
				{
					Session["SzKey"] = Request.QueryString["key"].Trim();
					Session["OperID"] = Request.QueryString["id"].Trim();
					Session["uid"] = Request.QueryString["LoginName"].Trim();                    
                    Response.Redirect("defaultNotice.aspx", false);                    
					return;
				}
				else
				{
					//if(ConfigurationManager.AppSettings["IsTestLogin"].Trim().ToLower() != "true")
					if(!classLibrary.getData.IsTestMode)
					{
						string url = ConfigurationManager.AppSettings["CMUrl"];
						string adminurl = url + ConfigurationManager.AppSettings["GROUPID"];                        
						Response.Redirect(adminurl);
						return;	
					}
					else
					{                        
						Response.Redirect("defaultNotice.aspx",false);
					}
				}
			}
		}

		private void NewPowerCheck()
		{
			string ticket = "";
			string str = Request.Url.ToString();
			if(Request.QueryString["auth_cm_com_ticket"] != null && Request.QueryString["auth_cm_com_ticket"].Trim() != "")
			{
				ticket = Request.QueryString["auth_cm_com_ticket"].Trim();
				Session["Ticket"] = ticket;
			}

            string requestUrl = GetRedirectUrl();

			if(!classLibrary.SensitivePowerOperaLib.CheckSession(ticket,this))
			{
				//Response.Redirect("../login.aspx?wh=1");
			}
			else
			{
                string url_Host = HttpContext.Current.Request.Url.Host;
                int port = HttpContext.Current.Request.Url.Port;
                string url_Host_port = port > 0 ? url_Host + ":" + port.ToString() : url_Host;
                string url = string.IsNullOrEmpty(requestUrl) ? "defaultNotice.aspx" : "defaultNotice.aspx?requestUrl=" + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Server.UrlEncode( requestUrl))) + "";
                //string url = string.IsNullOrEmpty(requestUrl) ? "defaultNotice.aspx" : "defaultNotice.aspx?requestUrl=" + requestUrl + "";
                //Response.Redirect(url, false);                  
                Response.Write("<script type='text/javascript'>window.location.href='" + url+ "';</script>");
                Response.End();
				return;
			}
		}

        /// <summary>
        /// ��ȡ��ת��¼ǰҳ��·��
        /// </summary>
        /// <returns></returns>
        private string GetRedirectUrl()
        {
            var redirectUrl = string.Empty;
            try
            {
                if (Request["returnurl"] != null && !string.IsNullOrEmpty(Request["returnurl"].ToString()))
                {
                    redirectUrl = Server.UrlDecode(Request["returnurl"].ToString());
                }
            }
            catch
            {
                redirectUrl = "defaultNotice.aspx";
            }

            return redirectUrl;
        }

		#region Web ������������ɵĴ���

		override protected void OnInit(EventArgs e)

		{

			//

			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�

			//

			InitializeComponent();

			base.OnInit(e);

		}

		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>

		private void InitializeComponent()
		{    
		}

		#endregion

		protected void Button1_Click(object sender, System.EventArgs e)
		{
			try
			{
				Query_Service.Query_Service qs = new Query_Service.Query_Service();
				Query_Service.TCreateSessionReply  sr = qs.ValidUser(this.TextBox_uid.Text.Trim(),this.TextBox2_pwd.Text.Trim(),Request.UserHostAddress);

				if (sr != null && sr.OpResult == 0)
				{
					TextBox_uid.Text = sr.OperId.ToString()+ " " +sr.szKey;
				}
				else
				{
					WebUtils.ShowMessage(this.Page,"�û��������벻��ȷ�������ԣ�");	
				}
			}
			catch(Exception eMsg)
			{
				WebUtils.ShowMessage(this.Page,"�û��������벻��ȷ�������ԣ�[" + eMsg.Message.ToString().Replace("'","��") + "]");
			}
		}


		protected void Button2_Click(object sender, System.EventArgs e)
		{
			this.TextBox_uid.Text = null;
			this.TextBox2_pwd.Text = null;
		}



	}

}
