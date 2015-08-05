using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CFT.CSOMS.DAL;
using System.Collections;
using CFT.CSOMS.DAL.Tests;
using CFT.CSOMS.BLL.CFTAccountModule;
using System.Data;
using CFT.CSOMS.BLL.ForeignCardModule;

namespace CFT.CSOMS.BLL.Tests
{
    [TestClass()]
    public class AccountServiceTest
    {
        [TestMethod]
        public void TestQueryForeignCardInfoByOrder()
        {
            string filed = "listid:" + "11111";
            filed += "|spid:" + "12345";
            filed += "|bank_listid:" + "2222";
            filed += "|bank_currency_fee:" + "200";
            filed += "|trade_state:" + "1";
            filed += "|stime:" + "2015-07-01 00:00:00";
            filed += "|etime:" + "2015-07-20 00:00:00";
            DataSet ds = new ForeignCardService().QueryForeignCardInfoByOrder(filed);
        }

        [TestMethod]
        public void LCTAccStateOperatorTest()
        {
            AccountService acc = new AccountService();
            string uin = "2085e9858e37f30ec57638b12a@wx.tenpay.com";
            string cre_id = "410504198701151035";
            string cre_type = "1";
            string name = "JoeyMwan";
            string op_type = "3";//查询
            string caller_name = "";
            string client_ip = "";

            //查询 此时为 未冻结 状态
            Boolean state = acc.LCTAccStateOperator(uin, cre_id, cre_type, name, "3", caller_name, client_ip);
            Assert.AreEqual(false, state);
            //冻结
            state = acc.LCTAccStateOperator(uin, cre_id, cre_type, name, "1", caller_name, client_ip);
            Assert.AreEqual(true, state);
            //解冻
            state = acc.LCTAccStateOperator(uin, cre_id, cre_type, name, "2", caller_name, client_ip);
            Assert.AreEqual(true, state);



        }
    }
}