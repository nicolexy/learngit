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

namespace CFT.CSOMS.DAL.InternetBank
{
    public  class InternetBankData
    {

        public void AddRefundInfo( string FOrderId ,int FRefund_type,string FSam_no ,string FRecycle_user,string FSubmit_user , string FRefund_amount)
        {
            string msg;
            var da = MySQLAccessFactory.GetMySQLAccess("DataSource_ht");
            try
            {
                string FCoding = "";
                string FAmount = ""; //交易金额
                int FTrade_state = 2;
                string FBuy_acc = "";
                string FTrade_desc = "";//交易说明
                string FMemo = "";//备注

                int FRefund_state = 1; //退款状态
                int FSubmit_refund = 3; //提交退款状态

                da.OpenConn();
                string strSql = "select " + GeRefundInfoFields() + " from c2c_fmdb.t_refund_info where Forder_id='" + FOrderId + "'";
                DataTable dt = da.GetTable(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //msg = "添加失败:财付通订单号已存在" + FOrderId;
                    throw new LogicException("财付通订单号已存在:" + FOrderId);
                }

                string striceWhere = "listid=" + FOrderId;
                DataTable dt_ice = CommQuery.GetTableFromICE(striceWhere, "query_order_service", out msg);
                if (dt_ice != null && dt_ice.Rows.Count > 0)
                {
                    object obj = dt_ice.Rows[0]["Fcoding"];
                    if (obj != null) FCoding = obj.ToString().Trim();
                    obj = dt_ice.Rows[0]["Fpaynum"];
                    if (obj != null) FAmount = obj.ToString().Trim();
                    obj = dt_ice.Rows[0]["Ftrade_state"];
                    if (obj != null)
                    {
                        FTrade_state = int.Parse(obj.ToString().Trim());
                        if (FTrade_state == 2)
                        {
                            FSubmit_refund = 2;
                        }
                    }
                    obj = dt_ice.Rows[0]["Fbuyid"];
                    if (obj != null) FBuy_acc = obj.ToString().Trim();
                    obj = dt_ice.Rows[0]["Fmemo"];
                    if (obj != null) FTrade_desc = obj.ToString().Trim();
                }
                else
                {
                    //msg = "通用查询订单号不存在：" + FOrderId;
                    throw new LogicException("订单号不存在：" + FOrderId + msg);
                }
                //判断退款金额<=订单金额
                int oAmount = Convert.ToInt32(FAmount);
                int rAmount = 0;
                if (!string.IsNullOrEmpty(FRefund_amount))
                {
                    rAmount = Convert.ToInt32(FRefund_amount);
                }
                if (rAmount <= oAmount)
                {
                    //退款金额<=订单金额
                    strSql = "insert into c2c_fmdb.t_refund_info(Forder_id,Fcoding,Famount,Ftrade_state,Fbuy_acc,Ftrade_desc,Frefund_type,Frefund_state,Fmemo,Fsubmit_user,Frecycle_user,Fsam_no,Fsubmit_refund,Frefund_amount,Fcreate_time,Fmodify_time) values('{0}','{1}','{2}',{3},'{4}','{5}',{6},{7},'{8}','{9}','{10}','{11}',{12},{13},now(),now())";
                    strSql = String.Format(strSql, FOrderId, FCoding, FAmount, FTrade_state, FBuy_acc, FTrade_desc, FRefund_type, FRefund_state, FMemo, FSubmit_user, FRecycle_user, FSam_no, FSubmit_refund, FRefund_amount);

                    da.ExecSqlNum(strSql);
                }
                else
                {
                    throw new LogicException("退款金额" + rAmount + "大于订单金额" + oAmount);
                }
            }
            catch (LogicException err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        private static string GeRefundInfoFields()
        {
            return " Fid,Forder_id,Fcoding,Famount,Ftrade_state,Fbuy_acc,Ftrade_desc,Frefund_type,Frefund_state,Fmemo,Fcreate_time,Fmodify_time,Fsubmit_user,Frecycle_user,Fsam_no,Fsubmit_refund,Frefund_amount ";
        }
    }
}
