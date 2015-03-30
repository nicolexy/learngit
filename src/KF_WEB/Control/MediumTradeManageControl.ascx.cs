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
	///		MediumTradeManageControl 的摘要说明。
	/// </summary>
	public partial class MediumTradeManageControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面

			if(!IsPostBack)
			{
				InitControl();
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
			menuControl.Title = "中介交易";

			string szkey = Session["SzKey"].ToString().Trim();
			int operid = Int32.Parse(Session["OperID"].ToString().Trim());
			string loginname = Session["uid"].ToString().Trim();

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "TradeLogQuery"))
			/*
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeLogQuery",this))
			{
				menuControl.AddSubMenu("中介订单查询","TradeManage/OrderQuery.aspx") ;
			}
			*/

			string stroperid = operid.ToString();
			string strsessionid = "operid="+operid;
			if(classLibrary.getData.IsNewSensitivePowerMode)
			{
				stroperid = this.Page.Session.SessionID;
				strsessionid = "sessionid=" + stroperid;
			}

			string spkfurl = System.Configuration.ConfigurationManager.AppSettings["SPKFUrl"].Trim();
			if(!spkfurl.EndsWith("/"))
				spkfurl += "/";

			string ip = Request.UserHostAddress.Trim();
			string Md5Key = ConfigurationManager.AppSettings["Md5Key"].Trim();

			
			string md5str = szkey.Replace("1","9").Replace("i","z") + stroperid.Replace("1","9").Replace("i","z")
				+ loginname.Replace("1","9").Replace("i","z") + ip.Replace("1","9").Replace("i","z")
				+ Md5Key.Replace("1","9").Replace("i","z");

			string md5value = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(md5str,"md5").ToLower();

			/*menuControl.AddSubMenu("用户处罚",spkfurl + "cgi-bin/userpunishmain.cgi" 
				+ "?szkey=" + szkey + "&" + strsessionid + "&loginname=" + loginname
				+ "&ip=" + ip + "&md5=" + md5value);*/
//
//			menuControl.AddSubMenu("订单仲裁",spkfurl + "cgi-bin/lawsuitarbitrage.cgi" 
//				+ "?szkey=" + szkey + "&operid=" + operid + "&loginname=" + loginname
//				+ "&ip=" + ip + "&md5=" + md5value);

//			menuControl.AddSubMenu("交易投诉",spkfurl + "cgi-bin/lawsuitinfo.cgi" 
//				+ "?szkey=" + szkey + "&operid=" + operid + "&loginname=" + loginname
//				+ "&ip=" + ip + "&md5=" + md5value);
            //menuControl.AddSubMenu("测试模块勿用投诉","TradeManage/AppealQuery.aspx");
            //menuControl.AddSubMenu("测试模块勿用仲裁",spkfurl + "cgi-bin/lawsuitarbitrage.cgi" 
            //    + "?szkey=" + szkey + "&operid=" + operid + "&loginname=" + loginname
            //    + "&ip=" + ip + "&md5=" + md5value);
//			menuControl.AddSubMenu("订单仲裁","TradeManage/AppealArbitrate.aspx");

		}

		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}

	}
}
