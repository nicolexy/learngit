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
    public partial class FCATransferQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
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
                    if (!classLibrary.ClassLib.ValidateRight("SPInfoManagement", this)) Response.Redirect("../login.aspx?wh=1");
                }
                this.btnQuery.Attributes.Add("onclick", "return CheckEmail();");
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

                string spid = txtspid.Text.ToString();

                int max = pager.PageSize;
                int start = max * (index - 1);
                DataSet ds = new DataSet();
                ds = FCBLLService.QueryMerTransfer(spid, s_begindate, s_enddate, start, max);
                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    DataGrid1.DataSource = null;
                    DataGrid1.DataBind();
                    throw new Exception("数据库无此记录");
                }
                ds.Tables[0].Columns.Add("Fcur_type_str"); //币种
                ds.Tables[0].Columns.Add("Fsettle_amount_str"); //结算净金额A
                ds.Tables[0].Columns.Add("Fcycins_unfreeze_amout_str"); //循保释放B
                ds.Tables[0].Columns.Add("Ffixins_freeze_amount_str"); //缴纳固保C
                ds.Tables[0].Columns.Add("Frefuse_unfreeze_amount_str"); //拒付解冻D
                ds.Tables[0].Columns.Add("Frefuse_freeze_amount_str"); //拒付冻结E
                ds.Tables[0].Columns.Add("huakuan_str"); //划款金额M=A+B-C+D-E
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fsettle_amount", "Fsettle_amount_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fcycins_unfreeze_amout", "Fcycins_unfreeze_amout_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Ffixins_freeze_amount", "Ffixins_freeze_amount_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Frefuse_unfreeze_amount", "Frefuse_unfreeze_amount_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Frefuse_freeze_amount", "Frefuse_freeze_amount_str");
                
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string cur_type = row["Fcur_type"].ToString();
                    if (InternetBankDictionary.CurTypeAIMark.ContainsKey(cur_type))
                    {
                        row["Fcur_type_str"] = InternetBankDictionary.CurTypeAIMark[cur_type];
                    }
                    else
                    {
                        row["Fcur_type_str"] = cur_type;
                    }
                    long a=long.Parse(row["Fsettle_amount"].ToString());
                    long b = long.Parse(row["Fcycins_unfreeze_amout"].ToString());
                    long c = long.Parse(row["Ffixins_freeze_amount"].ToString());
                    long d = long.Parse(row["Frefuse_unfreeze_amount"].ToString());
                    long e = long.Parse(row["Frefuse_freeze_amount"].ToString());
                    row["huakuan_str"] = (a + b - c + d - e).ToString();
                }
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fsettle_amount", "Fsettle_amount_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "huakuan_str", "huakuan_str");
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