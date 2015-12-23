using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Services.Protocols;
using System.Web.UI.WebControls;
using System.Xml;
using CFT.CSOMS.BLL.CFTAccountModule;
using CFT.CSOMS.COMMLIB;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using CFT.CSOMS.BLL.WechatPay;
using CFT.CSOMS.BLL.TransferMeaning;

namespace TENCENT.OSS.CFT.KF.KF_Web.WebchatPay
{
    public partial class WechatInfoQuery : System.Web.UI.Page
    {
        public string iFramePath;  //设置iFrame的路径
        public string iFrameHeight;  //设置iFrame(用户交易记录)显示区域的高度
        public string iFrameBank;
        protected System.Web.UI.WebControls.ImageButton ImageButton3;

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
            this.lb_wxName.Text = "";
            this.labMobile.Text = "";
            this.labQQstate.Text = "";
            this.labEmailState.Text = "";
            this.labMobileState.Text = "";
            this.lblLoginTime.Text = "";
            this.CKV_freeze.Text = "";
            this.CKV_WXRemainder.Text = "";

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

        private void BindData(int istr, int imax)
        {
            Query_Service.Query_Service myService = new Query_Service.Query_Service();
            myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
            DataSet ds = new DataSet();
            //判断是用微信查还是账号查 yinhuang 2013/7/30
            bool isWechat = true;
            if (this.WeChatId.Checked || this.WeChatQQ.Checked || this.WeChatMobile.Checked || this.WeChatEmail.Checked || this.WeChatCft.Checked || this.WeChatUid.Checked)
            {
                //使用微信查询
                isWechat = true;
                ds = myService.GetUserAccountFromWechat(Session["QQID"].ToString(), istr, imax);              
            }
            else { 
              //使用账号查询
                isWechat = false;            
                throw new Exception("请选择查询条件");
            }
                      
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                //也有可能是快速交易用户
                //furion 新加一个函数，判断是否为快速交易用户，fsign=2并且无t_user表。
                if (myService.IsFastPayUser(Session["QQID"].ToString()))
                {
                    this.Label14_Ftruename.Text = "快速交易用户";
                    this.Label12_Fstate.Text = "";
                }
                else
                    throw new Exception("数据库无此记录");
            }
            else
            {//2013/7/19 yinhuang 兼容不存在的列 PublicRes.objectToString
                //this.Label1_Acc.Text			= this.TextBox1_InputQQ.Text.Trim();  //"22000254";
                this.Label1_Acc.Text = PublicRes.objectToString(ds.Tables[0], "Fqqid");
                string s_curtype = PublicRes.objectToString(ds.Tables[0], "Fcurtype");
                if (s_curtype == "")
                {
                    this.Label2_Type.Text = "";
                }
                else {
                    this.Label2_Type.Text = Transfer.convertMoney_type(s_curtype);//tu.u_CurType;				   //"代金券";
                }

                this.Label3_LeftAcc.Text = classLibrary.setConfig.FenToYuan(PublicRes.objectToString(ds.Tables[0],"Fbalance"));//tu.u_Balance;				   //"3000";
                //this.Label4_Freeze.Text = classLibrary.setConfig.FenToYuan(PublicRes.objectToString(ds.Tables[0],"Fcon"));                  //"1000";
                this.Label5_YestodayLeft.Text = PublicRes.objectToString(ds.Tables[0],"Fyday_balance");		   //"10";
                this.lblLoginTime.Text = PublicRes.objectToString(ds.Tables[0],"Fcreate_time");
                this.Label6_LastModify.Text = PublicRes.objectToString(ds.Tables[0],"Fmodify_time");		   //"2005-05-01";
                this.Label7_SingleMax.Text = classLibrary.setConfig.FenToYuan(PublicRes.objectToString(ds.Tables[0],"Fquota"));		   //"2000";
                this.Label8_PerDayLmt.Text = classLibrary.setConfig.FenToYuan(PublicRes.objectToString(ds.Tables[0],"Fquota_pay"));			//"5000";
                this.Label9_LastSaveDate.Text = PublicRes.objectToString(ds.Tables[0],"Fsave_time");				//"2005-03-01";
                this.Label10_Drawing.Text = PublicRes.objectToString(ds.Tables[0],"Ffetch_time");              //"2005-04-15";
                this.Label11_Remark.Text = PublicRes.objectToString(ds.Tables[0],"Fmemo");					//"这个家伙很懒，什么都没有留下！";
                
                //微信查询的state判断有所不同 yinhuang
                if (isWechat)
                {
                    string str_state = PublicRes.objectToString(ds.Tables[0],"Fstate");
                    if (str_state == "1") {
                        this.Label12_Fstate.Text = "正常";
                    }
                    else if (str_state == "2")
                    {
                        this.Label12_Fstate.Text = "冻结";
                    }
                    else {
                        this.Label12_Fstate.Text = "";
                    }
                }
                else {
                    this.Label12_Fstate.Text = Transfer.accountState(PublicRes.objectToString(ds.Tables[0], "Fstate"));
                }
                this.Label13_Fuser_type.Text = Transfer.convertFuser_type(PublicRes.objectToString(ds.Tables[0], "Fuser_type"));
           
                //将上面取姓名的逻辑修改 yinhuang 2013/7/30
                string str_truename = PublicRes.objectToString(ds.Tables[0],"UserRealName2");
                if (str_truename == "") {
                    str_truename = PublicRes.objectToString(ds.Tables[0],"Ftruename");
                }
                this.Label14_Ftruename.Text = str_truename;

                string s_fz_amt = PublicRes.objectToString(ds.Tables[0], "Ffz_amt"); //分账冻结金额
                string s_balance = PublicRes.objectToString(ds.Tables[0],"Fbalance");
                string s_cron = PublicRes.objectToString(ds.Tables[0],"Fcon");
                long l_balance = 0,l_cron=0,l_fzamt=0;
                if (s_balance != "") {
                    l_balance = long.Parse(s_balance);
                }
                if (s_cron != "")
                {
                    l_cron = long.Parse(s_cron);
                }
                if (s_fz_amt != "")
                {
                    l_fzamt = long.Parse(s_fz_amt);
                }

                this.Label4_Freeze.Text = TENCENT.OSS.CFT.KF.Common.MoneyTransfer.FenToYuan(l_fzamt + l_cron).ToString("f2") + "元";   //classLibrary.setConfig.FenToYuan(l_fzamt + l_cron);//冻结金额=分账冻结金额+冻结金额
                //this.Label15_Useable.Text = classLibrary.setConfig.FenToYuan((long.Parse(ds.Tables[0].Rows[0]["Fbalance"].ToString()) - long.Parse(ds.Tables[0].Rows[0]["Fcon"].ToString())).ToString());  //帐户余额减去冻结余额= 可用余额
                this.Label15_Useable.Text = classLibrary.setConfig.FenToYuan(l_balance - l_cron);  //帐户余额减去冻结余额= 可用余额
                this.Label16_Fapay.Text = PublicRes.objectToString(ds.Tables[0],"Fapay");
                this.Label17_Flogin_ip.Text = PublicRes.objectToString(ds.Tables[0],"Flogin_ip");

                //furion 20061116 email登录修改
                this.labEmail.Text = PublicRes.GetString(PublicRes.objectToString(ds.Tables[0],"Femail"));
                this.labMobile.Text = PublicRes.GetString(PublicRes.objectToString(ds.Tables[0],"Fmobile"));
                //2006-10-18 edwinyang 增加产品属性
                int nAttid = 0;
                //				pbp.BindDropDownList(pm.QueryDicAccName(),ddlAttid,out Msg);
                string s_attid = PublicRes.objectToString(ds.Tables[0],"Att_id");
                if (s_attid != "") {
                    nAttid = int.Parse(s_attid);
                }
                if (nAttid != 0)
                {
                    this.Label18_Attid.Text = CheckBasicInfo(nAttid);
                }
                else {
                    this.Label18_Attid.Text = "";
                }
                
                this.lbInnerID.Text = PublicRes.objectToString(ds.Tables[0],"fuid");
                this.lbFetchMoney.Text = classLibrary.setConfig.FenToYuan(PublicRes.objectToString(ds.Tables[0],"Ffetch"));
                this.lbLeftPay.Text = Transfer.convertBPAY(PublicRes.objectToString(ds.Tables[0], "Fbpay_state"));
                this.lbSave.Text = classLibrary.setConfig.FenToYuan(PublicRes.objectToString(ds.Tables[0],"Fsave"));

                string fuid = PublicRes.objectToString(ds.Tables[0],"fuid");
                if (Label1_Acc.Text != "")
                {
                    string testuid = myService.QQ2Uid(Label1_Acc.Text);
                    if (testuid != null)
                    {
                        labQQstate.Text = "注册未关联";
                        if (testuid.Trim() == fuid)
                        {
                            //再判断是否是已激活 furion 20070226
                            string trueuid = myService.QQ2UidX(Label1_Acc.Text);
                            if (trueuid != null)
                                labQQstate.Text = "已关联";
                            else
                                labQQstate.Text = "已关联未激活";
                        }
                    }
                    else
                        labQQstate.Text = "未注册";
                }
                else
                    labQQstate.Text = "";

                if (labEmail.Text != "")
                {
                    string testuid = myService.QQ2Uid(labEmail.Text);
                    if (testuid != null)
                    {
                        labEmailState.Text = "注册未关联";
                        if (testuid.Trim() == fuid)
                        {
                            //再判断是否是已激活 furion 20070226
                            string trueuid = myService.QQ2UidX(labEmail.Text);
                            if (trueuid != null)
                                labEmailState.Text = "已关联";
                            else
                                labEmailState.Text = "已关联未激活";
                        }
                    }
                    else
                        labEmailState.Text = "未注册";
                }
                else
                    labEmailState.Text = "";

                if (labMobile.Text != "")
                {
                    string testuid = myService.QQ2Uid(labMobile.Text);
                    if (testuid != null)
                    {
                        labMobileState.Text = "注册未关联";
                        if (testuid.Trim() == fuid)
                        {
                            //再判断是否是已激活 furion 20070226
                            string trueuid = myService.QQ2UidX(labMobile.Text);
                            if (trueuid != null)
                                labMobileState.Text = "已关联";
                            else
                                labMobileState.Text = "已关联未激活";
                        }
                    }
                    else
                        labMobileState.Text = "未注册";
                }
                else
                    labMobileState.Text = "";

                LinkButton3.Text = "";

                if (Label12_Fstate.Text.Trim() == "正常")
                {
                    LinkButton3.Text = "冻结";
                }
                else if (Label12_Fstate.Text.Trim() == "冻结")
                {
                    LinkButton3.Text = "解冻";
                }
            }

