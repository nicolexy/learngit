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
	/// AppealSSetting 的摘要说明。
	/// </summary>
	public partial class AppealSSetting : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label_uid.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

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
        //    this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
		}
		#endregion

		private int GetCount()
		{
			string FUser = this.tb_user.Text.Trim();
			string FUserType = this.ddl_usertype.SelectedValue.Trim();
			string FState = this.ddl_state.SelectedValue.Trim();
			if(FUser == "") FUser = null;
			if(FUserType == "-1") FUserType = null;
			if(FState == "-1") FState = null;
			
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			int Count = qs.WS_AppealSQueryCount(null, FUserType, FUser, FState);

			this.lb_msg.Text = "记录条数：" + Count;
			return Count;
		}

		private void BindData(int index)
		{
			try
			{
				string FUser = this.tb_user.Text.Trim();
				string FUserType = this.ddl_usertype.SelectedValue.Trim();
				string FState = this.ddl_state.SelectedValue.Trim();
				if(FUser == "") FUser = null;
				if(FUserType == "-1") FUserType = null;
				if(FState == "-1") FState = null;
			
				int max = pager.PageSize;
				int start = max * (index-1) + 1;

				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				DataSet ds = qs.WS_AppealSQuery(null, FUserType, FUser, FState,start,max);
			
				// 显示增加转义字段
				DataColumn dc0 = new DataColumn("FUserTypeS",typeof(string));
				DataColumn dc1 = new DataColumn("FCycTypeS",typeof(string));
				DataColumn dc2 = new DataColumn("FCycS",typeof(string));
				DataColumn dc3 = new DataColumn("FCycNumberS",typeof(string));
				DataColumn dc4 = new DataColumn("FStateS",typeof(string));
				ds.Tables[0].Columns.Add(dc0);
				ds.Tables[0].Columns.Add(dc1);
				ds.Tables[0].Columns.Add(dc2);
				ds.Tables[0].Columns.Add(dc3);
				ds.Tables[0].Columns.Add(dc4);

				for( int i=0; i< ds.Tables[0].Rows.Count; i++)
				{				
					ds.Tables[0].Rows[i]["FCycTypeS"] = DicFCycType(ds.Tables[0].Rows[i]["FCycType"].ToString());
					ds.Tables[0].Rows[i]["FUserTypeS"] = DicFUserType(ds.Tables[0].Rows[i]["FUserType"].ToString());
					ds.Tables[0].Rows[i]["FCycS"] = DicFCyc(ds.Tables[0].Rows[i]["FCyc"].ToString());
					ds.Tables[0].Rows[i]["FCycNumberS"] = DicFCycNumber(ds.Tables[0].Rows[i]["FCycNumber"].ToString());
					ds.Tables[0].Rows[i]["FStateS"] = DicFState(int.Parse(ds.Tables[0].Rows[i]["FState"].ToString()));				
				}

				// 显示记录
				this.lb_msg.Text = "记录条数：" + ds.Tables[0].Rows.Count.ToString();
				this.DataGrid1.DataSource = ds;
				this.DataGrid1.DataBind();		
			}
			catch(Exception ex)
			{
				this.lb_msg.Text = ex.Message;
			}
		}



        //private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        //{
        //    string transaction_id = e.Item.Cells[0].Text.Trim();
        //    string draw_id = e.Item.Cells[1].Text.Trim();
        //    try
        //    {
        //        if (e.CommandName == "query")
        //            BindDataDetail(transaction_id, draw_id);
        //    }
        //    catch (Exception eSys)
        //    {
        //        WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message);
        //    }
        //}

        //public void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        //{
        //    object obj = e.Item.Cells[9].FindControl("queryButton");
        //    string isT0 = e.Item.Cells[7].Text.Trim();//如果T+0，则显示“编辑/查看”
        //    if (obj != null)
        //    {
        //        LinkButton lb = (LinkButton)obj;
        //        if (isT0 == "T+0")
        //        {
        //            lb.Visible = true;
        //        }
        //    }
        //}

        //private void BindDataDetail(string transaction_id, string draw_id)
        //{
        //    try
        //    {
        //            WebUtils.ShowMessage(this.Page, "没有查询到记录");
        //    }
        //    catch (Exception eSys)
        //    {
        //        WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message);
        //    }
        //}

		private string DicFCycNumber(string s)
		{
			if (s == null || s.Trim() == "")
			{
				return "";
			}
			else 
			{
				return "T+" + s;
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

		private string DicFCyc(string s)
		{
			if (s == null || s.Trim() == "")
			{
				return "";
			}
			else if (s.Trim() == "1")
			{
				return "天";
			}
			else if (s.Trim() == "2")
			{
				return "月";
			}			
			else 
			{
				return s;
			}
		}

		private string DicFUserType(string s)
		{
			if (s == null || s.Trim() == "")
			{
				return "";
			}
			else if (s.Trim() == "1")
			{
				return "个人";
			}
			else if (s.Trim() == "2")
			{
				return "商户";
			}			
			else 
			{
				return s;
			}
		}

		private string DicFCycType(string s)
		{
			if (s == null || s.Trim() == "")
			{
				return "";
			}
			else if (s.Trim() == "1")
			{
				return "结算周期";
			}
			else if (s.Trim() == "2")
			{
				return "提现周期";
			}
			else if (s.Trim() == "3")
			{
				return "暂缓生成收费实例";
			}
			else 
			{
				return s;
			}
		}

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

	}
}
