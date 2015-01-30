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

using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
	/// KFDiaryManagePage 的摘要说明。
	/// </summary>
	public partial class KFDiaryManagePage : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
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

		protected void btn_StartGrapDiary_Click(object sender, System.EventArgs e)
		{
			try
			{
				Check_WebService.Check_Service cs = new Check_WebService.Check_Service();

				if(!cs.SetSendLog(Session["OperID"].ToString()))
				{
					//throw new Exception("您没有权限开启日志发送功能或系统出现异常");
					WebUtils.ShowMessage(this,"您没有权限开启日志发送功能或系统出现异常");
				}
			}
			catch (System.Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
			
		}

		protected void btn_SendGrapedDiary_Click(object sender, System.EventArgs e)
		{
			try
			{
				Check_WebService.Check_Service cs = new Check_WebService.Check_Service();

                //if(!cs.SendLog(false))
                //{
                //    WebUtils.ShowMessage(this,"发送日志失败");
                //}
			}
			catch (System.Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
			
		}

		protected void btn_StopGrapDiary_Click(object sender, System.EventArgs e)
		{
			try
			{
				Check_WebService.Check_Service cs = new Check_WebService.Check_Service();

                //if(!cs.SendLog(true))
                //{
                //    WebUtils.ShowMessage(this,"发送日志失败");
                //}
			}
			catch (System.Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
			
		}
	}
}
