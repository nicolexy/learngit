using System;
using System.Collections;
using System.Data;
using CFT.CSOMS.DAL.FundModule;
using TENCENT.OSS.C2C.Finance.BankLib;

namespace CFT.CSOMS.BLL.FundModule
{
    /// <summary>
    ///理财通余额
    /// </summary>
    public class LCTBalanceService
    {
        public DataTable QueryLCTBalanceRollList(string tradeId,int start,int max)
        {

            DataTable LCTBalanceDT=new LCTBalance().QueryLCTBalanceRollList(tradeId,start, max);
            if (LCTBalanceDT != null && LCTBalanceDT.Rows.Count > 0)
            {
                LCTBalanceDT.Columns.Add("FtypeStr", typeof(string));
                LCTBalanceDT.Columns.Add("Fchannel_idStr", typeof(string));
                LCTBalanceDT.Columns.Add("FstateStr", typeof(string));
                LCTBalanceDT.Columns.Add("Ftotal_feeStr", typeof(string));
                LCTBalanceDT.Columns.Add("FInOrOUT", typeof(string));
                LCTBalanceDT.Columns.Add("Fbank_type_str", typeof(string));
                foreach (DataRow dr in LCTBalanceDT.Rows)
                {
                    dr["Fbank_type_str"] = BankIO.QueryBankName(dr["Fbank_type"].ToString());
                }
                //交易类型
                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "充值到余额");
                ht1.Add("2", "普通赎回到余额");
                ht1.Add("3", "快速赎回到余额");
                ht1.Add("4", "提现失败回到余额");
                ht1.Add("5", "余额提现(t+0)");
                ht1.Add("6", "余额申购");
                ht1.Add("7", "余额提现(t+1)");

                //渠道号
                Hashtable ht2 = new Hashtable();
                ht2.Add("1", "PC充值");
                ht2.Add("2", "微信充值");
                ht2.Add("3", "手Q充值");


                //渠道号
                Hashtable ht3 = new Hashtable();
                ht3.Add("0", "初始");
                ht3.Add("1", "支付成功");
                ht3.Add("2", "充值成功不可退款状态");
                ht3.Add("3", "转入充值退款");
                ht3.Add("4", "充值退款成功");
                ht3.Add("5", "子账户虚拟提现成功，总账户未支付到基金商户");
                ht3.Add("6", "余额转出成功");
                ht3.Add("7", "余额转出退款");

                Hashtable ht4= new Hashtable();
                ht4.Add("1", "入");
                ht4.Add("2", "入");
                ht4.Add("3", "入");
                ht4.Add("4", "入");
                ht4.Add("5", "出");
                ht4.Add("6", "出");
                ht4.Add("7", "出");

                COMMLIB.CommUtil.DbtypeToPageContent(LCTBalanceDT, "Ftype", "FtypeStr", ht1);
                COMMLIB.CommUtil.DbtypeToPageContent(LCTBalanceDT, "Fstandby1", "Fchannel_idStr", ht2);
                COMMLIB.CommUtil.DbtypeToPageContent(LCTBalanceDT, "Fstate", "FstateStr", ht3);
                COMMLIB.CommUtil.DbtypeToPageContent(LCTBalanceDT, "Ftype", "FInOrOUT", ht4);

                COMMLIB.CommUtil.FenToYuan_Table(LCTBalanceDT, "Ftotal_fee", "Ftotal_feeStr");
            }
            return LCTBalanceDT;
        }
    }
}
