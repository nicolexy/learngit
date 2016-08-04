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
    public partial class CrossBorderRemittances : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                pager1.RecordCount = 10000;
                pager1.Visible = false;
                txt_start_date.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                txt_end_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        protected void btnSerach_Click(object sender, EventArgs e)
        {
            try
            {
                pager1.Visible = true;
                Bind();
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(ex.Message));
            }
        }
        protected void Bind() 
        {
            string WeChatId = txt_WeChatId.Text.Trim();
            string s_phone_no = txt_s_phone_no.Text.Trim();
            string wx_pay_id = txt_wx_pay_id.Text.Trim();
            string wx_pay_state = ddl_wx_pay_state.SelectedValue.Trim();
            string remit_type = ddl_remit_type.SelectedValue.Trim();
            string start_date = txt_start_date.Text.Trim();
            string end_date = txt_end_date.Text.Trim();
            int limit = 10;
            int offset = (pager1.CurrentPageIndex - 1) * limit;
          
//#if DEBUG
//            wx_pay_id = "1010110543201512300001144529";
//#endif
            DataTable dt = new WechatPayService().CrossBorderRemittances(WeChatId, s_phone_no, wx_pay_id, wx_pay_state,
                                                  remit_type, start_date, end_date, offset, limit);
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();
            panDetail.Visible = false;
        }

        protected void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager1.CurrentPageIndex = e.NewPageIndex;
            try
            {
                Bind();
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(ex.Message));
            }
        }

        protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "detail")
                {
                    panDetail.Visible = true;
                    string wx_pay_id = e.Item.Cells[0].Text.Replace("&nbsp;","").Trim();

                    DataTable dt = new WechatPayService().CrossBorderRemittances("", "", wx_pay_id, "", "", "", "", 0, 1);
                    foreach (var control in this.panDetail.Controls)
                    {
                        if (control is Label)
                        {
                            Label label = control as Label;
                            try
                            {
                                label.Text = dt.Rows[0][label.ID.Replace("lbl_", "")].ToString();
                            }
                            catch (Exception ex)
                            {
                                label.Text = ex.Message;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                panDetail.Visible = false;
                WebUtils.ShowMessage(this.Page, "查询详情失败！" + HttpUtility.JavaScriptStringEncode(ex.Message));
            }
        }
    }
}