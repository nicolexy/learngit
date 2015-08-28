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
using System.Web.Services;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Configuration;
using CFT.CSOMS.BLL.BalanceModule;
using CFT.CSOMS.BLL.CFTAccountModule;
using CFT.CSOMS.COMMLIB;
using CFT.Apollo.Logging;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    public partial class InfoCenterNew : System.Web.UI.Page
    {
        public string iFramePath;  //设置iFrame的路径
        public string iFrameHeight;  //设置iFrame(用户交易记录)显示区域的高度
        public string iFrameBank;
        protected System.Web.UI.WebControls.ImageButton ImageButton3;

        protected BalaceService balaceService = new BalaceService();

        private static bool tradeUpOrDown;
        string uid;

        protected void Page_Load(object sender, System.EventArgs e)
        {

            this.LkBT_Refund.Visible = false;     //3.0暂时不提供该功能
            this.LkBT_Refund_Sale.Visible = false;   //3.0暂时不提供该功能

            if (!IsPostBack)
            {
                this.LinkButton3.Attributes["onClick"] = "if(!confirm('确定要执行该操作吗？')) return false;";
                this.btnDelClass.Attributes["onClick"] = "if(!confirm('确定要执行该操作吗？')) return false;";
                CheckInput();

                try
                {
                    uid = Session["uid"].ToString();
                    this.Label_uid.Text = uid;
                    string szkey = Session["szkey"].ToString();
                    if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
                }
                catch  //如果没有登陆或者没有权限就跳出
                {
                    Response.Redirect("../login.aspx?wh=1");
                }

                try  //处理从QQ查询得到listID的跳转处理
                {
                    string id = Request.QueryString["id"].ToString();
                    this.TextBox1_InputQQ.Text = id.Trim(); //自动绑定传递过来的ID
                    clickEvent();
                }

                catch //如果没有参数处理，进入正常页面处理
                {
                    setLableText_Null();
                    iFrameHeight = "0";
                    tradeUpOrDown = true;
                }
            }
        }

        private void CheckInput()
        {
            Button1.Attributes.Add("onclick", "return checkvlid(this);");
        }

        private void SetButtonVisible()
        {
            string szkey = Session["SzKey"].ToString();
            if (!classLibrary.ClassLib.ValidateRight("FreezeUser", this))
                this.btnDelClass.Visible = false;
            else
                this.btnDelClass.Visible = true;

            if (LinkButton3.Text == "冻结")
            {
                bool userright = classLibrary.ClassLib.ValidateRight("FreezeUser", this);
                if (!userright)
                    LinkButton3.Visible = false;
            }

            else if (LinkButton3.Text == "解冻")
            {
                bool userright = classLibrary.ClassLib.ValidateRight("UnFreezeUser", this);
                if (!userright) LinkButton3.Visible = false;
            }
        }

        private void setLableText_Null()
        {
            this.Label1_Acc.Text = "";
            this.Label2_Type.Text = "";
            this.Label3_LeftAcc.Text = "";
            this.Label4_Freeze.Text = "";
            this.Label5_YestodayLeft.Text = "";
            this.Label6_LastModify.Text = "";
            this.Label7_SingleMax.Text = "";
            this.Label8_PerDayLmt.Text = "";
            this.Label9_LastSaveDate.Text = "";
            this.Label10_Drawing.Text = "";
            this.Label11_Remark.Text = "";
            this.Label12_Fstate.Text = "";
            this.Label13_Fuser_type.Text = "";
            this.Label14_Ftruename.Text = "";
            this.Label15_Useable.Text = "";
            this.Label16_Fapay.Text = "";
            this.Label17_Flogin_ip.Text = "";
            this.Label18_Attid.Text = "";
            this.labEmail.Text = "";
            this.labMobile.Text = "";
            this.labQQstate.Text = "";
            this.labEmailState.Text = "";
            this.labMobileState.Text = "";
            this.lblLoginTime.Text = "";
            this.Label19_OpenOrNot.Text = "";
        }

        private string CheckBasicInfo(int nAttid)
        {
            //从数据字典中读取数据，绑定到web页
            DataSet ds = PermitPara.QueryDicAccName();
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return "";
            }
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                if (nAttid == int.Parse(dr["Value"].ToString().Trim()))
                {
                    return dr["Text"].ToString().Trim();
                }
            }
            return "";
        }

        private void setIframePath()
        {
            iFramePath = "TradeLog.aspx";
        }

        private void clickEvent()
        {
            if (Session["uid"] == null)
                Response.Redirect("../login.aspx?wh=1"); //重新登陆

            try
            {
                //初始化导航按钮
                this.LKBT_TradeLog.ForeColor = Color.Red;
                this.LKBT_TradeLog_Sale.ForeColor = Color.Black;
                this.LKBT_TradeLog_Unfinished.ForeColor = Color.Black;
                this.LKBT_TradeLog_Sale_Unfinished.ForeColor = Color.Black;
                this.LkBT_PaymentLog.ForeColor = Color.Black;
                this.LKBT_bankrollLog.ForeColor = Color.Black;
                this.LKBT_GatheringLog.ForeColor = Color.Black;
                this.LkBT_PaymentLog.ForeColor = Color.Black;
                this.LkBT_Refund.ForeColor = Color.Black;
                this.LkBT_Refund_Sale.ForeColor = Color.Black;

                if (string.IsNullOrEmpty(this.TextBox1_InputQQ.Text))
                {
                    WebUtils.ShowMessage(this.Page, "请输入账号!");
                    return;
                }

                string qqid = TextBox1_InputQQ.Text;
                Session["QQID"] = qqid;
                //账号类型:1,C账号;2,内部账号
                if (this.InternalID.Checked)
                {
                    Session["QQID"] = new AccountService().Uid2QQ(qqid);
                }

                GetUserInfo();
                iFrameHeight = "230";   //iFame显示区域的高度          
                ViewState["iswechat"] = "false";
                //setIframePath();        //设置路径	

                this.LKBT_TradeLog.Visible = true;
                this.LKBT_TradeLog_Sale.Visible = true;
                this.LkBT_PaymentLog.Visible = true;
                this.LKBT_bankrollLog.Visible = true;
                this.LKBT_GatheringLog.Visible = true;
                this.LkBT_PaymentLog.Visible = true;
                this.LkBT_Refund.Visible = true;
                this.LkBT_Refund_Sale.Visible = true;
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "查询错误：" + eSys.Message.ToString());
            }
        }

        private void GetUserInfo()
        {
            if (string.IsNullOrEmpty(this.TextBox1_InputQQ.Text))
            {
                WebUtils.ShowMessage(this.Page, "请输入账号!");
                return;
            }

            string qqid = TextBox1_InputQQ.Text;
            string type = "Uin";   //账号类型:1,C账号;2,内部账号
            if (this.InternalID.Checked)
            {
                type = "Uid";
            }

            try
            {
                //账户个人信息
                DataSet ds = new AccountService().GetPersonalInfo(qqid, type);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    this.Label1_Acc.Text = ds.Tables[0].Rows[0]["Fqqid"].ToString();
                    this.labQQstate.Text = ds.Tables[0].Rows[0]["Fqqid_state"].ToString();
                    this.labEmail.Text = ds.Tables[0].Rows[0]["Femail"].ToString();
                    this.labEmailState.Text = ds.Tables[0].Rows[0]["Femial_state"].ToString();
                    this.labMobile.Text = ds.Tables[0].Rows[0]["Fmobile"].ToString();
                    this.labMobileState.Text = ds.Tables[0].Rows[0]["Fmobile_state"].ToString();
                    this.Label12_Fstate.Text = ds.Tables[0].Rows[0]["Fstate_str"].ToString();
                    this.Label15_Useable.Text = ds.Tables[0].Rows[0]["Fuseable_fee"].ToString();
                    this.Label5_YestodayLeft.Text = ds.Tables[0].Rows[0]["Fyday_balance"].ToString();
                    this.Label2_Type.Text = ds.Tables[0].Rows[0]["Fcurtype_str"].ToString();

                    this.Label16_Fapay.Text = ds.Tables[0].Rows[0]["Fapay"].ToString();
                    this.Label8_PerDayLmt.Text = ds.Tables[0].Rows[0]["Fquota_pay_str"].ToString();
                    this.lbSave.Text = ds.Tables[0].Rows[0]["Fsave_str"].ToString();
                    this.Label10_Drawing.Text = ds.Tables[0].Rows[0]["Ffetch_time"].ToString();
                    this.Label6_LastModify.Text = ds.Tables[0].Rows[0]["Fmodify_time"].ToString();
                    this.Label11_Remark.Text = ds.Tables[0].Rows[0]["Fmemo"].ToString();
                    this.Label14_Ftruename.Text = ds.Tables[0].Rows[0]["Fname_str"].ToString();
                    this.lbInnerID.Text = ds.Tables[0].Rows[0]["Fuid"].ToString();
                    this.lbLeftPay.Text = ds.Tables[0].Rows[0]["Fbpay_state_str"].ToString();
                    this.Label13_Fuser_type.Text = ds.Tables[0].Rows[0]["Fuser_type_str"].ToString();

                    this.Label4_Freeze.Text = ds.Tables[0].Rows[0]["Ffreeze_fee"].ToString();
                    this.Label3_LeftAcc.Text = ds.Tables[0].Rows[0]["Fbalance_str"].ToString();
                    this.lblLoginTime.Text = ds.Tables[0].Rows[0]["Fcreate_time"].ToString();
                    this.Label7_SingleMax.Text = ds.Tables[0].Rows[0]["Fquota_str"].ToString();
                    this.lbFetchMoney.Text = ds.Tables[0].Rows[0]["Ffetch_str"].ToString();
                    this.Label9_LastSaveDate.Text = ds.Tables[0].Rows[0]["Fsave_time"].ToString();
                    this.Label17_Flogin_ip.Text = ds.Tables[0].Rows[0]["Flogin_ip"].ToString();
                    this.Label18_Attid.Text = ds.Tables[0].Rows[0]["Fpro_att"].ToString();
                }
            }
            catch
            { }

            //查询余额支付功能关闭与否
            try
            {
                string ip = "127.0.0.1";
                bool isOpen = new BalaceService().BalancePaidOrNotQuery(Session["QQID"].ToString(), ip);
                if (isOpen)
                    this.Label19_OpenOrNot.Text = "打开";
                else
                    this.Label19_OpenOrNot.Text = "已关闭";
            }
            catch
            { }

            //VIP信息
            try
            {
                DataTable dt = new VIPService().QueryVipInfo(Session["QQID"].ToString().Trim());
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.vip_value.Text = dt.Rows[0]["value"].ToString();
                    this.vip_flag.Text = dt.Rows[0]["vipflag_str"].ToString();
                    this.vip_level.Text = dt.Rows[0]["level"].ToString();
                    this.vip_channel.Text = dt.Rows[0]["subid_str"].ToString();
                    this.vip_exp_date.Text = dt.Rows[0]["exp_date"].ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("出现异常：" + ex.Message);
                LogHelper.LogInfo("异常堆栈信息：" + new System.Diagnostics.StackTrace().GetFrames().ToString());
                LogHelper.LogInfo("异常方法信息：" + ex.TargetSite.ToString());
                LogHelper.LogInfo("异常对象信息：" + ex.Source.ToString());
            }

            //实名认证状态
            try
            {
                string msg = "";
                int state = new AuthenInfoService().GetUserClassInfo(Session["QQID"].ToString(), out msg);
                labUserClassInfo.Text = msg;
            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("出现异常：" + ex.Message);
                LogHelper.LogInfo("异常堆栈信息：" + new System.Diagnostics.StackTrace().GetFrames().ToString());
                LogHelper.LogInfo("异常方法信息：" + ex.TargetSite.ToString());
                LogHelper.LogInfo("异常对象信息：" + ex.Source.ToString());
            }

            try
            {
                //增加删除认证的操作日志
                DataSet dsdgList = new AuthenInfoService().GetUserClassDeleteList(this.TextBox1_InputQQ.Text.Trim());
                if (dsdgList != null && dsdgList.Tables.Count > 0 && dsdgList.Tables[0].Rows.Count > 0)
                {
                    this.dgList.Visible = true;
                    this.dgList.DataSource = dsdgList.Tables[0].DefaultView;
                    this.DataBind();
                }
                else
                    this.dgList.Visible = false;
            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("出现异常：" + ex.Message);
                LogHelper.LogInfo("异常堆栈信息：" + new System.Diagnostics.StackTrace().GetFrames().ToString());
                LogHelper.LogInfo("异常方法信息：" + ex.TargetSite.ToString());
                LogHelper.LogInfo("异常对象信息：" + ex.Source.ToString());
            }

        }

        private void SetFrame(string path, int height, LinkButton activeButton)
        {
            iFramePath = path;
            iFrameHeight = height.ToString();
            this.LKBT_TradeLog.ForeColor = Color.Black;
            this.LKBT_TradeLog_Sale.ForeColor = Color.Black;
            this.LKBT_TradeLog_Unfinished.ForeColor = Color.Black;
            this.LKBT_TradeLog_Sale_Unfinished.ForeColor = Color.Black;
            this.LkBT_PaymentLog.ForeColor = Color.Black;
            this.LKBT_bankrollLog.ForeColor = Color.Black;
            this.LKBT_GatheringLog.ForeColor = Color.Black;
            this.LkBT_PaymentLog.ForeColor = Color.Black;
            this.LkBT_Refund.ForeColor = Color.Black;
            this.LkBT_Refund_Sale.ForeColor = Color.Black;
            this.LkBT_ButtonInfo.ForeColor = Color.Black;
            this.LkBT_Gwq.ForeColor = Color.Black;
            activeButton.ForeColor = Color.Red;
        }

        public void SyncUserNameClick(object sender, System.EventArgs e)
        {
            //同步姓名
            try
            {
                //支持AA同步姓名
                string account = labEmail.Text;
                if (string.IsNullOrEmpty(account))
                {
                    throw new Exception("账号为空！");
                }
                if (!(account.Contains("@aa.tenpay.com")))
                {
                    throw new Exception("只支持AA账号同步姓名！");
                }

                //1先通过openid查微信财付通账号
                string input_qq = TextBox1_InputQQ.Text.Trim(); //通过输入的值来查微信支付账号
                if (input_qq.IndexOf("@aa.tenpay.com") == -1)
                {
                    throw new Exception("只支持AA账号同步姓名！");
                }
                string openid = input_qq.Substring(0, input_qq.IndexOf("@aa.tenpay.com"));//得到xxxx@wx.tenpay.com
                string uin = WeChatHelper.GetAcctIdFromAAOpenId(openid);
                if (string.IsNullOrEmpty(uin))
                {
                    throw new Exception("转换成微信财付通账号失败：" + openid);
                }
                uin += "@wx.tenpay.com";

                //2通过uin查绑定姓名GetBankCardBindList_2
                string true_name = "";
                DataSet ds = new AccountService().GetUserAccount(uin, 1, 1, 1);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    true_name = ds.Tables[0].Rows[0]["Ftruename"].ToString();
                }
                else
                {
                    throw new Exception("没有记录！" + uin);
                }

                //3调接口同步姓名aac_syncusername_service
                string ret = new AccountService().SynUserName(labEmail.Text, Label14_Ftruename.Text, true_name, uin);
                if (ret == "0")
                {
                    WebUtils.ShowMessage(this.Page, "同步成功");
                }
                else
                {
                    throw new Exception(ret);
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "同步姓名错误：" + ex.Message);
            }
        }

        protected void LKBT_TradeLog_Click(object sender, System.EventArgs e)
        {
            if (Session["uid"] == null)
            {
                WebUtils.ShowMessage(this.Page, "超时，请重新登陆！");
            }
            else
            {
                SetFrame("TradeLog.aspx?user=buy", 230, this.LKBT_TradeLog);
            }
        }

        protected void LKBT_TradeLog_Sale_Click(object sender, System.EventArgs e)
        {
            if (Session["uid"] == null)
            {
                WebUtils.ShowMessage(this.Page, "超时，请重新登陆！");
            }
            else
            {
                SetFrame("TradeLog.aspx?user=sale", 230, this.LKBT_TradeLog_Sale);
            }
        }

        protected void LKBT_TradeLog_Unfinished_Click(object sender, System.EventArgs e)
        {
            if (Session["uid"] == null)
            {
                WebUtils.ShowMessage(this.Page, "超时，请重新登陆！");
            }
            else
            {
                SetFrame("TradeLog.aspx?user=Unfinished", 230, this.LKBT_TradeLog_Unfinished);
            }
        }

        protected void LKBT_TradeLog_Sale_Unfinished_Click(object sender, System.EventArgs e)
        {
            if (Session["uid"] == null)
            {
                WebUtils.ShowMessage(this.Page, "超时，请重新登陆！");
            }
            else
            {
                SetFrame("TradeLog.aspx?user=SaleUnfinished", 230, this.LKBT_TradeLog_Sale_Unfinished);
            }
        }

        protected void LKBT_bankrollLog_Click(object sender, System.EventArgs e)
        {
            if (Session["uid"] == null)
            {
                WebUtils.ShowMessage(this.Page, "超时，请重新登陆！");
            }
            else
            {
                SetFrame("bankrollLog.aspx?type=QQID", 230, this.LKBT_bankrollLog);
            }
        }

        protected void LKBT_GatheringLog_Click(object sender, System.EventArgs e)
        {
            if (Session["uid"] == null)
            {
                WebUtils.ShowMessage(this.Page, "超时，请重新登陆！");
            }
            else
            {
                string isHistory = CheckBox1.Checked.ToString();
                SetFrame("GatherLog.aspx?type=QQID&history=" + isHistory, 230, this.LKBT_GatheringLog);
            }
        }

        protected void LkBT_PaymentLog_Click(object sender, System.EventArgs e)
        {
            if (Session["uid"] == null)
            {
                WebUtils.ShowMessage(this.Page, "超时，请重新登陆！");
            }
            else
            {
                SetFrame("PaymentLogNew.aspx?type=QQID", 230, this.LkBT_PaymentLog);
            }
        }

        private void Submit1_ServerClick(object sender, System.EventArgs e)
        {
            iFrameHeight = "230";   //iFame显示区域的高度
            setIframePath();        //设置路径
        }

        protected void Button1_Click(object sender, System.EventArgs e)
        {
            string strszkey = Session["SzKey"].ToString().Trim();
            int ioperid = Int32.Parse(Session["OperID"].ToString());
            int iserviceid = Common.AllUserRight.GetServiceID("InfoCenter");
            string struserdata = Session["uid"].ToString().Trim();
            string content = struserdata + "执行了[个人账户查询]操作,操作对象[" + this.TextBox1_InputQQ.Text.Trim()
                + "]时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Common.AllUserRight.UpdateSession(strszkey, ioperid, PublicRes.GROUPID, iserviceid, struserdata, content);
            string log = SensitivePowerOperaLib.MakeLog("get", struserdata, "[个人账户查询]", this.TextBox1_InputQQ.Text.Trim(), "1", "1");
            SensitivePowerOperaLib.WriteOperationRecord("InfoCenter", log, this);

            clickEvent();
        }

        private void LinkButton1_Click(object sender, System.EventArgs e)
        {
            try
            {
                if ((this.Label1_Acc.Text != "") && (this.Label3_LeftAcc.Text.Trim() != "0"))
                {
                    Response.Redirect("LeftAccountFreeze.aspx?id=true&qq=" + Session["QQID"].ToString().Trim() + "&moy=" + this.Label15_Useable.Text.Trim());
                    iFrameHeight = "230";   //iFame显示区域的高度
                    setIframePath();        //设置路径
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "非法访问！");
                }
            }
            catch (Exception)
            {
                WebUtils.ShowMessage(this.Page, "冻结失败。请重试或者联系管理员");
            }
        }

        private void LinkButton2_Click(object sender, System.EventArgs e)
        {
            try
            {
                if ((this.Label1_Acc.Text != "") && (this.Label4_Freeze.Text.Trim() != "0"))
                {
                    Response.Redirect("LeftAccountFreeze.aspx?id=false&qq=" + Session["QQID"].ToString().Trim() + "&moy=" + this.Label4_Freeze.Text.Trim()); //参数：1 解冻 2 QQID 3 冻结金额
                    iFrameHeight = "230";   //iFame显示区域的高度
                    setIframePath();        //设置路径
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "非法访问！");
                }
            }
            catch (Exception emsg)
            {
                classLibrary.setConfig.exceptionMessage(emsg.Message);
            }
        }

        private void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            iFrameHeight = "0";
            setIframePath();
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
                iFrameHeight = "230";
                setIframePath();
                tradeUpOrDown = false;
            }
        }

        protected void LkBT_Refund_Click(object sender, System.EventArgs e)
        {
            if (Session["uid"] == null)
            {
                WebUtils.ShowMessage(this.Page, "超时，请重新登陆！");
            }
            else
            {
                SetFrame("RefundNew.aspx?type=buy", 230, this.LkBT_Refund);
            }
        }

        protected void LinkButton3_Click(object sender, System.EventArgs e)
        {
            if (Label12_Fstate.Text.Trim() == "正常")
            {
                Response.Write("<script language=javascript>window.location='freezeBankAcc.aspx?id=true&uid=" + Session["QQID"].ToString().Trim() + "&iswechat=" + ViewState["iswechat"].ToString() + "&type=per'</script>");
            }
            else if (Label12_Fstate.Text.Trim() == "冻结")
            {
                Response.Write("<script language=javascript>window.location='freezeBankAcc.aspx?id=false&uid=" + Session["QQID"].ToString().Trim() + "&iswechat=" + ViewState["iswechat"].ToString() + "&type=per'</script>");
            }
        }

        protected void LkBT_ButtonInfo_Click(object sender, System.EventArgs e)
        {
            if (Session["uid"] == null)
            {
                WebUtils.ShowMessage(this.Page, "超时，请重新登陆！");
            }
            else
            {
                SetFrame("ButtonInfo.aspx", 230, this.LkBT_ButtonInfo);
            }
        }

        protected void LkBT_Gwq_Click(object sender, System.EventArgs e)
        {
            if (Session["uid"] == null)
            {
                WebUtils.ShowMessage(this.Page, "超时，请重新登陆！");
            }
            else
            {
                SetFrame("GwqByQQ.aspx", 230, this.LkBT_Gwq);
            }
        }

        protected void LkBT_Refund_Sale_Click(object sender, System.EventArgs e)
        {
            if (Session["uid"] == null)
            {
                WebUtils.ShowMessage(this.Page, "超时，请重新登陆！");
            }
            else
            {
                SetFrame("RefundNew.aspx?type=sale", 230, this.LkBT_Refund_Sale);
            }
        }

        protected void btnDelClass_Click(object sender, System.EventArgs e)
        {
            //删除认证。
            try
            {
                if (Session["QQID"] == null || Session["QQID"].ToString().Trim() == "")
                    return;

                string msg = "";
                string qqid = Session["QQID"].ToString().Trim();
                string opera = this.Label_uid.Text;
                if (new AuthenInfoService().DelAuthen(qqid, opera, out msg))
                {
                    WebUtils.ShowMessage(this.Page, "执行成功！");
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "执行失败！" + classLibrary.setConfig.replaceMStr(msg));
                }
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, "调用Service失败！" + classLibrary.setConfig.replaceMStr(err.Message));
            }
        }

        public void dgList_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            this.dgList.CurrentPageIndex = e.NewPageIndex;
            GetUserInfo();
        }

        protected void LkBT_mediOrder_Click(object sender, EventArgs e)
        {
            if (Session["uid"] == null)
            {
                WebUtils.ShowMessage(this.Page, "超时，请重新登陆！");
            }
            else
            {
                SetFrame("TradeLog.aspx?user=mediOrder", 230, this.LkBT_mediOrder);
            }
        }
    }

}

