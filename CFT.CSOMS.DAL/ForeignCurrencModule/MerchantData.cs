using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.DAL.Infrastructure;
using System.Configuration;

namespace CFT.CSOMS.DAL.ForeignCurrencModule
{
    public class MerchantData
    {
        //商户资料接口查询
        public DataSet MerInfoQuery(string spid, string uid, string ip)
        {
            string msg = "";
            string service_name = "ia_ui_query_merinfo_service";
            string req = "";

            if (string.IsNullOrEmpty(spid) && string.IsNullOrEmpty(uid))
            {
                throw new Exception("spid和uid至少有一个不能为空！");
            }
            try
            {
                DataSet ds = null;
                if (!string.IsNullOrEmpty(spid))
                    req += "spid=" + spid;
                if (!string.IsNullOrEmpty(uid))
                    req += "&uid=" + uid;
                req += "&client_ip=" + ip ;

                ds = CommQuery.FCGetOneTableFromICE(req, "", service_name, false, out msg);
                if (msg != "")
                    throw new Exception("Service处理失败！" + msg);
               
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("查询商户资料Service处理失败！" + msg);
            }
        }

        //查询商户acno
        public DataSet AcnoQuery(string spid, string acc_type, string cur_type)
        {
            string msg = "";
            string req = "";

            if (string.IsNullOrEmpty(spid))
            {
                throw new Exception("spid不能为空！");
            }
            try
            {
                DataSet ds = null;
                if (!string.IsNullOrEmpty(spid))
                    req += "spid=" + spid;
                if (!string.IsNullOrEmpty(acc_type))
                    req += "&acc_type=" + acc_type;
                if (!string.IsNullOrEmpty(cur_type))
                    req += "&cur_type=" + cur_type;

                ds = CommQuery.GetDataSetFromICEIA(req, "SP_ACNO_QUERY", out msg);
                if (msg != "")
                    throw new Exception("Service处理失败！" + msg);

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("查询商户acno，Service处理失败！" + msg);
            }
        }

    }
}
