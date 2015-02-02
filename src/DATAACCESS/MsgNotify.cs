using System;
using System.Text;

namespace TENCENT.OSS.CFT.KF.Common
{
	/// <summary>
	/// MsgNotify 的摘要说明。
	/// </summary>
	public class MsgNotify
	{
		private string fIP; //远程服务器IP
		private int fPort;  //远程服务器服务端口.

		private int Version = 1;
		private int ReturnFlag = 1; //需要返回。
		private int ServiceType = 0; //服务类别 0代表发送消息。

		const int SENDTYPE = 7;  //屏蔽码为发出所有消息。

		public MsgNotify(string ServerIP, int ServerPort)
		{
			fIP = ServerIP;
			fPort = ServerPort;
		}

		private byte[] ToByte(int Uid, string msg)
		{
			byte[] tmpmsg = Encoding.GetEncoding("GB2312").GetBytes(msg);
			int strLen = tmpmsg.Length;

			int allLen = strLen + 12 + 13;
			byte[] result = new byte[allLen];

			result[0] = 1;

			byte[] tmp = UDP.GetByteFromInt(ReturnFlag);
			tmp.CopyTo(result,1);

			tmp = UDP.GetByteFromInt(ServiceType);
			tmp.CopyTo(result,5);

			tmp = UDP.GetByteFromInt(strLen + 12);
			tmp.CopyTo(result,9);

			tmp = UDP.GetByteFromInt(Uid);
			tmp.CopyTo(result,13);
            
			tmp = UDP.GetByteFromInt(SENDTYPE);
			tmp.CopyTo(result,17);

			tmp = UDP.GetByteFromInt(strLen);
			tmp.CopyTo(result,21);

			tmp = Encoding.GetEncoding("GB2312").GetBytes(msg);
			tmp.CopyTo(result,25);

			return result;
		}

		public bool SendMsg(int Uid, string msg, out string errorMsg)
		{
			errorMsg = "";

			if(msg == null || msg.Trim() == "") return false;

			byte[] buffer = ToByte(Uid,msg);

			byte[] result = UDP.GetUDPReply(buffer,fIP,fPort);

			if(result == null || result.Length == 0) return false;

			
			return DecodeResult(result,out errorMsg);
		}

		private bool DecodeResult(byte[] buffer, out string errorMsg)
		{
			errorMsg = "";

			byte[] input = new byte[4];
			input[0] = buffer[5];
			input[1] = buffer[6];
			input[2] = buffer[7];
			input[3] = buffer[8];

			int execResult = UDP.GetIntFromByte(input);

			if(execResult != 0)
			{
				input[0] = buffer[13];
				input[1] = buffer[14];
				input[2] = buffer[15];
				input[3] = buffer[16];

				int errorLength = UDP.GetIntFromByte(input);				

				if(errorLength > 0 && errorLength <= buffer.Length - 17)
				{
					byte[] error = new byte[errorLength];
					Array.Copy(buffer,17,error,0,errorLength);
					errorMsg = Encoding.GetEncoding("GB2312").GetString(error);
				}
			}

			return execResult == 0;
		}
	}
}
