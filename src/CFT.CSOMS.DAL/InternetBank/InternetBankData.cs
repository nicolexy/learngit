using System;
using System.Configuration;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.Common;
using System.Data;
using System.Collections;
using System.IO;
using System.Web.Mail;

using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Xml;
using System.Text.RegularExpressions;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.Apollo.Logging;

namespace CFT.CSOMS.DAL.InternetBank
{
    public  class InternetBankData
    {

        /// <summary>
        /// 新增退款登记
        /// </summary>
        /// <param name="FOrderId">订单号</param>
        /// <param name="FRefund_type">退款类型</param>
        /// <param name="memo">备注</param>
        /// <param name="FSubmit_user">登记人</param>
        /// <param name="FRecycle_user">物品回收人</param>
        /// <param name="FSam_no">SAM工单号</param>
        /// <returns>"0":成功,其它：失败信息</returns>
        ///  public bool AddRefundInfo()
        public bool AddRefundInfo(string FOrderId, int FRefund_type, string FSam_no, string FRecycle_user, string FSubmit_user, string FRefund_amount, string memo)
        {
            int submit_refund = 3; //提交退款状态,失效
            int refund_state = 1; //退款状态，已登记
            int refund_type = 11; //退款类型，默认：发货失败
            string coding = ""; //订单编码
            string amount = "0"; //订单金额
            int trade_state = 1; //交易状态
            string buy_acc = ""; //买家账号
            string trade_desc = ""; //交易说明

            string msg = "";

            var da = MySQLAccessFactory.GetMySQLAccess("DataSource_ht");

            try
            {
                if (string.IsNullOrEmpty(FOrderId))
                {
                    throw new Exception("订单号不能为空！");
                }
                if (FRefund_type == 10 || FRefund_type == 11)
                {
                    refund_type = FRefund_type;
                }

                da.OpenConn();
                string strSql = "select * from c2c_fmdb.t_refund_info where Forder_id='" + FOrderId + "'";
                DataTable dt = da.GetTable(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    throw new Exception("订单号已存在:" + FOrderId);
                }

                string striceWhere = "listid=" + FOrderId;
                DataTable dt_ice = CommQuery.GetTableFromICE(striceWhere, "query_order_service", out msg);
                if (dt_ice != null && dt_ice.Rows.Count > 0)
                {
                    object obj = dt_ice.Rows[0]["Fcoding"];
                    if (obj != null) coding = obj.ToString().Trim();
                    obj = dt_ice.Rows[0]["Fpaynum"];
                    if (obj != null) amount = obj.ToString().Trim();
                    obj = dt_ice.Rows[0]["Ftrade_state"];
                    if (obj != null)
                    {
                        trade_state = int.Parse(obj.ToString().Trim());
                        if (trade_state == 2)
                        {
                            submit_refund = 2;
                        }
                    }
                    obj = dt_ice.Rows[0]["Fbuyid"];
                    if (obj != null) buy_acc = obj.ToString().Trim();
                    obj = dt_ice.Rows[0]["Fmemo"];
                    if (obj != null) trade_desc = obj.ToString().Trim();
                }
                else
                {
                    //msg = "通用查询订单号不存在：" + FOrderId;
                    throw new Exception("订单号不存在：" + msg);
                }
                //判断退款金额<=订单金额
                int oAmount = Convert.ToInt32(amount);
                int rAmount = 0;
                if (!string.IsNullOrEmpty(FRefund_amount))
                {
                    rAmount = Convert.ToInt32(FRefund_amount);
                }
                if (rAmount <= oAmount)
                {
                    strSql = "insert into c2c_fmdb.t_refund_info(Forder_id,Fcoding,Famount,Ftrade_state,Fbuy_acc,Ftrade_desc,Frefund_type,Frefund_state,Fmemo,Fsubmit_user,Frecycle_user,Fsam_no,Fsubmit_refund,Frefund_amount,Fcreate_time,Fmodify_time) values('{0}','{1}','{2}',{3},'{4}','{5}',{6},{7},'{8}','{9}','{10}','{11}',{12},{13},now(),now())";
                    strSql = String.Format(strSql, FOrderId, coding, amount, trade_state, buy_acc, trade_desc, refund_type, refund_state, memo, FSubmit_user, FRecycle_user, FSam_no, submit_refund, rAmount);

                    da.ExecSqlNum(strSql);
                }
                else
                {
                    throw new Exception("退款金额" + rAmount + "大于订单金额" + oAmount);
                }
            }
            catch (Exception e)
            {
                loger.err("AddRefundInfo", e.Message);
                return false;
            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
            }

            return true;
        }


