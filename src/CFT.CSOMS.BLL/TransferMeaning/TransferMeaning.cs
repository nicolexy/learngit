using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web;
using System.Data;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace CFT.CSOMS.BLL.TransferMeaning
{
    public class Transfer{
        private static Dictionary<string,Hashtable> dicData=new Dictionary<string,Hashtable>();

        public static string convertActionType(string sType)  //动作类型  内部之间的帐务关系 
        {
            return returnDicStr("ACTION_TYPE", sType);
        }
        public static string convertMoney_type(string sType) //币种类型转换 CUR_TYPE
        {
            return returnDicStr("CUR_TYPE", sType);
        }
        public static string convertTradeType(string sType)  //入还是出 
        {
            return returnDicStr("BG_TYPE", sType);
        }
        public static string convertSubject(string sType)  //类别,科目  BG_SUBJECT
        {
            return returnDicStr("BG_SUBJECT", sType);
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
    }
}
