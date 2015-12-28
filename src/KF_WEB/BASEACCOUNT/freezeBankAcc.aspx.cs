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
using CFT.CSOMS.BLL.FreezeModule;
using CFT.Apollo.Logging;
using CFT.CSOMS.BLL.CFTAccountModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// freezeBankAcc 的摘要说明。
	/// </summary>
	public partial class freezeBankAcc : System.Web.UI.Page
	{

		private string sign;
		protected System.Web.UI.WebControls.ImageButton ButtonBeginDate;
		private string uid;
	
		//christy:解冻拍拍简化版帐号时，用户姓名和联系方式是空白显灰无法解冻.所以把用户姓名和联系方式这2个必填项的判断去掉
		protected void Page_Load(object sender, System.EventArgs e)
		{
			//furion 20050906 不修改以前的任何功能，只加入自已新的东西。
			// 在此处放置用户代码以初始化页面

			this.cbx_showEndDate.CheckedChanged += new EventHandler(cbx_showEndDate_CheckedChanged);

			try
			{
				if(System.Configuration.ConfigurationManager.AppSettings["isTestingMode"].ToString().ToLower() == "false")
				{
					labUid.Text = Session["uid"].ToString();
					string szkey = Session["SzKey"].ToString();
					//int operid = Int32.Parse(Session["OperID"].ToString());
					//if (!AllUserRight.ValidRight(szkey, operid, PublicRes.GROUPID, "FreezeUser")) Response.Redirect("../login.aspx?wh=1");						
					if(!classLibrary.ClassLib.ValidateRight("FreezeUser",this)) Response.Redirect("../login.aspx?wh=1");
				}
			}
			catch  //如果没有登陆或者没有权限就跳出
			{
				Response.Redirect("../login.aspx?wh=1");
			} 

			if (!Page.IsPostBack)
			{
				ViewState["showEndDate"] = "0";

                if (System.Configuration.ConfigurationManager.AppSettings["isTestingMode"].ToString().ToLower() == "false")//测试时false改为true
				{
					sign   = Request.QueryString["id"].ToString();
					uid = Request.QueryString["uid"].ToString();
                    string iswechat = Request.QueryString["iswechat"].ToString();

					//furion 20050906 上面这种局部变量的做法不行，必须用ViewState。
					ViewState["sign"] = sign;
					ViewState["uid"] = uid;
					//furion end.

                    //yinhuang 2013/8/15 增加对微信的处理
                    ViewState["iswechat"] = iswechat;
                    ViewState["tuserName"] = "";
					BindInfo();
				}
			}
		}

		private void BindInfo()
		{
			//绑定信息
			sign = ViewState["sign"].ToString();
			uid = ViewState["uid"].ToString();
			
			if (sign.ToLower() == "true")  //如果是正常帐户，进行冻结操作
			{
				//冻结操作
				Label1_state.Text     = "冻结账户";
				this.BT_F_Or_Not.Text = "冻结账户";

				//furion 20050906
				Label_listID.Text = uid;
				labReason.Text = "冻结原因";            
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
				Label1_state.Text     = "解冻帐户";
				this.BT_F_Or_Not.Text = "解冻帐户";

				//furion 20050906
				labReason.Text = "解冻原因"; 				
				tbUserName.Enabled = false;				
				tbContact.Enabled = false;
                ddlFreezeChannel.Enabled = false;
				Label_listID.Text = uid;

				//读取出原来提交的用户姓名和联系方式和帐户号码。
				Query_Service.Query_Service fm = new Query_Service.Query_Service();
				Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
				fm.Finance_HeaderValue = fh;
				
				try
				{
					//FFreezeType: 1为冻结帐户，2为锁定工单
					Query_Service.FreezeInfo fi = fm.GetExistFreeze(uid,1);
                    if (fi != null)
                    { //如果没有冻结记录的，也能解冻
                        ViewState["fid"] = fi.fid;

                        tbUserName.Text = (string.IsNullOrEmpty(fi.username)) ? "<无用户姓名>" : fi.username;
                        tbContact.Text = (string.IsNullOrEmpty(fi.contact)) ? "<无联系方式>" : fi.contact;
                        //ddlFreezeChannel.Text = (fi.FFreezeChannel == "" || fi.FFreezeChannel == null) ? "<无解冻渠道>" : fi.FFreezeChannel;
                        if (fi.FFreezeChannel == null || fi.FFreezeChannel == "" || fi.FFreezeChannel == "0")
                        {
                            ddlFreezeChannel.Items.Add(new ListItem("无冻结渠道", "0"));
                            ddlFreezeChannel.SelectedValue = "0";
                        }
                        else
                        {
                            ddlFreezeChannel.SelectedValue = fi.FFreezeChannel;
                        }
                        ViewState["tuserName"] = (string.IsNullOrEmpty(fi.username)) ? "" : fi.username;
                        ViewState["freezeChannel"] = ddlFreezeChannel.SelectedValue; //解冻渠道
                    }
                    else 
                    {
                        ViewState["fid"] = ""; 
                        ViewState["tuserName"] = "";
                        ViewState["freezeChannel"] = "";
                    }
					
				}
				catch
				{
					ViewState["fid"] = "";   //如果不是数据异常,说明是QQ简化注册的
                    ViewState["tuserName"] = "";
                    ViewState["freezeChannel"] = "";
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
			
			if(Session["uid"] == null)
				Response.Redirect("../login.aspx?wh=1"); //重新登陆

			sign = ViewState["sign"].ToString();
			uid = ViewState["uid"].ToString();

			try
			{
				string strszkey = Session["SzKey"].ToString().Trim();
				int ioperid = Int32.Parse(Session["OperID"].ToString());
				int iserviceid = Common.AllUserRight.GetServiceID("FreezeUser") ;
				string struserdata = Session["uid"].ToString().Trim();
				string content = struserdata + "执行了[冻结QQ帐户]操作,操作对象[" + uid
					+ "]时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

				Common.AllUserRight.UpdateSession(strszkey,ioperid,PublicRes.GROUPID,iserviceid,struserdata,content);

				Finance_Manage fm = new Finance_Manage();
				Finance_ManageService.Finance_Header fh = classLibrary.setConfig.setFH_Finance(this);
				fm.Finance_HeaderValue = fh;
                string op_type = "";//理财通账户状态操作类型

				//调用冻结或者解冻帐户的service
				if (sign.ToLower() == "true")  //如果是正常帐户，进行冻结操作
                {
                    #region
                    op_type = "1";
					bool exeSign = false;
					if (Request.QueryString["type"] == null)
					{
						exeSign = fm.freezeAccount(uid,1);	
					}
					else 
					{
                        string uname = "";
                        if (ViewState["tuserName"] != null && ViewState["tuserName"].ToString() != "")
                        {
                            uname = ViewState["tuserName"].ToString();
                        }
                        if (ViewState["iswechat"].ToString() == "true")
                        {
                            //微信处理流程 ,yinhuang 使用接口实现
                            //exeSign = fm.FreezePerAccountWechat(uid, 1);
                            exeSign = fm.FreezePerAccountWechat_New(uid, uname, ddlFreezeChannel.SelectedValue);
                        }
                        else {
                            //冻结 1 ui_freeze_user_service
                            exeSign = fm.freezePerAccount(uid, 1, uname, ddlFreezeChannel.SelectedValue);
                        }
					}

					if (false == exeSign)
					{
						WebUtils.ShowMessage(this.Page,"账户冻结失败！如果为商户的QQ号，没有冻结权限。请联系系统管理员。");
						return;
					}
					else
					{
                        WebUtils.ShowMessage(this.Page,"账户冻结成功！");	
					}

					//furion 20050906 要先加入工单，不成功不进行下面的工作。
					Query_Service.Query_Service qs = new Query_Service.Query_Service();
					Query_Service.Finance_Header fhq = classLibrary.setConfig.setFH(this);
					qs.Finance_HeaderValue = fhq;

					try
					{
						Query_Service.FreezeInfo fi = new FreezeInfo();
						fi.FFreezeID = uid;
						fi.FFreezeType = 1;
						fi.username = tbUserName.Text.Trim();
						fi.contact = tbContact.Text.Trim();
						fi.FFreezeReason = tbMemo.Text.Trim();
                        fi.FFreezeChannel = ddlFreezeChannel.SelectedValue;

						if(this.cbx_showEndDate.Checked)
						{
							DateTime endDate;

							try
							{
								endDate = DateTime.Parse(this.tbx_FreezeEndDate.Text);
								fi.strFreezeEndDate = endDate.ToString("yyyyMMdd");
							}
							catch
							{
								WebUtils.ShowMessage(this.Page,"请输入正确的日期格式");
								return;
							}
						}
						else
						{
							fi.strFreezeEndDate = "";
						}

						fi.username = classLibrary.setConfig.replaceMStr(fi.username);
						fi.contact = classLibrary.setConfig.replaceMStr(fi.contact);
						fi.FFreezeReason = classLibrary.setConfig.replaceMStr(fi.FFreezeReason);

						string log = classLibrary.SensitivePowerOperaLib.MakeLog("edit",struserdata,"[冻结QQ帐户]",
							fhq.UserName,fhq.UserIP,fhq.OperID.ToString(),fhq.SzKey,fi.FFreezeID,fi.FFreezeType.ToString(),
							fi.username,fi.contact,fi.FFreezeReason);

						if(!classLibrary.SensitivePowerOperaLib.WriteOperationRecord("FreezeUser",log,this))
						{
							
						}
                        
						qs.CreateNewFreeze(fi);
					}
					catch
					{
						WebUtils.ShowMessage(this.Page,"创建冻结工单时失败！");
					//	return;
					}
					//furion end

                    if (exeSign) 
                    {
                        //冻结成功，发送微信消息 yinhuang 2014/09/23
                        //发微信冻结消息
                        if (uid.IndexOf("@wx.tenpay.com") > 0)
                        {
                            string reqsource = "bus_kf_freeze";
                            string accid = uid.Substring(0, uid.IndexOf("@wx.tenpay.com"));
                            string templateid = "Td2l1120f5TCN9Ap2R3yWLhVS7yy41U379MZudwmiH0";
                            string cont1 = "你的微信支付账户已成功开启保护模式，账户暂不可用。";
                            string cont2 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            string cont3 = "请点击详情恢复至正常模式";
                            string msgtype = "freeze";
                            try
                            {
                                //为不影响线上，暂不处理异常
                                new FreezeService().SendWechatMsg(reqsource, accid, templateid, cont1, cont2, cont3, msgtype);
                            }
                            catch {
                                WebUtils.ShowMessage(this.Page, accid + "发微信冻结消息异常");
                            }
                        }
                    }
                    #endregion
                }
				else if (sign.ToLower() == "false")
                {
                    #region 
                    op_type = "2";
                    //解冻需要根据冻结渠道判断是否有权限 yinhuang 2013/12/9
                    string isChannel = "";
                    if (ViewState["freezeChannel"] != null && ViewState["freezeChannel"].ToString() != "") 
                    {
                        isChannel = ViewState["freezeChannel"].ToString();
                    }

                    string val = "";
                    string des = "";

                    if (isChannel != "" && isChannel != "0")
                    {
                        //如果为空,不需要进行权限判断;不为空,则需要进行权限判断.
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
                    }
                    else 
                    {
                        val = "UnFreezeChannelFK";
                        des = "风控冻结";
                    }

                    if (val != "" && !classLibrary.ClassLib.ValidateRight(val, this))
                    {
                        //进行权限判断
                        WebUtils.ShowMessage(this.Page, "没有解冻冻结渠道为[" + des + "]的权限！");
                        return;
                    }

					Query_Service.Query_Service qs = new Query_Service.Query_Service();
					Query_Service.Finance_Header fhq = classLibrary.setConfig.setFH(this);
					qs.Finance_HeaderValue = fhq;

					//先判断余额，如果超过，发起审批，原来的处理放在审批完成环节。
					long UNFreeze_BigMoney = long.Parse(System.Configuration.ConfigurationManager.AppSettings["UNFreeze_BigMoney"]);

					string Msg = "";
					long userbalance = 0;
                    if (ViewState["iswechat"].ToString() == "false")
                    {
                        userbalance = qs.GetUserBalance(uid, 1, out Msg);
                    }
                    
					if(userbalance < 0)
					{
						WebUtils.ShowMessage(this.Page,"查询用户余额失败！" + classLibrary.setConfig.replaceHtmlStr(Msg));
						return;
					}

					if(userbalance >= UNFreeze_BigMoney)
					{
						//发起审批后结束。
						Check_WebService.Check_Service cs = new Check_WebService.Check_Service();
						Check_WebService.Finance_Header fhc = classLibrary.setConfig.setFH_CheckService(this);

						cs.Finance_HeaderValue = fhc;

						Check_WebService.Param[] myparam = new Check_WebService.Param[8];

						myparam[0] = new Check_WebService.Param();
						myparam[0].ParamName = "uid";
						myparam[0].ParamValue = uid;

						myparam[1] = new Check_WebService.Param();
						myparam[1].ParamName = "mediflag";
						myparam[1].ParamValue = "false";

						myparam[2] = new Check_WebService.Param();
						myparam[2].ParamName = "username";
						myparam[2].ParamValue = fhc.UserName;

						myparam[3] = new Check_WebService.Param();
						myparam[3].ParamName = "userip";
						myparam[3].ParamValue = fhc.UserIP;

						myparam[4] = new Check_WebService.Param();
						myparam[4].ParamName = "type";
						myparam[4].ParamValue = "2";

						myparam[5] = new Check_WebService.Param();
						myparam[5].ParamName = "handleresult";
						myparam[5].ParamValue = classLibrary.setConfig.replaceSqlStr(tbMemo.Text.Trim());
						

						myparam[6] = new Check_WebService.Param();
						myparam[6].ParamName = "fid";
						myparam[6].ParamValue = ViewState["fid"].ToString();

						string returnUrl = "/BaseAccount/FreezeDetail.aspx?fid=" + ViewState["fid"].ToString();
						myparam[7] = new Check_WebService.Param();
						myparam[7].ParamName = "returnUrl";
						myparam[7].ParamValue = returnUrl;

						string fmemo = "解冻大金额用户：" + uid + "，金额为：" + MoneyTransfer.FenToYuan(userbalance);

						string mainId    = DateTime.Now.ToString("yyyyMMddHHmmssfff");
						cs.StartCheck(mainId,"UNFreezeCheck",fmemo,MoneyTransfer.FenToYuan(userbalance.ToString()),myparam);

						WebUtils.ShowMessage(this.Page,"解冻账户余额较大，发起审批成功！");
						this.Button1_back.Visible = true;
						this.BT_F_Or_Not.Visible = false;
						return;
					}

                    if (Request.QueryString["type"] == null)
                    { 
                        fm.freezeAccount(uid, 2); 
                    }
                    else
                    {
                        string uname = "";
                        if (ViewState["tuserName"] != null && ViewState["tuserName"].ToString() != "")
                        {
                            uname = ViewState["tuserName"].ToString();
                        }

                        if (ViewState["iswechat"].ToString() == "true")
                        {
                            //微信处理流程
                            //fm.FreezePerAccountWechat(uid, 2);
                            fm.UnFreezePerAccountWechat_New(uid, uname);
                        }
                        else
                        {
                            //解冻 2 ui_unfreeze_user_service
                            fm.freezePerAccount(uid, 2, uname, "");
                        }
                    }

					if(ViewState["fid"] != null && ViewState["fid"].ToString() != "")  //如果不是数据异常,为空说明是QQ简化注册的,不用走这
					{
						try
						{
							Query_Service.FreezeInfo fi = new FreezeInfo();
							fi.fid = ViewState["fid"].ToString();
							fi.FHandleResult = tbMemo.Text.Trim();
							fi.FFreezeType = 1;
							fi.FHandleResult = classLibrary.setConfig.replaceMStr(fi.FHandleResult);
							qs.UpdateFreezeInfo(fi);
						}
						catch
						{
							WebUtils.ShowMessage(this.Page,"处理冻结工单时失败！");
						//	return;
						}
                    }
                    #endregion
                }

                //理财通账户冻结或解冻操作
                try
                {
                    AccountService acc = new AccountService();
                    string ip = Request.UserHostAddress.ToString();
                    if (ip == "::1")
                        ip = "127.0.0.1";
                    Boolean state = acc.LCTAccStateOperator(uid, op_type, Session["uid"].ToString(), ip);
                }
                catch (Exception err)
                {
                    string errStr = PublicRes.GetErrorMsg(err.Message.ToString());
                    WebUtils.ShowMessage(this.Page, "操作理财通账户状态失败！" + errStr);
                    return;
                }

				this.Button1_back.Visible = true;
				this.BT_F_Or_Not.Visible = false;
			}
			catch(SoapException er) //捕获soap类
			{
				string str = PublicRes.GetErrorMsg(er.Message.ToString());
				WebUtils.ShowMessage(this.Page,"查询错误："+ str);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"操作失败！请重试："+ eSys.Message.ToString());
			}	
		}

		protected void Button1_back_Click(object sender, System.EventArgs e)
		{
			if(Session["uid"] == null)
				Response.Redirect("../login.aspx?wh=1"); //重新登陆
			
			uid = ViewState["uid"].ToString();

			if (Request.QueryString["type"] == null)
				Response.Write("<script language=javascript>window.parent.WorkArea.location='UserBankInfoQuery.aspx?id=" + uid + " '</script>"); 
			else
				Response.Write("<script language=javascript>window.location='InfoCenter.aspx?id=" + uid + " '</script>"); 
		}

		private void cbx_showEndDate_CheckedChanged(object sender, EventArgs e)
		{
			
		}
	}
}
