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
using System.Web.Mail;
using System.IO;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
    /// ComplainUserDetail ��ժҪ˵����
	/// </summary>
    public partial class ComplainUserDetail : System.Web.UI.Page
	{
        private string listid, qbussid, qbegindate, qenddate, qorderid, qcomptype, qstatus, qpage;
        private string flag, bussid;
        
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				//Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}
			if(!IsPostBack)
			{
                if (Request.QueryString["bussid"] != null && Request.QueryString["bussid"].Trim() != "")
                {
                    bussid = Request.QueryString["bussid"];
                    ViewState["bussid"] = bussid;
                }
                else {
                    ViewState["bussid"] = "";
                }
                if (Request.QueryString["qbussid"] != null && Request.QueryString["qbussid"].Trim() != "")
                {
                    qbussid = Request.QueryString["qbussid"];
                    ViewState["qbussid"] = qbussid;
                }
                else
                {
                    ViewState["qbussid"] = "";
                }
                if (Request.QueryString["begindate"] != null && Request.QueryString["begindate"].Trim() != "")
                {
                    qbegindate = Request.QueryString["begindate"].Trim();
                    ViewState["begindate"] = qbegindate;
                }
                else {
                    ViewState["begindate"] = DateTime.Now.ToString("yyyy��MM��dd��");
                }

                if (Request.QueryString["enddate"] != null && Request.QueryString["enddate"].Trim() != "")
                {
                    qenddate = Request.QueryString["enddate"].Trim();
                    ViewState["enddate"] = qenddate;
                }
                else
                {
                    ViewState["enddate"] = DateTime.Now.ToString("yyyy��MM��dd��");
                }

                if (Request.QueryString["orderid"] != null && Request.QueryString["orderid"].Trim() != "")
                {
                    qorderid = Request.QueryString["orderid"];
                    ViewState["orderid"] = qorderid;
                }
                else
                {
                    ViewState["orderid"] = "";
                }

                if (Request.QueryString["comptype"] != null && Request.QueryString["comptype"].Trim() != "")
                {
                    qcomptype = Request.QueryString["comptype"];
                    ViewState["comptype"] = qcomptype;
                }
                else
                {
                    ViewState["comptype"] = "0";
                }

                if (Request.QueryString["status"] != null && Request.QueryString["status"].Trim() != "")
                {
                    qstatus = Request.QueryString["status"];
                    ViewState["status"] = qstatus;
                }
                else
                {
                    ViewState["status"] = "0";
                }

                if (Request.QueryString["qpage"] != null && Request.QueryString["qpage"].Trim() != "")
                {
                    qpage = Request.QueryString["qpage"];
                    ViewState["qpage"] = qpage;
                }
                else
                {
                    ViewState["qpage"] = "1";
                }

                if (Request.QueryString["listid"] != null && Request.QueryString["listid"].Trim() != "")
				{
                    listid = Request.QueryString["listid"].Trim();
                    ViewState["listid"] = listid;
				}
				else
				{
                    listid = "";
                    ViewState["listid"] = listid;
				}
                if (Request.QueryString["flag"] != null && Request.QueryString["flag"].Trim() != "")
                {
                    flag = Request.QueryString["flag"].Trim();
                }
                else
                {
                    flag = "";
                }

                if (listid == "")
				{
					labTitle.Text = "�����û�Ͷ��";
                    statevisible.Visible = false;
				}
				else
				{
                    if (flag != null && flag != "")
                    {
                        //�߰�
                        remind_Click(listid);
                    }
                    else {
                        labTitle.Text = "�޸��û�Ͷ��";
                        statevisible.Visible = true;
                        BindData(listid);
                    }
                    
				}
			}
			else
			{
                listid = ViewState["listid"].ToString();
			}
		}

        private void BindData(string listid)
		{
			//��
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			string msg = "";
            Query_Service.UserComplainClass sc = qs.GetUserComplainDetail(listid, out msg);
			if(sc != null)
			{
				//�󶨿�ʼ
                bussNumber.Text = sc.FBussId.ToString();
                cftOrderId.Text = sc.FCftOrderId;
                ddlComplainType.SelectedValue = sc.FCompType.ToString();
                ddlCompState.SelectedValue = sc.FStatus.ToString();
                ddlReplyType.SelectedValue = sc.FReplyType.ToString();
                userContact.Text = sc.FContact;
                bussOrderId.Text = sc.FBussOrderId;
                memo.Text = sc.FMemo;

                //bussNumber.ReadOnly = true;
			}
			else
			{
				WebUtils.ShowMessage(this.Page,"��ȡʧ�ܣ�" + msg);
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
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			string msg = "";
            Query_Service.UserComplainClass cb = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.UserComplainClass();
            cb.FBussId = bussNumber.Text.Trim();
            cb.FCftOrderId = cftOrderId.Text.Trim();
            cb.FCompType = int.Parse(ddlComplainType.SelectedValue);
            cb.FReplyType = int.Parse(ddlReplyType.SelectedValue);
            cb.FContact = classLibrary.setConfig.replaceMStr(userContact.Text.Trim());
            cb.FBussOrderId = bussOrderId.Text.Trim();
            cb.FMemo = memo.Text.Trim();

            if (cb.FBussId == "")
            {
                WebUtils.ShowMessage(this.Page, "�������̻�����");
                return;
            }
            if (cb.FCftOrderId == "")
            {
                WebUtils.ShowMessage(this.Page, "������Ƹ�ͨ������");
                return;
            }
            if (cb.FContact == "")
            {
                WebUtils.ShowMessage(this.Page, "�������û���ϵ��ʽ");
                return;
            }
			
			//�޸�
            if (listid != "")
			{
				//�޸ġ�
                cb.FListId = int.Parse(listid);
                cb.FStatus = int.Parse(ddlCompState.SelectedValue);
                if(qs.ChangeUserComplain(cb,out msg))
				{
					WebUtils.ShowMessage(this.Page,"�޸ĳɹ�");
				}
				else
				{
					WebUtils.ShowMessage(this.Page,msg);
				}
			}
			else
			{
				//����
                cb.FStatus = 1;
                string ret = qs.AddUserComplain(cb, out msg);
                if (ret != null && ret != "")
				{
					//ͨ��bussid���email

                    Query_Service.ComplainBussClass sc = qs.GetComplainBussDetail(cb.FBussId, out msg);

                    //�����ʼ�
                    if (SendEmail(sc.FBussEmail, ret, "�û�Ͷ��֪ͨ"))
                    {
                        WebUtils.ShowMessage(this.Page, "�����ɹ�");
                    }
                    else
                    {
                        WebUtils.ShowMessage(this.Page, "�����ɹ����ʼ�����ʧ��");
                    }
				}
				else
				{
					WebUtils.ShowMessage(this.Page, msg);
				}
			}
		}

        private bool SendEmail(string email, string m_listid, string subject) {
            try 
            {
                string msg = "";
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                Query_Service.UserComplainClass sc = qs.GetUserComplainDetail(m_listid, out msg);
                if (sc != null)
                {
                    string comptype_str = "";
                    string reply_str = "";
                    if (sc.FCompType == 1)
                    {
                        comptype_str = "���Ҫ�󲹷���";
                    }
                    else if (sc.FCompType == 2)
                    {
                        comptype_str = "��������˿�";
                    }
                    else if (sc.FCompType == 3)
                    {
                        comptype_str = "��Ҷ���Ʒ����������";
                    }
                    else if (sc.FCompType == 4)
                    {
                        comptype_str = "���׾���";
                    }

                    if (sc.FReplyType == 1)
                    {
                        reply_str = "�绰�ظ�";
                    }
                    else if (sc.FReplyType == 2)
                    {
                        reply_str = "�ֻ����Żظ�";
                    }
                    else if (sc.FReplyType == 3)
                    {
                        reply_str = "QQ�ظ�";
                    }
                    else if (sc.FReplyType == 4)
                    {
                        reply_str = "����ظ�";
                    }
                    string s_order_fee = classLibrary.setConfig.FenToYuan(sc.FOrderFee);

                    string s_params = "p_subject=" + subject + "&p_name=" + sc.FBussName + "&p_parm1=" + sc.FBussName + "&p_parm2=" + sc.FBussId + "&p_parm3=" + sc.FCftOrderId + "&p_parm4=" + s_order_fee
                        + "&p_parm5=" + comptype_str + "&p_parm6=" + sc.FContact + "&p_parm7=" + reply_str + "&p_parm8=" + sc.FNoticeTime + "&p_parm9=" + sc.FRemindTime + "&p_parm10=" + sc.FBussOrderId + "&p_parm11=" + sc.FMemo;
                    TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(email, "2080", s_params);
                }

                
                return true;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }	
        }

        private void remind_Click(string listid)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            string msg = "";
            Query_Service.UserComplainClass cb = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.UserComplainClass();

            if (listid != "")
            {
                //�߰�
                cb.FListId = int.Parse(listid);
                if (qs.RemindUserComplain(cb, out msg))
                {
                    Query_Service.ComplainBussClass sc = qs.GetComplainBussDetail(bussid, out msg);

                    //�����ʼ�
                    if (SendEmail(sc.FBussEmail, listid, "�û�Ͷ�ߴ߰�"))
                     {
                         string url = "ComplainUserInput.aspx?qbussid=" + ViewState["qbussid"] + "&begindate=" + ViewState["begindate"] + "&enddate=" + ViewState["enddate"] + "&orderid=" + ViewState["orderid"] + "&comptype=" + ViewState["comptype"] + "&status=" + ViewState["status"] + "&qpage=" + ViewState["qpage"];
                         WebUtils.ShowMessageAndRedirect(this.Page, "�߰�ɹ�", url);
                     }
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, msg);
                }
            }
        }

        protected void btnBack_Click(object sender, System.EventArgs e)
		{
            Response.Redirect("ComplainUserInput.aspx?qbussid=" + ViewState["qbussid"] + "&begindate=" + ViewState["begindate"] + "&enddate=" + ViewState["enddate"] + "&orderid=" + ViewState["orderid"] + "&comptype=" + ViewState["comptype"] + "&status=" + ViewState["status"] + "&qpage=" + ViewState["qpage"]);
		}
	}
}