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
using System.Configuration;
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// gatheringlog 的摘要说明。
	/// </summary>
	public partial class gatheringlog : System.Web.UI.Page
	{
		protected System.Data.DataTable dataTable1;
		protected System.Data.DataColumn dataColumn1;
		protected System.Data.DataSet DS_Gather;
		protected System.Web.UI.WebControls.DataGrid DG_TBankGather;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if (!IsPostBack)
			{
				try
				{
					//绑定第一页数据
					BindData(1);
				}
				catch
				{
					Response.Write("<font color = red>超时，请重新登陆。</font>");
				}
			}		
		}

		private void BindData(int pageIndex)
		{
			//furion 不用以前的时间段,用新的配置来替代.
			//DateTime beginTime = DateTime.Parse(PublicRes.sBeginTime); 
			int pdaycount=int.Parse(PublicRes.GetZWDicValue("ShouKuanDayCount"));//收款向前查询天数  rowenawu 20120606
			DateTime beginTime = DateTime.Now.AddDays(-pdaycount); 
			//DateTime endTime   = DateTime.Parse(PublicRes.sEndTime); 
			//			DateTime endTime   = DateTime.Now.AddDays(1);
			DateTime endTime   = DateTime.Now;//截止到当天不用往前查  rowenawu 20120705

			int PageSize =  Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"].ToString());  //通过webconfig控制页大小

			if (Request.QueryString["type"].ToString() == "QQID")
			{
				PageSize=25;
			}
			int istr =PageSize * (pageIndex-1); //起始索引
			int imax = PageSize;                     //每页大小
    
			if (Request.QueryString["type"].ToString() == "QQID")
			{
				string selectStr = Session["QQID"].ToString();

				//furion 另入历史数据 20060522
				bool isHistory = false;
				int fcurtype=1;

				if(Request.QueryString["history"] != null)
				{
					isHistory = Request.QueryString["history"].ToLower().Trim() == "true";
				}

				if(Request.QueryString["fcurtype"] != null)
				{
					fcurtype=int.Parse(Request.QueryString["fcurtype"].Trim());
				}

                string fuid = "";
                if (Session["fuid"] != null) 
                {
                    fuid = Session["fuid"].ToString();
                }
                this.DS_Gather = new TradeService().GetBankRollListByListId(selectStr, "qq", fcurtype, beginTime, endTime, 0, null, null, "0000", istr, imax, fuid);
                //this.DS_Gather = classLibrary.setConfig.returnDataSet(selectStr,1,beginTime,endTime,0,"Gather",istr,imax,
                //    Session["uid"].ToString(),Request.UserHostAddress,isHistory);
				
				int total;
				if(DS_Gather != null && DS_Gather.Tables.Count != 0 && DS_Gather.Tables[0].Rows.Count != 0)
					total = Int32.Parse(DS_Gather.Tables[0].Rows[0]["total"].ToString());
				else
					total = 0;

				pager.RecordCount= total;
				pager.PageSize   = PageSize;

				pager.CustomInfoText="记录总数：<font color=\"blue\"><b>"+pager.RecordCount.ToString()+"</b></font>";
				pager.CustomInfoText+=" 总页数：<font color=\"blue\"><b>"+pager.PageCount.ToString()+"</b></font>";
				pager.CustomInfoText+=" 当前页：<font color=\"red\"><b>"+pager.CurrentPageIndex.ToString()+"</b></font>";
			}
			else if(Request.QueryString["type"].ToString() == "ListID")
			{
				string selectStr = Session["ListID"].ToString();
				if(Request.QueryString["begindate"] != null)
				{
					beginTime=DateTime.Parse(Request.QueryString["begindate"].Trim());
				}
				if(Request.QueryString["enddate"] != null)
				{
					endTime=DateTime.Parse(Request.QueryString["enddate"].Trim());
				}
				//当取数据条数发生变化时，分页组件和grid显示条数相应的pagesize属性都要跟着更改。 furion 20060527
				//this.DS_Gather = classLibrary.setConfig.returnDataSet(selectStr,beginTime,endTime,1,"Gather",istr,20,Session["uid"].ToString(),Request.UserHostAddress);
				this.DS_Gather = classLibrary.setConfig.returnDataSet(selectStr,1,beginTime,endTime,1,"Gather",
					istr,pager.PageSize,Session["uid"].ToString(),Request.UserHostAddress);
			}

			Page.DataBind();
			//
		}

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex=e.NewPageIndex;
			BindData(pager.CurrentPageIndex);

			//	pager.CustomInfoText="记录总数：<font color=\"blue\"><b>"+pager.RecordCount.ToString()+"</b></font>";
			//	pager.CustomInfoText+=" 总页数：<font color=\"blue\"><b>"+pager.PageCount.ToString()+"</b></font>";
			//	pager.CustomInfoText+=" 当前页：<font color=\"red\"><b>"+pager.CurrentPageIndex.ToString()+"</b></font>";
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
			this.DS_Gather = new System.Data.DataSet();
			this.dataTable1 = new System.Data.DataTable();
			this.dataColumn1 = new System.Data.DataColumn();
			((System.ComponentModel.ISupportInitialize)(this.DS_Gather)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();
			// 
			// DS_Gather
			// 
			this.DS_Gather.DataSetName = "NewDataSet";
			this.DS_Gather.Locale = new System.Globalization.CultureInfo("zh-CN");
			this.DS_Gather.Tables.AddRange(new System.Data.DataTable[] {
																		   this.dataTable1});
			// 
			// dataTable1
			// 
			this.dataTable1.Columns.AddRange(new System.Data.DataColumn[] {
																			  this.dataColumn1});
			this.dataTable1.TableName = "Table1";
			// 
			// dataColumn1
			// 
			this.dataColumn1.ColumnName = "Ftde_id";
			((System.ComponentModel.ISupportInitialize)(this.DS_Gather)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();

		}
		#endregion
	}
}
