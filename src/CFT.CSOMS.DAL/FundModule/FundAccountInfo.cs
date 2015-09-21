using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CFT.CSOMS.DAL.FundModule
{
    using CFT.CSOMS.DAL.Infrastructure;

    public class FundAccountInfo
    {
        public DataTable QueryFundAccountRelationInfo(string uin)
        {
            //if (string.IsNullOrEmpty(uin))
            //    throw new ArgumentNullException("uin");

            //using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            //{
            //    da.OpenConn();
            //    string Sql = string.Format("select * from fund_db.t_fund_bind where Fqqid='{0}'", uin);
            //    DataSet ds = da.dsGetTotalData(Sql);

            //    return ds.Tables[0];
            //}

            if (string.IsNullOrEmpty(uin))
                throw new ArgumentNullException("uin");

            DataTable dt = null;
            var serverIp = System.Configuration.ConfigurationManager.AppSettings["FundRateIP"].ToString();
            var serverPort = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["FundRatePort"].ToString());
            string requestText = "reqid=668&flag=1&fields=uin:" + uin;
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }
    }
}
