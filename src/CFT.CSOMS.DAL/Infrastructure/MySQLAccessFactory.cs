using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CFT.CSOMS.DAL.Infrastructure
{
    using TENCENT.OSS.CFT.KF.DataAccess;
    using CommLib;

    public class MySQLAccessFactory
    {
        public static MySqlAccess GetMySQLAccess(string dataSourceName)
        {
            return DbConnectionString.Instance.GetConnection(dataSourceName);
        }
    }
}
