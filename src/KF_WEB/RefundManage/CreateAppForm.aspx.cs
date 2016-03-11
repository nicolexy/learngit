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
	/// fetchName 的摘要说明。
	/// </summary>
	public partial class CreateAppForm : System.Web.UI.Page
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			//验证权限
            lbOperator.Text = Session["uid"].ToString();
            if (!IsPostBack)
            {
                BindData();
            }			
		}

		private void BindData()
		{
           ViewState["refundId"] = Request.QueryString["refundId"].ToString();
           tbUinID.Text     = Request.QueryString["uinId"].ToString();
          // lbNewBankType.Text = Request.QueryString["refundType"].ToString();
           tbBankListID.Text = Request.QueryString["bankListId"].ToString();
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
           tbCreateTime.Text = Request.QueryString["create"].ToString();
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

           log4net.ILog log = log4net.LogManager.GetLogger("特殊退款表单生成");
           log.InfoFormat("显示本地路径图片：igCommitment：{0} igIdentity：{1} igBankWater：{2} igAccount：{3}", igCommitment.ImageUrl, igIdentity.ImageUrl, igBankWater.ImageUrl, igAccount.ImageUrl);    
            
        }

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
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
                //生成审批编号
                ZWCheck_Service.Check_Service chkService = new ZWCheck_Service.Check_Service();
                chkService.Finance_HeaderValue = RefundPublicFun.SetWebServiceHeader(this);
                string[] aryOldID = ViewState["refundId"].ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < aryOldID.Length; ++i)
                {
                    string strHisCheckID = "";
                    if (string.IsNullOrEmpty(new RefundService().QueryAbnormalRefundCheckID(aryOldID[i], ref strHisCheckID)))
                    {
                        /*objectID组合规则 =  ViewState["refundId"] + strHisCheckID 
                        目的：确保每次申请唯一*/
                        string strObjectID = aryOldID[i];
                        if (!string.IsNullOrEmpty(strHisCheckID))
                        {
                            string[] arrHisCheckId = strHisCheckID.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int y = 0; y < arrHisCheckId.Length; ++y)
                            {
                                strObjectID += arrHisCheckId[y];
                            }
                        }

                        string strCheckId = chkService.StartCheckNew(strObjectID, RefundPublicFun.KFCHECKTYPE, RefundPublicFun.KFCHECKMEMO);
                        if (!string.IsNullOrEmpty(strCheckId))
                        {
                            new RefundService().SetAbnormalRefundListID(strCheckId, aryOldID[i]);
                        }
                    }
                }
                //存入数据
                string strMsg = "";
                string strImgCommitment = Request.QueryString["commitment"].ToString();
                string strImgIdentity = Request.QueryString["identityCard"].ToString();
                string strImgBankWater = Request.QueryString["bankWater"].ToString();
                string strImgCancellation = Request.QueryString["cancellation"].ToString();
                string strOperator = Session["uid"].ToString() + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (new RefundService().UpdateRefundData(aryOldID, lbIdentity.Text, lbInitBankAccNo.Text, lbMail.Text, lbNewBankAccNo.Text,
                    lbBankUserName.Text, lbReason.Text, strImgCommitment, strImgIdentity, strImgBankWater, strImgCancellation, lbBankName.Text, strOperator, int.Parse(ViewState["initBankID"].ToString()),
                    int.Parse(ViewState["newBankID"].ToString()), int.Parse(ViewState["userFlagID"].ToString()), int.Parse(ViewState["cardTypeID"].ToString()), 3, out strMsg) == false)
                {
                    WebUtils.ShowMessage(this.Page, "补填资料失败" + strMsg);
                }
                btCommit.Enabled = false;
                WebUtils.ShowMessage(this.Page, "补填资料提交成功！");
            }
            catch(Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("特殊退款表单生成");
                log.ErrorFormat("特殊退款表单生成：出错：{0} ", ex.Message);                 
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
