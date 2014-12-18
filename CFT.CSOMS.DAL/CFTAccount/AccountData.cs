using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.DAL.Infrastructure;
using System.Configuration;
using CFT.CSOMS.COMMLIB;
using System.Net;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using CFT.Apollo.Logging;

namespace CFT.CSOMS.DAL.CFTAccount
{
    public class AccountData
    {
        public DataTable QuerySubAccountInfo(string uin, int currencyType)
        {
            string fuid = ConvertToFuid(uin);
            if (fuid == null)
                fuid = "0";

            ICEAccess ice = null;
            if (currencyType != 1)
            {
                ice = ICEAccessFactory.GetICEAccess("ICEConnectionString3");
            }
            else
            {
                ice = ICEAccessFactory.GetICEAccess("ICEConnectionString");
            }

            try
            {
                ice.OpenConn();
                string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
                strwhere += ICEAccess.URLEncode("fcurtype=" + currencyType + "&");
                string strResp = "";

                DataTable dt = ice.InvokeQuery_GetDataTable(YWSourceType.用户资源, YWCommandCode.查询用户信息, fuid, strwhere, out strResp);

                if (dt == null || dt.Rows.Count == 0)
                    return null;

                ice.CloseConn();

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                ice.Dispose();
            }
        }

        public static string ConvertToFuid(string uin)
        {
            try
            {
                if (string.IsNullOrEmpty(uin))
                    throw new ArgumentNullException("uin");
               
                string errMsg = "";
                string strSql = "uin=" + uin;
                var fuid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_RELATION, "fuid", out errMsg);

                if (!string.IsNullOrEmpty(errMsg))
                    throw new Exception(errMsg);

                return fuid;

            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("查询Fuid异常:{0}", ex.Message));
            }
        }

        public static string Uid2QQ(string uid) 
        {
            try
            {
                string errMsg = "";
                string strSql = "uid=" + uid;
                DataSet ds = CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_USERINFO, out errMsg);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 1)
                {
                    return null;
                }

                string strtmp = ds.Tables[0].Rows[0]["fqqid"].ToString();
                if (strtmp != "")
                {
                    string fuid = ConvertToFuid(strtmp);
                    if (fuid != null && fuid == uid)
                    {
                        return strtmp;
                    }
                }

                strtmp = ds.Tables[0].Rows[0]["Femail"].ToString();
                if (strtmp != "")
                {
                    string fuid = ConvertToFuid(strtmp);
                    if (fuid != null && fuid == uid)
                    {
                        return strtmp;
                    }
                }

                strtmp =ds.Tables[0].Rows[0]["Fmobile"].ToString();
                if (strtmp != "")
                {
                    string fuid = ConvertToFuid(strtmp);
                    if (fuid != null && fuid == uid)
                    {
                        return strtmp;
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public static string GetUserNameFromUin(string uin) 
        {
            try
            {
                if (uin == null || uin.Trim().Length < 3)
                    return "";

                string uid = ConvertToFuid(uin);
                string strSql = "uid=" + uid;
                string errMsg = "";
                string usertype = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fuser_type", out errMsg);

                string fieldstr = "Fcompany_name";
                if (usertype == "1")
                {
                    fieldstr = "Ftruename";
                }

                return CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, fieldstr, out errMsg);
            }
            catch 
            {
                return "";
            }
        }

        //同步姓名
        public string SynUserName(string aaUin, string oldName, string newName, string wxUin) 
        {
            string msg = "";
            string SynUserNameKey = ConfigurationManager.AppSettings["SynUserNameKey"];
            string signMd5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(aaUin + "|" + wxUin + "|" + SynUserNameKey, "md5").ToLower();
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append("uin="+aaUin);
            strWhere.Append("&old_true_name="+oldName);
            strWhere.Append("&new_true_name=" + newName);
            strWhere.Append("&wxPayUin=" + wxUin);
            strWhere.Append("&sign=" + signMd5);

            CommQuery.GetOneTableFromICE(strWhere.ToString(), "", "aac_syncusername_service", out msg);
            if (msg != "") {
                return msg;
            }

            return "0";
        }

        public bool RemoveUserControlFin(string uid, string cur_type, string balance, string opera,int type)
        {
            if (string.IsNullOrEmpty(uid.Trim()))
            {
                throw new ArgumentNullException("uid为空！");
            }
            if (string.IsNullOrEmpty(opera.Trim()))
            {
                throw new ArgumentNullException("opera为空！");
            }
           
            string cgi = "";
            string msg = "";
            cgi = System.Configuration.ConfigurationManager.AppSettings["UserControlFinCgi"].ToString();
            cgi += "uid=" + uid;
            if (type == 3)//解绑指定金额
            {
                if (string.IsNullOrEmpty(cur_type.Trim()))
                {
                    throw new ArgumentNullException("cur_type为空！");
                }
                if (string.IsNullOrEmpty(balance.Trim()))
                {
                    throw new ArgumentNullException("balance为空！");
                }
                cgi += "&type=3";
                cgi += "&curtype=" + cur_type;
                cgi += "&balance=" + balance;//传分
                cgi += "&opera=" + opera;
            }
            else if (type == 4)//解绑所有子账户余额
            {
                cgi += "&type=4";
                cgi += "&opera=" + opera;
            }

           // 测试 cgi = "http://check.cf.com/cgi-bin/v1.0/BauClrBan.cgi?uid=400061433&type=1009&sum=2850&opera=1100000000";
            LogHelper.LogInfo("RemoveUserControlFin send req:" + cgi);
            string res = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetFromCGI(cgi, "", out msg);
            if (msg != "")
            {
                throw new Exception(msg);
            }
            LogHelper.LogInfo("RemoveUserControlFin return:" + res);

            if (res.IndexOf("执行成功") == -1)
                throw new Exception("解除失败："+res);
            else
                return true;
        }

        public DataTable QueryUserControledRecordCgi(string uid, string opera)
        {
          
            string cgi = "";
            string msg = "";
            cgi = System.Configuration.ConfigurationManager.AppSettings["UserControlFinCgi"].ToString();
            cgi += "uid=" + uid;
            cgi += "&type=1";
            cgi += "&opera=" + opera;

            // 测试 cgi = "http://check.cf.com/cgi-bin/v1.0/BauClrBan_new.cgi?type=1&uid=441845935&opera=yukini";
            LogHelper.LogInfo("QueryUserControledRecordCgi send req:" + cgi);
            string res = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetFromCGI(cgi, "", out msg);
            if (msg != "")
            {
                throw new Exception(msg);
            }
            LogHelper.LogInfo("QueryUserControledRecordCgi return:" + res);

            DataTable dt = new DataTable();
            dt.Columns.Add("cur_type", typeof(String));
            dt.Columns.Add("balance", typeof(String));

            if (res.IndexOf("执行成功") == -1)
            {
                throw new Exception("cgi查询失败，返回："+res);
            }
            res = res.Replace("执行成功 ","");
            string[] ans = res.Split(' ');
            DataRow drfield = dt.NewRow();

          
            foreach (string strtmp in ans)
            {
                string[] strlist2 = strtmp.Split(',');
                if (strlist2.Length != 2)
                {
                    continue;
                }
                string[] para=strlist2[0].Split(':');
                if (para.Length != 2)
                {
                    continue;
                }

                drfield.BeginEdit();

                drfield["cur_type"] = PublicRes.getCgiString(para[1].Trim());

                para = strlist2[1].Split(':');
                if (para.Length != 2)
                {
                    continue;
                }
                drfield["balance"] = PublicRes.getCgiString(para[1].Trim());

                drfield.EndEdit();
                dt.Rows.Add(drfield);
            }
           
            return dt;
        }

        /// <summary>
        /// 查询用户信息：邮箱、身份证号、身份证类型
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public DataTable GetUserInfoBase(string uid)
        {
            string errMsg = "";
            string strSql = "uid=" + uid;
            DataTable dtuserinfo = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_USERINFO, out errMsg);

            if (dtuserinfo == null || dtuserinfo.Rows.Count != 1)
                throw new Exception("调用ICE查询t_user_info出错" + errMsg);
            return dtuserinfo;
        }

        /// <summary>
        /// 查询用户银行账户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string GetUserBankId(string uid)
        {
            string errMsg = "";
            string strSql = "uid=" + uid + "&curtype=1";
            string Fbankid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_BANKUSER, "Fbankid", out errMsg);
            return Fbankid;
        }

        /// <summary>
        /// 注销账户
        /// </summary>
        /// <param name="qqid"></param>
        /// <param name="reason"></param>
        /// <param name="user"></param>
        /// <param name="userIP"></param>
        /// <returns></returns>
        public bool LogOnUser(string qqid, string reason, string user, string userIP)
        {
            string Msg = null;
            var fmda = MySQLAccessFactory.GetMySQLAccess("ht_DB");

            try
            {
                //furion 20080522 删除实名认证.
                string inmsg = "uin=" + qqid;
                inmsg += "&operator=" + user;
                inmsg += "&memo=帐务销户删除";

                string reply;
                short sresult;

                //3.0接口测试需要 furion 20090708
                if (commRes.middleInvoke("au_del_authen_service", inmsg, true, out reply, out sresult, out Msg))
                {
                    if (sresult != 0)
                    {
                        Msg = "au_del_authen_service接口失败：result=" + sresult + "，msg=" + Msg + "&reply=" + reply;
                        LogHelper.LogInfo("LogOnUser:" + Msg);
                        return false;
                    }
                    else
                    {
                        if (reply.IndexOf("result=0") > -1)
                        {
                        }
                        else if (reply.IndexOf("result=22520101") > -1)
                        {
                        }
                        else
                        {
                            Msg = "au_del_authen_service接口失败：result=" + sresult + "，msg=" + Msg + "&reply=" + reply;
                            LogHelper.LogInfo("LogOnUser:" + Msg);
                            return false;
                        }
                    }
                }
                else
                {
                    Msg = "au_del_authen_service接口失败：result=" + sresult + "，msg=" + Msg + "&reply=" + reply;
                    LogHelper.LogInfo("LogOnUser:" + Msg);
                    return false;
                }

                //end 删除实名认证
                //da.OpenConn();
                //da.StartTran();

                //da_zl.OpenConn();
                //da_zl.StartTran();

                fmda.OpenConn();
                fmda.StartTran();

                //销户判断。如果用户有未结束的交易或者是由余额，冻结余额等，不允许操作。 目前做最简单金额判断
                string uid = ConvertToFuid(qqid);
                if (uid == null || uid.Trim() == "" || uid.Trim() == "0")
                {
                    Msg = "获取QQ" + qqid + "号码对应的UID失败!";
                    LogHelper.LogInfo("LogOnUser:" + Msg);
                    //return false;
                    //furion 20070130 如果是已销户的,就返回真,这样可重复调用.
                    return true;
                }

                //				string str = "select fbalance,fcon from " + PublicRes.GetTName("t_user",uid) + " where fuid = '" + uid + "'";
                //				DataTable dt = PublicRes.returnDSAll(str,"YW_30").Tables[0];
                //				
                //				if (dt != null && dt.Rows.Count !=0 && dt.Rows[0]["fbalance"] != null && dt.Rows[0]["fcon"] != null)
                //				{
                //					string banlance = dt.Rows[0]["fbalance"].ToString().Trim();
                //					string fcon     = dt.Rows[0]["fcon"].ToString().Trim();
                //
                ////					if (banlance != "0" || fcon != "0")  //取消判断。用户销户审批通过后，即可以进行销户。
                ////					{
                ////						Msg = "销户帐户" + qqid + "的余额[" + banlance + "]或者冻结余额[" + fcon + "]不为０.不允许进行销户操作．";
                ////						commRes.sendLog4Log("Finance_Manage.logOnUser",Msg);
                ////						return false;
                ////					}	
                // 				}


                //插入历史备份数据库
                string nowTime = PublicRes.strNowTimeStander;
                string insertStr = "insert into c2c_fmdb.t_logon_history (fqqid,fquid,freason,handid,handip,flastMOdifyTime) values ('"
                    + qqid + "','" + ConvertToFuid(qqid) + "','" + commRes.replaceSqlStr(reason) + "','" + user + "','" + userIP + "','" + nowTime + "')";
                if (!fmda.ExecSql(insertStr))
                {
                    //da.RollBack();
                    fmda.RollBack();

                    Msg = "销户时插入历史备份数据库出错。";
                    LogHelper.LogInfo("LogOnUser:" + Msg);
                    commRes.sendLog4Log("FinanceManage.LogOnUser", Msg);
                    return false;
                }

                /*
                // TODO1: 客户信息资料外移
                //furion 20070206 如果销户,就全消,所以要查询出来所有的绑定的帐号.
                string strSql = "select ifnull(fqqid,''),ifnull(Femail,''),ifnull(Fmobile,'') from " 
                    + PublicRes.GetTName("t_user_info",uid) + " where fuid= '" + uid + "'";
                //DataSet ds = da.dsGetTotalData(strSql);
                DataSet ds = da_zl.dsGetTotalData(strSql);
				
                */

                string strSql = "uid=" + uid;
                DataSet ds = CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_USERINFO, out Msg);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 1)
                {
                    fmda.RollBack();
                    Msg = "获取用户资料时出错." + Msg;
                    LogHelper.LogInfo("LogOnUser:" + Msg);
                    return false;
                }

                string strtmp = QueryInfo.GetString(ds.Tables[0].Rows[0]["fqqid"]);
                if (strtmp != "")
                {
                    string fuid = ConvertToFuid(strtmp);
                    if (fuid != null && fuid == uid)
                    {
                        /*
                        //清空QQ号对应的对应关系表
                        string delStr = "delete from " + PublicRes.GetTName("t_relation",strtmp) + " where fqqid ='" + strtmp + "'";

                        //furion 20090302 t_relation移到用户资料库里。
                        //int i = da.ExecSqlNum(delStr);
                        int i = da_zl.ExecSqlNum(delStr);
                        */

                        strSql = "uin=" + strtmp;
                        int i = CommQuery.ExecSqlFromICE(strSql, CommQuery.DELETE_RELATION, out Msg);


                        if (i < 0)
                        {
                            //da.RollBack();
                            fmda.RollBack();
                            //da_zl.RollBack();

                            Msg = "删除错误！原因：" + Msg;
                            LogHelper.LogInfo("LogOnUser:" + Msg);
                            commRes.sendLog4Log("FinanceManage.LogOnUser", Msg);
                            return false;
                        }
                    }
                }

                strtmp = QueryInfo.GetString(ds.Tables[0].Rows[0]["Femail"]);
                if (strtmp != "")
                {
                    string fuid = ConvertToFuid(strtmp);
                    if (fuid != null && fuid == uid)
                    {
                        //清空email对应的对应关系表
                        string uid3 = "";
                        if (!TENCENT.OSS.CFT.KF.Common.DESCode.GetEmailUid(strtmp, out uid3))
                        {
                            //da.RollBack();
                            fmda.RollBack();
                            //da_zl.RollBack();

                            Msg = "获取内部ID时出错." + uid3;
                            LogHelper.LogInfo("LogOnUser:" + Msg);
                            return false;
                        }

                        /*
                        string delStr = "delete from " + PublicRes.GetTName("t_relation",uid3) + " where fqqid ='" + strtmp + "'";

                        //furion 20090302 t_relation移到用户资料库里。
                        //int i = da.ExecSqlNum(delStr);
                        int i = da_zl.ExecSqlNum(delStr);
                        */

                        strSql = "uin=" + strtmp;
                        int i = CommQuery.ExecSqlFromICE(strSql, CommQuery.DELETE_RELATION, out Msg);


                        if (i < 0)
                        {
                            //da.RollBack();
                            fmda.RollBack();
                            //da_zl.RollBack();

                            Msg = "删除错误！删除条数： " + i + " 条。";
                            LogHelper.LogInfo("LogOnUser:" + Msg);
                            commRes.sendLog4Log("FinanceManage.LogOnUser", Msg);
                            return false;
                        }

                    }
                }

                strtmp = QueryInfo.GetString(ds.Tables[0].Rows[0]["Fmobile"]);
                if (strtmp != "")
                {
                    string fuid = ConvertToFuid(strtmp);
                    if (fuid != null && fuid == uid)
                    {
                        /*
                        //清空手机号对应的对应关系表
                        string delStr = "delete from " + PublicRes.GetTName("t_relation",strtmp) + " where fqqid ='" + strtmp + "'";

                        //furion 20090302 t_relation移到用户资料库里。
                        //int i = da.ExecSqlNum(delStr);
                        int i = da_zl.ExecSqlNum(delStr);
                        */
                        strSql = "uin=" + strtmp;
                        int i = CommQuery.ExecSqlFromICE(strSql, CommQuery.DELETE_RELATION, out Msg);

                        if (i < 0)
                        {
                            //da.RollBack();
                            fmda.RollBack();
                            //da_zl.RollBack();

                            Msg = "删除错误！删除条数： " + i + " 条。";
                            LogHelper.LogInfo("LogOnUser:" + Msg);
                            commRes.sendLog4Log("FinanceManage.LogOnUser", Msg);
                            return false;
                        }
                    }
                }


                //da.Commit();
                fmda.Commit();
                //da_zl.Commit();
                return true;
            }
            catch (Exception e)
            {
                //da.RollBack();
                fmda.RollBack();
                //da_zl.RollBack();

                Msg = "删除用户关系对应表记录失败！请检查！" + commRes.replaceHtmlStr(e.Message);
                LogHelper.LogInfo("LogOnUser:" + Msg);
                commRes.sendLog4Log("FinanceManage.LogOnUser", Msg);
                return false;
            }
            finally
            {
                //da.Dispose();
                fmda.Dispose();
                //da_zl.Dispose();
            }


        }

        /// <summary>
        /// 通过微信号，微信绑定信息查微信财付通账号
        /// </summary>
        /// <param name="queryTypeTemp"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static string getQQID(string queryTypeTemp, string queryString)
        {
            if (string.IsNullOrEmpty(queryString))
            {
                throw new Exception("请输入要查询的账号");
            }
            var id = queryString;
            if (queryTypeTemp == "WeChatCft")
            {
                return id;
            }
            else if (queryTypeTemp == "WeChatUid")
            {
                return Uid2QQ(id);
            }
            else if (queryTypeTemp == "WeChatQQ" || queryTypeTemp == "WeChatMobile" || queryTypeTemp == "WeChatEmail")
            {
                string queryType = string.Empty;
                if (queryTypeTemp == "WeChatQQ")
                {
                    queryType = "QQ";
                }
                else if (queryTypeTemp == "WeChatMobile")
                {
                    queryType = "Mobile";
                }
                else if (queryTypeTemp == "WeChatEmail")
                {
                    queryType = "Email";
                }

                string openID = string.Empty, errorMessage = string.Empty;
                int errorCode = 0;
                var IPList = ConfigurationManager.AppSettings["WeChat"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < IPList.Length; j++)
                {
                    if (getOpenIDFromWeChat(queryType, id, out openID, out errorCode, out errorMessage, IPList[j]))
                    {
                        break;
                    }
                }
                if (errorCode == 0)
                {
                    return openID + "@wx.tenpay.com";
                }
                else if (errorCode == 1)
                {
                    LogHelper.LogInfo("getQQID:没有此用户");
                    throw new Exception("没有此用户");
                }
                else
                {
                    throw new Exception(errorCode + errorMessage);
                }
            }
            else if (queryTypeTemp == "WeChatId")
            {
                return WeChatHelper.GetUINFromWeChatName(id);
            }

            return id;
        }

        //通过微信绑定的QQ、手机或邮箱信息查询其openID，对应的财付通账号便是openID@wx.tenpay.com
        private static bool getOpenIDFromWeChat(string queryType, string ID, out string openID, out int errorCode, out string errorMessage, string IP)
        {
            openID = errorMessage = string.Empty;
            errorCode = 0;
            try
            {
                string parameterString = "<Request>{0}<AppId>wx482cac0d58846383</AppId></Request>";
                string IDstring = string.Empty;
                string API;
                if (queryType == "QQ")
                {
                    IDstring = string.Format("<QQ>{0}</QQ>", ID);
                    API = "ConvertQQToOuterAcctId";
                }
                else if (queryType == "Mobile")
                {
                    IDstring = string.Format("<Mobile>{0}</Mobile>", ID);
                    API = "ConvertMobileToOuterAcctId";
                }
                else if (queryType == "Email")
                {
                    IDstring = string.Format("<Email>{0}</Email>", ID);
                    API = "ConvertEmailToOuterAcctId";
                }
                else
                {
                    errorCode = -1;
                    errorMessage = "查询类型不正确";
                    return false;
                }
                parameterString = string.Format(parameterString, IDstring);
                var data = Encoding.Default.GetBytes(parameterString);
                var request = (HttpWebRequest)WebRequest.Create(string.Format("http://{0}:12137/cgi-bin/{1}?f=xml&appname=wx_tenpay", IP, API));
                request.Method = "POST";
                request.ContentType = "text/xml;charset=UTF-8";
                var parameter = request.GetRequestStream();
                parameter.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var myResponseStream = response.GetResponseStream();
                var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                var resultXml = new XmlDocument();
                resultXml.LoadXml(myStreamReader.ReadToEnd());
                myStreamReader.Close();
                myResponseStream.Close();
                var responseNode = resultXml.SelectSingleNode("Response");
                LogHelper.LogInfo("getQQID:" + resultXml.InnerXml);
                errorCode = Convert.ToInt32(responseNode.SelectSingleNode("error").SelectSingleNode("code").InnerText);
                errorMessage = responseNode.SelectSingleNode("error").SelectSingleNode("message").InnerText;
                openID = responseNode.SelectSingleNode("result").SelectSingleNode("OuterAcctId").InnerText;
                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                LogHelper.LogInfo("getQQID:" + errorMessage);
                return false;
            }
        }

        public DataSet QueryVipInfo(string uin)
        {

            if (string.IsNullOrEmpty(uin.Trim()))
                throw new ArgumentNullException("uin");
            string inmsg = "uin=" + uin.Trim();
            inmsg += "&query_str=level|value|vipflag|subid|exp_date";
            string vipQuer_ip = ConfigurationManager.AppSettings["vipQuer_IP"];
            string vipQuer_port = ConfigurationManager.AppSettings["vipQuer_PORT"];

            string Msg = "";
            string errMsg = "";

            string answer = commRes.GetFromRelayHead4(inmsg, vipQuer_ip, vipQuer_port, out Msg);

            if (answer == "")
            {
                return null;
            }
            if (Msg != "")
            {
                throw new Exception(Msg);
            }

            answer=PublicRes.getCgiStringUtil(answer);
            //解析relay str
            DataSet ds = CommQuery.ParseRelayStr(answer, out errMsg);
            if (errMsg != "")
            {
                throw new Exception(errMsg);
            }
            return ds;

        }

        /// <summary>
        /// 添加姓名异常信息
        /// </summary>
        /// <param name="nameAbnormal"></param>
        public void AddNameAbnormalInfo(NameAbnormalClass nameAbnormal)
        {
            try
            {
                //string serCre_id = BankLib.BankIOX.Encrypt(nameAbnormal.Fcre_id);//证件号加密

                string serCre_id = CommUtil.EncryptZerosPadding(nameAbnormal.Fcre_id);//证件号加密
                using (var da = MySQLAccessFactory.GetMySQLAccess("NameAbnormal"))
                {
                    da.OpenConn();
                    string Sql = string.Format("insert into c2c_fmdb.t_name_abnormal_check "+
                        "(Fuin,Fname_old,Fcre_id_old,Ftruename,Fcre_id,Fcre_type,Fcre_version,Fcre_valid_day,"+
                    "Faddress,Fimage_cre1,Fimage_cre2,Fimage_evidence,Fsubmit_time,Fsubmit_user,Fcheck_time,Fcheck_user,"+
                    "Fcheck_state,Frefuse_reason,Fcomment)" +
                        "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}')",
                       nameAbnormal.Fuin, nameAbnormal.Fname_old, nameAbnormal.Fcre_id_old, 
                       nameAbnormal.Ftruename, serCre_id, nameAbnormal.Fcre_type, 
                       nameAbnormal.Fcre_version, nameAbnormal.Fcre_valid_day, nameAbnormal.Faddress,
                       nameAbnormal.Fimage_cre1, nameAbnormal.Fimage_cre2, nameAbnormal.Fimage_evidence,
                       nameAbnormal.Fsubmit_time, nameAbnormal.Fsubmit_user, nameAbnormal.Fcheck_time, 
                       nameAbnormal.Fcheck_user, nameAbnormal.Fcheck_state, nameAbnormal.Frefuse_reason, 
                       nameAbnormal.Fcomment);
                    if (!da.ExecSql(Sql))
                    {
                        throw new Exception("添加出错");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("添加异常姓名出错：" + ex.Message);
            }
        }

        /// <summary>
        /// 查询姓名异常信息
        /// </summary>
        /// <param name="uin">uin</param>
        /// <param name="check_state">审批状态</param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public DataSet QueryNameAbnormalInfo(string uin,int check_state,string cre_id_old,int limit, int offset)
        {
            try
            {
                using (var da = MySQLAccessFactory.GetMySQLAccess("NameAbnormal"))
                {
                    da.OpenConn();
                    string Sql = " select Fid, Fuin,Fname_old,Fcre_id_old,Ftruename,Fcre_id,Fcre_type,Fcre_version,Fcre_valid_day,Faddress,Fimage_cre1,Fimage_cre2,Fimage_evidence,Fsubmit_time,Fsubmit_user,Fcheck_time,Fcheck_user,Fcheck_state,Frefuse_reason,Fcomment "
                        + "from c2c_fmdb.t_name_abnormal_check "
                        + "where Fuin='"+uin+"' ";

                    if (check_state!=99)
                        Sql += " and Fcheck_state='" + check_state + "' ";
                    if (!string.IsNullOrEmpty(cre_id_old))
                        Sql += " and Fcre_id_old='" + cre_id_old + "' ";
                    Sql += " order by Fsubmit_time desc limit " + limit + "," + offset ;

                    DataSet ds = da.dsGetTotalData(Sql);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            //证件号解密
                            row["Fcre_id"] = CommUtil.BankIDEncode_ForBankCardUnbind(row["Fcre_id"].ToString()).Replace("\0","");
                        }
                    }

                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("查询公告联系人所有分组出错：" + ex.Message);
            }
        }

        /// <summary>
        /// 更新姓名异常信息
        /// </summary>
        /// <param name="uin"></param>
        /// <param name="refuse_reason">拒绝原因</param>
        /// <param name="comment">人工填写备注</param>
        /// <param name="check_user">审批人</param>
        /// <param name="check_state">审批结果</param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool UpdateNameAbnormalInfo(string uin, string refuse_reason,string comment,string check_user,string check_state)
        {
            try
            {
                using (var da = MySQLAccessFactory.GetMySQLAccess("NameAbnormal"))
                {
                    da.OpenConn();
                    string Sql = string.Format(" update c2c_fmdb.t_name_abnormal_check set Fcheck_time='{0}',Fcheck_user='{1}',Fcheck_state='{2}',Frefuse_reason='{3}',Fcomment='{4}' "
                        + "where Fuin='{5}' and Fcheck_state='0'",
                        System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), check_user, check_state, refuse_reason, comment,uin);
                   
                    int num=da.ExecSqlNum(Sql);
                    if ( num== 1)
                        return true;
                    else
                    {
                        string error = "更新记录时出错 uin:" + uin + "  更新记录数:" + num;
                        LogHelper.LogInfo(error);
                        return false;
                        throw new Exception(error);
                    }
                }
            }
            catch (Exception ex)
            {
                string error = "更新姓名异常信息出错：" + ex.Message;
                LogHelper.LogInfo(error);
                throw new Exception(error);
            }
        }

        /// <summary>
        /// 通过relay 查询用户信息：姓名 证件号
        /// </summary>
        /// <param name="uin"></param>
        /// <param name="check_user"></param>
        /// <returns></returns>
        public DataSet QueryRealNameInfo(string uin, string submit_user)
        {
            string fuid = AccountData.ConvertToFuid(uin);
            if (fuid == null || fuid == "")
            {
                throw new Exception("根据C帐号获取Fuid失败	qqid:" + uin);
            }
            if (fuid == null || fuid.Length < 3)
            {
                throw new Exception("内部ID不正确！");
            }
            //string ip = "10.12.23.14";
            //int port = 35600;

            string ip = System.Configuration.ConfigurationManager.AppSettings["RealNameQueryIP"].ToString();
            int port =int.Parse(System.Configuration.ConfigurationManager.AppSettings["RealNameQueryPort"].ToString());
            string requestString = "uin=" + uin + "&uid=&operator=" + submit_user;
            DataSet ds = RelayAccessFactory.GetDSFromRelay(requestString, "100587", ip, port);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
               string key = System.Configuration.ConfigurationManager.AppSettings["RealNameKey"].ToString();
                DataRow row = ds.Tables[0].Rows[0];
                key += fuid + submit_user;
                row["cname"] = CommUtil.TripleDESDecryptRealName(row["cname"].ToString(), key);
                row["cre_id_tail"] = CommUtil.TripleDESDecryptRealName(row["cre_id_tail"].ToString(), key);

                //string name = CommUtil.TripleDESDecrypt("6292B13DBE70B45B", "ba6f0d572c4c18e0ae9e4d915a56a1cd" + fuid + submit_user);
            }
            return ds;
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

            string fuid = AccountData.ConvertToFuid(nameAbnormal.Fuin);
            if (fuid == null || fuid == "")
            {
                throw new Exception("根据C帐号获取Fuid失败	uin:" + nameAbnormal.Fuin);
            }
            if (fuid == null || fuid.Length < 3)
            {
                throw new Exception("内部ID不正确！");
            }

            string requestString = "uid=" + fuid;
            requestString += "&truename=" + nameAbnormal.Ftruename;
            requestString += "&cre_id=" + nameAbnormal.Fcre_id;
            requestString += "&cre_type=" + nameAbnormal.Fcre_type;
            requestString += "&cre_version=" + nameAbnormal.Fcre_version;
            requestString += "&path=" + nameAbnormal.Fimage_cre1 + "|" + nameAbnormal.Fimage_cre1;
            requestString += "&cre_valid_day=" + nameAbnormal.Fcre_valid_day;
            requestString += "&address=" + nameAbnormal.Faddress;
            requestString += "&MSGNO=" + DateTime.Now.ToString("yyyyMMddHHmmss");
            requestString += "&MSG_NO=" + DateTime.Now.ToString("yyyyMMddHHmmss");
            
            //string ip = "10.12.23.14";
            //int port = 35600;

            string ip = System.Configuration.ConfigurationManager.AppSettings["RealNameQueryIP"].ToString();
            int port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["RealNameQueryPort"].ToString());
            RelayAccessFactory.GetDSFromRelay(requestString, "5547", ip, port,true);
            return true;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="creid"></param>
        /// <param name="userType"></param>
        /// <param name="Uid"></param>
        public void WriteClearCreidLog(string creid, int userType, string Uid)
        {
            var tableName = GetLogTableName(creid);
            string sql = string.Format(@"INSERT INTO {0}(FUid,FCreid,FUser_type) VALUES('{1}','{2}',{3})", tableName, Uid, creid, userType);
            using (var da = MySQLAccessFactory.GetMySQLAccess("ClrearCreidLog"))
            {
                da.OpenConn();
                da.ExecSql(sql);
            }
        }

        /// <summary>
        /// 查询日志
        /// </summary>
        /// <param name="creid"></param>
        /// <returns></returns>
        public DataTable GetClearCreidLog(string creid)
        {
            var tableName = GetLogTableName(creid);
            string sql = string.Format(@"SELECT FCreid,FCreate_time,CASE FUser_type WHEN 0 THEN '普通用户' WHEN 1 THEN '微信用户' END AS FUser_type ,FUid FROM {0}  WHERE FCreid='{1}'", tableName, creid);
            using (var da = MySQLAccessFactory.GetMySQLAccess("ClrearCreidLog"))
            {
                da.OpenConn();
                return da.GetTable(sql);
            }
        }

        /// <summary>
        /// 获取清理次数
        /// </summary>
        /// <param name="creid"></param>
        /// <returns></returns>
        public int GetClearCreidCount(string creid, int userType)
        {
            var tableName = GetLogTableName(creid);
            string sql = string.Format(@"SELECT COUNT(FID) FROM {0} WHERE FCreid='{1}' AND FUser_type ={2}", tableName, creid, userType);
            using (var da = MySQLAccessFactory.GetMySQLAccess("ClrearCreidLog"))
            {
                da.OpenConn();
                var temp = da.GetOneResult(sql);
                return temp != null ? Convert.ToInt32(temp) : 0;
            }
        }

        /// <summary>
        /// 获取日志表名
        /// </summary>
        /// <param name="creid"></param>
        /// <returns></returns>
        private string GetLogTableName(string creid)
        {
            var tableName = "t_creid_Log";
            if (creid.Length > 2)
            {
                string s_db = "";
                string l_s = creid.Substring(creid.Length - 1);

                if (l_s.ToUpper() == "X")
                {
                    s_db = "00";
                    tableName = "statistics_db_" + s_db + "." + tableName + "_0";
                }
                else
                {
                    s_db = creid.Substring(creid.Length - 2);
                    tableName = "statistics_db_" + s_db + "." + tableName + "_" + creid.Substring(creid.Length - 3, 1);
                }
            }
            return tableName;
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
        public string Frefuse_reason;
        public string Fcomment;

    }
    #endregion
}
