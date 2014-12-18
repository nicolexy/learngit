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
	///		FundAccountManageControl 的摘要说明。
	/// </summary>
	public partial class FundAccountManageControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if(!IsPostBack)
			{
				menuControl.Title = "基金账户" ;
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
			menuControl.Title = "基金账户" ;

			string szkey = Session["SzKey"].ToString();
			//int operid = Int32.Parse(Session["OperID"].ToString());

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
				menuControl.AddSubMenu("签约解约查询","NewQueryInfoPages/QueryInverestorSignPage.aspx") ;
				menuControl.AddSubMenu("基金交易查询","NewQueryInfoPages/QueryFundInfoPage.aspx") ;
				menuControl.AddSubMenu("基金流水查询","NewQueryInfoPages/QueryChargeInfoPage.aspx") ;
				menuControl.AddSubMenu("基金账户查询","NewQueryInfoPages/GetUserFundAccountInfoPage.aspx") ;
			}
		}

		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}
	}
}
