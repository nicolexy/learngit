using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace CFT.CSOMS.BLL.PNRModule
{
    using CFT.CSOMS.DAL.PNRModule;
    using System.Collections;
    using TENCENT.OSS.C2C.Finance.Common.CommLib;
    using CFT.CSOMS.COMMLIB;
    public class PNRService
    {
        /// <summary>
        /// PNR订单查询
        /// </summary>
        /// <param name="pnr">pnr</param>
        /// <param name="payFlowCode">财付通订单号</param>
        /// <returns></returns>
        public DataTable QueryPNROrder(string pnr, string payFlowCode)
        {
            if (string.IsNullOrEmpty(pnr.Trim()) && string.IsNullOrEmpty(payFlowCode.Trim()))
            {
                throw new ArgumentNullException("pnr and payFlowCode");
            }

            DataSet ds = new PNRData().QueryPNROrder(pnr, payFlowCode);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("FbillStatus_str", typeof(String));//状态
                ds.Tables[0].Columns.Add("billTime", typeof(String));
                ds.Tables[0].Columns.Add("airInTime", typeof(String));
                ds.Tables[0].Columns.Add("airOutTime", typeof(String));
                ds.Tables[0].Columns.Add("payTime", typeof(String));

                COMMLIB.CommUtil.DbTimeToPageContent(ds.Tables[0], "FbillTime", "billTime");
                COMMLIB.CommUtil.DbTimeToPageContent(ds.Tables[0], "FairInTime", "airInTime");
                COMMLIB.CommUtil.DbTimeToPageContent(ds.Tables[0], "FairOutTime", "airOutTime");
                COMMLIB.CommUtil.DbTimeToPageContent(ds.Tables[0], "FpayTime", "payTime");

                Hashtable ht1 = new Hashtable();
                ht1.Add("0", "是入库成功未支付");
                ht1.Add("1", "已经支付");
                ht1.Add("2", "支付后出票失败");
                ht1.Add("3", "已出票但RR失败");
                ht1.Add("4", "出票成功");
                COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "FbillStatus", "FbillStatus_str", ht1);
                return ds.Tables[0];
            }
            else
                return null;
        }

        /// <summary>
        /// PNR操作查询
        /// </summary>
        /// <param name="pnr">pnr</param>
        /// <param name="startTime">开始时间戳</param>
        /// <param name="endTime">结束时间戳</param>
        /// <param name="agent">航空公司</param>
        /// <param name="status">状态</param>
        /// <param name="start"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public DataTable QueryPNROperate(string pnr, int startTime, int endTime, string agent, string status, int start, int max)
        {
            if (string.IsNullOrEmpty(pnr.Trim()) && startTime == 0 && endTime == 0)
            {
                throw new ArgumentNullException("pnr and startTime and endTime");
            }

            DataSet ds = new PNRData().QueryPNROperate(pnr, startTime, endTime, agent, status, start, max);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("Fagent_str", typeof(String));//公司
                ds.Tables[0].Columns.Add("Fstatus_str", typeof(String));//状态
                ds.Tables[0].Columns.Add("startTime", typeof(String));
                ds.Tables[0].Columns.Add("endTime", typeof(String));
             
                Hashtable htCompany = commData.AirCompany();
                Hashtable htPNRStatus = commData.PNRStatus();

                COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "Fagent", "Fagent_str", htCompany);
                COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "Fstatus", "Fstatus_str", htPNRStatus);
                COMMLIB.CommUtil.DbTimeToPageContent(ds.Tables[0], "FstartTime", "startTime");
                COMMLIB.CommUtil.DbTimeToPageContent(ds.Tables[0], "FendTime", "endTime");
                return ds.Tables[0];
            }
            else
                return null;
        }

    }
}
