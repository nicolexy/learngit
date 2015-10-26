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
    public partial class FCXGAccountQuery : System.Web.UI.Page
    {
        string uid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.lb_operatorID.Text = uid = Session["uid"] as string;
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
                {
                    Response.Redirect("../login.aspx?wh=1");
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

                var dt_user = bll.QueryUserInfo(queryuid);

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

                    if (true) //后期加入账户总余额判断  账户总余额>30港币   展示 申诉
                    {
                        btn_quick_reset.HRef = string.Format("FCXGDialogReset.aspx?sign=quick_reset&uin={0}", uin);
                        btn_quick_reset.InnerText = "快速重置密码";
                    }
                    else
                    {
                        btn_quick_reset.HRef = string.Format("FCXGDialogReset.aspx?sign=appeal_reset&uin={0}", uin);
                        btn_quick_reset.InnerText = "申诉重置密码";
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
        }
    }
}