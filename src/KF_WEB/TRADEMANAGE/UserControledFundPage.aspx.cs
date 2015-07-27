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
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.CFTAccountModule;
using System.Web.Services.Protocols;
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
    /// <summary>
    /// UserControledFundPage 的摘要说明。
    /// </summary>
    public partial class UserControledFundPage : System.Web.UI.Page
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["OperID"] != null)
                this.lb_operatorID.Text = Session["OperID"].ToString();

            // 权限管理
            if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");

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
            this.DataGrid_QueryResult.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid_ItemCommand);
        }
        #endregion

        private void StartQuery(string qqid)
        {
            lblmemo.Text = "";
            if (string.IsNullOrEmpty(qqid))
            {
                lblmemo.ForeColor = Color.Red;
                lblmemo.Text = "请输入要查询的账号!";
                return;
            }
            try
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

                DataTable dt = new AccountService().QueryUserCtrlFund(qqid, Session["uid"].ToString());

                if (dt != null && dt.Rows.Count > 0)
                {
                    this.DataGrid_QueryResult.DataSource = dt;
                    this.DataGrid_QueryResult.DataBind();
                }
                else
                {
                    this.DataGrid_QueryResult.DataSource = null;
                    this.DataGrid_QueryResult.DataBind();
                }             
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "查询异常！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        private void QueryLog(string qqid)
        {
            try
            {
                DataSet ds = new TradeService().RemoveControledFinLogQuery(qqid);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    this.DataGrid_OperLog.DataSource = ds.Tables[0];
                    this.DataGrid_OperLog.DataBind();
                }
                else
                {
                    this.DataGrid_OperLog.DataSource = null;
                    this.DataGrid_OperLog.DataBind();
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "查询解绑日志异常！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }
    
        protected void btn_query_Click(object sender, System.EventArgs e)
        {
            StartQuery(this.txt_qqid.Text);
            QueryLog(this.txt_qqid.Text);
        }

        protected void btn_removeAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!ClassLib.ValidateRight("UnFinanceControl", this))
                {
                    throw new Exception("无权限！");
                }
                if (new AccountService().UnbindAllCtrlFund(this.txt_qqid.Text, Session["uid"].ToString()))
                {
                    WebUtils.ShowMessage(this.Page, "解除成功！");
                    QueryLog(this.txt_qqid.Text.Trim());
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "解绑所有子账户余额异常：" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        private void ShowMsg(string msg)
        {
            Response.Write("<script language=javascript>alert('" + msg + "')</script>");
        }

        protected void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {     
            object obj = e.Item.Cells[7].FindControl("removeButton");

            if (obj != null)
            {
                decimal balance = Decimal.Parse(e.Item.Cells[9].Text.Trim());//受控金额 单位分
                Button lb = (Button)obj;
                if (balance > 0)
                {
                    lb.Visible = true;
                }
            }        
        }

        private void DataGrid_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {      
            string cur_type = e.Item.Cells[5].Text.Trim();//类型
            string balance = e.Item.Cells[9].Text.Trim();//金额 单位分
        
            try
            {
                if (e.CommandName == "remove")
                {
                    if (!ClassLib.ValidateRight("UnFinanceControl", this))
                    {
                        throw new Exception("无权限！");
                    }
                    if (new AccountService().UnbindSingleCtrlFund(this.txt_qqid.Text, Session["uid"].ToString(), cur_type, balance))
                    {
                        WebUtils.ShowMessage(this.Page, "解除成功！");
                        QueryLog(this.txt_qqid.Text.Trim());
                    }
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "解除用户受控资金异常：" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

    }
}
