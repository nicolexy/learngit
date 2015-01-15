using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TENCENT.OSS.CFT.KF.DataAccess;
using CFT.Apollo.Data.ConnectionString;

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
        /// 获取连接字符串
        /// </summary>
        /// <param name="dataSourceName">数据库别名</param>
        /// <returns></returns>
        public string GetConnectionString(string dataSourceName)
        {
            if (string.IsNullOrEmpty(dataSourceName))
            {
                throw new ArgumentNullException("dataSourceName");
            }

            var connectionString = ConnectionStringHelper.Get(dataSourceName);

            if (connectionString == null)
            {
                throw new Exception(string.Format("找不到名为\"{0}\"的连接字符串", dataSourceName));
            }

            return connectionString;
        }
    }
}
