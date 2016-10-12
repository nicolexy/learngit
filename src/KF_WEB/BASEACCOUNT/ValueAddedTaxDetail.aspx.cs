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
using System.Configuration;
using CFT.CSOMS.BLL.SPOA;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// ValueAddedTaxDetail ��ժҪ˵����
    /// </summary>
    public partial class ValueAddedTaxDetail : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();

                if (!classLibrary.ClassLib.ValidateRight("DrawAndApprove", this)) Response.Redirect("../login.aspx?wh=1");

                if (!IsPostBack)
                {
                    btnApprove.Attributes["onClick"] = "return confirm('ȷ�����ͨ����');";
                    btnCancel.Attributes["onClick"] = "return confirm('ȷ����˾ܾ���');";
                    string TaskId = Request.QueryString["TaskId"];
                    ViewState["TaskId"] = TaskId;
                    ViewState["uid"] = Session["uid"].ToString(); ;
                    BindInfo(TaskId,false);
                }
            }
            catch(Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("Ӫ������ϸ��Ϣ");
                log.ErrorFormat("Ӫ������ϸ��Ϣ����{0} ", ex.Message);
                throw new Exception("Ӫ������ϸ��Ϣ����" + ex.Message.ToString());
                //Response.Redirect("../login.aspx?wh=1");
            }
        }

        private void BindInfo(string TaskId,bool isMail)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            DataSet ds = qs.GetValueAddedTaxDetail(TaskId);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                this.labSPID.Text = dr["SPID"].ToString();
                if (dr["ApplyType"].ToString() == "0")
                    this.labApplyType.Text = "��������";
                else if (dr["ApplyType"].ToString() == "1")
                    this.labApplyType.Text = "ȫ���޸�����";
                else if (dr["ApplyType"].ToString() == "49")
                    this.labApplyType.Text = "spoa����";
                else
                    this.labApplyType.Text = dr["ApplyType"].ToString();

                if (dr["Flag"].ToString() == "0")
                    this.labFlag.Text = "��ʼ״̬(δ�����)";
                else if (dr["Flag"].ToString() == "1")
                    this.labFlag.Text = "����������Ȩ����ϴ�";
                else if (dr["Flag"].ToString() == "2")
                    this.labFlag.Text = "�������������";
                else if (dr["Flag"].ToString() == "3")
                    this.labFlag.Text = "ȫ���޸������";
                else if (dr["Flag"].ToString() == "4")
                    this.labFlag.Text = "���ͨ��";
                else if (dr["Flag"].ToString() == "5")
                    this.labFlag.Text = "��˲�ͨ��";
                else if (dr["Flag"].ToString() == "6")
                    this.labFlag.Text = "�ռ�����Ϣ�޸���";
                else if (dr["Flag"].ToString() == "7")
                    this.labFlag.Text = "�ռ�����Ϣ�޸ĳɹ�";
                else if (dr["Flag"].ToString() == "8")
                    this.labFlag.Text = "�ռ�����Ϣ�޸�ʧ��";
                else if (dr["Flag"].ToString() == "9")
                    this.labFlag.Text = "�ռ�����Ϣ�޸�ʧ��";
                else if (dr["Flag"].ToString() == "10")
                    this.labFlag.Text = "�ռ�����Ϣ�޸�ʧ��";
                else if (dr["Flag"].ToString() == "11")
                    this.labFlag.Text = "spoa���";
                else
                    this.labFlag.Text = dr["Flag"].ToString();


               
                if (dr["OldTaxerType"].ToString().Trim() != "")
                {
                    this.labOldTaxerType.Text = GetTaxerType(dr["OldTaxerType"].ToString());
                    this.labOldTaxInvoiceType.Text = GetTaxInvoiceType(dr["OldTaxInvoiceType"].ToString());
                    this.labOldCompanyName.Text = dr["OldCompanyName"].ToString();
                    this.labOldTaxerID.Text = dr["OldTaxerID"].ToString();
                    this.labOldBasebankName.Text = dr["OldBasebankName"].ToString();
                    this.labOldBaseBankAcct.Text = dr["OldBaseBankAcct"].ToString();
                    this.labOldReceiverName.Text = dr["OldReceiverName"].ToString();
                    this.labOldReceiverAddr.Text = dr["OldReceiverAddr"].ToString();
                    this.labOldReceiverPostalCode.Text = dr["OldReceiverPostalCode"].ToString();
                    this.labOldReceiverPhone.Text = dr["OldReceiverPhone"].ToString();
                    this.labOldUserType.Text = GetUserType(dr["OldUserType"].ToString());
                    this.labOldReceiverPostalCode.Text = dr["OldReceiverPostalCode"].ToString();
                    this.labOldCompanyAddress.Text = dr["OldCompanyAddress"].ToString();
                    this.labOldCompanyPhone.Text = dr["OldCompanyPhone"].ToString();
                    if (dr["OldIsInvoice"].ToString() == "1")
                    {
                        this.lbOldIsInvoice.Text = "��";
                    }
                    else {
                        this.lbOldIsInvoice.Text = "��";
                    }
                    
                    this.OldInfo.Visible = true;
                }
                else
                    this.OldInfo.Visible = false;

                this.labNewTaxerType.Text = GetTaxerType(dr["NewTaxerType"].ToString());
                this.labNewTaxInvoiceType.Text = GetTaxInvoiceType(dr["NewTaxInvoiceType"].ToString());
                this.labNewCompanyName.Text = dr["NewCompanyName"].ToString();
                this.labNewTaxerID.Text = dr["NewTaxerID"].ToString();
                this.labNewBasebankName.Text = dr["NewBasebankName"].ToString();
                this.labNewBaseBankAcct.Text = dr["NewBaseBankAcct"].ToString();
                this.labNewReceiverName.Text = dr["NewReceiverName"].ToString();
                this.labNewReceiverAddr.Text = dr["NewReceiverAddr"].ToString();
                this.labNewReceiverPostalCode.Text = dr["NewReceiverPostalCode"].ToString();
                this.labNewReceiverPhone.Text = dr["NewReceiverPhone"].ToString();
                this.labNewUserType.Text = GetUserType(dr["NewUserType"].ToString());
                this.labNewReceiverPostalCode.Text = dr["NewReceiverPostalCode"].ToString();
                this.labNewCompanyAddress.Text = dr["NewCompanyAddress"].ToString();
                this.labNewCompanyPhone.Text = dr["NewCompanyPhone"].ToString();
                if (dr["NewIsInvoice"].ToString() == "1")
                {
                    this.lbNewIsInvoice.Text = "��";
                }
                else
                {
                    this.lbNewIsInvoice.Text = "��";
                }

                this.lblApplyTime.Text = dr["ApplyTime"].ToString();
                this.txtMemo.Text = dr["Memo"].ToString();
                string url = null;
                try
                {
                    url = ConfigurationManager.AppSettings["ValueAddedTaxUrlPath"].Trim();
                }
                catch (Exception ex)
                {
                    log4net.ILog log = log4net.LogManager.GetLogger("Ӫ������ϸ��Ϣ==ValueAddedTaxUrlPath");
                    log.ErrorFormat("ȡ�ڵ�ValueAddedTaxUrlPath����{0} ", ex.Message);
                    url = "http://kf2.cf.com/uploadfile";
                }
                

                if (!url.EndsWith("/"))
                    url += "/";

                if (dr["TaxCert"].ToString() != "")
                    imgTaxCert.ImageUrl = url + dr["TaxCert"].ToString();
                else
                    imgTaxCert.ImageUrl = "";

                if (dr["BizLicenseCert"].ToString() != "")
                    imgBizLicenseCert.ImageUrl = url + dr["BizLicenseCert"].ToString();
                else
                    imgBizLicenseCert.ImageUrl = "";

                if (dr["AuthorizationCert"].ToString() != "")
                    imgAuthorizationCert.ImageUrl = url + dr["AuthorizationCert"].ToString();
                else
                    imgAuthorizationCert.ImageUrl = "";

                if (dr["Flag"].ToString() == "2" || dr["Flag"].ToString() == "3")
                {
                    this.txtMemo.ReadOnly = false;
                    this.btnApprove.Visible = true;
                    this.btnCancel.Visible = true;
                }
                else
                {
                    this.txtMemo.ReadOnly = true;
                    this.btnApprove.Visible = false;
                    this.btnCancel.Visible = false;
                }

                //send mail yinhuang 2013/8/7
                if (isMail) {
                    string msg = "";
                    Query_Service.ComplainBussClass sc = qs.GetComplainBussDetail(dr["SPID"].ToString(), out msg);
                    DataSet buss_ds = qs.GetPayBusinessList("", dr["SPID"].ToString(), "", "");
                    string emailTo = "";
                    if (buss_ds != null && buss_ds.Tables[0].Rows.Count == 1) 
                    {
                        string keyId = buss_ds.Tables[0].Rows[0]["ApplyCpInfoID"].ToString();
                        //DataSet ds1 = qs.GetPayBusinessInfo(keyId);
                        DataSet ds1 = new SPOAService().GetSpInfo(dr["SPID"].ToString(), keyId, "", "", "", "", 2, 0);
                        if (ds1 != null && ds1.Tables[0].Rows.Count == 1) {
                            emailTo = ds1.Tables[0].Rows[0]["ContactEmail"].ToString();
                        }
                    }
                    if (emailTo != "") {
                        string s_taxerType = dr["NewTaxerType"].ToString();
                        if (s_taxerType == "0")
                        {
                            //�̻���Ʊ��Ϣ����֪ͨ 2070
                            string str_params = "p_parm1=" + dr["SPID"].ToString() + "&p_parm2=" + dr["OldCompanyName"].ToString() + "&p_parm3=" + dr["OldReceiverName"].ToString() + "&p_parm4=" + dr["OldReceiverPhone"].ToString()
                                + "&p_parm5=" + dr["OldReceiverPostalCode"].ToString() + "&p_parm6=" + dr["NewTaxerID"].ToString() + "&p_parm7=" + dr["NewCompanyPhone"].ToString() + "&p_parm8=" + dr["NewBasebankName"].ToString()
                                + "&p_parm9=" + dr["NewBaseBankAcct"].ToString() + "&p_parm10=" + dr["NewReceiverAddr"].ToString() + "&p_parm11=" + dr["NewCompanyAddress"].ToString();
                            TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(emailTo, "2070", str_params);
                        }
                        else if (s_taxerType == "1")
                        {
                            //�̻���Ʊ��Ϣ�޸ĳɹ�֪ͨ 2071
                            string str_params = "p_parm1=" + dr["SPID"].ToString() + "&p_parm2=" + dr["OldUserType"].ToString() + "&p_parm3=" + dr["OldCompanyName"].ToString() + "&p_parm4=" + dr["OldReceiverName"].ToString()
                                + "&p_parm5=" + dr["OldReceiverPhone"].ToString() + "&p_parm6=" + dr["OldReceiverPostalCode"].ToString() + "&p_parm7=" + dr["NewReceiverAddr"].ToString() + "&p_parm8=" + dr["NewBasebankName"].ToString();
                            TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(emailTo, "2071", str_params);
                        }
                    } 
                } 
            }
            else
            {
                throw new Exception("û���ҵ���¼��");
            }
        }

        private string GetTaxerType(string TaxerType)
        {
            if (TaxerType == "0")
                return "��ֵ˰";
            else if (TaxerType == "1")
                return "������ֵ˰";
            else
                return TaxerType;
        }

        private string GetTaxInvoiceType(string TaxInvoiceType)
        {
            if (TaxInvoiceType == "0")
                return "��ֵ˰ר�÷�Ʊ";
            else if (TaxInvoiceType == "1")
                return "ͨ�û���Ʊ";
            else
                return TaxInvoiceType;
        }

        private string GetUserType(string UserType)
        {
            if (UserType == "1")
                return "����";
            else if (UserType == "2")
                return "��˾";
            else
                return UserType;
        }

        protected void btnApprove_Click(object sender, System.EventArgs e)
        {
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                qs.ValueAddedTaxApprove(ViewState["TaskId"].ToString(), classLibrary.setConfig.replaceMStr(this.txtMemo.Text.Trim()),
                    imgTaxCert.ImageUrl, imgBizLicenseCert.ImageUrl, imgAuthorizationCert.ImageUrl, ViewState["uid"].ToString());
                BindInfo(ViewState["TaskId"].ToString(),true);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "����ʧ�ܣ�" + classLibrary.setConfig.replaceMStr(ex.Message));
            }
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                qs.ValueAddedTaxCancel(ViewState["TaskId"].ToString(), this.labSPID.Text.Trim(), classLibrary.setConfig.replaceMStr(this.txtMemo.Text.Trim()), ViewState["uid"].ToString());
                BindInfo(ViewState["TaskId"].ToString(),false);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "����ʧ�ܣ�" + classLibrary.setConfig.replaceMStr(ex.Message));
            }
        }
    }
}
