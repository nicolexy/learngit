using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace CFT.CSOMS.Service.CSAPI.Utility
{
    public class ResultParse<T>
    {
        public RetObject<T> ReturnToObject(List<T> list, string code, string msg)
        {
            var subReq = new RetObject<T>();
            subReq.ReturnCode = code;
            subReq.Message = msg;

            if (list != null)
            {
                subReq.Count = list.Count.ToString();
                subReq.Records = list;
            }

            return subReq;
        }

        public RetObject<T> ReturnToObject(List<T> list) 
        {
            return ReturnToObject(list, APIUtil.SUCC, APIUtil.MSG_OK);
        }
    }

    [XmlRoot("root")]
    public class RetObject<T>
    {
        [XmlElement("return_code")]
        public string ReturnCode { get; set; }
        [XmlElement("msg")]
        public string Message { get; set; }
        [XmlElement("count")]
        public string Count { get; set; }

        [XmlArray("records")]
        [XmlArrayItem("record")]
        public List<T> Records { get; set; }

        public string toString()
        {
            return "return_code[" + ReturnCode + "],msg[" + Message + "]";
        }
    }

    public class Record
    {
        [XmlElement("ret_value")]
        public string RetValue { get; set; }
    }
}