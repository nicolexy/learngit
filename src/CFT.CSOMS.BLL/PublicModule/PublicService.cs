using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace CFT.CSOMS.BLL.PublicService
{
    using CFT.CSOMS.DAL.Infrastructure;
    using System.Collections;
    using System.Web;
    using TENCENT.OSS.C2C.Finance.Common.CommLib;
    using System.Security.Cryptography;
    using System.IO;
    public class PublicService
    {
        /// <summary>
        /// 姓名生僻字  姓名解密方法
        /// 与一点通解密算法一样，秘钥不一样
        /// </summary>
        /// <param name="base64Bankid"></param>
        /// <returns></returns>
        public static string NameEncode_ForRareName(string base64Bankid)
        {
            byte[] iv = { 0x2e, 0x3a, 0x35, 0x67, 0x45, 0x6a, 0x7f, 0x0a };
            byte[] newkey = { 0x3a, 0x2a, (byte)(-0x28 & 0xFF), 0x59, 0x43, (byte)(-0x23 & 0xFF), 0x16, (byte)(-0x65 & 0xFF) };

            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                //middle的base64转换不标准，这个地方需要替换下 
                byte[] inputByteArray = Convert.FromBase64String(base64Bankid.Replace("-", "+").Replace("_", "/").Replace("%3d", "=").Replace("%3D", "="));

                //建立加密对象的密钥和偏移量，此值重要，不能修改 

                des.Key = newkey;
                des.IV = iv;
                //Padding设置为None，这个很重要，因为middle是这个加密的
                des.Padding = System.Security.Cryptography.PaddingMode.None;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                return System.Text.Encoding.GetEncoding("gb2312").GetString(ms.ToArray()).Trim();
            }
            catch (Exception ex)
            {
                return base64Bankid;
            }
        }

        ///外币分转化成元
        public string FenToYuan(string strfen, string currency_type)
        {
            return MoneyTransfer.FenToYuan(strfen, currency_type);
        }

        public static string objectToString(DataTable dt, string col_name)
        {
            return objectToString(dt, 0, col_name);
        }

        public static string objectToString(DataTable dt, int row_id, string col_name)
        {
            string ret = "";
            try
            {
                if (col_name == null || col_name == "")
                {
                    return "";
                }
                if (dt.Columns.Contains(col_name))
                {
                    return dt.Rows[row_id][col_name].ToString();
                }
            }
            catch (Exception ex)
            {
                ret = "";
            }

            return ret;
        }

        //在DataTable中转换值，标识值到标识名称。
        public static void GetColumnValueFromDic(DataTable dt, string SourceColumn, string DestColumn, string sType)
        {
            try
            {
                Hashtable ht = GetAllValueByType(sType);
                if (ht == null || ht.Count == 0) return;

                foreach (DataRow dr in dt.Rows)
                {
                    string tmp = PublicRes.GetString(dr[SourceColumn]);
                    if (tmp != "")
                    {
                        dr.BeginEdit();
                        if (ht.ContainsKey(tmp))
                        {
                            dr[DestColumn] = ht[tmp].ToString();
                        }
                        else
                        {
                            dr[DestColumn] = "未知类型(" + tmp + ")";
                        }
                        dr.EndEdit();
                    }
                }
            }
            catch
            { }
        }

        //furion 20050804 取得指定类型的所有键和值。
        public static Hashtable GetAllValueByType(string sType)
        {
            Hashtable ht = new Hashtable();

            try
            {
                if (HttpContext.Current.Application[sType] == null)
                    queryDic(sType);

                ht = (Hashtable)HttpContext.Current.Application[sType];

                return ht;
            }
            catch
            {
                return null;
            }
        }

        public static string accountState(string sType)  //账户状态转换  USER_STATE
        {
            return returnDicStr("USER_STATE", sType);
        }

        public static string convertFuser_type(string sType) //账户类型转换  USER_TYPE
        {
            return returnDicStr("USER_TYPE", sType);
        }

        public static string convertMoney_type(string sType) //币种类型转换 CUR_TYPE
        {
            return returnDicStr("CUR_TYPE", sType);
        }

        public static string convertbankType(string sType)  //银行类型转换 BANK_TYPE
        {
            if (sType == "9999")
                return "汇总银行";

            return returnDicStr("BANK_TYPE", sType);
        }

        public static string convertTradeState(string sType)  //交易单状态 RLIST_STATE
        {
            return returnDicStr("RLIST_STATE", sType);
        }

        public static string convertSex(string sType)  //用户性别
        {
            return returnDicStr("SEX", sType);
        }

        public static string convertTradeListState(string sType)  //交易单状态 PAY_STATE
        {
            return returnDicStr("PAY_STATE", sType);
        }

        public static string convertSubject(string sType)  //类别,科目  BG_SUBJECT
        {
            return returnDicStr("BG_SUBJECT", sType);
        }

        public static string convertActionType(string sType)  //动作类型  内部之间的帐务关系 
        {
            return returnDicStr("ACTION_TYPE", sType);
        }

        public static string convertTradeType(string sType)  //入还是出 
        {
            return returnDicStr("BG_TYPE", sType);
        }

        public static string convertPayType(string sType)  //交易类别 c2c,b2c ,转帐
        {
            return returnDicStr("PAYLIST_TYPE", sType);
        }

        //	    public static string convertAdjustFlag(string )  //正常交易还是转帐标志

        public static string convertCurrentState(string sType)  //当前状态 TCPAY_STATE
        {
            return returnDicStr("TCPAY_STATE", sType);
        }

        public static string convertTradeSign(string sType)  //交易标记 1-成功 2-失败 TCLIST_SIGN
        {
            return returnDicStr("TCLIST_SIGN", sType);
        }

        //
        public static string convertTCSubject(string sType)  //类别 科目 TCLIST_SUBJECT
        {
            return returnDicStr("TCLIST_SUBJECT", sType);
        }

        public static string convertCheckState(string sType)  //对帐任务的执行状态 TASK_STATUS
        {
            return returnDicStr("TASK_STATUS", sType);
        }


        public static string convertCheckType(string sType)  //对帐任务的类型 SUB_TASK_NO
        {
            return returnDicStr("SUB_TASK_NO", sType);
        }

        public static string cRefundState(string sType)  //退款状态 REFUND_STATE
        {
            return returnDicStr("REFUND_STATE", sType);
        }

        public static string cRlistState(string sType)  //退款单状态 RLIST_STATE
        {
            return returnDicStr("RLIST_STATE", sType);
        }

        public static string cPay_type(string sType)  //支付类型 PAY_TYPE
        {
            return returnDicStr("PAY_TYPE", sType);
        }

        public static string convertTCfSubject(string sType)  //付款的类别 TC_PLIST_SUBJECT
        {
            return returnDicStr("TC_PLIST_SUBJECT", sType);
        }

        public static string convertTCState(string sType)  //付款的类别 TCLIST_State
        {
            return returnDicStr("TCLIST_State", sType);
        }

        public static string convertInnerCkType(string sType)  //内部对帐的类型 
        {
            return returnDicStr("SUB_TASK_NO1", sType);
        }

        public static string convertAdjustSign(string sType)  //调帐标记 
        {
            return returnDicStr("ADJUST_FLAG", sType);
        }

        public static string convertBPAY(string sType)  //余额支付状态 1 开启 2 关闭 
        {
            return returnDicStr("FBPAY_STATE", sType);
        }

        public static string returnDicStr(string type, string sType)
        {
            try
            {
                if (sType == "")  //传入空，则返回空
                {
                    return "";
                }
                else
                {
                    Hashtable ht = new Hashtable();

                    if (HttpContext.Current.Application[type] == null)
                        queryDic(type);
                    ht = (Hashtable)HttpContext.Current.Application[type];

                    string memo = ht[sType].ToString();
                    return memo;
                }
            }
            catch  //没有从数据字典中读到memo
            {
                return "";
            }
        }

        public static void queryDic(string type)
        {         
            string Msg;
            DataSet ds = QueryDicInfoByType(type, out Msg);

            if (ds == null) //如果获取数据字典失败
            {
                throw new Exception(Msg);
            }

            Hashtable myht = new Hashtable();
            try
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    myht.Add(dr["Fvalue"].ToString(), dr["Fmemo"].ToString());
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }

            //绑定数据字典
            HttpContext.Current.Application[type] = myht;
        }

        //通过类型查询返回字段表信息
        public static DataSet QueryDicInfoByType(string type, out string Msg)
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
                dt_all.Columns.Add("Fsymbol", System.Type.GetType("System.String"));

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
                            string symbol = dr2["Fsymbol"].ToString();

                            DataRow drNew = dt_all.NewRow();
                            drNew["Fno"] = fno;
                            drNew["FType"] = FType;
                            drNew["Fvalue"] = Fvalue;
                            drNew["Fmemo"] = Fmemo;
                            drNew["Fsymbol"] = symbol;
                            dt_all.Rows.Add(drNew);
                        }

                    }

                    limitStart = limitStart + onceCount;
                }

                int num = dt_all.Rows.Count;
                DataSet ds = new DataSet();
                ds.Tables.Add(dt_all);
                return ds;
            }
            catch
            {
                return null;
            }
        }

        public DataTable GetCheckInfo(string objid, string checkType)
        {
            string msg = "";
            DataTable dt= new PublicRes().GetCheckInfo(objid, checkType, out msg);
            if (msg != "")
                throw new Exception(msg);
            return dt;
        }
    }
}
