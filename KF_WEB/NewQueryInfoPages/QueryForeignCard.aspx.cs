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

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
	/// Summary description for QueryForeignCard.
	/// </summary>
	public partial class QueryForeignCard : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
			ButtonEndDate.Attributes.Add("onclick", "openModeEnd()"); 

			if (!Page.IsPostBack)
			{
				this.TextBoxBeginDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
				this.TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
			}
		}

		private void btn_query_Click(object sender, System.EventArgs e)
		{
			string strBeginDate = "",strEndDate = "";

			try
			{
				if(this.TextBoxBeginDate.Text.Trim() != "" && this.TextBoxEndDate.Text.Trim() != "")
				{
					DateTime start = DateTime.Parse(this.TextBoxBeginDate.Text);
					DateTime end = DateTime.Parse(this.TextBoxEndDate.Text);
					if(start.Year != end.Year)
					{
						ShowMsg("请不要跨年查询！");
						return;
					}
					strBeginDate = start.AddDays(-1).ToString("yyyy-MM-dd");
					strEndDate = end.ToString("yyyy-MM-dd");
				}
				
			}
			catch
			{
				ShowMsg("日期格式不正确！");
				return;
			}
			string merchant = txbMerchant.Text.Trim();
			string money = txbMoney.Text.Trim();
			string order = txbOrder.Text.Trim();
			string bankOrder = txtBankOrder.Text.Trim();
			if(merchant == string.Empty && order == string.Empty && bankOrder == string.Empty)
			{
				ShowMsg("请输入商户号、交易流水订单号或银行订单号！");
				return;
			}

			if(merchant != string.Empty && merchant.Length < 3)
			{
				ShowMsg("商户号太短！");
				return;
			}
			if(order != string.Empty && order.Length < 3)
			{
				ShowMsg("交易流水订单号太短！");
				return;
			}

			string condition = "";
			condition += string.Format(" Fcreate_time between '{0}' and '{1}' " , strBeginDate, strEndDate);

			if(order != string.Empty)
			{
				condition += " and Ftransaction_id = '" + order + "'";
			}
			if(merchant != string.Empty)
			{
				condition += " and Fspid = '" + merchant + "'";
			}
			if(money != string.Empty)
			{
				condition += " and Fbank_currency_fee = '" + money + "'";
			}
			if(bankOrder != string.Empty)
			{
				condition += " and Fbank_listid = '" + bankOrder + "'";
			}
			
			if(DropDownState.SelectedValue != "all")
			{
				int state = 0;
				switch(DropDownState.SelectedValue)
				{
					case "unpay":
						state = 1;
						break;
					case "payed":
						state = 2;
						break;
					case "refund":
						state = 4;
						break;
					case "waiting":
						state = 3;
						break;
				}
				condition += "and Ftrade_state = '" + state + "'";
			}
			if(order != string.Empty)
			{
				QueryByOrder(order, condition);
			}
			else if(merchant != string.Empty)
			{
				QueryByMerchant(merchant,condition);
			}
			else if(bankOrder != string.Empty)
			{
				QueryByBankOrder(strBeginDate.Substring(0,4), condition);
			}
		}

		private void QueryByOrder(string order, string sqlCondition)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataSet ds = qs.QueryForeignCardInfoByOrder(order, sqlCondition);
			dataBind(ds);
		}


		void QueryByMerchant(string merchant, string sqlCondition)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataSet ds = qs.QueryForeignCardInfoByMerchant(merchant, sqlCondition);
			dataBind(ds);
		}

		void QueryByBankOrder(string year, string sqlCondition)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataSet ds = qs.QueryForeignCardInfoByBankOrder(year, sqlCondition);
			dataBind(ds);
		}

		void dataBind(DataSet bindSet)
		{
			if(bindSet == null || bindSet.Tables.Count == 0 || bindSet.Tables[0].Rows.Count == 0)
			{
				this.ShowMsg("查询记录为空。");
				this.dgInfo.DataSource = bindSet;
				this.dgInfo.DataBind();
				return;
			}
			bindSet.Tables[0].Columns.Add("BankType", typeof(string));
			bindSet.Tables[0].Columns.Add("TradeState", typeof(string));
			foreach(DataRow row in bindSet.Tables[0].Rows)
			{
				int fBank_type = Convert.ToInt32(row["Fbank_type"]);
				int fTrad_state = Convert.ToInt32(row["Ftrade_state"]);
				switch(fBank_type)
				{
					case 1106:
						row["BankType"] = "MIGS";
						break;
					case 2113:
						row["BankType"] = "MOTO";
						break;
				}
				switch(fTrad_state)
				{
					case 1:
						row["TradeState"] = "未支付";
						break;
					case 2:
						row["TradeState"] = "支付成功";
						break;
					case 3:
						row["TradeState"] = "待处理";
						break;
					case 4:
						row["TradeState"] = "退款";
						break;
				}
			}
			this.dgInfo.DataSource = bindSet;
			this.dgInfo.DataBind();
		}


		private void ShowMsg(string msg)
		{
			Response.Write("<script language=javascript>alert('" + msg + "')</script>");
		}


		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			btQuery.Click += new EventHandler(btn_query_Click);
		}
		#endregion
	}
}
