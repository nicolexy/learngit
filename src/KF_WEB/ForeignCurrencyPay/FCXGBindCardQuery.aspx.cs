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
    public partial class FCXGBindCardQuery : System.Web.UI.Page
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
                var bll = new FCXGWallet();
                DataTable dt = null;
                var cardno = txt_CardNo.Text.Trim();

                var queryUin = GetUin();
                if (queryUin == "" && cardno == "")
                {
                    WebUtils.ShowMessage(this.Page, "请输入查询条件");
                }
                else
                {
                    dt = bll.QueryBindCardInfo(Request.UserHostAddress, queryUin, cardno);
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


        //获取Uin
        protected string GetUin()
        {
            var input = txt_input.Text.Trim();
            if (input.Length < 1)
            {
                return "";
            }

            if (checkUin.Checked)
            {
                return input;
            }
            else if (checkWeChatId.Checked)
            {
                return WeChatHelper.GetFCXGOpenIdFromWeChatName(input) + "@wx.hkg";
            }
            return "";
        }

    }
}