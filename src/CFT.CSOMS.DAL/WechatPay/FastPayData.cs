using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using CFT.CSOMS.DAL.Infrastructure;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using System.Collections;

namespace CFT.CSOMS.DAL.WechatPay
{
    public class FastPayData
    {
       /// <summary>
        /// 通过接口查询bank pos日表和月表的数据
       /// </summary>
       /// <param name="bankCard">卡号</param>
       /// <param name="bankDate">日期</param>
       /// <param name="biz_type">业务类型</param>
       /// <param name="offset"></param>
       /// <param name="limit"></param>
       /// <returns></returns>
        public DataSet QueryBankPosDataList(string bankCard, string bankDate, int biz_type, int offset, int limit,out int totalNum)
        {
            DateTime d1 = DateTime.ParseExact(bankDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

            string begintime = d1.ToString("yyyy-MM-dd") + " 00:00:00";
            string endtime = d1.ToString("yyyy-MM-dd") + " 23:59:59";
            string serBankaccno = BankLib.BankIOX.Encrypt(bankCard);//银行卡加密

            //测试
         //   serBankaccno = "WeWfyNvtk7-M7WwTIPZvesYv2WHITL3E";

            //string inmsg = "request_type=2617&ver=1&head_u=&sp_id=2000000501&req_id=1006&card_no=" + serBankaccno;

            string inmsg = "req_id=1006&card_no=" + serBankaccno;
            inmsg += "&biz_type=" + biz_type;
            inmsg += "&start_time="+begintime;
            inmsg +="&end_time="+endtime;
            inmsg += "&limit=" + limit;
            inmsg += "&offset=" + offset;
            inmsg += "&table_suffix=" + bankDate;
            inmsg += "&MSG_NO=26171006" + DateTime.Now.Ticks.ToString();

            string ip = System.Configuration.ConfigurationManager.AppSettings["QueryBankPosIP"].ToString();
            int port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["QueryBankPosPORT"].ToString());

            //request_type=2617&ver=1&head_u=&sp_id=2000000501&req_id=1006&card_no=8Qz189FjLX5CU24z_L2yWw==&biz_type=10100&start_time=2014-09-29 01:35:33&end_time=2014-09-29 20:35:33&limit=10&offset=0&table_suffix=20140929
            totalNum = 0;
            return RelayAccessFactory.GetDSFromRelayRowNum(out totalNum, inmsg, "2617", ip, port);
        }


        /// <summary>
        /// 通过接口查询bank pos日表和月表的数据
        /// </summary>
        /// <param name="bankCard">卡号</param>
        /// <param name="bankDate">日期</param>
        /// <param name="biz_type">业务类型</param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public DataSet GetBankPosDataList(string bankCard, string bankDate,string bankType ,int biz_type, int offset, int limit, out int totalNum)
        {
            DateTime d1 = DateTime.ParseExact(bankDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

            string begintime = d1.ToString("yyyy-MM-dd") + " 00:00:00";
            string endtime = d1.ToString("yyyy-MM-dd") + " 23:59:59";
            string serBankaccno = BankLib.BankIOX.Encrypt(bankCard);//银行卡加密
      
            //测试
            //   serBankaccno = "WeWfyNvtk7-M7WwTIPZvesYv2WHITL3E";

            string relayDefaultSPId = "20000000";
            string inmsg = "req_id=1006&card_no=" + serBankaccno;
            inmsg += "&biz_type=" + biz_type;
            inmsg += "&bank_type=" + bankType;
            inmsg += "&start_time=" + begintime;
            inmsg += "&end_time=" + endtime;
            inmsg += "&limit=" + limit;
            inmsg += "&offset=" + offset;
            inmsg += "&table_suffix=" + bankDate;
            inmsg += "&MSG_NO=26171006" + DateTime.Now.Ticks.ToString();

            string ip = System.Configuration.ConfigurationManager.AppSettings["QueryBankPosIP"].ToString();
            int port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["QueryBankPosPORT"].ToString());


            log4net.ILog log = log4net.LogManager.GetLogger("GetBankPosDataList");
            if (log.IsInfoEnabled)
            {
                log.InfoFormat("卡号={0} 卡号加密后={1}, IP={2} ,port={3},request_type={4}", bankCard, serBankaccno, ip, port, inmsg);
            }
            //request_type=2617&ver=1&head_u=&sp_id=2000000501&req_id=1006&card_no=F9L4wI4C0ON0_bGFziy1_g%3D%3D&biz_type=10100&start_time=2015-03-11 01:35:33&end_time=2015-03-11 23:35:33&limit=10&offset=0&table_suffix=20150311&bank_type=3115
            totalNum = 0;
            return RelayAccessFactory.GetDSFromRelayRowNumNew(out totalNum, inmsg, "2617", ip, port, false, false, relayDefaultSPId);
        }

