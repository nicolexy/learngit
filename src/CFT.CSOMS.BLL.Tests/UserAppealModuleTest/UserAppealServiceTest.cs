using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CFT.CSOMS.DAL.Tests;
using CFT.CSOMS.BLL.UserAppealModule;
using CFT.Apollo.Logging;

namespace CFT.CSOMS.BLL.Tests
{
       [TestClass()]
    public class UserAppealServiceTest
    {
        [TestInitialize]//测试之前初始化数据
        public void SetUp()
        {
            new UserAppealDataInitData().SetUp();
        }

        [TestMethod]
        public void TestBatchFinishAppeal()
        {
            //验证初始状态
            //普通解冻
            string sql = "select * from db_appeal_2015.t_tenpay_appeal_trans_04 where Fid='20150417001';";
            DataSet ds = new Tool().QuerData(sql, "fkdj");
            Assert.AreEqual("0", ds.Tables[0].Rows[0]["FState"].ToString());

            //微信解冻
            sql = "select * from db_appeal_2015.t_tenpay_appeal_trans_04 where Fid='20150417004';";
            ds = new Tool().QuerData(sql, "fkdj");
            Assert.AreEqual("0", ds.Tables[0].Rows[0]["FState"].ToString());

            sql = "select * from db_appeal_2015.t_tenpay_appeal_trans_02 where Fid='201502170011';";
            ds = new Tool().QuerData(sql, "fkdj");
            Assert.AreEqual("11", ds.Tables[0].Rows[0]["FState"].ToString());

            //解冻未处理状态批量结单
            new UserAppealService().BatchFinishAppeal("2015-4-15 00:00:00", "2015-4-15 23:59:59", "1");
            sql = "select * from db_appeal_2015.t_tenpay_appeal_trans_04 where Fid='20150417001';";
            ds = new Tool().QuerData(sql, "fkdj");
            Assert.AreEqual("21", ds.Tables[0].Rows[0]["FState"].ToString());

            sql = "select * from db_appeal_2015.t_tenpay_appeal_trans_04 where Fid='20150417004';";
            ds = new Tool().QuerData(sql, "fkdj");
            Assert.AreEqual("21", ds.Tables[0].Rows[0]["FState"].ToString());

            //特殊找密待补充资料结单
            new UserAppealService().BatchFinishAppeal("2015-2-01 00:00:00", "2015-2-28 23:59:59", "2");
            sql = "select * from db_appeal_2015.t_tenpay_appeal_trans_02 where Fid='201502170011';";
            ds = new Tool().QuerData(sql, "fkdj");
            Assert.AreEqual("20", ds.Tables[0].Rows[0]["FState"].ToString());

        }

        [TestMethod]
        public void TestScreenNoFreezeLogApeal()
        { 
            UserAppealService userAppealService=new UserAppealService();
            
            DataSet ds = userAppealService.QueryApealListNewDB("2015-4-15 00:00:00", "2015-4-15 23:59:59", 0, 8);
            userAppealService.ScreenNoFreezeLogApeal(ds);
            Assert.AreEqual("20150417001", ds.Tables[0].Rows[0]["Fid"].ToString());
            Assert.AreEqual("12345671", ds.Tables[0].Rows[0]["Fuin"].ToString());

            ds = new UserAppealService().QueryApealListNewDB("2015-4-15 00:00:00", "2015-4-15 23:59:59", 0, 19);
            userAppealService.ScreenNoFreezeLogApeal(ds);
            Assert.AreEqual("20150417004", ds.Tables[0].Rows[0]["Fid"].ToString());
            Assert.AreEqual("12345674", ds.Tables[0].Rows[0]["Fuin"].ToString());
        }
       
       
    
    }
}
