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
	/// BankRollLog ��ժҪ˵����
	/// </summary>
	public partial class BankRollLog : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		protected System.Data.DataSet DS_Bankroll;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
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
				WebUtils.ShowMessage(this.Page,"��ʱ�������µ�½��");
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
