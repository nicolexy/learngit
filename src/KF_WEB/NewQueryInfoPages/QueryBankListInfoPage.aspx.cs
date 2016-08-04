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
	/// QueryBankListInfoPage 的摘要说明。
	/// </summary>
	public partial class QueryBankListInfoPage : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面

			if(!IsPostBack)
			{
				this.tbx_beginDate.Value = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
				this.tbx_endDate.Value = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");

				this.pager.PageSize = 10;
				this.pager.RecordCount = GetCount();

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
					sTime = DateTime.Parse(this.tbx_beginDate.Value);
                    eTime = DateTime.Parse(this.tbx_endDate.Value);

					strSTime = sTime.ToString("yyyy-MM-dd HH:mm:ss");
					strETime = eTime.ToString("yyyy-MM-dd HH:mm:ss");

					if(sTime.AddDays(7) <= eTime)
					{
						WebUtils.ShowMessage(this,"日期跨度不能大于一周");
						return;
					}
					
				}
				catch
				{
					WebUtils.ShowMessage(this,"日期格式不正确");
					return;
				}

				DataSet ds =qs.QueryDKBankList(tbx_batchid.Text.Trim(),tbx_batchid_forbank.Text.Trim(),tbx_bank_type.Text.Trim(),ddl_status.SelectedValue,
					strSTime,strETime,					(pageIndex - 1) * this.pager.PageSize,this.pager.PageSize);

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					WebUtils.ShowMessage(this,"查询结果为空");
					this.Clear();
					return;
				}

				ds.Tables[0].Columns.Add("totalBatchUrl",typeof(string));
				ds.Tables[0].Columns.Add("successBatchUrl",typeof(string));
				ds.Tables[0].Columns.Add("failedBatchUrl",typeof(string));
				ds.Tables[0].Columns.Add("handlingBatchUrl",typeof(string));

				ds.Tables[0].Columns.Add("Ftotal_feeName",typeof(string));
				ds.Tables[0].Columns.Add("Fsucpay_feeName",typeof(string));
				ds.Tables[0].Columns.Add("Ffailpay_feeName",typeof(string));
				ds.Tables[0].Columns.Add("FHandling_feeName",typeof(string));

				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					dr["Ftotal_feeName"] = setConfig.FenToYuan(dr["Ftotal_fee"].ToString());
					dr["Fsucpay_feeName"] = setConfig.FenToYuan(dr["Fsucpay_fee"].ToString());
					dr["Ffailpay_feeName"] = setConfig.FenToYuan("0");
					dr["FHandling_feeName"] = setConfig.FenToYuan("0");


					// 这里要取的到底是商户批次号还是财付通批次号？
					dr["totalBatchUrl"] = "./QueryDKInfoPage.aspx?spid=" + dr["Fspid"].ToString() 
						+ "&batchid=" + dr["Fsp_batchid"].ToString() + "&sDate=" + dr["Fcreate_time"] + "&eDate=" + dr["Fmodify_time"];

					dr["successBatchUrl"] = "./QueryDKInfoPage.aspx?spid=" + dr["Fspid"].ToString() 
						+ "&batchid=" + dr["Fsp_batchid"].ToString() + "&state=s" + "&sDate=" + dr["Fcreate_time"] + "&eDate=" + dr["Fmodify_time"];

					dr["failedBatchUrl"] = "./QueryDKInfoPage.aspx?spid=" + dr["Fspid"].ToString() 
						+ "&batchid=" + dr["Fsp_batchid"].ToString() + "&state=f" + "&sDate=" + dr["Fcreate_time"] + "&eDate=" + dr["Fmodify_time"];

					dr["handlingBatchUrl"] = "./QueryDKInfoPage.aspx?spid=" + dr["Fspid"].ToString() 
						+ "&batchid=" + dr["Fsp_batchid"].ToString() + "&state=h" + "&sDate=" + dr["Fcreate_time"] + "&eDate=" + dr["Fmodify_time"];
				}

				DataSet ds2 = qs.CountDKBankList(tbx_batchid.Text.Trim(),tbx_batchid_forbank.Text.Trim(),tbx_bank_type.Text.Trim(),ddl_status.SelectedValue,
					strSTime,strETime);

				if(ds2 != null && ds2.Tables.Count != 0 && ds2.Tables[0].Rows.Count != 0)
				{
					DataRow dr2 = ds2.Tables[0].Rows[0];

					this.pager.RecordCount = int.Parse(dr2[0].ToString());

					this.lb_failAllMoney.Text = setConfig.FenToYuan(dr2[4].ToString());
					this.lb_failNum.Text = dr2[3].ToString();

					this.lb_successAllMoney.Text = setConfig.FenToYuan(dr2[2].ToString());
					this.lb_successNum.Text = dr2[1].ToString();

					this.lb_handlingAllMoney.Text = setConfig.FenToYuan(dr2[6].ToString());
					this.lb_handlingNum.Text = dr2[5].ToString();;
				}
				else
				{
					WebUtils.ShowMessage(this,"统计失败");
					//return;
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
			

			this.DataGrid_QueryResult.DataSource = null;
			this.DataGrid_QueryResult.DataBind();
		}



		private DataSet QueryDKBankListDetail(string bank_batch_id)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

			return qs.QueryDKBankListDetail(bank_batch_id);
		}


		private void DataGrid_QueryResult_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			if(e.CommandName == "detail")
			{
				DataSet ds = QueryDKBankListDetail(e.Item.Cells[0].Text.Trim());

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					WebUtils.ShowMessage(this,"查询结果为空");
					Clear();
					return;
				}

				DataRow dr = ds.Tables[0].Rows[0];

				string strCreateDate = dr["Fcreate_time"].ToString();
				DateTime createDate = DateTime.Parse(strCreateDate);

				lb_c1.Text = dr["Fbank_type"].ToString();
				lb_c2.Text = dr["Fbatchid_forbank"].ToString();
				lb_c3.Text = dr["Finterface_type"].ToString();
				lb_c4.Text = dr["Fbank_batch_id"].ToString();
				
				lb_c5.Text = dr["Fcur_type"].ToString();
				lb_c6.Text = dr["Fbatchid"].ToString();
				lb_c7.Text = setConfig.FenToYuan(dr["Ftotal_fee"].ToString());
				lb_c8.Text = setConfig.FenToYuan(dr["Fdispatch_fee"].ToString());

				lb_c9.Text = dr["Ftotal_count"].ToString();
				lb_c10.Text = dr["Fdispatch_count"].ToString();
				lb_c11.Text = setConfig.FenToYuan(dr["Fsucpay_fee"].ToString());
				lb_c12.Text = setConfig.FenToYuan(dr["Ffact_fee"].ToString());

				lb_c13.Text = dr["Fsucpay_count"].ToString();
				lb_c14.Text = dr["Ffact_count"].ToString();
				lb_c15.Text = dr["FstatusName"].ToString();
				lb_c16.Text = dr["Fexplain"].ToString();

				lb_c17.Text = dr["Fcreate_time"].ToString();
				lb_c18.Text = dr["Flastsend_time"].ToString();
				lb_c19.Text = dr["Fmodify_time"].ToString();
				lb_c20.Text = dr["Fexpect_rev_time"].ToString();

				lb_c21.Text = dr["Fret_code"].ToString();
				lb_c22.Text = dr["Fresult_info"].ToString();
				lb_c23.Text = dr["Fclient_ip"].ToString();
				lb_c24.Text = dr["Fmodify_ip"].ToString();
			}
		}
	}
}
