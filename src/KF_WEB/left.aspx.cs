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



namespace TENCENT.OSS.CFT.KF.KF_Web

{

	/// <summary>

	/// Left ��ժҪ˵����

	/// </summary>

	public partial class Left : System.Web.UI.Page

	{

		protected void Page_Load(object sender, System.EventArgs e)

		{

			// �ڴ˴������û������Գ�ʼ��ҳ��

			try

			{
				Session["uid"].ToString();
			}

			catch  //SessionΪ�գ�����ת

			{

//				Response.Redirect("login.aspx?wh=1");

				Response.Write("��ʱ��������<a herf = 'login.aspx' target = 'parent'> ��¼</a>��");

			}

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

	}

}

