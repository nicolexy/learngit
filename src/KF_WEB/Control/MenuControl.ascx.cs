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
