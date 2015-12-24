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
	/// SettQuery 的摘要说明。
	/// </summary>
	public partial class SettQuery : PageBase
	{
		protected System.Web.UI.WebControls.ImageButton IBNewFee;
		protected Wuqi.Webdiyer.AspNetPager pager;
		protected System.Web.UI.WebControls.Label lblSPID;
		protected System.Web.UI.WebControls.TextBox tbSPID;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.TextBox tbAccount;
		protected System.Web.UI.WebControls.Button Button2;
		protected System.Web.UI.HtmlControls.HtmlTable Table2;

	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{

				this.Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}

			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			} 

			if(!IsPostBack)
			{
				txtSettDate.Value = DateTime.Now.ToString("yyyy-MM-dd");

				EnumFillList(typeof(SettUseTag),false,"全部",ListUseTag);

				this.CheckBoxDate.Checked = true;

				BindData();
			}
		}
		
		private void BindData()
		{

			string filter = "(1=1)";
            string wx_filter = "";
            if (TextBoxId.Text != "")
            { 
                filter += " AND (FFeeContract=" + TextBoxId.Text + ")"; 
            }
            if (TextBoxSpid.Text != "")
            { 
                filter += " AND (FSpid='" + SunLibrary.DataAccess.DataProvider.GetSafeString(TextBoxSpid.Text) + "')";
                wx_filter += " AND (a.Fspid='" + SunLibrary.DataAccess.DataProvider.GetSafeString(TextBoxSpid.Text) + "')"; 
            }
            if (TextBoxFeeNo.Text != "")
            { 
                filter += " AND (FFeeNo=" + TextBoxFeeNo.Text + ")";
                wx_filter += " AND (a.Fincome_id=" + TextBoxFeeNo.Text + ")"; 
            }
            if (CheckBoxDate.Checked)
            { 
                filter += " AND (FPreDate='" + SunLibrary.DataAccess.DataProvider.DateFormat(Convert.ToDateTime(this.txtSettDate.Value)) + "')";
                wx_filter += " AND (a.Faccount_date='" + SunLibrary.DataAccess.DataProvider.DateFormat(Convert.ToDateTime(this.txtSettDate.Value)) + "')"; 
            }
            if (ListUseTag.SelectedIndex > 0)
            {
                filter += " AND (FUseTag=" + ListUseTag.SelectedValue + ")";
                wx_filter += " AND (a.Fstate=" + ListUseTag.SelectedValue + ")"; 
            }

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataSet ds  = qs.SettQuery(filter);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) 
            {

                ds = qs.SettQueryWechat(wx_filter);
            }
			
			if(ds != null && ds.Tables.Count >0)
			{
				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				this.DataGrid1.Visible = false;
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


		private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if ( e.Item.ItemIndex >= 0 )
			{
				HyperLink hl = (HyperLink)e.Item.Cells[0].FindControl("LinkId");
				hl.Attributes["onclick"] = ScriptPopup("SettShow.aspx?id="+hl.Text,450,700,false);

				SetSpidShow((HyperLink)e.Item.Cells[2].FindControl("LinkSpid"));

				e.Item.Cells[4].Text = CacheProductTypeName(e.Item.Cells[3].Text,e.Item.Cells[4].Text);
				e.Item.Cells[3].Text = CacheChannelNoName(e.Item.Cells[3].Text);

				e.Item.Cells[6].Text = MoneyFormat(e.Item.Cells[6].Text);

				e.Item.Cells[9].Text = MoneyFormat(e.Item.Cells[9].Text);
				e.Item.Cells[10].Text = MoneyFormat(e.Item.Cells[10].Text);

				e.Item.Cells[12].Text = DateTimeFormatLongDate(e.Item.Cells[12].Text);
				e.Item.Cells[13].Text = DateTimeFormatLongDate(e.Item.Cells[13].Text);
			}
		}

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			try
			{
                DataGrid1.CurrentPageIndex = 0;
				BindData();
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				this.DataGrid1.Visible = false;

				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				this.DataGrid1.Visible = false;

				WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message.ToString());
			}
		}

		private void DataGrid1_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			this.DataGrid1.CurrentPageIndex = e.NewPageIndex;
			BindData();
		}

	}
}
