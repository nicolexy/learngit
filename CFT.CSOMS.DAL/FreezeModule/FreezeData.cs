using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CFT.CSOMS.COMMLIB;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.DAL.CFTAccount;
using System.Data;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.Apollo.Logging;

namespace CFT.CSOMS.DAL.FreezeModule
{
    public class FreezeData
    {
        /// <summary>
        /// 发送微信消息
        /// </summary>
        /// <param name="reqsource"></param>
        /// <param name="accid"></param>
        /// <param name="templateid"></param>
        /// <param name="cont1"></param>
        /// <param name="cont2"></param>
        /// <param name="cont3"></param>
        /// <param name="msg_type"></param>
        public void SendWechatMsg(string reqsource, string accid, string templateid, string cont1, string cont2, string cont3, string msg_type)
        {
            string msg = "";
            string req = "";
            string cgi = "http://api.hera.cm.com/msgassistant/sendtemplate/sendMsg";
            try
            {
                cgi = System.Configuration.ConfigurationManager.AppSettings["SendWechatMsgCgi"].ToString();
            }
            catch
            {

            }
            PostArgs.BaseRequest args = new PostArgs.BaseRequest();
            args.reqsource = reqsource;  //冻结解冻补填请求类型
            args.accid = accid;
            args.topcolor = "";
            //冻结，补充资料都是这个链接
            args.navtourl = "http://kf.qq.com/touch/weixin/payment_unfreeze_app.html?ADTAG=veda.weixinpay.mp&tj_src=mp";

            args.templateid = templateid; //冻结，解冻，补填模板ID

            PostArgs.ValueClass first = new PostArgs.ValueClass();
            PostArgs.ValueClass keynote1 = new PostArgs.ValueClass();
            PostArgs.ValueClass remark = new PostArgs.ValueClass();
            first.value = cont1;
            keynote1.value = cont2;
            remark.value = cont3;

            PostArgs.TemplateClass templatedata = new PostArgs.TemplateClass();
            templatedata.first = first;
            templatedata.keynote1 = keynote1;
            templatedata.remark = remark;

            args.templatedata = templatedata;

            PostArgs.MsgClass msgdata = new PostArgs.MsgClass();
            msgdata.frechannel = "1";
            msgdata.strategyno = "1";
            msgdata.seclev = "1";
            msgdata.errcode = "1";

            PostArgs.ReserveClass reserve = new PostArgs.ReserveClass();
            reserve.msgdata = msgdata;
            reserve.msgtype = msg_type; //冻结，解冻，补填

            string sec = accid + templateid + "AAAAB3NzaC1yc2EAAAADAQABAAABAQCW";
            string md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sec, "md5").ToLower();

            args.reserve = reserve;
            args.sign = md5;

            req = CommUtil.ObjToJson<PostArgs.BaseRequest>(args);

           // LogHelper.LogInfo("SendWechatMsg:" + req);
            string ret = CommUtil.SendHttpPost(cgi, req, true, true, out msg);
            if (msg != "")
            {
                LogHelper.LogInfo("SendWechatMsg:" + msg);
            }
            //LogHelper.LogInfo("SendWechatMsg:" + ret);

            /*
            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("UTF-8");

            System.Net.HttpWebRequest webrequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(cgi);
            webrequest.ContentType = "application/x-www-form-urlencoded";
            webrequest.Method = "POST";

            var data = Encoding.UTF8.GetBytes(req);
            var parameter = webrequest.GetRequestStream();
            parameter.Write(data,0,data.Length);

            parameter.Close();

            System.Net.HttpWebResponse webresponse = (System.Net.HttpWebResponse)webrequest.GetResponse();
            Stream stream = webresponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(stream, encoding);
            string answer = streamReader.ReadToEnd();
            webresponse.Close();
            streamReader.Close();
            */



        }

        public bool SyncCreid(string uin, string oldCredId, string newCredId, int creType, string optUser, string ip)
        {
            bool ret = false;
            string fuid = AccountData.ConvertToFuid(uin);
            string strSql = "uid=" + fuid;
            //string name = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Ftruename", out errMsg);

            StringBuilder token = new StringBuilder(fuid);
            token.Append("|");
            token.Append(uin);
            token.Append("|");
            //token.Append(name); 
            token.Append("|");
            token.Append(newCredId);
            token.Append("|");
            token.Append("5416d7cd6ef195a0f7622a9c56b55e84");

            string md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(token.ToString(), "md5").ToLower();

            string serviceName = "ui_modify_usernameid_service";
            StringBuilder reqBuilder = new StringBuilder()
            .AppendFormat("uin={0}", uin)
            .AppendFormat("&cre_type={0}", creType)
            .AppendFormat("&cre_id={0}", newCredId)
                //.AppendFormat("&true_name={0}", name)
            .AppendFormat("&vali_type=100")
            .AppendFormat("&token={0}", md5);

            string sReply;
            short iResult;
            string sMsg;
            bool isRet = PublicRes.CommQuery(serviceName, reqBuilder.ToString(), true, out sReply, out iResult, out sMsg);

            if (isRet)
            {
                if (iResult == 0)
                {
                    LogHelper.LogInfo(serviceName + " SUCC:" + sReply);
                    if (sReply.IndexOf("result=0") > -1)
                    {
                        ret = true;
                        writeLog(uin, optUser, ip, oldCredId, newCredId, "证件号码", "changeUserInfo", "");
                    }
                }
                else
                {
                    LogHelper.LogInfo(serviceName + " ERROR:" + sMsg);
                }
            }
            else
            {
                LogHelper.LogError(serviceName + " ERROR:" + sMsg);
            }
            return ret;
        }

        private void writeLog(string fqqid, string strUserID, string ip, string fold, string fnew, string factionname, string factionType, string fmemo)
        {
            string logStr = "Insert c2c_fmdb.t_log_kf (Fqqid,FoperTime,Foperuser,Fip,Fold,Fnew,Factionname,FactionType,Fmemo) Values (" +
                "'" + fqqid + "'," + //c帐号
                " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ," +           //取数据库时间
                "'" + strUserID + "'," +     //操作人  
                "'" + ip + "'," +     //ip
                "'" + fold + "'," + //原资料
                "'" + fnew + "'," +   //新资料
                "'" + factionname + "'," + // 日志名字
                "'" + factionType + "'," + // 日志名字
                "'" + fmemo + "')";

            using (var da = MySQLAccessFactory.GetMySQLAccess("ht_DB"))
            {
                da.OpenConn();
                da.ExecSqlNum(logStr);
            }
        }
        public string GetCashOutFreezeListId(string Uid)
        {
            if (string.IsNullOrEmpty(Uid))
            {
                throw new ArgumentNullException("必填参数：内部ID为空！");
            }

            var listId = string.Empty;
            var dbNum = Uid.Substring(Uid.Length - 2, 2);
            var tableNum = Uid.Substring(Uid.Length - 3, 1);
            var sql = string.Format(@"SELECT Flistid FROM c2c_db_{0}.t_tcpay_list_{1} WHERE Fuid='{2}' AND Fstate=1 LIMIT 1", dbNum, tableNum, Uid);
            using (var da = MySQLAccessFactory.GetMySQLAccess("CashOutFreeze"))
            {
                da.OpenConn();
                var dt = da.GetTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    listId = dt.Rows[0][0].ToString();
                }
            }
            return listId;
        }
    }
}
