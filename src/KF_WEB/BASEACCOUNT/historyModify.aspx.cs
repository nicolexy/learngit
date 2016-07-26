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
using TENCENT.OSS.CFT.KF.Common;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// historyModify 的摘要说明。
	/// </summary>
	public partial class historyModify : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		public    string  Msg;
		int       istr=0;
		int       pageSize = 20;
		public    DataSet ds;   //数据源DataSet
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			/*
			if (Session["uid"] == null || !AllUserRight.ValidRight(Session["szkey"].ToString(),Int32.Parse(Session["OperID"].ToString()),
				PublicRes.GROUPID, "HistoryModify")) 			Response.Redirect("../login.aspx?wh=1");
				*/

			if(Session["uid"] == null || !classLibrary.ClassLib.ValidateRight("HistoryModify",this)) Response.Redirect("../login.aspx?wh=1");

			pageSize = Int32.Parse(ddlPageSize.SelectedValue);

			if (!Page.IsPostBack)
			{
				this.TextBoxEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");

				if (!bindaData())
				{
					return;
				}
			}
	
			//Page.DataBind();
		}

		private bool bindaData()
		{

			//构造查询语句
			DateTime bgDate = DateTime.Now;
			DateTime edDate = DateTime.Now;
			string whereStr = "";   //条件判断语句
  
			try
			{
                bgDate = DateTime.Parse(this.TextBoxBeginDate.Value.Trim());
                edDate = DateTime.Parse(this.TextBoxEndDate.Value.Trim());
			}
			catch
			{
				Msg = "日期格式输入错误！请检查！";
				return false;
			}

			if (this.txbCustom.Text != "" && this.txbCustom.Text.Trim() != "")
			{
				whereStr = " and " + this.ddlCondition.SelectedValue + " = '" + classLibrary.setConfig.replaceMStr(this.txbCustom.Text.Trim()) + "' " ;  //特定查询
			}
			

			Query_Service.Query_Service qs = new Query_Service.Query_Service();

			if (ViewState["newIndex"] != null)
				istr = Int32.Parse(ViewState["newIndex"].ToString());
			else
				istr = 0;

			int iStartIndex = istr*pageSize;

			string log = classLibrary.SensitivePowerOperaLib.MakeLog("get",Session["uid"].ToString(),"[查看修改历史]",iStartIndex.ToString(),
				pageSize.ToString(),bgDate.ToString(),edDate.ToString(),whereStr);

			if(!classLibrary.SensitivePowerOperaLib.WriteOperationRecord("HistoryModify",log,this))
			{
					
			}

			if (!qs.getUserModify(istr*pageSize,pageSize,bgDate,edDate,whereStr,out ds,out Msg)) return false;

			if (ds == null || ds.Tables.Count == 0 ||ds.Tables[0].Rows.Count == 0) 
			{
				AspNetPager1.Visible = false;
				this.Page.DataBind();

				Msg = "没有您选定范围内的数据。";
				WebUtils.ShowMessage(this.Page,Msg);
				return false;
			}

			AspNetPager1.PageSize    = pageSize;
			AspNetPager1.RecordCount = Int32.Parse(ds.Tables[0].Rows[0]["icount"].ToString());
			
			AspNetPager1.CustomInfoText ="记录总数：<font color=\"blue\"><b>"+AspNetPager1.RecordCount.ToString()+"</b></font>";
			AspNetPager1.CustomInfoText+=" 总页数：<font color=\"blue\"><b>" +AspNetPager1.PageCount.ToString()+"</b></font>";
			AspNetPager1.CustomInfoText+=" 当前页：<font color=\"red\"><b>"  +AspNetPager1.CurrentPageIndex.ToString()+"</b></font>";

			
			this.dgInfo.DataSource = ds.Tables[0].DefaultView;
			this.dgInfo.DataBind();

			return true;
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
			this.AspNetPager1.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.AspNetPager1_PageChanged);

		}
		#endregion

		protected void btQuery_Click(object sender, System.EventArgs e)
		{
			
			string strszkey = Session["SzKey"].ToString().Trim();
			int ioperid = Int32.Parse(Session["OperID"].ToString());
			int iserviceid = Common.AllUserRight.GetServiceID("HistoryModify") ;
			string struserdata = Session["uid"].ToString().Trim();
			string content = struserdata + "执行了[查看修改历史]操作,操作对象[" + " "
				+ "]时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

			Common.AllUserRight.UpdateSession(strszkey,ioperid,PublicRes.GROUPID,iserviceid,struserdata,content);

			

			ViewState["newIndex"] = null;  //如果重新点击一次查询，则清空查询的分页；否则无法查询到数据（比如单笔）
			AspNetPager1.Visible = true;
			this.AspNetPager1.CurrentPageIndex = 1;

			bindaData();
		}

		private void AspNetPager1_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			istr = e.NewPageIndex;
			AspNetPager1.CurrentPageIndex = istr;

			ViewState["newIndex"] = e.NewPageIndex -1;

			bindaData();
		}

		protected void ddlCondition_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			 //当其选择下拉菜单变化，并且输入框非空，表示其需要精确查询。清空当前的页码 ViewState["newIndex"]	
			if (txbCustom.Text != "" && txbCustom.Text.Trim() !="")
			{
				AspNetPager1.Visible = true;
				ViewState["newIndex"] = null;
  
				bindaData();
			}
		}
	}
}
