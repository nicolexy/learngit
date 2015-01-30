using System;
using System.Configuration;
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Data;
using System.Collections;
using System.Text;
using System.IO;
using System.Security.Cryptography; 
using System.Web;
using System.Threading;



namespace TENCENT.OSS.CFT.KF.KF_Web.classLibrary
{

	/// <summary>
	/// PermitParaData 数据类
	/// </summary>
	public class PermitParaData
	{
		public string FKey       ;   //参数键值         
		public string FCode      ;   //参数码           
		public string FSubCode   ;   //子参数码         
		public string FName      ;   //参数名称         
		public string FDouble    ;   //参数double型值   
		public string FInt       ;   //参数int型值      
		public string FSmallInt  ;   //参数smallint型值 
		public string FDatetime  ;   //参数datetime型值 
		public string FString    ;   //参数string型值   
		public string FSpecial0  ;   //special0         
		public string FSpecial1  ;   //special1         
		public string FSpecial2  ;   //special2         
		public string FSpecial3  ;   //special3         
		public string FSpecial4  ;   //special4         
		public string FModifyTime;   //修改时间
		public string FmodifyUser;   //最后修改人
        
		public string FOldFsubCode;  //用于修改时使用，非数据字段；
	}

	/// <summary>
	/// PermitPara 的摘要说明。
	/// </summary>
	public class PermitPara
	{
		public PermitPara()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		//public static DataSet QueryPermitPara(string PermitParaDataKey,int iStart,int iPageSize,MySqlAccess da,out string Msg)
//		public static DataSet QueryPermitPara(PermitParaData ppd,int iStart,int iPageSize,MySqlAccess da,out string Msg)
//		{
//			Msg = "";
//			try
//			{
//				string PermitParaDataKey = ppd.FKey;
//				string whereStr = "";
//				if (PermitParaDataKey.ToUpper() != "ALL")
//				{
//					whereStr = " WHERE Fkey = '" + PermitParaDataKey + "'";
//
//					if (ppd.FCode != null && ppd.FCode.Trim() != "" && ppd.FSubCode != null && ppd.FSubCode.Trim() != "")
//					{
//						whereStr += " and Fcode = '" + ppd.FCode + "' and FsubCode ='" + ppd.FSubCode + "' ";
//					}
// 
//				}
//				
//				//查询总记录数
//				string countStr =  "select count(*) from c2c_db_au.t_parameter " + whereStr;
//				string iCount      = da.GetOneResult(countStr);
//
//				string queryStr = "select *, " + iCount + " as iCount from c2c_db_au.t_parameter " + whereStr + " limit " + iStart + "," + iPageSize;
//				PublicRes.WriteFile(queryStr);
//				PublicRes.CloseFile();
//
//				DataTable dt = da.GetTable(queryStr);
//
//				//添加自定义显示项
//				dt.Columns.Add("FeffectTitle1",typeof(string));
//				dt.Columns.Add("FeffectValue1",typeof(string));
//				dt.Columns.Add("FeffectTitle2",typeof(string));
//				dt.Columns.Add("FeffectValue2",typeof(string));
//				dt.Columns.Add("FeffectTotol", typeof(string));
//
//
//				foreach(DataRow dr in dt.Rows)
//				{
//					int iEffect = 0;
//
//					string keyid = dr["Fkey"].ToString();
//					string str   = "select * from c2c_db_au.t_parameter_title where fkey ='" + keyid + "'";
//					DataRow tr = da.GetTable(str).Rows[0];
//					
//					#region 自定义数据项数据绑定
//					if (tr["FEffectDouble"].ToString().Trim() == "1")
//					{
//						iEffect ++;
//
//						if (iEffect == 1)
//						{
//							dr["FeffectTitle1"] = tr["FTitleDouble"];
//							dr["FeffectValue1"] = dr["FDouble"];	
//						}
//						else if (iEffect == 2)
//						{
//							dr["FeffectTitle2"] = tr["FTitleDouble"];
//							dr["FeffectValue2"] = dr["FDouble"];	
//						}
//					}
//
//					if (tr["FEffectInt"].ToString().Trim() == "1")
//					{
//						iEffect ++;
//
//						if (iEffect == 1)
//						{
//							dr["FeffectTitle1"] = tr["FTitleInt"];
//							dr["FeffectValue1"] = dr["FInt"];	
//						}
//						else if (iEffect == 2)
//						{
//							dr["FeffectTitle2"] = tr["FTitleInt"];
//							dr["FeffectValue2"] = dr["FInt"];	
//						}
//					}
//
//					if (tr["FEffectSmallInt"].ToString().Trim() == "1")
//					{
//						iEffect ++;
//
//						if (iEffect == 1)
//						{
//							dr["FeffectTitle1"] = tr["FTitleSmallInt"];
//							dr["FeffectValue1"] = dr["FSmallInt"];	
//						}
//						else if (iEffect == 2)
//						{
//							dr["FeffectTitle2"] = tr["FTitleSmallInt"];
//							dr["FeffectValue2"] = dr["FSmallInt"];	
//						}
//					}
//
//					if (tr["FEffectDatetime"].ToString().Trim() == "1")
//					{
//						iEffect ++;
//
//						if (iEffect == 1)
//						{
//							dr["FeffectTitle1"] = tr["FTitleDatetime"];
//							dr["FeffectValue1"] = dr["FDatetime"];	
//						}
//						else if (iEffect == 2)
//						{
//							dr["FeffectTitle2"] = tr["FTitleDatetime"];
//							dr["FeffectValue2"] = dr["FDatetime"];	
//						}
//					}
//
//					if (tr["FEffectString"].ToString().Trim() == "1")
//					{
//						iEffect ++;
//
//						if (iEffect == 1)
//						{
//							dr["FeffectTitle1"] = tr["FTitleString"];
//							dr["FeffectValue1"] = dr["FString"];	
//						}
//						else if (iEffect == 2)
//						{
//							dr["FeffectTitle2"] = tr["FTitleString"];
//							dr["FeffectValue2"] = dr["FString"];	
//						}
//					}
//
//					if (tr["FInEffectSpecial0"].ToString().Trim() == "1")
//					{
//						iEffect ++;
//
//						if (iEffect == 1)
//						{
//							dr["FeffectTitle1"] = tr["FTitleSpecial0"];
//							dr["FeffectValue1"] = dr["FSpecial0"];	
//						}
//						else if (iEffect == 2)
//						{
//							dr["FeffectTitle2"] = tr["FTitleSpecial0"];
//							dr["FeffectValue2"] = dr["FSpecial0"];	
//						}
//					}
//
//					if (tr["FInEffectSpecial1"].ToString().Trim() == "1")
//					{
//						iEffect ++;
//
//						if (iEffect == 1)
//						{
//							dr["FeffectTitle1"] = tr["FTitleSpecial1"];
//							dr["FeffectValue1"] = dr["FSpecial1"];	
//						}
//						else if (iEffect == 2)
//						{
//							dr["FeffectTitle2"] = tr["FTitleSpecial1"];
//							dr["FeffectValue2"] = dr["FSpecial1"];	
//						}
//					}
//
//					if (tr["FInEffectSpecial2"].ToString().Trim() == "1")
//					{
//						iEffect ++;
//
//						if (iEffect == 1)
//						{
//							dr["FeffectTitle1"] = tr["FTitleSpecial2"];
//							dr["FeffectValue1"] = dr["FSpecial2"];	
//						}
//						else if (iEffect == 2)
//						{
//							dr["FeffectTitle2"] = tr["FTitleSpecial2"];
//							dr["FeffectValue2"] = dr["FSpecial2"];	
//						}
//					}
//
//					if (tr["FInEffectSpecial3"].ToString().Trim() == "1")
//					{
//						iEffect ++;
//
//						if (iEffect == 1)
//						{
//							dr["FeffectTitle1"] = tr["FTitleSpecial3"];
//							dr["FeffectValue1"] = dr["FSpecial3"];	
//						}
//						else if (iEffect == 2)
//						{
//							dr["FeffectTitle2"] = tr["FTitleSpecial3"];
//							dr["FeffectValue2"] = dr["FSpecial3"];	
//						}
//					}
//
//					if (tr["FInEffectSpecial4"].ToString().Trim() == "1")
//					{
//						iEffect ++;
//
//						if (iEffect == 1)
//						{
//							dr["FeffectTitle1"] = tr["FTitleSpecial4"];
//							dr["FeffectValue1"] = dr["FSpecial4"];	
//						}
//						else if (iEffect == 2)
//						{
//							dr["FeffectTitle2"] = tr["FTitleSpecial4"];
//							dr["FeffectValue2"] = dr["FSpecial4"];	
//						}
//					}
//
//					#endregion
//
//					dr["FeffectTotol"] = iEffect;
//				}
//
//				DataSet ds = new DataSet();
//				ds.Tables.Add(dt);
//
//				return ds;
//			}
//			catch(Exception e)
//			{
////				Msg = "查询QueryPermitPara异常：" + replaceMStr(e.Message);
////				sendLog4Log("classLibray.PermitPara.QueryPermitPara",Msg);
//				return null;
//			}
//		}


