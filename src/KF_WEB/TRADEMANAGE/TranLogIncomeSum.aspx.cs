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
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// TranLogIncomeSum 的摘要说明。
	/// </summary>
	public partial class TranLogIncomeSum : PageBase
	{

		System.Data.DataSet ds;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());
 
				// if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch  //如果没有登陆或者没有权限就跳出
			{
				Response.Redirect("../login.aspx?wh=1");
			} 

			if (!IsPostBack)
			{	
				FillChannelNo(DropDownListChannel);

				PageStateRestore(DropDownListChannel);
				PageStateRestore(TextBoxSpid);

				if (Request.QueryString["channel"].Trim() !="")
				{
					if ( DropDownListChannel.Items.FindByValue(Request.QueryString["channel"].Trim())!=null )
						DropDownListChannel.SelectedValue = Request.QueryString["channel"].Trim();
				}
				if (Request.QueryString["spid"].Trim() != "")
					TextBoxSpid.Text = Request.QueryString["spid"].Trim();
				if (Request.QueryString["predate"] != "")
				{
					try
					{
						this.txtBeginDate.Value = Request.QueryString["predate"].Trim();
						if (Request.QueryString["nextdate"] == "")
                            this.txtEndDate.Value = this.txtBeginDate.Value;
						else
                            this.txtEndDate.Value = Request.QueryString["predate"].Trim();
					}
					catch
					{
					}
				}

				ShowList();
			}
		}

		private void ShowList()
		{
			DateTime begindate;
			DateTime enddate;

			try
			{
                begindate = DateTime.Parse(txtBeginDate.Value);
                enddate = DateTime.Parse(txtEndDate.Value);

				if(begindate>enddate)
				{
					DataGrid1.Visible = false;
					WebUtils.ShowMessage(this.Page,"输入的日期不正确");
					return;
				}
			}
			catch
			{
				DataGrid1.Visible = false;
				WebUtils.ShowMessage(this.Page,"输入的日期不正确");
				return;
			}

			CheckString(TextBoxSpid,"",false);

			PageStateSave(DropDownListChannel);
			PageStateSave(TextBoxSpid);
			
			string filter = "";
			if (DropDownListChannel.Items.Count==0)
				filter = "1=2";
			else
				filter = "FChannelNo="+DropDownListChannel.SelectedValue;

			if (TextBoxSpid.Text.Trim()!="")
			{
				string tbSpid = TextBoxSpid.Text.Trim().Replace("'","''").Replace("\r\n", "' + CHAR(13) + CHAR(10) + '").Replace("\n", "' + CHAR(10) + '");
				filter += " AND FSpid='"+tbSpid+"'";
			}

			try
			{
				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
				ds = qs.TranLogIncomeSumQuery(begindate,enddate,filter);
				DataGrid1.DataSource = ds.Tables[0];
				DataGrid1.DataBind();
				ds.Dispose();
			}
			catch
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
			this.DataGrid1.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.DataGrid1_PageIndexChanged);
			this.DataGrid1.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid1_ItemDataBound);

		}
		#endregion

		private void DataGrid1_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			DataGrid1.CurrentPageIndex = e.NewPageIndex;
			ShowList();
		}

		private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if ( e.Item.ItemIndex >=0 )
			{
				e.Item.Cells[0].Text = DateTimeFormatLongDate(e.Item.Cells[0].Text);
				e.Item.Cells[3].Text = CacheProductTypeName(e.Item.Cells[2].Text,e.Item.Cells[3].Text);
				e.Item.Cells[2].Text = CacheChannelNoName(e.Item.Cells[2].Text);
				
				e.Item.Cells[5].Text = MoneyFormat(e.Item.Cells[5].Text);
				e.Item.Cells[8].Text = MoneyFormat(e.Item.Cells[8].Text);

				e.Item.Cells[9].Text = EnumGetName(typeof(IncomeSumStatus),e.Item.Cells[9].Text);

				if ( e.Item.Cells[5].Text != e.Item.Cells[7].Text )
				{
					e.Item.Cells[5].ForeColor = System.Drawing.Color.Red;
					e.Item.Cells[8].ForeColor = System.Drawing.Color.Red;
				}
				if ( e.Item.Cells[4].Text != e.Item.Cells[7].Text )
				{
					e.Item.Cells[4].ForeColor = System.Drawing.Color.Red;
					e.Item.Cells[7].ForeColor = System.Drawing.Color.Red;
				}
			}
		}

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			DataGrid1.CurrentPageIndex = 0;
			ShowList();
		}
	}
}
