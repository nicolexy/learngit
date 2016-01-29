using System;
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
using CFT.CSOMS.BLL.TransferMeaning;
using CFT.CSOMS.BLL.RefundModule;
using System.Threading;
using CFT.Apollo.Logging;
using CFT.CSOMS.BLL.FreezeModule;

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
                bool stateMsg = false;
                var infos = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().GetUserAuthenState(paramsHt["uin"].ToString(), user_bank_id, bank_type, out stateMsg);

                Record record = new Record();
                record.RetValue = stateMsg.ToString().ToLower();
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

                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                if (infos != null && infos.Tables.Count > 0 && infos.Tables[0].Rows.Count > 0)
                {
                    infos.Tables[0].Columns.Add("FBalanceStr", typeof(String));//总金额
                    infos.Tables[0].Columns.Add("FconStr", typeof(String));//冻结金额
                    infos.Tables[0].Columns.Add("Fcre_type_str", typeof(String));//证件类型
                    if (infos.Tables[0].Rows.Count > 0)
                    {
                        string creid = infos.Tables[0].Rows[0]["Fcreid"].ToString();
                        infos.Tables[0].Rows[0]["Fcre_type_str"] = Transfer.GetCreType(infos.Tables[0].Rows[0]["Fcre_type"].ToString());
                    }
                    COMMLIB.CommUtil.FenToYuan_Table(infos.Tables[0], "FBalance", "FBalanceStr");
                    COMMLIB.CommUtil.FenToYuan_Table(infos.Tables[0], "Fcon", "FconStr");                    
                }

                List<BaseInfoC.UserInfoBasic> list = APIUtil.ConvertTo<BaseInfoC.UserInfoBasic>(infos.Tables[0]);
                APIUtil.Print<BaseInfoC.UserInfoBasic>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetAppealUserInfo").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetAppealUserInfo").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
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

                if (limit > 50 || limit <= 0)
                {
                    limit = 50;
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
        public void ConfirmAppealSpecial()
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
                SunLibrary.LoggerFactory.Get("ConfirmAppealSpecial").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("ConfirmAppealSpecial").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
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
                if (limit <= 0 || limit > 50)
                {
                    limit = 50;
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
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uin", "old_creid", "new_creid", "user","user_ip", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string uin = paramsHt.ContainsKey("uin") ? paramsHt["uin"].ToString() : "";
                string oldcreid = paramsHt.ContainsKey("old_creid") ? paramsHt["old_creid"].ToString() : "";//注册证件号码
                string newcreid = paramsHt.ContainsKey("new_creid") ? paramsHt["new_creid"].ToString() : "";//用户提交证件号码
                //int cretype = APIUtil.StringToInt(paramsHt["cre_type"].ToString());
                int cre_type = 1;
                string user = paramsHt.ContainsKey("user") ? paramsHt["user"].ToString() : "";
                string user_ip = paramsHt.ContainsKey("user_ip") ? paramsHt["user_ip"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.FreezeModule.FreezeService().SyncCreid(uin, oldcreid, newcreid, cre_type, user, user_ip);

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

        #region 审核数据
        [WebMethod]
        public void GetCheckStatistics()
        {
            try
            {

                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();

                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "check_type", "sdate", "edate", "offset", "limit", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                string check_type = paramsHt["check_type"];
                string op_name = paramsHt.TryGetValue("op_name", out op_name) ? op_name : "";
                var sdate = APIUtil.StrToDate(paramsHt["sdate"]);
                var edate = APIUtil.StrToDate(paramsHt["edate"]);
                var offset = APIUtil.StringToInt(paramsHt["offset"]);
                var limit = APIUtil.StringToInt(paramsHt["limit"]);

                DataSet result = null;
                if (check_type == "SpecialAppeal")
                {
                    result = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().GetStatisticsFreezeDiary(op_name, "", sdate, edate, offset, limit);
                }

                if (check_type == "SelfAppeal")
                {
                    result = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().GetKFTotalQueryList(op_name, "appeal", sdate, edate, offset, limit);
                }

                if (check_type == "SPEditAppeal")
                {
                    result = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().GetKFTotalQueryList(op_name, "domain", sdate, edate, offset, limit);
                }

                if (result == null || result.Tables.Count <= 0 || result.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                APIUtil.Print4DataTable(result.Tables[0], null, paramsHt);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetCheckStatistics").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetCheckStatistics").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
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

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.CertificateService().GetClearCreidLog(creid);
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

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.CertificateService().ClearCreidInfo(creid, u_type, opera);

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

                var infos = new CFT.CSOMS.BLL.FundModule.ControlFundService().QueryUserCtrlFund(qqid, opera);

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

                var infos = new CFT.CSOMS.BLL.FundModule.ControlFundService().UnbindSingleCtrlFund(qqid, opera, cur_type, balance);

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

                var infos = new CFT.CSOMS.BLL.FundModule.ControlFundService().UnbindAllCtrlFund(qqid, opera);

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

                var infos = new CFT.CSOMS.BLL.FundModule.ControlFundService().RemoveControledFinLogQuery(qqid);

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
        public void GetUserAccount()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "qqid", "type", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";
                //"Uin"-C账号,"Uid"-内部账号
                string type = paramsHt.ContainsKey("type") ? paramsHt["type"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AccountService().GetUserAccount(qqid, type);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<BaseInfoC.PersonalInfo> list = APIUtil.ConvertTo<BaseInfoC.PersonalInfo>(infos.Tables[0]);
                APIUtil.Print<BaseInfoC.PersonalInfo>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetUserAccount").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetUserAccount").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        ///会员信息查询下线，darrenran 2016-01-06
        /// </summary>
        //[WebMethod]
        //public void QueryVipInfo()
        //{
        //    try
        //    {
        //        Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
        //        //验证必填参数
        //        APIUtil.ValidateParamsNew(paramsHt, "appid", "qqid", "token");
        //        //验证token
        //        APIUtil.ValidateToken(paramsHt);

        //        string qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";

        //        var infos = new CFT.CSOMS.BLL.CFTAccountModule.VIPService().QueryVipInfo(qqid);
        //        if (infos == null || infos.Rows.Count <= 0)
        //        {
        //            throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
        //        }

        //        List<BaseInfoC.VIPInfo> list = APIUtil.ConvertTo<BaseInfoC.VIPInfo>(infos);
        //        APIUtil.Print<BaseInfoC.VIPInfo>(list);
        //    }
        //    catch (ServiceException se)
        //    {
        //        SunLibrary.LoggerFactory.Get("QueryVipInfo").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
        //        APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
        //    }
        //    catch (Exception ex)
        //    {
        //        SunLibrary.LoggerFactory.Get("QueryVipInfo").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
        //        APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
        //    }
        //}

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

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AuthenInfoService().GetUserClassDeleteList(qqid);
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

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AuthenInfoService().GetUserClassInfo(qqid, out msg);
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

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AuthenInfoService().DelAuthen(qqid, username, out msg);

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

        #region /*个人信息*/
       
        /// <summary>
        /// 获取个人信息
        /// </summary>
        [WebMethod]
        public void GetUserInfo()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "qqid", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";
                int offset = paramsHt.ContainsKey("offset") ? APIUtil.StringToInt(paramsHt["offset"].ToString()) : 0;
                int limit = paramsHt.ContainsKey("limit") ? APIUtil.StringToInt(paramsHt["limit"].ToString()) : 10;

                if (offset < 0)
                    offset = 0;
                if (limit < 0 || limit > 50)
                    limit = 50;

                string userType = "", Msg = "", userType_str = "";
                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AccountService().GetUserInfo(qqid, offset, limit);
                bool type = new CFT.CSOMS.BLL.CFTAccountModule.AccountService().GetUserType(qqid, out userType,out userType_str, out Msg);
                if (type && infos != null && infos.Tables.Count > 0 && infos.Tables[0].Rows.Count > 0)
                {
                    infos.Tables[0].Columns.Add("userType", typeof(String));
                    infos.Tables[0].Columns.Add("userType_str", typeof(String));
                    foreach (DataRow dr in infos.Tables[0].Rows)
                    {
                        dr["userType"] = userType;
                        dr["userType_str"] = userType_str;
                    }
                }
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<BaseInfoC.GetUserInfo> list = APIUtil.ConvertTo<BaseInfoC.GetUserInfo>(infos.Tables[0]);
                APIUtil.Print<BaseInfoC.GetUserInfo>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetUserInfo").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetUserInfo").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void QueryChangeUserInfoLog()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "qqid", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";
                int offset = paramsHt.ContainsKey("offset") ? APIUtil.StringToInt(paramsHt["offset"].ToString()) : 0;
                int limit = paramsHt.ContainsKey("limit") ? APIUtil.StringToInt(paramsHt["limit"].ToString()) : 10;

                if (offset < 0)
                    offset = 0;
                if (limit < 0 || limit > 50)
                    limit = 50;

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AccountService().QueryChangeUserInfoLog(qqid, offset, limit);
                if (infos == null || infos.Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<BaseInfoC.UserInfoLog> list = APIUtil.ConvertTo<BaseInfoC.UserInfoLog>(infos);
                APIUtil.Print<BaseInfoC.UserInfoLog>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("QueryChangeUserInfoLog").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("QueryChangeUserInfoLog").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }
     
        #endregion

        #region QQ账号修改申请
        [WebMethod]
        public void ChangeQQApply()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "old_qqid", "new_qqid", "reason", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string old_qqid = paramsHt.ContainsKey("old_qqid") ? paramsHt["old_qqid"].ToString() : "";
                string new_qqid = paramsHt.ContainsKey("new_qqid") ? paramsHt["new_qqid"].ToString() : "";
                string reason = paramsHt.ContainsKey("reason") ? paramsHt["reason"].ToString() : "";
                string opera = paramsHt.ContainsKey("opera") ? paramsHt["opera"].ToString() : "";
                string ip = paramsHt.ContainsKey("ip") ? paramsHt["ip"].ToString() : "";

                string outMsg = "";
                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AccountOperate().ChangeQQApply(old_qqid, new_qqid, reason, opera, ip, out outMsg);

                RecordNew record = new RecordNew();
                record.RetValue = infos.ToString().ToLower();
                record.RetMemo = outMsg;
                List<RecordNew> list = new List<RecordNew>();
                list.Add(record);
                APIUtil.Print<RecordNew>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("ChangeQQApply").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("ChangeQQApply").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 查询QQ是否可以修改
        /// </summary>
        [WebMethod]
        public void ChangeQQState()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "old_qqid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string old_qqid = paramsHt["old_qqid"].ToString();
                string outMsg = "";

                bool state = new CFT.CSOMS.BLL.CFTAccountModule.AccountOperate().ChangeQQState(old_qqid, out outMsg);

                RecordNew record = new RecordNew();
                record.RetValue = state.ToString().ToLower();
                record.RetMemo = outMsg;
                List<RecordNew> list = new List<RecordNew>();
                list.Add(record);
                APIUtil.Print<RecordNew>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("ChangeQQState").ErrorFormat("return_code{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("ChangeQQState").ErrorFormat("return_code{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        //修改QQ的日志记录
        [WebMethod]
        public void ChangeQQHistoryLog()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";
                string opera = paramsHt.ContainsKey("opera") ? paramsHt["opera"].ToString() : "";
                int offset = paramsHt.ContainsKey("offset") ? APIUtil.StringToInt(paramsHt["offset"].ToString()) : 0;
                int limit = paramsHt.ContainsKey("limit") ? APIUtil.StringToInt(paramsHt["limit"].ToString()) : 10;
                if (offset < 0)
                    offset = 0;
                if (limit < 0 || limit > 50)
                    limit = 50;

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AccountOperate().GetChangeQQList(opera, qqid, offset, limit);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<BaseInfoC.ChangeQQList> list = APIUtil.ConvertTo<BaseInfoC.ChangeQQList>(infos.Tables[0]);
                APIUtil.Print<BaseInfoC.ChangeQQList>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("ChangeQQHistoryLog").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("ChangeQQHistoryLog").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
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
                    end_time = end_time.AddDays(1);
                }
                int offset = APIUtil.StringToInt(paramsHt["offset"].ToString());
                int limit = APIUtil.StringToInt(paramsHt["limit"].ToString());

                if (offset < 0)
                {
                    offset = 0;
                }
                if (limit < 0 || limit > 50)
                {
                    limit = 50;
                }

                string msg="";
                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AccountOperate().GetCanncelAccountLog(qqid, opera, begin_time, end_time, offset, limit, out msg);
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
        [WebMethod]
        public void DeleteLogonAccount()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "query_id", "query_type", "reason", "send", "opera", "ip", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string query_id = paramsHt.ContainsKey("query_id") ? paramsHt["query_id"].ToString() : "";
                int query_type = paramsHt.ContainsKey("query_type") ? APIUtil.StringToInt(paramsHt["query_type"].ToString()) : 0;
                string reason = paramsHt.ContainsKey("reason") ? paramsHt["reason"].ToString() : "";
                bool is_Send = paramsHt.ContainsKey("send") ? APIUtil.StringToBool(paramsHt["send"].ToString()) : false;
                string email_Addr = paramsHt.ContainsKey("email_Addr") ? paramsHt["email_Addr"].ToString() : "";
                string opera = paramsHt.ContainsKey("opera") ? paramsHt["opera"].ToString() : "";
                string ip = paramsHt.ContainsKey("ip") ? paramsHt["ip"].ToString() : "";
                string ret_msg = "";

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AccountOperate().LogOnUserDeleteUser(query_id, query_type, reason, is_Send, email_Addr, opera, ip, out  ret_msg);

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

        /// <summary>
        /// 获取销户信息
        /// </summary>
        [WebMethod]
        public void GetCloseAccountInfo()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "query_id", "query_type", "reason", "send", "opera", "ip", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string query_id = paramsHt.ContainsKey("query_id") ? paramsHt["query_id"].ToString() : "";
                int query_type = paramsHt.ContainsKey("query_type") ? APIUtil.StringToInt(paramsHt["query_type"].ToString()) : 0;
                string reason = paramsHt.ContainsKey("reason") ? paramsHt["reason"].ToString() : "";
                bool is_Send = paramsHt.ContainsKey("send") ? APIUtil.StringToBool(paramsHt["send"].ToString()) : false;
                string email_Addr = paramsHt.ContainsKey("email_Addr") ? paramsHt["email_Addr"].ToString() : "";
                string opera = paramsHt.ContainsKey("opera") ? paramsHt["opera"].ToString() : "";
                string ip = paramsHt.ContainsKey("ip") ? paramsHt["ip"].ToString() : "";
                string ret_msg = "";

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AccountOperate().CloseingAccountInfo(query_id, query_type, reason, is_Send, email_Addr, opera, ip, out  ret_msg);

                RecordNew record = new RecordNew();
                record.RetValue = infos.ToString().ToLower();
                record.RetMemo = ret_msg;
                List<RecordNew> list = new List<RecordNew>();
                list.Add(record);
                APIUtil.Print<RecordNew>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetCloseAccountInfo").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetCloseAccountInfo").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
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
                if (limit < 0 || limit > 50)
                {
                    limit = 50;
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

        #region 手机绑定

        [WebMethod]
        public void MobileBindingInfo()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "qqid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                String qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.MobileModule.MobileService().GetMsgNotify(qqid);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<BaseInfoC.MobileBindInfo> list = APIUtil.ConvertTo<BaseInfoC.MobileBindInfo>(infos.Tables[0]);
                APIUtil.Print<BaseInfoC.MobileBindInfo>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("MobileBindingInfo").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("MobileBindingInfo").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void UpdateMobileBind()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "qqid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                String qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.MobileModule.MobileService().UpDateBindInfo(qqid);
                Record record = new Record();
                record.RetValue = infos.ToString().ToLower();
                List<Record> list = new List<Record>();
                list.Add(record);
                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("UpdateMobileBind").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("UpdateMobileBind").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void UnbindMobile()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                String uid = paramsHt.ContainsKey("uid") ? paramsHt["uid"].ToString() : "";
                string Msg = "";
                var infos = new CFT.CSOMS.BLL.MobileModule.MobileService().UnbindMsgNotify(uid, out Msg);
                RecordNew record = new RecordNew();
                record.RetValue = infos.ToString().ToLower();
                record.RetMemo = Msg;
                List<RecordNew> list = new List<RecordNew>();
                list.Add(record);
                APIUtil.Print<RecordNew>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("UnbindMobile").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("UnbindMobile").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        #endregion

        #region 冻结类

        //[WebMethod]
        //public void GetCashOutFreezeListId()
        //{
        //    try
        //    {
        //        Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
        //        //验证必填参数
        //        APIUtil.ValidateParamsNew(paramsHt, "appid", "uid", "token");
        //        //验证token
        //        APIUtil.ValidateToken(paramsHt);

        //        string uid = paramsHt.ContainsKey("uid") ? paramsHt["uid"].ToString() : "";

        //        var infos = new CFT.CSOMS.BLL.FreezeModule.FreezeService().GetCashOutFreezeListId(uid);

        //        Record record = new Record();
        //        record.RetValue = infos.ToString();
        //        List<Record> list = new List<Record>();
        //        list.Add(record);
        //        APIUtil.Print<Record>(list);
        //    }
        //    catch (ServiceException se)
        //    {
        //        SunLibrary.LoggerFactory.Get("GetCashOutFreezeListId").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
        //        APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
        //    }
        //    catch (Exception ex)
        //    {
        //        SunLibrary.LoggerFactory.Get("GetCashOutFreezeListId").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
        //        APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
        //    }
        //}

        /// <summary>
        /// 用户冻结记录查询
        /// </summary>
        [WebMethod]
        public void QueryUserFreezeRecord()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uin", "begin_time", "end_time", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string uin = paramsHt.ContainsKey("uin") ? paramsHt["uin"].ToString() : "";
                double balance = paramsHt.ContainsKey("balance") ? APIUtil.StringToInt(paramsHt["balance"].ToString()) : 0;
                DateTime begin_time = paramsHt.ContainsKey("begin_time") ? APIUtil.StrToDate(paramsHt["begin_time"]) : DateTime.Now;
                DateTime end_time = paramsHt.ContainsKey("end_time") ? APIUtil.StrToDate(paramsHt["end_time"]) : DateTime.Now;
                string begin_time_str = begin_time.ToString("yyyy-MM-dd");
                string end_time_str = end_time.AddDays(1).ToString("yyyy-MM-dd");
                string list_id = paramsHt.ContainsKey("list_id") ? paramsHt["list_id"].ToString() : "";
                int offset = paramsHt.ContainsKey("offset") ? APIUtil.StringToInt(paramsHt["offset"].ToString()) : 0;
                int limit = paramsHt.ContainsKey("limit") ? APIUtil.StringToInt(paramsHt["limit"].ToString()) : 1;

                if (offset < 0)
                    offset = 0;
                if (limit < 0 || limit > 50)
                    limit = 50;

                DataSet ds = new DataSet();
                if (string.IsNullOrEmpty(list_id))
                {
                    //查询列表
                    ds = new CFT.CSOMS.BLL.FreezeModule.FreezeService().QueryUserFreezeRecord(begin_time_str, end_time_str, uin, balance, "", offset, limit);
                }
                else
                {
                    //查询详情
                    ds = new CFT.CSOMS.BLL.FreezeModule.FreezeService().QueryUserFreezeRecord("", "", uin, 0, list_id, 0, 1);
                }
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                List<FreezeInfo.UserFreezeRecord> list = APIUtil.ConvertTo<FreezeInfo.UserFreezeRecord>(ds.Tables[0]);
                APIUtil.Print<FreezeInfo.UserFreezeRecord>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("QueryUserFreezeRecord").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("QueryUserFreezeRecord").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 冻结查询函数
        /// </summary>
        [WebMethod]
        public void GetFreezeList()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "id", "begin_time", "end_time", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string id = paramsHt.ContainsKey("id") ? paramsHt["id"].ToString() : "";
                DateTime begin_time = paramsHt.ContainsKey("begin_time") ? APIUtil.StrToDate(paramsHt["begin_time"]) : DateTime.Now;
                DateTime end_time = paramsHt.ContainsKey("end_time") ? APIUtil.StrToDate(paramsHt["end_time"]) : DateTime.Now;
                end_time = end_time.AddDays(1);
                string freezeuser = paramsHt.ContainsKey("freezeuser") ? paramsHt["freezeuser"].ToString() : "";
                string username = paramsHt.ContainsKey("username") ? paramsHt["username"].ToString() : "";
                int statetype = paramsHt.ContainsKey("statetype") ? APIUtil.StringToInt(paramsHt["statetype"].ToString()) : 0;
                int handletype = paramsHt.ContainsKey("handletype") ? APIUtil.StringToInt(paramsHt["handletype"].ToString()) : 0;
                int offset = paramsHt.ContainsKey("offset") ? APIUtil.StringToInt(paramsHt["offset"].ToString()) : 0;
                int limit = paramsHt.ContainsKey("limit") ? APIUtil.StringToInt(paramsHt["limit"].ToString()) : 1;

                if (offset < 0)
                    offset = 0;
                if (limit < 0 || limit > 50)
                    limit = 50;

                var infos = new CFT.CSOMS.BLL.FreezeModule.FreezeService().GetFreezeList(begin_time, end_time, freezeuser, username, handletype, statetype, id, offset, limit);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<FreezeInfo.FreezeList> list = APIUtil.ConvertTo<FreezeInfo.FreezeList>(infos.Tables[0]);
                APIUtil.Print<FreezeInfo.FreezeList>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetFreezeList").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetFreezeList").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 冻结查询详细函数
        /// </summary>
        [WebMethod]
        public void GetFreezeListDetail()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "id", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string id = paramsHt.ContainsKey("id") ? paramsHt["id"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.FreezeModule.FreezeService().GetFreezeListDetail(id);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<FreezeInfo.FreezeListDetail> list = APIUtil.ConvertTo<FreezeInfo.FreezeListDetail>(infos.Tables[0]);
                APIUtil.Print<FreezeInfo.FreezeListDetail>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetFreezeListDetail").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetFreezeListDetail").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 解冻
        /// </summary>
        [WebMethod]
        public void UnFreezeAccount()
        {
            List<BaseInfoC.FreezeThaw> list = new List<BaseInfoC.FreezeThaw>();
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "account", "channel", "executor", "executorip", "username", "contact", "reason", "appid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string account = paramsHt.ContainsKey("account") ? paramsHt["account"].ToString() : "";
                string channel = paramsHt.ContainsKey("channel") ? paramsHt["channel"].ToString() : "";
                string executor = paramsHt.ContainsKey("executor") ? paramsHt["executor"].ToString() : "";
                string executorip = paramsHt.ContainsKey("executorip") ? paramsHt["executorip"].ToString() : "";
                string userName = paramsHt.ContainsKey("username") ? paramsHt["username"].ToString() : "";
                string contact = paramsHt.ContainsKey("contact") ? paramsHt["contact"].ToString() : "";
                string reason = paramsHt.ContainsKey("reason") ? paramsHt["reason"].ToString() : "";
                string appid = paramsHt.ContainsKey("appid") ? paramsHt["appid"].ToString() : "";
                string token = paramsHt.ContainsKey("token") ? paramsHt["token"].ToString() : "";

                SunLibrary.LogHelper.LogError(string.Format("UnFreezeAccount 参数：[{0}，{1}，{2}，{3}，{4}，{5}，{6}]",
                    account, channel, executor, executorip, userName, contact, reason));

                bool exeSign = false;
                TENCENT.OSS.CFT.KF.KF_Service.Finance_Manage fm = new TENCENT.OSS.CFT.KF.KF_Service.Finance_Manage();
                TENCENT.OSS.CFT.KF.KF_Service.Finance_Header fhq = new TENCENT.OSS.CFT.KF.KF_Service.Finance_Header();
                fhq.UserIP = executorip;
                fhq.UserName = executor;
                fm.myHeader = fhq;

                string val;
                string des;
                bool iswechat = false;
                if (account.Contains("@wx.tenpay.com"))
                {
                    iswechat = true;
                }

                string freezeChannel = string.Empty;
                string tuserName = string.Empty;
                string fid = string.Empty;
                //FFreezeType: 1为冻结帐户，2为锁定工单

                //读取出原来提交的用户姓名和联系方式和帐户号码。
                TENCENT.OSS.CFT.KF.KF_Service.Query_Service fmquery = new TENCENT.OSS.CFT.KF.KF_Service.Query_Service();
                fmquery.myHeader = fhq;

                TENCENT.OSS.CFT.KF.KF_Service.FreezeInfo fi = fmquery.GetExistFreeze(account, 1);
                if (fi != null)
                { //如果没有冻结记录的，也能解冻
                    //ddlFreezeChannel.Text = (fi.FFreezeChannel == "" || fi.FFreezeChannel == null) ? "<无解冻渠道>" : fi.FFreezeChannel;
                    if (fi.FFreezeChannel != null && fi.FFreezeChannel != "" && fi.FFreezeChannel != "0")
                    {
                        freezeChannel = fi.FFreezeChannel;
                    }
                    fid = fi.fid;
                    tuserName = (string.IsNullOrEmpty(fi.username)) ? userName : fi.username;
                }

                if (channel != "" && channel != "0")
                {
                    //如果为空,不需要进行权限判断;不为空,则需要进行权限判断.
                    if (channel == "1" || channel == "6")
                    {
                        //风控渠道
                        val = "UnFreezeChannelFK";
                        des = "风控冻结";
                    }
                    else if (channel == "2")
                    {
                        //拍拍
                        val = "UnFreezeChannelPP";
                        des = "拍拍冻结";
                    }
                    else if (channel == "3")
                    {
                        //用户
                        val = "UnFreezeChannelYH";
                        des = "用户冻结";
                    }
                    else if (channel == "4")
                    {
                        //商户
                        val = "UnFreezeChannelSH";
                        des = "商户冻结";
                    }
                    else if (channel == "5")
                    {
                        //BG
                        val = "UnFreezeChannelBG";
                        des = "BG接口冻结";
                    }
                }
                else
                {
                    val = "UnFreezeChannelFK";
                    des = "风控冻结";
                }
                TENCENT.OSS.CFT.KF.KF_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Service.Query_Service();
                qs.myHeader = fhq;


                //先判断余额，如果超过，发起审批，原来的处理放在审批完成环节。
                long UNFreeze_BigMoney = long.Parse(System.Configuration.ConfigurationManager.AppSettings["UNFreeze_BigMoney"]);
                string Msg = "";
                long userbalance = 0;
                if (!iswechat)
                {
                    userbalance = qs.GetUserBalance(account, 1, out Msg);
                }

                if (userbalance >= UNFreeze_BigMoney)
                {
                    //发起审批后结束。
                    TENCENT.OSS.CFT.KF.KF_Service.Check_Service cs = new TENCENT.OSS.CFT.KF.KF_Service.Check_Service();
                    TENCENT.OSS.CFT.KF.KF_Service.Finance_Header fhc = fhq;
                    cs.myHeader = fhc;

                    TENCENT.OSS.C2C.Finance.BankLib.Param[] myparam = new TENCENT.OSS.C2C.Finance.BankLib.Param[8];

                    myparam[0] = new TENCENT.OSS.C2C.Finance.BankLib.Param();
                    myparam[0].ParamName = "uid";
                    myparam[0].ParamValue = account;

                    myparam[1] = new TENCENT.OSS.C2C.Finance.BankLib.Param();
                    myparam[1].ParamName = "mediflag";
                    myparam[1].ParamValue = "false";

                    myparam[2] = new TENCENT.OSS.C2C.Finance.BankLib.Param();
                    myparam[2].ParamName = "username";
                    myparam[2].ParamValue = fhc.UserName;

                    myparam[3] = new TENCENT.OSS.C2C.Finance.BankLib.Param();
                    myparam[3].ParamName = "userip";
                    myparam[3].ParamValue = fhc.UserIP;

                    myparam[4] = new TENCENT.OSS.C2C.Finance.BankLib.Param();
                    myparam[4].ParamName = "type";
                    myparam[4].ParamValue = "2";

                    myparam[5] = new TENCENT.OSS.C2C.Finance.BankLib.Param();
                    myparam[5].ParamName = "handleresult";
                    myparam[5].ParamValue = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.replaceSqlStr(reason);


                    myparam[6] = new TENCENT.OSS.C2C.Finance.BankLib.Param();
                    myparam[6].ParamName = "fid";
                    myparam[6].ParamValue = fid;

                    string returnUrl = "/BaseAccount/FreezeDetail.aspx?fid=" + fid.ToString();
                    myparam[7] = new TENCENT.OSS.C2C.Finance.BankLib.Param();
                    myparam[7].ParamName = "returnUrl";
                    myparam[7].ParamValue = returnUrl;

                    string fmemo = "解冻大金额用户：" + account + "，金额为：" + TENCENT.OSS.CFT.KF.Common.MoneyTransfer.FenToYuan(userbalance);

                    string mainId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    cs.StartCheck(mainId, "UNFreezeCheck", fmemo, TENCENT.OSS.CFT.KF.Common.MoneyTransfer.FenToYuan(userbalance.ToString()), myparam);

                    BaseInfoC.FreezeThaw freeze = new BaseInfoC.FreezeThaw();
                    freeze.flag = 99;
                    freeze.info = "解冻账户余额较大，发起审批成功！";
                    list.Add(freeze);

                    APIUtil.Print<BaseInfoC.FreezeThaw>(list);
                    return;
                }
                string uname = tuserName;

                bool refreeze = false;
                if (iswechat)
                {
                    //微信处理流程
                    refreeze = fm.UnFreezePerAccountWechat_New(account, uname);
                }
                else
                {
                    //解冻 2 ui_unfreeze_user_service
                    refreeze = fm.freezePerAccount(account, 2, uname, "");
                }

                if (refreeze)
                {
                    //解冻成功，发送微信消息
                    //发微信解冻消息
                    if (account.IndexOf("@wx.tenpay.com") > 0)
                    {
                        string reqsource = "bus_kf_unfreeze";//客服解冻
                        string accid = account.Substring(0, account.IndexOf("@wx.tenpay.com"));
                        string templateid = "DeNkYEfSBW7mVQET6QHwnilGWvG8cLssLSyRH0CSDk0";
                        string cont1 = "你的微信支付账户已排除了安全风险并由保护模式切换至正常模式。";
                        string cont2 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        string cont3 = "请点击详情查看微信支付安全保障介绍";
                        string msgtype = "unfreeze";
                        try
                        {
                            new CFT.CSOMS.BLL.FreezeModule.FreezeService().SendWechatMsg(reqsource, accid, templateid, cont1, cont2, cont3, msgtype);
                        }
                        catch (Exception ef)
                        {
                            LogHelper.LogError("发微信解冻消息[new FreezeService().SendWechatMsg]异常：" + ef.ToString());
                        }
                    }
                    BaseInfoC.FreezeThaw freeze = new BaseInfoC.FreezeThaw();
                    freeze.flag = 0;
                    freeze.info = "解冻成功。";
                    list.Add(freeze);
                }
                else
                {
                    //失败
                    BaseInfoC.FreezeThaw freeze = new BaseInfoC.FreezeThaw();
                    freeze.flag = 1;
                    freeze.info = "解冻失败。";
                    list.Add(freeze);
                }


                if (!string.IsNullOrEmpty(fid))  //如果不是数据异常,为空说明是QQ简化注册的,不用走这
                {
                    try
                    {
                        TENCENT.OSS.CFT.KF.KF_Service.FreezeInfo finfo = new TENCENT.OSS.CFT.KF.KF_Service.FreezeInfo();
                        fi.fid = fid;
                        fi.FHandleResult = TENCENT.OSS.CFT.KF.KF_Service.PublicRes.replaceMStr(reason);
                        fi.FFreezeType = 1;
                        qs.UpdateFreezeInfo(fi);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.LogError("ENCENT.OSS.CFT.KF.KF_Web.BaseAccount.freezeBankAcc" + " " + "protected void BT_F_Or_Not_Click(object sender, System.EventArgs e),处理冻结工单时失败:" + ex);
                    }
                }
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("UnFreezeAccount").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                //APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
                BaseInfoC.FreezeThaw freeze = new BaseInfoC.FreezeThaw();
                freeze.flag = -1;
                freeze.info = se.Message;
                list.Add(freeze);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("UnFreezeAccount").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                //APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS + "；" + ex.Message);
                BaseInfoC.FreezeThaw freeze = new BaseInfoC.FreezeThaw();
                freeze.flag = -1;
                freeze.info = ex.Message;
                list.Add(freeze);
            }
            APIUtil.Print<BaseInfoC.FreezeThaw>(list);
        }


        #endregion

        #region 个人证书管理

        /// <summary>
        /// 查询个人证书信息列表
        /// </summary>
        [WebMethod]
        public void GetUserCrtList()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "qqid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.CRTModule.CRTService().GetUserCrtList(qqid);

                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<BaseInfoC.UserCrtList> list = APIUtil.ConvertTo<BaseInfoC.UserCrtList>(infos.Tables[0]);
                APIUtil.Print<BaseInfoC.UserCrtList>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetUserCrtList").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetUserCrtList").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 查询关闭证书服务信息
        /// </summary>
        [WebMethod]
        public void GetDeleteQueryInfo()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "qqid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.CRTModule.CRTService().GetDeleteQueryInfo(qqid);

                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<BaseInfoC.DeleteCrtInfo> list = APIUtil.ConvertTo<BaseInfoC.DeleteCrtInfo>(infos.Tables[0]);
                APIUtil.Print<BaseInfoC.DeleteCrtInfo>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetDeleteQueryInfo").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetDeleteQueryInfo").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        #endregion

        #region 微信实名认证

        /// <summary>
        /// 微信实名认证查询
        /// </summary>
        [WebMethod]
        public void QueryWechatRealNameAuthen()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uin","serialNo","ip", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string uin = paramsHt.ContainsKey("uin") ? paramsHt["uin"].ToString() : "";
                string serialNo = paramsHt.ContainsKey("serialNo") ? paramsHt["serialNo"].ToString() : "";
                string ip = paramsHt.ContainsKey("ip") ? paramsHt["ip"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.WechatPay.AuthenService().QueryWechatRealNameAuthen(uin, serialNo, ip);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                Record record = new Record();
                record.RetValue = infos.Tables[0].Rows[0]["res_info"].ToString();
                List<Record> list = new List<Record>();
                list.Add(record);
                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("QueryWechatRealNameAuthen").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("QueryWechatRealNameAuthen").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        #endregion

        #region 实名认证这一块的需求

        /// <summary>
        /// 免费流量查询
        /// </summary>
        [WebMethod]
        public void GetFreeFlowInfo()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uin", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string uin = paramsHt.ContainsKey("uin") ? paramsHt["uin"].ToString() : "";

                string isVip = "", vip_exp_date = "", authenState = "", freeFlow = "", isBig = "", isTX = "";

                //查询该信息数据库下线，取消查询
                //tapd：57400249 
                //v_swuzhang
                //是否为VIP会员
                //var VipInfos = new CFT.CSOMS.BLL.CFTAccountModule.VIPService().QueryCFTMember(uin);
                //if (VipInfos == null || VipInfos.Tables.Count < 1 || VipInfos.Tables[0].Rows.Count != 1)
                //{
                //    isVip = "否";
                //}
                //else
                //{
                //    isVip = "是";
                //    vip_exp_date = VipInfos.Tables[0].Rows[0]["Fvip_exp_date"].ToString();
                //}

                //实名认证
                bool stateMsg = false;
                var Authen = new CFT.CSOMS.BLL.UserAppealModule.UserAppealService().GetUserAuthenState(uin, "", 0,out stateMsg);
                if (stateMsg)
                {
                    authenState = "是";
                }
                else
                {
                    authenState = "否";
                }

                var Flow = new CFT.CSOMS.BLL.CFTAccountModule.VIPService().GetFreeFlowInfo(uin);
                if (Flow != null && Flow.Tables.Count > 0)
                {
                    DataTable dt = Flow.Tables[0];
                    freeFlow = dt.Rows[0]["free_amount"].ToString();   //免费流量,单位分
                }

                //白名单、大额用户
                var Big = new CFT.CSOMS.BLL.CFTAccountModule.AccountService().GetUserTypeInfo(uin, 3, 1, 0, 1, 1); //1转账白名单
                if (Big != null && Big.Tables.Count > 0 && Big.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = Big.Tables[0];
                    string ys = dt.Rows[0]["eip_user"].ToString();
                    if (ys == "Y")
                    {
                        isBig = "是";
                    }
                    else if (ys == "N")
                    {
                        isBig = "否";
                    }
                    else
                    {
                        isBig = "";//是否为白名单
                    }

                }
                else
                {
                    isBig = "否";
                }

                var TX = new CFT.CSOMS.BLL.CFTAccountModule.AccountService().GetUserTypeInfo(uin, 5, 1, 0, 1, 1); //0提现大额用户   
                if (TX != null && TX.Tables.Count > 0 && TX.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = TX.Tables[0];
                    string ys = dt.Rows[0]["eip_user"].ToString();
                    if (ys == "Y")
                    {
                        isTX = "是";
                    }
                    else if (ys == "N")
                    {
                        isTX = "否";
                    }
                    else
                    {
                        isTX = "";//是否为大额用户
                    }

                }
                else
                {
                    isTX = "否";
                }

                BaseInfoC.FreeFlowInfo freeTable = new BaseInfoC.FreeFlowInfo();
                freeTable.isBig = isBig;
                freeTable.isTX = isTX;
                freeTable.isVip = isVip;
                freeTable.vip_exp_date = vip_exp_date;
                freeTable.authenState = authenState;
                freeTable.freeFlow = freeFlow;
                List<BaseInfoC.FreeFlowInfo> list = new List<BaseInfoC.FreeFlowInfo>();
                list.Add(freeTable);
                APIUtil.Print<BaseInfoC.FreeFlowInfo>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetFreeFlowInfo").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetFreeFlowInfo").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 实名处理查询页面
        /// </summary>
        [WebMethod]
        public void AuthenDealQuery()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uin", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string uin = paramsHt.ContainsKey("uin") ? paramsHt["uin"].ToString() : "";
                int offset = paramsHt.ContainsKey("offset") ? APIUtil.StringToInt(paramsHt["offset"].ToString()) : 0;
                int limit = paramsHt.ContainsKey("limit") ? APIUtil.StringToInt(paramsHt["limit"].ToString()) : 10;

                if (offset < 0)
                    offset = 0;
                if (limit < 0 || limit > 50)
                    limit = 50;

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AuthenInfoService().GetUserClassQueryListForThis(uin, offset, limit);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<BaseInfoC.AuthenDealList> list = APIUtil.ConvertTo<BaseInfoC.AuthenDealList>(infos.Tables[0]);
                APIUtil.Print<BaseInfoC.AuthenDealList>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("AuthenDealQuery").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("AuthenDealQuery").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        #endregion

        /// <summary>
        /// 付款延迟异常查询 根据业务单号进行查询
        /// </summary>
        [WebMethod]
        public void GetPaymenAbnormal()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);
                string listId = paramsHt.ContainsKey("listid") ? paramsHt["listid"].ToString() : "";
                DateTime time = APIUtil.StrToDateTime(listId.Substring(3, 8));

                DataTable dt = new RefundService().GetPaymenAbnormalByFListId(listId, time);
                if (dt == null || dt.Rows.Count <= 0)
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);

                List<BaseInfoC.PaymentAbnormal> list = APIUtil.ConvertTo<BaseInfoC.PaymentAbnormal>(dt);
                APIUtil.Print<BaseInfoC.PaymentAbnormal>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetPaymenAbnormal").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetPaymenAbnormal").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.ToString());
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 冻结
        /// </summary>
        [WebMethod]
        public void AccountFreeze()
        {
            List<BaseInfoC.FreezeThaw> list = new List<BaseInfoC.FreezeThaw>();
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "account", "channel", "executor", "executorip", "username", "contact", "reason", "appid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string account = paramsHt.ContainsKey("account") ? paramsHt["account"].ToString() : "";
                string channel = paramsHt.ContainsKey("channel") ? paramsHt["channel"].ToString() : "";
                string executor = paramsHt.ContainsKey("executor") ? paramsHt["executor"].ToString() : "";
                string executorip = paramsHt.ContainsKey("executorip") ? paramsHt["executorip"].ToString() : "";
                string userName = paramsHt.ContainsKey("username") ? paramsHt["username"].ToString() : "";
                string contact = paramsHt.ContainsKey("contact") ? paramsHt["contact"].ToString() : "";
                string reason = paramsHt.ContainsKey("reason") ? paramsHt["reason"].ToString() : "";

                SunLibrary.LogHelper.LogError(string.Format("AccountFreeze 参数：[{0}，{1}，{2}，{3}，{4}，{5}，{6}]", 
                    account, channel, executor, executorip, userName, contact, reason));

                bool exeSign = false;
                TENCENT.OSS.CFT.KF.KF_Service.Finance_Manage fm = new TENCENT.OSS.CFT.KF.KF_Service.Finance_Manage();
                fm.myHeader = new TENCENT.OSS.CFT.KF.KF_Service.Finance_Header();
                fm.myHeader.UserName = executor;
                fm.myHeader.UserIP = executorip;
                if (account.Contains("@wx.tenpay.com")) 
                {
                    //微信处理流程
                    exeSign = fm.FreezePerAccountWechat_New(account, executor, channel);
                }
                else
                {
                    //冻结 1 ui_freeze_user_service
                    exeSign = fm.freezePerAccount(account, 1, executor, channel);
                }

                //furion 20050906 要先加入工单，不成功不进行下面的工作。
                TENCENT.OSS.CFT.KF.KF_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Service.Query_Service();
                qs.myHeader = new TENCENT.OSS.CFT.KF.KF_Service.Finance_Header();
                qs.myHeader.UserName = executor;
                qs.myHeader.UserIP = executorip;

                try
                {
                    TENCENT.OSS.CFT.KF.KF_Service.FreezeInfo fi = new TENCENT.OSS.CFT.KF.KF_Service.FreezeInfo();
                    fi.FFreezeID = account;
                    fi.FFreezeType = 1;
                    fi.username = userName;
                    fi.contact = contact;
                    fi.FFreezeReason = reason;
                    fi.FFreezeChannel = channel;
                    fi.strFreezeEndDate = "";

                    fi.username = TENCENT.OSS.CFT.KF.KF_Service.PublicRes.replaceMStr(fi.username);
                    fi.contact = TENCENT.OSS.CFT.KF.KF_Service.PublicRes.replaceMStr(fi.contact);
                    fi.FFreezeReason = TENCENT.OSS.CFT.KF.KF_Service.PublicRes.replaceMStr(fi.FFreezeReason);

                    qs.CreateNewFreeze(fi);
                }
                catch (Exception ex)
                {
                    SunLibrary.LogHelper.LogError("创建冻结工单时失败:" + ex.ToString());
                }

                
                if (exeSign) //冻结成功
                {
                    if (account.IndexOf("@wx.tenpay.com") > 0) //发送微信消息
                    {
                        string reqsource = "bus_kf_freeze";
                        string accid = account.Substring(0, account.IndexOf("@wx.tenpay.com"));
                        string templateid = "Td2l1120f5TCN9Ap2R3yWLhVS7yy41U379MZudwmiH0";
                        string cont1 = "你的微信支付账户已成功开启保护模式，账户暂不可用。";
                        string cont2 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        string cont3 = "请点击详情恢复至正常模式";
                        string msgtype = "freeze";
                        try
                        {
                            //为不影响线上，暂不处理异常
                            new FreezeService().SendWechatMsg(reqsource, accid, templateid, cont1, cont2, cont3, msgtype);
                        }
                        catch (Exception ex)
                        {
                            SunLibrary.LogHelper.LogError("发微信冻结消息[new FreezeService().SendWechatMsg]异常：" + ex.ToString());
                        }
                    }

                    BaseInfoC.FreezeThaw freeze = new BaseInfoC.FreezeThaw();
                    freeze.flag = 0;
                    freeze.info = "冻结成功。";
                    list.Add(freeze);
                }
                else
                {
                    //失败
                    BaseInfoC.FreezeThaw freeze = new BaseInfoC.FreezeThaw();
                    freeze.flag = 1;
                    freeze.info = "冻结失败。";
                    list.Add(freeze);
                }                
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("AccountFreeze").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                BaseInfoC.FreezeThaw freeze = new BaseInfoC.FreezeThaw();
                freeze.flag = -1;
                freeze.info =  se.Message;
                list.Add(freeze);
                //APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("AccountFreeze").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.ToString());
                BaseInfoC.FreezeThaw freeze = new BaseInfoC.FreezeThaw();
                freeze.flag = -1;
                freeze.info = ex.Message;
                list.Add(freeze);
                //APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS + "；" + ex.Message);
            }
            APIUtil.Print<BaseInfoC.FreezeThaw>(list);
        }
    }
}
