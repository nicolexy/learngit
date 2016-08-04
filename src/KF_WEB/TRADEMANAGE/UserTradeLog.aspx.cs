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
	/// UserTradeLog ��ժҪ˵����
	/// </summary>
	public partial class UserTradeLog : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		protected System.Data.DataSet DS_Utradelog;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
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
				
				//���û�����ݣ�
				//if(DS_Utradelog == null || DS_Utradelog.Tables.Count == 0 || DS_Utradelog.Tables[0].Rows.Count == 0)
					//Response.Redirect("../baseAccount/blank.aspx");
			}
			catch (Exception emsg)
			{
				Response.Write("��ȡ���ݴ���!"+emsg.Message.ToString().Replace("'","��"));
			}
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
