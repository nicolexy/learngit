using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace CFT.CSOMS.BLL.LifeFeePaymentModule
{
    using CFT.CSOMS.DAL.LifeFeePaymentModule;
    public class LifeFeePaymentService
    {
        //查询缴费账单核心信息记录表
        public DataSet QueryChargeBill(string acc, string transId)
        {
            if (string.IsNullOrEmpty(acc.Trim()))
                throw new ArgumentNullException("acc");
            return new LifeFeePaymentData().QueryChargeBill(acc, transId);
        }

    }
}
