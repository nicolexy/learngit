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
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.RefundManage
{
	/// <summary>
	/// changeUserName_2 ��ժҪ˵����
	/// </summary>
    public partial class AddInformation : System.Web.UI.Page
	{
        private const string FILE_EMPTY         = null;        
        private const string FILE_COMMITMENT    = "R_Commitment";
        private const string FILE_IDENTITY      = "R_Identity";
        private const string FILE_BACKWATER     = "R_BankWater";
        private const string FILE_CANCELLATION  = "R_Cancel";

        private const string FILE_FOLDER = "uploadfile";
        private const int MAX_IAMGE_SIZE = 5 * 1024 * 1024;




		protected void Page_Load(object sender, System.EventArgs e)
		{
            this.Label1.Text = Session["uid"].ToString();
            //��ȡһ��ֵ����ֵָʾ��ҳ�Ƿ���Ϊ��Ӧ�ͻ��˻ط������أ��������Ƿ������״μ��غͷ��ʣ�
            //�����Ϊ��Ӧ�ͻ��˻ط������ظ�ҳ����Ϊtrue������Ϊ false��
            //uinID=" + strUinID + "&refundType" + strRefundType + "&bankListId" + strPayListid + "&oldId" + strOldId +"&time" + strCreateTime + 
            if (!IsPostBack)
            {
                string strUinID = Request.QueryString["uinID"].ToString();
                string strRefundType = Request.QueryString["refundType"].ToString();
                string strPayListid = Request.QueryString["bankListId"].ToString();
                string strOldId = Request.QueryString["oldId"].ToString();
                string strCreateTime = Request.QueryString["time"].ToString();
                string[] aryUinID = strUinID.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                if (aryUinID.Length == 1)
                {
                    lbHeadID.Text = "�����˿����Ϣ�Ǽ�";
                   // ViewState["refundId"] = aryOldId[0];
                }
                else
                {
                    lbHeadID.Text = "�����˿�������Ϣ�Ǽ�";
                }
                this.tbUinID.Text = strUinID;
                this.tbRefundType.Text = strRefundType;
                this.tbBankListId.Text = strPayListid;
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
		protected void Button_Update_Click(object sender, System.EventArgs e)
        {
            string strUinID         = tbUinID.Text;
            string strRefundType    = tbRefundType.Text;
            string strBankListId    = tbBankListId.Text;
            string strIdentity      = txtIdentity.Text;
            string strInitBankAccNo = txtInitBankAccNo.Text;
            string strInitBankType = DropOldBankType.SelectedItem.Text;
            string strInitBankID = DropOldBankType.SelectedItem.Value;
            string strMail          = txtMail.Text;
            string strNewBankAccNo = txtNewBankAccNo.Text;
            string strBankUserName =    txtBankUserName.Text;
            string strNewBankType = DropNewBankType.SelectedItem.Text;
            string strNewBankID = DropNewBankType.SelectedItem.Value;
            string strRemark        = txtRemark.Text;
            string strCreate = Request.QueryString["time"].ToString();
           // string strTrueName = Request.QueryString["trueName"].ToString();
            string strUserFlagText = ddlUserFlag.SelectedItem.Text;
            string strUserFlagID = ddlUserFlag.SelectedItem.Value;
            string strCardTypeText = ddlCardType.SelectedItem.Text;
            string strCardTypeID = ddlCardType.SelectedItem.Value;
            string strBankName = tbBankName.Text;

            string strCommitmentFile = null;
            ParseInputFile(this.commitmentFile, "��ŵ��", FILE_COMMITMENT, out strCommitmentFile);
            string strIdentityCardFile = null;
            ParseInputFile(this.identityCardFile, "���֤", FILE_IDENTITY, out strIdentityCardFile);
            string strBankWaterFile = null;
            ParseInputFile(this.bankWaterFile, "������ˮ�ʺ�", FILE_BACKWATER, out strBankWaterFile);
            string strCancellationFile = null;
            ParseInputFile(this.cancellationFile, "����֤��", FILE_CANCELLATION, out strCancellationFile);

            if (strCommitmentFile == FILE_EMPTY)
            {
                 strCommitmentFile = "";
            }
            if (strIdentityCardFile == FILE_EMPTY)
            {
                strIdentityCardFile = "";
            }
            if (strBankWaterFile == FILE_EMPTY)
            {
                strBankWaterFile = "";
            }
            if (strCancellationFile == FILE_EMPTY)
            {
                strCancellationFile = "";
            }
            string strURL = "CreateAppForm.aspx?uinId=" + strUinID +  "&bankListId=" + strBankListId +
            "&Identity=" + strIdentity + "&initBankAccNo=" + strInitBankAccNo + "&initBankType=" + strInitBankType + "&initBankID="+strInitBankID + "&mail=" + strMail 
            + "&newBankAccNo=" + strNewBankAccNo + "&bankUserName=" + strBankUserName + "&newBankType=" + strNewBankType+"&newBankID="+strNewBankID + "&remark=" + strRemark + "&create=" + strCreate
            + "&commitment=" + strCommitmentFile + "&identityCard=" + strIdentityCardFile + "&bankWater=" + strBankWaterFile + "&cancellation=" + strCancellationFile + "&refundId=" + Request.QueryString["oldId"].ToString()
            + "&userFlagID=" + strUserFlagID+"&userFlagText="+strUserFlagText+"&cardTypeText="+strCardTypeText + "&cardTypeID=" + strCardTypeID +"&bankName="+strBankName;

            strURL = strURL.Replace("\r", "").Replace("\n", "");
            Response.Redirect(strURL);
            
        }

        private void ParseInputFile(HtmlInputFile inputFile,string strTips,string strFormat ,out string  strFilePath)
        {
            //�ϴ���Ҫ��ͼƬ�������ض�Ӧ�������ϵĵ�ַ
            //����ļ�
            strFilePath = FILE_EMPTY;
            string strFilePaths = inputFile.Value;
            if (string.IsNullOrEmpty(strFilePaths))
            {
                return;
            }
            string szTypeName = strFilePaths.Substring(strFilePaths.Length - 4, 4);
            

            try
            {
                if (szTypeName.ToLower() != ".jpg" && szTypeName.ToLower() != ".gif" && szTypeName.ToLower() != ".bmp")
                {
                    WebUtils.ShowMessage(this.Page, strTips+"�ϴ����ļ�����ȷ������Ϊjpg,gif,bmp");
                    return;
                }
                if (inputFile.PostedFile.ContentLength > MAX_IAMGE_SIZE)
                {
                    WebUtils.ShowMessage(this, strTips+"�ϴ���֤��ͼƬ��С���ܴ���5M");
                    return;
                }
                if (!string.IsNullOrEmpty(strFilePaths) )
                {
                    //�ļ���
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.StaticNoManage() + szTypeName;
                    //�����ļ���
                    string paths = FILE_FOLDER + "\\" + System.DateTime.Now.ToString("yyyyMMdd") +"\\" + "CSOMS\\" + strFormat;                  
                    string targetPath = System.Configuration.ConfigurationManager.AppSettings["KFWebSrc"].ToString()  + paths;
                    PublicRes.CreateDirectory(targetPath);
                    //���ش��ļ�
                    string filePaths = targetPath + "\\" + fileName;
                    inputFile.PostedFile.SaveAs(filePaths);
                    //�洢�ļ�·��
                    strFilePath = paths.Replace("\\", "/") + "/" + fileName;
                    log4net.ILog log = log4net.LogManager.GetLogger("�����˿���������");
                    log.InfoFormat("���ɵ�ͼƬ����·�����汾���ļ���Ϣ��{0} �ύ�洢��Ϣ��{1}", filePaths, strFilePath);    
         
                }
            }
            catch (Exception eStr)
            {
                string errMsg = strTips+"�ϴ��ļ�ʧ�ܣ�" + eStr.Message.ToString().Replace("'", "��");
                WebUtils.ShowMessage(this.Page, "" + errMsg);
                return;
            }
 
        }
		private bool CheckOldName(string qqid,string oldName)
		{
			
            return true;
			
		}

        protected void DdlNewBankTypeSelectChanged(object sender, EventArgs e)
        {
            string strDdlText = DropNewBankType.SelectedItem.Text;
            string strText = strDdlText.Substring(strDdlText.Length - 3).Trim();
            if (strText == "���ÿ�")
            {
                ddlCardType.SelectedIndex = 1;
            }
            else
            {

                ddlCardType.SelectedIndex = 0;
            }
        }
	}
	
}
