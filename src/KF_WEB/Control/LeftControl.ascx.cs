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
    ///		LeftControl ��ժҪ˵����
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
                    throw new Exception("��ʱ�������µ�¼��");
                }
            }
            catch
            {
                Response.Redirect("../KF_Web/timeout.aspx");
            }


            // �ڴ˴������û������Գ�ʼ��ҳ��
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

                    if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("PayManagement", this))  //֧�������µĲ˵�������Ҫ�����Ȩ��
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
                    //				Response.Redirect("���ӳ�ʱ��");
                    string str = ex.Message;
                }
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
        ///		�����֧������ķ��� - ��Ҫʹ�ô���༭��
        ///		�޸Ĵ˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion



        private void InitAllMenu()
        {
            if (classLibrary.ClassLib.ValidateRight("InfoCenter", this))
            {
                TravelPlatform1.AddSubMenu("��Ʊ������ѯ", "TravelPlatform/AirTicketsOrderQuery.aspx");
                TravelPlatform1.AddSubMenu("�Ƶ궩����ѯ", "TravelPlatform/HotelOrderQuery.aspx");
                // ActivityCooperation1.AddSubMenu("�����û���ѯ", "BaseAccount/ManJianUserQuery.aspx");
                ActivityCooperation1.AddSubMenu("����ʹ�ò�ѯ", "BaseAccount/ManJianUsingQuery.aspx");
                ActivityCooperation1.AddSubMenu("���ĵ��̰�����", "NewQueryInfoPages/PaiPaiBMDQuery.aspx");
                SelfHelpAppealManage1.AddSubMenu("�ҷ��������", "BaseAccount/StartCheck.aspx");
                AccountLedgerManage1.AddSubMenu("���˶�����ˮ", "BaseAccount/SeparateOperation.aspx");
                //AccountLedgerManage1.AddSubMenu("���˶������","TradeManage/SeparateListQuery.aspx");
                //AccountLedgerManage1.AddSubMenu("��֧�����ѯ","TradeManage/SettleRule.aspx");
                AccountLedgerManage1.AddSubMenu("���˶�����ѯ", "TradeManage/SettleInfo.aspx");
                AccountLedgerManage1.AddSubMenu("�����˿��ѯ", "TradeManage/SettleRefund.aspx");
                AccountLedgerManage1.AddSubMenu("����ⶳ��ѯ", "TradeManage/SettleFreeze.aspx");
                AccountLedgerManage1.AddSubMenu("���˶�����ѯ", "TradeManage/AdjustList.aspx");
                AccountLedgerManage1.AddSubMenu("������˹�ϵ", "TradeManage/SettleAgent.aspx");
                AccountLedgerManage1.AddSubMenu("�����˻���ϸ", "TradeManage/SettleInfoDetail.aspx");
                AccountLedgerManage1.AddSubMenu("���������ѯ", "SpSettle/SettleReqQuery.aspx");
                AccountLedgerManage1.AddSubMenu("������ϸ��ѯ", "SpSettle/SettleReqDetail.aspx");
                AccountLedgerManage1.AddSubMenu("���ʲ�ѯ", "SpSettle/AdjustQuery.aspx");
                AccountLedgerManage1.AddSubMenu("���˹����ѯ", "SpSettle/SettleRuleQuery.aspx");
                AccountLedgerManage1.AddSubMenu("�̻�Ȩ�޲�ѯ", "SpSettle/SpControlQuery.aspx");
                AccountLedgerManage1.AddSubMenu("�����ϵ��ѯ", "SpSettle/OrderRelationQuery.aspx");
                AccountLedgerManage1.AddSubMenu("������ϸ��ѯ", "SpSettle/OrderInfoQuery.aspx");
                AccountLedgerManage1.AddSubMenu("�����˻���ѯ", "SpSettle/OrderAccountQuery.aspx");

                accountManage1.AddSubMenu("���ֹ����ѯ", "TradeManage/AppealDSettings.aspx");
               
                accountManage1.AddSubMenu("ֱ���̻���ѯ", "BaseAccount/PayBusinessQuery.aspx");
                accountManage1.AddSubMenu("�н��̻���ѯ", "BaseAccount/AgencyBusinessQuery.aspx");
                accountManage1.AddSubMenu("�����ѯ", "TradeManage/SettQuery.aspx");
                accountManage1.AddSubMenu("�����ѯ(��)", "TradeManage/SettQueryNew.aspx");
                accountManage1.AddSubMenu("��ͬ��ѯ", "TradeManage/ContractQuery.aspx");
                accountManage1.AddSubMenu("֤������ѯ", "TradeManage/MediCertManage.aspx");
                accountManage1.AddSubMenu("֤��CGI��ѯ", "TradeManage/CGIInfoQuery.aspx");
                accountManage1.AddSubMenu("֤�鵽�ڲ�ѯ", "TradeManage/MediCertExpireQuery.aspx");
                accountManage1.AddSubMenu("�Զ��۷�Э��", "TradeManage/PayLimitManage.aspx");
                accountManage1.AddSubMenu("T+0�����ѯ", "TradeManage/BatPayQuery.aspx");
                //	accountManage1.AddSubMenu("PNRǩԼ��ϵ��ѯ","BaseAccount/PNRQuery.aspx") ;
                accountManage1.AddSubMenu("ͬ����¼��ѯ", "TradeManage/SynRecordQuery.aspx");
                accountManage1.AddSubMenu("�ո��ײ�ѯ", "TradeManage/ShouFuYiQuery.aspx");
                accountManage1.AddSubMenu("��֤���˻���ѯ", "BaseAccount/DepositAccountQuery.aspx");
                accountManage1.AddSubMenu("΢��/�Ƹ�ͨ�̻���Ϣ����", "WebchatPay/WechatMerchantTrans.aspx");

                if (classLibrary.ClassLib.ValidateRight("MobileConfig", this))
                {
                    accountManage1.AddSubMenu("������Ч��", "TradeManage/MobileRechargeConfigValidDate.aspx");
                    accountManage1.AddSubMenu("��������", "TradeManage/MobileRechargeConfig.aspx");
                }
                accountManage1.AddSubMenu("�̻������嵥", "TradeManage/TradeLogList.aspx");
                accountManage1.AddSubMenu("�̻�������ѯ�Ƹ�ͨ����", "TradeManage/QuerySpOrderPage.aspx");
                accountManage1.AddSubMenu("����BD�̻���ѯ", "BaseAccount/SelfQuery.aspx");

                AccountOperaManage1.AddSubMenu("�̻������޸�", "BaseAccount/ModifyBusinessInfo.aspx");
                AccountOperaManage1.AddSubMenu("�޸���ϵ��Ϣ", "BaseAccount/ModifyContactInfo.aspx");
                AccountOperaManage1.AddSubMenu("�̻���Ϣ�޸Ĳ�ѯ", "BaseAccount/MerchantInfoModifyQuery.aspx");
                AccountOperaManage1.AddSubMenu("�̻����֤�޸�", "BaseAccount/UpdateMerchantCre.aspx");
                AccountOperaManage1.AddSubMenu("�̻���������", "TradeManage/BusinessFreeze.aspx");
                AccountOperaManage1.AddSubMenu("�̻��ر��˿�����", "TradeManage/ShutRefund.aspx");
                AccountOperaManage1.AddSubMenu("�̻���ͨ�˿�����", "TradeManage/ApplyRefund.aspx");
                AccountOperaManage1.AddSubMenu("�̻��ָ�����", "TradeManage/BusinessResume.aspx");
                AccountOperaManage1.AddSubMenu("�̻��˵�����", "RefundManage/SuspendRefundment.aspx");

                //�������θò˵������ڿ��ܻ�򿪣�����ɾ��
                //AccountOperaManage1.AddSubMenu("�̻�ע���˳�", "TradeManage/BusinessLogout.aspx");

                AccountOperaManage1.AddSubMenu("Ͷ���̻�¼��", "TradeManage/ComplainBussinessInput.aspx");
                AccountOperaManage1.AddSubMenu("�û�Ͷ��", "TradeManage/ComplainUserInput.aspx");


                SysManage1.AddSubMenu("ϵͳ�������", "SysManage/SysBulletinManage.aspx");
                SysManage1.AddSubMenu("��������ά��", "SysManage/QuestionManage.aspx");
                SysManage1.AddSubMenu("���нӿ�ά������", "SysManage/BankInterfaceManage.aspx");

                BankBillManage1.AddSubMenu("�����˵�����", "TradeManage/RefundMain.aspx");
                BankBillManage1.AddSubMenu("�˵����ܲ�ѯ", "RefundManage/RefundTotalQuery.aspx");
                BankBillManage1.AddSubMenu("����ʵʱ��ѯ", "TradeManage/RealTimeOrderQuery.aspx");
              //  BankBillManage1.AddSubMenu("�쳣���񵥹���", "RefundManage/RefundErrorMain.aspx?WorkType=task");
                BankBillManage1.AddSubMenu("�˵��쳣���ݲ�ѯ", "RefundManage/RefundErrorHandle.aspx");
                BankBillManage1.AddSubMenu("�˿�ʧ�ܲ�ѯ", "RefundManage/RefundRegistration.aspx");

                BankBillManage1.AddSubMenu("�����쳣��ѯ֪ͨ", "RefundManage/PaymentAbnormalQueryNotify.aspx");
                baseAccount1.AddSubMenu("�����˻���Ϣ", "BaseAccount/InfoCenter.aspx");              
                baseAccount1.AddSubMenu("QQ�ʺŻ���", "BaseAccount/QQReclaim.aspx");
                accountOperate1.AddSubMenu("�˻������޸�", "BaseAccount/changeUserName_2.aspx");
                baseAccount1.AddSubMenu("�û��ܿ��ʽ��ѯ", "TradeManage/QueryUserControledFinPage.aspx");
                baseAccount1.AddSubMenu("�û��ܿ��ʽ��ѯ-��", "TradeManage/UserControledFundPage.aspx");
              //  baseAccount1.AddSubMenu("�ֻ��󶨲�ѯ", "TradeManage/MobileBindQuery.aspx");
                baseAccount1.AddSubMenu("�ֻ��󶨲�ѯ", "TradeManage/MobileBindingQuery.aspx");   
                accountOperate1.AddSubMenu("֤����������", "BaseAccount/ClearCreidNew.aspx");
                baseAccount1.AddSubMenu("��Ѷ���ò�ѯ", "BaseAccount/TencentCreditQuery.aspx");

                FastPay1.AddSubMenu("һ��ͨҵ��", "BaseAccount/BankCardUnbind.aspx");
                FastPay1.AddSubMenu("һ��ͨҵ��(��)", "BaseAccount/BankCardUnbindNew.aspx");
               // FastPay1.AddSubMenu("���п���ѯ", "TradeManage/BankCardQuery.aspx");
                FastPay1.AddSubMenu("���п���ѯ", "TradeManage/BankCardQueryNew.aspx");
                FastPay1.AddSubMenu("������Ƨ��", "BaseAccount/RareNameQuery.aspx");
                FastPay1.AddSubMenu("����Ϣ��ѯ", "BaseAccount/CardInfoQuery.aspx");

                FundAccountManage1.AddSubMenu("ǩԼ��Լ��ѯ", "NewQueryInfoPages/QueryInverestorSignPage.aspx");
                FundAccountManage1.AddSubMenu("�����ײ�ѯ", "NewQueryInfoPages/QueryFundInfoPage.aspx");
                FundAccountManage1.AddSubMenu("������ˮ��ѯ", "NewQueryInfoPages/QueryChargeInfoPage.aspx");
                FundAccountManage1.AddSubMenu("�����˻���ѯ", "NewQueryInfoPages/GetUserFundAccountInfoPage.aspx");
                //FundAccountManage1.AddSubMenu("���ͨ��ѯ", "NewQueryInfoPages/GetFundRatePage.aspx");

                LifeFeeDetailManage1.AddSubMenu("����ɷѲ�ѯ", "TradeManage/FeeQuery.aspx");
                //LifeFeeDetailManage1.AddSubMenu("�ʴ�����ѯ", "RemitCheck/RemitQuery.aspx");
                LifeFeeDetailManage1.AddSubMenu("�ʴ�����ѯ", "RemitCheck/RemitQueryNew.aspx");
                LifeFeeDetailManage1.AddSubMenu("�ֻ���ֵ����ѯ", "TradeManage/FundCardQuery_Detail.aspx");
                LifeFeeDetailManage1.AddSubMenu("�ֻ���ֵ����ѯ-��", "TradeManage/FundCardQuery_DetailNew.aspx");
                //LifeFeeDetailManage1.AddSubMenu("���ÿ�����", "TradeManage/CreditQuery.aspx");
                LifeFeeDetailManage1.AddSubMenu("���ÿ�����", "TradeManage/CreditQueryNew.aspx");
                LifeFeeDetailManage1.AddSubMenu("���ѷ�����ѯ", "TradeManage/PhoneBillQuery.aspx");
                LifeFeeDetailManage1.AddSubMenu("�Զ���ֵ", "TradeManage/AutomaticRechargeQuery.aspx");
                LifeFeeDetailManage1.AddSubMenu("��������ֵ��ѯ", "TradeManage/BusCardPrepaidQuery.aspx");

                RiskConManage1.AddSubMenu("���ε�¼���볷��", "Trademanage/SuspendSecondPasseword.aspx");
                RiskConManage1.AddSubMenu("�ʽ���ˮ��ѯ", "BaseAccount/BankrollHistoryLog.aspx");
                RiskConManage1.AddSubMenu("��ؽⶳ���", "FreezeManage/FreezeQuery.aspx");
                RiskConManage1.AddSubMenu("�������ߴ���", "FreezeManage/FreezeNewQuery.aspx");
                RiskConManage1.AddSubMenu("����ͳ�����", "FreezeManage/FreezeCount.aspx");
                RiskConManage1.AddSubMenu("�ֻ���������", "BaseAccount/ClearMobileNumber.aspx");
                SelfHelpAppealManage1.AddSubMenu("�ͷ�ͳ�Ʋ�ѯ", "BaseAccount/KFTotalQuery.aspx");

                //tradeManage1.AddSubMenu("���ּ�¼��ѯ", "TradeManage/PickQuery.aspx");
                tradeManage1.AddSubMenu("���ּ�¼��ѯ(��)", "TradeManage/PickQueryNew.aspx");
                tradeManage1.AddSubMenu("�˿��ѯ", "TradeManage/B2CReturnQuery.aspx");
                

                DFManageControl1.AddSubMenu("�������ʲ�ѯ", "NewQueryInfoPages/DFDetailQuery.aspx");
                DFManageControl1.AddSubMenu("����������ѯ", "NewQueryInfoPages/DFBatchQuery.aspx");
                DKManageControl1.AddSubMenu("���۵��ʲ�ѯ", "NewQueryInfoPages/QueryDKInfoPage.aspx");
                DKManageControl1.AddSubMenu("����������ѯ", "NewQueryInfoPages/QueryDKListInfoPage.aspx");
                DFManageControl1.AddSubMenu("�Ƹ�ͨת��", "NewQueryInfoPages/CFTTransferQuery.aspx");


                DKManageControl1.AddSubMenu("�������β�ѯ", "NewQueryInfoPages/QueryBankListInfoPage.aspx");
                DKManageControl1.AddSubMenu("�����޶��ѯ", "NewQueryInfoPages/QueryDKLimitPage.aspx");
                DKManageControl1.AddSubMenu("�̻����Բ�ѯ", "NewQueryInfoPages/QueryDKServicePage.aspx");
                DKManageControl1.AddSubMenu("Э����ϸ��ѯ", "NewQueryInfoPages/QueryDKcontractPage.aspx");
                DKManageControl1.AddSubMenu("Э�����β�ѯ", "NewQueryInfoPages/QueryDKcontractListPage.aspx");

                DKManageControl1.AddSubMenu("Э��⵼��", "NewQueryInfoPages/ProtocolLibImport.aspx");
                DKManageControl1.AddSubMenu("Э����ѯ", "NewQueryInfoPages/QueryProtocolLib.aspx");

                tradeManage1.AddSubMenu("�û��ֻ���ֵ��¼��ѯ", "TradeManage/MobileRechargeQuery.aspx");
                tradeManage1.AddSubMenu("���ж����Ų�ѯ", "TradeManage/BankBillNoQuery.aspx");

				VIPAccountManage.AddSubMenu("�Ƹ�ֵ��ˮ","VIPAccount/PropertyTurnover.aspx");
                VIPAccountManage.AddSubMenu("���п���ѯ", "VIPAccount/QueryBankCard.aspx");
              //  VIPAccountManage.AddSubMenu("�Ƹ�ֵ��ˮ", "VIPAccount/PropertyTurnover.aspx");
                VIPAccountManage.AddSubMenu("ͼ�����", "VIPAccount/IconManagement.aspx");

                OverseasPay1.AddSubMenu("�⿨���ײ�ѯ", "NewQueryInfoPages/QueryForeignCard.aspx");
                OverseasPay1.AddSubMenu("��ͨ�˺���Ϣ��ѯ", "NewQueryInfoPages/QueryYTInfo.aspx");
                OverseasPay1.AddSubMenu("��ͨ�˺Ŷ����ѯ", "NewQueryInfoPages/QueryYTFreeze.aspx");
                OverseasPay1.AddSubMenu("��ͨ�˺Ž��ײ�ѯ", "NewQueryInfoPages/QueryYTTrade.aspx");
                OverseasPay1.AddSubMenu("�����˺���Ϣ��ѯ", "NewQueryInfoPages/QueryZHInfo.aspx");
                OverseasPay1.AddSubMenu("�����˺Ŷ����ѯ", "NewQueryInfoPages/QueryZHFreeze.aspx");
                OverseasPay1.AddSubMenu("�����˺Ž��ײ�ѯ", "NewQueryInfoPages/QueryZHTrade.aspx");
                OverseasPay1.AddSubMenu("�����ʲ�ѯ", "NewQueryInfoPages/QueryForeignExchangeRate.aspx");
                OverseasPay1.AddSubMenu("����΢��С��֧����ѯ", "NewQueryInfoPages/QueryWeiXinMircoPay.aspx");

                InternetBank.AddSubMenu("��ˮ��ѯ", "InternetBank/TurnoverQuery.aspx");
                InternetBank.AddSubMenu("�˵���ѯ", "InternetBank/BillQuery.aspx");
                InternetBank.AddSubMenu("��ʷ�˵�", "InternetBank/BillHistoryQuery.aspx");
                InternetBank.AddSubMenu("��Ա�Żݶ��", "InternetBank/MermberDiscount.aspx");
                InternetBank.AddSubMenu("�˿�Ǽ�", "InternetBank/RefundQuery.aspx");
                InternetBank.AddSubMenu("�˿��̻�¼��", "InternetBank/RefundMerchant.aspx");
                InternetBank.AddSubMenu("�Զ����Ѳ�ѯ", "InternetBank/AtuoRenewQuery.aspx");
                //��Q֧��
                HandQBusiness.AddSubMenu("��Q�����ѯ", "HandQBusiness/FindHandQRedPacket.aspx");
                HandQBusiness.AddSubMenu("��Q�����ѯ", "HandQBusiness/RefundHandQQuery.aspx");

                //�����
                ActivityCooperation1.AddSubMenu("��������", "NewQueryInfoPages/QueryDiscountCode.aspx");
                ActivityCooperation1.AddSubMenu("�û��μӵĻ", "NewQueryInfoPages/QueryUserJoinActivity.aspx");
                ActivityCooperation1.AddSubMenu("���־��ѯ", "NewQueryInfoPages/QueryActivityLogs.aspx");
                ActivityCooperation1.AddSubMenu("΢�Ż��ѯ", "NewQueryInfoPages/QueryWebchatPayActivity.aspx");
                ActivityCooperation1.AddSubMenu("��ˢ�����ѯ", "Activity/QueryLeShuaKaActivity.aspx");
                ActivityCooperation1.AddSubMenu("���ͨ���ѯ", "Activity/QueryLCTActivity.aspx");
                ActivityCooperation1.AddSubMenu("�»���", "Activity/LctActivityAdd.aspx");

                //�Ƹ�ȯ����
                TokenCoin1.AddSubMenu("�Ƹ�ȯ��ѯ", "TokenCoin/GwqQuery.aspx");
                TokenCoin1.AddSubMenu("�Ƹ�ȯ��ѯ��ϸ", "TokenCoin/GwqShow.aspx");

                //����֧��
                CreditPayControl1.AddSubMenu("������Ϣ", "CreditPay/QueryCreditUserInfo.aspx");
                CreditPayControl1.AddSubMenu("�˵���ѯ", "CreditPay/QueryCreditBillList.aspx");
                CreditPayControl1.AddSubMenu("Ƿ���ѯ", "CreditPay/QueryCreditDebt.aspx");
                CreditPayControl1.AddSubMenu("�����ѯ", "CreditPay/QueryRefund.aspx");
                CreditPayControl1.AddSubMenu("�ʽ���ˮ��ѯ", "CreditPay/QueryCapitalRoll.aspx");

                //΢��֧��
                WebchatPayControl1.AddSubMenu("΢��֧���ʺ�", "WebchatPay/WechatInfoQuery.aspx");
                WebchatPayControl1.AddSubMenu("AA�տ��ʺ�", "WebchatPay/WechatAACollection.aspx");
                WebchatPayControl1.AddSubMenu("���ͨ��ѯ", "NewQueryInfoPages/GetFundRatePage.aspx");
                WebchatPayControl1.AddSubMenu("���ͨ��ȫ��", "WebchatPay/SafeCardManage.aspx");
                WebchatPayControl1.AddSubMenu("΢�ź����ѯ", "WebchatPay/WechatRedPacket.aspx");
                WebchatPayControl1.AddSubMenu("С��ˢ��", "WebchatPay/SmallCreditCardQuery.aspx");
                WebchatPayControl1.AddSubMenu("΢�����ÿ�����", "WebchatPay/CreditCardRefundQuery.aspx");
                WebchatPayControl1.AddSubMenu("���ͨ��ֵȯ", "WebchatPay/AddedValueTicketQuery.aspx");
                WebchatPayControl1.AddSubMenu("��Լ����ѯ", "WebchatPay/QueryContractMachine.aspx");
                WebchatPayControl1.AddSubMenu("ʵʱ�����ѯ", "WebchatPay/QueryRealtimeRepayment.aspx");
            }

            if (classLibrary.ClassLib.ValidateRight("DKAdjust", this))
            {
                DKManageControl1.AddSubMenu("�����ļ�����", "NewQueryInfoPages/DKAdjust.aspx?querytype=fileselect");
                //DKManageControl1.AddSubMenu("����״̬��ѯ", "NewQueryInfoPages/DK_QueryAdjust.aspx");
            }
            /*
			if(classLibrary.ClassLib.ValidateRight("DrawAndApprove",this))
			{
				AccountOperaManage1.AddSubMenu("�����̻��쵥","BaseAccount/SelfQuery.aspx") ;
				AccountOperaManage1.AddSubMenu("�����̻����","BaseAccount/SelfQueryApprove.aspx") ;
				AccountOperaManage1.AddSubMenu("�̻������޸��������","BaseAccount/DomainApprove.aspx") ;
				AccountOperaManage1.AddSubMenu("�̻�Ӫ�������","BaseAccount/ValueAddedTaxApprove.aspx") ;
				AccountOperaManage1.AddSubMenu("�̻�Ӫ������ѯ","BaseAccount/ValueAddedTaxQuery.aspx") ;
			}

			if(classLibrary.ClassLib.ValidateRight("TradeLogQuery",this))
			{
				BankBillManage1.AddSubMenu("���ܸ�������","BaseAccount/batPay.aspx") ;

				MediumTradeManage1.AddSubMenu("�н鶩����ѯ","TradeManage/OrderQuery.aspx") ;

				tradeManage1.AddSubMenu("���׼�¼��ѯ","TradeManage/TradeLogQuery.aspx") ;
			}

			if(classLibrary.ClassLib.ValidateRight("FundQuery",this))
			{
				BankBillManage1.AddSubMenu("����ʵʱ����","TradeManage/RealtimeOrder.aspx") ;

				tradeManage1.AddSubMenu("��ֵ��¼��ѯ","TradeManage/FundQuery.aspx") ;
			}

			if(classLibrary.ClassLib.ValidateRight("UserBankInfoQuery",this))
			{
				baseAccount1.AddSubMenu("�����˺���Ϣ","BaseAccount/UserBankInfoQuery.aspx") ;
			}

			if(classLibrary.ClassLib.ValidateRight("ChangeUserInfo",this))
			{
				baseAccount1.AddSubMenu("������Ϣ","BaseAccount/ChangeUserInfo.aspx") ;
				MicroPay1.AddSubMenu("���ʻ���ѯ","BaseAccount/ChildrenQuery.aspx");
				MicroPay1.AddSubMenu("���ʻ�������ѯ","BaseAccount/ChildrenOrderFromQuery.aspx");
				MicroPay1.AddSubMenu("���ʻ�������ѯ(��)","BaseAccount/ChildrenOrderFromQueryNew.aspx");
                MicroPay1.AddSubMenu("��ʷ������ѯ", "BaseAccount/ChildrenHistoryOrderQuery.aspx");
			}

			if(classLibrary.ClassLib.ValidateRight("UserReport",this))
			{
				baseAccount1.AddSubMenu("���Ͷ�߲�ѯ","BaseAccount/userReport.aspx") ;
			}

			if(classLibrary.ClassLib.ValidateRight("HistoryModify",this))
			{
				baseAccount1.AddSubMenu("��Ϣ�޸���ʷ","BaseAccount/historyModify.aspx") ;
			}
            */
            if (classLibrary.ClassLib.ValidateRight("InfoCenter", this) || TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("DeleteCrt", this))
            {
                RiskConManage1.AddSubMenu("����֤�����", "Trademanage/CrtQuery.aspx");
            }

            //if(TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("FreezeList",this))
            //{
            //	RiskConManage1.AddSubMenu("�Ƹ��ܲ�ѯ","BaseAccount/CFDQuery.aspx") ;
            //    RiskConManage1.AddSubMenu("�ֻ�����", "Trademanage/MobileTokenQuery.aspx");
            //	RiskConManage1.AddSubMenu("���������ѯ","BaseAccount/FreezeList.aspx") ;
            //	RiskConManage1.AddSubMenu("�����ʽ��¼","BaseAccount/FreezeFinQuery.aspx");

            //}

            //if(classLibrary.ClassLib.ValidateRight("CFTUserAppeal",this))
            //{
            //	SelfHelpAppealManage1.AddSubMenu("�������߲�ѯ","BaseAccount/CFTUserAppeal.aspx") ;
            //}


            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("CFTUserPick", this))
            {
                SelfHelpAppealManage1.AddSubMenu("���ߴ���(��)", "BaseAccount/UserAppeal.aspx");
            }

            //if(classLibrary.ClassLib.ValidateRight("CancelAccount",this))
            //{
            //	SelfHelpAppealManage1.AddSubMenu("�ʻ�������¼","BaseAccount/logOnUser.aspx") ;
            //}

            //if(classLibrary.ClassLib.ValidateRight("UpdateAccountQQ",this))
            //{
            //	SelfHelpAppealManage1.AddSubMenu("�ʻ�QQ�޸�","BaseAccount/ChangeQQOld.aspx") ;
            //}

            //yinhuang������ز˵�Ҫ��֤Ȩ������
            //DKAdjust
            DKManageControl1.AddSubMenu("����״̬��ѯ", "NewQueryInfoPages/DK_QueryAdjust.aspx");

            //DrawAndApprove
          
            AccountOperaManage1.AddSubMenu("�����̻����", "BaseAccount/SelfQueryApprove.aspx");
            AccountOperaManage1.AddSubMenu("�̻������޸��������", "BaseAccount/DomainApprove.aspx");
            AccountOperaManage1.AddSubMenu("�̻�Ӫ�������", "BaseAccount/ValueAddedTaxApprove.aspx");
            AccountOperaManage1.AddSubMenu("�̻�Ӫ������ѯ", "BaseAccount/ValueAddedTaxQuery.aspx");
            AccountOperaManage1.AddSubMenu("��������ѯ", "TradeManage/AppealSSetting.aspx");
            AccountOperaManage1.AddSubMenu("֤��֪ͨ����������", "SpSettle/CertBlackList.aspx");

            //TradeLogQuery
            BankBillManage1.AddSubMenu("���ܸ�������", "BaseAccount/batPay.aspx");
            //MediumTradeManage1.AddSubMenu("�н鶩����ѯ", "TradeManage/OrderQuery.aspx");
            tradeManage1.AddSubMenu("���׼�¼��ѯ", "TradeManage/TradeLogQuery.aspx");

            //FundQuery
          //  BankBillManage1.AddSubMenu("����ʵʱ����", "TradeManage/RealtimeOrder.aspx");
            tradeManage1.AddSubMenu("��ֵ��¼��ѯ", "TradeManage/FundQuery.aspx");
            tradeManage1.AddSubMenu("���ж�����ѯ", "TradeManage/BankOrderListQuery.aspx");
            tradeManage1.AddSubMenu("ת�˵���ѯ", "TradeManage/TransferQuery.aspx");
            tradeManage1.AddSubMenu("��ʷ���׵�Ǩ��", "TradeManage/TradeMigration.aspx");
            tradeManage1.AddSubMenu("��ʷ����Ǩ��", "TradeManage/OrderMigration.aspx");
            //UserBankInfoQuery
            baseAccount1.AddSubMenu("�����˺���Ϣ", "BaseAccount/UserBankInfoQuery.aspx");

            //ChangeUserInfo
            baseAccount1.AddSubMenu("������Ϣ", "BaseAccount/ChangeUserInfo.aspx");
            MicroPay1.AddSubMenu("���ʻ���ѯ", "BaseAccount/ChildrenQuery.aspx");
            MicroPay1.AddSubMenu("���ʻ�������ѯ", "BaseAccount/ChildrenOrderFromQuery.aspx");
            MicroPay1.AddSubMenu("���ʻ�������ѯ(��)", "BaseAccount/ChildrenOrderFromQueryNew.aspx");
            MicroPay1.AddSubMenu("��ʷ������ѯ", "BaseAccount/ChildrenHistoryOrderQuery.aspx");

            //UserReport
            //baseAccount1.AddSubMenu("���Ͷ�߲�ѯ", "BaseAccount/userReport.aspx");

            //HistoryModify
         //   baseAccount1.AddSubMenu("��Ϣ�޸���ʷ", "BaseAccount/historyModify.aspx");
            accountOperate1.AddSubMenu("�Ƹ�ͨ�ʺŻָ�", "BaseAccount/RecoverQQ.aspx");

            //FreezeList
            RiskConManage1.AddSubMenu("�Ƹ��ܲ�ѯ", "BaseAccount/CFDQuery.aspx");
            RiskConManage1.AddSubMenu("�ֻ�����", "Trademanage/MobileTokenQuery.aspx");
            RiskConManage1.AddSubMenu("���������ѯ", "BaseAccount/FreezeList.aspx");
            
            RiskConManage1.AddSubMenu("�����ʽ��¼", "BaseAccount/FreezeFinQuery.aspx");
            RiskConManage1.AddSubMenu("�����ʽ��ѯ(�£�", "BaseAccount/FreezeFinQuery2.aspx");
            RiskConManage1.AddSubMenu("���ֶ����ʽ��ѯ", "BaseAccount/CashOutFreezeQuery.aspx");

            //CFTUserAppeal
            SelfHelpAppealManage1.AddSubMenu("�������߲�ѯ", "BaseAccount/CFTUserAppeal.aspx");

            //CancelAccount
            accountOperate1.AddSubMenu("�ʻ�������¼", "BaseAccount/logOnUser.aspx");
            //accountOperate1.AddSubMenu("�ʻ�������¼-��", "BaseAccount/logOnUserNew.aspx");

            //UpdateAccountQQ
            accountOperate1.AddSubMenu("�ʻ�QQ�޸�", "BaseAccount/ChangeQQOld.aspx");
            accountOperate1.AddSubMenu("���ֵ�����", "BaseAccount/FetchListIntercept.aspx");

            ForeignCurrencyPay1.AddSubMenu("����ʺŲ�ѯ", "ForeignCurrencyPay/FCXGAccountQuery.aspx");
            ForeignCurrencyPay1.AddSubMenu("�󿨲�ѯ", "ForeignCurrencyPay/FCXGBindCardQuery.aspx");
            ForeignCurrencyPay1.AddSubMenu("�˻��ʽ����ˮ��ѯ", "ForeignCurrencyPay/FCXGMoneyAndFlow.aspx");
            ForeignCurrencyPay1.AddSubMenu("������ѯ(��)", "ForeignCurrencyPay/FCXGOrderQuery.aspx");
            ForeignCurrencyPay1.AddSubMenu("���Ǯ�����������ѯ", "BaseAccount/FCXGFreezeLog.aspx");
            ForeignCurrencyPay1.AddSubMenu("����̻���ѯ", "ForeignCurrencyPay/FCXGSPQuery.aspx");
            ForeignCurrencyPay1.AddSubMenu("������ѯ", "ForeignCurrencyPay/FCOrderQuery.aspx");
            ForeignCurrencyPay1.AddSubMenu("�˿��ѯ", "ForeignCurrencyPay/FCRefundQuery.aspx");
            ForeignCurrencyPay1.AddSubMenu("�ܸ���ѯ", "ForeignCurrencyPay/FCRefusePayQuery.aspx");
            ForeignCurrencyPay1.AddSubMenu("�˻���ˮ��ѯ", "ForeignCurrencyPay/FCRollQuery.aspx");
            ForeignCurrencyPay1.AddSubMenu("����û����ײ�ѯ", "ForeignCurrencyPay/FCUserTradeQuery.aspx");

            ForeignCurrencyAccount1.AddSubMenu("�̻���Ϣ��ѯ", "ForeignCurrencyPay/FCAInfoQuery.aspx");
            ForeignCurrencyAccount1.AddSubMenu("�̻������ѯ", "ForeignCurrencyPay/FCASettlementQuery.aspx");
            ForeignCurrencyAccount1.AddSubMenu("�̻������ѯ", "ForeignCurrencyPay/FCATransferQuery.aspx");
            ForeignCardPay1.AddSubMenu("������ѯ", "ForeignCardPay/FCardOrderQuery.aspx");
            ForeignCardPay1.AddSubMenu("�ܸ���ѯ", "ForeignCardPay/FCardRefusePayQuery.aspx");
            ForeignCardPay1.AddSubMenu("�˻���ˮ��ѯ", "ForeignCardPay/FCardRollQuery.aspx");

            PNRQuery1.AddSubMenu("PNRǩԼ��ϵ��ѯ", "BaseAccount/PNRQuery.aspx");
            PNRQuery1.AddSubMenu("PNR������ѯ", "TradeManage/PNROrderQuery.aspx");
            PNRQuery1.AddSubMenu("PNR������ѯ", "TradeManage/PNROperateQuery.aspx");

            
            tradeManage1.AddSubMenu("δ��ɽ��׵���ѯ", "TradeManage/UnFinishTradeQuery.aspx");
            tradeManage1.AddSubMenu("ע��ǰ���ײ�ѯ", "TradeManage/BeforeCancelTradeQuery.aspx");
            tradeManage1.AddSubMenu("�н鶩����ѯ", "TradeManage/OrderQuery.aspx");

            FastPay1.AddSubMenu("��ݶ�Ȳ�ѯ", "FastPay/FastPayLimitQuery.aspx");


        }


    }
}
