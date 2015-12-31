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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Xml;
using CFT.CSOMS.BLL.ForeignCurrencyModule;
using TENCENT.OSS.CFT.KF.KF_Web.InternetBank;
using CFT.CSOMS.BLL.SysManageModule;
using System.IO;
using CFT.CSOMS.DAL.CheckModoule;
using CFT.CSOMS.COMMLIB;
using CFT.CSOMS.BLL.TradeModule;
using CFT.CSOMS.BLL.FundModule;
using System.Linq;
using System.Collections.Generic;
using commLib.Entity;
using CFT.Apollo.Common.Configuration;
namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
    /// <summary>
    /// QueryYTTrade 的摘要说明。
    /// </summary>
    public partial class GetFundRatePageDetail : System.Web.UI.Page
    {
        CheckService checkService = new CheckService();
        //static Hashtable images;
        protected void Page_Load(object sender, System.EventArgs e)
        {

            try
            {

                if (!IsPostBack)
                {
                    if (classLibrary.ClassLib.ValidateRight("InfoCenter", this))
                    {
                        Func<string, string> GetQueryString = name =>
                        {
                            var v = Request.QueryString[name];
                            if (v == null)
                            {
                                return string.Empty;
                            }
                            ViewState[name] = v = v.Trim();
                            return v;
                        };

                        var close_flag = GetQueryString("close_flag");
                        var opertype = GetQueryString("opertype");
                        var uin = GetQueryString("uin");
                        var bind_serialno = GetQueryString("bind_serialno");
                        var bank_type = GetQueryString("bank_type");
                        var card_tail = GetQueryString("card_tail");
                        var spid = GetQueryString("spid");
                        var fund_code = GetQueryString("fund_code");
                        var total_fee = GetQueryString("total_fee");
                        var end_date = GetQueryString("end_date");
                        var mobile = GetQueryString("mobile");
                        var LCTFund = GetQueryString("LCTFund");

                        if (opertype == "1")  //客服系统提交审批
                        {
                            if (LCTFund == "true" && uin != "" && total_fee != "") //理财通余额强赎
                            {
                                tbLCTinput.Visible = true;
                                return;
                            }

                            #region 封闭基金、非封闭基金、指数基金强赎、半封闭基金
                            if (close_flag != "" && uin != "" && spid != "" && fund_code != "" && total_fee != "")
                            {
                                if (close_flag == "2") //封闭基金
                                {
                                    if (end_date == "")
                                    {
                                        Response.Redirect("../login.aspx?wh=1");
                                    }
                                    this.tableCloseInput.Visible = true;
                                }
                                else if (close_flag == "1")  //不封闭基金, 指数基金
                                {
                                    if (bind_serialno == "" && card_tail == "" && bank_type == "" && mobile == "")
                                    {
                                        Response.Redirect("../login.aspx?wh=1");
                                    }

                                    FundService fundBLLService = new FundService();
                                    if (fundBLLService.isSpecialFund(fund_code, spid)) //指数基金
                                    {
                                        CreateFRateApply();
                                    }
                                    else
                                    {
                                        this.tableUNCloseApply.Visible = true;
                                        CreateApplyUNClose();
                                    }
                                }
                                else if (close_flag == "3")  //不封闭基金, 光大永明光明财富2号
                                {
                                    FundService fundBLLService = new FundService();
                                    if (fundBLLService.isInsurance(fund_code, spid)) // 光大使用指数基金  接口
                                    {
                                        Label27.Text = "保险强赎";
                                        GetQueryString("close_id");
                                        GetQueryString("total_fee");
                                        CreateFRateApply();
                                    }
                                }
                            }
                            else
                            {//必传参数
                                Response.Redirect("../login.aspx?wh=1");
                            }
                            #endregion
                        }
                        else if (opertype == "2")  //到期申购/赎回策略 预览
                        {
                            AlterEndStrategyPreview();
                        }
                        else
                        {
                            #region 账务系统查看

                            string objid = GetQueryString("objid");
                            var FundBalanceRedeem = GetQueryString("FundBalanceRedeem");
                            if (objid != "")
                            {
                                if (close_flag == "2")//封闭
                                {
                                    bindCloseApply(objid);
                                }
                                else if (close_flag == "1")//不封闭
                                {
                                    if (FundBalanceRedeem == "true")  //指数基金
                                    {
                                        bindFRateApply(objid);
                                    }
                                    else
                                    {
                                        bindUNCloseApply(objid);
                                    }
                                }
                                else if (LCTFund == "true") //理财通余额强赎 
                                {
                                    bindLCTFundApply(objid);
                                }
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        Response.Redirect("../login.aspx?wh=1");
                    }
                }
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
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

        //封闭 产生申请单
        public void btnCreateApplyClose_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                {
                    throw new Exception("截止日期为空！");
                }
                upImage(FileClose);

                tb_Cuin.Text = ViewState["uin"].ToString();
                tb_Cspid.Text = ViewState["spid"].ToString();
                tb_Cfund_code.Text = ViewState["fund_code"].ToString();
                tb_Ctotal_fee.Text = ViewState["total_fee"].ToString();
                tb_Cend_date.Text = ViewState["end_date"].ToString();
                tb_Cend_dateHand.Text = txtEndDate.Text.Trim();
                string ip = Request.UserHostAddress.ToString();
                if (ip == "::1")
                    ip = "127.0.0.1";
                tb_Cclient_ip.Text = ip;

                this.ImageC.ImageUrl = ViewState["kfPath"].ToString();//为了提交申请时在客服系统能看图片，此时浏览图片取的是客服系统保存的图片。
                ViewState["ImageCUrl"] = ViewState["alPath"]; //fileResult.url; //System.Configuration.ConfigurationManager.AppSettings["GetImageFromKf2Url"].ToString() + ViewState["alPath"].ToString();


                this.tableCloseApply.Visible = true;
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "生成申请单失败！" + PublicRes.GetErrorMsg(eSys.ToString()));
                return;
            }
        }

        //封闭 提交申请单
        public void btnSubmitCloseApply_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (tb_Cend_date.Text.Trim() != tb_Cend_dateHand.Text.Trim())
                {
                    WebUtils.ShowMessage(this.Page, "客服手工输入截止日期 与 基金截至日期 不一致！");
                    return;
                }

                string objid = System.DateTime.Now.ToString("yyyyMMddHHmmss");
                string ReturnUrl = "http://kf.cf.com/NewQueryInfoPages/GetFundRatePageDetail.aspx?close_flag=2&objid=" + objid + "&opertype=0";
                string[,] param = new string[,] {  { "close_flag", ViewState["close_flag"].ToString() },
                                { "uin", ViewState["uin"].ToString() }, 
                                { "spid", ViewState["spid"].ToString() },
                                { "fund_code", ViewState["fund_code"].ToString() },
                                { "total_fee", ViewState["total_fee"].ToString() },
                                { "acct_type", "2" },
                                { "channel_id","stat_type=68|fm_6_qs_1"},//透传
                                { "redem_type", "3" },
                                { "end_date", tb_Cend_date.Text.Trim() },
                                { "end_date_hand", tb_Cend_dateHand.Text.Trim() },//客服手工输入截止日期
                                { "client_ip",tb_Cclient_ip.Text.Trim() },
                                //{ "ImageUrl",ImageC.ImageUrl.Trim() },
                                { "ImageUrl",ViewState["ImageCUrl"].ToString().Trim() },
                                { "operator",Session["uid"].ToString() },
                                { "ReturnUrl",ReturnUrl},
                              };

                Check_WebService.Param[] pa = PublicRes.ToParamArray(param);
                string memo = "客服系统提交强赎审批信息，close_flag:2|uin:" + ViewState["uin"].ToString()
                             + "|spid:" + ViewState["spid"].ToString()
                             + "|fund_code:" + ViewState["fund_code"].ToString()
                             + "|total_fee:" + ViewState["total_fee"].ToString()
                             + "|end_date:" + tb_Cend_date.Text.Trim();
                SunLibrary.LoggerFactory.Get("GetFundRatePageDetail").Info(memo);
                PublicRes.CreateCheckService(this).StartCheck(objid, "CloseRedemFundType", memo, "0", pa);
                //  WebUtils.ShowMessage(this.Page, "已提交，请等待审批！");
                this.ShowMsg("已提交，请等待审批！");

            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "提交申请单失败：" + PublicRes.GetErrorMsg(eSys.ToString()));
                return;
            }
        }

        //封闭 账务系统查看
        public void bindCloseApply(string objid)
        {
            try
            {
                DataTable dt = checkService.GetCheckInfo(objid, "CloseRedemFundType");
                if (dt != null && dt.Rows.Count != 0)
                {
                    tb_Cuin.Text = dt.Rows[0]["uin"].ToString();
                    tb_Cspid.Text = dt.Rows[0]["spid"].ToString();
                    tb_Cfund_code.Text = dt.Rows[0]["fund_code"].ToString();
                    tb_Ctotal_fee.Text = dt.Rows[0]["total_fee"].ToString();
                    tb_Cend_date.Text = dt.Rows[0]["end_date"].ToString();
                    tb_Cend_dateHand.Text = dt.Rows[0]["end_date_hand"].ToString();
                    tb_Cclient_ip.Text = dt.Rows[0]["client_ip"].ToString();
                    this.ImageC.ImageUrl = dt.Rows[0]["ImageUrl"].ToString();
                    this.tableCloseApply.Visible = true;
                    this.tableUNCloseApply.Visible = false;
                    this.ButtonSubmitClose.Visible = false;
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "查询出错！");
                    return;
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "申请单查询失败！" + PublicRes.GetErrorMsg(eSys.ToString()));
                return;
            }
        }


        //不封闭 产生申请单
        public void CreateApplyUNClose()
        {
            try
            {
                var spid = ViewState["spid"].ToString();
                var fund_code = ViewState["fund_code"].ToString();
                tb_UNCuin.Text = ViewState["uin"].ToString();
                tb_UNCspid.Text = spid;
                tb_UNCfund_code.Text = fund_code;
                tb_UNCtotal_fee.Text = MoneyTransfer.FenToYuan(ViewState["total_fee"].ToString());
                tb_UNCbind_serialno.Text = ViewState["bind_serialno"].ToString();
                tb_UNCcard_tail.Text = ViewState["card_tail"].ToString();
                tb_UNCbank_type.Text = TENCENT.OSS.C2C.Finance.BankLib.BankIO.QueryBankName(ViewState["bank_type"].ToString());
                tb_UNmobile.Text = ViewState["mobile"].ToString();
                if (spid == "1230258701" && fund_code == "990001")
                {
                    tb_UNCredem_type.Text = "t+1";
                    tb_UNCredem_type.Attributes["data-value"] = "2";
                }
                string ip = Request.UserHostAddress.ToString();
                if (ip == "::1")
                    ip = "127.0.0.1";
                this.tb_UNCclient_ip.Text = ip;
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "生成申请单失败！" + PublicRes.GetErrorMsg(eSys.ToString()));
                return;
            }
        }

        /// <summary>
        /// 不封闭 提交申请单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnSubmitUNCloseApply_Click(object sender, System.EventArgs e)
        {
            try
            {
                string objid = System.DateTime.Now.ToString("yyyyMMddHHmmss");
                string ReturnUrl = "http://kf.cf.com/NewQueryInfoPages/GetFundRatePageDetail.aspx?close_flag=1&objid=" + objid + "&opertype=0";

                //        string bankID_Encode = CommUtil.EncryptZerosPadding(tb_UNCbank_id_new.Text.Trim());
                string[,] param = new string[,] {  { "close_flag", ViewState["close_flag"].ToString() },
                                { "uin", ViewState["uin"].ToString() }, 
                                { "spid", ViewState["spid"].ToString() },
                                { "total_fee", ViewState["total_fee"].ToString() },
                                { "fund_code", ViewState["fund_code"].ToString() },
                                { "channel_id","stat_type=68|fm_6_qs_2"},
                                { "bind_serialno", ViewState["bind_serialno"].ToString() },
                                { "card_tail", ViewState["card_tail"].ToString() },
                                { "bank_type",ViewState["bank_type"].ToString() },
                                { "cur_type","1" },
                                { "redem_type", tb_UNCredem_type.Attributes["data-value"] },//赎回类型 1 t+0  2 t+1
                                { "client_ip", this.tb_UNCclient_ip.Text.Trim()},
                                { "mobile",ViewState["mobile"].ToString() },
                                { "operator",Session["uid"].ToString() },
                                { "ReturnUrl",ReturnUrl},
                               };

                Check_WebService.Param[] pa = PublicRes.ToParamArray(param);
                string memo = "客服系统提交强赎审批信息，close_flag:1|uin:" + ViewState["uin"].ToString()
                             + "|spid:" + ViewState["spid"].ToString()
                             + "|fund_code:" + ViewState["fund_code"].ToString()
                             + "|total_fee:" + ViewState["total_fee"].ToString()
                             + "|bind_serialno:" + ViewState["bind_serialno"].ToString()
                             + "|card_tail:" + ViewState["card_tail"].ToString()
                             + "|bank_type:" + ViewState["bank_type"].ToString()
                             + "|mobile:" + ViewState["mobile"].ToString();
                SunLibrary.LoggerFactory.Get("GetFundRatePageDetail").Info(memo);
                PublicRes.CreateCheckService(this).StartCheck(objid, "UnCloseRedemFundType", memo, "0", pa);
                // WebUtils.ShowMessage(this.Page, "已提交，请等待审批！");
                this.ShowMsg("已提交，请等待审批！");
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "提交申请单失败：" + PublicRes.GetErrorMsg(eSys.ToString()));
                return;
            }
        }
        /// <summary>
        /// 理财通余额强赎提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLCTFundApply_Click(object sender, EventArgs e)
        {
            try
            {
                string objid = System.DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.StaticNoManage();
                string ReturnUrl = "http://kf.cf.com/NewQueryInfoPages/GetFundRatePageDetail.aspx?opertype=0&LCTFund=true&close_flag=&objid=" + objid;
                //#if Debug
                //ReturnUrl = "http://localhost:52584/NewQueryInfoPages/GetFundRatePageDetail.aspx?opertype=0&LCTFund=true&close_flag=&objid=" + objid;
                //#endif

                Hashtable ht = new Hashtable();

                string[,] param = new string[,] {   
                                { "uin", ViewState["uin"].ToString() }, 
                                { "total_fee", ViewState["total_fee"].ToString() }, 
                                //{ "fund_code", ViewState["fund_code"].ToString() }, 
                                { "bind_serialno", ViewState["bind_serialno"].ToString() }, 
                                { "bank_type", ViewState["bank_type"].ToString() }, 
                                { "card_tail", ViewState["card_tail"].ToString() }, 
                                { "Description",lblLCTDescription.Text.Trim() },
                                { "client_ip", lblLCTclient_ip.Text },

                                { "ImageUrl",ViewState["imgUrlLCT"].ToString().Trim() },
                                { "operator",Session["uid"].ToString() },
                                { "ReturnUrl",ReturnUrl},
                              };

                Check_WebService.Param[] pa = PublicRes.ToParamArray(param);
                string memo = "LCTFund:true"
                             + "|uin:" + ViewState["uin"].ToString()
                             + "|total_fee:" + ViewState["total_fee"].ToString()
                    //+ "|fund_code:" + ViewState["fund_code"].ToString()
                             + "|bind_serialno:" + ViewState["bind_serialno"].ToString()
                             + "|bank_type:" + ViewState["bank_type"].ToString()
                             + "|card_tail:" + ViewState["card_tail"].ToString()
                             + "|Description:" + lblLCTDescription.Text.Trim();
                SunLibrary.LoggerFactory.Get("GetFundRatePageDetail").Info(memo);
                string levelVelue = classLibrary.setConfig.FenToYuan(Convert.ToDouble(ViewState["total_fee"].ToString()));
                levelVelue = levelVelue.Replace("元", "");
                try
                {
                    PublicRes.CreateCheckService(this).StartCheck(objid, "LCTBalanceRedeem", memo, levelVelue, pa);
                }
                catch (Exception err)
                {
                    if (err.Message.Contains("该财付通账号已经有待审批的理财通余额强赎"))
                    {
                        this.ShowMsg("该财付通账号已经有待审批的理财通余额强赎！");
                    }
                    else
                    {
                        throw err;
                    }
                }
                this.ShowMsg("已提交，请等待审批！");

            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "提交申请单失败：" + PublicRes.GetErrorMsg(eSys.ToString()));
                return;
            }
        }
        /// <summary>
        /// 理财通余额枪赎产生申请单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCreateLCTFund_Click(object sender, EventArgs e)
        {
            try
            {
                upImage(FileLCT);
                lblLCT_uin.Text = ViewState["uin"].ToString();
                lblLCT_total_fee.Text = ViewState["total_fee"].ToString();
                //lblLCT_fund_code.Text = ViewState["fund_code"].ToString();
                lblLCT_bind_serialno.Text = ViewState["bind_serialno"].ToString();
                lblLCT_bank_type.Text = ViewState["bank_type"].ToString();
                lblLCT_card_tail.Text = ViewState["card_tail"].ToString();
                lblLCTDescription.Text = txtLCTDescription.Text;
                string ip = Request.UserHostAddress.ToString();
                if (ip == "::1")
                    ip = "127.0.0.1";
                lblLCTclient_ip.Text = ip;
                this.imgLCT.ImageUrl = ViewState["kfPath"].ToString();
                ViewState["imgUrlLCT"] = ViewState["alPath"];
                //#if Debug
                //ViewState["imgUrlLCT"] = "/" + imgUrlLCT;
                //#endif
                this.tbLCT.Visible = true;
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "生成申请单失败！" + PublicRes.GetErrorMsg(eSys.ToString()));
                return;
            }
        }
        /// <summary>
        /// 理财通余额强赎账务系统查看
        /// </summary>
        /// <param name="objid"></param>
        private void bindLCTFundApply(string objid)
        {
            try
            {
                DataTable dt = checkService.GetCheckInfo(objid, "LCTBalanceRedeem");
                if (dt != null && dt.Rows.Count != 0)
                {
                    lblLCT_uin.Text = dt.Rows[0]["uin"].ToString();
                    lblLCT_total_fee.Text = dt.Rows[0]["total_fee"].ToString();
                    //lblLCT_fund_code.Text = dt.Rows[0]["fund_code"].ToString();
                    lblLCT_bind_serialno.Text = dt.Rows[0]["bind_serialno"].ToString();
                    lblLCT_bank_type.Text = dt.Rows[0]["bank_type"].ToString();
                    lblLCT_card_tail.Text = dt.Rows[0]["card_tail"].ToString();
                    lblLCTclient_ip.Text = dt.Rows[0]["client_ip"].ToString();
                    lblLCTDescription.Text = dt.Rows[0]["Description"].ToString();
                    this.imgLCT.ImageUrl = dt.Rows[0]["ImageUrl"].ToString();
                    tbLCT.Visible = true;
                    tbLCTinput.Visible = false;
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "查询出错！");
                    return;
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "申请单查询失败！" + PublicRes.GetErrorMsg(eSys.ToString()));
                return;
            }
        }

        ////不封闭 账务系统查看
        public void bindUNCloseApply(string objid)
        {
            try
            {
                DataTable dt = checkService.GetCheckInfo(objid, "UnCloseRedemFundType");
                if (dt != null && dt.Rows.Count != 0)
                {
                    tb_UNCuin.Text = dt.Rows[0]["uin"].ToString();
                    tb_UNCspid.Text = dt.Rows[0]["spid"].ToString();
                    tb_UNCfund_code.Text = dt.Rows[0]["fund_code"].ToString();
                    tb_UNCtotal_fee.Text = MoneyTransfer.FenToYuan(dt.Rows[0]["total_fee"].ToString());
                    tb_UNCbind_serialno.Text = dt.Rows[0]["bind_serialno"].ToString();
                    tb_UNCcard_tail.Text = dt.Rows[0]["card_tail"].ToString();
                    tb_UNmobile.Text = dt.Rows[0]["mobile"].ToString();
                    tb_UNCbank_type.Text = TENCENT.OSS.C2C.Finance.BankLib.BankIO.QueryBankName(dt.Rows[0]["bank_type"].ToString());
                    tb_UNCclient_ip.Text = dt.Rows[0]["client_ip"].ToString();
                    this.tableCloseApply.Visible = false;
                    this.tableUNCloseApply.Visible = true;
                    this.btnSubmitUNClose.Visible = false;
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "查询出错！");
                    return;
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "申请单查询失败！" + PublicRes.GetErrorMsg(eSys.ToString()));
                return;
            }
        }
        protected void upImage(HtmlInputFile file)
        {
            try
            {
                //上传需要的图片，并返回对应服务器上的地址
                //存放文件
                // string s1 = File1.Value;
                if (file.Value == "")
                {
                    throw new Exception("请上传图片");
                }
                string szTypeName = Path.GetExtension(file.Value);
                string upStr = null;


                if (szTypeName.ToLower() != ".jpg" && szTypeName.ToLower() != ".gif" && szTypeName.ToLower() != ".bmp")
                {
                    throw new Exception("上传的文件不正确，必须为jpg,gif,bmp");
                }

                string fileName = "s1" + DateTime.Now.ToString("yyyyMMddHHmmss") + szTypeName;
                upStr = "uploadfile/" + DateTime.Now.ToString("yyyyMMdd") + "/CSOMS/RedemptionFund";

                string targetPath = Path.Combine(Server.MapPath(Request.ApplicationPath), upStr.Replace("/", "\\"));
                PublicRes.CreateDirectory(targetPath);

                file.PostedFile.SaveAs(Path.Combine(targetPath, fileName));

                if (AppSettings.Get<bool>("FPSenabled", false))
                {
                    //前期 先保留 老的  图片存储方式
                    var result = commLib.FPSFileHelper.UploadFile(file.PostedFile.InputStream, "RedemptionFund/" + fileName);
                    ViewState["alPath"] = ViewState["kfPath"] = result.url;
                }
                else
                {
                    ViewState["kfPath"] = "/" + upStr + "/" + fileName;
                    ViewState["alPath"] = System.Configuration.ConfigurationManager.AppSettings["GetImageFromKf2Url"].ToString() + ViewState["kfPath"].ToString();
                }

            }
            catch (Exception eStr)
            {
                throw new Exception("上传文件失败！", eStr);
            }
        }

        private void ShowMsg(string msg)
        {
            Response.Write("<script language=javascript>window.operner=null;alert('" + msg + "');window.close();</script>");
        }

        //指数基金额强赎
        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                var fund_code = ViewState["fund_code"].ToString();
                var spid = ViewState["spid"].ToString();
                string objid = System.DateTime.Now.ToString("yyyyMMddHHmmss");
                string ReturnUrl = "http://kf.cf.com/NewQueryInfoPages/GetFundRatePageDetail.aspx?close_flag=1&FundBalanceRedeem=true&objid=" + objid + "&opertype=0";

                string memo = "客服系统提交强赎审批信息，close_flag:1"
                             + "|uin:" + ViewState["uin"].ToString()
                             + "|spid:" + spid
                             + "|fund_code:" + fund_code
                             + "|total_fee:" + ViewState["total_fee"].ToString()
                             + "|bind_serialno:" + ViewState["bind_serialno"].ToString()
                             + "|card_tail:" + ViewState["card_tail"].ToString()
                             + "|bank_type:" + ViewState["bank_type"].ToString()
                             + "|mobile:" + ViewState["mobile"].ToString();
                SunLibrary.LoggerFactory.Get("GetFundRatePageDetail").Info(memo);
                var total_fee_yaun = MoneyTransfer.FenToYuan(ViewState["total_fee"].ToString());

                string[,] param = new string[,] {  //{ "close_flag", "1" },
                                { "uin", ViewState["uin"].ToString() }, 
                                { "spid", spid },
                                { "total_fee", ViewState["total_fee"].ToString() },
                                { "fund_code", fund_code },
                                { "channel_id","stat_type=68|fm_6_qs_2"},
                                { "bind_serialno", ViewState["bind_serialno"].ToString() },
                                { "card_tail", ViewState["card_tail"].ToString() },
                                { "bank_type",ViewState["bank_type"].ToString() },
                                { "cur_type","1" },
                                { "redem_type", "1" },//赎回类型 1 t+0  2 t+1
                                { "client_ip", this.fRate_client_ip.Text.Trim()},
                                { "mobile",ViewState["mobile"].ToString() },
                                { "operator",Session["uid"].ToString() },
                                { "ReturnUrl",ReturnUrl},
                               };
                FundService fundBLLService = new FundService();
                var checktype = "FundBalanceRedeem";
                if (fundBLLService.isInsurance(fund_code, spid))  //如果保险  多加字段
                {
                    var temp = new string[,]
                    {
                        {"close_id" , ViewState["close_id"].ToString()},
                        {"real_amt_fee" , ViewState["total_fee"].ToString()}
                    };
                    var tempAll = new string[param.GetLength(0) + temp.GetLength(0), 2];
                    Array.Copy(param, tempAll, param.Length);
                    Array.Copy(temp, 0, tempAll, param.Length, temp.Length);
                    param = tempAll;
                    checktype = "InsuranceBalance";
                }

                Check_WebService.Param[] pa = PublicRes.ToParamArray(param);
                PublicRes.CreateCheckService(this).StartCheck(objid, checktype, memo, "0", pa);
                this.ShowMsg("已提交，请等待审批！");
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "提交申请单失败：" + PublicRes.GetErrorMsg(eSys.ToString()));
                return;
            }
        }

        //创建指数基金额强赎
        public void CreateFRateApply()
        {
            fRate_table.Visible = true;
            fRate_uin.Text = ViewState["uin"] as string;
            fRate_spid.Text = ViewState["spid"] as string;
            fRate_fund_code.Text = ViewState["fund_code"] as string;
            fRate_total_fee.Text = ViewState["total_fee"] as string;
            fRate_bind_serial_no.Text = ViewState["bind_serialno"] as string;
            fRate_crad_tail.Text = ViewState["card_tail"] as string;
            fRate_bank_type.Text = ViewState["bank_type"] as string;
            fRate_mobile.Text = ViewState["mobile"] as string;
            this.fRate_client_ip.Text = Request.UserHostAddress == "::1" ? "127.0.0.1" : Request.UserHostAddress;
        }

        //指数基金强赎 账务系统查看
        public void bindFRateApply(string objid)
        {
            try
            {
                DataTable dt = checkService.GetCheckInfo(objid, "FundBalanceRedeem");
                if (dt != null && dt.Rows.Count != 0)
                {
                    fRate_uin.Text = dt.Rows[0]["uin"].ToString();
                    fRate_spid.Text = dt.Rows[0]["spid"].ToString();
                    fRate_fund_code.Text = dt.Rows[0]["fund_code"].ToString();
                    fRate_total_fee.Text = dt.Rows[0]["total_fee"].ToString();
                    fRate_bind_serial_no.Text = dt.Rows[0]["bind_serialno"].ToString();
                    fRate_crad_tail.Text = dt.Rows[0]["card_tail"].ToString();
                    fRate_mobile.Text = dt.Rows[0]["mobile"].ToString();
                    fRate_bank_type.Text = dt.Rows[0]["bank_type"].ToString();
                    fRate_client_ip.Text = dt.Rows[0]["client_ip"].ToString();
                    fRate_table.Visible = true;
                    Button1.Visible = false;
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "查询出错！");
                    return;
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "申请单查询失败！" + PublicRes.GetErrorMsg(eSys.ToString()));
                return;
            }
        }

        //到期申购/赎回策略 预览
        private void AlterEndStrategyPreview()
        {
            try
            {
                title.InnerText = "封闭基金到期策略修改";
                Func<string, string> GetQS = (key) =>
                {
                    var v = (Request.QueryString[key] ?? "").Trim();
                    if (v == "")
                    {
                        throw new Exception(key + " 参数不可为空");
                    }
                    ViewState[key] = v;
                    return v;
                };
                var uin = GetQS("uin");
                var Trade_id = GetQS("trade_id");
                var Fund_code = GetQS("fund_code");
                var Close_listid = GetQS("close_listid");
                var user_end_type = GetQS("user_end_type");
                var end_sell_type = GetQS("end_sell_type");
                var fund_name = setConfig.convertBase64(GetQS("fund_name").Replace("%3D", "="));
                //var client_ip = Request.UserHostAddress.ToString();
                //if (client_ip == "::1") client_ip = "127.0.0.1";

                AlterES_uin.Text = uin;
                AlterES_Trade_id.Text = Trade_id;
                AlterES_Fund_code.Text = Fund_code;
                AlterES_user_end_type.Value = user_end_type;
                AlterES_end_sell_type.Value = end_sell_type;
                AlterES_fund_name.Text = fund_name;

                AlterES_table.Visible = true;
            }
            catch (Exception ex)
            {
                ShowMsg(PublicRes.GetErrorMsg(ex.ToString()));
            }
        }

        //到期申购/赎回策略 修改
        protected void AlterEndStrategy_Click(object sender, EventArgs e)
        {
            try
            {
                FundService fs = new FundService();
                var trade_id = ViewState["trade_id"].ToString();
                var fund_code = ViewState["fund_code"].ToString();
                var close_listid = long.Parse(ViewState["close_listid"].ToString());
                var user_end_type = int.Parse(AlterES_user_end_type.Value);
                var end_sell_type = int.Parse(AlterES_end_sell_type.Value);

                var client_ip = Request.UserHostAddress.ToString();
                if (client_ip == "::1") client_ip = "127.0.0.1";

                var isSucceed = fs.AlterEndStrategy(trade_id, fund_code, close_listid, user_end_type, end_sell_type, client_ip);
                if (isSucceed)
                {
                    ShowMsg("修改成功");
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "修改失败: ");
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "修改出错: " + PublicRes.GetErrorMsg(ex.ToString()));
            }
        }
    }
}