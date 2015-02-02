using System;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.InternetBank
{
    public partial class BillHistoryQuery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_QQQuery_Click(object sender, EventArgs e)
        {
            var qq = txtQQ.Text.Trim();
            if (string.IsNullOrEmpty(qq))
            {
                ShowMsg("请输入QQ号码！");
                return;
            }
            var yearMonth = txtYearMonth.Text.Trim();
            if (string.IsNullOrEmpty(yearMonth))
            {
                ShowMsg("请输入年月");
                return;
            }
            if (yearMonth.Length != 6)
            {
                ShowMsg("请输入年月格式为YYYYMM");
                return;
            }
            int yearMonthNumber;
            if (!int.TryParse(yearMonth, out yearMonthNumber))
            {
                ShowMsg("请输入数字");
                return;
            }
            int year = yearMonthNumber / 100;
            if (year < 2012 || year > DateTime.Now.Year)
            {
                ShowMsg("此处提供2012年1月到上月的账单查询");
                return;
            }
            int month = yearMonthNumber - year * 100;
            if (month < 1 || month > 12 || (year == DateTime.Now.Year && month >= DateTime.Now.Month))
            {
                ShowMsg("请输入正确月份");
                return;
            }
            BindDate(qq, "", yearMonth);
        }

        protected void btn_OrderQuery_Click(object sender, EventArgs e)
        {
            var CFTOrder = txtOrder.Text.Trim();
            if (string.IsNullOrEmpty(CFTOrder) || CFTOrder.Length != 28)
            {
                ShowMsg("请输入28位财付通订单号");
                return;
            }
            var yearMonth = CFTOrder.Substring(10, 6);
            var order = CFTOrder.Substring(18, 10);
            int yearMonthNumber;
            if (!int.TryParse(yearMonth, out yearMonthNumber))
            {
                ShowMsg("财付通订单号格式不正确");
                return;
            }
            int year = yearMonthNumber / 100;
            if (year < 2012 || year > DateTime.Now.Year)
            {
                ShowMsg("此处提供2012年1月到上月的账单查询");
                return;
            }
            int month = yearMonthNumber - year * 100;
            if (month < 1 || month > 12 || (year == DateTime.Now.Year && month >= DateTime.Now.Month))
            {
                ShowMsg("此处提供2012年1月到上月的账单查询");
                return;
            }
            
            try 
            {
                BindDate("", order, yearMonth);
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, "查询出错:" + err.Message.ToString());
            }
        }

        void BindDate(string qq, string order, string yearMonth)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            var ds = qs.InternetBankBillHistoryQuery(qq, order, yearMonth);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                throw new Exception("查询记录为空。");
            }
            else
            {
                ds.Tables[0].Columns.Add("state", typeof(string));
                ds.Tables[0].Columns.Add("billState", typeof(string));
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var state = row["Fstate"].ToString();
                    if (InternetBankDictionary.TradeState.ContainsKey(state))
                    {
                        row["state"] = InternetBankDictionary.TradeState[state];
                    }
                    else
                    {
                        row["state"] = state;
                    }
                    var payChannel = row["Fpay_channel"].ToString();
                    if (InternetBankDictionary.PayChannel.ContainsKey(payChannel))
                    {
                        row["Fpay_channel"] = InternetBankDictionary.PayChannel[payChannel];
                    }
                    var serviceCode = row["Fservice_code"].ToString();
                    if (InternetBankDictionary.ServiceCode.ContainsKey(serviceCode))
                    {
                        row["Fservice_code"] = InternetBankDictionary.ServiceCode[serviceCode];
                    }
                    var billState = row["Fbillstate"].ToString();
                    if (InternetBankDictionary.BillState.ContainsKey(billState))
                    {
                        row["billState"] = InternetBankDictionary.BillState[billState];
                    }
                    else
                    {
                        row["billState"] = billState;
                    }

                }
                this.DataGrid_QueryResult.DataSource = ds;
                this.DataGrid_QueryResult.DataBind();
            }
        }

        private void ShowMsg(string msg)
        {
            Response.Write("<script language=javascript>alert('" + msg + "')</script>");
        }
    }
}