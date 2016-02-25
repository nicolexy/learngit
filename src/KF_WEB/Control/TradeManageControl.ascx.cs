using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using TENCENT.OSS.CFT.KF.Common;
using System.Configuration;

namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{

	/// <summary>
	///		TradeManageControl 的摘要说明。
	/// </summary>
	public partial class TradeManageControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl ;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if ( !IsPostBack )
			{
				menuControl.Title = "交易查询" ;
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
			
			menuControl.Title = "交易查询" ;
//
			string szkey = Session["SzKey"].ToString().Trim();
			int operid = Int32.Parse(Session["OperID"].ToString().Trim());
			string loginname = Session["uid"].ToString().Trim();

			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
                menuControl.AddSubMenu("提现记录查询", "TradeManage/PickQueryNew.aspx");
				menuControl.AddSubMenu("商户交易清单","TradeManage/TradeLogList.aspx") ;
				menuControl.AddSubMenu("退款单查询","TradeManage/B2CReturnQuery.aspx") ;
				menuControl.AddSubMenu("代扣单笔查询","NewQueryInfoPages/QueryDKInfoPage.aspx");
				menuControl.AddSubMenu("代扣批量查询","NewQueryInfoPages/QueryDKListInfoPage.aspx");
				menuControl.AddSubMenu("商户订单查询财付通订单","TradeManage/QuerySpOrderPage.aspx") ;
			}

            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeManagement", this))
			{
				menuControl.AddSubMenu("交易记录查询","TradeManage/TradeLogQuery.aspx") ;
			}

            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeManagement", this))
			{
				menuControl.AddSubMenu("充值记录查询","TradeManage/FundQuery.aspx") ;
			}

		}

		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}
	}
}
