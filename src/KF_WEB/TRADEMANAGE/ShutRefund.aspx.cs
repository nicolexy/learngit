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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// ShutRefund 的摘要说明。
	/// </summary>
	public partial class ShutRefund : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				// if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SPInfoManagement", this)) Response.Redirect("../login.aspx?wh=1");

				if(!IsPostBack)
				{
					this.btnApply.Attributes["onClick"] = "if(!confirm('确定要申请关闭退款吗？')) return false;"; 
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

		protected void btnApply_Click(object sender, System.EventArgs e)
		{
			try
			{
				string strszkey = Session["SzKey"].ToString().Trim();
				int ioperid = Int32.Parse(Session["OperID"].ToString());
				int iserviceid = TENCENT.OSS.CFT.KF.Common.AllUserRight.GetServiceID("InfoCenter") ;
				string struserdata = Session["uid"].ToString().Trim();
				string content = struserdata + "执行了[申请关闭退款]操作,操作对象[" + this.txtFspid.Text.Trim()
					+ "]时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

				Common.AllUserRight.UpdateSession(strszkey,ioperid,PublicRes.GROUPID,iserviceid,struserdata,content);

				string log = SensitivePowerOperaLib.MakeLog("add",Session["uid"].ToString().Trim(),"[申请关闭退款]",
					this.txtFspid.Text.Trim(),Session["uid"].ToString(),this.txtReason.Text.Trim());

				if(!SensitivePowerOperaLib.WriteOperationRecord("InfoCenter",log,this))
				{
					
				}
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

			try
			{
				if(this.txtFspid.Text.Trim() == "")
				{
					throw new Exception("请输入商户号!");
				}
				if(this.txtReason.Text.Trim() == "")
				{
					throw new Exception("请输入原因!");
				}

				//Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				//qs.ShutRefund(this.txtFspid.Text.Trim(),Session["uid"].ToString(),this.txtReason.Text.Trim());
                SPOA_Service.SPOA_Service spoaService = new TENCENT.OSS.CFT.KF.KF_Web.SPOA_Service.SPOA_Service();
                string spoa_ret = spoaService.CloseRefund(this.txtFspid.Text.Trim(), Session["uid"].ToString(), this.txtReason.Text.Trim());
                if (spoa_ret == "0")
                {

                }
                else
                {
                    throw new Exception("关闭退款错误：" + spoa_ret);
                }
			}
			catch(SoapException eSoap)
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message);
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,eSys.Message);
			}
		}
	}
}
