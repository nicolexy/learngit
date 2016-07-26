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
    public partial class CustomsPushingQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
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
                string partner = txt_partner.Text.Trim();
                string transaction_id = txt_transaction_id.Text.Trim();
                string out_trade_no = txt_out_trade_no.Text.Trim();
                string sub_order_no = txt_sub_order_no.Text.Trim();
                string sub_order_id = txt_sub_order_id.Text.Trim();
                Bind(partner, transaction_id, out_trade_no, sub_order_no, sub_order_id);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.Message));
            }
        }

        private void Bind(string partner, string transaction_id, string out_trade_no, string sub_order_no, string sub_order_id)
        {
            tbDetail.Visible = true;

            DataSet ds = new WechatPayService().QueryDeclareDogInfo(partner, transaction_id, out_trade_no, sub_order_no, sub_order_id);
            DataTable dt1 = ds.Tables[0];
            txt_partner1.Text = dt1.Rows[0]["partner"].ToString();
            txt_transaction_id1.Text = dt1.Rows[0]["transaction_id"].ToString();
            txt_out_trade_no1.Text = dt1.Rows[0]["out_trade_no"].ToString();
            txt_count.Text = dt1.Rows[0]["count"].ToString();

            DataTable dt2 = ds.Tables[1];
            DataGrid1.DataSource = dt2;
            DataGrid1.DataBind();
        }
    }
}