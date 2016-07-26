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

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// CFDQuery 的摘要说明。
	/// </summary>
	public partial class CFDQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
            try
            {
                if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");

                this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
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


		private void BindData(int index)
		{
			if(this.tbQQID.Text.Trim() == "")
			{
				WebUtils.ShowMessage(this,"请输入QQ号码");
				return;
			}

			this.pager.CurrentPageIndex = index;

			try
			{
				Query_Service.Query_Service qs = new Query_Service.Query_Service();
				//qs.Finance_HeaderValue = classLibrary.setConfig.setFH(Session["OperID"].ToString(),Request.UserHostAddress);
				qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

				DataSet ds = qs.QueryCFDInfo(this.tbQQID.Text.Trim());

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					WebUtils.ShowMessage(this,"查询结果为空");
					return;
				}

				ds.Tables[0].Columns.Add("FstateName",typeof(string));

				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					switch(dr["Fstate"].ToString())
					{
						case "1":
						{
							dr["FstateName"] = "已绑定";break;
						}
						case "2":
						{
							dr["FstateName"] = "未绑定";break;
						}
						default:
						{
							dr["FstateName"] = "未知状态" + dr["Fstate"].ToString();break;
						}
					}
				}

				this.DataGrid1.DataSource = ds;
				this.DataGrid1.DataBind();
			}
			catch (System.Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
		}

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			BindData(1);
		}

		private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			BindData(e.NewPageIndex);
		}
	}
}
