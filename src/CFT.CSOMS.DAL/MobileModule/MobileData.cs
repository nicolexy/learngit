using CFT.CSOMS.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.DataAccess;
using SunLibraryEX;

namespace CFT.CSOMS.DAL.MobileModule
{
    public class MobileData
    {
        #region 手机绑定

        /// <summary>
        /// 查询手机绑定信息
        /// </summary>
        /// <param name="QQ"></param>
        /// <returns></returns>
        public DataSet GetMsgNotify(string QQ)
        {
            MySqlAccess da = null;
            try
            {
                string Fuid = PublicRes.ConvertToFuid(QQ);
                if (Fuid == null)
                {
                    return null;
                }
                else
                {
                    da = new MySqlAccess(PublicRes.GetConnString("MN"));
                    da.OpenConn();
                    string strTable = "msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_"
                        + Fuid.Substring(Fuid.Length - 1, 1);
                    string sql = " select * from " + strTable + " where Fuid = '" + Fuid + "'";
                    return da.dsGetTotalData(sql);
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
            }
        }

        /// <summary>
        /// 根据手机号查询手机绑定信息
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public string GetMsgNotifyByPhoneNumber(string phoneNumber)
        {
            MySqlAccess da = null;
            try
            {
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    return null;
                }
                da = new MySqlAccess(PublicRes.GetConnString("MobileBind"));
                da.OpenConn();
                string sql = " select fuid from msgnotify.t_msgnotify_user where Fmobile = '" + phoneNumber + "'";
                var ds = da.dsGetTotalData(sql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0]["Fqqid"].ToString();
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;

            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
            }
        }

