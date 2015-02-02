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

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// Summary description for MobileRechargeQuery.
	/// </summary>
	public partial class MobileRechargeQuery : System.Web.UI.Page
	{
		
		string strBeginDate = "",strEndDate = "";

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				this.tbx_beginDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日");
				this.tbx_endDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
			}
			this.btnBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
			this.btnEndDate.Attributes.Add("onclick","openModeEnd()");
		}


		protected void btn_query_Click(object sender, System.EventArgs e)
		{
			BindData(1);
		}
		

		private bool validate()
		{
			if(tbx_acc.Text.Trim() == string.Empty)
			{
				ShowMsg("请输入用户账号！");
				return false;
			}
			try
			{
				if(this.tbx_beginDate.Text.Trim() != "" && this.tbx_endDate.Text.Trim() != "")
				{
					strBeginDate = DateTime.Parse(this.tbx_beginDate.Text).ToString("yyyy-MM-dd");
					strEndDate = DateTime.Parse(this.tbx_endDate.Text).AddDays(1).ToString("yyyy-MM-dd");
				}
				return true;
			}
			catch
			{
				ShowMsg("日期格式不正确！");
				return false;;
			}
		}


		private void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}


		private void BindData(int index)
		{
			if(!validate())
			{
				return;
			}
			
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataSet ds = qs.QueryMobilRecharge(tbx_acc.Text.Trim(), strBeginDate, strEndDate, index);
			if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
			{
				this.ShowMsg("查询记录为空。");
			}
			else
			{
				ds.Tables[0].Columns.Add("ProviderName",typeof(string));
				foreach(DataRow row in ds.Tables[0].Rows)
				{
					try
					{
						int type = Convert.ToInt32(row["Fcard_type"]);
						switch(type)
						{
							case 1:
								row["ProviderName"] = "移动";
								break;
							case 2:
								row["ProviderName"] = "联通";
								break;
							case 3:
								row["ProviderName"] = "电信";
								break;
						}
					}
					catch
					{}
				}
			}
			this.DataGrid_QueryResult.DataSource = ds;
			this.DataGrid_QueryResult.DataBind();
		}


		private void ShowMsg(string msg)
		{
			Response.Write("<script language=javascript>alert('" + msg + "')</script>");
		}


		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			pager.RecordCount = 1000;
			pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(ChangePage);
		}
		#endregion
	}
}
