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
            //��ʵӦ��ȡ�ô������Ĳ���,���ǵò���.
            if(!this.IsPostBack)
            {
                TextBox1.Text = DateTime.Now.ToLongDateString();
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
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
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
