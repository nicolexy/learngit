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
        //protected FastPayControl FastPay1;
        protected FundAccountManageControl FundAccountManage1;
        protected LifeFeeDetailControl LifeFeeDetailManage1;
        protected MicroPay MicroPay1;
        protected NameAuthenedControl NameAuthened1;
        protected SelfHelpAppealManageControl SelfHelpAppealManage1;
        //protected VIPAccountManageControl VIPAccountManage;
        protected FreezeManageControl FreezeManage1;
        protected SpecialManageControl SpecialManageControl1;
        protected DKManageControl DKManageControl1;
        protected DFManageControl DFManageControl1;
        //protected ActivityCooperation ActivityCooperation1;
        //protected TokenCoin TokenCoin1;
        protected SysManage SysManage1;
        protected CreditPayControl CreditPayControl1;
        protected WebchatPayControl WebchatPayControl1;
        protected FundControl FundControl1;

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
            #region ������Ϣ����
            if (classLibrary.ClassLib.ValidateRight("InfoCenter", this))
            {
                baseAccount1.Visible = true;
                NameAuthened1.Visible = true;
                accountOperate1.Visible = true;
                SelfHelpAppealManage1.Visible = true;
                RiskConManage1.Visible = true;
                FundAccountManage1.Visible = true;
                WebchatPayControl1.Visible = true;

                baseAccount1.AddSubMenu("�����˻���Ϣ", "BaseAccount/InfoCenter.aspx");
                baseAccount1.AddSubMenu("QQ�ʺŻ���", "BaseAccount/QQReclaim.aspx");
                baseAccount1.AddSubMenu("�ܿ��ʽ��ѯ", "TradeManage/UserControledFundPage.aspx");
                baseAccount1.AddSubMenu("�ֻ��󶨲�ѯ", "TradeManage/MobileBindingQuery.aspx");
                baseAccount1.AddSubMenu("��Ѷ���ò�ѯ", "BaseAccount/TencentCreditQuery.aspx");
                baseAccount1.AddSubMenu("�����˺���Ϣ", "BaseAccount/UserBankInfoQuery.aspx");
                baseAccount1.AddSubMenu("������Ϣ", "BaseAccount/ChangeUserInfo.aspx");

                accountOperate1.AddSubMenu("�˻������޸�", "BaseAccount/changeUserName_2.aspx");
                accountOperate1.AddSubMenu("֤����������", "BaseAccount/ClearCreidNew.aspx");
                accountOperate1.AddSubMenu("�Ƹ�ͨ�ʺŻָ�", "BaseAccount/RecoverQQ.aspx");
                accountOperate1.AddSubMenu("�ʻ�������¼", "BaseAccount/logOnUser.aspx");
                accountOperate1.AddSubMenu("����ע��", "BaseAccount/logOnUserBatch.aspx");
                accountOperate1.AddSubMenu("�ʻ�QQ�޸�", "BaseAccount/ChangeQQOld.aspx");
                accountOperate1.AddSubMenu("���ֵ�����", "BaseAccount/FetchListIntercept.aspx");
                accountOperate1.AddSubMenu("΢��֧���˻�ע��", "BaseAccount/logOnWxAccount.aspx");

                SelfHelpAppealManage1.AddSubMenu("�ҷ��������", "BaseAccount/StartCheck.aspx");
                SelfHelpAppealManage1.AddSubMenu("�ͷ�ͳ�Ʋ�ѯ", "BaseAccount/KFTotalQuery.aspx");
                if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("CFTUserPick", this))
                {
                    SelfHelpAppealManage1.AddSubMenu("���ߴ���(��)", "BaseAccount/UserAppeal.aspx");
                }
                SelfHelpAppealManage1.AddSubMenu("�������߲�ѯ", "BaseAccount/CFTUserAppeal.aspx");

                RiskConManage1.AddSubMenu("���ε�¼���볷��", "Trademanage/SuspendSecondPasseword.aspx");
                RiskConManage1.AddSubMenu("�ʽ���ˮ��ѯ", "BaseAccount/BankrollHistoryLog.aspx");
                RiskConManage1.AddSubMenu("��ؽⶳ���", "FreezeManage/FreezeQuery.aspx");
                RiskConManage1.AddSubMenu("�������ߴ���", "FreezeManage/FreezeNewQuery.aspx");
                RiskConManage1.AddSubMenu("����ͳ�����", "FreezeManage/FreezeCount.aspx");
                RiskConManage1.AddSubMenu("�ֻ���������", "BaseAccount/ClearMobileNumber.aspx");
                if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("DeleteCrt", this))
                {
                    RiskConManage1.AddSubMenu("����֤�����", "Trademanage/CrtQuery.aspx");
                }
                //FreezeList
                RiskConManage1.AddSubMenu("�Ƹ��ܲ�ѯ", "BaseAccount/CFDQuery.aspx");
                RiskConManage1.AddSubMenu("�ֻ�����", "Trademanage/MobileTokenQuery.aspx");
                RiskConManage1.AddSubMenu("���������ѯ", "BaseAccount/FreezeList.aspx");
                RiskConManage1.AddSubMenu("�����ʽ��¼", "BaseAccount/FreezeFinQuery.aspx");
                RiskConManage1.AddSubMenu("�����ʽ��ѯ(�£�", "BaseAccount/FreezeFinQuery2.aspx");
                if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("IceOutPPSecurityMoney", this))
                {
                    RiskConManage1.AddSubMenu("���ı�֤��ⶳ", "Trademanage/IceOutPPSecurityMoney.aspx");
                }
                // RiskConManage1.AddSubMenu("���ֶ����ʽ��ѯ", "BaseAccount/CashOutFreezeQuery.aspx");   �ù������ߣ�2016-01-05 darrenran


                FundAccountManage1.AddSubMenu("ǩԼ��Լ��ѯ", "NewQueryInfoPages/QueryInverestorSignPage.aspx");
                FundAccountManage1.AddSubMenu("�����ײ�ѯ", "NewQueryInfoPages/QueryFundInfoPage.aspx");
                FundAccountManage1.AddSubMenu("������ˮ��ѯ", "NewQueryInfoPages/QueryChargeInfoPage.aspx");
                FundAccountManage1.AddSubMenu("�����˻���ѯ", "NewQueryInfoPages/GetUserFundAccountInfoPage.aspx");
                //FundAccountManage1.AddSubMenu("���ͨ��ѯ", "NewQueryInfoPages/GetFundRatePage.aspx");

                //΢��֧��
                WebchatPayControl1.AddSubMenu("΢��֧���ʺ�", "WebchatPay/WechatInfoQuery.aspx");
                WebchatPayControl1.AddSubMenu("С��ˢ��", "WebchatPay/SmallCreditCardQuery.aspx");
                WebchatPayControl1.AddSubMenu("�羳���", "WebchatPay/CrossBorderRemittances.aspx");

            }
            #endregion

            #region ֧������
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("PayManagement", this))
            {
                CreditPayControl1.Visible = true;
                OverseasPay1.Visible = true;
                MicroPay1.Visible = true;
                ForeignCurrencyPay1.Visible = true;
                ForeignCardPay1.Visible = true;
                HKWalletPay.Visible = true;
                //����֧��
                CreditPayControl1.AddSubMenu("������Ϣ", "CreditPay/QueryCreditUserInfo.aspx");
                CreditPayControl1.AddSubMenu("�˵���ѯ", "CreditPay/QueryCreditBillList.aspx");
                CreditPayControl1.AddSubMenu("Ƿ���ѯ", "CreditPay/QueryCreditDebt.aspx");
                CreditPayControl1.AddSubMenu("�����ѯ", "CreditPay/QueryRefund.aspx");
                CreditPayControl1.AddSubMenu("�ʽ���ˮ��ѯ", "CreditPay/QueryCapitalRoll.aspx");

                OverseasPay1.AddSubMenu("�⿨���ײ�ѯ", "NewQueryInfoPages/QueryForeignCard.aspx");
                OverseasPay1.AddSubMenu("��ͨ�˺���Ϣ��ѯ", "NewQueryInfoPages/QueryYTInfo.aspx");
                OverseasPay1.AddSubMenu("��ͨ�˺Ŷ����ѯ", "NewQueryInfoPages/QueryYTFreeze.aspx");
                OverseasPay1.AddSubMenu("��ͨ�˺Ž��ײ�ѯ", "NewQueryInfoPages/QueryYTTrade.aspx");
                OverseasPay1.AddSubMenu("�����˺���Ϣ��ѯ", "NewQueryInfoPages/QueryZHInfo.aspx");
                OverseasPay1.AddSubMenu("�����˺Ŷ����ѯ", "NewQueryInfoPages/QueryZHFreeze.aspx");
                OverseasPay1.AddSubMenu("�����˺Ž��ײ�ѯ", "NewQueryInfoPages/QueryZHTrade.aspx");
                OverseasPay1.AddSubMenu("�����ʲ�ѯ", "NewQueryInfoPages/QueryForeignExchangeRate.aspx");
                OverseasPay1.AddSubMenu("����΢��С��֧����ѯ", "NewQueryInfoPages/QueryWeiXinMircoPay.aspx");

                MicroPay1.AddSubMenu("���ʻ���ѯ", "BaseAccount/ChildrenQuery.aspx");
                MicroPay1.AddSubMenu("���ʻ�������ѯ", "BaseAccount/ChildrenOrderFromQuery.aspx");
                MicroPay1.AddSubMenu("���ʻ�������ѯ(��)", "BaseAccount/ChildrenOrderFromQueryNew.aspx");
                MicroPay1.AddSubMenu("��ʷ������ѯ", "BaseAccount/ChildrenHistoryOrderQuery.aspx");


                ForeignCurrencyPay1.AddSubMenu("������ѯ", "ForeignCurrencyPay/FCOrderQuery.aspx");
                ForeignCurrencyPay1.AddSubMenu("�˿��ѯ", "ForeignCurrencyPay/FCRefundQuery.aspx");
                ForeignCurrencyPay1.AddSubMenu("�ܸ���ѯ", "ForeignCurrencyPay/FCRefusePayQuery.aspx");
                ForeignCurrencyPay1.AddSubMenu("�˻���ˮ��ѯ", "ForeignCurrencyPay/FCRollQuery.aspx");
                ForeignCurrencyPay1.AddSubMenu("����û����ײ�ѯ", "ForeignCurrencyPay/FCUserTradeQuery.aspx");

                ForeignCardPay1.AddSubMenu("������ѯ", "ForeignCardPay/FCardOrderQuery.aspx");
                ForeignCardPay1.AddSubMenu("�ܸ���ѯ", "ForeignCardPay/FCardRefusePayQuery.aspx");
                ForeignCardPay1.AddSubMenu("�˻���ˮ��ѯ", "ForeignCardPay/FCardRollQuery.aspx");

                //HKǮ��֧��
                HKWalletPay.AddSubMenu("�ʺŲ�ѯ", "ForeignCurrencyPay/FCXGAccountQuery.aspx");
                HKWalletPay.AddSubMenu("�󿨲�ѯ", "ForeignCurrencyPay/FCXGBindCardQuery.aspx");
                HKWalletPay.AddSubMenu("�˻��ʽ����ˮ��ѯ", "ForeignCurrencyPay/FCXGMoneyAndFlow.aspx");
                HKWalletPay.AddSubMenu("������ѯ", "ForeignCurrencyPay/FCXGOrderQuery.aspx");
                HKWalletPay.AddSubMenu("����ⶳ��ѯ", "BaseAccount/FCXGFreezeLog.aspx");
                HKWalletPay.AddSubMenu("�̻���ѯ", "ForeignCurrencyPay/FCXGSPQuery.aspx");
                HKWalletPay.AddSubMenu("ʵ����Ϣ��ѯ", "ForeignCurrencyPay/RealNameInformationQuery.aspx");
                HKWalletPay.AddSubMenu("ʵ�������ѯ", "ForeignCurrencyPay/RealNameCheck.aspx");
            }
            #endregion

            #region ϵͳ����
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SystemManagement", this))
            {
                SysManage1.Visible = true;
                FreezeManage1.Visible = true;
                SpecialManageControl1.Visible = true;
                BGBatchMenuControl.Visible = true;

                SysManage1.AddSubMenu("ϵͳ�������", "SysManage/SysBulletinManage.aspx");
                SysManage1.AddSubMenu("���нӿ�ά������", "SysManage/BankInterfaceManage.aspx");
                SysManage1.AddSubMenu("���з�����Ϣ����", "SysManage/BankClassifyManage.aspx");//ҳ��Ȩ��λ��BankClassifyInfo

                BGBatchMenuControl.Title = "BG��������";
                BGBatchMenuControl.AddSubMenu("��ʷ����Ǩ��", "TradeManage/OrderMigration.aspx");
                BGBatchMenuControl.AddSubMenu("��ʷ���׵�Ǩ��", "TradeManage/TradeMigration.aspx");
                BGBatchMenuControl.AddSubMenu("�˿��̻�¼��", "InternetBank/RefundMerchant.aspx");//ҳ��Ȩ��λ��RefundMerchantCheck
                BGBatchMenuControl.AddSubMenu("����ʵʱ��ѯ", "TradeManage/RealTimeOrderQuery.aspx");
            }
            #endregion

            #region  ���׹���
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeManagement", this))
            {
                tradeManage1.Visible = true;
                LifeFeeDetailManage1.Visible = true;
                InternetBank.Visible = true;
                BankBillManage1.Visible = true;

                tradeManage1.AddSubMenu("δ��ɽ��׵���ѯ", "TradeManage/UnFinishTradeQuery.aspx");
                tradeManage1.AddSubMenu("ע��ǰ���ײ�ѯ", "TradeManage/BeforeCancelTradeQuery.aspx");
                tradeManage1.AddSubMenu("�н鶩����ѯ", "TradeManage/OrderQuery.aspx");
                //MediumTradeManage1.AddSubMenu("�н鶩����ѯ", "TradeManage/OrderQuery.aspx");
                tradeManage1.AddSubMenu("���׼�¼��ѯ", "TradeManage/TradeLogQuery.aspx");
                tradeManage1.AddSubMenu("���׼�¼��ѯ(��)", "TradeManage/TradeLogQueryNew.aspx");
                //FundQuery
                //  BankBillManage1.AddSubMenu("����ʵʱ����", "TradeManage/RealtimeOrder.aspx");
                tradeManage1.AddSubMenu("��ֵ��¼��ѯ", "TradeManage/FundQuery.aspx");
                tradeManage1.AddSubMenu("��ֵ��¼��ѯ(��)", "TradeManage/FundQueryNew.aspx");
                tradeManage1.AddSubMenu("���ж�����ѯ", "TradeManage/BankOrderListQuery.aspx");//ҳ��Ȩ��λ��InternetBankRefund
                tradeManage1.AddSubMenu("ת�˵���ѯ", "TradeManage/TransferQuery.aspx");
                //tradeManage1.AddSubMenu("��ʷ���׵�Ǩ��", "TradeManage/TradeMigration.aspx");
                //tradeManage1.AddSubMenu("��ʷ����Ǩ��", "TradeManage/OrderMigration.aspx");
                //UserBankInfoQuery
                tradeManage1.AddSubMenu("���ּ�¼��ѯ", "TradeManage/PickQueryNew.aspx");
                tradeManage1.AddSubMenu("�˿��ѯ", "TradeManage/B2CReturnQuery.aspx");
                tradeManage1.AddSubMenu("ͬ����¼��ѯ", "TradeManage/SynRecordQuery.aspx");
                tradeManage1.AddSubMenu("�̻������嵥", "TradeManage/TradeLogList.aspx");
                //tradeManage1.AddSubMenu("�û��ֻ���ֵ��¼��ѯ", "TradeManage/MobileRechargeQuery.aspx");
                tradeManage1.AddSubMenu("���ж����Ų�ѯ", "TradeManage/BankBillNoQuery.aspx");



                LifeFeeDetailManage1.AddSubMenu("����ɷѲ�ѯ", "TradeManage/FeeQuery.aspx");
                LifeFeeDetailManage1.AddSubMenu("�ʴ�����ѯ", "RemitCheck/RemitQueryNew.aspx");
                LifeFeeDetailManage1.AddSubMenu("�Զ���ֵ", "TradeManage/AutomaticRechargeQuery.aspx");
                LifeFeeDetailManage1.AddSubMenu("��������ֵ��ѯ", "TradeManage/BusCardPrepaidQuery.aspx");
                LifeFeeDetailManage1.AddSubMenu("�ֻ���ֵ����ֵ��ѯ", "TradeManage/MobileRechargeQueryNew.aspx");

                InternetBank.AddSubMenu("��Ա�Żݶ��", "InternetBank/MermberDiscount.aspx");
                InternetBank.AddSubMenu("�Զ����Ѳ�ѯ", "InternetBank/AtuoRenewQuery.aspx");

                BankBillManage1.AddSubMenu("�����˵�����", "TradeManage/RefundMain.aspx");
                BankBillManage1.AddSubMenu("�˵����ܲ�ѯ", "RefundManage/RefundTotalQuery.aspx");
                BankBillManage1.AddSubMenu("�˵��쳣���ݲ�ѯ", "RefundManage/RefundErrorHandle.aspx");
                BankBillManage1.AddSubMenu("�˿�ʧ�ܲ�ѯ", "RefundManage/RefundRegistration.aspx");
                BankBillManage1.AddSubMenu("�����쳣��ѯ֪ͨ", "RefundManage/PaymentAbnormalQueryNotify.aspx");
                BankBillManage1.AddSubMenu("���ܸ�������", "BaseAccount/batPay.aspx");
            }
            #endregion

            #region  �̻���Ϣ����
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SPInfoManagement", this))
            {
                accountManage1.Visible = true;
                PNRQuery1.Visible = true;
                AccountLedgerManage1.Visible = true;
                AccountOperaManage1.Visible = true;
                DKManageControl1.Visible = true;
                DFManageControl1.Visible = true;
                TravelPlatform1.Visible = true;
                ForeignCurrencyAccount1.Visible = true;

                accountManage1.AddSubMenu("���ֹ����ѯ", "TradeManage/AppealDSettings.aspx");
                accountManage1.AddSubMenu("ֱ���̻���ѯ", "BaseAccount/PayBusinessQuery.aspx");
                accountManage1.AddSubMenu("�����ѯ", "TradeManage/SettQuery.aspx");
                accountManage1.AddSubMenu("�����ѯ(��)", "TradeManage/SettQueryNew.aspx");
                accountManage1.AddSubMenu("��ͬ��ѯ", "TradeManage/ContractQuery.aspx");
                accountManage1.AddSubMenu("֤������ѯ", "TradeManage/MediCertManage.aspx");
                accountManage1.AddSubMenu("֤��CGI��ѯ", "TradeManage/CGIInfoQuery.aspx");
                accountManage1.AddSubMenu("֤�鵽�ڲ�ѯ", "TradeManage/MediCertExpireQuery.aspx");
                accountManage1.AddSubMenu("�Զ��۷�Э��", "TradeManage/PayLimitManage.aspx");
                accountManage1.AddSubMenu("T+0�����ѯ", "TradeManage/BatPayQuery.aspx");
                accountManage1.AddSubMenu("΢��/�Ƹ�ͨ�̻���Ϣ����", "WebchatPay/WechatMerchantTrans.aspx");
                accountManage1.AddSubMenu("�̻�������ѯ�Ƹ�ͨ����", "TradeManage/QuerySpOrderPage.aspx");
                accountManage1.AddSubMenu("����BD�̻���ѯ", "BaseAccount/SelfQuery.aspx");//ҳ��Ȩ�ޣ�DrawAndApprove

                PNRQuery1.AddSubMenu("PNRǩԼ��ϵ��ѯ", "BaseAccount/PNRQuery.aspx");
                PNRQuery1.AddSubMenu("PNR������ѯ", "TradeManage/PNROrderQuery.aspx");
                PNRQuery1.AddSubMenu("PNR������ѯ", "TradeManage/PNROperateQuery.aspx");

                AccountLedgerManage1.AddSubMenu("���˶�����ˮ", "BaseAccount/SeparateOperation.aspx");
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

                //AccountOperaManage1.AddSubMenu("Ͷ���̻�¼��", "TradeManage/ComplainBussinessInput.aspx");
                //AccountOperaManage1.AddSubMenu("�û�Ͷ��", "TradeManage/ComplainUserInput.aspx");
                //DrawAndApprove

                //AccountOperaManage1.AddSubMenu("�����̻����", "BaseAccount/SelfQueryApprove.aspx");//20160715 ������: �ͷ�ϵͳ��ѯҳ�����Σ������ȱ��������ɿɻָ�����ֹ����ҵ������Ҫ����ʹ��
                AccountOperaManage1.AddSubMenu("�̻��޸��������", "BaseAccount/DomainApprove.aspx");//ҳ��Ȩ��λ��DrawAndApprove
                AccountOperaManage1.AddSubMenu("�̻�Ӫ�������", "BaseAccount/ValueAddedTaxApprove.aspx");//ҳ��Ȩ��λ��DrawAndApprove
                AccountOperaManage1.AddSubMenu("�̻�Ӫ������ѯ", "BaseAccount/ValueAddedTaxQuery.aspx");//ҳ��Ȩ��λ��DrawAndApprove
                AccountOperaManage1.AddSubMenu("��������ѯ", "TradeManage/AppealSSetting.aspx");
                //AccountOperaManage1.AddSubMenu("֤��֪ͨ����������", "SpSettle/CertBlackList.aspx");


                DKManageControl1.AddSubMenu("���۵��ʲ�ѯ", "NewQueryInfoPages/QueryDKInfoPage.aspx");//ҳ��Ȩ��λ��DKAdjust
                DKManageControl1.AddSubMenu("����������ѯ", "NewQueryInfoPages/QueryDKListInfoPage.aspx");
                DKManageControl1.AddSubMenu("�������β�ѯ", "NewQueryInfoPages/QueryBankListInfoPage.aspx");
                DKManageControl1.AddSubMenu("�����޶��ѯ", "NewQueryInfoPages/QueryDKLimitPage.aspx");
                DKManageControl1.AddSubMenu("�̻����Բ�ѯ", "NewQueryInfoPages/QueryDKServicePage.aspx");
                DKManageControl1.AddSubMenu("Э����ϸ��ѯ", "NewQueryInfoPages/QueryDKcontractPage.aspx");
                DKManageControl1.AddSubMenu("Э�����β�ѯ", "NewQueryInfoPages/QueryDKcontractListPage.aspx");
                DKManageControl1.AddSubMenu("Э��⵼��", "NewQueryInfoPages/ProtocolLibImport.aspx");
                DKManageControl1.AddSubMenu("Э����ѯ", "NewQueryInfoPages/QueryProtocolLib.aspx");
                if (classLibrary.ClassLib.ValidateRight("DKAdjust", this))
                {
                    DKManageControl1.AddSubMenu("�����ļ�����", "NewQueryInfoPages/DKAdjust.aspx?querytype=fileselect");
                    //DKManageControl1.AddSubMenu("����״̬��ѯ", "NewQueryInfoPages/DK_QueryAdjust.aspx");
                }
                //yinhuang������ز˵�Ҫ��֤Ȩ������
                //DKAdjust
                DKManageControl1.AddSubMenu("����״̬��ѯ", "NewQueryInfoPages/DK_QueryAdjust.aspx");//ҳ��Ȩ��λ��DKAdjust

                DFManageControl1.AddSubMenu("�������ʲ�ѯ", "NewQueryInfoPages/DFDetailQuery.aspx");
                DFManageControl1.AddSubMenu("����������ѯ", "NewQueryInfoPages/DFBatchQuery.aspx");
                DFManageControl1.AddSubMenu("�Ƹ�ͨת��", "NewQueryInfoPages/CFTTransferQuery.aspx");

                TravelPlatform1.AddSubMenu("��Ʊ������ѯ", "TravelPlatform/AirTicketsOrderQuery.aspx");
                TravelPlatform1.AddSubMenu("�Ƶ궩����ѯ", "TravelPlatform/HotelOrderQuery.aspx");

                ForeignCurrencyAccount1.AddSubMenu("�̻���Ϣ��ѯ", "ForeignCurrencyPay/FCAInfoQuery.aspx");
                ForeignCurrencyAccount1.AddSubMenu("�̻������ѯ", "ForeignCurrencyPay/FCASettlementQuery.aspx");
                ForeignCurrencyAccount1.AddSubMenu("�̻������ѯ", "ForeignCurrencyPay/FCATransferQuery.aspx");
            }
            #endregion

            #region ���ͨ
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("LCTMenu", this))
            {
                //���ͨ
                FundControl1.Visible = true;
                FundControl1.AddSubMenu("���ͨ��ѯ", "NewQueryInfoPages/GetFundRatePage.aspx");
                FundControl1.AddSubMenu("���ͨ��ȫ��", "WebchatPay/SafeCardManage.aspx");
                FundControl1.AddSubMenu("������", "WebchatPay/FundFixedInvestment.aspx");
                FundControl1.AddSubMenu("���ͨԤԼ����", "WebchatPay/LCTReserveOrder.aspx");
                FundControl1.AddSubMenu("���ͨ��ֵȯ", "WebchatPay/AddedValueTicketQuery.aspx");
                FundControl1.AddSubMenu("��Լ����ѯ", "WebchatPay/QueryContractMachine.aspx");
                FundControl1.AddSubMenu("���ͨתͶ��ѯ", "WebchatPay/LCTSwitchQuery.aspx");
                FundControl1.AddSubMenu("���۽��ײ�ѯ", "WebchatPay/QuotationTransactionQuery.aspx");
            }
            #endregion

            #region  ��q֧��
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("HandQMenu", this))
            {
                HandQBusiness.Visible = true;
                //��Q֧��
                HandQBusiness.AddSubMenu("��Q�����ѯ", "HandQBusiness/FindHandQRedPacket.aspx");
                //HandQBusiness.AddSubMenu("��Q�����ѯ", "HandQBusiness/RefundHandQQuery.aspx");
                HandQBusiness.AddSubMenu("��Qת�˲�ѯ", "HandQBusiness/HandQTransQuery.aspx");
            }
            #endregion

            #region ���ÿ�����
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("CreditQueryMenu", this))
            {
                CreditPayMenuControl.Visible = true;
                //���ÿ�����
                CreditPayMenuControl.Title = "���ÿ�����";

                CreditPayMenuControl.AddSubMenu("��Q�����ѯ", "HandQBusiness/RefundHandQQuery.aspx");
                CreditPayMenuControl.AddSubMenu("���ÿ�����", "TradeManage/CreditQueryNew.aspx");
                CreditPayMenuControl.AddSubMenu("΢�����ÿ�����", "WebchatPay/CreditCardRefundQuery.aspx");
                CreditPayMenuControl.AddSubMenu("ʵʱ�����ѯ", "WebchatPay/QueryRealtimeRepayment.aspx");
            }
            #endregion

            #region ���֧��
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("FastPayMenu", this))
            {
                //���֧��
                FastPayMenuControl.Visible = true;
                FastPayMenuControl.Title = "���֧��";
                FastPayMenuControl.AddSubMenu("һ��ͨҵ��", "BaseAccount/BankCardUnbind.aspx");
                FastPayMenuControl.AddSubMenu("һ��ͨҵ��(��)", "BaseAccount/BankCardUnbindNew.aspx");
                FastPayMenuControl.AddSubMenu("���п���ѯ", "TradeManage/BankCardQueryNew.aspx");
                FastPayMenuControl.AddSubMenu("���вο��Ų�ѯ", "TradeManage/BankRefereNoQuery.aspx");
                FastPayMenuControl.AddSubMenu("������Ƨ��", "BaseAccount/RareNameQuery.aspx");
                FastPayMenuControl.AddSubMenu("����Ϣ��ѯ", "BaseAccount/CardInfoQuery.aspx");//ҳ��Ȩ��λ��InternetBankRefund
            }
            #endregion

            #region ����˾ҵ��
            if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TencentbusinessMenu", this))
            {
                //����˾ҵ��
                TencentbusinessMenuControl.Visible = true;
                TencentbusinessMenuControl.Title = "����˾ҵ��";
                TencentbusinessMenuControl.AddSubMenu("�˿�Ǽ�", "InternetBank/RefundQuery.aspx");//ҳ��Ȩ��λ��InternetBankRefund
            }
            #endregion
        }


    }
}
