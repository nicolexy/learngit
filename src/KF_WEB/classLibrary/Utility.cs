using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Text;
using System.Data;
using System.IO;

namespace TENCENT.OSS.CFT.KF.KF_Web.classLibrary
{
    public class Utility
    {
        public static string GetDataCellValue(object dataCell)
        {
            if (dataCell == null)
                return string.Empty;

            return dataCell.ToString();

        }

        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message + ex.StackTrace);
            }
        }
    }

    public static class IEnumerableEx
    {
        /// <summary>
        /// 将数组转换成字符串，用指定的间隔字符间隔
        /// </summary>
        /// <param name="collections"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToString(this IEnumerable collections, string separator)
        {
            var newValueBuilder = new StringBuilder();
            foreach (var item in collections)
            {
                newValueBuilder.Append(item.ToString());
                newValueBuilder.Append(separator);
            }
            return newValueBuilder.Remove(newValueBuilder.Length - separator.Length, separator.Length)
                .ToString();
        }
    }
}