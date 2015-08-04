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
using System.Configuration;

using Wuqi.Webdiyer;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// RefundNew 的摘要说明。
    /// </summary>
    public partial class RefundNew : System.Web.UI.Page
    {
        protected System.Data.DataSet dataSet1;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            if (!IsPostBack)
            {
                try
                {
                    string type = "buy";
                    if (Request.QueryString["type"] != null)
                    {
                        type = Request.QueryString["type"].ToString().Trim();
                    }
                    ViewState["type"] = type;
                    //绑定第一页数据
                    BindData(1, type);
                }
                catch (Exception)
                {
                    WebUtils.ShowMessage(this.Page, "操作超时！请重新查询。");
                }
            }
        }

        private void BindData(int pageIndex, string type)
        {
            DateTime beginTime = DateTime.Now.AddDays(-PublicRes.PersonInfoDayCount);
            DateTime endTime = DateTime.Now.AddDays(1);

            int pageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"].ToString());  //通过webconfig控制页大小

            int istr = 1 + pageSize * (pageIndex - 1);   //初始为1（事实上索引0始）
            int imax = pageSize;                       //每页显示10条记录

            int querytype = 0;
            if (type == "sale")
                querytype = 1;

            string selectStr = Session["QQID"].ToString();
            this.dataSet1 = new TradeService().GetRefund(selectStr, querytype, beginTime, endTime, istr, imax);

            int total;
            if (dataSet1 != null && dataSet1.Tables.Count != 0 && dataSet1.Tables[0].Rows.Count != 0)
                total = Int32.Parse(dataSet1.Tables[0].Rows[0]["total"].ToString());
            else
                total = 0;

            pager.RecordCount = total;
            pager.PageSize = pageSize;

            pager.CustomInfoText = "记录总数：<font color=\"blue\"><b>" + pager.RecordCount.ToString() + "</b></font>";
            pager.CustomInfoText += " 总页数：<font color=\"blue\"><b>" + pager.PageCount.ToString() + "</b></font>";
            pager.CustomInfoText += " 当前页：<font color=\"red\"><b>" + pager.CurrentPageIndex.ToString() + "</b></font>";

            Page.DataBind();
        }

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;

            string type = ViewState["type"].ToString();

            BindData(pager.CurrentPageIndex, type);
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
            this.dataSet1 = new System.Data.DataSet();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "NewDataSet";
            this.dataSet1.Locale = new System.Globalization.CultureInfo("zh-CN");
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();

        }
        #endregion
    }
}
