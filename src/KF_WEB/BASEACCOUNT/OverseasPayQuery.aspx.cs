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
using TENCENT.OSS.CFT.KF.KF_Web.Facade;
using TENCENT.OSS.CFT.KF.Common;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;


namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// OverseasPayQuery 的摘要说明。
	/// </summary>
	public partial class OverseasPayQuery : System.Web.UI.Page
	{

	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
			ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");

			if(!IsPostBack)
			{
				this.rbtOtherQuery.Checked = true;
				TextBoxBeginDate.Text = new DateTime(DateTime.Today.Year,DateTime.Today.Month,1).ToString("yyyy年MM月dd日");
				TextBoxEndDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
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

		}
		#endregion

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}
		private void CheckData()
		{
			if(this.rbtKeyQuery.Checked && this.txtInvoiceID.Text.Trim() == "")
			{
				throw new Exception("请输入货单号！");
			}

			if(this.rbtOtherQuery.Checked)
			{
				if(this.txtOrder_no.Text.Trim() == "" && this.txtTransactionid.Text.Trim() == "" && this.txtShipcontent.Text.Trim() == "")
				{
					throw new Exception("请输入订单号,网关产生的交易号,收货的QQ号中任意一项!");
				}

				DateTime begindate;
				DateTime enddate;
				try
				{
					begindate = DateTime.Parse(TextBoxBeginDate.Text);
					enddate = DateTime.Parse(TextBoxEndDate.Text);
				}
				catch
				{
					throw new Exception("日期输入有误！");
				}

				if(begindate.CompareTo(enddate) > 0)
				{
					throw new Exception("终止日期小于起始日期，请重新输入！");
				}

				if(begindate.Year != enddate.Year || begindate.Month != enddate.Month)
				{
					throw new Exception("不允许跨月查询！");
				}
			}
		}

		private void BindData(int index)
		{
			pager.PageSize = 5;
			int max = pager.PageSize;
			int start = max * (index-1);

			DataTable dt = new DataTable();
			dt.Columns.Add("invoice_id",typeof(string));
			dt.Columns.Add("order_date",typeof(string));
			dt.Columns.Add("order_title",typeof(string));
			dt.Columns.Add("amount",typeof(string));
			dt.Columns.Add("currency",typeof(string));
			dt.Columns.Add("shipcontent",typeof(string));
			dt.Columns.Add("ip_region",typeof(string));

			if(this.rbtKeyQuery.Checked)
			{
				ExtQueryFacade FacadeObj = new ExtQueryFacade();
				Facade.ExtQueryResult ResultObj = FacadeObj.queryDetailByInvoiceID(this.txtInvoiceID.Text.Trim());
				if(ResultObj.invoice_id != null && ResultObj.invoice_id != "")
				{
					DataRow dr = dt.NewRow();
					dr["invoice_id"] = ResultObj.invoice_id;
					dr["order_date"] = ResultObj.order_date;
					dr["order_title"] = ResultObj.order_title;
					dr["amount"] =  classLibrary.setConfig.FenToYuan(ResultObj.amount);
					dr["currency"] = ResultObj.currency;
					dr["shipcontent"] = ResultObj.shipcontent;
					dr["ip_region"] = ResultObj.ip_region;
					dt.Rows.Add(dr);
				}
			}
			else
			{
				ExtQueryConditions ConditionsObj = new ExtQueryConditions();

				ConditionsObj.yearMonth = Convert.ToDateTime(this.TextBoxBeginDate.Text).ToString("yyyyMM");
				ConditionsObj.beginDate = Convert.ToDateTime(this.TextBoxBeginDate.Text).ToString("yyyyMMdd");
				ConditionsObj.endDate = Convert.ToDateTime(this.TextBoxEndDate.Text).ToString("yyyyMMdd");
				if(this.txtOrder_no.Text.Trim() != "")
					ConditionsObj.order_no = this.txtOrder_no.Text.Trim();
				if(this.txtTransactionid.Text.Trim() != "")
					ConditionsObj.transactionid = this.txtTransactionid.Text.Trim();
				if(this.txtShipcontent.Text.Trim() != "")
					ConditionsObj.shipcontent = this.txtShipcontent.Text.Trim();

				ConditionsObj.firstRecord = start;
				ConditionsObj.firstRecordSpecified = true;
				ConditionsObj.recordCount = max;
				ConditionsObj.recordCountSpecified = true;
				ExtQueryFacade FacadeObj = new ExtQueryFacade();
				ExtQueryResult[] ResultObjList = FacadeObj.queryDetails(ConditionsObj);
				for(int i=0; i<ResultObjList.Length; i++)
				{
					if(ResultObjList[i].invoice_id != null && ResultObjList[i].invoice_id != "")
					{
						DataRow dr = dt.NewRow();
						dr["invoice_id"] = ResultObjList[i].invoice_id;
						dr["order_date"] = ResultObjList[i].order_date;
						dr["order_title"] = ResultObjList[i].order_title;
						dr["amount"] =  classLibrary.setConfig.FenToYuan(ResultObjList[i].amount);
						dr["currency"] = ResultObjList[i].currency;
						dr["shipcontent"] = ResultObjList[i].shipcontent;
						dr["ip_region"] = ResultObjList[i].ip_region;
						dt.Rows.Add(dr);
					}
				}
			}
			this.DataGrid1.DataSource = dt.DefaultView;
			this.DataGrid1.DataBind();

		}
		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			try
			{
				CheckData();
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

			try
			{
				Table2.Visible = true;
				pager.RecordCount= 10000;
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
			if(e.CommandName == "Select")
			{
				try
				{
					this.PanelDetail.Visible = true;

					ExtQueryFacade FacadeObj = new ExtQueryFacade();
					Facade.ExtQueryResult ResultObj = FacadeObj.queryDetailByInvoiceID(e.Item.Cells[0].Text.Trim());
					this.lblinvoice_id.Text = ResultObj.invoice_id;
					this.lblorder_no.Text = ResultObj.order_no;
					this.lblorder_date.Text = ResultObj.order_date;
					this.lblchnid.Text = ResultObj.chnid;
					this.lblchtype.Text = ResultObj.chtype;
					this.lblamount.Text = MoneyTransfer.FenToYuan(ResultObj.amount);
					this.lblcurrency.Text = ResultObj.currency;
					this.lblshipcontent.Text = ResultObj.shipcontent;
					try
					{
						this.lblsettle_amount.Text = classLibrary.setConfig.FenToYuan(ResultObj.settle_amount);
					}
					catch
					{
						this.lblsettle_amount.Text = ResultObj.settle_amount;
					}
					this.lblsettle_currency.Text = ResultObj.settle_currency;
					this.lblpayment_status.Text = ResultObj.payment_status;
					this.lblpayer_status.Text = ResultObj.payer_status;

					string mail = ResultObj.payer_email;
					int Flag = mail.IndexOf("@");
					if(Flag > 0)
					{
						if(Flag > 4)
							mail = mail.Substring(0,Flag-4) + "****" + mail.Substring(Flag,mail.Length-Flag);
						else if(Flag == 4)
							mail = mail.Substring(0,1) + "***" + mail.Substring(Flag,mail.Length-Flag);
						else if(Flag == 3)
							mail = mail.Substring(0,1) + "**" + mail.Substring(Flag,mail.Length-Flag);
						else if(Flag == 2)
							mail = mail.Substring(0,1) + "*" + mail.Substring(Flag,mail.Length-Flag);
						else if(Flag == 1)
							mail = "*" + mail.Substring(Flag,mail.Length-Flag);
					}
					this.lblpayer_email.Text = mail;
					this.lblip_region.Text = ResultObj.ip_region;
					this.lblcountrycode.Text = ResultObj.countrycode;
					this.lbltransactionid.Text = ResultObj.transactionid;
					this.lbltransactiontype.Text = ResultObj.transactiontype;
					this.lblpaymenttype.Text = ResultObj.paymenttype;
					try
					{
						this.lblfee_amount.Text = classLibrary.setConfig.FenToYuan(ResultObj.fee_amount);
					}
					catch
					{
						this.lblsettle_amount.Text = ResultObj.fee_amount;
					}
					try
					{
						this.lbltax_amount.Text = classLibrary.setConfig.FenToYuan(ResultObj.tax_amount);
					}
					catch
					{
						this.lbltax_amount.Text = ResultObj.tax_amount;
					}
					this.lblexchange_rate.Text = ResultObj.exchange_rate;
					this.lblpayerid.Text = ResultObj.payerid;
					this.lblpaychannel.Text = ResultObj.paychannel;
					if(ResultObj.complete_time.ToString() != "")
					{
						this.lblcomplete_time.Text = ResultObj.complete_time.ToString();
					}
					else
					{
						this.lblcomplete_time.Text = "";
					}
					if(ResultObj.payment_date.ToString() != "")
					{
						this.lblpayment_date.Text = ResultObj.payment_date.ToString();
					}
					else
					{
						this.lblpayment_date.Text = "";
					}
				}
				catch(Exception ex)
				{
					WebUtils.ShowMessage(this.Page,ex.Message);
				}
			}
		}


	}
}
