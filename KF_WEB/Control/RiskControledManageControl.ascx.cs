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
	///		RiskControledManageControl ��ժҪ˵����
	/// </summary>
	public class RiskControledManageControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl;
		private void Page_Load(object sender, System.EventArgs e)
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
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion


		private void InitControl()
		{
			menuControl.Title = "��ع���" ;

			string szkey = Session["SzKey"].ToString();
			int operid = Int32.Parse(Session["OperID"].ToString());

			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
			{
				menuControl.AddSubMenu("���ε�¼���볷��","Trademanage/SuspendSecondPasseword.aspx") ;
			}

			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter") || AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "DeleteCrt"))
			{
				menuControl.AddSubMenu("����֤�����","Trademanage/CrtQuery.aspx") ;
			}

			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "FreezeList"))
			{
				menuControl.AddSubMenu("���������ѯ","BaseAccount/FreezeList.aspx") ;
				menuControl.AddSubMenu("�����ʽ��¼","BaseAccount/FreezeFinQuery.aspx");
			}


		}
	}
}
