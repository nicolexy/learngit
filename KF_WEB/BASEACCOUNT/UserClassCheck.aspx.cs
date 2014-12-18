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
using TENCENT.OSS.CFT.KF.Common;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;



namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// UserClassCheck 的摘要说明。
	/// </summary>
	public partial class UserClassCheck : System.Web.UI.Page
	{
		protected Control.UserClassCheckControl UserClassCheckControl0;
		protected Control.UserClassCheckControl UserClassCheckControl1;
		protected Control.UserClassCheckControl UserClassCheckControl2;
		protected Control.UserClassCheckControl UserClassCheckControl3;
		protected Control.UserClassCheckControl UserClassCheckControl4;
		protected Control.UserClassCheckControl UserClassCheckControl5;
		protected Control.UserClassCheckControl UserClassCheckControl6;
		protected Control.UserClassCheckControl UserClassCheckControl7;
		protected Control.UserClassCheckControl UserClassCheckControl8;
		protected Control.UserClassCheckControl UserClassCheckControl9;
		protected Control.UserClassCheckControl UserClassCheckControl10;
		protected Control.UserClassCheckControl UserClassCheckControl11;
		protected Control.UserClassCheckControl UserClassCheckControl12;
		protected Control.UserClassCheckControl UserClassCheckControl13;
		protected Control.UserClassCheckControl UserClassCheckControl14;
		protected Control.UserClassCheckControl UserClassCheckControl15;
		protected Control.UserClassCheckControl UserClassCheckControl16;
		protected Control.UserClassCheckControl UserClassCheckControl17;
		protected Control.UserClassCheckControl UserClassCheckControl18;
		protected Control.UserClassCheckControl UserClassCheckControl19;
		protected Control.UserClassCheckControl UserClassCheckControl20;
		protected Control.UserClassCheckControl UserClassCheckControl21;
		protected Control.UserClassCheckControl UserClassCheckControl22;
		protected Control.UserClassCheckControl UserClassCheckControl23;
		protected Control.UserClassCheckControl UserClassCheckControl24;
		protected Control.UserClassCheckControl UserClassCheckControl25;
		protected Control.UserClassCheckControl UserClassCheckControl26;
		protected Control.UserClassCheckControl UserClassCheckControl27;
		protected Control.UserClassCheckControl UserClassCheckControl28;
		protected Control.UserClassCheckControl UserClassCheckControl29;
		protected Control.UserClassCheckControl UserClassCheckControl30;
		protected Control.UserClassCheckControl UserClassCheckControl31;
		protected Control.UserClassCheckControl UserClassCheckControl32;
		protected Control.UserClassCheckControl UserClassCheckControl33;
		protected Control.UserClassCheckControl UserClassCheckControl34;
		protected Control.UserClassCheckControl UserClassCheckControl35;
		protected Control.UserClassCheckControl UserClassCheckControl36;
		protected Control.UserClassCheckControl UserClassCheckControl37;
		protected Control.UserClassCheckControl UserClassCheckControl38;
		protected Control.UserClassCheckControl UserClassCheckControl39;
		protected Control.UserClassCheckControl UserClassCheckControl40;
		protected Control.UserClassCheckControl UserClassCheckControl41;
		protected Control.UserClassCheckControl UserClassCheckControl42;
		protected Control.UserClassCheckControl UserClassCheckControl43;
		protected Control.UserClassCheckControl UserClassCheckControl44;
		protected Control.UserClassCheckControl UserClassCheckControl45;
		protected Control.UserClassCheckControl UserClassCheckControl46;
		protected Control.UserClassCheckControl UserClassCheckControl47;
		protected Control.UserClassCheckControl UserClassCheckControl48;
		protected Control.UserClassCheckControl UserClassCheckControl49;
		Query_Service.Query_Service qs = new Query_Service.Query_Service();
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			
			try
			{
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				// if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				ViewState["BeginDate"] = Convert.ToDateTime(Request["BeginDate"].ToString());
				ViewState["EndDate"] = Convert.ToDateTime(Request["EndDate"].ToString());
				ViewState["fstate"] = Request["fstate"].ToString();
				this.txtCount.Text = Request["Count"].ToString();
				ViewState["uid"] = Session["uid"].ToString();
				BindData();
			}
		}

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		private void BindData()
		{
			string msg = "";
			int TicketsCount;

			try
			{
				try
				{
					TicketsCount = int.Parse(this.txtCount.Text.Trim());
				}
				catch
				{
					throw new Exception("请输入正确的工单数！");
				}
				if(TicketsCount < 1)
				{
					throw new Exception("工单数不允许小于1！");
				}
				if(TicketsCount > 50)
				{
					throw new Exception("工单数的最大值为50！");
				}

				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				DataSet ds = qs.GetUserClassLockList(Convert.ToDateTime(ViewState["BeginDate"]),Convert.ToDateTime(ViewState["EndDate"]),int.Parse(ViewState["fstate"].ToString()),ViewState["uid"].ToString(),TicketsCount);
				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count ==0)
				{
					throw new Exception("没有单可领!");
				}
				
				for(int i=0;i<50;i++) //最大为50笔
				{
					System.Web.UI.Control ucl = FindControl("UserClassCheckControl" + i.ToString());
					if(ucl != null)
					{
						((Control.UserClassCheckControl)ucl).Clean();
						((Control.UserClassCheckControl)ucl).Visible = false;
					}
				}

				int TicketCount = ds.Tables[0].Rows.Count;

				for(int i=0;i<TicketCount;i++)
				{
					try
					{
						System.Web.UI.Control ucl = FindControl("UserClassCheckControl" + i.ToString());
						if(ucl != null)
						{
							((Control.UserClassCheckControl)ucl)._dr = ds.Tables[0].Rows[i];
							((Control.UserClassCheckControl)ucl).BindData();
							((Control.UserClassCheckControl)ucl).Visible = true;
						}
					}
					catch(Exception ex)
					{
						msg = ds.Tables[0].Rows[i]["Fqqid"].ToString() + ex.Message;
					}
					continue;
				}

				if(msg != "")
					WebUtils.ShowMessage(this.Page,"批量领单失败:" + msg);
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,"批量领单失败:" + ex.Message);
			}
		}

		private void Pass(Control.UserClassCheckControl Control,string UserName,out string msg)
		{
			msg = "";
			bool Flag = qs.UserClassConfirm(int.Parse(Control.flist_id),UserName,out msg);
			if(Flag)
			{
				msg = "";
			}
		}

		private void Cancel(Control.UserClassCheckControl Control,string UserName,out string msg)
		{
			msg = "";

			if(Control.cblRejectReason.SelectedIndex == -1)
			{
				msg = "请选择拒绝原因！";
				return;
			}
			if(Control.cblRejectReason.Items[5].Selected && Control.tbOtherReason == "")
			{
				msg = "请输入其它原因！";
				return;
			}
			string reason = "",OtherReason = "";
			for(int i=0; i<Control.cblRejectReason.Items.Count; i++)
			{
				if(i == 5)
					OtherReason = classLibrary.setConfig.replaceMStr(Control.tbOtherReason);
				else if(Control.cblRejectReason.Items[i].Selected)
				{
					reason += Control.cblRejectReason.Items[i].Text + "&";
				}
			}
			
			bool Flag = qs.UserClassCancel(int.Parse(Control.flist_id),reason,OtherReason,UserName,out msg);
			if(Flag)
			{
				msg = "";
			}
		}
		protected void btnSubmit_Click(object sender, System.EventArgs e)
		{
			//"1":通过"2":拒绝"3":挂起"
			string msg = "";
			string Error = "";
			string UserName = ViewState["uid"].ToString();

			for(int i=0;i<50;i++) //最大为50笔
			{
				System.Web.UI.Control ucl = FindControl("UserClassCheckControl" + i.ToString());

				try
				{
					if(ucl != null)
					{
						msg = "";

						if(((Control.UserClassCheckControl)ucl).Visible && ((Control.UserClassCheckControl)ucl).flist_id != "")
						{
							if(((Control.UserClassCheckControl)ucl).SubmitType == "1")
							{
								Pass(((Control.UserClassCheckControl)ucl),UserName,out msg);
							}
							else if(((Control.UserClassCheckControl)ucl).SubmitType == "2")
							{
								Cancel(((Control.UserClassCheckControl)ucl),UserName,out msg);
							}
							if(msg != "")
							{
								Error += ((Control.UserClassCheckControl)ucl).flist_id + " :" + msg;
							}
						}
					}
				}
				catch(Exception ex)
				{
					Error += ((Control.UserClassCheckControl)ucl).flist_id + " :" + ex.Message;
				}
				continue;
			}

			if(Error != "")
			{
				WebUtils.ShowMessage(this.Page,Error);
			}
		}

		protected void btnTicket_Click(object sender, System.EventArgs e)
		{
			BindData();
		}


	}
}
