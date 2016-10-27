using CFT.Apollo.Bow.Recharge;
using CFT.Apollo.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.DAL.TradeModule
{
  public class RechargeData
    {

        /// <summary>
        /// 查询充值单
        /// </summary>
        /// <param name="listid">充值单ID</param>
        /// <param name="bankListid">银行订单号</param>
        /// <param name="bankType">对方银行类型（工商银行，交通银行）</param>
        /// <param name="curtype">币种</param>
        /// <returns></returns>
        public RechargeItem GetRechargeItem(string listid, string bankListid, int bankType, int curtype)
        {
            var rechargeModel = new RechargeItem();

            try
            {
                CFT.Apollo.Bow.Recharge.RechargeRepository recharge = new RechargeRepository();
                rechargeModel = recharge.GetRechargeItem(listid, bankListid, bankType, curtype);
            }
            catch (Exception ex)
            {
                LogHelper.LogError("CFT.CSOMS.DAL.TradeModule.RechargeData  public RechargeItem GetRechargeItem(string listid, string bankListid, int bankType, int curtype), ERROR:" + ex.ToString());
            }

            return rechargeModel;
        }
    }
}
