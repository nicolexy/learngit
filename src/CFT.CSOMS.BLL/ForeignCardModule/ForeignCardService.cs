using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using CFT.CSOMS.DAL.ForeignCardModule;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.CSOMS.DAL.SPOA;
using CFT.CSOMS.DAL.CFTAccount;
using CFT.CSOMS.COMMLIB;
using CFT.CSOMS.BLL.TransferMeaning;

namespace CFT.CSOMS.BLL.ForeignCardModule
{
    public class ForeignCardService
    {
        //查询外卡订单
        public DataSet QueryForeignCardOrderList(string sp_uid, string spid, string sp_bill_no, string transaction_id, string email, string s_time, string e_time, string trade_state, int offset, int limit)
        {
            return new PayManageData().QueryForeignCardOrderList(sp_uid, spid,  sp_bill_no,  transaction_id,  email,  s_time,  e_time,  trade_state,  offset,  limit);
        }

        //外卡拒付列表查询
        public DataSet QueryForeignCardRefusePayList(string query_time, string spid, string coding, string list_id, string check_state, string sp_process_state, string s_time, string e_time, int offset, int limit)
        {
            return new PayManageData().QueryForeignCardRefusePayList(query_time, spid, coding, list_id, check_state, sp_process_state, s_time, e_time, offset, limit);
        }

        //查询外卡商户流水 只查C账户
        public DataSet GetForeignCardRollList(string spid, string s_time, string e_time, int offset, int limit)
        {
            string qqid = new MerchantData().GetMerchantCFuid(spid);
            string fuid = AccountData.ConvertToFuid(qqid);
            if (fuid == null || fuid == "")
            {
                throw new Exception("根据C帐号获取Fuid失败	qqid:" + qqid);
            }
            if (fuid == null || fuid.Length < 3)
            {
                throw new Exception("内部ID不正确！");
            }
            try
            {
                DataSet ds_Bankroll = new DataSet();
                //与 个人账户信息-用户资金流水 一样 fcurtype=1
                ds_Bankroll = new PayManageData().GetForeignCardRollList(fuid, 1, s_time, e_time, offset, limit);

                if (ds_Bankroll != null && ds_Bankroll.Tables.Count != 0 && ds_Bankroll.Tables[0].Rows.Count != 0)
                {
                    ds_Bankroll.Tables[0].Columns.Add("FpaynumStr", typeof(string));
                    ds_Bankroll.Tables[0].Columns.Add("FbalanceStr", typeof(string));
                    ds_Bankroll.Tables[0].Columns.Add("FconnumStr", typeof(string));
                    ds_Bankroll.Tables[0].Columns.Add("FconStr", typeof(string));

                    ds_Bankroll.Tables[0].Columns.Add("Faction_type_str", typeof(string));//动作类型
                    ds_Bankroll.Tables[0].Columns.Add("Fcurtype_str", typeof(string));//币种的类型
                    ds_Bankroll.Tables[0].Columns.Add("Ftype_str", typeof(string));//交易类型
                    ds_Bankroll.Tables[0].Columns.Add("Fsubject_str", typeof(string));//类别/科目
                    for (int i = 0; i < ds_Bankroll.Tables[0].Rows.Count; i++)
                    {
                        if (ds_Bankroll.Tables[0].Rows[i]["Fpaynum"].ToString().Trim() == "0")
                        {
                            ds_Bankroll.Tables[0].Rows[i].Delete();
                            i--;
                        }
                    }

                    CommUtil.FenToYuan_Table(ds_Bankroll.Tables[0], "Fpaynum", "FpaynumStr");
                    CommUtil.FenToYuan_Table(ds_Bankroll.Tables[0], "Fbalance", "FbalanceStr");
                    CommUtil.FenToYuan_Table(ds_Bankroll.Tables[0], "Fconnum", "FconnumStr");
                    CommUtil.FenToYuan_Table(ds_Bankroll.Tables[0], "Fcon", "FconStr");
                    foreach (DataRow row in ds_Bankroll.Tables[0].Rows)
                    {
                        row["Faction_type_str"] = Transfer.convertActionType(row["Faction_type"].ToString());
                        row["Fcurtype_str"] = Transfer.convertMoney_type(row["Fcurtype"].ToString());
                        row["Ftype_str"] = Transfer.convertTradeType(row["Ftype"].ToString());
                        row["Fsubject_str"] = Transfer.convertSubject(row["Fsubject"].ToString());
                        //row["Fmemo"] = Encoding.UTF8.GetString(Encoding.Default.GetBytes(row["Fmemo"].ToString()));//转换编码
                        //row["Fexplain"] = Encoding.UTF8.GetString(Encoding.Default.GetBytes(row["Fexplain"].ToString()));//转换编码
                    }
                }
                return ds_Bankroll;
            }
            catch (Exception ex)
            {
                throw new Exception("查询外卡商户流水(C账户)异常：" + ex.Message);
            }
        }

    }
}
