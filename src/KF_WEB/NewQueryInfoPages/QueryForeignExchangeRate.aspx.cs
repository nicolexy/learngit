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
using CFT.CSOMS.BLL.ForeignCardModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
    /// QueryForeignExchangeRate 的摘要说明。
	/// </summary>
    public partial class QueryForeignExchangeRate : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
        public DateTime qbegindate, qenddate;

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
                    string sbegindate = Request.QueryString["qbegindate"];
                    if (sbegindate != null && sbegindate != "")
                    {
                        qbegindate = DateTime.Parse(sbegindate);
                        TextBoxBeginDate.Value = qbegindate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        qbegindate = DateTime.Now;
                        TextBoxBeginDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
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

            if (string.IsNullOrEmpty(TextBoxBeginDate.Value) || string.IsNullOrEmpty(TextBoxEndDate.Value))
            {
                throw new Exception("请输入日期！");
            }

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
				throw new Exception("日期输入有误！");
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

            string fore_Type = ddlForeType.SelectedValue.ToString();
            string issu_bank = ddlIssueBank.SelectedValue.ToString();

            int max = pager.PageSize;
            int start = max * (index - 1);
        
            DataSet ds =new ForeignCardService().GetExchangeRateList(fore_Type, issu_bank, s_begindate, s_enddate, start, max);

			if(ds != null && ds.Tables.Count >0)
			{
                ds.Tables[0].Columns.Add("Fexchg_flag_str", typeof(String));
                ds.Tables[0].Columns.Add("Feffect_flag_str", typeof(String));
                ds.Tables[0].Columns.Add("Fbank_type_str", typeof(String));
                ds.Tables[0].Columns.Add("Fcurrency_sell_str", typeof(String));
                ds.Tables[0].Columns.Add("Fcurrency_sell_tuned_str", typeof(String));

                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "外币/人民币");
                ht1.Add("2", "人民币/外币");

                Hashtable ht2 = new Hashtable();
                ht2.Add("1", "生效");
                ht2.Add("2", "不生效");

                Hashtable ht3 = new Hashtable();
                ht3.Add("4501", "建设银行");

                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fexchg_flag", "Fexchg_flag_str", ht1);
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Feffect_flag", "Feffect_flag_str", ht2);
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fbank_type", "Fbank_type_str", ht3);
                classLibrary.setConfig.RateToYuan_Table(ds.Tables[0], "Fcurrency_sell", "Fcurrency_sell_str");
                classLibrary.setConfig.RateToYuan_Table(ds.Tables[0], "Fcurrency_sell_tuned", "Fcurrency_sell_tuned_str");

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
			}
			else
			{
				throw new LogicException("没有找到记录！");
			}
		}

	}
}