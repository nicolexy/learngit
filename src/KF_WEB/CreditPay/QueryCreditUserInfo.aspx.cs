using System;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.Common;
using System.Collections;

namespace TENCENT.OSS.CFT.KF.KF_Web.CreditPay
{
	/// <summary>
    /// QueryCreditUserInfo ��ժҪ˵����yinhuang 2013/11/25
	/// </summary>
    public partial class QueryCreditUserInfo : System.Web.UI.Page
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
		}
		#endregion

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			//BindData(e.NewPageIndex);
		}

		private void ValidateDate()
		{
            string ccftno = cftNo.Text.ToString();
            
            if (ccftno == "")
            {
                throw new Exception("����������һ����ѯ�");
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
        }

        private void BindData(int index)
		{
            string s_cftno = cftNo.Text.ToString();
            
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds = qs.QueryCreditUserInfo(s_cftno);
            
            if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("baseline_total_str", typeof(String));//�ܶ��
                ds.Tables[0].Columns.Add("baseline_rest_str", typeof(String));//���ö��
                ds.Tables[0].Columns.Add("baseline_used_str", typeof(String));//���ö��
                ds.Tables[0].Columns.Add("tmpline_total_str", typeof(String));//��ʱ���
                ds.Tables[0].Columns.Add("return_balance_str", typeof(String));//����ʽ�

                ds.Tables[0].Columns.Add("bank_acc_status_str", typeof(String));//�˻�״̬
                ds.Tables[0].Columns.Add("bank_creline_status_str", typeof(String));//���״̬

                Hashtable ht1 = new Hashtable();
                ht1.Add("10", "����");
                ht1.Add("22", "����");
                ht1.Add("23", "����");
                ht1.Add("24", "����");
                ht1.Add("25", "����");
                ht1.Add("81", "����-�˹�����");
                ht1.Add("89", "����-����ȫ�ջ�");
                ht1.Add("90", "�ѽ���");
                ht1.Add("1N", "δԤ����");

                Hashtable ht2 = new Hashtable();
                ht2.Add("10", "����");
                ht2.Add("21", "��ʱͣ��(�ͻ�����)");
                ht2.Add("22", "����(���ڣ�ϵͳ�Զ��趨)");
                ht2.Add("23", "����(�ڲ���Ա�˹��趨)");
                ht2.Add("24", "����(��;��ˣ�ϵͳ�Զ��趨)");
                ht2.Add("81", "�ѹ��������ޣ���ʧЧ");
                ht2.Add("82", "�ѹ�ʹ�����ޣ���ʧЧ");
                ht2.Add("90", "��ע��");

                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "baseline_total", "baseline_total_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "baseline_rest", "baseline_rest_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "baseline_used", "baseline_used_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "tmpline_total", "tmpline_total_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "return_balance", "return_balance_str");

                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "bank_acc_status", "bank_acc_status_str", ht1);
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "bank_creline_status", "bank_creline_status_str", ht2);

                DataRow dr = ds.Tables[0].Rows[0];
                lb_c1.Text = dr["Fqqid"].ToString();  //�˺�
                lb_c2.Text = dr["Ftruename"].ToString();  //����
                if (!(dr["Fcredit"] is DBNull))
                {
                    if (dr["Fcredit"].ToString() != "") 
                    {
                        lb_c3.Text = classLibrary.setConfig.ConvertID(dr["Fcredit"].ToString(), 4, 2); //���֤��
                    }
                }

                lb_c4.Text = dr["Fmobile"].ToString();  //�ֻ�����
                lb_c5.Text = dr["Femail"].ToString();  //����

                if (!(dr["Fcredit_result"] is DBNull)) 
                {
                    if (dr["Fcredit_result"].ToString() == "Y")
                    {
                        lb_c6.Text = "���ųɹ�";  //���Ž��
                    }
                    else 
                    {
                        lb_c6.Text = "����ʧ��";  //���Ž��
                    }
                }

                
                lb_c7.Text = dr["baseline_total_str"].ToString();  //�ܶ��

              
                if (!(dr["activeFlag"] is DBNull)) 
                {
                    string s = dr["activeFlag"].ToString();
                    if (string.IsNullOrEmpty(s) || s == "2")
                    {
                        lb_c8.Text = "δ����";
                    }
                    else 
                    {
                        lb_c8.Text = "�Ѽ���";
                    }
                }

                lb_c9.Text = dr["baseline_rest_str"].ToString();  //���ö��
                lb_c10.Text = dr["activate_date"].ToString();  //��������
                lb_c11.Text = dr["baseline_used_str"].ToString();  //���ö��
                lb_c12.Text = dr["bank_acc_status_str"].ToString();  //�˻�״̬
                lb_c13.Text = dr["tmpline_total_str"].ToString();  //��ʱ���

                if (!(dr["Fret_date"] is DBNull))
                {
                    string s_date = dr["Fret_date"].ToString();
                    if (string.IsNullOrEmpty(s_date) || s_date == "0")
                    {
                        lb_c14.Text = s_date;
                    }
                    else 
                    {
                        int billdate = int.Parse(dr["Fret_date"].ToString()) - 7;
                        lb_c14.Text = billdate.ToString();//�˵���
                    }
                }

                lb_c15.Text = dr["Fline_expdate"].ToString();  //�����Ч��
                lb_c16.Text = dr["Fret_date"].ToString();  //������
                lb_c17.Text = dr["bank_creline_status_str"].ToString();  //���״̬
                lb_c18.Text = dr["return_balance_str"].ToString();
            }
            else
            {
                throw new LogicException("û���ҵ���¼��");
            }
        }

	}
}