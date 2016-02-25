using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using TENCENT.OSS.CFT.KF.Common;
using System.Configuration;

namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{

	/// <summary>
	///		TradeManageControl ��ժҪ˵����
	/// </summary>
	public partial class TradeManageControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl ;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if ( !IsPostBack )
			{
				menuControl.Title = "���ײ�ѯ" ;
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
			
			menuControl.Title = "���ײ�ѯ" ;
//
			string szkey = Session["SzKey"].ToString().Trim();
			int operid = Int32.Parse(Session["OperID"].ToString().Trim());
			string loginname = Session["uid"].ToString().Trim();

			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
                menuControl.AddSubMenu("���ּ�¼��ѯ", "TradeManage/PickQueryNew.aspx");
				menuControl.AddSubMenu("�̻������嵥","TradeManage/TradeLogList.aspx") ;
				menuControl.AddSubMenu("�˿��ѯ","TradeManage/B2CReturnQuery.aspx") ;
				menuControl.AddSubMenu("���۵��ʲ�ѯ","NewQueryInfoPages/QueryDKInfoPage.aspx");
				menuControl.AddSubMenu("����������ѯ","NewQueryInfoPages/QueryDKListInfoPage.aspx");
				menuControl.AddSubMenu("�̻�������ѯ�Ƹ�ͨ����","TradeManage/QuerySpOrderPage.aspx") ;
			}

            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeManagement", this))
			{
				menuControl.AddSubMenu("���׼�¼��ѯ","TradeManage/TradeLogQuery.aspx") ;
			}

            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeManagement", this))
			{
				menuControl.AddSubMenu("��ֵ��¼��ѯ","TradeManage/FundQuery.aspx") ;
			}

		}

		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}
	}
}
