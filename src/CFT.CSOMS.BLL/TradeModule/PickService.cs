using CFT.CSOMS.DAL.TradeModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.BLL.TradeModule
{
    public class PickService
    {
        public DataSet GetPickList(string u_ID, DateTime u_BeginTime, DateTime u_EndTime, int fstate, float fnum, string banktype, int idtype, string sorttype, string cashtype,
            int iPageStart, int iPageMax)
        {
            return new PickData().GetPickList(u_ID, u_BeginTime, u_EndTime, fstate, fnum, banktype, idtype, sorttype, cashtype, iPageStart, iPageMax);
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

        public DataSet GetPickListDetail(string listid, DateTime u_BeginTime, DateTime u_EndTime)
        {
            return new PickData().GetPickListDetail(listid, u_BeginTime, u_EndTime);
        }

        public DataSet GetCreditQueryListForFaid(string QQOrEmail, DateTime begindate, DateTime enddate, int istart, int imax)
        {
            return new PickData().GetCreditQueryListForFaid(QQOrEmail, begindate, enddate, istart, imax);
        }

        public DataSet GetCreditQueryList(string Flistid, int istart, int imax)
        {
            return new PickData().GetCreditQueryList(Flistid, istart, imax);
        }

        public DataSet GetQuerySettlementTodayList(string Fspid)
        {
            return new PickData().GetQuerySettlementTodayList(Fspid);
        }

        public DataSet GetTCBankPAYList(string strID, int iIDType, DateTime dtBegin, DateTime dtEnd, int istr, int imax)
        {
            DataSet ds = new PickData().GetTCBankPAYList(strID, iIDType, dtBegin, dtEnd, istr, imax);

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
    }
}
