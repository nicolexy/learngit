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
            }
            else if (sign == "checked") //�Ѵ��������
            {
                if (right == "false")
                    this.dtDetail = cs.GetFinishCheckData(strType, uid.Trim().ToLower(), 1, 6, id);              
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
                objID = this.dtDetail.Rows[0]["FObjId"].ToString().Trim();             
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
    }
}
