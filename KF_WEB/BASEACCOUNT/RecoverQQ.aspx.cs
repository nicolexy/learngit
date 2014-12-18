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
using System.Configuration;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.CFT.KF.KF_Web.Check_WebService;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// RecoverQQ ��ժҪ˵����
	/// </summary>
	public class RecoverQQ : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.TextBox TX_QQID;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvqq;
		protected System.Web.UI.WebControls.TextBox TX_QQID_Confirm;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvqq_confirm;
		protected System.Web.UI.WebControls.CompareValidator CompareV_QQ;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvMail;
		protected System.Web.UI.WebControls.TextBox txReason;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvReason;
		protected System.Web.UI.WebControls.TextBox txQQUid;
		protected System.Web.UI.WebControls.TextBox TX_PSW;
		protected System.Web.UI.WebControls.Button Button_Update;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

			try
			{
				string sr = Session["SzKey"].ToString();
                this.Label1.Text = Session["uid"].ToString();
                //if (!AllUserRight.GetOneRightState("right_220",sr))
                //{
                //    Response.Redirect("../login.aspx?wh=1");
                //    return;
                //}
                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
				
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
				return;
			}
            if (!IsPostBack)
            {

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
			this.Button_Update.Click += new System.EventHandler(this.Button_Update_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Button_Update_Click(object sender, System.EventArgs e)
		{
		
		
			string qqid   = classLibrary.setConfig.replaceSqlStr(this.TX_QQID.Text);
			string reason = classLibrary.setConfig.replaceSqlStr(this.txReason.Text); 
			string uid= classLibrary.setConfig.replaceSqlStr(this.txQQUid.Text.Trim());
			string memo   = "[�ָ���QQ����:" + qqid + " �ָ������ڲ�ID:"+uid+"]�ָ�ԭ��:" + reason;

			string Msg = "";
			string emailPsw=classLibrary.setConfig.replaceSqlStr(this.TX_PSW.Text).Trim();
			
			try
			{
				//����û��Ƿ�ע��
				Finance_ManageService.Finance_Manage fm = new Finance_ManageService.Finance_Manage();
				if (fm.checkUserReg(qqid,out Msg))
				{
					Msg = qqid+"�ʺ��Ѿ�ע�ᣬ����ע�����ٻָ���";
					WebUtils.ShowMessage(this.Page,Msg);
					return;
				}

				//���Uid�Ƿ���� ��userinfo�У�����t_relation��
				string type="";
				if (!fm.CheckRecoverUid(uid,qqid,out Msg,out type))
				{
					WebUtils.ShowMessage(this.Page,Msg);
					return;
				}

				if(type=="qq"&&emailPsw!="")
				{
					
					WebUtils.ShowMessage(this.Page,"QQ�ʺŻָ�����Ҫ���õ�½���룡");
					return;
				}
				if((type=="mobile"||type=="email")&&emailPsw=="")
				{
					WebUtils.ShowMessage(this.Page,"�ָ�Email���ֻ��ʺ���Ҫ����һ����½���룡");
					return;
				}

				Check_WebService.Check_Service  cs = new Check_WebService.Check_Service();
				Check_WebService.Finance_Header fh = new Check_WebService.Finance_Header();
			
				fh.UserName    = Session["uid"].ToString();
				fh.UserIP      = Request.UserHostAddress;
				fh.OperID      = Int32.Parse(Session["OperID"].ToString());
				fh.SzKey       = Session["SzKey"].ToString();
				//fh.RightString = Session["key"].ToString();

				cs.Finance_HeaderValue = fh;

                Param[] myParams = new Param[5];

                myParams[0] = new Param();
                myParams[0].ParamName = "fqqid";
                myParams[0].ParamValue = this.TX_QQID_Confirm.Text.Trim();

                myParams[1] = new Param();
                myParams[1].ParamName = "fqquid";
                myParams[1].ParamValue = this.txQQUid.Text.Trim();

                myParams[2] = new Param();
                myParams[2].ParamName = "memo";
                myParams[2].ParamValue = memo;

                myParams[3] = new Param();
                myParams[3].ParamName = "returnUrl";
                myParams[3].ParamValue = "/BaseAccount/RecoverQQ.aspx?type=query";

                myParams[4] = new Param();
                myParams[4].ParamName = "psw";
                if (emailPsw.Trim() == "")
                {
                    myParams[4].ParamValue = "";
                }
                else
                {
                    myParams[4].ParamValue = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(emailPsw, "md5").ToLower();
                }

                string mainID = DateTime.Now.ToString("yyyyMMdd") + qqid;
                string checkType = "RecoverQQ";

                SunLibrary.LoggerFactory.Get("RecoverQQ").Info(" check memo:" + memo);

                cs.StartCheck(mainID, checkType, memo, "0", myParams);
			}
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"�ָ���������ʧ�ܣ�" + errStr);
				return;
			}
			catch(Exception err)
			{
				Msg += "�ָ����������쳣��" + commRes.replaceHtmlStr(err.Message);
				WebUtils.ShowMessage(this.Page,Msg);
				return;
			}

			WebUtils.ShowMessage(this.Page,"�ָ���������ɹ���");
			this.Button_Update.Enabled = false;
		}

	
	}
}
