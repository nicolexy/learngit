using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using CFT.CSOMS.BLL.CFTAccountModule;
using CFT.CSOMS.BLL.SPOA;
using CFT.CSOMS.COMMLIB;
using CFT.CSOMS.DAL.SPOA;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace KF_Test
{
    [TestClass]
    public class MerchantServiceTest
    {
        /// <summary>
        /// 查询商户证书统计
        /// </summary>
        [TestMethod]
        public void queryMerchantCertStatTest()
        {
            MerchantData mer = new MerchantData();
            DateTime start = DateTime.Parse("2014-07-15 00:54:44");
            DateTime end = DateTime.Parse("2014-07-15 23:54:44");
            int startTime = CommUtil.ConvertDateTimeInt(start);
            int endTime = CommUtil.ConvertDateTimeInt(end);
            DataTable tb = mer.queryMerchantCertStat(startTime, endTime, "1203487901", "2014");

        }

        [TestMethod]
        public void queryMerchantCertStatRelayTest()
        {
            MerchantData mer = new MerchantData();
            DateTime start = DateTime.Parse("2014-07-15 00:54:44");
            DateTime end = DateTime.Parse("2014-07-15 23:54:44");
            int startTime = CommUtil.ConvertDateTimeInt(start);
            int endTime = CommUtil.ConvertDateTimeInt(end);
            DataTable tb = mer.queryMerchantCertStat(startTime, endTime, "1203487901", "2014");

        }

        [TestMethod]
        public void GetContractStateTest()
        {
            string msg = "";
            string answer=commRes.GetFromCGI("http://10.12.189.116/CSP/Rest/CSPService/Get/GetContractState/appid/?contractid=201412035002", "utf-8", out msg);
            Assert.AreEqual(true, answer.IndexOf("Result")>0);

        }

        [TestMethod]
        public void GetContractStateResultTest()
        {
              string ret = "{\"Result\":\"OK\",\"State\":\"审批结束-待盖章\",\"Msg\":\"\"}";
              ret = "{\"Result\":\"ERROR\",\"State\":\"\",\"Msg\":\"XXX\"}";
              string result = ret.Substring(11, ret.IndexOf(",\"State\"")-12);
              int resultn=ret.IndexOf("\"State\":\"")+9;
              string state = ret.Substring(resultn, ret.IndexOf("\",\"Msg") - resultn);
              int megn = ret.IndexOf("\"Msg\"") + 7;
              string msg = ret.Substring(megn,ret.Length-megn-2);

        }

        [TestMethod]
        public void QueryContractTest()
        {
            new MerchantService().QueryContract("", "", "",
                                     "2014-01-01 00:00:00", "2014-12-30 23:59:59","","","","","","",
                                     0, 10);
        }


     
    }
}
