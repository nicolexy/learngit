using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.ForeignCurrencyModule;
using CFT.CSOMS.COMMLIB;

namespace TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay
{
    public partial class FCXGAccountQuery : PageBase
    {
        string uid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.lb_operatorID.Text = uid = Session["uid"] as string;
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
                {
                    Response.Write("<script>window.parent.location.href = '../login.aspx';</script>");
                    Response.End();
                }
                pager.RecordCount = 1000;
            }

        }

        //查询
        protected void btn_Query_Click(object sender, EventArgs e)
        {
            try
            {
                lb_WeChatID.Text = "";
                info_box.Visible = false;
                string input = txt_input_id.Text.Trim();
                if (input.Length < 1)
                {
                    WebUtils.ShowMessage(this.Page, "请输入查询条件!"); return;
                }

                var bll = new FCXGWallet();
                var queryuid = input;
                if (!checkUid.Checked)  //如果不是uid
                {
                    var uin = input;
                    if (checkWeChatId.Checked)
                    {
                        lb_WeChatID.Text = input;
                        uin = WeChatHelper.GetFCXGOpenIdFromWeChatName(input, Request.UserHostAddress) + "@wx.hkg";
                    }
                    queryuid = bll.QueryUserId(uin);
                }
                string ip = this.Page.Request.UserHostAddress.ToString();
                if (ip == "::1")
                    ip = "127.0.0.1";
                var dt_user = bll.QueryUserInfo(queryuid, ip);

                if (dt_user != null && dt_user.Rows.Count > 0)
                {
                    var row = dt_user.Rows[0];
                    BindUserInfo(row);
                    var lstate = row["lstate"] as string;
                    var uin = row["uin"] as string;
                    ViewState["queryUin"] = uin;

                    #region 冻结解冻按钮

                    btn_freeze.Visible = true;
                    if (lstate == "1")
                    {
                        btn_freeze.HRef = string.Format("FCXGDialogFreezeOrUnfreeze.aspx?sign=freeze&uin={0}", uin);
                        btn_freeze.InnerText = "冻结";
                    }
                    else if (lstate == "2")
                    {
                        btn_freeze.HRef = string.Format("FCXGDialogFreezeOrUnfreeze.aspx?sign=unfreeze&uin={0}", uin);
                        btn_freeze.InnerText = "解冻";
                    }
                    else
                    {
                        btn_freeze.Visible = false;
                    }

                    #endregion

                    #region 重置按钮
                    string total = string.IsNullOrEmpty(lbl_total.Text) ? "0" : lbl_total.Text;

                    //http://tapd.oa.com/tenpay_kf/prong/stories/view/1010068761057415079
                    //总账户余额<30.00，展示按钮：“快速重置密码“，无权限设置
                    //v_swuzhang

                    btn_pwd_reset.Visible = true;
                    if (Convert.ToDecimal(total) < 30)
                    {
                        btn_pwd_reset.Text = "快速重置密码";
                    }
                    else
                    {//总账户余额≥30.00，展示按钮：“申诉重置密码“，设置权限，权限ID：86，并如果此时当前客服系统登录账户的权限位86未校验通过，则隐藏“申诉重置密码“按钮。
                        if (classLibrary.ClassLib.ValidateRight("FCXGAccountPassWordReset", this))
                        {
                            btn_pwd_reset.Text = "申诉重置密码";
                        }
                        else
                        {
                            btn_pwd_reset.Text = "";
                            btn_pwd_reset.Visible = false;
                        }

                    }
                    #endregion
                }
                else
                {
                    BindUserInfo(null);
                    WebUtils.ShowMessage(this.Page, "没有找到数据!"); return;
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "异常:" + PublicRes.GetErrorMsg(ex.Message));
            }
            dg_passwordLog.DataSource = null;
            dg_passwordLog.DataBind();
        }

        //查看日记按钮事件
        protected void QueryLog_Click(object sender, EventArgs e)
        {
            pager.CurrentPageIndex = 1;
            BindRestLog();
        }

        //分页控件
        protected void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindRestLog();
        }

        //查询日记
        private void BindRestLog()
        {
            // limit 只能是10   借口限定的
            DataTable dt = null;
            try
            {
                var bll = new FCXGWallet();
                var uin = ViewState["queryUin"] as string;
                var param = new Dictionary<string, string>()
                {
                    {"uin",uin},
                };
                var skip = (pager.CurrentPageIndex - 1) * pager.PageSize;

                dt = bll.QueryResetPassWordLog(param, skip, pager.PageSize);

                if (dt == null || dt.Rows.Count < 1)
                {
                    dt = null;
                    WebUtils.ShowMessage(this.Page, "没有记录");
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, PublicRes.GetErrorMsg(ex.Message));
            }
            dg_passwordLog.DataSource = dt;
            dg_passwordLog.DataBind();
        }

        /// <summary>
        /// 显示查询的用户详情
        /// </summary>
        /// <param name="dt"></param>
        private void BindUserInfo(DataRow row)
        {
            if (row == null)
            {
                info_box.Visible = false; return;
            }

            Action<System.Web.UI.WebControls.Label, string> SetValue = (lb, name) =>
            {
                if (row.Table.Columns.Contains(name))
                {
                    lb.Text = row[name] as string;
                }
            };

            info_box.Visible = true;

            SetValue(lb_uin, "uin");
            SetValue(lb_uid, "uid");
            //  SetValue(lb_WeChatID, "uin");
            SetValue(lb_mobile, "mobile");
            SetValue(lb_state, "state_str");
            SetValue(lb_user_type_str, "user_type_str");
            SetValue(lb_create_time, "create_time");
            SetValue(lb_lstate_str, "lstate_str");

            SetValue(lb_lstate_str, "lstate_str");
            SetValue(lbl_balance, "balance");
            lbl_freeze.Text = "0.00";
            lbl_total.Text = (Convert.ToDecimal(lbl_balance.Text) + Convert.ToDecimal(lbl_freeze.Text)).ToString();
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_pwd_reset_Click(object sender, EventArgs e)
        {
            if (btn_pwd_reset.Text == "快速重置密码") //后期加入账户总余额判断  账户总余额>30港币   展示 申诉
            {
                Response.Redirect(string.Format("FCXGDialogReset.aspx?sign=quick_reset&uin={0}", ViewState["queryUin"].ToString()));
            }
            else if(btn_pwd_reset.Text == "申诉重置密码")
            {
                if (classLibrary.ClassLib.ValidateRight("FCXGAccountPassWordReset", this))
                {
                    Response.Redirect(string.Format("FCXGDialogReset.aspx?sign=appeal_reset&uin={0}", ViewState["queryUin"].ToString()));
                }
            }
        }
    }
}