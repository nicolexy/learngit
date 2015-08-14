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
using System.Web.Services.Protocols;

using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;

using TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// FreezeReason ��ժҪ˵����
	/// </summary>
	public partial class FreezeReason : System.Web.UI.Page
	{
	
		private string sign;
		//private string txtSign;
		private string listID;

		protected void Page_Load(object sender, System.EventArgs e)
		{	
			//furion 20050906 ���޸���ǰ���κι��ܣ�ֻ���������µĶ�����
			// �ڴ˴������û������Գ�ʼ��ҳ��
			try
			{
				labUid.Text = Session["uid"].ToString();

				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "LockTradeList")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("LockTradeList",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch  //���û�е�½����û��Ȩ�޾�����
			{
				Response.Redirect("../login.aspx?wh=1");
			} 


			if (!Page.IsPostBack)
			{
				sign   = Request.QueryString["id"].ToString();
				listID = Request.QueryString["lsd"].ToString();

				

				//furion 20050906 �������־ֲ��������������У�������ViewState��
				ViewState["sign"] = sign;
				ViewState["listID"] = listID;
				//furion end.

				BindInfo();
			}			

		}

		private void BindInfo()
		{
			//����Ϣ
			sign = ViewState["sign"].ToString();
			listID = ViewState["listID"].ToString();
			
			if (sign.ToLower() == "true")  //����������ʻ������ж������
			{
				//�������
				Label1_state.Text     = "�������׵�";
				this.BT_F_Or_Not.Text = "�������׵�";

				//furion 20050906
				Label_listID.Text = listID;
				labReason.Text = "����ԭ��";            
				tbUserName.Text = "";
				tbUserName.Enabled = true;
				tbContact.Text = "";
				tbContact.Enabled = true;
                ddlFreezeChannel.Enabled = true;
				//furion end
			}
			else if (sign.ToLower() == "false")   //����Ƕ����˻������нⶳ����
			{
				//�ⶳ����
				Label1_state.Text     = "�������׵�";
				this.BT_F_Or_Not.Text = "�������׵�";

				//furion 20050906
				labReason.Text = "����ԭ��"; 				
				tbUserName.Enabled = false;				
				tbContact.Enabled = false;
                ddlFreezeChannel.Enabled = false;
				Label_listID.Text = listID;

				//��ȡ��ԭ���ύ���û���������ϵ��ʽ���ʻ����롣
				Query_Service.Query_Service fm = new Query_Service.Query_Service();
//				Query_Service.Finance_Header fh = new Query_Service.Finance_Header();
//				fh.UserName = Session["uid"].ToString();
//				fh.UserIP   = Request.UserHostAddress;
//				fh.OperID = Int32.Parse(Session["OperID"].ToString());
//				fh.SzKey = Session["SzKey"].ToString();
//				//fh.RightString = Session["key"].ToString();
//				fm.Finance_HeaderValue = fh;
				Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
				fm.Finance_HeaderValue = fh;
				try
				{
					//FFreezeType: 1Ϊ�����ʻ���2Ϊ��������
					Query_Service.FreezeInfo fi = fm.GetExistFreeze(listID,2);
					ViewState["fid"] = fi.fid;

                    if (fi.FFreezeChannel == null || fi.FFreezeChannel == "" || fi.FFreezeChannel == "0")
                    {
                        ddlFreezeChannel.Items.Add(new ListItem("�޶�������", "0"));
                        ddlFreezeChannel.SelectedValue = "0";
                    }
                    else
                    {
                        ddlFreezeChannel.SelectedValue = fi.FFreezeChannel;
                    }
                    ViewState["freezeChannel"] = fi.FFreezeChannel; //�ⶳ����

					tbUserName.Text = fi.username;
					tbContact.Text = fi.contact;
				}
				catch
				{
					ViewState["fid"] = "";
                    ViewState["freezeChannel"] = ""; //��������
				}
				//furion end
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


		protected void BT_F_Or_Not_Click(object sender, System.EventArgs e)
		{
			try
			{
				Session["uid"].ToString();
			}
			catch
			{
				Response.Write("<script>alert('��½��ʱ�������µ�½��');</script>");
			}

			//����Ϣ
			sign = ViewState["sign"].ToString();
			listID = ViewState["listID"].ToString();

			string strszkey = Session["SzKey"].ToString().Trim();
			int ioperid = Int32.Parse(Session["OperID"].ToString());
			int iserviceid = Common.AllUserRight.GetServiceID("LockTradeList") ;
			string struserdata = Session["uid"].ToString().Trim();
			string content = struserdata + "ִ����[�������׵���ť]����,��������[" + listID
				+ "]ʱ��:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

			Common.AllUserRight.UpdateSession(strszkey,ioperid,PublicRes.GROUPID,iserviceid,struserdata,content);

			string log = classLibrary.SensitivePowerOperaLib.MakeLog("edit",Session["uid"].ToString().Trim(),"[�������׵���ť]",
				this.Label_listID.Text.Trim(),"1");

			if(!classLibrary.SensitivePowerOperaLib.WriteOperationRecord("LockTradeList",log,this))
			{
					
			}

			if (sign == "True")   
			{
				//�������
				//furion 20050906 Ҫ�ȼ��빤�������ɹ�����������Ĺ�����
				Query_Service.Query_Service qs = new Query_Service.Query_Service();
				Query_Service.Finance_Header fhq = classLibrary.setConfig.setFH(this);
//				fhq.UserName = Session["uid"].ToString();
//				fhq.UserIP   = Request.UserHostAddress;
//				fhq.OperID = Int32.Parse(Session["OperID"].ToString());
//				fhq.SzKey = Session["SzKey"].ToString();
//				//fhq.RightString = Session["key"].ToString();
				qs.Finance_HeaderValue = fhq;
				try
				{
					Query_Service.FreezeInfo fi = new FreezeInfo();
					fi.FFreezeID = listID;
					fi.FFreezeType = 2;
					fi.username = tbUserName.Text.Trim();
					fi.contact = tbContact.Text.Trim();
					fi.FFreezeReason = tbMemo.Text.Trim();
                    fi.FFreezeChannel = ddlFreezeChannel.SelectedValue;
					qs.CreateNewFreeze(fi);
				}
				catch
				{
					WebUtils.ShowMessage(this.Page,"�������Ṥ��ʱʧ�ܣ�");
					return;
				}
				//furion end

				TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService.Finance_Manage myService = new TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService.Finance_Manage();
				//myService.Finance_HeaderValue = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.setFH_Finance(Session["uid"].ToString(),Request.UserHostAddress);

				myService.Finance_HeaderValue = classLibrary.setConfig.setFH_Finance(this);

				myService.freezeTrade(this.Label_listID.Text.Trim(),"1");  //����2��ʾ���׵�Ҫ���ĳɵ�״̬ 1 ���� 2 ���� 3 ����

				Response.Write("<script>alert('�������ݳɹ���');</script>");
				this.Button1_back.Visible = true;
				this.BT_F_Or_Not.Visible = false;
			}
			else if (sign == "False")                 
			{
                string isChannel = "";
                if (ViewState["freezeChannel"] != null && ViewState["freezeChannel"].ToString() != "")
                {
                    isChannel = ViewState["freezeChannel"].ToString();
                }

                if (isChannel != "" && isChannel != "0")
                {
                    //���Ϊ��,����Ҫ����Ȩ���ж�;��Ϊ��,����Ҫ����Ȩ���ж�.
                    string val = "";
                    string des = "";
                    if (isChannel == "1" || isChannel == "6")
                    {
                        //�������
                        val = "UnFreezeChannelFK";
                        des = "��ض���";
                    }
                    else if (isChannel == "2")
                    {
                        //����
                        val = "UnFreezeChannelPP";
                        des = "���Ķ���";
                    }
                    else if (isChannel == "3")
                    {
                        //�û�
                        val = "UnFreezeChannelYH";
                        des = "�û�����";
                    }
                    else if (isChannel == "4")
                    {
                        //�̻�
                        val = "UnFreezeChannelSH";
                        des = "�̻�����";
                    }
                    else if (isChannel == "5")
                    {
                        //BG
                        val = "UnFreezeChannelBG";
                        des = "BG�ӿڶ���";
                    }

                    if (val != "" && !classLibrary.ClassLib.ValidateRight(val, this))
                    {
                        //����Ȩ���ж�
                        WebUtils.ShowMessage(this.Page, "û�нⶳ��������Ϊ[" + des + "]��Ȩ�ޣ�");
                        return;
                    }
                }
                
                //furion 20050906 Ҫ�ȼ��빤�������ɹ�����������Ĺ�����
				Query_Service.Query_Service qs = new Query_Service.Query_Service();
				Query_Service.Finance_Header fhq = classLibrary.setConfig.setFH(this);
//				fhq.UserName = Session["uid"].ToString();
//				fhq.UserIP   = Request.UserHostAddress;
//				fhq.OperID = Int32.Parse(Session["OperID"].ToString());
//				fhq.SzKey = Session["SzKey"].ToString();
//				//fhq.RightString = Session["key"].ToString();
				qs.Finance_HeaderValue = fhq;

				try
				{
					Query_Service.FreezeInfo fi = new FreezeInfo();
					fi.fid = ViewState["fid"].ToString();
					fi.FHandleResult = tbMemo.Text.Trim();
					fi.FFreezeType = 2;
					qs.UpdateFreezeInfo(fi);
				}
				catch
				{
					WebUtils.ShowMessage(this.Page,"�����Ṥ��ʱʧ�ܣ�");
					return;
				}
				//furion end

				//�ⶳ����
				TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService.Finance_Manage myService = new KF_Web.Finance_ManageService.Finance_Manage();
				//myService.Finance_HeaderValue = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.setFH_Finance(Session["uid"].ToString(),Request.UserHostAddress);

				myService.Finance_HeaderValue = classLibrary.setConfig.setFH_Finance(this);

				myService.freezeTrade(this.Label_listID.Text.Trim(),"2"); //����2��ʾ���׵�Ҫ���ĳɵ�״̬ 1 ���� 2 ���� 3 ����
				
				Response.Write("<script>alert('��������ɹ���');</script>");
				this.Button1_back.Visible = true;
				this.BT_F_Or_Not.Visible = false;
//				Response.Write("<script>window.opener=null;window.close()</script>");
			}
		}

		protected void Button1_back_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("TradelogQuery.aspx?id=" + this.Label_listID.Text.ToString().Trim());
		}

	
	}
}
