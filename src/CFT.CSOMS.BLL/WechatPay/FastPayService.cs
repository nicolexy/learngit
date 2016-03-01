using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CFT.CSOMS.DAL.WechatPay;
using CFT.CSOMS.COMMLIB;
using System.Collections;
using CFT.CSOMS.DAL.Infrastructure;

namespace CFT.CSOMS.BLL.WechatPay
{
    public class FastPayService
    {
        /// <summary>
        /// 银行卡信息查询解决步骤：（可按顺序查询，没有查后面的表）
        ///1.查c2c_db_pos.t_bank_pos_月表和日表记录
        ///2.查单表c2c_db_pos.t_pos_water（需查当前表和历史表）
        ///3.查对账单的库表
        /// </summary>
        /// <param name="bankCard">卡号</param>
        /// <param name="bankDate">日期</param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        //public DataSet QueryBankCardList(string bankCard, string bankDate, int offset, int limit)
        //{
        //    if (string.IsNullOrEmpty(bankCard.Trim()))
        //    {
        //        throw new ArgumentNullException("银行卡号为空！");
        //    }
        //    if (string.IsNullOrEmpty(bankDate.Trim()))
        //    {
        //        throw new ArgumentNullException("日期为空！");
        //    }

        //    //查pos月表
        //    DataSet ds = new FastPayData().QueryPosByMonthList(bankCard, bankDate, offset, limit);
        //    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        //    {
        //        //查pos日表
        //        ds = new FastPayData().QueryPosByDayList(bankCard, bankDate, offset, limit);
        //        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        //        {
        //            //查流水表当前表及历史表
        //            ds = new FastPayData().QueryPosWaterList(bankCard, bankDate, offset, limit);
        //            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        //            {
        //                //查对账单的库表
        //                ds = new FastPayData().QueryBankDataList(bankCard, bankDate, offset, limit);
        //            }
        //        }
        //    }

        //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        ds.Tables[0].Columns.Add("FamtStr", typeof(string));
        //        ds.Tables[0].Columns.Add("Fbiz_type_str", typeof(String));//业务状态

        //        Hashtable ht1 = new Hashtable();
        //        ht1.Add("10100", "支付");
        //        ht1.Add("10200", "提现");
        //        ht1.Add("10300", "退款");
        //        ht1.Add("10400", "ATM充值");
        //        ht1.Add("10500", "session索引KEY");


        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            dr["fpay_acc"] = bankCard;
        //        }

        //        CommUtil.FenToYuan_Table(ds.Tables[0], "Famt", "FamtStr");

        //        CommUtil.DbtypeToPageContent(ds.Tables[0], "Fbiz_type", "Fbiz_type_str", ht1);
        //    }

        //    return ds;
        //}

