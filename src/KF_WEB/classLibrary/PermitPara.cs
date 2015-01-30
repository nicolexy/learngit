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
	/// PermitParaData ������
	/// </summary>
	public class PermitParaData
	{
		public string FKey       ;   //������ֵ         
		public string FCode      ;   //������           
		public string FSubCode   ;   //�Ӳ�����         
		public string FName      ;   //��������         
		public string FDouble    ;   //����double��ֵ   
		public string FInt       ;   //����int��ֵ      
		public string FSmallInt  ;   //����smallint��ֵ 
		public string FDatetime  ;   //����datetime��ֵ 
		public string FString    ;   //����string��ֵ   
		public string FSpecial0  ;   //special0         
		public string FSpecial1  ;   //special1         
		public string FSpecial2  ;   //special2         
		public string FSpecial3  ;   //special3         
		public string FSpecial4  ;   //special4         
		public string FModifyTime;   //�޸�ʱ��
		public string FmodifyUser;   //����޸���
        
		public string FOldFsubCode;  //�����޸�ʱʹ�ã��������ֶΣ�
	}

	/// <summary>
	/// PermitPara ��ժҪ˵����
	/// </summary>
	public class PermitPara
	{
		public PermitPara()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
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
//				//��ѯ�ܼ�¼��
//				string countStr =  "select count(*) from c2c_db_au.t_parameter " + whereStr;
//				string iCount      = da.GetOneResult(countStr);
//
//				string queryStr = "select *, " + iCount + " as iCount from c2c_db_au.t_parameter " + whereStr + " limit " + iStart + "," + iPageSize;
//				PublicRes.WriteFile(queryStr);
//				PublicRes.CloseFile();
//
//				DataTable dt = da.GetTable(queryStr);
//
//				//����Զ�����ʾ��
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
//					#region �Զ������������ݰ�
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
////				Msg = "��ѯQueryPermitPara�쳣��" + replaceMStr(e.Message);
////				sendLog4Log("classLibray.PermitPara.QueryPermitPara",Msg);
//				return null;
//			}
//		}


		/// <summary>
		/// ȥ�������ַ�
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
//		public static string replaceMStr(string str)  //�Բ������ݿ���ַ����������ַ������滻,���÷Ƿ�sqlע�����
//		{
//			if(str == null) return null; //furion 20050819
//
//			str = str.Replace("'","��");  
//			str = str.Replace("\"","��");
//			str = str.Replace("script","�������");
//			str = str.Replace("<","��");
//			str = str.Replace(">","��");
//			//furion ������Ƿ�ֹHTML�İ�?
//			str = str.Replace("--","����");
//			str = str.Replace("#","��");
//			str = str.Replace("@","��");
//			str = str.Replace("&","��");
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
		/// PermitPara Insert with DbConnection (�����ݿ�����)
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
//					Msg = "���������ֵ-������-�Ӳ���ֵΪ���������������ظ�!���顣";
//				else
//					Msg = "��������쳣��" + commRes.replaceMStr(e.Message);
//
//				commRes.sendLog4Log("classLibray.PermitPara.AddPermitPara",Msg);
//				return false;
//			}
//		}
//		
		/// <summary>
		/// PermitPara Insert without DbConnection (�������ݿ�����)
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
		/// �༭PermitPara���� �����ݿ����ӣ�
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
//					throw new Exception("���¼�¼������ȷ[" + iEfectRow + "].");
//				}
//
//				return true;
//			}
//			catch(Exception e)
//			{
//				da.RollBack();
//				Msg = "����EditPerMitPara�쳣:" + commRes.replaceMStr(e.Message);
//				commRes.sendLog4Log("classLibray.PermitPara.EditPermitPara",Msg);
//				return false;
//			}
//		}

		/// <summary>
		/// �༭PermitPara���ݣ��������ݿ����ӣ�
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
		/// ɾ��PermitPara���ݣ������ݿ�����
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
//					Msg = "����Ĳ��������������飡";
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
//					throw new Exception("ɾ����¼������ȷ![" + iEfectRow + "]");
//				}
//
//				return true;
//			}
//			catch(Exception e)
//			{
//				da.RollBack();
//				Msg = "ɾ��DelPermitPara�쳣:" + commRes.replaceMStr(e.Message);
//				commRes.sendLog4Log("classLibray.PermitPara.DelPermitPara",Msg);
//				return false;
//			}
//		}

		/// <summary>
		/// ɾ��PermitPara,�������ݿ�����
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
		/// �����ʻ���������(�����ֵ�)
		/// </summary>
		/// <returns></returns>
		public static DataSet QueryDicAccoutnPro()
		{
			string queryString = "SELECT Fkey as Value,FName as Text FROM c2c_db_au.t_parameter_title order by cast(fkey as unsigned) ";
			return PublicRes.returnDSAll(queryString,"au");
		}

		///���������ֵ�-"�ʻ���������"
		public static DataSet QueryDicAccName()
		{
			string queryStr = "select FSubCode as Value,fstring as Text from c2c_db_au.t_parameter where FName = '�ʻ���������' order by cast(FSubCode as unsigned)";
			return PublicRes.returnDSAll(queryStr,"au");
		}


		//		//�������е�ParaTitle����
		//		public static DataSet PPTDataQueryType()
		//		{
		//			string queryString = "SELECT Fkey as Value,FName as Text FROM c2c_db_au.t_parameter_title order by cast(fkey as unsigned) ";
		//			return PublicRes.returnDSAll(queryString,"au");
		//		}

	}
}