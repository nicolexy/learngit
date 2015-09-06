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
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// CFTUserAppeal 的摘要说明。
	/// </summary>
	public partial class CFTUserAppeal : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.DropDownList ddlStateType;
		protected System.Web.UI.WebControls.TextBox tbFNum;
		protected System.Web.UI.WebControls.DropDownList Dropdownlist1;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			// 在此处放置用户代码以初始化页面
			try
			{
				Label1.Text = Session["uid"].ToString();

				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"CFTUserAppeal")) Response.Redirect("../login.aspx?wh=1");

				if(!classLibrary.ClassLib.ValidateRight("CFTUserAppeal",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				Table2.Visible = false;				
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
			this.DataGrid1.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid1_ItemDataBound);
			this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

		}
		#endregion

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}

		private void ValidateDate()
		{
			string stmp = tbFuin.Text.Trim();
			if(stmp == "")
			{
				throw new Exception("请输入用户帐号！");
			}

			ViewState["fuin"] = classLibrary.setConfig.replaceMStr(stmp);
		}

		protected void Button2_Click(object sender, System.EventArgs e)
		{
			try
			{
				string strszkey = Session["SzKey"].ToString().Trim();
				int ioperid = Int32.Parse(Session["OperID"].ToString());
				int iserviceid = Common.AllUserRight.GetServiceID("CFTUserAppeal") ;
				string struserdata = Session["uid"].ToString().Trim();
				string content = struserdata + "执行了[自助申诉查询]操作,操作对象[" + classLibrary.setConfig.replaceMStr(tbFuin.Text.Trim()) + "]时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

				Common.AllUserRight.UpdateSession(strszkey,ioperid,PublicRes.GROUPID,iserviceid,struserdata,content);

				string log = SensitivePowerOperaLib.MakeLog("get",struserdata,"[自助申诉查询]",
					tbFuin.Text.Trim(),"","","99","99","","1",pager.PageSize.ToString());

				if(!SensitivePowerOperaLib.WriteOperationRecord("CFTUserAppeal",log,this))
				{
					
				}

				ValidateDate();
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

			try
			{
				Table2.Visible = true;
				pager.RecordCount= 10000;//GetCount(); 
				BindData(1);
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message.ToString());
			}
		}

		private void BindData(int index)
		{
			string fuin = ViewState["fuin"].ToString();

			int max = pager.PageSize;
			int start = max * (index-1) + 1;

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			Finance_Header fh = setConfig.setFH(this);
			qs.Finance_HeaderValue = fh;
            DataSet ds = new DataSet();
            DataSet ds1 = qs.GetCFTUserAppealListNew(fuin, "", "", 99, 100, "", "9", start, max, 99);//ftype=100查询所有申诉类型

            //处理分页问题
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds1.Tables[0];
                dt = PublicRes.GetPagedTable(dt, index, pager.PageSize);
                ds.Tables.Add(dt);
            }

            ds = qs.GetCFTUserAppealListFunction(ds);//先分页再处理内部信息

			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0)
			{
				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				//throw new LogicException("没有找到记录！");
				WebUtils.ShowMessage(this,"查询记录为空");
			}
		}

		private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			string stmp = e.Item.Cells[0].Text.Trim();
			int strlen = stmp.Length;

			if(strlen > 5)
			{
				stmp = "***" + stmp.Substring(3,strlen-3);
				e.Item.Cells[0].Text = stmp;
			}
		}
	}
}
