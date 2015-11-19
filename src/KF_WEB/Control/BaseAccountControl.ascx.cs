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
    ///		BaseAccountControl 的摘要说明。
    /// </summary>
    public partial class BaseAccountControl : System.Web.UI.UserControl
    {
        protected MenuControl menuControl;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            if (!IsPostBack)
            {
                menuControl.Title = "账户管理";
                //InitControls() ;
            }
        }

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		设计器支持所需的方法 - 不要使用代码编辑器
        ///		修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

        private void InitControls()
        {

            menuControl.Title = "账户管理";

            string szkey = Session["SzKey"].ToString();
            //int operid = Int32.Parse(Session["OperID"].ToString());

            //if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
            {

                menuControl.AddSubMenu("个人账户信息", "BaseAccount/InfoCenter.aspx");
                menuControl.AddSubMenu("QQ帐号回收", "BaseAccount/QQReclaim.aspx");
                menuControl.AddSubMenu("账户姓名修改", "BaseAccount/changeUserName_2.aspx");
                menuControl.AddSubMenu("用户受控资金查询", "TradeManage/QueryUserControledFinPage.aspx");
                menuControl.AddSubMenu("手机绑定查询", "TradeManage/MobileBindQuery.aspx");
            }

            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
            {
                menuControl.AddSubMenu("银行账号信息", "BaseAccount/UserBankInfoQuery.aspx");
            }


            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
            {
                menuControl.AddSubMenu("个人信息", "BaseAccount/ChangeUserInfo.aspx");
            }

            //rayguo 20060302  
            //if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "UserReport"))
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("UserReport", this))
            {
                menuControl.AddSubMenu("意见投诉查询", "BaseAccount/userReport.aspx");
            }

            //if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "HistoryModify"))
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("HistoryModify", this))
            {
                menuControl.AddSubMenu("信息修改历史", "BaseAccount/historyModify.aspx");
            }

        }

        public void AddSubMenu(string menuName, string url)
        {
            menuControl.AddSubMenu(menuName, url);
        }
    }

}

