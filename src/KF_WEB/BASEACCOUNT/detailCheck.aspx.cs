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
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.C2C.Finance.DataAccess;
//using TENCENT.OSS.C2C.Finance.Finance_Web.classLibrary;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Check_WebService;
using TENCENT.OSS.CFT.KF.DataAccess;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// detailCheck 的摘要说明。
    /// </summary>
    public partial class detailCheck : System.Web.UI.Page
    {

        string id, strType, uid, exedSign;   //exedSign表示是否执行的标志 ,checkResult表示是否同意，
        public string sign, right, checkResult, objID, checkType, returnUrl;
        protected System.Data.DataSet dsDetail;
        protected System.Web.UI.WebControls.Label lbStartUser;
        protected System.Web.UI.WebControls.Label lbStartTime;
        protected System.Web.UI.WebControls.Label lbType;
        protected System.Web.UI.WebControls.Label lbTotalAccount;
        protected System.Web.UI.WebControls.Label lbState;
        protected System.Web.UI.WebControls.Label lbUid;
        protected System.Web.UI.WebControls.Label lbTime;
        protected System.Web.UI.WebControls.Label lbCheckLevel;
        protected System.Web.UI.WebControls.Label lbCLevel;
        protected System.Web.UI.WebControls.TextBox txSuguest;
        protected System.Web.UI.WebControls.Button btPass;
        protected System.Web.UI.WebControls.Button btRefuse;
        protected System.Web.UI.WebControls.Button btExeTask;
        protected System.Web.UI.WebControls.LinkButton lnkbtnObjID;
        protected System.Web.UI.WebControls.TextBox txtReason;
        protected System.Web.UI.WebControls.Button btsynFail;
        protected System.Web.UI.WebControls.LinkButton lnkbtnDetail;
        protected System.Web.UI.WebControls.HyperLink hplkDetail;
        protected System.Web.UI.WebControls.HyperLink hylkObjID;
        protected System.Web.UI.WebControls.Button btFail;
        protected System.Web.UI.WebControls.Button btnReturn;
        protected System.Data.DataTable dtDetail;

        private void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            if (Session["uid"] == null)
                Response.Write("<script language=javascript>window.parent.location='../login.aspx?wh=1'</script>");
            else
                uid = Session["uid"].ToString();

            if (!Page.IsPostBack)
            {
                //btRefuse.Attributes["onClick"] = "if(!confirm('确定拒绝该审批请求吗？')) return false;";
                //btFail.Attributes["onClick"] = "if(!confirm('确定撤销该任务吗？')) return false;";

                id = Request.QueryString["id"].ToString();
                strType = Request.QueryString["type"].ToString();
                sign = Request.QueryString["sign"].ToString();
                right = Request.QueryString["right"].ToString();

                ViewState["id"] = id;
                ViewState["strType"] = strType;
                ViewState["sign"] = sign; //check代表已审批，uncheck代表未审批
                ViewState["right"] = right; //notice代表我关注的 false 代表发起的 true 代表处理的

                BindData();
            }
            else
            {
                id = ViewState["id"].ToString();                 //如果刷新 会保持状态
                strType = ViewState["strType"].ToString();
                sign = ViewState["sign"].ToString();
                right = ViewState["right"].ToString();
            }

            //furion add
            if (right == "notice" || right == "query")
            {
                this.btExeTask.Visible = false;
                this.btFail.Visible = false;
                this.btPass.Visible = false;
                this.btRefuse.Visible = false;
                this.btsynFail.Visible = false;
            }

        }

        private void BindData()
        {

            Check_Service cs = new Check_Service();
            if (sign == "uncheck")          //未处理的审批
            {
                if (right == "false")      //察看自己的审批，没有权限修改
                    this.dtDetail = cs.GetStartCheckData(strType, uid.Trim().ToLower(), 1, 6, id);
                //else if (right == "true")  //处理审批，有修改权限
                //    this.dtDetail = DoCheck.GetStartCheckData(strType, uid.Trim().ToLower(), 1, 6, id, "0");

                //else if (right == "notice") //furion add
                //    this.dtDetail = NoticeCheck.GetStartCheckData(strType, uid.Trim().ToLower(), 1, 6, id);
            }
            else if (sign == "checked") //已处理的审批
            {
                if (right == "false")
                    this.dtDetail = cs.GetFinishCheckData(strType, uid.Trim().ToLower(), 1, 6, id);
                //else if (right == "true")
                //    this.dtDetail = cs.DoCheck.GetFinishCheckData(strType, uid.Trim().ToLower(), 1, 6, id, "0");

                //else if (right == "notice") //furion add
                //    this.dtDetail = NoticeCheck.GetFinishCheckData(strType, uid.Trim().ToLower(), 1, 6, id);

                //else if (right == "query") //furion add
                //    this.dtDetail = QUeryCheck.BindOneCheck(id);
            }

            //检查入口参数的合法性
            if (dtDetail == null || dtDetail.Rows.Count == 0)
            {
                Response.Write("<font color = red>读取数据错误！请联系管理员。</font>");
            }
            else
            {
                //绑定页面的基本信息
                string accTime = this.dtDetail.Rows[0]["zwTime"].ToString().Trim();
                returnUrl = this.dtDetail.Rows[0]["returnUrl"].ToString().Trim();
                string batLstID = this.dtDetail.Rows[0]["batLstID"].ToString().Trim();
                ViewState["batLstID"] = batLstID;
                string banktype = this.dtDetail.Rows[0]["banktype"].ToString().Trim();
                ViewState["banktype"] = banktype;

                if (accTime != null && accTime != "")
                {
                    ViewState["accTime"] = accTime.Replace("-", "").Substring(0, 8);
                }

                this.lbStartUser.Text = this.dtDetail.Rows[0]["FstartUser"].ToString().Trim();
                this.lbStartTime.Text = this.dtDetail.Rows[0]["FStartTime"].ToString().Trim();
                this.lbType.Text = this.dtDetail.Rows[0]["FTypeName"].ToString().Trim();
                this.lbTotalAccount.Text = this.dtDetail.Rows[0]["FcheckMoney"].ToString().Trim();
                this.lbState.Text = this.dtDetail.Rows[0]["Fstate"].ToString().Trim();
                string reason = this.dtDetail.Rows[0]["FcheckMemo"].ToString().Trim();
                this.txtReason.Text = reason;

                this.lbUid.Text = Session["uid"].ToString();
                this.lbTime.Text = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");

                this.lbCheckLevel.Text = this.dtDetail.Rows[0]["FCheckLevel"].ToString().Trim();
                this.lbCLevel.Text = this.dtDetail.Rows[0]["FCurrLevel"].ToString().Trim();

                ViewState["fid"] = this.dtDetail.Rows[0]["FID"].ToString().Trim();

                checkResult = this.dtDetail.Rows[0]["FcheckResult"].ToString();
                checkType = this.dtDetail.Rows[0]["FcheckType"].ToString().Trim();

                ViewState["checkType"] = checkType;
                ViewState["czType"] = this.dtDetail.Rows[0]["czType"].ToString().Trim();

                ////判读是否显示“执行任务”按钮
                //exedSign = this.dtDetail.Rows[0]["Fstate"].ToString();
                //if (exedSign.Trim() == "<font color = red>审批完成</font>")   //转义后的判断，只能用中文
                //{
                //    this.btExeTask.Visible = true;

                //    //if ("realtimeadjust" == strType.Trim().ToLower())  //实时调帐目前是可撤销的
                //    //this.btFail.Visible    = true;
                //}

                //furion 20060407 新加撤消功能 只要是发起人都可以撤消.
                //if (right == "false" && exedSign != "已执行" && exedSign != "已撤消")
                //{
                //    this.btFail.Visible = true;
                //}
                //end


                objID = this.dtDetail.Rows[0]["FObjId"].ToString().Trim();

                //如果是请求交易，确认付款，退款处理，则允许有同步失败状态
                //				string sSign = reason.Substring(reason.Length-2,1);   //取到类型表示,如果是1或者2,则是c2c同步调帐 
                //				if ((sSign == "1"|| sSign == "2") && (reason.IndexOf("=>") !=-1)  && (checkType == "fmjy02" || checkType == "fmfk03" || checkType == "fmtk04") && (exedSign.Trim() == "<font color = red>审批完成</font>"))  //转义后的判断，只能用中文
                //					this.btsynFail.Visible = true;
                //				else
                //					this.btsynFail.Visible = false;
                //if (right == "query")
                //{
                //    //furion 20060630  增加执行中回退到审批完成的功能
                //    if (this.dtDetail.Rows[0]["FstateName"].ToString().Trim() == "执行中")
                //    {
                //        btnReturn.Visible = true;
                //    }
                //    //
                //    checkResult = this.dtDetail.Rows[0]["FcheckResult"].ToString();
                //    this.lbCheckLevel.Text = this.dtDetail.Rows[0]["FCheckLevelName"].ToString().Trim();
                //    this.lbCLevel.Text = this.dtDetail.Rows[0]["FCurrLevelName"].ToString().Trim();
                //    this.lbState.Text = this.dtDetail.Rows[0]["FstateName"].ToString().Trim();
                //}

               // BindHplkUrl();
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
            this.dsDetail = new System.Data.DataSet();
            this.dtDetail = new System.Data.DataTable();
            ((System.ComponentModel.ISupportInitialize)(this.dsDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDetail)).BeginInit();
            //this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            //this.btPass.Click += new System.EventHandler(this.btPass_Click);
            //this.btRefuse.Click += new System.EventHandler(this.btRefuse_Click);
            //this.btExeTask.Click += new System.EventHandler(this.btExeTask_Click);
            //this.btFail.Click += new System.EventHandler(this.btFail_Click);
            //this.btsynFail.Click += new System.EventHandler(this.btsynFail_Click);
            // 
            // dsDetail
            // 
            this.dsDetail.DataSetName = "NewDataSet";
            this.dsDetail.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.dsDetail.Tables.AddRange(new System.Data.DataTable[] {
																		  this.dtDetail});
            // 
            // dtDetail
            // 
            this.dtDetail.TableName = "dtDetail";
            this.Load += new System.EventHandler(this.Page_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dsDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDetail)).EndInit();

        }
        #endregion

