namespace TENCENT.OSS.C2C.Finance.Portal.CFTHeader
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/* ********************* �÷�ʾ�� *************************

	1��ɾ��ԭ����ҳͷHtml�������ؿؼ���
	
	2���϶��ؼ���ҳ�涥������Ϊ��һ���ؼ��������ؼ���50px��
	
	3����ҳ���������Ӧ�ĳ�Ա�������壺
		protected TENCENT.OSS.C2C.Finance.Portal.CFTHeader.CFTHeader CFTHeader1;
	
	4����ҳ��PageLoad�¼����������´��룬��ʾ��ǰ��¼�û�ID�����ƣ�
		CFTHeader1.User = "��¼�û�";

	5����Web.config����������������޸ġ���ϵͳ���ơ���
		<!-- ��ǰ��ϵͳ���� -->
		<add key="CFT_SUBSYS_NAME" value="��ϵͳ����" />
		<!-- ��ǰ��ϵͳURL -->
		<add key="CFT_SUBSYS_URL" value="��ϵͳURL" />
		<!-- �Ƹ�ͨ��Ʒ�Ż���վURL�����û�У�Ӧ������Ϊ�մ� -->
		<add key="CFT_PORTAL_URL" value="http://www.cf.com" />
		<!-- �Ƹ�ͨ��ϵͳ���Ƽ�URL�б��÷ֺŷָ� -->
		<add key="CFT_HEADER_NAMES" value="����ϵͳ;���ϵͳ;�ͷ�ϵͳ;����ϵͳ;���ϵͳ" />
		<add key="CFT_HEADER_URLS" value="http://admin.cf.com ; http://kj.cf.com ; http://kf.cf.com; http://js.cf.com ; http://monit.cf.com" />
	
	6��ÿ�η����°汾ʱ����AssemblyInfo.cs���޸ĳ���汾�ţ�Ȼ�����Release����
	[assembly: AssemblyVersion("2.0.134")]

	********************************************************* */

	/// <summary>
	///	�Ƹ�ͨ��ϵͳҳͷ�ؼ�
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
			//����ϵͳ����
			LinkListBind();
			//�����Ż�URL����ϵͳ����
			LinkPortal.NavigateUrl = this.PortalUrl;
			LabelName.Text = "��Ѷ��˾�Ƹ�ͨ"+Name;
			SetCaption(LabelName.Text);
			//��ʾ�汾��
			LabelVersion.Text = "V"+ System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
		}
		#endregion

		#region ˽�г�Ա����
		void SetCaption(string caption)
		{
			string jsShowTitle = "<script language=JavaScript>document.title='"+caption+"';</script>";
            if (!Page.ClientScript.IsClientScriptBlockRegistered("SHOW_PAGE_TITLE"))
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "SHOW_PAGE_TITLE", jsShowTitle);
		}
		#endregion
		
		#region �������� Name/User/PortalUrl/CurrentUrl
		/// <summary>
		/// ��ϵͳ���ƣ��������ļ��л�ȡ
		/// </summary>
		public string Name
		{
			get
			{
				return System.Configuration.ConfigurationManager.AppSettings[HEADER_CONFIG_SUBSYS_NAME].ToString();
			}
		}

		/// <summary>
		/// ��ǰ��¼�û�
		/// </summary>
		public string User
		{
			set
			{
				LabelUser.Text = value;
			}
		}

		/// <summary>
		/// �Ż���վURL
		/// </summary>
		public string PortalUrl
		{
			get
			{
				return System.Configuration.ConfigurationManager.AppSettings[HEADER_CONFIG_PORTAL_URLS].ToString();
			}
		}

		/// <summary>
		/// ��ǰ��ϵͳURL
		/// </summary>
		public string CurrentUrl
		{
			get
			{
				return System.Configuration.ConfigurationManager.AppSettings[HEADER_CONFIG_SUBSYS_URL].ToString();
			}
		}
		#endregion

		#region ����ϵͳ���ӵĺ���
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
		///		�����֧������ķ��� - ��Ҫʹ�ô���༭��
		///		�޸Ĵ˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		#region ע���¼�
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
			//�����ǰҳ����IFrame�У���window.parentָ����ҳ�棻�����ǰҳ������ҳ�棬��window.parent������ҳ��
			string script = "<script language='javascript'>window.parent.location='"+url+"';</script>";
			Page.Response.Write(script);
			Page.Response.End();
		}
		#endregion
	}
}
