using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections;
using System.Text.RegularExpressions;

namespace TENCENT.OSS.CFT.KF.Common
{
	/// <summary>
	/// UDP ��ժҪ˵����
	/// </summary>
	public class UDP
	{
		public static bool GetTCPReply(byte[] sendBytes, string ServerIP, int ServerPort, out string ErrorMsg)
		{
			ErrorMsg = "";

			if(sendBytes.Length<1)
			{
				ErrorMsg = "���ݰ�����";
				return false;
			}

			
			TcpClient tcpClient = new TcpClient();

			IPAddress ipAddress = IPAddress.Parse(ServerIP);
			IPEndPoint ipLocalEndPoint = new IPEndPoint(ipAddress, ServerPort);
 			
			tcpClient.Connect(ipLocalEndPoint);
			NetworkStream ns = tcpClient.GetStream();

			try
			{

				ns.Write(sendBytes,0,sendBytes.Length);
			
				byte[] bufferout = new byte[8];
				int readcount = ns.Read(bufferout,0,8);

				if(readcount != 8) 
				{
					ErrorMsg = "";
					return false;
				}

				byte[] input = new byte[4];
				input[0] = bufferout[4];
				input[1] = bufferout[5];
				input[2] = bufferout[6];
				input[3] = bufferout[7];
				int datalen = BitConverter.ToInt32(input,0);
				//int datalen = UDP.GetIntFromByte(input);

				if (datalen < 0)
				{
					ErrorMsg = "δ֪����";
					return false;
				}
				else
				{
					return true;
				}
			}
			catch(Exception err)
			{
				ErrorMsg = err.Message;
				return false;
			}
			finally
			{
				ns.Close();
				tcpClient.Close();
			}			
		}

        public static string GetTCPReplyString(byte[] sendBytes, string ServerIP, int ServerPort, out string ErrorMsg)
        {
            ErrorMsg = "";
            if (sendBytes.Length < 1)
            {
                ErrorMsg = "���ݰ�����";
                return "";
            }

            TcpClient tcpClient = new TcpClient();
            IPAddress ipAddress = IPAddress.Parse(ServerIP);
            IPEndPoint ipLocalEndPoint = new IPEndPoint(ipAddress, ServerPort);
            tcpClient.Connect(ipLocalEndPoint);
            NetworkStream ns = tcpClient.GetStream();
            try
            {
                ns.Write(sendBytes, 0, sendBytes.Length);
                byte[] bufferOut = new byte[1024];
                string answer = "";
                ns.Read(bufferOut, 0, 1024);
                answer = Encoding.Default.GetString(bufferOut);
                return answer;
            }
            catch (Exception err)
            {
                ErrorMsg = err.Message;
                return "";
            }
            finally
            {
                ns.Close();
                tcpClient.Close();
            }
        }

        public static Hashtable tcpParameters(string answer)
        {
            string validAnswer = Regex.Match(answer, @"[^\\\0\r\n]*").Value;
            Hashtable paramsHt = new Hashtable();

            if (validAnswer != null)
            {
                foreach (String item in Regex.Split(validAnswer, "&", RegexOptions.IgnoreCase))
                {
                    String[] keyValue = Regex.Split(item, "=", RegexOptions.IgnoreCase);
                    if (keyValue.Length == 2)
                    {
                        paramsHt.Add(keyValue[0], keyValue[1]);
                    }
                }
                return paramsHt;
            }
            return paramsHt;
        }
        /// <summary>
        /// �ô���������ȥ��Զ�̶˿ڷ�������,���õ������.
        /// </summary>
        /// <param name="sendBytes">������</param>
        /// <returns>������</returns>
        public static byte[] GetUDPReply(byte[] sendBytes,string ServerIP, int ServerPort)
        {
            
            if(sendBytes.Length<1) throw new LogicException("���ݰ�����");

            UDPThread dt = new UDPThread(sendBytes,ServerIP,ServerPort);
            dt.OpenThread();

            for(int i=0; i<100; i++)
            {
                Thread.Sleep(100);
                if(dt.flag)
                {
                    return dt.ReceiveByte;
                }

                if(i == 99)
                {
                    dt.Close();
                    throw new LogicException("UDP�Ӱ���ʱ��");
                }
            }
            return null;
        }

		/// <summary>
		/// �ô���������ȥ��Զ�̶˿ڷ�������,���õ������.
		/// </summary>
		/// <param name="sendBytes">������</param>
		/// <returns>������</returns>
		public static void GetUDPReplyNoReturn(byte[] sendBytes,string ServerIP, int ServerPort)
		{
			if(sendBytes.Length<1) throw new LogicException("���ݰ�����");
			UDPThread dt = new UDPThread(sendBytes,ServerIP,ServerPort);
			dt.OpenThread();
			dt.Close();
		}

