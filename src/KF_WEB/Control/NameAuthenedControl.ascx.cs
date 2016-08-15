namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Configuration;

	/// <summary>
	///		NameAuthenedControl 的摘要说明。
	/// </summary>
	public partial class NameAuthenedControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面

			if(!IsPostBack)
			{
				this.InitControl();
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
			menuControl.Title = "实名认证";

			string szkey = Session["SzKey"].ToString();
			int operid = Int32.Parse(Session["OperID"].ToString());
			string loginname = Session["uid"].ToString().Trim();

			string spkfurl = System.Configuration.ConfigurationManager.AppSettings["SPKFUrl"].Trim();
			if(!spkfurl.EndsWith("/"))
				spkfurl += "/";

			string stroperid = operid.ToString();
			string strsessionid = "operid="+operid;
			if(classLibrary.getData.IsNewSensitivePowerMode)
			{
				stroperid = this.Page.Session.SessionID;
				strsessionid = "sessionid=" + stroperid;
			}

			string ip = Request.UserHostAddress.Trim();
			string Md5Key = ConfigurationManager.AppSettings["Md5Key"].Trim();

			string md5str = szkey.Replace("1","9").Replace("i","z") + operid.ToString().Replace("1","9").Replace("i","z")
				+ loginname.Replace("1","9").Replace("i","z") + ip.Replace("1","9").Replace("i","z")
				+ Md5Key.Replace("1","9").Replace("i","z");

			string md5value = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(md5str,"md5").ToLower();

			menuControl.AddSubMenu("实名处理查询","BaseAccount/UserClassQuery.aspx");


			menuControl.AddSubMenu("证件状态查询",spkfurl + "cgi-bin/authen_id_query.cgi" 
				+ "?szkey=" + szkey + "&" + strsessionid + "&loginname=" + loginname
				+ "&ip=" + ip + "&md5=" + md5value);

			menuControl.AddSubMenu("认证状态查询","NewQueryInfoPages/QueryAuthenInfoPage.aspx");

			menuControl.AddSubMenu("证件状态查询","NewQueryInfoPages/QueryAuthenStateInfoPage.aspx");

            menuControl.AddSubMenu("免费流量查询", "NewQueryInfoPages/QueryFreeFlow.aspx");
            menuControl.AddSubMenu("微信实名认证查询", "WebchatPay/QueryWechatRealNameAuthen.aspx");
            menuControl.AddSubMenu("姓名异常", "BaseAccount/NameAbnormal.aspx");
            menuControl.AddSubMenu("实名认证查询", "BaseAccount/RealNameCertifationQuery.aspx");
            menuControl.AddSubMenu("身份证影印件客服人工审核", "BaseAccount/IDCardManualReview.aspx");
		}
	}
}
