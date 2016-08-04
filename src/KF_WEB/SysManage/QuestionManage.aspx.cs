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

namespace TENCENT.OSS.CFT.KF.KF_Web.SysManage
{
    /// <summary>
    /// SysBulletinManage ��ժҪ˵����
    /// </summary>
    public partial class QuestionManage : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["sysid"] != null && Request.QueryString["sysid"].Trim() != "")
                {
                    ddlSysList.SelectedValue = Request.QueryString["sysid"].Trim();
                }
                btnIssue.Attributes["onClick"] = "return confirm('ȷ��Ҫִ�С�������������');";

                if (Request.QueryString["QuestionManage"] == "1")
                {

                }
            }

            BindData();
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
            this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);

        }
        #endregion

        private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            object obj = e.Item.Cells[1].FindControl("labOrder");
            if (obj != null)
            {
                Label lab = (Label)obj;
                lab.Text = (e.Item.ItemIndex + 1).ToString();
            }

            obj = e.Item.Cells[9].FindControl("lbGoHistory");
            if (obj != null)
            {
                LinkButton lb = (LinkButton)obj;
                if (lb.Visible)
                    lb.Attributes["onClick"] = "return confirm('ȷ��Ҫִ�С��Ƶ���ʷ��������');";
            }

            obj = e.Item.Cells[10].FindControl("lbDel");
            if (obj != null)
            {
                LinkButton lb = (LinkButton)obj;
                if (lb.Visible)
                    lb.Attributes["onClick"] = "return confirm('ȷ��Ҫִ�С�ɾ����������');";
            }
        }

        private void BindData()
        {
            string listtype = ddlSysList.SelectedValue;
            string outmsg = "";
            this.Table1.Visible = true;


            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            DataSet ds = qs.GetSysBulletin(listtype, out outmsg);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null)
            {
                WebUtils.ShowMessage(this.Page, "û�ж�ȡ�����ݣ�" + PublicRes.GetErrorMsg(outmsg));
                return;
            }
  
            DataGrid1.DataSource = ds.Tables[0].DefaultView;
            DataGrid1.DataBind();

        }

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string fid = e.Item.Cells[0].Text.Trim();
            string sysid = ddlSysList.SelectedValue;

            switch (e.CommandName)
            {
                case "PRIOR": //������
                    GoPrior(fid);
                    break;
                case "NEXT": //������
                    GoNext(fid);
                    break;
                case "CHANGE": //�޸�
                    Response.Redirect("SysBulletinManage_Detail.aspx?sysid=" + sysid + "&fid=" + fid);
                    break;
                case "GOHISTORY": //ת�Ƶ���ʷ��ֻ��sysid=1��Ч
                    GoHistory(fid);
                    break;
                case "DEL": //ɾ��������¼
                    Del(fid);
                    break;
            }
        }

        private void Del(string fid)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            string msg = "";
            if (qs.SysBulletinDel(fid, Request.UserHostAddress, out msg))
            {
                BindData();
            }
            else
            {
                WebUtils.ShowMessage(this.Page, "ɾ��ʧ�ܣ�" + PublicRes.GetErrorMsg(msg));
            }
        }

        private void GoHistory(string fid)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            string msg = "";
            if (qs.SysBulletinGoHistory(fid, Request.UserHostAddress, out msg))
            {
                BindData();
            }
            else
            {
                WebUtils.ShowMessage(this.Page, "����ʧ�ܣ�" + PublicRes.GetErrorMsg(msg));
            }
        }

        private void GoPrior(string fid)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            string msg = "";
            if (qs.SysBulletinGoPrior(fid, Request.UserHostAddress, out msg))
            {
                BindData();
            }
            else
            {
                WebUtils.ShowMessage(this.Page, "����ʧ�ܣ�" + PublicRes.GetErrorMsg(msg));
            }
        }

        private void GoNext(string fid)
        {
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            string msg = "";
            if (qs.SysBulletinGoNext(fid, Request.UserHostAddress, out msg))
            {
                BindData();
            }
            else
            {
                WebUtils.ShowMessage(this.Page, "����ʧ�ܣ�" + PublicRes.GetErrorMsg(msg));
            }
        }

        protected void btnIssue_Click(object sender, System.EventArgs e)
        {
            //����
            string sysid = ddlSysList.SelectedValue;

            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            string msg = "";
            if (qs.SysBulletinIssue(sysid, out msg))
            {
                WebUtils.ShowMessage(this.Page, "�����ɹ�");
            }
            else
            {
                WebUtils.ShowMessage(this.Page, "����ʧ�ܣ�" + PublicRes.GetErrorMsg(msg));
            }
        }

        protected void btnNew_Click(object sender, System.EventArgs e)
        {
            //����
            string sysid = ddlSysList.SelectedValue;
            Response.Redirect("SysBulletinManage_Detail.aspx?sysid=" + sysid);
        }

        protected void btadd_Click(object sender, System.EventArgs e)
        {
            string sysid = ddlSysList.SelectedValue;
            Response.Redirect("SysBulletinManage_Detail.aspx?sysid=" + sysid + "&opertype=1");
        }

        protected void btnQuery_Click(object sender, System.EventArgs e)
        {
            BindData();
        }

    }
}
