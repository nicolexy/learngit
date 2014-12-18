namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using TENCENT.OSS.CFT.KF.Common;

	/// <summary>
	///		FreezeManageControl 的摘要说明。
	/// </summary>
	public partial class FreezeManageControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面

			if(!IsPostBack)
			{
				this.menuControl1.Title = "资料审核";
				//InitControl();
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
			this.menuControl1.Title = "资料审核";

			string szkey = Session["SzKey"].ToString();
			//int operid = Int32.Parse(Session["OperID"].ToString());

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
				//this.menuControl1.AddSubMenu("风控解冻审核","FreezeManage/FreezeQuery.aspx");
				//this.menuControl1.AddSubMenu("报表统计输出","FreezeManage/FreezeCount.aspx");
				//this.menuControl1.AddSubMenu("快捷回复管理页面","FreezeManage/FastReplyManagePage.aspx");
				//this.menuControl1.AddSubMenu("测试的页面","NewQueryInfoPages/TestPage.aspx");
			}
		}
	}
}
