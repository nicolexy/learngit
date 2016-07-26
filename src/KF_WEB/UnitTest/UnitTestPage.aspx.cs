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


namespace TENCENT.OSS.CFT.KF.KF_Web.UnitTest
{
	/// <summary>
	/// UnitTestPage ��ժҪ˵����
	/// </summary>
	public partial class UnitTestPage : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

			if(classLibrary.getData.IsTestMode)
			{
				Session["uid"] = "1100000000";
				Session["SzKey"] = "123123242";
				Session["OperID"] = "12345";
				Session["QQID"] = "1100000000";
			}

			UT_CheckService.GetInstance(new EveryCheckFinHandler(FinEveryCheck)).DoAllCheck();
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


		public void UnitTest()
		{
			
		}


		private void FinEveryCheck(object _param1,object _param2)
		{
			string str = string.Format(@"{0} Test result:{1} <br/>",_param2.ToString(),_param1.ToString());

			Response.Write(str);
		}

	}
}
