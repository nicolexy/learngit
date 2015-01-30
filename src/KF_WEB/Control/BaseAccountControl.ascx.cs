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
	///		BaseAccountControl ��ժҪ˵����
	/// </summary>
	public partial class BaseAccountControl : System.Web.UI.UserControl
	{
		protected MenuControl menuControl ;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if ( !IsPostBack )
			{
				menuControl.Title = "�˻�����" ;
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

			menuControl.Title = "�˻�����" ;

			string szkey = Session["SzKey"].ToString();
			//int operid = Int32.Parse(Session["OperID"].ToString());

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
			{
				
				//menuControl.AddSubMenu("һ��ͨҵ��","BaseAccount/BankCardUnbind.aspx") ;
				//menuControl.AddSubMenu("�ͷ�ͳ�Ʋ�ѯ","BaseAccount/KFTotalQuery.aspx") ;
				//menuControl.AddSubMenu("ϵͳ�������","BaseAccount/SysBulletinManage.aspx") ;
				//menuControl.AddSubMenu("���ε�¼���볷��","Trademanage/SuspendSecondPasseword.aspx") ;
				//menuControl.AddSubMenu("PNRǩԼ��ϵ��ѯ","BaseAccount/PNRQuery.aspx") ;
				//menuControl.AddSubMenu("����ǩԼ��Լ��Ϣ","NewQueryInfoPages/QueryInverestorSignPage.aspx") ;
				//menuControl.AddSubMenu("�����ײ�ѯ","NewQueryInfoPages/QueryFundInfoPage.aspx") ;
				//menuControl.AddSubMenu("�����ֵ������Ϣ","NewQueryInfoPages/QueryChargeInfoPage.aspx") ;
				//menuControl.AddSubMenu("�������˻���Ϣ��ѯ","NewQueryInfoPages/GetUserFundAccountInfoPage.aspx") ;
				menuControl.AddSubMenu("�����˻���Ϣ","BaseAccount/InfoCenter.aspx") ;
				menuControl.AddSubMenu("QQ�ʺŻ���","BaseAccount/QQReclaim.aspx") ;
				menuControl.AddSubMenu("�˻������޸�","BaseAccount/changeUserName_2.aspx");
				menuControl.AddSubMenu("�û��ܿ��ʽ��ѯ","TradeManage/QueryUserControledFinPage.aspx");
				menuControl.AddSubMenu("�ֻ��󶨲�ѯ","TradeManage/MobileBindQuery.aspx") ;
			}
			/*
			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter") || AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "DeleteCrt"))
			{
				menuControl.AddSubMenu("����֤�����","Trademanage/CrtQuery.aspx") ;
			}
			*/

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "UserBankInfoQuery"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("UserBankInfoQuery",this))
			{
				menuControl.AddSubMenu("�����˺���Ϣ","BaseAccount/UserBankInfoQuery.aspx") ;
			}


			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "ChangeUserInfo"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("ChangeUserInfo",this))
			{
				menuControl.AddSubMenu("������Ϣ","BaseAccount/ChangeUserInfo.aspx") ;
				//menuControl.AddSubMenu("���ʻ���ѯ","BaseAccount/ChildrenQuery.aspx") ;
				//menuControl.AddSubMenu("���ʻ�������ѯ","BaseAccount/ChildrenOrderFromQuery.aspx") ;
				//menuControl.AddSubMenu("���ʻ�������ѯ(��)","BaseAccount/ChildrenOrderFromQueryNew.aspx") ;
			}

			//furion 20050906
			/*
			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "FreezeList"))
			{
				menuControl.AddSubMenu("���������ѯ","BaseAccount/FreezeList.aspx") ;
				menuControl.AddSubMenu("�����ʽ��¼","BaseAccount/FreezeFinQuery.aspx");
			}
			*/
			
			//rayguo 20060302  
			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "UserReport"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("UserReport",this))
			{
				menuControl.AddSubMenu("���Ͷ�߲�ѯ","BaseAccount/userReport.aspx") ;
			}

			//if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "HistoryModify"))
			if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("HistoryModify",this))
			{
				menuControl.AddSubMenu("��Ϣ�޸���ʷ","BaseAccount/historyModify.aspx") ;
			}

			/*  ҳ����ת��
			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "CFTUserAppeal"))
			{
				menuControl.AddSubMenu("�������߲�ѯ","BaseAccount/CFTUserAppeal.aspx") ;
			}
			

			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "CFTUserPick"))
			{
				//menuControl.AddSubMenu("�������ߴ���","BaseAccount/CFTUserPick.aspx") ; ҳ���ѷϳ�
				//menuControl.AddSubMenu("ʵ����֤����","BaseAccount/UserClass.aspx") ; ҳ���ѷϳ�
				menuControl.AddSubMenu("���ߴ���(��)","BaseAccount/UserAppeal.aspx") ;
			}
			*/
			
			/*
			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "CFTUserPickTJ"))
			{
			//	menuControl.AddSubMenu("���ߴ���ͳ��","BaseAccount/CFTUserPickTJ.aspx") ;  ҳ���ѷϳ�	
			//	menuControl.AddSubMenu("���ߴ����ѯ","BaseAccount/CFTAppealQuery.aspx") ;   ҳ���ѷϳ�	
			//	menuControl.AddSubMenu("ʵ������ͳ��","BaseAccount/UserClassTJ.aspx") ; ҳ���ѷϳ�
				//menuControl.AddSubMenu("ʵ�������ѯ","BaseAccount/UserClassQuery.aspx") ;
			}
			*/
			
			//edwinyang 20061018
//			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "QueryQQ"))
//			{
//				menuControl.AddSubMenu("QQ�����ѯ","BaseAccount/QueryQQ.aspx") ;
//			}


			/*   ҳ����ת��
			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "CancelAccount"))
			{
				menuControl.AddSubMenu("�ʻ�������¼","BaseAccount/logOnUser.aspx") ;
			}

			if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "UpdateAccountQQ"))
			{
				menuControl.AddSubMenu("�ʻ�QQ�޸�","BaseAccount/ChangeQQOld.aspx") ;
			}
			*/

		}
			
//			menuControl.AddSubMenu("����ⶳ","BaseAccount/BalanceFreeze.aspx") ;	


		public void AddSubMenu(string menuName,string url)
		{
			menuControl.AddSubMenu(menuName,url) ;
		}
	}



}

