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
    public partial class FCXGSPQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        string uid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.lb_operatorID.Text = uid = Session["uid"] as string;
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("PayManagement", this))
                {
                    Response.Redirect("../login.aspx?wh=1");
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                var inputValue = txt_input_id.Text.Trim();
                if (inputValue.Length < 1)
                {
                    WebUtils.ShowMessage(this.Page, "请输入查询条件"); return;
                }

                var spid = inputValue;

                var bll = new FCXGWallet();
                var dt = bll.QueryMerinfoBySpid(Request.UserHostAddress, spid);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    lb_company_name.Text = row["company_name"] as string;
                    lb_email.Text = row["email"] as string;
                    lb_mobile.Text = row["mobile"] as string;
                    lb_phone.Text = row["phone"] as string;
                    lb_spid.Text = row["spid"] as string;
                    lb_address.Text = row["address"] as string;
                    lb_mer_type.Text = "全行业";   //row["mer_type"] as string;   //目前没有对应的字典, 先写死
                    lb_boss_name.Text = row["boss_name"] as string;

                    lb_country.Text = row["country"] as string;
                    lb_area.Text = row["area"] as string;
                    lb_city.Text = row["city"] as string;
                    lb_create_time.Text = row["create_time"] as string;

                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "没有找到数据"); return;
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "出错" + PublicRes.GetErrorMsg(ex.Message)); return;
            }
        }
    }
}