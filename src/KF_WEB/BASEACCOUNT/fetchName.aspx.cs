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

using TENCENT.OSS.C2C.Finance.Common;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// fetchName ��ժҪ˵����
	/// </summary>
	public partial class fetchName : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{

        string oldName, newName, QQID, mail, reason, accPath, infoPath, newIdCardPath, oldIdCardPath, fetchNo, commTime;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			//��֤Ȩ��

			string szkey = Session["SzKey"].ToString();
			if(!classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

			BindData();
		}

		private void BindData()
		{
			if (Request.QueryString["checkUsr"] != null && Request.QueryString["FcheckTime"] != null && Request.QueryString["FcheckMemo"] != null)
			{
				this.lbCheckUid.Text = Request.QueryString["checkUsr"].ToString();
				this.lbCheckInfo.Text= Request.QueryString["FcheckMemo"].ToString();
				this.lbCheckDate.Text= Request.QueryString["FcheckTime"].ToString();
			}

			if (Request.QueryString["print"] != null)
			{
				this.hyLKPrint.Visible = false;
				this.lkPrint.Visible   = true;
			}

			oldName = Request.QueryString["oldName"].ToString().Trim();
			newName = Request.QueryString["newName"].ToString().Trim();
			QQID    = Request.QueryString["QQID"].ToString().Trim();
			mail    = Request.QueryString["mail"].ToString().Trim();
			reason  = Request.QueryString["reason"].ToString().Trim();
			
			accPath = Request.QueryString["accPath"].ToString().Trim();
			infoPath= Request.QueryString["infoPath"].ToString().Trim();

			fetchNo = Request.QueryString["fetchNo"].ToString().Trim();
			commTime= Request.QueryString["commTime"].ToString().Trim();
			
			this.lbQQ.Text      = QQID;
			this.lbOldName.Text = oldName;
			this.lbNewName.Text = newName;
			this.lbMail.Text    = mail;
			this.lbReason.Text  = reason;
			this.lbFetchNo.Text = fetchNo;

            string requestUrl = System.Configuration.ConfigurationManager.AppSettings["GetImageFromKf2Url"].ToString();

            this.imageUInfo.ImageUrl = requestUrl + "/" + infoPath;//"basicInfo.jpg";
            this.imgAccInfo.ImageUrl = requestUrl + "/" + accPath;//"accInfo.jpg";
           
			this.lbHandleID.Text   = Session["uid"].ToString();
			this.lbCommitTime.Text = commTime;
			this.lbTime.Text       = commTime;

			this.hyLKPrint.NavigateUrl = Request.RawUrl.ToString()+ "&print=true";
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
			string Msg = null;
			try
			{
				//�ύ����
				Check_WebService.Check_Service  cs = new Check_WebService.Check_Service();

				TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Finance_Header fh = classLibrary.setConfig.setFH_CheckService(this);

				cs.Finance_HeaderValue = fh;

				Check_WebService.Param [] myParams = new Check_WebService.Param[2]; 
				myParams[0]  = new Check_WebService.Param();  //�������� ��Ҫ�ֱ�ʵ����
				myParams[1]  = new Check_WebService.Param();
			
				//�ύ������URL��Ϣ�����쿴��ϸ������
				string reUrl = "fetchName.aspx?QQID=" + QQID + "&mail=" + mail + "&reason=" + reason
					+ "&fetchNo=" + fetchNo + "&oldName=" + oldName + "&newName=" + newName + "&commTime=" + commTime  
					+ "&accPath=" + accPath + "&infoPath=" + infoPath + "&newIdCardPath=" + newIdCardPath + "&oldIdCardPath=" + oldIdCardPath;
		
				myParams[0].ParamName  = "qqid";          //QQ��
				myParams[0].ParamValue = QQID;
				myParams[1].ParamName  = "returnUrl";
				myParams[1].ParamValue = reUrl;

				cs.StartCheck(fetchNo,"Mediation",reason,"0",myParams);
				
				WebUtils.ShowMessage(this.Page,"�����ύ�ɹ���");

				btCommit.Visible = false;

				string uid      = Session["uid"].ToString();			

				//				//ȷ���ύ�����Ļ�������ص����ݲ������
				Finance_ManageService.Finance_Manage fm = new Finance_ManageService.Finance_Manage();
				bool exeSign = fm.insertMediation(QQID,mail,"",reason,uid,fetchNo,commTime,accPath,infoPath,oldIdCardPath,"",commTime,newName,oldIdCardPath,out Msg);
			
				if (exeSign == false)
				{
					WebUtils.ShowMessage(this.Page,"�����ٲ����ݱ�ʧ�ܣ�" + Msg);	
					return;
				}
				return;

			}
			catch(SoapException eSoap)
			{
				string str = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				string errMsg = "�ύ���������ϸ��Ϣ��" + str;
				WebUtils.ShowMessage(this.Page,errMsg);
			}
			catch(Exception estr)
			{
				WebUtils.ShowMessage(this.Page,"�����ύʧ�ܣ�[" + estr.Message.ToString().Replace("'","��") +"]");
				return;
			}
		}

		protected void lkPrint_Click(object sender, System.EventArgs e)
		{
			this.imageUInfo.Width  = 630;
			this.imageUInfo.Height = 170;

			this.imgAccInfo.Width  = 630;
			this.imgAccInfo.Height = 170;

			this.lkPrint.Visible = false;
			this.lkView.Visible  = true;

			Response.Write("<script>javascript:print();</script>");	
		}
	}
}
