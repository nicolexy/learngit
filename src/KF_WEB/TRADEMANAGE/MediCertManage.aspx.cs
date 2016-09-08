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
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Web.Services.Protocols;
using System.Xml.Schema;
using System.Xml;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;



namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// MediCertManage 的摘要说明。
	/// </summary>
	public partial class MediCertManage : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
		protected System.Web.UI.HtmlControls.HtmlTable Table2;

	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				this.Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				// if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter")) Response.Redirect("../login.aspx?wh=1");
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SPInfoManagement", this)) Response.Redirect("../login.aspx?wh=1");
			}

			catch
			{
				Response.Redirect("../login.aspx?wh=1");
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
			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

		}
		#endregion

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			try
			{
				pager.CurrentPageIndex = e.NewPageIndex;
				BindData(e.NewPageIndex);
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + classLibrary.setConfig.replaceMStr(eSys.Message) );
			}
		}

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			if(tbSp.Text.Trim() == "")
				WebUtils.ShowMessage(this.Page,"商户号不能为空！");

			try
			{
				pager.RecordCount= GetCount();
				BindData(1);
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

		private int GetCount()
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			return qs.GetMediCertManageCount(tbSp.Text.Trim(),Convert.ToInt16(ddlStatus.SelectedValue),Convert.ToInt16(ddlListStatus.SelectedValue));
		}

		private void BindData(int index)
		{
			int max = pager.PageSize;
			int start = max * (index-1) + 1;

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataSet ds = qs.GetMediCertManageList(tbSp.Text.Trim(),Convert.ToInt16(ddlStatus.SelectedValue),Convert.ToInt16(ddlListStatus.SelectedValue),start,max);

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

	}
}
