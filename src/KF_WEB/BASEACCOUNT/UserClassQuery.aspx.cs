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
using CFT.CSOMS.BLL.CFTAccountModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// UserClassQuery 的摘要说明。
    /// </summary>
    public partial class UserClassQuery : System.Web.UI.Page
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
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
                Table2.Visible = false;
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
            string stmp = txtQQ.Text.Trim();
            ViewState["fuin"] = classLibrary.setConfig.replaceMStr(stmp);
        }

        protected void Button2_Click(object sender, System.EventArgs e)
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
                Table2.Visible = true;
                pager.RecordCount = 10000;
                BindData(1);
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }

        }

        private void BindData(int index)
        {

            string fuin = ViewState["fuin"].ToString();

            int max = pager.PageSize;
            int start = max * (index - 1) + 1;

            DataSet ds = new AuthenInfoService().GetUserClassQueryListForThis(fuin, start, max);

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("URL", typeof(string));
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["URL"] = "CFTUserCheck.aspx?from=UserClassQuery&fid=&db=&tb=&flist_id=" + dr["flist_id"].ToString();
                }
                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
            }
            else
            {
                throw new LogicException("没有找到记录！");
            }

        }
    }

}
