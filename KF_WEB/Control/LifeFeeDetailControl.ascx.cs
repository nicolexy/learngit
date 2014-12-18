namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using TENCENT.OSS.CFT.KF.Common;
	using TENCENT.OSS.CFT.KF.DataAccess;


	/// <summary>
	///		LifeFeeDetailControl ��ժҪ˵����
	/// </summary>
	public partial class LifeFeeDetailControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if(!IsPostBack)
			{
				menuControl.Title = "����ɷ�";
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
			menuControl.Title = "����ɷ�";

			string szkey = Session["SzKey"].ToString();
			int operid = Int32.Parse(Session["OperID"].ToString());

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
				menuControl.AddSubMenu("����ɷѲ�ѯ","TradeManage/FeeQuery.aspx") ;
				menuControl.AddSubMenu("�ʴ�����ѯ","RemitCheck/RemitQuery.aspx");
				menuControl.AddSubMenu("�ֻ���ֵ����ѯ","TradeManage/FundCardQuery_Detail.aspx") ;
				menuControl.AddSubMenu("���ÿ�����","TradeManage/CreditQuery.aspx") ;
				menuControl.AddSubMenu("���ѷ�����ѯ", "TradeManage/PhoneBillQuery.aspx");
			}

		}

		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}
	}
}
