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
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// SuspendSecondPasseword ��ժҪ˵����
	/// </summary>
	public partial class SuspendSecondPasseword : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				Table2.Visible = false;				
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

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			string qqid = this.txtQQ.Text.Trim();
			//Ŀǰֻ֧��QQ�ţ����Լ��ж�
			if( qqid == "")
			{
				WebUtils.ShowMessage(this.Page,"������QQ�ţ�");
				return;
			}

			try
			{
				Convert.ToInt64(qqid);
			}
			catch
			{
				WebUtils.ShowMessage(this.Page,"��������ȷQQ�ţ�");
				return;
			}

			try
			{
				Table2.Visible = true;
				this.lblQQ.Text = qqid;

				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				if(qs.IsSecondPasseword(qqid))
				{
					this.lblResult.Text = "�����˶��ε�¼����";
					//if(AllUserRight.ValidRight(Session["SzKey"].ToString(),Session["OperID"].ToString(),PublicRes.GROUPID, "DeleteCrt"))
					if(ClassLib.ValidateRight("DeleteCrt",this))
						this.btnSuspend.Visible = true;
					else
						this.btnSuspend.Visible = false;
				}
				else
				{
					this.lblResult.Text = "û�������˶��ε�¼����";
					this.btnSuspend.Visible = false;
				}
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

		protected void btnSuspend_Click(object sender, System.EventArgs e)
		{
			string qqid = this.lblQQ.Text.Trim();
			//Ŀǰֻ֧��QQ�ţ����Լ��ж�
			if( qqid == "")
			{
				WebUtils.ShowMessage(this.Page,"������QQ�ţ�");
				return;
			}

			try
			{
				Convert.ToInt64(qqid);
			}
			catch
			{
				WebUtils.ShowMessage(this.Page,"��������ȷQQ�ţ�");
				return;
			}

			string msg ="";
			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				if(qs.SuspendSecondPasseword(qqid,out msg))
				{
					WebUtils.ShowMessage(this.Page,"���ε�¼���볷���ɹ���");
				}
				else
				{
					WebUtils.ShowMessage(this.Page,"����ʧ�ܣ�" + msg);
				}
			}
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"���÷������" + msg + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + msg + eSys.Message.ToString());
			}
		}


	}
}
