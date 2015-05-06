using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace CFT.CSOMS.BLL.TravelModule
{
    public class AirTickets
    {
        private DAL.TravelModule.AirTickets Dal = new DAL.TravelModule.AirTickets();
        private static Dictionary<string, int> query_typeInfo;
        private static Dictionary<string, string> SpInfo;
        static AirTickets()
        {
            query_typeInfo = new Dictionary<string, int>();
            query_typeInfo.Add("TextSppreno", 1);
            query_typeInfo.Add("TextTransaction_id", 2);
            query_typeInfo.Add("TextInsur_no", 3);
            //   query_typeInfo.Add("", 4);
            query_typeInfo.Add("TextTicketno", 5);
            query_typeInfo.Add("TextCert_id", 6);
            query_typeInfo.Add("TextPassenger_name", 7);
            query_typeInfo.Add("TextMobile", 8);
            query_typeInfo.Add("TextName", 9);



            SpInfo = new Dictionary<string, string>();
            SpInfo.Add("qunar", "去哪儿网");
            SpInfo.Add("csair", "南航直销专区");
            SpInfo.Add("ceair", "东航直销专区");
            SpInfo.Add("hnair", "海航直销专区");
            SpInfo.Add("szair", "深航直销专区");
            SpInfo.Add("scair", "川航直销专区");


            SpInfo.Add("gh_ntn", "黄金假日");
            SpInfo.Add("51book_ntn", "财付通专区");
        }

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
            var answer = Dal.AirTicketsOrderQuery(query_type, wd, trade_type, uin, start_time, end_time, sp_code, limit, page_id);
            string msg;
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
        /// 获取查询条件和查询类型
        /// </summary>
        /// <param name="query_type">查询类型</param>
        /// <param name="txtBox">查询条件的TextBox控件对象数组</param>
        /// <returns></returns>
        public string GetKeyWordAndType(out int query_type, params TextBox[] txtBox)
        {
            foreach (var item in txtBox)
            {
                var txt = item.Text.Trim();
                if (!string.IsNullOrEmpty(txt))
                {
                    if (query_typeInfo.ContainsKey(item.ID))
                    {
                        query_type = query_typeInfo[item.ID];
                        return txt;
                    }
                }
            }
            query_type = 0;
            return string.Empty;
        }

        /// <summary>
        /// 获取订单状态文本
        /// </summary>
        /// <param name="strTrade_state"></param>
        /// <returns></returns>
        public string GetTradeState(string trade_state)
        {
            switch (trade_state)
            {
                case "1":
                case "2": return "创建订单";
                case "3": return "占座未支付";
                case "4": return "申请支付";
                case "5":
                case "6": return "支付成功";
                case "7": return "出票异常";
                case "8":
                case "12":
                case "13": return "出票成功";
                case "14":
                case "16": return "部分退票中";
                case "17": return "部分退票成功";
                case "18": return "部分退票失败";
                case "19":
                case "21": return "全部退票中";
                case "22": return "全部退票成功";
                case "23": return "全部退票失败";
                case "11": return "出票失败";
                case "99": return "抢购成功";
                default: return "未知";
            }
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
                    item["strTrade_state"] = GetTradeState(trade_state);
                    item["trip_flag_str"] = trip_flag == "1" ? "去程" : "回程";

                    //adult_airport_tax     adult_fuel_tax  adult_price
                    //child_airport_tax     child_fuel_tax  child_price
                    item["adult_total_money"] = ((Convert.ToDecimal(item["adult_airport_tax"]) + Convert.ToDecimal(item["adult_fuel_tax"]) + Convert.ToDecimal(item["adult_price"])) / 100 * Convert.ToInt32(item["adult_num"])).ToString("f2");
                    item["child_total_money"] = ((Convert.ToDecimal(item["child_airport_tax"]) + Convert.ToDecimal(item["child_fuel_tax"]) + Convert.ToDecimal(item["child_price"])) / 100 * Convert.ToInt32(item["child_num"])).ToString("f2"); ;
                    item.EndEdit();
                }
            }
        }
    }
}
