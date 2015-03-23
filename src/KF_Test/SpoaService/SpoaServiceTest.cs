using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CFT.CSOMS.BLL.SPOA;
using System.Data;

namespace KF_Test.SpoaService
{
    [TestClass]
    public class SpoaServiceTest
    {
        [TestMethod]
        public void GetOneValueAddedTax()
        {
            string spid="2000000501";
            SPOAService spoa = new SPOAService();
            DataSet ds = spoa.GetOneValueAddedTax(spid);
            Assert.AreEqual("2000000501", ds.Tables[0].Rows[0]["spid"].ToString());

        }

        [TestMethod]
        public void GetSelfQueryInfo()
        {
            string keyid = "10037";
            SPOAService spoa = new SPOAService();
            DataSet ds = spoa.GetSelfQueryInfo(keyid);
            Assert.AreEqual("10037", ds.Tables[0].Rows[0]["ApplyCpInfoID"].ToString());

        }

        [TestMethod]
        public void QueryAgencyBySpid()
        {
            string spid = "2201146701";
            SPOAService spoa = new SPOAService();
            DataSet ds = spoa.QueryAgencyBySpid(spid);
            Assert.AreEqual("2201146701", ds.Tables[0].Rows[0]["spid"].ToString());

        }

        [TestMethod]
        public void QueryAgencyInfoById() 
        {
            string keyid = "10037";
            SPOAService spoa = new SPOAService();
            DataSet ds = spoa.QueryAgencyInfoById(keyid);
            Assert.AreEqual("10037", ds.Tables[0].Rows[0]["ApplyCpInfoID"].ToString());

        }

        [TestMethod]
        public void GetShouFuYiList()
        {
            string qq = "3789";
            SPOAService spoa = new SPOAService();
            DataSet ds = spoa.GetShouFuYiList(qq);
            Assert.AreEqual("3789", ds.Tables[0].Rows[0]["qq"].ToString());
        }

        [TestMethod]
       public void BusinessLogout()
       {
           string spid = "1201101901";
           string reason="test";
           string UserName="v_yqyqguo";
           SPOAService spoa = new SPOAService();
           string res = spoa.BusinessLogout(spid, UserName, reason);
           Assert.AreEqual("0", res);
       }

        [TestMethod]
        public void ClosePay()
        {
            string spid = "1201101901";
            string reason = "test";
            string UserName = "v_yqyqguo";
            SPOAService spoa = new SPOAService();
            string res = spoa.ClosePay(spid, UserName, reason);
            Assert.AreEqual("0", res);
        }

        [TestMethod]
        public void FreezeSpid()
        {
            string spid = "1201101901";
            string reason = "test";
            string UserName = "v_yqyqguo";
            SPOAService spoa = new SPOAService();
            string res = spoa.FreezeSpid(spid, UserName, reason);
            Assert.AreEqual("0", res);
        }

        [TestMethod]
        public void LostOfSpid()
        {
            string spid = "1201101901";
            string reason = "test";
            string UserName = "v_yqyqguo";
            SPOAService spoa = new SPOAService();
            string res = spoa.LostOfSpid(spid, UserName, reason);
            Assert.AreEqual("0", res);
        }

        [TestMethod]
        public void CloseAgency()
        {
            string spid = "1201101901";
            string reason = "test";
            string UserName = "v_yqyqguo";
            SPOAService spoa = new SPOAService();
            string res = spoa.CloseAgency(spid, UserName, reason);
            Assert.AreEqual("0", res);
        }

        [TestMethod]
        public void CloseRefund()
        {
            string spid = "1201101901";
            string reason = "test";
            string UserName = "v_yqyqguo";
            SPOAService spoa = new SPOAService();
            string res = spoa.CloseRefund(spid, UserName, reason);
            Assert.AreEqual("0", res);
        }

        [TestMethod]
        public void RestoreOfSpid()
        {
            string spid = "1201101901";
            string reason = "test";
            string UserName = "v_yqyqguo";
            SPOAService spoa = new SPOAService();
            string res = spoa.RestoreOfSpid(spid, UserName, reason);
            Assert.AreEqual("0", res);
        }
    }
}
