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

namespace TENCENT.OSS.CFT.KF.KF_Web
{
	/// <summary>
	/// Top ��ժҪ˵����
	/// </summary>
	public partial class Top : System.Web.UI.Page
	{
		protected TENCENT.OSS.C2C.Finance.Portal.CFTHeader.CFTHeader CFTHeader1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if ( Session["uid"]!= null )
				CFTHeader1.User = Session["uid"].ToString();
			else
				CFTHeader1.User = "δ��¼";
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
