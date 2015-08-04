using CFT.CSOMS.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TENCENT.OSS.CFT.KF.DataAccess;

namespace CFT.CSOMS.DAL.CRTModule
{
    public class CRTData
    {
        /// <summary>
        /// 查询个人证书信息列表
        /// </summary>
        /// <param name="qqid"></param>
        /// <returns></returns>
        public DataSet GetUserCrtList(string qqid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CRT"));
            try
            {
                string uid = PublicRes.ConvertToFuid(qqid);

                if (uid == null || uid.Length < 3)
                {
                    return null;
                }

                string strSql = "select * from " + PublicRes.GetTName("cft_digit_crt", "t_user_crt", uid) + " where Fuid='"
                    + uid + "' and Ftype=1";

                da.OpenConn();
                DataSet ds = da.dsGetTotalData(strSql);
                return ds;
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        /// <summary>
        /// 查询关闭证书服务信息
        /// </summary>
        /// <param name="qqid"></param>
        /// <returns></returns>
        public DataSet GetDeleteQueryInfo(string qqid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CRT"));
            try
            {
                string uid = PublicRes.ConvertToFuid(qqid);

                if (uid == null || uid.Length < 3)
                {
                    return null;
                }

                string strSql = "select Fmodify_time ,Fstandby3 as DeleteIP from " + PublicRes.GetTName("cft_digit_crt", "t_user_attr", uid) + " where Fuid='"
                    + uid + "' and Fattr=1 and Fstate='2' and Fvalue='2'";

                da.OpenConn();

                DataSet ds = da.dsGetTotalData(strSql);

                if (ds == null || ds.Tables.Count == 0)
                {
                    return null;
                }
                return ds;
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }


        /// <summary>
        /// 删除个人证书
        /// </summary>
        /// <param name="qqid"></param>
        public void DeleteUserCrt(string qqid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CRT"));
            try
            {
                string uid = PublicRes.ConvertToFuid(qqid);

                if (uid == null || uid.Length < 3)
                {
                    throw new Exception("该用户不存在!");
                }

                da.OpenConn();

                string Sql = "delete from " + PublicRes.GetTName("cft_digit_crt", "t_user_crt", uid) + " where Fuid='"
                             + uid + "' and Ftype=1";

                da.ExecSqlNum(Sql);

                Sql = "delete from " + PublicRes.GetTName("cft_digit_crt", "t_user_attr", uid) + " where Fuid='"
                      + uid + "' and Fattr=1";

                da.ExecSqlNum(Sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        /// <summary>
        /// 关闭证书服务
        /// </summary>
        /// <param name="qqid"></param>
        public void DeleteCrtService(string qqid, string ip)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CRT"));
            try
            {
                string uid = PublicRes.ConvertToFuid(qqid);

                if (uid == null || uid.Length < 3)
                {
                    throw new Exception("该用户不存在!");
                }

                da.OpenConn();

                string Sql = "update " + PublicRes.GetTName("cft_digit_crt", "t_user_crt", uid) + " set Fstate = 4 , Flstate = 2 , Fmodify_time = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where Fuid = '"
                              + uid + "' and Ftype = 1 and Flstate != 2";

                da.ExecSqlNum(Sql);
                string IP = ip;
                Sql = "update " + PublicRes.GetTName("cft_digit_crt", "t_user_attr", uid) + " set Fstate = 2 , Fvalue = '2' , Fmodify_time = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' , Fstandby3 = '" + IP + "' where Fuid = '"
                             + uid + "' and  Fattr = 1";

                da.ExecSqlNum(Sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                da.Dispose();
            }
        }
    }
}
