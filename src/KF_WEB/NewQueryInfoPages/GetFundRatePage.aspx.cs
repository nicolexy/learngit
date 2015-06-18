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
using CFT.Apollo.Logging;


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
        /// <summary>
        /// ���ͨ���ǿ��
        /// </summary>
        private void LCTFundApply() 
        {
            try
            {
                int Balance = classLibrary.setConfig.YuanToFen(Convert.ToDouble(lbLCTBalance.Text.Replace("Ԫ", "")));
                if (Balance > 0)
                {
                    string param = "opertype=1&LCTFund=true&uin=" + ViewState["uin"].ToString() + //�Ƹ�ͨ�˺�
                                    "&total_fee=" + classLibrary.setConfig.YuanToFen(Convert.ToDouble(lbLCTBalance.Text.Replace("Ԫ", ""))) +//���ֽ��(��)
                                    //"&fund_code=" + ViewState["fundcode"] +        //�������
                                    "&bind_serialno=" + ViewState["bind_serialno"] +    //��ȫ�������к�
                                    "&bank_type=" + ViewState["bank_type"] +        //��ȫ����������
                                    "&card_tail=" + ViewState["card_tail"];        //��β��
                    btnLCTFundApply.OnClientClick = "window.open('GetFundRatePageDetail.aspx?" + param + "'); return false;";
                    btnLCTFundApply.Visible = true;
                }
            }
            catch (Exception e)
            {
                WebUtils.ShowMessage(this, "���ͨ���ǿ�꣺" + e.Message);
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                FetchInput();
                BindAllData();
                LCTFundApply();
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
            //���ͨ�˻������ⶳ����
            try
            {
                lbLCTAccState.Text = "";
                AccountService acc = new AccountService();
                string ip = Request.UserHostAddress.ToString();
                if (ip == "::1")
                    ip = "127.0.0.1";
                Boolean state = acc.LCTAccStateOperator(qqId, "3", Session["uid"].ToString(), ip);
                if (state)
                    lbLCTAccState.Text = "����";
                else
                    lbLCTAccState.Text = "����";
            }
            catch (Exception err)
            {
                string errStr = PublicRes.GetErrorMsg(err.Message.ToString());
                LogHelper.LogInfo("��ѯ���ͨ�˻�״̬ʧ�ܣ�" + errStr);
            }

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
            string mobile = "";
            if (safeCardInfo != null && safeCardInfo.Rows.Count > 0)
            {
                lblSafeBankCardType.Text = BankIO.QueryBankName(safeCardInfo.Rows[0]["Fbank_type"].ToString());
                lblSafeBankCardNoTail.Text = safeCardInfo.Rows[0]["Fcard_tail"].ToString();
                card_tail = safeCardInfo.Rows[0]["Fcard_tail"].ToString();
                bind_serialno = PublicRes.objectToString(safeCardInfo, "Fbind_serialno");//�����к�
                bank_type = safeCardInfo.Rows[0]["Fbank_type"].ToString();
                mobile = safeCardInfo.Rows[0]["Fmobile"].ToString();

                if (string.IsNullOrEmpty(card_tail.Trim()))//���ܴ����а�ȫ����¼����ʱ�ֻ��Ų�Ϊ�գ����ż�������Ϊ��
                    ViewState["HasSafeCard"] = false;
                else
                    ViewState["HasSafeCard"] = true;//����Ƿ��а�ȫ����Ϊ����ǿ����׼��
            }
            else
                ViewState["HasSafeCard"] = false;

            //��ȫ��β�ż�����
            ViewState["card_tail"] = card_tail.Trim();
            ViewState["bank_type"] = bank_type.Trim();
            //20141216 �û������е��ֻ��Ÿ��º�᲻��ǿ�꣬��Ϊȡ��ȫ���ֻ���
            // ViewState["mobile"] = lblCell.Text.Trim();
            ViewState["mobile"] = mobile.Trim();
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
            decimal totalMarkValue = 0;
            foreach (DataRow item in summaryTable.Rows)
            {
                totalBalance += long.Parse(item["balance"].ToString());
                totalProfit += long.Parse(item["Ftotal_profit"].ToString());
                totalMarkValue += decimal.Parse(item["markValue"].ToString());
            }
            if (!ClassLib.ValidateRight("BalanceControl", this))
            {
                lblBalance.Text = "";
            }
            else 
            {
                lblBalance.Text = classLibrary.setConfig.FenToYuan(totalBalance).Replace("Ԫ",""); //�˻��ܽ��
            }
            
            lblTotalProfit.Text = classLibrary.setConfig.FenToYuan(totalProfit);
            lbMarkValue.Text = totalMarkValue.ToString();//��ֵ
        }

        private void BindProfitList(string tradeId, string spId, DateTime beginDate, DateTime endDate, int pageIndex = 1)
        {

            try 
	        {
                this.pager.CurrentPageIndex = pageIndex;
                var profits = fundBLLService.BindProfitList(tradeId: tradeId,
                                                            beginDateStr:beginDate.ToString("yyyyMMdd"), 
                                                            endDateStr: endDate.ToString("yyyyMMdd"),
                                                            spId: spId,
                                                            currentPageIndex: pageIndex -1,
                                                            pageSize: pager.PageSize,
                                                            fund_code:ViewState["fundCode"].ToString());

                if (profits!=null&&profits.Rows.Count > 0)
                {
                    string fund_code = ViewState["fundCode"].ToString();
                    DataGrid_QueryResult.Columns[3].Visible = true;
                    DataGrid_QueryResult.Columns[4].Visible = true;
                    DataGrid_QueryResult.Columns[5].Visible = true;
                    DataGrid_QueryResult.Columns[7].Visible = true;
                    DataGrid_QueryResult.Columns[8].Visible = true;
                    DataGrid_QueryResult.Columns[9].Visible = true;
                    DataGrid_QueryResult.Columns[10].Visible = true;

                    //���ݻ���������չʾ�ֶ�
                    if ( fundBLLService.isSpecialFund(fund_code, spId)) //�׷��ﻦ��300����
                    {
                        DataGrid_QueryResult.Columns[3].Visible = false;
                        DataGrid_QueryResult.Columns[4].Visible = false;
                        DataGrid_QueryResult.Columns[5].Visible = false;
                    }
                    else
                    {
                        DataGrid_QueryResult.Columns[7].Visible = false;
                        DataGrid_QueryResult.Columns[8].Visible = false;
                        DataGrid_QueryResult.Columns[9].Visible = false;
                        DataGrid_QueryResult.Columns[10].Visible = false;
                    }

                    this.DataGrid_QueryResult.DataSource = profits.DefaultView;
                    this.DataGrid_QueryResult.DataBind();
                }
	        }
	        catch (Exception ex)
	        {	
		        throw new Exception(string.Format("��ѯ���������¼�쳣��{0}", PublicRes.GetErrorMsg(ex.Message.ToString())));
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

            //   var bankRollList = queryService.GetChildrenBankRollList(qqId, beginDate, endDate, curtype, start + 1, max, redirectionType, memo);
                var bankRollList = fundBLLService.GetChildrenBankRollListEx(qqId, spId, curtype, beginDate, endDate, start, max, redirectionType, memo);

                //��ȡǿ������URL,δ�ṩǿ�깦�ܽӿڵ��ͷ���
                GeForceRedeemUrl(bankRollList);
                string fund_code = ViewState["fundCode"].ToString();
                if (fundBLLService.isSpecialFund(fund_code, spId)) //�׷��ﻦ��300����
                {
                    this.dgBankRollList.Columns[3].HeaderText = "����ݶ�";
                    this.dgBankRollList.Columns[4].HeaderText = "�ݶ����";
                }
                else
                {
                    this.dgBankRollList.Columns[3].HeaderText = "���";
                    this.dgBankRollList.Columns[4].HeaderText = "�˻����";
                }
                dgBankRollList.DataSource = bankRollList.DefaultView;
                dgBankRollList.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("��ȡ�˻���ˮ�쳣:{0}", PublicRes.GetErrorMsg(ex.Message)));
            }
        }

        private void GeForceRedeemUrl(DataTable bankRollList)
        {
            if (bankRollList != null && bankRollList.Rows.Count > 0)
            {
                bool markQuerySfCar = false;
                foreach (DataRow dr in bankRollList.Rows)
                {
                  //  ViewState["HasSafeCard"] = false;//�����ް�ȫ��
                    if (ViewState["close_flag"].ToString() == "2")//��ջ���
                    {
                        dr["URL"] = "";
                    }
                    else if (ViewState["close_flag"].ToString() == "1")//����ջ���
                    {
                        try//�ް�ȫ�� ȡ�󶨿���Ϣ
                        {
                            //�Ƕ��ڻ���ǿ�꣺
                            //�а�ȫ������ȫ����أ�
                            //�ް�ȫ�����󶨿�ݿ���أ�
                            //�ް󶨿�������󶨿����
                            if (!(bool)(ViewState["HasSafeCard"]) && !markQuerySfCar)//�ް�ȫ��  
                            {
                                markQuerySfCar = true;

                                //��ȡ��ݡ��Ѱ�״̬�Ŀ�
                                DataSet ds = queryService.GetBankCardBindList_2(ViewState["uin"].ToString(), "", "", "", "", "", "", "", "", 2, 2, 0, 1);
                                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
                                {
                                    //ȡ��ݡ����״̬�Ŀ�
                                    ds = queryService.GetBankCardBindList_2(ViewState["uin"].ToString(), "", "", "", "", "", "", "", "", 2, 3, 0, 1);
                                }

                                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    ViewState["card_tail"] = ds.Tables[0].Rows[0]["Fcard_tail"].ToString();
                                    ViewState["bank_type"] = ds.Tables[0].Rows[0]["Fbank_type"].ToString();
                                    ViewState["mobile"] = ds.Tables[0].Rows[0]["Fmobilephone"].ToString();
                                    ViewState["bind_serialno"] = ds.Tables[0].Rows[0]["Fbind_serialno"].ToString();
                                }
                                else
                                {
                                    ViewState["card_tail"] = "";
                                    ViewState["bank_type"] = "";
                                    ViewState["mobile"] = "";
                                    ViewState["bind_serialno"] = "";
                                    LogHelper.LogInfo("Not  bind bank card!");
                                }
                            }

                        }
                        catch
                        {
                            LogHelper.LogInfo("Get Bind bank card error!");
                        }

                        dr["URL"] = "GetFundRatePageDetail.aspx?opertype=1&close_flag=1&uin=" + ViewState["uin"].ToString()
                     + "&spid=" + ViewState["fundSPId"].ToString()
                     + "&fund_code=" + ViewState["fundCode"].ToString()
                     + "&total_fee=" + dr["Fbalance"].ToString()
                     + "&bind_serialno=" + ViewState["bind_serialno"].ToString()
                     + "&card_tail=" + ViewState["card_tail"].ToString()
                     + "&mobile=" + ViewState["mobile"].ToString()
                     + "&bank_type=" + ViewState["bank_type"].ToString();
                    }
                    else
                    {
                        dr["URL"] = "";
                    }

                    if (!ClassLib.ValidateRight("BalanceControl", this))
                    {
                        //     classLibrary.setConfig.FenToYuan_Table(bankRollList, "Fbalance", "FbalanceText");
                        dr["FbalanceText"] = "";//û��Ȩ�޾Ͳ���ʾ���
                    }

                }
            }
        }

        //����ջ���ͷ�ǿ�갴ť
        public void dgBankRollList_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            object obj = e.Item.Cells[8].FindControl("UnCloseFundApplyButton");
            int index = this.bankRollListPager.CurrentPageIndex;
            string type = e.Item.Cells[2].Text.Trim();//��ȡ
             string fund_code = ViewState["fundCode"].ToString();
             string spid = ViewState["fundSPId"].ToString();
            if (obj != null)
            {
                LinkButton lb = (LinkButton)obj;
                if (index == 1 && e.Item.ItemIndex == 0 && type!="����"&&
                    ViewState["close_flag"].ToString() == "1")//��һҳ����һ����ˮ��¼������أ��Ҵ�ȡ״̬����Ϊ���� ��Ϊ�Ƕ��ڻ���
                {
                    if (!fundBLLService.isSpecialFund(fund_code, spid))//�׷��ﻦ��300ǿ�������أ��޽ӿ�
                    {
                    if (DateTime.Now.Hour >= 9 && DateTime.Now.Hour <= 15)//ǿ�귢��ʱ�乤����9��00��15��00������ʱ���޷�����ǿ�꣬��ť��ɫ
                    lb.Visible = true;
                    }
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

                //  DataTable bankRollList = fundBLLService.GetFundRollList(qqId, beginDate, endDate, curtype, start, max, redirectionType);
                //��������API�ӿ�һ��
                string fund_code = ViewState["fundCode"].ToString();
                DataTable bankRollList = fundBLLService.BindBankRollListNotChildren(qqId, curtype, beginDate, endDate, start, max, redirectionType, spId, fund_code);
                if (bankRollList != null)
                {

                    if (fundBLLService.isSpecialFund(fund_code, spId)) //�׷��ﻦ��300����
                    {
                        this.dgBankRollListNotChildren.Columns[2].Visible = true;
                        this.dgBankRollListNotChildren.Columns[5].Visible = true;
                    }
                    else
                    {
                        this.dgBankRollListNotChildren.Columns[2].Visible = false;
                        this.dgBankRollListNotChildren.Columns[5].Visible = false;
                    }

                    dgBankRollListNotChildren.DataSource = bankRollList.DefaultView;
                    dgBankRollListNotChildren.DataBind();
                }
                else
                {
                    dgBankRollListNotChildren.DataSource = null;
                    dgBankRollListNotChildren.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("��ȡ�û�������ˮ�쳣:{0}", PublicRes.GetErrorMsg(ex.Message)));
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
                WebUtils.ShowMessage(this, string.Format("��ҳ�쳣:{0}", PublicRes.GetErrorMsg(ex.Message)));
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
                WebUtils.ShowMessage(this, string.Format("��ҳ�쳣:{0}", PublicRes.GetErrorMsg(ex.Message)));
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
                WebUtils.ShowMessage(this, string.Format("��ҳ�쳣:{0}", PublicRes.GetErrorMsg(ex.Message)));
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
                    this.tableBankRollList.Visible = true;
                    BindCloseFundRoll(ViewState["tradeId"].ToString(), ViewState["fundCode"].ToString(), beginDate, endDate, 1);
                    BindBankRollList(ViewState["uin"].ToString(), ViewState["fundSPId"].ToString(), ViewState["curtype"].ToString(), beginDate, endDate, 1, redirectionType, memo);
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
                    tbCloseFundRollList.Columns.Add("FDate", typeof(string));

                    tbCloseFundRollList.Columns.Add("URL", typeof(string));
                    if (tbCloseFundRollList.Rows.Count > 0)
                        foreach (DataRow dr in tbCloseFundRollList.Rows)
                        {
                            dr["URL"] = "GetFundRatePageDetail.aspx?opertype=1&close_flag=2&uin=" + ViewState["uin"].ToString()
                                + "&spid=" + ViewState["fundSPId"].ToString()
                                + "&fund_code=" + ViewState["fundCode"].ToString()
                                + "&total_fee=" + dr["Fstart_total_fee"].ToString()//�ܽ����𣩵�λ��
                                + "&end_date=" + dr["Fend_date"].ToString();

                            string strEndDate = dr["Fend_date"].ToString();
                            dr["FDate"] = strEndDate.Substring(strEndDate.Length - 4);
                        }

                    
                    dgCloseFundRoll.DataSource = tbCloseFundRollList.DefaultView;
                    dgCloseFundRoll.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("��ѯ������ϸ:{0}", PublicRes.GetErrorMsg(ex.Message)));
            }
        }

        //��ջ���ͷ�ǿ�갴ť
        public void dgCloseFundRoll_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            object obj = e.Item.Cells[12].FindControl("CloseFundApplyButton");
            string state = e.Item.Cells[9].Text.Trim();//��״̬
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
                WebUtils.ShowMessage(this, string.Format("��ҳ�쳣:{0}", PublicRes.GetErrorMsg(ex.Message)));
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
                throw new Exception(string.Format("��ѯ���ͨ�����ˮ�쳣��{0}", PublicRes.GetErrorMsg(ex.Message)));
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
                WebUtils.ShowMessage(this, string.Format("��ҳ�쳣:{0}", PublicRes.GetErrorMsg(ex.Message)));
            }
        }
    }
}
