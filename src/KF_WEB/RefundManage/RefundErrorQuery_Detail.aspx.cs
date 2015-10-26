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
using TENCENT.OSS.CFT.KF.Common;
using CFT.CSOMS.BLL.TransferMeaning;


namespace TENCENT.OSS.CFT.KF.KF_Web.RefundManage
{
	/// <summary>
	/// RefundErrorQuery_Detail 的摘要说明。
	/// </summary>
	public partial class RefundErrorQuery_Detail : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				if (Session["uid"] == null||Session["SzKey"]==null)  Response.Redirect("../login.aspx?wh=1");
				labUid.Text = Session["uid"].ToString();
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!this.IsPostBack)
			{
				string oldID = Request.QueryString["oldid"];
				if(oldID == null || oldID.Trim() == "")
				{
					WebUtils.ShowMessage(this.Page,"参数有误！");
					return;
				}

				try
				{
					ViewState["oldID"] = oldID;
					BindInfo(oldID);
				}
				catch(Exception eSys)
				{
					WebUtils.ShowMessage(this.Page,"读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
				}
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

		private void BindInfo(string oldID)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataSet ds =  qs.GetRefundErrorDetail(oldID);

			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0 )
			{
				DataRow dr = ds.Tables[0].Rows[0];				

				if(dr["FPaylistid"]!=null&&dr["FPaylistid"].ToString()!=""&&dr["FPaylistid"].ToString().Length>10)
				{
					labFspid.Text = PublicRes.GetString(dr["FPaylistid"].ToString().Substring(0,10));
				}

				labFPaylistid.Text = PublicRes.GetString(dr["FPaylistid"]);
				labFbank_listid.Text = PublicRes.GetString(dr["Fbank_listid"]);
				labFbank_backid.Text = PublicRes.GetString(dr["Fbank_backid"]);
                labFbanktype.Text = Transfer.returnDicStr("BANK_TYPE", PublicRes.GetInt(dr["Fbanktype"]));

				
				labFamt.Text = PublicRes.GetString(dr["FamtName"]);
				labFreturnamt.Text = PublicRes.GetString(dr["FreturnamtName"]);
				labFbuyid.Text = PublicRes.GetString(dr["Fbuyid"]);

				labFbuy_name.Text = PublicRes.GetString(dr["Fbuy_name"]);
				labFbuy_bankid.Text = PublicRes.GetString(dr["Fbuy_bankid"]);
				labFPay_time.Text = PublicRes.GetString(dr["FPay_time"]);
				labFoldID.Text = PublicRes.GetString(dr["FOldid"]);
				labFCreateTime.Text = PublicRes.GetString(dr["FCreateTime"]);
				labFstate.Text = PublicRes.GetString(dr["FstateName"]);

				labFlstate.Text = PublicRes.GetString(dr["FlstateName"]);
				labFAdjustType.Text = PublicRes.GetString(dr["FAdjustTypeName"]);
				labFreturnState.Text = PublicRes.GetString(dr["FreturnStateName"]);
				labFrefundType.Text = PublicRes.GetString(dr["FrefundTypeName"]);
				labFrefundPath.Text = PublicRes.GetString(dr["FrefundPathName"]);

				labFmodify_time.Text = PublicRes.GetString(dr["Fmodify_Time"]);
				labFmemo.Text = PublicRes.GetString(dr["Fmemo"]);
				labFexplain.Text = PublicRes.GetString(dr["Fexplain"]);

				labFHandleType.Text=PublicRes.GetString(dr["FHandleTypeName"]);
				labFHandleMemo.Text=PublicRes.GetString(dr["FHandleMemo"]);

				labFRefundCount.Text=PublicRes.GetString(dr["FRefundCount"]);
				labFerrorType.Text=PublicRes.GetString(dr["FerrorTypeName"]);

				labFRefundMemo.Text=PublicRes.GetString(dr["FRefundMemo"]);
				labFHandleUser.Text=PublicRes.GetString(dr["FHandleUser"]);
				labFHandleTime.Text=PublicRes.GetString(dr["FHandleTime"]);

				labFBeforeBatchId1.Text=PublicRes.GetString(dr["FBeforeBatchId1"]);
				labFBeforeBatchId2.Text=PublicRes.GetString(dr["FBeforeBatchId2"]);
				labFBeforeBatchId3.Text=PublicRes.GetString(dr["FBeforeBatchId3"]);
				labFBeforeBatchId4.Text=PublicRes.GetString(dr["FBeforeBatchId4"]);
				labFOrigBatchId.Text=PublicRes.GetString(dr["FOrigBatchId"]);
				labFHandleBatchId.Text=PublicRes.GetString(dr["FHandleBatchId"]);
				labFAuthorizeFlagName.Text=PublicRes.GetString(dr["FAuthorizeFlagName"]);
			}
			else
			{
				throw new Exception("没有找到记录！");
			}
		}

	}
}
