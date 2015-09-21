using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CFT.CSOMS.DAL.CFTAccount;

namespace CFT.CSOMS.DAL.FundModule
{
    using CFT.CSOMS.DAL.Infrastructure;
    using TENCENT.OSS.CFT.KF.DataAccess;

    public class SafeCard
    {
        //获取理财通安全卡更换日志
        public DataTable GetFundTradeLog(string qqid, int istr, int imax)
        {
            //using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            //{
            //    var sqlBuilder = new StringBuilder(string.Format("select * from fund_db.t_fund_pay_card_log where Fqqid='{0}'", qqid));

            //    sqlBuilder.AppendFormat(" order by Fmodify_time desc limit " + istr + "," + imax);
                
            //    da.OpenConn();
            //    DataSet ds = da.dsGetTotalData(sqlBuilder.ToString());

            //    return ds.Tables[0];
            //}

            if (string.IsNullOrEmpty(qqid))
                throw new ArgumentNullException("qqid");

            DataTable dt = null;
            var serverIp = System.Configuration.ConfigurationManager.AppSettings["FundRateIP"].ToString();
            var serverPort = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["FundRatePort"].ToString());
            string requestText = "reqid=679&flag=2&offset={0}&limit={1}&fields=qqid:{2}";
            requestText = string.Format(requestText, istr, imax, qqid);
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }
        public DataTable GetPayCardInfo(string qqid)
        {
            //if (qqid == null || qqid == "")
            //{
            //    throw new ArgumentNullException("财付通账号不能为空！");
            //}

            //string sql = "select * from fund_db.t_fund_pay_card where Fqqid='" + qqid + "'";

            //using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            //{
            //    da.OpenConn();
            //    var dt = da.GetTable(sql);
            //    dt.TableName = "PayCardInfo";
            //    return dt;
            //}

            if (string.IsNullOrEmpty(qqid))
                throw new ArgumentNullException("qqid");

            DataTable dt = null;
            var serverIp = System.Configuration.ConfigurationManager.AppSettings["FundRateIP"].ToString();
            var serverPort = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["FundRatePort"].ToString());
            string requestText = "reqid=661&flag=1&fields=uin:" + qqid;
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                dt.TableName = "PayCardInfo";
            }
            return dt;

        }
       
          /// <summary>
        ///  查询理财通支持的银行类型
          /// </summary>
          /// <param name="bank_type"></param>
          /// <returns></returns>
        public bool GetFundSupportBank(string bank_type)
        {
            //if (string.IsNullOrEmpty(bank_type))
            //    throw new ArgumentNullException("bank_type");

            //string sql = "select * from fund_db.t_fund_bank_config where Flstate=1";

            //using (MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("fund")))
            //{
            //    da.OpenConn();
            //    var dt = da.GetTable(sql);
            //    dt.TableName = "SupportBank";
            //    var dd = dt.Select(" Fbank_type=" + bank_type);
            //    return (dd.Count() > 0);
            //}



            if (string.IsNullOrEmpty(bank_type))
                throw new ArgumentNullException("bank_type");

            var serverIp = System.Configuration.ConfigurationManager.AppSettings["FundRateIP"].ToString();
            var serverPort = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["FundRatePort"].ToString());
            string requestText = "reqid=616&flag=1&fields=bank_type:{0}|support_type:{1}";

            string requestText1 = string.Format(requestText, bank_type, 1);
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText1, "100769", serverIp, serverPort);

            string requestText2 = string.Format(requestText, bank_type, 2);
            DataSet ds2 = RelayAccessFactory.GetDSFromRelayFromXML(requestText2, "100769", serverIp, serverPort);

            string requestText3 = string.Format(requestText, bank_type, 3);
            DataSet ds3 = RelayAccessFactory.GetDSFromRelayFromXML(requestText3, "100769", serverIp, serverPort);

            ds = PublicRes.ToOneDataset(ds, ds2);
            ds = PublicRes.ToOneDataset(ds, ds3);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
