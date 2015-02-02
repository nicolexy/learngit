using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KF_Test.Util
{
    class TestUtil
    {
        public static XmlDocument testBaseInfoService(string interfaceName, string queryString)
        {
            string serviceName = "BaseInfoService.asmx";
            return UtilTool.testAPI(serviceName, interfaceName, queryString);
        }

        public static XmlDocument testPaymentService(string interfaceName, string queryString)
        {
            string serviceName = "PaymentService.asmx";
            return UtilTool.testAPI(serviceName, interfaceName, queryString);
        }

      

    }
}

