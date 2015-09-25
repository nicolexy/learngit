using CFT.Apollo.Logging;
using CFT.CSOMS.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace CFT.CSOMS.DAL.SysManageModule
{
    public class BankClassifyData
    {
        /// <summary>
        /// 银行业务分类信息查询接口
        /// </summary>
        /// <param name="req_type">查询银行编码与银行名称：1 其他则不传或者0</param>
        /// <param name="bank_code">银行字符编码</param>
        /// <param name="bank_type">银行数字编码，可查询多个，以|分割</param>
        /// <param name="business_type">银行业务类型</param>
        /// <param name="use_status">上线状态</param>
        /// <param name="offset">偏移量</param>
        /// <param name="limit">查询限制</param>
        /// <returns></returns>
        public DataSet QueryBankBusiInfo(int req_type, string bank_code, string bank_type, int business_type, int use_status,int offset,int limit,out int totalNum)
        {
            DataSet ds = null;

            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("BankClassifyIP", "172.27.31.177");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("BankClassifyPORT", 22000);

            string req = "req_type=" + req_type;            
            if (!string.IsNullOrEmpty(bank_code))
                req += "&bank_code=" + bank_code;

            if (!string.IsNullOrEmpty(bank_type))
                req += "&bank_type=" + bank_type;

            if (business_type > 0)
                req += "&business_type=" + business_type;

            if (use_status > 0)
                req += "&use_status=" + use_status;

            req += "&offset=" + offset + "&limit=" + limit;

            string answer = RelayAccessFactory.RelayInvoke(req, "101478", false, false, ip, port);

            string strMsg = null;
            ds = CommQuery.ParseRelayPageRowNum(answer, out strMsg, out totalNum);

            return ds;
        }

        /// <summary>
        /// 银行业务分类信息增删修操作接口
        /// </summary>
        /// <param name="op_type">增：1、删：2、改：3</param>
        /// <param name="bank_type">银行数字编码</param>
        /// <param name="business_type">
        /// 银行业务类型
        /// 1-提现(默认)、2-向银行卡付款、3-还房贷、4-信用卡还款、
        /// 5-代扣、6-收款、7-实时提现、8-退款
        /// </param>
        /// <param name="bank_code">银行字符编码（增、改必须；删非必须）</param>
        /// <param name="bank_name">银行名称（增、改必须；删非必须）</param>
        /// <param name="card_type">
        /// 卡类型 1-借记卡；2-信用卡；3-混合（增、改必须；删非必须）
        /// </param>
        /// <param name="use_status">
        /// 上线状态：1-未上线；2-正在使用；3-已下线（增、改必须；删非必须）
        /// </param>
        /// <returns></returns>
        public bool OpBankBusiInfo(int op_type, string bank_type, int business_type, 
            string bank_code, string bank_name, int card_type, int use_status)
        {
            bool result = false;

            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("BankClassifyIP", "172.27.31.177");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("BankClassifyPORT", 22000);

            string req = "op_type=" + op_type;
            req += "&bank_type=" + bank_type;
            req += "&business_type=" + business_type;

            if (!string.IsNullOrEmpty(bank_code))
                req += "&bank_code=" + bank_code;

            if (!string.IsNullOrEmpty(bank_name))
                req += "&bank_name=" + bank_name;

            if (card_type > 0)
                req += "&card_type=" + card_type;

            if (use_status > 0)
                req += "&use_status=" + use_status;

            string answer = RelayAccessFactory.RelayInvoke(req, "101479", false, false, ip, port);

            if (answer.IndexOf("result=0") > -1)
                result = true;
            else
            {
                LogHelper.LogInfo(bank_type + "银行分类信息导入错误：" + CommQuery.URLDecode(answer));
                //throw new Exception(CommQuery.URLDecode(answer));
            }

            return result;
        }
    }
}
