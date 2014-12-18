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
using System.Web.Services.Protocols;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
	/// QueryDKServicePage 的摘要说明。
	/// </summary>
	public partial class QueryDKServicePage : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lb_c13;
		protected System.Web.UI.WebControls.Label lb_c14;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if(!IsPostBack)
			{
                //this.tbx_beginDate.Text = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd HH:mm:ss");
                //this.tbx_endDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                //this.ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()");
                //this.ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");

				ddl_service_code.Items.Clear();
                ddl_service_code.Items.Add(new ListItem("所有类型", "9999999"));
				foreach(DictionaryEntry de in TENCENT.OSS.C2C.Finance.Common.CommLib.getData.htService_code)
				{
					ddl_service_code.Items.Add(new ListItem(de.Value.ToString(), de.Key.ToString()));
				}
				ddl_service_code.SelectedValue = "9999999";
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
			this.DataGrid_QueryResult.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid_QueryResult_ItemCommand);
			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.pager_PageChanged);

		}
		#endregion

		protected void btn_serach_Click(object sender, System.EventArgs e6)
		{
            if (tbx_spid.Text.Trim() == "")
            {
                WebUtils.ShowMessage(this, "请输入商户号");
                return;
            }
			if(ddl_service_code.SelectedValue == null || ddl_service_code.SelectedValue == "")
				return;

			ViewState["spid"] = tbx_spid.Text.Trim();
			ViewState["code"] = ddl_service_code.SelectedValue.Trim();

            //DateTime sTime, eTime;
            //try
            //{
            //    sTime = DateTime.Parse(this.tbx_beginDate.Text);
            //    eTime = DateTime.Parse(this.tbx_endDate.Text);

            //    ViewState["strSTime"] = sTime.ToString("yyyy-MM-dd HH:mm:ss");
            //    ViewState["strETime"] = eTime.ToString("yyyy-MM-dd HH:mm:ss");
            //}
            //catch
            //{
            //    WebUtils.ShowMessage(this, "日期格式不正确");
            //    return;
            //}
            this.pager.RecordCount = 1000;
			BindData(1);
		}

		private void BindData(int index)
		{
            try
            {
                string spid = ViewState["spid"].ToString();
                string code = ViewState["code"].ToString();

                //string strSTime = ViewState["strSTime"].ToString();
                //string strETime = ViewState["strETime"].ToString();

                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

                DataSet ds = qs.GetDKService_List(ddlBank.SelectedValue.Trim(), spid, code, (index - 1) * this.pager.PageSize, this.pager.PageSize);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    WebUtils.ShowMessage(this, "查询结果为空");
                    return;
                }

                DataGrid_QueryResult.DataSource = ds.Tables[0].DefaultView;
                DataGrid_QueryResult.DataBind();
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }
		}

		private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			int index = e.NewPageIndex;
			BindData(index);
		}

		private void DataGrid_QueryResult_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
            try
            {
                if (e.CommandName == "detail")
                {
                    string spid = e.Item.Cells[2].Text;
                    string codeid = e.Item.Cells[4].Text;
                    //string strSTime = ViewState["strSTime"].ToString();
                    //string strETime = ViewState["strETime"].ToString();

                    Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                    qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

                    //共用前面的函数来处理此业务对应的单笔限额等等。
                    DataSet ds = qs.GetDKService_List(ddlBank.SelectedValue.Trim(), spid, codeid, 0, 1);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        //return;
                    }
                    else
                    {
                        DataRow dr = ds.Tables[0].Rows[0];

                        lb_c1.Text = dr["Fspid"].ToString();
                        lb_c2.Text = dr["Fspname"].ToString();

                        lb_c31.Text = dr["Fonce_data"].ToString();

                        lb_c33.Text = dr["Fday_sum_data"].ToString();
                        Label17.Text = dr["Fweek_sum_data"].ToString();
                        Label19.Text = dr["Fmonth_sum_data"].ToString();
                        Label21.Text = dr["Fquarter_sum_data"].ToString();
                        Label23.Text = dr["Fyear_sum_data"].ToString();

                        lb_c34.Text = dr["Fday_sum_count"].ToString();
                        Label18.Text = dr["Fweek_sum_count"].ToString();
                        Label20.Text = dr["Fmonth_sum_count"].ToString();
                        Label22.Text = dr["Fquarter_sum_count"].ToString();
                        Label24.Text = dr["Fyear_sum_count"].ToString();

                        lb_c9.Text = dr["Fcodeid"].ToString();
                        lb_c10.Text = dr["Fservice_code"].ToString();
                        lb_c11.Text = dr["FcodeName"].ToString();
                        lb_c18.Text = dr["FlstateName"].ToString();
                    }

                    //处理商户总体单笔限额等等。
                    ds = qs.GetDKService_Detail9999(spid);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        //return;
                    }
                    else
                    {
                        DataRow dr = ds.Tables[0].Rows[0];

                        lb_c4.Text = dr["Fonce_data"].ToString();

                        lb_c5.Text = dr["Fday_sum_data"].ToString();
                        lb_c6.Text = dr["Fday_use_data"].ToString();

                        lb_c7.Text = dr["Fweek_sum_data"].ToString();
                        lb_c8.Text = dr["Fweek_use_data"].ToString();

                        Label1.Text = dr["Fmonth_sum_data"].ToString();
                        Label2.Text = dr["Fmonth_use_data"].ToString();

                        Label3.Text = dr["Fquarter_sum_data"].ToString();
                        Label4.Text = dr["Fquarter_use_data"].ToString();

                        Label5.Text = dr["Fyear_sum_data"].ToString();
                        Label6.Text = dr["Fyear_use_data"].ToString();

                        Label7.Text = dr["Fday_sum_count"].ToString();
                        Label8.Text = dr["Fday_use_count"].ToString();

                        Label9.Text = dr["Fweek_sum_count"].ToString();
                        Label10.Text = dr["Fweek_use_count"].ToString();

                        Label11.Text = dr["Fmonth_sum_count"].ToString();
                        Label12.Text = dr["Fmonth_use_count"].ToString();

                        Label13.Text = dr["Fquarter_sum_count"].ToString();
                        Label14.Text = dr["Fquarter_use_count"].ToString();

                        Label15.Text = dr["Fyear_sum_count"].ToString();
                        Label16.Text = dr["Fyear_use_count"].ToString();
                    }

                    //再处理单笔sp_service
                    string servicecode = e.Item.Cells[0].Text;
                    ds = qs.GetDKService_SPServiceDetail(servicecode);

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        //return;
                    }
                    else
                    {
                        DataRow dr = ds.Tables[0].Rows[0];

                        lb_c3.Text = dr["Fmer_class"].ToString();
                        lb_c12.Text = dr["Fservice_type"].ToString();

                        lb_c15.Text = dr["Fbank_allow"].ToString();
                        lb_c16.Text = dr["Fpay_mode"].ToString();

                        lb_c17.Text = dr["Fdirect"].ToString();
                        lb_c23.Text = dr["Fcreate_time"].ToString();
                        lb_c24.Text = dr["Fstime"].ToString();
                        lb_c25.Text = dr["Fsign_notify_url"].ToString();
                        lb_c26.Text = dr["Fetime"].ToString();
                        lb_c27.Text = dr["Fpay_notify_url"].ToString();
                        lb_c28.Text = dr["Fmodify_time"].ToString();
                        lb_c29.Text = dr["Fmemo"].ToString();
                        lb_c30.Text = dr["Fstandby3"].ToString();

                        //再处理单笔sp_service中的业务特性
                        int mask = Int32.Parse(dr["Fsevice_mask"].ToString());
                        for (int i = 0; i < 15; i++)
                        {
                            System.Web.UI.Control obj = FindControl("cb_Fsevice_mask_" + i.ToString());
                            if (obj != null)
                            {
                                CheckBox cb = (CheckBox)obj;
                                cb.Checked = (mask & 2 ^ i) > 0;
                            }
                        }

                        mask = Int32.Parse(dr["Friskcrtl_mask"].ToString());
                        for (int i = 0; i < 15; i++)
                        {
                            System.Web.UI.Control obj = FindControl("cb_Friskcrtl_mask_" + i.ToString());
                            if (obj != null)
                            {
                                CheckBox cb = (CheckBox)obj;
                                cb.Checked = (mask & 2 ^ i) > 0;
                            }
                        }

                        mask = Int32.Parse(dr["Fpayflow_mask"].ToString());
                        for (int i = 0; i < 15; i++)
                        {
                            System.Web.UI.Control obj = FindControl("cb_Fpayflow_mask_" + i.ToString());
                            if (obj != null)
                            {
                                CheckBox cb = (CheckBox)obj;
                                cb.Checked = (mask & 2 ^ i) > 0;
                            }
                        }

                        mask = Int32.Parse(dr["Fservice_channel"].ToString());
                        for (int i = 0; i < 15; i++)
                        {
                            System.Web.UI.Control obj = FindControl("cb_Fservice_channel_" + i.ToString());
                            if (obj != null)
                            {
                                CheckBox cb = (CheckBox)obj;
                                cb.Checked = (mask & 2 ^ i) > 0;
                            }
                        }
                    }

                }
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }
		}

	}
}
