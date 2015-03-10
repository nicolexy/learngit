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
        public int ControledFinRemoveLogInsert(string qqid, string FbalanceStr, string FtypeText, string curtype, DateTime FmodifyTime, string FupdateUser)
        {
            return new TradeData().RemoveControledFinLogInsert(qqid, FbalanceStr, FtypeText, curtype, FmodifyTime, FupdateUser);
        }
        public void RemoveControledFinLogInsertAll(string qqid, string uid, DataTable dt) 
        {
            foreach (DataRow item in dt.Rows)
            {
                new TradeData().RemoveControledFinLogInsert(qqid, item["FbalanceStr"].ToString(), item["FtypeText"].ToString(), item["cur_type"].ToString(), DateTime.Now, uid);
            }
        }
        public DataSet RemoveControledFinLogQuery(string qqid) 
        {
            return new TradeData().RemoveControledFinLogQuery(qqid);
        }
    }
}
