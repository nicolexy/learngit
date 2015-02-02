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
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// CrtQuery 的摘要说明。
	/// </summary>
    public partial class MobileTokenQuery : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			try
			{
                if (!classLibrary.ClassLib.ValidateRight("FreezeList", this)) Response.Redirect("../login.aspx?wh=1");

                Label1.Text = Session["uid"].ToString();

				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());
				
				
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

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			try
			{

				//开始查询
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                DataSet ds = qs.GetMobileTokenList(this.tbQQID.Text.Trim());

				if(ds != null && ds.Tables.Count >0)
				{
					DataGrid1.DataSource = ds.Tables[0].DefaultView;
					DataGrid1.DataBind();
				}
				else
				{
					WebUtils.ShowMessage(this.Page,"没有找到合适的记录！");
				}
			}
			catch
			{
				WebUtils.ShowMessage(this.Page,"查询出错");
			}
		}



        protected void DataGrid1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
	}
}
