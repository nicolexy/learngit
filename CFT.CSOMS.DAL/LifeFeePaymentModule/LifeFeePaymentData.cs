using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.CSOMS.DAL.CFTAccount;
using System.Text.RegularExpressions;

namespace CFT.CSOMS.DAL.LifeFeePaymentModule
{
    public class LifeFeePaymentData
    {
        //缴费账单核心信息记录表
        public DataSet QueryChargeBill(string acc, string transId)
        {
            try
            {
                using (var da = MySQLAccessFactory.GetMySQLAccess("UC_DB"))
                {
                    da.OpenConn();
                    string index=acc;
                    if (!IsNum(acc))
                       index= GetUinKey(acc).ToString();
                    var table_name = string.Format("public_utility_charge_platform_{0}.t_charge_bill_{1}", acc.Substring(acc.Length - 2), acc.Substring(acc.Length - 3, 1));
                    string Sql = string.Format(" select FTransId, FUserId,FStatus from {0} where FUin='{1}' ", table_name, acc);
                    if (!string.IsNullOrEmpty(transId.Trim()))
                        Sql += " and FTransId='"+transId+"' limit 0,1 ";
                    DataSet ds = da.dsGetTotalData(Sql);

                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("查询公告联缴费账单核心信息记录表" + ex.Message);
            }
        }

        public bool IsNum(string input)
        {
            string pattern = @"^[0-9]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        public int GetUinKey(string acc)
        {
            int nRes = 0;
            string strMd5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(acc, "md5").ToLower();
            nRes = 1000
            + strMd5[strMd5.Length - 3] % 10 * 100
            + strMd5[strMd5.Length - 2] % 10 * 10
            + strMd5[strMd5.Length - 1] % 10;
            return nRes;
        }

    }
}
