using CFT.CSOMS.DAL.TradeModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
namespace CFT.CSOMS.BLL.TradeModule
{
    public class PickService
    {
        public DataSet GetPickList(int idtype, string u_ID, DateTime u_BeginTime, DateTime u_EndTime, int fstate, float fnum, string banktype, string sorttype, string cashtype,
            int iPageStart, int iPageMax)
        {
            return new PickData().GetPickList(u_ID, idtype, u_BeginTime, u_EndTime, fstate, fnum, banktype, sorttype, cashtype, iPageStart, iPageMax);
        }

        public DataTable GetFetchListIntercept(string fetchListid)
        {
            if (string.IsNullOrEmpty(fetchListid))
            {
                throw new ArgumentNullException("fetchListid");
            }
            return (new PickData()).GetFetchListIntercept(fetchListid);
        }

        public bool AddFetchListIntercept(string fetchListid, string opera)
        {
            if (string.IsNullOrEmpty(fetchListid))
            {
                throw new ArgumentNullException("fetchListid");
            }

            var dt = new PickService().GetFetchListIntercept(fetchListid);
            if (dt != null && dt.Rows.Count > 0)
                throw new Exception("该体现单号已拦截！");

            return (new PickData()).AddFetchListIntercept(fetchListid, opera);
        }

        public DataTable GetPickListDetail(string listid)
        {
            //DataSet ds = new PickData().QueryPickByListid(listid, null, null, 0, 0, "0000", "0000");

            var dt = new PickData().QueryPickByListidNew(listid);

            return (dt == null || dt.Rows.Count == 0) ? null : dt;
        }

        public DataSet GetCreditQueryListForFaid(string QQOrEmail, DateTime begindate, DateTime enddate, int istart, int imax)
        {
            return new PickData().GetCreditQueryListForFaid(QQOrEmail, begindate, enddate, istart, imax);
        }

        public DataSet GetCreditQueryList(string Flistid, int istart, int imax)
        {
            DataSet ds = new PickData().GetCreditQueryList(Flistid, istart, imax);
            DataTable dt = (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) ? ds.Tables[0] : null;
            if (dt != null)
            {
                dt.Columns.Add("creditcard_id", typeof(string));
                foreach (DataRow dr in dt.Rows)
                {
                    string Fabankid = dr["Fabankid"].ToString();
                    try
                    {
                        dr["creditcard_id"] = Fabankid.Substring(Fabankid.Length - 4, 4);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return ds;
        }

        public DataSet GetQuerySettlementTodayList(string Fspid)
        {
            return new PickData().GetQuerySettlementTodayList(Fspid);
        }

        public DataSet GetTCBankPAYList(string strID, int iIDType, DateTime dtBegin, DateTime dtEnd, int istr, int imax, string fuid = "")
        {
            //DataSet ds = new PickData().GetTCBankPAYList(strID, iIDType, dtBegin, dtEnd, istr, imax);
            DataSet ds = new PickData().GetPickList(strID, iIDType, dtBegin, dtEnd, 0, 0, "0000", "0", "0000", istr, imax, fuid);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("Fstate_str", typeof(string));
                ds.Tables[0].Columns.Add("Ftype_str", typeof(string));
                ds.Tables[0].Columns.Add("Fsubject_str", typeof(string));
                ds.Tables[0].Columns.Add("Fnum_str", typeof(string));
                ds.Tables[0].Columns.Add("Fsign_str", typeof(string));
                ds.Tables[0].Columns.Add("Fbank_type_str", typeof(string));
                ds.Tables[0].Columns.Add("Fabank_type_str", typeof(string));
                ds.Tables[0].Columns.Add("Fcurtype_str", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["Fstate_str"] = TransferMeaning.Transfer.convertCurrentState(dr["Fstate"].ToString());
                    dr["Ftype_str"] = TransferMeaning.Transfer.convertTradeType(dr["Ftype"].ToString());
                    dr["Fsubject_str"] = TransferMeaning.Transfer.convertTCfSubject(dr["Fsubject"].ToString());
                    dr["Fnum_str"] = MoneyTransfer.FenToYuan(dr["Fnum"].ToString());
                    dr["Fsign_str"] = TransferMeaning.Transfer.convertTradeSign(dr["Fsign"].ToString());
                    dr["Fbank_type_str"] = TransferMeaning.Transfer.convertbankType(dr["Fbank_type"].ToString());
                    dr["Fabank_type_str"] = TransferMeaning.Transfer.convertbankType(dr["Fabank_type"].ToString());
                    dr["Fcurtype_str"] = TransferMeaning.Transfer.convertMoney_type(dr["Fcurtype"].ToString());
                }
            }

            return ds;
        }

        public string TdeToID(string tdeid)
        {
            return new PickData().TdeToID(tdeid);
        }

        /// <summary>
        /// 封装提现查询接口
        /// </summary>
        public DataSet GetWithdrawDepositRecord(int qry_type, string account, DateTime stime, DateTime etime,
            int offset, int limit)
        {

            DataSet ds = new PickData().GetPickList(account, qry_type, stime, etime, 0, 0, "0000", "0", "0000", offset, limit);
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (!ds.Tables[0].Columns.Contains("Fstandby3"))
                    {
                        ds.Tables[0].Columns.Add("Fstandby3", typeof(String)); //微信提现预计到账时间
                    }
                    ds.Tables[0].Columns.Add("Fabank_type_str", typeof(String)); //提现银行
                    ds.Tables[0].Columns.Add("Fbank_type_str", typeof(String)); //出款银行
                    ds.Tables[0].Columns.Add("Fsign_str", typeof(String)); //退票
                    ds.Tables[0].Columns.Add("Fcharge_str", typeof(String)); //手续费
                    ds.Tables[0].Columns.Add("Fnum_str", typeof(String)); //提现金额

                    MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Fcharge", "Fcharge_str");
                    MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Fnum", "Fnum_str");

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (GetString(dr["Fsign"]) == "7")
                        {
                            dr["Fsign_str"] = "是";
                        }
                        else
                        {
                            dr["Fsign_str"] = "否";
                        }
                        if (dr["Fproduct"].ToString().Trim() == "7" && (dr["Fbusiness_type"].ToString() == "2" || dr["Fbusiness_type"].ToString() == "3"))
                        {
                            dr["Fstandby3"] = GetString(dr["Fstandby3"]);
                        }
                        else
                        {
                            dr["Fstandby3"] = "";
                        }
                        dr["Fabank_type_str"] = TENCENT.OSS.C2C.Finance.BankLib.BankIO.QueryBankName(dr["Fabank_type"].ToString());
                        dr["Fbank_type_str"] = TENCENT.OSS.C2C.Finance.BankLib.BankIO.QueryBankName(dr["Fbank_type"].ToString());

                    }

                    ds.Tables[0].Columns.Add("FStateName", typeof(String));
                    GetColumnValueFromDic(ds.Tables[0], "Fsign", "FStateName", "TCLIST_SIGN");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("提现查询接口 GetWithdrawDepositRecord ：" + ex.ToString());
            }
            return ds;

        }


