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
    public partial class CustomsBatchRedeclare : System.Web.UI.Page
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
                string path = Server.MapPath("~/") + "PLFile" + "\\refund.xls";
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

                //查出商户号个数
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
                        string request_text = "partner=" + drs[0]["F1"].ToString().Trim();
                        request_text += "&customs_company_code=" + drs[0]["F2"].ToString().Trim();
                        request_text += "&customs=" + drs[0]["F3"].ToString().Trim();
                        request_text += "&total_num=" + drs.Count();

                        for (int i = 0; i < drs.Count(); i++)
                        {
                            if (string.IsNullOrEmpty(drs[i]["F4"].ToString().Trim()) && string.IsNullOrEmpty(drs[i]["F5"].ToString().Trim()))
                            {
                                throw new Exception("财付通支付单号和商户订单号不能同时为空");
                            }

                            request_text += "&transaction_id_" + i + "=" + drs[i]["F4"].ToString().Trim();
                            request_text += "&out_trade_no_" + i + "=" + drs[i]["F5"].ToString().Trim();
                            request_text += "&redeclare_reason_" + i + "=" + drs[i]["F6"].ToString().Trim();
                        }

                        //执行重推
                        System.Data.DataTable _dtfailed = new WechatPayService().CustomsRedeclare(request_text);
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
    }
}