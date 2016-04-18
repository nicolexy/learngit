using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TENCENT.OSS.CFT.KF.DataAccess;
using CFT.Apollo.Data.ConnectionString;
using CFT.Apollo.Logging;
using System.Diagnostics;

namespace CommLib
{
    public class DbConnectionString
    {

        #region  单例
        private static DbConnectionString _instance;

        private static readonly object _locker = new object();

        public static DbConnectionString Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new DbConnectionString();
                        }
                    }
                }
                return _instance;
            }
        }
        private DbConnectionString()
        {
        }
        #endregion

        /// <summary>
        /// 获取连接对象
        /// </summary>
        /// <param name="dataSourceName">数据库别名</param>
        /// <returns></returns>
        public MySqlAccess GetConnection(string dataSourceName)
        {
            var connectionString = GetConnectionString(dataSourceName);
            return new MySqlAccess(connectionString);
        }

        /// <summary>
        /// 获取连接字符串k
        /// </summary>
        /// <param name="dataSourceName">数据库别名</param>
        /// <returns></returns>
        public string GetConnectionString(string dataSourceName)
        {
            var connectionString = string.Empty;
            if (!string.IsNullOrEmpty(dataSourceName))
            {
                try
                {
                    #if DEBUG
                    connectionString = "Driver={mysql ODBC 5.2a Driver}; Database=mysql;Server=10.12.72.253;UID=root;PWD=root1234;charset=latin1;Option=3";
                    #else
                    connectionString = ConnectionStringHelper.Get(dataSourceName);
                    #endif

                }
                catch (Exception ex)
                {
                    LogHelper.LogInfo("获取连接字符串异常:" + ex.ToString());
                }
            }
            return connectionString;

        }
    }
}
