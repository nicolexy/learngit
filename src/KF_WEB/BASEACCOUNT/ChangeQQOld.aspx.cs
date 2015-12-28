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
using System.IO;
using System.Configuration;
using System.Data.OleDb;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using CFT.CSOMS.BLL.FundModule;
using CFT.CSOMS.BLL.CFTAccountModule;
using SunLibrary;
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.C2C.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// ChangeQQ 的摘要说明。
    /// </summary>
    public partial class ChangeQQOld : System.Web.UI.Page
    {
        int pageSize = 10;
        int istr = 0;
        string Msg = "";

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面

            try
            {
                //Label1.Text = Session["uid"].ToString();

                string szkey = Session["SzKey"].ToString();
                //int operid = Int32.Parse(Session["OperID"].ToString());

                //if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"CFTUserAppeal")) Response.Redirect("../login.aspx?wh=1");
                if (!ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if (!IsPostBack)
            {
                AspNetPager1.Visible = false;
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
            this.AspNetPager1.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.AspNetPager1_PageChanged);
        }
        #endregion

        private int GetCount()
        {
            string qq = ViewState["qq"].ToString();
            string userid = ViewState["userid"].ToString();
            Query_Service qs = new Query_Service();
            return qs.GetChangeQQListCount(userid, qq);
        }

        private void BindData()
        {
            string qq = ViewState["qq"].ToString();
            string userid = ViewState["userid"].ToString();
           
            if (ViewState["newIndex"] != null)
                istr = Int32.Parse(ViewState["newIndex"].ToString());
            else
                istr = 0;

            DataSet ds = new AccountOperate().GetChangeQQList(userid, qq, istr * pageSize + 1, pageSize);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                AspNetPager1.Visible = false;
                Page.DataBind();  //重新绑定 清除历史数据，避免引起误解

                Msg = "没有您选定范围内的数据。";
                WebUtils.ShowMessage(this.Page, Msg);
                return;
            }

            AspNetPager1.RecordCount = GetCount();
            AspNetPager1.PageSize = pageSize;
            AspNetPager1.CustomInfoText = "记录总数：<font color=\"blue\"><b>" + AspNetPager1.RecordCount.ToString() + "</b></font>";
            AspNetPager1.CustomInfoText += " 总页数：<font color=\"blue\"><b>" + AspNetPager1.PageCount.ToString() + "</b></font>";
            AspNetPager1.CustomInfoText += " 当前页：<font color=\"red\"><b>" + AspNetPager1.CurrentPageIndex.ToString() + "</b></font>";
            DataGrid1.DataSource = ds.Tables[0].DefaultView;
            DataGrid1.DataBind();
        }

        private void ValidateDate()
        {
            ViewState["qq"] = setConfig.replaceMStr(tbqueryQQ.Text.Trim());
            ViewState["userid"] = setConfig.replaceMStr(tbUserID.Text.Trim());
        }

        protected void btnQuery_Click(object sender, System.EventArgs e)
        {
            //查询出明细.
            try
            {
                ValidateDate();
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }

            ViewState["newIndex"] = null;  //如果重新点击一次查询，则清空查询的分页；否则无法查询到数据（比如单笔）
            AspNetPager1.Visible = true;
            this.AspNetPager1.CurrentPageIndex = 1;
            BindData();

        }

        private void AspNetPager1_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {

            istr = e.NewPageIndex;
            AspNetPager1.CurrentPageIndex = istr;
            ViewState["newIndex"] = e.NewPageIndex - 1;
            BindData();
        }
    
        protected void btnChangeQQ_Click(object sender, System.EventArgs e)
        {
            if (OldQQ.Text.Trim() == "")
            {
                WebUtils.ShowMessage(this.Page, "请输入旧帐号！");
                return;
            }

            if (NewQQ.Text.Trim() == "")
            {
                WebUtils.ShowMessage(this.Page, "请输入新帐号！");
                return;
            }

            string outMsg="";
            if (!new AccountOperate().ChangeQQState(OldQQ.Text.Trim(),out outMsg))
            {
                WebUtils.ShowMessage(this.Page, outMsg);
                return;
            }
           
            //发起审批。
            //在这里变成了一个提起审批的流程，而不再是直接审批。
            TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Param[] myParams = new TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Param[3];

            myParams[0] = new TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Param();
            myParams[0].ParamName = "OldQQ";
            myParams[0].ParamValue = OldQQ.Text.Trim();

            myParams[1] = new TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Param();
            myParams[1].ParamName = "NewQQ";
            myParams[1].ParamValue = NewQQ.Text.Trim();

            myParams[2] = new TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Param();
            myParams[2].ParamName = "Memo";
            myParams[2].ParamValue = "修改帐号，原帐号" + OldQQ.Text.Trim() + "，新帐号" + NewQQ.Text.Trim()
                + "。理由：" + TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceSqlStr(tbMemo.Text.Trim());

            TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Check_Service cs = new TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Check_Service();

            TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Finance_Header fh = setConfig.setFH_CheckService(this);

            cs.Finance_HeaderValue = fh;

            //需要生成memo和读出来钱。
            string strMemo = "修改帐号，原帐号" + OldQQ.Text.Trim() + "，新帐号" + NewQQ.Text.Trim()
                + "。理由：" + TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceSqlStr(tbMemo.Text.Trim());

            try
            {
                cs.StartCheck(OldQQ.Text.Trim(), "ChangeQQ", strMemo, "0", myParams);
                PublicRes.writeSysLog(Session["uid"].ToString(), Request.UserHostAddress, "changeqq", "修改帐号提请审批", 1, OldQQ.Text.Trim(), "");
                WebUtils.ShowMessage(this.Page, "提请审批成功。");
            }
            catch (Exception err)
            {
                PublicRes.writeSysLog(Session["uid"].ToString(), Request.UserHostAddress, "changeqq", "修改帐号提请审批", 0, OldQQ.Text.Trim(), "");
                WebUtils.ShowMessage(this.Page, "提请审批失败，错误原因：" + PublicRes.GetErrorMsg(err.Message) + "。" + ", stacktrace" + err.StackTrace);
            }
        }
    }
}
