﻿using CFT.CSOMS.BLL.CFTAccountModule;
using CFT.CSOMS.COMMLIB;
using SunLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.DataAccess;

namespace TENCENT.OSS.CFT.KF.KF_Web
{
    public partial class KFWebTest : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogHelper.LogInfo(" public partial class KFWebTest : TENCENT.OSS.CFT.KF.KF_Web.PageBase  ");
            if (Request["wechatname"] != null)
            {
                LogHelper.LogInfo(" KFWebTest.aspx  request key ：" + Request["wechatname"].ToString());


                LogHelper.LogInfo(" KFWebTest.aspx  wechatname ：" + Request["wechatname"].ToString());
                WeChatInfo(Request["wechatname"].ToString());
            }

            if (Request["dbkey"] != null)
            {
                LogHelper.LogInfo(" KFWebTest.aspx  request key ：" + Request["dbkey"].ToString());

                if (Request["gofunc"] != null && Request["gofunc"].ToLower() == "checkdbconn")
                {
                    LogHelper.LogInfo(" KFWebTest.aspx  dbkey ：" + Request["dbkey"].ToString());
                    CheckDBConn(Request["dbkey"].ToString());
                }
                else
                {
                    LogHelper.LogInfo(" KFWebTest.aspx  dbkey ：" + Request["dbkey"].ToString());
                    GetDBConnStr(Request["dbkey"].ToString());
                }
            }

