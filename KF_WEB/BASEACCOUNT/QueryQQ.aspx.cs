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
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using System.Text.RegularExpressions;


namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// QueryQQ 的摘要说明。
	/// </summary>
	public partial class QueryQQ : System.Web.UI.Page
	{
		public    DataSet ds;   //数据源DataSet
		public    string  Msg;

		int       istr=0;
		int       pageSize = 20;     //Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"]);

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			//if (Session["uid"] == null || !AllUserRight.ValidRight(Session["szkey"].ToString(),Int32.Parse(Session["OperID"].ToString()),PublicRes.GROUPID, "UserReport")) Response.Redirect("../login.aspx?wh=1");

			if(Session["uid"] == null || !classLibrary.ClassLib.ValidateRight("UserReport",this)) Response.Redirect("../login.aspx?wh=1");

			pageSize = Int32.Parse(ddlPageSize.SelectedValue);


			if (!Page.IsPostBack)
			{
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
			this.AspNetPager1.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.AspNetPager1_PageChanged);

		}
		#endregion

		private bool bindaData()
		{

			if (txbPara.Text == "" && this.txbPara.Text.Trim() == "")
			{
				Msg = "没有输入查询的数据。";
				WebUtils.ShowMessage(this.Page,Msg);
				return false;
			}

			string strContent	= setConfig.replaceMStr(txbPara.Text.Trim());
			string whereStr		= "";
			string dbName		= "";

			int nCondition = int.Parse(ddlCondition.SelectedIndex.ToString());

			if (0 == nCondition)
			{

				if (!IsNumeric(strContent))
				{
					Msg = "您所输入的数据格式不对。";
					WebUtils.ShowMessage(this.Page,Msg);
					return false;
				}

				whereStr="where Fuid="		+ strContent;	//按内部ID
				int nInternalID		= int.Parse(strContent);
				string strDBName	= string.Concat("c2c_db_" + nInternalID%100);
				string strTbl		= string.Concat("t_user_" + (nInternalID/100)%10);
				dbName				= strDBName + "." + strTbl;

			}
			else
			{

				if (1 == nCondition)
				{
					if (!IsNumeric(strContent))
					{
						Msg = "您所输入的数据格式不对。";
						WebUtils.ShowMessage(this.Page,Msg);
						return false;
					}
						
					whereStr	= "where Fbankid='"	+ strContent + "'";	//按银行卡号
					dbName		= "c2c_analy_db.t_bank_user";
				}
				else if (2 == nCondition)
				{
					whereStr	= "where Fcreid='"	+ strContent + "'";	//按身份证号
					dbName		= "c2c_analy_db.t_user_info";
				}
				else if (3 == nCondition)
				{
					whereStr	= "where Ftruename='" + strContent + "'";//按姓名
					dbName		= "c2c_analy_db.t_user_info";
				}
				else
				{
					return false;
				}						
				
			}
		


			if (ViewState["newIndex"] != null)
				istr = Int32.Parse(ViewState["newIndex"].ToString());
			else
				istr = 0;

			Query_Service.Query_Service qs = new Query_Service.Query_Service();

			if (!qs.GetQQByType(nCondition, dbName, istr*pageSize, pageSize, whereStr, out ds,out Msg)) return false;

			if (ds == null || ds.Tables.Count == 0 ||ds.Tables[0].Rows.Count == 0) 
			{
				AspNetPager1.Visible = false;
				Page.DataBind();  //重新绑定 清除历史数据，避免引起误解

				Msg = "没有您选定范围内的数据。";
				WebUtils.ShowMessage(this.Page,Msg);
				return false;
			}

			AspNetPager1.PageSize    = pageSize;
			AspNetPager1.RecordCount = Int32.Parse(ds.Tables[0].Rows[0]["icount"].ToString());
			
			AspNetPager1.CustomInfoText ="记录总数：<font color=\"blue\"><b>"+AspNetPager1.RecordCount.ToString()+"</b></font>";
			AspNetPager1.CustomInfoText+=" 总页数：<font color=\"blue\"><b>" +AspNetPager1.PageCount.ToString()+"</b></font>";
			AspNetPager1.CustomInfoText+=" 当前页：<font color=\"red\"><b>"  +AspNetPager1.CurrentPageIndex.ToString()+"</b></font>";

			
			this.dgInfo.DataSource = ds.Tables[0].DefaultView;
			this.dgInfo.DataBind();

			return true;
		}


		public static bool IsNumeric(string value)
		{
			return Regex.IsMatch(value, @"^\d*$");
		}

		private void clickEvent()
		{
			ViewState["newIndex"] = null;  //如果重新点击一次查询，则清空查询的分页；否则无法查询到数据（比如单笔）
			AspNetPager1.Visible = true;
			this.AspNetPager1.CurrentPageIndex = 1;

		}

		private void AspNetPager1_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			istr = e.NewPageIndex;
			AspNetPager1.CurrentPageIndex = istr;

			ViewState["newIndex"] = e.NewPageIndex -1;

			bindaData();
		}

		protected void btQuery_Click(object sender, System.EventArgs e)
		{

			clickEvent();
			bindaData();
		}

	}
}



