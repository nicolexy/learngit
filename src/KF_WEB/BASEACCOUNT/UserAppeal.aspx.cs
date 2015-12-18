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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.Common;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using CFT.Apollo.Logging;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
    /// UserAppeal 的摘要说明。
	/// </summary>
	public partial class UserAppeal : System.Web.UI.Page
	{
       
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			// 在此处放置用户代码以初始化页面
			ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
			ButtonEndDate.Attributes.Add("onclick", "openModeEnd()"); 

			try
			{
				Label1.Text = Session["uid"].ToString();

				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"CFTUserPick")) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
                //一级权限
                ViewState["CFTUserPick"] = classLibrary.ClassLib.ValidateRight("CFTUserPick", this);
                //二级权限
                ViewState["CFTUserPickQuer"] = classLibrary.ClassLib.ValidateRight("CFTUserPickQuer", this);

                if (!(bool.Parse(ViewState["CFTUserPick"].ToString()) || bool.Parse(ViewState["CFTUserPickQuer"].ToString())))
                    Response.Redirect("../login.aspx?wh=1");

				TextBoxBeginDate.Text = DateTime.Now.AddDays(-3).ToString("yyyy年MM月dd日");
				TextBoxEndDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
				DropDownListShow();

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

            if (!classLibrary.getData.IsTestMode)
                if (this.tbFuin.Text == "" && begindate.AddDays(30) < enddate)
                {
                    throw new Exception("账号为空，日期间隔大于30天，请重新输入！");
                }

			ViewState["fstate"] = ddlState.SelectedValue;
			ViewState["fstateUserClass"] = ddlStateUserClass.SelectedValue;

			ViewState["fuin"] = classLibrary.setConfig.replaceMStr(tbFuin.Text.Trim());

			ViewState["begindate"] = DateTime.Parse(begindate.ToString("yyyy-MM-dd 00:00:00"));
			ViewState["enddate"] = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59"));

			ViewState["ftype"] = ddlType.SelectedValue;
			ViewState["QQType"] = ddlQQType.SelectedValue;
            ViewState["SortType"] = ddlSortType.SelectedValue;//排序
            

			//增加高分单人工单查询条件。
			ViewState["dotype"] = DDL_DoType.SelectedValue;
		}

		protected void Button2_Click(object sender, System.EventArgs e)
		{
			try
			{
				ValidateDate();
			}
			catch(Exception err)
            {
                LogHelper.LogError("出现异常：" + err.ToString(), "UserAppeal");
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
                LogHelper.LogError("调用服务出错：" + eSoap.ToString(), "UserAppeal");
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
            {
                LogHelper.LogError("读取数据失败：" + eSys.ToString(), "UserAppeal");
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
			}
		}

		private int GetCount()
		{
			string fuin = ViewState["fuin"].ToString();
			DateTime begin = (DateTime)ViewState["begindate"];
			DateTime end = (DateTime)ViewState["enddate"];

			string begindate = begin.ToString("yyyy-MM-dd 00:00:00");
			string enddate = end.ToString("yyyy-MM-dd 23:59:59");

			int fstate = Int32.Parse(ViewState["fstate"].ToString());
			int fstateUserClass = Int32.Parse(ViewState["fstateUserClass"].ToString());
			int ftype = Int32.Parse(ViewState["ftype"].ToString());
			string QQType = ViewState["QQType"].ToString();
            int SortType = Int32.Parse(ViewState["SortType"].ToString());//排序

			//增加高分单低分单查询
			string dotype = ViewState["dotype"].ToString();
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

			int Count;
            if (ftype == 20)
			{
                Count = qs.GetUserClassQueryCount(begindate, enddate, fuin, fstateUserClass, QQType, SortType);
			}
            if (ftype == 1 || ftype == 5 || ftype == 6 || ftype == 99)//三种类型在原来基础上进行分库分表
            {
                Count = qs.GetCFTUserAppealCountNew(fuin, begindate, enddate, fstate, ftype, QQType, dotype, SortType);
            }
            else
            {
                Count = qs.GetCFTUserAppealCount(fuin, begindate, enddate, fstate, ftype, QQType, dotype, SortType);
            }

			this.lblTotal.Text = "记录条数:" + Count;
			return Count;
		}

		private void BindData(int index)
		{
            pager.CurrentPageIndex = index;
			string fuin = ViewState["fuin"].ToString();
			DateTime begin = (DateTime)ViewState["begindate"];
			DateTime end = (DateTime)ViewState["enddate"];

			string begindate = begin.ToString("yyyy-MM-dd 00:00:00");
			string enddate = end.ToString("yyyy-MM-dd 23:59:59");

			int fstate = Int32.Parse(ViewState["fstate"].ToString());
			int fstateUserClass = Int32.Parse(ViewState["fstateUserClass"].ToString());
			int ftype = Int32.Parse(ViewState["ftype"].ToString());
			string QQType = ViewState["QQType"].ToString();
            int SortType = Int32.Parse(ViewState["SortType"].ToString());//排序
            

			int max = pager.PageSize;
			int start = max * (index-1) + 1;

			//增加高分单低分单查询
			string dotype = ViewState["dotype"].ToString();

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			Finance_Header fh = classLibrary.setConfig.setFH(this);
			qs.Finance_HeaderValue = fh;

            DataSet ds = new DataSet();
			if(ftype == 20)
			{
                ds = qs.GetUserClassQueryList(begindate, enddate, fuin, fstateUserClass, QQType, start, max, SortType);
			}
            if (ftype == 1 || ftype == 5 || ftype == 6 || ftype == 99)//三种类型在原来基础上进行分库分表
            {
               DataSet dsNew = qs.GetCFTUserAppealListNew(fuin, begindate, enddate, fstate, ftype, QQType, dotype, start, max, SortType);
                //处理分页问题
                if (dsNew != null && dsNew.Tables.Count > 0 && dsNew.Tables[0].Rows.Count > 0)
                {
                    //排序
                    DataTable dt = dsNew.Tables[0];
                    DataView view = dt.DefaultView;
                    if (SortType==0)
                        view.Sort = "FSubmitTime asc";
                    if (SortType == 1)
                        view.Sort = "FSubmitTime desc";
                    dt = view.ToTable();
                    dt = PublicRes.GetPagedTable(dt, index, pager.PageSize);
                    ds.Tables.Add(dt);
                }

                ds = qs.GetCFTUserAppealListFunction(ds);//先排序再处理内部信息
            }
			else
			{
                ds = qs.GetCFTUserAppealList(fuin, begindate, enddate, fstate, ftype, QQType, dotype, start, max, SortType);
			}

			if(ds != null && ds.Tables.Count >0)
			{
				ds.Tables[0].Columns.Add("URL",typeof(string));

                if (ftype == 20 && ds.Tables[0].Rows.Count > 0)
				{
					ds.Tables[0].Columns.Add("FUin",typeof(String));
					ds.Tables[0].Columns.Add("Femail",typeof(String));
					ds.Tables[0].Columns.Add("FTypeName",typeof(String));
					ds.Tables[0].Columns.Add("FStateName",typeof(String));
					ds.Tables[0].Columns.Add("FSubmitTime",typeof(String));
					ds.Tables[0].Columns.Add("FCheckInfo",typeof(String));

					foreach(DataRow dr in ds.Tables[0].Rows)
					{
						dr["FUin"] = dr["Fqqid"].ToString();
						dr["FTypeName"] = "实名认证";
						dr["FStateName"] = dr["FpickstateName"].ToString();
						dr["FSubmitTime"] = dr["Fcreate_time"].ToString();
						dr["FCheckInfo"] = dr["Fmemo"].ToString();
                        dr["URL"] = "CFTUserCheck.aspx?fid=&db=&tb=&flist_id=" + dr["flist_id"].ToString();

						//在这里增加大金额红字显示。（因为已经有VIP查询选择项，所以不用处理VIP红字）
						if(dr["Fuincolor"].ToString() == "BIGMONEY")
						{
							dr["FUin"] = "<FONT color=\"red\">" + dr["Fuin"] + "</FONT>";
						}
					}
				}
                else
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (ftype == 1 || ftype == 5 || ftype == 6 || ftype == 99)
                        {
                            //这四种类型增加数据库、表两个参数
                            dr["URL"] = "CFTUserCheck.aspx?fid=" + dr["FID"].ToString() + "&flist_id=&db=" + dr["DBName"]+"&tb="+dr["tableName"];
                        }
                        else
                        {
                            dr["URL"] = "CFTUserCheck.aspx?fid=" + dr["FID"].ToString() + "&flist_id=&db=&tb=";
                        }
                        //在这里增加大金额红字显示。（因为已经有VIP查询选择项，所以不用处理VIP红字）
                        if (dr["Fuincolor"].ToString() == "BIGMONEY")
                        {
                            dr["FUin"] = "<FONT color=\"red\">" + dr["Fuin"] + "</FONT>";
                        }
                    }
                }

				DataGrid1.DataSource = ds.Tables[0].DefaultView;
				DataGrid1.DataBind();
			}
			else
			{
				throw new LogicException("没有找到记录！");
			}
		}

		protected void ddlType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DropDownListShow();
		}

		private void DropDownListShow()
		{
			if(this.ddlType.SelectedValue == "20")
			{
				this.ddlState.Visible = false;
				this.ddlStateUserClass.Visible = true;
			}
			else
			{
				this.ddlState.Visible = true;
				this.ddlStateUserClass.Visible = false;
			}
		}

		protected void btnGet_Click(object sender, System.EventArgs e)
		{
            try
            {
                ValidateDate();
            }
            catch (Exception err)
            {
                LogHelper.LogError("获取提交参数异常：" + err.ToString(), "UserAppeal");
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }

			try
			{
				DateTime begindate;
				DateTime enddate;
				int TicketsCount;

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

				try
				{
					TicketsCount = int.Parse(this.txtCount.Text.Trim());
				}
				catch
				{
					throw new Exception("请输入正确的工单数！");
				}
				if(TicketsCount < 1)
				{
					throw new Exception("工单数不允许小于1！");
				}
				if(TicketsCount > 50)
				{
					throw new Exception("工单数的最大值为50！");
				}

				if(!classLibrary.getData.IsTestMode)
					if(begindate.AddDays(30) < enddate)
					{
						throw new Exception("日期间隔大于30天，请重新输入！");
					}

                //增加高分单人工单查询条件。
                ViewState["dotype"] = DDL_DoType.SelectedValue;

                int SortType = Int32.Parse(ViewState["SortType"].ToString());//排序

				if(ddlType.SelectedValue == "20")
				{
					if(ddlStateUserClass.SelectedValue != "99" && ddlStateUserClass.SelectedValue != "0" && ddlStateUserClass.SelectedValue != "1")//0才允许批量领单,这里的99是为了领单方便,在领单是会变成0,新增1也许领了
					{
						throw new Exception("该状态不允许领单");
					}
					Response.Write("<script>window.open('UserClassCheck.aspx?BeginDate=" + begindate.ToString("yyyy-MM-dd") + "&EndDate=" + enddate.ToString("yyyy-MM-dd") + "&fstate=" +
						ddlStateUserClass.SelectedValue + "&Count=" + TicketsCount.ToString() + "&SortType=" + SortType + "','_blank','');</script>");
				}
				else
				{
					if(ddlState.SelectedValue != "99" && ddlState.SelectedValue != "0" && ddlState.SelectedValue != "3" && ddlState.SelectedValue != "4"
						&& ddlState.SelectedValue != "5" && ddlState.SelectedValue != "6" && ddlState.SelectedValue != "8")//0,3,4,5,6才允许批量领单,这里的99是为了领单方便,在领单是会变成0,3,4,5,6，,新增8也许领了
					{
						throw new Exception("该申诉状态不允许领单");
					}
					Response.Write("<script>window.open('UserAppealCheck.aspx?BeginDate=" + begindate.ToString("yyyy-MM-dd") + "&EndDate=" + enddate.ToString("yyyy-MM-dd") + "&fstate=" +
                        ddlState.SelectedValue + "&ftype=" + ddlType.SelectedValue + "&qqtype=" + ddlQQType.SelectedValue + "&Count=" + TicketsCount.ToString() + "&SortType=" + SortType + "&dotype=" + DDL_DoType.SelectedValue + "','_blank','');</script>");
				}
			}
			catch(Exception err)
            {
                LogHelper.LogError("获取信息异常：" + err.ToString(), "UserAppeal");
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}
		}

        //详细内容按钮
        public void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            object obj = e.Item.Cells[8].FindControl("queryButton");
            if (obj != null)
            {
                long balance = long.Parse(e.Item.Cells[7].Text.Trim());//账户资金
                LinkButton lb = (LinkButton)obj;
                if (balance / 100 >= 1000)//账户资金在1000元（包含1000元）以上的需申请一级权限（可处理任何金额的申诉）
                {
                    if (bool.Parse(ViewState["CFTUserPick"].ToString()))
                    {
                        lb.Visible = true;
                    }
                }
                else//1000元以下审批，已登录页面就说明有权限
                {
                    lb.Visible = true;
                }
            }
        }   
	}
}