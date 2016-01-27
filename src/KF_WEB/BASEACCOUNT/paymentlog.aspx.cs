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

using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// paymentlog ��ժҪ˵����
	/// </summary>
	public partial class paymentlog : System.Web.UI.Page
	{
		protected System.Data.DataSet DS_Payment;
		protected System.Data.DataTable dataTable1;
		protected System.Data.DataColumn dataColumn1;
	
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
				catch(Exception ex)
				{
					WebUtils.ShowMessage(this.Page,"������ʱ�������²�ѯ��" + PublicRes.GetErrorMsg(ex.Message));
				}
			}		
		}

		private void BindData(int pageIndex)
		{
			//furion ������ǰ��ʱ���,���µ����������.
			//DateTime beginTime = DateTime.Parse(PublicRes.sBeginTime); 
			DateTime beginTime = DateTime.Now.AddDays(-PublicRes.PersonInfoDayCount); 
			//DateTime endTime   = DateTime.Parse(PublicRes.sEndTime); 
			DateTime endTime   = DateTime.Now.AddDays(1);

			int pageSize =  Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"].ToString());  //ͨ��webconfig����ҳ��С

			int istr = 1 + pageSize * (pageIndex-1);  //��ʼΪ1����ʵ������0ʼ��
			int imax = pageSize;                       //ÿҳ��ʾ10����¼
    
			//�����QQID��ѯҳ�����
			if (Request.QueryString["type"].ToString() == "QQID")
			{
				string selectStr = Session["QQID"].ToString();
				this.DS_Payment = classLibrary.setConfig.returnDataSet(selectStr,1,beginTime,endTime,0,"PayList",istr,imax,Session["uid"].ToString(),Request.UserHostAddress);
				
				int total;
				if(DS_Payment != null && DS_Payment.Tables.Count != 0 && DS_Payment.Tables[0].Rows.Count != 0)
				{
                    DataView dv = DS_Payment.Tables[0].DefaultView;
                    dv.Sort = "Fpay_front_time DESC";
                    DS_Payment.Tables.RemoveAt(0);
                    DS_Payment.Tables.Add(dv.ToTable());

                    total = 10000;
				}
				else
					total = 0;

				pager.RecordCount= total;
				pager.PageSize   = pageSize;

				pager.CustomInfoText="��¼������<font color=\"blue\"><b>"+pager.RecordCount.ToString()+"</b></font>";
				pager.CustomInfoText+=" ��ҳ����<font color=\"blue\"><b>"+pager.PageCount.ToString()+"</b></font>";
				pager.CustomInfoText+=" ��ǰҳ��<font color=\"red\"><b>"+pager.CurrentPageIndex.ToString()+"</b></font>";
			
			}
			else if(Request.QueryString["type"].ToString() == "ListID")//�����listIDҳ�����
			{
				this.pager.Visible = false;
				string selectStrSession = Session["ListID"].ToString();
				this.DS_Payment = classLibrary.setConfig.returnDataSet(selectStrSession,1,beginTime,endTime,1,"PayList",istr,imax,Session["uid"].ToString(),Request.UserHostAddress);
			}

            if (DS_Payment != null && DS_Payment.Tables.Count != 0 && DS_Payment.Tables[0].Rows.Count > 0)
			{
                DataView dv = DS_Payment.Tables[0].DefaultView;
                dv.Sort = "Fpay_front_time DESC";
                DS_Payment.Tables.RemoveAt(0);
                DS_Payment.Tables.Add(dv.ToTable());

				DS_Payment.Tables[0].Columns.Add("FnumStr",typeof(string));
				string[] CoverPickFuid = System.Configuration.ConfigurationManager.AppSettings["CoverPickFuid"].ToString().Split('|');

				foreach(DataRow dr in DS_Payment.Tables[0].Rows)
				{
					try
					{
						string Fnum = classLibrary.setConfig.FenToYuan(dr["Fnum"].ToString());
						dr["FnumStr"] = Fnum;

						for(int i=0; i<CoverPickFuid.Length; i++)
						{
							if(CoverPickFuid[i].ToString() == dr["Fuid"].ToString())
							{
								try
								{
									int PointIndex = Fnum.IndexOf(".");
									dr["FnumStr"] = "******" + Fnum.Substring(PointIndex-1,Fnum.Length-PointIndex+1);
								}
								catch
								{
									dr["FnumStr"] = "******";
								}
							}
						}
					}
					catch(Exception ex)
					{
						throw new Exception("��" + dr["Fnum"].ToString() + "ת��������" + ex.Message);
					}
				}
                Page.DataBind();
			}
			
		}

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex=e.NewPageIndex;
			BindData(pager.CurrentPageIndex);

			//			pager.CustomInfoText="��¼������<font color=\"blue\"><b>"+pager.RecordCount.ToString()+"</b></font>";
			//			pager.CustomInfoText+=" ��ҳ����<font color=\"blue\"><b>"+pager.PageCount.ToString()+"</b></font>";
			//			pager.CustomInfoText+=" ��ǰҳ��<font color=\"red\"><b>"+pager.CurrentPageIndex.ToString()+"</b></font>";
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
			this.DS_Payment = new System.Data.DataSet();
			this.dataTable1 = new System.Data.DataTable();
			this.dataColumn1 = new System.Data.DataColumn();
			((System.ComponentModel.ISupportInitialize)(this.DS_Payment)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();
			// 
			// DS_Payment
			// 
			this.DS_Payment.DataSetName = "NewDataSet";
			this.DS_Payment.Locale = new System.Globalization.CultureInfo("zh-CN");
			this.DS_Payment.Tables.AddRange(new System.Data.DataTable[] {
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
			((System.ComponentModel.ISupportInitialize)(this.DS_Payment)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();

		}
		#endregion
	}
}
