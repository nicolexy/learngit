using CFT.CSOMS.BLL.CFTAccountModule;
using CFT.CSOMS.BLL.CreditModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    public partial class TencentCreditQuery : System.Web.UI.Page
    {
        public string uid;
        protected void Page_Load(object sender, EventArgs e)
        {
            uid = Session["uid"] as string;
            if (!ClassLib.ValidateRight("InfoCenter", this))
                Response.Redirect("../login.aspx?wh=1");
            lb_operatorID.Text = uid;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                credit_rank.InnerHtml = "";
                credit_star.InnerText = "";
                var bll = new AccountService();
                var uin = tbx_uin.Text;
                if (string.IsNullOrEmpty(uin))
                {
                    WebUtils.ShowMessage(this.Page, "请输入查询的QQ号");
                    return;
                }
                var ds = new TencentCreditService().TencentCreditQuery(uin.Trim(), uid);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    WebUtils.ShowMessage(this.Page, "没有找到数据");
                    return;
                }
                var row = ds.Tables[0].Rows[0];
                credit_rank.InnerHtml = row["credit_rank"].ToString() + "%";
                credit_star.InnerText = SwitchStar(row["credit_star"].ToString());
            }
            catch (Exception ex)
            {
                string errStr = PublicRes.GetErrorMsg(ex.Message);
                WebUtils.ShowMessage(this.Page, errStr);
            }
        }
        /// <summary>
        /// 把数字的参数转换成星级
        /// </summary>
        /// <param name="credit_star"></param>
        /// <returns></returns>
        private string SwitchStar(string credit_star)
        {
            if (!string.IsNullOrEmpty(credit_star))
            {
                var star_num = int.Parse(credit_star);
                if (star_num >= 10)
                {
                    int shiwei = star_num / 10;
                    int gewei = star_num % 10;
                    var star = shiwei.ToString() + "星";
                    if (gewei == 5)
                    {
                        star += "半";
                    }
                    return star;
                }
                return "0星"; //如果小于10
            }
            return null;
        }
    }
}