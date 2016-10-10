using System;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.C2C.Finance.Common;
using System.Collections;
using System.Configuration;
using System.Data.OleDb;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using System.Security.Cryptography;

namespace TENCENT.OSS.C2C.Finance.BankLib
{


    /// <summary>
    /// 传递参数类。
    /// </summary>
    public struct Param
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParamName;
        /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue;
        /// <summary>
        /// 标识
        /// </summary>
        public string ParamFlag;
    }

    /// <summary>
    /// BankIO 的摘要说明。
    /// </summary>
    public class BankIO
    {
        public BankIO()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public static Hashtable GetBankHashTable()
        {
            System.Web.Caching.Cache objCache = System.Web.HttpRuntime.Cache;
            if (objCache["BANK_TYPE"] == null)
                queryDic("BANK_TYPE");
            Hashtable ht = new Hashtable();
            ht = (Hashtable)objCache["BANK_TYPE"];
            return ht;
        }

        public static string QueryBankName(string bankType)
        {
            try
            {
                if (bankType == null || bankType == "")
                {
                    return "";
                }
                if (bankType == "9999")
                    return "汇总银行";

                return returnDicStr("BANK_TYPE", bankType);

            }
            catch (Exception err)
            {
                throw new Exception("获取银行名称失败：" + err);
            }

        }

        public static string returnDicStr(string type, string sType)
        {
            try
            {
                if (sType == null || sType == "")  //传入空，则返回空
                {
                    return "";
                }
                else
                {
                    System.Web.Caching.Cache objCache = System.Web.HttpRuntime.Cache;

                    Hashtable ht = new Hashtable();
                    if (objCache[type] == null)
                        queryDic(type);
                    ht = (Hashtable)objCache[type];
                    if (ht != null)
                    {
                        return ht[sType].ToString();
                    }
                    else
                    {
                        return "未知" + sType;
                    }
                }
            }
            catch (Exception e) //没有从数据字典中读到memo
            {

                return "未知" + sType;
            }
        }


        //获取数据字典
        public static void queryDic(string type)
        {
            try
            {
                string Msg = "";
                /*string strSql = "type=" + type;	
                commRes com=new commRes();
                DataSet ds = CommQuery.GetDataSetFromICE(strSql,CommQuery.QUERY_DIC,out Msg);
                if (ds == null) //如果获取数据字典失败
                {
                    throw new Exception(Msg);
                }*/
                DataTable dt = QueryDicInfoByType(type, out Msg);
                if (dt == null || dt.Rows.Count == 0) //如果获取数据字典失败
                {
                    throw new Exception(Msg);
                }

                Hashtable myht = new Hashtable();
                foreach (DataRow dr in dt.Rows)
                {
                    myht.Add(dr["Fvalue"].ToString(), dr["Fmemo"].ToString());
                }
                System.Web.Caching.Cache objCache = System.Web.HttpRuntime.Cache;
                objCache.Insert(type, myht);
            }
            catch (Exception e)
            {
                throw new Exception("查询银行字典信息失败：" + e.ToString());
            }


        }


        //通过类型查询返回字段表信息
        public static DataTable QueryDicInfoByType(string type, out string Msg)
        {
            Msg = "";
            try
            {
                //先查询出总笔数
                string icesql = "type=" + type;
                string count = CommQuery.GetOneResultFromICE(icesql, CommQuery.QUERY_DIC_COUNT, "acount", out Msg);
                if (count == null || count == "" || count == "0")
                {
                    return null;
                }
                int allCount = Convert.ToInt32(count);
                if (allCount <= 0)
                {
                    return null;
                }


                DataTable dt_all = new DataTable();
                dt_all.Columns.Add("Fno", System.Type.GetType("System.String"));
                dt_all.Columns.Add("FType", System.Type.GetType("System.String"));
                dt_all.Columns.Add("Fvalue", System.Type.GetType("System.String"));
                dt_all.Columns.Add("Fmemo", System.Type.GetType("System.String"));

                string strSqlTmp = "type=" + type;
                int limitStart = 0;
                int onceCount = 20;//一次返回笔数

                while (allCount > limitStart)
                {
                    string strSqlLimit = "&strlimit=limit " + limitStart + "," + onceCount;
                    string strSql = strSqlTmp + strSqlLimit;
                    DataTable dt_one = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_DIC, out Msg);
                    if (dt_one != null && dt_one.Rows.Count > 0)
                    {
                        foreach (DataRow dr2 in dt_one.Rows)
                        {
                            string fno = dr2["Fno"].ToString();
                            string FType = dr2["FType"].ToString();
                            string Fvalue = dr2["Fvalue"].ToString();
                            string Fmemo = dr2["Fmemo"].ToString();

                            DataRow drNew = dt_all.NewRow();
                            drNew["Fno"] = fno;
                            drNew["FType"] = FType;
                            drNew["Fvalue"] = Fvalue;
                            drNew["Fmemo"] = Fmemo;
                            dt_all.Rows.Add(drNew);
                        }

                    }

                    limitStart = limitStart + onceCount;
                }


                return dt_all;
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("查询字典表信息异常");
                if (log.IsErrorEnabled) log.Error(ex.ToString());
                return null;
            }
        }

        //根据Fproduct和Fbusiness_type字段做映射eg{Fproduct-Fbusiness_type}:key=11-9为付款退款-手Q
        public static string GetExtractCashType(string key)
        {
            System.Web.Caching.Cache objCache = System.Web.HttpRuntime.Cache;
            if (objCache["ExtractCash_Type"] == null)
                QueryDicExtractCashType("ExtractCash_Type");
            Hashtable ht = new Hashtable();
            ht = (Hashtable)objCache["ExtractCash_Type"];
            if (ht.ContainsKey(key))
            {
                return ht[key].ToString();
            }
            else
            {
                return "未知类型";
            }
        }


        //获取数据字典
        public static void QueryDicExtractCashType(string type)
        {
            Hashtable myht = InitExtractCashType();
            System.Web.Caching.Cache objCache = System.Web.HttpRuntime.Cache;
            objCache.Insert(type, myht);
        }

        public static Hashtable InitExtractCashType()
        {
            Hashtable myht = new Hashtable();
            myht.Add("2-1", "大额提现");
            myht.Add("2-2", "大额向银行卡付款");
            myht.Add("5-1", "主站向银行卡付款（PC小钱包）");
            myht.Add("5-2", "主站还房贷");
            myht.Add("5-3", "主站向绑定卡普通提现");
            myht.Add("5-4", "主站向绑定卡快速提现");
            myht.Add("5-6", "主站向银行卡付款（财付通APP）");
            myht.Add("5-7", "主站付款到银行（微彩票）");
            myht.Add("5-8", "手Q提现T+0");
            myht.Add("5-9", "微信现金券账户提现到银行卡");
            myht.Add("5-10", "手Q提现T+1");
            myht.Add("10002-1", "代扣随机打款用信用卡还款（只登记）");
            myht.Add("6-1", "手Q付款给银行卡");
            myht.Add("6-2", "手Q付款给银行卡(预留)");
            myht.Add("6-3", "手Q付款给银行卡(预留)");
            myht.Add("6-4", "手Q账户打款认证T+0");
            myht.Add("6-5", "手Q账户打款认证T+1");
            myht.Add("6-10", "手Q信用付提现T+0");
            myht.Add("6-11", "手Q信用付提现T+1");
            myht.Add("7-1", "微信提现T+0");
            myht.Add("7-2", "微信提现T+1");
            myht.Add("7-3", "微信准实时提现");
            myht.Add("8-1", "理财通提现T+0");
            myht.Add("8-2", "理财通提现T+1");
            myht.Add("9-1", "信用卡还款-微信还款");
            myht.Add("9-2", "信用卡还款-手Q还款");
            myht.Add("9-3", "信用卡还款-主站还款");
            myht.Add("9-4", "信用卡还款-财付通APP");
            myht.Add("9-5", "信用卡还款-O2");
            myht.Add("10-1", "实时结算提现-T+0");
            myht.Add("10-2", "实时结算提现-T+1");
            myht.Add("10-3", "代发提现退款-微信");
            myht.Add("10-4", "代发提现退款-手Q");
            myht.Add("11-1", "首发提现退款-微信");
            myht.Add("11-2", "首发提现退款-手Q");
            myht.Add("11-3", "退款分流-微信");
            myht.Add("11-4", "退款分流-手Q");
            myht.Add("11-5", "银行退单-微信");
            myht.Add("11-6", "银行退单-手Q");
            myht.Add("11-7", "银行退单-财付通");
            myht.Add("11-8", "付款退款-微信");
            myht.Add("11-9", "付款退款-手Q");
            myht.Add("11-10", "付款退款-财付通");
            myht.Add("11-11", "付款退款失败后直接提现成功（调平）");
            myht.Add("11-12", "赔付退单-微信");
            myht.Add("11-13", "赔付退单-手Q");
            myht.Add("11-14", "赔付退单-财付通");
            myht.Add("12-1", "商户业务-主动提现T+0");
            myht.Add("12-2", "商户业务-主动提现T+1");
            myht.Add("12-3", "商户业务-商户结算提现M0");
            myht.Add("12-4", "商户业务-商户结算提现M1");
            myht.Add("12-5", "商户业务-商户结算提现M2");
            myht.Add("12-6", "微信代付T+0");
            myht.Add("12-7", "微信代付T+1");
            myht.Add("12-8", "微信代付小额实时");
            myht.Add("12-9", "手Q代付T+0");
            myht.Add("12-10", "手Q代付T+1");
            myht.Add("12-11", "手Q代付小额实时");
            myht.Add("12-12", "财付通代付T+0");
            myht.Add("12-13", "财付通代付T+1");
            myht.Add("12-14", "财付通代付小额实时");
            myht.Add("12-15", "财付通代付新系统T+0");
            myht.Add("12-16", "财付通代付新系统T+1");
            myht.Add("12-17", "财付通代付新系统小额实时");
            myht.Add("13-1", "特殊业务-资金调拨");
            myht.Add("13-2", "特殊业务-手工提现");
            myht.Add("13-3", "特殊业务-收入结转");
            myht.Add("13-4", "特殊业务-Qbase款");
            myht.Add("13-5", "特殊业务-赔付付款");
            myht.Add("13-6", "特殊业务-保证金付款");
            myht.Add("14-1", "个性业务-公益付款");
            myht.Add("99-1", "付款压测");
            myht.Add("99-2", "付款压测");
            myht.Add("9999-1", "实时还款-手Q");
            myht.Add("9998-1", "实时还款-微信");
            myht.Add("9997-1", "实时还款-代付");
            myht.Add("9996-1", "实时还款-财付通");
            return myht;
        }
    }
}
