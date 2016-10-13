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
using CFT.CSOMS.BLL.SPOA;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// PayBusinessQuery ��ժҪ˵����
    /// </summary>
    public partial class PayBusinessQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {         
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {                
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                //int operid = Int32.Parse(Session["OperID"].ToString());

                //if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

                if (!classLibrary.ClassLib.ValidateRight("SPInfoManagement", this)) Response.Redirect("../login.aspx?wh=1");

                this.btnSave.Attributes.Add("onclick", "return CheckEmail();");

                if (!IsPostBack)
                {
                    this.divInfo.Visible = false;

                    // ������ת��ѯ�̻���Ϣ������
                    if (Request.QueryString["spid"] != null)
                    {
                        this.txtFspid.Text = Request.QueryString["spid"].Trim();

                        BindData(1);
                    }
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
            this.dgList.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgList_ItemCommand);
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

        }


        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }
        #endregion

        private void ValidateDate()
        {
            string spname = txtFspidName.Text.ToString();
            string spid = txtFspid.Text.ToString();
            string spwww = txtFspidAddress.Text.ToString();
            string spweb_name = txtWebName.Text.ToString();
            //string spalias = txtAlias.Text.ToString();

            if (spname == "" && spid == "" && spwww == "" && spweb_name == "")
            {
                throw new Exception("��ѯ��������Ϊ�գ�");
            }

        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            this.divInfo.Visible = false;

            try
            {
                this.pager.RecordCount = 1000;
                dgList.CurrentPageIndex = 0;
                BindData(1);
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                this.dgList.Visible = false;
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                this.dgList.Visible = false;
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message);
            }
        }

        private void BindData(int index)
        {
            this.pager.CurrentPageIndex = index;
            int max = pager.PageSize;
            int start = max * (index - 1);

            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            string spname = txtFspidName.Text.ToString();
            string spid = txtFspid.Text.ToString();
            string spwww = txtFspidAddress.Text.ToString();
            string spweb_name = txtWebName.Text.ToString();
            string appid = txtAPPID.Text.ToString();
            DataSet ds = new SPOAService().GetSpInfo(spid, "", spname, spwww, spweb_name, appid, max, start);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                this.dgList.Visible = true;
                ViewState["dt"] = ds.Tables[0];
                dgList.DataSource = ds.Tables[0].DefaultView;
                dgList.DataBind();
            }
            else
            {
                dgList.DataSource = null;
                throw new LogicException("û���ҵ���¼��");
            }
        }

        private string GetFilterString()
        {
            string filter = "";
            if (this.txtFspidName.Text.Trim() != "")
                filter += " and CompanyName.Contains('" + this.txtFspidName.Text.Trim() + "')  ";
            if (this.txtFspid.Text.Trim() != "")
                filter += " and SPID!=null and SPID.Contains('" + this.txtFspid.Text.Trim() + "') ";
            if (this.txtFspidAddress.Text.Trim() != "")
                filter += " and  WWWAdress.Contains('" + this.txtFspidAddress.Text.Trim() + "') ";
            if (this.txtWebName.Text.Trim() != "")
                filter += " and WebName.Contains('" + this.txtWebName.Text.Trim() + "') ";
            if (this.txtAPPID.Text.Trim() != "")
                filter += " and AppID.Contains('" + this.txtAPPID.Text.Trim() + "')";
            return filter;
        }

        private void dgList_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                try
                {
                    Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                    //DataSet ds1 = qs.GetPayBusinessInfo(e.Item.Cells[0].Text);
                    string KeyID = e.Item.Cells[0].Text;
                    //  DataSet ds1 = new SPOAService().GetSpInfo(" and ApplyCpInfoID="+KeyID, 2, 0);
                    DataSet ds1 = new SPOAService().GetSpInfo("", KeyID, "", "", "", "", 2, 0);

                   
                    
                    if (ds1.Tables[0].Rows.Count == 1)
                    {
                        this.divInfo.Visible = true;
                        this.btnEdit.Visible = true;
                        this.btnSave.Visible = false;
                        this.btnCancel.Visible = false;

                        this.txtConnetionName.Enabled = false;
                        this.txtPhone.Enabled = false;
                        this.txtMobile.Enabled = false;
                        this.txtQQNo.Enabled = false;


                        ViewState["KeyID"] = e.Item.Cells[0].Text;
                        ViewState["TableFlag"] = e.Item.Cells[1].Text;
                        this.lblFspid.Text = ds1.Tables[0].Rows[0]["SPID"].ToString();
                        this.lblURL.Text = ds1.Tables[0].Rows[0]["WWWAdress"].ToString();
                        this.lblType.Text = ds1.Tables[0].Rows[0]["TradeName"].ToString();
                        string IdentityCardNum = PublicRes.objectToString(ds1.Tables[0], "IdentityCardNum");

                        if (!string.IsNullOrEmpty(IdentityCardNum) && IdentityCardNum.Length > 5)
                            this.lblFsidID.Text = "**********" + IdentityCardNum.Substring(IdentityCardNum.Length - 5, 5);
                        else
                            this.lblFsidID.Text = IdentityCardNum;
                        this.lblArea.Text = ds1.Tables[0].Rows[0]["AreaName"].ToString();
                        this.lblBDName.Text = ds1.Tables[0].Rows[0]["BDName"].ToString();
                        this.txtConnetionName.Text = ds1.Tables[0].Rows[0]["ContactUser"].ToString();
                        bool isRight_SensitiveRole = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SensitiveRole", this);
                        //this.txtPhone.Text = ds1.Tables[0].Rows[0]["ContactPhone"].ToString();
                        this.txtPhone.Text = classLibrary.setConfig.ConvertTelephoneNumber(ds1.Tables[0].Rows[0]["ContactPhone"].ToString(), isRight_SensitiveRole);                        
                        //this.txtMobile.Text = ds1.Tables[0].Rows[0]["ContactMobile"].ToString();
                        this.txtMobile.Text = classLibrary.setConfig.ConvertTelephoneNumber(ds1.Tables[0].Rows[0]["ContactMobile"].ToString(), isRight_SensitiveRole);
                        this.txtEmail.Text = ds1.Tables[0].Rows[0]["ContactEmail"].ToString();
                        //	this.lblFaxNo.Text = ds1.Tables[0].Rows[0]["ContactFax"].ToString();
                        this.txtQQNo.Text = ds1.Tables[0].Rows[0]["ContactQQ"].ToString();
                        this.txtRemark.Text = ds1.Tables[0].Rows[0]["ErrorMemo"].ToString();

                        this.txtBDSpidType.Text = ds1.Tables[0].Rows[0]["BDSpidTypeName"].ToString();
                        this.txtIsWXBeil.Text = ds1.Tables[0].Rows[0]["IsWXBeilName"].ToString();
                        this.txtWXBeil.Text = ds1.Tables[0].Rows[0]["WXBeil"].ToString();
                        this.txtInType.Text = ds1.Tables[0].Rows[0]["InType"].ToString();
                        this.txtServiceUser.Text = ds1.Tables[0].Rows[0]["ServiceUser"].ToString();
                        this.txtServiceQQ.Text = ds1.Tables[0].Rows[0]["ServiceQQ"].ToString();
                        this.txtServiceTel.Text = ds1.Tables[0].Rows[0]["ServiceTel"].ToString();
                        this.txtServiceEmail.Text = ds1.Tables[0].Rows[0]["ServiceEmail"].ToString();

                        //�����̻���ַ���������� yinhuang 2013/8/2
                        //   this.txtComAddr.Text = PublicRes.objectToString(ds1.Tables[0], "CompanyAddress");
                        //  this.txtPosCode.Text = PublicRes.objectToString(ds1.Tables[0], "Postalcode"); 
                        ViewState["CompanyAddress"] = PublicRes.objectToString(ds1.Tables[0], "CompanyAddress");
                        ViewState["Postalcode"] = PublicRes.objectToString(ds1.Tables[0], "Postalcode");
                        this.lblPickType.Text = "δ�ҵ�����";
                        this.lblFBalance.Text = "δ�ҵ�����";

                            DataSet ds2 = qs.GetPayBusinessElseInfo(ds1.Tables[0].Rows[0]["SPID"].ToString().Trim());
                            DataSet ds3 = PermitPara.QueryDicAccName();
                            T_USER_MED ds4 = qs.GetUserMedInfo(ds1.Tables[0].Rows[0]["SPID"].ToString().Trim(), 1, 1);

                            if (ds2 != null && ds2.Tables[0].Rows.Count == 1)
                            {
                                this.lblFBalance.Text = ds2.Tables[0].Rows[0]["Fbalance"].ToString();

                                string PickType = ds2.Tables[0].Rows[0]["Fstandby1"].ToString().Trim();
                                if (PickType == null || PickType == "0" || PickType == "1" || PickType == "")
                                {
                                    this.lblPickType.Text = "��������";
                                }
                                else if (PickType == "2")
                                {
                                    this.lblPickType.Text = "C2C����";
                                }
                                else if (PickType == "3")
                                {
                                    this.lblPickType.Text = "�Զ�����";
                                }
                                else
                                {
                                    this.lblPickType.Text = "Unknown";
                                }
                            }

                            foreach (DataRow dr in ds3.Tables[0].Rows)
                            {
                                if (ds4.Fatt_id == dr["Value"].ToString())
                                {
                                    this.lblAttType.Text = dr["Text"].ToString();
                                    break;
                                }
                            }

                    }
                    else
                    {
                        this.divInfo.Visible = false;
                        WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�");
                    }
                }
                catch (Exception ex)
                {
                    this.divInfo.Visible = false;
                    WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + ex.Message);
                }
            }
        }

        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                qs.ModifyPayBusinessInfo(ViewState["TableFlag"].ToString(), ViewState["KeyID"].ToString(),
                    this.txtConnetionName.Text.Trim(), this.txtPhone.Text.Trim(), this.txtMobile.Text.Trim(), this.txtQQNo.Text.Trim(), this.txtEmail.Text.Trim(),
                    ViewState["CompanyAddress"].ToString(), ViewState["Postalcode"].ToString());

                DataSet ds = new SPOAService().GetSpInfo("", ViewState["KeyID"].ToString(), "", "", "", "", 2, 0);

                this.txtConnetionName.Text = ds.Tables[0].Rows[0]["ContactUser"].ToString();
                //this.txtPhone.Text = ds.Tables[0].Rows[0]["ContactPhone"].ToString();
                //this.txtMobile.Text = ds.Tables[0].Rows[0]["ContactMobile"].ToString();
                bool isRight_SensitiveRole = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SensitiveRole", this);
                this.txtPhone.Text = classLibrary.setConfig.ConvertTelephoneNumber(ds.Tables[0].Rows[0]["ContactPhone"].ToString(), isRight_SensitiveRole);                
                this.txtMobile.Text = classLibrary.setConfig.ConvertTelephoneNumber(ds.Tables[0].Rows[0]["ContactMobile"].ToString(), isRight_SensitiveRole);
                this.txtQQNo.Text = ds.Tables[0].Rows[0]["ContactQQ"].ToString();
                ControlBtn(true);
                this.txtConnetionName.Enabled = false;
                this.txtPhone.Enabled = false;
                this.txtMobile.Enabled = false;
                this.txtQQNo.Enabled = false;

            }
            catch
            {
                this.divInfo.Visible = false;
                throw new Exception("�����޸�ʧ��!");
            }

        }

        private void ControlBtn(bool IsEdit)
        {
            if (IsEdit)
            {
                this.btnEdit.Visible = true;
                this.btnSave.Visible = false;
                this.btnCancel.Visible = false;
            }
            else
            {
                this.btnEdit.Visible = false;
                this.btnSave.Visible = true;
                this.btnCancel.Visible = true;
            }
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                DataSet ds = new SPOAService().GetSpInfo("", ViewState["KeyID"].ToString(), "", "", "", "", 2, 0);

                if (ds.Tables[0].Rows.Count == 1)
                {
                    this.lblURL.Text = ds.Tables[0].Rows[0]["WWWAdress"].ToString();
                    this.lblType.Text = ds.Tables[0].Rows[0]["TradeName"].ToString();
                    this.lblFsidID.Text = ds.Tables[0].Rows[0]["IDNo"].ToString();
                    this.lblArea.Text = ds.Tables[0].Rows[0]["AreaName"].ToString();
                    this.txtConnetionName.Text = ds.Tables[0].Rows[0]["ContactUser"].ToString();
                    //this.txtPhone.Text = ds.Tables[0].Rows[0]["ContactPhone"].ToString();                   
                    //this.txtMobile.Text = ds.Tables[0].Rows[0]["ContactMobile"].ToString();
                    bool isRight_SensitiveRole = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SensitiveRole", this);
                    this.txtPhone.Text = classLibrary.setConfig.ConvertTelephoneNumber(ds.Tables[0].Rows[0]["ContactPhone"].ToString(), isRight_SensitiveRole);
                    this.txtMobile.Text = classLibrary.setConfig.ConvertTelephoneNumber(ds.Tables[0].Rows[0]["ContactMobile"].ToString(), isRight_SensitiveRole);
                    this.txtEmail.Text = ds.Tables[0].Rows[0]["ContactEmail"].ToString();
                    this.txtQQNo.Text = ds.Tables[0].Rows[0]["ContactQQ"].ToString();
                    this.txtRemark.Text = ds.Tables[0].Rows[0]["ErrorMemo"].ToString();
                }

                this.txtConnetionName.Enabled = false;
                this.txtPhone.Enabled = false;
                this.txtMobile.Enabled = false;
                this.txtQQNo.Enabled = false;
                ControlBtn(true);
            }
            catch
            {
                this.divInfo.Visible = false;
                throw new Exception("���ݶ�ȡʧ��!");
            }
        }
        private void ShowMsg(string msg)
        {
            Response.Write("<script language=javascript>alert('" + msg + "')</script>");
        }
        protected void btnEdit_Click(object sender, System.EventArgs e)
        {
            if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("PayBusinessCMD", this))
            {
                // Response.Redirect("../login.aspx?wh=1");
                ShowMsg("��û��Ȩ�޽��д˲�����������Ҫ��������Ȩ�ޣ�");
                return;
            }

            ControlBtn(false);
            this.txtConnetionName.Enabled = true;
            this.txtPhone.Enabled = true;
            this.txtQQNo.Enabled = true;

            if (ViewState["TableFlag"].ToString() == "ApplyCpInfoX")
                this.txtMobile.Enabled = true;
        }
    }
}
