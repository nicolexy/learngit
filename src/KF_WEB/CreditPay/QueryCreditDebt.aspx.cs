using System;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.CreditPay
{
	/// <summary>
    /// QueryCreditDebt ��ժҪ˵����yinhuang 2013/11/25
	/// </summary>
    public partial class QueryCreditDebt : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
        protected void Page_Load(object sender, System.EventArgs e)
		{
            
			try
            {
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("PayManagement", this))
                {
                    Response.Redirect("../login.aspx?wh=1");
                }
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
			//this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

		}
		#endregion

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{

		}

		private void ValidateDate()
		{
            string ccftno = cftNo.Text.ToString();

            if (ccftno == "")
            {
                throw new Exception("�Ƹ�ͨ�˺Ų���Ϊ�գ�");
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
                //this.pager.RecordCount = 1000;
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

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds = qs.QueryCreditDebt(cft_no);

			if(ds != null && ds.Tables.Count >0)
			{
                ds.Tables[0].Columns.Add("account_balance_str", typeof(String));  //Ƿ���ܶ�
                ds.Tables[0].Columns.Add("cur_return_total_amt_str", typeof(String));  //����Ӧ���ܶ�
                ds.Tables[0].Columns.Add("remain_total_prin_str", typeof(String));  //δ����Ƿ����

                ds.Tables[0].Columns.Add("cur_remain_total_amt_str", typeof(String));  //�˵����
                ds.Tables[0].Columns.Add("cur_return_instal_prin_str", typeof(String));  //���ڽ��
                ds.Tables[0].Columns.Add("ovd_return_prin_str", typeof(String));  //���ڽ��
                ds.Tables[0].Columns.Add("ovd_fee_str", typeof(String));  //������Ϣ

                ds.Tables[0].Columns.Add("unpay_total_pur_amt_str", typeof(String));  //���ѽ��
                ds.Tables[0].Columns.Add("cur_return_instal_prin_tmp_str", typeof(String));  //���ڽ��
                ds.Tables[0].Columns.Add("cur_return_instal_fee_tmp_str", typeof(String));  //����������

                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "account_balance", "account_balance_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "cur_return_total_amt", "cur_return_total_amt_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "remain_total_prin", "remain_total_prin_str");

                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "cur_remain_total_amt", "cur_remain_total_amt_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "cur_return_instal_prin", "cur_return_instal_prin_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "ovd_return_prin", "ovd_return_prin_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "ovd_fee", "ovd_fee_str");

                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "unpay_total_pur_amt", "unpay_total_pur_amt_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "cur_return_instal_prin_tmp", "cur_return_instal_prin_tmp_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "cur_return_instal_fee_tmp", "cur_return_instal_fee_tmp_str");

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid2.DataSource = ds.Tables[0].DefaultView;
                DataGrid3.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
                DataGrid2.DataBind();
                DataGrid3.DataBind();
			}
			else
			{
                DataGrid1.DataSource = null;
                DataGrid2.DataSource = null;
                DataGrid3.DataSource = null;
				throw new LogicException("û���ҵ���¼��");
			}
		}

	}
}