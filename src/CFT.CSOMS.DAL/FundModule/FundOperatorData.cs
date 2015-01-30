using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CFT.CSOMS.DAL.Infrastructure;
using System.Collections;
using CFT.Apollo.Logging;

namespace CFT.CSOMS.DAL.FundModule
{
    public class FundOperatorData
    {
        public DataTable QueryContractMachineDetail(string listid) 
        {
            if (string.IsNullOrEmpty(listid)) 
            {
                throw new ArgumentNullException("必填参数：单号为空！");
            }

            StringBuilder req = new StringBuilder();
            req.Append("flag=1");
            req.Append("&listid="+listid);

            string sReply;
            short iResult;
            string sMsg;
            string serviceName = "fm_qryuserorder_service";
            bool isRet = PublicRes.CommQuery(serviceName, req.ToString(), false, out sReply, out iResult, out sMsg);
            //这里还要写日志
            if (isRet)
            {
                if (iResult == 0)
                {
                    DataSet ds = ParseContractMachine(sReply);
                    if (ds != null && ds.Tables.Count > 0) 
                    {
                        return ds.Tables[0];
                    }
                }
                else
                {
                    LogHelper.LogInfo(serviceName + " ERROR:" + sMsg);
                }
            }
            else
            {
                LogHelper.LogError(serviceName + " ERROR:" + sMsg);
            }
            return null;
        }

        private DataSet ParseContractMachine(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            DataSet dsresult = null;
            Hashtable ht = null;
            string[] strlist1 = str.Split('&');
            ht = new Hashtable(strlist1.Length);

            foreach (string strtmp in strlist1)
            {
                string[] strlist2 = strtmp.Split('=');
                if (strlist2.Length != 2)
                {
                    continue;
                }

                ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
            }
            if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0")
            {
                return null;
            }
            int count = int.Parse(ht["count"].ToString());
            if (count > 0)
            {
                dsresult = new DataSet();
                DataTable dt = new DataTable();
                dsresult.Tables.Add(dt);

                dt.Columns.Add("listid");
                dt.Columns.Add("chanid");
                dt.Columns.Add("state");
                dt.Columns.Add("sign");
                dt.Columns.Add("uin");
                dt.Columns.Add("spid");
                dt.Columns.Add("sp_billno");
                dt.Columns.Add("total_fee");
                dt.Columns.Add("create_time");
                dt.Columns.Add("freeze_time");
                dt.Columns.Add("freeze_list");
                dt.Columns.Add("mcode");
                dt.Columns.Add("plan_code");
                dt.Columns.Add("desc");
                dt.Columns.Add("memo");
                dt.Columns.Add("bind_user_name");
                dt.Columns.Add("bind_mobile");
                dt.Columns.Add("bind_cre_type");
                dt.Columns.Add("bind_cre_id");
                dt.Columns.Add("express");
                dt.Columns.Add("exp_ticket");
                dt.Columns.Add("sp_name");
                dt.Columns.Add("act_time");
                dt.Columns.Add("expire_time");
                dt.Columns.Add("fund_spid");
                dt.Columns.Add("fund_code");
                dt.Columns.Add("mtype");
                dt.Columns.Add("period");
                dt.Columns.Add("unfreeze_time");
                dt.Columns.Add("cnee_name");
                dt.Columns.Add("cnee_mobile");
                dt.Columns.Add("cnee_address");
                dt.Columns.Add("post_code");

                for (int i = 0; i < count; i++)
                {
                    DataRow drfield = dt.NewRow();
                    drfield.BeginEdit();

                    drfield["listid"] = ht["listid_" + i].ToString();
                    drfield["chanid"] = ht["chanid_" + i].ToString();
                    drfield["state"] = ht["state_" + i].ToString();
                    drfield["sign"] = ht["sign_" + i].ToString();
                    drfield["uin"] = ht["uin_" + i].ToString();
                    drfield["spid"] = ht["spid_" + i].ToString();
                    drfield["sp_billno"] = ht["sp_billno_" + i].ToString();
                    drfield["total_fee"] = ht["total_fee_" + i].ToString();
                    drfield["create_time"] = ht["create_time_" + i].ToString();
                    drfield["freeze_time"] = ht["freeze_time_" + i].ToString();
                    drfield["freeze_list"] = ht["freeze_list_" + i].ToString();

                    drfield["mcode"] = ht["mcode_" + i].ToString();
                    drfield["plan_code"] = ht["plan_code_" + i].ToString();
                    drfield["desc"] = ht["desc_" + i].ToString();
                    drfield["memo"] = ht["memo_" + i].ToString();
                    drfield["bind_user_name"] = ht["bind_user_name_" + i].ToString();
                    drfield["bind_mobile"] = ht["bind_mobile_" + i].ToString();
                    drfield["bind_cre_type"] = ht["bind_cre_type_" + i].ToString();
                    drfield["bind_cre_id"] = ht["bind_cre_id_" + i].ToString();
                    drfield["express"] = ht["express_" + i].ToString();
                    drfield["exp_ticket"] = ht["exp_ticket_" + i].ToString();

                    drfield["sp_name"] = ht["sp_name_" + i].ToString();
                    drfield["act_time"] = ht["act_time_" + i].ToString();
                    drfield["expire_time"] = ht["expire_time_" + i].ToString();
                    drfield["fund_spid"] = ht["fund_spid_" + i].ToString();
                    drfield["fund_code"] = ht["fund_code_" + i].ToString();
                    drfield["mtype"] = ht["mtype_" + i].ToString();
                    drfield["period"] = ht["period_" + i].ToString();
                    drfield["unfreeze_time"] = ht["unfreeze_time_" + i].ToString();
                    drfield["cnee_name"] = ht["cnee_name_" + i].ToString();
                    drfield["cnee_mobile"] = ht["cnee_mobile_" + i].ToString();
                    drfield["cnee_address"] = ht["cnee_address_" + i].ToString();
                    drfield["post_code"] = ht["post_code_" + i].ToString();

                    drfield.EndEdit();
                    dt.Rows.Add(drfield);
                }
            }
            return dsresult;
        }

        public DataSet QueryPhoneDetail(string spid, string phone) 
        {
            if (string.IsNullOrEmpty(phone))
            {
                throw new ArgumentNullException("必填参数：phone为空！");
            }

            string sql = "SELECT Farea,Fstate FROM fund_mobile_db.t_phone_no WHERE Fphone_no='"+phone+"' ";

            if (!string.IsNullOrEmpty(spid))
            {
                sql += " AND Fspid='" + spid + "'";
            }

            using (var da = MySQLAccessFactory.GetMySQLAccess("FundPhoneNo"))
            {
                da.OpenConn();

                DataSet ds = da.dsGetTotalData(sql);

                return ds;
            }
        }

        /// <summary>
        /// 通过uin查询订单号
        /// </summary>
        /// <param name="uin"></param>
        /// <returns></returns>
        public DataSet QueryListidFromUin(string uin)
        {
            if (string.IsNullOrEmpty(uin))
            {
                throw new ArgumentNullException("必填参数：uin为空！");
            }

            string sql = "SELECT Flistid FROM fund_mobile_db.t_phone_order WHERE Fuin='" + uin + "' ";

            
            using (var da = MySQLAccessFactory.GetMySQLAccess("FundPhoneNo"))
            {
                da.OpenConn();

                DataSet ds = da.dsGetTotalData(sql);
                return ds;
            }
        }

    }
}
