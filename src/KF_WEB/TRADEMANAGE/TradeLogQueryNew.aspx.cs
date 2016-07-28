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
using System.Configuration;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using CFT.CSOMS.BLL.WechatPay;
using log4net;
using CFT.CSOMS.BLL.TradeModule;
using CFT.CSOMS.BLL.TransferMeaning;


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
    /// <summary>
    /// TradeLogQuery ��ժҪ˵����
    /// </summary>
    public partial class TradeLogQueryNew : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        public string iFramePath_Gathering;
        public string iFramePath_PaymentLog;
        public string iFramePath_bankrollLog;
        public string iFramePath_TradeLog;    //����iFrame��·��
        public string iFrameHeight;  //����iFrame(�û����׼�¼)��ʾ����ĸ߶�
        public string iFrameBank;    //����iFrameBank����ʾ����ĸ߶�

        private bool tradeUpOrDown
        {
            get
            {
                if (ViewState["tradeUpOrDown"] == null)
                    return true;
                else
                    return ViewState["tradeUpOrDown"].ToString().ToLower() == "true";
            }
            set
            {
                ViewState["tradeUpOrDown"] = value.ToString();
            }
        }
        bool sign
        {
            get
            {
                if (ViewState["sign"] == null)
                    return true;
                else
                    return ViewState["sign"].ToString().ToLower() == "true";
            }
            set
            {
                ViewState["sign"] = value.ToString();
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��
            if (!IsPostBack)
            {
                string uid;
                this.Label_uid.Text = uid = Session["uid"] as string;
                if (uid == null || !TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeManagement", this))
                    Response.Redirect("../login.aspx?wh=1");

                try  //�����QQ��ѯ�õ�listID����ת����
                {
                    string tmp = Request.QueryString["tdeid"];
                    if (tmp != null && tmp.ToString() != "")
                    {
                        this.TextBox1_ListID.Text = new PickService().TdeToID(tmp);
                    }
                    else
                    {
                        this.TextBox1_ListID.Text = Request.QueryString["id"].ToString();
                    }
                    clickEvent();
                }
                catch //���û�в���������������ҳ�洦��
                {
                    iFrameHeight = "0";

                    tradeUpOrDown = true;
                }
            }
            else
            {
                iFrameHeight = "0";

                tradeUpOrDown = true;
            }
            SetButtonVisible();
        }

        private void SetButtonVisible()
        {
            string szkey = Session["szkey"].ToString();

            if (LinkButton3_action.Text == "����")
            {
                bool userright = classLibrary.ClassLib.ValidateRight("LockTradeList", this);

                if (!userright) LinkButton3_action.Visible = false;
            }
            else if (LinkButton3_action.Text == "����")
            {
                bool userright = classLibrary.ClassLib.ValidateRight("UnLockTradeList", this);

                if (!userright) LinkButton3_action.Visible = false;
            }
        }

        private void setIframePath()
        {
            iFramePath_Gathering = "../BaseAccount/GatheringLog.aspx?type=ListID";    //�տ��¼	
            iFramePath_PaymentLog = "../BaseAccount/PaymentLog.aspx?type=ListID";      //�����¼
            iFramePath_bankrollLog = "../BaseAccount/bankrolllog.aspx?type=ListID";    //�ʽ���ˮ
            iFramePath_TradeLog = "./UserTradeLog.aspx?type=ListID";      //������ˮ
        }

        private void clickEvent()
        {
            if (Session["uid"] == null)
            {
                Response.Redirect("../login.aspx?wh=1"); //���µ�½
            }
            try
            {
                hlOrder.NavigateUrl = "#";

                int iType = 4;

                //�ж��Ǹ��ݽ��׵��Ų�ѯ���Ǹ������ж����Ų�ѯ

                var listID = this.TextBox1_ListID.Text.Trim(); // ����ǽ��׵� �Ͷ�ȡtextBox�е�ֵ

                if (listID.Length < 1)
                {
                    WebUtils.ShowMessage(this.Page, "�����뽻�׵���"); return;
                }

                //�󶨽��׵�������Ϣ
                Session["ListID"] = listID;
                BindTradeInfo(iType, listID);                                     

                iFrameHeight = "85";   //iFame��ʾ����ĸ߶�

                setIframePath();        //����·��				

                hlOrder.NavigateUrl = "OrderDetail.aspx?listid=" + listID;
            }
            catch (Exception e)
            {
                iFrameHeight = "0";

                WebUtils.ShowMessage(this.Page, "��ѯ����:" + e.Message.ToString());
            }
        }

        private void BindTradeInfo(int iType, string selectStrSession)
        {
            if (Session["uid"] == null)
            {
                Response.Redirect("../login.aspx?wh=1"); //���µ�½
            }

            //�󶨽���������Ϣ
            Query_Service.Query_Service myService = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
            DateTime beginTime = DateTime.Parse(ConfigurationManager.AppSettings["sBeginTime"].ToString());

            DateTime endTime = DateTime.Parse(ConfigurationManager.AppSettings["sEndTime"].ToString());

            int istr = 1;

            int imax = 2;

            DataSet ds;

            string log = classLibrary.SensitivePowerOperaLib.MakeLog("get", Session["uid"].ToString().Trim(), "[���׼�¼��ѯ]",
                selectStrSession, iType.ToString(), beginTime.ToString(), endTime.ToString(), istr.ToString(), imax.ToString());

            if (!classLibrary.SensitivePowerOperaLib.WriteOperationRecord("TradeManagement", log, this))
            {

            }

            ds = new TradeService().GetPayByListid(selectStrSession);

            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                btn_tradeBaseInfo.NavigateUrl = "/TradeManage/OrderDetail2.aspx?listid=" + selectStrSession;
                //listShow = "false";
                throw new Exception("���ݿ��޴˼�¼");
            }
            //else
            //{
            //    //listShow = "true";
            //}

            ds.Tables[0].Columns.Add("Fpay_type_str");  //֧������
            ds.Tables[0].Columns.Add("Fpaybuy_str"); //����ҽ��
            ds.Tables[0].Columns.Add("Fpaysale_str"); //�����ҽ��
            ds.Tables[0].Columns.Add("Fappeal_sign_str"); //���߱�־
            ds.Tables[0].Columns.Add("Fmedi_sign_str"); //�н��־
            ds.Tables[0].Columns.Add("Fchannel_id_str"); //�������

            classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fpaybuy", "Fpaybuy_str");
            classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fpaysale", "Fpaysale_str");

            string strtmp = ds.Tables[0].Rows[0]["Fappeal_sign"].ToString();
            if (strtmp == "1")
            {
                ds.Tables[0].Rows[0]["Fappeal_sign_str"] = "����";
            }
            else if (strtmp == "2")
            {
                ds.Tables[0].Rows[0]["Fappeal_sign_str"] = "��ת����";
            }
            else
            {
                ds.Tables[0].Rows[0]["Fappeal_sign_str"] = "δ֪����" + strtmp;
            }

            strtmp = ds.Tables[0].Rows[0]["Fmedi_sign"].ToString();
            if (strtmp == "1")
            {
                ds.Tables[0].Rows[0]["Fmedi_sign_str"] = "���н齻��";
            }
            else if (strtmp == "2")
            {
                ds.Tables[0].Rows[0]["Fmedi_sign_str"] = "���н齻��";
            }
            else if (strtmp == "0")
            {
                ds.Tables[0].Rows[0]["Fmedi_sign_str"] = "���н齻��(2.0)";
            }
            else
            {
                ds.Tables[0].Rows[0]["Fmedi_sign_str"] = "δ֪����" + strtmp;
            }

            strtmp = ds.Tables[0].Rows[0]["Fchannel_id"].ToString();
            Hashtable ht = new Hashtable();
            ht.Add("1", "�Ƹ�ͨ");
            ht.Add("2", "������");
            ht.Add("3", "�ͻ���СǮ��");
            ht.Add("4", "�ֻ�֧��");
            ht.Add("5", "������");
            ht.Add("6", "ivr");
            ht.Add("7", "ƽ̨ר���˻�");
            ht.Add("8", "ί�д���"); //ht.Add("8", "�����������");
            ht.Add("9", "΢֧��");
            ht.Add("100", "�ֻ��ͻ��ˣ��ֻ���ֵ����ֵ�Ƹ�ͨ��");
            ht.Add("101", "�ֻ��Ƹ�ͨHTMl5֧�����ģ��ֻ���ֵ����ֵ�Ƹ�ͨ��");
            ht.Add("102", "�ֻ�qq");
            ht.Add("103", "Pos�յ�");
            ht.Add("104", "΢����");
            ht.Add("105", "΢��Webɨ��֧��");
            ht.Add("106", "΢��app��ת֧��");
            ht.Add("107", "΢�Ź����ʺ���֧��");
            ht.Add("108", "�ֻ�֧��-wap");
            ht.Add("109", "�ֻ�֧��-HTML5");
            ht.Add("110", "�ֻ�֧��-�ͻ���");
            ht.Add("111", "��q֧��");
            ht.Add("112", "��ƽSDK");
            //2015-2-27��Ծǿ���
            ht.Add("113", "΢������֧��");
            ht.Add("114", "΢��b2cת��");
            ht.Add("115", "΢��ƽ̨���֧��");
            ht.Add("116", "΢��c2cת��");
            ht.Add("117", "��Q b2c");
            //2016-02-23 ���
            ht.Add("118", "΢��ί�д���");
            ht.Add("119", "��Qί�д���");
            ht.Add("120", "��q c2cת��");
            ht.Add("121", "΢��ʵʱ����");
            ht.Add("122", "΢��ʵʱ����");
            ht.Add("123", "�����ص�����");
            ht.Add("124", "�Ƹ�ͨnfc֧������q��");
            ht.Add("125", "��Qƽ̨���֧��");
            ht.Add("126", "��Qɨ��֧��");
            ht.Add("127", "��Q���ں�֧��");
            ht.Add("128", "��Q����֧��");
            classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fchannel_id", "Fchannel_id_str", ht);


            //�ر�ԭ��,֧�������к� yinhuang 2014/1/9
            this.lbPayBindSeqId.Text = PublicRes.objectToString(ds.Tables[0], "Fbuy_bankid");
            //this.lbCloseReason.Text = PublicRes.objectToString(ds.Tables[0], "Fstandby8");
            string s_close_reason = PublicRes.objectToString(ds.Tables[0], "Fstandby8");
            if (!string.IsNullOrEmpty(s_close_reason))
            {
                if (s_close_reason == "1")
                {
                    this.lbCloseReason.Text = "��عرն���";
                }
                else if (s_close_reason == "2")
                {
                    this.lbCloseReason.Text = "΢������֧���̻��رն���";
                }
                else if (s_close_reason == "3")
                {
                    this.lbCloseReason.Text = "����ȯ���չرն���";
                }
                else if (s_close_reason == "4")
                {
                    this.lbCloseReason.Text = "���Ĺرն���";
                }
                else if (s_close_reason == "5")
                {
                    this.lbCloseReason.Text = "�⸶���ʶ���";
                }
            }

            this.LB_Fbank_backid.Text = ds.Tables[0].Rows[0]["Fbank_backid"].ToString();
            this.LB_Fbank_listid.Text = ds.Tables[0].Rows[0]["Fbank_listid"].ToString();
            this.LB_Fbargain_time.Text = ds.Tables[0].Rows[0]["Fbargain_time"].ToString();
            this.LB_Fbuy_bank_type.Text = Transfer.convertbankType(ds.Tables[0].Rows[0]["Fbuy_bank_type"].ToString());
            //this.LB_Fbuy_bankid.Text = "";
            this.LB_Fbuy_name.Text = ds.Tables[0].Rows[0]["Fbuy_name"].ToString();
            this.LB_Fbuy_uid.Text = ds.Tables[0].Rows[0]["Fbuy_uid"].ToString();

            //this.LB_Fbuyid.Text = ds.Tables[0].Rows[0]["Fbuyid"].ToString();

            this.LB_Fcarriage.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fcarriage"].ToString());
            this.LB_Fcash.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fcash"].ToString());
            string fcoding = ds.Tables[0].Rows[0]["Fcoding"].ToString();
            string banktype = ds.Tables[0].Rows[0]["Fbuy_bank_type"].ToString();
            this.HyperLink1.Text = fcoding;

            if (ds.Tables[0].Rows[0]["Fspid"].ToString().Trim() == System.Configuration.ConfigurationManager.AppSettings["QQCOMSP"].Trim())
            {
                this.HyperLink1.NavigateUrl = String.Format(System.Configuration.ConfigurationManager.AppSettings["OrderUrlPath"]);
            }
            else
            {
                this.HyperLink1.NavigateUrl = "";
            }

            this.LB_Fcreate_time.Text = ds.Tables[0].Rows[0]["Fcreate_time"].ToString();
            this.LB_FCreate_time_c2c.Text = ds.Tables[0].Rows[0]["Fcreate_time_c2c"].ToString();
            //this.LB_Fcurtype.Text = Transfer.convertMoney_type(ds.Tables[0].Rows[0]["Fcurtype"].ToString());
            //this.LB_Fexplain.Text = ds.Tables[0].Rows[0]["Fmemo"].ToString();
            this.LB_Ffact.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Ffact"].ToString());
            this.LB_Fip.Text = ds.Tables[0].Rows[0]["Fip"].ToString();
            this.LB_Flistid.Text = ds.Tables[0].Rows[0]["Flistid"].ToString();
            this.LB_Flstate.Text = Transfer.convertTradeState(ds.Tables[0].Rows[0]["Flstate"].ToString());
            this.LB_Fmodify_time.Text = ds.Tables[0].Rows[0]["Fmodify_time"].ToString();
            this.LB_Fpay_time.Text = ds.Tables[0].Rows[0]["Fpay_time"].ToString();

            classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0], "Fpay_type", "Fpay_type_str", "PAY_TYPE");
            this.LB_Fpay_type.Text = ds.Tables[0].Rows[0]["Fpay_type_str"].ToString();  //֧������
            this.LB_Fpaynum.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fpaynum"].ToString());
            this.LB_Fprice.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fprice"].ToString());
            this.LB_Fprocedure.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fprocedure"].ToString());
            this.LB_Freceive_time.Text = ds.Tables[0].Rows[0]["Freceive_time"].ToString();
            this.LB_Freceive_time_c2c.Text = ds.Tables[0].Rows[0]["Freceive_time_c2c"].ToString();
            this.LB_Fsale_bank_type.Text = Transfer.convertbankType(ds.Tables[0].Rows[0]["Fsale_bank_type"].ToString());
            this.LB_Fsale_bankid.Text = ds.Tables[0].Rows[0]["Fsale_bankid"].ToString();
            this.LB_Fsale_name.Text = ds.Tables[0].Rows[0]["Fsale_name"].ToString();
            this.LB_Fsale_uid.Text = ds.Tables[0].Rows[0]["Fsale_uid"].ToString();
            this.LB_Fsaleid.Text = ds.Tables[0].Rows[0]["Fsaleid"].ToString();
            //this.LB_Fservice.Text = ds.Tables[0].Rows[0]["Fservice"].ToString();
            //this.LB_Fspid.Text = ds.Tables[0].Rows[0]["Fspid"].ToString().Trim();

            this.lbAdjustFlag.Text = Transfer.convertAdjustSign(ds.Tables[0].Rows[0]["Fadjust_flag"].ToString().Trim());
            try
            {
                //�����˿�����
                if (!string.IsNullOrEmpty(this.LB_Fbargain_time.Text))
                {
                    DateTime begindate = DateTime.Parse(this.LB_Fbargain_time.Text.Trim());
                    string strBeginTime = begindate.AddDays(-7).ToString("yyyy-MM-dd 00:00:00");
                    string strEndTime = begindate.AddDays(8).ToString("yyyy-MM-dd 23:59:59");
                    Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                    DataSet tmpDS = qs.GetB2cReturnList("", strBeginTime, strEndTime, 99, 99, selectStrSession, "", "0000", 99, "", 1, 1, 10);

                    if (tmpDS != null && tmpDS.Tables.Count > 0)
                    {
                        this.Frefund_typeName.Text = tmpDS.Tables[0].Rows[0]["Frefund_typeName"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("TradeManage.TradeLogQuery", "private void BindTradeInfo(int iType)�����˿����ͣ�����", ex);
            }

            //yinhuang 2013.8.12
            // this.Frefund_typeName.Text = ds.Tables[0].Rows[0]["Frefund_type"].ToString(); //�˿ʽ
            this.FpaybuyName.Text = ds.Tables[0].Rows[0]["Fpaybuy_str"].ToString(); //����ҽ��
            this.FpaysaleName.Text = ds.Tables[0].Rows[0]["Fpaysale_str"].ToString(); //�����ҽ��
            this.Freq_refund_time.Text = ds.Tables[0].Rows[0]["Freq_refund_time"].ToString(); //�����˿�ʱ��
            this.Fok_time.Text = ds.Tables[0].Rows[0]["Fok_time"].ToString(); //�˿�ʱ��
            this.Fok_time_acc.Text = ds.Tables[0].Rows[0]["Fok_time_acc"].ToString(); //�˿�ʱ��(����)
            //this.Fappeal_signName.Text = ds.Tables[0].Rows[0]["Fappeal_sign_str"].ToString(); //���߱�־
            this.Fmedi_signName.Text = ds.Tables[0].Rows[0]["Fmedi_sign_str"].ToString(); //�н��־
            this.Fchannel_idName.Text = ds.Tables[0].Rows[0]["Fchannel_id_str"].ToString(); //�������
            this.Fmemo.Text = ds.Tables[0].Rows[0]["Fmemo"].ToString(); //����˵��

            //yinhuang 2014.08.01
            DataTable wx_dt = null;
            try
            {
                //���Ľӿڻ�δ���ߣ����¿ͷ����й���ʹ�ò��ˣ���ʱ��������
                wx_dt = new WechatPayService().QueryWxTrans(ds.Tables[0].Rows[0]["Flistid"].ToString()); //��ѯ΢��ת��ҵ��
            }
            catch (Exception ex)
            {
                LogError("TradeManage.TradeLogQuery", "private void BindTradeInfo(int iType)��ѯ΢��ת��ҵ�����", ex);
            }
            if (wx_dt != null && wx_dt.Rows.Count > 0)
            {
                LB_Fcoding.Text = PublicRes.objectToString(wx_dt, "wx_trade_id");//���˻�����������
                string scene = PublicRes.objectToString(wx_dt, "scene");//����΢��ת�ˣ�����渶��
                if (scene == "0")
                {
                    this.LB_Fexplain.Text = "΢��ת��";
                }
                else
                {
                    this.LB_Fexplain.Text = "����渶��";
                }
                //ͨ�����ҽ��׵����鸶�
                this.LB_Fbuyid.Text = PublicRes.objectToString(wx_dt, "pay_openid");
            }
            else
            {
                this.LB_Fbuyid.Text = ds.Tables[0].Rows[0]["Fbuyid"].ToString();
                this.LB_Fexplain.Text = ds.Tables[0].Rows[0]["Fmemo"].ToString();
            }

            bool isC2C = false;
            int type = 0;
            if (int.TryParse(ds.Tables[0].Rows[0]["Ftrade_type"].ToString(), out type))
            {
                if (type == 1)
                {
                    isC2C = true;
                }
            }

            this.lbTradeType.Text = Transfer.convertPayType(ds.Tables[0].Rows[0]["Ftrade_type"].ToString().Trim());

            this.Label1_listID.Text = this.TextBox1_ListID.Text.Trim();

            this.lblTradeState.Text = "";

            //��ѯ���ҲƸ�ͨ�˺� ûд��
            FastPayService fpService = new FastPayService();//fcoding��������
            if (fcoding != "" && ds.Tables[0].Rows[0]["Fsale_name"].ToString().Contains("΢��ת��ҵ����ת�˻�"))//�ñ�Ϊ΢��ת��
            {
                DataSet dsCoinWalPay = fpService.CoinWalletsPaymentQuery(fcoding);
                if (dsCoinWalPay != null & dsCoinWalPay.Tables.Count > 0 & dsCoinWalPay.Tables[0].Rows.Count > 0)
                {
                    this.LB_FsaleidCFT.Text = dsCoinWalPay.Tables[0].Rows[0]["rcv_openid"].ToString();
                }
            }

            if (ds.Tables[0].Rows[0]["Flistid"].ToString() != "")
            {
                var listID = ds.Tables[0].Rows[0]["Flistid"].ToString();
                Query_Service.Query_Service qs = new Query_Service.Query_Service();
                DataSet dsState = qs.GetQueryListDetail(listID);

                if (dsState != null && dsState.Tables.Count > 0 && dsState.Tables[0].Rows.Count > 0)
                {
                    dsState.Tables[0].Columns.Add("Ftrade_stateName");
                    classLibrary.setConfig.GetColumnValueFromDic(dsState.Tables[0], "Ftrade_state", "Ftrade_stateName", "PAY_STATE");
                    this.lblTradeState.Text = dsState.Tables[0].Rows[0]["Ftrade_stateName"].ToString();
                    if (isC2C)
                    {
                        myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
                        var dsList = myService.GetBankRollList_withID(DateTime.Now.AddDays(-PublicRes.PersonInfoDayCount), DateTime.Now.AddDays(1), listID, 1, 50);
                        bool isRefund = false;
                        bool isCompelete = false;
                        if (dsList != null && dsList.Tables.Count > 0 && dsList.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in dsList.Tables[0].Rows)
                            {
                                var state = row["Fsubject"].ToString();
                                int stateNum = 0;
                                if (int.TryParse(state, out stateNum))
                                {
                                    if (stateNum == 5 || stateNum == 6)
                                    {
                                        isRefund = true;
                                    }
                                    else if (stateNum == 3 || stateNum == 4 || stateNum == 8)
                                    {
                                        isCompelete = true;
                                    }
                                }
                            }
                            //BG_SUBJECT	1	��ֵ֧�����н��ջ��
                            //BG_SUBJECT	2	��ֵ֧��
                            //BG_SUBJECT	3	���ȷ��
                            //BG_SUBJECT	4	���ȷ�ϣ��Զ����֣�
                            //BG_SUBJECT	5	�˿�
                            //BG_SUBJECT	6	�˿�����һ��
                            //BG_SUBJECT	7	��ֵ֧�������֧����
                            //BG_SUBJECT	8	���ȷ�ϣ������ջ��
                            //BG_SUBJECT	9	���ٽ���
                            //BG_SUBJECT	10	���֧��
                            //BG_SUBJECT	11	��ֵ
                            //BG_SUBJECT	12	��ֵת��
                            //BG_SUBJECT	13	ת��
                            //BG_SUBJECT	14	����
                        }

                        if (isRefund)
                        {
                            this.lblTradeState.Text = "ת���˿�";
                        }
                        else if (isCompelete)
                        {
                            this.lblTradeState.Text = "�������";
                        }
                    }
                }
            }



            if (ds.Tables[0].Rows[0]["Flstate"].ToString() == "1") //���������״̬
            {
                this.LinkButton3_action.Text = "����";
                sign = false;
            }
            else if (ds.Tables[0].Rows[0]["Flstate"].ToString() == "2")
            {
                this.LinkButton3_action.Text = "����";
                sign = true;
            }
            else
            {
                this.LinkButton3_action.Text = "��Ч";
                this.LinkButton3_action.Visible = false;
            }

            //��qת�˵���ѯ   
            if (lbTradeType.Text.ToUpper().Contains("B2C"))
            {
                 BindHandQTransfer(LB_Fbuyid.Text,LB_Fcoding.Text);
            }

            setIframePath();
            SetButtonVisible(); //furion 20050802;
        }

        private void BindHandQTransfer(string uin,string listId)
        {           
            try
            { 
                    string errorMsg = "";
                    if (!string.IsNullOrEmpty(listId) && !string.IsNullOrEmpty(uin))
                    {
                        DataSet dsMobileQTransfer = new TradeService().GetUnfinishedMobileQTransferByListId(uin, listId, out errorMsg);
                        if (!string.IsNullOrEmpty(errorMsg))
                        {
                            WebUtils.ShowMessage(this.Page, "��qת�˵��Ų�ѯ�û���ת�˼�¼ʧ��:" + errorMsg);
                            return;
                        }
                        if (dsMobileQTransfer != null && dsMobileQTransfer.Tables.Count > 0 && dsMobileQTransfer.Tables[0].Rows.Count == 1)
                        {
                            if (dsMobileQTransfer.Tables[0].Rows[0]["result"].ToString() == "192920023" || dsMobileQTransfer.Tables[0].Rows[0]["result"].ToString() == "192920031")
                            {
                                WebUtils.ShowMessage(this.Page, "��q����������!");
                                return;
                            }
                            if (dsMobileQTransfer.Tables[0].Rows[0]["result"].ToString() == "0")
                            {
                                LB_Fsaleid.Text = dsMobileQTransfer.Tables[0].Rows[0]["seller_uin"].ToString();
                                LB_Fsale_name.Text = dsMobileQTransfer.Tables[0].Rows[0]["seller_name"].ToString();
                            }                        
                    }
                }
            }
            catch (Exception e)
            {
                LogError("TradeManage.GetUnfinishedMobileQTransferByListId", " private void BindPaymentInfo(), �쳣��", e);
            }
        }


        private void BindPaymentInfo()
        {
            try
            {
                //ת�˵���1301278501201412090000015078
                //֧������1301278501201412090010900888
                string ListID = LB_Flistid.Text;
                string qry_type = "";
                if (lbTradeType.Text.Contains("B2C"))
                {
                    qry_type = "1";
                }
                else if (lbTradeType.Text.Contains("�̻�����"))
                {
                    //wenfengke(���ķ�) 03-25 17:35:07����ʱ���qry_type=2�ģ�listid�ȴ�ͼ�еĽ��׵��ţ���Ҫ�������أ��������ع��������������Զ��е�������������
                    qry_type = "2";
                    string IsReconfig = System.Configuration.ConfigurationManager.AppSettings["HandQ_Payment_IsReconfig"].ToString();
                    if (IsReconfig == "1")
                    {
                        ListID = LB_Fcoding.Text;
                    }
                }
                else
                {
                    return;
                }
                DataSet ds = new TradeService().QueryPaymentParty(ListID, "", qry_type, "");
                //DataSet ds = new TradeService().QueryPaymentParty("1301278501201412090010900888", "", "2", "");
                //qry_type = "2";
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["result"].ToString() == "0")
                    {
                        if (qry_type == "1")
                        {
                            LB_FsaleidCFT.Text = ds.Tables[0].Rows[0]["seller_uin"].ToString();
                        }
                        else if (qry_type == "2")
                        {
                            LB_FbuyidCFT.Text = ds.Tables[0].Rows[0]["payer_uin"].ToString();
                            LB_Fbuy_name.Text = ds.Tables[0].Rows[0]["payer_name"].ToString();
                            LB_Fcoding.Text = ds.Tables[0].Rows[0]["transaction_id"].ToString();
                        }
                    }
                    else
                    {
                        throw new Exception(ds.Tables[0].Rows[0]["res_info"].ToString());
                    }
                }
                else
                {
                    throw new Exception("���ýӿڣ���ѯ�û�ת�˵���¼ʧ�ܣ�");
                }
            }
            catch (Exception e)
            {
                LogError("TradeManage.TradeLogQuery", " private void BindPaymentInfo(), �쳣��", e);
                //WebUtils.ShowMessage(this.Page, "��ѯ�û�ת�˵���¼ʧ��:" + e.Message.ToString());
            }
        }

        protected void btQuery_Click(object sender, System.EventArgs e)
        {
            btn_tradeBaseInfo.NavigateUrl = null;
            string strszkey = Session["SzKey"].ToString().Trim();
            int ioperid = Int32.Parse(Session["OperID"].ToString());
            int iserviceid = Common.AllUserRight.GetServiceID("TradeManagement");
            string struserdata = Session["uid"].ToString().Trim();
            string content = struserdata + "ִ����[���׼�¼��ѯ]����,��������[" + this.TextBox1_ListID.Text.Trim()

                + "]ʱ��:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            Common.AllUserRight.UpdateSession(strszkey, ioperid, PublicRes.GROUPID, iserviceid, struserdata, content);
            clickEvent();
        }

        protected void LinkButton3_action_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("../TradeManage/FreezeReason.aspx?id=" + sign + "&lsd=" + this.LB_Flistid.Text.Trim());//this.TextBox1_ListID.Text.Trim());
            //��ʼ��ҳ��ʹ��
            iFrameHeight = "85";   //iFame��ʾ����ĸ߶�
            //setLableText_Demo();    //��ʾ���ݸ�ֵ
            setIframePath();        //����·�� 
        }

        protected void LinkButton_synchro_Click(object sender, System.EventArgs e)
        {

            if (this.TextBox1_ListID.Text.Trim() == "")
            {
                WebUtils.ShowMessage(this.Page, "�����붩���ţ�");
            }
            else
            {
                string msg = "";
                Query_Service.Query_Service qs = new Query_Service.Query_Service();
                if (qs.Synchro_State(this.TextBox1_ListID.Text.Trim(), out msg))
                {
                    WebUtils.ShowMessage(this.Page, "ͬ���ɹ���");
                }
                else
                    WebUtils.ShowMessage(this.Page, "ͬ��ʧ�ܣ�" + msg);
            }
        }
    }

}