            SetButtonVisible(); //furion 20050902
      
            try
            {
                string uin = Session["QQID"].ToString();
                AccountService acc = new AccountService();
                DataTable dt = new VIPService().QueryVipInfo(uin.Trim());
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.vip_value.Text = dt.Rows[0]["value"].ToString();
                    this.vip_flag.Text = dt.Rows[0]["vipflag_str"].ToString();
                    this.vip_level.Text = dt.Rows[0]["level"].ToString();
                    this.vip_channel.Text = dt.Rows[0]["subid_str"].ToString();
                    this.vip_exp_date.Text = dt.Rows[0]["exp_date"].ToString();
                }
            }
            catch
            {
            }

            try
            {
                //增加删除认证的操作日志
                DataSet dsdgList = myService.GetUserClassDeleteList(this.TextBox1_InputQQ.Text.Trim());
                if (dsdgList != null && dsdgList.Tables.Count > 0 && dsdgList.Tables[0].Rows.Count > 0)
                {
                    this.dgList.Visible = true;
                    this.dgList.DataSource = dsdgList.Tables[0].DefaultView;
                    this.DataBind();
                }
                else
                    this.dgList.Visible = false;
            }
            catch
            { }

            try
            {
                string msg = "";
                int state = GetUserClassInfo(Session["QQID"].ToString(), out msg);

                labUserClassInfo.Text = msg;
            }
            catch
            { }

        }

        //注销账号信息查询
        private void BindDataCancel(int istr, int imax)
        {
            Query_Service.Query_Service myService = new Query_Service.Query_Service();
            myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
            DataSet ds = new DataSet();
            ds = myService.GetUserAccountCancel(this.TextBox1_InputQQ.Text.Trim(), 1, istr, imax);

            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {             
                    throw new Exception("数据库无此记录");
            }
            else
            {
                this.lbInnerID.Text = ds.Tables[0].Rows[0]["Fuid"].ToString();
                //this.Label1_Acc.Text			= this.TextBox1_InputQQ.Text.Trim();  //"22000254";
                this.Label1_Acc.Text = ds.Tables[0].Rows[0]["Fqqid"].ToString();

                Session["QQID"] = ds.Tables[0].Rows[0]["Fqqid"].ToString();

                this.Label2_Type.Text = Transfer.convertMoney_type(ds.Tables[0].Rows[0]["Fcurtype"].ToString());//tu.u_CurType;				   //"代金券";
                this.Label3_LeftAcc.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fbalance"].ToString());//tu.u_Balance;				   //"3000";
                //this.Label4_Freeze.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fcon"].ToString());                  //"1000";
                this.Label5_YestodayLeft.Text = ds.Tables[0].Rows[0]["Fyday_balance"].ToString();		   //"10";
                this.lblLoginTime.Text = ds.Tables[0].Rows[0]["Fcreate_time"].ToString();
                this.Label6_LastModify.Text = ds.Tables[0].Rows[0]["Fmodify_time"].ToString();		   //"2005-05-01";
                this.Label7_SingleMax.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fquota"].ToString());		   //"2000";
                this.Label8_PerDayLmt.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fquota_pay"].ToString());			//"5000";
                this.Label9_LastSaveDate.Text = ds.Tables[0].Rows[0]["Fsave_time"].ToString();				//"2005-03-01";
                this.Label10_Drawing.Text = ds.Tables[0].Rows[0]["Ffetch_time"].ToString();              //"2005-04-15";
                this.Label11_Remark.Text = ds.Tables[0].Rows[0]["Fmemo"].ToString();					//"这个家伙很懒，什么都没有留下！";
                this.Label12_Fstate.Text = Transfer.accountState(ds.Tables[0].Rows[0]["Fstate"].ToString());
                this.Label13_Fuser_type.Text = Transfer.convertFuser_type(ds.Tables[0].Rows[0]["Fuser_type"].ToString());

                string s_fz_amt = PublicRes.objectToString(ds.Tables[0], "Ffz_amt"); //分账冻结金额
                string s_cron = PublicRes.objectToString(ds.Tables[0], "Fcon");
                long l_fzamt = 0, l_cron = 0;
                if (s_fz_amt != "")
                {
                    l_fzamt = long.Parse(s_fz_amt);
                }
                if (s_cron != "")
                {
                    l_cron = long.Parse(s_cron);
                }

                this.Label4_Freeze.Text = TENCENT.OSS.CFT.KF.Common.MoneyTransfer.FenToYuan(l_fzamt + l_cron).ToString("f2") + "元";    // classLibrary.setConfig.FenToYuan(l_fzamt + l_cron);//冻结金额=分账冻结金额+冻结金额

                // 2012/5/2 因为需要Q_USER_INFO获取准确的用户真实姓名而改动
                try
                {
                    this.Label14_Ftruename.Text = ds.Tables[0].Rows[0]["UserRealName2"].ToString();
                }
                catch
                {
                    this.Label14_Ftruename.Text = ds.Tables[0].Rows[0]["Ftruename"].ToString();
                }
                this.Label15_Useable.Text = classLibrary.setConfig.FenToYuan((long.Parse(ds.Tables[0].Rows[0]["Fbalance"].ToString()) - long.Parse(ds.Tables[0].Rows[0]["Fcon"].ToString())).ToString());  //帐户余额减去冻结余额= 可用余额
                this.Label16_Fapay.Text = ds.Tables[0].Rows[0]["Fapay"].ToString();
                this.Label17_Flogin_ip.Text = ds.Tables[0].Rows[0]["Flogin_ip"].ToString();

                //furion 20061116 email登录修改
                this.labEmail.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Femail"]);
                this.labMobile.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Fmobile"]);
                //2006-10-18 edwinyang 增加产品属性
                int nAttid = int.Parse(ds.Tables[0].Rows[0]["Att_id"].ToString());
                //				pbp.BindDropDownList(pm.QueryDicAccName(),ddlAttid,out Msg);
                this.Label18_Attid.Text = CheckBasicInfo(nAttid);
                this.lbFetchMoney.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Ffetch"].ToString().Trim());
                this.lbLeftPay.Text = Transfer.convertBPAY(ds.Tables[0].Rows[0]["Fbpay_state"].ToString().Trim());
                this.lbSave.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fsave"].ToString().Trim());

                string fuid = ds.Tables[0].Rows[0]["fuid"].ToString().Trim();

                if (Label1_Acc.Text != "")
                {
                    string testuid = myService.QQ2Uid(Label1_Acc.Text);
                    if (testuid != null)
                    {
                        labQQstate.Text = "注册未关联";
                        if (testuid.Trim() == fuid)
                        {
                            //再判断是否是已激活 furion 20070226
                            string trueuid = myService.QQ2UidX(Label1_Acc.Text);
                            if (trueuid != null)
                                labQQstate.Text = "已关联";
                            else
                                labQQstate.Text = "已关联未激活";
                        }
                    }
                    else
                        labQQstate.Text = "未注册";
                }
                else
                    labQQstate.Text = "";

                if (labEmail.Text != "")
                {
                    string testuid = myService.QQ2Uid(labEmail.Text);
                    if (testuid != null)
                    {
                        labEmailState.Text = "注册未关联";
                        if (testuid.Trim() == fuid)
                        {
                            //再判断是否是已激活 furion 20070226
                            string trueuid = myService.QQ2UidX(labEmail.Text);
                            if (trueuid != null)
                                labEmailState.Text = "已关联";
                            else
                                labEmailState.Text = "已关联未激活";
                        }
                    }
                    else
                        labEmailState.Text = "未注册";
                }
                else
                    labEmailState.Text = "";

                if (labMobile.Text != "")
                {
                    string testuid = myService.QQ2Uid(labMobile.Text);
                    if (testuid != null)
                    {
                        labMobileState.Text = "注册未关联";
                        if (testuid.Trim() == fuid)
                        {
                            //再判断是否是已激活 furion 20070226
                            string trueuid = myService.QQ2UidX(labMobile.Text);
                            if (trueuid != null)
                                labMobileState.Text = "已关联";
                            else
                                labMobileState.Text = "已关联未激活";
                        }
                    }
                    else
                        labMobileState.Text = "未注册";
                }
                else
                    labMobileState.Text = "";

                LinkButton3.Text = "";

                if (Label12_Fstate.Text.Trim() == "正常")
                {
                    LinkButton3.Text = "冻结";
                }
                else if (Label12_Fstate.Text.Trim() == "冻结")
                {
                    LinkButton3.Text = "解冻";
                }
            }

            SetButtonVisible(); //furion 20050902

            if (Session["QQID"]==null&&Session["QQID"]=="")
            {
                return;
            }

        
            try
            {
                string uin = Session["QQID"].ToString();
                AccountService acc = new AccountService();
                DataTable dt =new VIPService().QueryVipInfo(uin.Trim());
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.vip_value.Text = dt.Rows[0]["value"].ToString();
                    this.vip_flag.Text = dt.Rows[0]["vipflag_str"].ToString();
                    this.vip_level.Text = dt.Rows[0]["level"].ToString();
                    this.vip_channel.Text = dt.Rows[0]["subid_str"].ToString();
                    this.vip_exp_date.Text = dt.Rows[0]["exp_date"].ToString();
                }
            }
            catch
            {
            }

            try
            {
                //增加删除认证的操作日志
                DataSet dsdgList = myService.GetUserClassDeleteList(this.TextBox1_InputQQ.Text.Trim());
                if (dsdgList != null && dsdgList.Tables.Count > 0 && dsdgList.Tables[0].Rows.Count > 0)
                {
                    this.dgList.Visible = true;
                    this.dgList.DataSource = dsdgList.Tables[0].DefaultView;
                    this.DataBind();
                }
                else
                    this.dgList.Visible = false;
            }
            catch
            { }

            try
            {
                string msg = "";
                int state = GetUserClassInfo(Session["QQID"].ToString(), out msg);

                labUserClassInfo.Text = msg;
            }
            catch
            { }

        }

        protected void BindCKVValue(string uid)
        {
            var dic = new AuthenService().WXOperateCKVCGI(1,uid);
            if (dic == null)
            {
                WebUtils.ShowMessage(this.Page, "CKV查询出错");
                return;
            }
            CKV_WXRemainder.Text =classLibrary.setConfig.FenToYuan(dic["balance"]);
            CKV_freeze.Text = classLibrary.setConfig.FenToYuan(dic["con"]);
        }

        private int GetUserClassInfo(string qqid, out string msg)
        {
            //查询一下用户认证信息 furion 20071227
            string inmsg = "uin=" + qqid.Trim().ToLower();
            inmsg += "&opr_type=3";

            string reply;
            short sresult;

            if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("au_query_auinfo_service", inmsg, false, out reply, out sresult, out msg))
            {
                if (sresult != 0)
                {
                    msg = "au_query_auinfo_service接口失败：result=" + sresult + "，msg=" + msg + "&reply=" + reply;
                    return -1;
                }
                else
                {
                    if (reply.StartsWith("result=0"))
                    {
                        //在这取msg显示出来.
                        int iindex = reply.IndexOf("state=");
                        if (iindex > 0)
                        {
                            iindex = Int32.Parse(reply.Substring(iindex + 6, 1));
                            if (iindex == 0)
                            {
                                msg = "未验证";
                            }
                            else if (iindex == 1)
                            {
                                msg = "验证通过";
                            }
                            else if (iindex == 2)
                            {
                                msg = "身份验证中";
                            }
                            else if (iindex == 3)
                            {
                                msg = "验证失败,不可再申请";
                            }
                            else if (iindex == 4)
                            {
                                msg = "验证失败,可再申请";
                            }
                            else
                            {
                                msg = "未定义类型" + iindex;
                            }
                        }

                        return iindex;
                    }
                    else
                    {
                        msg = "au_query_auinfo_service接口失败：result=" + sresult + "，msg=" + msg + "&reply=" + reply;
                        return -1;
                    }
                }
            }
            else
            {
                msg = "au_query_auinfo_service接口失败：result=" + sresult + "，msg=" + msg + "&reply=" + reply;
                return -1;
            }
        }

        private static string getCgiString(string instr)
        {
            if (instr == null || instr.Trim() == "")
                return "";

            return System.Web.HttpContext.Current.Server.UrlDecode(instr).Replace("\r\n", "").Trim()
                .Replace("%3d", "=").Replace("%20", " ").Replace("%26", "&");
        }

        private void setIframePath()
        {
            iFramePath = "../BaseAccount/TradeLog.aspx";
        }

        private string GetQueryType()
        {
            if (string.IsNullOrEmpty(this.TextBox1_InputQQ.Text))
            {
                throw new Exception("请输入要查询的账号");
            }
            if (this.WeChatCft.Checked)
            {
                return "WeChatCft";
            }
            else if (this.WeChatUid.Checked)
            {
                return "WeChatUid";
            }
            else if (this.WeChatQQ.Checked)
            {
                return "WeChatQQ";
            }
            else if (this.WeChatMobile.Checked)
            {
                return "WeChatMobile";
            }
            else if (this.WeChatEmail.Checked)
            {
                return "WeChatEmail";
            }
            else if (this.WeChatId.Checked)
            {
                return "WeChatId";
            }

            return null;
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

                string queryType = GetQueryType();

                Session["QQID"] = AccountService.GetQQID(queryType, this.TextBox1_InputQQ.Text);                
                //Session["QQID"] = getQQID();

                iFrameHeight = "230";   //iFame显示区域的高度

                if (this.WeChatId.Checked || this.WeChatQQ.Checked || this.WeChatMobile.Checked || this.WeChatEmail.Checked || this.WeChatCft.Checked || this.WeChatUid.Checked)
                {
                    //使用微信查询
                    ViewState["iswechat"] = "true";
                }
                else
                {
                    //使用账号查询
                    ViewState["iswechat"] = "false";
                    throw new Exception("请选择查询条件");
                }
                
                if (Session["QQID"]==null)//echo,如果查询条件为内部id，且对应QQ为空（此时有可能是注销的账户）
                {
                    BindDataCancel(1, 1);   //查询注销账户信息
                }
                else 
                {
                    if (!(Session["QQID"] == null))
                    {
                    BindData(1, 1);           //绑定数据
                    }
                }
                setIframePath();        //设置路径	

                //GetUserAccount 得到用户账户信息
                BindCKVValue(this.lbInnerID.Text);
                this.LKBT_TradeLog.Visible = true;
                this.LKBT_TradeLog_Sale.Visible = true;
                this.LkBT_PaymentLog.Visible = true;
                this.LKBT_bankrollLog.Visible = true;
                this.LKBT_GatheringLog.Visible = true;
                this.LkBT_PaymentLog.Visible = true;
                this.LkBT_Refund.Visible = true;
                this.LkBT_Refund_Sale.Visible = true;

                if (Label1_Acc.Text.Length > 0 && Label1_Acc.Text.EndsWith("@wx.tenpay.com"))
                {
                    try
                    {
                        var ticket = Session["oa_ticket"] as string;
                        if (string.IsNullOrEmpty(ticket))
                        {
                            TempErrlog.InnerText = "ticket为空 不可获取";
                        }
                        else
                        {
                            var openid = Label1_Acc.Text.Substring(0, Label1_Acc.Text.Length - 14);
                            labEmail.Text = WeChatHelper.GetUserNameFromOpenid(openid, ticket);
                        }
                    }
                    catch (Exception ex)
                    {
                        //开发阶段   用这个方式 记录一下 异常
                        TempErrlog.InnerText = ex.ToString();
                    }
                }
            }
            catch (SoapException er) //捕获soap类
            {
                this.LKBT_TradeLog.Visible = false;
                this.LKBT_TradeLog_Sale.Visible = false;
                this.LKBT_TradeLog_Unfinished.Visible = false;
                this.LKBT_TradeLog_Sale_Unfinished.Visible = false;
                this.LkBT_PaymentLog.Visible = false;
                this.LKBT_bankrollLog.Visible = false;
                this.LKBT_GatheringLog.Visible = false;
                this.LkBT_PaymentLog.Visible = false;
                this.LkBT_Refund.Visible = false;
                this.LkBT_Refund_Sale.Visible = false;
                setLableText_Null();
                string str = PublicRes.GetErrorMsg(er.Message.ToString());
                WebUtils.ShowMessage(this.Page, "查询错误：" + str);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "查询错误：" + eSys.Message.ToString());
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

        protected void LKBT_TradeLog_Click(object sender, System.EventArgs e)
        {
            if (Session["uid"] == null)
            {
                WebUtils.ShowMessage(this.Page, "超时，请重新登陆！");
            }
            else
            {
                SetFrame("../BaseAccount/TradeLog.aspx?user=buy", 230, this.LKBT_TradeLog);
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
                SetFrame("../BaseAccount/TradeLog.aspx?user=sale", 230, this.LKBT_TradeLog_Sale);
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
                SetFrame("../BaseAccount/TradeLog.aspx?user=Unfinished", 230, this.LKBT_TradeLog_Unfinished);
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
                SetFrame("../BaseAccount/TradeLog.aspx?user=SaleUnfinished", 230, this.LKBT_TradeLog_Sale_Unfinished);
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
                SetFrame("../BaseAccount/bankrollLog.aspx?type=QQID", 230, this.LKBT_bankrollLog);
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
                SetFrame("../BaseAccount/GatheringLog.aspx?type=QQID&history=" + isHistory, 230, this.LKBT_GatheringLog);
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
                SetFrame("../BaseAccount/PaymentLog.aspx?type=QQID", 230, this.LkBT_PaymentLog);
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
                    Response.Redirect("../BaseAccount/LeftAccountFreeze.aspx?id=true&qq=" + Session["QQID"].ToString().Trim() + "&moy=" + this.Label15_Useable.Text.Trim());
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
                    Response.Redirect("../BaseAccount/LeftAccountFreeze.aspx?id=false&qq=" + Session["QQID"].ToString().Trim() + "&moy=" + this.Label4_Freeze.Text.Trim()); //参数：1 解冻 2 QQID 3 冻结金额
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
                SetFrame("../BaseAccount/refund.aspx?type=buy", 230, this.LkBT_Refund);
            }
        }

        protected void LinkButton3_Click(object sender, System.EventArgs e)
        {
            if (Label12_Fstate.Text.Trim() == "正常")
            {
                Response.Write("<script language=javascript>window.location='../BaseAccount/freezeBankAcc.aspx?id=true&uid=" + Session["QQID"].ToString().Trim() + "&iswechat=" + ViewState["iswechat"].ToString() + "&type=per'</script>");
            }
            else if (Label12_Fstate.Text.Trim() == "冻结")
            {
                Response.Write("<script language=javascript>window.location='../BaseAccount/freezeBankAcc.aspx?id=false&uid=" + Session["QQID"].ToString().Trim() + "&iswechat=" + ViewState["iswechat"].ToString() + "&type=per'</script>");
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
                SetFrame("../BaseAccount/ButtonInfo.aspx", 230, this.LkBT_ButtonInfo);
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
                SetFrame("../BaseAccount/GwqByQQ.aspx", 230, this.LkBT_Gwq);
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
                SetFrame("../BaseAccount/refund.aspx?type=sale", 230, this.LkBT_Refund_Sale);
            }
        }

        protected void btnDelClass_Click(object sender, System.EventArgs e)
        {
            //删除认证。
            try
            {
                if (Session["QQID"] == null || Session["QQID"].ToString().Trim() == "")
                    return;

                Query_Service.Query_Service myService = new Query_Service.Query_Service();
                Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
                myService.Finance_HeaderValue = fh;
                string msg = "";
                string qqid = Session["QQID"].ToString().Trim();
                if (myService.DelAuthen(qqid, out msg))
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
            BindData(1, 1);
        }

        protected void LkBT_mediOrder_Click(object sender, EventArgs e)
        {
            if (Session["uid"] == null)
            {
                WebUtils.ShowMessage(this.Page, "超时，请重新登陆！");
            }
            else
            {
                SetFrame("../BaseAccount/TradeLog.aspx?user=mediOrder", 230, this.LkBT_mediOrder);
            }
        }

        protected void CKV_Btn_synchro_Click(object sender, EventArgs e)
        {
            try
            {
                var dic = new AuthenService().WXOperateCKVCGI(2, lbInnerID.Text);
                if (dic == null)
                {
                    WebUtils.ShowMessage(this.Page, "CKV同步错误");
                    return;
                }
                WebUtils.ShowMessage(this.Page, dic["RESULT"] == "0" ? "同步成功" : "同步失败");
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "同步:" + classLibrary.setConfig.replaceMStr(ex.Message));
            }
  
        }
    }

}

