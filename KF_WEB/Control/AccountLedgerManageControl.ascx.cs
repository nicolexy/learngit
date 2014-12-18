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
	///		AccountLedgerManageControl 的摘要说明。
	/// </summary>
	public partial class AccountLedgerManageControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面

			if(!IsPostBack)
			{
				menuControl.Title = "商户分账";
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
			menuControl.Title = "商户分账";

			string szkey = Session["SzKey"].ToString();
			int operid = Int32.Parse(Session["OperID"].ToString());

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
				menuControl.AddSubMenu("分账订单流水","BaseAccount/SeparateOperation.aspx") ;
				menuControl.AddSubMenu("分账订单金额","TradeManage/SeparateListQuery.aspx") ;
				menuControl.AddSubMenu("收支分离查询","TradeManage/SettleRule.aspx") ;
				menuControl.AddSubMenu("分账订单查询","TradeManage/SettleInfo.aspx") ;
				menuControl.AddSubMenu("冻结解冻查询","TradeManage/SettleFreeze.aspx") ;
				menuControl.AddSubMenu("调账订单查询","TradeManage/AdjustList.aspx") ;
				menuControl.AddSubMenu("代理分账关系","TradeManage/SettleAgent.aspx") ;
			}
		}


		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}
	
	}
}
