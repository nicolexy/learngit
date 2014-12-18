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
        /// ��SELECT����з���һ��DATATABLE������.NET��WEBҳ����ʾ��
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
            //��QQ��
            if (uin.Length >= 5 && uin.Length <= 10 & !uin.StartsWith("0"))
            {
                return uin;
            }

            //email�û�,һ�㲻�����5λ
            if (uin.Length < 5)
            {
                return "00000";
            }
            char[] chUin = uin.ToCharArray();
            // Email��ǰ2λ�ֿ�
            int iDb = CHAR_HASH(chUin[0]) * 10 + CHAR_HASH(chUin[1]);

            //���CHAR_HASH ���ظ�����ȡ����ֵ
            if (iDb < 0)
            {
                iDb = -iDb;
            }

            // Email����3λ�ֱ�
            int iTbl = CHAR_HASH(chUin[2]);


            //���CHAR_HASH ���ظ�����ȡ����ֵ
            if (iTbl < 0)
            {
                iTbl = -iTbl;

            }

            //����
            int iHost = CHAR_HASH(chUin[3]) * 10 + CHAR_HASH(chUin[4]);

            //���CHAR_HASH ���ظ�����ȡ����ֵ
            if (iHost < 0)
            {
                iHost = -iHost;
            }
            int sum = iDb + iTbl * 100 + iHost * 1000;
            return sum.ToString();
        }

        public static int GetDBNode(string uin)
        {
            int m_uDBNodeCount = 3;//��̨����
            int uNode = Int32.Parse(uin) % 1367 % m_uDBNodeCount;
            return uNode;
        }

        #region �����ݱ���ת������ֵ��ͬʱ����NULL����
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
        /// �ṩ��.net���õĺ���
        /// </summary>
        /// <returns></returns>
        public DataSet GetResultX(int iPageStart, int iPageMax)
        {
            //			DataSet result = new DataSet();
            //			result.Tables.Add(QueryInfo.GetTable(fstrSql));
            return QueryInfo.GetTable(fstrSql, iPageStart, iPageMax);
        }

        /// <summary>
        /// �ṩ��.net���õĺ���
        /// </summary>
        /// <returns></returns>
        public DataSet GetResultX(int iPageStart, int iPageMax, string dbstr)
        {
            return QueryInfo.GetTable(fstrSql, iPageStart, iPageMax, dbstr);
        }
        
        //����ȫ�����߲�������
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