using System;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService;
using System.Collections;

namespace TENCENT.OSS.CFT.KF.KF_Web.CreditPay
{
	/// <summary>
    /// QueryCapitalRoll ��ժҪ˵����yinhuang 2014/05/15
	/// </summary>
    public partial class QueryCapitalRoll : System.Web.UI.Page
	{
        protected void Page_Load(object sender, System.EventArgs e)
		{
            
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    TextBoxBeginDate.Value = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    TextBoxEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
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

		}
		#endregion

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}

		private void ValidateDate()
		{
            DateTime begindate, enddate;
            try
            {
                string s_date = TextBoxBeginDate.Value;
                string e_date = TextBoxEndDate.Value;
                
                begindate = DateTime.Parse(s_date);
                enddate = DateTime.Parse(e_date);
            }
            catch
            {
                throw new Exception("������������");
            }
            if (begindate.CompareTo(enddate) > 0)
            {
                throw new Exception("��ֹ����С����ʼ���ڣ����������룡");
            }
            
            string ccftno = cftNo.Text.ToString();
            if (ccftno == "")
            {
                throw new Exception("������Ƹ�ͨ�˺ţ�");
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

        private void BindData(int index)
		{
            string cft_no = cftNo.Text.ToString();

            string s_stime = TextBoxBeginDate.Value;
            string s_begindate = "";
            if (s_stime != null && s_stime != "")
            {
                DateTime begindate = DateTime.Parse(s_stime);
                s_begindate = begindate.ToString("yyyyMMdd");
            }
            string s_etime = TextBoxEndDate.Value;
            string s_enddate = "";
            if (s_etime != null && s_etime != "")
            {
                DateTime enddate = DateTime.Parse(s_etime);
                s_enddate = enddate.ToString("yyyyMMdd");
            }

            int max = pager.PageSize;
            int start = max * (index - 1);

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds = qs.QueryCapitalRoll(cft_no, s_begindate, s_enddate, start, max);

			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count >0)
			{
                ViewState["g_dt"] = ds.Tables[0];
                ds.Tables[0].Columns.Add("txn_amt_str", typeof(String));
                ds.Tables[0].Columns.Add("txn_prin_str", typeof(String));
                ds.Tables[0].Columns.Add("txn_ovd_fee_str", typeof(String));
                ds.Tables[0].Columns.Add("txn_fee_str", typeof(String));
                ds.Tables[0].Columns.Add("uin", typeof(String));
                ds.Tables[0].Columns.Add("txn_type_str", typeof(String)); //��������
                ds.Tables[0].Columns.Add("txn_dist_str", typeof(String)); //��������

                Hashtable ht1 = new Hashtable();
                ht1.Add("11", "����");
                ht1.Add("12", "�����ֵ��δ�ṩ��");
                ht1.Add("14", "���ڿ���");
                ht1.Add("21", "�������ɷ�");
                ht1.Add("22", "��������������");
                ht1.Add("23", "������ֵ�����ѣ�δ�ṩ��");
                ht1.Add("31", "���ɷ�����");
                ht1.Add("41", "�˻������˻������");
                ht1.Add("51", "����");
                ht1.Add("52", "�˻�");
                ht1.Add("53", "���ѿ����ת");
                ht1.Add("61", "���ɷѼ���");
                ht1.Add("62", "���ɷ����");
                ht1.Add("63", "��ȡ��ֵ�����ѣ�δ�ṩ��");

                Hashtable ht2 = new Hashtable();
                ht2.Add("-", "��");
                ht2.Add("+", "��");

                foreach (DataRow dr in ds.Tables[0].Rows) 
                {
                    dr["uin"] = cft_no;
                }

                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "txn_dist", "txn_dist_str", ht1);
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "txn_type", "txn_type_str", ht2);

                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "txn_amt", "txn_amt_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "txn_prin", "txn_prin_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "txn_ovd_fee", "txn_ovd_fee_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "txn_fee", "txn_fee_str");

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
			}
			else
			{
                DataGrid1.DataSource = null;
                DataGrid1.DataBind();
			}
		}

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {

            int rid = e.Item.ItemIndex;
            GetDetail(rid);
        }

        private void GetDetail(int rid)
        {
            //��Ҫע���ҳ���
            clearDT();
            DataTable g_dt = (DataTable)ViewState["g_dt"];
            if (g_dt != null)
            {
                lb_c1.Text = g_dt.Rows[rid]["billno"].ToString();//����ID
                lb_c2.Text = g_dt.Rows[rid]["txn_type_str"].ToString();//��������
                lb_c3.Text = g_dt.Rows[rid]["bank_billno"].ToString();//���ж�����
                lb_c4.Text = g_dt.Rows[rid]["txn_dist_str"].ToString();//��������
                lb_c5.Text = g_dt.Rows[rid]["txn_amt_str"].ToString();//���׽��
                lb_c6.Text = g_dt.Rows[rid]["txn_date"].ToString();//��������
                lb_c7.Text = g_dt.Rows[rid]["txn_prin_str"].ToString(); //����
                lb_c8.Text = g_dt.Rows[rid]["txn_time"].ToString(); ;//����ʱ��
                lb_c9.Text = g_dt.Rows[rid]["txn_ovd_fee_str"].ToString(); ;//���ɷ�
                lb_c10.Text = g_dt.Rows[rid]["txn_fee_str"].ToString(); //����
                lb_c11.Text = g_dt.Rows[rid]["desc"].ToString();//��ע
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
        }
	}
}