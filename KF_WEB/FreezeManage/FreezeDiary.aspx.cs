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
	/// FreezeDiary 的摘要说明。
	/// </summary>
	public partial class FreezeDiary : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面

			if(!IsPostBack)
			{
				ViewState["FFreezeListID"] = Request.QueryString["FFreezeListID"].Trim();

				if(Request.QueryString["state"] != null && Request.QueryString["state"] == "s")
				{
					WebUtils.ShowMessage(this,"操作成功");
				}
				else if(Request.QueryString["state"] != null && Request.QueryString["state"] == "f")
				{
					WebUtils.ShowMessage(this,"操作失败，请确认冻结单的处理日志状态");
				}

				this.lb_operatorID.Text = Session["uid"].ToString();

				BindData_ForDiary(1);
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

		}
		#endregion


		private void BindData_ForDiary(int index)
		{
			Query_Service.Query_Service qs = new Query_Service.Query_Service();

			string tdeid = ViewState["FFreezeListID"].ToString();

			DataSet ds =  qs.GetFreezeDiary("",tdeid,"","","","","","",1,20);

			if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
			{
				WebUtils.ShowMessage(this,"该冻结单没有处理日志");
				return;
			}	

			ds.Tables[0].Columns.Add("DiaryHandleResult",typeof(string));

			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				dr["DiaryHandleResult"] = dr["FCreateDate"].ToString() + "  " + dr["FHandleUser"].ToString()
                    + " 执行了 " + ConvertHandleTypeToString(dr["FHandleType"].ToString()) + " 操作，用户描述为：" + dr["FMemo"].ToString() + "；客服处理结果为：" + dr["FHandleResult"].ToString();
			}

			this.Datagrid1.DataSource = ds;
			this.Datagrid1.DataBind();
		}

		private string ConvertHandleTypeToString(string type)
		{
			switch(type)
			{
				case "1":
				{
					return "结单(已解冻)";
				}
				case "2":
				{
					return "补充资料";
				}
				case "7":
				{
					return "作废";
				}
				case "8":
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
	}
}
