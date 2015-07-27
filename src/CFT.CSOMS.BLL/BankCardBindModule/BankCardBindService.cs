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
            DataSet ds = GetBankCardBindList(qqid, "", "", "", "", "", "", "", "", "", int.Parse(Fcurtype), true, 2, "", 0, 5);
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
                        dr["Fbank_id"] = CommUtil.TripleDESDecryptRealName(dr["Fbank_id"].ToString(), key);
                        dr["Fcre_id"] = CommUtil.TripleDESDecryptRealName(dr["Fcre_id"].ToString(), key);
                        dr["Ftelephone"] = CommUtil.TripleDESDecryptRealName(dr["Ftelephone"].ToString(), key);
                        dr["Fmobilephone"] = CommUtil.TripleDESDecryptRealName(dr["Fmobilephone"].ToString(), key);
                    }
                }
            }
            catch(Exception e)
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
        public DataTable SyncBankCardBind(string bankType, string cardTail, string bankId)
        {
            DataTable table = new DataTable();
            table.Columns.Add("ret_value", System.Type.GetType("System.String"));
            try
            {
                DataRow dr = table.NewRow();
                bool ret = new BankcardbindData().SyncBankCardBind(bankType, cardTail, bankId);
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
              
    }
}
