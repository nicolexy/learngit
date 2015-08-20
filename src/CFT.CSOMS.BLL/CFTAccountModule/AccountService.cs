using System;
using CFT.CSOMS.DAL.CFTAccount;
using System.Data;
using CFT.CSOMS.DAL.Infrastructure;
using TENCENT.OSS.C2C.Finance.BankLib;
using System.Collections;
using System.Reflection;
using CFT.CSOMS.COMMLIB;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.Apollo.Logging;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CFT.CSOMS.BLL.TradeModule;
using CFT.CSOMS.BLL.BankCardBindModule;

namespace CFT.CSOMS.BLL.CFTAccountModule
{   
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

        public string QQ2UidX(string qqid)
        {
            return AccountData.ConvertToFuidX(qqid);
        }

        public string QQ2Uid(string qqid)
        {
            return AccountData.ConvertToFuid(qqid);
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

        public DataSet GetUserAccount(string u_QQID, int fcurtype, int istr, int imax)
        {
            return new AccountData().GetUserAccount(u_QQID, fcurtype, istr, imax);
        }

        /// <summary>
        /// 解除用户受控资金
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="cur_type">类型</param>
        /// <param name="balance">金额</param>
        /// <param name="opera">操作人</param>
        /// <param name="dt">解绑资金信息</param>
        /// <returns></returns>
        public bool RemoveUserControlFin(string qqid, string cur_type, string balance, string opera, int type, DataTable dt)
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
            if (new AccountData().RemoveUserControlFin(fuid, cur_type, balance, opera, type))
            {
                foreach (DataRow item in dt.Rows)
                {
                    new CFT.CSOMS.DAL.TradeModule.TradeData().RemoveControledFinLogInsert(qqid, item["FbalanceStr"].ToString(), item["FtypeText"].ToString(), item["cur_type"].ToString(), DateTime.Now, opera);
                }
                return true;
            }
            else return false;
        }

