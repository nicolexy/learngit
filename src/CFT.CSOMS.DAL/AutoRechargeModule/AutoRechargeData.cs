using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace CFT.CSOMS.DAL.AutoRechargeModule
{
    public class AutoRechargeData
    {
        /// <summary>
        /// 自动充值签约查询
        /// </summary>
        public DataSet QueryAutomaticRecharge(string uin, int limStart, int limMax, string client_ip)
        {
            string msg = "";
            try
            {
                string service_name = "charge_query_plan_service";//接口名
                string req = "";

                DataSet ds = null;
                req = "charge_type=nsp&uin=" + uin + "&offset=" + limStart + "&limit=" + limMax + "&client_ip=" + client_ip;
                ds = CommQuery.GetXmlToDataSetFromICE(req, "", service_name, out msg);
                DataTable dtt = ds.Tables[0];
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("Service处理失败！" + msg);
            }
        }

        /// <summary>
        /// 自动充值交易单查询
        /// </summary>
        public DataSet QueryAutomaticRechargeBillList(string uin, string plan_id, int limStart, int limMax, string client_ip)
        {
            string msg = "";
            try
            {
                string service_name = "charge_query_bill_service";//接口名
                string req = "";

                DataSet ds = null;
                req = "uin=" + uin + "&offset=" + limStart + "&limit=" + limMax + "&plan_id=" + plan_id + "&client_ip=" + client_ip;
                ds = CommQuery.GetXmlToDataSetFromICE(req, "BATCH_QUERY", service_name, out msg);
                DataTable dtt = ds.Tables[0];
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("Service处理失败！" + msg);
            }

        }

        /// <summary>
        /// 自动充值扣款方式查询
        /// </summary>
        public DataSet GetBankTypeKK(string uin, string uid)
        {
            string msg = "";
            try
            {
                string service_name = "common_query_service";//接口名
                string spid = ConfigurationManager.AppSettings["AutomaticRechargeSpid"].Trim();
                string req = "";
                req = "fields=uid:" + uid;
                req += "|cur_type:1|spid:" + spid + "|channel_id:8|app_code:9|list_state:1|bind_status:2";
                req += "&flag=1&head_u=107986219&limit=10&offset=0&reqid=208&request_type=4006&sp_id=2000000000&ver=1";

                DataSet ds = null;
                ds = CommQuery.GetXmlToDataSetFromICE(req, "", service_name, out msg);
                DataTable dtt = ds.Tables[0];
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("Service处理失败！" + msg);
            }
        }

    }
}
