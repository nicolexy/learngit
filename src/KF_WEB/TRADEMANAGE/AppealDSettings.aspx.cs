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
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// AppealDSettings 的摘要说明。
	/// </summary>
	public partial class AppealDSettings : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label_uid.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SPInfoManagement", this)) Response.Redirect("../login.aspx?wh=1");

			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if (!IsPostBack)
			{
				pager.RecordCount= GetCount();
				BindData(1);
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
			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

		}
		#endregion

		protected void btn_query_Click(object sender, System.EventArgs e)
		{
			pager.RecordCount= GetCount();
			BindData(1);
		}

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			try
			{
				pager.CurrentPageIndex = e.NewPageIndex;
				BindData(e.NewPageIndex);
			}
			catch(SoapException eSoap)
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message);
			}
		}

		private int GetCount()
		{
			string FSpid = this.tb_spid.Text.Trim();
			string FUser = this.tb_user.Text.Trim();
			int FPriType = int.Parse(this.ddl_pritype.SelectedValue.Trim());
			int FState   = int.Parse(this.ddl_state.SelectedValue.Trim());

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			int Count = qs.WS_AppealDQueryCount(FSpid, FUser, FPriType, FState);

			this.lb_msg.Text = "记录条数：" + Count;

			return Count;
		}

		private void BindData(int index)
		{
			try
			{
				string FSpid = this.tb_spid.Text.Trim();
				string FUser = this.tb_user.Text.Trim();
				int FPriType = int.Parse(this.ddl_pritype.SelectedValue.Trim());
				int FState   = int.Parse(this.ddl_state.SelectedValue.Trim());

				int max = pager.PageSize;
				int start = max * (index-1) + 1;

				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				DataSet ds = qs.WS_AppealDQuery(FSpid, FUser, FPriType, FState, start,max);
					
				DataColumn dc0 = new DataColumn("FPriTypeS",typeof(string));
				DataColumn dc1 = new DataColumn("FAmountSetS",typeof(string));		
				DataColumn dc2 = new DataColumn("FStateS",typeof(string));

				ds.Tables[0].Columns.Add(dc0);
				ds.Tables[0].Columns.Add(dc1);
				ds.Tables[0].Columns.Add(dc2);
		
				for( int i=0; i< ds.Tables[0].Rows.Count; i++)
				{				
					ds.Tables[0].Rows[i]["FPriTypeS"] = DicFPriTypeS(int.Parse(ds.Tables[0].Rows[i]["FPriType"].ToString()));
					ds.Tables[0].Rows[i]["FAmountSetS"] = ds.Tables[0].Rows[i]["FAmountSet"].ToString();
					ds.Tables[0].Rows[i]["FStateS"] = DicFState(int.Parse(ds.Tables[0].Rows[i]["FState"].ToString()));					
				}

				this.DataGrid1.DataSource = ds.Tables[0].DefaultView;
				this.DataGrid1.DataBind();			
			}
			catch(Exception ex)
			{
				this.lb_msg.Text = ex.Message;
			}
		}

		private string DicFPriTypeS(int pritype)
		{
			switch(pritype)
			{
				case 1: return "结算金额全额转指定对方帐户";
				case 2: return "结算金额全额转银行帐户";
				case 3: return "结算金额全额留存本帐户";
				case 4: return "帐户余额全额转银行账户";
				case 5: return "帐户余额全额转指定对方帐户";
				case 6: return "帐户余额留存指定金额，其余转指定对方帐户";
				case 7: return "帐户余额留存指定金额，其余转银行账户";
				case 8: return "部分转对方帐户，使对方帐户至指定金额，剩余部分转银行账户";
				case 9: return "转指定金额至指定对方帐户";
				case 10: return "T+0提现";
				default: return "unknown";
			}
		}

		private string DicFState(int state)
		{
			switch(state)
			{
				case 1: return "商户申请";
				case 2: return "商户复核";
				case 3: return "财付通审批通过";
				case 4: return "拒绝";
				case 5: return "作废";
				case 6: return "过期";
				default: return "unknown";
			}
		}

	}
}
