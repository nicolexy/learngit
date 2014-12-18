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
    /// GetFundRatePage 的摘要说明。
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

                // 在此处放置用户代码以初始化页面
                ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()");
                ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");

                if (IsPostBack)
                {
                    //FetchInput(); TODO:无法合理中断异常,分散到个事件中
                }
                else
                {
                    this.tbx_beginDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    this.tbx_endDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

                    //绑定基金公司列表
                  //  BindFundsList();
                    this.tableQueryResult.Visible = false;
                    this.tableBankRollList.Visible = false;
                    this.tableBankRollListNotChildren.Visible = false;
                    this.tableCloseFundRoll.Visible = false;
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
            this.dgUserFundSummary.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgUserFundSummary_ItemCommand);

        }
        #endregion

        private void FetchInput()
        {
            string uin = this.TextBox1_InputQQ.Text.Trim();
            if (string.IsNullOrEmpty(uin))
            {
                throw new Exception("微信财付通账号不能为空！");
            }

            Session["QQID"] = getQQID();
            uin = Session["QQID"].ToString();

            string tradeId = fundBLLService.GetTradeIdByUIN(uin);
            if (string.IsNullOrEmpty(tradeId))
                throw new Exception("查询不到用户的TradeId，请确认当前用户是否有注册基金账户");
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
            //    WebUtils.ShowMessage(this, "日期格式不正确");
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
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }

            try//理财通余额查询
            {
                DataTable subAccountInfoTable = new AccountService().QuerySubAccountInfo(ViewState["uin"].ToString(), 89);//理财通余额，币种89

                if (subAccountInfoTable == null || subAccountInfoTable.Rows.Count < 1)
                {
                    lbLCTBalance.Text = "0";
                }
                else
                {
                    lbLCTBalance.Text = classLibrary.setConfig.FenToYuan(subAccountInfoTable.Rows[0]["Fbalance"].ToString());//分转元
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "查询理财通余额失败！" + eSys.Message.ToString());
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
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + HttpUtility.JavaScriptStringEncode(errStr));
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(eSys.Message));
            }
        }

        string getQQID()
        {
            if (string.IsNullOrEmpty(this.TextBox1_InputQQ.Text))
            {
                throw new Exception("请输入要查询的账号");
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
                    throw new Exception("没有此用户");
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
                    throw new Exception("未拉取到基金公司记录表");

                //this.ddl_companyName.DataSource = fundCorLists;
                //this.ddl_companyName.DataTextField = "Name";
                //this.ddl_companyName.DataValueField = "SPId";
                //this.ddl_companyName.DataBind();
                //插入空选项
                //this.ddl_companyName.Items.Insert(0, new ListItem("全部", string.Empty));

            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, "基金公司列表拉取失败:" + HttpUtility.JavaScriptStringEncode(ex.Message));
            }
        }

        private void BindBasicAccountInfo(string qqId)
        {
            //基金账户信息
            var basicFundAccountInfo = queryService.GetUserFundAccountInfo(qqId);
            if (basicFundAccountInfo != null && basicFundAccountInfo.Tables.Count > 0 && basicFundAccountInfo.Tables[0].Rows.Count > 0)
            {
                lblName.Text = basicFundAccountInfo.Tables[0].Rows[0]["Ftrue_name"].ToString();
                lblAccountStatus.Text = basicFundAccountInfo.Tables[0].Rows[0]["FstateName"].ToString();
                lblCell.Text = basicFundAccountInfo.Tables[0].Rows[0]["Fmobile"].ToString();
                lblCreateTime.Text = basicFundAccountInfo.Tables[0].Rows[0]["Fcreate_time"].ToString();
            }


            //安全卡信息
            var safeCardInfo = queryService.GetPayCardInfo(qqId);
            string card_tail = "";
            string bind_serialno = "";
            string bank_type = "";
            if (safeCardInfo != null && safeCardInfo.Rows.Count > 0)
            {
                lblSafeBankCardType.Text = BankIO.QueryBankName(safeCardInfo.Rows[0]["Fbank_type"].ToString());
                lblSafeBankCardNoTail.Text = safeCardInfo.Rows[0]["Fcard_tail"].ToString();
                card_tail = safeCardInfo.Rows[0]["Fcard_tail"].ToString();
                bind_serialno = PublicRes.objectToString(safeCardInfo, "Fbind_serialno");//绑定序列号
                bank_type = safeCardInfo.Rows[0]["Fbank_type"].ToString();
            }

            //安全卡尾号及类型
            ViewState["card_tail"] = card_tail.Trim();
            ViewState["bank_type"] = bank_type.Trim();
            ViewState["mobile"] = lblCell.Text.Trim();
            ViewState["bind_serialno"] = bind_serialno.Trim();
        }

        private void BindFundAccountsSummary(string uin)
        {
            var summaryTable = new FundService().GetUserFundSummary(uin);

            summaryTable.Columns.Add("profitText", typeof(string));
            summaryTable.Columns.Add("balanceText", typeof(string)); //余额
            summaryTable.Columns.Add("conText", typeof(string));//冻结金额
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
                        dr["close_flagText"] = "不封闭";
                        break;
                    case "2":
                        dr["close_flagText"] = "封闭";
                        break;
                    case "3":
                        dr["close_flagText"] = "半封闭";
                        break;
                }
                switch (dr["transfer_flag"].ToString())
                {
                    case "0":
                        dr["transfer_flagText"] = "不支持转入、转出";
                        break;
                    case "1":
                        dr["transfer_flagText"] = "支持转入不支持转出";
                        break;
                    case "2":
                        dr["transfer_flagText"] = "支持转出不支持转入";
                        break;
                    case "3":
                        dr["transfer_flagText"] = "同时支持转入和转出";
                        break;
                }
                switch (dr["buy_valid"].ToString())
                {
                    case "1":
                        dr["buy_validText"] = "支持申购";
                        break;
                    case "2":
                        dr["buy_validText"] = "支持认购";
                        break;
                    case "4":
                        dr["buy_validText"] = "支持申购/认购";
                        break;
                }
            }
            dgUserFundSummary.DataSource = summaryTable;
            dgUserFundSummary.DataBind();

            //统计收益总和，和余额总和
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
                lblBalance.Text = classLibrary.setConfig.FenToYuan(totalBalance); //账户总金额
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
                    profits.Columns.Add("Fvalid_money_str", typeof(String));//收益本金额
                    profits.Columns.Add("Fpur_typeName", typeof(String));//科目
                    profits.Columns.Add("F7day_profit_rate_str", typeof(String));//七日年化收益
                    profits.Columns.Add("Fprofit_str", typeof(String));//收益金额
                    profits.Columns.Add("Fspname", typeof(String));//基金公司名
                    profits.Columns.Add("Fprofit_per_ten_thousand", typeof(string));//万份收益

                    foreach (DataRow dr in profits.Rows)
                    {
                        dr["Fpur_typeName"] = "分红";
                        if (!(dr["F7day_profit_rate"] is DBNull))
                        {
                            string tmp = dr["F7day_profit_rate"].ToString();
                            if (!string.IsNullOrEmpty(tmp))
                            {
                                decimal d = (decimal)(Int64.Parse(tmp)) / 100000000;
                                dr["F7day_profit_rate_str"] = d.ToString("P4");
                            }
                        }
                        //万份收益计算
                        if (dr["F1day_profit_rate"] != null && dr["F1day_profit_rate"] != DBNull.Value && dr["F1day_profit_rate"].ToString() != string.Empty)
                        {
                            decimal oneDayProfitRrate = 0;
                            decimal.TryParse(dr["F1day_profit_rate"].ToString(), out oneDayProfitRrate);
                            decimal profitPerTenThousand = oneDayProfitRrate / 10000;

                            dr["Fprofit_per_ten_thousand"] = profitPerTenThousand.ToString("N3");
                        }

                        //基金公司名
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
		        throw new Exception(string.Format("查询基金收益记录异常：{0}", ex.Message));
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
                    throw new Exception(string.Format("无法同时查询所有基金的流水信息，请选择指定的基金"));

                //var fundInfo = FundService.GetAllFundInfo().Where(i => i.SPId == spId);

                //if(fundInfo.Count() < 1)
                //    throw new Exception(string.Format("找不到{0}对应的基金信息", spId));

                var bankRollList = queryService.GetChildrenBankRollList(qqId, beginDate, endDate, curtype, start + 1, max, redirectionType, memo);

                if (bankRollList.Tables != null && bankRollList.Tables.Count > 0)
                {
                    bankRollList.Tables[0].Columns.Add("FpaynumText", typeof(string));
                    bankRollList.Tables[0].Columns.Add("FbalanceText", typeof(string)); //账户余额
                    bankRollList.Tables[0].Columns.Add("FtypeText", typeof(string));
                    bankRollList.Tables[0].Columns.Add("FmemoText", typeof(string));
                    bankRollList.Tables[0].Columns.Add("FconStr", typeof(string));
                    bankRollList.Tables[0].Columns.Add("URL", typeof(string));

                    foreach (DataRow dr in bankRollList.Tables[0].Rows)
                    {
                        switch (dr["Ftype"].ToString())
                        {
                            case "1":
                                dr["FtypeText"] = "入";
                                break;
                            case "2":
                                dr["FtypeText"] = "出";
                                break;
                            case "3":
                                dr["FtypeText"] = "冻结";
                                break;
                            case "4":
                                dr["FtypeText"] = "解冻";
                                break;
                            default:
                                dr["FtypeText"] = dr["Ftype"].ToString();
                                break;
                        }

                        switch (dr["Fmemo"].ToString())
                        {
                            case "余额宝子账户提现":
                                dr["FmemoText"] = "提现";
                                break;
                            default:
                                dr["FmemoText"] = dr["Fmemo"].ToString();
                                break;
                        }

                        string duoFund = "";
                        string listid=dr["Flistid"].ToString();
                        if (dr["FmemoText"].ToString().Equals("基金申购"))
                        {
                            if (fundBLLService.IfAnewBoughtFund(dr["Flistid"].ToString(), dr["Fcreate_time"].ToString()))
                            {
                                dr["FmemoText"]="重新申购";
                            }

                            duoFund = QueryTradeFundInfo(spId, listid);//查询多基金转换
                            dr["FmemoText"] += duoFund;
                        }

                        if (dr["FmemoText"] .ToString().Equals("提现"))
                        {
                            duoFund = QueryTradeFundInfo(spId, listid.Substring(listid.Length - 18));//查询多基金转换
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
                throw new Exception(string.Format("获取账户流水异常:{0}", ex.Message));
            }
        }

        //不封闭基金客服强赎按钮
        public void dgBankRollList_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            object obj = e.Item.Cells[7].FindControl("UnCloseFundApplyButton");
            int index = this.bankRollListPager.CurrentPageIndex;
            string type = e.Item.Cells[2].Text.Trim();//存取
            if (obj != null)
            {
                LinkButton lb = (LinkButton)obj;
                if (index == 1 && e.Item.ItemIndex == 0 && type!="冻结")//第一页，第一行流水记录才能赎回，且存取状态不能为冻结
                {
                    if (DateTime.Now.Hour >= 9 && DateTime.Now.Hour <= 14)//强赎发起时间工作日9：00－15：00，其它时间无法发起强赎，按钮灰色
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
                    duoFund = "(" + fundName + "转入)";
                if (tmp == "12")
                    duoFund = "(转出至" + fundName + ")";
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
                 //   throw new Exception(string.Format("无法同时查询所有基金的流水信息，请选择指定的基金"));

                //var fundInfo = FundService.GetAllFundInfo().Where(i => i.SPId == spId);

                //if (fundInfo.Count() < 1)
                //    throw new Exception(string.Format("找不到{0}对应的基金信息", spId));

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
                                    dr["FtypeText"] = "入";
                                    break;
                                case "2":
                                    dr["FtypeText"] = "认购";
                                    break;
                                case "3":
                                    dr["FtypeText"] = "定投";
                                    break;
                                case "4":
                                    dr["FtypeText"] = "出";
                                    break;
                                case "5":
                                    dr["FtypeText"] = "撤销";
                                    break;
                                case "6":
                                    dr["FtypeText"] = "分红";
                                    break;
                                case "7":
                                    dr["FtypeText"] = "认申购失败";
                                    break;
                                case "8":
                                    dr["FtypeText"] = "比例确认退款";
                                    break;
                                case "9":
                                    dr["FtypeText"] = "赠送收益申购";
                                    break;
                                case "10":
                                    dr["FtypeText"] = "赠送份额申购";
                                    break;
                                default:
                                    dr["FtypeText"] = dr["Fpur_type"].ToString();
                                    break;
                            }

                            switch (dr["Floading_type"].ToString())
                            {
                                case "0":
                                    dr["Floading_type_str"] = "普通赎回";
                                    break;
                                case "1":
                                    dr["Floading_type_str"] = "快速赎回";
                                    break;
                                default:
                                    dr["Floading_type_str"] = dr["Floading_type"].ToString();
                                    break;
                            }

                            if (Fpur_type != "4")
                            {//除了出其他都没有赎回方式
                                dr["Floading_type_str"] = "";
                            }

                         
                        }
                      
                        Hashtable ht = new Hashtable();
                        ht.Add("0", "创建申购单");
                        ht.Add("1", "等待扣款");
                        ht.Add("2", "代扣成功");
                        ht.Add("3", "申购成功");
                        ht.Add("4", "初始赎回单");
                        ht.Add("5", "到基金公司赎回成功");
                        ht.Add("6", "到基金公司赎回失败");
                        ht.Add("7", "申购请求失败，订单失效");
                        ht.Add("8", "申购单申请退款");
                        ht.Add("9", "申购单转入退款");
                        ht.Add("10", "赎回单受理完成");
                        ht.Add("11", "子账户提现请求成功");
                        ht.Add("20", "作废");

                        classLibrary.setConfig.DbtypeToPageContent(bankRollList, "Fstate", "Fstate_str", ht);

                        classLibrary.setConfig.FenToYuan_Table(bankRollList, "Ftotal_fee", "Ftotal_fee_str");
                       
                    }

                    dgBankRollListNotChildren.DataSource = bankRollList.DefaultView;
                    dgBankRollListNotChildren.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("获取用户交易流水异常:{0}", ex.Message));
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
                WebUtils.ShowMessage(this, string.Format("翻页异常:{0}", ex.Message));
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
                WebUtils.ShowMessage(this, string.Format("翻页异常:{0}", ex.Message));
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
                WebUtils.ShowMessage(this, string.Format("翻页异常:{0}", ex.Message));
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
                if (ViewState["close_flag"].ToString() == "2")//封闭即定期
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
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message);
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
                if (ViewState["close_flag"].ToString() == "2")//封闭即定期
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
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(eSys.Message));
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
                WebUtils.ShowMessage(this, "日期格式不正确");
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
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
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
                    throw new Exception(string.Format("qqId或者fundCode为空"));

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
                                + "&total_fee=" + dr["Fstart_total_fee"].ToString()//总金额（本金）单位分
                                + "&end_date=" + dr["Fend_date"].ToString();
                        }
                    dgCloseFundRoll.DataSource = tbCloseFundRollList.DefaultView;
                    dgCloseFundRoll.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("查询交易明细:{0}", ex.Message));
            }
        }

        //封闭基金客服强赎按钮
        public void dgCloseFundRoll_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            object obj = e.Item.Cells[12].FindControl("CloseFundApplyButton");
            string state = e.Item.Cells[8].Text.Trim();//绑定状态
            if (obj != null)
            {
                LinkButton lb = (LinkButton)obj;
                if (state == "待执行")//待执行状态才能强赎
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
                WebUtils.ShowMessage(this, string.Format("翻页异常:{0}", ex.Message));
            }
        }

        /// <summary>
        /// 理财通余额流水查询
        /// </summary>
        protected void btnBalanceQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.tableLCTBalanceRoll.Visible = true;
                this.BalanceRollPager.RecordCount = 1000;
              //  ViewState["tradeId"] = "111111111111001";//测试
                BindLCTBalanceRollList(ViewState["tradeId"].ToString(), 1);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(eSys.Message));
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
                throw new Exception(string.Format("查询理财通余额流水异常：{0}", ex.Message));
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
                WebUtils.ShowMessage(this, string.Format("翻页异常:{0}", ex.Message));
            }
        }
    }
}
