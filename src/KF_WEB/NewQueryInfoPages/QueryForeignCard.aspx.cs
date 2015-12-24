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
using CFT.CSOMS.BLL.ForeignCardModule;
using System.Text;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
	/// Summary description for QueryForeignCard.
	/// </summary>
	public partial class QueryForeignCard : System.Web.UI.Page
	{
	    ForeignCardService fcs=new ForeignCardService();
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				this.TextBoxBeginDate.Value = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                this.TextBoxEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
			}
		}

		private void btn_query_Click(object sender, System.EventArgs e)
        {
                this.pager.RecordCount = 1000;
                dgInfo.CurrentPageIndex = 0;
                try
                {
                    validData();
                }
                catch (Exception ex)
                {
                    ShowMsg(ex.Message);
                    return;
                }

                BindData(1);
		}

        private void validData()
        {
            try
            {
                string strBeginDate = "", strEndDate = "";

                try
                {
                    if (this.TextBoxBeginDate.Value.Trim() != "" && this.TextBoxEndDate.Value.Trim() != "")
                    {
                        DateTime start = DateTime.Parse(this.TextBoxBeginDate.Value);
                        DateTime end = DateTime.Parse(this.TextBoxEndDate.Value);
                        if (start.Year != end.Year)
                        {
                            throw new Exception("�벻Ҫ�����ѯ��");
                        }
                        strBeginDate = start.AddDays(-1).ToString("yyyy-MM-dd");
                        strEndDate = end.ToString("yyyy-MM-dd");
                    }

                }
                catch
                {
                    throw new Exception("���ڸ�ʽ����ȷ��");
                }
                string merchant = txbMerchant.Text.Trim();
                string money = txbMoney.Text.Trim();
                string order = txbOrder.Text.Trim();
                string bankOrder = txtBankOrder.Text.Trim();
                if (merchant == string.Empty && order == string.Empty && bankOrder == string.Empty)
                {
                    throw new Exception("�������̻��š�������ˮ�����Ż����ж����ţ�");
                }

                if (merchant != string.Empty && merchant.Length < 3)
                {
                    throw new Exception("�̻���̫�̣�");
                }
                if (order != string.Empty && order.Length < 3)
                {
                    throw new Exception("������ˮ������̫�̣�");
                }

                string condition = "";
                if (!string.IsNullOrEmpty(strBeginDate) && !string.IsNullOrEmpty(strEndDate))
                {
                    condition += string.Format("|stime:{0}|etime:{1}", strBeginDate, strEndDate);
                }
                if (order != string.Empty)
                {
                    condition += "|listid:" + order;
                }
                if (merchant != string.Empty)
                {
                    condition += "|spid:" + merchant;
                }
                if (money != string.Empty)
                {
                    condition += "|bank_currency_fee:" + money;
                }
                if (bankOrder != string.Empty)
                {
                    condition += "|bank_listid:" + bankOrder;
                }

                if (DropDownState.SelectedValue != "all")
                {
                    int state = 0;
                    switch (DropDownState.SelectedValue)
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
                    condition += "|trade_state:" + state;
                }

                condition = condition.Substring(1, condition.Length-1);

                ViewState["order"] = order;
                ViewState["condition"] = condition;
                ViewState["merchant"] = merchant;
                ViewState["bankOrder"] = bankOrder;
                ViewState["strBeginDate"] = strBeginDate;
                ViewState["strEndDate"] = strEndDate;
            }
            catch (Exception ex)
            {
                throw new Exception("��ѯ�����쳣��" + PublicRes.GetErrorMsg(ex.Message));
            }
        }

        protected void BindData(int index)
        {
            try
            {
                pager.CurrentPageIndex = index;
                int max = pager.PageSize;
                int start = max * (index - 1) + 1;
                string order = ViewState["order"].ToString();
                string condition = ViewState["condition"].ToString();
                string merchant = ViewState["merchant"].ToString();
                string bankOrder = ViewState["bankOrder"].ToString();
                string strBeginDate = ViewState["strBeginDate"].ToString();
                string strEndDate = ViewState["strEndDate"].ToString();

                if (order != string.Empty)
                {
                    QueryByOrder(order, condition);
                }
                else if (merchant != string.Empty)
                {
                    QueryByMerchant(merchant, condition, start, max);
                }
                else if (bankOrder != string.Empty)
                {
                    if (string.IsNullOrEmpty(strBeginDate) || string.IsNullOrEmpty(strEndDate))
                    {
                        ShowMsg("����������������");
                        return;
                    }
                    QueryByBankOrder(strBeginDate.Substring(0, 4), condition, start, max);
                }
            }
            catch (Exception ex)
            {
                ShowMsg("��ѯ�쳣��" + PublicRes.GetErrorMsg(ex.Message)); 
                return;
            }
        }

		private void QueryByOrder(string order, string sqlCondition)
		{           
            DataSet ds = fcs.QueryForeignCardInfoByOrder(sqlCondition);
			dataBind(ds);
		}

		void QueryByMerchant(string merchant, string sqlCondition,int start,int max)
		{
            DataSet ds = fcs.QueryForeignCardInfoByMerchant(merchant, sqlCondition, start, max);
			dataBind(ds);
		}

        void QueryByBankOrder(string year, string sqlCondition, int start, int max)
		{
            DataSet ds = fcs.QueryForeignCardInfoByBankOrder(sqlCondition, start, max);
			dataBind(ds);
		}

		void dataBind(DataSet bindSet)
		{
			if(bindSet == null || bindSet.Tables.Count == 0 || bindSet.Tables[0].Rows.Count == 0)
			{
				this.ShowMsg("��ѯ��¼Ϊ�ա�");
				this.dgInfo.DataSource = bindSet;
				this.dgInfo.DataBind();
				return;
			}
			bindSet.Tables[0].Columns.Add("BankType", typeof(string));
			bindSet.Tables[0].Columns.Add("TradeState", typeof(string));
			foreach(DataRow row in bindSet.Tables[0].Rows)
			{
                row["Fdesc"] = Encoding.UTF8.GetString(Encoding.GetEncoding("gbk").GetBytes(row["Fdesc"].ToString()));
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
						row["TradeState"] = "δ֧��";
						break;
					case 2:
						row["TradeState"] = "֧���ɹ�";
						break;
					case 3:
						row["TradeState"] = "������";
						break;
					case 4:
						row["TradeState"] = "�˿�";
						break;
				}
			}
			this.dgInfo.DataSource = bindSet;
			this.dgInfo.DataBind();
		}

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
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
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);
		}
		#endregion
	}
}
