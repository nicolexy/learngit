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
using TENCENT.OSS.CFT.KF.KF_Web;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// ChildrenOrderFromQuery 的摘要说明。
	/// </summary>
	public partial class ChildrenOrderFromQuery : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

                if (!classLibrary.ClassLib.ValidateRight("ChangeUserInfo", this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
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

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(this.tbFlistid.Text.Trim() == "")
				{
					throw new Exception("请输入订单号!");
				}
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				DataSet ds = qs.GetChildrenFlistList(this.tbFlistid.Text.Trim(),this.ddlFcurtype.SelectedValue);
				if(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					string Fstate = ds.Tables[0].Rows[0]["Fstate"].ToString();
					if(Fstate == "1")
						this.lblFstate.Text = "支付中";
					else if(Fstate == "2")
						this.lblFstate.Text = "支付成功";
					else if(Fstate == "3")
						this.lblFstate.Text = "确认收货";
					else if(Fstate == "4")
						this.lblFstate.Text = "转入退款";
					else
						this.lblFstate.Text = "Unknow-" + Fstate;

					this.lblFmodify_time.Text = ds.Tables[0].Rows[0]["Fmodify_time"].ToString();
					this.tbFmemo.Text = ds.Tables[0].Rows[0]["Fmemo"].ToString();
				}
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message);
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,eSys.Message);
			}
		}
	}
}
