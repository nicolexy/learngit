using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using CFT.CSOMS.Service.CSAPI.Utility;
using System.IO;
using CFT.CSOMS.Service.CSAPI.Language;
using System.Collections;
using CFT.CSOMS.Service.CSAPI.BaseInfo;
using System.Collections.Specialized;
using System.Data;
using CFT.CSOMS.BLL.TransferMeaning;
using CFT.CSOMS.BLL.RealNameModule;
using System.Threading;
using CFT.Apollo.Logging;
using CFT.CSOMS.BLL.FreezeModule;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
namespace CFT.CSOMS.Service.CSAPI
{
    [WebService(Namespace = "")]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class RealNameCertificationService : System.Web.Services.WebService
    {
        private static readonly string RequestSource = "kf.cf.com";

        #region 对指定账户添加或者删除白名单
        [WebMethod]
        public void AuMaintainWhiteListC()
        {
            try
            {            
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "operator", "uin", "uid", "op_type", "appid", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);
                //初始化src
                if (!paramsHt.ContainsKey("src")) { paramsHt.Add("src", RequestSource); }
                string sign = string.Empty;
                string src = paramsHt["src"].ToString();
                string Operator = paramsHt["operator"].ToString();
                string uin = paramsHt["uin"].ToString();
                Int64 uid = 0;
                int op_type = 2;
                int valid_days = 1;
                string outMsg = "";
                if (paramsHt.Keys.Contains("uid"))
                {
                    uid = Int64.Parse(paramsHt["uid"].ToString());
                }
                if (paramsHt.Keys.Contains("op_type"))
                {
                    op_type = int.Parse(paramsHt["op_type"].ToString());
                }
                if (op_type == 1)
                {
                    if (paramsHt.Keys.Contains("valid_days"))
                    {
                        valid_days = int.Parse(paramsHt["valid_days"].ToString());
                        //加密签名
                        sign = new RealNameCertificateService().FormatReqParams(paramsHt, "uin|uid|operator|src|op_type|valid_days|key", "AuMaintainWhiteListCKey");
                    }
                    else
                    {
                        throw new ServiceException(APIUtil.ERR_PARAM, ErroMessage.MESSAGE_NULLPARAM);
                    }
                }
                else
                {
                    if (paramsHt.Keys.Contains("valid_days"))
                    {
                        valid_days = int.Parse(paramsHt["valid_days"].ToString());
                    }
                    //加密签名
                    sign = new RealNameCertificateService().FormatReqParams(paramsHt, "uin|uid|operator|src|op_type||key", "AuMaintainWhiteListCKey");
                }
                bool state = new RealNameCertificateService().AuMaintainWhiteListC(src, Operator, uin, uid, op_type, valid_days, sign, out outMsg);

                RecordNew record = new RecordNew();
                record.RetValue = state.ToString().ToLower();
                record.RetMemo = outMsg;
                List<RecordNew> list = new List<RecordNew>();
                list.Add(record);
                APIUtil.Print<RecordNew>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("AuMaintainWhiteListC").ErrorFormat("return_code{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("AuMaintainWhiteListC").ErrorFormat("return_code{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }
        #endregion

        #region 银行认证渠道信息查询接口
        [WebMethod]
        public void BindQryBindMaskInfoC()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "operator", "uid","appid","token");
                //token验证
                APIUtil.ValidateToken(paramsHt);
                //初始化src
                if (!paramsHt.ContainsKey("src")) { paramsHt.Add("src", RequestSource); }
                //加密签名
                string sign = new RealNameCertificateService().FormatReqParams(paramsHt, "uid|operator|src|key", "BindQryBindMaskInfoCKey");
                string src = paramsHt["src"].ToString();
                string Operator = paramsHt["operator"].ToString();
                Int64 uid = 0;
                string outMsg = "";
                if (paramsHt.Keys.Contains("uid"))
                {
                    uid = Int64.Parse(paramsHt["uid"].ToString());
                }
                DataSet ds = null;
                ds = new RealNameCertificateService().BindQryBindMaskInfoC(src, Operator, uid, sign);
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                if (ds.Tables.Count == 1)
                {
                    List<RealNameCertification.BandMaskInfo> list = APIUtil.ConvertTo<RealNameCertification.BandMaskInfo>(ds.Tables[0]);
                    APIUtil.Print<RealNameCertification.BandMaskInfo>(list);
                }
                else
                {
                    List<RealNameCertification.BandMaskInfo> list = APIUtil.ConvertTo<RealNameCertification.BandMaskInfo>(ds.Tables[1]);
                    APIUtil.Print<RealNameCertification.BandMaskInfo>(list);
                }
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("BindQryBindMaskInfoC").ErrorFormat("return_code{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("BindQryBindMaskInfoC").ErrorFormat("return_code{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }


        }
        #endregion

        #region  认证渠道信息查询接口
        [WebMethod]
        public void AuQryAuthStatusService()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "query_type","uin","appid","token");
                //token验证
                APIUtil.ValidateToken(paramsHt);
                //初始化src
                if (!paramsHt.ContainsKey("src")) { paramsHt.Add("src", RequestSource); }
                int query_type = int.Parse(paramsHt["query_type"].ToString());
                string sp_id = paramsHt.ContainsKey("sp_id") ? paramsHt["sp_id"] : string.Empty;
                string cre_id = paramsHt.ContainsKey("cre_id") ? paramsHt["cre_id"] : string.Empty;
                string cre_type = paramsHt.ContainsKey("cre_type") ? paramsHt["cre_type"] : string.Empty;
                string truename = paramsHt.ContainsKey("truename") ? paramsHt["truename"] : string.Empty;
                string src = paramsHt.ContainsKey("src") ? paramsHt["src"] : string.Empty;
                string Operator = paramsHt.ContainsKey("operator") ? paramsHt["operator"] : string.Empty;
                string uin = paramsHt.ContainsKey("uin") ? paramsHt["uin"] : string.Empty;
                Int64 uid = paramsHt.ContainsKey("uid") ? APIUtil.StringToInt(paramsHt["uid"]) : -1;
                int request_detail_info = paramsHt.ContainsKey("request_detail_info") ? APIUtil.StringToInt(paramsHt["request_detail_info"]) : 0;
                DataSet ds = null;
                switch (query_type)
                {
                    case 0:
                        {
                            //query_type sp_id cre_id cre_type truename uin
                            if (request_detail_info == 0)
                            {
                                ds = new RealNameCertificateService().AuQryAuthStatusServiceByQueryType0(query_type, sp_id, cre_id, cre_type, truename, uin);
                            }
                            else
                            {
                                //src operator sign
                                string sign = new RealNameCertificateService().FormatReqParams(paramsHt, "uin|uid|operator|src|key", "AuQryAuthStatusServiceKey");
                                ds = new RealNameCertificateService().AuQryAuthStatusServiceByQueryType0(query_type, sp_id, cre_id, cre_type, truename, uin, src, Operator, sign, request_detail_info);
                            }
                        }
                        break;
                    case 1:
                        {
                            if (!string.IsNullOrEmpty(uin))
                            {
                                if (uid == -1) throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                            }
                            //query_type uin uid
                            if (request_detail_info == 0)
                            {
                                ds = new RealNameCertificateService().AuQryAuthStatusServiceByQueryType1(query_type, uin, uid);
                            }
                            else
                            {
                                //src operator sign
                                string sign = new RealNameCertificateService().FormatReqParams(paramsHt, "uin|uid|operator|src|key", "AuQryAuthStatusServiceKey");
                                ds = new RealNameCertificateService().AuQryAuthStatusServiceByQueryType1(query_type, uin, uid, request_detail_info, src, Operator, sign);
                            }
                        }
                        break;
                }

                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                else
                {
                    if (request_detail_info == 0)
                    {
                        List<RealNameCertification.AuthStatusInfo> list = APIUtil.ConvertTo<RealNameCertification.AuthStatusInfo>(ds.Tables[0]);
                        APIUtil.Print<RealNameCertification.AuthStatusInfo>(list);
                    }
                    else
                    {
                        List<RealNameCertification.AuthStatusInfoDetail> list = APIUtil.ConvertTo<RealNameCertification.AuthStatusInfoDetail>(ds.Tables[0]);
                        APIUtil.Print<RealNameCertification.AuthStatusInfoDetail>(list);
                    }
                }
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("AuQryAuthStatusService").ErrorFormat("return_code{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("AuQryAuthStatusService").ErrorFormat("return_code{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }
        #endregion

        #region 账户信息查询接口
        [WebMethod]
        public void PQueryUserInfoService()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "curtype", "query_attach","appid","token");
                //token验证
                APIUtil.ValidateToken(paramsHt);
                if (!paramsHt.ContainsKey("uin") && !paramsHt.ContainsKey("uid"))
                {
                    throw new ServiceException(APIUtil.ERR_PARAM, ErroMessage.MESSAGE_NULLPARAM); 
                }
                string uin = paramsHt.ContainsKey("uin") ? paramsHt["uin"] : string.Empty;
                Int64 uid = paramsHt.ContainsKey("uid") ? APIUtil.StringToInt(paramsHt["uid"]) : -1;
                int curtype = paramsHt.ContainsKey("curtype") ? APIUtil.StringToInt(paramsHt["curtype"]) : -1;
                string query_attach = paramsHt.ContainsKey("query_attach") ? paramsHt["query_attach"] : string.Empty;
                DataSet ds = null;

                ds = new RealNameCertificateService().PQueryUserInfoService(uin, uid, query_attach, curtype);
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                else
                {
                    if (!string.IsNullOrEmpty(uin))
                    {
                        if (!string.IsNullOrEmpty(query_attach))
                        {
                            if (query_attach.Trim().ToUpper() == "QUERY_USERINFO")
                            {
                                List<RealNameCertification.UserInfoByUinAndQueryUser> list = APIUtil.ConvertTo<RealNameCertification.UserInfoByUinAndQueryUser>(ds.Tables[0]);
                                APIUtil.Print<RealNameCertification.UserInfoByUinAndQueryUser>(list);
                            }
                            if (query_attach.Trim().ToUpper() == "QUERY_USERATT")
                            {
                                List<RealNameCertification.UserInfoByUinAndQueryUserAtt> list = APIUtil.ConvertTo<RealNameCertification.UserInfoByUinAndQueryUserAtt>(ds.Tables[0]);
                                APIUtil.Print<RealNameCertification.UserInfoByUinAndQueryUserAtt>(list);
                            }
                            if (query_attach.Trim().ToUpper() == "QUERY_USERINFO|QUERY_USERATT")
                            {
                                List<RealNameCertification.UserInfoByUinAndUserAttach> list = APIUtil.ConvertTo<RealNameCertification.UserInfoByUinAndUserAttach>(ds.Tables[0]);
                                APIUtil.Print<RealNameCertification.UserInfoByUinAndUserAttach>(list);
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(query_attach))
                        {
                            if (query_attach.Trim().ToUpper() == "QUERY_USERINFO")
                            {
                                List<RealNameCertification.UserInfoByUidAndQueryUser> list = APIUtil.ConvertTo<RealNameCertification.UserInfoByUidAndQueryUser>(ds.Tables[0]);
                                APIUtil.Print<RealNameCertification.UserInfoByUidAndQueryUser>(list);
                            }
                            if (query_attach.Trim().ToUpper() == "QUERY_USERATT")
                            {
                                List<RealNameCertification.UserInfoByUidAndQueryUserAtt> list = APIUtil.ConvertTo<RealNameCertification.UserInfoByUidAndQueryUserAtt>(ds.Tables[0]);
                                APIUtil.Print<RealNameCertification.UserInfoByUidAndQueryUserAtt>(list);
                            }
                            if (query_attach.Trim().ToUpper() == "QUERY_USERINFO|QUERY_USERATT")
                            {
                                List<RealNameCertification.UserInfoByUidAndUserAttach> list = APIUtil.ConvertTo<RealNameCertification.UserInfoByUidAndUserAttach>(ds.Tables[0]);
                                APIUtil.Print<RealNameCertification.UserInfoByUidAndUserAttach>(list);
                            }
                        }
                    }

                }
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("PQueryUserInfoService").ErrorFormat("return_code{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("PQueryUserInfoService").ErrorFormat("return_code{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }
        #endregion


        #region 证件下账户列表查询
        [WebMethod]
        public void PQueryCreRelationC()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "cre_type", "cre_id","appid","token");
                //token验证
                APIUtil.ValidateToken(paramsHt);
                string cre_id = paramsHt["cre_id"];
                int cre_type = APIUtil.StringToInt(paramsHt["cre_type"]);
                DataSet ds = null;
                ds = new RealNameCertificateService().PQueryCreRelationC(cre_id, cre_type);
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                else
                {
                    List<RealNameCertification.PQueryCreRelation> list = APIUtil.ConvertTo<RealNameCertification.PQueryCreRelation>(ds.Tables[0]);
                    APIUtil.Print<RealNameCertification.PQueryCreRelation>(list);
                }
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("PQueryCreRelationC").ErrorFormat("return_code{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("PQueryCreRelationC").ErrorFormat("return_code{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }
        #endregion

        #region 账户限额数据查询
        [WebMethod]
        public void QuotaBanQueryC()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "uid_type", "uid", "have_cre_photocopy","appid","token");
                //token验证
                APIUtil.ValidateToken(paramsHt);
                int uid_type = paramsHt.ContainsKey("uid_type") ? APIUtil.StringToInt(paramsHt["uid_type"]) : 0;
                Int64 uid = APIUtil.StringToInt(paramsHt["uid"]);
                int have_cre_photocopy = paramsHt.ContainsKey("have_cre_photocopy") ? APIUtil.StringToInt(paramsHt["have_cre_photocopy"]) : 0;
                DataSet ds = null;
                if (uid_type == 1 | uid_type == 2 || uid_type == 3)
                {
                    APIUtil.ValidateParamsNew(paramsHt, "cre_type", "cre_id");
                    int cre_type = APIUtil.StringToInt(paramsHt["cre_type"]);
                    string cre_id = paramsHt["cre_id"];
                    ds = new RealNameCertificateService().QuotaBanQueryC(uid_type, uid, have_cre_photocopy, cre_id, cre_type);
                }
                else
                {
                    ds = new RealNameCertificateService().QuotaBanQueryC(uid_type, uid, have_cre_photocopy);
                } 
              
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                else
                {
                    List<RealNameCertification.QuotaBanQueryC> list = APIUtil.ConvertTo<RealNameCertification.QuotaBanQueryC>(ds.Tables[0]);
                    APIUtil.Print<RealNameCertification.QuotaBanQueryC>(list);
                }
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("QuotaBanQueryC").ErrorFormat("return_code{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("QuotaBanQueryC").ErrorFormat("return_code{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }
        #endregion
    }
}
