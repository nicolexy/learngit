using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CFT.CSOMS.BLL.WechatPay;
using System.Collections;
using CFT.CSOMS.DAL.Infrastructure;

namespace KF_Test
{
    /// <summary>
    /// 取银行名称及对应的bank_type集(格式：2001|2002|2003)。
    /// </summary>
    [TestClass]
    public class BankCardQuery
    {
        private string relayDefaultSPId = "20000000";
        
        [TestMethod]
        public void RequestBankInfo()
        {
            string ip = System.Configuration.ConfigurationManager.AppSettings["BankInfoIP"].ToString();
            int port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["BankInfoPort"].ToString());
            int totalnum;
            //快捷
            string requestString = "biz_type=FASTPAY&query_mode=1&query_type=1&specified_attrs=bank_type|bank_name&limit=200&offset=0";
            DataSet ds = RelayAccessFactory.GetBankInfoFromRelay(out totalnum, requestString, "6508", ip, port, false, false, relayDefaultSPId);

            //一点通
            requestString = "biz_type=ONECLICK&query_mode=1&query_type=1&specified_attrs=bank_type|bank_name&limit=200&offset=0";
            DataSet dsex = RelayAccessFactory.GetBankInfoFromRelay(out totalnum, requestString, "6508", ip, port, false, false, relayDefaultSPId);
        }

        /// <summary>
        /// 查询银行信息
        /// </summary>
        [TestMethod]
        public void RequestBankPosDataList()
        {
            string ip = System.Configuration.ConfigurationManager.AppSettings["QueryBankPosIP"].ToString();
            int port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["QueryBankPosPORT"].ToString());
            string request_type = "2617&ver=1&head_u=&sp_id=2000000501&req_id=1006&card_no=F9L4wI4C0ON0_bGFziy1_g%3D%3D&biz_type=10100&start_time=2015-03-11 01:35:33&end_time=2015-03-11 23:35:33&limit=10&offset=0&table_suffix=20150311&bank_type=3115";
            int totalNum = 0;
            DataSet ds = RelayAccessFactory.GetDSFromRelayRowNumNew(out totalNum, request_type, "2617", ip, port, false, false, relayDefaultSPId);
        }
    }
}
