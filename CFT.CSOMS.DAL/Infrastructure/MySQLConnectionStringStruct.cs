using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CFT.CSOMS.DAL.Infrastructure
{
    public struct MySQLConnectionStringStruct
    {
        public string DataSource { get; set; }

        public string DataBase { get; set; }

        public string UserName { get; set; }

        public string PassWord { get; set; }

        public string ConnectionCharacterSet { get; set; }

        public MySQLVersionEnum MySQLVersion { get; set; }

        public MySQLConnectionStringStruct LoadFromConfigFile(string dataSourceName)
        {
            string dataSourceKey = "DataSource_" + dataSourceName,
                    dataBaseKey = "DataBase_" + dataSourceName,
                    userNameKey = "UserID_" + dataSourceName,
                    passwordKey = "Password_" + dataSourceName,
                    connectionCharSetKey = "ConnCharSet_" + dataSourceName;

            DataSource = ConfigurationManager.AppSettings[dataSourceKey];
            if (string.IsNullOrEmpty(DataSource))
                throw new Exception(string.Format("读取配置文件中{0}的key:{1}异常", dataSourceName, dataSourceKey));

            DataBase = ConfigurationManager.AppSettings["DataBase_" + dataSourceName] ?? "mysql";

            UserName = ConfigurationManager.AppSettings["UserID_" + dataSourceName];
            if (string.IsNullOrEmpty(UserName))
                throw new Exception(string.Format("读取配置文件中{0}的key:{1}异常", dataSourceName, userNameKey));

            PassWord = ConfigurationManager.AppSettings["Password_" + dataSourceName];
            if (string.IsNullOrEmpty(PassWord))
                throw new Exception(string.Format("读取配置文件中{0}的key:{1}异常", dataSourceName, passwordKey));

            ConnectionCharacterSet = ConfigurationManager.AppSettings["ConnCharSet_" + dataSourceName] ?? "latin1";

            //MySQLVersion
            var db50List = (ConfigurationManager.AppSettings["DB50List"] ?? string.Empty)
                                .Split(new char[]{';'} , StringSplitOptions.RemoveEmptyEntries);

            MySQLVersion = (db50List.Count() > 1 && db50List.Contains(DataSource)) ? MySQLVersionEnum.V52A : MySQLVersionEnum.V351;
            

            return this;
        }

    }

    public enum MySQLVersionEnum
    {
        V351,V52A
    }
}
