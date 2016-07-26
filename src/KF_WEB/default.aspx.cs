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
	/// Default1 ��ժҪ˵����
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
			// �ڴ˴������û������Գ�ʼ��ҳ��
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

	}
}
