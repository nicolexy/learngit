using System;
using System.Collections;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.SpSettle
{
	/// <summary>
    /// OrderInfoQuery ��ժҪ˵����
	/// </summary>
    public partial class OrderInfoQuery : System.Web.UI.Page
	{
    
		protected void Page_Load(object sender, System.EventArgs e)
		{
            // �ڴ˴������û������Գ�ʼ��ҳ��
            rq_tb2.Visible = false;
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());

				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
		}

        protected void Button_qry_Click(object sender, System.EventArgs e) 
        {
            try
            {
                string merge_listid = this.txtMergeListid.Text.Trim();
                string listid = this.txtListid.Text.Trim();

                if (merge_listid == "" && listid == "")
                    throw new Exception("�������ѯ������");

                BindInfo(merge_listid, listid);
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, eSys.Message.ToString());
            }
        }

        private void BindInfo(string merge_listid, string listid)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds;
            ds = qs.QuerySubOrderList(merge_listid, listid);

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                ViewState["g_dt"] = dt;

                dt.Columns.Add("Ftype_str", typeof(string)); //ҵ������
                dt.Columns.Add("Fpay_type_str", typeof(string));//֧������
                dt.Columns.Add("Fcurtype_str", typeof(string)); //����
                dt.Columns.Add("Ftrade_state_str", typeof(string)); //����״̬
                dt.Columns.Add("Frefund_state_str", typeof(string)); //�˿�״̬
                dt.Columns.Add("Flstate_str", typeof(string)); //����״̬
                dt.Columns.Add("Fchannel_id_str", typeof(string)); //������

                dt.Columns.Add("Fprice_str", typeof(string)); //��Ʒ�۸�
                dt.Columns.Add("Fcarriage_str", typeof(string)); //��������
                dt.Columns.Add("Fpaynum_str", typeof(string)); //ʵ��֧������
                dt.Columns.Add("Ffact_str", typeof(string)); //��֧������
                dt.Columns.Add("Fprocedure_str", typeof(string)); //����������
                dt.Columns.Add("Fcash_str", typeof(string)); //�ֽ�֧�����
                dt.Columns.Add("Ftoken_str", typeof(string)); //����ȯ֧�����
                dt.Columns.Add("Ffee3_str", typeof(string)); //��������
                dt.Columns.Add("Fpaybuy_str", typeof(string)); //�˻�����ҽ��
                dt.Columns.Add("Fpaysale_str", typeof(string)); //�˻������ҽ��

                dt.Columns.Add("Fbuy_bank_type_str", typeof(string)); //��ҿ�����
                dt.Columns.Add("Fsale_bank_type_str", typeof(string)); //���ҿ�����

                foreach (DataRow dr in ds.Tables[0].Rows) 
                {
                    dr["Fbuy_bank_type_str"] = TENCENT.OSS.C2C.Finance.BankLib.BankIO.QueryBankName(dr["Fbuy_bank_type"].ToString());
                    dr["Fsale_bank_type_str"] = TENCENT.OSS.C2C.Finance.BankLib.BankIO.QueryBankName(dr["Fsale_bank_type"].ToString());
                }
                
                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "����֧��");
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Ftype", "Ftype_str", ht1);
                ht1.Clear();

                ht1.Add("1", "���п�֧��");
                ht1.Add("2", "�Ƹ�֧ͨ��");
                ht1.Add("3", "һ��֧ͨ��");
                ht1.Add("4", "���ÿ�֧��");
                ht1.Add("5", "ί�д���");
                ht1.Add("6", "���֧��");
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fpay_type", "Fpay_type_str", ht1);
                ht1.Clear();

                ht1.Add("1", "RMB");
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fcurtype", "Fcurtype_str", ht1);
                ht1.Clear();

                ht1.Add("1", "�ȴ����֧��");
                ht1.Add("2", "֧���ɹ�/�ȴ����ҷ���");
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Ftrade_state", "Ftrade_state_str", ht1);
                ht1.Clear();

                ht1.Add("0", "��ʼ״̬");
                ht1.Add("1", "�˿�����");
                ht1.Add("2", "�˿�ɹ�");
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Frefund_state", "Frefund_state_str", ht1);
                ht1.Clear();

                ht1.Add("1", "����");
                ht1.Add("2", "����");
                ht1.Add("3", "����");
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Flstate", "Flstate_str", ht1);
                ht1.Clear();

                ht1.Add("1", "�Ƹ�ͨ");
                ht1.Add("2", "������");
                ht1.Add("3", "�ͷ���СǮ��");
                ht1.Add("4", "�ֻ�֧��");
                ht1.Add("5", "������");
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fchannel_id", "Fchannel_id_str", ht1);
                ht1.Clear();

                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fprice", "Fprice_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fcarriage", "Fcarriage_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fpaynum", "Fpaynum_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Ffact", "Ffact_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fprocedure", "Fprocedure_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fcash", "Fcash_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Ftoken", "Ftoken_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Ffee3", "Ffee3_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fpaybuy", "Fpaybuy_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fpaysale", "Fpaysale_str");

                
                this.DataGrid1.DataSource = dt.DefaultView;
                this.DataGrid1.DataBind();
            }
            else 
            {
                throw new Exception("û�м�¼��");
            }
            
        }

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string listid = e.Item.Cells[0].Text.Trim(); //�Ƹ�ͨ������

            switch (e.CommandName)
            {
                case "DETAIL": //������ϸ
                    int rid = e.Item.ItemIndex;
                    rq_tb2.Visible = true;
                    GetReqDetail(rid);
                    break;
            }
        }

        private void GetReqDetail(int rid) 
        {
            clearDT();
            DataTable g_dt = (DataTable)ViewState["g_dt"];
            if (g_dt != null) 
            {
                lb_c1.Text = g_dt.Rows[rid]["Fmerge_listid"].ToString();//ԭ���׵�
                lb_c2.Text = g_dt.Rows[rid]["Ftype_str"].ToString();//ҵ������
                lb_c3.Text = g_dt.Rows[rid]["Flistid"].ToString();//����֧����
                lb_c4.Text = g_dt.Rows[rid]["Fcoding"].ToString();//��������
                lb_c5.Text = g_dt.Rows[rid]["Fspid"].ToString();//�̻���
                lb_c6.Text = g_dt.Rows[rid]["Fbank_listid"].ToString();//�����еĶ�����
                lb_c7.Text = g_dt.Rows[rid]["Fbank_backid"].ToString();//���з��صĶ�����
                lb_c8.Text = g_dt.Rows[rid]["Fpay_type_str"].ToString();//֧������
                lb_c9.Text = g_dt.Rows[rid]["Fbuy_uid"].ToString();//����ڲ�ID
                lb_c10.Text = g_dt.Rows[rid]["Fbuyid"].ToString();//����˻���

                lb_c11.Text = g_dt.Rows[rid]["Fbuy_name"].ToString();//�������
                lb_c12.Text = g_dt.Rows[rid]["Fbuy_bank_type_str"].ToString();//��ҿ�����
                lb_c13.Text = g_dt.Rows[rid]["Fsale_uid"].ToString();//�����ڲ�ID
                lb_c14.Text = g_dt.Rows[rid]["Fsaleid"].ToString();//�����˻���
                lb_c15.Text = g_dt.Rows[rid]["Fsale_name"].ToString();//���ҵ�����
                lb_c16.Text = g_dt.Rows[rid]["Fsale_bank_type_str"].ToString();//���ҿ�����
                lb_c17.Text = g_dt.Rows[rid]["Fcurtype_str"].ToString();//����
                lb_c18.Text = g_dt.Rows[rid]["Ftrade_state_str"].ToString();//����״̬
                lb_c19.Text = g_dt.Rows[rid]["Frefund_state_str"].ToString();//�˿�/����״̬
                lb_c20.Text = g_dt.Rows[rid]["Flstate_str"].ToString();//����״̬

                lb_c21.Text = g_dt.Rows[rid]["Fprice_str"].ToString();//��Ʒ�۸�
                lb_c22.Text = g_dt.Rows[rid]["Fcarriage_str"].ToString();//��������
                lb_c23.Text = g_dt.Rows[rid]["Fpaynum_str"].ToString();//ʵ��֧������
                lb_c24.Text = g_dt.Rows[rid]["Ffact_str"].ToString();//��֧������
                lb_c25.Text = g_dt.Rows[rid]["Fprocedure_str"].ToString();//����������
                lb_c26.Text = g_dt.Rows[rid]["Fservice"].ToString();//�������
                lb_c27.Text = g_dt.Rows[rid]["Fcash_str"].ToString();//�ֽ�֧�����
                lb_c28.Text = g_dt.Rows[rid]["Ftoken_str"].ToString();//����ȯ֧�����
                lb_c29.Text = g_dt.Rows[rid]["Ffee3_str"].ToString();//��������

                lb_c30.Text = g_dt.Rows[rid]["Fcreate_time"].ToString();//��������ʱ��
                lb_c31.Text = g_dt.Rows[rid]["Fpay_time"].ToString();//��Ҹ���ʱ��
                lb_c32.Text = g_dt.Rows[rid]["Fip"].ToString();//����޸Ľ��׵���IP
                lb_c33.Text = g_dt.Rows[rid]["Fmemo"].ToString();//����˵��
                lb_c34.Text = g_dt.Rows[rid]["Fexplain"].ToString();//��ע
                lb_c35.Text = g_dt.Rows[rid]["Fmodify_time"].ToString();//�޸�ʱ��
                lb_c36.Text = g_dt.Rows[rid]["Fchannel_id_str"].ToString();//����
                lb_c37.Text = g_dt.Rows[rid]["Fpaybuy_str"].ToString();//�˻�����ҵĽ��
                lb_c38.Text = g_dt.Rows[rid]["Fpaysale_str"].ToString();//�˻������ҵĽ��
            }
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
            lb_c17.Text = "";
            lb_c18.Text = "";
            lb_c19.Text = "";

            lb_c20.Text = "";
            lb_c21.Text = "";
            lb_c22.Text = "";
            lb_c23.Text = "";
            lb_c24.Text = "";
            lb_c25.Text = "";
            lb_c26.Text = "";
            lb_c27.Text = "";
            lb_c28.Text = "";
            lb_c29.Text = "";

            lb_c30.Text = "";
            lb_c31.Text = "";
            lb_c32.Text = "";
            lb_c33.Text = "";
            lb_c34.Text = "";
            lb_c35.Text = "";
            lb_c36.Text = "";
            lb_c37.Text = "";
            lb_c38.Text = "";
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
        }
		#endregion
	}
}
