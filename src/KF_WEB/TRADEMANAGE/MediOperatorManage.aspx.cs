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
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// MediOperatorManage 的摘要说明。
	/// </summary>
	public partial class MediOperatorManage : PageBase
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.RadioButton CheckAll1;
		protected System.Web.UI.WebControls.DropDownList ddlSP;
		protected System.Web.UI.WebControls.RadioButton CheckAll12;
		protected Wuqi.Webdiyer.AspNetPager pager;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());
				// if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			} 

			if (!IsPostBack)
			{	
				this.tbSPID.Text = Request.QueryString["spid"];
				DataGrid1.CurrentPageIndex = 0;
				BindData();
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
			this.DataGrid1.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.DataGrid1_PageIndexChanged);
			this.DataGrid1.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid1_ItemDataBound);

		}
		#endregion


		protected void Button2_Click(object sender, System.EventArgs e)
		{
			try
			{
				DataGrid1.CurrentPageIndex = 0;
				BindData();
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				this.DataGrid1.Visible = false;

				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				this.DataGrid1.Visible = false;

				WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message.ToString());
			}
		}

		private void BindData()
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataSet ds = qs.GetMediOperatorManageList(this.tbSPID.Text.Trim(),this.tbAccount.Text.Trim());

			if(ds != null && ds.Tables.Count >0)
			{
				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				throw new LogicException("没有找到记录！");
			}
		}

		private void DataGrid1_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			this.DataGrid1.CurrentPageIndex = e.NewPageIndex;
			BindData();
		}

		private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if ( e.Item.ItemIndex >= 0 )
			{
				HyperLink hl = (HyperLink)e.Item.Cells[7].FindControl("LinkSpid");
				hl.Attributes["onclick"] = ScriptPopup("MediOperatorManage_Detail.aspx?SPID="+e.Item.Cells[6].Text+"&QQ="+e.Item.Cells[2].Text,900,900,true);
			}
		}

	}
}
