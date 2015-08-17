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
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.CFT.KF.Common;
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
    /// <summary>
    /// FundCardQuery_DetailNew 的摘要说明。
    /// </summary>
    public partial class FundCardQuery_DetailNew : System.Web.UI.Page
    {
        TradeService service = new TradeService();
        int pagesize = 10;
        private void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            try
            {
                // string sr = Session["key"].ToString();

                Label_uid.Text = Session["uid"].ToString();

                pager.RecordCount = 1000;
                pager.PageSize = pagesize;
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
            if (!IsPostBack)
            {

                string tmp = Request.QueryString["flistID"];
                if (tmp != null && tmp.Trim() != "")
                {
                    this.TextBox1_ListID.Text = tmp;

                    try
                    {
                        BandData(1);
                    }
                    catch (LogicException err)
                    {
                        WebUtils.ShowMessage(this.Page, err.Message);
                    }
                }
                ShowPlan(1);
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
            this.btsearch.Click += new System.EventHandler(this.btsearch_Click);
            this.DGData.SelectedIndexChanged += new System.EventHandler(this.DGData_SelectedIndexChanged);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        void ShowPlan(int type)
        {
            if (type == 1)
            {
                this.Paneltitel.Visible = true;
                this.PanelList.Visible = true;
                this.PanelDetail.Visible = false;
            }
            if (type == 2)
            {
                this.Paneltitel.Visible = false;
                this.PanelList.Visible = false;
                this.PanelDetail.Visible = true;
            }
        }

        private void BandData(int index)
        {
            string Flistid = this.TextBox1_ListID.Text.Trim();
            string fsupplylist = this.TextBox_Fsupply_list.Text.Trim();
            string fcarrdid = this.Textbox_Fcard_id.Text.Trim();

            if (Flistid == "" && fsupplylist == "" && fcarrdid == "")
            {
                WebUtils.ShowMessage(this.Page, "查询条件必须至少填一个！");
                return;
            }

            DataSet ds = service.GetFundCardListDetail(Flistid, fsupplylist, fcarrdid, (index - 1) * pagesize, pagesize);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                this.DGData.DataSource = ds.Tables[0].DefaultView;
                this.DGData.DataBind();
            }
            else
            {
                WebUtils.ShowMessage(this.Page, "没有找到记录！");
            }
        }

        private void btsearch_Click(object sender, System.EventArgs e)
        {
            ShowPlan(1);
            BandData(1);
        }

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BandData(pager.CurrentPageIndex);
        }

        private void DGData_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            string flistid = DGData.SelectedItem.Cells[0].Text.Trim();
            if (flistid != "")
            {
                ShowPlan(2);

                DataSet ds = service.GetFundCardListDetail(flistid, "", "", 0, 1);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    labFListID.Text = PublicRes.GetString(dr["FListID"]);
                    labFNum.Text = PublicRes.GetString(dr["FNumYuan"]);
                    labFState.Text = PublicRes.GetString(dr["FStateName"]);
                    labFSign.Text = PublicRes.GetString(dr["FSignName"]);
                    labFsupply_list.Text = PublicRes.GetString(dr["Fsupply_list"]);
                    labFsp_back_prove.Text = PublicRes.GetString(dr["Fsp_back_prove"]);
                    LabFcard_id.Text = PublicRes.GetString(dr["Fcard_id"]);
                    LabFcard_type.Text = PublicRes.GetString(dr["FCardtypeName"]);
                    labFsupply_id.Text = PublicRes.GetString(dr["Fsupply_id"]);
                    labFuin.Text = PublicRes.GetString(dr["Fuin"]);
                    labFuser_name.Text = PublicRes.GetString(dr["Fuser_name"]);
                    labFpay_front_time.Text = PublicRes.GetDateTime(dr["Fpay_front_time"]);
                    labFsp_time.Text = PublicRes.GetDateTime(dr["Fsp_time"]);
                    labFmodify_time.Text = PublicRes.GetDateTime(dr["FModify_time"]);
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "没有找到记录！");
                }
            }
        }
    }
}
