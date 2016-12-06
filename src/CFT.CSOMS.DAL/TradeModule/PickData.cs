using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.CSOMS.DAL.CFTAccount;
using CommLib;
using System.Configuration;

using CFT.Apollo.Logging;
using CFT.CSOMS.COMMLIB;
using System.Xml;
using System.Collections;
using SunLibraryEX;
using CFT.Apollo.Bow.Extend;
using CFT.Apollo.Bow.Withdraw;
using TENCENT.OSS.CFT.KF.DataAccess;
using CFT.Apollo.Data.ConnectionString;

namespace CFT.CSOMS.DAL.TradeModule
{
    public class PickData
    {
        string serverIp = System.Configuration.ConfigurationManager.AppSettings["Relay_IP"].ToString();
        int serverPort = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Relay_PORT"].ToString());
        int PickRecordKeepTime = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("PickRecordKeepTime", 3);
        //提现记录查询
        public DataSet GetPickList(string u_ID, int idtype, DateTime u_BeginTime, DateTime u_EndTime, int fstate, float fnum, string banktype, string sorttype, string cashtype,
            int offset, int limit,string uid="")
        {
            string stime = u_BeginTime.ToString("yyyy-MM-dd");
            string etime = u_EndTime.ToString("yyyy-MM-dd HH:mm:ss");
            if (idtype == 0)
            {
                //按uid查询
                if (string.IsNullOrEmpty(uid))
                {
                    uid = PublicRes.ConvertToFuid(u_ID);
                }
//#if DEBUG
//                uid = "295169794";
//#endif
                return QueryPickByUid(uid, stime, etime, fstate, fnum, banktype, sorttype, cashtype, offset, limit);
            }
            else if (idtype == 1)
            {
                //按银行账号查询
                string bankID = BankLib.BankIOX.GetCreditEncode(u_ID, BankLib.BankIOX.fxykconn);
                DataSet ds = QueryPickByAbankid(bankID, stime, etime, fstate, fnum, banktype, sorttype, cashtype, offset, limit);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    bankID = u_ID;
                    ds = QueryPickByAbankid(bankID, stime, etime, fstate, fnum, banktype, sorttype, cashtype, offset, limit);
                }
                return ds;
            }
            else if (idtype == 2)
            {
                //按提现单号查询
                return QueryPickByListid(u_ID, stime, etime, fstate, fnum, banktype, cashtype);
            }
            else
            {
                throw new Exception("你输入的查询类型错误！");
            }
        }
        /// <summary>
        /// 按提记录按单号查询
        /// </summary>
        /// <returns></returns>
        public DataSet QueryPickByListid(string listid, string stime, string etime, int fstate, float fnum, string banktype, string cashtype)
        {
            if (string.IsNullOrEmpty(stime))
            {
                //如果日期为空，使用提现单号解析的日期，这个日期一般比Fpay_front_time_acc 大两天左右，
                //Fpay_front_time_acc为提现单生成的日期
                //数据库分表以提现单号解析的日期为准。
                //如果根据Fpay_front_time_acc和listid查询提现单，Fpay_front_time_acc的开始时间应该为listid的时间，结束时间应该在下一月底。

                DateTime DateStart = GetPayListTableFromID(listid);
                DateTime DateEnd = DateStart.AddDays(-DateStart.Day).AddMonths(2);
                stime = DateStart.ToString("yyyy-MM-dd");
                etime = DateEnd.ToString("yyyy-MM-dd");
            }

            string fields = "listid:" + listid + "|begintime:" + stime + "|endtime:" + etime;
            if (fstate != 0)
            {
                fields += "|sign:" + fstate.ToString();
            }
            long num = (long)Math.Round(fnum * 100, 0);

            fields += "|num:" + num.ToString();

            if (banktype != "0000")
            {
                fields += "|banktype:" + banktype;
            }
            if (cashtype != "0000")
            {
                fields += "|bankid:" + cashtype;
            }

            //2416:查询库表c2c_db.t_tcpay_list_$YYYY$$MM$
            //2427:查询库表c2c_db.t_tcpay_list_$YYYY$$MM$(spider)
            //2417:查询库表c2c_db.t_tcpay_list 
            //2431:查询库表c2c_db.t_tcpay_list_$YYYY$$MM$(SET2)


            Hashtable param = new Hashtable();
            DateTime Datestime = Convert.ToDateTime(stime);
            if (Datestime > DateTime.Now.AddDays(-PickRecordKeepTime * 30))
            {
                param.Add("2416", fields);
                param.Add("2417", fields);
                param.Add("2431", fields);
                param.Add("2427", fields);
            }
            else
            {
                param.Add("2427", fields);
                param.Add("2416", fields);
                param.Add("2417", fields);
                param.Add("2431", fields);

            }
            DataSet ds = multi_query(param, 0, 1, false);
            return ds;
        }


