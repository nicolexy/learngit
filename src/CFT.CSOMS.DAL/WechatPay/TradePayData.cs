using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CFT.Apollo.Common.Cryptography;
using CFT.Apollo.Logging;
using CFT.CSOMS.COMMLIB;
using CFT.CSOMS.DAL.Infrastructure;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using System.Xml;
using System.Collections;
using CFT.CSOMS.DAL.WechatPay.Entity.MmpayHongBaoModel;
using Google.ProtocolBuffers;
using SunLibraryEX;
using TENCENT.OSS.CFT.KF.DataAccess;
using BankLib;

namespace CFT.CSOMS.DAL.WechatPay
{
    public class TradePayData
    {
        /// <summary>
        /// 微信红包l5接口relay转发 action 对应requstType
        /// </summary>
        private static Dictionary<string, int> ActionDict;
        static TradePayData()
        {
            ActionDict = new Dictionary<string, int>()
            {
                {"QueryUserSendList",101205},
                {"QuerySendById",101202},
                {"QueryDetail",101203},
                {"QueryUserReceiveList",101204},
                {"QueryReceiveById",101201},
            };
        }
        public DataTable QueryWxTrans(string prime_trans_id)
        {
            if (string.IsNullOrEmpty(prime_trans_id))
            {
                throw new Exception("prime_trans_id不能为空！");
            }

            string msg;

            string serviceName = "wxt_pay_qryprime_service";
            string req = "prime_trans_id=" + prime_trans_id;
            DataSet ds = CommQuery.GetOneTableFromICE(req, "", serviceName, true, out msg);
            if (msg != "")
            {
                LogHelper.LogError(msg);
                throw new Exception(msg);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        /// <summary>
        /// 查询实时还款详情
        /// </summary>
        /// <param name="transaction_id">还款提现单号</param>
        /// <param name="acc_date">还款日期</param>
        /// <param name="status">交易状态1：请求；2：成功</param>
        /// <returns></returns>
        public DataTable QueryRealtimeRepayment(string transaction_id, DateTime acc_date, int status)
        {
            var req =
                "transaction_id=" + transaction_id +
                "&acc_date=" + acc_date.ToString("yyyyMMdd") +
                "&status=" + status.ToString();
            var ip = Apollo.Common.Configuration.AppSettings.Get<string>("QueryRealtimeRepayment_IP", "10.123.12.34");
            var port = Apollo.Common.Configuration.AppSettings.Get<int>("QueryRealtimeRepayment_Port", 22000);
            var result = RelayAccessFactory.RelayInvoke(req, "101477", false, false, ip, port);

            var msg = "";
            var ds = TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.ParseRelayStr(result, out msg, false);
            if (msg != "")
            {
                throw new Exception("查询实时还款详情出错:" + msg);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// 查询微信红包
        /// </summary>
        /// <param name="parameter">Post参数</param>
        /// <param name="action">执行的操作</param>
        /// <returns></returns>
        public DataSet QueryWechatHB(string[][] parameter, string action)
        {
            var doc = RequestPostWechatHB(parameter, action);

            if (doc == null)
                return null;

            switch (action)
            {
                case "QueryUserSendList":
                case "QuerySendById":
                    {
                        var nodes = doc.SelectNodes(@"Response/result/send_order/item");
                        var list = new List<HongBaoSendOrder>();
                        foreach (XmlNode item in nodes)
                        {
                            var byes = Convert.FromBase64String(item.FirstChild.InnerText);
                            var obj = HongBaoSendOrder.ParseFrom(byes);
                            list.Add(obj);
                        }
                        return ListToDataSet(list);
                    }
                case "QueryDetail":
                case "QueryUserReceiveList":
                case "QueryReceiveById":
                    {
                        var nodes = doc.SelectNodes(@"Response/result/receive_order/item");
                        var list = new List<HongBaoReceiveOrder>();
                        foreach (XmlNode item in nodes)
                        {
                            var byes = Convert.FromBase64String(item.FirstChild.InnerText);
                            var obj = HongBaoReceiveOrder.ParseFrom(byes);
                            list.Add(obj);
                        }
                        var ds = ListToDataSet(list);
                        if (action == "QueryDetail")
                        {
                            var node = doc.SelectSingleNode(@"Response/result/send_order");
                            var byes = Convert.FromBase64String(node.FirstChild.InnerText);
                            var obj = HongBaoSendOrder.ParseFrom(byes);
                            if (ds.Tables.Count > 0)
                            {
                                var dt = ds.Tables[0];
                                dt.Columns.Add("PayListid");
                                foreach (DataRow item in dt.Rows)
                                {
                                    item["PayListid"] = obj.PayListid.ToStringUtf8();
                                }
                            }
                        }
                        return ds;
                    }
                default: throw new Exception("参数错误");
            }
        }

        /// <summary>
        /// Post访问Http接口
        /// </summary>
        /// <param name="parameter">Post参数</param>
        /// <param name="action">操作名称</param>
        /// <returns></returns>
        protected XmlDocument RequestPostWechatHB(string[][] parameter, string action)
        {
            //var front = System.Configuration.ConfigurationManager.AppSettings["WebchatHongBaoHTTP"] ?? "http://10.12.199.212:11903/cgi-bin/wxhb/";
            //var url = front + action.ToLower() + "?f=xml";
            var ip = System.Configuration.ConfigurationManager.AppSettings["WebchatHongBaoRelayL5_Ip"] ?? "10.198.132.188"; //10.12.23.14
            var port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["WebchatHongBaoRelayL5_Port"] ?? "22000");

            #region 参数组合
            var buff = new StringBuilder();
            buff.Append("<root>");
            for (int i = 0; i < parameter.Length; i++)
            {
                var cur = parameter[i];
                buff.AppendFormat("<{0}>{1}</{2}>", cur[0], cur[1], cur[0]);
            }
            buff.Append("</root>");
            var req = "wechat_xml_text=" + buff.ToString();
            #endregion

            string msg = "";
            //var result1111 = commRes.GetFromCGIPost(url, "GBK", buff.ToString(), out msg);
            var requstType = ActionDict[action].ToString();
            var relay_result = RelayAccessFactory.RelayInvoke(req, requstType, false, false, ip, port);

            #region relay转发  响应结果处理
            if (msg != "")
            {
                throw new Exception(msg);
            }
            var relay_dic = relay_result.ToDictionary();
            if (relay_dic["result"] != "0")
            {
                throw new Exception("relay转发l5,异常 [" + relay_result + "]");
            }
            var result = System.Web.HttpUtility.UrlDecode(relay_dic["res_info"]);
            #endregion

            #region 接口响应处理
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            var reState = doc.SelectSingleNode(@"Response/result/ret_code");
            if (reState != null && reState.InnerText == "0")
            {
                return doc;
            }
            //else
            //{
            //    var re_msg = doc.SelectSingleNode(@"Response/result/ret_msg");
            //    if (re_msg != null)
            //        throw new Exception(re_msg.InnerText);
            //    else
            //        throw new Exception("接口错误");
            //}
            return null;
            #endregion
        }

        /// <summary>
        /// 集合转DataSet
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="list">集合</param>
        /// <returns></returns>
        protected DataSet ListToDataSet<T>(IEnumerable<T> list)
        {
            var ds = new DataSet();
            if (list.Any())
            {
                var dt = new DataTable();
                var properties = typeof(T).GetProperties().Where(u => !(u.PropertyType.Name == "Boolean" && u.Name.IndexOf("Has") == 0) && u.Name != "Item").ToArray();
                dt.Columns.AddRange(properties.Select(u => new DataColumn(u.Name)).ToArray());
                foreach (var item in list)
                {
                    var row = dt.NewRow();
                    foreach (var fild in properties)
                    {
                        string value = "";
                        switch (fild.PropertyType.Name)
                        {
                            case "ByteString": value = ((ByteString)fild.GetValue(item, null)).ToStringUtf8(); break;
                            default: value = fild.GetValue(item, null).ToString(); break;
                        }
                        row[fild.Name] = value;
                    }
                    dt.Rows.Add(row);
                }
                ds.Tables.Add(dt);
            }
            return ds;
        }

        /// <summary>
        /// 获取微信用户的参与的AA交易记录
        /// </summary>
        public DataSet GetAATradeList(string aaUIN, int startIndex, int count)
        {
            var aaUId = PublicRes.ConvertToFuid(aaUIN);

            if (aaUId == null)
                throw new Exception(string.Format("AA财付通帐号{0}查询不到对应的UID", aaUIN));

            var tableName = string.Format("wx_aa_collection_{0}.t_aa_record_{1}", aaUId.Substring(aaUId.Length - 2), aaUId.Substring(aaUId.Length - 3, 1));

            string strSql = string.Format(@"select * from {0} where Fuid='{1}' order by Fcreate_time desc limit {2},{3}", tableName, aaUId, startIndex, count);

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("WXAA"));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
            }
        }

        /// <summary>
        /// 获取指定的AA收单分单记录明细
        /// </summary>
        public DataSet GetAATradeDetailsSingleYear(string aaCollectionNo, DateTime createTime, int startIndex, int count)
        {
            var tableName = string.Format("wx_aa_collection.t_pay_list_{0}", createTime.ToString("yyyy"));
            //var nextYearTableName = string.Format("wx_aa_collection.t_pay_list_{0}", createTime.AddYears(1).ToString("yyyy"));

            string strSql = string.Format(@"select * from {0} where Faa_collection_no='{1}' order by Fcreate_time desc limit {2},{3}", tableName, aaCollectionNo, startIndex, count);

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("WXAA"));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
            }
        }

