using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;


namespace CFT.CSOMS.BLL.CFTAccountModule
{
    using CFT.CSOMS.DAL.CFTAccount;
    using System.Data;
    using CFT.CSOMS.DAL.Infrastructure;

    public class AccountService
    {
        public static string ConvertToFuid(string uin)
        {
            return AccountData.ConvertToFuid(uin);
        }

        public static string Uid2QQ(string uin)
        {
            return AccountData.Uid2QQ(uin);
        }

        public static string GetQQID(string queryType, string queryString)
        {
            return AccountData.getQQID(queryType, queryString);
        }

        public DataTable QuerySubAccountInfo(string uin, int currencyType)
        {
            return new AccountData().QuerySubAccountInfo(uin, currencyType);
        }

        public string SynUserName(string aaUin, string oldName, string newName, string wxUin)
        {
            return new AccountData().SynUserName(aaUin, oldName, newName, wxUin);
        }

        /// <summary>
        /// 解除用户受控资金
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="cur_type">类型</param>
        /// <param name="balance">金额</param>
        /// <param name="opera">操作人</param>
        /// <returns></returns>
        public bool RemoveUserControlFin(string qqid, string cur_type, string balance, string opera, int type)
        {
            string fuid = AccountData.ConvertToFuid(qqid);
          //  string fuid = "540444925";

            if (fuid == null || fuid == "")
            {
                throw new Exception("根据C帐号获取Fuid失败	qqid:" + qqid);
            }
            if (fuid == null || fuid.Length < 3)
            {
                throw new Exception("内部ID不正确！");
            }
            return new AccountData().RemoveUserControlFin(fuid, cur_type, balance, opera, type);
        }

        public DataTable QueryUserControledRecordCgi(string qqid, string opera)
        {

            if (string.IsNullOrEmpty(qqid))
            {
                throw new ArgumentNullException("qqid为空！");
            }
            if (string.IsNullOrEmpty(opera))
            {
                throw new ArgumentNullException("opera为空！");
            }

            string fuid = AccountData.ConvertToFuid(qqid);
          //  string fuid = "540444925";

            if (fuid == null || fuid == "")
            {
                throw new Exception("根据C帐号获取Fuid失败	qqid:" + qqid);
            }
            if (fuid == null || fuid.Length < 3)
            {
                throw new Exception("内部ID不正确！");
            }
            return new AccountData().QueryUserControledRecordCgi(fuid, opera);
        }

