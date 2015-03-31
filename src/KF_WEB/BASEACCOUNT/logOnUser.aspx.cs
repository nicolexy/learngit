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
using System.Xml.Schema;
using System.Xml;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.Check_WebService;
using TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService;
using System.IO;
using CFT.CSOMS.BLL.BalanceModule;
using System.Configuration;
using CFT.CSOMS.BLL.TradeModule;


namespace TENCENT.OSS.C2C.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// logOnUser 的摘要说明。
	/// </summary>
	public partial class logOnUser : System.Web.UI.Page
	{
        protected BalaceService balaceService = new BalaceService();
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
			ButtonEndDate.Attributes.Add("onclick", "openModeEnd()"); 

			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"CancelAccount")) Response.Redirect("../login.aspx?wh=1");

                if (!ClassLib.ValidateRight("CancelAccount", this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if (!Page.IsPostBack)
			{
				BindHistoryInfo("","",DateTime.Parse("1970-01-01 00:00:00"),DateTime.Now,0,10);
				this.TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
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

		public bool BindHistoryInfo(string qqid,string handid,DateTime bgDateTime,DateTime edDateTime,int pageIndex,int pageSize)
		{
			string Msg    = "";
			try
			{
				TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService.Finance_Manage fm = new TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService.Finance_Manage();
				DataSet ds =  fm.logOnUserHistory(bgDateTime,edDateTime,qqid,handid,pageIndex,pageSize,out Msg);  //默认显示最新20条
			
				this.dgInfo.DataSource = ds;
				this.dgInfo.DataBind();

				return true;
			}
			catch(Exception e)
			{
				WebUtils.ShowMessage(this.Page,Msg + e.Message);
				return false;
			}
		}


		/// <summary>
		/// 提交销户申请。先验证帐户是否存在
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btLogOn_Click(object sender, System.EventArgs e)  //暂不开放注销功能
		{
            // qs1.ClosedBalancePaid(this.TextBox1_QQID.Text.Trim());//测试用关闭余额支付功能，为了查询及打开
			string qqid   = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceSqlStr(this.TextBox1_QQID.Text);
			string reason = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceSqlStr(this.txtReason.Text);
            bool emailCheck = EmailCheckBox.Checked;
            string emailAddr = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceSqlStr(this.txtEmail.Text);

            if (emailCheck && TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.CheckEmail(emailAddr))
            {
                WebUtils.ShowMessage(this.Page, "请输入正确格式的用户邮箱地址");
                return;
            }

			string memo   = "[注销QQ号码:" + qqid + "]注销原因:" + reason;

			string Msg = "";

			try
			{
				Finance_Manage fm = new Finance_Manage();
				if (!fm.checkUserReg(qqid,out Msg))
				{
					Msg = "帐号非法或者未注册！";
					WebUtils.ShowMessage(this.Page,Msg);
					return;
				}

				Check_Service cs = new Check_Service();
				TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Finance_Header fh = setConfig.setFH_CheckService(this);
				cs.Finance_HeaderValue = fh;

				Param[] myParams = new Param[4];

				string fqqid = this.txbConfirmQ.Text.Trim();
				myParams[0] = new Param();
				myParams[0].ParamName = "fqqid";
				myParams[0].ParamValue = fqqid;

				myParams[1] = new Param();
				myParams[1].ParamName = "Memo";
				myParams[1].ParamValue = memo;

				myParams[2] = new Param();
				myParams[2].ParamName = "returnUrl";
				myParams[2].ParamValue = "/BaseAccount/InfoCenter.aspx?id=" + qqid;

				myParams[3] = new Param();
				myParams[3].ParamName = "fuser";
				myParams[3].ParamValue = Session["uid"].ToString();

				string mainID     = DateTime.Now.ToString("yyyyMMdd") + qqid;
				string checkType  = "logonUser";

                Query_Service qs = new Query_Service();
                TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Finance_Header fh2 = setConfig.setFH(this);
                qs.Finance_HeaderValue = fh2;

                if (255 < reason.Length)
                {
                    WebUtils.ShowMessage(this.Page, "备注字数不得超过255个字符");
                    return;
                }

                //是否有未完成的交易单
                if (qs.LogOnUsercheckOrder(fqqid, "1"))
                {
                    WebUtils.ShowMessage(this.Page, "有未完成的交易单");
                    return;
                }
               
                if (qs.LogOnUserCheckYDT(fqqid, "1"))//是否开通一点通
                {
                    WebUtils.ShowMessage(this.Page, "开通了一点通");
                    return;
                }

                //下面的流程
               // 1:判断金额大于阀值,发起一个审批
               //2:金额小于时,插入logonhistory表,调用接口注销,如果有邮箱,向邮箱发送邮件,反馈信息给一线人员.

				long balance=0;//就是金额
				
				DataSet ds1 = qs.GetChildrenInfo(fqqid,"1");//主帐户余额
				if(ds1!=null && ds1.Tables.Count>0 && ds1.Tables[0].Rows.Count>0)
				{
					balance+=long.Parse(ds1.Tables[0].Rows[0]["Fbalance"].ToString().Trim());
				}
				ds1 = qs.GetChildrenInfo(fqqid,"80");//游戏子帐户
				if(ds1!=null && ds1.Tables.Count>0 && ds1.Tables[0].Rows.Count>0)
				{
					balance+=long.Parse(ds1.Tables[0].Rows[0]["Fbalance"].ToString().Trim());
				}
				ds1 = qs.GetChildrenInfo(fqqid,"82");//直通车子帐户
				if(ds1!=null && ds1.Tables.Count>0 && ds1.Tables[0].Rows.Count>0)
				{
					balance+=long.Parse(ds1.Tables[0].Rows[0]["Fbalance"].ToString().Trim());
				}

                //这部分逻辑先注释掉：
                //当用户没有关于余额支付的功能时（没有打开也没有关闭时）zw_prodatt_query_service返回错误
                //线上打开余额支付也掉不通，可能是策略问题
                //string ip = fh2.UserIP;
                //bool isOpen = balaceService.BalancePaidOrNotQuery(fqqid, ip);//查询余额支付功能关闭与否
                //if (balance <= 200000 && !isOpen)//如余额支付功能已闭且余额小于2000元以下自动调接口打开余额支付的功能
                //{
                //    balaceService.OpenBalancePaid(qqid,ip);
                //}
                //if (balance > 200000 && !isOpen)//因为该情况不打开余额支付，就不能注销，为了系统性能速度
                //{
                //    WebUtils.ShowMessage(this.Page, "余额大于2000元，且余额支付为关闭状态，不能销户！");
                //    return;
                //}

                if (balance < 5000)//系统自动注销
                {
                    if (!qs.LogOnUserDeleteUser(qqid, reason, Label1.Text, "", out Msg))
                    {
                        throw new Exception(Msg);

                    }
                    //系统自动注销成功给用户发邮件
                    if (emailCheck)
                    {
                     SendEmail(emailAddr,qqid,"系统自动销户");
                    }
                    WebUtils.ShowMessage(this.Page, "系统自动销户成功！");
                    return;
                }
                else
                {
                    cs.StartCheck(mainID, checkType, memo, MoneyTransfer.FenToYuan(balance.ToString()), myParams);
                    WebUtils.ShowMessage(this.Page, "销户申请提请成功！");
                    return;
                }
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"销户操作申请失败：" + errStr);
				return;
			}
			catch(Exception err)
			{
				//Msg += "销户操作申请异常！" + Common.CommLib.commRes.replaceHtmlStr(err.Message);
				Msg += "销户操作申请异常！" + TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceHtmlStr(err.Message);
				WebUtils.ShowMessage(this.Page,Msg);
				return;
			}

			this.btLogOn.Enabled = false;
		}

        private bool SendEmail(string email, string qqid,string subject)
        {
            try
            {
                /*
                TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend newMail = new TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend();
                
                string pattern = "";
                string filename = Server.MapPath(Request.ApplicationPath).Trim().ToLower() + "\\Email\\DelUserYes.htm";

                StreamReader sr = new StreamReader(filename, System.Text.Encoding.GetEncoding("GB2312"));
                try
                {
                    pattern = sr.ReadToEnd();
                }
                finally
                {
                    sr.Close();
                }

                pattern = String.Format(pattern, qqid,DateTime.Now, "", "" ,"");
                newMail.SendMail(email, "", subject, pattern, true, null);
                */
                string str_params = "p_name=" + qqid + "&p_parm1=" + DateTime.Now + "&p_parm2=" + "" + "&p_parm3=" + "" + "&p_parm4=" + "系统自动销户";
                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(email, "2034", str_params);
                return true;
            }
            catch (Exception err)
            {
                throw new Exception("给用户发邮件出错："+err.Message);
            }
        }

		protected void btQuery_Click(object sender, System.EventArgs e)
		{
			DateTime bgTime;
			DateTime edTime;

			try
			{
				bgTime = DateTime.Parse(this.TextBoxBeginDate.Text);	
			}
			catch
			{
				WebUtils.ShowMessage(this.Page,"起始日期格式不正确!默认为1970年1月1日");
				this.TextBoxBeginDate.Text = "1970-01-01";
				bgTime = DateTime.Parse("1970-01-01 00:00:00");
			}
			
			try
			{
				edTime = DateTime.Parse(this.TextBoxEndDate.Text);	
			}
			catch
			{
				WebUtils.ShowMessage(this.Page,"结束日期格式不正确!默认为当天");
				this.TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
				edTime = DateTime.Now;
			}
			
			string qqid   = this.TxbQueryQQ.Text.Trim();
			string handid = this.txbHandID.Text.Trim();
            
            //增加微信相关查询 yinhuang 2014/2/20
            string wx_qq = this.tbWxQQ.Text.Trim();
            string wx_email = this.tbWxEmail.Text.Trim();
            string wx_phone = this.tbWxPhone.Text.Trim();

            try
            {
                string queryType = string.Empty;
                string id = string.Empty;
                if (!string.IsNullOrEmpty(wx_qq))
                {
                    queryType = "QQ";
                    id = wx_qq;
                }
                else if (!string.IsNullOrEmpty(wx_phone))
                {
                    queryType = "Mobile";
                    id = wx_phone;
                }
                else if (!string.IsNullOrEmpty(wx_email))
                {
                    queryType = "Email";
                    id = wx_email;
                }

                if (!string.IsNullOrEmpty(queryType)) 
                {
                    string openID = string.Empty, errorMessage = string.Empty;
                    int errorCode = 0;
                    var IPList = ConfigurationManager.AppSettings["WeChat"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int j = 0; j < IPList.Length; j++)
                    {
                        if (PublicRes.getOpenIDFromWeChat(queryType, id, out openID, out errorCode, out errorMessage, IPList[j]))
                        {
                            break;
                        }
                    }
                    if (errorCode == 0)
                    {
                        qqid = openID + "@wx.tenpay.com";
                    }
                    else if (errorCode == 1)
                    {
                        throw new Exception("没有此用户");
                    }
                    else
                    {
                        throw new Exception(errorCode + errorMessage);
                    }
                }
            }
            catch (Exception err) 
            {
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }
            

			BindHistoryInfo(qqid,handid,bgTime,edTime,0,1000); //默认查询1000条
		}

		protected void lkHistoryQuery_Click(object sender, System.EventArgs e)
		{
			this.TABLE3.Visible = true;
		}

        protected void CheckBox1_CheckedChanged(object sender, System.EventArgs e)
        {
           
        }

        
	}
}

