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
using TENCENT.OSS.C2C.Finance.DataAccess;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web.Check_WebService;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// StartCheck ��ժҪ˵����
    /// </summary>
    public partial class StartCheck : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected System.Web.UI.WebControls.LinkButton lkUnchecked;
        protected System.Web.UI.WebControls.DataGrid dgCheck;
        protected System.Data.DataSet dscheck;
        protected System.Data.DataTable dtcheck;
        protected System.Web.UI.WebControls.Label lbTask;
        protected System.Web.UI.WebControls.LinkButton lkChecked;

        public string iFramePath, iFrameHeight;
        public string signShow, exedSign, exeShow, sign;
        protected System.Web.UI.WebControls.DropDownList dlCheckType;
        protected System.Web.UI.WebControls.DataGrid dgCheckLog;
        protected System.Web.UI.WebControls.Label Label2;
        protected System.Web.UI.WebControls.LinkButton lkShow;
        protected Wuqi.Webdiyer.AspNetPager AspNetPager1;
        protected Wuqi.Webdiyer.AspNetPager pager;
        protected System.Web.UI.WebControls.Label lbInfo;
        protected System.Data.DataTable dtLog;

        private void Page_Load(object sender, System.EventArgs e)
        {
         
            // �ڴ˴������û������Գ�ʼ��ҳ��
            try
            {

                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
                   
            if (!Page.IsPostBack)
            {
                exeShow = "false";  //�Ƿ�ִ��

                //��ʼ����������
                Hashtable ht = getCheckType();
                
                foreach (string s in ht.Keys)
                {
                    dlCheckType.Items.Add(new ListItem(ht[s].ToString(), s));
                }
                this.dlCheckType.SelectedValue = "0";
                dlCheckType.DataBind();
                BindInfo();
            }
        }

        private static Hashtable getCheckType()
        {
            Check_Service cs = new Check_Service();
            DataSet ds = cs.getCheckType();
            Hashtable ht = new Hashtable();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    ht.Add(dr["ftypeid"].ToString(), dr["ftypeName"].ToString());
                }
            }
            return ht;
        }

        public void BindUncheck(int pageIndex)
        {
            //�ж��Ƿ��½
            if (Session["uid"] == null)
            {
                Response.Redirect("../login.aspx?wh=1"); //���µ�½
            }

            int iStr, iMax;
            iMax = pager.PageSize; //ÿҳ��ʾ
            iStr = (pageIndex - 1) * iMax + 1;  //��ʼ����
            pager.RecordCount = Int32.Parse(ViewState["uncheckNum"].ToString());  //��¼����

            //���ݰ󶨴���
            Check_Service cs = new Check_Service();
            this.dtcheck = cs.GetStartCheckData(this.dlCheckType.SelectedValue, Session["uid"].ToString().Trim().ToLower(), iStr, iMax, "");

            if (dtcheck.Rows.Count == 0 || dtcheck == null)
            {
                dgCheck.Visible = false;
                this.lbTask.Visible = true;
                this.lbTask.Text = "��ǰû�д���������";
            }
            else
            {
                dgCheck.Visible = true;
                this.lbTask.Visible = false;
            }
            iFrameHeight = "10";

            ViewState["sign"] = "uncheck";
            dgCheck.DataBind();
        }

        public void BindChecked(int pageIndex)
        {

            //�ж��Ƿ��½
            if (Session["uid"] == null)
            {
                Response.Redirect("../login.aspx?wh=1"); //���µ�½
            }

            int iStr, iMax;

            iMax = pager.PageSize; //ÿҳ��ʾ
            iStr = (pageIndex - 1) * iMax + 1;  //��ʼ����

            pager.RecordCount = Int32.Parse(ViewState["checkedNum"].ToString());  //��¼����

            //���ݰ󶨴���
            Check_Service cs = new Check_Service();
            this.dtcheck = cs.GetFinishCheckData(this.dlCheckType.SelectedValue, Session["uid"].ToString().Trim().ToLower(), iStr, iMax, "");

            if (dtcheck.Rows.Count == 0 || dtcheck == null)
            {
                dgCheck.Visible = false;
                this.lbTask.Visible = true;
                this.lbTask.Text = "��ǰû������������";
            }
            else
            {
                //����ݵ�ʱ�򣬶�ȡ״̬
                exedSign = dtcheck.Rows[0]["Fstate"].ToString().Trim();

                dgCheck.Visible = true;
                this.lbTask.Visible = false;
            }
            iFrameHeight = "10";
            ViewState["sign"] = "checked";
            dgCheck.DataBind();
        }

        public void BindInfo()
        {
            int uncheckNum, checkedNum;
            string selectType = this.dlCheckType.SelectedValue;
            string uid = Session["uid"].ToString();

            Check_Service cs = new Check_Service();
       
            uncheckNum =cs.GetStartCheckCount(selectType, uid);
            checkedNum = cs.GetFinishCheckCount(selectType, uid);
            ViewState["uncheckNum"] = uncheckNum;
            ViewState["checkedNum"] = checkedNum;
            this.lbInfo.Text = "�����ǣ�" + DateTime.Now.ToString("yyyy��MM��dd��") + " ����ǰ��" + this.dlCheckType.SelectedItem.Text + uncheckNum.ToString() + "��������," + checkedNum.ToString() + "��ͨ������," + cs.GetToDoNum(selectType, Session["uid"].ToString().Trim()) + "����ִ�С�";
            iFrameHeight = "0";

            //�������Ϣ
            this.lkShow.Visible = false;
            ViewState["signShow"] = true;

            if (ViewState["sign"] == null)
                ViewState["sign"] = "uncheck";

            if (ViewState["sign"].ToString() == "uncheck")
                BindUncheck(1); //Ĭ�ϰ󶨵�һҳ
            else if (ViewState["sign"].ToString() == "checked")
                BindChecked(1); //Ĭ�ϰ󶨵�һҳ

            if (dtcheck.Rows.Count == 0 || dtcheck == null)
            {
                dgCheck.Visible = false;
                this.lbTask.Visible = true;
                this.lbTask.Text = "��ǰû������";
                iFrameHeight = "10";
            }
        }

        /// <summary>
        /// ��ת��ȡ��ϸ��Ϣ�����ұ���idֵ��������ʹ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void detailCheck(Object sender, CommandEventArgs e)  //oncommand
        {

            //ÿ�ε����ˢ�·���������Ҫ���°�
            if (ViewState["sign"].ToString() == "uncheck")
                BindUncheck(pager.CurrentPageIndex);
            else
                BindChecked(pager.CurrentPageIndex);

            //������/չ��״̬Ϊtrue;
            //			lkShow.Visible= true;
            this.lkShow.Text = "- ����";
            ViewState["signShow"] = true;

            string id = e.CommandArgument.ToString();
            ViewState["id"] = id;
            iFramePath = "detailCheck.aspx?id=" + id + "&type=" + e.CommandName + "&sign=" + ViewState["sign"].ToString() + "&right=false";
            if (ViewState["sign"].ToString() == "uncheck")
            {
                iFrameHeight = "140";  //����Iframe�߶�
            }
            else
            {
                iFrameHeight = "140";
            }

            //����־
            dtLog = GetCheckLog(1, 10);

            sign = ViewState["sign"].ToString();  //����������δ�����ı�ʶ

            dgCheck.DataBind();
            dgCheckLog.DataBind();
        }

        public void detailPage(Object sender, CommandEventArgs e)
        {
            string type = e.CommandName.ToString().Trim();
            string objID = e.CommandArgument.ToString().Trim();

            if (type == "batchpay")
                Response.Redirect("../BatchPay/ShowDetail.Aspx?BatchID=" + objID + "&pos=check ");
            else
                Response.Redirect("../ACCOUNTMANAGE/AdjustDepositMoney.aspx?id=" + objID + "&pos=check");
        }


        public void executeTask(Object sender, CommandEventArgs e)
        {
            if (Session["uid"] == null)
            {
                Response.Redirect("../login.aspx?wh=1"); //���µ�½
            }

            try
            {
                string checkID = e.CommandArgument.ToString().Trim();

                Check_WebService.Check_Service cw = new Check_WebService.Check_Service();
                Check_WebService.Finance_Header fh = new Check_WebService.Finance_Header();
                fh.UserName = Session["uid"].ToString();
                fh.UserPassword = Session["pwd"].ToString();
                fh.UserIP = Request.UserHostAddress;
                cw.Finance_HeaderValue = fh;

                cw.ExecuteCheck(e.CommandArgument.ToString());
                Response.Write("<script>alert('ִ�гɹ���');</script>");
                //				Response.Write("<script language=javascript>window.parent.location='DoCheck.Aspx'</script>");   //��ת����ϸ���������
            }
            catch (Exception emsg)
            {
                //				Response.Write("<script>alert('ִ��ʧ�ܣ�');</script>");
                WebUtils.ShowMessage(this.Page, "ִ��ʧ�ܣ���ϸ��" + emsg.Message.ToString().Replace("'", "��"));
            }
            finally
            {
                //ִ����������ˢ��
                lkShow.Visible = false;
                dtLog = null;
                dgCheckLog.DataBind();
                this.lkChecked.ForeColor = Color.Red;
                this.lkUnchecked.ForeColor = Color.Black;
                BindChecked(1);
                Page.DataBind();
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
            this.dscheck = new System.Data.DataSet();
            this.dtcheck = new System.Data.DataTable();
            this.dtLog = new System.Data.DataTable();
            ((System.ComponentModel.ISupportInitialize)(this.dscheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtcheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtLog)).BeginInit();
            this.lkUnchecked.Click += new System.EventHandler(this.lkUnchecked_Click);
            this.lkChecked.Click += new System.EventHandler(this.lkChecked_Click);
            this.dlCheckType.SelectedIndexChanged += new System.EventHandler(this.dlCheckType_SelectedIndexChanged);
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);
            // 
            // dscheck
            // 
            this.dscheck.DataSetName = "NewDataSet";
            this.dscheck.Locale = new System.Globalization.CultureInfo("zh-CN");
            this.dscheck.Tables.AddRange(new System.Data.DataTable[] {
																		 this.dtcheck,
																		 this.dtLog});
            // 
            // dtcheck
            // 
            this.dtcheck.TableName = "dtcheck";
            // 
            // dtLog
            // 
            this.dtLog.TableName = "dtLog";
            this.Load += new System.EventHandler(this.Page_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dscheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtcheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtLog)).EndInit();

        }
        #endregion

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;

            if (ViewState["sign"].ToString() == "uncheck")
                BindUncheck(pager.CurrentPageIndex);
            else
                BindChecked(pager.CurrentPageIndex);

            sign = ViewState["sign"].ToString();  //����������δ�����ı�ʶ
            dgCheck.DataBind();
        }

        private void lkUnchecked_Click(object sender, System.EventArgs e)
        {
            exeShow = "false"; //�Ƿ���ʾҪִ�еİ�ť ��Ҫ����

            lkShow.Visible = false;

            //�������log������
            dtLog = null;
            dgCheckLog.DataBind();

            if (Session["uid"] == null)
            {
                WebUtils.ShowMessage(this.Page, "��ʱ�� �����µ�½��");
                Response.Redirect("../login.aspx?wh=1");
            }

            this.lkChecked.ForeColor = Color.Black;
            this.lkUnchecked.ForeColor = Color.Red;

            BindUncheck(1); //Ĭ�ϰ󶨵�һҳ 

            sign = ViewState["sign"].ToString();  //����������δ�����ı�ʶ

        }

        private void lkChecked_Click(object sender, System.EventArgs e)
        {
            lkCheckedClick();
        }

        public void lkCheckedClick()
        {
            exeShow = "true"; //�Ƿ���ʾҪִ�еİ�ť ��Ҫ����

            lkShow.Visible = false;

            //�������log������
            dtLog = null;
            dgCheckLog.DataBind();

            this.lkChecked.ForeColor = Color.Red;
            this.lkUnchecked.ForeColor = Color.Black;

            BindChecked(1);

            sign = ViewState["sign"].ToString();  //����������δ�����ı�ʶ
        }

        /// <summary>
        /// ��ȡ������־
        /// </summary>
        /// <returns></returns>
        private DataTable GetCheckLog(int iStartIndex, int iRecordCount)
        {
            DataTable dt = null;
            if (ViewState["id"] != null)
            {
                try
                {
                    Check_Service cs = new Check_Service();
                    dt = cs.GetCheckLog(ViewState["id"].ToString(),iStartIndex, iRecordCount);
                        return dt;
                }
                catch (Exception ex)
                {
                    WebUtils.ShowMessage(this.Page, ex.Message.ToString());
                    return dt;

                }
               
            }
            else
            {
                WebUtils.ShowMessage(this.Page, "��ʱ�� �����µ�½��");
                return dt;
            }

        }    

        private void lkCheckLog_Click(object sender, System.EventArgs e)
        {
            //			dtLog = GetCheckLog(1,6);
        }

        private void LinkButton2_Click(object sender, System.EventArgs e)
        {
            if ((bool)ViewState["signShow"] == true)
            {
                this.lkShow.Text = "+ չ��";
                //����������״̬Ϊfalse
                dtcheck = null;
                dgCheck.DataBind();
                iFramePath = "detailCheck.aspx?id=" + ViewState["id"] + "&type=" + this.dlCheckType.SelectedValue + "&sign=" + ViewState["sign"].ToString() + "&right=false";
                ViewState["signShow"] = false;
            }
            else
            {
                if (ViewState["sign"].ToString() == "uncheck")
                    BindUncheck(pager.CurrentPageIndex);
                else
                    BindChecked(pager.CurrentPageIndex);

                this.lkShow.Text = "- ����";
                //չ��������״̬Ϊtrue
                iFramePath = "detailCheck.aspx?id=" + ViewState["id"] + "&type=" + this.dlCheckType.SelectedValue + "&sign=" + ViewState["sign"].ToString() + "&right=false";
                ViewState["signShow"] = true;
            }
        }


        public string covert(string str)
        {
            if (str == "<font color = red>�������</font>")
            {
                exedSign = "No";
                return "���ִ��";
            }
            else if (str == "��ִ��")
            {
                exedSign = "Yes";
                return "��ִ��";
            }
            else
            {
                return str;
            }
        }

       

        private void dlCheckType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            BindInfo();
        }
    }
}
