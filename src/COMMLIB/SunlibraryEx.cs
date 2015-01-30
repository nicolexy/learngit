using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunLibraryEX
{
    /// <summary>
    /// 字符串通用扩展功能类
    /// </summary>
    public static class StringEx
    {
        /// <summary>
        /// 判断传入字符串是否全为数字
        /// </summary>
        /// <param name="s">待判断字符串</param>
        /// <returns></returns>
        public static bool IsNumber(string s)
        {
            try
            {
                System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(@"^\d+$");
                return rex.IsMatch(s);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsString(string s)
        {
            try
            {
                System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(@"^[a-z]+$");
                return rex.IsMatch(s.ToLower());
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 判断字符串长度是否为指定长度
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool MatchLength(string s, int length)
        {
            if (s.Length == length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool MatchLength(string s, int minLength, int maxLength)
        {
            if (s.Length >= minLength && s.Length <= maxLength)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class DateTimeEx
    {
        public static DateTime ConvertFromYYYYMMDD(string dateString)
        {
            return new DateTime(int.Parse(dateString.Substring(0, 4)), int.Parse(dateString.Substring(4, 2)), int.Parse(dateString.Substring(6, 2)));
        }


        public static string ConvertToYYYYMMDD(DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMdd");
        }
    }
}
