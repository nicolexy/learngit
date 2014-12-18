using CFT.CSOMS.BLL.FreezeModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    public partial class CashOutFreezeQuery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblFuid.Text = Session["uid"].ToString();
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateInput();
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }
            BindData();
        }
        private void ValidateInput()
        {
            var id = this.txtFuid.Text.Trim();
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("请输入内部ID！");
            }
        }

        private void BindData()
        {
            var id = this.txtFuid.Text.Trim();
            lblCashOutBillNo.Text = (new FreezeService()).GetCashOutFreezeListId(id);
        }
    }
}