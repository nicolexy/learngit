using CFT.CSOMS.BLL.TradeModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
    public partial class TransferQuery : System.Web.UI.Page
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
            try
            {
                if (check_sp.Checked)
                {
                    queryType = 2;
                }
                else if (check_tenpay.Checked)
                {
                    queryType = 1;
                }

                var bll = new TradeService();
                var ds = bll.TransferQuery(txt_order.Text, queryType);

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