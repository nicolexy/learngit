/************************************************************
  FileName: DES.cs
  Author: ����ҵ��֧�������       Version :    1.0.0      Date:2005-05-30
  Description: ����DES�ӽ��ܵ���    // ģ������      
  Version:   1.0.0      // �汾��Ϣ
  Function List:   // ��Ҫ�������书��
    1. Encrypt   �����ַ�����
    2. Decrypt   �����ַ�����
      <author>  <time>   <version >   <desc>
      Mouse   2005-05-30     1.0.0     build this moudle  
***********************************************************/
using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace TENCENT.OSS.CFT.KF.Common
{
	public enum enmQQType
	{
		UNKNOWN = 0,
		QQ = 1,		
		EMAIL = 2,
		MOBILE = 3,
	}


	/// <summary>
	/// ��DES���ܺͽ�����ص���(��ʱֻ֧��8λ����Կ)��
	/// </summary>
	public class DESCode
	{
		

		/// <summary>
		/// ��ȡ�ʺŵ����ͣ�0Ϊ�д�1ΪQQ�ţ�2Ϊ�ֻ���3ΪEMAIL
		/// </summary>
		/// <param name="strQQ">�ʺ�</param>
		/// <returns>�ʺ�����</returns>
		public static enmQQType GetQQType(string strQQ)
		{
			if(strQQ == null || strQQ.Trim() == "" || strQQ.Trim().Length >= 65)
			{
				return enmQQType.UNKNOWN;
			}

			int ilen = strQQ.Trim().Length;
			if( ilen > 13)
			{
				return enmQQType.EMAIL;
			}

			try
			{
				long itmp = long.Parse(strQQ.Trim());
				if(ilen == 13)
				{
					return enmQQType.MOBILE;
				}
				//else if(ilen >= 5 && ilen < 10)
				// 2012/5/29 �޸�12λ���µ��ʺŶ�����Ϊ��QQ����
				else if(ilen >= 5 && ilen < 13)
				{
					return enmQQType.QQ;
				}
				else
					return enmQQType.UNKNOWN;
			}
			catch
			{
				return enmQQType.EMAIL;
			}
		}

		public static bool GetUid3(string qqid, out string uid3)
		{
			uid3 = "0";
			if(qqid == null || qqid.Trim().Length < 3)
			{
				uid3 = "�ʺŲ���Ϊ��";
				return false;
			}

			//start
			string qq = qqid.Trim();
			try
			{
				long itmp = long.Parse(qq);
				uid3 = qq;
				return true;
			}
			catch
			{
				if(!Common.DESCode.GetEmailUid(qq,out uid3))
				{
					uid3 = "����EMAILʱ����" + uid3;
					return false;
				}
				return true;
			}
			//end	
		}

		public static bool GetEmailUid(string strEmail, out string uid3)
		{
			uid3 = "0";

			if(strEmail == null || strEmail.Trim().Length < 3)
			{
				return false;
			}

			//furion ��������email����Сдת��
			strEmail = strEmail.Trim().ToLower();

			byte[] inbuff = new byte[strEmail.Trim().Length];
			System.Text.Encoding.ASCII.GetBytes(strEmail.Trim(),0,strEmail.Trim().Length,inbuff,0);

			System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
			byte[] outbuff = md5.ComputeHash(inbuff);

			if(outbuff.Length < 3)
			{
				return false;
			}

			string result = "";
			string strasc = "";
			byte[] ascbuff = new byte[2];
			
			strasc = Convert.ToString(outbuff[outbuff.Length-2],16).ToLower().PadLeft(2,'0');
			System.Text.Encoding.ASCII.GetBytes(strasc,1,1,ascbuff,0);
			strasc = ascbuff[0].ToString();
			result = strasc.Substring(strasc.Length - 1,1);

			strasc = Convert.ToString(outbuff[outbuff.Length-1],16).ToLower().PadLeft(2,'0');
			System.Text.Encoding.ASCII.GetBytes(strasc,0,2,ascbuff,0);
			strasc = ascbuff[0].ToString();
			result += strasc.Substring(strasc.Length - 1,1);

			strasc = ascbuff[1].ToString();
			result += strasc.Substring(strasc.Length - 1,1);

			uid3 = result;
			return true;
		}

		/// <summary>
		/// ���캯��
		/// </summary>
		public DESCode()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}


		/// <summary>
		/// ��ָ������Կ��ָ�����ַ������ܡ�
		/// </summary>
		/// <param name="source">Դ�ַ���</param>
		/// <param name="key">��Կ(Ϊ8λ��ĸ��ɣ����ִ�Сд)</param>
		/// <returns>���ܺ��ַ���</returns>
		public static string Encrypt(string source,string key)
		{
			DESCryptoServiceProvider des = new DESCryptoServiceProvider();  

			//���ַ����ŵ�byte������  
			byte[] inputByteArray = Encoding.Default.GetBytes(source);  

			//�������ܶ������Կ��ƫ����  
			//ԭ��ʹ��ASCIIEncoding.ASCII������GetBytes����  
			//ʹ�����������������Ӣ���ı�  
			des.Key = ASCIIEncoding.ASCII.GetBytes(key);  
			des.IV  = ASCIIEncoding.ASCII.GetBytes(key);  
			MemoryStream ms = new MemoryStream();  
			CryptoStream cs = new CryptoStream(ms,des.CreateEncryptor(),CryptoStreamMode.Write);  

			cs.Write(inputByteArray,0,inputByteArray.Length);  
			cs.FlushFinalBlock();  

			StringBuilder ret = new StringBuilder();  
			foreach(byte b in ms.ToArray())  
			{  

				ret.AppendFormat("{0:X2}", b);  
			}  
			return ret.ToString();  
		}


		/// <summary>
		/// ��ָ������Կ��ָ�����ַ������ܡ�
		/// </summary>
		/// <param name="source">Դ�ַ���</param>
		/// <param name="key">��Կ(Ϊ8λ��ĸ��ɣ����ִ�Сд)</param>
		/// <returns>���ܺ��ַ���</returns>
		public static string Decrypt(string source,string key)
		{
			DESCryptoServiceProvider des = new DESCryptoServiceProvider();  

			byte[] inputByteArray = new byte[source.Length/2];  
			for (int x=0;x<source.Length/2;x++)  
			{  
				int i = (Convert.ToInt32(source.Substring(x*2,2),16));  
				inputByteArray[x] = (byte)i;  
			}  

			//�������ܶ������Կ��ƫ��������ֵ��Ҫ�������޸�  
			des.Key = ASCIIEncoding.ASCII.GetBytes(key);  
			des.IV  = ASCIIEncoding.ASCII.GetBytes(key);  
			MemoryStream ms = new MemoryStream();  
			CryptoStream cs = new CryptoStream(ms,des.CreateDecryptor(),CryptoStreamMode.Write);  
			cs.Write(inputByteArray,0,inputByteArray.Length);  
			cs.FlushFinalBlock();  

			//����StringBuild����CreateDecryptʹ�õ��������󣬱���ѽ��ܺ���ı����������  
			StringBuilder ret = new StringBuilder();  
             
			return System.Text.Encoding.Default.GetString(ms.ToArray());  
		}

	}
}
