using CFT.CSOMS.DAL.Infrastructure;
using CommLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TENCENT.OSS.CFT.KF.DataAccess;

namespace CFT.CSOMS.DAL.CFTAccount
{
    public class CertificateData
    {
        /// <summary>
        /// 查询日志
        /// </summary>
        /// <param name="creid"></param>
        /// <returns></returns>
        public DataTable GetClearCreidLog(string creid)
        {
            var tableName = GetTableName("t_creid_Log", creid);
            string sql = string.Format(@"SELECT FCreid,FCreate_time,CASE FUser_type WHEN 0 THEN '普通用户' WHEN 1 THEN '微信用户' END AS FUser_type ,FUid FROM {0}  WHERE FCreid='{1}'", tableName, creid);
            using (var da = MySQLAccessFactory.GetMySQLAccess("ClrearCreidLog"))
            {
                da.OpenConn();
                return da.GetTable(sql);
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="creid"></param>
        /// <param name="userType"></param>
        /// <param name="Uid"></param>
        public void WriteClearCreidLog(string creid, int userType, string Uid)
        {
            var tableName = GetTableName("t_creid_Log", creid);
            string sql = string.Format(@"INSERT INTO {0}(FUid,FCreid,FUser_type) VALUES('{1}','{2}',{3})", tableName, Uid, creid, userType);
            using (var da = MySQLAccessFactory.GetMySQLAccess("ClrearCreidLog"))
            {
                da.OpenConn();
                da.ExecSql(sql);
            }
        }

        /// <summary>
        /// 清理证件号码
        /// </summary>
        /// <param name="creid"></param>
        /// <param name="type">用户类型</param>
        /// <returns></returns>
        public bool ClearCreidInfo(string creid, int type)
        {
            int retNum = 0;
            string tableName = GetTableName("creid_statistics", creid);
            MySqlAccess da = null;
            try
            {
                if (type == 0)
                {
                    //普通用户
                    da = new MySqlAccess(DbConnectionString.Instance.GetConnectionString("NEW_statistics"));    //统计数据库   
                }
                else
                {
                    //微信用户
                    da = MySQLAccessFactory.GetMySQLAccess("comprehensive");    //综合业务数据库 
                }
                da.OpenConn();
                string sql = "update " + tableName + " set count=0 where Fcreid='" + creid + "'";
                retNum = da.ExecSqlNum(sql);
            }
            catch(Exception e)
            {
                throw new Exception("清理失败" + e.ToString());
            }
            finally
            {
                if (da != null)
                    da.Dispose();
            }
            return retNum == 1 ? true : false;
        }

        /// <summary>
        /// 获取清理次数
        /// </summary>
        /// <param name="creid"></param>
        /// <returns></returns>
        public int GetClearCreidCount(string creid, int userType)
        {
            var tableName = GetTableName("t_creid_Log", creid);
            string sql = string.Format(@"SELECT COUNT(FID) FROM {0} WHERE FCreid='{1}' AND FUser_type ={2}", tableName, creid, userType);
            using (var da = MySQLAccessFactory.GetMySQLAccess("ClrearCreidLog"))
            {
                da.OpenConn();
                var temp = da.GetOneResult(sql);
                return temp != null ? Convert.ToInt32(temp) : 0;
            }
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="creid"></param>
        /// <returns></returns>
        private string GetTableName(string tableName, string creid)
        {
            if (creid.Length > 2)
            {
                string s_db = "";
                string l_s = creid.Substring(creid.Length - 1);

                if (l_s.ToUpper() == "X")
                {
                    s_db = "00";
                    tableName = "statistics_db_" + s_db + "." + tableName + "_0";
                }
                else
                {
                    s_db = creid.Substring(creid.Length - 2);
                    tableName = "statistics_db_" + s_db + "." + tableName + "_" + creid.Substring(creid.Length - 3, 1);
                }
            }
            return tableName;
        }
    }
}
