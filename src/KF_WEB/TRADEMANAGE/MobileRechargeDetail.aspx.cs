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
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// Summary description for MobileRechargeDetail.
	/// </summary>
	public partial class MobileRechargeDetail : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{

		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				string flistid = Request.QueryString["flistid"].Trim();
				if(flistid != string.Empty)
				{
					try
					{
						Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
						DataSet ds = qs.QueryMobilRechargeOrder(flistid);
						if(ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count != 1)
						{
							Response.Write("<script language=javascript>alert('查询记录出错')</script>");
						}
						DataRow row = ds.Tables[0].Rows[0];
						lbl_pannel.Text = row["Fchannel"] is DBNull ? "" : row["Fchannel"].ToString();
						lbl_order.Text = row["Fsupply_list"] is DBNull ? "" : row["Fsupply_list"].ToString();
						lbl_realPrice.Text = row["Fnum"] is DBNull ? "" : row["Fnum"].ToString();
						lbl_denomination.Text = row["Fcard_value"] is DBNull ? "" : row["Fcard_value"].ToString();
						lbl_provider.Text = row["Provider"] is DBNull ? "" : row["Provider"].ToString();
						lbl_prepay.Text = row["Fpay_front_time"] is DBNull ? "" : row["Fpay_front_time"].ToString();
						lbl_return.Text = row["Fsp_time"] is DBNull ? "" : row["Fsp_time"].ToString();
						lbl_modify.Text = row["Fmodify_time"] is DBNull ? "" : row["Fmodify_time"].ToString();
					}
					catch(Exception ex)
					{
						loger.err("MobileRechargeDetail", ex.Message);
						Response.Write("<script language=javascript>alert('查询记录出错')</script>");
					}
				}
			}
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
