using System;
using CFT.CSOMS.DAL.AccountModule;

namespace CFT.CSOMS.BLL.AccountModule
{
    public class AccountService
    {
        /// <summary>
        /// 注销微信支付账户
        /// </summary>       
        public bool LogOnWxAccount(string accid, string username, string oaticket, string clientip, string reason, out string msg)
        {
            return new AccountData().LogOnWxAccount(accid, username, oaticket, clientip, reason, out msg);;
        }
    }
}
