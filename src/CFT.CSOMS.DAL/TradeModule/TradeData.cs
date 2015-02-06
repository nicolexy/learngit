using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.CSOMS.DAL.CFTAccount;
using CommLib;

namespace CFT.CSOMS.DAL.TradeModule
{
    public class TradeData
    {
        //注销前交易查询
        public DataSet BeforeCancelTradeQuery(string uid)
        {
            long uidL = long.Parse(uid);
            int uidEnd = int.Parse(uid.Substring(uid.Length - 2));
            string conString = "";
            //标记分机器的方式：uidSize 大于1.8亿一个库，小于另一个库； uidEnd 尾号 00-49 在一个库 50-99 在另外一个库
            string mark = "uidSize";
            try
            {
                mark = System.Configuration.ConfigurationManager.AppSettings["BankRollList_mark"].ToString();
            }
            catch
            { }

            //原来的用户数据库分成二个，uid<1.8亿的在DB1，uid>=1.8亿的在DB2。
            //现在，用户数据库要分成四个，uid后二位 < 25 的在 DB1， 
            //25 <= uid后二位 < 50 的在 DB2,
            //50 <= uid后二位 < 75 的在 DB3,
            //75 <= uid后二位 <= 99 的在 DB4,
            if (mark == "uidSize")
            {
                if (uidL < 180000000)
                    conString = "BankRollList1";
                else
                    conString = "BankRollList2";
            }
            if (mark == "uidEnd")
            {
                if (uidEnd < 25)
                    conString = "BankRollList1";
                else if (uidEnd >= 25 && uidEnd < 50)
                    conString = "BankRollList2";
                else if (uidEnd >= 50 && uidEnd < 75)
                    conString = "BankRollList3";
                else if (uidEnd >= 75 && uidEnd <= 99)
                    conString = "BankRollList4";
            }
            using (var da = MySQLAccessFactory.GetMySQLAccess(conString))
            {
                da.OpenConn();
                string tableStr = PublicRes.GetTableNameUid("t_bankroll_list", uid);
                string Sql = "Select * from  " + tableStr + " where Fuid='"+uid+"' Order by Fmodify_time DESC limit 1";
                DataSet ds = da.dsGetTotalData(Sql);
                return ds;
            }
        }

        public int RemoveControledFinLogInsert(string qqid, string FbalanceStr, string FtypeText, string cur_type, DateTime ApplyTime, string ApplyUser)
        {
            using (var da = CommLib.DbConnectionString.Instance.GetConnection("DataSource_ht"))
            {
                da.OpenConn();

                string Sql = "insert into  c2c_fmdb.t_log_ConFinRemove (Fuin ,FbalanceStr,FtypeText,FcurType ,FmodifyTime ,FupdateUser) " +
                        "values('" + qqid + "','" + FbalanceStr + "','" + FtypeText + "','" + cur_type + "','" + ApplyTime.ToString() + "','" + ApplyUser + "')";
               return da.ExecSqlNum(Sql);
            }
        }
        public DataSet RemoveControledFinLogQuery(string qqid)
        {
            using (var da = CommLib.DbConnectionString.Instance.GetConnection("DataSource_ht"))
            {
                da.OpenConn();

                string Sql = "Select * from  c2c_fmdb.t_log_ConFinRemove where Fuin='" + qqid + "' Order by FmodifyTime DESC";
                DataSet ds = da.dsGetTotalData(Sql);
                return ds;
            }
        }
    }
}
