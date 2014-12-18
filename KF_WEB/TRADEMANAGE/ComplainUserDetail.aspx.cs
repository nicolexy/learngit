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
    /// ComplainUserDetail 的摘要说明。
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
                    ViewState["begindate"] = DateTime.Now.ToString("yyyy年MM月dd日");
                }

                if (Request.QueryString["enddate"] != null && Request.QueryString["enddate"].Trim() != "")
                {
                    qenddate = Request.QueryString["enddate"].Trim();
                    ViewState["enddate"] = qenddate;
                }
                else
                {
                    ViewState["enddate"] = DateTime.Now.ToString("yyyy年MM月dd日");
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
					labTitle.Text = "新增用户投诉";
                    statevisible.Visible = false;
				}
				else
				{
                    if (flag != null && flag != "")
                    {
                        //催办
                        remind_Click(listid);
                    }
                    else {
                        labTitle.Text = "修改用户投诉";
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
			//绑定
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			string msg = "";
            Query_Service.UserComplainClass sc = qs.GetUserComplainDetail(listid, out msg);
			if(sc != null)
			{
				//绑定开始
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
				WebUtils.ShowMessage(this.Page,"读取失败：" + msg);
			}
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
                WebUtils.ShowMessage(this.Page, "请输入商户号码");
                return;
            }
            if (cb.FCftOrderId == "")
            {
                WebUtils.ShowMessage(this.Page, "请输入财付通订单号");
                return;
            }
            if (cb.FContact == "")
            {
                WebUtils.ShowMessage(this.Page, "请输入用户联系方式");
                return;
            }
			
			//修改
            if (listid != "")
			{
				//修改。
                cb.FListId = int.Parse(listid);
                cb.FStatus = int.Parse(ddlCompState.SelectedValue);
                if(qs.ChangeUserComplain(cb,out msg))
				{
					WebUtils.ShowMessage(this.Page,"修改成功");
				}
				else
				{
					WebUtils.ShowMessage(this.Page,msg);
				}
			}
			else
			{
				//新增
                cb.FStatus = 1;
                string ret = qs.AddUserComplain(cb, out msg);
                if (ret != null && ret != "")
				{
					//通过bussid获得email

                    Query_Service.ComplainBussClass sc = qs.GetComplainBussDetail(cb.FBussId, out msg);

                    //发送邮件
                    if (SendEmail(sc.FBussEmail, ret, "用户投诉通知"))
                    {
                        WebUtils.ShowMessage(this.Page, "新增成功");
                    }
                    else
                    {
                        WebUtils.ShowMessage(this.Page, "新增成功：邮件发送失败");
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
                        comptype_str = "买家要求补发货";
                    }
                    else if (sc.FCompType == 2)
                    {
                        comptype_str = "买家申请退款";
                    }
                    else if (sc.FCompType == 3)
                    {
                        comptype_str = "买家对商品质量不满意";
                    }
                    else if (sc.FCompType == 4)
                    {
                        comptype_str = "交易纠纷";
                    }

                    if (sc.FReplyType == 1)
                    {
                        reply_str = "电话回复";
                    }
                    else if (sc.FReplyType == 2)
                    {
                        reply_str = "手机短信回复";
                    }
                    else if (sc.FReplyType == 3)
                    {
                        reply_str = "QQ回复";
                    }
                    else if (sc.FReplyType == 4)
                    {
                        reply_str = "邮箱回复";
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
                //催办
                cb.FListId = int.Parse(listid);
                if (qs.RemindUserComplain(cb, out msg))
                {
                    Query_Service.ComplainBussClass sc = qs.GetComplainBussDetail(bussid, out msg);

                    //发送邮件
                    if (SendEmail(sc.FBussEmail, listid, "用户投诉催办"))
                     {
                         string url = "ComplainUserInput.aspx?qbussid=" + ViewState["qbussid"] + "&begindate=" + ViewState["begindate"] + "&enddate=" + ViewState["enddate"] + "&orderid=" + ViewState["orderid"] + "&comptype=" + ViewState["comptype"] + "&status=" + ViewState["status"] + "&qpage=" + ViewState["qpage"];
                         WebUtils.ShowMessageAndRedirect(this.Page, "催办成功", url);
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