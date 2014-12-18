using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CFT.CSOMS.DAL.FundModule
{
    using CFT.CSOMS.DAL.Infrastructure;

    public class LCTBalance
    {

        public DataTable QueryLCTBalanceRollList(string tradeId, int start, int max)
        {
            if (string.IsNullOrEmpty(tradeId.Trim()))
                throw new ArgumentNullException("tradeId");
            using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            {
                da.OpenConn();
                var table_name = string.Format("fund_db_{0}.t_fund_balance_order_{1}", tradeId.Substring(tradeId.Length - 2), tradeId.Substring(tradeId.Length - 3, 1));
                string Sql = string.Format(" select Flistid,Fcreate_time,Facc_time,Ftype,Fchannel_id,Fstate,Ftotal_fee,Fmemo from {0} where Ftrade_id='{1}' order by Fcreate_time desc limit {2},{3} ", table_name, tradeId, start, max);
                DataSet ds = da.dsGetTotalData(Sql);

                return ds.Tables[0];
            }
        }
    }
}
