using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.CSOMS.DAL.CFTAccount;

namespace CFT.CSOMS.DAL.PNRModule
{
    public class PNRData
    {
        //PNR订单查询
        public DataSet QueryPNROrder(string pnr, string payFlowCode)
        {
            using (var da = MySQLAccessFactory.GetMySQLAccess("BTBPNR"))
            {
                da.OpenConn();
                string Sql = " select * from btbPNR.t_bill where 1=1 ";
                if (!string.IsNullOrEmpty(pnr.Trim()))
                    Sql += "and Fpnr='" + pnr + "' ";
                if (!string.IsNullOrEmpty(payFlowCode.Trim()))
                    Sql += "and Fpayflowcode ='" + payFlowCode + "' ";
                DataSet ds = da.dsGetTotalData(Sql);

                return ds;
            }
        }


        //PNR操作查询
        public DataSet QueryPNROperate(string pnr, int startTime, int endTime, string agent, string status, int start, int max)
        {
            using (var da = MySQLAccessFactory.GetMySQLAccess("BTBPNR"))
            {
                da.OpenConn();
                string Sql = " select * from btbPNR.t_try_instore where 1=1 ";
                if (!string.IsNullOrEmpty(pnr.Trim()))
                    Sql += "and Fpnr='" + pnr + "' ";
                if (startTime != 0 && endTime != 0)
                    Sql += "and FstartTime >=" + startTime + " and FstartTime <=" + endTime + " ";
                if (!string.IsNullOrEmpty(agent.Trim()))
                    Sql += "and Fagent='" + agent + "' ";
                if (!string.IsNullOrEmpty(status.Trim()))
                    Sql += "and Fstatus='" + status + "' ";
                Sql +=" order by FstartTime desc limit " + start + " , " + max ;
                DataSet ds = da.dsGetTotalData(Sql);

                return ds;
            }
        }

    }
}
