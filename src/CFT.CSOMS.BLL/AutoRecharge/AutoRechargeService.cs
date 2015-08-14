using CFT.CSOMS.DAL.AutoRechargeModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.BLL.AutoRecharge
{
    public class AutoRechargeService
    {
        /// <summary>
        /// 自动充值签约查询
        /// </summary>
        public DataSet QueryAutomaticRecharge(string uin, int limStart, int limMax)
        {
            DataSet ds = new AutoRechargeData().QueryAutomaticRecharge(uin, limStart, limMax);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("threshold_amount_str", typeof(String));//最低额度
                ds.Tables[0].Columns.Add("bankType", typeof(String));//扣款方式

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["threshold_amount_str"] = !string.IsNullOrEmpty(dr["threshold_amount"].ToString()) ? MoneyTransfer.FenToYuan(dr["threshold_amount"].ToString()) : "";
                    dr["bankType"] = "未知";
                    if (dr["sign_state"].ToString() == "2")
                    {
                        //扣款方式查询 
                        DataSet dsKK = GetBankTypeKK(dr["withhold_uin"].ToString(), dr["withhold_uid"].ToString());
                        if (dsKK != null && dsKK.Tables.Count > 0 && dsKK.Tables[0].Rows.Count > 0)
                        {
                            string Fbank_type = dsKK.Tables[0].Rows[0]["Fbank_type"].ToString();
                            dr["bankType"] = TransferMeaning.Transfer.convertbankType(Fbank_type);
                        }
                    }
                }

            }

            return ds;
        }

        /// <summary>
        /// 自动充值交易单查询
        /// </summary>
        public DataSet QueryAutomaticRechargeBillList(string uin, string plan_id, int limStart, int limMax)
        {
            DataSet ds = new AutoRechargeData().QueryAutomaticRechargeBillList(uin, plan_id, limStart, limMax);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("bill_amount_str", typeof(String));//充值金额
                ds.Tables[0].Columns.Add("pay_amount_str", typeof(String));//支付金额
                ds.Tables[0].Columns.Add("FstateName", typeof(String));//交易状态
                ds.Tables[0].Columns.Add("trans_id_url", typeof(String));//订单号

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["bill_amount_str"] = !string.IsNullOrEmpty(dr["bill_amount"].ToString()) ? MoneyTransfer.FenToYuan(dr["bill_amount"].ToString()) : "";
                    dr["pay_amount_str"] = !string.IsNullOrEmpty(dr["pay_amount"].ToString()) ? MoneyTransfer.FenToYuan(dr["pay_amount"].ToString()) : "";
                    string state = dr["bill_status"].ToString();
                    switch (state)
                    {
                        case "0":
                            dr["FstateName"] = "查询欠费成功";
                            break;
                        case "1":
                            dr["FstateName"] = "生成交易单成功";
                            break;
                        case "2":
                            dr["FstateName"] = "支付成功";
                            break;
                        case "3":
                            dr["FstateName"] = "销帐中(通知商户销帐前设置)";
                            break;
                        case "4":
                            dr["FstateName"] = "销帐成功";
                            break;
                        case "5":
                            dr["FstateName"] = "销帐失败";
                            break;
                        case "6":
                            dr["FstateName"] = "退款成功";
                            break;
                        case "7":
                            dr["FstateName"] = "交易关闭";
                            break;
                    }

                    string transId = dr["trans_id"].ToString();
                    dr["trans_id_url"] = "<a href=" + "../TradeManage/TradeLogQuery.aspx?id=" + transId + " target=_blank >" + transId + "</a>";
                }
            }

            return ds;
        }

        /// <summary>
        /// 自动充值扣款方式查询
        /// </summary>
        public DataSet GetBankTypeKK(string uin, string uid)
        {
            return new AutoRechargeData().GetBankTypeKK(uin, uid);
        }
    }
}
