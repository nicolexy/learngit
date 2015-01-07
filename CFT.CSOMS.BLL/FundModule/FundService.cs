﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using CFT.CSOMS.DAL.FundModule;
using TENCENT.OSS.C2C.Finance.BankLib;

namespace CFT.CSOMS.BLL.FundModule
{       
    public class FundService
    {
        private static IList<Fund> FundInfos;

        public DataTable GetFundTradeLog(string qqid,int istr, int imax)
        {
            if (string.IsNullOrEmpty(qqid))
                throw new ArgumentNullException("qqid");
            return new SafeCard().GetFundTradeLog(qqid, istr, imax);
        }

        public DataTable GetPayCardInfo(string qqid) 
        {
            if (string.IsNullOrEmpty(qqid))
                throw new ArgumentNullException("qqid");
            DataTable dt=new SafeCard().GetPayCardInfo(qqid);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("bank_type_name", typeof(string));
                dt.Rows[0]["bank_type_name"] = BankIO.QueryBankName(dt.Rows[0]["Fbank_type"].ToString());
            }
            return dt;
        }

        public DataTable GetFundRollList(string u_QQID, DateTime u_BeginTime, DateTime u_EndTime, string Fcurtype, int istr, int imax, int Ftype)
        {
            if (string.IsNullOrEmpty(u_QQID))
                throw new ArgumentNullException("u_QQID");
            DataTable dt=new FundRoll().QueryFundRollList(u_QQID, u_BeginTime, u_EndTime, Fcurtype, istr, imax, Ftype);
            return dt;
        }

        public DataTable GetFundProfitRecord(string tradeId, string beginDateStr, string endDateStr, int currencyType = -1, string spId = "", int currentPageIndex = 0, int pageSize = 10)
        {
            return new FundProfit().QueryProfitRecord(tradeId, beginDateStr, endDateStr, currencyType, spId, currentPageIndex*pageSize, pageSize);            
        }

        public static IList<Fund> GetAllFundInfo()
        {
            //先看静态变量是否缓存
            if (FundInfos != null)
                return FundInfos;

            var fundInfoTable = new FundInfoData().QueryAllFundInfo();

            try
            {
                FundInfos = new List<Fund>();

                foreach (DataRow item in fundInfoTable.Rows)
	            {
                    FundInfos.Add(new Fund()
                    {
                        SPId = item["Fspid"].ToString(),
                        SPName = item["Fsp_name"].ToString(),
                        Name = item["Ffund_name"].ToString(),
                        Code = item["Ffund_code"].ToString(),
                        CurrencyType = int.Parse(item["Fcurtype"].ToString())
                    });
	            }
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("映射基金信息数据异常：{0}", ex.Message));
            }

