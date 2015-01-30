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
	/// TranDetail 的摘要说明。
	/// </summary>
	public partial class TranDetail : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			try
			{

				labUid.Text = "页面已废弃";
				return;
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				
				string listid = Request.QueryString["listid"];
				int trantype = Int32.Parse(Request.QueryString["trantype"]);

				if(listid == null || listid.Trim() == "")
				{
					WebUtils.ShowMessage(this.Page,"参数有误！");
				}


				try
				{
//					BindInfo(listid,trantype);
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

		private void BindInfo(string appealid, int trantype)
		{			
//			Query_Service.Query_Service qs = new Query_Service.Query_Service();
//
//			DataSet ds =  qs.GetTransportDetail(appealid,trantype);
//
//			if(ds != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count == 1 )
//			{
//				ds.Tables[0].Columns.Add("Ftransport_typeName");
//				ds.Tables[0].Columns.Add("Fgoods_typeName");
//				ds.Tables[0].Columns.Add("Ftran_typeName");
//				ds.Tables[0].Columns.Add("FstateName");
//
//				#region 转义
//				foreach(DataRow dr in ds.Tables[0].Rows)
//				{
//					string strtmp = dr["Ftransport_type"].ToString();
//					if(strtmp == "1")
//					{
//						dr["Ftransport_typeName"] = "卖方发货";
//					}
//					else if(strtmp == "2")
//					{
//						dr["Ftransport_typeName"] = "买方发送退货";
//					}						
//					else
//					{
//						dr["Ftransport_typeName"] = "未知类型" + strtmp;
//					}
//
//					strtmp = dr["Fgoods_type"].ToString();
//					if(strtmp == "1")
//					{
//						dr["Fgoods_typeName"] = "实物物品";
//					}
//					else if(strtmp == "2")
//					{
//						dr["Fgoods_typeName"] = "虚拟物品";
//					}						
//					else
//					{
//						dr["Fgoods_typeName"] = "未知类型" + strtmp;
//					}
//
//					strtmp = dr["Ftran_type"].ToString();
//					if(strtmp == "1")
//					{
//						dr["Ftran_typeName"] = "平邮";
//					}
//					else if(strtmp == "2")
//					{
//						dr["Ftran_typeName"] = "快递";
//					}			
//					else if(strtmp == "3")
//					{
//						dr["Ftran_typeName"] = "email发货";
//					}
//					else if(strtmp == "4")
//					{
//						dr["Ftran_typeName"] = "手机";
//					}
//					else if(strtmp == "5")
//					{
//						dr["Ftran_typeName"] = "其它";
//					}
//					else
//					{
//						dr["Ftran_typeName"] = "未知类型" + strtmp;
//					}
//					
//
//					
//					foreach(DataColumn dc in ds.Tables[0].Columns)
//					{
//						System.Web.UI.Control obj = FindControl(dc.ColumnName);
//						if(obj != null)
//						{
//							Label lab = (Label)obj;
//							lab.Text = ds.Tables[0].Rows[0][dc.ColumnName].ToString();
//						}
//					}
//
//				}
//
//				#endregion
//				
//			}
		}

	}
}
