using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.ForeignCurrencyModule;
using CFT.CSOMS.COMMLIB;
using System.Data;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay
{
    public partial class FCXGMoneyAndFlow : PageBase
    {
        string operatorID;
        FCXGWallet bll = new FCXGWallet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.lb_operatorID.Text = operatorID = Session["uid"] as string;

                DateTime Etime = DateTime.Now;
                DateTime Stime = Etime.AddDays(-31);

                if (Etime.Year - Stime.Year > 0) 
                {
                    Stime = Convert.ToDateTime(Etime.Year + "-01-01");
                }
                this.txtStime.Text = Stime.ToString("yyyy-MM-dd");
                this.txtEtime.Text = Etime.ToString("yyyy-MM-dd");

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
            DataGrid[] dgs = { dg_refund, dg_trade, dg_BankrollList, dg_fetch, dg_GetPackageList, dg_SendPackageList };
            foreach (var item in dgs)
            {
                item.DataSource = null;
                item.DataBind();
            }
            pager.Visible = false;
            ViewState.Remove("query_uid");
            ViewState.Remove("query_uin");
            ViewState["client_ip"] = Request.UserHostAddress == "::1" ? "127.0.0.1" : Request.UserHostAddress;

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
                            uin = WeChatHelper.GetFCXGOpenIdFromWeChatName(input, ViewState["client_ip"].ToString()) + "@wx.hkg";
                        }
                         ViewState["query_uin"] = uin;
                        query_uid = new FCXGWallet().QueryUserId(uin);
                    }
                    ViewState["query_uid"] = query_uid;

                    DateTime dts=Convert.ToDateTime(txtStime.Text);
                    DateTime dte = Convert.ToDateTime(txtEtime.Text);
                    TimeSpan d3 = dte.Subtract(dts);
                    if (d3.Days < 0) 
                    {
                        throw new Exception("结束时间必须大于开始时间！");
                    }
                    if (d3.Days > 31)
                    {
                        throw new Exception("时间间隔不能超过31天！");
                    }
                    if (dte.Year - dts.Year != 0)
                    {
                        throw new Exception("不能跨年查询！");
                    }

                    ViewState["stime"] = dts.ToString("yyyy-MM-dd");
                    ViewState["etime"] = dte.ToString("yyyy-MM-dd");
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "请输入查询条件");
                }
            }
            catch (Exception ex)
            {
                LogError("ForeignCurrencyPay.FCXGMoneyAndFlow", "protected void Button1_Click(object sender, EventArgs e)", ex);
                WebUtils.ShowMessage(this.Page, "查询出错:" + PublicRes.GetErrorMsg(ex.Message));
            }
        }

        //按钮处理程序选择
        protected void SwitchHandler(object sender, EventArgs e)
        {
            try
            {
                DataGrid[] dgs = { dg_refund, dg_trade, dg_BankrollList, dg_fetch,dg_GetPackageList,dg_SendPackageList };
                foreach (var item in dgs)
                {
                    item.DataSource = null;
                    item.DataBind();
                }

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
                LinkButton[] btns = { btn_refund, btn_trade, btn_BankrollList, btn_Fetch, btn_getPackage, btn_SendPackage };
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
                        case "btn_BankrollList": BankrollListHandler(query_uid,skip, pager.PageSize); break;
                        case "btn_Fetch": FetchHandler(query_uid, skip, pager.PageSize); ; break;
                        case "btn_getPackage": GetPackageList(query_uid, skip, pager.PageSize); ; break;
                        case "btn_SendPackage": SendPackageList(query_uid, skip, pager.PageSize); ; break;

                            
                        default: WebUtils.ShowMessage(this.Page, "查询出错:" + type + "不存在"); break;
                    }
                    pager.Visible = true;
                }
            }
            catch (Exception ex)
            {
                LogError("ForeignCurrencyPay.FCXGMoneyAndFlow", "protected void SwitchHandler(object sender, EventArgs e)", ex);
                WebUtils.ShowMessage(this.Page, "异常:" + PublicRes.GetErrorMsg(ex.Message));
            }
        }
        #region 收/发红包查询

       /// <summary>
       /// 收红包
       /// </summary>
       /// <param name="query_uid"></param>
       /// <param name="skip"></param>
       /// <param name="p"></param>
        private void GetPackageList(string query_uid, int skip, int p)
        {
            var bll = new FCXGWallet();
            var packageData = bll.QueryReceivePackageList(query_uid, ViewState["stime"].ToString(), ViewState["etime"].ToString(), skip, p, ViewState["client_ip"].ToString());
            if (packageData == null || packageData.Count < 1)
            {
                WebUtils.ShowMessage(this.Page, "未找到记录");
            }
            dg_GetPackageList.DataSource = packageData;
            dg_GetPackageList.DataBind();
        }

        /// <summary>
        /// 发红包
        /// </summary>
        /// <param name="query_uid"></param>
        /// <param name="skip"></param>
        /// <param name="p"></param>
        private void SendPackageList(string query_uid, int skip, int p)
        {
            var bll = new FCXGWallet();
            var packageData = bll.QuerySendPackageList(query_uid, ViewState["stime"].ToString(), ViewState["etime"].ToString(), skip, p, ViewState["client_ip"].ToString());
            if (packageData == null || packageData.Count < 1)
            {
                WebUtils.ShowMessage(this.Page, "未找到记录");
            }
            dg_SendPackageList.DataSource = packageData;
            dg_SendPackageList.DataBind();
        }

        #endregion

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
            var dt = bll.QueryRefundInfo(query_uid, ViewState["stime"].ToString(), ViewState["etime"].ToString(), offset, limit, ViewState["client_ip"].ToString());
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
            var dt = bll.QueryTradeInfo(query_uid, ViewState["stime"].ToString(), ViewState["etime"].ToString(), offset, limit, ViewState["client_ip"].ToString());
            if (dt == null || dt.Rows.Count < 1)
            {
                WebUtils.ShowMessage(this.Page, "未找到记录");
            }

            //if (!dt.Columns.Contains("card_type"))  //  接口未发布修正版的时候  保证不报错,  接口修正后 删除
            //{
            //    dt.Columns.Add("card_type");
            //}
            dg_trade.DataSource = dt;
            dg_trade.DataBind();
        }
        
        protected void FetchHandler(string query_uid, int offset, int limit)
        {
           

            var dt = bll.QueryFetchInfo(query_uid, ViewState["stime"].ToString(), ViewState["etime"].ToString(), offset, limit, ViewState["client_ip"].ToString()); 
            if (dt == null || dt.Rows.Count < 1)
            {
                WebUtils.ShowMessage(this.Page, "未找到记录");
            }

            //if (!dt.Columns.Contains("card_type"))  //  接口未发布修正版的时候  保证不报错,  接口修正后 删除
            //{
            //    dt.Columns.Add("card_type");
            //}

            dg_fetch.DataSource = dt;
            dg_fetch.DataBind();
        }

        protected void BankrollListHandler(string uin, int offset, int limit)
        {
            var bll = new FCXGWallet();

            var dt = bll.QueryBankrollList(uin, ViewState["stime"].ToString(), ViewState["etime"].ToString(), offset, limit, ViewState["client_ip"].ToString());
            if (dt == null || dt.Rows.Count < 1)
            {
                WebUtils.ShowMessage(this.Page, "未找到记录");
            }

            //if (!dt.Columns.Contains("card_type"))  //  接口未发布修正版的时候  保证不报错,  接口修正后 删除
            //{
            //    dt.Columns.Add("card_type");
            //}

            dg_BankrollList.DataSource = dt;
            dg_BankrollList.DataBind();
        }

        /// <summary>
        /// 获取红包种类
        /// </summary>
        /// <param name="typestr"></param>
        /// <returns></returns>
        public string HKWalletPackageType(string typestr) {

            var retTypeValue = string.Empty;
            //1拼手气，2固定值红包
            if (typestr == "1") {
                return "拼手气红包";
            }
            else if (typestr == "2")
            {
                return "固定值红包";
            }
                return retTypeValue;
        }

        /// <summary>
        /// 将日期转为年月日格式
        /// </summary>
        /// <param name="strObj"></param>
        /// <returns></returns>
        public string GetDateString(object strObj) {
            if (strObj != null)
            {
                DateTime tempdate = DateTime.Now;
                if (DateTime.TryParse(strObj.ToString(), out tempdate))
                {
                    return tempdate.ToString("yyyy-MM-dd");
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取支付方式
        /// </summary>
        /// <param name="objPayMean"></param>
        /// <returns></returns>
        public string HKWalletGetPayMeansType(object objPayMean)
        {
            var retPayMean = string.Empty;

            if (objPayMean != null)
            {
                var payMean = objPayMean.ToString();//1.银行卡,2.余额
                switch (payMean)
                {
                    case "1":
                        retPayMean = "银行卡";
                        break;
                    case "2":
                        retPayMean = "余额";
                        break;
                }
            }

            return retPayMean;
        }


        /// <summary>
        /// 分 to 元
        /// </summary>
        /// <param name="objMongey"></param>
        /// <returns></returns>
        public string ConvertFenToYuan(object objMongey)
        {
            var retMongey = string.Empty;

            if (objMongey != null)
            {
                var mongey = objMongey.ToString();//1.银行卡,2.余额
                if (!string.IsNullOrEmpty(mongey)) {
                    retMongey = MoneyTransfer.FenToYuan(mongey);
                }
            }

            return retMongey;
        }

    }
}