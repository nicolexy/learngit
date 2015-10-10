using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.ForeignCurrencyModule;
using CFT.CSOMS.COMMLIB;

namespace TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay
{
    public partial class FCXGMoneyAndFlow : System.Web.UI.Page
    {
        string operatorID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.lb_operatorID.Text = operatorID = Session["uid"] as string;
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
                {
                    Response.Redirect("../login.aspx?wh=1");
                }
            }
        }

        //查询
        protected void Button1_Click(object sender, EventArgs e)
        {
            #region 清空数据
            DataGrid[] dgs = { dg_refund, dg_trade };
            foreach (var item in dgs)
            {
                item.DataSource = null;
                item.DataBind();
            }
            pager.Visible = false;
            ViewState.Remove("query_uid");
            #endregion
            try
            {
                var input = txt_input.Text.Trim();
                if (input.Length > 0)
                {
                    var query_uid = "";
                    if (checkUId.Checked)
                    {
                        query_uid = input;
                    }
                    else
                    {
                        string uin = input;  //香港钱包 uin
                        if (checkWeChatId.Checked)
                        {
                            uin = WeChatHelper.GetFCXGOpenIdFromWeChatName(input) + "@wx.hkg";
                        }
                        query_uid = new FCXGWallet().QueryUserId(uin);
                    }
                    ViewState["query_uid"] = query_uid;
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "请输入查询条件");
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "查询出错:" + PublicRes.GetErrorMsg(ex.Message));
            }
        }

        //按钮处理程序选择
        protected void SwitchHandler(object sender, EventArgs e)
        {
            try
            {
                dg_refund.DataSource = dg_trade.DataSource = null;
                dg_refund.DataBind();
                dg_trade.DataBind();
                LinkButton btn = sender as LinkButton;
                string type = "";
                if (btn != null)
                {
                    type = btn.ID;
                    ViewState["btn_CurType"] = btn.ID;
                    pager.CurrentPageIndex = 1;
                    pager.RecordCount = 1000;
                }
                else
                {
                    type = (string)ViewState["btn_CurType"];
                }
                LinkButton[] btns = { btn_refund, btn_trade };
                foreach (var item in btns)
                {
                    if (item.ID == type)
                        item.ForeColor = System.Drawing.Color.Red;
                    else
                        item.ForeColor = System.Drawing.Color.Black;
                }

                var skip = pager.CurrentPageIndex * pager.PageSize - pager.PageSize;
                var query_uid = ViewState["query_uid"] as string;
                if (query_uid != null)
                {
                    switch (type)
                    {
                        case "btn_trade": TradeHandler(query_uid, skip, pager.PageSize); break;
                        case "btn_refund": RefundHandler(query_uid, skip, pager.PageSize); break;
                        default: WebUtils.ShowMessage(this.Page, "查询出错:" + type + "不存在"); break;
                    }
                    pager.Visible = true;
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "异常:" + PublicRes.GetErrorMsg(ex.Message));
            }
        }

        //分页
        protected void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            SwitchHandler(null, null);
        }

        //退款单
        protected void RefundHandler(string query_uid, int offset, int limit)
        {
            var bll = new FCXGWallet();
            var dt = bll.QueryRefundInfo(query_uid, offset, limit, Request.UserHostAddress);
            if (dt == null || dt.Rows.Count < 1)
            {
                WebUtils.ShowMessage(this.Page, "未找到记录");
            }
            dg_refund.DataSource = dt;
            dg_refund.DataBind();
        }

        //交易单
        protected void TradeHandler(string query_uid, int offset, int limit)
        {
            var bll = new FCXGWallet();
            var dt = bll.QueryTradeInfo(query_uid, offset, limit, Request.UserHostAddress);
            if (dt == null || dt.Rows.Count < 1)
            {
                WebUtils.ShowMessage(this.Page, "未找到记录");
            }
            dg_trade.DataSource = dt;
            dg_trade.DataBind();
        }
    }
}