using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text;
using TENCENT.OSS.CFT.KF.DataAccess;

namespace TENCENT.OSS.CFT.KF.Common
{
    /// <summary>
    /// Ȩ�޴���ӿ���.
    /// </summary>
    public class cSession
    {
        private string fIP; //Զ�̷�����IP
        private int fPort;  //Զ�̷���������˿�.

        public cSession(string ServerIP, int ServerPort)
        {
            fIP = ServerIP;
            fPort = ServerPort;
        }

        private byte[] GetUDPReply(byte[] sendBytes)
        {
            return UDP.GetUDPReply(sendBytes,fIP,fPort);
        }
//���Ժϲ��汾��33333
        /// <summary>
        /// ����SESSION����.
        /// </summary>
        /// <param name="myQuest">����SESSION��</param>
        /// <returns>���������</returns>
        public TCreateSessionReply CsCreateSessiond(TCreateSessionQuest myQuest)
        {
            byte[] tmp = GetUDPReply(myQuest.ToBytes());
			TCreateSessionReply sr = new TCreateSessionReply();
            sr.Init(tmp);
			return sr;
        }

        /// <summary>
        /// ����SESSION����.
        /// </summary>
        /// <param name="myQuest">����SESSION��</param>
        /// <returns>��������</returns>
        public TVerifySessionReply CsVerfiySessiond(TVerifySessionQuest myQuest)
        {
            return new TVerifySessionReply(GetUDPReply(myQuest.ToBytes()));
        }

        /// <summary>
        /// ����SESSION����
        /// </summary>
        /// <param name="myQuest">����SESSION��</param>
        /// <returns>���½����</returns>
        public TUpdateSessionReply CsUpdateSessiond(TUpdateSessionQuest myQuest)
        {
            return new TUpdateSessionReply(GetUDPReply(myQuest.ToBytes()));
        }

        /// <summary>
        /// ɾ��SESSION����.
        /// </summary>
        /// <param name="myQuest">ɾ��SESSION��</param>
        /// <returns>ɾ�������</returns>
        public TDeleteSessionReply CsDeleteSessiond(TDeleteSessionQuest myQuest)
        {
            return new TDeleteSessionReply(GetUDPReply(myQuest.ToBytes()));
        }
    }

    /// <summary>
    /// ��ͷ��ʽ. 
    /// </summary>
    struct TProtocolHead
    {
        public int Version; //�汾��
        public int Cmd; //�����ʶ
        public int Len; //���峤��
    }


    /// <summary>
    /// ����SESSION������.
    /// </summary>
    public class TCreateSessionQuest
    {
        public string szUserName = ""; //��¼���û���
        public string szPassword = "";  //��¼���û�����
        public string szUserData = "";  //��ҪSessiond������û��Զ�������

        private TProtocolHead myhead;

        private int UserNameLen = 20;
        private int PasswordLen = 16;
        private int UserDataLen = 255;

        public TCreateSessionQuest()
        {
            myhead = new TProtocolHead();
            myhead.Version = 1;
            myhead.Cmd = 1;
            myhead.Len = UserNameLen + PasswordLen + UserDataLen;
        }

        public byte[] ToBytes()
        {
            int bytelen = myhead.Len + 12; //���ݳ��ȼ���ͷ����.
            byte[] result = new byte[bytelen];

            byte[] tmp = UDP.GetByteFromInt(myhead.Version);
            tmp.CopyTo(result,0);

            tmp = UDP.GetByteFromInt(myhead.Cmd);
            tmp.CopyTo(result,4);

            tmp = UDP.GetByteFromInt(myhead.Len);
            tmp.CopyTo(result,8);

            if(szUserName.Length>UserNameLen)
            {
                szUserName = szUserName.Substring(0,UserNameLen);
            }

            if(szPassword.Length>PasswordLen)
            {
                szPassword = szPassword.Substring(0,PasswordLen);
            }

            if(szUserData.Length>UserDataLen)
            {
                szUserData = szUserData.Substring(0,UserDataLen);
            }

            tmp = Encoding.GetEncoding("GB2312").GetBytes(szUserName);
            tmp.CopyTo(result,12);

            tmp = Encoding.GetEncoding("GB2312").GetBytes(szPassword);
            tmp.CopyTo(result,12 + UserNameLen);

            tmp = Encoding.GetEncoding("GB2312").GetBytes(szUserData);
            tmp.CopyTo(result,12 + UserNameLen + PasswordLen);

            return result;
        }
    }

    /// <summary>
    /// ����SESSION�����
    /// </summary>
	public class TCreateSessionReply
    {
        public int OpResult; //���
        public string szKey; //ϵͳ���ɵ�Session key
        public int OperId;   //��¼���û�ID

