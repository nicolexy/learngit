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
	/// KFDiaryManagePage ��ժҪ˵����
	/// </summary>
	public partial class KFDiaryManagePage : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
		}

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
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
					//throw new Exception("��û��Ȩ�޿�����־���͹��ܻ�ϵͳ�����쳣");
					WebUtils.ShowMessage(this,"��û��Ȩ�޿�����־���͹��ܻ�ϵͳ�����쳣");
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
                //    WebUtils.ShowMessage(this,"������־ʧ��");
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
                //    WebUtils.ShowMessage(this,"������־ʧ��");
                //}
			}
			catch (System.Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
			
		}
	}
}
