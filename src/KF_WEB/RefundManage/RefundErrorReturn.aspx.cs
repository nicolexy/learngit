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

using TENCENT.OSS.CFT.KF.Common;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web;

namespace TENCENT.OSS.CFT.KF.KF_Web.RefundManage
{
	/// <summary>
	/// RefundErrorReturn 的摘要说明。
	/// </summary>
	public partial class RefundErrorReturn : System.Web.UI.Page
	{
		private string WeekIndex;
		private string BatchID;
		private string WorkType;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				//string tmp = Session["key"].ToString();
				//先把按钮全隐藏吧，就在这里。
				ddlFileType.Visible = false;
				File1.Visible = false;
				btnMain.Visible = false;
				labMain.Visible = false;
				btFinish.Visible = false;
				tbCancelReason.Visible = false;
				btCancel.Visible = false;
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}
			try
			{

				hlRefresh.NavigateUrl = Request.RawUrl;
				if (!IsPostBack)
				{   
					btnMain.Attributes.Add("onclick", "return checkvlid(this)");
					btFinish.Attributes.Add("onclick", "javascript:window.open('../batchpay/sendpaytask.aspx?batchid="+BatchID+"')");
					this.btCancel.Attributes.Add("onclick","return confirm('你确认要执行任务单回退操作吗？如果是生成异常而回退，请务必先确任务单生成操作已停止')");

					//第一次进来后，提取出要进行的工作类型和哪一周。
					WorkType = Request.QueryString["WorkType"];
					if(WorkType == null) WorkType = "task";
					ViewState["WorkType"] = WorkType;

					WeekIndex = Request.QueryString["WeekIndex"];
					if(WeekIndex == null) WeekIndex = "0";
					ViewState["WeekIndex"] = WeekIndex;

					SetViewState();

					try
					{
						//string sr = Session["key"].ToString();
						if(WorkType == "task")
						{
							lbTitle.Text = "异常任务单管理";
							//if(!AllUserRight.GetOneRightState("IDCCheck",sr)) Response.Redirect("../login.aspx?wh=1");
							this.Label1.Text = Session["uid"].ToString();
							string szkey = Session["SzKey"].ToString();
							int operid = Int32.Parse(Session["OperID"].ToString());

							// 这里检测的权限有待改正 
							//if(!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter")) Response.Redirect("../login.aspx?wh=1");
							if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
						}
					}
					catch  //如果没有登陆或者没有权限就跳出
					{
						Response.Redirect("../login.aspx?wh=1");
					}

					BatchID = Request.QueryString["BatchID"];
                
					if(BatchID == null) BatchID = "0"; 
					ViewState["BatchID"] = BatchID;
					btFinish.Attributes.Add("onclick", "javascript:window.open('../batchpay/sendpaytask.aspx?batchid="+BatchID+"')");

					ShowData(BatchID);
				}
				if(BatchID == null || BatchID == "")
				{
					WeekIndex = ViewState["WeekIndex"].ToString();
					BatchID = ViewState["BatchID"].ToString();
					WorkType = ViewState["WorkType"].ToString();
				}
			
			}
			catch
			{
				WebUtils.ShowMessage(this.Page,"读取数据出现错误！");
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


		public string GetUrl(string BatchID)
		{
			string url = "BatchID=" + BatchID + "&WorkType=" + WorkType + "&WeekIndex=" + WeekIndex;
			url = "../RefundManage/RefundErrorReturn.aspx?" + url;
			return url;
		}

		private void SetViewState()
		{
			if(Request.QueryString["BeginDate"]!=null)
			{
				ViewState["BeginDate"]=Request.QueryString["BeginDate"].ToString();
			}
			else
			{
				ViewState["BeginDate"]=System.DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日");
			}

			if(Request.QueryString["EndDate"]!=null)
			{
				ViewState["EndDate"]=Request.QueryString["EndDate"].ToString();
			}
			else
			{
				ViewState["EndDate"]=System.DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日");
			}

			if(Request.QueryString["BankType"]!=null)
			{
				ViewState["BankType"]=Request.QueryString["BankType"].ToString();
			}
			else
			{
				ViewState["BankType"]="0000";
			}

			if(Request.QueryString["BatchState"]!=null)
			{
				ViewState["BatchState"]=Request.QueryString["BatchState"].ToString();
			}
			else
			{
				ViewState["BatchState"]="9999";
			}

			if(Request.QueryString["Proposer"]!=null)
			{
				ViewState["Proposer"]=Request.QueryString["Proposer"].ToString();
			}
			else
			{
				ViewState["Proposer"]="";
			}

			if(Request.QueryString["RefundPath"]!=null)
			{
				ViewState["RefundPath"]=Request.QueryString["RefundPath"].ToString();
			}
			else
			{
				ViewState["RefundPath"]="";
			}
		}

		private void CreatePayTask(string BatchID)
		{
			
		}
		private void CreateReturnTask(string strBatchID)
		{
			
		}

		private void ShowButton(int iResult)
		{
			ddlFileType.Visible = false;
			File1.Visible = false;
			btnMain.Visible = false;
			labMain.Visible = false;
			btFinish.Visible = false;
			tbCancelReason.Visible = false;
			btCancel.Visible = false;
			ShowButtonByStatus(iResult);
		}

		private void ShowButtonByStatus(int status)
		{
			try
			{
				switch(WorkType)
				{
					case "task" :
					{
						if(status == 6 )
						{
							btnMain.Text = "生成授权退款任务单";
							btnMain.Visible = true;
							labMain.Text = WorkType;
							
						} 
						else if(status == 9) //取消掉完成功能.
						{
							labMain.Text = WorkType;

							//取消任务单完成功能,多出一个预览和发送功能.
							btFinish.Visible = true;
							btFinish.Text = "授权书任务单预览";

							btCancel.Visible = true;
							btCancel.Text = "授权书任务单取消";

							tbCancelReason.Text = "帐号错误";
							tbCancelReason.Visible = true;
						}
						else if(status == 98) //取消掉完成功能.
						{
							
						}
					}                        
						break;

				}
				
			}
			catch
			{
				WebUtils.ShowMessage(this.Page,"读取数据出现错误！");
			}
		}

		private void ShowData(string strBatchID)
		{
			DataTable dt;
			
			BatchPay_Service.BatchPay_Service bs = new TENCENT.OSS.CFT.KF.KF_Web.BatchPay_Service.BatchPay_Service();
			dt = bs.RefundOther_ShowData(strBatchID).Tables[0];

			if(dt == null || dt.Rows.Count == 0)
			{
				labApprover.Text = "  ";
				labAproveDate.Text = "  ";
				labExecutor.Text = "";
				labExecuteDate.Text = "  ";
				labUpdate.Text = "  ";
				labUpdateTime.Text = "  ";
				labPayCount.Text = "  ";
				labPaySum.Text = "  ";
				labAproveMsg.Text = " ";
			}
			else
			{
				try
				{
					DataRow dr = dt.Rows[0];
					labStatusName.Text = dr["FStatusName"].ToString();
					ViewState["FStatus"] =  dr["FStatus"].ToString();
					string tmpdate = dr["FBatchDay"].ToString();
					tmpdate = tmpdate.Substring(0,4) + "年" + tmpdate.Substring(4,2) + "月" + tmpdate.Substring(6,2) + "日";
					labDate.Text = tmpdate;
					labBank.Text =RefundErrorMain.GetBankName(dr["FBankType"].ToString()); 
					labApprover.Text = PublicRes.GetString(dr["FApprover"]);
					labAproveDate.Text = PublicRes.GetString(dr["FApproveDate"]);
					labExecutor.Text = PublicRes.GetString(dr["FProposer"]);
					labExecuteDate.Text = PublicRes.GetString(dr["FProposeDate"]);
					labUpdate.Text = PublicRes.GetString(dr["FUpdated"]);
					labUpdateTime.Text = PublicRes.GetString(dr["FUpdateTime"]);
					labPayCount.Text = PublicRes.GetString(dr["FPayCount"]);
					long sum = long.Parse(PublicRes.GetString(dr["FPaySum"]));

					double fsum = MoneyTransfer.FenToYuan(sum);
					labPaySum.Text = fsum.ToString();

					BatchPay_Service.BatchPay_Service bs2 = new TENCENT.OSS.CFT.KF.KF_Web.BatchPay_Service.BatchPay_Service();
					BatchPay_Service.Param[] ht = bs2.GetRefundState_Other(BatchID);

					if(ht != null)
					{
						labSuccessCount.Text = ht[0].ParamValue.ToString();
						labSuccessSum.Text = ht[1].ParamValue.ToString();
						labErrorCount.Text = ht[2].ParamValue.ToString();
						labErrorSum.Text = ht[3].ParamValue.ToString();
						labPayingCount.Text = ht[4].ParamValue.ToString();
						labPayingSum.Text = ht[5].ParamValue.ToString();
					}

					labAproveMsg.Text = PublicRes.GetString(dr["FApproveMsg"]);
					ShowButton(Convert.ToInt32( dr["FStatus"].ToString()));
				}
				catch
				{
					
				}
			}

			hlBack.NavigateUrl = "RefundErrorMain.aspx?WorkType=" + WorkType + "&WeekIndex=" + WeekIndex
				+"&BeginDate="+ViewState["BeginDate"].ToString()+"&EndDate="+ViewState["EndDate"].ToString()
				+"&BankType="+ViewState["BankType"].ToString()+"&BatchState="+ViewState["BatchState"].ToString()
				+"&Proposer="+ViewState["Proposer"].ToString()+"&RefundPath="+ViewState["RefundPath"].ToString();
		}


		private void btnMain_Click(object sender, System.EventArgs e)
		{
			labErrmsg.Text = "";
			//这个按钮处理 启动产生任务，审核，回导文件。
			try
			{
				btnMain.Visible = false;
				File1.Visible = false;
				ddlFileType.Visible = false;
				switch(labMain.Text)
				{
						
					case "task" :                      
						CreatePayTask(BatchID); 
						break;
                        
				}
				ShowData(BatchID);
				
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,"执行功能出错：" + PublicRes.GetErrorMsg(err.Message) + "，请重试！");
			}
		}

		private void btFinish_Click(object sender, System.EventArgs e)
		{
			

		}

		

	}
}
