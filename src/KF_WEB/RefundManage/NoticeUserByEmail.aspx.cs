using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CFT.CSOMS.BLL.RefundModule;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace TENCENT.OSS.CFT.KF.KF_Web.RefundManage
{
    public partial class NoticeUserByEmail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
  
                operatorID.Text = Session["OperID"].ToString();
                ViewState["truename"] = Request.QueryString["truename"].ToString();
                ViewState["paybanklist"] = Request.QueryString["paybanklist"].ToString();
                ViewState["oldid"] = Request.QueryString["oldid"].ToString();

        }

        private void showMsg(string msg)
        {
            Response.Write("<script language=javascript>alert('" + msg + "')</script>");
        }
        protected void BtnNoticeUser_Click(object sender, EventArgs e)
        {
            string strToMail = txtEmailID.Text.Trim();
            if (string.IsNullOrEmpty(strToMail))
            {
                showMsg("请输入邮箱号码。");
                return;
            }

            string strOldId = ViewState["oldid"].ToString();
            RefundService server = new RefundService();
            if (server.RequestItemState(strOldId).Trim() != "0" && server.RequestItemState(strOldId).Trim() != "1")
            {
                showMsg("订单不处于发邮件通知用户状态。");
                return;
            }
            
            string Report_Url = string.Format("https://www.tenpay.com/v2/cs/refund/apply_refund.shtml?spcl_refund_id={0}", ViewState["oldid"].ToString());

            string strParams = string.Format("nickname={0}&OrderNo={1}&Report_Url={2}&Report_Url={3}&Report_Url={4}", ViewState["truename"].ToString(), ViewState["paybanklist"].ToString(), Report_Url, Report_Url, Report_Url);
            CommMailSend.SendMsg(strToMail, "2473", strParams);

            server.SetRefundCheckState(1, strOldId);
        }
    }
}