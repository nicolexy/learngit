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
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// RefundMain 的摘要说明。
	/// </summary>
	public partial class RefundMain : System.Web.UI.Page
	{
		private string WeekIndex = "0";
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label_uid.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

				ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 

				if (!IsPostBack)
				{
					WeekIndex = DateTime.Today.AddDays(-1).ToString("yyyy年MM月dd日");
					TextBoxBeginDate.Text = WeekIndex;
				}
				else
				{
					WeekIndex = TextBoxBeginDate.Text.Trim();
				}

			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			try
			{   
				InitGrid();   
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,ex.Message);//"显示数据时出错，请重试。");
			}
		}

		private void InitGrid()
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataTable dt = qs.BatPay_InitGrid_R(WeekIndex).Tables[0];

			if(dt == null)
			{
				WebUtils.ShowMessage(this.Page,"读取数据时出错！请重试。");
				return;
			}

			ShowBatPay(dt);          
		}
	
		private void ShowBatPay(DataTable dt)
		{
			DateTime BeginDate = DateTime.Parse(WeekIndex);  
			string strDateUse = BeginDate.ToString("yyyyMMdd");
 
			dt.Columns.Add("Detail",typeof(string));
			if(dt.Rows.Count == 0)
			{
				if(CanVisible(strDateUse))
				{
					DataRow dr = dt.NewRow();
					dr["FBatchID"] = strDateUse + "1001R";

					string strDate = BeginDate.ToString("yyyy年MM月dd日");
					dr["FDate"] = strDate;
					dr["FStatusName"] = "尚未汇总退单数据";
					dr["FMsg"] = "可以汇总退单数据";
					dr["FUrl"] = "BatchID=" + strDateUse + "1001R" + "&WeekIndex=" + WeekIndex;
					dr["FBankID"] = "所有银行";
					dt.Rows.Add(dr);
				}
			}
			else
			{
				foreach(DataRow dr in dt.Rows)
				{
					int iStatus = Int32.Parse(dr["FStatus"].ToString());
					dr.BeginEdit();
					dr["FStatusName"] = GetStatusName(iStatus,dr["FBatchID"].ToString());
					if(iStatus == 5 && (dr["FPayCount"] == null || dr["FPayCount"].ToString() == "0"))
					{
						dr["FStatusName"] = "此银行无退单记录";
					}
					if((iStatus == 9 ) )
					{
						dr["FMsg"] = "可以汇总退单数据";
					}
					else
					{
						dr["FMsg"] = "";
					}
					if(dr["FPayCount"] != null || dr["FPayCount"].ToString() != "0")
					{
						dr["Detail"] = "详细";
					}

					dr["FUrl"] = "BatchID=" + dr["FBatchID"].ToString() + "&WeekIndex=" + WeekIndex;

					dr["FBankID"] = GetBankName(dr["FBankType"].ToString());

					string tmp = dr["FDate"].ToString();

					tmp = tmp.Substring(0,4) + "年" + tmp.Substring(4,2) + "月" + tmp.Substring(6,2) + "日";
					dr["FDate"] = tmp;
					dr.EndEdit();
				}
			}
                
			dt.DefaultView.Sort = "FBankType ";
			DataGrid1.DataSource = dt.DefaultView;
			DataGrid1.DataBind();
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

		private static bool CanVisible(string strDate)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			return qs.BatPay_CanVisible_R(strDate);
		}

		public static string GetBankName(string strBankID)
		{
			if(strBankID == "9999")
				return "汇总银行";
			else
				return classLibrary.setConfig.convertbankType(strBankID);
			
		}

		public static string GetStatusName(int iStatus, string strBatchID)
		{
			string[] strArray = new string[30];
			strArray[0] = "汇总退单数据中";
			strArray[1] = "退单数据已汇总";
			strArray[2] = "退单数据已审核";
			strArray[3] = "退单任务单已生成";
			strArray[4] = "退单已完成";
			strArray[5] = "退单结果已生效";
			strArray[6] = "退单任务单已生成";
			strArray[9] = "<FONT color=red>退单数据生成失败</FONT>";
			strArray[10] = "退单任务单生效执行结果";
			strArray[11] = "<FONT color=red>退单数据生成后回写业务系统失败</FONT>";
			strArray[20] = "退单数据已提交审批"; 
			strArray[21] = "退单任务单已完成"; 
			strArray[22] = "退单结果一次上传成功"; 
			strArray[23] = "退单结果二次上传成功"; 
			strArray[24] = "退单调整完成";
			strArray[25] = "退单挂帐已完成"; 
			strArray[7] = "转入财务审批流程";

			string tmp = "";
			if(strBatchID != "0" && iStatus > 8 && iStatus < 12) tmp = "";

			if(tmp != "")
				return strArray[iStatus] + "：" + tmp;
			else
				return strArray[iStatus];
            
		}

	}
}
