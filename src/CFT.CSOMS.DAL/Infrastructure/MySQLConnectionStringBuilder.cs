using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CFT.CSOMS.DAL.Infrastructure
{
    /// <summary>
    /// 数据库连接字符串生成类
    /// 配置文件中的数据库key，需要合适的前缀格式
    /// 数据源地址：DataSource_SPOA;数据库：DataBase_SPOA";用户名："UserID_SPOA";密码:"Password_SPOA";连接字符集："ConnCharSet_SPOA"
    /// </summary>
    public class MySQLConnectionStringBuilder
    {
        public static string GetConnSting(string dataSourceName)
        {
            if (string.IsNullOrEmpty(dataSourceName))
                throw new ArgumentNullException("dataSourceName");

            var connStringStruct = new MySQLConnectionStringStruct().LoadFromConfigFile(dataSourceName);

            var connStringTemplate = GetConnectionStringTemplate(connStringStruct);

            return ComposeConnectionString(connStringTemplate, connStringStruct);
            
 
        }

        protected static string GetConnectionStringTemplate(MySQLConnectionStringStruct connStrStruct)
        {
            var template = "Driver=[MySQL ODBC 3.51 Driver]; Server={0}; Database={1}; UID={2}; PWD={3}; charset={4}; Option=3";

            switch (connStrStruct.MySQLVersion)
            {
                case MySQLVersionEnum.V52A:
                    template =  "Driver=[mysql ODBC 5.2a Driver]; Server={0}; Database={1}; UID={2}; PWD={3};charset={4}; Option=3";
                    break;
            }

            return template;
        }

        protected static string ComposeConnectionString(string template, MySQLConnectionStringStruct connStrStruct)
        {
            return string.Format(template,
                connStrStruct.DataSource,
                connStrStruct.DataBase,
                connStrStruct.UserName,
                connStrStruct.PassWord,
                connStrStruct.MySQLVersion);
        }
    }
}