        /// <summary>
        /// 通过接口查询bank pos中的fbill_no
        /// </summary>
        /// <param name="real_bill_no">给银行实际订单号</param>
        /// <param name="bankDate">日期</param>
        /// <param name="bank_type">银行类型</param>
        /// <param name="biz_type">业务类型</param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public DataSet QueryBankBillNoList(string real_bill_no, string bank_type, string bankDate, int biz_type, int offset, int limit, out int totalNum)
        {
            DateTime d1 = DateTime.ParseExact(bankDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

           // string inmsg = "request_type=2617&ver=1&head_u=&sp_id=&req_id=1004&bank_type=" + bank_type;
            string inmsg = "req_id=1004&bank_type=" + bank_type;
            inmsg += "&real_bill_no=" + real_bill_no;
            inmsg += "&biz_type=" + biz_type;
            inmsg += "&limit=" + limit;
            inmsg += "&offset=" + offset;
            inmsg += "&table_suffix=" + bankDate;
            inmsg += "&MSG_NO=26171004" + DateTime.Now.Ticks.ToString();

            string ip = System.Configuration.ConfigurationManager.AppSettings["QueryBankPosIP"].ToString();
            int port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["QueryBankPosPORT"].ToString());

            //request_type=2617&ver=1&head_u=&sp_id=2000000501&req_id=1006&card_no=8Qz189FjLX5CU24z_L2yWw==&biz_type=10100&start_time=2014-09-29 01:35:33&end_time=2014-09-29 20:35:33&limit=10&offset=0&table_suffix=20140929
            totalNum = 0;
            return RelayAccessFactory.GetDSFromRelayRowNum(out totalNum,"2617",inmsg, ip, port);
        }
      