        /// <summary>
        /// 解除手机绑定信息
        /// </summary>
        /// <param name="Fuid"></param>
        public bool UnbindMsgNotify(string Fuid, out string Msg)
        {
            Msg = "";
            MySqlAccess da = null;
            try
            {
                da = new MySqlAccess(PublicRes.GetConnString("MN"));

                da.OpenConn();

                string sql = " select Fstatus from msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_" + Fuid.Substring(Fuid.Length - 1, 1) + " where Fuid = '" + Fuid + "'";

                string Fstatus = da.GetOneResult(sql);

                if (Fstatus == null || Fstatus == "")
                {
                    Msg = "不存在该记录";
                    return false;
                }
                else
                {
                    Fstatus = Convert.ToString(Convert.ToInt32(Fstatus), 2);

                    if (Fstatus.Length < 31)
                    {
                        Fstatus = Fstatus.PadLeft(31, '0');
                    }
                    if (Fstatus.Length != 31)
                    {
                        Msg = "记录状态数据异常";
                        return false;
                    }

                    try
                    {
                        if (Fstatus.Substring(30, 1).ToString() == "1")
                        {
                            Fstatus = Fstatus.Substring(0, 30) + "0";
                        }
                    }
                    catch
                    {

                    }
                    try
                    {
                        if (Fstatus.Substring(25, 1).ToString() == "1")
                        {
                            Fstatus = Fstatus.Substring(0, 25) + "0" + Fstatus.Substring(26, 5);
                        }
                    }
                    catch
                    {

                    }
                    try
                    {
                        if (Fstatus.Substring(24, 1).ToString() == "1")
                        {
                            Fstatus = Fstatus.Substring(0, 24) + "0" + Fstatus.Substring(25, 6);
                        }
                    }
                    catch
                    {

                    }

                    int NewFstatus = Convert.ToInt32(Fstatus, 2);

                    long timestamp = long.Parse(TimeTransfer.GetTickFromTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                    sql = "update msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_" + Fuid.Substring(Fuid.Length - 1, 1) + " set Fstatus = " + NewFstatus +
                        ",Fupdatetime = " + timestamp + ",Fmobile=''" + " where Fuid = '" + Fuid + "'";

                    da.ExecSqlNum(sql);
                }
                return true;
            }
            catch
            {
                Msg = "解绑失败!";
                return false;
            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
            }
        }

        /// <summary>
        /// 修改绑定信息
        /// </summary>
        /// <param name="QQ"></param>
        /// <returns></returns>
        public bool UpDateBindInfo(string QQ)
        {
            MySqlAccess da = null;
            try
            {
                string fuid = "";
                fuid = PublicRes.ConvertToFuid(QQ);

                da = new MySqlAccess(PublicRes.GetConnString("MN"));

                da.OpenConn();

                string strTable = "msgnotify_" + fuid.Substring(fuid.Length - 3, 2) + ".t_msgnotify_user_"
                    + fuid.Substring(fuid.Length - 1, 1);

                string sql = " select * from " + strTable + " where Fuid = '" + fuid + "'";

                // 2012/5/2 添加用户绑定手机但未绑定到财付通帐号的情况下，需要更新到绑定到财付通帐号
                DataSet ds = da.dsGetTotalData(sql);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return false;

                int fStatue = Convert.ToInt32(ds.Tables[0].Rows[0]["Fstatus"].ToString());

                // 0x00000004 0x00000008 0x00000010 0x00000100 都需置为为1
                fStatue = fStatue | 0x00000004;
                fStatue = fStatue | 0x00000008;
                fStatue = fStatue | 0x00000010;
                fStatue = fStatue | 0x00000100;

                string updateSql = "update " + strTable + " set Fqqid='" + QQ + "',Fstatus=" + fStatue
                    + " where Fuid='" + fuid + "' ";

                return da.ExecSql(updateSql);
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
            }
        }

        #endregion

        #region 手机号码清理

        /// <summary>
        /// 查询手机绑定次数
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> QueryMobileBoundNumber(string mobile)
        {
            var url = System.Configuration.ConfigurationManager.AppSettings["ClearMobileNumber"] ?? "http://op.cf.com/perltools/cgi-bin/msgnotify_ckv_tool";
            var req = url +
                "?jsonstr={" +
                "\"operation\":\"2\"," +
                "\"telphone\":\"" + mobile + "\"" +
                "}";
            string msg = "";
            var answer = commRes.GetFromCGI(req, null, out msg);
            if (msg != "")
            {
                throw new Exception("调用CGI出错:" + msg);
            }
            if (string.IsNullOrEmpty(answer))
            {
                throw new Exception("CGI返回值为空");
            }
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            var model = jss.Deserialize<QueryMobileBoundNumberJsonModel>(answer);
            if (model.result != "0" || model.ret_data == "")
            {
                throw new Exception("查询出错:[" + model.process_msg + model.result_msg + "]");
            }
            if (model.ret_data.IndexOf(mobile) == -1)
            {
                if (model.ret_data.IndexOf("nofound") != -1)
                {
                    throw new Exception("没有找到记录");
                }
                throw new Exception("查询失败:[" + model.ret_data + "]");
            }
            var dic = model.ret_data.Replace(" ", "").ToDictionary(',', ':');
            if (dic["mobile"] != mobile)
            {
                throw new ArgumentException("CGI返回值错误Model值和预期的不一致");
            }
            return dic;
        }

        /// <summary>
        /// 手机绑定清零
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns></returns>
        public bool ClearMobileBoundNumber(string mobile)
        {
            var url = System.Configuration.ConfigurationManager.AppSettings["ClearMobileNumber"] ?? "http://op.cf.com/perltools/cgi-bin/msgnotify_ckv_tool";
            var req = url +
                "?jsonstr={" +
                "\"operation\":\"0\"," +
                "\"telphone\":\"" + mobile + "\"" +
                "}";
            string msg = "";
            var answer = commRes.GetFromCGI(req, null, out msg);
            if (msg != "")
            {
                throw new Exception("调用CGI出错:" + msg);
            }
            if (string.IsNullOrEmpty(answer))
            {
                throw new Exception("CGI返回值为空");
            }
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            var model = jss.Deserialize<QueryMobileBoundNumberJsonModel>(answer);
            if (model.result != "0" || model.ret_data == "")
            {
                throw new Exception("清零出错:[" + model.process_msg + model.result_msg + "]");
            }
            if (model.ret_data.IndexOf(mobile) == -1)
            {
                throw new Exception("清零失败:[" + model.ret_data + "]");
            }
            return true;
        }

        #endregion
    }

    #region 查询手机绑定次数 实体类
    class QueryMobileBoundNumberJsonModel
    {
        public string ret_data;
        public string process_msg;
        public string result;
        public string result_msg;
    }
    #endregion

}