        //解绑单条用户受控资金
        public bool UnbindSingleCtrlFund(string qqid, string uid, string cur_type, string balance)
        {
            DataTable dt = QueryUserCtrlFund(qqid, uid);
            bool isExist = false;
            if (dt == null || dt.Rows.Count == 0)
            {
                return false; //解绑失败
            }
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["cur_type"].ToString().Trim() == cur_type.Trim() && dr["balance"].ToString().Trim() == balance.Trim())
                {
                    isExist = true;
                    string balanceStr = MoneyTransfer.FenToYuan(balance);
                    return RemoveUserControlFin(qqid, cur_type, balanceStr, uid, 3, dt);
                }
            }
            return isExist;
        }

        //解绑当前用户全部受控资金
        public bool UnbindAllCtrlFund(string qqid, string uid)
        {
            DataTable dt = QueryUserCtrlFund(qqid, uid);
            if (dt == null || dt.Rows.Count == 0)
            {
                return false;
            }
            return RemoveUserControlFin(qqid, "", "", uid, 4, dt);
        }

        /// <summary>
        /// 查询用户受控资金
        /// </summary>
        /// <param name="qqid"></param>
        /// <param name="opera"></param>
        /// <returns></returns>
        public DataTable QueryUserCtrlFund(string qqid, string opera)
        {
            try
            {
                DataTable dt = QueryUserControledRecordCgi(qqid, opera);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return null;
                }

                dt.Columns.Add("Fcur_typeName", typeof(string));
                dt.Columns.Add("FstateName", typeof(string));
                dt.Columns.Add("Fcreate_time", typeof(string));
                dt.Columns.Add("FtypeText", typeof(string));
                dt.Columns.Add("Fmodify_time", typeof(string));
                dt.Columns.Add("FbalanceStr", typeof(string));
                dt.Columns.Add("uid", typeof(string));
                dt.Rows[0]["FbalanceStr"] = MoneyTransfer.FenToYuan(dt.Rows[0]["balance"].ToString().Trim());

                foreach (DataRow dr in dt.Rows)
                {
                    string cur_type = dr["cur_type"].ToString();
                    DataSet ds = QueryUserControledRecord(qqid, "", "", cur_type, 0, 1);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        dr["Fcur_typeName"] = "";
                        dr["FstateName"] = "";
                        dr["Fcreate_time"] = "";
                        dr["Ftype_Text"] = "";
                        dr["Fmodify_time"] = "";
                        dr["uid"] = "";
                    }
                    else
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        dr["Fcur_typeName"] = row["Fcur_typeName"].ToString();
                        dr["FstateName"] = row["FlstateName"].ToString();
                        dr["Fcreate_time"] = row["Fcreate_time"].ToString();
                        dr["FtypeText"] = row["FtypeText"].ToString();
                        dr["Fmodify_time"] = row["Fmodify_time"].ToString();
                        dr["uid"] = row["uid"].ToString();
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("QueryUserCtrlFund 查询受控资金异常： " + ex.Message);
                throw new Exception("查询记录为空" + ex.Message);
            }
        }

        public DataSet QueryUserControledRecord(string qqid, string strBeginDate, string strEndDate, string cur_type, int iNumStart, int iNumMax)
        {
            try
            {
                string fuid = PublicRes.ConvertToFuid(qqid);
                if (fuid == null || fuid.Trim() == "")
                    throw new Exception("帐号不存在！");

                QeuryUserControledFinInfoClass query = new QeuryUserControledFinInfoClass(fuid, strBeginDate, strEndDate, cur_type, iNumStart, iNumMax);

                DataSet ds = query.GetResultX_ICE();

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return ds;

                ds.Tables[0].Columns.Add("Fcur_typeName", typeof(string));
                ds.Tables[0].Columns.Add("FlstateName", typeof(string));
                ds.Tables[0].Columns.Add("FtypeText", typeof(string));
                ds.Tables[0].Columns.Add("uid", typeof(string));
                ds.Tables[0].Columns.Add("FbalanceStr", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["uid"] = fuid;
                    dr["FbalanceStr"] = MoneyTransfer.FenToYuan(dr["Fbalance"].ToString()) + "元";

                    switch (dr["Flstate"].ToString())
                    {
                        case "1":
                            {
                                dr["FlstateName"] = "有效"; break;
                            }
                        default:
                            {
                                dr["FlstateName"] = "无效"; break;
                            }
                    }

                    switch (dr["Fcur_type"].ToString())
                    {
                        case "80":
                            {
                                dr["Fcur_typeName"] = "账户类"; break;
                            }
                        case "81":
                            {
                                dr["Fcur_typeName"] = "账户类"; break;
                            }
                        case "82":
                            {
                                dr["Fcur_typeName"] = "账户类"; break;
                            }
                        case "1002":
                            {
                                dr["Fcur_typeName"] = "不可提现付款类"; break;
                            }
                        case "1003":
                            {
                                dr["Fcur_typeName"] = "不可提现付款类"; break;
                            }
                        case "1006":
                            {
                                dr["Fcur_typeName"] = "不可提现付款类"; break;
                            }
                        case "1007":
                            {
                                dr["Fcur_typeName"] = "定向类"; break;
                            }
                        case "1008":
                            {
                                dr["Fcur_typeName"] = "定向类"; break;
                            }
                        case "1005":
                            {
                                dr["Fcur_typeName"] = "定向类"; break;
                            }
                        default:
                            {
                                // 这个是看表取值，因为目前“不可提现类”值太多，所以暂时使用这个判断
                                if (dr["Fcur_type"].ToString().Length == 5 || dr["Fcur_type"].ToString() == "1004")
                                {
                                    dr["Fcur_typeName"] = "不可提现类";
                                    break;
                                }

                                dr["Fcur_typeName"] = "未知类型"; break;
                            }
                    }

                    string type = dr["Fcur_type"].ToString().Trim();
                    if (type != null && type.Length == 5)//5位数去掉前面1按银行类型解析，其他按下文档解析
                    {
                        type = type.Remove(0, 1);
                        dr["FtypeText"] = BankIO.QueryBankName(type);
                    }
                    else
                    {
                        switch (type)
                        {
                            case "1005":
                                {
                                    dr["FtypeText"] = "优秀员工奖金受控"; break;
                                }
                            case "1007":
                                {
                                    dr["FtypeText"] = "航空专区派送现金受控"; break;
                                }
                            case "1008":
                                {
                                    dr["FtypeText"] = "国航赠送现金受控"; break;
                                }
                            case "1010":
                                {
                                    dr["FtypeText"] = "淘点网充值受控"; break;
                                }
                            case "1011":
                                {
                                    dr["FtypeText"] = "paipai大促"; break;
                                }
                            case "1012":
                                {
                                    dr["FtypeText"] = "运通返现资金受控"; break;
                                }
                            case "1013":
                                {
                                    dr["FtypeText"] = "201306拍拍大促"; break;
                                }
                            case "1014":
                                {
                                    dr["FtypeText"] = "航旅B2C授信金额"; break;
                                }
                            default:
                                {
                                    dr["FtypeText"] = "无效" + type; break;
                                }
                        }
                    }

                }
                return ds;
            }
            catch (Exception err)
            {
                throw new Exception("Service处理失败！" + err.Message);
            }
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
                //throw new Exception(string.Format("查询会员信息异常:{0}", ex.Message));
                log4net.LogManager.GetLogger("查询会员信息异常: " + ex.Message);
                return null;
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
        /// 清理证件号码
        /// </summary>
        /// <param name="creid"></param>
        /// <param name="type">用户类型</param>
        /// <returns></returns>
        public bool ClearCreidInfo(string creid, int type, string opera)
        {
            bool ret = new AccountData().ClearCreidInfo(creid, type);
            if (ret)
            {
                WriteClearCreidLog(creid, type, opera);
            }
            return ret;
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

        public bool AddChangeUserInfoLog(string qqid, string cre_type, string cre_type_old, string user_type, string user_type_old, string attid, string attid_old, string commet, string commet_old, string submit_user)
        {
            if (string.IsNullOrEmpty(qqid))
            {
                throw new ArgumentNullException("qqid");
            }

            return new AccountData().AddChangeUserInfoLog(qqid, cre_type, cre_type_old, user_type, user_type_old, attid, attid_old, commet, commet_old, submit_user);
        }

        public DataTable QueryChangeUserInfoLog(string qqid, int offset, int limit)
        {
            if (string.IsNullOrEmpty(qqid))
            {
                throw new ArgumentNullException("qqid");
            }
            return new AccountData().QueryChangeUserInfoLog(qqid, offset, limit);
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

        public Boolean LCTAccStateOperator(string uin, string cre_id, string cre_type, string name, string op_type, string caller_name, string client_ip)
        {
            if (string.IsNullOrEmpty(uin))
            {
                throw new ArgumentNullException("uin");
            }
            if (string.IsNullOrEmpty(cre_id))
            {
                throw new ArgumentNullException("cre_id");
            }
            if (string.IsNullOrEmpty(op_type))
            {
                throw new ArgumentNullException("op_type");
            }

            if (!(op_type == "1" || op_type == "2" || op_type == "3"))
            {
                throw new Exception("理财通账户状态操作类型不正确");
            }

            //查询状态
            Boolean state = new AccountData().LCTAccStateOperator(uin, cre_id, cre_type, name, "3", caller_name, client_ip);

            if (op_type == "3")//查询
                return state;
            else if (op_type == "1")//冻结
            {
                if (state)
                {
                    LogHelper.LogInfo("This account：" + uin + " is already be freezed");
                    return true;
                }
                else
                    return new AccountData().LCTAccStateOperator(uin, cre_id, cre_type, name, "1", caller_name, client_ip);
            }
            else  //(op_type == "2")解冻
            {
                if (!state)
                {
                    LogHelper.LogInfo("This account：" + uin + " is already be Unfreezed");
                    return true;
                }
                else
                    return new AccountData().LCTAccStateOperator(uin, cre_id, cre_type, name, "2", caller_name, client_ip);
            }
        }

        public Boolean LCTAccStateOperator(string uin, string op_type, string caller_name, string client_ip)
        {
            string fuid = AccountData.ConvertToFuid(uin);
            //  string fuid = "540444925";

            if (fuid == null || fuid == "")
            {
                throw new Exception("根据C帐号获取Fuid失败	uin:" + uin);
            }
            if (fuid == null || fuid.Length < 3)
            {
                throw new Exception("内部ID不正确！");
            }

            string errMsg = "";
            string sql = "uid=" + fuid;
            string cre_id = CommQuery.GetOneResultFromICE(sql, CommQuery.QUERY_USERINFO, "Fcreid", out errMsg);
            if (errMsg != "")
                throw new Exception(errMsg);
            string cre_type = CommQuery.GetOneResultFromICE(sql, CommQuery.QUERY_USERINFO, "Fcre_type", out errMsg);
            if (errMsg != "")
                throw new Exception(errMsg);
            return LCTAccStateOperator(uin, cre_id, cre_type, "", op_type, caller_name, client_ip);
        }


        /// <summary>
        /// 查询手机绑定次数
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> QueryMobileBoundNumber(string mobile)
        {
            return new AccountData().QueryMobileBoundNumber(mobile);
        }

        /// <summary>
        /// 查询手机绑定清理日记
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns></returns>
        public DataSet QueryClearMobileNumberLog(string mobile)
        {
            ArrayList keyNameList = new ArrayList()
            {
                  "Fsubmit_user",   //当前操作用户
                 // "FUser_type",     //用户属性
                  "FCreate_time",   //操作时间
                  "FMobile",        //手机号码
                  "MobileBindCount_Old",    //清理前绑定次数
              };
            return PublicRes.QueryKFLog("ClearMobileNumberLog", "Mobile", mobile, keyNameList);
        }

        /// <summary>
        /// 手机绑定清零
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns></returns>
        public bool ClearMobileBoundNumber(string mobile, string keyName, string keyValue, string operat_user, string obj_id, Param[] param)
        {
            var bol = new AccountData().ClearMobileBoundNumber(mobile);
            if (bol)
            {
                PublicRes.WirteKFLog(obj_id, "ClearMobileNumberLog", keyName, keyValue, operat_user, param);//写日志
            }
            return bol;
        }

        /// <summary>
        /// 腾讯信用查询
        /// </summary>
        /// <param name="uin">QQ号</param>
        /// <param name="username">操作员</param>
        /// <returns></returns>
        public DataSet TencentCreditQuery(string uin, string username)
        {
            return new AccountData().TencentCreditQuery(uin, username);
        }

        public DataTable GetFetchListIntercept(string fetchListid)
        {
            if (string.IsNullOrEmpty(fetchListid))
            {
                throw new ArgumentNullException("fetchListid");
            }
            return (new AccountData()).GetFetchListIntercept(fetchListid);
        }

        public bool AddFetchListIntercept(string fetchListid, string opera)
        {
            if (string.IsNullOrEmpty(fetchListid))
            {
                throw new ArgumentNullException("fetchListid");
            }

            var dt = new AccountService().GetFetchListIntercept(fetchListid);
            if (dt != null && dt.Rows.Count > 0)
                throw new Exception("该体现单号已拦截！");

            return (new AccountData()).AddFetchListIntercept(fetchListid, opera);
        }

        #region 销户操作

        /// <summary>
        /// 销户日志查询
        /// </summary>
        /// <returns></returns>
        public DataSet GetCanncelAccountLog(string qqid, string opera, DateTime begin_time, DateTime end_time, int offset, int limit, out string msg)
        {
            return new AccountData().logOnUserHistory(qqid, opera, begin_time, end_time, offset, limit, out msg);
        }

        /// <summary>
        /// 申请销户
        /// </summary>
        /// <param name="query_id"></param>
        /// <param name="query_type">0，财付通，手Q账号；1微信账号</param>
        /// <param name="reason"></param>
        /// <param name="is_Send">是否发送邮件</param>
        /// <param name="opera"></param>
        /// <param name="ip"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool LogOnUserDeleteUser(string query_id, int query_type, string reason, bool is_Send, string email_Addr, string opera, out string ret_msg, out bool ret_continue)
        {
            bool ret_value = false;
            ret_msg = "";
            ret_continue = false;

            if (reason.Length > 255)
            {
                ret_msg = "备注字数不得超过255个字符";
                return false;
            }
            if (is_Send && TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.CheckEmail(email_Addr))
            {
                ret_msg = "请输入正确格式的用户邮箱地址";
                return false;
            }
            string wx_id = "";
            //微信账号
            if (query_type == 1)
            {
                wx_id = query_id;
                query_id = WeChatHelper.GetUINFromWeChatName(query_id);
            }

            string memo = "[注销QQ号码:" + query_id + "]注销原因:" + reason;
            string Msg = "";
            if (!new AccountData().checkUserReg(query_id, out Msg))
            {
                ret_msg = "帐号非法或者未注册！";
                return false;
            }

            //手Q用户转账中、退款中、未完成的订单禁止注销和批量注销
            if (Regex.IsMatch(query_id, @"^[1-9]\d*$"))
            {
                DataSet dsHandQ = new TradeService().QueryPaymentParty("", "1,2,12,4", "3", query_id);
                if (dsHandQ != null && dsHandQ.Tables.Count > 0 && dsHandQ.Tables[0].Rows.Count > 0 && dsHandQ.Tables[0].Rows[0]["result"].ToString() != "97420006")
                {
                    ret_msg = "手Q用户转账中、退款中、未完成的订单禁止注销和批量注销";
                    return false;
                }
                try
                {
                    DataSet dsMobileQHB = new TradeService().GetUnfinishedMobileQHB(query_id);
                    if (dsMobileQHB.Tables[0].Columns.Contains("row_num"))
                    {
                        if (dsMobileQHB != null && dsMobileQHB.Tables.Count > 0 && dsMobileQHB.Tables[0].Rows.Count > 0 && int.Parse(dsMobileQHB.Tables[0].Rows[0]["row_num"].ToString()) > 0)
                        {
                            ret_msg = "该账户存在未完成的手Q红包交易，禁止注销和批量注销";
                            return false;
                        }
                    }
                    else
                    {
                        ret_msg = "查询是否有未完成手Q红包交易失败!";
                        return false;
                    }
                }
                catch (System.Exception ex)
                {
                    LogHelper.LogError("查询手Q红包未完成交易失败" + ex.Message, "LogOnUser");
                    ret_msg = "查询手Q红包未完成交易失败" + ex.Message;
                    return false;
                }
            }

            //有微信支付、转账、红包未完成的禁止注销和批量注销
            if (query_type == 1)
            {
                try
                {
                    int WXUnfinishedTrade = (new TradeService()).QueryWXUnfinishedTrade(wx_id);
                    if (WXUnfinishedTrade > 0)
                    {
                        LogHelper.LogInfo("此账号有未完成微信支付转账，禁止注销!");
                        ret_msg = "此账号有未完成微信支付转账，禁止注销!";
                        return false;
                    }
                }
                catch (System.Exception ex)
                {
                    LogHelper.LogError("销户操作查询微信支付在途条目出错" + ex.Message);
                    ret_msg = "销户操作查询微信支付在途条目出错" + ex.Message;
                    return false;
                }
            }

            //是否有未完成的交易单
            if (new TradeService().LogOnUsercheckOrder(query_id, "1"))
            {
                ret_msg = "有未完成的交易单";
                return false;
            }
            //是否开通一点通
            if (new BankCardBindService().LogOnUserCheckYDT(query_id, "1"))
            {
                ret_msg = "开通了一点通";
                return false;
            }

            //金额大于阀值，提起一个销户申请
            //金额小于阀值,插入logonhistory表,调用接口注销,如果有邮箱,向邮箱发送邮件,反馈信息给一线人员.

            long balance = 0;

            DataTable dt = QuerySubAccountInfo(query_id, 1);    //主帐户余额
            if (dt != null && dt.Rows.Count > 0)
            {
                balance += long.Parse(dt.Rows[0]["Fbalance"].ToString().Trim());
            }
            dt = QuerySubAccountInfo(query_id, 80);     //游戏子帐户
            if (dt != null && dt.Rows.Count > 0)
            {
                balance += long.Parse(dt.Rows[0]["Fbalance"].ToString().Trim());
            }
            dt = QuerySubAccountInfo(query_id, 82);     //直通车子帐户
            if (dt != null && dt.Rows.Count > 0)
            {
                balance += long.Parse(dt.Rows[0]["Fbalance"].ToString().Trim());
            }

            if (balance < 5000) //系统自动销户
            {
                if (new AccountData().LogOnUserDeleteUser(query_id, reason, opera, "", out Msg))
                {
                    throw new Exception(Msg);

                }
                //系统自动注销成功给用户发邮件
                if (is_Send)
                {
                    SendEmail(email_Addr, query_id, "系统自动销户", out Msg);
                }
                ret_msg = "系统自动销户成功！";
                return true;
            }
            else //提起销户申请
            {
                //TODO:提起销户申请
                ret_continue = true;
            }

            return ret_value;
        }

        private bool SendEmail(string email, string qqid, string subject, out string Msg)
        {
            Msg = "";
            try
            {
                string str_params = "p_name=" + qqid + "&p_parm1=" + DateTime.Now + "&p_parm2=" + "" + "&p_parm3=" + "" + "&p_parm4=" + "系统自动销户";
                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(email, "2034", str_params);
                return true;
            }
            catch (Exception err)
            {
                Msg = "给用户发邮件出错：" + err.Message;
                return false;
            }
        }

        #endregion

        #region 个人账户信息

        /// <summary>
        /// 查询个人信息
        /// </summary>
        /// <param name="qqid"></param>
        /// <param name="type">1,C账号;2,内部账号</param>
        /// <returns></returns>
        public DataSet GetPersonalInfo(string qqid, int type)
        {
            try
            {
                string QQID = GetQQIDByUid(qqid, type);
                DataSet ds = new DataSet();

                if (type == 2 && QQID == null)    //查询注销账户信息(可能是注销的账户）
                {
                    ds = GetUserAccountCancel(QQID, 1, 1, 1);
                }
                else
                {
                    if (QQID != null)
                    {
                        bool isWechat = false;
                        ds = new AccountData().GetUserAccount(QQID, 1, 1, 1);

                        if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                        {
                            if (new AccountData().IsFastPayUser(QQID))
                            {
                                ds.Tables[0].Rows[0]["Fname_str"] = "快速交易用户";
                                ds.Tables[0].Rows[0]["Fuser_type_str"] = "";
                            }
                            else
                                return null;
                        }
                        else
                        {

                            ds.Tables[0].Columns.Add("Fcurtype_str", typeof(string));
                            ds.Tables[0].Columns.Add("Fbalance_str", typeof(string));      //单位元
                            ds.Tables[0].Columns.Add("Fquota_str", typeof(string));        //单位元
                            ds.Tables[0].Columns.Add("Fquota_pay_str", typeof(string));    //单位元
                            ds.Tables[0].Columns.Add("Fstate_str", typeof(string));       //账户状态
                            ds.Tables[0].Columns.Add("Fuser_type_str", typeof(string));   //账户类型
                            ds.Tables[0].Columns.Add("Fname_str", typeof(string));        //真实姓名
                            ds.Tables[0].Columns.Add("Ffreeze_fee", typeof(string));      //冻结金额
                            ds.Tables[0].Columns.Add("Fuseable_fee", typeof(string));     //可用余额
                            ds.Tables[0].Columns.Add("Fpro_att", typeof(string));         //产品属性
                            ds.Tables[0].Columns.Add("Ffetch_str", typeof(string));       //当日提现金额
                            ds.Tables[0].Columns.Add("Fsave_str", typeof(string));        //当日已充值金额
                            ds.Tables[0].Columns.Add("Fqqid_state", typeof(string));      //qq关联状态
                            ds.Tables[0].Columns.Add("Femial_state", typeof(string));     //邮箱关联状态
                            ds.Tables[0].Columns.Add("Fmobile_state", typeof(string));    //手机关联状态
                            ds.Tables[0].Columns.Add("Fbpay_state_str", typeof(string));  //余额支付状态

                            #region 转换字段
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                dr["Fcurtype_str"] = TransferMeaning.Transfer.convertMoney_type(PublicRes.objectToString(ds.Tables[0], "Fcurtype"));   //币种类型转换
                                dr["Fbpay_state_str"] = TransferMeaning.Transfer.convertBPAY(PublicRes.objectToString(ds.Tables[0], "Fbpay_state"));       //余额支付状态
                                dr["Fuser_type_str"] = TransferMeaning.Transfer.convertFuser_type(dr["Fuser_type"].ToString());
                                dr["Femail"] = PublicRes.GetString(PublicRes.objectToString(ds.Tables[0], "Femail"));
                                dr["Fmobile"] = PublicRes.GetString(PublicRes.objectToString(ds.Tables[0], "Fmobile"));
                                MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Fbalance", "Fbalance_str");
                                MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Fquota", "Fquota_str");
                                MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Fquota_pay", "Fquota_pay_str");
                                MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Ffetch", "Ffetch_str");
                                MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Fsave", "Fsave_str");

                                string state = PublicRes.objectToString(ds.Tables[0], "Fstate");

                                if (isWechat)
                                {
                                    if (state == "1")
                                    {
                                        dr["Fstate_str"] = "正常";
                                    }
                                    else if (state == "2")
                                    {
                                        dr["Fstate_str"] = "冻结";
                                    }
                                    else
                                    {
                                        dr["Fstate_str"] = "";
                                    }
                                }
                                else
                                {
                                    dr["Fstate_str"] = TransferMeaning.Transfer.accountState(dr["Fstate"].ToString());
                                }

                                string s_fz_amt = PublicRes.objectToString(ds.Tables[0], "Ffz_amt"); //分账冻结金额
                                string s_balance = PublicRes.objectToString(ds.Tables[0], "Fbalance");
                                string s_cron = PublicRes.objectToString(ds.Tables[0], "Fcon");
                                long l_balance = 0, l_cron = 0, l_fzamt = 0;
                                if (s_balance != "")
                                {
                                    l_balance = long.Parse(s_balance);
                                }
                                if (s_cron != "")
                                {
                                    l_cron = long.Parse(s_cron);
                                }
                                if (s_fz_amt != "")
                                {
                                    l_fzamt = long.Parse(s_fz_amt);
                                }

                                dr["Ffreeze_fee"] = MoneyTransfer.FenToYuan((l_fzamt + l_cron).ToString());    //冻结金额=分账冻结金额+冻结金额
                                dr["Fuseable_fee"] = MoneyTransfer.FenToYuan((l_balance - l_cron).ToString());   //可用余额=帐户余额减去冻结余额

                                int tempAtt = 0;
                                if (dr["Att_id"].ToString() != "")
                                {
                                    tempAtt = int.Parse(dr["Att_id"].ToString());
                                }
                                if (tempAtt != 0)
                                {
                                    dr["Fpro_att"] = CheckBasicInfo(tempAtt);
                                }
                                else
                                {
                                    dr["Fpro_att"] = "";
                                }

                                string qq = dr["Fqqid"].ToString();
                                string email = dr["Femail"].ToString();
                                string mobile = dr["Fmobile"].ToString();
                                string fuid = PublicRes.objectToString(ds.Tables[0], "fuid");

                                if (qq != "")
                                {
                                    string uid1 = QQ2Uid(qq);
                                    if (uid1 != null)
                                    {
                                        dr["Fqqid_state"] = "注册未关联";
                                        if (uid1.Trim() == fuid)
                                        {
                                            //再判断是否是已激活
                                            string uid2 = QQ2UidX(qq);
                                            if (uid2 != null)
                                                dr["Fqqid_state"] = "已关联";
                                            else
                                                dr["Fqqid_state"] = "已关联未激活";
                                        }
                                    }
                                    else
                                        dr["Fqqid_state"] = "未注册";

                                }


                                if (email != "")
                                {
                                    string uid1 = QQ2Uid(email);
                                    if (uid1 != null)
                                    {
                                        dr["Femial_state"] = "注册未关联";
                                        if (uid1.Trim() == fuid)
                                        {
                                            string uid2 = QQ2UidX(email);
                                            if (uid2 != null)
                                                dr["Femial_state"] = "已关联";
                                            else
                                                dr["Femial_state"] = "已关联未激活";
                                        }
                                    }
                                    else
                                        dr["Femial_state"] = "未注册";
                                }


                                if (mobile != "")
                                {
                                    string uid1 = QQ2Uid(mobile);
                                    if (uid1 != null)
                                    {
                                        dr["Fmobile_state"] = "注册未关联";
                                        if (uid1.Trim() == fuid)
                                        {
                                            string uid2 = QQ2UidX(mobile);
                                            if (uid2 != null)
                                                dr["Fmobile_state"] = "已关联";
                                            else
                                                dr["Fmobile_state"] = "已关联未激活";
                                        }
                                    }
                                    else
                                        dr["Fmobile_state"] = "未注册";
                                }


                                try
                                {
                                    string name = dr["UserRealName2"].ToString();
                                    if (!string.IsNullOrEmpty(name))
                                    {
                                        dr["Fname_str"] = dr["UserRealName2"].ToString();
                                    }
                                }
                                catch
                                {
                                    dr["Fname_str"] = dr["Ftruename"].ToString();
                                }

                            }

                            #endregion
                        }

                    }
                }

                return ds;
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("查询个人账户信息出错: " + ex.Message);
                return null;
            }
        }

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
        /// 查询注销用户帐户表
        /// </summary>
        /// <param name="fuid"></param>
        /// <param name="fcurtype"></param>
        /// <param name="istr"></param>
        /// <param name="imax"></param>
        /// <returns></returns>
        public DataSet GetUserAccountCancel(string qqid, int fcurtype, int istr, int imax)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = new AccountData().GetUserAccountCancel(qqid, fcurtype, istr, imax);

                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                {
                    return null;
                }
                else
                {

                    ds.Tables[0].Columns.Add("Fcurtype_str", typeof(string));
                    ds.Tables[0].Columns.Add("Fbalance_str", typeof(string));      //单位元
                    ds.Tables[0].Columns.Add("Fquota_str", typeof(string));        //单位元
                    ds.Tables[0].Columns.Add("Fquota_pay_str", typeof(string));    //单位元
                    ds.Tables[0].Columns.Add("Fstate_str", typeof(string));       //账户状态
                    ds.Tables[0].Columns.Add("Fuser_type_str", typeof(string));   //账户类型
                    ds.Tables[0].Columns.Add("Fname_str", typeof(string));        //真实姓名
                    ds.Tables[0].Columns.Add("Ffreeze_fee", typeof(string));      //冻结金额
                    ds.Tables[0].Columns.Add("Fuseable_fee", typeof(string));     //可用余额
                    ds.Tables[0].Columns.Add("Fpro_att", typeof(string));         //产品属性
                    ds.Tables[0].Columns.Add("Ffetch_str", typeof(string));       //当日提现金额
                    ds.Tables[0].Columns.Add("Fsave_str", typeof(string));        //当日已充值金额
                    ds.Tables[0].Columns.Add("Fqqid_state", typeof(string));      //qq关联状态
                    ds.Tables[0].Columns.Add("Femial_state", typeof(string));     //邮箱关联状态
                    ds.Tables[0].Columns.Add("Fmobile_state", typeof(string));    //手机关联状态
                    ds.Tables[0].Columns.Add("Fbpay_state_str", typeof(string));  //余额支付状态

                    #region 转换字段

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["Fcurtype_str"] = TransferMeaning.Transfer.convertMoney_type(PublicRes.objectToString(ds.Tables[0], "Fcurtype"));   //币种类型转换
                        dr["Fbpay_state_str"] = TransferMeaning.Transfer.convertBPAY(PublicRes.objectToString(ds.Tables[0], "Fbpay_state"));       //余额支付状态
                        dr["Fstate_str"] = TransferMeaning.Transfer.accountState(dr["Fstate"].ToString());
                        dr["Fuser_type_str"] = TransferMeaning.Transfer.convertFuser_type(dr["Fuser_type"].ToString());
                        dr["Femail"] = PublicRes.GetString(PublicRes.objectToString(ds.Tables[0], "Femail"));
                        dr["Fmobile"] = PublicRes.GetString(PublicRes.objectToString(ds.Tables[0], "Fmobile"));
                        MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Fbalance", "Fbalance_str");
                        MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Fquota", "Fquota_str");
                        MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Fquota_pay", "Fquota_pay_str");
                        MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Ffetch", "Ffetch_str");
                        MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Fsave", "Fsave_str");

                        string s_fz_amt = PublicRes.objectToString(ds.Tables[0], "Ffz_amt"); //分账冻结金额
                        string s_balance = PublicRes.objectToString(ds.Tables[0], "Fbalance");
                        string s_cron = PublicRes.objectToString(ds.Tables[0], "Fcon");
                        long l_balance = 0, l_cron = 0, l_fzamt = 0;
                        if (s_balance != "")
                        {
                            l_balance = long.Parse(s_balance);
                        }
                        if (s_cron != "")
                        {
                            l_cron = long.Parse(s_cron);
                        }
                        if (s_fz_amt != "")
                        {
                            l_fzamt = long.Parse(s_fz_amt);
                        }

                        dr["Ffreeze_fee"] = MoneyTransfer.FenToYuan((l_fzamt + l_cron).ToString());    //冻结金额=分账冻结金额+冻结金额
                        dr["Fuseable_fee"] = MoneyTransfer.FenToYuan((l_balance - l_cron).ToString());   //可用余额=帐户余额减去冻结余额

                        int tempAtt = 0;
                        if (dr["Att_id"].ToString() != "")
                        {
                            tempAtt = int.Parse(dr["Att_id"].ToString());
                        }
                        if (tempAtt != 0)
                        {
                            dr["Fpro_att"] = CheckBasicInfo(tempAtt);
                        }
                        else
                        {
                            dr["Fpro_att"] = "";
                        }

                        string qq = dr["Fqqid"].ToString();
                        string email = dr["Femail"].ToString();
                        string mobile = dr["Fmobile"].ToString();
                        string fuid = PublicRes.objectToString(ds.Tables[0], "fuid");

                        if (qq != "")
                        {
                            string uid1 = QQ2Uid(qq);
                            if (uid1 != null)
                            {
                                dr["Fqqid_state"] = "注册未关联";
                                if (uid1.Trim() == fuid)
                                {
                                    //再判断是否是已激活
                                    string uid2 = QQ2UidX(qq);
                                    if (uid2 != null)
                                        dr["Fqqid_state"] = "已关联";
                                    else
                                        dr["Fqqid_state"] = "已关联未激活";
                                }
                            }
                            else
                                dr["Fqqid_state"] = "未注册";

                        }


                        if (email != "")
                        {
                            string uid1 = QQ2Uid(email);
                            if (uid1 != null)
                            {
                                dr["Femial_state"] = "注册未关联";
                                if (uid1.Trim() == fuid)
                                {
                                    string uid2 = QQ2UidX(email);
                                    if (uid2 != null)
                                        dr["Femial_state"] = "已关联";
                                    else
                                        dr["Femial_state"] = "已关联未激活";
                                }
                            }
                            else
                                dr["Femial_state"] = "未注册";
                        }


                        if (mobile != "")
                        {
                            string uid1 = QQ2Uid(mobile);
                            if (uid1 != null)
                            {
                                dr["Fmobile_state"] = "注册未关联";
                                if (uid1.Trim() == fuid)
                                {
                                    string uid2 = QQ2UidX(mobile);
                                    if (uid2 != null)
                                        dr["Fmobile_state"] = "已关联";
                                    else
                                        dr["Fmobile_state"] = "已关联未激活";
                                }
                            }
                            else
                                dr["Fmobile_state"] = "未注册";
                        }

                        try
                        {
                            string name = dr["UserRealName2"].ToString();
                            if (!string.IsNullOrEmpty(name))
                            {
                                dr["Fname_str"] = dr["UserRealName2"].ToString();
                            }
                        }
                        catch
                        {
                            dr["Fname_str"] = dr["Ftruename"].ToString();
                        }

                    }

                    return ds;

                    #endregion
                }
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("查询注销用户帐户表: " + ex.Message);
                return null;
            }
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

        private string CheckBasicInfo(int nAttid)
        {
            //从数据字典中读取数据，绑定到web页
            DataSet ds = PermitPara.QueryDicAccName();
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return "";
            }
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                if (nAttid == int.Parse(dr["Value"].ToString().Trim()))
                {
                    return dr["Text"].ToString().Trim();
                }
            }
            return "";
        }

        public string GetQQIDByUid(string qqid, int type)
        {
            if (string.IsNullOrEmpty(qqid))
            {
                throw new Exception("输入账号为空!");
            }

            try
            {
                string id = qqid;
                if (type == 1)//C账号
                {
                    return id;
                }
                else if (type == 2)//内部账号
                {
                    id = Uid2QQ(qqid);
                }
                else
                {
                    throw new Exception("账号类型错误!");
                }
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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

        /// <summary>
        /// 查询用户商家工具按钮表
        /// </summary>      
        public DataSet GetUserButtonInfo(string u_QQID, int istr, int imax)
        {
            return new AccountData().GetUserButtonInfo(u_QQID, istr, imax);
        }

        /// <summary>
        /// 查询用户交易流水表
        /// </summary>
        public DataSet GetUserPayList(string u_ID, int u_IDType, DateTime u_BeginTime, DateTime u_EndTime, int istr, int imax)
        {
            return new AccountData().GetUserPayList(u_ID, u_IDType, u_BeginTime, u_EndTime, istr, imax);
        }

        #endregion

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
        public string Fsubmit_time;
        public string Fsubmit_user;
        public string Fcheck_time;
        public string Fcheck_user;
        public string Fcheck_state;
        public string Frefuse_reason = "";
        public string Fcomment = "";
    }
    #endregion

    #region 实名认证置失效类
    public class AuthenStateDeleteClass
    {
        public string Fuid;
        public string Fname_old;
        public string Fcre_id;
        public string Fcre_type;
        public string Fimage_cre1;
        public string Fimage_cre2;
        public string Fimage_evidence;
        public string Fsubmit_time;
        public string Fsubmit_user;
    }
    #endregion
}
