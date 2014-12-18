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
			//int operid = Int32.Parse(Session["OperID"].ToString());

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
				//menuControl.AddSubMenu("商户注销申请","TradeManage/BusinessLogout.aspx") ;
				//menuControl.AddSubMenu("分账订单流水","BaseAccount/SeparateOperation.aspx") ;
				//menuControl.AddSubMenu("分账订单金额","TradeManage/SeparateListQuery.aspx") ;
				//menuControl.AddSubMenu("收支分离查询","TradeManage/SettleRule.aspx") ;
				//menuControl.AddSubMenu("分账订单查询","TradeManage/SettleInfo.aspx") ;
				//menuControl.AddSubMenu("冻结解冻查询","TradeManage/SettleFreeze.aspx") ;
				//menuControl.AddSubMenu("调账订单查询","TradeManage/AdjustList.aspx") ;
				//menuControl.AddSubMenu("代理分账关系","TradeManage/SettleAgent.aspx") ;
				//menuControl.AddSubMenu("商户资料修改","BaseAccount/ModifyBusinessInfo.aspx") ;
				//menuControl.AddSubMenu("商户交易清单","TradeManage/TradeLogList.aspx") ;
				//menuControl.AddSubMenu("退款单查询","TradeManage/B2CReturnQuery.aspx") ;
				//menuControl.AddSubMenu("信用卡还款","TradeManage/CreditQuery.aspx") ;
				//menuControl.AddSubMenu("商户冻结申请","TradeManage/BusinessFreeze.aspx") ;
				//menuControl.AddSubMenu("商户关闭退款申请","TradeManage/ShutRefund.aspx") ;
				//menuControl.AddSubMenu("商户开通退款申请","TradeManage/ApplyRefund.aspx");
				//menuControl.AddSubMenu("商户恢复申请","TradeManage/BusinessResume.aspx") ;
				//menuControl.AddSubMenu("退单异常数据查询","RefundManage/RefundErrorHandle.aspx") ;
				//menuControl.AddSubMenu("商户退单撤销","RefundManage/SuspendRefundment.aspx") ;

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


			/*  页面已转移
			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "DrawAndApprove"))
			{
				menuControl.AddSubMenu("自助商户领单","BaseAccount/SelfQuery.aspx") ;
				menuControl.AddSubMenu("自助商户审核","BaseAccount/SelfQueryApprove.aspx") ;
				menuControl.AddSubMenu("商户域名审核","BaseAccount/DomainApprove.aspx") ;
			}
			*/

		}

		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}

	}



}

