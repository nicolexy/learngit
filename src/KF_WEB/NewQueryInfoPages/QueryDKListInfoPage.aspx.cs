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
using System.IO;
using System.Web.Services.Protocols;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
	/// QueryDKListInfoPage 的摘要说明。
	/// </summary>
	public partial class QueryDKListInfoPage : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if(!IsPostBack)
			{
				this.tbx_beginDate.Value = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss");
                this.tbx_endDate.Value = DateTime.Now.AddDays(0).ToString("yyyy-MM-dd HH:mm:ss");
				this.pager.PageSize = 10;
				this.pager.RecordCount = GetCount();
			}

			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);
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


		private void BindData(int pageIndex)
		{
			DateTime sTime,eTime;
			string strSTime = "",strETime = "";
			try
			{
				try
				{
                    sTime = DateTime.Parse(this.tbx_beginDate.Value.Trim());
                    eTime = DateTime.Parse(this.tbx_endDate.Value.Trim());

					strSTime = sTime.ToString("yyyy-MM-dd HH:mm:ss");
					strETime = eTime.ToString("yyyy-MM-dd HH:mm:ss");

					if(sTime.AddMonths(3) <= eTime)
					{
						WebUtils.ShowMessage(this,"日期跨度不能大于3个月");
						return;
					}
                  
				}
				catch
				{
					WebUtils.ShowMessage(this,"日期格式不正确");
					return;
				}

				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

				qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

				string state = ddl_state.SelectedValue;

                DataSet ds = qs.QueryBatchInfo(strSTime, strETime, this.tbx_spid.Text, this.tbx_spBatchID.Text, this.tbx_batchid.Text, state,
					(pageIndex - 1) * this.pager.PageSize,this.pager.PageSize);

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
                    this.btn_outExcel.Visible = false;
					WebUtils.ShowMessage(this,"查询结果为空");
					return;
				}

                this.btn_outExcel.Visible = true;

				ds.Tables[0].Columns.Add("spidUrl",typeof(string));
				ds.Tables[0].Columns.Add("totalBatchUrl",typeof(string));
				ds.Tables[0].Columns.Add("successBatchUrl",typeof(string));
				ds.Tables[0].Columns.Add("failedBatchUrl",typeof(string));
				ds.Tables[0].Columns.Add("handlingBatchUrl",typeof(string));

				ds.Tables[0].Columns.Add("Ftotal_paynumName",typeof(string));
				ds.Tables[0].Columns.Add("Ffact_amountName",typeof(string));
				ds.Tables[0].Columns.Add("Fsucpay_amountName",typeof(string));
				ds.Tables[0].Columns.Add("Ffailpay_amountName",typeof(string));
				ds.Tables[0].Columns.Add("FHandling_amountName",typeof(string));

				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					dr["Ftotal_paynumName"] = setConfig.FenToYuan(dr["Ftotal_paynum"].ToString());
					dr["Ffact_amountName"] = setConfig.FenToYuan(dr["Ffact_amount"].ToString());
					dr["Fsucpay_amountName"] = setConfig.FenToYuan(dr["Fsucpay_amount"].ToString());
					dr["Ffailpay_amountName"] = setConfig.FenToYuan(dr["Ffailpay_amount"].ToString());
					dr["FHandling_amountName"] = setConfig.FenToYuan(dr["FHandling_amount"].ToString());

					dr["spidUrl"] = "../BaseAccount/PayBusinessQuery.aspx?spid=" + dr["Fspid"].ToString();

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

                DataSet ds2 = qs.CountBatchInfo(strSTime, strETime, this.tbx_spid.Text, this.tbx_spBatchID.Text, this.tbx_batchid.Text,ddl_state.SelectedValue);

				if(ds2 != null && ds2.Tables.Count != 0 && ds2.Tables[0].Rows.Count != 0)
				{
					DataRow dr2 = ds2.Tables[0].Rows[0];

					this.pager.RecordCount = int.Parse(dr2[0].ToString());

					this.lb_failAllMoney.Text = setConfig.FenToYuan(dr2[4].ToString());
					this.lb_failNum.Text = dr2[3].ToString();

					this.lb_successAllMoney.Text = setConfig.FenToYuan(dr2[2].ToString());
					this.lb_successNum.Text = dr2[1].ToString();

					this.lb_handlingMoney.Text = setConfig.FenToYuan(dr2[6].ToString());
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
                string errStr = PublicRes.GetErrorMsg(ex.Message);
                WebUtils.ShowMessage(this, errStr);
			}
		}


		protected void btn_serach_Click(object sender, System.EventArgs e)
		{
			BindData(1);

			this.pager.CurrentPageIndex = 1;
		}

		private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			this.pager.CurrentPageIndex = e.NewPageIndex;

			BindData(e.NewPageIndex);
		}

        protected void btn_outExcel_Click(object sender, System.EventArgs e)
        {
            BindDataOutExcel();

        }

        private void BindDataOutExcel()
        {
            DateTime sTime, eTime;
            string strSTime = "", strETime = "";
            try
            {
                try
                {
                    sTime = DateTime.Parse(this.tbx_beginDate.Value.Trim());
                    eTime = DateTime.Parse(this.tbx_endDate.Value.Trim());

                    strSTime = sTime.ToString("yyyy-MM-dd HH:mm:ss");
                    strETime = eTime.ToString("yyyy-MM-dd HH:mm:ss");

                }
                catch
                {
                    WebUtils.ShowMessage(this, "日期格式不正确");
                    return;
                }

                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

                string state = ddl_state.SelectedValue;

                DataSet ds = qs.QueryBatchInfo(strSTime, strETime, this.tbx_spid.Text, this.tbx_spBatchID.Text, this.tbx_batchid.Text,state,
                      -1, -1);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    WebUtils.ShowMessage(this, "查询结果为空");
                    return;
                }

                ds.Tables[0].Columns.Add("Ftotal_paynumName", typeof(string));
                ds.Tables[0].Columns.Add("Ffact_amountName", typeof(string));
                ds.Tables[0].Columns.Add("Fsucpay_amountName", typeof(string));
                ds.Tables[0].Columns.Add("Ffailpay_amountName", typeof(string));
                ds.Tables[0].Columns.Add("FHandling_amountName", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["Ftotal_paynumName"] = setConfig.FenToYuan(dr["Ftotal_paynum"].ToString());
                    dr["Ffact_amountName"] = setConfig.FenToYuan(dr["Ffact_amount"].ToString());
                    dr["Fsucpay_amountName"] = setConfig.FenToYuan(dr["Fsucpay_amount"].ToString());
                    dr["Ffailpay_amountName"] = setConfig.FenToYuan(dr["Ffailpay_amount"].ToString());
                    dr["FHandling_amountName"] = setConfig.FenToYuan(dr["FHandling_amount"].ToString());

                }

                DataTable dt = ds.Tables[0];
                StringWriter sw = new StringWriter();
                string excelHeader = DataGrid_QueryResult.Columns[0].HeaderText;
                for (int i = 1; i < DataGrid_QueryResult.Columns.Count; i++)
                {
                    excelHeader += "\t" + DataGrid_QueryResult.Columns[i].HeaderText;
                }
                sw.WriteLine(excelHeader);
                string str = "\"\t=\"";
                foreach (DataRow dr in dt.Rows)
                {
                    sw.WriteLine("=\"" + dr["Fcreate_time"].ToString() + str + dr["Fmodify_time"].ToString() + str + dr["Fsp_batchid"] + str
                        + dr["Fspid"] + str + dr["Fservice_codeName"] + str + dr["Ftotal_paynumName"] + str
                        + dr["Ftotal_count"] + str + dr["Fsucpay_amountName"] + str + dr["Fsucpay_count"] + str
                        + dr["Ffailpay_amountName"] + str + dr["Ffailpay_count"] + str + dr["FHandling_amountName"] + str
                        + dr["FHandling_Count"] + str + dr["FstateName"] + str + dr["Fresult_info"] + "\"");
                }
                sw.Close();
                Response.AddHeader("Content-Disposition", "attachment; filename=代扣批量查询.xls");
                Response.ContentType = "application/ms-excel";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                Response.Write(sw);
                Response.End();
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, ex.Message);
            }
        }
	}
}
