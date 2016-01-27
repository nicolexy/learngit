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
	/// paymentlog 的摘要说明。
	/// </summary>
	public partial class paymentlog : System.Web.UI.Page
	{
		protected System.Data.DataSet DS_Payment;
		protected System.Data.DataTable dataTable1;
		protected System.Data.DataColumn dataColumn1;
	
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
				catch(Exception ex)
				{
					WebUtils.ShowMessage(this.Page,"操作超时！请重新查询。" + PublicRes.GetErrorMsg(ex.Message));
				}
			}		
		}

		private void BindData(int pageIndex)
		{
			//furion 不用以前的时间段,用新的配置来替代.
			//DateTime beginTime = DateTime.Parse(PublicRes.sBeginTime); 
			DateTime beginTime = DateTime.Now.AddDays(-PublicRes.PersonInfoDayCount); 
			//DateTime endTime   = DateTime.Parse(PublicRes.sEndTime); 
			DateTime endTime   = DateTime.Now.AddDays(1);

			int pageSize =  Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"].ToString());  //通过webconfig控制页大小

			int istr = 1 + pageSize * (pageIndex-1);  //初始为1（事实上索引0始）
			int imax = pageSize;                       //每页显示10条记录
    
			//如果是QQID查询页面调用
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

				pager.CustomInfoText="记录总数：<font color=\"blue\"><b>"+pager.RecordCount.ToString()+"</b></font>";
				pager.CustomInfoText+=" 总页数：<font color=\"blue\"><b>"+pager.PageCount.ToString()+"</b></font>";
				pager.CustomInfoText+=" 当前页：<font color=\"red\"><b>"+pager.CurrentPageIndex.ToString()+"</b></font>";
			
			}
			else if(Request.QueryString["type"].ToString() == "ListID")//如果是listID页面调用
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
						throw new Exception("金额：" + dr["Fnum"].ToString() + "转换有问题" + ex.Message);
					}
				}
                Page.DataBind();
			}
			
		}

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex=e.NewPageIndex;
			BindData(pager.CurrentPageIndex);

			//			pager.CustomInfoText="记录总数：<font color=\"blue\"><b>"+pager.RecordCount.ToString()+"</b></font>";
			//			pager.CustomInfoText+=" 总页数：<font color=\"blue\"><b>"+pager.PageCount.ToString()+"</b></font>";
			//			pager.CustomInfoText+=" 当前页：<font color=\"red\"><b>"+pager.CurrentPageIndex.ToString()+"</b></font>";
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
