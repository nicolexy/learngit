using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CFT.CSOMS.BLL.ForeignCurrencyModule;
using Tencent.DotNet.Common.UI;
using System.Data;

namespace TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay
{
    public partial class FCXGOrderQuery : System.Web.UI.Page
    {
        string uid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.lb_operatorID.Text = uid = Session["uid"] as string;
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
                {
                    Response.Redirect("../login.aspx?wh=1");
                }
            }
        }

        //查询按钮
        protected void Button1_Click(object sender, EventArgs e)
        {
            var spid = txt_spid.Text.Trim();
            var sp_billno = txt_sp_billno.Text.Trim();
            var cardno = txt_card_no.Text.Trim();
            var cardQueryDate = txt_cardQuery_date.Value.Trim();//2015-07-27
            var mdlistid = txt_mdlistid.Text.Trim();
            tab_order_info.Visible = false;
            DataTable dt = null;
            try
            {
                var bll = new FCXGWallet();
                if (spid.Length != 0 && sp_billno.Length != 0)  //商户
                {
                    dt = bll.QueryOrderBySp(spid, sp_billno);
                }
                else if (cardno.Length != 0)    //银行卡
                {
                    DateTime date;
                    if (!DateTime.TryParse(cardQueryDate, out date))
                    {
                        throw new Exception("请输入正确的时间");
                    }

                    dt = bll.QueryOrderByCardNo(cardno, date, Request.UserHostAddress);
                }
                else if (mdlistid.Length != 0)  //MD
                {
                    dt = bll.QueryOrderByMDList(mdlistid);
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "(商户订单号、商户编号)组合查询 / MD订单号 / 银行卡号,以上至少一项有值"); return;
                }

                if (dt == null) WebUtils.ShowMessage(this.Page, "没有找到数据");
                ViewState["cacheOrderDataTable"] = dt;
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "查询订单出现异常:" + PublicRes.GetErrorMsg(ex.Message));
            }
            dg_OrderInfo.DataSource = dt;
            dg_OrderInfo.DataBind();
        }

        protected void dg_OrderInfo_Select(object sende, EventArgs e)
        {
            var dg = sende as DataGrid;
            tab_order_info.Visible = true;
            var dt = ViewState["cacheOrderDataTable"] as DataTable;
            var row = dt.Rows[dg.SelectedIndex];

            lb_acc_time.Text = row["acc_time"] as string;
            lb_coding.Text = row["coding"] as string;
            lb_listid.Text = row["listid"] as string;
            lb_paynum_str.Text = row["paynum_str"] as string;
            lb_sp_name.Text = row["sp_name"] as string;
            lb_spid.Text = row["spid"] as string;
            lb_buy_uid.Text = row["buy_uid"] as string;
            lb_sp_uid.Text = row["sp_uid"] as string;
            lb_card_curtype_str.Text = row["card_curtype_str"] as string;
            lb_refund_paynum_str.Text = row["total_payout_str"] as string;  // total_payout 总出款金额（退款和拒付金额的和)
            lb_refund_state_str.Text = row["refund_state_str"] as string;
            lb_refund_time_acc.Text = row["refund_time_acc"] as string;
            lb_trade_state_str.Text = row["trade_state_str"] as string;
            lb_rec_banklist.Text = row["rec_banklist"] as string;
            lb_appeal_sign_str.Text = row["appeal_sign_str"] as string;

            lb_card_bank_type.Text = row["card_bank_type"] as string;
            lb_create_time.Text = row["create_time"] as string;
            lb_create_time_spid.Text = row["create_time_spid"] as string;
            lb_bargain_time.Text = row["bargain_time"] as string;
            lb_pay_time.Text = row["pay_time"] as string;
            lb_buy_uin.Text = row["buy_uin"] as string;
            lb_sp_acno.Text = row["sp_acno"] as string;
            lb_price_str.Text = row["price_str"] as string;
            lb_price_curtype_str.Text = row["price_curtype_str"] as string;
        }
    }
}