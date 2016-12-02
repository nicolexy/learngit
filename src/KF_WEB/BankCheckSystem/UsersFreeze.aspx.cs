using CFT.CSOMS.BLL.BankCheckSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.BankCheckSystem
{
    public partial class WebForm1 : System.Web.UI.Page
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

                Aspnetpager1.RecordCount = 10000;
                Aspnetpager1.PageSize = 10;
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
            if (!IsPostBack)
            {
                Aspnetpager1.PageSize = 10;
                Aspnetpager1.RecordCount = 10000;
            }
        }

        protected void btnSerach_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txt_Fuser_login_account.Text.Trim()))
                {
                    WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode("请输入登录账号！"));
                    return;
                }
                Bind();
            }
            catch (LogicException ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.Message));
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.ToString()));
            }
        }
        public void Bind()
        {
            txt_Reason.Text = "";
            string userbindemail = txt_Fuser_login_account.Text.Trim();
            DataTable dt = new BankCheckSystemService().GetUserinfo2(userbindemail, "", "", "", 0, 1);
            if (dt != null && dt.Rows.Count > 0)
            {
                lblStatus.Text = dt.Rows[0]["Fuser_status_str"].ToString();
                string status = dt.Rows[0]["Fuser_status"].ToString();
                if (status == "0" || status == "1")
                {
                    btnFreeze.Visible = true;
                    btnUnFreeze.Visible = false;
                    btnToInvalid.Visible = true;
                }
                else if (status == "2")
                {
                    btnFreeze.Visible = false;
                    btnUnFreeze.Visible = true;
                    btnToInvalid.Visible = true;
                }
                else if (status == "3")
                {
                    btnFreeze.Visible = false;
                    btnUnFreeze.Visible = false;
                    btnToInvalid.Visible = false;
                }
            }
            else
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode("查询失败！"));
            }

            //绑定日志
            BindRecord(userbindemail, 1);
        }

        private void BindRecord(string userbindemail, int pageindex)
        {
            try
            {
                int limit = Aspnetpager1.PageSize;
                int offset = (pageindex - 1) * limit;

                DataTable dtRecord = new BankCheckSystemService().getUserRecords(userbindemail, offset, limit);
                DataGrid1.DataSource = dtRecord;
                DataGrid1.DataBind();
            }
            catch (LogicException ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.Message));
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.ToString()));
            }
        }

        protected void btnUnFreeze_Click(object sender, EventArgs e)
        {
            try
            {
                string userbindemail = txt_Fuser_login_account.Text.Trim();
                string reason = txt_Reason.Text.Trim();
            
                if ((!new BankCheckSystemService().EditUserStatus(userbindemail, ((int)UserStatus.IsNormal).ToString())))
                {
                    WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode("解冻失败"));
                    return;
                }

                if (!new BankCheckSystemService().InsertRecords(userbindemail, ((int)OperationType.UnFreeze).ToString(), reason, Session["uid"].ToString()))
                {
                    WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode("插入操作记录失败"));
                }
                Bind();
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode("解冻成功"));
            }
            catch (LogicException ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.Message));
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.ToString()));
            }
        }
        protected void btnFreeze_Click(object sender, EventArgs e)
        {

            try
            {
                string userbindemail = txt_Fuser_login_account.Text.Trim();
                string reason = txt_Reason.Text.Trim();
                if (string.IsNullOrEmpty(reason))
                {
                    WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode("请输入冻结原因！"));
                    return;
                }
                string uid = Session["uid"].ToString();
                if ((!new BankCheckSystemService().EditUserStatus(userbindemail, ((int)UserStatus.IsFreeze).ToString())))
                {
                    WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode("冻结失败"));
                    return;
                }
                if (!new BankCheckSystemService().InsertRecords(userbindemail, ((int)OperationType.Freeze).ToString(), reason, uid))
                {
                    WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode("插入操作记录失败"));
                }
                Bind();
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode("冻结成功"));
            }
            catch (LogicException ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.Message));
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.ToString()));
            }
        }

        protected void btnToInvalid_Click(object sender, EventArgs e)
        {
            try
            {
                string userbindemail = txt_Fuser_login_account.Text.Trim();
                string reason = txt_Reason.Text.Trim();
                string uid = Session["uid"].ToString();

                if ((!new BankCheckSystemService().EditUserStatus(userbindemail, ((int)UserStatus.IsInvalid).ToString())))
                {
                    WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode("作废失败"));
                    return;
                }
                if (!new BankCheckSystemService().InsertRecords(userbindemail, ((int)OperationType.ToInvalid).ToString(), reason, uid))
                {
                    WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode("插入操作记录失败"));
                }
                Bind();
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode("作废成功"));
            }
            catch (LogicException ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.Message));
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.ToString()));
            }

        }


        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                Aspnetpager1.CurrentPageIndex = e.NewPageIndex;
                string userbindemail = txt_Fuser_login_account.Text.Trim();
                BindRecord(userbindemail, Aspnetpager1.CurrentPageIndex);
            }
            catch (LogicException ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.Message));
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.ToString()));
            }
        }
    }
}