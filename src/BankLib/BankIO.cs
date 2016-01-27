using System;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.C2C.Finance.Common;
using System.Collections;
using System.Configuration;
using System.Data.OleDb;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using System.Security.Cryptography;

namespace TENCENT.OSS.C2C.Finance.BankLib
{

	
	/// <summary>
	/// 传递参数类。
	/// </summary>
	public struct Param
	{
		/// <summary>
		/// 参数名称
		/// </summary>
		public string ParamName;
		/// <summary>
		/// 参数值
		/// </summary>
		public string ParamValue;
		/// <summary>
		/// 标识
		/// </summary>
		public string ParamFlag;
	}

	/// <summary>
	/// BankIO 的摘要说明。
	/// </summary>
	public class BankIO
	{
		public BankIO()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

        public static Hashtable GetBankHashTable()
        {
            System.Web.Caching.Cache objCache = System.Web.HttpRuntime.Cache;
            if (objCache["BANK_TYPE"] == null)
                queryDic("BANK_TYPE");
            Hashtable ht = new Hashtable();
            ht = (Hashtable)objCache["BANK_TYPE"];
            return ht;
        }

		public static string QueryBankName(string bankType)
		{
			try
			{
				if(bankType==null||bankType=="")
				{
					return "";
				}
				if(bankType == "9999")
					return "汇总银行";

				return returnDicStr("BANK_TYPE",bankType);

			}
			catch(Exception err)
			{
				throw new Exception("获取银行名称失败：" + err);
			}

		}

		public static string returnDicStr(string type,string sType)
		{
			try
			{
				if (sType == null || sType == "")  //传入空，则返回空
				{
					return "";
				}
				else
				{
					System.Web.Caching.Cache objCache = System.Web.HttpRuntime.Cache;

					Hashtable ht = new Hashtable();
					if (objCache[type] == null)
						queryDic(type);
					ht = (Hashtable)objCache[type];
					if(ht!=null)
					{
						return  ht[sType].ToString();
					}
					else
					{
						return "未知" + sType;
					}
				}
			}
			catch (Exception e) //没有从数据字典中读到memo
			{
				
				return "未知" + sType;
			} 
		}


		//获取数据字典
		public static void queryDic(string type)
		{
			try
			{
				string Msg="";
				/*string strSql = "type=" + type;	
				commRes com=new commRes();
				DataSet ds = CommQuery.GetDataSetFromICE(strSql,CommQuery.QUERY_DIC,out Msg);
				if (ds == null) //如果获取数据字典失败
				{
					throw new Exception(Msg);
				}*/
				DataTable dt=QueryDicInfoByType(type,out Msg);
				if (dt == null||dt.Rows.Count==0) //如果获取数据字典失败
				{
					throw new Exception(Msg);
				}

				Hashtable myht = new Hashtable();
				foreach(DataRow dr in dt.Rows)
				{
					myht.Add(dr["Fvalue"].ToString(),dr["Fmemo"].ToString());
				}
				System.Web.Caching.Cache objCache = System.Web.HttpRuntime.Cache;
				objCache.Insert(type, myht);
			}
			catch (Exception e)
			{
				throw new Exception("查询银行字典信息失败："+e.ToString());
			}

		
		}


		//通过类型查询返回字段表信息
		public static DataTable QueryDicInfoByType(string type,out string Msg)
		{
			Msg="";
			try
			{
				//先查询出总笔数
				string icesql = "type=" + type;
				string count = CommQuery.GetOneResultFromICE(icesql,CommQuery.QUERY_DIC_COUNT,"acount",out Msg);
				if(count==null||count==""||count=="0")
				{
					return null;
				}
				int allCount=Convert.ToInt32(count);
				if(allCount<=0)
				{
					return null;
				}

				
				DataTable dt_all=new DataTable();
				dt_all.Columns.Add("Fno",System.Type.GetType("System.String")); 
				dt_all.Columns.Add("FType",System.Type.GetType("System.String")); 
				dt_all.Columns.Add("Fvalue",System.Type.GetType("System.String")); 
				dt_all.Columns.Add("Fmemo",System.Type.GetType("System.String")); 
				
				string strSqlTmp = "type=" + type;	
				int limitStart=0;
				int onceCount=20;//一次返回笔数

				while(allCount>limitStart)
				{
					string strSqlLimit = "&strlimit=limit "+limitStart+","+onceCount;
					string strSql=strSqlTmp+strSqlLimit;
					DataTable dt_one = CommQuery.GetTableFromICE(strSql,CommQuery.QUERY_DIC,out Msg);
					if(dt_one!=null&&dt_one.Rows.Count>0)
					{
						foreach(DataRow dr2 in dt_one.Rows)
						{
							string fno=dr2["Fno"].ToString();
							string FType=dr2["FType"].ToString();
							string Fvalue=dr2["Fvalue"].ToString();
							string Fmemo=dr2["Fmemo"].ToString();

							DataRow drNew =dt_all.NewRow();
							drNew["Fno"]=fno;
							drNew["FType"]=FType;
							drNew["Fvalue"]=Fvalue;
							drNew["Fmemo"]=Fmemo;
							dt_all.Rows.Add(drNew);
						}
						
					}

					limitStart=limitStart+onceCount;
				}

			
				return dt_all;
			}
			catch(Exception ex)
			{
				log4net.ILog log = log4net.LogManager.GetLogger("查询字典表信息异常");
				if(log.IsErrorEnabled) log.Error(ex.ToString());
				return null;
			}
		}
	}
}
