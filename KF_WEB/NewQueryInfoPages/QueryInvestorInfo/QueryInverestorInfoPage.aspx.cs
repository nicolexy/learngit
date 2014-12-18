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

namespace TENCENT.OSS.CFT.KF.KF_Web.QueryInvestorInfo
{
	/// <summary>
	/// QueryInverestorInfoPage 的摘要说明。
	/// </summary>
	public class QueryInverestorInfoPage : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.RadioButton rtnList;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.TextBox txtFlistid;
		protected System.Web.UI.WebControls.RadioButton rtbSpid;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.TextBox TextBoxBeginDate;
		protected System.Web.UI.WebControls.ImageButton ButtonBeginDate;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.TextBox txtFspid;
		protected System.Web.UI.WebControls.Button btnQuery;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.TextBox Textbox1;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.TextBox Textbox2;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.TextBox Textbox3;
		protected System.Web.UI.WebControls.DataGrid DataGrid1;

		protected Wuqi.Webdiyer.AspNetPager pager;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			string strRepay = "<script language=javascript>";

			Query_Service.Query_Service query = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			//DataSet queryResult = query.GetUserChargeInfo(Session["QQID"].ToString(),DateTime.Now,1,10);

			// 测试使用的QQID
			Session["QQID"] = "359050096";

			/*
			DataSet queryResult = query.GetUserChargeInfo("359050096",DateTime.Now,1,10);

			*/

			DataSet queryResult = query.GetUserUndoInfo(Session["QQID"].ToString(),DateTime.Now,1,10);

			Response.Write("<script language=javascript>alert('ok');</script>");

			if(queryResult == null || queryResult.Tables.Count == 0)
			{
				strRepay += "alert('No Tables');";
			}
			else 
			{
				if(queryResult.Tables[0].Columns.Count == 0)
				{
					//this.tbx_result.Text = "No Columns";

					strRepay += "alert('No Columns');";

					return;
				}

				if(queryResult.Tables[0].Rows.Count == 0)
				{
					//this.tbx_result.Text = "No Rows";

					strRepay += "alert('No Rows');";

					return;
				}

				for(int i=0;i<queryResult.Tables[0].Rows.Count;i++)
				{
					strRepay += "alert(" + queryResult.Tables[0].Rows.Count + "');";
				}
				
			}

			this.DataGrid1.DataSource = queryResult;
			this.DataGrid1.DataBind();

			//this.tbx_result.Text = "Yes";

			strRepay += "</script>";

			Response.Write(strRepay);
			
			
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
			this.Load += new EventHandler(Page_Load);
		}
		#endregion



		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			
		}
	}
}
