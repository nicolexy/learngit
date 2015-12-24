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

namespace TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay
{
	/// <summary>
    /// QueryYTTrade 的摘要说明。
	/// </summary>
    public partial class FCASettlementQuery : System.Web.UI.Page
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
				throw new Exception("日期输入有误！");
			}
            string spid = txtspid.Text.ToString();
            if (spid == "")
            {
                throw new Exception("请输入商户编号！");
            }
            if (string.IsNullOrEmpty(s_date) || string.IsNullOrEmpty(e_date))
            {
                throw new Exception("请输入日期！");
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
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message.ToString());
			}
		}

        private void BindData(int index)
        {
            try
            {
                this.pager.CurrentPageIndex = index;
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

                string spid = txtspid.Text.ToString();

                int max = pager.PageSize;
                int start = max * (index - 1);
                DataSet ds= new DataSet();
                ds = FCBLLService.QueryMerSettlement(spid,s_begindate, s_enddate,start, max);
                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    DataGrid1.DataSource = null;
                    DataGrid1.DataBind();
                    throw new Exception("数据库无此记录");
                }
                ds.Tables[0].Columns.Add("Fcur_type_str"); //交易币种
                ds.Tables[0].Columns.Add("Fpay_amout_str"); //支付金额
                ds.Tables[0].Columns.Add("Fpay_count"); //交易笔数
                ds.Tables[0].Columns.Add("Fref_amout_str"); //退款金额
                ds.Tables[0].Columns.Add("Fref_amout"); //退款笔数
                ds.Tables[0].Columns.Add("Fcycins_freeze_amount_str");//循保冻结金额
                ds.Tables[0].Columns.Add("Fdue_amount_str");//应付金额
                ds.Tables[0].Columns.Add("Fsettle_cur_type_str"); //结算币种
                ds.Tables[0].Columns.Add("Ffee_amount_str"); //手续费
                ds.Tables[0].Columns.Add("Fsettle_amount_str"); //结算金额
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fpay_amout", "Fpay_amout_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fref_amout", "Fref_amout_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Ffee_amount", "Ffee_amount_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fsettle_amount", "Fsettle_amount_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fcycins_freeze_amount", "Fcycins_freeze_amount_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fdue_amount", "Fdue_amount_str");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string cur_type = row["Fcur_type"].ToString();  //交易币种
                    if (InternetBankDictionary.CurTypeAIMark.ContainsKey(cur_type))
                    {
                        row["Fcur_type_str"] = InternetBankDictionary.CurTypeAIMark[cur_type];
                    }
                    else
                    {
                        row["Fcur_type_str"] = cur_type;
                    }

                    cur_type = row["Fsettle_cur_type"].ToString();  //结算币种
                    if (InternetBankDictionary.CurTypeAIMark.ContainsKey(cur_type))
                    {
                        row["Fsettle_cur_type_str"] = InternetBankDictionary.CurTypeAIMark[cur_type];
                    }
                    else
                    {
                        row["Fsettle_cur_type_str"] = cur_type;
                    }
                }
                DataGrid1.DataSource = ds;
                DataGrid1.DataBind();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }


        }


	}
}