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
	/// QueryInverestorSignPage 的摘要说明。
	/// </summary>
	public partial class QueryInverestorSignPage : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lb_1;
		protected System.Web.UI.WebControls.Label lb_2;
		protected System.Web.UI.WebControls.Label lb_3;
		protected System.Web.UI.WebControls.TextBox tbx_5;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			ButtonBeginDate.Attributes.Add("onclick","openModeBegin()"); 
			ButtonEndDate.Attributes.Add("onclick","openModeEnd()");

			this.DataGrid_QueryResult.ItemCommand += new DataGridCommandEventHandler(DataGrid_QueryResult_ItemCommand);
			this.dd_querySubject.SelectedIndexChanged += new EventHandler(dd_querySubject_SelectedIndexChanged);

			if(!this.IsPostBack)
			{
				this.tbx_beginDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日");
				this.tbx_endDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
			}

			this.pager.RecordCount = GetCount();
			this.pager.PageSize = 10;
			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);

			try
			{
				if(Session["OperID"] != null)
					this.lb_operatorID.Text = Session["OperID"].ToString();
			
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());
				if(!ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
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
		
		public void StartQuery(int index)
		{
			Query_Service.Query_Service query = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			int signType = this.dd_querySubject.SelectedIndex + 1;

			try
			{
				if(this.rtnList.Checked)
				{
					query.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

					DataSet queryResult = query.GetInversatorSignDetail(signType,"",this.tbx_tradeID.Text.Trim(),"","","","",""
						,(index - 1) * this.pager.PageSize,this.pager.PageSize);

					string log = SensitivePowerOperaLib.MakeLog("get","","",signType.ToString(),"",this.tbx_tradeID.Text.Trim()
						,"","","","","",((index-1)*this.pager.PageSize).ToString(),this.pager.PageSize.ToString());

					SensitivePowerOperaLib.WriteOperationRecord("InfoCenter",log,this);

					this.DataGrid_QueryResult.DataSource = queryResult;
					this.DataGrid_QueryResult.DataBind();
				}
				else
				{
					string beginDateStr = "";
					string endDateStr = "";

					try
					{
						if(this.tbx_beginDate.Text.Trim() != "")
							beginDateStr = DateTime.Parse(this.tbx_beginDate.Text).ToString("yyyy-MM-dd");

						if(this.tbx_endDate.Text.Trim() != "")
							endDateStr = DateTime.Parse(this.tbx_endDate.Text).ToString("yyyy-MM-dd");	
					}
					catch
					{
						beginDateStr = endDateStr = "";
					}

					//query.Finance_HeaderValue = setConfig.setFH(Session["OperID"].ToString(),Request.UserHostAddress);

					query.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

					DataSet queryResult = query.GetInversatorSignDetail(signType,this.tbx_1.Text.Trim(),tbx_tradeID.Text.Trim()
						,this.tbx_3.Text.Trim(),this.tbx_spid.Text.Trim(),this.tbx_4.Text.Trim(),beginDateStr,endDateStr,
						(index-1)*this.pager.PageSize,this.pager.PageSize);

					string log = SensitivePowerOperaLib.MakeLog("get","","",signType.ToString(),this.tbx_1.Text.Trim(),tbx_tradeID.Text.Trim()
						,this.tbx_3.Text.Trim(),this.tbx_spid.Text.Trim(),this.tbx_4.Text.Trim(),beginDateStr,endDateStr,
						((index-1)*this.pager.PageSize).ToString(),this.pager.PageSize.ToString());

					SensitivePowerOperaLib.WriteOperationRecord("InfoCenter",log,this);

					if(queryResult == null || queryResult.Tables.Count == 0 || queryResult.Tables[0].Rows.Count == 0)
					{
						ShowMsg("查询结果为空");
						this.DataGrid_QueryResult.DataSource = null;
						this.DataGrid_QueryResult.DataBind();
						return;
					}

					this.DataGrid_QueryResult.DataSource = queryResult;
					this.DataGrid_QueryResult.DataBind();
				}
			}
			catch(Exception ex)
			{
				this.ShowMsg(ex.Message);
			}
		}


		protected void btnQuery_Click(object sender, EventArgs e)
		{
			StartQuery(1);

			this.pager.CurrentPageIndex = 1;
		}



		private void BindDetail(DataSet ds)
		{
			this.lb_c1.Text = ds.Tables[0].Rows[0]["Fid"].ToString();
			this.lb_c2.Text = ds.Tables[0].Rows[0]["Fqqid"].ToString();
			this.lb_c3.Text = ds.Tables[0].Rows[0]["Fcur_typeName"].ToString();

			this.lb_c4.Text = ds.Tables[0].Rows[0]["Fspid"].ToString();
			this.lb_c5.Text = ds.Tables[0].Rows[0]["Fsp_uname"].ToString();
			this.lb_c6.Text = ds.Tables[0].Rows[0]["Fchannel_idName"].ToString();

			this.lb_c7.Text = ds.Tables[0].Rows[0]["FdirectName"].ToString();
			this.lb_c8.Text = ds.Tables[0].Rows[0]["Fcft_serialno"].ToString();
			this.lb_c9.Text = ds.Tables[0].Rows[0]["Fbind_channelName"].ToString();

			this.lb_c10.Text = ds.Tables[0].Rows[0]["Fop_typeName"].ToString();
            
            //20130827 lxl数据库有存cft|2004的格式
            string koukuan_bank = ds.Tables[0].Rows[0]["Fbank_type_new"].ToString();
            string[] bank = koukuan_bank.Split('|');
            if (bank.Length == 1&& bank[0] == "cft")
            {
                this.lb_c11.Text = "使用余额支付";
            }
            else if (bank.Length == 2 && bank[0] == "cft")
            {
                this.lb_c11.Text = "使用余额支付|" + getData.GetBankNameFromBankCode(bank[1]);
            }
            else
            {
                this.lb_c11.Text = getData.GetBankNameFromBankCode(ds.Tables[0].Rows[0]["Fbank_type_new"].ToString());
            }
            
			//this.lb_c12.Text = ds.Tables[0].Rows[0]["Fbind_status_new"].ToString();

			this.lb_c12.Text = ds.Tables[0].Rows[0]["Fcard_tail_new"].ToString();
			this.lb_c13.Text = ds.Tables[0].Rows[0]["Fcreate_time"].ToString();
			this.lb_c14.Text = ds.Tables[0].Rows[0]["Funchain_channelName"].ToString();
		}


		private void DataGrid_QueryResult_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			Query_Service.Query_Service query = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			//query.Finance_HeaderValue = setConfig.setFH(Session["OperID"].ToString(),Request.UserHostAddress);

			query.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

			int signType = this.dd_querySubject.SelectedIndex + 1;
			string serino = e.Item.Cells[0].Text;

			DataSet ds = query.GetInversatorSignDetail(signType,"",serino,"","","","","",0,1);

			string log = SensitivePowerOperaLib.MakeLog("get","","",signType.ToString(),"",serino,"","","","","","0","1");

			SensitivePowerOperaLib.WriteOperationRecord("InfoCenter",log,this);

			if(ClassLib.IsDataSetNull(ds))
			{
				WebUtils.ShowMessage(this,"查询结果为空");
				return;
			}

			BindDetail(ds);
		}

		private void dd_querySubject_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.lb_pageTitle.Text = this.dd_querySubject.SelectedItem.Text;
		}


		private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			this.pager.CurrentPageIndex = e.NewPageIndex;

			StartQuery(e.NewPageIndex);
		}


		private void ShowMsg(string msg)
		{
			Response.Write("<script language=javascript>alert('" + msg + "')</script>");
		}
	}
}
