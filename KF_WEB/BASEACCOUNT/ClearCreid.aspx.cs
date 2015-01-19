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
using System.Xml;
using CFT.CSOMS.BLL.CFTAccountModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// ClearCreid ��ժҪ˵����
    /// </summary>
    public partial class ClearCreid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label4.Text = Session["uid"].ToString();
                Label7.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {

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

        }
        #endregion

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {

        }

        private void ValidateDate()
        {
            string s_creid = creid.Text.ToString().Trim();

            if (string.IsNullOrEmpty(s_creid))
            {
                throw new Exception("������֤�����룡");
            }
        }

        public void btnClear_Click(object sender, System.EventArgs e)
        {
            try
            {
                ValidateDate();
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }

            try
            {
                BindData();
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
            }
        }

        private void BindData()
        {
            string s_creid = creid.Text.ToString();
            int type = rbpt.Checked ? 0 : 1;

            //�ж��������������2����ʾ
            //20150116 �ſ����ƣ��������޴�����
            //var result = (new AccountService()).GetClearCreidCount(s_creid, type);
            //if (result >= 2)
            //{
            //    WebUtils.ShowMessage(this.Page, "����ʧ�ܣ���������ѳ���2�Σ�");
            //    return;
            //}

            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            int ret = qs.ClearCreid(type, s_creid);
            if (ret == 1)
            {
                WebUtils.ShowMessage(this.Page, "����ɹ���");
                //��¼������־
                (new AccountService()).WriteClearCreidLog(s_creid, type, Session["uid"].ToString());
            }
            else
            {
                WebUtils.ShowMessage(this.Page, "����ʧ�ܣ�" + s_creid + "�����ڣ�");
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {

            string s_creid = txtCardId.Text.ToString().Trim();
            try
            {
                if (string.IsNullOrEmpty(s_creid))
                {
                    throw new Exception("������֤�����룡");
                }
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }

            try
            {
                QueryData(s_creid);
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
            }

        }

        private void QueryData(string creid)
        {
            var dt = (new AccountService()).GetClearCreidLog(creid);
            this.DataGrid1.DataSource = dt;
            this.DataGrid1.DataBind();
        }
    }
}