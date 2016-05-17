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
using CFT.CSOMS.BLL.RefundModule;
using SunLibrary;
using System.Collections.Generic;
using CFT.CSOMS.BLL.InternetBank;
using log4net;
using System.Data.OleDb;
using CFT.CSOMS.BLL.WechatPay;
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.InternetBank
{
    /// <summary>
    /// RefundQuery 的摘要说明。
    /// </summary>
    public partial class RefundQuery : System.Web.UI.Page
    {
        protected static List<int> refundIdList = new List<int>();

        protected void Page_Load(object sender, System.EventArgs e)
        {
            //string spath = Server.MapPath("~/") + "PLFile\\网银退款201605161605229563_1_3.xls";
            //commLib.FPSFileHelper.UploadFile(spath);

            // string txtFilePath = Server.MapPath("~/") + "PLFile\\{0}_{1}.txt";
            // string txtSuccess = string.Format(txtFilePath, "asdf", "success");
            // WriteTxt(txtSuccess,"1","2", "3", "4");
            //var dicSuccessFile = WriteXls(txtSuccess, "asdf", "success");

            // 在此处放置用户代码以初始化页面
            try
            {
                Label1.Text = Session["uid"].ToString();
                if (!classLibrary.ClassLib.ValidateRight("InternetBankRefund", this))
                {
                    btnSubRefund.Visible = false;
                }

                if (!IsPostBack)
                {
                    btnNew.Visible = classLibrary.ClassLib.ValidateRight("RefundCheck", this);
                    btnUpload.Visible = classLibrary.ClassLib.ValidateRight("RefundCheck", this);  //退款登记导入excel按钮权限

                    TextBoxBeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                    setConfig.GetAllTypeList(ddlTradeState, "PAY_STATE");
                    ddlTradeState.Items.Insert(0, new ListItem("全部", "0"));
                    //退款登记模版 v_yqyqguo
                    //DownloadTemplate.NavigateUrl = System.Configuration.ConfigurationManager.AppSettings["GetImageFromKf2Url"].ToString() + DownloadTemplate.NavigateUrl;

                    refundIdList.Clear();
                    DataSet ds = new InternetBankService().GetRefundByFrefundId(0, "", "", 0, 0);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            this.ddl_refund_id.DataSource = ds.Tables[0];
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                refundIdList.Add(Convert.ToInt32(item["Frefund_id"]));
                            }
                        }
                    }
                    else
                    {
                        this.ddl_refund_id.DataSource = null;
                    }
                    this.ddl_refund_id.DataTextField = "Frefund_id";
                    this.ddl_refund_id.DataValueField = "Frefund_id";
                    this.ddl_refund_id.DataBind();
                    this.ddl_refund_id.Items.Insert(0, new ListItem("全部", "0"));
                }
                Table3.Visible = false;
                Table2.Visible = true;
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
        }

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

        }
        #endregion

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }

        private void ValidateDate()
        {
            DateTime begindate;
            DateTime enddate;

            try
            {
                begindate = DateTime.Parse(TextBoxBeginDate.Text);
                enddate = DateTime.Parse(TextBoxEndDate.Text);
            }
            catch
            {
                throw new Exception("日期输入有误！");
            }

            if (begindate.CompareTo(enddate) > 0)
            {
                throw new Exception("终止日期小于起始日期，请重新输入！");
            }

        }

        public void Button1_Click(object sender, System.EventArgs e)
        {
            try
            {
                ValidateDate();
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }

            try
            {
                BindData(1);
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                LogHelper.LogError("Export_Click 异常！" + eSoap.Message + ", stacktrace" + eSoap.StackTrace + "\r\n" + eSoap.ToString());
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                LogHelper.LogError("Button1_Click 异常！" + eSys.Message + ", stacktrace" + eSys.StackTrace + "\r\n" + eSys.ToString());
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }
        }

        public void btnNew_Click(object sender, System.EventArgs e)
        {
            //新增

            Response.Redirect("RefundInfo.aspx?listid=");
        }


        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string fid = e.Item.Cells[0].Text.Trim(); //ID

            switch (e.CommandName)
            {
                case "CHANGE": //编辑
                    Response.Redirect("RefundInfo.aspx?listid=" + fid);
                    break;
                case "DEL": //删除
                    DelRefund(fid);
                    break;
            }
        }

        public void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            object obj = e.Item.Cells[13].FindControl("lbChange");
            if (obj != null)
            {
                LinkButton lb = (LinkButton)obj;
                lb.Attributes["onClick"] = "return confirm('确定要执行“编辑”操作吗？');";
            }
            object obj2 = e.Item.Cells[13].FindControl("lbDel");
            if (obj2 != null)
            {
                LinkButton lb2 = (LinkButton)obj2;
                lb2.Attributes["onClick"] = "return confirm('确定要执行“删除”操作吗？');";
            }
        }

        private void BindData(int index)
        {
            DateTime begindate = DateTime.Parse(TextBoxBeginDate.Text);
            string stime = begindate.ToString("yyyy-MM-dd 00:00:00");
            DateTime enddate = DateTime.Parse(TextBoxEndDate.Text);
            string etime = enddate.ToString("yyyy-MM-dd 23:59:59");

            string listid = listId.Text.Trim();                       //订单编码
            string cftorderid = cftOrderId.Text.Trim();               //财付通订单号
            int refund_id = int.Parse(ddl_refund_id.SelectedValue);   //商户号
            string submit_user = this.tbx_submit_user.Text.Trim();    //登记人
            int rf_type = int.Parse(ddlRefundType.SelectedValue);     //退款类型
            int rf_status = int.Parse(ddlRefundStatus.SelectedValue); //提交退款状态
            string s_trade_state = ddlTradeState.SelectedValue;       //交易状态
            string batchNum = batch_num.Text.Trim();                  //批次号

            int count = new RefundRegisterService().QueryRefundRegisterCount(listid, cftorderid, stime, etime, rf_type, rf_status,
                batchNum, s_trade_state, refund_id, submit_user);
            Label9.Text = count.ToString();

            pager.RecordCount = count;
            pager.CurrentPageIndex = index;
            int max = pager.PageSize;
            int start = max * (index - 1);

            DataSet ds = new RefundRegisterService().QueryRefundRegisterList(listid, cftorderid, stime, etime, rf_type, rf_status,
                batchNum, s_trade_state, refund_id, submit_user, start, max);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("Frefund_type_str", typeof(String));
                ds.Tables[0].Columns.Add("Fsubmit_refund_str", typeof(String));
                ds.Tables[0].Columns.Add("Ftrade_state_str", typeof(String));
                ds.Tables[0].Columns.Add("Famount_str", typeof(String));
                ds.Tables[0].Columns.Add("Frefund_amountStr", typeof(String));

                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "投诉退款");
                ht1.Add("2", "投诉退款");
                ht1.Add("3", "投诉退款");
                ht1.Add("4", "投诉退款");
                ht1.Add("5", "投诉退款");
                ht1.Add("10", "投诉退款"); //老数据退款类型为1\2\3\4\5的都归纳为投诉退款
                ht1.Add("11", "发货失败");
                Hashtable ht2 = new Hashtable();
                ht2.Add("1", "已提交");
                ht2.Add("2", "未提交");
                ht2.Add("3", "失效");

                //Ftrade_state_new DAL数据库获取信息后添加
                classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0], "Ftrade_state_new", "Ftrade_state_str", "PAY_STATE");

                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Famount", "Famount_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Frefund_amount", "Frefund_amountStr");
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Frefund_type", "Frefund_type_str", ht1);
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fsubmit_refund", "Fsubmit_refund_str", ht2);

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
            }
            else
            {
                DataGrid1.DataSource = null;
                DataGrid1.DataBind();
            }
        }

        public void Export_Click(object sender, System.EventArgs e)
        {
            try
            {
                ValidateDate();
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }

            try
            {
                Thread t = new Thread(ExportData);
                t.Start();
                WebUtils.ShowMessage(this.Page, "后台处理中，稍后请查收邮件。");
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                LogHelper.LogError("Export_Click 异常！" + eSoap.Message + ", stacktrace" + eSoap.StackTrace + "\r\n" + eSoap.ToString());
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception ex)
            {
                LogHelper.LogError("Export_Click 异常！" + ex.Message + ", stacktrace" + ex.StackTrace + "\r\n" + ex.ToString());
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        private void ExportData()
        {
            string uin = Session["uid"].ToString();
            if (string.IsNullOrEmpty(uin))
            {
                throw new Exception("账号不能为空，请重新登录！");
            }

            DateTime begindate = DateTime.Parse(TextBoxBeginDate.Text);
            string stime = begindate.ToString("yyyy-MM-dd 00:00:00");
            DateTime enddate = DateTime.Parse(TextBoxEndDate.Text);
            string etime = enddate.ToString("yyyy-MM-dd 23:59:59");

            string listid = listId.Text.Trim();
            string cftorderid = cftOrderId.Text.Trim();
            int refund_id = int.Parse(ddl_refund_id.SelectedValue);  //商户号
            string submit_user = this.tbx_submit_user.Text.Trim();   //登记人
            int rf_type = int.Parse(ddlRefundType.SelectedValue);
            int rf_status = int.Parse(ddlRefundStatus.SelectedValue);
            string s_trade_state = ddlTradeState.SelectedValue;
            string batchNum = batch_num.Text.Trim();

            List<string> fileAtta = new List<string>();
            try
            {
                int total = new RefundRegisterService().QueryRefundRegisterCount(listid, cftorderid, stime, etime, rf_type, rf_status,
                    batchNum, s_trade_state, refund_id, submit_user);

                if (total > 0)
                {
                    int maxSheet = 3000;
                    int countSheet = 1;
                    if (total > maxSheet)
                    {
                        countSheet = total % maxSheet == 0 ? total / maxSheet : (total / maxSheet) + 1;
                    }

                    for (int j = 0; j < countSheet; j++)
                    {
                        DataSet ds = new RefundRegisterService().QueryRefundRegisterList(listid, cftorderid, stime, etime, rf_type, rf_status,
                            batchNum, s_trade_state, refund_id, submit_user, j * maxSheet, maxSheet);
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            System.Data.DataTable dt = ds.Tables[0];
                            ds.Tables[0].Columns.Add("Frefund_type_str", typeof(String));
                            ds.Tables[0].Columns.Add("Fsubmit_refund_str", typeof(String));
                            ds.Tables[0].Columns.Add("Ftrade_state_str", typeof(String));
                            ds.Tables[0].Columns.Add("Famount_str", typeof(String));
                            ds.Tables[0].Columns.Add("Frefund_amountStr", typeof(String));
                            Hashtable ht1 = new Hashtable();
                            ht1.Add("1", "投诉退款");
                            ht1.Add("2", "投诉退款");
                            ht1.Add("3", "投诉退款");
                            ht1.Add("4", "投诉退款");
                            ht1.Add("5", "投诉退款");
                            ht1.Add("10", "投诉退款"); //老数据退款类型为1\2\3\4\5的都归纳为投诉退款
                            ht1.Add("11", "发货失败");
                            Hashtable ht2 = new Hashtable();
                            ht2.Add("1", "已提交");
                            ht2.Add("2", "未提交");
                            ht2.Add("3", "失效");

                            classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0], "Ftrade_state_new", "Ftrade_state_str", "PAY_STATE");

                            classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Famount", "Famount_str");
                            classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Frefund_amount", "Frefund_amountStr");
                            classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Frefund_type", "Frefund_type_str", ht1);
                            classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fsubmit_refund", "Fsubmit_refund_str", ht2);

                            try
                            {
                                #region 生成excel文件
                                var temp = DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.StaticNoManage();
                                string path = Server.MapPath("~/") + "PLFile" + "\\" + temp + "退款登记" + uin + ".xls"; //附件

                                Application xlApp = new ApplicationClass();
                                Workbooks workbooks = xlApp.Workbooks;
                                Workbook workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                                Worksheet worksheet = (Worksheet)workbook.Worksheets[1];//取得sheet1

                                Range range;
                                List<string> titList = new List<string>()
                        {
                            "财付通订单号","订单编码","交易状态","交易金额","买家账号","交易说明","登记人","物品回收人","退款类型","SAM工单号",
                            "创建时间","提交退款","退款金额"
                        };
                                for (int i = 0; i < titList.Count; i++)
                                {
                                    range = (Range)worksheet.Cells[1, i + 1];
                                    range.ColumnWidth = 40;
                                    range.NumberFormatLocal = "@";
                                    range.Font.Size = 15;
                                    range.Value2 = titList[i];
                                }

                                workbook.Saved = true;
                                //workbook.SaveCopyAs(path);  //2007版本
                                workbook.SaveAs(path, 56, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                    XlSaveAsAccessMode.xlExclusive, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                                range = null;
                                workbooks = null;
                                workbook = null;

                                if (xlApp != null)
                                {
                                    xlApp.Workbooks.Close();
                                    xlApp.Quit();
                                    xlApp = null;
                                }
                                fileAtta.Add(path);

                                #region excel写入数据
                                string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + path + ";" + "Excel 12.0 Xml;HDR=YES;";
                                OleDbConnection conn = new OleDbConnection(strConn);
                                conn.Open();
                                OleDbCommand cmd = new OleDbCommand();
                                cmd.Connection = conn;

                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    cmd.CommandText = string.Format(@"
                                    INSERT INTO [sheet1$] 
                                        (财付通订单号,订单编码,交易状态,交易金额,买家账号,交易说明,登记人,物品回收人,退款类型,SAM工单号,
                                        创建时间,提交退款,退款金额) 
                                    VALUES
                                        ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',
                                        '{10}','{11}','{12}')",
                                    dr["Forder_id"].ToString(), dr["Fcoding"].ToString(), dr["Ftrade_state_str"].ToString(), dr["Famount_str"].ToString(),
                                    dr["Fbuy_acc"].ToString(), dr["Ftrade_desc"].ToString(), dr["Fsubmit_user"].ToString(), dr["Frecycle_user"].ToString(),
                                    dr["Frefund_type_str"].ToString(), dr["Fsam_no"].ToString(), dr["Fcreate_time"].ToString(), dr["Fsubmit_refund_str"].ToString(),
                                    dr["Frefund_amountStr"].ToString());
                                    cmd.ExecuteNonQuery();
                                }
                                conn.Close();
                                #endregion

                                #endregion
                            }
                            catch (Exception ex)
                            {
                                LogHelper.LogError("退款登记生成Excel失败！" + ex.Message + ", stacktrace" + ex.StackTrace);
                            }
                            finally
                            {
                                GC.Collect();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError("导出退款登记失败！" + ex.Message + ", stacktrace" + ex.StackTrace + "\r\n" + ex.ToString());
            }

            if (fileAtta.Count > 0)
            {
                try
                {
                    CommMailSend.SendInternalMail(uin, "", "退款登记导出", "", false, fileAtta.ToArray());
                }
                catch (Exception ex)
                {
                    LogHelper.LogError("导出退款登记失败！" + ex.Message + ", stacktrace" + ex.StackTrace + "\r\n" + ex.ToString());
                }
            }
        }

        private void Upload()
        {
            string uid = Session["uid"].ToString();
            try
            {
                int succ = 0, fail = 0;
                int FRefundtype = 0;//退款类型
                int refundAmount = 0; //退款金额
                string errMsg = "";

                string path = Server.MapPath("~/") + "PLFile" + "\\refund.xls";
                File1.PostedFile.SaveAs(path);
                DataSet res_ds = PublicRes.readXls(path, "F1,F2,F3,F4,F5,F6");
                System.Data.DataTable res_dt = res_ds.Tables[0];
                //记录失败订单
                System.Data.DataTable failed_dt = new System.Data.DataTable();
                failed_dt.Columns.Add("订单号", typeof(System.String));
                failed_dt.Columns.Add("退款类型", typeof(System.String));
                failed_dt.Columns.Add("SAM工单号", typeof(System.String));
                failed_dt.Columns.Add("登记人", typeof(System.String));
                failed_dt.Columns.Add("物品回收人", typeof(System.String));
                failed_dt.Columns.Add("退款金额（元）", typeof(System.String));
                failed_dt.Columns.Add("Message", typeof(System.String));

                int iColums = res_dt.Columns.Count;
                int iRows = res_dt.Rows.Count;

                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                for (int i = 0; i < iRows; i++)
                {
                    string r1 = res_dt.Rows[i][0].ToString();
                    string r2 = res_dt.Rows[i][1].ToString();
                    string r3 = res_dt.Rows[i][2].ToString();//SAM工单号
                    string r4 = res_dt.Rows[i][3].ToString(); //登记人
                    string r5 = res_dt.Rows[i][4].ToString(); //物品回收人
                    string r6 = res_dt.Rows[i][5].ToString(); //退款金额

                    //记录错误订单
                    DataRow failed_dr = failed_dt.NewRow();
                    failed_dr[0] = r1;
                    failed_dr[1] = r2;
                    failed_dr[2] = r3;
                    failed_dr[3] = r4;
                    failed_dr[4] = r5;
                    failed_dr[5] = r6;

                    try
                    {
                        FRefundtype = Convert.ToInt32(r2);
                    }
                    catch
                    {
                        fail++;
                        errMsg += "第" + (i + 1) + "行:订单类型错误！";
                        failed_dr[6] = "订单类型错误";
                        failed_dt.Rows.Add(failed_dr);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(r6))
                        {
                            r6 = r6.Replace("元", "");
                            refundAmount = classLibrary.setConfig.YuanToFen(Convert.ToDouble(r6));
                        }
                    }
                    catch
                    {
                        fail++;
                        errMsg += "第" + (i + 1) + "行:退款金额输入错误！";
                        failed_dr[6] = "退款金额输入错误";
                        failed_dt.Rows.Add(failed_dr);
                        continue;
                    }
                    //组装
                    Query_Service.RefundInfoClass cb = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.RefundInfoClass();
                    cb.FOrderId = r1.Trim();
                    cb.FRefund_type = FRefundtype;
                    cb.FSam_no = r3.Trim();
                    cb.FSubmit_user = r4.Trim();
                    cb.FRecycle_user = r5.Trim();
                    cb.FRefund_state = 1;
                    cb.FRefund_amount = refundAmount.ToString();

                    try
                    {
                        qs.AddRefundInfo(cb);
                        succ++;
                    }
                    catch (SoapException ser)
                    {
                        fail++;
                        errMsg += "第" + (i + 1) + "行:" + ser.Message;
                        failed_dr[6] = ser.Message;
                        failed_dt.Rows.Add(failed_dr);
                    }
                }

                try
                {
                    path = Server.MapPath("~/") + "PLFile" + "\\" + uid + ".xls"; //附件
                    PublicRes.Export(failed_dt, path);
                    string[] fileAtta = { path };
                    //mail
                    CommMailSend.SendInternalMail(uid, "", "批量导入退款登记;总数：" + iRows + ";成功数：" + succ + ";失败数：" + fail, "", false, fileAtta);

                }
                catch (Exception e)
                {
                    throw e;
                }
                //展示成功多少，失败多少，错误信息
                Table3.Visible = true;
                Table2.Visible = false;

                lbTotal.Text = iRows.ToString();
                lbSucc.Text = succ.ToString();
                lbFail.Text = fail.ToString();
                lbError.Text = errMsg;
            }
            catch (Exception e)
            {
                LogHelper.LogError("批量导入退款登记失败!" + e.ToString());
                CommMailSend.SendInternalMail(uid, "", "批量导入退款登记失败", e.ToString(), false);
            }

        }
        public void btnUpload_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!File1.HasFile)
                {
                    WebUtils.ShowMessage(this.Page, "请选择上传文件！");
                    return;
                }
                if (Path.GetExtension(File1.FileName).ToLower() == ".xls")
                {
                    string path = Server.MapPath("~/") + "PLFile" + "\\refund.xls";
                    File1.PostedFile.SaveAs(path);
                    DataSet res_ds = PublicRes.readXls(path, "F1,F2,F3,F4,F5,F6");
                    System.Data.DataTable res_dt = res_ds.Tables[0];
                    if (res_dt != null && res_dt.Rows.Count > 0)
                    {
                        string orderId = "", filterOrder = "", wxBigOrder = string.Empty;
                        foreach (DataRow item in res_dt.Rows)
                        {
                            orderId = item[0].ToString();
                            if (orderId.Length >= 10 && !refundIdList.Contains(Convert.ToInt32(orderId.Substring(0, 10))))
                            {
                                filterOrder += orderId + "，";
                            }

                            try
                            {
                                DataSet ds = new TradeService().GetPayByListid(orderId); //查询微信转账业务
                                if (ds != null && ds.Tables[0].Rows.Count > 0)
                                {
                                    string wxTradeId = ds.Tables[0].Rows[0]["Fcoding"].ToString();//子账户关联订单号
                                    if (wxTradeId.Contains("mkt") || wxTradeId.Contains("wxp"))
                                    {
                                        wxBigOrder += orderId + "，";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LogHelper.LogError("查询微信转账业务异常：" + ex.Message + ex.StackTrace);
                                WebUtils.ShowMessage(this.Page, "查询微信转账业务异常：" + ex.Message + ex.StackTrace);
                            }
                        }
                        if (!string.IsNullOrEmpty(filterOrder))
                        {
                            WebUtils.ShowMessage(this.Page, filterOrder + " 商家的订单不允许走网银退款。");
                            return;
                        }
                        if (!string.IsNullOrEmpty(wxBigOrder))
                        {
                            WebUtils.ShowMessage(this.Page, wxBigOrder + " 微信大单不允许走网银退款。");
                            return;
                        }
                    }
                    //  Upload();
                    Thread thread = new Thread(Upload);
                    thread.Start();
                    WebUtils.ShowMessage(this.Page, "后台处理中，稍后请查收邮件。");
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "文件格式不正确，请选择xls格式文件上传。");
                    return;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(" public void btnUpload_Click(object sender, System.EventArgs e)！ 异常：" + ex.ToString() + ",StackTrace=" + ex.StackTrace);
                WebUtils.ShowMessage(this.Page, ex.Message);
            }
        }

        /// <summary>
        /// 提交账务退款
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnRefundEmail_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!classLibrary.ClassLib.ValidateRight("InternetBankRefund", this))
                {
                    //权限判断
                    WebUtils.ShowMessage(this.Page, "没有权限！");
                    return;
                }

                DateTime begindate, enddate;
                if (!DateTime.TryParse(TextBoxBeginDate.Text, out begindate) || !DateTime.TryParse(TextBoxEndDate.Text, out enddate))
                {
                    WebUtils.ShowMessage(this.Page, "请输入正确的时间。");
                    return;
                }
                ZWBatchPay_Service.BatchPay_Service bs = new ZWBatchPay_Service.BatchPay_Service();
                bs.Finance_HeaderValue = classLibrary.setConfig.ZWSetFH(this);

                string stime = begindate.ToString("yyyy-MM-dd 00:00:00");
                string etime = enddate.ToString("yyyy-MM-dd 23:59:59");

                const int fileRowConut = 1000;  //单个文件最大记录数
                List<string> fidList = new List<string>(); //需要更改状态的财付通订单Fid
                System.Data.DataTable dt = null;
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                string no = DateTime.Now.ToString("yyyyMMddHHmmssffff");  //退款批次

                LogHelper.LogInfo(string.Format("btnRefundEmail_Click batch_num ={0},stime={1},etime={2}", no, stime, etime));

                #region 更新提交状态为 (1 = 已提交)   的委托
                Action<List<string>, string> updateState = (fidArr, batchNum) =>
                {
                    int fidCount = 100;
                    string fids = string.Empty;
                    int updNum = fidArr.Count % fidCount == 0 ? fidArr.Count / fidCount : (fidArr.Count / fidCount) + 1;
                    for (int i = 0; i < updNum; i++)
                    {
                        for (int j = i * fidCount; j < fidArr.Count; j++)
                        {
                            fids += fidArr[j] + ",";
                        }

                        LogHelper.LogInfo(string.Format("btnRefundEmail_Click UpdateSubmitRefundState ，fids={0}, state={1},batchNum=={2},updNum={3}", fids.TrimEnd(','), 1, batchNum, updNum));

                        qs.UpdateSubmitRefundState(fids.TrimEnd(','), 1, batchNum);
                        fids = string.Empty;
                    }
                };
                #endregion

                #region 获取数据

                int count = new RefundRegisterService().GetExportRefundCount(stime, etime);

                LogHelper.LogInfo(string.Format("btnRefundEmail_Click new RefundRegisterService().GetExportRefundCount，stime={0}, etime={1},count=={2}", stime, etime, count));
                if (count > 0)
                {
                    int countSheet = 1;
                    if (count > fileRowConut)
                    {
                        countSheet = count % fileRowConut == 0 ? count / fileRowConut : (count / fileRowConut) + 1;
                    }
                    LogHelper.LogInfo(string.Format("btnRefundEmail_Click ，countSheet={0}", countSheet));

                    List<string> successFile = new List<string>();
                    List<string> failFile = new List<string>();
                    Dictionary<string, RefundFile> dicSuccessFile = null;
                    Dictionary<string, RefundFile> dicFailFile = null;

                    string zwMsg = string.Empty;
                    string outErrMsg = string.Empty;

                    string txtFilePath = Server.MapPath("~/") + "PLFile\\{0}_{1}.txt";
                    string txtSuccess = string.Format(txtFilePath, no, "success");
                    string txtFail = string.Format(txtFilePath, no, "fail");

                    LogHelper.LogInfo(string.Format("btnRefundEmail_Click txtFilePath={0},txtSuccess={1},txtFail=={2}", txtFilePath, txtSuccess, txtFail));

                    for (int j = 0; j < countSheet; j++)
                    {
                        DataSet dsa = new RefundRegisterService().GetExportRefundData(stime, etime, j * fileRowConut, fileRowConut);
                        dt = dsa != null ? dsa.Tables[0] : null;

                        LogHelper.LogInfo(string.Format("btnRefundEmail_Click new RefundRegisterService().GetExportRefundData   PageStart={0},PageMax={1},getDataRowsCount = {2}", j * fileRowConut, fileRowConut, dt != null ? dt.Rows.Count : 0));

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            #region 生成附件
                            try
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    string tradeType = row["Ftrade_state"].ToString();

                                    #region 账务检测
                                    bool flag = false;

                                    //2网银订单退款
                                    var refundType = 2;
                                    LogHelper.LogInfo(string.Format(" btnRefundEmail_Click  BatchRefundSingleCheck  请求提交参数  ZWBatchPay_Service.BatchPay_Service.BatchRefundSingleCheck： flag={0},zwMsg={1},tradeType={2},Forder_id={3},Frefund_type={4},Frefund_amount={5},refundType={6}", flag, zwMsg, tradeType, row["Forder_id"].ToString(), row["Frefund_type"].ToString(), row["Frefund_amount"].ToString(), refundType));

                                    try
                                    {
                                        flag = bs.BatchRefundSingleCheck(row["Forder_id"].ToString(), refundType,
                                            Convert.ToInt64(row["Frefund_amount"].ToString()), out zwMsg);

                                        if (!string.IsNullOrEmpty(zwMsg))
                                        {
                                            LogHelper.LogInfo("BatchRefundSingleCheck接口返回：" + zwMsg);
                                        }

                                        LogHelper.LogInfo(" btnRefundEmail_Click  BatchRefundSingleCheck 返回结果  ZWBatchPay_Service.BatchPay_Service.BatchRefundSingleCheck： flag=" + flag + ",zwMsg=" + zwMsg);
                                    }
                                    catch (Exception ex)
                                    {
                                        LogHelper.LogError(" btnRefundEmail_Click  BatchRefundSingleCheck接口调用异常：" + ex.Message + ex.StackTrace);
                                    }
                                    #endregion

                                    if ((tradeType == "2" || tradeType == "3") && flag)  //数据导入后订单的状态可能在其他地方发生改变，需再次判断
                                    {
                                        WriteTxt(txtSuccess, row["Fid"].ToString(), row["Frefund_amount"].ToString(), row["Forder_id"].ToString(), row["Fbuy_acc"].ToString());
                                    }
                                    else
                                    {
                                        WriteTxt(txtFail, row["Fid"].ToString(), row["Frefund_amount"].ToString(), row["Forder_id"].ToString(), row["Fbuy_acc"].ToString());
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LogHelper.LogError(" btnRefundEmail_Click  提交账务退款生成Excel失败！countSheet：" + countSheet + ", " + ex.ToString() + ", stacktrace" + ex.StackTrace);
                                outErrMsg = ex.Message;
                            }
                            finally
                            {
                                GC.Collect();
                            }

                            #endregion
                        }
                    }


                    if (File.Exists(txtSuccess))
                    {
                        dicSuccessFile = WriteXls(txtSuccess, no, "success");
                    }

                    if (File.Exists(txtFail))
                    {
                        dicFailFile = WriteXls(txtFail, no, "fail");
                    }

                    string uin = Session["uid"].ToString();
                    int okTotal = 0;
                    int subOkTotal = 0;
                    int subFailTotal = 0;
                    int failTotal = 0;

                    long okAmount = 0;
                    long subOkAmount = 0;
                    long subFailAmount = 0;
                    long failAmount = 0;

                    string subOkFile = string.Empty;
                    string subFailFile = string.Empty;

                    List<string> subFile = new List<string>();
                    if (dicSuccessFile != null && dicSuccessFile.Count > 0)
                    {
                        foreach (KeyValuePair<string, RefundFile> item in dicSuccessFile)
                        {
                            subFile.Add(item.Key);
                            okTotal += item.Value.listFid.Count;
                            okAmount += item.Value.amount;
                            bool sendFlag = false;
                            #region 提交到账务
                            try
                            {
                                LogHelper.LogInfo(string.Format(" btnRefundEmail_Click  BatchRefundRequest  ZWBatchPay_Service.BatchPay_Service.BatchRefundRequest接口返回：batchNum={0},uin={1},item.Value.remotePath={2},item.Value.listFid.Count={3}, item.Value.fileMd5={4}", item.Value.batchNum, uin, item.Value.remotePath, item.Value.listFid.Count, item.Value.fileMd5));
                                sendFlag = bs.BatchRefundRequest(item.Value.batchNum, 2, false, uin, item.Value.listFid.Count, item.Value.amount, item.Value.remotePath, item.Value.fileMd5, out zwMsg);
                                if (!string.IsNullOrEmpty(zwMsg))
                                    LogHelper.LogInfo(string.Format("BatchRefundRequest  ZWBatchPay_Service.BatchPay_Service.BatchRefundRequest接口返回：sendFlag={0},zwMsg={1}", sendFlag, zwMsg));
                            }
                            catch (Exception ex)
                            {
                                sendFlag = false;
                                LogHelper.LogError(" btnRefundEmail_Click  BatchRefundRequest接口调用异常：" + ex.ToString() + ex.StackTrace);
                            }
                            if (sendFlag)
                            {
                                subOkFile += item.Key + "|";
                                subOkTotal += item.Value.listFid.Count;
                                subOkAmount += item.Value.amount;
                                fidList.AddRange(item.Value.listFid);
                                updateState(fidList, item.Value.batchNum);
                                LogHelper.LogInfo(" btnRefundEmail_Click  BatchRefundRequest：subOkFile" + subOkFile + ",sendFlag = " + sendFlag);
                            }
                            else
                            {
                                subFailFile += item.Key + "|";
                                subFailTotal += item.Value.listFid.Count;
                                subFailAmount += item.Value.amount;
                                LogHelper.LogInfo(" btnRefundEmail_Click  BatchRefundRequest  返回失败：subFailFile" + subFailFile + ",sendFlag = " + sendFlag);
                            }
                            #endregion
                        }
                    }
                    if (dicFailFile != null && dicFailFile.Count > 0)
                    {
                        foreach (KeyValuePair<string, RefundFile> item in dicFailFile)
                        {
                            subFile.Add(item.Key);
                            failTotal += item.Value.listFid.Count;
                            failAmount += item.Value.amount;
                        }

                        LogHelper.LogInfo("btnRefundEmail_Click   ：dicFailFile.Count=" + dicFailFile.Count);
                    }


                    #region 邮件方式
                    // bool sendEmail = true;
                    try
                    {
                        #region 邮件发送
                        var emailMsg = new StringBuilder("<html><head><title></title></head><body>");
                        emailMsg.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:1310px;line-height:22px; margin-left:10px; table-layout:fixed;\" align='left' ID='Table1'><tr><td style='width:1300px;'>");

                        var msg = @"<p style='padding:10px 0;margin:0;'> 提交账务退款汇总结果：<br>&nbsp;&nbsp;&nbsp;&nbsp;
                                            总提交金额：{0}、总提交笔数：{1}、校验失败金额：{2}、校验失败笔数：{3}、
                                            提交失败金额：{4}、提交失败笔数：{5}、成功金额：{6}、成功笔数：{7}、
                                            提交失败文件：{8}、提交成功文件：{9}。
                                            </p>";
                        emailMsg.AppendFormat(msg,
                            classLibrary.setConfig.FenToYuan(okAmount + failAmount), okTotal + failTotal, classLibrary.setConfig.FenToYuan(failAmount), failTotal,
                            classLibrary.setConfig.FenToYuan(subFailAmount), subFailTotal, classLibrary.setConfig.FenToYuan(subOkAmount), subOkTotal,
                            subFailFile.TrimEnd('|'), subOkFile.TrimEnd('|'));

                        emailMsg.Append("</td></tr><tr><td height=\"15\"></td></tr></table></body></html>");

                        string sub = "【网银退款】编号：" + no + "提交结果通知";

                        LogHelper.LogInfo(string.Format(" btnRefundEmail_Click 发送邮件信息！sub={0},emailMsg={1},sendtomail={2}", sub, emailMsg.ToString(), uin));
                        //string toMail = ConfigurationManager.AppSettings["InternetRefundToMail"].ToString();
                        //string ccMail = ConfigurationManager.AppSettings["InternetRefundCcMail"].ToString();
                        CommMailSend.SendInternalMail(uin, "", sub, emailMsg.ToString(), true, subFile.ToArray());

                        WebUtils.ShowMessage(this.Page, "后台处理中，稍后请查收邮件。");
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        // sendEmail = false;
                        LogHelper.LogError(" btnRefundEmail_Click 提交账务退款汇总邮件发送失败！" + ex.Message + ", stacktrace" + ex.StackTrace);

                        WebUtils.ShowMessage(this.Page, "操作发生异常：" + ex.Message.Replace("'", ""));
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(" btnRefundEmail_Click 提交账务退款失败！" + ex.Message + ", stacktrace" + ex.StackTrace);

                WebUtils.ShowMessage(this.Page, "操作发生异常：" + ex.Message.Replace("'", ""));
            }
                #endregion
        }

        private void WriteTxt(string filePath, params string[] fields)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Write))
            {
                StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

                writer.WriteLine(string.Join("|", fields));

                writer.Close();
            }
        }

        private void CreateXls(string filePath)
        {
            Application xlApp = new ApplicationClass();
            Workbooks workbooks = xlApp.Workbooks;
            Workbook workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Worksheet worksheet = (Worksheet)workbook.Worksheets[1]; //取得sheet1
            Range range;

            #region 标题
            range = (Range)worksheet.Cells[1, 1];
            range.ColumnWidth = 45;
            range.NumberFormatLocal = "@";
            range.Font.Size = 15;
            range.Value2 = "交易单号";

            range = (Range)worksheet.Cells[1, 2];
            range.ColumnWidth = 25;
            range.NumberFormatLocal = "@";
            range.Font.Size = 15;
            range.Value2 = "退款金额（元）";

            range = (Range)worksheet.Cells[1, 3];
            range.ColumnWidth = 30;
            range.NumberFormatLocal = "@";
            range.Font.Size = 15;
            range.Value2 = "退款备注";
            #endregion

            workbook.Saved = true;

            workbook.SaveAs(filePath, 56, Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSaveAsAccessMode.xlExclusive,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            range = null;
            workbooks = null;
            workbook = null;
            if (xlApp != null)
            {
                xlApp.Workbooks.Close();
                xlApp.Quit();
                xlApp = null;
            }
        }

        private Dictionary<string, RefundFile> WriteXls(string txtFile, string no, string status)
        {
            Dictionary<string, RefundFile> dicFile = new Dictionary<string, RefundFile>();
            string path = Server.MapPath("~/") + "PLFile\\网银退款{0}_{1}_{2}.xls";
            string successPath = string.Empty;
            int lineCount = 0;
            try
            {
                using (StreamReader sr = new StreamReader(txtFile))
                {
                    while (sr.Peek() >= 0)
                    {
                        sr.ReadLine();
                        lineCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError("private Dictionary<string, RefundFile> WriteXls(string txtFile, string no, string status)！ " + ex.Message + ", stacktrace" + ex.StackTrace);
                throw;
            }

            int fileRowConut = 1000;
            int countSheet = 1;
            if (lineCount > fileRowConut)
            {
                countSheet = lineCount % fileRowConut == 0 ? lineCount / fileRowConut : (lineCount / fileRowConut) + 1;
            }
            using (StreamReader sr = new StreamReader(txtFile, Encoding.UTF8))
            {
                OleDbConnection conn = null;
                OleDbCommand cmd = null;
                List<string> listFid = null;
                long singleAmount = 0;
                string batchNum = string.Empty;

                for (int i = 0; i < countSheet; i++)
                {
                    int idx = 0;
                    listFid = new List<string>();
                    singleAmount = 0;
                    successPath = string.Format(path, no, i + 1, status);
                    batchNum = no + "_" + (i + 1);
                    CreateXls(successPath);
                    string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + successPath + ";" + "Excel 12.0 Xml;HDR=YES;";
                    conn = new OleDbConnection(strConn);
                    conn.Open();
                    cmd = new OleDbCommand();
                    cmd.Connection = conn;

                    LogHelper.LogInfo(" private Dictionary<string, RefundFile> WriteXls(string txtFile, string no, string status)  successPath文件地址： " + successPath);

                    String line;
                    while (idx < fileRowConut && (line = sr.ReadLine()) != null)
                    {
                        string[] arrStr = line.Split('|');
                        listFid.Add(arrStr[0]);
                        singleAmount += long.Parse(arrStr[1]);
                        cmd.CommandText = string.Format(@"
                        INSERT INTO [sheet1$] 
                            (交易单号,退款金额（元）,退款备注) 
                        VALUES 
                            ('{0}','{1}','{2}')",
                                                    arrStr[2], MoneyTransfer.FenToYuan(arrStr[1]), arrStr[3]);
                        cmd.ExecuteNonQuery();
                        idx++;
                    }

                    if (conn != null)
                    {
                        conn.Close();

                        string fileMd5 = Utility.GetMD5HashFromFile(successPath);
                        //文件上传
                        commLib.Entity.UploadFileModel result = null;
                        result = commLib.FPSFileHelper.UploadFile(successPath,"RefundQuery/"+Path.GetFileName(successPath));

                        if (result == null ||string.IsNullOrEmpty(result.url)) {
                            result = commLib.FPSFileHelper.UploadFile(successPath,Guid.NewGuid().ToString().Replace("-","")+Path.GetExtension(successPath));
                        }


                        RefundFile refundFile = new RefundFile();
                        refundFile.fileMd5 = fileMd5;
                        refundFile.listFid = listFid;
                        refundFile.amount = singleAmount;
                        refundFile.remotePath = result.url;
                        refundFile.batchNum = batchNum;
                        dicFile.Add(successPath, refundFile);

                        LogHelper.LogInfo(" commLib.FPSFileHelper.UploadFile  remotePath文件地址： " + result.url);
                    }
                }
                #region
                //                while ((line = sr.ReadLine()) != null)
                //                {
                //                    if (numLine % 1000 == 0)
                //                    {
                //                        if (conn != null)
                //                        {
                //                            conn.Close();

                //                            string fileMd5 = Utility.GetMD5HashFromFile(successPath);
                //                            //文件上传
                //                            var result = commLib.FPSFileHelper.UploadFile(successPath);
                //                            RefundFile refundFile = new RefundFile();
                //                            refundFile.fileMd5 = fileMd5;
                //                            refundFile.listFid = listFid;
                //                            refundFile.amount = singleAmount;
                //                            refundFile.remotePath = result.url;
                //                            dicFile.Add(successPath, refundFile);
                //                        }
                //                        listFid = new List<string>();
                //                        singleAmount = 0;
                //                        successPath = string.Format(path, no, (numLine % 1000) + 1, status);
                //                        CreateXls(successPath);
                //                        string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + successPath + ";" + "Excel 12.0 Xml;HDR=YES;";
                //                        conn = new OleDbConnection(strConn);
                //                        conn.Open();
                //                        cmd = new OleDbCommand();
                //                        cmd.Connection = conn;
                //                    }
                //                    string[] arrStr = line.Split('|');
                //                    listFid.Add(arrStr[0]);
                //                    singleAmount += long.Parse(arrStr[1]);
                //                    cmd.CommandText = string.Format(@"
                //                    INSERT INTO [sheet1$] 
                //                        (交易单号,退款金额（元）,退款备注) 
                //                    VALUES 
                //                        ('{0}','{1}','{2}')", 
                //                                            arrStr[2], arrStr[3], arrStr[4]);
                //                    cmd.ExecuteNonQuery();
                //                    numLine++;
                //                }
                #endregion
            }
            return dicFile;
        }

        private void DelRefund(string fid)
        {
            if (!classLibrary.ClassLib.ValidateRight("InternetBankRefund", this))
            {
                //权限判断
                WebUtils.ShowMessage(this.Page, "没有权限！");
                return;
            }
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                qs.DelRefundInfo(fid);

                //权限判断
                WebUtils.ShowMessage(this.Page, "删除成功");
            }
            catch (Exception ex)
            {
                LogHelper.LogError(" private void DelRefund(string fid)！ " + ex.Message + ", stacktrace" + ex.StackTrace);
                WebUtils.ShowMessage(this.Page, ex.Message);
            }
        }
    }

    public class RefundFile
    {
        public List<string> listFid { get; set; }

        public string fileMd5 { get; set; }

        public long amount { get; set; }

        public string remotePath { get; set; }

        public string batchNum { get; set; }
    }
}