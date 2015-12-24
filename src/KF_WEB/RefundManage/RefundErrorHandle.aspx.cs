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
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;

namespace TENCENT.OSS.CFT.KF.KF_Web.RefundManage
{
	/// <summary>
	/// RefundErrorHandle 的摘要说明。
	/// </summary>
	public partial class RefundErrorHandle : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				try
				{
					this.Label1.Text = Session["uid"].ToString();
					string szkey = Session["SzKey"].ToString();
					int operid = Int32.Parse(Session["OperID"].ToString());
 
					//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID, "InfoCenter")) Response.Redirect("../login.aspx?wh=1");
					if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
				}
				catch  //如果没有登陆或者没有权限就跳出
				{
					Response.Redirect("../login.aspx?wh=1");
				} 

				TextBoxBeginDate.Value = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                TextBoxEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");

				BindBankType(ddlrefund_bank);

				string requestStr1 = Request["batchid"];
				string requestStr2 = Request["onlyView"];

				if(requestStr1 != null && requestStr2 != null && requestStr1.Trim() != "" && requestStr2.Trim() != "")
				{
					ViewState["batchid"] = Request.QueryString["batchid"].Trim();
                    TextBoxBeginDate.Value = "2010-01-01";
					string batchid=Request.QueryString["batchid"];
					DateTime endDate=Convert.ToDateTime("20"+ batchid.Substring(0,2)+"-"+batchid.Substring(2,2)+"-"+batchid.Substring(4,2)).AddDays(10);
                    TextBoxEndDate.Value = endDate.ToString("yyyy-MM-dd");
					ddlorder_type.SelectedIndex=4;
					tbrefund_order.Text=batchid;
					Table4.Visible=false;

					SetViewState();
					btnQuery_Click(null,null);
				}
				else
				{
					ViewState["batchid"] = "";
					SetViewState();
				}
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

		private void BindBankType(DropDownList DropDownList1)
		{
			classLibrary.setConfig.GetAllBankList(ddlrefund_bank);
			ddlrefund_bank.Items.Insert(0,new ListItem("所有银行","0000"));
		}

