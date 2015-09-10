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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using CFT.CSOMS.BLL.CFTAccountModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// ChangeUserName ��ժҪ˵����
    /// </summary>
    public partial class ChangeUserInfo : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.ImageButton ImageButton3;
        protected System.Web.UI.WebControls.Button Button2_Submit;
        protected System.Web.UI.WebControls.Button Button3_Cancel;
        protected System.Web.UI.WebControls.LinkButton LinkButton1_Edit;
        protected System.Web.UI.WebControls.LinkButton Linkbutton2_Update;
        protected System.Web.UI.WebControls.TextBox Textbox10_Faddress;
        protected System.Web.UI.WebControls.TextBox Textbox11_Fpcode;
        protected System.Web.UI.WebControls.TextBox Textbox13_Fcreid;
        protected System.Web.UI.WebControls.TextBox Textbox7_Fmobile;
        protected System.Web.UI.WebControls.TextBox Textbox6_Fphone;
        protected System.Web.UI.WebControls.TextBox Textbox7_Femail;
        protected System.Web.UI.WebControls.TextBox Textbox5_Fage;
        protected System.Web.UI.WebControls.TextBox TextBox2_Ftruename;
        protected System.Web.UI.WebControls.Label Label1_Fqqid;
        protected System.Web.UI.WebControls.TextBox TX_QQID;
        protected System.Web.UI.WebControls.TextBox TX_Fmodify_time;
        protected System.Web.UI.WebControls.TextBox TX_Memo;
        protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
        protected System.Web.UI.WebControls.DropDownList DropDownList1_Sex;
        protected System.Web.UI.WebControls.DropDownList DropDownList2_certify;
        protected System.Web.UI.WebControls.Label Label_uid;
        protected System.Web.UI.WebControls.Button Button1;

        public string iprov, icity;
        protected System.Web.UI.HtmlControls.HtmlSelect area;
        protected System.Web.UI.WebControls.RegularExpressionValidator RegularExpressionValidator4;
        protected System.Web.UI.WebControls.RegularExpressionValidator RegularExpressionValidator5;
        protected System.Web.UI.HtmlControls.HtmlInputHidden Harea;
        protected System.Web.UI.WebControls.TextBox Textbox4_Company;
        protected System.Web.UI.WebControls.RegularExpressionValidator RegularExpressionValidator6;
        protected System.Web.UI.WebControls.DropDownList ddlType;
        protected System.Web.UI.WebControls.Button Button_Update;
        protected System.Web.UI.WebControls.Button Button3;
        protected System.Web.UI.HtmlControls.HtmlInputHidden Hcity;
        protected System.Web.UI.WebControls.DropDownList ddlAttid;
        protected System.Web.UI.HtmlControls.HtmlSelect city;

        private void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��
            CheckInput();

            try
            {
                this.Label_uid.Text = Session["uid"].ToString();

                string sr = Session["SzKey"].ToString();
                if (!ClassLib.ValidateRight("ChangeUserInfo", this)) Response.Redirect("../login.aspx?wh=1");

            }
            catch  //���û�е�½����û��Ȩ�޾�����
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if (!Page.IsPostBack)
            {
                setInfoNull();
                initBasicInfo();
            }

            SetButtonVisible();
        }

        private void SetButtonVisible()
        {
            string sr = Session["SzKey"].ToString();
            //�ͷ�ϵͳ��Ҫ����
            //	bool userright = AllUserRight.GetOneRightState("ChangeUser",sr); //�����ʻ�
            //	if(!userright) LinkButton1_Edit.Visible = false;
        }

        private void CheckInput()
        {
            #region
            Button1.Attributes.Add("onclick", "return checkvlid(this);");
            #endregion
        }
        private void setInfoNull()
        {
            this.Label1_Fqqid.Text = "";
            this.TextBox2_Ftruename.Text = "";
            this.Textbox4_Company.Text = "";
            this.Textbox5_Fage.Text = "";
            this.Textbox6_Fphone.Text = "";
            this.Textbox7_Fmobile.Text = "";

            this.Textbox7_Femail.Text = "";
            this.Textbox10_Faddress.Text = "";
            this.Textbox11_Fpcode.Text = "";
            this.Textbox13_Fcreid.Text = "";

            this.TX_Memo.Text = "";
            this.TX_Fmodify_time.Text = "";
            this.DropDownList1_Sex.Visible = false;
            //furion 20060816
            this.ddlAttid.Visible = false;
            this.DropDownList2_certify.Visible = false;

        }

        private void BindInfo(int istr, int imax)
        {
            if (Session["uid"] == null)
            {
                Response.Redirect("../login.aspx?wh=1"); //���µ�½
            }

            Query_Service.Query_Service myService = new Query_Service.Query_Service();
            myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

            DataSet ds = myService.GetUserInfo(this.TX_QQID.Text.Trim(), istr, imax);
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                throw new Exception("���ݿ��޴˼�¼");
            }
            //Response.Write("DS:" + ds.Tables[0].Rows[0][0].ToString());
            ViewState["qqid"] = this.TX_QQID.Text.Trim();

            this.Label1_Fqqid.Text = ds.Tables[0].Rows[0]["Fqqid"].ToString();
            this.TextBox2_Ftruename.Text = ds.Tables[0].Rows[0]["Ftruename"].ToString();
            this.DropDownList1_Sex.Visible = true;

            //furion 20060816
            this.ddlAttid.Visible = true;
            string fattid = classLibrary.setConfig.GetStringStr(ds.Tables[0].Rows[0]["Fatt_id"]);
            this.ddlAttid.SelectedValue = fattid;
            ViewState["Fatt_id"] = fattid;
            string fsex = classLibrary.setConfig.GetStringStr(ds.Tables[0].Rows[0]["Fsex"]);
            this.DropDownList1_Sex.SelectedValue = fsex;
            ViewState["Fsex"] = fsex;
            string fcompany_name = classLibrary.setConfig.GetStringStr(ds.Tables[0].Rows[0]["Fcompany_name"]);
            this.Textbox4_Company.Text = fcompany_name;
            ViewState["Fcompany_name"] = fcompany_name;
            string fage = classLibrary.setConfig.GetStringStr(ds.Tables[0].Rows[0]["Fage"]);
            this.Textbox5_Fage.Text = fage;
            ViewState["Fage"] = fage;
            string fphone = classLibrary.setConfig.GetStringStr(ds.Tables[0].Rows[0]["Fphone"]);
            this.Textbox6_Fphone.Text = fphone;
            ViewState["Fphone"] = fphone;
            string fmobile = classLibrary.setConfig.GetStringStr(ds.Tables[0].Rows[0]["Fmobile"]);
            this.Textbox7_Fmobile.Text = fmobile;
            ViewState["Fmobile"] = fmobile;
            string femail = classLibrary.setConfig.GetStringStr(ds.Tables[0].Rows[0]["Femail"]);
            this.Textbox7_Femail.Text = femail;
            ViewState["Femail"] = femail;
            string faddress = classLibrary.setConfig.GetStringStr(ds.Tables[0].Rows[0]["Faddress"]);
            this.Textbox10_Faddress.Text = faddress;
            ViewState["Faddress"] = faddress;

            string fpcode = classLibrary.setConfig.GetStringStr(ds.Tables[0].Rows[0]["Fpcode"]);
            this.Textbox11_Fpcode.Text = fpcode;
            ViewState["Fpcode"] = fpcode;
            this.DropDownList2_certify.Visible = true;
            string fcre_type = ds.Tables[0].Rows[0]["Fcre_type"].ToString();
            this.DropDownList2_certify.SelectedValue = fcre_type;
            ViewState["Fcre_type"] = fcre_type;
            string fcreid = classLibrary.setConfig.GetStringStr(ds.Tables[0].Rows[0]["Fcreid"]);
            this.Textbox13_Fcreid.Text = fcreid;
            ViewState["Fcreid"] = fcreid;
            string fmemo = classLibrary.setConfig.GetStringStr(ds.Tables[0].Rows[0]["Fmemo"]);
            this.TX_Memo.Text = fmemo;
            ViewState["Fmemo"] = fmemo;

            this.TX_Fmodify_time.Text = classLibrary.setConfig.GetStringStr(ds.Tables[0].Rows[0]["Fmodify_time"]);

            string farea = ds.Tables[0].Rows[0]["Farea"].ToString();
            iprov = farea;

            string fcity = ds.Tables[0].Rows[0]["Fcity"].ToString();
            icity = fcity;

            //����û��ʻ���Ϣ
            string userType = null;
            string Msg = null;
            string qqid = this.TX_QQID.Text.Trim();
            Finance_ManageService.Finance_Manage fm = new Finance_ManageService.Finance_Manage();
            bool exeSign = fm.getUserType(qqid, out userType, out Msg);

            if (exeSign == false)
            {
                WebUtils.ShowMessage(this.Page, "��ѯ�û��ʻ�����ʧ�ܡ�[" + Msg + "]");
                return;
            }

            this.ddlType.SelectedValue = userType.Trim();

            ViewState["Farea"] = farea;
            ViewState["Fcity"] = fcity;
            ViewState["UserType"] = userType.Trim();

        }

        private void ModifyInfo()
        {
            if (Session["uid"] == null)
            {
                Response.Redirect("../login.aspx?wh=1"); //���µ�½
            }

            try
            {
                //�������Դ����
                string[,] mi = new string[16, 4];  //mi: ModifyInfo

                mi[0, 0] = "Fqqid";
                //mi[0,1] = this.Label1_Fqqid.Text.Trim();
                mi[0, 1] = ViewState["qqid"].ToString().Trim();
                mi[0, 2] = ViewState["qqid"].ToString().Trim();
                mi[0, 3] = "QQ�ʺ�";

                mi[1, 0] = "Ftruename";
                mi[1, 1] = setConfig.replaceSqlStr(this.TextBox2_Ftruename.Text.Trim());
                mi[1, 2] = setConfig.replaceSqlStr(this.TextBox2_Ftruename.Text.Trim());
                mi[1, 3] = "��ʵ����";

                mi[2, 0] = "Fsex";
                mi[2, 1] = this.DropDownList1_Sex.SelectedValue;
                mi[2, 2] = ViewState["Fsex"].ToString().Trim();
                mi[2, 3] = "�Ա�";

                mi[3, 0] = "Fcompany_name";
                mi[3, 1] = setConfig.replaceSqlStr(this.Textbox4_Company.Text.Trim());
                mi[3, 2] = ViewState["Fcompany_name"].ToString().Trim();
                mi[3, 3] = "��˾����";

                mi[4, 0] = "Fage";
                mi[4, 1] = this.Textbox5_Fage.Text.Trim();
                mi[4, 2] = ViewState["Fage"].ToString().Trim();
                mi[4, 3] = "����";

                mi[5, 0] = "Fphone";
                mi[5, 1] = this.Textbox6_Fphone.Text.Trim();
                mi[5, 2] = ViewState["Fphone"].ToString().Trim();
                mi[5, 3] = "�̶��绰";

                mi[6, 0] = "Fmobile";
                mi[6, 1] = this.Textbox7_Fmobile.Text.Trim();
                mi[6, 2] = ViewState["Fmobile"].ToString().Trim();
                mi[6, 3] = "�ֻ�����";

                mi[7, 0] = "Femail";
                mi[7, 1] = this.Textbox7_Femail.Text.Trim().ToLower();
                mi[7, 2] = ViewState["Femail"].ToString().Trim();
                mi[7, 3] = "�û�Email";

                mi[8, 0] = "Farea";
                mi[8, 1] = this.Harea.Value;
                mi[8, 2] = ViewState["Farea"].ToString().Trim();
                mi[8, 3] = "ʡ��";

                mi[9, 0] = "Fcity";
                mi[9, 1] = this.Hcity.Value;
                mi[9, 2] = ViewState["Fcity"].ToString().Trim();
                mi[9, 3] = "����";

                mi[10, 0] = "Faddress";
                mi[10, 1] = setConfig.replaceSqlStr(this.Textbox10_Faddress.Text.Trim());
                mi[10, 2] = ViewState["Faddress"] == null ? "" : ViewState["Faddress"].ToString().Trim();
                mi[10, 3] = "��ϵ��ַ";

                mi[11, 0] = "Fpcode";
                mi[11, 1] = this.Textbox11_Fpcode.Text.Trim();
                mi[11, 2] = ViewState["Fpcode"].ToString().Trim();
                mi[11, 3] = "��������";

                mi[12, 0] = "Fcre_type";
                mi[12, 1] = this.DropDownList2_certify.SelectedValue;
                mi[12, 2] = ViewState["Fcre_type"].ToString().Trim();
                mi[12, 3] = "֤������";

                mi[13, 0] = "Fcreid";
                mi[13, 1] = classLibrary.setConfig.replaceSqlStr(this.Textbox13_Fcreid.Text.Trim());
                mi[13, 2] = ViewState["Fcreid"].ToString().Trim();
                mi[13, 3] = "֤������";

                mi[14, 0] = "Fmemo";
                mi[14, 1] = setConfig.replaceSqlStr(this.TX_Memo.Text.Trim());
                mi[14, 2] = ViewState["Fmemo"].ToString().Trim();
                mi[14, 3] = "��ע";

                mi[15, 0] = "Fmodify_time";
                mi[15, 1] = this.TX_Fmodify_time.Text.Trim();
                mi[15, 2] = this.TX_Fmodify_time.Text.Trim();
                mi[15, 3] = "";
       
                Query_Service.Query_Service myService = new Query_Service.Query_Service();
                myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

                T_FIELD_VALUE[] tfv = new T_FIELD_VALUE[16];
                int i;
                for (i = 0; i < 16; i++)
                {
                    //tfv[i] = new TENCENT.OSS.C2C.Finance.Finance_Web.Modify_Service.T_FIELD_VALUE();
                    tfv[i] = new T_FIELD_VALUE();
                    tfv[i].FieldName = mi[i, 0];
                    tfv[i].FieldValue = mi[i, 1];
                    tfv[i].FOldValue = mi[i, 2];
                    tfv[i].FTrueName = mi[i, 3];
                }

                myService.ModifyUserInfo(tfv);

                //�޸��ʻ�����
                string Msg = null;
                bool exeSign = myService.modifyUserType(this.TX_QQID.Text.Trim(), this.ddlType.SelectedValue, ViewState["UserType"].ToString().Trim(), out Msg);

                if (exeSign == false)
                {
                    WebUtils.ShowMessage(this.Page, "�޸��ʻ�����ʧ�ܣ�[" + Msg + "] �����ԣ�");
                    throw new Exception("�޸��ʻ�����ʧ�ܣ�[" + Msg + "] �����ԣ�");
                }

                //furion 20060816
                string oldatttype = ViewState["Fatt_id"] == null ? this.ddlAttid.SelectedValue : ViewState["Fatt_id"].ToString().Trim();

                exeSign = myService.modifyAttType(this.TX_QQID.Text.Trim(), this.ddlAttid.SelectedValue, oldatttype, out Msg);

                if (exeSign == false)
                {
                    WebUtils.ShowMessage(this.Page, "�޸��ʻ�����ʧ�ܣ�[" + Msg + "] �����ԣ�");
                    throw new Exception("�޸��ʻ�����ʧ�ܣ�[" + Msg + "] �����ԣ�");
                }
            }
            catch (SoapException er) //����soap��
            {
                string str = PublicRes.GetErrorMsg(er.Message.ToString());
                WebUtils.ShowMessage(this.Page, "����ʧ�ܣ�" + str);
                throw new Exception("����ʧ�ܣ�" + str);
            }
            catch (Exception errStr)
            {
                throw new Exception("ʧ�ܣ�" + errStr);
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
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            this.LinkButton1_Edit.Click += new System.EventHandler(this.LinkButton1_Edit_Click);
            this.Button_Update.Click += new System.EventHandler(this.Button_Update_Click);
            this.Button3.Click += new System.EventHandler(this.Button3_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        private void Button1_Click(object sender, System.EventArgs e)
        {
            //Response.Redirect("../TradeManage/Modify_Succ.aspx");
            if (string.IsNullOrEmpty(this.TX_QQID.Text.Trim()))
            {
                WebUtils.ShowMessage(this.Page, "�������˺ţ�");
            }
            this.logPager.RecordCount = 1000;
            selectClick();
            returnState();
        }

        private void selectClick()
        {
            try
            {
                BindInfo(1, 1);
                BindDataLog(1);//��ѯ��־
                //furion 20061116 email��¼�޸�
                ViewState["qqid"] = this.TX_QQID.Text.Trim();
            }
            catch (SoapException eSoap)
            {
                setInfoNull(); //���ʧ�ܣ��������
                string str = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, str);
            }
            catch (Exception emsg)
            {
                string str = PublicRes.GetErrorMsg(emsg.Message.ToString());
                WebUtils.ShowMessage(this.Page, str);
            }
        }

        private void BindDataLog(int index)
        {
            string qqid = this.TX_QQID.Text.Trim();
            this.logPager.CurrentPageIndex = index;

            //���ӷ�ҳ
            int max = logPager.PageSize;
            int start = max * (index - 1);

            var dt = new AccountService().QueryChangeUserInfoLog(this.TX_QQID.Text.Trim(), start, max);
            if (dt != null && dt.Rows.Count > 0)
            {              
                dgLog.DataSource = dt.DefaultView;
                dgLog.DataBind();
            }
            else
            {
                dgLog.DataSource = null;
                dgLog.DataBind();
            }
        }

        private void initBasicInfo()
        {
            //�������ֵ��ж�ȡ���ݣ��󶨵�webҳ

            //��֤������
            setConfig.bindDic("CRE_TYPE", this.DropDownList2_certify); //��ѯ֤������

            //���Ա�
            //			setConfig.bindDic("SEX",this.DropDownList1_Sex);//�Ա�

            //furion 20060816
            string Msg = "";

            PublicRes.BindDropDownList(PermitPara.QueryDicAccName(), ddlAttid, out Msg);

            Hashtable htCertify = new Hashtable();
            foreach (ListItem item in DropDownList2_certify.Items)
            {
                htCertify.Add(item.Value.ToString(), item.Text.ToString());
            }

            Hashtable htAttid = new Hashtable();
            foreach (ListItem item in ddlAttid.Items)
            {
                htAttid.Add(item.Value.ToString(), item.Text.ToString());
            }
            ViewState["htCertify"] = htCertify;
            ViewState["htAttid"] = htAttid;

        }

        /// �༭
        private void LinkButton1_Edit_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.TX_Memo.ReadOnly = false;      
                this.TX_Memo.BorderWidth = 1;
                this.TX_Memo.BackColor = Color.GreenYellow;

                this.DropDownList2_certify.Enabled = true;//֤�����Ϳ�ѡ
                this.ddlType.Enabled = true;//�˻����Ϳ�ѡ
                this.LinkButton1_Edit.Visible = false;
                //this.Linkbutton2_Update.Visible = true;
                this.Button_Update.Visible = true;
                this.Button3.Visible = true;

                SetButtonVisible();

                Hashtable htCanModify = new Hashtable();
                htCanModify.Add("1", "��ͨ�����˻�");
                htCanModify.Add("106", "��Q��ע���û�");
                htCanModify.Add("15", "paipai����5��");
                htCanModify.Add("17", "paipai����5��II");
                htCanModify.Add("19", "ֱͨ�������û�");
                htCanModify.Add("31", "paipai��ע��");
                htCanModify.Add("32", "email��ע��");
                htCanModify.Add("34", "200W����");
                htCanModify.Add("36", "paipai����10��");
                htCanModify.Add("41", "QQ����");
                htCanModify.Add("46", "paipai����30��");
                htCanModify.Add("72", "������������2W");
                htCanModify.Add("73", "������������20W");
                htCanModify.Add("79", "��˾����Ա������");
                htCanModify.Add("0", "��һ��");
                ViewState["htCanModify"] = htCanModify;

                if (!htCanModify.ContainsKey(this.ddlAttid.SelectedValue))
                    this.ddlAttid.Enabled = false;//��Ʒ���Բ���ѡ
                else
                    this.ddlAttid.Enabled = true;//��Ʒ���Կ�ѡ
            }
            catch (SoapException eSoap)
            {
                string str = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "����ʧ��:" + str);
            }
            catch (Exception er)
            {
                WebUtils.ShowMessage(this.Page, er.Message.ToString().Replace("'", "��"));
            }
            //BindInfo();
        }

        private void returnState()
        {
            //����֮����Ҫ�ָ�
            this.TX_Memo.ReadOnly = true;
            this.TX_Memo.BorderWidth = 0;
            this.TX_Memo.BackColor = Color.White;
            this.DropDownList2_certify.Enabled = false;//֤�����Ϳ�ѡ
            this.ddlType.Enabled = false;//�˻����Ϳ�ѡ
            this.ddlAttid.Enabled = false;//��Ʒ���Կ�ѡ
            this.DropDownList1_Sex.Enabled = false;//�Ա�
        }

        private void Hcity_ServerChange(object sender, System.EventArgs e)
        {
            //			 Hcity.Value
        }

        private void Button12_Click(object sender, System.EventArgs e)
        {
            Response.Write("ѡ���city:" + this.Hcity.Value);
            Response.Write("ѡ���area:" + this.Harea.Value);
        }

        ///ȡ��
        private void Button3_Click(object sender, System.EventArgs e)
        {
            //��ť��ʾ�ĸ���
            //this.Linkbutton2_Update.Visible = false;
            this.LinkButton1_Edit.Visible = true;
            this.Button_Update.Visible = false;
            this.Button3.Visible = false;

            //			setInfoNull();
            returnState();

            SetButtonVisible();
        }

        //����
        private void Button_Update_Click(object sender, System.EventArgs e)
        {
            try
            {
                Hashtable htCanModify = (Hashtable)ViewState["htCanModify"];

                if (htCanModify.ContainsKey(ViewState["Fatt_id"].ToString().Trim()))
                {
                    if (!htCanModify.ContainsKey(this.ddlAttid.SelectedValue))
                    {
                        WebUtils.ShowMessage(this.Page, "�˻����Բ����໥�޸����У� 1:��ͨ�����˻�/34:200W����/36:paipai����10��/41:QQ����/" +
                        "46 paipai����30��/72  ������������2W/73������������20W"
                        + "/31:paipai��ע��/32:email��ע��/106:��Q��ע���û�/19:ֱͨ�������û�"
                        + "/15:paipai����5��/17:paipai����5��II/79:��˾����Ա������");
                        return;
                    }
                }

                //��ͬʱ�޸�3�ű��õ����� t_user_info,t_user,t_bank_user
                Finance_ManageService.Finance_Manage fm = new Finance_ManageService.Finance_Manage();
                Finance_ManageService.Finance_Header fh = new Finance_ManageService.Finance_Header();
                fh.UserName = Session["uid"].ToString();
                fh.UserIP = Request.UserHostAddress;
                fm.Finance_HeaderValue = fh;

                //furion 20061116 email��¼�޸�				
                //string qqid        = setConfig.replaceMStr(this.Label1_Fqqid.Text.Trim());
                string qqid = setConfig.replaceSqlStr(ViewState["qqid"].ToString());

                string changedName = setConfig.replaceSqlStr(this.TextBox2_Ftruename.Text.Trim());
                string changedCom = setConfig.replaceSqlStr(this.Textbox4_Company.Text.Trim());

                if (changedCom != null && changedCom.Trim() != "")//ֻ�����޸Ĺ�˾���Ƶ���� andrew 20130820
                {
                    fm.modifyName(qqid, changedName, changedCom);
                }

                //�޸��û�����
                ModifyInfo();

                //��ť��ʾ�ĸ���
                //this.Linkbutton2_Update.Visible = false;
                this.LinkButton1_Edit.Visible = true;
                this.Button_Update.Visible = false;
                this.Button3.Visible = false;

                returnState();  //�ָ���ʽ

                WebUtils.ShowMessage(this.Page, "�����޸ĳɹ���");
                //��¼������־
                //PublicRes.writeSysLog(Session["uid"].ToString(),Request.UserHostAddress,"changeUserInfo","�޸��û�����",1,this.TX_QQID.Text,"changeUserInfo");
                //�ͷ�ϵͳֻ�����޸ģ�֤�����͡��˻����͡���Ʒ���ԡ���ע��������¼��־
                string commet = setConfig.replaceSqlStr(this.TX_Memo.Text.Trim());
                string oldatttype = ViewState["Fatt_id"] == null ? this.ddlAttid.SelectedValue : ViewState["Fatt_id"].ToString().Trim();
                new AccountService().AddChangeUserInfoLog(qqid,
                    this.DropDownList2_certify.SelectedValue, ViewState["Fcre_type"].ToString().Trim(),
                    this.ddlType.SelectedValue, ViewState["UserType"].ToString().Trim(),
                    this.ddlAttid.SelectedValue, oldatttype,
                    commet, ViewState["Fmemo"].ToString().Trim(),
                    Session["uid"].ToString());

                SetButtonVisible();

                //ˢ���޸�
                selectClick();

                //���cache
                //	PublicRes.ReleaseCache(qqid,"qqid");
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception er)
            {
                WebUtils.ShowMessage(this.Page, "�����޸��쳣�� ��ϸԭ��" + er.Message.ToString().Replace("'", "��"));
            }
        }

        private void CustomValidator1_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            if (args.Value.IndexOf("@") == -1)
            {
                args.IsValid = false;
            }

        }

        protected void logPager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                this.logPager.CurrentPageIndex = e.NewPageIndex;
                BindDataLog(e.NewPageIndex);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, string.Format("��ҳ�쳣:{0}", ex.Message));
            }
        }

    }
}
