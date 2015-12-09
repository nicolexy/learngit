using CFT.CSOMS.DAL.TradeModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.BLL.TradeModule
{
    public class SettleService
    {
        private SettleData settleData;
        public SettleService()
        {
             settleData = new SettleData();
        }
        public string getAmount(string qqid, string UinOrUid)
        {
            return settleData.getAmount(qqid, UinOrUid);
        }

        public DataTable QuerySettleRuleList(string Fspid, int offset, int limit)
        {
            return settleData.QuerySettleRuleList(Fspid, offset, limit);
        }

        public DataTable QuerySpControl(string spid)
        {
            return settleData.QuerySpControl(spid);
        }

        public DataTable QueryRelationOrderList(string szListid, string subListid)
        {
            return settleData.QueryRelationOrderList(szListid, subListid);
        }
        public DataTable GetTrustLimitList(string qqid, string Fspid)
        {
            return settleData.GetTrustLimitList(qqid, Fspid);
        }
        public DataTable GetSettleReqInfo(string szListid)
        {
            return settleData.GetSettleReqInfo(szListid);
        }

        public DataTable QueryAdjustList(string szListid, string spno, string spid, string adjustTime)
        {
            return settleData.QueryAdjustList(szListid, spno, spid, adjustTime);
        }
        public DataTable GetAirAdjustList(int iType, string Flistid, string szSpid, string szBeginDate, string szEndDate, int iPageStart, int iPageMax)
        {
            return settleData.GetAirAdjustList(iType, Flistid, szSpid, szBeginDate, szEndDate, iPageStart, iPageMax);
        }
        public DataTable GetSettleReqList(string szListid, string reqid)
        {
            return settleData.GetSettleReqList(szListid, reqid);
        }
        public DataTable QueryTrueLimtList(string spid, string qqid)
        {
            return settleData.QueryTrueLimtList(spid, qqid, "2");
        }
        public DataTable QuerySubOrderList(string mergeListid, string listid, int offset, int limit)
        {
            return settleData.QuerySubOrderList(mergeListid, listid, offset, limit);
        }
        public DataTable GetAirFreeze(string Spid, string QQid, string startDate, string EndDate, string order_type, int offset, int limit)
        {
            return settleData.GetAirFreeze(Spid, QQid, startDate, EndDate, order_type, offset, limit);
        }

        public DataTable GetSettleListAppend(string szListid)
        {
            return settleData.GetSettleListAppend(szListid);
        }

          //分账明细信息查询
        public DataTable GetSettleInfoListDetail(string szListid)
        {
            return settleData.GetSettleInfoListDetail(szListid);
        }

        public DataTable  GetSettleRefundListDetail(string szRefundId, string szListid)
        {
            return settleData.GetSettleRefundListDetail(szRefundId, szListid);
        }

        public DataTable GetSettleRefundList(string Flistid, int iQueryType, int iPageStart, int iPageMax)
        {
            return settleData.GetSettleRefundList(Flistid, iQueryType, iPageStart, iPageMax);
        }
    }
}
