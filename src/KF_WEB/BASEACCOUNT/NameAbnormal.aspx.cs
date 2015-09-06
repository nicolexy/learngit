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

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
    /// Summary description for BankBillNoQuery.
	/// </summary>
    public partial class NameAbnormal : System.Web.UI.Page
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
                this.tbx_cerDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");
            }
            this.btnDate.Attributes.Add("onclick", "openModeDate()"); 
		}


		protected void btn_query_Click(object sender, System.EventArgs e)
        {
            if (tbx_acc.Text.Trim() == string.Empty)
            {
                ShowMsg("�������û��˺ţ�");
                return;
            }
           
            ViewState["uin"] = tbx_acc.Text.Trim();
			BindData();
		}
		
		public void BindData()
		{
            try
            {
                DataSet ds = new DataSet();
                ds = new AuthenInfoService().QueryRealNameInfo(ViewState["uin"].ToString(),Session["uid"].ToString());//check_state=0 δ����״̬
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    this.ShowMsg("��ѯ��¼Ϊ��!");
                    return;
                }

                ds.Tables[0].Columns.Add("URL", typeof(string));
                //�鿴����������������¼
                ds.Tables[0].Rows[0]["URL"] = "NameAbnormalDetail.aspx?uin=" + ViewState["uin"].ToString() + "&cre_id_old=" + ds.Tables[0].Rows[0]["cre_id_tail"].ToString();
                this.DataGrid_QueryResult.DataSource = ds;
                this.DataGrid_QueryResult.DataBind();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ѯ�쳣��" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
		}


		private void ShowMsg(string msg)
		{
			Response.Write("<script language=javascript>alert('" + msg + "')</script>");
		}

        public void dg_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            object obj = e.Item.Cells[2].FindControl("CreateButton");
            string cre_id_old = e.Item.Cells[1].Text;
            if (obj != null)
            {
                Button lb = (Button)obj;

               // DataSet ds= acc.QueryNameAbnormalInfo(ViewState["uin"].ToString(), 0,cre_id_old, 0, 1);//check_state=0 δ����״̬
                DataSet ds = new AuthenInfoService().QueryNameAbnormalInfo(ViewState["uin"].ToString(), 0, cre_id_old, 0, 1);//check_state=0 δ����״̬

                if(!(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0))
                {
                        lb.Visible = true;
                }
            }
        }

        private void DataGrid_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            ViewState["name_old"]=e.Item.Cells[0].Text.Trim();
            ViewState["certifyNo_old"] = e.Item.Cells[1].Text.Trim();
            try
            {
                if (e.CommandName == "CREATE")
                {
                    this.tb_nameOld.Text = ViewState["name_old"].ToString();
                    this.tb_certifyNoOld.Text = ViewState["certifyNo_old"].ToString();
                    tableCreate.Visible = true;
                    clear();
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "�������뵥ʧ�ܣ�" + eSys.Message);
            }
        }

        protected void btnSubmitApply_Click(object sender, System.EventArgs e)
        {
            try
            {
                Valid();

                string alPathF="", alPathR="", alPathO="";
                HtmlInputFile inputFile = this.imageF;
                if (inputFile.Value == "")
                    throw new Exception("���ϴ����֤����ͼƬ");
                alPathF = PublicRes.upImage(inputFile, "Account");
                inputFile = this.imageR;
                if (inputFile.Value == "")
                    throw new Exception("���ϴ����֤����ͼƬ");
                alPathR = PublicRes.upImage(inputFile, "Account");
                inputFile = this.imageO;
                if (inputFile.Value != "")//�ɲ��ϴ�
                    alPathO = PublicRes.upImage(inputFile, "Account");

                NameAbnormalClass nameAbnormal = new NameAbnormalClass();
                nameAbnormal.Fuin = ViewState["uin"].ToString();
                nameAbnormal.Fname_old = this.tb_nameOld.Text.Trim();
                nameAbnormal.Fcre_id_old = this.tb_certifyNoOld.Text.Trim();
                nameAbnormal.Ftruename = this.tb_name.Text.Trim();
                nameAbnormal.Fcre_id = this.tb_certifyNo.Text.Trim();
                nameAbnormal.Fcre_type = "1";
                nameAbnormal.Fcre_version = this.ddlCreVis.SelectedValue;
                nameAbnormal.Fcre_valid_day = DateTime.Parse(this.tbx_cerDate.Text.Trim()).ToString("yyyyMMdd");
                nameAbnormal.Faddress = this.tb_address.Text.Trim();
                nameAbnormal.Fimage_cre1 = alPathF;
                nameAbnormal.Fimage_cre2 = alPathR;
                nameAbnormal.Fimage_evidence = alPathO;
                nameAbnormal.Fsubmit_user = Session["uid"].ToString();
                nameAbnormal.Fsubmit_time = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                nameAbnormal.Fcheck_state = "0";

                new AuthenInfoService().AddNameAbnormalInfo(nameAbnormal);
                this.tableCreate.Visible = false;
                BindData();
                WebUtils.ShowMessage(this.Page, "�������뵥�ɹ���");
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "�������뵥�쳣��" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        private void Valid()
        {
            if (tb_name.Text.Trim() == string.Empty)
            {
                throw new Exception("������������");
            }
            if (tb_certifyNo.Text.Trim() == string.Empty)
            {
                throw new Exception("������֤���ţ�");
            }

            if (this.tbx_cerDate.Text.Trim() == string.Empty)
            {
                throw new Exception("���������ڣ�");
            }
            try
            {
                strBeginDate = DateTime.Parse(this.tbx_cerDate.Text).ToString("yyyyMMdd");
                //this.tbx_cerDate.Text = strBeginDate;
            }
            catch
            {
                throw new Exception("���ڸ�ʽ����ȷ��");
            }
        }

        private void clear()
        {
             this.tb_name.Text="";
             this.tb_certifyNo.Text="";
             this.ddlCreVis.SelectedValue="1";
             this.tb_address.Text="";
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
            this.DataGrid_QueryResult.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid_ItemCommand);
		}
		#endregion
	}
}
