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

namespace TENCENT.OSS.CFT.KF.KF_Web.CreidtPay
{
	/// <summary>
    /// QueryCreditBillList 的摘要说明。yinhuang 2013/11/25
	/// </summary>
    public partial class QueryCreditBillList : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
        protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    //TextBoxBeginDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");

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
            this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);
            this.pager2.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage2);
		}
		#endregion

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}
        public void ChangePage2(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager2.CurrentPageIndex = e.NewPageIndex;
            BindDetail(e.NewPageIndex);
        }

		private void ValidateDate()
		{
			
            string ccftno = cftNo.Text.ToString();
           
            if (ccftno == "" )
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
                clearDT();
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

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            ViewState["cur_month"] = e.Item.Cells[0].Text.ToString();  //查询详情用的月份
            BindDetail(1);
        }

        private void clearDT() 
        {
            
        }

        private void BindData(int index)
		{
         
            DataGrid2.DataSource = null;
            DataGrid2.DataBind();

            pager.CurrentPageIndex = index;
            string s_cftno = cftNo.Text.ToString();
            ViewState["cur_uin"] = s_cftno;

            int max = pager.PageSize;
			int start = max * (index-1);

            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds = qs.QueryCreditBillList(s_cftno, "", start+1, max);
            
            if(ds != null && ds.Tables.Count >0)
            {
                //要对月份数据进行筛选

                ds.Tables[0].Columns.Add("consume_amount_str", typeof(String));//当期消费金额
                ds.Tables[0].Columns.Add("instal_prin_str", typeof(String));//分期金额

                ds.Tables[0].Columns.Add("type_str", typeof(String));//账单标识
                ds.Tables[0].Columns.Add("instal_flag_str", typeof(String));//是否分期

                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "当期");
                ht1.Add("2", "往期");
                ht1.Add("3", "未出账");
                Hashtable ht2 = new Hashtable();
                ht2.Add("1", "未分期");
                ht2.Add("2", "已分期");

                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "consume_amount", "consume_amount_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "instal_prin", "instal_prin_str");

                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "type", "type_str", ht1);
                classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "instal_flag", "instal_flag_str", ht2);

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
            }
            else
            {
                DataGrid1.DataSource = null;
                DataGrid2.DataSource = null;
                throw new LogicException("没有找到记录！");
            }
        }
        
        private void BindDetail(int index) 
        {
            try
            {
                pager2.CurrentPageIndex = index;
                string curUin = ViewState["cur_uin"].ToString();
                if (string.IsNullOrEmpty(curUin))
                {
                    throw new Exception("账号为空");
                }

                string curMonth = ViewState["cur_month"].ToString();
                if (string.IsNullOrEmpty(curMonth)) 
                {
                    throw new Exception("月份为空");
                }

                int max = pager2.PageSize;
                int start = max * (index - 1);

                this.pager2.RecordCount = 1000;

                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                DataSet ds = qs.QueryCreditBillDetail(curUin, curMonth, start, max);

                if (ds != null && ds.Tables.Count > 0)
                {

                    ds.Tables[0].Columns.Add("prin_str", typeof(String));//单期本金
                    ds.Tables[0].Columns.Add("fee_str", typeof(String));//单期手续费

                    ds.Tables[0].Columns.Add("status_str", typeof(String));//状态

                    Hashtable ht1 = new Hashtable();
                    ht1.Add("0", "无分期");
                    ht1.Add("1", "未还");
                    ht1.Add("2", "已还");
                    ht1.Add("3", "提前还");

                    classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "prin", "prin_str");
                    classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "fee", "fee_str");

                    classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "status", "status_str", ht1);

                    DataGrid2.DataSource = ds.Tables[0].DefaultView;
                    DataGrid2.DataBind();
                }
                else
                {
                    DataGrid2.DataSource = null;
                }
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务2出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据2失败！" + eSys.Message.ToString());
            }
        }
        
	}
}