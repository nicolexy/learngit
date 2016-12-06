using CFT.Apollo.Logging;
using CFT.CSOMS.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.DataAccess;

namespace CFT.CSOMS.DAL.BankCheckSystem
{
    public class BankCheckSystemData
    {
        private string connstr;
        public BankCheckSystemData()
        {
            //connstr = System.Configuration.ConfigurationManager.AppSettings["BankCheckSystem_connstr"].ToString();
            connstr = CommLib.DbConnectionString.Instance.GetConnectionString("ZW");
        }

        /// <summary>
        /// 新增用户信息
        /// </summary>
        /// <param name="userLoginAccount">登录账号</param>
        /// <param name="bankId">银行类型</param>
        /// <param name="userBindEmail">绑定邮箱</param>
        /// <param name="userName">申请人姓名</param>
        /// <param name="userIdNo">申请人证件号</param>
        /// <param name="contactAddress">申请人地址</param>
        /// <param name="contactName">联系人姓名</param>
        /// <param name="contactTel">联系人电话</param>
        /// <param name="contactMobile">联系人手机</param>
        /// <param name="userIdNoUrl">身份证图片地址</param>
        /// <param name="contactQQ">联系人qq</param>
        /// <param name="Fremark">备注</param>
        /// <param name="authLevels">权限</param>
        /// <returns></returns>
        public bool UsersAdd(out string pwd, string userLoginAccount, string bankId, string userBindEmail, string userName, string userIdNo,
            string contactAddress, string contactName, string contactTel, string contactMobile, string userIdNoUrl,
            string contactQQ, string Fremark, List<string> authLevels)
        {
            pwd = "";
            MySqlAccess da = new MySqlAccess(connstr);
            try
            {
                da.OpenConn();
                da.StartTran();

                //去重查询
                string sql = "select Fuser_bind_email from c2c_zwdb.t_bank_userinfos where Fuser_bind_email=? limit 1;";
                string oldUserBindEmail = da.GetOneResult_Parameters(sql, new List<string>() { userBindEmail });
                if (!string.IsNullOrEmpty(oldUserBindEmail))
                {
                    throw new LogicException("该邮箱已经申请！");
                }

                sql = "select Fuser_login_account from c2c_zwdb.t_bank_userinfo_login where Fuser_login_account=? limit 1;";
                string temp1 = da.GetOneResult_Parameters(sql, new List<string>() { userLoginAccount });
                if (!string.IsNullOrEmpty(temp1))
                {
                    throw new LogicException("该登录账号已存在！");
                }

                //插入用户信息           
                sql = @"INSERT INTO c2c_zwdb.t_bank_userinfos(Fbank_id,Fuser_bind_email,Fuser_name,Fuser_id_no,Fcontact_address,
                            Fcontact_name,Fcontact_tel,Fcontact_mobile,Fuser_id_no_url,Fcontact_qq,Fremark, Fcreate_time,Fmodify_time,Fuser_status) 
                            values(?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                List<string> paramUserinfo = new List<string>() { bankId, userBindEmail, userName, userIdNo, contactAddress,
                             contactName, contactTel, contactMobile, userIdNoUrl, contactQQ, Fremark,
                             DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),"1" };
                if (!da.ExecSql_Parameters(sql, paramUserinfo))
                {
                    throw new LogicException("插入用户信息失败！");
                }

                //查询刚插入的Fuser_id
                sql = "select Fuser_id from c2c_zwdb.t_bank_userinfos where Fuser_bind_email=? limit 1;";
                string Fuser_id = da.GetOneResult_Parameters(sql, new List<string>() { userBindEmail }).Trim();
                if (string.IsNullOrEmpty(Fuser_id))
                {
                    throw new LogicException("查询Fuser_id失败！");
                }

                //插入用户登录信息
                string newpassword = getRandomizer(7);
                pwd = newpassword;
                newpassword = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(newpassword, "md5").ToLower();
                sql = @"INSERT INTO c2c_zwdb.t_bank_userinfo_login(Fuser_login_account,Fuser_id,Fcreate_time,Fmodify_time,Fuser_password) 
                            values(?,?,?,?,?)";
 
                if (!da.ExecSql_Parameters(sql, new List<string>() { userLoginAccount, Fuser_id, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), newpassword }))
                {
                    throw new LogicException("插入用户登录信息失败！");
                }

