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
using CFT.CSOMS.BLL.ForeignCardModule;
using TENCENT.OSS.CFT.KF.KF_Web.InternetBank;
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
    /// <summary>
    /// QueryYTTrade ��ժҪ˵����
    /// </summary>
    public partial class BeforeCancelTradeQuery : System.Web.UI.Page
    {
        public DateTime qbegindate, qenddate;
        protected ForeignCardService FCBLLService = new ForeignCardService();
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
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

        private void ValidateDate()
        {
            string uid = txtuid.Text.ToString();
            if (uid == "")
            {
                throw new Exception("�������ѯ�");
            }
        }

        public void btnQuery_Click(object sender, System.EventArgs e)
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
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        private void BindData()
        {
            try
            {
                string uid = txtuid.Text.ToString();

                DataSet ds =new TradeService().BeforeCancelTradeQuery(uid);
                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    this.lbFlistid.Text = "";
                    throw new Exception("���ݿ��޴˼�¼");
                }
                this.lbFlistid.Text = ds.Tables[0].Rows[0]["Flistid"].ToString();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }


    }
}