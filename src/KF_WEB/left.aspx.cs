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

	/// Left 的摘要说明。

	/// </summary>

	public partial class Left : TENCENT.OSS.CFT.KF.KF_Web.PageBase

	{

		protected void Page_Load(object sender, System.EventArgs e)

		{

			// 在此处放置用户代码以初始化页面

			try
            {
                //检测未登录状态跳转到登录页面
                if (Session["uid"] == null)
                {
                    Response.Write("<script>window.parent.location.href = 'login.aspx';</script>");
                    Response.End();
                }
			}

			catch  //Session为空，则跳转

			{

//				Response.Redirect("login.aspx?wh=1");

				Response.Write("超时，请重新<a herf = 'login.aspx' target = 'parent'> 登录</a>！");

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

	}

}

