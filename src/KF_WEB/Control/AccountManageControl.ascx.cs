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
	///		BaseAccountControl 的摘要说明。
	/// </summary>
	public partial class AccountManageControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl ;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if ( !IsPostBack )
			{
				menuControl.Title = "商户管理";
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
			menuControl.Title = "商户管理";

			string szkey = Session["SzKey"].ToString();

			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
	
				menuControl.AddSubMenu("提现规则查询","TradeManage/AppealDSettings.aspx") ;
				menuControl.AddSubMenu("结算规则查询","TradeManage/AppealSSetting.aspx") ;				
                menuControl.AddSubMenu("分账退款查询","TradeManage/SettleRefund.aspx") ;
				menuControl.AddSubMenu("直付商户查询","BaseAccount/PayBusinessQuery.aspx") ;
				menuControl.AddSubMenu("中介商户查询","BaseAccount/AgencyBusinessQuery.aspx") ;
				menuControl.AddSubMenu("结算查询","TradeManage/SettQuery.aspx") ;
				menuControl.AddSubMenu("结算查询(新)","TradeManage/SettQueryNew.aspx") ;
				menuControl.AddSubMenu("证书管理查询","TradeManage/MediCertManage.aspx") ;
				menuControl.AddSubMenu("自动扣费协议","TradeManage/PayLimitManage.aspx") ;
				menuControl.AddSubMenu("T+0付款查询","TradeManage/BatPayQuery.aspx") ;
				menuControl.AddSubMenu("PNR签约关系查询","BaseAccount/PNRQuery.aspx") ;
				menuControl.AddSubMenu("同步记录查询","TradeManage/SynRecordQuery.aspx") ;
			}
		}

		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}

	}

}

