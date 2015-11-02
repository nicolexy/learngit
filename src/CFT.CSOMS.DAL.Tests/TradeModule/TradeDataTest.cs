using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CFT.CSOMS.DAL.TradeModule;

namespace CFT.CSOMS.DAL.Tests.TradeModule
{
    [TestClass]
    public class TradeDataTest
    {
        /// <summary>
        /// 测试 红包是否可以注销
        /// </summary>
        /// <param name="WeChatName"></param>
        /// <returns>true 可以注销</returns>
        [TestMethod]
        public void QueryWXUnfinishedHB()
        {
            TradeData dal = new TradeData();
            var username = "hhaozhang ";
            var bol = dal.QueryWXUnfinishedHB(username);
        }

        /// <summary>
        /// 测试 转账是否可以注销  
        /// </summary>
        /// <param name="WeChatName"></param>
        /// <returns>true 可以注销</returns>
        [TestMethod]
        public void QueryWXUnfinishedTrade()
        {
            TradeData dal = new TradeData();
            var username = "hhaozhang ";
            var bol = dal.QueryWXUnfinishedTrade(username);
        }
    }
}
