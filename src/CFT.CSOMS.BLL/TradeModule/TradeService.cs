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
        public int RemoveLogInsert(string qqid, string FbalanceStr, string FtypeText, string curtype, DateTime FmodifyTime, string FupdateUser)
        {
            return new TradeData().RemoveLogInsert(qqid, FbalanceStr, FtypeText, curtype, FmodifyTime, FupdateUser);
        }
        public void RemoveLogInsertAll(string qqid,string uid, DataTable dt) 
        {
            foreach (DataRow item in dt.Rows)
            {
                new TradeData().RemoveLogInsert(qqid, item["FbalanceStr"].ToString(), item["FtypeText"].ToString(), item["cur_type"].ToString(), DateTime.Now, uid);
            }
        }
        public DataSet RemoveLogQuery(string qqid) 
        {
            return new TradeData().RemoveLogQuery(qqid);
        }
    }
}
