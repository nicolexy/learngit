using CFT.CSOMS.BLL.CFTAccountModule;
using CFT.CSOMS.BLL.FundModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.WebchatPay
{
    public partial class LCTSwitchQuery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tbdetail.Visible = false;
                try
                {
                    string uid = Session["uid"].ToString();
                }
                catch  //如果没有登陆或者没有权限就跳出
                {
                    Response.Redirect("../login.aspx?wh=1");
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                tbdetail.Visible = true;
                FundService service = new FundService();
                if (this.txt_input_id.Text.Trim() == "")
                {
                    throw new Exception("请输入账号！");
                }
                string userType = UserType();
                string uin = AccountService.GetQQID(userType, this.txt_input_id.Text.Trim());
                string tradeId = service.GetTradeIdByUIN(uin);

                if (this.txtListid.Text.Trim() == "")
                {
                    throw new Exception("请输入单号！");
                }

                string buy_id = "";
                string redem_id = "";
                string change_id = "";
                if (Radio_buy_id.Checked)
                {
                    buy_id = txtListid.Text.Trim();
                }
                if (Radio_redem_id.Checked)
                {
                    redem_id = txtListid.Text.Trim();
                }
                if (Radio_change_id.Checked)
                {
                    change_id = txtListid.Text.Trim();
                }
//#if DEBUG
//                uin = "13123123123123";
//                tradeId = "20151229288671604";
//                buy_id = "1301898101201603213144462879";
//                redem_id = "1217608401201603213098505789";
//                change_id = "1217608401201603213062900227604";
//#endif
                DataTable dt = service.GetLCTSwith(tradeId, buy_id, redem_id, change_id);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    lbluin.Text = uin;
                    lblFchange_id.Text = dr["Fchange_id"].ToString();
                    lblFbuy_id.Text = dr["Fbuy_id"].ToString();
                    lblFredem_id.Text = dr["Fredem_id"].ToString();
                    lblFtotal_fee.Text = dr["Ftotal_fee"].ToString();
                    lblFori_fund_name.Text = dr["Fori_fund_name"].ToString();
                    lblFnew_fund_name.Text = dr["Fnew_fund_name"].ToString();
                    lblFstateStr.Text = dr["FstateStr"].ToString();
                    lblFacc_time.Text = dr["Facc_time"].ToString();
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.ToString()));
            }
        }

        string UserType() 
        {
            string UserType = "";
            foreach (var con in this.form1.Controls)
            {
                if (con is RadioButton) 
                {
                    RadioButton radio = con as RadioButton;
                    if (radio.GroupName == "UserType" && radio.Checked == true)
                    {
                        UserType = radio.ID;
                        break;
                    }
                }
            }
            return UserType;
        }
    }
}