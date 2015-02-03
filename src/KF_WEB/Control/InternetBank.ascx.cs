using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
    public partial class InternetBank : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }

        private void InitControl()
        {
            menuControl.Title = "网银查询";
        }

        public void AddSubMenu(string menuName, string url)
        {
            menuControl.AddSubMenu(menuName, url);
        }
    }
}