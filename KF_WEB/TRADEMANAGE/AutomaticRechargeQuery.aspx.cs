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
using System.Xml;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
    /// QueryDiscountCode 的摘要说明。
	/// </summary>
    public partial class AutomaticRechargeQuery : System.Web.UI.Page
	{
        protected void Page_Load(object sender, System.EventArgs e)
		{
            
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    this.pager.RecordCount = 1000;
                    this.pager2.RecordCount = 1000;
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
            GetDetailList((int)ViewState["rid"], e.NewPageIndex);
        }

		private void ValidateDate()
		{
            string cftno = cftNo.Text.ToString();
          
                if (string.IsNullOrEmpty(cftno))
                {
                    throw new Exception("请输入财付通账号！");
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

            int rid = e.Item.ItemIndex;
            
            GetDetailList(rid,1);
        }

        private void GetDetailList(int rid, int index)
        {
            try
            {
                pager2.CurrentPageIndex = index;
                DataTable g_dt = (DataTable)ViewState["g_dt"];
                if (g_dt != null)
                {
                    string plan_id = g_dt.Rows[rid]["plan_id"].ToString();
                    string uin = g_dt.Rows[rid]["withhold_uin"].ToString();
                    int max = pager2.PageSize;
                    int start = max * (index - 1);
                    ViewState["rid"] = rid;

                    Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                    qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
                    DataSet ds = qs.QueryAutomaticRechargeBillList(uin, plan_id, start, max);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].Columns.Add("bill_amount_str", typeof(String));//充值金额
                        ds.Tables[0].Columns.Add("pay_amount_str", typeof(String));//支付金额
                        ds.Tables[0].Columns.Add("FstateName", typeof(String));//交易状态
                        ds.Tables[0].Columns.Add("trans_id_url", typeof(String));//订单号
                        DataTable dtAll = null;
                        dtAll = ds.Tables[0].Clone();

                        classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "bill_amount", "bill_amount_str");
                        classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "pay_amount", "pay_amount_str");
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            string state = dr["bill_status"].ToString();
                            switch (state)
                            {
                                case "0":
                                    dr["FstateName"] = "查询欠费成功";
                                    break;
                                case "1":
                                    dr["FstateName"] = "生成交易单成功";
                                    break;
                                case "2":
                                    dr["FstateName"] = "支付成功";
                                    break;
                                case "3":
                                    dr["FstateName"] = "销帐中(通知商户销帐前设置)";
                                    break;
                                case "4":
                                    dr["FstateName"] = "销帐成功";
                                    break;
                                case "5":
                                    dr["FstateName"] = "销帐失败";
                                    break;
                                case "6":
                                    dr["FstateName"] = "退款成功";
                                    break;
                                case "7":
                                    dr["FstateName"] = "交易关闭";
                                    break;
                            }

                            string transId = dr["trans_id"].ToString();
                            dr["trans_id_url"] = "<a href=" + "../TradeManage/TradeLogQuery.aspx?id=" + transId + " target=_blank >" + transId + "</a>";

                            if (dr["bill_state"].ToString() == "1")
                            {
                                dtAll.ImportRow(dr);
                            }
                        }
                        this.div2.Visible = true;
                        DataGrid2.DataSource = dtAll.DefaultView;
                        DataGrid2.DataBind();
                    }
                    else
                    {
                        this.div2.Visible = false;
                        DataGrid2.DataSource = null;
                        DataGrid2.DataBind();
                        WebUtils.ShowMessage(this, "查询结果为空");
                        return;
                    }
                }
            } 
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);
            }
        }



        private void BindData(int index)
        {
            try
            {
                this.div2.Visible = false;
                pager.CurrentPageIndex = index;

                string s_cftno = cftNo.Text.ToString().Trim();               
                int max = pager.PageSize;
                int start = max * (index - 1);

                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
                DataSet ds = qs.QueryAutomaticRecharge(s_cftno, start, max);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("threshold_amount_str", typeof(String));//最低额度
                    ds.Tables[0].Columns.Add("bankType", typeof(String));//扣款方式
                    DataTable dtAll = null;
                    dtAll = ds.Tables[0].Clone();
                    ViewState["g_dt"] = ds.Tables[0];

                    classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "threshold_amount", "threshold_amount_str");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["bankType"] = "未知";
                        if (dr["sign_state"].ToString() == "2")
                        {
                            //扣款方式查询 
                            DataSet dsKK = qs.GetBankTypeKK(dr["withhold_uin"].ToString(), dr["withhold_uid"].ToString());
                            if (dsKK != null && dsKK.Tables.Count > 0 && dsKK.Tables[0].Rows.Count > 0)
                            {
                                string Fbank_type = dsKK.Tables[0].Rows[0]["Fbank_type"].ToString();
                                dr["bankType"] = classLibrary.getData.GetBankNameFromBankCode(Fbank_type);
                            }
                            dtAll.ImportRow(dr);
                        }
                    }
                    this.div1.Visible = true;
                    DataGrid1.DataSource = dtAll.DefaultView;
                    DataGrid1.DataBind();
                    return;
                }
                else
                {
                    this.div1.Visible = false;
                    DataGrid1.DataSource = null;
                    DataGrid1.DataBind();
                    WebUtils.ShowMessage(this, "查询结果为空");
                    return;
                }
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);
            }

        }
   
    
    
    }
}