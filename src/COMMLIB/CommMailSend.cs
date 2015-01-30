using System;
using System.Configuration;
using System.Net.Mail;
using TENCENT.OSS.CFT.KF.Common;
using System.Text.RegularExpressions;

namespace TENCENT.OSS.C2C.Finance.Common.CommLib
{
	/// <summary>
    /// CommMailSend 的摘要说明。
	/// </summary>
	public class CommMailSend
	{
        public CommMailSend()
		{
			
		}

        public static void SendInternalMail(string mailToStr, string mailccStr, string mailSubject, string mailBody)
        {
            SendInternalMail(mailToStr, mailccStr, mailSubject, mailBody, false, null);
        }

        public static void SendInternalMail(string mailToStr, string mailccStr, string mailSubject, string mailBody, bool isBodyHtml) 
        {
            SendInternalMail(mailToStr, mailccStr, mailSubject, mailBody, isBodyHtml, null);
        }

        public static void SendInternalMail(string mailToStr, string mailccStr, string mailSubject, string mailBody, bool isBodyHtml, string[] fileAttachment)
		{
			try
			{
                string from = ConfigurationManager.AppSettings["EmailAddress"].ToString();
                string passwd = ConfigurationManager.AppSettings["EmailPassword"].ToString();
                string smtpServer = ConfigurationManager.AppSettings["SMTPServer"].ToString();
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPServerPort"].ToString());

                var mail = new MailMessage();
                if (from.IndexOf("@") > -1)
                {
                    from = from.Substring(0, from.IndexOf("@"));
                }
                mail.From = new MailAddress(from + "@tencent.com");

				if(mailToStr!=null&&mailToStr!="")
				{
					string[] mainsTo=mailToStr.Split(';');
					foreach(string oneMailTo in mainsTo)
					{
						if(oneMailTo.Trim()!=""&&oneMailTo.Trim().IndexOf("@") > -1)
						{
                            mail.To.Add(oneMailTo);
						}
						else if(oneMailTo.Trim()!="")
						{
                            mail.To.Add(oneMailTo + "@tencent.com");
						}
					}
				}

				if(mailccStr!=null&&mailccStr.Trim()!="")
				{
					string[] mainsCC=mailccStr.Split(';');
					foreach(string oneMailCC in mainsCC)
					{
						if(oneMailCC.Trim()!=""&&oneMailCC.Trim().IndexOf("@") > -1)
						{
                            mail.CC.Add(oneMailCC);
						}
						else if(oneMailCC.Trim()!="")
						{
                            mail.CC.Add(oneMailCC + "@tencent.com");
						}
					}
				}

                mail.Subject = mailSubject;
                mail.Body = mailBody;
                mail.IsBodyHtml = isBodyHtml;

                mail.Attachments.Clear();
				if(fileAttachment!=null&&fileAttachment.Length>0)
				{
					foreach(string oneFile in fileAttachment)
					{
                        mail.Attachments.Add(new Attachment(oneFile));
					}
				}

                var clent = new SmtpClient(smtpServer, port);
                clent.Credentials = new System.Net.NetworkCredential(from, passwd);
                clent.Send(mail);
			}
			catch(Exception ex)
			{
				log4net.ILog log = log4net.LogManager.GetLogger("SendMail发送内部邮件异常");
				if(log.IsErrorEnabled) log.Error(ex.Message);
                throw new Exception("CommSendMail发送内部邮件异常"+ex.Message);
			}
		}