        /// <summary>
        /// 按提记录按单号查询--NEW
        /// </summary>
        /// <returns></returns>
        public DataTable QueryPickByListidNew(string listid)
        {
            WithdrawRepository obj = new WithdrawRepository();
            //obj.GetItemByListid(listid).ToDataTable();

          // var pageItem = new Apollo.Bow.Infrastructure.PageItem() { Offset = 0, PageCount = int.Parse(fnum.ToString()) };
           return obj.GetItemByListid(listid).ToDataTable();
        }



        /// <summary>
        /// 按提记录按uid查询
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        private DataSet QueryPickByUid(string uid, string stime, string etime, int fstate, float fnum, string banktype, string sorttype, string cashtype,
            int offset, int limit)
        {
            if (offset % limit == 1) offset -= 1;
            string fields = "uid:" + uid + "|begintime:" + stime + "|endtime:" + etime;

            if (fstate != 0)
            {
                fields += "|sign:" + fstate.ToString();
            }
            long num = (long)Math.Round(fnum * 100, 0);

            fields += "|num:" + num.ToString();

            if (banktype != "0000")
            {
                fields += "|banktype:" + banktype;
            }
            if (cashtype != "0000")
            {
                fields += "|bankid:" + cashtype;
            }
            if (sorttype != null && sorttype.Trim() != "")
            {
                if (sorttype.Trim() == "1")
                {
                    fields += "|asc:Fpay_front_time_acc";
                }
                else if (sorttype.Trim() == "2")
                {
                    fields += "|desc:Fpay_front_time_acc";
                }
                else if (sorttype.Trim() == "3")
                {
                    fields += "|asc:Fnum";
                }
                else if (sorttype.Trim() == "4")
                {
                    fields += "|desc:Fnum";
                }
                else
                {
                    fields += "|desc:Fpay_front_time_acc";
                }
            }

            //2416:查询库表c2c_db.t_tcpay_list_$YYYY$$MM$
            //2427:查询库表c2c_db.t_tcpay_list_$YYYY$$MM$(spider)
            //2417:查询库表c2c_db.t_tcpay_list 
            //2431:查询库表c2c_db.t_tcpay_list_$YYYY$$MM$(SET2)
            DataSet ds = new DataSet();
            Hashtable param = new Hashtable();
            DateTime Datestime = Convert.ToDateTime(stime);
            if (Datestime > DateTime.Now.AddDays(-PickRecordKeepTime * 30))
            {
                param.Add("2416", fields);
                param.Add("2417", fields);
                param.Add("2431", fields);
                ds = multi_query(param, offset, limit);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    param.Clear();
                    param.Add("2427", fields);
                    ds = multi_query(param, offset, limit);
                }
            }
            else
            {
                param.Add("2427", fields);
                ds = multi_query(param, offset, limit);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    param.Clear();
                    param.Add("2416", fields);
                    param.Add("2417", fields);
                    param.Add("2431", fields);
                    ds = multi_query(param, offset, limit);
                }
            }
            return ds;
        }
        /// <summary>
        /// 按提记录按银行卡号查询
        /// </summary>
        /// <param name="abankid"></param>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <param name="fstate"></param>
        /// <param name="fnum"></param>
        /// <param name="banktype"></param>
        /// <param name="sorttype"></param>
        /// <param name="cashtype"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        private DataSet QueryPickByAbankid(string abankid, string stime, string etime, int fstate, float fnum, string banktype, string sorttype, string cashtype,
            int offset, int limit)
        {
            if (offset % limit == 1) offset -= 1;
            string fields = "abankid:" + abankid + "|begintime:" + stime + "|endtime:" + etime;

            if (fstate != 0)
            {
                fields += "|sign:" + fstate.ToString();
            }
            long num = (long)Math.Round(fnum * 100, 0);

            fields += "|num:" + num.ToString();

            if (banktype != "0000")
            {
                fields += "|banktype:" + banktype;
            }
            if (cashtype != "0000")
            {
                fields += "|bankid:" + cashtype;
            }
            if (sorttype != null && sorttype.Trim() != "")
            {
                if (sorttype.Trim() == "1")
                {
                    fields += "|asc:Fpay_front_time_acc";
                }
                else if (sorttype.Trim() == "2")
                {
                    fields += "|desc:Fpay_front_time_acc";
                }
                else if (sorttype.Trim() == "3")
                {
                    fields += "|asc:Fnum";
                }
                else if (sorttype.Trim() == "4")
                {
                    fields += "|desc:Fnum";
                }
                else 
                {
                    fields += "|desc:Fpay_front_time_acc";
                }
            }

            //2416:查询库表c2c_db.t_tcpay_list_$YYYY$$MM$
            //2427:查询库表c2c_db.t_tcpay_list_$YYYY$$MM$(spider)
            //2417:查询库表c2c_db.t_tcpay_list 
            //2431:查询库表c2c_db.t_tcpay_list_$YYYY$$MM$(SET2)
            DataSet ds = new DataSet();
            Hashtable param = new Hashtable();
            DateTime Datestime = Convert.ToDateTime(stime);
            if (Datestime > DateTime.Now.AddDays(-PickRecordKeepTime * 30))
            {
                param.Add("2416", fields);
                param.Add("2417", fields);
                param.Add("2431", fields);
                ds = multi_query(param, offset, limit);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    param.Clear();
                    param.Add("2427", fields);
                    ds = multi_query(param, offset, limit);
                }
            }
            else
            {
                param.Add("2427", fields);
                ds = multi_query(param, offset, limit);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    param.Clear();
                    param.Add("2416", fields);
                    param.Add("2417", fields);
                    param.Add("2431", fields);
                    ds = multi_query(param, offset, limit);
                }
            }
            return ds;
        }
        /// <summary>
        /// 查询提现拦截记录
        /// </summary>
        /// <param name="fetchListid"></param>
        /// <returns></returns>
        public DataTable GetFetchListIntercept(string fetchListid)
        {
            string sql = string.Format(@"SELECT * FROM c2c_zwdb.t_fetch_listid_record  WHERE Ffetch_listid='{0}'", fetchListid);
            using (var da = MySQLAccessFactory.GetMySQLAccess("FetchListIntercept"))
            {
                da.OpenConn();
                return da.GetTable(sql);
            }
        }

        /// <summary>
        /// 添加提现拦截记录
        /// </summary>
        /// <param name="fetchListid">提现单号</param>
        public bool AddFetchListIntercept(string fetchListid, string opera)
        {
            try
            {
                using (var da = MySQLAccessFactory.GetMySQLAccess("FetchListIntercept"))
                {
                    da.OpenConn();
                    string Sql = string.Format("insert into c2c_zwdb.t_fetch_listid_record " +
                        "(Ffetch_listid,Foperator,Fmodify_type)" +
                        "values('{0}','{1}','{2}')",
                      fetchListid, opera, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (!da.ExecSql(Sql))
                    {
                        LogHelper.LogInfo("添加提现拦截记录");
                        throw new Exception("添加提现拦截记录出错");
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                string err = "添加提现拦截记录出错：" + ex;
                LogHelper.LogError(err);
                throw new Exception(err);
            }
        }


        #region 提现记录查询 From DB


        /// <summary>
        /// 根据用户内部帐号获取提现单库表名
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        private static string GetPayListTableFromUID(string uid)
        {
            try
            {
                if (!string.IsNullOrEmpty(uid))
                {
                    string db = "c2c_db_" + uid.Substring(uid.Length - 2, 2);
                    string table = "t_tcpay_list_" + uid.Substring(uid.Length - 3, 1);
                    return db + "." + table;
                }
                else
                    throw new Exception("根据UID查询提现单库表名时UID为空!!!");
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError("根据用户内部帐号获取提现单库表名异常" + ex, "GetPayListTableFromUID");
                throw new Exception("根据用户内部帐号获取提现单库表名异常" + ex.Message);
            }
        }



        /// <summary>
        /// 获取提现单备机所在DB(根据UID分库分表c2c_db_xx.t_tcpay_list_x)
        /// </summary>
        /// <returns></returns>
        private static DataSet GetTCPBackUpDataBase(string table,string commWhere)
        {
            MySqlAccess daTCP2 = null;

            DataSet pickData = new DataSet();

            try
            {
                 daTCP2 = MySQLAccessFactory.GetMySQLAccess("TCP2");

                string strSql = " select Faid,Fuid,Faname,Facc_name,Fnum,Fcharge,FaBankID,Fsign,Fpay_front_time,Fpay_front_time_acc,Ftde_id,Fbankid,Fstate,'' as FRTFlagName,''as FPayBankName ,'' as FNewNum ,'' as FStateName ,Flistid ,Fmemo,Fabank_type,'' as FabanktypeName , '' as FNewCharge ,Frefund_ticket_flag,Fbank_type,'' as  FPayBankName,'' as  FRTFlagName,Fsp_batch  from " + table + "  " + commWhere + "  ";
                daTCP2.OpenConn();

                 pickData = daTCP2.dsGetTotalData(strSql);
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("GetTCPBackUpDataBase");
                if (log.IsErrorEnabled) log.Error(ex);
            }
            finally
            {
                try
                {
                    if (daTCP2 != null)
                    {
                        daTCP2.CloseConn();
                    }
                }
                catch (Exception exdb)
                {
                    log4net.ILog log = log4net.LogManager.GetLogger("GetTCPBackUpDataBase");
                    if (log.IsErrorEnabled) log.Error(exdb);
                }
            }

            return pickData;

        }

        private static string GetCreditEncode(string xbankaccno, MySqlAccess da)
        {
            if (xbankaccno == null || xbankaccno.Trim() == "")
                return "";

            if (xbankaccno.Trim().Length < 9)
                return "无效" + xbankaccno;

            try
            {
                da.OpenConn();

                xbankaccno = xbankaccno.Trim();
                string md5value = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(xbankaccno, "md5").ToLower();

                string strSql = "select seqno from c2c_db.t_xyk_secret where cardno_md5='" + md5value + "'";
                string seqno = da.GetOneResult(strSql);

                //允许明文存在。
                if (seqno == null || seqno == "")
                    return xbankaccno;

                int ilen = xbankaccno.Length;
                return "X" + xbankaccno.Substring(0, 4) + seqno.PadLeft(12, '0') + xbankaccno.Substring(ilen - 4, 4);
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("GetCreditEncode");
                if (log.IsErrorEnabled) log.Error(ex);
                return "";
            }
            finally {
                try
                {
                    if (da != null) {
                        da.CloseConn();
                    }
                }
                catch (Exception exdb)
                {
                    log4net.ILog log = log4net.LogManager.GetLogger("GetCreditEncode");
                    if (log.IsErrorEnabled) log.Error(exdb);
                }
            }

        }

        /// <summary>
        /// 获取set库连接串
        /// </summary>
        /// <param name="setIndex">set序号，从1开始</param>
        /// <returns></returns>
        private static string GetSetConnString(int setIndex)
        {
            string localKey = "PayListSet" + setIndex.ToString();
            return ConnectionStringHelper.Get(localKey);
        }

        /// <summary>
        /// 通用批量查询(直查DB，多个set库的结果合集)
        /// </summary>
        /// <param name="sql">查询sql语句</param>
        /// <returns></returns>
        private static DataSet GetList(string strSql)
        {
            int setNum = PayList_Set_Num();
            MySqlAccess da = new MySqlAccess(GetSetConnString(1));//Set1
            try
            {
                da.OpenConn();
                DataSet ds1 = da.dsGetTotalData(strSql);//查询set1    

                //查询其它set
                for (int index = 2; index <= setNum; index++)
                {
                    da.Dispose();
                    da = new MySqlAccess(GetSetConnString(index));//其它set的DB连接
                    da.OpenConn();
                    DataSet dsOther = da.dsGetTotalData(strSql);//查询其它set
                    if (ds1 != null && ds1.Tables != null && ds1.Tables[0].Rows.Count > 0)
                    {
                        ds1.Merge(dsOther);//合并结果集
                    }
                    else
                    {
                        ds1 = dsOther;
                    }
                }
                #region 查歷史庫
                try
                {
                    da.Dispose();
                    da = new MySqlAccess(CFT.Apollo.Data.ConnectionString.ConnectionStringHelper.Get("PayListSpider1"));
                    da.OpenConn();
                    DataSet dsPayListHistory = da.dsGetTotalData(strSql);
                    if (ds1 != null && ds1.Tables != null && ds1.Tables[0].Rows.Count > 0)
                    {
                        ds1.Merge(dsPayListHistory);
                    }
                    else
                        ds1 = dsPayListHistory;
                }
                catch (System.Exception ex)
                {
                    LogHelper.LogError("提現查詢歷史庫表異常" + ex, "PayListHelper.GetList");
                }
                #endregion

                return DeleteDuplicateRow(ds1, "Flistid");//去掉重复的提现记录
            }
            catch (Exception ex)
            {
                LogHelper.LogError("通用批量查询(查询多个set库的结果合集)出现异常" + ex, "PayListHelper.GetList");
                throw new Exception("通用批量查询(查询多个set库的结果合集)出现异常：" + ex.ToString());
            }
            finally
            {
                da.Dispose();
            }
        }



        /// <summary>
        /// 刪除Set1和Spider查询出来的重复记录
        /// </summary>
        /// <param name="dsOriginal">查询结果</param>
        /// <param name="columnName">列键值</param>
        /// <returns>无重复记录的结果</returns>
        private static DataSet DeleteDuplicateRow(DataSet dsOriginal, string columnName)
        {
            if (dsOriginal != null && dsOriginal.Tables.Count > 0 && dsOriginal.Tables[0].Rows.Count > 1)
            {
                DataSet dsDistinct = new DataSet();
                DataTable dt = dsOriginal.Tables[0].Clone();
                dsDistinct.Tables.Add(dt);
                foreach (DataRow dr in dsOriginal.Tables[0].Rows)
                {
                    bool isExist = false;
                    for (int i = 0; i < dsDistinct.Tables[0].Rows.Count; i++)
                    {
                        if (dr[columnName].ToString() == dsDistinct.Tables[0].Rows[i][columnName].ToString())
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist)
                    {
                        dsDistinct.Tables[0].ImportRow(dr);
                    }
                }
                return dsDistinct;
            }
            else
                return dsOriginal;
        }


        /// <summary>
        /// 提现单set数量
        /// </summary>
        /// <returns></returns>
        private static int PayList_Set_Num()
        {
            if (ConfigurationManager.AppSettings["PayList_Set_Num"] != null && ConfigurationManager.AppSettings["PayList_Set_Num"] != "")
            {
                return int.Parse(ConfigurationManager.AppSettings["PayList_Set_Num"]);
            }
            else
            {
                return 4;
            }
        }


        /// <summary>
        /// 根据提现单号获取提现单库表名
        /// </summary>
        /// <param name="listid">提现单号</param>
        /// <param name="currtable">当月月表</param>
        /// <param name="othertable">其它月表（前一月或后一月）</param>
        private static void GetPayListTableFromID(string listid, out string currtable, out string othertable)
        {
            //1、3位系统ID+YYYYMMDD+10位流水号；
            //2、3位系统ID+10位商户号+YYYYMMDD+7位序列号
            //20151203　提现单号中日期的年份前两位“20”用作set标识了

            string strdate = "";
            if (listid.Length == 21)//3位业务编码 + 8位日期 + 10位序列号
            {
                //strdate = listid.Substring(3,4) + "-" + listid.Substring(7,2) + "-" + listid.Substring(9,2);
                strdate = "20" + listid.Substring(5, 2) + "-" + listid.Substring(7, 2) + "-" + listid.Substring(9, 2);
            }
            else if (listid.Length == 28)//3位业务编码 + 10位商户号+8位日期+7位序列号
            {
                //strdate = listid.Substring(13,4) + "-" + listid.Substring(17,2) + "-" + listid.Substring(19,2);
                strdate = "20" + listid.Substring(15, 2) + "-" + listid.Substring(17, 2) + "-" + listid.Substring(19, 2);
            }
            else if (listid.Length == 26)//3位业务编码+2位机器所在IDC标识+3位机器编号+2位提现单SET标识+6位日期+10位序列号
            {
                strdate = "20" + listid.Substring(10, 2) + "-" + listid.Substring(12, 2) + "-" + listid.Substring(14, 2);
            }
            DateTime dt;
            try
            {
                dt = DateTime.Parse(strdate);
            }
            catch (System.Exception ex)
            {
                dt = DateTime.Now;
            }


            GetPayListTableFromTime(dt, out currtable, out othertable);
        }


        /// <summary>
        /// 根据日期获取提现单库表名
        /// </summary>
        /// <param name="datetime">日期</param>
        /// <param name="currtable">当月月表</param>
        /// <param name="othertable">其它月表（前一月或后一月）</param>
        public static void GetPayListTableFromTime(DateTime datetime, out string currtable, out string othertable)
        {
            if (datetime < DateTime.Parse("2012-04-01"))
            {
                currtable = "";
                othertable = "";
                return;
            }

            currtable = "c2c_db.t_tcpay_list_" + datetime.ToString("yyyyMM");

            if (datetime.Day > 15)
            {
                othertable = "c2c_db.t_tcpay_list_" + datetime.AddMonths(1).ToString("yyyyMM");
            }
            else
            {
                othertable = "c2c_db.t_tcpay_list_" + datetime.AddMonths(-1).ToString("yyyyMM");
            }
        }


        /// <summary>
        /// 根据提现单ID查询记录(如果查不到，调用下全量查询)
        /// </summary>
        /// <param name="listid">提现单号</param>
        /// <param name="strSql">查询sql</param>
        /// <returns></returns>
        public static DataTable GetDataByListID(string listID, string strSql)
        {
            MySqlAccess ma = GetMaByListID(listID);
            try
            {
                ma.OpenConn();
                DataTable dt = ma.GetTable(strSql);
                if (dt == null || dt.Rows.Count < 1)//没查到，查下历史提现Spider
                {
                    DataSet ds = GetList(strSql);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        return ds.Tables[0].Copy();
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("PayListHelper.GetDataByListID");
                if (log.IsErrorEnabled) log.Error("根据提现单ID查询记录出现异常，listID:" + listID, ex);

                throw new Exception("根据提现单ID查询记录出现异常，listID:" + listID + ",异常信息：" + ex.ToString());
            }
            finally
            {
                ma.Dispose();
            }
        }


        /// <summary>
        /// 获取提现单所在set的DB
        /// </summary>
        /// <param name="listID">提现单ID</param>
        /// <returns></returns>
        public static MySqlAccess GetMaByListID(string listID)
        {
            //MySqlAccess da = new MySqlAccess(GetSetConnString(1)); //默认set1 DB，tcpay list
            int index = 1;
            listID = listID.Trim();
            if (listID.Length == 26)
            {
                string set = listID.Substring(8, 2);
                //int setIndex = int.Parse(listID.Substring(8, 2));
                switch (set)
                {
                    case "02":
                        index = 2;
                        break;
                    case "03":
                        index = 3;
                        break;
                    case "04":
                        index = 4;
                        break;
                    default:
                        index = 1;
                        break;
                }
            }
            else if (listID.Length == 21)
            {
                string set = listID.Substring(3, 2);
                switch (set)
                {
                    case "02":
                        index = 2;
                        break;
                    case "03":
                        index = 3;
                        break;
                    case "04":
                        index = 4;
                        break;
                    default:
                        index = 1;
                        break;
                }
            }
            else
            {
                //其它的都是set1 DB
            }
            return new MySqlAccess(GetSetConnString(index));
        }


        public DataSet GetPickListFromDB(string u_ID,int idtype, DateTime u_BeginTime, DateTime u_EndTime, int fstate, float fnum, string banktype,  string sorttype, string fpaybankType,
            int iPageStart, int iPageMax)
        {
            try
            {
                #region commWhere
                string commWhere = " where  Fpay_front_time_acc between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' and Fbankid!=4   ";
                if (fstate != 0 && fstate <= 5)
                {
                    commWhere += " and FSign=" + fstate.ToString() + " ";
                }
                if (fstate >= 6)//退票 是付款成功
                {
                    commWhere += " and Fstate=2  and FSign=1";
                    if (fstate == 6) //0：未退票
                    {
                        commWhere += " and  Frefund_ticket_flag=0";
                    }
                    if (fstate == 7)//1：已退票
                    {
                        commWhere += "and  Frefund_ticket_flag=1";
                    }
                }
                if (fpaybankType != "0000")
                {
                    commWhere += " and Fstate<>1 ";
                }
                long num = (long)Math.Round(fnum * 100, 0);
                commWhere += " and Fnum>=" + num.ToString() + " ";
                if (banktype != "0000")//提现银行
                {
                    commWhere += " and Fabank_type=" + banktype + " ";
                }
                #endregion

                #region 根据帐号、银行卡、提现单号确定库表名
                if (!string.IsNullOrEmpty(u_ID))
                {
                    if (idtype == 0)//帐号
                    {
                        string uid = PublicRes.ConvertToFuid(u_ID);
                        if (!string.IsNullOrEmpty(uid))
                        {
                            commWhere += "  and   Fuid='" + uid + "' ";
                            string table = GetPayListTableFromUID(uid);
                            var pickDS = GetTCPBackUpDataBase(table,commWhere);

                            return pickDS;
                        }
                        else
                            throw new Exception("提现查询函数未查出输入帐号" + u_ID + "的内部帐号，请核实或使用银行卡号or提现单号进行查询。");
                    }
                    else if (idtype == 1)//银行卡,需根据时间去查不同的set
                    {
                        string newbankacc = "";
                        if (u_ID != "" && !u_ID.StartsWith("X"))
                        {
                            //查询对应的加密卡号
                            MySqlAccess daxyk = new MySqlAccess("XYK");
                            newbankacc = GetCreditEncode(u_ID, daxyk);
                        }
                        if (newbankacc.StartsWith("X"))
                        {
                            commWhere += "  and (Fabankid='" + u_ID + "' or Fabankid='" + newbankacc + "')";
                        }
                        else
                        {
                            commWhere += "  and Fabankid='" + u_ID + "' ";
                        }

                        DateTime tmpDate = u_BeginTime;
                        bool flag = false;
                        string strGroup = "";
                        while (tmpDate <= u_EndTime && tmpDate > DateTime.Parse("2012-04-01")) //旧数据没有历史表
                        {
                            string TableName = "c2c_db.t_tcpay_list_" + tmpDate.ToString("yyyyMM");
                            if (flag)
                            {
                                strGroup += "  union all  ";
                            }
                            strGroup = strGroup + " select Faid,Fuid,Faname,Facc_name,Fnum,Fcharge,FaBankID,Fsign,Fpay_front_time,Fpay_front_time_acc,Ftde_id,Fbankid,Fstate,'' as FRTFlagName,''as FPayBankName ,'' as FNewNum ,'' as FStateName ,Flistid ,Fmemo,Fabank_type,'' as FabanktypeName , '' as FNewCharge ,Frefund_ticket_flag,Fbank_type,'' as  FPayBankName,'' as  FRTFlagName,Fsp_batch  from " + TableName + "  " + commWhere + "  ";

                            tmpDate = tmpDate.AddMonths(1);

                            string strTmp = tmpDate.ToString("yyyy-MM-");
                            tmpDate = DateTime.Parse(strTmp + "01 00:00:01");
                            flag = true;
                        }
                        return GetList(strGroup);
                    }
                    else if (idtype == 2)
                    {
                        commWhere += "  and Flistid='" + u_ID + "' ";
                        string strGroup = "";
                        string currTable = "";
                        string otherTable = "";
                        GetPayListTableFromID(u_ID, out currTable, out otherTable);
                        if (!string.IsNullOrEmpty(currTable))
                        {
                            strGroup += "  select Faid,Fuid,Faname,Facc_name,Fnum,Fcharge,FaBankID,Fsign,Fpay_front_time,Fpay_front_time_acc,Ftde_id,Fbankid,Fstate,'' as FRTFlagName,''as FPayBankName ,'' as FNewNum ,'' as FStateName ,Flistid ,Fmemo,Fabank_type,'' as FabanktypeName ,'' as FNewCharge ,Frefund_ticket_flag,Fbank_type,'' as  FPayBankName ,'' as  FRTFlagName,Fsp_batch from " + currTable + "   where  Flistid='" + u_ID + "' union all ";
                            strGroup += "  select Faid,Fuid,Faname,Facc_name,Fnum,Fcharge,FaBankID,Fsign,Fpay_front_time,Fpay_front_time_acc,Ftde_id,Fbankid,Fstate,'' as FRTFlagName,''as FPayBankName ,'' as FNewNum ,'' as FStateName ,Flistid ,Fmemo,Fabank_type,'' as FabanktypeName ,'' as FNewCharge ,Frefund_ticket_flag,Fbank_type,'' as  FPayBankName ,'' as  FRTFlagName,Fsp_batch  from " + otherTable + "   where Flistid='" + u_ID + "'  ";
                        }
                        DataTable dt = GetDataByListID(u_ID, strGroup);
                        DataSet ds = new DataSet();
                        ds.Tables.Add(dt);
                        return ds;
                    }
                }
                #endregion

                #region 根据时间段查询
                DateTime tmpDate1 = u_BeginTime;
                bool flag1 = false;
                string strGroup1 = "";
                while (tmpDate1 <= u_EndTime && tmpDate1 > DateTime.Parse("2012-04-01")) //旧数据没有历史表
                {
                    string TableName = "c2c_db.t_tcpay_list_" + tmpDate1.ToString("yyyyMM");
                    if (flag1)
                    {
                        strGroup1 += "  union all  ";
                    }
                    strGroup1 = strGroup1 + " select Faid,Fuid,Faname,Facc_name,Fnum,Fcharge,FaBankID,Fsign,Fpay_front_time,Fpay_front_time_acc,Ftde_id,Fbankid,Fstate,'' as FRTFlagName,''as FPayBankName ,'' as FNewNum ,'' as FStateName ,Flistid ,Fmemo,Fabank_type,'' as FabanktypeName , '' as FNewCharge ,Frefund_ticket_flag,Fbank_type,'' as  FPayBankName,'' as  FRTFlagName,Fsp_batch  from " + TableName + "  " + commWhere + "  ";

                    tmpDate1 = tmpDate1.AddMonths(1);

                    string strTmp = tmpDate1.ToString("yyyy-MM-");
                    tmpDate1 = DateTime.Parse(strTmp + "01 00:00:01");
                    flag1 = true;
                }
                return GetList(strGroup1);
                #endregion
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError("提现查询异常" + ex, "GetPickList");
                throw new Exception("提现查询异常" + ex.Message);
            }
        }

        #endregion



        //信用卡还款按财付通号查询
        public DataSet GetCreditQueryListForFaid(string QQOrEmail, DateTime begindate, DateTime enddate, int offset, int limit)
        {
            //2418:查询库表c2c_db.t_tcpay_list_$YYYY$$MM$
            //2419:查询库表c2c_db.t_tcpay_list
            //2428:查询库表c2c_db.t_tcpay_list_$YYYY$$MM$(spider)
            //2432:查询库表c2c_db.t_tcpay_list_$YYYY$$MM$(set2)
            if (offset % limit == 1) offset -= 1;

            string Fuid = PublicRes.ConvertToFuid(QQOrEmail);
            //#if DEBUG
            //             Fuid = QQOrEmail;
            //#endif
            string fields = "Fuid:{0}|begindate:{1}|enddate:{2}";
            fields = string.Format(fields, Fuid, begindate.ToString("yyyy-MM-dd 00:00:00"), enddate.ToString("yyyy-MM-dd 23:59:59"));

            Hashtable param = new Hashtable();
            param.Add("2418", fields);
            param.Add("2419", fields);
            param.Add("2428", fields);
            param.Add("2432", fields);
            DataSet ds = multi_query(param, offset, limit);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                //加查上月或者下月的表
                DataSet ds2 = new DataSet();
                fields = "Fuid:{0}|begindate:{1}|enddate:{2}";
                begindate = begindate.Day > 15 ? begindate.AddMonths(1) : begindate.AddMonths(-1);
                if (begindate > enddate)
                {
                    enddate = begindate;
                }
                fields = string.Format(fields, Fuid, begindate.ToString("yyyy-MM-01"), enddate.ToString("yyyy-MM-dd"));
                param.Clear();
                param.Add("2418", fields);
                param.Add("2428", fields);
                param.Add("2432", fields);
                ds = multi_query(param, offset, limit);
            }
            return ds;
        }
        //信用卡还款根据还款交易号查询
        public DataSet GetCreditQueryList(string Flistid, int offset, int limit)
        {
            //2420:查询库表c2c_db.t_tcpay_list
            //2421:查询库表c2c_db.t_tcpay_list_$YYYY$$MM$
            //2429:查询库表c2c_db.t_tcpay_list_$YYYY$$MM$(spider)
            //2433:查询库表c2c_db.t_tcpay_list_$YYYY$$MM$(set2)
            if (offset % limit == 1) offset -= 1;
            Hashtable param = new Hashtable();
            string fields = "listid:{0}";
            fields = string.Format(fields, Flistid);
            param.Add("2420", fields);

            DateTime date = GetPayListTableFromID(Flistid);
            fields = "listid:{0}|date:{1}";
            fields = string.Format(fields, Flistid, date.ToString("yyyy-MM-dd"));
            param.Add("2421", fields);
            param.Add("2429", fields);
            param.Add("2433", fields);
            DataSet ds = multi_query(param, offset, limit);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                //加查上月或者下月的表
                fields = "listid:{0}|date:{1}";
                date = date.Day > 15 ? date.AddMonths(1) : date.AddMonths(-1);
                fields = string.Format(fields, Flistid, date.ToString("yyyy-MM-dd"));

                param.Clear();
                param.Add("2421", fields);
                param.Add("2429", fields);
                param.Add("2433", fields);
                ds = multi_query(param, offset, limit);
            }
            return ds;
        }
        //结算查询 商户今天结算记录 c2c_db_00.t_tranlog_0 不存在
        public DataSet GetQuerySettlementTodayList(string Fspid) 
        {
            string fields = "spid:{0}|begindate:{1}|enddate:{2}";
            fields = string.Format(fields, Fspid, DateTime.Today.ToString("yyyy-MM-dd"), DateTime.Today.AddDays(1).ToString("yyyy-MM-dd"));
            DataSet ds = new PublicRes().QueryCommRelay8020("2422", fields, 0, 1);
            return ds;
        }

        public string TdeToID(string tdeid)  //传入付款单ID
        {
            string tmp = "";
            string fields = "tdeid:{0}";
            fields = string.Format(fields, tdeid);
            DataSet ds = new PublicRes().QueryCommRelay8020("2425", fields, 0, 1);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                tmp = ds.Tables[0].Rows[0]["Flistid"].ToString();
            }

            if (tmp == null || tmp.ToString().Trim() == "")
            {
                tmp = PublicRes.ExecuteOne("select Flistid from c2c_db.t_refund_list where Frlistid = (select FListID from c2c_db.t_tcpay_list where ftde_id='" + tdeid + "' and Fsubject=4 )", "ywb");  //执行并返回结果
                //return "0";
            }
            return tmp.ToString().Trim();
        }

            /// <summary>
       ///提现记录加查spider
       /// </summary>
       /// <param name="fields"></param>
       /// <returns></returns>
        private DataSet multi_query(Hashtable param, int offset = 0, int limit = 1, bool SelectAll = true)
        {
            string msg = "";
            DataSet ds = new DataSet();

            foreach (DictionaryEntry de in param)
            {
                try
                {
                    DataSet dstemp = new PublicRes().QueryCommRelay8020(de.Key.ToString(), de.Value.ToString(), offset, limit);
                    ds = PublicRes.ToOneDataset(ds, dstemp);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) 
                    {
                        if (SelectAll == true)
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }            
                }
                catch(Exception ef) {
                    LogHelper.LogError("CFT.CSOMS.DAL.TradeModule.PickData   private DataSet multi_query(Hashtable param, int offset = 0, int limit = 1, bool SelectAll = true),异常："+ef.ToString());
                }
            }
            return ds;
        }
        private DateTime GetPayListTableFromID(string listid)
        {
  
            string strdate = "";
            if (listid.Length == 21)//3位业务编码 + 8位日期 + 10位序列号
            {
                //strdate = listid.Substring(3,4) + "-" + listid.Substring(7,2) + "-" + listid.Substring(9,2);
                strdate = "20" + listid.Substring(5, 2) + "-" + listid.Substring(7, 2) + "-" + listid.Substring(9, 2);
            }
            else if (listid.Length == 28)//3位业务编码 + 10位商户号+8位日期+7位序列号
            {
                //strdate = listid.Substring(13,4) + "-" + listid.Substring(17,2) + "-" + listid.Substring(19,2);
                strdate = "20" + listid.Substring(15, 2) + "-" + listid.Substring(17, 2) + "-" + listid.Substring(19, 2);
            }
            else if (listid.Length == 26)//3位业务编码+2位机器所在IDC标识+3位机器编号+2位提现单SET标识+6位日期+10位序列号
            {
                strdate = "20" + listid.Substring(10, 2) + "-" + listid.Substring(12, 2) + "-" + listid.Substring(14, 2);
            }
            DateTime dt;
            try
            {
                dt = DateTime.Parse(strdate);
            }
            catch (System.Exception ex)
            {
                dt = DateTime.Now;
            }
            return dt;
        }
    }
}