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
	///		BankBillManageControl 的摘要说明。
	/// </summary>
	public partial class BankBillManageControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if(!IsPostBack)
			{
				menuControl.Title = "银行账单";
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
			menuControl.Title = "银行账单";

			string szkey = Session["SzKey"].ToString().Trim();
			//int operid = Int32.Parse(Session["OperID"].ToString().Trim());

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
				menuControl.AddSubMenu("系统公告管理","BaseAccount/SysBulletinManage.aspx") ;
				menuControl.AddSubMenu("汇总退单数据","TradeManage/RefundMain.aspx") ;
				menuControl.AddSubMenu("订单实时查询","TradeManage/RealTimeOrderQuery.aspx") ;
				menuControl.AddSubMenu("异常任务单管理","RefundManage/RefundErrorMain.aspx?WorkType=task");
				menuControl.AddSubMenu("退单异常数据查询","RefundManage/RefundErrorHandle.aspx") ;
			}

			
			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "TradeLogQuery"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeLogQuery",this))
			{
				menuControl.AddSubMenu("汇总付款数据","BaseAccount/batPay.aspx") ;
			}

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "FundQuery"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("FundQuery",this))
			{
				menuControl.AddSubMenu("订单实时调帐","TradeManage/RealtimeOrder.aspx") ;
			}
		}

		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}
	}
}
