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

namespace CFT.CSOMS.DAL.TradeModule
{
    public class PickData
    {
        //提现记录查询
        public DataSet GetPickList(string u_ID, DateTime u_BeginTime, DateTime u_EndTime, int fstate, float fnum, string banktype, int idtype, string sorttype, string cashtype,
            int iPageStart, int iPageMax)
        {
            try
            {
                DataSet ds = null;
                ds = PickQueryClass(u_ID, u_BeginTime, u_EndTime, fstate, fnum, banktype, sorttype, idtype, cashtype, false, iPageStart, iPageMax);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    if (idtype == 1)
                    {
                        ds = PickQueryClass(u_ID, u_BeginTime, u_EndTime, fstate, fnum, banktype, sorttype, idtype, cashtype, true, iPageStart, iPageMax);
                    }
                }
                return ds;
            }
            catch (Exception err)
            {
                throw new Exception("处理失败！" + err.Message);
            }
        }

       public  DataSet GetPickListDetail(string listid, DateTime u_BeginTime, DateTime u_EndTime)
        {
            DataSet ds = new DataSet();
            u_BeginTime = GetPayListTableFromID(listid);
            string fields = "listid:{0}|begintime:{1}|endtime:{2}";

            fields = string.Format(fields, listid, u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss"), u_EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
            DataSet ds1 = new PublicRes().QueryCommRelay8020("2416", fields, 0, 1);
            ds = PublicRes.ToOneDataset(ds, ds1);
            DataSet ds2 = new PublicRes().QueryCommRelay8020("2417", fields, 0, 1);
            ds = PublicRes.ToOneDataset(ds, ds2);

           //加查上月或者下月的表
            DataSet ds3 = new DataSet();
            fields = "listid:{0}|begintime:{1}|endtime:{2}";
            u_BeginTime = u_BeginTime.Day > 15 ? u_BeginTime.AddMonths(1) : u_BeginTime.AddMonths(-1);
            if (u_BeginTime > u_EndTime)
            {
                u_EndTime = u_BeginTime;
            }
            fields = string.Format(fields, listid, u_BeginTime.ToString("yyyy-MM-01"), u_EndTime.ToString("yyyy-MM-dd"));
            ds3 = new PublicRes().QueryCommRelay8020("2416", fields, 0, 1);
            ds = PublicRes.ToOneDataset(ds, ds3);
            return ds;
        }


         private DataSet PickQueryClass(string u_ID, DateTime u_BeginTime, DateTime u_EndTime, int fstate, float fnum, string banktype, string sorttype, int idtype, string cashtype, bool isSecret, int iPageStart, int iPageMax)
        {
            string fields = "";
            DataSet ds = new DataSet();
            if (u_ID != null && u_ID.Trim() != "")
            {
                if (idtype == 0)
                {
                    string uid = PublicRes.ConvertToFuid(u_ID);
                    fields += "|uid:" + uid;
                }
                else if (idtype == 1)
                {
                    if (isSecret)
                    {
                        string bankID = BankLib.BankIOX.GetCreditEncode(u_ID, BankLib.BankIOX.fxykconn);
                        fields += "|abankid:" + bankID;
                    }
                    else
                    {
                        fields += "|abankid:" + u_ID;
                    }
                }
                else if (idtype == 2)
                {
                    fields += "|listid:" + u_ID;
                }
            }

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

            string test = u_BeginTime.ToString("yyyy-MM-dd");


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
            fields += "|begintime:" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss") + "|endtime:" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss");
            if (fields.StartsWith("|"))
            {
                fields = fields.Substring(1, fields.Length - 1);
            }

            DataSet dstemp = new PublicRes().QueryCommRelay8020("2416", fields, iPageStart, iPageMax);
            ds = PublicRes.ToOneDataset(ds, dstemp);
            dstemp = new PublicRes().QueryCommRelay8020("2417", fields, iPageStart, iPageMax);
            ds = PublicRes.ToOneDataset(ds, dstemp);

            return ds;
            //fstrSql_count = "select 10000";//" select count(*) from ( " + strGroup + ") a ";
        }
        //信用卡还款按财付通号查询
         public DataSet GetCreditQueryListForFaid(string QQOrEmail, DateTime begindate, DateTime enddate, int istart, int imax)
         {
             if (istart % imax == 1) istart -= 1;

             string Fuid = PublicRes.ConvertToFuid(QQOrEmail);
//#if DEBUG
//             Fuid = QQOrEmail;
//#endif
             string fields = "Fuid:{0}|begindate:{1}|enddate:{2}";
             fields = string.Format(fields, Fuid, begindate.ToString("yyyy-MM-dd 00:00:00"), enddate.ToString("yyyy-MM-dd 23:59:59"));

             DataSet ds1 = new PublicRes().QueryCommRelay8020("2418", fields, istart, imax);
             DataSet ds2 = new PublicRes().QueryCommRelay8020("2419", fields, istart, imax);
             DataSet ds = PublicRes.ToOneDataset(ds1, ds2);

             //加查上月或者下月的表
             DataSet ds3 = new DataSet();
             fields = "Fuid:{0}|begindate:{1}|enddate:{2}";
             begindate = begindate.Day > 15 ? begindate.AddMonths(1) : begindate.AddMonths(-1);
             if (begindate > enddate)
             {
                 enddate = begindate;
             }
             fields = string.Format(fields, Fuid, begindate.ToString("yyyy-MM-01"), enddate.ToString("yyyy-MM-dd"));
             ds3 = new PublicRes().QueryCommRelay8020("2418", fields, istart, imax);
             ds = PublicRes.ToOneDataset(ds, ds3);
             return ds;
         }
        //信用卡还款根据还款交易号查询
        public DataSet GetCreditQueryList(string Flistid, int istart, int imax)
        {
            if (istart % imax == 1) istart -= 1;
            string fields = "listid:{0}";
            fields = string.Format(fields, Flistid);
            DataSet ds = new PublicRes().QueryCommRelay8020("2420", fields, istart, imax);
            DateTime date = GetPayListTableFromID(Flistid);
            fields = "listid:{0}|date:{1}";
            fields = string.Format(fields, Flistid, date.ToString("yyyy-MM-dd"));
            DataSet ds2 = new PublicRes().QueryCommRelay8020("2421", fields, istart, imax);
            ds = PublicRes.ToOneDataset(ds, ds2);

            //加查上月或者下月的表
            DataSet ds3 = new DataSet();
            fields = "listid:{0}|date:{1}";
            date = date.Day > 15 ? date.AddMonths(1) : date.AddMonths(-1);
            fields = string.Format(fields, Flistid, date.ToString("yyyy-MM-dd"));
            ds3 = new PublicRes().QueryCommRelay8020("2421", fields, istart, imax);
            ds = PublicRes.ToOneDataset(ds, ds3);
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

        public DataSet GetTCBankPAYList(string strID, int iIDType, DateTime dtBegin, DateTime dtEnd, int istr, int imax)
        {
            DataSet ds = new DataSet();
            try
            {
                string f_strID = ""; ;
                f_strID = strID;
                string fstrSql = "";
                if (iIDType == 0)
                {
                    string Fuid = PublicRes.ConvertToFuid(strID);
                    #if DEBUG
                                       Fuid = strID;
                    #endif
                    string fields = "uid:{0}|begintime:{1}|endtime:{2}";
                    fields = string.Format(fields, Fuid, dtBegin.ToString("yyyy-MM-dd"), dtEnd.ToString("yyyy-MM-dd"));
                    DataSet ds1 = new PublicRes().QueryCommRelay8020("2423", fields, istr, imax);
                    DataSet ds2 = new PublicRes().QueryCommRelay8020("2424", fields, istr, imax);

                    ds = PublicRes.ToOneDataset(ds1, ds2);

                    DataSet ds3 = new DataSet();
                    fields = "uid:{0}|begintime:{1}|endtime:{2}";
                    dtBegin = dtBegin.Day > 15 ? dtBegin.AddMonths(1) : dtBegin.AddMonths(-1);
                    if (dtBegin > dtEnd)
                    {
                        dtEnd = dtBegin;
                    }
                    fields = string.Format(fields, Fuid, dtBegin.ToString("yyyy-MM-01"), dtEnd.ToString("yyyy-MM-dd"));
                    ds3 = new PublicRes().QueryCommRelay8020("2423", fields, istr, imax);
                    ds = PublicRes.ToOneDataset(ds, ds3);
                }
                else
                {
                    string currtable = "";
                    string othertable = "";
                    CFT.CSOMS.DAL.Infrastructure.PickQueryClass.GetPayListTableFromID(f_strID, out currtable, out othertable);

                    fstrSql = "Select " + CFT.CSOMS.DAL.Infrastructure.PickQueryClass.GetTcPayListOldFields() + " from c2c_db.t_tcpay_list where flistid = '" + f_strID + "' or flistid = (select frlistid from c2c_db.t_refund_list where flistid ='" + f_strID + "'and flstate <> 3) ";
                    fstrSql += " union all Select " + CFT.CSOMS.DAL.Infrastructure.PickQueryClass.GetTcPayListNewFields() + " from " + currtable + " where flistid = '" + f_strID + "' or flistid = (select frlistid from c2c_db.t_refund_list where flistid ='" + f_strID + "'and flstate <> 3) ";
                    fstrSql += " union all Select " + CFT.CSOMS.DAL.Infrastructure.PickQueryClass.GetTcPayListNewFields() + " from " + othertable + " where flistid = '" + f_strID + "' or flistid = (select frlistid from c2c_db.t_refund_list where flistid ='" + f_strID + "'and flstate <> 3) ";
                    fstrSql += " Order by fpay_front_time DESC";
                    ds = QueryInfo.GetTable(fstrSql, istr, imax);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
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