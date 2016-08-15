using System;
using System.Configuration;
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace BankLib
{
	/// <summary>
	/// BankIOX 的摘要说明。
	/// </summary>
	public class BankIOX
	{
		public BankIOX()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		public static string fxykconn = "";

		/// <summary>
		/// 查询出银行卡号对应的密卡
		/// </summary>
		/// <param name="xbankaccno"></param>
		/// <param name="xykconn"></param>
		/// <returns></returns>
		public static string GetCreditEncode(string xbankaccno,string xykconn)
		{
			if(xbankaccno == null || xbankaccno.Trim() == "")
				return "";

			if(xbankaccno.Trim().Length < 9)
				return "无效" + xbankaccno;

			MySqlAccess da = new MySqlAccess(xykconn);
			try
			{
				xbankaccno = xbankaccno.Trim();
				string md5value = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(xbankaccno,"md5").ToLower();

				da.OpenConn();
				string strSql = "select seqno from c2c_db.t_xyk_secret where cardno_md5='" + md5value + "'";
				string seqno = da.GetOneResult(strSql);

				//允许明文存在。
				if(seqno == null || seqno == "")
					return  xbankaccno;

				int ilen = xbankaccno.Length;
				return "X" + xbankaccno.Substring(0,4) + seqno.PadLeft(12,'0') + xbankaccno.Substring(ilen - 4, 4);
			}
			finally
			{
				da.Dispose();
			}
		}



		
		static BankIOX()
		{
			if(ConfigurationManager.AppSettings["DB_XYK"] == null || ConfigurationManager.AppSettings["DB_XYK"].Trim() == "")
			{
				fxykconn = "";
			}
			else
			{
				string f_strDatabase_xyk = ConfigurationManager.AppSettings["DB_XYK"];
				string f_strDataSource_xyk = ConfigurationManager.AppSettings["DataSource_XYK"];
				string f_strUserID_xyk = ConfigurationManager.AppSettings["UserID_XYK"];
				string f_strPassword_xyk = ConfigurationManager.AppSettings["Password_XYK"];

                string db50list = ConfigurationManager.AppSettings["DB50List"];
                if (db50list.IndexOf(";XYK;") > -1)
                {
                    fxykconn = "Driver={mysql ODBC 5.2a Driver}; Database=" + f_strDatabase_xyk + ";Server=" + f_strDataSource_xyk + ";UID=" +
                   f_strUserID_xyk + ";PWD=" + f_strPassword_xyk + ";charset=latin1;Option=3";
                }
                else
                {
                    fxykconn = "Driver={MySQL ODBC 3.51 Driver}; Database=" + f_strDatabase_xyk + ";Server=" + f_strDataSource_xyk + ";UID=" +
                    f_strUserID_xyk + ";PWD=" + f_strPassword_xyk + ";Option=3";
                }

               
			}
		}


        //快捷支付-银行卡查询-银行卡加密
        public static string Encrypt(string source)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //把字符串放到byte数组中  
            byte[] inputByteArray = Encoding.Default.GetBytes(source);

            //建立加密对象的密钥和偏移量  
            //原文使用ASCIIEncoding.ASCII方法的GetBytes方法  
            //使得输入密码必须输入英文文本  

            byte[] newkey = { 0x4a, 0x08, 0x80, 0x58, 0x13, 0xad, 0x46, 0x89 };
            //		
            byte[] iv = { 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18 };
            //
            des.Key = newkey;
            des.IV = iv;
            des.Padding = System.Security.Cryptography.PaddingMode.Zeros;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();


            string bas64Str = Convert.ToBase64String(ms.ToArray());

            return bas64Str.Replace("+", "-").Replace("/", "_");

        }

        //快捷支付-银行卡查询-3des解密
        /// <summary>
        /// 用指定的密钥给指定的字符串解密(填充模式，不填充)。
        /// </summary>
        /// <param name="base64Source">base64源字符串</param>
        /// <param name="key">密钥(为8位字母组成，区分大小写)</param>
        /// <param name="iv">偏移量(为8位字母组成，区分大小写)</param>
        /// <returns>解密后字符串</returns>
        public static string DecryptNoPadding(string base64Source)
        {
            try
            {
                //核心固定使用这个密钥和向量
                byte[] key = { 0x4a, 0x08, 0x80, 0x58, 0x13, 0xad, 0x46, 0x89 };
                byte[] iv = { 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18 };
                return DecryptNoPadding(base64Source, key, iv);
            }
            catch (Exception ex)
            {
                string stmp = ex.Message;
                return "";
            }
        }

        /// <summary>
        /// 用指定的密钥给指定的字符串解密(填充模式，不填充)。
        /// </summary>
        /// <param name="base64Source">base64源字符串</param>
        /// <param name="key">密钥(为8位字母组成，区分大小写)</param>
        /// <param name="iv">偏移量(为8位字母组成，区分大小写)</param>
        /// <returns>解密后字符串</returns>
        public static string DecryptNoPadding(string base64Source, byte[] key, byte[] iv)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                //middle的base64转换不标准，这个地方需要替换下 
                byte[] inputByteArray = Convert.FromBase64String(base64Source.Replace("-", "+").Replace("_", "/"));

                //建立加密对象的密钥和偏移量，此值重要，不能修改 

                des.Key = key;
                des.IV = iv;
                //Padding设置为None，这个很重要，因为middle是这个加密的
                des.Padding = System.Security.Cryptography.PaddingMode.None;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                //return System.Text.Encoding.ASCII.GetString(ms.ToArray()).Trim(); 

                return System.Text.Encoding.GetEncoding("gb2312").GetString(ms.ToArray()).Trim().TrimEnd('\0'); //支持汉字解密 andrew
            }
            catch (Exception ex)
            {
                string stmp = ex.Message;
                return "";
            }
        }

        #region

        public string Encryptor(string Source, string Key)
        {
            if (Key.Length != 24)
            {
                throw new Exception("错误的秘钥！");
            }

            byte[] truekey = System.Text.Encoding.UTF8.GetBytes(Key);// Convert.FromBase64String(Key);
            byte[] trueSource = System.Text.Encoding.UTF8.GetBytes(Source);

            TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider()
            {

                Key = truekey,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.Zeros
            };

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, tripleDes.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(trueSource, 0, trueSource.Length);
            cs.FlushFinalBlock();

            byte[] str = ms.ToArray();

            return Convert.ToBase64String(str);
        }

        public string Decryptor(string Source, string Key = "PWMXbxkk98N62W1lxnixJtMy")//正式key:PWMXbxkk98N62W1lxnixJtMy
        {
            if (Key.Length != 24)
            {
                throw new Exception("错误的秘钥！");
            }

            byte[] truekey = System.Text.Encoding.UTF8.GetBytes(Key);
            byte[] trueSource = Convert.FromBase64String(Source);

            TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider()
            {
                Key = truekey,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.Zeros
            };

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, tripleDes.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(trueSource, 0, trueSource.Length);
            cs.FlushFinalBlock();

            byte[] str = ms.ToArray();

            return System.Text.Encoding.UTF8.GetString(str).TrimEnd('\0');
        }

        #endregion
	
    
    }
}
