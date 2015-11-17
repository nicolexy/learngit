﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Tencent.DotNet.Common.UI;
using System.Web.Services.Protocols;

namespace TENCENT.OSS.CFT.KF.KF_Web.InternetBank
{
    public partial class TurnoverQuery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime d1 = Convert.ToDateTime(DateTime.Today.AddMonths(-1).ToString("yyyy年MM月01日"));
                //DateTime now = DateTime.Now;
                //DateTime d1 = new DateTime(now.Year, now.Month, 1);
                this.tbx_beginDate.Text = d1.ToString("yyyy年MM月dd日");
                this.tbx_endDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
            }
            this.btnBeginDate.Attributes.Add("onclick", "openModeBegin()");
            this.btnEndDate.Attributes.Add("onclick", "openModeEnd()");
        }

        protected void btn_query_Click(object sender, EventArgs e)
        {
            DateTime beginDate;
            DateTime endDate;
            if (DateTime.TryParse(this.tbx_beginDate.Text.Trim(), out beginDate) && DateTime.TryParse(this.tbx_endDate.Text.Trim(), out endDate))
            {
               
            }
            else
            {
                ShowMsg("日期格式不正确！");
                return;
            }
            if (this.tbx_beginDate.Text.Trim() != "" && this.tbx_endDate.Text.Trim() != "")
            {
                beginDate = DateTime.Parse(this.tbx_beginDate.Text);//.ToString("yyyy-MM-dd");
                endDate = DateTime.Parse(this.tbx_endDate.Text);//.ToString("yyyy-MM-dd");

            }
            var qq = txtQQ.Text.Trim();
            var order = txtOrder.Text.Trim();
            //var turnoverID = txtBankTrunoverID.Text.Trim();
            string payAccount = txtPayAccount.Text.Trim();
            if (string.IsNullOrEmpty(qq) && string.IsNullOrEmpty(order)  && string.IsNullOrEmpty(payAccount))
            {
                ShowMsg("请输入查询条件"); return;
            }
            try
            {
                startQuery(beginDate, endDate, qq, order, "", payAccount);
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

        void startQuery(DateTime beginDate, DateTime endDate, string qq, string order, string turnoverID, string payAccount)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds = qs.InternetBankTurnoverBillQuery(qq, order, turnoverID,payAccount, beginDate, endDate, false);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                throw new Exception("查询记录为空。");
            }
            else
            {
                //20130923 lxl 去掉流水状态
              //  ds.Tables[0].Columns.Add("state", typeof(string));
                foreach (DataRow row in ds.Tables[0].Rows)
                {                  
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