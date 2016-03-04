﻿using System;
using System.Collections;
using System.Data;
using CFT.CSOMS.DAL.FundModule;
using TENCENT.OSS.C2C.Finance.BankLib;
using CFT.CSOMS.BLL.CFTAccountModule;

namespace CFT.CSOMS.BLL.FundModule
{
    /// <summary>
    ///理财通余额
    /// </summary>
    public class LCTBalanceService
    {
        public DataTable QueryLCTBalanceRollList(string tradeId, int start, int max)
        {

            DataTable LCTBalanceDT = new LCTBalance().QueryLCTBalanceRollList(tradeId, start, max);
            if (LCTBalanceDT != null && LCTBalanceDT.Rows.Count > 0)
            {
                LCTBalanceDT.Columns.Add("FtypeStr", typeof(string));
                LCTBalanceDT.Columns.Add("Fchannel_idStr", typeof(string));
                LCTBalanceDT.Columns.Add("FstateStr", typeof(string));
                LCTBalanceDT.Columns.Add("Ftotal_feeStr", typeof(string));
                LCTBalanceDT.Columns.Add("FInOrOUT", typeof(string));
                LCTBalanceDT.Columns.Add("Fbank_type_str", typeof(string));

                //交易类型
                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "充值成功");
                ht1.Add("2", "普通赎回到余额");
                ht1.Add("3", "快速赎回到余额");
                ht1.Add("4", "提现失败回到余额");
                ht1.Add("5", "提现成功");
                ht1.Add("6", "申购成功");
                ht1.Add("7", "余额提现(t+1)");
                ht1.Add("10", "到账成功");
                ht1.Add("11", "对账补单");

                //支付渠道
                Hashtable ht2 = new Hashtable();
                ht2.Add("1", "PC充值");
                ht2.Add("2", "微信充值");
                ht2.Add("3", "手Q充值");
                ht2.Add("68", "H5微信支付");
                ht2.Add("88", "QQ钱包");
                ht2.Add("97", "PC网银");
                ht2.Add("98", "PC微信支付");
                ht2.Add("99", "PCQQ钱包");

                //交易状态
                Hashtable ht3 = new Hashtable();
                ht3.Add("0", "初始");
                ht3.Add("1", "支付成功");
                ht3.Add("2", "充值成功不可退款状态");
                ht3.Add("3", "转入充值退款");
                ht3.Add("4", "充值退款成功");
                ht3.Add("5", "子账户虚拟提现成功，总账户未支付到基金商户");
                ht3.Add("6", "余额转出成功");
                ht3.Add("7", "余额转出退款");

                Hashtable ht4 = new Hashtable();
                ht4.Add("1", "入");
                ht4.Add("2", "入");
                ht4.Add("3", "入");
                ht4.Add("4", "入");
                ht4.Add("10", "入");
                ht4.Add("11", "入");
                ht4.Add("5", "出");
                ht4.Add("6", "出");
                ht4.Add("7", "出");

                foreach (DataRow dr in LCTBalanceDT.Rows)
                {
                    if (((string)dr["Fbank_type"]) != "0")
                    {
                        dr["Fbank_type_str"] = BankIO.QueryBankName(dr["Fbank_type"].ToString());
                    }

                    if (((string)dr["Ftype"]) == "1") //交易类型为：充值到余额的，出入状态为：入的记录，按接口返回支付渠道转义成中文展示 
                    {
                        var Fchannel = FchannelAnalysis((string)dr["Fchannel_id"]);
                        dr["Fchannel_idStr"] = COMMLIB.CommUtil.DbtypeToPageContent(Fchannel, ht2, "其他");
                    }
                }

                COMMLIB.CommUtil.DbtypeToPageContent(LCTBalanceDT, "Ftype", "FtypeStr", ht1);
                COMMLIB.CommUtil.DbtypeToPageContent(LCTBalanceDT, "Fstate", "FstateStr", ht3);
                COMMLIB.CommUtil.DbtypeToPageContent(LCTBalanceDT, "Ftype", "FInOrOUT", ht4);
                COMMLIB.CommUtil.FenToYuan_Table(LCTBalanceDT, "Ftotal_fee", "Ftotal_feeStr");
            }
            return LCTBalanceDT;
        }

        public DataTable QuerySubAccountInfo(string uin, int currencyType)
        {
            return new LCTBalance().QuerySubAccountInfo(uin, currencyType);
        }

        /// <summary>
        /// 理财通支付渠道字符串 解析
        /// </summary>
        /// <param name="Fchannel_id"></param>
        /// <returns></returns>
        protected string FchannelAnalysis(string Fchannel_id)
        {
            if (string.IsNullOrEmpty(Fchannel_id)) // 1   68|fm_3_unknown   2_98|fm_3_unknown
                return Fchannel_id;
            var arr1 = Fchannel_id.Split('|');
            if (arr1.Length > 0)
            {
                var arr2 = arr1[0].Split('_');
                string code = "";
                if (arr2.Length == 1)
                {
                    code = arr2[0];
                }
                else if (arr2.Length == 2)
                {
                    code = arr2[1];
                }
                return code.Trim();
            }
            return Fchannel_id;
        }
    }
}
