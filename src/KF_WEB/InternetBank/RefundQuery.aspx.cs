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

namespace TENCENT.OSS.CFT.KF.KF_Web.InternetBank
{
    /// <summary>
    /// RefundQuery 的摘要说明。
    /// </summary>
    public partial class RefundQuery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()");
            ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");

            try
            {
                Label1.Text = Session["uid"].ToString();
                if (!classLibrary.ClassLib.ValidateRight("InternetBankRefund", this))
                {
                    btnSubRefund.Visible = false;
                }

                if (!IsPostBack)
                {
                    TextBoxBeginDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
                    TextBoxEndDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");

                    setConfig.GetAllTypeList(ddlTradeState, "PAY_STATE");
                    ddlTradeState.Items.Insert(0, new ListItem("全部", "0"));
                    //退款登记模版 v_yqyqguo
                    DownloadTemplate.NavigateUrl = System.Configuration.ConfigurationManager.AppSettings["GetImageFromKf2Url"].ToString() + DownloadTemplate.NavigateUrl;
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
                int count = GetCount();
                Label9.Text = count.ToString();

                BindData(1);
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
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

        private int GetCount()
        {
            DateTime begindate = DateTime.Parse(TextBoxBeginDate.Text);
            string stime = begindate.ToString("yyyy-MM-dd 00:00:00");
            DateTime enddate = DateTime.Parse(TextBoxEndDate.Text);
            string etime = enddate.ToString("yyyy-MM-dd 23:59:59");

            string listid = listId.Text.Trim();
            string cftorderid = cftOrderId.Text.Trim();
            int rf_type = int.Parse(ddlRefundType.SelectedValue);
            int rf_status = int.Parse(ddlRefundStatus.SelectedValue);

            string s_trade_state = ddlTradeState.SelectedValue;


            //Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            //return qs.QueryRefundCount(listid, cftorderid, stime, etime, rf_type, rf_status, s_trade_state);
            return new RefundRegisterService().QueryRefundRegisterCount(listid, cftorderid, stime, etime, rf_type, rf_status, s_trade_state);
        }

        private void BindData(int index)
        {
            DateTime begindate = DateTime.Parse(TextBoxBeginDate.Text);
            string stime = begindate.ToString("yyyy-MM-dd 00:00:00");
            DateTime enddate = DateTime.Parse(TextBoxEndDate.Text);
            string etime = enddate.ToString("yyyy-MM-dd 23:59:59");

            string listid = listId.Text.Trim();
            string cftorderid = cftOrderId.Text.Trim();
            int rf_type = int.Parse(ddlRefundType.SelectedValue);
            int rf_status = int.Parse(ddlRefundStatus.SelectedValue); //提交退款状态

            string s_trade_state = ddlTradeState.SelectedValue;


            pager.RecordCount = 1000;
            pager.CurrentPageIndex = index;


            int max = pager.PageSize;
            int start = max * (index - 1);

            //Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            //DataSet ds = qs.QueryRefundInfo(listid, cftorderid, stime, etime, rf_type, rf_status,s_trade_state, start, max);
            DataSet ds = new RefundRegisterService().QueryRefundRegisterList(listid, cftorderid, stime, etime, rf_type, rf_status, s_trade_state, start, max);

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
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }
        }
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
            int rf_type = int.Parse(ddlRefundType.SelectedValue);
            int rf_status = int.Parse(ddlRefundStatus.SelectedValue);
            string s_trade_state = ddlTradeState.SelectedValue;

            int count = GetCount();
            //Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            //DataSet ds = qs.QueryRefundInfo(listid, cftorderid, stime, etime, rf_type, rf_status,s_trade_state, 1, count);
            DataSet ds = new RefundRegisterService().QueryRefundRegisterList(listid, cftorderid, stime, etime, rf_type, rf_status, s_trade_state, 0, count);
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

                /*
                StringWriter sw = new StringWriter();
                sw.WriteLine("财付通订单号\t订单编码\t交易状态\t交易金额\t买家账号\t交易说明\t登记人\t物品回收人\t退款类型\tSAM工单号\t创建时间\t提交退款");

                foreach (DataRow dr in dt.Rows)
                {
                    sw.WriteLine("=\"" + dr["Forder_id"].ToString() + "\"\t=\"" + dr["Fcoding"].ToString() + "\"\t=\"" + dr["Ftrade_state_str"]
                        + "\"\t" + dr["Famount_str"] + "\t=\"" + dr["Fbuy_acc"] + "\"\t=\"" + dr["Ftrade_desc"] + "\"\t=\"" + dr["Fsubmit_user"] + "\"\t=\"" + dr["Frecycle_user"]
                    + "\"\t=\"" + dr["Frefund_type_str"] + "\"\t=\"" + dr["Fsam_no"] + "\"\t=\"" + dr["Fcreate_time"]
                     + "\"\t=\"" + dr["Fsubmit_refund_str"] + "\"");

                }
                sw.WriteLine("总条数：=" + count + "\t总金额：=" + classLibrary.setConfig.FenToYuan(amount));
                sw.Close();
                string f_name = "退款登记";
                f_name = System.Web.HttpUtility.UrlEncode(System.Text.Encoding.UTF8.GetBytes(f_name));
                Response.AddHeader("Content-Disposition", "attachment; filename=" + f_name+".xls");
                Response.ContentType = "application/ms-excel";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                Response.Write(sw);
                Response.End();
                */
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

                    range = (Range)worksheet.Cells[1, 1];
                    range.ColumnWidth = 40;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "财付通订单号";

                    range = (Range)worksheet.Cells[1, 2];
                    range.ColumnWidth = 25;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "订单编码";

                    range = (Range)worksheet.Cells[1, 3];
                    range.ColumnWidth = 25;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "交易状态";

                    range = (Range)worksheet.Cells[1, 4];
                    range.ColumnWidth = 25;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "交易金额";

                    range = (Range)worksheet.Cells[1, 5];
                    range.ColumnWidth = 25;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "买家账号";

                    range = (Range)worksheet.Cells[1, 6];
                    range.ColumnWidth = 30;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "交易说明";

                    range = (Range)worksheet.Cells[1, 7];
                    range.ColumnWidth = 25;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "登记人";

                    range = (Range)worksheet.Cells[1, 8];
                    range.ColumnWidth = 25;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "物品回收人";

                    range = (Range)worksheet.Cells[1, 9];
                    range.ColumnWidth = 15;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "退款类型";

                    range = (Range)worksheet.Cells[1, 10];
                    range.ColumnWidth = 30;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "SAM工单号";

                    range = (Range)worksheet.Cells[1, 11];
                    range.ColumnWidth = 20;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "创建时间";

                    range = (Range)worksheet.Cells[1, 12];
                    range.ColumnWidth = 15;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "提交退款";

                    range = (Range)worksheet.Cells[1, 13];
                    range.ColumnWidth = 25;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "退款金额";

                    int total = 0;
                    int amount = 0; //总金额

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        total++;
                        amount += int.Parse(dr["Famount"].ToString());

                        range = (Range)worksheet.Cells[total + 1, 1]; //从第二行开始
                        range.ColumnWidth = 40;
                        range.NumberFormatLocal = "@";
                        range.Font.Size = 10;
                        range.Value2 = dr["Forder_id"].ToString();

                        range = (Range)worksheet.Cells[total + 1, 2];
                        range.ColumnWidth = 25;
                        range.NumberFormatLocal = "@";
                        range.Font.Size = 10;
                        range.Value2 = dr["Fcoding"].ToString();

                        range = (Range)worksheet.Cells[total + 1, 3];
                        range.ColumnWidth = 25;
                        range.Font.Size = 10;
                        range.Value2 = dr["Ftrade_state_str"].ToString();

                        range = (Range)worksheet.Cells[total + 1, 4];
                        range.ColumnWidth = 25;
                        range.Font.Size = 10;
                        range.Value2 = dr["Famount_str"].ToString();

                        range = (Range)worksheet.Cells[total + 1, 5];
                        range.ColumnWidth = 25;
                        range.Font.Size = 10;
                        range.Value2 = dr["Fbuy_acc"].ToString();

                        range = (Range)worksheet.Cells[total + 1, 6];
                        range.ColumnWidth = 30;
                        range.Font.Size = 10;
                        range.Value2 = dr["Ftrade_desc"].ToString();

                        range = (Range)worksheet.Cells[total + 1, 7];
                        range.ColumnWidth = 25;
                        range.Font.Size = 10;
                        range.Value2 = dr["Fsubmit_user"].ToString();

                        range = (Range)worksheet.Cells[total + 1, 8];
                        range.ColumnWidth = 25;
                        range.Font.Size = 10;
                        range.Value2 = dr["Frecycle_user"].ToString();

                        range = (Range)worksheet.Cells[total + 1, 9];
                        range.ColumnWidth = 15;
                        range.Font.Size = 10;
                        range.Value2 = dr["Frefund_type_str"].ToString();

                        range = (Range)worksheet.Cells[total + 1, 10];
                        range.ColumnWidth = 30;
                        range.Font.Size = 10;
                        range.Value2 = dr["Fsam_no"].ToString();

                        range = (Range)worksheet.Cells[total + 1, 11];
                        range.ColumnWidth = 20;
                        range.Font.Size = 10;
                        range.Value2 = dr["Fcreate_time"].ToString();

                        range = (Range)worksheet.Cells[total + 1, 12];
                        range.ColumnWidth = 15;
                        range.Font.Size = 10;
                        range.Value2 = dr["Fsubmit_refund_str"].ToString();

                        range = (Range)worksheet.Cells[total + 1, 13];
                        range.ColumnWidth = 25;
                        range.Font.Size = 10;
                        range.Value2 = dr["Frefund_amountStr"].ToString();
                    }

