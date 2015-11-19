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
		
	}
}