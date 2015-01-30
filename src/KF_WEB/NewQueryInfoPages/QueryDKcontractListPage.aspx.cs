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
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
	/// QueryDKcontractListPage 的摘要说明。
	/// </summary>
	public partial class QueryDKcontractListPage : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面

			if(!IsPostBack)
			{
				this.tbx_beginDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
				this.tbx_endDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");

				this.ButtonBeginDate.Attributes.Add("onclick","openModeBegin()");
				this.ButtonEndDate.Attributes.Add("onclick","openModeEnd()");

				
				this.pager.PageSize = 10;
				this.pager.RecordCount = GetCount();

				if(Request.QueryString["spid"] != null || Request.QueryString["spbatchid"] != null)
				{
					if(Request.QueryString["spid"] != null)
						this.tbx_spid.Text = Request.QueryString["spid"].Trim();

					if(Request.QueryString["spbatchid"] != null)
						this.tbx_sp_batchid.Text = Request.QueryString["spbatchid"].Trim();

					if(Request.QueryString["sDate"] != "")
						this.tbx_beginDate.Text = Request.QueryString["sDate"];

					if(Request.QueryString["eDate"] != "")
						this.tbx_endDate.Text = Request.QueryString["eDate"];

					BindData(1,true);
				}

			}


			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);
			this.DataGrid_QueryResult.ItemCommand += new DataGridCommandEventHandler(DataGrid_QueryResult_ItemCommand);
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

		}
		#endregion


	
		private int GetCount()
		{
			return 1000;
		}



		private void BindData(int pageIndex,bool needUpdateCount)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			//qs.Finance_HeaderValue = setConfig.setFH(Session["OperID"].ToString(),Request.UserHostAddress);

			qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

			try
			{
				DateTime sTime ,eTime;
				string strSTime,strETime;
				try
				{
					sTime = DateTime.Parse(this.tbx_beginDate.Text);
					eTime = DateTime.Parse(this.tbx_endDate.Text);

					strSTime = sTime.ToString("yyyy-MM-dd HH:mm:ss");
					strETime = eTime.ToString("yyyy-MM-dd HH:mm:ss");					
				}
				catch
				{
					WebUtils.ShowMessage(this,"日期格式不正确");
					return;
				}

				DataSet ds =qs.QueryDKContractBatchList(tbx_spid.Text.Trim(),tbx_sp_batchid.Text.Trim(),strSTime,strETime,
					(pageIndex - 1) * this.pager.PageSize,this.pager.PageSize);

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					WebUtils.ShowMessage(this,"查询结果为空");
					this.Clear();
					return;
				}

				ds.Tables[0].Columns.Add("Ftotal_count_URL",typeof(string));
				ds.Tables[0].Columns.Add("Fsuc_count_URL",typeof(string));
				ds.Tables[0].Columns.Add("Ffail_count_url",typeof(string));
				ds.Tables[0].Columns.Add("Fhandle_count_url",typeof(string));

				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					dr["Ftotal_count_URL"] = "./QueryDKcontractPage.aspx?spid=" + dr["Fspid"].ToString() 
						+ "&spbatchid=" + dr["Fsp_batchid"].ToString() + "&sDate=" + dr["Fcreate_time"] + "&eDate=" + dr["Fmodify_time"];

					dr["Fsuc_count_URL"] = "./QueryDKcontractPage.aspx?spid=" + dr["Fspid"].ToString() 
						+ "&spbatchid=" + dr["Fsp_batchid"].ToString() + "&state=s" + "&sDate=" + dr["Fcreate_time"] + "&eDate=" + dr["Fmodify_time"];

					dr["Ffail_count_url"] = "./QueryDKcontractPage.aspx?spid=" + dr["Fspid"].ToString() 
						+ "&spbatchid=" + dr["Fsp_batchid"].ToString() + "&state=f" + "&sDate=" + dr["Fcreate_time"] + "&eDate=" + dr["Fmodify_time"];

					dr["Fhandle_count_url"] = "./QueryDKcontractPage.aspx?spid=" + dr["Fspid"].ToString() 
						+ "&spbatchid=" + dr["Fsp_batchid"].ToString() + "&state=h" + "&sDate=" + dr["Fcreate_time"] + "&eDate=" + dr["Fmodify_time"];

				}

				

				this.DataGrid_QueryResult.DataSource = ds;
				this.DataGrid_QueryResult.DataBind();
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
		}



		protected void btn_serach_Click(object sender, System.EventArgs e)
		{
			BindData(1,true);

			this.pager.CurrentPageIndex = 1;
		}

		private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			this.pager.CurrentPageIndex = e.NewPageIndex;

			BindData(e.NewPageIndex,false);
		}


		private void Clear()
		{
			this.lb_c1.Text = "";
			this.lb_c2.Text = "";
			this.lb_c3.Text = "";
			this.lb_c4.Text = "";
			this.lb_c5.Text = "";
			this.lb_c6.Text = "";
			this.lb_c7.Text = "";
			this.lb_c8.Text = "";
			this.lb_c9.Text = "";
			this.lb_c10.Text = "";
			this.lb_c11.Text = "";
			this.lb_c12.Text = "";
			this.lb_c13.Text = "";
			this.lb_c14.Text = "";
			this.lb_c15.Text = "";
			this.lb_c16.Text = "";
			this.lb_c17.Text = "";
			this.lb_c18.Text = "";
			this.lb_c19.Text = "";
			this.lb_c20.Text = "";
			this.lb_c21.Text = "";
			this.lb_c22.Text = "";
			this.lb_c23.Text = "";
			this.lb_c24.Text = "";
			

			Label1.Text = "";
			Label2.Text = "";

			this.DataGrid_QueryResult.DataSource = null;
			this.DataGrid_QueryResult.DataBind();
		}



		private DataSet QueryDKContractBatchDetail(string batchid)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

			return qs.QueryDKContractBatchDetail(batchid);
		}


		private void DataGrid_QueryResult_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			if(e.CommandName == "detail")
			{
				DataSet ds = QueryDKContractBatchDetail(e.Item.Cells[0].Text.Trim());

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					WebUtils.ShowMessage(this,"查询结果为空");
					Clear();
					return;
				}

				DataRow dr = ds.Tables[0].Rows[0];

				lb_c1.Text = dr["Fspid"].ToString();
				lb_c2.Text = dr["Fsp_uid"].ToString();
				lb_c3.Text = dr["Fsp_batchid"].ToString();
				lb_c4.Text = dr["Fbatchid"].ToString();
				
				lb_c5.Text = dr["FstateName"].ToString();
				lb_c6.Text = dr["Ftotal_count"].ToString();
				lb_c7.Text = dr["Fsucc_count"].ToString();
				lb_c8.Text = dr["Ffail_count"].ToString();

				if(dr["Frcd_type"].ToString() == "1")
					lb_c9.Text = "单笔";
				else
					lb_c9.Text = "批量";
				lb_c10.Text = dr["Fchannel"].ToString();
				lb_c11.Text = dr["Fclass"].ToString();
				lb_c12.Text = dr["Ffname"].ToString();

				lb_c13.Text = dr["Fcreate_time"].ToString();
				lb_c14.Text = dr["Fmodify_time"].ToString();
				lb_c15.Text = dr["Flast_retcode"].ToString();
				lb_c16.Text = dr["Flast_retinfo"].ToString();

				string tmp = dr["Flstate"].ToString();
				if(tmp == "1")
					lb_c17.Text = "有效";
				else if(tmp == "2")
					lb_c17.Text = "吊销";
				else if(tmp == "3")
					lb_c17.Text = "作废";
				lb_c18.Text = dr["Fcnr_nofity_url"].ToString();
				lb_c19.Text = dr["Fmemo"].ToString();
				lb_c20.Text = dr["Fexplain"].ToString();

				lb_c21.Text = dr["Foper_id"].ToString();
				lb_c22.Text = dr["Foper_ip"].ToString();
				lb_c23.Text = dr["Fexamer_id"].ToString();
				lb_c24.Text = dr["Fexamer_ip"].ToString();

				Label1.Text = dr["Fclient_ip"].ToString();
				Label2.Text = dr["Fmodify_ip"].ToString();
				
			}
		}
	}
}
