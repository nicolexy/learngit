using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CFT.CSOMS.DAL.HandQModule;

namespace CFT.CSOMS.BLL.HandQModule
{
   public class HandQService
    {
        public DataSet QueryHandQInfor(string strUID, string strPayListID, string strBeginTime, string strEndTime,string strType,int offset, int limit,out string strOutMsg)
        {

            return new HandQDAL().QueryHandQInfor(strUID, strPayListID, strBeginTime, strEndTime, strType, offset, limit, out strOutMsg);
        }

        public DataSet RequestHandQDetail(string strSendList,int type, int offset, int limit, out string strOutMsg)
        {
            return new HandQDAL().RequestHandQDetail(strSendList, type,offset, limit, out strOutMsg);
        }
    }
}
