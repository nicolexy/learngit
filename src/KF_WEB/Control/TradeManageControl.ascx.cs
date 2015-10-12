namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
    using TENCENT.OSS.CFT.KF.Common;
	using System.Configuration;

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

//			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "TradeLogList"))
//			{
//			}
			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
				//menuControl.AddSubMenu("同步记录查询","TradeManage/SynRecordQuery.aspx") ;
				//menuControl.AddSubMenu("汇总退单数据","TradeManage/RefundMain.aspx") ;
				//menuControl.AddSubMenu("订单实时查询","TradeManage/RealTimeOrderQuery.aspx") ;
				//menuControl.AddSubMenu("生活缴费查询","TradeManage/FeeQuery.aspx") ;
				//menuControl.AddSubMenu("邮储汇款查询","RemitCheck/RemitQuery.aspx");
				//menuControl.AddSubMenu("手机充值卡查询","TradeManage/FundCardQuery_Detail.aspx") ;
				//menuControl.AddSubMenu("手机绑定查询","TradeManage/MobileBindQuery.aspx") ;
				
				//menuControl.AddSubMenu("银行卡查询","TradeManage/BankCardQuery.aspx") ;
				//menuControl.AddSubMenu("异常任务单管理","RefundManage/RefundErrorMain.aspx?WorkType=task");
				//menuControl.AddSubMenu("用户受控资金查询","TradeManage/QueryUserControledFinPage.aspx");

				menuControl.AddSubMenu("提现记录查询","TradeManage/PickQuery.aspx") ;
				menuControl.AddSubMenu("商户交易清单","TradeManage/TradeLogList.aspx") ;
				menuControl.AddSubMenu("退款单查询","TradeManage/B2CReturnQuery.aspx") ;
				menuControl.AddSubMenu("代扣单笔查询","NewQueryInfoPages/QueryDKInfoPage.aspx");
				menuControl.AddSubMenu("代扣批量查询","NewQueryInfoPages/QueryDKListInfoPage.aspx");
				menuControl.AddSubMenu("商户订单查询财付通订单","TradeManage/QuerySpOrderPage.aspx") ;
			}


			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "TradeLogQuery"))
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeManagement", this))
			{
				menuControl.AddSubMenu("交易记录查询","TradeManage/TradeLogQuery.aspx") ;
				//menuControl.AddSubMenu("汇总付款数据","BaseAccount/batPay.aspx") ;
			}

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "FundQuery"))
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeManagement", this))
			{
				menuControl.AddSubMenu("充值记录查询","TradeManage/FundQuery.aspx") ;
				//menuControl.AddSubMenu("订单实时调帐","TradeManage/RealtimeOrder.aspx") ;
			}

			/* 页面已经转移
			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "TradeLogQuery"))
			{
				menuControl.AddSubMenu("中介订单查询","TradeManage/OrderQuery.aspx") ;
			}
			*/

			/*  页面已经转移
			string spkfurl = System.Configuration.ConfigurationManager.AppSettings["SPKFUrl"].Trim();
			if(!spkfurl.EndsWith("/"))
				spkfurl += "/";

			string ip = Request.UserHostAddress.Trim();
			string Md5Key = ConfigurationManager.AppSettings["Md5Key"].Trim();

			string md5str = szkey.Replace("1","9").Replace("i","z") + operid.ToString().Replace("1","9").Replace("i","z")
				+ loginname.Replace("1","9").Replace("i","z") + ip.Replace("1","9").Replace("i","z")
				+ Md5Key.Replace("1","9").Replace("i","z");

			string md5value = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(md5str,"md5").ToLower();

			menuControl.AddSubMenu("用户处罚",spkfurl + "cgi-bin/userpunishmain.cgi" 
				+ "?szkey=" + szkey + "&operid=" + operid + "&loginname=" + loginname
				+ "&ip=" + ip + "&md5=" + md5value);

			menuControl.AddSubMenu("订单仲裁",spkfurl + "cgi-bin/lawsuitarbitrage.cgi" 
				+ "?szkey=" + szkey + "&operid=" + operid + "&loginname=" + loginname
				+ "&ip=" + ip + "&md5=" + md5value);

			menuControl.AddSubMenu("交易投诉",spkfurl + "cgi-bin/lawsuitinfo.cgi" 
				+ "?szkey=" + szkey + "&operid=" + operid + "&loginname=" + loginname
				+ "&ip=" + ip + "&md5=" + md5value);
			*/

			/*
			menuControl.AddSubMenu("认证状态查询",spkfurl + "cgi-bin/authen_state.cgi" 
				+ "?szkey=" + szkey + "&operid=" + operid + "&loginname=" + loginname
				+ "&ip=" + ip + "&md5=" + md5value);

			menuControl.AddSubMenu("证件状态查询",spkfurl + "cgi-bin/authen_id_query.cgi" 
				+ "?szkey=" + szkey + "&operid=" + operid + "&loginname=" + loginname
				+ "&ip=" + ip + "&md5=" + md5value);

			menuControl.AddSubMenu("认证状态查询_new","NewQueryInfoPages/QueryAuthenInfoPage.aspx");
			menuControl.AddSubMenu("证件状态查询_new","NewQueryInfoPages/QueryAuthenStateInfoPage.aspx");
			

			menuControl.AddSubMenu("授权关系查询",spkfurl + "cgi-bin/authen_relation.cgi" 
				+ "?szkey=" + szkey + "&operid=" + operid + "&loginname=" + loginname
				+ "&ip=" + ip + "&md5=" + md5value);
				
			*/
		}

		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}
	}
}