        /// <summary>
        /// 
        /// 对账单的库表
        /// 
        /// 通过银行卡号，日期查询银行卡订单信息
        /// </summary>
        /// <param name="bankCard">银行卡号</param>
        /// <param name="bankDate">日期</param>
        /// <returns></returns>
        public DataSet QueryBankDataList(string bankCard, string bankDate,int offset,int limit) 
        {
            string zwskDate = "20130331";
            try
            {
                zwskDate = ConfigurationManager.AppSettings["ZWSKDate"];
            }
            catch
            {
                zwskDate = "20130331";
            }

            if (string.IsNullOrEmpty(zwskDate))
            {
                zwskDate = "20130331";
            }

            string serBankaccno = BankLib.BankIOX.Encrypt(bankCard);//银行卡加密

            string Sql = "select fbank_order,fpay_acc,0 as Fbiz_type,Famt from c2c_zwdb_" + bankDate + ".t_bankdata_list where fpay_acc='" + bankCard + "' or fpay_acc='" + serBankaccno + "' order by date_format(Fbank_date,'%Y%m%d') ";
          
            if (limit != -1 && offset != -1)
                Sql += " limit " + offset + "," + limit;
            DataSet ds = null;

         

            DateTime d1 = DateTime.ParseExact(bankDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            DateTime d2 = DateTime.ParseExact(zwskDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            if (d1.CompareTo(d2) >= 0)
            {
                using (var da = MySQLAccessFactory.GetMySQLAccess("ZWSK"))
                {
                    da.OpenConn();

                    ds = da.dsGetTotalData(Sql.ToString());

                    return ds;
                }
            }
            else
            {
                using (var da = MySQLAccessFactory.GetMySQLAccess("ZWBasic"))
                {
                    da.OpenConn();

                    ds = da.dsGetTotalData(Sql.ToString());

                    return ds;
                }
            }
        }

        /// <summary>
        /// 
        /// c2c_db_pos.t_bank_pos_月表
        /// 
        /// 通过银行卡号，日期查询订单信息
        /// </summary>
        /// <param name="bankCard">银行卡号</param>
        /// <param name="bankDate">日期</param>
        /// <returns></returns>
        public DataSet QueryPosByMonthList(string bankCard, string bankDate, int offset, int limit) 
        {
            DateTime d1 = DateTime.ParseExact(bankDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

            string date = d1.ToString("yyyyMM");
            string begintime = d1.ToString("yyyy-MM-dd")+" 00:00:00";
            string endtime = d1.AddDays(1).ToString("yyyy-MM-dd")+" 23:59:59";
            string serBankaccno = BankLib.BankIOX.Encrypt(bankCard);//银行卡加密

            string strSql = "select  Fcard_no as fpay_acc ,Fbill_no as fbank_order,Famount as Famt,Fbiz_type from c2c_db_pos.t_bank_pos_" + date.Substring(0, 6) + " where Fcard_no='" + serBankaccno + "' and Fcreate_time between '" + begintime + "' and '" + endtime + "'";
            if (limit != -1 && offset != -1)
                strSql += " limit " + offset + "," + limit;
            DataSet ds = null;
            using (var da = MySQLAccessFactory.GetMySQLAccess("ZWNEWTABLE"))
            {
                da.OpenConn();

                ds = da.dsGetTotalData(strSql);
                
            }
            return ds;
        }

        /// <summary>
        /// 
        /// c2c_db_pos.t_bank_pos_日表
        /// 
        /// 通过银行卡号，日期查询订单信息
        /// </summary>
        /// <param name="bankCard">银行卡号</param>
        /// <param name="bankDate">日期</param>
        /// <returns></returns>
        public DataSet QueryPosByDayList(string bankCard, string bankDate, int offset, int limit) 
        {
            DateTime d1 = DateTime.ParseExact(bankDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

            string date = d1.ToString("yyyyMMdd");
            string begintime = d1.ToString("yyyy-MM-dd") + " 00:00:00";
            string endtime = d1.AddDays(1).ToString("yyyy-MM-dd") + " 23:59:59";
            string serBankaccno = BankLib.BankIOX.Encrypt(bankCard);//银行卡加密

            string strSql = "select  Fcard_no as fpay_acc ,Fbill_no as fbank_order,Famount as Famt,Fbiz_type from c2c_db_pos.t_bank_pos_" + date.Substring(0, 8) + " where Fcard_no='" + serBankaccno + "' and Fcreate_time between '" + begintime + "' and '" + endtime + "' order by Fcreate_time desc";
            if (limit != -1 && offset != -1)
                strSql += " limit " + offset + "," + limit;
            DataSet ds = null;
            using (var da = MySQLAccessFactory.GetMySQLAccess("ZWNEWTABLE_day"))
            {
                da.OpenConn();

                ds = da.dsGetTotalData(strSql);

            }
            return ds;
        }

        /// <summary>
        /// 
        /// 查单表c2c_db_pos.t_pos_water流水当前表、历史表
        /// 
        /// 通过银行卡号，日期查询订单信息
        /// </summary>
        /// <param name="bankCard">银行卡号</param>
        /// <param name="bankDate">日期</param>
        /// <returns></returns>
        public DataSet QueryPosWaterList(string bankCard, string bankDate, int offset, int limit)
        {
            DateTime d1 = DateTime.ParseExact(bankDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

            string begintime = d1.ToString("yyyy-MM-dd") + " 00:00:00";
            string endtime = d1.AddDays(1).ToString("yyyy-MM-dd") + " 23:59:59";
            string serBankaccno = BankLib.BankIOX.Encrypt(bankCard);//银行卡加密

            DataSet ds = null;
            string tableName = "c2c_db_pos.t_pos_water";//Fbiz_type字段无
            string strSql = "select  Fbankid as fpay_acc ,FBillNO as fbank_order,FAmount as Famt,0 as Fbiz_type from " + tableName + " where Fbankid='" + serBankaccno + "' and FModifyTime between '" + begintime + "' and '" + endtime + "' ";
            
            if (limit != -1 && offset != -1)
                strSql += " limit " + offset + "," + limit;
            using (var da = MySQLAccessFactory.GetMySQLAccess("ZWOLDTABLE"))
            {
                da.OpenConn();

                ds = da.dsGetTotalData(strSql);
            }
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                tableName = "c2c_db_pos.t_pos_water_2013";
                strSql = "select  Fbankid as fpay_acc ,FBillNO as fbank_order,FAmount as Famt,0 as Fbiz_type from " + tableName + " where Fbankid='" + serBankaccno + "' and FModifyTime between '" + begintime + "' and '" + endtime + "' ";
                if (limit != -1 && offset != -1)
                    strSql += " limit " + offset + "," + limit;
                using (var da = MySQLAccessFactory.GetMySQLAccess("ZWOLDTABLE_history"))//本来配置的是ZWOLDTABLE_history，后来因为在一个库里，刷包修改为ZWNEWTABLE_day
                {
                    da.OpenConn();

                    ds = da.dsGetTotalData(strSql);
                }
            }
            return ds;
        }
        /// <summary>
        /// 零钱包收付款记录查询
        /// </summary>
        /// <param name="prime_trans_id">商户订单号</param>
        /// <returns></returns>
        public DataSet CoinWalletsPaymentQuery(string prime_trans_id)
        {
            string msg = "";
            string service_name = "wxt_pay_qrylist_service";
            string req = "";

            if (string.IsNullOrEmpty(prime_trans_id))
            {
                throw new Exception("prime_trans_id不能为空！");
            }
            try
            {
                DataSet ds = null;
               // prime_trans_id="2000000501201405130301345082";
                if (!string.IsNullOrEmpty(prime_trans_id))
                    req += "prime_trans_id=" + prime_trans_id;
                ds = CommQuery.GetOneTableFromICE(req, "", service_name, true, out msg);
                if (msg != "")
                    throw new Exception(msg);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("零钱包收付款记录查询处理失败！" + msg);
            }
        }

        public DataSet FastPayLimitQuery(string ip,int port,string cardNo, string bankType, int card_type, int pay_type, int req, int category)
        {
            //string inmsg = "request_type=6952&ver=1&head_u=&sp_id=1000000000&channel_id=1&direct=1&query_dim=&req_type=";

            string inmsg = "channel_id=1&direct=1&query_dim=&req_type=";

            if (req == 25)
            {
                inmsg += "25&card_id=";
                inmsg += cardNo;
                inmsg += "&bank_type=" + bankType;
            }
            else if (req == 22)
            {
                inmsg += "22&card_id=";
                inmsg += cardNo;
                inmsg += "&bank_type=" + bankType;
                inmsg += "&pay_type=" + pay_type;
                inmsg += "&card_type=" + card_type;
                inmsg += "&category=" + category;
            }

          //  return RelayAccessFactory.GetDSFromRelay(inmsg, ip, port);
            return RelayAccessFactory.GetDSFromRelay(inmsg,"6952", ip, port);
        }

     
    }
}
