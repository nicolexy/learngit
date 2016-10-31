using CFT.Apollo.Bow.Recharge;
using CFT.Apollo.Bow.Extend;
using CFT.CSOMS.DAL.TradeModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CFT.CSOMS.BLL.TradeModule
{
   public class RechargeService
    {
        /// <summary>
        /// 查询充值单
        /// </summary>
        /// <param name="listid">充值单ID</param>
        /// <param name="bankListid">银行订单号</param>
        /// <param name="bankType">对方银行类型（工商银行，交通银行）</param>
        /// <param name="curtype">币种</param>
        /// <returns></returns>
        public RechargeItem GetRecharge(string listid, string bankListid, int bankType, int curtype)
        {
            var rechargeModel = new RechargeData();

            return rechargeModel.GetRechargeItem(listid, bankListid, bankType, curtype);
        }


        /// <summary>
        /// 查询充值单
        /// </summary>
        /// <param name="listid">充值单ID</param>
        /// <param name="bankListid">银行订单号</param>
        /// <param name="bankType">对方银行类型（工商银行，交通银行）</param>
        /// <param name="curtype">币种</param>
        /// <returns></returns>
        public DataTable GetRechargeTable(string listid, string bankListid, int bankType, int curtype)
        {
            var rechargeModel = new RechargeData();

            var dt = rechargeModel.GetRechargeItem(listid, bankListid, bankType, curtype).ToDataTable();
            
            return dt;
        }
    }
}
