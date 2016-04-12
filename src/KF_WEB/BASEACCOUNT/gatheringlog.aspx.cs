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
	/// gatheringlog ��ժҪ˵����
	/// </summary>
	public partial class gatheringlog : System.Web.UI.Page
	{
		protected System.Data.DataTable dataTable1;
		protected System.Data.DataColumn dataColumn1;
		protected System.Data.DataSet DS_Gather;
		protected System.Web.UI.WebControls.DataGrid DG_TBankGather;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if (!IsPostBack)
			{
				try
				{
					//�󶨵�һҳ����
					BindData(1);
				}
				catch
				{
					Response.Write("<font color = red>��ʱ�������µ�½��</font>");
				}
			}		
		}

		private void BindData(int pageIndex)
		{
			//furion ������ǰ��ʱ���,���µ����������.
			//DateTime beginTime = DateTime.Parse(PublicRes.sBeginTime); 
			int pdaycount=int.Parse(PublicRes.GetZWDicValue("ShouKuanDayCount"));//�տ���ǰ��ѯ����  rowenawu 20120606
			DateTime beginTime = DateTime.Now.AddDays(-pdaycount); 
			//DateTime endTime   = DateTime.Parse(PublicRes.sEndTime); 
			//			DateTime endTime   = DateTime.Now.AddDays(1);
			DateTime endTime   = DateTime.Now;//��ֹ�����첻����ǰ��  rowenawu 20120705

			int PageSize =  Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"].ToString());  //ͨ��webconfig����ҳ��С

			if (Request.QueryString["type"].ToString() == "QQID")
			{
				PageSize=25;
			}
			int istr =PageSize * (pageIndex-1); //��ʼ����
			int imax = PageSize;                     //ÿҳ��С
    
			if (Request.QueryString["type"].ToString() == "QQID")
			{
				string selectStr = Session["QQID"].ToString();

				//furion ������ʷ���� 20060522
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

				pager.CustomInfoText="��¼������<font color=\"blue\"><b>"+pager.RecordCount.ToString()+"</b></font>";
				pager.CustomInfoText+=" ��ҳ����<font color=\"blue\"><b>"+pager.PageCount.ToString()+"</b></font>";
				pager.CustomInfoText+=" ��ǰҳ��<font color=\"red\"><b>"+pager.CurrentPageIndex.ToString()+"</b></font>";
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
				//��ȡ�������������仯ʱ����ҳ�����grid��ʾ������Ӧ��pagesize���Զ�Ҫ���Ÿ��ġ� furion 20060527
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

			//	pager.CustomInfoText="��¼������<font color=\"blue\"><b>"+pager.RecordCount.ToString()+"</b></font>";
			//	pager.CustomInfoText+=" ��ҳ����<font color=\"blue\"><b>"+pager.PageCount.ToString()+"</b></font>";
			//	pager.CustomInfoText+=" ��ǰҳ��<font color=\"red\"><b>"+pager.CurrentPageIndex.ToString()+"</b></font>";
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