//        private void check(int icheckResult)
//        {
//            if (Session["uid"] == null)
//            {
//                Response.Write("<font color = red>登陆超时！请重新登陆。</font>");
//            }
//            else
//            {
//                try
//                {
//                    Check_WebService.Check_Service cw = new Check_WebService.Check_Service();
//                    Check_WebService.Finance_Header fh = new Check_WebService.Finance_Header();
//                    fh.UserName = Session["uid"].ToString();
//                    fh.UserIP = Request.UserHostAddress;
//                    if (ViewState["checkType"] != null && ViewState["checkType"].ToString() != "CNBankTreasurer") //出纳资金调拨从审批系统传来
//                    {
//                        fh.SzKey = Session["SzKey"].ToString();
//                        fh.UserPassword = Session["pwd"].ToString();
//                        fh.OperID = Int32.Parse(Session["OperID"].ToString());
//                        fh.RightString = Session["key"].ToString();
//                    }


//                    cw.Finance_HeaderValue = fh;

//                    string memo = setConfig.replaceSqlStr(this.txSuguest.Text.ToString());
//                    cw.DoCheck(id, memo, icheckResult);
//                }
//                catch (SoapException er) //捕获soap类
//                {
//                    string str = PublicRes.GetErrorMsg(er.Message.ToString());
//                    throw new Exception("审批失败：" + str);
//                }
//                catch (Exception emsg)
//                {
//                    throw new Exception("审批失败！" + emsg.Message.ToString().Replace("'", "’"));
//                }
//            }
//        }

