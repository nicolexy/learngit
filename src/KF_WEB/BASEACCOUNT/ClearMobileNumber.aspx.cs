using CFT.CSOMS.BLL.CFTAccountModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.C2C.Finance.BankLib;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    public partial class ClearMobileNumber : System.Web.UI.Page
    {
        string uid;
        protected void Page_Load(object sender, EventArgs e)
        {
            uid = Session["uid"] as string;
            if (!IsPostBack)
            {
                var ClearMobileNumber = classLibrary.ClassLib.ValidateRight("ClearMobileNumber", this);
                if (!(ClearMobileNumber || ClassLib.ValidateRight("InfoCenter", this)))
                    Response.Redirect("../login.aspx?wh=1");
                ViewState["ClearMobileNumber"] = ClearMobileNumber;
                Button1.Visible = ClearMobileNumber;
                Label7.Text = uid;
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            BindCount.Text = null;
            DataGrid1.DataSource = null;
            var mobile = txt_mobile.Text.Trim();
            try
            {
                if (!string.IsNullOrEmpty(mobile))
                {
                    var bll = new AccountService();
                    var dic = bll.QueryMobileBoundNumber(mobile);
                    BindCount.Text = dic["value"];
                    var ds = bll.QueryClearMobileNumberLog(mobile);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        var dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            DataGrid1.DataSource = dt;
                        }
                    }
                }
                else
                {
                    mobile = null;
                    WebUtils.ShowMessage(this.Page, "请输入正确的手机号码");
                }
            }
            catch (Exception eSoap)
            {
                mobile = null;
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, errStr);
            }
            ViewState["curMobile"] = mobile;
            DataGrid1.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                var clearMobileNumber = Convert.ToBoolean(ViewState["ClearMobileNumber"]);
                if (clearMobileNumber)
                {
                    var count = BindCount.Text;
                    var mobile = (ViewState["curMobile"] as string);//?? txt_mobile.Text.Trim(); 
                    if (string.IsNullOrEmpty(count) || string.IsNullOrEmpty(mobile))
                    {
                        WebUtils.ShowMessage(this.Page, "请输入手机号码");
                        return;
                    }
                    if (count == "0")
                    {
                        WebUtils.ShowMessage(this.Page, "当前手机号码不需要清零");
                        return;
                    }
                    var bll = new AccountService();
                    Param[] param = {
                                    new Param(){ ParamName = "Fsubmit_user" , ParamValue= uid },
                                   //    new Param(){ ParamName = "FUser_type" , ParamValue= "1" },
                                    new Param(){ ParamName = "FCreate_time" , ParamValue= DateTime.Now.ToString() }, 
                                    new Param(){ ParamName = "FMobile" , ParamValue= mobile }, 
                                    new Param(){ ParamName = "MobileBindCount_Old" , ParamValue= count }, 
                                };
                    string objid = System.DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.StaticNoManage();
                    var bol = bll.ClearMobileBoundNumber(mobile, "mobile", mobile, uid, objid, param);
                    if (bol)
                    {
                        WebUtils.ShowMessage(this.Page, mobile + "清零成功");
                        ViewState["curMobile"] = null;
                        BindCount.Text = "0";
                    }
                }
            }
            catch (Exception eSoap)
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, errStr);
            }
        }
    }
}
