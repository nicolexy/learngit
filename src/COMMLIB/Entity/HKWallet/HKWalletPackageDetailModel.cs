using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/** ==============================================================================
* Created: 1/19/2016 10:58:18 AM
* Author: v_swuzhang
* Description: 获取红包详情信息
* ==============================================================================*/
namespace commLib.Entity
{
    public class HKWalletDetailItem
    {
        public HKWalletGetDetailHBList hb_items { get; set; }
        public HKWalletSendPackageDetailHBOrder hb_order { get; set; }
    }

    /// <summary>
    /// 收红包列表
    /// </summary>
    public class HKWalletGetDetailHBList
    {
        public List<HKWalletGetPackageDetailModel> hb_list { get; set; }
        public int ret_num { get; set; }
    }
 
    /// <summary>
    /// 收红包详情
    /// </summary>
    public class HKWalletGetPackageDetailModel
    {
        public string receive_accid { get; set; }
        public string receive_name { get; set; }
        public string recv_listid { get; set; }
        public string recv_amount { get; set; }
        public string recv_time { get; set; }
        public string account_time { get; set; }
        public string memo { get; set; }
    }


    /// <summary>
    /// 发红包详情
    /// </summary>
    public class HKWalletSendPackageDetailHBOrder
    {
        public string hb_type { get; set; }
        public string send_listid { get; set; }
        public string create_time { get; set; }
        public string pay_state { get; set; }
        public string card_type { get; set; }
        public string total_amount { get; set; }
        public string total_num { get; set; }
        public string fee_amount { get; set; }
        public string refund_time { get; set; }
        public string refund_listid { get; set; }
        public string send_account { get; set; }
        public string send_name { get; set; }
        public string pay_time { get; set; }
        public string pay_means { get; set; }
        public string card_num { get; set; }
        public string received_amount { get; set; }
        public string received_num { get; set; }
        public string invalid_time { get; set; }
        public string refund_amount { get; set; }
    }
}