//        private void btPass_Click(object sender, System.EventArgs e)
//        {
//            try
//            {
//                #region rowena   20090902   系判判断任务单若为经办人，不可以同时审批人(现付款除外)
//                if (!NotCheckLimitBank())
//                {
//                    return;
//                }
//                #endregion

//                //c2c批量转账的，审批前先校验一下
//                if (ViewState["checkType"] != null && ViewState["checkType"].ToString() == "batchc2ctransfer")
//                {
//                    string checkMsg = "";
//                    BatchPay_Service.BatchPay_Service bs = new TENCENT.OSS.C2C.Finance.Finance_Web.BatchPay_Service.BatchPay_Service();
//                    bool checkFalg = bs.CheckBatchC2CTransfer(this.hylkObjID.Text.Trim(), out checkMsg);
//                    if (!checkFalg)
//                    {
//                        WebUtils.ShowMessage(this.Page, "审批操作失败！原因：" + checkMsg);
//                        return;
//                    }

//                }
//                //退票，审批前先上传文件校验下
//                else if (ViewState["checkType"] != null && ViewState["checkType"].ToString() == "returnticket")
//                {
//                    string checkMsg = "";
//                    BatchPay_Service.BatchPay_Service bs = new TENCENT.OSS.C2C.Finance.Finance_Web.BatchPay_Service.BatchPay_Service();
//                    bool checkFalg = bs.CheckReturnticket(this.hylkObjID.Text.Trim(), out checkMsg);
//                    if (!checkFalg)
//                    {
//                        WebUtils.ShowMessage(this.Page, "审批操作失败！原因：" + checkMsg);
//                        return;
//                    }
//                }
//                //b2c/fastpya，审批前先上传文件校验下
//                else if (ViewState["checkType"] != null && ViewState["checkType"].ToString() == "batchrefundapply")
//                {
//                    string checkMsg = "";
//                    BatchPay_Service.BatchPay_Service bs = new TENCENT.OSS.C2C.Finance.Finance_Web.BatchPay_Service.BatchPay_Service();
//                    bool checkFalg = bs.CheckBatchRefund(this.hylkObjID.Text.Trim(), out checkMsg);
//                    if (!checkFalg)
//                    {
//                        WebUtils.ShowMessage(this.Page, "审批操作失败！原因：" + checkMsg);
//                        return;
//                    }
//                }
//                //红包发放审批前校验判断
//                else if (ViewState["checkType"] != null && ViewState["checkType"].ToString() == "HongBaoSend")
//                {
//                    string checkMsg = "";
//                    BatchPay_Service.BatchPay_Service bs = new TENCENT.OSS.C2C.Finance.Finance_Web.BatchPay_Service.BatchPay_Service();
//                    bool checkFalg = bs.CheckHONGBAO(this.hylkObjID.Text.Trim(), out checkMsg);
//                    if (!checkFalg)
//                    {
//                        WebUtils.ShowMessage(this.Page, "红包审批操作失败！原因：" + checkMsg);
//                        return;
//                    }
//                }
//                else if (ViewState["checkType"] != null && (ViewState["checkType"].ToString() == "remitfetch"
//                    || ViewState["checkType"].ToString() == "remitrefund" || ViewState["checkType"].ToString() == "remitmodify"))
//                {
//                    string checkMsg = "";
//                    BatchPay_Service.BatchPay_Service bs = new TENCENT.OSS.C2C.Finance.Finance_Web.BatchPay_Service.BatchPay_Service();
//                    bool checkFalg = bs.CheckRemitWater(this.hylkObjID.Text.Trim(), out checkMsg);
//                    if (!checkFalg)
//                    {
//                        WebUtils.ShowMessage(this.Page, "邮储汇款审批操作失败！原因：" + checkMsg);
//                        return;
//                    }
//                }
//                //赔付校验
//                else if (ViewState["checkType"] != null && ViewState["checkType"].ToString() == "RefundCompensation")
//                {
//                    string checkMsg = "";
//                    BatchPay_Service.BatchPay_Service bs = new TENCENT.OSS.C2C.Finance.Finance_Web.BatchPay_Service.BatchPay_Service();
//                    bool checkFalg = bs.CheckRefundCompensation(this.hylkObjID.Text.Trim(), out checkMsg);
//                    if (!checkFalg)
//                    {
//                        WebUtils.ShowMessage(this.Page, "审批操作失败！原因：" + checkMsg);
//                        return;
//                    }
//                }
//                //新华线下转账
//                else if (ViewState["checkType"] != null && ViewState["checkType"].ToString() == "XinHuaOffPayCheck")
//                {
//                    string checkMsg = "";
//                    BatchPay_Service.BatchPay_Service bs = new TENCENT.OSS.C2C.Finance.Finance_Web.BatchPay_Service.BatchPay_Service();
//                    bool checkFalg = bs.CheckXinHuaOffPay(this.hylkObjID.Text.Trim(), out checkMsg);
//                    if (!checkFalg)
//                    {
//                        WebUtils.ShowMessage(this.Page, "审批操作失败！原因：" + checkMsg);
//                        return;
//                    }
//                }

