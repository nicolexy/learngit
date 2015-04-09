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
	///		MediumTradeManageControl ��ժҪ˵����
	/// </summary>
	public partial class MediumTradeManageControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

			if(!IsPostBack)
			{
				InitControl();
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
			menuControl.Title = "�н齻��";

			string szkey = Session["SzKey"].ToString().Trim();
			int operid = Int32.Parse(Session["OperID"].ToString().Trim());
			string loginname = Session["uid"].ToString().Trim();

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "TradeLogQuery"))
			/*
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeLogQuery",this))
			{
				menuControl.AddSubMenu("�н鶩����ѯ","TradeManage/OrderQuery.aspx") ;
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

			/*menuControl.AddSubMenu("�û�����",spkfurl + "cgi-bin/userpunishmain.cgi" 
				+ "?szkey=" + szkey + "&" + strsessionid + "&loginname=" + loginname
				+ "&ip=" + ip + "&md5=" + md5value);*/
//
//			menuControl.AddSubMenu("�����ٲ�",spkfurl + "cgi-bin/lawsuitarbitrage.cgi" 
//				+ "?szkey=" + szkey + "&operid=" + operid + "&loginname=" + loginname
//				+ "&ip=" + ip + "&md5=" + md5value);

//			menuControl.AddSubMenu("����Ͷ��",spkfurl + "cgi-bin/lawsuitinfo.cgi" 
//				+ "?szkey=" + szkey + "&operid=" + operid + "&loginname=" + loginname
//				+ "&ip=" + ip + "&md5=" + md5value);
            //menuControl.AddSubMenu("����ģ������Ͷ��","TradeManage/AppealQuery.aspx");
            //menuControl.AddSubMenu("����ģ�������ٲ�",spkfurl + "cgi-bin/lawsuitarbitrage.cgi" 
            //    + "?szkey=" + szkey + "&operid=" + operid + "&loginname=" + loginname
            //    + "&ip=" + ip + "&md5=" + md5value);
//			menuControl.AddSubMenu("�����ٲ�","TradeManage/AppealArbitrate.aspx");

		}

		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}

	}
}
