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
using Wuqi.Webdiyer;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;
using SunLibrary;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// GwqByQQ 的摘要说明。
	/// </summary>
	public partial class GwqByQQ : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if (!IsPostBack)
			{
				try
				{
					//绑定第一页数据
					BindData(1);
				}
				catch(Exception emsg)
				{
					WebUtils.ShowMessage(this.Page,"操作超时！请重新查询。" + classLibrary.setConfig.replaceMStr(emsg.Message));
				}
			}		
		}

		private void BindData(int index)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			Finance_Header fh = classLibrary.setConfig.setFH(this);
			qs.Finance_HeaderValue = fh;

			int pageSize =  Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"].ToString());  //通过webconfig控制页大小
			pager.PageSize   = pageSize;
			pager.RecordCount = qs.GetGwqCount(Session["QQID"].ToString(),"");	
			pager.CurrentPageIndex = index;
			int max = pager.PageSize;
			int start = max * (index-1) + 1;

			DataSet ds = qs.GetGwq(Session["QQID"].ToString(),"",start,max);

			if(ds != null && ds.Tables.Count >0)
			{
				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				throw new LogicException("没有找到记录！");
			}
		}

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex=e.NewPageIndex;
			BindData(pager.CurrentPageIndex);
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
			this.DataGrid1.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid1_ItemDataBound);

		}
		#endregion

		private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if (e.Item.ItemIndex>=0)
			{
				e.Item.Cells[21].Text = TokenCoin.Define.FeeFormat(e.Item.Cells[16].Text,e.Item.Cells[21].Text);
				e.Item.Cells[14].Text = Sun.EnumGetName(typeof(TokenCoin.DonateType),e.Item.Cells[14].Text);
				e.Item.Cells[25].Text = Sun.MoneyFormat(e.Item.Cells[25].Text);
				e.Item.Cells[17].Text = Sun.EnumGetName(typeof(TokenCoin.PubType),e.Item.Cells[17].Text);
				e.Item.Cells[18].Text = Sun.EnumGetName(typeof(TokenCoin.GwqState),e.Item.Cells[18].Text);
				e.Item.Cells[19].Text = Sun.EnumGetName(typeof(TokenCoin.ListState),e.Item.Cells[19].Text);
				e.Item.Cells[20].Text = Sun.EnumGetName(typeof(TokenCoin.RollAdjust),e.Item.Cells[20].Text);
				e.Item.Cells[40].Text = Sun.EnumGetName(typeof(TokenCoin.AcFlag),e.Item.Cells[40].Text);
				e.Item.Cells[16].Text = Sun.EnumGetName(typeof(TokenCoin.CoinType),e.Item.Cells[16].Text);
			}
		}
	}
}
