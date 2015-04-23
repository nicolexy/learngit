using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CFT.CSOMS.DAL.Infrastructure;
using System.Collections;
using System.Data;
using CFT.CSOMS.DAL.UserAppealModule;

namespace CFT.CSOMS.DAL.Tests
{
     [TestClass()]
    public class UserAppealDataTest
    {
         [TestInitialize]//测试之前初始化数据
         public void SetUp()
         {
             new UserAppealDataInitData().SetUp();
         }

         [TestMethod]
         public void TestQueryApealListNewDB()
         {
             UserAppealData userAppealData = new UserAppealData();
             //普通解冻
             DataSet ds = userAppealData.QueryApealListNewDB("2015-4-15 00:00:00", "2015-4-15 23:59:59", 0, 8);
             Assert.AreEqual("20150417001",ds.Tables[0].Rows[0]["Fid"].ToString());
             Assert.AreEqual("12345671", ds.Tables[0].Rows[0]["Fuin"].ToString());
             Assert.AreEqual("0", ds.Tables[0].Rows[0]["FState"].ToString());
             Assert.AreEqual("2015-04-15", DateTime.Parse(ds.Tables[0].Rows[0]["FSubmitTime"].ToString()).ToString("yyyy-MM-dd"));

             //微信解冻
             ds = userAppealData.QueryApealListNewDB("2015-4-15 00:00:00", "2015-4-15 23:59:59", 0, 19);
             Assert.AreEqual("20150417003", ds.Tables[0].Rows[0]["Fid"].ToString());
             Assert.AreEqual("12345673", ds.Tables[0].Rows[0]["Fuin"].ToString());
             Assert.AreEqual("0", ds.Tables[0].Rows[0]["FState"].ToString());
             Assert.AreEqual("2015-04-15", DateTime.Parse(ds.Tables[0].Rows[0]["FSubmitTime"].ToString()).ToString("yyyy-MM-dd"));

             //特殊找密
             ds = userAppealData.QueryApealListNewDB("2015-2-1 00:00:00", "2015-2-25 23:59:59", 11, 11);
             Assert.AreEqual("201502170011", ds.Tables[0].Rows[0]["Fid"].ToString());
             Assert.AreEqual("12345671", ds.Tables[0].Rows[0]["Fuin"].ToString());
             Assert.AreEqual("11", ds.Tables[0].Rows[0]["FState"].ToString());
             Assert.AreEqual("2015-02-15", DateTime.Parse(ds.Tables[0].Rows[0]["FSubmitTime"].ToString()).ToString("yyyy-MM-dd"));
         }

         [TestMethod]
         public void TestFinishAppealNewDB()
         {
             UserAppealData userAppealData = new UserAppealData();
             //普通解冻
             bool result = userAppealData.FinishAppealNewDB("20150417001", "test", "echo");
             Assert.AreEqual(true, result);
             string sql = "select * from db_appeal_2015.t_tenpay_appeal_trans_04 where Fid='20150417001';";
             DataSet ds =new Tool().QuerData(sql, "fkdj");
             Assert.AreEqual("21", ds.Tables[0].Rows[0]["FState"].ToString());

             //特殊找密
             result = userAppealData.FinishAppealNewDB("201502170011", "testSpecialFindKey", "echo");
             Assert.AreEqual(true, result);
             sql = "select * from db_appeal_2015.t_tenpay_appeal_trans_02 where Fid='201502170011';";
              ds = new Tool().QuerData(sql, "fkdj");
             Assert.AreEqual("20", ds.Tables[0].Rows[0]["FState"].ToString());
         }

         [TestMethod]
         public void TestQueryFreezeListLog()
         {
             UserAppealData userAppealData = new UserAppealData();
            
             DataSet ds = userAppealData.QueryFreezeListLog("12345672",1);
             Assert.AreEqual("12345672", ds.Tables[0].Rows[0]["FFreezeID"].ToString());
             Assert.AreEqual("1", ds.Tables[0].Rows[0]["FHandleFinish"].ToString());

             ds = userAppealData.QueryFreezeListLog("12345673", 1);
             Assert.AreEqual("12345673", ds.Tables[0].Rows[0]["FFreezeID"].ToString());
             Assert.AreEqual("1", ds.Tables[0].Rows[0]["FHandleFinish"].ToString());
         }
    }
}