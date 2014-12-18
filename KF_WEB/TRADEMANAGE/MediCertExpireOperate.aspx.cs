using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Xml;
using CFT.CSOMS.BLL.ForeignCurrencyModule;
using TENCENT.OSS.CFT.KF.KF_Web.InternetBank;
using CFT.CSOMS.BLL.SysManageModule;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using System.Text;
using CFT.CSOMS.BLL.SPOA;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
    /// QueryYTTrade 的摘要说明。
	/// </summary>
    public partial class MediCertExpireOperate : System.Web.UI.Page
	{
        protected MerchantService merService = new MerchantService();
        protected void Page_Load(object sender, System.EventArgs e)
		{

			try
			{
                if (!IsPostBack)
                {
                    if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
                    
                    if (!string.IsNullOrEmpty(Request.QueryString["spid"]))
                    {
                        string spid = Request.QueryString["spid"].Trim();
                        ViewState["spid"] = spid;
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["crt_etime"]))
                    {
                        string crt_etime = Request.QueryString["crt_etime"].Trim();
                        ViewState["crt_etime"] = crt_etime;
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["memo"]))
                    {
                        string memo = Request.QueryString["memo"].Trim();
                        ViewState["memo"] = memo;
                    }
                    else
                        ViewState["memo"] = "";
                    this.tbSpid.Text = ViewState["spid"].ToString();
                    this.tbmemo.Text = ViewState["memo"].ToString();
                }
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}
            
		}

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion


        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                string spid = ViewState["spid"].ToString();
                string crt_etime = ViewState["crt_etime"].ToString();
                if (string.IsNullOrEmpty(spid) || string.IsNullOrEmpty(crt_etime))
                    throw new Exception("spid或者crt_etime为空!");
                string updateUser = Session["uid"].ToString();
                string memo = this.tbmemo.Text.Trim();
                merService.EditExpiredCertMemo(spid,memo,crt_etime,updateUser);
                WebUtils.ShowMessage(this.Page, "修改备注成功！");
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "修改备注失败！" + PublicRes.GetErrorMsg(eSys.Message));
            }
        }


	}
}