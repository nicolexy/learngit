using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TENCENT.OSS.CFT.KF.KF_Web
{
    public partial class CheckLogin : System.Web.UI.Page
    {
        //这个页面的作用是  定时刷新session 防止过期  在top.aspx页面中定义了  定时js
        protected void Page_Load(object sender, EventArgs e)
        {
            var SzKey = Session["SzKey"] as string;
            var OperID = Session["OperID"] as string;
            if (SzKey == null || OperID == null)
            {
                Response.Redirect("/login.aspx?wh=1");
            }
            Response.End();
        }
    }
}