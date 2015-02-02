using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CFT.CSOMS.DAL.Infrastructure;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace CFT.CSOMS.DAL.RefundModule
{
    public class RefundRegisterData
    {
        //退款登记列表查询
        public DataSet QueryRefundRegisterList(string coding, string orderId, string stime, string etime, int refundType, int refundState, string tradeState, int iPageStart, int iPageMax) 
        {
            StringBuilder Sql = new StringBuilder("select * from  c2c_fmdb.t_refund_info where 1=1 ");
            
            if (!string.IsNullOrEmpty(coding))
            {
                Sql.Append(" and Fcoding='" + coding + "'");
            }
            if (!string.IsNullOrEmpty(orderId))
            {
                Sql.Append(" and Forder_id='" + orderId + "'");
            }
            if (!string.IsNullOrEmpty(stime))
            {
                Sql.Append(" and Fcreate_time>='" + stime + "'");
            }
            if (!string.IsNullOrEmpty(etime))
            {
                Sql.Append(" and Fcreate_time<='" + etime + "'");
            }
            if (refundType != null && refundType != 0)
            {
                if (refundType == 10)
                {
                    Sql.Append(" and Frefund_type IN(1,2,3,4,5,10) ");
                }
                else
                {
                    Sql.Append(" and Frefund_type=" + refundType);
                }

            }
            if (refundState != null && refundState != 0)
            {
                Sql.Append(" and Fsubmit_refund=" + refundState);
            }
            if (!string.IsNullOrEmpty(tradeState) && tradeState != "0")
            {
                Sql.Append(" and Ftrade_state='" + tradeState + "'");
            }
            Sql.Append(" limit " + iPageStart + "," + iPageMax);

            DataSet ds = null;
            using (var da = MySQLAccessFactory.GetMySQLAccess("RefundRegister"))
            {
                da.OpenConn();

                ds = da.dsGetTotalData(Sql.ToString());
            }

            return ds;
        }

        public int QueryRefundRegisterCount(string coding, string orderId, string stime, string etime, int refundType, int refundState, string tradeState)
        {
            StringBuilder Sql = new StringBuilder("select count(*) from  c2c_fmdb.t_refund_info where 1=1 ");

            if (!string.IsNullOrEmpty(coding))
            {
                Sql.Append(" and Fcoding='" + coding + "'");
            }
            if (!string.IsNullOrEmpty(orderId))
            {
                Sql.Append(" and Forder_id='" + orderId + "'");
            }
            if (!string.IsNullOrEmpty(stime))
            {
                Sql.Append(" and Fcreate_time>='" + stime + "'");
            }
            if (!string.IsNullOrEmpty(etime))
            {
                Sql.Append(" and Fcreate_time<='" + etime + "'");
            }
            if (refundType != null && refundType != 0)
            {
                if (refundType == 10)
                {
                    Sql.Append(" and Frefund_type IN(1,2,3,4,5,10) ");
                }
                else
                {
                    Sql.Append(" and Frefund_type=" + refundType);
                }

            }
            if (refundState != null && refundState != 0)
            {
                Sql.Append(" and Fsubmit_refund=" + refundState); 
            }
            if (!string.IsNullOrEmpty(tradeState) && tradeState != "0")
            {
                Sql.Append(" and Ftrade_state='" + tradeState + "'");
            }

            string ret = null;
            using (var da = MySQLAccessFactory.GetMySQLAccess("RefundRegister"))
            {
                da.OpenConn();

                ret = da.GetOneResult(Sql.ToString());
            }
            int count = 0;
            if (!string.IsNullOrEmpty(ret)) 
            {
                count = Int32.Parse(ret);
            }

            return count;
        }

        public string QueryTradeAmount(string listid) 
        {
            string msg = "";
            string amount = "0";
            string striceWhere = "listid=" + listid;
            DataTable dt_ice = CommQuery.GetTableFromICE(striceWhere, "query_order_service", out msg);
            if (msg != "")
            {

            }
            if (dt_ice != null && dt_ice.Rows.Count > 0) 
            {
                amount = dt_ice.Rows[0]["Fpaynum"].ToString();
            }

            return amount;
        }
    }
}