		/// <summary>
		/// 去除敏感字符
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
//		public static string replaceMStr(string str)  //对插入数据库的字符串的敏感字符进行替换,放置非法sql注入访问
//		{
//			if(str == null) return null; //furion 20050819
//
//			str = str.Replace("'","’");  
//			str = str.Replace("\"","”");
//			str = str.Replace("script","ｓｃｒｉｐｔ");
//			str = str.Replace("<","〈");
//			str = str.Replace(">","〉");
//			//furion 上面的是防止HTML的吧?
//			str = str.Replace("--","——");
//			str = str.Replace("#","＃");
//			str = str.Replace("@","＠");
//			str = str.Replace("&","＆");
//			return str;
//		}

//		public static DataSet QueryPermitPara(PermitParaData ppd,int iStart,int iPageSize,out string Msg)
//		{
//			Msg = "";
//			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("au"));
//
//			try
//			{
//				da.OpenConn();
//				return QueryPermitPara(ppd,iStart,iPageSize,da,out Msg);
//			}
//			catch(Exception e)
//			{
//				Msg = e.Message;
//				return null;
//			}
//			finally
//			{
//				da.Dispose();
//			}
//		}
		
		/// <summary>
		/// PermitPara Insert with DbConnection (带数据库连接)
		/// </summary>
		/// <param name="ppd"></param>
		/// <param name="da"></param>
		/// <param name="Msg"></param>
		/// <returns></returns>
//		public static bool AddPermitPara(PermitParaData ppd,MySqlAccess da,out string Msg)
//		{
//			Msg = "";
//			try
//			{
//				string systemTime = PublicRes.strNowTimeStander;
//				ppd.FModifyTime  = systemTime;
//
//				string insertStr = "Insert c2c_db_au.t_parameter (FKey ,FCode ,FSubCode ,FName ,FDouble ,FInt ,FSmallInt ,FDatetime ,FString ,FSpecial0 ,FSpecial1 ,FSpecial2 ,FSpecial3 ,FSpecial4 ,FmodifyUser,FModifyTime ) Values (" + "'" + ppd.FKey  + "',"  + "'" + ppd.FCode  + "',"  + "'" + ppd.FSubCode  + "',"  + "'" + ppd.FName  + "',"  + "'" + ppd.FDouble  + "',"  + "'" + ppd.FInt  + "',"  + "'" + ppd.FSmallInt  + "',"  + "'" + ppd.FDatetime  + "',"  + "'" + ppd.FString  + "',"  + "'" + ppd.FSpecial0  + "',"  + "'" + ppd.FSpecial1  + "',"  + "'" + ppd.FSpecial2  + "',"  + "'" + ppd.FSpecial3  + "',"  + "'" + ppd.FSpecial4  + "'," + "'" + ppd.FmodifyUser  + "',"  + "'" + ppd.FModifyTime  + "' )";
//				PublicRes.WriteFile(insertStr);
//				PublicRes.CloseFile();
//				da.ExecSql(insertStr);
//				return true;
//			}
//			catch(Exception e)
//			{
//				da.RollBack();
//				if (e.Message.IndexOf("Duplicate entry") != -1)
//					Msg = "插入参数键值-参数码-子参数值为联合主键，不能重复!请检查。";
//				else
//					Msg = "添加数据异常！" + commRes.replaceMStr(e.Message);
//
//				commRes.sendLog4Log("classLibray.PermitPara.AddPermitPara",Msg);
//				return false;
//			}
//		}
//		
		/// <summary>
		/// PermitPara Insert without DbConnection (不带数据库连接)
		/// </summary>
		/// <param name="ppd"></param>
		/// <param name="Msg"></param>
		/// <returns></returns>
//		public static bool AddPermitPara(PermitParaData ppd,out string Msg)
//		{
//			Msg = "";
//			MySqlAccess da	= new MySqlAccess(PublicRes.GetConnString("au"));
//
//			try
//			{
//				da.OpenConn();
//				return AddPermitPara(ppd,da,out Msg);
//			}
//			catch(Exception e)
//			{
//				Msg = commRes.replaceMStr(e.Message);
//				return false;
//			}
//			finally
//			{
//				da.Dispose();
//			}
//		}

