using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TENCENT.OSS.CFT.KF.Common;

namespace commLib.Entity.HKWallet
{
    public class HKWalletTransRollList
    {
        public string trans_listid { get; set; }
        public string pay_nickname { get; set; }
        public string recv_nickname { get; set; }
        public string amount { get; set; }
        public string fee { get; set; }
        public string pay_state { get; set; }
        //public string pay_time { get; set; }
        public string card_last4No { get; set; }
        public string create_time { get; set; }
        public string pay_time { get; set; }
        public string recv_time { get; set; }
        public string inaccount_time { get; set; }
        public string refund_time { get; set; }

        public string amount_str
        {
            get;
            set;
        }
        public string fee_str
        {
            get;
            set;
        }

        public string pay_state_str
        {
            get
            {
                return pay_state == "1" ? "初始" :
                    pay_state == "2" ? "付款方B2C支付完成" :
                    pay_state == "3" ? "付款方B2C支付失败" :
                    pay_state == "4" ? "收款方确认收款" :
                    pay_state == "5" ? "收款方B2C转账完成" :
                    pay_state == "6" ? "付款方B2C退款中" :
                    pay_state == "7" ? "付款方B2C退款成功(最终状态)" :
                    pay_state == "8" ? "付款方B2C退款失败(最终状态,此时需要人工介入)" :
                    pay_state == "9" ? "收款方退还	" : "未知:" + pay_state;
            }
            set { }
        }
    }
}
