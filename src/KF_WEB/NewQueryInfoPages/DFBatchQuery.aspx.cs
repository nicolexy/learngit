using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using Tencent.DotNet.Common.UI;
using System.IO;
using System.Web.Services.Protocols;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
    public partial class DFBatchQuery1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            if (!IsPostBack)
            {
                this.tbx_beginDate.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss");
                this.tbx_endDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                this.ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()");
                this.ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");

                this.pager.PageSize = 10;
                this.pager.RecordCount = GetCount();
            }

            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);
        }

        protected void btn_serach_Click(object sender, EventArgs e)
        {
            BindData(1);

            this.pager.CurrentPageIndex = 1;
        }


        private int GetCount()
        {
            return 1000;
        }


        private void BindData(int pageIndex)
        {
            DateTime sTime, eTime;
            string strSTime = "", strETime = "";
            try
            {
                try
                {
                    sTime = DateTime.Parse(this.tbx_beginDate.Text.Trim());
                    eTime = DateTime.Parse(this.tbx_endDate.Text.Trim());

                    strSTime = sTime.ToString("yyyy-MM-dd 00:00:00");
                    strETime = eTime.ToString("yyyy-MM-dd 23:59:59");

                    if (sTime.AddDays(10) <= eTime)
                    {
                        WebUtils.ShowMessage(this, "日期跨度不能大于10天");
                        return;
                    }

                }
                catch
                {
                    WebUtils.ShowMessage(this, "日期格式不正确");
                    return;
                }

                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                //qs.Finance_HeaderValue = setConfig.setFH(Session["OperID"].ToString(),Request.UserHostAddress);

                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

                string state = ddl_state.SelectedValue;

                DataSet ds = qs.QueryBatchInfo_DF(strSTime, strETime, this.tbx_spid.Text, this.tbx_spBatchID.Text, state,
                    (pageIndex - 1) * this.pager.PageSize + 1, this.pager.PageSize);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    this.btn_outExcel.Visible = false;
                    WebUtils.ShowMessage(this, "查询结果为空");
                    return;
                }

                this.btn_outExcel.Visible = true;

                ds.Tables[0].Columns.Add("spidUrl", typeof(string));
                ds.Tables[0].Columns.Add("totalBatchUrl", typeof(string));
                ds.Tables[0].Columns.Add("successBatchUrl", typeof(string));
                ds.Tables[0].Columns.Add("failedBatchUrl", typeof(string));
                ds.Tables[0].Columns.Add("handlingBatchUrl", typeof(string));

                ds.Tables[0].Columns.Add("Ftotal_paynumName", typeof(string));
                ds.Tables[0].Columns.Add("Fsucpay_amountName", typeof(string));
                ds.Tables[0].Columns.Add("Ffailpay_amountName", typeof(string));
                ds.Tables[0].Columns.Add("FHandling_amountName", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["Ftotal_paynumName"] = setConfig.FenToYuan(dr["Ffact_fee"].ToString());
                    dr["Fsucpay_amountName"] = setConfig.FenToYuan(dr["Fsucpay_fee"].ToString());
                    dr["Ffailpay_amountName"] = setConfig.FenToYuan(dr["Ffailpay_fee"].ToString());
                    dr["FHandling_amountName"] = setConfig.FenToYuan(dr["FHandling_fee"].ToString());

                    dr["spidUrl"] = "";// "../BaseAccount/PayBusinessQuery.aspx?spid=" + dr["Fspid"].ToString();

                    // 这里要取的到底是商户批次号还是财付通批次号？
                    dr["totalBatchUrl"] = "./DFDetailQuery.aspx?spid=" + dr["Fspid"].ToString()
                        + "&batchid=" + dr["Fubatch_id"].ToString() + "&sDate=" + dr["Fcreate_time"] + "&eDate=" + dr["Fmodify_time"];

                    dr["successBatchUrl"] = "./DFDetailQuery.aspx?spid=" + dr["Fspid"].ToString()
                        + "&batchid=" + dr["Fubatch_id"].ToString() + "&state=s" + "&sDate=" + dr["Fcreate_time"] + "&eDate=" + dr["Fmodify_time"];

                    dr["failedBatchUrl"] = "./DFDetailQuery.aspx?spid=" + dr["Fspid"].ToString()
                        + "&batchid=" + dr["Fubatch_id"].ToString() + "&state=f" + "&sDate=" + dr["Fcreate_time"] + "&eDate=" + dr["Fmodify_time"];

                    dr["handlingBatchUrl"] = "./DFDetailQuery.aspx?spid=" + dr["Fspid"].ToString()
                        + "&batchid=" + dr["Fubatch_id"].ToString() + "&state=h" + "&sDate=" + dr["Fcreate_time"] + "&eDate=" + dr["Fmodify_time"];
                }

                //DataSet ds2 = qs.CountBatchInfo_DF(strSTime, strETime, this.tbx_spid.Text, this.tbx_spBatchID.Text, ddl_state.SelectedValue);

                //if (ds2 != null && ds2.Tables.Count != 0 && ds2.Tables[0].Rows.Count != 0)
                //{
                //    DataRow dr2 = ds2.Tables[0].Rows[0];

                //    this.pager.RecordCount = int.Parse(dr2[0].ToString());

                //    this.lb_failAllMoney.Text = setConfig.FenToYuan(dr2[4].ToString());
                //    this.lb_failNum.Text = dr2[3].ToString();

                //    this.lb_successAllMoney.Text = setConfig.FenToYuan(dr2[2].ToString());
                //    this.lb_successNum.Text = dr2[1].ToString();

                //    this.lb_handlingMoney.Text = setConfig.FenToYuan(dr2[6].ToString());
                //    this.lb_handlingNum.Text = dr2[5].ToString(); ;
                //}
                //else
                //{
                //    WebUtils.ShowMessage(this, "统计失败");
                //    //return;
                //}

                double sTotalMoney = 0, fTotalMoney = 0, hTotalMoney = 0;
                long sTotalNums = 0, fTotalNums = 0, hTotalNums = 0;
                //需求查询出全部结果
                DataSet dsStatistics= qs.QueryBatchInfo_DF(strSTime, strETime, this.tbx_spid.Text, this.tbx_spBatchID.Text, state,
                    -1, -1);
                if (dsStatistics != null && dsStatistics.Tables.Count != 0 && dsStatistics.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow dr in dsStatistics.Tables[0].Rows)
                    {
                        sTotalNums += long.Parse(dr["Fsucpay_num"].ToString());
                        fTotalNums += long.Parse(dr["Ffailpay_num"].ToString());
                        hTotalNums += long.Parse(dr["FHandling_num"].ToString());

                        sTotalMoney += double.Parse(dr["Fsucpay_fee"].ToString());
                        fTotalMoney += double.Parse(dr["Ffailpay_fee"].ToString());
                        hTotalMoney += double.Parse(dr["FHandling_fee"].ToString());
                    }

                    this.lb_failAllMoney.Text = setConfig.FenToYuan(fTotalMoney.ToString());
                    this.lb_failNum.Text = fTotalNums.ToString();

                    this.lb_successAllMoney.Text = setConfig.FenToYuan(sTotalMoney.ToString());
                    this.lb_successNum.Text = sTotalNums.ToString();

                    this.lb_handlingMoney.Text = setConfig.FenToYuan(hTotalMoney.ToString());
                    this.lb_handlingNum.Text = hTotalNums.ToString();
                }
                else
                {
                    WebUtils.ShowMessage(this, "统计失败");
                    //return;
                }
                this.DataGrid_QueryResult.DataSource = ds;
                this.DataGrid_QueryResult.DataBind();
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, ex.Message);
            }
        }


        private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            this.pager.CurrentPageIndex = e.NewPageIndex;

            BindData(e.NewPageIndex);
        }

        protected void btn_outExcel_Click(object sender, System.EventArgs e)
        {
            BindDataOutExcel();

        }

        private void BindDataOutExcel()
        {
            DateTime sTime, eTime;
            string strSTime = "", strETime = "";
            try
            {
                try
                {
                    sTime = DateTime.Parse(this.tbx_beginDate.Text.Trim());
                    eTime = DateTime.Parse(this.tbx_endDate.Text.Trim());

                    strSTime = sTime.ToString("yyyy-MM-dd 00:00:00");
                    strETime = eTime.ToString("yyyy-MM-dd 23:59:59");
                }
                catch
                {
                    WebUtils.ShowMessage(this, "日期格式不正确");
                    return;
                }

                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                //qs.Finance_HeaderValue = setConfig.setFH(Session["OperID"].ToString(),Request.UserHostAddress);

                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

                string state = ddl_state.SelectedValue;

                DataSet ds = qs.QueryBatchInfo_DF(strSTime, strETime, this.tbx_spid.Text, this.tbx_spBatchID.Text, state,
                    -1, -1);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    WebUtils.ShowMessage(this, "查询结果为空");
                    return;
                }

                ds.Tables[0].Columns.Add("Ftotal_paynumName", typeof(string));
                ds.Tables[0].Columns.Add("Fsucpay_amountName", typeof(string));
                ds.Tables[0].Columns.Add("Ffailpay_amountName", typeof(string));
                ds.Tables[0].Columns.Add("FHandling_amountName", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["Ftotal_paynumName"] = setConfig.FenToYuan(dr["Ffact_fee"].ToString());
                    dr["Fsucpay_amountName"] = setConfig.FenToYuan(dr["Fsucpay_fee"].ToString());
                    dr["Ffailpay_amountName"] = setConfig.FenToYuan(dr["Ffailpay_fee"].ToString());
                    dr["FHandling_amountName"] = setConfig.FenToYuan(dr["FHandling_fee"].ToString());
                }

                DataTable dt = ds.Tables[0];
                StringWriter sw = new StringWriter();
                string excelHeader = DataGrid_QueryResult.Columns[0].HeaderText;
                for (int i = 1; i < DataGrid_QueryResult.Columns.Count;i++ )
                {
                    excelHeader += "\t" + DataGrid_QueryResult.Columns[i].HeaderText;
                }
                sw.WriteLine(excelHeader);
                foreach (DataRow dr in dt.Rows)
                {
                    sw.WriteLine("=\"" + dr["Fcreate_time"].ToString() + "\"\t=\"" + dr["Fmodify_time"].ToString() + "\"\t=\"" + dr["Fubatch_id"] + "\"\t=\""
                        + dr["Fspid"] + "\"\t=\"" + dr["Ftotal_paynumName"] + "\"\t=\"" + dr["Ffact_num"] + "\"\t=\"" + dr["Fsucpay_amountName"] + "\"\t=\""
                        + dr["Fsucpay_num"] + "\"\t=\"" + dr["Ffailpay_amountName"] + "\"\t=\"" + dr["Ffailpay_num"] + "\"\t=\""
                        + dr["FHandling_amountName"] + "\"\t=\"" + dr["FHandling_num"] + "\"\t=\"" + dr["FstateName"] + "\"\t=\"" + dr["Fresult_info"]+"\"");
                }
                sw.Close();
                Response.AddHeader("Content-Disposition", "attachment; filename=代付批量查询.xls");
                Response.ContentType = "application/ms-excel";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                Response.Write(sw);
                Response.End();
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, ex.Message);
            }
        }
    }

   
}