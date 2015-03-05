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
using CFT.CSOMS.BLL.RefundModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.RefundManage
{
	/// <summary>
	/// fetchName ��ժҪ˵����
	/// </summary>
	public partial class CreateAppForm : System.Web.UI.Page
	{
        private const string KFCHECKTYPE = "KFAbnormalRefund";
        private const string KFCHECKMEMO = "�����˿�������������";
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			//��֤Ȩ��
            lbOperator.Text = Session["OperID"].ToString();
            if (!IsPostBack)
            {
                BindData();
            }			
		}

		private void BindData()
		{
           ViewState["refundId"] = Request.QueryString["refundId"].ToString();
           lbUinID.Text     = Request.QueryString["uinId"].ToString();
          // lbNewBankType.Text = Request.QueryString["refundType"].ToString();
           lbBankListID.Text = Request.QueryString["bankListId"].ToString();
           lbIdentity.Text = Request.QueryString["Identity"].ToString();
           lbInitBankAccNo.Text = Request.QueryString["initBankAccNo"].ToString();
           lbInitBankType.Text = Request.QueryString["initBankType"].ToString();
           ViewState["initBankID"] = Request.QueryString["initBankID"].ToString();
           lbMail.Text = Request.QueryString["mail"].ToString();
           lbNewBankAccNo.Text = Request.QueryString["newBankAccNo"].ToString();///newBankAccNo
          // m_nBankName = Request.QueryString["bankUserName"].ToString();-->bankusername
           lbBankUserName.Text = Request.QueryString["bankUserName"].ToString(); 
           lbNewBankType.Text = Request.QueryString["newBankType"].ToString();
           ViewState["newBankID"] = Request.QueryString["newBankID"].ToString();
           lbReason.Text = Request.QueryString["remark"].ToString();
           lbCreateTime.Text = Request.QueryString["create"].ToString();
           lbOperatorDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

           lbUserFlag.Text = Request.QueryString["userFlagText"].ToString();
           ViewState["userFlagID"] = Request.QueryString["userFlagID"].ToString();
           lbCardType.Text = Request.QueryString["cardTypeText"].ToString();
           ViewState["cardTypeID"] = Request.QueryString["cardTypeID"].ToString();
           lbBankName.Text = Request.QueryString["bankName"].ToString();
           //string strPaths = System.Configuration.ConfigurationManager.AppSettings["KFWebSrc"].ToString();
           igCommitment.ImageUrl =  "../"  + Request.QueryString["commitment"].ToString();
           igIdentity.ImageUrl =    "../" + Request.QueryString["identityCard"].ToString();
           igBankWater.ImageUrl =   "../" + Request.QueryString["bankWater"].ToString();
           igAccount.ImageUrl =     "../" + Request.QueryString["cancellation"].ToString();

           log4net.ILog log = log4net.LogManager.GetLogger("�����˿������");
           log.InfoFormat("��ʾ����·��ͼƬ��igCommitment��{0} igIdentity��{1} igBankWater��{2} igAccount��{3}", igCommitment.ImageUrl, igIdentity.ImageUrl, igBankWater.ImageUrl, igAccount.ImageUrl);    
            
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


		protected void btCommit_Click(object sender, System.EventArgs e)
		{
         //(string strUinId, string strBankListId, string strIdentity, string strBankAccNoOld, string strBankTypeOld, string strUserEmail, string strNewBankAccNo, string strNewBankType,
           // string strBankUserName, string strReason, int nState, out string outMsg
            try
            {
                string strHisCheckID = "";
                if (string.IsNullOrEmpty(new RefundService().QueryAbnormalRefundCheckID(ViewState["refundId"].ToString(),ref strHisCheckID)))
                {
                    //�����������
                    ZWCheck_Service.Check_Service chkService = new ZWCheck_Service.Check_Service();
                    chkService.Finance_HeaderValue = RefundPublicFun.SetWebServiceHeader(this);

                    /*objectID��Ϲ��� =  ViewState["refundId"] + strHisCheckID 
                    Ŀ�ģ�ȷ��ÿ������Ψһ*/
                    string strObjectID = ViewState["refundId"].ToString();
                    if (!string.IsNullOrEmpty(strHisCheckID))
                    {
                        string[] arrHisCheckId = strHisCheckID.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        for(int i=0;i<arrHisCheckId.Length;++i)
                        {
                            strObjectID += arrHisCheckId[i];
                        }
                    }

                    string strCheckId = chkService.StartCheckNew(strObjectID, KFCHECKTYPE, KFCHECKMEMO);
                    if (!string.IsNullOrEmpty(strCheckId))
                    {
                        new RefundService().SetAbnormalRefundListID(strCheckId, ViewState["refundId"].ToString());
                    }
                }
                //��������
                string strMsg = "";
                string strImgCommitment = Request.QueryString["commitment"].ToString();
                string strImgIdentity = Request.QueryString["identityCard"].ToString();
                string strImgBankWater = Request.QueryString["bankWater"].ToString();
                string strImgCancellation = Request.QueryString["cancellation"].ToString();
                if (new RefundService().UpdateRefundData(ViewState["refundId"].ToString(), lbIdentity.Text, lbInitBankAccNo.Text, lbMail.Text, lbNewBankAccNo.Text,
                    lbBankUserName.Text, lbReason.Text, strImgCommitment, strImgIdentity, strImgBankWater, strImgCancellation,lbBankName.Text, int.Parse(ViewState["initBankID"].ToString()),
                    int.Parse(ViewState["newBankID"].ToString()), int.Parse(ViewState["userFlagID"].ToString()), int.Parse(ViewState["cardTypeID"].ToString()), 2, out strMsg) == false)
                {
                
                    WebUtils.ShowMessage(this.Page, "��������ʧ��" + strMsg);
                    return;
                }
                btCommit.Enabled = false;
                WebUtils.ShowMessage(this.Page, "���������ύ�ɹ���");
            }
            catch(Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("�����˿������");
                log.ErrorFormat("�����˿�����ɣ�����{0} ", ex.Message);                 
                throw new Exception(ex.Message);
            }


		}

		protected void lkPrint_Click(object sender, System.EventArgs e)
		{
            this.igCommitment.Width = 630;
            this.igCommitment.Height = 170;

            this.igIdentity.Width = 630;
            this.igIdentity.Height = 170;

            this.igBankWater.Width = 630;
            this.igBankWater.Height = 170;

            this.igAccount.Width = 630;
            this.igAccount.Height = 170;

			//this.lkPrint.Visible = false;
			//this.lkView.Visible  = true;

			Response.Write("<script>javascript:print();</script>");	
		}
	}
}
