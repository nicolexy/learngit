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

using TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// FreezeReason 的摘要说明。
	/// </summary>
	public partial class FreezeReason : System.Web.UI.Page
	{
	
		private string sign;
		//private string txtSign;
		private string listID;

		protected void Page_Load(object sender, System.EventArgs e)
		{	
			//furion 20050906 不修改以前的任何功能，只加入自已新的东西。
			// 在此处放置用户代码以初始化页面
			try
			{
				labUid.Text = Session["uid"].ToString();

				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "LockTradeList")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("LockTradeList",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch  //如果没有登陆或者没有权限就跳出
			{
				Response.Redirect("../login.aspx?wh=1");
			} 


			if (!Page.IsPostBack)
			{
				sign   = Request.QueryString["id"].ToString();
				listID = Request.QueryString["lsd"].ToString();

				

				//furion 20050906 上面这种局部变量的做法不行，必须用ViewState。
				ViewState["sign"] = sign;
				ViewState["listID"] = listID;
				//furion end.

				BindInfo();
			}			

		}

		private void BindInfo()
		{
			//绑定信息
			sign = ViewState["sign"].ToString();
			listID = ViewState["listID"].ToString();
			
			if (sign.ToLower() == "true")  //如果是正常帐户，进行冻结操作
			{
				//冻结操作
				Label1_state.Text     = "锁定交易单";
				this.BT_F_Or_Not.Text = "锁定交易单";

				//furion 20050906
				Label_listID.Text = listID;
				labReason.Text = "锁定原因";            
				tbUserName.Text = "";
				tbUserName.Enabled = true;
				tbContact.Text = "";
				tbContact.Enabled = true;
                ddlFreezeChannel.Enabled = true;
				//furion end
			}
			else if (sign.ToLower() == "false")   //如果是冻结账户，进行解冻操作
			{
				//解冻操作
				Label1_state.Text     = "解锁交易单";
				this.BT_F_Or_Not.Text = "解锁交易单";

				//furion 20050906
				labReason.Text = "解锁原因"; 				
				tbUserName.Enabled = false;				
				tbContact.Enabled = false;
                ddlFreezeChannel.Enabled = false;
				Label_listID.Text = listID;

				//读取出原来提交的用户姓名和联系方式和帐户号码。
				Query_Service.Query_Service fm = new Query_Service.Query_Service();
//				Query_Service.Finance_Header fh = new Query_Service.Finance_Header();
//				fh.UserName = Session["uid"].ToString();
//				fh.UserIP   = Request.UserHostAddress;
//				fh.OperID = Int32.Parse(Session["OperID"].ToString());
//				fh.SzKey = Session["SzKey"].ToString();
//				//fh.RightString = Session["key"].ToString();
//				fm.Finance_HeaderValue = fh;
				Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
				fm.Finance_HeaderValue = fh;
				try
				{
					//FFreezeType: 1为冻结帐户，2为锁定工单
					Query_Service.FreezeInfo fi = fm.GetExistFreeze(listID,2);
					ViewState["fid"] = fi.fid;

                    if (fi.FFreezeChannel == null || fi.FFreezeChannel == "" || fi.FFreezeChannel == "0")
                    {
                        ddlFreezeChannel.Items.Add(new ListItem("无冻结渠道", "0"));
                        ddlFreezeChannel.SelectedValue = "0";
                    }
                    else
                    {
                        ddlFreezeChannel.SelectedValue = fi.FFreezeChannel;
                    }
                    ViewState["freezeChannel"] = fi.FFreezeChannel; //解冻渠道

					tbUserName.Text = fi.username;
					tbContact.Text = fi.contact;
				}
				catch
				{
					ViewState["fid"] = "";
                    ViewState["freezeChannel"] = ""; //冻结渠道
				}
				//furion end
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


		protected void BT_F_Or_Not_Click(object sender, System.EventArgs e)
		{
			try
			{
				Session["uid"].ToString();
			}
			catch
			{
				Response.Write("<script>alert('登陆超时！请重新登陆。');</script>");
			}

			//绑定信息
			sign = ViewState["sign"].ToString();
			listID = ViewState["listID"].ToString();

			string strszkey = Session["SzKey"].ToString().Trim();
			int ioperid = Int32.Parse(Session["OperID"].ToString());
			int iserviceid = Common.AllUserRight.GetServiceID("LockTradeList") ;
			string struserdata = Session["uid"].ToString().Trim();
			string content = struserdata + "执行了[锁定交易单按钮]操作,操作对象[" + listID
				+ "]时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

			Common.AllUserRight.UpdateSession(strszkey,ioperid,PublicRes.GROUPID,iserviceid,struserdata,content);

			string log = classLibrary.SensitivePowerOperaLib.MakeLog("edit",Session["uid"].ToString().Trim(),"[锁定交易单按钮]",
				this.Label_listID.Text.Trim(),"1");

			if(!classLibrary.SensitivePowerOperaLib.WriteOperationRecord("LockTradeList",log,this))
			{
					
			}

			if (sign == "True")   
			{
				//冻结操作
				//furion 20050906 要先加入工单，不成功不进行下面的工作。
				Query_Service.Query_Service qs = new Query_Service.Query_Service();
				Query_Service.Finance_Header fhq = classLibrary.setConfig.setFH(this);
//				fhq.UserName = Session["uid"].ToString();
//				fhq.UserIP   = Request.UserHostAddress;
//				fhq.OperID = Int32.Parse(Session["OperID"].ToString());
//				fhq.SzKey = Session["SzKey"].ToString();
//				//fhq.RightString = Session["key"].ToString();
				qs.Finance_HeaderValue = fhq;
				try
				{
					Query_Service.FreezeInfo fi = new FreezeInfo();
					fi.FFreezeID = listID;
					fi.FFreezeType = 2;
					fi.username = tbUserName.Text.Trim();
					fi.contact = tbContact.Text.Trim();
					fi.FFreezeReason = tbMemo.Text.Trim();
                    fi.FFreezeChannel = ddlFreezeChannel.SelectedValue;
					qs.CreateNewFreeze(fi);
				}
				catch
				{
					WebUtils.ShowMessage(this.Page,"创建冻结工单时失败！");
					return;
				}
				//furion end

				TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService.Finance_Manage myService = new TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService.Finance_Manage();
				//myService.Finance_HeaderValue = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.setFH_Finance(Session["uid"].ToString(),Request.UserHostAddress);

				myService.Finance_HeaderValue = classLibrary.setConfig.setFH_Finance(this);

				myService.freezeTrade(this.Label_listID.Text.Trim(),"1");  //参数2表示交易单要更改成的状态 1 锁定 2 正常 3 作废

				Response.Write("<script>alert('锁定单据成功！');</script>");
				this.Button1_back.Visible = true;
				this.BT_F_Or_Not.Visible = false;
			}
			else if (sign == "False")                 
			{
                string isChannel = "";
                if (ViewState["freezeChannel"] != null && ViewState["freezeChannel"].ToString() != "")
                {
                    isChannel = ViewState["freezeChannel"].ToString();
                }

                if (isChannel != "" && isChannel != "0")
                {
                    //如果为空,不需要进行权限判断;不为空,则需要进行权限判断.
                    string val = "";
                    string des = "";
                    if (isChannel == "1" || isChannel == "6")
                    {
                        //风控渠道
                        val = "UnFreezeChannelFK";
                        des = "风控冻结";
                    }
                    else if (isChannel == "2")
                    {
                        //拍拍
                        val = "UnFreezeChannelPP";
                        des = "拍拍冻结";
                    }
                    else if (isChannel == "3")
                    {
                        //用户
                        val = "UnFreezeChannelYH";
                        des = "用户冻结";
                    }
                    else if (isChannel == "4")
                    {
                        //商户
                        val = "UnFreezeChannelSH";
                        des = "商户冻结";
                    }
                    else if (isChannel == "5")
                    {
                        //BG
                        val = "UnFreezeChannelBG";
                        des = "BG接口冻结";
                    }

                    if (val != "" && !classLibrary.ClassLib.ValidateRight(val, this))
                    {
                        //进行权限判断
                        WebUtils.ShowMessage(this.Page, "没有解冻冻结渠道为[" + des + "]的权限！");
                        return;
                    }
                }
                
                //furion 20050906 要先加入工单，不成功不进行下面的工作。
				Query_Service.Query_Service qs = new Query_Service.Query_Service();
				Query_Service.Finance_Header fhq = classLibrary.setConfig.setFH(this);
//				fhq.UserName = Session["uid"].ToString();
//				fhq.UserIP   = Request.UserHostAddress;
//				fhq.OperID = Int32.Parse(Session["OperID"].ToString());
//				fhq.SzKey = Session["SzKey"].ToString();
//				//fhq.RightString = Session["key"].ToString();
				qs.Finance_HeaderValue = fhq;

				try
				{
					Query_Service.FreezeInfo fi = new FreezeInfo();
					fi.fid = ViewState["fid"].ToString();
					fi.FHandleResult = tbMemo.Text.Trim();
					fi.FFreezeType = 2;
					qs.UpdateFreezeInfo(fi);
				}
				catch
				{
					WebUtils.ShowMessage(this.Page,"处理冻结工单时失败！");
					return;
				}
				//furion end

				//解冻操作
				TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService.Finance_Manage myService = new KF_Web.Finance_ManageService.Finance_Manage();
				//myService.Finance_HeaderValue = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.setFH_Finance(Session["uid"].ToString(),Request.UserHostAddress);

				myService.Finance_HeaderValue = classLibrary.setConfig.setFH_Finance(this);

				myService.freezeTrade(this.Label_listID.Text.Trim(),"2"); //参数2表示交易单要更改成的状态 1 锁定 2 正常 3 作废
				
				Response.Write("<script>alert('解除锁定成功！');</script>");
				this.Button1_back.Visible = true;
				this.BT_F_Or_Not.Visible = false;
//				Response.Write("<script>window.opener=null;window.close()</script>");
			}
		}

		protected void Button1_back_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("TradelogQuery.aspx?id=" + this.Label_listID.Text.ToString().Trim());
		}

	
	}
}
