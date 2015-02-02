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
	///		AccountLedgerManageControl ��ժҪ˵����
	/// </summary>
	public partial class AccountLedgerManageControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

			if(!IsPostBack)
			{
				menuControl.Title = "�̻�����";
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
			menuControl.Title = "�̻�����";

			string szkey = Session["SzKey"].ToString();
			int operid = Int32.Parse(Session["OperID"].ToString());

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
				menuControl.AddSubMenu("���˶�����ˮ","BaseAccount/SeparateOperation.aspx") ;
				menuControl.AddSubMenu("���˶������","TradeManage/SeparateListQuery.aspx") ;
				menuControl.AddSubMenu("��֧�����ѯ","TradeManage/SettleRule.aspx") ;
				menuControl.AddSubMenu("���˶�����ѯ","TradeManage/SettleInfo.aspx") ;
				menuControl.AddSubMenu("����ⶳ��ѯ","TradeManage/SettleFreeze.aspx") ;
				menuControl.AddSubMenu("���˶�����ѯ","TradeManage/AdjustList.aspx") ;
				menuControl.AddSubMenu("������˹�ϵ","TradeManage/SettleAgent.aspx") ;
			}
		}


		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}
	
	}
}