		private void ValidateDate()
		{
			DateTime begindate;
			DateTime enddate;
			
			try
			{
                begindate = DateTime.Parse(TextBoxBeginDate.Value);
                enddate = DateTime.Parse(TextBoxEndDate.Value);
			}
			catch
			{
				throw new Exception("日期输入有误！");
			}
			
			if(begindate.CompareTo(enddate) > 0)
			{
				throw new Exception("终止日期小于起始日期，请重新输入！");
			}
			
			ViewState["refundorder"] = tbrefund_order.Text.Trim();
			ViewState["ordertype"] = ddlorder_type.SelectedValue;
			ViewState["begindate"] = begindate.ToString("yyyy-MM-dd 00:00:00");
			ViewState["enddate"] = enddate.ToString("yyyy-MM-dd 23:59:59");
			ViewState["refundtype"] = ddlrefund_type.SelectedValue;
			ViewState["refundbank"] = ddlrefund_bank.SelectedValue;
			ViewState["refundpath"] = ddlrefund_path.SelectedValue;
			ViewState["handletype"] = ddlhandle_type.SelectedValue;
			ViewState["errortype"] = ddlerror_type.SelectedValue;
			ViewState["refundstate"]=ddlState.SelectedValue;
		}

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.labErrMsg.Text="";
				ValidateDate();
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,PublicRes.GetErrorMsg(err.Message));
				return;
			}
			
			try
			{
				Table2.Visible = true;
				pager.RecordCount= GetCount(); 
				BindData(1);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
			}
		}

		private int GetCount()
		{
			string refundOrder = ViewState["refundorder"].ToString();
			int orderType = Int32.Parse(ViewState["ordertype"].ToString());
			string beginDate = ViewState["begindate"].ToString();
			string endDate = ViewState["enddate"].ToString();
			int refundType = Int32.Parse(ViewState["refundtype"].ToString());
			string bankType = ViewState["refundbank"].ToString();
			int refundPath = Int32.Parse(ViewState["refundpath"].ToString());
			int handleType = Int32.Parse(ViewState["handletype"].ToString());
			int errorType = Int32.Parse(ViewState["errortype"].ToString());
			int refundState = Int32.Parse(ViewState["refundstate"].ToString());
			string viewOldIds= ViewState["ViewOldIds"].ToString();
			
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			return qs.GetRefundErrorCount("",refundOrder,orderType,beginDate,endDate,refundType,bankType,refundPath,handleType,errorType,refundState,viewOldIds);
		}

		private void BindData(int index)
		{
			string refundOrder = ViewState["refundorder"].ToString();
			int orderType = Int32.Parse(ViewState["ordertype"].ToString());
			string beginDate = ViewState["begindate"].ToString();
			string endDate = ViewState["enddate"].ToString();
			int refundType = Int32.Parse(ViewState["refundtype"].ToString());
			string bankType = ViewState["refundbank"].ToString();
			int refundPath = Int32.Parse(ViewState["refundpath"].ToString());
			int handleType = Int32.Parse(ViewState["handletype"].ToString());
			int errorType = Int32.Parse(ViewState["errortype"].ToString());
			int refundState = Int32.Parse(ViewState["refundstate"].ToString());
			string viewOldIds= ViewState["ViewOldIds"].ToString();

			int max = pager.PageSize;
			int start = max * (index-1) + 1;

			string batchid=ViewState["batchid"].ToString();
			
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataSet ds = qs.GetRefundErrorList(batchid,refundOrder,orderType,beginDate,endDate,refundType,bankType,refundPath,handleType,errorType,refundState,false,viewOldIds,start,max);
			
			if(ds != null && ds.Tables.Count >0)
			{			
				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Fbanktype","Fbank_typeName","BANK_TYPE");

				ds.Tables[0].Columns.Add("Furl",typeof(System.String));
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					dr.BeginEdit();

                    dr["FUrl"] = "oldID=" + dr["Foldid"].ToString() + "&beginTime=" + this.TextBoxBeginDate.Value + "&endTime=" + this.TextBoxEndDate.Value + "&State=" + this.ddlState.SelectedValue
						+"&refundType="+this.ddlrefund_type.SelectedValue+"&RefundPath="+this.ddlrefund_path.SelectedValue
						+"&refundBank="+this.ddlrefund_bank.SelectedValue+"&handleType="+this.ddlhandle_type.SelectedValue
						+"&errorType="+this.ddlerror_type.SelectedValue+"&fromMain="+ViewState["fromMain"].ToString();
					dr.EndEdit();
				}

				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				throw new Exception("没有找到记录！");
			}
		}

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}


		private void SetViewState()
		{
			if(Request.QueryString["beginTime"]!=null)
			{
                this.TextBoxBeginDate.Value = Request.QueryString["beginTime"].ToString();
			}

			if(Request.QueryString["endTime"]!=null)
			{
                this.TextBoxEndDate.Value = Request.QueryString["endTime"].ToString();
			}

			if(Request.QueryString["State"]!=null)
			{
				this.ddlState.SelectedValue=Request.QueryString["State"].ToString();
			}
			if(Request.QueryString["refundType"]!=null)
			{
				this.ddlrefund_type.SelectedValue=Request.QueryString["refundType"].ToString();
			}
			if(Request.QueryString["RefundPath"]!=null)
			{
				this.ddlrefund_path.SelectedValue=Request.QueryString["RefundPath"].ToString();
			}

			if(Request.QueryString["refundBank"]!=null)
			{
				this.ddlrefund_bank.SelectedValue=Request.QueryString["refundBank"].ToString();
			}

			if(Request.QueryString["handleType"]!=null)
			{
				this.ddlhandle_type.SelectedValue=Request.QueryString["handleType"].ToString();
			}

			if(Request.QueryString["errorType"]!=null)
			{
				this.ddlerror_type.SelectedValue=Request.QueryString["errorType"].ToString();
			}

			if(Request.QueryString["fromMain"]!=null)
			{
				ViewState["fromMain"]=Request.QueryString["fromMain"].ToString();;
			}
			else
			{
				ViewState["fromMain"] = "";
			}

			if(Request.QueryString["ViewOldIds"] != null)
			{
				ViewState["ViewOldIds"] = Request.QueryString["ViewOldIds"].Trim();
			}
			else
			{
				ViewState["ViewOldIds"]="";
			}

			ValidateDate();

		}


	}
}