            if (Request["reqtype"] != null)
            {
                var reqtype= Request["reqtype"].ToString().ToLower();
                LogHelper.LogInfo(" KFWebTest.aspx  request reqtype ：" + reqtype);

                switch (reqtype)
                {
                    case "sendmail":
                        var actiontype = Request["actiontype"] != null ? Request["actiontype"].ToString() : "11914";
                        LogHelper.LogInfo(" KFWebTest.aspx  request actiontype ：" + actiontype);
                        SendEmail(actiontype);
                        break;
                    case "updateuserinfoattr":
                        var qqid = Request["qqid"] != null ? Request["qqid"].ToString() : "380885873";
                        LogHelper.LogInfo(" KFWebTest.aspx  request qqid ：" + qqid);
                        UpdateUserInfoAttr(qqid);
                        break;
                }
            }

        }

        private void GetDBConnStr(string strkey) {
           string dbstr = CommLib.DbConnectionString.Instance.GetConnectionString(strkey.Trim().ToUpper());

           LogHelper.LogInfo(" test.aspx  private void GetDBConnStr  strKey：" +strkey+",dbstr:"+ dbstr);

            string qq="404968099";
                if (Request["qq"] != null){
                    qq = Request["qq"].ToString();
                }
            this.Label1.Text = dbstr;

            bindshimingrzData(qq);

           Response.Write(dbstr);

        }


        #region  企业C财付通帐号注销通知

        private void SendEmail(string actiontype)
        {
            try
            {
                CommMailSend.SendMsg("yukini@tencent.com", actiontype, "Fqqid=380885873", 2);
            }
            catch (Exception ep)
            {
                LogHelper.LogError(" KFWebTest.aspx  SendEmail ：" + ep);
                try
                {
                    CommMailSend.SendMsg("yukini@tencent.com", actiontype, "Fqqid=yukini@tencent.com", 1);
                }
                catch (Exception ep2)
                {
                    LogHelper.LogError(" KFWebTest.aspx  SendEmail2 ：" + ep2);
                }
            }
        }


        public static string ConvertToFuid(string QQID)
        {
            try
            {
                //furion 20061115 email登录相关
                if (QQID == null || QQID.Trim().Length < 3)
                    return null;

                //start
                string qqid = QQID.Trim();

          
                string errMsg = "";
                string strSql = "uin=" + qqid;
                string struid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_RELATION, "fuid", out errMsg);

                if (struid == null)
                    return null;
                else
                    return struid;

            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("KF_Service PublicRes").Info("ConvertToFuid error:" + ex);
                return null;
            }
        }




        /// <summary>
        /// 获得数据库的时间 格式：yyyy-MM-dd HH:mm:ss
        /// </summary>
        public static string strNowTimeStander
        {
            get
            {
                //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YWB-V30"));
                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZW"));
                try
                {
                    da.OpenConn();
                    return da.GetOneResult("select DATE_FORMAT(now(),'%Y-%m-%d %H:%i:%s')");
                }
                catch
                {
                    return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                finally
                {
                    da.Dispose();
                }
            }
        }


        private void UpdateUserInfoAttr(string qqid){
            int accountType = 0;
            DataSet ds = new AccountService().GetUserInfo(qqid, accountType, 1, 1);
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                LogHelper.LogInfo(" KFWebTest.aspx  qqid=" + qqid + "未获取到数据");

                Response.Write(" KFWebTest.aspx  UpdateUserInfoAttr qqid=" + qqid + "未获取到数据");
                Response.End();
                return;
            }

            string atttype = "108";
            string oldatttype = classLibrary.setConfig.GetStringStr(ds.Tables[0].Rows[0]["Fatt_id"]);
            string fcre_type = ds.Tables[0].Rows[0]["Fcre_type"].ToString();

            //获得用户帐户信息
            string userType = null;
            string userType_str = null;
            string Msg = null;
            bool exeSign = new AccountService().GetUserType(qqid, accountType, out userType, out userType_str, out Msg);

            if (exeSign == false)
            {
                userType = string.Empty;
                LogHelper.LogInfo(" KFWebTest.aspx  userType=" + userType + "未获取到数据");
            }

            string oldfmemo = classLibrary.setConfig.GetStringStr(ds.Tables[0].Rows[0]["Fmemo"]);
            string fmemo = "1测试1";
            if (modifyAttType(qqid, atttype, oldatttype))
            {
                upgradeLog(qqid, fcre_type, userType, oldatttype, atttype, oldfmemo,fmemo);
            }
        }

        /// <summary>
        /// 修改用户属性类型
        /// </summary>
        /// <param name="qqid"></param>
        /// <param name="atttype"></param>
        /// <param name="oldattid"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        private bool modifyAttType(string qqid, string atttype, string oldattid)
        {
         string   Msg = string.Empty;

            try
            {
                //furion 20061116 email登录修改。
                string strID = ConvertToFuid(qqid);  //先转换成fuid
                string strSql = "uid=" + strID;
                strSql += "&attid=" + atttype;
                strSql += "&modify_time=" + CommQuery.ICEEncode(strNowTimeStander);

                Msg=string.Format("qqid={0},   atttype={1},    oldattid={2},",qqid,atttype,oldattid);

                if (CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERATT, out Msg) < 0)
                {
                    Msg +=  ",修改用户属性类型失败！";
                    LogHelper.LogInfo(" KFWebTest.aspx  " + Msg);
                    return false;
                }
                else{
                 Msg +=",修改用户属性类型成功！";
                LogHelper.LogInfo(" KFWebTest.aspx  " + Msg);
                return true;
                }
            }
            catch (Exception e)
            {
                Msg +=",修改用户属性类型异常！[" + e.ToString().Replace("'", "’") + "]";
                LogHelper.LogError(" KFWebTest.aspx  " + Msg);
                return false;
            }
        }


        private bool upgradeLog(string qqid, string fcre_type, string userType, string oldatttype, string atttype, string oldfmemo, string fmemo)
        {
            string Msg = string.Empty;
            try
            {
                bool operSucc =  new AccountService().AddChangeUserInfoLog(qqid,
                    fcre_type, fcre_type,
                    userType, userType,
                    atttype, oldatttype,
                    fmemo, oldfmemo,
                    Session["uid"].ToString());

                Msg="qqid=" + qqid + ",fcre_type="+fcre_type  + ",userType=" +userType + ",  oldatttype= "+oldatttype+", atttype= "+atttype+" , oldfmemo="+oldfmemo+",fmemo= "+fmemo;
                if (operSucc)
                {
                    Msg += ",修改用户属性类型成功！";
                }
                else {

                    Msg += ",修改用户属性类型失败！";
                }
                LogHelper.LogInfo(" KFWebTest.aspx  " + Msg);
                return operSucc;
            }
            catch (Exception e) {

                Msg += ",修改用户属性类型异常！[" + e.ToString().Replace("'", "’") + "]";
                LogHelper.LogError(" KFWebTest.aspx  " + Msg);
                return false;
            }

        }
        
        #endregion


        private void CheckDBConn(string strkey)
        {
            string dbstr = CommLib.DbConnectionString.Instance.GetConnectionString(strkey.Trim().ToUpper());

            LogHelper.LogInfo(" test.aspx  private void GetDBConnStr  strKey：" + strkey + ",dbstr:" + dbstr);
            using (MySqlAccess da = new MySqlAccess(dbstr))
            {
                da.OpenConn();

                string sql_findUID_2 = "select * from c2c_db_74.t_card_bind_relation_5 where Fcard_id='6230582000010046574' or Fcard_id='762271dfd841546d1a271a6ee8763dbb' ";

                DataSet ds = da.dsGetTableByRange(sql_findUID_2, 0, 10);

                if (ds != null && ds.Tables.Count > 0)
                {
                    this.GridView1.DataSource = ds.Tables[0].DefaultView;
                    this.GridView1.DataBind();
                }
                else
                {
                    LogHelper.LogInfo(" KFWebTest.aspx  CheckDBConn 返回空数据。");

                    Response.Write(" KFWebTest.aspx  CheckDBConn 返回空数据。");
                    Response.End();
                }
            }
        }


        private void bindshimingrzData(string qq)
        {
            string fuin = classLibrary.setConfig.replaceMStr(qq);

            int max = 20;
            int start = 1;

            DataSet ds = new AuthenInfoService().GetUserClassQueryListForThis(fuin, start, max);

            if (ds != null && ds.Tables.Count > 0)
            {
                this.GridView1.DataSource = ds.Tables[0].DefaultView;
                this.GridView1.DataBind();
            }
            else
            {
                throw new LogicException("bindshimingrzData没有找到记录！");
            }

        }


        private void WeChatInfo(string wechatName)
        {
            string retInfo = string.Empty;

            string tempopenid = WeChatHelper.GetAAOpenIdFromWeChatName(wechatName);
            retInfo += tempopenid + "_=_" + WeChatHelper.GetAcctIdFromAAOpenId(tempopenid);

            tempopenid = WeChatHelper.GetHBOpenIdFromWeChatName(wechatName);
            retInfo += "_=_" + tempopenid;
            retInfo += "_=_" + WeChatHelper.GetAcctIdFromOpenId(tempopenid);

            retInfo += "_=_" + WeChatHelper.GetXYKHKOpenIdFromWeChatName(wechatName);

            tempopenid = WeChatHelper.GetFCXGOpenIdFromWeChatName(wechatName,Request.UserHostAddress);
            retInfo += "_=_" + tempopenid + "_=_Request IP:" + Request.UserHostAddress;

            LogHelper.LogInfo(" test.aspx  private void WeChatInfo  retInfo：" + retInfo);

            Response.Write(retInfo);
            Response.End();
        }


    }
}