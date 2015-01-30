namespace TENCENT.OSS.C2C.Finance.Portal.CFTHeader
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/* ********************* 用法示例 *************************

	1。删除原来的页头Html代码和相关控件。
	
	2。拖动控件到页面顶部，作为第一个控件。（本控件高50px）
	
	3。在页面中添加相应的成员变量定义：
		protected TENCENT.OSS.C2C.Finance.Portal.CFTHeader.CFTHeader CFTHeader1;
	
	4。在页面PageLoad事件中增加以下代码，显示当前登录用户ID或名称：
		CFTHeader1.User = "登录用户";

	5。在Web.config中增加以下配置项，修改“子系统名称”：
		<!-- 当前子系统名称 -->
		<add key="CFT_SUBSYS_NAME" value="子系统名称" />
		<!-- 当前子系统URL -->
		<add key="CFT_SUBSYS_URL" value="子系统URL" />
		<!-- 财付通产品门户网站URL，如果没有，应该配置为空串 -->
		<add key="CFT_PORTAL_URL" value="http://www.cf.com" />
		<!-- 财付通子系统名称及URL列表，用分号分隔 -->
		<add key="CFT_HEADER_NAMES" value="账务系统;会计系统;客服系统;结算系统;监控系统" />
		<add key="CFT_HEADER_URLS" value="http://admin.cf.com ; http://kj.cf.com ; http://kf.cf.com; http://js.cf.com ; http://monit.cf.com" />
	
	6。每次发布新版本时，在AssemblyInfo.cs中修改程序版本号，然后编译Release包：
	[assembly: AssemblyVersion("2.0.134")]

	********************************************************* */

	/// <summary>
	///	财付通子系统页头控件
	/// </summary>
	public partial class CFTHeader : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.LinkButton LinkButton1;

		#region PageLoad
		protected void Page_Load(object sender, System.EventArgs e)
		{
			System.Web.UI.HtmlControls.HtmlTable cftheader = 
				(System.Web.UI.HtmlControls.HtmlTable)FindControl("cftheader");
			cftheader.Attributes["background"] = this.ResolveUrl("bg01.gif");
			//绑定子系统链接
			LinkListBind();
			//设置门户URL、子系统名称
			LinkPortal.NavigateUrl = this.PortalUrl;
			LabelName.Text = "腾讯公司财付通"+Name;
			SetCaption(LabelName.Text);
			//显示版本号
			LabelVersion.Text = "V"+ System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
		}
		#endregion

		#region 私有成员函数
		void SetCaption(string caption)
		{
			string jsShowTitle = "<script language=JavaScript>document.title='"+caption+"';</script>";
            if (!Page.ClientScript.IsClientScriptBlockRegistered("SHOW_PAGE_TITLE"))
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "SHOW_PAGE_TITLE", jsShowTitle);
		}
		#endregion
		
		#region 公有属性 Name/User/PortalUrl/CurrentUrl
		/// <summary>
		/// 子系统名称，从配置文件中获取
		/// </summary>
		public string Name
		{
			get
			{
				return System.Configuration.ConfigurationManager.AppSettings[HEADER_CONFIG_SUBSYS_NAME].ToString();
			}
		}

		/// <summary>
		/// 当前登录用户
		/// </summary>
		public string User
		{
			set
			{
				LabelUser.Text = value;
			}
		}

		/// <summary>
		/// 门户网站URL
		/// </summary>
		public string PortalUrl
		{
			get
			{
				return System.Configuration.ConfigurationManager.AppSettings[HEADER_CONFIG_PORTAL_URLS].ToString();
			}
		}

		/// <summary>
		/// 当前子系统URL
		/// </summary>
		public string CurrentUrl
		{
			get
			{
				return System.Configuration.ConfigurationManager.AppSettings[HEADER_CONFIG_SUBSYS_URL].ToString();
			}
		}
		#endregion

		#region 绑定子系统链接的函数
		const string HEADER_CONFIG_SUBSYS_NAME = "CFT_SUBSYS_NAME";
		const string HEADER_CONFIG_SUBSYS_URL = "CFT_SUBSYS_URL";
		const string HEADER_CONFIG_PORTAL_URLS = "CFT_PORTAL_URL";
		const string LINK_CSS = "cftheaderlink";
		const string DIV_CSS = "cftheaderdiv";
		const string HEADER_CONFIG_NAMES = "CFT_HEADER_NAMES";
		const string HEADER_CONFIG_URLS = "CFT_HEADER_URLS";
		
		void LinkListBind()
		{
			//LinkList.Controls.Clear();

			string name = System.Configuration.ConfigurationManager.AppSettings[HEADER_CONFIG_NAMES].ToString();
			string url = System.Configuration.ConfigurationManager.AppSettings[HEADER_CONFIG_URLS].ToString();
			string[] nameList = name.Split(";".ToCharArray());
			string[] urlList = url.Split(";".ToCharArray());

			HyperLink link; Label label;
			for (int i=0;i<nameList.Length;i++)
			{
				link = new HyperLink();
				link.CssClass = LINK_CSS;
				link.NavigateUrl = urlList[i].Trim();
				link.Text = nameList[i].Trim();
				link.Target = "_blank";
				//LinkList.Controls.Add(link);

				if ( (i+1) < nameList.Length )
				{
					label = new Label();
					label.Text = "&nbsp;|&nbsp;";
					label.CssClass = DIV_CSS;
					//LinkList.Controls.Add(label);
				}
			}
		}
		#endregion
		
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

		#region 注销事件
		protected void LinkLogout_Click(object sender, System.EventArgs e)
		{
			Page.Session.Clear();
			if ( this.PortalUrl!="" )
				RedirectTo(this.PortalUrl);
			else
				RedirectTo(this.CurrentUrl);
		}

		public void RedirectTo(string url)
		{
			//如果当前页面在IFrame中，则window.parent指向主页面；如果当前页面是主页面，则window.parent仍是主页面
			string script = "<script language='javascript'>window.parent.location='"+url+"';</script>";
			Page.Response.Write(script);
			Page.Response.End();
		}
		#endregion
	}
}
