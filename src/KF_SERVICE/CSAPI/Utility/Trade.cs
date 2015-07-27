using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace CFT.CSOMS.Service.CSAPI.Trade
{
    public class Trade
    {
        public Trade()
        {

        }

        public class TransferDic
        {
            [XmlElement("no")]
            public string Fno { get; set; }
            [XmlElement("type")]
            public string FType { get; set; }
            [XmlElement("value")]
            public string Fvalue { get; set; }
            [XmlElement("memo")]
            public string Fmemo { get; set; }
            [XmlElement("symbol")]
            public string Fsymbol { get; set; }
        }

        public class QQRollList
        {
            [XmlElement("action_type")]
            public string faction_type { get; set; }
            [XmlElement("action_type_str")]
            public string Faction_type_str { get; set; }
            [XmlElement("curtype")]
            public string fcurtype { get; set; }
            [XmlElement("curtype_str")]
            public string Fcurtype_str { get; set; }
            [XmlElement("type")]
            public string ftype { get; set; }
            [XmlElement("type_str")]
            public string Ftype_str { get; set; }
            [XmlElement("subject")]
            public string fsubject { get; set; }
            [XmlElement("subject_str")]
            public string Fsubject_str { get; set; }
            [XmlElement("paynum_str")]
            public string Fpaynum_str { get; set; }
            [XmlElement("balance_str")]
            public string Fbalance_str { get; set; }
            [XmlElement("con_str")]
            public string Fcon_str { get; set; }

            [XmlElement("BKid")]
            public string fBKid { get; set; }
            [XmlElement("listid")]
            public string flistid { get; set; }
            [XmlElement("uid")]
            public string fuid { get; set; }
            [XmlElement("qqid")]
            public string fqqid { get; set; }
            [XmlElement("true_name")]
            public string ftrue_name { get; set; }
            [XmlElement("from_uid")]    //对方内部帐号
            public string ffrom_uid { get; set; }
            [XmlElement("from_id")]
            public string ffromid { get; set; }
            [XmlElement("from_name")]
            public string ffrom_name { get; set; }

            [XmlElement("bank_type")]
            public string fbank_type { get; set; }
            [XmlElement("spid")]
            public string fspid { get; set; }
            [XmlElement("prove")]
            public string fprove { get; set; }
            [XmlElement("applyid")]
            public string fapplyid { get; set; }
            [XmlElement("ip")]
            public string fip { get; set; }
            [XmlElement("memo")]
            public string fmemo { get; set; }
            [XmlElement("modify_time_acc")]
            public string fmodify_time_acc { get; set; }
            [XmlElement("modify_time")]
            public string fmodify_time { get; set; }
            [XmlElement("vs_qqid")]
            public string fvs_qqid { get; set; }
            [XmlElement("explain")]
            public string fexplain { get; set; }
        }

        /// <summary>
        /// 提现单
        /// </summary>
        public class TXRollList
        {
            [XmlElement("state")]   //当前状态
            public string Fstate { get; set; }
            [XmlElement("state_str")]
            public string Fstate_str { get; set; }
            [XmlElement("type")]
            public string Ftype { get; set; }
            [XmlElement("type_str")]
            public string Ftype_str { get; set; }
            [XmlElement("subject")]
            public string Fsubject { get; set; }
            [XmlElement("subject_str")]
            public string Fsubject_str { get; set; }
            [XmlElement("num_str")]
            public string Fnum_str { get; set; }
            [XmlElement("sign")]
            public string Fsign { get; set; }
            [XmlElement("sign_str")]
            public string Fsign_str { get; set; }
            [XmlElement("bank_type")]
            public string Fbank_type { get; set; }
            [XmlElement("bank_type_str")]
            public string Fbank_type_str { get; set; }
            [XmlElement("abank_type")]
            public string Fabank_type { get; set; }
            [XmlElement("abank_type_str")]
            public string Fabank_type_str { get; set; }
            [XmlElement("curtype")]
            public string Fcurtype { get; set; }
            [XmlElement("curtype_str")]
            public string Fcurtype_str { get; set; }

            [XmlElement("modify_time")]
            public string Fmodify_time { get; set; }
            [XmlElement("trade_id")]    //流水ID
            public string Ftde_id { get; set; }
            [XmlElement("listid")]
            public string Flistid { get; set; }
            [XmlElement("bankid")]
            public string Fbankid { get; set; }
            [XmlElement("bank_list")]
            public string Fbank_list { get; set; }
            [XmlElement("bank_acc")]
            public string Fbank_acc { get; set; }
            [XmlElement("uid")]
            public string Fuid { get; set; }
            [XmlElement("aid")]
            public string Faid { get; set; }
            [XmlElement("abankid")]
            public string Fabankid { get; set; }
            [XmlElement("acc_name")]
            public string Facc_name { get; set; }
            [XmlElement("prove")]
            public string Fprove { get; set; }

            [XmlElement("ip")]
            public string Fip { get; set; }
            [XmlElement("memo")]
            public string Fmemo { get; set; }
            [XmlElement("pay_front_time_acc")]
            public string Fpay_front_time_acc { get; set; }
            [XmlElement("pay_front_time")]
            public string Fpay_front_time { get; set; }
            [XmlElement("pay_time")]
            public string Fpay_time { get; set; }
        }

        /// <summary>
        /// 退款单
        /// </summary>
        public class TKRollList
        {
            [XmlElement("pay_type")]
            public string Fpay_type { get; set; }
            [XmlElement("pay_type_str")]
            public string Fpay_type_str { get; set; }
            [XmlElement("buy_bank_type")]
            public string Fbuy_bank_type { get; set; }
            [XmlElement("buy_bank_type_str")]
            public string Fbuy_bank_type_str { get; set; }
            [XmlElement("sale_bank_type")]
            public string Fsale_bank_type { get; set; }
            [XmlElement("sale_bank_type_str")]
            public string Fsale_bank_type_str { get; set; }
            [XmlElement("sale_bankid")]
            public string Fsale_bankid { get; set; }          
            [XmlElement("state")]
            public string Fstate { get; set; }
            [XmlElement("state_str")]
            public string Fstate_str { get; set; }
            [XmlElement("lstate")]
            public string Flstate { get; set; }
            [XmlElement("lstate_str")]
            public string Flstate_str { get; set; }
            [XmlElement("paybuy_str")]
            public string Fpaybuy_str { get; set; }
            [XmlElement("paysale_str")]
            public string Fpaysale_str { get; set; }
            [XmlElement("procedure_str")]
            public string Fprocedure_str { get; set; }

            [XmlElement("rlistid")]
            public string Frlistid { get; set; }
            [XmlElement("listid")]
            public string Flistid { get; set; }
            [XmlElement("spid")]
            public string Fspid { get; set; }
            [XmlElement("buy_uid")]
            public string Fbuy_uid { get; set; }
            [XmlElement("buyid")]
            public string Fbuyid { get; set; }
            [XmlElement("buy_name")]
            public string Fbuy_name { get; set; }
            [XmlElement("buy_bankid")]
            public string Fbuy_bankid { get; set; }
            [XmlElement("sale_uid")]
            public string Fsale_uid { get; set; }
            [XmlElement("saleid")]
            public string Fsaleid { get; set; }
            [XmlElement("sale_name")]
            public string Fsale_name { get; set; }

            [XmlElement("bargain_time")]
            public string Fbargain_time { get; set; }
            [XmlElement("create_time")]
            public string Fcreate_time { get; set; }
            [XmlElement("ok_time")]
            public string Fok_time { get; set; }
            [XmlElement("memo")]
            public string Fmemo { get; set; }
            [XmlElement("ip")]
            public string Fip { get; set; }
            [XmlElement("explain")]
            public string Fexplain { get; set; }
            [XmlElement("ok_time_acc")]
            public string Fok_time_acc { get; set; }
            [XmlElement("modify_time")]
            public string Fmodify_time { get; set; }
        }
    }
}