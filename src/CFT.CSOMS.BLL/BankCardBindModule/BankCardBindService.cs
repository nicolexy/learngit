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
using CFT.CSOMS.COMMLIB;
using CFT.Apollo.Logging;

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
        /// 判断是否一点通用户
        /// </summary>
        /// <param name="qqid"></param>
        /// <param name="Fcurtype"></param>
        /// <returns></returns>
        public bool LogOnUserCheckYDT(string qqid, string Fcurtype)
        {
            //调用快捷支付-一点通业务中查询函数来确定是否开通一点通
            DataSet ds = GetBankCardBindList_New(qqid, "", "", "", "", "", "", "", "", "", int.Parse(Fcurtype), true, 2, "", 0, 5);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
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
            try
            {
                DataSet ds = new BankcardbindData().GetBankCardBindList(
                    fuin, Fbank_type, bankID, uid, creType, creID, protocolno, phoneno,
                    strBeginDate, strEndDate, queryType, isShowAboutDetail, bindStatue, bind_serialno, limStart, limCount);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    return null;
                    //throw new Exception("没有查找到相应的记录！");
                }

                DataTable dt = ds.Tables[0];
                dt.Columns.Add("bank_status_str", typeof(string));
                dt.Columns.Add("bind_type_str", typeof(string));
                dt.Columns.Add("bind_status_str", typeof(string));
                dt.Columns.Add("bind_flag_str", typeof(string));
                dt.Columns.Add("xyzf_type_Str", typeof(string));//信用支付类型
                dt.Columns.Add("bank_type_str", typeof(string));

                foreach (DataRow dr in dt.Rows)
                {
                    if (!(dr["Fi_character4"] is DBNull)
                         && dr["Fi_character4"].ToString() == "33")
                    {
                        dr["xyzf_type_Str"] = "是";
                    }
                    else
                    {
                        dr["xyzf_type_Str"] = "否";
                    }

                    string Fbank_status = dr["Fbank_status"].ToString();
                    if (Fbank_status == "0")
                        dr["bank_status_str"] = "未定义";
                    else if (Fbank_status == "1")
                        dr["bank_status_str"] = "预绑定状态(未激活)";
                    else if (Fbank_status == "2")
                        dr["bank_status_str"] = "绑定确认(正常)";
                    else if (Fbank_status == "3")
                        dr["bank_status_str"] = "解除绑定";
                    else
                        dr["bank_status_str"] = "Unknown";

                    string Fbind_type = dr["Fbind_type"].ToString();
                    if (Fbind_type == "0")
                        dr["bind_type_str"] = "未知类型";
                    else if (Fbind_type == "1")
                        dr["bind_type_str"] = "普通借记卡关联";
                    else if (Fbind_type == "2")
                        dr["bind_type_str"] = "银行联名卡关联";
                    else if (Fbind_type == "3")
                        dr["bind_type_str"] = "信用卡关联";
                    else if (Fbind_type == "4")
                        dr["bind_type_str"] = "内部绑定";
                    else if (Fbind_type == "20")
                        dr["bind_type_str"] = "普通信用卡关联";
                    else
                        dr["bind_type_str"] = "Unknown";



                    string Fbind_status = dr["Fbind_status"].ToString();
                    if (Fbind_status == "0")
                        dr["bind_status_str"] = "未定义";
                    else if (Fbind_status == "1")
                        dr["bind_status_str"] = "初始状态";
                    else if (Fbind_status == "2")
                        dr["bind_status_str"] = "开启";
                    else if (Fbind_status == "3")
                        dr["bind_status_str"] = "关闭";
                    else if (Fbind_status == "4")
                        dr["bind_status_str"] = "解除";
                    else if (Fbind_status == "5")
                        dr["bind_status_str"] = "银行已激活，用户未激活";
                    else
                        dr["bind_status_str"] = "Unknown";


                    string Fbind_flag = dr["Fbind_flag"].ToString();
                    if (Fbind_flag == "0")
                        dr["bind_flag_str"] = "未知";
                    else if (Fbind_flag == "1")
                        dr["bind_flag_str"] = "有效";
                    else if (Fbind_flag == "2")
                        dr["bind_flag_str"] = "无效";
                    else
                        dr["bind_flag_str"] = "Unknown";


                    dr["bank_type_str"] = Transfer.convertbankType(dr["fbank_type"].ToString());
                }
                return ds;
            }
            catch
            {
                throw new Exception("没有查找到相应的记录！");
            }
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
        public DataSet GetBankCardBindList_New(string fuin, string Fbank_type, string bankID, string uid, string creID, string protocolno, string phoneno,
            string strBeginDate, string strEndDate, int bindStatue, string bind_serialno, string Operator, int bind_type, string cre_type, int limStart, int limCount)
        {
            DataSet ds = new BankcardbindData().GetBankCardBindList_New(fuin, Fbank_type, bankID, uid, creID, protocolno, phoneno, strBeginDate, strEndDate,
                bindStatue, bind_serialno, Operator, bind_type, cre_type, limStart, limCount);
            try
            {
                //对返回的xml解密的秘钥
                string key = System.Configuration.ConfigurationManager.AppSettings["RealNameKey"].ToString();
                key += Operator;

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["Ftruename"] = CommUtil.TripleDESDecryptRealName(dr["Ftruename"].ToString(), key);
                        var bank_id = CommUtil.TripleDESDecryptRealName(dr["Fbank_id"].ToString(), key); ;
                        dr["Fbank_id"] = CommUtil.EncryptZerosPadding(bank_id);
                        dr["Fcre_id"] = CommUtil.TripleDESDecryptRealName(dr["Fcre_id"].ToString(), key);
                        dr["Ftelephone"] = CommUtil.TripleDESDecryptRealName(dr["Ftelephone"].ToString(), key);
                        dr["Fmobilephone"] = CommUtil.TripleDESDecryptRealName(dr["Fmobilephone"].ToString(), key);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("查询一点通业务和快捷支付业务 解密失败：" + e.Message);
            }
            return ds;
        }



        public DataSet GetBankCardBindRelationList(string bank_type, string bank_id,
            string cre_type, string cre_id, string protocol_no, string phoneno, int bind_state, int limStart, int limCount)
        {
            try
            {
                BankcardbindData dal = new BankcardbindData();
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
                        drResult["uin"] = dr["fuin"].ToString();
                        drResult["bank_type"] = dr["Fbank_type"].ToString();
                        drResult["cre_id"] = dr["fcre_id"].ToString();
                        drResult["bank_id"] = dr["fbank_id"].ToString();
                        drResult["card_tail"] = dr["fcard_tail"].ToString();
                        dt.Rows.Add(drResult);
                    }
                }
                if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds2.Tables[0].Rows)
                    {
                        DataRow drResult = dt.NewRow();
                        drResult["uin"] = dr["fuin"].ToString();
                        drResult["bank_type"] = dr["Fbank_type"].ToString();
                        drResult["cre_id"] = dr["fcre_id"].ToString();
                        drResult["bank_id"] = dr["fbank_id"].ToString();
                        drResult["card_tail"] = dr["fcard_tail"].ToString();
                        dt.Rows.Add(drResult);
                    }
                }
                return ds;
            }
            catch (System.Exception ex)
            {
                throw new Exception("service处理错误：" + ex.Message);
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
            DataSet ds = new BankcardbindData().GetBankCardBindDetail(fuid, findex, fBDIndex);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                throw new Exception("没有查找到相应的记录！");
            }
            DataTable dt = ds.Tables[0];
            dt.Columns.Add("bank_status_str", typeof(string));
            dt.Columns.Add("bind_type_str", typeof(string));
            dt.Columns.Add("bind_status_str", typeof(string));
            dt.Columns.Add("bind_flag_str", typeof(string));
            dt.Columns.Add("sms_flag_str", typeof(string));
            dt.Columns.Add("cre_type_str", typeof(string));
            //dt.Columns.Add("xyzf_type_Str", typeof(string));//信用支付类型

            foreach (DataRow dr in dt.Rows)
            {
                string Fbank_status = dr["Fbank_status"].ToString();
                if (Fbank_status == "0")
                    dr["bank_status_str"] = "未定义";
                else if (Fbank_status == "1")
                    dr["bank_status_str"] = "预绑定状态(未激活)";
                else if (Fbank_status == "2")
                    dr["bank_status_str"] = "绑定确认(正常)";
                else if (Fbank_status == "3")
                    dr["bank_status_str"] = "解除绑定";
                else
                    dr["bank_status_str"] = "Unknown";

                string Fbind_type = dr["Fbind_type"].ToString();
                if (Fbind_type == "0")
                    dr["bind_type_str"] = "未知类型";
                else if (Fbind_type == "1")
                    dr["bind_type_str"] = "普通借记卡关联";
                else if (Fbind_type == "2")
                    dr["bind_type_str"] = "银行联名卡关联";
                else if (Fbind_type == "3")
                    dr["bind_type_str"] = "信用卡关联";
                else if (Fbind_type == "4")
                    dr["bind_type_str"] = "内部绑定";
                else if (Fbind_type == "20")
                    dr["bind_type_str"] = "普通信用卡关联";
                else
                    dr["bind_type_str"] = "Unknown";



                string Fbind_status = dr["Fbind_status"].ToString();
                if (Fbind_status == "0")
                    dr["bind_status_str"] = "未定义";
                else if (Fbind_status == "1")
                    dr["bind_status_str"] = "初始状态";
                else if (Fbind_status == "2")
                    dr["bind_status_str"] = "开启";
                else if (Fbind_status == "3")
                    dr["bind_status_str"] = "关闭";
                else if (Fbind_status == "4")
                    dr["bind_status_str"] = "解除";
                else if (Fbind_status == "5")
                    dr["bind_status_str"] = "银行已激活，用户未激活";
                else
                    dr["bind_status_str"] = "Unknown";


                string Fbind_flag = dr["Fbind_flag"].ToString();
                if (Fbind_flag == "0")
                    dr["bind_flag_str"] = "未知";
                else if (Fbind_flag == "1")
                    dr["bind_flag_str"] = "有效";
                else if (Fbind_flag == "2")
                    dr["bind_flag_str"] = "无效";
                else
                    dr["bind_flag_str"] = "Unknown";


                string sms_flag = dr["sms_flag"].ToString();
                if (sms_flag == "1")
                {
                    dr["sms_flag_str"] = "已开启";
                }
                else if (sms_flag == "0")
                {
                    dr["sms_flag_str"] = "已关闭";
                }

                string Fcre_type = dr["Fcre_type"].ToString();
                if (Fcre_type == "1")
                {
                    dr["cre_type_str"] = "身份证";
                }
                else if (Fcre_type == "2")
                {
                    dr["cre_type_str"] = "护照";
                }
                else if (Fcre_type == "3")
                {
                    dr["cre_type_str"] = "军官证";
                }
                else
                {
                    dr["cre_type_str"] = "未知";
                }
            }
            return ds;
        }





        /// <summary>
        /// 同步绑定信息
        /// </summary>
        /// <param name="bankType"></param>
        /// <param name="cardTail"></param>
        /// <param name="bankId"></param>
        /// <returns></returns>
        public DataTable SyncBankCardBind(string bankType, string cardTail, string bankId, string uid = "")
        {
            DataTable table = new DataTable();
            table.Columns.Add("ret_value", System.Type.GetType("System.String"));
            try
            {
                DataRow dr = table.NewRow();
                bool ret = new BankcardbindData().SyncBankCardBind(bankType, cardTail, bankId, uid);
                dr["ret_value"] = ret ? "true" : "false";
                table.Rows.Add(dr);
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
                bool ret = new BankcardbindData().UnbindBankCardBind(bankType, qqid, protocolNo, userIP);
                dr["ret_value"] = ret ? "true" : "false";
                table.Rows.Add(dr);
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
                bool ret = new BankcardbindData().UnBindBankCardBindSpecial(bankType, qqid, card_tail, bindSerialno, protocol_no);
                dr["ret_value"] = ret ? "true" : "false";
                table.Rows.Add(dr);
            }
            catch (Exception err)
            {
                throw new Exception("Service处理失败！", err);
            }
            return table;
        }


        /// <summary>
        /// 查询一点通业务和快捷支付业务;暂时保留，用于注销查询是否开通一点通  darrenran  2015.10.28
        /// </summary
        public DataSet GetBankCardBindList_New(string u_QQID, string Fbank_type, string bankID, string uid, string creType, string creID,
           string protocolno, string phoneno, string strBeginDate, string strEndDate, int queryType, bool isShowAboutDetail,
           int bindStatue, string bind_serialno, int limStart, int limCount)
        {
            MySqlAccess da = null;
            try
            {
                string filter = "(1=1)";
                string fuid = "";
                if (u_QQID != null && u_QQID.Trim() != "")
                {
                    fuid = PublicRes.ConvertToFuid(u_QQID);

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

                    MySqlAccess da_findUID = new MySqlAccess(PublicRes.GetConnString("HT"));
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

                da = new MySqlAccess(PublicRes.GetConnString("BD"));
                da.OpenConn();
                // 有一个专门是Fprotocol_no分表的数据表，所以跟据条件判断查哪个表，因为功能目前暂缓，暂不做
                // 2012/5/29 新增查询证件号码项
                string Sql = "select 1 as FBDIndex , Findex,Fbind_serialno,Fprotocol_no,Fuin,Fuid,Fbank_type,Fbind_flag,Fbind_type,Fbind_status,Fbank_status,right(Fcard_tail,4) as Fcard_tail," +
                    "Fbank_id,Ftruename,Funchain_time_local,Fmodify_time,Fmemo,Fcre_id,Ftelephone,Fmobilephone,Fi_character4,Fbind_time_bank,Fbind_time_local from " + PublicRes.GetTName("t_user_bind", fuid) + " where " + filter;
                //加查临时表
                string Sql2 = "select 2 as FBDIndex , Findex,Fbind_serialno,Fprotocol_no,Fuin,Fuid,Fbank_type,Fbind_flag,Fbind_type,Fbind_status,Fbank_status,right(Fcard_tail,4) as Fcard_tail," +
                    "Fbank_id,Ftruename,Funchain_time_local,Fmodify_time,Fmemo,Fcre_id,Ftelephone,Fmobilephone,Fi_character4,Fbind_time_bank,Fbind_time_local from c2c_db.t_user_bind_tmp where " + filter;
                Sql = Sql + " union all " + Sql2 + " limit " + limStart + "," + limCount;
                return da.dsGetTotalData(Sql);
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                if (da != null)
                    da.Dispose();
            }
        }

        /// <summary>
        /// 一点通业务（101140），提供接口方，关于分页的
        /// </summary>
        /// <returns></returns>
        public DataSet GetBankCardBindList_FinalNew(string fuin, string Fbank_type, string bankID, string uid, string creID, string protocolno, string phoneno,
     string strBeginDate, string strEndDate, int bindStatue, string bind_serialno, string Operator, int bind_type, string cre_type, int limStart, int limCount, out int total_num)
        {
            DataSet ds = new BankcardbindData().GetBankCardBindList_FinalNew(fuin, Fbank_type, bankID, uid, creID, protocolno, phoneno, strBeginDate, strEndDate,
                bindStatue, bind_serialno, Operator, bind_type, cre_type, limStart, limCount, out total_num);
            try
            {
                //对返回的xml解密的秘钥
                string key = System.Configuration.ConfigurationManager.AppSettings["RealNameKey"].ToString();
                key += Operator;

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Fonce_quota", "Fonce_quota");
                        MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Fday_quota", "Fday_quota");
                        dr["Ftruename"] = CommUtil.TripleDESDecryptRealName(dr["Ftruename"].ToString(), key);
                        var bank_id = CommUtil.TripleDESDecryptRealName(dr["Fbank_id"].ToString(), key); ;
                        dr["Fbank_id"] = CommUtil.EncryptZerosPadding(bank_id);
                        dr["Fcre_id"] = CommUtil.TripleDESDecryptRealName(dr["Fcre_id"].ToString(), key);
                        dr["Ftelephone"] = CommUtil.TripleDESDecryptRealName(dr["Ftelephone"].ToString(), key);
                        dr["Fmobilephone"] = CommUtil.TripleDESDecryptRealName(dr["Fmobilephone"].ToString(), key);
                        dr["sms_flag"] = dr["sms_flag"].ToString() == "1" ? "已开启" : "已关闭";
                        dr["Fbank_status"] = GetBankStatus(dr["Fbank_status"].ToString());
                        dr["Fbind_type"] = GetBindType(dr["Fbind_type"].ToString());
                        dr["Fbind_status"] = GetBindStatus(dr["Fbind_status"].ToString());
                        dr["Fbind_flag"] = GetBindFlag(dr["Fbind_flag"].ToString());
                        dr["Fcre_type"] = GetCreType(dr["Fcre_type"].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("查询一点通业务和快捷支付业务 解密失败：" + e.Message);
            }
            return ds;
        }

        public string GetBankStatus(string bank_status)
        {
            string ret = string.Empty;
            if (bank_status == "0")
                ret = "未定义";
            else if (bank_status == "1")
                ret = "预绑定状态(未激活)";
            else if (bank_status == "2")
                ret = "绑定确认(正常)";
            else if (bank_status == "3")
                ret = "解除绑定";
            else
                ret = "Unknown";
            return ret;
        }

        public string GetBindType(string bind_type)
        {
            string ret = string.Empty;
            if (bind_type == "0")
                ret = "未知类型";
            else if (bind_type == "1")
                ret = "普通借记卡关联";
            else if (bind_type == "2")
                ret = "银行联名卡关联";
            else if (bind_type == "3")
                ret = "信用卡关联";
            else if (bind_type == "4")
                ret = "内部绑定";
            else if (bind_type == "20")
                ret = "普通信用卡关联";
            else
                ret = "Unknown";
            return ret;
        }

        public string GetBindStatus(string bind_status)
        {
            string ret = string.Empty;
            if (bind_status == "0")
                ret = "未定义";
            else if (bind_status == "1")
                ret = "初始状态";
            else if (bind_status == "2")
                ret = "开启";
            else if (bind_status == "3")
                ret = "关闭";
            else if (bind_status == "4")
                ret = "解除";
            else if (bind_status == "5")
                ret = "银行已激活，用户未激活";
            else
                ret = "Unknown";
            return ret;
        }

        public string GetBindFlag(string bind_flag)
        {
            string ret = string.Empty;
            if (bind_flag == "0")
                ret = "未知";
            else if (bind_flag == "1")
                ret = "有效";
            else if (bind_flag == "2")
                ret = "无效";
            else
                ret = "Unknown";
            return ret;
        }

        public string GetCreType(string cre_type)
        {
            string ret = string.Empty;
            switch (cre_type)
            {
                case "1":
                    {
                        ret = "身份证";
                    } break;
                case "2":
                    {
                        ret = "护照";
                    } break;
                case "3":
                    {
                        ret = "军官证";
                    } break;
                default:
                    {
                        ret = "未知";
                    } break;
            }
            return ret;
        }

        /// <summary>
        /// 银行查补单状态查询
        /// </summary>
        /// <param name="bank_type">银行类型</param>
        /// <param name="bill_no">给银行订单号</param>
        /// <param name="transaction_id">财付通订单号</param>
        /// <returns></returns>
        public string GetBankSyncState(int bank_type, string bill_no, string transaction_id)
        {            
            string pay_result = string.Empty;
            try
            {
                string getBankSyncStateResult = new BankcardbindData().GetBankSyncState(bank_type, bill_no, transaction_id);                
                Dictionary<string, string> resultToDictionary = CommQuery.StringToDictionary(getBankSyncStateResult, '&', '=');
                if (resultToDictionary != null && resultToDictionary.Count>0)
                {
                    if (resultToDictionary.ContainsKey("result"))
                    {
                        string result = resultToDictionary["result"].ToString();//0 调用成功； 131512015 记录不存在 ； 131515001 参数错误；  131512009 查询数据库错误                        
                        string res_info = resultToDictionary["res_info"].ToString();//返回信息
                        if (result.Equals("0"))
                        {
                            if (resultToDictionary["pay_result"].ToString().Equals("1"))
                            {
                                pay_result = "支付结果未知";
                            }
                            else if (resultToDictionary["pay_result"].ToString().Equals("2"))
                            {
                                pay_result = "银行扣款成功";
                            }                            
                        }
                        else if (result.Equals("131512015"))
                        {
                            LogHelper.LogInfo(string.Format("BankCardBindService类GetBankSyncState方法调用出错,参数bank_type={0},bill_no={1},transaction_id={2},错误信息:{3}", bank_type, bill_no, transaction_id, "记录不存在 " + res_info));
                            pay_result = string.Empty;
                        }
                        else if (result.Equals("131515001"))
                        {
                            LogHelper.LogInfo(string.Format("BankCardBindService类GetBankSyncState方法调用出错,参数bank_type={0},bill_no={1},transaction_id={2},错误信息:{3}", bank_type, bill_no, transaction_id, "参数错误 " + res_info));
                            pay_result = string.Empty;
                        }
                        else if (result.Equals("131512009"))
                        {
                            LogHelper.LogInfo(string.Format("BankCardBindService类GetBankSyncState方法调用出错,参数bank_type={0},bill_no={1},transaction_id={2},错误信息:{3}", bank_type, bill_no, transaction_id, "查询数据库错误 " + res_info));
                            pay_result = string.Empty;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                //throw new Exception("GetBankSyncState Service处理失败！" + err.Message);
                pay_result = string.Empty;
                LogHelper.LogInfo(string.Format("BankCardBindService类GetBankSyncState方法调用出错,参数bank_type={0},bill_no={1},transaction_id={2},错误信息:{3}", bank_type, bill_no, transaction_id, err.Message));
            }
            return pay_result;
        }


        /// <summary>
        /// 银行查补单状态查询
        /// </summary>
        /// <param name="bank_type">银行类型</param>
        /// <param name="bill_no">给银行订单号</param>
        /// <param name="transaction_id">财付通订单号</param>
        /// <returns></returns>
        public DataSet GetBankSyncStateDataSet(int bank_type, string bill_no, string transaction_id)
        {
            DataSet ds = null;
            try
            {
                ds= new BankcardbindData().GetBankSyncStateDataSet(bank_type, bill_no, transaction_id);
            }
            catch (Exception err)
            {
                //throw new Exception("GetBankSyncStateDataSet Service处理失败！" + err.Message);
                ds = null;
                LogHelper.LogInfo(string.Format("BankCardBindService类GetBankSyncStateDataSet方法调用出错,参数bank_type={0},bill_no={1},transaction_id={2},错误信息:{3}", bank_type, bill_no, transaction_id, err.Message));
            }
            return ds;
        }
        /// <summary>
        /// 银行查补单状态支付状态返回结果
        /// </summary>
        /// <param name="getBankSyncStateResult">银行查补单状态查询返回字符串</param>
        /// <returns></returns>
        public string GetBankSyncStatePayResult(string getBankSyncStateResult)
        {
            string result = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(getBankSyncStateResult))
                {
                    string payResultStr = getBankSyncStateResult.Split('&')[7];
                    string payResult = payResultStr.Substring(payResultStr.IndexOf("=") + 1);
                    if (!string.IsNullOrEmpty(payResult))
                    {
                        if (payResult.Equals("1"))
                        {
                            result = "支付结果未知";
                        }
                        else if (payResult.Equals("2"))
                        {
                            result = "银行扣款成功";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = string.Empty;
            }
            return result;
        } 

    }
}
