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
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// QuerySpOrderPage 的摘要说明。
	/// </summary>
	public partial class QuerySpOrderPage : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string szkey = Session["SzKey"].ToString();
			//int operid = Int32.Parse(Session["OperID"].ToString());
			//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
			if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(pager_PageChanged);

			classLibrary.setConfig.bindDic("PAY_STATE",this.ddl_state);

			this.pager.RecordCount = 1000;

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


		private void BindData(int index)
		{
			this.pager.CurrentPageIndex = index;

            Query_Service.Query_Service qs = new Query_Service.Query_Service();

            //qs.Finance_HeaderValue = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.setFH(Session["uid"].ToString(),Request.UserHostAddress);

            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

            DataSet ds = qs.GetMediListX(this.tbx_spid.Text.Trim(),this.tbx_spcoding.Text.Trim(),"","","","",
                this.pager.PageSize * (index - 1),this.pager.PageSize);
            //System.Data.DataSet ds = new TradeService().MediListQueryClass(this.tbx_spid.Text.Trim(), this.tbx_spcoding.Text.Trim(), "", "", "", "",
            //    this.pager.PageSize * (index - 1), this.pager.PageSize);
			if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
			{
				WebUtils.ShowMessage(this,"查询结果为空");
				ds = null;
			}
			else
			{
				ds.Tables[0].Columns.Add("flistidUrl",typeof(string));

				ds.Tables[0].Rows[0]["flistidUrl"] = "TradeLogQuery.aspx?id=" + ds.Tables[0].Rows[0]["flistid"].ToString();
			}

			this.DataGrid1.DataSource = ds;
			this.DataGrid1.DataBind();
		}

		private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if (e.Item.ItemIndex>=0)
			{
				string state = e.Item.Cells[3].Text;
				if (ddl_state.Items.FindByValue(state)!=null)
					e.Item.Cells[3].Text = ddl_state.Items.FindByValue(state).Text;
				e.Item.Cells[6].Text = classLibrary.setConfig.FenToYuan(e.Item.Cells[6].Text);
			}
		}
		

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			if(this.tbx_spid.Text.Trim() == "" || this.tbx_spcoding.Text.Trim() == "")
			{
				WebUtils.ShowMessage(this,"请输入商户号和商户订单号");
				return;
			}

			Query_Service.Query_Service qs = new Query_Service.Query_Service();

			//qs.Finance_HeaderValue = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.setFH(Session["uid"].ToString(),Request.UserHostAddress);

			qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

			try
			{
				DataSet ds = qs.GetSPOrder(this.tbx_spid.Text.Trim(),this.tbx_spcoding.Text.Trim());

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				{
					BindData(1);
					return;
				}

				DataSet ds2 = new DataSet();

				ds2.Tables.Add(new DataTable());

				ds2.Tables[0].Columns.Add("flistid",typeof(string));
				ds2.Tables[0].Columns.Add("Fcoding",typeof(string));
				ds2.Tables[0].Columns.Add("fmodify_time",typeof(string));
				ds2.Tables[0].Columns.Add("Fstate",typeof(string));
				ds2.Tables[0].Columns.Add("FBuyid",typeof(string));
				ds2.Tables[0].Columns.Add("FBuy_name",typeof(string));
				ds2.Tables[0].Columns.Add("Fpaynum",typeof(string));
				ds2.Tables[0].Columns.Add("fmemo",typeof(string));
				ds2.Tables[0].Columns.Add("flistidUrl",typeof(string));

				ds2.Tables[0].Rows.Add(ds2.Tables[0].NewRow());

				ds2.Tables[0].Rows[0]["flistid"] = ds.Tables[0].Rows[0]["transaction_ids"].ToString();
				ds2.Tables[0].Rows[0]["flistidUrl"] = "TradeLogQuery.aspx?id=" + ds.Tables[0].Rows[0]["transaction_ids"].ToString();
				ds2.Tables[0].Rows[0]["Fcoding"] = "null";
				ds2.Tables[0].Rows[0]["fmodify_time"] = "null";
				ds2.Tables[0].Rows[0]["Fstate"] = "null";
				ds2.Tables[0].Rows[0]["FBuyid"] = "null";
				ds2.Tables[0].Rows[0]["FBuy_name"] = "null";
				ds2.Tables[0].Rows[0]["Fpaynum"] = "null";
				ds2.Tables[0].Rows[0]["fmemo"] = "null";

				this.DataGrid1.DataSource = ds2;
				this.DataGrid1.DataBind();
				ds.Dispose();

				this.pager.CurrentPageIndex = 1;

				//this.lb_cftListID.Text = ds.Tables[0].Rows[0]["transaction_ids"].ToString();
			}
			catch
			{
				//WebUtils.ShowMessage(this,ex.Message);
			}
		}

		private void pager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			BindData(e.NewPageIndex);
		}
	}
}
