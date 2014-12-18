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
	/// B2CReturnQuery 的摘要说明。
	/// </summary>
	public partial class B2CReturnQuery : System.Web.UI.Page
	{
	

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
			ButtonEndDate.Attributes.Add("onclick", "openModeEnd()"); 

			try
			{
				this.Label_uid.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}

			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			} 

			if(!IsPostBack)
			{
				TextBoxBeginDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
				TextBoxEndDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
				Table2.Visible = false;
				BindBankType(ddlBankType);
			}
		}

		void BindBankType(DropDownList DropDownList1)
		{
			setConfig.GetAllBankList(DropDownList1);
			DropDownList1.Items.Insert(0,new ListItem("所有银行","0000"));
		}

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			pager.CurrentPageIndex = e.NewPageIndex;
			BindData(e.NewPageIndex);
		}

		private void ValidateDate(bool huizongFlag)
		{
			DateTime begindate;
			DateTime enddate;
			
			try
			{
				begindate = DateTime.Parse(TextBoxBeginDate.Text);
				enddate = DateTime.Parse(TextBoxEndDate.Text);
			}
			catch
			{
				throw new Exception("日期输入有误！");
			}
			
			if(begindate.CompareTo(enddate) > 0)
			{
				throw new Exception("终止日期小于起始日期，请重新输入！");
			}
			
						
			if(begindate.AddDays(30).CompareTo(enddate) < 0)
			{
				throw new Exception("选择时间段超过了三十天，请重新输入！");
			}
									
			if(!huizongFlag)
			{
				if(tbSPID.Text.Trim()==""&&tbTransID.Text.Trim()==""&&tbDrawID.Text.Trim()=="")
				{
					throw new Exception("商户号、交易单号、退单号不能全部为空！");
				}
			}
			
			ViewState["spid"] = tbSPID.Text.Trim();
			
			
			ViewState["begindate"] = begindate.ToString("yyyy-MM-dd 00:00:00");
			ViewState["enddate"] = enddate.ToString("yyyy-MM-dd 23:59:59");
			
			ViewState["refundtype"] = ddlrefund_type.SelectedValue.Trim();
			ViewState["status"] = ddlStatus.SelectedValue.Trim();

			ViewState["transid"] = tbTransID.Text.Trim();
			ViewState["buyqq"] = tbBuyerID.Text.Trim();

			ViewState["banktype"] = ddlBankType.SelectedValue;

			ViewState["sumtype"] =ddlSumType.SelectedValue;

			ViewState["drawid"]=tbDrawID.Text.Trim();
		}

		public void Button2_Click(object sender, System.EventArgs e)
		{
			try
			{
				ValidateDate(false);
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}
			
			try
			{
				Table2.Visible = true;
				pager.RecordCount= GetCount(); 
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

		private int GetCount()
		{
			string spid = ViewState["spid"].ToString();
			string begindate = ViewState["begindate"].ToString();
			string enddate = ViewState["enddate"].ToString();
		
			int refundtype = Int32.Parse(ViewState["refundtype"].ToString());
			int status = Int32.Parse(ViewState["status"].ToString());

			string transid = ViewState["transid"].ToString();
			string buyqq = ViewState["buyqq"].ToString();
		
		
			string banktype = ViewState["banktype"].ToString();

			int sumtype=Int32.Parse(ViewState["sumtype"].ToString());

			string drawid=ViewState["drawid"].ToString();

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			return qs.GetB2cReturnCount(spid,begindate,enddate,refundtype,status,transid,buyqq,banktype,sumtype,drawid);
		}

		private void BindData(int index)
		{
			string spid = ViewState["spid"].ToString();
			string begindate = ViewState["begindate"].ToString();
			string enddate = ViewState["enddate"].ToString();
		
			int refundtype = Int32.Parse(ViewState["refundtype"].ToString());
			int status = Int32.Parse(ViewState["status"].ToString());

			string transid = ViewState["transid"].ToString();
			string buyqq = ViewState["buyqq"].ToString();
			
			string banktype = ViewState["banktype"].ToString();

			int sumtype=Int32.Parse(ViewState["sumtype"].ToString());

			string drawid=ViewState["drawid"].ToString();

			int max = pager.PageSize;
			int start = max * (index-1) + 1;
			
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataSet ds = qs.GetB2cReturnList(spid,begindate,enddate,refundtype,status,transid,buyqq,banktype,sumtype,drawid, Convert.ToInt32(this.ddlTableType.SelectedValue),start,max);
			
			if(ds != null && ds.Tables.Count >0)
			{			
				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				DataGrid1.DataSource = null;
				DataGrid1.DataBind();
			}
		}
	}
}