//                check(0); //审批完成  0表示通过审批
//                //Response.Write("<script>alert('您同意了这个审批请求！');</script>");
//                if (ViewState["checkType"] != null && ViewState["checkType"].ToString() == "CNBankTreasurer")
//                {
//                    string strUserID = Session["uid"].ToString().Trim().ToLower();
//                    Response.Write("<script language=javascript>window.parent.location='DoCheck.Aspx?paycheck=true&&paycheckuid=" + strUserID + "'</script>");
//                }
//                else
//                {
//                    Response.Write("<script language=javascript>window.parent.location='DoCheck.Aspx'</script>");   //跳转到详细的任务界面
//                }
//            }
//            catch (SoapException er) //捕获soap类
//            {
//                //执行审批失败，则需要再次发起审批，否则，该交易单则一直处于审批状态，无法对其进行其它操作。
//                //如果是确认付款或者退款
//                if (ViewState["checkType"].ToString() == "fmfk03" || ViewState["checkType"].ToString() == "fmtk04")
//                {
//                    this.btsynFail.Visible = true;
//                }
//                else   //如果是其他类型
//                {
//                    //furion 20060407 为什么在这里开始呢?
//                    //this.btFail.Visible = true;
//                }

//                string str = PublicRes.GetErrorMsg(er.Message.ToString());
//                ViewState["errMsg"] = str;
//                WebUtils.ShowMessage(this.Page, str);
//            }
//            catch (Exception em)
//            {
//                WebUtils.ShowMessage(this.Page, "审批操作失败！原因：" + em.Message.ToString());
//            }
//        }
//        //rowena   20090902   系判判断任务单若为经办人，不可以同时审批人(现付款除外)
//        private bool NotCheckLimitBank()
//        {
//            if (Session["uid"] == null)
//            {
//                Response.Write("<font color = red>登陆超时！请重新登陆。</font>");
//                return false;
//            }
//            else
//            {
//                string checktype = ViewState["checkType"].ToString().Trim().ToLower();
//                string conchecktype = System.Configuration.ConfigurationSettings.AppSettings["NotCheckLimitBank"].Trim().ToLower();

