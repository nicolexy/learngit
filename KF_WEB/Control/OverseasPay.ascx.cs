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
	///		OverseasPay ��ժҪ˵����
	/// </summary>
	public partial class OverseasPay : System.Web.UI.UserControl
	{

		protected MenuControl menuControl ;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if ( !IsPostBack )
			{
				InitControls() ;
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

			menuControl.Title = "����֧��" ;

			string szkey = Session["SzKey"].ToString();
			int operid = Int32.Parse(Session["OperID"].ToString());
		
			menuControl.AddSubMenu("����֧����ѯ","BaseAccount/OverseasPayQuery.aspx") ;
            menuControl.AddSubMenu("�����յ��˿�", "BaseAccount/OverseasReturnQuery.aspx");
		}

		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		} 
			
	}
}
