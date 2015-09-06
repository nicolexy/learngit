namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;


	/// <summary>
	///		MenuControl ��ժҪ˵����
	/// </summary>
	public partial class MenuControl : System.Web.UI.UserControl
	{


		/// <summary>
		/// �˵�����
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
					return "(δ�������)" ;
				}
			}
		}

		/// <summary>
		/// �Ƿ�չ���Ӳ˵�
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
		/// ����Ӳ˵�
		/// </summary>
		/// <param name="subTitle">�Ӳ˵�����</param>
		public void AddSubMenu(string subTitle)
		{
			this.ViewState["SubMenu"] += subTitle + "#" + "javascript:alert('��ģ�����ڿ����С�����')" + "@" ;
		}
		/// <summary>
		/// ����Ӳ˵�
		/// </summary>
		/// <param name="subTitle">�Ӳ˵�����</param>
		/// <param name="subURL">�Ӳ˵�����</param>
		public void AddSubMenu(string subTitle, string subURL)
		{
			this.ViewState["SubMenu"] += subTitle + "#" + subURL + "@" ;
		}


		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if ( !IsPostBack )
			{
				InitControls() ;
			}
		}


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
