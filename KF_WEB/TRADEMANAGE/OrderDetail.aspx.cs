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
	/// OrderDetail 的摘要说明。
	/// </summary>
	public partial class OrderDetail : System.Web.UI.Page
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
				string listid = Request.QueryString["listid"];

				if(listid == null || listid.Trim() == "")
				{
					WebUtils.ShowMessage(this.Page,"参数有误！");
				}

				try
				{
					BindInfo(listid);
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


		private void BindInfo(string listid)
		{

			Query_Service.Query_Service qs = new Query_Service.Query_Service();

			DataSet ds =  qs.GetQueryListDetail(listid);

			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0 )
			{
				hlTrade.NavigateUrl = "TradeLogQuery.aspx?id=" + listid;
				//金额类
				ds.Tables[0].Columns.Add("FpriceName");
				ds.Tables[0].Columns.Add("FcarriageName");
				ds.Tables[0].Columns.Add("FpaynumName");
				ds.Tables[0].Columns.Add("FfactName");
				ds.Tables[0].Columns.Add("FprocedureName");
				ds.Tables[0].Columns.Add("FcashName");
				ds.Tables[0].Columns.Add("FtokenName");
				ds.Tables[0].Columns.Add("Ffee3Name");
				ds.Tables[0].Columns.Add("FpaybuyName");
				ds.Tables[0].Columns.Add("FpaysaleName");
				//有数据字典类
				ds.Tables[0].Columns.Add("Fpay_typeName");
				ds.Tables[0].Columns.Add("Ftrade_stateName");				
				ds.Tables[0].Columns.Add("FlstateName");
				ds.Tables[0].Columns.Add("Ftrade_typeName");
				ds.Tables[0].Columns.Add("Fadjust_flagName");
				//手工转义吧
				ds.Tables[0].Columns.Add("Frefund_stateName");
				ds.Tables[0].Columns.Add("Fchannel_idName");
				ds.Tables[0].Columns.Add("Frefund_typeName");
				ds.Tables[0].Columns.Add("Fappeal_signName");
				ds.Tables[0].Columns.Add("Fmedi_signName");
				

				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fprice","FpriceName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fcarriage","FcarriageName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fpaynum","FpaynumName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Ffact","FfactName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fprocedure","FprocedureName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fcash","FcashName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Ffee3","Ffee3Name");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fpaybuy","FpaybuyName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Fpaysale","FpaysaleName");
				classLibrary.setConfig.FenToYuan_Table(ds.Tables[0],"Ftoken","FtokenName");

				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Fpay_type","Fpay_typeName","PAY_TYPE");
				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Ftrade_state","Ftrade_stateName","PAY_STATE");
				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Flstate","FlstateName","PAY_LSTATE");
				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Ftrade_type","Ftrade_typeName","PAYLIST_TYPE");
				classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0],"Fadjust_flag","Fadjust_flagName","ADJUST_FLAG");
				//显示定单信息
				DataRow drmain = ds.Tables[0].Rows[0];
				string strtmp = drmain["Frefund_state"].ToString();
				if(strtmp == "0")
				{
					drmain["Frefund_stateName"] = "初始状态";
				}
				else if(strtmp == "1")
				{
					drmain["Frefund_stateName"] = "可以退款(退款前)";
				}
				else if(strtmp == "2")
				{
					drmain["Frefund_stateName"] = "退款成功";
				}
				else if(strtmp == "3")
				{
					drmain["Frefund_stateName"] = "退款失败";
				}
				else if(strtmp == "4")
				{
					drmain["Frefund_stateName"] = "申请退款，等待卖家确认协议";
				}
				else if(strtmp == "5")
				{
					drmain["Frefund_stateName"] = "买家收到货，申请退货，等待卖家确认协议";
				}
				else if(strtmp == "6")
				{
					drmain["Frefund_stateName"] = "买家（要退款）卖家不同意退款协议，等待买家修改退款申请";
				}
				else if(strtmp == "7")
				{
					drmain["Frefund_stateName"] = "买家收到货（要退货）卖家不同意退货协议，等待买家修改退款申请";
				}
				else if(strtmp == "8")
				{
					drmain["Frefund_stateName"] = "卖家同意退货，等待买家发送退货";
				}
				else if(strtmp == "9")
				{
					drmain["Frefund_stateName"] = "卖家同意退款，交易结束（退款成功）";
				}
				else if(strtmp == "10")
				{
					drmain["Frefund_stateName"] = "买家已发退货，等待卖家确认";
				}
				else if(strtmp == "11")
				{
					drmain["Frefund_stateName"] = "卖家确认收到退货，交易结束（退款成功）";
				}
				else if(strtmp == "12")
				{
					drmain["Frefund_stateName"] = "买家已发退货，卖家不同意退款，等待买家修改退款申请";
				}
				else if(strtmp == "13")
				{
					drmain["Frefund_stateName"] = "买家已发退货，买家已修改退款申请，等待卖家确认";
				}
				else
				{
					drmain["Frefund_stateName"] = "未知类型" + strtmp;
				}

				strtmp = drmain["Fchannel_id"].ToString();
				if(strtmp == "1")
				{
					drmain["Fchannel_idName"] = "财付通";
				}
				else if(strtmp == "2")
				{
					drmain["Fchannel_idName"] = "拍拍网";
				}
				else if(strtmp == "3")
				{
					drmain["Fchannel_idName"] = "客户端小钱包";
				}
				else if(strtmp == "4")
				{
					drmain["Fchannel_idName"] = "手机支付";
				}
				else if(strtmp == "5")
				{
					drmain["Fchannel_idName"] = "第三方";
				}
				else
				{
					drmain["Fchannel_idName"] = "未知类型" + strtmp;
				}

				strtmp = drmain["Frefund_type"].ToString();	
				drmain["Frefund_typeName"] = strtmp;


				strtmp = drmain["Fappeal_sign"].ToString();
				if(strtmp == "1")
				{
					drmain["Fappeal_signName"] = "正常";
				}
				else if(strtmp == "2")
				{
					drmain["Fappeal_signName"] = "已转申诉";
				}
				else
				{
					drmain["Fappeal_signName"] = "未知类型" + strtmp;
				}

				strtmp = drmain["Fmedi_sign"].ToString();
				if(strtmp == "1")
				{
					drmain["Fmedi_signName"] = "是中介交易";
				}
				else if(strtmp == "2")
				{
					drmain["Fmedi_signName"] = "非中介交易";
				}
				else
				{
					drmain["Fmedi_signName"] = "未知类型" + strtmp;
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

				int iType = 1;

				DateTime beginTime = DateTime.Parse(PublicRes.sBeginTime);
				DateTime endTime   = DateTime.Parse(PublicRes.sEndTime);

				int istr = 1;
				int imax = 2;
		
				DataSet dsMemo = qs.GetPayList(listid,iType,beginTime,endTime,istr,imax);				
				if(dsMemo == null || dsMemo.Tables.Count<1 || dsMemo.Tables[0].Rows.Count<1) 
				{
					this.LB_Fexplain.Text = "查询失败！";
					this.LB_FMemo.Text = "查询失败！";
				}
				else
				{
					this.LB_FMemo.Text = dsMemo.Tables[0].Rows[0]["Fmemo"].ToString();
					this.LB_Fexplain.Text = dsMemo.Tables[0].Rows[0]["Fexplain"].ToString();
				}

				//显示投诉单信息.
				DataSet dsappeal = qs.GetAppealList(listid);
				if(dsappeal != null && dsappeal.Tables.Count >0)
				{
					dsappeal.Tables[0].Columns.Add("FstateName");
					dsappeal.Tables[0].Columns.Add("Fpunish_flagName");
					dsappeal.Tables[0].Columns.Add("Fcheck_stateName");
					dsappeal.Tables[0].Columns.Add("Fappeal_typeName");

					foreach(DataRow dr in dsappeal.Tables[0].Rows)
					{
						strtmp = dr["FState"].ToString();
						if(strtmp == "0")
						{
							dr["FstateName"] = "初始状态";
						}
						else if(strtmp == "1")
						{
							dr["FstateName"] = "申请投诉";
						}
						else if(strtmp == "2")
						{
							dr["FstateName"] = "投诉处理中";
						}
						else if(strtmp == "3")
						{
							dr["FstateName"] = "投诉处理完毕";
						}
						else if(strtmp == "4")
						{
							dr["FstateName"] = "取消投诉";
						}
						else
						{
							dr["FstateName"] = "未知类型" + strtmp;
						}

						strtmp = dr["Fpunish_flag"].ToString();
						if(strtmp == "1")
						{
							dr["Fpunish_flagName"] = "无需处罚";
						}
						else if(strtmp == "2")
						{
							dr["Fpunish_flagName"] = "处罚买家";
						}
						else if(strtmp == "3")
						{
							dr["Fpunish_flagName"] = "处罚卖家";
						}
						else
						{
							dr["Fpunish_flagName"] = "未知类型" + strtmp;
						}

						strtmp = dr["Fcheck_state"].ToString();
						if(strtmp == "1")
						{
							dr["Fcheck_stateName"] = "未审核";
						}
						else if(strtmp == "2")
						{
							dr["Fcheck_stateName"] = "已提交审核";
						}
						else if(strtmp == "3")
						{
							dr["Fcheck_stateName"] = "审核不通过";
						}
						else if(strtmp == "4")
						{
							dr["Fcheck_stateName"] = "审核通过";
						}
						else if(strtmp == "5")
						{
							dr["Fcheck_stateName"] = "已退款";
						}
						else
						{
							dr["Fcheck_stateName"] = "未知类型" + strtmp;
						}


						strtmp = dr["Fappeal_type"].ToString();
						if(strtmp == "1")
						{
							dr["Fappeal_typeName"] = "成交不买";
						}
						else if(strtmp == "2")
						{
							dr["Fappeal_typeName"] = "收货不付款（确认）";
						}
						else if(strtmp == "3")
						{
							dr["Fappeal_typeName"] = "（卖家投诉买家）退款纠纷";
						}
						else if(strtmp == "4")
						{
							dr["Fappeal_typeName"] = "买家恶意评价";
						}
						else if(strtmp == "5")
						{
							dr["Fappeal_typeName"] = "成交不卖";
						}
						else if(strtmp == "6")
						{
							dr["Fappeal_typeName"] = "卖家拒绝使用财付通交易";
						}
						else if(strtmp == "7")
						{
							dr["Fappeal_typeName"] = "收款不发货";
						}
						else if(strtmp == "8")
						{
							dr["Fappeal_typeName"] = "商品与描述不符";
						}
						else if(strtmp == "9")
						{
							dr["Fappeal_typeName"] = "卖家恶意评价";
						}
						else if(strtmp == "10")
						{
							dr["Fappeal_typeName"] = "（买家投诉卖家）退款纠纷";
						}
						else if(strtmp == "11")
						{
							dr["Fappeal_typeName"] = "卖家要求买家先确认收货";
						}
						else
						{
							dr["Fappeal_typeName"] = "未知类型" + strtmp;
						}
					}

					DataGrid1.DataSource = dsappeal.Tables[0].DefaultView;
				}

				//显示交易流水信息.
				DataSet dsuserpay = qs.GetUserpayList(listid);
				if(dsuserpay != null && dsuserpay.Tables.Count >0)
				{
					dsuserpay.Tables[0].Columns.Add("FtypeName");
					dsuserpay.Tables[0].Columns.Add("FsubjectName");
					dsuserpay.Tables[0].Columns.Add("FpaynumName");
					dsuserpay.Tables[0].Columns.Add("FpaybuyName");
					dsuserpay.Tables[0].Columns.Add("FpaysaleName");

					classLibrary.setConfig.GetColumnValueFromDic(dsuserpay.Tables[0],"Fsubject","FsubjectName","USERPAY_SUBJECT");
					foreach(DataRow dr in dsuserpay.Tables[0].Rows)
					{
						strtmp = dr["Ftype"].ToString();
						if(strtmp == "1")
						{
							dr["FtypeName"] = "入";
						}
						else if(strtmp == "2")
						{
							dr["FtypeName"] = "出";
						}						
						else
						{
							dr["FtypeName"] = "未知类型" + strtmp;
						}

//						strtmp = dr["Fsubject"].ToString();
//						if(strtmp == "1")
//						{
//							dr["FsubjectName"] = "请求生成订单";
//						}
//						else if(strtmp == "2")
//						{
//							dr["FsubjectName"] = "生成物流单";
//						}	
//						else if(strtmp == "3")
//						{
//							dr["FsubjectName"] = "订单支付成功";
//						}	
//						else if(strtmp == "4")
//						{
//							dr["FsubjectName"] = "卖家发货";
//						}	
//						else if(strtmp == "5")
//						{
//							dr["FsubjectName"] = "买家申请退款(卖家未发货)";
//						}	
//						else if(strtmp == "6")
//						{
//							dr["FsubjectName"] = "买家收货确认（未用）";
//						}	
//						else if(strtmp == "7")
//						{
//							dr["FsubjectName"] = "修改物流单";
//						}	
//						else if(strtmp == "8")
//						{
//							dr["FsubjectName"] = "买家收货确认";
//						}	
//						else if(strtmp == "9")
//						{
//							dr["FsubjectName"] = "修改订单价格";
//						}	
//						else if(strtmp == "10")
//						{
//							dr["FsubjectName"] = "我要付款请求";
//						}	
//						else if(strtmp == "11")
//						{
//							dr["FsubjectName"] = "我要付款确认";
//						}	
//						else if(strtmp == "12")
//						{
//							dr["FsubjectName"] = "我要付款拒绝";
//						}	
//						else if(strtmp == "13")
//						{
//							dr["FsubjectName"] = "交易关闭";
//						}	
//						else if(strtmp == "14")
//						{
//							dr["FsubjectName"] = "取消退款";
//						}	
//						else if(strtmp == "15")
//						{
//							dr["FsubjectName"] = "卖家不同意退款（买家请求退款）";
//						}	
//						else if(strtmp == "16")
//						{
//							dr["FsubjectName"] = "买家修改退款申请（买家请求退款）";
//						}	
//						else if(strtmp == "17")
//						{
//							dr["FsubjectName"] = "买家申请退款(卖家已发货，买家未收到货)";
//						}	
//						else if(strtmp == "18")
//						{
//							dr["FsubjectName"] = "卖家同意退款协议，（卖家已发货，买家要退货）";
//						}	
//						else if(strtmp == "19")
//						{
//							dr["FsubjectName"] = "申诉请求";
//						}	
//						else if(strtmp == "20")
//						{
//							dr["FsubjectName"] = "取消申诉";
//						}	
//						else if(strtmp == "21")
//						{
//							dr["FsubjectName"] = "卖家确认退款，（卖家未发货）";
//						}	
//						else if(strtmp == "22")
//						{
//							dr["FsubjectName"] = "交易过期";
//						}	
//						else if(strtmp == "23")
//						{
//							dr["FsubjectName"] = "交易作废";
//						}	
//						else
//						{
//							dr["FsubjectName"] = "未知类型" + strtmp;
//						}
					}

					classLibrary.setConfig.FenToYuan_Table(dsuserpay.Tables[0],"Fpaynum","FpaynumName");
					classLibrary.setConfig.FenToYuan_Table(dsuserpay.Tables[0],"Fpaybuy","FpaybuyName");
					classLibrary.setConfig.FenToYuan_Table(dsuserpay.Tables[0],"Fpaysale","FpaysaleName");

					DataGrid2.DataSource = dsuserpay.Tables[0].DefaultView;
				}

				//显示物流单
				DataSet dstransport = qs.GetTransportList(listid);
				if(dstransport != null && dstransport.Tables.Count >0)
				{
					dstransport.Tables[0].Columns.Add("Ftransport_typeName");
					dstransport.Tables[0].Columns.Add("Fgoods_typeName");
					dstransport.Tables[0].Columns.Add("Ftran_typeName");
					dstransport.Tables[0].Columns.Add("FstateName");

					foreach(DataRow dr in dstransport.Tables[0].Rows)
					{
						strtmp = dr["Ftransport_type"].ToString();
						if(strtmp == "1")
						{
							dr["Ftransport_typeName"] = "卖方发货";
						}
						else if(strtmp == "2")
						{
							dr["Ftransport_typeName"] = "买方发送退货";
						}						
						else
						{
							dr["Ftransport_typeName"] = "未知类型" + strtmp;
						}

						strtmp = dr["Fgoods_type"].ToString();
						if(strtmp == "1")
						{
							dr["Fgoods_typeName"] = "实物物品";
						}
						else if(strtmp == "2")
						{
							dr["Fgoods_typeName"] = "虚拟物品";
						}						
						else
						{
							dr["Fgoods_typeName"] = "未知类型" + strtmp;
						}

						strtmp = dr["Ftran_type"].ToString();
						if(strtmp == "1")
						{
							dr["Ftran_typeName"] = "平邮";
						}
						else if(strtmp == "2")
						{
							dr["Ftran_typeName"] = "快递";
						}			
						else if(strtmp == "3")
						{
							dr["Ftran_typeName"] = "email发货";
						}
						else if(strtmp == "4")
						{
							dr["Ftran_typeName"] = "手机";
						}
						else if(strtmp == "5")
						{
							dr["Ftran_typeName"] = "其它";
						}
						else
						{
							dr["Ftran_typeName"] = "未知类型" + strtmp;
						}
					}

					Datagrid3.DataSource = dstransport.Tables[0].DefaultView;
				}
				this.DataBind();
			}
			else
			{
				throw new LogicException("没有找到记录！");
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

	}
}
