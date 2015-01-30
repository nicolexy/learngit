using System;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.WechatPay;
using System.Data;

namespace TENCENT.OSS.CFT.KF.KF_Web.WebchatPay
{
    public partial class QueryWechatRealNameAuthen : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                var accId = txtAccId.Text.Trim();
                var serialNo = txtSerialno.Text.Trim();

                if (string.IsNullOrEmpty(accId) || string.IsNullOrEmpty(serialNo))
                {
                    WebUtils.ShowMessage(this.Page, "账号和序列号不能为空！");
                    return;
                }

                BindInfo(accId, serialNo);
            }
            
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }
        }

        private void BindInfo(string accId, string serialNo)
        {
            //微信支付商户号TO财付通商户号
            DataSet ds = new AuthenService().QueryWechatRealNameAuthen(accId, serialNo, this.Page.Request.UserHostAddress);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) 
            {
                lbAuthenState.Text = ds.Tables[0].Rows[0]["res_info"].ToString();
            }
            
            

            //APPID

        }
    }
}