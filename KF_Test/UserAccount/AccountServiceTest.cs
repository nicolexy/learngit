using System;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using CFT.CSOMS.BLL.CFTAccountModule;
using CFT.CSOMS.COMMLIB;
using CFT.CSOMS.DAL.CFTAccount;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KF_Test
{
    [TestClass]
    public class AccountServiceTest
    {
        /// <summary>
        /// 会员信息查询接口测试
        /// </summary>
        [TestMethod]
        public void TestQueryVipInfo()
        {
            string uin = "19751515";
            AccountService mer = new AccountService();
            DataTable dt = mer.QueryVipInfo(uin);
            Assert.AreEqual("0", dt.Rows[0]["result"].ToString());

        }

        [TestMethod]
        public void TestDecrypt3DES()
        {
            string srcstring = "6292B13DBE70B45B";
            string strKey = "ba6f0d572c4c18e0ae9e4d915a56a1cd299677456testkef";
            Assert.AreEqual("支付七", CommUtil.TripleDESDecryptRealName(srcstring, strKey));
        }

        /// <summary>
        /// 用户实名信息查询加、解密算法
        /// </summary>
        [TestMethod()]
        public void TripleDESRealNameTest()
        {
            string source = "支付七";
            string key = "ba6f0d572c4c18e0ae9e4d915a56a1cd299677456testkef";
            string encryptStr = CommUtil.TripleDESEncryptRealName(source, key);
            string decryptStr = CommUtil.TripleDESDecryptRealName(encryptStr, key);

            Assert.AreEqual(source, decryptStr);
        }

        [TestMethod]
        public void QueryRealNameInfoTest()
        {
            string uin = "1417652838";
            string submit_user = "echoliao";
            AccountData acc = new AccountData();
            DataSet ds = acc.QueryRealNameInfo(uin, submit_user);
            Assert.AreEqual("0", ds.Tables[0].Rows[0]["result"].ToString());
        }

    }
    
}
