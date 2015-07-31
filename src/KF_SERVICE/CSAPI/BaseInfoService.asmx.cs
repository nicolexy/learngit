﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
namespace CFT.CSOMS.Service.CSAPI
{
    /// <summary>
    /// Summary description for WechatService
    /// </summary>
    [WebService(Namespace = "")]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class BaseInfoService : System.Web.Services.WebService
    {
        #region 自助申诉

        [WebMethod]
        public void GetUserAuthenState()
        {
            try
            {

                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();

                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uin", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                string user_bank_id = "";
                int bank_type = 0;
                if (paramsHt.Keys.Contains("user_bank_id"))
                {
                    user_bank_id = paramsHt["user_bank_id"].ToString();
                }
                if (paramsHt.Keys.Contains("bank_type"))
                {
                    bank_type = APIUtil.StringToInt(paramsHt["bank_type"].ToString());
                }

                var infos = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().GetUserAuthenState(paramsHt["uin"].ToString(), user_bank_id, bank_type);

                Record record = new Record();
                record.RetValue = infos.ToString().ToLower();
                List<Record> list = new List<Record>();
                list.Add(record);

                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetUserAuthenState").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetUserAuthenState").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void GetAppealUserInfo()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();

                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uin", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                var infos = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().GetAppealUserInfo(paramsHt["uin"].ToString());

                if (infos == null || infos.Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                List<BaseInfoC.UserInfoBasic> list = APIUtil.ConvertTo<BaseInfoC.UserInfoBasic>(infos);

                // var ret = new ResultParse<BaseInfoC.UserInfoBasic>().ReturnToObject(list);
                APIUtil.Print<BaseInfoC.UserInfoBasic>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetUserAuthenState").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetUserAuthenState").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void GetUserAppealList()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();

                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uin", "start_date", "end_date", "state", "type", "dotype", "offset", "limit", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                //检查查询是否为int
                int state = APIUtil.StringToInt(paramsHt["state"].ToString());
                int type = APIUtil.StringToInt(paramsHt["type"].ToString());
                int offset = APIUtil.StringToInt(paramsHt["offset"].ToString());
                int limit = APIUtil.StringToInt(paramsHt["limit"].ToString());

                if (offset < 0)
                {
                    offset = 0;
                }

                if (limit > 20 || limit <= 0)
                {
                    limit = 20;
                }
                //校验时间格式
                APIUtil.ValidateDate(paramsHt["start_date"].ToString(), "yyyy-MM-dd HH:mm:ss", false);
                APIUtil.ValidateDate(paramsHt["end_date"].ToString(), "yyyy-MM-dd HH:mm:ss", false);

                var infos = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().GetUserAppealList(paramsHt["uin"].ToString(), paramsHt["start_date"].ToString(), paramsHt["end_date"].ToString(),
                    state, type, paramsHt["qqtype"].ToString(), paramsHt["dotype"].ToString(), offset, limit, 99);//不排序

                if (infos == null || infos.Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                List<BaseInfoC.UserAppealList> list = APIUtil.ConvertTo<BaseInfoC.UserAppealList>(infos);
                APIUtil.Print<BaseInfoC.UserAppealList>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetUserAppealList").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetUserAppealList").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void GetUserAppealDetail()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();

                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "fid", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                string db = "", tb = "";

                if (paramsHt.Keys.Contains("db"))
                {
                    db = paramsHt["db"].ToString();
                }
                if (paramsHt.Keys.Contains("tb"))
                {
                    tb = paramsHt["tb"].ToString();
                }


                var infos = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().GetUserAppealDetail(paramsHt["fid"].ToString(), db, tb);

                if (infos == null || infos.Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                List<BaseInfoC.UserAppealDetail> list = APIUtil.ConvertTo<BaseInfoC.UserAppealDetail>(infos);

                APIUtil.Print<BaseInfoC.UserAppealDetail>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetUserAppealList").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetUserAppealList").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void DelAppeal()
        {
            try
            {
                //获取请求参数
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();

                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "fid", "user", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);


                string comment = "", user_ip = "", db = "", tb = "";
                if (paramsHt.Keys.Contains("comment"))
                {
                    comment = paramsHt["comment"].ToString();
                }
                if (paramsHt.Keys.Contains("user_ip"))
                {
                    user_ip = paramsHt["user_ip"].ToString();
                }
                if (paramsHt.Keys.Contains("db"))
                {
                    db = paramsHt["db"].ToString();
                }
                if (paramsHt.Keys.Contains("tb"))
                {
                    tb = paramsHt["tb"].ToString();
                }

                var infos = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().DelAppeal(paramsHt["fid"].ToString(), comment, paramsHt["user"].ToString(), user_ip, db, tb);
                Record record = new Record();
                record.RetValue = infos.ToString();
                List<Record> list = new List<Record>();
                list.Add(record);

                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("DelAppeal").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("DelAppeal").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void CannelAppeal()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();

                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "fid", "user", "reason", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                string comment = "", other_reason = "", user_ip = "", db = "", tb = "";
                if (paramsHt.Keys.Contains("comment"))
                {
                    comment = paramsHt["comment"].ToString();
                }
                if (paramsHt.Keys.Contains("other_reason"))
                {
                    other_reason = paramsHt["other_reason"].ToString();
                }
                if (paramsHt.Keys.Contains("user_ip"))
                {
                    user_ip = paramsHt["user_ip"].ToString();
                }
                if (paramsHt.Keys.Contains("db"))
                {
                    db = paramsHt["db"].ToString();
                }
                if (paramsHt.Keys.Contains("tb"))
                {
                    tb = paramsHt["tb"].ToString();
                }

                var infos = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().CannelAppeal(paramsHt["fid"].ToString(), paramsHt["reason"].ToString(), other_reason, comment, paramsHt["user"].ToString(), user_ip, db, tb);
                Record record = new Record();
                record.RetValue = infos.ToString();
                List<Record> list = new List<Record>();
                list.Add(record);

                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("CannelAppeal").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("CannelAppeal").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void ConfirmAppeal()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();

                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "fid", "user", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);


                string comment = "", user_ip = "", db = "", tb = "";
                if (paramsHt.Keys.Contains("comment"))
                {
                    comment = paramsHt["comment"].ToString();
                }
                if (paramsHt.Keys.Contains("user_ip"))
                {
                    user_ip = paramsHt["user_ip"].ToString();
                }
                if (paramsHt.Keys.Contains("db"))
                {
                    db = paramsHt["db"].ToString();
                }
                if (paramsHt.Keys.Contains("tb"))
                {
                    tb = paramsHt["tb"].ToString();
                }

                var infos = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().ConfirmAppeal(paramsHt["fid"].ToString(), comment, paramsHt["user"].ToString(), user_ip, db, tb);
                Record record = new Record();
                record.RetValue = infos.ToString();
                List<Record> list = new List<Record>();
                list.Add(record);

                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("ConfirmAppeal").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("ConfirmAppeal").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        #endregion

        #region 特殊申诉处理

        // 特殊申诉处理-通过
        [WebMethod]
        public void ComfirmAppealSpecial()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "fid", "user", "handle_result", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string fid = paramsHt.ContainsKey("fid") ? paramsHt["fid"].ToString() : "";
                string user = paramsHt.ContainsKey("user") ? paramsHt["user"].ToString() : "";
                string user_desc = paramsHt.ContainsKey("user_desc") ? paramsHt["user_desc"].ToString() : "";
                string user_ip = paramsHt.ContainsKey("user_ip") ? paramsHt["user_ip"].ToString() : "";
                string handle_result = paramsHt.ContainsKey("handle_result") ? paramsHt["handle_result"].ToString() : "";
                string appeal_db = paramsHt.ContainsKey("db") ? paramsHt["db"].ToString() : "";
                string appeal_tb = paramsHt.ContainsKey("tb") ? paramsHt["tb"].ToString() : "";
                string comment = paramsHt.ContainsKey("comment") ? paramsHt["comment"].ToString() : ""; //备注
 
                var infos = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().ConfirmAppealSpecial(fid, handle_result,comment, user_desc, user, user_ip, appeal_db, appeal_tb);

                Record record = new Record();
                record.RetValue = infos.ToString().ToLower();
                List<Record> list = new List<Record>();
                list.Add(record);
                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("ComfirmAppealSpecial").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("ComfirmAppealSpecial").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        //特殊申诉处理-拒绝
        [WebMethod]
        public void CannelAppealSpecial()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "fid", "user", "handle_result", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string fid = paramsHt.ContainsKey("fid") ? paramsHt["fid"].ToString() : "";
                string user = paramsHt.ContainsKey("user") ? paramsHt["user"].ToString() : "";
                string user_desc = paramsHt.ContainsKey("user_desc") ? paramsHt["user_desc"].ToString() : "";
                string user_ip = paramsHt.ContainsKey("user_ip") ? paramsHt["user_ip"].ToString() : "";
                string handle_result = paramsHt.ContainsKey("handle_result") ? paramsHt["handle_result"].ToString() : "";
                string appeal_db = paramsHt.ContainsKey("db") ? paramsHt["db"].ToString() : "";
                string appeal_tb = paramsHt.ContainsKey("tb") ? paramsHt["tb"].ToString() : "";
                string comment = paramsHt.ContainsKey("comment") ? paramsHt["comment"].ToString() : ""; //备注
        
                var infos = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().CannelAppealSpecial(fid, handle_result,comment, user_desc, user, user_ip, appeal_db, appeal_tb);

                Record record = new Record();
                record.RetValue = infos.ToString().ToLower();
                List<Record> list = new List<Record>();
                list.Add(record);
                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("CannelAppealSpecial").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("CannelAppealSpecial").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        //特殊申诉处理-删除
        [WebMethod]
        public void DelAppealSpecial()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "fid", "user", "handle_result", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string fid = paramsHt.ContainsKey("fid") ? paramsHt["fid"].ToString() : "";
                string user = paramsHt.ContainsKey("user") ? paramsHt["user"].ToString() : "";
                string user_desc = paramsHt.ContainsKey("user_desc") ? paramsHt["user_desc"].ToString() : "";
                string user_ip = paramsHt.ContainsKey("user_ip") ? paramsHt["user_ip"].ToString() : "";
                string handle_result = paramsHt.ContainsKey("handle_result") ? paramsHt["handle_result"].ToString() : "";
                string appeal_db = paramsHt.ContainsKey("db") ? paramsHt["db"].ToString() : "";
                string appeal_tb = paramsHt.ContainsKey("tb") ? paramsHt["tb"].ToString() : "";
                string comment = paramsHt.ContainsKey("comment") ? paramsHt["comment"].ToString() : ""; //备注
        
                var infos = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().DelAppealSpecial(fid, handle_result,comment, user_desc, user, user_ip, appeal_db, appeal_tb);

                Record record = new Record();
                record.RetValue = infos.ToString().ToLower();
                List<Record> list = new List<Record>();
                list.Add(record);
                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("DelAppealSpecial").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("DelAppealSpecial").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        //特殊申诉处理-获取日志
        [WebMethod]
        public void GetFreezeDiary()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "fid", "type", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string fid = paramsHt.ContainsKey("fid") ? paramsHt["fid"].ToString() : "";
                int type = APIUtil.StringToInt(paramsHt["type"].ToString());    //申诉类型

                var infos = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().GetFreezeDiary(fid, type);

                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<BaseInfoC.AppealLog> list = APIUtil.ConvertTo<BaseInfoC.AppealLog>(infos.Tables[0]);
                APIUtil.Print<BaseInfoC.AppealLog>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetFreezeDiary").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetFreezeDiary").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        //特殊申诉列表查询
        [WebMethod]
        public void GetSpecialAppealList()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uin", "begin_date", "end_date", "appeal_type", "order_state", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string uin = paramsHt.ContainsKey("uin") ? paramsHt["uin"].ToString() : "";
                string begin_date = paramsHt.ContainsKey("begin_date") ? paramsHt["begin_date"].ToString() : "";
                string end_date = paramsHt.ContainsKey("end_date") ? paramsHt["end_date"].ToString() : "";
                int appeal_type = APIUtil.StringToInt(paramsHt["appeal_type"].ToString());  //申诉类型
                int order_state = APIUtil.StringToInt(paramsHt["order_state"].ToString());  //订单状态
                int offset = APIUtil.StringToInt(paramsHt["offset"].ToString());
                int limit = APIUtil.StringToInt(paramsHt["limit"].ToString());

                if (offset < 0)
                {
                    offset = 0;
                }
                if (limit <= 0)
                {
                    limit = 20;
                }

                string order_type = paramsHt.ContainsKey("order_type") ? paramsHt["order_type"].ToString() : "1";   //排序方式
                string fid = paramsHt.ContainsKey("fid") ? paramsHt["fid"].ToString() : "";
                string handler = paramsHt.ContainsKey("handler") ? paramsHt["handler"].ToString() : "";
                string freeze_reason = paramsHt.ContainsKey("freeze_reason") ? paramsHt["freeze_reason"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().GetSpecialAppealList(uin, begin_date, end_date, appeal_type, order_state,
                    fid, handler, freeze_reason, offset, limit, order_type);

                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count < 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<BaseInfoC.SpecialAppealList> list = APIUtil.ConvertTo<BaseInfoC.SpecialAppealList>(infos.Tables[0]);
                APIUtil.Print<BaseInfoC.SpecialAppealList>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetSpecialAppealList").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetSpecialAppealList").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        //特殊申诉详情查询
        [WebMethod]
        public void GetSpecialAppealDetail()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "fid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string fid = paramsHt.ContainsKey("fid") ? paramsHt["fid"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().GetSpecialAppealDetail(fid);

                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<BaseInfoC.SpecialAppealDetail> list = APIUtil.ConvertTo<BaseInfoC.SpecialAppealDetail>(infos.Tables[0]);
                APIUtil.Print<BaseInfoC.SpecialAppealDetail>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetSpecialAppealDetail").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetSpecialAppealDetail").ErrorFormat("return_code:{0},mag:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        //补充资料
        [WebMethod]
        public void AddAppealMaterial()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "fid", "user", "handle_result", "uin", "submit_time", "type", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string fid = paramsHt.ContainsKey("fid") ? paramsHt["fid"].ToString() : "";
                string user = paramsHt.ContainsKey("user") ? paramsHt["user"].ToString() : "";
                string handle_result = paramsHt.ContainsKey("handle_result") ? paramsHt["handle_result"].ToString() : "";
                string uin = paramsHt.ContainsKey("uin") ? paramsHt["uin"].ToString() : "";
                string submit_time = paramsHt.ContainsKey("submit_time") ? paramsHt["submit_time"].ToString() : "";
                int type = APIUtil.StringToInt(paramsHt["type"].ToString());

                string creid_img = paramsHt.ContainsKey("creid_img") ? paramsHt["creid_img"].ToString() : "";
                string creid2_img = paramsHt.ContainsKey("creid2_img") ? paramsHt["creid2_img"].ToString() : "";
                string bank_img = paramsHt.ContainsKey("bank_img") ? paramsHt["bank_img"].ToString() : "";
                string fund_img1 = paramsHt.ContainsKey("fund_img1") ? paramsHt["fund_img1"].ToString() : "";
                string fund_img2 = paramsHt.ContainsKey("fund_img2") ? paramsHt["fund_img2"].ToString() : "";
                string fund_img3 = paramsHt.ContainsKey("fund_img3") ? paramsHt["fund_img3"].ToString() : "";
                string fund_img4 = paramsHt.ContainsKey("fund_img4") ? paramsHt["fund_img4"].ToString() : "";
                string fund_img5 = paramsHt.ContainsKey("fund_img5") ? paramsHt["fund_img5"].ToString() : "";
                string fund_img6 = paramsHt.ContainsKey("fund_img6") ? paramsHt["fund_img6"].ToString() : "";
                string fund_img7 = paramsHt.ContainsKey("fund_img7") ? paramsHt["fund_img7"].ToString() : "";

                string phone_no = paramsHt.ContainsKey("phone_no") ? paramsHt["phone_no"].ToString() : "";
                string user_desc = paramsHt.ContainsKey("user_desc") ? paramsHt["user_desc"].ToString() : "";
                string zdy_title1 = paramsHt.ContainsKey("zdy_title1") ? paramsHt["zdy_title1"].ToString() : "";
                string zdy_title2 = paramsHt.ContainsKey("zdy_title2") ? paramsHt["zdy_title2"].ToString() : "";
                string zdy_title3 = paramsHt.ContainsKey("zdy_title3") ? paramsHt["zdy_title3"].ToString() : "";
                string zdy_title4 = paramsHt.ContainsKey("zdy_title4") ? paramsHt["zdy_title4"].ToString() : "";
                string zdy_info1 = paramsHt.ContainsKey("zdy_info1") ? paramsHt["zdy_info1"].ToString() : "";
                string zdy_info2 = paramsHt.ContainsKey("zdy_info2") ? paramsHt["zdy_info2"].ToString() : "";
                string zdy_info3 = paramsHt.ContainsKey("zdy_info3") ? paramsHt["zdy_info3"].ToString() : "";
                string zdy_info4 = paramsHt.ContainsKey("zdy_info4") ? paramsHt["zdy_info4"].ToString() : "";

                int bt = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().GetIntBT(creid_img, creid2_img, bank_img, fund_img1, fund_img2, fund_img3, fund_img4,
                    fund_img5, fund_img6, fund_img7, user_desc);

                var infos = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().CreateFreezeDiary(fid, type, user, handle_result, "", uin, phone_no, submit_time, bt, user_desc,
                    zdy_title1, zdy_title2, zdy_title3, zdy_title4, zdy_info1, zdy_info2, zdy_info3, zdy_info4); //普通/微信解冻handleType=2；特殊找回支付密码handleType=11

                Record record = new Record();
                record.RetValue = infos.ToString().ToLower();
                List<Record> list = new List<Record>();
                list.Add(record);
                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("AddAppealMaterial").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("AddAppealMaterial").ErrorFormat("return_code:{0},mag:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        //挂起
        [WebMethod]
        public void HangAppeal()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "fid", "user", "handle_result", "uin", "submit_time", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string fid = paramsHt.ContainsKey("fid") ? paramsHt["fid"].ToString() : "";
                string user = paramsHt.ContainsKey("user") ? paramsHt["user"].ToString() : "";
                string handle_result = paramsHt.ContainsKey("handle_result") ? paramsHt["handle_result"].ToString() : "";
                string uin = paramsHt.ContainsKey("uin") ? paramsHt["uin"].ToString() : "";
                string submit_time = paramsHt.ContainsKey("submit_time") ? paramsHt["submit_time"].ToString() : "";

                string phone_no = paramsHt.ContainsKey("phone_no") ? paramsHt["phone_no"].ToString() : "";
                string user_desc = paramsHt.ContainsKey("user_desc") ? paramsHt["user_desc"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().CreateFreezeDiary(fid, 8, user, handle_result, "", uin, phone_no,
                    submit_time, 0, user_desc, "", "", "", "", "", "", "", ""); //挂起handleType=8

                Record record = new Record();
                record.RetValue = infos.ToString().ToLower();
                List<Record> list = new List<Record>();
                list.Add(record);
                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("HangAppeal").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("HangAppeal").ErrorFormat("return_code:{0},mag:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        //作废
        [WebMethod]
        public void DeleteAppeal()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "fid", "user", "handle_result", "uin", "submit_time", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string fid = paramsHt.ContainsKey("fid") ? paramsHt["fid"].ToString() : "";
                string user = paramsHt.ContainsKey("user") ? paramsHt["user"].ToString() : "";
                string handle_result = paramsHt.ContainsKey("handle_result") ? paramsHt["handle_result"].ToString() : "";
                string uin = paramsHt.ContainsKey("uin") ? paramsHt["uin"].ToString() : "";
                string submit_time = paramsHt.ContainsKey("submit_time") ? paramsHt["submit_time"].ToString() : "";

                string phone_no = paramsHt.ContainsKey("phone_no") ? paramsHt["phone_no"].ToString() : "";
                string user_desc = paramsHt.ContainsKey("user_desc") ? paramsHt["user_desc"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().CreateFreezeDiary(fid, 7, user, handle_result, "", uin, phone_no,
                    submit_time, 0, user_desc, "", "", "", "", "", "", "", ""); //作废handleType=7

                Record record = new Record();
                record.RetValue = infos.ToString().ToLower();
                List<Record> list = new List<Record>();
                list.Add(record);
                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("DeleteAppeal").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("DeleteAppeal").ErrorFormat("return_code:{0},mag:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        //结单（已解决）
        [WebMethod]
        public void CompleteAppeal()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "fid", "user", "handle_result", "uin", "submit_time", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string fid = paramsHt.ContainsKey("fid") ? paramsHt["fid"].ToString() : "";
                string user = paramsHt.ContainsKey("user") ? paramsHt["user"].ToString() : "";
                string handle_result = paramsHt.ContainsKey("handle_result") ? paramsHt["handle_result"].ToString() : "";
                string uin = paramsHt.ContainsKey("uin") ? paramsHt["uin"].ToString() : "";
                string submit_time = paramsHt.ContainsKey("submit_time") ? paramsHt["submit_time"].ToString() : "";

                string phone_no = paramsHt.ContainsKey("phone_no") ? paramsHt["phone_no"].ToString() : "";
                string user_desc = paramsHt.ContainsKey("user_desc") ? paramsHt["user_desc"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().CreateFreezeDiary(fid, 1, user, handle_result, "", uin, phone_no,
                    submit_time, 0, user_desc, "", "", "", "", "", "", "", ""); //结单handleType=1

                Record record = new Record();
                record.RetValue = infos.ToString().ToLower();
                List<Record> list = new List<Record>();
                list.Add(record);
                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("CompleteAppeal").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("CompleteAppeal").ErrorFormat("return_code:{0},mag:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        //同步身份证号
        [WebMethod]
        public void SyncCreid()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uin", "oldcreid", "newcreid", "user", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string uin = paramsHt.ContainsKey("uin") ? paramsHt["uin"].ToString() : "";
                string oldcreid = paramsHt.ContainsKey("oldcreid") ? paramsHt["oldcreid"].ToString() : "";//注册证件号码
                string newcreid = paramsHt.ContainsKey("newcreid") ? paramsHt["newcreid"].ToString() : "";//用户提交证件号码
                //int cretype = APIUtil.StringToInt(paramsHt["cretype"].ToString());
                int cretype = 1;
                string user = paramsHt.ContainsKey("user") ? paramsHt["user"].ToString() : "";
                string ip = paramsHt.ContainsKey("ip") ? paramsHt["ip"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.FreezeModule.FreezeService().SyncCreid(uin, oldcreid, newcreid, cretype, user, ip);

                Record record = new Record();
                record.RetValue = infos.ToString().ToLower();
                List<Record> list = new List<Record>();
                list.Add(record);
                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("SyncCreid").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("SyncCreid").ErrorFormat("return_code:{0},mag:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        #endregion

        #region 证件号码清理

        //证件信息查询
        [WebMethod]
        public void QueryCreidList()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "creid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                String creid = paramsHt.ContainsKey("creid") ? paramsHt["creid"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AccountService().GetClearCreidLog(creid);
                if (infos == null || infos.Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<BaseInfoC.CreidInfoBasic> list = APIUtil.ConvertTo<BaseInfoC.CreidInfoBasic>(infos);
                APIUtil.Print<BaseInfoC.CreidInfoBasic>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("QueryCreidList").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("QueryCreidList").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        //证件号码清理
        [WebMethod]
        public void ClearCreid()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "creid", "type", "opera", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                String creid = paramsHt.ContainsKey("creid") ? paramsHt["creid"].ToString() : "";
                String opera = paramsHt.ContainsKey("opera") ? paramsHt["opera"].ToString() : "";
                int u_type = APIUtil.StringToInt(paramsHt["type"]); //type=0普通用户，type=1微信用户

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AccountService().ClearCreidInfo(creid, u_type, opera);

                Record record = new Record();
                record.RetValue = infos.ToString().ToLower();
                List<Record> list = new List<Record>();
                list.Add(record);
                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("ClearCreid").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("ClearCreid").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }
        #endregion

        #region 受控资金查询

        /// <summary>
        /// 查询用户受控资金
        /// </summary>
        [WebMethod]
        public void QueryUserCtrlFund()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "qqid", "opera", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";
                string opera = paramsHt.ContainsKey("opera") ? paramsHt["opera"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AccountService().QueryUserCtrlFund(qqid, opera);

                if (infos == null || infos.Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<BaseInfoC.UserControledFund> list = APIUtil.ConvertTo<BaseInfoC.UserControledFund>(infos);
                APIUtil.Print<BaseInfoC.UserControledFund>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("QueryUserCtrlFund").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("QueryUserCtrlFund").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 解绑用户单条受控资金
        /// </summary>
        [WebMethod]
        public void UnbindSingleCtrlFund()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "qqid", "opera", "balance", "cur_type", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";
                string opera = paramsHt.ContainsKey("opera") ? paramsHt["opera"].ToString() : "";
                string balance = paramsHt.ContainsKey("balance") ? paramsHt["balance"].ToString() : "";
                string cur_type = paramsHt.ContainsKey("cur_type") ? paramsHt["cur_type"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AccountService().UnbindSingleCtrlFund(qqid, opera, cur_type, balance);

                Record record = new Record();
                record.RetValue = infos.ToString().ToLower();
                List<Record> list = new List<Record>();
                list.Add(record);
                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("UnbindSingleCtrlFund").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("UnbindSingleCtrlFund").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 解绑当前用户全部受控资金
        /// </summary>
        [WebMethod]
        public void UnbindAllCtrlFund()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "qqid", "opera", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";
                string opera = paramsHt.ContainsKey("opera") ? paramsHt["opera"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AccountService().UnbindAllCtrlFund(qqid, opera);

                Record record = new Record();
                record.RetValue = infos.ToString().ToLower();
                List<Record> list = new List<Record>();
                list.Add(record);
                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("UnbindAllCtrlFund").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("UnbindAllCtrlFund").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 获得受控资金操作日志
        /// </summary>
        [WebMethod]
        public void GetCtrlFundLog()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "qqid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.TradeModule.TradeService().RemoveControledFinLogQuery(qqid);

                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                List<BaseInfoC.CtrlFundLog> list = APIUtil.ConvertTo<BaseInfoC.CtrlFundLog>(infos.Tables[0]);
                APIUtil.Print<BaseInfoC.CtrlFundLog>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetCtrlFundLog").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetCtrlFundLog").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        #endregion

        #region 个人账户信息

        /// <summary>
        /// 获取个人账户信息
        /// </summary>
        [WebMethod]
        public void GetPersonalInfo()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "qqid", "type", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";
                int type = APIUtil.StringToInt(paramsHt["type"].ToString());//1,C账号;2,内部账号

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AccountService().GetPersonalInfo(qqid, type);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<BaseInfoC.PersonalInfo> list = APIUtil.ConvertTo<BaseInfoC.PersonalInfo>(infos.Tables[0]);
                APIUtil.Print<BaseInfoC.PersonalInfo>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetPersonalInfo").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetPersonalInfo").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 会员信息
        /// </summary>
        [WebMethod]
        public void QueryVipInfo()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "qqid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AccountService().QueryVipInfo(qqid);
                if (infos == null || infos.Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                List<BaseInfoC.VIPInfo> list = APIUtil.ConvertTo<BaseInfoC.VIPInfo>(infos);
                APIUtil.Print<BaseInfoC.VIPInfo>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("QueryVipInfo").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("QueryVipInfo").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 删除认证的操作日志
        /// </summary>
        [WebMethod]
        public void GetDeleteCertLog()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "qqid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AccountService().GetUserClassDeleteList(qqid);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                List<BaseInfoC.DeleteCertLog> list = APIUtil.ConvertTo<BaseInfoC.DeleteCertLog>(infos.Tables[0]);
                APIUtil.Print<BaseInfoC.DeleteCertLog>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetDeleteCertLog").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetDeleteCertLog").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 实名认证状态
        /// </summary>
        [WebMethod]
        public void RealNameCert()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "qqid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";
                string msg = "";

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AccountService().GetUserClassInfo(qqid, out msg);
                if (msg.Contains("接口失败"))
                {
                    msg = "未查询到该用户的实名认证状态";
                }

                Record record = new Record();
                record.RetValue = msg;
                List<Record> list = new List<Record>();
                list.Add(record);
                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("RealNameCert").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("RealNameCert").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 查询余额支付功能关闭与否
        /// </summary>
        [WebMethod]
        public void BalancePaidOrNot()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "qqid", "ip", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";
                string ip = paramsHt.ContainsKey("ip") ? paramsHt["ip"].ToString() : "127.0.0.1";

                var infos = new CFT.CSOMS.BLL.BalanceModule.BalaceService().BalancePaidOrNotQuery(qqid, ip);

                Record record = new Record();
                record.RetValue = infos.ToString().ToLower();
                List<Record> list = new List<Record>();
                list.Add(record);
                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("BalancePaidOrNot").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("BalancePaidOrNot").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }
      
        /// <summary>
        /// 删除认证信息
        /// </summary>
        [WebMethod]
        public void DelAuthen()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "qqid", "username", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";
                string username = paramsHt.ContainsKey("username") ? paramsHt["username"].ToString() : "";
                string msg = "";

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AccountService().DelAuthen(qqid, username, out msg);

                Record record = new Record();
                record.RetValue = infos.ToString().ToLower();
                List<Record> list = new List<Record>();
                list.Add(record);
                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("DelAuthen").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("DelAuthen").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }
     
        #endregion

        #region 销户操作

        /// <summary>
        /// 销户日志
        /// </summary>
        [WebMethod]
        public void GetCancelAccountLog()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "begin_time", "end_time", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";
                string opera = paramsHt.ContainsKey("opera") ? paramsHt["opera"].ToString() : "";
                DateTime begin_time = DateTime.Parse("1970-01-01 00:00:00");
                if (paramsHt.ContainsKey("begin_time"))
                {
                    begin_time = APIUtil.StrToDate(paramsHt["begin_time"].ToString());
                }
                DateTime end_time = DateTime.Now;
                if (paramsHt.ContainsKey("end_time"))
                {
                    end_time = APIUtil.StrToDate(paramsHt["end_time"].ToString());
                }
                int offset = APIUtil.StringToInt(paramsHt["offset"].ToString());
                int limit = APIUtil.StringToInt(paramsHt["limit"].ToString());

                //限制数据过多
                int days= (end_time - begin_time).Days;
                if (days > 366)
                {
                    end_time = begin_time.AddDays(366);
                }

                if (offset < 0)
                {
                    offset = 0;
                }
                if (limit < 0 || limit > 1000)
                {
                    limit = 20;
                }

                string msg="";
                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AccountService().GetCanncelAccountLog(qqid, opera, begin_time, end_time, offset, limit, out msg);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                List<BaseInfoC.CancelAccountLog> list = APIUtil.ConvertTo<BaseInfoC.CancelAccountLog>(infos.Tables[0]);
                APIUtil.Print<BaseInfoC.CancelAccountLog>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetCancelAccountLog").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetCancelAccountLog").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 销户申请
        /// </summary>
    
        public void DeleteLogonAccount()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "begin_time", "end_time", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string query_id = paramsHt.ContainsKey("query_id") ? paramsHt["query_id"].ToString() : "";
                int query_type = paramsHt.ContainsKey("query_type") ? APIUtil.StringToInt(paramsHt["query_type"].ToString()) : 0;
                string reason = paramsHt.ContainsKey("reason") ? paramsHt["reason"].ToString() : "";
                bool is_Send = paramsHt.ContainsKey("send") ? APIUtil.StringToBool(paramsHt["send"].ToString()) : false;
                string email_Addr = paramsHt.ContainsKey("email_Addr") ? paramsHt["email_Addr"].ToString() : "";
                string opera = paramsHt.ContainsKey("opera") ? paramsHt["opera"].ToString() : "";           
                string ret_msg = "";
                bool ret_continue;

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AccountService().LogOnUserDeleteUser(query_id, query_type, reason, is_Send, email_Addr, opera, out  ret_msg,out ret_continue);

                RecordNew record = new RecordNew();
                record.RetValue = infos.ToString().ToLower();
                record.RetMemo = ret_msg;
                List<RecordNew> list = new List<RecordNew>();
                list.Add(record);
                APIUtil.Print<RecordNew>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("DeleteLogonAccount").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("DeleteLogonAccount").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        #endregion

        #region 证书通知

        [WebMethod]
        public void QueryCertNoticeBlackList()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string spid = paramsHt.ContainsKey("spid") ? paramsHt["spid"].ToString() : "";
                string begin_time = paramsHt.ContainsKey("begin_time") ? paramsHt["begin_time"].ToString() : "";
                string end_time = paramsHt.ContainsKey("end_time") ? paramsHt["end_time"].ToString() : "";

                int offset = APIUtil.StringToInt(paramsHt["offset"].ToString());
                int limit = APIUtil.StringToInt(paramsHt["limit"].ToString());


                if (offset < 0)
                {
                    offset = 0;
                }
                if (limit < 0 || limit > 1000)
                {
                    limit = 20;
                }

                string msg = "";
                var infos = new CFT.CSOMS.BLL.SPOA.MerchantService().QueryCertNoticeBlackList(spid, begin_time, end_time, offset, limit);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                List<BaseInfoC.CertNoticeBlackList> list = APIUtil.ConvertTo<BaseInfoC.CertNoticeBlackList>(infos.Tables[0]);
                APIUtil.Print<BaseInfoC.CertNoticeBlackList>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("QueryCertNoticeBlackList").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("QueryCertNoticeBlackList").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }
        #endregion
    }
}
