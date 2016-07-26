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

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// Summary description for MobileRechargeConfigValidDateEdit.
	/// </summary>
	public partial class MobileRechargeConfigValidDateEdit : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!classLibrary.ClassLib.ValidateRight("MobileConfig",this)) Response.Redirect("../login.aspx?wh=1");
			if(!Page.IsPostBack)
			{
				Session["supplierID"] = Request["id"];
				string startTime =  Request["startTime"];
				string endTime = Request["endTime"];
				this.tbx_beginDate.Text = DateTime.Parse(startTime).ToString("yyyy-MM-dd");
				this.tbx_endDate.Text = DateTime.Parse(endTime).ToString("yyyy-MM-dd");
			}
		}

		public void modifyBtn_Click(object sender, EventArgs e)
		{
			DateTime start = DateTime.Parse(this.tbx_beginDate.Text);
			DateTime end =  DateTime.Parse(this.tbx_endDate.Text);
			if(start >= end)
			{
				ShowMsg("请重新选择时间");
				return;
			}
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			string id = Convert.ToString(Session["supplierID"]);
			qs.UpdateSupplierVaildDate(id, start, end);
			Response.Write("<script language=javascript>window.parent.cancel();alert('更新成功');window.parent.location.reload();</script>");
		}

		private void ShowMsg(string msg)
		{
			Response.Write("<script language=javascript>alert('" + msg + "')</script>");
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion

		
	}
}
