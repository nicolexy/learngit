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

using TENCENT.OSS.C2C.Finance;
using TENCENT.OSS.CFT.KF.Common;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.TransferMeaning;

namespace TENCENT.OSS.CFT.KF.KF_Web.RefundManage
{
	/// <summary>
	/// RefundErrorMain 的摘要说明。
	/// </summary>
	public partial class RefundErrorMain : System.Web.UI.Page
	{

		private string WorkType = "";
		private string WeekIndex = "0";
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
			ButtonEndDate.Attributes.Add("onclick", "openModeEnd()"); 

			// 在此处放置用户代码以初始化页面
			if (!IsPostBack)
			{           
				WeekIndex = Request.QueryString["WeekIndex"];
				try
				{
					DateTime dt = DateTime.Parse(WeekIndex);
				}
				catch
				{
					try
					{
						WeekIndex = DateTime.Parse(PublicRes.strNowTime).AddDays(-1).ToString("yyyy年MM月dd日");	
					}
					catch
					{
						WeekIndex = DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日");
					}

					if(WeekIndex == null || WeekIndex.Trim() == "")
						WeekIndex = DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日");
				} 
   
				ViewState["WorkType"] = WorkType;
				TextBoxBeginDate.Text = WeekIndex;
				TextBoxEndDate.Text = WeekIndex;

				classLibrary.setConfig.GetAllBankList(ddlBankType);
				ddlBankType.Items.Insert(0,new ListItem("所有银行","0000"));

				SetViewState();
			}
			else
			{
				WorkType = ViewState["WorkType"].ToString();
				WeekIndex = TextBoxBeginDate.Text.Trim();
			}
			//权限判断
			try
			{
				//第一次进来后，提取出要进行的工作类型和哪一周。
				WorkType = Request.QueryString["WorkType"];
				if(WorkType == null) WorkType = "batpay";

				try
				{
					this.Label_uid.Text = Session["uid"].ToString();
					//string sr = Session["key"].ToString();					
					
					if(WorkType == "task")
					{
						lbTitle.Text = "异常退单管理";
						//this.Label1.Text = Session["uid"].ToString();
						string szkey = Session["SzKey"].ToString();
						int operid = Int32.Parse(Session["OperID"].ToString());
							
						// if(!AllUserRight.GetOneRightState("IDCCheck",sr)) Response.Redirect("../login.aspx?wh=1");
						//if(!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter"))
						if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
						{
							Response.Redirect("../login.aspx?wh=1");
						}
					}		
				}
				catch  //如果没有登陆或者没有权限就跳出
				{
					Response.Redirect("../login.aspx?wh=1");
				}

				this.Label_uid.Text = Session["uid"].ToString();  
				//工作类型，星期，银行全确定了，就可以先取出数据了。
                
				InitGrid(this.TextBoxBeginDate.Text.Trim(),this.TextBoxEndDate.Text.Trim(),this.ddlBankType.SelectedValue,int.Parse(this.ddlState.SelectedValue),this.txtProposer.Text.Trim(),this.ddlRefundPath.SelectedValue);
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,"显示数据时出错，请重试。"+PublicRes.GetErrorMsg(ex.Message));
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
				try
				{
					DateTime.Parse(TextBoxBeginDate.Text.Trim());
					DateTime.Parse(TextBoxEndDate.Text.Trim());
				}
				catch
				{
					WebUtils.ShowMessage(this.Page,"选择的日期非法！");
					return;
				}

