using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CFT.CSOMS.DAL.WechatPay;

namespace CFT.CSOMS.BLL.WechatPay
{
    public class AuthenService
    {
        public DataSet QueryWechatRealNameAuthen(string uin, string serialno, string clientIp) 
        {
            return new AuthenData().QueryWechatRealNameAuthen(uin,serialno,clientIp);
        }
    }
}
