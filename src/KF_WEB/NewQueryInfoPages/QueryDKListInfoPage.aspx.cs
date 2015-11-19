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
	/// QueryDKListInfoPage ��ժҪ˵����
	/// </summary>
	public partial class QueryDKListInfoPage : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if(!IsPostBack)
			{
				this.tbx_beginDate.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss");
				this.tbx_endDate.Text = DateTime.Now.AddDays(0).ToString("yyyy-MM-dd HH:mm:ss");

				this.ButtonBeginDate.Attributes.Add("onclick","openModeBegin()");
				this.ButtonEndDate.Attributes.Add("onclick","openModeEnd()");

				this.pager.PageSize = 10;
				this.pager.RecordCount = GetCount();
			}

			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);
		}

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
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
					sTime = DateTime.Parse(this.tbx_beginDate.Text.Trim());
					eTime = DateTime.Parse(this.tbx_endDate.Text.Trim());

					strSTime = sTime.ToString("yyyy-MM-dd HH:mm:ss");
					strETime = eTime.ToString("yyyy-MM-dd HH:mm:ss");

					if(sTime.AddMonths(3) <= eTime)
					{
						WebUtils.ShowMessage(this,"���ڿ�Ȳ��ܴ���3����");
						return;
					}
                  
				}
				catch
				{
					WebUtils.ShowMessage(this,"���ڸ�ʽ����ȷ");
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
					WebUtils.ShowMessage(this,"��ѯ���Ϊ��");
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

					// ����Ҫȡ�ĵ������̻����κŻ��ǲƸ�ͨ���κţ�
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
					WebUtils.ShowMessage(this,"ͳ��ʧ��");
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
                    sTime = DateTime.Parse(this.tbx_beginDate.Text.Trim());
                    eTime = DateTime.Parse(this.tbx_endDate.Text.Trim());

                    strSTime = sTime.ToString("yyyy-MM-dd HH:mm:ss");
                    strETime = eTime.ToString("yyyy-MM-dd HH:mm:ss");

                }
                catch
                {
                    WebUtils.ShowMessage(this, "���ڸ�ʽ����ȷ");
                    return;
                }

                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

                string state = ddl_state.SelectedValue;

                DataSet ds = qs.QueryBatchInfo(strSTime, strETime, this.tbx_spid.Text, this.tbx_spBatchID.Text, this.tbx_batchid.Text,state,
                      -1, -1);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    WebUtils.ShowMessage(this, "��ѯ���Ϊ��");
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
                Response.AddHeader("Content-Disposition", "attachment; filename=����������ѯ.xls");
                Response.ContentType = "application/ms-excel";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                Response.Write(sw);
                Response.End();
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, ex.Message);
            }
        }
	}
}
