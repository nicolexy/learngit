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
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Xml;
using CFT.CSOMS.BLL.ForeignCurrencyModule;
using TENCENT.OSS.CFT.KF.KF_Web.InternetBank;
using System.IO;

namespace TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay
{
    /// <summary>
    /// QueryYTTrade ��ժҪ˵����
    /// </summary>
    public partial class FCOrderQuery : System.Web.UI.Page
    {
        protected ForeignCurrencyService FCBLLService = new ForeignCurrencyService();

        protected void Page_Load(object sender, System.EventArgs e)
        {
            ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()");
            ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");

            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
                }
                this.btnQuery.Attributes.Add("onclick", "return CheckEmail();");
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
            DateTime begindate = new DateTime(), enddate = new DateTime();

            try
            {
                string s_date = TextBoxBeginDate.Text;
                if (s_date != null && s_date != "")
                {
                    begindate = DateTime.Parse(s_date);
                }
                string e_date = TextBoxEndDate.Text;
                if (e_date != null && e_date != "")
                {
                    enddate = DateTime.Parse(e_date);
                }
            }
            catch
            {
                throw new Exception("������������");
            }
            if (begindate.Year != enddate.Year)
            {
                throw new Exception("�벻Ҫ�����ѯ��");
            }
            string spid = txtspid.Text.ToString();
            string spListID = txtspListID.Text.ToString();
            string coding = txtCoding.Text.ToString();
            if (spid == "" && spListID == "" && coding == "")
            {
                throw new Exception("����������һ����ѯ�");
            }
            if (coding != "" && spid == "")
            {
                throw new Exception("�������̻���ţ�");
            }
        }