		/// <summary>
		/// 编辑PermitPara数据 带数据库连接；
		/// </summary>
		/// <param name="ppd"></param>
		/// <param name="da"></param>
		/// <param name="Msg"></param>
		/// <returns></returns>
//		public static bool EditPermitPara(PermitParaData ppd,MySqlAccess da, out string Msg)
//		{
//			Msg = "";
//			try
//			{
//				ppd.FModifyTime = PublicRes.strNowTimeStander;
//
//				string updateStr = "UPDATE c2c_db_au.t_parameter SET " + " FKey  = '" + ppd.FKey   + "', FCode  = '" + ppd.FCode   + "', FSubCode  = '" + ppd.FSubCode   
//					+ "', FName  = '" + ppd.FName   + "', FDouble  = '" + ppd.FDouble   + "', FInt  = '" + ppd.FInt   + "', FSmallInt  = '" + ppd.FSmallInt   
//					+ "', FDatetime  = '" + ppd.FDatetime   + "', FString  = '" + ppd.FString   + "', FSpecial0  = '" + ppd.FSpecial0   + "', FSpecial1  = '" 
//					+ ppd.FSpecial1   + "', FSpecial2  = '" + ppd.FSpecial2   + "', FSpecial3  = '" + ppd.FSpecial3   + "', FSpecial4  = '" + ppd.FSpecial4 + "', FmodifyUser  = '" + ppd.FmodifyUser     
//					+ "', FModifyTime  = '" + ppd.FModifyTime   + "'" 
//					+ "  WHERE Fkey='" + ppd.FKey + "' and Fcode ='" + ppd.FCode + "' and fsubcode ='" + ppd.FOldFsubCode + "'";
//				PublicRes.WriteFile(updateStr);
//				PublicRes.CloseFile();
//				
//				int iEfectRow = da.ExecSqlNum(updateStr);
//
//				if (iEfectRow != 1)
//				{
//					da.RollBack();
//					throw new Exception("更新记录数不正确[" + iEfectRow + "].");
//				}
//
//				return true;
//			}
//			catch(Exception e)
//			{
//				da.RollBack();
//				Msg = "更新EditPerMitPara异常:" + commRes.replaceMStr(e.Message);
//				commRes.sendLog4Log("classLibray.PermitPara.EditPermitPara",Msg);
//				return false;
//			}
//		}