        //可发送密件
        public static void SendInternalMailCanSecret(string mailToStr, string mailccStr, string mailSecrectStr, string mailSubject, string mailBody, bool isBodyHtml, string[] fileAttachment)
        {
            try
            {
                string from = ConfigurationManager.AppSettings["EmailAddress"].ToString();
                string passwd = ConfigurationManager.AppSettings["EmailPassword"].ToString();
                string smtpServer = ConfigurationManager.AppSettings["SMTPServer"].ToString();
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPServerPort"].ToString());

                var mail = new MailMessage();
                if (from.IndexOf("@") > -1)
                {
                    from = from.Substring(0, from.IndexOf("@"));
                }
                mail.From = new MailAddress(from + "@tencent.com");

                if (mailToStr != null && mailToStr != "")
                {
                    string[] mainsTo = mailToStr.Split(';');
                    foreach (string oneMailTo in mainsTo)
                    {
                        if (oneMailTo.Trim() != "" && oneMailTo.Trim().IndexOf("@") > -1)
                        {
                            mail.To.Add(oneMailTo);
                        }
                        else if (oneMailTo.Trim() != "")
                        {
                            mail.To.Add(oneMailTo + "@tencent.com");
                        }
                    }
                }

                if (mailccStr != null && mailccStr.Trim() != "")
                {
                    string[] mainsCC = mailccStr.Split(';');
                    foreach (string oneMailCC in mainsCC)
                    {
                        if (oneMailCC.Trim() != "" && oneMailCC.Trim().IndexOf("@") > -1)
                        {
                            mail.CC.Add(oneMailCC);
                        }
                        else if (oneMailCC.Trim() != "")
                        {
                            mail.CC.Add(oneMailCC + "@tencent.com");
                        }
                    }
                }

                if (mailSecrectStr != null && mailSecrectStr.Trim() != "")
                {
                    string[] mailSecrect = mailSecrectStr.Split(';');
                    foreach (string onemailSecrectStr in mailSecrect)
                    {
                        if (onemailSecrectStr.Trim() != "" && onemailSecrectStr.Trim().IndexOf("@") > -1)
                        {
                            mail.Bcc.Add(onemailSecrectStr);
                        }
                        else if (onemailSecrectStr.Trim() != "")
                        {
                            mail.Bcc.Add(onemailSecrectStr + "@tencent.com");
                        }
                    }
                }

                mail.Subject = mailSubject;
                mail.Body = mailBody;
                mail.IsBodyHtml = isBodyHtml;

                mail.Attachments.Clear();
                if (fileAttachment != null && fileAttachment.Length > 0)
                {
                    foreach (string oneFile in fileAttachment)
                    {
                        mail.Attachments.Add(new Attachment(oneFile));
                    }
                }

                var clent = new SmtpClient(smtpServer, port);
                clent.Credentials = new System.Net.NetworkCredential(from, passwd);
                clent.Send(mail);
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("SendMail发送内部邮件异常");
                if (log.IsErrorEnabled) log.Error(ex.Message);
                throw new Exception("CommSendMail发送内部邮件异常" + ex.Message);
            }
        }

        //发邮件
        public static void SendMsg(string toStr, string actionTypeStr, string paramStr) 
        {
            SendMsg(toStr, actionTypeStr, paramStr, 1);
        }

        //发短信
        public static void SendMessage(string toStr, string actionTypeStr, string paramStr)
        {
            SendMsg(toStr, actionTypeStr, paramStr, 3);
        }

        //发QQtips
        public static void SendMsgQQTips(string toStr, string actionTypeStr, string paramStr)
        {
            SendMsg(toStr, actionTypeStr, paramStr, 2);
        }

        public static void SendMsg(string toStr, string actionTypeStr, string paramStr, int directType) 
        {
            try
            {
                string msgIP = ConfigurationManager.AppSettings["MsgEmailIP"].ToString();
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["MsgEmailPort"].ToString());

                //direct=1&direct_email=123@qq.com&direct_qq=123&direct_mobile=15209242542&actiontype=123&param=xxx
                string msg = "direct=1";
                if (toStr != null && toStr != "")
                {
                    if (directType == 1) {
                        //email
                        msg += "&direct_email=" + toStr;
                    }
                    else if (directType == 2) {
                        //qq
                        msg += "&direct_qq=" + toStr;
                    }
                    else if (directType == 3) { 
                        //mobile
                        msg += "&direct_mobile=" + toStr;
                    }
                }
                if (actionTypeStr != null && actionTypeStr != "")
                {
                    msg += "&actiontype=" + actionTypeStr;
                }
                if (paramStr != null && paramStr != "")
                {
                    msg += "&" + paramStr;
                }

                CommMailSend cms = new CommMailSend();
                cms.Send(msgIP, port, msg);
            }
            catch (Exception ex) 
            {
                throw new Exception("CommSendMail发送消息异常" + ex.Message);
            }
        }

