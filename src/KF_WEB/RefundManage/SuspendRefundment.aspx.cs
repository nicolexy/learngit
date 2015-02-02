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

namespace TENCENT.OSS.CFT.KF.KF_Web.RefundManage
{
	/// <summary>
	/// SuspendRefundment 的摘要说明。
	/// </summary>
	public partial class SuspendRefundment : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
			ButtonEndDate.Attributes.Add("onclick", "openModeEnd()"); 
			
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");
				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				btnSuspend.Attributes["onClick"]= "return confirm('确定要执行退款撤销操作吗？');";
				TextBoxBeginDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
				TextBoxEndDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
				Table2.Visible = false;		
			
				BindBankType(ddlBankType);
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

		#region 本地方法
		void BindBankType(DropDownList DropDownList1)
		{
			classLibrary.setConfig.GetAllBankList(DropDownList1);
			DropDownList1.Items.Insert(0,new ListItem("所有银行","0000"));
		}

		private void ValidateDate()
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
			
			ViewState["spid"] = tbSPID.Text.Trim();
			
			
			ViewState["begindate"] = begindate.ToString("yyyy-MM-dd 00:00:00");
			ViewState["enddate"] = enddate.ToString("yyyy-MM-dd 23:59:59");
			
			ViewState["refundtype"] = ddlrefund_type.SelectedValue;
			ViewState["status"] = ddlStatus.SelectedValue.Trim();

			ViewState["transid"] = tbTransID.Text.Trim();
			ViewState["buyqq"] = tbBuyerID.Text.Trim();

			ViewState["banktype"] = ddlBankType.SelectedValue;

			ViewState["sumtype"] =ddlSumType.SelectedValue;

			ViewState["drawid"]=tbDrawID.Text.Trim();
		}

		private void BindData(int index)
		{
			Label12.Text="";
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
			
			FQuery_Service.Query_Service fq = new FQuery_Service.Query_Service();
			DataSet ds = fq.GetB2cReturnList(spid,begindate,enddate,refundtype,status,transid,buyqq,banktype,sumtype,drawid,1, start,max);
			
			if(ds != null && ds.Tables.Count >0)
			{			
				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();

				ShowSuspendButton(ds.Tables[0]);
			}
			else
			{
				this.btnSuspend.Visible=false;
				//throw new LogicException("没有找到记录！");
				Label12.Text="没有找到记录！";
				
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

		public static string GetTransUrl(string listID)
		{
			string returnUrl = "";

			if(listID.Length == 21)
			{
				string checkdate=listID.Trim().Substring(3,8).Trim();
				returnUrl = "../TradeManage/FundQuery.aspx?czID=" + listID+"&checkdate="+checkdate+"";  ;
			}
			else if(listID.Length == 28)
			{
				returnUrl = "../TradeManage/TradeLogQuery.aspx?id=" + listID;
			}

			return returnUrl;
		}

		private ArrayList GetAllSelect()
		{
			ArrayList al = new ArrayList();
			for(int i = 0; i < DataGrid1.Items.Count; i++)
			{
				System.Web.UI.Control obj = DataGrid1.Items[i].Cells[10].FindControl("CheckBox1");
				if(obj != null)	
				{
					CheckBox cb = (CheckBox)obj;
					string fid = DataGrid1.Items[i].Cells[8].Text.Trim();

					string drawid = DataGrid1.Items[i].Cells[13].Text.Trim();
					if(drawid=="&nbsp;")
					{
						drawid="";
					}

					string refund_type=DataGrid1.Items[i].Cells[12].Text.Trim();

					if(cb.Checked &&drawid=="")
					{
						throw new LogicException("交易单:"+fid+"对应的提现单ID为空");
					}
					if(cb.Checked && fid != "异常标识")
					{
						//al.Add(fid);
						al.Add(fid + ";" + drawid+";"+refund_type);
					}
				}
			}
			return al;
		}

		private void ShowSuspendButton(DataTable dt)
		{
			if(dt!=null&&dt.Rows.Count>0)
			{
				bool showflag=false;
				foreach(DataRow dr in dt.Rows)
				{
					if(dr["Fstatus"].ToString()=="9"&dr["fstandby1"].ToString()=="0")
					{
						showflag=true;
						break;
					}
				}

				if(showflag)
				{
					this.btnSuspend.Visible=true;
				}
				else
				{
					this.btnSuspend.Visible=false;
				}

			}
			else
			{
				this.btnSuspend.Visible=false;
			}
		}

		#endregion

		#region 事件

		public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
		{
			try
			{
				pager.CurrentPageIndex = e.NewPageIndex;
				BindData(e.NewPageIndex);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,eSys.Message.ToString());
			}
		}

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			try
			{
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

		

		private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			string tmp = e.Item.Cells[9].Text.Trim();
			string refundtype = e.Item.Cells[12].Text.Trim();
			
			string sumtype=e.Item.Cells[14].Text.Trim();
			//增加业务状态为退款中，汇总状态为尚未汇总可以看见CheckBox
			
			if((tmp=="9"&&sumtype=="0"))
			{				
				System.Web.UI.Control obj = e.Item.Cells[10].FindControl("CheckBox1");
				if(obj != null )obj.Visible = true;
				
			}
			
		}
		protected void btnSuspend_Click(object sender, System.EventArgs e)
		{
			
			try
			{
				
				ArrayList al = GetAllSelect();

				if(al.Count > 0)
				{
					FQuery_Service.Query_Service fs = new FQuery_Service.Query_Service();
					FQuery_Service.Finance_Header Ffh = classLibrary.setConfig.FsetFH(this);
					fs.Finance_HeaderValue = Ffh;

//					FQuery_Service.Finance_Header fh = new FQuery_Service.Finance_Header();
//					fh.UserIP = Request.UserHostAddress;
//					fh.UserName = Session["uid"].ToString();
//					fh.UserPassword = Session["pwd"].ToString();
//					fh.SzKey = Session["SzKey"].ToString();
//					fh.RightString = Session["key"].ToString();
//					fh.OperID = Int32.Parse(Session["OperID"].ToString());
//					fs.Finance_HeaderValue = fh;

					object[] newal = new object[al.Count];
					al.CopyTo(newal,0);

					string msg ;
					if(fs.SuspendRefundment(newal,out msg))
					{
						WebUtils.ShowMessage(this.Page,"执行撤销退款成功！");
					}
					else
					{
						msg=msg.Replace("\'","\\\'");
						WebUtils.ShowMessage(this.Page,"执行撤销退款失败 \\r\\n" + msg);
					}
				}
				else
				{
					WebUtils.ShowMessage(this.Page,"请先勾选需要撤销退款的数据!");
				}

				BindData(pager.CurrentPageIndex);
			}
			catch(Exception err)
			{
				string msg=err.Message.Replace("\'","\\\'");
				WebUtils.ShowMessage(this.Page,"执行撤销退款失败:" + msg);
			}

		
		}

		#endregion
	}
}
