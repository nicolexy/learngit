using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Text;

namespace TENCENT.OSS.CFT.KF.Common
{
	/// <summary>
	/// ICE批量查询
	/// </summary>
	public class CommQuery
	{
		public static string ICEEncode(string instr)
		{
			if(instr == null || instr.Trim() == "")
				return instr;
			else
			{
				return instr.Replace("%","%25").Replace("=","%3d").Replace("&","%26");
			}
		}

		public static string URLEncode(string strField)
		{
			if(strField == null || strField == "")
				return "";
			else
				return 	System.Web.HttpUtility.UrlEncode(strField,System.Text.Encoding.GetEncoding("gb2312"));
		}

		public static string URLDecode(string strField)
		{
			if(strField == null || strField == "")
				return "";
			else
				return 	System.Web.HttpUtility.UrlDecode(strField,System.Text.Encoding.GetEncoding("gb2312"));
		}

		public CommQuery()
		{
		}


		public static string C帐户资金流水 = "QUERY_USER_BANKROLL";


		public static DataSet GetDataSetFromICE(string strWhere, string strCmd, out string errMsg)
		{
			DataSet dsresult = null;
			errMsg = "";

			string	sReply;
			short	iResult;
			string sMsg;

			string sInmsg = "CMD=" + strCmd + "&" + strWhere;

			string servicename =  "ex_common_query_service";

			if(commRes.middleInvoke(servicename, sInmsg, false, out sReply, out iResult, out sMsg))
			{
				if(iResult == 0)
				{
					//对sreply进行解析
					if(sReply == null || sReply.Trim() == "")
					{
						dsresult = null;
						errMsg = "调用通用查询失败,无返回结果" + servicename + sInmsg ;
						return null;
					}
					else
					{
						
						string[] strlist1 = sReply.Split('&');

						if(strlist1.Length == 0)
						{
							dsresult = null;
							errMsg = "调用通用查询失败,返回结果有误" + sReply ;
							return null;
						}

						Hashtable ht = new Hashtable(strlist1.Length);

						foreach(string strtmp in strlist1)
						{
							string[] strlist2 = strtmp.Split('=');
							if(strlist2.Length != 2)
							{
								dsresult = null;
								errMsg = "调用通用查询失败,返回结果有误" + sReply ;
								return null;
							}
                            
							ht.Add(strlist2[0].Trim(),strlist2[1].Trim());
						}

						if(!ht.Contains("result") || ht["result"].ToString().Trim() != "0" || !ht.Contains("row_num") )
						{
							dsresult = null;
							errMsg = "调用通用查询失败,返回结果有误" + sReply ;
							return null;
						}

						int irowcount = Int32.Parse(ht["row_num"].ToString().Trim());

						if(irowcount == 0)
						{
							dsresult = null;
							errMsg = "查询结果为空";
							return null;
						}

						dsresult = new DataSet();
						DataTable dt = new DataTable();
						dsresult.Tables.Add(dt);

						string firstrow = ht["row1"].ToString().Trim();

						firstrow = URLDecode(firstrow);

						string[] strsplit3 = firstrow.Split('&');

						if(strsplit3.Length == 0)
						{
							dsresult = null;
							errMsg = "查询结果中无法解析出字段";
							return null;
						}
						

						foreach(string stmp in strsplit3)
						{
							if(stmp == null || stmp.Trim() == "")
								continue;

							string[] fieldsplit = stmp.Split('=');
						
							if(fieldsplit.Length != 2)
								continue;

							dt.Columns.Add(fieldsplit[0]);
						}

						for(int i=1; i<=irowcount ;i++ )
						{
							string onerow = ht["row" + i].ToString().Trim();
							onerow = URLDecode(onerow);
							string[] strsplit_detail = onerow.Split('&');

							DataRow drfield = dt.NewRow();
							drfield.BeginEdit();							

							foreach(string stmp in strsplit_detail)
							{
								if(stmp == null || stmp.Trim() == "")
									continue;

								string[] fieldsplit = stmp.Split('=');
						
								if(fieldsplit.Length != 2)
									continue;

								drfield[fieldsplit[0]] = fieldsplit[1].Trim();
							}

							drfield.EndEdit();
							dt.Rows.Add(drfield);

						}

					}
					return dsresult;
				}
				else
				{
					dsresult = null;
					errMsg = "调用通用查询失败iresult=" + iResult + "|err="  + sMsg;
					return null;
				}
			}
			else
			{
				dsresult = null;
				errMsg = "调用通用查询失败了:" + sMsg;
				return null;
			}

		}

		public static bool GetDataFromICE(string strWhere, string strCmd, out string errMsg, out DataSet dsresult)
		{
			dsresult = GetDataSetFromICE(strWhere,strCmd,out errMsg);		

			return true;
		}
	}
}
