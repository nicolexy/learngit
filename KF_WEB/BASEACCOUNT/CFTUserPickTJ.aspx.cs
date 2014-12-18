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
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Web.Services.Protocols;

using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;


namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// CFTUserPickTJ 的摘要说明。
	/// </summary>
	public partial class CFTUserPickTJ : System.Web.UI.Page
	{
	
		public string pagebegindate
		{
			get{return ViewState["begindate"].ToString();}
		}

		public string pageenddate
		{
			get{return ViewState["enddate"].ToString();}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
			ButtonEndDate.Attributes.Add("onclick", "openModeEnd()"); 

			try
			{
				Label1.Text = Session["uid"].ToString();

				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"CFTUserPickTJ")) Response.Redirect("../login.aspx?wh=1");

				if(!classLibrary.ClassLib.ValidateRight("CFTUserPickTJ",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				TextBoxBeginDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日");
				TextBoxEndDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");

				DateTime begindate;
				DateTime enddate;

				begindate = DateTime.Parse(TextBoxBeginDate.Text);
				enddate = DateTime.Parse(TextBoxEndDate.Text);

				ViewState["begindate"] = DateTime.Parse(begindate.ToString("yyyy-MM-dd 00:00:00"));
				ViewState["enddate"] = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59"));

				Table2.Visible = false;				
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

		private void ValidateDate()
		{
			DateTime begindate;
			DateTime enddate;

			try
			{
				begindate = DateTime.Parse(TextBoxBeginDate.Text);
				enddate = DateTime.Parse(TextBoxEndDate.Text);
			}
			catch
			{
				throw new Exception("日期输入有误！");
			}

			if(begindate.CompareTo(enddate) > 0)
			{
				throw new Exception("终止日期小于起始日期，请重新输入！");
			}

			ViewState["begindate"] = DateTime.Parse(begindate.ToString("yyyy-MM-dd 00:00:00"));
			ViewState["enddate"] = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59"));
		}


		public string GetUrl(int state, string user)
		{
			string stmp = "CftAppealQuery.aspx?state=" + state + "&begin=" + pagebegindate.Substring(0,10) 
				+ "&end=" + pageenddate.Substring(0,10) + "&user=" + user;
			return stmp;
		}

		private void BindData()
		{/* 页面已被废除 begin
			DateTime begindate = (DateTime)ViewState["begindate"];
			DateTime enddate = (DateTime)ViewState["enddate"];

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			Finance_Header fh = new Finance_Header();
			fh.UserIP = Request.UserHostAddress;
			fh.UserName = Session["uid"].ToString();

			fh.OperID = Int32.Parse(Session["OperID"].ToString());
			fh.SzKey = Session["SzKey"].ToString();
			//fh.RightString = Session["key"].ToString();

			qs.Finance_HeaderValue = fh;

			string msg = "";
			DataSet ds = qs.GetCFTUserPickTJ(begindate,enddate,out msg);

			if(ds != null && ds.Tables.Count >0)
			{
				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				throw new LogicException("没有找到记录！" + msg);
			}

			//再查询一下各统计信息.
			string[] countlist = qs.GetAppealSum(begindate,enddate);
			if(countlist != null || countlist.Length == 3)
			{
				labSumCount.Text = countlist[0].Trim();
				labHandleCount.Text = countlist[1].Trim();
				labothercount.Text = countlist[2].Trim();
			}
			 页面已被废除 end */
		}

		protected void btnTJ_Click(object sender, System.EventArgs e)
		{
			try
			{
				string strszkey = Session["SzKey"].ToString().Trim();
				int ioperid = Int32.Parse(Session["OperID"].ToString());
				int iserviceid = Common.AllUserRight.GetServiceID("CFTUserPickTJ") ;
				string struserdata = Session["uid"].ToString().Trim();
				string content = struserdata + "执行了[自助申诉统计]操作,操作对象[" + " "
					+ "]时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

				Common.AllUserRight.UpdateSession(strszkey,ioperid,PublicRes.GROUPID,iserviceid,struserdata,content);

				string log = SensitivePowerOperaLib.MakeLog("get",struserdata,"[自助申诉统计]","");

				if(!SensitivePowerOperaLib.WriteOperationRecord("CFTUserPickTJ",log,this))
				{
					
				}

				ValidateDate();
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

			try
			{
				Table2.Visible = true;
				BindData();
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message.ToString());
			}
		}
	}
}
