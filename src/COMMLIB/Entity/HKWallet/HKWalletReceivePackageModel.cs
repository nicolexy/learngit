using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/** ==============================================================================
* Created: 1/18/2016 5:44:39 PM
* Author: v_swuzhang
* Description: Hk钱包支付功能 -- 收红包信息记录
* ==============================================================================*/
namespace commLib.Entity
{
   public class HKWalletReceivePackageModel
    {
        public string hb_type{get;set;}
         public string send_name{get;set;}
         public string send_account{get;set;}
         public string send_listid{get;set;}
         public string pay_time{get;set;}
         public string total_amount{get;set;}
         public string recv_listid{get;set;}
         public string recv_time{get;set;}
         public string account_time{get;set;}
         public string recv_amount { get; set; }
    }
}
