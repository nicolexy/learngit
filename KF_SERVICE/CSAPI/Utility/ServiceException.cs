using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFT.CSOMS.Service.CSAPI.Utility
{
    public class ServiceException : System.Exception
    {
        private string msg;
        private string retcode;

        public ServiceException(string _retcode,string _message) : base(_message) 
        {
            msg = _message;
            retcode = _retcode;
        }

        public string GetRetcode 
        {
            get { return retcode; }
        }

        public string GetRetmsg
        {
            get { return msg; }
        }
    }
}