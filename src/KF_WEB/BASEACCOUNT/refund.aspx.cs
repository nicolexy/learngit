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

using Wuqi.Webdiyer;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// refund ��ժҪ˵����
	/// </summary>
	public partial class refund : System.Web.UI.Page
	{
		protected System.Data.DataSet dataSet1;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if (!IsPostBack)
			{
				try
				{
					string type = "buy";
					if(Request.QueryString["type"] != null)
					{
						type = Request.QueryString["type"].ToString().Trim();
					}

					ViewState["type"] = type;

					//�󶨵�һҳ����
					BindData(1,type);
				}
				catch(Exception)
				{
					WebUtils.ShowMessage(this.Page,"������ʱ�������²�ѯ��");
				}
			}		
		}

		private void BindData(int pageIndex,string type)
		{
			//furion ������ǰ��ʱ���,���µ����������.
			//DateTime beginTime = DateTime.Parse(PublicRes.sBeginTime); 
			DateTime beginTime = DateTime.Now.AddDays(-PublicRes.PersonInfoDayCount); 
			//DateTime endTime   = DateTime.Parse(PublicRes.sEndTime); 
			DateTime endTime   = DateTime.Now.AddDays(1);

			int pageSize =  Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"].ToString());  //ͨ��webconfig����ҳ��С

			int istr = 1 + pageSize * (pageIndex-1);   //��ʼΪ1����ʵ������0ʼ��
			int imax = pageSize;                       //ÿҳ��ʾ10����¼
    
			int querytype = 0;
			if(type == "sale")
				querytype = 1;

			string selectStr = Session["QQID"].ToString();

			this.dataSet1 = classLibrary.setConfig.returnDataSet(selectStr,1,beginTime,endTime,querytype,"Refund",istr,imax,Session["uid"].ToString(),Request.UserHostAddress);
			
			int total;
			if(dataSet1 != null && dataSet1.Tables.Count != 0 && dataSet1.Tables[0].Rows.Count != 0)
				total = Int32.Parse(dataSet1.Tables[0].Rows[0]["total"].ToString());
			else
				total = 0;

			pager.RecordCount= total;
			pager.PageSize   = pageSize;

			pager.CustomInfoText="��¼������<font color=\"blue\"><b>"+pager.RecordCount.ToString()+"</b></font>";
			pager.CustomInfoText+=" ��ҳ����<font color=\"blue\"><b>"+pager.PageCount.ToString()+"</b></font>";
			pager.CustomInfoText+=" ��ǰҳ��<font color=\"red\"><b>"+pager.CurrentPageIndex.ToString()+"</b></font>";

			Page.DataBind();
		}

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex=e.NewPageIndex;

			string type = ViewState["type"].ToString();

			BindData(pager.CurrentPageIndex,type);
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
			this.dataSet1 = new System.Data.DataSet();
			((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
			// 
			// dataSet1
			// 
			this.dataSet1.DataSetName = "NewDataSet";
			this.dataSet1.Locale = new System.Globalization.CultureInfo("zh-CN");
			((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();

		}
		#endregion


	}
}
