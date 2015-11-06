using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.CSOMS.DAL.CFTAccount;
using TENCENT.OSS.CFT.KF.Common;
using System.Configuration;
using System.IO;
using TENCENT.OSS.CFT.KF.DataAccess;
using CFT.CSOMS.COMMLIB;
using System.Web;
using CFT.Apollo.Logging;
using CFT.CSOMS.DAL.FreezeModule;
using CFT.CSOMS.DAL.FundModule;

namespace CFT.CSOMS.DAL.UserAppealModule
{
    public class UserAppealData
    {
      /// <summary>
      /// 申诉新库表，单个表记录列表查询
      /// </summary>
      /// <param name="startTime"></param>
      /// <param name="endTime"></param>
      /// <param name="state">状态</param>
      /// <param name="type">申诉类型</param>
      /// <returns></returns>
        public DataSet QueryApealListNewDB(string startTime, string endTime, int state, int type)
        {
            if (string.IsNullOrEmpty(startTime) || string.IsNullOrEmpty(endTime))
                throw new ArgumentNullException("startTime或者endTime");
            
            DateTime sdate = DateTime.Parse(startTime);
            //DateTime edate = DateTime.Parse(startTime);
            //if (sdate.Month != edate.Month)
            //{
            //    string err = "申诉新库表查询单表，开始及结束时间不是同一个月份";
            //    throw new Exception(err);
            //    LogHelper.LogInfo(err);
            //}

            int month = sdate.Month;
            string monthDB = "";
            if (month < 10)
            {
                monthDB = "0" + month;
            }
            else
            {
                monthDB = month.ToString();
            }
            string table = "db_appeal_" + sdate.Year.ToString() + ".t_tenpay_appeal_trans_" + monthDB;
            using (var da = MySQLAccessFactory.GetMySQLAccess("fkdj"))
            {
                da.OpenConn();
                string strWhere = " where 1=1 ";

                if (startTime != null && endTime != "")
                {
                    strWhere += " and FSubmitTime between '" + startTime + "' and '" + endTime + "' ";
                }

                if (type != 99)
                {
                    strWhere += " and FType=" + type + " ";
                }

                if (state != 99)
                {
                    if (state == 10)   //直接申诉成功，即审核成功，且FCheckUser为system
                    {
                        if (type == 8 || type == 19)
                        {
                            strWhere += " and FState='" + state + "'  ";
                        }
                        else
                        {
                            strWhere += " and FState='1' and FCheckUser='system' ";
                        }
                    }
                    else
                    {
                        strWhere += " and FState='" + state + "'  ";
                    }
                }

               string sql = "select Fid,FType,Fuin,FSubmitTime,FState from " + table + " "
                + strWhere ;
               DataSet ds = da.dsGetTotalData(sql);

                return ds;
            }
        }


        /// <summary>
        /// 申诉未完成状态结单处理,更新申诉新库表记录
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="Fcomment"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool FinishAppealNewDB(string fid, string comment,string user)
        {
            string msg = "";
            try
            {
                using (var da = MySQLAccessFactory.GetMySQLAccess("fkdj"))
                {
                    da.OpenConn();
                    string year = fid.Trim().Substring(0, 4);
                    string month = fid.Trim().Substring(4, 2);
                    string table = "db_appeal_" + year + ".t_tenpay_appeal_trans_" + month;
                    string strSql = " select * from " + table + " where Fid='" + fid + "'";
                    DataSet ds = da.dsGetTotalData(strSql);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        int fstate = Int32.Parse(dr["FState"].ToString());
                        int ftype = Int32.Parse(dr["Ftype"].ToString());
                        string pattern = "update {0} set FState='{1}', Fcomment='{2}',FCheckUser='{3}',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='{4}',"
                                + "FModifyTime=Now(),FReCheckTime=now(),FRecheckUser='{5}'"
                                + " where Fid='{6}' and Fstate='{7}'";

                        string resultState = "";//申诉需要修改的结果状态
                        if (fstate == 0 && (ftype == 8 || ftype == 19))//解冻 未处理
                        {
                            resultState = "21";
                        }
                        else if (fstate == 2 && (ftype == 8 || ftype == 19))//解冻 待补充资料
                        {
                            resultState = "21";
                        }
                        else if (fstate == 10 && (ftype == 8 || ftype == 19))//解冻 已补充资料
                        {
                            resultState = "21";
                        }
                        else if (fstate == 11 && ftype == 11)//特殊找密
                        {
                            resultState = "20";
                        }
                        else
                        {
                            msg = "更新原有记录出错,此记录原始状态不正确";
                            LogHelper.LogInfo("UpdateAppealNewDB:" + msg + " fid=" + fid);
                            return false;
                        }

                        strSql = string.Format(@pattern, table, resultState, comment, user, user, user, fid, fstate);

                        if (da.ExecSqlNum(strSql) == 1)
                        {
                            return true;
                        }
                        else
                        {
                            msg = "更新原有记录出错,此记录不存在或原始状态不正确";
                            LogHelper.LogInfo("UpdateAppealNewDB:" + msg + " fid=" + fid);
                            return false;
                        }
                    }
                    else
                    {
                        msg = "查找记录失败.";
                        LogHelper.LogInfo("UpdateAppealNewDB:" + msg + " fid=" + fid);
                        return false;
                    }
                }

            }
            catch (Exception err)
            {
                LogHelper.LogInfo("UpdateAppealNewDB:" + err.Message);
                return false;
            }
        }


        public DataSet QueryFreezeListLog(string uin, int handleFinish)
        {
            FreezeQueryClass cuser2 = new FreezeQueryClass(uin, handleFinish);
            return cuser2.GetResultX(0, 1, "DataSource_ht");
        }
        /// <summary>
        /// 更新特殊申诉（包括解冻、特殊找回支付密码）操作日志
        /// </summary>
        /// <returns></returns>
        public bool UpdateSepcialApealLog(string ffreezeListID, int handleType, string handleUser, string handleResult, string userDesc)
        {
            try
            {
                string tableName = "c2c_fmdb.t_Freeze_Detail";

                using (var da = MySQLAccessFactory.GetMySQLAccess("ht_DB"))
                {
                    da.OpenConn();
                    string sqlCmd = "insert into " + tableName + " (FFreezeListID,FCreateDate,FHandleType,FHandleUser,FHandleResult,FMemo) values ("
                                     + ffreezeListID + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + handleType + ",'"
                                     + handleUser + "','" + handleResult + "','" + userDesc + "')";
                    if (da.ExecSql(sqlCmd))
                        return true;
                    else
                    {
                        LogHelper.LogInfo("UpdateSepcialApealLog  false :FFreezeListID=" + ffreezeListID);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("添加日志失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 使用财付通帐号或者银行卡号获取用户实名认证信息
        /// </summary>
        /// <param name="uin">财付通帐号uin</param>
        /// <param name="userBankID">银行卡号</param>
        /// <param name="bankType">银行类型</param>
        /// <returns></returns>
        public DataSet GetUserAuthenState(string uin, string userBankID, int bankType)
        {
            try
            {
                //1.查询是否通过实名认证
                QueryUserAuthenedStateInfo queryInfo2 = new QueryUserAuthenedStateInfo(uin);
                DataSet ds = queryInfo2.GetResultX_ICE();
                DataRow dr = null;
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    //如果结果为空，表示没通过实名认证
                    QueryUserAuthenStateInfo queryInfo = new QueryUserAuthenStateInfo(uin, userBankID, bankType);
                    ds = queryInfo.GetResultX_ICE();

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        return null;
                    }

                    ds.Tables[0].Columns.Add("queryType", typeof(string));

                    // 从目前来看，该查询返回的结果只有1个。
                    dr = ds.Tables[0].Rows[0];
                    dr["queryType"] = "1";// 表示查的是过程表还是已认证表,1代表查过程表，2代表已认证表

                    return ds;
                }
                dr = ds.Tables[0].Rows[0];
                if (dr["Fattr"].ToString() == "2" && dr["Fstate"].ToString() == "1")
                {
                    //Fattr = 2 & Fstate = 1通过认证
                    ds.Tables[0].Columns.Add("queryType", typeof(string));

                    // 从目前来看，该查询返回的结果只有1个。
                    dr = ds.Tables[0].Rows[0];
                    dr["queryType"] = "2";// 表示查的是过程表还是已认证表,1代表查过程表，2代表已认证表

                }
                else
                {
                    //2.未通过，则查询过程表
                    QueryUserAuthenStateInfo queryInfo = new QueryUserAuthenStateInfo(uin, userBankID, bankType);
                    ds = queryInfo.GetResultX_ICE();

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        return null;
                    }

                    ds.Tables[0].Columns.Add("queryType", typeof(string));

                    // 从目前来看，该查询返回的结果只有1个。
                    dr = ds.Tables[0].Rows[0];
                    dr["queryType"] = "1";// 表示查的是过程表还是已认证表,1代表查过程表，2代表已认证表

                }

                return ds;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 自助申诉查询-根据财付通帐号uin查询用户基本信息
        /// </summary>
        /// <param name="qqid"></param>
        /// <returns></returns>
        public DataSet GetAppealUserInfo(string qqid)
        {
            //已更改 furion V30_FURION核心查询需改动 查询核心走接口.
            string ServerIP = System.Configuration.ConfigurationManager.AppSettings["ICEServerIP"];
            int Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ICEPort"]);
            ICEAccess ice = new ICEAccess(ServerIP, Port);

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZW"));
            try
            {
                string uid = PublicRes.ConvertToFuid(qqid);
                if (uid == null || uid.Trim() == "")
                {
                    throw new LogicException("找不到此用户");
                }

                // TODO: 1客户信息资料外移 
                da.OpenConn();
                ice.OpenConn();
                string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + uid + "&");
                strwhere += ICEAccess.URLEncode("fcurtype=1&");

                string strResp = "";
                DataTable dt = ice.InvokeQuery_GetDataTable(YWSourceType.用户资源, YWCommandCode.查询用户信息, uid, strwhere, out strResp);
                if (dt == null || dt.Rows.Count == 0)
                    throw new LogicException("调用ICE查询T_user无记录" + strResp);

                string Fbalance = dt.Rows[0]["Fbalance"].ToString();
                string Fcon = dt.Rows[0]["Fcon"].ToString();
                string Ftruename = dt.Rows[0]["Ftruename"].ToString();
                string Fcompany_name = dt.Rows[0]["Fcompany_name"].ToString();
                string Fuser_type = dt.Rows[0]["Fuser_type"].ToString();
                string Fqqid = dt.Rows[0]["Fqqid"].ToString();
                string errMsg = "";
                string strSql = "uid=" + uid;
                DataTable dtuserinfo = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_USERINFO, out errMsg);

                if (dtuserinfo == null || dtuserinfo.Rows.Count != 1)
                    throw new LogicException("调用ICE查询t_user_info出错" + errMsg);

                string Femail = dtuserinfo.Rows[0]["Femail"].ToString();
                string Fcre_type = dtuserinfo.Rows[0]["Fcre_type"].ToString();
                string Fcreid = dtuserinfo.Rows[0]["Fcreid"].ToString();
                strSql = "uid=" + uid + "&curtype=1";
                string Fbankid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_BANKUSER, "Fbankid", out errMsg);

                strSql = @"select '" + Fqqid + "' as Fqqid,'" + Ftruename + "' as Ftruename,'" + Fcompany_name + "' as Fcompany_name,'"
                    + Fbalance + "' as Fbalance,'" + Fcon + "' as Fcon,'" + Fuser_type + "' as Fuser_type,'" + Femail
                    + "' as Femail,'" + Fcre_type + "' as Fcre_type,'" + Fcreid + "' as Fcreid,'" + Fbankid + "' as Fbankid";

                return da.dsGetTotalData(strSql);
            }
            finally
            {
                da.Dispose();
                ice.Dispose();
            }
      
        }


        /// <summary>
        /// 自助申诉查询函数 ftype == 1 || ftype == 5 || ftype == 6 || ftype == 99
        /// </summary>
        /// <param name="fuin">财付通账号</param>
        /// <param name="u_BeginTime">开始时间</param>
        /// <param name="u_EndTime">结束时间</param>
        /// <param name="fstate">申诉状态</param>
        /// <param name="ftype">申诉类型</param>
        /// <param name="QQType">帐号类型</param>
        /// <param name="dotype">处理类别</param>
        /// <param name="iPageStart">起始位 </param>
        /// <param name="iPageMax">记录数</param>
        /// <param name="SortType">排序方式</param>
        /// <returns></returns>
        public DataSet GetCFTUserAppealListNew(string fuin, string u_BeginTime, string u_EndTime, int fstate, int ftype, string QQType, string dotype, int SortType)
        {
            try
            {
                CFTUserAppealClass cuser = new CFTUserAppealClass(fuin, u_BeginTime, u_EndTime, fstate, ftype, QQType, dotype, SortType);
                DataSet ds = cuser.GetResultX("CFTB_DB");

                //为了自助申诉查询功能添加
                if (u_EndTime == "" && u_BeginTime == "")
                {
                    u_EndTime = DateTime.Now.ToString();
                    u_BeginTime = "2014-01-01 00:00:00";
                }

                DateTime date = DateTime.Parse(u_BeginTime);
                int yearEnd = DateTime.Parse(u_EndTime).Year;
                int monEnd = DateTime.Parse(u_EndTime).Month;
                List<string> listdb = new List<string>();
                List<string> listtb = new List<string>();
                if (yearEnd >= 2014)//才查分库表
                {
                    while (date.Year < 2014)
                    {
                        date = date.AddMonths(1);
                    }
                    while (!((date.Year == yearEnd && date.Month > monEnd) || (date.Year > yearEnd)))
                    {
                        listdb.Add(date.Year.ToString());
                        listtb.Add(date.Month.ToString());
                        date = date.AddMonths(1);
                    }
                }

                DataSet dsFenResult = new DataSet();
                if (listdb == null || listdb.Count == 0)
                    dsFenResult = null;
                else
                {
                    int index = 0;//计数添加的有数据的数据表
                    for (int i = 0; i < listdb.Count; i++)
                    {
                        string db = listdb[i];
                        string tb = listtb[i];
                        CFTUserAppealClass cuser2 = new CFTUserAppealClass(fuin, u_BeginTime, u_EndTime, fstate, ftype, QQType, dotype, SortType, true, db, tb);//分库分表的查询
                        DataSet dsfen = cuser2.GetResultX("CFTNEW_DB");

                        if (dsfen != null && dsfen.Tables.Count > 0 && dsfen.Tables[0].Rows.Count > 0)
                        {
                            if (index == 0)
                            {
                                dsFenResult.Tables.Add(dsfen.Tables[0].Copy());
                                index++;
                            }
                            else
                            {
                                foreach (DataRow dr in dsfen.Tables[0].Rows)
                                {
                                    dsFenResult.Tables[0].ImportRow(dr);//将记录加入到一个表里
                                }
                            }
                        }
                    }
                }
                //将旧表与分库表数据写入一个表
                DataSet dsAll = PublicRes.NewMethod(ds, dsFenResult);
                return dsAll;

            }
            catch (Exception ex)
            {
                throw new Exception("自助申诉查询函数异常！" + ex.Message);
            }
        }

        /// <summary>
        /// 查询风控冻结处理日志
        /// </summary>
        public DataSet GetFreezeDiary(string fid, string ffreezeID, string handleType, string handleUser,
            string handleResult, string memo, string strBeginDate, string strEndDate, int startIndex, int maxPage)
        {
            string tableName = "c2c_fmdb.t_Freeze_Detail";

            string cmd = "select * from " + tableName + " where (1=1) ";
            if (fid.Trim() != "")
                cmd += " and FID=" + fid;
            if (ffreezeID.Trim() != "")
                cmd += " and FFreezeListID=" + ffreezeID;
            if (handleType.Trim() != "")
                cmd += " and FHandleType in(" + handleType + ")";
            if (handleUser.Trim() != "")
                cmd += " and FHandleUser like '%" + handleUser + "%'";
            if (handleResult.Trim() != "")
                cmd += " and FHandleResult like '%" + handleResult + "%'";
            if (strBeginDate.Trim() != "")
                cmd += " and FCreateDate >='" + strBeginDate.Trim() + "'";
            if (strEndDate.Trim() != "")
                cmd += " and FCreateDate<='" + strEndDate.Trim() + "'";
            
            cmd += " order by FCreateDate DESC ";
            DataSet ds = new DataSet();
            MySqlAccess da = null;
            try
            {
                da = MySQLAccessFactory.GetMySQLAccess("DataSource_ht");
                da.OpenConn();
                if (startIndex != -1)
                    ds = da.dsGetTableByRange(cmd, startIndex, maxPage);
                else
                    ds = da.dsGetTotalData(cmd);

                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("GetFreezeDiary: 日志查询失败: " + ex.Message);
                return null;
            }
            finally
            {
                da.Dispose();
            }
        }

        /// <summary>
        /// 创建风控冻结处理日志NEW
        /// </summary>
        public bool CreateFreezeDiary(string ffreezeListID, int handleType, string handleUser, string handleResult
        , string memo, string uin, string userPhone, string submitDate, int bt, string userDesc, string zdyBt1, string zdyBt2, string zdyBt3
        , string zdyBt4, string zdyCont1, string zdyCont2, string zdyCont3, string zdyCont4)
        {

            string tableName = "c2c_fmdb.t_Freeze_Detail";

            DateTime date = DateTime.Parse(submitDate);
            int i_m = date.Month;
            string s_m = "";
            if (i_m < 10)
            {
                s_m = "0" + i_m;
            }
            else
            {
                s_m = i_m.ToString();
            }
            string table = "db_appeal_" + date.Year.ToString() + ".t_tenpay_appeal_trans_" + s_m;
            CFTUserAppealClass cuser = new CFTUserAppealClass(ffreezeListID, table);
            DataSet ds = cuser.GetResultX(0, 1, "fkdj");

            // 记录结单状态和结单人员
            int srcHandleType = 0;
            string srcHandleUser = "";
            int reqType = 8; //处理8,19类型的记录

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                //记录不存在，再查询一下上个月记录 yinhuang
                DateTime d2 = date.AddMonths(-1);
                i_m = d2.Month;
                if (i_m < 10)
                {
                    s_m = "0" + i_m;
                }
                else
                {
                    s_m = i_m.ToString();
                }
                table = "db_appeal_" + d2.Year.ToString() + ".t_tenpay_appeal_trans_" + s_m;
                CFTUserAppealClass cuser2 = new CFTUserAppealClass(ffreezeListID, table);
                ds = cuser2.GetResultX(0, 1, "fkdj");
            }

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                throw new Exception("记录不存在" + ffreezeListID);
            }
            else
            {
                srcHandleType = int.Parse(ds.Tables[0].Rows[0]["FState"].ToString());
                srcHandleUser = ds.Tables[0].Rows[0]["FCheckUser"].ToString();
                reqType = int.Parse(ds.Tables[0].Rows[0]["FType"].ToString());

                if (reqType == 8 || reqType == 19)
                {
                    // 结单的日志只允许补充处理结果
                    if (srcHandleType == 1 || srcHandleType == 2)
                    {
                        if (handleType != 100)
                        {
                            return false;
                        }
                    }

                    // 作废的日志就不许再操作了
                    if (srcHandleType == 7)
                        return false;
                }
                else if (reqType == 11)//特殊找回密码
                {
                    if (!(srcHandleType == 0 || srcHandleType == 12))//除了未处理、已补充资料状态外，不能补充资料操作
                        return false;
                }
            }

            if (handleResult.Trim() != "")
            {
                handleResult = PublicRes.replaceMStr(handleResult);
            }

            if (handleType != 100)
            {
                MySqlAccess da_2 = null;
                try
                {
                    da_2 = MySQLAccessFactory.GetMySQLAccess("FKDJ");
                    da_2.OpenConn();

                    if (reqType == 8 || reqType == 19)
                        memo = "风控冻结." + memo;
                    else if (reqType == 11)
                        memo = "特殊找回密码." + memo;

                    string sqlCmd_updateAppeal = "update " + table + " set FState=" + handleType
                        + ",Fcomment='" + memo + "', FCheckUser='" + handleUser + "',FCheckTime=Now(),"
                        + " FPickTime=now(),FPickUser='" + handleUser + "',FStandBy1=" + bt
                        + " ,FReCheckTime=now(),FRecheckUser='" + handleUser + "',FCheckInfo='" + handleResult + "',Fsup_desc1='" + zdyBt1
                        + "',Fsup_desc2='" + zdyBt2 + "',Fsup_desc3='" + zdyBt3 + "',Fsup_desc4='" + zdyBt4 + "',Fsup_tips1='" + zdyCont1
                        + "',Fsup_tips2='" + zdyCont2 + "',Fsup_tips3='" + zdyCont3 + "',Fsup_tips4='" + zdyCont4 + "' "
                        + " where Fid='" + ffreezeListID + "'";

                    if (!da_2.ExecSql(sqlCmd_updateAppeal))
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogInfo("CreateFreezeDiary: 更新失败" + ex.Message);
                    return false;
                }
                finally
                {
                    da_2.Dispose();
                }
            }

            MySqlAccess da = null;
            string sqlCmd = "insert into " + tableName + " (FFreezeListID,FCreateDate,FHandleType,FHandleUser,FHandleResult,FMemo) values ("
                + ffreezeListID + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + handleType + ",'"
                + handleUser + "','" + handleResult + "','" + userDesc + "')";

            try
            {
                da = MySQLAccessFactory.GetMySQLAccess("DataSource_ht");
                da.OpenConn();
                if (da.ExecSql(sqlCmd))
                {
                    // 成功更新数据库，则检查是否结单操作并发送邮件
                    if (reqType == 8 || reqType == 19)
                    {
                        if (handleType == 1)
                        {
                            //结单解冻
                            if (reqType == 19)
                            {
                                //发微信解冻消息
                                if (uin.IndexOf("@wx.tenpay.com") > 0)
                                {
                                    string reqsource = "bus_kf_unfreeze";
                                    string accid = uin.Substring(0, uin.IndexOf("@wx.tenpay.com"));
                                    string templateid = "DeNkYEfSBW7mVQET6QHwnilGWvG8cLssLSyRH0CSDk0";
                                    string cont1 = "你的微信支付账户已排除了安全风险并由保护模式切换至正常模式。";
                                    string cont2 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                    string cont3 = "请点击详情查看微信支付安全保障介绍";
                                    string msgtype = "unfreeze";

                                    new FreezeData().SendWechatMsg(reqsource, accid, templateid, cont1, cont2, cont3, msgtype);
                                }
                            }
                            else
                            {
                                string str_params = "http://action.tenpay.com/cuifei/2014/fengkong/unfreeze_suc.shtml?clientuin=$UIN$&clientkey=$KEY$";
                                str_params = "url=" + System.Web.HttpUtility.UrlEncode(str_params, System.Text.Encoding.GetEncoding("gb2312"));
                                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsgQQTips(uin, "2236", str_params);
                                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMessage(userPhone, "2236", "userid=" + uin);
                            }
                        }
                        else if (handleType == 2)
                        {
                            //补充资料
                            if (reqType == 19)
                            {
                                //发微信补填资料消息
                                if (uin.IndexOf("@wx.tenpay.com") > 0)
                                {
                                    string reqsource = "bus_kf_supple";
                                    string accid = uin.Substring(0, uin.IndexOf("@wx.tenpay.com"));
                                    string templateid = "p7DifLpETQvbtDPtRPDSI6x4ufUtnjZXcb6LpVIbZ70";
                                    string cont1 = "你的微信支付账户仍处于保护模式，请点击详情补充恢复微信支付账户所需资料。";
                                    string cont2 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                    string cont3 = "";
                                    string msgtype = "supple";

                                    new FreezeData().SendWechatMsg(reqsource, accid, templateid, cont1, cont2, cont3, msgtype);
                                }
                            }
                            else
                            {
                                string str_params = "http://action.tenpay.com/cuifei/2014/fengkong/unfreeze_fail.shtml?clientuin=$UIN$&clientkey=$KEY$";
                                str_params = "url=" + System.Web.HttpUtility.UrlEncode(str_params, System.Text.Encoding.GetEncoding("gb2312"));
                                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsgQQTips(uin, "2237", str_params);
                                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMessage(userPhone, "2237", "user=" + uin);
                            }
                        }
                    }
                    else if (reqType == 11)//特殊找回密码 发tips和短信 模板位申请，需更改下面代码
                    {
                        string str_params = "www.tenpay.com/v2/cs/";
                        str_params = "url=" + System.Web.HttpUtility.UrlEncode(str_params, System.Text.Encoding.GetEncoding("gb2312"));
                        //uin = "466678748";userPhone = "18718489269";
                        TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsgQQTips(uin, "2429", str_params);
                        TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMessage(userPhone, "2429", str_params);
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string msg = "添加日志失败" + ex.Message;
                LogHelper.LogInfo("CreateFreezeDiary", msg);
                throw new Exception("添加日志失败：" + ex.Message);
            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
            }

            return true;
        }

        /// <summary>
        /// 获取特殊申诉列表
        /// </summary>
        /// <param name="uin"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="ftype"></param>
        /// <param name="iStatue"></param>
        /// <param name="szListID"></param>
        /// <param name="szFreezeUser"></param>
        /// <param name="szFreezeReason"></param>
        /// <param name="iPageStart"></param>
        /// <param name="iPageMax"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public DataSet GetSpecialAppealList(string uin, string beginDate, string endDate, int ftype, int iStatue,
            string szListID, string szFreezeUser, string szFreezeReason, int iPageStart, int iPageMax, string orderType)
        {
            try
            {
                #region 查询

                DataSet ds = null;
                int recordCount = 0;
                DateTime sDate = DateTime.Parse(beginDate);
                DateTime eDate = DateTime.Parse(endDate);
                int iMonth = 0;
                string sMonth = "";
                if (sDate.Month != eDate.Month)
                {
                    //月份不一样，则需要查询两块表的数据
                    DateTime qDate = DateTime.Now.AddDays(1);   //结束日期暂为当前日期后一天，因为查询使用的是 between
                    iMonth = sDate.Month;
                    sMonth = "";

                    if (iMonth < 10)
                        sMonth = "0" + iMonth;
                    else
                        sMonth = iMonth.ToString();

                    string table = "db_appeal_" + sDate.Year.ToString() + ".t_tenpay_appeal_trans_" + sMonth;
                    CFTUserAppealClass cuser = new CFTUserAppealClass(uin, beginDate, qDate.ToString("yyyy-MM-dd"), iStatue, ftype, "", szFreezeUser, szListID, szFreezeReason,
                        orderType, table);
                    ds = cuser.GetResultX(iPageStart, iPageMax, "fkdj");
                    int count1 = cuser.GetCount("fkdj");


                    DateTime qsDate = eDate.AddDays(1 - eDate.Day); //当月第一天
                    iMonth = eDate.Month;
                    sMonth = "";

                    if (iMonth < 10)
                        sMonth = "0" + iMonth;
                    else
                        sMonth = iMonth.ToString();

                    table = "db_appeal_" + sDate.Year.ToString() + ".t_tenpay_appeal_trans_" + sMonth;
                    CFTUserAppealClass cuser2 = new CFTUserAppealClass(uin, qsDate.ToString("yyyy-MM-dd"), endDate, iStatue, ftype, "", szFreezeUser, szListID, szFreezeReason,
                        orderType, table);
                    DataSet ds2 = cuser2.GetResultX(iPageStart, iPageMax, "fkdj");
                    int count2 = cuser2.GetCount("fkdj");
                    recordCount = count1 + count2;

                    if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    {
                        //将两张表的数据合并到一张表
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds2.Tables[0].Rows)
                            {
                                ds.Tables[0].ImportRow(dr);
                            }
                        }
                        else
                        {
                            ds = new DataSet();
                            ds.Tables.Add(ds2.Tables[0].Copy());
                        }
                    }
                }
                else
                {
                    //同一个月份
                    iMonth = sDate.Month;
                    sMonth = "";
                    if (iMonth < 10)
                        sMonth = "0" + iMonth;
                    else
                        sMonth = iMonth.ToString();

                    string table = "db_appeal_" + sDate.Year.ToString() + ".t_tenpay_appeal_trans_" + sMonth;
                    CFTUserAppealClass cuser = new CFTUserAppealClass(uin, beginDate, endDate, iStatue, ftype, "", szFreezeUser, szListID, szFreezeReason,
                        orderType, table);
                    ds = cuser.GetResultX(iPageStart, iPageMax, "fkdj");
                    recordCount = cuser.GetCount("fkdj");
                }
                #endregion

                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                {
                    return null;
                }

