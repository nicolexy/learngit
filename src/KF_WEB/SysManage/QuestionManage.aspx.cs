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
    /// SysBulletinManage 的摘要说明。
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
                btnIssue.Attributes["onClick"] = "return confirm('确定要执行“发布”操作吗？');";

                if (Request.QueryString["QuestionManage"] == "1")
                {

                }
            }

            BindData();
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
                    lb.Attributes["onClick"] = "return confirm('确定要执行“移到历史”操作吗？');";
            }

            obj = e.Item.Cells[10].FindControl("lbDel");
            if (obj != null)
            {
                LinkButton lb = (LinkButton)obj;
                if (lb.Visible)
                    lb.Attributes["onClick"] = "return confirm('确定要执行“删除”操作吗？');";
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
                WebUtils.ShowMessage(this.Page, "没有读取到数据：" + PublicRes.GetErrorMsg(outmsg));
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
                case "PRIOR": //向上移
                    GoPrior(fid);
                    break;
                case "NEXT": //向下移
                    GoNext(fid);
                    break;
                case "CHANGE": //修改
                    Response.Redirect("SysBulletinManage_Detail.aspx?sysid=" + sysid + "&fid=" + fid);
                    break;
                case "GOHISTORY": //转移到历史，只对sysid=1有效
                    GoHistory(fid);
                    break;
                case "DEL": //删除此条记录
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
                WebUtils.ShowMessage(this.Page, "删除失败：" + PublicRes.GetErrorMsg(msg));
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
                WebUtils.ShowMessage(this.Page, "调整失败：" + PublicRes.GetErrorMsg(msg));
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
                WebUtils.ShowMessage(this.Page, "调整失败：" + PublicRes.GetErrorMsg(msg));
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
                WebUtils.ShowMessage(this.Page, "调整失败：" + PublicRes.GetErrorMsg(msg));
            }
        }

        protected void btnIssue_Click(object sender, System.EventArgs e)
        {
            //发布
            string sysid = ddlSysList.SelectedValue;

            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            string msg = "";
            if (qs.SysBulletinIssue(sysid, out msg))
            {
                WebUtils.ShowMessage(this.Page, "发布成功");
            }
            else
            {
                WebUtils.ShowMessage(this.Page, "发布失败：" + PublicRes.GetErrorMsg(msg));
            }
        }

        protected void btnNew_Click(object sender, System.EventArgs e)
        {
            //新增
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
