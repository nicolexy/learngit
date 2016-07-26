using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.ForeignCurrencyModule;
using CFT.CSOMS.COMMLIB;
using System.Data;

namespace TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay
{
    public partial class FCXGBindCardQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
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
            }
        }

        #region 绑卡查询
        //绑卡查询
        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = null;
                var input = txt_input.Text.Trim();
                if (input.Length > 0)
                {
                    var bll = new FCXGWallet();
                    if (checkBankCard.Checked)
                    {
                        dt = bll.QueryBindCardInfo(Request.UserHostAddress, null, input);
                    }
                    else
                    {
                        string QueryUin = "";
                        if (checkUin.Checked)
                        {
                            QueryUin = input;
                        }
                        else if (checkWeChatId.Checked)
                        {
                            QueryUin = WeChatHelper.GetFCXGOpenIdFromWeChatName(input, Request.UserHostAddress) + "@wx.hkg";
                        }
                        dt = bll.QueryBindCardInfo(Request.UserHostAddress, QueryUin, null);
                    }

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dt.Columns.Add("UnbundlingUrl");
                        dt.Columns.Add("UnbundInfoUrl");
                        foreach (DataRow row in dt.Rows)
                        {
                            var bind_serialno = row["bind_serialno"];
                            var uin = row["uin"];
                            var Queryuid = row["uid"];
                            var card_tail = row["card_tail"];
                            row["UnbundlingUrl"] = string.Format("FCXGDialogUnBindCard.aspx?sign=unbind&bind_serialno={0}&uin={1}&card_tail={2}", bind_serialno, uin, card_tail);
                            row["UnbundInfoUrl"] = string.Format("FCXGDialogUnBindCard.aspx?sign=unbind_info&bind_serialno={0}&uid={1}", bind_serialno, Queryuid);
                        }
                    }
                    else
                    {
                        WebUtils.ShowMessage(this.Page, "没有找到数据");
                    }
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "请输入查询条件");
                }

                dg_passwordLog.DataSource = dt;
                dg_passwordLog.DataBind();
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "查询出错" + PublicRes.GetErrorMsg(ex.Message));
            }
        }

        //绑卡列表的,行绑定事件
        protected void dg_passwordLog_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemIndex > -1)
            {
                var row = e.Item.DataItem as DataRowView;
                if ((string)row["bind_status"] == "2")
                {//解绑操作

                    e.Item.Cells[12].Controls[0].Visible = false;
                }
                else
                { //解绑详细
                    e.Item.Cells[11].Controls[0].Visible = false;
                }
            }
        }
        #endregion

    }
}