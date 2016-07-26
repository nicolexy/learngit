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
using System.Configuration;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// UserTradeLog 的摘要说明。
	/// </summary>
	public partial class UserTradeLog : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		protected System.Data.DataSet DS_Utradelog;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			BindData();
		}

		private void BindData()
		{
			try
			{
				string selectStrSession = Session["ListID"].ToString();
			
				DateTime beginTime = DateTime.Parse(ConfigurationManager.AppSettings["sBeginTime"].ToString());
				DateTime endTime   = DateTime.Parse(ConfigurationManager.AppSettings["sEndTime"].ToString());
		
				int istr = 1;
				int imax = 10;

				TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service myService = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				//myService.Finance_HeaderValue = classLibrary.setConfig.setFH(Session["uid"].ToString(),Request.UserHostAddress);  
				myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

				string QQID = "1";

				this.DS_Utradelog = myService.GetUserPayList_withID(QQID,beginTime,endTime,selectStrSession,istr,imax); //classLibrary.setConfig.returnDataSet(selectStrSession,beginTime,endTime,1,"Gather",istr,imax);
				Page.DataBind();
				
				//如果没有数据，
				//if(DS_Utradelog == null || DS_Utradelog.Tables.Count == 0 || DS_Utradelog.Tables[0].Rows.Count == 0)
					//Response.Redirect("../baseAccount/blank.aspx");
			}
			catch (Exception emsg)
			{
				Response.Write("读取数据错误!"+emsg.Message.ToString().Replace("'","’"));
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
			this.DS_Utradelog = new System.Data.DataSet();
			((System.ComponentModel.ISupportInitialize)(this.DS_Utradelog)).BeginInit();
			// 
			// DS_Utradelog
			// 
			this.DS_Utradelog.DataSetName = "NewDataSet";
			this.DS_Utradelog.Locale = new System.Globalization.CultureInfo("zh-CN");
			((System.ComponentModel.ISupportInitialize)(this.DS_Utradelog)).EndInit();

		}
		#endregion
	}
}
