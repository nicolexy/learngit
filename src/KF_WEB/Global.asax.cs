using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;

namespace KF_Web 
{
	/// <summary>
	/// Global ��ժҪ˵����
	/// </summary>
	public class Global : System.Web.HttpApplication
	{
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)
		{
            CFT.Apollo.Logging.LogHelper.LogInfo("Application_Start--վ������", "Global");
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
                    CFT.Apollo.Logging.LogHelper.LogError(string.Format("Application_Error ��ȡ���쳣��\r\n {0} \r\n������ϸ��Ϣ:{1}", ex.ToString(), error), "Global");
                }
            }
		}

		protected void Session_End(Object sender, EventArgs e)
		{

		}

		protected void Application_End(Object sender, EventArgs e)
		{

            CFT.Apollo.Logging.LogHelper.LogInfo("Application_End--վ��رա�", "Global");
		}
			
		#region Web ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    
			this.components = new System.ComponentModel.Container();
		}
		#endregion
	}
}

