using CFT.CSOMS.BLL.TradeModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.CFTAccountModule;
using CFT.Apollo.Logging;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
    public partial class TransferQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.lb_operatorID.Text = Session["uid"] as string;
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
                {
                    Response.Redirect("../login.aspx?wh=1");
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            int queryType = 0;
            string orderNumber = string.Empty;            //商户订单号或财付通订单号
            string fuid = string.Empty;              //财付通账户对应内部UID
            try
            {
                if (check_sp.Checked)
                {
                    //商户订单号
                    queryType = 2;
                    orderNumber = txt_SpOrder.Text.Trim();
                    string caiFuTongAccount = txt_CaiFuTongAccount.Text.Trim();//财付通账户
                    if (string.IsNullOrEmpty(orderNumber))
                    {
                        tab_transfer_info.Visible = false;
                        WebUtils.ShowMessage(this.Page, "请输入商户订单号");
                        return;
                    }
                    else if (string.IsNullOrEmpty(caiFuTongAccount))
                    {
                        tab_transfer_info.Visible = false;
                        WebUtils.ShowMessage(this.Page, "请输入财付通账户");
                        return;
                    }
                    fuid = new AccountService().QQ2Uid(caiFuTongAccount);
                }
                else if (check_tenpay.Checked)
                {
                    //财付通订单
                    queryType = 1;
                    orderNumber = txt_order.Text.Trim();
                    if (string.IsNullOrEmpty(orderNumber))
                    {
                        tab_transfer_info.Visible = false;
                        WebUtils.ShowMessage(this.Page, "请输入财付通订单号");
                        return;
                    }
                }

                var bll = new TradeService();
                var ds = bll.TransferQuery(orderNumber, queryType, fuid);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var row = ds.Tables[0].Rows[0];
                    lab_Fbussredo_sign.Text = row["Fbussredo_sign"] as string;
                    lab_Fbuy_uid.Text = row["Fbuy_uid"] as string;
                    lab_Fbuy_uin.Text = row["Fbuy_uin"] as string;
                    lab_Fchannel_id.Text = row["Fchannel_id"] as string;
                    lab_Fexplain.Text = row["Fexplain"] as string;
                    lab_Flist_type_str.Text = row["Flist_type_str"] as string;
                    lab_Flistid.Text = row["Flistid"] as string;
                    lab_Fmemo.Text = row["Fmemo"] as string;
                    lab_Fmodify_time.Text = row["Fmodify_time"] as string;
                    lab_Forigin_listid.Text = row["Forigin_listid"] as string;
                    lab_Forigin_sp_billno.Text = row["Forigin_sp_billno"] as string;
                    lab_Fsale_uid.Text = row["Fsale_uid"] as string;
                    lab_Fsale_uin.Text = row["Fsale_uin"] as string;
                    lab_Fsequence_no.Text = row["Fsequence_no"] as string;
                    //lab_Fsequence_no_only.Text = row["Fsequence_no"] as string;
                    lab_Fsource_time_stamp.Text = row["Fsource_time_stamp"] as string;
                    lab_Fsubtrans_no.Text = row["Fsubtrans_no"] as string;
                    lab_Ftotalnum_str.Text = row["Ftotalnum_str"] as string;
                    lab_Ftrans_no.Text = row["Ftrans_no"] as string;
                    lab_Ftrans_time.Text = row["Ftrans_time"] as string;

                    tab_transfer_info.Visible = true;
                }
                else
                {
                    tab_transfer_info.Visible = false;
                    WebUtils.ShowMessage(this.Page, "没有找到订单");
                }
            }
            catch (Exception ex)
            {
                tab_transfer_info.Visible = false;
                WebUtils.ShowMessage(this.Page, PublicRes.GetErrorMsg(ex.Message));
            }
        }
    }
}