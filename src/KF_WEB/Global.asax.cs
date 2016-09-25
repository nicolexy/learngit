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
            CFT.Apollo.Logging.LogHelper.LogInfo("=================Application_Start--站点启动==========================", "Global");
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
            Exception error = Server.GetLastError();
            if (error != null)
            {
                HttpContext ctx = HttpContext.Current;
                Exception ex = error.GetBaseException();

                if (ex != null)
                {
                    string errorStr = TENCENT.OSS.CFT.KF.KF_Web.PageBase.GetRequestError(ctx);
                    CFT.Apollo.Logging.LogHelper.LogError(string.Format("=========Application_Error========== 获取到异常：\r\n {0} \r\n请求详细信息:{1}", ex.ToString(), errorStr), "Global");
                }
                //将获取的异常记录并清空
                Server.ClearError();
            }
        }

		protected void Session_End(Object sender, EventArgs e)
		{

		}

		protected void Application_End(Object sender, EventArgs e)
		{
            CFT.Apollo.Logging.LogHelper.LogInfo("=================Application_End--站点关闭============,===站点关闭原因：" + GetShutdownReson(System.Web.Hosting.HostingEnvironment.ShutdownReason.ToString()), "Global");
		}

        
        /// <summary>
        /// 获取系统关闭原因释义
        /// </summary>
        private string GetShutdownReson(string sdKey)
        {
            string getValue = string.Empty;
            switch (sdKey)
            {
                case "BinDirChangeOrDirectoryRename":
                    getValue = "Bin 文件夹已更改，或其中的文件已更改。";
                    break;
                case "BrowsersDirChangeOrDirectoryRename":
                    getValue = "App_Browsers 文件夹已更改，或其中的文件已更改。";
                    break;
                case "BuildManagerChange":
                    getValue = "编译系统关闭了应用程序域.";
                    break;
                case "ChangeInGlobalAsax":
                    getValue = "Global.asax 文件已更改。";
                    break;
                case "ChangeInSecurityPolicyFile":
                    getValue = "代码访问安全性策略文件已更改。";
                    break;
                case "CodeDirChangeOrDirectoryRename":
                    getValue = "App_Code 文件夹已更改，或其中的文件已更改。";
                    break;
                case "ConfigurationChange":
                    getValue = "应用程序级配置文件已更改。";
                    break;
                case "HostingEnvironment":
                    getValue = "宿主环境关闭了应用程序域。";
                    break;
                case "HttpRuntimeClose":
                    getValue = "已调用 Close。";
                    break;
                case "IdleTimeout":
                    getValue = "已达到最长空闲时间限制。";
                    break;
                case "InitializationError":
                    getValue = "出现 AppDomain 初始化错误。";
                    break;
                case "MaxRecompilationsReached":
                    getValue = "已达到资源的动态重新编译最大次数。";
                    break;
                case "None":
                    getValue = "未提供关闭原因。";
                    break;
                case "PhysicalApplicationPathChanged":
                    getValue = "应用程序的物理路径已更改。";
                    break;
                case "ResourcesDirChangeOrDirectoryRename":
                    getValue = "App_GlobalResources 文件夹已更改，或其中的文件已更改。";
                    break;
                case "UnloadAppDomainCalled":
                    getValue = "已调用 UnloadAppDomain。";
                    break;
                default:
                    getValue = "未知";
                    break;
            }

            var retString = string.Format("{0}:{1}", sdKey, getValue);
            return retString;

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

