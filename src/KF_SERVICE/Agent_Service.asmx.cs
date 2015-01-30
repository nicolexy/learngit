using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Service
{
	/// <summary>
	/// Agent_Service 的摘要说明。
	/// </summary>
	[WebService(Namespace="http://Tencent.com/OSS/C2C/Finance/Agent_Service")]
	public class Agent_Service : System.Web.Services.WebService
	{
		/// <summary>
		/// SOAP头
		/// </summary>
		public Finance_Header myHeader;

		public Agent_Service()
		{
			//CODEGEN: 该调用是 ASP.NET Web 服务设计器所必需的
			InitializeComponent();
		}

		#region 组件设计器生成的代码
		
		//Web 服务设计器所必需的
		private IContainer components = null;
				
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		// WEB 服务示例
		// HelloWorld() 示例服务返回字符串 Hello World
		// 若要生成，请取消注释下列行，然后保存并生成项目
		// 若要测试此 Web 服务，请按 F5 键

//		[WebMethod]
//		public string HelloWorld()
//		{
//			return "Hello World";
//		}


		
		[WebMethod(Description="获取邮储汇款商户")] 
		public string[] GetRemitSpid()
		{
			try
			{	
				string spidstr= System.Configuration.ConfigurationManager.AppSettings["RemitSpid"].ToString().Trim();
				if(spidstr!="")
				{
					string [] spids =spidstr.Split('|');
					return spids;
				}
				else
				{
					return null;
				}
			}
			catch(Exception err)
			{
				throw new LogicException("Service处理失败！"+err.Message);
			}
		}


		[WebMethod(Description="邮储汇款查询函数")] 
		[SoapHeader("myHeader", Direction=SoapHeaderDirection.In)]
		public DataSet GetRemitDataList(string batchid,string tranType,string dataType,string remitType,string tranState,string spid,string remitRec,string listID, int start,int max )
		{
			try
			{	
				RemitQueryClass cuser = new RemitQueryClass(batchid,tranType,dataType,remitType,tranState,spid,remitRec,listID);
				DataSet ds = cuser.GetResultX(start,max,"ZW");

				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
					return null;

				DataRow dr = ds.Tables[0].Rows[0];

				QueryRemitStateInfo query2 = new QueryRemitStateInfo(dr["Ford_date"].ToString(),dr["Ford_ssn"].ToString());

				DataSet ds2 = query2.GetResultX(0,1,"REMIT");

				if(ds2 == null || ds2.Tables.Count == 0 || ds2.Tables[0].Rows.Count == 0)
					return null;

				ds.Tables[0].Columns.Add("Fdest_name",typeof(string));
				ds.Tables[0].Columns.Add("Fdest_card",typeof(string));
				ds.Tables[0].Columns.Add("Fstate",typeof(string));

				ds.Tables[0].Rows[0]["Fdest_name"] = ds2.Tables[0].Rows[0]["Fdest_name"].ToString();
				ds.Tables[0].Rows[0]["Fdest_card"] = ds2.Tables[0].Rows[0]["Fdest_card"].ToString();

				switch(ds2.Tables[0].Rows[0]["Fstate"].ToString())
				{
					case "1":
					{
						ds.Tables[0].Rows[0]["Fstate"] = "支付前";break;
					}
					case "2":
					{
						ds.Tables[0].Rows[0]["Fstate"] = "支付成功";break;
					}
					case "3":
					{
						ds.Tables[0].Rows[0]["Fstate"] = "邮储已收讫";break;
					}
					case "4":
					{
						ds.Tables[0].Rows[0]["Fstate"] = "邮储收款确认";break;
					}	
					case "5":
					{
						ds.Tables[0].Rows[0]["Fstate"] = "邮储已兑付（收款人取钱）";break;
					}	
					case "6":
					{
						ds.Tables[0].Rows[0]["Fstate"] = "转入退汇";break;
					}	
					case "7":
					{
						ds.Tables[0].Rows[0]["Fstate"] = "退款成功";break;
					}	
					case "8":
					{
						ds.Tables[0].Rows[0]["Fstate"] = "已超期";break;
					}	
					case "9":
					{
						ds.Tables[0].Rows[0]["Fstate"] = "退款中";break;
					}
					case "10":
					{
						ds.Tables[0].Rows[0]["Fstate"] = "挂起失败";break;
					}
					default:
					{
						ds.Tables[0].Rows[0]["Fstate"] = "未定义的状态";break;
					}	
				}
				
				return ds;
			}
			catch(Exception err)
			{
				throw new LogicException("Service处理失败！"+err.Message);
			}
		}



		[WebMethod(Description="邮储汇款记录数查询函数")] 
		[SoapHeader("myHeader", Direction=SoapHeaderDirection.In)]
		public int GetRemitListCount(string batchid,string tranType,string dataType,string remitType,string tranState,string spid,string remitRec,string listID)
		{
			try
			{	
				RemitQueryClass cuser = new RemitQueryClass(batchid,tranType,dataType,remitType,tranState,spid,remitRec,listID);
				return cuser.GetCount("ZW");
			}
			catch(Exception err)
			{
				throw new LogicException("Service处理失败！"+err.Message);
			}
		}


	}
}