		/// <summary>
		/// �ô���������ȥ��Զ�̶˿ڷ�������,�������ս����.
		/// </summary>
		/// <param name="sendBytes">������</param>
		/// <returns>������</returns>
		public static void GetUDPReplyNoReturnNotReturn(byte[] sendBytes,string ServerIP, int ServerPort)
		{
			if(sendBytes.Length<1) throw new LogicException("���ݰ�����");
			UDPThread dt = new UDPThread(sendBytes,ServerIP,ServerPort);
			dt.OpenThreadNotReturn();
			dt.Close();
		}


        /// <summary>
        /// �ô���������ȥ��Զ�̶˿ڷ�������,���õ����.
        /// </summary>
        public static bool SendUDP(byte[] sendBytes,string ServerIP, int ServerPort)
        {
            UdpClient udpClient = new UdpClient();

            IPAddress ipAddress = IPAddress.Parse(ServerIP);
            IPEndPoint ipLocalEndPoint = new IPEndPoint(ipAddress, ServerPort);
 			
            int isend =udpClient.Send(sendBytes, sendBytes.Length,ipLocalEndPoint);
            return isend == sendBytes.Length;
        }

        /// <summary>
        /// ��һ��32λ������ת���ɷ��Ϸ��Ҫ�����λbyte����.
        /// </summary>
        /// <param name="input">Ҫת����������</param>
        /// <returns>���ɵĳ���Ϊ4��byte����</returns>
        public static byte[] GetByteFromInt(int input)
        {
            byte[] ResultX = BitConverter.GetBytes(input);
            byte[] Result = new byte[4];
            Result[0] = ResultX[3];
            Result[1] = ResultX[2];
            Result[2] = ResultX[1];
            Result[3] = ResultX[0];
            return Result;
        }

        /// <summary>
        /// ��һ������Ϊ4��byte����ת����һ��32λ������
        /// </summary>
        /// <param name="input">����Ϊ4��byte����</param>
        /// <returns>32λ������</returns>
        public static int GetIntFromByte(byte[] input)
        {
            if(input.Length != 4) return 0;

            byte[] tmp = new byte[4];
            tmp[0] = input[3];
            tmp[1] = input[2];
            tmp[2] = input[1];
            tmp[3] = input[0];

            return BitConverter.ToInt32(tmp,0);
        }



        public static byte[] GetByteFromUInt(uint input)
        {
            byte[] ResultX = BitConverter.GetBytes(input);
            byte[] Result = new byte[4];
            Result[0] = ResultX[3];
            Result[1] = ResultX[2];
            Result[2] = ResultX[1];
            Result[3] = ResultX[0];
            return Result;
        }

        //public static byte[] GetByteFromString(string input)
        //{
        //    byte[] ResultX = new byte[65];
        //    byte[] x=System.Text.Encoding.GetEncoding("GB2312").GetBytes(input);
        //    x.CopyTo(ResultX, 0);
        //    byte[] Result = new byte[65];
          
        //    for (int i = 0; i < 65;i++ )
        //    {
        //        Result[i] = ResultX[64-i];
               
        //    }
        //    return Result;
        //}

    }

    public class UDPThread
    {
        public byte[] ReceiveByte;
        public bool flag = false;

        private Thread tr = null;
        private byte[] fsendBytes;
        private string fServerIP;
        private int fServerPort;

        public void Close()
        {
            if(tr != null )
            {
                tr.Abort();
            }

        }

        public UDPThread(byte[] sendBytes,string ServerIP, int ServerPort)
        {
            fsendBytes = sendBytes;
            fServerIP = ServerIP;
            fServerPort = ServerPort;
        }

        public void OpenThread()
        {
            Thread tr = new Thread(new ThreadStart(RunThread));
            tr.Start();
        }

		public void OpenThreadNotReturn()
		{
			Thread tr = new Thread(new ThreadStart(RunThreadNotReturn));
			tr.Start();
		}

        void RunThread()
        {
            flag = false;

            UdpClient udpClient = new UdpClient();
            try
            {         
                udpClient.Client.SendTimeout = 60000;//��������ʱ����60s
                udpClient.Client.ReceiveTimeout = 5000;//5s
                IPAddress ipAddress = IPAddress.Parse(fServerIP);
                IPEndPoint ipLocalEndPoint = new IPEndPoint(ipAddress, fServerPort);

                udpClient.Connect(ipLocalEndPoint); 
                int isend =udpClient.Send(fsendBytes, fsendBytes.Length);   

               
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                ReceiveByte = udpClient.Receive(ref RemoteIpEndPoint);

                flag = true;
            }
            catch(Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("UDP error");
                if (log.IsInfoEnabled)
                    log.Info("UDP.RunThread error:" + ex);

                udpClient.Close();

                flag = false;
            }            
        }

		//�޷���ֵ��
		void RunThreadNotReturn()
		{
			try
			{
				UdpClient udpClient = new UdpClient();

				IPAddress ipAddress = IPAddress.Parse(fServerIP);
				IPEndPoint ipLocalEndPoint = new IPEndPoint(ipAddress, fServerPort);

				udpClient.Connect(ipLocalEndPoint); 
				udpClient.Send(fsendBytes, fsendBytes.Length);   
				IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
			}
			catch
			{
			}            
		}

    }
}
