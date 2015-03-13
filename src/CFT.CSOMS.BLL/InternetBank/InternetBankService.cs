using System;
using System.Collections.Generic;
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
    }
}
