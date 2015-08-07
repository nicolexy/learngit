using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CFT.CSOMS.DAL;
using System.Collections;
using CFT.CSOMS.DAL.Tests;
using System.Data;
using CFT.CSOMS.DAL.Infrastructure;

namespace CFT.CSOMS.BLL.Tests
{
    [TestClass()]
    public class PublicResTest
    {
        [TestMethod]
        public void TestQueryCommRelay()
        {
            DataSet ds = new DataSet();
            string filed="";

            filed += "stime:2015-05-01 17:44:04|etime:2015-05-29 17:44:04|currency_type:HKD|bank_type:4501";
            ds = new PublicRes().QueryCommRelay("3952", filed, 0, 10);

            //filed += "listid:" + "2000000501201504231230007000";
            //filed += "|spid:" + "2000000501";
            //filed += "|bank_listid:" + "";
            //filed += "|bank_currency_fee:" + "2";
            //filed += "|trade_state:" + "1";
            //filed += "|stime:" + "2015-04-01 17:44:04";
            //filed += "|etime:" + "2015-04-29 17:44:04";
            //ds = new PublicRes().QueryCommRelay("3904", filed);

            //filed = "sp_uid:1000100";
            //filed += "|spid:" + "2000000501";
            //filed += "|bank_listid:" + "22222";
            //filed += "|bank_currency_fee:" + "2";
            //filed += "|trade_state:" + "1";
            //filed += "|stime:" + "2015-04-01 17:44:04";
            //filed += "|etime:" + "2015-04-29 17:44:04";
            //ds = new PublicRes().QueryCommRelay("3953", filed, 0, 10);

            //filed = "";
            //filed += "bank_listid:" + "201501070004403";
            //filed += "|bank_currency_fee:" + "2";
            //filed += "|trade_state:" + "1";
            //filed += "|stime:" + "2015-04-01 17:44:04";
            //filed += "|etime:" + "2015-04-29 17:44:04";
            //ds = new PublicRes().QueryCommRelay("3954", filed, 0, 10);
        }
    }
}