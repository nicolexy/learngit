using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace CFT.CSOMS.BLL.BalanceModule
{
    using CFT.CSOMS.DAL.BalanceModule;
    public class BalaceService
    {
        public bool BalancePaidOrNotQuery(string uin,string ip)
        {
            return new BalanceData().BalancePaidOrNotQuery(uin, ip);
        }

        public void OpenBalancePaid(string uin, string ip)
        {
            new BalanceData().OpenBalancePaid(uin, ip);
        }

        public void ClosedBalancePaid(string uin, string ip)
        {
            new BalanceData().ClosedBalancePaid(uin, ip);
        }
    }
}
