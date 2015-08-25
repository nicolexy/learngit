using CFT.CSOMS.DAL.CreditModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.BLL.CreditModule
{
    public class TencentCreditService
    {
        /// <summary>
        /// 腾讯信用查询
        /// </summary>
        /// <param name="uin">QQ号</param>
        /// <param name="username">操作员</param>
        /// <returns></returns>
        public DataSet TencentCreditQuery(string uin, string username)
        {
            return new CreditData().TencentCreditQuery(uin, username);
        }
    }
}