//                if (conchecktype.IndexOf(checktype) > -1)
//                {
//                    return true;
//                }

//                string startuser = this.lbStartUser.Text.Trim().ToLower();
//                if (startuser == "" || startuser == null)
//                {
//                    WebUtils.ShowMessage(this.Page, "申请人为空");
//                    return false;
//                }
//                string strUserID = Session["uid"].ToString().Trim().ToLower();
//                if (strUserID.Equals(startuser))
//                {
//                    WebUtils.ShowMessage(this.Page, "经办人不能为审批人！");
//                    return false;
//                }
//                return true;
//            }
//        }
//        private void btRefuse_Click(object sender, System.EventArgs e)
//        {
//            string objId = null;
//            try
//            {

//                #region rowena   20090902   系判判断任务单若为经办人，不可以同时审批人(现付款除外)
//                if (!NotCheckLimitBank())
//                {
//                    return;
//                }
//                #endregion



//                //此处待整理 实在是乱。 ray 20051101
//                if (ViewState["checkType"].ToString() == "settle")  //处理商户入帐核算失败的动作。更新状态为1；
//                {
//                    string msg = this.lbCLevel.Text + "拒绝了商户入帐结算的请求![" + this.hylkObjID.Text + "].原因：" + this.txSuguest.Text.ToString();
//                    //setSettleSign
//                    Finance_ManageService.Finance_Manage fm = new Finance_ManageService.Finance_Manage();

//                    string Msg = null;
//                    objId = this.hylkObjID.Text.Trim();

//                    if (!fm.setSettleSign(objId, out Msg))
//                    {
//                        throw new Exception(Msg);
//                    }
//                }

//                check(1);//审批未通过  1 表示拒绝审批	

//                string batLstID = null;
//                if (ViewState["batLstID"] != null)
//                    batLstID = ViewState["batLstID"].ToString();

//                string strCheckType = ViewState["checkType"].ToString();

//                if (batLstID.IndexOf("#") != -1 && strCheckType == "fmcz01") //如果是多笔的充值 
//                {
//                    if (batLstID.IndexOf("@") != -1 && strCheckType == "fmcz01")
//                    {
//                        int iBegin = batLstID.IndexOf("@");
//                        int iEnd = batLstID.LastIndexOf("@");
//                        int iSpan = iEnd - iBegin - 1;

//                        string backID = batLstID.Substring(iBegin + 1, iSpan);  //取到交易单号；

//                        string[] ar = backID.Split(',');

//                        string type = ViewState["czType"].ToString();
//                        string sign = null;

//                        if (type == "normal")   //根据状态，置状态
//                        {
//                            sign = "1";  //充值
//                        }
//                        else if (type == "czPay")
//                        {
//                            sign = "3";  //充值支付
//                        }

//                        int iSign = Int32.Parse(sign);
//                        Finance_ManageService.Finance_Manage fm = new Finance_ManageService.Finance_Manage();
//                        string banktype = ViewState["banktype"].ToString().Trim();
//                        fm.updateBatCheck(ar, iSign, ViewState["accTime"].ToString(), banktype);  //1表示需要充值调整

//                    }
//                    else
//                    {
//                        //没有银行返回的订单号，更新失败
//                        Response.Write("<script>alert('错误！没有返回的银行订单号。更新状态失败！');</script>");
//                    }
//                }

//                //此处需要考虑事务，失败了怎么办？ 会有什么影响。此处需要进一步考虑。此处存在风险
//                string listID = this.hylkObjID.Text.Trim();     //交易单号
//                string reason = this.txtReason.Text.ToString(); //原因

//                Manage_Service.Manage_Service ms = new Manage_Service.Manage_Service(); //同步失败给C2C
//                if (ViewState["checkType"].ToString() == "fmfk03")
//                {
//                    string msg = this.lbCLevel.Text + "拒绝了交易单" + listID + "的确认付款请求！ 原因：" + this.txSuguest.Text.ToString();
//                    ms.DoConfirmFail(listID, msg, reason);
//                }
//                else if (ViewState["checkType"].ToString() == "fmtk04")
//                {
//                    string msg = this.lbCLevel.Text + "拒绝了交易单" + listID + "的退款请求！ 原因：" + this.txSuguest.Text.ToString();
//                    ms.DoRefoundFail(listID, msg, reason);
//                }

