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
    public class CommonTest
    {
        [TestMethod]
        public void MiddleInvokeTest()
        {
            string gmtToTime = CommUtil.GMT2Local("Sep 29 07:04:05 2011 GMT").ToString("yyyy-MM-dd 00:00:00");
            gmtToTime = CommUtil.GMT2Local("Jun 11 07:30:20 2015 GMT").ToString("yyyy-MM-dd 00:00:00");
            gmtToTime = CommUtil.GMT2Local("Sep  2 06:53:28 2015 GMT").ToString("yyyy-MM-dd 00:00:00");
        }

        [TestMethod]
        public void DecryptNoPaddingTest()
        {
            string card_noEn = "N-URCH0idyuLXT7CzV-6Qw==";
            string card_n = BankLib.BankIOX.DecryptNoPadding(card_noEn);
        }

    }
}
