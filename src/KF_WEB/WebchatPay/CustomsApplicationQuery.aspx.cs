using CFT.CSOMS.BLL.WechatPay;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.WebchatPay
{
    public partial class CustomsApplicationQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string uid = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
        }


        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                string partner = txtpartner.Text.Trim();
                Bind(partner);
            }
            catch (Exception ex) 
            {
                WebUtils.ShowMessage(this.Page, "调用服务出错！" + HttpUtility.JavaScriptStringEncode(ex.Message));
            }
        }
        private void Bind(string partner)
        {
            DataSet ds = new WechatPayService().QueryMerchantCustom(partner);
            DataTable dt1 = ds.Tables[0];
            txt_partner.Text = dt1.Rows[0]["partner"].ToString();
            txt_sp_name.Text = dt1.Rows[0]["sp_name"].ToString();
            txt_merchant_type.Text = dt1.Rows[0]["merchant_type_str"].ToString();
            txt_contact_email.Text = dt1.Rows[0]["contact_email"].ToString();
            txt_contact_name.Text = dt1.Rows[0]["contact_name"].ToString();
            txt_contact_phone.Text = dt1.Rows[0]["contact_phone"].ToString();
            DataTable dt2 = ds.Tables[1];
            DataGrid1.DataSource = dt2;
            DataGrid1.DataBind();
        }
    }
}