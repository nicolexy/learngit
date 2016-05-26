using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KF_Test.Util;
using System.Xml;
namespace KF_Test.RealName
{
    [TestClass]
    public class RealNameCertificationServiceTest
    {
        [TestMethod]
        public void AuMaintainWhiteListC()
        {
            //uid: 10556498 uin  1367226630@wx.tenpay.com op_type =1 
            string req = "src=kf.cf.com&operator=test&uin=1367226630@wx.tenpay.com&uid=10556498&op_type=1&valid_days=2";
            XmlDocument ret = UtilTool.testAPI("RealNameCertificationService.asmx", "AuMaintainWhiteListC", req);
            Assert.IsTrue(ret.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }
        [TestMethod]
        public void BindQryBindMaskInfoC()
        {
            //uin:343216739 uid:1100852476
            string req = "operator=test&uid=33431975";
            XmlDocument ret = UtilTool.testAPI("RealNameCertificationService.asmx", "BindQryBindMaskInfoC", req);
            Assert.IsTrue(ret.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        [TestMethod]
        public void AuQryAuthStatusService()
        {
            //第一种query_type = 0 request_detail_info=0  query_type,sp_id,cre_id,cre_type,truename,uin
        
            //第二种query_type = 0 request_detail_info=1  query_type, sp_id, cre_id, cre_type, truename, uin, src, Operator, sign, request_detail_info

            //第三种query_type = 1 request_detail_info=0  query_type,uin,uid 

            //第四种query_type = 1 request_detail_info=1  query_type,uin,uid,request_detail_info,src,Operator,sign
            //req = "uin=400000001&uid=10556498&query_type=1&request_detail_info=1&src=test&operator=test&sign=082cfa719f5e837a2409ce79b0fa378b"

            string req = "uin=1367226630@wx.tenpay.com&uid=10556498&query_type=1&request_detail_info=1&src=kf.cf.com&operator=test";
            XmlDocument ret = UtilTool.testAPI("RealNameCertificationService.asmx", "AuQryAuthStatusService", req);
            Assert.IsTrue(ret.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        [TestMethod]
        public void PQueryUserInfoService()
        {   //uin：woodydai1@wx.tenpay.com uid：100323812 /uid: 10556498 uin  1367226630@wx.tenpay.com
            string req = "uin=1367226630@wx.tenpay.com&uid=10556498&query_attach=QUERY_USERINFO&curtype=1";
            XmlDocument ret = UtilTool.testAPI("RealNameCertificationService.asmx", "PQueryUserInfoService", req);
            Assert.IsTrue(ret.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        [TestMethod]
        public void PQueryCreRelationC()
        {
            // "cre_type=1&cre_id=kKtlcGEzXrcTkb97WJjUh9LigsUrG2eU" 1 creid=360782198812120002"
            string req = "cre_id=14072819870509867X&cre_type=1";
            XmlDocument ret = UtilTool.testAPI("RealNameCertificationService.asmx", "PQueryCreRelationC", req);
            Assert.IsTrue(ret.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }

        [TestMethod]
        public void QuotaBanQueryC()
        {

            //uid_type=2&uid=1234512346&cre_type=1&cre_id=6bf3a668d47551976bf3a668d4755122&have_cre_photocopy=1"  creid=360782198812120002
            //uid_type=2&uid=10556498&cre_type=1&cre_id=14072819870509867X&have_cre_photocopy=1
            //uid_type=0&uid=10556498&have_cre_photocopy=1
            string req = "uid_type=2&uid=10556498&cre_type=1&cre_id=14072819870509867X&have_cre_photocopy=1";
            XmlDocument ret = UtilTool.testAPI("RealNameCertificationService.asmx", "QuotaBanQueryC", req);
            Assert.IsTrue(ret.SelectSingleNode("/root/return_code").FirstChild.InnerText == "20000");
        }
    }
}
