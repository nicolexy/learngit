using System;
using System.Data;
using CFT.CSOMS.BLL.CFTAccountModule;
using CFT.CSOMS.BLL.SPOA;
using CFT.CSOMS.COMMLIB;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.CSOMS.DAL.LifeFeePaymentModule;
using CFT.CSOMS.DAL.SPOA;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KF_Test
{
    [TestClass]
    public class MiddleAccessFactoryTest
    {
        /// <summary>
        /// 产生数据exau_limitquery_service
        /// </summary>
        [TestMethod]
        public void MiddleInvokeTest()
        {
            CommRes middle = new CommRes();
           
            string inmsg = "sp_id=1000000000&channel_id=1&direct=1&req_type=25&query_dim=&card_id=121212&bank_type=1010&amount=200";
            inmsg = "sp_id=1000000000&channel_id=1&direct=1&req_type=22&query_dim=&card_id=121212&bank_type=1010&amount=500&category=1&pay_type=2&card_type=1";
            
            string msg = "";
            bool index = true;
          //  DataSet ds = middle.GetOneTableFromICETest("exau_limitsum_service", inmsg, index);
        }

    }
}
