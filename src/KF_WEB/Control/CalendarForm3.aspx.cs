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


namespace SDate
{

	public partial class CalendarForm3 : System.Web.UI.Page
	{



		protected void Page_Load(object sender, System.EventArgs e)
		{
            //其实应该取得传进来的参数,但是得不到.
            if(!this.IsPostBack)
            {
                TextBox1.Text = DateTime.Now.ToLongDateString();
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

        }
		#endregion

		protected void Calendar1_SelectionChanged(object sender, System.EventArgs e)
		{
			TextBox1.Text = Calendar1.SelectedDate.ToLongDateString();
		}

		protected void Button1_Click(object sender, System.EventArgs e)
		{
			Response.Write("<script language=javascript>window.returnValue='" + TextBox1.Text + "';window.close();</script>");
		}
	}
}