        private void Send(string ip, int port, string msg) 
        {
            //[1]协议 1
            byte[] v = new byte[1] { 0x02 };
            //[2]是否需要返回结果 0不需要
            byte[] needr = UDP.GetByteFromInt(0);
            //[3]服务类型 CMD_EMAIL_MSG_ON 33
            byte[] cmd_type = UDP.GetByteFromInt(0);
            //[5]内容
            //{1}用户内部ID 1直发模式 0使用uid
            byte[] b_uid = UDP.GetByteFromInt(1);
            //{2}发送掩码  0x00000002
            byte[] b_mask = UDP.GetByteFromInt(0x00000007);
            //{4}消息
            byte[] b_msg = System.Text.Encoding.GetEncoding("GB2312").GetBytes(msg);
            //{3}消息长度
            byte[] b_msg_len = UDP.GetByteFromInt(b_msg.Length);

            //<1>对内容组包
            byte[] contData = new byte[b_uid.Length + b_mask.Length + b_msg_len.Length + b_msg.Length];
            b_uid.CopyTo(contData, 0);
            b_mask.CopyTo(contData, b_uid.Length);
            b_msg_len.CopyTo(contData, b_uid.Length + b_mask.Length);
            b_msg.CopyTo(contData, b_uid.Length + b_mask.Length + b_msg_len.Length);

            //[4]内容长度
            byte[] cont_len = UDP.GetByteFromInt(contData.Length);
            //[6]包序列号
            byte[] seq = UDP.GetByteFromInt(0);

            //<2>发送包
            byte[] data = new byte[v.Length + needr.Length + cmd_type.Length + cont_len.Length + contData.Length + seq.Length];
            v.CopyTo(data, 0);
            needr.CopyTo(data, v.Length);
            cmd_type.CopyTo(data, v.Length + needr.Length);
            cont_len.CopyTo(data, v.Length + needr.Length + cmd_type.Length);
            contData.CopyTo(data, v.Length + needr.Length + cmd_type.Length + cont_len.Length);
            seq.CopyTo(data, v.Length + needr.Length + cmd_type.Length + cont_len.Length + contData.Length);

            UDP.SendUDP(data, ip, port);
        }

        //更新手机
        public static bool ChangeMobile(int uid, string moblie, string signedString, out string msgErr)
        {
            return ChangeOrBindMobile(uid, moblie, signedString, 4, out msgErr);
        }

        //绑定手机
        public static bool BindMobile(int uid, string moblie, string signedString, out string msgErr)
        {
            return ChangeOrBindMobile(uid, moblie, signedString, 41, out msgErr);
        }

