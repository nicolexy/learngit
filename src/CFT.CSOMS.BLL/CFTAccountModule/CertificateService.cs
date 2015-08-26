using CFT.CSOMS.DAL.CFTAccount;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.BLL.CFTAccountModule
{
    public class CertificateService
    {
        /// <summary>
        /// 清理日志查询
        /// </summary>
        /// <param name="creid">身份证号</param>
        /// <returns></returns>
        public DataTable GetClearCreidLog(string creid)
        {
            return (new CertificateData()).GetClearCreidLog(creid);
        }
       
        /// <summary>
        /// 记录证件号码清理日志
        /// </summary>
        /// <param name="creid"></param>
        /// <param name="userType"></param>
        /// <param name="Uid"></param>
        public void WriteClearCreidLog(string creid, int userType, string Uid)
        {
            (new CertificateData()).WriteClearCreidLog(creid, userType, Uid);
        }

        /// <summary>
        /// 清理证件号码
        /// </summary>
        /// <param name="creid"></param>
        /// <param name="type">用户类型</param>
        /// <returns></returns>
        public bool ClearCreidInfo(string creid, int type, string opera)
        {
            bool ret = new CertificateData().ClearCreidInfo(creid, type);
            if (ret)
            {
                WriteClearCreidLog(creid, type, opera);
            }
            return ret;
        }

        /// <summary>
        /// 获取清理次数
        /// </summary>
        /// <param name="creid"></param>
        /// <returns></returns>
        public int GetClearCreidCount(string creid, int userType)
        {
            return (new CertificateData()).GetClearCreidCount(creid, userType);
        }

    }
}
