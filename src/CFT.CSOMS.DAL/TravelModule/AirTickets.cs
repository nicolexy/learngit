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

        private static Dictionary<string, string> SpInfo;
        private static Dictionary<string, string> TradeStateInfo;
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
        public DataSet AirTicketsOrderQuery(int query_type, string wd, string trade_type, string uin, DateTime start_time, DateTime end_time, string sp_code, int limit, int page_id = 1)
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
            //using (StreamReader sr = new StreamReader(@"C:\Users\Administrator\Desktop\kyao\01.xml", true))
            //{
            //    answer = sr.ReadToEnd(); //读取桌面XML文件模拟
            //}
            if (answer == null)
            {
                throw new ArgumentNullException("获取CGI失败 URL:" + cgi);
            }
            //请求失败
            if (answer.IndexOf("<retcode>00</retcode>") == -1)
            {
                throw new Exception("请求错误返回值是:[" + answer + "]");
            }
            //如果有记录
            if (answer.IndexOf("<ret_num>0</ret_num>") == -1)
            {
                var ds = CommQuery.PaseCgiXmlForTravelPlatform(answer, out msg);
                var dts = AnalyzeXmlPersonsByString(answer, out msg);
                if (dts != null)
                {
                    ds.Tables.AddRange(dts);
                }
                FieldMapString(ds);
                return ds;
            }
            return null;
        }

        /// <summary>
        /// 解析XML 中 passengers|contact 两个节点
        /// </summary>
        /// <param name="answer"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private DataTable[] AnalyzeXmlPersonsByString(string answer, out string msg)
        {
            msg = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(answer);
                XmlElement root = doc.DocumentElement;
                DataTable passengers = new DataTable("passengers");
                DataTable contact = new DataTable("contact");
                var strType = typeof(string);
                var records = root.SelectSingleNode("records");

                passengers.Columns.Add("listid", strType);
                var passengersTitles = records.FirstChild.SelectSingleNode("passengers").FirstChild.Attributes.Cast<XmlAttribute>().Select(u => new DataColumn(u.Name, strType)).ToArray();
                passengers.Columns.AddRange(passengersTitles);

                contact.Columns.Add("listid", strType);
                var contactTitles = records.FirstChild.SelectSingleNode("contact").Attributes.Cast<XmlAttribute>().Select(u => new DataColumn(u.Name, strType)).ToArray();
                contact.Columns.AddRange(contactTitles);
                contact.Columns.Add("mailtype");

                foreach (XmlNode item in records)
                {
                    var listid = item.SelectSingleNode("listinfo").Attributes.GetNamedItem("listid").Value;
                    foreach (XmlNode passenger in item.SelectSingleNode("passengers").ChildNodes)
                    {
                        var pRow = passengers.NewRow();
                        foreach (XmlAttribute attr in passenger.Attributes)
                        {
                            pRow[attr.Name] = attr.Value;
                        }
                        pRow["listid"] = listid;
                        passengers.Rows.Add(pRow);
                    }
                    var cRow = contact.NewRow();
                    foreach (XmlAttribute attr in item.SelectSingleNode("contact").Attributes)
                    {
                        cRow[attr.Name] = attr.Value;
                    }

                    var xcd = item.SelectSingleNode("xcdexpress");
                    if (xcd != null)
                    {
                        cRow["mailtype"] = xcd.Attributes["mailtype"].Value ?? "";
                    }
                    cRow["listid"] = listid;
                    contact.Rows.Add(cRow);
                }

                return new DataTable[] { passengers, contact };
            }
            catch (Exception ex)
            {
                msg = "解析XML,passengers|contact 两个节点出错" + ex.ToString();
                return null;
            }
        }


        /// <summary>
        /// 把DataSet中的一些代码字段映射成文本
        /// </summary>
        /// <param name="ds"></param>
        private void FieldMapString(DataSet ds)
        {
            var pDT = ds.Tables["passengers"];
            if (pDT != null && pDT.Rows.Count > 0)
            {
                pDT.Columns.Add("cert_type_str", typeof(string));
                pDT.Columns.Add("type_str", typeof(string));
                foreach (DataRow item in pDT.Rows)
                {
                    var certType = item["cert_type"].ToString();
                    var _type = item["type"].ToString();
                    item.BeginEdit();
                    item["cert_type_str"] = certType == "NI" ? "身份证" : certType == "PP" ? "护照" : "其他";//OT  其他
                    item["type_str"] = _type == "AD" ? "成人" : "儿童";  //CH  儿童
                    item.EndEdit();
                }
            }

            var DT0 = ds.Tables[0];
            if (DT0 != null && DT0.Rows.Count > 0)
            {
                DT0.Columns.Add("insur_com_str", typeof(string));
                DT0.Columns.Add("strSp_code", typeof(string));
                DT0.Columns.Add("strTrade_state", typeof(string));
                DT0.Columns.Add("trip_flag_str", typeof(string));

                DT0.Columns.Add("adult_total_money", typeof(string));
                DT0.Columns.Add("child_total_money", typeof(string));
                foreach (DataRow item in DT0.Rows)
                {
                    var company = item["insur_com"].ToString();
                    var sp_code = item["sp_code"].ToString();
                    var trade_state = item["trade_state"].ToString();
                    var trip_flag = item["trip_flag_str"].ToString();
                    item.BeginEdit();
                    item["insur_com_str"] = company == "hezhong" ? "合众" : "其他";
                    item["strSp_code"] = SpInfo.ContainsKey(sp_code) ? SpInfo[sp_code] : "未知";
                    item["strTrade_state"] = TradeStateInfo.ContainsKey(trade_state) ? TradeStateInfo[trade_state] : "其他";
                    item["trip_flag_str"] = trip_flag == "1" ? "去程" : "回程";

                    //adult_airport_tax     adult_fuel_tax  adult_price
                    //child_airport_tax     child_fuel_tax  child_price
                    var child_num = Convert.ToInt32(item["child_num"]);
                    var adult_num = Convert.ToInt32(item["adult_num"]);
                    var adult_total_money = (Convert.ToDouble(item["adult_airport_tax"]) + Convert.ToDouble(item["adult_fuel_tax"]) + Convert.ToDouble(item["adult_price"])) * adult_num;
                    var child_total_money = (Convert.ToDouble(item["child_airport_tax"]) + Convert.ToDouble(item["child_fuel_tax"]) + Convert.ToDouble(item["child_price"])) * child_num;
                    item["adult_total_money"] = TENCENT.OSS.CFT.KF.Common.MoneyTransfer.FenToYuan(adult_total_money);
                    item["child_total_money"] = TENCENT.OSS.CFT.KF.Common.MoneyTransfer.FenToYuan(child_total_money);
                    item.EndEdit();
                }
            }
        }


        static AirTickets()
        {

            SpInfo = new Dictionary<string, string>();
            SpInfo.Add("qunar", "去哪儿网");
            SpInfo.Add("csair", "南航直销专区");
            SpInfo.Add("ceair", "东航直销专区");
            SpInfo.Add("hnair", "海航直销专区");
            SpInfo.Add("szair", "深航直销专区");
            SpInfo.Add("scair", "川航直销专区");
            SpInfo.Add("gh_ntn", "黄金假日");
            SpInfo.Add("51book_ntn", "财付通专区");

            TradeStateInfo = new Dictionary<string, string>();
            TradeStateInfo.Add("1", "创建订单");
            TradeStateInfo.Add("2", "创建订单");
            TradeStateInfo.Add("3", "占座未支付");
            TradeStateInfo.Add("4", "申请支付");
            TradeStateInfo.Add("5", "支付成功");
            TradeStateInfo.Add("6", "支付成功");
            TradeStateInfo.Add("7", "出票异常");
            TradeStateInfo.Add("8", "出票成功");
            TradeStateInfo.Add("12", "出票成功");
            TradeStateInfo.Add("13", "出票成功");
            TradeStateInfo.Add("14", "部分退票中");
            TradeStateInfo.Add("16", "部分退票中");
            TradeStateInfo.Add("17", "部分退票成功");
            TradeStateInfo.Add("18", "部分退票失败");
            TradeStateInfo.Add("19", "全部退票中");
            TradeStateInfo.Add("21", "全部退票中");
            TradeStateInfo.Add("22", "全部退票成功");
            TradeStateInfo.Add("23", "全部退票失败");
            TradeStateInfo.Add("11", "出票失败");
            TradeStateInfo.Add("99", "抢购成功");

        }
    }
}
