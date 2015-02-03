using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using CFT.Apollo.Common.Cryptography;
using CFT.Apollo.CommunicationFramework;
using CFT.CSOMS.BLL.SPOA;
using CFT.CSOMS.BLL.WechatPay;
using CFT.CSOMS.DAL.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace KF_Test
{
    [TestClass]
    public class FastPayServiceTest
    {
        [TestMethod]
        public void QueryBankCardListTest()
        {
            string bankCard = BankLib.BankIOX.DecryptNoPadding("8Qz189FjLX5CU24z_L2yWw==");
            int biz_type = 10100;
            string bankDate = "20140929";
            int limit = 10;
            int offset = 0;
            FastPayService fast = new FastPayService();
            DataSet ds = fast.QueryBankCardList(bankCard, bankDate, biz_type, offset, limit);
            Assert.AreEqual("0", ds.Tables[0].Rows[0]["result"].ToString());
        }

        //[TestMethod]
        //public void QueryWechatHBSendTest()
        //{
        //    string send_openid = "onqOjjqoNArgAnXUC-g2QyPfL9fQ";
        //    string req = "MSG_NO=1003361" + DateTime.Now.Ticks.ToString();
        //    req += "&client_ip=12.0.0.1";
        //    req += "&offset=0";
        //    req += "&limit=2";
        //    req += "&op_source=cft_kf_sys";
        //    string sign = "cft_kf_sys" + "2WLTM0CVO9NBG5EQJFPIAH";
        //    sign = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sign, "md5").ToUpper();
        //    req += "&sign="+sign;
        //  string  req2 = "&flag=1";
        //  req2 += "&send_openid=" + send_openid;
        //    //10.208.148.243 28000
        //  DataSet dsSend = RelayAccessFactory.GetDSFromRelayResultXML(req + req2, "100336", "10.12.189.172", 22000);
        //  Assert.AreEqual(send_openid, dsSend.Tables[0].Rows[0]["Fpay_openid"].ToString());

        //    req2 = "&flag=2";
        //    req2 += "&rec_openid=" + send_openid;
        //    DataSet dsRecive = RelayAccessFactory.GetDSFromRelayResultXML(req + req2, "100336", "10.12.189.172", 22000);
        //    Assert.AreEqual(send_openid, dsSend.Tables[0].Rows[0]["Freceive_openid"].ToString());

        //    req2 = "&flag=3";
        //    req2 += "&send_listid=1000000000201412122000000674";
        //    DataSet dsAllSend= RelayAccessFactory.GetDSFromRelayResultXML(req + req2, "100336", "10.12.189.172", 22000);
        //    Assert.AreEqual("1000000000201412122000000674", dsSend.Tables[0].Rows[0]["Fsend_list_id"].ToString());

        //    req2 = "&flag=3";
        //    req2 += "&send_listid=1000000000201412162000001870";
        //    DataSet dsAllRecive = RelayAccessFactory.GetDSFromRelayResultXML(req + req2, "100336", "10.12.189.172", 22000);
        //    Assert.AreEqual("1000000000201412162000001870", dsSend.Tables[0].Rows[0]["Fsend_list_id"].ToString());
        //}

        [TestMethod]
        public void QueryWechatHBSend_Two_Test()
        {
            string send_openid = "onqOjjqoNArgAnXUC-g2QyPfL9fQ";
            DataSet dsSend = GetWebchatHB("send_openid", send_openid,1,"127.0.0.1",0,2);
           
            //需要用这个方法
            // var relayResponse = RelayHelper.CommunicateWithRelay(req, true, "100336", "10.12.189.172", "2000000000");
          //  string answer = Encoding.Default.GetString(relayResponse.ResponseBuffer);
            Assert.AreEqual(send_openid, dsSend.Tables[0].Rows[0]["Fpay_openid"].ToString());
            
            //string req2 = "&flag=2";
            //req2 += "&rec_openid=" + send_openid;
          //  DataSet dsRecive = RelayAccessFactory.GetDSFromRelayResultXML(req + req2, "100336", "10.12.189.172", 22000);
            //Assert.AreEqual(send_openid, dsRecive.Tables[0].Rows[0]["Freceive_openid"].ToString());
            DataSet dsRecive = GetWebchatHB("rec_openid", send_openid, 2, "127.0.0.1", 0, 2);
            Assert.AreEqual(send_openid, dsRecive.Tables[0].Rows[0]["Freceive_openid"].ToString());

            //req2 = "&flag=3";
            //req2 += "&send_listid=1000000000201412122000000674";
            //DataSet dsAllSend = RelayAccessFactory.GetDSFromRelayResultXML(req + req2, "100336", "10.12.189.172", 22000);
            //Assert.AreEqual("1000000000201412122000000674", dsAllSend.Tables[0].Rows[0]["Fsend_list_id"].ToString());
            DataSet dsAllSend = GetWebchatHB("send_listid", "1000000000201412122000000674", 3, "127.0.0.1", 0, 2);
            Assert.AreEqual("1000000000201412122000000674", dsAllSend.Tables[0].Rows[0]["Fsend_list_id"].ToString());



            //req2 = "&flag=3";
            //req2 += "&send_listid=1000000000201412162000001870";
            //DataSet dsAllRecive = RelayAccessFactory.GetDSFromRelayResultXML(req + req2, "100336", "10.12.189.172", 22000);
            //Assert.AreEqual("1000000000201412162000001870", dsAllRecive.Tables[0].Rows[0]["Fsend_list_id"].ToString());
            DataSet dsAllRecive = GetWebchatHB("send_listid", "1000000000201412162000001870", 3, "127.0.0.1", 0, 2);
            Assert.AreEqual("1000000000201412162000001870", dsAllRecive.Tables[0].Rows[0]["Fsend_list_id"].ToString());
        
        
        }

        private DataSet GetWebchatHB(string paramName, string send_openid, int flag, string client_ip,int offset, int limit)
        {
            
            string op_source = "cft_kf_sys";
            string key = "2WLTM0CVO9NBG5EQJFPIAH";

            Dictionary<string, string> paramsDic = new Dictionary<string, string>();
         //   paramsDic.Add("MSG_NO", "1003361" + DateTime.Now.Ticks.ToString());//特殊的大写拎出来
          //  paramsDic.Add("head_u", "");
            paramsDic.Add("sp_id", "2000000000");
            paramsDic.Add("ver", "1");
            paramsDic.Add("request_type", "100336");
            paramsDic.Add("client_ip", client_ip);
            paramsDic.Add("op_source", op_source);
            paramsDic.Add("offset", offset.ToString());
            paramsDic.Add("limit", limit.ToString());
            paramsDic.Add("flag", flag.ToString());
            paramsDic.Add(paramName, send_openid);
            //paramsDic.Add("sign", "");
            Dictionary<string, string> paramsAsc = DictionarySort(paramsDic);
            string paramsStr = "MSG_NO=" +"1003361" + DateTime.Now.Ticks.ToString();
            foreach (var d in paramsAsc)
            {
                paramsStr +="&"+ d.Key + "=" + d.Value ;
            }
            string signStr = paramsStr + "&key=" + op_source + key;
            string sign= MD5Helper.Encrypt(signStr).ToUpper();
            //10.208.148.243 28000
            DataSet dsSend = RelayAccessFactory.GetDSFromRelayFromXML(paramsStr + "&head_u=" + "&sgin=" + sign, "10.12.189.172", 22000);
            return dsSend;
        }

        private Dictionary<string, string> DictionarySort(Dictionary<string, string> dic)
        {
            Dictionary<string, string> dicASC = new Dictionary<string, string>();
            if (dic.Count > 0)
            {
                List<KeyValuePair<string, string>> lst = new List<KeyValuePair<string, string>>(dic);
                lst.Sort(delegate(KeyValuePair<string, string> s1, KeyValuePair<string, string> s2)
                {
                  return s1.Key.CompareTo(s2.Key);
                     
                });
                dic.Clear();

                foreach (KeyValuePair<string, string> kvp in lst)
                    dicASC.Add(kvp.Key, kvp.Value);
            }

           return dicASC;
        }

        

      
    }
}
