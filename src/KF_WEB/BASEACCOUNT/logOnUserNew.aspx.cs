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
using CFT.CSOMS.BLL.FundModule;


namespace TENCENT.OSS.C2C.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// logOnUserNew 的摘要说明。
    /// </summary>
    public partial class logOnUserNew : System.Web.UI.Page
    {
        protected BalaceService balaceService = new BalaceService();
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                if (!ClassLib.ValidateRight("CancelAccount", this))
                    Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if (!Page.IsPostBack)
            {
                string msg = "";
                try
                {
                    DataSet ds = new AccountOperate().GetCanncelAccountLog("", "", DateTime.Parse("1970-01-01 00:00:00"), DateTime.Now, 0, 10, out msg);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        this.dgInfo.DataSource = ds;
                        this.dgInfo.DataBind();
                    }
                    else
                    {
                        this.dgInfo.DataSource = null;
                        this.dgInfo.DataBind();
                    }
                }
                catch (Exception ex)
                {

                }
                this.TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
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

        /// <summary>
        /// 提交销户申请
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btLogOn_Click(object sender, System.EventArgs e)  //暂不开放注销功能
        {
            string qqid = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceSqlStr(this.TextBox1_QQID.Text);
            string wxid = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceSqlStr(this.TextBox2_WX.Text);
            string reason = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceSqlStr(this.txtReason.Text);
            bool emailCheck = EmailCheckBox.Checked;
            string emailAddr = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceSqlStr(this.txtEmail.Text);

            int wxFlag = 0;//是否是微信账号
            if (string.IsNullOrEmpty(TextBox1_QQID.Text) && string.IsNullOrEmpty(TextBox2_WX.Text))
            {
                ValidateID.ForeColor = Color.Red;
                ValidateID.Text = "请输入手Q账号或下方微信号";
                return;
            }
            if (!string.IsNullOrEmpty(TextBox1_QQID.Text) && !string.IsNullOrEmpty(TextBox2_WX.Text))
            {
                ValidateID.ForeColor = Color.Red;
                ValidateID.Text = "不能同时输入手Q账号和微信账号!!!";
                return;
            }
            string wxUIN = "";
            string wxHBUIN = "";
            if (string.IsNullOrEmpty(TextBox1_QQID.Text))
            {
                if (TextBox2_WX.Text != txbConfirmQ.Text)
                {
                    ValidateID.ForeColor = Color.Red;
                    ValidateID.Text = "两次输入的账号不相同，请重新输入!!!";
                    return;
                }
                wxFlag = 1;
                wxUIN = WeChatHelper.GetUINFromWeChatName(TextBox2_WX.Text);
                wxHBUIN = WeChatHelper.GetHBUINFromWeChatName(TextBox2_WX.Text);
                qqid = wxUIN;
            }
            else if (TextBox1_QQID.Text != txbConfirmQ.Text)
            {
                ValidateID.ForeColor = Color.Red;
                ValidateID.Text = "两次输入的账号不相同，请重新输入!!!";
                return;
            }

            string memo = "[注销QQ号码:" + qqid + "]注销原因:" + reason;
            string Msg = "";

            try
            {
                Finance_Manage fm = new Finance_Manage();
                Check_Service cs = new Check_Service();
                TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Finance_Header fh = setConfig.setFH_CheckService(this);
                cs.Finance_HeaderValue = fh;

                Param[] myParams = new Param[4];

                string fqqid = this.txbConfirmQ.Text.Trim();
                if (wxFlag == 1)
                {
                    fqqid = wxUIN;
                }
                myParams[0] = new Param();
                myParams[0].ParamName = "fqqid";
                myParams[0].ParamValue = fqqid;

                myParams[1] = new Param();
                myParams[1].ParamName = "Memo";
                myParams[1].ParamValue = memo;

                myParams[2] = new Param();
                myParams[2].ParamName = "returnUrl";
                myParams[2].ParamValue = "/BaseAccount/InfoCenter.aspx?id=" + qqid;

                myParams[3] = new Param();
                myParams[3].ParamName = "fuser";
                myParams[3].ParamValue = Session["uid"].ToString();

                string mainID = DateTime.Now.ToString("yyyyMMdd") + qqid;
                string checkType = "logonUser";

                Query_Service qs = new Query_Service();
                TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Finance_Header fh2 = setConfig.setFH(this);
                qs.Finance_HeaderValue = fh2;

                string ret_msg = "";
       
                string ip = "";
                //TODO:申请销户提起的部分还没做
                bool isOk = new AccountOperate().LogOnUserDeleteUser(qqid, wxFlag, reason, emailCheck, emailAddr, Session["uid"].ToString(), ip, out ret_msg);

                if (!string.IsNullOrEmpty(ret_msg))
                {
                    WebUtils.ShowMessage(this.Page, ret_msg);
                }               
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "销户操作申请失败：" + errStr);
                return;
            }
            catch (Exception err)
            {
                //Msg += "销户操作申请异常！" + Common.CommLib.commRes.replaceHtmlStr(err.Message);
                Msg += "销户操作申请异常！" + TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceHtmlStr(err.Message);
                WebUtils.ShowMessage(this.Page, Msg);
                return;
            }

            this.btLogOn.Enabled = false;
        }

        protected void btQuery_Click(object sender, System.EventArgs e)
        {
            DateTime begin_time;
            DateTime end_time;

            try
            {
                begin_time = DateTime.Parse(this.TextBoxBeginDate.Text);
            }
            catch
            {
                WebUtils.ShowMessage(this.Page, "起始日期格式不正确!默认为1970年1月1日");
                this.TextBoxBeginDate.Text = "1970-01-01";
                begin_time = DateTime.Parse("1970-01-01 00:00:00");
            }

            try
            {
                end_time = DateTime.Parse(this.TextBoxEndDate.Text);
                end_time.AddDays(1);
            }
            catch
            {
                WebUtils.ShowMessage(this.Page, "结束日期格式不正确!默认为当天");
                this.TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                end_time = DateTime.Now;
            }

            string qqid = this.TxbQueryQQ.Text.Trim();
            string handid = this.txbHandID.Text.Trim();

            //增加微信相关查询 yinhuang 2014/2/20
            string wx_qq = this.tbWxQQ.Text.Trim();
            string wx_email = this.tbWxEmail.Text.Trim();
            string wx_phone = this.tbWxPhone.Text.Trim();

            try
            {
                string queryType = "";
                string id = "";
                if (!string.IsNullOrEmpty(wx_qq))
                {
                    queryType = "QQ";
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

                if (!string.IsNullOrEmpty(queryType))
                {
                    qqid = AccountService.GetQQID(queryType, id);
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);
                return;
            }

            try
            {
                string msg = "";
                DataSet ds = new AccountOperate().GetCanncelAccountLog(qqid, handid, begin_time, end_time, 0, 1000, out msg);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    this.dgInfo.DataSource = ds;
                    this.dgInfo.DataBind();
                }
                else
                {
                    this.dgInfo.DataSource = null;
                    this.dgInfo.DataBind();
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);
                return;
            }

        }

        protected void lkHistoryQuery_Click(object sender, System.EventArgs e)
        {
            this.TABLE3.Visible = true;
        }

    }
}

