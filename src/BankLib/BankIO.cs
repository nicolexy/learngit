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
    /// ���ݲ����ࡣ
    /// </summary>
    public struct Param
    {
        /// <summary>
        /// ��������
        /// </summary>
        public string ParamName;
        /// <summary>
        /// ����ֵ
        /// </summary>
        public string ParamValue;
        /// <summary>
        /// ��ʶ
        /// </summary>
        public string ParamFlag;
    }

    /// <summary>
    /// BankIO ��ժҪ˵����
    /// </summary>
    public class BankIO
    {
        public BankIO()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
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
                    return "��������";

                return returnDicStr("BANK_TYPE", bankType);

            }
            catch (Exception err)
            {
                throw new Exception("��ȡ��������ʧ�ܣ�" + err);
            }

        }

        public static string returnDicStr(string type, string sType)
        {
            try
            {
                if (sType == null || sType == "")  //����գ��򷵻ؿ�
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
                        return "δ֪" + sType;
                    }
                }
            }
            catch (Exception e) //û�д������ֵ��ж���memo
            {

                return "δ֪" + sType;
            }
        }


        //��ȡ�����ֵ�
        public static void queryDic(string type)
        {
            try
            {
                string Msg = "";
                /*string strSql = "type=" + type;	
                commRes com=new commRes();
                DataSet ds = CommQuery.GetDataSetFromICE(strSql,CommQuery.QUERY_DIC,out Msg);
                if (ds == null) //�����ȡ�����ֵ�ʧ��
                {
                    throw new Exception(Msg);
                }*/
                DataTable dt = QueryDicInfoByType(type, out Msg);
                if (dt == null || dt.Rows.Count == 0) //�����ȡ�����ֵ�ʧ��
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
                throw new Exception("��ѯ�����ֵ���Ϣʧ�ܣ�" + e.ToString());
            }


        }


        //ͨ�����Ͳ�ѯ�����ֶα���Ϣ
        public static DataTable QueryDicInfoByType(string type, out string Msg)
        {
            Msg = "";
            try
            {
                //�Ȳ�ѯ���ܱ���
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
                int onceCount = 20;//һ�η��ر���

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
                log4net.ILog log = log4net.LogManager.GetLogger("��ѯ�ֵ����Ϣ�쳣");
                if (log.IsErrorEnabled) log.Error(ex.ToString());
                return null;
            }
        }

        //����Fproduct��Fbusiness_type�ֶ���ӳ��eg{Fproduct-Fbusiness_type}:key=11-9Ϊ�����˿�-��Q
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
                return "δ֪����";
            }
        }


        //��ȡ�����ֵ�
        public static void QueryDicExtractCashType(string type)
        {
            Hashtable myht = InitExtractCashType();
            System.Web.Caching.Cache objCache = System.Web.HttpRuntime.Cache;
            objCache.Insert(type, myht);
        }

        public static Hashtable InitExtractCashType()
        {
            Hashtable myht = new Hashtable();
            myht.Add("2-1", "�������");
            myht.Add("2-2", "��������п�����");
            myht.Add("5-1", "��վ�����п����PCСǮ����");
            myht.Add("5-2", "��վ������");
            myht.Add("5-3", "��վ��󶨿���ͨ����");
            myht.Add("5-4", "��վ��󶨿���������");
            myht.Add("5-6", "��վ�����п�����Ƹ�ͨAPP��");
            myht.Add("5-7", "��վ������У�΢��Ʊ��");
            myht.Add("5-8", "��Q����T+0");
            myht.Add("5-9", "΢���ֽ�ȯ�˻����ֵ����п�");
            myht.Add("5-10", "��Q����T+1");
            myht.Add("10002-1", "���������������ÿ����ֻ�Ǽǣ�");
            myht.Add("6-1", "��Q��������п�");
            myht.Add("6-2", "��Q��������п�(Ԥ��)");
            myht.Add("6-3", "��Q��������п�(Ԥ��)");
            myht.Add("6-4", "��Q�˻������֤T+0");
            myht.Add("6-5", "��Q�˻������֤T+1");
            myht.Add("6-10", "��Q���ø�����T+0");
            myht.Add("6-11", "��Q���ø�����T+1");
            myht.Add("7-1", "΢������T+0");
            myht.Add("7-2", "΢������T+1");
            myht.Add("7-3", "΢��׼ʵʱ����");
            myht.Add("8-1", "���ͨ����T+0");
            myht.Add("8-2", "���ͨ����T+1");
            myht.Add("9-1", "���ÿ�����-΢�Ż���");
            myht.Add("9-2", "���ÿ�����-��Q����");
            myht.Add("9-3", "���ÿ�����-��վ����");
            myht.Add("9-4", "���ÿ�����-�Ƹ�ͨAPP");
            myht.Add("9-5", "���ÿ�����-O2");
            myht.Add("10-1", "ʵʱ��������-T+0");
            myht.Add("10-2", "ʵʱ��������-T+1");
            myht.Add("10-3", "���������˿�-΢��");
            myht.Add("10-4", "���������˿�-��Q");
            myht.Add("11-1", "�׷������˿�-΢��");
            myht.Add("11-2", "�׷������˿�-��Q");
            myht.Add("11-3", "�˿����-΢��");
            myht.Add("11-4", "�˿����-��Q");
            myht.Add("11-5", "�����˵�-΢��");
            myht.Add("11-6", "�����˵�-��Q");
            myht.Add("11-7", "�����˵�-�Ƹ�ͨ");
            myht.Add("11-8", "�����˿�-΢��");
            myht.Add("11-9", "�����˿�-��Q");
            myht.Add("11-10", "�����˿�-�Ƹ�ͨ");
            myht.Add("11-11", "�����˿�ʧ�ܺ�ֱ�����ֳɹ�����ƽ��");
            myht.Add("11-12", "�⸶�˵�-΢��");
            myht.Add("11-13", "�⸶�˵�-��Q");
            myht.Add("11-14", "�⸶�˵�-�Ƹ�ͨ");
            myht.Add("12-1", "�̻�ҵ��-��������T+0");
            myht.Add("12-2", "�̻�ҵ��-��������T+1");
            myht.Add("12-3", "�̻�ҵ��-�̻���������M0");
            myht.Add("12-4", "�̻�ҵ��-�̻���������M1");
            myht.Add("12-5", "�̻�ҵ��-�̻���������M2");
            myht.Add("12-6", "΢�Ŵ���T+0");
            myht.Add("12-7", "΢�Ŵ���T+1");
            myht.Add("12-8", "΢�Ŵ���С��ʵʱ");
            myht.Add("12-9", "��Q����T+0");
            myht.Add("12-10", "��Q����T+1");
            myht.Add("12-11", "��Q����С��ʵʱ");
            myht.Add("12-12", "�Ƹ�ͨ����T+0");
            myht.Add("12-13", "�Ƹ�ͨ����T+1");
            myht.Add("12-14", "�Ƹ�ͨ����С��ʵʱ");
            myht.Add("12-15", "�Ƹ�ͨ������ϵͳT+0");
            myht.Add("12-16", "�Ƹ�ͨ������ϵͳT+1");
            myht.Add("12-17", "�Ƹ�ͨ������ϵͳС��ʵʱ");
            myht.Add("13-1", "����ҵ��-�ʽ����");
            myht.Add("13-2", "����ҵ��-�ֹ�����");
            myht.Add("13-3", "����ҵ��-�����ת");
            myht.Add("13-4", "����ҵ��-Qbase��");
            myht.Add("13-5", "����ҵ��-�⸶����");
            myht.Add("13-6", "����ҵ��-��֤�𸶿�");
            myht.Add("14-1", "����ҵ��-���渶��");
            myht.Add("99-1", "����ѹ��");
            myht.Add("99-2", "����ѹ��");
            myht.Add("9999-1", "ʵʱ����-��Q");
            myht.Add("9998-1", "ʵʱ����-΢��");
            myht.Add("9997-1", "ʵʱ����-����");
            myht.Add("9996-1", "ʵʱ����-�Ƹ�ͨ");
            return myht;
        }
    }
}
