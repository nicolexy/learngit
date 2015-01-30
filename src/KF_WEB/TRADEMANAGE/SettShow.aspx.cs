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
	/// SettShow 的摘要说明。
	/// </summary>
	public class SettShow : PageBase
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label LabelNo;
		protected System.Web.UI.WebControls.Label LabelFeeContract;
		protected System.Web.UI.WebControls.Label LabelUid;
		protected System.Web.UI.WebControls.Label LabelChannelNo;
		protected System.Web.UI.WebControls.Label LabelProductType;
		protected System.Web.UI.WebControls.Label LabelDiscount;
		protected System.Web.UI.WebControls.Label LabelAgentSPID;
		protected System.Web.UI.WebControls.Label LabelFeeItem;
		protected System.Web.UI.WebControls.Label LabelItem;
		protected System.Web.UI.WebControls.Label LabelInUid;
		protected System.Web.UI.WebControls.Label LabelFeeStandard;
		protected System.Web.UI.WebControls.Label LabelMinAmount;
		protected System.Web.UI.WebControls.Label LabelMaxAmount;
		protected System.Web.UI.WebControls.Label LabelTag;
		protected System.Web.UI.WebControls.Label LabelMinTag;
		protected System.Web.UI.WebControls.Label LabelMaxTag;
		protected System.Web.UI.WebControls.Label LabelPriceFormat;
		protected System.Web.UI.WebControls.Label LabelCalUnit;
		protected System.Web.UI.WebControls.Label LabelFix;
		protected System.Web.UI.WebControls.Label LabelCurType;
		protected System.Web.UI.WebControls.Label LabelPerMolecule;
		protected System.Web.UI.WebControls.Label LabelPerDenominator;
		protected System.Web.UI.WebControls.Label LabelCount;
		protected System.Web.UI.WebControls.Label LabelAmount;
		protected System.Web.UI.WebControls.Label LabelCalAmount;
		protected System.Web.UI.WebControls.Label LabelDueAmount;
		protected System.Web.UI.WebControls.Label LabelUseTag;
		protected System.Web.UI.WebControls.Label LabelFeeNo;
		protected System.Web.UI.WebControls.Label LabelCreateTime;
		protected System.Web.UI.WebControls.Label LabelPreDate;
		protected System.Web.UI.WebControls.Label LabelNextDate;
		protected System.Web.UI.WebControls.Label LabelModify_time;
		protected System.Web.UI.WebControls.Label LabelUserId;
		protected System.Web.UI.WebControls.Label LabelRecordStatus;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label LabelModId;
		protected System.Web.UI.WebControls.Label LabelModSpecial;
		protected System.Web.UI.WebControls.Label LabelModName;
		protected System.Web.UI.WebControls.Label LabelModBank;
		protected System.Web.UI.WebControls.Label LabelModUid;
		protected System.Web.UI.WebControls.Label LabelModUidMiddle;
		protected System.Web.UI.WebControls.Label LabelModContract;
		protected System.Web.UI.WebControls.Label LabelModStatus;
		protected System.Web.UI.WebControls.Label LabelModContract1;
		protected System.Web.UI.WebControls.Label LabelModTime;
		protected System.Web.UI.WebControls.Label LabelModUserId;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.DataGrid Datagrid2;
		protected System.Web.UI.WebControls.Label lblSpid;
		protected System.Web.UI.WebControls.HyperLink LinkRefresh;
	
		private void Page_Load(object sender, System.EventArgs e)
		{

			if (!IsPostBack)
			{
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				// if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter")) Response.Redirect("../login.aspx?wh=1");

				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

				if ( Request.QueryString["id"].Trim() != null && Request.QueryString["id"].Trim() != "" )
				{
					ShowItem();
				}
			}
		}


		private void ShowItem()
		{
			int Flag;

			try
			{
				Flag = Convert.ToInt32(Request.QueryString["id"].Trim());
			}
			catch
			{
				Flag = 0;
			}
			string filter = "FCalculateNo="+Flag;
            string wx_filter = "AND a.Fsettle_id=" + Flag;

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataSet ds  = qs.SettQuery(filter);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) 
            {
                ds = qs.SettQueryWechat(wx_filter);
            }
			
			LinkRefresh.NavigateUrl = "SettShow.aspx?id="+Request.QueryString["id"].Trim();

			LabelNo.Text = ds.Tables[0].Rows[0]["FCalculateNo"].ToString();
			lblSpid.Text = ds.Tables[0].Rows[0]["FSpid"].ToString();
			LabelFeeContract.Text = ds.Tables[0].Rows[0]["FFeeContract"].ToString();
			LabelFeeItem.Text = CacheFeeItemName(ds.Tables[0].Rows[0]["FFeeItem"].ToString());
			LabelChannelNo.Text = CacheChannelNoName(ds.Tables[0].Rows[0]["FChannelNo"].ToString());
			LabelProductType.Text = CacheProductTypeName(ds.Tables[0].Rows[0]["FChannelNo"].ToString(),ds.Tables[0].Rows[0]["FProductType"].ToString());
			LabelFeeStandard.Text = CacheFeeStandardName(ds.Tables[0].Rows[0]["FFeeStandard"].ToString());
			LabelDiscount.Text = ds.Tables[0].Rows[0]["FDiscount"].ToString()+"%";
			LabelItem.Text = ds.Tables[0].Rows[0]["FItem"].ToString();
			LabelInUid.Text = ds.Tables[0].Rows[0]["FInUid"].ToString();
			LabelCurType.Text = EnumGetName(typeof(CurType),ds.Tables[0].Rows[0]["Fcurtype"].ToString());
			LabelMinAmount.Text = MoneyFormat(ds.Tables[0].Rows[0]["FMinAmount"].ToString());
			LabelMaxAmount.Text = MoneyFormat(ds.Tables[0].Rows[0]["FMaxAmount"].ToString());
			LabelTag.Text = EnumGetName(typeof(FeeStandardTag),ds.Tables[0].Rows[0]["FGradationTag"].ToString());
			LabelCalUnit.Text = EnumGetName(typeof(FeeStandardCalUnit),ds.Tables[0].Rows[0]["FCalculateUnit"].ToString());
			LabelMinTag.Text = MoneyFormat(ds.Tables[0].Rows[0]["FMinGradation"].ToString());
			LabelMaxTag.Text = MoneyFormat(ds.Tables[0].Rows[0]["FMaxGradation"].ToString());
			LabelPriceFormat.Text = EnumGetName(typeof(FeeStandardPriceFormat),ds.Tables[0].Rows[0]["FPriceFormat"].ToString());
			LabelFix.Text = MoneyFormat(ds.Tables[0].Rows[0]["FFixAmount"].ToString());
			LabelPerMolecule.Text = ds.Tables[0].Rows[0]["FPerMolecule"].ToString();
			LabelPerDenominator.Text = ds.Tables[0].Rows[0]["FPerDenominator"].ToString();
			LabelUid.Text = ds.Tables[0].Rows[0]["FUid"].ToString();
			LabelCount.Text = ds.Tables[0].Rows[0]["FTransactionCount"].ToString();
			LabelAmount.Text = MoneyFormat(ds.Tables[0].Rows[0]["FTransactionAmount"].ToString());
			LabelCalAmount.Text = MoneyFormat(ds.Tables[0].Rows[0]["FCalculateAmount"].ToString());
			LabelDueAmount.Text = MoneyFormat(ds.Tables[0].Rows[0]["FDueAmount"].ToString());
			LabelUseTag.Text = EnumGetName(typeof(SettUseTag),ds.Tables[0].Rows[0]["FUseTag"].ToString());
			LabelFeeNo.Text = ds.Tables[0].Rows[0]["FFeeNo"].ToString();
			LabelCreateTime.Text = DateTimeFormatLong(ds.Tables[0].Rows[0]["FCreateTime"].ToString());
			LabelPreDate.Text = DateTimeFormatLongDate(ds.Tables[0].Rows[0]["FPreDate"].ToString());
			LabelNextDate.Text = DateTimeFormatLongDate(ds.Tables[0].Rows[0]["FNextDate"].ToString());
			LabelUserId.Text = ds.Tables[0].Rows[0]["FuserId"].ToString();
			LabelModify_time.Text = DateTimeFormatLong(ds.Tables[0].Rows[0]["FModify_time"].ToString());
			LabelRecordStatus.Text = EnumGetName(typeof(FeeRecordStatus),ds.Tables[0].Rows[0]["FRecordStatus"].ToString());

			LabelAgentSPID.Text = ds.Tables[0].Rows[0]["FAgentSPID"].ToString();

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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