        /*
        30000	//用户不存在(查询或修改用户信息注册信息时)
        30001	//用户已经注册过(注册用户时)
        30002	//用户注册后还没激活
        30003	//用户没有开通任何类型的消息通知或者没有开通任何指定的消息通知类型,无法发送消息
        30004	//用户没有注册QQ号码,无法开通QQ通知
        30005	//用户没有注册EMAIL,无法开通EMAIL通知
        30006	//用户没有注册手机,无法开通手机通知
        30007	//随机验证码发送过于频繁
        30008   //请求参数错误
        30009   //脏词
        30010   //队列满
        30011	//签名错误
        30012	//原手机验证码错误
        30013	//新手机验证码错误
        30014	//系统繁忙,小于1000号系统错误全部采用此错误信息
        */
        //申诉新库表，type=1 绑定手机； type=5,6更新手机
        public static bool ChangeOrBindMobile(int uid, string moblie, string signedString, int cmdType,out string msgErr)
        {
            try
            {
                msgErr = "";
                string ip = ConfigurationManager.AppSettings["MsgEmailIP"].ToString();
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["MsgEmailPort"].ToString());

                //[1]协议 1
                byte[] v = new byte[1] { 0x02 };
                //[2]是否需要返回结果 0不需要
                byte[] needr = UDP.GetByteFromInt(0);
                //[3]服务类型 CMD_EMAIL_MSG_ON 33
                byte[] cmd_type = UDP.GetByteFromInt(cmdType);//4 更新手机 41 绑定手机

                //[5]内容
                //{1}用户内部ID 1直发模式 0使用uid
                byte[] b_uid = UDP.GetByteFromInt(uid);
                //内容[2] QQ号码 
                byte[] b_qqid = UDP.GetByteFromInt(0);
                //内容[3] Femail
                byte[] b_email = new byte[66];
                byte[] email = System.Text.Encoding.GetEncoding("GB2312").GetBytes("");
                email.CopyTo(b_email, 0);
                //内容[4] Fmobile
                byte[] b_moblile = new byte[65];
                byte[] moblile = System.Text.Encoding.GetEncoding("GB2312").GetBytes(moblie);
                moblile.CopyTo(b_moblile, 0);
                //内容[5] Fstatus
                byte[] b_status = UDP.GetByteFromInt(0x00000040);
                //内容[6] FregTime
                byte[] b_regTime = UDP.GetByteFromUInt(GetStamp(DateTime.Now));
                //内容[7] FupdateTime
                byte[] b_updateTime = UDP.GetByteFromUInt(GetStamp(DateTime.Now));


                //签名
                byte[] sign = System.Text.Encoding.GetEncoding("GB2312").GetBytes(signedString);

                //<1>对内容组包
                byte[] contData = new byte[b_uid.Length + b_qqid.Length + b_email.Length + b_moblile.Length + b_status.Length + b_regTime.Length + b_updateTime.Length + sign.Length];
                b_uid.CopyTo(contData, 0);
                b_qqid.CopyTo(contData, b_uid.Length);
                b_email.CopyTo(contData, b_uid.Length + b_qqid.Length);
                b_moblile.CopyTo(contData, b_uid.Length + b_qqid.Length + b_email.Length);
                b_status.CopyTo(contData, b_uid.Length + b_qqid.Length + b_email.Length + b_moblile.Length);
                b_regTime.CopyTo(contData, b_uid.Length + b_qqid.Length + b_email.Length + b_moblile.Length + b_status.Length);
                b_updateTime.CopyTo(contData, b_uid.Length + b_qqid.Length + b_email.Length + b_moblile.Length + b_status.Length + b_regTime.Length);
                sign.CopyTo(contData, b_uid.Length + b_qqid.Length + b_email.Length + b_moblile.Length + b_status.Length + b_regTime.Length + b_updateTime.Length);

                //[4]内容长度
                byte[] cont_len = UDP.GetByteFromInt(contData.Length);
                //[6]包序列号
                byte[] seq = UDP.GetByteFromInt(0);

                //<2>发送包
                byte[] data = new byte[v.Length + needr.Length + cmd_type.Length + cont_len.Length + contData.Length + seq.Length];
                v.CopyTo(data, 0);
                needr.CopyTo(data, v.Length);
                cmd_type.CopyTo(data, v.Length + needr.Length);
                cont_len.CopyTo(data, v.Length + needr.Length + cmd_type.Length);
                contData.CopyTo(data, v.Length + needr.Length + cmd_type.Length + cont_len.Length);
                seq.CopyTo(data, v.Length + needr.Length + cmd_type.Length + cont_len.Length + contData.Length);


                byte[] result = UDP.GetUDPReply(data, ip, port);


                byte[] m_dwRetCode = new byte[4] { result[5], result[6], result[7], result[8] };
                int dwRetCode = UDP.GetIntFromByte(m_dwRetCode);
                if (dwRetCode == 0)
                {
                    //手机更新成功
                    return true;
                }
                else
                {
                    byte[] m_dwContentLen = new byte[4] { result[9], result[10], result[11], result[12] };
                    int dwContentLen = UDP.GetIntFromByte(m_dwContentLen);
                    if (dwContentLen > 0)
                    {
                        byte[] m_sContent = new byte[dwContentLen];
                        for (int i = 0; i < dwContentLen; i++)
                        {
                            m_sContent[i] = result[13 + i];
                        }
                        string sContent = System.Text.Encoding.Default.GetString(m_sContent);//错误信息
                        Regex re = new Regex(@"([^A-Za-z\u4E00-\u9FA5])", RegexOptions.Multiline | RegexOptions.ExplicitCapture);
                        MatchCollection mc = re.Matches(sContent);
                        foreach (Match ma in mc)
                        {
                            sContent = sContent.Replace(ma.Value, "");
                        }
                        msgErr = "调服务更新手机出错：" + sContent;
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                msgErr = e.Message;
                return false;
            }
            return true;
        }

        public static UInt32 GetStamp(DateTime dt)
        {
            TimeSpan ts = dt - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            UInt32 uiStamp = Convert.ToUInt32(ts.TotalSeconds);
            return uiStamp;
        }

	}
}
