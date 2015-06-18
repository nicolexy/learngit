using CFT.CSOMS.DAL.InternationalityPayModule;
using commLib.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace CFT.CSOMS.BLL.InternationalityPayModule
{
    public class QueryWeiXinMircoPay
    {
        CFT.CSOMS.DAL.InternationalityPayModule.QueryWeiXinMircoPay dal = new DAL.InternationalityPayModule.QueryWeiXinMircoPay();
        public static readonly Dictionary<string, string> PayState = new Dictionary<string, string>(); //前台页面需要用于生成下拉框
        static QueryWeiXinMircoPay()
        {
            PayState.Add("0", "开始下单");
            PayState.Add("1", "支付成功");
            PayState.Add("2", "己退款");
            PayState.Add("3", "未支付");
            PayState.Add("4", "订单关闭");
            PayState.Add("5", "订单撤销");
            PayState.Add("6", "用户支付中");
            PayState.Add("7", " 未支付");
            PayState.Add("8", " 支付失败");
        }
        /// <summary>
        /// 通过条件查询境外微信小额支付信息
        /// </summary>
        /// <param name="spid">商户号</param>
        /// <param name="out_trade_no">商户订单号</param>
        /// <param name="listid">微信订单号</param>
        /// <param name="cftlistid">财付通订单号</param>
        /// <param name="mobile">用户手机号码</param>
        /// <param name="name">用户名称</param>
        /// <param name="fromtime">查询开始时间</param>
        /// <param name="totime">查询结束时间</param>
        /// <param name="pagenum">页码</param>
        /// <param name="limit">页大小</param> 
        /// <param name="trade_state">交易状态</param>
        /// <returns></returns>
        public List<Fmicro_Order_Result> FMicroOrderList(string spid, string out_trade_no, string listid, string cftlistid, string mobile, string name, DateTime fromtime, DateTime totime, int pagenum, int limit, string trade_state)
        {
            var obj = dal.FMicroOrderList(spid, out_trade_no, listid, cftlistid, mobile, name, fromtime, totime, pagenum, limit, trade_state);
            return SwitchValueCode(obj);
        }


        /// <summary>
        /// 把支付信息中的一些代码进行对应的转换
        /// </summary>
        /// <param name="list"></param>
        private List<Fmicro_Order_Result> SwitchValueCode(List<Fmicro_Order_Result> list)
        {
            if (list != null)
            {
                list.ForEach(u =>
                {
                    u.total_fee_fc = TENCENT.OSS.CFT.KF.Common.MoneyTransfer.FenToYuan(u.total_fee_fc);
                    u.total_fee_rmb = TENCENT.OSS.CFT.KF.Common.MoneyTransfer.FenToYuan(u.total_fee_rmb);
                    u.input_fc_rmb = u.input_fc_rmb == "1" ? "外币" : "人民币";
                    u.trade_state = PayState.ContainsKey(u.trade_state ?? "") ? PayState[u.trade_state] : "其他(" + u.trade_state + ")";
                    u.trans_rate = string.IsNullOrEmpty(u.trans_rate) ? u.trans_rate : (double.Parse(u.trans_rate) / Math.Pow(10, 8)).ToString();
                });
            }
            return list;
        }
    }

}
