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
		
	}
}