//                if (ViewState["checkType"].ToString() == "otherAdjust")
//                {
//                    string Msg = null;
//                    //ms.updateBankResultSign(id,out Msg);
//                    //失败，更新对帐异常表为未处理
//                    string fstate = "0"; //如果失败，更新状态为未处理
//                    string memo = "审批被拒绝！";
//                    Finance_ManageService.Finance_Manage fm = new Finance_ManageService.Finance_Manage();

//                    //根据调帐的objID 获得对应的异常挂帐表的kid
//                    string kid = "";
//                    if (!fm.getKid(this.hylkObjID.Text.Trim(), out kid, out Msg))
//                    {
//                        WebUtils.ShowMessage(this.Page, "获取挂帐表KID失败！");
//                        return;
//                    }

//                    if (!fm.updateExcepState(kid, fstate, memo, out Msg))
//                    {
//                        WebUtils.ShowMessage(this.Page, "更新挂帐异常表失败！");
//                        return;
//                    }
//                }

//                Response.Write("<script>alert('您不同意此审批请求！');</script>");
//                Response.Write("<script language=javascript>window.parent.location='DoCheck.Aspx'</script>");   //跳转到详细的任务界面
//            }
//            catch (Exception em)
//            {
//                WebUtils.ShowMessage(this.Page, "审批操作失败！原因：" + em.Message.ToString());
//            }
//        }

//        private void btExeTask_Click(object sender, System.EventArgs e)
//        {
//            if (Session["uid"] == null)
//            {
//                Response.Write("<script language=javascript>window.parent.location='../login.aspx?wh=1'</script>");
//                return;
//            }

//            try
//            {
//                Check_WebService.Check_Service cw = new Check_WebService.Check_Service();
//                Check_WebService.Finance_Header fh = new Check_WebService.Finance_Header();
//                fh.UserName = Session["uid"].ToString();
//                fh.UserIP = Request.UserHostAddress;

//                if (ViewState["checkType"] != null && ViewState["checkType"].ToString() != "CNBankTreasurer") //出纳资金调拨从审批系统传来
//                {

//                    fh.UserPassword = Session["pwd"].ToString();
//                    fh.SzKey = Session["SzKey"].ToString();
//                    fh.OperID = Int32.Parse(Session["OperID"].ToString());
//                    fh.RightString = Session["key"].ToString();
//                }
//                cw.Finance_HeaderValue = fh;

//                cw.ExecuteCheck(ViewState["fid"].ToString());  //传入审批的ID
//                Response.Write("<script>alert('执行成功！');</script>");
//                if (ViewState["checkType"] != null && ViewState["checkType"].ToString() == "CNBankTreasurer")
//                {
//                    string strUserID = Session["uid"].ToString().Trim().ToLower();
//                    Response.Write("<script language=javascript>window.parent.location='StartCheck.Aspx?paycheck=true&&paycheckuid=" + strUserID + "'</script>");
//                }
//                else
//                {
//                    Response.Write("<script language=javascript>window.parent.location='StartCheck.aspx'</script>");   //跳转到详细的任务界面
//                }
//            }
//            catch (SoapException er) //捕获soap类
//            {
//                //执行审批失败，则需要再次发起审批，否则，该交易单则一直处于审批状态，无法对其进行其它操作。
//                string str = PublicRes.GetErrorMsg(er.Message.ToString());
//                ViewState["errMsg"] = str;

//                WebUtils.ShowMessage(this.Page, str);

//                //如果是确认付款或者退款
//                if (ViewState["checkType"].ToString() == "fmfk03" || ViewState["checkType"].ToString() == "fmtk04")
//                {
//                    this.btsynFail.Visible = true;
//                }
//                else   //如果是其他类型
//                {
//                    this.btFail.Visible = true;
//                }
//            }
//            catch (Exception emsg)
//            {
//                //					Response.Write("<script>alert('执行失败！');</script>");
//                WebUtils.ShowMessage(this.Page, PublicRes.GetErrorMsg(emsg.Message.ToString().Replace("'", "’")));
//                this.btFail.Visible = true;
//            }
//        }

//        private void lnkbtnObjID_Click(object sender, System.EventArgs e)
//        {
//            //Response.Write("<script language=javascript>window.parent.location='../ACCOUNTMANAGE/AdjustDepositMoney.aspx?id=" + this.lnkbtnObjID.Text.Trim() + "'</script>"); 
//            //			Response.Redirect("../ACCOUNTMANAGE/AdjustDepositMoney.aspx?id=" + this.lnkbtnObjID.Text.Trim());
//        }

//        private void Button1_Click(object sender, System.EventArgs e)
//        {
//            try
//            {

//                string listID = this.hylkObjID.Text.Trim();     //交易单号
//                string reason = this.txtReason.Text.ToString(); //原因