            return FundInfos;
        }

        public string GetTradeIdByUIN(string uin)
        {
            if (string.IsNullOrEmpty(uin))
                throw new ArgumentNullException("uin");

            var fundAccountInfo = new FundAccountInfo().QueryFundAccountRelationInfo(uin);

            if (fundAccountInfo.Rows.Count < 1)
                return null;

            return fundAccountInfo.Rows[0]["Ftrade_id"].ToString();
        }

        public DataTable GetUserFundAccountInfo(string uin)
        {
            if (string.IsNullOrEmpty(uin))
                throw new ArgumentNullException("uin");
            try
            {
                var fundAccountInfo = new FundAccountInfo().QueryFundAccountRelationInfo(uin);

                if (fundAccountInfo == null || fundAccountInfo.Rows.Count == 0)
                    return null;

                fundAccountInfo.Columns.Add("FstateName", typeof(string));
                fundAccountInfo.Columns.Add("FlstateName", typeof(string));

                foreach (DataRow dr in fundAccountInfo.Rows)
                {
                    if (dr["Fstate"].ToString() == "1")
                    {
                        dr["FstateName"] = "初始态";
                    }
                    else if (dr["Fstate"].ToString() == "2")
                    {
                        dr["FstateName"] = "审核中（预留）";
                    }
                    else if (dr["Fstate"].ToString() == "3")
                    {
                        dr["FstateName"] = "开户完成";
                    }
                    else
                    {
                        dr["FstateName"] = "未定义的状态：" + dr["Fstate"];
                    }

                }

                return fundAccountInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("基金账户信息查询异常：{0}", ex.Message));
            }
        }

        /// <summary>
        /// 获取用户的基金概要（余额，收益）
        /// </summary>
        /// <param name="uin"></param>
        /// <returns></returns>
        public DataTable GetUserFundSummary(string uin)
        {
            var tradeId = GetTradeIdByUIN(uin);
            if (string.IsNullOrEmpty(tradeId))
                throw new Exception(string.Format("{0}没有对应的基金账户，查询不到TradeId", uin));

            //获取用户的所有基金
           // var userFundsTable = new FundProfit().QueryProfitStatistic(tradeId);
            var userFundsTable = new FundProfit().QueryFundBind(tradeId);//查商户号

            if(userFundsTable.Rows.Count < 1)
                throw new Exception(string.Format("{0}没有申购的基金", uin));

            userFundsTable.Columns.Add("balance", typeof(string));
            userFundsTable.Columns.Add("fundName", typeof(string));
            userFundsTable.Columns.Add("Fcurtype", typeof(string));
            userFundsTable.Columns.Add("Ftotal_profit", typeof(string));
            userFundsTable.Columns.Add("con", typeof(string));//冻结金额
            userFundsTable.Columns.Add("fund_code", typeof(string));
            userFundsTable.Columns.Add("close_flag", typeof(string));
            userFundsTable.Columns.Add("transfer_flag", typeof(string));
            userFundsTable.Columns.Add("buy_valid", typeof(string));

            var cftAccountBLLService = new CFTAccountModule.AccountService();
            DataTable subAccountInfoTable;

            foreach (DataRow item in userFundsTable.Rows)
            {
                var FundInfoBySpidTable = new FundInfoData().QueryFundInfoBySpid(item["Fspid"].ToString());//查Fcurtype及基金名称
                if (FundInfoBySpidTable != null && FundInfoBySpidTable.Rows.Count > 0)
                {
                    item["Fcurtype"] = FundInfoBySpidTable.Rows[0]["Fcurtype"].ToString();
                    item["fundName"] = FundInfoBySpidTable.Rows[0]["Ffund_name"].ToString();
                    item["fund_code"] = FundInfoBySpidTable.Rows[0]["Ffund_code"].ToString();
                    item["close_flag"] = FundInfoBySpidTable.Rows[0]["Fclose_flag"].ToString();
                    item["transfer_flag"] = FundInfoBySpidTable.Rows[0]["Ftransfer_flag"].ToString();
                    item["buy_valid"] = FundInfoBySpidTable.Rows[0]["Fbuy_valid"].ToString();
                }

                //balance
                subAccountInfoTable = cftAccountBLLService.QuerySubAccountInfo(uin, int.Parse(item["Fcurtype"].ToString()));
                if (subAccountInfoTable == null || subAccountInfoTable.Rows.Count < 1)
                {
                    item["balance"] = "0";
                    item["con"] = "0";
                }
                else
                {
                    item["balance"] = subAccountInfoTable.Rows[0]["Fbalance"].ToString();
                    item["con"] = subAccountInfoTable.Rows[0]["Fcon"].ToString();
                }

                //fundname

           //    item["fundName"] = GetAllFundInfo().Where(i => i.CurrencyType == int.Parse(item["Fcurtype"].ToString())).First().Name;

                var ProfitTable = new FundProfit().QueryProfitStatistic(tradeId, int.Parse(item["Fcurtype"].ToString()));//查累计收益
                if (ProfitTable != null && ProfitTable.Rows.Count > 0)
                {
                    item["Ftotal_profit"] = ProfitTable.Rows[0]["Ftotal_profit"].ToString();
                }
                else
                {
                    item["Ftotal_profit"] = "0";
                }
               
            }

            return userFundsTable;
        }

        /// 获取用户的总余额
        public long GetUserFundTotal(string uin)
        {
            //总帐户余额
            var summaryTable = GetUserFundSummary(uin);

            //统计余额总和
            long totalBalance = 0;
            if (summaryTable != null && summaryTable.Rows.Count > 0)
            foreach (DataRow item in summaryTable.Rows)
            {
                totalBalance += long.Parse(item["balance"].ToString());
            }
            return totalBalance;
        }

        public bool IfAnewBoughtFund(string listid, string time)
        {
            if (string.IsNullOrEmpty(listid))
                throw new ArgumentNullException("listid");
            return new FundInfoData().QueryIfAnewBoughtFund(listid, DateTime.Parse(time));
        }
        //查询基金交易单
        public DataTable QueryTradeFundInfo(string spid, string listid)
        {
            return new FundInfoData().QueryTradeFundInfo(spid, listid);
        }

        //查询定期产品用户交易记录
        public DataTable QueryCloseFundRollList( string tradeId,string fundCode, string beginDateStr, string endDateStr,int currentPageIndex = 0, int pageSize = 10)
        {
             DataTable tbCloseFundRollList=new FundInfoData().QueryCloseFundRollList(tradeId, fundCode, beginDateStr, endDateStr, currentPageIndex, pageSize);
             if (tbCloseFundRollList != null)
             {
                 tbCloseFundRollList.Columns.Add("Fstart_total_fee_str", typeof(string));
                 tbCloseFundRollList.Columns.Add("Fcurrent_total_fee_str", typeof(string));
                 tbCloseFundRollList.Columns.Add("Fend_tail_fee_str", typeof(string));
                 tbCloseFundRollList.Columns.Add("Fstate_str", typeof(string));//绑定状态
                 tbCloseFundRollList.Columns.Add("Fpay_type_str", typeof(string));//支付类型
                 tbCloseFundRollList.Columns.Add("Fchannel_id_str", typeof(string));//渠道信息
                 tbCloseFundRollList.Columns.Add("Fuser_end_type_str", typeof(string));//到期策略

                 if (tbCloseFundRollList.Rows.Count > 0)
                     foreach (DataRow dr in tbCloseFundRollList.Rows)
                     {
                         switch (dr["Fstate"].ToString())
                         {
                             case "1":
                                 dr["Fstate_str"] = "初始状态";
                                 break;
                             case "2":
                                 dr["Fstate_str"] = "待执行";
                                 break;
                             case "3":
                                 dr["Fstate_str"] = "扫尾执行中";
                                 break;
                             case "4":
                                 dr["Fstate_str"] = "赎回成功";
                                 break;
                             case "5":
                                 dr["Fstate_str"] = "执行成功";
                                 break;
                             case "6":
                                 dr["Fstate_str"] = "执行失败";
                                 break;
                             default:
                                 dr["Fstate_str"] = dr["Fstate"].ToString();
                                 break;
                         }
                         switch (dr["Fuser_end_type"].ToString())
                         {
                             case "1":
                                 dr["Fuser_end_type_str"] = "指定赎回金额，余下全额申购";
                                 break;
                             case "2":
                                 dr["Fuser_end_type_str"] = "全额赎回";
                                 break;
                             case "3":
                                 dr["Fuser_end_type_str"] = "全部顺延至下一期";
                                 break;
                             case "4":
                                 dr["Fuser_end_type_str"] = "指定申购金额，余下全额赎回";
                                 break;
                             case "99":
                                 dr["Fuser_end_type_str"] = "客服强赎";
                                 break;
                             default:
                                 dr["Fuser_end_type_str"] = dr["Fuser_end_type"].ToString();
                                 break;
                         }

                         switch (dr["Fpay_type"].ToString())
                         {
                             case "1":
                                 dr["Fpay_type_str"] = "微信支付";
                                 break;
                             case "2":
                                 dr["Fpay_type_str"] = "期顺延申购";
                                 break;
                             case "3":
                                 dr["Fpay_type_str"] = "微信支付及期顺延申购混合";
                                 break;
                             default:
                                 dr["Fpay_type_str"] = dr["Fpay_type"].ToString();
                                 break;
                         }
                         switch (dr["Fchannel_id"].ToString())
                         {
                             case "1":
                                 dr["Fchannel_id_str"] = "财付通网站";
                                 break;
                             case "2":
                                 dr["Fchannel_id_str"] = "微信";
                                 break;
                             case "3":
                                 dr["Fchannel_id_str"] = "手Q";
                                 break;
                             default:
                                 dr["Fchannel_id_str"] = dr["Fchannel_id"].ToString();
                                 break;
                         }

                     }

                 COMMLIB.CommUtil.FenToYuan_Table(tbCloseFundRollList, "Fstart_total_fee", "Fstart_total_fee_str");
                 COMMLIB.CommUtil.FenToYuan_Table(tbCloseFundRollList, "Fcurrent_total_fee", "Fcurrent_total_fee_str");
                 COMMLIB.CommUtil.FenToYuan_Table(tbCloseFundRollList, "Fend_tail_fee", "Fend_tail_fee_str");
             }

             return tbCloseFundRollList;
        }

          //通过商户号查询基金公司信息
        public DataTable QueryFundInfoBySpid(string spid)
        {
            return new FundInfoData().QueryFundInfoBySpid(spid);
        }
        //返回所有基金公司信息
        public DataTable QueryAllFundInfo()
        {
            return new FundInfoData().QueryAllFundInfo();
        }
        //查询用户余额收益情况明细  -- 将页面的逻辑封装在接口中
        public DataTable BindProfitList(string tradeId, string beginDateStr, string endDateStr, int currencyType = -1, string spId = "", int currentPageIndex = 0, int pageSize = 5)
        {
            try
            {
                DataTable profits = new FundProfit().QueryProfitRecord(tradeId, beginDateStr, endDateStr, currencyType, spId, currentPageIndex * pageSize, pageSize);
                if (profits.Rows.Count > 0)
                {
                    profits.Columns.Add("Fvalid_money_str", typeof(String));//收益本金额
                    profits.Columns.Add("Fpur_typeName", typeof(String));//科目
                    profits.Columns.Add("F7day_profit_rate_str", typeof(String));//七日年化收益
                    profits.Columns.Add("Fprofit_str", typeof(String));//收益金额
                    profits.Columns.Add("Fspname", typeof(String));//基金公司名
                    profits.Columns.Add("Fprofit_per_ten_thousand", typeof(string));//万份收益

                    foreach (DataRow dr in profits.Rows)
                    {
                        dr["Fpur_typeName"] = "分红";
                        if (!(dr["F7day_profit_rate"] is DBNull))
                        {
                            string tmp = dr["F7day_profit_rate"].ToString();
                            if (!string.IsNullOrEmpty(tmp))
                            {
                                decimal d = (decimal)(Int64.Parse(tmp)) / 100000000;
                                dr["F7day_profit_rate_str"] = d.ToString("P4");
                            }
                        }
                        //万份收益计算
                        if (dr["F1day_profit_rate"] != null && dr["F1day_profit_rate"] != DBNull.Value && dr["F1day_profit_rate"].ToString() != string.Empty)
                        {
                            decimal oneDayProfitRrate = 0;
                            decimal.TryParse(dr["F1day_profit_rate"].ToString(), out oneDayProfitRrate);
                            decimal profitPerTenThousand = oneDayProfitRrate / 10000;

                            dr["Fprofit_per_ten_thousand"] = profitPerTenThousand.ToString("N3");
                        }

                        try
                        {
                            //基金公司名
                            dr["Fspname"] = FundService.GetAllFundInfo().Where(i => i.SPId == dr["Fspid"].ToString()).First().SPName;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("查询spid:"+dr["Fspid"].ToString()+"的基金公司名异常！");
                        }
                    }

                    COMMLIB.CommUtil.FenToYuan_Table(profits, "Fvalid_money", "Fvalid_money_str");
                    COMMLIB.CommUtil.FenToYuan_Table(profits, "Fprofit", "Fprofit_str");
                    return profits;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("查询基金收益记录异常：{0}", ex.Message));
            }
            return null;
        }
        
        //查询用户交易流水情况(整合解析功能) 
        public DataTable BindBankRollListNotChildren(string qqId, string curtype, DateTime beginDate, DateTime endDate, int pageIndex = 0, int pageMax = 5, int redirectionType = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(qqId))
                    throw new ArgumentNullException("qqId");

                /*
                var fundInfo = FundService.GetAllFundInfo().Where(i => i.SPId == spId);

                if (fundInfo.Count() < 1)
                    throw new Exception(string.Format("找不到{0}对应的基金信息", spId));
                */

                DataTable bankRollList = GetFundRollList(qqId, beginDate, endDate, curtype, pageIndex, pageMax, redirectionType);

                if (bankRollList != null)
                {
                    bankRollList.Columns.Add("Fsub_trans_id_str", typeof(string));
                    bankRollList.Columns.Add("FtypeText", typeof(string));
                    bankRollList.Columns.Add("Ftotal_fee_str", typeof(string));
                    bankRollList.Columns.Add("Floading_type_str", typeof(string));
                    bankRollList.Columns.Add("Fstate_str", typeof(string));

                    if (bankRollList.Rows.Count > 0)
                    {
                        foreach (DataRow dr in bankRollList.Rows)
                        {
                            dr["Fsub_trans_id_str"] = "104" + dr["Fsub_trans_id"].ToString();
                            string Fpur_type = dr["Fpur_type"].ToString();
                            switch (Fpur_type)
                            {
                                case "1":
                                    dr["FtypeText"] = "入";
                                    break;
                                case "2":
                                    dr["FtypeText"] = "认购";
                                    break;
                                case "3":
                                    dr["FtypeText"] = "定投";
                                    break;
                                case "4":
                                    dr["FtypeText"] = "出";
                                    break;
                                case "5":
                                    dr["FtypeText"] = "撤销";
                                    break;
                                case "6":
                                    dr["FtypeText"] = "分红";
                                    break;
                                case "7":
                                    dr["FtypeText"] = "认申购失败";
                                    break;
                                case "8":
                                    dr["FtypeText"] = "比例确认退款";
                                    break;
                                case "9":
                                    dr["FtypeText"] = "赠送收益申购";
                                    break;
                                case "10":
                                    dr["FtypeText"] = "赠送份额申购";
                                    break;
                                case "11":
                                    dr["FtypeText"] = "从余额购买基金";
                                    break;
                                case "12":
                                    dr["FtypeText"] = "基金赎回到余额";
                                    break;
                                default:
                                    dr["FtypeText"] = dr["Fpur_type"].ToString();
                                    break;
                            }

                            if (Fpur_type == "4")
                            {
                                switch (dr["Floading_type"].ToString())
                                {
                                    case "0":
                                        dr["Floading_type_str"] = "普通赎回";
                                        break;
                                    case "1":
                                        dr["Floading_type_str"] = "快速赎回";
                                        break;
                                    default:
                                        dr["Floading_type_str"] = dr["Floading_type"].ToString();
                                        break;
                                }

                            }
                            else if (Fpur_type == "11" || Fpur_type == "12")
                            {
                                switch (dr["Fpurpose"].ToString())
                                {
                                    case "0":
                                        dr["Floading_type_str"] = "普通赎回";
                                        break;
                                    case "1":
                                        dr["Floading_type_str"] = "赎回提现";
                                        break;
                                    case "2":
                                        dr["Floading_type_str"] = "消费";
                                        break;
                                    case "3":
                                        dr["Floading_type_str"] = "份额转换";
                                        break;
                                    //case "4":
                                    //    dr["Floading_type_str"] = "";
                                    //    break;
                                    case "5":
                                        dr["Floading_type_str"] = "买合约机";
                                        break;
                                    case "6":
                                        dr["Floading_type_str"] = "余额申购";
                                        break;
                                    case "7":
                                        dr["Floading_type_str"] = "快速赎回到余额";
                                        break;
                                    case "8":
                                        dr["Floading_type_str"] = "合约机赎回提现给商户";
                                        break;
                                    case "9":
                                        dr["Floading_type_str"] = "t+1赎回到余额";
                                        break;
                                    default:
                                        dr["Floading_type_str"] = dr["Fpurpose"].ToString();
                                        break;
                                }
                            }else
                            {//除了出其他都没有赎回方式
                                dr["Floading_type_str"] = "";
                            }
                        }

                        Hashtable ht = new Hashtable();
                        ht.Add("0", "创建申购单");
                        ht.Add("1", "等待扣款");
                        ht.Add("2", "代扣成功");
                        ht.Add("3", "申购成功");
                        ht.Add("4", "初始赎回单");
                        ht.Add("5", "到基金公司赎回成功");
                        ht.Add("6", "到基金公司赎回失败");
                        ht.Add("7", "申购请求失败，订单失效");
                        ht.Add("8", "申购单申请退款");
                        ht.Add("9", "申购单转入退款");
                        ht.Add("10", "赎回单受理完成");
                        ht.Add("11", "子账户提现请求成功");
                        ht.Add("20", "作废");

                        COMMLIB.CommUtil.DbtypeToPageContent(bankRollList, "Fstate", "Fstate_str", ht);
                        COMMLIB.CommUtil.FenToYuan_Table(bankRollList, "Ftotal_fee", "Ftotal_fee_str");
                    }
                    return bankRollList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("获取用户交易流水异常:{0}", ex.Message));
            }
            return null;
        }

        /// <summary>
        /// 查询合约机详情
        /// </summary>
        /// <param name="listid">订单号</param>
        /// <returns></returns>
        public DataTable QueryContractMachineDetail(string listid) 
        {
            if (string.IsNullOrEmpty(listid))
            {
                throw new ArgumentNullException("必填参数：单号为空！");
            }

            DataTable dt = new FundOperatorData().QueryContractMachineDetail(listid);
            if (dt != null && dt.Rows.Count > 0) 
            {
                dt.Columns.Add("stateStr", typeof(String));//状态
                dt.Columns.Add("chanidStr", typeof(String));//购买渠道
                dt.Columns.Add("totalfeeStr", typeof(String));//冻结金额

                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "未支付");
                ht1.Add("2", "支付成功");
                ht1.Add("3", "支付失败");
                ht1.Add("4", "已退款");
                ht1.Add("5", "可发货");
                ht1.Add("6", "已发货");
                ht1.Add("7", "取消发货");

                Hashtable ht2 = new Hashtable();
                ht2.Add("1000", "理财通");
                ht2.Add("1001", "营业厅");

                COMMLIB.CommUtil.FenToYuan_Table(dt, "total_fee", "totalfeeStr");

                COMMLIB.CommUtil.DbtypeToPageContent(dt, "state", "stateStr", ht1);
                COMMLIB.CommUtil.DbtypeToPageContent(dt, "chanid", "chanidStr", ht2);
            }

            return dt;
        }

        public DataTable QueryPhoneDetail(string spid, string phone) 
        {
            if (string.IsNullOrEmpty(phone))
            {
                throw new ArgumentNullException("必填参数：phone为空！");
            }

            DataTable dt = null;

            DataSet ds = new FundOperatorData().QueryPhoneDetail(spid, phone);
            if (ds != null && ds.Tables.Count > 0) 
            {
                dt = ds.Tables[0];
                dt.Columns.Add("FstateStr", typeof(String));//状态

                Hashtable ht1 = new Hashtable();
                ht1.Add("1", "未使用");
                ht1.Add("2", "已使用");
                ht1.Add("3", "冻结");
                ht1.Add("4", "作废");

                COMMLIB.CommUtil.DbtypeToPageContent(dt, "Fstate", "FstateStr", ht1);
            }

            return dt;
        }
        /// <summary>
        /// 通过uin查询合约机订单号
        /// </summary>
        /// <param name="uin"></param>
        /// <returns></returns>
        public DataSet QueryListidFromUin(string uin) 
        {
            if (string.IsNullOrEmpty(uin))
            {
                throw new ArgumentNullException("必填参数：uin为空！");
            }

            return new FundOperatorData().QueryListidFromUin(uin); 
        }

        /// <summary>
        /// 子帐户资金流水查询函数(整合了页面逻辑) 从KF_SERVICE迁移过来的
        /// </summary>
        /// <param name="qqId"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="spId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageMax"></param>
        /// <param name="fType"></param>
        /// <param name="fMemo"></param>
        /// <returns></returns>
        public DataTable GetChildrenBankRollListEx(string qqId, string spId, string curtype, DateTime beginTime, DateTime endTime, int offset, int limit, int fType, string fMemo)
        {
            try
            {
               // int start = pageMax * (pageIndex - 1);
                //if (string.IsNullOrEmpty(spId))
                //    throw new Exception(string.Format("无法同时查询所有基金的流水信息，请选择指定的基金"));

                //var fundInfo = FundService.GetAllFundInfo().Where(i => i.SPId == spId);


                //if (fundInfo.Count() < 1)
                //    throw new Exception(string.Format("找不到{0}对应的基金信息", spId));

                var bankRollList = new FundRoll().GetChildrenBankRollList(qqId, beginTime, endTime, curtype, offset, limit, fType, fMemo);

                if (bankRollList.Tables != null && bankRollList.Tables.Count > 0)
                {
                    bankRollList.Tables[0].Columns.Add("FpaynumText", typeof(string));
                    bankRollList.Tables[0].Columns.Add("FbalanceText", typeof(string));
                    bankRollList.Tables[0].Columns.Add("FtypeText", typeof(string));
                    bankRollList.Tables[0].Columns.Add("FmemoText", typeof(string));
                    bankRollList.Tables[0].Columns.Add("FconStr", typeof(string));
                    bankRollList.Tables[0].Columns.Add("URL", typeof(string));

                    foreach (DataRow dr in bankRollList.Tables[0].Rows)
                    {
                        switch (dr["Ftype"].ToString())
                        {
                            case "1":
                                dr["FtypeText"] = "入";
                                break;
                            case "2":
                                dr["FtypeText"] = "出";
                                break;
                            case "3":
                                dr["FtypeText"] = "冻结";
                                break;
                            case "4":
                                dr["FtypeText"] = "解冻";
                                break;
                            default:
                                dr["FtypeText"] = dr["Ftype"].ToString();
                                break;
                        }

                        switch (dr["Fmemo"].ToString())
                        {
                            case "余额宝子账户提现":
                                dr["FmemoText"] = "提现";
                                break;
                            default:
                                dr["FmemoText"] = dr["Fmemo"].ToString();
                                break;
                        }

                        string duoFund = "";
                        string listid = dr["Flistid"].ToString();
                        if (dr["FmemoText"].ToString().Equals("基金申购"))
                        {
                            if (IfAnewBoughtFund(dr["Flistid"].ToString(), dr["Fcreate_time"].ToString()))
                            {
                                dr["FmemoText"] = "重新申购";
                            }

                            duoFund = QueryTradeFundInfoPro(spId, listid);//查询多基金转换
                            dr["FmemoText"] += duoFund;
                        }

                        if (dr["FmemoText"].ToString().Equals("提现"))
                        {
                            duoFund = QueryTradeFundInfoPro(spId, listid.Substring(listid.Length - 18));//查询多基金转换
                            dr["FmemoText"] += duoFund;
                        }

                        dr["FpaynumText"] = CFT.CSOMS.COMMLIB.CommUtil.FenToYuan(dr["Fpaynum"].ToString());
                        dr["FbalanceText"] = CFT.CSOMS.COMMLIB.CommUtil.FenToYuan(dr["Fbalance"].ToString());
                        dr["FconStr"] = CFT.CSOMS.COMMLIB.CommUtil.FenToYuan(dr["Fcon"].ToString());
                    }

                    return bankRollList.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("获取账户流水异常:{0}", ex.Message));
            }
            return null;
        }

        private string QueryTradeFundInfoPro(string spId, string listid)
        {
            string duoFund = "";
            var tradeFund = QueryTradeFundInfo(spId, listid);
            if (tradeFund != null && tradeFund.Rows.Count > 0)
            {
                string fundName = tradeFund.Rows[0]["Ffund_name"].ToString();
                string tmp = tradeFund.Rows[0]["Fpur_type"].ToString();
                if (tmp == "11")
                    duoFund = "(" + fundName + "转入)";
                if (tmp == "12")
                    duoFund = "(转出至" + fundName + ")";
            }
            return duoFund;
        }
    }
}
