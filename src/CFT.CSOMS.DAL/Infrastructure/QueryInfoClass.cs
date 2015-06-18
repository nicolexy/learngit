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

    #region	 受控资金信息查询

    public class QeuryUserControledFinInfoClass : Query_BaseForNET
    {
        public QeuryUserControledFinInfoClass(string fuid, string beginDateStr, string endDateStr, string cur_type, int iNumStart, int iNumMax)
        {
            this.ICEcommand = "FINANCE_QUERY_USER_CONTROLED";
            this.ICESql = "uid=" + fuid;

            if (beginDateStr != null && beginDateStr.Trim() != "" && endDateStr != null && endDateStr.Trim() != "")
            {
                this.ICESql += "&createTime_begin=" + beginDateStr + "&createTime_end=" + endDateStr;
            }

            if (cur_type != null && cur_type.Trim() != "")
            {
                this.ICESql += "&cur_type=" + cur_type;
            }

            this.ICESql += "&strlimit=limit " + iNumStart + "," + iNumMax;
        }
    }
    #endregion


    #region 邮储汇款查询类

    public class RemitQueryClass : Query_BaseForNET
    {
        public RemitQueryClass(string batchid, string tranType, string dataType, string remitType, string tranState, string spid, string remitRec, string listID)
        {
            string strWhere = " where 1=1 ";

            if (batchid != null && batchid != "")
            {
                strWhere += " and Fbatchid='" + batchid + "'";
            }

            if (spid != null && spid != "")
            {
                strWhere += " and Fspid='" + spid + "'";
            }

            if (tranType != "99")
            {
                strWhere += " and Ftran_type=" + tranType;
            }

            if (dataType != "99")
            {
                strWhere += " and Fdata_type=" + dataType;
            }

            if (remitType != "99")
            {
                strWhere += " and Fremit_type=" + remitType;
            }

            if (tranState != "99")
            {
                strWhere += " and Ftran_state='" + tranState + "'";
            }

            if (listID != null && listID.Trim() != "")
            {
                strWhere += " and Flistid='" + listID.Trim() + "' ";
            }

            if (remitRec != null && remitRec.Trim() != "")
            {
                strWhere += " and Fremit_rec='" + remitRec.Trim() + "' ";
            }

            fstrSql = "select * from c2c_zwdb.t_remit_order " + strWhere;
            fstrSql_count = "select count(*) from c2c_zwdb.t_remit_order " + strWhere;
        }
    }

    public class QueryRemitStateInfo : Query_BaseForNET
    {
        public QueryRemitStateInfo(string Ford_date, string Ford_ssn)
        {
            string strWhere = " where Ford_date='" + Ford_date + "' and Ford_ssn='" + Ford_ssn + "'";

            fstrSql = "select * from c2c_db_remit.t_remit_list " + strWhere;
        }
    }

    #endregion

}
