using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TENCENT.OSS.CFT.KF.Common;
using System.Collections;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.IO;
using System.Threading;

namespace CFT.CSOMS.COMMLIB
{
    public class CommUtil
    {
       

        /// <summary>
        /// 保证不重复管理器
        /// 初始值为10，使用时，每次+1；当达到100时，循环使用。 跟在秒后使用。
        /// </summary>
        public static int StaticNo = 10; //初始值 当达到99后，则循环，从10开始
        public static bool StaticNoManageSign = true;

        public static string StaticNoManage()
        {
            //如果标志位为false,则等待
            try
            {
                while (!StaticNoManageSign)
                {
                    Thread.Sleep(50);
                }

                StaticNoManageSign = false;

                StaticNo++;

                if (StaticNo > 99)
                {
                    StaticNo = 10;  //清空为初始状态
                }
            }
            finally
            {
                StaticNoManageSign = true;
            }
            return StaticNo.ToString();
        }

        /// <summary>
        /// Dictionary按key排序，ASII排序方式
        /// </summary>
        /// <param name="dic">待排序的Dictionary</param>
        /// <returns></returns>
        public static Dictionary<string, string> DictionarySort(Dictionary<string, string> dic)
        {
            Dictionary<string, string> dicASC = new Dictionary<string, string>();

            ArrayList akeys = new ArrayList(dic.Keys);
            akeys.Sort();
            foreach (string k in akeys)
            {
                dicASC.Add(k.ToString(), dic[k].ToString());
            }
            return dicASC;
        }

        /// <summary>  

        /// GMT时间转成本地时间  

        /// </summary>  

        /// <param name="gmt">字符串形式的GMT时间</param>  

        /// <returns></returns>  

