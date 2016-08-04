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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using System.Web.Mail;
using System.IO;
using CFT.CSOMS.BLL.TransferMeaning;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// QQReclaim 的摘要说明。
	/// </summary>
	public partial class QQReclaim : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

				if(!classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

				btnSend.Attributes.Add("onclick","return CheckEmail()");

				if(!IsPostBack)
				{
					this.rbtnQQ.Checked = true;
				}
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
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

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			try
			{
				string strszkey = Session["SzKey"].ToString().Trim();
				int ioperid = Int32.Parse(Session["OperID"].ToString());
				int iserviceid = TENCENT.OSS.CFT.KF.Common.AllUserRight.GetServiceID("InfoCenter") ;
				string struserdata = Session["uid"].ToString().Trim();
				string content = struserdata + "执行了[QQ帐号回收]操作,操作对象[" + " "
					+ "]时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

				Common.AllUserRight.UpdateSession(strszkey,ioperid,PublicRes.GROUPID,iserviceid,struserdata,content);

				string log = SensitivePowerOperaLib.MakeLog("get",struserdata,"[QQ帐号回收]",this.txtQQ.Text.Trim());

				if(!SensitivePowerOperaLib.WriteOperationRecord("InfoCenter",log,this))
				{
					
				}
			}
			catch(Exception err)
			{
				this.lblBalance.Text = "";
				this.lblReclaimTime.Text = "";
				this.btnSend.Enabled = false;
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

			try
			{
				if(this.rbtnQQ.Checked) //普通帐号
				{
					Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
					DataSet ds = qs.GetQQReclaimRecord(this.txtQQ.Text.Trim());

					if(ds == null || ds.Tables.Count<1 || ds.Tables[0].Rows.Count != 1)
					{
						throw new Exception("该QQ帐号不处于回收中状态！");					
					}
					else
					{
						//this.lblBalance.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fmemo"].ToString());
						string fmemo = ds.Tables[0].Rows[0]["Fmemo"].ToString();

						char[] splitChars = new char[]{'0','1','2','3','4','5','6','7','8','9'};
						int index1 = fmemo.IndexOfAny(splitChars);
						int index2 = fmemo.LastIndexOfAny(splitChars);
						string strFen = fmemo.Substring(index1,(index2-index1+1));

						this.lblBalance.Text = classLibrary.setConfig.FenToYuan(strFen);

						this.lblReclaimTime.Text = ds.Tables[0].Rows[0]["Fcreate_time"].ToString();
						this.btnSend.Enabled = true;
						ViewState["IsQQ"] = "yes";
						ViewState["Fuid"] = ds.Tables[0].Rows[0]["Fuid"].ToString();
						ViewState["QQid"] = this.txtQQ.Text.Trim();
						ViewState["Fexpired_time"] = ds.Tables[0].Rows[0]["Fcreate_time"].ToString();
					}
				}
				else    //靓号
				{
					if(IsNormalAccount(this.txtQQ.Text.Trim()))
					{
						Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
						DataSet ds = qs.GetReclaimRecord(this.txtQQ.Text.Trim());

						if(ds == null || ds.Tables.Count<1 || ds.Tables[0].Rows.Count != 1)
						{
							throw new Exception("该QQ帐号不处于回收中状态！");					
						}
						else
						{
							this.lblBalance.Text = MoneyTransfer.FenToYuan(ds.Tables[0].Rows[0]["Fbalance"].ToString());
							this.lblReclaimTime.Text = ds.Tables[0].Rows[0]["Fexpired_time"].ToString();
							this.btnSend.Enabled = true;
							ViewState["IsQQ"] = "no";
							ViewState["QQid"] = this.txtQQ.Text.Trim();
							ViewState["Fexpired_time"] = ds.Tables[0].Rows[0]["Fexpired_time"].ToString();
						}
					}
					else
					{
						throw new Exception("该帐户处于冻结状态！");	
					}
				}
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				this.lblBalance.Text = "";
				this.lblReclaimTime.Text = "";
				this.btnSend.Enabled = false;
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				this.lblBalance.Text = "";
				this.lblReclaimTime.Text = "";
				this.btnSend.Enabled = false;
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message.ToString());
			}
		}

		private bool IsNormalAccount(string QQid)   //判断该帐户是否处于正常状态.
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataSet ds = qs.GetUserAccountMain(QQid,1,1);

			if(ds == null || ds.Tables.Count<1 || ds.Tables[0].Rows.Count<1)
			{
				throw new Exception("数据库无此记录");
			}
			else
			{
                string Flag = Transfer.accountState(ds.Tables[0].Rows[0]["Fstate"].ToString());

				if (Flag == "正常")
				{
					return true;
				}
				else if (Flag == "冻结")
				{
					return false;
				}
				else
				{
					throw new Exception("该帐户处于异常状态！");
				}
			}
		}

		protected void btnSend_Click(object sender, System.EventArgs e)
		{
			try
			{
				SendEmail(this.txtEmail.Text.Trim());
				WebUtils.ShowMessage(this.Page,"邮件发送成功！");
			}
			catch(SoapException eSoap)
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				string errStr = PublicRes.GetErrorMsg(eSys.Message.ToString());
				WebUtils.ShowMessage(this.Page,"邮件发送失败！" + errStr);
			}
		}

		private bool SendEmail(string email)
		{
			try
			{
				TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend newMail=new TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend();
				newMail.SendMail(email,"","更换新的财付通账户名",GetOutMailContent(),true,null);
				return true;
			}
			catch(Exception err)
			{
				throw new Exception(err.Message);
			}			
		}

		private string GetOutMailContent()
		{
			string pattern = "";
			StreamReader sr = new StreamReader(Server.MapPath(Request.ApplicationPath).Trim().ToLower() 
				+ "\\Email\\QQReclaim.htm",System.Text.Encoding.GetEncoding("GB2312"));
			try
			{
				pattern = sr.ReadToEnd();
			}
			finally
			{
				sr.Close();
			}

			string TimeToTick,md5str,md5value,URL;
			if(ViewState["IsQQ"].ToString() == "yes")
			{
				TimeToTick = GetTickFromTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				md5str = "uin="+ViewState["QQid"].ToString()+"&uid="+ViewState["Fuid"].ToString()+"&tmstmp="+TimeToTick+"&md5_mask=CREPLACERETRIEVEACC_MD5_MASK";
				md5value = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(md5str,"MD5").ToLower();
				URL = "https://www.tenpay.com/cgi-bin/v1.0/replace_retrieved_acc_pre.cgi?uin="+ViewState["QQid"].ToString()+"&uid="+ViewState["Fuid"].ToString()+"&tmstmp=" + TimeToTick + "&skey=" + md5value;
			}
			else
			{
				string szkey = Session["SzKey"].ToString().Trim();
				int operid = Int32.Parse(Session["OperID"].ToString().Trim());
				string loginname = Session["uid"].ToString().Trim();
				string ip = Request.UserHostAddress.Trim();
				TimeToTick = GetTickFromTime(ViewState["Fexpired_time"].ToString());
				md5str = "uin="+ViewState["QQid"].ToString()+"&tmstmp="+TimeToTick+"&key=41A1A0B41CC27030827FF19486417ADA";
				md5value = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(md5str,"MD5").ToLower();
				URL = "http://www.tenpay.com/cgi-bin/v1.0/replace_acc_pre.cgi?uin=" + ViewState["QQid"].ToString() +
					"&tmstmp=" + TimeToTick + "&skey=" + md5value;
			}
			pattern = String.Format(pattern,ViewState["QQid"].ToString(),URL,URL);
			return pattern;
		}

		private string GetTickFromTime(string time)
		{
			if(time == null || time.Trim() == "")
			{
				return "";
			}

			long ltick = 0;
			try
			{
				if(time.Length == 8)
					time = time.Substring(0,4)+"-"+time.Substring(4,2)+"-"+time.Substring(6,2);
				ltick = DateTime.Parse(time).Ticks;
			}
			catch
			{
				return "";
			}

			ltick = ltick - DateTime.Parse("1970-01-01").Ticks;
			ltick = ltick / 10000000;
			return ltick.ToString();
		}
		
	}
}
