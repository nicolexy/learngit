using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using CFT.CSOMS.DAL.PNRModule;
using System.Collections;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.COMMLIB;
using CFT.CSOMS.DAL.TradeModule;

namespace CFT.CSOMS.BLL.TradeModule
{
   
    public class TradeService
    {
        public DataSet BeforeCancelTradeQuery(string uid)
        {
            if (string.IsNullOrEmpty(uid.Trim()) && uid.Trim().Length<3)
            {
                throw new Exception("内部id有误");
            }

            return new TradeData().BeforeCancelTradeQuery(uid);

        }

        public DataSet QueryWxBuyOrderByUid(int uid, DateTime startTime, DateTime endTime)
        {
            return (new TradeData()).QueryWxBuyOrderByUid(uid, startTime, endTime);
        }
    }
}
