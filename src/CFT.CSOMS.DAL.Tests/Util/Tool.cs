using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CFT.CSOMS.DAL.Infrastructure;
using System.Collections;
using System.Data;
using CFT.CSOMS.DAL.UserAppealModule;

namespace CFT.CSOMS.DAL.Tests
{
    public class Tool
    {
        /// <summary>
        /// 插入或删除数据
        /// </summary>
        /// <param name="sqlList">sql list</param>
        /// <param name="DBCon"></param>
         public void InsertORDeleteData(ArrayList sqlList, string DBCon)
         {
             using (var da = MySQLAccessFactory.GetMySQLAccess(DBCon))
             {
                 da.OpenConn();
                 foreach (string sql in sqlList)
                 {
                     da.ExecSqlNum(sql);
                 }
             }
         }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="DBCon"></param>
        /// <returns></returns>
         public DataSet QuerData(string sql, string DBCon)
         {
             using (var da = MySQLAccessFactory.GetMySQLAccess(DBCon))
             {
                 da.OpenConn();
                 DataSet ds = da.dsGetTotalData(sql);
                 return ds;
             }
         }

    }
}