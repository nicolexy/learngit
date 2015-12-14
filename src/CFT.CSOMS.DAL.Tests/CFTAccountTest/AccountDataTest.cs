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
                GetUserInfoByCurType("1");
                GetUserInfoByCurType("80");
            }
            catch (Exception ex)
            {
            }
        }

        public DataTable GetUserInfoByCurType(string fcurtype)
        {
            string errMsg = "";
            DataTable dt = new DataTable();

            string req = "MSG_NO=" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            req += "&uid=298684980";
            if (fcurtype == "1")
            {
                req += "&curtype=" + fcurtype;
                DataSet userDs = CommQuery.GetDSForServiceFromICE(req, "qry_user_balance_slave_c", false, out errMsg);
                if (userDs != null && userDs.Tables.Count > 0 && userDs.Tables[0] != null && userDs.Tables[0].Columns.Count > 0)
                {
                    dt = userDs.Tables[0].Copy();
                    #region
                    dt.Columns["uid"].ColumnName = "Fuid";
                    dt.Columns["curtype"].ColumnName = "Fcurtype";
                    dt.Columns["balance"].ColumnName = "Fbalance";
                    dt.Columns["con"].ColumnName = "Fcon";
                    dt.Columns["state"].ColumnName = "Fstate";
                    dt.Columns["standby1"].ColumnName = "Fstandby1";
                    dt.Columns["standby2"].ColumnName = "Fstandby2";
                    dt.Columns["modify_time"].ColumnName = "Fmodify_time";
                    dt.Columns["user_type"].ColumnName = "Fuser_type";
                    dt.Columns["qqid"].ColumnName = "Fqqid";
                    dt.Columns["truename"].ColumnName = "Ftruename";
                    dt.Columns["company_name"].ColumnName = "Fcompany_name";
                    dt.Columns["login_ip"].ColumnName = "Flogin_ip";
                    dt.Columns["memo"].ColumnName = "Fmemo";
                    dt.Columns["create_time"].ColumnName = "Fcreate_time";
                    dt.Columns["bpay_state"].ColumnName = "Fbpay_state";
                    #endregion
                }
            }
            else // 子账户余额查询
            {
                req += "&curtype_list=" + fcurtype;
                DataSet userDs = CommQuery.GetDSForServiceFromICE(req, "qry_subuser_balance_c", false, out errMsg);
                if (userDs != null && userDs.Tables.Count > 0 && userDs.Tables[0] != null && userDs.Tables[0].Columns.Count > 0)
                {
                    dt = userDs.Tables[0].Copy();
                    #region
                    dt.Columns["Fbalance_"+fcurtype].ColumnName = "Fbalance";
                    dt.Columns["Fcon_" + fcurtype].ColumnName = "Fcon";
                    dt.Columns["Fstate_" + fcurtype].ColumnName = "Fstate";
                    #endregion
                }
            }
            return dt;
        }
    }
}
