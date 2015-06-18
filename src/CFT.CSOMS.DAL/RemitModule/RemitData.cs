using CFT.CSOMS.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.DAL.RemitModule
{
    public class RemitData
    {
        public DataSet GetRemitDataList(string batchid, string tranType, string dataType, string remitType, string tranState, string spid, string remitRec, string listID, int start, int max)
        {
            try
            {
                RemitQueryClass cuser = new RemitQueryClass(batchid, tranType, dataType, remitType, tranState, spid, remitRec, listID);
                return cuser.GetResultX(start, max, "ZW");
            }
            catch(Exception ex)
            {
                log4net.LogManager.GetLogger("Service处理失败！" + ex.Message);
                throw new Exception("Service处理失败！");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Ford_date">汇款平台日期</param>
        /// <param name="Ford_ssn">汇款平台流水</param>
        /// <returns></returns>
        public DataSet GetRemitStateInfo(string Ford_date, string Ford_ssn)
        {
            try
            {
                QueryRemitStateInfo query = new QueryRemitStateInfo(Ford_date, Ford_ssn);
                return query.GetResultX(0, 1, "REMIT");
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("Service处理失败！" + ex.Message);
                throw new Exception("Service处理失败！");
            }
        }

        public int GetRemitListCount(string batchid, string tranType, string dataType, string remitType, string tranState, string spid, string remitRec, string listID)
        {
            try
            {
                RemitQueryClass cuser = new RemitQueryClass(batchid, tranType, dataType, remitType, tranState, spid, remitRec, listID);
                return cuser.GetCount("ZW");
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("Service处理失败！" + ex.Message);
                throw new Exception("Service处理失败！");
            }
        }

    }
}
