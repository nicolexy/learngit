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
	///		NameAuthenedControl ��ժҪ˵����
	/// </summary>
	public partial class NameAuthenedControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

			if(!IsPostBack)
			{
				this.InitControl();
			}
		}

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		�����֧������ķ��� - ��Ҫʹ�ô���༭��
		///		�޸Ĵ˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion

		private void InitControl()
		{
			menuControl.Title = "ʵ����֤";

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

			menuControl.AddSubMenu("ʵ�������ѯ","BaseAccount/UserClassQuery.aspx");


			menuControl.AddSubMenu("֤��״̬��ѯ",spkfurl + "cgi-bin/authen_id_query.cgi" 
				+ "?szkey=" + szkey + "&" + strsessionid + "&loginname=" + loginname
				+ "&ip=" + ip + "&md5=" + md5value);

			menuControl.AddSubMenu("��֤״̬��ѯ","NewQueryInfoPages/QueryAuthenInfoPage.aspx");

			menuControl.AddSubMenu("֤��״̬��ѯ","NewQueryInfoPages/QueryAuthenStateInfoPage.aspx");

            menuControl.AddSubMenu("���������ѯ", "NewQueryInfoPages/QueryFreeFlow.aspx");
            menuControl.AddSubMenu("΢��ʵ����֤��ѯ", "WebchatPay/QueryWechatRealNameAuthen.aspx");
            menuControl.AddSubMenu("�����쳣", "BaseAccount/NameAbnormal.aspx");
            menuControl.AddSubMenu("ʵ����֤��ѯ", "BaseAccount/RealNameCertifationQuery.aspx");
            menuControl.AddSubMenu("���֤Ӱӡ���ͷ��˹����", "BaseAccount/IDCardManualReview.aspx");
		}
	}
}