        public void btnQuery_Click(object sender, System.EventArgs e)
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
                ButtonExport.Visible = false;
                clearDT();
                clearDTUser();
                this.tableOrder.Visible = false;
                this.tableUser.Visible = false;
                this.pager.RecordCount = 1000;
                BindData(1);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
            }
        }

        private void BindData(int index)
        {
            try
            {
                this.pager.CurrentPageIndex = index;
                int max = pager.PageSize;
                int start = max * (index - 1);
                DataSet dsOrder = QueryData(start,max);
                if (dsOrder == null || dsOrder.Tables.Count < 1 || dsOrder.Tables[0].Rows.Count < 1)
                {
                    DataGrid1.DataSource = null;
                    DataGrid1.DataBind();
                    throw new Exception("���ݿ��޴˼�¼");
                }
                ButtonExport.Visible = true;
                ViewState["g_dt"] = dsOrder;
                DataGrid1.DataSource = dsOrder;
                DataGrid1.DataBind();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
            }

        }

        private DataSet QueryData(int start, int max)
        {
            try
            {
                string s_stime = TextBoxBeginDate.Text;
                string s_begindate = "";
                if (s_stime != null && s_stime != "")
                {
                    DateTime begindate = DateTime.Parse(s_stime);
                    s_begindate = begindate.ToString("yyyy-MM-dd 00:00:00");
                }
                string s_etime = TextBoxEndDate.Text;
                string s_enddate = "";
                if (s_etime != null && s_etime != "")
                {
                    DateTime enddate = DateTime.Parse(s_etime);
                    s_enddate = enddate.ToString("yyyy-MM-dd 23:59:59");
                }

                string spid = txtspid.Text.ToString();
                string coding = txtCoding.Text.ToString();
                string spListID = txtspListID.Text.ToString();

                TemplateControl temp = this;
                string ip = this.Page.Request.UserHostAddress;

                DataSet dsOrder = new DataSet();
                if (!string.IsNullOrEmpty(spid))
                {
                    DataSet ds = FCBLLService.MerInfoQuery(spid, "", ip);
                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                        throw new Exception("��ѯ�����̻��ڲ�id��");
                    string uid = ds.Tables[0].Rows[0]["uid"].ToString();//�̻��ڲ�id
                    dsOrder = FCBLLService.QueryOrderBySpid(uid, coding, s_begindate, s_enddate, ddl_state.SelectedValue, start, max);
                }
                else if (!string.IsNullOrEmpty(spListID))
                {
                    dsOrder = FCBLLService.QueryOrderByListId(spListID);
                }
                if (dsOrder == null || dsOrder.Tables.Count < 1 || dsOrder.Tables[0].Rows.Count < 1)
                    return null;

                dsOrder.Tables[0].Columns.Add("Fpaynum_str"); //����+���׽��
                dsOrder.Tables[0].Columns.Add("Ftrade_state_str"); //����״̬
                dsOrder.Tables[0].Columns.Add("FTK_str"); //�Ƿ��˿�
                dsOrder.Tables[0].Columns.Add("FZF_str"); //�Ƿ�ܸ�
                dsOrder.Tables[0].Columns.Add("Fprice_curtype_str"); //����
                classLibrary.setConfig.FenToYuan_Table(dsOrder.Tables[0], "Fpaynum", "Fpaynum_str");//���׽��

                foreach (DataRow row in dsOrder.Tables[0].Rows)
                {
                    string cur_type = row["Fprice_curtype"].ToString();
                    if (InternetBankDictionary.CurTypeAIMark.ContainsKey(cur_type))
                    {
                        row["Fprice_curtype_str"] = InternetBankDictionary.CurTypeAIMark[cur_type];
                    }
                    else
                    {
                        row["Fprice_curtype_str"] = cur_type;
                    }
                    row["Fpaynum_str"] = row["Fprice_curtype_str"].ToString() + "-" + row["Fpaynum_str"].ToString();
                    string tmp = row["Ftrade_state"].ToString();
                    switch (tmp)
                    {
                        case "1":
                            row["Ftrade_state_str"] = "�ȴ����֧��"; break;
                        case "2":
                            row["Ftrade_state_str"] = " ֧���ɹ�"; break;
                        case "7":
                            row["Ftrade_state_str"] = "ת���˿�"; break;
                        case "99":
                            row["Ftrade_state_str"] = " ���׹ر�"; break;
                        default:
                            row["Ftrade_state_str"] = "δ֪"; break;
                    }
                    if (tmp == "7")
                        row["FTK_str"] = "��";
                    else
                        row["FTK_str"] = "��";
                    int n = Convert.ToInt32(row["Fbusiness_flag"].ToString());
                    if (Convert.ToInt32(n & 1) == 1)
                        row["FZF_str"] = "��";
                    else
                        row["FZF_str"] = "��";

                }
                return dsOrder;
            }
            catch (Exception eSys)
            {
                throw new Exception(eSys.Message);
            }

        }

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            try
            {
                int rid = e.Item.ItemIndex;
                GetDetail(rid);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message);
            }
        }

        private void GetDetail(int rid)
        {
            try
            {
                this.tableOrder.Visible = true;
                this.tableUser.Visible = true;
                //��Ҫע���ҳ���
                clearDT();
                clearDTUser();
                DataSet ds = (DataSet)ViewState["g_dt"];
                DataTable g_dt = ds.Tables[0];
                if (g_dt != null)
                {
                    lb_c1.Text = g_dt.Rows[rid]["Fspid"].ToString();//�̻����
                    lb_c2.Text = g_dt.Rows[rid]["Fcoding"].ToString();//�̼Ҷ�����
                    lb_c3.Text = g_dt.Rows[rid]["Flistid"].ToString();//�Ƹ�ͨ������
                    lb_c4.Text = g_dt.Rows[rid]["Frec_banklist"].ToString();//���ж�����
                    lb_c6.Text = g_dt.Rows[rid]["Fcreate_time"].ToString();//����ʱ��
                    lb_c7.Text = g_dt.Rows[rid]["Fpaynum_str"].ToString(); //���׽��
                    lb_c8.Text = g_dt.Rows[rid]["Fsub_business_type"].ToString(); //���׿���
                    lb_c10.Text = g_dt.Rows[rid]["Ftrade_state_str"].ToString();//����״̬
                    string tmp = g_dt.Rows[rid]["Frefund_state"].ToString();//�˿�״̬
                    switch (tmp)
                    {
                        case "1":
                            lb_c12.Text = "��ʼ״̬"; break;
                        case "9":
                            lb_c12.Text = " �˿�ɹ�"; break;
                    }
                    lb_c13.Text = classLibrary.setConfig.FenToYuan(g_dt.Rows[rid]["Ftotal_payout"].ToString());//�˿���

                    //������ˮ
                    DataSet dsbank = new DataSet();
                    string bankNo = g_dt.Rows[rid]["Frec_banklist"].ToString();
                    if (!string.IsNullOrEmpty(bankNo))
                        dsbank = FCBLLService.BankRollQuery(bankNo, g_dt.Rows[rid]["Fbank_type"].ToString(), "10100", "", "");
                    if (dsbank != null && dsbank.Tables.Count > 0 && dsbank.Tables[0].Rows.Count > 0)
                    {
                        string cardNo = dsbank.Tables[0].Rows[0]["card_no"].ToString();     //����
                        //���ܿ�
                        cardNo = FCBLLService.BankDecodingQuery(cardNo);
                        if (cardNo.Length > 10)
                            cardNo = cardNo.Substring(0, 6) + "****" + cardNo.Substring(cardNo.Length - 4);
                        lb_c9.Text = cardNo;
                        lb_c5.Text = dsbank.Tables[0].Rows[0]["authorize"].ToString();//������Ȩ��
                        bb_c1.Text = dsbank.Tables[0].Rows[0]["user_name"].ToString(); //�ֿ�������
                        bb_c2.Text = dsbank.Tables[0].Rows[0]["user_email"].ToString();//����
                        bb_c3.Text = dsbank.Tables[0].Rows[0]["user_phone"].ToString();//�绰
                        bb_c4.Text = dsbank.Tables[0].Rows[0]["client_ip"].ToString();//IP��Դ
                        bb_c5.Text = dsbank.Tables[0].Rows[0]["user_address"].ToString();//����/����
                        bb_c6.Text = dsbank.Tables[0].Rows[0]["user_zipCode"].ToString();//��ַ
                    }

                    //���ݶ�����ѯ�ܸ�
                    DataSet dsJF = new DataSet();
                    if (!string.IsNullOrEmpty(g_dt.Rows[rid]["Fcreate_time"].ToString()) && !string.IsNullOrEmpty(g_dt.Rows[rid]["Flistid"].ToString()))
                        dsJF = FCBLLService.QueryRefuseOrder("", g_dt.Rows[rid]["Flistid"].ToString(), g_dt.Rows[rid]["Fcreate_time"].ToString(), "", "","","","", 0, 2);
                    if (dsJF != null && dsJF.Tables.Count > 0 && dsJF.Tables[0].Rows.Count > 0)
                    {
                        string str = dsJF.Tables[0].Rows[0]["Fcheck_state"].ToString();//�ܸ�״̬
                        switch (str)
                        {
                            case "1":
                                lb_c14.Text = "�鵥"; break;
                            case "2":
                                lb_c14.Text = "�ܸ�"; break;
                            case "3":
                                lb_c14.Text = "Ԥ�ٲ�"; break;
                            case "4":
                                lb_c14.Text = "�ٲ�"; break;
                            default:
                                lb_c14.Text = str; break;
                        }
                        str = dsJF.Tables[0].Rows[0]["Fsp_process_state"].ToString();//�̻�����״̬
                        switch (str)
                        {
                            case "1":
                                lb_c15.Text = "δ����"; break;
                            case "2":
                                lb_c15.Text = "������"; break;
                            case "3":
                                lb_c15.Text = "ͬ��ܸ�"; break;
                            case "4":
                                lb_c15.Text = "���账���Ѿ��˿"; break;
                            case "5":
                                lb_c15.Text = "�̻�����ת�˿�"; break;
                            default:
                                lb_c15.Text = str; break;
                        }
                        lb_c16.Text = classLibrary.setConfig.FenToYuan(dsJF.Tables[0].Rows[0]["Ftrade_refund_fee"].ToString());//�ܸ��˿���
                    }
                }
            }
            catch (Exception eSys)
            {
                throw new Exception(eSys.Message);
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
                ExportData();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
            }
        }

        private void ExportData()
        {
         //   DataSet dsOrder = QueryData(0, 1000);
            DataSet dsOrder = (DataSet)ViewState["g_dt"];
            if (dsOrder == null || dsOrder.Tables.Count < 1 || dsOrder.Tables[0].Rows.Count < 1)
            {
                throw new Exception("���ݿ��޴˼�¼");
            }

            StringWriter sw = new StringWriter();
            string excelHeader = DataGrid1.Columns[0].HeaderText;
            for (int i = 1; i < DataGrid1.Columns.Count; i++)
            {
                excelHeader += "\t" + DataGrid1.Columns[i].HeaderText;
            }
            sw.WriteLine(excelHeader);

            foreach (DataRow dr in dsOrder.Tables[0].Rows)
            {
                sw.WriteLine("=\"" + dr["Fspid"].ToString() + "\"\t=\"" + dr["Fcoding"].ToString() + "\"\t=\"" + dr["Flistid"]
                    + "\"\t" + dr["Fcreate_time"] + "\t=\"" + dr["Fpaynum_str"] + "\"\t=\"" + dr["Ftrade_state_str"] + "\"\t=\"" + dr["FTK_str"] + "\"\t=\"" + dr["FZF_str"]
                + "\"");

            }
            sw.Close();
            Response.AddHeader("Content-Disposition", "attachment; filename=������ѯ.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.Write(sw);
            Response.End();
        }
        private void clearDT()
        {
            lb_c1.Text = "";
            lb_c2.Text = "";
            lb_c3.Text = "";
            lb_c4.Text = "";
            lb_c5.Text = "";
            lb_c6.Text = "";
            lb_c7.Text = "";

            lb_c8.Text = "";
            lb_c9.Text = "";
            lb_c10.Text = "";
            lb_c11.Text = "";
            lb_c12.Text = "";
            lb_c13.Text = "";
            lb_c14.Text = "";
            lb_c15.Text = "";
            lb_c16.Text = "";
        }

        private void clearDTUser()
        {
            bb_c1.Text = "";
            bb_c2.Text = "";
            bb_c3.Text = "";
            bb_c4.Text = "";
            bb_c5.Text = "";
            bb_c6.Text = "";
            bb_c7.Text = "";
            bb_c8.Text = "";
        }

    }
}