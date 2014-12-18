using System;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.WechatPay;
using CFT.CSOMS.BLL.SPOA;
using System.Data;

namespace TENCENT.OSS.CFT.KF.KF_Web.WebchatPay
{
    public partial class WechatMerchantTrans : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                var accId = txtAccId.Text.Trim();

                if (string.IsNullOrEmpty(accId))
                {
                    WebUtils.ShowMessage(this.Page, "账号输入有误");
                    return;
                }

                BindInfo(accId);
            }
            
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }
        }

        private void BindInfo(string accId)
        {
            //微信支付商户号TO财付通商户号
            var spid = new WechatMerchantService().WechatMchIdToSpId(accId);
            lbSpid.Text = spid;
            
            //财付通商户号TO微信支付商户号
            var mchid = new WechatMerchantService().WechatSpIdToMchId(accId);
            lbMchid.Text = mchid;

            //APPID

        }
    }
}