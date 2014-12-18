using System;
using System.Data;
using CFT.CSOMS.BLL.CFTAccountModule;
using CFT.CSOMS.BLL.SPOA;
using CFT.CSOMS.COMMLIB;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.CSOMS.DAL.LifeFeePaymentModule;
using CFT.CSOMS.DAL.SPOA;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace KF_Test
{
    [TestClass]
    public class ServiceToRelayTest
    {
        string strReplyInfo = "";
        short iResult = 0;
        string Msg = "";

        [TestMethod]
        public void query_order_serviceTest()
        {

            bool index = new commRes().NoStaticMiddleInvokeTest("query_order_service", "listid=121212121", false, out strReplyInfo, out iResult, out Msg);
        }

        [TestMethod]
        public void itg_modify_refund_serviceTest()
        {

            bool index = new commRes().NoStaticMiddleInvokeTest("itg_modify_refund_service", "", false, out strReplyInfo, out iResult, out Msg);
        }

    }
}
