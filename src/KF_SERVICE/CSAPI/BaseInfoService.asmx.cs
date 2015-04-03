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
                    bank_type =APIUtil.StringToInt( paramsHt["bank_type"].ToString());
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

                string db = "",tb="";

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
                APIUtil.ValidateParamsNew(paramsHt,"appid", "fid", "user", "token");
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
                APIUtil.ValidateParamsNew(paramsHt,"appid", "fid", "user", "reason", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                string comment = "", other_reason="", user_ip = "", db = "", tb = "";
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
                APIUtil.ValidateParamsNew(paramsHt,"appid", "fid", "user", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);
                
             
                string comment="", user_ip="", db="", tb="";
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

        //[WebMethod]
        //public void Hello() 
        //{
        //    try
        //    {
        //        Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
        //        //必填参数验证
        //        APIUtil.ValidateParamsNew(paramsHt,"appid", "uin", "token" );
        //        //token验证
        //        APIUtil.ValidateToken(paramsHt);
        //        //Hashtable map = APIUtil.getReqParamMap();
              
        //        //业务处理
        //        var infos = new CFT.CSOMS.BLL.FundModule.FundService().GetTradeIdByUIN(paramsHt["uin"].ToString());

        //        //封装或转换成list
        //        Record record = new Record();
        //        record.RetValue = infos;
        //        List<Record> list = new List<Record>();
        //        list.Add(record);

        //        //输出
        //        APIUtil.Print<Record>(list);
                
        //    }
        //    catch (ServiceException se)
        //    {
        //        SunLibrary.LoggerFactory.Get("ConfirmAppeal").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
        //        APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
                
        //    }
        //    catch (Exception ex)
        //    {
        //        SunLibrary.LoggerFactory.Get("ConfirmAppeal").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
        //        APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
                
        //    }
        //}


        #endregion

        #region 证件号码清理

        [WebMethod(Description = "证件信息查询")]
        public void QueryCreidList()    //string appid,string creid,string token
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

        [WebMethod(Description = "证件号码清理")]
        public void ClearCreid()    //string appid,string creid,int type,string opera,string token ; type=0普通用户，type=1微信用户
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
                int u_type = APIUtil.StringToInt(paramsHt["type"]);

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
                APIUtil.ValidateParamsNew(paramsHt, "appid", "qqid", "opera","token");
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

        #endregion
    }
}