        public static string GetString(object aValue)
        {
            try
            {
                if (aValue != null)
                {
                    return aValue.ToString().Replace("\\", "\\\\").Replace("'", "\\'");
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }


        //在DataTable中转换值，标识值到标识名称。
        public static void GetColumnValueFromDic(DataTable dt, string SourceColumn, string DestColumn, string sType)
        {
            try
            {
                Hashtable ht = GetAllValueByType(sType);
                if (ht == null || ht.Count == 0) return;

                foreach (DataRow dr in dt.Rows)
                {
                    string tmp = GetString(dr[SourceColumn]);
                    if (tmp != "")
                    {
                        dr.BeginEdit();
                        if (ht.ContainsKey(tmp))
                        {
                            dr[DestColumn] = ht[tmp].ToString();
                        }
                        else
                        {
                            dr[DestColumn] = "未知类型(" + tmp + ")";
                        }
                        dr.EndEdit();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public static Hashtable GetAllValueByType(string sType)
        {
            Hashtable ht = new Hashtable();

            try
            {
                if (HttpContext.Current.Application[sType] == null)
                    queryDic(sType);

                ht = (Hashtable)HttpContext.Current.Application[sType];

                return ht;
            }
            catch
            {
                return null;
            }
        }


        public static void queryDic(string type)
        {
            string Msg;
            DataSet ds = QueryDicInfoByType(type, out Msg);

            if (ds == null)
            {
                throw new Exception(Msg);
            }

            Hashtable myht = new Hashtable();
            try
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    myht.Add(dr["Fvalue"].ToString(), dr["Fmemo"].ToString());
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            //绑定数据字典
            HttpContext.Current.Application[type] = myht;
        }
        public static DataSet QueryDicInfoByType(string type, out string Msg)
        {
            Msg = "";
            try
            {
                //先查询出总笔数
                string icesql = "type=" + type;
                string count = CommQuery.GetOneResultFromICE(icesql, CommQuery.QUERY_DIC_COUNT, "acount", out Msg);
                if (count == null || count == "" || count == "0")
                {
                    return null;
                }
                int allCount = Convert.ToInt32(count);
                if (allCount <= 0)
                {
                    return null;
                }


                DataTable dt_all = new DataTable();
                dt_all.Columns.Add("Fno", System.Type.GetType("System.String"));
                dt_all.Columns.Add("FType", System.Type.GetType("System.String"));
                dt_all.Columns.Add("Fvalue", System.Type.GetType("System.String"));
                dt_all.Columns.Add("Fmemo", System.Type.GetType("System.String"));
                dt_all.Columns.Add("Fsymbol", System.Type.GetType("System.String"));

                string strSqlTmp = "type=" + type;
                int limitStart = 0;
                int onceCount = 20;//一次返回笔数

                while (allCount > limitStart)
                {
                    string strSqlLimit = "&strlimit=limit " + limitStart + "," + onceCount;
                    string strSql = strSqlTmp + strSqlLimit;
                    DataTable dt_one = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_DIC, out Msg);
                    if (dt_one != null && dt_one.Rows.Count > 0)
                    {
                        foreach (DataRow dr2 in dt_one.Rows)
                        {
                            string fno = dr2["Fno"].ToString();
                            string FType = dr2["FType"].ToString();
                            string Fvalue = dr2["Fvalue"].ToString();
                            string Fmemo = dr2["Fmemo"].ToString();
                            string symbol = dr2["Fsymbol"].ToString();

                            DataRow drNew = dt_all.NewRow();
                            drNew["Fno"] = fno;
                            drNew["FType"] = FType;
                            drNew["Fvalue"] = Fvalue;
                            drNew["Fmemo"] = Fmemo;
                            drNew["Fsymbol"] = symbol;
                            dt_all.Rows.Add(drNew);
                        }

                    }

                    limitStart = limitStart + onceCount;
                }

                int num = dt_all.Rows.Count;
                DataSet ds = new DataSet();
                ds.Tables.Add(dt_all);
                return ds;
            }
            catch
            {
                return null;
            }
        }
    }
}
