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

        /// <summary>
        /// 微信CKV值获取和同步
        /// </summary>
        /// <param name="flag">操作类型 1:查询 2:同步</param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public Dictionary<string, string> WXOperateCKVCGI(int flag, string uid)
        {
            return new AuthenData().WXOperateCKVCGI(flag, uid);
        }
    }
}
