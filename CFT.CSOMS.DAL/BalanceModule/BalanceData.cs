using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.CSOMS.DAL.CFTAccount;

namespace CFT.CSOMS.DAL.BalanceModule
{
    public class BalanceData
    {
        //查询余额支付功能关闭与否
        public bool BalancePaidOrNotQuery(string uin,string ip)
        {
            string msg = "";
            string service_name = "zw_prodatt_query_service";
            string req = "";
         
            string uid = AccountData.ConvertToFuid(uin);
            if (uid == null || uid.Length < 3)
            {
                throw new Exception("内部ID不正确！");
            }
            try
            {
                DataSet ds = null;
                req = "uid=" + uid + "&client_ip=" + ip + "&memo=KFWeb&op_type=1";

                ds = CommQuery.GetOneTableFromICE(req, "", service_name, false, out msg);
                if (msg != "")
                    throw new Exception("Service处理失败！" + msg);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    string Fbalance_state = dt.Rows[0]["Fbalance_state"].ToString();
                    if (Fbalance_state == "1")//1 不可使用余额支付 。2可以使用余额支付 
                        return false;
                    else if (Fbalance_state == "2")
                        return true;
                    else
                        throw new Exception("Service处理失败！不能判断是否可用余额支付Fbalance_state=" + Fbalance_state);
                }
                else
                    throw new Exception("Service处理失败！判断是否可用余额支付出错");
            }
            catch (Exception e)
            {
                throw new Exception("查询余额支付功能关闭与否Service处理失败！" + msg);
            }
        }

        //打开余额支付功能
        public void OpenBalancePaid(string uin, string ip)
        {
            string msg = "";
            string service_name = "";
            string req = "";
            string att_id = "";
            string uid = AccountData.ConvertToFuid(uin);
            if (uid == null || uid.Length < 3)
            {
                throw new Exception("内部ID不正确！");
            }

            //查询att_id
            try
            {
                service_name = "query_user_service";
                DataSet ds = null;
                //uin：165324199   可测试
                req = "request_type=105&head_u=jusonzhu-risk3&ver=1&sp_id=1000000000&uin=" + uin;

                ds = CommQuery.GetOneTableFromICE(req, "", service_name, false, out msg);
                if (msg != "")
                    throw new Exception("Service处理失败！查询用户账户信息：" + msg);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    att_id = dt.Rows[0]["att_id"].ToString();
                }
                else
                    throw new Exception("Service处理失败！att_id未查到");
            }
            catch (Exception e)
            {
                throw new Exception("Service处理失败！查询用户账户信息：" + msg);
            }

            //打开打开余额支付功能
            try
            {
                service_name = "kf_balance_unfreeze_service";
                DataSet ds = null;
                req = "uid=" + uid + "&client_ip=" + ip + "&memo=KFWeb&attid=" + att_id;

                ds = CommQuery.GetOneTableFromICE(req, "", service_name, false, out msg);
                if (msg != "")
                    throw new Exception("打开余额支付功能Service处理失败！" + msg);
            }
            catch (Exception e)
            {
                throw new Exception("打开余额支付功能Service处理失败！" + msg);
            }
        }

        //关闭余额支付功能
        public void ClosedBalancePaid(string uin,string ip)
        {
            string msg = "";
            string service_name = "kf_balance_freeze_service";
            string req = "";

            string uid = AccountData.ConvertToFuid(uin);
            if (uid == null || uid.Length < 3)
            {
                throw new Exception("内部ID不正确！");
            }
            try
            {
                DataSet ds = null;
                //uin：165324199   可测试
                //attid=1为165324199调接口查的
                req = "uid=" + uid + "&client_ip=" + ip + "&memo=KFWeb&attid=1";

                ds = CommQuery.GetOneTableFromICE(req, "", service_name, false, out msg);
                if (msg != "")
                    throw new Exception("Service处理失败！" + msg);
            }
            catch (Exception e)
            {
                throw new Exception("查询余额支付功能关闭与否Service处理失败！" + msg);
            }
        }
    }
}
