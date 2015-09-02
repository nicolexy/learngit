using CFT.CSOMS.DAL.InternetBank;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.BLL.InternetBank
{
    public  class InternetBankService
    {
        public bool AddRefundInfo(string FOrderId, int FRefund_type, string FSam_no, string FRecycle_user, string FSubmit_user, string FRefund_amount, string memo)
        {
            if (string.IsNullOrEmpty(FOrderId)) 
            {
                throw new ArgumentNullException("FOrderId");
            }
            return  new CFT.CSOMS.DAL.InternetBank.InternetBankData().AddRefundInfo(FOrderId, FRefund_type, FSam_no, FRecycle_user, FSubmit_user, FRefund_amount,  memo);
        }

        /// <summary>
        /// 添加 退款商户名单 
        /// </summary>
        /// <param name="Frefund_id">商户号码</param>
        /// <param name="Frefund_name">商户名称</param>
        /// <param name="Fcreate_by">创建人</param>
        /// <param name="Fmodify_by">修改人</param>
        /// <returns></returns>
        public int AddRefundList(int Frefund_id, string Frefund_name, string Fcreate_by)
        {
            return new InternetBankData().AddRefundList(Frefund_id, Frefund_name, Fcreate_by);
        }

        /// <summary>
        /// 删除  退款商户名单 
        /// </summary>
        /// <param name="Fid">自增ID</param>
        /// <returns></returns>
        public bool DelRefundByFid(int Fid)
        {
            return new InternetBankData().DelRefundByFid(Fid);
        }

        /// <summary>
        /// 修改 退款商户名单 
        /// </summary>
        /// <param name="Frefund_id">商户号码</param>
        /// <param name="Frefund_name">商户名称</param>
        /// <param name="Fmodify_by">修改人</param>
        /// <param name="Fid">自增ID</param>
        /// <returns></returns>
        public int EditRefundByFid(int Frefund_id, int Old_Frefund_id,string Frefund_name, string Fmodify_by, int Fid)
        {
            return new InternetBankData().EditRefundByFid(Frefund_id,Old_Frefund_id, Frefund_name, Fmodify_by, Fid);
        }

        /// <summary>
        /// 查询 退款商户名单 
        /// </summary>
        /// <param name="Frefund_id">商户号码</param>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <returns></returns>
        public DataSet GetRefundByFrefundId(int Frefund_id, string sTime, string eTime, int start, int max)
        {
            return new InternetBankData().GetRefundByFrefundId(Frefund_id, sTime, eTime, start, max);
        }

        public int GetRefundCount(int Frefund_id, string sTime, string eTime)
        {
            return new InternetBankData().GetRefundCount(Frefund_id, sTime, eTime);
        }
    }
}
