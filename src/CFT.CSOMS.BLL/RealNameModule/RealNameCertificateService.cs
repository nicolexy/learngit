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

namespace CFT.CSOMS.BLL.RealNameModule
{
    public class RealNameCertificateService
    {
        public bool AuMaintainWhiteListC(string src, string Operator, string uin, long uid, int op_type, int valid_days,string sign, out string msg)
        {       
            DataSet ds = new RealNameCertificateData().AuMaintainWhiteListC(src, Operator, uin, uid, op_type, valid_days,sign);
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


        public DataSet BindQryBindMaskInfoC(string src, string Operator, long uid, string sign)
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
                            string temp = "result="+dr["result"].ToString() + "&" + dr["bind_bank_info"];
                            DataTable dt=CommQuery.ParseRelayStringToDataTableNew(temp, "bind_bank_num", "row");
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
            DataSet ds = new RealNameCertificateData().AuQryAuthStatusServiceByQueryType0(query_type,sp_id,cre_id,cre_type,truename,uin);           
            return ds;
        }

        public DataSet AuQryAuthStatusServiceByQueryType0(int query_type, string sp_id, string cre_id, string cre_type, string truename, string uin, string src, string Operator, string sign, int request_detail_info)
        {          
            DataSet ds = new RealNameCertificateData().AuQryAuthStatusServiceByQueryType0(query_type,sp_id,cre_id,cre_type,truename,uin,src,Operator,sign,request_detail_info);
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

        public DataSet AuQryAuthStatusServiceByQueryType1(int query_type, string uin,long uid)
        {
            DataSet ds = new RealNameCertificateData().AuQryAuthStatusServiceByQueryType1(query_type, uin, uid);

            return ds;
        }
        public DataSet AuQryAuthStatusServiceByQueryType1(int query_type, string uin, long uid, int request_detail_info, string src, string Operator, string sign)
        {
            DataSet ds = new RealNameCertificateData().AuQryAuthStatusServiceByQueryType1(query_type,uin,uid,request_detail_info,src,Operator,sign);
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

        public DataSet PQueryUserInfoService(string uin, long uid, string query_attach, int curtype)
        {
            DataSet ds = new RealNameCertificateData().PQueryUserInfoService(uin, uid, query_attach,curtype);
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
                                    dr["cre_id"] = CommUtil.BankIDEncode_ForBankCardUnbind(dr["cre_id"].ToString()).Replace("\0","");
                                   
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
            if (!string.IsNullOrEmpty(cre_id)) { cre_id=CommUtil.EncryptZerosPadding(cre_id); }
            DataSet ds = new RealNameCertificateData().PQueryCreRelationC(cre_id,cre_type);       

            return ds;
        }

        public DataSet QuotaBanQueryC(int uid_type, long uid, int have_cre_photocopy, string cre_id, int cre_type)
        {
            if (!string.IsNullOrEmpty(cre_id)) { cre_id = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(cre_id.ToString(), "md5").ToLower(); }
            DataSet ds = new RealNameCertificateData().QuotaBanQueryC(uid_type, uid, have_cre_photocopy, cre_id, cre_type);

            return ds;
        }

        public DataSet QuotaBanQueryC(int uid_type, long uid, int have_cre_photocpy)
        {
            DataSet ds = new RealNameCertificateData().QuotaBanQueryC(uid_type,uid,have_cre_photocpy);

            return ds;
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
                reqFomat = new StringBuilder(reqFomat.ToString().Substring(0,reqFomat.ToString().Length - 1));
            }

            string md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(reqFomat.ToString(), "md5").ToLower();

            return md5;

        }

       


    }
}