		public string RightString;

        public TCreateSessionReply()
        {
            
        }

		public void Init(byte[] buffer)
		{
			byte[] input = new byte[4];
			input[0] = buffer[8];
			input[1] = buffer[9];
			input[2] = buffer[10];
			input[3] = buffer[11];
			int datalen = UDP.GetIntFromByte(input);

			OpResult = buffer[12];

			input[0] = buffer[datalen + 8];
			input[1] = buffer[datalen + 9];
			input[2] = buffer[datalen + 10];
			input[3] = buffer[datalen + 11];

			OperId = UDP.GetIntFromByte(input);
			szKey = Encoding.GetEncoding("GB2312").GetString(buffer,13,datalen-5);
		}

		public string GetRightString(string strConn)
		{
			MySqlAccess da = new MySqlAccess(strConn);
			try
			{
				da.OpenConn();
				string strSql = "select FOperatorRoleID  from t_user_droit where FGroupID=56 and FOperatorID=" + OperId.ToString();
				string tmp = da.GetOneResult(strSql);

				strSql = "select FRoleDroit from t_operator_role where  FOperatorRoleID=" + tmp;				
				return da.GetOneResult(strSql);
			}
			catch(Exception err) 
			{
				string tmp = err.Message;
				throw err;
			}
			finally
			{
				da.Dispose();
			}
		}
    }

    /// <summary>
    /// ����SESSION��.
    /// </summary>
    public class TVerifySessionQuest
    {
        public string szKey = ""; //��cookies�ж�ȡ��Sessiond Key  ����16
        public int OperId = 0;   //��cookies�ж�ȡ��OPerId
        public int GroupId = 0;  //��ǰ��ҵ���� Ϊ0ʱֻ��ȡ�û���������
        public int ServiceId = 0; //��ǰ�Ĳ������� ��1��ʼ��0����
        public int iUin = 0; //���β�����QQ���룬���Բ���

        private TProtocolHead myhead;

        public TVerifySessionQuest()
        {
            myhead = new TProtocolHead();
            myhead.Version = 1;
            myhead.Cmd = 2;
            myhead.Len = 16 + 4*4;
        }

        public byte[] ToBytes()
        {
            int bytelen = myhead.Len + 12; //���ݳ��ȼ���ͷ����.
            byte[] result = new byte[bytelen];

            byte[] tmp = UDP.GetByteFromInt(myhead.Version);
            tmp.CopyTo(result,0);

            tmp = UDP.GetByteFromInt(myhead.Cmd);
            tmp.CopyTo(result,4);

            tmp = UDP.GetByteFromInt(myhead.Len);
            tmp.CopyTo(result,8);

            if(szKey.Length>16)
            {
                szKey = szKey.Substring(0,16);
            }

            tmp = Encoding.GetEncoding("GB2312").GetBytes(szKey);
            tmp.CopyTo(result,12);

            tmp = UDP.GetByteFromInt(OperId);
            tmp.CopyTo(result,12 + 16);

            tmp = UDP.GetByteFromInt(GroupId);
            tmp.CopyTo(result,12 + 16 + 4);

            tmp = UDP.GetByteFromInt(ServiceId);
            tmp.CopyTo(result,12 + 16 + 4 + 4);

            tmp = UDP.GetByteFromInt(iUin);
            tmp.CopyTo(result,12 + 16 + 4 + 4 + 4);

            return result;
        }
    }

    /// <summary>
    /// ����SESSION�����.
    /// </summary>
    public class TVerifySessionReply
    {
        public int OpResult; //���
        public string szUserData; //�����û��Զ�������

        public TVerifySessionReply(byte[] buffer)
        {
            OpResult = buffer[12];
            szUserData = Encoding.GetEncoding("GB2312").GetString(buffer,13,buffer.Length-13).Trim();
        }
    }


    /// <summary>
    /// ����SESSION��.
    /// </summary>
    public class TUpdateSessionQuest
    {
        public string szKey = ""; //��cookies�ж�ȡ��Sessiond Key  ����16
        public int OperId = 0;   //��cookies�ж�ȡ��OPerId
        public int GroupId = 0;  //��ǰ��ҵ���� Ϊ0ʱֻ��ȡ�û���������
        public int ServiceId = 0; //��ǰ�Ĳ������� ��1��ʼ��0����
        public int iUin = 0; //���β�����QQ���룬���Բ���

        public string szContext = ""; //������¼��ע 80
        public string szUserData = ""; //�û����� 255

        private TProtocolHead myhead;

