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
	/// AppealQuery 的摘要说明。
	/// </summary>
	public partial class AppealQuery : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				this.BeginDate.Attributes.Add("onclick","openModeBegin()");
				this.EndDate.Attributes.Add("onclick","openModeEnd()");

				this.ModifyBeginDate.Attributes.Add("onclick","openModeModifyBegin()");
				this.ModifyEndDate.Attributes.Add("onclick","openModeModifyEnd()");

				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this))
					Response.Redirect("../login.aspx?wh=1");

				if(!IsPostBack)
				{
					this.PanelList.Visible = true;
					this.PanelMod.Visible = false;

					if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString().Trim() == "detail")
					{
						ShowDetail();
					}
				}
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

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			try
			{
				pager.RecordCount= 100;
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

		private void BindData(int index)
		{
			string errMsg = "";

			try
			{
				int max = pager.PageSize;
				int start = max * (index-1);

				this.pager.CurrentPageIndex = index;

				Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

				DataSet ds = qs.Gett_appealList(txtRequestDate.Text.Trim(), txtRequestDate1.Text.Trim(),txtRequestUpdDate.Text.Trim(),txtRequestUpdDate1.Text.Trim(),
					txtAppealID.Text.Trim(),txtOrderNo.Text.Trim(),txtAppealQQ.Text.Trim(),txtAppealedQQ.Text.Trim(),ddlAppealType.SelectedValue,ddlFcheck_state.SelectedValue,
					ddlAppealState.SelectedValue,ddlRefundFlag.SelectedValue,ddlResponseFlag.SelectedValue,start,max,out errMsg);

				if(ds != null && ds.Tables.Count >0)
				{
					ds.Tables[0].Columns.Add("FstateStr",typeof(string));
					ds.Tables[0].Columns.Add("Fcheck_stateStr",typeof(string));
					ds.Tables[0].Columns.Add("CLResult",typeof(string));
					ds.Tables[0].Columns.Add("KFResult",typeof(string));

					int kk = ds.Tables[0].Rows.Count;

					foreach(DataRow dr in ds.Tables[0].Rows)
					{
						string Fstate = dr["Fstate"].ToString();
						string Fresponse_flag = dr["Fresponse_flag"].ToString();
						string Fcheck_state = dr["Fcheck_state"].ToString();

						if(Fstate == "1" || Fstate == "2")                // 投诉单状态
						{
							if(Fresponse_flag == "1")
								dr["FstateStr"] = "未申诉";
							else
								dr["FstateStr"] = "已申诉";
						}
						else if(Fstate == "3")
							dr["FstateStr"] = "已结束";
						else if(Fstate == "4")
							dr["FstateStr"] = "已撤销";
						else
							dr["FstateStr"] = "";

						if(dr["Fappeal_type"].ToString() == "1")
							dr["Fappeal_type"] = "成交不买";
						else if(dr["Fappeal_type"].ToString() == "2")
							dr["Fappeal_type"] = "收货不付款（确认）";
						else if(dr["Fappeal_type"].ToString() == "3")
							dr["Fappeal_type"] = "退款纠纷（卖家投诉买家）";
						else if(dr["Fappeal_type"].ToString() == "4")
							dr["Fappeal_type"] = "买家恶意评价";
						else if(dr["Fappeal_type"].ToString() == "5")
							dr["Fappeal_type"] = "成交不卖";
						else if(dr["Fappeal_type"].ToString() == "6")
							dr["Fappeal_type"] = "卖家拒绝使用财付通交易";
						else if(dr["Fappeal_type"].ToString() == "7")
							dr["Fappeal_type"] = "收款不发货";
						else if(dr["Fappeal_type"].ToString() == "8")
							dr["Fappeal_type"] = "商品与描述不符";
						else if(dr["Fappeal_type"].ToString() == "9")
							dr["Fappeal_type"] = "卖家恶意评价";
						else if(dr["Fappeal_type"].ToString() == "10")
							dr["Fappeal_type"] = "退款纠纷（买家投诉卖家）";
						else if(dr["Fappeal_type"].ToString() == "11")
							dr["Fappeal_type"] = "卖家要求买家先确认收货，卖家再发货";
						else 
							dr["Fappeal_type"] = "Unknown";

						if(Fcheck_state == "1") //审核结果
							dr["Fcheck_stateStr"] = "未审核";
						else if(Fcheck_state == "2")
							dr["Fcheck_stateStr"] = "已提交";
						else if(Fcheck_state == "3")
							dr["Fcheck_stateStr"] = "审核不通过";
						else if(Fcheck_state == "4")
							dr["Fcheck_stateStr"] = "审核通过";
						else if(Fcheck_state == "5")
							dr["Fcheck_stateStr"] = "已退款";
						else
							dr["Fcheck_stateStr"] = "";

						if(Fstate == "3" || Fstate == "4")  //处理结果
							dr["CLResult"] = "关闭";
						else
							dr["CLResult"] = "无";

						if(Fstate == "1")  //客服审核状态
							dr["KFResult"] = "未处理";
						else if(Fstate == "2")
						{
							if(Fcheck_state == "1")
								dr["KFResult"] = "处理中";
							else if(Fcheck_state == "2")
								dr["KFResult"] = "提交审核";
							else if(Fcheck_state == "3")
								dr["KFResult"] = "审核未通过";
							else if(Fcheck_state == "4" || Fcheck_state == "5")
								dr["KFResult"] = "处理结束";
							else
								dr["KFResult"] = "";
						}
						else if(Fstate == "3" || Fstate == "4")
							dr["KFResult"] = "处理结束";
						else
							dr["KFResult"] = "";

					}
					DataGrid1.DataSource = ds.Tables[0].DefaultView;
					DataGrid1.DataBind();
				}
				else
				{
					throw new LogicException("没有找到记录！" + errMsg);
				}
			}
			catch
			{
				throw new Exception(errMsg);
			}
		}
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


		private void ShowDetail()
		{
			this.PanelList.Visible = false;
			this.PanelMod.Visible = true;

			string errMsg = "";
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			DataSet ds = qs.Gett_appealList("","","","",Request.QueryString["Fappealid"].ToString(),"","","","","","","","",0,1,out errMsg);

			if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 1)
			{
				throw new Exception("没有查找到相应的记录！");
			}
			else
			{
				string Fstate = ds.Tables[0].Rows[0]["Fstate"].ToString();

				if(Fstate == "1")
				{
					this.Button1.Visible = true;
					this.Button2.Visible = true;
					this.Button3.Visible = true;
					this.Button4.Visible = true;
				}
				else
				{
					this.Button1.Visible = false;
					this.Button2.Visible = false;
					this.Button3.Visible = false;
					this.Button4.Visible = false;
				}

				this.lblFqqid.Text = ds.Tables[0].Rows[0]["Fqqid"].ToString();
				this.lblFappealid.Text = ds.Tables[0].Rows[0]["Fappealid"].ToString();
				if(Fstate == "3" || Fstate == "4")  //处理结果
					this.lblFresponse_flag.Text = "关闭";
				else
					this.lblFresponse_flag.Text = "无";

				this.lblFvs_qqid.Text = ds.Tables[0].Rows[0]["Fvs_qqid"].ToString();
				this.lblFlistid.Text = ds.Tables[0].Rows[0]["Flistid"].ToString();
				this.lblFtotal_fee.Text = ds.Tables[0].Rows[0]["Ftotal_fee"].ToString();
				this.lblFpaybuy.Text = ds.Tables[0].Rows[0]["Fpaybuy"].ToString();

				this.lblFlist_state.Text = ""; //订单状态
				this.lblFtoken_fee.Text = ds.Tables[0].Rows[0]["Ftoken_fee"].ToString();
				this.lblFpaysale.Text = ds.Tables[0].Rows[0]["Fpaysale"].ToString();

				if(ds.Tables[0].Rows[0]["Fappeal_type"].ToString() == "1")
				{
					this.lblFappeal_type.Text = "2"; //卖家投诉买家
					this.lblFappeal_typeStr.Text = "成交不买";
				}
				else if(ds.Tables[0].Rows[0]["Fappeal_type"].ToString() == "2")
				{
					this.lblFappeal_type.Text = "2"; //卖家投诉买家
					this.lblFappeal_typeStr.Text = "收货不付款（确认）";
				}
				else if(ds.Tables[0].Rows[0]["Fappeal_type"].ToString() == "3")
				{
					this.lblFappeal_type.Text = "2"; //卖家投诉买家
					this.lblFappeal_typeStr.Text = "退款纠纷（卖家投诉买家）";
				}
				else if(ds.Tables[0].Rows[0]["Fappeal_type"].ToString() == "4")
					this.lblFappeal_typeStr.Text = "买家恶意评价";
				else if(ds.Tables[0].Rows[0]["Fappeal_type"].ToString() == "5")
				{
					this.lblFappeal_type.Text = "1"; //买家投诉卖家
					this.lblFappeal_typeStr.Text = "成交不卖";
				}
				else if(ds.Tables[0].Rows[0]["Fappeal_type"].ToString() == "6")
				{
					this.lblFappeal_type.Text = "1"; //买家投诉卖家
					this.lblFappeal_typeStr.Text = "卖家拒绝使用财付通交易";
				}
				else if(ds.Tables[0].Rows[0]["Fappeal_type"].ToString() == "7")
				{
					this.lblFappeal_type.Text = "1"; //买家投诉卖家
					this.lblFappeal_typeStr.Text = "收款不发货";
				}
				else if(ds.Tables[0].Rows[0]["Fappeal_type"].ToString() == "8")
				{
					this.lblFappeal_type.Text = "1"; //买家投诉卖家
					this.lblFappeal_typeStr.Text = "商品与描述不符";
				}
				else if(ds.Tables[0].Rows[0]["Fappeal_type"].ToString() == "9")
					this.lblFappeal_typeStr.Text = "卖家恶意评价";
				else if(ds.Tables[0].Rows[0]["Fappeal_type"].ToString() == "10")
				{
					this.lblFappeal_type.Text = "1"; //买家投诉卖家
					this.lblFappeal_typeStr.Text = "退款纠纷（买家投诉卖家）";
				}
				else if(ds.Tables[0].Rows[0]["Fappeal_type"].ToString() == "11")
				{
					this.lblFappeal_type.Text = "1"; //买家投诉卖家
					this.lblFappeal_typeStr.Text = "卖家要求买家先确认收货，卖家再发货";
				}
				else 
					this.lblFappeal_typeStr.Text = "Unknown";

				this.lblFappeal_time.Text = ds.Tables[0].Rows[0]["Fappeal_time"].ToString();
				this.lblFend_time.Text = ds.Tables[0].Rows[0]["Fend_time"].ToString();
				this.lblFappeal_con.Text = ds.Tables[0].Rows[0]["Fappeal_con"].ToString();
				this.lblFmemo.Text = ds.Tables[0].Rows[0]["Fmemo"].ToString();  //处理意见，待确定

				DataSet dsDetail = qs.Gett_message(this.lblFappealid.Text,0,100,out errMsg);

				string url = System.Configuration.ConfigurationManager.AppSettings["AppealUrlPath"].Trim();
				if(!url.EndsWith("/"))
					url += "/";

				if(dsDetail != null && dsDetail.Tables.Count >0)
				{
					dsDetail.Tables[0].Columns.Add("FtypeStr",typeof(string));
			
					foreach(DataRow dr in dsDetail.Tables[0].Rows)
					{
						if(dr["Ftype"].ToString() == "1")
							dr["FtypeStr"] = "投诉方留言";
						else if(dr["Ftype"].ToString() == "2")
							dr["FtypeStr"] = "被投诉方留言";
						else if(dr["Ftype"].ToString() == "3")
							dr["FtypeStr"] = "客服要求投诉方解释";
						else if(dr["Ftype"].ToString() == "4")
							dr["FtypeStr"] = "客服要求被投诉方解释";
						else if(dr["Ftype"].ToString() == "5")
							dr["FtypeStr"] = "客服要求双方解释";
						else if(dr["Ftype"].ToString() == "6")
							dr["FtypeStr"] = "客服上传数据";
						else if(dr["Ftype"].ToString() == "7")
							dr["FtypeStr"] = "客服提交上级审核";
						else if(dr["Ftype"].ToString() == "8")
							dr["FtypeStr"] = "审核不通过";
						else if(dr["Ftype"].ToString() == "9")
							dr["FtypeStr"] = "审核通过";
						else
							dr["FtypeStr"] = "";

						if(dr["Fattach1"].ToString() != "")
						{
							dr["Fattach1"] = url + dr["Fattach1"].ToString();
						}

						try
						{
							dr["Fmsg"] = HttpUtility.UrlDecode(dr["Fmsg"].ToString(),System.Text.Encoding.GetEncoding("GB2312"));
						}
						catch
						{
						}
						try
						{
							dr["Fmemo"] = HttpUtility.UrlDecode(dr["Fmemo"].ToString(),System.Text.Encoding.GetEncoding("GB2312"));
						}
						catch
						{
						}
					}
					Datagrid2.DataSource = dsDetail.Tables[0].DefaultView;
					Datagrid2.DataBind();
				}
				else
				{
					WebUtils.ShowMessage(this.Page,"查询明细列表失败！" + classLibrary.setConfig.replaceMStr(errMsg) );
				}
			}
		}

		protected void Button1_Click(object sender, System.EventArgs e)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			string msg = "";
			if(!qs.Savetp_add_msg(Request.QueryString["Fappealid"].ToString(),"5",Session["uid"].ToString(),Session["OperID"].ToString(),this.txtMess.Text.Trim(),"",out msg))
			{
				WebUtils.ShowMessage(this.Page,"操作失败！" + classLibrary.setConfig.replaceMStr(msg) );
			}
			else
			{
				ShowDetail();
			}
		}

		protected void Button2_Click(object sender, System.EventArgs e)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			string msg = "";
			if(!qs.Savetp_add_msg(Request.QueryString["Fappealid"].ToString(),"4",Session["uid"].ToString(),Session["OperID"].ToString(),this.txtMess.Text.Trim(),"",out msg))
			{
				WebUtils.ShowMessage(this.Page,"操作失败！" + classLibrary.setConfig.replaceMStr(msg) );
			}
			else
			{
				ShowDetail();
			}
		}

		protected void Button3_Click(object sender, System.EventArgs e)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			string msg = "";
			if(!qs.Savetp_add_msg(Request.QueryString["Fappealid"].ToString(),"3",Session["uid"].ToString(),Session["OperID"].ToString(),this.txtMess.Text.Trim(),"",out msg))
			{
				WebUtils.ShowMessage(this.Page,"操作失败！" + classLibrary.setConfig.replaceMStr(msg) );
			}
			else
			{
				ShowDetail();
			}
		}

		protected void Button4_Click(object sender, System.EventArgs e)
		{
			if(!this.IsFfund_flag.Checked && !this.NoFfund_flag.Checked)
			{
				WebUtils.ShowMessage(this.Page,"请选择是否涉及退款处理！");
				return;
			}

			Int64 AppealFee=0;
			Int64 AppealedFee=0;

			int Ffund_flag =  1;
			if(this.IsFfund_flag.Checked)
				Ffund_flag = 2;

			if(Ffund_flag==2)
			{
				if(this.lblFappeal_type.Text != "1" && this.lblFappeal_type.Text != "2")
				{
					WebUtils.ShowMessage(this.Page,"该投诉类型不应该涉及退款操作！");
					return;
				}

				AppealFee = Int64.Parse(this.lblFpaybuy.Text);
				AppealedFee = Int64.Parse(this.lblFpaysale.Text);

				if(AppealFee==0 && AppealedFee==0)
				{
					WebUtils.ShowMessage(this.Page,"金额为0，不涉及退款，请操作员检查！");
					return;
				}
			}

			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			string msg = "";
			if(!qs.SubmitApprove(Request.QueryString["Fappealid"].ToString(),this.lblFlistid.Text,Ffund_flag,this.lblFappeal_type.Text,AppealFee,AppealedFee,Session["uid"].ToString(),out msg))
			{
				WebUtils.ShowMessage(this.Page,"操作失败！" + classLibrary.setConfig.replaceMStr(msg) );
			}
			else
			{
				ShowDetail();
			}
		}

		protected void Button5_Click(object sender, System.EventArgs e)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
//			if(!qs.ApprovePass(Request.QueryString["Fappealid"].ToString(),this.lblFlistid.Text,Session["uid"].ToString(),out msg))
//			{
//				WebUtils.ShowMessage(this.Page,"操作失败！" + classLibrary.setConfig.replaceMStr(msg) );
//			}
//			else
//			{
//				ShowDetail();
//			}
		}
		protected void Button6_Click(object sender, System.EventArgs e)
		{
			Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
			string msg = "";
			if(!qs.ApproveCancel(Request.QueryString["Fappealid"].ToString(),this.lblFlistid.Text,Session["uid"].ToString(),out msg))
			{
				WebUtils.ShowMessage(this.Page,"操作失败！" + classLibrary.setConfig.replaceMStr(msg) );
			}
			else
			{
				ShowDetail();
			}
		}


	}
}
