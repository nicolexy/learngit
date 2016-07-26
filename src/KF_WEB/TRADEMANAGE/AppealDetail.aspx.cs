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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// AppealDetail 的摘要说明。
	/// </summary>
	public partial class AppealDetail : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			try
			{

				labUid.Text = Session["uid"].ToString();
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				
				string tdeid = Request.QueryString["appealid"];

				if(tdeid == null || tdeid.Trim() == "")
				{
					WebUtils.ShowMessage(this.Page,"参数有误！");
				}


				try
				{
					BindInfo(tdeid);
				}
				catch(LogicException err)
				{
					WebUtils.ShowMessage(this.Page,err.Message);
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

		private void BindInfo(string appealid)
		{
			
			Query_Service.Query_Service qs = new Query_Service.Query_Service();

			DataSet ds =  qs.GetAppealListDetail(appealid);

			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0 )
			{//金额类
				ds.Tables[0].Columns.Add("Ftotal_feeName");
				ds.Tables[0].Columns.Add("Ftoken_feeName");
				ds.Tables[0].Columns.Add("FpaybuyName");
				ds.Tables[0].Columns.Add("FpaysaleName");
				//有数据字典类
				ds.Tables[0].Columns.Add("Flist_stateName");
				ds.Tables[0].Columns.Add("Freturn_stateName");					
				ds.Tables[0].Columns.Add("Flist_state_nextName");
				ds.Tables[0].Columns.Add("Freturn_state_nextName");
				ds.Tables[0].Columns.Add("FlstateName");
			

				
				//手工转义吧
				ds.Tables[0].Columns.Add("Ftran_stateName");
				ds.Tables[0].Columns.Add("Ftran_state_nextName");
				ds.Tables[0].Columns.Add("FstateName");
				ds.Tables[0].Columns.Add("Fresponse_flagName");
				ds.Tables[0].Columns.Add("Fpunish_flagName");
				ds.Tables[0].Columns.Add("Fcheck_stateName");
				ds.Tables[0].Columns.Add("Ffund_flagName");
				ds.Tables[0].Columns.Add("Fappeal_typeName");

				
				

				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Ftotal_fee","Ftotal_feeName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Ftoken_fee","Ftoken_feeName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fpaybuy","FpaybuyName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fpaysale","FpaysaleName");

				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Flist_state","Flist_stateName","PAY_STATE");
				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Freturn_state","Freturn_stateName","RLIST_STATE");
				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Flist_state_next","Flist_state_nextName","PAY_STATE");
				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Freturn_state_next","Freturn_state_nextName","RLIST_STATE");
				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Flstate","FlstateName","PAY_LSTATE");
				//显示定单信息
				DataRow drmain = ds.Tables[0].Rows[0];
				string strtmp = drmain["Ftran_state"].ToString();
				if(strtmp == "1")
				{
					drmain["Ftran_stateName"] = "正常";
				}
				else if(strtmp == "2")
				{
					drmain["Ftran_stateName"] = "冻结";
				}
				else if(strtmp == "3")
				{
					drmain["Ftran_stateName"] = "作废";
				}				
				else
				{
					drmain["Ftran_stateName"] = "未知类型" + strtmp;
				}

				strtmp = drmain["Ftran_state_next"].ToString();
				if(strtmp == "1")
				{
					drmain["Ftran_state_nextName"] = "正常";
				}
				else if(strtmp == "2")
				{
					drmain["Ftran_state_nextName"] = "冻结";
				}
				else if(strtmp == "3")
				{
					drmain["Ftran_state_nextName"] = "作废";
				}				
				else
				{
					drmain["Ftran_state_nextName"] = "未知类型" + strtmp;
				}

				strtmp = drmain["Fstate"].ToString();
				if(strtmp == "0")
				{
					drmain["FstateName"] = "初始状态";
				}
				else if(strtmp == "1")
				{
					drmain["FstateName"] = "申请投诉";
				}
				else if(strtmp == "2")
				{
					drmain["FstateName"] = "投诉处理中";
				}
				else if(strtmp == "3")
				{
					drmain["FstateName"] = "投诉处理完毕";
				}	
				else if(strtmp == "4")
				{
					drmain["FstateName"] = "取消投诉";
				}	
				else
				{
					drmain["FstateName"] = "未知类型" + strtmp;
				}

				strtmp = drmain["Fresponse_flag"].ToString();
				if(strtmp == "1")
				{
					drmain["Fresponse_flagName"] = "无申诉";
				}
				else if(strtmp == "2")
				{
					drmain["Fresponse_flagName"] = "已申诉";
				}			
				else
				{
					drmain["Fresponse_flagName"] = "未知类型" + strtmp;
				}


				strtmp = drmain["Fpunish_flag"].ToString();
				if(strtmp == "1")
				{
					drmain["Fpunish_flagName"] = "无需处罚";
				}
				else if(strtmp == "2")
				{
					drmain["Fpunish_flagName"] = "处罚买家";
				}		
				else if(strtmp == "3")
				{
					drmain["Fpunish_flagName"] = "处罚卖家";
				}	
				else
				{
					drmain["Fpunish_flagName"] = "未知类型" + strtmp;
				}

				strtmp = drmain["Fcheck_state"].ToString();
				if(strtmp == "1")
				{
					drmain["Fcheck_stateName"] = "未审核";
				}
				else if(strtmp == "2")
				{
					drmain["Fcheck_stateName"] = "已提交审核";
				}		
				else if(strtmp == "3")
				{
					drmain["Fcheck_stateName"] = "审核不通过";
				}	
				else if(strtmp == "4")
				{
					drmain["Fcheck_stateName"] = "审核通过";
				}	
				else if(strtmp == "5")
				{
					drmain["Fcheck_stateName"] = "已退款";
				}	
				else
				{
					drmain["Fcheck_stateName"] = "未知类型" + strtmp;
				}

				strtmp = drmain["Ffund_flag"].ToString();
				if(strtmp == "1")
				{
					drmain["Ffund_flagName"] = "不涉及";
				}
				else if(strtmp == "2")
				{
					drmain["Ffund_flagName"] = "涉及";
				}						
				else
				{
					drmain["Ffund_flagName"] = "未知类型" + strtmp;
				}

				strtmp = drmain["Fappeal_type"].ToString();
				if(strtmp == "1")
				{
					drmain["Fappeal_typeName"] = "成交不买";
				}
				else if(strtmp == "2")
				{
					drmain["Fappeal_typeName"] = "收货不付款（确认）";
				}
				else if(strtmp == "3")
				{
					drmain["Fappeal_typeName"] = "（卖家投诉买家）退款纠纷";
				}
				else if(strtmp == "4")
				{
					drmain["Fappeal_typeName"] = "买家恶意评价";
				}
				else if(strtmp == "5")
				{
					drmain["Fappeal_typeName"] = "成交不卖";
				}
				else if(strtmp == "6")
				{
					drmain["Fappeal_typeName"] = "卖家拒绝使用财付通交易";
				}
				else if(strtmp == "7")
				{
					drmain["Fappeal_typeName"] = "收款不发货";
				}
				else if(strtmp == "8")
				{
					drmain["Fappeal_typeName"] = "商品与描述不符";
				}
				else if(strtmp == "9")
				{
					drmain["Fappeal_typeName"] = "卖家恶意评价";
				}
				else if(strtmp == "10")
				{
					drmain["Fappeal_typeName"] = "（买家投诉卖家）退款纠纷";
				}
				else if(strtmp == "11")
				{
					drmain["Fappeal_typeName"] = "卖家要求买家先确认收货";
				}
				else
				{
					drmain["Fappeal_typeName"] = "未知类型" + strtmp;
				}

				foreach(DataColumn dc in ds.Tables[0].Columns)
				{
					System.Web.UI.Control obj = FindControl(dc.ColumnName);
					if(obj != null)
					{
						Label lab = (Label)obj;
						lab.Text = drmain[dc.ColumnName].ToString();
					}
				}

			}
			else
			{
				throw new LogicException("没有找到记录！");
			}
		}

	}
}
