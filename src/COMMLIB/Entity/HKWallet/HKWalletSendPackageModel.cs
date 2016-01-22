using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/** ==============================================================================
* Created: 1/18/2016 5:44:50 PM
* Author: v_swuzhang
* Description: Hk钱包支付功能 -- 发红包信息记录
* ==============================================================================*/
namespace commLib.Entity
{
   public class HKWalletSendPackageModel
    {
        public string hb_type{get;set;}
         public string send_name{get;set;}
         public string send_listid{get;set;} 
         public string create_time{get;set;}
         public string pay_time{get;set;}
         public string pay_state{get;set;}
         public string pay_means{get;set;}
         public string card_type{get;set;}
         public string card_num{get;set;}
         public string total_amount{get;set;}
         public string total_num{get;set;}
         public string recv_amount{get;set;}
         public string received_num{ get; set; }
         public string fee_amount{get;set;}
         public string refund_amount{get;set;}
         public string refund_listid{get;set;}
         public string refund_time { get; set; }


    }
}
