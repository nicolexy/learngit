using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CFT.CSOMS.DAL.Infrastructure
{
    using TENCENT.OSS.CFT.KF.DataAccess;

    public class MySQLAccessFactory
    {
        public static MySqlAccess GetMySQLAccess(string dataSourceName)
        {
            if (string.IsNullOrEmpty(dataSourceName))
                throw new ArgumentNullException("dataSourceName");

            var connectionString = ConfigurationManager.ConnectionStrings[dataSourceName];

            if (connectionString == null)
                throw new Exception(string.Format("配置文件中找不到名为\"{0}\"的连接字符串", dataSourceName));

            return new MySqlAccess(connectionString.ConnectionString);
        }
    }
}
