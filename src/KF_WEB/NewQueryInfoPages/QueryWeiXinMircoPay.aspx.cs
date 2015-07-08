using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using nBll = CFT.CSOMS.BLL;
using commLib.Entity;
using System.Collections;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
    public partial class QueryWeiXinMircoPay : System.Web.UI.Page
    {
        nBll.InternationalityPayModule.QueryWeiXinMircoPay bll = new nBll.InternationalityPayModule.QueryWeiXinMircoPay();
        Dictionary<string, string> PayState = nBll.InternationalityPayModule.QueryWeiXinMircoPay.PayState;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Label_uid.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this))
                {
                    Response.Redirect("../login.aspx?wh=1");
                }
                else
                {
                    dialog.Visible = false;
                    if (!IsPostBack)
                    {
                        var today = DateTime.Today;
                        txt_fromtime.Text = today.ToString("yyyy年MM月dd日");
                        txt_totime.Text = today.ToString("yyyy年MM月dd日");
                        DropDownListTrade_State.Items.Add(new ListItem("全部", ""));
                        foreach (var item in PayState)
                        {
                            DropDownListTrade_State.Items.Add(new ListItem(item.Value, item.Key));
                        }
                        DropDownListTrade_State.DataBind();
                    }
                    else
                    {
                        this.pager.PageChanged += (s, ee) => Bind(ee.NewPageIndex);
                        dgList.SelectedIndexChanged += dgList_SelectedIndexChanged;
                    }

                }
            }
            catch (Exception)
            {
                Response.Redirect("../login.aspx?wh=1");
            }
        }

        private void dgList_SelectedIndexChanged(object sender, EventArgs e)
        {
            dialog.Visible = true;
            var list = ViewState["FMicroOrderList"] as List<Fmicro_Order_Result>;
            if (list == null)
            {
                WebUtils.ShowMessage(Page, "未知错误! 请刷新后,再次操作");  //ViewState["FMicroOrderList"]  不存在值
            }
            var cur = list[dgList.SelectedIndex];
            iframe_tradelog.Attributes.Add("src", "/TradeManage/TradeLogQuery.aspx?id=" + cur.cftlistid);
            #region 给详细页,标记赋值
            currency_type.InnerText = cur.currency_type;
            trans_rate.InnerText = cur.trans_rate;
            rate_time.InnerText = cur.rate_time;
            input_fc_rmb.InnerText = cur.input_fc_rmb;
            total_fee_fc.InnerText = cur.total_fee_fc;
            total_fee_rmb.InnerText = cur.total_fee_rmb;
            name.InnerText = cur.name;
            mobile.InnerText = cur.mobile;
            buyid.InnerText = cur.buyid;
            cre_num.InnerText = cur.cre_num;
            ErrorReason.InnerText = cur.trade_state == "支付失败" ? "失败原因：" + cur.trade_state_desc : "";
            #endregion
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            pager.RecordCount = 100000;
            Bind(1);
        }
        private void Bind(int index)
        {
            try
            {
                var fTime = DateTime.Parse(txt_fromtime.Text);
                var tTime = DateTime.Parse(txt_totime.Text);
                if (tTime.Month != fTime.Month)  //在同一个月份中 时差15天 开始时间不能小于结束时间
                {
                    WebUtils.ShowMessage(this.Page, "日期错误: 不能跨月查找");
                    return;
                }
                var difference = (tTime - fTime).Days;
                if (difference > 15 || difference < 0)
                {
                    WebUtils.ShowMessage(this.Page, "日期错误: 日期区间必须在15天以内");
                    return;
                }
                if (txt_spid.Text == "" && txt_out_trade_no.Text == "" && txt_listid.Text == "" && txt_cftlistid.Text == "" && txt_mobile.Text == "" && txt_name.Text == "")
                {
                    WebUtils.ShowMessage(this.Page, "最少输入一个查询条件!");
                    return;
                }
                var limit = pager.PageSize;
                pager.CurrentPageIndex = index;
                var trade_state = DropDownListTrade_State.SelectedItem.Value;
                if (trade_state == "2" || trade_state == "5")
                {
                    if (string.IsNullOrEmpty(txt_spid.Text) && string.IsNullOrEmpty(txt_out_trade_no.Text) && string.IsNullOrEmpty(txt_listid.Text))
                    {
                        var str = PayState[trade_state];
                        WebUtils.ShowMessage(this.Page, string.Format("交易状态等于 [{0}] 时,商户号、商户订单号、微信订单号 必须有一个或以上条件有值", str));
                        return;
                    }
                }
                var list = bll.FMicroOrderList(txt_spid.Text, txt_out_trade_no.Text, txt_listid.Text, txt_cftlistid.Text, txt_mobile.Text, txt_name.Text, fTime, tTime, index, limit, trade_state);
                if (list != null && list.Count != 0)
                {
                    dgList.DataSource = list;
                    ViewState["FMicroOrderList"] = list;
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "没有查找到记录!");
                    dgList.DataSource = null;
                }
                dgList.DataBind();
            }
            catch (Exception ex)
            {
                var error = PublicRes.GetErrorMsg(ex.Message);
                WebUtils.ShowMessage(this.Page, error);
            }
        }
    }
}