        public static DateTime GMT2Local(string gmt)
        {

            DateTime dt = DateTime.MinValue;

            try
            {

                string pattern = "";

                //if (gmt.IndexOf("+0") != -1)
                //{

                //    gmt = gmt.Replace("GMT", "");

                //    pattern = "ddd, dd MMM yyyy HH':'mm':'ss zzz";
                //}
                if (gmt.ToUpper().IndexOf("GMT") != -1)
                {
                    pattern = "MMM dd HH':'mm':'ss yyyy 'GMT'";
                }
                if (pattern != "")
                {
                    try
                    {
                        dt = DateTime.ParseExact(gmt, pattern, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal);
                    }
                    catch//处理那种不按格式来的，例如：Sep  2 06:53:28 2015 GMT
                    {
                        pattern = "MMM  d HH':'mm':'ss yyyy 'GMT'";
                        dt = DateTime.ParseExact(gmt, pattern, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal);
                    }
                    dt = dt.ToLocalTime();
                }
                else
                {
                    dt = Convert.ToDateTime(gmt);
                }
            }
            catch
            {
            }
            return dt;
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        public static string FenToYuan(double fen)
        {
            return MoneyTransfer.FenToYuan(fen) + "元";
        }

        public static string FenToYuan(string fen)
        {
            //rayguo 06.04.16 支持粗体的分元转换
            bool strong = false;
            if (fen.IndexOf("<B>") != -1 || fen.IndexOf("<b>") != -1)
            {
                strong = true;
                fen = fen.Replace("<B>", "").Replace("</B>", "").Replace("<b>", "").Replace("/b", "");
            }

            //furion 20051012 分元转换专用章
            string s = MoneyTransfer.FenToYuan(fen) + "元";

            if (strong == true)
            {
                s = "<font color =blue><B>" + s + "</B></font>";
            }

            return s;
        }

        /// <summary>
        /// 把一个数据表中的某个字段从分转换成元。furion 20050820
        /// </summary>
        /// <param name="dt">要转换的表</param>
        /// <param name="FieldName">要转换的字段</param>
        public static void FenToYuan_Table(DataTable dt, string FieldName, string destField)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string fen = dr[FieldName].ToString();
                    string yuan = FenToYuan(fen).Replace("元", "");
                    dr.BeginEdit();
                    dr[destField] = yuan;
                    dr.EndEdit();
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// 通过数据库字段内容转成页面显示内容。furion 20130517
        /// </summary>
        /// <param name="dt">要转换的表</param>
        /// <param name="FieldName">要转换的字段</param>
        public static void DbtypeToPageContent(DataTable dt, string FieldName, string destField, Hashtable ht)
        {
            try
            {
                if (ht == null)
                    return;

                if (dt.Columns.Contains(FieldName))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string field = dr[FieldName].ToString();
                        string ret = "";
                        if (ht.ContainsKey(field))
                        {
                            ret = ht[field].ToString();
                        }
                        else
                        {
                            ret = "未知：" + field;
                        }

                        dr.BeginEdit();
                        dr[destField] = ret;
                        dr.EndEdit();
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr[destField] = "";
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// 通过数据库字段内容转成页面显示内容。时间戳转为C#格式时间
        /// </summary>
        /// <param name="dt">要转换的表</param>
        /// <param name="FieldName">要转换的字段</param>
        public static void DbTimeToPageContent(DataTable dt, string FieldName, string destField)
        {
            try
            {
                if (dt.Columns.Contains(FieldName))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string field = dr[FieldName].ToString();
                        string ret = CommUtil.GetTime(field.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        if (ret.Trim() == "1970-01-01 08:00:00")
                            ret = "";
                        dr.BeginEdit();
                        dr[destField] = ret;
                        dr.EndEdit();
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr[destField] = "";
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public static string GetLCTTableId(string uin)
        {
            if (string.IsNullOrEmpty(uin))
            {
                return "";
            }
            uin = uin.Substring(0, uin.IndexOf("@wx.tenpay.com"));
            int iTableId = 0;

            char[] uinArr = uin.ToCharArray();

            int len = uinArr.Length;
            if (len < 2) { return ""; }
            iTableId = Math.Abs(CharHash(uinArr[len - 1]));
            iTableId += 10 * Math.Abs(CharHash(uinArr[len - 2]));

            return iTableId.ToString();
        }
        public static int CharHash(char c)
        {
            return (c - '0') % 10;
        }

        public static string URLDecode(string strField)
        {
            if (strField == null || strField == "")
                return "";
            else
            {
                string stmp = System.Web.HttpUtility.UrlDecode(strField, System.Text.Encoding.GetEncoding("gb2312"));
                return stmp.Replace("?26", "&");
            }
        }

        public static string URLEncode(string strField)
        {
            return URLEncode(strField, System.Text.Encoding.GetEncoding("gb2312"));
        }

        public static string URLEncode(string strField, Encoding code)
        {
            if (strField == null || strField == "")
                return "";
            else
                return System.Web.HttpUtility.UrlEncode(strField, code);
        }

        /// <summary>
        /// 解析字符串多条记录，格式如：result=0&res_info=ok&count=3&batch_id_0=1001&batch_id_1=1001&batch_id_2=1001。yinhuang 20140620
        /// </summary>
        /// <param name="str">待解析字符串</param>
        /// <param name="countStr">记录数名称，例如：count</param>
        /// <param name="rowStr">行名称，例如：batch_id_</param>
        public static DataSet ParseStrPageRow(string str, string countStr, string rowStr, out string errMsg)
        {
            DataSet dsresult = null;
            Hashtable ht = null;
            errMsg = "";

            if (str != null && str != "")
            {
                string[] strlist1 = str.Split('&'); //result=0&xx1=1&xx2=2

                if (strlist1.Length == 0)
                {
                    dsresult = null;
                    errMsg = "调用失败,返回结果有误" + str;
                    return null;
                }

                ht = new Hashtable(strlist1.Length);

                foreach (string strtmp in strlist1)
                {
                    string[] strlist2 = strtmp.Split('=');
                    if (strlist2.Length != 2)
                    {
                        continue;
                    }

                    ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
                }

                if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0")
                {
                    dsresult = null;
                    errMsg = "调用失败,返回结果有误" + str;
                    return null;
                }

                dsresult = new DataSet();
                DataTable dt = new DataTable();
                dsresult.Tables.Add(dt);

                int irowcount = Int32.Parse(ht[countStr].ToString().Trim());
                if (irowcount > 0)
                {
                    for (int i = 0; i < irowcount; i++)
                    {
                        string onerow = ht[rowStr + i].ToString().Trim();
                        onerow = URLDecode(onerow);
                        string[] strsplit_detail = onerow.Split('&');


                        DataRow drfield = dt.NewRow();
                        drfield.BeginEdit();

                        foreach (string stmp in strsplit_detail)
                        {
                            if (stmp == null || stmp.Trim() == "")
                                continue;

                            string[] fieldsplit = stmp.Split('=');

                            if (fieldsplit.Length != 2)
                                continue;

                            if (i == 0)
                            {
                                dt.Columns.Add(fieldsplit[0]);
                            }

                            drfield[fieldsplit[0]] = URLDecode(fieldsplit[1].Trim());
                        }

                        drfield.EndEdit();
                        dt.Rows.Add(drfield);
                    }
                }

            }

            return dsresult;
        }

        public static string ICEEncode(string instr)
        {
            if (instr == null || instr.Trim() == "")
                return instr;
            else
            {
                return instr.Replace("%", "%25").Replace("=", "%3d").Replace("&", "%26");
            }
        }

        public static string ObjToJson<T>(T t)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.Serialize(t);

            return js.Serialize(t);
        }

        /// <summary>
        /// 发送http post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="req">请求参数</param>
        /// <param name="isUtf">请求使用utf-8编码</param>
        /// <param name="resEncode">返回使用的编码格式</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string SendHttpPost(string url, string req, bool isReqUtf, bool isResUtf, out string msg, string contentType = "application/x-www-form-urlencoded")
        {
            SunLibrary.LoggerFactory.Get("CommUtil").Info("SendHttpPost:url:" + url+ " param:"+ req);
            msg = "";
            string answer = "";

            try
            {
                System.Text.Encoding encoding;
                if (isResUtf)
                {
                    encoding = System.Text.Encoding.GetEncoding("UTF-8");
                }
                else
                {
                    encoding = System.Text.Encoding.GetEncoding("GBK");
                }

                System.Net.HttpWebRequest webrequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                webrequest.ContentType = contentType;
                webrequest.Method = "POST";
                webrequest.Timeout = 600000;

                byte[] data;
                if (isReqUtf)
                {
                    data = Encoding.UTF8.GetBytes(req);
                }
                else
                {
                    data = Encoding.Default.GetBytes(req);
                }
                var parameter = webrequest.GetRequestStream();
                parameter.Write(data, 0, data.Length);

                parameter.Close();

                System.Net.HttpWebResponse webresponse = (System.Net.HttpWebResponse)webrequest.GetResponse();
                Stream stream = webresponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream, encoding);
                answer = streamReader.ReadToEnd();
                webresponse.Close();
                streamReader.Close();
                SunLibrary.LoggerFactory.Get("CommUtil").Info("SendHttpPost  Get:" + answer);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return "";
            }
            return answer;
        }

        /// <summary>
        /// 一点通银行卡解密方法
        /// </summary>
        /// <param name="base64Bankid"></param>
        /// <returns></returns>
        public static string BankIDEncode_ForBankCardUnbind(string base64Bankid)
        {
            byte[] iv = { 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18 };
            byte[] newkey = { 0x4a, 0x08, 0x80, 0x58, 0x13, 0xad, 0x46, 0x89 };

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

                return System.Text.Encoding.ASCII.GetString(ms.ToArray()).Trim();
            }
            catch (Exception ex)
            {
                return base64Bankid;
            }
        }

        /// <summary>
        /// 一点通银行卡加密算法
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string EncryptZerosPadding(string source)
        {
            if (source.Trim() == "")
                return "";

            try
            {

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                //把字符串放到byte数组中  
                byte[] inputByteArray = Encoding.Default.GetBytes(source);

                byte[] key = { 0x4a, 0x08, 0x80, 0x58, 0x13, 0xad, 0x46, 0x89 };

                byte[] iv = { 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18 };

                des.Key = key;
                des.IV = iv;
                des.Padding = System.Security.Cryptography.PaddingMode.Zeros;//0填充
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                string bas64Str = Convert.ToBase64String(ms.ToArray());
                return bas64Str.Replace("+", "-").Replace("/", "_");//.Replace("=","%3d").Replace("=","%3D");


            }
            catch (Exception ex)
            {
                return "";
            }
        }


        /// <summary>
        /// 3DES加密算法 通用
        /// </summary>
        /// <param name="trueSourceBytes">源串byte[]</param>
        /// <param name="keyBytes">密钥byte[]</param>
        /// <returns></returns>
        public static string TripleDESEncrypt(byte[] trueSourceBytes, byte[] keyBytes)
        {
            TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider()
            {
                Key = keyBytes,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.Zeros
            };

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, tripleDes.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(trueSourceBytes, 0, trueSourceBytes.Length);
            cs.FlushFinalBlock();

            byte[] str = ms.ToArray();
            return System.Text.Encoding.Default.GetString(str);

        }

        /// <summary>
        /// 3DES解密算法 通用
        /// </summary>
        /// <param name="trueSourceBytes">待解密串byte[]</param>
        /// <param name="keyBytes">解密密钥</param>
        /// <returns></returns>
        public static string TripleDESDecrypt(byte[] trueSourceBytes, byte[] keyBytes)
        {

            TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider()
            {
                //Key = hashMD5.ComputeHash(Encoding.ASCII.GetBytes("ba6f0d572c4c18e0ae9e4d915a56a1cd" + "299677456" + "testkef")),
                Key = keyBytes,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.Zeros
            };

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, tripleDes.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(trueSourceBytes, 0, trueSourceBytes.Length);
            cs.FlushFinalBlock();

            return Encoding.GetEncoding("gb2312").GetString(ms.ToArray()).Replace("\0", "");
        }

        /// <summary>
        /// 用户实名信息查询加密算法
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string TripleDESEncryptRealName(string source, string key)
        {
            byte[] trueSourceBytes = null;
            byte[] keyBytes = null;
            GetSourceAndKeyByteEncrypt(source, key, out trueSourceBytes, out keyBytes);
            string desResult = TripleDESEncrypt(trueSourceBytes, keyBytes);
            return AsciiToBcd2(desResult);
        }

        /// <summary>
        ///  用户实名信息查询解密算法
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string TripleDESDecryptRealName(string source, string key)
        {
            byte[] trueSourceBytes = null;
            byte[] keyBytes = null;
            GetSourceAndKeyByteDecrypt(source, key, out trueSourceBytes, out keyBytes);
            return TripleDESDecrypt(trueSourceBytes, keyBytes);
        }

        /// <summary>
        /// 实名信息补填接口解密算法 输入源字符串和key计算后，得到最后加密的源串和key
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="trueSourceBytes"></param>
        /// <param name="genkey"></param>
        public static void GetSourceAndKeyByteEncrypt(string source, string key, out byte[] trueSourceBytes, out byte[] truekey)
        {
            string md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").ToUpper();

            truekey = Encoding.GetEncoding("latin1").GetBytes(Bcd2ToAscii(md5));

            trueSourceBytes = Encoding.GetEncoding("gb2312").GetBytes(source);//防止汉字编码后不可解密
        }

        /// <summary>
        /// 实名信息查询接口解密算法 输入源字符串和key计算后，得到最后解密的源串和key
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="trueSourceBytes"></param>
        /// <param name="genkey"></param>
        public static void GetSourceAndKeyByteDecrypt(string source, string key, out byte[] trueSourceBytes, out byte[] truekey)
        {
            trueSourceBytes = Encoding.GetEncoding("latin1").GetBytes(Bcd2ToAscii(source));
            string md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").ToUpper();
            truekey = Encoding.GetEncoding("latin1").GetBytes(Bcd2ToAscii(md5));
        }


        public static char[] Bcd2ToAscii(string str)
        {
            char[] tbl = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

            int enLen = ((str.Length / 2) % 8) == 0 ? (str.Length / 2) : (((str.Length / 2) / 8 + 1) * 8);
            char[] ascii = new char[enLen];

            int i, j;

            for (i = 0; i < ((str.Length + 1) / 2); i++)
            {
                for (j = 0; j < 16; j++)
                    if (tbl[j] == str[2 * i])
                        break;

                ascii[i] = Convert.ToChar(j);
                ascii[i] = Convert.ToChar(Convert.ToInt32(ascii[i]) << 4);
                for (j = 0; j < 16; j++)
                    if (tbl[j] == str[2 * i + 1])
                        break;

                ascii[i] += Convert.ToChar(j);


            }

            return ascii;
        }

        public static string AsciiToBcd2(string source)
        {
            byte[] str = Encoding.Default.GetBytes(source);

            char[] tbl = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            // char[] tb2 = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ':', ';', '<', '=', '>', '?' };

            int enLen = ((str.Length / 2) % 8) == 0 ? (str.Length / 2) : (((str.Length / 2) / 8 + 1) * 8);


            char[] bcd = new char[enLen * 2];
            char t;
            for (int i = 0; i < enLen; i++)
            {
                t = Convert.ToChar(str[i] & (0xf0));
                t = Convert.ToChar(t >> 4);
                bcd[2 * i] = tbl[t];
                t = Convert.ToChar(str[i] & (0x0f));
                bcd[2 * i + 1] = tbl[t];
            }
            return new string(bcd);

        }

    }



}
