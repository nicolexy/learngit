using CFT.CSOMS.DAL.SysManageModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.BLL.SysManageModule
{
    public class BankClassifyService
    {
        public static readonly Dictionary<string, string> BankBusinessType = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> BankUseStatus = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> BankCardType = new Dictionary<string, string>();

        static BankClassifyService()
        {
            BankBusinessType.Add("1", "提现");
            BankBusinessType.Add("2", "向银行卡付款");
            BankBusinessType.Add("3", "还房贷");
            BankBusinessType.Add("4", "信用卡还款");
            BankBusinessType.Add("5", "代扣");
            BankBusinessType.Add("6", "收款");
            BankBusinessType.Add("7", "实时提现");
            BankBusinessType.Add("8", "退款");

            BankUseStatus.Add("1", "未上线");
            BankUseStatus.Add("2", "正在使用");
            BankUseStatus.Add("3", "已下线");

            BankCardType.Add("1", "借记卡");
            BankCardType.Add("2", "信用卡");
            BankCardType.Add("3", "混合");
        }

        public DataSet QueryBankBusiInfo(int req_type, string bank_code, string bank_type, int business_type, int use_status, int offset, int limit, out int totalNum)
        {
            DataSet ds = new BankClassifyData().QueryBankBusiInfo(req_type, bank_code, bank_type, business_type, use_status, offset, limit, out totalNum);
            if (req_type == 0)
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("banktype_str", typeof(System.String));

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr.BeginEdit();
                        dr["banktype_str"] = TransferMeaning.Transfer.convertbankType(dr["bank_type"].ToString());
                        dr.EndEdit();
                    }
                }
            }
            return ds;
        }

        public bool OpBankBusiInfo(int op_type, string bank_type, int business_type,
           string bank_code, string bank_name, int card_type, int use_status)
        {
            return new BankClassifyData().OpBankBusiInfo(op_type, bank_type, business_type,bank_code, bank_name, card_type, use_status);
        }
    }
}
