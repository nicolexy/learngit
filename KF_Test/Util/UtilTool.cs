using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
namespace KF_Test.Util
{
    class UtilTool
    {
        public static XmlDocument testAPI(string serviceName, string interfaceName, string queryString)
        {

            //以测试环境url创建HttpWebRequest
            System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest
                .Create("http://localhost:61131/CSAPI/" + serviceName + "/" + interfaceName + "?" + queryString);
            //设置HttpWebRequest头信息
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 600000;
            httpWebRequest.ServicePoint.Expect100Continue = false;
            httpWebRequest.CachePolicy = new System.Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore);
            httpWebRequest.ContentType = "application/xml; charset=utf-8";
            httpWebRequest.Accept = "application/xml";

            //获取响应输入流
            System.Net.WebResponse webResponse = httpWebRequest.GetResponse();
            System.IO.Stream responseStream = webResponse.GetResponseStream();
            //读取返回流  
            var myStreamReader = new System.IO.StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            string result = myStreamReader.ReadToEnd();
            result = HttpUtility.UrlDecode(result);
            //关闭请求连接  
            myStreamReader.Close();

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(result);

            return xmldoc;
        }

        public static string GetToken(string paramstring)
        {
            paramstring = HttpUtility.UrlDecode(paramstring);
            Dictionary<string, string> paramsHt = new Dictionary<string,string>();
            string[] paramsAll = paramstring.Split('&');
            foreach (string pa in paramsAll)
            {
                string[] p = pa.Split('=');
                paramsHt.Add(p[0], p[1]);
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
                throw new Exception("appKey出错了！");
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

            return md5;
           
        }
    
    }
}

