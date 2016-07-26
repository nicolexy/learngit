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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.DataAccess;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// UserBankAccountQuery ��ժҪ˵����
	/// </summary>
	public partial class UserBankAccountQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		protected System.Web.UI.WebControls.TextBox TextBox1;
		protected System.Web.UI.WebControls.Button Button1;
		protected System.Web.UI.WebControls.Label Label5_Area;
		protected System.Web.UI.WebControls.Label Label9_City;
		protected System.Web.UI.HtmlControls.HtmlTable Table1;

		public string iprov,icity;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			try
			{
				
				BindInfo(1,1);
			}
			catch
			{
                
				WebUtils.ShowMessage(this.Page,"���ݿ��޼�¼��");
			
				this.Label1_State.Text        = "";
				this.Label2_BankID.Text       = "";
				this.Label3_QQID.Text         = "";
				this.Label4_TrueName.Text     = "";
				
				this.Label6_LastIP.Text       = "";
				this.Label8_BankName.Text     = "";
				
				this.Label11_Modify_Time.Text = "";
				this.Label12_Memo.Text        = "";
				this.Label13_BankType.Text    = "";
			}
		}

		private void BindInfo(int istr,int imax)
		{
			try
			{
				Session["uid"].ToString();
			}
			catch
			{
				WebUtils.ShowMessage(this.Page,"��ʱ�������µ�½��");
			}
			if(Request.QueryString["iprov"]!=null)
			{
				iprov=Request.QueryString["iprov"].Trim();
			}
			if(Request.QueryString["icity"]!=null)
			{
				icity=Request.QueryString["icity"].Trim();
			}
			if(Request.QueryString["state"]!=null)
			{
				this.Label1_State.Text =Request.QueryString["state"].Trim();
			}
			if(Request.QueryString["bankid"]!=null)
			{
				this.Label2_BankID.Text   =Request.QueryString["bankid"].Trim();
			}
			if(Request.QueryString["trueName"]!=null)
			{
				this.Label4_TrueName.Text    =Request.QueryString["trueName"].Trim();
			}
			if(Request.QueryString["LastIP"]!=null)
			{
				this.Label6_LastIP.Text     =Request.QueryString["LastIP"].Trim();
			}
			if(Request.QueryString["BankName"]!=null)
			{
				this.Label8_BankName.Text    =Request.QueryString["BankName"].Trim();
			}
			if(Request.QueryString["Modify_Time"]!=null)
			{
				this.Label11_Modify_Time.Text   =Request.QueryString["Modify_Time"].Trim();
			}
			if(Request.QueryString["Memo"]!=null)
			{
				this.Label12_Memo.Text    =Request.QueryString["Memo"].Trim();
			}
			if(Request.QueryString["BankType"]!=null)
			{
				this.Label13_BankType.Text    =Request.QueryString["BankType"].Trim();
			}
			if(Request.QueryString["compayname"]!=null)
			{
				this.lbCompay.Text     =Request.QueryString["compayname"].Trim();
			}
			if(Request.QueryString["accCreate"]!=null)
			{
				this.lbAccCreate.Text    =Request.QueryString["accCreate"].Trim();
			}

			if (Label1_State.Text.Trim() == "����")
			{
				this.lkb_acc.Text = "����";  //����ʻ�Ϊ���������Խ��ж������
			}
			else if (Label1_State.Text.Trim() == "����")
			{
				this.lkb_acc.Text = "�ⶳ";   //����ʻ�Ϊ���ᣬ����Խ��нⶳ����
			}
			this.Label3_QQID.Text         = Session["QQID"].ToString().Trim();
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

		protected void lkb_acc_Click(object sender, System.EventArgs e)
		{
			if (Label1_State.Text.Trim() == "����")
			{
				
				Response.Write("<script language=javascript>window.parent.location='freezeBankAcc.aspx?id=true&uid=" + this.Label3_QQID.Text.Trim() +"'</script>"); 
			}
			else if (Label1_State.Text.Trim() == "����")
			{
				Response.Write("<script language=javascript>window.parent.location='freezeBankAcc.aspx?id=false&uid=" + this.Label3_QQID.Text.Trim() +"'</script>"); 
			}
			
		}
	}
}
