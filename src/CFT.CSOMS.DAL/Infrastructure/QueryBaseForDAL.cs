using System;
using System.Configuration;
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Data;
using System.Collections;
using System.IO;

using CFT.CSOMS.DAL.Infrastructure;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace CFT.CSOMS.DAL.Infrastructure
{
    public class QueryInfo
    {
        /// <summary>
        /// 从SELECT语句中返回一个DATATABLE。方便.NET中WEB页的显示。
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static DataSet GetTable(string strSql, int iPageStart, int iPageMax)
        {
            DataSet ds = new DataSet();
                using (var da = MySQLAccessFactory.GetMySQLAccess("YWB"))
                {
                    da.OpenConn();
                    //				result = da.GetTable(strSql);
                    ds = da.dsGetTableByRange(strSql, iPageStart, iPageMax);

                    return ds;
                }
        }

        public static DataSet GetTable(string strSql, int iPageStart, int iPageMax, string dbStr)
        {
            DataSet ds = new DataSet();
            using (var da = MySQLAccessFactory.GetMySQLAccess(dbStr))
                {
                    da.OpenConn();
                    ds = da.dsGetTableByRange(strSql, iPageStart, iPageMax);
                   // da.Dispose();
                    return ds;
                }
        }

        public static DataSet GetTable_Conn(string strSql, int iPageStart, int iPageMax, string connstr)
        {
            MySqlAccess da = new MySqlAccess(connstr);
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTableByRange(strSql, iPageStart, iPageMax);

                da.Dispose();
                return ds;
            }
            catch (Exception e)
            {
                da.Dispose();
                throw e;
            }
        }

        public static int CHAR_HASH(char c)
        {
            return (c - '0') % 10;
        }

        public static string GetDbNum(string uin)
        {
            //是QQ号
            if (uin.Length >= 5 && uin.Length <= 10 & !uin.StartsWith("0"))
            {
                return uin;
            }

            //email用户,一般不会低于5位
            if (uin.Length < 5)
            {
                return "00000";
            }
            char[] chUin = uin.ToCharArray();
            // Email：前2位分库
            int iDb = CHAR_HASH(chUin[0]) * 10 + CHAR_HASH(chUin[1]);

            //如果CHAR_HASH 返回负数，取绝对值
            if (iDb < 0)
            {
                iDb = -iDb;
            }

            // Email：第3位分表
            int iTbl = CHAR_HASH(chUin[2]);


            //如果CHAR_HASH 返回负数，取绝对值
            if (iTbl < 0)
            {
                iTbl = -iTbl;

            }

            //主机
            int iHost = CHAR_HASH(chUin[3]) * 10 + CHAR_HASH(chUin[4]);

            //如果CHAR_HASH 返回负数，取绝对值
            if (iHost < 0)
            {
                iHost = -iHost;
            }
            int sum = iDb + iTbl * 100 + iHost * 1000;
            return sum.ToString();
        }

        public static int GetDBNode(string uin)
        {
            int m_uDBNodeCount = 3;//三台机器
            int uNode = Int32.Parse(uin) % 1367 % m_uDBNodeCount;
            return uNode;
        }

        #region 从数据表中转换出来值（同时进行NULL处理）
        public static string GetInt(object aValue)
        {
            try
            {
                if (aValue != null)
                {
                    long num = long.Parse(aValue.ToString());
                    return num.ToString();
                }
                else
                {
                    return "0";
                }
            }
            catch
            {
                return "0";
            }
        }

        public static string GetString(object aValue)
        {
            try
            {
                if (aValue != null)
                {
                    return aValue.ToString().Replace("\\", "\\\\").Replace("'", "\\'");
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }


        public static string GetDateTime(object aValue)
        {
            try
            {
                if (aValue != null)
                {
                    return (DateTime.Parse(aValue.ToString())).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }
        #endregion
    }

    public class Query_BaseForNET
    {
        protected string fstrSql;
        protected string fstrSql_count;

        protected string ICESql;
        protected string ICEcommand;
        /// <summary>
        /// 提供给.net调用的函数
        /// </summary>
        /// <returns></returns>
        public DataSet GetResultX(int iPageStart, int iPageMax)
        {
            //			DataSet result = new DataSet();
            //			result.Tables.Add(QueryInfo.GetTable(fstrSql));
            return QueryInfo.GetTable(fstrSql, iPageStart, iPageMax);
        }

        /// <summary>
        /// 提供给.net调用的函数
        /// </summary>
        /// <returns></returns>
        public DataSet GetResultX(int iPageStart, int iPageMax, string dbstr)
        {
            return QueryInfo.GetTable(fstrSql, iPageStart, iPageMax, dbstr);
        }
        
        //返回全部或者部分数据
        public DataSet GetResultX_AllAndLimit(int iPageStart, int iPageMax, string dbstr)
        {
            if (iPageStart == -1 && iPageMax == -1)
                return PublicRes.returnDSAll(fstrSql, dbstr);
            else
                return QueryInfo.GetTable(fstrSql, iPageStart, iPageMax, dbstr);
        }

        public DataSet GetResultX_Conn(int iPageStart, int iPageMax, string connstr)
        {
            //			DataSet result = new DataSet();
            //			result.Tables.Add(QueryInfo.GetTable(fstrSql));
            return QueryInfo.GetTable_Conn(fstrSql, iPageStart, iPageMax, connstr);
        }

        //public DataSet GetResultX()
        //{
        //    return PublicRes.returnDSAll(fstrSql, "ywb");
        //}

        public DataSet GetResultX_ICE()
        {
            string errmsg = "";
            DataSet ds = CommQuery.GetDataSetFromICE(ICESql, ICEcommand, out errmsg);
            return ds;
        }

        // 2012/4/9
        public DataSet GetResultX_ICE(string serviceName)
        {
            string errmsg = "";
            DataSet ds = CommQuery.GetDataSetFromICE(ICESql, ICEcommand, false, serviceName, out errmsg, false);

            return ds;
        }

        public DataSet GetResultX(string dbstr)
        {
            return PublicRes.returnDSAll(fstrSql, dbstr);
        }

        public DataSet GetResultX_Conn(string connstr)
        {
            return PublicRes.returnDSAll_Conn(fstrSql, connstr);
        }
        public int GetCount()
        {
            return Int32.Parse(PublicRes.ExecuteOne(fstrSql_count, "ywb"));
        }

        public int GetCount(string dbstr)
        {
            return Int32.Parse(PublicRes.ExecuteOne(fstrSql_count, dbstr));
        }

        public int GetCountApeal(string dbstr)
        {
            if (fstrSql_count == "")
                return 0;
            else
                return Int32.Parse(PublicRes.ExecuteOne(fstrSql_count, dbstr));
        }

        public int GetCount_Conn(string connstr)
        {
            return Int32.Parse(PublicRes.ExecuteOne_Conn(fstrSql_count, connstr));
        }
    }


}