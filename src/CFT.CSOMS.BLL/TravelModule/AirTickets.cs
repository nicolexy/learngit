using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace CFT.CSOMS.BLL.TravelModule
{
    public class AirTickets
    {
        private DAL.TravelModule.AirTickets Dal = new DAL.TravelModule.AirTickets();
        private static Dictionary<string, int> query_typeInfo;
        static AirTickets()
        {
            query_typeInfo = new Dictionary<string, int>();
            query_typeInfo.Add("TextSppreno", 1);
            query_typeInfo.Add("TextTransaction_id", 2);
            query_typeInfo.Add("TextInsur_no", 3);
            //   query_typeInfo.Add("", 4);
            query_typeInfo.Add("TextTicketno", 5);
            query_typeInfo.Add("TextCert_id", 6);
            query_typeInfo.Add("TextPassenger_name", 7);
            query_typeInfo.Add("TextMobile", 8);
            query_typeInfo.Add("TextName", 9);
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="query_type">查询类型</param>
        /// <param name="wd">关键字</param>
        /// <param name="trade_type">取值状态</param>
        /// <param name="uin">财付通账号</param>
        /// <param name="start_time">开始时间</param>
        /// <param name="end_time">结束时间</param>
        /// <param name="sp_code">Sp代码</param>
        /// <param name="limit">页大小</param>
        /// <param name="page_id">当前页</param>
        /// <returns></returns>
        public DataSet AirTicketsOrderQuery(int query_type, string wd, string trade_type, string uin, DateTime start_time, DateTime end_time, string sp_code, int limit, int page_id = 1)
        {
            var ds = Dal.AirTicketsOrderQuery(query_type, wd, trade_type, uin, start_time, end_time, sp_code, limit, page_id);
            if (ds != null && ds.Tables != null)
            {
                FenToYuan_Table(ds.Tables[0], "_str", "total_money", "adult_airport_tax", "adult_fuel_tax", "child_airport_tax", "child_fuel_tax", "adult_price", "child_price", "airport_tax_money", "fuel_tax_money", "ticket_money", "insurance_money");
            }
            return ds;

        }

        /// <summary>
        /// 通过uin获取订单信息
        /// </summary>
        /// <param name="uin">财付通账号</param>
        /// <param name="trade_type">订单状态</param>
        /// <param name="start_time">开始时间</param>
        /// <param name="end_time">结束时间</param>
        /// <param name="sp_code">Sp代码</param>
        /// <param name="limit">页大小</param>
        /// <param name="page_id">当前页</param>
        /// <returns></returns>
        public DataSet AirTicketsOrderQueryByUin(string uin, string trade_type, DateTime start_time, DateTime end_time, string sp_code, int limit, int page_id = 1)
        {
            var ds = Dal.AirTicketsOrderQueryByUin(uin, trade_type, start_time, end_time, sp_code, limit, page_id);
            if (ds != null && ds.Tables != null)
            {
                FenToYuan_Table(ds.Tables[0], "_str", "total_money", "adult_airport_tax", "adult_fuel_tax", "child_airport_tax", "child_fuel_tax", "adult_price", "child_price", "airport_tax_money", "fuel_tax_money", "ticket_money", "insurance_money");
            }
            return ds;
        }

        /// <summary>
        /// 获取查询条件和查询类型
        /// </summary>
        /// <param name="query_type">查询类型</param>
        /// <param name="txtBox">查询条件的TextBox控件对象数组</param>
        /// <returns></returns>
        public string GetKeyWordAndType(out int query_type, params TextBox[] txtBox)
        {
            foreach (var item in txtBox)
            {
                var txt = item.Text.Trim();
                if (!string.IsNullOrEmpty(txt))
                {
                    if (query_typeInfo.ContainsKey(item.ID))
                    {
                        query_type = query_typeInfo[item.ID];
                        return txt;
                    }
                }
            }
            query_type = 0;
            return string.Empty;
        }

        /// <summary>
        /// RMB分转元
        /// </summary>
        /// <param name="dt">表对象</param>
        /// <param name="suffix">元字段加入的后缀</param>
        /// <param name="column">要转换的分字段</param>
        public void FenToYuan_Table(DataTable dt, string suffix, params string[] column)
        {
            var columns = new DataColumn[column.Length];
            for (int i = 0; i < column.Length; i++)
            {
                var col = column[i];
                if (!dt.Columns.Contains(col))
                {
                    throw new Exception("dt中不存在 '" + col + " '这个字段");
                }
                columns[i] = new DataColumn(col + suffix, typeof(string));
            }
            dt.Columns.AddRange(columns);
            foreach (DataRow item in dt.Rows)
            {
                item.BeginEdit();
                foreach (var cName in column)
                {
                    var fen = item[cName].ToString();
                    item[cName + suffix] = TENCENT.OSS.CFT.KF.Common.MoneyTransfer.FenToYuan(fen);
                }
                item.EndEdit();
            }
        }

    }
}
