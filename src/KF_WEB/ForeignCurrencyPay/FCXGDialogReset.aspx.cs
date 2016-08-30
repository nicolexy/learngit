using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.ForeignCurrencyModule;
using SunLibrary;

namespace TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay
{
    public partial class FCXGDialogReset : PageBase
    {
        string uid, sign;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.lb_operatorID.Text = uid = Session["uid"] as string;
                sign = Request.QueryString["sign"] ?? ViewState["sign"] as string;
                if (!IsPostBack)
                {
                    ChekedShowTitle(sign);
                    ViewState["sign"] = sign;
                    var uin = Request.QueryString["uin"] as string;
                    if (string.IsNullOrEmpty(uin))
                    {
                        throw new ArgumentNullException("uin", "未发现uin字段");
                    }
                    lb_uin.Text = uin;
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);
            }
        }
        //重置
        protected void Button1_Click(object sender, EventArgs e)
        {
            var uin = lb_uin.Text;
            var reason = txt_reason.Text.Trim();
            var contact = txt_contact.Text.Trim();
            var trueName = txt_userName.Text.Trim();
            var ip = Request.UserHostAddress;
            var bll = new FCXGWallet();

            try
            {
                if (trueName.Length < 1)
                {
                    throw new Exception("用户姓名不可以为空");
                }
                if (contact.Length < 1)
                {
                    throw new Exception("联系方式不可以为空");
                }

                bool result = false;
                //注意：快速重置和申诉重置调用的接口是一样的。
                LogHelper.LogInfo(string.Format("{0}调用接口开始"),DateTime.Now.ToString());
                if (sign == "quick_reset" || sign == "appeal_reset")
                {
                    result = bll.ResetPassWord(uin, uid, trueName, contact, reason, "", ip);
                }
                LogHelper.LogInfo(string.Format("{0}调用接口结束"), DateTime.Now.ToString());
                //else if (sign == "appeal_reset")
                //{
                //    throw new Exception("暂未开放申诉重置功能!");
                //    //result = bll.ResetPassWord(uin, uid, "", trueName, contact, reason, "", ip);
                //}

                if (result)
                {
                    Close(lb_title.InnerText + "成功");
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, lb_title.InnerText + "失败");
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, PublicRes.GetErrorMsg(ex.Message));
                LogHelper.LogInfo(string.Format("调用接口出错{0}"), PublicRes.GetErrorMsg(ex.Message));
            }
        }

        /// <summary>
        /// 选择显示标题
        /// </summary>
        /// <param name="type">快速重置/申诉重置(quick_reset/appeal_reset)</param>
        private void ChekedShowTitle(string sign)
        {
            if (sign == "quick_reset")
            {
                lb_title.InnerText = pageTitle.Text = "快速重置";
            }
            else if (sign == "appeal_reset")
            {
                lb_title.InnerText = pageTitle.Text = "申诉重置";
            }
            else
            {
                throw new ArgumentException("未知的值sign:" + sign, "sign");
            }
        }


        private void Close(string msg = null)
        {
            var js = new System.Web.UI.HtmlControls.HtmlGenericControl("script");
            if (msg != null)
            {
                js.InnerHtml += "alert('" + msg + "');";
            }
            js.InnerHtml += "window.opener=null;window.open('','_self');window.close();";
            Page.Header.Controls.Add(js);
        }
    }
}