                ds.Tables[0].Columns.Add("FreezeReason", typeof(string));
                ds.Tables[0].Columns.Add("FreezeUser", typeof(string));
                ds.Tables[0].Columns.Add("isFreezeListHas", typeof(string));
                ds.Tables[0].Columns.Add("Fuincolor", typeof(string));

                long Appeal_FreezeMoney = long.Parse(System.Configuration.ConfigurationManager.AppSettings["Appeal_FreezeMoney"]);

                string ServerIP = System.Configuration.ConfigurationManager.AppSettings["ICEServerIP"];
                int Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ICEPort"]);
                ICEAccess ice = new ICEAccess(ServerIP, Port);
                try
                {
                    ice.OpenConn();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        try
                        {
                            #region 查询冻结日志信息
                            if (ftype == 8 || ftype == 19)
                            {
                                FreezeQueryClass cuser2 = new FreezeQueryClass(dr["Fuin"].ToString(), 1);

                                DataSet ds2 = cuser2.GetResultX(0, 1, "HT");

                                if (ds2 != null && ds2.Tables.Count != 0 && ds2.Tables[0].Rows.Count != 0)
                                {
                                    dr["FreezeReason"] = ds2.Tables[0].Rows[0]["FFreezeReason"].ToString();
                                    dr["FreezeUser"] = ds2.Tables[0].Rows[0]["FHandleUserID"].ToString();
                                    dr["isFreezeListHas"] = "1";
                                }
                                else
                                {
                                    dr["isFreezeListHas"] = "0";
                                }
                            }
                            #endregion

                            #region 添加大额标记
                            dr["Fuincolor"] = "";
                            string fuid = PublicRes.ConvertToFuid(dr["Fuin"].ToString());

                            string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
                            strwhere += ICEAccess.URLEncode("fcurtype=1&");

                            string strResp = "";

                            DataTable dtuser = ice.InvokeQuery_GetDataTable(YWSourceType.用户资源, YWCommandCode.查询用户信息, fuid, strwhere, out strResp);

                            if (dtuser == null || dtuser.Rows.Count == 0)
                            {
                                continue;
                            }

                            long lbalance = long.Parse(dtuser.Rows[0]["fbalance"].ToString());

                            if (lbalance >= Appeal_FreezeMoney)
                            {
                                dr["Fuincolor"] = "BIGMONEY";
                            }
                            #endregion
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    ice.CloseConn();
                }
                finally
                {
                    ice.Dispose();
                }
                return ds;

            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("GetSpecialAppealList: 特殊申诉列表查询失败: ", ex.Message);
                return null;
            }
        }

        public DataSet GetSpecialAppealDetail(string fid)
        {
            try
            {
                string year = fid.Substring(0, 4);
                string month = fid.Substring(4, 2);
                string sMonth = "";

                string table = "db_appeal_" + year + ".t_tenpay_appeal_trans_" + month;
                CFTUserAppealClass cuser = new CFTUserAppealClass(fid, table);
                DataSet ds = cuser.GetResultX(0, 1, "fkdj");

                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                {
                    //查询另外一张表
                    int month2 = int.Parse(month) - 1;
                    if (month2 < 10)
                        sMonth = "0" + month2;
                    else
                        sMonth = month2.ToString();

                    string table2 = "db_appeal_" + year + ".t_tenpay_appeal_trans_" + sMonth;
                    CFTUserAppealClass cuser2 = new CFTUserAppealClass(fid, table2);
                    ds = cuser2.GetResultX(0, 1, "fkdj");
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int ftype = int.Parse(ds.Tables[0].Rows[0]["Ftype"].ToString());
                    #region 字段处理

                    ds.Tables[0].Columns.Add("isFreezeListHas", typeof(string));
                    ds.Tables[0].Columns.Add("FreezeReason", typeof(string));//冻结原因
                    ds.Tables[0].Columns.Add("detail_score", typeof(string)); //得分明细
                    ds.Tables[0].Columns.Add("risk_result", typeof(string)); //风控标记

                    DataRow dr = ds.Tables[0].Rows[0];
                    string risk_result = CFTUserAppealClass.getCgiString(dr["FRiskState"].ToString());
                    if (risk_result == "0")
                        dr["risk_result"] = "";
                    else if (risk_result == "1")
                        dr["risk_result"] = "风控异常单无需人工回访用户";
                    else if (risk_result == "2")
                        dr["risk_result"] = "风控异常单需人工回访用户";
                    else
                        dr["risk_result"] = risk_result;

                    dr["detail_score"] = CFTUserAppealClass.getCgiString(dr["FDetailScore"].ToString());

                    if (dr["detail_score"].ToString() != "")
                    {
                        try
                        {
                            string detail_score = System.Web.HttpUtility.UrlDecode(dr["detail_score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));

                            if (detail_score.IndexOf("PwdProtection") > -1)
                            {
                                detail_score = detail_score.Replace("PwdProtection", "密保校验得分");
                            }
                            if (detail_score.IndexOf("CertifiedId") > -1)
                            {
                                detail_score = detail_score.Replace("CertifiedId", "证件号校验得分");
                            }
                            if (detail_score.IndexOf("bind_email") > -1)
                            {
                                detail_score = detail_score.Replace("bind_email", "绑定邮箱校验得分");
                            }
                            if (detail_score.IndexOf("bind_mobile") > -1)
                            {
                                detail_score = detail_score.Replace("bind_mobile", "绑定手机校验得分");
                            }
                            if (detail_score.IndexOf("QQReceipt") > -1)
                            {
                                detail_score = detail_score.Replace("QQReceipt", "QQ申诉回执号得分");
                            }
                            if (detail_score.IndexOf("CertifiedBankCard") > -1)
                            {
                                detail_score = detail_score.Replace("CertifiedBankCard", "实名认证银行卡号校验得分");
                            }
                            if (detail_score.IndexOf("CreditCardPayHist") > -1)
                            {
                                detail_score = detail_score.Replace("CreditCardPayHist", "信用卡还款信息校验得分");
                            }
                            if (detail_score.IndexOf("WithdrawHist") > -1) //andrew 20110419
                            {
                                detail_score = detail_score.Replace("WithdrawHist", "提现记录得分");
                            }
                            if (detail_score.IndexOf("MBVerify") > -1)
                            {
                                detail_score = detail_score.Replace("MBVerify", "安平密保验证得分");
                            }
                            if (detail_score.IndexOf("MBQuery") > -1)
                            {
                                detail_score = detail_score.Replace("MBQuery", "通过安全中心密保得分");
                            }
                            if (detail_score.IndexOf("BindMobile") > -1)
                            {
                                detail_score = detail_score.Replace("BindMobile", "绑定的手机号码得分");
                            }
                            if (detail_score.IndexOf("Mobile") > -1)
                            {
                                detail_score = detail_score.Replace("Mobile", "手机得分");
                            }
                            if (detail_score.IndexOf("Email_QQ") > -1)
                            {
                                detail_score = detail_score.Replace("Email_QQ", "绑定QQ邮箱得分");
                            }
                            if (detail_score.IndexOf("Mobile_New") > -1)
                            {
                                detail_score = detail_score.Replace("Mobile_New", "未注册手机得分");
                            }
                            if (detail_score.IndexOf("QQReceipt_6") > -1)
                            {
                                detail_score = detail_score.Replace("QQReceipt_6", "简化注册用户QQ申诉回执单号得分");
                            }
                            dr["detail_score"] = detail_score;

                        }
                        catch (Exception ex)
                        {
                            string msg = "获取得分明细失败" + ex.Message;
                            LogHelper.LogInfo("DAL-GetSpecialAppealDetail", msg);
                        }
                    }
                    #endregion
                    if (ftype == 8 || ftype == 19)
                    {
                        FreezeQueryClass cuser2 = new FreezeQueryClass(ds.Tables[0].Rows[0]["Fuin"].ToString(), 1);
                        DataSet ds2 = cuser2.GetResultX(0, 1, "HT");

                        if (ds2 != null && ds2.Tables.Count != 0 && ds2.Tables[0].Rows.Count != 0)
                        {
                            ds.Tables[0].Rows[0]["isFreezeListHas"] = "1";
                            ds.Tables[0].Rows[0]["FreezeReason"] = ds2.Tables[0].Rows[0]["FFreezeReason"].ToString();
                        }
                        else
                        {
                            ds.Tables[0].Rows[0]["isFreezeListHas"] = "0";
                        }
                    }
                    else if (ftype == 11)//特殊找回支付密码
                    {
                        ds.Tables[0].Rows[0]["isFreezeListHas"] = "";
                        ds.Tables[0].Rows[0]["FreezeReason"] = "";
                    }
                    //else
                    //{
                    //    throw new Exception("只处理解冻申诉、特殊找回支付密码，记录类型错误：" + ftype);
                    //}
                }

                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("GetSpecialAppealDetail： 查询记录为空： ", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 自助申诉查询函数
        /// </summary>
        /// <param name="fuin">财付通账号</param>
        /// <param name="u_BeginTime">开始时间</param>
        /// <param name="u_EndTime">结束时间</param>
        /// <param name="fstate">申诉状态</param>
        /// <param name="ftype">申诉类型</param>
        /// <param name="QQType">帐号类型</param>
        /// <param name="dotype">处理类别</param>
        /// <param name="iPageStart">起始位 </param>
        /// <param name="iPageMax">记录数</param>
        /// <param name="SortType">排序方式</param>
        /// <returns></returns>
        public DataSet GetCFTUserAppealList(string fuin, string u_BeginTime, string u_EndTime, int fstate, int ftype, string QQType, string dotype, int iPageStart, int iPageMax, int SortType)
        {
            try
            {
                CFTUserAppealClass cuser = new CFTUserAppealClass(fuin, u_BeginTime, u_EndTime, fstate, ftype, QQType, dotype, SortType);
                DataSet ds = cuser.GetResultX(iPageStart + 1, iPageMax, "CFTB_DB");

                //   long Appeal_BigMoney = long.Parse(System.Configuration.ConfigurationManager.AppSettings["Appeal_BigMoney"]);

                //   ds=GetCFTUserAppealListFunction(ds);

                return ds;

            }
            catch (Exception ex)
            {
                throw new LogicException("自助申诉查询函数异常！" + ex.Message);
            }
        }

        /// <summary>
        /// 自助申诉查询三种类型( ftype == 1 || ftype == 5 || ftype == 6 || ftype == 99)，处理用户信息
        /// </summary>
        /// <param name="dsAll"></param>
        /// <returns></returns>
        public DataSet GetCFTUserAppealListFunction(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                CFTUserAppealClass.HandleParameter(ds, false);
                long Appeal_BigMoney = long.Parse(System.Configuration.ConfigurationManager.AppSettings["Appeal_BigMoney"]);
                ds.Tables[0].Columns.Add("Fuincolor", typeof(String));
                ds.Tables[0].Columns.Add("balance", typeof(String));//金额 

                ICEAccess ice = ICEAccessFactory.GetICEAccess("ICEConnectionString");
                try
                {
                    ice.OpenConn();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        try
                        {
                            dr["Fuincolor"] = "";
                            string fuid = AccountData.ConvertToFuid(dr["Fuin"].ToString());

                            string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
                            strwhere += ICEAccess.URLEncode("fcurtype=1&");

                            string strResp = "";

                            DataTable dtuser = ice.InvokeQuery_GetDataTable(YWSourceType.用户资源, YWCommandCode.查询用户信息, fuid, strwhere, out strResp);

                            if (dtuser == null || dtuser.Rows.Count == 0)
                            {
                                dr["balance"] = "0";
                                continue;
                            }

                            long lbalance = long.Parse(dtuser.Rows[0]["fbalance"].ToString());
                            dr["balance"] = lbalance.ToString();

                            if (lbalance >= Appeal_BigMoney)
                            {
                                dr["Fuincolor"] = "BIGMONEY";
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    ice.CloseConn();
                }
                finally
                {
                    ice.Dispose();
                }
            }
            return ds;
        }

        /// <summary>
        /// 查询自助申诉记录详细信息  老库表
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        public DataSet GetCFTUserAppealDetail(int fid)
        {

            //   MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                CFTUserAppealClass cuser = new CFTUserAppealClass(fid);
                DataSet ds = cuser.GetResultX(0, 1, "CFT_DB");

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int ftype = int.Parse(ds.Tables[0].Rows[0]["Ftype"].ToString());
                    if (ftype == 8)
                    {
                        CFTUserAppealClass.HandleParameter_ForControledFreeze(ds, true);

                        FreezeQueryClass cuser2 = new FreezeQueryClass(ds.Tables[0].Rows[0]["Fuin"].ToString(), 1);

                        DataSet ds2 = cuser2.GetResultX(0, 1, "DataSource_ht");//根据kf_service来修改的

                        ds.Tables[0].Columns.Add("isFreezeListHas", typeof(string));

                        if (ds2 != null && ds2.Tables.Count != 0 && ds2.Tables[0].Rows.Count != 0)
                        {
                            ds.Tables[0].Rows[0]["isFreezeListHas"] = "1";
                        }
                        else
                        {
                            ds.Tables[0].Rows[0]["isFreezeListHas"] = "0";
                            //throw new  Exception("单号" + dr["fid"] + "风控冻结单的帐号在冻结单表中不存在！");
                        }
                    }
                    else
                    {
                        CFTUserAppealClass.HandleParameter(ds, true);
                    }
                    //CFTUserAppealClass.HandleParameter(ds,true);

                    //如果类型是5:完整注册用户更换关联手机 ,增加上外呼结果. //IVR外呼专用furion
                    ds.Tables[0].Columns.Add("FIVRResult");
                    ds.Tables[0].Rows[0]["FIVRResult"] = "";

                    if (ftype == 5)
                    {
                        DataTable dtivr = new DataTable();
                        using (var da = MySQLAccessFactory.GetMySQLAccess("CFT_DB"))
                        {
                            da.OpenConn();
                            string cftDB = ConfigurationManager.AppSettings["DB_CFT"];
                            string strSql = "select * from  " + cftDB + ".t_tenpay_appeal_IVR where Fappealid=" + fid;
                            dtivr = da.GetTable(strSql);
                        }

                        if (dtivr != null && dtivr.Rows.Count == 1)
                        {
                            string ivrresult = "呼叫次数:" + dtivr.Rows[0]["Fcallnum"].ToString();
                            string tmp = dtivr.Rows[0]["Fcallresult"].ToString();
                            if (tmp == "1")
                                ivrresult += "$$呼叫状态:用户主动回复1同意";
                            else if (tmp == "2")
                                ivrresult += "$$呼叫状态:用户主动回复2拒绝";
                            else if (tmp == "3")
                                ivrresult += "$$呼叫状态:用户主动回复其它值.";
                            else if (tmp == "4")
                                ivrresult += "$$呼叫状态:用户不接听电话";
                            else if (tmp == "5")
                                ivrresult += "$$呼叫状态:用户主动挂机";
                            else if (tmp == "6")
                                ivrresult += "$$呼叫状态:呼叫无法建立(空号,关机)";
                            else if (tmp == "7")
                                ivrresult += "$$呼叫状态:Ivr主动挂机(超过1分钟用户没有按键)";

                            ivrresult += "$$呼叫结果:" + dtivr.Rows[0]["Fcallmemo"].ToString();
                            ivrresult += "$$原绑定手机号码:" + dtivr.Rows[0]["Fmobile"].ToString();

                            ds.Tables[0].Rows[0]["FIVRResult"] = ivrresult;
                        }
                    }
                }

                return ds;
            }
            catch (LogicException err)
            {
                throw;
            }
            catch (Exception err)
            {
                throw new LogicException("自助申诉记录详细老库表查询异常！");
            }
        }

        /// <summary>
        ///  查询自助申诉记录详细信息  新库表
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="db"></param>
        /// <param name="tb"></param>
        /// <returns></returns>
        public DataSet GetCFTUserAppealDetailByDBTB(string fid, string db, string tb)
        {
           // MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                CFTUserAppealClass cuser = new CFTUserAppealClass(fid, db, tb, "ByDBTB");
                DataSet ds = cuser.GetResultX(0, 1, "CFTNEW_DB");

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int ftype = int.Parse(ds.Tables[0].Rows[0]["Ftype"].ToString());
                   
                    HandleParameterByDBTB(ds, true);//分库表的处理方法，处理结果后的结果与旧表一样
                  
                    //如果类型是5:完整注册用户更换关联手机 ,增加上外呼结果. //IVR外呼专用furion
                    ds.Tables[0].Columns.Add("FIVRResult");
                    ds.Tables[0].Rows[0]["FIVRResult"] = "";

                    if (ftype == 5)
                    {
                        DataTable dtivr = new DataTable();
                        using (var da = MySQLAccessFactory.GetMySQLAccess("CFT_DB"))
                        {
                            da.OpenConn();

                            string dateTime = ds.Tables[0].Rows[0]["FSubmitTime"].ToString();
                            int year = DateTime.Parse(dateTime).Year;
                            int month = DateTime.Parse(dateTime).Month;
                            string dbIVR = "db_apeal_IVR_" + year;
                            string tbIVR = "";
                            if (month < 10)
                                tbIVR = "t_tenpay_appeal_IVR_0" + month;
                            else
                                tbIVR = "t_tenpay_appeal_IVR_" + month;

                            string strSql = "select * from  " + dbIVR + "." + tbIVR + "  where Fappealid='" + fid + "'";//外呼库表需重构
                            dtivr = da.GetTable(strSql);
                        }
                        if (dtivr != null && dtivr.Rows.Count == 1)
                        {
                            string ivrresult = "呼叫次数:" + dtivr.Rows[0]["Fcallnum"].ToString();
                            string tmp = dtivr.Rows[0]["Fcallresult"].ToString();
                            if (tmp == "1")
                                ivrresult += "$$呼叫状态:用户主动回复1同意";
                            else if (tmp == "2")
                                ivrresult += "$$呼叫状态:用户主动回复2拒绝";
                            else if (tmp == "3")
                                ivrresult += "$$呼叫状态:用户主动回复其它值.";
                            else if (tmp == "4")
                                ivrresult += "$$呼叫状态:用户不接听电话";
                            else if (tmp == "5")
                                ivrresult += "$$呼叫状态:用户主动挂机";
                            else if (tmp == "6")
                                ivrresult += "$$呼叫状态:呼叫无法建立(空号,关机)";
                            else if (tmp == "7")
                                ivrresult += "$$呼叫状态:Ivr主动挂机(超过1分钟用户没有按键)";

                            ivrresult += "$$呼叫结果:" + dtivr.Rows[0]["Fcallmemo"].ToString();
                            ivrresult += "$$原绑定手机号码:" + dtivr.Rows[0]["Fmobile"].ToString();

                            ds.Tables[0].Rows[0]["FIVRResult"] = ivrresult;
                        }
                    }
                }

                return ds;
            }
            catch (LogicException err)
            {
                throw;
            }
            catch (Exception err)
            {
                throw new LogicException("自助申诉记录详细新库表查询异常！");
            }
        }

        /// <summary>
        /// 自助申诉删除
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="Fcomment"></param>
        /// <param name="user"></param>
        /// <param name="userIP"></param>
        /// <returns></returns>
        public bool DelAppeal(string fid, string Fcomment, string user, string userIP, string appeal_db, string appeal_tb)
        {
            string msg = "";
            if (Fcomment != null && Fcomment.Trim() != "")
            {
                Fcomment = PublicRes.replaceMStr(Fcomment);
            }
            var da = MySQLAccessFactory.GetMySQLAccess("CFT_DB");
            try
            {
                string db = "db_appeal";
                try
                {
                    db = System.Configuration.ConfigurationManager.AppSettings["DB_CFT"].ToString();
                }
                catch
                {
                    db = "db_appeal";
                }
                string tb = "t_tenpay_appeal_trans";
                if (!string.IsNullOrEmpty(appeal_db) && !string.IsNullOrEmpty(appeal_tb))
                {
                    db = appeal_db;
                    tb = appeal_tb;
                    da = MySQLAccessFactory.GetMySQLAccess("CFTNEW_DB");
                }

                da.OpenConn();

                string strSql = " select * from " + db + "." + tb + " where Fid='" + fid + "'";
                DataSet ds = da.dsGetTotalData(strSql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    if (!string.IsNullOrEmpty(appeal_db) && !string.IsNullOrEmpty(appeal_tb))
                    {
                        HandleParameterByDBTB(ds, true);
                    }
                    else
                    {
                        HandleParameter(ds, true);
                    }


                    DataRow dr = ds.Tables[0].Rows[0];
                    int fstate = Int32.Parse(dr["FState"].ToString());
                    int ftype = Int32.Parse(dr["Ftype"].ToString());

                    if (fstate == 0)
                    {
                        strSql = "update " + db + "." + tb + " set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=0";
                    }
                    else if (fstate == 3)
                    {
                        strSql = " update " + db + "." + tb + " set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=3";
                    }
                    else if (fstate == 4)
                    {
                        strSql = " update " + db + "." + tb + " set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=4";
                    }
                    else if (fstate == 5)
                    {
                        strSql = "update " + db + "." + tb + " set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=5";
                    }
                    else if (fstate == 6)
                    {
                        strSql = " update " + db + "." + tb + " set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=6";
                    }
                    else if (fstate == 8)
                    {
                        strSql = " update " + db + "." + tb + " set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=8";
                    }
                    else if (fstate == 11)
                    {
                        strSql = " update " + db + "." + tb + " set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=11";
                    }
                    else if (fstate ==12)
                    {
                        strSql = " update " + db + "." + tb + " set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=12";
                    }
                    else
                    {
                        msg = "更新原有记录出错,此记录原始状态不正确";
                        LogHelper.LogInfo("DelAppeal:" + msg);
                        return false;
                    }

                    if (da.ExecSqlNum(strSql) == 1)
                    {
                        InputAppealNumber(user, "Delete", "appeal");
                        string IsSendAppeal = System.Configuration.ConfigurationManager.AppSettings["IsSendAppeal"].ToString();
                        if (IsSendAppeal == "Yes")
                        {
                            try
                            {
                                if (ftype == 1 || ftype == 5)//发送数据给风控目前只要密码（1）和更换关联手机（5）
                                {
                                    string fuin = System.Web.HttpUtility.UrlEncode(dr["fuin"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string ftypestr = System.Web.HttpUtility.UrlEncode(ftype.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string fstatestr = System.Web.HttpUtility.UrlEncode(fstate.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string cre_id = System.Web.HttpUtility.UrlEncode(dr["cre_id"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string clear_ppsstr = System.Web.HttpUtility.UrlEncode(dr["clear_pps"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string is_answer = System.Web.HttpUtility.UrlEncode(dr["labIsAnswer"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string mobile_no = System.Web.HttpUtility.UrlEncode(dr["mobile_no"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string email = System.Web.HttpUtility.UrlEncode(dr["email"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string score = System.Web.HttpUtility.UrlEncode(dr["score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string is_pass = System.Web.HttpUtility.UrlEncode(dr["IsPass"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string client_ip = System.Web.HttpUtility.UrlEncode(dr["client_ip"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string reason = System.Web.HttpUtility.UrlEncode(dr["reason"].ToString().Trim(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string fcomment = System.Web.HttpUtility.UrlEncode(Fcomment, System.Text.Encoding.GetEncoding("GB2312")); //备注
                                    string fcheck_info = System.Web.HttpUtility.UrlEncode("", System.Text.Encoding.GetEncoding("GB2312"));   //拒绝原因
                                    string fsubmittime = System.Web.HttpUtility.UrlEncode(dr["FSubmitTime"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string fchecktime = System.Web.HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Text.Encoding.GetEncoding("GB2312"));

                                    string Data = "fuin=" + fuin + "&ftype=" + ftypestr + "&fstate=3&cre_id=" + cre_id + "&clear_pps=" + clear_ppsstr +
                                        "&is_answer=" + is_answer + "&mobile_no=" + mobile_no + "&email=" + email + "&score=" + score + "&is_pass=" + is_pass +
                                        "&client_ip=" + client_ip + "&fsubmittime=" + fsubmittime + "&fchecktime=" + fchecktime + "&reason=" + reason + "&fcomment=" + fcomment + "&fcheck_info=" + fcheck_info;
                                    Data = System.Web.HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                                    Data = "protocol=user_appeal_notify&version=1.0&data=" + Data;

                                    CFT.Apollo.Logging.LogHelper.LogInfo("user_appeal_notify send:" + Data);

                                    System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                                    byte[] sendBytes = GB2312.GetBytes(Data);

                                    string IP = System.Configuration.ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                                    string PORT = System.Configuration.ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                                    TENCENT.OSS.CFT.KF.Common.UDP.GetUDPReplyNoReturnNotReturn(sendBytes, IP, Int32.Parse(PORT));
                                }
                            }
                            catch
                            {
                            }
                        }
                        return true;
                    }
                    else
                    {
                        msg = "更新原有记录出错,此记录不存在或原始状态不正确";
                        LogHelper.LogInfo("DelAppeal:" + msg);
                        return false;
                    }
                }
                else
                {
                    msg = "查找记录失败.";
                    LogHelper.LogInfo("DelAppeal:" + msg);
                    return false;
                }

            }
            catch (Exception err)
            {
                LogHelper.LogInfo("DelAppeal:" + err.Message);
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        /// <summary>
        /// 自助申诉拒绝
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="reason"></param>
        /// <param name="OtherReason"></param>
        /// <param name="Fcomment"></param>
        /// <param name="user"></param>
        /// <param name="userIP"></param>
        /// <returns></returns>
        public bool CannelAppeal(string fid, string reason, string OtherReason, string Fcomment, string user, string userIP)
        {
            string msg = "";
            var da = MySQLAccessFactory.GetMySQLAccess("CFT_DB");
            try
            {
                string db = "db_appeal";
                try
                {
                    db = System.Configuration.ConfigurationManager.AppSettings["DB_CFT"].ToString();
                }
                catch
                {
                    db = "db_appeal";
                }
                string tb = db + ".t_tenpay_appeal_trans";

                if (OtherReason != null && OtherReason.Trim() != "")
                {
                    OtherReason = PublicRes.replaceMStr(OtherReason);
                }
                if (Fcomment != null && Fcomment.Trim() != "")
                {
                    Fcomment = PublicRes.replaceMStr(Fcomment);
                }

                da.OpenConn();
                //修改姓名需要新姓名. 取回支付密码需要clear_pps, 注销不需要多余东西
                string strSql = "select * from " + tb + " where Fid='" + fid + "'";
                DataSet ds = da.dsGetTotalData(strSql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    HandleParameter(ds, true);

                    DataRow dr = ds.Tables[0].Rows[0];
                    int fstate = Int32.Parse(dr["FState"].ToString());
                    int ftype = Int32.Parse(dr["FType"].ToString());
                    string fuin = dr["FUin"].ToString();
                    string email = dr["Femail"].ToString();
                    string submittime = DateTime.Parse(dr["FSubmitTime"].ToString()).ToString("yyyy年MM月dd日");

                    //读取现在的用户名？furion 20060902
                    string username = AccountData.GetUserNameFromUin(fuin);
                    //简化注册用户这里处理下
                    if ((username == null || username.Trim() == "") && ftype == 6)
                    {
                        username = "亲爱的财付通客户";
                    }

                    if (username == null || username.Trim() == "")
                    {
                        msg = "取原有用户名时出错";
                        LogHelper.LogInfo("CannelAppeal:" + msg);
                        username = fuin;
                        return false;
                    }
                    string EmailReason = reason;

                    if (ftype == 1 || ftype == 11)  //新流程中，对于绑定手机的用户就发给他原来绑定的邮箱，对于绑定邮箱的用户就发给录入的邮箱
                    {
                        //找回密码
                        string cont_type = dr["cont_type"].ToString();

                        if (cont_type == "1") //cont_type为新加字段，如果不为空就说明走新流程,cont_type=1时,mobile_no有效 cont_type=2时,femail有效
                        {
                            string Fuid = AccountData.ConvertToFuid(fuin);

                            if (Fuid == null || Fuid == "")
                            {
                                msg = "该帐号的内部ID为空,查询绑定邮箱失败!";
                                LogHelper.LogInfo("CannelAppeal:" + msg);
                                return false;
                            }

                            string sql = " select * from msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_" + Fuid.Substring(Fuid.Length - 1, 1) + " where Fuid = '" + Fuid + "'";
                            DataSet emailds = da.dsGetTotalData(sql);
                            if (emailds == null || emailds.Tables.Count == 0 || emailds.Tables[0].Rows.Count == 0)
                            {
                                msg = "该帐号没有绑定邮箱!";
                                LogHelper.LogInfo("CannelAppeal:" + msg);
                                return false;
                            }

                            email = emailds.Tables[0].Rows[0]["Femail"].ToString();
                        }

                        EmailReason = reason + "<br/> 温馨提示：<br/> 如果您的账号已经绑定了手机号码，并可有效接收验证码，您可以直接进入<a href='https://www.tenpay.com/v2.0/main/getPwdByMobile_index.shtml'>手机绑定找回支付密码地址</a>通过绑定手机快速找回您的支付密码。&";
                    }
                    //发送失败邮件 失败的一律没有三四参数
                    if (!SendAppealMail(email, ftype, false, username, submittime, "", "", EmailReason, OtherReason, fuin, out msg))
                    {
                        msg = "发送邮件失败：" + msg;
                        LogHelper.LogInfo("CannelAppeal:" + msg);
                        return false;
                    }
                    else
                    {
                        if (fstate == 0)
                        {
                            strSql = "update " + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=0";
                        }
                        else if (fstate == 3)
                        {
                            strSql = "update " + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=3";
                        }
                        else if (fstate == 4)
                        {
                            strSql = " update " + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=4";
                        }
                        else if (fstate == 5)
                        {
                            strSql = "update " + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=5";
                        }
                        else if (fstate == 6)
                        {
                            strSql = " update " + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=6";
                        }
                        else if (fstate == 8)
                        {
                            strSql = " update " + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=8";
                        }
                        else if (fstate == 11)//特殊申诉 待补充资料状态
                        {
                            strSql = " update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=11";
                        }
                        else if (fstate == 12)//特殊申诉 已补充资料状态
                        {
                            strSql = " update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=12";
                        }
                        else
                        {
                            msg = "更新原有记录出错,此记录原始状态不正确";
                            LogHelper.LogInfo("CannelAppeal:" + msg);
                            return false;
                        }
                        if (da.ExecSqlNum(strSql) == 1)
                        {
                            InputAppealNumber(user, "Fail", "appeal");
                            string IsSendAppeal = System.Configuration.ConfigurationManager.AppSettings["IsSendAppeal"].ToString();
                            if (IsSendAppeal == "Yes")
                            {
                                try
                                {
                                    if (ftype == 1 || ftype == 5)//发送数据给风控目前只要密码（1）和更换关联手机（5）
                                    {
                                        fuin = System.Web.HttpUtility.UrlEncode(fuin, System.Text.Encoding.GetEncoding("GB2312"));
                                        string ftypestr = System.Web.HttpUtility.UrlEncode(ftype.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        //string fstatestr = System.Web.HttpUtility.UrlEncode(fstate.ToString(),System.Text.Encoding.GetEncoding("GB2312"));
                                        string cre_id = System.Web.HttpUtility.UrlEncode(dr["cre_id"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string clear_ppsstr = System.Web.HttpUtility.UrlEncode(dr["clear_pps"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string is_answer = System.Web.HttpUtility.UrlEncode(dr["labIsAnswer"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string mobile_no = System.Web.HttpUtility.UrlEncode(dr["mobile_no"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        email = System.Web.HttpUtility.UrlEncode(email, System.Text.Encoding.GetEncoding("GB2312"));
                                        string score = System.Web.HttpUtility.UrlEncode(dr["score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string is_pass = System.Web.HttpUtility.UrlEncode(dr["IsPass"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string client_ip = System.Web.HttpUtility.UrlEncode(dr["client_ip"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string reasonstr = System.Web.HttpUtility.UrlEncode(dr["reason"].ToString().Trim(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string fcomment = System.Web.HttpUtility.UrlEncode(Fcomment, System.Text.Encoding.GetEncoding("GB2312")); //备注
                                        string fcheck_info = System.Web.HttpUtility.UrlEncode(reason + OtherReason, System.Text.Encoding.GetEncoding("GB2312"));   //拒绝原因
                                        string fsubmittime = System.Web.HttpUtility.UrlEncode(dr["FSubmitTime"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string fchecktime = System.Web.HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Text.Encoding.GetEncoding("GB2312"));

                                        string Data = "fuin=" + fuin + "&ftype=" + ftypestr + "&fstate=2&cre_id=" + cre_id + "&clear_pps=" + clear_ppsstr +
                                            "&is_answer=" + is_answer + "&mobile_no=" + mobile_no + "&email=" + email + "&score=" + score + "&is_pass=" + is_pass +
                                            "&client_ip=" + client_ip + "&fsubmittime=" + fsubmittime + "&fchecktime=" + fchecktime + "&reason=" + reasonstr + "&fcomment=" + fcomment + "&fcheck_info=" + fcheck_info;
                                        Data = System.Web.HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                                        Data = "protocol=user_appeal_notify&version=1.0&data=" + Data;

                                        CFT.Apollo.Logging.LogHelper.LogInfo("user_appeal_notify send:" + Data);

                                        System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                                        byte[] sendBytes = GB2312.GetBytes(Data);

                                        string IP = System.Configuration.ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                                        string PORT = System.Configuration.ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                                        TENCENT.OSS.CFT.KF.Common.UDP.GetUDPReplyNoReturnNotReturn(sendBytes, IP, Int32.Parse(PORT));
                                    }
                                }
                                catch
                                {
                                }
                            }
                            return true;
                        }
                        else
                        {
                            msg = "更新原有记录出错,此记录不存在或原始状态不正确";
                            LogHelper.LogInfo("CannelAppeal:" + msg);
                            return false;
                        }
                    }
                }
                else
                {
                    msg = "查找记录失败.";
                    LogHelper.LogInfo("CannelAppeal:" + msg);
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                LogHelper.LogInfo("CannelAppeal:" + msg);
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public bool CancelAppealDBTB(string fid, string db, string tb, string reason, string OtherReason, string Fcomment, string user, string userIP)
        {
            string msg = "";

            if (reason != null && reason.Trim() != "")
            {
                reason = PublicRes.replaceMStr(reason);
            }

            MySqlAccess da = MySQLAccessFactory.GetMySQLAccess("CFTNEW_DB");
            MySqlAccess da2 = MySQLAccessFactory.GetMySQLAccess("CFT_DB");
            try
            {
                da.OpenConn();
                da2.OpenConn();
                if (OtherReason != null && OtherReason.Trim() != "")
                {
                    OtherReason = PublicRes.replaceMStr(OtherReason);
                }
                if (Fcomment != null && Fcomment.Trim() != "")
                {
                    Fcomment = PublicRes.replaceMStr(Fcomment);
                }

                //修改姓名需要新姓名. 取回支付密码需要clear_pps, 注销不需要多余东西
                string strSql = "select * from " + db + "." + tb + " where FID='" + fid + "'";
                DataSet ds = da.dsGetTotalData(strSql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    HandleParameterByDBTB(ds, true);

                    DataRow dr = ds.Tables[0].Rows[0];
                    int fstate = Int32.Parse(dr["FState"].ToString());
                    int ftype = Int32.Parse(dr["FType"].ToString());
                    string fuin = dr["FUin"].ToString();
                    string email = dr["Femail"].ToString();
                    string submittime = DateTime.Parse(dr["FSubmitTime"].ToString()).ToString("yyyy年MM月dd日");
                    string mobile = dr["mobile_no"].ToString();
                    string mobile_bind = dr["FMobile"].ToString();//绑定手机号,取到传给风控

                    //读取现在的用户名？furion 20060902
                    string username = AccountData.GetUserNameFromUin(fuin);
                    //简化注册用户这里处理下
                    if ((username == null || username.Trim() == "") && ftype == 6)
                    {
                        username = "亲爱的财付通客户";
                    }

                    if (username == null || username.Trim() == "")
                    {
                        msg = "取原有用户名时出错";
                        username = fuin;
                        LogHelper.LogInfo("CancelAppealDBTB:" + msg);
                        return false;
                    }
                    string EmailReason = reason;
                    string mesReason = "";
                    mesReason = reason + "&" + OtherReason;
                    if (ftype == 1 || ftype == 11)
                    {
                        EmailReason = reason + "<br/> 温馨提示：<br/> 如果您的账号已经绑定了手机号码，并可有效接收验证码，您可以直接进入<a href='https://www.tenpay.com/v2.0/main/getPwdByMobile_index.shtml'>手机绑定找回支付密码地址</a>通过绑定手机快速找回您的支付密码。&";
                    }

                    if (!SendAppealMessage(mobile, ftype, "", false, mesReason, out msg))
                    {
                        msg = "发送短信失败：" + msg;
                        LogHelper.LogInfo("CancelAppealDBTB:" + msg);
                        return false;
                    }
                    //发送失败邮件 失败的一律没有三四参数
                    if (!SendAppealMail(email, ftype, false, username, submittime, "", "", EmailReason, OtherReason, fuin, out msg))
                    {
                        msg = "发送邮件失败：" + msg;
                        LogHelper.LogInfo("CancelAppealDBTB:" + msg);
                        return false;
                    }
                    else
                    {
                        if (fstate == 0)
                        {
                            strSql = "update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=0";
                        }
                        else if (fstate == 3)
                        {
                            strSql = "update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=3";
                        }
                        else if (fstate == 4)
                        {
                            strSql = " update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=4";
                        }
                        else if (fstate == 5)
                        {
                            strSql = "update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=5";
                        }
                        else if (fstate == 6)
                        {
                            strSql = " update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=6";
                        }
                        else if (fstate == 8)
                        {
                            strSql = " update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=8";
                        }
                        else if (fstate == 11)//特殊申诉 待补充资料状态
                        {
                            strSql = " update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=11";
                        }
                        else if (fstate == 12)//特殊申诉 已补充资料状态
                        {
                            strSql = " update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=12";
                        }
                        else
                        {
                            msg = "更新原有记录出错,此记录原始状态不正确";
                            LogHelper.LogInfo("CancelAppealDBTB:" + msg);
                            return false;
                        }
                        if (da.ExecSqlNum(strSql) == 1)
                        {
                            InputAppealNumber(user, "Fail", "appeal");
                            string IsSendAppeal = System.Configuration.ConfigurationManager.AppSettings["IsSendAppeal"].ToString();
                            if (IsSendAppeal == "Yes")
                            {
                                try
                                {
                                    if (ftype == 1 || ftype == 5 || ftype == 11)//发送数据给风控目前只要密码（1）和更换关联手机（5）
                                    {
                                        fuin = System.Web.HttpUtility.UrlEncode(fuin, System.Text.Encoding.GetEncoding("GB2312"));
                                        string ftypestr = System.Web.HttpUtility.UrlEncode(ftype.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        //string fstatestr = System.Web.HttpUtility.UrlEncode(fstate.ToString(),System.Text.Encoding.GetEncoding("GB2312"));
                                        string cre_id = System.Web.HttpUtility.UrlEncode(dr["cre_id"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string clear_ppsstr = System.Web.HttpUtility.UrlEncode(dr["clear_pps"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string is_answer = System.Web.HttpUtility.UrlEncode(dr["labIsAnswer"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        mobile_bind = System.Web.HttpUtility.UrlEncode(mobile_bind, System.Text.Encoding.GetEncoding("GB2312"));
                                        email = System.Web.HttpUtility.UrlEncode(email, System.Text.Encoding.GetEncoding("GB2312"));
                                        string score = System.Web.HttpUtility.UrlEncode(dr["score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string is_pass = System.Web.HttpUtility.UrlEncode(dr["IsPass"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string client_ip = System.Web.HttpUtility.UrlEncode(dr["client_ip"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string reasonstr = System.Web.HttpUtility.UrlEncode(dr["reason"].ToString().Trim(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string fcomment = System.Web.HttpUtility.UrlEncode(Fcomment, System.Text.Encoding.GetEncoding("GB2312")); //备注
                                        string fcheck_info = System.Web.HttpUtility.UrlEncode(reason + OtherReason, System.Text.Encoding.GetEncoding("GB2312"));   //拒绝原因
                                        string fsubmittime = System.Web.HttpUtility.UrlEncode(dr["FSubmitTime"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string fchecktime = System.Web.HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Text.Encoding.GetEncoding("GB2312"));

                                        string Data = "fuin=" + fuin + "&ftype=" + ftypestr + "&fstate=2&cre_id=" + cre_id + "&clear_pps=" + clear_ppsstr +
                                            "&is_answer=" + is_answer + "&mobile_no=" + mobile_bind + "&email=" + email + "&score=" + score + "&is_pass=" + is_pass +
                                            "&client_ip=" + client_ip + "&fsubmittime=" + fsubmittime + "&fchecktime=" + fchecktime + "&reason=" + reasonstr + "&fcomment=" + fcomment + "&fcheck_info=" + fcheck_info;
                                        Data = System.Web.HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                                        Data = "protocol=user_appeal_notify&version=1.0&data=" + Data;

                                        CFT.Apollo.Logging.LogHelper.LogInfo("user_appeal_notify send:" + Data);

                                        System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                                        byte[] sendBytes = GB2312.GetBytes(Data);

                                        string IP = System.Configuration.ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                                        string PORT = System.Configuration.ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                                        TENCENT.OSS.CFT.KF.Common.UDP.GetUDPReplyNoReturnNotReturn(sendBytes, IP, Int32.Parse(PORT));
                                    }
                                }
                                catch
                                {
                                }
                            }
                            return true;
                        }
                        else
                        {
                            msg = "更新原有记录出错,此记录不存在或原始状态不正确";
                            LogHelper.LogInfo("CancelAppealDBTB:" + msg);
                            return false;
                        }
                    }
                }
                else
                {
                    msg = "查找记录失败.";
                    LogHelper.LogInfo("CancelAppealDBTB:" + msg);
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                LogHelper.LogInfo("CancelAppealDBTB:" + msg);
                return false;
            }
            finally
            {
                da.Dispose();
                da2.Dispose();
            }
        }

        /// <summary>
        /// 申诉通过
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="Fcomment"></param>
        /// <param name="user"></param>
        /// <param name="userIP"></param>
        /// <returns></returns>
        public bool ConfirmAppeal(string fid, string Fcomment, string user, string userIP)
        {
            string msg = "";

            string db = "db_appeal";
            try
            {
                db = System.Configuration.ConfigurationManager.AppSettings["DB_CFT"].ToString();
            }
            catch
            {
                db = "db_appeal";
            }
            string tb = "t_tenpay_appeal_trans";

            MySqlAccess da = MySQLAccessFactory.GetMySQLAccess("CFT_DB");
            try
            {
                da.OpenConn();

                if (Fcomment != null && Fcomment.Trim() != "")
                {
                    Fcomment = PublicRes.replaceMStr(Fcomment);
                }

                //先取出一些需要的信息 当前状态,修改类别, QQ号,email
                //修改姓名需要新姓名. 取回支付密码需要clear_pps, 注销不需要多余东西
                string strSql = "select * from " + db + "." + tb + " where Fid='" + fid + "'";
                DataSet ds = da.dsGetTotalData(strSql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    HandleParameter(ds, true);

                    DataRow dr = ds.Tables[0].Rows[0];
                    int fstate = Int32.Parse(dr["FState"].ToString());
                    int ftype = Int32.Parse(dr["FType"].ToString());
                    string fuin = dr["FUin"].ToString();
                    string email = dr["Femail"].ToString();
                    string question1 = dr["question1"].ToString();
                    string answer1 = dr["answer1"].ToString();
                    string cont_type = dr["cont_type"].ToString();
                    string mobile_no = dr["mobile_no"].ToString();
                    string client_ip = dr["client_ip"].ToString();

                    if (fstate == 1 || fstate == 7)
                    {
                        msg = "原记录的状态不正确";
                        return false;
                    }

                    string nowtime = PublicRes.strNowTimeStander;
                    string new_name = "";
                    int clear_pps = 0;

                    string submittime = DateTime.Parse(dr["FSubmitTime"].ToString()).ToString("yyyy年MM月dd日");

                    //读取现在的用户名？furion 20060902
                    string username = AccountData.GetUserNameFromUin(fuin);
                    //简化注册用户这里处理下
                    if ((username == null || username.Trim() == "") && ftype == 6)
                    {
                        username = "亲爱的财付通客户";
                    }
                    if (username == null || username.Trim() == "")
                    {
                        msg = "取原有用户名时出错";
                        //return false;
                    }

                    string p3 = "";
                    string p4 = "";

                    //					if (!PublicRes.ReleaseCache(fuin,"qqid"))
                    //					{
                    //						Common.CommLib.commRes.sendLog4Log("QUeryInfo.ConfirmAppeal","通过自助申诉时，预清除用户"+ fuin + "cache失败！请检查！");
                    //						msg = "通过自助申诉时，预清除用户"+ fuin + "cache失败！请检查！";
                    //						return false;
                    //					}
                    //进行处理.	 //发送邮件
                    #region 进行处理
                    if (ftype == 0)
                    {
                        string Fuid = AccountData.ConvertToFuid(fuin);
                        if (Fuid.Length < 3)
                        {
                            msg = "取内部ID失败";
                            LogHelper.LogInfo("ConfirmAppeal:" + msg);
                            return false;
                        }

                        /*
                        string inmsg = "uid=" + Fuid;
                        inmsg += "&optype=3";
                        inmsg += "&time_stamp=" + TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(nowtime);
                        inmsg += "&server_ip=" + userIP;
                        inmsg += "&client_ip=" + client_ip;
                        inmsg += "&sign=" + System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Fuid + "3" + TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(nowtime) + "cft_uk_key","md5").ToLower();
                        */

                        //md5( uid + optype + time_stamp + watch_word)
                        string watchWord = System.Configuration.ConfigurationManager.AppSettings["watchword"].ToLower();
                        string inmsg = "watchword=" + watchWord;
                        inmsg += "&uid=" + Fuid;
                        inmsg += "&optype=3";
                        inmsg += "&time_stamp=" + TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(nowtime);
                        inmsg += "&server_ip=" + userIP;
                        inmsg += "&client_ip=" + client_ip;
                        inmsg += "&sign=" + System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Fuid + "3" + TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(nowtime) + watchWord, "md5").ToLower();


                        string reply;
                        short sresult;

                        if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("ra_apeal_service", inmsg, true, out reply, out sresult, out msg))
                        {
                            if (sresult != 0)
                            {
                                msg = "ra_apeal_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                LogHelper.LogInfo("ConfirmAppeal:" + msg);
                                return false;
                            }
                            else
                            {
                                if (reply.IndexOf("result=0") > -1)   //&0=298751028&res_info=ok&result=0 正常值
                                {
                                }
                                else
                                {
                                    msg = "ra_apeal_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                    LogHelper.LogInfo("ConfirmAppeal:" + msg);
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            msg = "ra_apeal_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                            LogHelper.LogInfo("ConfirmAppeal:" + msg);
                            return false;
                        }
                    }
                    else if (ftype == 1 || ftype == 11)
                    {
                        //找回密码
                        clear_pps = Int32.Parse(dr["clear_pps"].ToString());

                        bool IsNew; //是否新流程
                        if (cont_type != "") //cont_type为新加字段，如果为空就说明走老流程
                        {
                            IsNew = true;
                            bool IsMobile = false;
                            bool IsMail = false;
                            //cont_type=1时,mobile_no有效 cont_type=2时,femail有效

                            if (cont_type == "1")
                                IsMobile = true;
                            else if (cont_type == "2")
                                IsMail = true;

                            if (cont_type == "1" || cont_type == "2")//无需绑定
                            {
                                //如果是邮箱型用户，不需要绑定邮箱（默认绑定邮箱就是该帐号）
                                if (IsMail && fuin.IndexOf("@") > -1)
                                {
                                }
                                else
                                {
                                    string BindMail = "";
                                    if (!BindMsgNotify(fuin, IsMobile, mobile_no, IsMail, email, client_ip, out BindMail, out msg)) return false;

                                    email = BindMail;
                                }
                            }
                        }
                        else
                        {
                            IsNew = false;
                        }

                        if (!GetNewPwd(fuin, clear_pps, userIP, nowtime, question1, answer1, IsNew, out msg)) return false;

                        string warmTips = @"<br/><br/>温馨提示：<br/>如果您的账号还未绑定关联手机，建议您现在立即绑定。绑定成功后，您既可根据关联手机接收验证码快速实时的修改您的支付密码。";

                        p3 = msg;
                        if (clear_pps == 1)
                            //p4 = "您的密码保护问题已被清空，请登陆重新设置您的密码保护问题！"; 
                            p4 = "您的密保资料已经更新成功，请使用该新的密保答案操作您的财付通账户！" + warmTips;
                        else
                            p4 = warmTips;
                    }
                    else if (ftype == 2)
                    {
                        //修改用户姓名
                        new_name = QueryInfo.GetString(dr["new_name"]);
                        /*旧接口
                        if(!UpdateUserName(fuin,new_name,false,userIP,nowtime, out msg))
                            return false;
                        */
                        //新接口
                        string Fuid = AccountData.ConvertToFuid(fuin);
                        if (Fuid.Length < 3)
                        {
                            msg = "取内部ID失败";
                            LogHelper.LogInfo("ConfirmAppeal:" + msg);
                            return false;
                        }

                        //遇中文MD5加密有问题哦，因为编码规则不一样
                        System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                        byte[] outbuff = GB2312.GetBytes(Fuid + "|" + fuin + "|" + new_name + "||5416d7cd6ef195a0f7622a9c56b55e84");
                        System.Security.Cryptography.MD5CryptoServiceProvider get_md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                        byte[] hash_byte = get_md5.ComputeHash(outbuff);

                        string inmsg = "uin=" + fuin;
                        inmsg += "&true_name=" + new_name;
                        inmsg += "&vali_type=100";
                        inmsg += "&token=" + System.BitConverter.ToString(hash_byte).Replace("-", "").ToLower();

                        string reply;
                        short sresult;

                        if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("ui_modify_usernameid_service", inmsg, true, out reply, out sresult, out msg))
                        {
                            if (sresult != 0)
                            {
                                msg = "ui_modify_usernameid_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                LogHelper.LogInfo("ConfirmAppeal:" + msg);
                                return false;
                            }
                            else
                            {
                                if (reply.StartsWith("result=0"))
                                {
                                }
                                else
                                {
                                    msg = "ui_modify_usernameid_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                    LogHelper.LogInfo("ConfirmAppeal:" + msg);
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            msg = "ui_modify_usernameid_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                            LogHelper.LogInfo("ConfirmAppeal:" + msg);
                            return false;
                        }

                        p3 = new_name;
                    }
                    else if (ftype == 3)
                    {
                        //修改公司名
                        new_name = QueryInfo.GetString(dr["new_company"]);

                        if (!UpdateUserName(fuin, new_name, true, userIP, nowtime, out msg))
                            return false;

                        p3 = new_name;
                    }
                    else if (ftype == 4)
                    {
                        //注销帐号
                        if (!DelUser(fuin, email, Fcomment, user, userIP, nowtime, out msg))
                            return false;

                        //因为现在暂时不支持注销绑定关系 furion 20060902
                        //p3 = "，（已同时注销了该帐户原银行卡号和身份证号码的绑定关系）谢谢";
                        p3 = "";
                    }
                    else if (ftype == 5)//完整帐号的申述找回手机的，审核通过后客服系统就增加一步绑定邮箱。 andrew 20110419
                    {

                        //找回密码
                        clear_pps = Int32.Parse(dr["clear_pps"].ToString());

                        bool IsNew; //是否新流程
                        if (cont_type != "") //cont_type为新加字段，如果为空就说明走老流程
                        {
                            IsNew = true;
                            bool IsMobile = false;
                            bool IsMail = false;
                            //cont_type=1时,mobile_no有效 cont_type=2时,femail有效

                            if (cont_type == "1")
                                IsMobile = true;
                            else if (cont_type == "2")
                                IsMail = true;

                            if (cont_type == "1" || cont_type == "2")
                            {
                                //如果是邮箱型用户，不需要绑定邮箱（默认绑定邮箱就是该帐号）
                                if (IsMail && fuin.IndexOf("@") > -1)
                                {
                                }
                                else
                                {
                                    string BindMail = "";
                                    if (!BindMsgNotify(fuin, IsMobile, mobile_no, IsMail, email, client_ip, out BindMail, out msg)) return false;

                                    email = BindMail;
                                }
                            }
                        }
                        else
                        {
                            IsNew = false;
                        }

                        if (clear_pps == 1)
                        {
                            //只替换密保问题和答案
                            if (!CheckQuestion(fuin, userIP, nowtime, question1, answer1, IsNew, out msg)) return false;
                        }

                        if (dr["from"].ToString() == "TENPAY_MOBILE")
                        {
                            string Fuid = AccountData.ConvertToFuid(fuin);
                            string ret = "";
                            string appealtime = TimeTransfer.GetTickFromTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("GB2312");
                            string cgi = System.Configuration.ConfigurationManager.AppSettings["MobileCgi"].ToString();
                            cgi += "?uin=" + fuin;
                            cgi += "&bargainor_id=1000000000";
                            cgi += "&uid=" + Fuid;
                            cgi += "&appealtime=" + appealtime;
                            cgi += "&status=0";
                            cgi += "&cmd=update";
                            cgi += "&newmobile=" + mobile_no;
                            cgi += "&chv=9";
                            cgi += "&sign=" + System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("appealtime=" + appealtime + "&bargainor_id=1000000000" +
                                "&chv=9" + "&cmd=update" + "&newmobile=" + mobile_no + "&status=0" + "&uid=" + Fuid + "&uin=" + fuin + "&key=Adf^#KK12D", "md5").ToLower();
                            System.Net.HttpWebRequest webrequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(cgi);
                            System.Net.HttpWebResponse webresponse = (System.Net.HttpWebResponse)webrequest.GetResponse();
                            Stream stream = webresponse.GetResponseStream();
                            StreamReader streamReader = new StreamReader(stream, encoding);
                            ret = streamReader.ReadToEnd();
                            webresponse.Close();
                            streamReader.Close();
                        }

                        //更换关联手机,不用做什么操作,直接发出邮件就可以了.
                        string url = System.Configuration.ConfigurationManager.AppSettings["ChangeMobileUrl"].Trim();

                        // 更换关联手机的话，需要添加WarmTips
                        string warmTips = "<br/> 温馨提示：<br/>此链接地址有效期为72小时，请您在有效期内完成新手机帐号的绑定，如果您无法正常打开链接地址，请您使用IE浏览器登录<a href='www.tenpay.com'>财付通主站</a>后，再重新点击邮件链接地址，或者请您直接将以上完整链接地址复制到浏览器地址栏中打开操作即可,谢谢！";

                        p3 = url + fid;

                        if (clear_pps == 1)
                            p4 = p3 + "<br/>您的密保资料已经更新成功，请使用您申诉时成功修改的密保答案即可！" + warmTips;
                        else
                            p4 = p3 + warmTips;
                    }
                    else if (ftype == 6) //andrew 20110419
                    {
                        //更换关联手机,不用做什么操作,直接发出邮件就可以了.
                        string url = System.Configuration.ConfigurationManager.AppSettings["ChangeMobileUrl"].Trim();

                        // 更换关联手机的话，需要添加WarmTips
                        string warmTips = "<br/> 温馨提示：<br/>此链接地址有效期为72小时，请您在有效期内完成新手机帐号的绑定，如果您无法正常打开链接地址，请您使用IE浏览器登录<a href='www.tenpay.com'>财付通主站</a>后，再重新点击邮件链接地址，或者请您直接将以上完整链接地址复制到浏览器地址栏中打开操作即可,谢谢！";

                        p3 = url + fid;
                        p4 = p3 + warmTips;
                    }
                    else if (ftype == 7)
                    {
                        string new_cre_id = dr["new_cre_id"].ToString();
                        //修改身份证号
                        /*旧接口
                        if(!UpdateCreid(fuin,new_cre_id,userIP,nowtime, out msg))
                            return false;
                        */
                        //新接口
                        string Fuid = AccountData.ConvertToFuid(fuin);
                        if (Fuid.Length < 3)
                        {
                            msg = "取内部ID失败";
                            LogHelper.LogInfo("ConfirmAppeal:" + msg);
                            return false;
                        }
                        string inmsg = "uin=" + fuin;
                        inmsg += "&cre_type=" + "1";
                        inmsg += "&cre_id=" + new_cre_id;
                        inmsg += "&vali_type=100";
                        inmsg += "&token=" + System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Fuid + "|" + fuin + "||" + new_cre_id + "|5416d7cd6ef195a0f7622a9c56b55e84", "md5").ToLower();

                        string reply;
                        short sresult;

                        if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("ui_modify_usernameid_service", inmsg, true, out reply, out sresult, out msg))
                        {
                            if (sresult != 0)
                            {
                                msg = "ui_modify_usernameid_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                LogHelper.LogInfo("ConfirmAppeal:" + msg);
                                return false;
                            }
                            else
                            {
                                if (reply.StartsWith("result=0"))
                                {
                                }
                                else
                                {
                                    msg = "ui_modify_usernameid_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                    LogHelper.LogInfo("ConfirmAppeal:" + msg);
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            msg = "ui_modify_usernameid_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                            LogHelper.LogInfo("ConfirmAppeal:" + msg);
                            return false;
                        }
                        p3 = new_cre_id;
                    }
                    else if (ftype == 9) //bruceliao 20121106
                    {
                        string Key = System.Configuration.ConfigurationManager.AppSettings["Mbtoken_Unbind_Key"].Trim();
                        string Fuid = AccountData.ConvertToFuid(fuin);
                        string time_stamp = TimeTransfer.GetTickFromTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        string inmsg = "uin=" + fuin;
                        inmsg += "&optype=" + "1";
                        inmsg += "&uid=" + Fuid;
                        inmsg += "&time_stamp=" + time_stamp;
                        inmsg += "&sign=" + System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Fuid + "1" + time_stamp + Key, "md5").ToLower();

                        string reply;
                        short sresult;

                        if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("mbtoken_unbind_service", inmsg, true, out reply, out sresult, out msg))
                        {
                            if (sresult != 0)
                            {
                                msg = "mbtoken_unbind_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                LogHelper.LogInfo("ConfirmAppeal:" + msg);
                                return false;
                            }
                            else
                            {
                                if (reply.IndexOf("result=0") > -1)
                                {
                                }
                                else
                                {
                                    msg = "mbtoken_unbind_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                    LogHelper.LogInfo("ConfirmAppeal:" + msg);
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            msg = "mbtoken_unbind_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                            LogHelper.LogInfo("ConfirmAppeal:" + msg);
                            return false;
                        }
                    }
                    else if (ftype == 10) //申诉类型第三方令牌
                    {
                        string ip = System.Configuration.ConfigurationManager.AppSettings["TCPMobileTokenIP"].Trim();
                        int portNumber;
                        int.TryParse(System.Configuration.ConfigurationManager.AppSettings["TCPMobileTokenPORT"].Trim(), out portNumber);//字符串形式port转换成int形式
                        string Key = System.Configuration.ConfigurationManager.AppSettings["Mbtoken_Unbind_Key"].Trim();
                        string Fuid = AccountData.ConvertToFuid(fuin);
                        string time_stamp = TimeTransfer.GetTickFromTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        string request_type = "8161";
                        string inmsg = "request_type=" + request_type;
                        inmsg += "&optype=" + "1";
                        inmsg += "&uid=" + Fuid;
                        inmsg += "&ver=" + "1";
                        inmsg += "&head_u=" + Fuid;
                        inmsg += "&time_stamp=" + time_stamp;
                        inmsg += "&sign=" + System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Fuid + "1" + time_stamp + Key, "md5").ToLower();
                        var parameters = System.Text.Encoding.Default.GetBytes(inmsg);
                        var length = new byte[4];
                        length = BitConverter.GetBytes(parameters.Length);
                        List<byte> bufferIn = new List<byte>();
                        bufferIn.AddRange(length);
                        bufferIn.AddRange(parameters);


                        string token_seq;
                        string sresult;

                        TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTCPReply(inmsg, bufferIn.ToArray(), ip, portNumber, out msg, "sdyb_mbtoken_itg_srv", out sresult, out token_seq);
                        if (!("0".Equals(sresult)))//sresult="0"表示解绑成功
                        {
                            msg = "sdyb_mbtoken_itg_srv接口返回失败应答：result=" + sresult + "，msg=" + msg + "，token_seq=" + token_seq;
                            LogHelper.LogInfo("ConfirmAppeal:" + msg);
                            return false;
                        }
                    }
                    else
                    {
                        msg = "请求类型不正确";
                        LogHelper.LogInfo("ConfirmAppeal:" + msg);
                        return false;
                    }
                    #endregion
                    if (!SendAppealMail(email, ftype, true, username, submittime, p3, p4, "", "", fuin, out msg))
                    {
                        msg = "发送邮件失败：" + msg;
                        LogHelper.LogInfo("ConfirmAppeal:" + msg);
                        return false;
                    }
                    else
                    {

                        if (fstate == 0)
                        {
                            strSql = "update " + db + "." + tb + " set FState=1,"
                                + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=0";
                        }
                        else if (fstate == 2)
                        {
                            strSql = "update " + db + "." + tb + " set FState=1,"
                                + " Fcomment='二次审核,拒绝转审核通过." + Fcomment + "', FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=2";
                        }
                        else if (fstate == 3)
                        {
                            strSql = "update " + db + "." + tb + " set FState=1,"
                                + " Fcomment='" + Fcomment + "', FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=3";
                        }
                        else if (fstate == 4)
                        {
                            strSql = " update " + db + "." + tb + " set FState=1,"
                                + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=4";
                        }
                        else if (fstate == 5)
                        {
                            strSql = "update " + db + "." + tb + " set FState=1,"
                                + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=5";
                        }
                        else if (fstate == 6)
                        {
                            strSql = " update " + db + "." + tb + " set FState=1,"
                                + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=6";
                        }
                        else if (fstate == 8)
                        {
                            strSql = " update " + db + "." + tb + " set FState=1,"
                                + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=8";
                        }
                        // 2012/4/25 添加允许通过短信撤销状态的申诉！，并添加相应的风控通知！
                        else if (fstate == 9)
                        {
                            strSql = " update " + db + "." + tb + " set FState=1,"
                                + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=9";
                        }
                        else if (fstate == 11)//待补充资料,20150429，增加通过
                        {
                            strSql = " update " + db + "." + tb + " set FState=1,"
                                + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=11";
                        }
                        else if (fstate == 12)//已补充资料
                        {
                            strSql = " update " + db + "." + tb + " set FState=1,"
                                + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=12";
                        }
                        else
                        {
                            msg = "更新原有记录出错,此记录原始状态不正确";
                            LogHelper.LogInfo("ConfirmAppeal:" + msg);
                            return false;
                        }
                        if (da.ExecSqlNum(strSql) == 1)
                        {
                            InputAppealNumber(user, "Success", "appeal");

                            string IsSendAppeal = System.Configuration.ConfigurationManager.AppSettings["IsSendAppeal"].ToString();
                            if (IsSendAppeal == "Yes")
                            {
                                try
                                {
                                    if (ftype == 1 || ftype == 5 || ftype == 6)//发送数据给风控目前只要密码（1）和更换关联手机（5） 2012/4/28 新添fstate=9，ftype=6通知风控2次通过“短信撤销状态”的申诉
                                    {
                                        fuin = System.Web.HttpUtility.UrlEncode(fuin, System.Text.Encoding.GetEncoding("GB2312"));
                                        string ftypestr;
                                        //2014/07/11 xiuling有type=6未发风控才修改
                                        //if (fstate == 9)
                                        //    ftypestr = System.Web.HttpUtility.UrlEncode("6", System.Text.Encoding.GetEncoding("GB2312"));
                                        //else
                                        ftypestr = System.Web.HttpUtility.UrlEncode(ftype.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        //string fstatestr = System.Web.HttpUtility.UrlEncode(fstate.ToString(),System.Text.Encoding.GetEncoding("GB2312"));
                                        string cre_id = System.Web.HttpUtility.UrlEncode(dr["cre_id"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string clear_ppsstr = System.Web.HttpUtility.UrlEncode(dr["clear_pps"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string is_answer = System.Web.HttpUtility.UrlEncode(dr["labIsAnswer"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        mobile_no = System.Web.HttpUtility.UrlEncode(mobile_no, System.Text.Encoding.GetEncoding("GB2312"));
                                        email = System.Web.HttpUtility.UrlEncode(email, System.Text.Encoding.GetEncoding("GB2312"));
                                        string score = System.Web.HttpUtility.UrlEncode(dr["score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string is_pass = System.Web.HttpUtility.UrlEncode(dr["IsPass"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        client_ip = System.Web.HttpUtility.UrlEncode(client_ip, System.Text.Encoding.GetEncoding("GB2312"));
                                        string reason = System.Web.HttpUtility.UrlEncode(dr["reason"].ToString().Trim(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string fcomment = System.Web.HttpUtility.UrlEncode(Fcomment, System.Text.Encoding.GetEncoding("GB2312")); //备注
                                        string fcheck_info = System.Web.HttpUtility.UrlEncode("", System.Text.Encoding.GetEncoding("GB2312"));   //拒绝原因
                                        string fsubmittime = System.Web.HttpUtility.UrlEncode(dr["FSubmitTime"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string fchecktime = System.Web.HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Text.Encoding.GetEncoding("GB2312"));

                                        string Data = "fuin=" + fuin + "&ftype=" + ftypestr + "&fstate=1&cre_id=" + cre_id + "&clear_pps=" + clear_ppsstr +
                                            "&is_answer=" + is_answer + "&mobile_no=" + mobile_no + "&email=" + email + "&score=" + score + "&is_pass=" + is_pass +
                                            "&client_ip=" + client_ip + "&fsubmittime=" + fsubmittime + "&fchecktime=" + fchecktime + "&reason=" + reason + "&fcomment=" + fcomment + "&fcheck_info=" + fcheck_info;
                                        Data = System.Web.HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                                        Data = "protocol=user_appeal_notify&version=1.0&data=" + Data;

                                        CFT.Apollo.Logging.LogHelper.LogInfo("user_appeal_notify send:" + Data);

                                        System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                                        byte[] sendBytes = GB2312.GetBytes(Data);

                                        string IP = System.Configuration.ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                                        string PORT = System.Configuration.ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                                        TENCENT.OSS.CFT.KF.Common.UDP.GetUDPReplyNoReturnNotReturn(sendBytes, IP, Int32.Parse(PORT));
                                    }
                                }
                                catch
                                {
                                }
                            }
                            return true;
                        }
                        else
                        {
                            msg = "更新原有记录出错,此记录不存在或原始状态不正确";
                            LogHelper.LogInfo("ConfirmAppeal:" + msg);
                            return false;
                        }
                    }
                }
                else
                {
                    msg = "查找记录失败.";
                    LogHelper.LogInfo("ConfirmAppeal:" + msg);
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                LogHelper.LogInfo("ConfirmAppeal:" + msg);
                return false;
            }
            finally
            {
                LogHelper.LogInfo("申诉通过报错:" + msg);
                da.Dispose();
            }
        }

        private bool IsBindMobilePhone(string Fuid)
        {
            bool bState = false;
            using (var da = MySQLAccessFactory.GetMySQLAccess("MN"))
            {
                
                da.OpenConn();
                string strTable = "msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_"
                    + Fuid.Substring(Fuid.Length - 1, 1);
                string sql = " select Fstatus from " + strTable + " where Fuid = '" + Fuid + "'";
                DataSet ds = da.dsGetTotalData(sql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int nState = int.Parse(ds.Tables[0].Rows[0]["Fstatus"].ToString());
                    string strMeg = string.Format("IsBindMobilePhone:nState={0}", nState);
                    LogHelper.LogInfo(strMeg);
                    int nBit = 64;
                    if ((nState & nBit) == nBit)
                    {
                        bState = true;
                    }

                }
            }
            return bState;

        }

        public bool ConfirmAppealDBTB(string fid, string db, string tb, string Fcomment, string user, string userIP)
        {
            string msg = "";

            MySqlAccess da = MySQLAccessFactory.GetMySQLAccess("CFTNEW_DB");
            try
            {
                da.OpenConn();

                if (Fcomment != null && Fcomment.Trim() != "")
                {
                    Fcomment = PublicRes.replaceMStr(Fcomment);
                }

                //先取出一些需要的信息 当前状态,修改类别, QQ号,email
                //修改姓名需要新姓名. 取回支付密码需要clear_pps, 注销不需要多余东西
                string strSql = "select * from " + db + "." + tb + " where FID='" + fid + "'";
                DataSet ds = da.dsGetTotalData(strSql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    #region
                    HandleParameterByDBTB(ds, true);

                    DataRow dr = ds.Tables[0].Rows[0];
                    int fstate = Int32.Parse(dr["FState"].ToString());
                    int ftype = Int32.Parse(dr["FType"].ToString());
                    string fuin = dr["FUin"].ToString();
                    string email = dr["Femail"].ToString();
                    string question1 = dr["question1"].ToString();
                    string answer1 = dr["answer1"].ToString();
                    string cont_type = dr["cont_type"].ToString();
                    string mobile_no = dr["mobile_no"].ToString();
                    string client_ip = dr["client_ip"].ToString();
                 //   string mobile_bind = dr["FMobile"].ToString();//绑定手机号,取到传给风控
                    string certno = dr["cre_id"].ToString();

                    if (fstate == 1 || fstate == 7)
                    {
                        msg = "原记录的状态不正确";
                        return false;
                    }

                    string nowtime = PublicRes.strNowTimeStander;
                    string new_name = "";
                    int clear_pps = 0;

                    string submittime = DateTime.Parse(dr["FSubmitTime"].ToString()).ToString("yyyy年MM月dd日");

                    //读取现在的用户名？furion 20060902
                    string username = AccountData.GetUserNameFromUin(fuin);
                    //简化注册用户这里处理下
                    if ((username == null || username.Trim() == "") && ftype == 6)
                    {
                        username = "亲爱的财付通客户";
                    }
                    if (username == null || username.Trim() == "")
                    {
                        msg = "取原有用户名时出错";
                        //return false;
                    }

                    string p3 = "";
                    string p4 = "";

                    //					if (!PublicRes.ReleaseCache(fuin,"qqid"))
                    //					{
                    //						Common.CommLib.commRes.sendLog4Log("QUeryInfo.ConfirmAppeal","通过自助申诉时，预清除用户"+ fuin + "cache失败！请检查！");
                    //						msg = "通过自助申诉时，预清除用户"+ fuin + "cache失败！请检查！";
                    //						return false;
                    //					}
                    //进行处理.	 //发送邮件
                    if (ftype == 1 || ftype == 11)
                    {
                        //找回密码
                        clear_pps = Int32.Parse(dr["clear_pps"].ToString());

                        bool IsNew; //是否新流程
                        if (cont_type != "") //cont_type为新加字段，如果为空就说明走老流程
                        {
                            IsNew = true;

                            if (cont_type == "1" )//前端会传回1或者3,3的时候不需要客服绑定
                            {
                                string Fuid = AccountData.ConvertToFuid(fuin);
                                string client_id = System.Configuration.ConfigurationManager.AppSettings["client_id"].ToString();
                                string singed = PublicRes.SingedByService("client_id=" + client_id + "&cmd=update_mobile&mobile=" + mobile_no + "&uid=" + Fuid);
                               
                                //1.找回密码、完整及简化版更换手机都要发 user_appeal_notify 通知
                                //2.涉及到绑定、更换手机，要发验证、通知
                                string old_mobile = "";
                                if (!BindOrChangeMobile(Fuid, fuin, old_mobile, mobile_no, client_ip, certno, singed, out msg))
                                    return false;
                            }
                        }
                        else
                        {
                            IsNew = false;
                        }

                        if (clear_pps == 1)
                        {
                            //替换密保问题和答案
                            if (!CheckQuestion(fuin, userIP, nowtime, question1, answer1, true, out msg)) return false;
                        }

                        string url = System.Configuration.ConfigurationManager.AppSettings["GetPassWKeyEmailUrl"].ToString();
                        string urlQQtips = System.Configuration.ConfigurationManager.AppSettings["GetPassWKeyTipsUrl"].ToString();
                        string urlMessage = System.Configuration.ConfigurationManager.AppSettings["GetPassWKeyMesUrl"].ToString();
                        //发邮件、短信、QQTips (string qqid, int ftype, string url, bool issucc, out string msg)
                        if (!SendAppealMailNew(email, ftype, true, username, url, url, url, "", "", fuin, out msg))
                        {
                            msg = "发送邮件失败：" + msg;
                            return false;
                        }
                        if (!SendAppealQQTips(fuin, ftype, urlQQtips, true, out msg))
                        {
                            msg = "发送QQTips失败：" + msg;
                            return false;
                        }
                        if (!SendAppealMessage(mobile_no, ftype, urlMessage, true, "", out msg))
                        {
                            msg = "发送短信失败：" + msg;
                            return false;
                        }
                    }

                    else if (ftype == 5 || ftype == 6)
                    {
                        //修改密保
                        clear_pps = Int32.Parse(dr["clear_pps"].ToString());

                        //ftype=5,固定cont_type=3
                        if (cont_type == "3")
                        {
                            string Fuid = AccountData.ConvertToFuid(fuin);
                            string client_id = System.Configuration.ConfigurationManager.AppSettings["client_id"].ToString();
                            string singed = PublicRes.SingedByService("client_id=" + client_id + "&cmd=update_mobile&mobile=" + mobile_no + "&uid=" + Fuid);

                            string old_mobile =GetOldBindMobile(Fuid, out msg);
                            if (msg != "")
                                return false;
                            if (string.IsNullOrEmpty(old_mobile.Trim()))
                            {
                                msg = "原绑定手机为：" + old_mobile;
                                return false;
                            }

                            if (!BindOrChangeMobile(Fuid, fuin, old_mobile, mobile_no, client_ip, certno, singed, out msg))
                                return false;
                        }
                        else
                        {
                            msg = "cont_type不为3";
                            return false;
                        }

                        if (clear_pps == 1)
                        {
                            //替换密保问题和答案  lxl 20131226 只有完整版和修改支付密码有改密保
                            if (!CheckQuestion(fuin, userIP, nowtime, question1, answer1, true, out msg)) return false;
                        }

                        //发邮件、短信(string qqid, int ftype, string url, bool issucc, out string msg) 
                        if (mobile_no == "" || mobile_no.Length != 11)
                        {
                            msg = "mobile不符合规范，mobile=" + mobile_no;
                            return false;
                        }
                        string noticeMobile = mobile_no.Substring(0, 3) + "******" + mobile_no.Substring(9, 2);
                        if (!SendAppealMailNew(email, ftype, true, username, noticeMobile, "", "", "", "", fuin, out msg))
                        {
                            msg = "发送邮件失败：" + msg;
                            return false;
                        }
                        if (!SendAppealMessage(mobile_no, ftype, "", true, "", out msg))
                        {
                            msg = "发送短信失败：" + msg;
                            return false;
                        }
                    }
                    else
                    {
                        msg = "请求类型不正确";
                        return false;
                    }

                    #region
                    if (fstate == 0)
                    {
                        strSql = "update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=0";
                    }
                    else if (fstate == 2)
                    {
                        strSql = "update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='二次审核,拒绝转审核通过." + Fcomment + "', FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=2";
                    }
                    else if (fstate == 3)
                    {
                        strSql = "update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "', FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=3";
                    }
                    else if (fstate == 4)
                    {
                        strSql = " update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=4";
                    }
                    else if (fstate == 5)
                    {
                        strSql = "update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=5";
                    }
                    else if (fstate == 6)
                    {
                        strSql = " update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=6";
                    }
                    else if (fstate == 8)
                    {
                        strSql = " update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=8";
                    }
                    // 2012/4/25 添加允许通过短信撤销状态的申诉！，并添加相应的风控通知！
                    else if (fstate == 9)
                    {
                        strSql = " update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=9";
                    }
                    else if (fstate == 11)//待补充资料,20150429，增加通过
                    {
                        strSql = " update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=11";
                    }
                    else if (fstate == 12)//已补充资料
                    {
                        strSql = " update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=12";
                    }
                    else
                    {
                        msg = "更新原有记录出错,此记录原始状态不正确";
                        return false;
                    }
                    if (da.ExecSqlNum(strSql) == 1)
                    {
                        InputAppealNumber(user, "Success", "appeal");

                        string IsSendAppeal = System.Configuration.ConfigurationManager.AppSettings["IsSendAppeal"].ToString();
                        if (IsSendAppeal == "Yes")
                        {
                            try
                            {
                                if (ftype == 1 || ftype == 5 || ftype == 6)//发送数据给风控目前只要密码（1）和更换关联手机（5） 2012/4/28 新添fstate=9，ftype=6通知风控2次通过“短信撤销状态”的申诉
                                {
                                    fuin = System.Web.HttpUtility.UrlEncode(fuin, System.Text.Encoding.GetEncoding("GB2312"));
                                    string ftypestr;
                                    //if (fstate == 9)
                                    //    ftypestr = System.Web.HttpUtility.UrlEncode("6", System.Text.Encoding.GetEncoding("GB2312"));
                                    //else
                                    ftypestr = System.Web.HttpUtility.UrlEncode(ftype.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    //string fstatestr = System.Web.HttpUtility.UrlEncode(fstate.ToString(),System.Text.Encoding.GetEncoding("GB2312"));
                                    string cre_id = System.Web.HttpUtility.UrlEncode(dr["cre_id"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string clear_ppsstr = System.Web.HttpUtility.UrlEncode(dr["clear_pps"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string is_answer = System.Web.HttpUtility.UrlEncode(dr["labIsAnswer"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    mobile_no = System.Web.HttpUtility.UrlEncode(mobile_no, System.Text.Encoding.GetEncoding("GB2312"));
                                    email = System.Web.HttpUtility.UrlEncode(email, System.Text.Encoding.GetEncoding("GB2312"));
                                    string score = System.Web.HttpUtility.UrlEncode(dr["score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string is_pass = System.Web.HttpUtility.UrlEncode(dr["IsPass"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    client_ip = System.Web.HttpUtility.UrlEncode(client_ip, System.Text.Encoding.GetEncoding("GB2312"));
                                    string reason = System.Web.HttpUtility.UrlEncode(dr["reason"].ToString().Trim(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string fcomment = System.Web.HttpUtility.UrlEncode(Fcomment, System.Text.Encoding.GetEncoding("GB2312")); //备注
                                    string fcheck_info = System.Web.HttpUtility.UrlEncode("", System.Text.Encoding.GetEncoding("GB2312"));   //拒绝原因
                                    string fsubmittime = System.Web.HttpUtility.UrlEncode(dr["FSubmitTime"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string fchecktime = System.Web.HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Text.Encoding.GetEncoding("GB2312"));

                                    string Data = "fuin=" + fuin + "&ftype=" + ftypestr + "&fstate=1&cre_id=" + cre_id + "&clear_pps=" + clear_ppsstr +
                                        "&is_answer=" + is_answer + "&mobile_no=" + mobile_no + "&email=" + email + "&score=" + score + "&is_pass=" + is_pass +
                                        "&client_ip=" + client_ip + "&fsubmittime=" + fsubmittime + "&fchecktime=" + fchecktime + "&reason=" + reason + "&fcomment=" + fcomment + "&fcheck_info=" + fcheck_info;
                                    Data = System.Web.HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                                    Data = "protocol=user_appeal_notify&version=1.0&data=" + Data;

                                    LogHelper.LogInfo("user_appeal_notify send:" + Data);

                                    System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                                    byte[] sendBytes = GB2312.GetBytes(Data);

                                    string IP = System.Configuration.ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                                    string PORT = System.Configuration.ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                                    TENCENT.OSS.CFT.KF.Common.UDP.GetUDPReplyNoReturnNotReturn(sendBytes, IP, Int32.Parse(PORT));
                                }
                            }
                            catch
                            {
                            }
                        }
                        return true;
                    }
                    else
                    {
                        msg = "更新原有记录出错,此记录不存在或原始状态不正确";
                        return false;
                    }
                    #endregion

                    #endregion
                }
                else
                {
                    msg = "查找记录失败.";
                    return false;
                }
            }
            catch (Exception err)
            {
                LogHelper.LogInfo("申诉通过异常:" + "Fid:" + fid + msg + err.Message);
                throw new Exception(msg + err.Message, err);
            }
            finally
            {
                if (msg != "")
                {
                    LogHelper.LogInfo("申诉通过报错:" + msg);
                }
                da.Dispose();
            }
        }

        public string GetOldBindMobile(string Fuid, out string Msg)
        {
            string old_mobile = "";
            Msg = "";
         //   MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("MN"));

            try
            {
                using (var da = MySQLAccessFactory.GetMySQLAccess("MN_DB"))
                {
                    da.OpenConn();
                    string sql = " select * from msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_" + Fuid.Substring(Fuid.Length - 1, 1) + " where Fuid = '" + Fuid + "'";
                    DataSet ds = da.dsGetTotalData(sql);

                    if (ds != null || ds.Tables.Count > 0 || ds.Tables[0].Rows.Count > 0)
                        old_mobile = ds.Tables[0].Rows[0]["Fmobile"].ToString();
                    return old_mobile;
                }
            }
            catch (Exception ex)
            {
                Msg = "获取旧绑定手机失败:" + ex.Message;
                return "";
            }
        }

        /// <summary>
        ///  发风控验证、绑定或更换手机、发风控通知
        /// </summary>
        public bool BindOrChangeMobile(string Fuid, string fuin, string old_mobile, string mobile_no, string client_ip, string certno, string singed, out string msg)
        {
            msg = "BindOrChangeMobile...";
            if (old_mobile == "")
            {
                //已绑定,就不需要再去绑定了
                if (IsBindMobilePhone(Fuid))
                {
                    string strMsg = string.Format("QQ号={0}手机号已经绑定了", fuin);
                    LogHelper.LogInfo(strMsg);
                    return true;
                }
            }
            else
            {
                //更改手机相同时
                if (old_mobile.Trim() == mobile_no.Trim())
                {
                    string strMsg = string.Format("QQ号={0}，更改手机号码相同", fuin);
                    LogHelper.LogInfo(strMsg);
                    return true;
                }

            }
            // 以下三步走
            //发验证

            if (!VerifyMobile(Fuid, fuin, old_mobile, mobile_no, client_ip, certno))
            {
                msg = "发风控change_mobile_verify验证不通过";
                return false;
            }

            if (old_mobile == "")
            {
                //绑定手机
                if (!CommMailSend.BindMobile(int.Parse(Fuid), mobile_no, singed, out msg))
                    return false;
            }
            else
            {
                //更新手机
                if (!CommMailSend.ChangeMobile(int.Parse(Fuid), mobile_no, singed, out msg))
                    return false;
            }

            //更新或绑定手机后，发风控通知
            try
            {
               SendMobile(Fuid, fuin, old_mobile, mobile_no, client_ip, certno);
            }
            catch { }
            return true;
        }

        private bool HandleParameter(DataSet ds, bool haveimg)
        {
            try
            {
                ds.Tables[0].Columns.Add("FTypeName", typeof(String));
                ds.Tables[0].Columns.Add("FStateName", typeof(String));

                ds.Tables[0].Columns.Add("cre_id", typeof(string)); //证件号码
                ds.Tables[0].Columns.Add("cre_type", typeof(string)); //证件类型
                ds.Tables[0].Columns.Add("cre_image", typeof(string)); //图片
                ds.Tables[0].Columns.Add("clear_pps", typeof(string)); //1为清除密保
                ds.Tables[0].Columns.Add("reason", typeof(string)); //原因
                ds.Tables[0].Columns.Add("old_name", typeof(string));
                ds.Tables[0].Columns.Add("new_name", typeof(string));
                ds.Tables[0].Columns.Add("old_company", typeof(string));
                ds.Tables[0].Columns.Add("new_company", typeof(string));

                //更换手机暂时加入一个字段,原请求证件地址
                ds.Tables[0].Columns.Add("cre_image2", typeof(string)); //图片
                ds.Tables[0].Columns.Add("question1", typeof(string)); //密保1
                ds.Tables[0].Columns.Add("answer1", typeof(string)); //答案1
                ds.Tables[0].Columns.Add("cont_type", typeof(string)); // 1: 手机 2: email//cont_type=1时,mobile_no有效 cont_type=2时,femail有效
                ds.Tables[0].Columns.Add("mobile_no", typeof(string));
                ds.Tables[0].Columns.Add("labIsAnswer", typeof(string)); //是否答对密保
                ds.Tables[0].Columns.Add("client_ip", typeof(string)); //申诉人IP
                ds.Tables[0].Columns.Add("score", typeof(string)); //实际得分
                ds.Tables[0].Columns.Add("standard_score", typeof(string)); //免审核标准分
                ds.Tables[0].Columns.Add("detail_score", typeof(string)); //得分明细
                ds.Tables[0].Columns.Add("IsPass", typeof(string)); //结果
                ds.Tables[0].Columns.Add("new_cre_id", typeof(string)); //新身份证
                ds.Tables[0].Columns.Add("risk_result", typeof(string)); //风控标记
                ds.Tables[0].Columns.Add("from", typeof(string)); //申诉渠道标记（手机）

                // 2012/4/28 修改，因为目前如果其中一个元素查找失败(可能是由于该帐号已经注销)，则会导致后面的dataRow查询失败！
                // 所以增加一个try catch模块，忽略查询失败的记录
                //加入一个证书库连接.
                using (var da = MySQLAccessFactory.GetMySQLAccess("CRT_DB"))
                {
                    da.OpenConn();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        try
                        {
                            dr["cre_id"] = "";
                            dr["cre_type"] = "";
                            dr["cre_image"] = "";
                            dr["clear_pps"] = "0";
                            dr["reason"] = "";
                            dr["old_name"] = "";
                            dr["new_name"] = "";
                            dr["old_company"] = "";
                            dr["new_company"] = "";
                            dr["question1"] = "";
                            dr["answer1"] = "";
                            dr["cont_type"] = "";
                            dr["mobile_no"] = "";
                            dr["labIsAnswer"] = "";
                            dr["client_ip"] = "";
                            dr["score"] = "";
                            dr["standard_score"] = "";
                            dr["detail_score"] = "";
                            dr["IsPass"] = "";
                            dr["new_cre_id"] = "";
                            dr["risk_result"] = "";
                            dr["from"] = "";

                            string fuin = dr["FUin"].ToString();

                            string fuid = AccountData.ConvertToFuid(fuin);
                            if (fuid == null || fuid.Trim() == "")
                            {
                                // 2012/4/28 修改，用户注销之后，应该是查不到相应的UID的
                                continue;
                                //return false;
                            }

                            string strSql = "select Fattach from " + PublicRes.GetTName("cft_digit_crt", "t_user_attr", fuid)
                                + " where Fuid=" + fuid + " and Fattr=1";

                            string strtmp = QueryInfo.GetString(da.GetOneResult(strSql));
                            dr["cre_image2"] = strtmp;

                            //string parameter = dr["FParameter"].ToString();
                            if (haveimg)
                            {
                                string parameter = GetOneParameter(dr["Fid"].ToString());

                                string[] paramlist = parameter.Split('&');

                                foreach (string param in paramlist)
                                {
                                    if (param.StartsWith("cre_id="))
                                    {
                                        dr["cre_id"] = PublicRes.getCgiString(param.Replace("cre_id=", ""));
                                    }
                                    else if (param.StartsWith("cre_type="))
                                    {
                                        dr["cre_type"] = PublicRes.getCgiString(param.Replace("cre_type=", ""));
                                    }
                                    else if (param.StartsWith("cre_image="))
                                    {
                                        dr["cre_image"] = PublicRes.getCgiString(param.Replace("cre_image=", ""));
                                    }
                                    else if (param.StartsWith("clear_pps="))
                                    {
                                        dr["clear_pps"] = PublicRes.getCgiString(param.Replace("clear_pps=", ""));
                                    }
                                    //						else if(param.StartsWith("email"))
                                    //						{
                                    //							dr["email"] = getCgiString(param.Replace("email=",""));
                                    //						}
                                    else if (param.StartsWith("reason="))
                                    {
                                        dr["reason"] = PublicRes.getCgiString(param.Replace("reason=", ""));
                                    }
                                    else if (param.StartsWith("old_name="))
                                    {
                                        dr["old_name"] = PublicRes.getCgiString(param.Replace("old_name=", ""));
                                    }
                                    else if (param.StartsWith("new_name="))
                                    {
                                        dr["new_name"] = PublicRes.getCgiString(param.Replace("new_name=", ""));
                                    }
                                    else if (param.StartsWith("old_company="))
                                    {
                                        dr["old_company"] = PublicRes.getCgiString(param.Replace("old_company=", ""));
                                    }
                                    else if (param.StartsWith("new_company="))
                                    {
                                        dr["new_company"] = PublicRes.getCgiString(param.Replace("new_company=", ""));
                                    }
                                    else if (param.StartsWith("question1="))
                                    {
                                        dr["question1"] = PublicRes.getCgiString(param.Replace("question1=", ""));
                                    }
                                    else if (param.StartsWith("answer1="))
                                    {
                                        dr["answer1"] = PublicRes.getCgiString(param.Replace("answer1=", ""));
                                    }
                                    else if (param.StartsWith("cont_type="))
                                    {
                                        dr["cont_type"] = PublicRes.getCgiString(param.Replace("cont_type=", ""));
                                    }
                                    else if (param.StartsWith("mobile_no="))
                                    {
                                        dr["mobile_no"] = PublicRes.getCgiString(param.Replace("mobile_no=", ""));
                                    }
                                    else if (param.StartsWith("ENV_ClientIp="))
                                    {
                                        dr["client_ip"] = PublicRes.getCgiString(param.Replace("ENV_ClientIp=", ""));
                                    }
                                    else if (param.StartsWith("score="))
                                    {
                                        dr["score"] = PublicRes.getCgiString(param.Replace("score=", ""));
                                    }
                                    else if (param.StartsWith("standard_score="))
                                    {
                                        dr["standard_score"] = PublicRes.getCgiString(param.Replace("standard_score=", ""));
                                    }
                                    else if (param.StartsWith("detail_score="))
                                    {
                                        dr["detail_score"] = PublicRes.getCgiString(param.Replace("detail_score=", ""));
                                    }
                                    else if (param.StartsWith("new_cre_id="))
                                    {
                                        dr["new_cre_id"] = PublicRes.getCgiString(param.Replace("new_cre_id=", ""));
                                    }
                                    else if (param.StartsWith("RISK_RESULT="))
                                    {
                                        string risk_result = PublicRes.getCgiString(param.Replace("RISK_RESULT=", ""));
                                        if (risk_result == "0")
                                            dr["risk_result"] = "";
                                        else if (risk_result == "1")
                                            dr["risk_result"] = "风控异常单无需人工回访用户";
                                        else if (risk_result == "2")
                                            dr["risk_result"] = "风控异常单需人工回访用户";
                                        else
                                            dr["risk_result"] = risk_result;
                                    }
                                    else if (param.StartsWith("from="))
                                    {
                                        dr["from"] = PublicRes.getCgiString(param.Replace("from=", ""));
                                    }

                                }
                            }

                            if (dr["detail_score"].ToString() != "")
                            {
                                try
                                {
                                    string detail_score = System.Web.HttpUtility.UrlDecode(dr["detail_score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));

                                    if (detail_score.IndexOf("PwdProtection") > -1)
                                    {
                                        detail_score = detail_score.Replace("PwdProtection", "密保校验得分");
                                    }
                                    if (detail_score.IndexOf("CertifiedId") > -1)
                                    {
                                        detail_score = detail_score.Replace("CertifiedId", "证件号校验得分");
                                    }
                                    if (detail_score.IndexOf("bind_email") > -1)
                                    {
                                        detail_score = detail_score.Replace("bind_email", "绑定邮箱校验得分");
                                    }
                                    if (detail_score.IndexOf("bind_mobile") > -1)
                                    {
                                        detail_score = detail_score.Replace("bind_mobile", "绑定手机校验得分");
                                    }
                                    if (detail_score.IndexOf("QQReceipt") > -1)
                                    {
                                        detail_score = detail_score.Replace("QQReceipt", "QQ申诉回执号得分");
                                    }
                                    if (detail_score.IndexOf("CertifiedBankCard") > -1)
                                    {
                                        detail_score = detail_score.Replace("CertifiedBankCard", "实名认证银行卡号校验得分");
                                    }
                                    if (detail_score.IndexOf("CreditCardPayHist") > -1)
                                    {
                                        detail_score = detail_score.Replace("CreditCardPayHist", "信用卡还款信息校验得分");
                                    }
                                    if (detail_score.IndexOf("WithdrawHist") > -1) //andrew 20110419
                                    {
                                        detail_score = detail_score.Replace("WithdrawHist", "提现记录得分");
                                    }
                                    if (detail_score.IndexOf("MBVerify") > -1)
                                    {
                                        detail_score = detail_score.Replace("MBVerify", "安平密保验证得分");
                                    }
                                    if (detail_score.IndexOf("MBQuery") > -1)
                                    {
                                        detail_score = detail_score.Replace("MBQuery", "通过安全中心密保得分");
                                    }
                                    if (detail_score.IndexOf("BindMobile") > -1)
                                    {
                                        detail_score = detail_score.Replace("BindMobile", "绑定的手机号码得分");
                                    }
                                    if (detail_score.IndexOf("Mobile") > -1)
                                    {
                                        detail_score = detail_score.Replace("Mobile", "手机得分");
                                    }
                                    if (detail_score.IndexOf("Email_QQ") > -1)
                                    {
                                        detail_score = detail_score.Replace("Email_QQ", "绑定QQ邮箱得分");
                                    }
                                    if (detail_score.IndexOf("Mobile_New") > -1)
                                    {
                                        detail_score = detail_score.Replace("Mobile_New", "未注册手机得分");
                                    }
                                    if (detail_score.IndexOf("QQReceipt_6") > -1)
                                    {
                                        detail_score = detail_score.Replace("QQReceipt_6", "简化注册用户QQ申诉回执单号得分");
                                    }
                                    dr["detail_score"] = detail_score;

                                }
                                catch
                                { }
                            }

                            string tmp = dr["FType"].ToString();
                            if (tmp == "1")    //目前只开放找回支付密码得分超标准分免审核，以后会慢慢开放的
                            {
                                dr["FTypeName"] = "找回密码";

                                if (dr["clear_pps"].ToString() == "0") //不清空
                                {
                                    string Sql = "uid=" + fuid;
                                    string errMsg = "";
                                    string answer1 = CommQuery.GetOneResultFromICE(Sql, CommQuery.QUERY_USERINFO, "Fanswer1", out errMsg);
                                    if (answer1 != null && answer1.Trim() != "")
                                    {
                                        if (dr["answer1"].ToString().Trim() == answer1.ToString().Trim())
                                            dr["labIsAnswer"] = "答对了";
                                        else
                                            dr["labIsAnswer"] = "答错了";
                                    }
                                }

                                try
                                {
                                    if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                        dr["IsPass"] = "Y";
                                    else
                                        dr["IsPass"] = "N";
                                }
                                catch
                                { }
                            }
                            else if (tmp == "2")
                            {
                                dr["FTypeName"] = "修改姓名";
                                try
                                {
                                    if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                        dr["IsPass"] = "Y";
                                    else
                                        dr["IsPass"] = "N";
                                }
                                catch
                                { }
                            }
                            else if (tmp == "3")
                            {
                                dr["FTypeName"] = "修改公司名";
                            }
                            else if (tmp == "4")
                            {
                                dr["FTypeName"] = "注销帐号";
                            }
                            else if (tmp == "5")// andrew 超标准分免审 20110419
                            {
                                dr["FTypeName"] = "完整注册用户更换关联手机";

                                try
                                {
                                    //IVR外呼专用furion  因为有高分单不会被领单,所以这里不用改动.
                                    if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                        dr["IsPass"] = "Y";
                                    else
                                        dr["IsPass"] = "N";
                                }
                                catch
                                { }
                            }
                            else if (tmp == "6")//andrew 20110419
                            {
                                dr["FTypeName"] = "简化注册用户更换绑定手机";
                                try
                                {
                                    if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                        dr["IsPass"] = "Y";
                                    else
                                        dr["IsPass"] = "N";
                                }
                                catch
                                { }
                            }
                            else if (tmp == "7")
                            {
                                dr["FTypeName"] = "更换证件号";
                                try
                                {
                                    if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                        dr["IsPass"] = "Y";
                                    else
                                        dr["IsPass"] = "N";
                                }
                                catch
                                { }
                            }
                            else if (tmp == "9")
                            {
                                dr["FTypeName"] = "手机令牌";
                            }
                            else if (tmp == "10")
                            {
                                dr["FTypeName"] = "第三方令牌";
                            }
                            else if (tmp == "0")
                            {
                                dr["FTypeName"] = "财付盾解绑";
                            }

                            tmp = dr["FState"].ToString();
                            if (tmp == "0")
                            {
                                dr["FStateName"] = "未处理";
                            }
                            else if (tmp == "1")
                            {
                                dr["FStateName"] = "申诉成功";
                            }
                            else if (tmp == "2")
                            {
                                dr["FStateName"] = "申诉失败";
                            }
                            else if (tmp == "3")
                            {
                                dr["FStateName"] = "大额待复核";
                            }
                            else if (tmp == "4")
                            {
                                dr["FStateName"] = "直接转后台";
                            }
                            else if (tmp == "5")
                            {
                                dr["FStateName"] = "异常转后台";
                            }
                            else if (tmp == "6")
                            {
                                dr["FStateName"] = "发邮件失败";
                            }
                            else if (tmp == "7")
                            {
                                dr["FStateName"] = "已删除";
                            }
                            else if (tmp == "8")
                            {
                                dr["FStateName"] = "已领单";
                            }
                            else if (tmp == "9")
                            {
                                dr["FStateName"] = "短信撤销申诉";
                            }
                        }
                        catch (System.Exception ex)
                        {
                            LogHelper.LogInfo("HandleParameter:" + ex.Message);
                        }
                    }
                }
                return true;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        private bool HandleParameterByDBTB(DataSet ds, bool haveimg)
        {
            try
            {
                ds.Tables[0].Columns.Add("FTypeName", typeof(String));
                ds.Tables[0].Columns.Add("FStateName", typeof(String));

                ds.Tables[0].Columns.Add("cre_id", typeof(string)); //证件号码
                ds.Tables[0].Columns.Add("cre_type", typeof(string)); //证件类型
                ds.Tables[0].Columns.Add("cre_image", typeof(string)); //图片
                ds.Tables[0].Columns.Add("clear_pps", typeof(string)); //1为清除密保
                ds.Tables[0].Columns.Add("reason", typeof(string)); //原因
                ds.Tables[0].Columns.Add("old_name", typeof(string));
                ds.Tables[0].Columns.Add("new_name", typeof(string));
                ds.Tables[0].Columns.Add("old_company", typeof(string));
                ds.Tables[0].Columns.Add("new_company", typeof(string));

                //更换手机暂时加入一个字段,原请求证件地址
                ds.Tables[0].Columns.Add("cre_image2", typeof(string)); //图片
                ds.Tables[0].Columns.Add("question1", typeof(string)); //密保1
                ds.Tables[0].Columns.Add("answer1", typeof(string)); //答案1
                ds.Tables[0].Columns.Add("cont_type", typeof(string)); // 1: 手机 2: email//cont_type=1时,mobile_no有效 cont_type=2时,femail有效
                ds.Tables[0].Columns.Add("mobile_no", typeof(string));
                ds.Tables[0].Columns.Add("labIsAnswer", typeof(string)); //是否答对密保
                ds.Tables[0].Columns.Add("client_ip", typeof(string)); //申诉人IP
                ds.Tables[0].Columns.Add("score", typeof(string)); //实际得分
                ds.Tables[0].Columns.Add("standard_score", typeof(string)); //免审核标准分
                ds.Tables[0].Columns.Add("detail_score", typeof(string)); //得分明细
                ds.Tables[0].Columns.Add("IsPass", typeof(string)); //结果
                ds.Tables[0].Columns.Add("new_cre_id", typeof(string)); //新身份证
                ds.Tables[0].Columns.Add("risk_result", typeof(string)); //风控标记
                ds.Tables[0].Columns.Add("from", typeof(string)); //申诉渠道标记（手机）

                // 风控冻结要求加入字段如下：
                /*
                ds.Tables[0].Columns.Add("email",typeof(string));		// 邮箱
                ds.Tables[0].Columns.Add("pkey",typeof(string));		// 验证邮件连接时传的PKEY
                ds.Tables[0].Columns.Add("contact_no",typeof(string));	// 用户联系电话
                ds.Tables[0].Columns.Add("otherImage1",typeof(string));	// 其他图片1
                ds.Tables[0].Columns.Add("otherImage2",typeof(string));	// 其他图片2
                ds.Tables[0].Columns.Add("otherImage3",typeof(string));	// 其他图片3
                ds.Tables[0].Columns.Add("otherImage4",typeof(string));	// 其他图片4
                ds.Tables[0].Columns.Add("otherImage5",typeof(string));	// 其他图片5
                ds.Tables[0].Columns.Add("rec_cftadd",typeof(string));	// 最近使用财付通的地址
                ds.Tables[0].Columns.Add("prove_banlance_image",typeof(string));	// 余额资金来源证明
                ds.Tables[0].Columns.Add("DC_timeaddress",typeof(string));	// 最近安装数字证书的时间和地址
                ds.Tables[0].Columns.Add("mobile",typeof(string));		// 绑定的手机号
                */
                using (var da = MySQLAccessFactory.GetMySQLAccess("CRT_DB"))
                {

                    da.OpenConn();

                    // 2012/4/28 修改，因为目前如果其中一个元素查找失败(可能是由于该帐号已经注销)，则会导致后面的dataRow查询失败！
                    // 所以增加一个try catch模块，忽略查询失败的记录
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        try
                        {
                            dr["cre_id"] = "";
                            dr["cre_type"] = "";
                            dr["cre_image"] = "";
                            dr["clear_pps"] = "0";
                            dr["reason"] = "";
                            dr["old_name"] = "";
                            dr["new_name"] = "";
                            dr["old_company"] = "";
                            dr["new_company"] = "";
                            dr["question1"] = "";
                            dr["answer1"] = "";
                            dr["cont_type"] = "";
                            dr["mobile_no"] = "";
                            dr["labIsAnswer"] = "";
                            dr["client_ip"] = "";
                            dr["score"] = "";
                            dr["standard_score"] = "";
                            dr["detail_score"] = "";
                            dr["IsPass"] = "";
                            dr["new_cre_id"] = "";
                            dr["risk_result"] = "";
                            dr["from"] = "";

                            string fuin = dr["FUin"].ToString();

                            string fuid = AccountData.ConvertToFuid(fuin);
                            if (fuid == null || fuid.Trim() == "")
                            {
                                // 2012/4/28 修改，用户注销之后，应该是查不到相应的UID的
                                continue;
                                //return false;
                            }

                            string strSql = "select Fattach from " + PublicRes.GetTName("cft_digit_crt", "t_user_attr", fuid)
                                + " where Fuid=" + fuid + " and Fattr=1";

                            string strtmp = QueryInfo.GetString(da.GetOneResult(strSql));
                            dr["cre_image2"] = strtmp;

                            //string parameter = dr["FParameter"].ToString();
                            if (haveimg)
                            {
                                dr["cre_id"] = PublicRes.getCgiString(dr["FCreId"].ToString());

                                dr["cre_type"] = PublicRes.getCgiString(dr["FCreType"].ToString());

                               // dr["cre_image"] = PublicRes.getCgiString(dr["FCreImg1"].ToString()) + "|" + PublicRes.getCgiString(dr["FCreImg2"].ToString());
                                dr["cre_image"] = PublicRes.getCgiString(dr["FCreImg1"].ToString()) + "|" + PublicRes.getCgiString(dr["FProveBanlanceImage"].ToString());
                                
                                dr["clear_pps"] = PublicRes.getCgiString(dr["FClearPps"].ToString());

                                dr["reason"] = PublicRes.getCgiString(dr["FAppealReason"].ToString());

                                dr["old_name"] = PublicRes.getCgiString(dr["FOldName"].ToString());

                                dr["new_name"] = PublicRes.getCgiString(dr["FNewName"].ToString());

                                dr["old_company"] = PublicRes.getCgiString(dr["FOldCompanyName"].ToString());

                                dr["new_company"] = PublicRes.getCgiString(dr["FNewCompanyName"].ToString());

                                dr["question1"] = PublicRes.getCgiString(dr["FMbQuestion"].ToString());

                                dr["answer1"] = PublicRes.getCgiString(dr["FMbAnswer"].ToString());

                                dr["cont_type"] = PublicRes.getCgiString(dr["FContType"].ToString());

                                dr["mobile_no"] = PublicRes.getCgiString(dr["FReservedMobile"].ToString());

                                dr["client_ip"] = PublicRes.getCgiString(dr["FIp"].ToString());

                                dr["score"] = PublicRes.getCgiString(dr["FAppealScore"].ToString()); ;

                                dr["standard_score"] = PublicRes.getCgiString(dr["FStandardScore"].ToString());

                                dr["detail_score"] = PublicRes.getCgiString(dr["FDetailScore"].ToString());

                                dr["new_cre_id"] = PublicRes.getCgiString(dr["FNewCreId"].ToString());

                                string risk_result = PublicRes.getCgiString(dr["FRiskState"].ToString());
                                if (risk_result == "0")
                                    dr["risk_result"] = "";
                                else if (risk_result == "1")
                                    dr["risk_result"] = "风控异常单无需人工回访用户";
                                else if (risk_result == "2")
                                    dr["risk_result"] = "风控异常单需人工回访用户";
                                else
                                    dr["risk_result"] = risk_result;

                                dr["from"] = PublicRes.getCgiString(dr["FChanel"].ToString());

                            }

                            if (dr["detail_score"].ToString() != "")
                            {
                                try
                                {
                                    string detail_score = System.Web.HttpUtility.UrlDecode(dr["detail_score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));

                                    if (detail_score.IndexOf("PwdProtection") > -1)
                                    {
                                        detail_score = detail_score.Replace("PwdProtection", "密保校验得分");
                                    }
                                    if (detail_score.IndexOf("CertifiedId") > -1)
                                    {
                                        detail_score = detail_score.Replace("CertifiedId", "证件号校验得分");
                                    }
                                    if (detail_score.IndexOf("bind_email") > -1)
                                    {
                                        detail_score = detail_score.Replace("bind_email", "绑定邮箱校验得分");
                                    }
                                    if (detail_score.IndexOf("bind_mobile") > -1)
                                    {
                                        detail_score = detail_score.Replace("bind_mobile", "绑定手机校验得分");
                                    }
                                    if (detail_score.IndexOf("QQReceipt") > -1)
                                    {
                                        detail_score = detail_score.Replace("QQReceipt", "QQ申诉回执号得分");
                                    }
                                    if (detail_score.IndexOf("CertifiedBankCard") > -1)
                                    {
                                        detail_score = detail_score.Replace("CertifiedBankCard", "实名认证银行卡号校验得分");
                                    }
                                    if (detail_score.IndexOf("CreditCardPayHist") > -1)
                                    {
                                        detail_score = detail_score.Replace("CreditCardPayHist", "信用卡还款信息校验得分");
                                    }
                                    if (detail_score.IndexOf("WithdrawHist") > -1) //andrew 20110419
                                    {
                                        detail_score = detail_score.Replace("WithdrawHist", "提现记录得分");
                                    }
                                    if (detail_score.IndexOf("MBVerify") > -1)
                                    {
                                        detail_score = detail_score.Replace("MBVerify", "安平密保验证得分");
                                    }
                                    if (detail_score.IndexOf("MBQuery") > -1)
                                    {
                                        detail_score = detail_score.Replace("MBQuery", "通过安全中心密保得分");
                                    }
                                    if (detail_score.IndexOf("BindMobile") > -1)
                                    {
                                        detail_score = detail_score.Replace("BindMobile", "绑定的手机号码得分");
                                    }
                                    if (detail_score.IndexOf("Mobile") > -1)
                                    {
                                        detail_score = detail_score.Replace("Mobile", "手机得分");
                                    }
                                    if (detail_score.IndexOf("Email_QQ") > -1)
                                    {
                                        detail_score = detail_score.Replace("Email_QQ", "绑定QQ邮箱得分");
                                    }
                                    if (detail_score.IndexOf("Mobile_New") > -1)
                                    {
                                        detail_score = detail_score.Replace("Mobile_New", "未注册手机得分");
                                    }
                                    if (detail_score.IndexOf("QQReceipt_6") > -1)
                                    {
                                        detail_score = detail_score.Replace("QQReceipt_6", "简化注册用户QQ申诉回执单号得分");
                                    }
                                    dr["detail_score"] = detail_score;

                                }
                                catch
                                { }
                            }

                            string tmp = dr["FType"].ToString();
                            if (tmp == "1")    //目前只开放找回支付密码得分超标准分免审核，以后会慢慢开放的
                            {
                                dr["FTypeName"] = "找回密码";

                                if (dr["clear_pps"].ToString() == "0") //不清空
                                {
                                    string Sql = "uid=" + fuid;
                                    string errMsg = "";
                                    string answer1 = CommQuery.GetOneResultFromICE(Sql, CommQuery.QUERY_USERINFO, "Fanswer1", out errMsg);
                                    if (answer1 != null && answer1.Trim() != "")
                                    {
                                        if (dr["answer1"].ToString().Trim() == answer1.ToString().Trim())
                                            dr["labIsAnswer"] = "答对了";//是否答对密保
                                        else
                                            dr["labIsAnswer"] = "答错了";
                                    }
                                }

                                try
                                {
                                    if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                        dr["IsPass"] = "Y";
                                    else
                                        dr["IsPass"] = "N";
                                }
                                catch
                                { }
                            }
                            else if (tmp == "2")
                            {
                                dr["FTypeName"] = "修改姓名";
                                try
                                {
                                    if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                        dr["IsPass"] = "Y";
                                    else
                                        dr["IsPass"] = "N";
                                }
                                catch
                                { }
                            }
                            else if (tmp == "3")
                            {
                                dr["FTypeName"] = "修改公司名";
                            }
                            else if (tmp == "4")
                            {
                                dr["FTypeName"] = "注销帐号";
                            }
                            else if (tmp == "5")// andrew 超标准分免审 20110419
                            {
                                dr["FTypeName"] = "完整注册用户更换关联手机";

                                try
                                {
                                    //IVR外呼专用furion  因为有高分单不会被领单,所以这里不用改动.
                                    if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                        dr["IsPass"] = "Y";
                                    else
                                        dr["IsPass"] = "N";
                                }
                                catch
                                { }
                            }
                            else if (tmp == "6")//andrew 20110419
                            {
                                dr["FTypeName"] = "简化注册用户更换绑定手机";
                                try
                                {
                                    if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                        dr["IsPass"] = "Y";
                                    else
                                        dr["IsPass"] = "N";
                                }
                                catch
                                { }
                            }
                            else if (tmp == "7")
                            {
                                dr["FTypeName"] = "更换证件号";
                                try
                                {
                                    if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                        dr["IsPass"] = "Y";
                                    else
                                        dr["IsPass"] = "N";
                                }
                                catch
                                { }
                            }
                            else if (tmp == "9")
                            {
                                dr["FTypeName"] = "手机令牌";
                            }
                            else if (tmp == "10")
                            {
                                dr["FTypeName"] = "第三方令牌";
                            }
                            else if (tmp == "0")
                            {
                                dr["FTypeName"] = "财付盾解绑";
                            }

                            tmp = dr["FState"].ToString();
                            if (tmp == "0")
                            {
                                dr["FStateName"] = "未处理";
                            }
                            else if (tmp == "1")
                            {
                                dr["FStateName"] = "申诉成功";
                            }
                            else if (tmp == "2")
                            {
                                dr["FStateName"] = "申诉失败";
                            }
                            else if (tmp == "3")
                            {
                                dr["FStateName"] = "大额待复核";
                            }
                            else if (tmp == "4")
                            {
                                dr["FStateName"] = "直接转后台";
                            }
                            else if (tmp == "5")
                            {
                                dr["FStateName"] = "异常转后台";
                            }
                            else if (tmp == "6")
                            {
                                dr["FStateName"] = "发邮件失败";
                            }
                            else if (tmp == "7")
                            {
                                dr["FStateName"] = "已删除";
                            }
                            else if (tmp == "8")
                            {
                                dr["FStateName"] = "已领单";
                            }
                            else if (tmp == "9")
                            {
                                dr["FStateName"] = "短信撤销申诉";
                            }
                        }
                        catch (System.Exception ex)
                        {

                        }
                    }
                }
                return true;
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
        }

        private string GetOneParameter(string fid)
        {
            try
            {
                string db = "db_appeal";
                try
                {
                    db = System.Configuration.ConfigurationManager.AppSettings["DB_CFT"].ToString();
                }
                catch
                {
                    db = "db_appeal";
                }
                using (var da = MySQLAccessFactory.GetMySQLAccess("CFT_DB"))
                {
                    da.OpenConn();
                    string strSql = "select FParameter from " + db + ".t_tenpay_appeal_trans where Fid='" + fid + "'";
                    return da.GetOneResult(strSql);
                }
            }
            catch
            {
                return "";
            }
        }

        private void InputAppealNumber(string User, string Type, string OperationType)
        {
            try
            {
                using (var da = MySQLAccessFactory.GetMySQLAccess("CFT_DB"))
                {
                    da.OpenConn();
                    string sql = "select count(1) from db_appeal.t_tenpay_appeal_kf_total where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType='" + OperationType + "'";
                    if (da.GetOneResult(sql) == "1")
                    {
                        if (Type == "Success")
                            sql = "update db_appeal.t_tenpay_appeal_kf_total set SuccessNum = SuccessNum + 1 where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType='" + OperationType + "'";
                        else if (Type == "Fail")
                            sql = "update db_appeal.t_tenpay_appeal_kf_total set FailNum = FailNum + 1 where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType='" + OperationType + "'";
                        else if (Type == "Delete")
                            sql = "update db_appeal.t_tenpay_appeal_kf_total set DeleteNum = DeleteNum + 1 where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType='" + OperationType + "'";
                        else
                            sql = "update db_appeal.t_tenpay_appeal_kf_total set OtherNum = OtherNum + 1 where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType='" + OperationType + "'";
                    }
                    else
                    {
                        if (Type == "Success")
                            sql = "insert into db_appeal.t_tenpay_appeal_kf_total(User,OperationDay,SuccessNum,OperationType) values('" + User + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "',1,'" + OperationType + "')";
                        else if (Type == "Fail")
                            sql = "insert into db_appeal.t_tenpay_appeal_kf_total(User,OperationDay,FailNum,OperationType) values('" + User + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "',1,'" + OperationType + "')";
                        else if (Type == "Delete")
                            sql = "insert into db_appeal.t_tenpay_appeal_kf_total(User,OperationDay,DeleteNum,OperationType) values('" + User + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "',1,'" + OperationType + "')";
                        else
                            sql = "insert into db_appeal.t_tenpay_appeal_kf_total(User,OperationDay,OtherNum,OperationType) values('" + User + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "',1,'" + OperationType + "')";
                    }

                    da.ExecSqlNum(sql);
                }
            }
            catch
            {
                throw new Exception("记录处理统计失败！");
            }
        }

        private bool SendAppealMail(string email, int ftype, bool issucc, string param1,
            string param2, string param3, string param4, string Reason, string OtherReason, string fuin, out string msg)
        {
            msg = "";

            if (email == null || email.Trim() == "")
            {
                //furion 20060902 是否支持不发邮件取决于这里.要么返回真,要么返回假
                return true;
            }

            string filename = System.Configuration.ConfigurationManager.AppSettings["ServicePath"].Trim();
            if (!filename.EndsWith("\\"))
                filename += "\\";

            string title = "";
            string actiontype = "";
            switch (ftype)
            {
                case 0:
                    {
                        title = "财付通申诉解绑财付盾通知！";
                        if (issucc)
                            filename += "UKeyYes.htm";
                        else
                            filename += "UKeyNo.htm";
                        break;
                    }
                case 1:
                case 11:
                    {
                        title = "财付通取回支付密码通知！";
                        if (issucc)
                        {
                            filename += "GetPwdYes.htm";
                            actiontype = "2031";
                        }
                        else
                        {
                            filename += "GetPwdNo.htm";
                            actiontype = "2032";
                        }
                        break;
                    }
                case 2:
                case 3:
                    {
                        title = "财付通修改真实姓名通知！";
                        if (issucc)
                        {
                            filename += "ChangeNameYes.htm";
                            actiontype = "2035";
                        }
                        else
                        {
                            filename += "ChangeNameNo.htm";
                            actiontype = "2036";
                        }
                        break;
                    }
                case 4:
                    {
                        title = "注销财付通帐户的通知！";
                        if (issucc)
                        {
                            filename += "DelUserYes.htm";
                            actiontype = "2034";
                        }
                        else
                        {
                            filename += "DelUserNo.htm";
                            actiontype = "2043";
                        }
                        break;
                    }
                case 5:
                case 6://简化注册用户换手机 andrew 20110419
                    {
                        title = "财付通关联手机更换申诉通知！";
                        if (issucc)
                        {
                            filename += "ChangeMobileYes.htm";
                            actiontype = "2027";
                        }
                        else
                        {
                            filename += "ChangeMobileNo.htm";
                            actiontype = "2037";
                        }
                        break;
                    }
                case 7://更换身份证号
                    {
                        title = "财付通提醒您！";
                        if (issucc)
                        {
                            filename += "ChangeCreidYes.htm";
                            actiontype = "2041";
                        }
                        else
                        {
                            filename += "ChangeCreidNo.htm";
                            actiontype = "2042";
                        }
                        break;
                    }
                case 9://手机令牌
                    {
                        title = "财付通提醒您！";
                        if (issucc)
                        {
                            filename += "MobileCommandYes.htm";
                            actiontype = "2029";
                        }
                        else
                        {
                            filename += "MobileCommandNo.htm";
                            actiontype = "2030";
                        }
                        break;
                    }
                case 10://第三方令牌
                    {
                        title = "财付通提醒您！";
                        if (issucc)
                        {
                            filename += "MobileCommandYes.htm";
                            actiontype = "2029";
                        }
                        else
                        {
                            filename += "MobileCommandNo.htm";
                            actiontype = "2030";
                        }
                        break;
                    }
                default:
                    {
                        msg = "给出类型不对";
                        return false;
                    }
            }

            try
            {
                if (!issucc)
                {
                    if (Reason == "")
                    {
                        Reason = "- " + OtherReason;
                    }
                    else
                    {
                        Reason = Reason.Substring(0, Reason.Length - 1);
                        string[] ReasonDetail = Reason.Split('&');
                        Reason = "";
                        // 2012/4/18 添加申诉原因
                        for (int i = 0; i < ReasonDetail.Length; i++)
                        {
                            if (ReasonDetail[i] == "特殊申诉找回地址")
                            {
                                string specialAppealFindBack = @"请您重新提交申述表时，上传账户资金来源截图。请参考“<a href='http://help.tenpay.com/cgi-bin/helpcenter/help_center.cgi?id=2232&type=0'>特殊申诉找回</a>”指引";
                                Reason += "- " + specialAppealFindBack + "<br>";
                            }
                            else
                            {
                                Reason += "- " + ReasonDetail[i].ToString() + "<br>";
                            }
                        }
                        if (OtherReason != "")
                        {
                            Reason += "- " + OtherReason + "<br>";
                        }
                    }
                }

                if (PublicRes.IgnoreLimitCheck)
                    return true;
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }

            if (ftype == 0)
            {
                //使用老的邮件发送方式
                StreamReader sr = new StreamReader(filename, System.Text.Encoding.GetEncoding("GB2312"));
                try
                {
                    string content = sr.ReadToEnd();
                    content = String.Format(content, param1, fuin, param2, param3, param4, Reason);
                    TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend newMail = new TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend();
                    newMail.SendMail(email, "", title, content, true, null);
                    return true;
                }
                catch (Exception err)
                {
                    msg = err.Message;
                    return false;
                }
                finally
                {
                    sr.Close();
                }


            }
            else
            {
                //yinhuang 2031/8/7
                string str_params = "p_name=" + param1 + "&p_parm1=" + param2 + "&p_parm2=" + param3 + "&p_parm3=" + param4 + "&p_parm4=" + Reason;
                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(email, actiontype, str_params);

                return true;
            }
        }

        private bool SendAppealMessage(string mobile, int ftype, string url, bool issucc, string reason, out string msg)
        {
            msg = "";

            if (mobile == null || mobile.Trim() == "")
            {
                // 是否支持不发短信取决于这里.要么返回真,要么返回假
                return false;
            }
            string noticeMobile = "";//短信中手机号，屏蔽中间6位
            string actiontype = "";
            switch (ftype)
            {

                case 1:
                case 11:
                    {
                        if (issucc)
                        {
                            actiontype = "2217";
                        }
                        else
                        {
                            actiontype = "2222";
                        }
                        break;
                    }
                case 5:
                case 6://简化注册用户换手机 
                    {
                        noticeMobile = mobile.Substring(0, 3) + "******" + mobile.Substring(9, 2);
                        if (issucc)
                        {
                            actiontype = "2223";
                        }
                        else
                        {
                            actiontype = "2224";
                        }
                        break;
                    }
                default:
                    {
                        msg = "给出类型不对";
                        return false;
                    }
            }
            try
            {
                if (!issucc)
                {
                    reason = reason.Substring(0, reason.Length - 1);
                    string[] ReasonDetail = reason.Split('&');
                    reason = "";
                    for (int i = 0; i < ReasonDetail.Length; i++)
                    {
                        if (i == 0)
                            reason += ReasonDetail[i].ToString();
                        else
                        {
                            if (ReasonDetail[i].ToString() != "")
                                reason += "；" + ReasonDetail[i].ToString();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }

            string str_params = "url=" + url + "&reason=" + reason + "&phoneNUM=" + noticeMobile;
            TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMessage(mobile, actiontype, str_params);
            return true;

        }

        //通过申诉用方法
        public bool BindMsgNotify(string Fqqid, bool IsMobile, string Mobile, bool IsMail, string Mail, string client_ip, out string BindMail, out string Msg)
        {
            /*转化为2进制(0为未开通,1为开通)不足7位前面补0,排序从最后一位开始
            1.是否开通短信提醒
            2.是否绑定email
            3.是否绑定qq
            4.是否激活
            5.动态验证玛(废弃)
            6.是否开通手机支付
            7.是否绑定手机
            消息通道开通标志
            0x00000001 //短信开通标志
            0x00000002 //email开通标志
            0x00000004 //tips开通标志
            0x00000008 //催费钱包消息开通标志
            0x00000010 //小钱包消息开通标志
            0x00000020 //wap支付开通标志，兼容老版本
            0x00000040 //手机绑定标志，兼容老版本
            0x00000080 //email 绑定标志
            0x00000100 //QQ绑定标志
            */
            Msg = "";
            BindMail = "";
            string Fuid = AccountData.ConvertToFuid(Fqqid);

            if (Fuid == null || Fuid == "")
            {
                Msg = "该帐号的内部ID为空,绑定失败!";
                return false;
            }

            if (!IsMobile && !IsMail)
            {
                Msg = "选择清空密保时,需要绑定手机或邮箱,绑定失败!";
                return false;
            }

            if ((IsMobile && Mobile == "") || (IsMail && Mail == ""))
            {
                Msg = "选择清空密保时,绑定项的值不能为空,绑定失败!";
                return false;
            }

            try
            {
                using (var da = MySQLAccessFactory.GetMySQLAccess("MN_DB"))
                {
                    da.OpenConn();
                    string sql = " select * from msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_" + Fuid.Substring(Fuid.Length - 1, 1) + " where Fuid = '" + Fuid + "'";
                    DataSet ds = da.dsGetTotalData(sql);

                    string strSql = "uid=" + Fuid;
                    strSql += "&modify_time=" + PublicRes.strNowTimeStander;
                    string Fstatus = "";
                    int NewFstatus = 0;
                    long timestamp = long.Parse(TimeTransfer.GetTickFromTime(PublicRes.strNowTimeStander));
                    string old_mobile = "";
                    int iresult;

                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        if (IsMobile)
                        {
                            //Fstatus = "000000000000000000000000" + "1" + "000000";  //手机
                            Fstatus = "000000000000000000000000" + "1" + "000001";  //手机 第一位表示打开短信提醒
                            NewFstatus = Convert.ToInt32(Fstatus, 2);

                            sql = "insert into msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_" + Fuid.Substring(Fuid.Length - 1, 1) + "(Fuid,Fmobile,Fstatus,Fregtime,Fupdatetime) " +
                                "values('" + Fuid + "','" + Mobile + "'," + NewFstatus + "," + timestamp + "," + timestamp + ")";

                            strSql += "&mobile=" + Mobile;
                        }
                        else  //只走邮箱
                        {
                            //Fstatus = "00000000000000000000000000000" + "1" + "0";  //邮箱
                            Fstatus = "00000000000000000000000100000" + "1" + "0";  //邮箱  第2位表示是否打开邮件通知状态,第8位邮箱绑定状态。

                            NewFstatus = Convert.ToInt32(Fstatus, 2);

                            sql = "insert into msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_" + Fuid.Substring(Fuid.Length - 1, 1) + "(Fuid,Femail,Fstatus,Fregtime,Fupdatetime) " +
                                "values('" + Fuid + "','" + Mail + "'," + NewFstatus + "," + timestamp + "," + timestamp + ")";

                            strSql += "&email=" + Mail;
                        }

                        da.ExecSqlNum(sql);

                        iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERINFO, out Msg);
                        if (iresult != 1)
                        {
                            Msg = "手机或邮箱绑定失败,基本信息表更新了非一条记录:" + Msg;
                            return false;
                        }
                    }
                    else
                    {
                        Fstatus = Convert.ToString(Convert.ToInt32(ds.Tables[0].Rows[0]["Fstatus"].ToString()), 2);
                        old_mobile = ds.Tables[0].Rows[0]["Fmobile"].ToString();

                        if (Fstatus.Length < 31)
                        {
                            Fstatus = Fstatus.PadLeft(31, '0');
                        }
                        if (Fstatus.Length != 31)
                        {
                            Msg = "清空密保时,记录状态数据异常,绑定失败!";
                            return false;
                        }

                        if (IsMobile)
                        {
                            //Fstatus = Fstatus.Substring(0,24) + "1" + Fstatus.Substring(25,6);  //手机
                            Fstatus = Fstatus.Substring(0, 24) + "1" + Fstatus.Substring(25, 5) + "1";  //手机
                            NewFstatus = Convert.ToInt32(Fstatus, 2);

                            sql = "update msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_" + Fuid.Substring(Fuid.Length - 1, 1) + " set Fstatus = " + NewFstatus +
                                ",Fmobile = '" + Mobile + "',Fupdatetime = " + timestamp + " where Fuid = '" + Fuid + "'";

                            strSql += "&mobile=" + Mobile;
                        }
                        else  //只走邮箱
                        {
                            //Fstatus = Fstatus.Substring(0,29) + "1" + Fstatus.Substring(30,1);
                            Fstatus = Fstatus.Substring(0, 23) + "1" + Fstatus.Substring(24, 5) + "1" + Fstatus.Substring(30, 1);//第2位表示是否打开邮件通知状态,第8位邮箱绑定状态。
                            NewFstatus = Convert.ToInt32(Fstatus, 2);

                            sql = "update msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_" + Fuid.Substring(Fuid.Length - 1, 1) + " set Fstatus = " + NewFstatus +
                                ",Femail = '" + Mail + "',Fupdatetime = " + timestamp + " where Fuid = '" + Fuid + "'";

                            strSql += "&email=" + Mail;
                        }

                        da.ExecSqlNum(sql);

                        iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERINFO, out Msg);
                        if (iresult != 1)
                        {
                            Msg = "手机或邮箱绑定失败,基本信息表更新了非一条记录:" + Msg;
                            return false;
                        }
                    }

                    if (IsMail)
                    {
                        BindMail = Mail;
                    }
                    else if (IsMobile)
                    {
                        if (Fqqid.IndexOf("@") > -1)
                            BindMail = Fqqid;
                        else if (ds.Tables[0].Rows.Count > 0)
                            BindMail = ds.Tables[0].Rows[0]["Femail"].ToString();

                        try
                        {
                            SendMobile(Fuid, Fqqid, old_mobile, Mobile, client_ip,"");
                        }
                        catch { }
                    }
                    return true;

                }
            }
            catch (Exception ex)
            {
                Msg = "手机或邮箱绑定失败:" + ex.Message;
                return false;
            }
        }

        //绑定或更换手机前，发风控验证
        public bool VerifyMobile(string uid, string uin, string old_mobile, string new_mobile, string client_ip, string certno)
        {
            try
            {
              //  string hdId = CommUtil.GetHardDiskId();取不到用户的所以传空
              //  string mac = CommUtil.GetNetworkMAC();
                string Data = "purchaser_uid=" + uid + "&purchaser_id=" + uin + "&old_mobile=" + old_mobile 
                    + "&new_mobile=" + new_mobile + "&client_ip=" + client_ip +
                    "&cookie=&change_way=2&diskid=&macaddr=&certno=" + certno + "&crt_state=0";
                Data = HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                Data = "protocol=change_mobile_verify&version=1.0&data=" + Data;

                LogHelper.LogInfo("change_mobile_verify send:" + Data);

                System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                byte[] sendBytes = GB2312.GetBytes(Data);

                string IP = ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                string PORT = ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                byte[] answer =UDP.GetUDPReply(sendBytes, IP, Int32.Parse(PORT));
                string answerStr = Encoding.Default.GetString(answer);//  result=0  验证通过。允许修改

                CFT.Apollo.Logging.LogHelper.LogInfo("change_mobile_verify get:" + answerStr);

                if (answerStr.IndexOf("result=0") < 0)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        //绑定或更换手机后，发风控通知
        public void SendMobile(string uid, string uin, string old_mobile, string new_mobile, string client_ip, string certno)
        {
            try
            {
               // string hdId = CommUtil.GetHardDiskId();
              //  string mac = CommUtil.GetNetworkMAC();
                string Data = "purchaser_uid=" + uid + "&purchaser_id=" + uin + "&old_mobile=" + old_mobile 
                    + "&new_mobile=" + new_mobile + "&client_ip=" + client_ip 
                    + "&cookie=&change_way=2&diskid=&macaddr=&certno=" + certno;
                Data = HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                Data = "protocol=change_mobile_notify&version=1.0&data=" + Data;

                CFT.Apollo.Logging.LogHelper.LogInfo("change_mobile_notify send:" + Data);

                System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                byte[] sendBytes = GB2312.GetBytes(Data);

                string IP = ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                string PORT = ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                TENCENT.OSS.CFT.KF.Common.UDP.GetUDPReplyNoReturn(sendBytes, IP, Int32.Parse(PORT));
            }
            catch
            {

            }

        }

        private bool GetNewPwd(string qqid, int clear_pps, string userip, string nowtime, string question1, string answer1, bool IsNew, out string msg)
        {
            msg = "";

            try
            {
                string pwd = PublicRes.makePwd();
                string pwdmd5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "md5").ToLower();

                string uid = AccountData.ConvertToFuid(qqid);

                // TODO: 1客户信息资料外移
                if (uid != null && uid.Trim() != "")
                {
                    string strSql = "uid=" + uid;
                    strSql += "&modify_time=" + PublicRes.strNowTimeStander;
                    strSql += "&passwd=" + pwdmd5;
                    strSql += "&pass_flag=0";
                    strSql += "&ip=" + userip;

                    if (clear_pps == 1)
                    {
                        if (IsNew)
                        {
                            strSql += "&question1=" + question1;
                            strSql += "&answer1=" + answer1;
                            strSql += "&question2=";
                            strSql += "&answer2=";

                            if (question1 == "" || answer1 == "")
                            {
                                msg = "密保问题或答案为空,操作失败";
                                return false;
                            }
                        }
                        else
                        {
                            strSql += "&question1=" + question1;
                            strSql += "&answer1=" + answer1;
                            strSql += "&question2=";
                            strSql += "&answer2=";
                        }
                    }

                    int iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERINFO, out msg);

                    if (iresult == 1)//测试加iresult == -5
                    {
                        msg = pwd;
                        //发送邮件吧.
                        return true;
                    }
                    else
                    {
                        msg = "执行更新密码操作时失败";
                        return false;
                    }
                }
                else
                {
                    msg = "获取内部ID失败.";
                    return false;
                }

            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
        }

        private bool UpdateUserName(string qqid, string new_name, bool iscompany, string userip, string nowtime, out string msg)
        {
            msg = "";

            if (new_name == null || new_name.Trim() == "")
            {
                msg = "新名称不能为空";
                return false;
            }

            new_name = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.replaceSqlStr(new_name);

            try
            {
                string uid = AccountData.ConvertToFuid(qqid);

                // TODO: 1客户信息资料外移
                if (uid != null && uid.Trim() != "")
                {
                    string strsetname = "&truename=" + new_name;
                    if (iscompany)
                        strsetname = "&company_name=" + new_name;

                    string systemtime = PublicRes.strNowTimeStander;

                    string strSql = "uid=" + uid;
                    strSql += "&modify_time=" + systemtime;
                    strSql += "&ip=" + userip;
                    strSql += strsetname;

                    int iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERINFO, out msg);

                    if (iresult != 1)
                        return false;


                    ICEAccess ice = ICEAccessFactory.GetICEAccess("ICEConnectionString");
                    try
                    {
                        ice.OpenConn();
                        string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + uid + "&");
                        strwhere += ICEAccess.URLEncode("fcurtype=" + 1 + "&");

                        string strUpdate = "data=" + ICEAccess.URLEncode("fip=" + userip);
                        if (iscompany)
                        {
                            strUpdate += ICEAccess.URLEncode("&fcompany_name=" + ICEAccess.URLEncode(new_name));
                        }
                        else
                        {
                            strUpdate += ICEAccess.URLEncode("&ftruename=" + ICEAccess.URLEncode(new_name));
                        }

                        strUpdate += ICEAccess.URLEncode("&fmodify_time=" + PublicRes.strNowTimeStander);

                        string strResp = "";
                        //3.0接口测试需要 furion 20090708
                        if (ice.InvokeQuery_Exec(YWSourceType.用户资源, YWCommandCode.修改用户信息, uid, strwhere + "&" + strUpdate, out strResp) != 0)
                        {
                            throw new Exception("修改用户信息时出错！" + strResp);
                        }

                        return true;
                    }
                    catch (Exception err)
                    {
                        msg = err.Message;
                        return false;
                    }
                    finally
                    {
                        ice.Dispose();
                    }
                }
                else
                {
                    msg = "获取内部ID失败.";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
        }

        private bool DelUser(string qqid, string email, string reason, string user, string userIP, string nowtime, out string msg)
        {
            msg = "";

            try
            {
                AccountData ad = new AccountData();
                if (!ad.LogOnUser(qqid, reason, user, userIP))
                {
                    msg += "销户操作失败！";
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                LogHelper.LogInfo("DelUser:" + msg);
                return false;
            }
        }

        private bool CheckQuestion(string qqid, string userip, string nowtime, string question1, string answer1, bool IsNew, out string msg)
        {
            msg = "";
            try
            {
                string uid = AccountData.ConvertToFuid(qqid);

                // TODO: 1客户信息资料外移
                if (uid != null && uid.Trim() != "")
                {


                    string strSql = "uid=" + uid;
                    strSql += "&modify_time=" + PublicRes.strNowTimeStander;
                    strSql += "&ip=" + userip;


                    if (IsNew)
                    {
                        strSql += "&question1=" + question1;
                        strSql += "&answer1=" + answer1;
                        strSql += "&question2=";
                        strSql += "&answer2=";

                        if (question1 == "" || answer1 == "")
                        {
                            msg = "密保问题或答案为空,操作失败";
                            LogHelper.LogInfo("CheckQuestion:" + msg);
                            return false;
                        }
                    }
                    else
                    {
                        strSql += "&question1=" + question1;
                        strSql += "&answer1=" + answer1;
                        strSql += "&question2=";
                        strSql += "&answer2=";
                    }


                    int iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERINFO, out msg);

                    if (iresult == 1)
                    {
                        msg = "密保问题和答案替换成功";
                        LogHelper.LogInfo("CheckQuestion:" + msg);
                        //发送邮件吧.
                        return true;
                    }
                    else
                    {
                        msg = "密保问题和答案替换失败";
                        LogHelper.LogInfo("CheckQuestion:" + msg);
                        return false;
                    }
                }
                else
                {
                    msg = "获取内部ID失败.";
                    LogHelper.LogInfo("CheckQuestion:" + msg);
                    return false;
                }

            }
            catch (Exception err)
            {
                msg = err.Message;
                LogHelper.LogInfo("CheckQuestion:" + msg);
                return false;
            }
        }

        //申诉新库表发邮件，只有1、5、6
        private bool SendAppealMailNew(string email, int ftype, bool issucc, string param1,
           string param2, string param3, string param4, string Reason, string OtherReason, string fuin, out string msg)
        {
            msg = "";

            if (email == null || email.Trim() == "")
            {
                //furion 20060902 是否支持不发邮件取决于这里.要么返回真,要么返回假
                return true;
            }

            string title = "";
            string actiontype = "";
            switch (ftype)
            {

                case 1:
                case 11:
                    {
                        title = "财付通取回支付密码通知！";
                        if (issucc)
                        {
                            actiontype = "2217";
                        }
                        else
                        {
                            actiontype = "2032";
                        }
                        break;
                    }

                case 5:
                case 6://简化注册用户换手机 
                    {
                        title = "财付通关联手机更换申诉通知！";
                        if (issucc)
                        {
                            actiontype = "2223";
                        }
                        else
                        {
                            actiontype = "2037";
                        }
                        break;
                    }
                default:
                    {
                        msg = "给出类型不对";
                        return false;
                    }
            }
            string str_params = "p_name=" + param1 + "&p_parm1=" + param2 + "&p_parm2=" + param3 + "&p_parm3=" + param4 + "&p_parm4=" + Reason;
            TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(email, actiontype, str_params);
            return true;

        }

        //申诉新库表发QQTips
        private bool SendAppealQQTips(string qqid, int ftype, string url, bool issucc, out string msg)
        {
            msg = "";

            if (qqid == null || qqid.Trim() == "")
            {
                // 是否支持不发QQtips取决于这里.要么返回真,要么返回假
                return true;
            }

            string actiontype = "";
            switch (ftype)
            {

                case 1:
                case 11:
                    {
                        if (issucc)
                        {
                            actiontype = "2217";
                        }
                        else
                        {
                            actiontype = "";
                        }
                        break;
                    }
                default:
                    {
                        return false;
                    }
            }
            string str_params = "url=" + url;
            TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsgQQTips(qqid, actiontype, str_params);
            return true;

        }
    }


    // 查已通过认证信息
    public class QueryUserAuthenedStateInfo : Query_BaseForNET
    {
        public QueryUserAuthenedStateInfo(string uin)
        {
            this.ICEcommand = "FINANCE_QUERY_USER_AUTHENED";
            string fuid = AccountData.ConvertToFuid(uin);
            this.ICESql = "uid=" + fuid;
        }
    }

    // 查实名认证中(过程表)信息
    public class QueryUserAuthenStateInfo : Query_BaseForNET
    {
        public QueryUserAuthenStateInfo(string userAcc, string bankID, int bankType)
        {
            if (userAcc.Trim() != "")
            {
                this.ICEcommand = "FINANCE_QUERY_USR_AUTHENING";

                this.ICESql = "qqid=" + userAcc;
            }
            else if (bankID.Trim() != "")
            {
                this.ICEcommand = "FINANCE_QUERY_CARD_AUTHENING";

                this.ICESql = "bank_id=" + bankID + "&bank_type=" + bankType;
            }
            else
            {
                throw new Exception("查询条件必须填写！");
            }
        }
    }

    #region 自助申诉处理类

    public class CFTUserAppealClass : Query_BaseForNET
    {
        private static string cftDB = ConfigurationManager.AppSettings["DB_CFT"];
        public CFTUserAppealClass(string fuin, string u_BeginTime, string u_EndTime, int fstate, int ftype, string QQType, string dotype, int SortType)
        {
            string strWhere = " where 1=1 ";

            if (fuin != null && fuin.Trim() != "")
            {
                strWhere += " and Fuin='" + fuin.Trim() + "' ";
            }

            if (u_BeginTime != null && u_BeginTime != "")
            {

                strWhere += " and FSubmitTime between '" + u_BeginTime + "' and '" + u_EndTime + "' ";
            }

            if (fstate != 99)
            {
                if (fstate == 10)   //直接申诉成功，即审核成功，且FCheckUser为system
                    strWhere += " and FState='1' and FCheckUser='system' ";
                else
                    strWhere += " and FState='" + fstate + "'  ";
            }


            if (ftype != 99)
            {
                strWhere += " and FType=" + ftype + " ";
            }
            else
            {
                // 8号申诉是风控冻结，有另外的处理入口，申诉处理暂不包含改类型的申诉
                strWhere += " and FType!=8 and FType!=19  and FType!=11";
            }

            if (QQType == "0")    //"" 所有类型; "0" 非会员; "1" 普通会员; "2" VIP会员
            {
                strWhere += " and (FParameter not like '%vip_flag=1%') and (FParameter not like '%vip_flag=2%') ";
            }
            else if (QQType == "1")
            {
                strWhere += " and FParameter like '%vip_flag=1%' ";
            }
            else if (QQType == "2")
            {
                strWhere += " and FParameter like '%vip_flag=2%' ";
            }

            //增加高分单低分单查询
            if (dotype == "1")
            {
                //高分
                strWhere += " and FParameter like '%&AUTO_APPEAL=1%' ";
            }
            else if (dotype == "0")
            {
                //低分
                strWhere += " and FParameter not like '%&AUTO_APPEAL=1%' ";
            }

            if (SortType != 99)
            {
                if (SortType == 0)   //排序：时间小到大
                    strWhere += " order by FSubmitTime asc ";
                if (SortType == 1)   //排序：时间大到小
                    strWhere += " order by FSubmitTime desc ";
            }

            //lxl 20131116加两列tableName、DBName
            fstrSql = "select '' as DBName, '' as tableName,Fid,FType,Fuin,FSubmitTime,FState,FCheckTime,FReCheckTime,Fpicktime,FReCheckUser,FCheckInfo,FCheckUser,FComment,Femail from " + cftDB + ".t_tenpay_appeal_trans "
                + strWhere;
            fstrSql_count = "select count(1) from " + cftDB + ".t_tenpay_appeal_trans " + strWhere;
        }



        public CFTUserAppealClass(string fuin, string u_BeginTime, string u_EndTime, int fstate, int ftype, string QQType,
          string pickUser, string fid, string szReason, string orderType, string table)
        {
            string strWhere = " where 1=1 ";

            if (fuin != null && fuin.Trim() != "")
            {
                strWhere += " and Fuin='" + fuin.Trim() + "' ";
            }

            if (u_BeginTime != null && u_BeginTime != "")
            {
                strWhere += " and FSubmitTime between '" + u_BeginTime + "' and '" + u_EndTime + "' ";
            }

            if (fstate != 99)
            {
                if (fstate == 10)   //直接申诉成功，即审核成功，且FCheckUser为system
                {
                    if (ftype == 8 || ftype == 19)
                    {
                        strWhere += " and FState='" + fstate + "'  ";
                    }
                    else
                    {
                        strWhere += " and FState='1' and FCheckUser='system' ";
                    }
                }
                else
                {
                    strWhere += " and FState='" + fstate + "'  ";
                }
            }

            if (ftype != 99)
            {
                strWhere += " and FType=" + ftype + " ";
            }
            if (QQType == "0")    //"" 所有类型; "0" 非会员; "1" 普通会员; "2" VIP会员
            {
                strWhere += " and (FParameter not like '%vip_flag=1%') and (FParameter not like '%vip_flag=2%') ";
            }
            else if (QQType == "1")
            {
                strWhere += " and FParameter like '%vip_flag=1%' ";
            }
            else if (QQType == "2")
            {
                strWhere += " and FParameter like '%vip_flag=2%' ";
            }
            if (pickUser != "")
            {
                strWhere += " and fpickuser like '%" + pickUser + "%' ";
            }
            if (fid != "")
            {
                strWhere += " and Fid='" + fid + "'";
            }
            if (szReason != "")
            {
                strWhere += " and FComment like '%" + szReason + "%' ";
            }

            // 默认按照提交时间最早来排
            //string orderStr = " order by date_format(FSubmitTime,'%Y%m%d'),FUin asc";
            string orderStr = " order by date_format(FSubmitTime,'%Y%m%d') asc";

            if (orderType == "2")
            {
                orderStr = "order by date_format(FSubmitTime,'%Y%m%d') desc";
            }

            fstrSql = "select Fid,FType,Fuin,FSubmitTime,FState,FCheckTime,Fpicktime,FCheckInfo,FCheckUser,FComment,Femail,FPickUser from " + table + " "
                + strWhere + orderStr;

            fstrSql_count = "select count(1) from " + table + " " + strWhere;
        }

        public CFTUserAppealClass(string fuin, string u_BeginTime, string u_EndTime, int fstate, int ftype, string QQType, string dotype, int SortType, bool mark, string db, string tb)
        {
            string strWhere = " where 1=1 ";

            if (fuin != null && fuin.Trim() != "")
            {
                strWhere += " and Fuin='" + fuin.Trim() + "' ";
            }

            if (u_BeginTime != null && u_BeginTime != "")
            {

                strWhere += " and FSubmitTime between '" + u_BeginTime + "' and '" + u_EndTime + "' ";
            }

            if (fstate != 99)
            {
                if (fstate == 10)   //直接申诉成功，即审核成功，且FCheckUser为system
                    strWhere += " and FState='1' and FCheckUser='system' ";
                else
                    strWhere += " and FState='" + fstate + "'  ";
            }


            if (ftype != 99)
            {
                strWhere += " and FType=" + ftype + " ";
            }
            else
            {
                // 8号申诉是风控冻结，有另外的处理入口，申诉处理暂不包含改类型的申诉
                strWhere += " and FType!=8  and FType!=19  and FType!=11";
            }

            if (QQType == "0")    //"" 所有类型; 0 非会员; 1 普通会员; 2 VIP会员
            {
                strWhere += " and FVipFlag<>1 and FVipFlag<>2 ";
            }
            else if (QQType == "1")
            {
                strWhere += " and FVipFlag=1 ";
            }
            else if (QQType == "2")
            {
                strWhere += " and FVipFlag=2 ";
            }

            //增加高分单低分单查询
            if (dotype == "1")
            {
                //高分
                strWhere += " and FAutoAppeal=1 ";
            }
            else if (dotype == "0")
            {
                //低分
                strWhere += " and FAutoAppeal<>1 ";
            }
            fstrSql = "";
            fstrSql_count = "";
            string str = " Fid,FType,Fuin,FSubmitTime,FState,FCheckTime,FReCheckTime,Fpicktime,FReCheckUser,FCheckInfo,FCheckUser,FComment,Femail from db_appeal_";

            if (int.Parse(tb) < 10)
                fstrSql += " select 'db_appeal_" + db + "' as DBName,'t_tenpay_appeal_trans_" + "0" + tb + "' as tableName," + str + db + ".t_tenpay_appeal_trans_" + "0" + tb + strWhere;
            else
                fstrSql += " select 'db_appeal_" + db + "' as DBName,'t_tenpay_appeal_trans_" + tb + "' as tableName," + str + db + ".t_tenpay_appeal_trans_" + tb + strWhere;
            fstrSql_count = "select count(1) from ( " + fstrSql + " ) as total";
        }

        public static bool HandleParameter(DataSet ds, bool haveimg)
        {
            //加入一个证书库连接.
            // MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CRT"));

            try
            {
                ds.Tables[0].Columns.Add("FTypeName", typeof(String));
                ds.Tables[0].Columns.Add("FStateName", typeof(String));

                ds.Tables[0].Columns.Add("cre_id", typeof(string)); //证件号码
                ds.Tables[0].Columns.Add("cre_type", typeof(string)); //证件类型
                ds.Tables[0].Columns.Add("cre_image", typeof(string)); //图片
                ds.Tables[0].Columns.Add("clear_pps", typeof(string)); //1为清除密保
                ds.Tables[0].Columns.Add("reason", typeof(string)); //原因
                ds.Tables[0].Columns.Add("old_name", typeof(string));
                ds.Tables[0].Columns.Add("new_name", typeof(string));
                ds.Tables[0].Columns.Add("old_company", typeof(string));
                ds.Tables[0].Columns.Add("new_company", typeof(string));

                //更换手机暂时加入一个字段,原请求证件地址
                ds.Tables[0].Columns.Add("cre_image2", typeof(string)); //图片
                ds.Tables[0].Columns.Add("question1", typeof(string)); //密保1
                ds.Tables[0].Columns.Add("answer1", typeof(string)); //答案1
                ds.Tables[0].Columns.Add("cont_type", typeof(string)); // 1: 手机 2: email//cont_type=1时,mobile_no有效 cont_type=2时,femail有效
                ds.Tables[0].Columns.Add("mobile_no", typeof(string));
                ds.Tables[0].Columns.Add("labIsAnswer", typeof(string)); //是否答对密保
                ds.Tables[0].Columns.Add("client_ip", typeof(string)); //申诉人IP
                ds.Tables[0].Columns.Add("score", typeof(string)); //实际得分
                ds.Tables[0].Columns.Add("standard_score", typeof(string)); //免审核标准分
                ds.Tables[0].Columns.Add("detail_score", typeof(string)); //得分明细
                ds.Tables[0].Columns.Add("IsPass", typeof(string)); //结果
                ds.Tables[0].Columns.Add("new_cre_id", typeof(string)); //新身份证
                ds.Tables[0].Columns.Add("risk_result", typeof(string)); //风控标记
                ds.Tables[0].Columns.Add("from", typeof(string)); //申诉渠道标记（手机）

                using (var da = MySQLAccessFactory.GetMySQLAccess("CRT_DB"))
                {
                    da.OpenConn();

                    // 2012/4/28 修改，因为目前如果其中一个元素查找失败(可能是由于该帐号已经注销)，则会导致后面的dataRow查询失败！
                    // 所以增加一个try catch模块，忽略查询失败的记录
                    #region
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        try
                        {
                            dr["cre_id"] = "";
                            dr["cre_type"] = "";
                            dr["cre_image"] = "";
                            dr["clear_pps"] = "0";
                            dr["reason"] = "";
                            dr["old_name"] = "";
                            dr["new_name"] = "";
                            dr["old_company"] = "";
                            dr["new_company"] = "";
                            dr["question1"] = "";
                            dr["answer1"] = "";
                            dr["cont_type"] = "";
                            dr["mobile_no"] = "";
                            dr["labIsAnswer"] = "";
                            dr["client_ip"] = "";
                            dr["score"] = "";
                            dr["standard_score"] = "";
                            dr["detail_score"] = "";
                            dr["IsPass"] = "";
                            dr["new_cre_id"] = "";
                            dr["risk_result"] = "";
                            dr["from"] = "";

                            string fuin = dr["FUin"].ToString();

                            string fuid = AccountData.ConvertToFuid(fuin);
                            if (fuid == null || fuid.Trim() == "")
                            {
                                // 2012/4/28 修改，用户注销之后，应该是查不到相应的UID的
                                continue;
                                //return false;
                            }

                            string strSql = "select Fattach from " + PublicRes.GetTName("cft_digit_crt", "t_user_attr", fuid)
                                + " where Fuid=" + fuid + " and Fattr=1";

                            string strtmp = QueryInfo.GetString(da.GetOneResult(strSql));
                            dr["cre_image2"] = strtmp;

                            //string parameter = dr["FParameter"].ToString();
                            if (haveimg)
                            {
                                string parameter = GetOneParameter(dr["Fid"].ToString());

                                string[] paramlist = parameter.Split('&');

                                foreach (string param in paramlist)
                                {
                                    if (param.StartsWith("cre_id="))
                                    {
                                        dr["cre_id"] = PublicRes.getCgiString(param.Replace("cre_id=", ""));
                                    }
                                    else if (param.StartsWith("cre_type="))
                                    {
                                        dr["cre_type"] = PublicRes.getCgiString(param.Replace("cre_type=", ""));
                                    }
                                    else if (param.StartsWith("cre_image="))
                                    {
                                        dr["cre_image"] = PublicRes.getCgiString(param.Replace("cre_image=", ""));
                                    }
                                    else if (param.StartsWith("clear_pps="))
                                    {
                                        dr["clear_pps"] = PublicRes.getCgiString(param.Replace("clear_pps=", ""));
                                    }
                                    //						else if(param.StartsWith("email"))
                                    //						{
                                    //							dr["email"] = getCgiString(param.Replace("email=",""));
                                    //						}
                                    else if (param.StartsWith("reason="))
                                    {
                                        dr["reason"] = PublicRes.getCgiString(param.Replace("reason=", ""));
                                    }
                                    else if (param.StartsWith("old_name="))
                                    {
                                        dr["old_name"] = PublicRes.getCgiString(param.Replace("old_name=", ""));
                                    }
                                    else if (param.StartsWith("new_name="))
                                    {
                                        dr["new_name"] = PublicRes.getCgiString(param.Replace("new_name=", ""));
                                    }
                                    else if (param.StartsWith("old_company="))
                                    {
                                        dr["old_company"] = PublicRes.getCgiString(param.Replace("old_company=", ""));
                                    }
                                    else if (param.StartsWith("new_company="))
                                    {
                                        dr["new_company"] = PublicRes.getCgiString(param.Replace("new_company=", ""));
                                    }
                                    else if (param.StartsWith("question1="))
                                    {
                                        dr["question1"] = PublicRes.getCgiString(param.Replace("question1=", ""));
                                    }
                                    else if (param.StartsWith("answer1="))
                                    {
                                        dr["answer1"] = PublicRes.getCgiString(param.Replace("answer1=", ""));
                                    }
                                    else if (param.StartsWith("cont_type="))
                                    {
                                        dr["cont_type"] = PublicRes.getCgiString(param.Replace("cont_type=", ""));
                                    }
                                    else if (param.StartsWith("mobile_no="))
                                    {
                                        dr["mobile_no"] = PublicRes.getCgiString(param.Replace("mobile_no=", ""));
                                    }
                                    else if (param.StartsWith("ENV_ClientIp="))
                                    {
                                        dr["client_ip"] = PublicRes.getCgiString(param.Replace("ENV_ClientIp=", ""));
                                    }
                                    else if (param.StartsWith("score="))
                                    {
                                        dr["score"] = PublicRes.getCgiString(param.Replace("score=", ""));
                                    }
                                    else if (param.StartsWith("standard_score="))
                                    {
                                        dr["standard_score"] = PublicRes.getCgiString(param.Replace("standard_score=", ""));
                                    }
                                    else if (param.StartsWith("detail_score="))
                                    {
                                        dr["detail_score"] = PublicRes.getCgiString(param.Replace("detail_score=", ""));
                                    }
                                    else if (param.StartsWith("new_cre_id="))
                                    {
                                        dr["new_cre_id"] = PublicRes.getCgiString(param.Replace("new_cre_id=", ""));
                                    }
                                    else if (param.StartsWith("RISK_RESULT="))
                                    {
                                        string risk_result = PublicRes.getCgiString(param.Replace("RISK_RESULT=", ""));
                                        if (risk_result == "0")
                                            dr["risk_result"] = "";
                                        else if (risk_result == "1")
                                            dr["risk_result"] = "风控异常单无需人工回访用户";
                                        else if (risk_result == "2")
                                            dr["risk_result"] = "风控异常单需人工回访用户";
                                        else
                                            dr["risk_result"] = risk_result;
                                    }
                                    else if (param.StartsWith("from="))
                                    {
                                        dr["from"] = PublicRes.getCgiString(param.Replace("from=", ""));
                                    }

                                }
                            }

                            if (dr["detail_score"].ToString() != "")
                            {
                                try
                                {
                                    string detail_score = System.Web.HttpUtility.UrlDecode(dr["detail_score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));

                                    if (detail_score.IndexOf("PwdProtection") > -1)
                                    {
                                        detail_score = detail_score.Replace("PwdProtection", "密保校验得分");
                                    }
                                    if (detail_score.IndexOf("CertifiedId") > -1)
                                    {
                                        detail_score = detail_score.Replace("CertifiedId", "证件号校验得分");
                                    }
                                    if (detail_score.IndexOf("bind_email") > -1)
                                    {
                                        detail_score = detail_score.Replace("bind_email", "绑定邮箱校验得分");
                                    }
                                    if (detail_score.IndexOf("bind_mobile") > -1)
                                    {
                                        detail_score = detail_score.Replace("bind_mobile", "绑定手机校验得分");
                                    }
                                    if (detail_score.IndexOf("QQReceipt") > -1)
                                    {
                                        detail_score = detail_score.Replace("QQReceipt", "QQ申诉回执号得分");
                                    }
                                    if (detail_score.IndexOf("CertifiedBankCard") > -1)
                                    {
                                        detail_score = detail_score.Replace("CertifiedBankCard", "实名认证银行卡号校验得分");
                                    }
                                    if (detail_score.IndexOf("CreditCardPayHist") > -1)
                                    {
                                        detail_score = detail_score.Replace("CreditCardPayHist", "信用卡还款信息校验得分");
                                    }
                                    if (detail_score.IndexOf("WithdrawHist") > -1) //andrew 20110419
                                    {
                                        detail_score = detail_score.Replace("WithdrawHist", "提现记录得分");
                                    }
                                    if (detail_score.IndexOf("MBVerify") > -1)
                                    {
                                        detail_score = detail_score.Replace("MBVerify", "安平密保验证得分");
                                    }
                                    if (detail_score.IndexOf("MBQuery") > -1)
                                    {
                                        detail_score = detail_score.Replace("MBQuery", "通过安全中心密保得分");
                                    }
                                    if (detail_score.IndexOf("BindMobile") > -1)
                                    {
                                        detail_score = detail_score.Replace("BindMobile", "绑定的手机号码得分");
                                    }
                                    if (detail_score.IndexOf("Mobile") > -1)
                                    {
                                        detail_score = detail_score.Replace("Mobile", "手机得分");
                                    }
                                    if (detail_score.IndexOf("Email_QQ") > -1)
                                    {
                                        detail_score = detail_score.Replace("Email_QQ", "绑定QQ邮箱得分");
                                    }
                                    if (detail_score.IndexOf("Mobile_New") > -1)
                                    {
                                        detail_score = detail_score.Replace("Mobile_New", "未注册手机得分");
                                    }
                                    if (detail_score.IndexOf("QQReceipt_6") > -1)
                                    {
                                        detail_score = detail_score.Replace("QQReceipt_6", "简化注册用户QQ申诉回执单号得分");
                                    }
                                    dr["detail_score"] = detail_score;

                                }
                                catch
                                { }
                            }

                            string tmp = dr["FType"].ToString();
                            if (tmp == "1")    //目前只开放找回支付密码得分超标准分免审核，以后会慢慢开放的
                            {
                                dr["FTypeName"] = "找回密码";

                                if (dr["clear_pps"].ToString() == "0") //不清空
                                {
                                    string Sql = "uid=" + fuid;
                                    string errMsg = "";
                                    string answer1 = CommQuery.GetOneResultFromICE(Sql, CommQuery.QUERY_USERINFO, "Fanswer1", out errMsg);
                                    if (answer1 != null && answer1.Trim() != "")
                                    {
                                        if (dr["answer1"].ToString().Trim() == answer1.ToString().Trim())
                                            dr["labIsAnswer"] = "答对了";
                                        else
                                            dr["labIsAnswer"] = "答错了";
                                    }
                                }

                                try
                                {
                                    if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                        dr["IsPass"] = "Y";
                                    else
                                        dr["IsPass"] = "N";
                                }
                                catch
                                { }
                            }
                            else if (tmp == "2")
                            {
                                dr["FTypeName"] = "修改姓名";
                                try
                                {
                                    if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                        dr["IsPass"] = "Y";
                                    else
                                        dr["IsPass"] = "N";
                                }
                                catch
                                { }
                            }
                            else if (tmp == "3")
                            {
                                dr["FTypeName"] = "修改公司名";
                            }
                            else if (tmp == "4")
                            {
                                dr["FTypeName"] = "注销帐号";
                            }
                            else if (tmp == "5")// andrew 超标准分免审 20110419
                            {
                                dr["FTypeName"] = "完整注册用户更换关联手机";

                                try
                                {
                                    //IVR外呼专用furion  因为有高分单不会被领单,所以这里不用改动.
                                    if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                        dr["IsPass"] = "Y";
                                    else
                                        dr["IsPass"] = "N";
                                }
                                catch
                                { }
                            }
                            else if (tmp == "6")//andrew 20110419
                            {
                                dr["FTypeName"] = "简化注册用户更换绑定手机";
                                try
                                {
                                    if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                        dr["IsPass"] = "Y";
                                    else
                                        dr["IsPass"] = "N";
                                }
                                catch
                                { }
                            }
                            else if (tmp == "7")
                            {
                                dr["FTypeName"] = "更换证件号";
                                try
                                {
                                    if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                        dr["IsPass"] = "Y";
                                    else
                                        dr["IsPass"] = "N";
                                }
                                catch
                                { }
                            }
                            else if (tmp == "9")
                            {
                                dr["FTypeName"] = "手机令牌";
                            }
                            else if (tmp == "10")
                            {
                                dr["FTypeName"] = "第三方令牌";
                            }
                            else if (tmp == "0")
                            {
                                dr["FTypeName"] = "财付盾解绑";
                            }

                            tmp = dr["FState"].ToString();
                            if (tmp == "0")
                            {
                                dr["FStateName"] = "未处理";
                            }
                            else if (tmp == "1")
                            {
                                dr["FStateName"] = "申诉成功";
                            }
                            else if (tmp == "2")
                            {
                                dr["FStateName"] = "申诉失败";
                            }
                            else if (tmp == "3")
                            {
                                dr["FStateName"] = "大额待复核";
                            }
                            else if (tmp == "4")
                            {
                                dr["FStateName"] = "直接转后台";
                            }
                            else if (tmp == "5")
                            {
                                dr["FStateName"] = "异常转后台";
                            }
                            else if (tmp == "6")
                            {
                                dr["FStateName"] = "发邮件失败";
                            }
                            else if (tmp == "7")
                            {
                                dr["FStateName"] = "已删除";
                            }
                            else if (tmp == "8")
                            {
                                dr["FStateName"] = "已领单";
                            }
                            else if (tmp == "9")
                            {
                                dr["FStateName"] = "短信撤销申诉";
                            }
                        }
                        catch (System.Exception ex)
                        {

                        }


                    }
                    #endregion
                }
                return true;
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
        }

        public static bool HandleParameter_ForControledFreeze(DataSet ds, bool haveimg)
        {
            //加入一个证书库连接.
            // MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CRT"));
            try
            {
                ds.Tables[0].Columns.Add("FTypeName", typeof(String));
                ds.Tables[0].Columns.Add("FStateName", typeof(String));

                ds.Tables[0].Columns.Add("clear_pps", typeof(string));
                ds.Tables[0].Columns.Add("uin", typeof(string));
                ds.Tables[0].Columns.Add("uid", typeof(string));
                ds.Tables[0].Columns.Add("cre_id", typeof(string)); //证件号码
                ds.Tables[0].Columns.Add("cre_type", typeof(string)); //证件类型
                ds.Tables[0].Columns.Add("cre_image", typeof(string)); //图片
                ds.Tables[0].Columns.Add("reason", typeof(string)); //原因

                //更换手机暂时加入一个字段,原请求证件地址
                ds.Tables[0].Columns.Add("client_ip", typeof(string)); //申诉人IP
                ds.Tables[0].Columns.Add("risk_result", typeof(string)); //风控标记

                // 风控冻结要求加入字段如下：
                ds.Tables[0].Columns.Add("email", typeof(string));		// 邮箱
                ds.Tables[0].Columns.Add("pkey", typeof(string));		// 验证邮件连接时传的PKEY
                ds.Tables[0].Columns.Add("contact_no", typeof(string));	// 用户联系电话
                ds.Tables[0].Columns.Add("otherImage1", typeof(string));	// 其他图片1
                ds.Tables[0].Columns.Add("otherImage2", typeof(string));	// 其他图片2
                ds.Tables[0].Columns.Add("otherImage3", typeof(string));	// 其他图片3
                ds.Tables[0].Columns.Add("otherImage4", typeof(string));	// 其他图片4
                ds.Tables[0].Columns.Add("otherImage5", typeof(string));	// 其他图片5
                ds.Tables[0].Columns.Add("rec_cftadd", typeof(string));	// 最近使用财付通的地址
                ds.Tables[0].Columns.Add("prove_banlance_image", typeof(string));	// 余额资金来源证明
                ds.Tables[0].Columns.Add("DC_timeaddress", typeof(string));	// 最近安装数字证书的时间和地址
                ds.Tables[0].Columns.Add("mobile", typeof(string));		// 绑定的手机号

                using (var da = MySQLAccessFactory.GetMySQLAccess("CRT_DB"))
                {
                    da.OpenConn();

                    #region
                    // 2012/4/28 修改，因为目前如果其中一个元素查找失败(可能是由于该帐号已经注销)，则会导致后面的dataRow查询失败！
                    // 所以增加一个try catch模块，忽略查询失败的记录
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        try
                        {
                            dr["uin"] = "";
                            dr["uid"] = "";
                            dr["cre_id"] = "";
                            dr["cre_type"] = "";
                            dr["cre_image"] = "";
                            dr["clear_pps"] = "0";
                            dr["reason"] = "";

                            dr["email"] = "";
                            dr["pkey"] = "";
                            dr["contact_no"] = "";
                            dr["otherImage1"] = "";
                            dr["otherImage2"] = "";
                            dr["otherImage3"] = "";
                            dr["otherImage4"] = "";
                            dr["otherImage5"] = "";
                            dr["rec_cftadd"] = "";
                            dr["prove_banlance_image"] = "";
                            dr["DC_timeaddress"] = "";
                            dr["mobile"] = "";

                            string fuin = dr["FUin"].ToString();

                            string fuid = AccountData.ConvertToFuid(fuin);
                            if (fuid == null || fuid.Trim() == "")
                            {
                                // 2012/4/28 修改，用户注销之后，应该是查不到相应的UID的
                                continue;
                                //return false;
                            }

                            //string parameter = dr["FParameter"].ToString();
                            if (haveimg)
                            {
                                string parameter = GetOneParameter(dr["Fid"].ToString());

                                string[] paramlist = parameter.Split('&');

                                foreach (string param in paramlist)
                                {
                                    if (param.StartsWith("uin="))
                                    {
                                        dr["uin"] = PublicRes.getCgiString(param.Replace("uin=", ""));
                                    }
                                    else if (param.StartsWith("uid="))
                                    {
                                        dr["uid"] = PublicRes.getCgiString(param.Replace("uid=", ""));
                                    }
                                    if (param.StartsWith("cre_id="))
                                    {
                                        dr["cre_id"] = PublicRes.getCgiString(param.Replace("cre_id=", ""));
                                    }
                                    else if (param.StartsWith("cre_type="))
                                    {
                                        dr["cre_type"] = PublicRes.getCgiString(param.Replace("cre_type=", ""));
                                    }
                                    else if (param.StartsWith("cre_image="))
                                    {
                                        dr["cre_image"] = PublicRes.getCgiString(param.Replace("cre_image=", ""));
                                    }
                                    else if (param.StartsWith("clear_pps="))
                                    {
                                        dr["clear_pps"] = PublicRes.getCgiString(param.Replace("clear_pps=", ""));
                                    }
                                    else if (param.StartsWith("email"))
                                    {
                                        dr["email"] = PublicRes.getCgiString(param.Replace("email=", ""));
                                    }
                                    else if (param.StartsWith("reason="))
                                    {
                                        dr["reason"] = PublicRes.getCgiString(param.Replace("reason=", ""));
                                    }

                                    else if (param.StartsWith("pkey="))
                                    {
                                        dr["pkey"] = PublicRes.getCgiString(param.Replace("pkey=", ""));
                                    }
                                    else if (param.StartsWith("mobile="))
                                    {
                                        dr["mobile"] = PublicRes.getCgiString(param.Replace("mobile=", ""));
                                    }
                                    else if (param.StartsWith("contact_no="))
                                    {
                                        dr["contact_no"] = PublicRes.getCgiString(param.Replace("contact_no=", ""));
                                    }
                                    else if (param.StartsWith("rec_cftadd="))
                                    {
                                        dr["rec_cftadd"] = PublicRes.getCgiString(param.Replace("rec_cftadd=", ""));
                                    }
                                    else if (param.StartsWith("prove_banlance_image="))
                                    {
                                        dr["prove_banlance_image"] = PublicRes.getCgiString(param.Replace("prove_banlance_image=", ""));
                                    }
                                    else if (param.StartsWith("other1_image="))
                                    {
                                        dr["otherImage1"] = PublicRes.getCgiString(param.Replace("other1_image=", ""));
                                    }
                                    else if (param.StartsWith("other2_image="))
                                    {
                                        dr["otherImage2"] = PublicRes.getCgiString(param.Replace("other2_image=", ""));
                                    }
                                    else if (param.StartsWith("other3_image="))
                                    {
                                        dr["otherImage3"] = PublicRes.getCgiString(param.Replace("other3_image=", ""));
                                    }
                                    else if (param.StartsWith("other4_image="))
                                    {
                                        dr["otherImage4"] = PublicRes.getCgiString(param.Replace("other4_image=", ""));
                                    }
                                    else if (param.StartsWith("other5_image="))
                                    {
                                        dr["otherImage5"] = PublicRes.getCgiString(param.Replace("other5_image=", ""));
                                    }
                                    else if (param.StartsWith("DC_timeaddress="))
                                    {
                                        dr["DC_timeaddress"] = PublicRes.getCgiString(param.Replace("DC_timeaddress=", ""));
                                    }
                                }
                            }

                            string tmp = dr["FState"].ToString();
                            if (tmp == "0")
                            {
                                dr["FStateName"] = "未处理";
                            }
                            else if (tmp == "1")
                            {
                                dr["FStateName"] = "申诉成功";
                            }
                            else if (tmp == "2")
                            {
                                dr["FStateName"] = "申诉失败";
                            }
                            else if (tmp == "3")
                            {
                                dr["FStateName"] = "大额待复核";
                            }
                            else if (tmp == "4")
                            {
                                dr["FStateName"] = "直接转后台";
                            }
                            else if (tmp == "5")
                            {
                                dr["FStateName"] = "异常转后台";
                            }
                            else if (tmp == "6")
                            {
                                dr["FStateName"] = "发邮件失败";
                            }
                            else if (tmp == "7")
                            {
                                dr["FStateName"] = "已删除";
                            }
                            else if (tmp == "8")
                            {
                                dr["FStateName"] = "已领单";
                            }
                            else if (tmp == "9")
                            {
                                dr["FStateName"] = "短信撤销申诉";
                            }
                        }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                            return false;
                        }
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return false;
            }

            return true;
        }

        //private static string getCgiString(string instr)
        //{
        //    if (instr == null || instr.Trim() == "")
        //        return "";

        //    //System.Text.Encoding enc = System.Text.Encoding.GetEncoding("GB2312");
        //    return System.Web.HttpContext.Current.Server.UrlDecode(instr).Replace("\r\n", "").Trim()
        //        .Replace("%3d", "=").Replace("%20", " ").Replace("%26", "&").Replace("%7c", "|");
        //}

        public CFTUserAppealClass(string fid, string table)
        {
            //fstrSql = "select Fid,FType,Fuin,FSubmitTime,FState,Fpicktime,FCheckTime,FCheckInfo,FCheckUser,FComment,Femail from t_tenpay_appeal_trans where FID=" + fid ;
            fstrSql = "select * from " + table + " where FID='" + fid + "'";
            fstrSql_count = "select count(1) from " + table + " where FID='" + fid + "'";
        }

        public static string getCgiString(string instr)
        {
            if (instr == null || instr.Trim() == "")
                return "";

            return System.Web.HttpContext.Current.Server.UrlDecode(instr).Replace("\r\n", "").Trim()
                .Replace("%3d", "=").Replace("%20", " ").Replace("%26", "&").Replace("%7c", "|");
        }

        private static string GetOneParameter(string fid)
        {
            try
            {
                using (var da = MySQLAccessFactory.GetMySQLAccess("CFT_DB"))
                {
                    da.OpenConn();
                    string strSql = "select FParameter from " + cftDB + ".t_tenpay_appeal_trans where Fid=" + fid;
                    return da.GetOneResult(strSql);
                }
            }
            catch (Exception err)
            {
                return "";
            }
        }

        public CFTUserAppealClass(int fid)
        {
            //fstrSql = "select Fid,FType,Fuin,FSubmitTime,FState,Fpicktime,FCheckTime,FCheckInfo,FCheckUser,FComment,Femail from t_tenpay_appeal_trans where FID=" + fid ;
            fstrSql = "select * from " + cftDB + ".t_tenpay_appeal_trans where FID=" + fid;
            fstrSql_count = "select count(1) from " + cftDB + ".t_tenpay_appeal_trans where FID=" + fid;
        }

        //20131107 lxl 查询分库表
        public CFTUserAppealClass(string fid, string db, string tb, string mark)
        {
            //fstrSql = "select Fid,FType,Fuin,FSubmitTime,FState,Fpicktime,FCheckTime,FCheckInfo,FCheckUser,FComment,Femail from t_tenpay_appeal_trans where FID=" + fid ;
            fstrSql = "select * from " + db + "." + tb + " where FID='" + fid + "'";
            fstrSql_count = "select count(1) from " + db + "." + tb + " where FID='" + fid + "'";
        }

        //20131107 lxl
        //分库表的处理方法，处理结果后的结果与旧表一样
        //public static bool HandleParameterByDBTB(DataSet ds, bool haveimg)
        //{
        //    //加入一个证书库连接.
        //   // MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CRT"));
        //    try
        //    {
        //        ds.Tables[0].Columns.Add("FTypeName", typeof(String));
        //        ds.Tables[0].Columns.Add("FStateName", typeof(String));

        //        ds.Tables[0].Columns.Add("cre_id", typeof(string)); //证件号码
        //        ds.Tables[0].Columns.Add("cre_type", typeof(string)); //证件类型
        //        ds.Tables[0].Columns.Add("cre_image", typeof(string)); //图片
        //        ds.Tables[0].Columns.Add("clear_pps", typeof(string)); //1为清除密保
        //        ds.Tables[0].Columns.Add("reason", typeof(string)); //原因
        //        ds.Tables[0].Columns.Add("old_name", typeof(string));
        //        ds.Tables[0].Columns.Add("new_name", typeof(string));
        //        ds.Tables[0].Columns.Add("old_company", typeof(string));
        //        ds.Tables[0].Columns.Add("new_company", typeof(string));

        //        //更换手机暂时加入一个字段,原请求证件地址
        //        ds.Tables[0].Columns.Add("cre_image2", typeof(string)); //图片
        //        ds.Tables[0].Columns.Add("question1", typeof(string)); //密保1
        //        ds.Tables[0].Columns.Add("answer1", typeof(string)); //答案1
        //        ds.Tables[0].Columns.Add("cont_type", typeof(string)); // 1: 手机 2: email//cont_type=1时,mobile_no有效 cont_type=2时,femail有效
        //        ds.Tables[0].Columns.Add("mobile_no", typeof(string));
        //        ds.Tables[0].Columns.Add("labIsAnswer", typeof(string)); //是否答对密保
        //        ds.Tables[0].Columns.Add("client_ip", typeof(string)); //申诉人IP
        //        ds.Tables[0].Columns.Add("score", typeof(string)); //实际得分
        //        ds.Tables[0].Columns.Add("standard_score", typeof(string)); //免审核标准分
        //        ds.Tables[0].Columns.Add("detail_score", typeof(string)); //得分明细
        //        ds.Tables[0].Columns.Add("IsPass", typeof(string)); //结果
        //        ds.Tables[0].Columns.Add("new_cre_id", typeof(string)); //新身份证
        //        ds.Tables[0].Columns.Add("risk_result", typeof(string)); //风控标记
        //        ds.Tables[0].Columns.Add("from", typeof(string)); //申诉渠道标记（手机）

        //        // 风控冻结要求加入字段如下：
        //        /*
        //        ds.Tables[0].Columns.Add("email",typeof(string));		// 邮箱
        //        ds.Tables[0].Columns.Add("pkey",typeof(string));		// 验证邮件连接时传的PKEY
        //        ds.Tables[0].Columns.Add("contact_no",typeof(string));	// 用户联系电话
        //        ds.Tables[0].Columns.Add("otherImage1",typeof(string));	// 其他图片1
        //        ds.Tables[0].Columns.Add("otherImage2",typeof(string));	// 其他图片2
        //        ds.Tables[0].Columns.Add("otherImage3",typeof(string));	// 其他图片3
        //        ds.Tables[0].Columns.Add("otherImage4",typeof(string));	// 其他图片4
        //        ds.Tables[0].Columns.Add("otherImage5",typeof(string));	// 其他图片5
        //        ds.Tables[0].Columns.Add("rec_cftadd",typeof(string));	// 最近使用财付通的地址
        //        ds.Tables[0].Columns.Add("prove_banlance_image",typeof(string));	// 余额资金来源证明
        //        ds.Tables[0].Columns.Add("DC_timeaddress",typeof(string));	// 最近安装数字证书的时间和地址
        //        ds.Tables[0].Columns.Add("mobile",typeof(string));		// 绑定的手机号
        //        */

        //        using (var da = MySQLAccessFactory.GetMySQLAccess("CRT_DB"))
        //        {
        //            da.OpenConn();

        //            #region
        //            // 2012/4/28 修改，因为目前如果其中一个元素查找失败(可能是由于该帐号已经注销)，则会导致后面的dataRow查询失败！
        //            // 所以增加一个try catch模块，忽略查询失败的记录
        //            foreach (DataRow dr in ds.Tables[0].Rows)
        //            {
        //                try
        //                {
        //                    dr["cre_id"] = "";
        //                    dr["cre_type"] = "";
        //                    dr["cre_image"] = "";
        //                    dr["clear_pps"] = "0";
        //                    dr["reason"] = "";
        //                    dr["old_name"] = "";
        //                    dr["new_name"] = "";
        //                    dr["old_company"] = "";
        //                    dr["new_company"] = "";
        //                    dr["question1"] = "";
        //                    dr["answer1"] = "";
        //                    dr["cont_type"] = "";
        //                    dr["mobile_no"] = "";
        //                    dr["labIsAnswer"] = "";
        //                    dr["client_ip"] = "";
        //                    dr["score"] = "";
        //                    dr["standard_score"] = "";
        //                    dr["detail_score"] = "";
        //                    dr["IsPass"] = "";
        //                    dr["new_cre_id"] = "";
        //                    dr["risk_result"] = "";
        //                    dr["from"] = "";

        //                    string fuin = dr["FUin"].ToString();

        //                    string fuid = AccountData.ConvertToFuid(fuin);
        //                    if (fuid == null || fuid.Trim() == "")
        //                    {
        //                        // 2012/4/28 修改，用户注销之后，应该是查不到相应的UID的
        //                        continue;
        //                        //return false;
        //                    }

        //                    string strSql = "select Fattach from " + PublicRes.GetTName("cft_digit_crt", "t_user_attr", fuid)
        //                        + " where Fuid=" + fuid + " and Fattr=1";

        //                    string strtmp = QueryInfo.GetString(da.GetOneResult(strSql));
        //                    dr["cre_image2"] = strtmp;

        //                    //string parameter = dr["FParameter"].ToString();
        //                    if (haveimg)
        //                    {
        //                        dr["cre_id"] = PublicRes.getCgiString(dr["FCreId"].ToString());

        //                        dr["cre_type"] = PublicRes.getCgiString(dr["FCreType"].ToString());

        //                       // dr["cre_image"] = PublicRes.getCgiString(dr["FCreImg1"].ToString()) + "|" + PublicRes.getCgiString(dr["FCreImg2"].ToString());
        //                        dr["cre_image"] = getCgiString(dr["FCreImg1"].ToString()) + "|" + getCgiString(dr["FProveBanlanceImage"].ToString());

        //                        dr["clear_pps"] = PublicRes.getCgiString(dr["FClearPps"].ToString());

        //                        dr["reason"] = PublicRes.getCgiString(dr["FAppealReason"].ToString());

        //                        dr["old_name"] = PublicRes.getCgiString(dr["FOldName"].ToString());

        //                        dr["new_name"] = PublicRes.getCgiString(dr["FNewName"].ToString());

        //                        dr["old_company"] = PublicRes.getCgiString(dr["FOldCompanyName"].ToString());

        //                        dr["new_company"] = PublicRes.getCgiString(dr["FNewCompanyName"].ToString());

        //                        dr["question1"] = PublicRes.getCgiString(dr["FMbQuestion"].ToString());

        //                        dr["answer1"] = PublicRes.getCgiString(dr["FMbAnswer"].ToString());

        //                        dr["cont_type"] = PublicRes.getCgiString(dr["FContType"].ToString());

        //                        dr["mobile_no"] = PublicRes.getCgiString(dr["FReservedMobile"].ToString());

        //                        dr["client_ip"] = PublicRes.getCgiString(dr["FIp"].ToString());

        //                        dr["score"] = PublicRes.getCgiString(dr["FAppealScore"].ToString()); ;

        //                        dr["standard_score"] = PublicRes.getCgiString(dr["FStandardScore"].ToString());

        //                        dr["detail_score"] = PublicRes.getCgiString(dr["FDetailScore"].ToString());

        //                        dr["new_cre_id"] = PublicRes.getCgiString(dr["FNewCreId"].ToString());

        //                        string risk_result = PublicRes.getCgiString(dr["FRiskState"].ToString());
        //                        if (risk_result == "0")
        //                            dr["risk_result"] = "";
        //                        else if (risk_result == "1")
        //                            dr["risk_result"] = "风控异常单无需人工回访用户";
        //                        else if (risk_result == "2")
        //                            dr["risk_result"] = "风控异常单需人工回访用户";
        //                        else
        //                            dr["risk_result"] = risk_result;

        //                        dr["from"] = PublicRes.getCgiString(dr["FChanel"].ToString());

        //                    }

        //                    if (dr["detail_score"].ToString() != "")
        //                    {
        //                        try
        //                        {
        //                            string detail_score = System.Web.HttpUtility.UrlDecode(dr["detail_score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));

        //                            if (detail_score.IndexOf("PwdProtection") > -1)
        //                            {
        //                                detail_score = detail_score.Replace("PwdProtection", "密保校验得分");
        //                            }
        //                            if (detail_score.IndexOf("CertifiedId") > -1)
        //                            {
        //                                detail_score = detail_score.Replace("CertifiedId", "证件号校验得分");
        //                            }
        //                            if (detail_score.IndexOf("bind_email") > -1)
        //                            {
        //                                detail_score = detail_score.Replace("bind_email", "绑定邮箱校验得分");
        //                            }
        //                            if (detail_score.IndexOf("bind_mobile") > -1)
        //                            {
        //                                detail_score = detail_score.Replace("bind_mobile", "绑定手机校验得分");
        //                            }
        //                            if (detail_score.IndexOf("QQReceipt") > -1)
        //                            {
        //                                detail_score = detail_score.Replace("QQReceipt", "QQ申诉回执号得分");
        //                            }
        //                            if (detail_score.IndexOf("CertifiedBankCard") > -1)
        //                            {
        //                                detail_score = detail_score.Replace("CertifiedBankCard", "实名认证银行卡号校验得分");
        //                            }
        //                            if (detail_score.IndexOf("CreditCardPayHist") > -1)
        //                            {
        //                                detail_score = detail_score.Replace("CreditCardPayHist", "信用卡还款信息校验得分");
        //                            }
        //                            if (detail_score.IndexOf("WithdrawHist") > -1) //andrew 20110419
        //                            {
        //                                detail_score = detail_score.Replace("WithdrawHist", "提现记录得分");
        //                            }

        //                            dr["detail_score"] = detail_score;

        //                        }
        //                        catch
        //                        { }
        //                    }

        //                    string tmp = dr["FType"].ToString();
        //                    if (tmp == "1")    //目前只开放找回支付密码得分超标准分免审核，以后会慢慢开放的
        //                    {
        //                        dr["FTypeName"] = "找回密码";

        //                        if (dr["clear_pps"].ToString() == "0") //不清空
        //                        {
        //                            string Sql = "uid=" + fuid;
        //                            string errMsg = "";
        //                            string answer1 = CommQuery.GetOneResultFromICE(Sql, CommQuery.QUERY_USERINFO, "Fanswer1", out errMsg);
        //                            if (answer1 != null && answer1.Trim() != "")
        //                            {
        //                                if (dr["answer1"].ToString().Trim() == answer1.ToString().Trim())
        //                                    dr["labIsAnswer"] = "答对了";//是否答对密保
        //                                else
        //                                    dr["labIsAnswer"] = "答错了";
        //                            }
        //                        }

        //                        try
        //                        {
        //                            if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
        //                                dr["IsPass"] = "Y";
        //                            else
        //                                dr["IsPass"] = "N";
        //                        }
        //                        catch
        //                        { }
        //                    }
        //                    else if (tmp == "2")
        //                    {
        //                        dr["FTypeName"] = "修改姓名";
        //                        try
        //                        {
        //                            if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
        //                                dr["IsPass"] = "Y";
        //                            else
        //                                dr["IsPass"] = "N";
        //                        }
        //                        catch
        //                        { }
        //                    }
        //                    else if (tmp == "3")
        //                    {
        //                        dr["FTypeName"] = "修改公司名";
        //                    }
        //                    else if (tmp == "4")
        //                    {
        //                        dr["FTypeName"] = "注销帐号";
        //                    }
        //                    else if (tmp == "5")// andrew 超标准分免审 20110419
        //                    {
        //                        dr["FTypeName"] = "完整注册用户更换关联手机";

        //                        try
        //                        {
        //                            //IVR外呼专用furion  因为有高分单不会被领单,所以这里不用改动.
        //                            if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
        //                                dr["IsPass"] = "Y";
        //                            else
        //                                dr["IsPass"] = "N";
        //                        }
        //                        catch
        //                        { }
        //                    }
        //                    else if (tmp == "6")//andrew 20110419
        //                    {
        //                        dr["FTypeName"] = "简化注册用户更换绑定手机";
        //                        try
        //                        {
        //                            if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
        //                                dr["IsPass"] = "Y";
        //                            else
        //                                dr["IsPass"] = "N";
        //                        }
        //                        catch
        //                        { }
        //                    }
        //                    else if (tmp == "7")
        //                    {
        //                        dr["FTypeName"] = "更换证件号";
        //                        try
        //                        {
        //                            if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
        //                                dr["IsPass"] = "Y";
        //                            else
        //                                dr["IsPass"] = "N";
        //                        }
        //                        catch
        //                        { }
        //                    }
        //                    else if (tmp == "9")
        //                    {
        //                        dr["FTypeName"] = "手机令牌";
        //                    }
        //                    else if (tmp == "10")
        //                    {
        //                        dr["FTypeName"] = "第三方令牌";
        //                    }
        //                    else if (tmp == "0")
        //                    {
        //                        dr["FTypeName"] = "财付盾解绑";
        //                    }

        //                    tmp = dr["FState"].ToString();
        //                    if (tmp == "0")
        //                    {
        //                        dr["FStateName"] = "未处理";
        //                    }
        //                    else if (tmp == "1")
        //                    {
        //                        dr["FStateName"] = "申诉成功";
        //                    }
        //                    else if (tmp == "2")
        //                    {
        //                        dr["FStateName"] = "申诉失败";
        //                    }
        //                    else if (tmp == "3")
        //                    {
        //                        dr["FStateName"] = "大额待复核";
        //                    }
        //                    else if (tmp == "4")
        //                    {
        //                        dr["FStateName"] = "直接转后台";
        //                    }
        //                    else if (tmp == "5")
        //                    {
        //                        dr["FStateName"] = "异常转后台";
        //                    }
        //                    else if (tmp == "6")
        //                    {
        //                        dr["FStateName"] = "发邮件失败";
        //                    }
        //                    else if (tmp == "7")
        //                    {
        //                        dr["FStateName"] = "已删除";
        //                    }
        //                    else if (tmp == "8")
        //                    {
        //                        dr["FStateName"] = "已领单";
        //                    }
        //                    else if (tmp == "9")
        //                    {
        //                        dr["FStateName"] = "短信撤销申诉";
        //                    }
        //                }
        //                catch (System.Exception ex)
        //                {

        //                }
        //            }
        //            #endregion
        //        }
        //        return true;
        //    }
        //    catch (Exception err)
        //    {
        //        throw new LogicException(err.Message);
        //    }
        //}













    }
    #endregion

    #region 冻结查询类。

    public class FreezeQueryClass : Query_BaseForNET
    {
        public FreezeQueryClass(string qqid, int handleFinish)
        {
            fstrSql = "select * from c2c_fmdb.t_freeze_list  where  FFreezeID='" + qqid + "' and FHandleFinish=1";
            fstrSql_count = "select count(*) from c2c_fmdb.t_freeze_list where  FFreezeID='" + qqid + "' and FHandleFinish=1";
        }

    }
    #endregion

}

