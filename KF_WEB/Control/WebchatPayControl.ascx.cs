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
    ///		WebchatPayControl ��ժҪ˵����
	/// </summary>
    public partial class WebchatPayControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl ;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if ( !IsPostBack )
			{
				menuControl.Title = "΢��֧��" ;
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

            //menuControl.Title = "΢��֧��";

			string szkey = Session["SzKey"].ToString();
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
                menuControl.AddSubMenu("΢��֧���ʺ�", "WebchatPay/WechatInfoQuery.aspx");
                menuControl.AddSubMenu("AA�տ��ʺ�", "WebchatPay/WechatAACollection.aspx");
                menuControl.AddSubMenu("���ͨ��ѯ", "NewQueryInfoPages/GetFundRatePage.aspx");
                menuControl.AddSubMenu("���ͨ��ȫ��", "WebchatPay/SafeCardManage.aspx");
                menuControl.AddSubMenu("΢�ź����ѯ", "WebchatPay/WechatRedPacket.aspx");
                menuControl.AddSubMenu("С��ˢ��", "WebchatPay/SmallCreditCardQuery.aspx");
                menuControl.AddSubMenu("΢�����ÿ�����", "WebchatPay/CreditCardRefundQuery.aspx");
			}
		}
			
		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}
	}



}

