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
    }
}
