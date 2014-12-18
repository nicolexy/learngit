namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		SpecialManageControl 的摘要说明。
	/// </summary>
	public partial class SpecialManageControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if(!IsPostBack)
			{
				InitControl();
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
		///		设计器支持所需的方法 - 不要使用代码编辑器
		///		修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion


		private void InitControl()
		{
			this.menuControl.Title = "日志管理";

			string szkey = Session["SzKey"].ToString();
			string operid = Session["OperID"].ToString();
			string uid = Session["uid"].ToString();

			if(uid == "alexguan" || operid == "1100000000")
			{
				this.menuControl.AddSubMenu("快捷回复管理页面","FreezeManage/FastReplyManagePage.aspx");
				this.menuControl.AddSubMenu("测试的页面","NewQueryInfoPages/TestPage.aspx");
				this.menuControl.AddSubMenu("客服日志管理","NewQueryInfoPages/KFDiaryManagePage.aspx");
				this.menuControl.AddSubMenu("财付盾信息查询","BaseAccount/CFDQuery.aspx");
			}
		}
	}
}
