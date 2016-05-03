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
        /// <summary>
        /// 退款登记列表查询
        /// </summary>
        /// <param name="coding">订单编码</param>
        /// <param name="orderId">财付通订单号</param>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <param name="refundType">退款类型</param>
        /// <param name="refundState">提交退款状态</param>
        /// <param name="tradeState">交易状态</param>
        /// <param name="refund_id">商户号</param> 
        /// <param name="submit_user">登记人</param>
        /// <param name="iPageStart"></param>
        /// <param name="iPageMax"></param>
        /// <returns></returns>
        public DataSet QueryRefundRegisterList(string coding, string orderId, string stime, string etime, int refundType, int refundState,
            string batchNum, string tradeState, int refund_id, string submit_user, int iPageStart, int iPageMax)
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
            if (!string.IsNullOrEmpty(batchNum) && batchNum != "0")
            {
                Sql.Append(" and Fbatch_num='" + batchNum + "'");
            }
            if (!string.IsNullOrEmpty(tradeState) && tradeState != "0")
            {
                Sql.Append(" and Ftrade_state='" + tradeState + "'");
            }
            if (refund_id > 0)
                Sql.Append(" and LEFT(Forder_id,10) = " + refund_id);

            if (!string.IsNullOrEmpty(submit_user))
                Sql.Append(" and Fsubmit_user = '" + submit_user + "'");

            Sql.Append(" limit " + iPageStart + "," + iPageMax);

            DataSet ds = null;
            using (var da = MySQLAccessFactory.GetMySQLAccess("RefundRegister"))
            {
                da.OpenConn();

                ds = da.dsGetTotalData(Sql.ToString());
            }

            return ds;
        }

        /// <summary>
        /// 获取提交财务退款数据
        /// "交易状态"(支付成功/等待卖家发货)
        /// "提交退款"(未提交、失效)
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <param name="iPageStart"></param>
        /// <param name="iPageMax"></param>
        /// <returns></returns>
        public DataSet GetExportRefundData(string stime, string etime, int iPageStart, int iPageMax)
        {
            DataSet ds = null;

            string strSql = string.Format(@"
            select * from  c2c_fmdb.t_refund_info 
                where Fcreate_time>='{0}' and Fcreate_time<='{1}' and (Fsubmit_refund=2 or Fsubmit_refund=3) and Ftrade_state=2 
            limit {2},{3}",
            stime, etime, iPageStart, iPageMax);

            using (var da = MySQLAccessFactory.GetMySQLAccess("RefundRegister"))
            {
                da.OpenConn();

                ds = da.dsGetTotalData(strSql);
            }
            return ds;
        }
        /// <summary>
        /// 获取总数
        /// "交易状态"(支付成功/等待卖家发货)
        /// "提交退款"(未提交、失效)
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <returns></returns>
        public int GetExportRefundCount(string stime, string etime)
        {
            //Fsubmit_refund：提交退款状态 为 2、3
            //Ftrade_state：交易状态 为 2
            string strSql = string.Format(@"
            select count(*) from  c2c_fmdb.t_refund_info 
                where Fcreate_time>='{0}' and Fcreate_time<='{1}' and (Fsubmit_refund=2 or Fsubmit_refund=3) and Ftrade_state=2", 
            stime, etime);
            
            string ret = null;
            using (var da = MySQLAccessFactory.GetMySQLAccess("RefundRegister"))
            {
                da.OpenConn();

                ret = da.GetOneResult(strSql);
            }
            int count = 0;
            if (!string.IsNullOrEmpty(ret))
            {
                count = Int32.Parse(ret);
            }

            return count;
        }

        public int QueryRefundRegisterCount(string coding, string orderId, string stime, string etime, int refundType, int refundState,
            string batchNum, string tradeState, int refund_id, string submit_user)
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
            if (!string.IsNullOrEmpty(batchNum) && batchNum != "0")
            {
                Sql.Append(" and Fbatch_num='" + batchNum + "'");
            }
            if (!string.IsNullOrEmpty(tradeState) && tradeState != "0")
            {
                Sql.Append(" and Ftrade_state='" + tradeState + "'");
            }
            if (refund_id > 0)
                Sql.Append(" and LEFT(Forder_id,10) = " + refund_id);

            if (!string.IsNullOrEmpty(submit_user))
                Sql.Append(" and Fsubmit_user = '" + submit_user + "'");

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
