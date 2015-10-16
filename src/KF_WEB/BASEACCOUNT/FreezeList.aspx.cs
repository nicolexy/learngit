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
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// FreezeList ��ժҪ˵����
    /// </summary>
    public partial class FreezeList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��
            ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()");
            ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");

            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                //int operid = Int32.Parse(Session["OperID"].ToString());
                //if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "FreezeList")) Response.Redirect("../login.aspx?wh=1");
                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if (!IsPostBack)
            {
                TextBoxBeginDate.Text = DateTime.Now.AddDays(-30).ToString("yyyy��MM��dd��");

                TextBoxEndDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");

                Table2.Visible = false;
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
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);
        }

        #endregion

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                pager.CurrentPageIndex = e.NewPageIndex;
                BindData(e.NewPageIndex);
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�");
            }
        }

        private void ValidateDate()
        {
            DateTime begindate;
            DateTime enddate;

            try
            {
                begindate = DateTime.Parse(TextBoxBeginDate.Text);
                enddate = DateTime.Parse(TextBoxEndDate.Text);
            }
            catch
            {
                throw new Exception("������������");
            }

            if (begindate.CompareTo(enddate) > 0)
            {
                throw new Exception("��ֹ����С����ʼ���ڣ����������룡");
            }

            tbFreezeUser.Text = classLibrary.setConfig.replaceMStr(tbFreezeUser.Text.Trim());
            tbUserName.Text = classLibrary.setConfig.replaceMStr(tbUserName.Text.Trim());
            ViewState["freezeuser"] = tbFreezeUser.Text.Trim();
            ViewState["username"] = tbUserName.Text;
            ViewState["handletype"] = ddlHandleType.SelectedValue;
            ViewState["statetype"] = ddlStateType.SelectedValue;
            ViewState["begindate"] = DateTime.Parse(begindate.ToString("yyyy-MM-dd 00:00:00"));
            ViewState["enddate"] = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59"));
            ViewState["qq"] = tbQQ.Text.Trim();
        }

        protected void Button2_Click(object sender, System.EventArgs e)
        {
            Table2.Visible = false;

            try
            {
                string strszkey = Session["SzKey"].ToString().Trim();
                int ioperid = Int32.Parse(Session["OperID"].ToString());
                int iserviceid = Common.AllUserRight.GetServiceID("InfoCenter");
                string struserdata = Session["uid"].ToString().Trim();
                string content = struserdata + "ִ����[�鿴�����б�]����,��������[" + " "
                    + "]ʱ��:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Common.AllUserRight.UpdateSession(strszkey, ioperid, PublicRes.GROUPID, iserviceid, struserdata, content);
                ValidateDate();
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }

            try
            {
                Table2.Visible = true;
                pager.RecordCount = GetCount();
                BindData(1);
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());

                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�");
            }
        }

        private int GetCount()
        {
            DateTime begindate = (DateTime)ViewState["begindate"];
            DateTime enddate = (DateTime)ViewState["enddate"];
            string freezeuser = ViewState["freezeuser"].ToString();
            string username = ViewState["username"].ToString();
            int handletype = Int32.Parse(ViewState["handletype"].ToString());
            int statetype = Int32.Parse(ViewState["statetype"].ToString());
            string qq = ViewState["qq"].ToString();
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            return qs.GetFreezeListCount(begindate, enddate, freezeuser, username, handletype, statetype, qq);
        }

        private void BindData(int index)
        {
            DateTime begindate = (DateTime)ViewState["begindate"];
            DateTime enddate = (DateTime)ViewState["enddate"];
            string freezeuser = ViewState["freezeuser"].ToString();
            string username = ViewState["username"].ToString();
            int handletype = Int32.Parse(ViewState["handletype"].ToString());
            int statetype = Int32.Parse(ViewState["statetype"].ToString());
            string qq = ViewState["qq"].ToString();
            int max = pager.PageSize;
            int start = max * (index - 1) + 1;

            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
            qs.Finance_HeaderValue = fh;

            string log = classLibrary.SensitivePowerOperaLib.MakeLog("get", fh.UserName, "[�鿴�����б�]", fh.UserIP, fh.UserName, fh.OperID.ToString(),
                fh.SzKey, begindate.ToString(), enddate.ToString(), freezeuser, username, handletype.ToString(), statetype.ToString(), qq,
                start.ToString(), max.ToString());

            if (!classLibrary.SensitivePowerOperaLib.WriteOperationRecord("InfoCenter", log, this))
            {

            }

            DataSet ds = qs.GetFreezeList(begindate, enddate, freezeuser, username, handletype, statetype, qq, start, max);

            if (ds != null && ds.Tables.Count > 0)
            {
                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
            }
            else
            {
                throw new LogicException("û���ҵ���¼��");
            }
        }
    }

}

