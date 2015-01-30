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
	///		MicroPay ��ժҪ˵����
	/// </summary>
	public partial class MicroPay : System.Web.UI.UserControl
	{
		protected MenuControl menuControl;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

			if(!IsPostBack)
			{
				menuControl.Title = "΢֧��";
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
			menuControl.Title = "΢֧��";

			string szkey = Session["SzKey"].ToString();
			int operid = Int32.Parse(Session["OperID"].ToString());

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "ChangeUserInfo"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("ChangeUserInfo",this))
			{
				menuControl.AddSubMenu("���ʻ���ѯ","BaseAccount/ChildrenQuery.aspx");
				menuControl.AddSubMenu("���ʻ�������ѯ","BaseAccount/ChildrenOrderFromQuery.aspx");
				menuControl.AddSubMenu("���ʻ�������ѯ(��)","BaseAccount/ChildrenOrderFromQueryNew.aspx");
			}
		}

		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}
	}
}
