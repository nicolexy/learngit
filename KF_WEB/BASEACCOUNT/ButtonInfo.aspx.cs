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
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;

using System.Reflection;
using Wuqi.Webdiyer;
using System.Configuration;

using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// ButtonInfo 的摘要说明。
	/// </summary>
	public partial class ButtonInfo : System.Web.UI.Page
	{

		System.Data.DataSet ds;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				BindData(0,1); 
			}
		}

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex=e.NewPageIndex;
			BindData(0,pager.CurrentPageIndex);
		}
		
		private void BindData(int i,int pageIndex)
		{	    
			if(Session["uid"] == null)
			{
				WebUtils.ShowMessage(this.Page,"超时，请重新登陆！");
				Response.Write("<script language=javascript>window.parent.location='../login.aspx?wh=1'</script>");
			}
			else
			{
				try
				{
					string selectStr = Session["QQID"].ToString();

					DateTime beginTime = DateTime.Parse(ConfigurationManager.AppSettings["sBeginTime"].ToString());
					DateTime endTime   = DateTime.Parse(ConfigurationManager.AppSettings["sEndTime"].ToString());

					int pageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"].ToString());

					int istr = 1 + pageSize * (pageIndex-1);   //1表示第一页
					int imax = pageSize;

					ds = classLibrary.setConfig.returnDataSet(selectStr,1,beginTime,endTime,0,"ButtonInfo",istr,imax,Session["uid"].ToString(),Request.UserHostAddress); 

					//查询交易单的纪录数
					int total;
					if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
						total = Int32.Parse(ds.Tables[0].Rows[0]["total"].ToString().Trim());
					else
						total = 0;
					pager.RecordCount= total;
					pager.PageSize   = pageSize;

					pager.CustomInfoText="记录总数：<font color=\"blue\"><b>"+pager.RecordCount.ToString()+"</b></font>";
					pager.CustomInfoText+=" 总页数：<font color=\"blue\"><b>"+pager.PageCount.ToString()+"</b></font>";
					pager.CustomInfoText+=" 当前页：<font color=\"red\"><b>"+pager.CurrentPageIndex.ToString()+"</b></font>";

					DataGrid2.DataSource = ds.Tables[0];
					DataGrid2.DataBind();
					if (ds!=null)
						ds.Dispose();
				}
				catch(SoapException eSoap) //捕获soap类
				{
					string str = PublicRes.GetErrorMsg(eSoap.Message.ToString());
					WebUtils.ShowMessage(this.Page,"查询错误，详细："+ str);
				}
				catch(Exception e)
				{
					Response.Write("<font color=red>查询错误，详细：" + e.Message + "</font>");
				}		
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
			this.DataGrid2.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid2_ItemDataBound);

		}
		#endregion

		private void DataGrid2_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if (e.Item.ItemIndex>=0)
			{
				HyperLink hl = (HyperLink)e.Item.Cells[2].Controls[0];
				if (hl.Text.Length>30)
					hl.Text = hl.Text.Substring(0,30)+"...";
				e.Item.Cells[3].Text = setConfig.FenToYuan(e.Item.Cells[3].Text);
				e.Item.Cells[4].Text = e.Item.Cells[4].Text=="0" ? "买家" : "卖家";
				e.Item.Cells[5].Text = e.Item.Cells[5].Text=="-1" ? "不支持" : setConfig.FenToYuan(e.Item.Cells[5].Text);
				e.Item.Cells[6].Text = e.Item.Cells[6].Text=="-1" ? "不支持" : setConfig.FenToYuan(e.Item.Cells[6].Text);
				string status = e.Item.Cells[11].Text;
				if (status=="0")
				{
					e.Item.Cells[11].Text = "无效";
					e.Item.ForeColor = System.Drawing.Color.Gray;
				}
				else if (status=="1")
					e.Item.Cells[11].Text = "有效";
				else if (status=="2")
					e.Item.Cells[11].Text = "冻结";
			}
		}
	}
}
