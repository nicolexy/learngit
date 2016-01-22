using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/** ==============================================================================
* Created: 1/18/2016 5:50:16 PM
* Author: v_swuzhang
* Description: HK钱包支付接口
* ==============================================================================*/
namespace commLib.Entity
{

    public class HKWalletrecvModel<T> where T : class
    {
        public List<T> recv_hb_list { get; set; }
        public int ret_num { get; set; }
    }

   public class HKWalletSendModel<T> where T : class
   {
       public List<T> send_hb_list { get; set; }
       public int ret_num { get; set; }
   }
}
