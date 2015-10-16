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
	///		RiskConManage 的摘要说明。
	/// </summary>
	public partial class RiskConManage : System.Web.UI.UserControl
	{
		protected MenuControl menuControl ;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			// 在此处放置用户代码以初始化页面
			if ( !IsPostBack )
			{
				menuControl.Title = "风控管理" ;
				//InitControls() ;
			}
		}

		private void InitControls()
		{
			menuControl.Title = "风控管理" ;

			string spkfurl = System.Configuration.ConfigurationManager.AppSettings["RickConUrlPath"].Trim();
			if(!spkfurl.EndsWith("/"))
				spkfurl += "/";

			/*风控取消了该页面的查询
			menuControl.AddSubMenu("异常提示",spkfurl + "NotifyList.aspx") ;
			menuControl.AddSubMenu("拒收手机",spkfurl + "MaskMobile.aspx") ;
			menuControl.AddSubMenu("异常交易",spkfurl + "BlockLog.aspx") ;
			menuControl.AddSubMenu("异常登陆",spkfurl + "LoginViolation.aspx") ;*/
			menuControl.AddSubMenu("资金流水查询","BaseAccount/BankrollHistoryLog.aspx") ;

			string szkey = Session["SzKey"].ToString();
			int operid = Int32.Parse(Session["OperID"].ToString());

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
				menuControl.AddSubMenu("二次登录密码撤销","Trademanage/SuspendSecondPasseword.aspx") ;
			}

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter") || AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "DeleteCrt"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this) || TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("DeleteCrt",this))
			{
				menuControl.AddSubMenu("个人证书管理","Trademanage/CrtQuery.aspx") ;
			}

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "FreezeList"))
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
			{
				menuControl.AddSubMenu("冻结操作查询","BaseAccount/FreezeList.aspx") ;
				menuControl.AddSubMenu("冻结资金记录","BaseAccount/FreezeFinQuery.aspx");
			}

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
				menuControl.AddSubMenu("风控解冻审核","FreezeManage/FreezeQuery.aspx");
				menuControl.AddSubMenu("报表统计输出","FreezeManage/FreezeCount.aspx");
				//this.menuControl1.AddSubMenu("快捷回复管理页面","FreezeManage/FastReplyManagePage.aspx");
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

		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}
	}
}
