using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.Models
{
    /// <summary>
    /// 用户充值卡充值返回结果
    /// </summary>
    public class UserMobileRechargeOrderResult
    {
        /// <summary>
        /// 错误码 0成功，其他失败
        /// </summary>
        public int retCode { get; set; }
        /// <summary>
        /// 错误描述，一般为空
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 订单总数
        /// </summary>
        public int total { get; set; }
        public UserMobileRechargeOrderRows data { get; set; }
    }

    public class UserMobileRechargeOrderRows
    {
        public List<UserMobileRechargeOrderModel> rows { get; set; }
    }

    /// <summary>
    /// 用户充值卡充值数据
    /// </summary>
    public class UserMobileRechargeOrderModel
    {
        /// <summary>
        /// 买家uin
        /// </summary>
        public int buyerUin { get; set; }
        /// <summary>
        /// 面值（用户输入，选择的面值，参考）单位分
        /// </summary>
        public int faceValue { get; set; }
        /// <summary>
        /// 销卡金额（卡号真实金额）单位分
        /// </summary>
        public int payFee { get; set; }
        /// <summary>
        /// 销卡时间
        /// </summary>
        public string recoverTime { get; set; }
        /// <summary>
        /// 充值卡卡号
        /// </summary>
        public string cardNo { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 业务订单ID
        /// </summary>
        public string dealId { get; set; }
        /// <summary>
        /// 销卡结果描述
        /// </summary>
        public string msg { get; set; }
    }
}
