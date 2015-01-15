using System;
using System.Data;
using System.Text;
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
    public class RelayAccessFactoryTest
    {
        [TestMethod]
        public void RelayInvokeTest()
        {
            RelayAccessFactory relay = new RelayAccessFactory();
            string inmsg = "request_type=6952&ver=1&head_u=&sp_id=1000000000&channel_id=1&direct=1&req_type=25&query_dim=&card_id=121212&bank_type=1010";
            string ip="10.12.199.234";
            int port=22000;

          //  relay.GetDSFromRelay(inmsg, ip, port);
        }
    }
}
