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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// SpidShow 的摘要说明。
	/// </summary>
	public class SpidShow : PageBase
	{
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.HyperLink LinkRefresh;
		protected System.Web.UI.WebControls.Label LabelModId;
		protected System.Web.UI.WebControls.Label LabelModUid;
		protected System.Web.UI.WebControls.Label LabelModUidMiddle;
		protected System.Web.UI.WebControls.Label LabelModContract;
		protected System.Web.UI.WebControls.Label LabelModContract1;
		protected System.Web.UI.WebControls.Label LabelModTime;
		protected System.Web.UI.WebControls.Label LabelModUserId;
		protected System.Web.UI.WebControls.DataGrid Datagrid2;
		protected System.Web.UI.WebControls.Label LabelModSpecial;
		protected System.Web.UI.WebControls.Label LabelModName;
		protected System.Web.UI.WebControls.Label LabelModBank;
		protected System.Web.UI.WebControls.Label LabelModStatus;
	
		System.Data.DataSet ds;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

				if ( Request.QueryString["id"].Trim() != null && Request.QueryString["id"].Trim() != "" )
				{
					ShowItem();
				}
			}
		}

		private void ShowItem()
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			DataSet ds = qs.QueryForSelect6(Request.QueryString["id"].Trim());

			LinkRefresh.NavigateUrl = "SpidShow.aspx?id="+Request.QueryString["id"].Trim();

			LabelModId.Text = ds.Tables[0].Rows[0]["FSpid"].ToString();
			LabelModName.Text = ds.Tables[0].Rows[0]["FName"].ToString();
			LabelModUid.Text = ds.Tables[0].Rows[0]["FUid"].ToString();
			LabelModUidMiddle.Text = ds.Tables[0].Rows[0]["FUidMiddle"].ToString();
			LabelModContract.Text = ds.Tables[0].Rows[0]["FFeeContract"].ToString();
			LabelModContract1.Text = ds.Tables[0].Rows[0]["FContract"].ToString();
			LabelModSpecial.Text = ds.Tables[0].Rows[0]["Fspecial"].ToString();
			LabelModStatus.Text = EnumGetName(typeof(FeeRecordStatus),ds.Tables[0].Rows[0]["FRecordStatus"].ToString());
			LabelModTime.Text = DateTimeFormatLong(ds.Tables[0].Rows[0]["FModify_time"].ToString());
			LabelModUserId.Text = ds.Tables[0].Rows[0]["FUserId"].ToString();

			LabelModBank.Text = qs.QueryForSelect7(LabelModSpecial.Text.Trim());
			
			ds.Dispose();

			string Spid = LabelModId.Text.Replace("'","''").Replace("\r\n", "' + CHAR(13) + CHAR(10) + '").Replace("\n", "' + CHAR(10) + '");
			ds = qs.QueryForSelect8("t_feecontract.FSpid='"+Spid+"'");
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) 
            {
                ds = qs.QueryContractWechat("a.Fspid='"+Spid+"'");
            }
			Datagrid2.DataSource = ds.Tables[0].DefaultView;
			Datagrid2.DataBind();

			ds.Dispose();
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
			this.Datagrid2.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.Datagrid2_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Datagrid2_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if ( e.Item.ItemIndex >=0 )
			{
				e.Item.Cells[2].Text = CacheProductTypeName(e.Item.Cells[1].Text,e.Item.Cells[2].Text);
				e.Item.Cells[1].Text = CacheChannelNoName(e.Item.Cells[1].Text);
				e.Item.Cells[3].Text = CacheFeeItemName(e.Item.Cells[3].Text);
				e.Item.Cells[4].Text = CacheFeeStandardName(e.Item.Cells[4].Text);
				
				e.Item.Cells[8].Text = MoneyFormat(e.Item.Cells[8].Text);
				e.Item.Cells[9].Text = MoneyFormat(e.Item.Cells[9].Text);
				
				e.Item.Cells[6].Text = EnumGetName(typeof(FeeContractCyc),e.Item.Cells[6].Text);
				e.Item.Cells[10].Text = EnumGetName(typeof(FeeContractStatus),e.Item.Cells[10].Text);
		
				e.Item.Cells[11].Text = DateTimeFormatLongDate(e.Item.Cells[11].Text);
				e.Item.Cells[12].Text = DateTimeFormatLongDate(e.Item.Cells[12].Text);
			}
		}
	}
}
