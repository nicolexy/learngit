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
using TENCENT.OSS.CFT.KF.Common;
using CFT.CSOMS.BLL.RemitModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.RemitCheck
{
    /// <summary>
    /// RemitQueryNew 的摘要说明。
    /// </summary>
    public partial class RemitQueryNew : System.Web.UI.Page
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
            if (!IsPostBack)
            {
                GetSpid();
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

        protected void btnQuery_Click(object sender, System.EventArgs e)
        {
            labErrMsg.Text = "";
            try
            {
                GetCount();
                BindData(1);
            }
            catch (Exception ex)
            {
                string msg = ex.Message.Replace("\'", "\\\'");
                WebUtils.ShowMessage(this.Page, msg);
            }
        }

        private void ShowMsg(string msg)
        {
            Response.Write("<script language=javascript>alert('" + msg + "')</script>");
        }

        private void GetSpid()
        {
            try
            {
                DataTable dt = new RemitService().GetRemitSpid();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ddlSpid.Items.Add(dr["spid"].ToString());
                    }
                }
                else
                {
                    ShowMsg("spid未配置！");
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowMsg("spid未配置！" + ex.ToString());
                return;
            }
        }

        private void BindData(int index)
        {
            string tranType = this.ddlTrantype.SelectedValue;
            string dataType = this.ddlDataType.SelectedValue;
            string remitType = this.ddlRemitType.SelectedValue;
            string tranState = this.ddlTranState.SelectedValue;
            string spid = ddlSpid.SelectedValue;
            string remitRec = this.tbRemitRec.Text.Trim();
            string listID = tblistID.Text.Trim();

            int max = pager.PageSize;
            int start = max * (index - 1) + 1;
            try
            {             
                DataSet ds = new RemitService().GetRemitDataList("", tranType, dataType, remitType, tranState, spid, remitRec, listID, start, max);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    this.DataGrid1.DataSource = ds;
                    this.DataGrid1.DataBind();
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "没有找到记录");
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "没有找到记录");
            }
        }

        private void GetCount()
        {
            string tranType = this.ddlTrantype.SelectedValue;
            string dataType = this.ddlDataType.SelectedValue;
            string remitType = this.ddlRemitType.SelectedValue;
            string tranState = this.ddlTranState.SelectedValue;
            string spid = ddlSpid.SelectedValue;
            string remitRec = this.tbRemitRec.Text.Trim();
            string listID = tblistID.Text.Trim();

            pager.RecordCount = new RemitService().GetRemitListCount("", tranType, dataType, remitType, tranState, spid, remitRec, listID);
        }

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                pager.CurrentPageIndex = e.NewPageIndex;
                BindData(e.NewPageIndex);
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + classLibrary.setConfig.replaceHtmlStr(eSys.Message));
            }
        }
    }
}
