using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CFT.CSOMS.DAL.TradeModule;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace CFT.CSOMS.DAL.Tests.TradeModule
{
    [TestClass]
    public class TradeDataTestHyw
    {
        [TestMethod]
        public void Test()
        {
            qry_user_bankroll_cTest();
            qry_sp_bankroll_cTest();
            qry_bank_bankroll_cTest();
        }

        [TestMethod]
        public void qry_user_bankroll_cTest()
        {
            try
            {
                //DataSet ds = new TradeData().GetBankRollList_withID(DateTime.Now.AddDays(-1), DateTime.Now, "", 0, 10);
                //qry_user_bankroll_c
                string ListID = "10000000002009060515551105798103";
                string onerow = "295188308";
                string req = "MSG_NO=" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                req += "&transaction_id=" + ListID;
                req += "&uid=" + onerow;
                string errMsg;
                DataSet ds = CommQuery.GetDataSetFromICE(req, CommQuery.交易单资金流水_个人, false, "qry_user_bankroll_c", out errMsg);
            }
            catch (Exception ex)
            {
            }
        }

        [TestMethod]
        public void qry_sp_bankroll_cTest()
        {
            try
            {
                string ListID = "2000000501201509281230009222";
                string spUid = "10004";
                string startTime = "2015-09-28";
                string req = "MSG_NO=" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                req += "&transaction_id=" + ListID;
                req += "&sp_uid=" + spUid;
                req += "&start_time=" + startTime;
                string errMsg;
                DataSet ds = CommQuery.GetDataSetFromICE(req, CommQuery.交易单资金流水_商户, false, "qry_sp_bankroll_c", out errMsg);
            }
            catch (Exception ex)
            {
            }
        }

        [TestMethod]
        public void qry_bank_bankroll_cTest()
        {
            try
            {
                string ListID = "2000000501421509251230000216";
                string bankType = "2011";
                string startTime = "2015-09-28";
                string req = "MSG_NO=" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                req += "&transaction_id=" + ListID;
                req += "&bank_type=" + bankType;
                req += "&start_time=" + startTime;
                string errMsg;
                DataSet ds = CommQuery.GetDataSetFromICE(req, CommQuery.交易单资金流水_银行, false, "qry_bank_bankroll_c", out errMsg);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
