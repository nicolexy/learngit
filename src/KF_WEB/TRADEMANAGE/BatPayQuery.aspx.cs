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
	/// BatPayQuery 的摘要说明。
	/// </summary>
	public partial class BatPayQuery : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Button btnMain;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if (!IsPostBack)
			{            
				this.txBatchOrder.Text = "1";
				this.TextBoxBeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
			}

			InitGrid();
		}

		private void InitGrid()
		{
			Query_Service.Query_Service qs = new Query_Service.Query_Service();
			DataTable dt = qs.BatPay_InitGrid_B(this.TextBoxBeginDate.Text.Trim(),this.txBatchOrder.Text.Trim()).Tables[0];

			if(dt == null)
			{
				WebUtils.ShowMessage(this.Page,"读取数据时出错！请重试。");
				return;
			}

			dt.Columns.Add("FBatchOrder");
			ShowBatPay(dt);		
		}

		private void ShowBatPay(DataTable dt)
		{
			DateTime BeginDate = Convert.ToDateTime(TextBoxBeginDate.Text.Trim());
			string strDateUse = BeginDate.ToString("yyyyMMdd");
                
			if(dt.Rows.Count == 0)
			{
				if(CanVisible(strDateUse,this.txBatchOrder.Text.Trim()))
				{
					DataRow dr = dt.NewRow();
					dr["FBatchID"] = strDateUse + "1001B" + T0Transfer.Order2Asc(Int32.Parse(txBatchOrder.Text.Trim()));

					string strDate = BeginDate.ToString("yyyy年MM月dd日");
					dr["FDate"] = strDate;
					dr["FStatusName"] = "尚未汇总付款数据";

					if(CanStartTask(strDateUse + "1001B" + T0Transfer.Order2Asc(Int32.Parse(txBatchOrder.Text.Trim()))))
					{
						dr["FMsg"] = "可以汇总付款数据";
					}
					else
					{
						dr["FStatusName"] = "有正在处理的任务或者日期超过今日。";
						dr["FMsg"] = "";
					}
					dr["FUrl"] = "BatchID=" + strDateUse + "1001B" + T0Transfer.Order2Asc(Int32.Parse(txBatchOrder.Text.Trim())) + "&WeekIndex=" + TextBoxBeginDate.Text.Trim()
						+ "&BatchOrder=" + this.txBatchOrder.Text.Trim();

					dr["FBankID"] = "所有银行";

					dr["FbatchOrder"] = T0Transfer.Asc2Order( dr["FBatchID"].ToString().Substring(13,1));
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
						dr["FStatusName"] = "此银行无付款记录";
					}
					if((iStatus == 9 || iStatus == 0 || iStatus==11) && CanStartTask(dr["FBatchID"].ToString()))
					{
						dr["FMsg"] = "可以汇总付款数据";
					}
					else
					{
						dr["FMsg"] = "";
					}

					dr["FUrl"] = "BatchID=" + dr["FBatchID"].ToString() + "&WeekIndex=" + TextBoxBeginDate.Text.Trim()
						+ "&BatchOrder=" + this.txBatchOrder.Text.Trim();

					dr["FBankID"] = GetBankName(dr["FBankType"].ToString());

					string tmp = dr["FDate"].ToString();

					tmp = tmp.Substring(0,4) + "年" + tmp.Substring(4,2) + "月" + tmp.Substring(6,2) + "日";
					dr["FDate"] = tmp;

					dr["FbatchOrder"] = T0Transfer.Asc2Order( dr["FBatchID"].ToString().Substring(13,1));
					dr.EndEdit();
				}
			}
                
			dt.DefaultView.Sort = "FBankType ";
			DataGrid1.DataSource = dt.DefaultView;
			DataGrid1.DataBind();
		}


		private static bool CanVisible(string strDate,string batchorder)
		{
			Query_Service.Query_Service qs = new Query_Service.Query_Service();
			return qs.BatPay_CanVisible_B(strDate,batchorder);
		}

		public static bool CanStartTask(string strBatchID)
		{
			if(CheckSnapFinish(strBatchID))
			{
				if(CheckFinish11(strBatchID))
				{								
					Query_Service.Query_Service qs = new Query_Service.Query_Service();
					return qs.BatPay_SixCheck(strBatchID);
				}
			}
			return false;
		}

		private static bool CheckSnapFinish(string strBatchID)
		{
			Query_Service.Query_Service qs = new Query_Service.Query_Service();
			return qs.BatPay_CheckSnapFinish(strBatchID);
		}

		private static bool CheckFinish11(string strBatchID)
		{
			Query_Service.Query_Service qs = new Query_Service.Query_Service();
			return qs.BatPay_CheckFinish11(strBatchID);
		}

		public static string GetStatusName(int iStatus, string strBatchID)
		{
			string[] strArray = new string[30];
			strArray[0] = "可以汇总付款数据";
			strArray[1] = "付款数据已汇总";
			strArray[2] = "付款数据已审核";
			strArray[3] = "付款任务单已生成";//"正在进行付款";
			strArray[4] = "付款已完成";
			strArray[5] = "付款结果已生效";
			strArray[6] = "付款单已生成"; 
			strArray[9] = "<FONT color=red>付款数据生成失败</FONT>";  //FBatchNoExp
			strArray[10] = "付款单生效执行结果"; //FBatchNoImp  FStatusDes
			strArray[11] = "<FONT color=red>付款数据生成后回写业务系统失败</FONT>";
			strArray[20] = "付款数据已提交审批"; 
			strArray[21] = "付款任务单已完成"; 
			strArray[22] = "付款结果一次上传成功"; 
			strArray[23] = "付款结果二次上传成功"; 
			strArray[24] = "付款调整完成";
			strArray[25] = "付款挂帐已完成"; 
			strArray[7] = "转入财务审批流程";
			string tmp = "";
			if(strBatchID != "0" && iStatus > 8 && iStatus < 12) tmp = GetErrorMsg(strBatchID, iStatus);

			if(tmp != "")
				return strArray[iStatus] + "：" + tmp;
			else
				return strArray[iStatus];
		}

		private static string GetErrorMsg(string strBatchID, int iStatus)
		{
			Query_Service.Query_Service qs = new Query_Service.Query_Service();
			return qs.BatPay_GetErrorMsg(strBatchID,iStatus);
		}

		public static string GetBankName(string strBankID)
		{
			switch(strBankID.Trim())
			{
				case "1001" :
					return "招商银行";
				case "1002" :
					return "工商银行";
				case "1003" : 
					return "建设银行";
				case "1005" :
					return "农业银行"; 
				case "1004" :
					return "浦发银行"; 
				case "1010" :
					return "深圳平安银行";
				case "1020" :
					return "交通银行";
				case "1027" :
					return "广东发展银行";
				case "1021" :
					return "中信银行";
				case "9999" :
					return "汇总银行";
				default :
					return "无此银行";
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

	}
}
