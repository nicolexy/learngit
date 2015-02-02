using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CFT.CSOMS.DAL.CFTAccount;

namespace CFT.CSOMS.DAL.FundModule
{
    using CFT.CSOMS.DAL.Infrastructure;

    public class SafeCard
    {
        //获取理财通安全卡更换日志
        public DataTable GetFundTradeLog(string qqid, int istr, int imax)
        {
            using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            {
                var sqlBuilder = new StringBuilder(string.Format("select * from fund_db.t_fund_pay_card_log where Fqqid='{0}'", qqid));

                sqlBuilder.AppendFormat(" order by Fmodify_time desc limit " + istr + "," + imax);
                
                da.OpenConn();
                DataSet ds = da.dsGetTotalData(sqlBuilder.ToString());

                return ds.Tables[0];
            }
        }
        public DataTable GetPayCardInfo(string qqid)
        {
            if (qqid == null || qqid == "")
            {
                throw new ArgumentNullException("财付通账号不能为空！");
            }

            string sql = "select * from fund_db.t_fund_pay_card where Fqqid='" + qqid + "'";

            using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            {
                da.OpenConn();
                var dt = da.GetTable(sql);
                dt.TableName = "PayCardInfo";
                return dt;
            }

        }
    }
}
