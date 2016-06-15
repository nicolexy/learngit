using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Collections;
using CFT.CSOMS.Service.CSAPI.Language;
using System.Collections.Specialized;
using System.Threading;

namespace CFT.CSOMS.Service.CSAPI.Utility
{
    public class APIUtil
    {
        public static string SUCC = "20000";
        public static string MSG_OK = "ok";
        public static string ERR_NORECORD = "40400";
        public static string ERR_GENERAL = "50000";
        public static string ERR_PARAM = "50100";
        public static string ERR_TOKEN = "50101";
        public static string ERR_APPKEY = "50102";
        public static string ERR_DATE = "50103";
        public static string ERR_PARAM_LOST = "50104";
        public static string ERR_SYSTEM = "40000";

        public static bool ValidateParams(params string[] str)
        {
            if (str == null || str.Length == 0)
            {
                throw new ServiceException(ERR_PARAM, ErroMessage.MESSAGE_NULLPARAM);
            }

            foreach (string s in str)
            {
                if (string.IsNullOrEmpty(s.Trim()))
                {
                    throw new ServiceException(ERR_PARAM, ErroMessage.MESSAGE_NULLPARAM);
                }
                string[] arr = s.Split('|');
                if (arr.Length == 2)
                {
                    if (string.IsNullOrEmpty(arr[1]))
                    {
                        throw new ServiceException(ERR_PARAM, arr[0] + ErroMessage.MESSAGE_NULLPARAM);
                    }
                }
                else
                {
                    throw new ServiceException(ERR_PARAM, "参数格式错误：" + s);
                }
            }

            return true;
        }

        public static bool ValidateParamsNew(Dictionary<string, string> paramsHt, params string[] str)
        {
            PrintLogRequest();//打印请求日志

            if (str == null || str.Length == 0)
            {
                throw new ServiceException(ERR_PARAM, ErroMessage.MESSAGE_NULLPARAM);
            }

            //  Hashtable nvc = GetQueryStrings();
            if (paramsHt == null || paramsHt.Count == 0)
            {
                throw new ServiceException(ERR_PARAM, ErroMessage.MESSAGE_NULLPARAM);
            }

            //  string[] arr = paramsHt.Keys.Cast<string>().ToArray();

            foreach (string s in str)
            {
                if (string.IsNullOrEmpty(s.Trim()))
                {
                    throw new ServiceException(ERR_PARAM, ErroMessage.MESSAGE_NULLPARAM);
                }
                if (paramsHt.Keys.Contains(s))
                {
                    string v = paramsHt[s].ToString();
                    if (string.IsNullOrEmpty(v.Trim()))
                    {
                        throw new ServiceException(ERR_PARAM, s + ErroMessage.MESSAGE_NULLPARAM);
                    }
                }
                else
                {
                    throw new ServiceException(ERR_PARAM_LOST, s + "参数缺失！");
                }
            }

            return true;
        }

        public static bool ValidateToken(string token, params string[] str)
        {
            if (str == null || str.Length == 0)
            {
                throw new ServiceException(ERR_PARAM, ErroMessage.MESSAGE_NULLPARAM);
            }
            string appid = str[0];
            //取appkey
            string appKey = "";
            try
            {
                appKey = System.Configuration.ConfigurationManager.AppSettings[appid].ToString();
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("APIUtil.ValidateToken").Info("get appKey error:" + ex.Message);
                throw new ServiceException(ERR_APPKEY, "appkey失效！");
            }

            StringBuilder req = new StringBuilder();
            foreach (string s in str)
            {
                req.Append(s);
            }
            req.Append(appKey);

            string md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(req.ToString(), "md5").ToLower();

            if (md5 == token)
            {
                return true;
            }
            else
            {
                SunLibrary.LoggerFactory.Get("APIUtil.ValidateToken").Info("right md5:" + md5);
                throw new ServiceException(ERR_TOKEN, "token验证失败");
            }
        }

