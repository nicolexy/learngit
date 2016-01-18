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
using System.Web.Mail;
using System.IO;
using System.Xml;
using CFT.CSOMS.BLL.SPOA;
using System.Collections.Generic;
using CFT.Apollo.Logging;


namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// ModifyBusinessInfo 的摘要说明。
    /// </summary>
    public partial class ModifyBusinessInfo : System.Web.UI.Page
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                //int operid = Int32.Parse(Session["OperID"].ToString());

                //if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");

                if (!IsPostBack)
                {
                    this.rbtBusiness.Checked = true;
                    this.Table1.Visible = false;
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
            this.dgList.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgList_PageIndexChanged);

        }
        #endregion

        private void CheckData()
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet dsSPOA, dsMain;
            if (this.rbtBusiness.Checked)          //判断是否是真的直付商户
            {
                dsSPOA = qs.GetPayBusinessInfoList(this.txtFspid.Text.Trim());

                if (dsSPOA != null && dsSPOA.Tables[0].Rows.Count != 1)
                {
                    throw new Exception("请输入正确直付商户号!");
                }
                else
                    dsMain = qs.GetBusinessInfoList(this.txtFspid.Text.Trim());
            }
            else                    //判断是否是真的中介商户
            {
                dsSPOA = qs.GetAgencyBusinessInfoList(this.txtFspid.Text.Trim());

                if (dsSPOA != null && dsSPOA.Tables[0].Rows.Count != 1)
                {
                    throw new Exception("请输入正确中介商户号!");
                }
                else
                    dsMain = qs.GetBusiness2InfoList(this.txtFspid.Text.Trim());
            }

            if (dsMain != null && dsMain.Tables[0].Rows.Count == 1)       //判断身份证后五位是否正确
            {

            }
            else
            {
                throw new Exception("该商户资料不存在!");
            }

            if (this.rbtBusiness.Checked)
            {
                //测试的时候注释掉
                DataSet ds = qs.GetBusinessBankList(this.txtFspid.Text.Trim());
                if (ds != null && ds.Tables[0].Rows.Count == 1)    //判断银行卡后五位是否正确
                {
                    string BankCard = ds.Tables[0].Rows[0]["Fbankid"].ToString().Trim();

                    if (BankCard != null && BankCard.Length >= 5)
                    {
                        if (BankCard.Substring(BankCard.Length - 5, 5) != this.txtBankCard.Text.Trim())
                            throw new Exception("银行卡验证不通过,请输入正确银行卡后五位!");
                        else
                        {

                            T_USER_MED ds1 = qs.GetUserMedInfo(this.txtFspid.Text.Trim(), 1, 1);

                            DataSet dsEmail = qs.GetBusinessEmail(this.txtFspid.Text.Trim());
                            this.lblFspName.Text = dsMain.Tables[0].Rows[0]["FspidName"].ToString().Trim();
                            this.lblEmail.Text = dsEmail.Tables[0].Rows[0]["Femail"].ToString().Trim();
                            this.lblAddress.Text = dsSPOA.Tables[0].Rows[0]["WWWAdress"].ToString().Trim();
                            this.lblMerKey.Text = ds1.fmer_key;
                            this.lblContactMobile.Text = dsSPOA.Tables[0].Rows[0]["ContactMobile"].ToString().Trim();
                            ViewState["Fspid"] = this.txtFspid.Text.Trim();
                        }
                    }
                    else
                    {
                        throw new Exception("该商户银行卡不存在!");
                    }
                }
                else
                {
                    throw new Exception("该商户银行绑定资料不存在!");
                }
            }
            else
            {
                this.lblMerKey.Text = "";
                this.txtFspName.Enabled = false;
                this.lblFspName.Text = "";
                this.lblEmail.Text = dsSPOA.Tables[0].Rows[0]["Femail"].ToString().Trim();
                this.lblAddress.Text = dsSPOA.Tables[0].Rows[0]["Fdomain"].ToString().Trim();
            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();

                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");

                if (!IsPostBack)
                {
                    this.rbtBusiness.Checked = true;
                }
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            try
            {
                this.btnSave.Enabled = true;
                if (this.rbtBusiness.Checked)
                    this.Linkbutton2.Visible = true;
                else
                    this.Linkbutton2.Visible = false;
                CheckData();
                this.txtFspName.Text = this.lblFspName.Text;
                this.txtAddress.Text = this.lblAddress.Text;
                this.txtEmail.Text = this.lblEmail.Text;
                this.txtContactMobile.Text = this.lblContactMobile.Text;
                this.Table1.Visible = true;
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                LogHelper.LogError("请求页面：" + Request.Url.AbsoluteUri + ",btnSearch_Click，捕获soap类异常，异常:" + eSoap.ToString());
                this.Table1.Visible = false;
                this.dgList.Visible = false;
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                LogHelper.LogError("请求页面：" + Request.Url.AbsoluteUri + ",btnSearch_Click，异常:" + eSys.ToString());
                this.Table1.Visible = false;
                this.dgList.Visible = false;
                WebUtils.ShowMessage(this.Page, eSys.Message);
            }
        }

        protected void btnBankCardCheck_Click(object sender, System.EventArgs e)
        {
            if (this.rbtBusiness.Checked)
            {
                try
                {
                    Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                    DataSet ds = qs.GetBusinessBankList(this.txtFspid.Text.Trim());

                    if (ds != null && ds.Tables[0].Rows.Count == 1)
                    {
                        string BankCard = ds.Tables[0].Rows[0]["Fbankid"].ToString().Trim();

                        if (BankCard != null && BankCard.Length >= 5)
                        {
                            if (BankCard.Substring(BankCard.Length - 5, 5) == this.txtBankCard.Text.Trim())
                                throw new Exception("银行卡验证通过!");
                            else
                                throw new Exception("银行卡验证不通过,请输入正确银行卡后五位!");
                        }
                        else
                        {
                            throw new Exception("该商户银行卡不存在!");
                        }
                    }
                    else
                    {
                        throw new Exception("该商户银行绑定资料不存在!");
                    }
                }
                catch (SoapException eSoap) //捕获soap类异常
                {
                    LogHelper.LogError("请求页面：" + Request.Url.AbsoluteUri + ",btnBankCardCheck_Click，捕获soap类异常，异常:" + eSoap.ToString());
                    string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                    WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
                }
                catch (Exception eSys)
                {
                    LogHelper.LogError("请求页面：" + Request.Url.AbsoluteUri + ",btnBankCardCheck_Click，异常:" + eSys.ToString());
                    WebUtils.ShowMessage(this.Page, eSys.Message);
                }
            }
        }

        private void BindData()
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds = qs.GetHisBusinessList(this.txtFspid.Text.Trim());
            dgList.DataSource = ds.Tables[0].DefaultView;
            dgList.DataBind();

            DataSet dsContactMobileHistory = new SPOAService().QueryApplyListBySpid(this.txtFspid.Text.Trim());
            dgContactMobileHistory.DataSource = dsContactMobileHistory.Tables[0].DefaultView;
            dgContactMobileHistory.DataBind();
        }

        protected void btnHisSearch_Click(object sender, System.EventArgs e)
        {
            try
            {
                dgList.CurrentPageIndex = 0;
                BindData();
                this.dgList.Visible = true;
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                LogHelper.LogError("请求页面：" + Request.Url.AbsoluteUri + ",btnHisSearch_Click，捕获soap类异常，异常:" + eSoap.ToString());
                this.dgList.Visible = false;
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                LogHelper.LogError("请求页面：" + Request.Url.AbsoluteUri + ",btnHisSearch_Click，异常:" + eSys.ToString());
                this.dgList.Visible = false;
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message);
            }
        }

        private void dgList_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            this.dgList.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }

        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            string[] fileArr = new string[2];
            string strfileMobileChange = "";//修改手机变更函
            string strfileMobileCredit = "";//修改手机身份证
            try
            {
                string file = "";
                //上传图片
                if (this.lblEmail.Text.Trim() != this.txtEmail.Text.Trim())//若修改邮箱，必传邮箱变更扫描件
                {
                    if (FileEmail.Value == "")
                        throw new Exception("请上传图片");
                    upImage(FileEmail);
                    file += FileEmail.Value;
                    file += "|" + ViewState["ViewFileName"].ToString();
                    file += "|0";
                    fileArr[0] = file;
                }
                if (FileCredit.Value != "")//身份证扫描件可传可不传
                {
                    upImage(FileCredit);
                    file = "";
                    file += FileEmail.Value;
                    file += "|" + ViewState["ViewFileName"].ToString();
                    file += "|1";
                    fileArr[1] = file;
                }

                //上传手机变更函扫描件
                if (this.lblContactMobile.Text.Trim() != this.txtContactMobile.Text.Trim())
                {
                    if (this.fileMobileChange.Value == "")
                        throw new Exception("请上传图片");
                    upImage(fileMobileChange);
                    strfileMobileChange = ViewState["ViewFileName"].ToString();
                    strfileMobileChange = strfileMobileChange.Replace("uploadfile/", "");
                }
                //手机身份证扫描件可传可不传
                if (fileMobileCredit.Value != "")
                {
                    upImage(fileMobileCredit);
                    strfileMobileCredit = ViewState["ViewFileName"].ToString();
                    strfileMobileCredit = strfileMobileCredit.Replace("uploadfile/", "");
                }

                string strszkey = Session["SzKey"].ToString().Trim();
                int ioperid = Int32.Parse(Session["OperID"].ToString());
                int iserviceid = TENCENT.OSS.CFT.KF.Common.AllUserRight.GetServiceID("InfoCenter");
                string struserdata = Session["uid"].ToString().Trim();
                string content = struserdata + "执行了[修改商户资料]操作,操作对象[" + " "
                    + "]时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                Common.AllUserRight.UpdateSession(strszkey, ioperid, PublicRes.GROUPID, iserviceid, struserdata, content);

                string log = SensitivePowerOperaLib.MakeLog("edit", struserdata, "[修改商户资料]", Session["uid"].ToString(),
                    this.txtFspid.Text.Trim(), this.lblFspName.Text.Trim(), this.txtFspName.Text.Trim(), this.lblEmail.Text.Trim(),
                    this.txtEmail.Text.Trim(), this.lblAddress.Text.Trim(), this.txtAddress.Text.Trim());

                if (!SensitivePowerOperaLib.WriteOperationRecord("InfoCenter", log, this))
                {

                }
                log = SensitivePowerOperaLib.MakeLog("edit", struserdata, "[修改商户联系人手机]", Session["uid"].ToString(),
                    this.txtFspid.Text.Trim(), this.lblContactMobile.Text.Trim(), this.txtContactMobile.Text.Trim());

                if (!SensitivePowerOperaLib.WriteOperationRecord("InfoCenter", log, this))
                {

                }
            }
            catch (Exception err)
            {
                LogHelper.LogError("请求页面：" + Request.Url.AbsoluteUri + ",btnSave_Click，异常:" + err.ToString());
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }
            if (lblFspName.Text.Trim() != txtFspName.Text.Trim() || lblAddress.Text.Trim() != txtAddress.Text.Trim() || lblEmail.Text.Trim() != txtEmail.Text.Trim())
            {
                try
                {
                    if (this.txtEmail.Text.Trim() == "")
                    {
                        throw new Exception("请输入新邮箱!");
                    }

                    CheckData();

                    new SPOAService().SubmitBusinessInfo(Session["uid"].ToString(), this.txtFspid.Text.Trim(), this.lblFspName.Text.Trim(), this.txtFspName.Text.Trim(),
                        this.lblEmail.Text.Trim(), this.txtEmail.Text.Trim(), this.lblAddress.Text.Trim(), this.txtAddress.Text.Trim(),
                        this.tbReasonText.Text.Trim(), fileArr);
                    this.btnSave.Enabled = false;
                }
                catch (SoapException eSoap)
                {
                    LogHelper.LogError("请求页面：" + Request.Url.AbsoluteUri + ",btnSave_Click，调用服务出错-1，异常:" + eSoap.ToString());
                    string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                    WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
                }
                catch (Exception eSys)
                {
                    LogHelper.LogError("请求页面：" + Request.Url.AbsoluteUri + ",btnSave_Click，调用服务出错-2，异常:" + eSys.ToString());
                    WebUtils.ShowMessage(this.Page, "调用服务出错：" + eSys.Message.Replace("'", "‘"));
                }
            }
            if (this.lblContactMobile.Text.Trim() != this.txtContactMobile.Text.Trim())
            {
                try
                {
                    CheckData();
                    //修改联系人手机
                    if (this.txtContactMobile.Text.Trim() == "")
                    {
                        throw new Exception("请输入新联系人手机!");
                    }
                    string msg = new SPOAService().SpidMobileApply(Session["uid"].ToString(), this.txtFspid.Text.Trim(), this.txtContactMobile.Text.Trim(),
                                strfileMobileCredit, strfileMobileChange, this.tbReasonText.Text.Trim());
                    if (msg != "")
                    {
                        throw new Exception(msg);
                    }

                    this.btnSave.Enabled = false;
                }
                catch (SoapException eSoap)
                {
                    LogHelper.LogError("请求页面：" + Request.Url.AbsoluteUri + ",btnSave_Click，修改联系人手机出错-1，异常:" + eSoap.ToString());
                    string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                    WebUtils.ShowMessage(this.Page, "修改联系人手机出错：" + errStr);
                }
                catch (Exception eSys)
                {
                    LogHelper.LogError("请求页面：" + Request.Url.AbsoluteUri + ",btnSave_Click，修改联系人手机出错-2，异常:" + eSys.ToString());
                    WebUtils.ShowMessage(this.Page, "修改联系人手机出错：" + eSys.Message);
                }
            }
        }

        protected void Linkbutton2_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (ViewState["Fspid"] == null || ViewState["Fspid"].ToString() == "")
                {
                    throw new Exception("请先点查询按钮,查询出直付商户!");
                }
                if (this.txtSendEmail.Text.Trim() == "")
                {
                    throw new Exception("请输入发送邮箱!");
                }
                string Password = "";
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                if (qs.ResetMediPasswd(ViewState["Fspid"].ToString(), out Password))
                {
                    if (Password == "")
                    {
                        throw new Exception("密码无效!");
                    }
                    if (SendEmailForPassword(ViewState["Fspid"].ToString(), this.txtSendEmail.Text.Trim(), Password))
                        WebUtils.ShowMessage(this.Page, "密码发送成功");
                    else
                    {
                        WebUtils.ShowMessage(this.Page, "无效的值，密码发送失败");
                    }
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "无效的值，密码发送失败");
                }
            }
            catch (SoapException eSoap)
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, eSys.Message);
            }
        }

        private bool SendEmailForPassword(string Fspid, string Email, string Password)
        {
            try
            {
                //yinhuang 2013/8/7
                string str_params = "p_parm1=" + Fspid + "&p_parm2=" + Password + "&p_parm3=" + DateTime.Now.ToString("yyyy年MM月dd日 HH:时mm分ss秒");
                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(Email, "2038", str_params);

                return true;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        protected void btnSendEmail_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (ViewState["Fspid"] == null || ViewState["Fspid"].ToString() == "")
                {
                    throw new Exception("请先点查询按钮,查询出直付商户!");
                }

                if (this.txtSendEmail.Text.Trim() == "")
                {
                    throw new Exception("请输入发送邮箱!");
                }
                if (this.lblMerKey.Text.Trim() == "")
                {
                    throw new Exception("密钥无效!");
                }

                if (SendEmailForKey(ViewState["Fspid"].ToString(), this.txtSendEmail.Text.Trim(), this.lblMerKey.Text.Trim()))
                    WebUtils.ShowMessage(this.Page, "密钥发送成功");
                else
                    WebUtils.ShowMessage(this.Page, "无效的值，密钥发送失败");
            }
            catch (SoapException eSoap)
            {
                LogHelper.LogError("请求页面：" + Request.Url.AbsoluteUri + ",btnSendEmail_Click，捕获soap类异常，异常:" + eSoap.ToString());
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                LogHelper.LogError("请求页面：" + Request.Url.AbsoluteUri + ",btnSendEmail_Click，异常:" + eSys.ToString());
                WebUtils.ShowMessage(this.Page, eSys.Message);
            }
        }

        private bool SendEmailForKey(string Fspid, string Email, string Key)
        {
            try
            {
                //yinhuang 2013/8/7
                string str_params = "p_parm1=" + Fspid + "&p_parm2=" + Key + "&p_parm3=" + DateTime.Now.ToString("yyyy年MM月dd日 HH:时mm分ss秒");
                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(Email, "2040", str_params);

                return true;
            }
            catch (Exception err)
            {
                LogHelper.LogError("请求页面：" + Request.Url.AbsoluteUri + ",SendEmailForKey，异常:" + err.ToString());
                throw new Exception(err.Message);
            }
        }

        protected void btnSendEmailAgain_Click(object sender, System.EventArgs e)
        {
            try
            {
                string msg = "";
                //ViewState["Fspid"] = "2000000501";
                //Session["uid"] = "v_xlingliao";
                bool send = new SPOAService().ReSendEmail_ToSP(out msg, ViewState["Fspid"].ToString(), Session["uid"].ToString());
                if (send)
                    WebUtils.ShowMessage(this.Page, "重发商户通知邮件成功");
                else
                    throw new Exception("spoa接口返回msg:" + msg);
            }
            catch (Exception err)
            {
                LogHelper.LogError("请求页面：" + Request.Url.AbsoluteUri + ",btnSendEmailAgain_Click，重发商户通知邮件失败:" + err.ToString());
                WebUtils.ShowMessage(this.Page, "重发商户通知邮件失败：" + PublicRes.GetErrorMsg(err.Message.ToString()));
            }
        }

        protected void btnSendCertificateAgain_Click(object sender, System.EventArgs e)
        {
            try
            {
                string msg = "";
                //ViewState["Fspid"] = "2000000501";
                //Session["uid"] = "v_xlingliao";
                bool send = new SPOAService().ReSendCertificate(out msg, ViewState["Fspid"].ToString(), Session["uid"].ToString());
                if (send)
                    WebUtils.ShowMessage(this.Page, "重发证书成功");
                else
                    throw new Exception("spoa接口返回msg:" + msg);
            }
            catch (Exception err)
            {
                LogHelper.LogError("请求页面：" + Request.Url.AbsoluteUri + ",btnSendCertificateAgain_Click，重发证书失败:" + err.ToString());
                WebUtils.ShowMessage(this.Page, "重发证书失败：" + PublicRes.GetErrorMsg(err.Message.ToString()));
            }
        }

        protected void upImage(HtmlInputFile file)
        {
            string objid = "";
            try
            {
                //上传需要的图片，并返回对应服务器上的地址
                //存放文件
                // string s1 = File1.Value;
                string s1 = file.Value;
                //放到外面控制
                //if (s1 == "")
                //{
                //    throw new Exception("请上传图片");
                //}
                string szTypeName = s1.Substring(s1.Length - 4, 4);
                string alPath;
                HtmlInputFile inputFile = file;
                string upStr = null;


                if (szTypeName.ToLower() != ".jpg" && szTypeName.ToLower() != ".gif" && szTypeName.ToLower() != ".bmp")
                {
                    throw new Exception("上传的文件不正确，必须为jpg,gif,bmp");
                }

                if (inputFile.Value != "")
                {
                    objid = System.DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.StaticNoManage();
                    //  string fileName = "kf" + DateTime.Now.ToString("yyyyMMddHHmmss") + szTypeName; //
                    string fileName = objid + szTypeName; //

                    upStr = "uploadfile/" + System.DateTime.Now.ToString("yyyyMMdd") + "/CSOMS/Merchant";//System.Configuration.ConfigurationManager.AppSettings["uploadPath"].ToString();

                    string targetPath = Server.MapPath(Request.ApplicationPath) + "\\" + upStr;
                    PublicRes.CreateDirectory(targetPath);

                    string path = targetPath + "\\" + fileName;
                    inputFile.PostedFile.SaveAs(path);

                    //alPath.Add(upStr+ "/" +fileName);	
                    alPath = upStr + "/" + fileName;
                    ViewState["ViewFileName"] = alPath;
                }
                else
                {
                    throw new Exception("请上传正确的图片");
                }
            }
            catch (Exception eStr)
            {
                LogHelper.LogError("请求页面：" + Request.Url.AbsoluteUri + ",upImage，上传文件异常:" + eStr.ToString());
                string errMsg = "上传文件失败！" + PublicRes.GetErrorMsg(eStr.Message.ToString());
                throw new Exception(errMsg);
            }
        }
    }
}
