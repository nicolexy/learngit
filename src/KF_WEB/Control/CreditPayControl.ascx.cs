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
    ///		CreditPayControl ��ժҪ˵����
	/// </summary>
    public partial class CreditPayControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl ;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if ( !IsPostBack )
			{
				menuControl.Title = "����֧��" ;
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

            //menuControl.Title = "����֧��";

			string szkey = Session["SzKey"].ToString();
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
                menuControl.AddSubMenu("������Ϣ", "CreditPay/QueryCreditUserInfo.aspx");
                menuControl.AddSubMenu("�˵���ѯ", "CreditPay/QueryCreditBillList.aspx");
                menuControl.AddSubMenu("Ƿ���ѯ", "CreditPay/QueryCreditDebt.aspx");
                menuControl.AddSubMenu("�����ѯ", "CreditPay/QueryRefund.aspx");
                //menuControl.AddSubMenu("�˻���ѯ", "CreditPay/AccountSearch.aspx");
                //menuControl.AddSubMenu("�˵���ѯ", "CreditPay/BillSearch.aspx");
                //menuControl.AddSubMenu("��ϸ��ѯ", "CreditPay/DetailSearch.aspx");
            }
		}
			
		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}
	}



}

