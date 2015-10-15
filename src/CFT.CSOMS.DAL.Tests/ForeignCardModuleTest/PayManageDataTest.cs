using CFT.CSOMS.DAL.ForeignCardModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.DAL.Tests.ForeignCardModuleTest
{
    [TestClass]
    public class PayManageDataTest
    {
        /// <summary>
        /// 个人资金流水 测试
        /// qry_bankroll_list_service
        /// </summary>
        [TestMethod]
        public void GetForeignCardRollListTest()
        {
            try
            {
                DataSet ds = new PayManageData().GetForeignCardRollList("295199001", 1, "0000-00-00 00:00:00", "0000-00-00 00:00:00", 0, 10);
                Assert.IsNotNull(ds);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