		/// <summary>
		/// 编辑PermitPara数据，不带数据库连接；
		/// </summary>
		/// <param name="ppd"></param>
		/// <param name="Msg"></param>
		/// <returns></returns>
//		public static bool EditPermitPara(PermitParaData ppd,out string Msg)
//		{
//			Msg = "";
//			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("au"));
//
//			try
//			{
//				da.OpenConn();
//				return EditPermitPara(ppd,da,out Msg);
//			}
//			catch(Exception e)
//			{
//				Msg = e.Message;
//				return false;
//			}
//			finally
//			{
//				da.Dispose();
//			}
//		}

		/// <summary>
		/// 删除PermitPara数据，带数据库连接
		/// </summary>
		/// <param name="PermitParaKeyID"></param>
		/// <param name="da"></param>
		/// <param name="Msg"></param>
		/// <returns></returns>
//		public static bool DelPermitPara(PermitParaData ppd,MySqlAccess da,out string Msg)
//		{
//			Msg = "";
//			
//			try
//			{
//				if (ppd.FKey == null || ppd.FKey.Trim() == "" || ppd.FCode == null || ppd.FCode.Trim() == "" || ppd.FSubCode == null || ppd.FSubCode.Trim() == "")
//				{
//					Msg = "传入的参数不完整！请检查！";
//					return false;
//				}
//
//				string delStr = "delete from c2c_db_au.t_parameter where fkey='" + ppd.FKey + "' and fcode = '" + ppd.FCode + "' and fsubcode = '" + ppd.FSubCode + "'";
//				PublicRes.WriteFile(delStr);
//				PublicRes.CloseFile();
//
//				int iEfectRow = da.ExecSqlNum(delStr);
//				if (iEfectRow != 1)
//				{
//					da.RollBack();
//					throw new Exception("删除记录数不正确![" + iEfectRow + "]");
//				}
//
//				return true;
//			}
//			catch(Exception e)
//			{
//				da.RollBack();
//				Msg = "删除DelPermitPara异常:" + commRes.replaceMStr(e.Message);
//				commRes.sendLog4Log("classLibray.PermitPara.DelPermitPara",Msg);
//				return false;
//			}
//		}

