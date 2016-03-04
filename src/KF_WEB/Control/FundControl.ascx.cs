using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
    public partial class FundControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            if (!IsPostBack)
            {
                menuControl.Title = "理财通";
                //InitControls() ;
            }
        }

        public void AddSubMenu(string menuName, string url)
        {
            menuControl.AddSubMenu(menuName, url);
        }
    }
}