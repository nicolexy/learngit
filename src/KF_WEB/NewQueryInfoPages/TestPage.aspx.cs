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

using TENCENT.OSS.CFT.KF.KF_Web.Control;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;


namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
	/// TestPage ��ժҪ˵����
	/// </summary>
	public partial class TestPage : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			//WebUtils.ShowMessage(this,"this is a msg");

			// �ڴ˴������û������Գ�ʼ��ҳ��
			//Response.Write("<script language=javascript>alert(" + this.CheckBoxList1.SelectedIndex.ToString() + ")</script>");
			this.CheckBoxList1.SelectedIndexChanged +=new EventHandler(CheckBoxList1_SelectedIndexChanged);
			this.cb_1.CheckedChanged += new EventHandler(cb_1_CheckedChanged);
			this.btn_submit.Click += new EventHandler(btn_submit_Click);

			this.lb_text1.Text = "<br> -1";

			this.lb_text1.Text += "<br/> �������뱣�������ѱ���գ����½���������������뱣�����⣡";

			try
			{
				WebUtils.ShowMessage(this,Session["test"].ToString());
			}
			catch
			{
				WebUtils.ShowMessage(this,"test is null");
			}	

			if(!IsPostBack)
			{
			
			}
			this.ddl_1.SelectedIndexChanged += new EventHandler(ddl_1_SelectedIndexChanged);
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

		private void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
		{
			Response.Write("<script language=javascript>alert('fired')</script>");
			if(this.CheckBoxList1.SelectedIndex == 1)
			{
				if(this.CheckBoxList1.Items[1].Selected == true)
				{
					this.cbxl_1.Visible = true;
				}
				else
				{
					this.cbxl_1.Visible = false;
				}
			}
		}

		private void cb_1_CheckedChanged(object sender, EventArgs e)
		{
			Response.Write("<script language=javascript>alert(" + this.CheckBoxList1.SelectedIndex.ToString() + ")</script>");
			if(this.cb_1.Checked)
			{
				this.cbxl_1.Visible = true;
			}
			else
			{
				this.cbxl_1.Visible = false;
			}
		}


		protected void btn_submit_Click(object sender, EventArgs e)
		{
			string reason = "",otherReason = "";
			string showReason = "",showOtherReason = "";
			for(int i=0;i<7;i++)
			{
				UserAppealCheckControl control = (UserAppealCheckControl)this.FindControl("Userappealcheckcontrol" + i);

				if(control != null)
				{
					control.GetRejectReason(out reason,out otherReason);
					showReason += i.ToString() + reason;
					showOtherReason += i.ToString() + otherReason;
				}
			}

			this.ShowMsg(showReason + showOtherReason);
		}


		private void ShowMsg(string msg)		{			Response.Write("<script language=javascript>alert('" + msg + "')</script>");		}

		protected void btn_addFastReply_Click(object sender, System.EventArgs e)
		{
			getData.AddFreezeFastReplay(this,"��ӵĿ�ݻظ�\n");

			this.ddl_1.Items.Clear();

			string [] strList = getData.GetFreezeFastReplay(this,false);

			if(strList != null)
			{
				this.ddl_1.Items.Clear();
				foreach(string str in strList)
				{
					if(str != null && str.Trim() != "")
					{
						this.ddl_1.Items.Add(str);
					}
				}
			}
		}


		private void ddl_1_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.tbx_1.Text += this.ddl_1.SelectedItem.Text;
		}

		protected void btn_updateConfigFile_Click(object sender, System.EventArgs e)
		{
			try
			{
				System.Configuration.ConfigurationManager.AppSettings["isTestingMode"] = "false";
			}
			catch (System.Exception ex)
			{
				WebUtils.ShowMessage(this,ex.Message);
			}
			
		}
	}
}
