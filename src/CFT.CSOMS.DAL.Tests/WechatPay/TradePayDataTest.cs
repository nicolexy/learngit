using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CFT.CSOMS.DAL.Tests.WechatPay
{
    [TestClass]
    public class TradePayDataTest
    {
        [TestMethod]
        public void QueryRealtimeRepayment()
        {
            var dal = new CFT.CSOMS.DAL.WechatPay.TradePayData();

            var data = dal.QueryRealtimeRepayment("203201510073023193911", new DateTime(2015, 10, 7), 1);
            Assert.IsNotNull(data);
        }
    }
}
