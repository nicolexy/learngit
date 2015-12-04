using CFT.CSOMS.DAL.CheckModoule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.BLL.CheckModoule
{
    public class FinishCheckService
    {
        /// <summary>
        /// 修改用户姓名
        /// </summary>
        /// <param name="fuid"></param>
        /// <param name="userip">客户端IP</param>
        /// <param name="companyName">公司名</param>
        /// <param name="trueName">用户名</param>
        /// <param name="qqId"></param>
        /// <returns></returns>
        public static bool Update_UserName(string fuid, string userip, string companyName, string trueName, string qqId)
        {
            return FinishCheckData.Update_UserName(fuid, userip, companyName, trueName, qqId);
        }

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="fuid"></param>
        /// <param name="fstate"></param>
        /// <param name="qqId"></param>
        /// <returns></returns>
        public static bool Freeze_User(string fuid, string fcurtype, string fstate, string qqId)
        {
            return FinishCheckData.Freeze_User(fuid, fcurtype, fstate, qqId);
        }

        /// <summary>
        /// 修改用户账户类型
        /// </summary>
        /// <param name="fuid"></param>
        /// <param name="fuser_type"></param>
        /// <param name="qqId"></param>
        /// <returns></returns>
        public static bool Update_User_Type(string fuid, string fuser_type, string qqId)
        {
            return FinishCheckData.Update_User_Type(fuid, fuser_type, qqId);
        }
    }
}
