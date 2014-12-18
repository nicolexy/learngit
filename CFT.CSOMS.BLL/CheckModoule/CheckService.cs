

using System;
using System.Collections.Generic;
using System.Data;
using CFT.CSOMS.COMMLIB;
using CFT.CSOMS.DAL.Infrastructure;
namespace CFT.CSOMS.DAL.CheckModoule
{
    public class CheckService
    {
        public DataTable GetCheckInfo(string objid, string checkType)
        {
            try
            {
                DataTable dt = new CheckData().GetCheckInfo(objid, checkType);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataTable dtR = new DataTable();
                    string col_name, col_value;
                    List<String> row = new List<String>();
                    string sql = "select ";
                    foreach (DataRow dr in dt.Rows)
                    {
                        col_name = dr["FKey"].ToString().Trim();
                        dtR.Columns.Add(col_name.ToLower());//列名
                        col_value = dr["FValue"].ToString().Trim();

                        sql += "'"+col_value + "' as " + col_name + " ,";
                    }
                    sql = sql.Substring(0, sql.Length - 1);
                    dtR = PublicRes.returnDSAll(sql, "ht_DB").Tables[0];
                    return dtR;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception("service获取审批数据异常：" + ex.Message);
            }
        }


    }
}
