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
using System.Web.Mail;
using System.IO;
using System.Xml;
using CFT.CSOMS.BLL.SPOA;


namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// ModifyBusinessInfo 的摘要说明。
	/// </summary>
    public partial class ModifyContactInfo : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

                if (!classLibrary.ClassLib.ValidateRight("SPInfoManagement", this)) Response.Redirect("../login.aspx?wh=1");

				if(!IsPostBack)
				{
					//this.Table1.Visible = false;
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

		

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
            string spid = this.txtSpid.Text;

            if (string.IsNullOrEmpty(spid.Trim()))
            {
                WebUtils.ShowMessage(this.Page, "请输入商户号！");
                return;
            }
            ViewState["spid"] = spid.Trim();
            BindData();
		}

		private void BindData()
		{
            try
            {
              
                MerchantService mer = new MerchantService();
                DataSet ds = mer.QuerySPContactInfo(ViewState["spid"].ToString());
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    WebUtils.ShowMessage(this.Page, "没有查询到记录！");
                    return;
                }
                DataRow dr=ds.Tables[0].Rows[0];
                clear();

                this.name1.Text = dr["name1"].ToString();
                this.standbya1.Text = dr["standbya1"].ToString();
                this.tele1.Text = dr["tele1"].ToString();
                ViewState["email1"] = dr["email1"].ToString();
            //    this.mobile1.Text = dr["mobile1"].ToString();

                this.name2.Text = dr["name2"].ToString();
                this.tele2.Text = dr["tele2"].ToString();
                this.qqnum2.Text = dr["qqnum2"].ToString();
                this.email2.Text = dr["email2"].ToString();

                this.name3.Text = dr["name3"].ToString();
                this.tele3.Text = dr["tele3"].ToString();
                this.qqnum3.Text = dr["qqnum3"].ToString();
                this.email3.Text = dr["email3"].ToString();

                this.name4.Text = dr["name4"].ToString();
                this.tele4.Text = dr["tele4"].ToString();
                this.qqnum4.Text = dr["qqnum4"].ToString();
                this.email4.Text = dr["email4"].ToString();

                this.name5.Text = dr["name5"].ToString();
                this.tele5.Text = dr["tele5"].ToString();
                this.qqnum5.Text = dr["qqnum5"].ToString();
                this.email5.Text = dr["email5"].ToString();

                this.name6.Text = dr["name6"].ToString();
                this.tele6.Text = dr["tele6"].ToString();
                this.qqnum6.Text = dr["qqnum6"].ToString();
                this.email6.Text = dr["email6"].ToString();

                this.name7.Text = dr["name7"].ToString();
                this.tele7.Text = dr["tele7"].ToString();
                this.qqnum7.Text = dr["qqnum7"].ToString();
                this.email7.Text = dr["email7"].ToString();


            }catch(Exception ex){

                WebUtils.ShowMessage(this.Page, "读取数据失败！" + ex.Message);
            }
			
		}



        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {

                SPContact spContact = new SPContact();
                spContact.spid = ViewState["spid"].ToString();
                spContact.name1 = this.name1.Text.Trim();
                spContact.standbya1 = this.standbya1.Text.Trim();
                spContact.tele1 = this.tele1.Text.Trim();
               // spContact.mobile1 = this.mobile1.Text.Trim();
                spContact.email1 = ViewState["email1"].ToString();

                spContact.name2 = this.name2.Text.Trim();
                spContact.tele2 = this.tele2.Text.Trim();
                spContact.qqnum2 = this.qqnum2.Text.Trim();
                spContact.email2 = this.email2.Text.Trim();

                spContact.name3 = this.name3.Text.Trim();
                spContact.tele3 = this.tele3.Text.Trim();
                spContact.qqnum3 = this.qqnum3.Text.Trim();
                spContact.email3 = this.email3.Text.Trim();

                spContact.name4 = this.name4.Text.Trim();
                spContact.tele4 = this.tele4.Text.Trim();
                spContact.qqnum4 = this.qqnum4.Text.Trim();
                spContact.email4 = this.email4.Text.Trim();

                spContact.name5 = this.name5.Text.Trim();
                spContact.tele5 = this.tele5.Text.Trim();
                spContact.qqnum5 = this.qqnum5.Text.Trim();
                spContact.email5 = this.email5.Text.Trim();

                spContact.name6 = this.name6.Text.Trim();
                spContact.tele6 = this.tele6.Text.Trim();
                spContact.qqnum6 = this.qqnum6.Text.Trim();
                spContact.email6 = this.email6.Text.Trim();

                spContact.name7 = this.name7.Text.Trim();
                spContact.tele7 = this.tele7.Text.Trim();
                spContact.qqnum7 = this.qqnum7.Text.Trim();
                spContact.email7 = this.email7.Text.Trim();

                MerchantService mer = new MerchantService();
                if (mer.InsertOrUpdateSPContactInfo(spContact, Session["uid"].ToString(), Request.UserHostAddress.ToString()))
                    WebUtils.ShowMessage(this.Page, "修改成功！");
                BindData();
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "修改失敗！" + ex.Message);
                return;
            }


        }

        private void clear()
        {
            this.name1.Text = "";
            this.standbya1.Text = "";
            this.tele1.Text = "";
            //this.mobile1.Text = "";

            this.name2.Text = "";
            this.tele2.Text = "";
            this.qqnum2.Text = "";
            this.email2.Text = "";

            this.name3.Text = "";
            this.tele3.Text = "";
            this.qqnum3.Text = "";
            this.email3.Text = "";

            this.name4.Text = "";
            this.tele4.Text = "";
            this.qqnum4.Text = "";
            this.email4.Text = "";

            this.name5.Text = "";
            this.tele5.Text = "";
            this.qqnum5.Text = "";
            this.email5.Text = "";

            this.name6.Text = "";
            this.tele6.Text = "";
            this.qqnum6.Text = "";
            this.email6.Text = "";

            this.name7.Text = "";
            this.tele7.Text = "";
            this.qqnum7.Text = "";
            this.email7.Text = "";
        }
		

	}
}