        public static bool ValidateToken(Dictionary<string, string> paramsHt)
        {
            //   Hashtable nvc = GetQueryStrings();

            if (paramsHt == null || paramsHt.Count == 0)
            {
                throw new ServiceException(ERR_PARAM, ErroMessage.MESSAGE_NULLPARAM);
            }

            //   string[] arr = paramsHt.Keys.Cast<string>().ToArray();

            if (!paramsHt.Keys.Contains("appid"))
            {
                throw new ServiceException(ERR_PARAM_LOST, "appid参数缺失！");
            }
            if (!paramsHt.Keys.Contains("token"))
            {
                throw new ServiceException(ERR_PARAM_LOST, "token参数缺失！");
            }

            string appid = paramsHt["appid"].ToString();
            //取appkey
            string appKey = "";
            try
            {
                appKey = System.Configuration.ConfigurationManager.AppSettings[appid].ToString();
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("APIUtil.ValidateToken").Info("get appKey error:" + ex.Message);
                throw new ServiceException(ERR_APPKEY, "appkey失效！");
            }

            StringBuilder req = new StringBuilder();

            foreach (string s in paramsHt.Keys)
            {
                if (s == "token")
                {
                    continue;
                }
                req.Append(paramsHt[s]);
            }

            req.Append(appKey);

            string md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(req.ToString(), "md5").ToLower();

            string token = paramsHt["token"].ToString();

            if (md5 == token)
            {
                return true;
            }
            else
            {
                SunLibrary.LoggerFactory.Get("APIUtil.ValidateToken").Info("right md5:" + md5);
                throw new ServiceException(ERR_TOKEN, "token验证失败");
            }
        }

        public static bool ValidateSign(Dictionary<string, string> paramsHt, string formatStr, string key)
        {
            //   Hashtable nvc = GetQueryStrings();

            if (paramsHt == null || paramsHt.Count == 0)
            {
                throw new ServiceException(ERR_PARAM, ErroMessage.MESSAGE_NULLPARAM);
            }

            if (!paramsHt.Keys.Contains("sign"))
            {
                throw new ServiceException(ERR_PARAM_LOST, "sign参数缺失！");
            }

            string[] formatArr = formatStr.Split('|');
            StringBuilder reqFomat = new StringBuilder();
            foreach (string a in formatArr)
            {
                if (paramsHt.Keys.Contains(a))
                {
                    reqFomat.Append(paramsHt[a]);
                }
                else
                {
                    if (a == "key")
                    {
                        reqFomat.Append(key);
                    }
                    else
                    {
                        reqFomat.Append("");
                    }
                }
                reqFomat.Append("|");
            }
            if (reqFomat.ToString().EndsWith("|"))
            {
                reqFomat = new StringBuilder(reqFomat.ToString().Substring(0,reqFomat.ToString().Length - 1));
            }

            string md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(reqFomat.ToString(), "md5").ToLower();

            string token = paramsHt["sign"].ToString();

            if (md5 == token)
            {
                return true;
            }
            else
            {
                SunLibrary.LoggerFactory.Get("APIUtil.ValidateSign").Info("right md5:" + md5);
                throw new ServiceException(ERR_TOKEN, "sign验证失败");
            }
        }




        public static bool ValidateDate(string strDate, string strFormat, bool DateCanNull)
        {
            if (DateCanNull)
            {
                if (string.IsNullOrEmpty(strDate))
                    return true;
            }
            else
            {
                if (string.IsNullOrEmpty(strDate.Trim()))
                {
                    throw new ServiceException(ERR_DATE, ErroMessage.MESSAGE_ERROFROMDATE);
                }
            }

            if (string.IsNullOrEmpty(strFormat))
            {
                throw new ServiceException(ERR_GENERAL, "日期模板格式不能为空！");
            }

            try
            {
                DateTime date = DateTime.ParseExact(strDate, strFormat, System.Globalization.CultureInfo.CurrentCulture);

                return true;
            }
            catch
            {
                throw new ServiceException(ERR_DATE, ErroMessage.MESSAGE_ERROFROMDATE);
            }
        }

        public static DateTime StrToDate(string strDate)
        {
            if (string.IsNullOrEmpty(strDate))
            {
                throw new ServiceException(ERR_DATE, ErroMessage.MESSAGE_ERROFROMDATE);
            }

            try
            {
                DateTime date = DateTime.Parse(strDate);

                return date;
            }
            catch
            {
                throw new ServiceException(ERR_DATE, ErroMessage.MESSAGE_ERROFROMDATE);
            }
        }

        /// <summary>
        /// yyyyMMdd 格式
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static DateTime StrToDateTime(string strDate)
        {
            if (string.IsNullOrEmpty(strDate))
            {
                throw new ServiceException(ERR_DATE, ErroMessage.MESSAGE_ERROFROMDATE);
            }

            try
            {
                DateTime date = DateTime.ParseExact(strDate, "yyyyMMdd", Thread.CurrentThread.CurrentCulture);

                return date;
            }
            catch
            {
                throw new ServiceException(ERR_DATE, ErroMessage.MESSAGE_ERROFROMDATE);
            }
        }

