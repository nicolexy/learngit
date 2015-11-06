using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web;
using System.Data;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.DAL.Infrastructure;

namespace CFT.CSOMS.BLL.TransferMeaning
{
    public class Transfer{
        private static Dictionary<string,Hashtable> dicData=new Dictionary<string,Hashtable>();

        public static string accountState(string sType)  //账户状态转换  USER_STATE
        {
            return returnDicStr("USER_STATE", sType);
        }

        public static string convertFuser_type(string sType) //账户类型转换  USER_TYPE
        {
            return returnDicStr("USER_TYPE", sType);
        }

        public static string convertFcre_type(string sType) //查询证件类型  CRE_TYPE
        {
            return returnDicStr("CRE_TYPE", sType);
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

        public static string convertCurrentState(string sType)  //当前状态 TCPAY_STATE
        {
            return returnDicStr("TCPAY_STATE", sType);
        }

        public static string convertTradeSign(string sType)  //交易标记 1-成功 2-失败 TCLIST_SIGN
        {
            return returnDicStr("TCLIST_SIGN", sType);
        }

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

        public static string convertProAttType(int nAttid) //产品属性
        {
            //从数据字典中读取数据，绑定到web页
            DataSet ds = PermitPara.QueryDicAccName();
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return "";
            }
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                if (nAttid == int.Parse(dr["Value"].ToString().Trim()))
                {
                    return dr["Text"].ToString().Trim();
                }
            }
            return "";
        }

        /// <summary>
        /// 证件类型转义
        /// </summary>
        /// <param name="creid"></param>
        /// <returns></returns>
        public static string GetCreType(string creid)
        {

            if (creid == null || creid.Trim() == "")
                return "未指定类型";

            int icreid = 0;
            try
            {
                icreid = Int32.Parse(creid);
            }
            catch
            {
                return "不正确类型" + creid;
            }

            if (icreid >= 1 && icreid <= 11)
            {
                if (icreid == 1)
                {
                    return "身份证";
                }
                else if (icreid == 2)
                {
                    return "护照";
                }
                else if (icreid == 3)
                {
                    return "军官证";
                }
                else if (icreid == 4)
                {
                    return "士兵证";
                }
                else if (icreid == 5)
                {
                    return "回乡证";
                }
                else if (icreid == 6)
                {
                    return "临时身份证";
                }
                else if (icreid == 7)
                {
                    return "户口簿";
                }
                else if (icreid == 8)
                {
                    return "警官证";
                }
                else if (icreid == 9)
                {
                    return "台胞证";
                }
                else if (icreid == 10)
                {
                    return "营业执照";
                }
                else if (icreid == 11)
                {
                    return "其它证件";
                }
                else
                {
                    return "不正确类型" + creid;
                }
            }
            else
            {
                return "不正确类型" + creid;
            }
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

                    if (!dicData.ContainsKey(type)||(Hashtable)dicData[type] == null)
                        queryDic(type);
                    ht = (Hashtable)dicData[type];

                    string memo = ht[sType].ToString();
                    return memo;
                }
            }
            catch  //没有从数据字典中读到memo
            {
                return "";
            }
        }

        //获取数据字典
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
            dicData[type] = myht;
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

    }
}
