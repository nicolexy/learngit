using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;


namespace CFT.CSOMS.BLL.CFTAccountModule
{
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
                    throw new Exception("查询记录为空");
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
              return  new AccountData().TencentCreditQuery(uin,username);
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
