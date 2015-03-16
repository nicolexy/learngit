using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.CSOMS.DAL.CFTAccount;

namespace CFT.CSOMS.DAL.TradeModule
{
    public class TradeData
    {
        //注销前交易查询
        public DataSet BeforeCancelTradeQuery(string uid)
        {
            long uidL = long.Parse(uid);
            int uidEnd = int.Parse(uid.Substring(uid.Length - 2));
            string conString = "";
            //标记分机器的方式：uidSize 大于1.8亿一个库，小于另一个库； uidEnd 尾号 00-49 在一个库 50-99 在另外一个库
            string mark = "uidSize";
            try
            {
                mark = System.Configuration.ConfigurationManager.AppSettings["BankRollList_mark"].ToString();
            }
            catch
            { }

            //原来的用户数据库分成二个，uid<1.8亿的在DB1，uid>=1.8亿的在DB2。
            //现在，用户数据库要分成四个，uid后二位 < 25 的在 DB1， 
            //25 <= uid后二位 < 50 的在 DB2,
            //50 <= uid后二位 < 75 的在 DB3,
            //75 <= uid后二位 <= 99 的在 DB4,
            if (mark == "uidSize")
            {
                if (uidL < 180000000)
                    conString = "BankRollList1";
                else
                    conString = "BankRollList2";
            }
            if (mark == "uidEnd")
            {
                if (uidEnd >= 0 && uidEnd <= 3)
                    conString = "zw1";
                else if (uidEnd >= 4 && uidEnd <= 8)
                    conString = "zw2";
                else if (uidEnd >= 9 && uidEnd <= 12)
                    conString = "zw3";
                else if (uidEnd >= 13 && uidEnd <= 16)
                    conString = "zw4";
                else if (uidEnd >= 17 && uidEnd <= 20)
                    conString = "zw5";
                else if (uidEnd >= 21 && uidEnd <= 24)
                    conString = "zw6";
                else if (uidEnd >= 25 && uidEnd <= 28)
                    conString = "zw7";
                else if (uidEnd >= 29 && uidEnd <= 32)
                    conString = "zw8";
                else if (uidEnd >= 33 && uidEnd <= 36)
                    conString = "zw9";
                else if (uidEnd >= 37 && uidEnd <= 40)
                    conString = "zw10";
                else if (uidEnd >= 41 && uidEnd <= 44)
                    conString = "zw11";
                else if (uidEnd >= 45 && uidEnd <= 48)
                    conString = "zw12";
                else if (uidEnd >= 49 && uidEnd <= 52)
                    conString = "zw13";
                else if (uidEnd >= 53 && uidEnd <= 56)
                    conString = "zw14";
                else if (uidEnd >= 57 && uidEnd <= 60)
                    conString = "zw15";
                else if (uidEnd >= 61 && uidEnd <= 64)
                    conString = "zw16";
                else if (uidEnd >= 65 && uidEnd <= 68)
                    conString = "zw17";
                else if (uidEnd >= 69 && uidEnd <= 72)
                    conString = "zw18";
                else if (uidEnd >= 73 && uidEnd <= 76)
                    conString = "zw19";
                else if (uidEnd >= 77 && uidEnd <= 80)
                    conString = "zw20";
                else if (uidEnd >= 81 && uidEnd <= 84)
                    conString = "zw21";
                else if (uidEnd >= 85 && uidEnd <= 88)
                    conString = "zw22";
                else if (uidEnd >= 89 && uidEnd <= 92)
                    conString = "zw23";
                else if (uidEnd >= 93 && uidEnd <= 96)
                    conString = "zw24";
                else if (uidEnd >= 97 && uidEnd <= 99)
                    conString = "zw25";
            }
            using (var da = MySQLAccessFactory.GetMySQLAccess(conString))
            {
                da.OpenConn();
                string tableStr = PublicRes.GetTableNameUid("t_bankroll_list", uid);
                string Sql = "Select * from  " + tableStr + " where Fuid='" + uid + "' Order by Fmodify_time DESC limit 1";
                DataSet ds = da.dsGetTotalData(Sql);
                return ds;
            }
        }

        /// <summary>
        /// 微信买家纬度用户订单查询
        /// </summary>
        /// <returns></returns>
        public DataSet QueryWxBuyOrderByUid(int uid, DateTime startTime, DateTime endTime)
        {
            //ver=1&head_u=&sp_id=2000000501&request_type=100878&uid=123456&s_time=2015-01-01&e_time=2015-03-01&offset=0&limit=10&icard_flag=0
            string reqString = "uid=" + uid.ToString();
            reqString += "&s_time=" + startTime.ToString("yyyy_MM-dd 00:00:00");
            reqString += "&e_time=" + endTime.ToString("yyyy_MM-dd 23:59:59");
            reqString += "&offset=0";
            reqString += "&limit=10";
            reqString += "&icard_flag=0";
            reqString += "&MSG_NO=100878" + DateTime.Now.Ticks.ToString();

            var serverIp = System.Configuration.ConfigurationManager.AppSettings["WX_Order_RelayIP"].ToString();
            var serverPort = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["WX_Order_RelayPort"].ToString());

            return RelayAccessFactory.GetDSFromRelayRowNumStartWithZero(reqString, "100878", serverIp, serverPort);
        }
    }
}
;