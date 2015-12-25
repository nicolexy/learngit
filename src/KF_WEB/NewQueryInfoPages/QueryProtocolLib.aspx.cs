using System;
using System.Configuration;
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
using System.IO;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
    /// QueryProtocolLib ��ժҪ˵����
	/// </summary>
    public partial class QueryProtocolLib : System.Web.UI.Page
	{
        public DateTime qbegindate, qenddate;

        protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    string sbegindate = Request.QueryString["qbegindate"];
                    if (sbegindate != null && sbegindate != "")
                    {
                        qbegindate = DateTime.Parse(sbegindate);
                        TextBoxBeginDate.Value = qbegindate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        qbegindate = DateTime.Now;
                        TextBoxBeginDate.Value = DateTime.Now.ToString("yyyy-MM-01");
                    }
                    string senddate = Request.QueryString["qenddate"];
                    if (senddate != null && senddate != "")
                    {
                        qenddate = DateTime.Parse(senddate);
                        TextBoxEndDate.Value = qenddate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        qenddate = DateTime.Now;
                        TextBoxEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                }
                 
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
            //this.pager2.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage_DT);
		}
		#endregion

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}

        public void ChangePage_DT(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            //pager2.CurrentPageIndex = e.NewPageIndex;
            //BindData_DT(e.NewPageIndex);
        }

		private void ValidateDate()
		{
            DateTime begindate, enddate;

            try
            {
                string s_date = TextBoxBeginDate.Value;
                if (s_date != null && s_date != "")
                {
                    begindate = DateTime.Parse(s_date);
                }
                string e_date = TextBoxEndDate.Value;
                if (e_date != null && e_date != "")
                {
                    enddate = DateTime.Parse(e_date);
                }
            }
			catch
			{
				throw new Exception("������������");
			}
            string buss_id = bussId.Text.Trim();
            string buss_name = bussName.Text.Trim();

            if (buss_id == "" && buss_name == "")
            {
                throw new Exception("�̻��Ż��̻����ƣ�����һ���Ϊ�գ�");
            }
		}

        public void btnQuery_Click(object sender, System.EventArgs e)
		{
			try
			{
				ValidateDate();
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

			try
			{
                clearDT();
                this.pager.RecordCount = 1000;
                BindData(1);
			}
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
			}
		}

        public void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e) 
        {
            object obj = e.Item.Cells[6].FindControl("lbChange");
            string s_state = e.Item.Cells[9].Text.Trim();//����״̬
            string s_optype = e.Item.Cells[10].Text.Trim();//����״̬
            if (obj != null)
            {
                LinkButton lb = (LinkButton)obj;
                if (s_state != "" && s_state == "3" && s_optype == "3")
                {
                    lb.Visible = true;
                }
                lb.Attributes["onClick"] = "return confirm('ȷ��Ҫִ�С����ͨ����������');";
            }
        }

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string spid = e.Item.Cells[2].Text.Trim(); //�̻���
            string sp_batchid = e.Item.Cells[1].Text.Trim();//�̻����κ�
            string batchid = e.Item.Cells[0].Text.Trim();//���κ�
            string import_time = e.Item.Cells[4].Text.Trim();//����ʱ��

            string total_count = e.Item.Cells[3].Text.Trim();//������
            string f_name = e.Item.Cells[7].Text.Trim();//�ļ���
            string veri_way = e.Item.Cells[8].Text.Trim();//��֤��ʽ
            
            switch (e.CommandName) 
            {
                case "DETAIL": //��ʾ��ϸ
                    GetDetail(spid, f_name);
                    break;
                case "CHANGE": //���ͨ��
                    ToChange(spid, sp_batchid, batchid, import_time, total_count, f_name, veri_way);
                    break;
            }
        }

        private void ToChange(string _spid, string _sp_batchid, string _batchid, string _importtime, string _totalcount, string _fname, string _veriway) 
        {
            try {
                if (_spid == "" && _sp_batchid == "")
                {
                    throw new Exception("�̻��Ż��̻����κŲ���Ϊ�գ�");
                }

                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
                qs.BatchWithholdReq(_spid, _sp_batchid,_batchid, _fname, int.Parse(_veriway), int.Parse(_totalcount), 1,4);

                TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend newMail = new TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend();
                string to_str = ConfigurationManager.AppSettings["dkProtocolEmailTo"];
                string time_str = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string cont_str = "���κţ�{0}������ʱ�䣺{1}�������ˣ�{2}������ʱ�䣺{3}��״̬��{4}";
                cont_str = String.Format(cont_str, _batchid, _importtime, Session["uid"].ToString(), time_str,"Ԥ�������");
                newMail.SendMail(to_str, "", "����Э��⵼������ͨ�����֪ͨ", cont_str, true, null);
                
                WebUtils.ShowMessage(this.Page, "���ͨ����");
                BindData(this.pager.CurrentPageIndex);
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, eSys.Message.ToString());
                return;
            }
        }

        private void GetDetail(string _spid, string _fname)
        {
            try {
                
                if (_spid == "")
                {
                    throw new Exception("�̻��Ų���Ϊ�գ�");
                }
                if (_fname == "")
                {
                    throw new Exception("�ļ�������Ϊ�գ�");
                }
                ViewState["spid"] = _spid;

                clearDT();
                //this.pager2.CurrentPageIndex = 1;
                //this.pager2.RecordCount = 1000;

                //BindData_DT(1);

                //���ļ��ж�ȡչʾ
                string path = Server.MapPath("~/") + "PLFile" + "\\" + _fname;
                FileInfo fi = new FileInfo(path);
                if (!fi.Exists)
                {
                    throw new Exception("�ļ������ڣ�" + fi.FullName);
                }
                if (Path.GetExtension(_fname).ToLower() == ".csv")
                {
                    DataSet res_ds = new DataSet(); //��װds
                    DataTable res_dt = new DataTable();
                    res_ds.Tables.Add(res_dt);
                    res_dt.Columns.Add("Fseq");
                    res_dt.Columns.Add("Fbuss_proto_flag");
                    res_dt.Columns.Add("Fbuss_user_flag");
                    res_dt.Columns.Add("Facc_att");
                    res_dt.Columns.Add("Fname");
                    res_dt.Columns.Add("Fphone");
                    res_dt.Columns.Add("Fopenacc_cer_type");
                    res_dt.Columns.Add("Fopenacc_cer_id"); //����֤������
                    res_dt.Columns.Add("Fbank_name");
                    res_dt.Columns.Add("Facc_type");
                    res_dt.Columns.Add("Fbank_acc");
                    res_dt.Columns.Add("Fprovince");
                    res_dt.Columns.Add("Fbranch_name");//֧������
                    res_dt.Columns.Add("Fcre_card_valid");
                    res_dt.Columns.Add("Fcre_card_cvv2");
                    res_dt.Columns.Add("Fcft_no");
                    res_dt.Columns.Add("Fbuss_code");
                    res_dt.Columns.Add("Fpay_type");
                    res_dt.Columns.Add("Fproto_start_time");
                    res_dt.Columns.Add("Fproto_end_time");
                    res_dt.Columns.Add("Fproto_brief_desc");
                    res_dt.Columns.Add("Fproto_detail_desc");

                    StreamReader reader = new StreamReader(path, System.Text.Encoding.Default);
                    int count = 0;
                    int total_count = 0; //�ܱ���
                    while (reader.Peek() > 0)
                    {
                        count++;
                        string str = reader.ReadLine();
                        if (str == "" || str.Contains("signature=")) 
                        {
                            continue;
                        }
                        if (count == 1 || count == 3)
                        {
                            continue;
                        }

                        string[] arr = str.Split(',');
                        if (count == 2)
                        {
                            continue;
                        }

                        DataRow res_dr = res_dt.NewRow();
                        res_dr.BeginEdit();

                        res_dr["Fseq"] = arr[0];
                        res_dr["Fbuss_proto_flag"] = arr[1];
                        res_dr["Fbuss_user_flag"] = arr[2];
                        res_dr["Facc_att"] = arr[3];
                        res_dr["Fname"] = arr[4];
                        res_dr["Fphone"] = arr[5];
                        res_dr["Fopenacc_cer_type"] = arr[6];
                        res_dr["Fopenacc_cer_id"] = arr[7];
                        res_dr["Fbank_name"] = arr[8];
                        res_dr["Facc_type"] = arr[9];
                        res_dr["Fbank_acc"] = arr[10];
                        res_dr["Fprovince"] = arr[11];
                        res_dr["Fbranch_name"] = arr[12];
                        res_dr["Fcre_card_valid"] = arr[13];
                        res_dr["Fcre_card_cvv2"] = arr[14];
                        res_dr["Fcft_no"] = arr[15];
                        res_dr["Fbuss_code"] = arr[16];
                        res_dr["Fpay_type"] = arr[17];
                        res_dr["Fproto_start_time"] = arr[18];
                        res_dr["Fproto_end_time"] = arr[19];
                        res_dr["Fproto_brief_desc"] = arr[20];
                        res_dr["Fproto_detail_desc"] = arr[21];

                        res_dr.EndEdit();
                        res_dt.Rows.Add(res_dr);
                        total_count++;
                    }

                    reader.Close();

                    DataGrid2.DataSource = res_dt.DefaultView;
                    DataGrid2.DataBind();
                }
                else if (Path.GetExtension(_fname).ToLower() == ".xls")
                {
                    DataSet res_ds = PublicRes.readXls(path);
                    DataTable res_dt = res_ds.Tables[0];
                    DataTable new_dt = new DataTable();
                    res_ds.Tables.Add(new_dt);
                    int iColums = res_dt.Columns.Count;
                    int iRows = res_dt.Rows.Count;

                    new_dt.Columns.Add("Fseq");
                    new_dt.Columns.Add("Fbuss_proto_flag");
                    new_dt.Columns.Add("Fbuss_user_flag");
                    new_dt.Columns.Add("Facc_att");
                    new_dt.Columns.Add("Fname");
                    new_dt.Columns.Add("Fphone");
                    new_dt.Columns.Add("Fopenacc_cer_type");
                    new_dt.Columns.Add("Fopenacc_cer_id"); //����֤������
                    new_dt.Columns.Add("Fbank_name");
                    new_dt.Columns.Add("Facc_type");
                    new_dt.Columns.Add("Fbank_acc");
                    new_dt.Columns.Add("Fprovince");
                    new_dt.Columns.Add("Fbranch_name");//֧������
                    new_dt.Columns.Add("Fcre_card_valid");
                    new_dt.Columns.Add("Fcre_card_cvv2");
                    new_dt.Columns.Add("Fcft_no");
                    new_dt.Columns.Add("Fbuss_code");
                    new_dt.Columns.Add("Fpay_type");
                    new_dt.Columns.Add("Fproto_start_time");
                    new_dt.Columns.Add("Fproto_end_time");
                    new_dt.Columns.Add("Fproto_brief_desc");
                    new_dt.Columns.Add("Fproto_detail_desc");

                    for (int i = 0; i < iRows; i++)
                    {
                        if (i == 0)
                        {
                            continue;
                        }
                        if (i == 1)
                        {
                            continue;
                        }
                        DataRow res_dr = new_dt.NewRow();

                        res_dr.BeginEdit();

                        res_dr["Fseq"] = res_dt.Rows[i][0].ToString();
                        res_dr["Fbuss_proto_flag"] = res_dt.Rows[i][1];
                        res_dr["Fbuss_user_flag"] = res_dt.Rows[i][2];
                        res_dr["Facc_att"] = res_dt.Rows[i][3];
                        res_dr["Fname"] = res_dt.Rows[i][4];
                        res_dr["Fphone"] = res_dt.Rows[i][5];
                        res_dr["Fopenacc_cer_type"] = res_dt.Rows[i][6];
                        res_dr["Fopenacc_cer_id"] = res_dt.Rows[i][7];// classLibrary.setConfig.ConvertID(res_dt.Rows[i][7].ToString(), 4, 2);
                        res_dr["Fbank_name"] = res_dt.Rows[i][8];
                        res_dr["Facc_type"] = res_dt.Rows[i][9];
                        res_dr["Fbank_acc"] = res_dt.Rows[i][10];
                        res_dr["Fprovince"] = res_dt.Rows[i][11];
                        res_dr["Fbranch_name"] = res_dt.Rows[i][12];
                        res_dr["Fcre_card_valid"] = res_dt.Rows[i][13];
                        res_dr["Fcre_card_cvv2"] = res_dt.Rows[i][14];
                        res_dr["Fcft_no"] = res_dt.Rows[i][15];
                        res_dr["Fbuss_code"] = res_dt.Rows[i][16];
                        res_dr["Fpay_type"] = res_dt.Rows[i][17];
                        res_dr["Fproto_start_time"] = res_dt.Rows[i][18];
                        res_dr["Fproto_end_time"] = res_dt.Rows[i][19];
                        res_dr["Fproto_brief_desc"] = res_dt.Rows[i][20];
                        res_dr["Fproto_detail_desc"] = res_dt.Rows[i][21];

                        res_dr.EndEdit();
                        new_dt.Rows.Add(res_dr);
                    }

                    DataGrid2.DataSource = new_dt.DefaultView;
                    DataGrid2.DataBind();
                }
                else
                {
                    throw new Exception("�ļ���ʽ����");
                }
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, eSys.Message.ToString());
                return;
            }
        }

        private void clearDT()
        {
            DataGrid2.DataSource = null;
            DataGrid2.DataBind();
        }

        private void BindData(int index)
		{
            DataGrid2.DataSource = null;
            DataGrid2.DataBind();

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

            string buss_id = bussId.Text.Trim();
            string buss_name = bussName.Text.Trim();

            int max = pager.PageSize;
            int start = max * (index - 1);

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
            DataSet ds = qs.QueryProtocolBatch(buss_id, buss_name, s_begindate, s_enddate, start, max);

			if(ds != null && ds.Tables.Count >0)
			{
                foreach (DataRow dr in ds.Tables[0].Rows) { 
                    
                }
                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
			}
			else
			{
                DataGrid1.DataSource = null;
                DataGrid1.DataBind();
                DataGrid2.DataSource = null;
                DataGrid2.DataBind();
                return;
			}
		}

        private void BindData_DT(int index)
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

            string buss_id = ViewState["spid"].ToString();

            //int max = pager2.PageSize;
            int max = 10;
            int start = max * (index - 1);

            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
            DataSet ds = qs.QueryProtocolDetail(buss_id, s_begindate, s_enddate, start, max);

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("Fbankacc_attr_str", typeof(String));
                ds.Tables[0].Columns.Add("Fcredit_type_str", typeof(String));
                ds.Tables[0].Columns.Add("Fbankacc_type_str", typeof(String));
                ds.Tables[0].Columns.Add("Fceppay_mode_str", typeof(String));
                
                Hashtable ht1 = new Hashtable();
                ht1.Add("0", "��˾");
                ht1.Add("1", "����");

                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fbankacc_attr", "Fbankacc_attr_str", ht1);
                ht1.Clear();

                ht1.Add("1", "���֤");
                ht1.Add("2", "����");
                ht1.Add("3", "����֤");
                ht1.Add("4", "ʿ��֤");
                ht1.Add("5", "����֤");
                ht1.Add("6", "��ʱ���֤");
                ht1.Add("7", "���ڲ�");
                ht1.Add("8", "����֤");
                ht1.Add("9", "̨��֤");
                ht1.Add("10", "Ӫҵִ��");
                ht1.Add("11", "����֤��");

                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fcredit_type", "Fcredit_type_str", ht1);
                ht1.Clear();

                ht1.Add("0", "���п�");
                ht1.Add("1", "����");
                ht1.Add("2", "���ÿ�");

                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fbankacc_type", "Fbankacc_type_str", ht1);
                ht1.Clear();

                ht1.Add("4", "�����˻�֧��");
                ht1.Add("8", "���ÿ��˻�֧��");

                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fceppay_mode", "Fceppay_mode_str", ht1);

                //cert
                foreach (DataRow dr in ds.Tables[0].Rows) {
                    string s_cret = classLibrary.setConfig.ConvertID(dr["Fcredit_id"].ToString(), 4, 2);
                    dr.BeginEdit();
                    dr["Fcredit_id"] = s_cret;
                    dr.EndEdit();
                }
                

                DataGrid2.DataSource = ds.Tables[0].DefaultView;
                DataGrid2.DataBind();
            }
            else
            {
                DataGrid2.DataSource = null;
                DataGrid2.DataBind();
                return;
            }
        }

	}
}