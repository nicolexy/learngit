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
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// SuspendSecondPasseword 的摘要说明。
	/// </summary>
	public partial class SuspendSecondPasseword : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
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

			if(!IsPostBack)
			{
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

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			string qqid = this.txtQQ.Text.Trim();
			//目前只支持QQ号，所以加判断
			if( qqid == "")
			{
				WebUtils.ShowMessage(this.Page,"请输入QQ号！");
				return;
			}

			try
			{
				Convert.ToInt64(qqid);
			}
			catch
			{
				WebUtils.ShowMessage(this.Page,"请输入正确QQ号！");
				return;
			}

			try
			{
				Table2.Visible = true;
				this.lblQQ.Text = qqid;

				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				if(qs.IsSecondPasseword(qqid))
				{
					this.lblResult.Text = "设置了二次登录密码";
					//if(AllUserRight.ValidRight(Session["SzKey"].ToString(),Session["OperID"].ToString(),PublicRes.GROUPID, "DeleteCrt"))
					if(ClassLib.ValidateRight("DeleteCrt",this))
						this.btnSuspend.Visible = true;
					else
						this.btnSuspend.Visible = false;
				}
				else
				{
					this.lblResult.Text = "没有设置了二次登录密码";
					this.btnSuspend.Visible = false;
				}
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

		protected void btnSuspend_Click(object sender, System.EventArgs e)
		{
			string qqid = this.lblQQ.Text.Trim();
			//目前只支持QQ号，所以加判断
			if( qqid == "")
			{
				WebUtils.ShowMessage(this.Page,"请输入QQ号！");
				return;
			}

			try
			{
				Convert.ToInt64(qqid);
			}
			catch
			{
				WebUtils.ShowMessage(this.Page,"请输入正确QQ号！");
				return;
			}

			string msg ="";
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				if(qs.SuspendSecondPasseword(qqid,out msg))
				{
					WebUtils.ShowMessage(this.Page,"二次登录密码撤销成功！");
				}
				else
				{
					WebUtils.ShowMessage(this.Page,"撤销失败：" + msg);
				}
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + msg + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + msg + eSys.Message.ToString());
			}
		}


	}
}