                //插入权限
                if (authLevels.Count > 0)
                {
                    string sqlRelation = "INSERT INTO c2c_zwdb.t_userauth_relation set Fuser_id=?,Fauth_level=?,Fcreate_time=?;";
                    foreach (string item in authLevels)
                    {
                        List<string> paramRelation = new List<string>() { Fuser_id, item, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
                        if (!da.ExecSql_Parameters(sqlRelation, paramRelation))
                        {
                            throw new LogicException("插入用户访问页面权限关系失败！");
                        }
                    }
                }
                da.Commit();
                return true;
            }
            catch (Exception ex)
            {
                da.RollBack();
                throw ex;
            }
            finally
            {
                da.CloseConn();
                da.Dispose();
            }
        }
        /// <summary>
        /// 检验用户
        /// </summary>
        /// <param name="userLoginAccount">登录账号</param>
        /// <param name="userIdNo">身份证后五位</param>
        /// <returns></returns>
        public bool CheckUser(string userLoginAccount, string userIdNo)
        {
            using (MySqlAccess da = new MySqlAccess(connstr))
            {
                da.OpenConn();

                string sql = "select Fuser_bind_email from c2c_zwdb.t_bank_userinfos where Fuser_bind_email=? and right(Fuser_id_no,5)=? limit 1;";

                List<string> Parameters = new List<string>() { userLoginAccount, userIdNo };

                string temp = da.GetOneResult_Parameters(sql, Parameters);
                if (!string.IsNullOrEmpty(temp))
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="userLoginAccount">登录账号</param>
        /// <returns></returns>
        public DataTable GetUserinfo(string userLoginAccount)
        {
            using (MySqlAccess da = new MySqlAccess(connstr))
            {
                da.OpenConn();
                string sql = "select * from c2c_zwdb.t_bank_userinfos where Fuser_bind_email=? limit 1";
                List<string> Parameters = new List<string>() { userLoginAccount };

                DataTable dt = da.GetTable_Parameters(sql, Parameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    dt.Columns.Add("Flast_update_date_login", typeof(string));
                    string sql1 = @"select Fmodify_time from c2c_zwdb.t_bank_userinfo_login where Fuser_login_account=? limit 1";
                    string Flast_update_date = da.GetOneResult_Parameters(sql1, Parameters);
                    dt.Rows[0]["Flast_update_date_login"] = Flast_update_date;
                }
                return dt;
            }
        }

        /// <summary>
        /// 查询用户信息——批量
        /// </summary>
        /// <param name="userBindEmail"></param>
        /// <param name="bankId"></param>
        /// <param name="userName"></param>
        /// <param name="userIdNo"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public DataTable GetUserinfo2(string userBindEmail, string bankId, string userName, string userIdNo, int offset, int limit)
        {
            using (MySqlAccess da = new MySqlAccess(connstr))
            {
                da.OpenConn();
                List<string> Parameters = new List<string>();
                string sql = "select * from c2c_zwdb.t_bank_userinfos where 1=1 ";
                if (!string.IsNullOrEmpty(userBindEmail))
                {
                    sql += "and Fuser_bind_email=? ";
                    Parameters.Add(userBindEmail);
                }
                if (!string.IsNullOrEmpty(bankId))
                {
                    sql += "and Fbank_id=? ";
                    Parameters.Add(bankId);
                }
                if (!string.IsNullOrEmpty(userName))
                {
                    sql += "and Fuser_name=? ";
                    Parameters.Add(userName);
                }
                if (!string.IsNullOrEmpty(userIdNo))
                {
                    sql += "and Fuser_id_no=? ";
                    Parameters.Add(userIdNo);
                }
                sql += " order by Fcreate_time desc limit " + offset + "," + limit;
                DataTable dt = da.GetTable_Parameters(sql, Parameters);
                return dt;
            }
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="userLoginAccount">登录账号</param>
        /// <param name="newPassword">新密码，默认666666</param>
        /// <returns></returns>
        public bool ResetPwd(string userLoginAccount, out string newPassword)
        {
            newPassword = getRandomizer(7);
            using (MySqlAccess da = new MySqlAccess(connstr))
            {
                da.OpenConn();
                string newpasswordmd5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword, "md5").ToLower();
                string sql1 = "update c2c_zwdb.t_bank_userinfo_login set Fuser_password=?  where Fuser_login_account=? limit 1";
                List<string> Parameters = new List<string> { newpasswordmd5, userLoginAccount };
                return da.ExecSql_Parameters(sql1, Parameters);
            }
        }

        /// <summary>
        /// 生成随机密码
        /// </summary>
        /// <param name="intLength">密码长度</param>
        /// <returns></returns>
        public string getRandomizer(int intLength)
        {
            string strLetter = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";//字母
            string strNumber = "0123456789";//数字
            string strSymbol = "!@#$%^&*()_-+=,.?";//符号
            //定义
            Random ranA = new Random();
            int intResultRound = 0;
            int intA = 0;
            string strB = "";

            //字母
            intA = ranA.Next(0, strLetter.Length);
            strB = strB + strLetter[intA];
            //符号
            intA = ranA.Next(0, strSymbol.Length);
            strB = strB + strSymbol[intA];
            //数字
            intA = ranA.Next(0, strNumber.Length);
            strB = strB + strNumber[intA];

            while (intResultRound < (intLength - 3))
            {
                //生成随机数A，表示生成类型
                //1=数字，2=符号，3=字母
                intA = ranA.Next(1, 4);

                //1=数字
                if (intA == 1)
                {
                    intA = ranA.Next(0, strNumber.Length);
                    strB = strB + strNumber[intA];
                    intResultRound = intResultRound + 1;
                    continue;
                }
                //2=符号
                if (intA == 2)
                {
                    intA = ranA.Next(0, strSymbol.Length);
                    strB = strB + strSymbol[intA];
                    intResultRound = intResultRound + 1;
                    continue;
                }
                //3=字母
                if (intA == 3)
                {
                    intA = ranA.Next(0, strLetter.Length);
                    strB = strB + strLetter[intA];
                    intResultRound = intResultRound + 1;
                    continue;
                }
            }
            return strB;
        }


        /// <summary>
        /// 查询用户权限
        /// </summary>
        /// <param name="userId">外键，用户信息表主键</param>
        /// <returns></returns>
        public DataTable GetUserAuthRelation(string userId)
        {
            using (MySqlAccess da = new MySqlAccess(connstr))
            {
                da.OpenConn();
                string sql = "SELECT * FROM c2c_zwdb.t_userauth_relation WHERE Fuser_id=? ";
                DataTable dt = da.GetTable_Parameters(sql, new List<string>() { userId });
                return dt;
            }
        }


        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="userId">外键，用户信息表主键</param>
        /// <param name="rights">新权限</param>
        /// <returns></returns>
        public bool EditAuthRelation(string userId, List<string> rights)
        {
            MySqlAccess da = new MySqlAccess(connstr);
            try
            {
                da.OpenConn();
                da.StartTran();

                string sql1 = "DELETE FROM c2c_zwdb.t_userauth_relation WHERE Fuser_id=?";
                da.ExecSql_Parameters(sql1, new List<string>() { userId });

                string sql5 = "INSERT INTO c2c_zwdb.t_userauth_relation set Fuser_id=?,Fauth_level=?,Fcreate_time=?;";
                foreach (string item in rights)
                {
                    List<string> paramers6 = new List<string>() { userId, item, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
                    if (!da.ExecSql_Parameters(sql5, paramers6))
                    {
                        throw new LogicException("插入用户访问页面权限关系失败！");
                    }
                }
                da.Commit();
                return true;
            }
            catch (Exception ex)
            {
                da.RollBack();
                throw ex;
            }
            finally
            {
                da.CloseConn();
                da.Dispose();
            }
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="Fuser_bind_email"></param>
        /// <param name="Fcontact_name"></param>
        /// <param name="Fcontact_mobile"></param>
        /// <param name="Fcontact_qq"></param>
        /// <param name="Fcontact_email"></param>
        /// <param name="Fcontact_manager"></param>
        /// <param name="Fremark"></param>
        /// <returns></returns>
        public bool EditUsersInfo(string Fuser_bind_email, string Fcontact_name, string Fcontact_mobile, string Fcontact_qq, string Fcontact_email, string Fcontact_manager, string Fremark)
        {
            using (MySqlAccess da = new MySqlAccess(connstr))
            {
                da.OpenConn();
                string sql1 = "UPDATE c2c_zwdb.t_bank_userinfos SET Fcontact_name=?,Fcontact_mobile=?,Fcontact_qq=?, Fcontact_email=?,Fcontact_manager=?,Fremark=?  WHERE Fuser_bind_email=? LIMIT 1";
                return da.ExecSql_Parameters(sql1, new List<string>() { Fcontact_name, Fcontact_mobile, Fcontact_qq, Fcontact_email, Fcontact_manager, Fremark, Fuser_bind_email });
            }
        }

        /// <summary>
        /// 查询操作记录
        /// </summary>
        /// <param name="Fuser_bind_email">绑定邮箱</param>
        /// <returns></returns>
        public DataTable getUserRecords(string Fuser_bind_email, int offset, int limit)
        {
            using (MySqlAccess da = new MySqlAccess(connstr))
            {
                da.OpenConn();
                string sql1 = "SELECT Fuser_id FROM c2c_zwdb.t_bank_userinfos WHERE Fuser_bind_email=? LIMIT 1";
                string userid = da.GetOneResult_Parameters(sql1, new List<string>() { Fuser_bind_email});

                string sql2 = "SELECT * FROM c2c_zwdb.t_bank_admin_records WHERE Fuser_id=?  ORDER BY Fcreate_time DESC LIMIT ?,?";
                DataTable dt = da.GetTable_Parameters(sql2, new List<string>() { userid, offset.ToString(), limit.ToString() });
                return dt;
            }
        }
        /// <summary>
        /// 插入操作日志
        /// </summary>
        /// <param name="Fuser_bind_email">登录名，绑定邮箱</param>
        /// <param name="TypeId">操作类型</param>
        /// <param name="Reason">理由</param>
        /// <param name="adminname">操作人</param>
        /// <returns></returns>
        public bool InsertRecords(string Fuser_bind_email, string TypeId, string Reason,string adminName)
        {
            using (MySqlAccess da = new MySqlAccess(connstr))
            {
                da.OpenConn();
                string sql1 = "SELECT Fuser_id FROM c2c_zwdb.t_bank_userinfos WHERE Fuser_bind_email=?";
                string userid = da.GetOneResult_Parameters(sql1, new List<string>() { Fuser_bind_email });
                string sql2 = "INSERT INTO c2c_zwdb.t_bank_admin_records SET Fuser_id=?,Ftype_id=?,Frec_reason=?,Fcreate_time=?,Frec_admin_name=?";
                return da.ExecSql_Parameters(sql2, new List<string>() { userid, TypeId, Reason, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") ,adminName });
            }

        }
        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="Fuser_bind_email">登录名，绑定邮箱</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public bool EditUserStatus(string Fuser_bind_email,string Status)
        {
            using (MySqlAccess da = new MySqlAccess(connstr))
            {
                da.OpenConn();
                string sql1 = "UPDATE c2c_zwdb.t_bank_userinfos SET Fuser_status=? where Fuser_bind_email=?";
                return da.ExecSql_Parameters(sql1, new List<string>() { Status, Fuser_bind_email});
            }
        }

    }


}

 
 
 
 