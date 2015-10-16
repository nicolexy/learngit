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
	///		TradeManageControl ��ժҪ˵����
	/// </summary>
	public partial class TradeManageControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl ;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if ( !IsPostBack )
			{
				menuControl.Title = "���ײ�ѯ" ;
				//InitControls() ;
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

		private void InitControls()
		{
			
			menuControl.Title = "���ײ�ѯ" ;
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
				//menuControl.AddSubMenu("ͬ����¼��ѯ","TradeManage/SynRecordQuery.aspx") ;
				//menuControl.AddSubMenu("�����˵�����","TradeManage/RefundMain.aspx") ;
				//menuControl.AddSubMenu("����ʵʱ��ѯ","TradeManage/RealTimeOrderQuery.aspx") ;
				//menuControl.AddSubMenu("����ɷѲ�ѯ","TradeManage/FeeQuery.aspx") ;
				//menuControl.AddSubMenu("�ʴ�����ѯ","RemitCheck/RemitQuery.aspx");
				//menuControl.AddSubMenu("�ֻ���ֵ����ѯ","TradeManage/FundCardQuery_Detail.aspx") ;
				//menuControl.AddSubMenu("�ֻ��󶨲�ѯ","TradeManage/MobileBindQuery.aspx") ;
				
				//menuControl.AddSubMenu("���п���ѯ","TradeManage/BankCardQuery.aspx") ;
				//menuControl.AddSubMenu("�쳣���񵥹���","RefundManage/RefundErrorMain.aspx?WorkType=task");
				//menuControl.AddSubMenu("�û��ܿ��ʽ��ѯ","TradeManage/QueryUserControledFinPage.aspx");

				menuControl.AddSubMenu("���ּ�¼��ѯ","TradeManage/PickQuery.aspx") ;
				menuControl.AddSubMenu("�̻������嵥","TradeManage/TradeLogList.aspx") ;
				menuControl.AddSubMenu("�˿��ѯ","TradeManage/B2CReturnQuery.aspx") ;
				menuControl.AddSubMenu("���۵��ʲ�ѯ","NewQueryInfoPages/QueryDKInfoPage.aspx");
				menuControl.AddSubMenu("����������ѯ","NewQueryInfoPages/QueryDKListInfoPage.aspx");
				menuControl.AddSubMenu("�̻�������ѯ�Ƹ�ͨ����","TradeManage/QuerySpOrderPage.aspx") ;
			}


			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "TradeLogQuery"))
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeManagement", this))
			{
				menuControl.AddSubMenu("���׼�¼��ѯ","TradeManage/TradeLogQuery.aspx") ;
				//menuControl.AddSubMenu("���ܸ�������","BaseAccount/batPay.aspx") ;
			}

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "FundQuery"))
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeManagement", this))
			{
				menuControl.AddSubMenu("��ֵ��¼��ѯ","TradeManage/FundQuery.aspx") ;
				//menuControl.AddSubMenu("����ʵʱ����","TradeManage/RealtimeOrder.aspx") ;
			}

			/* ҳ���Ѿ�ת��
			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "TradeLogQuery"))
			{
				menuControl.AddSubMenu("�н鶩����ѯ","TradeManage/OrderQuery.aspx") ;
			}
			*/

			/*  ҳ���Ѿ�ת��
			string spkfurl = System.Configuration.ConfigurationManager.AppSettings["SPKFUrl"].Trim();
			if(!spkfurl.EndsWith("/"))
				spkfurl += "/";

			string ip = Request.UserHostAddress.Trim();
			string Md5Key = ConfigurationManager.AppSettings["Md5Key"].Trim();

			string md5str = szkey.Replace("1","9").Replace("i","z") + operid.ToString().Replace("1","9").Replace("i","z")
				+ loginname.Replace("1","9").Replace("i","z") + ip.Replace("1","9").Replace("i","z")
				+ Md5Key.Replace("1","9").Replace("i","z");

			string md5value = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(md5str,"md5").ToLower();

			menuControl.AddSubMenu("�û�����",spkfurl + "cgi-bin/userpunishmain.cgi" 
				+ "?szkey=" + szkey + "&operid=" + operid + "&loginname=" + loginname
				+ "&ip=" + ip + "&md5=" + md5value);

			menuControl.AddSubMenu("�����ٲ�",spkfurl + "cgi-bin/lawsuitarbitrage.cgi" 
				+ "?szkey=" + szkey + "&operid=" + operid + "&loginname=" + loginname
				+ "&ip=" + ip + "&md5=" + md5value);

			menuControl.AddSubMenu("����Ͷ��",spkfurl + "cgi-bin/lawsuitinfo.cgi" 
				+ "?szkey=" + szkey + "&operid=" + operid + "&loginname=" + loginname
				+ "&ip=" + ip + "&md5=" + md5value);
			*/

			/*
			menuControl.AddSubMenu("��֤״̬��ѯ",spkfurl + "cgi-bin/authen_state.cgi" 
				+ "?szkey=" + szkey + "&operid=" + operid + "&loginname=" + loginname
				+ "&ip=" + ip + "&md5=" + md5value);

			menuControl.AddSubMenu("֤��״̬��ѯ",spkfurl + "cgi-bin/authen_id_query.cgi" 
				+ "?szkey=" + szkey + "&operid=" + operid + "&loginname=" + loginname
				+ "&ip=" + ip + "&md5=" + md5value);

			menuControl.AddSubMenu("��֤״̬��ѯ_new","NewQueryInfoPages/QueryAuthenInfoPage.aspx");
			menuControl.AddSubMenu("֤��״̬��ѯ_new","NewQueryInfoPages/QueryAuthenStateInfoPage.aspx");
			

			menuControl.AddSubMenu("��Ȩ��ϵ��ѯ",spkfurl + "cgi-bin/authen_relation.cgi" 
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
