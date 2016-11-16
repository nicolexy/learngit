using System;
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

        //理财通渠道字典
        static Dictionary<string, string> DicFchannel;
        //基金类型
        static Dictionary<string, string> DicFtype;

        //账户总金额
        private long _totalBalance = 0;
        public long totalBalance { get { return _totalBalance; } }
        //累计收益
        private long _totalProfit = 0;
        public long totalProfit { get { return _totalProfit; } }
        //理财通市值
        private decimal _totalLCTMarkValue = 0;
        public decimal totalLCTMarkValue { get { return _totalLCTMarkValue; } }
        //零钱市值
        private decimal _totalChangeMarkValue = 0;
        public decimal totalChangeMarkValue { get { return _totalChangeMarkValue; } }
        
        //余额+
        private long _BalancePlus = 0;
        public long BalancePlus { get { return _BalancePlus; } }

        //余额+基金
        private string _BalancePlus_fundName = "";
        public string BalancePlus_fundName { get { return _BalancePlus_fundName; } }

       
        static FundService()
        {
            DicFchannel = new Dictionary<string, string>()
            {
                {"1", "PC充值"},
                {"2", "微信充值"},
                {"3", "手Q充值"},
                {"68", "H5微信支付"},
                {"69", "零钱"},
                {"88", "QQ钱包"},
                {"97", "PC网银"},
                {"98", "PC微信支付"},
                {"99", "PCQQ钱包"}
            };

            DicFtype = new Dictionary<string, string>()
            {
                {"1", "货币"},
                {"2", "定期"},
                {"3", "保险"},
                {"4", "指数"},
                {"5", "非标"},
                {"6", "报价回购"},
                {"7", "投连险"}
            };
        }

        public bool isSpecialFund(string fund_code, string spid)
        {
            Hashtable htFund = new Hashtable();
            htFund.Add("110020", "1238657101");//易方达沪深300基金
            htFund.Add("481009", "1241176901");//工银沪深300基金
            htFund.Add("160706", "1239537001");//嘉实沪深300基金
            htFund.Add("160119", "1249279401");//南方中证500交易型开放式指数证券投资基金联接基金
            htFund.Add("000948", "1249643101");//华夏沪港通恒生交易型开放式指数证券投资基金联接基金
            htFund.Add("110031", "1250802101");//易方达恒生中国企业交易型开放式指数证券投资基金联接基金

            if (htFund.Contains(fund_code))
            {
                if (htFund[fund_code].ToString() == spid)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否保险
        /// </summary>
        /// <param name="fund_code"></param>
        /// <param name="spid"></param>
        /// <returns></returns>
        public bool isInsurance(string fund_code, string spid)
        {
            var GuangDadic = new Dictionary<string, string>()
            {
                { "1269516501" , "9000003" }, //光大永明光明财富2号A款年金保险（投资连结型）
                { "1269516701" , "9000004" }, //光大永明光明财富2号B款年金保险（投资连结型）
            };
            return GuangDadic.ContainsKey(spid) && GuangDadic[spid] == fund_code;
        }
        /// <summary>
        /// 零钱商户
        /// </summary>
        /// <param name="spid"></param>
        /// <returns></returns>
        public bool IsSmallChange(string spid)
        {
            //1295277801|1294122801|1294122101|1306760801 
            List<string> SmallChangedic = new List<string>() { "1295277801", "1294122801", "1294122101", "1306760801" };

           return SmallChangedic.Any(p => p == spid);
        }

        public DataTable GetFundTradeLog(string qqid, int istr, int imax)
        {
            if (string.IsNullOrEmpty(qqid))
                throw new ArgumentNullException("qqid");
            string Tradeid = GetTradeIdByUIN(qqid);
            return new SafeCard().GetFundTradeLog(Tradeid, qqid, istr, imax);
        }

        public DataTable GetPayCardInfo(string qqid) 
        {
            if (string.IsNullOrEmpty(qqid))
                throw new ArgumentNullException("qqid");
            string Tradeid = GetTradeIdByUIN(qqid);
            DataTable dt=new SafeCard().GetPayCardInfo(Tradeid,qqid);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("bank_type_name", typeof(string));
                dt.Rows[0]["bank_type_name"] = BankIO.QueryBankName(dt.Rows[0]["Fbank_type"].ToString());
            }
            return dt;
        }

        public bool GetFundSupportBank(string bank_type)
        {
            return new SafeCard().GetFundSupportBank(bank_type);
        }

        public DataTable GetFundRollList(string u_QQID, DateTime u_BeginTime, DateTime u_EndTime, string Fcurtype, int istr, int imax, int Ftype)
        {
            if (string.IsNullOrEmpty(u_QQID))
                throw new ArgumentNullException("u_QQID");
            string Tradeid = GetTradeIdByUIN(u_QQID);
            DataTable dt = new FundRoll().QueryFundRollList(Tradeid, u_QQID, u_BeginTime, u_EndTime, Fcurtype, istr, imax, Ftype);
            return dt;
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
            return new FundAccountInfo().GetTradeIdByUIN(uin);
        }

        public DataTable GetUserFundAccountInfo(string uin)
        {
            if (string.IsNullOrEmpty(uin))
                throw new ArgumentNullException("uin");
            try
            {
                string Tradeid = GetTradeIdByUIN(uin);

                var fundAccountInfo = new FundAccountInfo().QueryFundAccountRelationInfo(Tradeid, uin);

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
                throw new Exception(string.Format("基金账户信息查询异常：{0}", ex.ToString()));
            }
        }

        /// <summary>
        /// 获取用户的基金概要（余额，收益）
        /// </summary>
        /// <param name="uin"></param>
        /// <returns></returns>
        public DataTable GetUserFundSummary(string uin)
        {
            /*
            var tradeId = GetTradeIdByUIN(uin);
            if (string.IsNullOrEmpty(tradeId))
                throw new Exception(string.Format("{0}没有对应的基金账户，查询不到TradeId", uin));

            //获取用户的所有基金
           // var userFundsTable = new FundProfit().QueryProfitStatistic(tradeId);
            var userFundsTable = new FundProfit().QueryFundBind(tradeId);//查商户号

            if (userFundsTable == null || userFundsTable.Rows.Count < 1)
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
            userFundsTable.Columns.Add("markValue", typeof(string));//市值

            userFundsTable.Columns.Add("profitText", typeof(string));
            userFundsTable.Columns.Add("balanceText", typeof(string)); //余额
            userFundsTable.Columns.Add("conText", typeof(string));//冻结金额
            userFundsTable.Columns.Add("close_flagText", typeof(string));
            userFundsTable.Columns.Add("transfer_flagText", typeof(string));
            userFundsTable.Columns.Add("buy_validText", typeof(string));

            //基金类型
            userFundsTable.Columns.Add("Ftype", typeof(string));//基金类型
            userFundsTable.Columns.Add("FType_Str", typeof(string));//基金类型

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
                    item["Ftype"] = FundInfoBySpidTable.Rows[0]["Ftype"].ToString();
                }

                //balance
                int Fcurtype = 0;
                if (!int.TryParse(item["Fcurtype"].ToString().Trim(), out Fcurtype)) 
                {
                    throw new Exception("查询基金币种类型失败：Fspid=" + item["Fspid"].ToString());
                }

                subAccountInfoTable = new LCTBalanceService().QuerySubAccountInfo(uin, Fcurtype);
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
                try
                {
                    //查累计收益
                    var ProfitTable = new FundProfit().QueryProfitStatistic(tradeId, Fcurtype);

                    if (ProfitTable != null && ProfitTable.Rows.Count > 0)
                    {
                        if (item["Facct_type"] == "2") 
                        {
                            //查余额+累计收益
                            item["Ftotal_profit"] = ProfitTable.Rows[0]["Fstandby7"].ToString();
                        }
                        else
                        {
                            //普通基金累计收益
                            item["Ftotal_profit"] = ProfitTable.Rows[0]["Ftotal_profit"].ToString();
                        }
                    }
                    else
                    {
                        item["Ftotal_profit"] = "0";
                    }
                }
                catch
                {

                }
                string fund_code = item["fund_code"].ToString();
                string spid = item["Fspid"].ToString();
                string balance = item["balance"].ToString();
                try
                {
                    item["markValue"] = GetMarkValueForFund(fund_code, spid, balance);
                }
                catch (Exception ex)
                {
                    throw new Exception("查询市值异常：" + ex.Message);
                }


                #region 字段转义
                //基金类型转义
                if (IsSmallChange(item["Fspid"].ToString()))
                {
                    item["FType_Str"] = "零钱";
                }
                else
                {
                    item["FType_Str"] = DicFtype[item["FType"].ToString()];
                }

                switch (item["close_flag"].ToString())
                {
                    case "1":
                        item["close_flagText"] = "不封闭";
                        break;
                    case "2":
                        item["close_flagText"] = "封闭";
                        break;
                    case "3":
                        item["close_flagText"] = "半封闭";
                        break;
                }
                switch (item["transfer_flag"].ToString())
                {
                    case "0":
                        item["transfer_flagText"] = "不支持转入、转出";
                        break;
                    case "1":
                        item["transfer_flagText"] = "支持转入不支持转出";
                        break;
                    case "2":
                        item["transfer_flagText"] = "支持转出不支持转入";
                        break;
                    case "3":
                        item["transfer_flagText"] = "同时支持转入和转出";
                        break;
                }
                switch (item["buy_valid"].ToString())
                {
                    case "1":
                        item["buy_validText"] = "支持申购";
                        break;
                    case "2":
                        item["buy_validText"] = "支持认购";
                        break;
                    case "4":
                        item["buy_validText"] = "支持申购/认购";
                        break;
                }
                #endregion

                #region  统计收益总和，和余额总和

                //_totalBalance += long.Parse(item["balance"].ToString());
                //_totalProfit += long.Parse(item["Ftotal_profit"].ToString());
                //if (IsSmallChange(item["Fspid"].ToString()))
                //{
                //    _totalChangeMarkValue += decimal.Parse(item["markValue"].ToString());

                //}
                //else
                //{
                //    _totalLCTMarkValue += decimal.Parse(item["markValue"].ToString());
                //}

                long temp_totalBalance = 0;
                long temp_totalProfit = 0;
                decimal temp_totalChangeMarkValue = 0;
                decimal temp_totalLCTMarkValue = 0;

                long.TryParse(item["balance"].ToString().Trim(), out temp_totalBalance);
                long.TryParse(item["Ftotal_profit"].ToString().Trim(), out temp_totalProfit);

                if (IsSmallChange(item["Fspid"].ToString()))
                {
                    decimal.TryParse(item["markValue"].ToString().Trim(), out temp_totalChangeMarkValue);
                }
                else
                {
                    decimal.TryParse(item["markValue"].ToString().Trim(), out temp_totalLCTMarkValue);
                }

                if (item["Facct_type"].ToString() == "2") 
                {
                    //余额＋基金在用户各基金中最多只有一个，所有可以在循环体中赋值，而不用+=；
                    _BalancePlus = temp_totalBalance;
                    _BalancePlus_fundName = item["fundName"].ToString();
                }


                _totalBalance += temp_totalBalance;
                _totalProfit += temp_totalProfit;
                _totalChangeMarkValue += temp_totalChangeMarkValue;
                _totalLCTMarkValue += temp_totalLCTMarkValue;
                #endregion
            }

            return userFundsTable;

            */

            var tradeId = GetTradeIdByUIN(uin);
            if (string.IsNullOrEmpty(tradeId))
                throw new Exception(string.Format("{0}没有对应的基金账户，查询不到TradeId", uin));

            //获取用户的所有基金
            // var userFundsTable = new FundProfit().QueryProfitStatistic(tradeId);
            var dt = new FundProfit().QueryFundBind(tradeId);//查商户号
            if (dt == null || dt.Rows.Count < 1)
                throw new Exception(string.Format("{0}没有申购的基金", uin));

            var userFundsTable = new DataTable();

            userFundsTable.Columns.Add("Fspid", typeof(string));

            userFundsTable.Columns.Add("balance", typeof(string));
            userFundsTable.Columns.Add("fundName", typeof(string));
            userFundsTable.Columns.Add("Fcurtype", typeof(string));
            userFundsTable.Columns.Add("Ftotal_profit", typeof(string));
            userFundsTable.Columns.Add("con", typeof(string));//冻结金额
            userFundsTable.Columns.Add("fund_code", typeof(string));
            userFundsTable.Columns.Add("close_flag", typeof(string));
            userFundsTable.Columns.Add("transfer_flag", typeof(string));
            userFundsTable.Columns.Add("buy_valid", typeof(string));
            userFundsTable.Columns.Add("markValue", typeof(string));//市值

            userFundsTable.Columns.Add("profitText", typeof(string));
            userFundsTable.Columns.Add("balanceText", typeof(string)); //余额
            userFundsTable.Columns.Add("conText", typeof(string));//冻结金额
            userFundsTable.Columns.Add("close_flagText", typeof(string));
            userFundsTable.Columns.Add("transfer_flagText", typeof(string));
            userFundsTable.Columns.Add("buy_validText", typeof(string));

            //基金类型
            userFundsTable.Columns.Add("Ftype", typeof(string));//基金类型
            userFundsTable.Columns.Add("FType_Str", typeof(string));//基金类型

            var cftAccountBLLService = new CFTAccountModule.AccountService();
            DataTable subAccountInfoTable;

            foreach (DataRow item in dt.Rows)
            {
                //  (); (item["Fspid"].ToString());//查Fcurtype及基金名称
                DataTable dtFundInfo = new FundInfoData().QueryAllFundInfo();

                if (dtFundInfo == null || dtFundInfo.Rows.Count == 0)
                {
                    continue;
                }
                DataRow[] drsFundInfo = dtFundInfo.Select(" Fspid='" + item["Fspid"].ToString() + "'");

                foreach (DataRow itemFundInfo in drsFundInfo)
                {
                    DataRow dr = userFundsTable.NewRow();
                    dr["Fspid"] = item["Fspid"].ToString();

                    dr["Fcurtype"] = itemFundInfo["Fcurtype"].ToString();
                    dr["fundName"] = itemFundInfo["Ffund_name"].ToString();
                    dr["fund_code"] = itemFundInfo["Ffund_code"].ToString();
                    dr["close_flag"] = itemFundInfo["Fclose_flag"].ToString();
                    dr["transfer_flag"] = itemFundInfo["Ftransfer_flag"].ToString();
                    dr["buy_valid"] = itemFundInfo["Fbuy_valid"].ToString();
                    dr["Ftype"] = itemFundInfo["Ftype"].ToString();

                    #region  查询份额
                    int Fcurtype = 0;
                    if (!int.TryParse(dr["Fcurtype"].ToString().Trim(), out Fcurtype))
                    {
                        throw new Exception("查询币种类型失败，fund_code：" + itemFundInfo["Ffund_name"].ToString());
                    }
                    subAccountInfoTable = new LCTBalanceService().QuerySubAccountInfo(uin, Fcurtype);
                    if (subAccountInfoTable == null || subAccountInfoTable.Rows.Count < 1)
                    {
                        dr["balance"] = "0";
                        dr["con"] = "0";
                    }
                    else
                    {
                        dr["balance"] = subAccountInfoTable.Rows[0]["Fbalance"].ToString();
                        dr["con"] = subAccountInfoTable.Rows[0]["Fcon"].ToString();
                    }
                    #endregion

                    #region  查询收益
                    //fundname

                    //    item["fundName"] = GetAllFundInfo().Where(i => i.CurrencyType == int.Parse(item["Fcurtype"].ToString())).First().Name;
                    try
                    {
                        //查累计收益
                        var ProfitTable = new FundProfit().QueryProfitStatistic(tradeId, Fcurtype);

                        if (ProfitTable != null && ProfitTable.Rows.Count > 0)
                        {
                            if (dr["Facct_type"].ToString() == "2")
                            {
                                //查余额+累计收益
                                dr["Ftotal_profit"] = ProfitTable.Rows[0]["Fstandby7"].ToString();
                            }
                            else
                            {
                                //普通基金累计收益
                                dr["Ftotal_profit"] = ProfitTable.Rows[0]["Ftotal_profit"].ToString();
                            }
                        }
                        else
                        {
                            dr["Ftotal_profit"] = "0";
                        }
                    }
                    catch
                    {

                    }
                    #endregion

                    #region 查询市值
                    string fund_code = dr["fund_code"].ToString();
                    string spid = dr["Fspid"].ToString();
                    string balance = dr["balance"].ToString();
                    try
                    {
                        dr["markValue"] = GetMarkValueForFund(tradeId,fund_code, spid, balance);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("查询市值异常：" + ex.Message);
                    }
                    #endregion

                    #region 字段转义
                    //基金类型转义
                    if (IsSmallChange(dr["Fspid"].ToString()))
                    {
                        dr["FType_Str"] = "零钱";
                    }
                    else
                    {
                        dr["FType_Str"] = DicFtype[dr["FType"].ToString()];
                    }

                    switch (dr["close_flag"].ToString())
                    {
                        case "1":
                            dr["close_flagText"] = "不封闭";
                            break;
                        case "2":
                            dr["close_flagText"] = "封闭";
                            break;
                        case "3":
                            dr["close_flagText"] = "半封闭";
                            break;
                    }
                    switch (dr["transfer_flag"].ToString())
                    {
                        case "0":
                            dr["transfer_flagText"] = "不支持转入、转出";
                            break;
                        case "1":
                            dr["transfer_flagText"] = "支持转入不支持转出";
                            break;
                        case "2":
                            dr["transfer_flagText"] = "支持转出不支持转入";
                            break;
                        case "3":
                            dr["transfer_flagText"] = "同时支持转入和转出";
                            break;
                    }
                    switch (dr["buy_valid"].ToString())
                    {
                        case "1":
                            dr["buy_validText"] = "支持申购";
                            break;
                        case "2":
                            dr["buy_validText"] = "支持认购";
                            break;
                        case "4":
                            dr["buy_validText"] = "支持申购/认购";
                            break;
                    }

                    #endregion

                    #region  统计收益总和，和余额总和
                    long temp_totalBalance = 0;
                    long temp_totalProfit = 0;
                    decimal temp_totalChangeMarkValue = 0;
                    decimal temp_totalLCTMarkValue = 0;

                    long.TryParse(dr["balance"].ToString().Trim(), out temp_totalBalance);
                    long.TryParse(dr["Ftotal_profit"].ToString().Trim(), out temp_totalProfit);

                    if (IsSmallChange(dr["Fspid"].ToString()))
                    {
                        decimal.TryParse(dr["markValue"].ToString().Trim(), out temp_totalChangeMarkValue);
                    }
                    else
                    {
                        decimal.TryParse(dr["markValue"].ToString().Trim(), out temp_totalLCTMarkValue);
                    }

                    if (dr["Facct_type"].ToString() == "2")
                    {
                        //余额＋基金在用户各基金中最多只有一个，所有可以在循环体中赋值，而不用+=；
                        _BalancePlus = temp_totalBalance;
                        _BalancePlus_fundName = dr["fundName"].ToString();
                    }

                    _totalBalance += temp_totalBalance;
                    _totalProfit += temp_totalProfit;
                    _totalChangeMarkValue += temp_totalChangeMarkValue;
                    _totalLCTMarkValue += temp_totalLCTMarkValue;
                    #endregion

                    userFundsTable.Rows.Add(dr);
                }
            }
            return userFundsTable;
        }

        private  string GetMarkValueForFund(string tradeId, string fund_code, string spid, string balance)
        {
            string balanceStr = MoneyTransfer.FenToYuan(balance);//页面上也转了一次，因为API接口是分，所以函数不能随便改必须兼容
            if (isSpecialFund(fund_code, spid)) //易方达沪深300基金的市值=基金份额*每日单位净值 其他基金市值=份额
            {
                DateTime dateTime = System.DateTime.Now.AddDays(-1);
                //查昨天的单位净值，并且是交易日
                string weekDay = Convert.ToInt16(dateTime.DayOfWeek).ToString();
                while (weekDay == "7" || weekDay == "6")
                {
                    dateTime = dateTime.AddDays(-1);
                    weekDay = Convert.ToInt16(dateTime.DayOfWeek).ToString();
                }
                //string date = dateTime.ToString("yyyMMdd");
                var fundProfit = new FundProfit().QueryFundProfitRate(tradeId, spid, fund_code);
                decimal F1day_profit_rate = 0.0M;

                if (fundProfit != null && fundProfit.Rows.Count > 0)
                {
                    if (fundProfit.Rows[0]["F1day_profit_rate"] != null && fundProfit.Rows[0]["F1day_profit_rate"] != DBNull.Value && fundProfit.Rows[0]["F1day_profit_rate"].ToString() != string.Empty)
                    {
                        decimal.TryParse(fundProfit.Rows[0]["F1day_profit_rate"].ToString(), out F1day_profit_rate);
                    }
                }
                decimal profitPerTenThousand = F1day_profit_rate / 10000;
                decimal balanceDec = 0.0M;
                decimal.TryParse(balanceStr, out balanceDec);
                return (balanceDec * profitPerTenThousand).ToString("f2");
            }
            else
                return balanceStr;
        }

        /// 获取用户的总份额
        public long GetUserFundTotal(string uin)
        {
            //总帐户份额
            var summaryTable = GetUserFundSummary(uin);

            //统计份额总和
            long totalBalance = 0;
            if (summaryTable != null && summaryTable.Rows.Count > 0)
            foreach (DataRow item in summaryTable.Rows)
            {
                totalBalance += long.Parse(item["balance"].ToString());
            }
            return totalBalance;
        }

        /// 获取用户的总市值
        public decimal GetUserFundMarkValue(string uin)
        {
            //总帐户总市值
            var summaryTable = GetUserFundSummary(uin);

            //统计市值总和
            decimal totalMarkValue = 0;
            if (summaryTable != null && summaryTable.Rows.Count > 0)
                foreach (DataRow item in summaryTable.Rows)
                {
                    totalMarkValue += decimal.Parse(item["markValue"].ToString());
                }
            return totalMarkValue;
        }

        public bool IfAnewBoughtFund(string tradeId, string listid, string time)
        {
            if (string.IsNullOrEmpty(listid))
                throw new ArgumentNullException("listid");
            return new FundInfoData().QueryIfAnewBoughtFund(tradeId, listid, DateTime.Parse(time));
        }
        //查询基金交易单
        public DataTable QueryTradeFundInfo(string tradeId,string spid, string listid)
        {
            return new FundInfoData().QueryTradeFundInfo(tradeId, spid, listid);
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
                 tbCloseFundRollList.Columns.Add("Fend_sell_type_str", typeof(string));//到期操作

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
                             case "5":
                                 dr["Fuser_end_type_str"] = "自动延期，延期后可随时赎回";
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

                         #region 支付渠道
                         var Fchannel = FchannelAnalysis((string)dr["Fchannel_id"]);
                         dr["Fchannel_id_str"] = COMMLIB.CommUtil.DbtypeToPageContent(Fchannel, DicFchannel, "");

                         //switch ()
                         //{
                         //    case "1":
                         //        dr["Fchannel_id_str"] = "财付通网站";
                         //        break;
                         //    case "2":
                         //        dr["Fchannel_id_str"] = "微信";
                         //        break;
                         //    case "3":
                         //        dr["Fchannel_id_str"] = "手Q";
                         //        break;
                         //    default:
                         //        dr["Fchannel_id_str"] = dr["Fchannel_id"].ToString();
                         //        break;
                         //}
                         #endregion    

                         switch (dr["Fend_sell_type"].ToString())
                         {
                             case "1":
                                 dr["Fend_sell_type_str"] = "赎回用户提现到银行卡";
                                 break;
                             case "2":
                                 dr["Fend_sell_type_str"] = "赎回用于转换另一只基金";
                                 break;
                             case "3":
                                 dr["Fend_sell_type_str"] = "赎回用户转余额账户";
                                 break;
                             default:
                                 dr["Fend_sell_type_str"] = dr["Fend_sell_type"].ToString();
                                 break;
                         }
                     }

                 COMMLIB.CommUtil.FenToYuan_Table(tbCloseFundRollList, "Fstart_total_fee", "Fstart_total_fee_str");
                 COMMLIB.CommUtil.FenToYuan_Table(tbCloseFundRollList, "Fcurrent_total_fee", "Fcurrent_total_fee_str");
                 COMMLIB.CommUtil.FenToYuan_Table(tbCloseFundRollList, "Fend_tail_fee", "Fend_tail_fee_str");
             }

             return tbCloseFundRollList;
        }

        //  //通过商户号查询基金公司信息
        //public DataTable QueryFundInfoBySpid(string spid)
        //{
        //    return new FundInfoData().QueryFundInfoBySpid(spid);
        //}
       
        //查询用户余额收益情况明细  -- 将页面的逻辑封装在接口中
        public DataTable BindProfitList(string tradeId, string beginDateStr, string endDateStr, int currencyType = -1, string spId = "", int currentPageIndex = 0, int pageSize = 5, string fund_code = "")
        {
            try
            {
                DataTable profits = new FundProfit().QueryProfitRecord(tradeId, beginDateStr, endDateStr, currencyType, spId, currentPageIndex * pageSize, pageSize);
                if (profits != null && profits.Rows.Count > 0)
                {
                    profits.Columns.Add("Fvalid_money_str", typeof(String));//收益本金额
                    profits.Columns.Add("Fpur_typeName", typeof(String));//科目
                    profits.Columns.Add("F7day_profit_rate_str", typeof(String));//七日年化收益
                    profits.Columns.Add("Fprofit_str", typeof(String));//收益金额
                    profits.Columns.Add("Fspname", typeof(String));//基金公司名
                    profits.Columns.Add("Fprofit_per_ten_thousand", typeof(string));//万份收益

                    //以下字段只有易方达沪深300才会展示的字段
                    profits.Columns.Add("fund_value", typeof(string));//单位净值
                    profits.Columns.Add("Sday_profit_rate_str", typeof(string));//日涨跌
                    profits.Columns.Add("fund_balance", typeof(string));//基金份额
                    profits.Columns.Add("mark_value", typeof(string));//市值
                    profits.Columns.Add("EstimateFprofit", typeof(string));//预估收益

                    COMMLIB.CommUtil.FenToYuan_Table(profits, "Fvalid_money", "Fvalid_money_str");
                    COMMLIB.CommUtil.FenToYuan_Table(profits, "Fprofit", "Fprofit_str");

                    var fundall=FundService.GetAllFundInfo();
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

                            dr["Fprofit_per_ten_thousand"] = profitPerTenThousand.ToString("N4");
                        }

                        try
                        {
                            //基金公司名
                            dr["Fspname"] = fundall.Where(i => i.SPId == dr["Fspid"].ToString()).First().SPName;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("查询spid:" + dr["Fspid"].ToString() + "的基金公司名异常！" + ex.ToString());
                        }

                        if (isSpecialFund(fund_code, spId)) //易方达沪深300基金
                        {
                            dr["fund_value"] = dr["Fprofit_per_ten_thousand"].ToString();
                            dr["fund_balance"] = dr["Fvalid_money_str"].ToString();
                            try
                            {
                                var fundProfit = new FundProfit().QueryFundProfitRate(tradeId, spId, fund_code);

                                if (fundProfit != null && fundProfit.Rows.Count > 0)
                                {
                                    DataRow row = fundProfit.Rows[0];
                                    //日涨跌
                                    if (!(row["F7day_profit_rate"] is DBNull))
                                    {
                                        string tmp = row["F7day_profit_rate"].ToString();
                                        if (!string.IsNullOrEmpty(tmp))
                                        {

                                            decimal d = (decimal)(Int64.Parse(tmp)) / 10000;
                                            dr["Sday_profit_rate_str"] = d.ToString("P2");
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("查询spid:" + dr["Fspid"].ToString() + "  fund_code：" + fund_code + " 的日涨跌异常！" + ex.Message);
                            }

                            decimal fund_value = 0.0M;
                            decimal.TryParse(dr["fund_value"].ToString(), out fund_value);
                            decimal fund_balance = 0.0M;
                            decimal.TryParse(dr["fund_balance"].ToString(), out fund_balance);
                            dr["mark_value"] = (fund_balance * fund_value).ToString("f2");//份额*单位净值
                        }
                        else
                        {
                            dr["fund_value"] = "";
                            dr["Sday_profit_rate_str"] = "";
                            dr["fund_balance"] = "";
                            dr["mark_value"] = "";
                        }


                        #region 查询预估收益
                        try
                        {
                            string fday = dr["Fday"].ToString();
                            string Fprofit = "0";
                            // DataTable dtFprofit = new FundInfoData().QueryEstimateProfit("20140117000094000", "9000004", fday, fday, 0, 1);
                            DataTable dtEstimateFprofit = new FundInfoData().QueryEstimateProfit(tradeId, fund_code, fday, fday, 0, 1);
                            if (dtEstimateFprofit != null && dtEstimateFprofit.Rows.Count > 0)
                            {
                                Fprofit = COMMLIB.CommUtil.FenToYuan(Convert.ToDouble(dtEstimateFprofit.Rows[0]["Fprofit"]));
                            }
                            dr["EstimateFprofit"] = Fprofit;

                        }
                        catch (Exception exc)
                        {
                            dr["EstimateFprofit"] ="查询预估收益失败："+ exc.Message;
                        }
                        #endregion
                    }

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
        public DataTable BindBankRollListNotChildren(string qqId, string curtype, DateTime beginDate, DateTime endDate, int pageIndex = 0, int pageMax = 5, int redirectionType = 0,string spid="",string fund_code="")
        {
            try
            {
                if (string.IsNullOrEmpty(qqId))
                    throw new ArgumentNullException("qqId");
                string Tradeid = GetTradeIdByUIN(qqId);
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
                    bankRollList.Columns.Add("Fbank_type_str", typeof(string));
                    bankRollList.Columns.Add("Fchannel_str", typeof(string));

                    //这两个字段只有在易方达沪深300基金才有值
                    bankRollList.Columns.Add("charge_fee", typeof(string));//手续费分
                    bankRollList.Columns.Add("charge_fee_str", typeof(string));//手续费
                    bankRollList.Columns.Add("fund_balance", typeof(string));//份额
                    bankRollList.Columns.Add("Remark", typeof(string));//备注（是否余额+）
                    if (bankRollList.Rows.Count > 0)
                    {
                        COMMLIB.CommUtil.FenToYuan_Table(bankRollList, "Ftotal_fee", "Ftotal_fee_str");

                        foreach (DataRow dr in bankRollList.Rows)
                        {
                            dr["Fsub_trans_id_str"] = "104" + dr["Fsub_trans_id"].ToString();
                            string Fpur_type = dr["Fpur_type"].ToString();
                            string Floading_type = dr["Floading_type"].ToString();
                            string Fpurpose = dr["Fpurpose"].ToString();
                            var tradeFund = QueryTradeFundInfo(Tradeid, spid, dr["Flistid"].ToString());
                            string Fbusiness_type = "";
                            if (tradeFund != null && tradeFund.Rows.Count > 0)
                            {
                                Fbusiness_type = tradeFund.Rows[0]["Fbusiness_type"].ToString();
                            }
                            #region 存取
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
                            #endregion

                            #region 赎回用途
                            if (Fpur_type == "4" && Floading_type == "1")
                            {
                                dr["Floading_type_str"] = "快速赎回到银行卡";
                            }
                            else if (Fpur_type == "4" && Floading_type == "0" && (Fpurpose == "0" || Fpurpose == "1" || Fpurpose == "2"))
                            {
                                dr["Floading_type_str"] = "普通赎回到银行卡";
                            }
                            else if (Fpurpose == "7" && Fpur_type == "12")
                            {
                                dr["Floading_type_str"] = "赎回到余额";
                            }
                            else if (Fpurpose == "9")
                            {
                                dr["Floading_type_str"] = "T+1赎回到余额";
                            }
                            else if (Fpurpose == "8")
                            {
                                dr["Floading_type_str"] = "赎回提现给商户";
                            }

                            else if (Fpur_type == "12" && Fpurpose == "3" && Fbusiness_type == "0")
                            {
                                dr["Floading_type_str"] = "转投";
                            }
                            else if (Fpur_type == "12" && Fpurpose == "3" && Fbusiness_type == "3")
                            {
                                dr["Floading_type_str"] = "预约转换";
                            }
                            else if (Fpur_type == "12" && Fpurpose == "3" && Fbusiness_type == "5")
                            {
                                dr["Floading_type_str"] = "预约募集";
                            }

                            else
                            {
                                //除了出其他都没有赎回方式
                                dr["Floading_type_str"] = "";
                            }

                            //零钱赎回用途
                            if (Fpur_type == "4" && IsSmallChange(spid))
                            {
                                // 保险箱赎回判断
                                if ("1" == Floading_type)
                                {
                                    dr["Floading_type_str"] = "快速取出到零钱"; //"T+0赎回赎回到零钱";
                                }
                                else if ("0" == Floading_type)
                                {
                                    dr["Floading_type_str"] = "普通取出到零钱";// "T+1赎回到零钱";
                                }
                                else
                                {
                                    dr["Floading_type_str"] = "未知类型";
                                }
                            }
                            #endregion

                            #region 注释
                            /*
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
                            */
                            
                            #endregion

                            dr["Fbank_type_str"] = BankIO.QueryBankName(dr["Fbank_type"].ToString());

                            #region 易方达沪深300基金
                            if (isSpecialFund(fund_code, spid)) //易方达沪深300基金
                            {
                                //原存取状态“入”就是金额
                                //原存取状态“出”就是份额
                                string type = dr["FtypeText"].ToString();
                                if (type == "入")
                                {
                                    dr["fund_balance"] = "";
                                }
                                else if (type == "出")
                                {
                                    dr["fund_balance"] = dr["Ftotal_fee_str"].ToString();
                                    dr["Ftotal_fee_str"] = "";
                                }

                                //try
                                //{
                                //    var tradeFund = QueryTradeFundInfo(spid, dr["Flistid"].ToString());
                                if (tradeFund != null && tradeFund.Rows.Count > 0)
                                {
                                    dr["charge_fee"] = tradeFund.Rows[0]["Fcharge_fee"].ToString();
                                }
                                //}
                                //catch (Exception ex)
                                //{
                                //    throw new Exception("查询手续费异常：" + ex.Message);
                                //}
                            } 
                            #endregion

                            #region 支付渠道

                            if (Fpur_type == "1" && (string)dr["Fstate"] == "3")
                            {
                                var Fchannel = FchannelAnalysis((string)dr["Fchannel_id"]);
                                dr["Fchannel_str"] = COMMLIB.CommUtil.DbtypeToPageContent(Fchannel, DicFchannel, "");
                            }
                        
                            #endregion    

                            #region 是否余额＋

                            if (dr["Fbusi_flag"].ToString() == "1" || dr["Fbusi_flag"].ToString() == "2")
                            {

                                dr["Remark"] = "余额+";
                            }
                            #endregion

                        }

                        #region 状态
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
                        ht.Add("12", "支付完成，到基金公司发起申购成功，但申购结果待确认");
                        ht.Add("13", "到基金公司发起赎回成功，但赎回结果待确认");
                        ht.Add("20", "作废");

                        COMMLIB.CommUtil.DbtypeToPageContent(bankRollList, "Fstate", "Fstate_str", ht); 
                        #endregion

                        COMMLIB.CommUtil.FenToYuan_Table(bankRollList, "charge_fee", "charge_fee_str");
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
                string Tradeid = GetTradeIdByUIN(qqId);
                if (bankRollList!=null&&bankRollList.Tables.Count>0 && bankRollList.Tables.Count > 0)
                {
                    bankRollList.Tables[0].Columns.Add("FpaynumText", typeof(string));
                    bankRollList.Tables[0].Columns.Add("FbalanceText", typeof(string));
                    bankRollList.Tables[0].Columns.Add("FtypeText", typeof(string));
                    bankRollList.Tables[0].Columns.Add("FmemoText", typeof(string));
                    bankRollList.Tables[0].Columns.Add("FconStr", typeof(string));
                    bankRollList.Tables[0].Columns.Add("URL", typeof(string));
                    bankRollList.Tables[0].Columns.Add("fetchid", typeof(string));//提现单号

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

                        string listid = dr["Flistid"].ToString();
                        if (dr["FmemoText"].ToString().Equals("基金申购"))
                        {
                            if (IfAnewBoughtFund(Tradeid,dr["Flistid"].ToString(), dr["Fcreate_time"].ToString()))
                            {
                                dr["FmemoText"] = "重新申购";
                            }

                            DataTable tradeFund= QueryTradeFundInfoPro(Tradeid,spId, listid);//查询多基金转换
                            if (tradeFund != null && tradeFund.Rows.Count > 0)
                            {
                                dr["FmemoText"] += tradeFund.Rows[0]["duoFund"].ToString();
                                dr["fetchid"] += tradeFund.Rows[0]["Ffetchid"].ToString();
                            }
                        }

                        if (dr["FmemoText"].ToString().Equals("提现"))
                        {
                            DataTable tradeFund = QueryTradeFundInfoPro(Tradeid,spId, listid.Substring(listid.Length - 18));//查询多基金转换
                            if (tradeFund != null && tradeFund.Rows.Count > 0)
                            {
                                dr["FmemoText"] += tradeFund.Rows[0]["duoFund"].ToString();
                                dr["fetchid"] += tradeFund.Rows[0]["Ffetchid"].ToString();
                            }
                        }

                        dr["FpaynumText"] = CFT.CSOMS.COMMLIB.CommUtil.FenToYuan(dr["Fpaynum"].ToString()).Replace("元","");
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

        private DataTable QueryTradeFundInfoPro(string Tradeid,string spId, string listid)
        {
            var tradeFund = QueryTradeFundInfo(Tradeid,spId, listid);
            if (tradeFund != null && tradeFund.Rows.Count > 0)
            {
                tradeFund.Columns.Add("duoFund", typeof(string));
                string fundName = tradeFund.Rows[0]["Ffund_name"].ToString();
                string tmp = tradeFund.Rows[0]["Fpur_type"].ToString();
                if (tmp == "11")
                    tradeFund.Rows[0]["duoFund"] = "(" + fundName + "转入)";
                if (tmp == "12")
                    tradeFund.Rows[0]["duoFund"] = "(转出至" + fundName + ")";
            }
            return tradeFund;
        }

        /// <summary>
        /// 定期修改到期策略
        /// </summary>
        /// <param name="Trade_id">基金交易账户对应id</param>
        /// <param name="Fund_code">基金代码</param>
        /// <param name="Close_listid">定期产品用户交易记录表的自增主键</param>
        /// <param name="user_end_type">用户指定的到期申购/赎回策略</param>
        /// <param name="end_sell_type">到期操作</param>
        /// <param name="client_ip">操作ip</param>
        /// <returns></returns>
        public bool AlterEndStrategy(string Trade_id, string Fund_code, long Close_listid, int user_end_type, int end_sell_type, string client_ip,string spid)
        {
           return new FundProfit().AlterEndStrategy(Trade_id, Fund_code, Close_listid, user_end_type, end_sell_type, client_ip,spid);
        }

        #region 理财通定投和定赎
        public DataTable Get_DT_fundBuyPlan(string Tradeid, string uid, int offset, int limit)
        {
            FundInfoData data = new FundInfoData();
            DataTable dt = data.Get_DT_fundBuyPlan(Tradeid,uid, offset, limit);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("Ffund_name", typeof(string));
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Ffund_name"] = GetFundName(dr["Fspid"].ToString(), dr["Ffund_code"].ToString());
                    dr["Ftotal_plan_fee"] = MoneyTransfer.FenToYuan(dr["Ftotal_plan_fee"].ToString());
                    dr["Fplan_fee"] = MoneyTransfer.FenToYuan(dr["Fplan_fee"].ToString());
                    dr["Ftotal_buy_fee"] = MoneyTransfer.FenToYuan(dr["Ftotal_buy_fee"].ToString());
                    string Fstate = dr["Fstate"].ToString();
                    dr["Fstate"] = Fstate == "0" ? "初始，未签约代扣协议" :
                                    Fstate == "1" ? "定投中" :
                                    Fstate == "2" ? "终止" :
                                    Fstate == "3" ? "解约中（用户终止或者更换基金导致）" :
                                    Fstate == "4" ? "解约中(连续扣款失败导致)" :
                                    Fstate == "5" ? "重新签约中" : "未知：" + Fstate;

                    string Fcease_reason = dr["Fcease_reason"].ToString();
                    dr["Fcease_reason"] = Fcease_reason == "1" ? "用户终止" :
                                  Fcease_reason == "2" ? "到期终止" :
                                  Fcease_reason == "3" ? "连续扣款失败终止" :
                                  "未知：" + Fcease_reason;

                    string Flstate = dr["Flstate"].ToString();
                    dr["Flstate"] = Flstate == "1" ? "有效" :
                                  Flstate == "2" ? "无效" :
                                  "未知：" + Flstate;

                    string Ftype = dr["Ftype"].ToString();
                    dr["Ftype"] = Ftype == "1" ? "按月" :
                                  Ftype == "2" ? "按天" :
                                  "未知：" + Ftype;

                }
            }
            return dt;
        }

        public DataTable Get_DT_fundBuyPlanByPlanid(string Tradeid,string uid, string plan_id)
        {
            FundInfoData data = new FundInfoData();
            DataTable dt = data.Get_DT_fundBuyPlanByPlanid(Tradeid,uid, plan_id);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Fplan_first_fee"] = MoneyTransfer.FenToYuan(dr["Fplan_first_fee"].ToString());
                    dr["Ftotal_profit"] = MoneyTransfer.FenToYuan(dr["Ftotal_profit"].ToString());
                    dr["Fprofit"] = MoneyTransfer.FenToYuan(dr["Fprofit"].ToString());
                    string Fcease_reason = dr["Fcease_reason"].ToString();
                    dr["Fcease_reason"] = Fcease_reason == "1" ? "用户终止" :
                                 Fcease_reason == "2" ? "到期终止" :
                                 Fcease_reason == "3" ? "连续扣款失败终止" :
                                 "未知：" + Fcease_reason;
                }
            }
            return dt;
        }

        public DataTable Get_DT_PlanBuyOrder(string Tradeid,string uid, string plan_id, int offset, int limit)
        {
            FundInfoData data = new FundInfoData();
            DataTable dt = data.Get_DT_PlanBuyOrder(Tradeid,uid, plan_id, offset, limit);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("Ffund_name", typeof(string));
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Ffund_name"] = GetFundName(dr["Fspid"].ToString(), dr["Ffund_code"].ToString());
                    dr["Ftotal_fee"] = MoneyTransfer.FenToYuan(dr["Ftotal_fee"].ToString());
                    string Fstate = dr["Fstate"].ToString();
                    dr["Fstate"] = Fstate == "0" ? "初始单,用户未确认" :
                                   Fstate == "1" ? "用户已确认,等待扣款" :
                                   Fstate == "2" ? "用户主动取消" :
                                   Fstate == "5" ? "扣款成功" :
                                   Fstate == "4" ? "当次扣款失败,等待重试" :
                                   Fstate == "10" ? "最终失败" :
                                   Fstate == "8" ? "转入退款" :
                                   "未知:" + Fstate;
                    string Flstate = dr["Flstate"].ToString();
                    dr["Flstate"] = Flstate == "1" ? "有效" :
                                Flstate == "2" ? "无效" :
                                "未知:" + Flstate;
                }
            }
            return dt;
        }

        public DataTable Get_HFD_FundFetchPlan(string uin, int offset, int limit)
        {
            string Tradeid = GetTradeIdByUIN(uin);
            FundInfoData data = new FundInfoData();
            DataTable dt = data.Get_HFD_FundFetchPlan(Tradeid, uin, offset, limit);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("Ffund_name", typeof(string));
                dt.Columns.Add("Ffund_name_list", typeof(string));
                dt.Columns.Add("Ffund_name_list1", typeof(string));

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Ffund_name"] = GetFundName(dr["Fspid"].ToString(), dr["Ffund_code"].ToString());
                    string Ffund_name_list = GetFundNameList(dr["Fspid_list"].ToString(), dr["Ffund_code_list"].ToString());
                    dr["Ffund_name_list"] = Ffund_name_list;
                    dr["Ffund_name_list1"] = Ffund_name_list.Length > 10 ? (Ffund_name_list.Substring(0, 10) + "...") : Ffund_name_list;

                    dr["Ftotal_plan_fee"] = MoneyTransfer.FenToYuan(dr["Ftotal_plan_fee"].ToString());
                    dr["Fplan_fee"] = MoneyTransfer.FenToYuan(dr["Fplan_fee"].ToString());
                    dr["Ftotal_fetch_fee"] = MoneyTransfer.FenToYuan(dr["Ftotal_fetch_fee"].ToString());
                    string Flstate = dr["Flstate"].ToString();
                    dr["Flstate"] = Flstate == "1" ? "有效" :
                                    Flstate == "2" ? "无效" :
                                    "未知:" + Flstate;
                    string Fstate = dr["Fstate"].ToString();
                    dr["Fstate"] = Fstate == "1" ? "生效" :
                                    Fstate == "2" ? "终止" :
                                    "未知:" + Fstate;

                    string Fcease_reason = dr["Fcease_reason"].ToString();
                    dr["Fcease_reason"] = Fcease_reason == "0" ? "没有终止" :
                                    Fcease_reason == "1" ? "用户终止" :
                                    Fcease_reason == "2" ? "到期终止" :
                                    Fcease_reason == "3" ? "连续多期提现失败终止" :
                                    Fcease_reason == "4" ? "提现资金不足终止" :
                                    Fcease_reason == "5" ? "余额不足终止" :
                                    Fcease_reason == "6" ? "提前还款" :
                                    Fcease_reason == "7" ? "超过赎回时间" :
                                    "未知:" + Fcease_reason;

                    string Fbussi_type = dr["Fbussi_type"].ToString();
                    dr["Fbussi_type"] =  Fbussi_type == "1" ? "借记卡类还款（房贷）" :
                                        Fbussi_type == "2" ? "信用卡类还款" :
                                        "未知:" + Fbussi_type;

                    //string Fchannel_id = dr["Fchannel_id"].ToString();
                    //dr["Fchannel_id"] = Fchannel_id == "1" ? "财付通网站" :
                    //                    Fchannel_id == "2" ? "微信" :
                    //                    Fchannel_id == "3" ? "手Q" :
                    //                    "未知:" + Fchannel_id;

                    string Ftype = dr["Ftype"].ToString();
                    dr["Ftype"] = Ftype == "1" ? "按月" :
                                  "未知：" + Ftype;

                    string Ffetch_order_type = dr["Ffetch_order_type"].ToString();
                    dr["Ffetch_order_type"] = Ffetch_order_type == "1" ? "智能还款" :
                                                Ffetch_order_type == "2" ? "半智能还款" :
                                                Ffetch_order_type == "3" ? "非智能还款" :
                                                "未知：" + Ffetch_order_type;
                    
                }
            }
            return dt;
        }
        public DataTable Get_HFD_FundFetchPlanByPlanid(string uin, string plan_id)
        {
            string Tradeid = GetTradeIdByUIN(uin);
            FundInfoData data = new FundInfoData();
            DataTable dt = data.Get_HFD_FundFetchPlanByPlanid(Tradeid, uin, plan_id);

            if (dt != null && dt.Rows.Count > 0)
            {
                //dt.Columns.Add("Ffund_name", typeof(string));
                //dt.Columns.Add("Ffund_name_list", typeof(string));
                //dt.Columns.Add("Ffund_name_list1", typeof(string));

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Ftotal_fetch_fee"] = MoneyTransfer.FenToYuan(dr["Ftotal_fetch_fee"].ToString());
                    string Flstate = dr["Flstate"].ToString();
                    dr["Flstate"] = Flstate == "1" ? "有效" :
                                   Flstate == "2" ? "无效" :
                                   "未知:" + Flstate;
                    string Fbussi_type = dr["Fbussi_type"].ToString();
                    dr["Fbussi_type"] = Fbussi_type == "1" ? "借记卡类还款（房贷）" :
                                       Fbussi_type == "2" ? "信用卡类还款" :
                                       "未知:" + Fbussi_type;
                }
            }
            return dt;
        }
        public DataTable Get_HFD_PlanFetchOrder(string Tradeid,string PROJECT, string uid, string plan_id, int offset, int limit)
        {
            string bussi_type = "";
            if (PROJECT == "HFD")
            {
                bussi_type = "2";
            }
            else 
            {
                bussi_type = "4";
            }

            FundInfoData data = new FundInfoData();
            DataTable dt = data.Get_HFD_PlanFetchOrder(Tradeid,bussi_type, uid, plan_id, offset, limit);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("Ffund_name", typeof(string));
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Ffund_name"] = GetFundName(dr["Fspid"].ToString(), dr["Ffund_code"].ToString());
                    dr["Ftotal_fee"] = MoneyTransfer.FenToYuan(dr["Ftotal_fee"].ToString());
                    dr["Ftotal_transfer_fee"] = MoneyTransfer.FenToYuan(dr["Ftotal_transfer_fee"].ToString());

                    string Fbussi_type = dr["Fbussi_type"].ToString();
                    dr["Fbussi_type"] = Fbussi_type == "1" ? "借记卡类还款（房贷）" :
                                       Fbussi_type == "2" ? "信用卡类还款" :
                                       "未知:" + Fbussi_type;

                    string Fstate = dr["Fstate"].ToString();
                    dr["Fstate"] = Fstate == "0" ? "待转换完成" :
                                     Fstate == "1" ? "待赎回" :
                                     Fstate == "2" ? "待提现" :
                                     Fstate == "3" ? "赎回成功且提现请求提现成功" :
                                     Fstate == "4" ? "提现成功" :
                                     Fstate == "5" ? "提现失败回理财通余额" :
                                     Fstate == "8" ? "还款失败" :
                                      "未知:" + Fstate;
                    string Flstate = dr["Flstate"].ToString();
                    dr["Flstate"] = Flstate == "1" ? "有效" :
                                   Flstate == "2" ? "无效" :
                                   "未知:" + Flstate;

                    string Ffail_reason = dr["Ffail_reason"].ToString();
                    dr["Ffail_reason"] = Ffail_reason == "0" ? "没有失败" :
                                            Ffail_reason == "1" ? "份额不足" :
                                            Ffail_reason == "2" ? "提现失败" :
                                            "未知:" + Ffail_reason;
                }
            }
            return dt;
        }
        #endregion

        #region 梦想计划
        public DataTable Get_DreamProject_Plan(string uin, int offset, int limit)
        {
            string Tradeid = GetTradeIdByUIN(uin);
            FundInfoData data = new FundInfoData();
            DataTable dt = data.Get_DreamProject_Plan(Tradeid, uin, offset, limit);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Ftotal_plan_fee"] = MoneyTransfer.FenToYuan(dr["Ftotal_plan_fee"].ToString());
                    dr["Ftotal_buy_fee"] = MoneyTransfer.FenToYuan(dr["Ftotal_buy_fee"].ToString());
                    string Fstate = dr["Fstate"].ToString();
                    dr["Fstate"] = Fstate == "1" ? "进行中" :
                                   Fstate == "2" ? "终止中" :
                                    Fstate == "3" ? "终止" :
                                   "未知:" + Fstate;

                }
            }
            return dt;
        }
        public DataTable Get_DreamProject_trans(string plan_id, string trade_id, int offset, int limit)
        {
            FundInfoData data = new FundInfoData();
            DataTable dt = data.Get_DreamProject_trans(plan_id, trade_id, offset, limit);

            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("FundName", typeof(string));
                foreach (DataRow dr in dt.Rows)
                {
                    dr["FundName"] = GetFundName(dr["Fspid"].ToString(), dr["Ffund_code"].ToString());
                    dr["Ftotal_fee"] = MoneyTransfer.FenToYuan(dr["Ftotal_fee"].ToString());

                    string Ftype = dr["Ftype"].ToString();
                    dr["Ftype"] = Ftype == "1" ? "申购" :
                                   Ftype == "2" ? "预约" :
                                   "未知:" + Ftype;
                    string Fstate = dr["Fstate"].ToString();
                    dr["Fstate"] = Fstate == "0" ? "初始" :
                                   Fstate == "1" ? "支付成功(暂时没用)" :
                                    Fstate == "2" ? "买入成功" :
                                     Fstate == "3" ? "买入失败" :
                                   "未知:" + Fstate;
                }
            }
            return dt;
        }
        public DataTable Get_DreamProject_asset(string plan_id, string trade_id, int offset, int limit)
        {
            FundInfoData data = new FundInfoData();
            DataTable dt = data.Get_DreamProject_asset(plan_id, trade_id, offset, limit);

            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("Ftotal_control_unit", typeof(string));
                dt.Columns.Add("Fbusiness_type", typeof(string));
                dt.Columns.Add("FundName", typeof(string));
                foreach (DataRow dr in dt.Rows)
                {
                    dr["FundName"] = GetFundName(dr["Fspid"].ToString(), dr["Ffund_code"].ToString());
                    string fund_code = dr["Ffund_code"].ToString();
                    string spid = dr["Fspid"].ToString();

#if DEBUG
                    trade_id = "1111000";
                    spid = "1111";
                    fund_code = "0000";
#endif
                    DataTable dt_controlasset = data.Get_DreamProject_controlasset(trade_id, spid, fund_code);
                    if (dt_controlasset != null && dt_controlasset.Rows.Count > 0)
                    {
                        dr["Ftotal_control_unit"] = dt_controlasset.Rows[0]["Ftotal_control_unit"].ToString();
                        dr["Fbusiness_type"] = dt_controlasset.Rows[0]["Fbusiness_type"].ToString();
                    }

                    dr["Ftotal_buy_fee"] = MoneyTransfer.FenToYuan(dr["Ftotal_buy_fee"].ToString());
                    dr["Ftotal_profit"] = MoneyTransfer.FenToYuan(dr["Ftotal_profit"].ToString());
                    dr["Fprofit"] = MoneyTransfer.FenToYuan(dr["Fprofit"].ToString());
                    dr["Ftotal_control_unit"] = MoneyTransfer.FenToYuan(dr["Ftotal_control_unit"].ToString());

                    string Fstate = dr["Fstate"].ToString();
                    dr["Fstate"] = Fstate == "1" ? "进行中" :
                                    Fstate == "2" ? "终止" :
                                   "未知:" + Fstate;
                    string Fbusiness_type = dr["Fbusiness_type"].ToString();
                    dr["Fbusiness_type"] = Fbusiness_type == "0" ? "默认" :
                                    Fbusiness_type == "1" ? "梦想计划" :
                                   "未知:" + Fbusiness_type;

                }
            }
            return dt;
        }
        #endregion
        /// <summary>
        /// 理财通预约买入
        /// </summary>
        /// <param name="trade_id"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public DataTable GetLCTReserveOrder(string trade_id, string listid, string stime, string etime, int offset, int limit)
        {
            FundInfoData data = new FundInfoData();
            DataTable dt = data.GetLCTReserveOrder(trade_id, listid, stime, etime, offset, limit);
            //获取基金名称
            
            dt.Columns.Add("Freserve_fund_name", typeof(string));
            dt.Columns.Add("Ffrom_fund_name", typeof(string));

            foreach (DataRow dr in dt.Rows)
            {   
                dr["Freserve_fund_name"] = GetFundName(dr["Freserve_spid"].ToString(), dr["Freserve_fund_code"].ToString());
                dr["Ffrom_fund_name"] = GetFundName(dr["Ffrom_spid"].ToString(), dr["Ffrom_fund_code"].ToString());

                dr["Ftotal_fee"] = MoneyTransfer.FenToYuan(dr["Ftotal_fee"].ToString());
                string state = dr["Fstate"].ToString();
                dr["Fstate"] = LCTReserveOrder_State.ContainsKey(state) ? LCTReserveOrder_State[state] : "未知:" + state;
                string Fcancel_reason = dr["Fcancel_reason"].ToString();
                dr["Fcancel_reason"] = Fcancel_reason == "0" ? "" :
                                       Fcancel_reason == "1" ? "用户取消预约" :
                                       Fcancel_reason == "2" ? "过期自动取消预约" : 
                                       "未知:" + Fcancel_reason;

                string Freserve_type = dr["Freserve_type"].ToString();
                dr["Freserve_type"] = Freserve_type == "1" ? "新买入货币基金" :
                                       Freserve_type == "2" ? "已有基金冻结" :
                                       "未知:" + Freserve_type;                
            }
            return dt;
        }


        public DataTable GetLCTSwith(string trade_id, string buy_id, string redem_id, string change_id)
        {
            FundInfoData data = new FundInfoData();
            DataTable dt = data.GetLCTSwith(trade_id, buy_id, redem_id, change_id);
            dt.Columns.Add("Fori_fund_name", typeof(string));//转出基金公司
            dt.Columns.Add("Fnew_fund_name", typeof(string));//转入基金公司
            dt.Columns.Add("FstateStr", typeof(string));
            foreach (DataRow dr in dt.Rows)
            {
                dr["Fori_fund_name"] = GetFundName(dr["Fori_spid"].ToString(), dr["Fori_fund_code"].ToString());
                dr["Fnew_fund_name"] = GetFundName(dr["Fnew_spid"].ToString(), dr["Fnew_fund_code"].ToString());
                string Fstate = dr["Fstate"].ToString().Trim();
                dr["FstateStr"] = Fstate == "0" ? "初始（如果流程中断这个初始态是一种最终态）" :
                                  Fstate == "1" ? "申购申请成功" :
                                  Fstate == "2" ? "赎回成功" :
                                  Fstate == "3" ? "赎回失败（最终状态）" :
                                  Fstate == "4" ? "转换成功（最终状态）" : ("未知：" + Fstate);
                dr["Ftotal_fee"] = MoneyTransfer.FenToYuan(dr["Ftotal_fee"].ToString());
            }
            return dt;

        }


        public Dictionary<string, string> LCTReserveOrder_State = new Dictionary<string, string>()
        {
             {"0","初始状态"},
             {"1","待冻结货币基金"},
             {"2","冻结成功"},
             {"3","待用户确认(如需用户确认)"},
             {"4","确认成功待转换"},
             {"5","转换成功（最终状态）"},
             {"6","取消预约"},
             {"7","解冻成功（最终状态）"}
         };
           
        
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

        public string GetFundName(string Spid, string fundcode)
        {
            try
            {
                var FundInfoBySpidTable = new FundInfoData().QueryFundInfoBySpid(Spid,fundcode);//查Fcurtype及基金名称

                if (FundInfoBySpidTable != null && FundInfoBySpidTable.Rows.Count > 0)
                {
                    return FundInfoBySpidTable.Rows[0]["Ffund_name"].ToString();
                }
                else
                {
                    return Spid;
                }
            }
            catch
            {
                return Spid;
            }
        }
        private string GetFundNameList(string spidlist, string fundcodelist)
        {
            try
            {
                string Fund_Namelist = "";
                string[] spids = spidlist.Split(',');
                string[] fundcodes = fundcodelist.Split(',');
                for (int i = 0; i < spids.Length; i++)
                {
                    string spid = spids[i].Trim();
                    string fundcode = fundcodes[i].Trim();
                    if (!string.IsNullOrEmpty(spid) && !string.IsNullOrEmpty(fundcode))
                    {

                        var FundInfoBySpidTable = new FundInfoData().QueryFundInfoBySpid(spid, fundcode);//查Fcurtype及基金名称
                        if (FundInfoBySpidTable != null && FundInfoBySpidTable.Rows.Count > 0)
                        {
                            if (Fund_Namelist == "")
                            {
                                Fund_Namelist = FundInfoBySpidTable.Rows[0]["Ffund_name"].ToString();
                            }
                            else
                            {
                                Fund_Namelist += "," + FundInfoBySpidTable.Rows[0]["Ffund_name"].ToString();
                            }
                        }
                        else
                        {
                            if (Fund_Namelist == "")
                            {
                                Fund_Namelist = spid;
                            }
                            else
                            {
                                Fund_Namelist += "," + spid;
                            }
                        }
                    }
                }
                return Fund_Namelist;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
           
        }
    }
}
