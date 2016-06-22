using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Net.Sockets;
using System.Net;
using System.Web;
using System.Collections;
using CFT.CSOMS.BLL.TransferMeaning;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.CSOMS.DAL.RealNameModule;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.COMMLIB;
using SunLibraryEX;
using CFT.CSOMS.BLL.CFTAccountModule;
namespace CFT.CSOMS.BLL.RealNameModule
{
    public class RealNameCertificateService
    {
        public bool AuMaintainWhiteListC(string src, string Operator, string uin, Int64 uid, int op_type, int valid_days, string sign, out string msg)
        {
            DataSet ds = new RealNameCertificateData().AuMaintainWhiteListC(src, Operator, uin, uid, op_type, valid_days, sign);
            bool ret = false;
            msg = "";
            try
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["result"].ToString()) && int.Parse(ds.Tables[0].Rows[0]["result"].ToString()) == 0)
                    {
                        ret = true;
                    }
                    msg = ds.Tables[0].Rows[0]["res_info"].ToString();
                }
            }
            catch (Exception e)
            {
                throw new Exception("对指定账户添加或者删除白名单：" + e.Message);
            }
            return ret;
        }


        public DataSet BindQryBindMaskInfoC(string src, string Operator, Int64 uid, string sign)
        {
            DataSet ds = new RealNameCertificateData().BindQryBindMaskInfoC(src, Operator, uid, sign);
            try
            {
                //对返回的xml解密的秘钥
                string key = Operator + System.Configuration.ConfigurationManager.AppSettings["BindQryBindMaskInfoCKey"].ToString();
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["result"].ToString()) && int.Parse(dr["result"].ToString()) == 0)
                        {
                            dr["bind_bank_info"] = CommUtil.TripleDESDecryptRealName(dr["bind_bank_info"].ToString(), key);
                            string temp = "result=" + dr["result"].ToString() + "&" + dr["bind_bank_info"];
                            DataTable dt = CommQuery.ParseRelayStringToDataTableNew(temp, "bind_bank_num", "row");
                            if (dt != null)
                            {
                                ds.Tables.Add(dt);
                            }
                        }

                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("银行认证渠道信息查询接口：" + e.Message);
            }
            return ds;
        }


        public DataSet AuQryAuthStatusServiceByQueryType0(int query_type, string sp_id, string cre_id, string cre_type, string truename, string uin)
        {
            DataSet ds = new RealNameCertificateData().AuQryAuthStatusServiceByQueryType0(query_type, sp_id, cre_id, cre_type, truename, uin);
            return ds;
        }

        public DataSet AuQryAuthStatusServiceByQueryType0(int query_type, string sp_id, string cre_id, string cre_type, string truename, string uin, string src, string Operator, string sign, int request_detail_info)
        {
            DataSet ds = new RealNameCertificateData().AuQryAuthStatusServiceByQueryType0(query_type, sp_id, cre_id, cre_type, truename, uin, src, Operator, sign, request_detail_info);
            try
            {
                //对返回的xml解密的秘钥
                string key = Operator + System.Configuration.ConfigurationManager.AppSettings["AuQryAuthStatusServiceKey"].ToString();
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["result"].ToString()) && int.Parse(dr["result"].ToString()) == 0)
                        {
                            dr["gov_authen_info"] = CommUtil.TripleDESDecryptRealName(dr["gov_authen_info"].ToString(), key);
                            dr["ocr_authen_info"] = CommUtil.TripleDESDecryptRealName(dr["ocr_authen_info"].ToString(), key);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("银行认证渠道信息查询接口：" + e.Message);
            }
            return ds;
        }

        public DataSet AuQryAuthStatusServiceByQueryType1(int query_type, string uin, Int64 uid)
        {
            DataSet ds = new RealNameCertificateData().AuQryAuthStatusServiceByQueryType1(query_type, uin, uid);

            return ds;
        }
        public DataSet AuQryAuthStatusServiceByQueryType1(int query_type, string uin, Int64 uid, int request_detail_info, string src, string Operator, string sign)
        {
            DataSet ds = new RealNameCertificateData().AuQryAuthStatusServiceByQueryType1(query_type, uin, uid, request_detail_info, src, Operator, sign);
            try
            {
                //对返回的xml解密的秘钥
                string key = Operator + System.Configuration.ConfigurationManager.AppSettings["AuQryAuthStatusServiceKey"].ToString();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Columns.Contains("gov_authen_info") && ds.Tables[0].Columns.Contains("ocr_authen_info"))
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (!string.IsNullOrEmpty(dr["result"].ToString()) && int.Parse(dr["result"].ToString()) == 0)
                            {
                                dr["gov_authen_info"] = CommUtil.TripleDESDecryptRealName(dr["gov_authen_info"].ToString(), key);
                                dr["ocr_authen_info"] = CommUtil.TripleDESDecryptRealName(dr["ocr_authen_info"].ToString(), key);
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("银行认证渠道信息查询接口 ：" + e.Message);
            }
            return ds;
        }

        public DataSet PQueryUserInfoService(string uin, Int64 uid, string query_attach, int curtype)
        {
            DataSet ds = new RealNameCertificateData().PQueryUserInfoService(uin, uid, query_attach, curtype);
            try
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (!string.IsNullOrEmpty(query_attach))
                        {
                            if (query_attach.ToUpper().Contains("QUERY_USERINFO"))
                            {
                                if (!string.IsNullOrEmpty(dr["cre_id"].ToString()))
                                {
                                    dr["cre_id"] = CommUtil.BankIDEncode_ForBankCardUnbind(dr["cre_id"].ToString()).Replace("\0", "");

                                }
                            }
                        }

                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("账户信息查询 解密失败 ：" + e.Message);
            }

            return ds;
        }

        public DataSet PQueryCreRelationC(string cre_id, int cre_type)
        {
            if (!string.IsNullOrEmpty(cre_id)) { cre_id = CommUtil.EncryptZerosPadding(cre_id); }
            DataSet ds = new RealNameCertificateData().PQueryCreRelationC(cre_id, cre_type);

            return ds;
        }

        public DataSet QuotaBanQueryC(int uid_type, Int64 uid, int have_cre_photocopy, string cre_id, int cre_type)
        {
            if (!string.IsNullOrEmpty(cre_id)) { cre_id = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(cre_id.ToString(), "md5").ToLower(); }
            DataSet ds = new RealNameCertificateData().QuotaBanQueryC(uid_type, uid, have_cre_photocopy, cre_id, cre_type);

            return ds;
        }

        public DataSet QuotaBanQueryC(int uid_type, Int64 uid, int have_cre_photocpy)
        {
            DataSet ds = new RealNameCertificateData().QuotaBanQueryC(uid_type, uid, have_cre_photocpy);

            return ds;
        }

        //通过证件号获取实名认证的相关信息
        public DataTable GetInfoByIdentityCard(string identityId, string Operator)
        {
            DataTable dt = CreateCommonDt();
            try
            {
                GetCreRelationCByIdentitiyId(identityId, ref dt);
                GetUserInfoByUidList(ref dt);
                GetAuthStatusInfo(Operator, ref dt);
                GetBindCardInfo(Operator, ref dt);
            }
            catch (Exception ex)
            {
                dt = null;
                loger.err("GetInfoByIdentityCard", ex.Message);               
            }
            return dt;
        }

        //通过账号和账号类型获取实名认证的相关信息
        public DataTable GetInfoByUid(string username, string usertype, string Operator)
        {
            DataTable dt = CreateCommonDt();
            try
            {
                string uin = AccountService.GetQQID(usertype, username);
                string uid = new AccountService().QQ2Uid(uin);
                if (!string.IsNullOrEmpty(uid))
                {
                    DataRow row = dt.NewRow();
                    row["uid"] = uid;
                    dt.Rows.Add(row);
                    GetUserInfoByUidList(ref dt);
                    GetAuthStatusInfo(Operator, ref dt);
                    GetBindCardInfo(Operator, ref dt);
                }
            }
            catch (Exception ex)
            {
                dt = null;
                loger.err("GetInfoByUid", ex.Message);               
            }
            return dt;
        }

        /// <summary>
        /// 通过证件号获取账户列表，并增加记录到dt
        /// </summary>  
        public void GetCreRelationCByIdentitiyId(string identityId, ref DataTable dt)
        {
            try
            {
                DataSet ds_relationCardUid = new RealNameCertificateService().PQueryCreRelationC(identityId, 1);
                if (ds_relationCardUid != null || ds_relationCardUid.Tables.Count > 0 || ds_relationCardUid.Tables[0].Rows.Count == 1)
                {
                    string uid_list = ds_relationCardUid.Tables[0].Rows[0]["uid_list"].ToString();

                    if (!string.IsNullOrEmpty(uid_list))
                    {
                        string[] uid_arr = uid_list.Split('|');
                        for (int i = 0; i < uid_arr.Length; i++)
                        {
                            DataRow row = dt.NewRow();
                            row["uid"] = uid_arr[i];
                            dt.Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("GetCreRelationCByIdentitiyId 通过证件号获取账户列表异常： " + ex.Message);
                throw new Exception("查询记录为空" + ex.Message);
            }
        }

        /// <summary>
        /// 获取账户限额明细
        /// </summary>    
        public DataTable GetQuotaDetail(int uid_type, Int64 uid, int cre_type, string cre_id, int have_cre_photocopy)
        {
            DataSet ds = null;
            DataTable dt = null;
            try
            {
                if (uid_type == 1 | uid_type == 2 || uid_type == 3)
                {
                    ds = new RealNameCertificateService().QuotaBanQueryC(uid_type, uid, have_cre_photocopy, cre_id, cre_type);
                }
                else
                {
                    ds = new RealNameCertificateService().QuotaBanQueryC(uid_type, uid, have_cre_photocopy);
                }
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            catch (Exception e)
            {
                dt = null;
            }
            return dt;
        }

        /// <summary>
        /// 通过包含uid的Dt找关联的用户信息，并更新每一行的相关数据
        /// </summary>       
        public void GetUserInfoByUidList(ref DataTable dt)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Int64 uid = Int64.Parse(dt.Rows[i]["uid"].ToString());
                        DataSet ds_userinfo = PQueryUserInfoService("", uid, "QUERY_USERINFO", 1);
                        if (ds_userinfo != null || ds_userinfo.Tables.Count > 0 || ds_userinfo.Tables[0].Rows.Count == 1)
                        {
                            DataRow cur_row = dt.Rows[i];
                            cur_row.BeginEdit();
                            cur_row["uin"] = ds_userinfo.Tables[0].Rows[0]["uin"];
                            cur_row["authen_account_type"] = GetAccountType(ds_userinfo.Tables[0].Rows[0]["authen_account_type"].ToString());
                            cur_row["uid_type"] = ds_userinfo.Tables[0].Rows[0]["authen_account_type"];
                            cur_row["authen_channel_state"] = ds_userinfo.Tables[0].Rows[0]["authen_channel_state"];
                            cur_row["user_true_name"] = ds_userinfo.Tables[0].Rows[0]["user_true_name"];
                            cur_row["cre_type"] = ds_userinfo.Tables[0].Rows[0]["cre_type"];
                            cur_row["cre_type_txt"] = GetCreTypeText((ds_userinfo.Tables[0].Rows[0]["cre_type"] != null && ds_userinfo.Tables[0].Rows[0]["cre_type"].ToString()!="")?ds_userinfo.Tables[0].Rows[0]["cre_type"].ToString():"0");
                            cur_row["cre_id"] = ds_userinfo.Tables[0].Rows[0]["cre_id"];
                            cur_row.EndEdit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("GetUserInfoByUidList 通过包含uid的Dt找关联的用户信息异常： " + ex.Message);
                throw new Exception("查询记录为空" + ex.Message);
            }

        }
        /// <summary>
        /// 通过包含uid,uin的Dt找关联的用户认证信息，并更新每一行的相关数据
        /// </summary>     
        public void GetAuthStatusInfo(string Operator, ref DataTable dt)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Int64 uid = Int64.Parse(dt.Rows[i]["uid"].ToString());
                        string uin = dt.Rows[i]["uin"].ToString();
                        string temp = uin + "|" + uid + "|" + Operator + "|kf.cf.com|";
                        string sign = FormatStrEnscript(temp, "AuQryAuthStatusServiceKey");
                        DataSet ds_auth = AuQryAuthStatusServiceByQueryType1(1, uin, uid, 1, "kf.cf.com", Operator, sign);
                        if (ds_auth != null || ds_auth.Tables.Count > 0 || ds_auth.Tables[0].Rows.Count == 1)
                        {
                            DataRow cur_row = dt.Rows[i];
                            cur_row.BeginEdit();
                            if (ds_auth.Tables[0].Columns.Contains("gov_auth_fail_reason"))
                            {
                                cur_row["gov_auth_result"] = GetAuthResult(ds_auth.Tables[0].Rows[0]["gov_auth_fail_reason"].ToString());
                            }
                            else
                            {
                                if (ds_auth.Tables[0].Columns.Contains("gov_authen_info") && ds_auth.Tables[0].Rows[0]["gov_authen_info"] != null && ds_auth.Tables[0].Rows[0]["gov_authen_info"].ToString() != "")
                                {
                                    cur_row["gov_auth_result"] = "公安部实名成功";
                                }
                                else
                                {
                                    cur_row["gov_auth_result"] = "未去公安部实名";
                                }
                            }
                            if (ds_auth.Tables[0].Rows[0]["gov_authen_info"] != null && ds_auth.Tables[0].Rows[0]["gov_authen_info"].ToString() != "")
                            {
                                string gov_auth_fail_reason_dt = "authen_time";
                                DealStr(ds_auth.Tables[0].Rows[0]["gov_authen_info"].ToString(), ref gov_auth_fail_reason_dt);
                                cur_row["gov_auth_fail_reason_dt"] = gov_auth_fail_reason_dt;
                            }
                            if (ds_auth.Tables[0].Rows[0]["ocr_authen_info"] != null && ds_auth.Tables[0].Rows[0]["ocr_authen_info"].ToString() != "")
                            {
                                string ocr_authen_info_dt = "authen_time";
                                DealStr(ds_auth.Tables[0].Rows[0]["ocr_authen_info"].ToString(), ref ocr_authen_info_dt);
                                cur_row["ocr_authen_info_dt"] = ocr_authen_info_dt;
                            }
                            cur_row.EndEdit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("GetAuthStatusInfo 通过包含uid,uin的Dt找关联的用户认证信息异常： " + ex.Message);
                throw new Exception("查询记录为空" + ex.Message);
            }
        }
        /// <summary>
        /// 通过包含uid的Dt找uid所属的绑卡信息，并更新每一行的相关数据
        /// </summary> 
        public void GetBindCardInfo(string Operator, ref DataTable dt)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Int64 uid = Int64.Parse(dt.Rows[i]["uid"].ToString());
                        string temp = uid + "|" + Operator + "|kf.cf.com|";
                        string sign = FormatStrEnscript(temp, "BindQryBindMaskInfoCKey");
                        DataSet ds_bindcard = new RealNameCertificateService().BindQryBindMaskInfoC("kf.cf.com", Operator, uid, sign);
                        if (ds_bindcard != null || ds_bindcard.Tables.Count > 0 || ds_bindcard.Tables[0].Rows.Count == 1)
                        {
                            DataRow cur_row = dt.Rows[i];
                            cur_row.BeginEdit();
                            cur_row["bind_bank_info"] = ds_bindcard.Tables[0].Rows[0]["bind_bank_info"];
                            cur_row.EndEdit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("GetBindCardInfo 通过包含uid的Dt找uid所属的绑卡信息异常： " + ex.Message);
                throw new Exception("查询记录为空" + ex.Message);
            }
        }

        #region 动态分割DataTable分页
        public DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)
        {
            if (PageIndex == 0||dt==null||dt.Rows.Count==0)
            {
                return dt;
            }
            DataTable newdt = dt.Copy();
            newdt.Clear();
            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
            {
                return newdt;
            }

            if (rowend > dt.Rows.Count)
            {
                rowend = dt.Rows.Count;
            }
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }
        #endregion
        public DataTable CreateCommonDt()
        {
            DataTable dt = new DataTable();
            //uid
            dt.Columns.Add("uid", typeof(string));
            //账户
            dt.Columns.Add("uin", typeof(string));
            //账户类型【数值】
            dt.Columns.Add("uid_type", typeof(string));
            //账户类型authen_account_type=99为白名单【文本】
            dt.Columns.Add("authen_account_type", typeof(string));
            //认证渠道【状态集】
            dt.Columns.Add("authen_channel_state", typeof(string));
            //用户名称
            dt.Columns.Add("user_true_name", typeof(string));
            //证件类型
            dt.Columns.Add("cre_type", typeof(string));
            //证件类型线上的文本
            dt.Columns.Add("cre_type_txt", typeof(string));
            //证件号
            dt.Columns.Add("cre_id", typeof(string));

            ////认证结果
            dt.Columns.Add("gov_auth_result", typeof(string));
            //gov_authen_info公安部认证渠道详细信息【公安部认证时间】
            dt.Columns.Add("gov_auth_fail_reason_dt", typeof(string));
            //ocr_authen_info影印件认证详细信息【影印件认证时间】
            dt.Columns.Add("ocr_authen_info_dt", typeof(string));

            //绑卡信息
            dt.Columns.Add("bind_bank_info", typeof(string));
            ////【银行卡卡号】四位卡尾号
            //dt.Columns.Add("card_tail", typeof(string));
            ////【银行名称】
            //dt.Columns.Add("bank_name", typeof(string));
            ////【银行预留手机】手机号
            //dt.Columns.Add("mobile", typeof(string));
            ////银行卡认证时间
            //dt.Columns.Add("authen_time", typeof(string));



            return dt;
        }

        /// <summary>
        ///  name_mask=xx&cre_id_mask=xx&cre_type=xx&authen_time=xx
        /// </summary>
        public void DealStr(string source, ref string key)
        {
            source = source.Replace("%25", "%").Replace("%26", "&").Replace("%3D", "=").Replace("%3d", "=");
            string[] strlist1 = source.Split('&');
            Hashtable ht = null;
            ht = new Hashtable(strlist1.Length);

            foreach (string strtmp in strlist1)
            {
                string[] strlist2 = strtmp.Split('=');
                if (strlist2.Length != 2)
                {
                    continue;
                }
                ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
            }
            if (ht.ContainsKey(key))
            {
                key = ht[key].ToString();
            }
            else
            {
                key = string.Empty;
            }
        }

        public string GetAuthResult(string s)
        {
            string ret = string.Empty;
            switch (s)
            {
                case "1":
                    ret = "证件号码合法";
                    break;
                case "2":
                    ret = "库中无此号";
                    break;
                case "3":
                    ret = "证件信息不一致";
                    break;
                case "4":
                    ret = "重号不一致";
                    break;
                case "5":
                    ret = "证件号码不规则";
                    break;
            }

            return string.Format("公安部实名失败({0})", ret);
        }

        public string GetAccountType(string s)
        {
            string ret = string.Empty;
            switch (s)
            {
                case "1":
                    ret = "I类";
                    break;
                case "2":
                    ret = "II类";
                    break;
                case "3":
                    ret = "III类";
                    break;
                case "99":
                    ret = " 白名单类";
                    break;
                default:
                    ret = "未认证";
                    break;
            }
            return ret;
        }

        public string GetCreTypeText(string s)
        {
            string ret = string.Empty;
            switch (s)
            {
                case "0":
                    ret = "未定义";
                    break;
                case "1":
                    ret = "身份证";
                    break;
                case "2":
                    ret = "护照";
                    break;
                case "3":
                    ret = "军官证";
                    break;
                case "4":
                    ret = "士兵证";
                    break;
                case "5":
                    ret = "回乡证";
                    break;
                case "6":
                    ret = "临时身份证";
                    break;
                case "7":
                    ret = "户口簿";
                    break;
                case "8":
                    ret = "警官证";
                    break;
                case "9":
                    ret = "台胞证";
                    break;
                case "10":
                    ret = "营业执照";
                    break;
                case "11":
                    ret = "其它证件";
                    break;
            }
            return ret;
        }
        public string FormatStrEnscript(string dealstr, string key_name)
        {
            string key = System.Configuration.ConfigurationManager.AppSettings[key_name].ToString();
            dealstr += key;
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(dealstr, "md5").ToLower();
        }


        public string FormatReqParams(Dictionary<string, string> paramsHt, string formatStr, string key_name)
        {
            StringBuilder req = new StringBuilder();
            if (paramsHt == null || paramsHt.Count == 0)
            {
                return string.Empty;
            }
            string key = System.Configuration.ConfigurationManager.AppSettings[key_name].ToString();
            string[] formatArr = formatStr.Split('|');
            StringBuilder reqFomat = new StringBuilder();
            foreach (string a in formatArr)
            {
                if (paramsHt.Keys.Contains(a))
                {
                    reqFomat.Append(paramsHt[a]);
                }
                else
                {
                    if (a == "key")
                    {
                        reqFomat.Append(key);
                    }
                    else
                    {
                        reqFomat.Append("");
                    }
                }
                reqFomat.Append("|");
            }
            if (reqFomat.ToString().EndsWith("|"))
            {
                reqFomat = new StringBuilder(reqFomat.ToString().Substring(0, reqFomat.ToString().Length - 1));
            }

            string md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(reqFomat.ToString(), "md5").ToLower();

            return md5;

        }




    }
}
