using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.BLL.ForeignCurrencyModule
{
    public class FCUserTradeQuery
    {
        CFT.CSOMS.DAL.ForeignCardModule.FCUserTradeQuery dal = new CFT.CSOMS.DAL.ForeignCardModule.FCUserTradeQuery();
        //交易状态
        private static Dictionary<string, string> TradeState;
        //交易类型
        private static Dictionary<string, string> TradeType;
        //交易单状态
        private static Dictionary<string, string> ListState;
        //币种
        private static Dictionary<string, string> PriceCurType;
        //资金流向
        private static Dictionary<string, string> PriceFlowDirection;
        //资金流向
        private static Dictionary<string, string> CardType;
        //标示
        private static Dictionary<string, string> DeleteFlag;
        static FCUserTradeQuery()
        {
            TradeType = new Dictionary<string, string>() 
            { 
                { "2", "B2C" },
                { "3", "Fastpay" },
                { "20", "DCC交易" }
            };

            TradeState = new Dictionary<string, string>() 
            {
                { "1", "等待买家支付" },
                { "2", "支付成功" },
                { "7", "转入退款" },
                { "99", "交易关闭" }
            };

            PriceCurType = new Dictionary<string, string>() 
            {
                { "344", "HKD" },
                { "156", "CUY" },
                { "392", "JPY" },
                { "840", "USD" }
            };

            PriceFlowDirection = new Dictionary<string, string>() 
            {
                { "1", "入" },
                { "2", "出" },
                { "3", "冻结" },
                { "4", "解冻" }
            };

            CardType = new Dictionary<string, string>() 
            {
                { "1", "MasterCard" },
                { "2", "Visa" },
                { "3", "JCB" },
                { "4", "AE" },
                { "5", "DinnerCard" }
            };

            ListState = new Dictionary<string, string>() 
            {
                { "0", "成功" },
                { "1", "提现中" },
                { "2", "提现失败" },
            };
            DeleteFlag = new Dictionary<string, string>() 
            {
                { "0", "正常" },
                { "1", "用户手工删除" },
            };
        }

        /// <summary>
        /// 查询交易单或退款单
        /// </summary>
        /// <param name="uin">财富通账号</param>
        /// <param name="list_type">交易单:101 退款单:102</param>
        /// <param name="limit">页大小</param>
        /// <param name="offset">跳过行</param>
        /// <returns></returns>
        public DataSet QueryFCTradeBillsAndRefund(string uin, short list_type, int limit, int offset)
        {
            var dic = dal.QueryFCTradeInfoByUin(uin, list_type, limit, offset);
            if (dic != null)
            {
                var ds = dal.QueryFCTradeBillsAndRefund(dic, list_type, uin);
                if (ds != null && ds.Tables.Count > 0)
                {
                    var dt = ds.Tables[0];
                    FenToYuan(dt, "_str", "bank_paynum", "price");
                    CodeToString(dt, TradeState, "trade_state");
                    CodeToString(dt, TradeType, "trade_type");
                    CodeToString(dt, PriceCurType, "price_curtype");
                    CodeToString(dt, ListState, "list_state");
                }
                return ds;
            }
            return null;
        }

        /// <summary>
        /// 查询绑卡记录
        /// </summary>
        /// <param name="uin">财富通账号</param>
        /// <returns></returns>
        public DataSet QueryFCBindCardRecord(string uin)
        {
            var ds = dal.QueryFCBindCardRecord(uin);
            if (ds != null && ds.Tables.Count > 0)
            {
                var dt = ds.Tables[0];
                dt.Columns.Add("card_owner_name");
                foreach (DataRow item in dt.Rows)
                {
                    item["card_owner_name"] = (item["bill_first_name"] as string) + (item["bill_last_name"] as string);         
                } 
                CodeToString(dt, CardType, "card_type");
            }
            return ds;
        }

        /// <summary>
        /// 查询资金流水
        /// </summary>
        /// <param name="uin">财付通账号</param>
        /// <param name="cur_type">币种类型 港币:344</param>
        /// <param name="acc_type">账户类型 普通账户:1 , 微信红包账户:2</param>
        /// <param name="limit">页大小</param>
        /// <param name="offset">跳过行</param>
        /// <returns></returns>
        public DataSet QueryFCFlow(string uin, string cur_type, string acc_type, int limit, int offset)
        {
            var client_ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            var dic = dal.QueryFCFlowInfoByUin(uin, cur_type, acc_type, client_ip);
            if (dic != null && dic.ContainsKey("acno"))
            {
                var acno = dic["acno"];
                var dt = dal.QueryFCFlow(acno, limit, offset);
                if (dt != null)
                {
                    FenToYuan(dt, "_str", "balance", "paynum");
                    CodeToString(dt, PriceFlowDirection, "type");
                    CodeToString(dt, PriceCurType, "curtype");
                    var ds = new DataSet();
                    ds.Tables.Add(dt);
                    return ds;
                }
            }
            return null;
        }



        /// <summary>
        /// 查询资金流水查询全部
        /// </summary>
        /// <param name="uin">财付通账号</param>
        /// <param name="cur_type">币种类型 港币:344</param>
        /// <param name="acc_type">账户类型 普通账户:1 , 微信红包账户:2</param>
        /// <returns></returns>
        public DataSet QueryFCFlowAll(string uin, string cur_type, string acc_type)
        {
            var client_ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            var dic = dal.QueryFCFlowInfoByUin(uin, cur_type, acc_type, client_ip);
            if (dic != null && dic.ContainsKey("acno"))
            {
                var acno = dic["acno"];
                int limit = 20;
                DataTable dts = null;
                for (int i = 0; i < 100 * 20; i += 20)  //最多进行100次访问
                {
                    var dt = dal.QueryFCFlow(acno, limit, i);
                    if (dt == null)
                    {
                        break;
                    }
                    MergeDataTable(ref dts, dt);
                }
                if (dts != null)
                {
                    FenToYuan(dts, "_str", "balance", "paynum");
                    CodeToString(dts, PriceFlowDirection, "type");
                    CodeToString(dts, PriceCurType, "curtype");
                    var ds = new DataSet();
                    ds.Tables.Add(dts);
                    return ds;
                }
            }
            return null;
        }

        /// <summary>
        /// 查询全部交易单或退款单
        /// </summary>
        /// <param name="uin">财富通账号</param>
        /// <param name="list_type">交易单:101 退款单:102</param>
        /// <returns></returns>
        public DataSet QueryFCTradeBillsAndRefundAll(string uin, short list_type)
        {
            #region 也许下次需求要改
            //var limit = 20;
            //Dictionary<string, Dictionary<string, string>> dics = new Dictionary<string, Dictionary<string, string>>();
            //for (int i = 0; i < 20 * 100; i += 20)
            //{
            //    var dic = dal.QueryFCTradeInfoByUin(uin, list_type, limit, i);
            //    if (dic == null)
            //    {
            //        break;
            //    }
            //    int j = 0;
            //    foreach (var item in dic)
            //    {
            //        dics.Add((++j * (i+1)).ToString(), item.Value);
            //    }
            //}
            //var ds = dal.QueryFCTradeBillsAndRefund(dics, list_type, uin);
            //if (ds != null && ds.Tables.Count > 0)
            //{
            //    var dt = ds.Tables[0];
            //    FenToYuan(dt, "_str", "bank_paynum", "price");
            //    CodeToString(dt, TradeState, "trade_state");
            //    CodeToString(dt, TradeType, "trade_type");
            //    CodeToString(dt, PriceCurType, "price_curtype");
            //    CodeToString(dt, ListState, "list_state");
            //}
            //return ds; 
            #endregion
            var limit = 20;
            Dictionary<string, Dictionary<string, string>> dics = new Dictionary<string, Dictionary<string, string>>();
            for (int i = 0; i < 20 * 100; i += 20)
            {
                var dic = dal.QueryFCTradeInfoByUin(uin, list_type, limit, i);
                if (dic == null)
                {
                    break;
                }
                int j = 0;
                foreach (var item in dic)
                {
                    dics.Add((++j * (i+1)).ToString(), item.Value);
                }
            }
            var dt = dal.DictionaryToDataTable(dics);
            if (dt != null)
            {
                FenToYuan(dt, "_str","total_fee");
                CodeToString(dt, ListState, "list_state");
                CodeToString(dt, DeleteFlag, "delete_flag");
                CodeToString(dt, PriceCurType, "cur_type");
                var ds = new DataSet();
                ds.Tables.Add(dt);
                return ds;
            }
            return null;
        }





        /// <summary>
        /// 把表中的分转成元
        /// </summary>
        /// <param name="dt">表</param>
        /// <param name="suffix">加后缀</param>
        /// <param name="fields">要转换的字段</param>
        private void FenToYuan(DataTable dt, string suffix, params string[] fields)
        {
            var column = fields.Select(u => new DataColumn(u + suffix)).ToArray();
            dt.Columns.AddRange(column);
            foreach (DataRow row in dt.Rows)
            {
                foreach (var field in fields)
                {
                    var fen = row[field] as string;
                    var yuan = MoneyTransfer.FenToYuan(fen);
                    row[field + suffix] = yuan;
                }
            }
        }

        /// <summary>
        /// 把表中的编码转成字符
        /// </summary>
        /// <param name="dt">表</param>
        /// <param name="dic">转换对应字典</param>
        /// <param name="field">字段</param>
        /// <param name="suffix">转换后字段加入后缀</param>
        /// <param name="defaultValue">如果出现未知code的显示</param>
        private void CodeToString(DataTable dt, IDictionary dic, string field, string suffix = "_str", string defaultValue = "未知")
        {
            dt.Columns.Add(field + suffix);
            foreach (DataRow row in dt.Rows)
            {
                var key = row[field];
                if (key != null)
                {
                    if (dic.Contains(key))
                    {
                        row[field + suffix] = dic[key];
                        continue;
                    }
                }
                row[field + suffix] = defaultValue;
            }
        }

        /// <summary>
        /// 把一个表合并到第一个表中
        /// </summary>
        /// <param name="result"></param>
        /// <param name="dt"></param>
        private void MergeDataTable(ref DataTable result, DataTable dt)
        {
            if (dt != null)
            {
                if (result != null)
                    result.Merge(dt);
                else
                    result = dt;
            }
        }
    }
}
