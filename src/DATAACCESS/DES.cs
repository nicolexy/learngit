/************************************************************
  FileName: DES.cs
  Author: 借用业务支持组的类       Version :    1.0.0      Date:2005-05-30
  Description: 进行DES加解密的类    // 模块描述      
  Version:   1.0.0      // 版本信息
  Function List:   // 主要函数及其功能
    1. Encrypt   加密字符串。
    2. Decrypt   解密字符串。
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
	/// 与DES加密和解密相关的类(暂时只支持8位的密钥)。
	/// </summary>
	public class DESCode
	{
		

		/// <summary>
		/// 获取帐号的类型：0为有错，1为QQ号，2为手机，3为EMAIL
		/// </summary>
		/// <param name="strQQ">帐号</param>
		/// <returns>帐号类型</returns>
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
				// 2012/5/29 修改12位以下的帐号都先认为是QQ号码
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
				uid3 = "帐号不能为空";
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
					uid3 = "解析EMAIL时出错。" + uid3;
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

			//furion 这里必须把email进行小写转化
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
		/// 构造函数
		/// </summary>
		public DESCode()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}


		/// <summary>
		/// 用指定的密钥给指定的字符串加密。
		/// </summary>
		/// <param name="source">源字符串</param>
		/// <param name="key">密钥(为8位字母组成，区分大小写)</param>
		/// <returns>加密后字符串</returns>
		public static string Encrypt(string source,string key)
		{
			DESCryptoServiceProvider des = new DESCryptoServiceProvider();  

			//把字符串放到byte数组中  
			byte[] inputByteArray = Encoding.Default.GetBytes(source);  

			//建立加密对象的密钥和偏移量  
			//原文使用ASCIIEncoding.ASCII方法的GetBytes方法  
			//使得输入密码必须输入英文文本  
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
		/// 用指定的密钥给指定的字符串解密。
		/// </summary>
		/// <param name="source">源字符串</param>
		/// <param name="key">密钥(为8位字母组成，区分大小写)</param>
		/// <returns>解密后字符串</returns>
		public static string Decrypt(string source,string key)
		{
			DESCryptoServiceProvider des = new DESCryptoServiceProvider();  

			byte[] inputByteArray = new byte[source.Length/2];  
			for (int x=0;x<source.Length/2;x++)  
			{  
				int i = (Convert.ToInt32(source.Substring(x*2,2),16));  
				inputByteArray[x] = (byte)i;  
			}  

			//建立加密对象的密钥和偏移量，此值重要，不能修改  
			des.Key = ASCIIEncoding.ASCII.GetBytes(key);  
			des.IV  = ASCIIEncoding.ASCII.GetBytes(key);  
			MemoryStream ms = new MemoryStream();  
			CryptoStream cs = new CryptoStream(ms,des.CreateDecryptor(),CryptoStreamMode.Write);  
			cs.Write(inputByteArray,0,inputByteArray.Length);  
			cs.FlushFinalBlock();  

			//建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象  
			StringBuilder ret = new StringBuilder();  
             
			return System.Text.Encoding.Default.GetString(ms.ToArray());  
		}

	}
}