        public Hashtable RequestBankInfo()
        {
            Hashtable ht = new Hashtable();
            string relayDefaultSPId = "20000000";
            string ip = System.Configuration.ConfigurationManager.AppSettings["BankInfoIP"].ToString();
            int port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["BankInfoPort"].ToString());
            //快捷
            string requestString = "biz_type=FASTPAY&query_mode=1&query_type=1&specified_attrs=bank_type|bank_name&limit=200&offset=0";
            DataSet ds = RelayAccessFactory.GetBankInfoFromRelay(requestString, "6508", ip, port, false, false, relayDefaultSPId);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (ht.ContainsKey(dr["bank_name"].ToString().Trim()))
                    {
                        string strBankID = ht[dr["bank_name"].ToString().Trim()].ToString();
                        ht[dr["bank_name"].ToString().Trim()] = string.Format("{0}|{1}", strBankID, dr["bank_type"].ToString());
                    }
                    else
                    {
                        ht[dr["bank_name"].ToString().Trim()] = dr["bank_type"].ToString();
                    }
                }
            }
            //一点通
            requestString = "biz_type=ONECLICK&query_mode=1&query_type=1&specified_attrs=bank_type|bank_name&limit=200&offset=0";
            DataSet dsex = RelayAccessFactory.GetBankInfoFromRelay(requestString, "6508", ip, port, false, false, relayDefaultSPId);
            if (dsex != null && dsex.Tables.Count > 0 && dsex.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in dsex.Tables[0].Rows)
                {
                    if (ht.ContainsKey(dr["bank_name"].ToString().Trim()))
                    {
                        string strBankID = ht[dr["bank_name"].ToString().Trim()].ToString();
                        ht[dr["bank_name"].ToString().Trim()] = string.Format("{0}|{1}", strBankID, dr["bank_type"].ToString());
                    }
                    else
                    {
                        ht[dr["bank_name"].ToString().Trim()] = dr["bank_type"].ToString();
                    }
                }
            }
            return ht;
        }
        /// <summary>
        /// 通过接口查询bank pos日表和月表的数据
        /// </summary>
        /// <param name="QueryType">查询类型1:银行卡查询 ,2 银行参考号查询</param>
        /// <param name="bankCard">卡号</param>
        /// <param name="bankDate">日期</param>
        /// <param name="bankType"></param>
        /// <param name="biz_type">业务类型</param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="real_bill_no">银行参考号查询 QueryType=2时,必填 </param>
        /// <returns></returns>
        public DataSet QueryBankCardNewList(int QueryType, string bankCard, string bankDate, string bankType, int biz_type, int offset, int limit, string real_bill_no="")
        {
            if (QueryType==1 && string.IsNullOrEmpty(bankCard.Trim()))
            {
                throw new ArgumentNullException("银行卡号为空！");
            }
            if (string.IsNullOrEmpty(bankDate.Trim()))
            {
                throw new ArgumentNullException("日期为空！");
            }
            
            string[] aryBankType = bankType.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            DataSet allPos = null;
            DataSet[] dsPos = new DataSet[aryBankType.Length];
            for (int i = 0; i < aryBankType.Length; ++i)
            {
                /* 通过接口查询到所有pos表的数据  */
                int offsetQ = 0;
                int limitQ = 8;
                int totalNum = 0;
                //查pos月表
                dsPos[i] = new FastPayData().GetBankPosDataList(QueryType, bankCard, bankDate, aryBankType[i], biz_type, offsetQ, limitQ, out totalNum, real_bill_no);
                //需多次查询合并结果
                int index = 1;
                while (limitQ * index < totalNum)
                {
                    offsetQ = limitQ * index;
                    DataSet ds2 = new FastPayData().GetBankPosDataList(QueryType, bankCard, bankDate, aryBankType[i], biz_type, offsetQ, limitQ, out totalNum, real_bill_no);
                    index++;
                    dsPos[i] = PublicRes.ToOneDataset(dsPos[i], ds2);//将两个库的数据合并到一个库
                }
                //合并
                allPos = PublicRes.ToOneDataset(allPos, dsPos[i]);
            }


            DataSet dsPosResult = new DataSet();
            DataTable dtPos = new DataTable();
            dsPosResult.Tables.Add(dtPos);
            dtPos.Columns.Add("fpay_acc", typeof(string));
            dtPos.Columns.Add("fbank_order", typeof(string));
            dtPos.Columns.Add("Famt", typeof(string));
            dtPos.Columns.Add("Fbiz_type", typeof(string));
            if (allPos != null && allPos.Tables.Count > 0 && allPos.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in allPos.Tables[0].Rows)
                {
                    DataRow drfield = dtPos.NewRow();
                    drfield.BeginEdit();

                    drfield["fbank_order"] = row["fbill_no"].ToString();
                    drfield["fpay_acc"] = bankCard;
                    drfield["Famt"] = row["famount"].ToString();
                    drfield["Fbiz_type"] = biz_type;

                    drfield.EndEdit();
                    dtPos.Rows.Add(drfield);
                }
            }
          
            DataSet ds = new DataSet();
            if (offset == -1 && limit == -1)
                ds = dsPosResult;
            else
            {
                if (dsPosResult != null && dsPosResult.Tables.Count > 0 && dsPosResult.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = dsPosResult.Tables[0];
                    dt = PublicRes.GetPagedTable(dt, offset, limit);//分页处理
                    ds.Tables.Add(dt.Copy());
                }
            }

            //处理数据
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("FamtStr", typeof(string));
                ds.Tables[0].Columns.Add("Fbiz_type_str", typeof(String));//业务状态

                Hashtable ht1 = new Hashtable();
                ht1.Add("10100", "支付");
                ht1.Add("10200", "提现");
                ht1.Add("10300", "退款");
                ht1.Add("10400", "ATM充值");
                ht1.Add("10500", "session索引KEY");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["fpay_acc"] = bankCard;
                }

                CommUtil.FenToYuan_Table(ds.Tables[0], "Famt", "FamtStr");

                CommUtil.DbtypeToPageContent(ds.Tables[0], "Fbiz_type", "Fbiz_type_str", ht1);
            }

            return ds;
        }

        public DataSet QueryBankCardList(string bankCard, string bankDate, int biz_type, int offset, int limit)
        {
            if (string.IsNullOrEmpty(bankCard.Trim()))
            {
                throw new ArgumentNullException("银行卡号为空！");
            }
            if (string.IsNullOrEmpty(bankDate.Trim()))
            {
                throw new ArgumentNullException("日期为空！");
            }

            /* 通过接口查询到所有pos表的数据  */
            int offsetQ = 0;
            int limitQ = 8;
            int totalNum = 0;
            //查pos月表
            DataSet dsPos = new FastPayData().QueryBankPosDataList(bankCard, bankDate, biz_type, offsetQ, limitQ, out totalNum);
            //需多次查询合并结果
            int index = 1;
            while (limitQ * index < totalNum)
            {
                offsetQ = limitQ * index;
                DataSet ds2 = new FastPayData().QueryBankPosDataList(bankCard, bankDate, biz_type, offsetQ, limitQ, out totalNum);
                index++;
                dsPos = PublicRes.ToOneDataset(dsPos, ds2);//将两个库的数据合并到一个库
            }

            DataSet dsPosResult = new DataSet();
            DataTable dtPos = new DataTable();
            dsPosResult.Tables.Add(dtPos);
            dtPos.Columns.Add("fpay_acc", typeof(string));
            dtPos.Columns.Add("fbank_order", typeof(string));
            dtPos.Columns.Add("Famt", typeof(string));
            dtPos.Columns.Add("Fbiz_type", typeof(string));
            if (dsPos != null && dsPos.Tables.Count > 0 && dsPos.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsPos.Tables[0].Rows)
                {
                    DataRow drfield = dtPos.NewRow();
                    drfield.BeginEdit();

                    drfield["fbank_order"] = row["fbill_no"].ToString();
                    drfield["fpay_acc"] = bankCard;
                    drfield["Famt"] = row["famount"].ToString();
                    drfield["Fbiz_type"] = biz_type;

                    drfield.EndEdit();
                    dtPos.Rows.Add(drfield);
                }
            }


            /* 查询到所有water表的数据  */
            offsetQ = 0;
            limitQ = 10;
            index = 0;
            bool mark=false;//标记是否查到数据
            //查流水表当前表及历史表
            DataSet dsWater = new DataSet();

            do
            {
                DataSet dsWater2 = new FastPayData().QueryPosWaterList(bankCard, bankDate, offsetQ, limitQ);
                dsWater = PublicRes.ToOneDataset(dsWater, dsWater2);
                if (dsWater2 != null && dsWater2.Tables.Count > 0 && dsWater2.Tables[0].Rows.Count == limitQ)//查到记录数等于limiQ需再查一次
                    mark = true;
                else
                    mark = false;
                index++;
                offsetQ = limitQ * index;
            } while (mark);
         

            /* 查询到所有datalist表的数据  */
            offsetQ = 0;
            limitQ = 10;
            index = 0;
            mark = false;
            //查对账单的库表
            DataSet dsBankData = new DataSet();
            
           do {
               DataSet dsBankData2 = new FastPayData().QueryBankDataList(bankCard, bankDate, offsetQ, limitQ);
                dsBankData = PublicRes.ToOneDataset(dsBankData, dsBankData2);
                if (dsBankData2 != null && dsBankData2.Tables.Count > 0 && dsBankData2.Tables[0].Rows.Count == limitQ)//查到记录数等于limiQ需再查一次
                    mark = true;
                else
                    mark = false;
                index++;
                offsetQ = limitQ * index;
            } while (mark) ;

            //综合所有的数据到ds
            DataSet dsAll = new DataSet();
            dsAll = PublicRes.ToOneDataset(dsPosResult, dsWater);
            dsAll = PublicRes.ToOneDataset(dsAll, dsBankData);
            DataSet ds = new DataSet();

            if (offset == -1 && limit == -1)
                ds = dsAll;
            else
            {
                if (dsAll != null && dsAll.Tables.Count > 0 && dsAll.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = dsAll.Tables[0];
                    dt = PublicRes.GetPagedTable(dt, offset, limit);//分页处理
                    ds.Tables.Add(dt.Copy());
                }
            }

            //处理数据
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("FamtStr", typeof(string));
                ds.Tables[0].Columns.Add("Fbiz_type_str", typeof(String));//业务状态

                Hashtable ht1 = new Hashtable();
                ht1.Add("10100", "支付");
                ht1.Add("10200", "提现");
                ht1.Add("10300", "退款");
                ht1.Add("10400", "ATM充值");
                ht1.Add("10500", "session索引KEY");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["fpay_acc"] = bankCard;
                }

                CommUtil.FenToYuan_Table(ds.Tables[0], "Famt", "FamtStr");

                CommUtil.DbtypeToPageContent(ds.Tables[0], "Fbiz_type", "Fbiz_type_str", ht1);
            }

            return ds;
        }


        public DataSet QueryBankBillNoList(string real_bill_no, string bank_type, string bankDate, int biz_type, int offset, int limit)
        {
            if (string.IsNullOrEmpty(real_bill_no.Trim()))
            {
                throw new ArgumentNullException("real_bill_no为空！");
            }
            if (string.IsNullOrEmpty(bankDate.Trim()))
            {
                throw new ArgumentNullException("日期为空！");
            }
            if (string.IsNullOrEmpty(bank_type.Trim()))
            {
                throw new ArgumentNullException("bank_type为空！");
            }

            int totalNum = 0;
           return new FastPayData().QueryBankBillNoList(real_bill_no, bank_type, bankDate, biz_type, offset, limit, out totalNum);
        }

        //零钱包收付款记录查询
        public DataSet CoinWalletsPaymentQuery(string prime_trans_id)
        {
            return new FastPayData().CoinWalletsPaymentQuery(prime_trans_id);
        }

        public DataSet FastPayLimitQuery(string bankCard, string bankType, int card_type, int pay_type, int req)
        {
            try
            {
                DataSet ds = new DataSet();
                string ip = System.Configuration.ConfigurationManager.AppSettings["FastLimitIP"].ToString();
                int port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["FastLimitPORT"].ToString());

                string cardNo = BankLib.BankIOX.Encrypt(bankCard);//银行卡加密

                if (req == 25)
                {
                    DataSet dsOne = new FastPayData().FastPayLimitQuery(ip, port, cardNo, bankType, card_type, pay_type, 25, 0);
                    if (dsOne != null && dsOne.Tables.Count > 0 && dsOne.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        ds.Tables.Add(dt);
                        if (dsOne.Tables[0].Rows[0]["is_exist_0"].ToString() == "1")
                        {
                            dt.Columns.Add("type", typeof(String));
                            dt.Columns.Add("day_0", typeof(String));
                            dt.Columns.Add("day_existed_num_0", typeof(String));
                            dt.Columns.Add("day_existed_amount_0", typeof(String));
                            dt.Columns.Add("month_existed_num_0", typeof(String));
                            dt.Columns.Add("month_existed_amount_0", typeof(String));

                            DataRow row = dsOne.Tables[0].Rows[0];
                            DataRow dr = dt.NewRow();
                            dr.BeginEdit();
                            dr["type"] = "银行";
                            dr["day_0"] = CommRes.IceDecode(row["day_0"].ToString().Trim());
                            dr["day_existed_num_0"] = CommRes.IceDecode(row["day_existed_num_0"].ToString().Trim());
                            dr["day_existed_amount_0"] = CommRes.IceDecode(row["day_existed_amount_0"].ToString().Trim());
                            dr["month_existed_num_0"] = CommRes.IceDecode(row["month_existed_num_0"].ToString().Trim());
                            dr["month_existed_amount_0"] = CommRes.IceDecode(row["month_existed_amount_0"].ToString().Trim());
                            dr.EndEdit();
                            dt.Rows.Add(dr);

                            if (dsOne.Tables[0].Rows[0]["is_exist_0"].ToString() == "1")
                            {
                                DataRow dr1 = dt.NewRow();
                                dr1.BeginEdit();
                                dr1["type"] = "一键支付与小额免短";
                                dr1["day_0"] = PublicRes.objectToString(dsOne.Tables[0], "day_1");
                                dr1["day_existed_num_0"] = PublicRes.objectToString(dsOne.Tables[0], "day_existed_num_1");
                                dr1["day_existed_amount_0"] = PublicRes.objectToString(dsOne.Tables[0], "day_existed_amount_1");
                                dr1["month_existed_num_0"] = PublicRes.objectToString(dsOne.Tables[0], "month_existed_num_1");
                                dr1["month_existed_amount_0"] = PublicRes.objectToString(dsOne.Tables[0], "month_existed_amount_1");
                                dr1.EndEdit();
                                dt.Rows.Add(dr1);
                            }

                            COMMLIB.CommUtil.FenToYuan_Table(ds.Tables[0], "day_existed_amount_0", "day_existed_amount_0");
                            COMMLIB.CommUtil.FenToYuan_Table(ds.Tables[0], "month_existed_amount_0", "month_existed_amount_0");
                            
                        }
                    }
                }
                else if (req == 22)
                {
                    string categoryStr = "";
                    int category = 0;
                    Hashtable ht = new Hashtable();
                    ht.Add(1, "虚拟物品行业限制(1)");
                    ht.Add(3, "实物行业限制(3)");
                    ht.Add(101, "1000(101)");
                    ht.Add(102, "2000(102)");
                    ht.Add(103, "3000(103)");
                    ht.Add(104, "4000(104)");
                    ht.Add(105, "5000(105)");
                    ht.Add(106, "6000(106)");
                    ht.Add(107, "7000(107)");
                    ht.Add(108, "8000(108)");
                    ht.Add(109, "9000(109)");
                    ht.Add(110, "10000(110)");
                    ht.Add(10, "内部转账类行业限制(10)");
                    ht.Add(11, "快捷灰名单(11)");
                    ht.Add(4, "体验白名单(4)");
                    ht.Add(7, "特殊商户限制(7)");
                    ht.Add(8, "理财产品类限制(8)");
                    ht.Add(9, "京东大额类别(9)");
                    ht.Add(12, "大额实物3-仅供特斯拉商户使用(12)");

                    DataTable dt = new DataTable();
                    ds.Tables.Add(dt);
                    dt.Columns.Add("type", typeof(String));
                    dt.Columns.Add("day_0", typeof(String));
                    dt.Columns.Add("day_existed_num_0", typeof(String));
                    dt.Columns.Add("day_existed_amount_0", typeof(String));
                    dt.Columns.Add("month_existed_num_0", typeof(String));
                    dt.Columns.Add("month_existed_amount_0", typeof(String));
                    dt.Columns.Add("limit_type_1_0", typeof(String));
                    dt.Columns.Add("limit_type_2_0", typeof(String));
                    dt.Columns.Add("limit_type_3_0", typeof(String));
                    dt.Columns.Add("limit_type_4_0", typeof(String));
                    dt.Columns.Add("limit_type_5_0", typeof(String));
                    dt.Columns.Add("day_amount_0", typeof(String));

                    foreach (DictionaryEntry de in ht)
                    {
                        category = int.Parse(de.Key.ToString());
                        categoryStr = de.Value.ToString();
                        DataSet dsOne = new FastPayData().FastPayLimitQuery(ip, port, cardNo, bankType, card_type, pay_type, 22, category);
                        if (dsOne != null && dsOne.Tables.Count > 0 && dsOne.Tables[0].Rows.Count > 0)
                        {
                            if (dsOne.Tables[0].Rows[0]["is_exist_0"].ToString() == "1")
                            {
                                DataRow row = dsOne.Tables[0].Rows[0];
                                DataRow dr = dt.NewRow();
                                dr.BeginEdit();
                                dr["type"] = categoryStr;
                                dr["day_0"] = CommRes.IceDecode(row["day_0"].ToString().Trim());
                                dr["day_existed_num_0"] = CommRes.IceDecode(row["day_existed_num_0"].ToString().Trim());
                                dr["day_existed_amount_0"] = CommRes.IceDecode(row["day_existed_amount_0"].ToString().Trim());
                                dr["month_existed_num_0"] = CommRes.IceDecode(row["month_existed_num_0"].ToString().Trim());
                                dr["month_existed_amount_0"] = CommRes.IceDecode(row["month_existed_amount_0"].ToString().Trim());
                                dr["limit_type_1_0"] = PublicRes.objectToString(dsOne.Tables[0], "limit_type_1_0");
                                dr["limit_type_2_0"] = PublicRes.objectToString(dsOne.Tables[0], "limit_type_2_0");
                                dr["limit_type_3_0"] = PublicRes.objectToString(dsOne.Tables[0], "limit_type_3_0");
                                dr["limit_type_4_0"] = PublicRes.objectToString(dsOne.Tables[0], "limit_type_4_0");
                                dr["limit_type_5_0"] = PublicRes.objectToString(dsOne.Tables[0], "limit_type_5_0");

                                dr.EndEdit();
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        COMMLIB.CommUtil.FenToYuan_Table(ds.Tables[0], "day_existed_amount_0", "day_existed_amount_0");
                        COMMLIB.CommUtil.FenToYuan_Table(ds.Tables[0], "month_existed_amount_0", "month_existed_amount_0");
                        COMMLIB.CommUtil.FenToYuan_Table(ds.Tables[0], "limit_type_1_0", "limit_type_1_0");
                        COMMLIB.CommUtil.FenToYuan_Table(ds.Tables[0], "limit_type_2_0", "limit_type_2_0");
                        COMMLIB.CommUtil.FenToYuan_Table(ds.Tables[0], "limit_type_4_0", "limit_type_4_0");
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("查询限额统计异常：" + ex);
            }
        }

    }
}
