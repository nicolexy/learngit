using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CFT.CSOMS.DAL.Infrastructure;
using System.Collections;
using System.Data;
using CFT.CSOMS.DAL.SysManageModule;
using CFT.CSOMS.DAL.FundModule;
using TENCENT.OSS.CFT.KF.DataAccess;
using CFT.CSOMS.DAL.ActivityModule;

namespace CFT.CSOMS.DAL.Tests
{
    [TestClass()]
    public class SysManageDataTest
    {
        [TestMethod]
        public void TestUpdateBankBulletin()
        {
            SysManageData sysManageData = new SysManageData();

            string businesstype = "1";
            int op_support_flag = 1;
            int banktype = 4501;
            string closetype = "2";
            string title = "为了测试自己新增公告";
            string maintext = "银行系统维护中，预计startime恢复。因银行系统维护，此期间操作的资金将延迟到XX月XX日返回结果。";
            string popuptext = "因银行系统维护，此期间操作的资金将延迟到XX月XX日返回结果。";
            string createuser = "echo";
            string updateuser = "echo";

            // 为空字符串模拟老接口
            string op_flag = "1";
            string bull_type = "1";
            string bulletin_id = System.DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.NewStaticNoManage();
            string starttime = "2015-07-28 00:00:00";
            string endtime = "2015-07-30 23:59:59";
            bool index = sysManageData.UpdateBankBulletin(businesstype, op_support_flag,
                banktype, bulletin_id, closetype, title, maintext, popuptext, createuser,
                updateuser, op_flag, bull_type, starttime, endtime);
        }


        [TestMethod]
        public void Testfund_616()
        {


            string sql = "select * from fund_db.t_fund_bank_config where Flstate=1";

            using (MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("fund")))
            {
                da.OpenConn();
                var dt = da.GetTable(sql);
                dt.TableName = "SupportBank";
            }



            SafeCard service = new SafeCard();
            bool dd = service.GetFundSupportBank("2006");
        }

        [TestMethod]
        public void Testfund_677()
        {
            //FundRoll service = new FundRoll();
            //DateTime dts = DateTime.Now.AddYears(-10);
            //DateTime dte = DateTime.Now;
            //DataTable dt = service.QueryFundRollList("1563686969", dts, dte, "", 0, 10, 1);

            ActivityData service1 = new ActivityData();

            string dd = service1.GetChannelIDByFUId("298686152");
        }

        [TestMethod]
        public void Testfund_658()
        {
            FundInfoData service = new FundInfoData();
            DataTable dt = service.QueryAllFundInfo();
        }

        [TestMethod]
        public void Testfund_659()
        {
            string spid = "2000000507";
            if (string.IsNullOrEmpty(spid))
                throw new ArgumentNullException("spid");
            using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            {
                da.OpenConn();
                string Sql = " select Fsp_name,Flstate,Fspid,Ffund_name,Ffund_code,Fcurtype,Fclose_flag,Ftransfer_flag,Fbuy_valid from fund_db.t_fund_sp_config where Fspid='" + spid + "'";
                DataSet ds = da.dsGetTotalData(Sql);
            }

            FundInfoData service = new FundInfoData();
            DataTable dt = service.QueryFundInfoBySpid("1219839601");
        }

        [TestMethod]
        public void Testfund_661()
        {
            SafeCard service = new SafeCard();
            DataTable dt = service.GetPayCardInfo("1563686969");
        }
        [TestMethod]
        public void Testfund_668()
        {
            FundAccountInfo service = new FundAccountInfo();
            DataTable dt = service.QueryFundAccountRelationInfo("1563686969");
        }

        [TestMethod]
        public void Testfund_682()
        {
            LCTBalance service = new LCTBalance();
            DataTable dt = service.QueryLCTBalanceRollList("20140311301196518", 0, 10);
        }

        [TestMethod]
        public void Testfund_681()
        {
            FundInfoData service = new FundInfoData();
            DataTable dt = service.QueryCloseFundRollList("20140311301196518", "000022", "20100816", "20150917");
        }
        [TestMethod]
        public void Testfund_684()
        {
            FundInfoData service = new FundInfoData();
            DataTable dt = service.QueryEstimateProfit("20140117000094000", "9000004", "2010-08-16", "2015-09-30");
        }

        [TestMethod]
        public void Testfund_680()
        {
            DateTime time = DateTime.Now;
            using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            {
                da.OpenConn();
                string Sql = " select * from fund_db.t_fetch_list_" + time.ToString("yyyyMM");
                DataSet dsse = da.dsGetTotalData(Sql);
            }


            FundInfoData service = new FundInfoData();
            DateTime dte = DateTime.Now;
            bool dt = service.QueryIfAnewBoughtFund("2000000507201403111922321999", dte);
        }
        [TestMethod]
        public void Testfund_665()
        {
            FundInfoData service = new FundInfoData();
            DateTime dte = DateTime.Now;
            DataTable dt = service.QueryTradeFundInfo("", "2000000507201403111922321999");
        }

         [TestMethod]
        public void Testfund_667()
        { 
            FundProfit service = new FundProfit();
            DateTime dte = DateTime.Now;
            DataTable dt = service.QueryFundProfitRate("200000070", "234234");
        }

         [TestMethod]
         public void Testfund_623()
         {
             //reqid=623&flag=2&offset=0&limit=10&fields=trade_id:|begin_time:2010-01-10|end_time:2015-10-01|spid:2000000507
             //FundProfit service = new FundProfit();
             //DateTime dte = DateTime.Now;
             //DataTable dt = service.QueryProfitRecord("20140117000094000", "9000004", "2010-01-10", "2015-09-30", -1, "2000000507");


             //FundProfit service = new FundProfit();
             //DateTime dte = DateTime.Now;
             //DataTable dt = service.QueryProfitRecord("20140311301196518","", "2010-01-10", "2015-10-01", -1, "2000000507");
         }
         [TestMethod]
         public void Testfund_678()
         {
             //reqid=623&flag=2&offset=0&limit=10&fields=trade_id:|begin_time:2010-01-10|end_time:2015-10-01|spid:2000000507

             FundProfit service = new FundProfit();
             DateTime dte = DateTime.Now;
             DataTable dt = service.QueryProfitStatistic("20140311301196518",-1,"");
         }
         [TestMethod]
         public void Testfund_612()
         {
             //reqid=623&flag=2&offset=0&limit=10&fields=trade_id:|begin_time:2010-01-10|end_time:2015-10-01|spid:2000000507

             FundProfit service = new FundProfit();
             DateTime dte = DateTime.Now;
             DataTable dt = service.QueryFundBind("20140311301196518");
         }
         [TestMethod]
         public void Testfund_679()
         {
             //reqid=623&flag=2&offset=0&limit=10&fields=trade_id:|begin_time:2010-01-10|end_time:2015-10-01|spid:2000000507

             SafeCard service = new SafeCard();
             DateTime dte = DateTime.Now;
             DataTable dt = service.GetFundTradeLog("1563686969", 0, 10);
         }

    }
}