using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.DAL.Infrastructure;
using System.Configuration;

namespace CFT.CSOMS.DAL.CheckModoule
{
    public class CheckData
    {
        /// <summary>
        /// 获取审批数据 c2c_fmdb.t_check_param
        /// </summary>
        /// <param name="objid">审批ID</param>
        /// <param name="checkType">审批类型</param>
        /// <returns></returns>
        public DataTable GetCheckInfo(string objid, string checkType)
        {
            try
            {
                if (objid == null || objid == "")
                {
                    throw new Exception("审批单ID不能为空！");
                }
                using (var daht = MySQLAccessFactory.GetMySQLAccess("ht_DB"))
                {
                    daht.OpenConn();
                    string strSql = "select Fid from c2c_fmdb.t_check_main where fobjid='" + objid + "' and fcheckType='" + checkType + "' ";
                    DataTable dt_main = daht.GetTable(strSql);
                    if (dt_main == null || dt_main.Rows.Count != 1)
                    {
                        throw new Exception("审批单任务单ID" + objid + "对应的记录数不唯一");
                    }

                    strSql = "select * from c2c_fmdb.t_check_param where fcheckid=" + dt_main.Rows[0]["Fid"].ToString() + "";
                    DataTable dt_param = daht.GetTable(strSql);
                    if (dt_param == null || dt_param.Rows.Count == 0)
                    {
                        throw new Exception("审批单ID" + dt_main.Rows[0]["Fid"].ToString() + "对应的参数为空");
                    }

                    return dt_param;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("获取审批数据异常："+ex.Message);
            }
        }

    }
}
