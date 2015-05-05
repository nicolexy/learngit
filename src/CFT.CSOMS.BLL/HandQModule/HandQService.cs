using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CFT.CSOMS.DAL.HandQModule;
using System.Collections;
using TENCENT.OSS.C2C.Finance.BankLib;

namespace CFT.CSOMS.BLL.HandQModule
{
    public class HandQService
    {
        public DataSet QueryHandQInfor(string strUID, string strPayListID, string strBeginTime, string strEndTime, string strType, int offset, int limit, out string strOutMsg)
        {

            return new HandQDAL().QueryHandQInfor(strUID, strPayListID, strBeginTime, strEndTime, strType, offset, limit, out strOutMsg);
        }

        public DataSet RequestHandQDetail(string strSendList, int type, int offset, int limit, out string strOutMsg)
        {
            return new HandQDAL().RequestHandQDetail(strSendList, type, offset, limit, out strOutMsg);
        }

        public DataSet RefundHandQQuery(string acc,string type,string start_time, string end_time,int offset, int limit)
        {
            if (string.IsNullOrEmpty(acc))
            {
                throw new ArgumentNullException("acc为空！");
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException("type为空！");
            }
            if (string.IsNullOrEmpty(start_time))
            {
                throw new ArgumentNullException("start_time为空！");
            }
            if (string.IsNullOrEmpty(end_time))
            {
                throw new ArgumentNullException("end_time为空！");
            }
            DataSet ds = new DataSet();
            if (type == "1")
                ds = new HandQDAL().RefundHandQQuery(acc, start_time, end_time, "", "", "",  offset,  limit);
            else if (type == "2")
            {
                string XcardId = new HandQDAL().BankCardEncryptStartX(acc);
                ds = new HandQDAL().RefundHandQQuery("", start_time, end_time, "", XcardId, "", offset, limit);
            }
            else
                throw new Exception("查询类型不正确");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("state_str", typeof(String));
                ds.Tables[0].Columns.Add("num_str", typeof(String));
                ds.Tables[0].Columns.Add("isTP", typeof(String));
                ds.Tables[0].Columns.Add("bank_type_str", typeof(String));

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["state"].ToString() == "7")
                        row["isTP"]= "是";
                    else
                        row["isTP"] = "否";
                    row["bank_type_str"] = BankIO.QueryBankName(row["bank_type"].ToString());
                }
                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "等待支付");
                ht1.Add("2", "支付完成");
                ht1.Add("3", "B2C转账完成");
                ht1.Add("4", "提现发起");
                ht1.Add("5", "提现成功");
                ht1.Add("6", "提现失败");
                ht1.Add("7", "退票");
                ht1.Add("8", "退款");
                COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "state", "state_str", ht1);
                COMMLIB.CommUtil.FenToYuan_Table(ds.Tables[0], "num", "num_str");
            }
            return ds;
        }

        public DataSet RefundHandQDetailQuery(string acc,string type)
        {
            if (string.IsNullOrEmpty(acc))
            {
                throw new ArgumentNullException("acc为空！");
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException("type为空！");
            }

            DataSet ds = new DataSet();
            if (type == "3")
                ds = new HandQDAL().RefundHandQDetailQuery("", acc, "");
            else if (type == "4")
                ds = new HandQDAL().RefundHandQDetailQuery(acc, "", "");
            else
                throw new Exception("查询类型不正确");

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("state_str", typeof(String));
                ds.Tables[0].Columns.Add("num_str", typeof(String));
                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "等待支付");
                ht1.Add("2", "支付完成");
                ht1.Add("3", "B2C转账完成");
                ht1.Add("4", "提现发起");
                ht1.Add("5", "提现成功");
                ht1.Add("6", "提现失败");
                ht1.Add("7", "退票");
                ht1.Add("8", "退款");
                COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "state", "state_str", ht1);
                COMMLIB.CommUtil.FenToYuan_Table(ds.Tables[0], "num", "num_str");
            }
            return ds;
        }

        public string BankCardEncryptStartX(string card_id)
        {
            return new HandQDAL().BankCardEncryptStartX(card_id);
        }

    }
}
