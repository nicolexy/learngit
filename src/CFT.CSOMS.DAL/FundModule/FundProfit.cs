using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SunLibraryEX;

namespace CFT.CSOMS.DAL.FundModule
{
    using CFT.CSOMS.DAL.Infrastructure;

    public class FundProfit
    {
        //查询指数型基金（目前只有易方达沪深300基金）的每日单位净值和日涨跌字段
        //2016-1-14 v_yqyqguo 指数型基金查询最近有值记录
        public DataTable QueryFundProfitRate(string spid, string fund_code)
        {
            //if (string.IsNullOrEmpty(spid))
            //    throw new ArgumentNullException("spid");
            //if (string.IsNullOrEmpty(fund_code))
            //    throw new ArgumentNullException("fund_code");
            //if (string.IsNullOrEmpty(date))
            //    throw new ArgumentNullException("date");
            //using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            //{
            //    da.OpenConn();
            //    string Sql = " select F1day_profit_rate,F7day_profit_rate from fund_db.t_fund_profit_rate where Fspid='" + spid + "' and Ffund_code='" + fund_code + "' and Fdate='" + date + "'";
            //    DataSet ds = da.dsGetTotalData(Sql);

            //    return ds.Tables[0];
            //}


            DataTable dt = null;
            var serverIp = System.Configuration.ConfigurationManager.AppSettings["FundRateIP"].ToString();
            var serverPort = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["FundRatePort"].ToString());
            string requestText = "reqid=667&flag=2&offset=0&limit=1&fields=spid:{0}|fund_code:{1}";
            requestText = string.Format(requestText, spid, fund_code);
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        public DataTable QueryProfitRecord(string tradeId, string beginDateStr, string endDateStr, int currencyType = -1, string spId = "", int limStart = 0, int limCount = 10)
        {
            //if (string.IsNullOrEmpty(tradeId))
            //    throw new ArgumentNullException("tradeId");

            //using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            //{
            //    var table_name = string.Format("fund_db_{0}.t_fund_profit_record_{1}", tradeId.Substring(tradeId.Length - 2), tradeId.Substring(tradeId.Length - 3, 1));

            //    var sqlBuilder = new StringBuilder(string.Format("select * from {0} where Ftrade_id='{1}'", table_name, tradeId));

            //    if (!string.IsNullOrEmpty(beginDateStr))
            //        sqlBuilder.AppendFormat(" and Fday >='{0}'", beginDateStr);

            //    if (!string.IsNullOrEmpty(endDateStr))
            //        sqlBuilder.AppendFormat(" and Fday <='{0}'", endDateStr);

            //    if (!string.IsNullOrEmpty(spId))
            //        sqlBuilder.AppendFormat(" and Fspid = '{0}'", spId);

            //    if (currencyType != -1)
            //        sqlBuilder.AppendFormat(" and Fcurtype = {0}", currencyType);

            //    sqlBuilder.AppendFormat(" order by Fday desc limit " + limStart + "," + limCount);

            //    da.OpenConn();
            //    DataSet ds = da.dsGetTotalData(sqlBuilder.ToString());

            //  return ds.Tables[0];
            //}

            DataTable dt = null;
            var serverIp = System.Configuration.ConfigurationManager.AppSettings["FundRateIP"].ToString();
            var serverPort = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["FundRatePort"].ToString());
            string requestText = "reqid=623&flag=2&offset={0}&limit={1}&fields=trade_id:{2}|begin_time:{3}|end_time:{4}|spid:{5}";
            requestText = string.Format(requestText, limStart, limCount, tradeId, beginDateStr, endDateStr, spId);
            DataSet ds1 = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                dt = ds1.Tables[0];
            }
            return dt;
        }

        //用户基金账户绑定记录，只能查到有收益的
        public DataTable QueryProfitStatistic(string tradeId, int currencyType = -1, string spId = "")
        {

            //if (string.IsNullOrEmpty(tradeId))
            //    throw new ArgumentNullException("tradeId");

            //using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            //{
            //    var table_name = string.Format("fund_db_{0}.t_fund_profit_{1}", tradeId.Substring(tradeId.Length - 2), tradeId.Substring(tradeId.Length - 3, 1));

                
            //    var sqlBuilder = new StringBuilder(string.Format("select * from {0} where Ftrade_id='{1}'", table_name, tradeId));

            //    if (!string.IsNullOrEmpty(spId))
            //        sqlBuilder.AppendFormat(" and Fspid = '{0}'", spId);

            //    if (currencyType != -1)
            //        sqlBuilder.AppendFormat(" and Fcurtype = {0}", currencyType);
                
            //    da.OpenConn();
            //    DataSet ds = da.dsGetTotalData(sqlBuilder.ToString());

            //    return ds.Tables[0];
            //}


            DataTable dt = null;
            var serverIp = System.Configuration.ConfigurationManager.AppSettings["FundRateIP"].ToString();
            var serverPort = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["FundRatePort"].ToString());
            string requestText = "reqid=678&flag=2&offset=0&limit=10&fields=trade_id:" + tradeId;
            if (!string.IsNullOrEmpty(spId))
            {
                requestText += "|spid:" + spId;
            }
            if (currencyType != -1)
            {
                requestText += "|curtype:" + currencyType;
            }

            requestText = string.Format(requestText, tradeId, "2010-01-10", "2015-10-01", spId);
            DataSet ds1 = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                dt = ds1.Tables[0];
            }
            return dt;
        }

        //基金账户绑定记录表，查到有份额的所有基金
        public DataTable QueryFundBind(string tradeId)
        {

            //if (string.IsNullOrEmpty(tradeId))
            //    throw new ArgumentNullException("tradeId");

            //using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            //{
            //    var table_name = string.Format("fund_db_{0}.t_fund_bind_sp_{1}", tradeId.Substring(tradeId.Length - 2), tradeId.Substring(tradeId.Length - 3, 1));


            //    var sqlBuilder = new StringBuilder(string.Format("select * from {0} where Ftrade_id='{1}'", table_name, tradeId));

            //    da.OpenConn();
            //    DataSet ds = da.dsGetTotalData(sqlBuilder.ToString());

            //    return ds.Tables[0];
            //}

            if (string.IsNullOrEmpty(tradeId))
                throw new ArgumentNullException("tradeId");
            DataTable dt = null;
            var serverIp = System.Configuration.ConfigurationManager.AppSettings["FundRateIP"].ToString();
            var serverPort = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["FundRatePort"].ToString());
            string requestText = "reqid=612&flag=2&offset=0&limit=20&fields=trade_id:{0}";
            requestText = string.Format(requestText, tradeId);
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
          
        }

        /// <summary>
        /// 定期修改到期策略
        /// </summary>
        /// <param name="Trade_id">基金交易账户对应id</param>
        /// <param name="Fund_code">基金代码</param>
        /// <param name="Close_listid">定期产品用户交易记录表的自增主键</param>
        /// <param name="user_end_type">用户指定的到期申购/赎回策略</param>
        /// <param name="end_sell_type">到期操作</param>
        /// <param name="client_ip">操作ip</param>
        /// <returns></returns>
        public bool AlterEndStrategy(string trade_id, string fund_code, long close_listid, int user_end_type, int end_sell_type, string client_ip)
        {
            var ip = CFT.Apollo.Common.Configuration.AppSettings.Get("AlterEndStrategy_IP", "172.27.31.177");
            var port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("AlterEndStrategy_Port", 22000);

            var watch_word = "f7d02dddf897ffef786e1c499eae41f4";
            var token = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", trade_id, fund_code, close_listid, user_end_type, end_sell_type, watch_word);
            token = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(token, "md5").ToLower();

            var req =
              "trade_id=" + trade_id +
              "&fund_code=" + fund_code +
              "&close_listid=" + close_listid +
              "&user_end_type=" + user_end_type +
              "&end_sell_type=" + end_sell_type +
              "&client_ip=" + client_ip +
              "&watch_word=" + watch_word +
              "&token=" + token;

            var relay_result = RelayAccessFactory.RelayInvoke(req, "101303", true, false, ip, port);

            var dic= relay_result.ToDictionary('&', '=');
            if (dic["result"] != "0")
            {
                throw new Exception("修改失败: " + dic["res_info"]);
            } 
            return true;       
        }
    }
}
