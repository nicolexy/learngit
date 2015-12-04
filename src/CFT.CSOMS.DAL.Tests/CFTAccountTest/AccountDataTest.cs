using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.C2C.Finance.DataAccess;

namespace CFT.CSOMS.DAL.Tests.CFTAccountTest
{
    [TestClass]
    public class AccountDataTest
    {
        [TestMethod]
        public void qry_user_balance_slave_cTest()
        {
            try
            {
                //qry_user_balance_slave_c
                string errMsg = "";
                string req = "MSG_NO=" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                req += "&curtype=1";
                req += "&uid=298684980";
                DataSet userDs = CommQuery.GetDSForServiceFromICE(req, "qry_user_balance_slave_c", false, out errMsg);
                //YWCommandCode.查询用户信息
            }
            catch (Exception ex)
            {
            }
        }
    }
}
