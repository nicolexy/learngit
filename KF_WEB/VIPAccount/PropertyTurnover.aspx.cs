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
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using System.Web.Mail;

namespace TENCENT.OSS.CFT.KF.KF_Web.VIPAccount
{
    /// <summary>
    /// Summary description for PropertyTurnover.
    /// </summary>
    public partial class PropertyTurnover : System.Web.UI.Page
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                this.tbx_beginDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy��MM��dd��");
                this.tbx_endDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");
            }
            this.btnBeginDate.Attributes.Add("onclick", "openModeBegin()");
            this.btnEndDate.Attributes.Add("onclick", "openModeEnd()");
            totalValue.Text = "0";
        }

        private void btn_query_Click(object sender, System.EventArgs e)
        {
            string strBeginDate = "", strEndDate = "";

            try
            {
                if (this.tbx_beginDate.Text.Trim() != "" && this.tbx_endDate.Text.Trim() != "")
                {
                    strBeginDate = DateTime.Parse(this.tbx_beginDate.Text).ToString("yyyy-MM-dd");
                    strEndDate = DateTime.Parse(this.tbx_endDate.Text).AddDays(1).ToString("yyyy-MM-dd");
                }
            }
            catch
            {
                ShowMsg("���ڸ�ʽ����ȷ��");
                return;
            }

            StartQuery(strBeginDate, strEndDate);
        }

        private void StartQuery(string strBeginDate, string strEndDate)
        {
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                DataSet ds = qs.QueryTurnover(tbx_acc.Text.Trim(), tbx_order.Text.Trim(), strBeginDate, strEndDate);
                int total = 0;
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    this.ShowMsg("��ѯ��¼Ϊ�ա�");
                }
                else
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string svc_id = row["FSvc_id"].ToString();
                        if (FSvc_id.ContainsKey(svc_id))
                        {
                            row["FSvc_id"] = FSvc_id[svc_id];
                        }
                        total += Convert.ToInt32(row["FIncrease"]);
                    }
                }
                totalValue.Text = total.ToString();
                this.DataGrid_QueryResult.DataSource = ds;
                this.DataGrid_QueryResult.DataBind();
            }
            catch (Exception e)
            {
                ShowMsg("��ѯ����" + e.Message);
            }
        }


        private void ShowMsg(string msg)
        {
            Response.Write("<script language=javascript>alert('" + msg + "')</script>");
        }

        private Hashtable FSvc_id
        {
            get
            {
                if (_FSvc_id == null)
                {
                    //					|cert:�״ο�֤ͨ��|oneclick:�״ο�ͨһ��ͨ|firstvip:��ͨSVIP����|authname:�״ο�ͨʵ����֤|activate:����|
                    //						closevip:�ر�SVIP|present:����|normalmember:����VIP|creditcardAdd:���ÿ���������|balancePay:���֧��
                    //						|quickPay:���֧��|lotteryAdd:��Ʊ����|mobileAdd:�ֻ���ֵ����|microPay:΢֧��|partnerConsume:��������������|
                    //						balanceRefund:����˿�|microRefund:΢֧���˿�|quickRefund:���֧���˿�|batch_increase:SVIP�����Ƹ�ֵ|batch_decrease:VIP�ǻ�Ծ�ۼ�|
                    //						aticketAdd:���ջ�Ʊ����|gift:����|xy:�����þ��ֲ���Ա|travelAdd:QQ�����Ƶ�Ԥ������|qtktAdd:QQ������Ʊ����|qgpAdd:QQ�Ź�����|
                    //						qcateAdd:QQ��ʳȯ��������|icbc:�󶨹��а�����|citic:���������ÿ�|"
                    _FSvc_id = new Hashtable();
                    _FSvc_id.Add("cert", "�״ο�֤ͨ��"); _FSvc_id.Add("oneclick", "�״ο�ͨһ��ͨ"); _FSvc_id.Add("firstvip", "��ͨSVIP����");
                    _FSvc_id.Add("authname", "�״ο�ͨʵ����֤"); _FSvc_id.Add("activate", "����"); _FSvc_id.Add("closevip", "�ر�SVIP");
                    _FSvc_id.Add("present", "����"); _FSvc_id.Add("normalmember", "����VIP");
                    _FSvc_id.Add("creditcardAdd", "���ÿ���������"); _FSvc_id.Add("balancePay", "���֧��");
                    _FSvc_id.Add("quickPay", "���֧��"); _FSvc_id.Add("lotteryAdd", "��Ʊ����");
                    _FSvc_id.Add("mobileAdd", "�ֻ���ֵ����"); _FSvc_id.Add("microPay", "΢֧��"); _FSvc_id.Add("partnerConsume", "��������������");
                    _FSvc_id.Add("balanceRefund", "����˿�"); _FSvc_id.Add("microRefund", "΢֧���˿�"); _FSvc_id.Add("quickRefund", "���֧���˿�");
                    _FSvc_id.Add("batch_increase", "SVIP�����Ƹ�ֵ"); _FSvc_id.Add("batch_decrease", "VIP�ǻ�Ծ�ۼ�");
                    _FSvc_id.Add("aticketAdd", "���ջ�Ʊ����"); _FSvc_id.Add("gift", "����"); _FSvc_id.Add("xy", "�����þ��ֲ���Ա");
                    _FSvc_id.Add("travelAdd", "QQ�����Ƶ�Ԥ������"); _FSvc_id.Add("qtktAdd", "QQ������Ʊ����"); _FSvc_id.Add("qgpAdd", "QQ�Ź�����");
                    _FSvc_id.Add("qcateAdd", "QQ��ʳȯ��������"); _FSvc_id.Add("icbc", "�󶨹��а�����"); _FSvc_id.Add("citic", "���������ÿ�");
                }
                return _FSvc_id;
            }
        }

        private Hashtable _FSvc_id = null;

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btn_query.Click += new EventHandler(btn_query_Click);
        }
        #endregion
    }
}
