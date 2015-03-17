using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.DAL.Infrastructure
{
    /// <summary>
    /// 交易单表的类形式
    /// </summary>
    public class T_PAY_LIST
    {
        public string u_ListID;
        public string u_Coding;					//订单编码
        public string u_SPID;					//机构代码名称（发起者）
        public string u_Bank_ListID;			//给银行的订单号
        public string u_Pay_Type;					//支付类型
        public string u_BuyID;					//买家帐户号码
        public string u_Buy_Name;				//付款方名称
        public string u_Buy_Bank_Type;				//买家开户行
        public string u_Buy_BankID;				//买家的银行帐号
        public string u_SaleID;					//卖家帐户号码
        public string u_Sale_Name;				//卖家的名称
        public string u_CurType;					//币种代码
        public string u_State;						//交易状态
        public string u_LState;					//交易单状态
        public string u_Price;						//产品的价格
        public string u_Carriage;					//物流费用
        public string u_PayNum;					//应支付的总价格
        public string u_Fact;						//实际支付费用
        public string u_Procedure;					//交易（服务）手续费
        public string u_Service;					//服务费率
        public string u_Cash;						//现金支付金额
        public string u_Create_Time_C2C;		//定单创建时间（c2c）
        public string u_Create_Time;			//定单创建时间（本地系统时间）
        public string u_Bargain_Time;			//买家付款时间(bank)
        public string u_Receive_Time_C2C;		//打款给卖家时间(c2c)
        public string u_Receive_Time;			//打款给卖家时间（本地）
        public string u_IP;						//最后修改交易单的IP
        public string u_Memo;					//交易说明
        public string u_Explain;				//备注（后台人员操作记录）
        public string u_Modify_Time;			//最后修改时间（c2c）/ 本地系统时间

        public string u_Pay_Time;
    }
}
