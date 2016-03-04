using SunLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace TENCENT.OSS.CFT.KF.KF_Web.classLibrary
{
    public class UDPConnection
    {
        private IPAddress _ip = null;
        private int _port;
        private UdpClient _udpc = null;

        public UDPConnection(IPAddress ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        public UdpClient Connection()
        {
            try
            {
                _udpc = new UdpClient();
                _udpc.Connect(_ip, _port);
            }
            catch (Exception ex)
            {
                LogHelper.LogError("UdpClient 初始化异常：" + ex.Message + ex.StackTrace);
                return null;
            }
            return _udpc;
        }
    }
}