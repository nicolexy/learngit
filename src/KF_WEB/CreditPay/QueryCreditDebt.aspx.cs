using System;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.CreditPay
{
	/// <summary>
    /// QueryCreditDebt 的摘要说明。yinhuang 2013/11/25
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

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
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
                throw new Exception("财付通账号不能为空！");
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
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message.ToString());
			}
		}

        private void BindData(int index)
		{
            string cft_no = cftNo.Text.ToString();

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds = qs.QueryCreditDebt(cft_no);

			if(ds != null && ds.Tables.Count >0)
			{
                ds.Tables[0].Columns.Add("account_balance_str", typeof(String));  //欠款总额
                ds.Tables[0].Columns.Add("cur_return_total_amt_str", typeof(String));  //本期应还总额
                ds.Tables[0].Columns.Add("remain_total_prin_str", typeof(String));  //未出账欠款金额

                ds.Tables[0].Columns.Add("cur_remain_total_amt_str", typeof(String));  //账单金额
                ds.Tables[0].Columns.Add("cur_return_instal_prin_str", typeof(String));  //分期金额
                ds.Tables[0].Columns.Add("ovd_return_prin_str", typeof(String));  //逾期金额
                ds.Tables[0].Columns.Add("ovd_fee_str", typeof(String));  //逾期利息

                ds.Tables[0].Columns.Add("unpay_total_pur_amt_str", typeof(String));  //消费金额
                ds.Tables[0].Columns.Add("cur_return_instal_prin_tmp_str", typeof(String));  //分期金额
                ds.Tables[0].Columns.Add("cur_return_instal_fee_tmp_str", typeof(String));  //分期手续费

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
				throw new LogicException("没有找到记录！");
			}
		}

	}
}