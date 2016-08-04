/*
 
 提现冻结资金查询功能已下架，此页面暂弃用 2016-01-05 darrenran
 
 */

//using CFT.CSOMS.BLL.FreezeModule;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using Tencent.DotNet.Common.UI;

//namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
//{
//    public partial class CashOutFreezeQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            lblFuid.Text = Session["uid"].ToString();
//        }

//        protected void btnQuery_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                ValidateInput();
//            }
//            catch (Exception err)
//            {
//                WebUtils.ShowMessage(this.Page, err.Message);
//                return;
//            }
//            BindData();
//        }
//        private void ValidateInput()
//        {
//            var id = this.txtFuid.Text.Trim();
//            if (string.IsNullOrEmpty(id))
//            {
//                throw new Exception("请输入内部ID！");
//            }
//        }

//        private void BindData()
//        {
//            try
//            {
//                var id = this.txtFuid.Text.Trim();
//                lblCashOutBillNo.Text = (new FreezeService()).GetCashOutFreezeListId(id);
//            }
//            catch (Exception ex)
//            {
//                string errStr = PublicRes.GetErrorMsg(ex.Message);
//                WebUtils.ShowMessage(this.Page, "查询异常：" + errStr + ", stacktrace" + ex.StackTrace);
//            }
//        }
//    }
//}