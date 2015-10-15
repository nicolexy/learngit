using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CFT.CSOMS.DAL.Infrastructure;
using TENCENT.OSS.C2C.Finance.DataAccess;
using CFT.Apollo.Logging;
using CFT.CSOMS.DAL.CFTAccount;

namespace CFT.CSOMS.DAL.FundModule
{
    public class LCTBalance
    {

        public DataTable QueryLCTBalanceRollList(string tradeId, int start, int max)
        {

            //if (string.IsNullOrEmpty(tradeId.Trim()))
            //    throw new ArgumentNullException("tradeId");
            //using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            //{
            //    da.OpenConn();
            //    var table_name = string.Format("fund_db_{0}.t_fund_balance_order_{1}", tradeId.Substring(tradeId.Length - 2), tradeId.Substring(tradeId.Length - 3, 1));
            //    string Sql = string.Format(" select Flistid,Fcreate_time,Facc_time,Ftype,Fstandby1,Fstate,Ftotal_fee,Fmemo,Fcard_tail,Fbank_type from {0} where Ftrade_id='{1}' order by Fcreate_time desc limit {2},{3} ", table_name, tradeId, start, max);
            //    DataSet ds = da.dsGetTotalData(Sql);

            //    return ds.Tables[0];
            //}

            DataTable dt = null;
            var serverIp = System.Configuration.ConfigurationManager.AppSettings["FundRateIP"].ToString();
            var serverPort = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["FundRatePort"].ToString());
            string requestText = "reqid=682&flag=2&offset={0}&limit={1}&fields=trade_id:{2}";
            requestText = string.Format(requestText, start, max, tradeId);
            DataSet ds1 = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                dt = ds1.Tables[0];
            }
            return dt;
        }

        public DataTable QuerySubAccountInfo(string uin, int currencyType)
        {
            string fuid =AccountData.ConvertToFuid(uin);
         
            if (fuid == null)
                fuid = "0";

            //ICEAccess ice = null;
            //if (currencyType != 1)
            //{
            //    ice = ICEAccessFactory.GetICEAccess("ICEConnectionString3");
            //}
            //else
            //{
            //    ice = ICEAccessFactory.GetICEAccess("ICEConnectionString");
            //}

            try
            {
                //ice.OpenConn();
                //string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
                //strwhere += ICEAccess.URLEncode("fcurtype=" + currencyType + "&");
                //string strResp = "";
                //LogHelper.LogInfo("QuerySubAccountInfo send strwhere : " + strwhere);
                //string setID = PublicRes.GetSetIDByQQID(uin);
                //DataTable dt = ice.InvokeQuery_GetDataTable_SetID(YWSourceType.用户资源, YWCommandCode.查询用户信息, fuid, setID, strwhere, out strResp);
                string errMsg = "";
                DataTable dt = AccountData.GetAccountInfo(fuid, currencyType.ToString(), out errMsg);

                if (dt == null || dt.Rows.Count == 0)
                    return null;

                //ice.CloseConn();

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                //ice.Dispose();
            }
        }

    }
}
