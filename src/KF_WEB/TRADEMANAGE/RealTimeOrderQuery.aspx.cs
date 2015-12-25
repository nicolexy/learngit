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
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using CFT.CSOMS.BLL.TransferMeaning;


namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// RealTimeOrderQuery 的摘要说明。
	/// </summary>
	public partial class RealTimeOrderQuery : System.Web.UI.Page
	{
		public string begintime = DateTime.Now.ToString("yyyy-MM-dd");
		public string endtime = DateTime.Now.ToString("yyyy-MM-dd");

	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				// if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
	
				TextBoxBeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
				TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

				classLibrary.setConfig.GetAllBankList(ddlBankType);
				ddlBankType.Items.Insert(0,new ListItem("所有银行","0000"));

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
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message);
			}
		}

		private void ValidateDate()
		{
			DateTime begindate;
			DateTime enddate;
			string u_ID = tbQQID.Text.Trim();

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

			if(begindate.AddDays(3).CompareTo(enddate) < 0)
			{
				throw new Exception("选择时间段超过了三天，请重新输入！");
			}

			try
			{
				float tmp = float.Parse(tbFNum.Text.Trim());
			}
			catch
			{
				throw new Exception("请输入正确的金额！");
			}

			ViewState["fnum"] = tbFNum.Text.Trim();
			ViewState["fstate"] = ddlStateType.SelectedValue;

			ViewState["uid"] = u_ID;
			ViewState["begindate"] = DateTime.Parse(begindate.ToString("yyyy-MM-dd 00:00:00"));
			begintime = begindate.ToString("yyyy-MM-dd");
			ViewState["enddate"] = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59"));
			endtime = enddate.ToString("yyyy-MM-dd");

			ViewState["sorttype"] = ddlSortType.SelectedValue;
			ViewState["banktype"] = ddlBankType.SelectedValue;

		}


		protected void Button2_Click(object sender, System.EventArgs e)
		{
			Table2.Visible = false;

			try
			{
				ValidateDate();

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
				WebUtils.ShowMessage(this.Page, eSys.Message);
			}
		}

		private int GetCount()
		{
			string u_ID = ViewState["uid"].ToString();
			DateTime begindate = (DateTime)ViewState["begindate"];
			DateTime enddate = (DateTime)ViewState["enddate"];

			float fnum = float.Parse(ViewState["fnum"].ToString());
			int fstate = Int32.Parse(ViewState["fstate"].ToString());

			string banktype = ViewState["banktype"].ToString();

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			return qs.GetRealTimeOrderListCount(u_ID,begindate,enddate,fstate,fnum,banktype);
		}

		private void BindData(int index)
		{
			string u_ID = ViewState["uid"].ToString();
			DateTime begindate = (DateTime)ViewState["begindate"];
			DateTime enddate = (DateTime)ViewState["enddate"];

			string banktype = ViewState["banktype"].ToString();

			string sorttype = ViewState["sorttype"].ToString();

			begintime = begindate.ToString("yyyy-MM-dd");
			endtime = enddate.ToString("yyyy-MM-dd");

			float fnum = float.Parse(ViewState["fnum"].ToString());
			int fstate = Int32.Parse(ViewState["fstate"].ToString());

			int max = pager.PageSize;
			int start = max * (index-1) + 1;

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataSet ds = qs.GetRealTimeOrderList(u_ID,begindate,enddate,fstate,fnum,banktype,sorttype, start,max);

			if(ds != null && ds.Tables.Count >0)
			{
				ds.Tables[0].Columns.Add("FNewAmount",typeof(String));
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"FAmount","FNewAmount");

				ds.Tables[0].Columns.Add("FStatusName",typeof(String));
				ds.Tables[0].Columns.Add("FSignName",typeof(String));

				ds.Tables[0].Columns.Add("FBankName",typeof(String));
				
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					dr.BeginEdit();

					string tmp = dr["FStatus"].ToString();
					if(tmp == "0") 
						tmp = "已结帐";
					else if(tmp == "1") 
						tmp = "未结帐";
					else if(tmp == "2") 
						tmp = "部分结帐";
					else if(tmp == "3") 
						tmp = "付款失败";
					else if(tmp == "3") 
						tmp = "等待付款";
					else if(tmp == "4") 
						tmp = "等待付款";
					else if(tmp == "9") 
						tmp = "支付成功";
					
					dr["FStatusName"] = tmp;

					tmp = dr["FSign"].ToString();
					if(tmp == "0") 
						tmp = "初始状态";
					else if(tmp == "1") 
						tmp = "无需处理";
					else if(tmp == "2") 
						tmp = "需要调帐";
					else if(tmp == "3") 
						tmp = "调帐失败";
					else if(tmp == "4") 
						tmp = "原交易已成功";
					else if(tmp == "5") 
						tmp = "调账已经成功";
					else if(tmp == "6") 
						tmp = "订单匹配失败";

					dr["FSignName"] = tmp;

					tmp = dr["FBank_Type"].ToString();
                    dr["FBankName"] = Transfer.returnDicStr("BANK_TYPE", tmp);
					dr.EndEdit();
				}

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
