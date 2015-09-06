using CFT.CSOMS.COMMLIB;
using CFT.CSOMS.DAL.CFTAccount;
using CFT.CSOMS.DAL.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TENCENT.OSS.C2C.Finance.BankLib;

namespace CFT.CSOMS.BLL.CFTAccountModule
{
    /// <summary>
    /// 实名认证,姓名异常
    /// </summary>
    public class AuthenInfoService
    {
        #region 异常姓名

        public DataSet QueryNameAbnormalInfo(string uin, int check_state, string cre_id_old, int limit, int offset)
        {
            return new AccountData().QueryNameAbnormalInfo(uin, check_state, cre_id_old, limit, offset);
        }

        public void AddNameAbnormalInfo(NameAbnormalClass nameAbnormal)
        {
            CFT.CSOMS.DAL.CFTAccount.NameAbnormalClass nameA = new DAL.CFTAccount.NameAbnormalClass();
            nameA.Fuin = nameAbnormal.Fuin;
            nameA.Fname_old = nameAbnormal.Fname_old;
            nameA.Fcre_id_old = nameAbnormal.Fcre_id_old;
            nameA.Ftruename = nameAbnormal.Ftruename;
            nameA.Fcre_id = nameAbnormal.Fcre_id;
            nameA.Fcre_type = nameAbnormal.Fcre_type;
            nameA.Fcre_version = nameAbnormal.Fcre_version;
            nameA.Fcre_valid_day = nameAbnormal.Fcre_valid_day;
            nameA.Faddress = nameAbnormal.Faddress;
            nameA.Fimage_cre1 = nameAbnormal.Fimage_cre1;
            nameA.Fimage_cre2 = nameAbnormal.Fimage_cre2;
            nameA.Fimage_evidence = nameAbnormal.Fimage_evidence;
            nameA.Fimage_other = nameAbnormal.Fimage_other;
            nameA.Fsubmit_time = nameAbnormal.Fsubmit_time;
            nameA.Fsubmit_user = nameAbnormal.Fsubmit_user;
            nameA.Fcheck_time = nameAbnormal.Fcheck_time;
            nameA.Fcheck_user = nameAbnormal.Fcheck_user;
            nameA.Fcheck_state = nameAbnormal.Fcheck_state;
            nameA.Frefuse_reason = nameAbnormal.Frefuse_reason;
            nameA.Fcomment = nameAbnormal.Fcomment;

            new AccountData().AddNameAbnormalInfo(nameA);
        }

        public bool UpdateNameAbnormalInfo(string uin, string refuse_reason, string comment, string check_user, string check_state)
        {
            return new AccountData().UpdateNameAbnormalInfo(uin, refuse_reason, comment, check_user, check_state);
        }

        public DataSet QueryRealNameInfo(string uin, string submit_user)
        {
            return new AccountData().QueryRealNameInfo(uin, submit_user);
        }

        /// <summary>
        /// au_append_cre_info_service
        /// 通过relay 补填用户信息：姓名 证件号
        /// </summary>
        /// <param name="uin"></param>
        /// <param name="check_user"></param>
        /// <returns></returns>
        public bool UpdateRealNameInfo(NameAbnormalClass nameAbnormal)
        {
            DataSet ds = QueryNameAbnormalInfo(nameAbnormal.Fuin, 0, nameAbnormal.Fcre_id_old, 0, 1);//check_state=0 未处理状态

            if (!(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0))
            {
                throw new Exception("申请单状态不正确！");
            }

            CFT.CSOMS.DAL.CFTAccount.NameAbnormalClass nameA = new DAL.CFTAccount.NameAbnormalClass();
            nameA.Fuin = nameAbnormal.Fuin;
            nameA.Fname_old = nameAbnormal.Fname_old;
            nameA.Fcre_id_old = nameAbnormal.Fcre_id_old;
            nameA.Ftruename = nameAbnormal.Ftruename;
            nameA.Fcre_id = nameAbnormal.Fcre_id;
            nameA.Fcre_type = nameAbnormal.Fcre_type;
            nameA.Fcre_version = nameAbnormal.Fcre_version;
            nameA.Fcre_valid_day = nameAbnormal.Fcre_valid_day;
            nameA.Faddress = nameAbnormal.Faddress;
            nameA.Fimage_cre1 = nameAbnormal.Fimage_cre1;
            nameA.Fimage_cre2 = nameAbnormal.Fimage_cre2;
            nameA.Fimage_evidence = nameAbnormal.Fimage_evidence;
            nameA.Fimage_other = nameAbnormal.Fimage_other;
            nameA.Fsubmit_time = nameAbnormal.Fsubmit_time;
            nameA.Fsubmit_user = nameAbnormal.Fsubmit_user;
            nameA.Fcheck_time = nameAbnormal.Fcheck_time;
            nameA.Fcheck_user = nameAbnormal.Fcheck_user;
            nameA.Fcheck_state = nameAbnormal.Fcheck_state;
            nameA.Frefuse_reason = nameAbnormal.Frefuse_reason;
            nameA.Fcomment = nameAbnormal.Fcomment;

            new AccountData().UpdateRealNameInfo(nameA);

            //修改库表审批信息
            if (new AccountData().UpdateNameAbnormalInfo(nameA.Fuin, nameA.Frefuse_reason, nameA.Fcomment, nameA.Fcheck_user, nameA.Fcheck_state))
                return true;
            else
                return false;
        }


