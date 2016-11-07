using CFT.CSOMS.BLL.BankCheckSystem;
using CFT.CSOMS.BLL.WechatPay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.BankCheckSystem
{
    public partial class PasswordManage : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());

                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("BankCheck", this))
                    Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
        }
        private DataTable GetBankType(string binkidField, string bankNameField, DataTable dt)
        {
            Hashtable ht = new FastPayService().RequestBankInfo();
            if (dt == null) return null;
            dt.Columns.Add(bankNameField, typeof(string));
            foreach (DataRow dr in dt.Rows)
            {
                string binkid = dr[binkidField].ToString().Trim();
                //binkid = binkid.Split('|')[0];
                if (ht.ContainsValue(binkid))
                {
                    dr[bankNameField] = new Func<string>(() =>
                    {
                        foreach (var item in ht.Keys)
                        {
                            if (ht[item].ToString() == binkid)
                            {
                                return item.ToString();
                            }
                        }
                        return "未知：" + binkid;
                    })();
                }
                else
                {
                    dr[bankNameField] = "未知：" + binkid;
                }
            }
            return dt;
        }

        protected void btncheck_Click(object sender, EventArgs e)
        {
            try
            {
                if (new BankCheckSystemService().checkUser(txt_Fuser_login_account.Text.Trim(), txt_Fuser_id_no.Text.Trim()))
                {
                    WebUtils.ShowMessage(this.Page, "身份证验证成功！");
                    //btncheck.Enabled = false;
                    btnSerach.Visible = true;
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "身份证验证失败！");
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.ToString()));
            }
        }

        protected void btnSerach_Click(object sender, EventArgs e)
        {
            try
            {
                if (!new BankCheckSystemService().checkUser(txt_Fuser_login_account.Text.Trim(), txt_Fuser_id_no.Text.Trim()))
                {
                    WebUtils.ShowMessage(this.Page, "身份证验证失败！");
                    return;
                }

                DataTable dt = new BankCheckSystemService().getUserinfo(txt_Fuser_login_account.Text.Trim());
                dt = GetBankType("Fbank_id", "Fbank_id_str", dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    txt_email.Text = txt_Fuser_login_account.Text.Trim();
                    lbl_Fbank_id.Text = dt.Rows[0]["Fbank_id_str"].ToString();
                    lbl_Fcontact_name.Text = dt.Rows[0]["Fcontact_name"].ToString();
                    lbl_Flast_update_date.Text = dt.Rows[0]["Flast_update_date_login"].ToString();
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "查询失败！");
                }
                tb_detail.Visible = true;
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.ToString()));
            }

        }

        protected void lblreset_Click(object sender, EventArgs e)
        {
            try
            {
                string email = txt_email.Text.Trim();
                string newpassword = "";
                if (new BankCheckSystemService().ResetPwd(txt_Fuser_login_account.Text.Trim(),out newpassword))
                {
                    WebUtils.ShowMessage(this.Page, "重置成功！");
                    string str_params = "username={0}&initial={1}";
                    str_params = string.Format(str_params, lbl_Fcontact_name.Text.Trim(), newpassword);
                    TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(email, "2591", str_params);

                    if (!new BankCheckSystemService().InsertRecords(txt_Fuser_login_account.Text.Trim(), ((int)OperationType.重置密码).ToString(), "", Session["uid"].ToString()))
                    {
                        WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode("插入操作记录失败"));
                    }
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "重置失败！");
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.ToString()));
            }

        }
    }
}