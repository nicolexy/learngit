using CFT.CSOMS.BLL.CFTAccountModule;
using BLL = CFT.CSOMS.BLL.ForeignCurrencyModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;

namespace TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay
{
    public partial class FCUserTradeQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        BLL.FCUserTradeQuery bll = new BLL.FCUserTradeQuery();
        protected void Page_Load(object sender, EventArgs e)
        {
            pager.RecordCount = 1000;
            var uid = Session["uid"] as string;
            if (!IsPostBack)
            {
                if (!ClassLib.ValidateRight("InfoCenter", this))
                    Response.Redirect("../login.aspx?wh=1");
                lb_operatorID.Text = uid;
            }
            HideDataGrids();
        }
        //搜索
        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                wrap_info.Visible = false;
                var weChatId = WeChatId.Text.Trim();
                if (weChatId == "")
                {
                    WebUtils.ShowMessage(this.Page, "WeChat Id 不可以为空");
                    return;
                }
                var uin = AccountService.GetQQID("WeChatId", weChatId); //"085e9858ed8ed3aa9a95e4252@wx.tenpay.com";    // "test5@md.tenpay.com";  //085e9858e4e80d2ce148400b6@wx.tenpay.com
                ViewState["uin"] = uin;
                wrap_info.Visible = true;
            }
            catch (Exception ex)
            {
                var error = PublicRes.GetErrorMsg(ex.Message);
                WebUtils.ShowMessage(this.Page, error);
            }
        }

        //生成CVS
        protected void btn_createCVS_Click(object sender, EventArgs e)
        {
            try
            {
                string btnName = ViewState["btnName"] as string;
                string uin = ViewState["uin"] as string;
                string fileName = null;
                if (string.IsNullOrEmpty(uin))
                {
                    WebUtils.ShowMessage(this.Page, "请先查询 WeChatId"); return;
                }
                if (string.IsNullOrEmpty(btnName))
                {
                    WebUtils.ShowMessage(this.Page, "请先选择分类"); return;
                }
                #region 数据获取

                string[][] columns = null;
                DataSet ds = null;
                switch (btnName)
                {
                    case "btn_tradeBill":
                        { ds = bll.QueryFCTradeBillsAndRefundAll(uin, 101); columns = TradeColumns; fileName = "交易单"; break; }
                    case "btn_refund":
                        { ds = bll.QueryFCTradeBillsAndRefundAll(uin, 102); columns = TradeColumns; fileName = "退款单"; break; }
                    case "btn_bindCardRecord":
                        { ds = bll.QueryFCBindCardRecord(uin); columns = GetDataGridColumns(dg_bindCardRecord); fileName = "绑卡记录"; break; }
                    case "btn_fundStream":
                        { ds = bll.QueryFCFlowAll(uin, "344", "2"); columns = GetDataGridColumns(dg_BankRollFlow); fileName = "资金流水"; break; }
                }
                if (ds == null || ds.Tables.Count == 0)
                {
                    WebUtils.ShowMessage(this.Page, "没有找到数据"); return;
                }

                #endregion

                #region 解析成CVS

                var dt = ds.Tables[0];
                var buff = new System.Text.StringBuilder(8 * columns.Length * dt.Rows.Count);
                buff.AppendLine(string.Join(",", columns.Select(u => u[1]).ToArray()));
                foreach (DataRow row in dt.Rows)
                {
                    string[] values = new string[columns.Length];
                    for (int i = 0; i < columns.Length; i++)
                    {
                        values[i] = "\t" + row[columns[i][0]] as string; //加入 \t 防止excel 打开时把订单号当成数值
                    }
                    buff.AppendLine(string.Join(",", values));
                }
                #endregion

                #region 输出文件

                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".csv");
                Response.ContentType = "application/ms-excel";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                Response.Write(buff.ToString());
                Response.End();

                #endregion

            }
            catch (Exception ex)
            {
                var error = PublicRes.GetErrorMsg(ex.Message);
                WebUtils.ShowMessage(this.Page, error);
            }
        }

        //tab标签页按钮公用事件
        protected void btn_Click(object sender, EventArgs e)
        {
            var cur = sender as LinkButton;
            BtnSwitch(cur.ID, 1);
            ViewState["btnName"] = cur.ID;
        }

        //资金流水
        protected void a_fundStream_Click(object sender, EventArgs e)
        {
            var uin = ViewState["uin"] as string;
            if (!string.IsNullOrEmpty(uin))
            {
                ViewState["btnName"] = "btn_fundStream";
                fundStreamTermInfo.Visible = true;
                return;
            }
            WebUtils.ShowMessage(this.Page, "请先查询 WeChatId"); return;
        }

        //分页 页码改变事件
        protected void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            var btnName = ViewState["btnName"] as string;
            if (!string.IsNullOrEmpty(btnName))
            {
                BtnSwitch(btnName, e.NewPageIndex);
            }
        }

        /// <summary>
        /// 选择调用BLL方法
        /// </summary>
        /// <param name="btnName">操作名称</param>
        /// <param name="index">页码</param>
        private void BtnSwitch(string btnName, int index)
        {
            DataSet ds = null;
            DataGrid dg = null;
            var uin = ViewState["uin"] as string;
            fundStreamTermInfo.Visible = btnName == "btn_fundStream";
            try
            {
                if (uin != null)
                {
                    pager.CurrentPageIndex = index;
                    var skip = index * 10 - 10;
                    switch (btnName)
                    {
                        case "btn_tradeBill":
                            ds = bll.QueryFCTradeBills(uin, 10, skip); dg = dg_tradeBill; break;
                        case "btn_refund":
                            ds = bll.QueryFCRefundBills(uin, 10, skip); dg = dg_refundBill; break;
                        case "btn_bindCardRecord":
                            ds = bll.QueryFCBindCardRecord(uin); dg = dg_bindCardRecord; break;
                        case "btn_fundStream":
                            ds = bll.QueryFCFlow(uin, FCurType.Value, FAccType.Value, 10, skip); dg = dg_BankRollFlow; break;
                    }
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dg.DataSource = ds.Tables[0];
                        dg.DataBind();
                        dg.Visible = true;
                        return;
                    }
                    else
                    {
                        dg.DataSource = null;
                        WebUtils.ShowMessage(this.Page, "没有找到记录");
                    }
                }
                else
                {
                    dg.DataSource = null;
                    WebUtils.ShowMessage(this.Page, "请先查询 WeChatId");
                }
            }
            catch (Exception ex)
            {
                var error = PublicRes.GetErrorMsg(ex.Message);
                WebUtils.ShowMessage(this.Page, error);
            }
        }

        #region 页面方法

        // 隐藏DataGrid控件
        private void HideDataGrids()
        {
            foreach (System.Web.UI.Control item in wrap_info.Controls)
            {
                if (item is DataGrid)
                {
                    item.Visible = false;
                }
            }
        }

        //从页面DataGrid对象中获取字典
        private string[][] GetDataGridColumns(DataGrid dg)
        {
            var arr = new string[dg.Columns.Count][];
            for (int i = 0; i < dg.Columns.Count; i++)
            {
                BoundColumn item = (BoundColumn)dg.Columns[i];
                arr[i] = new string[] { item.DataField, item.HeaderText };
            }
            return arr;
        }


        //交易单退款单 生成CVS对应字段
        private static readonly string[][] TradeColumns = new string[][]
            {
                 new string[]{"acc_time","交易时间"},
                 new string[]{"listid","财付通订单号/MD订单号"},
                 new string[]{"coding","关联单号"},
                 //new string[]{"list_type","交易单/退款单"},
                 new string[]{"sp_name","商户名称"},
                 new string[]{"total_fee_str","交易金额"},
                 new string[]{"cur_type_str","币种"},
                 new string[]{"memo","产品备注"},
                 new string[]{"delete_flag_str","标示"},
                 new string[]{"list_state_str","交易单状态"},
            };
        #endregion
    }
}