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

namespace TENCENT.OSS.CFT.KF.KF_Web.FreezeManage
{
	/// <summary>
	/// FreezeVerify 的摘要说明。
	/// </summary>
	public partial class FreezeVerify : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox tbx_listno1;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面

			if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

			this.btnBeginDate.Attributes.Add("onclick","openModeBegin()");
			this.btnEndDate.Attributes.Add("onclick","openModeEnd()");

			if(!IsPostBack)
			{
				this.tbx_beginDate.Text = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd 00:00:00");
                this.tbx_endDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
			}

			//this.pager.RecordCount = this.GetRecordCount();
            pager.PageSize = 10;
			pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);

			//this.DataGrid_QueryResult.ItemCommand += new DataGridCommandEventHandler(DataGrid_QueryResult_ItemCommand);
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
			this.DataGrid_QueryResult.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid_QueryResult_ItemDataBound);

		}
		#endregion




		private void BindData(int index)
		{
			this.pager.CurrentPageIndex = index;

			DateTime beginDate;
			DateTime endDate;

			try
			{
                beginDate = DateTime.Parse(this.tbx_beginDate.Text);
				endDate = DateTime.Parse(this.tbx_endDate.Text);

				if(beginDate.CompareTo(endDate) > 0)
				{
					throw new Exception("开始日期大于结束日期");
				}
                if (beginDate.AddMonths(1).AddDays(1) < endDate)
				{
					throw new Exception("日期间隔大于一个月，请重新输入！");
				}

				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

				Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
				qs.Finance_HeaderValue = fh;

				int allRecordCount = 0;

                DataSet ds = qs.GetFreezeList_3(this.tbx_payAccount.Text.Trim(), beginDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"),
                    int.Parse(this.ddl_orderState.SelectedValue), this.tbx_listNo.Text.Trim(), this.tbx_people.Text.Trim(), this.tbx_reason.Text.Trim(),
                    (index - 1) * this.pager.PageSize, this.pager.PageSize, this.ddl_queryOrderType.SelectedValue, out allRecordCount);

				this.lb_count.Text = allRecordCount.ToString();

				this.pager.RecordCount = allRecordCount;

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					ShowMsg("查询结果为空");
					this.DataGrid_QueryResult.DataSource = null;
					this.DataGrid_QueryResult.DataBind();
					return;
				}

				ds.Tables[0].Columns.Add("OpUrl",typeof(string));
				ds.Tables[0].Columns.Add("DiaryUrl",typeof(string));
				ds.Tables[0].Columns.Add("handleStateName",typeof(string));
				ds.Tables[0].Columns.Add("handleUserName",typeof(string));

				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					DataRow dr = ds.Tables[0].Rows[i];

					//dr["OpUrl"] = @".\FreezeVerify.aspx?fid="+ dr["FID"].ToString() + "&ffreeze_id=" + dr["FFreezeID"].ToString();
					//dr["DiaryUrl"] = @".\FreezeDiary.aspx?FFreezeListID="+ dr["FID"].ToString() + "&ffreeze_id=" + dr["FFreezeID"].ToString();

                    dr["OpUrl"] = @".\FreezeVerify.aspx?fid=" + dr["FID"].ToString() + "&ffreeze_id=" + dr["FUin"].ToString();
                    dr["DiaryUrl"] = @".\FreezeDiary.aspx?FFreezeListID=" + dr["FID"].ToString() + "&ffreeze_id=" + dr["FUin"].ToString();

					string stateName = "{0}";

					if(dr["isFreezeListHas"].ToString() == "0")
					{
						stateName = "该账户不处于冻结状态 ({0})";
					}
					
					switch(dr["Fstate"].ToString())
					{
						case "0":
						{
							stateName = string.Format(stateName,"未处理");
							//dr["handleStateName"] = stateName;
							//dr["handleUserName"] = dr["FCheckUser"].ToString();
							break;
						}
						case "1":
						{
							stateName = string.Format(stateName,"结单(已解冻)");
							//dr["handleStateName"] = stateName;
							//dr["handleUserName"] =  dr["FCheckUser"].ToString();
							break;
						}
						case "2":
						{
                            stateName = string.Format(stateName, "结单（未解冻）");
							//dr["handleStateName"] = "结单（未解冻）";
							//dr["handleUserName"] =  dr["FCheckUser"].ToString();
							break;
						}
						case "7":
						{
							stateName = string.Format(stateName,"已作废");
							//dr["handleStateName"] = "已作废";
							//dr["handleUserName"] = dr["FCheckUser"].ToString();
							break;
						}
						case "8":
						{
							stateName = string.Format(stateName,"挂起");
							//dr["handleStateName"] = "挂起";
							//dr["handleUserName"] = dr["FCheckUser"].ToString();
							break;
						}
                        
						/*
						case "5":
						{
							dr["handleStateName"] = "补充处理结果";
							dr["handleUserName"] = dr["FCheckUser"].ToString();
							break;
						}
						*/
						default:
						{
							stateName = string.Format(stateName,"未知" + dr["Fstate"].ToString());
							//stateName += "(未知" + dr["Fstate"].ToString() + ")";
							//dr["handleStateName"] = "未知" + dr["Fstate"].ToString();
							//dr["handleUserName"] = "未知";
							break;
						}
					}

					dr["handleStateName"] = stateName;
					dr["handleUserName"] = dr["FCheckUser"].ToString();
					
				}

				ds.AcceptChanges();

				this.DataGrid_QueryResult.DataSource = ds;
				this.DataGrid_QueryResult.DataBind();
			}
			catch(Exception ex)
			{
                WebUtils.ShowMessage(this.Page, ex.Message);
			}
		}

		protected void btn_query_Click(object sender, System.EventArgs e)
		{
			BindData(1);
		}



		private int GetRecordCount()
		{
			// 不采用查询数据库来查询页面数了，直接采用10000
			return 10000;
		}


		private void ShowMsg(string szMsg)
		{
			Response.Write("<script language=javascript>alert('" + szMsg + "')</script>");
		}

		private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			int newPageIndex = e.NewPageIndex;
			this.pager.CurrentPageIndex = newPageIndex;

			BindData(newPageIndex);
		}


		/*
		private void BindData_ForDiary(int index)
		{
			Query_Service.Query_Service qs = new Query_Service.Query_Service();

			string tdeid = ViewState["FFreezeListID"].ToString();

			DataSet ds =  qs.GetFreezeDiary("",tdeid,"","","","","","",0,1);

			if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
			{
				WebUtils.ShowMessage(this,"该记录的处理日志为空");
				this.Datagrid1.DataSource = null;
				this.Datagrid1.DataBind();
				return;
			}

			ds.Tables[0].Columns.Add("DiaryHandleResult",typeof(string));

			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				dr["DiaryHandleResult"] = dr["FCreateDate"].ToString() + "  " + dr["FHandleUser"].ToString() 
					+ " 执行了 " + ConvertHandleTypeToString(dr["FHandleType"].ToString()) + " 操作，结果为：" + dr["FHandleResult"].ToString();
			}

			this.Datagrid1.DataSource = ds;
			this.Datagrid1.DataBind();
		}
		*/


		private string ConvertHandleTypeToString(string type)
		{
			switch(type)
			{
				case "1":
				{
					return "结单(未解冻)";
				}
				case "2":
				{
					return "结单(已解冻)";
				}
				case "3":
				{
					return "作废";
				}
				case "10":
				{
					return "挂起";
				}
				case "100":
				{
					return "补充处理结果";
				}
				default:
				{
					return "未知操作" + type;
				}
			}
		}

		private void DataGrid_QueryResult_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.Cells[8].Text == "BIGMONEY")
			{
				e.Item.Cells[0].ForeColor = Color.Red;
				e.Item.Cells[1].ForeColor = Color.Red;
			}
		}


	}
}
