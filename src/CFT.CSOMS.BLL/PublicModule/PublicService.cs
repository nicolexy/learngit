using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace CFT.CSOMS.BLL.PublicService
{
    using CFT.CSOMS.DAL.Infrastructure;
    using System.Collections;
    using System.Web;
    using TENCENT.OSS.C2C.Finance.Common.CommLib;
    using System.Security.Cryptography;
    using System.IO;
    public class PublicService
    {
        /// <summary>
        /// 姓名生僻字  姓名解密方法
        /// 与一点通解密算法一样，秘钥不一样
        /// </summary>
        /// <param name="base64Bankid"></param>
        /// <returns></returns>
        public static string NameEncode_ForRareName(string base64Bankid)
        {
            byte[] iv = { 0x2e, 0x3a, 0x35, 0x67, 0x45, 0x6a, 0x7f, 0x0a };
            byte[] newkey = { 0x3a, 0x2a, (byte)(-0x28 & 0xFF), 0x59, 0x43, (byte)(-0x23 & 0xFF), 0x16, (byte)(-0x65 & 0xFF) };

            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                //middle的base64转换不标准，这个地方需要替换下 
                byte[] inputByteArray = Convert.FromBase64String(base64Bankid.Replace("-", "+").Replace("_", "/").Replace("%3d", "=").Replace("%3D", "="));

                //建立加密对象的密钥和偏移量，此值重要，不能修改 

                des.Key = newkey;
                des.IV = iv;
                //Padding设置为None，这个很重要，因为middle是这个加密的
                des.Padding = System.Security.Cryptography.PaddingMode.None;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                return System.Text.Encoding.GetEncoding("gb2312").GetString(ms.ToArray()).Trim();
            }
            catch (Exception ex)
            {
                return base64Bankid;
            }
        }

        ///外币分转化成元
        public string FenToYuan(string strfen, string currency_type)
        {
            return MoneyTransfer.FenToYuan(strfen, currency_type);
        }

        public static string objectToString(DataTable dt, string col_name)
        {
            return objectToString(dt, 0, col_name);
        }

        public static string objectToString(DataTable dt, int row_id, string col_name)
        {
            string ret = "";
            try
            {
                if (col_name == null || col_name == "")
                {
                    return "";
                }
                if (dt.Columns.Contains(col_name))
                {
                    return dt.Rows[row_id][col_name].ToString();
                }
            }
            catch (Exception ex)
            {
                ret = "";
            }

            return ret;
        }
         
        public DataTable GetCheckInfo(string objid, string checkType)
        {
            string msg = "";
            DataTable dt= new PublicRes().GetCheckInfo(objid, checkType, out msg);
            if (msg != "")
                throw new Exception(msg);
            return dt;
        }
    }
}
