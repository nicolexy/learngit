using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace CFT.CSOMS.DAL.TravelModule
{
    public class AirTickets
    {

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="query_type">查询类型</param>
        /// <param name="wd">关键字</param>
        /// <param name="trade_type">取值状态</param>
        /// <param name="uin">财付通账号</param>
        /// <param name="start_time">开始时间</param>
        /// <param name="end_time">结束时间</param>
        /// <param name="sp_code">Sp代码</param>
        /// <param name="limit">页大小</param>
        /// <param name="page_id">当前页</param>
        /// <returns></returns>
        public string AirTicketsOrderQuery(int query_type, string wd, string trade_type, string uin, DateTime start_time, DateTime end_time, string sp_code, int limit, int page_id = 1)
        {
            var url = System.Configuration.ConfigurationManager.AppSettings["QueryAirOrderByParamCgi"];
            var cgi = url
               + "?query_type=" + query_type
               + "&wd=" + System.Web.HttpUtility.UrlEncode(wd.ToString(), System.Text.Encoding.GetEncoding("gb2312"))
               + "&trade_type=" + trade_type
               + "&uin=" + uin
               + "&start_time=" + start_time.ToString("yyyy-MM-dd")
               + "&end_time=" + end_time.ToString("yyyy-MM-dd")
               + "&sp_code=" + sp_code
               + "&limit=" + limit
               + "&page_id=" + page_id
              ;
            string msg;
            var answer = commRes.GetFromCGI(cgi, null, out msg);
            //using (StreamWriter sw = new StreamWriter(@"C:\Users\Administrator\Desktop\新建文件夹\Log.txt", true))
            //{
            //    sw.WriteLine(cgi);
            //}
            //using (StreamReader sr = new StreamReader(@"C:\Users\Administrator\Desktop\新建文件夹\01.xml", true))
            //{
            //    answer = sr.ReadToEnd(); //读取桌面XML文件模拟
            //}
            if (answer == null)
            {
                throw new ArgumentNullException("获取CGI失败 URL:" + cgi);
            }
            return answer;
        }
    }
}
