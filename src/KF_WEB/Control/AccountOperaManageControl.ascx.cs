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
	///		AccountOperaManageControl 的摘要说明。
	/// </summary>
	public partial class AccountOperaManageControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if(!IsPostBack)
			{
				menuControl.Title = "商户操作";
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
			menuControl.Title = "商户操作";

			string szkey = Session["SzKey"].ToString();
			//int operid = Int32.Parse(Session["OperID"].ToString());

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
				menuControl.AddSubMenu("商户资料修改","BaseAccount/ModifyBusinessInfo.aspx") ;
				menuControl.AddSubMenu("商户身份证修改","BaseAccount/UpdateMerchantCre.aspx") ;
				menuControl.AddSubMenu("商户冻结申请","TradeManage/BusinessFreeze.aspx") ;
				menuControl.AddSubMenu("商户关闭退款申请","TradeManage/ShutRefund.aspx") ;
				menuControl.AddSubMenu("商户开通退款申请","TradeManage/ApplyRefund.aspx");
				menuControl.AddSubMenu("商户恢复申请","TradeManage/BusinessResume.aspx") ;
				menuControl.AddSubMenu("商户退单撤销","RefundManage/SuspendRefundment.aspx") ;
			}


			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "DrawAndApprove"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("DrawAndApprove",this))
			{
				menuControl.AddSubMenu("自助商户领单","BaseAccount/SelfQuery.aspx") ;
                //menuControl.AddSubMenu("自助商户审核","BaseAccount/SelfQueryApprove.aspx") ;//20160715 龙海军: 客服系统查询页面屏蔽，代码先保留，做成可恢复，防止后续业务变更需要重新使用
                menuControl.AddSubMenu("商户修改资料审核", "BaseAccount/DomainApprove.aspx");
			}
		}



		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}
	
	}
}
