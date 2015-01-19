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
using CFT.CSOMS.BLL.CFTAccountModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// ClearCreid 的摘要说明。
    /// </summary>
    public partial class ClearCreid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label4.Text = Session["uid"].ToString();
                Label7.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {

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

        }
        #endregion

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {

        }

        private void ValidateDate()
        {
            string s_creid = creid.Text.ToString().Trim();

            if (string.IsNullOrEmpty(s_creid))
            {
                throw new Exception("请输入证件号码！");
            }
        }

        public void btnClear_Click(object sender, System.EventArgs e)
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
                BindData();
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

        private void BindData()
        {
            string s_creid = creid.Text.ToString();
            int type = rbpt.Checked ? 0 : 1;

            //判断清理次数，大于2则提示
            //20150116 放开限制，可以无限次清理。
            //var result = (new AccountService()).GetClearCreidCount(s_creid, type);
            //if (result >= 2)
            //{
            //    WebUtils.ShowMessage(this.Page, "操作失败：清理次数已超过2次！");
            //    return;
            //}

            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            int ret = qs.ClearCreid(type, s_creid);
            if (ret == 1)
            {
                WebUtils.ShowMessage(this.Page, "清理成功！");
                //记录操作日志
                (new AccountService()).WriteClearCreidLog(s_creid, type, Session["uid"].ToString());
            }
            else
            {
                WebUtils.ShowMessage(this.Page, "清理失败：" + s_creid + "不存在！");
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {

            string s_creid = txtCardId.Text.ToString().Trim();
            try
            {
                if (string.IsNullOrEmpty(s_creid))
                {
                    throw new Exception("请输入证件号码！");
                }
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }

            try
            {
                QueryData(s_creid);
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

        private void QueryData(string creid)
        {
            var dt = (new AccountService()).GetClearCreidLog(creid);
            this.DataGrid1.DataSource = dt;
            this.DataGrid1.DataBind();
        }
    }
}