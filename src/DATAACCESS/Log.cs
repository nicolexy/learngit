/************************************************************
  FileName: Log.cs
  Author: Furion       Version :    1.0      Date:2005-06-01
  Description: ��־�࣬���Լ�¼������־���쳣��־��Ч�ʼ�⡣    // ģ������      
  Version:         // �汾��Ϣ
  Function List:   // ��Ҫ�������书��
    1. WriteLog      ��¼�ճ�������־��
    2. WriteErrLog   ��¼�쳣��־��
    3. WriteStart    ������ʼ��ʱ��
    4. WriteEnd      ������ʱ������
  History:         // ��ʷ�޸ļ�¼
      <author>  <time>   <version >   <desc>
      Furion  2005-06-01     1.0     build this moudle  
***********************************************************/

using System;
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Text;

namespace TENCENT.OSS.CFT.KF.Common
{
	/// <summary>
	/// ��־������̬�࣬�����ܶ࣬���Ը�ϵͳ��ʹ��ʱҪ���н�һ����װ������PublicRes.CS�С�
	/// </summary>
    public class LogManage
    {
        private string fIP; //Զ�̷�����IP
        private int fPort;  //Զ�̷���������˿�.

        public LogManage(string ServerIP, int ServerPort)
        {
            fIP = ServerIP;
            fPort = ServerPort;
        }

        private bool WriteLog(string FileType,string Suin,string SystemId, string Level, string ErrorId, string Content)
        {
            TWriteLogIn wli = new TWriteLogIn(FileType);
            wli.Suin = Suin;
            wli.SystemId = SystemId;
            wli.Level = Level;
            wli.ErrorId = ErrorId;
            wli.Content = Content;
            
            return  UDP.SendUDP(wli.ToBytes(),fIP,fPort);
        }


        /// <summary>
        /// ��¼��־���ļ�
        /// </summary>
        /// <param name="Suin">������QQ������߲������ǳ�</param>
        /// <param name="SystemId">��¼ϵͳ�ı�ʶ,��֧��server��¼pay_server</param>
        /// <param name="Level">��¼��־����,��Ϊ���¼���info,debug,warning,error</param>
        /// <param name="ErrorId">��¼����ID��,���ڲ�ѯ��ͳ��</param>
        /// <param name="Content">��������,����С��512byte</param>
        /// <returns></returns>
        public bool Log(string Suin,string SystemId, string Level, string ErrorId, string Content)
        {
            return WriteLog("file",Suin,SystemId,Level,ErrorId,Content);
        }

        /// <summary>
        /// ��¼��־�����ݿ�
        /// </summary>
        /// <param name="Suin">������QQ������߲������ǳ�</param>
        /// <param name="SystemId">��¼ϵͳ�ı�ʶ,��֧��server��¼pay_server</param>
        /// <param name="Level">��¼��־����,��Ϊ���¼���info,debug,warning,error</param>
        /// <param name="ErrorId">��¼����ID��,���ڲ�ѯ��ͳ��</param>
        /// <param name="Content">��������,����С��512byte</param>
        /// <returns></returns>
        public bool DBLog(string Suin,string SystemId, string Level, string ErrorId, string Content)
        {
            return WriteLog("table",Suin,SystemId,Level,ErrorId,Content);
        }
    }


    public class TWriteLogIn
    {
        private string LogType; //11
        public string Suin; //����13byte,��¼������QQ������߲������ǳ�
        public string SystemId; //����11byte,��¼ϵͳ�ı�ʶ,��֧��server��¼pay_server.
        public string Level; //����11byte,��¼��־����,��Ϊ���¼���info,debug,warning,error
        public string ErrorId; //����11byte, ��¼����ID��,���ڲ�ѯ��ͳ��
        public string Content; //��¼��������,����С��512byte

        public TWriteLogIn(string logType)
        {
            LogType = logType;
        }

        public byte[] ToBytes()
        {
            int ilen = 0;
            int bytelen = 569;

            byte[] result = new byte[bytelen];

            byte[] tmp = Encoding.GetEncoding("GB2312").GetBytes(LogType);
            tmp.CopyTo(result,0);
            ilen = 11;

            tmp = Encoding.GetEncoding("GB2312").GetBytes(Suin);
            tmp.CopyTo(result,ilen );
            ilen = ilen + 13;

            tmp = Encoding.GetEncoding("GB2312").GetBytes(SystemId);
            tmp.CopyTo(result,ilen);
            ilen = ilen + 11;

            tmp = Encoding.GetEncoding("GB2312").GetBytes(Level);
            tmp.CopyTo(result, ilen);
            ilen = ilen + 11;

            tmp = Encoding.GetEncoding("GB2312").GetBytes(ErrorId);
            tmp.CopyTo(result,ilen );
            ilen = ilen + 11;

            tmp = Encoding.GetEncoding("GB2312").GetBytes(Content);
            tmp.CopyTo(result,ilen);

            return result;
        }
    }

}
