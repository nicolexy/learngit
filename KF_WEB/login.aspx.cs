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


namespace TENCENT.OSS.CFT.KF.KF_Web
{
	/// <summary>
	/// login 的摘要说明。
	/// </summary>

	public partial class login : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label Label1;
		string position;

		protected void Page_Load(object sender, System.EventArgs e)
		{
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
					Response.Redirect("default.aspx",false);
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
						Response.Redirect("default.aspx",false);
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

			if(!classLibrary.SensitivePowerOperaLib.CheckSession(ticket,this))
			{
				//Response.Redirect("../login.aspx?wh=1");
			}
			else
			{
				Response.Redirect("default.aspx",false);
				return;
			}
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
				Query_Service.Query_Service qs = new Query_Service.Query_Service();
				Query_Service.TCreateSessionReply  sr = qs.ValidUser(this.TextBox_uid.Text.Trim(),this.TextBox2_pwd.Text.Trim(),Request.UserHostAddress);

				if (sr != null && sr.OpResult == 0)
				{
					TextBox_uid.Text = sr.OperId.ToString()+ " " +sr.szKey;
				}
				else
				{
					WebUtils.ShowMessage(this.Page,"用户名或密码不正确！请重试！");	
				}
			}
			catch(Exception eMsg)
			{
				WebUtils.ShowMessage(this.Page,"用户名或密码不正确！请重试！[" + eMsg.Message.ToString().Replace("'","’") + "]");
			}
		}


		protected void Button2_Click(object sender, System.EventArgs e)
		{
			this.TextBox_uid.Text = null;
			this.TextBox2_pwd.Text = null;
		}



	}

}
