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

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// FreezeFinQuery 的摘要说明。
	/// </summary>
	public partial class FreezeFinQuery : System.Web.UI.Page
	{

		protected int ElemCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"].ToString());
	
		protected void Page_Load(object sender, System.EventArgs e)
		{

			this.ButtonBeginDate.Attributes.Add("onclick","openModeBegin()");
			this.ButtonEndDate.Attributes.Add("onclick","openModeEnd()");

			try
			{
				this.lb_operatorID.Text = Session["uid"].ToString(); 

				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());
				
				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "FreezeList")) Response.Redirect("../login.aspx?wh=1");
				if(!classLibrary.ClassLib.ValidateRight("FreezeList",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				this.tbx_beginDate.Text = DateTime.Now.AddYears(-1).ToString("yyyy年MM月dd日");
				this.tbx_endDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");

				this.tb_detail.Visible = false;
			}

			this.DataGrid_QueryResult.ItemCommand += new DataGridCommandEventHandler(DataGrid_QueryResult_ItemCommand);

			// 在此处放置用户代码以初始化页面
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

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			qs.Credentials = System.Net.CredentialCache.DefaultCredentials;

			DateTime beginDate;
			DateTime endDate;
			string strBeginDate;
			string strEndDate;

			try
			{
				beginDate = DateTime.Parse(this.tbx_beginDate.Text);
				endDate = DateTime.Parse(this.tbx_endDate.Text);

				if(beginDate.CompareTo(endDate) > 0)
				{
					this.ShowMsg("开始日期不能大于结束日期，请重新输入。");
					return;
				}

				strBeginDate = beginDate.ToString("yyyy-MM-dd");
				strEndDate = endDate.ToString("yyyy-MM-dd");
			}
			catch
			{
				this.ShowMsg("输入的日期格式有误！请重新输入。");
				return;
			}

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
			DataSet ds = qs.QueryUserFreezeRecord(strBeginDate,strEndDate,this.tbx_payAccount.Text,fin,"",(this.pager.CurrentPageIndex - 1) * ElemCount,ElemCount);

			if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
			{
				this.ShowMsg("查询结果为空");
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

			string strFlistID = e.Item.Cells[5].Text.Trim();

			//qs.Finance_HeaderValue = setConfig.setFH(Session["OperID"].ToString(),Request.UserHostAddress);
			qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
			DataSet ds = qs.QueryUserFreezeRecord("","",this.tbx_payAccount.Text,0,strFlistID,0,1);

			this.lb_c1.Text = ds.Tables[0].Rows[0]["Flistid"].ToString();
			this.lb_c2.Text = ds.Tables[0].Rows[0]["Fbkid"].ToString();
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


		private void ShowMsg(string msg)
		{
			Response.Write("<script language=javascript>alert('" + msg + "')</script>");
		}


	}
}
