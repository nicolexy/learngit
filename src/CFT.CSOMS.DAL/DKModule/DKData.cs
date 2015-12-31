using CFT.CSOMS.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.CFT.KF.DataAccess;

namespace CFT.CSOMS.DAL.DKModule
{
    public class DKData
    {
        public DataTable GetDKBankList()
        {
            var ip = Apollo.Common.Configuration.AppSettings.Get<string>("Relay_IP", "10.123.6.291");
            var port = Apollo.Common.Configuration.AppSettings.Get<int>("Relay_PORT", 356001);
            string requestString = "banktype_filter_rule=1&biz_type=1";
            string answer = RelayAccessFactory.RelayInvoke(requestString, "6891", false, false, ip, port, "");
            string msg;
            DataTable  dt = CommQuery.ParseRelayPageMethod2(answer, out msg);
           
            if (!string.IsNullOrEmpty(msg))
            {
                throw new Exception(msg);
            }
            return dt;
        }

        //银行账户限额批次查询函数
        public DataTable GetDKLimit_List(string bank_sname, string bankaccno, string servicecode, int limStart, int limMax)
        {

            DataSet ds = new DataSet();
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC_NEW"));
            try
            {
                da.OpenConn();
                bankaccno = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(bankaccno, "md5").ToLower();
                string roleid = bank_sname + bankaccno + "@cep.tenpay.com";
                roleid = PublicRes.ConvertToFuid(roleid);
                //roleid = "2000000501";
                string strSql = " select Fservice_code,'" + bank_sname + "' as Fbanktype,'" + bankaccno + "' as Fbankaccno, ";

                strSql += "sum(case Fdimension when 1 then Fonce_data else 0 end)/100 as Fonce_data,";//单笔限额

                strSql += "sum(case Fdimension when 1 then Fday_data else 0 end)/100 as Fday_sum_data,";//单日限额
                strSql += "sum(case Fdimension when 3 then Fday_data else 0 end)/100 as Fday_use_data,";//单日累计使用限额

                strSql += "sum(case Fdimension when 1 then Fweek_data else 0 end)/100 as Fweek_sum_data,";//单周限额
                strSql += "sum(case Fdimension when 3 then Fweek_data else 0 end)/100 as Fweek_use_data,";//单周累计使用限额

                strSql += "sum(case Fdimension when 1 then Fmounth_data else 0 end)/100 as Fmonth_sum_data,";//单月限额
                strSql += "sum(case Fdimension when 3 then Fmounth_data else 0 end)/100 as Fmonth_use_data,";//单月累计使用限额

                strSql += "sum(case Fdimension when 1 then Fquarter_data else 0 end)/100 as Fquarter_sum_data,";//单季限额
                strSql += "sum(case Fdimension when 3 then Fquarter_data else 0 end)/100 as Fquarter_use_data,";//单季累计使用限额

                strSql += "sum(case Fdimension when 1 then Fyear_data else 0 end)/100 as Fyear_sum_data,";//单年限额
                strSql += "sum(case Fdimension when 3 then Fyear_data else 0 end)/100 as Fyear_use_data,";//单年累计使用限额

                strSql += "sum(case Fdimension when 2 then Fday_data else 0 end) as Fday_sum_count,";//单日限次
                strSql += "sum(case Fdimension when 4 then Fday_data else 0 end) as Fday_use_count,";//单日累计使用次数

                strSql += "sum(case Fdimension when 2 then Fweek_data else 0 end) as Fweek_sum_count,";//单周限次
                strSql += "sum(case Fdimension when 4 then Fweek_data else 0 end) as Fweek_use_count,";//单周累计使用次数

                strSql += "sum(case Fdimension when 2 then Fmounth_data else 0 end) as Fmonth_sum_count,";//单月限次
                strSql += "sum(case Fdimension when 4 then Fmounth_data else 0 end) as Fmonth_use_count,";//单月累计使用次数

                strSql += "sum(case Fdimension when 2 then Fquarter_data else 0 end) as Fquarter_sum_count,";//单季限次
                strSql += "sum(case Fdimension when 4 then Fquarter_data else 0 end) as Fquarter_use_count,";//单季累计使用次数

                strSql += "sum(case Fdimension when 2 then Fyear_data else 0 end) as Fyear_sum_count,";//单年限次
                strSql += "sum(case Fdimension when 4 then Fyear_data else 0 end) as Fyear_use_count ";//单年累计使用次数

                strSql += " from " + PublicRes.GetTName("cft_cep_db", "t_service_limit", roleid) + " where Frole_id='" + roleid + "' ";

                strSql += string.IsNullOrEmpty(servicecode) ? "" : " and Fservice_code='" + servicecode + "'";

                strSql += " and Fflag=1 group by Fservice_code ";

                ds = da.dsGetTableByRange(strSql, limStart, limMax);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;

                ds.Tables[0].Columns.Add("Fspid", typeof(string));
                ds.Tables[0].Columns.Add("Fspname", typeof(string));
                ds.Tables[0].Columns.Add("Fcodeid", typeof(string));
                ds.Tables[0].Columns.Add("Fcodename", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string tmp = dr["Fservice_code"].ToString();
                    string spid = tmp.Substring(0, 10);
                    dr["Fspid"] = spid;

                    string msg = "";
                    dr["Fspname"] = CommQuery.GetOneResultFromICE("spid=" + spid, CommQuery.QUERY_MERCHANTINFO, "FName", out msg);
                    string codeid = dr["Fservice_code"].ToString().Replace(spid, "");
                    dr["Fcodeid"] = codeid;

                    if (getData.htService_code.Contains(codeid))
                    {
                        dr["Fcodename"] = getData.htService_code[codeid].ToString();
                    }
                    else
                    {
                        dr["Fcodename"] = "";
                    }
                }

                return ds.Tables[0];
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                da.Dispose();
            }
        }
    }
}