        public TUpdateSessionQuest()
        {
            myhead = new TProtocolHead();
            myhead.Version = 1;
            myhead.Cmd = 3;
            myhead.Len = 16 + 4*4 + 80 + 255;
        }

        public byte[] ToBytes()
        {
            int bytelen = myhead.Len + 12; //���ݳ��ȼ���ͷ����.
            byte[] result = new byte[bytelen];

            byte[] tmp = UDP.GetByteFromInt(myhead.Version);
            tmp.CopyTo(result,0);

            tmp = UDP.GetByteFromInt(myhead.Cmd);
            tmp.CopyTo(result,4);

            tmp = UDP.GetByteFromInt(myhead.Len);
            tmp.CopyTo(result,8);

            if(szKey.Length>16)
            {
                szKey = szKey.Substring(1,16);
            }

            if(szContext.Length>80)
            {
                szContext = szContext.Substring(0,80);
            }

            if(szUserData.Length>255)
            {
                szUserData = szUserData.Substring(0,255);
            }

            tmp = Encoding.GetEncoding("GB2312").GetBytes(szKey);
            tmp.CopyTo(result,12);

            tmp = UDP.GetByteFromInt(OperId);
            tmp.CopyTo(result,12 + 16);

            tmp = UDP.GetByteFromInt(GroupId);
            tmp.CopyTo(result,12 + 16 + 4);

            tmp = UDP.GetByteFromInt(ServiceId);
            tmp.CopyTo(result,12 + 16 + 4 + 4);

            tmp = UDP.GetByteFromInt(iUin);
            tmp.CopyTo(result,12 + 16 + 4 + 4 + 4);

            tmp = Encoding.GetEncoding("GB2312").GetBytes(szContext);
            tmp.CopyTo(result,12 + 16 + 4*4);

            tmp = Encoding.GetEncoding("GB2312").GetBytes(szUserData);
            tmp.CopyTo(result,12 + 16 + 4*4 + 80);

            return result;
        }
    }

    /// <summary>
    /// ����SESSION�����.
    /// </summary>
    public class TUpdateSessionReply
    {
        public int OpResult; //���

        public TUpdateSessionReply(byte[] buffer)
        {
            OpResult = buffer[12];
        }
    }


    /// <summary>
    /// ɾ��SESSION��
    /// </summary>
    public class TDeleteSessionQuest
    {
        public string szKey = ""; //��cookies�ж�ȡ��Sessiond Key  ����16
        public int OperId = 0;   //��cookies�ж�ȡ��OPerId

        private TProtocolHead myhead;

        public TDeleteSessionQuest()
        {
            myhead = new TProtocolHead();
            myhead.Version = 1;
            myhead.Cmd = 4;
            myhead.Len = 16 + 4;
        }

        public byte[] ToBytes()
        {
            int bytelen = myhead.Len + 12; //���ݳ��ȼ���ͷ����.
            byte[] result = new byte[bytelen];

            byte[] tmp = UDP.GetByteFromInt(myhead.Version);
            tmp.CopyTo(result,0);

            tmp = UDP.GetByteFromInt(myhead.Cmd);
            tmp.CopyTo(result,4);

            tmp = UDP.GetByteFromInt(myhead.Len);
            tmp.CopyTo(result,8);

            if(szKey.Length>16)
            {
                szKey = szKey.Substring(0,16);
            }

            tmp = Encoding.GetEncoding("GB2312").GetBytes(szKey);
            tmp.CopyTo(result,12);

            tmp = UDP.GetByteFromInt(OperId);
            tmp.CopyTo(result,12 + 16);

            return result;
        }
    }

    /// <summary>
    /// ɾ��SESSION�����.
    /// </summary>
    public class TDeleteSessionReply
    {
        public int OpResult; //���

        public TDeleteSessionReply(byte[] buffer)
        {
            OpResult = buffer[12];
        }
    }

	/// <summary>
	/// handle the cache ��.
	/// </summary>
	public class TReleaseCache
	{
		public int commandlength = 12;
		public int commandcode = 7; 
		public int qq = 0;

		public TReleaseCache(int aqq)
		{
			qq = aqq;
		}

		public byte[] ToByte()
		{
			byte[] result = new byte[commandlength];

			//byte[] tmp = UDP.GetByteFromInt(commandlength);
			byte[] tmp = BitConverter.GetBytes(commandlength);
			tmp.CopyTo(result,0);

			//tmp = UDP.GetByteFromInt(commandcode);
			tmp = BitConverter.GetBytes(commandcode);
			tmp.CopyTo(result,4);

			//tmp = UDP.GetByteFromInt(qq);
			tmp = BitConverter.GetBytes(qq);
			tmp.CopyTo(result,8);

			return result;
		}
	}
}
