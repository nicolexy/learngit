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

namespace TENCENT.OSS.CFT.KF.KF_Web.InternetBank
{
    /// <summary>
    /// RefundQuery ��ժҪ˵����
    /// </summary>
    public partial class RefundQuery : System.Web.UI.Page
    {
        protected static List<int> refundIdList = new List<int>();

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��
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
                    btnUpload.Visible = classLibrary.ClassLib.ValidateRight("RefundCheck", this);  //�˿�Ǽǵ���excel��ťȨ��

                    TextBoxBeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                    //setConfig.GetAllTypeList(ddlTradeState, "PAY_STATE");
                    ddlTradeState.Items.Insert(0, new ListItem("ȫ��", "0"));
                    //�˿�Ǽ�ģ�� v_yqyqguo
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
                    this.ddl_refund_id.Items.Insert(0, new ListItem("ȫ��", "0"));
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

        private void BindData(int index)
        {
            DateTime begindate = DateTime.Parse(TextBoxBeginDate.Text);
            string stime = begindate.ToString("yyyy-MM-dd 00:00:00");
            DateTime enddate = DateTime.Parse(TextBoxEndDate.Text);
            string etime = enddate.ToString("yyyy-MM-dd 23:59:59");

            string listid = listId.Text.Trim();                       //��������
            string cftorderid = cftOrderId.Text.Trim();               //�Ƹ�ͨ������
            int refund_id = int.Parse(ddl_refund_id.SelectedValue);   //�̻���
            string submit_user = this.tbx_submit_user.Text.Trim();    //�Ǽ���
            int rf_type = int.Parse(ddlRefundType.SelectedValue);     //�˿�����
            int rf_status = int.Parse(ddlRefundStatus.SelectedValue); //�ύ�˿�״̬
            string s_trade_state = ddlTradeState.SelectedValue;       //����״̬

            int count = new RefundRegisterService().QueryRefundRegisterCount(listid, cftorderid, stime, etime, rf_type, rf_status, s_trade_state, refund_id, submit_user);
            Label9.Text = count.ToString();

            pager.RecordCount = count;
            pager.CurrentPageIndex = index;
            int max = pager.PageSize;
            int start = max * (index - 1);

            DataSet ds = new RefundRegisterService().QueryRefundRegisterList(listid, cftorderid, stime, etime, rf_type, rf_status, s_trade_state, refund_id, submit_user, start, max);

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


                //classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0], "Ftrade_state_new", "Ftrade_state_str", "PAY_STATE");

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

        /// <summary>
        /// ����Excel
        /// </summary>
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
            int refund_id = int.Parse(ddl_refund_id.SelectedValue);  //�̻���
            string submit_user = this.tbx_submit_user.Text.Trim();   //�Ǽ���
            int rf_type = int.Parse(ddlRefundType.SelectedValue);
            int rf_status = int.Parse(ddlRefundStatus.SelectedValue);
            string s_trade_state = ddlTradeState.SelectedValue;

            List<string> fileAtta = new List<string>();
            try
            {
                int total = new RefundRegisterService().QueryRefundRegisterCount(listid, cftorderid, stime, etime, rf_type, rf_status, s_trade_state, refund_id, submit_user);

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
                        DataSet ds = new RefundRegisterService().QueryRefundRegisterList(listid, cftorderid, stime, etime, rf_type, rf_status, s_trade_state,
                            refund_id, submit_user, j * maxSheet, maxSheet);
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
                                List<string> titList = new List<string>()
                        {
                            "�Ƹ�ͨ������","��������","����״̬","���׽��","����˺�","����˵��","�Ǽ���","��Ʒ������","�˿�����","SAM������",
                            "����ʱ��","�ύ�˿�","�˿���"
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
                                //workbook.SaveCopyAs(path);  //2007�汾
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

                                #region excelд������
                                string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + path + ";" + "Excel 12.0 Xml;HDR=YES;";
                                OleDbConnection conn = new OleDbConnection(strConn);
                                conn.Open();
                                OleDbCommand cmd = new OleDbCommand();
                                cmd.Connection = conn;

                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    cmd.CommandText = string.Format(@"
                                    INSERT INTO [sheet1$] 
                                        (�Ƹ�ͨ������,��������,����״̬,���׽��,����˺�,����˵��,�Ǽ���,��Ʒ������,�˿�����,SAM������,
                                        ����ʱ��,�ύ�˿�,�˿���) 
                                    VALUES
                                        ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',
                                        '{10}','{11}','{12}')",
                                    dr["Forder_id"].ToString(), dr["Fcoding"].ToString(), dr["Ftrade_state_str"].ToString(), dr["Famount_str"].ToString(),
                                    dr["Fbuy_acc"].ToString(), dr["Ftrade_desc"].ToString(), dr["Fsubmit_user"].ToString(), dr["Frecycle_user"].ToString(),
                                    dr["Frefund_type_str"].ToString(),dr["Fsam_no"].ToString(),dr["Fcreate_time"].ToString(),dr["Fsubmit_refund_str"].ToString(),
                                    dr["Frefund_amountStr"].ToString());
                                    cmd.ExecuteNonQuery();
                                }
                                conn.Close();
                                #endregion

                                #endregion
                            }
                            catch (Exception ex)
                            {
                                LogManager.GetLogger("�˿�Ǽ�����Excelʧ�ܣ�" + ex.Message + ", stacktrace" + ex.StackTrace);
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
                LogManager.GetLogger("�����˿�Ǽ�ʧ�ܣ�" + ex.Message + ", stacktrace" + ex.StackTrace);
            }

