using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;

namespace KF_Web 
{
	/// <summary>
	/// Global 的摘要说明。
	/// </summary>
	public class Global : System.Web.HttpApplication
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)
		{
            CFT.Apollo.Logging.LogHelper.LogInfo("Application_Start--站点启动", "Global");
		}
 
		protected void Session_Start(Object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_Error(Object sender, EventArgs e)
		{
            if (Context != null)
            {
                HttpContext ctx = HttpContext.Current;
                Exception ex = ctx.Server.GetLastError();
                if (ex != null)
                {
                    string error = TENCENT.OSS.CFT.KF.KF_Web.PageBase.GetRequestError(ctx);
                    CFT.Apollo.Logging.LogHelper.LogError(string.Format("Application_Error 获取到异常：\r\n {0} \r\n请求详细信息:{1}", ex.ToString(), error), "Global");
                }
            }
		}

		protected void Session_End(Object sender, EventArgs e)
		{

		}

		protected void Application_End(Object sender, EventArgs e)
		{

            CFT.Apollo.Logging.LogHelper.LogInfo("Application_End--站点关闭。", "Global");
		}
			
		#region Web 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    
			this.components = new System.ComponentModel.Container();
		}
		#endregion
	}
}

