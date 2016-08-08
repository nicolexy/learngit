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
    /// TradeLogQuery 的摘要说明。
    /// </summary>
    public partial class TradeLogQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        public string iFramePath_Gathering;
        public string iFramePath_PaymentLog;
        public string iFramePath_bankrollLog;
        public string iFramePath_TradeLog;    //设置iFrame的路径
        public string iFrameHeight;  //设置iFrame(用户交易记录)显示区域的高度
        public string iFrameBank;    //设置iFrameBank的显示区域的高度
        //public string listShow;

        protected System.Web.UI.WebControls.LinkButton LinkButton1;
        protected System.Web.UI.WebControls.LinkButton LinkButton2;
        protected System.Web.UI.WebControls.ImageButton ImageButton3;
        protected System.Web.UI.WebControls.ImageButton ImageButton2;
        protected System.Web.UI.WebControls.ImageButton Imagebutton1;
        protected System.Web.UI.WebControls.ImageButton Imagebutton3;
        protected System.Web.UI.WebControls.ImageButton Imagebutton4;
        protected System.Web.UI.WebControls.Label Label3_LeftAcc;
        protected System.Web.UI.WebControls.Label Label4_Freeze;
        protected System.Web.UI.WebControls.Label Label5_YestodayLeft;
        protected System.Web.UI.WebControls.Label Label6_LastModigy;
        protected System.Web.UI.WebControls.LinkButton LinkButton4;
        protected System.Web.UI.WebControls.DropDownList DropDownList1;

        //private static bool tradeUpOrDown; //furion 最烦static变量，网页上根本不敢用这种。
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

        public string id;
        //static bool sign; //要处理页面里所有的static变量
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

        public string listID;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.LinkButton3_action.Attributes["onClick"] = "if(!confirm('确定要执行该操作吗？')) return false;";
            this.LinkButton_synchro.Attributes["onClick"] = "if(!confirm('确定要同步订单状态吗？')) return false;";

            // 在此处放置用户代码以初始化页面
            if (!IsPostBack)
            {
                try
                {
                    this.Label_uid.Text = Session["uid"].ToString();

                    string szkey = Session["SzKey"].ToString();

                    int operid = Int32.Parse(Session["OperID"].ToString());

                    // if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "TradeLogQuery")) Response.Redirect("../login.aspx?wh=1");
                    if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("TradeManagement", this)) Response.Redirect("../login.aspx?wh=1");
                }
                catch  //如果没有登陆或者没有权限就跳出
                {
                    Response.Redirect("../login.aspx?wh=1");
                }

                try  //处理从QQ查询得到listID的跳转处理
                {
                    //***furionzhang 20050801 begin
                    string tmp = Request.QueryString["tdeid"];

                    if (tmp != null && tmp.ToString() != "")
                    {
                        id = TdeToID(tmp);
                    }
                    else
                    {
                        id = Request.QueryString["id"].ToString();
                    }

                    //***furionzhang 20050801 end

                    this.TextBox1_ListID.Text = id; //自动绑定传递过来的ID

                    clickEvent();
                }
                catch //如果没有参数处理，进入正常页面处理
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

            if (this.rbBank.Checked)   //银行订单号
            {
                this.lbEnter.Visible = true;
                this.txDate.Visible = true;
                //this.lkTime.Visible = true;
                this.rfvDate.Visible = true;
                this.rfvNum.Enabled = false;
            }
            else   //交易单号
            {
                this.lbEnter.Visible = false;
                this.txDate.Visible = false;
                //this.lkTime.Visible = false;
                this.rfvDate.Visible = false;
                this.rfvNum.Enabled = true;
            }

            SetButtonVisible(); //furion 20050802;
        }

        private void SetButtonVisible()
        {
            string szkey = Session["szkey"].ToString();

            if (LinkButton3_action.Text == "锁定")
            {
                bool userright = classLibrary.ClassLib.ValidateRight("LockTradeList", this);

                if (!userright) LinkButton3_action.Visible = false;
            }
            else if (LinkButton3_action.Text == "解锁")
            {
                bool userright = classLibrary.ClassLib.ValidateRight("UnLockTradeList", this);

                if (!userright) LinkButton3_action.Visible = false;
            }
        }

        private string TdeToID(string tdeid)
        {
            return new PickService().TdeToID(tdeid);
        }

        private void setIframePath()
        {
            iFramePath_Gathering = "../BaseAccount/GatheringLog.aspx?type=ListID";    //收款记录	
            iFramePath_PaymentLog = "../BaseAccount/PaymentLog.aspx?type=ListID";      //付款记录
            iFramePath_bankrollLog = "../BaseAccount/bankrolllog.aspx?type=ListID";    //资金流水
            iFramePath_TradeLog = "./UserTradeLog.aspx?type=ListID";      //交易流水
        }

        private void LKBT_TradeLog_Click(object sender, System.EventArgs e)
        {
            iFramePath_TradeLog = "UserTradeLog.aspx?type=ListID";

            //iFrameHeight = "85";
            this.LKBT_TradeLog.ForeColor = Color.Red;
            this.LKBT_bankrollLog.ForeColor = Color.Black;
            this.LKBT_GatheringLog.ForeColor = Color.Black;
            setIframePath();
        }

        private void LKBT_bankrollLog_Click(object sender, System.EventArgs e)
        {
            iFramePath_bankrollLog = "../BaseAccount/bankrolllog.aspx?type=ListID";
            iFrameHeight = "85";
            this.LKBT_TradeLog.ForeColor = Color.Black;
            this.LKBT_bankrollLog.ForeColor = Color.Red;
            this.LKBT_GatheringLog.ForeColor = Color.Black;
            setIframePath();
        }

        private void LKBT_GatheringLog_Click(object sender, System.EventArgs e)
        {
            iFramePath_Gathering = "../BaseAccount/GatheringLog.aspx?type=ListID";
            iFrameHeight = "85";
            this.LKBT_TradeLog.ForeColor = Color.Black;
            this.LKBT_bankrollLog.ForeColor = Color.Black;
            this.LKBT_GatheringLog.ForeColor = Color.Red;
            setIframePath();
        }

        private void LkBT_PaymentLog_Click(object sender, System.EventArgs e)
        {
            iFramePath_PaymentLog = "../BaseAccount/PaymentLog.aspx?type=ListID";
            iFrameHeight = "85";
            this.LKBT_TradeLog.ForeColor = Color.Black;
            this.LKBT_bankrollLog.ForeColor = Color.Black;
            this.LKBT_GatheringLog.ForeColor = Color.Black;
            setIframePath();
        }

        private void Submit1_ServerClick(object sender, System.EventArgs e)
        {
            iFrameHeight = "85";   //iFame显示区域的高度
            setIframePath();        //设置路径
        }

        private void Button1_Click(object sender, System.EventArgs e)
        {
            clickEvent();
        }

        private void clickEvent()
        {
            if (Session["uid"] == null)
            {
                Response.Redirect("../login.aspx?wh=1"); //重新登陆
            }
            try
            {
                hlOrder.NavigateUrl = "#";

                int iType;

                //判断是根据交易单号查询还是根据银行订单号查询
                if (this.rbBank.Checked == true)
                {
                    //根据定单号和日期查询交易单号
                    try
                    {
                        //如果是银行订单号 则根据银行订单号和日期去查询腾讯收款表 得到交易单号 然后再查询。
                        Query_Service.Query_Service qs = new Query_Service.Query_Service();

                        listID = qs.returnListID(this.TextBox1_ListID.Text.Trim(), this.txDate.Text.ToString());
                    }
                    catch
                    {
                        throw new Exception(this.txDate.Text.ToString() + "日的银行返回订单号" + this.TextBox1_ListID.Text.Trim() + "不存在！请核对重试。");
                    }

                    if (listID == null || listID == "") //如果交易单号不存在,返回错误信息,该日期的该银行返回的订单号不存在
                    {
                        throw new Exception(this.txDate.Text.ToString() + "日的银行返回订单号" + this.TextBox1_ListID.Text.Trim() + "不存在！请核对重试。");
                    }

                    iType = 1;
                }
                else if (this.rbList.Checked == true)
                {
                    listID = this.TextBox1_ListID.Text.Trim(); // 如果是交易单 就读取textBox中的值

                    iType = 4;
                }
                else
                {
                    throw new Exception("错误的类型！");
                }

                //绑定交易单基础信息
                Session["ListID"] = listID;

                id = listID;

                BindTradeInfo(iType);               

                iFrameHeight = "85";   //iFame显示区域的高度

                setIframePath();        //设置路径				

                hlOrder.NavigateUrl = "OrderDetail.aspx?listid=" + listID;
            }
            catch (Exception e)
            {
                iFrameHeight = "0";

                WebUtils.ShowMessage(this.Page, "查询出错:" + e.Message.ToString());
            }
        }

        private void BindTradeInfo(int iType)
        {
            if (Session["uid"] == null)
            {
                Response.Redirect("../login.aspx?wh=1"); //重新登陆
            }

            //绑定交易资料信息
            Query_Service.Query_Service myService = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

            string selectStrSession = Session["ListID"].ToString();

            DateTime beginTime = DateTime.Parse(ConfigurationManager.AppSettings["sBeginTime"].ToString());

            DateTime endTime = DateTime.Parse(ConfigurationManager.AppSettings["sEndTime"].ToString());

            int istr = 1;

            int imax = 2;

            DataSet ds = new DataSet();

            string log = classLibrary.SensitivePowerOperaLib.MakeLog("get", Session["uid"].ToString().Trim(), "[交易记录查询]",
                selectStrSession, iType.ToString(), beginTime.ToString(), endTime.ToString(), istr.ToString(), imax.ToString());

            if (!classLibrary.SensitivePowerOperaLib.WriteOperationRecord("TradeManagement", log, this))
            {

            }
            ds = myService.GetPayList(selectStrSession, iType, beginTime, endTime, istr, imax);

            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                btn_tradeBaseInfo.NavigateUrl = "/TradeManage/OrderDetail2.aspx?listid=" + selectStrSession;
                //listShow = "false";
                throw new Exception("数据库无此记录");
            }
            //else
            //{
            //    //listShow = "true";
            //}

            ds.Tables[0].Columns.Add("Fpay_type_str");  //支付类型
            ds.Tables[0].Columns.Add("Fpaybuy_str"); //退买家金额
            ds.Tables[0].Columns.Add("Fpaysale_str"); //退卖家金额
            ds.Tables[0].Columns.Add("Fappeal_sign_str"); //申诉标志
            ds.Tables[0].Columns.Add("Fmedi_sign_str"); //中介标志
            ds.Tables[0].Columns.Add("Fchannel_id_str"); //渠道编号

            classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fpaybuy", "Fpaybuy_str");
            classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fpaysale", "Fpaysale_str");

            string strtmp = ds.Tables[0].Rows[0]["Fappeal_sign"].ToString();
            if (strtmp == "1")
            {
                ds.Tables[0].Rows[0]["Fappeal_sign_str"] = "正常";
            }
            else if (strtmp == "2")
            {
                ds.Tables[0].Rows[0]["Fappeal_sign_str"] = "已转申诉";
            }
            else
            {
                ds.Tables[0].Rows[0]["Fappeal_sign_str"] = "未知类型" + strtmp;
            }

            strtmp = ds.Tables[0].Rows[0]["Fmedi_sign"].ToString();
            if (strtmp == "1")
            {
                ds.Tables[0].Rows[0]["Fmedi_sign_str"] = "是中介交易";
            }
            else if (strtmp == "2")
            {
                ds.Tables[0].Rows[0]["Fmedi_sign_str"] = "非中介交易";
            }
            else if (strtmp == "0")
            {
                ds.Tables[0].Rows[0]["Fmedi_sign_str"] = "非中介交易(2.0)";
            }
            else
            {
                ds.Tables[0].Rows[0]["Fmedi_sign_str"] = "未知类型" + strtmp;
            }

            strtmp = ds.Tables[0].Rows[0]["Fchannel_id"].ToString();
            Hashtable ht = new Hashtable();
            ht.Add("1", "财付通");
            ht.Add("2", "拍拍网");
            ht.Add("3", "客户端小钱包");
            ht.Add("4", "手机支付");
            ht.Add("5", "第三方");
            ht.Add("6", "ivr");
            ht.Add("7", "平台专用账户");
            ht.Add("8", "委托代扣"); //ht.Add("8", "基金基础代扣");
            ht.Add("9", "微支付");
            ht.Add("100", "手机客户端（手机充值卡充值财付通）");
            ht.Add("101", "手机财付通HTMl5支付中心（手机充值卡充值财付通）");
            ht.Add("102", "手机qq");
            ht.Add("103", "Pos收单");
            ht.Add("104", "微生活");
            ht.Add("105", "微信Web扫码支付");
            ht.Add("106", "微信app跳转支付");
            ht.Add("107", "微信公众帐号内支付");
            ht.Add("108", "手机支付-wap");
            ht.Add("109", "手机支付-HTML5");
            ht.Add("110", "手机支付-客户端");
            ht.Add("111", "手q支付");
            ht.Add("112", "数平SDK");
            //2015-2-27郭跃强添加
            ht.Add("113", "微信离线支付");
            ht.Add("114", "微信b2c转账");
            ht.Add("115", "微信平台余额支付");
            ht.Add("116", "微信c2c转账");
            ht.Add("117", "手Q b2c");
            //2016-02-23 添加
            ht.Add("118", "微信委托代扣");
            ht.Add("119", "手Q委托代扣");
            ht.Add("120", "手q c2c转账");
            ht.Add("121", "微信实时结算");
            ht.Add("122", "微信实时分润");
            ht.Add("123", "订单关单冲正");
            ht.Add("124", "财付通nfc支付（手q）");
            ht.Add("125", "手Q平台余额支付");
            ht.Add("126", "手Q扫码支付");
            ht.Add("127", "手Q公众号支付");
            ht.Add("128", "手Q境外支付");
            classLibrary.setConfig.DbtypeToPageContent(ds.Tables[0], "Fchannel_id", "Fchannel_id_str", ht);
          

            //关闭原因,支付绑定序列号 yinhuang 2014/1/9
            this.lbPayBindSeqId.Text = PublicRes.objectToString(ds.Tables[0], "Fbuy_bankid");
            //this.lbCloseReason.Text = PublicRes.objectToString(ds.Tables[0], "Fstandby8");
            string s_close_reason = PublicRes.objectToString(ds.Tables[0], "Fstandby8");
            if (!string.IsNullOrEmpty(s_close_reason)) 
            {
                if (s_close_reason == "1") 
                {
                    this.lbCloseReason.Text = "风控关闭订单";
                }
                else if (s_close_reason == "2") 
                {
                    this.lbCloseReason.Text = "微信线下支付商户关闭订单";
                }
                else if (s_close_reason == "3")
                {
                    this.lbCloseReason.Text = "购物券回收关闭订单";
                }
                else if (s_close_reason == "4")
                {
                    this.lbCloseReason.Text = "拍拍关闭订单";
                }
                else if (s_close_reason == "5")
                {
                    this.lbCloseReason.Text = "赔付调帐订单";
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
            this.LB_Fpay_type.Text = ds.Tables[0].Rows[0]["Fpay_type_str"].ToString();  //支付类型
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
                //增加退款类型
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
                LogError("TradeManage.TradeLogQuery", "private void BindTradeInfo(int iType)增加退款类型：出错", ex);               
            }

            //yinhuang 2013.8.12
           // this.Frefund_typeName.Text = ds.Tables[0].Rows[0]["Frefund_type"].ToString(); //退款方式
            this.FpaybuyName.Text = ds.Tables[0].Rows[0]["Fpaybuy_str"].ToString(); //退买家金额
            this.FpaysaleName.Text = ds.Tables[0].Rows[0]["Fpaysale_str"].ToString(); //退卖家金额
            this.Freq_refund_time.Text = ds.Tables[0].Rows[0]["Freq_refund_time"].ToString(); //请求退款时间
            this.Fok_time.Text = ds.Tables[0].Rows[0]["Fok_time"].ToString(); //退款时间
            this.Fok_time_acc.Text = ds.Tables[0].Rows[0]["Fok_time_acc"].ToString(); //退款时间(账务)
            //this.Fappeal_signName.Text = ds.Tables[0].Rows[0]["Fappeal_sign_str"].ToString(); //申诉标志
            this.Fmedi_signName.Text = ds.Tables[0].Rows[0]["Fmedi_sign_str"].ToString(); //中介标志
            this.Fchannel_idName.Text = ds.Tables[0].Rows[0]["Fchannel_id_str"].ToString(); //渠道编号
            this.Fmemo.Text = ds.Tables[0].Rows[0]["Fmemo"].ToString(); //交易说明

            //yinhuang 2014.08.01
            DataTable wx_dt = null;
            try
            {
                //担心接口还未上线，导致客服现有功能使用不了，暂时这样处理
                wx_dt = new WechatPayService().QueryWxTrans(ds.Tables[0].Rows[0]["Flistid"].ToString()); //查询微信转账业务
            }
            catch (Exception ex)
            {
                LogError("TradeManage.TradeLogQuery", "private void BindTradeInfo(int iType)查询微信转账业务出错", ex);
            }
            if (wx_dt != null && wx_dt.Rows.Count > 0)
            {
                LB_Fcoding.Text = PublicRes.objectToString(wx_dt, "wx_trade_id");//子账户关联订单号
                string scene = PublicRes.objectToString(wx_dt, "scene");//区分微信转账，面对面付款
                if (scene == "0")
                {
                    this.LB_Fexplain.Text = "微信转账";
                }
                else 
                {
                    this.LB_Fexplain.Text = "面对面付款";
                }
                //通过卖家交易单反查付款方
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

            //查询卖家财付通账号 没写完
            FastPayService fpService = new FastPayService();//fcoding订单编码
            if (fcoding != "" && ds.Tables[0].Rows[0]["Fsale_name"].ToString().Contains("微信转账业务中转账户"))//该笔为微信转账
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
                            //BG_SUBJECT	1	充值支付（中介收货款）
                            //BG_SUBJECT	2	充值支付
                            //BG_SUBJECT	3	买家确认
                            //BG_SUBJECT	4	买家确认（自动提现）
                            //BG_SUBJECT	5	退款
                            //BG_SUBJECT	6	退款（退卖家货款）
                            //BG_SUBJECT	7	充值支付（余额支付）
                            //BG_SUBJECT	8	买家确认（卖家收货款）
                            //BG_SUBJECT	9	快速交易
                            //BG_SUBJECT	10	余额支付
                            //BG_SUBJECT	11	充值
                            //BG_SUBJECT	12	充值转帐
                            //BG_SUBJECT	13	转帐
                            //BG_SUBJECT	14	提现
                        }

                        if (isRefund)
                        {
                            this.lblTradeState.Text = "转入退款";
                        }
                        else if (isCompelete)
                        {
                            this.lblTradeState.Text = "交易完成";
                        }
                    }
                }
            }



            if (ds.Tables[0].Rows[0]["Flstate"].ToString() == "1") //如果是锁定状态
            {
                this.LinkButton3_action.Text = "解锁";
                sign = false;
            }
            else if (ds.Tables[0].Rows[0]["Flstate"].ToString() == "2")
            {
                this.LinkButton3_action.Text = "锁定";
                sign = true;
            }
            else
            {
                this.LinkButton3_action.Text = "无效";
                this.LinkButton3_action.Visible = false;
            }
            //手q转账单查询   
            if (lbTradeType.Text.ToUpper().Contains("B2C"))
            {
                BindHandQTransfer(LB_Fbuyid.Text, LB_Fcoding.Text);
            }

            setIframePath();
            SetButtonVisible(); //furion 20050802;
        }

        private void BindHandQTransfer(string uin,string listId)
        {
            try
            {       
               if (lbTradeType.Text.ToUpper().Contains("B2C"))
                {                  
                    string errorMsg = "";
                    if (!string.IsNullOrEmpty(listId) && !string.IsNullOrEmpty(uin))
                    {
                        DataSet dsMobileQTransfer = new TradeService().GetUnfinishedMobileQTransferByListId(uin, listId, out errorMsg);
                        if (!string.IsNullOrEmpty(errorMsg))
                        {                           
                            return;
                        }
                        if (dsMobileQTransfer != null && dsMobileQTransfer.Tables.Count > 0 && dsMobileQTransfer.Tables[0].Rows.Count == 1)
                        {
                            if (dsMobileQTransfer.Tables[0].Rows[0]["result"].ToString() == "0")
                            {
                                LB_Fsaleid.Text = dsMobileQTransfer.Tables[0].Rows[0]["seller_uin"].ToString();
                                LB_Fsale_name.Text = dsMobileQTransfer.Tables[0].Rows[0]["seller_name"].ToString();
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogError("TradeManage.GetUnfinishedMobileQTransferByListId", " private void BindPaymentInfo(), 异常：", e);
            }
        }

        private void BindPaymentInfo()
        {
            try
            {
                //转账单号1301278501201412090000015078
                //支付单号1301278501201412090010900888
                string ListID = LB_Flistid.Text;
                string qry_type = "";
                if (lbTradeType.Text.Contains("B2C"))
                {
                    qry_type = "1";
                }
                else if (lbTradeType.Text.Contains("商户结算"))
                {
                    //wenfengke(柯文锋) 03-25 17:35:07你暂时针对qry_type=2的，listid先传图中的交易单号，但要做个开关，等我们重构后，你们有能力自动切到，传订单编码
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
                    throw new Exception("调用接口：查询用户转账单记录失败！");
                }
            }
            catch (Exception e)
            {
                LogError("TradeManage.TradeLogQuery", " private void BindPaymentInfo(), 异常：", e);
                //WebUtils.ShowMessage(this.Page, "查询用户转账单记录失败:" + e.Message.ToString());
            }
        }



        private void LinkButton1_Click(object sender, System.EventArgs e)
        {
            Response.Write("<script language=javascript>window.open('LeftAccountFreeze.aspx?id=true','','left=100,top=50,width=300,height=250');</script>");

            //初始化页面使用
            iFrameHeight = "85";   //iFame显示区域的高度
            setIframePath();        //设置路径
        }



        private void LinkButton2_Click(object sender, System.EventArgs e)
        {
            Response.Write("<script language=javascript>window.open('LeftAccountFreeze.aspx?id=false','','left=100,top=50,width=300,height=250');</script>");

            //初始化页面使用
            iFrameHeight = "85";   //iFame显示区域的高度
            setIframePath();        //设置路径
        }

        private void ImageButton2_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (tradeUpOrDown == false)
            {
                this.ImageButton2.ImageUrl = "../Images/Page/down.gif";
                iFrameHeight = "0";
                setIframePath();
                tradeUpOrDown = true;
            }
            else
            {
                this.ImageButton2.ImageUrl = "../Images/Page/up.gif";
                iFrameHeight = "85";
                setIframePath();
                tradeUpOrDown = false;
            }
        }

        private void Imagebutton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {

        }

        private void Imagebutton3_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {

        }

        private void Imagebutton4_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {

        }



        private void LinkButton3_Click(object sender, System.EventArgs e)
        {

            //			Response.Write("<script language=javascript>window.open('../BaseAccount/FreezeReason.aspx?id=true','','left=100,top=50,width=300,height=250');</script>");

            Response.Redirect("../TradeManage/FreezeReason.aspx?id=" + sign + "&lsd=" + this.TextBox1_ListID.Text.Trim());

            //初始化页面使用
            iFrameHeight = "85";   //iFame显示区域的高度
            //setLableText_Demo();    //演示数据赋值
            setIframePath();        //设置路径 
        }



        private void LinkButton4_Click(object sender, System.EventArgs e)
        {
            iFramePath_bankrollLog = "../BaseAccount/Bank_Counteract.aspx?type=ListID";
            iFramePath_Gathering = "../BaseAccount/GatheringLog.aspx?type=ListID";
            iFramePath_PaymentLog = "../BaseAccount/PaymentLog.aspx?type=ListID";
            iFramePath_TradeLog = "UserTradeLog.aspx?type=ListID";
            iFrameHeight = "125";
        }

        private void DropDownList2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            clickEvent(); //触发事件
        }

        protected void LinkButton3_action_Click(object sender, System.EventArgs e)
        {

            //Response.Write("<script language=javascript>window.open('../BaseAccount/FreezeReason.aspx?id=true','','left=100,top=50,width=300,height=250');</script>");
            Response.Redirect("../TradeManage/FreezeReason.aspx?id=" + sign + "&lsd=" + this.LB_Flistid.Text.Trim());//this.TextBox1_ListID.Text.Trim());
            //初始化页面使用
            iFrameHeight = "85";   //iFame显示区域的高度
            //setLableText_Demo();    //演示数据赋值
            setIframePath();        //设置路径 
        }

        private void Button2_Click(object sender, System.EventArgs e)
        {
            clickEvent();
        }

        protected void btQuery_Click(object sender, System.EventArgs e)
        {
            btn_tradeBaseInfo.NavigateUrl = null;
            string strszkey = Session["SzKey"].ToString().Trim();
            int ioperid = Int32.Parse(Session["OperID"].ToString());
            int iserviceid = Common.AllUserRight.GetServiceID("TradeManagement");
            string struserdata = Session["uid"].ToString().Trim();
            string content = struserdata + "执行了[交易记录查询]操作,操作对象[" + this.TextBox1_ListID.Text.Trim()

                + "]时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            Common.AllUserRight.UpdateSession(strszkey, ioperid, PublicRes.GROUPID, iserviceid, struserdata, content);
            clickEvent();
        }

        //protected void lkTime_Click(object sender, System.EventArgs e)
        //{
        //    this.Calendar1.Visible = true;
        //}

        //protected void Calendar1_SelectionChanged(object sender, System.EventArgs e)
        //{
        //    this.Calendar1.Visible = false;
        //    this.txDate.Text = this.Calendar1.SelectedDate.ToString("yyyy-MM-dd");
        //}

        protected void LinkButton_synchro_Click(object sender, System.EventArgs e)
        {

            if (this.TextBox1_ListID.Text.Trim() == "")
            {
                WebUtils.ShowMessage(this.Page, "请输入订单号！");
            }
            else
            {
                string msg = "";
                Query_Service.Query_Service qs = new Query_Service.Query_Service();
                if (qs.Synchro_State(this.TextBox1_ListID.Text.Trim(), out msg))
                {
                    WebUtils.ShowMessage(this.Page, "同步成功！");
                }
                else
                    WebUtils.ShowMessage(this.Page, "同步失败：" + msg);
            }
        }
    }

}

