using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.Apollo.CommunicationFramework;


namespace CFT.CSOMS.DAL.HandQModule
{
   public class HandQDAL
    {
       public DataSet QueryHandQInfor(string strUID, string strPayListID, string strBeginTime, string strEndTime, string strType, int offset, int limit, out string strOutMsg)
        {
            strOutMsg = "QueryHandQInfor";
            
            string relayDefaultSPId = "20000000";
            string ip = System.Configuration.ConfigurationManager.AppSettings["HandQHBIP"].ToString();
            int port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["HandQHBPort"].ToString());
            string requestString = "uin=" + strUID + "&trans_id=" + strPayListID + "&busi_type=3" + "&start_date=" + strBeginTime + "&end_date="+strEndTime+"&order_type="+strType+"&offset="+offset+"&limit="+limit;
            strOutMsg += requestString;
            return RelayAccessFactory.GetHQDataFromRelay(requestString, "100631", ip, port, false, false, relayDefaultSPId);
 
        }

       public DataSet RequestHandQDetail(string strSendList,int type, int offset, int limit, out string strOutMsg)
        {
            strOutMsg = "RequestHandQDetail";
            if (string.IsNullOrEmpty(strSendList))
            {
                strOutMsg += "红包总单号不能为空";
                return null;
            }
            string relayDefaultSPId = "20000000";
            string ip = System.Configuration.ConfigurationManager.AppSettings["HandQHBIP"].ToString();
            int port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["HandQHBPort"].ToString());
            string requestString = "send_listid=" + strSendList +"&type ="+type + "&offset=" + offset + "&limit=" + limit; 
            strOutMsg += requestString;
            return RelayAccessFactory.GetHBDetailFromRelay(requestString, "101003", ip, port, true, false, relayDefaultSPId);
        }

       /// <summary>
       /// 手Q还款列表查询
       /// </summary>
       /// <param name="openid">用户uin</param>
       /// <param name="start_time">查询开始时间范围 (YYYY-MM-DD hh:mm:ss)</param>
       /// <param name="end_time">查询结束时间范围 (YYYY-MM-DD hh:mm:ss)</param>
       /// <param name="bank_type">银行类型</param>
       /// <param name="card_id">信用卡Xcard(X开头的信用卡密文) 通过接口加密获得</param>
       /// <param name="cilent_ip">客户端IP</param>
       /// <returns></returns>
       public DataSet RefundHandQQuery(string openid, string start_time, string end_time, string bank_type, string card_id, string cilent_ip, int offset, int limit)
       {
           string ip = Apollo.Common.Configuration.AppSettings.Get<string>("RefundHandQIP", "10.128.160.10");
           int port = Apollo.Common.Configuration.AppSettings.Get<int>("RefundHandQPORT", 22000);
           string RefundHandQWatch_word="e401461dc78e4c99819515ef6ecf436e";
           string paramsStr = "";
           paramsStr += "op_type=kf_query&watch_word=" + RefundHandQWatch_word + "&";
           paramsStr += "openid=" + openid + "&";
           paramsStr += "business_type=4&";
           paramsStr += "start_time=" + start_time + "&";
           paramsStr += "end_time=" + end_time + "&";
           paramsStr += "bank_type=" + bank_type + "&";
           paramsStr += "card_id=" + card_id + "&";
           paramsStr += "cilent_ip=" + cilent_ip + "&";
           paramsStr += "offset=" + offset + "&";
           paramsStr += "limit=" + limit ;

           DataSet ds = RelayAccessFactory.GetDSFromRelayMethod1(paramsStr, "100944", ip, port);
           return ds;
       }

       /// <summary>
       /// 手Q还款詳情查询
       /// </summary>
       /// <param name="wx_fetch_no">商户订单号（手还款订单号）</param>
       /// <param name="wx_trade_no">手Q交易单号</param>
       /// <param name="cilent_ip">客户端IP</param>
       /// <returns></returns>
       public DataSet RefundHandQDetailQuery(string wx_fetch_no, string wx_trade_no, string cilent_ip)
       {
           string ip = Apollo.Common.Configuration.AppSettings.Get<string>("RefundHandQIP", "10.128.160.10");
           int port = Apollo.Common.Configuration.AppSettings.Get<int>("RefundHandQPORT", 22000);
           string RefundHandQWatch_word = "e401461dc78e4c99819515ef6ecf436e";
           string paramsStr = "";
           paramsStr += "op_type=kf_query&";
           paramsStr += "business_type=4&";
           paramsStr += "watch_word=" + RefundHandQWatch_word + "&";
           paramsStr += "wx_fetch_no=" + wx_fetch_no + "&";
           paramsStr += "wx_trade_no=" + wx_trade_no + "&";
           paramsStr += "cilent_ip=" + cilent_ip + "&";
           //若查询不到单详情，注意接口方直接返回错误信息：result=26502015&res_info=[26502015]System busy
           DataSet ds = RelayAccessFactory.GetDSFromRelay(paramsStr, "100945", ip, port);
           return ds;
       }

       /// <summary>
       /// 通过明文卡号获取加密X类型
       /// </summary>
       /// <param name="card_id">明文卡号</param>
       /// <returns></returns>
       public string BankCardEncryptStartX(string card_id)
       {
           string ip = Apollo.Common.Configuration.AppSettings.Get<string>("RefundHandQIP", "10.128.160.10");
           int port = Apollo.Common.Configuration.AppSettings.Get<int>("RefundHandQPORT", 22000);

         //  string paramsStr = "request_type=3309&ver=1&head_u=&sp_id=20000000000";
           string paramsStr = "";
           paramsStr += "uin=kf&";
           paramsStr += "creditid_list=" + card_id + "";
           DataSet ds = RelayAccessFactory.GetDSFromRelayAnsNotEncr(paramsStr, "3309", ip, port);
           if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
               return ds.Tables[0].Rows[0]["creditcode_list"].ToString();
           else
               throw new Exception("通过明文卡号获取加密X类型异常");
       }
    }
}
