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
	///		RiskConManage ��ժҪ˵����
	/// </summary>
	public partial class RiskConManage : System.Web.UI.UserControl
	{
		protected MenuControl menuControl ;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if ( !IsPostBack )
			{
				menuControl.Title = "��ع���" ;
				//InitControls() ;
			}
		}

		private void InitControls()
		{
			menuControl.Title = "��ع���" ;

			string spkfurl = System.Configuration.ConfigurationManager.AppSettings["RickConUrlPath"].Trim();
			if(!spkfurl.EndsWith("/"))
				spkfurl += "/";

			/*���ȡ���˸�ҳ��Ĳ�ѯ
			menuControl.AddSubMenu("�쳣��ʾ",spkfurl + "NotifyList.aspx") ;
			menuControl.AddSubMenu("�����ֻ�",spkfurl + "MaskMobile.aspx") ;
			menuControl.AddSubMenu("�쳣����",spkfurl + "BlockLog.aspx") ;
			menuControl.AddSubMenu("�쳣��½",spkfurl + "LoginViolation.aspx") ;*/
			menuControl.AddSubMenu("�ʽ���ˮ��ѯ","BaseAccount/BankrollHistoryLog.aspx") ;

			string szkey = Session["SzKey"].ToString();
			int operid = Int32.Parse(Session["OperID"].ToString());

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
				menuControl.AddSubMenu("���ε�¼���볷��","Trademanage/SuspendSecondPasseword.aspx") ;
			}

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter") || AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "DeleteCrt"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this) || TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("DeleteCrt",this))
			{
				menuControl.AddSubMenu("����֤�����","Trademanage/CrtQuery.aspx") ;
			}

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "FreezeList"))
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
			{
				menuControl.AddSubMenu("���������ѯ","BaseAccount/FreezeList.aspx") ;
				menuControl.AddSubMenu("�����ʽ��¼","BaseAccount/FreezeFinQuery.aspx");
			}

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
				menuControl.AddSubMenu("��ؽⶳ���","FreezeManage/FreezeQuery.aspx");
				menuControl.AddSubMenu("����ͳ�����","FreezeManage/FreezeCount.aspx");
				//this.menuControl1.AddSubMenu("��ݻظ�����ҳ��","FreezeManage/FastReplyManagePage.aspx");
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

		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}
	}
}
