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
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// CFTAppealQuery 的摘要说明。
    /// </summary>
    public partial class CFTAppealQuery : System.Web.UI.Page
    {
        public static string rooturl
        {
            get
            {
                string url = System.Configuration.ConfigurationManager.AppSettings["AppealUrlPath"].Trim();
                if (!url.EndsWith("/"))
                    url += "/";
                return url;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                //int operid = Int32.Parse(Session["OperID"].ToString());
                //if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"CFTUserPickTJ")) Response.Redirect("../login.aspx?wh=1");

                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if (!IsPostBack)
            {
                TextBoxBeginDate.Value = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                TextBoxEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                Table2.Visible = false;

                if (Request.QueryString["user"] != null)
                {
                    tbFuin.Text = Request.QueryString["user"].Trim();
                    if (tbFuin.Text.StartsWith("合计"))
                        tbFuin.Text = "";
                }

                if (Request.QueryString["state"] != null)
                {
                    ddlState.SelectedValue = Request.QueryString["state"].Trim();
                }

                if (Request.QueryString["begin"] != null)
                {
                    TextBoxBeginDate.Value = DateTime.Parse(Request.QueryString["begin"].Trim()).ToString("yyyy-MM-dd");
                }

                if (Request.QueryString["end"] != null)
                {
                    TextBoxEndDate.Value = DateTime.Parse(Request.QueryString["end"].Trim()).ToString("yyyy-MM-dd");
                    Button2_Click(null, null);
                }

                this.rbtnFuin.Checked = true;
            }

            if (rbtnFuin.Checked)
            {
                txtQQ.Enabled = false;
                TextBoxBeginDate.Disabled = false;
                //ButtonBeginDate.Enabled = true;
                TextBoxEndDate.Disabled = false;
                //ButtonEndDate.Enabled = true;
                tbFuin.Enabled = true;
                ddlState.Enabled = true;
            }
            else
            {
                txtQQ.Enabled = true;
                TextBoxBeginDate.Disabled = true;
                //ButtonBeginDate.Enabled = false;
                TextBoxEndDate.Disabled = true;
                //ButtonEndDate.Enabled = false;
                tbFuin.Enabled = false;
                ddlState.Enabled = false;
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
            this.DataGrid1.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid1_ItemDataBound);
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);
        }

        #endregion

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }

        private void ValidateDate()
        {
            ViewState["rbtnFuin"] = rbtnFuin.Checked;

            if (this.rbtnFuin.Checked)
            {
                DateTime begindate;
                DateTime enddate;

                try
                {
                    begindate = DateTime.Parse(TextBoxBeginDate.Value);
                    enddate = DateTime.Parse(TextBoxEndDate.Value);
                }
                catch
                {
                    throw new Exception("日期输入有误！");
                }

                if (begindate.CompareTo(enddate) > 0)
                {
                    throw new Exception("终止日期小于起始日期，请重新输入！");
                }

                ViewState["fstate"] = ddlState.SelectedValue;
                string stmp = tbFuin.Text.Trim();
                ViewState["fuin"] = classLibrary.setConfig.replaceMStr(stmp);
                ViewState["begindate"] = DateTime.Parse(begindate.ToString("yyyy-MM-dd 00:00:00"));
                ViewState["enddate"] = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59"));
            }
            else
            {
                ViewState["fQQ"] = txtQQ.Text.Trim();
            }

        }

        protected void Button2_Click(object sender, System.EventArgs e)
        {

            try
            {
                ValidateDate();
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, err.Message + ", stacktrace" + err.StackTrace);
                return;
            }

            try
            {
                Table2.Visible = true;
                pager.RecordCount = GetCount();
                BindData(1);

            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr + ", stacktrace" + eSoap.StackTrace);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString() + ", stacktrace" + eSys.StackTrace);
            }
        }

        private int GetCount()
        {
            return 10000;
        }

        private void BindData(int index)
        {

        }

        private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            string stmp = e.Item.Cells[0].Text.Trim();
            int strlen = stmp.Length;
            if (strlen > 6)
            {
                stmp = "***" + stmp.Substring(3, strlen - 3);
                e.Item.Cells[0].Text = stmp;
            }

        }

    }

}
