using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.CFTAccountModule;
using System.Web.Services.Protocols;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    public partial class ClearCreidNew : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblCode1.Text = Session["uid"].ToString();
                lblCode2.Text = Session["uid"].ToString();

                if (!IsPostBack)
                {

                }
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string creid = txtCreid.Text.ToString().Trim();
            try
            {
                if (string.IsNullOrEmpty(creid))
                    throw new Exception("请输入证件号码");               
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);
                return;
            }

            try
            {
                var dt = new CertificateService().GetClearCreidLog(creid);
                this.DataGrid1.DataSource = dt;
                this.DataGrid1.DataBind();          
            }           
            catch (Exception ex)
            {
                this.DataGrid1.DataSource = null;
                this.DataGrid1.DataBind();
                WebUtils.ShowMessage(this.Page, "查询结果为空");
                log4net.LogManager.GetLogger("查询结果为空" + ex.Message);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            string creid = txtCreid2.Text.ToString().Trim();
            try
            {
                if (string.IsNullOrEmpty(creid))
                    throw new Exception("请输入证件号码");
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);
                return;
            }

            int type = rbtPT.Checked ? 0 : 1;
            string uid = Session["uid"].ToString();

            try
            {
                var ret = new CertificateService().ClearCreidInfo(creid, type, uid);
                if (ret)
                {
                    WebUtils.ShowMessage(this.Page, "清理成功");
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "注册未超限，不需要处理");
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "清理失败" + HttpUtility.JavaScriptStringEncode(ex.ToString()));
                log4net.LogManager.GetLogger("清理失败" + ex.Message);
            }
        }

    }
}