        /// <summary>
        /// 添加 退款商户名单 
        /// </summary>
        /// <param name="Frefund_id">商户号码</param>
        /// <param name="Frefund_name">商户名称</param>
        /// <param name="Fcreate_by">创建人</param>
        /// <returns>-1:失败，0：成功，1：商户号码已存在</returns>
        public int AddRefundList(int Frefund_id,string Frefund_name, string Fcreate_by)
        {
            int result = -1;
            var da = MySQLAccessFactory.GetMySQLAccess("NameAbnormal");
            try
            {
                da.OpenConn();
                string strSql = string.Format(
                    "SELECT COUNT(Fid) FROM c2c_fmdb.t_refund_list WHERE Frefund_state = 0 AND Frefund_id = {0}", 
                    Frefund_id);
                if (Convert.ToInt32(da.GetOneResult(strSql)) == 0)
                {
                    strSql = string.Format(@"
                    INSERT c2c_fmdb.t_refund_list 
                      (Frefund_id, Frefund_name, Fcreate_by, Fmodify_by, Fcreate_time, Fmodify_time) 
                    VALUES 
                      ({0},'{1}','{2}','{3}',NOW(),NOW())",
                    Frefund_id, Frefund_name, Fcreate_by, Fcreate_by);

                    int flag = da.ExecSqlNum(strSql);
                    if (flag > 0)
                        result = 0;
                }
                else
                {
                    result = 1;
                }
            }
            catch (Exception e)
            {
                loger.err("AddRefundList", e.Message);
            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
            }
            return result;
        }

        /// <summary>
        /// 删除  退款商户名单 
        /// </summary>
        /// <param name="Fid">自增ID</param>
        /// <returns></returns>
        public bool DelRefundByFid(int Fid)
        {
            bool result = false;
            string strSql = string.Format("UPDATE c2c_fmdb.t_refund_list SET Frefund_state = 1 WHERE Fid = {0}", Fid);
            using (var da = MySQLAccessFactory.GetMySQLAccess("NameAbnormal"))
            {
                da.OpenConn();
                if (da.ExecSql(strSql))
                    result = true;
            }
            return result;
        }

        /// <summary>
        /// 修改 退款商户名单 
        /// </summary>
        /// <param name="Frefund_id">商户号码</param>
        /// <param name="Frefund_name">商户名称</param>
        /// <param name="Fmodify_by">修改人</param>
        /// <param name="Fid">自增ID</param>
        /// <returns>-1:失败，0：成功，1：商户号码已存在</returns>
        public int EditRefundByFid(int Frefund_id,int Old_Frefund_id,string Frefund_name, string Fmodify_by,int Fid)
        {
            int result = -1;
            string strSql = string.Format(
                    "SELECT COUNT(Fid) FROM c2c_fmdb.t_refund_list WHERE Frefund_state = 0 AND Frefund_id = {0}",
                    Frefund_id);
            using (var da = MySQLAccessFactory.GetMySQLAccess("NameAbnormal"))
            {
                da.OpenConn();
                int temp_flag = 0;
                if (Frefund_id != Old_Frefund_id)
                {
                    temp_flag = Convert.ToInt32(da.GetOneResult(strSql));
                }
                if (temp_flag == 0)
                {
                    strSql = string.Format(@"
                        UPDATE c2c_fmdb.t_refund_list 
                            SET Frefund_id = {0}, Frefund_name = '{1}', Fmodify_by = '{2}', Fmodify_time = now() 
                        WHERE Fid = {3}",
                        Frefund_id, Frefund_name, Fmodify_by, Fid);
                    if (da.ExecSql(strSql))
                        result = 0;
                }
                else
                {
                    result = 1;
                }
            }
            return result;
        }

        /// <summary>
        /// 查询 退款商户名单 
        /// </summary>
        /// <param name="Frefund_id"></param>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <returns></returns>
        public DataSet GetRefundByFrefundId(int Frefund_id, string sTime, string eTime, int start, int max)
        {
            DataSet ds = null;
            string strLimit = string.Empty;

            string strWhere = " WHERE Frefund_state = 0 ";

            if (Frefund_id > 0)
                strWhere += " AND Frefund_id = " + Frefund_id;

            if (!string.IsNullOrEmpty(sTime) && !string.IsNullOrEmpty(eTime))
                strWhere += " AND Fcreate_time >= '" + sTime + "' AND Fcreate_time <= '" + eTime + "'";

            if (max > 0)
                strLimit = string.Format(" LIMIT {0},{1}", start, max);
           

            string strSql = string.Format(
                "SELECT * FROM c2c_fmdb.t_refund_list {0} ORDER BY Fcreate_time DESC {1}",
                strWhere, strLimit);
            using (var da = MySQLAccessFactory.GetMySQLAccess("NameAbnormal"))
            {
                da.OpenConn();
                ds = da.dsGetTotalData(strSql);
            }
            return ds;
        }

        public int GetRefundCount(int Frefund_id, string sTime, string eTime)
        {
            int result = 0;
            string strWhere = " WHERE Frefund_state = 0 ";

            if (Frefund_id > 0)
                strWhere += " AND Frefund_id = " + Frefund_id;

            if (!string.IsNullOrEmpty(sTime) && !string.IsNullOrEmpty(eTime))
                strWhere += " AND Fcreate_time >= '" + sTime + "' AND Fcreate_time <= '" + eTime + "'";

            string strSql = string.Format(
                "SELECT COUNT(Fid) FROM c2c_fmdb.t_refund_list {0}", strWhere);
            using (var da = MySQLAccessFactory.GetMySQLAccess("NameAbnormal"))
            {
                da.OpenConn();
                result = Convert.ToInt32(da.GetOneResult(strSql));
            }
            return result;
        }
    }
}
