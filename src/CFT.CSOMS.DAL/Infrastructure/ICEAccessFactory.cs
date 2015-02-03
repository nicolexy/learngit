using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TENCENT.OSS.C2C.Finance.DataAccess;
using System.Configuration;
using CFT.Apollo.CommunicationFramework;
using System.Collections;

namespace CFT.CSOMS.DAL.Infrastructure
{
    public class ICEAccessFactory
    {
        public static ICEAccess GetICEAccess(string iceSourceName)
        {

            Hashtable ht = GetICEAccessInfo(iceSourceName);
            return new ICEAccess(ht["ServerIp"].ToString(), int.Parse(ht["ServerPort"].ToString()));
        }

        public static Hashtable GetICEAccessInfo(string iceSourceName)
        {
            if (string.IsNullOrEmpty(iceSourceName))
                throw new ArgumentNullException("iceSourceName");

            var connectionString = ConfigurationManager.AppSettings[iceSourceName];

            if (string.IsNullOrEmpty(connectionString))
                throw new Exception(string.Format("配置文件中找不到Key为\"{0}\"的配置项", iceSourceName));

            var connectionStringArray = connectionString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            if (connectionStringArray.Length != 4)
                throw new Exception(string.Format("配置文件中Key为\"{0}\"的配置项格式不对", iceSourceName));

            int icePort;

            if (!int.TryParse(connectionStringArray[1], out icePort))
                throw new Exception(string.Format("配置文件中Key为\"{0}\"的配置项端口格式不对", iceSourceName));

            Hashtable ht = new Hashtable();
            ht.Add("ServerIp", connectionStringArray[0]);
            ht.Add("ServerPort", icePort);
            ht.Add("UserName", connectionStringArray[2]);
            ht.Add("Password", connectionStringArray[3]);
            return ht;
        }


        public static bool ICEMiddleInvoke(string serviceName, string inmsg, bool secret, out string strReplyInfo, out short iResult, out string Msg)
        {
            strReplyInfo = "";
            iResult = 9999;
            Msg = "";

            string CFTAcc = ConfigurationManager.AppSettings["CFTAccount"].Trim();
            Hashtable ht = GetICEAccessInfo("ICEConnectionString");

            TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ht["ServerIp"].ToString(), serviceName, inmsg, "KF");

            try
            {
                using (var conn = new ICECommunicationConnection(
               new ICEConnectionParameter()
               {
                   ServerIp = ht["ServerIp"].ToString(),
                   ServerPort = int.Parse(ht["ServerPort"].ToString()),
                   UserName = ht["UserName"].ToString(),
                   Password = ht["Password"].ToString()
               }))
                {
                    var communication = new ICECommunication();
                    var request = new ICERequest();
                    if (secret)
                        request = new ICERequest()
                       {
                           ServiceName = serviceName,
                           RequestMsg = "sp_id=" + CFTAcc + "&request_text=" + MiddleRequestString.EncryptRequestText(inmsg)//加密
                       };
                    else
                        request = new ICERequest()
                          {
                              ServiceName = serviceName,
                              RequestMsg = "sp_id=" + CFTAcc + "&" + inmsg//不加密
                          };
                    var response = communication
                        .Request<ICERequest, ICEResponse>(conn, request);

                    strReplyInfo = response.Msg; 
                    iResult = (short)response.IResult;
                    Msg = response.StrReplyInfo;
                    TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ht["ServerIp"].ToString(), serviceName, "返回包如下1:" + "[flag=]" + response.IResult + "[msg=]" + response.StrReplyInfo + "[replyinfo=]" + response.Msg, "KF");
                }

                return true;

            }
            catch (Exception err)
            {
                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ht["ServerIp"].ToString(), serviceName, err.Message, "KF");

                iResult = 9999;
                Msg = "调用ICE服务前失败" + err.Message;
                return false;
            }

        }
    }
}
