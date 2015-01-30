using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CFT.Apollo.Logging;

namespace CFT.CSOMS.DAL.WechatPay
{
    public class AuthenData
    {
        public DataSet QueryWechatRealNameAuthen(string uin, string serialno, string clientIp) 
        {
            /*
            string inmsg = "spid=2000000501&CMD=FINANCE_MERINFO_STANDBY_XX&standby6=siming.huang@mangocity.com;liping.li@mangocity.com;bo.wei@mangoctestmodify@qq.com&client_ip=::1&operator_id=1100000000&module=修改外卡商户客服联系邮箱&function=modify_kf_email&modify_time=2014-09-12 17:04:19";
            string reply = "";
            string msg = "";
            short result = 1;

            TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("ui_common_update_service", inmsg, true, out reply, out result, out msg);
            LogHelper.LogInfo("ui_common_update_service" + " return:" + msg);
            return null;
            */
            
            string msg = string.Empty;
            string serviceName = "au_check_cre_service";
            StringBuilder reqBuilder = new StringBuilder()
            .AppendFormat("uin={0}", uin)
            .AppendFormat("&bind_serialno={0}", serialno)
            .AppendFormat("&client_ip={0}", clientIp);

            LogHelper.LogInfo(serviceName + " send req:" + reqBuilder.ToString());
            DataSet ds = TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.GetOneTableFromICE(reqBuilder.ToString(), "", serviceName, false, out msg);
            LogHelper.LogInfo(serviceName + " return:" + msg);

            return ds;
            
        }
    }
}
