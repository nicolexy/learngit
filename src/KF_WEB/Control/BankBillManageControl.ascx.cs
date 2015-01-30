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
	///		BankBillManageControl ��ժҪ˵����
	/// </summary>
	public partial class BankBillManageControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if(!IsPostBack)
			{
				menuControl.Title = "�����˵�";
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
			menuControl.Title = "�����˵�";

			string szkey = Session["SzKey"].ToString().Trim();
			//int operid = Int32.Parse(Session["OperID"].ToString().Trim());

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
				menuControl.AddSubMenu("ϵͳ�������","BaseAccount/SysBulletinManage.aspx") ;
				menuControl.AddSubMenu("�����˵�����","TradeManage/RefundMain.aspx") ;
				menuControl.AddSubMenu("����ʵʱ��ѯ","TradeManage/RealTimeOrderQuery.aspx") ;
				menuControl.AddSubMenu("�쳣���񵥹���","RefundManage/RefundErrorMain.aspx?WorkType=task");
				menuControl.AddSubMenu("�˵��쳣���ݲ�ѯ","RefundManage/RefundErrorHandle.aspx") ;
			}

			
			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "TradeLogQuery"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeLogQuery",this))
			{
				menuControl.AddSubMenu("���ܸ�������","BaseAccount/batPay.aspx") ;
			}

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "FundQuery"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("FundQuery",this))
			{
				menuControl.AddSubMenu("����ʵʱ����","TradeManage/RealtimeOrder.aspx") ;
			}
		}

		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}
	}
}
