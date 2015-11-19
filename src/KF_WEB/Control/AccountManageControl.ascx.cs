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
	///		BaseAccountControl ��ժҪ˵����
	/// </summary>
	public partial class AccountManageControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl ;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if ( !IsPostBack )
			{
				menuControl.Title = "�̻�����";
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
			menuControl.Title = "�̻�����";

			string szkey = Session["SzKey"].ToString();

			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
	
				menuControl.AddSubMenu("���ֹ����ѯ","TradeManage/AppealDSettings.aspx") ;
				menuControl.AddSubMenu("��������ѯ","TradeManage/AppealSSetting.aspx") ;				
                menuControl.AddSubMenu("�����˿��ѯ","TradeManage/SettleRefund.aspx") ;
				menuControl.AddSubMenu("ֱ���̻���ѯ","BaseAccount/PayBusinessQuery.aspx") ;
				menuControl.AddSubMenu("�н��̻���ѯ","BaseAccount/AgencyBusinessQuery.aspx") ;
				menuControl.AddSubMenu("�����ѯ","TradeManage/SettQuery.aspx") ;
				menuControl.AddSubMenu("�����ѯ(��)","TradeManage/SettQueryNew.aspx") ;
				menuControl.AddSubMenu("֤������ѯ","TradeManage/MediCertManage.aspx") ;
				menuControl.AddSubMenu("�Զ��۷�Э��","TradeManage/PayLimitManage.aspx") ;
				menuControl.AddSubMenu("T+0�����ѯ","TradeManage/BatPayQuery.aspx") ;
				menuControl.AddSubMenu("PNRǩԼ��ϵ��ѯ","BaseAccount/PNRQuery.aspx") ;
				menuControl.AddSubMenu("ͬ����¼��ѯ","TradeManage/SynRecordQuery.aspx") ;
			}
		}

		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}

	}

}