                    workbook.Saved = true;
                    //workbook.SaveCopyAs(path);  //2007版本
                    workbook.SaveAs(path, 56, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                    range = null;

                    workbooks = null;
                    workbook = null;

                    if (xlApp != null)
                    {
                        xlApp.Workbooks.Close();
                        xlApp.Quit();
                        xlApp = null;
                    }

                    string[] fileAtta = { path };

                    if (total > 0)
                    {
                        //mail
                        CommMailSend.SendInternalMail(uin, "", "退款登记导出", "", false, fileAtta);
                    }

                    #endregion
                }
                finally
                {
                    GC.Collect();
                }
            }
        }

        private bool isValidAmount(string ListID, string  Amount, out string msg)
        {
            //绑定交易资料信息
            msg = "";
            try
            {
                double FRefundAmount = Convert.ToDouble(Amount);
                Query_Service.Query_Service myService = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                DateTime beginTime = DateTime.Parse(ConfigurationManager.AppSettings["sBeginTime"].ToString());
                DateTime endTime = DateTime.Parse(ConfigurationManager.AppSettings["sEndTime"].ToString());
                DataSet ds = new DataSet();
                ds = myService.GetPayList(ListID, 4, beginTime, endTime, 1, 2);
                double _FRefundAmount = Convert.ToDouble(classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Ffact"].ToString()));

                if (FRefundAmount > 0 && FRefundAmount <= _FRefundAmount)
                {
                    return true;
                }
                else
                {
                    msg = "退款金额超出范围，退款金额：" + FRefundAmount + ",订单金额:" + _FRefundAmount;
                    return false;
                }
            }
            catch (Exception e)
            {
                msg ="查询订单金额失败："+ e.Message;
                return false;
            }
        }
        private bool isValidState(string listID, out string msg) 
        {
            msg = "";
            try
            {
                Query_Service.Query_Service qs = new Query_Service.Query_Service();
                DataSet dsState = qs.GetQueryListDetail(listID);
                string state = "";
                if (dsState != null && dsState.Tables.Count > 0 && dsState.Tables[0].Rows.Count > 0)
                {
                    dsState.Tables[0].Columns.Add("Ftrade_stateName");
                    classLibrary.setConfig.GetColumnValueFromDic(dsState.Tables[0], "Ftrade_state", "Ftrade_stateName", "PAY_STATE");
                    state = dsState.Tables[0].Rows[0]["Ftrade_stateName"].ToString();
                }
                if (state.Contains("支付成功"))
                {
                    return true;
                }
                else 
                {
                    msg = "该订单状态不允许录入，订单状态：" + state;  
                    return false;
                }
            }
            catch (Exception e)
            {
                msg = "判断订单状态失败：" + e.Message;
                return false;
            }
        }

        public void btnUpload_Click(object sender, System.EventArgs e)
        {
            if (!File1.HasFile)
            {
                WebUtils.ShowMessage(this.Page, "请选择上传文件！");
                return;
            }
            if (Path.GetExtension(File1.FileName).ToLower() == ".xls")
            {
                int succ = 0, fail = 0;
                int refundAmount = 0; //退款金额
                string errMsg = "";

                string path = Server.MapPath("~/") + "PLFile" + "\\refund.xls";
                File1.PostedFile.SaveAs(path);

                DataSet res_ds = PublicRes.readXls(path);
                System.Data.DataTable res_dt = res_ds.Tables[0];
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
                        continue;
                    }
                  
                    string msg;
                    //订单状态为“支付成功/等待卖家发货”的订单才允许录入，非此状态的订单限制录入.
                    if (!isValidState(r1, out  msg))
                    {
                        fail++;
                        errMsg += "第" + (i + 1) + "行:" + msg + ".";
                        continue;
                    }
                    //批量导入模版中金额需限制为:0<退款金额≦订单金额。
                    if (!isValidAmount(r1, r6, out msg))
                    {
                        fail++;
                        errMsg += "第" + (i + 1) + "行:" + msg + ".";
                        continue;
                    }
                    //组装
                    Query_Service.RefundInfoClass cb = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.RefundInfoClass();
                    cb.FOrderId = r1.Trim();
                    cb.FRefund_type = int.Parse(r2);
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
                    }
                }

                //展示成功多少，失败多少，错误信息
                Table3.Visible = true;
                Table2.Visible = false;

                lbTotal.Text = iRows.ToString();
                lbSucc.Text = succ.ToString();
                lbFail.Text = fail.ToString();
                lbError.Text = errMsg;
            }
            else
            {
                WebUtils.ShowMessage(this.Page, "文件格式不正确，请选择xls格式文件上传。");
                return;
            }
        }

        public void btnRefund_Click(object sender, System.EventArgs e)
        {

            if (!classLibrary.ClassLib.ValidateRight("InternetBankRefund", this))
            {
                //权限判断
                WebUtils.ShowMessage(this.Page, "没有权限！");
                return;
            }

            DateTime begindate = DateTime.Parse(TextBoxBeginDate.Text);
            string stime = begindate.ToString("yyyy-MM-dd 00:00:00");
            DateTime enddate = DateTime.Parse(TextBoxEndDate.Text);
            string etime = enddate.ToString("yyyy-MM-dd 23:59:59");



            //先查询退款记录
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds = qs.QueryRefundInfo("", "", stime, etime, 0, 2, "2", 1, 1000); //支付成功，未提交

            FQuery_Service.Query_Service fs = new TENCENT.OSS.CFT.KF.KF_Web.FQuery_Service.Query_Service();
            FQuery_Service.Finance_Header Ffh = classLibrary.setConfig.FsetFH(this);
            fs.Finance_HeaderValue = Ffh;
            string msg = "";

            bool flag = false; //发起退款成功标志
            int total = 0, succ = 0, fail = 0;
            //string errMsg = "";
            var errMsg = new StringBuilder("");

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    msg = "";
                    if (dr["FTrade_Type"].ToString() == "2" || dr["FTrade_Type"].ToString() == "3")
                    {
                        total++;
                        //发请求
                        try
                        {
                            flag = fs.StartTraderRefund_KF(dr["FTrade_Type"].ToString(), dr["Fbuy_acc"].ToString(), dr["Forder_id"].ToString(), long.Parse(dr["Famount"].ToString()), "客服发起退款", "", out msg);
                            //如果为true，则更新状态为已提交
                            if (flag)
                            {
                                succ++;
                                qs.UpdateSubmitRefundState(dr["Fid"].ToString(), 1);
                            }
                            else
                            {
                                fail++;
                                errMsg.AppendLine(msg);
                            }
                        }
                        catch (SoapException ser)
                        {
                            //fail++;
                            errMsg.AppendLine(ser.Message);
                        }
                    }
                }
            }
            //再查一次失效的记录，防止订单状态发生改变
            ds = qs.QueryRefundInfo("", "", stime, etime, 0, 3, "2", 1, 1000); //支付成功，失效的
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    msg = "";
                    if (dr["FTrade_Type"].ToString() == "2" || dr["FTrade_Type"].ToString() == "3")
                    {
                        total++;
                        //发请求
                        try
                        {
                            flag = fs.StartTraderRefund_KF(dr["FTrade_Type"].ToString(), dr["Fbuy_acc"].ToString(), dr["Forder_id"].ToString(), long.Parse(dr["Famount"].ToString()), "客服发起退款", "", out msg);
                            //如果为true，则更新状态为已提交
                            if (flag)
                            {
                                succ++;
                                qs.UpdateSubmitRefundState(dr["Fid"].ToString(), 1);
                            }
                            else
                            {
                                fail++;
                                errMsg.AppendLine(msg);
                            }
                        }
                        catch (SoapException ser)
                        {
                            //fail++;
                            errMsg.AppendLine(ser.Message);
                        }
                    }
                }
            }

            Table3.Visible = true;
            Table2.Visible = false;

            lbTotal.Text = total.ToString();
            lbSucc.Text = succ.ToString();
            lbFail.Text = fail.ToString();
            lbError.Text = errMsg.ToString();

        }

        public void btnRefundEmail_Click(object sender, System.EventArgs e)
        {

            if (!classLibrary.ClassLib.ValidateRight("InternetBankRefund", this))
            {
                //权限判断
                WebUtils.ShowMessage(this.Page, "没有权限！");
                return;
            }

            Thread t = new Thread(RefundEmailMethod);
            t.Start();
            WebUtils.ShowMessage(this.Page, "后台处理中，稍后请查收邮件。");
        }

        private void RefundEmailMethod()
        {
            try
            {
                DateTime begindate = DateTime.Parse(TextBoxBeginDate.Text);
                string stime = begindate.ToString("yyyy-MM-dd 00:00:00");
                DateTime enddate = DateTime.Parse(TextBoxEndDate.Text);
                string etime = enddate.ToString("yyyy-MM-dd 23:59:59");


                //先查询退款记录
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                //DataSet ds = qs.QueryRefundInfo("", "", stime, etime, 0, 2, "2", 1, 5000); //支付成功，未提交
                DataSet ds = new RefundRegisterService().QueryRefundRegisterList("", "", stime, etime, 0, 2, "2", 0, 5000);

                #region 生成excel文件
                int total = 0; //总笔数
                var emailMsg = new StringBuilder("<html><head><title></title></head><body><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:1310px;line-height:22px; margin-left:10px; table-layout:fixed;\" align='left' ID='Table1'><tr><td style='width:1300px;'><p style='padding:10px 0;margin:0;'> ");
                string cs = "亲爱的财务同事：<br>&nbsp;&nbsp;&nbsp;&nbsp;以下为本期网银退款数据，共计{0}笔，{1}。请协助办理批量退款处理。谢谢！</p>"; //2

                var apptmp1 = new StringBuilder(); //3
                apptmp1.Append("<p style='padding:10px 0; margin:0;'><table width='100%' border='1' align='center' cellpadding=\"0\" cellspacing=\"0\" ID=\"Table6\"><tr><td style=\"width:110px;\">财付通单号</td><td style=\"width:55px;\">订单编码</td>");
                apptmp1.Append("<td style=\"width:65px;\">交易状态</td><td style=\"width:50px;\">交易金额</td><td style=\"width:55px\">买家账号</td><td style=\"width:120px\">交易说明</td><td style=\"width:55px\">登记人</td>");
                apptmp1.Append("<td style=\"width:55px\">物品回收人</td><td style=\"width:50px\">退款类型</td><td style=\"width:100px\">SAM工单号</td><td style=\"width:80px\">创建时间</td><td style=\"width:45px\">提交退款</td></tr>");

                int amount = 0; //总金额
                DateTime d = DateTime.Now;
                string no = d.ToString("yyyyMMddHHmmssffff");

                string path = Server.MapPath("~/") + "PLFile" + "\\网银退款" + no + ".xls"; //附件
                //StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default);
                //string csv_cont = "退款订单号\t退款金额(元)\t财付通账号";
                //sw.WriteLine(csv_cont);

                Application xlApp = new ApplicationClass();
                Workbooks workbooks = xlApp.Workbooks;
                Workbook workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                Worksheet worksheet = (Worksheet)workbook.Worksheets[1];//取得sheet1

                Range range;

                range = (Range)worksheet.Cells[1, 1];
                range.ColumnWidth = 45;
                range.NumberFormatLocal = "@";
                range.Font.Size = 15;
                range.Value2 = "交易单号";

                range = (Range)worksheet.Cells[1, 2];
                range.ColumnWidth = 25;
                range.NumberFormatLocal = "@";
                range.Font.Size = 15;
                range.Value2 = "退款金额(元)";

                range = (Range)worksheet.Cells[1, 3];
                range.ColumnWidth = 30;
                range.NumberFormatLocal = "@";
                range.Font.Size = 15;
                range.Value2 = "退款备注";

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
                    //Hashtable ht2 = new Hashtable();
                    //ht2.Add("1", "已提交");
                    //ht2.Add("2", "未提交");
                    //ht2.Add("3", "失效");

                    //先转义
                    classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0], "Ftrade_state_new", "Ftrade_state_str", "PAY_STATE");
                    classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Famount", "Famount_str");
                    classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Frefund_amount", "Frefund_amountStr");
                    classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Frefund_type", "Frefund_type_str", ht1);
                    //classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fsubmit_refund", "Fsubmit_refund_str", ht2);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["FTrade_Type"].ToString() == "2" || dr["FTrade_Type"].ToString() == "3")
                        {
                            amount += int.Parse(dr["Frefund_amount"].ToString());
                            total++;

                            //csv_cont = "" + dr["Forder_id"].ToString() + "\t" + MoneyTransfer.FenToYuan(dr["Famount"].ToString()) + "\t" + dr["Fbuy_acc"].ToString();
                            //sw.WriteLine(csv_cont);

                            range = (Range)worksheet.Cells[total + 1, 1]; //从第二行开始
                            range.ColumnWidth = 45;
                            range.NumberFormatLocal = "@";
                            range.Font.Size = 10;
                            range.Value2 = dr["Forder_id"].ToString();

                            range = (Range)worksheet.Cells[total + 1, 2];
                            range.ColumnWidth = 25;
                            range.NumberFormatLocal = "@";
                            range.Font.Size = 10;
                            range.Value2 = MoneyTransfer.FenToYuan(dr["Frefund_amount"].ToString());

                            range = (Range)worksheet.Cells[total + 1, 3];
                            range.ColumnWidth = 30;
                            //range.NumberFormatLocal = "@";
                            range.Font.Size = 10;
                            range.Value2 = dr["Fbuy_acc"].ToString();

                            if (total < 2)
                            { //邮件内容中只展示100条
                                apptmp1.Append("<tr>");

                                //组装数据
                                apptmp1.Append("<td>");
                                apptmp1.Append(dr["Forder_id"].ToString());
                                apptmp1.Append("</td>");
                                apptmp1.Append("<td>");
                                apptmp1.Append(dr["Fcoding"].ToString());
                                apptmp1.Append("</td>");
                                apptmp1.Append("<td>");
                                apptmp1.Append(dr["Ftrade_state_str"].ToString());
                                apptmp1.Append("</td>");
                                apptmp1.Append("<td>");
                                apptmp1.Append(dr["Famount_str"].ToString());
                                apptmp1.Append("</td>");
                                apptmp1.Append("<td>");
                                apptmp1.Append(dr["Fbuy_acc"].ToString());
                                apptmp1.Append("</td>");
                                apptmp1.Append("<td>");
                                apptmp1.Append(dr["Ftrade_desc"].ToString());
                                apptmp1.Append("</td>");
                                apptmp1.Append("<td>");
                                apptmp1.Append(dr["Fsubmit_user"].ToString());
                                apptmp1.Append("</td>");
                                apptmp1.Append("<td>");
                                apptmp1.Append(dr["Frecycle_user"].ToString());
                                apptmp1.Append("</td>");
                                apptmp1.Append("<td>");
                                apptmp1.Append(dr["Frefund_type_str"].ToString());
                                apptmp1.Append("</td>");
                                apptmp1.Append("<td>");
                                apptmp1.Append(dr["Fsam_no"].ToString());
                                apptmp1.Append("</td>");
                                apptmp1.Append("<td>");
                                apptmp1.Append(dr["Fcreate_time"].ToString());
                                apptmp1.Append("</td>");
                                apptmp1.Append("<td>");
                                apptmp1.Append("已提交");
                                apptmp1.Append("</td>");

                                apptmp1.Append("</tr>");
                            }
                        }
                    }
                }
                //更新提交状态为1=已提交
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["FTrade_Type"].ToString() == "2" || dr["FTrade_Type"].ToString() == "3")
                        {
                            qs.UpdateSubmitRefundState(dr["Fid"].ToString(), 1);
                        }
                    }
                }

                if (total < 5000)
                {
                    //总笔数控制在5000内


                    //再查一次失效的记录，防止订单状态发生改变
                    //ds = qs.QueryRefundInfo("", "", stime, etime, 0, 3, "2", 1, 5000 - total); //支付成功，失效的
                    ds = new RefundRegisterService().QueryRefundRegisterList("", "", stime, etime, 0, 3, "2", 0, 5000 - total);
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
                        //Hashtable ht2 = new Hashtable();
                        //ht2.Add("1", "已提交");
                        //ht2.Add("2", "未提交");
                        //ht2.Add("3", "失效");

                        //先转义
                        classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0], "Ftrade_state_new", "Ftrade_state_str", "PAY_STATE");
                        classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Famount", "Famount_str");
                        classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Frefund_amount", "Frefund_amountStr");
                        classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Frefund_type", "Frefund_type_str", ht1);
                        //classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fsubmit_refund", "Fsubmit_refund_str", ht2);

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (dr["FTrade_Type"].ToString() == "2" || dr["FTrade_Type"].ToString() == "3")
                            {
                                amount += int.Parse(dr["Frefund_amount"].ToString());
                                total++;

                                //csv_cont = "=\"" + dr["Forder_id"].ToString() + "\"\t" + MoneyTransfer.FenToYuan(dr["Famount"].ToString()) + "\t=\"" + dr["Fbuy_acc"].ToString() + "\"";
                                //sw.WriteLine(csv_cont);
                                range = (Range)worksheet.Cells[total + 1, 1]; //从第二行开始
                                range.ColumnWidth = 45;
                                range.NumberFormatLocal = "@";
                                range.Font.Size = 10;
                                range.Value2 = dr["Forder_id"].ToString();

                                range = (Range)worksheet.Cells[total + 1, 2];
                                range.ColumnWidth = 25;
                                range.NumberFormatLocal = "@";
                                range.Font.Size = 10;
                                range.Value2 = MoneyTransfer.FenToYuan(dr["Frefund_amount"].ToString());

                                range = (Range)worksheet.Cells[total + 1, 3];
                                range.ColumnWidth = 30;
                                //range.NumberFormatLocal = "@";
                                range.Font.Size = 10;
                                range.Value2 = dr["Fbuy_acc"].ToString();

                                if (total < 2)
                                {
                                    apptmp1.Append("<tr>");

                                    //组装数据
                                    apptmp1.Append("<td>");
                                    apptmp1.Append(dr["Forder_id"].ToString());
                                    apptmp1.Append("</td>");
                                    apptmp1.Append("<td>");
                                    apptmp1.Append(dr["Fcoding"].ToString());
                                    apptmp1.Append("</td>");
                                    apptmp1.Append("<td>");
                                    apptmp1.Append(dr["Ftrade_state_str"].ToString());
                                    apptmp1.Append("</td>");
                                    apptmp1.Append("<td>");
                                    apptmp1.Append(dr["Famount_str"].ToString());
                                    apptmp1.Append("</td>");
                                    apptmp1.Append("<td>");
                                    apptmp1.Append(dr["Fbuy_acc"].ToString());
                                    apptmp1.Append("</td>");
                                    apptmp1.Append("<td>");
                                    apptmp1.Append(dr["Ftrade_desc"].ToString());
                                    apptmp1.Append("</td>");
                                    apptmp1.Append("<td>");
                                    apptmp1.Append(dr["Fsubmit_user"].ToString());
                                    apptmp1.Append("</td>");
                                    apptmp1.Append("<td>");
                                    apptmp1.Append(dr["Frecycle_user"].ToString());
                                    apptmp1.Append("</td>");
                                    apptmp1.Append("<td>");
                                    apptmp1.Append(dr["Frefund_type_str"].ToString());
                                    apptmp1.Append("</td>");
                                    apptmp1.Append("<td>");
                                    apptmp1.Append(dr["Fsam_no"].ToString());
                                    apptmp1.Append("</td>");
                                    apptmp1.Append("<td>");
                                    apptmp1.Append(dr["Fcreate_time"].ToString());
                                    apptmp1.Append("</td>");
                                    apptmp1.Append("<td>");
                                    apptmp1.Append("已提交");
                                    apptmp1.Append("</td>");

                                    apptmp1.Append("</tr>");
                                }
                            }
                        }
                    }
                }
                workbook.Saved = true;
                //workbook.SaveCopyAs(path);  //2007版本
                workbook.SaveAs(path, 56, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                range = null;

                workbooks = null;
                workbook = null;

                if (xlApp != null)
                {
                    xlApp.Workbooks.Close();
                    xlApp.Quit();
                    xlApp = null;
                }
                #endregion

                //更新提交状态为1=已提交
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["FTrade_Type"].ToString() == "2" || dr["FTrade_Type"].ToString() == "3")
                        {
                            qs.UpdateSubmitRefundState(dr["Fid"].ToString(), 1);
                        }
                    }
                }

                cs = string.Format(cs, total, classLibrary.setConfig.FenToYuan(amount));

                emailMsg.Append(cs);
                //emailMsg.Append(apptmp1);
                emailMsg.Append("</table></p></td></tr><tr><td height=\"15\"></td></tr></table></body></html>");

                string[] fileAtta = { path };

                string sub = "【网银退款】编号：" + no;
                if (total > 0)
                {
                    string toMail = ConfigurationManager.AppSettings["InternetRefundToMail"].ToString();
                    string ccMail = ConfigurationManager.AppSettings["InternetRefundCcMail"].ToString();
                    CommMailSend.SendInternalMail(toMail, ccMail, sub, emailMsg.ToString(), true, fileAtta);
                }
            }

            finally
            {
                GC.Collect();
            }
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
            }
            catch (Exception e)
            {
                WebUtils.ShowMessage(this.Page, e.Message);
            }
        }
    }
}