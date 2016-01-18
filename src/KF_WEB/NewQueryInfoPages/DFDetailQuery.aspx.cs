using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.C2C.Finance.BankLib;
using System.IO;
using System.Text;
using System.Web.Services.Protocols;
using CFT.Apollo.Logging;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
    public partial class DFBatchQuery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);
            this.DataGrid_QueryResult.ItemCommand += new DataGridCommandEventHandler(DataGrid_QueryResult_ItemCommand);

            if (!IsPostBack)
            {
                this.tbx_beginDate.Value = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd HH:mm:ss");
                this.tbx_endDate.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                //银行下拉列表
                GetAllBankList(ddlBankType);
                ddlBankType.Items.Insert(0, new ListItem("所有银行", ""));

                this.pager.PageSize = 10;
                this.pager.RecordCount = GetCount();

                if (Request.QueryString["spid"] != null || Request.QueryString["batchid"] != null)
                {
                    if (Request.QueryString["spid"] != null)
                        this.tbx_spid.Text = Request.QueryString["spid"].Trim();

                    if (Request.QueryString["batchid"] != null)
                        this.tbx_spBatchID.Text = Request.QueryString["batchid"].Trim();

                    if (Request.QueryString["sDate"] != "")
                        this.tbx_beginDate.Value = Request.QueryString["sDate"];

                    if (Request.QueryString["eDate"] != "")
                        this.tbx_endDate.Value = Request.QueryString["eDate"];

                    if (Request.QueryString["state"] != null)
                    {
                        if (Request.QueryString["state"] == "s")
                        {
                            this.ddl_state.SelectedIndex = 1;

                            //代付批量成功到单笔查询，单笔查询中成功的状态包含成功、退票两种状态
                           // 而且因为退票时间长，所以不能根据时间来查询。
                            this.tbx_beginDate.Value = "";
                            this.tbx_endDate.Value = "";
                        }
                        else if (Request.QueryString["state"] == "f")
                        {
                            this.ddl_state.SelectedIndex = 2;
                        }
                        else if (Request.QueryString["state"] == "h")
                        {
                            this.ddl_state.SelectedIndex = 3;
                        }
                    }

                    BindData(1, true);
                }
                
            }
        }

        public static void GetAllBankList(System.Web.UI.WebControls.DropDownList ddl)
        {
            ddl.Items.Clear();
            Hashtable ht = new Hashtable();
            ht = BankIO.GetBankHashTable();
            if (ht == null || ht.Count == 0) return;
            Hashtable htTurn = new Hashtable();
            //键值转换下，利于排序
            foreach (DictionaryEntry de in ht)
            {
                if (!(htTurn.Contains(de.Value.ToString())))
                {
                    htTurn.Add(de.Value.ToString(), de.Key.ToString());
                }
            }

            ArrayList akeys = new ArrayList(htTurn.Keys);
            akeys.Sort();
            foreach (string k in akeys)
            {
              //  ddlBankType.Items.Add(new ListItem(k.ToString(), bankTypeName[k].ToString()));

                ListItem li = new ListItem(k.ToString(),htTurn[k].ToString());
                ddl.Items.Add(li);
            }          
        }

        private int GetCount()
        {
            return 1000;
        }



        private void BindData(int pageIndex, bool needUpdateCount)
        {
            try
            {
                ValidData();
            }
            catch (Exception ex)
            {
                LogHelper.LogError("获取请求参数异常：" + ex.ToString(), "DFDetailQuery");
                WebUtils.ShowMessage(this, ex.Message);
                return;
            }
            
            try
            {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
            string strSTime = ViewState["strSTime"].ToString();
            string strETime = ViewState["strETime"].ToString();


                DataSet ds = qs.QueryDFInfo(this.tbx_bankID.Text, this.tbx_userName.Text,strSTime,
                    strETime, this.tbx_spid.Text, this.tbx_spListID.Text, this.tbx_spBatchID.Text, this.ddl_state.SelectedValue,
                    txb_transaction_id.Text.Trim(), ddlBankType.SelectedValue, ddl_service_code.SelectedValue,
                    (pageIndex - 1) * this.pager.PageSize + 1, this.pager.PageSize);


                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    WebUtils.ShowMessage(this, "查询结果为空");
                    this.btn_outExcel.Visible = false;
                    this.Clear();
                    return;
                }

                this.btn_outExcel.Visible = true;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //处理银行卡号
                    string bankId = dr["Fbank_id"].ToString();
                    dr["Fbank_id"] = classLibrary.setConfig.ConvertID(bankId, 5, 5);
                }

                if (needUpdateCount)
                {
                    if (this.ddl_state.SelectedValue == "0")
                    {
                        //TODO
                        //需要减少请求查询次数，能一次获取数据就一次返回。可以将多次请求表数据，保存到DataSet集合中，
                        //或可以将数据获取，再linq分类查询。
                        //v_swuzhang

                        DataSet ds2 = qs.CountDFInfo(this.tbx_bankID.Text, this.tbx_userName.Text, strSTime, strETime,
                            this.tbx_spid.Text, this.tbx_spListID.Text, this.tbx_spBatchID.Text, "0", 
                            txb_transaction_id.Text.Trim(), ddlBankType.SelectedValue, ddl_service_code.SelectedValue);


                        this.pager.RecordCount = int.Parse(ds2.Tables[0].Rows[0]["totalRecordCount"].ToString());

                        ds2 = qs.CountDFInfo(this.tbx_bankID.Text, this.tbx_userName.Text, strSTime, strETime,
                            this.tbx_spid.Text, this.tbx_spListID.Text, this.tbx_spBatchID.Text, "1",
                            txb_transaction_id.Text.Trim(), ddlBankType.SelectedValue, ddl_service_code.SelectedValue);


                        this.lb_successAllMoney.Text = setConfig.FenToYuan(ds2.Tables[0].Rows[0]["totalPayNum"].ToString());
                        this.lb_successNum.Text = ds2.Tables[0].Rows[0]["totalRecordCount"].ToString();

                        ds2 = qs.CountDFInfo(this.tbx_bankID.Text, this.tbx_userName.Text, strSTime, strETime,
                            this.tbx_spid.Text, this.tbx_spListID.Text, this.tbx_spBatchID.Text, "2",
                            txb_transaction_id.Text.Trim(), ddlBankType.SelectedValue, ddl_service_code.SelectedValue);
                        

                        this.lb_failAllMoney.Text = setConfig.FenToYuan(ds2.Tables[0].Rows[0]["totalPayNum"].ToString());
                        this.lb_failNum.Text = ds2.Tables[0].Rows[0]["totalRecordCount"].ToString();
                        
                        ds2 = qs.CountDFInfo(this.tbx_bankID.Text, this.tbx_userName.Text, strSTime, strETime,
                            this.tbx_spid.Text, this.tbx_spListID.Text, this.tbx_spBatchID.Text, "3",
                            txb_transaction_id.Text.Trim(), ddlBankType.SelectedValue, ddl_service_code.SelectedValue);
                        

                        this.lb_handlingAllMoney.Text = setConfig.FenToYuan(ds2.Tables[0].Rows[0]["totalPayNum"].ToString());
                        this.lb_handlingNum.Text = ds2.Tables[0].Rows[0]["totalRecordCount"].ToString();
                    }
                    else
                    {
                        DataSet ds2 = qs.CountDFInfo(this.tbx_bankID.Text, this.tbx_userName.Text, strSTime, strETime,
                            this.tbx_spid.Text, this.tbx_spListID.Text, this.tbx_spBatchID.Text, this.ddl_state.SelectedValue,
                            txb_transaction_id.Text.Trim(), ddlBankType.SelectedValue, ddl_service_code.SelectedValue);
                        

                        switch (this.ddl_state.SelectedValue)
                        {
                            case "1":
                                {
                                    this.pager.RecordCount = int.Parse(ds2.Tables[0].Rows[0]["totalRecordCount"].ToString());
                                    this.lb_successAllMoney.Text = setConfig.FenToYuan(ds2.Tables[0].Rows[0]["totalPayNum"].ToString());
                                    this.lb_successNum.Text = ds2.Tables[0].Rows[0]["totalRecordCount"].ToString();
                                    this.lb_failAllMoney.Text = "0";
                                    this.lb_failNum.Text = "0";
                                    this.lb_handlingAllMoney.Text = "0";
                                    this.lb_handlingNum.Text = "0";
                                    break;
                                }
                            case "2":
                                {
                                    this.pager.RecordCount = int.Parse(ds2.Tables[0].Rows[0]["totalRecordCount"].ToString());
                                    this.lb_failAllMoney.Text = setConfig.FenToYuan(ds2.Tables[0].Rows[0]["totalPayNum"].ToString());
                                    this.lb_failNum.Text = ds2.Tables[0].Rows[0]["totalRecordCount"].ToString();
                                    this.lb_successAllMoney.Text = "0";
                                    this.lb_successNum.Text = "0";
                                    this.lb_handlingAllMoney.Text = "0";
                                    this.lb_handlingNum.Text = "0";
                                    break;
                                }
                            case "3":
                                {
                                    this.pager.RecordCount = int.Parse(ds2.Tables[0].Rows[0]["totalRecordCount"].ToString());
                                    this.lb_handlingAllMoney.Text = setConfig.FenToYuan(ds2.Tables[0].Rows[0]["totalPayNum"].ToString());
                                    this.lb_handlingNum.Text = ds2.Tables[0].Rows[0]["totalRecordCount"].ToString();
                                    this.lb_failAllMoney.Text = "0";
                                    this.lb_failNum.Text = "0";
                                    this.lb_successAllMoney.Text = "0";
                                    this.lb_successNum.Text = "0";
                                    break;
                                }
                        }
                    }
                }
              
                this.DataGrid_QueryResult.DataSource = ds;
                this.DataGrid_QueryResult.DataBind();
            }
            catch (Exception ex)
            {
                LogHelper.LogError("出现异常：" + ex.ToString(), "DFDetailQuery");
                WebUtils.ShowMessage(this, PublicRes.GetErrorMsg(ex.Message));
            }
        }

        private void ValidData()
        {
            if (string.IsNullOrEmpty(this.tbx_bankID.Text.Trim()) && string.IsNullOrEmpty(this.tbx_userName.Text.Trim()) &&
                string.IsNullOrEmpty(this.tbx_spid.Text.Trim()) && string.IsNullOrEmpty(this.tbx_spListID.Text.Trim()) &&
                string.IsNullOrEmpty(this.txb_transaction_id.Text.Trim()) && string.IsNullOrEmpty(this.tbx_spBatchID.Text.Trim()))
            {
                throw new Exception("请输入查询条件！");
            }

            DateTime sTime, eTime;
            ViewState["strSTime"] = "";
            ViewState["strETime"] = "";
            string begin = this.tbx_beginDate.Value.Trim();
            string end = this.tbx_endDate.Value.Trim();
            try
            {
                if ((!string.IsNullOrEmpty(begin)) && (!string.IsNullOrEmpty(end)))
                {
                    sTime = DateTime.Parse(this.tbx_beginDate.Value);
                    eTime = DateTime.Parse(this.tbx_endDate.Value);

                    ViewState["strSTime"] = sTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ViewState["strETime"] = eTime.ToString("yyyy-MM-dd HH:mm:ss");

                    if (sTime.AddDays(7) <= eTime)
                    {
                        throw new Exception("日期跨度不能大于一周");
                    }
                }
            }
            catch
            {
                throw new Exception("日期格式不正确");
            }

            if (!string.IsNullOrEmpty(this.tbx_spBatchID.Text.Trim()))
            {
                if (string.IsNullOrEmpty(this.tbx_spid.Text.Trim()))
                {
                    throw new Exception("请输入商户号一起查询！");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(begin) || string.IsNullOrEmpty(end))
                {
                    throw new Exception("请输入查询时间段！");
                }
            }
        }

        protected void btn_serach_Click(object sender, System.EventArgs e)
        {
           
            BindData(1, true);

            this.pager.CurrentPageIndex = 1;
        }

        private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            this.pager.CurrentPageIndex = e.NewPageIndex;

            BindData(e.NewPageIndex, false);
        }


        private void Clear()
        {
            this.lb_c1.Text = "";
            this.lb_c2.Text = "";
            this.lb_c3.Text = "";
            this.lb_c4.Text = "";
            this.lb_c5.Text = "";
            this.lb_c6.Text = "";
            this.lb_c7.Text = "";
            this.lb_c8.Text = "";
            this.lb_c9.Text = "";
            this.lb_c10.Text = "";
            this.lb_c11.Text = "";
            this.lb_c12.Text = "";
            this.lb_c13.Text = "";
            this.lb_c14.Text = "";
            this.lb_c15.Text = "";
            this.lb_c16.Text = "";
            this.lb_c17.Text = "";

            this.lb_c18.Text = "";
            this.lb_c19.Text = "";
            this.lb_c20.Text = "";
            this.lb_c21.Text = "";
            this.lb_c22.Text = "";
            this.lb_c23.Text = "";
            this.lb_c24.Text = "";

            this.DataGrid_QueryResult.DataSource = null;
            this.DataGrid_QueryResult.DataBind();
        }



        private DataSet GetDKDetail(string cep_id)
        {
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

                return qs.QueryDFDetail(cep_id);
            }
            catch (Exception ex)
            {
                LogHelper.LogError("出现异常：" + ex.ToString(), "DFDetailQuery");
                WebUtils.ShowMessage(this, PublicRes.GetErrorMsg(ex.Message));
                return null;
            }
        }


        private void DataGrid_QueryResult_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "detail")
            {
                DataSet ds = GetDKDetail(e.Item.Cells[0].Text.Trim());

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    WebUtils.ShowMessage(this, "查询结果为空");
                    Clear();
                    return;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                //自动跳转到直付商户查询结果中
                this.lb_c1.Text = "<a href=../BaseAccount/PayBusinessQuery.aspx?spid=" + dr["Fspid"].ToString() + " target=_blank>" + dr["Fspid"].ToString() + "</a>";
                //this.lb_c1.Text = dr["Fspid"].ToString();
                this.lb_c2.Text = setConfig.FenToYuan(dr["Famount"].ToString());
                this.lb_c3.Text = dr["Fbatch_id"].ToString();
                this.lb_c4.Text = dr["Fdesc"].ToString();
                this.lb_c5.Text = dr["Ftransaction_id"].ToString();
                this.lb_c6.Text = dr["Fmodify_time"].ToString();

                this.lb_c7.Text = dr["Ftransaction_id"].ToString();
                this.lb_c8.Text = "";
                this.lb_c9.Text = dr["Fcreate_time"].ToString();
                this.lb_c10.Text = dr["Fauto_id"].ToString();
                this.lb_c11.Text = dr["FstatusName"].ToString();
                this.lb_c12.Text = dr["Fu_serialno"].ToString();

                this.lb_c13.Text = dr["Frecv_true_name"].ToString();
                this.lb_c14.Text = dr["Fresult_info"].ToString();
                this.lb_c15.Text = "";
                this.lb_c16.Text = dr["Fbank_typeName"].ToString();
                this.lb_c17.Text = dr["Farea_str"].ToString();
                this.lb_c18.Text = "";
                this.lb_c19.Text = dr["Fcity_str"].ToString();
                this.lb_c20.Text = "";

                this.lb_c21.Text = dr["Fbank_name"].ToString();
                this.lb_c22.Text = "";

                //处理银行卡号
                string bankId = dr["Fbank_id"].ToString();
                this.lb_c23.Text = classLibrary.setConfig.ConvertID(bankId, 5, 5);

            }
        }


        protected void btn_outExcel_Click(object sender, System.EventArgs e)
        {
            BindDataOutExcel();
        
        }

        private void BindDataOutExcel()
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

            try
            {
                string strSTime, strETime;
                try
                {
                    strSTime = DateTime.Parse(this.tbx_beginDate.Value).ToString("yyyy-MM-dd HH:mm:ss");
                    strETime = DateTime.Parse(this.tbx_endDate.Value).ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch (Exception ex)
                {
                    LogHelper.LogError("出现异常：" + ex.ToString(), "DFDetailQuery");
                    WebUtils.ShowMessage(this, "日期格式不正确");
                    return;
                }

                DataSet ds = qs.QueryDFInfo(this.tbx_bankID.Text, this.tbx_userName.Text, strSTime,
                    strETime, this.tbx_spid.Text, this.tbx_spListID.Text, this.tbx_spBatchID.Text, this.ddl_state.SelectedValue,
                    txb_transaction_id.Text.Trim(), ddlBankType.SelectedValue, ddl_service_code.SelectedValue,
                    -1, -1);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    WebUtils.ShowMessage(this, "查询结果为空");
                    this.Clear();
                    return;
                }

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //处理银行卡号
                    string bankId = dr["Fbank_id"].ToString();
                    dr["Fbank_id"] = classLibrary.setConfig.ConvertID(bankId, 5, 5);
                }

                DataTable dt = ds.Tables[0];
                StringWriter sw = new StringWriter();
                string excelHeader = DataGrid_QueryResult.Columns[2].HeaderText;
                for (int i = 3; i < DataGrid_QueryResult.Columns.Count; i++)
                {
                    excelHeader += "\t" + DataGrid_QueryResult.Columns[i].HeaderText;
                }
                sw.WriteLine(excelHeader);
               // sw.WriteLine("批次号\t付款单号\t提交时间\t订单金额\t账户类型\t交易状态\t最后修改时间\t银行渠道\t银行卡号\t用户名\t失败原因");

                foreach (DataRow dr in dt.Rows)
                {
                    sw.WriteLine("=\"" + dr["Fubatch_id"].ToString() + "\"\t=\"" + dr["Ftransaction_id"].ToString() + "\"\t=\"" + dr["Fcreate_time"]
                        + "\"\t=\"" + dr["FamountName"] + "\"\t=\"" + dr["Fuser_typeName"] + "\"\t=\"" + dr["FstatusName"]
                    + "\"\t=\"" + dr["Fmodify_time"] + "\"\t=\"" + dr["Fbank_typeName"] + "\"\t=\"" + dr["Fbank_id"]
                    + "\"\t=\"" + dr["Frecv_true_name"] + "\"\t=\"" + dr["Fresult_info"] + "\"");
                }
                sw.Close();
                Response.AddHeader("Content-Disposition", "attachment; filename=代付单笔查询.xls");
                Response.ContentType = "application/ms-excel";
                Response.ContentEncoding = Encoding.UTF8;
                Response.Write(sw);
                Response.End(); 
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                LogHelper.LogError("出现异常：" + eSoap.ToString(), "DFDetailQuery");
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception ex)
            {
                LogHelper.LogError("出现异常：" + ex.ToString(), "DFDetailQuery");
                WebUtils.ShowMessage(this, ex.Message);
            }
        }

        /// <summary>
        /// datatable导出excel
        /// </summary>
        /// <param name="dt">datatable</param>
        /// <param name="excelHeader">excel表头  例如：批次号\t付款单号\t提交时间\t订单金额\t账户类型\t交易状态\t最后修改时间\t银行渠道\t银行卡号\t用户名\t失败原因</param>
        /// <param name="excelRowMember">单元格对应datatable字段  例如："=\"" + dr["Fubatch_id"].ToString() + "\"\t=\"" + dr["Ftransaction_id"].ToString() + "\"\t=\"" + dr["Fcreate_time"] + "\"\t" + dr["FamountName"] + "\t" + dr["Fuser_typeName"] + "\t" + dr["FstatusName"] + "\t=\"" + dr["Fmodify_time"] + "\"\t" + dr["Fbank_typeName"] + "\t" + dr["Fbank_id"] + "\t" + dr["Frecv_true_name"] + "\t" + dr["Fresult_info"]</param>
        /// <returns></returns>
        public void datatableOutExcel(DataTable dt, string excelHeader, string excelRowMember,string excelName)
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine(excelHeader);
            foreach (DataRow dr in dt.Rows)
            {
                sw.WriteLine(excelRowMember);
            }
            sw.Close();
            Response.AddHeader("Content-Disposition", "attachment; filename="+excelName+".xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(sw);
            Response.End();
        }

    }
}