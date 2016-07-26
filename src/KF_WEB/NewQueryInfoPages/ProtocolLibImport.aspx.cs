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
using System.Net;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
    /// <summary>
    /// ProtocolLibImport ��ժҪ˵����
	/// </summary>
    public partial class ProtocolLibImport : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
        protected void Page_Load(object sender, System.EventArgs e)
		{
            
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    
                }

                //Table2.Visible = false;
                //btnConfirm.Visible = false;
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
            //this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
            //this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

		}
		#endregion

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			//pager.CurrentPageIndex = e.NewPageIndex;
			//BindData(e.NewPageIndex);
		}

		private void ValidateDate()
		{
            string bussid = bussId.Text.Trim();
            //string bussname = bussName.Text.Trim();

            if (bussid == "")
            {
                throw new Exception("�̻��Ų���Ϊ�գ�");
            }
		}

        public void btnUpload_Click(object sender, System.EventArgs e)
		{
			try
			{
				ValidateDate();
                if (!File1.HasFile) {
                    throw new Exception("��ѡ���ϴ��ļ���");
                }
                string bussid = bussId.Text.Trim();
                DataGrid1.DataSource = null;
                if (Path.GetExtension(File1.FileName).ToLower() == ".csv")
                {
                    //����csv
                    ViewState["file_ext"] = ".csv";
                    string path = Server.MapPath("~/") + "PLFile" + "\\" + File1.FileName;
                    File1.PostedFile.SaveAs(path);
                    //ViewState["ftp_path"] = @path;

                    Table2.Visible = true;

                    //��װdataset
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
                    
                    StreamReader reader = new StreamReader(path,System.Text.Encoding.Default);
                    int count = 0;
                    int total_count = 0; //�ܱ���
                    while (reader.Peek() > 0) {
                        count++;
                        string str = reader.ReadLine();
                        if (count == 1 || count == 3) {
                            continue;
                        }
                        
                        string[] arr = str.Split(',');
                        if (count == 2)
                        {
                            //�������к�
                            ViewState["pz_seq"] = arr[0];
                            Fpz_seq.Text = arr[0];
                            Fproto_count.Text = arr[2];
                            Fpz_memo.Text = arr[1];
                            Fproto_veri.Text = arr[3];
                            ViewState["verify_way"] = arr[3];
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

                    //save file
                    string new_file = "CHAN8192_" + bussid + "_" + ViewState["pz_seq"].ToString() + ViewState["file_ext"].ToString();
                    path = Server.MapPath("~/") + "PLFile" + "\\" + new_file;
                    File1.PostedFile.SaveAs(path);
                    ViewState["ftp_path"] = @path;

                    ViewState["total_count"] = total_count;
                    if (total_count > 0) {
                        btnConfirm.Visible = true;
                    }
                    reader.Close();
                    
                    DataGrid1.DataSource = res_dt.DefaultView;
                    DataGrid1.DataBind();
                }
                else if (Path.GetExtension(File1.FileName).ToLower() == ".xls")
                {
                    //����xls
                    ViewState["file_ext"] = ".csv"; //��ʱ��д��csv
                    string path = Server.MapPath("~/") + "PLFile" + "\\" + File1.FileName;
                    File1.PostedFile.SaveAs(path);
                    //ViewState["ftp_path"] = @path;

                    Table2.Visible = true;

                    int total_count = 0; //�ܱ���

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

                    //��xlsת����csv
                    string s_pzno = res_dt.Rows[0][0].ToString();
                    if (s_pzno == null && s_pzno == "") 
                    {
                        //������κ�Ϊ��
                        WebUtils.ShowMessage(this.Page, "�ļ������κŲ���Ϊ�գ�");
                        return;
                    }
                    string new_file = "CHAN8192_" + bussid + "_" + s_pzno + ".csv";
                    path = Server.MapPath("~/") + "PLFile" + "\\" + new_file;
                    ViewState["ftp_path"] = @path;
                    FileStream fos = new FileStream(path,FileMode.Create);
                    StreamWriter sw = new StreamWriter(fos, System.Text.Encoding.Default);
                    string csv_cont = "�������к�,��Э����,����˵��,Э����֤��ʽ";
                    sw.WriteLine(csv_cont);
                    for (int i = 0; i < iRows; i++) 
                    {
                        if (i == 0) {
                            ViewState["pz_seq"] = res_dt.Rows[0][0].ToString();
                            Fpz_seq.Text = res_dt.Rows[0][0].ToString();
                            Fproto_count.Text = res_dt.Rows[0][1].ToString();
                            Fpz_memo.Text = res_dt.Rows[0][2].ToString();
                            Fproto_veri.Text = res_dt.Rows[0][3].ToString();
                            ViewState["verify_way"] = res_dt.Rows[0][3].ToString();

                            csv_cont = res_dt.Rows[0][0].ToString() + "," + res_dt.Rows[0][1].ToString() + "," + res_dt.Rows[0][2].ToString() + "," + res_dt.Rows[0][3].ToString();
                            sw.WriteLine(csv_cont);
                            csv_cont = "���,�̻���Э���ʶ,�̻����û���ʶ,�˻�����(��˾������),����/��˾����,�ֻ�����,����֤������,����֤������,��������,�˺�����(���п�/����/���ÿ�),�����˺�,ʡ������,֧������,���ÿ���Ч��,���ÿ�cvv2,�Ƹ�ͨ�˺�,ҵ�����,֧����ʽ,Э����ʼ��,Э�������,Э���Ҫ˵��,Э����ϸ˵��";
                            sw.WriteLine(csv_cont);

                            continue;
                        }
                        if (i == 1) {
                            continue;
                        }

                        csv_cont = res_dt.Rows[i][0].ToString() + "," + res_dt.Rows[i][1].ToString() + "," + res_dt.Rows[i][2].ToString() + "," + res_dt.Rows[i][3].ToString()+","
                                   + res_dt.Rows[i][4].ToString() + "," + res_dt.Rows[i][5].ToString() + "," + res_dt.Rows[i][6].ToString() + "," + res_dt.Rows[i][7].ToString() + ","
                                   + res_dt.Rows[i][8].ToString() + "," + res_dt.Rows[i][9].ToString() + "," + res_dt.Rows[i][10].ToString() + "," + res_dt.Rows[i][11].ToString() + ","
                                   + res_dt.Rows[i][12].ToString() + "," + res_dt.Rows[i][13].ToString() + "," + res_dt.Rows[i][14].ToString() + "," + res_dt.Rows[i][15].ToString() + ","
                                   + res_dt.Rows[i][16].ToString() + "," + res_dt.Rows[i][17].ToString() + "," + res_dt.Rows[i][18].ToString() + "," + res_dt.Rows[i][19].ToString() + ","
                                   + res_dt.Rows[i][20].ToString() + "," + res_dt.Rows[i][21].ToString();
                        sw.WriteLine(csv_cont);

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
                        total_count++;
                    }
                    sw.Flush();
                    sw.Close();
                    fos.Close();
                    //save file
                    //string new_file = "CHAN8192_" + bussid + "_" + ViewState["pz_seq"].ToString() + ".csv";
                   // path = Server.MapPath("~/") + "PLFile" + "\\" + new_file;
                    //File1.PostedFile.SaveAs(path);
                    

                    ViewState["total_count"] = total_count;
                    if (total_count > 0)
                    {
                        btnConfirm.Visible = true;
                    }
                    //����ȡ�����������
                    string prot_count = Fproto_count.Text;
                    if (prot_count == null || prot_count == "") {
                        prot_count = total_count.ToString();
                        Fproto_count.Text = prot_count;
                    }
                    DataGrid1.DataSource = new_dt.DefaultView;
                    DataGrid1.DataBind();
                }
                else 
                {
                    throw new Exception("��ѡ��csv��xls��ʽ���ļ���");
                }
                
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}
		}

        public void btnConfirm_Click(object sender, System.EventArgs e) 
        {
            try
            {
                //Table2.Visible = true;
                //�ϴ�FTP������cep_batchinfo_mng_service�ӿ�
                if (ViewState["ftp_path"] == null || ViewState["ftp_path"].ToString() == "")
                {
                    throw new Exception("�ļ������ڣ�");
                }

                string bussid = bussId.Text.Trim();
                if (bussid == "")
                {
                    throw new Exception("�̻��Ų���Ϊ�գ�");
                }
                if (ViewState["pz_seq"] == null || ViewState["pz_seq"].ToString() == "")
                {
                    throw new Exception("�ļ��е��������кŲ���Ϊ�գ�");
                }

                FileInfo fi = new FileInfo(ViewState["ftp_path"].ToString());
                if (!fi.Exists)
                {
                    throw new Exception("�ļ������ڣ�" + fi.FullName);
                }
                
                string new_file = "CHAN8192_" + bussid + "_" + ViewState["pz_seq"].ToString() + ViewState["file_ext"].ToString();
                //pz_seq=20120425001;ftp_path=D:\\kefuWorks\\KF_WEB\\PLFile\\protocol_template.csv;total_count=2
               /* string new_file = "CHAN8192_2000000501_20130617121.csv";
                ViewState["ftp_path"] = "D:\\kefuWorks\\KF_WEB\\PLFile\\protocol_template(1).csv";
                ViewState["verify_way"] = "������";
                ViewState["total_count"] = "2";
                ViewState["pz_seq"] = "20130617121";
                string bussid = "2000000501"; */
                
                //�ȶ��ļ�����д����
                if (ViewState["file_ext"].ToString() == ".csv") 
                {
                    //��MD5д���ļ�
                    FileStream fs = new FileStream(ViewState["ftp_path"].ToString(),FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.Write("\r\n"); //\r\n��ȥ����MD5�����Դ˴�������
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }
                
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
                string ret_md5 = qs.QueryKpsMD5(bussid, bussid, ViewState["ftp_path"].ToString());
                if (ret_md5 == "")
                {
                    throw new Exception("δȡ���ļ�MD5ֵ��");
                }

                if (ViewState["file_ext"].ToString() == ".csv")
                {
                    //��MD5д���ļ�
                    FileStream fs = new FileStream(ViewState["ftp_path"].ToString(), FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.Write("signature=" + ret_md5); //\r\n��ȥ����MD5�����Դ˴�������
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }
                else if (ViewState["file_ext"].ToString() == ".xls")
                {
                    string cont_xls = "signature=" + ret_md5;
                    PublicRes.writeXls(ViewState["ftp_path"].ToString(), cont_xls);
                }
                else
                {
                    throw new Exception("�ļ����ʹ���");
                }

                //�ϴ��ļ���FTP
                string dk_ftp_ip = ConfigurationManager.AppSettings["dkFtpIP"];
                string dk_ftp_uname = ConfigurationManager.AppSettings["dkFtpUname"];
                string dk_ftp_pwd = ConfigurationManager.AppSettings["dkFtpPwd"];
                int ret = PublicRes.FtpUploadFile(dk_ftp_ip, dk_ftp_uname, dk_ftp_pwd,ViewState["ftp_path"].ToString(), new_file);
                //int ret = 0;
                if (ret == 0) {
                    //���ýӿ�
                    string s_way = ViewState["verify_way"].ToString();
                    int i_way = 1;
                    if (s_way == "������֤")
                    {
                        i_way = 1;
                    }
                    else if (s_way == "����ǩԼ")
                    {
                        i_way = 2;
                    }
                    else if (s_way == "������")
                    {
                        i_way = 4;
                    }
                    else if (s_way == "�������")
                    {
                        i_way = 4;
                    }
                    int i_total = int.Parse(ViewState["total_count"].ToString());
                    
                    DataSet ds =  qs.BatchWithholdReq(bussid, ViewState["pz_seq"].ToString(),"", new_file, i_way, i_total, 1, 1);
                    if (ds != null)
                    {
                        string s_result = ds.Tables[0].Rows[0]["result"].ToString();
                        if (s_result == "0")
                        {
                            btnConfirm.Visible = false;
                            WebUtils.ShowMessage(this.Page, "�����ɹ���");
                        }
                    }
                    else { 
                        throw new Exception("����ʧ�ܣ�");
                    }
                }
            }
            catch (Exception ferr) {
                WebUtils.ShowMessage(this.Page, ferr.Message);
                return;
            } 
        }
	}
}