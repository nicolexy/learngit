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
	/// QueryFundInfoPage 的摘要说明。
	/// </summary>
	public partial class QueryFundInfoPage : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label rtnList;
		protected System.Web.UI.WebControls.Label lb_2;
		protected System.Web.UI.WebControls.Label lb_1;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			this.DataGrid_QueryResult.ItemCommand += new DataGridCommandEventHandler(DataGrid_QueryResult_ItemCommand);

			if(!this.IsPostBack)
			{
				this.tbx_beginDate.Value = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                this.tbx_endDate.Value = DateTime.Now.ToString("yyyy-MM-dd");

				this.pager.RecordCount = this.GetCount();
				this.pager.PageSize = 10;
			}
			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);
			try
			{
				if(Session["OperID"] != null)
					this.lb_operatorID.Text = Session["OperID"].ToString();
			
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				if (!ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}
			
		}

		private int GetCount()
		{
			return 10000;
		}


		private void StartQuery(int index)
		{
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
				WebUtils.ShowMessage(this,"日期格式不正确");
				return;
			}

			Query_Service.Query_Service queryService = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();


			queryService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

			DataSet queryResult = null;

			try
			{
				queryResult = queryService.GetFundInfo(this.tbx_1.Text.Trim(),this.tbx_2.Text.Trim(),beginDateStr,endDateStr,
					this.dd_type.SelectedValue,(index - 1) * this.pager.PageSize,this.pager.PageSize);

				string log = SensitivePowerOperaLib.MakeLog("get","","",this.tbx_1.Text.Trim(),this.tbx_2.Text.Trim(),beginDateStr,endDateStr,
					this.dd_type.SelectedValue,((index - 1) * this.pager.PageSize).ToString(),this.pager.PageSize.ToString());

				SensitivePowerOperaLib.WriteOperationRecord("InfoCenter",log,this);

				if(queryResult == null || queryResult.Tables.Count == 0 || queryResult.Tables[0].Rows.Count == 0)
				{
					this.ShowMsg("查询结果为空");
					this.DataGrid_QueryResult.DataSource = null;
					this.DataGrid_QueryResult.DataBind();
					return;
				}

				this.DataGrid_QueryResult.DataSource = queryResult.Tables[0];

				this.DataGrid_QueryResult.DataBind();

				if(this.tbx_1.Text.Trim() != "")
				{
					BindDetailInfo(queryResult);
				}
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
		}


		protected void btnQuery_Click(object sender, EventArgs e)
		{
			StartQuery(1);

			this.pager.CurrentPageIndex = 1;
		}


		private void ShowMsg(string msg)
		{
			Response.Write("<script language=javascript>alert('" + msg + "')</script>");
		}



		private void BindDetailInfo(DataSet ds)
		{
			this.lb_c1.Text = ds.Tables[0].Rows[0]["Flistid"].ToString();
			this.lb_c2.Text = ds.Tables[0].Rows[0]["Fspid"].ToString();
			this.lb_c3.Text = ds.Tables[0].Rows[0]["Fcoding"].ToString();

			this.lb_c4.Text = ds.Tables[0].Rows[0]["Ftrade_id"].ToString();
			this.lb_c5.Text = ds.Tables[0].Rows[0]["Ffund_name"].ToString();
			this.lb_c6.Text = ds.Tables[0].Rows[0]["Ffund_code"].ToString();

			this.lb_c7.Text = ds.Tables[0].Rows[0]["Fpur_type"].ToString();
			this.lb_c8.Text = ds.Tables[0].Rows[0]["Ftotal_fee"].ToString();
			//this.lb_c9.Text = ds.Tables[0].Rows[0]["Fbank_type"].ToString();

			this.lb_c9.Text = ds.Tables[0].Rows[0]["Fcard_no"].ToString();
			this.lb_c10.Text = ds.Tables[0].Rows[0]["Fstate"].ToString();
			this.lb_c11.Text = ds.Tables[0].Rows[0]["Flstate"].ToString();

			this.lb_c12.Text = ds.Tables[0].Rows[0]["Ftrade_date"].ToString();
			this.lb_c13.Text = ds.Tables[0].Rows[0]["Ffund_value"].ToString();
			this.lb_c14.Text = ds.Tables[0].Rows[0]["Ffund_vdate"].ToString();

			this.lb_c15.Text = ds.Tables[0].Rows[0]["Ffund_type"].ToString();
			this.lb_c16.Text = ds.Tables[0].Rows[0]["Frela_listid"].ToString();
			this.lb_c17.Text = ds.Tables[0].Rows[0]["Fcreate_time"].ToString();

			this.lb_c18.Text = ds.Tables[0].Rows[0]["Fmodify_time"].ToString();

			if(this.tbx_2.Text.Trim() != "")
			{
				this.lb_c19.Text = this.tbx_2.Text;
			}
			else
			{
				//this.lb_c19.Text = ds.Tables[0].Rows[0]["Ftrade_id"].ToString();
			}
		}



		private void DataGrid_QueryResult_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			Query_Service.Query_Service queryService = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			queryService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

			DataSet ds = queryService.GetFundInfo(e.Item.Cells[0].Text.Trim(),"","","","0",0,1);

			string log = SensitivePowerOperaLib.MakeLog("get","","",e.Item.Cells[0].Text.Trim(),"","","","0","0","1");

			SensitivePowerOperaLib.WriteOperationRecord("InfoCenter",log,this);

			if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
			{
				WebUtils.ShowMessage(this,"查询结果为空");
				return;
			}

			BindDetailInfo(ds);
		}



		private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			this.pager.CurrentPageIndex = e.NewPageIndex;

			this.StartQuery(e.NewPageIndex);
		}
	}
}