            if (fileAtta.Count > 0)
            {
                try
                {
                    CommMailSend.SendInternalMail(uin, "", "�˿�Ǽǵ���", "", false, fileAtta.ToArray());
                }
                catch (Exception ex){}
            }
        }

        private void Upload()
        {
            string uid = Session["uid"].ToString();
            try
            {
                int succ = 0, fail = 0;
                int FRefundtype = 0;//�˿�����
                int refundAmount = 0; //�˿���
                string errMsg = "";

                string path = Server.MapPath("~/") + "PLFile" + "\\refund.xls";
                File1.PostedFile.SaveAs(path);
                DataSet res_ds = PublicRes.readXls(path, "F1,F2,F3,F4,F5,F6");
                System.Data.DataTable res_dt = res_ds.Tables[0];
                //��¼ʧ�ܶ���
                System.Data.DataTable failed_dt = new System.Data.DataTable();
                failed_dt.Columns.Add("������", typeof(System.String));
                failed_dt.Columns.Add("�˿�����", typeof(System.String));
                failed_dt.Columns.Add("SAM������", typeof(System.String));
                failed_dt.Columns.Add("�Ǽ���", typeof(System.String));
                failed_dt.Columns.Add("��Ʒ������", typeof(System.String));
                failed_dt.Columns.Add("�˿��Ԫ��", typeof(System.String));
                failed_dt.Columns.Add("Message", typeof(System.String));

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

                    //��¼���󶩵�
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
                        errMsg += "��" + (i + 1) + "��:�������ʹ���";
                        failed_dr[6] = "�������ʹ���";
                        failed_dt.Rows.Add(failed_dr);
                        continue;
                    }

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
                        failed_dr[6] = "�˿����������";
                        failed_dt.Rows.Add(failed_dr);
                        continue;
                    }
                    //��װ
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
                        errMsg += "��" + (i + 1) + "��:" + ser.Message;
                        failed_dr[6] = ser.Message;
                        failed_dt.Rows.Add(failed_dr);
                    }
                }

                try
                {
                    path = Server.MapPath("~/") + "PLFile" + "\\" + uid + ".xls"; //����
                    PublicRes.Export(failed_dt, path);
                    string[] fileAtta = { path };
                    //mail
                    CommMailSend.SendInternalMail(uid, "", "�˿�Ǽǵ���;������" + iRows + ";�ɹ�����" + succ + ";ʧ������" + fail, "", false, fileAtta);

                }
                catch (Exception e)
                {
                    throw e;
                }
                //չʾ�ɹ����٣�ʧ�ܶ��٣�������Ϣ
                Table3.Visible = true;
                Table2.Visible = false;

                lbTotal.Text = iRows.ToString();
                lbSucc.Text = succ.ToString();
                lbFail.Text = fail.ToString();
                lbError.Text = errMsg;
            }
            catch (Exception e)
            {
                LogHelper.LogInfo("���������˿�Ǽ�ʧ��!" + e.Message);
                CommMailSend.SendInternalMail(uid, "", "���������˿�Ǽ�ʧ��", e.ToString(), false);
            }

        }
        public void btnUpload_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!File1.HasFile)
                {
                    WebUtils.ShowMessage(this.Page, "��ѡ���ϴ��ļ���");
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
                        string orderId = "", filterOrder = "";
                        foreach (DataRow item in res_dt.Rows)
                        {
                            orderId = item[0].ToString();
                            if (orderId.Length >= 10 && !refundIdList.Contains(Convert.ToInt32(orderId.Substring(0, 10))))
                            {
                                filterOrder += orderId + "��";
                            }
                        }
                        if (!string.IsNullOrEmpty(filterOrder))
                        {
                            WebUtils.ShowMessage(this.Page, filterOrder + " �̼ҵĶ����������������˿");
                            return;
                        }
                    }
                    //  Upload();
                    Thread thread = new Thread(Upload);
                    thread.Start();
                    WebUtils.ShowMessage(this.Page, "��̨�����У��Ժ�������ʼ���");
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "�ļ���ʽ����ȷ����ѡ��xls��ʽ�ļ��ϴ���");
                    return;
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);
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

        /// <summary>
        /// �ύ�����˿�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnRefundEmail_Click(object sender, System.EventArgs e)
        {
            if (!classLibrary.ClassLib.ValidateRight("InternetBankRefund", this))
            {
                //Ȩ���ж�
                WebUtils.ShowMessage(this.Page, "û��Ȩ�ޣ�");
                return;
            }
           
            DateTime begindate, enddate;
            if(!DateTime.TryParse(TextBoxBeginDate.Text,out begindate) || !DateTime.TryParse(TextBoxEndDate.Text,out enddate))
            {
                WebUtils.ShowMessage(this.Page, "��������ȷ��ʱ�䡣");
                return ;
            }
            string stime = begindate.ToString("yyyy-MM-dd 00:00:00");
            string etime = enddate.ToString("yyyy-MM-dd 23:59:59");

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                long amount = 0;    //�ܽ��
                const int fileRowConut = 3000;  //�����ļ�����¼��
                List<string> fidList = new List<string>(); //��Ҫ����״̬�ĲƸ�ͨ����Fid
                System.Data.DataTable dt = null;
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                #region �����ύ״̬Ϊ (1 = ���ύ)   ��ί��
                Action<List<string>> updateState = fidArr =>
                {
                    int fidCount = 0;
                    string fids = string.Empty;
                    foreach (string row in fidArr)
                    {
                        if (fidCount < 100)
                        {
                            fids += row + ",";
                            fidCount++;
                        }
                        else
                        {
                            fidCount = 0;
                            qs.UpdateSubmitRefundState(fids.TrimEnd(','), 1);
                        }
                    }
                }; 
                #endregion
               
                #region ��ȡ����
                try
                {
                    int count = new RefundRegisterService().GetExportRefundCount(stime, etime);
                    if (count > 0)
                    {
                        int countSheet = 1;
                        if (count > fileRowConut)
                        {
                            countSheet = count % fileRowConut == 0 ? count / fileRowConut : (count / fileRowConut) + 1;
                        }
                        List<string> excelFile = new List<string>();
                        string no = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                        for (int j = 0; j < countSheet; j++)
                        {
                            DataSet dsa = new RefundRegisterService().GetExportRefundData(stime, etime, j * fileRowConut, fileRowConut);
                            dt = dsa.Tables[0];

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                #region ���ɸ���
                                try
                                {
                                    string path = Server.MapPath("~/") + "PLFile\\�����˿�{0}_{1}.xls";
                                    Application xlApp = new ApplicationClass();
                                    Workbooks workbooks = xlApp.Workbooks;
                                    Workbook workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                                    Worksheet worksheet = (Worksheet)workbook.Worksheets[1]; //ȡ��sheet1
                                    Range range;

                                    #region ����
                                    range = (Range)worksheet.Cells[1, 1];
                                    range.ColumnWidth = 45;
                                    range.NumberFormatLocal = "@";
                                    range.Font.Size = 15;
                                    range.Value2 = "���׵���";

                                    range = (Range)worksheet.Cells[1, 2];
                                    range.ColumnWidth = 25;
                                    range.NumberFormatLocal = "@";
                                    range.Font.Size = 15;
                                    range.Value2 = "�˿��Ԫ��";

                                    range = (Range)worksheet.Cells[1, 3];
                                    range.ColumnWidth = 30;
                                    range.NumberFormatLocal = "@";
                                    range.Font.Size = 15;
                                    range.Value2 = "�˿ע";
                                    #endregion

                                    workbook.Saved = true;
                                    var curPath = string.Format(path, no, j + 1);
                                    workbook.SaveAs(curPath, 56, Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSaveAsAccessMode.xlExclusive,
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

                                    excelFile.Add(curPath);

                                    string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + curPath + ";" + "Excel 12.0 Xml;HDR=YES;";
                                    OleDbConnection conn = new OleDbConnection(strConn);
                                    conn.Open();
                                    OleDbCommand cmd = new OleDbCommand();
                                    cmd.Connection = conn;

                                    foreach (DataRow row in dt.Rows)
                                    {
                                        amount += long.Parse(row["Frefund_amount"].ToString());
                                        var tradeType = row["FTrade_Type"].ToString();
                                        if (tradeType == "2" || tradeType == "3")  //���ݵ���󶩵���״̬�����������ط������ı䣬���ٴ��ж�
                                        {
                                            fidList.Add(row["Fid"].ToString());
                                        }
                                        cmd.CommandText = string.Format(@"
                                        INSERT INTO [sheet1$] 
                                            (���׵���,�˿��Ԫ��,�˿ע) 
                                        VALUES
                                        ('{0}','{1}','{2}')",
                                        row["Forder_id"].ToString(), MoneyTransfer.FenToYuan(row["Frefund_amount"].ToString()), row["Fbuy_acc"].ToString());
                                        cmd.ExecuteNonQuery();
                                    }
                                    conn.Close();
                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger("�ύ�����˿�����Excelʧ�ܣ�countSheet��" + countSheet + ", " + ex.Message + ", stacktrace" + ex.StackTrace);
                                }
                                finally
                                {
                                    GC.Collect();
                                }

                                #endregion
                            }
                        }

                        if (excelFile.Count > 0)
                        {
                            bool sendFlag = true;
                            try
                            {
                                #region �ʼ�����
                                var emailMsg = new StringBuilder("<html><head><title></title></head><body>");
                                emailMsg.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:1310px;line-height:22px; margin-left:10px; table-layout:fixed;\" align='left' ID='Table1'><tr><td style='width:1300px;'>");

                                var msg = "<p style='padding:10px 0;margin:0;'> �װ��Ĳ���ͬ�£�<br>&nbsp;&nbsp;&nbsp;&nbsp;����Ϊ���������˿����ݣ�����{0}�ʣ�{1}����Э�����������˿��лл��</p>";
                                emailMsg.AppendFormat(msg, count, classLibrary.setConfig.FenToYuan(amount));

                                emailMsg.Append("</table></p></td></tr><tr><td height=\"15\"></td></tr></table></body></html>");

                                string sub = "�������˿��ţ�" + no;
                                string toMail = ConfigurationManager.AppSettings["InternetRefundToMail"].ToString();
                                string ccMail = ConfigurationManager.AppSettings["InternetRefundCcMail"].ToString();
                                CommMailSend.SendInternalMail(toMail, ccMail, sub, emailMsg.ToString(), true, excelFile.ToArray());
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                sendFlag = false;
                            }
                            if (sendFlag) //�ʼ��������쳣����״̬
                            {
                                updateState(fidList);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger("�ύ�����˿�ʧ�ܣ�" + ex.Message + ", stacktrace" + ex.StackTrace);
                }
                #endregion
            });

            WebUtils.ShowMessage(this.Page, "��̨�����У��Ժ�������ʼ���");
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