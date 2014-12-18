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
using System.Linq;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.C2C.Finance.BankLib;
using CFT.CSOMS.BLL.FundModule;
using System.Configuration;
using CFT.CSOMS.COMMLIB;
using CFT.CSOMS.BLL.CFTAccountModule;


namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{

    /// <summary>
    /// GetFundRatePage ��ժҪ˵����
    /// </summary>
    public partial class GetFundRatePage : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Label rtnList;
        protected System.Web.UI.WebControls.Label lb_2;
        protected System.Web.UI.WebControls.Label lb_1;

        protected Query_Service.Query_Service queryService = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
        protected FundService fundBLLService = new FundService();

       // private string uin, fundSPId;
      //  private string uin;
        private DateTime beginDate = DateTime.Now;
        private DateTime endDate = DateTime.Now;
        private int redirectionType = 0;
        private string memo = string.Empty;
       // private string tradeId;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (!ClassLib.ValidateRight("LCTQuery", this))
                {
                    Response.Redirect("../login.aspx?wh=1");
                }
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            try
            {
                queryService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

                // �ڴ˴������û������Գ�ʼ��ҳ��
                ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()");
                ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");

                if (IsPostBack)
                {
                    //FetchInput(); TODO:�޷������ж��쳣,��ɢ�����¼���
                }
                else
                {
                    this.tbx_beginDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    this.tbx_endDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

                    //�󶨻���˾�б�
                  //  BindFundsList();
                    this.tableQueryResult.Visible = false;
                    this.tableBankRollList.Visible = false;
                    this.tableBankRollListNotChildren.Visible = false;
                    this.tableCloseFundRoll.Visible = false;
                }
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
            }

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
            this.dgUserFundSummary.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgUserFundSummary_ItemCommand);

        }
        #endregion

        private void FetchInput()
        {
            string uin = this.TextBox1_InputQQ.Text.Trim();
            if (string.IsNullOrEmpty(uin))
            {
                throw new Exception("΢�ŲƸ�ͨ�˺Ų���Ϊ�գ�");
            }

            Session["QQID"] = getQQID();
            uin = Session["QQID"].ToString();

            string tradeId = fundBLLService.GetTradeIdByUIN(uin);
            if (string.IsNullOrEmpty(tradeId))
                throw new Exception("��ѯ�����û���TradeId����ȷ�ϵ�ǰ�û��Ƿ���ע������˻�");
            ViewState["uin"] = uin;
            ViewState["tradeId"] = tradeId;
        //    fundSPId = this.ddl_companyName.SelectedValue;

            //try
            //{
            //    if (this.tbx_beginDate.Text.Trim() != "")
            //        beginDate = DateTime.Parse(this.tbx_beginDate.Text);

            //    if (this.tbx_endDate.Text.Trim() != "")
            //        endDate = DateTime.Parse(this.tbx_endDate.Text);
            //}
            //catch
            //{
            //    WebUtils.ShowMessage(this, "���ڸ�ʽ����ȷ");
            //}

            //redirectionType = int.Parse(ddlDirection.SelectedItem.Value);
            //memo = ddlMemo.SelectedItem.Value;
        }

        private void BindAllData()
        {
            BindBasicAccountInfo(ViewState["uin"].ToString());
            try
            {
                BindFundAccountsSummary(ViewState["uin"].ToString());
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
            }

            try//���ͨ����ѯ
            {
                DataTable subAccountInfoTable = new AccountService().QuerySubAccountInfo(ViewState["uin"].ToString(), 89);//���ͨ������89

                if (subAccountInfoTable == null || subAccountInfoTable.Rows.Count < 1)
                {
                    lbLCTBalance.Text = "0";
                }
                else
                {
                    lbLCTBalance.Text = classLibrary.setConfig.FenToYuan(subAccountInfoTable.Rows[0]["Fbalance"].ToString());//��תԪ
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ѯ���ͨ���ʧ�ܣ�" + eSys.Message.ToString());
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                FetchInput();
                BindAllData();
                this.tableLCTBalanceRoll.Visible = false;
                this.tableQueryResult.Visible = false;
                this.tableBankRollList.Visible = false;
                this.tableBankRollListNotChildren.Visible = false;
                this.tableCloseFundRoll.Visible = false;
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "���÷������" + HttpUtility.JavaScriptStringEncode(errStr));
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + HttpUtility.JavaScriptStringEncode(eSys.Message));
            }
        }

        string getQQID()
        {
            if (string.IsNullOrEmpty(this.TextBox1_InputQQ.Text))
            {
                throw new Exception("������Ҫ��ѯ���˺�");
            }
            var id = this.TextBox1_InputQQ.Text.Trim();
            if (this.WeChatCft.Checked)
            {
                return id;
            }
            else if (this.WeChatUid.Checked)
            {
                var qs = new Query_Service.Query_Service();
                return qs.Uid2QQ(id);
            }
            else if (this.WeChatQQ.Checked || this.WeChatMobile.Checked || this.WeChatEmail.Checked)
            {
                string queryType = string.Empty;
                if (this.WeChatQQ.Checked)
                {
                    queryType = "QQ";
                }
                else if (this.WeChatMobile.Checked)
                {
                    queryType = "Mobile";
                }
                else if (this.WeChatEmail.Checked)
                {
                    queryType = "Email";
                }

                string openID = string.Empty, errorMessage = string.Empty;
                int errorCode = 0;
                var IPList = ConfigurationManager.AppSettings["WeChat"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < IPList.Length; j++)
                {
                    if (PublicRes.getOpenIDFromWeChat(queryType, id, out openID, out errorCode, out errorMessage, IPList[j]))
                    {
                        break;
                    }
                }
                if (errorCode == 0)
                {
                    return openID + "@wx.tenpay.com";
                }
                else if (errorCode == 1)
                {
                    throw new Exception("û�д��û�");
                }
                else
                {
                    throw new Exception(errorCode + errorMessage);
                }
            }
            else if (this.WeChatId.Checked)
            {
                return WeChatHelper.GetUINFromWeChatName(id);
            }

            return id;
        }

        private void BindFundsList()
        {
            try
            {
                var fundCorLists = FundService.GetAllFundInfo();

                if (fundCorLists.Count < 1)
                    throw new Exception("δ��ȡ������˾��¼��");

                //this.ddl_companyName.DataSource = fundCorLists;
                //this.ddl_companyName.DataTextField = "Name";
                //this.ddl_companyName.DataValueField = "SPId";
                //this.ddl_companyName.DataBind();
                //�����ѡ��
                //this.ddl_companyName.Items.Insert(0, new ListItem("ȫ��", string.Empty));

            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, "����˾�б���ȡʧ��:" + HttpUtility.JavaScriptStringEncode(ex.Message));
            }
        }

        private void BindBasicAccountInfo(string qqId)
        {
            //�����˻���Ϣ
            var basicFundAccountInfo = queryService.GetUserFundAccountInfo(qqId);
            if (basicFundAccountInfo != null && basicFundAccountInfo.Tables.Count > 0 && basicFundAccountInfo.Tables[0].Rows.Count > 0)
            {
                lblName.Text = basicFundAccountInfo.Tables[0].Rows[0]["Ftrue_name"].ToString();
                lblAccountStatus.Text = basicFundAccountInfo.Tables[0].Rows[0]["FstateName"].ToString();
                lblCell.Text = basicFundAccountInfo.Tables[0].Rows[0]["Fmobile"].ToString();
                lblCreateTime.Text = basicFundAccountInfo.Tables[0].Rows[0]["Fcreate_time"].ToString();
            }


            //��ȫ����Ϣ
            var safeCardInfo = queryService.GetPayCardInfo(qqId);
            string card_tail = "";
            string bind_serialno = "";
            string bank_type = "";
            if (safeCardInfo != null && safeCardInfo.Rows.Count > 0)
            {
                lblSafeBankCardType.Text = BankIO.QueryBankName(safeCardInfo.Rows[0]["Fbank_type"].ToString());
                lblSafeBankCardNoTail.Text = safeCardInfo.Rows[0]["Fcard_tail"].ToString();
                card_tail = safeCardInfo.Rows[0]["Fcard_tail"].ToString();
                bind_serialno = PublicRes.objectToString(safeCardInfo, "Fbind_serialno");//�����к�
                bank_type = safeCardInfo.Rows[0]["Fbank_type"].ToString();
            }

            //��ȫ��β�ż�����
            ViewState["card_tail"] = card_tail.Trim();
            ViewState["bank_type"] = bank_type.Trim();
            ViewState["mobile"] = lblCell.Text.Trim();
            ViewState["bind_serialno"] = bind_serialno.Trim();
        }

        private void BindFundAccountsSummary(string uin)
        {
            var summaryTable = new FundService().GetUserFundSummary(uin);

            summaryTable.Columns.Add("profitText", typeof(string));
            summaryTable.Columns.Add("balanceText", typeof(string)); //���
            summaryTable.Columns.Add("conText", typeof(string));//������
            summaryTable.Columns.Add("close_flagText", typeof(string));
            summaryTable.Columns.Add("transfer_flagText", typeof(string));
            summaryTable.Columns.Add("buy_validText", typeof(string));

            classLibrary.setConfig.FenToYuan_Table(summaryTable, "Ftotal_profit", "profitText");
            classLibrary.setConfig.FenToYuan_Table(summaryTable, "con", "conText");

            if (ClassLib.ValidateRight("BalanceControl", this))
            {
                classLibrary.setConfig.FenToYuan_Table(summaryTable, "balance", "balanceText");
            }

            foreach (DataRow dr in summaryTable.Rows)
            {
                switch (dr["close_flag"].ToString())
                {
                    case "1":
                        dr["close_flagText"] = "�����";
                        break;
                    case "2":
                        dr["close_flagText"] = "���";
                        break;
                    case "3":
                        dr["close_flagText"] = "����";
                        break;
                }
                switch (dr["transfer_flag"].ToString())
                {
                    case "0":
                        dr["transfer_flagText"] = "��֧��ת�롢ת��";
                        break;
                    case "1":
                        dr["transfer_flagText"] = "֧��ת�벻֧��ת��";
                        break;
                    case "2":
                        dr["transfer_flagText"] = "֧��ת����֧��ת��";
                        break;
                    case "3":
                        dr["transfer_flagText"] = "ͬʱ֧��ת���ת��";
                        break;
                }
                switch (dr["buy_valid"].ToString())
                {
                    case "1":
                        dr["buy_validText"] = "֧���깺";
                        break;
                    case "2":
                        dr["buy_validText"] = "֧���Ϲ�";
                        break;
                    case "4":
                        dr["buy_validText"] = "֧���깺/�Ϲ�";
                        break;
                }
            }
            dgUserFundSummary.DataSource = summaryTable;
            dgUserFundSummary.DataBind();

            //ͳ�������ܺͣ�������ܺ�
            long totalBalance = 0, totalProfit = 0;
            foreach (DataRow item in summaryTable.Rows)
            {
                totalBalance += long.Parse(item["balance"].ToString());
                totalProfit += long.Parse(item["Ftotal_profit"].ToString());
            }
            if (!ClassLib.ValidateRight("BalanceControl", this))
            {
                lblBalance.Text = "";
            }
            else 
            {
                lblBalance.Text = classLibrary.setConfig.FenToYuan(totalBalance); //�˻��ܽ��
            }
            
            lblTotalProfit.Text = classLibrary.setConfig.FenToYuan(totalProfit);
        }

        private void BindProfitList(string tradeId, string spId, DateTime beginDate, DateTime endDate, int pageIndex = 1)
        {

            try 
	        {
                this.pager.CurrentPageIndex = pageIndex;
		        var profits = fundBLLService.GetFundProfitRecord(tradeId: tradeId,
                                                            beginDateStr:beginDate.ToString("yyyyMMdd"), 
                                                            endDateStr: endDate.ToString("yyyyMMdd"),
                                                            spId: spId,
                                                            currentPageIndex: pageIndex -1,
                                                            pageSize: pager.PageSize);

                if (profits.Rows.Count > 0)
                {
                    profits.Columns.Add("Fvalid_money_str", typeof(String));//���汾���
                    profits.Columns.Add("Fpur_typeName", typeof(String));//��Ŀ
                    profits.Columns.Add("F7day_profit_rate_str", typeof(String));//�����껯����
                    profits.Columns.Add("Fprofit_str", typeof(String));//������
                    profits.Columns.Add("Fspname", typeof(String));//����˾��
                    profits.Columns.Add("Fprofit_per_ten_thousand", typeof(string));//�������

                    foreach (DataRow dr in profits.Rows)
                    {
                        dr["Fpur_typeName"] = "�ֺ�";
                        if (!(dr["F7day_profit_rate"] is DBNull))
                        {
                            string tmp = dr["F7day_profit_rate"].ToString();
                            if (!string.IsNullOrEmpty(tmp))
                            {
                                decimal d = (decimal)(Int64.Parse(tmp)) / 100000000;
                                dr["F7day_profit_rate_str"] = d.ToString("P4");
                            }
                        }
                        //����������
                        if (dr["F1day_profit_rate"] != null && dr["F1day_profit_rate"] != DBNull.Value && dr["F1day_profit_rate"].ToString() != string.Empty)
                        {
                            decimal oneDayProfitRrate = 0;
                            decimal.TryParse(dr["F1day_profit_rate"].ToString(), out oneDayProfitRrate);
                            decimal profitPerTenThousand = oneDayProfitRrate / 10000;

                            dr["Fprofit_per_ten_thousand"] = profitPerTenThousand.ToString("N3");
                        }

                        //����˾��
                        dr["Fspname"] = FundService.GetAllFundInfo().Where(i => i.SPId == dr["Fspid"].ToString()).First().SPName;                    
                    }
                    if (ClassLib.ValidateRight("BalanceControl", this))
                    {
                        classLibrary.setConfig.FenToYuan_Table(profits, "Fvalid_money", "Fvalid_money_str");
                    }
                    
                    classLibrary.setConfig.FenToYuan_Table(profits, "Fprofit", "Fprofit_str");


                    this.DataGrid_QueryResult.DataSource = profits.DefaultView;
                    this.DataGrid_QueryResult.DataBind();
                }
	        }
	        catch (Exception ex)
	        {	
		        throw new Exception(string.Format("��ѯ���������¼�쳣��{0}", ex.Message));
	        }
            
        }

        private void BindBankRollList(string qqId, string spId, string curtype, DateTime beginDate, DateTime endDate, int pageIndex = 1, int redirectionType = 0, string memo = "")
        {
            try
            {
                this.bankRollListPager.CurrentPageIndex = pageIndex;
                int max = pager.PageSize;
                int start = max * (pageIndex - 1);

                if (string.IsNullOrEmpty(spId))
                    throw new Exception(string.Format("�޷�ͬʱ��ѯ���л������ˮ��Ϣ����ѡ��ָ���Ļ���"));

                //var fundInfo = FundService.GetAllFundInfo().Where(i => i.SPId == spId);

                //if(fundInfo.Count() < 1)
                //    throw new Exception(string.Format("�Ҳ���{0}��Ӧ�Ļ�����Ϣ", spId));

                var bankRollList = queryService.GetChildrenBankRollList(qqId, beginDate, endDate, curtype, start + 1, max, redirectionType, memo);

                if (bankRollList.Tables != null && bankRollList.Tables.Count > 0)
                {
                    bankRollList.Tables[0].Columns.Add("FpaynumText", typeof(string));
                    bankRollList.Tables[0].Columns.Add("FbalanceText", typeof(string)); //�˻����
                    bankRollList.Tables[0].Columns.Add("FtypeText", typeof(string));
                    bankRollList.Tables[0].Columns.Add("FmemoText", typeof(string));
                    bankRollList.Tables[0].Columns.Add("FconStr", typeof(string));
                    bankRollList.Tables[0].Columns.Add("URL", typeof(string));

                    foreach (DataRow dr in bankRollList.Tables[0].Rows)
                    {
                        switch (dr["Ftype"].ToString())
                        {
                            case "1":
                                dr["FtypeText"] = "��";
                                break;
                            case "2":
                                dr["FtypeText"] = "��";
                                break;
                            case "3":
                                dr["FtypeText"] = "����";
                                break;
                            case "4":
                                dr["FtypeText"] = "�ⶳ";
                                break;
                            default:
                                dr["FtypeText"] = dr["Ftype"].ToString();
                                break;
                        }

                        switch (dr["Fmemo"].ToString())
                        {
                            case "�����˻�����":
                                dr["FmemoText"] = "����";
                                break;
                            default:
                                dr["FmemoText"] = dr["Fmemo"].ToString();
                                break;
                        }

                        string duoFund = "";
                        string listid=dr["Flistid"].ToString();
                        if (dr["FmemoText"].ToString().Equals("�����깺"))
                        {
                            if (fundBLLService.IfAnewBoughtFund(dr["Flistid"].ToString(), dr["Fcreate_time"].ToString()))
                            {
                                dr["FmemoText"]="�����깺";
                            }

                            duoFund = QueryTradeFundInfo(spId, listid);//��ѯ�����ת��
                            dr["FmemoText"] += duoFund;
                        }

                        if (dr["FmemoText"] .ToString().Equals("����"))
                        {
                            duoFund = QueryTradeFundInfo(spId, listid.Substring(listid.Length - 18));//��ѯ�����ת��
                            dr["FmemoText"] += duoFund;
                        }

                        dr["FpaynumText"] = classLibrary.setConfig.FenToYuan(dr["Fpaynum"].ToString());
                        
                        dr["FconStr"] = classLibrary.setConfig.FenToYuan(dr["Fcon"].ToString());


                        dr["URL"] = "GetFundRatePageDetail.aspx?opertype=1&close_flag=1&uin=" + ViewState["uin"].ToString()
                        + "&spid=" + ViewState["fundSPId"].ToString()
                        + "&fund_code=" + ViewState["fundCode"].ToString()
                        + "&total_fee=" + dr["Fbalance"].ToString()
                        + "&bind_serialno=" + ViewState["bind_serialno"].ToString()
                        + "&card_tail=" + ViewState["card_tail"].ToString()
                        + "&mobile=" + ViewState["mobile"].ToString()
                        + "&bank_type=" + ViewState["bank_type"].ToString();

                    }

                    if (ClassLib.ValidateRight("BalanceControl", this))
                    {
                        classLibrary.setConfig.FenToYuan_Table(bankRollList.Tables[0], "Fbalance", "FbalanceText");
                    }

                    dgBankRollList.DataSource = bankRollList.Tables[0].DefaultView;
                    dgBankRollList.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("��ȡ�˻���ˮ�쳣:{0}", ex.Message));
            }
        }

        //����ջ���ͷ�ǿ�갴ť
        public void dgBankRollList_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            object obj = e.Item.Cells[7].FindControl("UnCloseFundApplyButton");
            int index = this.bankRollListPager.CurrentPageIndex;
            string type = e.Item.Cells[2].Text.Trim();//��ȡ
            if (obj != null)
            {
                LinkButton lb = (LinkButton)obj;
                if (index == 1 && e.Item.ItemIndex == 0 && type!="����")//��һҳ����һ����ˮ��¼������أ��Ҵ�ȡ״̬����Ϊ����
                {
                    if (DateTime.Now.Hour >= 9 && DateTime.Now.Hour <= 14)//ǿ�귢��ʱ�乤����9��00��15��00������ʱ���޷�����ǿ�꣬��ť��ɫ
                    lb.Visible = true;
                }
            }
        }

        private string QueryTradeFundInfo(string spId, string listid)
        {
            string duoFund = "";
            var tradeFund = fundBLLService.QueryTradeFundInfo(spId, listid);
            if (tradeFund != null && tradeFund.Rows.Count > 0)
            {
                string fundName = tradeFund.Rows[0]["Ffund_name"].ToString();
                string tmp = tradeFund.Rows[0]["Fpur_type"].ToString();
                if (tmp == "11")
                    duoFund = "(" + fundName + "ת��)";
                if (tmp == "12")
                    duoFund = "(ת����" + fundName + ")";
            }
            return duoFund;
        }

        private void BindBankRollListNotChildren(string qqId, string spId, string curtype, DateTime beginDate, DateTime endDate, int pageIndex = 1, int redirectionType = 0)
        {
            try
            {
                this.bankRollListNotChildrenPager.CurrentPageIndex = pageIndex;
                int max = pager.PageSize;
                int start = max * (pageIndex - 1);

                //if (string.IsNullOrEmpty(spId))
                 //   throw new Exception(string.Format("�޷�ͬʱ��ѯ���л������ˮ��Ϣ����ѡ��ָ���Ļ���"));

                //var fundInfo = FundService.GetAllFundInfo().Where(i => i.SPId == spId);

                //if (fundInfo.Count() < 1)
                //    throw new Exception(string.Format("�Ҳ���{0}��Ӧ�Ļ�����Ϣ", spId));

                DataTable bankRollList = fundBLLService.GetFundRollList(qqId, beginDate, endDate, curtype, start, max, redirectionType);

                if (bankRollList != null)
                {
                    bankRollList.Columns.Add("Fsub_trans_id_str", typeof(string));
                    bankRollList.Columns.Add("FtypeText", typeof(string));
                    bankRollList.Columns.Add("Ftotal_fee_str", typeof(string));
                    bankRollList.Columns.Add("Floading_type_str", typeof(string));
                    bankRollList.Columns.Add("Fstate_str", typeof(string));

                    if (bankRollList.Rows.Count > 0)
                    {
                        foreach (DataRow dr in bankRollList.Rows)
                        {
                            dr["Fsub_trans_id_str"] = "104" + dr["Fsub_trans_id"].ToString();
                            string Fpur_type = dr["Fpur_type"].ToString();
                            switch (Fpur_type)
                            {
                                case "1":
                                    dr["FtypeText"] = "��";
                                    break;
                                case "2":
                                    dr["FtypeText"] = "�Ϲ�";
                                    break;
                                case "3":
                                    dr["FtypeText"] = "��Ͷ";
                                    break;
                                case "4":
                                    dr["FtypeText"] = "��";
                                    break;
                                case "5":
                                    dr["FtypeText"] = "����";
                                    break;
                                case "6":
                                    dr["FtypeText"] = "�ֺ�";
                                    break;
                                case "7":
                                    dr["FtypeText"] = "���깺ʧ��";
                                    break;
                                case "8":
                                    dr["FtypeText"] = "����ȷ���˿�";
                                    break;
                                case "9":
                                    dr["FtypeText"] = "���������깺";
                                    break;
                                case "10":
                                    dr["FtypeText"] = "���ͷݶ��깺";
                                    break;
                                default:
                                    dr["FtypeText"] = dr["Fpur_type"].ToString();
                                    break;
                            }

                            switch (dr["Floading_type"].ToString())
                            {
                                case "0":
                                    dr["Floading_type_str"] = "��ͨ���";
                                    break;
                                case "1":
                                    dr["Floading_type_str"] = "�������";
                                    break;
                                default:
                                    dr["Floading_type_str"] = dr["Floading_type"].ToString();
                                    break;
                            }

                            if (Fpur_type != "4")
                            {//���˳�������û����ط�ʽ
                                dr["Floading_type_str"] = "";
                            }

                         
                        }
                      
                        Hashtable ht = new Hashtable();
                        ht.Add("0", "�����깺��");
                        ht.Add("1", "�ȴ��ۿ�");
                        ht.Add("2", "���۳ɹ�");
                        ht.Add("3", "�깺�ɹ�");
                        ht.Add("4", "��ʼ��ص�");
                        ht.Add("5", "������˾��سɹ�");
                        ht.Add("6", "������˾���ʧ��");
                        ht.Add("7", "�깺����ʧ�ܣ�����ʧЧ");
                        ht.Add("8", "�깺�������˿�");
                        ht.Add("9", "�깺��ת���˿�");
                        ht.Add("10", "��ص��������");
                        ht.Add("11", "���˻���������ɹ�");
                        ht.Add("20", "����");

                        classLibrary.setConfig.DbtypeToPageContent(bankRollList, "Fstate", "Fstate_str", ht);

                        classLibrary.setConfig.FenToYuan_Table(bankRollList, "Ftotal_fee", "Ftotal_fee_str");
                       
                    }

                    dgBankRollListNotChildren.DataSource = bankRollList.DefaultView;
                    dgBankRollListNotChildren.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("��ȡ�û�������ˮ�쳣:{0}", ex.Message));
            }
        }

        protected void bankRollListPager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                this.bankRollListPager.CurrentPageIndex = e.NewPageIndex;
                FetchInputDetail();
                BindBankRollList(ViewState["uin"].ToString(), ViewState["fundSPId"].ToString(), ViewState["curtype"].ToString(), beginDate, endDate, e.NewPageIndex, redirectionType, memo);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, string.Format("��ҳ�쳣:{0}", ex.Message));
            }
        }

        protected void bankRollListNotChildrenPager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                this.bankRollListNotChildrenPager.CurrentPageIndex = e.NewPageIndex;
                FetchInputDetail();
                BindBankRollListNotChildren(ViewState["uin"].ToString(), ViewState["fundSPId"].ToString(), ViewState["curtype"].ToString(), beginDate, endDate, e.NewPageIndex, redirectionType);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, string.Format("��ҳ�쳣:{0}", ex.Message));
            }
        }

        protected void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                this.pager.CurrentPageIndex = e.NewPageIndex;
                FetchInputDetail();
                BindProfitList(ViewState["tradeId"].ToString(), ViewState["fundSPId"].ToString(), beginDate, endDate, e.NewPageIndex);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, string.Format("��ҳ�쳣:{0}", ex.Message));
            }
        }

        private void dgUserFundSummary_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            try
            {
                int rid = e.Item.ItemIndex;
                ViewState["fundSPId"] = e.Item.Cells[0].Text.Trim();
                ViewState["curtype"] = e.Item.Cells[1].Text.Trim();
                ViewState["fundCode"] = e.Item.Cells[2].Text.Trim();
                ViewState["close_flag"] = e.Item.Cells[3].Text.Trim();
                if (ViewState["close_flag"].ToString() == "2")//��ռ�����
                    this.queryDiv.Visible = false;
                else
                    this.queryDiv.Visible = true;

                this.tableQueryResult.Visible = false;
                this.tableBankRollList.Visible = false;
                this.tableBankRollListNotChildren.Visible = false;
                this.tableCloseFundRoll.Visible = false;
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message);
            }
        }

        protected void btnQueryDetail_Click(object sender, EventArgs e)
        {
            try
            {
                this.pager.RecordCount = 1000;
                this.bankRollListPager.RecordCount = 1000;
                this.bankRollListNotChildrenPager.RecordCount = 1000;
                this.CloseFundRollPager.RecordCount = 1000;
                FetchInputDetail();
                if (ViewState["close_flag"].ToString() == "2")//��ռ�����
                {
                    this.tableCloseFundRoll.Visible = true;
                    BindCloseFundRoll(ViewState["tradeId"].ToString(), ViewState["fundCode"].ToString(), beginDate, endDate, 1);
                }
                else
                {
                    this.tableQueryResult.Visible = true;
                    this.tableBankRollList.Visible = true;
                    this.tableBankRollListNotChildren.Visible = true;
                    BindAllDetailData();
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + HttpUtility.JavaScriptStringEncode(eSys.Message));
            }
        }

        private void FetchInputDetail()
        {
            try
            {
                if (this.tbx_beginDate.Text.Trim() != "")
                    beginDate = DateTime.Parse(this.tbx_beginDate.Text);

                if (this.tbx_endDate.Text.Trim() != "")
                    endDate = DateTime.Parse(this.tbx_endDate.Text);
            }
            catch
            {
                WebUtils.ShowMessage(this, "���ڸ�ʽ����ȷ");
            }

            redirectionType = int.Parse(ddlDirection.SelectedItem.Value);
            memo = ddlMemo.SelectedItem.Value;
        }
        private void BindAllDetailData()
        {
            try
            {
                BindProfitList(ViewState["tradeId"].ToString(), ViewState["fundSPId"].ToString(), beginDate, endDate);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
            }
            BindBankRollList(ViewState["uin"].ToString(), ViewState["fundSPId"].ToString(), ViewState["curtype"].ToString(), beginDate, endDate, 1, redirectionType, memo);
            BindBankRollListNotChildren(ViewState["uin"].ToString(), ViewState["fundSPId"].ToString(), ViewState["curtype"].ToString(), beginDate, endDate, 1, redirectionType);
        }

        private void BindCloseFundRoll(string tradeId, string fundCode, DateTime beginDate, DateTime endDate, int pageIndex = 1)
        {
            try
            {
                this.CloseFundRollPager.CurrentPageIndex = pageIndex;
                int max = pager.PageSize;
                int start = max * (pageIndex - 1);

                if (string.IsNullOrEmpty(tradeId) || string.IsNullOrEmpty(fundCode))
                    throw new Exception(string.Format("qqId����fundCodeΪ��"));

                DataTable tbCloseFundRollList = fundBLLService.QueryCloseFundRollList(tradeId, fundCode, beginDate.ToString("yyyyMMdd"), endDate.ToString("yyyyMMdd"), start, max);

                if (tbCloseFundRollList != null)
                {
                    tbCloseFundRollList.Columns.Add("URL", typeof(string));
                    if (tbCloseFundRollList.Rows.Count > 0)
                        foreach (DataRow dr in tbCloseFundRollList.Rows)
                        {
                            dr["URL"] = "GetFundRatePageDetail.aspx?opertype=1&close_flag=2&uin=" + ViewState["uin"].ToString()
                                + "&spid=" + ViewState["fundSPId"].ToString()
                                + "&fund_code=" + ViewState["fundCode"].ToString()
                                + "&total_fee=" + dr["Fstart_total_fee"].ToString()//�ܽ����𣩵�λ��
                                + "&end_date=" + dr["Fend_date"].ToString();
                        }
                    dgCloseFundRoll.DataSource = tbCloseFundRollList.DefaultView;
                    dgCloseFundRoll.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("��ѯ������ϸ:{0}", ex.Message));
            }
        }

        //��ջ���ͷ�ǿ�갴ť
        public void dgCloseFundRoll_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            object obj = e.Item.Cells[12].FindControl("CloseFundApplyButton");
            string state = e.Item.Cells[8].Text.Trim();//��״̬
            if (obj != null)
            {
                LinkButton lb = (LinkButton)obj;
                if (state == "��ִ��")//��ִ��״̬����ǿ��
                {
                    lb.Visible = true;
                }
            }
        }

        protected void CloseFundRollPager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                this.CloseFundRollPager.CurrentPageIndex = e.NewPageIndex;
                FetchInputDetail();
                BindCloseFundRoll(ViewState["tradeId"].ToString(), ViewState["fundCode"].ToString(), beginDate, endDate, e.NewPageIndex);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, string.Format("��ҳ�쳣:{0}", ex.Message));
            }
        }

        /// <summary>
        /// ���ͨ�����ˮ��ѯ
        /// </summary>
        protected void btnBalanceQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.tableLCTBalanceRoll.Visible = true;
                this.BalanceRollPager.RecordCount = 1000;
              //  ViewState["tradeId"] = "111111111111001";//����
                BindLCTBalanceRollList(ViewState["tradeId"].ToString(), 1);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + HttpUtility.JavaScriptStringEncode(eSys.Message));
            }
        }

        private void BindLCTBalanceRollList(string tradeId, int index = 1)
        {
            try
            {
                this.BalanceRollPager.CurrentPageIndex = index;
                int max = BalanceRollPager.PageSize;
                int start = max * (index - 1);
                var balanceRoll = new LCTBalanceService().QueryLCTBalanceRollList(tradeId, start, max);

                if (balanceRoll!=null&&balanceRoll.Rows.Count > 0)
                {
                    this.dgLCTBalanceRollList.DataSource = balanceRoll.DefaultView;
                    this.dgLCTBalanceRollList.DataBind();
                    return;
                }
                this.dgLCTBalanceRollList.DataSource = null;
                this.dgLCTBalanceRollList.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("��ѯ���ͨ�����ˮ�쳣��{0}", ex.Message));
            }

        }
        protected void BalanceRollPager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                this.BalanceRollPager.CurrentPageIndex = e.NewPageIndex;
                BindLCTBalanceRollList(ViewState["tradeId"].ToString(), e.NewPageIndex);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, string.Format("��ҳ�쳣:{0}", ex.Message));
            }
        }
    }
}
