using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CFT.Apollo.Common.Cryptography;
using CFT.Apollo.Logging;
using CFT.CSOMS.COMMLIB;
using CFT.CSOMS.DAL.Infrastructure;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace CFT.CSOMS.DAL.WechatPay
{
    public class TradePayData
    {
        public DataTable QueryWxTrans(string prime_trans_id) 
        {
            if (string.IsNullOrEmpty(prime_trans_id))
            {
                throw new Exception("prime_trans_id不能为空！");
            }

            string msg;
            
            string serviceName = "wxt_pay_qryprime_service";
            string req = "prime_trans_id=" + prime_trans_id;
            DataSet ds = CommQuery.GetOneTableFromICE(req, "", serviceName, true, out msg);
            if (msg != "")
            {
                LogHelper.LogError(msg);
                throw new Exception(msg); 
            }

            if (ds != null && ds.Tables.Count > 0) 
            {
                return ds.Tables[0];
            }

            return null;
        }

        /// <summary>
        /// 微信红包查询
        /// 
        /// 包括接受红包、发送红包、红包详情查询
        /// 其中sign为所有 非空 参数按ASII（区分大小写）组合+key，然后MD5后大写
        /// </summary>
        /// <param name="paramName">接口参数名（接受红包send_openid、发送红包rec_openid、红包详情send_listid）</param>
        /// <param name="sendData">paramName参数数据</param>
        /// <param name="flag">发红包:1  接红包：2  红包详情：3</param>
        /// <param name="client_ip">客户端ip</param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public DataSet QueryWebchatHB(string paramName, string sendData, int flag, string client_ip, int offset, int limit)
        {

            string op_source = "cft_kf_sys";
           // string key = "2WLTM0CVO9NBG5EQJFPIAH";
            string ipStr = "";
            Random ran = new Random();
            int num = ran.Next(0,2);
            if (num == 0)
                ipStr = "WebchatHBIPFir";
            else if(num == 1)
                ipStr = "WebchatHBIPSec";

            string key = System.Configuration.ConfigurationManager.AppSettings["WebchatHBKey"].ToString();
            string ip = System.Configuration.ConfigurationManager.AppSettings[ipStr].ToString();
            int port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["WebchatHBPORT"].ToString());

            Dictionary<string, string> paramsDic = new Dictionary<string, string>();
            //   paramsDic.Add("MSG_NO", "1003361" + DateTime.Now.Ticks.ToString());//特殊的大写拎出来
            //  paramsDic.Add("head_u", "");
            paramsDic.Add("sp_id", "2000000000");
            paramsDic.Add("ver", "1");
            paramsDic.Add("request_type", "100336");
            paramsDic.Add("client_ip", client_ip);
            paramsDic.Add("op_source", op_source);
            paramsDic.Add("offset", offset.ToString());
            paramsDic.Add("limit", limit.ToString());
            paramsDic.Add("flag", flag.ToString());
            paramsDic.Add(paramName, sendData);
            //paramsDic.Add("sign", "");
            Dictionary<string, string> paramsAsc = CommUtil.DictionarySort(paramsDic);
            string paramsStr = "MSG_NO=" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + CommUtil.StaticNoManage();
            foreach (var d in paramsAsc)
            {
                paramsStr += "&" + d.Key + "=" + d.Value;
            }
            string signStr = paramsStr + "&key=" + op_source + key;
            string sign = MD5Helper.Encrypt(signStr).ToUpper();
            //10.208.148.243 28000
            DataSet dsSend = RelayAccessFactory.GetDSFromRelayFromXML(paramsStr + "&head_u=" + "&sgin=" + sign, ip, port, "utf-8");
            return dsSend;
        }
    }
}
