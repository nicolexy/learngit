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
        #region 从数据表中转换出来值（同时进行NULL处理）
        public static string GetInt(object aValue)
        {
            try
            {
                if (aValue != null)
                {
                    long num = long.Parse(aValue.ToString());
                    return num.ToString();
                }
                else
                {
                    return "0";
                }
            }
            catch
            {
                return "0";
            }
        }

        public static string GetString(object aValue)
        {
            try
            {
                if (aValue != null)
                {
                    return aValue.ToString().Replace("\\", "\\\\").Replace("'", "\\'");
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }


        public static string GetDateTime(object aValue)
        {
            try
            {
                if (aValue != null)
                {
                    return (DateTime.Parse(aValue.ToString())).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }
        #endregion

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

        /// <summary>
        /// 把字符串解析成字典 解析格式 key1=0,key2=0,key3=0,key4=0,key5=0 类型的字符串
        /// </summary>
        /// <param name="str">要解析的字符串</param>
        /// <param name="separator">分割字符串的依据</param>
        /// <param name="keySeparator">分割键值对的依据</param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictionary(this String str, char separator = '&', char keySeparator = '=')
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            var arr = str.Split(separator);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in arr)
            {
                var value = item.Split(keySeparator);
                switch (value.Length)
                {
                    case 0: break;
                    case 1: dic.Add(value[0], ""); break;
                    case 2: dic.Add(value[0], value[1]); break;
                    default:
                        {
                            dic.Add(value[0], item.Substring(value[0].Length + 1)); //有多个 "keySeparator" 就把第一个 "keySeparator" 后面的当成是值
                        } break;
                }
            }
            return dic;
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
