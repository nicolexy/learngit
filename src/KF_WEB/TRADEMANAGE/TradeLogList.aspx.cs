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
using TENCENT.OSS.CFT.KF.Common;
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// TradeLogList 的摘要说明。
	/// </summary>
	public partial class TradeLogList : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label lblBeginDate;
		protected System.Web.UI.WebControls.TextBox txtBeginDate;
		protected System.Web.UI.WebControls.ImageButton btnBeginDate;
		protected System.Web.UI.WebControls.Label lblEndDate;
		protected System.Web.UI.WebControls.TextBox txtEndDate;
		protected System.Web.UI.WebControls.ImageButton btnEndDate;
		protected System.Web.UI.WebControls.DropDownList DropDownListChannel;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected System.Web.UI.WebControls.RadioButtonList rblist_tradeType;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{

			LabelError.Text = "";
			if (!IsPostBack)
			{
				classLibrary.setConfig.bindDic("PAY_STATE",DropDownList2_tradeState);

				SetSpid();
				BindSpid(false);
				this.ListState.SelectedIndex = 0;
				try
				{
					this.Label_uid.Text = Session["uid"].ToString();
					string szkey = Session["SzKey"].ToString();
					int operid = Int32.Parse(Session["OperID"].ToString());
 
					//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter")) Response.Redirect("../login.aspx?wh=1");
					if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
				}
				catch  //如果没有登陆或者没有权限就跳出
				{
					Response.Redirect("../login.aspx?wh=1");
				}
                
                this.pager.PageSize = 15;
                this.pager.RecordCount = 2000;

			}
            
			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);
		}

		private void BindSpid(bool IncludeSystemSpid)
		{
			DropDownList1.Items.Clear();
			return;
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
        
        private void showMsg(string msg)
        {
            Response.Write("<script language=javascript>alert('" + msg + "')</script>");
        }
		
        private void ShowData(int index)
		{

			string spid = ListSpidSelect.Checked ? DropDownList1.SelectedValue : TextBoxSpid.Text;
			if(spid==null || spid=="" )// || DropDownList1.Items.FindByValue(spid)==null )
			{
                showMsg("商户号为空，请输入");
				return;
			}


            string strBeginDate = "", strEndDate = "";
			if(this.txDateBegin.Text.Trim()!="" && this.txDateEnd.Text.Trim()!="")
			{
				if(Convert.ToDateTime(this.txDateBegin.Text).AddDays(15)<Convert.ToDateTime(this.txDateEnd.Text))
				{
                    showMsg("查询日期跨度不能超过15天，请重新输入时间");
					return;
				}
                DateTime begindate = DateTime.Parse(txDateBegin.Text.Trim());
                strBeginDate = begindate.ToString("yyyy-MM-dd HH:mm:ss");

                DateTime enddate = DateTime.Parse(txDateEnd.Text.Trim());
                strEndDate = enddate.ToString("yyyy-MM-dd HH:mm:ss");

                
            }
            else
            {
                showMsg("查询起始时间和结束时间不能为空。");
                return;
            }
	
			//string filter = ListState.SelectedIndex==0 ? "":"Fstate='"+DropDownList2_tradeState.SelectedValue+"'";
			string filter = ListState.SelectedIndex==0 ? "":"Ftrade_state='"+DropDownList2_tradeState.SelectedValue+"'";
			string order = "Flistid,FBuyid";//RadioButtonListOrder.SelectedIndex==0 ? "Flistid,FBuyid" : "FBuyid,Flistid";
            #region old
            //Query_Service.Query_Service myService = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            //myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
            //System.Data.DataSet ds = myService.GetMediListX(spid,this.txtFcode.Text.Trim(),strBeginDate,strEndDate,filter,order,
            //    this.pager.PageSize * (index - 1),this.pager.PageSize);
            #endregion
            //v_yqyqguo sql转relay
            filter = ListState.SelectedIndex == 0 ? "" : DropDownList2_tradeState.SelectedValue;
            System.Data.DataSet ds = new TradeService().MediListQueryClass(spid, this.txtFcode.Text.Trim(), strBeginDate, strEndDate, filter, order,
               this.pager.PageSize * (index - 1), this.pager.PageSize);
			if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
			{
				WebUtils.ShowMessage(this,"查询结果为空");
				return;
			}


			DataGrid1.DataSource = ds.Tables[0];
			DataGrid1.DataBind();
			ds.Dispose();
		}

		protected void btQuery_Click(object sender, System.EventArgs e)
		{
			DataGrid1.CurrentPageIndex = 0;
			try
			{
				ShowData(1);
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,ex.Message.ToString());
			}
		}

		private void DataGrid1_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			DataGrid1.CurrentPageIndex = e.NewPageIndex;
			try
			{
				ShowData(e.NewPageIndex);
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,ex.Message.ToString());
			}
		}

		private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if (e.Item.ItemIndex>=0)
			{
				string state = e.Item.Cells[3].Text;
				if (DropDownList2_tradeState.Items.FindByValue(state)!=null)
					e.Item.Cells[3].Text = DropDownList2_tradeState.Items.FindByValue(state).Text;
				e.Item.Cells[6].Text = classLibrary.setConfig.FenToYuan(e.Item.Cells[6].Text);
			}
		}

		void SetSpid()
		{
			DropDownList1.Enabled = ListSpidSelect.Checked;
			TextBoxSpid.Enabled = ListSpidInput.Checked;
			this.lblFcode.Visible = ListSpidInput.Checked;
			this.txtFcode.Visible = ListSpidInput.Checked;
			if(!txtFcode.Visible)
			{
				this.txtFcode.Text="";
			}
		}


		private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;

			ShowData(e.NewPageIndex);
		}
	}
}
