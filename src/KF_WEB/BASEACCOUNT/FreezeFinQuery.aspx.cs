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
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// FreezeFinQuery ��ժҪ˵����
	/// </summary>
	public partial class FreezeFinQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{

		protected int ElemCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"].ToString());
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				this.lb_operatorID.Text = Session["uid"].ToString(); 

				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());
				
				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "FreezeList")) Response.Redirect("../login.aspx?wh=1");
                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				this.tbx_beginDate.Value = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
                this.tbx_endDate.Value = DateTime.Now.ToString("yyyy-MM-dd");

				this.tb_detail.Visible = false;
			}

			this.DataGrid_QueryResult.ItemCommand += new DataGridCommandEventHandler(DataGrid_QueryResult_ItemCommand);

			// �ڴ˴������û������Գ�ʼ��ҳ��
		}

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);
		}
		#endregion

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
            try
            {
                ValidateDate();
            }
            catch (Exception err)
            {
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }

            try
            {
                pager.RecordCount = 1000;
                BindData(1);
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
		}

        private void ValidateDate()
        {
            DateTime beginDate;
            DateTime endDate;
            string strBeginDate;
            string strEndDate;

            try
            {
                beginDate = DateTime.Parse(this.tbx_beginDate.Value);
                endDate = DateTime.Parse(this.tbx_endDate.Value);

                if (beginDate.CompareTo(endDate) > 0)
                {
                    this.ShowMsg("��ʼ���ڲ��ܴ��ڽ������ڣ����������롣");
                    return;
                }

                strBeginDate = beginDate.ToString("yyyy-MM-dd");
                strEndDate = endDate.ToString("yyyy-MM-dd");
            }
            catch
            {
                this.ShowMsg("��������ڸ�ʽ�������������롣");
                return;
            }

            ViewState["strBeginDate"] = strBeginDate;
            ViewState["strEndDate"] = strEndDate;
        }

        private void BindData(int index)
        {
            pager.CurrentPageIndex = index;
            int max = pager.PageSize;
            int start = max * (index - 1);
            Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

            qs.Credentials = System.Net.CredentialCache.DefaultCredentials;

            double fin = 0;
            try
            {
                fin = double.Parse(this.tbx_fin.Text);
            }
            catch
            {
                fin = 0;
            }

            //qs.Finance_HeaderValue = setConfig.setFH(Session["OperID"].ToString(),Request.UserHostAddress);
            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
            DataSet ds = qs.QueryUserFreezeRecord(ViewState["strBeginDate"].ToString(), ViewState["strEndDate"].ToString(), this.tbx_payAccount.Text, fin, "", start, max);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                this.ShowMsg("��ѯ���Ϊ��");
                this.DataGrid_QueryResult.DataSource = null;
                this.DataGrid_QueryResult.DataBind();
                return;
            }

            this.DataGrid_QueryResult.DataSource = ds;

            this.DataGrid_QueryResult.DataBind();
        }

		private void DataGrid_QueryResult_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			string strFlistID = e.Item.Cells[6].Text.Trim();

			//qs.Finance_HeaderValue = setConfig.setFH(Session["OperID"].ToString(),Request.UserHostAddress);
			qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
			DataSet ds = qs.QueryUserFreezeRecord("","",this.tbx_payAccount.Text,0,strFlistID,0,1);

			this.lb_c1.Text = ds.Tables[0].Rows[0]["Flistid"].ToString();
            //this.lb_c2.Text = ds.Tables[0].Rows[0]["Fbkid"].ToString();
			this.lb_c3.Text = ds.Tables[0].Rows[0]["Ftrue_name"].ToString();
			this.lb_c4.Text = ds.Tables[0].Rows[0]["strFreason"].ToString();
			this.lb_c5.Text = ds.Tables[0].Rows[0]["Fpaynum"].ToString();
			this.lb_c6.Text = ds.Tables[0].Rows[0]["Fconnum"].ToString();
			this.lb_c7.Text = ds.Tables[0].Rows[0]["Fbalance"].ToString();
			this.lb_c8.Text = ds.Tables[0].Rows[0]["Fcon"].ToString();
			this.lb_c9.Text = ds.Tables[0].Rows[0]["strFcurtype"].ToString();
			this.lb_c10.Text = ds.Tables[0].Rows[0]["strBankName"].ToString();
			this.lb_c11.Text = ds.Tables[0].Rows[0]["Fmodify_time"].ToString();
			this.lb_c12.Text = ds.Tables[0].Rows[0]["Fmemo"].ToString();

			this.lb_c19.Text = ds.Tables[0].Rows[0]["Fqqid"].ToString();

			lb_c13.Text = ds.Tables[0].Rows[0]["Fapplyid"].ToString();

			this.tb_detail.Visible = true;
		}

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }

		private void ShowMsg(string msg)
		{
			Response.Write("<script language=javascript>alert('" + msg + "')</script>");
		}


	}
}
