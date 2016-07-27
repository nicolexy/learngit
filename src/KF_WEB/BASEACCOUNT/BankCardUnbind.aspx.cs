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
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// BankCardUnbind ��ժҪ˵����
    /// </summary>
    public partial class BankCardUnbind : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected Wuqi.Webdiyer.AspNetPager Aspnetpager1;
        bool isRight_SensitiveRole = false;        
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                isRight_SensitiveRole = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SensitiveRole", this);
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();

                if (!ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");

                this.rbtn_bkt_JJK.CheckedChanged += new EventHandler(rbtns_CheckedChanged);
                this.rbtn_bkt_XYK.CheckedChanged += new EventHandler(rbtns_CheckedChanged);
                this.rbtn_bkt_ALL.CheckedChanged += new EventHandler(rbtns_CheckedChanged);

                this.rbtn_all.CheckedChanged += new EventHandler(rbtns_CheckedChanged);
                this.rbtn_fastPay.CheckedChanged += new EventHandler(rbtns_CheckedChanged);
                this.rbtn_ydt.CheckedChanged += new EventHandler(rbtns_CheckedChanged);

                if (!IsPostBack)
                {
                    if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString().Trim() == "edit")
                    {
                        ShowEdit();
                    }

                    btnUnbind.Attributes["onClick"] = "if(!confirm('ȷ��Ҫ�������')) return false;";

                    this.rbtn_all.Checked = true;
                    this.rbtn_bkt_ALL.Checked = true;

                    this.ddl_BankType.Items.Clear();
                    this.ddl_BankType.Items.Add(new ListItem("ȫ��", ""));
                    AddAllBankType();

                    this.pager.RecordCount = 1000;
                    this.pager.PageSize = 10;

                    this.pager1.RecordCount = 1000;
                    this.pager1.PageSize = 10;
                }

                this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);
                this.pager1.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager1_PageChanged);
                this.Datagrid1.ItemCommand += new DataGridCommandEventHandler(Datagrid1_ItemCommand);
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

        }
        #endregion

        private void ShowEdit()
        {
            this.PanelList.Visible = false;
            this.PanelMod.Visible = true;
            
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            // 20130809 ���ݿ��ǣ�FBDIndex=1�󶨱� FBDIndex=2 ��ʱ�󶨱�
            DataSet ds = qs.GetBankCardBind(Request.QueryString["Fuid"].ToString(), Request.QueryString["Findex"].ToString(), Request.QueryString["FBDIndex"].ToString());

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 1)
            {
                //throw new Exception("û�в��ҵ���Ӧ�ļ�¼��");
                WebUtils.ShowMessage(this, "û�в��ҵ���Ӧ�ļ�¼");
                return;
            }
            else
            {
                DataRow dr = ds.Tables[0].Rows[0];

                //֧���޶�lxl
                ds.Tables[0].Columns.Add("Fonce_quota_str", typeof(String));
                ds.Tables[0].Columns.Add("Fday_quota_str", typeof(String));
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fonce_quota", "Fonce_quota_str");
                classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fday_quota", "Fday_quota_str");
                this.lblonce_quota.Text = ds.Tables[0].Rows[0]["Fonce_quota_str"].ToString();
                this.lblday_quota.Text = ds.Tables[0].Rows[0]["Fday_quota_str"].ToString();
                //С�����֪ͨlxl
                if (ds.Tables[0].Rows[0]["sms_flag"].ToString() == "1")
                {
                    this.lbli_character2.Text = "�ѿ���";
                }
                else if (ds.Tables[0].Rows[0]["sms_flag"].ToString() == "0")
                {
                    this.lbli_character2.Text = "�ѹر�";
                }

                this.lblFuin.Text = ds.Tables[0].Rows[0]["Fuin"].ToString();
                string Fbank_type = ds.Tables[0].Rows[0]["Fbank_type"].ToString();

                this.lblFbank_type.Text = classLibrary.getData.GetBankNameFromBankCode(Fbank_type);
                lblFbankType.Text = Fbank_type; //����bank_type yinhuang 2013.7.18

                this.lblFbind_serialno.Text = ds.Tables[0].Rows[0]["Fbind_serialno"].ToString();
                this.lblFprotocol_no.Text = ds.Tables[0].Rows[0]["Fprotocol_no"].ToString();
                string Fbank_status = ds.Tables[0].Rows[0]["Fbank_status"].ToString();

                if (Fbank_status == "0")
                    this.lblFbank_status.Text = "δ����";
                else if (Fbank_status == "1")
                    this.lblFbank_status.Text = "Ԥ��״̬(δ����)";
                else if (Fbank_status == "2")
                    this.lblFbank_status.Text = "��ȷ��(����)";
                else if (Fbank_status == "3")
                    this.lblFbank_status.Text = "�����";
                else
                    this.lblFbank_status.Text = "Unknown";

                string cardTail = ds.Tables[0].Rows[0]["Fcard_tail"].ToString();
                if (cardTail != "" && cardTail.Length > 4)
                {
                    this.lblFcard_tail.Text = cardTail.Substring(cardTail.Length - 4, 4);
                }
                this.lblFcard_tail_db.Text = cardTail;
                
                this.lblFtruename.Text = classLibrary.setConfig.ConvertName(ds.Tables[0].Rows[0]["Ftruename"].ToString(), isRight_SensitiveRole);
                string Fbind_type = ds.Tables[0].Rows[0]["Fbind_type"].ToString();

                if (Fbind_type == "0")
                    this.lblFbind_type.Text = "δ֪����";
                else if (Fbind_type == "1")
                    this.lblFbind_type.Text = "��ͨ��ǿ�����";
                else if (Fbind_type == "2")
                    this.lblFbind_type.Text = "��������������";
                else if (Fbind_type == "3")
                    this.lblFbind_type.Text = "���ÿ�����";
                else if (Fbind_type == "4")
                    this.lblFbind_type.Text = "�ڲ���";
                else if (Fbind_type == "20")
                    this.lblFbank_type.Text = "��ͨ���ÿ�����";
                else
                    this.lblFbind_type.Text = "Unknown";

                string Fbind_status = ds.Tables[0].Rows[0]["Fbind_status"].ToString();

                if (Fbind_status == "0")
                    this.lblFbind_status.Text = "δ����";
                else if (Fbind_status == "1")
                    this.lblFbind_status.Text = "��ʼ״̬";
                else if (Fbind_status == "2")
                    this.lblFbind_status.Text = "����";
                else if (Fbind_status == "3")
                    this.lblFbind_status.Text = "�ر�";
                else if (Fbind_status == "4")
                    this.lblFbind_status.Text = "���";
                else if (Fbind_status == "5")
                    this.lblFbind_status.Text = "�����Ѽ���û�δ����";
                else
                    this.lblFbind_status.Text = "Unknown";

                string Fbind_flag = ds.Tables[0].Rows[0]["Fbind_flag"].ToString();

                if (Fbind_flag == "0")
                    this.lblFbind_flag.Text = "δ֪";
                else if (Fbind_flag == "1")
                    this.lblFbind_flag.Text = "��Ч";
                else if (Fbind_flag == "2")
                    this.lblFbind_flag.Text = "��Ч";
                else
                    this.lblFbind_flag.Text = "Unknown";

                this.lblFbank_id.Text = ds.Tables[0].Rows[0]["Fbank_id"].ToString();
                this.lblFindex.Text = ds.Tables[0].Rows[0]["Findex"].ToString();
                this.lblFuid.Text = ds.Tables[0].Rows[0]["Fuid"].ToString();
                this.txtFmemo.Text = ds.Tables[0].Rows[0]["Fmemo"].ToString();

                try
                {
                    this.lblCreID.Text = classLibrary.setConfig.ConvertCreID(ds.Tables[0].Rows[0]["Fcre_id"].ToString());
                    if (ds.Tables[0].Rows[0]["Fmobilephone"].ToString() != "")
                        this.lblPhone.Text = classLibrary.setConfig.ConvertTelephoneNumber(ds.Tables[0].Rows[0]["Fmobilephone"].ToString(), isRight_SensitiveRole);
                    else
                        this.lblPhone.Text = classLibrary.setConfig.ConvertTelephoneNumber(ds.Tables[0].Rows[0]["Ftelephone"].ToString(), isRight_SensitiveRole); 

                    this.lblUid.Text = ds.Tables[0].Rows[0]["Fuid"].ToString();

                    switch (ds.Tables[0].Rows[0]["Fcre_type"].ToString())
                    {
                        case "1":
                            {
                                this.lblcreType.Text = "���֤"; break;
                            }
                        case "2":
                            {
                                this.lblcreType.Text = "����"; break;
                            }
                        case "3":
                            {
                                this.lblcreType.Text = "����֤"; break;
                            }
                        default:
                            {
                                this.lblcreType.Text = "δ֪"; break;
                            }
                    }
                    if (this.lblcreType.Text == "���֤")
                    {
                        this.lblCreID.Text = classLibrary.setConfig.IDCardNoSubstring(ds.Tables[0].Rows[0]["Fcre_id"].ToString(), isRight_SensitiveRole);
                    }
                    else
                    {
                        this.lblCreID.Text = ds.Tables[0].Rows[0]["Fcre_id"].ToString();
                    }
                    this.lblCreateTime.Text = dr["Fcreate_time"].ToString();
                    this.lblbindTimeLocal.Text = dr["Fbind_time_local"].ToString();
                    this.lblbindTimeBank.Text = dr["Fbind_time_bank"].ToString();
                    this.lblUnbindTimeLocal.Text = dr["Funchain_time_local"].ToString();
                    this.lblUnbindTimeBank.Text = dr["Funchain_time_bank"].ToString();
                }
                catch (Exception ex)
                {
                    WebUtils.ShowMessage(this, ex.Message + ", stacktrace" + ex.StackTrace);

                }

                if (Fbind_flag == "2" && Fbind_status == "4" && Fbank_status == "3")
                    this.btnUnbind.Enabled = false;
                else
                    this.btnUnbind.Enabled = true;
            }
        }

        protected void btnUnbind_Click(object sender, System.EventArgs e)
        {
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

                //20130809 ���Ϊ�ʴ�һ��ͨ������һ��ͨ������һ��ͨ�����ǵ�Э��Ŵ�����bankID�ֶΣ�������ͳһ�Ľ��ʽ
                //���ʱ���ð󶨷�����,ʹ��uid ��β��
                //ʹ�ÿ�β�ţ��������޳���������4λ����ʹ��uid  Fbind_serialno���

                //20140425 lxl ���⣺����һ���û�һ�����п��������ε��������ѯ�������󶨼�¼�����ǽ����
                //��Ҫ��������ʽ��uid  Fbind_serialno�����
                //���Ҫ������������������������������У����ܽ�������¾��ø��ֽ��
                //���ν�UnbindBankCardSpecial�ӿ�����������������
                if (CheckBoxUnbind.Checked == true)
                {
                    DataSet ds = qs.UnbindBankCardSpecial(this.lblFbankType.Text, this.lblFuin.Text, this.lblFcard_tail_db.Text, this.lblFbind_serialno.Text, this.lblFprotocol_no.Text);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        string res_info = dt.Rows[0]["res_info"].ToString();
                        if (res_info != null && res_info == "ok")
                        {
                            WebUtils.ShowMessage(this.Page, "�������������ɹ�");
                        }
                    }
                }
                else
                {
                    //2013/7/18 yinhuang ԭ�����޸ı����������ʹ�ýӿ������
                    qs.UnbindBankCard(this.lblFbankType.Text, this.lblFuin.Text, this.lblFprotocol_no.Text);
                    WebUtils.ShowMessage(this.Page, "���ɹ�");
                }
                this.btnUnbind.Enabled = false;
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr + ", stacktrace" + eSoap.StackTrace);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message + ", stacktrace" + ex.StackTrace);
            }
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("BankCardUnbind.aspx");
        }

        //ͬ��
        protected void btnSynchron_Click(object sender, System.EventArgs e)
        {
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                qs.SynchronBankCardBind(this.lblFbankType.Text, this.lblFcard_tail_db.Text, this.lblFbank_id.Text);
                WebUtils.ShowMessage(this.Page, "ͬ���ɹ�");
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr + ", stacktrace" + eSoap.StackTrace);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message + ", stacktrace" + ex.StackTrace);
            }
        }

        private void BindData_UIN(int index)
        {
            this.pager1.CurrentPageIndex = index;
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                DataSet ds1 = qs.GetBankCardBindList_UIN(this.ddl_BankType.SelectedValue, this.tbx_bankID.Text.Trim(),
                    this.ddl_creType.SelectedValue, this.tbx_creID.Text.Trim(), this.tbx_serNum.Text.Trim(), this.tbx_phone.Text.Trim(),
                    int.Parse(this.ddl_bindStatue.SelectedValue), this.pager1.PageSize * (index - 1), this.pager1.PageSize);
                //������ʵʱ�󶨿��Ϊ�˲鵱���¼lxl
                DataSet ds2 = qs.GetBankCardBindList_UIN_2(this.ddl_BankType.SelectedValue, this.tbx_bankID.Text.Trim(),
                    this.ddl_creType.SelectedValue, this.tbx_creID.Text.Trim(), this.tbx_serNum.Text.Trim(), this.tbx_phone.Text.Trim(),
                    int.Parse(this.ddl_bindStatue.SelectedValue), this.pager1.PageSize * (index - 1), this.pager1.PageSize);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.Tables[0].Columns.Add("fuin", typeof(string));
                ds.Tables[0].Columns.Add("Fbank_typeStr", typeof(string));
                ds.Tables[0].Columns.Add("fcre_id", typeof(string));
                ds.Tables[0].Columns.Add("fbank_id", typeof(string));
                ds.Tables[0].Columns.Add("fcard_tail", typeof(string));//20131017 lxl ���ź���λ
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds1.Tables[0].Rows)
                    {
                        DataRow drResult = dt.NewRow();
                        drResult["fuin"] = dr["fuin"].ToString();
                        drResult["Fbank_typeStr"] = classLibrary.getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());
                        drResult["fcre_id"] = setConfig.ConvertCreID(dr["fcre_id"].ToString());
                        drResult["fbank_id"] = setConfig.ConvertCreID(dr["fbank_id"].ToString());
                        drResult["fcard_tail"] = dr["fcard_tail"].ToString();
                        dt.Rows.Add(drResult);
                    }
                }
                if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds2.Tables[0].Rows)
                    {
                        DataRow drResult = dt.NewRow();
                        drResult["fuin"] = dr["fuin"].ToString();
                        drResult["Fbank_typeStr"] = classLibrary.getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());
                        drResult["fcre_id"] = "";
                        drResult["fbank_id"] = dr["fcard_id"].ToString();
                        drResult["fcard_tail"] = dr["fcard_tail"].ToString();
                        dt.Rows.Add(drResult);
                    }
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    this.Datagrid1.DataSource = ds;
                    this.Datagrid1.DataBind();

                    BindData(ds.Tables[0].Rows[0]["fuin"].ToString(), 1);
                }
                else
                {
                    WebUtils.ShowMessage(this, "��ѯһ��ͨ�˻��б�Ϊ��");
                    this.dgList.DataSource = null;
                    this.dgList.DataBind();
                    return;
                }
            }
            catch (System.Exception ex)
            {
                WebUtils.ShowMessage(this, ex.Message);
            }
        }


        private void BindData(string qqid, int index)
        {
            this.pager.CurrentPageIndex = index;
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                int queryType = 0;
                if (this.rbtn_ydt.Checked)
                {
                    queryType = 1;
                }
                else if (this.rbtn_fastPay.Checked)
                {
                    queryType = 2;
                }

                string beginDateStr = this.tbx_beginDate.Value.Trim();
                string endDateStr = this.tbx_endDate.Value.Trim();

                try
                {
                    if (beginDateStr != "")
                        DateTime.Parse(beginDateStr);

                    if (endDateStr != "")
                        DateTime.Parse(endDateStr);
                }
                catch
                {
                    WebUtils.ShowMessage(this.Page, "��������ȷ�����ڸ�ʽ");
                    return;
                }

                DataSet ds = qs.GetBankCardBindList_2(qqid.Trim(), this.ddl_BankType.SelectedValue, this.tbx_bankID.Text.Trim(),
                    this.ddl_creType.SelectedValue, this.tbx_creID.Text.Trim(), this.tbx_serNum.Text.Trim(),
                    this.tbx_phone.Text.Trim(), beginDateStr, endDateStr, queryType,
                    int.Parse(this.ddl_bindStatue.SelectedValue), (index - 1) * this.pager.PageSize, this.pager.PageSize);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    this.dgList.DataSource = null;
                    this.dgList.DataBind();
                    WebUtils.ShowMessage(this, "��ѯһ��ͨ��ϢΪ��");
                    return;
                }
                else
                {
                    DataTable dt = ds.Tables[0];
                    dt.Columns.Add("Fbank_typeStr", typeof(string));
                    dt.Columns.Add("Fbank_statusStr", typeof(string));
                    dt.Columns.Add("Fxyzf_typeStr", typeof(string)); //����֧������

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!(dr["Fi_character4"] is DBNull))
                        {
                            if (dr["Fi_character4"].ToString() == "33")
                            {
                                dr["Fxyzf_typeStr"] = "��";
                            }
                            else
                            {
                                dr["Fxyzf_typeStr"] = "��";
                            }
                        }
                        else
                        {
                            dr["Fxyzf_typeStr"] = "��";
                        }

                        dr["Fbank_typeStr"] = classLibrary.getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());                        
                        dr["Ftruename"] = classLibrary.setConfig.ConvertName(dr["Ftruename"].ToString(), isRight_SensitiveRole);
                        
                        if (dr["Fbank_status"].ToString() == "0")
                            dr["Fbank_statusStr"] = "δ����";
                        else if (dr["Fbank_status"].ToString() == "1")
                            dr["Fbank_statusStr"] = "Ԥ��״̬(δ����)";
                        else if (dr["Fbank_status"].ToString() == "2")
                            dr["Fbank_statusStr"] = "��ȷ��(����)";
                        else if (dr["Fbank_status"].ToString() == "3")
                            dr["Fbank_statusStr"] = "�����";
                        else
                            dr["Fbank_statusStr"] = "Unknown";
                    }
                    this.dgList.DataSource = dt.DefaultView;
                    dgList.DataBind();
                    Session["qqid"] = this.dgList.Items[0].Cells[3].Text.Trim();
                }
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr + ", stacktrace" + eSoap.StackTrace);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString() + ", stacktrace" + eSys.StackTrace);
            }
        }

        private void BindData(int index)
        {
            this.pager.CurrentPageIndex = index;
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                DataSet ds = null;

                int queryType = 0;

                if (this.rbtn_ydt.Checked)
                {
                    queryType = 1;
                }
                else if (this.rbtn_fastPay.Checked)
                {
                    queryType = 2;
                }

                string beginDateStr = this.tbx_beginDate.Value.Trim();
                string endDateStr = this.tbx_endDate.Value.Trim();

                try
                {
                    if (beginDateStr != "")
                        DateTime.Parse(beginDateStr);

                    if (endDateStr != "")
                        DateTime.Parse(endDateStr);
                }
                catch
                {
                    WebUtils.ShowMessage(this.Page, "��������ȷ�����ڸ�ʽ");
                    return;
                }

                ds = qs.GetBankCardBindList_New(this.txtQQ.Text.Trim(), this.ddl_BankType.SelectedValue, this.tbx_bankID.Text.Trim(),
                    this.tbx_uid.Text.Trim(), this.ddl_creType.SelectedValue, this.tbx_creID.Text.Trim(), this.tbx_serNum.Text.Trim(),
                    this.tbx_phone.Text.Trim(), beginDateStr, endDateStr, queryType, this.cbx_showAbout.Checked,
                    int.Parse(this.ddl_bindStatue.SelectedValue), "", (index - 1) * this.pager.PageSize, this.pager.PageSize);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    throw new Exception("û�в��ҵ���Ӧ�ļ�¼��");
                }
                else
                {
                    DataTable dt = ds.Tables[0];
                    dt.Columns.Add("Fbank_typeStr", typeof(string));
                    dt.Columns.Add("Fbank_statusStr", typeof(string));
                    dt.Columns.Add("Fxyzf_typeStr", typeof(string)); //����֧������

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!(dr["Fi_character4"] is DBNull))
                        {
                            if (dr["Fi_character4"].ToString() == "33")
                            {
                                dr["Fxyzf_typeStr"] = "��";
                            }
                            else
                            {
                                dr["Fxyzf_typeStr"] = "��";
                            }
                        }
                        else
                        {
                            dr["Fxyzf_typeStr"] = "��";
                        }

                        dr["Fbank_typeStr"] = classLibrary.getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());
                        dr["Ftruename"] = classLibrary.setConfig.ConvertName(dr["Ftruename"].ToString(), isRight_SensitiveRole);
                        if (dr["Fbank_status"].ToString() == "0")
                            dr["Fbank_statusStr"] = "δ����";
                        else if (dr["Fbank_status"].ToString() == "1")
                            dr["Fbank_statusStr"] = "Ԥ��״̬(δ����)";
                        else if (dr["Fbank_status"].ToString() == "2")
                            dr["Fbank_statusStr"] = "��ȷ��(����)";
                        else if (dr["Fbank_status"].ToString() == "3")
                            dr["Fbank_statusStr"] = "�����";
                        else
                            dr["Fbank_statusStr"] = "Unknown";
                    }
                    this.dgList.DataSource = dt.DefaultView;
                    dgList.DataBind();
                    Session["qqid"] = this.dgList.Items[0].Cells[3].Text.Trim();
                }
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr + ", stacktrace" + eSoap.StackTrace);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString() + ", stacktrace" + eSys.StackTrace);
            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            if (this.tbx_uid.Text.Trim() != "" || this.txtQQ.Text.Trim() != "")
            {
                BindData(1);
            }
            else
            {
                BindData_UIN(1);
            }
        }

        private void rbtns_CheckedChanged(object sender, EventArgs e)
        {
            getData.BankClass[] bkInfoList = null;
            this.ddl_BankType.Items.Clear();

            this.ddl_BankType.Items.Add(new ListItem("ȫ��", ""));

            if (this.rbtn_fastPay.Checked)
            {
                if (this.rbtn_bkt_JJK.Checked)
                {
                    bkInfoList = getData.GetFPBankList(1);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }
                }
                else if (this.rbtn_bkt_XYK.Checked)
                {
                    bkInfoList = getData.GetFPBankList(2);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }
                }
                else if (this.rbtn_bkt_ALL.Checked)
                {
                    bkInfoList = getData.GetFPBankList(1);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }

                    bkInfoList = getData.GetFPBankList(2);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }
                }
            }
            else if (this.rbtn_ydt.Checked)
            {
                if (this.rbtn_bkt_JJK.Checked)
                {
                    bkInfoList = getData.GetOPBankList(1);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }
                }
                else if (this.rbtn_bkt_XYK.Checked)
                {
                    bkInfoList = getData.GetOPBankList(2);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }
                }
                else if (this.rbtn_bkt_ALL.Checked)
                {
                    bkInfoList = getData.GetOPBankList(1);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }

                    bkInfoList = getData.GetOPBankList(2);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }
                }
            }
            else if (this.rbtn_all.Checked)
            {
                if (this.rbtn_bkt_JJK.Checked)
                {
                    bkInfoList = getData.GetOPBankList(1);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }

                    bkInfoList = getData.GetFPBankList(1);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }
                }
                else if (this.rbtn_bkt_XYK.Checked)
                {
                    bkInfoList = getData.GetOPBankList(2);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }

                    bkInfoList = getData.GetFPBankList(2);
                    for (int i = 0; i < bkInfoList.Length; i++)
                    {
                        this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
                    }
                }
                else if (this.rbtn_bkt_ALL.Checked)
                {
                    AddAllBankType();
                }
            }
        }

        private void AddAllBankType()
        {
            getData.BankClass[] bkInfoList = null;

            bkInfoList = getData.GetOPBankList(1);
            for (int i = 0; i < bkInfoList.Length; i++)
            {
                this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
            }

            bkInfoList = getData.GetOPBankList(2);
            for (int i = 0; i < bkInfoList.Length; i++)
            {
                this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
            }

            bkInfoList = getData.GetFPBankList(1);
            for (int i = 0; i < bkInfoList.Length; i++)
            {
                this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
            }

            bkInfoList = getData.GetFPBankList(2);
            for (int i = 0; i < bkInfoList.Length; i++)
            {
                this.ddl_BankType.Items.Add(new ListItem(bkInfoList[i].bankName, bkInfoList[i].bankValue.ToString()));
            }
        }

        private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            //20130828 lxl ���ط�ҳ�������޸�
            string qqid = Session["qqid"].ToString();

            this.BindData(qqid, e.NewPageIndex);
        }

        private void pager1_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            this.BindData_UIN(e.NewPageIndex);
        }

        private void Datagrid1_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "query")
            {
                this.BindData(e.Item.Cells[0].Text.Trim(), 1);
            }
        }
    }
}
