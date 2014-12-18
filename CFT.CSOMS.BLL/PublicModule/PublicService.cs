using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace CFT.CSOMS.BLL.PublicService
{
    using CFT.CSOMS.DAL.Infrastructure;
    public class PublicService
    {
        ///外币分转化成元
        public string FenToYuan(string strfen, string currency_type)
        {
            return MoneyTransfer.FenToYuan(strfen, currency_type);
        }
    }
}
