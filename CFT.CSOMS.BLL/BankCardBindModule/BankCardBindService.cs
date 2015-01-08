using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Net.Sockets;
using System.Net;
using System.Web;
using System.Collections;
using CFT.CSOMS.BLL.TransferMeaning;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.CSOMS.DAL.BankcardUnbind;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace CFT.CSOMS.BLL.BankCardBindModule
{
    public class BankCardBindService
    {

        /// <summary>
        /// 获得银行字典数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetBankDic()
        {
            String msg = "";
            return TransferMeaning.Transfer.QueryDicInfoByType("BANK_TYPE", out msg);

        }



        /// <summary>
        /// 根据条件查询绑定的卡列表 qqId不能为空
        /// </summary>
        /// <param name="fuin"></param>
        /// <param name="Fbank_type"></param>
        /// <param name="bankID"></param>
        /// <param name="uid"></param>
        /// <param name="creType"></param>
        /// <param name="creID"></param>
        /// <param name="protocolno"></param>
        /// <param name="phoneno"></param>
        /// <param name="strBeginDate"></param>
        /// <param name="strEndDate"></param>
        /// <param name="queryType"></param>
        /// <param name="isShowAboutDetail"></param>
        /// <param name="bindStatue"></param>
        /// <param name="bind_serialno"></param>
        /// <param name="limStart"></param>
        /// <param name="limCount"></param>
        /// <returns></returns>
        public DataSet GetBankCardBindList(string fuin, string Fbank_type, string bankID, string uid,
            string creType, string creID, string protocolno, string phoneno, string strBeginDate,
            string strEndDate, int queryType, bool isShowAboutDetail, int bindStatue, string bind_serialno, int limStart, int limCount)
        {
            return new BankcardUnbindData().GetBankCardBindList(
                fuin, Fbank_type, bankID, uid, creType, creID, protocolno, phoneno, 
                strBeginDate, strEndDate, queryType, isShowAboutDetail, bindStatue, bind_serialno, limStart, limCount);
            #region 废弃代码
            
            MySqlAccess da = null;
            try
            {
                string filter = "(1=1)";
                string fuid = "";
                if (fuin != null && fuin.Trim() != "")
                {
                    fuid = CFTAccountModule.AccountService.ConvertToFuid(fuin);

                    if (fuid == null || fuid.Trim() == "")
                    {
                        throw new Exception("输入的QQ号码有误！");
                    }

                    filter += " and fuid=" + fuid;
                }

                if (uid.Trim() != "" && fuid.Trim() == "")
                {
                    fuid = uid;
                    filter += " and fuid=" + uid;
                }

                //string bankID_Encode = PublicRes.BankIDEncode_ForBankCardUnbind(bankID);
                string bankID_Encode = PublicRes.EncryptZerosPadding(bankID);

                DataSet ds_findUID = null;

                if (fuid == "")
                {
                    // 如果fuid为空则查询c2c_db.t_bind_relation
                    string sql_findUID = "select fuid from c2c_db.t_bind_relation where ";
                    string sql_findUID_filter = " (1=1) ";
                    int sql_findUID_filter_startLen = sql_findUID_filter.Length;

                    if (Fbank_type.Trim() != "")
                    {
                        sql_findUID_filter += " And Fbank_type=" + Fbank_type;
                    }

                    if (bankID != "")
                    {
                        sql_findUID_filter += " And (Fbank_id='" + bankID + "' or Fbank_id='" + bankID_Encode + "') ";
                    }

                    if (creType != "")
                    {
                        sql_findUID_filter += " And Fcre_type=" + creType;
                    }

                    if (creID != "")
                    {
                        sql_findUID_filter += " And Fcre_id='" + creID + "' ";
                    }

                    if (protocolno != "")
                    {
                        sql_findUID_filter += " And ( Fprotocol_no='" + protocolno + "' or Fbank_id='" + protocolno + "')";
                    }

                    if (phoneno != "")
                    {
                        sql_findUID_filter += " And Fmobilephone='" + phoneno + "' ";
                    }

                    if (sql_findUID_filter.Length == sql_findUID_filter_startLen)
                    {
                        //throw new Exception("请输入必须的查询条件");
                        return null;
                    }

                    sql_findUID += sql_findUID_filter;

                    MySqlAccess da_findUID = MySQLAccessFactory.GetMySQLAccess("HT");
                    da_findUID.OpenConn();
                    ds_findUID = da_findUID.dsGetTotalData(sql_findUID);
                    //DataSet ds_findUID = da_findUID.dsGetTableByRange(sql_findUID,0,1);

                    if (ds_findUID == null || ds_findUID.Tables.Count == 0 || ds_findUID.Tables[0].Rows.Count == 0)
                    {
                        return null;
                    }

                    // 这里的实现需要和产品沟通，因为一个银行卡号，证件号绑定不止一个uid

                    filter += " and fuid in (";
                    for (int i = 0; i < ds_findUID.Tables[0].Rows.Count; i++)
                    {
                        fuid = ds_findUID.Tables[0].Rows[i]["fuid"].ToString();
                        if (fuid == null || fuid.Trim() == "")
                            return null;
                        filter += fuid + ",";
                    }

                    filter = filter.Substring(0, filter.Length - 1) + ") ";

                    /*
                    fuid = ds_findUID.Tables[0].Rows[0]["fuid"].ToString();
                    if(fuid == null || fuid.Trim() == "")
                        return null;
                    filter += " and fuid=" + fuid;
                    */

                    //throw new Exception("必须输入QQ帐号或内部ID作为查询条件！");
                }

                if (!isShowAboutDetail)
                {
                    if (Fbank_type != "")
                    {
                        filter += " and Fbank_type=" + Fbank_type;
                    }

                    if (bankID != null && bankID.Trim() != "")
                    {
                        filter += " and (Fbank_id='" + bankID + "' or Fbank_id='" + bankID_Encode + "') ";
                    }

                    if (creType != null && creType.Trim() != "")
                    {
                        filter += " and Fcre_type='" + creType + "' ";
                    }

                    if (creID.Trim() != "")
                    {
                        filter += " and Fcre_id='" + creID + "' ";
                    }

                    if (protocolno.Trim() != "")
                    {
                        filter += " and Fprotocol_no='" + protocolno + "' ";
                    }

                    if (phoneno != null && phoneno.Trim() != "")
                    {
                        filter += " and Fmobilephone='" + phoneno + "' ";
                    }

                    if (bind_serialno != null && bind_serialno.Trim() != "")//序列号
                    {
                        filter += " and Fbind_serialno='" + bind_serialno + "' ";
                    }
                }

                if (strBeginDate != null && strBeginDate.Trim() != "")
                {
                    filter += " and Fcreate_time>='" + strBeginDate + "' ";
                }

                if (strEndDate != null && strEndDate.Trim() != "")
                {
                    filter += " and Fcreate_time<='" + strEndDate + "' ";
                }


                if (queryType == 1)
                {
                    // 一点通
                    filter += " and ( (Fbind_type >=1 and Fbind_type<=9) or (Fbind_type >=20 and Fbind_type<=29) or (Fbind_type >=100 and Fbind_type<=119)) ";
                }
                else if (queryType == 2)
                {
                    // 快捷支付
                    filter += " and Fbind_type >=10 and Fbind_type<=19 ";
                }

                if (bindStatue != 99)
                {
                    filter += " and Fbank_status=" + bindStatue;
                }

                //filter += " limit " + limStart + "," + limCount;

                da = MySQLAccessFactory.GetMySQLAccess("BD");
                da.OpenConn();
                // 有一个专门是Fprotocol_no分表的数据表，所以跟据条件判断查哪个表，因为功能目前暂缓，暂不做
                // 2012/5/29 新增查询证件号码项
                string Sql = "select 1 as FBDIndex , Findex,Fbind_serialno,Fprotocol_no,Fuin,Fuid,Fbank_type,Fbind_flag,Fbind_type,Fbind_status,Fbank_status,right(Fcard_tail,4) as Fcard_tail," +
                    "Fbank_id,Ftruename,Funchain_time_local,Fmodify_time,Fmemo,Fcre_id,Ftelephone,Fmobilephone,Fi_character4,Fbind_time_bank,Fbind_time_local from " + PublicRes.GetTName("c2c_db","t_user_bind", fuid) + " where " + filter;
                //加查临时表
                string Sql2 = "select 2 as FBDIndex , Findex,Fbind_serialno,Fprotocol_no,Fuin,Fuid,Fbank_type,Fbind_flag,Fbind_type,Fbind_status,Fbank_status,right(Fcard_tail,4) as Fcard_tail," +
                    "Fbank_id,Ftruename,Funchain_time_local,Fmodify_time,Fmemo,Fcre_id,Ftelephone,Fmobilephone,Fi_character4,Fbind_time_bank,Fbind_time_local from c2c_db.t_user_bind_tmp where " + filter;
                Sql = Sql + " union all " + Sql2 +" limit " +limStart + "," + limCount;
                return da.dsGetTotalData(Sql);
            }
            catch (Exception err)
            {
                return null;
            }
            finally
            {
                if (da != null)
                    da.Dispose();
            }
            #endregion
        }




        public DataSet GetBankCardBindRelationList(string bank_type, string bank_id, 
            string cre_type, string cre_id, string protocol_no, string phoneno, int bind_state, int limStart, int limCount)
        {
            try
            {
                BankcardUnbindData dal = new BankcardUnbindData();
                DataSet ds1 = dal.GetBankCardBindList_UIN(
                    bank_type, bank_id, cre_type, cre_id, protocol_no, phoneno, bind_state, limStart, limCount);

                //继续查实时绑定库表，为了查当天记录lxl
                DataSet ds2 = dal.GetBankCardBindList_UIN_2(
                    bank_type, bank_id, cre_type, cre_id, protocol_no, phoneno, bind_state, limStart, limCount);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.Tables[0].Columns.Add("uin", typeof(string));
                ds.Tables[0].Columns.Add("bank_type", typeof(string));
                ds.Tables[0].Columns.Add("cre_id", typeof(string));
                ds.Tables[0].Columns.Add("bank_id", typeof(string));
                ds.Tables[0].Columns.Add("card_tail", typeof(string));//20131017 lxl 卡号后四位
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds1.Tables[0].Rows)
                    {
                        DataRow drResult = dt.NewRow();
                        drResult["uin"]         = dr["fuin"].ToString();
                        drResult["bank_type"]   = dr["Fbank_type"].ToString();
                        drResult["cre_id"]      = dr["fcre_id"].ToString();
                        drResult["bank_id"]     = dr["fbank_id"].ToString();
                        drResult["card_tail"]   = dr["fcard_tail"].ToString();
                        dt.Rows.Add(drResult);
                    }
                }
                if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds2.Tables[0].Rows)
                    {
                        DataRow drResult = dt.NewRow();
                        drResult["uin"]         = dr["fuin"].ToString();
                        drResult["bank_type"]   = dr["Fbank_type"].ToString();
                        drResult["cre_id"]      = dr["fcre_id"].ToString();
                        drResult["bank_id"]     = dr["fbank_id"].ToString();
                        drResult["card_tail"]   = dr["fcard_tail"].ToString();
                        dt.Rows.Add(drResult);
                    }
                }
                return ds;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }



        /// <summary>
        /// 获得卡绑定详细信息
        /// </summary>
        /// <param name="fuid"></param>
        /// <param name="findex"></param>
        /// <param name="fBDIndex"></param>
        /// <returns></returns>
        public DataSet GetBankCardBindDetail(string fuid, string findex, string fBDIndex)
        {
            return new BankcardUnbindData().GetBankCardBindDetail(fuid, findex, fBDIndex);
        }

           



        /// <summary>
        /// 同步绑定信息
        /// </summary>
        /// <param name="bankType"></param>
        /// <param name="cardTail"></param>
        /// <param name="bankId"></param>
        /// <returns></returns>
        public DataTable SyncBankCardBind(string bankType, string cardTail, string bankId)
        {
            DataTable table = new DataTable();
            table.Columns.Add("ret_value", System.Type.GetType("System.String"));
            try
            {
                DataRow dr = table.NewRow();
                bool ret = new BankcardUnbindData().SyncBankCardBind(bankType, cardTail, bankId);
                dr["ret_value"] = ret ? "true" : "false";
            }
            catch (Exception e)
            {
                throw new Exception("service处理错误：" + e.Message);
            }
            return table;
        }





        /// <summary>
        /// 普通解绑操作
        /// </summary>
        /// <param name="bankType"></param>
        /// <param name="qqid"></param>
        /// <param name="protocolNo"></param>
        public DataTable UnbindBankCardBind(String bankType, String qqid, String protocolNo, String userIP)
        {
            DataTable table = new DataTable();
            table.Columns.Add("ret_value", System.Type.GetType("System.String"));
            try
            {
                DataRow dr = table.NewRow();
                bool ret = new BankcardUnbindData().UnbindBankCardBind(bankType, qqid, protocolNo, userIP);
                dr["ret_value"] = ret ? "true" : "false";
            }
            catch (Exception err)
            {
                throw new Exception("Service处理失败！" + err.Message);
            }
            return table;
        }




        /// <summary>
        /// 特殊解绑操作
        /// </summary>
        /// <param name="bankType"></param>
        /// <param name="qqid"></param>
        /// <param name="card_tail"></param>
        /// <param name="bindSerialno"></param>
        /// <param name="protocol_no"></param>
        /// <returns></returns>
        public DataTable UnBindBankCardBindSpecial(string bankType, string qqid, string card_tail, string bindSerialno, string protocol_no)
        {
            DataTable table = new DataTable();
            table.Columns.Add("ret_value", System.Type.GetType("System.String"));
            try
            {
                DataRow dr = table.NewRow();
                bool ret = new BankcardUnbindData().UnBindBankCardBindSpecial(bankType, qqid, card_tail, bindSerialno, protocol_no);
                dr["ret_value"] = ret ? "true" : "false";
            }
            catch (Exception err)
            {
                throw new Exception("Service处理失败！", err);
            }
            return table;
        }
              
    }
}
