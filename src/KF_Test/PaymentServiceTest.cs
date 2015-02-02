using CFT.CSOMS.Service.CSAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Xml;
using KF_Test.Util;

namespace KF_Test
{

    /// <summary>
    /// 支付管理模块API测试
    /// </summary>
    [TestClass()]
    public class PaymentServiceTest
    {
        #region 理财通
        [TestMethod()]
        public void GetTradeIdByUINTest()
        {
            string queryString = "appid=10001&uin=1563686969&token=b11c9da91afa8fa9debf3d2d3bafaddf";//请求参数
            XmlDocument answer = TestUtil.testPaymentService("GetTradeIdByUIN", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        [TestMethod()]
        public void GetUserProfitListTest()
        {
            string queryString = "appid=10001&trade_id=20140311301196518&start_date=&end_date=&currency_type=-1&spid=&offset=0&limit=10&token=0aa384f3e5c520b9109e212456e98fd3";
            XmlDocument answer = TestUtil.testPaymentService("GetUserProfitList", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        [TestMethod()]
        public void GetPayCardInfoTest()
        {
            string queryString = "appid=10001&uin=1563686969&token=b11c9da91afa8fa9debf3d2d3bafaddf";
            XmlDocument answer = TestUtil.testPaymentService("GetPayCardInfo", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        [TestMethod()]
        public void GetBankRollListNotChildrenTest()
        {
            string queryString = "appid=10001&uin=1563686969&cur_type=1&start_date=2014-01-21 00:00:00&end_date=2014-07-22 23:59:59&redirection_type=0&offset=0&limit=10&token=05ceaf2de5b1c5c08de13cbf1d5dc26c";
            XmlDocument answer = TestUtil.testPaymentService("GetBankRollListNotChildren", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        [TestMethod()]
        public void GetUserFundSummaryTest()
        {
            string queryString = "appid=10001&uin=1563686969&token=b11c9da91afa8fa9debf3d2d3bafaddf";
            XmlDocument answer = TestUtil.testPaymentService("GetUserFundSummary", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        [TestMethod()]
        public void GetUserFundAccountInfoTest()
        {
            string queryString = "appid=10001&uin=1563686969&token=b11c9da91afa8fa9debf3d2d3bafaddf";
            XmlDocument answer = TestUtil.testPaymentService("GetUserFundAccountInfo", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        [TestMethod()]
        public void QueryCloseFundRollListTest()
        {
            string queryString = "appid=10001&trade_id=20140311301196518&fund_code=000022&start_date=20140121&end_date=20140722&offset=0&limit=10&token=649b954bc6ee4bdd2793c5d1c4926881";
            XmlDocument answer = TestUtil.testPaymentService("QueryCloseFundRollList", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        [TestMethod()]
        public void GetChildrenBankRollListExTest()
        {
            string queryString = "appid=10001&uin=1563686969&spid=2000000505&curtype=90&start_date=2014-01-21 00:00:00&end_date=2014-07-22  00:00:00&offset=0&limit=10&ftype=0&token=44f2e75e4c4720af0b4f0f59acf01c7d";
            XmlDocument answer = TestUtil.testPaymentService("GetChildrenBankRollListEx", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        [TestMethod()]
        public void GetUINFromWechatTest()
        {
            string queryString = "appid=10001&query_type=WeChatMobile&query_string=13724372081&token=8c31a698355b25d0218c0de00609d98f";
            XmlDocument answer = TestUtil.testPaymentService("GetUINFromWechat", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }
        #endregion

        #region 快捷支付
        [TestMethod]
        public void QueryBankCardListTest()
        {
            string queryString = "appid=10002&bank_card=12345678901&date=20111025&offset=0&limit=10&token=5faa5c24c1a0d0c326084fd7c8c1747f";
            XmlDocument answer = TestUtil.testPaymentService("QueryBankCardList", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        #endregion

        #region 一点通业务

        [TestMethod]
        public void GetBankDicTest()
        {
            string queryString = "appid=10001&token=be8f3a8675c60476b3d961b072a47e1a";
            XmlDocument answer = TestUtil.testPaymentService("GetBankDic", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        [TestMethod]
        public void SyncBankCardBindTest()
        {
            string queryString = "appid=10001&bank_type=2101&card_tail=1234&bank_id=581234&token=cb49c68289043633e0dd4ab45f793ffa";
            XmlDocument answer = TestUtil.testPaymentService("SyncBankCardBind", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }


        [TestMethod]
        public void UnbindBankCardBindTest()
        {
            string queryString = "appid=10001&bank_type=2005&uin=17934958&protocol_no=20110607130744431600000000023808&userip=10.10.10.18&token=675658f8772a47a1d014f0c3f5c4194d";
            XmlDocument answer = TestUtil.testPaymentService("UnbindBankCardBind", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }


        [TestMethod]
        public void UnbingBankCardBindSpecialTest()
        {
            string queryString = "appid=10001&bank_type=2101&uin=17934958&bind_serialno=10607441730295214000&card_tail=1234&protocol_no=20110607130744426300000000018395&token=cc9d466a64ab8e78cb129cf9990142f0";
            XmlDocument answer = TestUtil.testPaymentService("UnbingBankCardBindSpecial", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }


        [TestMethod]
        public void GetBankCardBindListTest()
        {
            string queryString = "appid=10001&bank_type=4184&uin=2322405969&bank_id=&uid=&cre_type=&cre_id=500234197805037308&protocol_no=BB2F7432F3CA4ADA860582DCD3C4A1EE&phoneno=15013500723&begin_date=&end_date=&bind_state=99&query_type=0&offset=0&limit=1&token=b12d4049401ae67586d22a936b01dc05";
            XmlDocument answer = TestUtil.testPaymentService("GetBankCardBindList", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        [TestMethod]
        public void GetBankCardBindDetailTest()
        {
            string queryString = "appid=10001&uid=298849000&index=20&bdindex=1&token=b4f395605b4e6c5a7b1a61fdd82dac49";
            XmlDocument answer = TestUtil.testPaymentService("GetBankCardBindDetail", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        [TestMethod]
        public void GetBankCardBindRelationListTest()
        {
            string queryString = "appid=10001&bank_type=&bank_id=&cre_type=&cre_id=500234197805037308&protocol_no=&phoneno=&bind_state=99&offset=0&limit=100&token=1721638a064a91a8cec51be4625b6bb7";
            XmlDocument answer = TestUtil.testPaymentService("GetBankCardBindRelationList", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        #endregion
    }
}