        public static RetObject<Record> ToSingleObj(string value, string code, string msg)
        {
            var subReq = new RetObject<Record>();
            subReq.ReturnCode = code;
            subReq.Message = msg;

            if (!string.IsNullOrEmpty(value))
            {
                Record record = new Record();
                record.RetValue = value;

                List<Record> list = new List<Record>();
                list.Add(record);

                subReq.Records = list;
            }

            return subReq;
        }

        public static RetObject<Record> ToSingleObj(string code, string msg)
        {
            return ToSingleObj("", code, msg);
        }

        public static RetObject<Record> ToSingleObj(string value)
        {
            return ToSingleObj(value, SUCC, MSG_OK);
        }

        public static RetObject<T> ToErrorObj<T>(T t, string code, string msg)
        {
            var subReq = new RetObject<T>();
            subReq.ReturnCode = code;
            subReq.Message = msg;

            return subReq;
        }
        public static RetObject<T> ToErrorObj<T>(T t)
        {
            return ToErrorObj(t, SUCC, MSG_OK);
        }

        public static List<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        public static List<T> ConvertTo<T>(List<DataRow> rows)
        {
            List<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        public static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                    try
                    {
                        object value = row[column.ColumnName];
                        prop.SetValue(obj, value.ToString(), null);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return obj;
        }

        public static XmlDocument ConverToXml<T>(T t)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlSerializer xsSubmit = new XmlSerializer(t.GetType());
            MemoryStream stream = new MemoryStream();
            XmlWriterSettings setting = new XmlWriterSettings();
            setting.Encoding = new UTF8Encoding(false);
            //   setting.Encoding = Encoding.GetEncoding("gb2312");
            setting.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(stream, setting))
            {
                xsSubmit.Serialize(writer, t, ns);
            }

            XmlDocument xmldoc = new XmlDocument();

            //    xmldoc.LoadXml(Encoding.GetEncoding("gb2312").GetString(stream.ToArray()));
            xmldoc.LoadXml(Encoding.UTF8.GetString(stream.ToArray()));

            return xmldoc;
        }

        public static XmlDocument ToErrorXml(string code, string msg)
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlDeclaration xmlDel = xmldoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmldoc.AppendChild(xmlDel);

            XmlElement root = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(root);

            XmlElement retEle = xmldoc.CreateElement("", "return_code", "");
            XmlElement msgEle = xmldoc.CreateElement("", "msg", "");

            XmlText xmltext = xmldoc.CreateTextNode(code);
            retEle.AppendChild(xmltext);
            xmltext = xmldoc.CreateTextNode(msg);
            msgEle.AppendChild(xmltext);

            root.AppendChild(retEle);
            root.AppendChild(msgEle);

