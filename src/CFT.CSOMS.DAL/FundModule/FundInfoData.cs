using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace CFT.CSOMS.DAL.FundModule
{
    using TENCENT.OSS.CFT.KF.DataAccess;
    using CFT.CSOMS.DAL.Infrastructure;

    public class FundInfoData
    {
        public DataTable QueryAllFundInfo()
        {
            using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            {
                da.OpenConn();
                string Sql = " select Fsp_name,Fspid,Ffund_name,Ffund_code,Fcurtype from fund_db.t_fund_sp_config";
                DataSet ds = da.dsGetTotalData(Sql);

                return ds.Tables[0];
            }
        }

        //通过商户号查询基金公司信息
        public DataTable QueryFundInfoBySpid(string spid)
        {
            if (string.IsNullOrEmpty(spid))
                throw new ArgumentNullException("spid");
            using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            {
                da.OpenConn();
                string Sql = " select Fsp_name,Fspid,Ffund_name,Ffund_code,Fcurtype,Fclose_flag,Ftransfer_flag,Fbuy_valid from fund_db.t_fund_sp_config where Fspid='" + spid + "'";
                DataSet ds = da.dsGetTotalData(Sql);

                return ds.Tables[0];
            }
        }
       
        //判断是否重新申购基金
        public bool QueryIfAnewBoughtFund(string listid, DateTime time)
        {
            using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            {
                da.OpenConn();
                string Sql = " select count(1) from fund_db.t_fetch_list_" + time.ToString("yyyyMM") + " where Ffund_apply_id= '" + listid + "'";
                if (da.GetOneResult(Sql) == "1")  //存在表示重新申购的基金
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //通过商户号+订单号（若为提现单则是后18位）查询基金交易单
        public DataTable QueryTradeFundInfo(string spid, string listid)
        {
            if (string.IsNullOrEmpty(spid.Trim()))
                throw new ArgumentNullException("spid");
            if (string.IsNullOrEmpty(listid))
                throw new ArgumentNullException("listid");
            using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            {
                da.OpenConn();
                var table_name = string.Format("fund_db_{0}.t_trade_fund_{1}", listid.Substring(listid.Length - 2), listid.Substring(listid.Length - 3, 1));
                string Sql = string.Format(" select Fspid,Ffund_name,Ffund_code,Fpur_type,Fcharge_fee from {0} where Fspid='{1}' and Flistid='{2}'", table_name, spid, listid);
                DataSet ds = da.dsGetTotalData(Sql);

                return ds.Tables[0];
            }
        }

        //查询定期产品用户交易记录
        public DataTable QueryCloseFundRollList(string tradeId, string fundCode, string beginDateStr, string endDateStr, int currentPageIndex = 0, int pageSize = 10)
        {
            if (string.IsNullOrEmpty(tradeId))
                throw new ArgumentNullException("tradeId");
            if (string.IsNullOrEmpty(fundCode))
                throw new ArgumentNullException("fundCode");
            using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            {
                da.OpenConn();
                var table_name = string.Format("fund_db_{0}.t_fund_close_trans_{1}", tradeId.Substring(tradeId.Length - 2), tradeId.Substring(tradeId.Length - 3, 1));
                string Sql = string.Format(" select * from {0} where Ftrade_id='{1}' and Ffund_code='{2}' ", table_name, tradeId, fundCode);
                Sql = string.Format(Sql + " and Ftrans_date between '{0}' and '{1}' ", beginDateStr, endDateStr);
                Sql = string.Format(Sql + " limit {0},{1} ", currentPageIndex, pageSize);
                DataSet ds = da.dsGetTotalData(Sql);
                return ds.Tables[0];
            }
        }

    }
}
