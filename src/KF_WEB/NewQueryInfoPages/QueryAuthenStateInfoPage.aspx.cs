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

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
    /// Summary description for BankBillNoQuery.
	/// </summary>
    public partial class QueryAuthenStateInfoPage : System.Web.UI.Page
	{
        AccountService acc = new AccountService();
        AuthenStateDeleteClass authenInfo = new AuthenStateDeleteClass();
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
            }
		}


		protected void btn_query_Click(object sender, System.EventArgs e)
        {
            if (tb_acc.Text.Trim() == string.Empty)
            {
                ShowMsg("请输入证件号！");
                return;
            }

            
            ViewState["uin"] = tb_acc.Text.Trim();
			BindData();
		}
		
		public void BindData()
		{
            try
            {
                DataSet ds = new DataSet();
                ds=QueryUserAuthenLog();
                QueryUserAuthen(ds);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, eSys.Message);
                return;
            }
		}

        private void QueryUserAuthen(DataSet ds)
        {
            try
            {
                //查询实名认证信息
                clearCreate();
                ds = acc.QueryUserAuthenByCredid("1", ViewState["uin"].ToString(), Session["uid"].ToString());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    this.tb_cre_id.Text = ViewState["uin"].ToString();
                    this.tb_uid.Text = ds.Tables[0].Rows[0]["cuid"].ToString();
                    this.tb_name_old.Text = ds.Tables[0].Rows[0]["cname"].ToString();
                }
                tableCreate.Visible = true;
                tableDetail.Visible = false;
            }
            catch (Exception eSys)
            {
                throw new Exception("查询实名认证信息异常！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        private DataSet QueryUserAuthenLog()
        {
            try
            {
                //查询操作日志
                DataSet ds = acc.QueryUserAuthenDisableLog(ViewState["uin"].ToString());
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    this.DataGrid_QueryResult.Visible = false;
                }
                this.DataGrid_QueryResult.Visible = true;
                this.DataGrid_QueryResult.DataSource = ds;
                this.DataGrid_QueryResult.DataBind();
                return ds;
            }
            catch (Exception eSys)
            {
                throw new Exception("查询日志异常！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

		private void ShowMsg(string msg)
		{
			Response.Write("<script language=javascript>alert('" + msg + "')</script>");
		}

        public void btnSubmit_Click(object sender, System.EventArgs e)
        {
            try
            {
                string alPathF="", alPathR="", alPathO="";
                HtmlInputFile inputFile = this.imageF;
                if (inputFile.Value == "")
                    throw new Exception("请上传身份证正面图片");
                alPathF = PublicRes.upImage(inputFile, "Account");
                inputFile = this.imageR;
                if (inputFile.Value == "")
                    throw new Exception("请上传身份证反面图片");
                alPathR = PublicRes.upImage(inputFile, "Account");
                inputFile = this.imageO;
                if (inputFile.Value == "")
                    throw new Exception("请上传改名凭证");
                alPathO = PublicRes.upImage(inputFile, "Account");

               
                authenInfo.Fuid = this.tb_uid.Text.Trim();
                authenInfo.Fcre_id = this.tb_cre_id.Text.Trim();
                authenInfo.Fcre_type = "1";
                authenInfo.Fname_old = this.tb_name_old.Text.Trim();
                authenInfo.Fimage_cre1 = alPathF;
                authenInfo.Fimage_cre2 = alPathR;
                authenInfo.Fimage_evidence = alPathO;
                authenInfo.Fsubmit_user = Session["uid"].ToString();
                authenInfo.Fsubmit_time = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ViewState["Fimage_cre1"] = alPathF;
                ViewState["Fimage_cre2"] = alPathR;
                ViewState["Fimage_evidence"] = alPathO;

                this.tableDetail.Visible = true;
                this.ButtonOK.Visible = true;
                showDetail(authenInfo);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "数据异常！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        protected void btnOK_Click(object sender, System.EventArgs e)
        {
            try
            {
                string objid = System.DateTime.Now.ToString("yyyyMMddHHmmss")+ PublicRes.StaticNoManage();
                string[,] param = new string[,] {  { "Fuid", this.tb_uid.Text.Trim() },
                                { "Fcre_id", this.tb_cre_id.Text.Trim() },
                                { "Fcre_type", "1" },
                                { "Fname_old", this.tb_name_old.Text.Trim() },
                                { "Fimage_cre1", ViewState["Fimage_cre1"].ToString() },
                                { "Fimage_cre2",  ViewState["Fimage_cre2"].ToString() },
                                { "Fimage_evidence",ViewState["Fimage_evidence"].ToString() },
                                { "Fsubmit_user",Session["uid"].ToString() },
                                { "Fsubmit_time",System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                              };

                TENCENT.OSS.C2C.Finance.BankLib.Param[] pa = PublicRes.ToParamArrayStruct(param);

                if (acc.DisableUserAuthenInfo("1", ViewState["uin"].ToString(), Session["uid"].ToString(), "kf", objid, "UserAuthenDisableLog", "cre_id", pa))
                    WebUtils.ShowMessage(this.Page, "实名认证置失效成功！");
                tableDetail.Visible = false;
                QueryUserAuthenLog();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "实名认证置失效异常！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }


        private void showDetail(AuthenStateDeleteClass authenInfo)
        {
            clearDetail();
            this.lb_cre_id.Text = authenInfo.Fcre_id;
            this.lb_uin.Text = authenInfo.Fuid;
            this.lb_name_old.Text = authenInfo.Fname_old;

            string requestUrl = System.Configuration.ConfigurationManager.AppSettings["GetImageFromKf2Url"].ToString();
            string strLocFile = System.Configuration.ConfigurationManager.AppSettings["KFWebSrc"].ToString();
            string strLocImage_cre1 = strLocFile + authenInfo.Fimage_cre1;
            string strLocImage_cre2 = strLocFile + authenInfo.Fimage_cre2;
            string strLocImage_evidence = strLocFile + authenInfo.Fimage_evidence;

            Image1.ImageUrl = File.Exists(strLocImage_cre1.Trim()) ? "../" + authenInfo.Fimage_cre1 : requestUrl + "/" + authenInfo.Fimage_cre1;
            Image2.ImageUrl = File.Exists(strLocImage_cre2.Trim()) ? "../" + authenInfo.Fimage_cre2 : requestUrl + "/" + authenInfo.Fimage_cre2;
            Image3.ImageUrl = File.Exists(strLocImage_evidence.Trim()) ? "../" + authenInfo.Fimage_evidence : requestUrl + "/" + authenInfo.Fimage_evidence;

        }

        private void dgList_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                try
                {
                    AuthenStateDeleteClass authenInfo = new AuthenStateDeleteClass();
                    authenInfo.Fuid = e.Item.Cells[5].Text;
                    authenInfo.Fcre_id = e.Item.Cells[3].Text;
                    authenInfo.Fname_old = e.Item.Cells[4].Text;
                    authenInfo.Fimage_cre1 = e.Item.Cells[0].Text;
                    authenInfo.Fimage_cre2 = e.Item.Cells[1].Text;
                    authenInfo.Fimage_evidence = e.Item.Cells[2].Text;
                    this.tableDetail.Visible = true;
                    this.ButtonOK.Visible = false;
                    showDetail(authenInfo);
                }
                catch (Exception ex)
                {
                    //this.divInfo.Visible = false;
                    WebUtils.ShowMessage(this.Page, "读取数据失败！" + ex.Message);
                }
            }
        }

        private void clearCreate()
        {
            this.tb_cre_id.Text = "";
            this.tb_uid.Text = "";
            this.tb_name_old.Text = "";
        }
        private void clearDetail()
        {
            this.lb_cre_id.Text = "";
            this.lb_uin.Text = "";
            this.lb_name_old.Text = "";
            this.Image1.ImageUrl = "";
            this.Image2.ImageUrl = "";
            this.Image3.ImageUrl = "";
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
            this.DataGrid_QueryResult.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgList_ItemCommand);
		}
		#endregion
	}
}
