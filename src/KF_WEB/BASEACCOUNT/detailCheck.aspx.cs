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
    /// detailCheck ��ժҪ˵����
    /// </summary>
    public partial class detailCheck : System.Web.UI.Page
    {

        string id, strType, uid, exedSign;   //exedSign��ʾ�Ƿ�ִ�еı�־ ,checkResult��ʾ�Ƿ�ͬ�⣬
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
            // �ڴ˴������û������Գ�ʼ��ҳ��
            if (Session["uid"] == null)
                Response.Write("<script language=javascript>window.parent.location='../login.aspx?wh=1'</script>");
            else
                uid = Session["uid"].ToString();

            if (!Page.IsPostBack)
            {
                //btRefuse.Attributes["onClick"] = "if(!confirm('ȷ���ܾ�������������')) return false;";
                //btFail.Attributes["onClick"] = "if(!confirm('ȷ��������������')) return false;";

                id = Request.QueryString["id"].ToString();
                strType = Request.QueryString["type"].ToString();
                sign = Request.QueryString["sign"].ToString();
                right = Request.QueryString["right"].ToString();

                ViewState["id"] = id;
                ViewState["strType"] = strType;
                ViewState["sign"] = sign; //check������������uncheck����δ����
                ViewState["right"] = right; //notice�����ҹ�ע�� false ������� true �������

                BindData();
            }
            else
            {
                id = ViewState["id"].ToString();                 //���ˢ�� �ᱣ��״̬
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
            if (sign == "uncheck")          //δ���������
            {
                if (right == "false")      //�쿴�Լ���������û��Ȩ���޸�
                    this.dtDetail = cs.GetStartCheckData(strType, uid.Trim().ToLower(), 1, 6, id);
                //else if (right == "true")  //�������������޸�Ȩ��
                //    this.dtDetail = DoCheck.GetStartCheckData(strType, uid.Trim().ToLower(), 1, 6, id, "0");

                //else if (right == "notice") //furion add
                //    this.dtDetail = NoticeCheck.GetStartCheckData(strType, uid.Trim().ToLower(), 1, 6, id);
            }
            else if (sign == "checked") //�Ѵ��������
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

            //�����ڲ����ĺϷ���
            if (dtDetail == null || dtDetail.Rows.Count == 0)
            {
                Response.Write("<font color = red>��ȡ���ݴ�������ϵ����Ա��</font>");
            }
            else
            {
                //��ҳ��Ļ�����Ϣ
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

                ////�ж��Ƿ���ʾ��ִ�����񡱰�ť
                //exedSign = this.dtDetail.Rows[0]["Fstate"].ToString();
                //if (exedSign.Trim() == "<font color = red>�������</font>")   //ת�����жϣ�ֻ��������
                //{
                //    this.btExeTask.Visible = true;

                //    //if ("realtimeadjust" == strType.Trim().ToLower())  //ʵʱ����Ŀǰ�ǿɳ�����
                //    //this.btFail.Visible    = true;
                //}

                //furion 20060407 �¼ӳ������� ֻҪ�Ƿ����˶����Գ���.
                //if (right == "false" && exedSign != "��ִ��" && exedSign != "�ѳ���")
                //{
                //    this.btFail.Visible = true;
                //}
                //end


                objID = this.dtDetail.Rows[0]["FObjId"].ToString().Trim();

                //����������ף�ȷ�ϸ���˿����������ͬ��ʧ��״̬
                //				string sSign = reason.Substring(reason.Length-2,1);   //ȡ�����ͱ�ʾ,�����1����2,����c2cͬ������ 
                //				if ((sSign == "1"|| sSign == "2") && (reason.IndexOf("=>") !=-1)  && (checkType == "fmjy02" || checkType == "fmfk03" || checkType == "fmtk04") && (exedSign.Trim() == "<font color = red>�������</font>"))  //ת�����жϣ�ֻ��������
                //					this.btsynFail.Visible = true;
                //				else
                //					this.btsynFail.Visible = false;
                //if (right == "query")
                //{
                //    //furion 20060630  ����ִ���л��˵�������ɵĹ���
                //    if (this.dtDetail.Rows[0]["FstateName"].ToString().Trim() == "ִ����")
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
//                Response.Write("<font color = red>��½��ʱ�������µ�½��</font>");
//            }
//            else
//            {
//                try
//                {
//                    Check_WebService.Check_Service cw = new Check_WebService.Check_Service();
//                    Check_WebService.Finance_Header fh = new Check_WebService.Finance_Header();
//                    fh.UserName = Session["uid"].ToString();
//                    fh.UserIP = Request.UserHostAddress;
//                    if (ViewState["checkType"] != null && ViewState["checkType"].ToString() != "CNBankTreasurer") //�����ʽ����������ϵͳ����
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
//                catch (SoapException er) //����soap��
//                {
//                    string str = PublicRes.GetErrorMsg(er.Message.ToString());
//                    throw new Exception("����ʧ�ܣ�" + str);
//                }
//                catch (Exception emsg)
//                {
//                    throw new Exception("����ʧ�ܣ�" + emsg.Message.ToString().Replace("'", "��"));
//                }
//            }
//        }

//        private void btPass_Click(object sender, System.EventArgs e)
//        {
//            try
//            {
//                #region rowena   20090902   ϵ���ж�������Ϊ�����ˣ�������ͬʱ������(�ָ������)
//                if (!NotCheckLimitBank())
//                {
//                    return;
//                }
//                #endregion

//                //c2c����ת�˵ģ�����ǰ��У��һ��
//                if (ViewState["checkType"] != null && ViewState["checkType"].ToString() == "batchc2ctransfer")
//                {
//                    string checkMsg = "";
//                    BatchPay_Service.BatchPay_Service bs = new TENCENT.OSS.C2C.Finance.Finance_Web.BatchPay_Service.BatchPay_Service();
//                    bool checkFalg = bs.CheckBatchC2CTransfer(this.hylkObjID.Text.Trim(), out checkMsg);
//                    if (!checkFalg)
//                    {
//                        WebUtils.ShowMessage(this.Page, "��������ʧ�ܣ�ԭ��" + checkMsg);
//                        return;
//                    }

//                }
//                //��Ʊ������ǰ���ϴ��ļ�У����
//                else if (ViewState["checkType"] != null && ViewState["checkType"].ToString() == "returnticket")
//                {
//                    string checkMsg = "";
//                    BatchPay_Service.BatchPay_Service bs = new TENCENT.OSS.C2C.Finance.Finance_Web.BatchPay_Service.BatchPay_Service();
//                    bool checkFalg = bs.CheckReturnticket(this.hylkObjID.Text.Trim(), out checkMsg);
//                    if (!checkFalg)
//                    {
//                        WebUtils.ShowMessage(this.Page, "��������ʧ�ܣ�ԭ��" + checkMsg);
//                        return;
//                    }
//                }
//                //b2c/fastpya������ǰ���ϴ��ļ�У����
//                else if (ViewState["checkType"] != null && ViewState["checkType"].ToString() == "batchrefundapply")
//                {
//                    string checkMsg = "";
//                    BatchPay_Service.BatchPay_Service bs = new TENCENT.OSS.C2C.Finance.Finance_Web.BatchPay_Service.BatchPay_Service();
//                    bool checkFalg = bs.CheckBatchRefund(this.hylkObjID.Text.Trim(), out checkMsg);
//                    if (!checkFalg)
//                    {
//                        WebUtils.ShowMessage(this.Page, "��������ʧ�ܣ�ԭ��" + checkMsg);
//                        return;
//                    }
//                }
//                //�����������ǰУ���ж�
//                else if (ViewState["checkType"] != null && ViewState["checkType"].ToString() == "HongBaoSend")
//                {
//                    string checkMsg = "";
//                    BatchPay_Service.BatchPay_Service bs = new TENCENT.OSS.C2C.Finance.Finance_Web.BatchPay_Service.BatchPay_Service();
//                    bool checkFalg = bs.CheckHONGBAO(this.hylkObjID.Text.Trim(), out checkMsg);
//                    if (!checkFalg)
//                    {
//                        WebUtils.ShowMessage(this.Page, "�����������ʧ�ܣ�ԭ��" + checkMsg);
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
//                        WebUtils.ShowMessage(this.Page, "�ʴ������������ʧ�ܣ�ԭ��" + checkMsg);
//                        return;
//                    }
//                }
//                //�⸶У��
//                else if (ViewState["checkType"] != null && ViewState["checkType"].ToString() == "RefundCompensation")
//                {
//                    string checkMsg = "";
//                    BatchPay_Service.BatchPay_Service bs = new TENCENT.OSS.C2C.Finance.Finance_Web.BatchPay_Service.BatchPay_Service();
//                    bool checkFalg = bs.CheckRefundCompensation(this.hylkObjID.Text.Trim(), out checkMsg);
//                    if (!checkFalg)
//                    {
//                        WebUtils.ShowMessage(this.Page, "��������ʧ�ܣ�ԭ��" + checkMsg);
//                        return;
//                    }
//                }
//                //�»�����ת��
//                else if (ViewState["checkType"] != null && ViewState["checkType"].ToString() == "XinHuaOffPayCheck")
//                {
//                    string checkMsg = "";
//                    BatchPay_Service.BatchPay_Service bs = new TENCENT.OSS.C2C.Finance.Finance_Web.BatchPay_Service.BatchPay_Service();
//                    bool checkFalg = bs.CheckXinHuaOffPay(this.hylkObjID.Text.Trim(), out checkMsg);
//                    if (!checkFalg)
//                    {
//                        WebUtils.ShowMessage(this.Page, "��������ʧ�ܣ�ԭ��" + checkMsg);
//                        return;
//                    }
//                }

//                check(0); //�������  0��ʾͨ������
//                //Response.Write("<script>alert('��ͬ���������������');</script>");
//                if (ViewState["checkType"] != null && ViewState["checkType"].ToString() == "CNBankTreasurer")
//                {
//                    string strUserID = Session["uid"].ToString().Trim().ToLower();
//                    Response.Write("<script language=javascript>window.parent.location='DoCheck.Aspx?paycheck=true&&paycheckuid=" + strUserID + "'</script>");
//                }
//                else
//                {
//                    Response.Write("<script language=javascript>window.parent.location='DoCheck.Aspx'</script>");   //��ת����ϸ���������
//                }
//            }
//            catch (SoapException er) //����soap��
//            {
//                //ִ������ʧ�ܣ�����Ҫ�ٴη������������򣬸ý��׵���һֱ��������״̬���޷������������������
//                //�����ȷ�ϸ�������˿�
//                if (ViewState["checkType"].ToString() == "fmfk03" || ViewState["checkType"].ToString() == "fmtk04")
//                {
//                    this.btsynFail.Visible = true;
//                }
//                else   //�������������
//                {
//                    //furion 20060407 Ϊʲô�����￪ʼ��?
//                    //this.btFail.Visible = true;
//                }

//                string str = PublicRes.GetErrorMsg(er.Message.ToString());
//                ViewState["errMsg"] = str;
//                WebUtils.ShowMessage(this.Page, str);
//            }
//            catch (Exception em)
//            {
//                WebUtils.ShowMessage(this.Page, "��������ʧ�ܣ�ԭ��" + em.Message.ToString());
//            }
//        }
//        //rowena   20090902   ϵ���ж�������Ϊ�����ˣ�������ͬʱ������(�ָ������)
//        private bool NotCheckLimitBank()
//        {
//            if (Session["uid"] == null)
//            {
//                Response.Write("<font color = red>��½��ʱ�������µ�½��</font>");
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
//                    WebUtils.ShowMessage(this.Page, "������Ϊ��");
//                    return false;
//                }
//                string strUserID = Session["uid"].ToString().Trim().ToLower();
//                if (strUserID.Equals(startuser))
//                {
//                    WebUtils.ShowMessage(this.Page, "�����˲���Ϊ�����ˣ�");
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

//                #region rowena   20090902   ϵ���ж�������Ϊ�����ˣ�������ͬʱ������(�ָ������)
//                if (!NotCheckLimitBank())
//                {
//                    return;
//                }
//                #endregion



//                //�˴������� ʵ�����ҡ� ray 20051101
//                if (ViewState["checkType"].ToString() == "settle")  //�����̻����ʺ���ʧ�ܵĶ���������״̬Ϊ1��
//                {
//                    string msg = this.lbCLevel.Text + "�ܾ����̻����ʽ��������![" + this.hylkObjID.Text + "].ԭ��" + this.txSuguest.Text.ToString();
//                    //setSettleSign
//                    Finance_ManageService.Finance_Manage fm = new Finance_ManageService.Finance_Manage();

//                    string Msg = null;
//                    objId = this.hylkObjID.Text.Trim();

//                    if (!fm.setSettleSign(objId, out Msg))
//                    {
//                        throw new Exception(Msg);
//                    }
//                }

//                check(1);//����δͨ��  1 ��ʾ�ܾ�����	

//                string batLstID = null;
//                if (ViewState["batLstID"] != null)
//                    batLstID = ViewState["batLstID"].ToString();

//                string strCheckType = ViewState["checkType"].ToString();

//                if (batLstID.IndexOf("#") != -1 && strCheckType == "fmcz01") //����Ƕ�ʵĳ�ֵ 
//                {
//                    if (batLstID.IndexOf("@") != -1 && strCheckType == "fmcz01")
//                    {
//                        int iBegin = batLstID.IndexOf("@");
//                        int iEnd = batLstID.LastIndexOf("@");
//                        int iSpan = iEnd - iBegin - 1;

//                        string backID = batLstID.Substring(iBegin + 1, iSpan);  //ȡ�����׵��ţ�

//                        string[] ar = backID.Split(',');

//                        string type = ViewState["czType"].ToString();
//                        string sign = null;

//                        if (type == "normal")   //����״̬����״̬
//                        {
//                            sign = "1";  //��ֵ
//                        }
//                        else if (type == "czPay")
//                        {
//                            sign = "3";  //��ֵ֧��
//                        }

//                        int iSign = Int32.Parse(sign);
//                        Finance_ManageService.Finance_Manage fm = new Finance_ManageService.Finance_Manage();
//                        string banktype = ViewState["banktype"].ToString().Trim();
//                        fm.updateBatCheck(ar, iSign, ViewState["accTime"].ToString(), banktype);  //1��ʾ��Ҫ��ֵ����

//                    }
//                    else
//                    {
//                        //û�����з��صĶ����ţ�����ʧ��
//                        Response.Write("<script>alert('����û�з��ص����ж����š�����״̬ʧ�ܣ�');</script>");
//                    }
//                }

//                //�˴���Ҫ��������ʧ������ô�죿 ����ʲôӰ�졣�˴���Ҫ��һ�����ǡ��˴����ڷ���
//                string listID = this.hylkObjID.Text.Trim();     //���׵���
//                string reason = this.txtReason.Text.ToString(); //ԭ��

//                Manage_Service.Manage_Service ms = new Manage_Service.Manage_Service(); //ͬ��ʧ�ܸ�C2C
//                if (ViewState["checkType"].ToString() == "fmfk03")
//                {
//                    string msg = this.lbCLevel.Text + "�ܾ��˽��׵�" + listID + "��ȷ�ϸ������� ԭ��" + this.txSuguest.Text.ToString();
//                    ms.DoConfirmFail(listID, msg, reason);
//                }
//                else if (ViewState["checkType"].ToString() == "fmtk04")
//                {
//                    string msg = this.lbCLevel.Text + "�ܾ��˽��׵�" + listID + "���˿����� ԭ��" + this.txSuguest.Text.ToString();
//                    ms.DoRefoundFail(listID, msg, reason);
//                }

//                if (ViewState["checkType"].ToString() == "otherAdjust")
//                {
//                    string Msg = null;
//                    //ms.updateBankResultSign(id,out Msg);
//                    //ʧ�ܣ����¶����쳣��Ϊδ����
//                    string fstate = "0"; //���ʧ�ܣ�����״̬Ϊδ����
//                    string memo = "�������ܾ���";
//                    Finance_ManageService.Finance_Manage fm = new Finance_ManageService.Finance_Manage();

//                    //���ݵ��ʵ�objID ��ö�Ӧ���쳣���ʱ��kid
//                    string kid = "";
//                    if (!fm.getKid(this.hylkObjID.Text.Trim(), out kid, out Msg))
//                    {
//                        WebUtils.ShowMessage(this.Page, "��ȡ���ʱ�KIDʧ�ܣ�");
//                        return;
//                    }

//                    if (!fm.updateExcepState(kid, fstate, memo, out Msg))
//                    {
//                        WebUtils.ShowMessage(this.Page, "���¹����쳣��ʧ�ܣ�");
//                        return;
//                    }
//                }

//                Response.Write("<script>alert('����ͬ�����������');</script>");
//                Response.Write("<script language=javascript>window.parent.location='DoCheck.Aspx'</script>");   //��ת����ϸ���������
//            }
//            catch (Exception em)
//            {
//                WebUtils.ShowMessage(this.Page, "��������ʧ�ܣ�ԭ��" + em.Message.ToString());
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

//                if (ViewState["checkType"] != null && ViewState["checkType"].ToString() != "CNBankTreasurer") //�����ʽ����������ϵͳ����
//                {

//                    fh.UserPassword = Session["pwd"].ToString();
//                    fh.SzKey = Session["SzKey"].ToString();
//                    fh.OperID = Int32.Parse(Session["OperID"].ToString());
//                    fh.RightString = Session["key"].ToString();
//                }
//                cw.Finance_HeaderValue = fh;

//                cw.ExecuteCheck(ViewState["fid"].ToString());  //����������ID
//                Response.Write("<script>alert('ִ�гɹ���');</script>");
//                if (ViewState["checkType"] != null && ViewState["checkType"].ToString() == "CNBankTreasurer")
//                {
//                    string strUserID = Session["uid"].ToString().Trim().ToLower();
//                    Response.Write("<script language=javascript>window.parent.location='StartCheck.Aspx?paycheck=true&&paycheckuid=" + strUserID + "'</script>");
//                }
//                else
//                {
//                    Response.Write("<script language=javascript>window.parent.location='StartCheck.aspx'</script>");   //��ת����ϸ���������
//                }
//            }
//            catch (SoapException er) //����soap��
//            {
//                //ִ������ʧ�ܣ�����Ҫ�ٴη������������򣬸ý��׵���һֱ��������״̬���޷������������������
//                string str = PublicRes.GetErrorMsg(er.Message.ToString());
//                ViewState["errMsg"] = str;

//                WebUtils.ShowMessage(this.Page, str);

//                //�����ȷ�ϸ�������˿�
//                if (ViewState["checkType"].ToString() == "fmfk03" || ViewState["checkType"].ToString() == "fmtk04")
//                {
//                    this.btsynFail.Visible = true;
//                }
//                else   //�������������
//                {
//                    this.btFail.Visible = true;
//                }
//            }
//            catch (Exception emsg)
//            {
//                //					Response.Write("<script>alert('ִ��ʧ�ܣ�');</script>");
//                WebUtils.ShowMessage(this.Page, PublicRes.GetErrorMsg(emsg.Message.ToString().Replace("'", "��")));
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

//                string listID = this.hylkObjID.Text.Trim();     //���׵���
//                string reason = this.txtReason.Text.ToString(); //ԭ��

//                Manage_Service.Manage_Service ms = new Manage_Service.Manage_Service(); //ͬ��ʧ�ܸ�C2C
//                string msg = "ִ�����������ʱ�����[" + ViewState["errMsg"].ToString().Replace("'", "��") + "]";
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

//                //���¸�����Ϊ����״̬�������û����ٴη������ִ�����񣬵��³��ֲ�һ�µ����
//                //����ִ�в��˵Ķ�Ӧ����һ�������ǳ���������

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
//        /// ������������ϸҳ�棬�������߲ο�,���ݲ�ͬ���������ת����ͬ��ҳ��
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
//            else if (batLstID.IndexOf("#") == -1 && sCheckType == "fmcz01")      //����ǵ��ʵĳ�ֵ
//            {
//                url = "../ACCOUNTMANAGE/AdjustDepositMoney.aspx?id=" + this.hylkObjID.Text.Trim();
//            }
//            else if (batLstID.IndexOf("#") != -1 && sCheckType == "fmcz01") //����Ƕ�ʵĳ�ֵ 
//            {
//                string reason = ViewState["batLstID"].ToString();//setConfig.convertToBase64(this.txtReason.Text.Trim())
//                string[] ar = reason.Split('#');
//                reason = ar[2]; //ȡ�����з��صĶ�����
//                reason = setConfig.convertToBase64(reason);
//                string banktype = ViewState["banktype"].ToString().Trim();
//                url = "../FinanceManage/batPayShow.aspx?reason=" + reason + "&accTime=" + ViewState["accTime"].ToString() + "&banktype=" + banktype;
//            }
//            else if (sCheckType == "batchpay")  //��������
//            {
//                url = "../BatchPay/ShowDetail.Aspx?BatchID=" + this.hylkObjID.Text.Trim() + "&pos=check";
//            }
//            else if (sCheckType == "fmfk03" || sCheckType == "fmtk04")//�������͵���
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

////			else if(sCheckType == "CNBankTreasurer") //20110428 �����ʽ����  rowenawu
//            //			{
//            //				string paywebPath = System.Configuration.ConfigurationSettings.AppSettings["PayWebPath"].ToString();
//            //				url=paywebPath+returnUrl+"&checkid=" + ViewState["id"].ToString()+"&startuser="+this.lbStartUser.Text.Trim()+"&starttime="+this.lbStartTime.Text.Trim()+"&no="+this.hylkObjID.Text.Trim();
//            //			}
//            //����ص������ furion 20050103
//            else if (returnUrl.Trim() == "")
//            {
//                url = "";
//                this.hplkDetail.Enabled = false;
//                this.hylkObjID.Enabled = false;
//            }
//            else  //������Ⱥ��������
//            {
//                //returnUrl�ĸ�ʽ����·����ֻ���ļ���·��
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

//                string listID = this.hylkObjID.Text.Trim();     //���׵���
//                string reason = Common.CommLib.commRes.replaceSqlStr(this.txtReason.Text.ToString()); //ԭ��

//                Manage_Service.Manage_Service ms = new Manage_Service.Manage_Service(); //ͬ��ʧ�ܸ�C2C
//                //string msg    = "ִ�����������ʱ�����[" + ViewState["errMsg"].ToString().Replace("'","��") + "]"; 
//                string msg = "ִ�����������ʱ�����[" + Common.CommLib.commRes.replaceSqlStr(ViewState["errMsg"].ToString()) + "]";

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

//                //���¸�����Ϊ����״̬�������û����ٴη������ִ�����񣬵��³��ֲ�һ�µ����
//                //����ִ�в��˵Ķ�Ӧ����һ�������ǳ���������

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

//        private void btFail_Click(object sender, System.EventArgs e) //������,update��������Ϊfinish״̬
//        {
//            string msg = null; //��������Ϣ
//            string strCheckID = ViewState["fid"].ToString();
//            Check_WebService.Check_Service cw = new Check_WebService.Check_Service();
//            bool sign = cw.RecallCheck(strCheckID, out msg);

//            if (sign == true)
//            {
//                WebUtils.ShowMessage(this.Page, msg);
//                this.btExeTask.Visible = false;
//                this.btFail.Visible = false;

//                //Common.CommLib.commRes.sendLog4Log("MakeFail", Session["uid"] + "������������!����ID:" + strCheckID + ",�������ͣ�" + ViewState["strType"].ToString());
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
//                WebUtils.ShowMessage(this.Page, "����״̬���ʱ����" + PublicRes.GetErrorMsg(err.Message));
//            }
//            finally
//            {
//                da.Dispose();
//                BindData();
//            }
//        }
    }
}
