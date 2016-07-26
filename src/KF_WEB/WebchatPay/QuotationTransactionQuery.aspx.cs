using CFT.CSOMS.BLL.FundModule;
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
    public partial class QuotationTransactionQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        int limit = 10;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pager1.RecordCount = 10000;
                pager1.Visible = false;
                txt_profit_end_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        protected void btnSerach_Click(object sender, EventArgs e)
        {
            try
            {
                pager1.CurrentPageIndex = 1;
                Bind();
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(ex.ToString()));
            }
        }

        private void Bind()
        {
            string QQID = UserNameControl1.QQID;
            //string Fuid = UserNameControl1.Fuid;
            ViewState["txt_QQID"] = QQID;
            string trade_id = new FundService().GetTradeIdByUIN(QQID);
            ViewState["trade_id"] = trade_id;
//#if DEBUG
//            trade_id = "20150908000028213";
//#endif

            //string fund_code = ddl_fund.SelectedValue.Trim();
            //string state = ddl_state.SelectedValue.Trim();
            DateTime Fdue_date = Convert.ToDateTime(txt_profit_end_date.Text.Trim());

            int offset = (pager1.CurrentPageIndex - 1) * limit;
            DataTable dt = new WechatPayService().Query_QuotationTransaction(trade_id, Fdue_date, offset, limit);
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();

        }

        protected void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager1.CurrentPageIndex = e.NewPageIndex;
            try
            {
                DataBind();
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(ex.ToString()));
            }
        }

        protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "detail")
                {
                    panDetail.Visible = true;
                    txt_QQID.Text = ViewState["txt_QQID"].ToString();
                    string fid = e.Item.Cells[0].Text.Replace("&nbsp;", "").Trim();
                    string trade_id = ViewState["trade_id"].ToString();
                    DateTime Fdue_date = Convert.ToDateTime(txt_profit_end_date.Text.Trim());
                    int offset = (pager1.CurrentPageIndex - 1) * limit;
                    DataTable dt = new WechatPayService().Query_QuotationTransaction(trade_id, Fdue_date, offset, limit);
                    DataRow dr = dt.Select(" Fid='" + fid + "'")[0];
                    
                    foreach (var control in this.panDetail.Controls)
                    {
                        if (control is Label)
                        {
                            Label label = control as Label;
                            if (label.ID.StartsWith("lbl_"))
                            {
                                try
                                {
                                    label.Text = dr[label.ID.Replace("lbl_", "")].ToString();
                                }
                                catch (Exception ex)
                                {
                                    label.Text = ex.Message;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                panDetail.Visible = false;
                WebUtils.ShowMessage(this.Page, "查询详情失败！" + HttpUtility.JavaScriptStringEncode(ex.ToString()));
            }
        }
    }
}