using CFT.CSOMS.DAL.TradeModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.BLL.TradeModule
{
   public   class PickService
    {
        public   DataSet GetPickList(string u_ID, DateTime u_BeginTime, DateTime u_EndTime, int fstate, float fnum, string banktype, int idtype, string sorttype, string cashtype,
            int iPageStart, int iPageMax)
        {
            return new PickData().GetPickList(u_ID, u_BeginTime, u_EndTime, fstate, fnum, banktype, idtype, sorttype, cashtype, iPageStart, iPageMax);
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
            return new PickData().GetTCBankPAYList(strID, iIDType, dtBegin, dtEnd, istr, imax);
        }
        public string TdeToID(string tdeid) 
        {
            return new PickData().TdeToID(tdeid);
        }
    }
}
