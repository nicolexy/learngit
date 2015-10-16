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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// FreezeDetail ��ժҪ˵����
	/// </summary>
	public partial class FreezeDetail : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			try
			{
				labUid.Text = Session["uid"].ToString();
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{			

				string tdeid = Request.QueryString["fid"];


				if(tdeid == null || tdeid.Trim() == "")
				{
					WebUtils.ShowMessage(this.Page,"��������");
				}

				try
				{
					string strszkey = Session["SzKey"].ToString().Trim();
					int ioperid = Int32.Parse(Session["OperID"].ToString());
                    int iserviceid = Common.AllUserRight.GetServiceID("InfoCenter");
					string struserdata = Session["uid"].ToString().Trim();
					string content = struserdata + "ִ����[�鿴��������]����,��������[" + tdeid
						+ "]ʱ��:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

					Common.AllUserRight.UpdateSession(strszkey,ioperid,PublicRes.GROUPID,iserviceid,struserdata,content);


					string log = SensitivePowerOperaLib.MakeLog("get",struserdata,"[�鿴��������]",tdeid);

                    if (!SensitivePowerOperaLib.WriteOperationRecord("InfoCenter", log, this))
					{
						
					}

					ViewState["UserName"] = Session["uid"].ToString().Trim();
					BindInfo(tdeid);
				}
				catch(LogicException err)
				{
					WebUtils.ShowMessage(this.Page,err.Message);
				}
				catch(SoapException eSoap) //����soap���쳣
				{
					string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
					WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
				}
				catch(Exception eSys)
				{
					WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
				}
			}
		}


		private void BindInfo(string tdeid)
		{
			this.btnSave.Visible = false;

			Query_Service.Query_Service qs = new Query_Service.Query_Service();
			DataSet ds =  qs.GetFreezeListDetail(tdeid);

			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0 )
			{
				DataRow dr = ds.Tables[0].Rows[0];

				lblfid.Text = tdeid;
				labFUserName.Text = PublicRes.GetString(dr["FUserName"]);
				labFContact.Text = PublicRes.GetString(dr["FContact"]);
				string tmp = PublicRes.GetString(dr["FFreezeType"]);
				if(tmp == "1")
				{
					labFFreezeTypeName.Text = "�����ʻ�";
				}
				else if(tmp == "2")
				{
					labFFreezeTypeName.Text = "�������׵�";
				}

				labFFreezeID.Text = PublicRes.GetString(dr["FFreezeID"]);
				labFFreezeUserID.Text = PublicRes.GetString(dr["FFreezeUserID"]);
				labFFreezeTime.Text = PublicRes.GetDateTime(dr["FFreezeTime"]);
				txtFFreezeReason.Text = PublicRes.GetString(dr["FFreezeReason"]);
				labFHandleUserID.Text = PublicRes.GetString(dr["FHandleUserID"]);
				labFHandleTime.Text = PublicRes.GetDateTime(dr["FHandleTime"]);
				labFHandleResult.Text = PublicRes.GetString(dr["FHandleResult"]);
                labFfreezeChannel.Text = GetFreezeChannelStr(PublicRes.GetString(dr["Ffreeze_channel"]));
				this.btnSave.Visible = true;
				
			}
			else
			{
				throw new LogicException("û���ҵ���¼��");
			}
		}

        private string GetFreezeChannelStr(string type) 
        {
            switch (type) 
            {
                case "1":
                    return "��ض���";
                case "2":
                    return "���Ķ���";
                case "3":
                    return "�û�����";
                case "4":
                    return "�̻�����";
                case "5":
                    return "BG�ӿڶ���";
                case "6":
                    return "���ӿ��ɽ��׶���";
                default:
                    return "�޶�������";
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

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				string UserName = ViewState["UserName"].ToString();
				string UserIP   = Request.UserHostAddress;
				string FreezeReason = classLibrary.setConfig.replaceMStr(txtFFreezeReason.Text.Trim());

				Query_Service.Query_Service qs = new Query_Service.Query_Service();
				qs.UpdateFreezeListDetail(lblfid.Text.Trim(),FreezeReason,UserName,UserIP);
				WebUtils.ShowMessage(this.Page,"����ɹ���");
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,"����ʧ�ܣ�" + ex.Message);
			}
		}
	}
}
