using CFT.CSOMS.DAL.TradeModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CFT.CSOMS.DAL.Tests
{
    [TestClass]
    public class TradeDataTest
    {
        [TestMethod]
        public void QueryWxBuyOrderByUidTest()
        {
            var uid = "334063101";
            var s_time = "2014-09-06 00:00:00";
            var e_time = "2014-09-11 23:59:59";
            DataSet ds = (new TradeData()).QueryWxBuyOrderByUid(int.Parse(uid), DateTime.Parse(s_time), DateTime.Parse(e_time));
        }
    }
}
