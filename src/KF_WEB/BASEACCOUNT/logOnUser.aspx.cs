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
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Web.Services.Protocols;
using System.Xml.Schema;
using System.Xml;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.Check_WebService;
using TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService;
using System.IO;
using CFT.CSOMS.BLL.BalanceModule;
using System.Configuration;
using CFT.CSOMS.BLL.TradeModule;
using System.Text.RegularExpressions;
using CFT.Apollo.Logging;
using CFT.CSOMS.COMMLIB;
using CFT.CSOMS.BLL.CFTAccountModule;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace TENCENT.OSS.C2C.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// logOnUser 的摘要说明。
	/// </summary>
	public partial class logOnUser : PageBase
	{
        protected BalaceService balaceService = new BalaceService();
		protected void Page_Load(object sender, System.EventArgs e)
		{             
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"CancelAccount")) Response.Redirect("../login.aspx?wh=1");

                if (!ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if (!Page.IsPostBack)
			{
				BindHistoryInfo("","",DateTime.Parse("1970-01-01 00:00:00"),DateTime.Now,0,10);
                TextBoxBeginDate.Value = DateTime.Now.ToString("yyyy-01-01");
				this.TextBoxEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
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

		}
		#endregion

		public bool BindHistoryInfo(string qqid,string handid,DateTime bgDateTime,DateTime edDateTime,int pageIndex,int pageSize)
		{
			string Msg    = "";
			try
			{
				TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService.Finance_Manage fm = new TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService.Finance_Manage();
				DataSet ds =  fm.logOnUserHistory(bgDateTime,edDateTime,qqid,handid,pageIndex,pageSize,out Msg);  //默认显示最新20条
			
				this.dgInfo.DataSource = ds;
				this.dgInfo.DataBind();

				return true;
			}
			catch(Exception e)
            {
                LogError("BaseAccount.logOnUser.public bool BindHistoryInfo(string qqid,string handid,DateTime bgDateTime,DateTime edDateTime,int pageIndex,int pageSize)", "获取数据信息异常", e);
				WebUtils.ShowMessage(this.Page,Msg + e.Message);
				return false;
			}
		}


		/// <summary>
		/// 提交销户申请。先验证帐户是否存在
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btLogOn_Click(object sender, System.EventArgs e)  //暂不开放注销功能
        {      
               // qs1.ClosedBalancePaid(this.TextBox1_QQID.Text.Trim());//测试用关闭余额支付功能，为了查询及打开
			string qqid   = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceSqlStr(this.TextBox1_QQID.Text).Trim();
            string wxid = this.TextBox2_WX.Text.Trim();
			string reason = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceSqlStr(this.txtReason.Text).Trim();
            bool emailCheck = EmailCheckBox.Checked;
            string emailAddr = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceSqlStr(this.txtEmail.Text).Trim();

            int wxFlag = 0;//是否是微信账号
            if (string.IsNullOrEmpty(TextBox1_QQID.Text) && string.IsNullOrEmpty(TextBox2_WX.Text))
            {
                ValidateID.Text = "请输入手Q账号或下方微信号";
                return;
            }
            if (!string.IsNullOrEmpty(TextBox1_QQID.Text) && TextBox1_QQID.Text.Contains("@wx.tenpay.com"))
            {
                ValidateID.Text = "微信支付帐号请输入微信号注销!";
                return;
            }
            if (!string.IsNullOrEmpty(TextBox1_QQID.Text) && !string.IsNullOrEmpty(TextBox2_WX.Text))
            {
                ValidateID.Text = "不能同时输入手Q账号和微信账号!!!";
                return;
            }
            string wxUIN = "";
            //string wxHBUIN = "";
            if (string.IsNullOrEmpty(TextBox1_QQID.Text))
            {
                if (TextBox2_WX.Text != txbConfirmQ.Text)
                {
                    ValidateID.Text = "两次输入的账号不相同，请重新输入!!!";
                    return;
                }
                wxFlag = 1;
                wxUIN = WeChatHelper.GetUINFromWeChatName(wxid);
                //wxHBUIN = WeChatHelper.GetHBUINFromWeChatName(wxid);
                qqid = wxUIN;
            }
            else if (TextBox1_QQID.Text != txbConfirmQ.Text)
            {
                ValidateID.Text = "两次输入的账号不相同，请重新输入!!!";
                return;
            }
            if (emailCheck && TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.CheckEmail(emailAddr))
            {
                WebUtils.ShowMessage(this.Page, "请输入正确格式的用户邮箱地址");
                return;
            }

			string Msg = "";

            try
            {
                Finance_Manage fm = new Finance_Manage();
                if (!fm.checkUserReg(qqid, out Msg))
                {
                    Msg = "帐号非法或者未注册！";
                    WebUtils.ShowMessage(this.Page, Msg);
                    return;
                }

                if (255 < reason.Length)
                {
                    WebUtils.ShowMessage(this.Page, "备注字数不得超过255个字符");
                    return;
                }


                Check_Service cs = new Check_Service();
                TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Finance_Header fh = setConfig.setFH_CheckService(this);
                cs.Finance_HeaderValue = fh;

                Query_Service qs = new Query_Service();
                TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Finance_Header fh2 = setConfig.setFH(this);
                qs.Finance_HeaderValue = fh2;

                string accountName = string.Empty;
                long balance = 0;//就是金额
                if (ViewState["Cached" + qqid] == null)
                {
                    //手Q用户转账中、退款中、未完成的订单禁止注销和批量注销
                    if (Regex.IsMatch(qqid, @"^[1-9]\d*$"))
                    {
                        #region 手Q特有判断

                        #region 转账
                        try
                        {
                            string mobileQTransferErrorMsg = "";
                            DataSet dsMobileQTransfer = new TradeService().GetUnfinishedMobileQTransfer(qqid, out mobileQTransferErrorMsg);
                            if (!string.IsNullOrEmpty(mobileQTransferErrorMsg))
                            {
                                WebUtils.ShowMessage(this.Page, "查询手Q转账记录失败" + mobileQTransferErrorMsg);
                                return;
                            }
                            if (dsMobileQTransfer != null && dsMobileQTransfer.Tables.Count > 0 && dsMobileQTransfer.Tables[0].Rows.Count > 0 && dsMobileQTransfer.Tables[0].Rows[0]["result"].ToString() != "192720108")
                            {
                                WebUtils.ShowMessage(this.Page, "手Q用户转账中、退款中、未完成的订单禁止注销和批量注销");
                                return;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            LogHelper.LogError("查询手Q转账记录失败" + ex.Message, "LogOnUser");
                            WebUtils.ShowMessage(this.Page, "查询手Q转账记录失败" + ex.Message);
                            return;
                        }
                        #endregion
                        //DataSet dsHandQ = new TradeService().QueryPaymentParty("", "1,2,12,4,6,7", "3", qqid);
                        //if (dsHandQ != null && dsHandQ.Tables.Count > 0 && dsHandQ.Tables[0].Rows.Count > 0 && dsHandQ.Tables[0].Rows[0]["result"].ToString() != "97420006")
                        //{
                        //    WebUtils.ShowMessage(this.Page, "手Q用户转账中、退款中、未完成的订单禁止注销和批量注销");
                        //    return;
                        //}
                        #region 红包
                        try
                        {
                            DataSet dsMobileQHB = new TradeService().GetUnfinishedMobileQHB(qqid);
                            if (dsMobileQHB.Tables[0].Columns.Contains("row_num"))
                            {
                                if (dsMobileQHB != null && dsMobileQHB.Tables.Count > 0 && dsMobileQHB.Tables[0].Rows.Count > 0 && int.Parse(dsMobileQHB.Tables[0].Rows[0]["row_num"].ToString()) > 0)
                                {
                                    WebUtils.ShowMessage(this.Page, "该账户存在未完成的手Q红包交易，禁止注销和批量注销");
                                    return;
                                }
                            }
                            else
                            {
                                WebUtils.ShowMessage(this.Page, "查询是否有未完成手Q红包交易失败!");
                                return;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            LogHelper.LogError("查询手Q红包未完成交易失败" + ex.Message, "LogOnUser");
                            WebUtils.ShowMessage(this.Page, "查询手Q红包未完成交易失败" + ex.Message);
                            return;
                        }
                        #endregion                    

                        #region 微粒贷
                        try
                        {
                            if (new TradeService().HasUnfinishedWeiLibDai(qqid))
                            {
                                WebUtils.ShowMessage(this.Page, "存在未完成的微粒贷欠款,禁止注销和批量注销");
                                return;
                            }
                        }
                        catch (Exception)
                        {
                            WebUtils.ShowMessage(this.Page, "微粒贷查询,出错");
                            return;
                        }
                        #endregion

                        #endregion
                    }

                    //有微信支付、转账、红包未完成的禁止注销和批量注销
                    if (wxFlag == 1)
                    {
                        #region 微信特有判断
                        try
                        {
                            var WXUnfinishedTrade = (new TradeService()).QueryWXUnfinishedTrade(qqid);
                            if (!WXUnfinishedTrade)
                            {
                                LogHelper.LogInfo(wxid + "此账号有未完成微信支付转账，禁止注销!");
                                WebUtils.ShowMessage(this.Page, "此账号有未完成微信支付转账，禁止注销!");
                                return;
                            }

                            var HasUnfinishedHB = (new TradeService()).QueryWXUnfinishedHB(qqid);
                            if (!HasUnfinishedHB)
                            {
                                LogHelper.LogInfo(wxid + "此账号有未完成微信红包，禁止注销!");
                                WebUtils.ShowMessage(this.Page, "此账号有未完成微信红包，禁止注销!");
                                return;
                            }
                            string resultCodeString = "";
                            var HasUnfinishedRepayment = new TradeService().QueryWXFCancelAsRepayMent(qqid, out resultCodeString);
                            if (HasUnfinishedRepayment)
                            {
                                LogHelper.LogInfo(wxid + "此账号有未完成微信还款，禁止注销!");
                                WebUtils.ShowMessage(this.Page, "此账号有未完成微信还款，禁止注销!" + resultCodeString);
                                return;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            LogError("BaseAccount.logOnUser.protected void btLogOn_Click(object sender, System.EventArgs e) ", "销户操作查询微信支付在途条目出错:", ex);
                            WebUtils.ShowMessage(this.Page, "销户操作查询微信支付在途条目出错" + ex.Message);
                            return;
                        }
                        #endregion
                    }

                    //是否有未完成的交易单
                    if (qs.LogOnUsercheckOrder(qqid, "1"))
                    {
                        WebUtils.ShowMessage(this.Page, "有未完成的交易单");
                        return;
                    }

                    if (qs.LogOnUserCheckYDT(qqid, "1"))//是否开通一点通
                    {
                        WebUtils.ShowMessage(this.Page, "开通了一点通");
                        return;
                    }

                    if (qs.LogOnUserCheckYDT(qqid, "2"))//是否开通了快捷支付
                    {
                        WebUtils.ShowMessage(this.Page, "开通了快捷支付");
                        return;
                    }

                    #region 金额判断
                    //这部分逻辑先注释掉：
                    //当用户没有关于余额支付的功能时（没有打开也没有关闭时）zw_prodatt_query_service返回错误
                    //线上打开余额支付也掉不通，可能是策略问题
                    //string ip = fh2.UserIP;
                    //bool isOpen = balaceService.BalancePaidOrNotQuery(fqqid, ip);//查询余额支付功能关闭与否
                    //if (balance <= 200000 && !isOpen)//如余额支付功能已闭且余额小于2000元以下自动调接口打开余额支付的功能
                    //{
                    //    balaceService.OpenBalancePaid(qqid,ip);
                    //}
                    //if (balance > 200000 && !isOpen)//因为该情况不打开余额支付，就不能注销，为了系统性能速度
                    //{
                    //    WebUtils.ShowMessage(this.Page, "余额大于2000元，且余额支付为关闭状态，不能销户！");
                    //    return;
                    //}

                    //下面的流程
                    // 1:判断金额大于阀值,发起一个审批
                    //2:金额小于时,插入logonhistory表,调用接口注销,如果有邮箱,向邮箱发送邮件,反馈信息给一线人员.


                    var ds = qs.GetUserAccount(qqid, 1, 1, 1);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        accountName = ds.Tables[0].Rows[0]["UserRealName2"].ToString();
                        string s_fz_amt = PublicRes.objectToString(ds.Tables[0], "Ffz_amt"); //分账冻结金额
                        string s_cron = PublicRes.objectToString(ds.Tables[0], "Fcon");

                        var freezeMoney = 0L;
                        if (s_fz_amt != "")
                        {
                            freezeMoney += long.Parse(s_fz_amt);
                        }

                        if (s_cron != "")
                        {
                            freezeMoney += long.Parse(s_cron);
                        }

                        if (freezeMoney > 0)
                        {
                            WebUtils.ShowMessage(this.Page, "账户有冻结资金不支持注销！");
                            return;
                        }

                    }


                    DataSet ds1 = qs.GetChildrenInfo(qqid, "1");//主帐户余额
                    if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        balance += long.Parse(ds1.Tables[0].Rows[0]["Fbalance"].ToString().Trim());
                    }
                    ds1 = qs.GetChildrenInfo(qqid, "80");//游戏子帐户
                    if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        balance += long.Parse(ds1.Tables[0].Rows[0]["Fbalance"].ToString().Trim());
                    }
                    ds1 = qs.GetChildrenInfo(qqid, "82");//直通车子帐户
                    if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        balance += long.Parse(ds1.Tables[0].Rows[0]["Fbalance"].ToString().Trim());
                    }
                    #endregion

                    ViewState["Cached" + qqid] = balance;
                    var yuan = MoneyTransfer.FenToYuan(balance);
                    Response.Write("<script type='text/javascript'>window.onload=function(){ confirm(' 该账户余额：" + yuan + "元 \\r\\n 一旦注销成功无法恢复，请确认是否继续注销？') && document.getElementById('btLogOn').click();}</script>");
                    return;
                }

                balance = Convert.ToInt64(ViewState["Cached" + qqid]);
                ViewState["Cached" + qqid] = null;

                #region 注销操作

                string mainID = DateTime.Now.ToString("yyyyMMdd") + qqid;
                string checkType = "logonUser";
                if (balance < 10 * 10 * 200)//系统自动注销
                {
                    if (!qs.LogOnUserDeleteUser(qqid, reason, Label1.Text, "", out Msg))
                    {
                        throw new Exception(Msg);

                    }
                    //系统自动注销成功给用户发邮件
                    if (emailCheck)
                    {
                        SendEmail(emailAddr, qqid, "系统自动销户");
                    }
                    UnRegisterNotify(qqid, accountName, fh.UserIP);
                    WebUtils.ShowMessage(this.Page, "系统自动销户成功！");
                    return;
                }
                else
                {
                    string memo = "[注销QQ号码:" + qqid + "]注销原因:" + reason;
                    Param[] myParams = new Param[4];

                    myParams[0] = new Param();
                    myParams[0].ParamName = "fqqid";
                    myParams[0].ParamValue = qqid;

                    myParams[1] = new Param();
                    myParams[1].ParamName = "Memo";
                    myParams[1].ParamValue = memo;

                    myParams[2] = new Param();
                    myParams[2].ParamName = "returnUrl";
                    myParams[2].ParamValue = "/BaseAccount/InfoCenter.aspx?id=" + qqid;

                    myParams[3] = new Param();
                    myParams[3].ParamName = "fuser";
                    myParams[3].ParamValue = Session["uid"].ToString();

                    cs.StartCheck(mainID, checkType, memo, MoneyTransfer.FenToYuan(balance.ToString()), myParams);
                    WebUtils.ShowMessage(this.Page, "销户申请提请成功！");
                    return;
                }

                #endregion
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                LogError("BaseAccount.logOnUser.protected void btLogOn_Click(object sender, System.EventArgs e) ", "销户操作申请失败,SoapException:", eSoap);
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "销户操作申请失败：" + errStr + "，StackTrace：" + eSoap.StackTrace);
                return;
            }
            catch (Exception err)
            {
                LogError("BaseAccount.logOnUser.protected void btLogOn_Click(object sender, System.EventArgs e) ", "销户操作申请异常:", err);
                //Msg += "销户操作申请异常！" + Common.CommLib.commRes.replaceHtmlStr(err.Message);
                Msg += "销户操作申请异常！" + TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceHtmlStr(err.Message);
                WebUtils.ShowMessage(this.Page, Msg + "，StackTrace：" + err.StackTrace);
                return;
            }           
		}

        /// <summary>
        /// 注销成功通知风控
        /// </summary>
        /// <param name="account"></param>
        /// <param name="accountName"></param>
        /// <param name="executorip"></param>
        private void UnRegisterNotify(string account, string accountName, string executorip)
        {
            IPAddress ipAddr;
            string ip = ConfigurationManager.AppSettings["UnRegisterNotify_IP"].ToString();
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["UnRegisterNotify_Port"].ToString());
            if (!IPAddress.TryParse(ip, out ipAddr))
            {
                LogHelper.LogError("ip地址错误：" + ipAddr.ToString());
            }
            else
            {
                UDPConnection conn = new UDPConnection(ipAddr, port);
                UdpClient udpC = conn.Connection();
                if (udpC == null)
                {
                    LogHelper.LogError("注销财付通通知unregister_notify，无法连接到服务器。");
                }
                else
                {
                    string dataStr = "channel_id=111";
                    dataStr += "&purchaser_id=" + account;
                    dataStr += "&client_ip=" + executorip;
                    dataStr += "&time_stamp=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dataStr += "&imei=";
                    dataStr += "&guid=";
                    dataStr += "&mobile_info=";
                    dataStr += "&QQRiskInfo=";
                    dataStr += "&certno_3des=";
                    dataStr += "&name=" + accountName;
                    dataStr += "&ftype=3";

                    string reqStr = "protocol=unregister_notify&version=1.0&data=" + CommUtil.URLEncode(dataStr);
                    LogHelper.LogInfo("unregister_notify send:" + reqStr);
                    byte[] sendbytes = Encoding.Default.GetBytes(reqStr);
                    udpC.Send(sendbytes, sendbytes.Length);
                }
            }
        }

        private bool SendEmail(string email, string qqid,string subject)
        {
            try
            {
                string str_params = "p_name=" + qqid + "&p_parm1=" + DateTime.Now + "&p_parm2=" + "" + "&p_parm3=" + "" + "&p_parm4=" + "系统自动销户";
                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(email, "2034", str_params);
                return true;
            }
            catch (Exception err)
            {
                LogError("BaseAccount.logOnUser.private bool SendEmail(string email, string qqid,string subject) ", "给用户发邮件出错:", err);
                throw new Exception("给用户发邮件出错："+err.Message);
            }
        }

        protected void btQuery_Click(object sender, System.EventArgs e)
        {
            DateTime bgTime, edTime;
            if (!DateTime.TryParse(this.TextBoxBeginDate.Value, out bgTime))
            {
                //WebUtils.ShowMessage(this.Page, "起始日期格式不正确!默认为1970年1月1日");
                bgTime = new DateTime(DateTime.Now.Year, 1, 1);
                this.TextBoxBeginDate.Value = bgTime.ToString("yyyy-MM-dd");
            }

            if (!DateTime.TryParse(this.TextBoxEndDate.Value, out edTime))
            {
                //WebUtils.ShowMessage(this.Page, "结束日期格式不正确!默认为当天");
                edTime = DateTime.Today;
                this.TextBoxEndDate.Value = edTime.ToString("yyyy-MM-dd");
                
            }

            string qqid = this.TxbQueryQQ.Text.Trim();
            string handid = this.txbHandID.Text.Trim();

            //增加微信相关查询 yinhuang 2014/2/20
            string wx_qq = this.tbWxQQ.Text.Trim();
            string wx_email = this.tbWxEmail.Text.Trim();
            string wx_phone = this.tbWxPhone.Text.Trim();
            string tbWxNo = this.tbWxNo.Text.Trim();
            try
            {
                string queryType = string.Empty;
                string id = string.Empty;
                if (!string.IsNullOrEmpty(wx_qq))
                {
                    queryType = "WeChatQQ";
                    id = wx_qq;
                }
                else if (!string.IsNullOrEmpty(wx_phone))
                {
                    queryType = "WeChatMobile";
                    id = wx_phone;
                }
                else if (!string.IsNullOrEmpty(wx_email))
                {
                    queryType = "WeChatEmail";
                    id = wx_email;
                }
                else if (!string.IsNullOrEmpty(tbWxNo))
                {
                    queryType = "WeChatId";
                    id = tbWxNo;
                }
                if (!string.IsNullOrEmpty(queryType))
                {
                    qqid = AccountService.GetQQID(queryType, id);
                    if (qqid == null || qqid.IndexOf("@") == 0) //返回Null  或者 @wx.tenpay.com 时显示错误 
                    {
                        WebUtils.ShowMessage(this.Page, queryType + " 转换财付通账号失败"); return;
                    }
                }
            }
            catch (Exception err)
            {
                LogError("BaseAccount.logOnUser.protected void btQuery_Click(object sender, System.EventArgs e) ", "查询信息异常:", err);
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }
            BindHistoryInfo(qqid, handid, bgTime, edTime, 0, 1000); //默认查询1000条
        }

		protected void lkHistoryQuery_Click(object sender, System.EventArgs e)
		{
			this.TABLE3.Visible = true;
		}

        protected void CheckBox1_CheckedChanged(object sender, System.EventArgs e)
        {
           
        }

        
	}
}

