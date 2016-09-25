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
            CFT.Apollo.Logging.LogHelper.LogInfo("=================Application_Start--վ������==========================", "Global");
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
                    CFT.Apollo.Logging.LogHelper.LogError(string.Format("=========Application_Error========== ��ȡ���쳣��\r\n {0} \r\n������ϸ��Ϣ:{1}", ex.ToString(), errorStr), "Global");
                }
                //����ȡ���쳣��¼�����
                Server.ClearError();
            }
        }

		protected void Session_End(Object sender, EventArgs e)
		{

		}

		protected void Application_End(Object sender, EventArgs e)
		{
            CFT.Apollo.Logging.LogHelper.LogInfo("=================Application_End--վ��ر�============,===վ��ر�ԭ��" + GetShutdownReson(System.Web.Hosting.HostingEnvironment.ShutdownReason.ToString()), "Global");
		}

        
        /// <summary>
        /// ��ȡϵͳ�ر�ԭ������
        /// </summary>
        private string GetShutdownReson(string sdKey)
        {
            string getValue = string.Empty;
            switch (sdKey)
            {
                case "BinDirChangeOrDirectoryRename":
                    getValue = "Bin �ļ����Ѹ��ģ������е��ļ��Ѹ��ġ�";
                    break;
                case "BrowsersDirChangeOrDirectoryRename":
                    getValue = "App_Browsers �ļ����Ѹ��ģ������е��ļ��Ѹ��ġ�";
                    break;
                case "BuildManagerChange":
                    getValue = "����ϵͳ�ر���Ӧ�ó�����.";
                    break;
                case "ChangeInGlobalAsax":
                    getValue = "Global.asax �ļ��Ѹ��ġ�";
                    break;
                case "ChangeInSecurityPolicyFile":
                    getValue = "������ʰ�ȫ�Բ����ļ��Ѹ��ġ�";
                    break;
                case "CodeDirChangeOrDirectoryRename":
                    getValue = "App_Code �ļ����Ѹ��ģ������е��ļ��Ѹ��ġ�";
                    break;
                case "ConfigurationChange":
                    getValue = "Ӧ�ó��������ļ��Ѹ��ġ�";
                    break;
                case "HostingEnvironment":
                    getValue = "���������ر���Ӧ�ó�����";
                    break;
                case "HttpRuntimeClose":
                    getValue = "�ѵ��� Close��";
                    break;
                case "IdleTimeout":
                    getValue = "�Ѵﵽ�����ʱ�����ơ�";
                    break;
                case "InitializationError":
                    getValue = "���� AppDomain ��ʼ������";
                    break;
                case "MaxRecompilationsReached":
                    getValue = "�Ѵﵽ��Դ�Ķ�̬���±�����������";
                    break;
                case "None":
                    getValue = "δ�ṩ�ر�ԭ��";
                    break;
                case "PhysicalApplicationPathChanged":
                    getValue = "Ӧ�ó��������·���Ѹ��ġ�";
                    break;
                case "ResourcesDirChangeOrDirectoryRename":
                    getValue = "App_GlobalResources �ļ����Ѹ��ģ������е��ļ��Ѹ��ġ�";
                    break;
                case "UnloadAppDomainCalled":
                    getValue = "�ѵ��� UnloadAppDomain��";
                    break;
                default:
                    getValue = "δ֪";
                    break;
            }

            var retString = string.Format("{0}:{1}", sdKey, getValue);
            return retString;

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

