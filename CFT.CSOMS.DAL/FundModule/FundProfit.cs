using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CFT.CSOMS.DAL.FundModule
{
    using CFT.CSOMS.DAL.Infrastructure;

    public class FundProfit
    {
        public DataTable QueryProfitRecord(string tradeId, string beginDateStr, string endDateStr, int currencyType = -1, string spId = "", int limStart = 0, int limCount = 10)
        {
            if (string.IsNullOrEmpty(tradeId))
                throw new ArgumentNullException("tradeId");

            using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            {
                var table_name = string.Format("fund_db_{0}.t_fund_profit_record_{1}", tradeId.Substring(tradeId.Length - 2), tradeId.Substring(tradeId.Length - 3, 1));

                var sqlBuilder = new StringBuilder(string.Format("select * from {0} where Ftrade_id='{1}'", table_name, tradeId));

                if (!string.IsNullOrEmpty(beginDateStr))
                    sqlBuilder.AppendFormat(" and Fday >='{0}'", beginDateStr);

                if (!string.IsNullOrEmpty(endDateStr))
                    sqlBuilder.AppendFormat(" and Fday <='{0}'", endDateStr);

                if (!string.IsNullOrEmpty(spId))
                    sqlBuilder.AppendFormat(" and Fspid = '{0}'", spId);

                if(currencyType != -1)
                    sqlBuilder.AppendFormat(" and Fcurtype = {0}", currencyType);

                sqlBuilder.AppendFormat(" order by Fday desc limit " + limStart + "," + limCount);

                da.OpenConn();
                DataSet ds = da.dsGetTotalData(sqlBuilder.ToString());

                return ds.Tables[0];
            }
        }

        //用户基金账户绑定记录，只能查到有收益的
        public DataTable QueryProfitStatistic(string tradeId, int currencyType = -1, string spId = "")
        {

            if (string.IsNullOrEmpty(tradeId))
                throw new ArgumentNullException("tradeId");

            using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            {
                var table_name = string.Format("fund_db_{0}.t_fund_profit_{1}", tradeId.Substring(tradeId.Length - 2), tradeId.Substring(tradeId.Length - 3, 1));

                
                var sqlBuilder = new StringBuilder(string.Format("select * from {0} where Ftrade_id='{1}'", table_name, tradeId));

                if (!string.IsNullOrEmpty(spId))
                    sqlBuilder.AppendFormat(" and Fspid = '{0}'", spId);

                if (currencyType != -1)
                    sqlBuilder.AppendFormat(" and Fcurtype = {0}", currencyType);
                
                da.OpenConn();
                DataSet ds = da.dsGetTotalData(sqlBuilder.ToString());

                return ds.Tables[0];
            }
        }

        //基金账户绑定记录表，查到有份额的所有基金
        public DataTable QueryFundBind(string tradeId)
        {

            if (string.IsNullOrEmpty(tradeId))
                throw new ArgumentNullException("tradeId");

            using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            {
                var table_name = string.Format("fund_db_{0}.t_fund_bind_sp_{1}", tradeId.Substring(tradeId.Length - 2), tradeId.Substring(tradeId.Length - 3, 1));


                var sqlBuilder = new StringBuilder(string.Format("select * from {0} where Ftrade_id='{1}'", table_name, tradeId));

                da.OpenConn();
                DataSet ds = da.dsGetTotalData(sqlBuilder.ToString());

                return ds.Tables[0];
            }
        }
    }
}
