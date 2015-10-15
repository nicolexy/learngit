using CFT.CSOMS.DAL.CheckModoule;
using CFT.CSOMS.DAL.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.C2C.Finance.DataAccess;

namespace CFT.CSOMS.DAL.Tests
{
    [TestClass]
    public class FinishCheckDataTest
    {
        [TestMethod]
        public void UPDATE_USERNAME_Test()
        {
            //string setID = PublicRes.GetSetIDByQQID("298684980");
            string cCompany = "阿斯蒂芬";
            string changedName = "asdf张三";

            string errMsg = "";

            string req = "CMD=UPDATE_USERNAME";
            req += "&MSG_NO=" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            

            string reqMd5 = "fuid=298684980";
            reqMd5 += "&fcurtype=1";
            reqMd5 += "&acc_set=2";
            if (!string.IsNullOrEmpty(cCompany))
            {
                reqMd5 += "&fcompany_name=" + cCompany;
            }
            reqMd5 += "&ftruename=" + changedName;
            reqMd5 += "&fmodify_time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            req += "&" + reqMd5;
            req += "&token=" + md5(reqMd5 + "16894168");

            req = req.Replace(changedName,ICEAccess.URLEncode(changedName));
            if (!string.IsNullOrEmpty(cCompany))
            {
                req = req.Replace(cCompany, ICEAccess.URLEncode(cCompany));
            }
            
            string reply = string.Empty;
            short sresult;
            //YWCommandCode.修改用户信息
            if (commRes.middleInvoke("ui_mod_user_acc_c", req, true, out reply, out sresult, out errMsg))
            {
                if (sresult != 0)
                {
                    throw new Exception("修改个人姓名出错！：result=" + sresult + "，msg=" + errMsg + "&reply=" + reply);
                }
                else
                {
                    if (reply.StartsWith("result=0"))
                    {
                        
                    }
                }
            }
        }

        [TestMethod]
        public void MDTest()
        {
            string asdf = FormsAuthentication.HashPasswordForStoringInConfigFile("fuid=298684980&fcurtype=1&acc_set=2&ftruename=asdf张三&fmodify_time=2015-11-09 14:25:1416894168", "md5");
            string temp = md5("fuid=298684980&fcurtype=1&acc_set=2&ftruename=asdf张三&fmodify_time=2015-11-09 14:25:1416894168");
        }

        public static String md5(String s)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.GetEncoding("gb2312").GetBytes(s);
            //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);
            bytes = md5.ComputeHash(bytes);
            md5.Clear();

            string ret = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }

            return ret.PadLeft(32, '0').ToUpper();
        }

        [TestMethod]
        public void FREEZE_USER_Test()
        {
            string errMsg = "";

            string req = "CMD=FREEZE_USER";
            req += "&MSG_NO=" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string reqMd5 = "fuid=298684980";
            reqMd5 += "&fcurtype=1";
            reqMd5 += "&acc_set=2";
            reqMd5 += "&fstate=1";
            reqMd5 += "&fmodify_time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            req += "&" + reqMd5;
            req += "&token=" + FormsAuthentication.HashPasswordForStoringInConfigFile(reqMd5 + "16894168", "md5");

            string reply = string.Empty;
            short sresult;
            if (commRes.middleInvoke("ui_mod_user_acc_c", req, true, out reply, out sresult, out errMsg))
            {
                if (sresult != 0)
                {
                    throw new Exception("修改用户状态出错！：result=" + sresult + "，msg=" + errMsg + "&reply=" + reply);
                }
                else
                {
                    if (reply.StartsWith("result=0"))
                    {

                    }
                }
            }
        }

        [TestMethod]
        public void UPDATE_USER_TYPE_Test()
        {
            string errMsg = "";

            string req = "CMD=UPDATE_USER_TYPE";
            req += "&MSG_NO=" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string reqMd5 = "fuid=298684980";
            reqMd5 += "&fcurtype=1";
            reqMd5 += "&acc_set=2";
            reqMd5 += "&fuser_type=2";
            reqMd5 += "&fmodify_time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            req += "&" + reqMd5;
            req += "&token=" + FormsAuthentication.HashPasswordForStoringInConfigFile(reqMd5 + "16894168", "md5");

            string reply = string.Empty;
            short sresult;
            if (commRes.middleInvoke("ui_mod_user_acc_c", req, true, out reply, out sresult, out errMsg))
            {
                if (sresult != 0)
                {
                    throw new Exception("修改用户类型出错！：result=" + sresult + "，msg=" + errMsg + "&reply=" + reply);
                }
                else
                {
                    if (reply.StartsWith("result=0"))
                    {

                    }
                }
            }
        }
    }
}
