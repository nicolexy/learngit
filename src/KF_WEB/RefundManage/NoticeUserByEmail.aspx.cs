using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TENCENT.OSS.CFT.KF.KF_Web.RefundManage
{
    public partial class NoticeUserByEmail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
  
                operatorID.Text = Session["OperID"].ToString();

        }

        protected void BtnNoticeUser_Click(object sender, EventArgs e)
        {

        }
    }
}