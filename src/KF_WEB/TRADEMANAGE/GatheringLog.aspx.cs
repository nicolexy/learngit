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

using TENCENT.OSS.CFT.KF.KF_Web;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// GatheringLog ��ժҪ˵����
	/// </summary>
	public partial class GatheringLog : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		protected System.Data.DataSet DS_Gather;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			try
			{
				BindData();
				Page.DataBind();
			}
			catch(Exception emsg)
			{
				classLibrary.setConfig.exceptionMessage(emsg.Message);	
			}
		}

		private void BindData()
		{
//			Session["ListID"] = "17858478";
			string selectStrSession = Session["ListID"].ToString();
			
			DateTime beginTime = classLibrary.setConfig.SetTime("20000101000000"); 
			DateTime endTime   = classLibrary.setConfig.SetTime("30000101000000"); 
		
			int istr = 1;
			int imax = 10;

			this.DS_Gather = classLibrary.setConfig.returnDataSet(selectStrSession,1,beginTime,endTime,1,"Gather",istr,imax,Session["uid"].ToString(),Request.UserHostAddress);

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
			this.DS_Gather = new System.Data.DataSet();
			((System.ComponentModel.ISupportInitialize)(this.DS_Gather)).BeginInit();
			// 
			// DS_Gather
			// 
			this.DS_Gather.DataSetName = "NewDataSet";
			this.DS_Gather.Locale = new System.Globalization.CultureInfo("zh-CN");
			((System.ComponentModel.ISupportInitialize)(this.DS_Gather)).EndInit();

		}
		#endregion

	}
}
