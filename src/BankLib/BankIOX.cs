using System;
using System.Configuration;
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace BankLib
{
	/// <summary>
	/// BankIOX ��ժҪ˵����
	/// </summary>
	public class BankIOX
	{
		public BankIOX()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		public static string fxykconn = "";

		/// <summary>
		/// ��ѯ�����п��Ŷ�Ӧ���ܿ�
		/// </summary>
		/// <param name="xbankaccno"></param>
		/// <param name="xykconn"></param>
		/// <returns></returns>
		public static string GetCreditEncode(string xbankaccno,string xykconn)
		{
			if(xbankaccno == null || xbankaccno.Trim() == "")
				return "";

			if(xbankaccno.Trim().Length < 9)
				return "��Ч" + xbankaccno;

			MySqlAccess da = new MySqlAccess(xykconn);
			try
			{
				xbankaccno = xbankaccno.Trim();
				string md5value = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(xbankaccno,"md5").ToLower();

				da.OpenConn();
				string strSql = "select seqno from c2c_db.t_xyk_secret where cardno_md5='" + md5value + "'";
				string seqno = da.GetOneResult(strSql);

				//�������Ĵ��ڡ�
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


        //���֧��-���п���ѯ-���п�����
        public static string Encrypt(string source)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //���ַ����ŵ�byte������  
            byte[] inputByteArray = Encoding.Default.GetBytes(source);

            //�������ܶ������Կ��ƫ����  
            //ԭ��ʹ��ASCIIEncoding.ASCII������GetBytes����  
            //ʹ�����������������Ӣ���ı�  

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

        //���֧��-���п���ѯ-3des����
        /// <summary>
        /// ��ָ������Կ��ָ�����ַ�������(���ģʽ�������)��
        /// </summary>
        /// <param name="base64Source">base64Դ�ַ���</param>
        /// <param name="key">��Կ(Ϊ8λ��ĸ��ɣ����ִ�Сд)</param>
        /// <param name="iv">ƫ����(Ϊ8λ��ĸ��ɣ����ִ�Сд)</param>
        /// <returns>���ܺ��ַ���</returns>
        public static string DecryptNoPadding(string base64Source)
        {
            try
            {
                //���Ĺ̶�ʹ�������Կ������
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
        /// ��ָ������Կ��ָ�����ַ�������(���ģʽ�������)��
        /// </summary>
        /// <param name="base64Source">base64Դ�ַ���</param>
        /// <param name="key">��Կ(Ϊ8λ��ĸ��ɣ����ִ�Сд)</param>
        /// <param name="iv">ƫ����(Ϊ8λ��ĸ��ɣ����ִ�Сд)</param>
        /// <returns>���ܺ��ַ���</returns>
        public static string DecryptNoPadding(string base64Source, byte[] key, byte[] iv)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                //middle��base64ת������׼������ط���Ҫ�滻�� 
                byte[] inputByteArray = Convert.FromBase64String(base64Source.Replace("-", "+").Replace("_", "/"));

                //�������ܶ������Կ��ƫ��������ֵ��Ҫ�������޸� 

                des.Key = key;
                des.IV = iv;
                //Padding����ΪNone���������Ҫ����Ϊmiddle��������ܵ�
                des.Padding = System.Security.Cryptography.PaddingMode.None;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                //return System.Text.Encoding.ASCII.GetString(ms.ToArray()).Trim(); 

                return System.Text.Encoding.GetEncoding("gb2312").GetString(ms.ToArray()).Trim().TrimEnd('\0'); //֧�ֺ��ֽ��� andrew
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
                throw new Exception("�������Կ��");
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

        public string Decryptor(string Source, string Key = "PWMXbxkk98N62W1lxnixJtMy")//��ʽkey:PWMXbxkk98N62W1lxnixJtMy
        {
            if (Key.Length != 24)
            {
                throw new Exception("�������Կ��");
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
