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
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// TranDetail ��ժҪ˵����
	/// </summary>
	public partial class TranDetail : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			try
			{

				labUid.Text = "ҳ���ѷ���";
				return;
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				
				string listid = Request.QueryString["listid"];
				int trantype = Int32.Parse(Request.QueryString["trantype"]);

				if(listid == null || listid.Trim() == "")
				{
					WebUtils.ShowMessage(this.Page,"��������");
				}


				try
				{
//					BindInfo(listid,trantype);
				}
				catch(LogicException err)
				{
					WebUtils.ShowMessage(this.Page,err.Message);
				}
				catch(SoapException eSoap) //����soap���쳣
				{
					string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
					WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
				}
				catch(Exception eSys)
				{
					WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
				}
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

		private void BindInfo(string appealid, int trantype)
		{			

		}

	}
}