        /// <summary>
        /// 获取指定的AA收款总单信息
        /// </summary>
        /// <param name="aaCollectionNo"></param>
        /// <returns></returns>
        public DataSet QueryAATotalTradeInfo(string aaCollectionNo)
        {
            var tableName = string.Format("wx_aa_collection_{0}.t_collection_list_{1}", aaCollectionNo.Substring(aaCollectionNo.Length - 2), aaCollectionNo.Substring(aaCollectionNo.Length - 3, 1));
            //var nextYearTableName = string.Format("wx_aa_collection.t_pay_list_{0}", createTime.AddYears(1).ToString("yyyy"));

            string strSql = string.Format(@"select * from {0} where Faa_collection_no='{1}' limit 1", tableName, aaCollectionNo);

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("WXAA"));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
            }
        }

        /// <summary>
        /// 查询申报流水
        /// </summary>
        /// <param name="partner">商户号</param>
        /// <param name="transaction_id">支付单号</param>
        /// <param name="out_trade_no">商户订单号</param>
        /// <param name="sub_order_no">子商户订单号</param>
        /// <param name="sub_order_id">子支付单号</param>
        /// <returns></returns>
        public DataSet QueryDeclareDogInfo(string partner, string transaction_id, string out_trade_no, string sub_order_no, string sub_order_id)
        {
            if (string.IsNullOrEmpty(partner.Trim()) && string.IsNullOrEmpty(transaction_id.Trim()))
            {
                throw new Exception("商户号和支付单号不能同时为空！");
            }
            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("CustomsIP", "10.123.6.29");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("CustomsPort", 443);

            string requestText = "1=1";
            if (!string.IsNullOrEmpty(partner))
            {
                requestText += "&partner=" + partner;
            }
            if (!string.IsNullOrEmpty(transaction_id))
            {
                requestText += "&transaction_id=" + transaction_id;
            }

            if (!string.IsNullOrEmpty(out_trade_no))
            {
                requestText += "&out_trade_no=" + out_trade_no;
            }
            if (!string.IsNullOrEmpty(sub_order_no))
            {
                requestText += "&sub_order_no=" + sub_order_no;
            }
            if (!string.IsNullOrEmpty(sub_order_id))
            {
                requestText += "&sub_order_id=" + sub_order_id;
            }

            requestText = string.Format(requestText, partner, transaction_id, out_trade_no, sub_order_no, sub_order_id);
            string answer = RelayAccessFactory.RelayInvoke(requestText, "101658", true, false, ip, port, "");
            // string answer = "result=0&res_info=ok&partner=1234567890&out_trade_no=45678901&transaction_id=12343531231231&count=1&mch_customs_no_0=4401962010&customs_0=1&state_0=1&modify_time_0=20151208132419&business_type_0=1&explanation_0=";
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            if (answer.Contains("result=0&res_info=ok"))
            {
                dt1 = Getdata(answer, "partner,out_trade_no,transaction_id,count");
                dt2 = Getdata(answer, "mch_customs_no,customs,state,explanation,modify_time,sub_order_no,sub_order_id,fee_type,order_fee,duty,transport_fee,product_fee,business_type", true);
                ds.Tables.Add(dt1);
                ds.Tables.Add(dt2);
            }
            else
            {
                throw new Exception("请求串:" + requestText + "; 返回串:" + answer);
            }
            return ds;
        }
       /// <summary>
        /// 查询海关配置
       /// </summary>
       /// <param name="partner"></param>
       /// <returns></returns>
        public DataSet QueryMerchantCustom(string partner)
        {
            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("CustomsIP", "10.123.6.29");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("CustomsPort", 443);

            string requestText = "partner=" + partner;
            string answer = RelayAccessFactory.RelayInvoke(requestText, "101659", true, false, ip, port, "");
//#if DEBUG
//            answer = "result=0&res_info=ok&partner=1234567890&merchant_type=0&partner_customs_conf=[{\"custom_id\":\"1\",\"customs_company_code\":\"TEST\",\"customs_company_name\":\"测试（ 空格 ）士大夫啊 的啊\"}]&sp_name=&contact_name=X9g00J7um0k%3d&contact_email=OlrL-1xBYqJrMoQ8pkq8xYLkRdY4xl_J&contact_phone=APOBWa-bAUJA7C7-OyrHww%3d%3d";
//#endif
            if (!answer.Contains("result=0&res_info=ok"))
            {
                throw new Exception("请求串:" + requestText + ";返回串:" + answer);
            }
            string msg;
            DataTable dt1 = Getdata(answer);
            DataTable dt2 = new DataTable();
            if (dt1 != null && dt1.Rows.Count > 0)
            {
                string partner_customs_conf = dt1.Rows[0]["partner_customs_conf"].ToString();
                dt2 = TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.JSONToDataTable(partner_customs_conf);
            }
            return new DataSet() { Tables = { dt1, dt2 } };

            // requestText = "partner=1234567890&customs=1&customs_company_code=4401962010&total_num=2&transaction_id_0=12343531231231&out_trade_no_0=423&redeclare_reason_0=no reason&transaction_id_1=1213&out_trade_no_1=423&redeclare_reason_1=no reason";
            // answer = RelayAccessFactory.RelayInvoke(requestText, "101660", true, false, ip, port, "");
            //var ds = TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.ParseRelayStringToDataTable(result, out msg, false);
            // return null;

        }
        /// <summary>
        /// 重推
        /// </summary>
        /// <param name="requesttext"></param>
        /// <returns></returns>
        public DataTable CustomsRedeclare(string requesttext)
        {
            DataTable dt = null;

            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("CustomsIP", "10.123.6.29");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("CustomsPort", 443);

            string answer = RelayAccessFactory.RelayInvoke(requesttext, "101725", true, false, ip, port, "");
            if (answer.Contains("result=0&res_info=ok"))
            {
                dt = Getdata(answer, "transaction_id,out_trade_no,fail_info", true);
            }
            else
            {
                throw new Exception("请求串:" + requesttext + ";返回串：" + answer);
            }

            return dt;
        }

        /// <summary>
        /// 解析result=0&res_info=ok&partner=1234567890&out_trade_no=45678901&transaction_id=12343531231231&count=1&mch_customs_no_0=4401962010&customs_0=1&state_0=1&modify_time_0=20151208132419&business_type_0=1&explanation_0=
        /// </summary>
        /// <param name="answer"></param>
        /// <param name="cols"></param>
        /// <param name="multiRows"></param>
        /// <returns></returns>
        private DataTable Getdata(string answer, string cols = "", bool multiRows = false)
        {
            Hashtable ht = new Hashtable();
            foreach (string item in answer.Split('&'))
            {
                string[] _item = item.Split('=');
                ht.Add(_item[0], _item[1]);
            }
            DataTable dt = new DataTable();

            if (string.IsNullOrEmpty(cols))
            {
                foreach (DictionaryEntry item in ht)
                {
                    dt.Columns.Add(item.Key.ToString(), typeof(string));
                }
                DataRow dr = dt.NewRow();
                foreach (DictionaryEntry item in ht)
                {
                    dr[item.Key.ToString()] = item.Value.ToString();
                }
                dt.Rows.Add(dr);
                return dt;
            }
            else
            {
                foreach (string item in cols.Split(','))
                {
                    dt.Columns.Add(item, typeof(string));
                }

                if (!multiRows)
                {
                    DataRow drF = dt.NewRow();
                    foreach (string item in cols.Split(','))
                    {
                        if (ht.Contains(item))
                        {
                            drF[item] = ht[item].ToString().Trim();
                        }
                    }
                    dt.Rows.Add(drF);
                }
                else
                {
                    int count = ht.Contains("count") ? Convert.ToInt32(ht["count"].ToString().Trim()) : Convert.ToInt32(ht["fail_num"].ToString().Trim());
                    for (int i = 0; i < count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        foreach (string item in cols.Split(','))
                        {
                            if (ht.Contains(item + "_" + i))
                            {
                                dr[item] = ht[item + "_" + i].ToString();
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
                return dt;
            }
        }
        /// <summary>
        /// 解析result=0&res_info=ok&partner=1234567890&out_trade_no=45678901&transaction_id=12343531231231&count=1&mch_customs_no_0=4401962010&customs_0=1&state_0=1&modify_time_0=20151208132419&business_type_0=1&explanation_0=
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        private DataTable Getdata2(string answer)
        {
            Hashtable ht = new Hashtable();
            foreach (string item in answer.Split('&'))
            {
                string[] _item = item.Split('=');
                ht.Add(_item[0], _item[1]);
            }
            DataTable dt = new DataTable();

            foreach (DictionaryEntry item in ht)
            {
                if (item.Key.ToString().EndsWith("_0"))
                {
                    dt.Columns.Add(item.Key.ToString().Replace("_0", ""), typeof(string));
                }
            }

            int i = 0;
            while (true)
            {
                bool endwhile = true;
                DataRow dr = dt.NewRow();
                foreach (DataColumn dc in dt.Columns)
                {
                    string colName = dc.ColumnName;
                    if (ht.Contains(colName + "_" + i))
                    {
                        dr[colName] = ht[colName + "_" + i].ToString();
                        endwhile = false;
                    }
                }
                if (endwhile)
                {
                    break;
                }
                dt.Rows.Add(dr);
                i++;
            }
            return dt;
        }
    }
}
