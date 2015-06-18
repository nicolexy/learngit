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
        /// <summary>
        /// 理财通余额强赎
        /// </summary>
        private void LCTFundApply() 
        {
            try
            {
                int Balance = classLibrary.setConfig.YuanToFen(Convert.ToDouble(lbLCTBalance.Text.Replace("元", "")));
                if (Balance > 0)
                {
                    string param = "opertype=1&LCTFund=true&uin=" + ViewState["uin"].ToString() + //财付通账号
                                    "&total_fee=" + classLibrary.setConfig.YuanToFen(Convert.ToDouble(lbLCTBalance.Text.Replace("元", ""))) +//提现金额(分)
                                    //"&fund_code=" + ViewState["fundcode"] +        //基金编码
                                    "&bind_serialno=" + ViewState["bind_serialno"] +    //安全卡绑定序列号
                                    "&bank_type=" + ViewState["bank_type"] +        //安全卡银行类型
                                    "&card_tail=" + ViewState["card_tail"];        //卡尾号
                    btnLCTFundApply.OnClientClick = "window.open('GetFundRatePageDetail.aspx?" + param + "'); return false;";
                    btnLCTFundApply.Visible = true;
                }
            }
            catch (Exception e)
            {
                WebUtils.ShowMessage(this, "理财通余额强赎：" + e.Message);
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
            //理财通账户冻结或解冻操作
            try
            {
                lbLCTAccState.Text = "";
                AccountService acc = new AccountService();
                string ip = Request.UserHostAddress.ToString();
                if (ip == "::1")
                    ip = "127.0.0.1";
                Boolean state = acc.LCTAccStateOperator(qqId, "3", Session["uid"].ToString(), ip);
                if (state)
                    lbLCTAccState.Text = "冻结";
                else
                    lbLCTAccState.Text = "正常";
            }
            catch (Exception err)
            {
                string errStr = PublicRes.GetErrorMsg(err.Message.ToString());
                LogHelper.LogInfo("查询理财通账户状态失败！" + errStr);
            }

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
            string mobile = "";
            if (safeCardInfo != null && safeCardInfo.Rows.Count > 0)
            {
                lblSafeBankCardType.Text = BankIO.QueryBankName(safeCardInfo.Rows[0]["Fbank_type"].ToString());
                lblSafeBankCardNoTail.Text = safeCardInfo.Rows[0]["Fcard_tail"].ToString();
                card_tail = safeCardInfo.Rows[0]["Fcard_tail"].ToString();
                bind_serialno = PublicRes.objectToString(safeCardInfo, "Fbind_serialno");//绑定序列号
                bank_type = safeCardInfo.Rows[0]["Fbank_type"].ToString();
                mobile = safeCardInfo.Rows[0]["Fmobile"].ToString();

                if (string.IsNullOrEmpty(card_tail.Trim()))//可能存在有安全卡记录，此时手机号不为空，卡号及卡类型为空
                    ViewState["HasSafeCard"] = false;
                else
                    ViewState["HasSafeCard"] = true;//标记是否有安全卡，为后面强赎做准备
            }
            else
                ViewState["HasSafeCard"] = false;

            //安全卡尾号及类型
            ViewState["card_tail"] = card_tail.Trim();
            ViewState["bank_type"] = bank_type.Trim();
            //20141216 用户绑定银行的手机号更新后会不能强赎，改为取安全卡手机号
            // ViewState["mobile"] = lblCell.Text.Trim();
            ViewState["mobile"] = mobile.Trim();
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
                lblBalance.Text = classLibrary.setConfig.FenToYuan(totalBalance).Replace("元",""); //账户总金额
            }
            
            lblTotalProfit.Text = classLibrary.setConfig.FenToYuan(totalProfit);
            lbMarkValue.Text = totalMarkValue.ToString();//市值
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

                    //根据基金来控制展示字段
                    if ( fundBLLService.isSpecialFund(fund_code, spId)) //易方达沪深300基金
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
		        throw new Exception(string.Format("查询基金收益记录异常：{0}", PublicRes.GetErrorMsg(ex.Message.ToString())));
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

            //   var bankRollList = queryService.GetChildrenBankRollList(qqId, beginDate, endDate, curtype, start + 1, max, redirectionType, memo);
                var bankRollList = fundBLLService.GetChildrenBankRollListEx(qqId, spId, curtype, beginDate, endDate, start, max, redirectionType, memo);

                //获取强赎申请URL,未提供强赎功能接口到客服部
                GeForceRedeemUrl(bankRollList);
                string fund_code = ViewState["fundCode"].ToString();
                if (fundBLLService.isSpecialFund(fund_code, spId)) //易方达沪深300基金
                {
                    this.dgBankRollList.Columns[3].HeaderText = "基金份额";
                    this.dgBankRollList.Columns[4].HeaderText = "份额余额";
                }
                else
                {
                    this.dgBankRollList.Columns[3].HeaderText = "金额";
                    this.dgBankRollList.Columns[4].HeaderText = "账户余额";
                }
                dgBankRollList.DataSource = bankRollList.DefaultView;
                dgBankRollList.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("获取账户流水异常:{0}", PublicRes.GetErrorMsg(ex.Message)));
            }
        }

        private void GeForceRedeemUrl(DataTable bankRollList)
        {
            if (bankRollList != null && bankRollList.Rows.Count > 0)
            {
                bool markQuerySfCar = false;
                foreach (DataRow dr in bankRollList.Rows)
                {
                  //  ViewState["HasSafeCard"] = false;//测试无安全卡
                    if (ViewState["close_flag"].ToString() == "2")//封闭基金
                    {
                        dr["URL"] = "";
                    }
                    else if (ViewState["close_flag"].ToString() == "1")//不封闭基金
                    {
                        try//无安全卡 取绑定卡信息
                        {
                            //非定期基金强赎：
                            //有安全卡往安全卡赎回，
                            //无安全卡往绑定快捷卡赎回，
                            //无绑定卡往解除绑定卡赎回
                            if (!(bool)(ViewState["HasSafeCard"]) && !markQuerySfCar)//无安全卡  
                            {
                                markQuerySfCar = true;

                                //先取快捷、已绑定状态的卡
                                DataSet ds = queryService.GetBankCardBindList_2(ViewState["uin"].ToString(), "", "", "", "", "", "", "", "", 2, 2, 0, 1);
                                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
                                {
                                    //取快捷、解绑状态的卡
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
                        dr["FbalanceText"] = "";//没有权限就不显示余额
                    }

                }
            }
        }

        //不封闭基金客服强赎按钮
        public void dgBankRollList_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            object obj = e.Item.Cells[8].FindControl("UnCloseFundApplyButton");
            int index = this.bankRollListPager.CurrentPageIndex;
            string type = e.Item.Cells[2].Text.Trim();//存取
             string fund_code = ViewState["fundCode"].ToString();
             string spid = ViewState["fundSPId"].ToString();
            if (obj != null)
            {
                LinkButton lb = (LinkButton)obj;
                if (index == 1 && e.Item.ItemIndex == 0 && type!="冻结"&&
                    ViewState["close_flag"].ToString() == "1")//第一页，第一行流水记录才能赎回，且存取状态不能为冻结 且为非定期基金
                {
                    if (!fundBLLService.isSpecialFund(fund_code, spid))//易方达沪深300强赎先隐藏，无接口
                    {
                    if (DateTime.Now.Hour >= 9 && DateTime.Now.Hour <= 15)//强赎发起时间工作日9：00－15：00，其它时间无法发起强赎，按钮灰色
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

                //  DataTable bankRollList = fundBLLService.GetFundRollList(qqId, beginDate, endDate, curtype, start, max, redirectionType);
                //更换成与API接口一致
                string fund_code = ViewState["fundCode"].ToString();
                DataTable bankRollList = fundBLLService.BindBankRollListNotChildren(qqId, curtype, beginDate, endDate, start, max, redirectionType, spId, fund_code);
                if (bankRollList != null)
                {

                    if (fundBLLService.isSpecialFund(fund_code, spId)) //易方达沪深300基金
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
                throw new Exception(string.Format("获取用户交易流水异常:{0}", PublicRes.GetErrorMsg(ex.Message)));
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
                WebUtils.ShowMessage(this, string.Format("翻页异常:{0}", PublicRes.GetErrorMsg(ex.Message)));
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
                WebUtils.ShowMessage(this, string.Format("翻页异常:{0}", PublicRes.GetErrorMsg(ex.Message)));
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
                WebUtils.ShowMessage(this, string.Format("翻页异常:{0}", PublicRes.GetErrorMsg(ex.Message)));
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
                    tbCloseFundRollList.Columns.Add("FDate", typeof(string));

                    tbCloseFundRollList.Columns.Add("URL", typeof(string));
                    if (tbCloseFundRollList.Rows.Count > 0)
                        foreach (DataRow dr in tbCloseFundRollList.Rows)
                        {
                            dr["URL"] = "GetFundRatePageDetail.aspx?opertype=1&close_flag=2&uin=" + ViewState["uin"].ToString()
                                + "&spid=" + ViewState["fundSPId"].ToString()
                                + "&fund_code=" + ViewState["fundCode"].ToString()
                                + "&total_fee=" + dr["Fstart_total_fee"].ToString()//总金额（本金）单位分
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
                throw new Exception(string.Format("查询交易明细:{0}", PublicRes.GetErrorMsg(ex.Message)));
            }
        }

        //封闭基金客服强赎按钮
        public void dgCloseFundRoll_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            object obj = e.Item.Cells[12].FindControl("CloseFundApplyButton");
            string state = e.Item.Cells[9].Text.Trim();//绑定状态
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
                WebUtils.ShowMessage(this, string.Format("翻页异常:{0}", PublicRes.GetErrorMsg(ex.Message)));
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
                throw new Exception(string.Format("查询理财通余额流水异常：{0}", PublicRes.GetErrorMsg(ex.Message)));
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
                WebUtils.ShowMessage(this, string.Format("翻页异常:{0}", PublicRes.GetErrorMsg(ex.Message)));
            }
        }
    }
}
