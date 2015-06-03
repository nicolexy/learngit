using CFT.CSOMS.DAL.Infrastructure;
using SunLibraryEX;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace CFT.CSOMS.DAL.ForeignCardModule
{
    public class FCUserTradeQuery
    {
        private static string Ip = System.Configuration.ConfigurationManager.AppSettings["FCUserTradeQueryIp"] ?? "10.197.8.104";
        private static int Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["FCUserTradeQueryPort"] ?? "22000");

        /// <summary>
        /// 前置查询:--外币用户交易信息查询
        /// </summary>
        /// <param name="uin">财富通账号</param>
        /// <param name="list_type">交易单:101 退款单:102</param>
        /// <param name="limit">页大小</param>
        /// <param name="offset">跳过行</param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, string>> QueryFCTradeInfoByUin(string uin, short? list_type, int limit, int offset)
        {
            var req = "uin=" + uin + "&limit=" + limit + "&offset=" + offset;
            if (list_type != null) req += "&list_type=" + list_type;

            string answer = RelayAccessFactory.RelayInvoke(req, "101101", true, false, Ip, Port);
            var dic = StringEx.ToDictionary(answer); //AnalyzeDictionary(answer);
            if (dic["result"] != "0")
            {
                throw new Exception("查询失败:" + answer);
            }
            var num = int.Parse(dic["row_num"]);
            if (num > 0)
            {
                var info = dic["row_info"];
                return AnalyzeStringToDictionary(info);
            }
            return null;
        }

        /// <summary>
        /// 外币用户交易单或退款单查询
        /// </summary>
        /// <param name="tradeInfo">交易信息表</param>
        /// <param name="list_type">交易单:101 退款单:102</param>
        /// <param name="uin">财富通账号</param>
        /// <returns></returns>
        public DataSet QueryFCTradeBillsAndRefund(Dictionary<string, Dictionary<string, string>> tradeInfo, short list_type, string uin)
        {
            var ds = new DataSet();
            DataTable dt = null;
            foreach (Dictionary<string, string> item in tradeInfo.Values)
            {
                var type = item["list_type"];
                if ((string)type == list_type.ToString())
                {
                    var req = "listid=" + item["listid"] + "&type=" + item["list_type"] + "&uin=" + uin;
                    string answer = RelayAccessFactory.RelayInvoke(req, "101102", true, false, Ip, Port);
                    var dic = StringEx.ToDictionary(answer);
                    if (dic["result"] == "0")
                    {
                        dic.Add("list_state", item["list_state"]);
                        if (dt == null)
                        {
                            dt = new DataTable();
                            dt.Columns.AddRange(dic.Keys.Select(u => new DataColumn(u)).ToArray());
                            ds.Tables.Add(dt);
                        }
                        var row = dt.NewRow();
                        foreach (var kv in dic)
                        {
                            row[kv.Key] = kv.Value;
                        }
                        dt.Rows.Add(row);
                    }
                }
            }
            return ds;
        }

        /// <summary>
        /// 查询绑卡记录
        /// </summary>
        /// <param name="uin">财富通账号</param>
        /// <returns></returns>
        public DataSet QueryFCBindCardRecord(string uin)
        {
            var req = "uin=" + uin;

            string answer = RelayAccessFactory.RelayInvoke(req, "101103", true, false, Ip, Port);
            var dic = StringEx.ToDictionary(answer); //AnalyzeDictionary(answer);
            if (dic["result"] != "0")
            {
                throw new Exception("查询失败:" + answer);
            }
            var num = int.Parse(dic["row_num"]);
            if (num > 0)
            {
                var info = dic["row_info"];
                var dt = DictionaryToDataTable(AnalyzeStringToDictionary(info, "%2526", "%253D"));
                if (dt != null)
                {
                    var ds = new DataSet();
                    ds.Tables.Add(dt);
                    return ds;
                }
            }
            return null;
        }

        /// <summary>
        /// 前置查询:--资金流水查询
        /// </summary>
        /// <param name="uin">微信账号</param>
        /// <param name="cur_type">币种类型</param>
        /// <param name="acc_type">账户类型</param>
        /// <param name="client_ip">客户端ip</param>
        /// <returns>接口响应字典</returns>
        public Dictionary<string, string> QueryFCFlowInfoByUin(string uin, string cur_type, string acc_type, string client_ip)
        {
            var req = "uin=" + uin +
                "&cur_type=" + cur_type +
                "&acc_type=" + acc_type +
                "&client_ip=" + client_ip;
            string answer = RelayAccessFactory.RelayInvoke(req, "100385", false, false, Ip, Port);
            var dic = StringEx.ToDictionary(answer); //AnalyzeDictionary(answer);
            if (dic["result"] != "0")
            {
                throw new Exception("查询失败:" + answer);
            }
            return dic;
        }

        /// <summary>
        /// 资金流水查询
        /// </summary>
        /// <param name="acno"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public DataTable QueryFCFlow(string acno, int limit, int offset)
        {
            var req = "acno=" + acno + "&limit=" + limit + "&offset=" + offset;
            string answer = RelayAccessFactory.RelayInvoke(req, "101109", true, false, Ip, Port);
            var dic = StringEx.ToDictionary(answer); //AnalyzeDictionary(answer);
            if (dic["result"] != "0")
            {
                throw new Exception("查询失败:" + answer);
            }
            if (dic["row_num"] != "0")
            {
                return DictionaryToDataTable( AnalyzeStringToDictionary(dic));
            }
            return null;
        }





        /// <summary>
        /// 把Relay返回值解析成字典x_0&xx_0&x_1&xx1
        /// </summary>
        /// <param name="str">Relay返回值</param>
        /// <returns></returns>
        private Dictionary<string, Dictionary<string, string>> AnalyzeStringToDictionary(string str, string separator1 = "%26", string separator2 = "%3D")
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            var kv_arr = str.Split(new string[] { separator1 }, StringSplitOptions.None);
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            for (int i = 0; i < kv_arr.Length; i++)
            {
                var kvs = kv_arr[i];
                var kv = kvs.Split(new string[] { separator2 }, StringSplitOptions.None);
                var knum_str = kv[0];
                var value = kv[1];
                var index = knum_str.LastIndexOf('_');
                var k = knum_str.Substring(0, index);
                var num = knum_str.Substring(index + 1);
                if (!dic.ContainsKey(num))
                {
                    dic[num] = new Dictionary<string, string>();
                }
                dic[num].Add(k, value);
            }
            return dic;
        }

        /// <summary>
        /// 把Relay返回值解析成字典row1=1&row=2
        /// </summary>
        /// <param name="str">Relay返回值</param>
        /// <returns></returns>
        private Dictionary<string, Dictionary<string, string>> AnalyzeStringToDictionary(Dictionary<string, string> dic)
        {
            Dictionary<string, Dictionary<string, string>> analyzeDic = new Dictionary<string, Dictionary<string, string>>();
            foreach (var item in dic)
            {
                if (Regex.IsMatch(item.Key, @"row[\d]+"))
                {
                    if (!analyzeDic.ContainsKey(item.Key))
                    {
                        analyzeDic[item.Key] = new Dictionary<string, string>();
                    }
                    analyzeDic[item.Key] = CommQuery.IceDecode(item.Value).ToDictionary();
                }
            }
            return analyzeDic;
        }

        /// <summary>
        /// 把字典对象转换成DataTable对象
        /// </summary>
        /// <param name="dic">字典</param>
        /// <param name="dt">DataTable对象</param>
        /// <returns></returns>
        public DataTable DictionaryToDataTable(Dictionary<string, Dictionary<string, string>> dic, DataTable dt = null)
        {
            if (dt != null)
            {
                if (dt.Columns.Count == 0)
                {
                    throw new Exception("DataTable 对象必须存在Column");
                }
            }
            else
            {
                if (dic.Count > 0)
                {
                    dt = new DataTable();
                    var column = dic.Values.First().Keys.Select(u => new DataColumn(u)).ToArray();
                    dt.Columns.AddRange(column);
                    foreach (var item in dic.Values)
                    {
                        var row = dt.NewRow();
                        foreach (var kv in item)
                        {
                            row[kv.Key] = kv.Value;
                        }
                        dt.Rows.Add(row);
                    }
                }
            }
            return dt;
        }
    }
}