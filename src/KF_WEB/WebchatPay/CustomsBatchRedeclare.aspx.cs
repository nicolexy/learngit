using System;
using System.Collections;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.Common;
using System.IO;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using System.Web.UI.WebControls;
using System.Text;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using System.Configuration;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using System.Reflection;
using System.Threading;
using SunLibrary;
using System.Collections.Generic;
using System.Linq;
using CFT.CSOMS.BLL.WechatPay;
using System.Web;

namespace TENCENT.OSS.CFT.KF.KF_Web.WebchatPay
{
    public partial class CustomsBatchRedeclare : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string uid = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                if (!IsPostBack)
                {
                    lblmessage.Visible = false;
                }
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                //Upload();
                Thread thread = new Thread(Upload);
                thread.Start();
                lblmessage.Visible = true;
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.ToString()));
            }
        }

        private void Upload()
        {
            string uid = Session["uid"].ToString();
            try
            {
                string path =  Server.MapPath("~/") + "PLFile" + "\\refund.xls";
                if (fileUpload.FileName.EndsWith(".xlsx"))
                {
                    path = Server.MapPath("~/") + "PLFile" + "\\refund.xlsx";
                }
                fileUpload.PostedFile.SaveAs(path);
                DataSet res_ds = PublicRes.readXls(path, "F1,F2,F3,F4,F5,F6");
                System.Data.DataTable dt = res_ds.Tables[0];

                //记录失败订单
                System.Data.DataTable dt_failed = new System.Data.DataTable();
                dt_failed.Columns.Add("商户号", typeof(System.String));
                dt_failed.Columns.Add("商户海关备案号", typeof(System.String));
                dt_failed.Columns.Add("海关编号", typeof(System.String));
                dt_failed.Columns.Add("重推总数", typeof(System.String));
                dt_failed.Columns.Add("失败数量", typeof(System.String));
                dt_failed.Columns.Add("失败的财付通支付单号", typeof(System.String));
                dt_failed.Columns.Add("失败的商户订单号", typeof(System.String));
                dt_failed.Columns.Add("失败原因", typeof(System.String));

                //每次重推格式
                int percount = 10;
                //查出所有商户号
                List<string> partner_list = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    string partner = dr["F1"].ToString().Trim();
                    if (!partner_list.Any(p => p == partner))
                    {
                        partner_list.Add(partner);
                    }
                }
              
                foreach (string partner in partner_list)
                {
                    DataRow[] drs = dt.Select(" F1='" + partner + "'");
                    try
                    {
                        string reqfather = "partner=" + drs[0]["F1"].ToString().Trim();
                        if (!string.IsNullOrEmpty(drs[0]["F2"].ToString().Trim()))
                        {
                            reqfather += "&customs_company_code=" + drs[0]["F2"].ToString().Trim();
                        }
                        reqfather += "&customs=" + drs[0]["F3"].ToString().Trim();
                        //reqfather += "&total_num=" + drs.Count();

                        string reqStr = reqfather;
                        int index = 0;
                        for (int i = 0; i < drs.Count(); i++)
                        {
                            if (string.IsNullOrEmpty(drs[i]["F4"].ToString().Trim()) && string.IsNullOrEmpty(drs[i]["F5"].ToString().Trim()))
                            {
                                throw new Exception("财付通支付单号和商户订单号不能同时为空");
                            }

                            reqStr += "&transaction_id_" + index + "=" + drs[i]["F4"].ToString().Trim();
                            reqStr += "&out_trade_no_" + index + "=" + drs[i]["F5"].ToString().Trim();
                            reqStr += "&redeclare_reason_" + index + "=" + drs[i]["F6"].ToString().Trim();
                            //每10条执行一次重推，最后一条执行一次
                            if ((i+1) % percount == 0 || (i+1) == drs.Count())
                            {
                                int count = index+1;
                                reqStr += "&total_num=" + count;
                                //执行重推
                                System.Data.DataTable _dtfailed = new WechatPayService().CustomsRedeclare(reqStr);
                                //回复初始
                                reqStr = reqfather;
                                index = 0;
                                //记录失败的单
                                if (_dtfailed != null && _dtfailed.Rows.Count > 0)
                                {
                                    foreach (DataRow item in _dtfailed.Rows)
                                    {
                                        DataRow dr_faild = dt_failed.NewRow();
                                        dr_faild["商户号"] = drs[0]["F1"].ToString();
                                        dr_faild["商户海关备案号"] = drs[0]["F2"].ToString();
                                        dr_faild["海关编号"] = drs[0]["F3"].ToString();
                                        dr_faild["重推总数"] = drs.Count();
                                        dr_faild["失败数量"] = _dtfailed.Rows.Count;
                                        dr_faild["失败的财付通支付单号"] = item["transaction_id"].ToString();
                                        dr_faild["失败的商户订单号"] = item["out_trade_no"].ToString();
                                        dr_faild["失败原因"] = item["fail_info"].ToString();
                                        dt_failed.Rows.Add(dr_faild);
                                    }
                                }
                                continue;
                            }
                            index++;
                        }                     
                    }
                    catch (Exception error)
                    {
                        for (int i = 0; i < drs.Count(); i++)
                        {
                            //记录失败的单
                            DataRow dr_faild = dt_failed.NewRow();
                            dr_faild["商户号"] = drs[0]["F1"].ToString();
                            dr_faild["商户海关备案号"] = drs[0]["F2"].ToString();
                            dr_faild["海关编号"] = drs[0]["F3"].ToString();
                            dr_faild["重推总数"] = drs.Count();
                            dr_faild["失败数量"] = drs.Count();
                            dr_faild["失败的财付通支付单号"] = drs[i]["F4"].ToString();
                            dr_faild["失败的商户订单号"] = drs[i]["F5"].ToString();
                            dr_faild["失败原因"] = error.Message;

                            dt_failed.Rows.Add(dr_faild);
                        }
                    }

                }

                try
                {
//#if DEBUG
//                    uid = "v_yqyqguo";
//#endif
                    //重推总数
                    int total = dt != null ? dt.Rows.Count : 0;
                    //失败数量
                    int total_faild = dt_failed != null ? dt_failed.Rows.Count : 0;
                    path = Server.MapPath("~/") + "PLFile" + "\\" + uid + ".xls"; //附件
                    PublicRes.Export(dt_failed, path);
                    string[] fileAtta = { path };
                    //mail
                    CommMailSend.SendInternalMail(uid, "", "批量海关重推", "重推总数:" + total + ";失败单数:" + total_faild, false, fileAtta);

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            catch (Exception e)
            {
                LogHelper.LogInfo("批量海关重推失败!" + e.ToString());
                CommMailSend.SendInternalMail(uid, "", "批量海关重推失败", e.ToString(), false);
            }

        }

        protected void btn_one_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txt_partner.Text.Trim()))
                {
                    throw new Exception("商户号不能为空！");
                }
                if (string.IsNullOrEmpty(txt_customs.Text.Trim()))
                {
                    throw new Exception("海关编号不能为空！");
                }
                if (string.IsNullOrEmpty(txt_transaction_id.Text.Trim()) && string.IsNullOrEmpty(txt_out_trade_no.Text.Trim()))
                {
                    throw new Exception("财付通支付单号和商户订单号不能同时为空");
                }
              

                string request_text = "partner=" + txt_partner.Text.Trim();
                request_text += "&customs=" + txt_customs.Text.Trim();
                request_text += "&total_num=1";
                if (!string.IsNullOrEmpty(txt_customs_company_code.Text.Trim()))
                {
                    request_text += "&customs_company_code=" + txt_customs_company_code.Text.Trim();
                }

                if (!string.IsNullOrEmpty(txt_transaction_id.Text.Trim()))
                {
                    request_text += "&transaction_id_0=" + txt_transaction_id.Text.Trim();
                }

                if (!string.IsNullOrEmpty(txt_out_trade_no.Text.Trim()))
                {
                    request_text += "&out_trade_no_0=" + txt_out_trade_no.Text.Trim();
                }
              
                request_text += "&redeclare_reason_0=" + txt_redeclare_reason.Text.Trim();
                System.Data.DataTable dtfailed = new WechatPayService().CustomsRedeclare(request_text);
                if (dtfailed != null && dtfailed.Rows.Count > 0)
                {
                    lblmessageOne.Text = "重推失败，失败原因：" + dtfailed.Rows[0]["fail_info"].ToString();
                }
                else 
                {
                    lblmessageOne.Text = "重推成功";
                }
            }
            catch (Exception ex)
            {
                lblmessageOne.Text = "重推失败，失败原因：" + HttpUtility.JavaScriptStringEncode(ex.Message);
            }
        }
    }
}