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
	/// Top 的摘要说明。
	/// </summary>
	public partial class Top : System.Web.UI.Page
	{
		protected TENCENT.OSS.C2C.Finance.Portal.CFTHeader.CFTHeader CFTHeader1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if ( Session["uid"]!= null )
				CFTHeader1.User = Session["uid"].ToString();
			else
				CFTHeader1.User = "未登录";
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
