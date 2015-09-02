namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;


	/// <summary>
	///		MenuControl 的摘要说明。
	/// </summary>
	public partial class MenuControl : System.Web.UI.UserControl
	{


		/// <summary>
		/// 菜单标题
		/// </summary>
		public string Title
		{
			set
			{
				this.ViewState["Title"] = value ;
			}
			get
			{
				try
				{
					return this.ViewState["Title"].ToString() ;
				}
				catch
				{
					return "(未定义标题)" ;
				}
			}
		}

		/// <summary>
		/// 是否展开子菜单
		/// </summary>
		public bool ExpandSubMenu
		{
			set
			{
				if ( value )
				{
					this.ViewState["Expand"] = "none" ;
				}
				else
				{
					this.ViewState["Expand"] = "" ;
				}
			}
		}
		/// <summary>
		/// 添加子菜单
		/// </summary>
		/// <param name="subTitle">子菜单标题</param>
		public void AddSubMenu(string subTitle)
		{
			this.ViewState["SubMenu"] += subTitle + "#" + "javascript:alert('该模块正在开发中。。。')" + "@" ;
		}
		/// <summary>
		/// 添加子菜单
		/// </summary>
		/// <param name="subTitle">子菜单标题</param>
		/// <param name="subURL">子菜单链接</param>
		public void AddSubMenu(string subTitle, string subURL)
		{
			this.ViewState["SubMenu"] += subTitle + "#" + subURL + "@" ;
		}


		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if ( !IsPostBack )
			{
				InitControls() ;
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
		///		设计器支持所需的方法 - 不要使用代码编辑器
		///		修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		private string BuildSubMenu()
		{
			try
			{
				string subMenu = this.ViewState["SubMenu"].ToString() ;
				string buildString = "" ;

				for ( int i = 0 ; i < subMenu.Split('@').Length-1 ; i ++ )
				{
					string subMenuString = subMenu.Split('@')[i] ;
					string subTitle = subMenu.Split('@')[i].Split('#')[0] ;
					string subURL = subMenu.Split('@')[i].Split('#')[1] ;

					buildString += "<tr>" ;
					buildString += "<td><a href=\""+subURL+"\" class=\"Red\" TARGET=\"WorkArea\">"+subTitle+"</a></td>" ;

					buildString += "</tr>" ;
				}
				return buildString ;
			}
			catch ( Exception exce )
			{
				string err = exce.Message ;
				return "" ;
			}

		}

		private void InitControls()
		{
			string expand = "none" ;
			try
			{
				expand = this.ViewState["Expand"].ToString() ;
			}
			catch
			{
			}

			string menuString = "" ;
			menuString += "<table width=\"96%\" border=\"0\" cellpadding=\"2\" cellspacing=\"2\">" ;

            menuString += "<tr style=\"cursor:pointer;\" name=\"menu_flag\" onclick=\"javascript:expandObject('" + this.ClientID + "_SubMenu')\">";
			//menuString += "<td background=\"images/page/menu_bk.gif\" height=\"20\">" ;

            menuString += "<td height=\"20\">";
			menuString += "<strong>" + Title + "</strong>" ;
			menuString += "</td>" ;
			menuString += "</tr>" ;
			menuString += "<tr id=\""+this.ClientID+"_SubMenu\" style=\"display:"+expand+"\">" ;
			menuString += "<td align=\"left\">" ;
			menuString += "<table width=\"93%\" border=\"0\" cellpadding=\"2\" cellspacing=\"2\">" ;

			menuString += BuildSubMenu() ;

			menuString += "</table>" ;
			menuString += "</td>" ;
			menuString += "</tr>" ;
			menuString += "</table>" ;

			lbMenu.Text = menuString ;
		}
	}
}
