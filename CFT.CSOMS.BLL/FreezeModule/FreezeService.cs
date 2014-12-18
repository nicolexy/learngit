using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CFT.CSOMS.DAL.FreezeModule;

namespace CFT.CSOMS.BLL.FreezeModule
{
    public class FreezeService
    {
        public void SendWechatMsg(string reqsource, string accid, string templateid, string cont1, string cont2, string cont3, string msg_type)
        {
            new FreezeData().SendWechatMsg(reqsource, accid, templateid, cont1, cont2, cont3, msg_type);
        }

        public bool SyncCreid(string uin, string oldCredId, string newCredId, int creType, string optUser, string ip)
        {
            return new FreezeData().SyncCreid(uin, oldCredId, newCredId, creType, optUser, ip);
        }

        public string GetCashOutFreezeListId(string Uid)
        {
            return (new FreezeData()).GetCashOutFreezeListId(Uid);
        }
    }
}
