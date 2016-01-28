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

namespace CFT.CSOMS.DAL.TradeModule
{
    public class PickData
    {
        string serverIp = System.Configuration.ConfigurationManager.AppSettings["Relay_IP"].ToString();
        int serverPort = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Relay_PORT"].ToString());
        int PickRecordKeepTime = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("PickRecordKeepTime", 3);
        //提现记录查询
        public DataSet GetPickList(string u_ID, int idtype, DateTime u_BeginTime, DateTime u_EndTime, int fstate, float fnum, string banktype, string sorttype, string cashtype,
            int offset, int limit)
        {
            string stime = u_BeginTime.ToString("yyyy-MM-dd");
            string etime = u_EndTime.ToString("yyyy-MM-dd HH:mm:ss");
            if (idtype == 0)
            {
                //按uid查询
                string uid = PublicRes.ConvertToFuid(u_ID);
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