//                Manage_Service.Manage_Service ms = new Manage_Service.Manage_Service(); //同步失败给C2C
//                string msg = "执行请求的任务时候出错！[" + ViewState["errMsg"].ToString().Replace("'", "’") + "]";
//                if (ViewState["checkType"].ToString() == "fmfk03")
//                {
//                    ms.DoConfirmFail(listID, msg, reason);
//                }
//                else if (ViewState["checkType"].ToString() == "fmtk04")
//                {
//                    ms.DoRefoundFail(listID, msg, reason);
//                }

//                this.btsynFail.Visible = false;
//                this.btExeTask.Visible = false;

//                //更新该审批为结束状态，否则用户会再次发起这个执行任务，导致出现不一致的情况
//                //所有执行不了的都应该有一个功能是撤销该审批

//                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
//                try
//                {
//                    da.OpenConn();
//                    //da.ExecSql("update c2c_fmdb.t_check_main set FState='finished' where FID=" + ViewState["fid"].ToString().Trim());
//                    da.ExecSql("update c2c_fmdb.t_check_main set FNewState=3 where FID=" + ViewState["fid"].ToString().Trim());
//                }
//                finally
//                {
//                    da.Dispose();
//                }
//            }
//            catch (SoapException er)
//            {
//                string str = PublicRes.GetErrorMsg(er.Message.ToString());
//                WebUtils.ShowMessage(this.Page, str);
//            }
//        }

//        private void LinkButton1_Click(object sender, System.EventArgs e)
//        {

//        }

//        /// <summary>
//        /// 处理审批的详细页面，供审批者参考,根据不同的情况，跳转到不同的页面
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void BindHplkUrl()
//        {
//            //			string aReason = this.txtReason.Text;
//            string batLstID = ViewState["batLstID"].ToString();

//            string sCheckType = ViewState["checkType"].ToString();

//            this.hylkObjID.Text = objID;
//            string url = null;

//            if (sCheckType.Trim() == "Mediation")
//            {
//                url = "../BaseAccount/" + returnUrl + "&posi=check";
//            }
//            else if (batLstID.IndexOf("#") == -1 && sCheckType == "fmcz01")      //如果是单笔的充值
//            {
//                url = "../ACCOUNTMANAGE/AdjustDepositMoney.aspx?id=" + this.hylkObjID.Text.Trim();
//            }
//            else if (batLstID.IndexOf("#") != -1 && sCheckType == "fmcz01") //如果是多笔的充值 
//            {
//                string reason = ViewState["batLstID"].ToString();//setConfig.convertToBase64(this.txtReason.Text.Trim())
//                string[] ar = reason.Split('#');
//                reason = ar[2]; //取得银行返回的订单号
//                reason = setConfig.convertToBase64(reason);
//                string banktype = ViewState["banktype"].ToString().Trim();
//                url = "../FinanceManage/batPayShow.aspx?reason=" + reason + "&accTime=" + ViewState["accTime"].ToString() + "&banktype=" + banktype;
//            }
//            else if (sCheckType == "batchpay")  //批量付款
//            {
//                url = "../BatchPay/ShowDetail.Aspx?BatchID=" + this.hylkObjID.Text.Trim() + "&pos=check";
//            }
//            else if (sCheckType == "fmfk03" || sCheckType == "fmtk04")//其他类型调帐
//            {
//                url = "../ACCOUNTMANAGE/AdjustDepositMoney.aspx?id=" + this.hylkObjID.Text.Trim();
//            }
//            else if (sCheckType == "mcTrans" || sCheckType == "NewCoinPub")
//            {
//                url = returnUrl;
//            }
//            else if (sCheckType == "settle")
//            {
//                string webPath = System.Configuration.ConfigurationSettings.AppSettings["webPath"].ToString();
//                url = webPath + returnUrl + "&pos=check";
//            }
//            else if (sCheckType.ToLower().Trim() == "mannalfetch")
//            {
//                this.hplkDetail.Enabled = false;
//                this.hylkObjID.Enabled = false;
//            }
//            else if (sCheckType == "bankexception" || sCheckType == "batchexception" || sCheckType == "twoexception" || sCheckType == "refundexception")
//            {
//                url = "../FinanceManage/BankDataErrorHandle_Detail.aspx?checkid=" + ViewState["id"].ToString();

//            }
//            else if (sCheckType == "subaccczpayexception" || sCheckType == "subacctxpayexception" || sCheckType == "Subacctwoexception" || sCheckType == "Subbatchexception")
//            {
//                url = "../FinanceManage/BankDataErrorHandle_Detail.aspx?checkid=" + ViewState["id"].ToString() + "&type=2";
//            }
//            else if (sCheckType == "changebankinfo")
//            {
//                url = "../BaseAccount/ChangeUserBankInfo.aspx?checkid=" + ViewState["id"].ToString();
//            }
//            else if (sCheckType == "KFmannalForRefund")
//            {
//                url = "../FinanceManage/KFmanualRefund.aspx?checkid=" + ViewState["id"].ToString().Trim(); ;
//            }

