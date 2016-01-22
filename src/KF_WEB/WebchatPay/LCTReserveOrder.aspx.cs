using CFT.CSOMS.BLL.CFTAccountModule;
using CFT.CSOMS.BLL.FundModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.WebchatPay
{
    public partial class LCTReserveOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pager1.RecordCount = 10000;
                pager1.PageSize = 5;
                txtStime.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                txtEtime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                pager1.CurrentPageIndex = 1;
                string stime = txtStime.Text;
                string etime = txtEtime.Text;

                if (!string.IsNullOrEmpty(stime))
                {
                    stime = DateTime.Parse(stime).ToString("yyyy-MM-dd");
                }
                if (!string.IsNullOrEmpty(etime))
                {
                    etime = DateTime.Parse(etime).ToString("yyyy-MM-dd");
                }
                if (!string.IsNullOrEmpty(stime) && !string.IsNullOrEmpty(etime))
                {
                    DateTime _dateStart = DateTime.Parse(stime);
                    DateTime _dateEnd = DateTime.Parse(etime);
                    if (_dateStart > _dateEnd)
                    {
                        throw new LogicException("开始日期不能大于结束日期！");
                    }
                    if (_dateEnd > _dateStart.AddDays(30))
                    {
                        throw new LogicException("时间跨度不能超过30天！");
                    }
                }
                ViewState["stime"] = stime;
                ViewState["etime"] = etime;
                string USERTYPE = RadioButtonList_SelectValue("USERTYPE");
                string uin = AccountService.GetQQID(USERTYPE, this.txt_user.Text.Trim());
                ViewState["uin"] = uin;

                string tradeId = new FundService().GetTradeIdByUIN(uin);
                if (string.IsNullOrEmpty(tradeId))
                {
                    throw new LogicException("查询不到用户的TradeId，请确认当前用户是否有注册基金账户");
                }
//#if DEBUG
//                tradeId = "20151102870920001000";
//#endif
                ViewState["tradeId"] = tradeId;
                Bind();
            }
            catch (LogicException eSys)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(eSys.Message));
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(eSys.ToString()));
            }
        }

        private void Bind()
        {
            int limit = pager1.PageSize;
            int offset = (pager1.CurrentPageIndex - 1) * limit;
            FundService serviec = new FundService();
            string tradeId = ViewState["tradeId"].ToString();
            string stime = ViewState["stime"].ToString();
            string etime = ViewState["etime"].ToString();
            DataTable dt = serviec.GetLCTReserveOrder(tradeId, null, stime, etime, offset, limit);
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();
        }
        protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                panelDetail.Visible = true;
                string listid = e.Item.Cells[2].Text.Trim();//ID 
                string tradeId = ViewState["tradeId"].ToString();
                if (e.CommandName == "detail")
                {
                    FundService serviec = new FundService();
                    DataTable dt = serviec.GetLCTReserveOrder(tradeId, listid, null, null, 0, 1);
                   
                    foreach (object control in this.panelDetail.Controls)
                    {
                        if (control.GetType() == typeof(Label))
                        {
                            Label label = control as Label;
                            label.Text = dt.Rows[0][label.ID.Replace("Label_", "")].ToString();
                        }
                    }
                }
            }
            catch (LogicException eSys)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(eSys.Message));
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(eSys.ToString()));
            }
        }
        public void ChangePage1(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                pager1.CurrentPageIndex = e.NewPageIndex;
                Bind();
            }
            catch (LogicException eSys)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(eSys.Message));
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(eSys.ToString()));
            }
        }
        private string RadioButtonList_SelectValue(string GroupName)
        {
            string value = "";
            foreach (object control in this.formMain.Controls)
            {
                if (control.GetType() == typeof(RadioButton))
                {
                    var radio = control as RadioButton;
                    if (radio.GroupName == GroupName && radio.Checked == true)
                    {
                        value = radio.ID;
                    }
                }
            }
            return value;
        }
        
    }
}