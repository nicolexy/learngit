using CFT.CSOMS.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TENCENT.OSS.CFT.KF.DataAccess;

namespace CFT.CSOMS.DAL.CreditModule
{
    public class CreditData
    {
        /// <summary>
        /// 查询信用卡还款记录数
        /// </summary>
        /// <param name="Flistid"></param>
        /// <returns></returns>
        public DataSet GetCreditQueryListCount(string Flistid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YWB"));
            try
            {
                da.OpenConn();

                string sql = "SELECT Count(1) FROM c2c_db.t_tcpay_list WHERE Flistid='" + Flistid + "'";

                DataSet ds = da.dsGetTotalData(sql);

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
            }
            finally
            {
                da.Dispose();
            }
        }

        /// <summary>
        /// 查询信用卡还款记录(按财付通号查询)
        /// </summary>
        /// <param name="QQOrEmail"></param>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <param name="istart"></param>
        /// <param name="imax"></param>
        /// <returns></returns>
        public DataSet GetCreditQueryListForFaid(string QQOrEmail, DateTime begindate, DateTime enddate, int istart, int imax)
        {
            string Fuid = CFTAccount.AccountData.ConvertToFuidX(QQOrEmail);

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YWB"));
            try
            {
                da.OpenConn();

                string strwhere = " WHERE Fuid='" + Fuid +
                    "' AND Fpay_front_time_acc >='" + begindate.ToString("yyyy-MM-dd 00:00:00") +
                    "' AND Fpay_front_time_acc <='" + enddate.ToString("yyyy-MM-dd 23:59:59") + "' AND Fbankid IN (5,8,36,37)" +
                    " AND Fmodify_time >= '" + begindate.ToString("yyyy-MM-dd 00:00:00") +
                    "' AND Fmodify_time <= '" + enddate.AddDays(7).ToString("yyyy-MM-dd 23:59:59") + "' AND Fbank_type <> 2033";

                string currtable = "";
                string othertable = "";
                PickQueryClass.GetPayListTableFromTime(begindate, out currtable, out othertable);

                //**furion提现单改造20120216 不再使用 时间用Fpay_front_time_acc
                string sql = "SELECT Fbank_type,Flistid,Fsign,Fbank_name,RIGHT(Fabankid,4) AS creditcard_id,Fnum,Fpay_front_time " +
                    "FROM c2c_db.t_tcpay_list " + strwhere;

                sql += " union all select Fbank_type,Flistid,Fsign,Fbank_name,RIGHT(Fabankid,4) AS creditcard_id,Fnum,Fpay_front_time from " + currtable + strwhere;
                sql += " union all select Fbank_type,Flistid,Fsign,Fbank_name,RIGHT(Fabankid,4) AS creditcard_id,Fnum,Fpay_front_time from " + othertable + strwhere;
                sql += " ORDER BY Fpay_front_time DESC " + " Limit " + (istart - 1) + "," + imax;

                DataSet ds = da.dsGetTotalData(sql);
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
            }
            finally
            {
                da.Dispose();
            }
        }

        /// <summary>
        /// 查询信用卡还款记录
        /// </summary>
        /// <param name="Flistid"></param>
        /// <param name="istart"></param>
        /// <param name="imax"></param>
        /// <returns></returns>
        public DataSet GetCreditQueryList(string Flistid, int istart, int imax)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YWB"));
            try
            {
                da.OpenConn();

                //**furion提现单改造20120216
                string sql = "SELECT Fbank_type,Flistid,Fsign,Fbank_name,RIGHT(Fabankid,4) AS creditcard_id,Fnum,Fpay_front_time FROM c2c_db.t_tcpay_list WHERE Flistid='" + Flistid + "'";

                string currtable = "";
                string othertable = "";
                PickQueryClass.GetPayListTableFromID(Flistid, out currtable, out othertable);

                sql += " union all select Fbank_type,Flistid,Fsign,Fbank_name,RIGHT(Fabankid,4) AS creditcard_id,Fnum,Fpay_front_time from " + currtable + " where  Flistid='" + Flistid + "'";
                sql += " union all select Fbank_type,Flistid,Fsign,Fbank_name,RIGHT(Fabankid,4) AS creditcard_id,Fnum,Fpay_front_time from " + othertable + " where  Flistid='" + Flistid + "'";
                sql += " Limit " + (istart - 1) + "," + imax;

                DataSet ds = da.dsGetTotalData(sql);
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
            }
            finally
            {
                da.Dispose();
            }
        }

    }
}
