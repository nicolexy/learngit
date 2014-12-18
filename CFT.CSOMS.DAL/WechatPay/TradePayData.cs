using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CFT.Apollo.Logging;
using CFT.CSOMS.DAL.Infrastructure;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace CFT.CSOMS.DAL.WechatPay
{
    public class TradePayData
    {
        public DataTable QueryWxTrans(string prime_trans_id) 
        {
            if (string.IsNullOrEmpty(prime_trans_id))
            {
                throw new Exception("prime_trans_id不能为空！");
            }

            string msg;
            
            string serviceName = "wxt_pay_qryprime_service";
            string req = "prime_trans_id=" + prime_trans_id;
            DataSet ds = CommQuery.GetOneTableFromICE(req, "", serviceName, true, out msg);
            if (msg != "")
            {
                LogHelper.LogError(msg);
                throw new Exception(msg); 
            }

            if (ds != null && ds.Tables.Count > 0) 
            {
                return ds.Tables[0];
            }

            return null;
        }
    }
}
