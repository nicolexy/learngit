using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CFT.CSOMS.DAL.Infrastructure;


namespace CFT.CSOMS.DAL.HandQModule
{
   public class HandQDAL
    {
       public DataSet QueryHandQInfor(string strUID, string strPayListID, string strBeginTime, string strEndTime, string strType, int offset, int limit, out string strOutMsg)
        {
            strOutMsg = "QueryHandQInfor";
            if (string.IsNullOrEmpty(strPayListID))
            {
                if (string.IsNullOrEmpty(strBeginTime) || string.IsNullOrEmpty(strEndTime))
                {
                    strOutMsg += "订单为空时，必需输入时间";
                    return null;
                }
            }

            //RelayAccessFactory.GetDSFromRelay(
            string ip = System.Configuration.ConfigurationManager.AppSettings["HandQHBIP"].ToString();
            int port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["HandQHBPort"].ToString());
            string requestString = "uin=" + strUID + "&trans_id=" + strPayListID + "&busi_type=3" + "&start_date=" + strBeginTime + "&end_date="+strEndTime+"&order_type="+strType+"&offset="+offset+"&limit="+limit;
            strOutMsg += requestString;
            return RelayAccessFactory.GetHQDataFromRelay(requestString, "100631", ip, port);
 
        }

        public DataSet RequestHandQDetail(string strSendList,out string strOutMsg)
        {
            strOutMsg = "RequestHandQDetail";
            if (string.IsNullOrEmpty(strSendList))
            {
                strOutMsg += "红包总单号不能为空";
                return null;
            }
            string ip = System.Configuration.ConfigurationManager.AppSettings["HandQHBIP"].ToString();
            int port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["HandQHBPort"].ToString());
            string requestString = "send_listid=" + strSendList;
            strOutMsg += requestString;
            return RelayAccessFactory.GetHBDetailFromRelay(requestString, "100602", ip, port, true);
        }
    }
}
