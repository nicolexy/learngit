using System;
using System.Data;
using CFT.CSOMS.BLL.CFTAccountModule;
using CFT.CSOMS.BLL.SPOA;
using CFT.CSOMS.COMMLIB;
using CFT.CSOMS.DAL.LifeFeePaymentModule;
using CFT.CSOMS.DAL.SPOA;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KF_Test
{
    [TestClass]
    public class LifeFeePaymentTest
    {
        /// <summary>
        /// 非qq用户，是用这个函数计算结果的后3位数字分库表
        /// </summary>
        [TestMethod]
        public void GetUinKeyTest()
        {
            LifeFeePaymentData life= new LifeFeePaymentData();
            Assert.AreEqual(1206,life.GetUinKey("466678748@163.com"));
            Assert.AreEqual(1572, life.GetUinKey("echo@qq.com"));
        }

        [TestMethod]
        public void IsNumTest()
        {
            LifeFeePaymentData life = new LifeFeePaymentData();
            Assert.AreEqual(true, life.IsNum("3452346"));
            Assert.AreEqual(false, life.IsNum("s1212@qq.com"));
        }
    }
}