////			else if(sCheckType == "CNBankTreasurer") //20110428 出纳资金调拨  rowenawu
//            //			{
//            //				string paywebPath = System.Configuration.ConfigurationSettings.AppSettings["PayWebPath"].ToString();
//            //				url=paywebPath+returnUrl+"&checkid=" + ViewState["id"].ToString()+"&startuser="+this.lbStartUser.Text.Trim()+"&starttime="+this.lbStartTime.Text.Trim()+"&no="+this.hylkObjID.Text.Trim();
//            //			}
//            //解决重叠问题吧 furion 20050103
//            else if (returnUrl.Trim() == "")
//            {
//                url = "";
//                this.hplkDetail.Enabled = false;
//                this.hylkObjID.Enabled = false;
//            }
//            else  //冲正类等后来加入的
//            {
//                //returnUrl的格式不带路径，只待文件夹路径
//                string webPath = System.Configuration.ConfigurationSettings.AppSettings["webPath"].ToString();

//                if (!returnUrl.StartsWith("http"))
//                    url = webPath + returnUrl;
//                else
//                    url = returnUrl;
//            }

//            this.hplkDetail.NavigateUrl = url;
//            this.hylkObjID.NavigateUrl = url;
//        }

//        private void btsynFail_Click(object sender, System.EventArgs e)
//        {
//            try
//            {

//                string listID = this.hylkObjID.Text.Trim();     //交易单号
//                string reason = Common.CommLib.commRes.replaceSqlStr(this.txtReason.Text.ToString()); //原因

//                Manage_Service.Manage_Service ms = new Manage_Service.Manage_Service(); //同步失败给C2C
//                //string msg    = "执行请求的任务时候出错！[" + ViewState["errMsg"].ToString().Replace("'","’") + "]"; 
//                string msg = "执行请求的任务时候出错！[" + Common.CommLib.commRes.replaceSqlStr(ViewState["errMsg"].ToString()) + "]";

//                if (ViewState["checkType"].ToString() == "fmfk03")
//                {
//                    ms.DoConfirmFail(listID, msg, reason);
//                }
//                else if (ViewState["checkType"].ToString() == "fmtk04")
//                {
//                    ms.DoRefoundFail(listID, msg, reason);
//                }

//                this.btsynFail.Visible = false;
//                this.btExeTask.Visible = false;

//                //更新该审批为结束状态，否则用户会再次发起这个执行任务，导致出现不一致的情况
//                //所有执行不了的都应该有一个功能是撤销该审批

//                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
//                try
//                {
//                    da.OpenConn();
//                    //da.ExecSql("update c2c_fmdb.t_check_main set FState='finished' where FID=" + ViewState["fid"].ToString().Trim());
//                    da.ExecSql("update c2c_fmdb.t_check_main set FNewState=3 where FID=" + ViewState["fid"].ToString().Trim());
//                }
//                finally
//                {
//                    da.Dispose();
//                }
//            }
//            catch (SoapException er)
//            {
//                string str = PublicRes.GetErrorMsg(er.Message.ToString());
//                WebUtils.ShowMessage(this.Page, str);
//            }
//        }

//        private void btFail_Click(object sender, System.EventArgs e) //任务撤销,update审批任务为finish状态
//        {
//            string msg = null; //描述性信息
//            string strCheckID = ViewState["fid"].ToString();
//            Check_WebService.Check_Service cw = new Check_WebService.Check_Service();
//            bool sign = cw.RecallCheck(strCheckID, out msg);

//            if (sign == true)
//            {
//                WebUtils.ShowMessage(this.Page, msg);
//                this.btExeTask.Visible = false;
//                this.btFail.Visible = false;

//                //Common.CommLib.commRes.sendLog4Log("MakeFail", Session["uid"] + "撤销审批操作!审批ID:" + strCheckID + ",审批类型：" + ViewState["strType"].ToString());
//            }
//            else if (sign == false)
//            {
//                WebUtils.ShowMessage(this.Page, msg);
//            }
//        }

//        private void btnReturn_Click(object sender, System.EventArgs e)
//        {
//            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
//            try
//            {
//                da.OpenConn();
//                da.ExecSql("update c2c_fmdb.t_check_main set FNewState=2 where FID=" + ViewState["fid"].ToString().Trim() + " and FNewState=9");
//            }
//            catch (Exception err)
//            {
//                WebUtils.ShowMessage(this.Page, "审批状态变更时出错：" + PublicRes.GetErrorMsg(err.Message));
//            }
//            finally
//            {
//                da.Dispose();
//                BindData();
//            }
//        }
    }
}
