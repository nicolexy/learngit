using System;
using CFT.CSOMS.DAL.UserAppealModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KF_Test.UserApeal
{
    [TestClass]
    public class UserAppealTest
    {
        /// <summary>
        /// 绑定、改绑手机发风控验证接口测试
        /// </summary>
        [TestMethod]
        public void TestVerifyMobile()
        {
            string uid = "253649253",
                uin = "19751515",
                old_mobile = "12345678901",
                new_mobile = "15814497048",
                client_ip = "127.0.0.1",
                certno = "441802198403013232";
            UserAppealData userAppeal = new UserAppealData();
            bool result=userAppeal.VerifyMobile(uid, uin, old_mobile, new_mobile, client_ip, certno);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// 绑定、改绑手机发风控通知接口测试
        /// </summary>
        [TestMethod]
        public void TestSendMobile()
        {
            string uid = "253649253",
              uin = "19751515",
                old_mobile = "12345678901",
                new_mobile = "15814497048",
                client_ip = "127.0.0.1",
                certno = "441802198403013232"; 
            UserAppealData userAppeal = new UserAppealData();
            userAppeal.SendMobile(uid, uin, old_mobile, new_mobile, client_ip, certno);
        }

    }
}
