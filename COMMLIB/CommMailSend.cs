using System;
using System.Configuration;
using System.Net.Mail;
using TENCENT.OSS.CFT.KF.Common;
using System.Text.RegularExpressions;

namespace TENCENT.OSS.C2C.Finance.Common.CommLib
{
	/// <summary>
    /// CommMailSend ��ժҪ˵����
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
				log4net.ILog log = log4net.LogManager.GetLogger("SendMail�����ڲ��ʼ��쳣");
				if(log.IsErrorEnabled) log.Error(ex.Message);
                throw new Exception("CommSendMail�����ڲ��ʼ��쳣"+ex.Message);
			}
		}

        //�ɷ����ܼ�
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
                log4net.ILog log = log4net.LogManager.GetLogger("SendMail�����ڲ��ʼ��쳣");
                if (log.IsErrorEnabled) log.Error(ex.Message);
                throw new Exception("CommSendMail�����ڲ��ʼ��쳣" + ex.Message);
            }
        }

        //���ʼ�
        public static void SendMsg(string toStr, string actionTypeStr, string paramStr) 
        {
            SendMsg(toStr, actionTypeStr, paramStr, 1);
        }

        //������
        public static void SendMessage(string toStr, string actionTypeStr, string paramStr)
        {
            SendMsg(toStr, actionTypeStr, paramStr, 3);
        }

        //��QQtips
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
                throw new Exception("CommSendMail������Ϣ�쳣" + ex.Message);
            }
        }

        private void Send(string ip, int port, string msg) 
        {
            //[1]Э�� 1
            byte[] v = new byte[1] { 0x02 };
            //[2]�Ƿ���Ҫ���ؽ�� 0����Ҫ
            byte[] needr = UDP.GetByteFromInt(0);
            //[3]�������� CMD_EMAIL_MSG_ON 33
            byte[] cmd_type = UDP.GetByteFromInt(0);
            //[5]����
            //{1}�û��ڲ�ID 1ֱ��ģʽ 0ʹ��uid
            byte[] b_uid = UDP.GetByteFromInt(1);
            //{2}��������  0x00000002
            byte[] b_mask = UDP.GetByteFromInt(0x00000007);
            //{4}��Ϣ
            byte[] b_msg = System.Text.Encoding.GetEncoding("GB2312").GetBytes(msg);
            //{3}��Ϣ����
            byte[] b_msg_len = UDP.GetByteFromInt(b_msg.Length);

            //<1>���������
            byte[] contData = new byte[b_uid.Length + b_mask.Length + b_msg_len.Length + b_msg.Length];
            b_uid.CopyTo(contData, 0);
            b_mask.CopyTo(contData, b_uid.Length);
            b_msg_len.CopyTo(contData, b_uid.Length + b_mask.Length);
            b_msg.CopyTo(contData, b_uid.Length + b_mask.Length + b_msg_len.Length);

            //[4]���ݳ���
            byte[] cont_len = UDP.GetByteFromInt(contData.Length);
            //[6]�����к�
            byte[] seq = UDP.GetByteFromInt(0);

            //<2>���Ͱ�
            byte[] data = new byte[v.Length + needr.Length + cmd_type.Length + cont_len.Length + contData.Length + seq.Length];
            v.CopyTo(data, 0);
            needr.CopyTo(data, v.Length);
            cmd_type.CopyTo(data, v.Length + needr.Length);
            cont_len.CopyTo(data, v.Length + needr.Length + cmd_type.Length);
            contData.CopyTo(data, v.Length + needr.Length + cmd_type.Length + cont_len.Length);
            seq.CopyTo(data, v.Length + needr.Length + cmd_type.Length + cont_len.Length + contData.Length);

            UDP.SendUDP(data, ip, port);
        }

        //�����ֻ�
        public static bool ChangeMobile(int uid, string moblie, string signedString, out string msgErr)
        {
            return ChangeOrBindMobile(uid, moblie, signedString, 4, out msgErr);
        }

        //���ֻ�
        public static bool BindMobile(int uid, string moblie, string signedString, out string msgErr)
        {
            return ChangeOrBindMobile(uid, moblie, signedString, 41, out msgErr);
        }

        /*
        30000	//�û�������(��ѯ���޸��û���Ϣע����Ϣʱ)
        30001	//�û��Ѿ�ע���(ע���û�ʱ)
        30002	//�û�ע���û����
        30003	//�û�û�п�ͨ�κ����͵���Ϣ֪ͨ����û�п�ͨ�κ�ָ������Ϣ֪ͨ����,�޷�������Ϣ
        30004	//�û�û��ע��QQ����,�޷���ͨQQ֪ͨ
        30005	//�û�û��ע��EMAIL,�޷���ͨEMAIL֪ͨ
        30006	//�û�û��ע���ֻ�,�޷���ͨ�ֻ�֪ͨ
        30007	//�����֤�뷢�͹���Ƶ��
        30008   //�����������
        30009   //���
        30010   //������
        30011	//ǩ������
        30012	//ԭ�ֻ���֤�����
        30013	//���ֻ���֤�����
        30014	//ϵͳ��æ,С��1000��ϵͳ����ȫ�����ô˴�����Ϣ
        */
        //�����¿��type=1 ���ֻ��� type=5,6�����ֻ�
        public static bool ChangeOrBindMobile(int uid, string moblie, string signedString, int cmdType,out string msgErr)
        {
            try
            {
                msgErr = "";
                string ip = ConfigurationManager.AppSettings["MsgEmailIP"].ToString();
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["MsgEmailPort"].ToString());

                //[1]Э�� 1
                byte[] v = new byte[1] { 0x02 };
                //[2]�Ƿ���Ҫ���ؽ�� 0����Ҫ
                byte[] needr = UDP.GetByteFromInt(0);
                //[3]�������� CMD_EMAIL_MSG_ON 33
                byte[] cmd_type = UDP.GetByteFromInt(cmdType);//4 �����ֻ� 41 ���ֻ�

                //[5]����
                //{1}�û��ڲ�ID 1ֱ��ģʽ 0ʹ��uid
                byte[] b_uid = UDP.GetByteFromInt(uid);
                //����[2] QQ���� 
                byte[] b_qqid = UDP.GetByteFromInt(0);
                //����[3] Femail
                byte[] b_email = new byte[66];
                byte[] email = System.Text.Encoding.GetEncoding("GB2312").GetBytes("");
                email.CopyTo(b_email, 0);
                //����[4] Fmobile
                byte[] b_moblile = new byte[65];
                byte[] moblile = System.Text.Encoding.GetEncoding("GB2312").GetBytes(moblie);
                moblile.CopyTo(b_moblile, 0);
                //����[5] Fstatus
                byte[] b_status = UDP.GetByteFromInt(0x00000040);
                //����[6] FregTime
                byte[] b_regTime = UDP.GetByteFromUInt(GetStamp(DateTime.Now));
                //����[7] FupdateTime
                byte[] b_updateTime = UDP.GetByteFromUInt(GetStamp(DateTime.Now));


                //ǩ��
                byte[] sign = System.Text.Encoding.GetEncoding("GB2312").GetBytes(signedString);

                //<1>���������
                byte[] contData = new byte[b_uid.Length + b_qqid.Length + b_email.Length + b_moblile.Length + b_status.Length + b_regTime.Length + b_updateTime.Length + sign.Length];
                b_uid.CopyTo(contData, 0);
                b_qqid.CopyTo(contData, b_uid.Length);
                b_email.CopyTo(contData, b_uid.Length + b_qqid.Length);
                b_moblile.CopyTo(contData, b_uid.Length + b_qqid.Length + b_email.Length);
                b_status.CopyTo(contData, b_uid.Length + b_qqid.Length + b_email.Length + b_moblile.Length);
                b_regTime.CopyTo(contData, b_uid.Length + b_qqid.Length + b_email.Length + b_moblile.Length + b_status.Length);
                b_updateTime.CopyTo(contData, b_uid.Length + b_qqid.Length + b_email.Length + b_moblile.Length + b_status.Length + b_regTime.Length);
                sign.CopyTo(contData, b_uid.Length + b_qqid.Length + b_email.Length + b_moblile.Length + b_status.Length + b_regTime.Length + b_updateTime.Length);

                //[4]���ݳ���
                byte[] cont_len = UDP.GetByteFromInt(contData.Length);
                //[6]�����к�
                byte[] seq = UDP.GetByteFromInt(0);

                //<2>���Ͱ�
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
                    //�ֻ����³ɹ�
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
                        string sContent = System.Text.Encoding.Default.GetString(m_sContent);//������Ϣ
                        Regex re = new Regex(@"([^A-Za-z\u4E00-\u9FA5])", RegexOptions.Multiline | RegexOptions.ExplicitCapture);
                        MatchCollection mc = re.Matches(sContent);
                        foreach (Match ma in mc)
                        {
                            sContent = sContent.Replace(ma.Value, "");
                        }
                        msgErr = "����������ֻ�����" + sContent;
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
