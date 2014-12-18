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
	///		RiskControledManageControl 的摘要说明。
	/// </summary>
	public class RiskControledManageControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl;
		private void Page_Load(object sender, System.EventArgs e)
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
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion


		private void InitControl()
		{
			menuControl.Title = "风控管理" ;

			string szkey = Session["SzKey"].ToString();
			int operid = Int32.Parse(Session["OperID"].ToString());

			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
			{
				menuControl.AddSubMenu("二次登录密码撤销","Trademanage/SuspendSecondPasseword.aspx") ;
			}

			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter") || AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "DeleteCrt"))
			{
				menuControl.AddSubMenu("个人证书管理","Trademanage/CrtQuery.aspx") ;
			}

			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "FreezeList"))
			{
				menuControl.AddSubMenu("冻结操作查询","BaseAccount/FreezeList.aspx") ;
				menuControl.AddSubMenu("冻结资金记录","BaseAccount/FreezeFinQuery.aspx");
			}


		}
	}
}
