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

using TENCENT.OSS.CFT.KF.Common;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.DataAccess;

namespace TENCENT.OSS.CFT.KF.KF_Web
{
	/// <summary>
	/// Default1 的摘要说明。
	/// </summary>
	public partial class Default : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		public string path;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(classLibrary.getData.IsTestMode && !classLibrary.getData.IsNewSensitivePowerMode)
			{
				Session["uid"] = "1100000000";
				Session["SzKey"] = "123123242";
				Session["OperID"] = "1100000000";
				Session["QQID"] = "1100000000";
				Session["TestLoginUserName"] = "";
			}
			// 在此处放置用户代码以初始化页面
			//if (Session["uid"] == null)
			if (Session["uid"] == null && !classLibrary.getData.IsNewSensitivePowerMode)
			{
				//Response.Redirect("login.aspx");
				GetCurrentUser();
			}
			else
				path = "middle.htm";
			
			//Response.Write("<script language=javascript>window.WorkArea.location='" + path + "'</script>"); 
		}


		private void GetCurrentUser()
		{
			string url = ConfigurationManager.AppSettings["CMUrl"];
			string adminurl = url + ConfigurationManager.AppSettings["GROUPID"];
			Response.Redirect(adminurl);
			return;				
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

	}
}
