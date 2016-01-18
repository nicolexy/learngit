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
        //提现记录查询
        public DataSet GetPickList(string u_ID, int idtype, DateTime u_BeginTime, DateTime u_EndTime, int fstate, float fnum, string banktype, string sorttype, string cashtype,
            int offset, int limit)
        {
            string stime = u_EndTime.ToString("yyyy-MM-dd");
            string etime = u_EndTime.ToString("yyyy-MM-dd HH:mm:ss");
            if (idtype == 0)
            {
                //按uid查询
                string uid = PublicRes.ConvertToFuid(u_ID);
                return QueryPickByUid(uid, stime, etime, fstate, banktype, cashtype, offset, limit);
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
                return QueryPickByListid(u_ID);
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
        public DataSet QueryPickByListid(string listid)
        {
            //listid=101201509246269862740
            string requestString = "transaction_id={0}&MSG_NO={1}";
            requestString = string.Format(requestString, listid, "101779" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.NewStaticNoManage());

            string answer = RelayAccessFactory.RelayInvoke(requestString, "101779", false, false, serverIp, serverPort);
            answer = System.Web.HttpUtility.UrlDecode(answer, System.Text.Encoding.GetEncoding("utf-8"));
            if (!answer.Contains("result=0&res_info=ok"))
            {
                throw new Exception("按提现单号查询报错:" + answer);
            }
            answer = answer.Replace("result=0&res_info=ok&row_num=1&row0=", "");
            var list = StringEx.ToDictionary(answer, '&', '=');
            DataTable dt = new DataTable();
            dt.Columns.Add("Flistid", typeof(string));
            foreach (var item in list)
            {
                dt.Columns.Add("F" + item.Key, typeof(string));
            }
            DataRow dr = dt.NewRow();
            dr["Flistid"] = list["transaction_id"];
            foreach (var item in list)
            {
                dr["F" + item.Key] = item.Value;
            }
            dt.Rows.Add(dr);
            return new DataSet() { Tables = { dt } };
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
        private DataSet QueryPickByUid(string uid, string stime, string etime, int fstate, string banktype, string cashtype, int offset, int limit)
        {
            if (offset % limit == 1) offset -= 1;
            string fields = "uid:" + uid + "|cur_type:1";
            if (!string.IsNullOrEmpty(stime))
            {
                fields = "|s_time:" + stime;
            }
            if (!string.IsNullOrEmpty(etime))
            {
                fields = "|e_time:" + etime;
            }
            if (fstate != 0)
            {
                fields += "|sign:" + fstate.ToString();
            }
            if (banktype != "0000")
            {
                fields += "|bank_type:" + banktype;
            }
            if (cashtype != "0000")
            {
                fields += "|bankid_list:" + cashtype;
            }

            fields = string.Format(fields, uid, stime, etime);
            //#if DEBUG
            //            fields = "uid:295169794|cur_type:1";
            //#endif
            DataSet ds = new PublicRes().QueryCommRelay8020("409", fields, offset, limit);

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
            if (offset % limit == 1) offset -= 1;
            string fields = "";
            fields += "|abankid:" + abankid;

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
            }
            fields += "|begintime:" + stime + "|endtime:" + etime;
            if (fields.StartsWith("|"))
            {
                fields = fields.Substring(1, fields.Length - 1);
            }

            //2416:查询库表c2c_db.t_tcpay_list_$YYYY$$MM$
            //2427:查询库表c2c_db.t_tcpay_list_$YYYY$$MM$(spider)
            //2417:查询库表c2c_db.t_tcpay_list 
            //2431:查询库表c2c_db.t_tcpay_list_$YYYY$$MM$(SET2)
            Hashtable param = new Hashtable();
            param.Add("2416", fields);
            param.Add("2417", fields);
            param.Add("2427", fields);
            param.Add("2431", fields);
            DataSet ds = multi_query(param, offset, limit);
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
                string err = "添加提现拦截记录出错：" + ex.Message;
                LogHelper.LogInfo(err);
                throw new Exception(err);
            }
        }

   
        //信用卡还款按财付通号查询
        public DataTable  GetCreditQueryListForFaid(string QQOrEmail, DateTime begindate, DateTime enddate, int offset, int limit)
        {

        //     select Fbank_type,Flistid,Fsign,Fbank_name,RIGHT(Fabankid,4) AS creditcard_id,Fnum,Fpay_front_time from c2c_db.t_tcpay_list_$YYYY$$MM$  \
        //WHERE Fuid= '$Fuid$' \
        //AND Fpay_front_time_acc &gt;='$begindate$' \
        //AND Fpay_front_time_acc &lt;='$enddate$' \
        //AND Fbankid IN (5,8,36,37) \
        //AND Fmodify_time &gt;= '$begindate$'\
        //AND Fmodify_time &lt;= '$enddate$' \
        //AND Fbank_type !=  2033 \
        //ORDER BY Fpay_front_time DESC \

            if (offset % limit == 1) offset -= 1;
            Hashtable param = new Hashtable();

            string Fuid = PublicRes.ConvertToFuid(QQOrEmail);
//#if DEBUG
//            Fuid = "295169794";
//#endif
            string stime = begindate.ToString("yyyy-MM-dd 00:00:00");
            string etime=enddate.ToString("yyyy-MM-dd 23:59:59");


            string fields = "uid:{0}|stime:{1}|futuretime:{2}|ftime:{3}";
            fields = string.Format(fields, Fuid, stime, etime, etime);
 

            param.Add("104", fields);
            DataSet ds = multi_query(param, offset, limit);
            return ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
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
        private DataSet multi_query(Hashtable param, int offset = 0, int limit = 1)
        {
            string msg = "";
            DataSet ds = new DataSet();

            foreach (DictionaryEntry de in param)
            {
                try
                {
                    DataSet dstemp = new PublicRes().QueryCommRelay8020(de.Key.ToString(), de.Value.ToString(), offset, limit);
                    ds = PublicRes.ToOneDataset(ds, dstemp);
                }
                catch { }
            }
            return ds;
        }
        private DateTime GetPayListTableFromID(string listid)
        {
            //1、3位系统ID+YYYYMMDD+10位流水号；
            //2、3位系统ID+10位商户号+YYYYMMDD+7位序列号

            string strdate = "";
            if (listid.Length == 21)
            {
                strdate = listid.Substring(3, 4) + "-" + listid.Substring(7, 2) + "-" + listid.Substring(9, 2);
            }
            else if (listid.Length == 28)
            {
                strdate = listid.Substring(13, 4) + "-" + listid.Substring(17, 2) + "-" + listid.Substring(19, 2);
            }

            DateTime dt = DateTime.Now;
            try
            {
                dt = DateTime.Parse(strdate);
            }
            catch
            {
                dt = DateTime.Now;
            }
            return dt;
        }
    }
}