using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.WechatPay;
namespace TENCENT.OSS.CFT.KF.KF_Web.WebchatPay
{
    public partial class QueryRealtimeRepayment : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.lb_operatorID.Text = Session["uid"] as string;
            }
        }

        protected void btn_Query_Click(object sender, EventArgs e)
        {
            try
            {
                var listid = txt_listid.Text.Trim();
                var queryTime = txt_query_date.Value.Trim();
                if (listid.Length < 1)
                {
                    WebUtils.ShowMessage(this.Page, "请输入还款提现单号"); return;
                }
                if (queryTime.Length < 1)
                {
                    WebUtils.ShowMessage(this.Page, "请输入还款日期"); return;
                }
                DateTime date;
                if (!DateTime.TryParse(queryTime, out date))
                {
                    WebUtils.ShowMessage(this.Page, "请输入正确的还款日期"); return;
                }

                var bll = new WechatPayService();
                var dt = bll.QueryRealtimeRepayment(listid, date, 2);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    lab_Fbank_type.Text = row["Fbank_type"] as string;
                    lab_Fbank_billno.Text = row["Fbank_billno"] as string;
                    lab_Fcreate_time.Text = row["Fcreate_time"] as string;
                    lab_Flistid.Text = row["Flistid"] as string;
                    lab_Fstatus_str.Text = row["Fstatus_str"] as string;
                    lab_Fret_code.Text = row["Fret_code"] as string;
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "没有找到数据"); return;
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "查询出错" + PublicRes.GetErrorMsg(ex.Message));
            }
        }
    }
}