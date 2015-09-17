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
				string subMenu = this.ViewState["SubMenu"]as string;
                if (string.IsNullOrEmpty(subMenu)) 
                    return string.Empty;

				var arr = subMenu.Split('@') ;
                var buff = new System.Text.StringBuilder(arr.Length * 50);
                for (int i = 0; i < arr.Length - 1; i++)
				{
                    var menuArr = arr[i].Split('#');
                    string subTitle = menuArr[0];
                    string subURL = menuArr[1];
                    buff.AppendFormat("<div><a href='{0}' class='Red' TARGET='WorkArea' style='display:block;padding:2px 0;'>{1}</a></div>", subURL, subTitle);
				}
                return buff.ToString();
			}
			catch {}
            return " ";
		}

		private void InitControls()
		{
            string expand = this.ViewState["Expand"] as string ?? "none";
            var menuString =
                    "<div style='cursor:hand;padding:2px 0 4px 4px;' onclick='expandObject(this)'>" + 
                        "<strong>" + Title + "</strong>" + 
                        "<div style='padding:4px 0 4px 8px;display:" + expand + "'>" + BuildSubMenu() + "</div>"+
                    "</div>" ;
			lbMenu.Text = menuString ;
		}
	}
}
