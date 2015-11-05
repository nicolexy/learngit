namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using System.Web.UI;

    using Microsoft.Web.UI.WebControls;
    using TENCENT.OSS.CFT.KF.KF_Web;
    using TENCENT.OSS.CFT.KF.Common;

    /// <summary>
    ///		LeftControl 的摘要说明。
    /// </summary>
    public partial class LeftControl : System.Web.UI.UserControl
    {
        protected BaseAccountControl baseAccount1;
        protected AccountOperateControl accountOperate1;
        protected TradeManageControl tradeManage1;
        protected AccountManageControl accountManage1;
        protected RiskConManage RiskConManage1;
        protected OverseasPay OverseasPay1;

        protected AccountLedgerManageControl AccountLedgerManage1;
        protected AccountOperaManageControl AccountOperaManage1;
        protected BankBillManageControl BankBillManage1;
        protected FastPayControl FastPay1;
        protected FundAccountManageControl FundAccountManage1;
        protected LifeFeeDetailControl LifeFeeDetailManage1;
        protected MicroPay MicroPay1;
        protected NameAuthenedControl NameAuthened1;
        protected SelfHelpAppealManageControl SelfHelpAppealManage1;
        protected VIPAccountManageControl VIPAccountManage;
        protected FreezeManageControl FreezeManage1;
        protected SpecialManageControl SpecialManageControl1;
        protected DKManageControl DKManageControl1;
        protected DFManageControl DFManageControl1;
        protected ActivityCooperation ActivityCooperation1;
        protected TokenCoin TokenCoin1;
        protected SysManage SysManage1;
        protected CreditPayControl CreditPayControl1;
        protected WebchatPayControl WebchatPayControl1;
        protected TravelPlatform TravelPlatform1;
        protected ForeignCurrencyPay ForeignCurrencyPay1;
        protected ForeignCurrencyAccount ForeignCurrencyAccount1;
        protected ForeignCardPay ForeignCardPay1;
        protected PNRQueryControl PNRQuery1;
        

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (Session["uid"] == null || Session["uid"].ToString() == "")
                {
                    throw new Exception("超时，请重新登录！");
                }
            }
            catch
            {
                Response.Redirect("../KF_Web/timeout.aspx");
            }


            // 在此处放置用户代码以初始化页面
            if (!this.IsPostBack)
            {
                InitAllMenu();

                try
                {
                    SysManage1.Visible = true;
                    ActivityCooperation1.Visible = true;
                    baseAccount1.Visible = false;
                    accountOperate1.Visible = false;
                    tradeManage1.Visible = false;
                    accountManage1.Visible = false;
                    RiskConManage1.Visible = true;
                    OverseasPay1.Visible = true;
                    TravelPlatform1.Visible = true;


                    AccountLedgerManage1.Visible = true;
                    AccountOperaManage1.Visible = true;
                    BankBillManage1.Visible = true;
                    FastPay1.Visible = true;
                    FundAccountManage1.Visible = true;
                    LifeFeeDetailManage1.Visible = true;
                    MicroPay1.Visible = true;
                    ForeignCurrencyPay1.Visible = true;
                    ForeignCardPay1.Visible = true;
                    ForeignCurrencyAccount1.Visible = true;
                    NameAuthened1.Visible = true;
                    SelfHelpAppealManage1.Visible = true;
                    VIPAccountManage.Visible = true;

                    DKManageControl1.Visible = true;
                    DFManageControl1.Visible = true;

                    CreditPayControl1.Visible = true;
                    WebchatPayControl1.Visible = true;
                    PNRQuery1.Visible = true;

                    if (classLibrary.getData.IsTestMode)
                    {
                        FreezeManage1.Visible = true;
                        SpecialManageControl1.Visible = true;
                    }
                    else
                    {
                        FreezeManage1.Visible = false;
                        SpecialManageControl1.Visible = false;
                    }


                    string szkey = Session["SzKey"].ToString();
                    //int operid = Int32.Parse(Session["OperID"].ToString());

                    //if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "baseAccount"))
                    if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
                    {
                        baseAccount1.Visible = true;
                        accountOperate1.Visible = true;
                        accountManage1.Visible = true;
                    }

                    if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("PayManagement", this))  //支付管理下的菜单，都需要申请该权限
                    {
                        WebchatPayControl1.Visible=false;
                        FastPay1.Visible=false;
                        CreditPayControl1.Visible=false;
                        OverseasPay1.Visible=false;
                        MicroPay1.Visible=false;
                        ForeignCurrencyPay1.Visible=false;
                        ForeignCardPay1.Visible=false;
                        HandQBusiness.Visible = false;
                    }

                    //if (AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "tradeManage"))
                    if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeManagement", this))
                    {
                        tradeManage1.Visible = true;
                    }
                    TokenCoin1.Visible = true;
                }
                catch (Exception ex)
                {
                    //				Response.Redirect("连接超时！");
                    string str = ex.Message;
                }
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
        ///		设计器支持所需的方法 - 不要使用代码编辑器
        ///		修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion



        private void InitAllMenu()
        {
            if (classLibrary.ClassLib.ValidateRight("InfoCenter", this))
            {
                TravelPlatform1.AddSubMenu("机票订单查询", "TravelPlatform/AirTicketsOrderQuery.aspx");
                TravelPlatform1.AddSubMenu("酒店订单查询", "TravelPlatform/HotelOrderQuery.aspx");
                // ActivityCooperation1.AddSubMenu("满减用户查询", "BaseAccount/ManJianUserQuery.aspx");
                ActivityCooperation1.AddSubMenu("满减使用查询", "BaseAccount/ManJianUsingQuery.aspx");
                ActivityCooperation1.AddSubMenu("拍拍店铺白名单", "NewQueryInfoPages/PaiPaiBMDQuery.aspx");
                SelfHelpAppealManage1.AddSubMenu("我发起的审批", "BaseAccount/StartCheck.aspx");
                AccountLedgerManage1.AddSubMenu("分账订单流水", "BaseAccount/SeparateOperation.aspx");
                //AccountLedgerManage1.AddSubMenu("分账订单金额","TradeManage/SeparateListQuery.aspx");
                //AccountLedgerManage1.AddSubMenu("收支分离查询","TradeManage/SettleRule.aspx");
                AccountLedgerManage1.AddSubMenu("分账订单查询", "TradeManage/SettleInfo.aspx");
                AccountLedgerManage1.AddSubMenu("分账退款查询", "TradeManage/SettleRefund.aspx");
                AccountLedgerManage1.AddSubMenu("冻结解冻查询", "TradeManage/SettleFreeze.aspx");
                AccountLedgerManage1.AddSubMenu("调账订单查询", "TradeManage/AdjustList.aspx");
                AccountLedgerManage1.AddSubMenu("代理分账关系", "TradeManage/SettleAgent.aspx");
                AccountLedgerManage1.AddSubMenu("分账账户明细", "TradeManage/SettleInfoDetail.aspx");
                AccountLedgerManage1.AddSubMenu("分账请求查询", "SpSettle/SettleReqQuery.aspx");
                AccountLedgerManage1.AddSubMenu("分账明细查询", "SpSettle/SettleReqDetail.aspx");
                AccountLedgerManage1.AddSubMenu("调帐查询", "SpSettle/AdjustQuery.aspx");
                AccountLedgerManage1.AddSubMenu("分账规则查询", "SpSettle/SettleRuleQuery.aspx");
                AccountLedgerManage1.AddSubMenu("商户权限查询", "SpSettle/SpControlQuery.aspx");
                AccountLedgerManage1.AddSubMenu("补差关系查询", "SpSettle/OrderRelationQuery.aspx");
                AccountLedgerManage1.AddSubMenu("补差明细查询", "SpSettle/OrderInfoQuery.aspx");
                AccountLedgerManage1.AddSubMenu("补差账户查询", "SpSettle/OrderAccountQuery.aspx");

                accountManage1.AddSubMenu("提现规则查询", "TradeManage/AppealDSettings.aspx");
               
                accountManage1.AddSubMenu("直付商户查询", "BaseAccount/PayBusinessQuery.aspx");
                accountManage1.AddSubMenu("中介商户查询", "BaseAccount/AgencyBusinessQuery.aspx");
                accountManage1.AddSubMenu("结算查询", "TradeManage/SettQuery.aspx");
                accountManage1.AddSubMenu("结算查询(新)", "TradeManage/SettQueryNew.aspx");
                accountManage1.AddSubMenu("合同查询", "TradeManage/ContractQuery.aspx");
                accountManage1.AddSubMenu("证书管理查询", "TradeManage/MediCertManage.aspx");
                accountManage1.AddSubMenu("证书CGI查询", "TradeManage/CGIInfoQuery.aspx");
                accountManage1.AddSubMenu("证书到期查询", "TradeManage/MediCertExpireQuery.aspx");
                accountManage1.AddSubMenu("自动扣费协议", "TradeManage/PayLimitManage.aspx");
                accountManage1.AddSubMenu("T+0付款查询", "TradeManage/BatPayQuery.aspx");
                //	accountManage1.AddSubMenu("PNR签约关系查询","BaseAccount/PNRQuery.aspx") ;
                accountManage1.AddSubMenu("同步记录查询", "TradeManage/SynRecordQuery.aspx");
                accountManage1.AddSubMenu("收付易查询", "TradeManage/ShouFuYiQuery.aspx");
                accountManage1.AddSubMenu("保证金账户查询", "BaseAccount/DepositAccountQuery.aspx");
                accountManage1.AddSubMenu("微信/财付通商户信息互查", "WebchatPay/WechatMerchantTrans.aspx");

                if (classLibrary.ClassLib.ValidateRight("MobileConfig", this))
                {
                    accountManage1.AddSubMenu("方案有效期", "TradeManage/MobileRechargeConfigValidDate.aspx");
                    accountManage1.AddSubMenu("分流配置", "TradeManage/MobileRechargeConfig.aspx");
                }
                accountManage1.AddSubMenu("商户交易清单", "TradeManage/TradeLogList.aspx");
                accountManage1.AddSubMenu("商户订单查询财付通订单", "TradeManage/QuerySpOrderPage.aspx");
                accountManage1.AddSubMenu("自助BD商户查询", "BaseAccount/SelfQuery.aspx");

                AccountOperaManage1.AddSubMenu("商户资料修改", "BaseAccount/ModifyBusinessInfo.aspx");
                AccountOperaManage1.AddSubMenu("修改联系信息", "BaseAccount/ModifyContactInfo.aspx");
                AccountOperaManage1.AddSubMenu("商户信息修改查询", "BaseAccount/MerchantInfoModifyQuery.aspx");
                AccountOperaManage1.AddSubMenu("商户身份证修改", "BaseAccount/UpdateMerchantCre.aspx");
                AccountOperaManage1.AddSubMenu("商户冻结申请", "TradeManage/BusinessFreeze.aspx");
                AccountOperaManage1.AddSubMenu("商户关闭退款申请", "TradeManage/ShutRefund.aspx");
                AccountOperaManage1.AddSubMenu("商户开通退款申请", "TradeManage/ApplyRefund.aspx");
                AccountOperaManage1.AddSubMenu("商户恢复申请", "TradeManage/BusinessResume.aspx");
                AccountOperaManage1.AddSubMenu("商户退单撤销", "RefundManage/SuspendRefundment.aspx");

                //暂且屏蔽该菜单，后期可能会打开，不可删除
                //AccountOperaManage1.AddSubMenu("商户注销退出", "TradeManage/BusinessLogout.aspx");

                AccountOperaManage1.AddSubMenu("投诉商户录入", "TradeManage/ComplainBussinessInput.aspx");
                AccountOperaManage1.AddSubMenu("用户投诉", "TradeManage/ComplainUserInput.aspx");


                SysManage1.AddSubMenu("系统公告管理", "SysManage/SysBulletinManage.aspx");
                SysManage1.AddSubMenu("常见问题维护", "SysManage/QuestionManage.aspx");
                SysManage1.AddSubMenu("银行接口维护管理", "SysManage/BankInterfaceManage.aspx");

                BankBillManage1.AddSubMenu("汇总退单数据", "TradeManage/RefundMain.aspx");
                BankBillManage1.AddSubMenu("退单汇总查询", "RefundManage/RefundTotalQuery.aspx");
                BankBillManage1.AddSubMenu("订单实时查询", "TradeManage/RealTimeOrderQuery.aspx");
              //  BankBillManage1.AddSubMenu("异常任务单管理", "RefundManage/RefundErrorMain.aspx?WorkType=task");
                BankBillManage1.AddSubMenu("退单异常数据查询", "RefundManage/RefundErrorHandle.aspx");
                BankBillManage1.AddSubMenu("退款失败查询", "RefundManage/RefundRegistration.aspx");

                BankBillManage1.AddSubMenu("付款异常查询通知", "RefundManage/PaymentAbnormalQueryNotify.aspx");
                baseAccount1.AddSubMenu("个人账户信息", "BaseAccount/InfoCenter.aspx");              
                baseAccount1.AddSubMenu("QQ帐号回收", "BaseAccount/QQReclaim.aspx");
                accountOperate1.AddSubMenu("账户姓名修改", "BaseAccount/changeUserName_2.aspx");
                baseAccount1.AddSubMenu("用户受控资金查询", "TradeManage/QueryUserControledFinPage.aspx");
                baseAccount1.AddSubMenu("用户受控资金查询-新", "TradeManage/UserControledFundPage.aspx");
              //  baseAccount1.AddSubMenu("手机绑定查询", "TradeManage/MobileBindQuery.aspx");
                baseAccount1.AddSubMenu("手机绑定查询", "TradeManage/MobileBindingQuery.aspx");   
                accountOperate1.AddSubMenu("证件号码清理", "BaseAccount/ClearCreidNew.aspx");
                baseAccount1.AddSubMenu("腾讯信用查询", "BaseAccount/TencentCreditQuery.aspx");

                FastPay1.AddSubMenu("一点通业务", "BaseAccount/BankCardUnbind.aspx");
                FastPay1.AddSubMenu("一点通业务(新)", "BaseAccount/BankCardUnbindNew.aspx");
               // FastPay1.AddSubMenu("银行卡查询", "TradeManage/BankCardQuery.aspx");
                FastPay1.AddSubMenu("银行卡查询", "TradeManage/BankCardQueryNew.aspx");
                FastPay1.AddSubMenu("姓名生僻字", "BaseAccount/RareNameQuery.aspx");
                FastPay1.AddSubMenu("卡信息查询", "BaseAccount/CardInfoQuery.aspx");

                FundAccountManage1.AddSubMenu("签约解约查询", "NewQueryInfoPages/QueryInverestorSignPage.aspx");
                FundAccountManage1.AddSubMenu("基金交易查询", "NewQueryInfoPages/QueryFundInfoPage.aspx");
                FundAccountManage1.AddSubMenu("基金流水查询", "NewQueryInfoPages/QueryChargeInfoPage.aspx");
                FundAccountManage1.AddSubMenu("基金账户查询", "NewQueryInfoPages/GetUserFundAccountInfoPage.aspx");
                //FundAccountManage1.AddSubMenu("理财通查询", "NewQueryInfoPages/GetFundRatePage.aspx");

                LifeFeeDetailManage1.AddSubMenu("生活缴费查询", "TradeManage/FeeQuery.aspx");
                //LifeFeeDetailManage1.AddSubMenu("邮储汇款查询", "RemitCheck/RemitQuery.aspx");
                LifeFeeDetailManage1.AddSubMenu("邮储汇款查询", "RemitCheck/RemitQueryNew.aspx");
                LifeFeeDetailManage1.AddSubMenu("手机充值卡查询", "TradeManage/FundCardQuery_Detail.aspx");
                LifeFeeDetailManage1.AddSubMenu("手机充值卡查询-新", "TradeManage/FundCardQuery_DetailNew.aspx");
                //LifeFeeDetailManage1.AddSubMenu("信用卡还款", "TradeManage/CreditQuery.aspx");
                LifeFeeDetailManage1.AddSubMenu("信用卡还款", "TradeManage/CreditQueryNew.aspx");
                LifeFeeDetailManage1.AddSubMenu("话费发货查询", "TradeManage/PhoneBillQuery.aspx");
                LifeFeeDetailManage1.AddSubMenu("自动充值", "TradeManage/AutomaticRechargeQuery.aspx");
                LifeFeeDetailManage1.AddSubMenu("公交卡充值查询", "TradeManage/BusCardPrepaidQuery.aspx");

                RiskConManage1.AddSubMenu("二次登录密码撤销", "Trademanage/SuspendSecondPasseword.aspx");
                RiskConManage1.AddSubMenu("资金流水查询", "BaseAccount/BankrollHistoryLog.aspx");
                RiskConManage1.AddSubMenu("风控解冻审核", "FreezeManage/FreezeQuery.aspx");
                RiskConManage1.AddSubMenu("特殊申诉处理", "FreezeManage/FreezeNewQuery.aspx");
                RiskConManage1.AddSubMenu("报表统计输出", "FreezeManage/FreezeCount.aspx");
                RiskConManage1.AddSubMenu("手机绑定数清理", "BaseAccount/ClearMobileNumber.aspx");
                SelfHelpAppealManage1.AddSubMenu("客服统计查询", "BaseAccount/KFTotalQuery.aspx");

                //tradeManage1.AddSubMenu("提现记录查询", "TradeManage/PickQuery.aspx");
                tradeManage1.AddSubMenu("提现记录查询(新)", "TradeManage/PickQueryNew.aspx");
                tradeManage1.AddSubMenu("退款单查询", "TradeManage/B2CReturnQuery.aspx");
                

                DFManageControl1.AddSubMenu("代付单笔查询", "NewQueryInfoPages/DFDetailQuery.aspx");
                DFManageControl1.AddSubMenu("代付批量查询", "NewQueryInfoPages/DFBatchQuery.aspx");
                DKManageControl1.AddSubMenu("代扣单笔查询", "NewQueryInfoPages/QueryDKInfoPage.aspx");
                DKManageControl1.AddSubMenu("代扣批量查询", "NewQueryInfoPages/QueryDKListInfoPage.aspx");
                DFManageControl1.AddSubMenu("财付通转账", "NewQueryInfoPages/CFTTransferQuery.aspx");


                DKManageControl1.AddSubMenu("银行批次查询", "NewQueryInfoPages/QueryBankListInfoPage.aspx");
                DKManageControl1.AddSubMenu("代扣限额查询", "NewQueryInfoPages/QueryDKLimitPage.aspx");
                DKManageControl1.AddSubMenu("商户特性查询", "NewQueryInfoPages/QueryDKServicePage.aspx");
                DKManageControl1.AddSubMenu("协议明细查询", "NewQueryInfoPages/QueryDKcontractPage.aspx");
                DKManageControl1.AddSubMenu("协议批次查询", "NewQueryInfoPages/QueryDKcontractListPage.aspx");

                DKManageControl1.AddSubMenu("协议库导入", "NewQueryInfoPages/ProtocolLibImport.aspx");
                DKManageControl1.AddSubMenu("协议库查询", "NewQueryInfoPages/QueryProtocolLib.aspx");

                tradeManage1.AddSubMenu("用户手机充值记录查询", "TradeManage/MobileRechargeQuery.aspx");
                tradeManage1.AddSubMenu("银行订单号查询", "TradeManage/BankBillNoQuery.aspx");

				VIPAccountManage.AddSubMenu("财付值流水","VIPAccount/PropertyTurnover.aspx");
                VIPAccountManage.AddSubMenu("银行卡查询", "VIPAccount/QueryBankCard.aspx");
              //  VIPAccountManage.AddSubMenu("财付值流水", "VIPAccount/PropertyTurnover.aspx");
                VIPAccountManage.AddSubMenu("图标管理", "VIPAccount/IconManagement.aspx");

                OverseasPay1.AddSubMenu("外卡交易查询", "NewQueryInfoPages/QueryForeignCard.aspx");
                OverseasPay1.AddSubMenu("运通账号信息查询", "NewQueryInfoPages/QueryYTInfo.aspx");
                OverseasPay1.AddSubMenu("运通账号冻结查询", "NewQueryInfoPages/QueryYTFreeze.aspx");
                OverseasPay1.AddSubMenu("运通账号交易查询", "NewQueryInfoPages/QueryYTTrade.aspx");
                OverseasPay1.AddSubMenu("中行账号信息查询", "NewQueryInfoPages/QueryZHInfo.aspx");
                OverseasPay1.AddSubMenu("中行账号冻结查询", "NewQueryInfoPages/QueryZHFreeze.aspx");
                OverseasPay1.AddSubMenu("中行账号交易查询", "NewQueryInfoPages/QueryZHTrade.aspx");
                OverseasPay1.AddSubMenu("外汇汇率查询", "NewQueryInfoPages/QueryForeignExchangeRate.aspx");
                OverseasPay1.AddSubMenu("境外微信小额支付查询", "NewQueryInfoPages/QueryWeiXinMircoPay.aspx");

                InternetBank.AddSubMenu("流水查询", "InternetBank/TurnoverQuery.aspx");
                InternetBank.AddSubMenu("账单查询", "InternetBank/BillQuery.aspx");
                InternetBank.AddSubMenu("历史账单", "InternetBank/BillHistoryQuery.aspx");
                InternetBank.AddSubMenu("会员优惠额度", "InternetBank/MermberDiscount.aspx");
                InternetBank.AddSubMenu("退款登记", "InternetBank/RefundQuery.aspx");
                InternetBank.AddSubMenu("退款商户录入", "InternetBank/RefundMerchant.aspx");
                InternetBank.AddSubMenu("自动续费查询", "InternetBank/AtuoRenewQuery.aspx");
                //手Q支付
                HandQBusiness.AddSubMenu("手Q红包查询", "HandQBusiness/FindHandQRedPacket.aspx");
                HandQBusiness.AddSubMenu("手Q还款查询", "HandQBusiness/RefundHandQQuery.aspx");

                //活动合作
                ActivityCooperation1.AddSubMenu("打折密码", "NewQueryInfoPages/QueryDiscountCode.aspx");
                ActivityCooperation1.AddSubMenu("用户参加的活动", "NewQueryInfoPages/QueryUserJoinActivity.aspx");
                ActivityCooperation1.AddSubMenu("活动日志查询", "NewQueryInfoPages/QueryActivityLogs.aspx");
                ActivityCooperation1.AddSubMenu("微信活动查询", "NewQueryInfoPages/QueryWebchatPayActivity.aspx");
                ActivityCooperation1.AddSubMenu("乐刷卡活动查询", "Activity/QueryLeShuaKaActivity.aspx");
                ActivityCooperation1.AddSubMenu("理财通活动查询", "Activity/QueryLCTActivity.aspx");
                ActivityCooperation1.AddSubMenu("新活动添加", "Activity/LctActivityAdd.aspx");

                //财付券管理
                TokenCoin1.AddSubMenu("财付券查询", "TokenCoin/GwqQuery.aspx");
                TokenCoin1.AddSubMenu("财付券查询明细", "TokenCoin/GwqShow.aspx");

                //信用支付
                CreditPayControl1.AddSubMenu("基本信息", "CreditPay/QueryCreditUserInfo.aspx");
                CreditPayControl1.AddSubMenu("账单查询", "CreditPay/QueryCreditBillList.aspx");
                CreditPayControl1.AddSubMenu("欠款查询", "CreditPay/QueryCreditDebt.aspx");
                CreditPayControl1.AddSubMenu("还款查询", "CreditPay/QueryRefund.aspx");
                CreditPayControl1.AddSubMenu("资金流水查询", "CreditPay/QueryCapitalRoll.aspx");

                //微信支付
                WebchatPayControl1.AddSubMenu("微信支付帐号", "WebchatPay/WechatInfoQuery.aspx");
                WebchatPayControl1.AddSubMenu("AA收款帐号", "WebchatPay/WechatAACollection.aspx");
                WebchatPayControl1.AddSubMenu("理财通查询", "NewQueryInfoPages/GetFundRatePage.aspx");
                WebchatPayControl1.AddSubMenu("理财通安全卡", "WebchatPay/SafeCardManage.aspx");
                WebchatPayControl1.AddSubMenu("微信红包查询", "WebchatPay/WechatRedPacket.aspx");
                WebchatPayControl1.AddSubMenu("小额刷卡", "WebchatPay/SmallCreditCardQuery.aspx");
                WebchatPayControl1.AddSubMenu("微信信用卡还款", "WebchatPay/CreditCardRefundQuery.aspx");
                WebchatPayControl1.AddSubMenu("理财通增值券", "WebchatPay/AddedValueTicketQuery.aspx");
                WebchatPayControl1.AddSubMenu("合约机查询", "WebchatPay/QueryContractMachine.aspx");
                WebchatPayControl1.AddSubMenu("实时还款查询", "WebchatPay/QueryRealtimeRepayment.aspx");
            }

            if (classLibrary.ClassLib.ValidateRight("DKAdjust", this))
            {
                DKManageControl1.AddSubMenu("批量文件调整", "NewQueryInfoPages/DKAdjust.aspx?querytype=fileselect");
                //DKManageControl1.AddSubMenu("调整状态查询", "NewQueryInfoPages/DK_QueryAdjust.aspx");
            }
            /*
			if(classLibrary.ClassLib.ValidateRight("DrawAndApprove",this))
			{
				AccountOperaManage1.AddSubMenu("自助商户领单","BaseAccount/SelfQuery.aspx") ;
				AccountOperaManage1.AddSubMenu("自助商户审核","BaseAccount/SelfQueryApprove.aspx") ;
				AccountOperaManage1.AddSubMenu("商户申请修改资料审核","BaseAccount/DomainApprove.aspx") ;
				AccountOperaManage1.AddSubMenu("商户营改增审核","BaseAccount/ValueAddedTaxApprove.aspx") ;
				AccountOperaManage1.AddSubMenu("商户营改增查询","BaseAccount/ValueAddedTaxQuery.aspx") ;
			}

			if(classLibrary.ClassLib.ValidateRight("TradeLogQuery",this))
			{
				BankBillManage1.AddSubMenu("汇总付款数据","BaseAccount/batPay.aspx") ;

				MediumTradeManage1.AddSubMenu("中介订单查询","TradeManage/OrderQuery.aspx") ;

				tradeManage1.AddSubMenu("交易记录查询","TradeManage/TradeLogQuery.aspx") ;
			}

			if(classLibrary.ClassLib.ValidateRight("FundQuery",this))
			{
				BankBillManage1.AddSubMenu("订单实时调帐","TradeManage/RealtimeOrder.aspx") ;

				tradeManage1.AddSubMenu("充值记录查询","TradeManage/FundQuery.aspx") ;
			}

			if(classLibrary.ClassLib.ValidateRight("UserBankInfoQuery",this))
			{
				baseAccount1.AddSubMenu("银行账号信息","BaseAccount/UserBankInfoQuery.aspx") ;
			}

			if(classLibrary.ClassLib.ValidateRight("ChangeUserInfo",this))
			{
				baseAccount1.AddSubMenu("个人信息","BaseAccount/ChangeUserInfo.aspx") ;
				MicroPay1.AddSubMenu("子帐户查询","BaseAccount/ChildrenQuery.aspx");
				MicroPay1.AddSubMenu("子帐户订单查询","BaseAccount/ChildrenOrderFromQuery.aspx");
				MicroPay1.AddSubMenu("子帐户订单查询(新)","BaseAccount/ChildrenOrderFromQueryNew.aspx");
                MicroPay1.AddSubMenu("历史订单查询", "BaseAccount/ChildrenHistoryOrderQuery.aspx");
			}

			if(classLibrary.ClassLib.ValidateRight("UserReport",this))
			{
				baseAccount1.AddSubMenu("意见投诉查询","BaseAccount/userReport.aspx") ;
			}

			if(classLibrary.ClassLib.ValidateRight("HistoryModify",this))
			{
				baseAccount1.AddSubMenu("信息修改历史","BaseAccount/historyModify.aspx") ;
			}
            */
            if (classLibrary.ClassLib.ValidateRight("InfoCenter", this) || TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("DeleteCrt", this))
            {
                RiskConManage1.AddSubMenu("个人证书管理", "Trademanage/CrtQuery.aspx");
            }

            //if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("FreezeList",this))
            //{
            //	RiskConManage1.AddSubMenu("财付盾查询","BaseAccount/CFDQuery.aspx") ;
            //    RiskConManage1.AddSubMenu("手机令牌", "Trademanage/MobileTokenQuery.aspx");
            //	RiskConManage1.AddSubMenu("冻结操作查询","BaseAccount/FreezeList.aspx") ;
            //	RiskConManage1.AddSubMenu("冻结资金记录","BaseAccount/FreezeFinQuery.aspx");

            //}

            //if(classLibrary.ClassLib.ValidateRight("CFTUserAppeal",this))
            //{
            //	SelfHelpAppealManage1.AddSubMenu("自助申诉查询","BaseAccount/CFTUserAppeal.aspx") ;
            //}


            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("CFTUserPick", this))
            {
                SelfHelpAppealManage1.AddSubMenu("申诉处理(新)", "BaseAccount/UserAppeal.aspx");
            }

            //if(classLibrary.ClassLib.ValidateRight("CancelAccount",this))
            //{
            //	SelfHelpAppealManage1.AddSubMenu("帐户销户记录","BaseAccount/logOnUser.aspx") ;
            //}

            //if(classLibrary.ClassLib.ValidateRight("UpdateAccountQQ",this))
            //{
            //	SelfHelpAppealManage1.AddSubMenu("帐户QQ修改","BaseAccount/ChangeQQOld.aspx") ;
            //}

            //yinhuang处理加载菜单要验证权限问题
            //DKAdjust
            DKManageControl1.AddSubMenu("调整状态查询", "NewQueryInfoPages/DK_QueryAdjust.aspx");

            //DrawAndApprove
          
            AccountOperaManage1.AddSubMenu("自助商户审核", "BaseAccount/SelfQueryApprove.aspx");
            AccountOperaManage1.AddSubMenu("商户申请修改资料审核", "BaseAccount/DomainApprove.aspx");
            AccountOperaManage1.AddSubMenu("商户营改增审核", "BaseAccount/ValueAddedTaxApprove.aspx");
            AccountOperaManage1.AddSubMenu("商户营改增查询", "BaseAccount/ValueAddedTaxQuery.aspx");
            AccountOperaManage1.AddSubMenu("结算规则查询", "TradeManage/AppealSSetting.aspx");
            AccountOperaManage1.AddSubMenu("证书通知黑名单管理", "SpSettle/CertBlackList.aspx");

            //TradeLogQuery
            BankBillManage1.AddSubMenu("汇总付款数据", "BaseAccount/batPay.aspx");
            //MediumTradeManage1.AddSubMenu("中介订单查询", "TradeManage/OrderQuery.aspx");
            tradeManage1.AddSubMenu("交易记录查询", "TradeManage/TradeLogQuery.aspx");

            //FundQuery
          //  BankBillManage1.AddSubMenu("订单实时调帐", "TradeManage/RealtimeOrder.aspx");
            tradeManage1.AddSubMenu("充值记录查询", "TradeManage/FundQuery.aspx");
            tradeManage1.AddSubMenu("银行订单查询", "TradeManage/BankOrderListQuery.aspx");
            tradeManage1.AddSubMenu("转账单查询", "TradeManage/TransferQuery.aspx");
            tradeManage1.AddSubMenu("历史交易单迁移", "TradeManage/TradeMigration.aspx");
            tradeManage1.AddSubMenu("历史订单迁移", "TradeManage/OrderMigration.aspx");
            //UserBankInfoQuery
            baseAccount1.AddSubMenu("银行账号信息", "BaseAccount/UserBankInfoQuery.aspx");

            //ChangeUserInfo
            baseAccount1.AddSubMenu("个人信息", "BaseAccount/ChangeUserInfo.aspx");
            MicroPay1.AddSubMenu("子帐户查询", "BaseAccount/ChildrenQuery.aspx");
            MicroPay1.AddSubMenu("子帐户订单查询", "BaseAccount/ChildrenOrderFromQuery.aspx");
            MicroPay1.AddSubMenu("子帐户订单查询(新)", "BaseAccount/ChildrenOrderFromQueryNew.aspx");
            MicroPay1.AddSubMenu("历史订单查询", "BaseAccount/ChildrenHistoryOrderQuery.aspx");

            //UserReport
            //baseAccount1.AddSubMenu("意见投诉查询", "BaseAccount/userReport.aspx");

            //HistoryModify
         //   baseAccount1.AddSubMenu("信息修改历史", "BaseAccount/historyModify.aspx");
            accountOperate1.AddSubMenu("财付通帐号恢复", "BaseAccount/RecoverQQ.aspx");

            //FreezeList
            RiskConManage1.AddSubMenu("财付盾查询", "BaseAccount/CFDQuery.aspx");
            RiskConManage1.AddSubMenu("手机令牌", "Trademanage/MobileTokenQuery.aspx");
            RiskConManage1.AddSubMenu("冻结操作查询", "BaseAccount/FreezeList.aspx");
            
            RiskConManage1.AddSubMenu("冻结资金记录", "BaseAccount/FreezeFinQuery.aspx");
            RiskConManage1.AddSubMenu("冻结资金查询(新）", "BaseAccount/FreezeFinQuery2.aspx");
            RiskConManage1.AddSubMenu("提现冻结资金查询", "BaseAccount/CashOutFreezeQuery.aspx");

            //CFTUserAppeal
            SelfHelpAppealManage1.AddSubMenu("自助申诉查询", "BaseAccount/CFTUserAppeal.aspx");

            //CancelAccount
            accountOperate1.AddSubMenu("帐户销户记录", "BaseAccount/logOnUser.aspx");
            //accountOperate1.AddSubMenu("帐户销户记录-新", "BaseAccount/logOnUserNew.aspx");

            //UpdateAccountQQ
            accountOperate1.AddSubMenu("帐户QQ修改", "BaseAccount/ChangeQQOld.aspx");
            accountOperate1.AddSubMenu("提现单拦截", "BaseAccount/FetchListIntercept.aspx");

            ForeignCurrencyPay1.AddSubMenu("外币帐号查询", "ForeignCurrencyPay/FCXGAccountQuery.aspx");
            ForeignCurrencyPay1.AddSubMenu("绑卡查询", "ForeignCurrencyPay/FCXGBindCardQuery.aspx");
            ForeignCurrencyPay1.AddSubMenu("账户资金和流水查询", "ForeignCurrencyPay/FCXGMoneyAndFlow.aspx");
            ForeignCurrencyPay1.AddSubMenu("订单查询(新)", "ForeignCurrencyPay/FCXGOrderQuery.aspx");
            ForeignCurrencyPay1.AddSubMenu("香港钱包冻结操作查询", "BaseAccount/FCXGFreezeLog.aspx");
            ForeignCurrencyPay1.AddSubMenu("外币商户查询", "ForeignCurrencyPay/FCXGSPQuery.aspx");
            ForeignCurrencyPay1.AddSubMenu("订单查询", "ForeignCurrencyPay/FCOrderQuery.aspx");
            ForeignCurrencyPay1.AddSubMenu("退款查询", "ForeignCurrencyPay/FCRefundQuery.aspx");
            ForeignCurrencyPay1.AddSubMenu("拒付查询", "ForeignCurrencyPay/FCRefusePayQuery.aspx");
            ForeignCurrencyPay1.AddSubMenu("账户流水查询", "ForeignCurrencyPay/FCRollQuery.aspx");
            ForeignCurrencyPay1.AddSubMenu("外币用户交易查询", "ForeignCurrencyPay/FCUserTradeQuery.aspx");

            ForeignCurrencyAccount1.AddSubMenu("商户信息查询", "ForeignCurrencyPay/FCAInfoQuery.aspx");
            ForeignCurrencyAccount1.AddSubMenu("商户结算查询", "ForeignCurrencyPay/FCASettlementQuery.aspx");
            ForeignCurrencyAccount1.AddSubMenu("商户划款查询", "ForeignCurrencyPay/FCATransferQuery.aspx");
            ForeignCardPay1.AddSubMenu("订单查询", "ForeignCardPay/FCardOrderQuery.aspx");
            ForeignCardPay1.AddSubMenu("拒付查询", "ForeignCardPay/FCardRefusePayQuery.aspx");
            ForeignCardPay1.AddSubMenu("账户流水查询", "ForeignCardPay/FCardRollQuery.aspx");

            PNRQuery1.AddSubMenu("PNR签约关系查询", "BaseAccount/PNRQuery.aspx");
            PNRQuery1.AddSubMenu("PNR订单查询", "TradeManage/PNROrderQuery.aspx");
            PNRQuery1.AddSubMenu("PNR操作查询", "TradeManage/PNROperateQuery.aspx");

            
            tradeManage1.AddSubMenu("未完成交易单查询", "TradeManage/UnFinishTradeQuery.aspx");
            tradeManage1.AddSubMenu("注销前交易查询", "TradeManage/BeforeCancelTradeQuery.aspx");
            tradeManage1.AddSubMenu("中介订单查询", "TradeManage/OrderQuery.aspx");

            FastPay1.AddSubMenu("快捷额度查询", "FastPay/FastPayLimitQuery.aspx");


        }


    }
}