		/// <summary>
		/// 删除PermitPara,不带数据库连接
		/// </summary>
		/// <param name="PermitParaKeyID"></param>
		/// <param name="Msg"></param>
		/// <returns></returns>
//		public static bool DelPermitPara(PermitParaData ppd,out string Msg)
//		{
//			Msg = "";
//			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("au"));
//			
//			try
//			{
//				da.OpenConn();
//				return DelPermitPara(ppd,da,out Msg);
//			}
//			catch(Exception e)
//			{
//				Msg = e.Message;
//				return false;
//			}
//			finally
//			{
//				da.Dispose();
//			}
//		}

		/// <summary>
		/// 返回帐户属性类型(数据字典)
		/// </summary>
		/// <returns></returns>
		public static DataSet QueryDicAccoutnPro()
		{
			string queryString = "SELECT Fkey as Value,FName as Text FROM c2c_db_au.t_parameter_title order by cast(fkey as unsigned) ";
			return PublicRes.returnDSAll(queryString,"au");
		}

		///返回数据字典-"帐户属性名称"
		public static DataSet QueryDicAccName()
		{
			string queryStr = "select FSubCode as Value,fstring as Text from c2c_db_au.t_parameter where FName = '帐户属性名称' order by cast(FSubCode as unsigned)";
			return PublicRes.returnDSAll(queryStr,"au");
		}


		//		//返回所有的ParaTitle类型
		//		public static DataSet PPTDataQueryType()
		//		{
		//			string queryString = "SELECT Fkey as Value,FName as Text FROM c2c_db_au.t_parameter_title order by cast(fkey as unsigned) ";
		//			return PublicRes.returnDSAll(queryString,"au");
		//		}

	}
}