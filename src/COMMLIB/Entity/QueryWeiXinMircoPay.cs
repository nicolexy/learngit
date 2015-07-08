using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace commLib.Entity
{
    /// <summary>
    /// 微信境外小额支付查询返回值对象
    /// 注释加*好的表示不确定
    /// </summary>
    [Serializable]
    public class Fmicro_Order_Result
    {
        public string appid { get; set; }
        public string attach { get; set; }
        //*银行类型
        public string bank_type { get; set; }
        public string body { get; set; }
        public string buy_uid { get; set; }
        //买家帐号
        public string buyid { get; set; }
        //*现金卷(外币)
        public string cash_fee_fc { get; set; }
        //*现金卷(人民币)
        public string cash_fee_rmb { get; set; }
        //*现金卷类型
        public string cash_fee_type { get; set; }
        // 财付通订单号
        public string cftlistid { get; set; }
        //*购物劵(外币)
        public string coupon_fee_fc { get; set; }
        //*购物劵(人民币)
        public string coupon_fee_rmb { get; set; }
        public string cre_num { get; set; }
        public string cre_type { get; set; }
        //创建日期
        public string create_time { get; set; }
        //外币类型：
        public string currency_type { get; set; }
        //*详细
        public string detail { get; set; }
        //*设配信息
        public string device_info { get; set; }
        //*商品标记
        public string goods_tag { get; set; }
        //传入币种类型
        public string input_fc_rmb { get; set; }
        //是否订阅
        public string is_subscribe { get; set; }
        //微信订单号
        public string listid { get; set; }
        //手机号
        public string mobile { get; set; }
        //*修改时间
        public string modify_time { get; set; }
        //买家姓名
        public string name { get; set; }
        public string openid { get; set; }
        //商户订单号
        public string out_trade_no { get; set; }
        //交易汇率时间
        public string rate_time { get; set; }
        //*商户创建IP地址
        public string spbill_create_ip { get; set; }
        //商户号
        public string spid { get; set; }
        //*终止时间
        public string time_expire { get; set; }
        //*开始时间
        public string time_start { get; set; }
        //交易金额(外币)
        public string total_fee_fc { get; set; }
        //交易金额（人民币）
        public string total_fee_rmb { get; set; }
        //*交易类型
        public string trade_type { get; set; }
        //交易汇率
        public string trans_rate { get; set; }
        //交易状态
        public string trade_state { get; set; }
        //失败原因
        public string trade_state_desc { get; set; } //在订单 支付失败 的情况下才有内容 
    }

}