				InitGrid(TextBoxBeginDate.Text.Trim(), TextBoxEndDate.Text.Trim(),ddlBankType.SelectedValue,int.Parse(ddlState.SelectedValue),this.txtProposer.Text,this.ddlRefundPath.SelectedValue);
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,PublicRes.GetErrorMsg(ex.Message));
			}
		}


		private void SetViewState()
		{
			if(Request.QueryString["BeginDate"]!=null)
			{
				this.TextBoxBeginDate.Text=Request.QueryString["BeginDate"].ToString();
			}
			else
			{
				this.TextBoxBeginDate.Text=System.DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日");
			}

			if(Request.QueryString["EndDate"]!=null)
			{
				this.TextBoxEndDate.Text=Request.QueryString["EndDate"].ToString();
			}
			else
			{
				this.TextBoxEndDate.Text=System.DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日");
			}

			if(Request.QueryString["BankType"]!=null)
			{
				this.ddlBankType.SelectedValue=Request.QueryString["BankType"].ToString();
			}
			else
			{
				this.ddlBankType.SelectedValue="0000";
			}

			if(Request.QueryString["BatchState"]!=null)
			{
				this.ddlState.SelectedValue=Request.QueryString["BatchState"].ToString();
			}
			else
			{
				this.ddlState.SelectedValue="9999";
			}

			if(Request.QueryString["Proposer"]!=null)
			{
				this.txtProposer.Text=Request.QueryString["Proposer"].ToString();
			}
			else
			{
				this.txtProposer.Text="";
			}
			if(Request.QueryString["RefundPath"]!=null)
			{
				this.ddlRefundPath.SelectedValue=Request.QueryString["RefundPath"].ToString();
			}
			else
			{
				this.ddlRefundPath.SelectedValue="9999";
			}
		}

		public static string GetBankName(string strBankID)
		{
			//不再从数据库里取,单独处理
			if(strBankID == "9999")
				return "汇总银行";
			else
                return Transfer.convertbankType(strBankID);
		}

		public static void GetAllBankList(DropDownList ddl)
		{
			GetAllPayBankList(ddl);
			ddl.Items.Add(new ListItem("汇总银行","9999"));
		}

		public static void GetAllPayBankList(DropDownList ddl)
		{
			ddl.Items.Clear();

			//			string allrefundbank = System.Configuration.ConfigurationManager.AppSettings["RefundFileBankType"].Trim();
			//string allrefundbank = BankLib.ZWDicClass.GetZWDicValue("RefundFileBankType",PublicRes.GetConnString("ZW"));
			string allrefundbank = "";
			string[] banklist = allrefundbank.Split('|');
            
			if(banklist.Length == 0)
				return;

			foreach(string strbank in banklist)
			{
				string strtmp = strbank.Trim();
				if(strtmp.Length >= 4)
				{
					strtmp = strtmp.Substring(0,4);
                    string bankname = Transfer.convertbankType(strtmp);
					ddl.Items.Add(new ListItem(bankname,strtmp));
				}
			}
		}

		private void InitGrid(string beginDate,string endDate,string banktype,int status,string proposer,string refundPath)
		{
			DataTable dt;

			BatchPay_Service.BatchPay_Service bs = new TENCENT.OSS.CFT.KF.KF_Web.BatchPay_Service.BatchPay_Service();
			dt = bs.RefundOther_InitGrid_R(beginDate,endDate,banktype,status,proposer,refundPath).Tables[0];

			ViewState["BeginDate"]=beginDate;
			ViewState["EndDate"]=endDate;
			ViewState["BankType"]=banktype;
			ViewState["BatchState"]=status;
			ViewState["Proposer"]=proposer;
			ViewState["RefundPath"]=refundPath;

			if(dt == null)
			{
				//读取数据出错
				//WebUtils.ShowMessage(this.Page,"读取数据时出错！请重试。");
				DataGrid1.DataSource = null;
				DataGrid1.DataBind();
				return;
			}

			switch(WorkType)
			{
				case "task" :
					ShowTask(dt);
					break;
			}            
		}

		private void ShowTask(DataTable dt)
		{
			foreach(DataRow dr in dt.Rows)
			{
				dr.BeginEdit();
				dr["Furl2"]= "batchid=" + dr["FBatchID"].ToString()+"&onlyView=false";
				dr["FUrl"] = "batchid=" + dr["FBatchID"].ToString() + "&WorkType=" + WorkType + "&WeekIndex=" + WeekIndex
					+"&BeginDate="+ViewState["BeginDate"].ToString()+"&EndDate="+ViewState["EndDate"].ToString()
					+"&BankType="+ViewState["BankType"].ToString()+"&BatchState="+ViewState["BatchState"].ToString()
					+"&Proposer="+ViewState["Proposer"].ToString()+"&RefundPath="+ViewState["RefundPath"].ToString();
				dr["FBankTypeName"] = GetBankName(dr["FBankType"].ToString());
				dr.EndEdit();
			}
		
			DataGrid1.DataSource = dt.DefaultView;
			DataGrid1.DataBind();
		}
	}
}
