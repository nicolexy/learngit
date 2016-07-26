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
using System.Xml;
using CFT.CSOMS.BLL.AutoRecharge;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
    /// <summary>
    /// QueryDiscountCode 的摘要说明。
    /// </summary>
    public partial class AutomaticRechargeQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {

            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    this.pager.RecordCount = 1000;
                    this.pager2.RecordCount = 1000;
                }

            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
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
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);
            this.pager2.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage2);

        }
        #endregion

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }
        public void ChangePage2(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager2.CurrentPageIndex = e.NewPageIndex;
            GetDetailList((int)ViewState["rid"], e.NewPageIndex);
        }

        private void ValidateDate()
        {
            string cftno = cftNo.Text.ToString();

            if (string.IsNullOrEmpty(cftno))
            {
                throw new Exception("请输入财付通账号！");
            }
        }

        public void btnQuery_Click(object sender, System.EventArgs e)
        {
            try
            {
                ValidateDate();
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }

            try
            {
                BindData(1);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }
        }

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {

            int rid = e.Item.ItemIndex;

            GetDetailList(rid, 1);
        }

        private void GetDetailList(int rid, int index)
        {
            try
            {
                pager2.CurrentPageIndex = index;
                DataTable g_dt = (DataTable)ViewState["g_dt"];
                if (g_dt != null)
                {
                    string plan_id = g_dt.Rows[rid]["plan_id"].ToString();
                    string uin = g_dt.Rows[rid]["withhold_uin"].ToString();
                    int max = pager2.PageSize;
                    int start = max * (index - 1);
                    ViewState["rid"] = rid;

                    DataSet ds = new AutoRechargeService().QueryAutomaticRechargeBillList(uin, plan_id, start, max, Request.UserHostAddress.ToString());
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtAll = null;
                        dtAll = ds.Tables[0].Clone();

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (dr["bill_state"].ToString() == "1")
                            {
                                dtAll.ImportRow(dr);
                            }
                        }
                        this.div2.Visible = true;
                        DataGrid2.DataSource = dtAll.DefaultView;
                        DataGrid2.DataBind();
                    }
                    else
                    {
                        this.div2.Visible = false;
                        DataGrid2.DataSource = null;
                        DataGrid2.DataBind();
                        WebUtils.ShowMessage(this, "查询结果为空");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);
            }
        }

        private void BindData(int index)
        {
            try
            {
                this.div2.Visible = false;
                pager.CurrentPageIndex = index;

                string s_cftno = cftNo.Text.ToString().Trim();
                int max = pager.PageSize;
                int start = max * (index - 1);
                string ip = Request.UserHostAddress.ToString();

                DataSet ds = new AutoRechargeService().QueryAutomaticRecharge(s_cftno, start, max, ip);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dtAll = null;
                    dtAll = ds.Tables[0].Clone();
                    ViewState["g_dt"] = ds.Tables[0];

                    this.div1.Visible = true;
                    DataGrid1.DataSource = dtAll.DefaultView;
                    DataGrid1.DataBind();
                    return;
                }
                else
                {
                    this.div1.Visible = false;
                    DataGrid1.DataSource = null;
                    DataGrid1.DataBind();
                    WebUtils.ShowMessage(this, "查询结果为空");
                    return;
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);
            }

        }

    }
}