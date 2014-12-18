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
	///		FreezeManageControl ��ժҪ˵����
	/// </summary>
	public partial class FreezeManageControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

			if(!IsPostBack)
			{
				this.menuControl1.Title = "�������";
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
			this.menuControl1.Title = "�������";

			string szkey = Session["SzKey"].ToString();
			//int operid = Int32.Parse(Session["OperID"].ToString());

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
				//this.menuControl1.AddSubMenu("��ؽⶳ���","FreezeManage/FreezeQuery.aspx");
				//this.menuControl1.AddSubMenu("����ͳ�����","FreezeManage/FreezeCount.aspx");
				//this.menuControl1.AddSubMenu("��ݻظ�����ҳ��","FreezeManage/FastReplyManagePage.aspx");
				//this.menuControl1.AddSubMenu("���Ե�ҳ��","NewQueryInfoPages/TestPage.aspx");
			}
		}
	}
}
