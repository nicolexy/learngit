using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.DAL.FreezeModule
{
    public class PostArgs
    {
        public class BaseRequest 
        {
            public string reqsource { get; set; }
            public string accid { get; set; }
            public string topcolor { get; set; }
            public string navtourl { get; set; }
            public string templateid { get; set; }

            public TemplateClass templatedata;
            public ReserveClass reserve;

            public string sign { get; set; }

        }

        public class TemplateClass 
        {
            public ValueClass first;
            public ValueClass keynote1;
            public ValueClass remark;
        }

        public class ReserveClass 
        {
            public string msgtype { get; set; }
            public MsgClass msgdata;
        }

        public class MsgClass 
        {
            public string frechannel { get; set; }
            public string strategyno { get; set; }
            public string seclev { get; set; }
            public string errcode { get; set; }
        } 

        public class ValueClass 
        {
            public string value { get; set; }
            public string color { get; set; }
        }
    }
}
