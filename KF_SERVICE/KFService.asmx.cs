using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.KF_Service;
using System.Data;

namespace CFT.CSOMS.Service.WebService
{
    /// <summary>
    /// 客服系统WebService接口
    /// </summary>
    public class KFService : System.Web.Services.WebService {

        /// <summary>
        /// 新增退款登记 -- 数平访问
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="refundType">退款类型</param>
        /// <param name="memo">备注</param>
        /// <param name="submitUser">登记人</param>
        /// <param name="recycleUser">物品回收人</param>
        /// <param name="samNo">SAM工单号</param>
        /// <returns>"0":成功,其它：失败信息</returns>
        [WebMethod]
        public string AddRefundInfo(string orderId,int refundType,string memo,string submitUser,string recycleUser,string samNo, string refundAmount)
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

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try
            {
                if (string.IsNullOrEmpty(orderId))
                {
                    throw new Exception("订单号不能为空！");
                }
                if (refundType == 10 || refundType == 11)
                {
                    refund_type = refundType;
                }

                da.OpenConn();
                string strSql = "select * from c2c_fmdb.t_refund_info where Forder_id='" + orderId + "'";
                DataTable dt = da.GetTable(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    throw new Exception("订单号已存在:" + orderId);
                }

                string striceWhere = "listid=" + orderId;
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
                if (!string.IsNullOrEmpty(refundAmount))
                {
                    rAmount = Convert.ToInt32(refundAmount);
                }
                if (rAmount <= oAmount)
                {
                    strSql = "insert into c2c_fmdb.t_refund_info(Forder_id,Fcoding,Famount,Ftrade_state,Fbuy_acc,Ftrade_desc,Frefund_type,Frefund_state,Fmemo,Fsubmit_user,Frecycle_user,Fsam_no,Fsubmit_refund,Frefund_amount,Fcreate_time,Fmodify_time) values('{0}','{1}','{2}',{3},'{4}','{5}',{6},{7},'{8}','{9}','{10}','{11}',{12},{13},now(),now())";
                    strSql = String.Format(strSql, orderId, coding, amount, trade_state, buy_acc, trade_desc, refund_type, refund_state, memo, submitUser, recycleUser, samNo, submit_refund, rAmount);

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
                return e.Message;
            }
            finally 
            {
                if (da != null) {
                    da.Dispose();
                }
            }
            
            return "0";
        }
    }
}
