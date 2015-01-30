namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		SpecialManageControl ��ժҪ˵����
	/// </summary>
	public partial class SpecialManageControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if(!IsPostBack)
			{
				InitControl();
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
			this.menuControl.Title = "��־����";

			string szkey = Session["SzKey"].ToString();
			string operid = Session["OperID"].ToString();
			string uid = Session["uid"].ToString();

			if(uid == "alexguan" || operid == "1100000000")
			{
				this.menuControl.AddSubMenu("��ݻظ�����ҳ��","FreezeManage/FastReplyManagePage.aspx");
				this.menuControl.AddSubMenu("���Ե�ҳ��","NewQueryInfoPages/TestPage.aspx");
				this.menuControl.AddSubMenu("�ͷ���־����","NewQueryInfoPages/KFDiaryManagePage.aspx");
				this.menuControl.AddSubMenu("�Ƹ�����Ϣ��ѯ","BaseAccount/CFDQuery.aspx");
			}
		}
	}
}
