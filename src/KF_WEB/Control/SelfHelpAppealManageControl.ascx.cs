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
	///		SelfHelpAppealManageControl ��ժҪ˵����
	/// </summary>
	public partial class SelfHelpAppealManageControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if(!IsPostBack)
			{
				menuControl.Title = "��������";
				//InitControl();
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
			menuControl.Title = "��������";

			string szkey = Session["SzKey"].ToString();
			int operid = Int32.Parse(Session["OperID"].ToString());

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
				menuControl.AddSubMenu("�ͷ�ͳ�Ʋ�ѯ","BaseAccount/KFTotalQuery.aspx") ;
			}


			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "CFTUserAppeal"))
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
			{
				menuControl.AddSubMenu("�������߲�ѯ","BaseAccount/CFTUserAppeal.aspx") ;
			}


			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "CFTUserPick"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("CFTUserPick",this))
			{
				//menuControl.AddSubMenu("�������ߴ���","BaseAccount/CFTUserPick.aspx") ; ҳ���ѷϳ�
				//menuControl.AddSubMenu("ʵ����֤����","BaseAccount/UserClass.aspx") ; ҳ���ѷϳ�
				menuControl.AddSubMenu("���ߴ���(��)","BaseAccount/UserAppeal.aspx") ;
			}

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "CancelAccount"))
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
			{
				menuControl.AddSubMenu("�ʻ�������¼","BaseAccount/logOnUser.aspx") ;
			}

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "UpdateAccountQQ"))
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
			{
				menuControl.AddSubMenu("�ʻ�QQ�޸�","BaseAccount/ChangeQQOld.aspx") ;
			}

		}

		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}
	}
}