            return xmldoc;
        }

        public static string[] getValidateParamsArr(string paramstr)
        {
            string[] paramArr = paramstr.Split('&');
            List<string> validateParlist = new List<string>();
            foreach (string str in paramArr)
            {
                string[] pa = str.Split('=');
                if (pa[0] != "token")
                    validateParlist.Add(pa[1]);
            }
            string[] ValidateParamsArr = validateParlist.ToArray();
            return ValidateParamsArr;
        }

        public static string[] getValidateParamsArr()
        {
            string s = getReqParamStr();
            if (s == "")
            {
                return null;
            }
            return getValidateParamsArr(s);
        }

        public static string getReqParamStr()
        {
            HttpRequest request = HttpContext.Current.Request;
            string paramstr = request.RawUrl.ToString();//获取当前请求的URL
            paramstr = HttpUtility.UrlDecode(paramstr);
            //  SunLibrary.LoggerFactory.Get("APIUtil.getReqParamStr").Info("request params:" + paramstr);
            if (paramstr.IndexOf("?") == -1)
            {
                return "";
            }
            paramstr = paramstr.Substring(paramstr.IndexOf("?") + 1, paramstr.Length - paramstr.IndexOf("?") - 1);
            return paramstr;
        }

        public static void PrintLogRequest()
        {
            HttpRequest request = HttpContext.Current.Request;
            string paramstr = request.RawUrl.ToString();//获取当前请求的URL
            SunLibrary.LoggerFactory.Get("APIUtil.getReqParamStr").Info("RawUrl:" + paramstr);
            paramstr = HttpUtility.UrlDecode(paramstr);
            SunLibrary.LoggerFactory.Get("APIUtil.getReqParamStr").Info("RawUrl UrlDecode:" + paramstr);
        }

        public static void Print<T>(List<T> list)
        {
            //Hashtable map = getReqParamMap();
            Dictionary<string, string> nvc = GetQueryStrings();
            string[] map = nvc.Keys.Cast<string>().ToArray();
            string cb = nvc.ContainsKey("cb") ? nvc["cb"].ToString() : string.Empty;
            if (map != null && map.Contains("f"))
            {
                string s = nvc["f"].ToString();
                if (string.IsNullOrEmpty(s))
                {
                    PrintXML<T>(list, cb);
                }
                else if (s.ToLower() == "xml")
                {
                    PrintXML<T>(list, cb);
                }
                else if (s.ToLower() == "json")
                {
                    PrintJSON<T>(list, cb);
                }
                else
                {
                    PrintXML<T>(list, cb);
                }
            }
            else
            {
                PrintXML<T>(list, cb);
            }
        }

        public static void PrintError(string code, string msg)
        {
            string paramstr = APIUtil.getReqParamStr();
            if (paramstr.ToLower().IndexOf("?f=json") > 0
                || paramstr.ToLower().IndexOf("&f=json&") > 0
                || paramstr.ToLower().IndexOf("&f=json") == paramstr.Length - 7)//是json请求
                PrintJSON(code, msg);
            else
                PrintXML(code, msg);

        }

        public static void PrintJSON(string code, string msg)
        {
            HttpResponse res = HttpContext.Current.Response;
            res.ContentType = "text/plain";
            res.Charset = "gb2312";
            res.Write("{");
            res.Write("'return_code':" + code);
            res.Write(",'msg':" + msg);
            res.Write("}");
            res.End();
        }

        public static void PrintJSON<T>(List<T> list,string strJsonCallBack ="")
        {
            var ret = new ResultParse<T>().ReturnToObject(list);
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = js.Serialize(ret);

            HttpResponse res = HttpContext.Current.Response;
            res.ContentType = "text/plain";
            res.Charset = "utf-8";
            res.ContentEncoding = Encoding.UTF8;

            if (!string.IsNullOrEmpty(strJsonCallBack))
            {
                res.Write(strJsonCallBack+"("+json+")");
            }
            else
            {
                res.Write(json);
            }
            //res.End();
        }

        public static void PrintXML(string code, string msg)
        {
            HttpResponse res = HttpContext.Current.Response;
            res.ContentType = "text/xml";
            res.Charset = "utf-8";
            res.ContentEncoding = Encoding.UTF8;

            //xmldocument
            System.Xml.XmlDocument doc = ToErrorXml(code, msg);
            doc.Save(res.OutputStream);
            res.End();
        }

        public static void PrintXML<T>(List<T> list, string strJsonCallBack = "")
        {
            var ret = new ResultParse<T>().ReturnToObject(list);

            HttpResponse res = HttpContext.Current.Response;
            res.Charset = "utf-8";
            res.ContentEncoding = Encoding.UTF8;
            //xmldocument
            System.Xml.XmlDocument doc = ConverToXml<RetObject<T>>(ret);
            //doc.Save(res.OutputStream);

            if (!string.IsNullOrEmpty(strJsonCallBack))
            {
                res.Write(strJsonCallBack + "(" + doc.OuterXml + ")");
            }
            else
            {
                res.ContentType = "text/xml";
                res.Write(doc.OuterXml);
            }

            //res.End();
        }

        public static void Print4DataTable(
            DataTable table, List<String> excludedColNames, Dictionary<String, String> colNameMaps)
        {
            Dictionary<string, string> nvc = GetQueryStrings();
            string[] map = nvc.Keys.Cast<string>().ToArray();
            if (map != null && map.Contains("f")
                && !string.IsNullOrEmpty(nvc["f"].ToString())
                && nvc["f"].ToString().ToLower() == "json")
            {
                PrintJson4DataTable(table, excludedColNames, colNameMaps);
            }
            else
            {
                PrintXML4DataTable(table, excludedColNames, colNameMaps);
            }
        }

        /// <summary>
        /// 将DataTable转换成为XML格式
        /// </summary>
        /// <param name="table"></param>
        /// <param name="excludedColNames">不需要显示的列</param>
        /// <param name="colNameMaps"> 列名需要重命名的列</param>
        /// <returns></returns>
        public static void PrintXML4DataTable(
            DataTable table, List<String> excludedColNames, Dictionary<String, String> colNameMaps)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<root>");
            sb.Append(String.Format("<return_code>{0}</return_code>", APIUtil.SUCC));
            sb.Append(String.Format("<msg>{0}</msg>", APIUtil.MSG_OK));
            sb.Append(String.Format("<count>{0}</count>", table.Rows.Count));
            sb.Append("<records>");
            foreach (DataRow dr in table.Rows)
            {
                sb.Append("<record>");
                foreach (DataColumn col in table.Columns)
                {
                    //不需要显示的列处理
                    if (excludedColNames != null && excludedColNames.Count > 0 &&
                        excludedColNames.Contains(col.ColumnName, StringComparer.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    //列明需要重命名的列
                    String colName = col.ColumnName;
                    if (colNameMaps != null && colNameMaps.Count > 0)
                    {
                        if (colNameMaps.ContainsKey(colName))
                        {
                            if (!String.IsNullOrEmpty(colNameMaps[colName]))
                            {
                                colName = colNameMaps[colName];
                            }

                        }
                        else if (colNameMaps.ContainsKey(colName.ToLower()))
                        {
                            if (!String.IsNullOrEmpty(colNameMaps[colName.ToLower()]))
                            {
                                colName = colNameMaps[colName.ToLower()];
                            }
                        }
                    }
                    sb.Append(String.Format("<{0}>{1}</{0}>", colName.ToLower(), dr[col.ColumnName].ToString()));
                }
                sb.Append("</record>");
            }
            sb.Append("</records></root>");

            HttpResponse res = HttpContext.Current.Response;
            res.ContentType = "text/xml";
            res.Charset = "utf-8";
            res.ContentEncoding = Encoding.UTF8;
            res.Write(sb.ToString());
        }



        /// <summary>
        /// 将DataTable转换成为Json格式
        /// </summary>
        /// <param name="table"></param>
        /// <param name="excludedColNames">不需要显示的列</param>
        /// <param name="colNameMaps"> 列名需要重命名的列</param>
        /// <returns></returns>
        public static void PrintJson4DataTable(
            DataTable table, List<String> excludedColNames, Dictionary<String, String> colNameMaps)
        {
            System.Web.Script.Serialization.JavaScriptSerializer
                serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("return_code", APIUtil.SUCC);
            result.Add("msg", APIUtil.MSG_OK);
            result.Add("count", table.Rows.Count);

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in table.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    //不需要显示的列处理
                    if (excludedColNames != null && excludedColNames.Count > 0 &&
                        excludedColNames.Contains(col.ColumnName, StringComparer.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    //列明需要重命名的列
                    String colName = col.ColumnName;
                    if (colNameMaps != null && colNameMaps.Count > 0)
                    {
                        if (colNameMaps.ContainsKey(colName))
                        {
                            if (!String.IsNullOrEmpty(colNameMaps[colName]))
                            {
                                colName = colNameMaps[colName];
                            }

                        }
                        else if (colNameMaps.ContainsKey(colName.ToLower()))
                        {
                            if (!String.IsNullOrEmpty(colNameMaps[colName.ToLower()]))
                            {
                                colName = colNameMaps[colName.ToLower()];
                            }
                        }
                    }
                    row.Add(colName.ToLower(), dr[col.ColumnName]);
                }
                rows.Add(row);
            }
            result.Add("records", rows);
            //return serializer.Serialize(result);

            HttpResponse res = HttpContext.Current.Response;
            res.ContentType = "text/plain";
            res.Charset = "utf-8";
            res.ContentEncoding = Encoding.UTF8;
            res.Write(serializer.Serialize(result));
        }

        public static Dictionary<string, string> GetQueryStrings()
        {
            string paramstr = APIUtil.getReqParamStr();
            //  Hashtable paramHt=new Hashtable();
            Dictionary<string, string> paramDic = new Dictionary<string, string>();
            string[] paramArr = paramstr.Split('&');
            foreach (string str in paramArr)
            {
                int pos = str.IndexOf('=');
                string name = str.Substring(0, pos);
                //string value = HttpContext.Current.Request.QueryString[name];
                string value = str.Substring(pos + 1);

                if (!string.IsNullOrEmpty(value))
                {
                    paramDic.Add(name, value);
                }
            }

            return paramDic;
        }

        public static int StringToInt(string str)
        {
            try
            {
                return int.Parse(str);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("APIUtil.paramIsInt").Info("string(" + str + ") to int error:" + ex.Message);
                throw new ServiceException(ERR_APPKEY, str + "参数不为int");
            }
        }

        public static bool StringToBool(string str)
        {
            string strb = str.ToLower();
            if (strb == "true")
                return true;
            else
                return false;
        }
    }
}