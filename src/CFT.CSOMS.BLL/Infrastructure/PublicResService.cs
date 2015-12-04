using CFT.CSOMS.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.BLL.Infrastructure
{
    public class PublicResService
    {
        /// <summary>
        /// 根据QQ帐号获取，facc_set
        /// </summary>
        /// <param name="QQID"></param>
        /// <returns></returns>
        public static string GetSetIDByQQID(string QQID)
        {
            return PublicRes.GetSetIDByQQID(QQID);
        }
    }
}
