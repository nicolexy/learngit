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
using System.Collections.Generic;


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
                if (IsPostBack)
                {
                    //FetchInput(); TODO:�޷������ж��쳣,��ɢ�����¼���
                }
                else
                {
                    this.tbx_beginDate.Value = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    this.tbx_endDate.Value = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

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
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.ToString()));
            }

        }

        private void FetchInput()
        {
            string uin = this.TextBox1_InputQQ.Text.Trim();
            if (string.IsNullOrEmpty(uin))
            {
                throw new Exception("΢�ŲƸ�ͨ�˺Ų���Ϊ�գ�");
            }
            string queryType = GetQueryType();
            Session["QQID"] = AccountService.GetQQID(queryType, this.TextBox1_InputQQ.Text.Trim());
            uin = Session["QQID"].ToString();

            string tradeId = fundBLLService.GetTradeIdByUIN(uin);
            if (string.IsNullOrEmpty(tradeId))
                throw new Exception("��ѯ�����û���TradeId����ȷ�ϵ�ǰ�û��Ƿ���ע������˻�");
            ViewState["uin"] = uin;
            ViewState["tradeId"] = tradeId;
        }

        private void BindAllData()
        {
            lbUin.Text = ViewState["uin"].ToString();
            BindBasicAccountInfo(ViewState["uin"].ToString());
            try
            {
                BindFundAccountsSummary(ViewState["uin"].ToString());
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + HttpUtility.JavaScriptStringEncode(eSys.ToString()));
            }

            try//���ͨ����ѯ
            {
                DataTable subAccountInfoTable = new LCTBalanceService().QuerySubAccountInfo(ViewState["uin"].ToString(), 89);//���ͨ������89

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
                WebUtils.ShowMessage(this.Page, "��ѯ���ͨ���ʧ�ܣ�" + HttpUtility.JavaScriptStringEncode(eSys.ToString()));
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
                WebUtils.ShowMessage(this, "���ͨ���ǿ�꣺" + HttpUtility.JavaScriptStringEncode(e.ToString()));
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
                WebUtils.ShowMessage(this.Page, "���÷������" + HttpUtility.JavaScriptStringEncode(eSoap.ToString()));
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + HttpUtility.JavaScriptStringEncode(eSys.ToString()));
            }
        }

        private string GetQueryType()
        {
            if (string.IsNullOrEmpty(this.TextBox1_InputQQ.Text))
            {
                throw new Exception("������Ҫ��ѯ���˺�");
            }
            if (this.WeChatCft.Checked)
            {
                return "WeChatCft";
            }
            else if (this.WeChatUid.Checked)
            {
                return "WeChatUid";
            }
            else if (this.WeChatQQ.Checked)
            {
                return "WeChatQQ";
            }
            else if (this.WeChatMobile.Checked)
            {
                return "WeChatMobile";
            }
            else if (this.WeChatEmail.Checked)
            {
                return "WeChatEmail";
            }
            else if (this.WeChatId.Checked)
            {
                return "WeChatId";
            }

            return null;
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
                string errStr = PublicRes.GetErrorMsg(err.ToString());
                LogHelper.LogInfo("��ѯ���ͨ�˻�״̬ʧ�ܣ�" + errStr);
            }

            //�����˻���Ϣ
            //var basicFundAccountInfo = queryService.GetUserFundAccountInfo(qqId);

            var basicFundAccountInfo = fundBLLService.GetUserFundAccountInfo(qqId);
            if (basicFundAccountInfo != null && basicFundAccountInfo.Rows.Count > 0)
            {
                lblName.Text = basicFundAccountInfo.Rows[0]["Ftrue_name"].ToString();
                lblAccountStatus.Text = basicFundAccountInfo.Rows[0]["FstateName"].ToString();
                lblCell.Text = basicFundAccountInfo.Rows[0]["Fmobile"].ToString();
                lblCreateTime.Text = basicFundAccountInfo.Rows[0]["Fcreate_time"].ToString();
            }


            //��ȫ����Ϣ
            var safeCardInfo = fundBLLService.GetPayCardInfo(qqId);
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
                try
                {
                    totalBalance += long.Parse(item["balance"].ToString());
                    totalProfit += long.Parse(item["Ftotal_profit"].ToString());
                    totalMarkValue += decimal.Parse(item["markValue"].ToString());
                }
                catch
                {

                }
            }
            if (!ClassLib.ValidateRight("BalanceControl", this))
            {
                lblBalance.Text = "";
            }
            else
            {
                lblBalance.Text = classLibrary.setConfig.FenToYuan(totalBalance).Replace("Ԫ", ""); //�˻��ܽ��
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
                                                            beginDateStr: beginDate.ToString("yyyyMMdd"),
                                                            endDateStr: endDate.ToString("yyyyMMdd"),
                                                            spId: spId,
                                                            currentPageIndex: pageIndex - 1,
                                                            pageSize: pager.PageSize,
                                                            fund_code: ViewState["fundCode"].ToString());

                if (profits != null && profits.Rows.Count > 0)
                {
                    string fund_code = ViewState["fundCode"].ToString();
                    ExhibitionDataGridColumns(DataGrid_QueryResult, true, null);    //��ʾ�����ֶ� ��ѯ�û�������������ϸ
                    //���ݻ���������չʾ�ֶ�
                    if (fundBLLService.isSpecialFund(fund_code, spId)) //�׷��ﻦ��300����
                    {
                        ExhibitionDataGridColumns(DataGrid_QueryResult, false, 3, 4, 5);
                    }
                    else
                    {
                        ExhibitionDataGridColumns(DataGrid_QueryResult, false, 7, 8, 9, 10);
                    }
                    DataGrid_QueryResult.Columns[11].Visible = (string)ViewState["close_flag"] == "3"; //���ղ�չʾԤ������
                    this.DataGrid_QueryResult.DataSource = profits.DefaultView;
                    this.DataGrid_QueryResult.DataBind();
                }
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                WebUtils.ShowMessage(this.Page, "��ѯ���������¼�쳣��" + HttpUtility.JavaScriptStringEncode(eSoap.ToString()));
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ѯ���������¼�쳣:" + HttpUtility.JavaScriptStringEncode(eSys.ToString()));
            }

        }

        //��ѯ - �û��ʽ���ˮ���
        private void BindBankRollList(string qqId, string spId, string curtype, DateTime beginDate, DateTime endDate, int pageIndex = 1, int redirectionType = 0, string memo = "")
        {
            try
            {
                this.bankRollListPager.CurrentPageIndex = pageIndex;
                int max = pager.PageSize;
                int start = max * (pageIndex - 1);

                if (string.IsNullOrEmpty(spId))
                    throw new Exception("�޷�ͬʱ��ѯ���л������ˮ��Ϣ����ѡ��ָ���Ļ���");

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
                WebUtils.ShowMessage(this, string.Format("��ȡ�û��ʽ���ˮ���:{0}", PublicRes.GetErrorMsg(ex.ToString())));
            }
        }

        private void GeForceRedeemUrl(DataTable bankRollList)
        {
            if (bankRollList != null && bankRollList.Rows.Count > 0)
            {
                bool markQuerySfCar = false;
                foreach (DataRow dr in bankRollList.Rows)
                {
                    //ViewState["HasSafeCard"] = false;//�����ް�ȫ��
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
            try
            {
                if (e.Item.ItemIndex >= 0)
                {
                    var row = ((DataRowView)e.Item.DataItem).Row;  //��ǰ��
                    object obj = e.Item.Cells[8].FindControl("UnCloseFundApplyButton");
                    int index = this.bankRollListPager.CurrentPageIndex;
                    string type = e.Item.Cells[2].Text.Trim();//��ȡ
                    string fund_code = ViewState["fundCode"].ToString();
                    string spid = ViewState["fundSPId"].ToString();
                    if (obj != null)
                    {
                        LinkButton lb = (LinkButton)obj;
                        if (index == 1 && e.Item.ItemIndex == 0 && type != "����" &&
                            ViewState["close_flag"].ToString() == "1" && decimal.Parse(row["Fbalance"].ToString()) > 0)//��һҳ����һ����ˮ��¼������أ��Ҵ�ȡ״̬����Ϊ���� ��Ϊ�Ƕ��ڻ���
                        {
                            if (DateTime.Now.Hour >= 9 && DateTime.Now.Hour <= 15)//ǿ�귢��ʱ�乤����9��00��15��00������ʱ���޷�����ǿ�꣬��ť��ɫ
                            {
                                lb.Visible = true;
                            }
                            if (fundBLLService.isSpecialFund(fund_code, spid)) //ָ������
                            {
                                lb.Text = "ǿ��";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, string.Format("dgBankRollList_ItemDataBound �쳣{0}", PublicRes.GetErrorMsg(ex.ToString())));
            }
        }

        //��ѯ - �û�������ˮ���
        private void BindBankRollListNotChildren(string qqId, string spId, string curtype, DateTime beginDate, DateTime endDate, int pageIndex = 1, int redirectionType = 0)
        {
            try
            {
                this.bankRollListNotChildrenPager.CurrentPageIndex = pageIndex;
                int max = pager.PageSize;
                int start = max * (pageIndex - 1);

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
                WebUtils.ShowMessage(this, string.Format("��ȡ�û�������ˮ�쳣:{0}", PublicRes.GetErrorMsg(ex.ToString())));
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
                WebUtils.ShowMessage(this, string.Format("��ҳ�쳣:{0}", PublicRes.GetErrorMsg(ex.ToString())));
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
                WebUtils.ShowMessage(this, string.Format("��ҳ�쳣:{0}", PublicRes.GetErrorMsg(ex.ToString())));
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
                WebUtils.ShowMessage(this, string.Format("��ҳ�쳣:{0}", PublicRes.GetErrorMsg(ex.ToString())));
            }
        }

        protected void dgUserFundSummary_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            try
            {
                int rid = e.Item.ItemIndex;
                ViewState["fundSPId"] = e.Item.Cells[0].Text.Trim();
                ViewState["curtype"] = e.Item.Cells[1].Text.Trim();
                ViewState["fundCode"] = e.Item.Cells[2].Text.Trim();
                ViewState["close_flag"] = e.Item.Cells[3].Text.Trim();
                ViewState["fund_name"] = e.Item.Cells[4].Text.Trim();
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
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.ToString());
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
                var close_flag = ViewState["close_flag"].ToString();
                DataGrid_QueryResult.Columns[11].Visible = false; //���ղ�չʾԤ������

                var fundCode = ViewState["fundCode"].ToString();
                var fundSPId = ViewState["fundSPId"].ToString();
                if (close_flag == "2")//��ռ�����
                {
                    this.tableCloseFundRoll.Visible = true;
                    this.tableBankRollList.Visible = true;
                    BindCloseFundRoll(ViewState["tradeId"].ToString(), fundCode, beginDate, endDate, 1);
                    BindBankRollList(ViewState["uin"].ToString(), fundSPId, ViewState["curtype"].ToString(), beginDate, endDate, 1, redirectionType, memo);
                }
                else
                {
                    dgCloseFundRoll.DataSource = null;
                    dgCloseFundRoll.DataBind();
                    this.tableQueryResult.Visible = true;
                    this.tableBankRollList.Visible = true;
                    this.tableBankRollListNotChildren.Visible = true;

                    ExhibitionDataGridColumns(dgCloseFundRoll, true, null);         //��ʾ�����ֶ� ��ѯ������ϸ
                    if (close_flag == "3") //����
                    {
                        var hideList = new List<int>() { 0, 6, 7, 8, 9, 16 };
                        FundService fundBLLService = new FundService();
                        if (fundBLLService.isInsurance(fundCode, fundSPId)) // �⼸�� ���⴦�� ���Խ���ǿ��
                        {
                            hideList.Remove(16);
                        }
                        ExhibitionDataGridColumns(dgCloseFundRoll, false, hideList.ToArray());

                        this.tableCloseFundRoll.Visible = true;
                        BindCloseFundRoll(ViewState["tradeId"].ToString(), fundCode, beginDate, endDate, 1);
                    }
                    else if (close_flag == "1") //�����
                    {
                        ExhibitionDataGridColumns(dgCloseFundRoll, false, 4, 5);
                    }

                    BindProfitList(ViewState["tradeId"].ToString(), fundSPId, beginDate, endDate);
                    BindBankRollList(ViewState["uin"].ToString(), fundSPId, ViewState["curtype"].ToString(), beginDate, endDate, 1, redirectionType, memo);
                    BindBankRollListNotChildren(ViewState["uin"].ToString(), fundSPId, ViewState["curtype"].ToString(), beginDate, endDate, 1, redirectionType);
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + HttpUtility.JavaScriptStringEncode(eSys.ToString()));
            }
        }

        private void FetchInputDetail()
        {
            try
            {
                if (this.tbx_beginDate.Value.Trim() != "")
                    beginDate = DateTime.Parse(this.tbx_beginDate.Value);

                if (this.tbx_endDate.Value.Trim() != "")
                    endDate = DateTime.Parse(this.tbx_endDate.Value);
            }
            catch
            {
                WebUtils.ShowMessage(this, "���ڸ�ʽ����ȷ");
            }

            redirectionType = int.Parse(ddlDirection.SelectedItem.Value);
            memo = ddlMemo.SelectedItem.Value;
        }


        //��ѯ - ������ϸ
        private void BindCloseFundRoll(string tradeId, string fundCode, DateTime beginDate, DateTime endDate, int pageIndex = 1)
        {
            try
            {
                this.CloseFundRollPager.CurrentPageIndex = pageIndex;
                int max = pager.PageSize;
                int start = max * (pageIndex - 1);

                if (string.IsNullOrEmpty(tradeId) || string.IsNullOrEmpty(fundCode))
                    throw new Exception("qqId����fundCodeΪ��");

                DataTable tbCloseFundRollList = fundBLLService.QueryCloseFundRollList(tradeId, fundCode, beginDate.ToString("yyyyMMdd"), endDate.ToString("yyyyMMdd"), start, max);

                if (tbCloseFundRollList != null)
                {
                    tbCloseFundRollList.Columns.Add("FDate", typeof(string));
                    tbCloseFundRollList.Columns.Add("AlterEndStrategyURL", typeof(string));
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

                            dr["AlterEndStrategyURL"] = "GetFundRatePageDetail.aspx?opertype=2"
                                + "&uin=" + ViewState["uin"].ToString()
                                + "&fund_Code=" + ViewState["fundCode"].ToString()
                                + "&trade_id=" + tradeId
                                + "&close_listid=" + dr["Fid"].ToString()
                                + "&user_end_type=" + dr["Fuser_end_type"].ToString()
                                + "&end_sell_type=" + dr["Fend_sell_type"].ToString()
                                + "&fund_name=" + setConfig.convertToBase64(ViewState["fund_name"].ToString()); //�����ֶ� , ʹ��base64 ��ֹ����
                            ;
                        }


                    dgCloseFundRoll.DataSource = tbCloseFundRollList.DefaultView;
                    dgCloseFundRoll.DataBind();
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, string.Format("��ѯ������ϸ:���ִ���:{0}", PublicRes.GetErrorMsg(ex.ToString())));
            }
        }


        //��ջ���ͷ�ǿ�갴ť
        public void dgCloseFundRoll_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemIndex > -1)
            {
                var row = (DataRowView)e.Item.DataItem;
                if (row["Fstate_str"].ToString() == "��ִ��")
                {
                    LinkButton CloseFundApply_btn = (LinkButton)e.Item.FindControl("CloseFundApplyButton");
                    string url = "";
                    var fundCode = ViewState["fundCode"].ToString();
                    var fundSPId = ViewState["fundSPId"].ToString();

                    FundService fundBLLService = new FundService();
                    if (fundBLLService.isInsurance(fundCode, fundSPId)) // ���ʹ��ָ������  �ӿ�
                    {
                        url = "GetFundRatePageDetail.aspx?opertype=1&close_flag=3&uin=" + ViewState["uin"].ToString()
                            + "&spid=" + fundSPId
                            + "&fund_code=" + fundCode
                            + "&total_fee=" + row["Fstart_total_fee"].ToString()
                            + "&bind_serialno=" + ViewState["bind_serialno"].ToString()
                            + "&card_tail=" + ViewState["card_tail"].ToString()
                            + "&mobile=" + ViewState["mobile"].ToString()
                            + "&bank_type=" + ViewState["bank_type"].ToString()
                            + "&close_id=" + row["FDate"].ToString();
                    }
                    else
                    {
                        url = (string)row["URL"];
                    }
                    CloseFundApply_btn.Attributes.Add("href", url);

                    var AlterEndStrategy_btn = e.Item.FindControl("AlterEndStrategy");
                    CloseFundApply_btn.Visible = true;
                    AlterEndStrategy_btn.Visible = true;
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
                WebUtils.ShowMessage(this, string.Format("��ҳ�쳣:{0}", PublicRes.GetErrorMsg(ex.ToString())));
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
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + HttpUtility.JavaScriptStringEncode(eSys.ToString()));
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

                if (balanceRoll != null && balanceRoll.Rows.Count > 0)
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
                WebUtils.ShowMessage(this, string.Format("��ѯ���ͨ�����ˮ�쳣:{0}", PublicRes.GetErrorMsg(ex.ToString())));
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
                WebUtils.ShowMessage(this, string.Format("��ҳ�쳣:{0}", PublicRes.GetErrorMsg(ex.ToString())));
            }
        }

        /// <summary>
        /// ����DataGrid �ֶε���ʾ 
        /// </summary>
        /// <param name="dg">DataGrid �ؼ�����</param>
        /// <param name="visable">�ɼ���</param>
        /// <param name="Fields">�±꼯�� Fields == null ���� ȫ������</param>
        protected void ExhibitionDataGridColumns(DataGrid dg, bool visable, params int[] Fields)
        {
            for (int i = 0; i < dg.Columns.Count; i++)
            {
                if (Fields != null)
                {
                    if (Fields.Contains(i))
                    {
                        dg.Columns[i].Visible = visable;
                    }
                }
                else
                {
                    dg.Columns[i].Visible = visable;
                }
            }
        }
    }
}
