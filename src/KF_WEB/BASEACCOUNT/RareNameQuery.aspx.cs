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

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// SysBulletinManage 的摘要说明。
    /// </summary>
    public partial class RareNameQuery : System.Web.UI.Page
    {
        private string add = "1";
        private string change = "3";
        private string delete = "2";

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                //int operid = Int32.Parse(Session["OperID"].ToString());

                //if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if (!IsPostBack)
            {
               
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
            this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
        }
        #endregion

        private void BindData()
        {
            try
            {
                string outmsg = "";
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                DataSet ds = qs.RareNameQuery(this.txtCardNo.Text.Trim());

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null)
                {
                    WebUtils.ShowMessage(this.Page, "没有读取到数据：" + PublicRes.GetErrorMsg(outmsg));
                    return;
                }

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }

        }
        private void operation(string op_type)
        {
            try
            {
                Query_Service.Query_Service qs = new Query_Service.Query_Service();
                Query_Service.T_RareName_INFO rareName = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.T_RareName_INFO();
                rareName.op_type = op_type;
                rareName.card_no = this.card_no.Text.Trim();
                rareName.account_name = this.account_name.Text.Trim();
                rareName.user_type = this.ddlUserType.SelectedValue;
                rareName.modify_type = this.ddlModifyType.SelectedValue;
                rareName.card_state = this.ddlCardState.SelectedValue;
                rareName.modify_username = Session["uid"].ToString();
                qs.AddOneRareName(rareName);
                this.tableDetail.Visible = false;
                BindData();
                WebUtils.ShowMessage(this.Page, "操作成功！");
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "操作失败：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "操作失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }

        }

        private void Show(DataGridItem item)
        {
            try
            {
                this.tableDetail.Visible = true;
                this.Button4.Text = "编辑";
                this.card_no.Text = item.Cells[4].Text.Trim();
                this.account_name.Text = item.Cells[5].Text.Trim();
                this.ddlUserType.SelectedValue = item.Cells[0].Text.Trim();
                this.ddlModifyType.SelectedValue = item.Cells[2].Text.Trim();
                this.ddlCardState.SelectedValue = item.Cells[1].Text.Trim();
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "操作失败：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "操作失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        private void Delete(DataGridItem item)
        {
            try
            {
                Query_Service.Query_Service qs = new Query_Service.Query_Service();
                Query_Service.T_RareName_INFO rareName = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.T_RareName_INFO();
                rareName.op_type = delete;
                rareName.card_no = item.Cells[4].Text.Trim();
                rareName.account_name = item.Cells[5].Text.Trim();
                rareName.user_type = item.Cells[0].Text.Trim();
                rareName.modify_type = item.Cells[2].Text.Trim();
                rareName.card_state = item.Cells[1].Text.Trim();
                rareName.modify_username = Session["uid"].ToString();
                qs.AddOneRareName(rareName);
                BindData();
                WebUtils.ShowMessage(this.Page, "删除成功！");
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "删除失败：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "删除失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

       
        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string fid = e.Item.Cells[0].Text.Trim();

            switch (e.CommandName)
            {
                case "CHANGE": //修改
                    Session["op_type"] = change;
                    Show(e.Item);
                    break;
                case "DEL": //删除此条记录
                    Session["op_type"] = delete;
                    Delete(e.Item);
                    break;
            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            BindData();
        }

        protected void btnAdd_Click(object sender, System.EventArgs e)
        {
            this.tableDetail.Visible = true;
            this.Button4.Text = "新增";
            this.card_no.Text = "";
            this.account_name.Text = "";
            Session["op_type"] = add;
        }

        protected void operation_Click(object sender, System.EventArgs e)
        {
            this.tableDetail.Visible = true;
           // 操作类型(1,新增，3修改)
            operation(Session["op_type"].ToString());
        }

    }
}
