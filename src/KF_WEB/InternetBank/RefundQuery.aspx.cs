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
    /// RefundQuery ��ժҪ˵����
    /// </summary>
    public partial class RefundQuery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��
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
                    TextBoxBeginDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");
                    TextBoxEndDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");

                    setConfig.GetAllTypeList(ddlTradeState, "PAY_STATE");
                    ddlTradeState.Items.Insert(0, new ListItem("ȫ��", "0"));
                    //�˿�Ǽ�ģ�� v_yqyqguo
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

        #region Web ������������ɵĴ���
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: �õ����� ASP.NET Web ���������������ġ�
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
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
                throw new Exception("������������");
            }

            if (begindate.CompareTo(enddate) > 0)
            {
                throw new Exception("��ֹ����С����ʼ���ڣ����������룡");
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
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
            }
        }

        public void btnNew_Click(object sender, System.EventArgs e)
        {
            //����

            Response.Redirect("RefundInfo.aspx?listid=");
        }


        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string fid = e.Item.Cells[0].Text.Trim(); //ID

            switch (e.CommandName)
            {
                case "CHANGE": //�༭
                    Response.Redirect("RefundInfo.aspx?listid=" + fid);
                    break;
                case "DEL": //ɾ��
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
                lb.Attributes["onClick"] = "return confirm('ȷ��Ҫִ�С��༭��������');";
            }
            object obj2 = e.Item.Cells[13].FindControl("lbDel");
            if (obj2 != null)
            {
                LinkButton lb2 = (LinkButton)obj2;
                lb2.Attributes["onClick"] = "return confirm('ȷ��Ҫִ�С�ɾ����������');";
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
            int rf_status = int.Parse(ddlRefundStatus.SelectedValue); //�ύ�˿�״̬

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
                ht1.Add("1", "Ͷ���˿�");
                ht1.Add("2", "Ͷ���˿�");
                ht1.Add("3", "Ͷ���˿�");
                ht1.Add("4", "Ͷ���˿�");
                ht1.Add("5", "Ͷ���˿�");
                ht1.Add("10", "Ͷ���˿�"); //�������˿�����Ϊ1\2\3\4\5�Ķ�����ΪͶ���˿�
                ht1.Add("11", "����ʧ��");
                Hashtable ht2 = new Hashtable();
                ht2.Add("1", "���ύ");
                ht2.Add("2", "δ�ύ");
                ht2.Add("3", "ʧЧ");


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
                WebUtils.ShowMessage(this.Page, "��̨�����У��Ժ�������ʼ���");
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
            }
        }
        private void ExportData()
        {
            string uin = Session["uid"].ToString();
            if (string.IsNullOrEmpty(uin))
            {
                throw new Exception("�˺Ų���Ϊ�գ������µ�¼��");
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
                ht1.Add("1", "Ͷ���˿�");
                ht1.Add("2", "Ͷ���˿�");
                ht1.Add("3", "Ͷ���˿�");
                ht1.Add("4", "Ͷ���˿�");
                ht1.Add("5", "Ͷ���˿�");
                ht1.Add("10", "Ͷ���˿�"); //�������˿�����Ϊ1\2\3\4\5�Ķ�����ΪͶ���˿�
                ht1.Add("11", "����ʧ��");
                Hashtable ht2 = new Hashtable();
                ht2.Add("1", "���ύ");
                ht2.Add("2", "δ�ύ");
                ht2.Add("3", "ʧЧ");

                classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0], "Ftrade_state_new", "Ftrade_state_str", "PAY_STATE");

                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Famount", "Famount_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Frefund_amount", "Frefund_amountStr");
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Frefund_type", "Frefund_type_str", ht1);
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fsubmit_refund", "Fsubmit_refund_str", ht2);

                /*
                StringWriter sw = new StringWriter();
                sw.WriteLine("�Ƹ�ͨ������\t��������\t����״̬\t���׽��\t����˺�\t����˵��\t�Ǽ���\t��Ʒ������\t�˿�����\tSAM������\t����ʱ��\t�ύ�˿�");

                foreach (DataRow dr in dt.Rows)
                {
                    sw.WriteLine("=\"" + dr["Forder_id"].ToString() + "\"\t=\"" + dr["Fcoding"].ToString() + "\"\t=\"" + dr["Ftrade_state_str"]
                        + "\"\t" + dr["Famount_str"] + "\t=\"" + dr["Fbuy_acc"] + "\"\t=\"" + dr["Ftrade_desc"] + "\"\t=\"" + dr["Fsubmit_user"] + "\"\t=\"" + dr["Frecycle_user"]
                    + "\"\t=\"" + dr["Frefund_type_str"] + "\"\t=\"" + dr["Fsam_no"] + "\"\t=\"" + dr["Fcreate_time"]
                     + "\"\t=\"" + dr["Fsubmit_refund_str"] + "\"");

                }
                sw.WriteLine("��������=" + count + "\t�ܽ�=" + classLibrary.setConfig.FenToYuan(amount));
                sw.Close();
                string f_name = "�˿�Ǽ�";
                f_name = System.Web.HttpUtility.UrlEncode(System.Text.Encoding.UTF8.GetBytes(f_name));
                Response.AddHeader("Content-Disposition", "attachment; filename=" + f_name+".xls");
                Response.ContentType = "application/ms-excel";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                Response.Write(sw);
                Response.End();
                */
                try
                {
                    #region ����excel�ļ�
                    var temp = DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.StaticNoManage();
                    string path = Server.MapPath("~/") + "PLFile" + "\\" + temp + "�˿�Ǽ�" + uin + ".xls"; //����

                    Application xlApp = new ApplicationClass();
                    Workbooks workbooks = xlApp.Workbooks;
                    Workbook workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                    Worksheet worksheet = (Worksheet)workbook.Worksheets[1];//ȡ��sheet1

                    Range range;

                    range = (Range)worksheet.Cells[1, 1];
                    range.ColumnWidth = 40;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "�Ƹ�ͨ������";

                    range = (Range)worksheet.Cells[1, 2];
                    range.ColumnWidth = 25;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "��������";

                    range = (Range)worksheet.Cells[1, 3];
                    range.ColumnWidth = 25;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "����״̬";

                    range = (Range)worksheet.Cells[1, 4];
                    range.ColumnWidth = 25;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "���׽��";

                    range = (Range)worksheet.Cells[1, 5];
                    range.ColumnWidth = 25;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "����˺�";

                    range = (Range)worksheet.Cells[1, 6];
                    range.ColumnWidth = 30;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "����˵��";

                    range = (Range)worksheet.Cells[1, 7];
                    range.ColumnWidth = 25;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "�Ǽ���";

                    range = (Range)worksheet.Cells[1, 8];
                    range.ColumnWidth = 25;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "��Ʒ������";

                    range = (Range)worksheet.Cells[1, 9];
                    range.ColumnWidth = 15;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "�˿�����";

                    range = (Range)worksheet.Cells[1, 10];
                    range.ColumnWidth = 30;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "SAM������";

                    range = (Range)worksheet.Cells[1, 11];
                    range.ColumnWidth = 20;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "����ʱ��";

                    range = (Range)worksheet.Cells[1, 12];
                    range.ColumnWidth = 15;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "�ύ�˿�";

                    range = (Range)worksheet.Cells[1, 13];
                    range.ColumnWidth = 25;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = 15;
                    range.Value2 = "�˿���";

                    int total = 0;
                    int amount = 0; //�ܽ��

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        total++;
                        amount += int.Parse(dr["Famount"].ToString());

                        range = (Range)worksheet.Cells[total + 1, 1]; //�ӵڶ��п�ʼ
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
                    //workbook.SaveCopyAs(path);  //2007�汾
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
                        CommMailSend.SendInternalMail(uin, "", "�˿�Ǽǵ���", "", false, fileAtta);
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
            //�󶨽���������Ϣ
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
                    msg = "�˿������Χ���˿��" + FRefundAmount + ",�������:" + _FRefundAmount;
                    return false;
                }
            }
            catch (Exception e)
            {
                msg ="��ѯ�������ʧ�ܣ�"+ e.Message;
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
                if (state.Contains("֧���ɹ�"))
                {
                    return true;
                }
                else 
                {
                    msg = "�ö���״̬������¼�룬����״̬��" + state;  
                    return false;
                }
            }
            catch (Exception e)
            {
                msg = "�ж϶���״̬ʧ�ܣ�" + e.Message;
                return false;
            }
        }

        public void btnUpload_Click(object sender, System.EventArgs e)
        {
            if (!File1.HasFile)
            {
                WebUtils.ShowMessage(this.Page, "��ѡ���ϴ��ļ���");
                return;
            }
            if (Path.GetExtension(File1.FileName).ToLower() == ".xls")
            {
                int succ = 0, fail = 0;
                int refundAmount = 0; //�˿���
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
                    string r3 = res_dt.Rows[i][2].ToString();//SAM������
                    string r4 = res_dt.Rows[i][3].ToString(); //�Ǽ���
                    string r5 = res_dt.Rows[i][4].ToString(); //��Ʒ������
                    string r6 = res_dt.Rows[i][5].ToString(); //�˿���

                    try
                    {
                        if (!string.IsNullOrEmpty(r6))
                        {
                            r6 = r6.Replace("Ԫ", "");
                            refundAmount = classLibrary.setConfig.YuanToFen(Convert.ToDouble(r6));
                        }
                    }
                    catch
                    {
                        fail++;
                        errMsg += "��" + (i + 1) + "��:�˿����������";
                        continue;
                    }
                  
                    string msg;
                    //����״̬Ϊ��֧���ɹ�/�ȴ����ҷ������Ķ���������¼�룬�Ǵ�״̬�Ķ�������¼��.
                    if (!isValidState(r1, out  msg))
                    {
                        fail++;
                        errMsg += "��" + (i + 1) + "��:" + msg + ".";
                        continue;
                    }
                    //��������ģ���н��������Ϊ:0<�˿���Q������
                    if (!isValidAmount(r1, r6, out msg))
                    {
                        fail++;
                        errMsg += "��" + (i + 1) + "��:" + msg + ".";
                        continue;
                    }
                    //��װ
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
                        errMsg += "��" + (i + 1) + "��:" + ser.Message;
                    }
                }

                //չʾ�ɹ����٣�ʧ�ܶ��٣�������Ϣ
                Table3.Visible = true;
                Table2.Visible = false;

                lbTotal.Text = iRows.ToString();
                lbSucc.Text = succ.ToString();
                lbFail.Text = fail.ToString();
                lbError.Text = errMsg;
            }
            else
            {
                WebUtils.ShowMessage(this.Page, "�ļ���ʽ����ȷ����ѡ��xls��ʽ�ļ��ϴ���");
                return;
            }
        }

        public void btnRefund_Click(object sender, System.EventArgs e)
        {

            if (!classLibrary.ClassLib.ValidateRight("InternetBankRefund", this))
            {
                //Ȩ���ж�
                WebUtils.ShowMessage(this.Page, "û��Ȩ�ޣ�");
                return;
            }

            DateTime begindate = DateTime.Parse(TextBoxBeginDate.Text);
            string stime = begindate.ToString("yyyy-MM-dd 00:00:00");
            DateTime enddate = DateTime.Parse(TextBoxEndDate.Text);
            string etime = enddate.ToString("yyyy-MM-dd 23:59:59");



            //�Ȳ�ѯ�˿��¼
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds = qs.QueryRefundInfo("", "", stime, etime, 0, 2, "2", 1, 1000); //֧���ɹ���δ�ύ

            FQuery_Service.Query_Service fs = new TENCENT.OSS.CFT.KF.KF_Web.FQuery_Service.Query_Service();
            FQuery_Service.Finance_Header Ffh = classLibrary.setConfig.FsetFH(this);
            fs.Finance_HeaderValue = Ffh;
            string msg = "";

            bool flag = false; //�����˿�ɹ���־
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
                        //������
                        try
                        {
                            flag = fs.StartTraderRefund_KF(dr["FTrade_Type"].ToString(), dr["Fbuy_acc"].ToString(), dr["Forder_id"].ToString(), long.Parse(dr["Famount"].ToString()), "�ͷ������˿�", "", out msg);
                            //���Ϊtrue�������״̬Ϊ���ύ
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
            //�ٲ�һ��ʧЧ�ļ�¼����ֹ����״̬�����ı�
            ds = qs.QueryRefundInfo("", "", stime, etime, 0, 3, "2", 1, 1000); //֧���ɹ���ʧЧ��
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    msg = "";
                    if (dr["FTrade_Type"].ToString() == "2" || dr["FTrade_Type"].ToString() == "3")
                    {
                        total++;
                        //������
                        try
                        {
                            flag = fs.StartTraderRefund_KF(dr["FTrade_Type"].ToString(), dr["Fbuy_acc"].ToString(), dr["Forder_id"].ToString(), long.Parse(dr["Famount"].ToString()), "�ͷ������˿�", "", out msg);
                            //���Ϊtrue�������״̬Ϊ���ύ
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
                //Ȩ���ж�
                WebUtils.ShowMessage(this.Page, "û��Ȩ�ޣ�");
                return;
            }

            Thread t = new Thread(RefundEmailMethod);
            t.Start();
            WebUtils.ShowMessage(this.Page, "��̨�����У��Ժ�������ʼ���");
        }

        private void RefundEmailMethod()
        {
            try
            {
                DateTime begindate = DateTime.Parse(TextBoxBeginDate.Text);
                string stime = begindate.ToString("yyyy-MM-dd 00:00:00");
                DateTime enddate = DateTime.Parse(TextBoxEndDate.Text);
                string etime = enddate.ToString("yyyy-MM-dd 23:59:59");


                //�Ȳ�ѯ�˿��¼
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                //DataSet ds = qs.QueryRefundInfo("", "", stime, etime, 0, 2, "2", 1, 5000); //֧���ɹ���δ�ύ
                DataSet ds = new RefundRegisterService().QueryRefundRegisterList("", "", stime, etime, 0, 2, "2", 0, 5000);

                #region ����excel�ļ�
                int total = 0; //�ܱ���
                var emailMsg = new StringBuilder("<html><head><title></title></head><body><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:1310px;line-height:22px; margin-left:10px; table-layout:fixed;\" align='left' ID='Table1'><tr><td style='width:1300px;'><p style='padding:10px 0;margin:0;'> ");
                string cs = "�װ��Ĳ���ͬ�£�<br>&nbsp;&nbsp;&nbsp;&nbsp;����Ϊ���������˿����ݣ�����{0}�ʣ�{1}����Э�����������˿��лл��</p>"; //2

                var apptmp1 = new StringBuilder(); //3
                apptmp1.Append("<p style='padding:10px 0; margin:0;'><table width='100%' border='1' align='center' cellpadding=\"0\" cellspacing=\"0\" ID=\"Table6\"><tr><td style=\"width:110px;\">�Ƹ�ͨ����</td><td style=\"width:55px;\">��������</td>");
                apptmp1.Append("<td style=\"width:65px;\">����״̬</td><td style=\"width:50px;\">���׽��</td><td style=\"width:55px\">����˺�</td><td style=\"width:120px\">����˵��</td><td style=\"width:55px\">�Ǽ���</td>");
                apptmp1.Append("<td style=\"width:55px\">��Ʒ������</td><td style=\"width:50px\">�˿�����</td><td style=\"width:100px\">SAM������</td><td style=\"width:80px\">����ʱ��</td><td style=\"width:45px\">�ύ�˿�</td></tr>");

                int amount = 0; //�ܽ��
                DateTime d = DateTime.Now;
                string no = d.ToString("yyyyMMddHHmmssffff");

                string path = Server.MapPath("~/") + "PLFile" + "\\�����˿�" + no + ".xls"; //����
                //StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default);
                //string csv_cont = "�˿����\t�˿���(Ԫ)\t�Ƹ�ͨ�˺�";
                //sw.WriteLine(csv_cont);

                Application xlApp = new ApplicationClass();
                Workbooks workbooks = xlApp.Workbooks;
                Workbook workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                Worksheet worksheet = (Worksheet)workbook.Worksheets[1];//ȡ��sheet1

                Range range;

                range = (Range)worksheet.Cells[1, 1];
                range.ColumnWidth = 45;
                range.NumberFormatLocal = "@";
                range.Font.Size = 15;
                range.Value2 = "���׵���";

                range = (Range)worksheet.Cells[1, 2];
                range.ColumnWidth = 25;
                range.NumberFormatLocal = "@";
                range.Font.Size = 15;
                range.Value2 = "�˿���(Ԫ)";

                range = (Range)worksheet.Cells[1, 3];
                range.ColumnWidth = 30;
                range.NumberFormatLocal = "@";
                range.Font.Size = 15;
                range.Value2 = "�˿ע";

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("Frefund_type_str", typeof(String));
                    ds.Tables[0].Columns.Add("Fsubmit_refund_str", typeof(String));
                    ds.Tables[0].Columns.Add("Ftrade_state_str", typeof(String));
                    ds.Tables[0].Columns.Add("Famount_str", typeof(String));
                    ds.Tables[0].Columns.Add("Frefund_amountStr", typeof(String));
                    Hashtable ht1 = new Hashtable();
                    ht1.Add("1", "Ͷ���˿�");
                    ht1.Add("2", "Ͷ���˿�");
                    ht1.Add("3", "Ͷ���˿�");
                    ht1.Add("4", "Ͷ���˿�");
                    ht1.Add("5", "Ͷ���˿�");
                    ht1.Add("10", "Ͷ���˿�"); //�������˿�����Ϊ1\2\3\4\5�Ķ�����ΪͶ���˿�
                    ht1.Add("11", "����ʧ��");
                    //Hashtable ht2 = new Hashtable();
                    //ht2.Add("1", "���ύ");
                    //ht2.Add("2", "δ�ύ");
                    //ht2.Add("3", "ʧЧ");

                    //��ת��
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

                            range = (Range)worksheet.Cells[total + 1, 1]; //�ӵڶ��п�ʼ
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
                            { //�ʼ�������ֻչʾ100��
                                apptmp1.Append("<tr>");

                                //��װ����
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
                                apptmp1.Append("���ύ");
                                apptmp1.Append("</td>");

                                apptmp1.Append("</tr>");
                            }
                        }
                    }
                }
                //�����ύ״̬Ϊ1=���ύ
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
                    //�ܱ���������5000��


                    //�ٲ�һ��ʧЧ�ļ�¼����ֹ����״̬�����ı�
                    //ds = qs.QueryRefundInfo("", "", stime, etime, 0, 3, "2", 1, 5000 - total); //֧���ɹ���ʧЧ��
                    ds = new RefundRegisterService().QueryRefundRegisterList("", "", stime, etime, 0, 3, "2", 0, 5000 - total);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].Columns.Add("Frefund_type_str", typeof(String));
                        ds.Tables[0].Columns.Add("Fsubmit_refund_str", typeof(String));
                        ds.Tables[0].Columns.Add("Ftrade_state_str", typeof(String));
                        ds.Tables[0].Columns.Add("Famount_str", typeof(String));
                        ds.Tables[0].Columns.Add("Frefund_amountStr", typeof(String));
                        Hashtable ht1 = new Hashtable();
                        ht1.Add("1", "Ͷ���˿�");
                        ht1.Add("2", "Ͷ���˿�");
                        ht1.Add("3", "Ͷ���˿�");
                        ht1.Add("4", "Ͷ���˿�");
                        ht1.Add("5", "Ͷ���˿�");
                        ht1.Add("10", "Ͷ���˿�"); //�������˿�����Ϊ1\2\3\4\5�Ķ�����ΪͶ���˿�
                        ht1.Add("11", "����ʧ��");
                        //Hashtable ht2 = new Hashtable();
                        //ht2.Add("1", "���ύ");
                        //ht2.Add("2", "δ�ύ");
                        //ht2.Add("3", "ʧЧ");

                        //��ת��
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
                                range = (Range)worksheet.Cells[total + 1, 1]; //�ӵڶ��п�ʼ
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

                                    //��װ����
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
                                    apptmp1.Append("���ύ");
                                    apptmp1.Append("</td>");

                                    apptmp1.Append("</tr>");
                                }
                            }
                        }
                    }
                }
                workbook.Saved = true;
                //workbook.SaveCopyAs(path);  //2007�汾
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

                //�����ύ״̬Ϊ1=���ύ
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

                string sub = "�������˿��ţ�" + no;
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
                //Ȩ���ж�
                WebUtils.ShowMessage(this.Page, "û��Ȩ�ޣ�");
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