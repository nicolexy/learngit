using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CFT.CSOMS.DAL.RefundModule;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace CFT.CSOMS.BLL.RefundModule
{
    public class RefundRegisterService
    {
        /// <summary>
        /// 查询退款登记记录
        /// </summary>
        /// <param name="coding"></param>
        /// <param name="orderId"></param>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <param name="refundType"></param>
        /// <param name="refundState"></param>
        /// <param name="tradeState"></param>
        /// <param name="iPageStart"></param>
        /// <param name="iPageMax"></param>
        /// <returns></returns>
        public DataSet QueryRefundRegisterList(string coding, string orderId, string stime, string etime, int refundType, int refundState, string tradeState, int iPageStart, int iPageMax) 
        {
            DataSet newDs = null;
            DataSet ds = new RefundRegisterData().QueryRefundRegisterList(coding, orderId, stime, etime, refundType, refundState, "", iPageStart, iPageMax);

            if (ds != null && ds.Tables.Count > 0) 
            {
                string msg = "";
                ds.Tables[0].Columns.Add("Ftrade_state_new", typeof(String));
                ds.Tables[0].Columns.Add("FTrade_Type", typeof(String));

                newDs = new DataSet();
                DataTable res_dt = new DataTable();
                newDs.Tables.Add(res_dt);
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    res_dt.Columns.Add(ds.Tables[0].Columns[i].ColumnName);
                }

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    msg = "";
                    string striceWhere = "listid=" + dr["Forder_id"].ToString();
                    DataTable dt_ice = CommQuery.GetTableFromICE(striceWhere, "query_order_service", out msg);
                    if (msg != "")
                    {

                    }
                    if (dt_ice != null && dt_ice.Rows.Count > 0)
                    {
                        dr["FTrade_Type"] = dt_ice.Rows[0]["Ftrade_type"].ToString();
                        object obj = dt_ice.Rows[0]["Ftrade_state"];
                        //通过交易状态查询条件，来过滤记录
                        if (obj != null)
                        {
                            string state = obj.ToString().Trim();
                            dr["Ftrade_state_new"] = state;
                            if (!string.IsNullOrEmpty(tradeState) && tradeState != "0")
                            {
                                if (state == tradeState)
                                {
                                    //如果状态相同，表示是需要查询的记录
                                    res_dt.ImportRow(dr);
                                }
                            }
                            else
                            {
                                res_dt.ImportRow(dr);
                            }
                        }
                    }
                }    
            }

            return newDs;
        }

        public int QueryRefundRegisterCount(string coding, string orderId, string stime, string etime, int refundType, int refundState, string tradeState) 
        {
            return new RefundRegisterData().QueryRefundRegisterCount(coding, orderId, stime, etime, refundType, refundState, tradeState);
        }

        public string QueryTradeAmount(string listid) 
        {
            return new RefundRegisterData().QueryTradeAmount(listid);
        }
    }
}
