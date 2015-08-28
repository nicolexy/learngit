using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.CFTAccountModule;
using System.Web.Services.Protocols;
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    public partial class FetchListIntercept : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblCode1.Text = Session["uid"].ToString();

                if (!IsPostBack)
                {
                    if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
                }
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
          
            try
            {
                InitialData();
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);
                return;
            }

            BindData();
        }

        private void BindData()
        {
            try
            {
                var dt = new PickService().GetFetchListIntercept(ViewState["fetchList"].ToString());
                this.DataGrid1.DataSource = dt;
                this.DataGrid1.DataBind();
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "查询异常" + PublicRes.GetErrorMsg(ex.Message));
            }
        }

        private void InitialData()
        {
            string fetchList = txtFetchList.Text.ToString().Trim();
            if (string.IsNullOrEmpty(fetchList))
                throw new Exception("请输入提现单号");
            ViewState["fetchList"] = fetchList;
        }

        protected void btnIntercept_Click(object sender, EventArgs e)
        {
            try
            {
                InitialData();
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);
                return;
            }

            string uid = Session["uid"].ToString();

            try
            {
                new PickService().AddFetchListIntercept(ViewState["fetchList"].ToString(), uid);
                WebUtils.ShowMessage(this.Page, "拦截成功!");
                BindData();
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "拦截失败:" + PublicRes.GetErrorMsg(ex.Message));
            }
        }

    }
}