        public DataTable QueryVipInfo(string uin)
        {
            try
            {
                DataSet ds = new AccountData().QueryVipInfo(uin);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("vipflag_str", typeof(String));
                    ds.Tables[0].Columns.Add("subid_str", typeof(String));
                    DataRow dr = ds.Tables[0].Rows[0];
                    dr.BeginEdit();
                    string vipflag = dr["vipflag"].ToString();
                    if (vipflag == "0")
                        dr["vipflag_str"] = "非会员";
                    else if (vipflag == "1")
                        dr["vipflag_str"] = "普通会员";
                    else if (vipflag == "2")
                        dr["vipflag_str"] = "VIP会员";
                    else if (vipflag == "4")
                        dr["vipflag_str"] = "连续1个月不做任务的普通会员（同非会员）";
                    else
                        dr["vipflag_str"] = "Unknown";

                    string subid = dr["subid"].ToString();
                    if (subid == "0")
                        dr["subid_str"] = "无支付方式";
                    else if (subid == "1")
                        dr["subid_str"] = "手机支付";
                    else if (subid == "2")
                        dr["subid_str"] = "个人帐户支付";
                    else if (subid == "3")
                        dr["subid_str"] = "Vnet支付";
                    else if (subid == "4")
                        dr["subid_str"] = "PHS小灵通支付";
                    else if (subid == "5")
                        dr["subid_str"] = "银行支付";
                    else if (subid == "100")
                        dr["subid_str"] = "Q币卡支付";
                    else if (subid == "101")
                        dr["subid_str"] = "声讯支付";
                    else if (subid == "102")
                        dr["subid_str"] = "ESALES支付";
                    else if (subid == "103")
                        dr["subid_str"] = "他人赠送";
                    else if (subid == "104")
                        dr["subid_str"] = "索要";
                    else if (subid == "105")
                        dr["subid_str"] = "公司活动赠送";
                    else if (subid == "106")
                        dr["subid_str"] = "免费（用于公免）";
                    else if (subid == "107")
                        dr["subid_str"] = "电信卡";
                    else if (subid == "108")
                        dr["subid_str"] = "缴费卡支付";
                    else if (subid == "109")
                        dr["subid_str"] = "积分";
                    else if (subid == "110")
                        dr["subid_str"] = "广州收费易";
                    else if (subid == "111")
                        dr["subid_str"] = "Q点";
                    else if (subid == "112")
                        dr["subid_str"] = "EPAY";
                    else if (subid == "113")
                        dr["subid_str"] = "MPAY";
                    else if (subid == "114")
                        dr["subid_str"] = "声讯预付费（活动接口）";
                    else if (subid == "115")
                        dr["subid_str"] = "PPW_PAIPAI 拍拍渠道";
                    else if (subid == "116")
                        dr["subid_str"] = "赠送索回";
                    else if (subid == "117")
                        dr["subid_str"] = "声讯预付费  2006.10,移动联通";
                    else if (subid == "118")
                        dr["subid_str"] = "Q币不足Q点支付";
                    else if (subid == "119")
                        dr["subid_str"] = "Q点不足Q币支付";
                    else if (subid == "120")
                        dr["subid_str"] = "移动do分离";
                    else if (subid == "121")
                        dr["subid_str"] = "tenpay";
                    else if (subid == "122")
                        dr["subid_str"] = "手机声讯渠道";
                    else if (subid == "123")
                        dr["subid_str"] = "统一帐户渠道";
                    else
                        dr["subid_str"] = "Unknown";
                    dr.EndEdit();

                    return ds.Tables[0];
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("查询会员信息异常:{0}", ex.Message));
            }
        }

        /// <summary>
        /// 清理日志查询
        /// </summary>
        /// <param name="creid">身份证号</param>
        /// <returns></returns>
        public DataTable GetClearCreidLog(string creid)
        {
            return (new AccountData()).GetClearCreidLog(creid);
        }
        public DataSet QueryNameAbnormalInfo(string uin, int check_state, string cre_id_old, int limit, int offset)
        {
            return new AccountData().QueryNameAbnormalInfo(uin, check_state, cre_id_old, limit, offset);
        }

        /// <summary>
        /// 记录证件号码清理日志
        /// </summary>
        /// <param name="creid"></param>
        /// <param name="userType"></param>
        /// <param name="Uid"></param>
        public void WriteClearCreidLog(string creid, int userType, string Uid)
        {
            (new AccountData()).WriteClearCreidLog(creid, userType, Uid);
        }

        /// <summary>
        /// 获取清理次数
        /// </summary>
        /// <param name="creid"></param>
        /// <returns></returns>
        public int GetClearCreidCount(string creid, int userType)
        {
            return (new AccountData()).GetClearCreidCount(creid, userType);
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
    }

    #region 异常姓名类
    public class NameAbnormalClass
    {
        public string Fuin;
        public string Fname_old;
        public string Fcre_id_old;
        public string Ftruename;
        public string Fcre_id;
        public string Fcre_type;
        public string Fcre_version;
        public string Fcre_valid_day;
        public string Faddress;
        public string Fimage_cre1;
        public string Fimage_cre2;
        public string Fimage_evidence;
        public string Fimage_other;
        public string Fsubmit_time ;
        public string Fsubmit_user;
        public string Fcheck_time ;
        public string Fcheck_user;
        public string Fcheck_state;
        public string Frefuse_reason="";
        public string Fcomment="";

    }
    #endregion
}
