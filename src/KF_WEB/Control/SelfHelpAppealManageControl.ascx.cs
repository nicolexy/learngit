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
	///		SelfHelpAppealManageControl 的摘要说明。
	/// </summary>
	public partial class SelfHelpAppealManageControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if(!IsPostBack)
			{
				menuControl.Title = "自助申诉";
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
			menuControl.Title = "自助申诉";

			string szkey = Session["SzKey"].ToString();
			int operid = Int32.Parse(Session["OperID"].ToString());

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
				menuControl.AddSubMenu("客服统计查询","BaseAccount/KFTotalQuery.aspx") ;
			}


			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "CFTUserAppeal"))
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
			{
				menuControl.AddSubMenu("自助申诉查询","BaseAccount/CFTUserAppeal.aspx") ;
			}


			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "CFTUserPick"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("CFTUserPick",this))
			{
				//menuControl.AddSubMenu("自助申诉处理","BaseAccount/CFTUserPick.aspx") ; 页面已废除
				//menuControl.AddSubMenu("实名认证处理","BaseAccount/UserClass.aspx") ; 页面已废除
				menuControl.AddSubMenu("申诉处理(新)","BaseAccount/UserAppeal.aspx") ;
			}

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "CancelAccount"))
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
			{
				menuControl.AddSubMenu("帐户销户记录","BaseAccount/logOnUser.aspx") ;
			}

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "UpdateAccountQQ"))
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
			{
				menuControl.AddSubMenu("帐户QQ修改","BaseAccount/ChangeQQOld.aspx") ;
			}

		}

		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}
	}
}
