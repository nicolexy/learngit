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
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;

using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
	/// QueryChargeInfoPage 的摘要说明。
	/// </summary>
	public partial class QueryChargeInfoPage : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		protected System.Web.UI.WebControls.Label lb_1;
		protected System.Web.UI.WebControls.TextBox tbx_findDate;
		protected System.Web.UI.WebControls.Label lb_c17;
		protected System.Web.UI.WebControls.Label lb_c18;
		protected System.Web.UI.WebControls.Label Label1_Fqqid;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				if(Session["OperID"] != null)
					this.lb_operatorID.Text = Session["OperID"].ToString();
				if(!ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!this.IsPostBack)
			{
				this.tbx_beginDate.Value = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                this.tbx_endDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
				this.pager.RecordCount = GetCount();
				this.pager.PageSize = 10;
			}

			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);
			this.DataGrid_QueryResult.ItemCommand +=new DataGridCommandEventHandler(DataGrid_QueryResult_ItemCommand);
		}

		private int GetCount()
		{
			return 10000;
		}
		
		public void StartQuery(int index)
		{
			if(this.tbx_1.Text.Trim() == "")
			{
				this.ShowMsg("用户帐号不能为空");

				return;
			}

			Query_Service.Query_Service query = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			query.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

			string beginDateStr = "";
			string endDateStr = "";

			try
			{
                if (this.tbx_beginDate.Value.Trim() != "")
                    beginDateStr = DateTime.Parse(this.tbx_beginDate.Value).ToString("yyyy-MM-dd");

                if (this.tbx_endDate.Value.Trim() != "")
                    endDateStr = DateTime.Parse(this.tbx_endDate.Value).ToString("yyyy-MM-dd");
			}
			catch	
			{
				this.ShowMsg("日期格式不正确，搜索失败!");

				return;
			}

			DataSet queryResult = null;

			try
			{
				queryResult = query.GetChargeInfo(this.dd_queryType.SelectedValue,this.tbx_1.Text.Trim(),beginDateStr
					,endDateStr,this.tbx_3.Text.Trim(),(index- 1) * this.pager.PageSize,this.pager.PageSize);

				if(queryResult == null || queryResult.Tables.Count == 0 || queryResult.Tables[0].Rows.Count == 0)
				{
					this.ShowMsg("查询结果为空");
					this.DataGrid_QueryResult.DataSource = null;
					this.DataGrid_QueryResult.DataBind();

					this.Datagrid1.DataSource = null;
					this.Datagrid1.DataBind();
					return;
				}

				this.DataGrid_QueryResult.DataSource = queryResult;
				this.DataGrid_QueryResult.DataBind();

				this.Datagrid1.DataSource = queryResult;
				this.Datagrid1.DataBind();
			}
			catch(Exception ex)
			{
				this.ShowMsg(ex.Message);
				return;
			}
		}


		protected void btnQuery_Click(object sender,EventArgs e)
		{
			StartQuery(1);

			this.pager.CurrentPageIndex = 1;
		}


		private void ShowMsg(string msg)
		{
			Response.Write("<script language=javascript>alert('" + msg + "')</script>");
		}


		private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			this.pager.CurrentPageIndex = e.NewPageIndex;

			StartQuery(e.NewPageIndex);
		}

	
		private void BindDetail(DataSet ds)
		{
			this.lb_c1.Text = ds.Tables[0].Rows[0]["Flistid"].ToString();
			this.lb_c2.Text = ds.Tables[0].Rows[0]["Fspid"].ToString();

			this.lb_c3.Text = ds.Tables[0].Rows[0]["Fqqid"].ToString();
		//	this.lb_c4.Text = ds.Tables[0].Rows[0]["Ftrue_name"].ToString();
			this.lb_c5.Text = ds.Tables[0].Rows[0]["FtypeName"].ToString();

			this.lb_c6.Text = ds.Tables[0].Rows[0]["FsubjectName"].ToString();
			this.lb_c7.Text = ds.Tables[0].Rows[0]["Ffromid"].ToString();
			this.lb_c8.Text = ds.Tables[0].Rows[0]["Ffrom_name"].ToString();

		//	this.lb_c9.Text = ds.Tables[0].Rows[0]["Fbalance"].ToString();
			this.lb_c10.Text = ds.Tables[0].Rows[0]["Fpaynum"].ToString();
			this.lb_c11.Text = ds.Tables[0].Rows[0]["Fbank_typeName"].ToString();

			this.lb_c12.Text = ds.Tables[0].Rows[0]["Fcurtype"].ToString();
			this.lb_c13.Text = ds.Tables[0].Rows[0]["Fip"].ToString();
			this.lb_c14.Text = ds.Tables[0].Rows[0]["Fmodify_time"].ToString();

			this.lb_c15.Text = ds.Tables[0].Rows[0]["Fmemo"].ToString();
			this.lb_c16.Text = ds.Tables[0].Rows[0]["Fcreate_time"].ToString();
		}


		private void BindDetail(int index)
		{
			this.lb_c1.Text = DataGrid_QueryResult.Items[index].Cells[1].Text;
			this.lb_c2.Text = DataGrid_QueryResult.Items[index].Cells[2].Text;

			this.lb_c3.Text = DataGrid_QueryResult.Items[index].Cells[3].Text;
		//	this.lb_c4.Text = Datagrid1.Items[index].Cells[0].Text;
			this.lb_c5.Text = DataGrid_QueryResult.Items[index].Cells[4].Text;

			this.lb_c6.Text = DataGrid_QueryResult.Items[index].Cells[5].Text;
			this.lb_c7.Text = DataGrid_QueryResult.Items[index].Cells[6].Text;


			this.lb_c8.Text = Datagrid1.Items[index].Cells[1].Text;

		//	this.lb_c9.Text = Datagrid1.Items[index].Cells[2].Text;
			this.lb_c10.Text = Datagrid1.Items[index].Cells[3].Text;
			this.lb_c11.Text = Datagrid1.Items[index].Cells[4].Text;

			this.lb_c12.Text = Datagrid1.Items[index].Cells[5].Text;
			this.lb_c13.Text = Datagrid1.Items[index].Cells[6].Text;
			this.lb_c14.Text = Datagrid1.Items[index].Cells[7].Text;

			this.lb_c15.Text = Datagrid1.Items[index].Cells[8].Text;
			this.lb_c16.Text = Datagrid1.Items[index].Cells[9].Text;
		}


		private void DataGrid_QueryResult_ItemCommand(object source, DataGridCommandEventArgs e)
		{		
			BindDetail(e.Item.ItemIndex);
		}
	}
}
