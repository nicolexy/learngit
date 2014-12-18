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
using CFT.CSOMS.BLL.WechatPay;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.CFTAccountModule;
using System.IO;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
    /// Summary description for BankBillNoQuery.
	/// </summary>
    public partial class NameAbnormalDetail : System.Web.UI.Page
	{
		
		string strBeginDate = "",strEndDate = "";
        AccountService acc = new AccountService();
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
                try
                {
                    if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
                }
                catch
                {
                    Response.Redirect("../login.aspx?wh=1");
                }

                if (Request.QueryString["uin"] != null && Request.QueryString["uin"].Trim() != "")
                {
                    ViewState["uin"] = Request.QueryString["uin"].Trim();
                }

                if (Request.QueryString["cre_id_old"] != null && Request.QueryString["cre_id_old"].Trim() != "")
                {
                    ViewState["cre_id_old"] = Request.QueryString["cre_id_old"].Trim();
                }

                BindData();
            }
		}

		public void BindData()
		{
            try
            {
                DataSet ds = acc.QueryNameAbnormalInfo(ViewState["uin"].ToString(), 99, ViewState["cre_id_old"].ToString(), 0, 1);//check_state=0 未处理状态

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    this.ShowMsg("查询记录为空!");
                    return;
                }

                ViewState["dt"] = ds.Tables[0];
                DataRow row = ds.Tables[0].Rows[0];
                this.tb_nameOld.Text = row["Fname_old"].ToString();
                this.tb_certifyNoOld.Text = row["Fcre_id_old"].ToString();
                this.tb_name.Text = row["Ftruename"].ToString();
                this.tb_certifyNo.Text = row["Fcre_id"].ToString();
                this.ddlRefuseReason.SelectedValue = row["Frefuse_reason"].ToString();
                this.tb_comment.Text = row["Fcomment"].ToString();
                this.tbx_cerDate.Text = row["Fcre_valid_day"].ToString();
                this.tb_address.Text = row["Faddress"].ToString();
                string tmd = row["Fcre_type"].ToString();
                if (tmd == "1")
                    this.tb_cre_type.Text = "身份证";
                tmd = row["Fcre_version"].ToString();
                switch (tmd)
                {
                    case "1": this.tb_cre_version.Text = "一代"; break;
                    case "2": this.tb_cre_version.Text = "二代"; break;
                    case "3": this.tb_cre_version.Text = "临时"; break;
                    default: this.tb_cre_version.Text = "未知" + tmd; break;
                }

                string requestUrl = System.Configuration.ConfigurationManager.AppSettings["GetImageFromKf2Url"].ToString();
                string strLocFile = System.Configuration.ConfigurationManager.AppSettings["KFWebSrc"].ToString();
                string strLocImage_cre1 = strLocFile + row["Fimage_cre1"].ToString();
                string strLocImage_cre2 = strLocFile + row["Fimage_cre2"].ToString();
                string strLocImage_evidence = strLocFile + row["Fimage_evidence"].ToString();

                ImageF.ImageUrl = File.Exists(strLocImage_cre1.Trim()) ? "../" + row["Fimage_cre1"].ToString() : requestUrl + "/" + row["Fimage_cre1"].ToString();
                ImageR.ImageUrl = File.Exists(strLocImage_cre2.Trim()) ? "../" + row["Fimage_cre2"].ToString() : requestUrl + "/" + row["Fimage_cre2"].ToString();
                ImageO.ImageUrl = File.Exists(strLocImage_evidence.Trim()) ? "../" + row["Fimage_evidence"].ToString() : requestUrl + "/" + row["Fimage_evidence"].ToString();
                if (row["Fcheck_state"].ToString() == "0")
                {
                    if (TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("NameAbnormalCheck", this))
                    {
                        this.ButtonOK.Visible = true;
                        this.ButtonNO.Visible = true;
                    }
                    else
                    {
                        WebUtils.ShowMessage(this.Page, "无审批权限！");
                    }
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "查询异常！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
		}

        protected void btnOK_Click(object sender, System.EventArgs e)
        {
            try
            {  
                
                NameAbnormalClass nameAbnormal = new NameAbnormalClass();
                DataTable dt= (DataTable)ViewState["dt"];

                nameAbnormal.Fuin = dt.Rows[0]["Fuin"].ToString();
                nameAbnormal.Fname_old = dt.Rows[0]["Fname_old"].ToString();
                nameAbnormal.Fcre_id_old = dt.Rows[0]["Fcre_id_old"].ToString();
                nameAbnormal.Ftruename = dt.Rows[0]["Ftruename"].ToString();
                nameAbnormal.Fcre_id = dt.Rows[0]["Fcre_id"].ToString();
                nameAbnormal.Fcre_type = dt.Rows[0]["Fcre_type"].ToString();
                nameAbnormal.Fcre_version = dt.Rows[0]["Fcre_version"].ToString();
                nameAbnormal.Fcre_valid_day = dt.Rows[0]["Fcre_valid_day"].ToString();
                nameAbnormal.Faddress = dt.Rows[0]["Faddress"].ToString();
                nameAbnormal.Fimage_cre1 = dt.Rows[0]["Fimage_cre1"].ToString();
                nameAbnormal.Fimage_cre2 = dt.Rows[0]["Fimage_cre2"].ToString();
                nameAbnormal.Fcheck_user = Session["uid"].ToString();
                nameAbnormal.Fcheck_time = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                nameAbnormal.Fcheck_state = "1";
                nameAbnormal.Frefuse_reason = this.ddlRefuseReason.SelectedValue;
                nameAbnormal.Fcomment = dt.Rows[0]["Fcomment"].ToString();

                if (acc.UpdateRealNameInfo(nameAbnormal))
                    WebUtils.ShowMessage(this.Page, "通过成功！");
                else
                    WebUtils.ShowMessage(this.Page, "通过失败！");

                    this.ButtonOK.Visible = false;
                    this.ButtonNO.Visible = false;
                    return;

            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "审批通过异常！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        protected void btnNO_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (acc.UpdateNameAbnormalInfo(ViewState["uin"].ToString(), this.ddlRefuseReason.SelectedValue, this.tb_comment.Text, Session["uid"].ToString(), "2"))
                    WebUtils.ShowMessage(this.Page, "拒绝成功！");
                else
                    WebUtils.ShowMessage(this.Page, "拒绝失败！");
                this.ButtonOK.Visible = false;
                this.ButtonNO.Visible = false;
                return;
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "审批拒绝异常！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        private void ShowMsg(string msg)
        {
            Response.Write("<script language=javascript>alert('" + msg + "')</script>");
        }
        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
        {
         //   this.DataGrid_QueryResult.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid_ItemCommand);
		}
		#endregion
	}
}
