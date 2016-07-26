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
    public partial class FCXGDialogUnBindCard : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        string uid;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.lb_operatorID.Text = lab_info_uid.Text = uid = Session["uid"] as string;
                if (!IsPostBack)
                {
                    if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
                    {
                        Response.Redirect("../login.aspx?wh=1");
                    }

                    var bind_serialno = (ViewState["bind_serialno"] = Request.QueryString["bind_serialno"]) as string;

                    var sign = Request.QueryString["sign"];
                    if (sign == "unbind")
                    {
                        ViewState["queryUin"] = lb_userName.Text = Request.QueryString["uin"];
                        tab_unbind.Visible = true;
                        ViewState["card_tail"] = lb_cardNo.Text = Request.QueryString["card_tail"];
                    }
                    else if (sign == "unbind_info")
                    {
                        var queryuid = (string)(ViewState["queryuid"] = Request.QueryString["uid"]);
                        tab_unbind_info.Visible = true;
                        QueryUnbindInfo(bind_serialno, queryuid);
                    }
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);
            }
        }

        //解绑卡
        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                var queryUin = ViewState["queryUin"] as string;
                var bind_serialno = ViewState["bind_serialno"] as string; ;
                var true_name = txt_trueName.Text.Trim();
                var phone = txt_contact.Text.Trim();
                var reason = txt_reason.Text.Trim();
                if (true_name.Length < 1)
                {
                    throw new Exception("用户姓名不可以为空");
                }
                if (phone.Length < 1)
                {
                    throw new Exception("联系方式不可以为空");
                }

                FCXGWallet bll = new FCXGWallet();
                var result = bll.FreeBindCard(queryUin, bind_serialno, uid, true_name, phone, reason, "", Request.UserHostAddress);
                if (result)
                {
                    Close("解绑成功");
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "解绑失败");
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, PublicRes.GetErrorMsg(ex.Message));
            }
        }

        //解绑卡详细
        protected void QueryUnbindInfo(string bind_serialno, string queryUin)
        {
            FCXGWallet bll = new FCXGWallet();
            var dt = bll.QueryBindCardLog(queryUin, bind_serialno, 0, 1);
            if (dt == null || dt.Rows.Count < 1)
            {
                WebUtils.ShowMessage(this.Page, "查询解绑详细失败"); return;
            }

            var row = dt.Rows[0];
            info_bindTime.Text = row["fbind_time"] as string;
            info_card.Text = row["fcard_tail"] as string;
            info_contact.Text = row["fphone"] as string;
            info_opName.Text = row["fop_name"] as string;
            info_reason.Text = row["freason"] as string;
            info_trueName.Text = row["ftrue_name"] as string;
            info_uin.Text = row["fuin"] as string;
            info_unbindTime.Text = row["funbind_time"] as string;
            info_unbind_type_str.Text = row["unbind_type_str"] as string;
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

        protected void UnbindInfo_Close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}