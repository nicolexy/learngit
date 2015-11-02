using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CFT.CSOMS.DAL.TradeModule;
using System.Data;
using CFT.CSOMS.DAL.Infrastructure;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace CFT.CSOMS.DAL.Tests.SysManageModuleTest
{
    [TestClass]
    public class SettleTest
    {
        [TestMethod]
        public void GetAirFreeze_140()
        {
            SettleData data = new SettleData();
            DataTable dt = data.GetAirFreeze("2000000501", "280320495", "2009-10-1", "2012-10-1", 0, 10);
        }

        [TestMethod]
        public void getAmount_170()
        {
            SettleData data = new SettleData();
            string uid = "295161818";
            string dd = data.getAmount(uid, "uid");
        }


        [TestMethod]
        public void QuerySettleRuleList_174()
        {
            SettleData data = new SettleData();
            string spid = "2201160801";
            DataTable dt = data.QuerySettleRuleList(spid, 0, 1);
        }

        [TestMethod]
        public void QuerySpControl_175()
        {
            SettleData data = new SettleData();
            string spid = "20000000501";
            DataTable dt = data.QuerySpControl(spid);
        }
        [TestMethod]
        public void QueryRelationOrderList_176()
        {
            SettleData data = new SettleData();
            DataTable dt = data.QueryRelationOrderList("2201195901201210188118681566", "");
        }
        [TestMethod]
        public void GetSettleListAppend_177()
        {
            SettleData data = new SettleData();
            DataTable dt = data.GetSettleListAppend("2000000501201003101610540000");
        }
        [TestMethod]
        public void GetSettleReqList_178()
        {
            SettleData data = new SettleData();
            DataTable dt = data.GetSettleReqList("2201195901201302052137428000", "");
        }
        [TestMethod]
        public void GetSettleReqInfo_179()
        {
            SettleData data = new SettleData();
            DataTable dt = data.GetSettleReqInfo("2000000501201104290821599101");
        }
        [TestMethod]
        public void QueryAdjustList_180()
        {
            SettleData data = new SettleData();
            DataTable dt = data.QueryAdjustList("2000000501201006070618854476", "", "", "");
        }
         [TestMethod]
        public void QueryTrueLimtList_181()
        {
            SettleData data = new SettleData();
            DataTable dt = data.QueryTrueLimtList("2000000501", "", "2");
        }
         [TestMethod]
         public void GetSettleInfoListDetail_182()
         {
             SettleData data = new SettleData();
             DataTable dt = data.GetSettleInfoListDetail("1202038701200802011359035000");
         }
         [TestMethod]
         public void GetSettleRefundListDetail_183()
         {
              SettleData data = new SettleData();
              DataTable dt = data.GetSettleRefundListDetail("1202038701200805081826473101", "1091202038701200805080006517");
         }
         [TestMethod]
         public void GetSettleRefundList_184()
         {
             SettleData data = new SettleData();
             DataTable dt = data.GetSettleRefundList("1091202038701200805080006517", 0, 0, 10);
         }





    }
}