        public DataSet QueryUserAuthenByCredid(string cre_type, string cre_id, string opera)
        {
            DataSet ds = new AccountData().QueryUserAuthenByCredid(cre_type, cre_id, opera);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string key = System.Configuration.ConfigurationManager.AppSettings["RealNameKey"].ToString();
                key += opera;
                ds.Tables[0].Rows[0]["cname"] = CommUtil.TripleDESDecryptRealName(ds.Tables[0].Rows[0]["cname"].ToString(), key);
                ds.Tables[0].Rows[0]["cuin"] = CommUtil.TripleDESDecryptRealName(ds.Tables[0].Rows[0]["cuin"].ToString(), key);
                ds.Tables[0].Rows[0]["cuid"] = CommUtil.TripleDESDecryptRealName(ds.Tables[0].Rows[0]["cuid"].ToString(), key);
            }
            return ds;
        }

        /// <summary>
        /// 实名认证置失效 并记录客服系统日志
        /// </summary>
        /// <param name="cre_type">证件类型</param>
        /// <param name="cre_id">证件号</param>
        /// <param name="opera">操作者</param>
        /// <param name="memo">说明</param>
        /// <param name="FObjID">objid 唯一</param>
        /// <param name="log_type">日志业务类型</param>
        /// <param name="key_name">关键字段</param>
        /// <param name="myParams">参数列表</param>
        /// <returns></returns>
        public Boolean DisableUserAuthenInfo(string cre_type, string cre_id, string opera, string memo,
            string FObjID, string log_type, string key_name, Param[] myParams)
        {
            if (string.IsNullOrEmpty(cre_type))
            {
                throw new ArgumentNullException("cre_type");
            }
            if (string.IsNullOrEmpty(cre_id))
            {
                throw new ArgumentNullException("cre_id");
            }
            if (string.IsNullOrEmpty(opera))
            {
                throw new ArgumentNullException("opera");
            }

            try
            {
                if (new AccountData().DisableUserAuthenInfo(cre_type, cre_id, opera, memo))//实名认证置失效
                    PublicRes.WirteKFLog(FObjID, log_type, key_name, cre_id, opera, myParams);//写日志
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public DataSet QueryUserAuthenDisableLog(string cre_id)
        {
            if (string.IsNullOrEmpty(cre_id))
            {
                throw new ArgumentNullException("cre_id");
            }
            ArrayList keyNameList = new ArrayList();
            keyNameList.Add("Fuid");
            keyNameList.Add("Fname_old");
            keyNameList.Add("Fcre_id");
            keyNameList.Add("Fcre_type");
            keyNameList.Add("Fimage_cre1");
            keyNameList.Add("Fimage_cre2");
            keyNameList.Add("Fimage_evidence");
            keyNameList.Add("Fsubmit_time");
            keyNameList.Add("Fsubmit_user");
            return PublicRes.QueryKFLog("UserAuthenDisableLog", "cre_id", cre_id, keyNameList);
        }

        #endregion

        #region 个人账户信息

        /// <summary>
        /// 实名认证状态
        /// </summary>
        /// <param name="qqid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public int GetUserClassInfo(string qqid, out string msg)
        {
            return new AccountData().GetUserClassInfo(qqid, out msg);
        }

        /// <summary>
        /// 查询删除实名认证日志
        /// </summary>
        /// <param name="Fqqid"></param>
        /// <returns></returns>
        public DataSet GetUserClassDeleteList(string Fqqid)
        {
            try
            {
                return UserClassClass.GetDeleteList(Fqqid);
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("查询删除实名认证日志失败: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 删除认证信息
        /// </summary>
        /// <param name="qqid"></param>
        /// <param name="username"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool DelAuthen(string qqid, string username, out string msg)
        {
            return new AccountData().DelAuthen(qqid, username, out msg);
        }

        #endregion

        /// <summary>
        /// 实名认证处理查询--专为(实名认证处理查询)做的查询  把权限做到页面里面
        /// </summary>
        /// <param name="fuin"></param>
        /// <param name="iPageStart"></param>
        /// <param name="iPageMax"></param>
        /// <returns></returns>
        public DataSet GetUserClassQueryListForThis(string fuin, int iPageStart, int iPageMax)
        {
            try
            {
                UserClassClass cuser = new UserClassClass(fuin, "UserClassQuery");
                DataSet ds = cuser.GetResultX(iPageStart, iPageMax, "RU");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //这里和页面不同,引入页面时需要加上一个URL地址的组成
                    ds.Tables[0].Columns.Add("Result", typeof(string));                  
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {                    
                        if (dr["Fcard_stat"].ToString() == "1" && dr["Fcre_stat"].ToString() == "1")
                        {
                            dr["Result"] = "认证成功";
                        }
                        else if (dr["Fcard_stat"].ToString() == "9" && dr["Fcre_stat"].ToString() == "9")
                        {
                            dr["Result"] = "认证失败";
                        }
                        else if (dr["Fcard_stat"].ToString() == "10" && dr["Fcre_stat"].ToString() == "10")
                        {
                            dr["Result"] = "作废";
                        }
                        else
                        {
                            dr["Result"] = "认证处理中";
                        }
                    }
                }

                return ds;
            }
            catch (Exception err)
            {
                throw new Exception("Service处理失败！");
            }
        }

    }  
}
