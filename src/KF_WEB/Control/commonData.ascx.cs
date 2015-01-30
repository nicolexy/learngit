namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
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
	using System.Text;

	/// <summary>
	///		commonData 的摘要说明。
	/// </summary>
	public partial class commonData : System.Web.UI.UserControl
	{










		public    string  Msg;
		int       istr=0;
		int       pageSize = 20;
		public    DataSet ds;   //数据源DataSet
		Hashtable ht;

		protected string   queryType;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			// if (Session["uid"] == null || !AllUserRight.ValidRight(Session["szkey"].ToString(),Int32.Parse(Session["OperID"].ToString()),PublicRes.GROUPID, "HistoryModify")) Response.Redirect("../login.aspx?wh=1");
			if(Session["uid"] == null || !classLibrary.ClassLib.ValidateRight("HistoryModify",this)) Response.Redirect("../login.aspx?wh=1");
			pageSize = Int32.Parse(ddlPageSize.SelectedValue);
			
			string strbe = this.TextBoxBeginDate.ClientID;
			string stred = this.TextBoxEndDate.ClientID;

			ButtonBeginDate.Attributes.Add("onclick", "openModeBegin(" + strbe + ")"); 
			ButtonEndDate.Attributes.Add("onclick", "openModeEnd(" + stred + ")"); 

			ClientSubmitBind(this.txbCustom,this.btQuery);

			if (!Page.IsPostBack)
			{
				this.TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

				if (!bindaData())
				{
					return;
				}

				//邦定需要精确查询的字段和值
//				int i = ht.Count;
//				for (i=0;i<ht.Count;i++)
//				{
//					this.ddlCondition.Items.Add(new ListItem(ht[i].Keys.ToString(),"按" + ht.Values[i].ToString() + "查询"));	
//				}
				
				IEnumerator im = ht.GetEnumerator();
				im.MoveNext();
				Response.Write(im.Current.GetHashCode());

				foreach(string s in ht.Keys)
				{
					this.ddlCondition.Items.Add(new ListItem("按" + ht[s].ToString() + "查询",s));
				}

				this.ddlCondition.DataBind();
			}
	
			//Page.DataBind();
		}

		private bool bindaData()
		{
			//构造查询语句
			DateTime bgDate = DateTime.Now;
			DateTime edDate = DateTime.Now;
			string whereStr = "";   //条件判断语句
  
			try
			{
				bgDate = DateTime.Parse(this.TextBoxBeginDate.Text.Trim());
				edDate = DateTime.Parse(this.TextBoxEndDate.Text.Trim());
			}
			catch
			{
				Msg = "日期格式输入错误！请检查！";
				return false;
			}

			if (this.txbCustom.Text != "" && this.txbCustom.Text.Trim() != "")
			{
				whereStr = " and " + this.ddlCondition.SelectedValue + " = '" + classLibrary.setConfig.replaceMStr(this.txbCustom.Text.Trim()) + "' " ;  //特定查询
			}
			

			Query_Service.Query_Service qs = new Query_Service.Query_Service();

			if (ViewState["newIndex"] != null)
				istr = Int32.Parse(ViewState["newIndex"].ToString());
			else
				istr = 0;

			if (!qs.getQueryData(istr*pageSize,pageSize,bgDate,edDate,whereStr,queryType,out ds,out Msg)) return false;

			if (ds == null || ds.Tables.Count == 0 ||ds.Tables[0].Rows.Count == 0) 
			{
				AspNetPager1.Visible = false;
				this.Page.DataBind();

				Msg = "没有您选定范围内的数据。";
				WebUtils.ShowMessage(this.Page,Msg);
				return false;
			}

			//绑定需要传入的数据源
			this.dgInfo.AutoGenerateColumns = false;  

			foreach(string i in ht.Keys)
			{
				BoundColumn bc = new BoundColumn();
				bc.DataField   = i;
				bc.HeaderText  = ht[i].ToString(); 
				dgInfo.Columns.Add(bc);		
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

		/// <summary>
		/// 为指定输入框绑定默认提交按钮，当焦点在该输入框内时，按下回车键将触发提交按钮的Click事件
		/// </summary>
		/// <param name="txtBox">要绑定的输入框</param>
		/// <param name="btnBindSubmit">要绑定的提交按钮，为null时将使回车键无效</param>
		public void ClientSubmitBind(System.Web.UI.WebControls.TextBox txtBox,System.Web.UI.WebControls.Button btnBindSubmit)
		{
			string script;
			if (btnBindSubmit!=null)
				script = "if(event.keyCode == 13){document.getElementById('" + btnBindSubmit.ClientID + "').click();event.returnValue=false;}"; 
			else
				script = "if(event.keyCode == 13){event.returnValue=false;}"; 
			txtBox.Attributes["onkeydown"] = script;
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

		protected void btQuery_Click(object sender, System.EventArgs e)
		{
			ViewState["newIndex"] = null;  //如果重新点击一次查询，则清空查询的分页；否则无法查询到数据（比如单笔）
			AspNetPager1.Visible = true;
			this.AspNetPager1.CurrentPageIndex = 1;

			bindaData();
		}

		private void AspNetPager1_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			istr = e.NewPageIndex;
			AspNetPager1.CurrentPageIndex = istr;

			ViewState["newIndex"] = e.NewPageIndex -1;

			bindaData();
		}

		protected void ddlCondition_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			//当其选择下拉菜单变化，并且输入框非空，表示其需要精确查询。清空当前的页码 ViewState["newIndex"]	
			if (txbCustom.Text != "" && txbCustom.Text.Trim() !="")
			{
				AspNetPager1.Visible = true;
				ViewState["newIndex"] = null;
  
				bindaData();
			}
		}

		public string   QueryType
		{
			get
			{
				return queryType;
			}
			set
			{
				 queryType = value;
			}
		}

		/// <summary>
		/// 设置需要显示的字段和标题名称
		/// 例：Hashtable ht = new Hashtable();
		///     ht.Add("uin","QQ号码");
		///     ht.Add("dttm","时间");
		///     commDs.ht    = ht;  //commDs是控件名称
		/// </summary>
		public Hashtable htData
		{
			get
			{
				return ht;
			}
			set
			{
				ht = value; 
			}
		}
	}
}
