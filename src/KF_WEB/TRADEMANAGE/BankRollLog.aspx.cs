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

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// BankRollLog 的摘要说明。
	/// </summary>
	public partial class BankRollLog : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		protected System.Data.DataSet DS_Bankroll;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
//			if (!IsPostBack)
//			{
				BindData();
//			}	
		}
		
		private void BindData()
		{
			try
			{
				Session["uid"].ToString();
			}
			catch
			{
				WebUtils.ShowMessage(this.Page,"超时，请重新登陆。");
			}

			string selectStrSession = Session["ListID"].ToString();
			
			DateTime beginTime = classLibrary.setConfig.SetTime("20000101000000"); 
			DateTime endTime   = classLibrary.setConfig.SetTime("30000101000000"); 
		
			int istr = 1;
			int imax = 10;

			TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service myService = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			//myService.Finance_HeaderValue = classLibrary.setConfig.setFH(Session["uid"].ToString(),Request.UserHostAddress);      

			myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

			//string QQID = "1122";

			this.DS_Bankroll = myService.GetBankRollList_withID(beginTime,endTime,selectStrSession,istr,imax); //classLibrary.setConfig.returnDataSet(selectStrSession,beginTime,endTime,1,"Gather",istr,imax);
            Page.DataBind();
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
			this.DS_Bankroll = new System.Data.DataSet();
			((System.ComponentModel.ISupportInitialize)(this.DS_Bankroll)).BeginInit();
			// 
			// DS_Bankroll
			// 
			this.DS_Bankroll.DataSetName = "NewDataSet";
			this.DS_Bankroll.Locale = new System.Globalization.CultureInfo("zh-CN");
			((System.ComponentModel.ISupportInitialize)(this.DS_Bankroll)).EndInit();

		}
		#endregion


		//SELECT * FROM c2c_db.t_bankroll_list_200505 where  flistid = '1000000101200505311703714260' union select *  from  c2c_db_47.t_bankroll_list_0 where  flistid = '1000000101200505311703714260'
		


	}
}
