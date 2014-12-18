using System;
using System.Data;
using CFT.CSOMS.BLL.SPOA;
using CFT.CSOMS.BLL.WechatPay;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KF_Test
{
    [TestClass]
    public class FastPayServiceTest
    {
        [TestMethod]
        public void QueryBankCardListTest()
        {
            string bankCard = BankLib.BankIOX.DecryptNoPadding("8Qz189FjLX5CU24z_L2yWw==");
            int biz_type = 10100;
            string bankDate = "20140929";
            int limit = 10;
            int offset = 0;
            FastPayService fast = new FastPayService();
            DataSet ds = fast.QueryBankCardList(bankCard, bankDate, biz_type, offset, limit);
            Assert.AreEqual("0", ds.Tables[0].Rows[0]["result"].ToString());
        }



      
    }
}
