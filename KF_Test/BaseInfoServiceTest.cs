using CFT.CSOMS.Service.CSAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Xml;
using KF_Test.Util;

namespace KF_Test
{
    /// <summary>
    /// 基本信息查询模块API测试
    /// </summary>
    [TestClass()]
    public class BaseInfoServiceTest
    {
        [TestMethod]
        public void TestGetToken()
        {
            string queryString = "appid=10001&uin=273912258&user_bank_id=&bank_type=0&token=6c8e9568f3699e079667c4a748da64ae";
            queryString = "appid=10001&uin=273912258&start_date=2014-10-15%2000%3A00%3A00&end_date=2014-10-29%2000%3A00%3A00&state=99&type=99&qqtype=&dotype=9&offset=0&limit=10&token=4448514d9c9cec247570ed06dfae2e0c";
            queryString= "appid=10001&fid=20380400&user=echo&user_ip=127.0.0.1&comment=测试&db=&tb=&token=a38672e4f9c8d8cd33b143d2afe90bdd";
            queryString = "appid=10001&fid=2014100002586451&db=db_appeal_2014&tb=t_tenpay_appeal_trans_10&token=97883af5ed5fa37e0a7c895e84257482";
            
            string token = UtilTool.GetToken(queryString);
        }


        #region 自助申诉查询测试
        [TestMethod]
        public void TestGetUserAuthenState()
        {
            string queryString = "appid=10001&uin=453294603&user_bank_id=&bank_type=0&token=6c8e9568f3699e079667c4a748da64ae";
            XmlDocument answer = TestUtil.testBaseInfoService("GetUserAuthenState", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        [TestMethod]
        public void TestGetAppealUserInfo()
        {
            string queryString = "appid=10001&uin=453294603&token=cf0eb513c99e9e6c12ea554bd034a7fd";
            XmlDocument answer = TestUtil.testBaseInfoService("GetAppealUserInfo", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        [TestMethod]
        public void TestGetUserAppealList()
        {
            string queryString = "appid=10001&uin=453294603&start_date=2014-01-01 00:00:00&end_date=2014-08-28 00:00:00&state=99&type=99&qqtype=&dotype=9&offset=0&limit=10&token=f099b4989238ef1e4af4530f83cd6b33";
            XmlDocument answer = TestUtil.testBaseInfoService("GetUserAppealList", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        [TestMethod]
        public void TestGetUserAppealDetail()
        {
            string queryString = "appid=10001&fid=2014040000003527&db=db_appeal_2014&tb=t_tenpay_appeal_trans_04&token=1df91d89b5ac714db3df49f2876e40d5";
            XmlDocument answer = TestUtil.testBaseInfoService("GetUserAppealDetail", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        [TestMethod]
        public void TestDelAppeal()
        {
            string queryString = "appid=10001&fid=2310&user=echo&user_ip=127.0.0.1&comment=测试&db=&tb=&token=a38672e4f9c8d8cd33b143d2afe90bdd";
            XmlDocument answer = TestUtil.testBaseInfoService("DelAppeal", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        [TestMethod]
        public void TestCannelAppeal()
        {
            string queryString = "appid=10001&fid=2310&user=echo&user_ip=127.0.0.1&comment=测试&reason=扫描件整体不清晰&other_reason=&db=&tb=&token=d9a0a167a1d94b10b72058503190d605";
            XmlDocument answer = TestUtil.testBaseInfoService("CannelAppeal", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        [TestMethod]
        public void TestConfirmAppeal()
        {
            string queryString = "appid=10001&fid=2310&user=echo&user_ip=127.0.0.1&comment=测试&db=&tb=&token=a38672e4f9c8d8cd33b143d2afe90bdd";
            XmlDocument answer = TestUtil.testBaseInfoService("ConfirmAppeal", queryString);
            Assert.IsTrue(answer.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }
    }

    #endregion


}
