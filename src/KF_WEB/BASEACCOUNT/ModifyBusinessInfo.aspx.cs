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
    /// ModifyBusinessInfo ��ժҪ˵����
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
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
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
            if (this.rbtBusiness.Checked)          //�ж��Ƿ������ֱ���̻�
            {
                dsSPOA = qs.GetPayBusinessInfoList(this.txtFspid.Text.Trim());

                if (dsSPOA != null && dsSPOA.Tables[0].Rows.Count != 1)
                {
                    throw new Exception("��������ȷֱ���̻���!");
                }
                else
                    dsMain = qs.GetBusinessInfoList(this.txtFspid.Text.Trim());
            }
            else                    //�ж��Ƿ�������н��̻�
            {
                dsSPOA = qs.GetAgencyBusinessInfoList(this.txtFspid.Text.Trim());

                if (dsSPOA != null && dsSPOA.Tables[0].Rows.Count != 1)
                {
                    throw new Exception("��������ȷ�н��̻���!");
                }
                else
                    dsMain = qs.GetBusiness2InfoList(this.txtFspid.Text.Trim());
            }

            if (dsMain != null && dsMain.Tables[0].Rows.Count == 1)       //�ж����֤����λ�Ƿ���ȷ
            {

            }
            else
            {
                throw new Exception("���̻����ϲ�����!");
            }

            if (this.rbtBusiness.Checked)
            {
                //���Ե�ʱ��ע�͵�
                DataSet ds = qs.GetBusinessBankList(this.txtFspid.Text.Trim());
                if (ds != null && ds.Tables[0].Rows.Count == 1)    //�ж����п�����λ�Ƿ���ȷ
                {
                    string BankCard = ds.Tables[0].Rows[0]["Fbankid"].ToString().Trim();

                    if (BankCard != null && BankCard.Length >= 5)
                    {
                        if (BankCard.Substring(BankCard.Length - 5, 5) != this.txtBankCard.Text.Trim())
                            throw new Exception("���п���֤��ͨ��,��������ȷ���п�����λ!");
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
                        throw new Exception("���̻����п�������!");
                    }
                }
                else
                {
                    throw new Exception("���̻����а����ϲ�����!");
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
            catch (SoapException eSoap) //����soap���쳣
            {
                LogHelper.LogError("����ҳ�棺" + Request.Url.AbsoluteUri + ",btnSearch_Click������soap���쳣���쳣:" + eSoap.ToString());
                this.Table1.Visible = false;
                this.dgList.Visible = false;
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                LogHelper.LogError("����ҳ�棺" + Request.Url.AbsoluteUri + ",btnSearch_Click���쳣:" + eSys.ToString());
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
                                throw new Exception("���п���֤ͨ��!");
                            else
                                throw new Exception("���п���֤��ͨ��,��������ȷ���п�����λ!");
                        }
                        else
                        {
                            throw new Exception("���̻����п�������!");
                        }
                    }
                    else
                    {
                        throw new Exception("���̻����а����ϲ�����!");
                    }
                }
                catch (SoapException eSoap) //����soap���쳣
                {
                    LogHelper.LogError("����ҳ�棺" + Request.Url.AbsoluteUri + ",btnBankCardCheck_Click������soap���쳣���쳣:" + eSoap.ToString());
                    string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                    WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
                }
                catch (Exception eSys)
                {
                    LogHelper.LogError("����ҳ�棺" + Request.Url.AbsoluteUri + ",btnBankCardCheck_Click���쳣:" + eSys.ToString());
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
            catch (SoapException eSoap) //����soap���쳣
            {
                LogHelper.LogError("����ҳ�棺" + Request.Url.AbsoluteUri + ",btnHisSearch_Click������soap���쳣���쳣:" + eSoap.ToString());
                this.dgList.Visible = false;
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                LogHelper.LogError("����ҳ�棺" + Request.Url.AbsoluteUri + ",btnHisSearch_Click���쳣:" + eSys.ToString());
                this.dgList.Visible = false;
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message);
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
            string strfileMobileChange = "";//�޸��ֻ������
            string strfileMobileCredit = "";//�޸��ֻ����֤
            try
            {
                string file = "";
                //�ϴ�ͼƬ
                if (this.lblEmail.Text.Trim() != this.txtEmail.Text.Trim())//���޸����䣬�ش�������ɨ���
                {
                    if (FileEmail.Value == "")
                        throw new Exception("���ϴ�ͼƬ");
                    upImage(FileEmail);
                    file += FileEmail.Value;
                    file += "|" + ViewState["ViewFileName"].ToString();
                    file += "|0";
                    fileArr[0] = file;
                }
                if (FileCredit.Value != "")//���֤ɨ����ɴ��ɲ���
                {
                    upImage(FileCredit);
                    file = "";
                    file += FileEmail.Value;
                    file += "|" + ViewState["ViewFileName"].ToString();
                    file += "|1";
                    fileArr[1] = file;
                }

                //�ϴ��ֻ������ɨ���
                if (this.lblContactMobile.Text.Trim() != this.txtContactMobile.Text.Trim())
                {
                    if (this.fileMobileChange.Value == "")
                        throw new Exception("���ϴ�ͼƬ");
                    upImage(fileMobileChange);
                    strfileMobileChange = ViewState["ViewFileName"].ToString();
                    strfileMobileChange = strfileMobileChange.Replace("uploadfile/", "");
                }
                //�ֻ����֤ɨ����ɴ��ɲ���
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
                string content = struserdata + "ִ����[�޸��̻�����]����,��������[" + " "
                    + "]ʱ��:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                Common.AllUserRight.UpdateSession(strszkey, ioperid, PublicRes.GROUPID, iserviceid, struserdata, content);

                string log = SensitivePowerOperaLib.MakeLog("edit", struserdata, "[�޸��̻�����]", Session["uid"].ToString(),
                    this.txtFspid.Text.Trim(), this.lblFspName.Text.Trim(), this.txtFspName.Text.Trim(), this.lblEmail.Text.Trim(),
                    this.txtEmail.Text.Trim(), this.lblAddress.Text.Trim(), this.txtAddress.Text.Trim());

                if (!SensitivePowerOperaLib.WriteOperationRecord("InfoCenter", log, this))
                {

                }
                log = SensitivePowerOperaLib.MakeLog("edit", struserdata, "[�޸��̻���ϵ���ֻ�]", Session["uid"].ToString(),
                    this.txtFspid.Text.Trim(), this.lblContactMobile.Text.Trim(), this.txtContactMobile.Text.Trim());

                if (!SensitivePowerOperaLib.WriteOperationRecord("InfoCenter", log, this))
                {

                }
            }
            catch (Exception err)
            {
                LogHelper.LogError("����ҳ�棺" + Request.Url.AbsoluteUri + ",btnSave_Click���쳣:" + err.ToString());
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }
            if (lblFspName.Text.Trim() != txtFspName.Text.Trim() || lblAddress.Text.Trim() != txtAddress.Text.Trim() || lblEmail.Text.Trim() != txtEmail.Text.Trim())
            {
                try
                {
                    if (this.txtEmail.Text.Trim() == "")
                    {
                        throw new Exception("������������!");
                    }

                    CheckData();

                    new SPOAService().SubmitBusinessInfo(Session["uid"].ToString(), this.txtFspid.Text.Trim(), this.lblFspName.Text.Trim(), this.txtFspName.Text.Trim(),
                        this.lblEmail.Text.Trim(), this.txtEmail.Text.Trim(), this.lblAddress.Text.Trim(), this.txtAddress.Text.Trim(),
                        this.tbReasonText.Text.Trim(), fileArr);
                    this.btnSave.Enabled = false;
                }
                catch (SoapException eSoap)
                {
                    LogHelper.LogError("����ҳ�棺" + Request.Url.AbsoluteUri + ",btnSave_Click�����÷������-1���쳣:" + eSoap.ToString());
                    string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                    WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
                }
                catch (Exception eSys)
                {
                    LogHelper.LogError("����ҳ�棺" + Request.Url.AbsoluteUri + ",btnSave_Click�����÷������-2���쳣:" + eSys.ToString());
                    WebUtils.ShowMessage(this.Page, "���÷������" + eSys.Message.Replace("'", "��"));
                }
            }
            if (this.lblContactMobile.Text.Trim() != this.txtContactMobile.Text.Trim())
            {
                try
                {
                    CheckData();
                    //�޸���ϵ���ֻ�
                    if (this.txtContactMobile.Text.Trim() == "")
                    {
                        throw new Exception("����������ϵ���ֻ�!");
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
                    LogHelper.LogError("����ҳ�棺" + Request.Url.AbsoluteUri + ",btnSave_Click���޸���ϵ���ֻ�����-1���쳣:" + eSoap.ToString());
                    string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                    WebUtils.ShowMessage(this.Page, "�޸���ϵ���ֻ�����" + errStr);
                }
                catch (Exception eSys)
                {
                    LogHelper.LogError("����ҳ�棺" + Request.Url.AbsoluteUri + ",btnSave_Click���޸���ϵ���ֻ�����-2���쳣:" + eSys.ToString());
                    WebUtils.ShowMessage(this.Page, "�޸���ϵ���ֻ�����" + eSys.Message);
                }
            }
        }

        protected void Linkbutton2_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (ViewState["Fspid"] == null || ViewState["Fspid"].ToString() == "")
                {
                    throw new Exception("���ȵ��ѯ��ť,��ѯ��ֱ���̻�!");
                }
                if (this.txtSendEmail.Text.Trim() == "")
                {
                    throw new Exception("�����뷢������!");
                }
                string Password = "";
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                if (qs.ResetMediPasswd(ViewState["Fspid"].ToString(), out Password))
                {
                    if (Password == "")
                    {
                        throw new Exception("������Ч!");
                    }
                    if (SendEmailForPassword(ViewState["Fspid"].ToString(), this.txtSendEmail.Text.Trim(), Password))
                        WebUtils.ShowMessage(this.Page, "���뷢�ͳɹ�");
                    else
                    {
                        WebUtils.ShowMessage(this.Page, "��Ч��ֵ�����뷢��ʧ��");
                    }
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "��Ч��ֵ�����뷢��ʧ��");
                }
            }
            catch (SoapException eSoap)
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
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
                string str_params = "p_parm1=" + Fspid + "&p_parm2=" + Password + "&p_parm3=" + DateTime.Now.ToString("yyyy��MM��dd�� HH:ʱmm��ss��");
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
                    throw new Exception("���ȵ��ѯ��ť,��ѯ��ֱ���̻�!");
                }

                if (this.txtSendEmail.Text.Trim() == "")
                {
                    throw new Exception("�����뷢������!");
                }
                if (this.lblMerKey.Text.Trim() == "")
                {
                    throw new Exception("��Կ��Ч!");
                }

                if (SendEmailForKey(ViewState["Fspid"].ToString(), this.txtSendEmail.Text.Trim(), this.lblMerKey.Text.Trim()))
                    WebUtils.ShowMessage(this.Page, "��Կ���ͳɹ�");
                else
                    WebUtils.ShowMessage(this.Page, "��Ч��ֵ����Կ����ʧ��");
            }
            catch (SoapException eSoap)
            {
                LogHelper.LogError("����ҳ�棺" + Request.Url.AbsoluteUri + ",btnSendEmail_Click������soap���쳣���쳣:" + eSoap.ToString());
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                LogHelper.LogError("����ҳ�棺" + Request.Url.AbsoluteUri + ",btnSendEmail_Click���쳣:" + eSys.ToString());
                WebUtils.ShowMessage(this.Page, eSys.Message);
            }
        }

        private bool SendEmailForKey(string Fspid, string Email, string Key)
        {
            try
            {
                //yinhuang 2013/8/7
                string str_params = "p_parm1=" + Fspid + "&p_parm2=" + Key + "&p_parm3=" + DateTime.Now.ToString("yyyy��MM��dd�� HH:ʱmm��ss��");
                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(Email, "2040", str_params);

                return true;
            }
            catch (Exception err)
            {
                LogHelper.LogError("����ҳ�棺" + Request.Url.AbsoluteUri + ",SendEmailForKey���쳣:" + err.ToString());
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
                    WebUtils.ShowMessage(this.Page, "�ط��̻�֪ͨ�ʼ��ɹ�");
                else
                    throw new Exception("spoa�ӿڷ���msg:" + msg);
            }
            catch (Exception err)
            {
                LogHelper.LogError("����ҳ�棺" + Request.Url.AbsoluteUri + ",btnSendEmailAgain_Click���ط��̻�֪ͨ�ʼ�ʧ��:" + err.ToString());
                WebUtils.ShowMessage(this.Page, "�ط��̻�֪ͨ�ʼ�ʧ�ܣ�" + PublicRes.GetErrorMsg(err.Message.ToString()));
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
                    WebUtils.ShowMessage(this.Page, "�ط�֤��ɹ�");
                else
                    throw new Exception("spoa�ӿڷ���msg:" + msg);
            }
            catch (Exception err)
            {
                LogHelper.LogError("����ҳ�棺" + Request.Url.AbsoluteUri + ",btnSendCertificateAgain_Click���ط�֤��ʧ��:" + err.ToString());
                WebUtils.ShowMessage(this.Page, "�ط�֤��ʧ�ܣ�" + PublicRes.GetErrorMsg(err.Message.ToString()));
            }
        }

        protected void upImage(HtmlInputFile file)
        {
            string objid = "";
            try
            {
                //�ϴ���Ҫ��ͼƬ�������ض�Ӧ�������ϵĵ�ַ
                //����ļ�
                // string s1 = File1.Value;
                string s1 = file.Value;
                //�ŵ��������
                //if (s1 == "")
                //{
                //    throw new Exception("���ϴ�ͼƬ");
                //}
                string szTypeName = s1.Substring(s1.Length - 4, 4);
                string alPath;
                HtmlInputFile inputFile = file;
                string upStr = null;


                if (szTypeName.ToLower() != ".jpg" && szTypeName.ToLower() != ".gif" && szTypeName.ToLower() != ".bmp")
                {
                    throw new Exception("�ϴ����ļ�����ȷ������Ϊjpg,gif,bmp");
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
                    throw new Exception("���ϴ���ȷ��ͼƬ");
                }
            }
            catch (Exception eStr)
            {
                LogHelper.LogError("����ҳ�棺" + Request.Url.AbsoluteUri + ",upImage���ϴ��ļ��쳣:" + eStr.ToString());
                string errMsg = "�ϴ��ļ�ʧ�ܣ�" + PublicRes.GetErrorMsg(eStr.Message.ToString());
                throw new Exception(errMsg);
            }
        }
    }
}
