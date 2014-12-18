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
	public partial class BaseAccountControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl ;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if ( !IsPostBack )
			{
				menuControl.Title = "账户管理" ;
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

			menuControl.Title = "账户管理" ;

			string szkey = Session["SzKey"].ToString();
			//int operid = Int32.Parse(Session["OperID"].ToString());

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
				
				//menuControl.AddSubMenu("一点通业务","BaseAccount/BankCardUnbind.aspx") ;
				//menuControl.AddSubMenu("客服统计查询","BaseAccount/KFTotalQuery.aspx") ;
				//menuControl.AddSubMenu("系统公告管理","BaseAccount/SysBulletinManage.aspx") ;
				//menuControl.AddSubMenu("二次登录密码撤销","Trademanage/SuspendSecondPasseword.aspx") ;
				//menuControl.AddSubMenu("PNR签约关系查询","BaseAccount/PNRQuery.aspx") ;
				//menuControl.AddSubMenu("基金签约解约信息","NewQueryInfoPages/QueryInverestorSignPage.aspx") ;
				//menuControl.AddSubMenu("基金交易查询","NewQueryInfoPages/QueryFundInfoPage.aspx") ;
				//menuControl.AddSubMenu("基金充值提现信息","NewQueryInfoPages/QueryChargeInfoPage.aspx") ;
				//menuControl.AddSubMenu("基金易账户信息查询","NewQueryInfoPages/GetUserFundAccountInfoPage.aspx") ;
				menuControl.AddSubMenu("个人账户信息","BaseAccount/InfoCenter.aspx") ;
				menuControl.AddSubMenu("QQ帐号回收","BaseAccount/QQReclaim.aspx") ;
				menuControl.AddSubMenu("账户姓名修改","BaseAccount/changeUserName_2.aspx");
				menuControl.AddSubMenu("用户受控资金查询","TradeManage/QueryUserControledFinPage.aspx");
				menuControl.AddSubMenu("手机绑定查询","TradeManage/MobileBindQuery.aspx") ;
			}
			/*
			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter") || AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "DeleteCrt"))
			{
				menuControl.AddSubMenu("个人证书管理","Trademanage/CrtQuery.aspx") ;
			}
			*/

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "UserBankInfoQuery"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("UserBankInfoQuery",this))
			{
				menuControl.AddSubMenu("银行账号信息","BaseAccount/UserBankInfoQuery.aspx") ;
			}


			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "ChangeUserInfo"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("ChangeUserInfo",this))
			{
				menuControl.AddSubMenu("个人信息","BaseAccount/ChangeUserInfo.aspx") ;
				//menuControl.AddSubMenu("子帐户查询","BaseAccount/ChildrenQuery.aspx") ;
				//menuControl.AddSubMenu("子帐户订单查询","BaseAccount/ChildrenOrderFromQuery.aspx") ;
				//menuControl.AddSubMenu("子帐户订单查询(新)","BaseAccount/ChildrenOrderFromQueryNew.aspx") ;
			}

			//furion 20050906
			/*
			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "FreezeList"))
			{
				menuControl.AddSubMenu("冻结操作查询","BaseAccount/FreezeList.aspx") ;
				menuControl.AddSubMenu("冻结资金记录","BaseAccount/FreezeFinQuery.aspx");
			}
			*/
			
			//rayguo 20060302  
			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "UserReport"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("UserReport",this))
			{
				menuControl.AddSubMenu("意见投诉查询","BaseAccount/userReport.aspx") ;
			}

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "HistoryModify"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("HistoryModify",this))
			{
				menuControl.AddSubMenu("信息修改历史","BaseAccount/historyModify.aspx") ;
			}

			/*  页面已转移
			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "CFTUserAppeal"))
			{
				menuControl.AddSubMenu("自助申诉查询","BaseAccount/CFTUserAppeal.aspx") ;
			}
			

			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "CFTUserPick"))
			{
				//menuControl.AddSubMenu("自助申诉处理","BaseAccount/CFTUserPick.aspx") ; 页面已废除
				//menuControl.AddSubMenu("实名认证处理","BaseAccount/UserClass.aspx") ; 页面已废除
				menuControl.AddSubMenu("申诉处理(新)","BaseAccount/UserAppeal.aspx") ;
			}
			*/
			
			/*
			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "CFTUserPickTJ"))
			{
			//	menuControl.AddSubMenu("申诉处理统计","BaseAccount/CFTUserPickTJ.aspx") ;  页面已废除	
			//	menuControl.AddSubMenu("申诉处理查询","BaseAccount/CFTAppealQuery.aspx") ;   页面已废除	
			//	menuControl.AddSubMenu("实名处理统计","BaseAccount/UserClassTJ.aspx") ; 页面已废除
				//menuControl.AddSubMenu("实名处理查询","BaseAccount/UserClassQuery.aspx") ;
			}
			*/
			
			//edwinyang 20061018
//			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "QueryQQ"))
//			{
//				menuControl.AddSubMenu("QQ号码查询","BaseAccount/QueryQQ.aspx") ;
//			}


			/*   页面已转移
			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "CancelAccount"))
			{
				menuControl.AddSubMenu("帐户销户记录","BaseAccount/logOnUser.aspx") ;
			}

			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "UpdateAccountQQ"))
			{
				menuControl.AddSubMenu("帐户QQ修改","BaseAccount/ChangeQQOld.aspx") ;
			}
			*/

		}
			
//			menuControl.AddSubMenu("余额冻结解冻","BaseAccount/BalanceFreeze.aspx") ;	


		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}
	}



}

