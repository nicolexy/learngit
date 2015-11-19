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
        protected MenuControl menuControl;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��
            if (!IsPostBack)
            {
                menuControl.Title = "�˻�����";
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

            menuControl.Title = "�˻�����";

            string szkey = Session["SzKey"].ToString();
            //int operid = Int32.Parse(Session["OperID"].ToString());

            //if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
            {

                menuControl.AddSubMenu("�����˻���Ϣ", "BaseAccount/InfoCenter.aspx");
                menuControl.AddSubMenu("QQ�ʺŻ���", "BaseAccount/QQReclaim.aspx");
                menuControl.AddSubMenu("�˻������޸�", "BaseAccount/changeUserName_2.aspx");
                menuControl.AddSubMenu("�û��ܿ��ʽ��ѯ", "TradeManage/QueryUserControledFinPage.aspx");
                menuControl.AddSubMenu("�ֻ��󶨲�ѯ", "TradeManage/MobileBindQuery.aspx");
            }

            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
            {
                menuControl.AddSubMenu("�����˺���Ϣ", "BaseAccount/UserBankInfoQuery.aspx");
            }


            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
            {
                menuControl.AddSubMenu("������Ϣ", "BaseAccount/ChangeUserInfo.aspx");
            }

            //rayguo 20060302  
            //if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "UserReport"))
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("UserReport", this))
            {
                menuControl.AddSubMenu("���Ͷ�߲�ѯ", "BaseAccount/userReport.aspx");
            }

            //if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "HistoryModify"))
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("HistoryModify", this))
            {
                menuControl.AddSubMenu("��Ϣ�޸���ʷ", "BaseAccount/historyModify.aspx");
            }

        }

        public void AddSubMenu(string menuName, string url)
        {
            menuControl.AddSubMenu(menuName, url);
        }
    }

}

