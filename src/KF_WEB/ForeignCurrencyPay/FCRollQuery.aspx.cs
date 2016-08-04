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
    public partial class FCRollQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        public DateTime qbegindate, qenddate;
        protected ForeignCurrencyService FCBLLService = new ForeignCurrencyService();
        protected void Page_Load(object sender, System.EventArgs e)
        {
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
            this.DataGridCurType.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGridCurType_ItemCommand);
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

        }
        #endregion

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                pager.CurrentPageIndex = e.NewPageIndex;
                BindData(e.NewPageIndex);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
            }
        }

        private void ValidateDate()
        {
            DateTime begindate = new DateTime(), enddate = new DateTime();
            string s_date = TextBoxBeginDate.Value;
            string e_date = TextBoxEndDate.Value;
            try
            {
                if (s_date != null && s_date != "")
                {
                    begindate = DateTime.Parse(s_date);
                }
                if (e_date != null && e_date != "")
                {
                    enddate = DateTime.Parse(e_date);
                }
            }
            catch
            {
                throw new Exception("������������");
            }
            string spid = txtspid.Text.ToString();

            if (spid == "")
            {
                throw new Exception("�������̻���ţ�");
            }
            if (string.IsNullOrEmpty(s_date) || string.IsNullOrEmpty(e_date))
            {
                throw new Exception("���������ڣ�");
            }
            if (begindate.Day != enddate.Day)
            {
                throw new Exception("ֻ�ܲ�ѯһ����ˮ��");
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
                DataGrid1.DataSource = null;
                DataGrid1.DataBind();
                this.pager.RecordCount = 1000;
                BindCurType(1);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
            }
        }

        private void BindCurType(int index)
        {
            try
            {
                string s_stime = TextBoxBeginDate.Value;
                string s_begindate = "";
                if (s_stime != null && s_stime != "")
                {
                    DateTime begindate = DateTime.Parse(s_stime);
                    s_begindate = begindate.ToString("yyyy-MM-dd 00:00:00");
                }
                string s_etime = TextBoxEndDate.Value;
                string s_enddate = "";
                if (s_etime != null && s_etime != "")
                {
                    DateTime enddate = DateTime.Parse(s_etime);
                    s_enddate = enddate.ToString("yyyy-MM-dd 23:59:59");
                }

                string spid = txtspid.Text.ToString().Trim();
                int max = pager.PageSize;
                int start = max * (index - 1);

                //��ѯ�̻��ڲ�id
                string ip = this.Page.Request.UserHostAddress;
                DataSet dsUid = FCBLLService.MerInfoQuery(spid, "", ip);
                if (dsUid == null || dsUid.Tables.Count < 1 || dsUid.Tables[0].Rows.Count < 1)
                    throw new Exception("��ѯ�����̻��ڲ�id��");
                string uid = dsUid.Tables[0].Rows[0]["uid"].ToString();//�̻��ڲ�id
                ViewState["spidUid"] = uid;
                ViewState["s_begindate"] = s_begindate;
                ViewState["s_enddate"] = s_enddate;

                //�����̻��ż����Ͳ鵽acno
                DataSet ds = new DataSet();
                ds = FCBLLService.AcnoQuery(spid, acc_type.SelectedValue, "");
                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    DataGridCurType.DataSource = null;
                    DataGridCurType.DataBind();
                    throw new Exception("���ݿ��޴˼�¼");
                }
                ds.Tables[0].Columns.Add("Fcur_type_str"); //����
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string cur_type = row["Fcur_type"].ToString();
                    if (InternetBankDictionary.CurTypeAI.ContainsKey(cur_type))
                    {
                        row["Fcur_type_str"] = InternetBankDictionary.CurTypeAI[cur_type];
                    }
                    else
                    {
                        row["Fcur_type_str"] = cur_type;
                    }

                }
                if (ds.Tables[0].Rows.Count != 1)
                {
                    DataGridCurType.DataSource = ds;
                    DataGridCurType.DataBind();
                    return;
                }
                else//ֻ��һ�����־�ֱ�Ӳ�ѯ��ˮ
                {
                    ViewState["acno"] = ds.Tables[0].Rows[0]["Facno"].ToString().Trim();
                    BindData(index);
                }
            }
            catch (Exception eSys)
            {
                throw new Exception(eSys.Message.ToString());
                //WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
            }

        }
        private void BindData(int index)
        {
            try
            {
                this.pager.CurrentPageIndex = index;
                int max = pager.PageSize;
                int start = max * (index - 1);
              
                DataSet ds = QueryData(max, start);
                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    ButtonExport.Visible = false;
                    DataGrid1.DataSource = null;
                    DataGrid1.DataBind();
                    throw new Exception("���ݿ��޴˼�¼");
                }
                ViewState["g_dt"] = ds;
                DataGrid1.DataSource = ds;
                DataGrid1.DataBind();
            }
            catch (Exception eSys)
            {
                throw new Exception(eSys.Message.ToString());
            }
        }

        private DataSet QueryData( int max, int start)
        {
            try
            {
                string uid = ViewState["spidUid"].ToString();
                string s_begindate = ViewState["s_begindate"].ToString();
                string s_enddate = ViewState["s_enddate"].ToString();
                DataSet ds = new DataSet();
                //��ѯ�̻���ˮ
                ds = FCBLLService.QueryMerchantRoll(uid, "", s_begindate, s_enddate, ViewState["acno"].ToString(), start, max);
                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    ButtonExport.Visible = false;
                    return null;
                }

                ds.Tables[0].Columns.Add("Fspid"); //�̻����
                ds.Tables[0].Columns.Add("Facc_type_str"); //�˻�����
                ds.Tables[0].Columns.Add("Fsubject_str"); //��������
                ds.Tables[0].Columns.Add("isIn"); //����
                ds.Tables[0].Columns.Add("isOut"); //֧��
                ds.Tables[0].Columns.Add("Fbalance_str"); //���?????
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fbalance", "Fbalance_str");
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    row["Fspid"] = TextBoxBeginDate.Value;
                    string type = row["Ftype"].ToString();
                    if (type == "1")//������ͣ�1-��2-�� 3-���� 4-�ⶳ 
                    {
                        row["isIn"] = "��";
                        row["isOut"] = "��";
                    }
                    else if (type == "2")
                    {
                        row["isIn"] = "��";
                        row["isOut"] = "��";
                    }
                    else
                    {
                        row["isIn"] = "��";
                        row["isOut"] = "��";
                    }
                    row["Fmemo"] = row["Fmemo"].ToString();
                    string subject = row["Fsubject"].ToString();
                    switch (subject)
                    {
                        case "9":
                            row["Fsubject_str"] = "���ٽ���"; break;
                        case "10":
                            row["Fsubject_str"] = "���֧��"; break;
                        case "11":
                            row["Fsubject_str"] = "��ֵ"; break;
                        case "12":
                            row["Fsubject_str"] = "��ֵת��"; break;
                        case "13":
                            row["Fsubject_str"] = "ת��"; break;
                        case "14":
                            row["Fsubject_str"] = "����"; break;
                        case "5":
                            row["Fsubject_str"] = "�˿�"; break;
                        default:
                            row["Fsubject_str"] = subject; break;
                    }
                    row["Facc_type_str"] = acc_type.SelectedItem.Text;
                }
                ButtonExport.Visible = true;
                return ds;
            }
            catch (Exception eSys)
            {
                throw new Exception(eSys.Message.ToString());
            }
        }

        private void DataGridCurType_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            try
            {
                ViewState["acno"] = e.Item.Cells[0].Text.Trim();
                BindData(1);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
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
                WebUtils.ShowMessage(this.Page, "��������ʧ�ܣ�" + eSys.Message.ToString());
            }
        }

        private void ExportData()
        {
           // DataSet ds= QueryData(1000, 0);
            DataSet ds = (DataSet)ViewState["g_dt"];//��ҳ����
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
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

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sw.WriteLine("=\"" + dr["Fspid"].ToString() + "\"\t=\"" + dr["Ftrade_time"].ToString() + "\"\t=\"" + dr["Fcoding"]
                    + "\"\t" + dr["Facc_type_str"] + "\t=\"" + dr["Fsubject_str"] + "\"\t=\"" + dr["isIn"] + "\"\t=\"" 
                    + dr["isOut"] + "\"\t=\"" + dr["Fbalance_str"]+ "\"\t=\"" + dr["Fmemo"]  + "\"");

            }
            sw.Close();
            Response.AddHeader("Content-Disposition", "attachment; filename=����˻���ˮ��ѯ.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.Write(sw);
            Response.End();
        }
    }
}