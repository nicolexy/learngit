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
    ///		CreditPayControl 的摘要说明。
	/// </summary>
    public partial class CreditPayControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl ;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if ( !IsPostBack )
			{
				menuControl.Title = "信用支付" ;
				//InitControls() ;
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

		private void InitControls()
		{

            //menuControl.Title = "信用支付";

			string szkey = Session["SzKey"].ToString();
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
                menuControl.AddSubMenu("基本信息", "CreditPay/QueryCreditUserInfo.aspx");
                menuControl.AddSubMenu("账单查询", "CreditPay/QueryCreditBillList.aspx");
                menuControl.AddSubMenu("欠款查询", "CreditPay/QueryCreditDebt.aspx");
                menuControl.AddSubMenu("还款查询", "CreditPay/QueryRefund.aspx");
                //menuControl.AddSubMenu("账户查询", "CreditPay/AccountSearch.aspx");
                //menuControl.AddSubMenu("账单查询", "CreditPay/BillSearch.aspx");
                //menuControl.AddSubMenu("明细查询", "CreditPay/DetailSearch.aspx");
            }
		}
			
		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}
	}



}

