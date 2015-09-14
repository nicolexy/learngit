using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.ForeignCurrencyModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay
{
    public partial class FCXGDialogFreezeOrUnfreeze : System.Web.UI.Page
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
                    if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
                    {
                        Response.Redirect("../login.aspx?wh=1");
                    }

                    ViewState["sign"] = sign;
                    var uin = Request.QueryString["uin"] as string;
                    if (string.IsNullOrEmpty(uin))
                    {
                        throw new ArgumentNullException("uin", "未发现uin字段");
                    }
                    lb_uin.Text = uin;
                    ChekedShow(sign, uin);
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);
            }
        }


        //冻结/解冻
        protected void Button1_Click(object sender, EventArgs e)
        {
            var uin = lb_uin.Text;
            var reason = txt_reason.Text.Trim();
            var contact = txt_contact.Text.Trim();
            var trueName = txt_userName.Text.Trim();
            var channel = list_channel.SelectedValue;
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

                if (sign == "freeze")
                {
                    result = bll.LockUser(uin, 0, channel, uid, trueName, contact, reason, "", ip);
                }
                else if (sign == "unfreeze")
                {
                    result = bll.LockUser(uin, 1, channel, uid, trueName, contact, reason, "", ip);
                }
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
            }

        }

        /// <summary>
        /// 选择显示标题
        /// </summary>
        /// <param name="type">冻结/解冻(freeze/unfreeze)</param>
        private void ChekedShow(string sign, string uin)
        {
            if (sign == "freeze")
            {
                lb_title.InnerText = Button1.Text = pageTitle.Text = "冻结账户";
                lb_reason.InnerText = "冻结原因";
            }
            else if (sign == "unfreeze")
            {
                lb_title.InnerText = Button1.Text = pageTitle.Text = "解冻账户";
                lb_reason.InnerText = "解冻原因";
                txt_contact.Enabled = false;
                txt_userName.Enabled = false;
                list_channel.Enabled = false;

                var bll = new FCXGWallet();
                var param = new Dictionary<string, string>()
                {
                    {"uin",uin}
                };
                var dt = bll.QueryFreezeLog(param, 0, 1);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    txt_contact.Text = (string)row["ftrue_name"];
                    txt_userName.Text = (string)row["fphone"];
                    list_channel.SelectedValue = (string)row["fchannel"];
                }
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