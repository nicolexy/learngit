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
using System.Text.RegularExpressions;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
    /// <summary>
    /// TradeMigration 的摘要说明。
    /// </summary>
    public class TradeMigration : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Label Label_uid;
        protected System.Web.UI.WebControls.Button btMigration;
        protected System.Web.UI.WebControls.Label labErrMsg;
        protected System.Web.UI.WebControls.Label lblUId;
        protected System.Web.UI.WebControls.TextBox txtTradeId;
        //protected System.Web.UI.WebControls.TextBox txtTradeId;sayid原本上面有了tradeId，这一行的order替换了就多了
        protected System.Web.UI.WebControls.RegularExpressionValidator rfvNum;

        private void Page_Load(object sender, System.EventArgs e)
        {
            lblUId.Text = Session["uid"].ToString();
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
            this.btMigration.Click += new System.EventHandler(this.btMigration_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        private void btMigration_Click(object sender, System.EventArgs e)
        {

            string tradeId = txtTradeId.Text.Trim();
            if (!SunLibraryEX.StringEx.IsNumber(tradeId) || !SunLibraryEX.StringEx.MatchLength(tradeId, 0, 32))
            {
                labErrMsg.Text = "交易单号有误，交易单号必须为小于等于32位数字！";
                WebUtils.ShowMessage(this.Page, labErrMsg.Text);
                return;
            }
            string msg = "";
            if (!MigrationCheck(tradeId, out  msg))
            {
                labErrMsg.Text = "提起交易单迁移审批失败，失败信息如下:<br>" + msg;
                WebUtils.ShowMessage(this.Page, "提起交易单迁移审批失败，失败信息如下：" + PublicRes.GetErrorMsg(msg));
                return;
            }
            else
            {
                labErrMsg.Text = "提起交易单迁移审批申请成功!";
                WebUtils.ShowMessage(this.Page, labErrMsg.Text);

            }
        }
        //设置soap头信息
        private ZWCheck_Service.Finance_Header SetWebServiceHeader(TemplateControl page)
        {

            ZWCheck_Service.Finance_Header header = new ZWCheck_Service.Finance_Header();
            //header.SrcUrl = page.Page.Request.Url.ToString();
            header.UserIP = page.Page.Request.UserHostAddress;
            header.UserName = (page.Page.Session["uid"] == null) ? "" : page.Page.Session["uid"].ToString();
            //header.SessionID = page.Page.Session.SessionID;
            header.SzKey = (page.Page.Session["SzKey"] == null) ? "" : page.Page.Session["SzKey"].ToString();
            header.OperID = (page.Page.Session["OperID"] == null) ? 0 : Int32.Parse(page.Page.Session["OperID"].ToString());
            header.RightString = (page.Page.Session["SzKey"] == null) ? "" : page.Page.Session["SzKey"].ToString();
            return header;
        }
        private bool MigrationCheck(string tradeId, out string msg)
        {
            msg = "";
            try
            {
                ZWCheck_Service.Check_Service checkService = new ZWCheck_Service.Check_Service();
                ZWCheck_Service.Param[] parameters = new ZWCheck_Service.Param[1];
                parameters[0] = new ZWCheck_Service.Param();
                parameters[0].ParamName = "MsgId";
                parameters[0].ParamValue = tradeId;
                checkService.Finance_HeaderValue = SetWebServiceHeader(this);
                checkService.StartCheck(tradeId, "TradeMigration", "交易单迁移申请", "0", parameters);
                //PublicRes.CreateCheckService(this).StartCheck(tradeId, "TradeMigration", "交易单迁移申请", "0", parameters);
                return true;

            }
            catch (Exception ex)
            {
                msg += ex.Message;
                return false;
            }

        }
    }
}
