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
using CFT.CSOMS.BLL.WechatPay;

namespace TENCENT.OSS.CFT.KF.KF_Web.FastPay
{
    /// <summary>
    /// QueryYTTrade ��ժҪ˵����
    /// </summary>
    public partial class FastPayLimitQuery : System.Web.UI.Page
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

            string cardNo = txtCardNo.Text.ToString();
            string bankType = txtBankType.Text.ToString();
            if (cardNo == "" || bankType == "")
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

            BindData(25);//�����޶�
            BindData(22);//ҵ���޶�
        }

        private void BindData(int req)
        {
            try
            {
                string cardNo = txtCardNo.Text.ToString();
                string bankType = txtBankType.Text.ToString();

                DataSet ds = new FastPayService().FastPayLimitQuery(cardNo, bankType,
                    int.Parse(this.ddlcard_type.SelectedValue), int.Parse(this.ddlpay_type.SelectedValue), req);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (req == 25)
                    {
                        DataGrid1.DataSource = ds;
                        DataGrid1.DataBind();
                    }
                    else
                    {
                        DataGrid2.DataSource = ds;
                        DataGrid2.DataBind();
                    }
                }
                else
                {
                    if (req == 25)
                    {
                        DataGrid1.DataSource = null;
                        DataGrid1.DataBind();
                    }
                    else
                    {
                        DataGrid2.DataSource = null;
                        DataGrid2.DataBind();
                    }
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
                return;
            }
        }


    }
}