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
    ///		WebchatPayControl 的摘要说明。
	/// </summary>
    public partial class WebchatPayControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl ;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if ( !IsPostBack )
			{
				menuControl.Title = "微信支付" ;
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

            //menuControl.Title = "微信支付";

			string szkey = Session["SzKey"].ToString();
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
                menuControl.AddSubMenu("微信支付帐号", "WebchatPay/WechatInfoQuery.aspx");
                menuControl.AddSubMenu("AA收款帐号", "WebchatPay/WechatAACollection.aspx");
                menuControl.AddSubMenu("理财通查询", "NewQueryInfoPages/GetFundRatePage.aspx");
                menuControl.AddSubMenu("理财通安全卡", "WebchatPay/SafeCardManage.aspx");
                menuControl.AddSubMenu("微信红包查询", "WebchatPay/WechatRedPacket.aspx");
                menuControl.AddSubMenu("小额刷卡", "WebchatPay/SmallCreditCardQuery.aspx");
                menuControl.AddSubMenu("微信信用卡还款", "WebchatPay/CreditCardRefundQuery.aspx");
			}
		}
			
		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}
	}



}

