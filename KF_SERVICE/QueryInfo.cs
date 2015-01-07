using System;
using System.Configuration;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.Common;
using System.Data;
using System.Collections;
using System.IO;
using System.Web.Mail;

using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace TENCENT.OSS.CFT.KF.KF_Service
{

    public class QueryInfo
    {
        /// <summary>
        /// ��SELECT����з���һ��DATATABLE������.NET��WEBҳ����ʾ��
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static DataSet GetTable(string strSql, int iPageStart, int iPageMax)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString());
            //			DataTable result = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                //				result = da.GetTable(strSql);
                ds = da.dsGetTableByRange(strSql, iPageStart, iPageMax);

                return ds;
            }
            finally
            {
                da.Dispose();
            }
        }

        public static DataSet GetTable(string strSql, int iPageStart, int iPageMax, string dbStr)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString(dbStr));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTableByRange(strSql, iPageStart, iPageMax);
                da.Dispose();
                return ds;
            }
            catch (Exception e)
            {
                da.Dispose();
                throw e;
            }
        }

        public static DataSet GetTable_Conn(string strSql, int iPageStart, int iPageMax, string connstr)
        {
            MySqlAccess da = new MySqlAccess(connstr);
            //			DataTable result = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                //				result = da.GetTable(strSql);
                ds = da.dsGetTableByRange(strSql, iPageStart, iPageMax);

                da.Dispose();
                return ds;
            }
            catch (Exception e)
            {
                da.Dispose();
                throw e;
            }
        }

        public static int CHAR_HASH(char c)
        {
            return (c - '0') % 10;
        }

        public static string GetDbNum(string uin)
        {
            //��QQ��
            if (uin.Length >= 5 && uin.Length <= 10 & !uin.StartsWith("0"))
            {
                return uin;
            }

            //email�û�,һ�㲻�����5λ
            if (uin.Length < 5)
            {
                return "00000";
            }
            char[] chUin = uin.ToCharArray();
            // Email��ǰ2λ�ֿ�
            int iDb = CHAR_HASH(chUin[0]) * 10 + CHAR_HASH(chUin[1]);

            //���CHAR_HASH ���ظ�����ȡ����ֵ
            if (iDb < 0)
            {
                iDb = -iDb;
            }

            // Email����3λ�ֱ�
            int iTbl = CHAR_HASH(chUin[2]);

           
            //���CHAR_HASH ���ظ�����ȡ����ֵ
            if (iTbl < 0)
            {
                iTbl = -iTbl;

            }

            //����
            int iHost = CHAR_HASH(chUin[3]) * 10 + CHAR_HASH(chUin[4]);

            //���CHAR_HASH ���ظ�����ȡ����ֵ
            if (iHost < 0)
            {
                iHost = -iHost;
            }
            int sum = iDb + iTbl * 100 + iHost * 1000;
            return sum.ToString();
        }

        public static int GetDBNode(string uin)
        { 
            int m_uDBNodeCount = 3;//��̨����
            int uNode = Int32.Parse(uin) % 1367 % m_uDBNodeCount;
            return uNode;
        }

        #region �����ݱ���ת������ֵ��ͬʱ����NULL����
        public static string GetInt(object aValue)
        {
            try
            {
                if (aValue != null)
                {
                    long num = long.Parse(aValue.ToString());
                    return num.ToString();
                }
                else
                {
                    return "0";
                }
            }
            catch
            {
                return "0";
            }
        }

        public static string GetString(object aValue)
        {
            try
            {
                if (aValue != null)
                {
                    return aValue.ToString().Replace("\\", "\\\\").Replace("'", "\\'");
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }


        public static string GetDateTime(object aValue)
        {
            try
            {
                if (aValue != null)
                {
                    return (DateTime.Parse(aValue.ToString())).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }
        #endregion
    }


    public class Query_BaseForNET
    {
        protected string fstrSql;
        protected string fstrSql_count;

        protected string ICESql;
        protected string ICEcommand;
        /// <summary>
        /// �ṩ��.net���õĺ���
        /// </summary>
        /// <returns></returns>
        public DataSet GetResultX(int iPageStart, int iPageMax)
        {
            //			DataSet result = new DataSet();
            //			result.Tables.Add(QueryInfo.GetTable(fstrSql));
            return QueryInfo.GetTable(fstrSql, iPageStart, iPageMax);
        }

        /// <summary>
        /// �ṩ��.net���õĺ���
        /// </summary>
        /// <returns></returns>
        public DataSet GetResultX(int iPageStart, int iPageMax, string dbstr)
        {
            return QueryInfo.GetTable(fstrSql, iPageStart, iPageMax, dbstr);
        }
        
        //����ȫ�����߲�������
        public DataSet GetResultX_AllAndLimit(int iPageStart, int iPageMax, string dbstr)
        {
            if (iPageStart == -1 && iPageMax == -1)
                return PublicRes.returnDSAll(fstrSql, dbstr);
            else
                return QueryInfo.GetTable(fstrSql, iPageStart, iPageMax, dbstr);
        }

        public DataSet GetResultX_Conn(int iPageStart, int iPageMax, string connstr)
        {
            //			DataSet result = new DataSet();
            //			result.Tables.Add(QueryInfo.GetTable(fstrSql));
            return QueryInfo.GetTable_Conn(fstrSql, iPageStart, iPageMax, connstr);
        }

        public DataSet GetResultX()
        {
            return PublicRes.returnDSAll(fstrSql, "ywb");
        }

        public DataSet GetResultX_ICE()
        {
            string errmsg = "";
            DataSet ds = CommQuery.GetDataSetFromICE(ICESql, ICEcommand, out errmsg);
            return ds;
        }

        // 2012/4/9
        public DataSet GetResultX_ICE(string serviceName)
        {
            string errmsg = "";
            DataSet ds = CommQuery.GetDataSetFromICE(ICESql, ICEcommand, false, serviceName, out errmsg, false);

            return ds;
        }

        public DataSet GetResultX(string dbstr)
        {
            return PublicRes.returnDSAll(fstrSql, dbstr);
        }

        public DataSet GetResultX_Conn(string connstr)
        {
            return PublicRes.returnDSAll_Conn(fstrSql, connstr);
        }
        public int GetCount()
        {
            return Int32.Parse(PublicRes.ExecuteOne(fstrSql_count, "ywb"));
        }

        public int GetCount(string dbstr)
        {
            return Int32.Parse(PublicRes.ExecuteOne(fstrSql_count, dbstr));
        }

        public int GetCountApeal(string dbstr)
        {
            if (fstrSql_count == "")
                return 0;
            else
                return Int32.Parse(PublicRes.ExecuteOne(fstrSql_count, dbstr));
        }

        public int GetCount_Conn(string connstr)
        {
            return Int32.Parse(PublicRes.ExecuteOne_Conn(fstrSql_count, connstr));
        }
    }

    #region �û��̼ҹ��߰�ť���ѯ����

    /// <summary>
    /// �û��̼ҹ��߰�ť��Ĳ�ѯ�ദ��
    /// </summary>
    public class Q_BUTTONINFO : Query_BaseForNET
    {
        private string f_strID;
        public Q_BUTTONINFO(string strID, int istr, int imax)
        {
            f_strID = strID;

            string tname = PublicRes.GetTableName("t_button_info", f_strID);
            string sWhereStr = " where " + PublicRes.GetSqlFromQQ(strID, "fowner_uin");

            string count = PublicRes.ExecuteOne("select count(*) from " + tname + sWhereStr, "ZJB");

            //fstrSql = "Select A.*,B.total from " 
            //+ tname + " A,(Select count(1) as total from " + tname + sWhereStr + ") B"
            fstrSql = "select *," + count + " as total from " + tname
                + sWhereStr
                + " ORDER By Fcreate_time DESC limit " + (istr - 1) + "," + imax;

        }
    }
    #endregion


    // 2011/7/21

    #region   2011/7/21


    /// <summary>
    /// ��ѯͶ����ǩԼ��Լ��Ϣ
    /// </summary>
    public class Q_InvestorSignInfo : Query_BaseForNET
    {
        public Q_InvestorSignInfo(int signType, string strID, string serialno, string cerNum, string spid, string spName, string beginTimeStr
            , string endTimeStr, int lim_start, int lim_count)
        {
            // finace.xml��Ӧ��SQL��������
            this.ICEcommand = "QUERY_SIGNINFO_BYDETAIL";

            if (signType != 1 && signType != 2)
                throw new Exception("�ò�ѯ��֧����������������");

            this.ICESql = "fop_type=" + signType;

            string fuid1 = "";
            string fuid2 = "";

            if (strID != null && strID.Trim() != "")
            {
                fuid1 = PublicRes.ConvertToFuid(strID);

                if (fuid1 == null || fuid1.Trim() == "")
                    throw new Exception("�Ƹ�ͨ�ʺ���Ч����ȷ������");

                this.ICESql += "&uid=" + fuid1;
            }

            if (serialno != null && serialno.Trim() != "")
                this.ICESql += "&cft_serialno=" + serialno;

            /*
            if(cerNum != null && cerNum.Trim() != "")
            {
                string errMsg = "";

                fuid2 = CommQuery.GetOneResultFromICE("cre_id="+cerNum,"QUERY_UID_BYCERINFO","Fuid",out errMsg);

                if(fuid1 != null && fuid1.Trim() != "" && fuid1.Trim() != "0" && fuid2 != fuid1)
                {
                    throw new Exception("֤���źͲƸ�ͨ�ʺŲ�ƥ�䣬��������");
                }

                if(fuid1 == null || fuid1.Trim() == "" || fuid1.Trim() == "0")
                {
                    this.ICESql += "&uid=" + fuid2;
                }
            }
            */

            if (spid != "")
                this.ICESql += "&spid=" + spid;

            if (spName != null && spName.Trim() != "")
                this.ICESql += "&sp_uname=" + spName;

            if (beginTimeStr.Trim() != "" && endTimeStr.Trim() != "")
            {
                this.ICESql += "&starttime=" + beginTimeStr;
                this.ICESql += "&endtime=" + endTimeStr;
            }

            this.ICESql += "&lim_start=" + lim_start;
            this.ICESql += "&lim_count=" + lim_count;
        }
    }


    public class Q_FundInfo : Query_BaseForNET
    {
        /// <summary>
        /// �ԲƸ�ͨ�������ʺŻ�����ʺŻ�����һ���ѯ������Ϣ
        /// </summary>
        /// <param name="listID"></param>
        /// <param name="findDate"></param>
        /// <param name="limStart"></param>
        /// <param name="limCount"></param>
        public Q_FundInfo(string listID, string uid, string beginDateStr, string endDateStr, string fundType, int limStart, int limCount)
        {
            if (listID != null && listID.Trim() != "")
            {
                this.ICEcommand = "QUERY_FUNDINFO_LISTID";

                this.ICESql += "listid=" + listID;

                if (fundType != "" && fundType != "0")
                {
                    this.ICESql += "&purtype=" + fundType;
                }

                this.ICESql += "&lim_start=" + limStart;
                this.ICESql += "&lim_count=" + limCount;

                return;
            }
            else if (uid != null || uid.Trim() != "")
            {
                this.ICEcommand = "QUERY_FUNDINFO_UID";

                string fuid = PublicRes.ConvertToFuid(uid);

                if (fuid == null || fuid.Trim() == "")
                    fuid = "0";

                this.ICESql += "uid=" + fuid;
                // �����õ�����
                //this.ICESql += "uid=" + 0;
            }


            if (beginDateStr != null && beginDateStr.Trim() != "" && endDateStr != null && endDateStr.Trim() != "")
            {
                //��̨��Ҫ�ĸ�ʽ��yyyyMMdd 
                beginDateStr = DateTime.Parse(beginDateStr).ToString("yyyyMMdd");
                endDateStr = DateTime.Parse(endDateStr).ToString("yyyyMMdd");

                this.ICESql += "&begin_date=" + beginDateStr;
                this.ICESql += "&end_date=" + endDateStr;
            }

            if (fundType != "" && fundType != "0")
                this.ICESql += "&purtype=" + fundType;


            this.ICESql += "&lim_start=" + limStart;
            this.ICESql += "&lim_count=" + limCount;
        }

    }




    public class Q_ChargeInfo : Query_BaseForNET
    {
        public Q_ChargeInfo(string qType, string strID, string beginDateStr, string endDateStr, string listid, int lim_start, int lim_count)
        {
            string fuid;
            if (strID != null && strID.Trim() != "")
            {
                this.ICEcommand = "QUERY_USER_BANKROLL_FULL";

                fuid = PublicRes.ConvertToFuid(strID);

                if (fuid == null || fuid.Trim() == "")
                    fuid = "0";

                this.ICESql = "uid=" + fuid;
            }
            else
            {
                throw new Exception("��ѯ���������û��ʺ�");
            }

            // �����listid��ѯ����ʹ��QUERY_BANKROLL_LISTID_2��������ӿ�ֻ����2������
            if (listid != null && listid.Trim() != "")
            {
                this.ICEcommand = "QUERY_BANKROLL_LISTID_2";

                this.ICESql += "&listid=" + listid;
            }
            else
            {
                if (qType != "" && qType == "99")
                {
                    this.ICESql += "&subject_list=11,14";
                }
                else if (qType != "" && qType != "0")
                {
                    this.ICESql += "&subject_list=" + qType;
                }

                if (beginDateStr != "" && endDateStr != "")
                {
                    this.ICESql += "&start_time=" + beginDateStr;
                    this.ICESql += "&end_time=" + endDateStr;
                }

                this.ICESql += "&lim_start=" + lim_start;
                this.ICESql += "&lim_count=" + lim_count;

                // curType=2�������
                this.ICESql += "&curtype=2";
            }
            string str = this.ICESql;
        }
    }



    #endregion

    // 2012/4

    #region  2012/4

    // ��ʵ����֤��(���̱�)��Ϣ
    public class QueryUserAuthenStateInfo : Query_BaseForNET
    {
        public QueryUserAuthenStateInfo(string userAcc, string bankID, int bankType)
        {
            if (userAcc.Trim() != "")
            {
                this.ICEcommand = "FINANCE_QUERY_USR_AUTHENING";

                this.ICESql = "qqid=" + userAcc;
            }
            else if (bankID.Trim() != "")
            {
                this.ICEcommand = "FINANCE_QUERY_CARD_AUTHENING";

                this.ICESql = "bank_id=" + bankID + "&bank_type=" + bankType;
            }
            else
            {
                throw new Exception("��ѯ����������д��");
            }
        }
    }


    // ����ͨ����֤��Ϣ
    public class QueryUserAuthenedStateInfo : Query_BaseForNET
    {
        public QueryUserAuthenedStateInfo(string userAcc)
        {
            this.ICEcommand = "FINANCE_QUERY_USER_AUTHENED";
            string fuid = PublicRes.ConvertToFuid(userAcc);
            this.ICESql = "uid=" + fuid;
        }
    }



    public class QueryAuthenRelationInfo : Query_BaseForNET
    {
        public QueryAuthenRelationInfo(string firstAuthenID)
        {
            if (firstAuthenID == null || firstAuthenID.Trim() == "")
            {
                throw new Exception("firstAuthenID����Ϊ�գ�");
            }

            this.ICEcommand = "FINANCE_QUERY_RELATION";

            this.ICESql = "uid=" + firstAuthenID;
        }
    }



    public class QueryAuthenInfo_ByCre2 : Query_BaseForNET
    {
        public QueryAuthenInfo_ByCre2(string creid, int creType)
        {
            if (creid == null || creid.Trim() == "")
                throw new Exception("֤���Ų���Ϊ��");

            this.ICEcommand = "FINANCE_QUERY_CRE_RELATION";

            this.ICESql = "cre_id=" + creid + "&cre_type=" + creType;
        }
    }







    #endregion

    // 2012/6
    #region	����

    public class QueryDKInfo : Query_BaseForNET
    {
        public QueryDKInfo(string cep_id, string strBeginDate, string strEndDate)
        {
            this.ICEcommand = "FINANCE_QUERY_CEPSPLIST";

            this.ICESql += "cep_id=" + cep_id;
            this.ICESql += "&stime=" + strBeginDate;
            this.ICESql += "&etime=" + strEndDate;
            //this.ICESql += "&lim_start=0&lim_count=1";
            this.ICESql += "&strlimit=limit 0,1";
        }

        public QueryDKInfo(string explain, string bankID, string userID, string strBeginDate, string strEndDate, string spid, string spListID
            , string spBatchID, string cep_id,string state, string transaction_id, string bank_type, string service_code, int limStart, int limMax)
        {
            string strWhere = "";
            strWhere = " where Fcreate_time between '" + strBeginDate + "' and '" + strEndDate + "' ";
            if (spid.Trim() != "")
                strWhere += " and Fspid='" + spid.Trim() + "' ";
            if (bankID.Trim() != "")
                strWhere += " and Fbankacc_no='" + bankID.Trim() + "' ";
            if (userID.Trim() != "")
                strWhere += " and Funame='" + userID.Trim() + "' ";
            if (spListID.Trim() != "")
                strWhere += " and Fcoding='" + spListID.Trim() + "' ";
            if (spBatchID.Trim() != "")
                strWhere += " and Fsp_batchid='" + spBatchID.Trim() + "' ";
            if (cep_id.Trim() != "")
                strWhere += " and Fcep_id='" + cep_id.Trim() + "' ";
            if (state.Trim() != "" && state != "0")
            {
                if (state == "3")
                    strWhere += " and Fstate<8 ";
                if (state == "1")
                    strWhere += " and Fstate=8";//״̬8���ݿ��гɹ�
                if (state == "2")
                    strWhere += " and Fstate=9";//״̬9���ݿ���ʧ��
            }

            if (transaction_id.Trim() != "")
                strWhere += " and Ftransaction_id='" + transaction_id + "' ";
            if (bank_type.Trim() != "")
                strWhere += " and Fbank_type='" + bank_type + "' ";
            if (service_code.Trim() != "9999999")
                strWhere += " and Fservice_code like '%" + service_code + "' ";
            if (explain.Trim() != "")
                strWhere += " and Fexplain like'" + explain + "%' ";

            fstrSql = " select * from cft_cep_db.t_cep_list " + strWhere;
            fstrSql_count = " select count(*) from cft_cep_db.t_cep_list " + strWhere;


            this.ICEcommand = "FINANCE_QUERY_CEPSPLIST";

            this.ICESql += "stime=" + strBeginDate;
            this.ICESql += "&etime=" + strEndDate;

            if (spid.Trim() != "")
                this.ICESql += "&sp_id=" + spid;

            if (bankID.Trim() != "")
                this.ICESql += "&bankacc_no=" + bankID;

            if (userID.Trim() != "")
                this.ICESql += "&name=" + userID;

            if (spListID.Trim() != "")
                this.ICESql += "&coding=" + spListID;

            if (spBatchID.Trim() != "")
                this.ICESql += "&sp_batchid=" + spBatchID;

            if (state.Trim() != "")
            {
                // �����д���ҳ��ȷ��
                if (state == "3")
                {
                    this.ICESql += "&processing=";
                }
                else if (state == "1")
                {
                    this.ICESql += "&success_state=";
                }
                else if (state == "2")
                {
                    this.ICESql += "&failed_state=";
                }
            }

            this.ICESql += "&upbymodfy=";

            //this.ICESql += "&lim_start=" + limStart + "&lim_count=" + limMax;
            this.ICESql += "&strlimit=limit " + limStart + "," + limMax;

            string str = this.ICESql;
        }
    }



    public class QueryBatchDKInfo : Query_BaseForNET
    {
        public QueryBatchDKInfo(string strBeginDate, string strEndDate, string spid, string spBatchID, string batchid, string state, int limStart, int limMax)
        {
            string strWhere = " where Fcreate_time between '" + strBeginDate + "' and '" + strEndDate + "' ";
            if (spid.Trim() != "")
            {
                strWhere += " and Fspid='" + spid + "' ";
            }

            if (spBatchID.Trim() != "")
                strWhere += " and Fsp_batchid='" + spBatchID + "' ";

            if (batchid.Trim() != "")
                strWhere += " and Fbatchid='" + batchid + "' ";

            if (state != "0")
                strWhere += " and Fstate=" + state;

            fstrSql = " select * from cft_cep_db.t_batch_record " + strWhere;
            fstrSql_count = " select count(*) from cft_cep_db.t_batch_record " + strWhere;

            this.ICEcommand = "FINANCE_QUERY_CEPSPBATCH";

            this.ICESql += "stime=" + strBeginDate;
            this.ICESql += "&etime=" + strEndDate;

            if (spid.Trim() != "")
                this.ICESql += "&sp_id=" + spid;

            if (spBatchID.Trim() != "")
                this.ICESql += "&sp_batchid=" + spBatchID;

            //this.ICESql += "&upbymodfy=";

            this.ICESql += "&strlimit=limit " + limStart + "," + limMax;

            string str = this.ICESql;

        }
    }



    public class CepspService : Query_BaseForNET
    {
        public CepspService(string spid, string service_code)
        {
            this.ICEcommand = "FINANCE_QUERY_CEPSPSERVICE";

            this.ICESql += "sp_id=" + spid;
            this.ICESql += "&service_code=" + service_code;
        }
    }


    #endregion


    #region �û��˻����ѯ����

    /// <summary>
    /// �û��ʻ���Ĳ�ѯ�ദ��
    /// </summary>
    public class Q_USER : Query_BaseForNET
    {
        private string f_strID;
        public string FlagForTable;
        public Q_USER(string fuid, int fcurtype)
        {
          //  f_strID = strID;

         //   string fuid = PublicRes.ConvertToFuid(strID);//תfuidת�����ú���
            if (fuid == null)
                fuid = "0";

            // TODO: 1�ͻ���Ϣ��������
            //�Ȱ�email��mobile��t_user_info��ȡ��,�ٷ����SQL��.
            //MySqlAccess da_zl = new MySqlAccess(PublicRes.GetConnString("ZL"));
            string femail = "";
            string fmobile = "";
            string fatt_id = "";
            string ftrueName = "";
            string fz_amt = ""; //���˶����� yinhuang 2014/1/8

            ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            try
            {
                string errMsg = "";
                string strSql = "uid=" + fuid;
                //femail = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Femail", out errMsg);
                //fmobile = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fmobile", out errMsg);
                fatt_id = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERATT, "Fatt_id", out errMsg);

                fatt_id = QueryInfo.GetString(fatt_id);

                DataTable dt_userInfo = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_USERINFO, out errMsg);
                if (dt_userInfo != null && dt_userInfo.Rows.Count == 1)
                {
                    femail = dt_userInfo.Rows[0]["Femail"].ToString();
                    fmobile = dt_userInfo.Rows[0]["Fmobile"].ToString();
                    ftrueName = dt_userInfo.Rows[0]["FtrueName"].ToString();

                    string fusertype = QueryInfo.GetString(dt_userInfo.Rows[0]["Fuser_type"]);
                    if (fusertype == "2")//��˾����
                    {
                        ftrueName = dt_userInfo.Rows[0]["Fcompany_name"].ToString();
                    }
                }

                ice.OpenConn();
                string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
                strwhere += ICEAccess.URLEncode("fcurtype="+fcurtype+"&");

                string strResp = "";
                DataTable dt = ice.InvokeQuery_GetDataTable(YWSourceType.�û���Դ, YWCommandCode.��ѯ�û���Ϣ, fuid, strwhere, out strResp);
                if (dt == null || dt.Rows.Count == 0)
                    throw new LogicException("����ICE��ѯT_user�޼�¼" + strResp);

                // 2012/5/2 ��Ϊ��ҪQ_USER_INFO��ȡ׼ȷ���û���ʵ�������Ķ�
                //ȡ�����ȡt_user_info
                /*
                dt.Columns.Add("UserRealName2", typeof(string));

                try
                {
                    Q_USER_INFO cuser = new Q_USER_INFO(fuid);
                    DataSet ds2 = cuser.GetResultX(1, 1, "ZW");

                    if (ds2 != null && ds2.Tables.Count != 0 && ds2.Tables[0].Rows.Count != 0)
                    {
                        dt.Rows[0]["UserRealName2"] = ds2.Tables[0].Rows[0]["Ftruename"].ToString();
                    }
                }
                catch
                { }
                */

                ice.CloseConn();

                da.OpenConn();
                string sql = "select * from app_platform.t_account_freeze where Fuid = '" + fuid + "'";
                DataTable dt2 = da.GetTable(sql);
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    fz_amt = dt2.Rows[0]["Famount"].ToString();
                }

                //��dt���һ����¼��ϳ�select��䡣
                string strtmp = " select ";
                foreach (DataColumn dc in dt.Columns)
                {
                    string valuetmp = QueryInfo.GetString(dt.Rows[0][dc.ColumnName]);
                    strtmp += " '" + valuetmp + "' as " + dc.ColumnName + ",";
                }

                fstrSql = strtmp + "'" + femail + "' as Femail,'" + fmobile + "' as Fmobile, '" + fatt_id + "' as Att_id, '" + ftrueName + "' as UserRealName2, '" + fz_amt + "' as Ffz_amt ";
            }
            finally
            {
                ice.Dispose();
                da.Dispose();
            }
        }

        //Ϊ���ʻ�ר��
        public Q_USER(string strID, string Fcurtype)
        {
            f_strID = strID;

            string fuid = PublicRes.ConvertToFuid(strID);
            if (fuid == null)
                fuid = "0";

            // TODO: 1�ͻ���Ϣ��������

            //MySqlAccess da_zl = new MySqlAccess(PublicRes.GetConnString("ZL"));

            ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
            string femail = "";
            string fmobile = "";
            try
            {
                /*
                da_zl.OpenConn();

                string strSql = "select Femail from " + PublicRes.GetTName("t_user_info",fuid) + " where fuid=" + fuid;
				
                femail = QueryInfo.GetString(da_zl.GetOneResult(strSql));

                strSql = "select Fmobile from " + PublicRes.GetTName("t_user_info",fuid) + " where fuid=" + fuid;
				
                fmobile = QueryInfo.GetString(da_zl.GetOneResult(strSql));
                */

                string errMsg = "";
                string strSql = "uid=" + fuid;
                femail = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Femail", out errMsg);
                fmobile = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fmobile", out errMsg);

                femail = QueryInfo.GetString(femail);
                fmobile = QueryInfo.GetString(fmobile);
            }
            finally
            {
                //da_zl.Dispose();
            }

            /*
            string FlagTable = PublicRes.GetTName("t_user",fuid);
            fstrSql = "Select *,'" + femail +"' as Femail,'" + fmobile + "' as Fmobile from " + FlagTable + "  where fuid='" + fuid + "' and Fcurtype='" + Fcurtype + "'";

            if(FlagTable.IndexOf("t_user") >= 0)
                FlagForTable = "USER";
                */

            ice.OpenConn();
            string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
            strwhere += ICEAccess.URLEncode("fcurtype=" + Fcurtype + "&");

            string strResp = "";
            DataTable dt = ice.InvokeQuery_GetDataTable(YWSourceType.�û���Դ, YWCommandCode.��ѯ�û���Ϣ, fuid, strwhere, out strResp);
            if (dt == null || dt.Rows.Count == 0)
                throw new LogicException("����ICE��ѯT_user�޼�¼" + strResp);

            ice.CloseConn();

            //��dt���һ����¼��ϳ�select��䡣
            string strtmp = " select ";
            foreach (DataColumn dc in dt.Columns)
            {
                string valuetmp = QueryInfo.GetString(dt.Rows[0][dc.ColumnName]);
                strtmp += " '" + valuetmp + "' as " + dc.ColumnName + ",";
            }

            fstrSql = strtmp + "'" + femail + "' as Femail,'" + fmobile + "' as Fmobile ";

        }

        /// <summary>
        /// �ṩ���������Ե��õĺ������Թ̶����෵��ֵ��
        /// </summary>
        /// <returns></returns>
        public T_USER GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));
            T_USER result = new T_USER();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    result.u_APay = QueryInfo.GetInt(dr["fapay"]);
                    result.u_Balance = QueryInfo.GetInt(dr["fBalance"]);
                    result.u_Con = QueryInfo.GetInt(dr["fcon"]);
                    result.u_CurType = QueryInfo.GetInt(dr["fcurtype"]);
                    result.u_Fetch_Time = QueryInfo.GetDateTime(dr["ffetch_time"]);
                    result.u_Login_IP = QueryInfo.GetString(dr["flogin_ip"]);
                    result.u_Memo = QueryInfo.GetString(dr["fmemo"]);
                    result.u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                    result.u_Modify_Time_C2C = QueryInfo.GetDateTime(dr["fmodify_time_c2c"]);
                    result.u_Quota = QueryInfo.GetInt(dr["fquota"]);
                    result.u_Quota_Pay = QueryInfo.GetInt(dr["fquota_pay"]);
                    result.u_Save_Time = QueryInfo.GetDateTime(dr["fsave_time"]);
                    result.u_State = QueryInfo.GetInt(dr["fstate"]);
                    result.u_TrueName = QueryInfo.GetString(dr["ftruename"]);
                    result.u_Yday_Balance = QueryInfo.GetInt(dr["fyday_balance"]);
                    result.u_User_Type = QueryInfo.GetInt(dr["fuser_type"]);

                    result.u_QQID = QueryInfo.GetString(dr["fqqid"]);
                }
                else
                {
                    throw new Exception("û�в��ҵ���Ӧ�ļ�¼");
                }
                da.CloseConn();
                da.Dispose();
                return result;
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }
    }

    #endregion


    #region �н��ʻ���Ĳ�ѯ����


    /// <summary>
    /// �н��˻���Ĳ�ѯ��
    /// </summary>
    public class Q_USER_MED : Query_BaseForNET
    {
        private string f_strID;

        //��ѯ�õ����е��̻����ݼ� edwardzheng 20051110
        public Q_USER_MED()
        {
            /*
            //fstrSql = "Select Fuid,FQqid,FSpid,Ftruename from c2c_db.t_middle_user ORDER BY FSpid";
            fstrSql = "Select FuidMiddle as Fuid,Fspecial as Fqqid,FSpid,Fname as Ftruename from c2c_db.t_merchant_info ORDER BY FSpid";
            */
            //����Ӧ�ò��ٴ���ȡ�����̻���Ϣ�ĵط�.
            fstrSql = "Select '10001' as Fuid,'10001' as Fqqid,'1000000000' as FSpid,'test' as Ftruename";
        }

        public Q_USER_MED(string strID)
        {
            /*
            //f_strID = PublicRes.ExecuteOne("select fuid from c2c_db.t_middle_user where fspid='" + strID + "'","YW_30");
            //f_strID = PublicRes.ExecuteOne("select fuid from c2c_db.t_middle_user where fspid='" + strID + "'","YWB_30");
            f_strID = PublicRes.ExecuteOne("select fuidMiddle from c2c_db.t_merchant_info where fspid='" + strID + "'","ZL");
            */

            string strSql = "spid=" + strID;
            string errMsg = "";
            f_strID = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, "FuidMiddle", out errMsg);

            if (f_strID == null || f_strID.Trim() == "")
            {
                throw new LogicException("���Ҳ���ָ���ļ�¼����ȷ����������Ƿ���ȷ��" + errMsg);
            }


            //MySqlAccess da_zl = new MySqlAccess(PublicRes.GetConnString("ZL"));



            //furion 20080707 ��Ϊ��ǰ��ȡ��������ͬ�ֶ����ƵĻ�����ȡ���һ�γ����ֶε�ֵ��
            string strusertype = "";

            string strtmp = " ";

            ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
            try
            {
                //da_zl.OpenConn();

                ice.OpenConn();
                string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + f_strID + "&");
                strwhere += ICEAccess.URLEncode("fcurtype=1&");

                string strResp = "";

                //3.0�ӿڲ�����Ҫ furion 20090708
                DataTable dt = ice.InvokeQuery_GetDataTable(YWSourceType.�̻���Դ, YWCommandCode.��ѯ�̻���Ϣ, f_strID, strwhere, out strResp);
                if (dt == null || dt.Rows.Count == 0)
                    throw new LogicException("����ICE��ѯT_user�޼�¼" + strResp);

                ice.CloseConn();

                //				string strSql  = "Select "+
                //					"C.FUid AS FBindUid,C.FFeeContract,C.FContract,C.Fspecial AS FBindQQid,C.FUserId AS FModifyUser,"+
                //					"C.FModify_time AS FModifyTime,C.Fmer_key,C.Fstate,C.Flist_state,"+
                //					"C.Fstandby1,C.Fstandby2,C.Fstandby3,C.Fstandby4,C.Fstandby5,C.Fstandby6,C.Fstandby7,C.Fstandby8,C.Fstandby9,C.Fstandby10,"+
                //					"C.Fmemo AS FExtMemo,C.FIp AS FModifyIp,B.* "+
                //					" from  " + PublicRes.GetTName("t_user_info",f_strID) + " B "+
                //					" LEFT OUTER JOIN c2c_db.t_merchant_info C ON C.Fuidmiddle=B.Fuid "+
                //					" where B.Fuid ='" + f_strID + "'";
                //
                //				DataTable dt = da_zl.GetTable(strSql);

                //				if(dt ==null || dt.Rows.Count != 1)
                //				{
                //					throw new LogicException("���Ҳ���ָ���ļ�¼����ȷ����������Ƿ���ȷ��");
                //				}



                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName.ToLower() == "fuser_type" || dc.ColumnName.ToLower() == "fatt_id")
                    {
                        //strusertype = " '" + valuetmp + "' as " + dc.ColumnName;
                        continue;
                    }

                    string valuetmp = QueryInfo.GetString(dt.Rows[0][dc.ColumnName]);
                    strtmp += " '" + valuetmp + "' as " + dc.ColumnName + ",";
                }

                strtmp = strtmp.Remove(strtmp.Length - 1, 1);
            }
            finally
            {
                //da_zl.Dispose();
                ice.Dispose();
            }

            /*
            // TODO: 1�ͻ���Ϣ��������
            //			fstrSql = "Select "+
            //				"C.FUid AS FBindUid,C.FFeeContract,C.FContract,C.Fspecial AS FBindQQid,C.FUserId AS FModifyUser,"+
            //				"C.FModify_time AS FModifyTime,C.Fmer_key,C.Fstate,C.Flist_state,"+
            //				"C.Fstandby1,C.Fstandby2,C.Fstandby3,C.Fstandby4,C.Fstandby5,C.Fstandby6,C.Fstandby7,C.Fstandby8,C.Fstandby9,C.Fstandby10,"+
            //				"C.Fmemo AS FExtMemo,C.FIp AS FModifyIp,A.*,B.* "+
            //				" from c2c_db.t_middle_user A LEFT OUTER JOIN " + PublicRes.GetTName("t_user_info",f_strID) + " B ON B.Fuid=A.Fuid"+
            //				" LEFT OUTER JOIN c2c_db.t_merchant_info C ON A.Fspid=C.FSpid"+
            //				" where A.Fuid ='" + f_strID + "'";

            //furion 20080707
            fstrSql  = "select " +
                "C.FUid AS FBindUid,C.FFeeContract,C.FContract,C.Fspecial AS FBindQQid,C.FUserId AS FModifyUser,"+
                "C.FModify_time AS FModifyTime,C.Fmer_key,C.Fstate,C.Flist_state,"+
                "C.Fstandby1,C.Fstandby2,C.Fstandby3,C.Fstandby4,C.Fstandby5,C.Fstandby6,C.Fstandby7,C.Fstandby8,C.Fstandby9,C.Fstandby10,"+
                "C.Fmemo AS FExtMemo,C.FIp AS FModifyIp,B.*, " + strtmp +
                " from  " + PublicRes.GetTName("t_user_info",f_strID) + " B "+
                " LEFT OUTER JOIN c2c_db.t_merchant_info C ON C.Fuidmiddle=B.Fuid "+
                " where B.Fuid ='" + f_strID + "'";

//			fstrSql = strtmp + "A.* ," + strusertype +
//				" from c2c_db.t_middle_user A  where A.Fuid ='" + f_strID + "'";
*/

            errMsg = "";
            strSql = "spid=" + strID;
            DataTable dtc = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, out errMsg);

            if (dtc == null || dtc.Rows.Count != 1)
            {
                throw new LogicException(errMsg);
            }

            string csql = "";
            DataRow dr = dtc.Rows[0];
            csql += " '" + QueryInfo.GetString(dr["Fuid"]) + "' as FBindUid,";
            csql += " '" + QueryInfo.GetString(dr["FFeeContract"]) + "' as FFeeContract,";
            csql += " '" + QueryInfo.GetString(dr["FContract"]) + "' as FContract,";
            csql += " '" + QueryInfo.GetString(dr["Fspecial"]) + "' as FBindQQid,";

            csql += " '" + QueryInfo.GetString(dr["FUserId"]) + "' as FModifyUser,";
            csql += " '" + QueryInfo.GetString(dr["FModify_time"]) + "' as FModifyTime,";
            csql += " '" + QueryInfo.GetString(dr["Fmer_key"]) + "' as Fmer_key,";
            csql += " '" + QueryInfo.GetString(dr["Fstate"]) + "' as Fstate,";
            csql += " '" + QueryInfo.GetString(dr["Flist_state"]) + "' as Flist_state,";

            csql += " '" + QueryInfo.GetString(dr["Fmemo"]) + "' as FExtMemo,";
            csql += " '" + QueryInfo.GetString(dr["FIp"]) + "' as FModifyIp,";

            csql += " '" + QueryInfo.GetString(dr["Fstandby1"]) + "' as Fstandby1,";
            csql += " '" + QueryInfo.GetString(dr["Fstandby2"]) + "' as Fstandby2,";
            csql += " '" + QueryInfo.GetString(dr["Fstandby3"]) + "' as Fstandby3,";
            csql += " '" + QueryInfo.GetString(dr["Fstandby4"]) + "' as Fstandby4,";
            csql += " '" + QueryInfo.GetString(dr["Fstandby5"]) + "' as Fstandby5,";
            csql += " '" + QueryInfo.GetString(dr["Fstandby6"]) + "' as Fstandby6,";
            csql += " '" + QueryInfo.GetString(dr["Fstandby7"]) + "' as Fstandby7,";
            csql += " '" + QueryInfo.GetString(dr["Fstandby8"]) + "' as Fstandby8,";
            csql += " '" + QueryInfo.GetString(dr["Fstandby9"]) + "' as Fstandby9,";
            csql += " '" + QueryInfo.GetString(dr["Fstandby10"]) + "' as Fstandby10,";


            strSql = "uid=" + f_strID;
            DataTable dtb = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_USERINFO, out errMsg);

            if (dtb == null || dtb.Rows.Count != 1)
            {
                throw new LogicException(errMsg);
            }

            string bsql = "";
            foreach (DataColumn dc in dtb.Columns)
            {
                string valuetmp = QueryInfo.GetString(dtb.Rows[0][dc.ColumnName]);
                bsql += " '" + valuetmp + "' as " + dc.ColumnName + ",";
            }


            fstrSql = "select " + csql + bsql + strtmp;

            strSql = "uid=" + f_strID;
            string Fatt_id = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERATT, "Fatt_id", out errMsg);

            fstrSql += "," + Fatt_id + " as Fatt_id";
        }

        /// <summary>
        /// �ṩ���������Ե��õĺ������Թ̶����෵��ֵ��
        /// </summary>
        /// <returns></returns>
        public T_USER_MED GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));
            T_USER_MED result = new T_USER_MED();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    result.GetInfoFromDB(dr);
                }
                else
                {
                    throw new LogicException("û�в��ҵ���Ӧ�ļ�¼");
                }
                return result;
            }
            finally
            {
                da.CloseConn();
                da.Dispose();
            }
        }

        /// <summary>
        /// �ṩ���������Ե��õĺ������Թ̶����෵��ֵ��
        /// </summary>
        /// <returns></returns>
        public T_USER_MED GetResult(string DBstr)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString(DBstr));
            T_USER_MED result = new T_USER_MED();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    result.LoadFromDB(dr);
                }
                else
                {
                    throw new LogicException("û�в��ҵ���Ӧ�ļ�¼");
                }
                return result;
            }
            finally
            {
                da.CloseConn();
                da.Dispose();
            }
        }
    }

    #endregion


    #region �û����ϱ�Ĳ�ѯ����


    /// <summary>
    /// �û����ϱ�Ĳ�ѯ��
    /// </summary>
    public class Q_USER_INFO : Query_BaseForNET
    {
        private string f_strID;
        public Q_USER_INFO(string fuid)
        {
          //  f_strID = strID;

          //  string fuid = PublicRes.ConvertToFuid(strID);

            // TODO: 1�ͻ���Ϣ��������
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YWB_30"));
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));
            string Fatt_id = "";
            try
            {
                /*
                da.OpenConn();

                //string strSql = "select Fatt_id from " + PublicRes.GetTableName("t_user",f_strID)  + " where " + PublicRes.GetSqlFromQQ(strID,"fuid");
                string strSql = "select Fatt_id from " + PublicRes.GetTName("t_useratt_info",fuid)  + " where fuid=" + fuid;
				
                Fatt_id = QueryInfo.GetString(da.GetOneResult_OBJ(strSql));
                */

                string errmsg = "";
                string strSql = "uid=" + fuid;
                Fatt_id = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERATT, "Fatt_id", out errmsg);
            }
            finally
            {
                //da.Dispose();
            }

            /*
            //fstrSql = "Select * from " + PublicRes.GetTableName("t_user_info",f_strID) + " where " + PublicRes.GetSqlFromQQ(strID,"fuid");
            //furion 20060816
            //			fstrSql = "Select A.*,B.Fatt_id from " + PublicRes.GetTableName("t_user_info",f_strID) + " A,"
            //                    + PublicRes.GetTableName("t_user",f_strID) + " B where A.Fuid=B.Fuid and A." + PublicRes.GetSqlFromQQ(strID,"fuid");

            fstrSql = "Select *,'" + Fatt_id + "' as Fatt_id from " + PublicRes.GetTableName("t_user_info",f_strID) + " where " + PublicRes.GetSqlFromQQ(strID,"fuid");
            */

            string errMsg = "";
            string strsql = "uid=" + fuid;
            DataTable dt = CommQuery.GetTableFromICE(strsql, CommQuery.QUERY_USERINFO, out errMsg);

            if (dt == null || dt.Rows.Count != 1)
            {
                throw new LogicException(errMsg);
            }

            fstrSql = "select ";
            foreach (DataColumn dc in dt.Columns)
            {
                string valuetmp = QueryInfo.GetString(dt.Rows[0][dc.ColumnName]);
                fstrSql += " '" + valuetmp + "' as " + dc.ColumnName + ",";
            }

            fstrSql += "'" + Fatt_id + "' as Fatt_id";

        }

        /// <summary>
        /// �ṩ���������Ե��õĺ������Թ̶����෵��ֵ��
        /// </summary>
        /// <returns></returns>
        public T_USER_INFO GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));
            T_USER_INFO result = new T_USER_INFO();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    result.u_Address = QueryInfo.GetString(dr["faddress"]);
                    result.u_Age = QueryInfo.GetInt(dr["fage"]);
                    result.u_Area = QueryInfo.GetString(dr["farea"]);
                    result.u_City = QueryInfo.GetString(dr["fcity"]);
                    result.u_Cre_Type = QueryInfo.GetInt(dr["fcre_type"]);
                    result.u_Credit = QueryInfo.GetInt(dr["fcredit"]);
                    result.u_CreID = QueryInfo.GetString(dr["fcreid"]);
                    result.u_Email = QueryInfo.GetString(dr["femail"]);
                    result.u_Memo = QueryInfo.GetString(dr["fmemo"]);
                    result.u_Mobile = QueryInfo.GetString(dr["fmobile"]);
                    result.u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                    result.u_PCode = QueryInfo.GetString(dr["fpcode"]);
                    result.u_Phone = QueryInfo.GetString(dr["fphone"]);
                    result.u_QQID = QueryInfo.GetString(dr["fqqid"]);
                    result.u_Sex = QueryInfo.GetInt(dr["fsex"]);
                    result.u_TrueName = QueryInfo.GetString(dr["ftruename"]);
                }
                else
                {
                    throw new Exception("û�в��ҵ���Ӧ�ļ�¼");
                }
                da.CloseConn();
                da.Dispose();
                return result;
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }
    }

    #endregion


    #region �û��������ʻ���Ĳ�ѯ����

    /// <summary>
    /// �û��������ʻ���Ĳ�ѯ��
    /// </summary>
    public class Q_BANK_USER : Query_BaseForNET
    {
        private string f_strID;
        public Q_BANK_USER(string strID)
        {
            // TODO: 1�ͻ���Ϣ��������
            f_strID = strID;
            fstrSql = "Select * from " + PublicRes.GetTableName("t_fetch_bank", f_strID) + " where " + PublicRes.GetSqlFromQQ(strID, "fuid");
            string fuid = "";
            try
            {

                fuid = PublicRes.ConvertToFuid(f_strID);
            }
            catch (Exception e)
            {

                throw new Exception("���û�������," + e.Message.ToString());
            }
            ICESql = "uid=" + fuid;
            ICESql += "&curtype=1";

            ICEcommand = CommQuery.QUERY_BANKUSER;
        }
        public Q_BANK_USER(string strID, bool isbatch)
        {
            // TODO: 1�ͻ���Ϣ��������
            f_strID = strID;
            fstrSql = "Select * from " + PublicRes.GetTableName("t_fetch_bank", f_strID) + " where " + PublicRes.GetSqlFromQQ(strID, "fuid");



            string fuid = PublicRes.ConvertToFuid(f_strID);

            ICESql = "uid=" + fuid;


            ICEcommand = CommQuery.BATCH_BANKUSER;


        }

        /// <summary>
        /// �ṩ���������Ե��õĺ������Թ̶����෵��ֵ��
        /// </summary>
        /// <returns></returns>
        public T_BANK_USER[] GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));

            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    T_BANK_USER[] result = new T_BANK_USER[dt.Rows.Count];
                    int i = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        result[i] = new T_BANK_USER();

                        result[i].u_Area = QueryInfo.GetString(dr["farea"]); //
                        result[i].u_Bank_Name = QueryInfo.GetString(dr["fbank_name"]);//
                        result[i].u_BankID = QueryInfo.GetString(dr["fbankid"]); //
                        result[i].u_Bank_Type = QueryInfo.GetInt(dr["fbank_type"]);
                        result[i].u_City = QueryInfo.GetString(dr["fcity"]); //
                        result[i].u_Login_IP = QueryInfo.GetString(dr["flogin_ip"]);//
                        result[i].u_Memo = QueryInfo.GetString(dr["fmemo"]);//
                        result[i].u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);//
                        result[i].u_State = QueryInfo.GetInt(dr["fstate"]); //
                        result[i].u_TrueName = QueryInfo.GetString(dr["ftruename"]); //

                        result[i].u_QQID = QueryInfo.GetString(dr["fqqid"]);//
                        i++;
                    }
                    da.CloseConn();
                    da.Dispose();
                    return result;
                }
                else
                {
                    da.CloseConn();
                    da.Dispose();
                    throw new Exception("û�в��ҵ���Ӧ�ļ�¼");
                }
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }
    }

    #endregion


    #region ���׵���Ĳ�ѯ����

    /// <summary>
    /// ���׵���Ĳ�ѯ��
    /// </summary>
    public class Q_PAY_LIST : Query_BaseForNET
    {
        private string f_strID;
        public string ICESQL = "";

        public Q_PAY_LIST(string strID, int iIDType, DateTime dtBegin, DateTime dtEnd, int istr, int imax)
        {
            f_strID = strID;
            if (iIDType == 0)  //����QQ�������ҽ��׵�
            {
                //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZJ"));

                try
                {
                    //da.OpenConn();

                    //furion 20061117 email��¼�޸�
                    //��ѯ�ڲ�UID
                    //string str = "select fuid from " + PublicRes.GetTName("t_relation",strID) + " where fqqid= '" + strID + "'";
                    //string fuid = da.GetOneResult(str);
                    string fuid = PublicRes.ConvertToFuid(strID);

                    //string tPayList  = PublicRes.GetTName("t_pay_list",fuid);             //���׵��ı�
                    string tPayList = PublicRes.GetTName("t_user_order", fuid);             //���׵��ı�

                    //string sWhereStr = " where (fbuy_uid='" + fuid + "' or fsale_uid ='" + fuid + "') and fcreate_time between '" 
                    string sWhereStr = " where (fbuy_uid='" + fuid + "')  and fcurtype=1 and fcreate_time between  '"
                        + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                    //ʹ��or �ᵼ������ʧЧ�� ������union���� rayguo . ���Ƿ��������union����ѯ��ĺ����� ���Ի���or ��ʱ�� rayguo 07.07

                    string sWhereStr1 = " where fbuy_uid='" + fuid + "' and fcurtype=1  and fcreate_time between '"
                        + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                    string sWhereStr2 = " where fsale_uid='" + fuid + "' and fcurtype=1 and fcreate_time between '"
                        + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                    //��ȡ�ܼ�¼��
                    string fstrSql_count1 = "Select count(1) as total from " + tPayList + sWhereStr1;
                    string fstrSql_count2 = "Select count(1) as total from " + tPayList + sWhereStr2;

                    //int iCount1    = Int32.Parse(da.GetOneResult(fstrSql_count1)); //����
                    //int iCount2    = Int32.Parse(da.GetOneResult(fstrSql_count2)); //����

                    int iCount = 10000;

                    //fstrSql       = "Select *," + iCount + " as total from " + tPayList + sWhereStr + " ORDER By Fcreate_time DESC limit "+ (istr-1) +"," + imax;
                    fstrSql = "Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr1
                        //+ "UNION Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr2  
                        + " ORDER By Fcreate_time DESC limit " + (istr - 1) + "," + imax;
                }
                catch (Exception e)
                {
                    //					Common.CommLib.commRes.sendLog4Log("Q_PAY_LIST:Query_BaseForNET","��ѯ���׵�ʧ��" + e.Message);
                    throw;
                }
                finally
                {
                    //da.Dispose();
                }
            }
            else if (iIDType == 9)  //����QQ��������ҽ��׵�
            {
                //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZJ"));

                try
                {
                    //da.OpenConn();

                    //furion 20061117 email��¼�޸�
                    //��ѯ�ڲ�UID
                    //string str = "select fuid from " + PublicRes.GetTName("t_relation",strID) + " where fqqid= '" + strID + "'";
                    //string fuid = da.GetOneResult(str);
                    string fuid = PublicRes.ConvertToFuid(strID);

                    //string tPayList  = PublicRes.GetTName("t_pay_list",fuid);             //���׵��ı�
                    string tPayList = PublicRes.GetTName("t_user_order", fuid);             //���׵��ı�

                    //string sWhereStr = " where (fbuy_uid='" + fuid + "' or fsale_uid ='" + fuid + "') and fcreate_time between '" 
                    string sWhereStr = " where ( fsale_uid ='" + fuid + "') and fcurtype=1 and fcreate_time between '"
                        + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                    //ʹ��or �ᵼ������ʧЧ�� ������union���� rayguo . ���Ƿ��������union����ѯ��ĺ����� ���Ի���or ��ʱ�� rayguo 07.07

                    string sWhereStr1 = " where fbuy_uid='" + fuid + "' and fcurtype=1  and fcreate_time between '"
                        + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                    string sWhereStr2 = " where fsale_uid='" + fuid + "' and fcurtype=1  and fcreate_time between '"
                        + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                    //��ȡ�ܼ�¼��
                    string fstrSql_count1 = "Select count(1) as total from " + tPayList + sWhereStr1;
                    string fstrSql_count2 = "Select count(1) as total from " + tPayList + sWhereStr2;

                    //int iCount1    = Int32.Parse(da.GetOneResult(fstrSql_count1)); //����
                    //int iCount2    = Int32.Parse(da.GetOneResult(fstrSql_count2)); //����

                    int iCount = 10000;

                    //fstrSql       = "Select *," + iCount + " as total from " + tPayList + sWhereStr + " ORDER By Fcreate_time DESC limit "+ (istr-1) +"," + imax;
                    //					fstrSql       = "Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr1  
                    //						+ "UNION Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr2  
                    //						+ " ORDER By Fcreate_time DESC limit "+ (istr-1) +"," + imax;
                    fstrSql = " Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr2
                        + " ORDER By Fcreate_time DESC limit " + (istr - 1) + "," + imax;
                }
                catch (Exception e)
                {
                    //					Common.CommLib.commRes.sendLog4Log("Q_PAY_LIST:Query_BaseForNET","��ѯ���׵�ʧ��" + e.Message);
                    throw;
                }
                finally
                {
                    //da.Dispose();
                }
            }
            // 2012/5/29 ����Ӹ���QQ�Ų�ѯ�н齻��
            else if (iIDType == 10)
            {
                try
                {
                    //da.OpenConn();

                    //furion 20061117 email��¼�޸�
                    //��ѯ�ڲ�UID
                    //string str = "select fuid from " + PublicRes.GetTName("t_relation",strID) + " where fqqid= '" + strID + "'";
                    //string fuid = da.GetOneResult(str);
                    string fuid = PublicRes.ConvertToFuid(strID);

                    //string fuid = "20364000";
                    //string tPayList  = PublicRes.GetTName("t_pay_list",fuid);             //���׵��ı�
                    string tPayList = PublicRes.GetTName("t_user_order", fuid);             //���׵��ı�

                    //string sWhereStr = " where (fbuy_uid='" + fuid + "' or fsale_uid ='" + fuid + "') and fcreate_time between '" 
                    string sWhereStr = " where ( fsale_uid ='" + fuid + "') and fcurtype=1 and fcreate_time between '"
                        + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                    //ʹ��or �ᵼ������ʧЧ�� ������union���� rayguo . ���Ƿ��������union����ѯ��ĺ����� ���Ի���or ��ʱ�� rayguo 07.07

                    // 2012/5/29 ��ʵ������iIDType=9������£��Ӷ�һ��ѡ���н齻�׵����
                    string sWhereStr1 = " where fbuy_uid='" + fuid + "' and fcurtype=1  and fcreate_time between '"
                        + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and Fmedi_sign=1 and Ftrade_type=4 ";

                    string sWhereStr2 = " where fsale_uid='" + fuid + "' and fcurtype=1  and fcreate_time between '"
                        + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and Fmedi_sign=1 and Ftrade_type=4";

                    //��ȡ�ܼ�¼��
                    string fstrSql_count1 = "Select count(1) as total from " + tPayList + sWhereStr1;
                    string fstrSql_count2 = "Select count(1) as total from " + tPayList + sWhereStr2;

                    //int iCount1    = Int32.Parse(da.GetOneResult(fstrSql_count1)); //����
                    //int iCount2    = Int32.Parse(da.GetOneResult(fstrSql_count2)); //����

                    int iCount = 10000;

                    //fstrSql       = "Select *," + iCount + " as total from " + tPayList + sWhereStr + " ORDER By Fcreate_time DESC limit "+ (istr-1) +"," + imax;
                    //					fstrSql       = "Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr1  
                    //						+ "UNION Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr2  
                    //						+ " ORDER By Fcreate_time DESC limit "+ (istr-1) +"," + imax;
                    fstrSql = " Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr2
                        + " ORDER By Fcreate_time DESC limit " + (istr - 1) + "," + imax;
                }
                catch (Exception e)
                {
                    //					Common.CommLib.commRes.sendLog4Log("Q_PAY_LIST:Query_BaseForNET","��ѯ���׵�ʧ��" + e.Message);
                    throw;
                }
                finally
                {
                    //da.Dispose();
                }
            }
            else if (iIDType == 13)  //yinhuang С��ˢ�����ײ�ѯ
            {
                try
                {
                    //��ѯ�ڲ�UID
                    string fuid = PublicRes.ConvertToFuid(strID);

                    string tPayList = PublicRes.GetTName("t_user_order", fuid);             //���׵��ı�

                    //string sWhereStr = " where (fbuy_uid='" + fuid + "')  and fcurtype=1 and fcreate_time between  '"
                    //    + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                    string sWhereStr1 = " where fbuy_uid='" + fuid + "' and fcurtype=1 and fchannel_id=113";

                   // string sWhereStr2 = " where fsale_uid='" + fuid + "' and fcurtype=1 and fcreate_time between '"
                    //    + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                    //��ȡ�ܼ�¼��
                    //string fstrSql_count1 = "Select count(1) as total from " + tPayList + sWhereStr1;
                    //string fstrSql_count2 = "Select count(1) as total from " + tPayList + sWhereStr2;

                    int iCount = 10000;

                    fstrSql = "Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr1
                        + " ORDER By Fcreate_time DESC limit " + istr + "," + imax;
                }
                catch (Exception e)
                {
                    throw;
                }
                finally
                {
                    //da.Dispose();
                }
            }
            else if (iIDType == 1)
            {
                ICESQL = "listid=" + f_strID;
                //fstrSql = "Select * from " + PublicRes.GetTName("t_tran_list",strID) + " where flistid='" + f_strID + "'";
                fstrSql = "Select *,Ftrade_state as Fstate from " + PublicRes.GetTName("t_order", strID) + " where flistid='" + f_strID + "'";
            }
            else if (iIDType == 2) //furion 20090805 2�����Ӧ��û��.
            {
                ICESQL = "listid=" + f_strID;
                //fstrSql = "Select * from " + PublicRes.GetTName("t_tran_list",strID) + " where Fbank_backid ='" + f_strID + "'";
                fstrSql = "Select *,Ftrade_state as Fstate from " + PublicRes.GetTName("t_order", strID) + " where Fbank_backid ='" + f_strID + "'";
            }
            else if (iIDType == 4)
            {
                ICESQL = "listid=" + f_strID;
                // TODO: 1furion ���ݿ��Ż� 20080111 ����ʱҪ��ѯ
                //fstrSql = "Select * from " + PublicRes.GetTName("t_tran_list",strID) + " where flistid='" + f_strID + "'";
                fstrSql = "Select * from " + PublicRes.GetTName("t_order", strID) + " where flistid='" + f_strID + "'";
            }
            else
            {
                throw new Exception("���׵���ѯ�����������");
            }

        }

        public Q_PAY_LIST(string strID)
        {
            f_strID = strID;
            //string tPayList  = PublicRes.GetTName("t_tran_list",f_strID); 
            string tPayList = PublicRes.GetTName("t_order", f_strID);
            fstrSql = "Select *,Ftrade_state as Fstate from " + tPayList + " where flistid='" + f_strID + "'";

            ICESQL = "listid=" + f_strID;
        }

        /// <summary>
        /// �ṩ���������Ե��õĺ������Թ̶����෵��ֵ��
        /// </summary>
        /// <returns></returns>
        public T_PAY_LIST GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YWB"));
            T_PAY_LIST result = new T_PAY_LIST();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    result.u_Bank_ListID = QueryInfo.GetString(dr["fbank_listid"]);
                    result.u_Bargain_Time = QueryInfo.GetDateTime(dr["fbargain_time"]);
                    result.u_Buy_Bank_Type = QueryInfo.GetInt(dr["fbuy_bank_type"]);
                    result.u_Buy_BankID = QueryInfo.GetString(dr["fbuy_bankid"]);
                    result.u_Buy_Name = QueryInfo.GetString(dr["fbuy_name"]);
                    result.u_BuyID = QueryInfo.GetString(dr["fbuyid"]);
                    result.u_Carriage = QueryInfo.GetInt(dr["fcarriage"]);
                    result.u_Cash = QueryInfo.GetInt(dr["fcash"]);
                    result.u_Coding = QueryInfo.GetString(dr["fcoding"]);
                    result.u_Create_Time = QueryInfo.GetDateTime(dr["fcreate_time"]);
                    result.u_Create_Time_C2C = QueryInfo.GetDateTime(dr["fcreate_time_c2c"]);
                    result.u_CurType = QueryInfo.GetInt(dr["fcurtype"]);
                    result.u_Explain = QueryInfo.GetString(dr["fexplain"]);
                    result.u_Fact = QueryInfo.GetInt(dr["ffact"]);
                    result.u_IP = QueryInfo.GetString(dr["fip"]);
                    result.u_LState = QueryInfo.GetInt(dr["flstate"]);
                    result.u_Memo = QueryInfo.GetString(dr["fmemo"]);
                    result.u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                    result.u_Pay_Type = QueryInfo.GetInt(dr["fpay_type"]);
                    result.u_PayNum = QueryInfo.GetInt(dr["fpaynum"]);
                    result.u_Price = QueryInfo.GetInt(dr["fprice"]);
                    result.u_Procedure = QueryInfo.GetInt(dr["fprocedure"]);
                    result.u_Receive_Time = QueryInfo.GetDateTime(dr["freceive_time"]);
                    result.u_Receive_Time_C2C = QueryInfo.GetDateTime(dr["freceive_time_c2c"]);
                    result.u_Sale_Name = QueryInfo.GetString(dr["fsale_name"]);
                    result.u_SaleID = QueryInfo.GetString(dr["fsaleid"]);
                    result.u_Service = QueryInfo.GetInt(dr["fservice"]);
                    result.u_SPID = QueryInfo.GetString(dr["fspid"]);
                    result.u_State = QueryInfo.GetInt(dr["fstate"]);

                    result.u_ListID = QueryInfo.GetString(dr["flistid"]);

                    result.u_Pay_Time = QueryInfo.GetDateTime(dr["fpay_time"]);
                }
                else
                {
                    throw new Exception("û�в��ҵ���Ӧ�ļ�¼");
                }
                da.CloseConn();
                da.Dispose();
                return result;
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }
    }

    #endregion


    #region �û��ʻ���ˮ��Ĳ�ѯ����
    /// <summary>
    /// �û��ʻ���ˮ��Ĳ�ѯ��  (���н�)
    /// </summary>
    public class Q_BANKROLL_LIST : Query_BaseForNET
    {
        private string f_strID;
        private DateTime f_dtBegin;
        private DateTime f_dtEnd;

        //�����׵���ѯ�ʽ���ˮר�ã�������Ҫ��ѯ�Ŀ�����ơ�
        public ArrayList alTables;

        public Q_BANKROLL_LIST(string strID, DateTime dtBegin, DateTime dtEnd, int istr, int imax)
        {
            f_strID = strID;
            f_dtBegin = dtBegin;
            f_dtEnd = dtEnd;

            string tableStr = PublicRes.GetTableName("t_bankroll_list", f_strID);
            string whereStr = "  where " + PublicRes.GetSqlFromQQ(strID, "fuid") + " and fmodify_time between '" + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

            //fstrSql = "Select A.*,B.total from  " + tableStr + " A, (Select count(1) as total from " + tableStr + whereStr + ") B "
            fstrSql = "Select A.*,10000 as total from  " + tableStr + " A "
                + whereStr
                + " Order by fmodify_time DESC ";
        }

        /// <summary>
        /// ������Ҫ��ı�Ϳ��ԣ���ΪSQLһ����
        /// </summary>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <param name="listID"></param>
        public Q_BANKROLL_LIST(DateTime dtBegin, DateTime dtEnd, String listID)
        {
            f_dtBegin = dtBegin;
            f_dtEnd = dtEnd;

            //******************************************************************************************************************
            //���׵���ص��ʽ���ˮ�����3���ط�;
            //1 ���fuid�ֿ�ֱ���ʽ���ˮ 2����fuid�ֿ�ֱ���ʽ���ˮ 3 �н��˻��ֿ�ֱ���ʽ���ˮ
            //������Ҫ��3����ѯ���ֱ��ѯ�����ϲ������������������ʾĳlistID���׵�����ص������ʽ���ˮ

            //���ݽ��׵��Ų�ѯ��ص��û��ʽ���ˮ����Ϊ�ֿ�ֱ�����ⲻ�ܹ�ֱ�Ӳ�ѯ��
            //���ȸ��ݽ��׵���ѯ����ص����QQ�ţ����QQ�š��ڸ���QQ����ֿ�ֱ��ҵ���Ӧ���ڲ�Fuid.
            //����Fuid��ѯ��ص��û��ʽ���ˮ���������ڲ�ѯ��ص��н��˻���ˮ������ˮ
            //******************************************************************************************************************

            // TODO1: furion ���ݿ��Ż� 20080111
            //string listDb = PublicRes.GetTName("t_tran_list",listID);
            //string listDb = PublicRes.GetTName("t_order",listID);


            MySqlAccess da = null;
            /*
            //da = new MySqlAccess(PublicRes.GetConnString("YW_30"));  //ѡ���ȡ�����ݿ�
            da = new MySqlAccess(PublicRes.GetConnString("ZJ"));  //ѡ���ȡ�����ݿ�
            da.OpenConn();   //������
            */
            TENCENT.OSS.C2C.Finance.Common.CommLib.tradeList tl = new TENCENT.OSS.C2C.Finance.Common.CommLib.tradeList();
            string[] ar = PublicRes.returnlistInfo(listID, out tl, da, "noUpdate");
            /*
            da.Dispose();
            */
            //
            //			ar[25]= "Fstate";      //���׵���״̬  Fstate��Fpay_time��Freceive_time��Fmodify_time
            //			ar[26]= "Fpay_time";   //��Ҹ���ʱ�䣨���أ�
            //			ar[27]= "Freceive_time"; //��������ʱ��
            //			ar[28]= "Fmodify_time";  //����޸�ʱ��

            //ȷ�����н飩�˻���ˮ��ı���
            string timeStr = null;
            string timeStr2 = null;
            string lstState = ar[25];  //���׵���״̬

            //���ǳ��ֿ��µ����⣬��Ҫ��richard��һ���̶�

            alTables = new ArrayList();

            alTables.Add("UID=" + ar[3].Trim());
            alTables.Add("UID=" + ar[12].Trim());

            if (lstState == "2")       //2 ��ʾ��Ҹ���
            {
                timeStr = ar[26].ToString().Trim();  //��Ҹ���ʱ�䣨���أ�
                timeStr2 = null;

                alTables.Add("TME=" + timeStr);
            }
            else if (lstState == "4")  //4 ���׽���
            {
                if (ar[27].Substring(0, 10) == ar[26].Substring(0, 10))
                {
                    timeStr = ar[26].ToString().Trim();  //�����Ҹ���ʹ���������ͬһ���£���ȡ��Ҹ������
                    timeStr2 = null;

                    alTables.Add("TME=" + timeStr);
                }
                else
                {
                    timeStr = ar[26].ToString().Trim();  //��Ҹ���ʹ������Ҳ�����ͬһ���£�����Ҫ����������ȥ�����н��ʻ�����ˮ
                    timeStr2 = ar[27].ToString().Trim();  //��������

                    alTables.Add("TME=" + timeStr);
                    alTables.Add("TME=" + timeStr2);
                }
            }
            else if (lstState == "7")  //7 ת���˿�
            {
                Hashtable ht = new Hashtable();

                if (!ht.ContainsKey(ar[26].Substring(0, 10)))
                {
                    ht.Add(ar[26].Substring(0, 10), "");
                }

                if (!ht.ContainsKey(ar[28].Substring(0, 10)))
                {
                    ht.Add(ar[28].Substring(0, 10), "");
                }

                //b2c�Ϳ��ٽ��ײ�ѯ�˿����뵥 ������Բ��ܣ����˲鲻�����ݡ�
                //if(tl.Ftrade_type == "2" || tl.Ftrade_type == "3")
                {
                    /*
                    string strSql = " select Fcreate_time from c2c_db_inc.t_spm_refund where Ftransaction_id='" + listID + "' and Fstatus != 5";
                    DataTable dt = PublicRes.returnDataTable(strSql,"INC");
                    */

                    string strSql = "transaction_id=" + listID + "&statusno=5";
                    string errMsg = "";
                    DataTable dt = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_MCH_REFUND, out errMsg);

                    //20141114 FINANCE_OD_QUERY_MCH_REFUND�ĵ�relay
                    /////////////////////////////////////
                    //DataTable dt = new DataTable();
                    //string qzj_ip = ConfigurationManager.AppSettings["ComQueryToRelay_IP"];
                    //string qzj_port = ConfigurationManager.AppSettings["ComQueryToRelay_PORT"];

                    //string req = "request_type=100568&ver=1&head_u=&sp_id=&" + strSql;


                    //string Msg = ""; //����

                    //string answer = commRes.GetFromRelay(req, qzj_ip, qzj_port, out Msg);

                    //if (answer == "")
                    //{
                    //    dt= null;
                    //}
                    //if (Msg != "")
                    //{
                    //    throw new Exception("��relay�쳣��" + Msg);
                    //}

                    ////����relay str
                    //DataSet ds = CommQuery.ParseRelayPageRowNum0(answer, out Msg);
                    //if (Msg != "")
                    //{
                    //    throw new Exception("����relay�쳣��" + Msg);
                    //}

                    //if (ds != null && ds.Tables.Count > 0 )
                    //{
                    //    dt = ds.Tables[0];
                    //}
                    ///////////////////////////////
                 

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            //string tmp = DateTime.Parse(dr[0].ToString()).ToString("yyyy-MM-dd");
                            string tmp = DateTime.Parse(dr["Fcreate_time"].ToString()).ToString("yyyy-MM-dd");
                            if (!ht.ContainsKey(tmp.Substring(0, 10)))
                            {
                                ht.Add(tmp.Substring(0, 10), "");
                            }
                        }
                    }
                }

                foreach (DictionaryEntry de in ht)
                {
                    alTables.Add("TME=" + de.Key.ToString());
                }
                /*
                //furion V30_FURION�Ķ� need ����ط���Ϊ�鵽ʱ��ֻ��Ϊ�˲�ѯ�ʽ���ˮ,������ʽ���ˮ��ѯ�㶨����.
                //���˿�в����˿��ʱ��
                string rTime = "Select Fcreate_time from c2c_db.t_refund_list where flistid = '" + listID + "'"; 
                timeStr2     = PublicRes.ExecuteOne(rTime,"yw_30");
                timeStr2     = DateTime.Parse(timeStr2).ToString("yyyy-MM-dd HH:mm:ss");
                if (ar[26].Substring(0,7) == timeStr2.Substring(0,7))
                {
                    timeStr = ar[26].ToString().Trim();
                    timeStr2= null;
                }
                else
                {
                    timeStr  = ar[26].ToString().Trim();
                    timeStr2 = timeStr2;
                }
                */
            }
            else
            {
                timeStr = ar[9].ToString().Trim();   //Ĭ��ʱ�䣨��������ʱ�䣩
                timeStr2 = null;

                alTables.Add("TME=" + timeStr);
            }



            /*
            //furion ����ǿ������
            string indexstr_mid = "";
            string indexstr_mid2 = "";
            string indexstr_buy = "";
            string indexstr_sale = "";

            string MidSqlStr;
            string strTable_Mid  = PublicRes.GetTableByDate("t_bankroll_list",timeStr);
            indexstr_mid = "XPK1t_bankroll_list_" + timeStr.Replace(":","").Replace(" ","").Replace("-","").Substring(0,6);

            if (timeStr2 == null || timeStr2 == "")
            {				
                //MidSqlStr = "SELECT * FROM " + strTable_Mid  + " where flistid = '" + listID + "'";
                MidSqlStr = "SELECT * FROM " + strTable_Mid  + " force index(" + indexstr_mid + ") where flistid = '" + listID + "'";
            }
            else
            {
				
                string strTable_Mid2 = PublicRes.GetTableByDate("t_bankroll_list",timeStr2);
                indexstr_mid2 = "XPK1t_bankroll_list_" + timeStr2.Replace(":","").Replace(" ","").Replace("-","").Substring(0,6);

                //				MidSqlStr = "SELECT * FROM " + strTable_Mid   + " where flistid = '" + listID + "' UNION " +
                //					"SELECT * FROM " + strTable_Mid2  + " where flistid = '" + listID + "' ";
                MidSqlStr = "SELECT * FROM " + strTable_Mid   + " force index(" + indexstr_mid + ") where flistid = '" + listID + "' UNION " +
                    "SELECT * FROM " + strTable_Mid2  + " force index(" + indexstr_mid2 + ") where flistid = '" + listID + "' ";
            }
			
            //ȷ������ң��˻���ˮ��ı���
            //			string strSelectBuyID  = "SELECT fbuy_uid  FROM " + listDb + " WHERE flistid ='" + listID + "'" ;
            string strTable_Buy = PublicRes.GetTName("t_bankroll_list",ar[3].ToString().Trim());
            string strindexID = ar[3].ToString().Trim();
            indexstr_buy = "XPK1t_bankroll_list_" + strindexID.Substring(strindexID.Length-3,1);

            //ȷ�������ң��˻���ˮ��ı���
            //			string strSelectSaleID = "SELECT fsale_uid  FROM " + listDb + " WHERE flistid = '" + listID + "'" ;
            string strTable_Sale = PublicRes.GetTName("t_bankroll_list",ar[12].ToString().Trim());
            strindexID = ar[12].ToString().Trim();
            indexstr_sale = "XPK1t_bankroll_list_" + strindexID.Substring(strindexID.Length-3,1);

            //���޸� furion V30_FURION���Ĳ�ѯ��Ķ� �˿�γɵ��ʽ���ˮ��3.0��Ӧ�û���Flistid=���׵�ID
            //furion V30_FURION�Ķ� need
            string reStr = ") ";
            /*
            //����Ҫ�˿��ID
            string rID   = "Select frlistid from c2c_db.t_refund_list where flistid = '" + listID + "'"; 
            string strID = PublicRes.ExecuteOne(rID,"yw");

            string reStr;
            if (strID != null && strID != "")
            {
                reStr = " or flistid = '" + strID + "') ";
            }
            else
            {
                reStr = ") ";
            }			
            /
						

            //			string refoundID��= " '" + listID + "'";

            //����ѯ���
            fstrSql = MidSqlStr + " UNION " 
				      
                + "select * from " + strTable_Buy  + " force index(" + indexstr_buy + ") where (flistid = '" + listID + "'" + reStr + " UNION " 
                + "select * from " + strTable_Sale + " force index(" + indexstr_sale + ") where (flistid = '" + listID + "'" + reStr + " order  by Faction_Type DESC";
            */
        }


        /// <summary>
        /// �ṩ��ѯ���е�������ˮ�����ص�ǰ��������acrionType
        /// </summary>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <param name="listID"></param>
        /// <param name="type"></param>
        public Q_BANKROLL_LIST(DateTime dtBegin, DateTime dtEnd, string listID, string type)
        {
            f_dtBegin = dtBegin;
            f_dtEnd = dtEnd;

            //******************************************************************************************************************
            //���׵���ص��ʽ���ˮ�����3���ط�;
            //1 ���fuid�ֿ�ֱ���ʽ���ˮ 2����fuid�ֿ�ֱ���ʽ���ˮ 3 �н��˻��ֿ�ֱ���ʽ���ˮ
            //������Ҫ��3����ѯ���ֱ��ѯ�����ϲ������������������ʾĳlistID���׵�����ص������ʽ���ˮ

            //���ݽ��׵��Ų�ѯ��ص��û��ʽ���ˮ����Ϊ�ֿ�ֱ�����ⲻ�ܹ�ֱ�Ӳ�ѯ��
            //���ȸ��ݽ��׵���ѯ����ص����QQ�ţ����QQ�š��ڸ���QQ����ֿ�ֱ��ҵ���Ӧ���ڲ�Fuid.
            //����Fuid��ѯ��ص��û��ʽ���ˮ���������ڲ�ѯ��ص��н��˻���ˮ������ˮ
            //******************************************************************************************************************

            // TODO1: furion ���ݿ��Ż� 20080111
            //string listDb = PublicRes.GetTName("t_tran_list",listID);
            //string listDb = PublicRes.GetTName("t_order",listID);

            MySqlAccess da = null;
            /*
            //da = new MySqlAccess(PublicRes.GetConnString("YW_30"));  //ѡ���ȡ�����ݿ�
            da = new MySqlAccess(PublicRes.GetConnString("ZJ"));  //ѡ���ȡ�����ݿ�
            da.OpenConn();   //������
            */
            TENCENT.OSS.C2C.Finance.Common.CommLib.tradeList tl = new TENCENT.OSS.C2C.Finance.Common.CommLib.tradeList();
            string[] ar = PublicRes.returnlistInfo(listID, out tl, da, "noUpdate");
            /*
            da.Dispose();
            */

            //ȷ�����н飩�˻���ˮ��ı���
            string timeStr = null;
            string timeStr2 = null;
            string lstState = ar[25];  //���׵���״̬

            //���ǳ��ֿ��µ����⣬��Ҫ��richard��һ���̶�

            if (lstState == "2")       //2 ��ʾ��Ҹ���
            {
                timeStr = ar[26].ToString().Trim();  //��Ҹ���ʱ�䣨���أ�
                timeStr2 = null;
            }
            else if (lstState == "4")  //4 ���׽���
            {
                if (ar[27].Substring(0, 7) == ar[26].Substring(0, 7))
                {
                    timeStr = ar[26].ToString().Trim();  //�����Ҹ���ʹ���������ͬһ���£���ȡ��Ҹ������
                    timeStr2 = null;
                }
                else
                {
                    timeStr = ar[26].ToString().Trim();  //��Ҹ���ʹ������Ҳ�����ͬһ���£�����Ҫ����������ȥ�����н��ʻ�����ˮ
                    timeStr2 = ar[27].ToString().Trim();  //��������
                }
            }
            else if (lstState == "7")  //7 ת���˿�
            {
                //furion V30_FURION�Ķ� need
                //���˿�в����˿��ʱ��
                string rTime = "Select Fcreate_time from c2c_db.t_refund_list where flistid = '" + listID + "'";
                timeStr2 = PublicRes.ExecuteOne(rTime, "yw_30");
                timeStr2 = DateTime.Parse(timeStr2).ToString("yyyy-MM-dd HH:mm:ss");
                if (ar[26].Substring(0, 7) == timeStr2.Substring(0, 7))
                {
                    timeStr = ar[26].ToString().Trim();
                    timeStr2 = null;
                }
                else
                {
                    timeStr = ar[26].ToString().Trim();
                    timeStr2 = timeStr2;
                }
            }
            else
            {
                timeStr = ar[9].ToString().Trim();   //Ĭ��ʱ�䣨��������ʱ�䣩
                timeStr2 = null;
            }

            string MidSqlStr;
            string strTable_Mid = PublicRes.GetTableByDate("t_bankroll_list", timeStr);
            if (timeStr2 == null || timeStr2 == "")
            {
                MidSqlStr = "SELECT * FROM " + strTable_Mid + " where flistid = '" + listID + "' and (Flist_sign= 0 || Flist_sign is null)";
            }
            else
            {
                string strTable_Mid2 = PublicRes.GetTableByDate("t_bankroll_list", timeStr2);
                MidSqlStr = "SELECT * FROM " + strTable_Mid + " where flistid = '" + listID + "' and (Flist_sign= 0 || Flist_sign is null)  UNION " +
                    "SELECT * FROM " + strTable_Mid2 + " where flistid = '" + listID + "' and (Flist_sign= 0 || Flist_sign is null) ";
            }

            //ȷ������ң��˻���ˮ��ı���
            //			string strSelectBuyID  = "SELECT fbuy_uid  FROM " + listDb + " WHERE flistid ='" + listID + "'" ;
            string strTable_Buy = PublicRes.GetTName("t_bankroll_list", ar[3].ToString().Trim());


            //ȷ�������ң��˻���ˮ��ı���
            //			string strSelectSaleID = "SELECT fsale_uid  FROM " + listDb + " WHERE flistid = '" + listID + "'" ;
            string strTable_Sale = PublicRes.GetTName("t_bankroll_list", ar[12].ToString().Trim());

            //�ѸĶ� furion V30_FURION���Ĳ�ѯ��Ķ� �˿��γɵ��ʽ���ˮ��3.0��,Ӧ�û���Flistid=���׵�ID
            //furion V30_FURION�Ķ� need
            string reStr = ") ";
            //����Ҫ�˿��ID
            /*
            string rID   = "Select frlistid from c2c_db.t_refund_list where flistid = '" + listID + "'"; 
            string strID = PublicRes.ExecuteOne(rID,"yw");

            string reStr;
            if (strID != null && strID != "")
            {
                reStr = " or flistid = '" + strID + "') ";
            }
            else
            {
                reStr = ") ";
            }	
            */

            //			string refoundID��= " '" + listID + "'";

            //����ѯ���
            fstrSql = MidSqlStr + " UNION "

                + "select * from " + strTable_Buy + " where (flistid = '" + listID + "' and (Flist_sign= 0 || Flist_sign is null) " + reStr + " UNION "
                + "select * from " + strTable_Sale + " where (flistid = '" + listID + "' and (Flist_sign= 0 || Flist_sign is null) " + reStr + " order  by Faction_type DESC";
        }


        /// <summary>
        /// �ṩ���������Ե��õĺ������Թ̶����෵��ֵ��
        /// </summary>
        /// <returns></returns>
        public T_BANKROLL_LIST[] GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString());

            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    T_BANKROLL_LIST[] result = new T_BANKROLL_LIST[dt.Rows.Count];
                    int i = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        result[i] = new T_BANKROLL_LIST();

                        result[i].u_ApplyID = QueryInfo.GetString(dr["fapplyid"]);
                        result[i].u_Balance = QueryInfo.GetInt(dr["fbalance"]);
                        result[i].u_Bank_Type = QueryInfo.GetInt(dr["fbank_type"]);
                        //result[i].u_Create_Time = QueryInfo.GetDateTime(dr["fcreate_time"]); fmodify_time
                        result[i].u_Create_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                        result[i].u_CurType = QueryInfo.GetInt(dr["fcurtype"]);
                        result[i].u_From_Name = QueryInfo.GetString(dr["ffrom_name"]);
                        result[i].u_FromID = QueryInfo.GetString(dr["ffromid"]);
                        result[i].u_IP = QueryInfo.GetString(dr["fip"]);
                        result[i].u_ListID = QueryInfo.GetString(dr["flistid"]);
                        result[i].u_Memo = QueryInfo.GetString(dr["fmemo"]);
                        result[i].u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                        result[i].u_PayNum = QueryInfo.GetInt(dr["fpaynum"]);
                        result[i].u_Prove = QueryInfo.GetString(dr["fprove"]);
                        result[i].u_SPID = QueryInfo.GetString(dr["fspid"]);
                        result[i].u_Subject = QueryInfo.GetInt(dr["fsubject"]);
                        result[i].u_True_Name = QueryInfo.GetString(dr["ftrue_name"]);
                        result[i].u_Type = QueryInfo.GetInt(dr["ftype"]);
                        result[i].u_BKID = QueryInfo.GetInt(dr["fbkid"]);
                        result[i].u_UID = QueryInfo.GetString(dr["fuid"]);

                        result[i].u_Action_Type = QueryInfo.GetInt(dr["faction_type"]);

                        i++;
                    }
                    da.CloseConn();
                    da.Dispose();
                    return result;
                }
                else
                {
                    da.CloseConn();
                    da.Dispose();
                    throw new Exception("û�в��ҵ���Ӧ�ļ�¼");
                }
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }
    }
    #endregion


    #region �û�������ˮ��Ĳ�ѯ����
    /// <summary>
    /// �û�������ˮ��Ĳ�ѯ��
    /// </summary>
    public class Q_USERPAY_LIST : Query_BaseForNET
    {
        private string f_strID;
        private DateTime f_dtBegin;
        private DateTime f_dtEnd;


        //			public Q_USERPAY_LIST(string strID, int iIDType, DateTime dtBegin, DateTime dtEnd,int istr, int imax)
        //			{
        //				f_strID = strID;
        //				f_dtBegin = dtBegin;
        //				f_dtEnd = dtEnd;
        //
        //				if(iIDType ==0)
        //				{
        //					fstrSql = "Select * from  " + PublicRes.GetTableName("t_userpay_list",f_strID) + "  where " + PublicRes.GetSqlFromQQ(strID,"fuid");
        //					if(dtBegin.ToString("yyyy-MM-dd") != "1900-01-01" && dtEnd.ToString("yyyy-MM-dd") != "4000-01-01")
        //					{
        //						fstrSql += " and fcreate_time between '" + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" 	
        //							+ dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        //					}
        //					fstrSql +=  " Order by fude_id DESC limit "+ (istr-1) +"," + imax;
        //				}
        //			}

        /*
            public Q_USERPAY_LIST(string strID, DateTime dtBegin, DateTime dtEnd,String listID)
            {
                f_strID = strID;
                f_dtBegin = dtBegin;
                f_dtEnd = dtEnd;

                //********************************************************************************************************************************
                //���׼�¼��¼��ˮ�����2���ط���1�Ǹ������fuid�ֿ�ֱ��t_userpay_list�У���һ��������fuid�ֿ�ֱ��t_userpay_list��.
                //����Ҫ�ֱ���ݽ��׵��Ų�ѯ����Һ����ҵ��ڲ�fuid,����fuid��λ����,Ȼ��ֱ��ѯ������ϳ�һ���������
                //********************************************************************************************************************************

                //����û�fuid
                //string strSelectBuyID  = "SELECT  fbuy_uid   FROM " + PublicRes.GetTName("t_tran_list",listID) + " WHERE flistid ='" + listID + "'" ;
                string strSelectBuyID  = "SELECT  fbuy_uid   FROM " + PublicRes.GetTName("t_order",listID) + " WHERE flistid ='" + listID + "'" ;
                string strTable_Buy = PublicRes.GetTName("t_userpay_list",PublicRes.ExecuteOne(strSelectBuyID,"ZJB"));

                //����û�fuid
                //string strSelectSaleID  = "SELECT fsale_uid  FROM " + PublicRes.GetTName("t_tran_list",listID) + " WHERE flistid ='" + listID + "'" ;
                string strSelectSaleID  = "SELECT fsale_uid  FROM " + PublicRes.GetTName("t_order",listID) + " WHERE flistid ='" + listID + "'" ;
                string strTable_Sale = PublicRes.GetTName("t_userpay_list",PublicRes.ExecuteOne(strSelectSaleID,"ZJB"));

                fstrSql = "SELECT * FROM " + strTable_Buy   + " where  flistid = '" + listID + "' union " 
                    +"SELECT * FROM " + strTable_Sale  + " where  flistid = '" + listID + "'";

            }
            */


        /// <summary>
        /// �ṩ���������Ե��õĺ������Թ̶����෵��ֵ��
        /// </summary>
        /// <returns></returns>
        public T_USERPAY_LIST[] GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString());

            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    T_USERPAY_LIST[] result = new T_USERPAY_LIST[dt.Rows.Count];
                    int i = 0;
                    foreach (DataRow dr in dt.Rows) //��
                    {
                        result[i] = new T_USERPAY_LIST();

                        result[i].u_ApplyID = QueryInfo.GetString(dr["fapplyid"]);
                        result[i].u_Balance = QueryInfo.GetInt(dr["fbalance"]);
                        result[i].u_Bank_Type = QueryInfo.GetInt(dr["fbank_type"]);
                        result[i].u_Create_Time = QueryInfo.GetDateTime(dr["fcreate_time"]);
                        result[i].u_CurType = QueryInfo.GetInt(dr["fcurtype"]);
                        result[i].u_From_Balance = QueryInfo.GetInt(dr["ffrom_balance"]);
                        result[i].u_From_Name = QueryInfo.GetString(dr["ffrom_name"]);
                        result[i].u_FromID = QueryInfo.GetString(dr["ffromid"]);
                        result[i].u_IP = QueryInfo.GetString(dr["fip"]);
                        result[i].u_ListID = QueryInfo.GetString(dr["flistid"]);
                        result[i].u_Memo = QueryInfo.GetString(dr["fmemo"]);
                        result[i].u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                        result[i].u_PayNum = QueryInfo.GetInt(dr["fpaynum"]);
                        result[i].u_Prove = QueryInfo.GetString(dr["fprove"]);
                        result[i].u_SPID = QueryInfo.GetString(dr["fspid"]);
                        result[i].u_Subject = QueryInfo.GetInt(dr["fsubject"]);
                        result[i].u_True_Name = QueryInfo.GetString(dr["ftrue_name"]);
                        result[i].u_Type = QueryInfo.GetInt(dr["ftype"]);
                        result[i].u_Ude_ID = QueryInfo.GetInt(dr["ftype"]);
                        result[i].u_QQID = QueryInfo.GetString(dr["fqqid"]);

                        i++;
                    }
                    da.CloseConn();
                    da.Dispose();
                    return result;
                }
                else
                {
                    da.CloseConn();
                    da.Dispose();
                    throw new Exception("û�в��ҵ���Ӧ�ļ�¼");
                }
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }

    }
    #endregion


    #region ��Ѷ�����ʻ���Ĳ�ѯ����
    /// <summary>
    /// ��Ѷ�����ʻ���Ĳ�ѯ��
    /// </summary>
    public class Q_TC_BANK : Query_BaseForNET
    {
        private string f_strID;
        public Q_TC_BANK(string strID)
        {
            f_strID = strID;
            fstrSql = "Select * from c2c_db.t_tc_bank where fbankid='" + f_strID + "'";
        }

        /// <summary>
        /// �ṩ���������Ե��õĺ������Թ̶����෵��ֵ��
        /// </summary>
        /// <returns></returns>
        public T_TC_BANK GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));
            T_TC_BANK result = new T_TC_BANK();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    result.u_Area = QueryInfo.GetString(dr["farea"]);
                    result.u_Balance = QueryInfo.GetInt(dr["fbalance"]);
                    result.u_Bank_Name = QueryInfo.GetString(dr["fbank_name"]);
                    result.u_Bank_Type = QueryInfo.GetInt(dr["fbank_type"]);
                    result.u_City = QueryInfo.GetString(dr["fcity"]);
                    result.u_Create_Time = QueryInfo.GetDateTime(dr["fcreate_time"]);
                    result.u_CurType = QueryInfo.GetString(dr["fcurtype"]);
                    result.u_IP = QueryInfo.GetString(dr["fip"]);
                    result.u_Memo = QueryInfo.GetString(dr["fmemo"]);
                    result.u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                    result.u_TrueName = QueryInfo.GetString(dr["ftruename"]);
                    result.u_Yd_Balance = QueryInfo.GetInt(dr["fyd_balance"]);
                }
                else
                {
                    throw new Exception("û�в��ҵ���Ӧ�ļ�¼");
                }
                da.CloseConn();
                da.Dispose();
                return result;
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }
    }

    #endregion


    #region ��Ѷ�տ��¼��Ĳ�ѯ����
    /// <summary>
    /// ��Ѷ�տ��¼��Ĳ�ѯ��
    /// </summary>
    public class Q_TCBANKROLL_LIST : Query_BaseForNET
    {
        public string ICESQL;

        private string f_strID;
        //		public Q_TCBANKROLL_LIST(string strID, int iIDType, DateTime dtBegin, DateTime dtEnd)
        //		{
        //			f_strID = strID;
        //			if(iIDType == 0)  //����QQID��ѯ
        //			{
        //				string whereStr = " where " + PublicRes.GetSqlFromQQ(strID,"fauid") + " and fpay_front_time between '" 
        //					+ dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "'";
        //				fstrSql = "Select A.*,B.total from c2c_db.t_tcbankroll_list A,(Select count(1) as total from c2c_db.t_tcbankroll_list " + whereStr + ") B "
        //					+ whereStr
        ////					+ " Order by fpay_front_time DESC";
        //					+ " Order by Ftde_id DESC";
        //			}
        //			else
        //			{
        //				fstrSql = "Select * from c2c_db.t_tcbankroll_list where flistid='" + f_strID + "'";
        //			}
        //		}
        public Q_TCBANKROLL_LIST(string strID, int iIDType, DateTime u_BeginTime, DateTime u_EndTime, bool isHistory)
        {
            //			f_strID = strID;
            //			if(iIDType == 0)  //����QQID��ѯ
            //			{
            //				string whereStr = " where " + PublicRes.GetSqlFromQQ(strID,"fauid") + " and fpay_front_time between '" 
            //					+ dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            //				fstrSql = "Select A.*,B.total from c2c_db.t_tcbankroll_list A,(Select count(1) as total from c2c_db.t_tcbankroll_list " + whereStr + ") B "
            //					+ whereStr
            //					//					+ " Order by fpay_front_time DESC";
            //					+ " Order by Ftde_id DESC";
            //			}
            //			else
            //			{
            //				fstrSql = "Select * from c2c_db.t_tcbankroll_list where flistid='" + f_strID + "'";
            //			}


            string strGroup = "";
            string strWhere = "";

            if (strID == null || strID.Trim() == "")
            {
            }
            else if (strID != null && strID.Trim() != "" && iIDType == 0)  //����QQ�Ų�ѯ��ע��ʹ���ڲ�uid
            {
                //furion 20051101 �Ժ��ѯȫ���ڲ�ID��ʼ.
                string uid = PublicRes.ConvertToFuid(strID);
                strWhere += " where fauid='" + uid + "' ";
                ICESQL = "auid=" + uid;
            }
            else if (strID != null && strID.Trim() != "")  //�����еĶ�����
            {
                strWhere += " where flistid='" + strID + "' ";
                ICESQL = "listid=" + strID;
            }


            //��Ϊ��ֵ��¼�ڸ�����Ϣ��һҳ��ѯ��ʱ��begintime�϶�����1940,������и�д
            //			u_BeginTime = DateTime.Now.AddMonths(-4);
            //			u_EndTime = DateTime.Now.AddMonths(-1);

            string test = u_BeginTime.ToString("yyyy-MM-dd");

            if (test != "1940-01-01")
            {
                if (strWhere != "")
                {
                    ICESQL += "&fronttime_start=" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ICESQL += "&fronttime_end=" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss");

                    strWhere += " and Fpay_front_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }
                else
                {
                    ICESQL = "fronttime_start=" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ICESQL += "&fronttime_end=" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss");

                    strWhere = " where Fpay_front_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }

            }

            if (test != "1940-01-01" && isHistory)
            {
                //				strWhere += " and Fpay_front_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss") 
                //					+ "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                //��Ч�ʿ���,ֻȡ��ʼʱ��ͽ���ʱ�������µ���ʷ��¼,������ȡ.
                //DateTime tmpDate = u_BeginTime.AddMonths(-1);
                DateTime tmpDate = u_BeginTime;
                //while(tmpDate <= u_EndTime.AddMonths(1))
                while (tmpDate <= u_EndTime)
                {
                    //string TableName = "c2c_db_receive.t_tcbankroll_list_" + tmpDate.ToString("yyyyMM");
                    string TableName = "c2c_db_history.t_tcbankroll_list_" + tmpDate.ToString("yyyyMM");

                    strGroup = strGroup + " select *  from " + TableName + strWhere + " union all";

                    tmpDate = tmpDate.AddMonths(1);

                    string strTmp = tmpDate.ToString("yyyy-MM-");
                    tmpDate = DateTime.Parse(strTmp + "01 00:00:01");
                }


            }

            string TableName1 = "c2c_db.t_tcbankroll_list";
            strGroup = strGroup + " select *  from " + TableName1 + strWhere + " ";

            string strorder = " Order by Ftde_id DESC ";


            fstrSql = strGroup + strorder;

            fstrSql_count = "select 100000";//" select count(1) from ( " + strGroup + ") tmpTalbe";

            //ICESQL = "strwhere=" + strWhere;
            //			if(test != "1940-01-01" )			//����ʱ����� andrew 20110322
            //			{
            //				ICESQL += "&start_time=" + test;
            //			}
        }

        /// <summary>
        /// �ṩ���������Ե��õĺ������Թ̶����෵��ֵ��
        /// </summary>
        /// <returns></returns>
        public T_TCBANKROLL_LIST GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZJB"));
            T_TCBANKROLL_LIST result = new T_TCBANKROLL_LIST();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    result.u_Aid = QueryInfo.GetString(dr["faid"]);
                    result.u_Bank_Acc = QueryInfo.GetString(dr["fbank_acc"]);
                    result.u_Bank_List = QueryInfo.GetString(dr["fbank_list"]);
                    result.u_Bank_Type = QueryInfo.GetInt(dr["fbank_type"]);
                    result.u_CurType = QueryInfo.GetInt(dr["fcurtype"]);
                    result.u_IP = QueryInfo.GetString(dr["fip"]);
                    result.u_Memo = QueryInfo.GetString(dr["fmemo"]);
                    result.u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                    result.u_Num = QueryInfo.GetInt(dr["fnum"]);
                    result.u_Prove = QueryInfo.GetString(dr["fprove"]);
                    result.u_Sign = QueryInfo.GetString(dr["fsign"]);
                    result.u_SPID = QueryInfo.GetString(dr["fspid"]);
                    result.u_Subject = QueryInfo.GetInt(dr["fsubject"]);
                    result.u_Tde_ID = QueryInfo.GetInt(dr["ftde_id"]);
                    result.u_Type = QueryInfo.GetInt(dr["ftype"]);
                    result.u_UID = QueryInfo.GetString(dr["fuid"]);

                    result.u_State = QueryInfo.GetInt(dr["fstate"]);
                    result.u_Pay_Front_Time = QueryInfo.GetDateTime(dr["fpay_front_time"]);
                    result.u_Pay_Time = QueryInfo.GetDateTime(dr["fpay_time"]);

                    result.u_ListID = QueryInfo.GetString(dr["flistid"]);
                }
                else
                {
                    throw new Exception("û�в��ҵ���Ӧ�ļ�¼");
                }
                da.CloseConn();
                da.Dispose();
                return result;
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }
    }

    #endregion


    #region ��Ѷ�����¼��Ĳ�ѯ����

    /// <summary>
    /// ��Ѷ�����¼��Ĳ�ѯ��
    /// </summary>
    public class Q_TCBANKPAY_LIST : Query_BaseForNET
    {
        private string f_strID;
        public Q_TCBANKPAY_LIST(string strID, int iIDType, DateTime dtBegin, DateTime dtEnd)
        {
            f_strID = strID;
            if (iIDType == 0)
            {
                string currtable = "";
                string othertable = "";
                PickQueryClass.GetPayListTableFromTime(dtBegin, out currtable, out othertable);

                //**furion���ֵ�����20120216
                string whereStr = " where " + PublicRes.GetSqlFromQQ(strID, "fuid") + " and Fcurtype=1 and fpay_front_time_acc between '" + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' and Fbankid!=4 ";//ȥ�����ֱ��������˵�

                fstrSql = "";

                DateTime tmpDate = dtBegin;
                while (tmpDate <= dtEnd)
                {
                    string TableName = "c2c_db.t_tcpay_list_" + tmpDate.ToString("yyyyMM");

                    fstrSql = fstrSql + " select " + PickQueryClass.GetTcPayListNewFields() + ",10000 as Total from " + TableName + whereStr + " union all";

                    tmpDate = tmpDate.AddMonths(1);

                    string strTmp = tmpDate.ToString("yyyy-MM-");
                    tmpDate = DateTime.Parse(strTmp + "01 00:00:01");
                }

                string TableName1 = "c2c_db.t_tcpay_list";
                fstrSql = fstrSql + " select " + PickQueryClass.GetTcPayListOldFields() + ",10000 as Total from " + TableName1 + whereStr + " ";

                fstrSql += " Order by fpay_front_time_acc DESC";

            }
            else
            {
                string currtable = "";
                string othertable = "";
                PickQueryClass.GetPayListTableFromID(f_strID, out currtable, out othertable);

                //**furion���ֵ�����20120216
                //furion V30_FURION�Ķ� need
                //				fstrSql = "Select * from c2c_db.t_tcpay_list where flistid=(select frlistid from c2c_db.t_refund_list where flistid ='" + f_strID + "') Order by fpay_front_time DESC";
                fstrSql = "Select " + PickQueryClass.GetTcPayListOldFields() + " from c2c_db.t_tcpay_list where flistid = '" + f_strID + "' or flistid = (select frlistid from c2c_db.t_refund_list where flistid ='" + f_strID + "'and flstate <> 3) ";
                fstrSql += " union all Select " + PickQueryClass.GetTcPayListNewFields() + " from " + currtable + " where flistid = '" + f_strID + "' or flistid = (select frlistid from c2c_db.t_refund_list where flistid ='" + f_strID + "'and flstate <> 3) ";
                fstrSql += " union all Select " + PickQueryClass.GetTcPayListNewFields() + " from " + othertable + " where flistid = '" + f_strID + "' or flistid = (select frlistid from c2c_db.t_refund_list where flistid ='" + f_strID + "'and flstate <> 3) ";
                fstrSql += " Order by fpay_front_time DESC";
            }
        }

        /// <summary>
        /// �ṩ���������Ե��õĺ������Թ̶����෵��ֵ��
        /// </summary>
        /// <returns></returns>
        public T_TCBANKPAY_LIST[] GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZJB"));
            //T_TCBANKPAY_LIST result = new T_TCBANKPAY_LIST();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);

                if (dt.Rows.Count > 0)
                {
                    T_TCBANKPAY_LIST[] result = new T_TCBANKPAY_LIST[dt.Rows.Count];  //��������ʱ����������
                    int i = 0;
                    foreach (DataRow dr in dt.Rows) //��
                    {
                        result[i] = new T_TCBANKPAY_LIST();               //�����һ�д��һ���࣬��ά��������Ŷ����

                        result[i].u_Tde_ID = QueryInfo.GetString(dr["Ftde_id"]);
                        result[i].u_ListID = QueryInfo.GetString(dr["Flistid"]);
                        result[i].u_Bank_List = QueryInfo.GetString(dr["Fbank_list"]);
                        result[i].u_Bankid = QueryInfo.GetString(dr["Fbankid"]);
                        result[i].u_State = QueryInfo.GetInt(dr["Fstate"]);
                        result[i].u_Type = QueryInfo.GetInt(dr["Ftype"]);
                        result[i].u_Subject = QueryInfo.GetInt(dr["Fsubject"]);
                        result[i].u_Num = QueryInfo.GetInt(dr["Fnum"]);
                        result[i].u_Sign = QueryInfo.GetInt(dr["Fsign"]);
                        result[i].u_Bank_Acc = QueryInfo.GetString(dr["Fbank_acc"]);
                        result[i].u_Bank_Type = QueryInfo.GetInt(dr["Fbank_type"]);
                        result[i].u_Curtype = QueryInfo.GetInt(dr["Fcurtype"]);
                        result[i].u_Aid = QueryInfo.GetString(dr["Faid"]);
                        result[i].u_ABankid = QueryInfo.GetString(dr["Fabankid"]);
                        result[i].u_aName = QueryInfo.GetString(dr["Faname"]);
                        result[i].u_Prove = QueryInfo.GetString(dr["Fprove"]);
                        result[i].u_IP = QueryInfo.GetString(dr["Fip"]);
                        result[i].u_Memo = QueryInfo.GetString(dr["Fmemo"]);
                        result[i].u_Modify_Time = QueryInfo.GetDateTime(dr["Fmodify_time"]);
                        result[i].u_Pay_Front_Time = QueryInfo.GetDateTime(dr["Fpay_front_time"]);
                        result[i].u_Pay_Time = QueryInfo.GetDateTime(dr["Fpay_time"]);
                        result[i].u_Uid = QueryInfo.GetString(dr["Fuid"]);

                        i++;
                    }
                    da.CloseConn();
                    da.Dispose();
                    return result;
                }
                else
                {
                    da.CloseConn();
                    da.Dispose();
                    throw new Exception("û�в��ҵ���Ӧ�ļ�¼");
                }
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }

        public static string GetBankName(string banktype)
        {
            switch (banktype)
            {
                #region
                case "1001":
                    return "��������";

                case "1002":
                    return "�й���������";

                case "1003":
                    return "�й���������";

                case "1004":
                    return "�Ϻ��ֶ���չ����";

                case "1005":
                    return "�й�ũҵ����";

                case "1006":
                    return "�й���������";

                case "1007":
                    return "ũ�й��ʿ�";

                case "1008":
                    return "���ڷ�չ����";

                case "1009":
                    return "��ҵ����";

                case "1010":
                    return "����ƽ������";

                case "1011":
                    return "�й�������������";

                case "1020":
                    return "�й���ͨ����";

                case "1021":
                    return "����ʵҵ����";

                case "1022":
                    return "�й��������";

                case "1023":
                    return "ũ�����������";

                case "1024":
                    return "�Ϻ�����";

                case "1025":
                    return "��������";

                case "1026":
                    return "�й�����";

                case "1027":
                    return "�㶫��չ����";

                case "1028":
                    return "�㶫����";

                case "1099":
                    return "��������";

                case "1030":
                    return "����B2B";
                case "1031":
                    return "���д��";
                case "1032":
                    return "��������";
                case "1033":
                    return "����ͨ";
                case "1034":
                    return "���д��";
                case "1037":
                    return "���д��";
                case "1038":
                    return "���л���ҵ��";

                case "1039":
                    return "����ֱ��";

                case "1040":
                    return "����B2B";
                case "1041":
                    return "������ǿ�";
                case "1042":
                    return "����B2B";

                case "2001":
                    return "����һ��ͨ";
                case "2002":
                    return "����һ��ͨ";
                case "3001":
                    return "��ҵ���ÿ�";
                case "3002":
                    return "�������ÿ�";

                #endregion

                case "9999":
                    return "��������";

                case "0000":
                    return "��������";
                default:
                    return "";
            }
        }
    }


    #endregion


    #region �˿��Ĳ�ѯ����
    /// <summary>
    /// �˿��Ĳ�ѯ��
    /// </summary>
    public class Q_REFUND : Query_BaseForNET
    {
        private string f_strID;
        public Q_REFUND(string strID, int iIDType, DateTime dtBegin, DateTime dtEnd)
        {
            f_strID = strID;
            //			if(iIDType == 0)
            //			{
            //				string whereStr = " where (" + PublicRes.GetSqlFromQQ(strID,"fbuy_uid") 
            //					+ " or " + PublicRes.GetSqlFromQQ(strID,"fsale_uid") + ") ";
            //				if(dtBegin.ToString("yyyy-MM-dd") != "1900-01-01" && dtEnd.ToString("yyyy-MM-dd") != "4000-01-01")
            //				{
            //					whereStr += "and fcreate_time between '" + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
            //				}
            //
            //				string count = PublicRes.ExecuteOne("select count(*) from c2c_db.t_refund_list " + whereStr,"ywb");
            //
            //				//fstrSql = "Select A.*,B.total from c2c_db.t_refund_list A, (Select count(1) as total from c2c_db.t_refund_list "  + whereStr + ") B "
            //					//+ whereStr		+ " Order by Fcreate_time DESC";
            //				fstrSql = "select *," + count + " as total from c2c_db.t_refund_list " + whereStr + " Order by Fcreate_time DESC";
            //			}
            if (iIDType == 0 || iIDType == 1)
            {
                string fuid = PublicRes.ConvertToFuid(strID);
                if (fuid == null)
                    fuid = "0";

                string whereStr = " where fbuy_uid=" + fuid + " ";
                if (iIDType == 1)
                {
                    whereStr = " where fsale_uid=" + fuid + " ";
                }

                if (dtBegin.ToString("yyyy-MM-dd") != "1900-01-01" && dtEnd.ToString("yyyy-MM-dd") != "4000-01-01")
                {
                    whereStr += "and fcreate_time between '" + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }

                //string count = PublicRes.ExecuteOne("select count(*) from c2c_db.t_refund_list " + whereStr,"ywb");
                string count = "10000";

                //fstrSql = "Select A.*,B.total from c2c_db.t_refund_list A, (Select count(1) as total from c2c_db.t_refund_list "  + whereStr + ") B "
                //+ whereStr		+ " Order by Fcreate_time DESC";
                fstrSql = "select *," + count + " as total from c2c_db.t_refund_list " + whereStr + " Order by Fcreate_time DESC";
            }
            else  //�ͷ�ϵͳδʹ��
            {
                fstrSql = "Select * from c2c_db.t_refund_list where flistid='" + f_strID + "'";
            }
        }

        /// <summary>
        /// �ṩ���������Ե��õĺ������Թ̶����෵��ֵ��
        /// </summary>
        /// <returns></returns>
        public T_REFUND GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            T_REFUND result = new T_REFUND();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    result.u_Bargain_Time = QueryInfo.GetDateTime(dr["fbargain_time"]);
                    result.u_Buy_Bank_Type = QueryInfo.GetInt(dr["fbuy_bank_type"]);
                    result.u_Buy_BankID = QueryInfo.GetString(dr["fbuy_bankid"]);
                    result.u_Buy_Name = QueryInfo.GetString(dr["fbuy_name"]);
                    result.u_BuyID = QueryInfo.GetString(dr["fbuyid"]);
                    result.u_Create_Time = QueryInfo.GetDateTime(dr["fcreate_time"]);
                    result.u_Explain = QueryInfo.GetString(dr["fexplain"]);
                    result.u_IP = QueryInfo.GetString(dr["fip"]);
                    result.u_LState = QueryInfo.GetInt(dr["flstate"]);
                    result.u_Memo = QueryInfo.GetString(dr["fmemo"]);
                    result.u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                    result.u_OK_Time = QueryInfo.GetDateTime(dr["fok_time"]);
                    result.u_PayBuy = QueryInfo.GetInt(dr["fpaybuy"]);
                    result.u_PaySale = QueryInfo.GetInt(dr["fpaysale"]);
                    result.u_PayType = QueryInfo.GetInt(dr["fpaytype"]);
                    result.u_Procedure = QueryInfo.GetInt(dr["fprocedure"]);
                    result.u_RListID = QueryInfo.GetInt(dr["frlistid"]);
                    result.u_Sale_Bank_Type = QueryInfo.GetInt(dr["fsale_bank_type"]);
                    result.u_Sale_BankID = QueryInfo.GetString(dr["fsale_bankid"]);
                    result.u_Sale_Name = QueryInfo.GetString(dr["fsale_name"]);
                    result.u_SaleID = QueryInfo.GetString(dr["fsaleid"]);
                    result.u_SPID = QueryInfo.GetString(dr["fspid"]);
                    result.u_State = QueryInfo.GetInt(dr["fsatate"]);

                    result.u_ListID = QueryInfo.GetString(dr["flistid"]);
                }
                else
                {
                    throw new Exception("û�в��ҵ���Ӧ�ļ�¼");
                }
                da.CloseConn();
                da.Dispose();
                return result;
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }
    }

    #endregion

    #region �˵�ʧ�ܺ���쳣�ļ�¼��ѯ
    public class RefundErrorClass : Query_BaseForNET
    {
        public RefundErrorClass(string batchid, string refundOrder, int orderType, string beginDate, string endDate, int refundType, string bankType,
            int refundPath, int handleType, int errorType, int refundState, string viewOldIds)
        {
            string strWhere = " where t.FoldId=o.FoldID and o.FCreateTime between '" + beginDate + "' and '" + endDate + "' ";

            string uniteFlag = ConfigurationManager.AppSettings["UniteFlag"];

            if (viewOldIds != null && viewOldIds != "")
            {
                viewOldIds = viewOldIds.Replace("|", "','");
                viewOldIds = "'" + viewOldIds + "'";
                strWhere += " and o.FoldId in (" + viewOldIds + ") ";

            }

            if (batchid != null && batchid != "")
            {
                strWhere += " and (o.FHandleBatchId='" + batchid + "' or o.FBeforeBatchId1='" + batchid + "' " +
                    " or o.FBeforeBatchId2='" + batchid + "' or o.FBeforeBatchId3='" + batchid + "' or o.FBeforeBatchId4='" + batchid + "') ";
            }

            if (orderType == 1 && refundOrder != null && refundOrder != "")
            {
                string tmp = uniteFlag + "__" + refundOrder;
                strWhere += " and (t.Fbank_listid='" + refundOrder + "' or t.Fbank_listid like '" + tmp + "')";

            }

            if (orderType == 2 && refundOrder != null && refundOrder != "")
            {

                strWhere += " and t.FPaylistid='" + refundOrder + "' ";

            }

            if (orderType == 3 && refundOrder != null && refundOrder != "")
            {

                strWhere += " and t.FOldId='" + refundOrder + "' ";

            }
            if (orderType == 4 && refundOrder != null && refundOrder != "")
            {
                strWhere += " and  left(t.FPaylistid,10) = '" + refundOrder + "'";
            }


            if (refundType != 99)
            {

                strWhere += " and t.FrefundType=" + refundType + " ";

            }

            if (bankType != "0000")
            {

                strWhere += " and t.Fbanktype=" + bankType + " ";

            }


            if (refundPath != 99)
            {

                strWhere += " and t.FrefundPath=" + refundPath + " ";

            }

            if (errorType != 99)
            {

                strWhere += " and o.FerrorType=" + errorType + " ";

            }

            if (handleType != 99)
            {

                strWhere += " and o.FhandleType=" + handleType + " ";

            }

            if (refundState != 99)
            {

                strWhere += " and t.Fstate=" + refundState + " ";

            }

          //  strWhere += " order by o.Forigbatchid  desc ";

            fstrSql = " select o.FBeforeBatchId1,o.FBeforeBatchId2,o.FBeforeBatchId3,o.FBeforeBatchId4,o.FOrigBatchId,o.FHandleBatchId,o.FPickUser,o.FPickTime,o.FErrMsg,o.FHandleUser,o.FHandleTime,o.FHandleType,o.FHandleMemo,o.FRefundCount,o.FErrorType,o.FRefundMemo,o.FCreateTime,o.Fmodify_Time,o.FAuthorizeFlag,left(o.Forigbatchid ,8) as FBatch_date,t.* "
                + " from c2c_zwdb.t_refund_other o,c2c_zwdb.t_refund_total t " + strWhere;
            fstrSql_count = " select count(*)  from c2c_zwdb.t_refund_other o,c2c_zwdb.t_refund_total t " + strWhere;
        }


        public RefundErrorClass(string oldId)
        {
            fstrSql = " select o.FBeforeBatchId1,o.FBeforeBatchId2,o.FBeforeBatchId3,o.FBeforeBatchId4,o.FOrigBatchId,o.FHandleBatchId,o.FPickUser,o.FPickTime,o.FErrMsg,o.FHandleUser,o.FHandleTime,o.FHandleType,o.FHandleMemo,o.FRefundCount,o.FErrorType,o.FRefundMemo,o.FCreateTime,o.Fmodify_Time,o.FAuthorizeFlag,left(o.Forigbatchid ,8) as FBatch_date,t.* "
                + " from c2c_zwdb.t_refund_other o,c2c_zwdb.t_refund_total t  where o.Foldid=t.foldid and  t.FoldID='" + oldId + "' ";
            fstrSql_count = "select count(*) from c2c_zwdb.t_refund_other o,c2c_zwdb.t_refund_total t  where o.Foldid=t.foldid and t.FoldID='" + oldId + "' ";
        }


    }
    #endregion


    #region B2C�˿��ѯ

    public class B2cReturnClass : Query_BaseForNET
    {

        public string ICESQL = "";

        public B2cReturnClass(string transid, string drawid)
        {
            fstrSql = "select * from c2c_zwdb.t_refund_snap where Ftransaction_id='" + transid + "' ";
            fstrSql_count = "select count(*) from c2c_zwdb.t_refund_snap where Ftransaction_id='" + transid + "' ";

            ICESQL = "transaction_id=" + transid;
            if (drawid != null && drawid.Trim() != "")
            {
                fstrSql = "select * from c2c_zwdb.t_refund_snap where Ftransaction_id='" + transid + "' and  Fdraw_id='" + drawid + "'";
                fstrSql_count = "select count(*) from c2c_zwdb.t_refund_snap where Ftransaction_id='" + transid + "' and  Fdraw_id='" + drawid + "'";

                ICESQL = "transaction_id=" + transid + "&draw_id=" + drawid;
            }

        }


        public B2cReturnClass(string spid, string begintime, string endtime, int refundtype,
            int status, string tranid, string buyqq, string banktype, int sumtype, string drawid)
        {
            string strWhere = " where Fmodify_time  between '" + begintime + "' and '" + endtime + "' ";
            //ICESQL = "start_time=" + begintime + "&end_time=" + endtime;
            ICESQL = "start_time_modify=" + begintime + "&end_time_modify=" + endtime;//�޸�Ϊ����modifytime��ѯ

            if (sumtype != 99)
            {
                if (sumtype == 0)
                {
                    strWhere += " and (Fstandby1 is null or Fstandby1=0) ";
                    ICESQL += "&standby1=0";
                }
                else
                {
                    strWhere += " and Fstandby1=" + sumtype + " ";
                    ICESQL += "&standby1=" + sumtype;
                }
            }

            if (drawid != null && drawid.Trim() != "")
            {
                strWhere += " and Fdraw_id='" + drawid + "' ";
                ICESQL += "&draw_id=" + drawid;
            }


            if (spid != null && spid.Trim() != "")
            {
                strWhere += " and Fspid='" + spid + "' ";
                ICESQL += "&spid=" + spid;
            }

            if (refundtype != 99)
            {
                strWhere += " and Frefund_type=" + refundtype + " ";
                ICESQL += "&refund_type=" + refundtype;
            }

            if (status != 99)
            {
                strWhere += " and Fstatus=" + status + " ";
                ICESQL += "&status=" + status;
            }

            if (tranid != null && tranid.Trim() != "")
            {
                strWhere += " and Ftransaction_id='" + tranid + "' ";
                ICESQL += "&transaction_id=" + tranid;
            }

            if (buyqq != null && buyqq.Trim() != "")
            {
                //furion 20061121 email��¼�޸�.
                //strWhere += " and Fbuyid='" + buyqq + "' ";
                string fuid = PublicRes.ConvertToFuid(buyqq);
                strWhere += " and Fbuy_uid=" + fuid + " ";

                ICESQL += "&buy_uid=" + fuid;
            }

            if (banktype != null && banktype.Trim() != "" && banktype.Trim() != "0000")
            {
                strWhere += " and Fbank_type=" + banktype + " ";
                ICESQL += "&bank_type=" + banktype;
            }

            fstrSql = " select * from c2c_zwdb.t_refund_snap " + strWhere;
            fstrSql_count = "select 10000";//" select count(*) from c2c_db_inc.t_spm_refund " + strWhere;

            //ICESQL = "strwhere=" + strWhere.Trim();
        }

        /// <summary>
        /// �����˿�
        /// </summary>
        /// <param name="transid">���׵�ID</param>
        /// <param name="msg">������Ϣ</param>
        /// <returns>�����Ƿ�ɹ�</returns>
        public static bool SuspendRefundment(ArrayList al, string UserIP, out string msg)
        {
            msg = "";

            if (al == null || al.Count == 0)
            {
                msg = "�������ȷ�Ľ��׵�ID";
                return false;
            }

            //���ж��Ƿ��д˼�¼��״̬�Ƿ���ȷ.Frefund_type=3 - ����ֱ���˿�
            //Fstatus=1 - ������
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            MySqlAccess dazw = new MySqlAccess(PublicRes.GetConnString("ZWTK"));
            try
            {
                //da.OpenConn();
                dazw.OpenConn();
                //da.StartTran();

                foreach (object obj in al)
                {
                    string transid = obj.ToString();

                    string drawid = "";
                    string refund_type = "";
                    if (transid.IndexOf(";") > 0)
                    {
                        string[] split = transid.Split(';');
                        if (split.Length != 3)
                        {
                            msg = "���ղ�����Ҫ�󣬳����˿�ʧ�ܣ�tranid=" + transid;
                            return false;
                        }

                        transid = split[0].Trim();
                        drawid = split[1].Trim();
                        refund_type = split[2].Trim();

                    }

                    //					string strSql = " select count(*) from c2c_db_inc.t_spm_refund where Ftransaction_id='" 
                    //						+ transid + "' and Frefund_type=3 and Fstatus=9 and (Fstandby1 is null or Fstandby1=0) ";
                    //
                    //					if(drawid != "")
                    //					{
                    //						strSql = " select count(*) from c2c_db_inc.t_spm_refund where Ftransaction_id='" 
                    //							+ transid + "' and Fdraw_id='" + drawid + "'  and Frefund_type=3 and Fstatus=9 and (Fstandby1 is null or Fstandby1=0) ";
                    //					}

                    /*
                    string tmp = da.GetOneResult(strSql);

                    if(tmp == null || tmp.Trim() == "" || tmp != "1")
                    {
                        da.RollBack();
                        msg = "ִ���˿�������������ڴ˼�¼���ߴ˼�¼״̬����tranid=" + transid;
                        return false;
                    }
                    */


                    //��ѯ�»��ܱ���û�д˼�¼

                    string strSql = "select count(*) from c2c_zwdb.t_refund_total where  fpaylistid='" + transid + "' ";
                    if (drawid != "")
                    {
                        strSql = "select count(*) from c2c_zwdb.t_refund_total where  fpaylistid='" + transid + "' and foldid='" + drawid + "'";
                    }

                    string count = dazw.GetOneResult(strSql);
                    if (count != "0")
                    {
                        //da.RollBack();
                        msg = "ִ���˿�����������˿���Ϣ�Ѿ����ܵ��˿���ܱ���tranid=" + transid;
                        return false;
                    }

                    string memo = "�̻����볷���˿�";


                    /*
                    strSql = " update c2c_db_inc.t_spm_refund set Fstatus=8  ,Fip='" 
                        + myheader.UserIP + "',Fmodify_time='" + PublicRes.strNowTimeStander + "',fmemo='"+memo+"',Fstandby1=2 where Ftransaction_id='" 
                        + transid + "' and Frefund_type=3 and Fstatus=9 and (Fstandby1 is null or Fstandby1=0)";

                    if(drawid != "")
                    {
                        strSql = " update c2c_db_inc.t_spm_refund set Fstatus=8  ,Fip='" 
                            + myheader.UserIP + "',Fmodify_time='" + PublicRes.strNowTimeStander + "',fmemo='"+memo+"',Fstandby1=2 where Ftransaction_id='" 
                            + transid + "' and Fdraw_id='" + drawid + "'   and Frefund_type=3 and Fstatus=9 and (Fstandby1 is null or Fstandby1=0)";
                    }

                    if(da.ExecSqlNum(strSql) != 1)
                    {
                        da.RollBack();
                        msg = "ִ���˿���������ڸ��¼�¼״̬ʱ���� transid=" + transid;
                        return false;
                    }
                    */

                    /*����Ϊ�µĸ��·�ʽ andrew 20120515
                    strSql = "transaction_id=" + transid + "&refund_type="+refund_type+"&status=9&standby1null=0";
                    if(drawid != "")
                    {
                        strSql = "transaction_id=" 
                            + transid + "&draw_id=" + drawid + "&refund_type="+refund_type+"&status=9&standby1null=0";
                    }

                    strSql += "&setstatus=8";
                    strSql += "&setstandby1=2";
                    strSql += "&ip=" + myheader.UserIP;
                    strSql += "&modify_time=" + PublicRes.strNowTimeStander;

                    int iresult = CommQuery.ExecSqlFromICE(strSql,CommQuery.UPDATE_SPMREFUND,out msg);

                    if(iresult != 1)
                    {
                        //da.RollBack();
                        msg = "ִ���˿���������ڸ��¼�¼״̬ʱ���� transid=" +  transid;
                        return false;
                    }
                    */

                    string modifyMsg = "";
                    bool modifyFlag = PublicRes.ModifySPMRefundService((int)PublicRes.ModifySPMRefundType.���볷���˿�23, transid, drawid, out modifyMsg);
                    if (!modifyFlag)
                    {
                        throw new LogicException(modifyMsg);

                    }
                    else
                    {
                        //���Ӹ��¿��ջ��ܱ�־ andrew 20120515
                        string snapMsg = "";
                        bool updateSnap = UpdataRefundSnapSuspend(drawid, transid, UserIP, dazw, true, out snapMsg);
                        if (!updateSnap)
                        {
                            msg = snapMsg;
                            return false;
                        }
                    }

                }
                //da.Commit();
                return true;
            }
            catch (Exception err)
            {
                //da.RollBack();
                log4net.ILog log = log4net.LogManager.GetLogger("B2cReturnClass");
                if (log.IsErrorEnabled) log.Error("SuspendRefundment ", err);
                msg = err.Message;
                return false;
            }
            finally
            {
                dazw.Dispose();
                //da.Dispose();
            }

        }

        //���¿��ձ���ܱ�־��״̬
        //���¿��ձ���ܱ�־��״̬
        private static bool UpdataRefundSnapSuspend(string drawid, string tranid, string userIP, MySqlAccess dazw, bool suspend, out string msg)
        {
            msg = "";
            try
            {
                if (drawid == null || drawid == "" || tranid == null || tranid == "")
                {
                    msg = "drawid��tranid����Ϊ��";
                    return false;
                }
                string strSql = "";
                if (suspend)
                {
                    strSql = "update  c2c_zwdb.t_refund_snap set Fstatus=8, Fstandby1=2 ,Fip='"
                        + userIP + "',Fmodify_time='" + PublicRes.strNowTimeStander + "' where Fstatus=9  and Ftransaction_id='"
                        + tranid + "' and Fdraw_id='" + drawid + "' and ifnull(Fstandby1,0) =0 ";
                }
                else
                {
                    strSql = "update  c2c_zwdb.t_refund_snap set Fstatus=9, Fstandby1=0 ,Fip='"
                        + userIP + "',Fmodify_time='" + PublicRes.strNowTimeStander + "' where Fstatus=8  and Ftransaction_id='"
                        + tranid + "' and Fdraw_id='" + drawid + "' and Fstandby1=2 ";
                }
                int count = dazw.ExecSqlNum(strSql);
                if (count != 1)
                {
                    msg = "�����˿����뵥���ձ��¼��Ϊ:" + count + " Fdraw_id=" + drawid;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;

            }
        }

    }
    #endregion


    #region �˿��ѯ��

    public class RefundQueryClass : Query_BaseForNET
    {
        public RefundQueryClass(string batchid, int ifromtype, int irefundtype, int irefundstate, int ireturnstate, string listid)
        {
            string strWhere = " where Fbatchid='" + batchid + "' ";
            if (ifromtype != 9)
            {
                strWhere += " and FrefundType=" + ifromtype;
            }

            if (irefundtype != 9)
            {
                strWhere += " and FrefundPath=" + irefundtype;
            }

            if (irefundstate != 9)
            {
                strWhere += " and Fstate=" + irefundstate;
            }

            if (ireturnstate != 9)
            {
                strWhere += " and FreturnState=" + ireturnstate;
            }

            if (listid != null && listid.Trim() != "")
            {
                strWhere += " and FPaylistid='" + listid.Trim() + "' ";
            }

            fstrSql = "select * from c2c_zwdb.t_refund_total " + strWhere;
            fstrSql_count = "select count(*) from c2c_zwdb.t_refund_total " + strWhere;
        }
    }

    #endregion


    #region �̻�����Ա��ѯ

    public class MediOperatorManageClass : Query_BaseForNET
    {
        public string ICESQL = "";

        public static int GetRole(string spid, string qq, int signorder)
        {
            if (signorder < 1 || signorder > 10)//��������ϵͳ��4��Ϊ10
            {
                throw new LogicException("Ȩ��λԽ��");
            }

            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YWB_30"));
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));
            try
            {
                /*
                // TODO: 1�ͻ���Ϣ��������
                da.OpenConn();
                string strSql = "select Fsign" + signorder + " from c2c_db.t_muser_user where Fqqid='" + qq 
                    + "' and FSpid='" + spid + "'";

                return Int32.Parse(da.GetOneResult(strSql));
                */

                string strSql = "qqid=" + qq + "&spid=" + spid + "";
                string fieldstr = "Fsign" + signorder;
                string errMsg = "";
                string sign = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MUSER, fieldstr, out errMsg);

                int isign = Int32.Parse(sign);
                return isign;
            }
            finally
            {
                //da.Dispose();
            }
        }

        public static bool SetRole(string spid, string qq, int newrole, int signorder)
        {
            if (signorder < 1 || signorder > 4)
            {
                throw new LogicException("Ȩ��λԽ��");
            }

            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YW_30"));
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));
            try
            {
                // TODO: 1�ͻ���Ϣ��������
                /*
                string STRSQL = "update c2c_db.t_muser_user set Fsign" + signorder + "={0} where Fqqid='{1}' and FSpid='" + spid + "'";

                */

                string STRSQL = "qqid={1}&spid=" + spid;
                STRSQL += "&modify_time=" + PublicRes.strNowTimeStander;
                STRSQL += "&sign" + signorder + "={0}";

                //da.OpenConn();
                string strSql = "";

                string str = Convert.ToString((long)newrole, 2);
                str = str.PadLeft(32, '0');

                if (spid == qq)
                {
                    // TODO: 1�ͻ���Ϣ��������
                    //��У��,������޸ĵĹ���Ա,��Ѳ���Ա�Ĺ���ԱΪ0��Ȩ��λҲ��0
                    /*
                    strSql = " select Fqqid,Fsign" + signorder + " from  c2c_db.t_muser_user where Fspid='" + spid + "' ";
                    DataTable dt = da.GetTable(strSql);
                    */

                    string errMsg = "";
                    strSql = "spid=" + spid;
                    DataTable dt = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_MUSER, out errMsg);

                    if (dt == null || dt.Rows.Count == 0)
                        return false;

                    foreach (DataRow dr in dt.Rows)
                    {
                        string operatorqq = dr["Fqqid"].ToString().Trim();
                        if (operatorqq == spid)
                        {
                            int changedrole = Convert.ToInt32(str, 2);
                            strSql = String.Format(STRSQL, changedrole, spid);
                            /*
                            da.ExecSqlNum(strSql);
                            */
                            CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_MUSER, out errMsg);
                        }
                        else
                        {
                            /*
                            strSql = " select Fsign" + signorder + " from c2c_db.t_muser_user where Fqqid='" + operatorqq + "' and Fspid='" + spid + "'";
                            long opersign = long.Parse(da.GetOneResult(strSql));
                            */
                            strSql = "spid=" + spid + "&qqid=" + operatorqq;
                            long opersign = long.Parse(CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MUSER, "Fsign" + signorder, out errMsg));

                            string strspsign = Convert.ToString(opersign, 2);
                            strspsign = strspsign.PadLeft(32, '0');

                            for (int i = 0; i <= 31; i++)
                            {
                                if (str[i] == '0')
                                {
                                    strspsign = strspsign.Substring(0, i) + "0" + strspsign.Substring(i + 1, 32 - i - 1);
                                }
                            }

                            int changedrole = Convert.ToInt32(strspsign, 2);
                            strSql = String.Format(STRSQL, changedrole, operatorqq);
                            /*
                            da.ExecSqlNum(strSql);
                            */
                            CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_MUSER, out errMsg);
                        }
                    }

                    return true;
                }
                else if (qq.StartsWith(spid) && spid != qq)
                {
                    // TODO: 1�ͻ���Ϣ��������
                    //����޸ĵ��ǲ���Ա,��ѹ���ԱΪ0��Ȩ��λҲ��0
                    /*
                    strSql = " select Fsign" + signorder + " from c2c_db.t_muser_user where Fqqid='" + spid + "' and Fspid='" + spid + "'";
                    long spsign = long.Parse(da.GetOneResult(strSql));
                    */

                    string errMsg = "";
                    strSql = "qqid=" + spid + "&spid=" + spid;
                    long spsign = long.Parse(CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MUSER, "Fsign" + signorder, out errMsg));

                    string strspsign = Convert.ToString(spsign, 2);
                    strspsign = strspsign.PadLeft(32, '0');

                    for (int i = 0; i <= 31; i++)
                    {
                        if (strspsign[i] == '0')
                        {
                            str = str.Substring(0, i) + "0" + str.Substring(i + 1, 32 - i - 1);
                        }
                    }

                    int changedrole = Convert.ToInt32(str, 2);
                    strSql = String.Format(STRSQL, changedrole, qq);

                    /*
                    return da.ExecSqlNum(strSql) == 1;
                    */
                    int iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_MUSER, out errMsg);
                    return iresult == 1;
                }
                else
                    return false;

            }
            finally
            {
                //da.Dispose();
            }
        }

        public MediOperatorManageClass(string spid, string account)
        {
            string strwhere = "";
            if (spid.Trim() != "")
            {
                strwhere += " where Fspid='" + spid + "' ";
                ICESQL = "spid=" + spid;
            }

            if (account.Trim() != "")
            {
                if (strwhere == "")
                {
                    strwhere += " where Fqqid='" + account + "' ";
                    ICESQL = "qqid=" + account;
                }
                else
                {
                    strwhere += " and Fqqid='" + account + "' ";
                    ICESQL += "&qqid=" + account;
                }
            }


            /*
            // TODO: 1�ͻ���Ϣ��������
            fstrSql = " select * from c2c_db.t_muser_user " + strwhere;*/

            fstrSql = "strwhere=" + strwhere.Trim();
            fstrSql_count = "select 10000";//" select count(*) from c2c_db.t_muser_user " + strwhere;

        }

    }
    #endregion

    #region ���ֹ����ѯ
    public class AppealDQuery : Query_BaseForNET
    {
        public AppealDQuery(string FSpid, string FUser, int FPriType, int FState)
        {
            string wheres = " where 1=1 ";

            if (FSpid != null && FSpid != "") wheres += " and FSpid = '" + FSpid + "' ";
            if (FUser != null && FUser != "") wheres += " and FUser like '" + FUser + "' ";
            if (FPriType != -1) wheres += " and FPriType =" + FPriType.ToString() + " ";
            if (FState != -1) wheres += " and FState=" + FState.ToString();

            fstrSql = "SELECT * FROM c2c_db.t_appeal_d " + wheres + " order by FNo desc";
            fstrSql_count = "SELECT count(1) FROM c2c_db.t_appeal_d " + wheres;
        }
    }
    #endregion

    #region ��������ѯ
    public class AppealSQuery : Query_BaseForNET
    {
        public AppealSQuery(string Fno, string FUserType, string FUser, string FState)
        {
            string wheres = " where 1=1 ";

            if (Fno != null && Fno != "") wheres += " and Fno=" + Fno;
            if (FUserType != null && FUserType != "") wheres += " and FUserType=" + FUserType;
            if (FUser != null && FUser != "") wheres += " and FUser like '%" + FUser + "%' ";
            if (FState != null && FState != "") wheres += " and FState=" + FState;

            fstrSql = "SELECT * FROM c2c_merchant.t_appeal_s " + wheres + " order by FNo desc";
            fstrSql_count = "SELECT count(1) FROM c2c_merchant.t_appeal_s " + wheres;
        }
    }
    #endregion


    #region	����/�����ҽ��׵���

    /// <summary>
    /// ����/�����ҽ��׵���Ĳ�ѯ��
    /// </summary>
    public class Q_PAY_LIST_BYTYPE : Query_BaseForNET
    {
        private string f_strID;
        private int f_iType;
        private DateTime f_dtBegin;
        private DateTime f_dtEnd;


        public Q_PAY_LIST_BYTYPE(string strID, DateTime dtBegin, DateTime dtEnd, int iType)
        {
            f_iType = iType;
            f_strID = strID;
            f_dtBegin = dtBegin;
            f_dtEnd = dtEnd;

            //fstrSql = "Select * from  " + PublicRes.GetTableName("t_pay_list",f_strID) ;
            fstrSql = "Select *,Ftrade_state as Fstate from  " + PublicRes.GetTableName("t_user_order", f_strID);
            //�����������,0
            if (f_iType == 0)
            {
                fstrSql += "  where fbuyid='" + f_strID + "' ";
            }
            else
            {
                fstrSql += "  where fsaleid='" + f_strID + "' ";
            }

            if (dtBegin.ToString("yyyy-MM-dd") != "1900-01-01" && dtEnd.ToString("yyyy-MM-dd") != "4000-01-01")
            {
                fstrSql += " and fcreate_time between '" + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '"
                    + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
        }

        /// <summary>
        /// �ṩ���������Ե��õĺ������Թ̶����෵��ֵ��
        /// </summary>
        /// <returns></returns>
        public T_PAY_LIST[] GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZJB"));

            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();

                dt = da.GetTable(fstrSql);

                if (dt.Rows.Count > 0)
                {
                    T_PAY_LIST[] result = new T_PAY_LIST[dt.Rows.Count];
                    int i = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        result[i] = new T_PAY_LIST();

                        result[i].u_Bank_ListID = QueryInfo.GetString(dr["fbank_listid"]);
                        result[i].u_Bargain_Time = QueryInfo.GetDateTime(dr["fbargain_time"]);
                        result[i].u_Buy_Bank_Type = QueryInfo.GetInt(dr["fbuy_bank_type"]);
                        result[i].u_Buy_BankID = QueryInfo.GetString(dr["fbuy_bankid"]);
                        result[i].u_Buy_Name = QueryInfo.GetString(dr["fbuy_name"]);
                        result[i].u_BuyID = QueryInfo.GetString(dr["fbuyid"]);
                        result[i].u_Carriage = QueryInfo.GetInt(dr["fcarriage"]);
                        result[i].u_Cash = QueryInfo.GetInt(dr["fcash"]);
                        result[i].u_Coding = QueryInfo.GetString(dr["fcoding"]);
                        result[i].u_Create_Time = QueryInfo.GetDateTime(dr["fcreate_time"]);
                        result[i].u_Create_Time_C2C = QueryInfo.GetDateTime(dr["fcreate_time_c2c"]);
                        result[i].u_CurType = QueryInfo.GetInt(dr["fcurtype"]);
                        result[i].u_Explain = QueryInfo.GetString(dr["fexplain"]);
                        result[i].u_Fact = QueryInfo.GetInt(dr["ffact"]);
                        result[i].u_IP = QueryInfo.GetString(dr["fip"]);
                        result[i].u_LState = QueryInfo.GetInt(dr["flstate"]);
                        result[i].u_Memo = QueryInfo.GetString(dr["fmemo"]);
                        result[i].u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                        result[i].u_Pay_Type = QueryInfo.GetInt(dr["fpay_type"]);
                        result[i].u_PayNum = QueryInfo.GetInt(dr["fpaynum"]);
                        result[i].u_Price = QueryInfo.GetInt(dr["fprice"]);
                        result[i].u_Procedure = QueryInfo.GetInt(dr["fprocedure"]);
                        result[i].u_Receive_Time = QueryInfo.GetDateTime(dr["freceive_time"]);
                        result[i].u_Receive_Time_C2C = QueryInfo.GetDateTime(dr["freceive_time_c2c"]);
                        result[i].u_Sale_Name = QueryInfo.GetString(dr["fsale_name"]);
                        result[i].u_SaleID = QueryInfo.GetString(dr["fsaleid"]);
                        result[i].u_Service = QueryInfo.GetInt(dr["fservice"]);
                        result[i].u_SPID = QueryInfo.GetString(dr["fspid"]);
                        result[i].u_State = QueryInfo.GetInt(dr["ftrade_state"]);

                        result[i].u_Pay_Time = QueryInfo.GetDateTime(dr["fpay_time"]);

                        i++;
                    }
                    da.CloseConn();
                    da.Dispose();
                    return result;
                }
                else
                {
                    da.CloseConn();
                    da.Dispose();
                    throw new Exception("û�в��ҵ���Ӧ�ļ�¼");
                }
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }
    }
    #endregion


    #region ��ֵ��ѯ�ࡣ

    public class FundQueryClass : Query_BaseForNET
    {
        public string ICESQL = "";
        public string ICETYPE = "";
        public string HISSQL = "";

        public FundQueryClass(string u_ID, string u_QueryType, int fcurtype, DateTime u_BeginTime, DateTime u_EndTime, int fstate,
            float fnum, float fnumMax, string banktype, string sorttype, bool isHistory)
        {
            string strGroup = "";
            string strWhere = "";

            //furion 20090515 ����ط��ĺϵ���ѯ,��Ϊunion ��������������ѯ.
            string strsource = "";
            string strUnite = "";

            string strsource_ice = "";
            string strunite_ice = "";

            string uniteFlag = ConfigurationManager.AppSettings["UniteFlag"];
            if (string.IsNullOrEmpty(u_ID))
            {
                throw new Exception("ID ����Ϊ��");
            }
            else if (u_QueryType.ToLower() == "qq")  //����QQ�Ų�ѯ��ע��ʹ���ڲ�uid
            {
                string uid = PublicRes.ConvertToFuid(u_ID);
                strWhere += " where fauid='" + uid + "' ";
                ICESQL = "auid=" + uid;
            }
            else if (u_QueryType.ToLower() == "tobank")  //�����еĶ�����
            {
                strsource = " where Fbank_list='" + u_ID + "' ";
                strUnite = " where Fbank_list in ('" + uniteFlag + "01" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "02" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "03" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "04" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "05" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "06" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "07" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "08" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "09" + u_ID + "') ";
                strWhere += strsource;
                strsource_ice = "bank_list_or=" + u_ID;
                strunite_ice = "bank_list_likestr=" + u_ID;
            }
            else if (u_QueryType.ToLower() == "bankback") //���з���u
            {
                strsource = " where Fbank_acc='" + u_ID + "' ";
                strUnite = " where Fbank_acc in ('" + uniteFlag + "01" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "02" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "03" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "04" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "05" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "06" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "07" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "08" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "09" + u_ID + "') ";
                strWhere += strsource;
                strsource_ice = "bank_acc_or=" + u_ID;
                strunite_ice = "bank_acc_likestr=" + u_ID;
            }
            else if (u_QueryType.ToLower() == "czd")
            {
                strWhere += " where listid='" + u_ID + "' ";
                ICESQL = "listid=" + u_ID;
            }
            else
            {
                throw new Exception("queryType ����ȷ");
            }

            if (fstate != 0)
            {
                if (strWhere != "")
                {
                    ICESQL += "&sign=" + fstate;
                    strWhere += " and FSign=" + fstate.ToString() + " ";
                }
                else
                {
                    ICESQL = "sign=" + fstate;
                    strWhere = " where FSign=" + fstate.ToString() + " ";
                }
            }

            long num = (long)Math.Round(fnum * 100, 0);
            long numMax = (long)Math.Round(fnumMax * 100, 0);

            if (strWhere != "")
            {
                ICESQL += "&num_start=" + num;
                ICESQL += "&num_end=" + numMax;
                strWhere += " and Fnum>" + num.ToString() + " and Fnum<" + numMax.ToString() + " ";
            }
            else
            {
                ICESQL = "num_start=" + num;
                ICESQL += "&num_end=" + numMax;
                strWhere = " where Fnum>" + num.ToString() + " and Fnum<" + numMax.ToString() + " ";
            }

            if (banktype != "0000")
            {
                strWhere += " and Fbank_type=" + banktype + " ";
                ICESQL += "&bank_type=" + banktype;
            }

            string test = u_BeginTime.ToString("yyyy-MM-dd");

            if (test != "1940-01-01")
            {
                if (strWhere != "")
                {

                    strWhere += " and Fpay_front_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'  and Fcurtype=" + fcurtype + " ";
                    ICESQL += "&fronttime_start=" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ICESQL += "&fronttime_end=" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ICESQL += "&curtype=" + fcurtype; //rowena 20100722���ӻ�����Ŀ

                }
                else
                {
                    strWhere += " where Fpay_front_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'  and Fcurtype=" + fcurtype + " ";
                    ICESQL = "fronttime_start=" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ICESQL += "&fronttime_end=" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ICESQL += "&curtype=" + fcurtype; //rowena 20100722���ӻ�����Ŀ
                }
            }

            ICETYPE = CommQuery.QUERY_TCBANKROLL;
            if (test != "1940-01-01")
            {
                //��Ч�ʿ���,ֻȡ��ʼʱ��ͽ���ʱ�������µ���ʷ��¼,������ȡ.
                //DateTime tmpDate = u_BeginTime.AddMonths(-1);
                DateTime tmpDate = u_BeginTime.AddMonths(-1);//��ǰ��ѯһ���°�
                DateTime date_End = u_EndTime;
                if (DateTime.Now.CompareTo(u_BeginTime.AddMonths(2)) > 0)//Ĭ������Ѱ4���µ���ʷ��¼��
                {
                    date_End = u_BeginTime.AddMonths(2);
                }

                while (tmpDate <= date_End)
                {
                    //string TableName = "c2c_db_receive.t_tcbankroll_list_" + tmpDate.ToString("yyyyMM");
                    string TableName = "c2c_db_history.t_tcbankroll_list_" + tmpDate.ToString("yyyyMM");
                    strGroup = strGroup + " select * from " + TableName + strWhere + " union all";
                    tmpDate = tmpDate.AddMonths(1);
                    string strTmp = tmpDate.ToString("yyyy-MM-");
                    tmpDate = DateTime.Parse(strTmp + "01 00:00:01");
                }
                if (isHistory)
                {
                    ICETYPE = CommQuery.QUERY_TCBANKROLL_HISTORY;
                }
                else
                {
                    ICETYPE = CommQuery.QUERY_TCBANKROLL;
                }
            }
            ICESQL += "&start_time=" + test; //����ʱ����� andrew 20110322
            string TableName1 = "c2c_db.t_tcbankroll_list";
            strGroup = strGroup + " select * from " + TableName1 + strWhere + " ";
            //furion 20090515 ����ط��ĺϵ���ѯ,��Ϊunion ��������������ѯ.
            if (strsource != "" && strUnite != "")
            {
                strGroup += " union all " + strGroup.Replace(strsource, strUnite);
                ICESQL += "&" + strsource_ice;
                ICESQL += "&" + strunite_ice;
            }

            string strorder = "";
            if (sorttype != null && sorttype.Trim() != "")
            {
                if (sorttype.Trim() == "1")
                {
                    if (u_QueryType == "total")
                    {
                        strorder = " order by Fpay_time_acc asc ";
                    }
                    else
                    {
                        strorder = " order by Fpay_front_time asc ";
                    }
                }
                else if (sorttype.Trim() == "2")
                {
                    if (u_QueryType == "total")
                    {
                        strorder = " order by Fpay_time_acc desc ";
                    }
                    else
                    {
                        strorder = " order by Fpay_front_time desc ";
                    }
                }
                else if (sorttype.Trim() == "3")
                {
                    strorder = " order by Fnum asc ";
                }
                else if (sorttype.Trim() == "4")
                {
                    strorder = " order by Fnum desc ";
                }
            }
            fstrSql = strGroup + strorder;
            fstrSql_count = "select 100000";
            HISSQL = strWhere + strorder;
        }

        public FundQueryClass(string tdeid, string listid, DateTime u_BeginTime, DateTime u_EndTime, bool oldflag, bool isHistory)
        {
            if (string.IsNullOrEmpty(tdeid) && string.IsNullOrEmpty(listid))
            {
                throw new Exception("��ѯ�������߱�");
            }
            string strGroup = "";
            string strWhere = "";

            if (tdeid != null && tdeid.Trim() != "")
            {
                ICESQL = "tde_id=" + tdeid;
                strWhere += " where Ftde_id=" + tdeid + "  ";
            }
            else
            {
                strWhere += "";
            }

            if (listid != null && listid.Trim() != "")
            {
                ICESQL += "&listid=" + listid;
            }

            string test = u_BeginTime.ToString("yyyy-MM-dd");
            if (test != "1940-01-01")
            {
                if (strWhere != "")
                {
                    ICESQL += "&fronttime_start=" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ICESQL += "&fronttime_end=" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss");

                    strWhere += " and Fpay_front_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }
                else
                {
                    ICESQL += "fronttime_start=" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ICESQL += "&fronttime_end=" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss");

                    strWhere = " where Fpay_front_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }
            }

            ICETYPE = CommQuery.QUERY_TCBANKROLL;

            if (test != "1940-01-01" && isHistory)
            {
                //��Ч�ʿ���,ֻȡ��ʼʱ��ͽ���ʱ�������µ���ʷ��¼,������ȡ.
                DateTime tmpDate = u_BeginTime.AddMonths(-1);

                DateTime date_End = u_EndTime;
                if (DateTime.Now.CompareTo(u_BeginTime.AddMonths(2)) > 0)//Ĭ������Ѱ4���µ���ʷ��¼��
                {
                    date_End = u_BeginTime.AddMonths(2);
                }
                while (tmpDate <= date_End)
                {
                    string TableName = "c2c_db_history.t_tcbankroll_list_" + tmpDate.ToString("yyyyMM");

                    strGroup = strGroup + " select * from " + TableName + strWhere + " union all";

                    tmpDate = tmpDate.AddMonths(1);

                    string strTmp = tmpDate.ToString("yyyy-MM-");
                    tmpDate = DateTime.Parse(strTmp + "01 00:00:01");
                }
                if (isHistory)
                {
                    ICETYPE = CommQuery.QUERY_TCBANKROLL_HISTORY;
                }
                else
                {
                    ICETYPE = CommQuery.QUERY_TCBANKROLL;
                }
            }

            ICESQL += "&start_time=" + u_BeginTime.ToString("yyyy-MM-dd");  //����ʱ����� andrew 20110322
            string TableName1 = "c2c_db.t_tcbankroll_list";
            strGroup = strGroup + " select * from " + TableName1 + strWhere + " ";

            fstrSql = strGroup;
            fstrSql_count = "select 10000";//" select count(*) from ( " + strGroup + ")  a ";
        }
    }

    #endregion

    #region ͬ����¼��ѯ

    public class SynRecordClass : Query_BaseForNET
    {
        public static string GetNowSynTableName()
        {
            int idate = DateTime.Now.Day;
            string nowtablename = DateTime.Now.ToString("yyyyMM");

            if (idate < 11)
            {
                nowtablename += "01";
            }
            else if (idate < 21)
            {
                nowtablename += "02";
            }
            else
                nowtablename += "03";

            return nowtablename;

        }

        public static string GetSynTableNameFromDate(DateTime dt)
        {
            int idate = dt.Day;
            string nowtablename = dt.ToString("yyyyMM");

            if (idate < 11)
            {
                nowtablename += "01";
            }
            else if (idate < 21)
            {
                nowtablename += "02";
            }
            else
                nowtablename += "03";

            return nowtablename;

        }
        public static string GetPriorSynTableNameFromDate(DateTime dt)
        {
            string nowtablename = GetSynTableNameFromDate(dt);

            if (nowtablename.EndsWith("01"))
            {
                nowtablename = DateTime.Now.AddMonths(-1).ToString("yyyyMM") + "03";
            }
            else
            {
                int inowindex = Int32.Parse(nowtablename.Substring(6, 2));
                inowindex--;
                nowtablename = nowtablename.Substring(0, 6) + "0" + inowindex.ToString();
            }

            return nowtablename;
        }

        public static string GetPriorSynTableName()
        {
            string nowtablename = GetNowSynTableName();

            if (nowtablename.EndsWith("01"))
            {
                nowtablename = DateTime.Now.AddMonths(-1).ToString("yyyyMM") + "03";
            }
            else
            {
                int inowindex = Int32.Parse(nowtablename.Substring(6, 2));
                inowindex--;
                nowtablename = nowtablename.Substring(0, 6) + "0" + inowindex.ToString();
            }

            return nowtablename;
        }

        /*
        public static string GetPriorSynTableName()
        {
            string nowtablename = GetNowSynTableName();

            if(nowtablename.EndsWith("01"))
            {
                nowtablename = DateTime.Now.AddMonths(-1).ToString("yyyyMM") + "01";
            }
            else
            {
                int inowindex = Int32.Parse(nowtablename.Substring(6,2));
                inowindex --;
                nowtablename = nowtablename.Substring(0,6) + "0" + inowindex.ToString();
            }

            return nowtablename;
        }
        */

        public string ICESQL = "";
        public static void ResetSynRecordState(string transid, string createtime, int inum)
        {
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("SYNINFO"));
            try
            {
                //furion �õ���ǰʱ��ֵ.
                //long timestamp = long.Parse(Common.CommLib.commRes.GetTickFromTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                /*
                string strSql = "update cft_syn_record.t_syn_record set Fsyn_count=" + inum 
                    + ",Fsyn_status=2,Flast_modify_time="+timestamp+",Fsyn_status=1 where Ftransaction_id='" + transid + "'";

                if(inum > 0)
                {
                    strSql = "update cft_syn_record.t_syn_record set Fsyn_count=" + inum 
                        + ",Fsyn_status=2,Flast_modify_time="+timestamp+" where Ftransaction_id='" + transid + "'";
                }

                da.OpenConn();

                da.ExecSql(strSql);
                */

                string Msg = "";
                string sql = "transaction_id=" + transid + "&create_time=" + SynRecordClass.GetNowSynTableName();
                string pay_type = CommQuery.GetOneResultFromICE(sql, CommQuery.QUERY_SYNREC_ID, "Fpay_type", out Msg);

                if (pay_type == null || pay_type.Trim() == "")
                {
                    sql = "transaction_id=" + transid + "&create_time=" + SynRecordClass.GetPriorSynTableName();
                    pay_type = TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.GetOneResultFromICE(sql, CommQuery.QUERY_SYNREC_ID, "Fpay_type", out Msg);

                    if (pay_type == null || pay_type.Trim() == "")
                    {
                        //�Ҳ�����¼����ʹ���ýӿ�Ҳ��ʧ�ܡ�
                        Msg = "���ýӿ�syn_update4KZ_serviceǰ����ѯ��¼ʧ��" + transid;
                        TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.sendLog4Log("TradeSynch.noc2cSynch", Msg);
                        return;
                    }
                }

                string inmsg1 = "&transaction_id=" + transid;
                inmsg1 += "&update_type=2";
                inmsg1 += "&pay_type=" + pay_type;
                //inmsg1 += "&call_source=KZ";				
                inmsg1 += "&modify_time=" + TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                string reply = "";
                short result;
                string msg = "";


                if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("syn_update4KZ_service", inmsg1, true, out reply, out result, out msg))
                {
                    if (result != 0)
                    {
                        Msg = "���ýӿ�syn_update4KZ_serviceδ�ܳɹ� result=" + result + "��msg=" + msg;
                        TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.sendLog4Log("TradeSynch.noc2cSynch", Msg);
                    }
                    else
                    {
                        if (reply.IndexOf("result=0") > -1)
                        {
                            return;
                        }
                        else
                        {
                            Msg = "���ýӿ�syn_update4KZ_serviceδ�ܳɹ�" + reply;
                            TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.sendLog4Log("TradeSynch.noc2cSynch", Msg);
                        }
                    }
                }
                else
                {
                    Msg = "���ýӿ�syn_update4KZ_serviceδ�ܳɹ�" + inmsg1;
                    TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.sendLog4Log("TradeSynch.noc2cSynch", Msg);
                }
            }
            catch
            { }
            finally
            {
                //da.Dispose();
            }
        }


        public SynRecordClass(string transid, string createdate, int flag)
        {
            if (flag == 0)
            {
                fstrSql = "select * from cft_syn_record.t_syn_record where Ftransaction_id='" + transid + "'";
                fstrSql_count = "select count(*) from cft_syn_record.t_syn_record where Ftransaction_id='" + transid + "'";
            }
            else
            {
                string tablename = "cft_syn_record_history.t_syn_record_" + DateTime.Parse(createdate).ToString("yyyyMM");
                fstrSql = "select * from " + tablename + " where Ftransaction_id='" + transid + "'";
                fstrSql_count = "select count(*) from cft_syn_record.t_syn_record where Ftransaction_id='" + transid + "'";
            }
        }

        public SynRecordClass(string transid)
        {
            fstrSql = "select *,0 as flag from cft_syn_record.t_syn_record where Ftransaction_id='" + transid + "'";
            fstrSql_count = "select 10000";
        }

        //ͬ��֧��״̬
        public static bool SynPayState(string transid, string createtime,out string strMsg)
        {

            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("SYNINFO"));
            //MySqlAccess dayw = new MySqlAccess(PublicRes.GetConnString("YW"));
            strMsg = "��ʼ";
            try
            {
                //da.OpenConn();
                //dayw.OpenConn();

                long timestamp = long.Parse(TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                /*
                string strSql="select Fpay_time from "+ PublicRes.GetTName("t_tran_list",transid) +" where flistid='"+transid+"' and Fstate=2";

                string pay_time=dayw.GetOneResult(strSql);
                */

                string msg = "";
                string strSql = "listid=" + transid + "&state=2"; //������������state����
                string pay_time = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_ORDER, "Fpay_time", out msg);
                strMsg += string.Format("strSql={0} pay_time={1}", strSql, pay_time);
                if (pay_time == null || pay_time == "")
                {
                    strMsg += string.Format("����ԭ��{0}",msg);
                    return false;
                }

                string pay_type = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_ORDER, "Fpay_type", out msg);
                /*
                string strUpdate  = "update cft_syn_record.t_syn_record set Fsyn_count= 0" 
                    + ",Fsyn_status=1,Flast_modify_time="+timestamp+",fpay_status=2,Fpay_time='"+pay_time+"' where Ftransaction_id='" + transid + "' and Fpay_status=1";

								
                return da.ExecSql(strUpdate);
                */

                string inmsg1 = "&transaction_id=" + transid;
                inmsg1 += "&update_type=1";
                inmsg1 += "&pay_type=" + pay_type;
                //inmsg1 += "&call_source=KZ";	
                inmsg1 += "&pay_status=2";
                inmsg1 += "&pay_time=" + TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(pay_time);
                inmsg1 += "&modify_time=" + timestamp;
                
                strMsg += string.Format("inmsg1={0}", inmsg1);
                string reply = "";
                short result;

                string Msg = "";

                if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("syn_update4KZ_service", inmsg1, true, out reply, out result, out msg))
                {
                    if (result != 0)
                    {
                        Msg = "���ýӿ�syn_update4KZ_serviceδ�ܳɹ� result=" + result + "��msg=" + msg;

                        strMsg += string.Format("Msg={0}", Msg);
                        TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.sendLog4Log("TradeSynch.noc2cSynch", Msg);
                        return false;
                    }
                    else
                    {
                        if (reply.IndexOf("result=0") > -1)
                        {
                            return true;
                        }
                        else
                        {
                            Msg = "���ýӿ�syn_update4KZ_serviceδ�ܳɹ�" + reply;
                            strMsg += string.Format("Msg={0}", Msg);
                            TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.sendLog4Log("TradeSynch.noc2cSynch", Msg);
                            return false;
                        }
                    }
                }
                else
                {
                    Msg = "���ýӿ�syn_update4KZ_serviceδ�ܳɹ�" + inmsg1;
                    strMsg += string.Format("Msg={0}", Msg);
                    TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.sendLog4Log("TradeSynch.noc2cSynch", Msg);
                    return false;
                }
            }
            catch (Exception ex)
            {
                strMsg += string.Format("Exception ex={0}", ex.Message);
                throw new Exception(ex.Message);
                return false;
            }
            finally
            {
                //da.Dispose();
                //dayw.Dispose();
            }
        }

        /*
        public SynRecordClass(string transid, string createdate, int flag)
        {
            if(flag == 0)
            {
                fstrSql = "transaction_id=" + transid + "&create_time=" + createdate;
                ICESQL =fstrSql;
            }
            else
            {
                /*
                string tablename = "cft_syn_record_history.t_syn_record_" + DateTime.Parse(createdate).ToString("yyyyMM");
                fstrSql = "select * from " + tablename + " where Ftransaction_id='" + transid + "'";
                fstrSql_count = "select count(*) from cft_syn_record.t_syn_record where Ftransaction_id='" + transid + "'";
				
                fstrSql = "transaction_id=" + transid + "&create_time=" + createdate;
                ICESQL =fstrSql;
            }
        }
        */



        public SynRecordClass(string transid, string begintime, string endtime, int paystatus,
            int synstatus, int syntype, int paytype, string spid, string spbillno, string purchaser, string bargainor, int flag, int synresult)
        {
            if (transid.Trim() == "" && spid.Trim() == "")
            {
                throw new LogicException("�̻�ID�ͽ��׵�ID����������һ��.");
            }

            string strGroup = "";
            string strWhere = "";

            string ibegin = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(begintime);
            string iend = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(endtime);

            strWhere = "create_time=" + GetSynTableNameFromDate(DateTime.Parse(endtime));
            strWhere += "&start_time=" + ibegin + "&end_time=" + iend;


            if (spid.Trim() != "")
            {
                strWhere += "&sp_id=" + spid;
            }

            if (paystatus != 9)
            {
                strWhere += "&pay_status=" + paystatus;
            }

            if (synstatus != 9)
            {
                strWhere += "&syn_status=" + synstatus;
            }

            if (syntype != 9)
            {
                strWhere += "&syn_type=" + syntype;
            }

            if (paytype != 9)
            {
                strWhere += "&pay_type=" + paytype;
            }

            if (synresult != 9)
            {
                strWhere += "&syn_result=" + synresult;
            }

            if (spbillno.Trim() != "")
            {
                strWhere += "&sp_billno=" + spbillno;
            }

            if (purchaser.Trim() != "")
            {
                strWhere += "&purchaser_uin=" + purchaser;
            }

            if (bargainor.Trim() != "")
            {
                strWhere += "&bargainor_uin=" + bargainor;
            }

            if (transid.Trim() != "")
            {
                //furion 20090514 ���ָ�����׵�������ʹ��ʱ������
                strWhere = "create_time=" + GetSynTableNameFromDate(DateTime.Parse(endtime));
                strWhere += "&transaction_id=" + transid;
            }

            ICESQL = strWhere;

            /*
            if(flag == 1)
            {
                DateTime tmpDate = DateTime.Parse(begintime);

                while(tmpDate <= DateTime.Parse(endtime))
                {
                    string TableName = "cft_syn_record_history.t_syn_record_" + tmpDate.ToString("yyyyMM");

                    strGroup = strGroup + " select *,1 as flag from " + TableName + strWhere + " union all";

                    tmpDate = tmpDate.AddMonths(1);

                    string strTmp = tmpDate.ToString("yyyy-MM-");
                    tmpDate = DateTime.Parse(strTmp + "01 00:00:00");
                }
            }
			
            string TableName1 = "cft_syn_record.t_syn_record";
            strGroup = strGroup + " select *,0 as flag from " + TableName1 + strWhere + " ";


            fstrSql = strGroup;
            fstrSql_count  = "select 10000";//" select count(*) from ( " + strGroup + ")  a ";
            */
        }
    }

    #endregion


    #region ������ѯ��

    public class OrderQueryClass : Query_BaseForNET
    {
        public OrderQueryClass(string listid)
        {
            fstrSql = "select * from " + PublicRes.GetTName("t_order", listid) + " where Flistid='" + listid + "'";
            fstrSql_count = "select count(*) from " + PublicRes.GetTName("t_order", listid) + " where Flistid='" + listid + "'";
        }

        public OrderQueryClass(DateTime u_BeginTime, DateTime u_EndTime, string buyqq, string saleqq,
            string u_QueryType, string queryvalue, int fstate)
        {
            if (u_QueryType != "FlistID" || queryvalue.Trim() == "")
            {
                if (buyqq.Trim() == "" && saleqq.Trim() == "")
                {
                    throw new Exception("����ʺ�,�����ʺ�,���׵�ID��������һ��");
                }

                string tablename = "";
                string strWhere = " where FCreate_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                if (buyqq.Trim() != "")
                {
                    string buyid = PublicRes.ConvertToFuid(buyqq);

                    tablename = PublicRes.GetTName("t_user_order", buyid);
                    strWhere += " and FBuy_uid=" + buyid;
                }

                if (saleqq.Trim() != "")
                {
                    string saleid = PublicRes.ConvertToFuid(saleqq);

                    tablename = PublicRes.GetTName("t_user_order", saleid);
                    strWhere += " and FSale_uid=" + saleid;
                }

                if (queryvalue.Trim() != "")
                    strWhere += " and " + u_QueryType + "='" + queryvalue + "' ";

                if (fstate != 99)
                    strWhere += " and FTrade_State=" + fstate;

                fstrSql = "select * from " + tablename + strWhere;
                fstrSql_count = "select count(*) from " + tablename + strWhere;
            }
            else
            {
                fstrSql = "select * from " + PublicRes.GetTName("t_order", queryvalue) + " where Flistid='" + queryvalue + "'";
                fstrSql_count = "select count(*) from " + PublicRes.GetTName("t_order", queryvalue) + " where Flistid='" + queryvalue + "'";
            }
        }
    }

    #endregion
    #region ������ѯ��

    public class OrderQueryClassZJ : Query_BaseForNET
    {
        public string ICESQL = "";

        public OrderQueryClassZJ(string listid)
        {
            ICESQL = "listid=" + listid;
            fstrSql = "select * from " + PublicRes.GetTName("t_order", listid) + " where Flistid='" + listid + "'";
            fstrSql_count = "select count(*) from " + PublicRes.GetTName("t_order", listid) + " where Flistid='" + listid + "'";
        }

        public OrderQueryClassZJ(DateTime u_BeginTime, DateTime u_EndTime, string buyqq, string saleqq,string buyqqInnerID, string saleqqInnerID,string u_QueryType, string queryvalue, int fstate, int fcurtype)
        {
            if (u_QueryType != "FlistID" || queryvalue.Trim() == "")
            {
                if (buyqq.Trim() == "" && saleqq.Trim() == "" && buyqqInnerID == "" && saleqqInnerID=="")
                {
                    throw new Exception("����ʺ�,�����ʺ�,����ʺ��ڲ�ID,�����ʺ��ڲ�ID�����׵�ID��������һ��");
                }

                string tablename = "";
                string strWhere = " where FCreate_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' and Fcurtype=" + fcurtype + " ";

                if (buyqqInnerID.Trim() != "")
                {
                    tablename = PublicRes.GetTName("t_user_order", buyqqInnerID);
                    strWhere += " and FBuy_uid=" + buyqqInnerID;
                }

                if (saleqqInnerID.Trim() != "")
                {
                    tablename = PublicRes.GetTName("t_user_order", saleqqInnerID);
                    strWhere += " and FSale_uid=" + saleqqInnerID;
                }

                if (buyqq.Trim() != "")
                {
                    string buyid = PublicRes.ConvertToFuid(buyqq);

                    if (buyid == null || buyid == "" || buyid == "0")
                    {
                        throw new LogicException("���δע��");
                    }

                    tablename = PublicRes.GetTName("t_user_order", buyid);
                    strWhere += " and FBuy_uid=" + buyid;
                }

                if (saleqq.Trim() != "")
                {
                    string saleid = PublicRes.ConvertToFuid(saleqq);

                    if (saleid == null || saleid == "" || saleid == "0")
                    {
                        throw new LogicException("����δע��");
                    }

                    tablename = PublicRes.GetTName("t_user_order", saleid);
                    strWhere += " and FSale_uid=" + saleid;
                }

                if (queryvalue.Trim() != "")
                    strWhere += " and " + u_QueryType + "='" + queryvalue + "' ";

                if (fstate != 99)
                    strWhere += " and FTrade_State=" + fstate;

                fstrSql = "select * from " + tablename + strWhere;
                fstrSql_count = "select 10000";//"select count(*) from " + tablename + strWhere;					
            }
            else
            {
                ICESQL = "listid=" + queryvalue;
                fstrSql = "select * from " + PublicRes.GetTName("t_order", queryvalue) + " where Flistid='" + queryvalue + "' and Fcurtype=" + fcurtype + "";
                fstrSql_count = "select count(*) from " + PublicRes.GetTName("t_order", queryvalue) + " where Flistid='" + queryvalue + "' and Fcurtype=" + fcurtype + "";
            }
        }
    }

    #endregion

    #region ����ʵʱ��ѯ��
    public class RealTimeOrderQueryClass : Query_BaseForNET
    {

        public RealTimeOrderQueryClass(string u_ID, DateTime u_BeginTime, DateTime u_EndTime, int fstate, float fnum, string banktype, string sorttype)
        {
            string strGroup = "";
            string strWhere = "";

            if (u_ID != null && u_ID.Trim() != "")
            {
                strWhere += " where Freq_orderno='" + u_ID + "' ";
            }

            if (fstate != 9)
            {
                if (strWhere == "")
                {
                    strWhere = " where FSign=" + fstate.ToString() + " ";
                }
                else
                    strWhere += " and FSign=" + fstate.ToString() + " ";
            }

            if (banktype != "0000")
            {
                if (strWhere == "")
                {
                    strWhere = " where Fbank_type=" + banktype.ToString() + " ";
                }
                else
                    strWhere += " and Fbank_type=" + banktype.ToString() + " ";
            }

            long num = (long)Math.Round(fnum * 100, 0);

            if (strWhere == "")
            {
                strWhere = " where Famount>=" + num.ToString() + " ";
            }
            else
                strWhere += " and Famount>=" + num.ToString() + " ";

            string test = u_BeginTime.ToString("yyyy-MM-dd");
            if (test != "1940-01-01")
            {
                strWhere += " and Fbanktran_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                DateTime tmpDate = u_BeginTime.AddDays(-1);

                while (tmpDate <= u_EndTime.AddDays(1))
                {
                    string TableName = "cft_bankorder_" + tmpDate.ToString("yyyyMM") + " .t_bankorder_info_" + tmpDate.ToString("dd");

                    strGroup = strGroup + " select * from " + TableName + strWhere + " union all";

                    tmpDate = tmpDate.AddDays(1);

                    string strTmp = tmpDate.ToString("yyyy-MM-dd");
                    tmpDate = DateTime.Parse(strTmp + " 00:00:01");
                }

                strGroup = strGroup.Substring(0, strGroup.Length - 10);
            }

            string strorder = "";
            if (sorttype != null && sorttype.Trim() != "")
            {
                if (sorttype.Trim() == "1")
                {
                    strorder = " order by Fcreate_time asc ";
                }
                else if (sorttype.Trim() == "2")
                {
                    strorder = " order by Fcreate_time desc ";
                }
                else if (sorttype.Trim() == "3")
                {
                    strorder = " order by FAmount asc ";
                }
                else if (sorttype.Trim() == "4")
                {
                    strorder = " order by FAmount desc ";
                }
            }

            fstrSql = strGroup + strorder;

            fstrSql_count = "select 10000";

        }


    }

    #endregion

    #region Ͷ�ߵ���ѯ��

    public class AppealQueryClass : Query_BaseForNET
    {
        public string ICESQL;
        //			select * from c2c_db_appeal.t_appeal where 1=1 [:and Fappealid='$sppealid$':] [:and Flistid='$listid$':] [:and Fstate=$state$:] [:and Fuid=$uid$:]
        //			[:and Fappeal_time between '$time_start$' and '$time_end$':] [:and Fvs_uid=$vs_uid$:];

        public AppealQueryClass(string appealid)
        {
            fstrSql = "select * from c2c_db_appeal.t_appeal where Fappealid='" + appealid + "'";
            fstrSql_count = "select count(*) from c2c_db_appeal.t_appeal where Fappealid='" + appealid + "'";

            //ICESQL = "appealid=" + appealid ;
            ICESQL = "sppealid=" + appealid;
        }

        public AppealQueryClass(DateTime u_BeginTime, DateTime u_EndTime, string buyqq, string saleqq,
            string queryvalue, int fstate)
        {

            string tablename = "c2c_db_appeal.t_appeal";
            string strWhere = " where Fappeal_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

            ICESQL = "time_start=" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
            ICESQL += "&time_end=" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss");

            if (buyqq.Trim() != "")
            {
                string buyid = PublicRes.ConvertToFuid(buyqq);

                strWhere += " and Fuid=" + buyid;
                ICESQL += "&uid=" + buyid;
            }

            if (saleqq.Trim() != "")
            {
                string saleid = PublicRes.ConvertToFuid(saleqq);

                strWhere += " and Fvs_uid=" + saleid;
                ICESQL += "&vs_uid=" + saleid;
            }

            if (queryvalue.Trim() != "")
            {
                strWhere += " and Flistid='" + queryvalue + "' ";
                ICESQL += "&listid=" + queryvalue;
            }

            if (fstate != 99)
            {
                strWhere += " and FState=" + fstate;
                ICESQL += "&state=" + fstate;
            }

            fstrSql = "select * from " + tablename + strWhere;
            fstrSql_count = "select 10000";//"select count(*) from " + tablename + strWhere;	
        }
    }

    #endregion

    #region ���ֲ�ѯ��
    public class PickQueryClass : Query_BaseForNET
    {
        public PickQueryClass(string u_ID)
        {

            //**furion���ֵ�����20120216
            fstrSql = "select " + GetTcPayListOldFields() + " from c2c_db.t_tcpay_list where  Flistid='" + u_ID + "'";

            string currtable = "";
            string othertable = "";
            GetPayListTableFromID(u_ID, out currtable, out othertable);

            fstrSql += " union all select " + GetTcPayListNewFields() + " from " + currtable + " where  Flistid='" + u_ID + "'";
            fstrSql += " union all select " + GetTcPayListNewFields() + " from " + othertable + " where  Flistid='" + u_ID + "'";

            //fstrSql_count = "select count(*) from c2c_db.t_tcpay_list where Flistid='" + u_ID + "'";
            fstrSql_count = "select 1";
        }

        // 2012/5/29 ����Ƿ�������п�����
        public PickQueryClass(string u_ID, DateTime u_BeginTime, DateTime u_EndTime, int fstate, float fnum, string banktype, string sorttype, int idtype, string cashtype, bool isSecret)
        {

            string strGroup = "";
            string strWhere = "";

            if (u_ID != null && u_ID.Trim() != "")
            {
                if (idtype == 0)
                {
                    //furion 20051101 �Ժ��ѯȫ���ڲ�ID��ʼ.
                    string uid = PublicRes.ConvertToFuid(u_ID);

                    //strWhere += " where faid='" + u_ID + "' and Fsubject=14 ";
                    //strWhere += " where Fuid=" + uid + " and Fsubject=14 ";
                    strWhere += " where Fuid=" + uid + " ";
                }
                else if (idtype == 1)
                {
                    if (isSecret)
                    {
                        string bankID = BankLib.BankIOX.GetCreditEncode(u_ID, BankLib.BankIOX.fxykconn);
                        strWhere += " where Fabankid='" + bankID + "' ";
                    }
                    else
                    {
                        strWhere += " where Fabankid='" + u_ID + "' ";
                    }
                }
                else if (idtype == 2)
                {
                    strWhere += "  where Flistid='" + u_ID + "' ";
                }
            }

            if (fstate != 0)
            {
                if (strWhere != "")
                {
                    strWhere += " and FSign=" + fstate.ToString() + " ";
                }
                else
                {
                    strWhere += " where FSign=" + fstate.ToString() + " ";
                }
            }

            long num = (long)Math.Round(fnum * 100, 0);
            if (strWhere != "")
            {
                strWhere += " and Fnum>=" + num.ToString() + " ";
            }
            else
            {
                strWhere += " where Fnum>=" + num.ToString() + " ";
            }

            if (banktype != "0000")
            {
                strWhere += " and Fabank_type=" + banktype + " ";
            }
            if (cashtype != "0000")
            {
                strWhere += " and Fbankid=" + cashtype + " ";
            }

            string test = u_BeginTime.ToString("yyyy-MM-dd");
            if (test != "1940-01-01")
            {
                strWhere += " and Fpay_front_time_acc between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                //��Ч�ʿ���,ֻȡ��ʼʱ��ͽ���ʱ�������µ���ʷ��¼,������ȡ.
                //DateTime tmpDate = u_BeginTime.AddMonths(-1);
                DateTime tmpDate = u_BeginTime;
                //while(tmpDate <= u_EndTime.AddMonths(1))
                while (tmpDate <= u_EndTime)
                {
                    //string TableName = "c2c_db_tcpay.t_tcpay_list_" + tmpDate.ToString("yyyyMM");
                    string TableName = "c2c_db.t_tcpay_list_" + tmpDate.ToString("yyyyMM");

                    strGroup = strGroup + " select " + GetTcPayListNewFields() + " from " + TableName + strWhere + " union all";

                    tmpDate = tmpDate.AddMonths(1);

                    string strTmp = tmpDate.ToString("yyyy-MM-");
                    tmpDate = DateTime.Parse(strTmp + "01 00:00:01");
                }

            }
            //**furion���ֵ�����20120216
            string TableName1 = "c2c_db.t_tcpay_list";
            strGroup = strGroup + " select " + GetTcPayListOldFields() + " from " + TableName1 + strWhere + " ";

            string strorder = "";
            if (sorttype != null && sorttype.Trim() != "")
            {
                if (sorttype.Trim() == "1")
                {
                    strorder = " order by Fpay_front_time_acc asc ";
                }
                else if (sorttype.Trim() == "2")
                {
                    strorder = " order by Fpay_front_time_acc desc ";
                }
                else if (sorttype.Trim() == "3")
                {
                    strorder = " order by Fnum asc ";
                }
                else if (sorttype.Trim() == "4")
                {
                    strorder = " order by Fnum desc ";
                }
            }

            fstrSql = strGroup + strorder;
            fstrSql_count = "select 10000";//" select count(*) from ( " + strGroup + ") a ";

        }

        public PickQueryClass(string listid, DateTime u_BeginTime, DateTime u_EndTime, bool oldflag)
        {
            string strGroup = "";
            string strWhere = "";

            string test = u_BeginTime.ToString("yyyy-MM-dd");

            if (listid != null && listid.Trim() != "")
            {
                strWhere += " where Flistid='" + listid + "' ";

                string currtable = "";
                string othertable = "";
                GetPayListTableFromID(listid, out currtable, out othertable);

                strGroup += "  select " + GetTcPayListNewFields() + " from " + currtable + " where  Flistid='" + listid + "' union all ";
                strGroup += "  select " + GetTcPayListNewFields() + " from " + othertable + " where Flistid='" + listid + "' union all ";
                test = "1940-01-01"; //����ʹ�������ѭ���幹��strgroup
            }
            else
            {
                //strWhere += " where Fsubject=14 ";
            }

            if (test != "1940-01-01")
            {
                if (strWhere == "")
                {
                    strWhere = " where Fpay_front_time_acc between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }
                else
                {
                    strWhere += " and Fpay_front_time_acc between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }

                //��Ч�ʿ���,ֻȡ��ʼʱ��ͽ���ʱ�������µ���ʷ��¼,������ȡ.
                //DateTime tmpDate = u_BeginTime.AddMonths(-1);
                DateTime tmpDate = u_BeginTime;
                //while(tmpDate <= u_EndTime.AddMonths(1))
                while (tmpDate <= u_EndTime)
                {
                    //string TableName = "c2c_db_tcpay.t_tcpay_list_" + tmpDate.ToString("yyyyMM");
                    string TableName = "c2c_db.t_tcpay_list_" + tmpDate.ToString("yyyyMM");

                    strGroup = strGroup + " select " + GetTcPayListNewFields() + " from " + TableName + strWhere + " union all";

                    tmpDate = tmpDate.AddMonths(1);

                    string strTmp = tmpDate.ToString("yyyy-MM-");
                    tmpDate = DateTime.Parse(strTmp + "01 00:00:01");
                }

            }
            //**furion���ֵ�����20120216
            string TableName1 = "c2c_db.t_tcpay_list";
            strGroup = strGroup + " select " + GetTcPayListOldFields() + " from " + TableName1 + strWhere + " ";


            fstrSql = strGroup;
            fstrSql_count = "select 10000";//" select count(*) from ( " + strGroup + ")  a ";
        }

        public static string GetTcPayListNewFields()
        {
            return " Ftde_id,Flistid,Fdraw_id,Fsp_batch,Fsp_serial,Fsp_operator,Fspid,Fbankid,Fproduct,Fbusiness_type,Ftype,Fsubject,Fnum,"
                + "Fcharge,Fcharge_payer,Fcharge_recv_uid,Fstate,Fsign,Fbank_list,Fbank_acc,Fbank_type,Fcurtype,Fuid,Faid,Faname,Fabank_type,"
                + "Fabankid,Fprove,Fip,Fmemo,Fbank_memo,Fresult,Frefund_ticket_flag,Frefund_ticket_list,Fpay_front_time,Fpay_front_time_acc,"
                + "Fpay_time,Fpay_time_acc,Fmodify_time,Fbank_name,Farea,Fcity,Facc_name,Fuser_type,Fstandby1,Fstandby2,Fstandby3,Fstandby4,Fstandby5,Fstandby6 ";
        }

        public static string GetTcPayListOldFields()
        {

            return " Ftde_id,Flistid,'' as Fdraw_id,'' as Fsp_batch,'' as Fsp_serial,'' as Fsp_operator,Fspid,Fbankid,0 as Fproduct,0 as Fbusiness_type,Ftype,Fsubject,Fnum,"
                + "0 as Fcharge,0 as Fcharge_payer,0 as Fcharge_recv_uid,Fstate,Fsign,Fbank_list,Fbank_acc,Fbank_type,Fcurtype,Fuid,Faid,Faname,Fabank_type,"
                + "Fabankid,Fprove,Fip,Fmemo,'' as Fbank_mem,'' as Fresult,1 as Frefund_ticket_flag,'' as Frefund_ticket_list,Fpay_front_time,Fpay_front_time_acc,"
                + "Fpay_time,Fpay_time_acc,Fmodify_time,Fbank_name,Farea,Fcity,Facc_name,Fuser_type,0 as Fstandby1,0 as Fstandby2,'' as Fstandby3,'' as Fstandby4,'' as Fstandby5,'' as Fstandby6 ";
        }

        public static void GetPayListTableFromTime(DateTime datetime, out string currtable, out string othertable)
        {
            currtable = "c2c_db.t_tcpay_list_" + datetime.ToString("yyyyMM");

            if (datetime.Day > 15)
            {
                othertable = "c2c_db.t_tcpay_list_" + datetime.AddMonths(1).ToString("yyyyMM");
            }
            else
            {
                othertable = "c2c_db.t_tcpay_list_" + datetime.AddMonths(-1).ToString("yyyyMM");
            }
        }

        public static void GetPayListTableFromID(string listid, out string currtable, out string othertable)
        {
            //1��3λϵͳID+YYYYMMDD+10λ��ˮ�ţ�
            //2��3λϵͳID+10λ�̻���+YYYYMMDD+7λ���к�

            string strdate = "";
            if (listid.Length == 21)
            {
                strdate = listid.Substring(3, 4) + "-" + listid.Substring(7, 2) + "-" + listid.Substring(9, 2);
            }
            else if (listid.Length == 28)
            {
                strdate = listid.Substring(13, 4) + "-" + listid.Substring(17, 2) + "-" + listid.Substring(19, 2);
            }

            DateTime dt = DateTime.Now;
            try
            {
                dt = DateTime.Parse(strdate);
            }
            catch
            {
                dt = DateTime.Now;
            }

            GetPayListTableFromTime(dt, out currtable, out othertable);
        }
    }

    #endregion

    #region Ͷ���̻���ѯ��
    public class ComplainBussClass : Query_BaseForNET
    {
        public ComplainBussClass()
        {
           
        }

        public ComplainBussClass(string bussid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try
            {
                da.OpenConn();
                string strSql = "  select " + GetComplainBussListFields() + " from  c2c_fmdb.t_complain_buss_list where Fbuss_id='" + bussid+"'";
                DataTable dt = da.GetTable(strSql);
                if (dt == null || dt.Rows.Count != 1)
                {
                    throw new LogicException("��ȡ��������");
                }
                FBussId = bussid;
                object obj = dt.Rows[0]["Fbuss_name"];
                if (obj != null) FBussName = obj.ToString().Trim();
                obj = dt.Rows[0]["Fbuss_email"];
                if (obj != null) FBussEmail = obj.ToString().Trim();
                obj = dt.Rows[0]["Fcreate_time"];
                if (obj != null) FCreateTime = obj.ToString().Trim();
                obj = dt.Rows[0]["Fmodify_time"];
                if (obj != null) FModifyTime = obj.ToString().Trim();
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally 
            {
                da.Dispose();
            }
        }

        public ComplainBussClass(string bussid, DateTime u_BeginTime, DateTime u_EndTime)
        {
            string strGroup = "  select " + GetComplainBussListFields() + " from  c2c_fmdb.t_complain_buss_list where 1=1 ";
            string strWhere = "";

            if (bussid != null && bussid.Trim() != "")
            {
                strWhere += " and Fbuss_id='" + bussid + "'";
            }
            else {
                strWhere += " and (Fcreate_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' )";
            }

            fstrSql = strGroup + strWhere;
            fstrSql_count = "select count(1) from c2c_fmdb.t_complain_buss_list where 1=1 " + strWhere;
        }

        public string FBussId = "";
        public string FBussName = "";
        public string FBussEmail = "";
        public string FCreateTime = "";
        public string FModifyTime = "";

        public bool addComplainBuss(out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try 
            {
                da.OpenConn();
                string strSql = "select " + GetComplainBussListFields() + " from c2c_fmdb.t_complain_buss_list where Fbuss_id='" + FBussId+"'";
                DataTable dt = da.GetTable(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    msg = "���ʧ��:�̻������Ѵ���" + FBussId;
                    return false;
                }

                strSql = "insert into c2c_fmdb.t_complain_buss_list(Fbuss_id,Fbuss_name,Fbuss_email,Fcreate_time,Fmodify_time) values('{0}','{1}','{2}',now(),now())";
                strSql = String.Format(strSql, FBussId, FBussName, FBussEmail);

                if (da.ExecSqlNum(strSql) == 1)
                {
                    return true;
                }
                else {
                    msg = "���ʧ��";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public bool changeComplainBuss(out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try
            {
                da.OpenConn();
                string strSql = "select " + GetComplainBussListFields() + " from c2c_fmdb.t_complain_buss_list where Fbuss_id='" + FBussId+"'";
                DataTable dt = da.GetTable(strSql);
                if (dt != null && dt.Rows.Count < 0)
                {
                    msg = "�޸�ʧ��:�̻����벻����" + FBussId;
                    return false;
                }

                strSql = "update c2c_fmdb.t_complain_buss_list set Fbuss_name='{0}',Fbuss_email='{1}',Fmodify_time=now() where Fbuss_id='{2}' ";
                strSql = String.Format(strSql, FBussName, FBussEmail, FBussId);

                if (da.ExecSqlNum(strSql) == 1)
                {
                    return true;
                }
                else
                {
                    msg = "�޸�ʧ��";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public static string GetComplainBussListFields()
        {
            return " Fbuss_id,Fbuss_name,Fbuss_email,Fcreate_time,Fmodify_time ";
        }
    }
    #endregion

    #region �û�Ͷ����
    public class UserComplainClass : Query_BaseForNET
    {
        public UserComplainClass()
        {

        }
        public UserComplainClass(string bussid, string cft_orderid,int comptype, int compstatus, DateTime u_BeginTime, DateTime u_EndTime)
        {
            string strGroup = "  select " + GetComplainUserListFields() + " from  c2c_fmdb.t_complain_user_list where 1=1 ";
            string strWhere = "";
            if (bussid != null && bussid.Trim() != "")
            {
                strWhere += " and Fbuss_id='" + bussid + "'";
            }
            if (cft_orderid != null && cft_orderid.Trim() != "")
            {
                strWhere += " and Forder_id='" + cft_orderid + "'";
            }
            if (comptype != null && comptype != 0) 
            {
                strWhere += " and Fcomp_type=" + comptype;
            }
            if (compstatus != null && compstatus != 0)
            {
                strWhere += " and Fstatus=" + compstatus;
            }
            if (u_BeginTime != null && u_EndTime != null)
            {
                strWhere += " and (Fcreate_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' )";
            }

            fstrSql = strGroup + strWhere;
            fstrSql_count = "select count(1) from c2c_fmdb.t_complain_user_list where 1=1 " +strWhere;
        }

        public UserComplainClass(int listid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try
            {
                da.OpenConn();
                string strSql = "  select " + GetComplainUserListFields() + " from  c2c_fmdb.t_complain_user_list where Flistid=" + listid;
                DataTable dt = da.GetTable(strSql);
                if (dt == null || dt.Rows.Count != 1)
                {
                    throw new LogicException("��ȡ��������");
                }
                FListId = listid;
                object obj = dt.Rows[0]["Fbuss_id"];
                if (obj != null) FBussId = obj.ToString().Trim();
                obj = dt.Rows[0]["Fbuss_name"];
                if (obj != null) FBussName = obj.ToString().Trim();
                obj = dt.Rows[0]["Forder_id"];
                if (obj != null) FCftOrderId = obj.ToString().Trim();
                obj = dt.Rows[0]["Forder_fee"];
                if (obj != null) FOrderFee = obj.ToString().Trim();
                obj = dt.Rows[0]["Fcomp_type"];
                if (obj != null) FCompType = Convert.ToInt16(obj.ToString().Trim());
                obj = dt.Rows[0]["Fstatus"];
                if (obj != null) FStatus = Convert.ToInt16(obj.ToString().Trim());
                obj = dt.Rows[0]["Fcontact"];
                if (obj != null) FContact = obj.ToString().Trim();
                obj = dt.Rows[0]["Freply_type"];
                if (obj != null) FReplyType = Convert.ToInt16(obj.ToString().Trim());
                obj = dt.Rows[0]["Fnotice_time"];
                if (obj != null) FNoticeTime = obj.ToString().Trim();
                obj = dt.Rows[0]["Fremind_time"];
                if (obj != null) FRemindTime = obj.ToString().Trim();
                obj = dt.Rows[0]["Fcreate_time"];
                if (obj != null) FCreateTime = obj.ToString().Trim();
                obj = dt.Rows[0]["Fmodify_time"];
                if (obj != null) FModifyTime = obj.ToString().Trim();
                obj = dt.Rows[0]["Fbuss_order_id"];
                if (obj != null) FBussOrderId = obj.ToString().Trim();
                obj = dt.Rows[0]["Fmemo"];
                if (obj != null) FMemo = obj.ToString().Trim();
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }


        public int FListId = 0;
        public string FBussId = "";
        public string FBussName = "";
        public string FCftOrderId = "";
        public string FOrderFee = "";
        public int FCompType = 1;
        public int FStatus = 1;
        public int FReplyType = 1;
        public string FContact = "";
        public string FNoticeTime = "";
        public string FRemindTime = "";
        public string FCreateTime = "";
        public string FModifyTime = "";
        //add by yinhuang 2013/8/9
        public string FBussOrderId = "";
        public string FMemo = "";

        public string addUserComplain(out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try
            {
                da.OpenConn();
                string strSql = "select " + GetComplainUserListFields() + " from c2c_fmdb.t_complain_user_list where Forder_id='" + FCftOrderId + "'";
                DataTable dt = da.GetTable(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    msg = "���ʧ��:�Ƹ�ͨ�������Ѵ���" + FCftOrderId;
                    return "";
                }
                strSql = "select Fbuss_name from c2c_fmdb.t_complain_buss_list where Fbuss_id='" + FBussId + "'";
                dt = da.GetTable(strSql);
                if (dt == null || dt.Rows.Count != 1)
                {
                    msg = "���ʧ��:�̻����벻����" + FBussId;
                    return "";
                }
                FBussName = dt.Rows[0]["Fbuss_name"].ToString().Trim();

                string striceWhere = "listid=" + FCftOrderId;
                string QUERY_ORDER = "query_order_service";
                DataTable dt_ice = CommQuery.GetTableFromICE(striceWhere, QUERY_ORDER, out msg);
                if (dt_ice != null && dt_ice.Rows.Count > 0)
                {
                    object obj = dt_ice.Rows[0]["Fpaynum"];
                    if (obj != null) FOrderFee = obj.ToString().Trim();
                }
                else 
                {
                    return "";
                }

                strSql = "insert into c2c_fmdb.t_complain_user_list(Fbuss_id,Fbuss_name,Forder_id,Forder_fee,Fcomp_type,Fstatus,Fcontact,Freply_type,Fbuss_order_id,Fmemo,Fnotice_time,Fcreate_time,Fmodify_time) values('{0}','{1}','{2}',{3},{4},{5},'{6}',{7},'{8}','{9}',now(),now(),now())";
                strSql = String.Format(strSql, FBussId, FBussName, FCftOrderId, FOrderFee, FCompType, FStatus, FContact, FReplyType, FBussOrderId, FMemo);

                if (da.ExecSqlNum(strSql) == 1)
                {
                    strSql = "select Flistid from c2c_fmdb.t_complain_user_list where Forder_id='" + FCftOrderId + "'";
                    dt = da.GetTable(strSql);
                    if (dt != null && dt.Rows.Count > 0) {
                        return dt.Rows[0]["Flistid"].ToString().Trim();
                    }
                    return "";
                }
                else
                {
                    msg = "���ʧ��";
                    return "";
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return "";
            }
            finally
            {
                da.Dispose();
            }
        }

        public bool changeUserComplain(out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try
            {
                da.OpenConn();
                string strSql = "select " + GetComplainUserListFields() + " from c2c_fmdb.t_complain_user_list where Flistid="+FListId;
                DataTable dt = da.GetTable(strSql);
                if (dt != null && dt.Rows.Count < 0)
                {
                    msg = "�޸�ʧ��:ID������" + FListId;
                    return false;
                }
                strSql = "select Fbuss_name from c2c_fmdb.t_complain_buss_list where Fbuss_id='" + FBussId + "'";
                dt = da.GetTable(strSql);
                if (dt == null || dt.Rows.Count != 1)
                {
                    msg = "�޸�ʧ��:�̻����벻����" + FBussId;
                    return false;
                }
                FBussName = dt.Rows[0]["Fbuss_name"].ToString().Trim();

                string striceWhere = "listid=" + FCftOrderId;
                string QUERY_ORDER = "query_order_service";
                DataTable dt_ice = CommQuery.GetTableFromICE(striceWhere, QUERY_ORDER, out msg);
                if (dt_ice != null && dt_ice.Rows.Count > 0)
                {
                    object obj = dt_ice.Rows[0]["Fpaynum"];
                    if (obj != null) FOrderFee = obj.ToString().Trim();
                }
                else
                {
                    return false;
                }

                strSql = "update c2c_fmdb.t_complain_user_list set Fbuss_id='{0}',Fbuss_name='{1}',Forder_id='{2}',Forder_fee='{3}',Fcomp_type={4},Fstatus={5},Fcontact='{6}',Freply_type={7},Fbuss_order_id='{8}',Fmemo='{9}',Fmodify_time=now() where Flistid='{10}' ";
                strSql = String.Format(strSql, FBussId, FBussName, FCftOrderId, FOrderFee, FCompType, FStatus, FContact, FReplyType,FBussOrderId, FMemo, FListId);

                if (da.ExecSqlNum(strSql) == 1)
                {
                    return true;
                }
                else
                {
                    msg = "�޸�ʧ��";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public bool remindUserComplain(out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try
            {
                da.OpenConn();
                string strSql = "select " + GetComplainUserListFields() + " from c2c_fmdb.t_complain_user_list where Flistid=" + FListId;
                DataTable dt = da.GetTable(strSql);
                if (dt != null && dt.Rows.Count < 0)
                {
                    msg = "�߰�ʧ��:ID������" + FListId;
                    return false;
                }

                strSql = "update c2c_fmdb.t_complain_user_list set Fstatus=2, Fremind_time=now() where Flistid='{0}' ";
                strSql = String.Format(strSql, FListId);

                if (da.ExecSqlNum(strSql) == 1)
                {
                    return true;
                }
                else
                {
                    msg = "�߰�ʧ��";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public static string GetComplainUserListFields()
        {
            return " Flistid,Fbuss_id,Fbuss_name,Forder_id,Forder_fee,Fcomp_type,Fstatus,Fcontact,Freply_type,Fbuss_order_id,Fmemo,Fnotice_time,Fremind_time,Fcreate_time,Fmodify_time ";
        }
    }
    #endregion

    #region �˿�Ǽ���
    public class RefundInfoClass : Query_BaseForNET 
    {
        public RefundInfoClass() { }
        public RefundInfoClass(string coding, string orderId, string stime, string etime, int refundType, int refundState, string tradeState) 
        {
            string strGroup = "  select " + GeRefundInfoFields() + " from  c2c_fmdb.t_refund_info where 1=1 ";
            string strWhere = "";
            if (!string.IsNullOrEmpty(coding)) 
            {
                strWhere += " and Fcoding='" + coding + "'";
            }
            if (!string.IsNullOrEmpty(orderId))
            {
                strWhere += " and Forder_id='" + orderId + "'";
            }
            if (!string.IsNullOrEmpty(stime))
            {
                strWhere += " and Fcreate_time>='" + stime + "'";
            }
            if (!string.IsNullOrEmpty(etime))
            {
                strWhere += " and Fcreate_time<='" + etime + "'";
            }
            if (refundType != null && refundType != 0)
            {
                if (refundType == 10)
                {
                    strWhere += " and Frefund_type IN(1,2,3,4,5,10) ";
                }
                else {
                    strWhere += " and Frefund_type=" + refundType;
                }
                
            }
            if (refundState != null && refundState != 0)
            {
                //strWhere += " and Frefund_state=" + refundState;
                strWhere += " and Fsubmit_refund=" + refundState;
            }
            if (!string.IsNullOrEmpty(tradeState) && tradeState != "0") 
            {
                strWhere += " and Ftrade_state='" + tradeState + "'";
            }

            fstrSql = strGroup + strWhere;
            fstrSql_count = "select count(1) from c2c_fmdb.t_refund_info where 1=1 " + strWhere;
        }

        public RefundInfoClass(string fid) 
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try
            {
                da.OpenConn();
                string strSql = "  select " + GeRefundInfoFields() + " from  c2c_fmdb.t_refund_info where Fid=" + fid;
                DataTable dt = da.GetTable(strSql);
                if (dt == null || dt.Rows.Count != 1)
                {
                    throw new LogicException("���ݲ����ڣ�");
                }
                FId = int.Parse(fid);
                object obj = dt.Rows[0]["Forder_id"];
                if (obj != null) FOrderId = obj.ToString().Trim();
                obj = dt.Rows[0]["Fcoding"];
                if (obj != null) FCoding = obj.ToString().Trim();
                obj = dt.Rows[0]["Famount"];
                if (obj != null) FAmount = obj.ToString().Trim();
                obj = dt.Rows[0]["Ftrade_state"];
                if (obj != null) FTrade_state = Convert.ToInt16(obj.ToString().Trim());
                obj = dt.Rows[0]["Fbuy_acc"];
                if (obj != null) FBuy_acc = obj.ToString().Trim();
                obj = dt.Rows[0]["Ftrade_desc"];
                if (obj != null) FTrade_desc = obj.ToString().Trim();
                obj = dt.Rows[0]["Frefund_type"];
                if (obj != null) FRefund_type =  Convert.ToInt16(obj.ToString().Trim());
                obj = dt.Rows[0]["Frefund_state"];
                if (obj != null) FRefund_state = Convert.ToInt16(obj.ToString().Trim());
                obj = dt.Rows[0]["Fcreate_time"];
                if (obj != null) FCreateTime = obj.ToString().Trim();
                obj = dt.Rows[0]["Fmodify_time"];
                if (obj != null) FModifyTime = obj.ToString().Trim();
                obj = dt.Rows[0]["Fmemo"];
                if (obj != null) FMemo = obj.ToString().Trim();
                obj = dt.Rows[0]["Fsubmit_user"];
                if (obj != null) FSubmit_user = obj.ToString().Trim();
                obj = dt.Rows[0]["Frecycle_user"];
                if (obj != null) FRecycle_user = obj.ToString().Trim();
                obj = dt.Rows[0]["Fsam_no"];
                if (obj != null) FSam_no = obj.ToString().Trim();
                obj = dt.Rows[0]["Fsubmit_refund"];
                if (obj != null) FSubmit_refund = Convert.ToInt16(obj.ToString().Trim());
                obj = dt.Rows[0]["Frefund_amount"];
                if (obj != null) FRefund_amount = obj.ToString().Trim();
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        public int FId = 0;
        public string FOrderId = ""; //�Ƹ�ͨ������
        public string FCoding = "";
        public string FAmount = ""; //���׽��
        public int FTrade_state = 2;
        public string FBuy_acc = "";
        public string FTrade_desc = "";//����˵��
        public int FRefund_type = 1; //�˿�����
        public int FRefund_state = 10; //�˿�״̬
        public string FCreateTime = "";
        public string FModifyTime = "";
        public string FMemo = "";//��ע
        public string FSubmit_user = "";//�Ǽ���
        public string FRecycle_user = "";//������
        public string FSam_no = "";//SAM������
        public int FSubmit_refund = 3; //�ύ�˿�״̬
        public string FRefund_amount="0"; //�˿���

        public void addRefundInfo(out string msg) 
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try
            {
                da.OpenConn();
                string strSql = "select " + GeRefundInfoFields() + " from c2c_fmdb.t_refund_info where Forder_id='" + FOrderId + "'";
                DataTable dt = da.GetTable(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //msg = "���ʧ��:�Ƹ�ͨ�������Ѵ���" + FOrderId;
                    throw new LogicException("�Ƹ�ͨ�������Ѵ���:" + FOrderId);
                }

                string striceWhere = "listid=" + FOrderId;
                DataTable dt_ice = CommQuery.GetTableFromICE(striceWhere, "query_order_service", out msg);
                if (dt_ice != null && dt_ice.Rows.Count > 0)
                {
                    object obj = dt_ice.Rows[0]["Fcoding"];
                    if (obj != null) FCoding = obj.ToString().Trim();
                    obj = dt_ice.Rows[0]["Fpaynum"];
                    if (obj != null) FAmount = obj.ToString().Trim();
                    obj = dt_ice.Rows[0]["Ftrade_state"];
                    if (obj != null) { 
                        FTrade_state = int.Parse(obj.ToString().Trim());
                        if (FTrade_state == 2) {
                            FSubmit_refund = 2;
                        }
                    }
                    obj = dt_ice.Rows[0]["Fbuyid"];
                    if (obj != null) FBuy_acc = obj.ToString().Trim();
                    obj = dt_ice.Rows[0]["Fmemo"];
                    if (obj != null) FTrade_desc = obj.ToString().Trim();
                }
                else
                {
                    //msg = "ͨ�ò�ѯ�����Ų����ڣ�" + FOrderId;
                    throw new LogicException("�����Ų����ڣ�" + FOrderId + msg);
                }
                //�ж��˿���<=�������
                int oAmount = Convert.ToInt32(FAmount);
                int rAmount = 0;
                if (!string.IsNullOrEmpty(FRefund_amount)) 
                {
                    rAmount = Convert.ToInt32(FRefund_amount);
                }
                if (rAmount <= oAmount)
                {
                    //�˿���<=�������
                    strSql = "insert into c2c_fmdb.t_refund_info(Forder_id,Fcoding,Famount,Ftrade_state,Fbuy_acc,Ftrade_desc,Frefund_type,Frefund_state,Fmemo,Fsubmit_user,Frecycle_user,Fsam_no,Fsubmit_refund,Frefund_amount,Fcreate_time,Fmodify_time) values('{0}','{1}','{2}',{3},'{4}','{5}',{6},{7},'{8}','{9}','{10}','{11}',{12},{13},now(),now())";
                    strSql = String.Format(strSql, FOrderId, FCoding, FAmount, FTrade_state, FBuy_acc, FTrade_desc, FRefund_type, FRefund_state, FMemo, FSubmit_user, FRecycle_user, FSam_no, FSubmit_refund, FRefund_amount);

                    da.ExecSqlNum(strSql);
                }
                else 
                {
                    throw new LogicException("�˿���" + rAmount + "���ڶ������" + oAmount);
                }
            }
            catch (LogicException err) 
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        public void changeRefundInfo(out string msg) 
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try
            {
                da.OpenConn();
                string strSql = "select " + GeRefundInfoFields() + " from c2c_fmdb.t_refund_info where Fid="+FId+" AND Forder_id<>'" + FOrderId + "'";
                DataTable dt = da.GetTable(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    throw new LogicException("�޸�ʧ��:�Ƹ�ͨ�������Ѵ���" + FOrderId);
                }

                string striceWhere = "listid=" + FOrderId;
                string QUERY_ORDER = "query_order_service";
                DataTable dt_ice = CommQuery.GetTableFromICE(striceWhere, QUERY_ORDER, out msg);
                if (dt_ice != null && dt_ice.Rows.Count > 0)
                {
                    object obj = dt_ice.Rows[0]["Fcoding"];
                    if (obj != null) FCoding = obj.ToString().Trim();
                    obj = dt_ice.Rows[0]["Fpaynum"];
                    if (obj != null) FAmount = obj.ToString().Trim();
                    obj = dt_ice.Rows[0]["Ftrade_state"];
                    if (obj != null) FTrade_state = int.Parse(obj.ToString().Trim());
                    obj = dt_ice.Rows[0]["Fbuyid"];
                    if (obj != null) FBuy_acc = obj.ToString().Trim();
                    obj = dt_ice.Rows[0]["Fmemo"];
                    if (obj != null) FTrade_desc = obj.ToString().Trim();
                    obj = dt_ice.Rows[0]["Ftrade_state"];
                    if (obj != null)
                    {
                        FTrade_state = int.Parse(obj.ToString().Trim());
                        if (FTrade_state == 2)
                        {
                            FSubmit_refund = 2;
                        }
                    }
                }
                else
                {
                    throw new LogicException("ͨ�ò�ѯ�����Ų����ڣ�" + FOrderId);
                }

                //�ж��˿���<=�������
                int oAmount = Convert.ToInt32(FAmount);
                int rAmount = 0;
                if (!string.IsNullOrEmpty(FRefund_amount))
                {
                    rAmount = Convert.ToInt32(FRefund_amount);
                }
                if (rAmount <= oAmount)
                {
                    //�˿���<=�������
                    strSql = "update c2c_fmdb.t_refund_info set Forder_id='{0}',Fcoding='{1}',Famount='{2}',Ftrade_state={3},Fbuy_acc='{4}',Ftrade_desc='{5}',Frefund_type={6},Frefund_state={7},Fmemo='{8}',Fsubmit_user='{9}',Frecycle_user='{10}',Fsam_no='{11}',Fsubmit_refund={12},Frefund_amount={13},Fmodify_time=now() where Fid={14} ";
                    strSql = String.Format(strSql, FOrderId, FCoding, FAmount, FTrade_state, FBuy_acc, FTrade_desc, FRefund_type, FRefund_state, FMemo, FSubmit_user, FRecycle_user, FSam_no, FSubmit_refund, FRefund_amount, FId);

                    da.ExecSqlNum(strSql);
                }
                else
                {
                    throw new LogicException("�˿���" + rAmount + "���ڶ������" + oAmount);
                }
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }
        
        public static string GeRefundInfoFields()
        {
            return " Fid,Forder_id,Fcoding,Famount,Ftrade_state,Fbuy_acc,Ftrade_desc,Frefund_type,Frefund_state,Fmemo,Fcreate_time,Fmodify_time,Fsubmit_user,Frecycle_user,Fsam_no,Fsubmit_refund,Frefund_amount ";
        }
    }
    #endregion

    #region �����ʲ�ѯ��
    public class ExchangeRateQueryClass : Query_BaseForNET 
    {
        public ExchangeRateQueryClass() 
        {
        }

        public ExchangeRateQueryClass(string foreType, string issueBank, string beginTime, string endTime) 
        {
            string strGroup = "  select Fcurrency_type,Fexchg_flag,Feffect_flag,Frate_time,Fbank_type,Fcurrency_sell,Fcurrency_sell_tuned,Fmodify_time from  fcpay_db.t_rate_log where 1=1 ";
            string strWhere = "";
            if (foreType != null && foreType.Trim() != "" && foreType != "0")
            {
                strWhere += " and Fcurrency_type='" + foreType + "'";
            }
            if (issueBank != null && issueBank.Trim() != "" && issueBank != "0")
            {
                strWhere += " and Fbank_type='" + issueBank + "'";
            }
            if (beginTime != null && endTime != null)
            {
                strWhere += " and (Frate_time between '" + beginTime
                    + "' and '" + endTime + "' )";
            }

            fstrSql = strGroup + strWhere;
            fstrSql_count = "select count(1) from fcpay_db.t_rate_log where 1=1 " + strWhere;
        }
    }

    #endregion

    #region ���ٽ��ײ�ѯ��

    public class QuickTradeQueryClass : Query_BaseForNET
    {
        //��ʱ������̻���ˮ��ѯ�е��ˡ� furion 20050817
        public QuickTradeQueryClass(string listid)
        {
            string tmpspid = listid.Substring(0, 10);

            /*
            string strSql = "select Fspecial,FName from c2c_db.t_merchant_info where Fspid='" + tmpspid + "'";
            DataTable dt = PublicRes.returnDataTable(strSql,"ZL");
            */

            string errMsg = "";
            string strSql = "spid=" + tmpspid;
            DataTable dt = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, out errMsg);

            string FSaleID = "";
            string FSale_Name = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                /*
                FSaleID = dt.Rows[0][0].ToString();
                FSale_Name = dt.Rows[0][1].ToString();
                */

                FSaleID = dt.Rows[0]["Fspecial"].ToString();
                FSale_Name = dt.Rows[0]["Fname"].ToString();
            }


            //�ٵ���һ���ֶΣ���������ҳ�����ж��˿�İ�ť�Ƿ���þ����ˡ�furion 20050817
            /*
            //string tablename1 = "c2c_db.t_tcbankroll_list B ";
            //string tablename2 = PublicRes.GetTName("t_tran_list",listid) + " A ";
            string tablename2 = PublicRes.GetTName("t_order",listid) + " A ";
            //string tablename3 = "c2c_db.t_middle_user C ";

            //string strWhere = " where B.FListID=A.FListID and A.FListID='" + listid + "' and  A.Ftrade_type=3 and A.FSpID=C.FSpID";
            //string strWhere = " where  A.FListID='" + listid + "' and A.FSpID=C.FSpID";
            string strWhere = " where  A.FListID='" + listid + "' ";

            //fstrSql = "select A.FCoding,A.Flistid,A.Fbank_listid,C.Fqqid as FSaleID,C.Ftruename as FSale_Name,A.Fstate as FState1,A.Flstate,A.Fcreate_time_c2c,A.Fmemo,A.FTrade_Type " 
            fstrSql = "select A.FCoding,A.Flistid,A.Fbank_listid,'" + FSaleID + "' as FSaleID," + FSale_Name + " as FSale_Name,A.FTrade_state as FState1,A.Flstate,A.Fcreate_time_c2c,A.Fmemo,A.FTrade_Type " 
                + ",A.FPayNum as Fnum,A.Fbuyid,A.Fbuy_name,A.FSpid,A.Fbuy_uid  from " 
                //+ tablename2 + "," + tablename3 + strWhere;
                + tablename2  + strWhere;
            //fstrSql_count = "select count(*) from " + tablename2 + "," + tablename3 + strWhere;
            */

            strSql = "listid=" + listid;
            DataTable dtlst = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_ORDER, out errMsg);

            DataRow dr = dtlst.Rows[0];

            fstrSql = "select '" + QueryInfo.GetString(dr["Fcoding"]) + "' as FCoding,'"
                + listid + "' as Flistid,'"
                + QueryInfo.GetString(dr["Fbank_listid"]) + "' as Fbank_listid,'"
                + FSaleID + "' as FSaleID," + FSale_Name + " as FSale_Name,"
                + dr["FTrade_state"].ToString() + " as FState1,"
                + dr["Flstate"].ToString() + " as Flstate,'"
                + QueryInfo.GetString(dr["Fcreate_time_c2c"]) + "' as Fcreate_time_c2c,'"
                + QueryInfo.GetString(dr["Fmemo"]) + "' as Fmemo,"
                + dr["Ftrade_type"].ToString() + " as FTrade_Type,"
                + dr["fpaynum"].ToString() + " as Fnum,'"
                + dr["fbuyid"].ToString() + "' as Fbuyid,'"
                + QueryInfo.GetString(dr["Fbuy_name"]) + "' as Fbuy_name,'"
                + dr["Fspid"].ToString() + "' as FSpid,"
                + dr["Fbuy_uid"].ToString() + " as Fbuy_uid";

            fstrSql_count = "select 1";
        }

        //�Ժ��ʵļ�¼�����˿��
        public static bool Refund(string listid, Finance_Header fh)
        {
            //furion 20061201 �������˵�
            return false;

            //			if(listid == null || listid.Trim() == "") throw new LogicException("���׵�ID����Ϊ�գ�");
            //
            //			
            //
            //			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YW"));
            //			try
            //			{
            //				da.OpenConn();
            //				da.StartTran(); //�¼����������ִ�����еĺ����������ԣ�20050818 furion
            //
            //				string strSql = "select * from " + PublicRes.GetTName("t_tran_list",listid) + " where FlistID='" + listid + "' for update";
            //				DataSet ds = da.dsGetTotalData(strSql);
            //
            //				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 1)
            //				{
            //					throw new LogicException("���Ҳ���ָ���Ľ��׵���Ϣ��");
            //				}
            //
            //				DataRow dr = ds.Tables[0].Rows[0];
            //
            //				if(dr["FTrade_Type"] == null || dr["FTrade_Type"].ToString() != "2")
            //				{
            //					//ֻ��B2C�Ŀ
            //					throw new LogicException("ָ���Ľ��׵������˿");
            //				}
            //
            //				if(dr["Flstate"] == null || dr["Flstate"].ToString() != "2")
            //				{
            //					throw new LogicException("ָ���Ľ��׵���������������״̬��");
            //				}
            //
            //				if(dr["Fstate"] == null || dr["Fstate"].ToString() != "2")
            //				{
            //					throw new LogicException("ָ���Ľ��׵�״̬���ԣ������˿");
            //				}
            //
            //				int paynum = Int32.Parse(dr["fpaynum"].ToString());
            //				string saleid = dr["Fsale_uid"].ToString();
            //				
            //
            //				strSql = "select * from c2c_db.t_middle_user where Fuid='" + saleid + "' for update";
            //				DataSet ds1 = da.dsGetTotalData(strSql);
            //
            //				if(ds1 == null || ds1.Tables.Count == 0 || ds1.Tables[0].Rows.Count != 1)
            //				{
            //					throw new LogicException("���Ҳ���ָ�����û���Ϣ��");
            //				}
            //
            //				DataRow dr1 = ds1.Tables[0].Rows[0];
            //
            //				if(dr1["Fstate"] == null || dr1["Fstate"].ToString() == "2")
            //				{
            //					throw new LogicException("�û��ʻ��������ˣ�");
            //				}
            //
            //				int num = Int32.Parse(dr1["Fbalance"].ToString()) - Int32.Parse(dr1["Fcon"].ToString());
            //
            //				if(num < paynum)
            //				{
            //					throw new LogicException("�û��������㣬�޷��˿");
            //				}
            //
            //				strSql = "select * from " + PublicRes.GetTName("t_user",dr["fbuy_uid"].ToString()) + " where Fuid='" + dr["fbuy_uid"].ToString()+ "' for update";
            //				DataSet ds2 = da.dsGetTotalData(strSql);
            //
            //				if(ds2 == null || ds2.Tables.Count == 0 || ds2.Tables[0].Rows.Count != 1)
            //				{
            //					throw new LogicException("���Ҳ���ָ�����û���Ϣ��");
            //				}
            //
            //				DataRow dr2 = ds2.Tables[0].Rows[0];
            //				//��ʼ�˿
            //				//ģ��C2C���˿����̡�
            //				//�޸Ľ��׵�״̬Ϊת���˿
            //				//�����˿��״̬���ȴ�֧��ͨ��
            //				//�̻��ʻ���ˮ�����
            //				//����ʻ���ˮ���롣
            //				//����˫����
            //				//��¼������ˮ�� �̻�����ҡ�
            //
            //			
            //				//�޸Ľ��׵�״̬Ϊת���˿�
            //				strSql = "update " + PublicRes.GetTName("t_tran_list",listid) + " set Fstate=7 where flistid='" + listid + "'";				
            //				if(!da.ExecSql(strSql)) throw new LogicException("�޸Ľ��׵�ʱ����");
            //
            //				strSql = "update " + PublicRes.GetTName("t_pay_list",dr["fsale_uid"].ToString()) + " set Fstate=7 where flistid='" + listid + "'";				
            //				if(!da.ExecSql(strSql)) throw new LogicException("�޸Ľ��׵�ʱ����");
            //
            //				strSql = "update " + PublicRes.GetTName("t_pay_list",dr["fbuy_uid"].ToString()) + " set Fstate=7 where flistid='" + listid + "'";				
            //				if(!da.ExecSql(strSql)) throw new LogicException("�޸Ľ��׵�ʱ����");
            //
            //				int paysale = 0;
            //
            //				//�����˿ dr���׵���dr1��� dr2����
            //				strSql = "Insert c2c_db.t_refund_list(Flistid,Fspid,Fbuy_uid,Fbuyid,Fsale_uid,Fsaleid,Fstate,Flstate,Fpaybuy,Fpaysale,Fbargain_time,Fcreate_time,Fok_time,Fok_time_acc,Fip,Fmodify_time,Ftrade_type,Fbuy_name,Fsale_name)"
            //					+ " values('{0}','{1}','{2}','{3}','{4}','{5}',2,2,{6},{7},now(),now(),now(),now(),'{8}',now(),2,'{9}','{10}')";
            //				strSql = String.Format(strSql,listid,dr["fspid"],dr["fbuy_uid"],dr["fbuyid"],dr["fsale_uid"],dr["fsaleid"],paynum,paysale,fh.UserIP,dr["FBuy_Name"],dr["FSale_Name"]);
            //				if(!da.ExecSql(strSql)) throw new LogicException("�����˿ʱ����");
            //
            //				
            //				//�̻��ʻ���ˮ����
            //				string strdate = DateTime.Now.ToString("yyyyMM");
            //				string tablename = "c2c_db_medi_user."  + "t_bankroll_list_" + strdate; 
            //				int newbalance = Int32.Parse(dr1["Fbalance"].ToString()) - paynum;
            //
            //				strSql = "insert " + tablename + "(Flistid,Fspid,Fuid,Fqqid,Ftype,Fsubject,Ffrom_uid,Ffromid,fbalance,Fpaynum,Fip,Faction_type,Fmodify_time_acc,Fmodify_time,Fvs_qqid,Flist_sign,FCurType,Ftrue_name,Ffrom_name,Fexplain)"
            //					+ " values('{0}','{1}','{2}','{3}',2,5,'{4}','{5}',{6},{7},'{8}',16,now(),now(),'{9}',0,{10},'{11}','{12}','{13}')";
            //				strSql = String.Format(strSql,listid,dr["fspid"],dr["fsale_uid"],dr["fsaleid"],dr["fbuy_uid"],dr["fbuyid"],newbalance,paynum,fh.UserIP,dr["fbuyid"],dr["FCurType"],dr["FBuy_Name"],dr["FSale_Name"],dr["Fexplain"]);
            //				if(!da.ExecSql(strSql)) throw new LogicException("�����̻��ʻ���ˮʱ����");
            //
            //				//����ʻ���ˮ�롣
            //				tablename = PublicRes.GetTName("t_bankroll_list",dr["fbuy_uid"].ToString()); 
            //				int newbalance1 = Int32.Parse(dr2["Fbalance"].ToString()) + paynum;
            //
            //				strSql = "insert " + tablename + "(Flistid,Fspid,Fuid,Fqqid,Ftype,Fsubject,Ffrom_uid,Ffromid,fbalance,Fpaynum,Fip,Faction_type,Fmodify_time_acc,Fmodify_time,Fvs_qqid,Flist_sign,FCurType,Ftrue_name,Ffrom_name,Fexplain)"
            //					+ " values('{0}','{1}','{2}','{3}',1,5,'{4}','{5}',{6},{7},'{8}',16,now(),now(),'{9}',0,{10},'{11}','{12}','{13}')";
            //				strSql = String.Format(strSql,listid,dr["fspid"],dr["fbuy_uid"],dr["fbuyid"],dr["fsale_uid"],dr["fsaleid"],newbalance1,paynum,fh.UserIP,dr["fsaleid"],dr["FCurType"],dr["FBuy_Name"],dr["FSale_Name"],dr["Fexplain"]);
            //				if(!da.ExecSql(strSql)) throw new LogicException("��������ʻ���ˮʱ����");
            //
            //				//��¼������ˮ��
            //				tablename = PublicRes.GetTName("t_userpay_list",dr["fsale_uid"].ToString()); //furion 20050829 ������ˮҪ��¼���̼��ˡ�
            //				strSql = "insert " + tablename + "(Flistid,Fspid,Fuid,Fqqid,Ftype,Fsubject,Ffrom_uid,Ffromid,Fbalance,Ffrom_balance,Fpaynum,Fcreate_time,Fip,Fmodify_time,Flist_sign,Fold_state,Fnew_state,FCurType,Ftrue_name,Ffrom_name,Fcoding)"
            //					+ " values('{0}','{1}','{2}','{3}',2,5,'{4}','{5}',{6},{7},{8},now(),'{9}',now(),0,2,7,{10},'{11}','{12}','{13}')";
            //				strSql = String.Format(strSql,listid,dr["fspid"],dr["fsale_uid"],dr["fsaleid"],dr["fbuy_uid"],dr["fbuyid"],newbalance,newbalance1,paynum,fh.UserIP,dr["FCurType"],dr["FSale_name"],dr["FBuy_Name"],dr["FCoding"]);
            //				if(!da.ExecSql(strSql)) throw new LogicException("���ӽ�����ˮʱ����");
            //
            //				//����˫����
            //				strSql = "update " + PublicRes.GetTName("t_user",dr["fbuy_uid"].ToString()) + " set Fbalance=Fbalance+" + paynum.ToString() 
            //					+ " where Fuid='" + dr["fbuy_uid"].ToString() + "'";
            //				if(!da.ExecSql(strSql)) throw new LogicException("����������ʱ����");
            //
            //				strSql = "update c2c_db.t_middle_user set Fbalance=Fbalance-" + paynum.ToString() 
            //					+ " where Fuid='" + dr["fsale_uid"].ToString() + "'";
            //				if(!da.ExecSql(strSql)) throw new LogicException("�����������ʱ����");			
            //				
            //				//��¼������־��
            //				strSql = "select now()";
            //				string tmp = da.GetOneResult(strSql);
            //				tmp = DateTime.Parse(tmp).ToString("yyyyMMdd");
            //				tablename = "c2c_db_paylog.t_paylog_" + tmp;
            //				strSql = "Insert " + tablename + "(Flistid,Fspid,Fuid,Fqqid,Fauid,Fvs_qqid,Fcurtype,Fnum,Fpaybuy,Frequest_type,Fcreate_time_c2c,Fmodify_time,Fip,Flist_sign,Ftrade_type)"
            //					+ " values('{0}','{1}','{2}','{3}','{4}','{5}',{6},{7},{8},12,now(),now(),'{9}',0,2)";
            //				strSql = String.Format(strSql,dr["FListID"],dr["FSpid"],dr["Fbuy_uid"],dr["Fbuyid"],dr["Fsale_uid"],dr["FSaleID"],dr["Fcurtype"],dr["Fpaynum"],dr["Fpaynum"],fh.UserIP);
            //				if(!da.ExecSql(strSql)) throw new LogicException("��¼������־��ʱ����");
            //
            //				da.Commit();
            //				da.CloseConn();
            //				return true;
            //			}
            //			catch(LogicException err)
            //			{
            //				da.RollBack();
            //				da.CloseConn();
            //				throw err;
            //				return false;
            //			}
            //			catch(Exception err)
            //			{
            //				da.RollBack();
            //				da.CloseConn();
            //				throw new LogicException("��ִ������ʱ�����˴���");
            //				return false;
            //			}
            //			finally
            //			{
            //				da.Dispose();
            //			}           

        }
    }

    #endregion




    #region �̻���ˮ��ѯ�� furion 20050817 ��Ϊ�н�ĳ��̻��Ļ������Դ������ҽ��׵����в��ˡ�

    //��ʵ������̻���Ϊ���ҵĽ��ס� furion 20050817

    public class MediListQueryClass : Query_BaseForNET
    {
        public MediListQueryClass(string u_ID, string Fcode, DateTime u_BeginTime, DateTime u_EndTime)
            : this(u_ID, Fcode, u_BeginTime, u_EndTime, "", "Flistid") { }

        public MediListQueryClass(string u_ID, string Fcode, DateTime u_BeginTime, DateTime u_EndTime, string u_UserFilter, string u_OrderBy)
        {
            if (u_ID != null && u_ID.Trim() != "")
            {
                /*
                //furion 20050817 �ȴ��̻��ʻ�����ȡ��UID������
                //string strSql = "select fuid from c2c_db.t_middle_user where fqqid='" + u_ID + "'";
                string strSql = "select fuidMiddle from c2c_db.t_merchant_info where fspid='" + u_ID + "'";

                //string fuid = PublicRes.ExecuteOne(strSql,"YW_30");
                //string fuid = PublicRes.ExecuteOne(strSql,"YWB_30");
                string fuid = PublicRes.ExecuteOne(strSql,"ZL");
                //string tablename = "c2c_db_medi_user.t_bankroll_list_" + u_BeginTime.ToString("yyyyMM");
                */

                string strSql = "spid=" + u_ID;
                string errMsg = "";
                string fuid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, "FuidMiddle", out errMsg);

                if (fuid == null || fuid.Trim() == "")
                {
                    throw new LogicException("���Ҳ���ָ�����̻���" + errMsg);
                }

                //string tablename = PublicRes.GetTName("t_pay_list", fuid);
                string tablename = PublicRes.GetTName("t_user_order", fuid);

                fstrSql = " select *,Ftrade_state as Fstate from  " + tablename;
                fstrSql_count = " select count(*) from " + tablename;


                fstrSql += " where Fsale_uid='" + fuid + "'  ";
                fstrSql_count += " where Fsale_uid='" + fuid + "'  ";


                fstrSql += " and Fcreate_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                fstrSql_count += " and Fcreate_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                if (Fcode != "")
                {
                    fstrSql += "and Fcoding = '" + Fcode + "' ";
                    fstrSql_count += "and Fcoding = '" + Fcode + "' ";
                }

                //�������Զ����ѯ���� edwardzheng 20051110
                if (u_UserFilter != "")
                {
                    fstrSql += " AND (" + u_UserFilter + ")";
                    fstrSql_count += " AND (" + u_UserFilter + ")";
                }
                //�����˹����ظ��嵥�Ĵ��� edwardzheng 20051110
               // fstrSql += " GROUP BY Flistid";
                fstrSql_count += " GROUP BY Flistid";

                //�����Ƿ�����ORDER BY������������� yonghua
               // if (u_OrderBy == "") u_OrderBy = "Flistid";
               // fstrSql += " ORDER BY " + u_OrderBy;

                fstrSql_count = "select 10000";
            }
            else
            {
                throw new LogicException("�̻��ʺŲ���Ϊ��");
            }
        }



        public MediListQueryClass(string u_ID, string Fcode, string strBeginTime, string strEndTime, string u_UserFilter
            , string u_OrderBy, int limStart, int limCount)
        {
            if (u_ID != null && u_ID.Trim() != "")
            {
                string strSql = "spid=" + u_ID;
                string errMsg = "";
                string fuid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, "FuidMiddle", out errMsg);

                if (fuid == null || fuid.Trim() == "")
                {
                    throw new LogicException("���Ҳ���ָ�����̻���" + errMsg);
                }

                //string tablename = PublicRes.GetTName("t_pay_list", fuid);
                string tablename = PublicRes.GetTName("t_user_order", fuid);

                fstrSql = " select *,Ftrade_state as Fstate from  " + tablename;
                //fstrSql_count  = " select count(*) from " + tablename;


                fstrSql += " where Fsale_uid='" + fuid + "'  ";
                //fstrSql_count += " where Fsale_uid='" + fuid + "'  ";


                if (strBeginTime.Trim() != "")
                {
                    fstrSql += " and Fcreate_time >='" + strBeginTime + "'";
                    //fstrSql_count += " and Fcreate_time >='" + strBeginTime + "'";
                }

                if (strEndTime.Trim() != "")
                {
                    fstrSql += " and Fcreate_time <='" + strEndTime + "'";
                    //fstrSql_count += " and Fcreate_time <='" + strEndTime + "'";
                }

                /*
                fstrSql += " and Fcreate_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss") 
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                fstrSql_count += " and Fcreate_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss") 
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                    */

                if (Fcode != "")
                {
                    fstrSql += "and Fcoding = '" + Fcode + "' ";
                    //fstrSql_count += "and Fcoding = '" + Fcode + "' ";
                }

                //�������Զ����ѯ���� edwardzheng 20051110
                if (u_UserFilter != "")
                {
                    fstrSql += " AND (" + u_UserFilter + ")";
                    //fstrSql_count += " AND (" + u_UserFilter + ")";
                }

                //fstrSql_count += " limit " + limStart + "," + limCount;

                //�����˹����ظ��嵥�Ĵ��� edwardzheng 20051110
               // fstrSql += " GROUP BY Flistid";
                //fstrSql_count += " GROUP BY Flistid";

               // if (u_OrderBy == "") u_OrderBy = "Flistid";
               // fstrSql += " ORDER BY " + u_OrderBy;

                fstrSql += " limit " + limStart + "," + limCount;

                fstrSql_count = "select 10000";
            }
            else
            {
                throw new LogicException("�̻��ʺŲ���Ϊ��");
            }
        }

    }


    #endregion

    #region �����ѯ�ࡣ

    public class FreezeQueryClass : Query_BaseForNET
    {
        public FreezeQueryClass(string fid)
        {
            fstrSql = "select * from c2c_fmdb.t_freeze_list  where  Fid='" + fid + "'";
            fstrSql_count = "select count(*) from c2c_fmdb.t_freeze_list where  Fid='" + fid + "'";
        }

        public FreezeQueryClass(string qqid, int handleFinish)
        {
            fstrSql = "select * from c2c_fmdb.t_freeze_list  where  FFreezeID='" + qqid + "' and FHandleFinish=1";
            fstrSql_count = "select count(*) from c2c_fmdb.t_freeze_list where  FFreezeID='" + qqid + "' and FHandleFinish=1";
        }

        public FreezeQueryClass(DateTime u_BeginTime, DateTime u_EndTime, string freezeuser, string username, int handletype, int statetype, string qqid)
        {
            string strGroup = "";
            string strWhere = "";

            strWhere += " where FFreezeTime between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

            if (statetype != 0)
            {
                strWhere += " and FHandleFinish=" + statetype.ToString() + " ";
            }

            if (handletype != 0)
            {
                strWhere += " and FFreezeType=" + handletype.ToString() + " ";
            }

            if (freezeuser != null && freezeuser.Trim() != "")
            {
                strWhere += " and FFreezeuserID='" + freezeuser + "' ";
            }

            if (username != null && username.Trim() != "")
            {
                strWhere += " and FUserName like '%" + username + "%' ";
            }

            if (qqid != null && qqid.Trim() != "")
            {
                strWhere += " and FFreezeID = '" + qqid + "' ";
            }

            string TableName1 = "c2c_fmdb.t_freeze_list";
            strGroup = strGroup + " select FID,FFreezeUserID,FFreezeTime,FFreezeType,FContact,FFreezeReason,FHandleUserID,FHandleResult,"
                + "(case FFreezeType when 1 then '�����ʻ�' when 2 then '�������׵�' else '' end) FFreezeTypeName,FUserName,"
                + "FHandleFinish,(case FHandleFinish when 1 then '������' when 9 then '�������' else '' end ) FHandleFinishName,FFreezeID from "
                + TableName1 + strWhere + " ";

            fstrSql = strGroup;
            fstrSql_count = " select count(*) from ( " + strGroup + ") a ";

        }

    }


    public class FreezeFinQueryClass : Query_BaseForNET
    {
        // Ŀǰҳ���ṩ�Ķ�����ֻ��һ���޶�������һ�����ޡ�
        private const double m_FreezeFinMax = 10000;

        public FreezeFinQueryClass(string strBeginDate, string strEndDate, string fpayAccount, double freezeFin, string flistID, int limStart, int limMax)
        {
            if (flistID == null || flistID.Trim() == "")
            {
                if (fpayAccount == null)
                {
                    throw new Exception("��ѯ�˻�����Ϊ�գ�");
                }

                this.ICEcommand = "QUERY_USER_BANKROLL_FULL";

                string strUID = PublicRes.ConvertToFuid(fpayAccount);

                this.ICESql += "uid=" + strUID;
                //�����õ�UID
                //this.ICESql += "uid=295191000";
                this.ICESql += "&start_time=" + strBeginDate + "&end_time=" + strEndDate + "&type=3";

                if (freezeFin != 0)
                {
                    this.ICESql += "&con_low=" + (freezeFin * 100) + "&con_high=" + (m_FreezeFinMax * 100);
                }

                this.ICESql += "&lim_start=" + limStart;
                this.ICESql += "&lim_count=" + limMax;
            }
            else
            {
                // �����listid��ѯ����ʹ��QUERY_BANKROLL_LISTID_2��������ӿ�ֻ����2������
                if (fpayAccount == null)
                {
                    throw new Exception("��ѯ�˻�����Ϊ�գ�");
                }

                this.ICEcommand = "QUERY_BANKROLL_LISTID_2";
                string strUID = PublicRes.ConvertToFuid(fpayAccount);
                this.ICESql += "uid=" + strUID;
                // �����õ�UID
                //this.ICESql += "uid=295191000";
                this.ICESql += "&listid=" + flistID;
            }
        }
    }





    public class FreezeQueryClass_2 : Query_BaseForNET
    {
        public FreezeQueryClass_2(string fid)
        {
            fstrSql = "select * from c2c_fmdb.t_freeze_list  where  Fid='" + fid + "'";
            fstrSql_count = "select count(*) from c2c_fmdb.t_freeze_list where  Fid='" + fid + "'";
        }


        /*
        public FreezeQueryClass_2(string strBeginDate,string strEndDate)
        {
            CFTUserAppealClass cuser = new CFTUserAppealClass("",strBeginDate,strEndDate,99,8,"");
            DataSet ds = cuser.GetResultX(iPageStart,iPageMax,"CFT");		
	
            if(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                CFTUserAppealClass.HandleParameter_ForControledFreeze(ds,true);
            }

            return ds;
        }
        */

        public FreezeQueryClass_2(string szQQID, string szBeginDate, string szEndDate, string szStatue, string szListID,
            string szFreezeUser, string szFreezeReason)
        {
            string strGroup = "";
            string strWhere = " where (1=1) ";

            if (szBeginDate != null && szBeginDate != "")
            {
                strWhere += " and a.FFreezeTime>= '" + szBeginDate + "' ";
            }

            if (szEndDate != null && szEndDate != "")
            {
                strWhere += " and a.FFreezeTime<= '" + szEndDate + "' ";
            }

            if (szStatue != "99")
            {
                if (szStatue == "0")
                {
                    strWhere += " and b.FField3 is null";
                }
                else if (szStatue == "10")
                {
                    szStatue = "0";

                    strWhere += " and b.fsourceType=" + szStatue + " and b.FField3=''";
                }
                else
                {
                    strWhere += " and b.fsourceType=" + szStatue + " and b.FField3 != '' ";
                }
            }

            if (szFreezeUser != null && szFreezeUser != "")
            {
                strWhere += " and b.FField3 like '%" + szFreezeUser + "%' ";
            }

            if (szQQID != null && szQQID != "")
            {
                strWhere += " and a.FFreezeID = '" + szQQID + "' ";
            }

            if (szListID != null && szListID != "")
            {
                strWhere += " and a.FID= '" + szListID + "'";
            }

            if (szFreezeReason != "")
            {
                strWhere += " and a.FFreezeReason like '%" + szFreezeReason + "%' ";
            }

            // ֻ��δ�ⶳ�Ľ�������
            //strWhere += " and a.FHandleFinish=1 and a.FID=b.ffreezeListID group by a.fid order by b.FCreateDate DESC ";
            strWhere += " and a.FHandleFinish=1 group by a.fid order by a.FFreezeTime DESC ";

            /*
                strWhere += " where FFreezeTime between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss") 
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                if(statetype != 0)
                {
                    strWhere += " and FHandleFinish=" + statetype.ToString() + " ";
                }
 
                if(handletype != 0)
                {
                    strWhere += " and FFreezeType=" + handletype.ToString() + " ";
                }
			
                if(freezeuser !=null && freezeuser.Trim() != "")
                {
                    strWhere += " and FFreezeuserID='" + freezeuser + "' ";
                }

                if(username !=null && username.Trim() != "")
                {
                    strWhere += " and FUserName like '%" + username + "%' ";
                }

                if(qqid != null && qqid.Trim() != "")
                {
                    strWhere += " and FFreezeID = '" + qqid + "' ";
                }
                */

            string TableName = "c2c_fmdb.t_freeze_list a ";
            //string joinSql = " left outer join (select ffreezelistid,ffield3,fsourcetype from c2c_fmdb.t_Freeze_Detail) b on a.fid = b.FFreezeListID ";
            string joinSql = " left outer join c2c_fmdb.t_Freeze_Detail b on a.fid = b.FFreezeListID ";

            /*
                strGroup += " select distinct a.FID,FFreezeUserID,FFreezeTime,FFreezeType,"
                    + "(case FFreezeType when 1 then '�����ʻ�' when 2 then '�������׵�' else '' end) FFreezeTypeName,FUserName,"
                    + "FHandleFinish,(case FHandleFinish when 1 then '������' when 9 then '�������' else '' end ) FHandleFinishName,FFreezeID,FFreezeReason from " 
                    + TableName + joinSql + strWhere + " ";
                    */

            strGroup += " select * from " + TableName + joinSql + strWhere;

            fstrSql = strGroup;
            fstrSql_count = " select count(*) from ( " + strGroup + ") a ";
        }
    }


    #endregion


    #region	 �ܿ��ʽ���Ϣ��ѯ

    public class QeuryUserControledFinInfoClass : Query_BaseForNET
    {
        public QeuryUserControledFinInfoClass(string fuid, string beginDateStr, string endDateStr, string cur_type, int iNumStart, int iNumMax)
        {
            this.ICEcommand = "FINANCE_QUERY_USER_CONTROLED";

            //string fuid = PublicRes.ConvertToFuid(qqid);
            //// fuid = "295191000";����
            //if (fuid == null || fuid.Trim() == "")
            //    throw new Exception("�ʺŲ����ڣ�");

            this.ICESql = "uid=" + fuid;
            //this.ICESql = "uid=298684980";

            if (beginDateStr != null && beginDateStr.Trim() != "" && endDateStr != null && endDateStr.Trim() != "")
            {
                this.ICESql += "&createTime_begin=" + beginDateStr + "&createTime_end=" + endDateStr;
            }

            if (cur_type != null && cur_type.Trim() != "" )
            {
                this.ICESql += "&cur_type=" + cur_type ;
            }

            this.ICESql += "&strlimit=limit " + iNumStart + "," + iNumMax;
        }
    }


    #endregion

    #region �Ƹ�ȯ��ѯ��
    public class GwqQueryClass : Query_BaseForNET
    {
        public GwqQueryClass(string u_ID)
            : this(u_ID, "")
        {
        }

        public GwqQueryClass(string u_ID, int is_tdeID)
            : this(u_ID, is_tdeID, "")
        {
        }

        public GwqQueryClass(string u_ID, string filter)
        {
            if (u_ID != null && u_ID.Trim() != "")
            {
                string tablename = "";
                string fuid = PublicRes.ConvertToFuid(u_ID);
                try
                {
                    //tablename = PublicRes.GetTableName("t_gwq",u_ID);
                    tablename = PublicRes.GetTName("t_gwq", fuid);
                }
                catch
                {
                    throw new LogicException("������Ч�򲻴���");
                }

                fstrSql = " select * from " + tablename;
                fstrSql_count = " select count(1) from " + tablename;

                //fstrSql += " where (Fuser_id='" + u_ID + "') ";
                fstrSql += " where (Fuser_uid=" + fuid + ") ";
                //fstrSql_count += " where (Fuser_id='" + u_ID + "') ";
                fstrSql_count += " where (Fuser_uid=" + fuid + ") ";

                if (filter != "")
                {
                    fstrSql += " AND (" + filter + ")";
                    fstrSql_count += " AND (" + filter + ")";
                }
            }
            else
            {
                throw new LogicException("���벻��Ϊ��");
            }
        }

        public GwqQueryClass(string u_ID,int is_tdeID,string filter)
        {
            if (u_ID != null && u_ID.Trim() != "")
            {
                string tablename = "";
                try
                {
                    if (is_tdeID != 1)
                        tablename = PublicRes.GetTableName("t_gwq", u_ID);
                    else
                    {
                        CoinPubQueryClass pq = new CoinPubQueryClass(u_ID, 1);
                        T_COIN_PUB pub = pq.GetResult();
                        tablename = PublicRes.GetTName("t_gwq", pub.fuid);
                    }
                }
                catch
                {
                    throw new LogicException("������Ч�򲻴���");
                }

                fstrSql = " select * from " + tablename;
                fstrSql_count = " select count(1) from " + tablename;

                if (is_tdeID == 1)
                {
                    fstrSql += " where (Ftde_id=" + u_ID + ") ";
                    fstrSql_count += " where (Ftde_id=" + u_ID + ") ";
                }
                else
                {
                    string uid = PublicRes.ConvertToFuid(u_ID);
                    fstrSql += " where (Fuser_uid='" + uid + "') ";
                    fstrSql_count += " where (Fuser_uid='" + uid + "') ";
                }

                if (filter != "")
                {
                    fstrSql += " AND (" + filter + ")";
                    fstrSql_count += " AND (" + filter + ")";
                }

                //��ӡ
                //PublicRes.WriteFile(fstrSql);
                //PublicRes.WriteFile(fstrSql_count);
                //PublicRes.CloseFile();
            }
            else
            {
                throw new LogicException("���벻��Ϊ��");
            }
        }
        public T_GWQ GetResult(string ticket_id)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("GWQ"));
            T_GWQ result = new T_GWQ();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql + " AND (Fticket_id='" + ticket_id + "')");
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    result.GetInfoFromDB(dr);
                }
                else
                {
                    throw new LogicException("û�в��ҵ���Ӧ�ļ�¼");
                }
                return result;
            }
            finally
            {
                da.CloseConn();
                da.Dispose();
            }
        }
    }
    #endregion

    #region �Ƹ�ȯ���е���ѯ��
    public class CoinPubQueryClass : Query_BaseForNET
    {
        public CoinPubQueryClass(string u_ID, int is_tdeID)
        {
            if (u_ID != null && u_ID.Trim() != "")
            {
                string tablename = "c2c_db_gwq.t_gwq_pub";

                fstrSql = " select * from " + tablename;
                fstrSql_count = " select count(1) from " + tablename;

                if (is_tdeID == 1)
                {
                    fstrSql += " where (Ftde_id=" + u_ID + ") ";
                    fstrSql_count += " where (Ftde_id=" + u_ID + ") ";
                }
                else
                {
                    fstrSql += " where (FSpid='" + u_ID + "') ";
                    fstrSql_count += " where (FSpid='" + u_ID + "') ";
                }
            }
            else
            {
                throw new LogicException("�����ߺ��벻��Ϊ��");
            }
        }

        public CoinPubQueryClass(string filter)
        {
            filter = filter == "" ? "" : (" WHERE " + filter);

            string tablename = "c2c_db_gwq.t_gwq_pub";
            fstrSql = " select * from " + tablename + filter;
            fstrSql_count = " select count(1) from " + tablename + filter;
        }

        public T_COIN_PUB GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("GWQ"));
            T_COIN_PUB result = new T_COIN_PUB();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    result.LoadFromDB(dr, true);
                }
                else
                {
                    throw new LogicException("û�в��ҵ���Ӧ�ļ�¼");
                }
                return result;
            }
            finally
            {
                da.CloseConn();
                da.Dispose();
            }
        }

    }
    #endregion

    #region �Ƹ�ȯ������־��ѯ��
    public class GwqRollQueryClass : Query_BaseForNET
    {
        public GwqRollQueryClass(string u_ID, string ticket_id)
        {
            if (u_ID != null && u_ID.Trim() != "")
            {
                string tablename = "";
                try
                {
                    tablename = PublicRes.GetTableName("t_operate_roll", u_ID);
                }
                catch
                {
                    throw new LogicException("��ѯ�������ʱ��������");
                }

                string fuid = PublicRes.ConvertToFuid(u_ID);

                fstrSql = " select * from " + tablename;
                fstrSql_count = " select count(1) from " + tablename;

                //fstrSql += " where (Fuser_id='" + u_ID + "' AND Fticket_id='"+ticket_id+"') ";
                fstrSql += " where (Fuser_uid='" + fuid + "' AND Fticket_id='" + ticket_id + "') ";
                //fstrSql_count += " where (Fuser_id='" + u_ID + "' AND Fticket_id='"+ticket_id+"') ";
                fstrSql_count += " where (Fuser_uid='" + fuid + "' AND Fticket_id='" + ticket_id + "') ";
            }
            else
            {
                throw new LogicException("���벻��Ϊ��");
            }
        }
    }
    #endregion

    #region �Ƹ�ȯ���м�¼��
    public class T_COIN_PUB : T_CLASS_BASIC
    {
        public bool IsNew = false; //�Ƿ�Ϊ�������н��ʻ�

        public string ftde_id;
        public string fspid;
        public string fatt_name;
        public string fqqid;
        public string fuid;
        public string fmer_id;
        public string ftype;
        public string fpub_type;
        public string fstate;
        public string flist_state;
        public string fstime;
        public string fetime;
        public string fpub_time;
        public string fpub_num;
        public string ffact_num;
        public string fdonate_type;
        public string ffee;
        public string fpre_fee;
        public string ffact_fee;
        public string fuse_pro;
        public string fmin_fee;
        public string fmax_num;
        public string furl;
        public string fpub_user;
        public string fex_user1;
        public string fex_user2;
        public string fuser_ip;
        public string fpub_ip;
        public string fstandby1;
        public string fstandby2;
        public string fstandby3;
        public string fstandby4;
        public string fstandby5;
        public string fstandby6;
        public string fstandby7;
        public string fstandby8;
        public string fmemo;
        public string fmodify_time;
        public string fpub_name;
        public string fac_stime;
        public string fac_etime;
        public string fac_flag;
        public string fac_num;
        public string fac_uin;
        public string fac_uid;
        public string returnUrl;

        public string Limits;
        public const string LIMIT_ALL = "00000";

        public void LoadFromDB(DataRow dr, bool GetLimits)
        {
            IsNew = false;
            base.LoadFromDB(dr);

            Limits = "";
            if (GetLimits)
            {
                System.Data.DataSet ds = null;
                try
                {
                    ds = PublicRes.returnDSAll("SELECT fspid,fpub_id,fmer_id FROM c2c_db_gwq.t_gwq_limit WHERE ftde_id=" + ftde_id, "GWQ");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0) Limits += ";";
                        Limits += ds.Tables[0].Rows[i]["fspid"].ToString() + "," +
                            ds.Tables[0].Rows[i]["fpub_id"].ToString() + "," +
                            ds.Tables[0].Rows[i]["fmer_id"].ToString();
                    }
                }
                catch
                {
                }
                if (ds != null) ds.Dispose();
            }
        }

        public bool CheckRight()
        {
            if (fspid == null || fspid == "")
                throw new LogicException("�����ߺ��벻��Ϊ�գ�");

            // TODO: 1�ͻ���Ϣ��������
            //����SPID��ȡ
            /*
            string strSql = "select fspecial,fname,fuid,fuidmiddle from c2c_db.t_merchant_info where fspid='" + fspid + "'";
            */
            string strSql = "spid=" + fspid;

            System.Data.DataSet dsTemp = null;
            string spid_fuid, spid_fqqid, spid_fpub_name, spid_fuidmiddle;
            try
            {
                /*
                //dsTemp = PublicRes.returnDSAll(strSql,"YW_30");
                dsTemp = PublicRes.returnDSAll(strSql,"ZL");
                */
                string errMsg = "";
                dsTemp = CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, out errMsg);

                if (dsTemp == null || dsTemp.Tables.Count == 0 || dsTemp.Tables[0].Rows.Count == 0)
                    throw new LogicException("�̻��ź�QQ��Ӧ��ϵ��ҵ��ϵͳ����δ�Ǽ�");

                spid_fuid = dsTemp.Tables[0].Rows[0]["fuid"].ToString();
                spid_fuidmiddle = dsTemp.Tables[0].Rows[0]["fuidmiddle"].ToString();
                if (spid_fuid == null || spid_fuid == "") spid_fuid = "0";
                spid_fqqid = dsTemp.Tables[0].Rows[0]["fspecial"].ToString();
                spid_fpub_name = dsTemp.Tables[0].Rows[0]["fname"].ToString();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (dsTemp != null) dsTemp.Dispose();
            }
            //�Զ���ȡQQ���롢Fuid������
            if (fqqid != null && fqqid != "" && fqqid != spid_fqqid)
            {
                //��ָ����fqqid���Һ�spid�󶨵�qqid��ͬʱ������QQ�Ż�ȡ
                fuid = PublicRes.ConvertToFuid(fqqid);
                if (fuid == null || fuid == "")
                    throw new LogicException("QQ����ҵ��ϵͳ��δ�Ǽ�");

                /*
                //strSql = "Select ftruename from " + PublicRes.GetTableName("t_user",fqqid) + " where fuid="+fuid;
                strSql = "Select ftruename from " + PublicRes.GetTableName("t_user_info",fqqid) + " where fuid="+fuid;
                //fpub_name = PublicRes.ExecuteOne(strSql,"YW_30");
                fpub_name = PublicRes.ExecuteOne(strSql,"ZL");
                */

                strSql = "uid=" + fuid;
                string Msg = "";
                fpub_name = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Ftruename", out Msg);
            }
            else
            {
                //����SPID��ȡ
                fuid = spid_fuid;
                fqqid = spid_fqqid;
                fpub_name = spid_fpub_name;
            }
            //����Ƿ���Ȩ��
            bool HasRight = false;
            //string ssign = PublicRes.ExecuteOne("Select FSign1 FROM c2c_db.t_user_att where fuid="+spid_fuidmiddle,"YW_30");
            string ssign = PublicRes.ExecuteOne("Select FSign1 FROM c2c_db.t_user_att where fuid=" + spid_fuidmiddle, "AU");
            if (ssign == null)
                HasRight = true;
            else
            {
                int isign = Convert.ToInt32(ssign);
                if ((isign & 1073741824) == 0)
                    HasRight = false;
                else
                    HasRight = true;
            }
            if (!HasRight)
                throw new LogicException("�̻��˺š�" + fspid + "��û�вƸ�ȯ����Ȩ�ޡ�");
            if (fuid != spid_fuid)
            {
                //ssign = PublicRes.ExecuteOne("Select FSign1 FROM c2c_db.t_user_att where fuid="+fuid,"YW_30");
                ssign = PublicRes.ExecuteOne("Select FSign1 FROM c2c_db.t_user_att where fuid=" + fuid, "AU");
                if (ssign == null)
                    HasRight = true;
                else
                {
                    int isign = Convert.ToInt32(ssign);
                    if ((isign & 1073741824) == 0)
                        HasRight = false;
                    else
                        HasRight = true;
                }
                if (!HasRight)
                    throw new LogicException("�����˺š�" + fqqid + "��û�вƸ�ȯ����Ȩ�ޡ�");
            }

            //��ʼ����������
            if (fstate == "" || fstate == null) fstate = "2";
            if (flist_state == "" || flist_state == null) flist_state = "1";
            fmodify_time = PublicRes.strNowTime.Replace("'", "");
            if (fpub_time == "" || fpub_time == null) fpub_time = Convert.ToDateTime(fmodify_time).ToString("yyyy-MM-dd");
            if (ffact_num == "" || ffact_num == null) ffact_num = "0";
            if (ftype == "" || ftype == null) ftype = "1";
            if (ftype == "1")
                fpre_fee = Convert.ToString(Convert.ToInt64(ffee) * Convert.ToInt64(fpub_num));
            else
                fpre_fee = "0";
            if (ffact_fee == "" || ffact_fee == null) ffact_fee = "0";
            if (fex_user1 == null) fex_user1 = "";
            if (fex_user2 == null) fex_user2 = "";
            if (fstandby1 == "" || fstandby1 == null) fstandby1 = "0";
            if (fstandby2 == "" || fstandby2 == null) fstandby2 = "0";
            if (fstandby3 == "" || fstandby3 == null) fstandby3 = "";
            if (fstandby4 == "" || fstandby4 == null) fstandby4 = "";
            if (fstandby5 == "" || fstandby5 == null) fstandby5 = "0";
            if (fstandby6 == "" || fstandby6 == null) fstandby6 = "0";
            if (fstandby7 == "" || fstandby7 == null) fstandby7 = "";
            if (fstandby8 == "" || fstandby8 == null) fstandby8 = "";
            if (Convert.ToDateTime(fstime) >= Convert.ToDateTime(fetime))
                throw new LogicException("��Ч���ڱ���С�ڽ�������");
            if (ftype == "2")
            {
                int iFee = Convert.ToInt32(ffee);
                if (iFee <= 0 || iFee >= 10000)
                    throw new LogicException("�����ۿ�ȯ����ֵ������1��9999֮��");
            }
            //��ʼ��������ص��ֶ�
            if (fac_flag == "" || fac_flag == null) fac_flag = "2";
            if (fac_num == "" || fac_num == null) fac_num = "1";
            if (fpub_type == "1")
            {
                if (Convert.ToDateTime(fetime) <= Convert.ToDateTime(fac_etime) ||
                    Convert.ToDateTime(fac_stime) >= Convert.ToDateTime(fac_etime))
                    throw new LogicException("��ʼ����ʱ�����С�ڽ�������ʱ�䣬���Ҷ�����С�ڽ�������");
                fac_uid = PublicRes.ConvertToFuid(fac_uin);
                if (fac_uid == null || fac_uid == "")
                    throw new LogicException("���������˺���ҵ��ϵͳ��δ�Ǽ�");
            }
            else
            {
                fac_uin = "";
                fac_uid = "0";
            }
            if (fpub_type == "1")
            {
                //				if (fdonate_type!="1")
                //					throw new LogicException("��������Ϊ����ʱ��������������");
            }
            return true;
        }

        public bool Update(Finance_Header fh)
        {
            ArrayList al = new ArrayList();
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("GWQ"));
            try
            {
                CheckRight();

                //��ʼ����
                da.OpenConn();
                if (IsNew)
                {
                    ftde_id = da.GetOneResult("SELECT MAX(ftde_id)+1 FROM c2c_db_gwq.t_gwq_pub");
                    if (ftde_id == null || ftde_id == "")
                        ftde_id = "1";
                    string strSql = "Insert c2c_db_gwq.t_gwq_pub "
                        + " (ftde_id,fspid,fatt_name,fqqid,fuid, fmer_id,ftype,fpub_type,fstate,flist_state,"
                        + " fstime,fetime,fpub_time,fpub_num,ffact_num,fdonate_type,"
                        + " ffee,fpre_fee,ffact_fee,fuse_pro,fmin_fee,fmax_num,furl,fpub_user,fex_user1,"
                        + " fex_user2,fuser_ip,fpub_ip,fstandby1,fstandby2,fstandby3,fstandby4,fmemo,fmodify_time,"
                        + " fpub_name,fac_stime,fac_etime,fac_flag,fac_num,fac_uin,fac_uid,fstandby5,fstandby6,fstandby7,fstandby8)"
                        + " values "
                        + " ({34},'{0}','{1}','{2}',{3},'{4}',{5},{6},{7},{8},"
                        + " '{9}','{10}','{11}',{12},{13},{14},"
                        + " {15},{16},{17},{18},{19},{20},'{21}','{22}','{23}',"
                        + " '{24}','{25}','{26}',{27},{28},'{29}','{30}','{31}','{32}',"
                        + " '{33}','{35}','{36}',{37},{38},'{39}',{40},{41},{42},'{43}','{44}')";
                    strSql = String.Format(strSql,
                        fspid, fatt_name, fqqid, fuid, fmer_id, ftype, fpub_type, fstate, flist_state,
                        fstime, fetime, fpub_time, fpub_num, ffact_num, fdonate_type,
                        ffee, fpre_fee, ffact_fee, fuse_pro, fmin_fee, fmax_num, furl, fpub_user, fex_user1,
                        fex_user2, fuser_ip, fpub_ip, fstandby1, fstandby2, fstandby3, fstandby4, fmemo, fmodify_time, fpub_name,
                        ftde_id, fac_stime, fac_etime, fac_flag, fac_num, fac_uin, fac_uid, fstandby5, fstandby6, fstandby7, fstandby8);
                    al.Add(strSql);
                }
                else
                {
                    string strSql = "UPDATE c2c_db_gwq.t_gwq_pub SET "
                        + " fspid='{0}',fatt_name='{1}',fqqid='{2}',fuid={3}, fmer_id='{4}',ftype={5},fpub_type={6},fstate={7},flist_state={8},"
                        + " fstime='{9}',fetime='{10}',fpub_time='{11}',fpub_num={12},ffact_num={13},fdonate_type={14},"
                        + " ffee={15},fpre_fee={16},ffact_fee={17},fuse_pro={18},fmin_fee={19},fmax_num={20},furl='{21}',fpub_user='{22}',fex_user1='{23}',"
                        + " fex_user2='{24}',fuser_ip='{25}',fpub_ip='{26}',fstandby1={27},fstandby2={28},fstandby3='{29}',fstandby4='{30}',fmemo='{31}',fmodify_time='{32}',"
                        + " fpub_name='{33}',fac_stime='{35}',fac_etime='{36}',fac_flag={37},fac_num={38},fac_uin='{39}',fac_uid={40},fstandby5={41},fstandby6={42},fstandby7='{43}',fstandby8='{44}'"
                        + " WHERE Ftde_id={34}";
                    strSql = String.Format(strSql,
                        fspid, fatt_name, fqqid, fuid, fmer_id, ftype, fpub_type, fstate, flist_state,
                        fstime, fetime, fpub_time, fpub_num, ffact_num, fdonate_type,
                        ffee, fpre_fee, ffact_fee, fuse_pro, fmin_fee, fmax_num, furl, fpub_user, fex_user1,
                        fex_user2, fuser_ip, fpub_ip, fstandby1, fstandby2, fstandby3, fstandby4, fmemo, fmodify_time,
                        fpub_name,
                        ftde_id,
                        fac_stime, fac_etime, fac_flag, fac_num, fac_uin, fac_uid, fstandby5, fstandby6, fstandby7, fstandby8);
                    al.Add(strSql);
                }
                //����Ƿ��м����ã����Զ���ʼ��in��
                if (fpub_type == "1")
                {
                    al.Add("DELETE FROM c2c_db_gwq.t_gwq_in WHERE Ftde_id=" + ftde_id);
                    string strSql = "INSERT c2c_db_gwq.t_gwq_in (Ftde_id,Fson_id,Fqqid,FNum,FImportDate,FReleaseDate,FState,FModify_Time)" +
                        " VALUES(" + ftde_id + ",0,'" + fac_uin + "'," + fpub_num + ",'" + fmodify_time + "','0000-00-00 00:00:00',1,'" + fmodify_time + "')";
                    al.Add(strSql);
                }
                UpdateLimit(fh, al);
                return da.ExecSqls_Trans(al);
            }
            finally
            {
                da.Dispose();
            }
        }

        public bool UpdateLimit(Finance_Header fh, ArrayList al)
        {
            try
            {
                Limits = Limits.Replace(" ", "").Replace("��", "").Replace("��", ";").Replace("��", ",");
                if (Limits == "")
                {
                    Limits = LIMIT_ALL + "," + LIMIT_ALL + "," + LIMIT_ALL;
                }
                string[] limits = Limits.Split(";".ToCharArray());

                string sql = "DELETE FROM c2c_db_gwq.t_gwq_limit WHERE ftde_id = " + ftde_id;
                al.Add(sql);

                sql = "INSERT c2c_db_gwq.t_gwq_limit (Ftde_id,Fpub_id,Fpub_uid,Fspid,Fmer_id," +
                    " Fstandby1,Fstandby2,Fstandby3,Fstandby4,Flist_state,Fcreate_time,Fmodify_time)" +
                    " VALUES ( " + ftde_id + ",'{2}',{3},'{0}','{1}',0,0,'','',1,'" + fmodify_time + "','" + fmodify_time + "')";

                string temp_pub_uid;
                for (int i = 0; i < limits.Length; i++)
                {
                    if (limits[i] == "") continue;
                    //����ظ���������
                    bool IsExist = false;
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (limits[i] == limits[j])
                        {
                            IsExist = true;
                            break;
                        }
                    }
                    if (IsExist) continue;
                    //��ʼ����
                    string[] a = limits[i].Split(",".ToCharArray());
                    if (a == null || a.Length != 3)
                        throw new LogicException("ʹ�÷�Χ�С�" + limits[i] + "����ʽ����ȷ");
                    if (a[0] != LIMIT_ALL)
                    {
                        if (!SPIDIsExists(a[0]))
                            throw new LogicException("ʹ�÷�Χ���̻��š�" + a[0] + "��������");
                    }
                    if (a[1] != LIMIT_ALL)
                    {
                        temp_pub_uid = PublicRes.ConvertToFuid(a[1]);
                        if (temp_pub_uid == null || temp_pub_uid == "")
                            throw new LogicException("ʹ�÷�Χ�������ʺš�" + a[1] + "��������");
                    }
                    else
                        temp_pub_uid = "0";
                    al.Add(String.Format(sql, a[0], a[2], a[1], temp_pub_uid));
                }
                return true;
            }
            finally
            {
            }
        }
    }
    #endregion

    #region �Ƹ�ȯ��
    public class T_GWQ : T_CLASS_BASIC
    {
        public bool IsNew = false; //�Ƿ�Ϊ�������н��ʻ�

        public string fauto_id;
        public string fticket_id;
        public string ftde_id;
        public string fson_id;
        public string fuser_id;
        public string fuser_uid;
        public string fpub_id;
        public string fpub_uid;
        public string flistid;
        public string fspid;
        public string fatt_name;
        public string fmer_id;
        public string fdonate_type;
        public string fdonate_num;
        public string ftype;
        public string fpub_type;
        public string fstate;
        public string flist_state;
        public string fadjust_flag;
        public string ffee;
        public string fuse_pro;
        public string fmin_fee;
        public string fmax_num;
        public string ffact_fee;
        public string fstime;
        public string fetime;
        public string fpub_time;
        public string fuse_time;
        public string furl;
        public string fpub_user;
        public string fuser_ip;
        public string fpub_ip;
        public string fstandby1;
        public string fstandby2;
        public string fstandby3;
        public string fstandby4;
        public string fmemo;
        public string fmodify_time;
        public string fpub_name;
        public string fdonate_id;
        public string fdonate_uid;
        public string fdonate_time;
        public string fuse_listid;
        public string fac_stime;
        public string fac_etime;
        public string fac_flag;
        public string fac_num;
        public string fac_uin;
        public string fac_uid;

        public void GetInfoFromDB(DataRow dr)
        {
            IsNew = false;
            base.LoadFromDB(dr);
        }
    }
    #endregion

    #region �������ߴ�����

    public class CFTUserAppealClass : Query_BaseForNET
    {
        //δ�쵥���߲��ҵ���Ч�� furion 20080508
        public static int AppealValidDay = Int32.Parse(ConfigurationManager.AppSettings["AppealValidDay"]);
        /*
                public static bool GetAppealUserSumInfo(string user, DateTime begintime, DateTime endtime, out string msg)
                {
                    msg = "";

                    MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
                    try
                    {				
                        da.OpenConn();

                        string validdaysql = DateTime.Now.AddDays(-AppealValidDay).ToString("yyyy-MM-dd 00:00:00");
                        validdaysql = " FsubmitTime >='" + validdaysql + "' ";

                        string strSql = "select count(*) from t_tenpay_appeal_trans where " + validdaysql + " and Fstate=0 and  FType in (1,2,3,5) "; //����δ������
					

                        int itmp = Int32.Parse(da.GetOneResult(strSql));
                        msg = "����δ�������ߣ�[" + itmp + "];";

                        strSql = "select count(*) from t_tenpay_appeal_trans where " + validdaysql + " and Fstate=8 and Fpickuser='" + user + "'"; //�쵥��				
                        itmp = Int32.Parse(da.GetOneResult(strSql));
                        msg += "�����쵥����[" + itmp + "];";

        //				strSql = "select count(*) from t_tenpay_appeal_trans where Fpickuser='" + user 
        //					//+ "' and Fstate<>8 and FPickTime between '" + begintime.ToString("yyyy-MM-dd HH:mm:ss")
        //					+ "' and Fstate<>8 and FCheckTime between '" + begintime.ToString("yyyy-MM-dd HH:mm:ss")
        //					+ "' and '" + endtime.ToString("yyyy-MM-dd HH:mm:ss") + "'"; //ָ��ʱ�����Ѵ�����
        //				itmp = Int32.Parse(da.GetOneResult(strSql));
                        strSql = "select SuccessNum,FailNum,OtherNum from t_tenpay_appeal_kf_total where User='" + user + "' and OperationDay='" + begintime.ToString("yyyy-MM-dd") + "'";
                        DataSet ds= da.dsGetTotalData(strSql);
                        if(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                        {
                            itmp = Int32.Parse(ds.Tables[0].Rows[0][0].ToString()) + Int32.Parse(ds.Tables[0].Rows[0][1].ToString()) + Int32.Parse(ds.Tables[0].Rows[0][2].ToString());
                        }
                        else
                            itmp = 0;

                        msg += "����" + begintime.ToString("yyyy-MM-dd") + "��������[" + itmp + "];";
                        return true;
                    }
                    catch(Exception err)
                    {
                        msg = PublicRes.replaceMStr(err.Message);
                        return false;
                    }
                    finally
                    {
                        da.Dispose();
                    }
			
                }
        */
        private static bool SendAppealMail(string email, int ftype, bool issucc, string param1,
            string param2, string param3, string param4, string Reason, string OtherReason, string fuin, out string msg)
        {
            msg = "";

            if (email == null || email.Trim() == "")
            {
                //furion 20060902 �Ƿ�֧�ֲ����ʼ�ȡ��������.Ҫô������,Ҫô���ؼ�
                return true;
            }

            string filename = ConfigurationManager.AppSettings["ServicePath"].Trim();
            if (!filename.EndsWith("\\"))
                filename += "\\";

            string title = "";
            string actiontype = "";
            switch (ftype)
            {
                case 0:
                    {
                        title = "�Ƹ�ͨ���߽��Ƹ���֪ͨ��";
                        if (issucc)
                            filename += "UKeyYes.htm";
                        else
                            filename += "UKeyNo.htm";
                        break;
                    }
                case 1:
                    {
                        title = "�Ƹ�ͨȡ��֧������֪ͨ��";
                        if (issucc)
                        {
                            filename += "GetPwdYes.htm";
                            actiontype = "2031";
                        }
                        else
                        { 
                            filename += "GetPwdNo.htm";
                            actiontype = "2032";
                        }
                        break;
                    }
                case 2:
                case 3:
                    {
                        title = "�Ƹ�ͨ�޸���ʵ����֪ͨ��";
                        if (issucc)
                        {
                            filename += "ChangeNameYes.htm";
                            actiontype = "2035";
                        }
                        else
                        {
                            filename += "ChangeNameNo.htm";
                            actiontype = "2036";
                        }
                        break;
                    }
                case 4:
                    {
                        title = "ע���Ƹ�ͨ�ʻ���֪ͨ��";
                        if (issucc)
                        {
                            filename += "DelUserYes.htm";
                            actiontype = "2034";
                        }
                        else
                        { 
                            filename += "DelUserNo.htm";
                            actiontype = "2043";
                        }
                        break;
                    }
                case 5:
                case 6://��ע���û����ֻ� andrew 20110419
                    {
                        title = "�Ƹ�ͨ�����ֻ���������֪ͨ��";
                        if (issucc)
                        { 
                            filename += "ChangeMobileYes.htm";
                            actiontype = "2027";
                        }
                        else
                        { 
                            filename += "ChangeMobileNo.htm";
                            actiontype = "2037";
                        }
                        break;
                    }
                case 7://�������֤��
                    {
                        title = "�Ƹ�ͨ��������";
                        if (issucc)
                        { 
                            filename += "ChangeCreidYes.htm";
                            actiontype = "2041";
                        }
                        else
                        { 
                            filename += "ChangeCreidNo.htm";
                            actiontype = "2042";
                        }
                        break;
                    }
                case 9://�ֻ�����
                    {
                        title = "�Ƹ�ͨ��������";
                        if (issucc)
                        { 
                            filename += "MobileCommandYes.htm";
                            actiontype = "2029";
                        }
                        else
                        { 
                            filename += "MobileCommandNo.htm";
                            actiontype = "2030";
                        }
                        break;
                    }
                case 10://����������
                    {
                        title = "�Ƹ�ͨ��������";
                        if (issucc)
                        { 
                            filename += "MobileCommandYes.htm";
                            actiontype = "2029";
                        }
                        else
                        { 
                            filename += "MobileCommandNo.htm";
                            actiontype = "2030";
                        }
                        break;
                    }
                default:
                    {
                        msg = "�������Ͳ���";
                        return false;
                    }
            }

            try
            {
                if (!issucc)
                {
                    if (Reason == "")
                    {
                        Reason = "- " + OtherReason;
                    }
                    else
                    {
                        Reason = Reason.Substring(0, Reason.Length - 1);
                        string[] ReasonDetail = Reason.Split('&');
                        Reason = "";
                        // 2012/4/18 �������ԭ��
                        for (int i = 0; i < ReasonDetail.Length; i++)
                        {
                            if (ReasonDetail[i] == "���������һص�ַ")
                            {
                                string specialAppealFindBack = @"���������ύ������ʱ���ϴ��˻��ʽ���Դ��ͼ����ο���<a href='http://help.tenpay.com/cgi-bin/helpcenter/help_center.cgi?id=2232'> ���������һ�</a>��ָ��";
                                
                                Reason += "- " + specialAppealFindBack + "<br>";
                            }
                            else
                            {
                                Reason += "- " + ReasonDetail[i].ToString() + "<br>";
                            }
                        }
                        if (OtherReason != "")
                        {
                            Reason += "- " + OtherReason + "<br>";
                        }
                    }
                }

                if (PublicRes.IgnoreLimitCheck)
                    return true;
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            
            if (ftype == 0)
            {
                //ʹ���ϵ��ʼ����ͷ�ʽ
                StreamReader sr = new StreamReader(filename, System.Text.Encoding.GetEncoding("GB2312"));
                try {
                    string content = sr.ReadToEnd();
                    content = String.Format(content, param1, fuin, param2, param3, param4, Reason);
                    TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend newMail = new TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend();
                    newMail.SendMail(email, "", title, content, true, null);
                    return true;
                }
                catch (Exception err)
                {
                    msg = err.Message;
                    return false;
                }
                finally
                {
                    sr.Close();
                }
                

            }
            else { 
                //yinhuang 2031/8/7
                string str_params = "p_name=" + param1 + "&p_parm1=" + param2 + "&p_parm2=" + param3 + "&p_parm3=" + param4 + "&p_parm4=" + Reason;
                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(email, actiontype, str_params);

                return true;
            }
        }

        //�����¿���ʼ���ֻ��1��5��6
        private static bool SendAppealMailNew(string email, int ftype, bool issucc, string param1,
           string param2, string param3, string param4, string Reason, string OtherReason, string fuin, out string msg)
        {
            msg = "";

            if (email == null || email.Trim() == "")
            {
                //furion 20060902 �Ƿ�֧�ֲ����ʼ�ȡ��������.Ҫô������,Ҫô���ؼ�
                return true;
            }

            string title = "";
            string actiontype = "";
            switch (ftype)
            {
                
                case 1:
                    {
                        title = "�Ƹ�ͨȡ��֧������֪ͨ��";
                        if (issucc)
                        {
                            actiontype = "2217";
                        }
                        else
                        {
                            actiontype = "2032";
                        }
                        break;
                    }
              
                case 5:
                case 6://��ע���û����ֻ� 
                    {
                        title = "�Ƹ�ͨ�����ֻ���������֪ͨ��";
                        if (issucc)
                        {
                            actiontype = "2223";
                        }
                        else
                        {
                            actiontype = "2037";
                        }
                        break;
                    }
                default:
                    {
                        msg = "�������Ͳ���";
                        return false;
                    }
            }
            string str_params = "p_name=" + param1 + "&p_parm1=" + param2 + "&p_parm2=" + param3 + "&p_parm3=" + param4 + "&p_parm4=" + Reason;
                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(email, actiontype, str_params);
                return true;

        }

        //�����¿��QQTips
        private static bool SendAppealQQTips(string qqid, int ftype, string url, bool issucc, out string msg)
        {
            msg = "";

            if (qqid == null || qqid.Trim() == "")
            {
                // �Ƿ�֧�ֲ���QQtipsȡ��������.Ҫô������,Ҫô���ؼ�
                return true;
            }

            string actiontype = "";
            switch (ftype)
            {

                case 1:
                    {
                        if (issucc)
                        {
                            actiontype = "2217";
                        }
                        else
                        {
                            actiontype = "";
                        }
                        break;
                    }
                default:
                    {
                        return false;
                    }
            }
            string str_params = "url=" + url ;
            TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsgQQTips(qqid, actiontype, str_params);
            return true;

        }

        //�����¿������
        private static bool SendAppealMessage(string mobile,int ftype, string url, bool issucc,string reason, out string msg)
        {
            msg = "";

            if (mobile == null || mobile.Trim() == "")
            {
                // �Ƿ�֧�ֲ�������ȡ��������.Ҫô������,Ҫô���ؼ�
                return false;
            }
            string noticeMobile = "";//�������ֻ��ţ������м�6λ
            string actiontype = "";
            switch (ftype)
            {

                case 1:
                    {
                        if (issucc)
                        {
                            actiontype = "2217";
                        }
                        else
                        {
                            actiontype = "2222";
                        }
                        break;
                    }
                case 5:
                case 6://��ע���û����ֻ� 
                    {
                        noticeMobile = mobile.Substring(0, 3) + "******" + mobile.Substring(9, 2);
                        if (issucc)
                        {
                            actiontype = "2223";
                        }
                        else
                        {
                            actiontype = "2224";
                        }
                        break;
                    }
                default:
                    {
                        msg = "�������Ͳ���";
                        return false;
                    }
            }
            try
            {
                if (!issucc)
                {
                    reason = reason.Substring(0, reason.Length - 1);
                    string[] ReasonDetail = reason.Split('&');
                    reason = "";
                    for (int i = 0; i < ReasonDetail.Length; i++)
                    {
                        if (i == 0)
                            reason += ReasonDetail[i].ToString();
                        else
                        {
                            if (ReasonDetail[i].ToString() != "")
                                reason += "��" + ReasonDetail[i].ToString();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
   
            string str_params = "url=" + url + "&reason=" + reason + "&phoneNUM=" + noticeMobile;
            TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMessage(mobile, actiontype, str_params);
            return true;

        }

        /*
                public static string makePwd()
                {
                    string str1 = "ABCDEFGHIJKLMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz";   //�����ֵ��дABCDEF + Сдghijklmnopqrstuvwxyz
                    int iStr1 = str1.Length;
			
                    string str2 = "2345678998765432";                   //�����ֵ������ַ� + ��������
                    int iStr2 = str2.Length;

                    string str3 = "abcdefghjkmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ";   //�����ֵ�Сдabcdef + ��дGHIJKLMNOPQRSTUVWXYZ
                    int iStr3 = str3.Length;

                    string str4 = "2345678998765432";   //�����ֵ�Сдabcdef + ��дGHIJKLMNOPQRSTUVWXYZ
                    int iStr4 = str4.Length;

                    string s= null;

                    ArrayList al = new ArrayList();
                    al.Add(str1);
                    al.Add(str2);
                    al.Add(str3);
                    al.Add(str4);
			
                    for (int i = 0;i<4; i++)
                    {
                        s += pwd(al[i].ToString());  //��ÿ�������ֵ������ȡ��2λ�����12λ�������
                    }

                    return s;

                }

                private static string pwd(string str)
                {
                    int ln = str.Length;
			
                    System.Random rd = new Random();
			
                    string pwd = null;
			
                    for (int i = 0; i< 2; i++)
                    {
                        //furion ����ط�һ����Ҫ��ʱ��
                        System.Threading.Thread.Sleep(1);
                        int j = rd.Next(str.Length);
                        pwd += str[j];
                    }

                    return pwd;
                }
        */

        public static string makePwd()
        {
            string s = null;

            for (int i = 0; i < 8; i++)
            {
                System.Random rd = new Random();

                //furion ����ط�һ����Ҫ��ʱ��
                System.Threading.Thread.Sleep(1);
                s += rd.Next(10);
            }

            return s;

        }

        private static string pwd(string str)
        {
            int ln = str.Length;

            System.Random rd = new Random();

            string pwd = null;

            for (int i = 0; i < 2; i++)
            {
                //furion ����ط�һ����Ҫ��ʱ��
                System.Threading.Thread.Sleep(1);
                int j = rd.Next(str.Length);
                pwd += str[j];
            }

            return pwd;
        }

        private static bool GetNewPwd(string qqid, int clear_pps, string userip, string nowtime, string question1, string answer1, bool IsNew, out string msg)
        {
            msg = "";

            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YW_30"));
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));
            try
            {
                //da.OpenConn();
                string pwd = makePwd();
                string pwdmd5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "md5").ToLower();

                string uid = PublicRes.ConvertToFuid(qqid);

                // TODO: 1�ͻ���Ϣ��������
                if (uid != null && uid.Trim() != "")
                {

                    /*
                    //furion 20061128 email��¼�޸ģ�����ĵط���Ҫ�޸ġ�
                    string strSql = "update " + PublicRes.GetTName("t_user_info",uid) + " SET Fpasswd = '" + pwdmd5 
                        + "',Fip='" + userip + "',Fmodify_time='" + nowtime + "' where fuid =" + uid ;

                    if(clear_pps == 1)
                    {
                        strSql = "update " + PublicRes.GetTName("t_user_info",uid) + " SET Fpasswd = '" + pwdmd5 
                            + "',Fquestion1 = null,Fanswer1 = null,Fquestion2 = null,Fanswer2 = null" 
                            + ",Fip='" + userip + "',Fmodify_time='" + nowtime + "' where fuid =" + uid ;
                    }				
			

                    if(da.ExecSqlNum(strSql) == 1)
                    */

                    string strSql = "uid=" + uid;
                    strSql += "&modify_time=" + PublicRes.strNowTimeStander;
                    strSql += "&passwd=" + pwdmd5;
                    strSql += "&pass_flag=0";
                    strSql += "&ip=" + userip;

                    if (clear_pps == 1)
                    {
                        if (IsNew)
                        {
                            strSql += "&question1=" + question1;
                            strSql += "&answer1=" + answer1;
                            strSql += "&question2=";
                            strSql += "&answer2=";

                            if (question1 == "" || answer1 == "")
                            {
                                msg = "�ܱ�������Ϊ��,����ʧ��";
                                return false;
                            }
                        }
                        else
                        {
                            strSql += "&question1=" + question1;
                            strSql += "&answer1=" + answer1;
                            strSql += "&question2=";
                            strSql += "&answer2=";
                        }
                    }

                    int iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERINFO, out msg);

                    if (iresult == 1)//���Լ�iresult == -5
                    {
                        msg = pwd;
                        //�����ʼ���.
                        return true;
                    }
                    else
                    {
                        msg = "ִ�и����������ʱʧ��";
                        return false;
                    }
                }
                else
                {
                    msg = "��ȡ�ڲ�IDʧ��.";
                    return false;
                }

            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                //da.Dispose();
            }
        }

        private static bool CheckQuestion(string qqid, string userip, string nowtime, string question1, string answer1, bool IsNew, out string msg)
        {
            msg = "";
            try
            {
                string uid = PublicRes.ConvertToFuid(qqid);

                // TODO: 1�ͻ���Ϣ��������
                if (uid != null && uid.Trim() != "")
                {


                    string strSql = "uid=" + uid;
                    strSql += "&modify_time=" + PublicRes.strNowTimeStander;
                    strSql += "&ip=" + userip;


                    if (IsNew)
                    {
                        strSql += "&question1=" + question1;
                        strSql += "&answer1=" + answer1;
                        strSql += "&question2=";
                        strSql += "&answer2=";

                        if (question1 == "" || answer1 == "")
                        {
                            msg = "�ܱ�������Ϊ��,����ʧ��";
                            return false;
                        }
                    }
                    else
                    {
                        strSql += "&question1=" + question1;
                        strSql += "&answer1=" + answer1;
                        strSql += "&question2=";
                        strSql += "&answer2=";
                    }


                    int iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERINFO, out msg);

                    if (iresult == 1)
                    {
                        msg = "�ܱ�����ʹ��滻�ɹ�";
                        //�����ʼ���.
                        return true;
                    }
                    else
                    {
                        msg = "�ܱ�����ʹ��滻ʧ��";
                        return false;
                    }
                }
                else
                {
                    msg = "��ȡ�ڲ�IDʧ��.";
                    return false;
                }

            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                //da.Dispose();
            }
        }
        private static bool DelUser(string qqid, string email, string reason, string user, string userIP, string nowtime, out string msg)
        {
            msg = "";

            try
            {
                Finance_Manage ms = new Finance_Manage();
                if (!ms.logOnUser(qqid, reason, user, userIP, out msg))
                {
                    msg += "��������ʧ�ܣ�";
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
        }

        private static bool UpdateUserName(string qqid, string new_name, bool iscompany, string userip, string nowtime, out string msg)
        {
            msg = "";

            if (new_name == null || new_name.Trim() == "")
            {
                msg = "�����Ʋ���Ϊ��";
                return false;
            }

            new_name = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.replaceSqlStr(new_name);

            //�Ѹ���  furion V30_FURION���Ĳ�ѯ��Ķ� �޸ĵ��ú���
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YW"));
            //MySqlAccess da_zl = new MySqlAccess(PublicRes.GetConnString("ZL"));
            try
            {
                string uid = PublicRes.ConvertToFuid(qqid);

                // TODO: 1�ͻ���Ϣ��������
                if (uid != null && uid.Trim() != "")
                {
                    /*
                    string strCmd1 = "update " + PublicRes.GetTName("t_user_info",uid) + " SET Ftruename = '" + new_name  
                        + "',Fip='" + userip + "',Fmodify_time='" + nowtime + "' where fuid =" + uid ;
                    string strCmd2 = "update " + PublicRes.GetTName("t_user",uid)      + " SET Ftruename = '" + new_name  
                        + "',Flogin_ip='" + userip + "',Fmodify_time='" + nowtime + "' where fuid =" + uid ;
                    string strCmd3 = "update " + PublicRes.GetTName("t_bank_user",uid) + " SET Ftruename = '" + new_name  
                        + "',Flogin_ip='" + userip + "',Fmodify_time='" + nowtime + "' where fuid =" + uid ;
			
                    if(iscompany)
                    {
                        strCmd1 = "update " + PublicRes.GetTName("t_user_info",uid) + " SET Fcompany_name='" + new_name  
                            + "',Fip='" + userip + "',Fmodify_time='" + nowtime + "' where fuid =" + uid ;
                        strCmd2 = "update " + PublicRes.GetTName("t_user",uid)      + " SET Fcompany_name='" + new_name  
                            + "',Flogin_ip='" + userip + "',Fmodify_time='" + nowtime + "' where fuid =" + uid ;
                        strCmd3 = "update " + PublicRes.GetTName("t_bank_user",uid) + " SET Fcompany_name='" + new_name  
                            + "',Flogin_ip='" + userip + "',Fmodify_time='" + nowtime + "' where fuid =" + uid ; 
                    }

                    ArrayList al = new ArrayList();
			
                    al.Add(strCmd1);
                    //al.Add(strCmd2);
                    al.Add(strCmd3);

                    //da.OpenConn();
                    da_zl.OpenConn();

                    //if(da.ExecSqls_Trans(al))
                    //if(da.ExecSqlNum(strCmd2) == 1 && da_zl.ExecSqls_Trans(al))
                    if( da_zl.ExecSqls_Trans(al))
                    {
                    */

                    string strsetname = "&truename=" + new_name;
                    if (iscompany)
                        strsetname = "&company_name=" + new_name;

                    string systemtime = PublicRes.strNowTimeStander;

                    string strSql = "uid=" + uid;
                    strSql += "&modify_time=" + systemtime;
                    strSql += "&ip=" + userip;
                    strSql += strsetname;

                    int iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERINFO, out msg);

                    if (iresult != 1)
                        return false;


                    ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
                    try
                    {
                        ice.OpenConn();
                        string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + uid + "&");
                        strwhere += ICEAccess.URLEncode("fcurtype=" + 1 + "&");

                        string strUpdate = "data=" + ICEAccess.URLEncode("fip=" + userip);
                        if (iscompany)
                        {
                            strUpdate += ICEAccess.URLEncode("&fcompany_name=" + ICEAccess.URLEncode(new_name));
                        }
                        else
                        {
                            strUpdate += ICEAccess.URLEncode("&ftruename=" + ICEAccess.URLEncode(new_name));
                        }

                        strUpdate += ICEAccess.URLEncode("&fmodify_time=" + PublicRes.strNowTimeStander);

                        string strResp = "";
                        //3.0�ӿڲ�����Ҫ furion 20090708
                        if (ice.InvokeQuery_Exec(YWSourceType.�û���Դ, YWCommandCode.�޸��û���Ϣ, uid, strwhere + "&" + strUpdate, out strResp) != 0)
                        {
                            throw new Exception("�޸��û���Ϣʱ����" + strResp);
                        }

                        return true;
                    }
                    catch (Exception err)
                    {
                        msg = err.Message;
                        return false;
                    }
                    finally
                    {
                        ice.Dispose();
                    }
                }
                else
                {
                    msg = "��ȡ�ڲ�IDʧ��.";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                //da.Dispose();
                //da_zl.Dispose();
            }
        }

        private static bool UpdateCreid(string qqid, string creid, string userip, string nowtime, out string msg)
        {
            msg = "";

            if (creid == null || creid.Trim() == "")
            {
                msg = "�����֤�Ų���Ϊ��";
                return false;
            }

            creid = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.replaceSqlStr(creid);
            try
            {
                string uid = PublicRes.ConvertToFuid(qqid);

                // TODO: 1�ͻ���Ϣ��������
                if (uid != null && uid.Trim() != "")
                {
                    string systemtime = PublicRes.strNowTimeStander;

                    string strSql = "uid=" + uid;
                    strSql += "&modify_time=" + systemtime;
                    strSql += "&ip=" + userip;
                    strSql += "&creid=" + creid;

                    int iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERINFO, out msg);

                    if (iresult == 1)
                        return true;
                    else
                        return false;
                }
                else
                {
                    msg = "��ȡ�ڲ�IDʧ��.";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
        }

        public static string getCgiString(string instr)
        {
            if (instr == null || instr.Trim() == "")
                return "";

            //System.Text.Encoding enc = System.Text.Encoding.GetEncoding("GB2312");
            return System.Web.HttpContext.Current.Server.UrlDecode(instr).Replace("\r\n", "").Trim()
                .Replace("%3d", "=").Replace("%20", " ").Replace("%26", "&").Replace("%7c", "|");
        }

        private static string GetOneParameter(string fid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                da.OpenConn();
                string strSql = "select FParameter from t_tenpay_appeal_trans where Fid=" + fid;
                return da.GetOneResult(strSql);
            }
            catch (Exception err)
            {
                return "";
            }
            finally
            {
                da.Dispose();
            }
        }

        public static bool HandleParameter(DataSet ds, bool haveimg)
        {
            //����һ��֤�������.
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CRT"));
            try
            {
                ds.Tables[0].Columns.Add("FTypeName", typeof(String));
                ds.Tables[0].Columns.Add("FStateName", typeof(String));

                ds.Tables[0].Columns.Add("cre_id", typeof(string)); //֤������
                ds.Tables[0].Columns.Add("cre_type", typeof(string)); //֤������
                ds.Tables[0].Columns.Add("cre_image", typeof(string)); //ͼƬ
                ds.Tables[0].Columns.Add("clear_pps", typeof(string)); //1Ϊ����ܱ�
                ds.Tables[0].Columns.Add("reason", typeof(string)); //ԭ��
                ds.Tables[0].Columns.Add("old_name", typeof(string));
                ds.Tables[0].Columns.Add("new_name", typeof(string));
                ds.Tables[0].Columns.Add("old_company", typeof(string));
                ds.Tables[0].Columns.Add("new_company", typeof(string));

                //�����ֻ���ʱ����һ���ֶ�,ԭ����֤����ַ
                ds.Tables[0].Columns.Add("cre_image2", typeof(string)); //ͼƬ
                ds.Tables[0].Columns.Add("question1", typeof(string)); //�ܱ�1
                ds.Tables[0].Columns.Add("answer1", typeof(string)); //��1
                ds.Tables[0].Columns.Add("cont_type", typeof(string)); // 1: �ֻ� 2: email//cont_type=1ʱ,mobile_no��Ч cont_type=2ʱ,femail��Ч
                ds.Tables[0].Columns.Add("mobile_no", typeof(string));
                ds.Tables[0].Columns.Add("labIsAnswer", typeof(string)); //�Ƿ����ܱ�
                ds.Tables[0].Columns.Add("client_ip", typeof(string)); //������IP
                ds.Tables[0].Columns.Add("score", typeof(string)); //ʵ�ʵ÷�
                ds.Tables[0].Columns.Add("standard_score", typeof(string)); //����˱�׼��
                ds.Tables[0].Columns.Add("detail_score", typeof(string)); //�÷���ϸ
                ds.Tables[0].Columns.Add("IsPass", typeof(string)); //���
                ds.Tables[0].Columns.Add("new_cre_id", typeof(string)); //�����֤
                ds.Tables[0].Columns.Add("risk_result", typeof(string)); //��ر��
                ds.Tables[0].Columns.Add("from", typeof(string)); //����������ǣ��ֻ���

                // ��ض���Ҫ������ֶ����£�
                /*
                ds.Tables[0].Columns.Add("email",typeof(string));		// ����
                ds.Tables[0].Columns.Add("pkey",typeof(string));		// ��֤�ʼ�����ʱ����PKEY
                ds.Tables[0].Columns.Add("contact_no",typeof(string));	// �û���ϵ�绰
                ds.Tables[0].Columns.Add("otherImage1",typeof(string));	// ����ͼƬ1
                ds.Tables[0].Columns.Add("otherImage2",typeof(string));	// ����ͼƬ2
                ds.Tables[0].Columns.Add("otherImage3",typeof(string));	// ����ͼƬ3
                ds.Tables[0].Columns.Add("otherImage4",typeof(string));	// ����ͼƬ4
                ds.Tables[0].Columns.Add("otherImage5",typeof(string));	// ����ͼƬ5
                ds.Tables[0].Columns.Add("rec_cftadd",typeof(string));	// ���ʹ�òƸ�ͨ�ĵ�ַ
                ds.Tables[0].Columns.Add("prove_banlance_image",typeof(string));	// ����ʽ���Դ֤��
                ds.Tables[0].Columns.Add("DC_timeaddress",typeof(string));	// �����װ����֤���ʱ��͵�ַ
                ds.Tables[0].Columns.Add("mobile",typeof(string));		// �󶨵��ֻ���
                */


                da.OpenConn();

                // 2012/4/28 �޸ģ���ΪĿǰ�������һ��Ԫ�ز���ʧ��(���������ڸ��ʺ��Ѿ�ע��)����ᵼ�º����dataRow��ѯʧ�ܣ�
                // ��������һ��try catchģ�飬���Բ�ѯʧ�ܵļ�¼
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    try
                    {
                        dr["cre_id"] = "";
                        dr["cre_type"] = "";
                        dr["cre_image"] = "";
                        dr["clear_pps"] = "0";
                        dr["reason"] = "";
                        dr["old_name"] = "";
                        dr["new_name"] = "";
                        dr["old_company"] = "";
                        dr["new_company"] = "";
                        dr["question1"] = "";
                        dr["answer1"] = "";
                        dr["cont_type"] = "";
                        dr["mobile_no"] = "";
                        dr["labIsAnswer"] = "";
                        dr["client_ip"] = "";
                        dr["score"] = "";
                        dr["standard_score"] = "";
                        dr["detail_score"] = "";
                        dr["IsPass"] = "";
                        dr["new_cre_id"] = "";
                        dr["risk_result"] = "";
                        dr["from"] = "";

                        string fuin = dr["FUin"].ToString();

                        string fuid = PublicRes.ConvertToFuid(fuin);
                        if (fuid == null || fuid.Trim() == "")
                        {
                            // 2012/4/28 �޸ģ��û�ע��֮��Ӧ���ǲ鲻����Ӧ��UID��
                            continue;
                            //return false;
                        }

                        string strSql = "select Fattach from " + PublicRes.GetTName("cft_digit_crt", "t_user_attr", fuid)
                            + " where Fuid=" + fuid + " and Fattr=1";

                        string strtmp = QueryInfo.GetString(da.GetOneResult(strSql));
                        dr["cre_image2"] = strtmp;

                        //string parameter = dr["FParameter"].ToString();
                        if (haveimg)
                        {
                            string parameter = GetOneParameter(dr["Fid"].ToString());

                            string[] paramlist = parameter.Split('&');

                            foreach (string param in paramlist)
                            {
                                if (param.StartsWith("cre_id="))
                                {
                                    dr["cre_id"] = getCgiString(param.Replace("cre_id=", ""));
                                }
                                else if (param.StartsWith("cre_type="))
                                {
                                    dr["cre_type"] = getCgiString(param.Replace("cre_type=", ""));
                                }
                                else if (param.StartsWith("cre_image="))
                                {
                                    dr["cre_image"] = getCgiString(param.Replace("cre_image=", ""));
                                }
                                else if (param.StartsWith("clear_pps="))
                                {
                                    dr["clear_pps"] = getCgiString(param.Replace("clear_pps=", ""));
                                }
                                //						else if(param.StartsWith("email"))
                                //						{
                                //							dr["email"] = getCgiString(param.Replace("email=",""));
                                //						}
                                else if (param.StartsWith("reason="))
                                {
                                    dr["reason"] = getCgiString(param.Replace("reason=", ""));
                                }
                                else if (param.StartsWith("old_name="))
                                {
                                    dr["old_name"] = getCgiString(param.Replace("old_name=", ""));
                                }
                                else if (param.StartsWith("new_name="))
                                {
                                    dr["new_name"] = getCgiString(param.Replace("new_name=", ""));
                                }
                                else if (param.StartsWith("old_company="))
                                {
                                    dr["old_company"] = getCgiString(param.Replace("old_company=", ""));
                                }
                                else if (param.StartsWith("new_company="))
                                {
                                    dr["new_company"] = getCgiString(param.Replace("new_company=", ""));
                                }
                                else if (param.StartsWith("question1="))
                                {
                                    dr["question1"] = getCgiString(param.Replace("question1=", ""));
                                }
                                else if (param.StartsWith("answer1="))
                                {
                                    dr["answer1"] = getCgiString(param.Replace("answer1=", ""));
                                }
                                else if (param.StartsWith("cont_type="))
                                {
                                    dr["cont_type"] = getCgiString(param.Replace("cont_type=", ""));
                                }
                                else if (param.StartsWith("mobile_no="))
                                {
                                    dr["mobile_no"] = getCgiString(param.Replace("mobile_no=", ""));
                                }
                                else if (param.StartsWith("ENV_ClientIp="))
                                {
                                    dr["client_ip"] = getCgiString(param.Replace("ENV_ClientIp=", ""));
                                }
                                else if (param.StartsWith("score="))
                                {
                                    dr["score"] = getCgiString(param.Replace("score=", ""));
                                }
                                else if (param.StartsWith("standard_score="))
                                {
                                    dr["standard_score"] = getCgiString(param.Replace("standard_score=", ""));
                                }
                                else if (param.StartsWith("detail_score="))
                                {
                                    dr["detail_score"] = getCgiString(param.Replace("detail_score=", ""));
                                }
                                else if (param.StartsWith("new_cre_id="))
                                {
                                    dr["new_cre_id"] = getCgiString(param.Replace("new_cre_id=", ""));
                                }
                                else if (param.StartsWith("RISK_RESULT="))
                                {
                                    string risk_result = getCgiString(param.Replace("RISK_RESULT=", ""));
                                    if (risk_result == "0")
                                        dr["risk_result"] = "";
                                    else if (risk_result == "1")
                                        dr["risk_result"] = "����쳣�������˹��ط��û�";
                                    else if (risk_result == "2")
                                        dr["risk_result"] = "����쳣�����˹��ط��û�";
                                    else
                                        dr["risk_result"] = risk_result;
                                }
                                else if (param.StartsWith("from="))
                                {
                                    dr["from"] = getCgiString(param.Replace("from=", ""));
                                }

                            }
                        }

                        if (dr["detail_score"].ToString() != "")
                        {
                            try
                            {
                                string detail_score = System.Web.HttpUtility.UrlDecode(dr["detail_score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));

                                if (detail_score.IndexOf("PwdProtection") > -1)
                                {
                                    detail_score = detail_score.Replace("PwdProtection", "�ܱ�У��÷�");
                                }
                                if (detail_score.IndexOf("CertifiedId") > -1)
                                {
                                    detail_score = detail_score.Replace("CertifiedId", "֤����У��÷�");
                                }
                                if (detail_score.IndexOf("bind_email") > -1)
                                {
                                    detail_score = detail_score.Replace("bind_email", "������У��÷�");
                                }
                                if (detail_score.IndexOf("bind_mobile") > -1)
                                {
                                    detail_score = detail_score.Replace("bind_mobile", "���ֻ�У��÷�");
                                }
                                if (detail_score.IndexOf("QQReceipt") > -1)
                                {
                                    detail_score = detail_score.Replace("QQReceipt", "QQ���߻�ִ�ŵ÷�");
                                }
                                if (detail_score.IndexOf("CertifiedBankCard") > -1)
                                {
                                    detail_score = detail_score.Replace("CertifiedBankCard", "ʵ����֤���п���У��÷�");
                                }
                                if (detail_score.IndexOf("CreditCardPayHist") > -1)
                                {
                                    detail_score = detail_score.Replace("CreditCardPayHist", "���ÿ�������ϢУ��÷�");
                                }
                                if (detail_score.IndexOf("WithdrawHist") > -1) //andrew 20110419
                                {
                                    detail_score = detail_score.Replace("WithdrawHist", "���ּ�¼�÷�");
                                }

                                dr["detail_score"] = detail_score;

                            }
                            catch
                            { }
                        }

                        string tmp = dr["FType"].ToString();
                        if (tmp == "1")    //Ŀǰֻ�����һ�֧������÷ֳ���׼������ˣ��Ժ���������ŵ�
                        {
                            dr["FTypeName"] = "�һ�����";

                            if (dr["clear_pps"].ToString() == "0") //�����
                            {
                                string Sql = "uid=" + fuid;
                                string errMsg = "";
                                string answer1 = CommQuery.GetOneResultFromICE(Sql, CommQuery.QUERY_USERINFO, "Fanswer1", out errMsg);
                                if (answer1 != null && answer1.Trim() != "")
                                {
                                    if (dr["answer1"].ToString().Trim() == answer1.ToString().Trim())
                                        dr["labIsAnswer"] = "�����";
                                    else
                                        dr["labIsAnswer"] = "�����";
                                }
                            }

                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "2")
                        {
                            dr["FTypeName"] = "�޸�����";
                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "3")
                        {
                            dr["FTypeName"] = "�޸Ĺ�˾��";
                        }
                        else if (tmp == "4")
                        {
                            dr["FTypeName"] = "ע���ʺ�";
                        }
                        else if (tmp == "5")// andrew ����׼������ 20110419
                        {
                            dr["FTypeName"] = "����ע���û����������ֻ�";

                            try
                            {
                                //IVR���ר��furion  ��Ϊ�и߷ֵ����ᱻ�쵥,�������ﲻ�øĶ�.
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "6")//andrew 20110419
                        {
                            dr["FTypeName"] = "��ע���û��������ֻ�";
                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "7")
                        {
                            dr["FTypeName"] = "����֤����";
                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "9")
                        {
                            dr["FTypeName"] = "�ֻ�����";
                        }
                        else if (tmp == "10")
                        {
                            dr["FTypeName"] = "����������";
                        }
                        else if (tmp == "0")
                        {
                            dr["FTypeName"] = "�Ƹ��ܽ��";
                        }

                        tmp = dr["FState"].ToString();
                        if (tmp == "0")
                        {
                            dr["FStateName"] = "δ����";
                        }
                        else if (tmp == "1")
                        {
                            dr["FStateName"] = "���߳ɹ�";
                        }
                        else if (tmp == "2")
                        {
                            dr["FStateName"] = "����ʧ��";
                        }
                        else if (tmp == "3")
                        {
                            dr["FStateName"] = "��������";
                        }
                        else if (tmp == "4")
                        {
                            dr["FStateName"] = "ֱ��ת��̨";
                        }
                        else if (tmp == "5")
                        {
                            dr["FStateName"] = "�쳣ת��̨";
                        }
                        else if (tmp == "6")
                        {
                            dr["FStateName"] = "���ʼ�ʧ��";
                        }
                        else if (tmp == "7")
                        {
                            dr["FStateName"] = "��ɾ��";
                        }
                        else if (tmp == "8")
                        {
                            dr["FStateName"] = "���쵥";
                        }
                        else if (tmp == "9")
                        {
                            dr["FStateName"] = "���ų�������";
                        }
                    }
                    catch (System.Exception ex)
                    {

                    }
                }

                return true;
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }
        
        //20131107 lxl
        //�ֿ��Ĵ���������������Ľ����ɱ�һ��
        public static bool HandleParameterByDBTB(DataSet ds, bool haveimg)
        {
            //����һ��֤�������.
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CRT"));
            try
            {
                ds.Tables[0].Columns.Add("FTypeName", typeof(String));
                ds.Tables[0].Columns.Add("FStateName", typeof(String));

                ds.Tables[0].Columns.Add("cre_id", typeof(string)); //֤������
                ds.Tables[0].Columns.Add("cre_type", typeof(string)); //֤������
                ds.Tables[0].Columns.Add("cre_image", typeof(string)); //ͼƬ
                ds.Tables[0].Columns.Add("clear_pps", typeof(string)); //1Ϊ����ܱ�
                ds.Tables[0].Columns.Add("reason", typeof(string)); //ԭ��
                ds.Tables[0].Columns.Add("old_name", typeof(string));
                ds.Tables[0].Columns.Add("new_name", typeof(string));
                ds.Tables[0].Columns.Add("old_company", typeof(string));
                ds.Tables[0].Columns.Add("new_company", typeof(string));

                //�����ֻ���ʱ����һ���ֶ�,ԭ����֤����ַ
                ds.Tables[0].Columns.Add("cre_image2", typeof(string)); //ͼƬ
                ds.Tables[0].Columns.Add("question1", typeof(string)); //�ܱ�1
                ds.Tables[0].Columns.Add("answer1", typeof(string)); //��1
                ds.Tables[0].Columns.Add("cont_type", typeof(string)); // 1: �ֻ� 2: email//cont_type=1ʱ,mobile_no��Ч cont_type=2ʱ,femail��Ч
                ds.Tables[0].Columns.Add("mobile_no", typeof(string));
                ds.Tables[0].Columns.Add("labIsAnswer", typeof(string)); //�Ƿ����ܱ�
                ds.Tables[0].Columns.Add("client_ip", typeof(string)); //������IP
                ds.Tables[0].Columns.Add("score", typeof(string)); //ʵ�ʵ÷�
                ds.Tables[0].Columns.Add("standard_score", typeof(string)); //����˱�׼��
                ds.Tables[0].Columns.Add("detail_score", typeof(string)); //�÷���ϸ
                ds.Tables[0].Columns.Add("IsPass", typeof(string)); //���
                ds.Tables[0].Columns.Add("new_cre_id", typeof(string)); //�����֤
                ds.Tables[0].Columns.Add("risk_result", typeof(string)); //��ر��
                ds.Tables[0].Columns.Add("from", typeof(string)); //����������ǣ��ֻ���

                // ��ض���Ҫ������ֶ����£�
                /*
                ds.Tables[0].Columns.Add("email",typeof(string));		// ����
                ds.Tables[0].Columns.Add("pkey",typeof(string));		// ��֤�ʼ�����ʱ����PKEY
                ds.Tables[0].Columns.Add("contact_no",typeof(string));	// �û���ϵ�绰
                ds.Tables[0].Columns.Add("otherImage1",typeof(string));	// ����ͼƬ1
                ds.Tables[0].Columns.Add("otherImage2",typeof(string));	// ����ͼƬ2
                ds.Tables[0].Columns.Add("otherImage3",typeof(string));	// ����ͼƬ3
                ds.Tables[0].Columns.Add("otherImage4",typeof(string));	// ����ͼƬ4
                ds.Tables[0].Columns.Add("otherImage5",typeof(string));	// ����ͼƬ5
                ds.Tables[0].Columns.Add("rec_cftadd",typeof(string));	// ���ʹ�òƸ�ͨ�ĵ�ַ
                ds.Tables[0].Columns.Add("prove_banlance_image",typeof(string));	// ����ʽ���Դ֤��
                ds.Tables[0].Columns.Add("DC_timeaddress",typeof(string));	// �����װ����֤���ʱ��͵�ַ
                ds.Tables[0].Columns.Add("mobile",typeof(string));		// �󶨵��ֻ���
                */


                da.OpenConn();

                // 2012/4/28 �޸ģ���ΪĿǰ�������һ��Ԫ�ز���ʧ��(���������ڸ��ʺ��Ѿ�ע��)����ᵼ�º����dataRow��ѯʧ�ܣ�
                // ��������һ��try catchģ�飬���Բ�ѯʧ�ܵļ�¼
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    try
                    {
                        dr["cre_id"] = "";
                        dr["cre_type"] = "";
                        dr["cre_image"] = "";
                        dr["clear_pps"] = "0";
                        dr["reason"] = "";
                        dr["old_name"] = "";
                        dr["new_name"] = "";
                        dr["old_company"] = "";
                        dr["new_company"] = "";
                        dr["question1"] = "";
                        dr["answer1"] = "";
                        dr["cont_type"] = "";
                        dr["mobile_no"] = "";
                        dr["labIsAnswer"] = "";
                        dr["client_ip"] = "";
                        dr["score"] = "";
                        dr["standard_score"] = "";
                        dr["detail_score"] = "";
                        dr["IsPass"] = "";
                        dr["new_cre_id"] = "";
                        dr["risk_result"] = "";
                        dr["from"] = "";

                        string fuin = dr["FUin"].ToString();

                        string fuid = PublicRes.ConvertToFuid(fuin);
                        if (fuid == null || fuid.Trim() == "")
                        {
                            // 2012/4/28 �޸ģ��û�ע��֮��Ӧ���ǲ鲻����Ӧ��UID��
                            continue;
                            //return false;
                        }

                        string strSql = "select Fattach from " + PublicRes.GetTName("cft_digit_crt", "t_user_attr", fuid)
                            + " where Fuid=" + fuid + " and Fattr=1";

                        string strtmp = QueryInfo.GetString(da.GetOneResult(strSql));
                        dr["cre_image2"] = strtmp;

                        //string parameter = dr["FParameter"].ToString();
                        if (haveimg)
                        {
                                    dr["cre_id"] = getCgiString(dr["FCreId"].ToString());

                                    dr["cre_type"] = getCgiString(dr["FCreType"].ToString());

                                    //dr["cre_image"] = getCgiString(dr["FCreImg1"].ToString()) + "|" + getCgiString(dr["FCreImg2"].ToString());
                                   //20141020 echo �����֤�����Ϊ�ʽ���Դ
                                    dr["cre_image"] = getCgiString(dr["FCreImg1"].ToString()) + "|" + getCgiString(dr["FProveBanlanceImage"].ToString());

                                    dr["clear_pps"] = getCgiString(dr["FClearPps"].ToString());

                                    dr["reason"] = getCgiString(dr["FAppealReason"].ToString());

                                    dr["old_name"] = getCgiString(dr["FOldName"].ToString());

                                    dr["new_name"] = getCgiString(dr["FNewName"].ToString());

                                    dr["old_company"] = getCgiString(dr["FOldCompanyName"].ToString());

                                    dr["new_company"] = getCgiString(dr["FNewCompanyName"].ToString());

                                    dr["question1"] = getCgiString(dr["FMbQuestion"].ToString());

                                    dr["answer1"] = getCgiString(dr["FMbAnswer"].ToString());

                                    dr["cont_type"] = getCgiString(dr["FContType"].ToString());

                                    dr["mobile_no"] = getCgiString(dr["FReservedMobile"].ToString());

                                    dr["client_ip"] = getCgiString(dr["FIp"].ToString());

                                    dr["score"] = getCgiString(dr["FAppealScore"].ToString()); ;

                                    dr["standard_score"] = getCgiString(dr["FStandardScore"].ToString());

                                    dr["detail_score"] = getCgiString(dr["FDetailScore"].ToString());

                                    dr["new_cre_id"] = getCgiString(dr["FNewCreId"].ToString());

                                    string risk_result = getCgiString(dr["FRiskState"].ToString()); 
                                    if (risk_result == "0")
                                        dr["risk_result"] = "";
                                    else if (risk_result == "1")
                                        dr["risk_result"] = "����쳣�������˹��ط��û�";
                                    else if (risk_result == "2")
                                        dr["risk_result"] = "����쳣�����˹��ط��û�";
                                    else
                                        dr["risk_result"] = risk_result;

                                    dr["from"] = getCgiString(dr["FChanel"].ToString());
                          
                        }

                        if (dr["detail_score"].ToString() != "")
                        {
                            try
                            {
                                string detail_score = System.Web.HttpUtility.UrlDecode(dr["detail_score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));

                                if (detail_score.IndexOf("PwdProtection") > -1)
                                {
                                    detail_score = detail_score.Replace("PwdProtection", "�ܱ�У��÷�");
                                }
                                if (detail_score.IndexOf("CertifiedId") > -1)
                                {
                                    detail_score = detail_score.Replace("CertifiedId", "֤����У��÷�");
                                }
                                if (detail_score.IndexOf("bind_email") > -1)
                                {
                                    detail_score = detail_score.Replace("bind_email", "������У��÷�");
                                }
                                if (detail_score.IndexOf("bind_mobile") > -1)
                                {
                                    detail_score = detail_score.Replace("bind_mobile", "���ֻ�У��÷�");
                                }
                                if (detail_score.IndexOf("QQReceipt") > -1)
                                {
                                    detail_score = detail_score.Replace("QQReceipt", "QQ���߻�ִ�ŵ÷�");
                                }
                                if (detail_score.IndexOf("CertifiedBankCard") > -1)
                                {
                                    detail_score = detail_score.Replace("CertifiedBankCard", "ʵ����֤���п���У��÷�");
                                }
                                if (detail_score.IndexOf("CreditCardPayHist") > -1)
                                {
                                    detail_score = detail_score.Replace("CreditCardPayHist", "���ÿ�������ϢУ��÷�");
                                }
                                if (detail_score.IndexOf("WithdrawHist") > -1) //andrew 20110419
                                {
                                    detail_score = detail_score.Replace("WithdrawHist", "���ּ�¼�÷�");
                                }

                                dr["detail_score"] = detail_score;

                            }
                            catch
                            { }
                        }

                        string tmp = dr["FType"].ToString();
                        if (tmp == "1")    //Ŀǰֻ�����һ�֧������÷ֳ���׼������ˣ��Ժ���������ŵ�
                        {
                            dr["FTypeName"] = "�һ�����";

                            if (dr["clear_pps"].ToString() == "0") //�����
                            {
                                string Sql = "uid=" + fuid;
                                string errMsg = "";
                                string answer1 = CommQuery.GetOneResultFromICE(Sql, CommQuery.QUERY_USERINFO, "Fanswer1", out errMsg);
                                if (answer1 != null && answer1.Trim() != "")
                                {
                                    if (dr["answer1"].ToString().Trim() == answer1.ToString().Trim())
                                        dr["labIsAnswer"] = "�����";//�Ƿ����ܱ�
                                    else
                                        dr["labIsAnswer"] = "�����";
                                }
                            }

                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "2")
                        {
                            dr["FTypeName"] = "�޸�����";
                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "3")
                        {
                            dr["FTypeName"] = "�޸Ĺ�˾��";
                        }
                        else if (tmp == "4")
                        {
                            dr["FTypeName"] = "ע���ʺ�";
                        }
                        else if (tmp == "5")// andrew ����׼������ 20110419
                        {
                            dr["FTypeName"] = "����ע���û����������ֻ�";

                            try
                            {
                                //IVR���ר��furion  ��Ϊ�и߷ֵ����ᱻ�쵥,�������ﲻ�øĶ�.
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "6")//andrew 20110419
                        {
                            dr["FTypeName"] = "��ע���û��������ֻ�";
                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "7")
                        {
                            dr["FTypeName"] = "����֤����";
                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "9")
                        {
                            dr["FTypeName"] = "�ֻ�����";
                        }
                        else if (tmp == "10")
                        {
                            dr["FTypeName"] = "����������";
                        }
                        else if (tmp == "0")
                        {
                            dr["FTypeName"] = "�Ƹ��ܽ��";
                        }

                        tmp = dr["FState"].ToString();
                        if (tmp == "0")
                        {
                            dr["FStateName"] = "δ����";
                        }
                        else if (tmp == "1")
                        {
                            dr["FStateName"] = "���߳ɹ�";
                        }
                        else if (tmp == "2")
                        {
                            dr["FStateName"] = "����ʧ��";
                        }
                        else if (tmp == "3")
                        {
                            dr["FStateName"] = "��������";
                        }
                        else if (tmp == "4")
                        {
                            dr["FStateName"] = "ֱ��ת��̨";
                        }
                        else if (tmp == "5")
                        {
                            dr["FStateName"] = "�쳣ת��̨";
                        }
                        else if (tmp == "6")
                        {
                            dr["FStateName"] = "���ʼ�ʧ��";
                        }
                        else if (tmp == "7")
                        {
                            dr["FStateName"] = "��ɾ��";
                        }
                        else if (tmp == "8")
                        {
                            dr["FStateName"] = "���쵥";
                        }
                        else if (tmp == "9")
                        {
                            dr["FStateName"] = "���ų�������";
                        }
                    }
                    catch (System.Exception ex)
                    {

                    }
                }

                return true;
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        //20131107 lxl
        //�ֿ��Ĵ���������������Ľ����ɱ�һ��
        public static bool HandleParameterByDBTBList(DataSet ds, bool haveimg)
        {
            //����һ��֤�������.
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CRT"));
            MySqlAccess da2 = new MySqlAccess(PublicRes.GetConnString("CFTNEW"));
        
            try
            {
                ds.Tables[0].Columns.Add("FTypeName", typeof(String));
                ds.Tables[0].Columns.Add("FStateName", typeof(String));

                ds.Tables[0].Columns.Add("cre_id", typeof(string)); //֤������
                ds.Tables[0].Columns.Add("cre_type", typeof(string)); //֤������
                ds.Tables[0].Columns.Add("cre_image", typeof(string)); //ͼƬ
                ds.Tables[0].Columns.Add("clear_pps", typeof(string)); //1Ϊ����ܱ�
                ds.Tables[0].Columns.Add("reason", typeof(string)); //ԭ��
                ds.Tables[0].Columns.Add("old_name", typeof(string));
                ds.Tables[0].Columns.Add("new_name", typeof(string));
                ds.Tables[0].Columns.Add("old_company", typeof(string));
                ds.Tables[0].Columns.Add("new_company", typeof(string));

                //�����ֻ���ʱ����һ���ֶ�,ԭ����֤����ַ
                ds.Tables[0].Columns.Add("cre_image2", typeof(string)); //ͼƬ
                ds.Tables[0].Columns.Add("question1", typeof(string)); //�ܱ�1
                ds.Tables[0].Columns.Add("answer1", typeof(string)); //��1
                ds.Tables[0].Columns.Add("cont_type", typeof(string)); // 1: �ֻ� 2: email//cont_type=1ʱ,mobile_no��Ч cont_type=2ʱ,femail��Ч
                ds.Tables[0].Columns.Add("mobile_no", typeof(string));
                ds.Tables[0].Columns.Add("labIsAnswer", typeof(string)); //�Ƿ����ܱ�
                ds.Tables[0].Columns.Add("client_ip", typeof(string)); //������IP
                ds.Tables[0].Columns.Add("score", typeof(string)); //ʵ�ʵ÷�
                ds.Tables[0].Columns.Add("standard_score", typeof(string)); //����˱�׼��
                ds.Tables[0].Columns.Add("detail_score", typeof(string)); //�÷���ϸ
                ds.Tables[0].Columns.Add("IsPass", typeof(string)); //���
                ds.Tables[0].Columns.Add("new_cre_id", typeof(string)); //�����֤
                ds.Tables[0].Columns.Add("risk_result", typeof(string)); //��ر��
                ds.Tables[0].Columns.Add("from", typeof(string)); //����������ǣ��ֻ���

                // ��ض���Ҫ������ֶ����£�
                /*
                ds.Tables[0].Columns.Add("email",typeof(string));		// ����
                ds.Tables[0].Columns.Add("pkey",typeof(string));		// ��֤�ʼ�����ʱ����PKEY
                ds.Tables[0].Columns.Add("contact_no",typeof(string));	// �û���ϵ�绰
                ds.Tables[0].Columns.Add("otherImage1",typeof(string));	// ����ͼƬ1
                ds.Tables[0].Columns.Add("otherImage2",typeof(string));	// ����ͼƬ2
                ds.Tables[0].Columns.Add("otherImage3",typeof(string));	// ����ͼƬ3
                ds.Tables[0].Columns.Add("otherImage4",typeof(string));	// ����ͼƬ4
                ds.Tables[0].Columns.Add("otherImage5",typeof(string));	// ����ͼƬ5
                ds.Tables[0].Columns.Add("rec_cftadd",typeof(string));	// ���ʹ�òƸ�ͨ�ĵ�ַ
                ds.Tables[0].Columns.Add("prove_banlance_image",typeof(string));	// ����ʽ���Դ֤��
                ds.Tables[0].Columns.Add("DC_timeaddress",typeof(string));	// �����װ����֤���ʱ��͵�ַ
                ds.Tables[0].Columns.Add("mobile",typeof(string));		// �󶨵��ֻ���
                */


                da.OpenConn();

                // 2012/4/28 �޸ģ���ΪĿǰ�������һ��Ԫ�ز���ʧ��(���������ڸ��ʺ��Ѿ�ע��)����ᵼ�º����dataRow��ѯʧ�ܣ�
                // ��������һ��try catchģ�飬���Բ�ѯʧ�ܵļ�¼
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    try
                    {
                        dr["cre_id"] = "";
                        dr["cre_type"] = "";
                        dr["cre_image"] = "";
                        dr["clear_pps"] = "0";
                        dr["reason"] = "";
                        dr["old_name"] = "";
                        dr["new_name"] = "";
                        dr["old_company"] = "";
                        dr["new_company"] = "";
                        dr["question1"] = "";
                        dr["answer1"] = "";
                        dr["cont_type"] = "";
                        dr["mobile_no"] = "";
                        dr["labIsAnswer"] = "";
                        dr["client_ip"] = "";
                        dr["score"] = "";
                        dr["standard_score"] = "";
                        dr["detail_score"] = "";
                        dr["IsPass"] = "";
                        dr["new_cre_id"] = "";
                        dr["risk_result"] = "";
                        dr["from"] = "";

                        string fuin = dr["FUin"].ToString();

                        string fuid = PublicRes.ConvertToFuid(fuin);
                        if (fuid == null || fuid.Trim() == "")
                        {
                            // 2012/4/28 �޸ģ��û�ע��֮��Ӧ���ǲ鲻����Ӧ��UID��
                            continue;
                            //return false;
                        }

                        string strSql = "select Fattach from " + PublicRes.GetTName("cft_digit_crt", "t_user_attr", fuid)
                            + " where Fuid=" + fuid + " and Fattr=1";

                        string strtmp = QueryInfo.GetString(da.GetOneResult(strSql));
                        dr["cre_image2"] = strtmp;

                        //��ѯ��FCreId��
                        da2.OpenConn();
                        strSql = "select * from " + dr["DBName"].ToString() + "." + dr["tableName"].ToString() + " where FID='" + dr["Fid"]+"'";
                        DataTable dtByFid = da2.GetTable(strSql);
                       
                        if (dtByFid != null && dtByFid.Rows.Count > 0)
                        {
                            DataRow row = dtByFid.Rows[0];
                            if (haveimg)
                            {
                                dr["cre_id"] = getCgiString(row["FCreId"].ToString());

                                dr["cre_type"] = getCgiString(row["FCreType"].ToString());

                                if (getCgiString(row["FCreImg2"].ToString()) != "")
                                {
                                    dr["cre_image"] = getCgiString(row["FCreImg1"].ToString()) + "|" + getCgiString(row["FCreImg2"].ToString());
                                }
                                else
                                {
                                    dr["cre_image"] = getCgiString(row["FCreImg1"].ToString());
                                }

                                dr["clear_pps"] = getCgiString(row["FClearPps"].ToString());

                                dr["reason"] = getCgiString(row["FAppealReason"].ToString());

                                dr["old_name"] = getCgiString(row["FOldName"].ToString());

                                dr["new_name"] = getCgiString(row["FNewName"].ToString());

                                dr["old_company"] = getCgiString(row["FOldCompanyName"].ToString());

                                dr["new_company"] = getCgiString(row["FNewCompanyName"].ToString());

                                dr["question1"] = getCgiString(row["FMbQuestion"].ToString());

                                dr["answer1"] = getCgiString(row["FMbAnswer"].ToString());

                                dr["cont_type"] = getCgiString(row["FContType"].ToString());

                                dr["mobile_no"] = getCgiString(row["FMobile"].ToString());

                                dr["client_ip"] = getCgiString(row["FIp"].ToString());

                                dr["score"] = getCgiString(row["FAppealScore"].ToString()); ;

                                dr["standard_score"] = getCgiString(row["FStandardScore"].ToString());

                                dr["detail_score"] = getCgiString(row["FDetailScore"].ToString());

                                dr["new_cre_id"] = getCgiString(row["FNewCreId"].ToString());

                                string risk_result = getCgiString(row["FRiskState"].ToString());
                                if (risk_result == "0")
                                    dr["risk_result"] = "";
                                else if (risk_result == "1")
                                    dr["risk_result"] = "����쳣�������˹��ط��û�";
                                else if (risk_result == "2")
                                    dr["risk_result"] = "����쳣�����˹��ط��û�";
                                else
                                    dr["risk_result"] = risk_result;

                                dr["from"] = getCgiString(row["FChanel"].ToString());

                            }
                        }

                        if (dr["detail_score"].ToString() != "")
                        {
                            try
                            {
                                string detail_score = System.Web.HttpUtility.UrlDecode(dr["detail_score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));

                                if (detail_score.IndexOf("PwdProtection") > -1)
                                {
                                    detail_score = detail_score.Replace("PwdProtection", "�ܱ�У��÷�");
                                }
                                if (detail_score.IndexOf("CertifiedId") > -1)
                                {
                                    detail_score = detail_score.Replace("CertifiedId", "֤����У��÷�");
                                }
                                if (detail_score.IndexOf("bind_email") > -1)
                                {
                                    detail_score = detail_score.Replace("bind_email", "������У��÷�");
                                }
                                if (detail_score.IndexOf("bind_mobile") > -1)
                                {
                                    detail_score = detail_score.Replace("bind_mobile", "���ֻ�У��÷�");
                                }
                                if (detail_score.IndexOf("QQReceipt") > -1)
                                {
                                    detail_score = detail_score.Replace("QQReceipt", "QQ���߻�ִ�ŵ÷�");
                                }
                                if (detail_score.IndexOf("CertifiedBankCard") > -1)
                                {
                                    detail_score = detail_score.Replace("CertifiedBankCard", "ʵ����֤���п���У��÷�");
                                }
                                if (detail_score.IndexOf("CreditCardPayHist") > -1)
                                {
                                    detail_score = detail_score.Replace("CreditCardPayHist", "���ÿ�������ϢУ��÷�");
                                }
                                if (detail_score.IndexOf("WithdrawHist") > -1) //andrew 20110419
                                {
                                    detail_score = detail_score.Replace("WithdrawHist", "���ּ�¼�÷�");
                                }

                                dr["detail_score"] = detail_score;

                            }
                            catch
                            { }
                        }

                        string tmp = dr["FType"].ToString();
                        if (tmp == "1")    //Ŀǰֻ�����һ�֧������÷ֳ���׼������ˣ��Ժ���������ŵ�
                        {
                            dr["FTypeName"] = "�һ�����";

                            if (dr["clear_pps"].ToString() == "0") //�����
                            {
                                string Sql = "uid=" + fuid;
                                string errMsg = "";
                                string answer1 = CommQuery.GetOneResultFromICE(Sql, CommQuery.QUERY_USERINFO, "Fanswer1", out errMsg);
                                if (answer1 != null && answer1.Trim() != "")
                                {
                                    if (dr["answer1"].ToString().Trim() == answer1.ToString().Trim())
                                        dr["labIsAnswer"] = "�����";//�Ƿ����ܱ�
                                    else
                                        dr["labIsAnswer"] = "�����";
                                }
                            }

                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "2")
                        {
                            dr["FTypeName"] = "�޸�����";
                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "3")
                        {
                            dr["FTypeName"] = "�޸Ĺ�˾��";
                        }
                        else if (tmp == "4")
                        {
                            dr["FTypeName"] = "ע���ʺ�";
                        }
                        else if (tmp == "5")// andrew ����׼������ 20110419
                        {
                            dr["FTypeName"] = "����ע���û����������ֻ�";

                            try
                            {
                                //IVR���ר��furion  ��Ϊ�и߷ֵ����ᱻ�쵥,�������ﲻ�øĶ�.
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "6")//andrew 20110419
                        {
                            dr["FTypeName"] = "��ע���û��������ֻ�";
                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "7")
                        {
                            dr["FTypeName"] = "����֤����";
                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "9")
                        {
                            dr["FTypeName"] = "�ֻ�����";
                        }
                        else if (tmp == "10")
                        {
                            dr["FTypeName"] = "����������";
                        }
                        else if (tmp == "0")
                        {
                            dr["FTypeName"] = "�Ƹ��ܽ��";
                        }

                        tmp = dr["FState"].ToString();
                        if (tmp == "0")
                        {
                            dr["FStateName"] = "δ����";
                        }
                        else if (tmp == "1")
                        {
                            dr["FStateName"] = "���߳ɹ�";
                        }
                        else if (tmp == "2")
                        {
                            dr["FStateName"] = "����ʧ��";
                        }
                        else if (tmp == "3")
                        {
                            dr["FStateName"] = "��������";
                        }
                        else if (tmp == "4")
                        {
                            dr["FStateName"] = "ֱ��ת��̨";
                        }
                        else if (tmp == "5")
                        {
                            dr["FStateName"] = "�쳣ת��̨";
                        }
                        else if (tmp == "6")
                        {
                            dr["FStateName"] = "���ʼ�ʧ��";
                        }
                        else if (tmp == "7")
                        {
                            dr["FStateName"] = "��ɾ��";
                        }
                        else if (tmp == "8")
                        {
                            dr["FStateName"] = "���쵥";
                        }
                        else if (tmp == "9")
                        {
                            dr["FStateName"] = "���ų�������";
                        }
                    }
                    catch (System.Exception ex)
                    {

                    }
                }

                return true;
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
                da2.Dispose();
            }
        }

        public static bool HandleParameter_ForControledFreeze(DataSet ds, bool haveimg)
        {
            //����һ��֤�������.
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CRT"));
            try
            {
                ds.Tables[0].Columns.Add("FTypeName", typeof(String));
                ds.Tables[0].Columns.Add("FStateName", typeof(String));

                ds.Tables[0].Columns.Add("clear_pps", typeof(string));
                ds.Tables[0].Columns.Add("uin", typeof(string));
                ds.Tables[0].Columns.Add("uid", typeof(string));
                ds.Tables[0].Columns.Add("cre_id", typeof(string)); //֤������
                ds.Tables[0].Columns.Add("cre_type", typeof(string)); //֤������
                ds.Tables[0].Columns.Add("cre_image", typeof(string)); //ͼƬ
                ds.Tables[0].Columns.Add("reason", typeof(string)); //ԭ��

                //�����ֻ���ʱ����һ���ֶ�,ԭ����֤����ַ
                ds.Tables[0].Columns.Add("client_ip", typeof(string)); //������IP
                ds.Tables[0].Columns.Add("risk_result", typeof(string)); //��ر��

                // ��ض���Ҫ������ֶ����£�
                ds.Tables[0].Columns.Add("email", typeof(string));		// ����
                ds.Tables[0].Columns.Add("pkey", typeof(string));		// ��֤�ʼ�����ʱ����PKEY
                ds.Tables[0].Columns.Add("contact_no", typeof(string));	// �û���ϵ�绰
                ds.Tables[0].Columns.Add("otherImage1", typeof(string));	// ����ͼƬ1
                ds.Tables[0].Columns.Add("otherImage2", typeof(string));	// ����ͼƬ2
                ds.Tables[0].Columns.Add("otherImage3", typeof(string));	// ����ͼƬ3
                ds.Tables[0].Columns.Add("otherImage4", typeof(string));	// ����ͼƬ4
                ds.Tables[0].Columns.Add("otherImage5", typeof(string));	// ����ͼƬ5
                ds.Tables[0].Columns.Add("rec_cftadd", typeof(string));	// ���ʹ�òƸ�ͨ�ĵ�ַ
                ds.Tables[0].Columns.Add("prove_banlance_image", typeof(string));	// ����ʽ���Դ֤��
                ds.Tables[0].Columns.Add("DC_timeaddress", typeof(string));	// �����װ����֤���ʱ��͵�ַ
                ds.Tables[0].Columns.Add("mobile", typeof(string));		// �󶨵��ֻ���


                da.OpenConn();

                // 2012/4/28 �޸ģ���ΪĿǰ�������һ��Ԫ�ز���ʧ��(���������ڸ��ʺ��Ѿ�ע��)����ᵼ�º����dataRow��ѯʧ�ܣ�
                // ��������һ��try catchģ�飬���Բ�ѯʧ�ܵļ�¼
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    try
                    {
                        dr["uin"] = "";
                        dr["uid"] = "";
                        dr["cre_id"] = "";
                        dr["cre_type"] = "";
                        dr["cre_image"] = "";
                        dr["clear_pps"] = "0";
                        dr["reason"] = "";

                        dr["email"] = "";
                        dr["pkey"] = "";
                        dr["contact_no"] = "";
                        dr["otherImage1"] = "";
                        dr["otherImage2"] = "";
                        dr["otherImage3"] = "";
                        dr["otherImage4"] = "";
                        dr["otherImage5"] = "";
                        dr["rec_cftadd"] = "";
                        dr["prove_banlance_image"] = "";
                        dr["DC_timeaddress"] = "";
                        dr["mobile"] = "";

                        string fuin = dr["FUin"].ToString();

                        string fuid = PublicRes.ConvertToFuid(fuin);
                        if (fuid == null || fuid.Trim() == "")
                        {
                            // 2012/4/28 �޸ģ��û�ע��֮��Ӧ���ǲ鲻����Ӧ��UID��
                            continue;
                            //return false;
                        }

                        //string parameter = dr["FParameter"].ToString();
                        if (haveimg)
                        {
                            string parameter = GetOneParameter(dr["Fid"].ToString());

                            string[] paramlist = parameter.Split('&');

                            foreach (string param in paramlist)
                            {
                                if (param.StartsWith("uin="))
                                {
                                    dr["uin"] = getCgiString(param.Replace("uin=", ""));
                                }
                                else if (param.StartsWith("uid="))
                                {
                                    dr["uid"] = getCgiString(param.Replace("uid=", ""));
                                }
                                if (param.StartsWith("cre_id="))
                                {
                                    dr["cre_id"] = getCgiString(param.Replace("cre_id=", ""));
                                }
                                else if (param.StartsWith("cre_type="))
                                {
                                    dr["cre_type"] = getCgiString(param.Replace("cre_type=", ""));
                                }
                                else if (param.StartsWith("cre_image="))
                                {
                                    dr["cre_image"] = getCgiString(param.Replace("cre_image=", ""));
                                }
                                else if (param.StartsWith("clear_pps="))
                                {
                                    dr["clear_pps"] = getCgiString(param.Replace("clear_pps=", ""));
                                }
                                else if (param.StartsWith("email"))
                                {
                                    dr["email"] = getCgiString(param.Replace("email=", ""));
                                }
                                else if (param.StartsWith("reason="))
                                {
                                    dr["reason"] = getCgiString(param.Replace("reason=", ""));
                                }

                                else if (param.StartsWith("pkey="))
                                {
                                    dr["pkey"] = getCgiString(param.Replace("pkey=", ""));
                                }
                                else if (param.StartsWith("mobile="))
                                {
                                    dr["mobile"] = getCgiString(param.Replace("mobile=", ""));
                                }
                                else if (param.StartsWith("contact_no="))
                                {
                                    dr["contact_no"] = getCgiString(param.Replace("contact_no=", ""));
                                }
                                else if (param.StartsWith("rec_cftadd="))
                                {
                                    dr["rec_cftadd"] = getCgiString(param.Replace("rec_cftadd=", ""));
                                }
                                else if (param.StartsWith("prove_banlance_image="))
                                {
                                    dr["prove_banlance_image"] = getCgiString(param.Replace("prove_banlance_image=", ""));
                                }
                                else if (param.StartsWith("other1_image="))
                                {
                                    dr["otherImage1"] = getCgiString(param.Replace("other1_image=", ""));
                                }
                                else if (param.StartsWith("other2_image="))
                                {
                                    dr["otherImage2"] = getCgiString(param.Replace("other2_image=", ""));
                                }
                                else if (param.StartsWith("other3_image="))
                                {
                                    dr["otherImage3"] = getCgiString(param.Replace("other3_image=", ""));
                                }
                                else if (param.StartsWith("other4_image="))
                                {
                                    dr["otherImage4"] = getCgiString(param.Replace("other4_image=", ""));
                                }
                                else if (param.StartsWith("other5_image="))
                                {
                                    dr["otherImage5"] = getCgiString(param.Replace("other5_image=", ""));
                                }
                                else if (param.StartsWith("DC_timeaddress="))
                                {
                                    dr["DC_timeaddress"] = getCgiString(param.Replace("DC_timeaddress=", ""));
                                }
                            }
                        }

                        string tmp = dr["FState"].ToString();
                        if (tmp == "0")
                        {
                            dr["FStateName"] = "δ����";
                        }
                        else if (tmp == "1")
                        {
                            dr["FStateName"] = "���߳ɹ�";
                        }
                        else if (tmp == "2")
                        {
                            dr["FStateName"] = "����ʧ��";
                        }
                        else if (tmp == "3")
                        {
                            dr["FStateName"] = "��������";
                        }
                        else if (tmp == "4")
                        {
                            dr["FStateName"] = "ֱ��ת��̨";
                        }
                        else if (tmp == "5")
                        {
                            dr["FStateName"] = "�쳣ת��̨";
                        }
                        else if (tmp == "6")
                        {
                            dr["FStateName"] = "���ʼ�ʧ��";
                        }
                        else if (tmp == "7")
                        {
                            dr["FStateName"] = "��ɾ��";
                        }
                        else if (tmp == "8")
                        {
                            dr["FStateName"] = "���쵥";
                        }
                        else if (tmp == "9")
                        {
                            dr["FStateName"] = "���ų�������";
                        }
                    }
                    catch (Exception ex)
                    {
                        string str = ex.Message;
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return false;
            }

            return true;
        }

        public static void InputAppealNumber(string User, string Type, string OperationType)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                da.OpenConn();
                string sql = "select count(1) from t_tenpay_appeal_kf_total where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType='" + OperationType + "'";
                if (da.GetOneResult(sql) == "1")
                {
                    if (Type == "Success")
                        sql = "update t_tenpay_appeal_kf_total set SuccessNum = SuccessNum + 1 where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType='" + OperationType + "'";
                    else if (Type == "Fail")
                        sql = "update t_tenpay_appeal_kf_total set FailNum = FailNum + 1 where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType='" + OperationType + "'";
                    else if (Type == "Delete")
                        sql = "update t_tenpay_appeal_kf_total set DeleteNum = DeleteNum + 1 where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType='" + OperationType + "'";
                    else
                        sql = "update t_tenpay_appeal_kf_total set OtherNum = OtherNum + 1 where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType='" + OperationType + "'";
                }
                else
                {
                    if (Type == "Success")
                        sql = "insert into t_tenpay_appeal_kf_total(User,OperationDay,SuccessNum,OperationType) values('" + User + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "',1,'" + OperationType + "')";
                    else if (Type == "Fail")
                        sql = "insert into t_tenpay_appeal_kf_total(User,OperationDay,FailNum,OperationType) values('" + User + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "',1,'" + OperationType + "')";
                    else if (Type == "Delete")
                        sql = "insert into t_tenpay_appeal_kf_total(User,OperationDay,DeleteNum,OperationType) values('" + User + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "',1,'" + OperationType + "')";
                    else
                        sql = "insert into t_tenpay_appeal_kf_total(User,OperationDay,OtherNum,OperationType) values('" + User + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "',1,'" + OperationType + "')";
                }

                da.ExecSqlNum(sql);
            }
            catch
            {
                throw new Exception("��¼����ͳ��ʧ�ܣ�");
            }
            finally
            {
                da.Dispose();
            }
        }

        public CFTUserAppealClass(string fuin, string u_BeginTime, string u_EndTime, int fstate, int ftype, string QQType, string dotype, int SortType)
        {
            string strWhere = " where 1=1 ";

            if (fuin != null && fuin.Trim() != "")
            {
                strWhere += " and Fuin='" + fuin.Trim() + "' ";
            }

            if (u_BeginTime != null && u_BeginTime != "")
            {

                strWhere += " and FSubmitTime between '" + u_BeginTime + "' and '" + u_EndTime + "' ";
            }

            if (fstate != 99)
            {
                if (fstate == 10)   //ֱ�����߳ɹ�������˳ɹ�����FCheckUserΪsystem
                    strWhere += " and FState='1' and FCheckUser='system' ";
                else
                    strWhere += " and FState='" + fstate + "'  ";
            }


            if (ftype != 99)
            {
                strWhere += " and FType=" + ftype + " ";
            }
            else
            {
                // 8�������Ƿ�ض��ᣬ������Ĵ�����ڣ����ߴ����ݲ����������͵�����
                strWhere += " and FType!=8 and FType!=19 ";
            }

            if (QQType == "0")    //"" ��������; "0" �ǻ�Ա; "1" ��ͨ��Ա; "2" VIP��Ա
            {
                strWhere += " and (FParameter not like '%vip_flag=1%') and (FParameter not like '%vip_flag=2%') ";
            }
            else if (QQType == "1")
            {
                strWhere += " and FParameter like '%vip_flag=1%' ";
            }
            else if (QQType == "2")
            {
                strWhere += " and FParameter like '%vip_flag=2%' ";
            }

            //���Ӹ߷ֵ��ͷֵ���ѯ
            if (dotype == "1")
            {
                //�߷�
                strWhere += " and FParameter like '%&AUTO_APPEAL=1%' ";
            }
            else if (dotype == "0")
            {
                //�ͷ�
                strWhere += " and FParameter not like '%&AUTO_APPEAL=1%' ";
            }
            
            if (SortType != 99)
            {
                if (SortType == 0)   //����ʱ��С����
                    strWhere += " order by FSubmitTime asc ";
                if (SortType == 1)   //����ʱ���С
                    strWhere += " order by FSubmitTime desc ";
            }

            //lxl 20131116������tableName��DBName
            fstrSql = "select '' as DBName, '' as tableName,Fid,FType,Fuin,FSubmitTime,FState,FCheckTime,FReCheckTime,Fpicktime,FReCheckUser,FCheckInfo,FCheckUser,FComment,Femail from t_tenpay_appeal_trans "
                + strWhere;
            fstrSql_count = "select count(1) from t_tenpay_appeal_trans " + strWhere;
        }

        public CFTUserAppealClass(string fuin, string u_BeginTime, string u_EndTime, int fstate, int ftype, string QQType, string dotype, int SortType, bool mark)
        {
            string strWhere = " where 1=1 ";

            if (fuin != null && fuin.Trim() != "")
            {
                strWhere += " and Fuin='" + fuin.Trim() + "' ";
            }

            if (u_BeginTime != null && u_BeginTime != "")
            {

                strWhere += " and FSubmitTime between '" + u_BeginTime + "' and '" + u_EndTime + "' ";
            }

            if (fstate != 99)
            {
                if (fstate == 10)   //ֱ�����߳ɹ�������˳ɹ�����FCheckUserΪsystem
                    strWhere += " and FState='1' and FCheckUser='system' ";
                else
                    strWhere += " and FState='" + fstate + "'  ";
            }


            if (ftype != 99)
            {
                strWhere += " and FType=" + ftype + " ";
            }
            else
            {
                // 8�������Ƿ�ض��ᣬ������Ĵ�����ڣ����ߴ����ݲ����������͵�����
                strWhere += " and FType!=8 ";
            }

            if (QQType == "0")    //"" ��������; 0 �ǻ�Ա; 1 ��ͨ��Ա; 2 VIP��Ա
            {
                strWhere += " and FVipFlag<>1 and FVipFlag<>2 ";
            }
            else if (QQType == "1")
            {
                strWhere += " and FVipFlag=1 ";
            }
            else if (QQType == "2")
            {
                strWhere += " and FVipFlag=2 ";
            }

            //���Ӹ߷ֵ��ͷֵ���ѯ
            if (dotype == "1")
            {
                //�߷�
                strWhere += " and FAutoAppeal=1 ";
            }
            else if (dotype == "0")
            {
                //�ͷ�
                strWhere += " and FAutoAppeal<>1 ";
            }
            fstrSql = "";
            fstrSql_count = "";
            string str = " Fid,FType,Fuin,FSubmitTime,FState,FCheckTime,FReCheckTime,Fpicktime,FReCheckUser,FCheckInfo,FCheckUser,FComment,Femail from db_appeal_";

            int yearBegin = DateTime.Parse(u_BeginTime).Year;
            int monBegin = DateTime.Parse(u_BeginTime).Month;
            int yearEnd = DateTime.Parse(u_EndTime).Year;
            int monEnd = DateTime.Parse(u_EndTime).Month;

            while (yearBegin <= yearEnd)
            {
                if (yearBegin < 2014)
                {
                    yearBegin++;
                    continue;
                }
                for (int i = 1; i <= 12; i++)//�·�
                {
                    if (yearBegin == yearEnd && i > monEnd)//�鵽��������
                        break;
                    if (i < monBegin)//С�ڿ�ʼ�·ݵ����ݲ���
                        continue;

                    if (fstrSql != "")
                        fstrSql += "union all ";
                    if (i < 10)
                        fstrSql += " select 'db_appeal_" + yearBegin + "' as DBName,'t_tenpay_appeal_trans_" + "0" + i + "' as tableName," + str + yearBegin + ".t_tenpay_appeal_trans_" + "0" + i + strWhere;
                    else
                        fstrSql += " select 'db_appeal_" + yearBegin + "' as DBName,'t_tenpay_appeal_trans_" + i + "' as tableName," + str + yearBegin + ".t_tenpay_appeal_trans_" + i + strWhere;

                }
                yearBegin++;
                monBegin = 0;//����ݺ󣬽���ʼ�·���Ϊ0
            }
            fstrSql_count = "select count(1) from ( " + fstrSql + " ) as total";
        }

        public CFTUserAppealClass(string fuin, string u_BeginTime, string u_EndTime, int fstate, int ftype, string QQType, string dotype, int SortType, bool mark,string db,string tb)
        {
            string strWhere = " where 1=1 ";

            if (fuin != null && fuin.Trim() != "")
            {
                strWhere += " and Fuin='" + fuin.Trim() + "' ";
            }

            if (u_BeginTime != null && u_BeginTime != "")
            {

                strWhere += " and FSubmitTime between '" + u_BeginTime + "' and '" + u_EndTime + "' ";
            }

            if (fstate != 99)
            {
                if (fstate == 10)   //ֱ�����߳ɹ�������˳ɹ�����FCheckUserΪsystem
                    strWhere += " and FState='1' and FCheckUser='system' ";
                else
                    strWhere += " and FState='" + fstate + "'  ";
            }


            if (ftype != 99)
            {
                strWhere += " and FType=" + ftype + " ";
            }
            else
            {
                // 8�������Ƿ�ض��ᣬ������Ĵ�����ڣ����ߴ����ݲ����������͵�����
                strWhere += " and FType!=8  and FType!=19 ";
            }

            if (QQType == "0")    //"" ��������; 0 �ǻ�Ա; 1 ��ͨ��Ա; 2 VIP��Ա
            {
                strWhere += " and FVipFlag<>1 and FVipFlag<>2 ";
            }
            else if (QQType == "1")
            {
                strWhere += " and FVipFlag=1 ";
            }
            else if (QQType == "2")
            {
                strWhere += " and FVipFlag=2 ";
            }

            //���Ӹ߷ֵ��ͷֵ���ѯ
            if (dotype == "1")
            {
                //�߷�
                strWhere += " and FAutoAppeal=1 ";
            }
            else if (dotype == "0")
            {
                //�ͷ�
                strWhere += " and FAutoAppeal<>1 ";
            }
            fstrSql = "";
            fstrSql_count = "";
            string str = " Fid,FType,Fuin,FSubmitTime,FState,FCheckTime,FReCheckTime,Fpicktime,FReCheckUser,FCheckInfo,FCheckUser,FComment,Femail from db_appeal_";

            if (int.Parse(tb) < 10)
                fstrSql += " select 'db_appeal_" + db + "' as DBName,'t_tenpay_appeal_trans_" + "0" + tb + "' as tableName," + str + db + ".t_tenpay_appeal_trans_" + "0" + tb + strWhere;
            else
                fstrSql += " select 'db_appeal_" + db + "' as DBName,'t_tenpay_appeal_trans_" + tb + "' as tableName," + str + db + ".t_tenpay_appeal_trans_" + tb + strWhere;
            fstrSql_count = "select count(1) from ( " + fstrSql + " ) as total";
        }

        public CFTUserAppealClass(string fuin, string u_BeginTime, string u_EndTime, int fstate, int ftype, string QQType,
            string pickUser, string fid, string szReason, string orderType)
        {
            string strWhere = " where 1=1 ";

            if (fuin != null && fuin.Trim() != "")
            {
                strWhere += " and Fuin='" + fuin.Trim() + "' ";
            }

            if (u_BeginTime != null && u_BeginTime != "")
            {

                strWhere += " and FSubmitTime between '" + u_BeginTime + "' and '" + u_EndTime + "' ";
            }

            if (fstate != 99)
            {
                if (fstate == 10)   //ֱ�����߳ɹ�������˳ɹ�����FCheckUserΪsystem
                    strWhere += " and FState='1' and FCheckUser='system' ";
                else
                    strWhere += " and FState='" + fstate + "'  ";
            }

            if (ftype != 99)
            {
                strWhere += " and FType=" + ftype + " ";
            }
            else
            {
                // 8�������Ƿ�ض��ᣬ������Ĵ�����ڣ����ߴ����ݲ����������͵�����
                strWhere += " and FType!=8 ";
            }

            if (QQType == "0")    //"" ��������; "0" �ǻ�Ա; "1" ��ͨ��Ա; "2" VIP��Ա
            {
                strWhere += " and (FParameter not like '%vip_flag=1%') and (FParameter not like '%vip_flag=2%') ";
            }
            else if (QQType == "1")
            {
                strWhere += " and FParameter like '%vip_flag=1%' ";
            }
            else if (QQType == "2")
            {
                strWhere += " and FParameter like '%vip_flag=2%' ";
            }

            if (pickUser != "")
            {
                strWhere += " and fpickuser like '%" + pickUser + "%' ";
            }

            if (fid != "")
            {
                strWhere += " and Fid=" + fid;
            }

            if (szReason != "")
            {
                strWhere += " and FComment like '%" + szReason + "%' ";
            }

            // Ĭ�ϰ����ύʱ����������
            //string orderStr = " order by date_format(FSubmitTime,'%Y%m%d'),FUin asc";
            string orderStr = " order by date_format(FSubmitTime,'%Y%m%d') asc";

            if (orderType == "2")
            {
                orderStr = "order by date_format(FSubmitTime,'%Y%m%d') desc";
            }

            fstrSql = "select Fid,FType,Fuin,FSubmitTime,FState,FCheckTime,Fpicktime,FCheckInfo,FCheckUser,FComment,Femail,FPickUser from t_tenpay_appeal_trans "
                + strWhere + orderStr;

            fstrSql_count = "select count(1) from t_tenpay_appeal_trans " + strWhere;
        }

        //yinhuang 2014/02/13 �ֿ�ֱ�
        public CFTUserAppealClass(string fuin, string u_BeginTime, string u_EndTime, int fstate, int ftype, string QQType,
            string pickUser, string fid, string szReason, string orderType, string table)
        {
            string strWhere = " where 1=1 ";

            if (fuin != null && fuin.Trim() != "")
            {
                strWhere += " and Fuin='" + fuin.Trim() + "' ";
            }

            if (u_BeginTime != null && u_BeginTime != "")
            {

                strWhere += " and FSubmitTime between '" + u_BeginTime + "' and '" + u_EndTime + "' ";
            }

            if (fstate != 99)
            {
                if (fstate == 10)   //ֱ�����߳ɹ�������˳ɹ�����FCheckUserΪsystem
                {
                    if (ftype == 8)
                    {
                        strWhere += " and FState='" + fstate + "'  ";
                    }
                    else 
                    {
                        strWhere += " and FState='1' and FCheckUser='system' ";
                    } 
                }
                else
                {
                    strWhere += " and FState='" + fstate + "'  ";
                }
            }

            if (ftype != 99)
            {
                //if (ftype == 8)
                //{
                //    strWhere += " and (FType=8 or FType=19) ";
                //}
                //else 
                //{
                    strWhere += " and FType=" + ftype + " ";
                //} 
            }

            if (QQType == "0")    //"" ��������; "0" �ǻ�Ա; "1" ��ͨ��Ա; "2" VIP��Ա
            {
                strWhere += " and (FParameter not like '%vip_flag=1%') and (FParameter not like '%vip_flag=2%') ";
            }
            else if (QQType == "1")
            {
                strWhere += " and FParameter like '%vip_flag=1%' ";
            }
            else if (QQType == "2")
            {
                strWhere += " and FParameter like '%vip_flag=2%' ";
            }

            if (pickUser != "")
            {
                strWhere += " and fpickuser like '%" + pickUser + "%' ";
            }

            if (fid != "")
            {
                strWhere += " and Fid='" + fid+"'";
            }

            if (szReason != "")
            {
                strWhere += " and FComment like '%" + szReason + "%' ";
            }

            // Ĭ�ϰ����ύʱ����������
            //string orderStr = " order by date_format(FSubmitTime,'%Y%m%d'),FUin asc";
            string orderStr = " order by date_format(FSubmitTime,'%Y%m%d') asc";

            if (orderType == "2")
            {
                orderStr = "order by date_format(FSubmitTime,'%Y%m%d') desc";
            }

            fstrSql = "select Fid,FType,Fuin,FSubmitTime,FState,FCheckTime,Fpicktime,FCheckInfo,FCheckUser,FComment,Femail,FPickUser from " + table+" "
                + strWhere + orderStr;

            fstrSql_count = "select count(1) from "+ table+" " + strWhere;
        }

        //ֱ��ͨ���Ĺ��캯��
        public CFTUserAppealClass(string u_BeginTime, string u_EndTime, string ftypes)
        {
            string strWhere = " where FSubmitTime between '" + u_BeginTime + "' and '" + u_EndTime + "' and FState='0' and FType in(" + ftypes + ")";

            fstrSql = "select Fid,FType,Fuin,FSubmitTime,FState,FCheckTime,Fpicktime,FCheckInfo,FCheckUser,FComment,Femail from t_tenpay_appeal_trans " + strWhere;
            fstrSql_count = "select count(1) from t_tenpay_appeal_trans " + strWhere;
        }
        public CFTUserAppealClass(int fid)
        {
            //fstrSql = "select Fid,FType,Fuin,FSubmitTime,FState,Fpicktime,FCheckTime,FCheckInfo,FCheckUser,FComment,Femail from t_tenpay_appeal_trans where FID=" + fid ;
            fstrSql = "select * from t_tenpay_appeal_trans where FID=" + fid;
            fstrSql_count = "select count(1) from t_tenpay_appeal_trans where FID=" + fid;
        }

        //yinhuang 2014/02/12 �ⶳ���ߵ��ֿ�ֱ�
        public CFTUserAppealClass(string fid, string table)
        {
            //fstrSql = "select Fid,FType,Fuin,FSubmitTime,FState,Fpicktime,FCheckTime,FCheckInfo,FCheckUser,FComment,Femail from t_tenpay_appeal_trans where FID=" + fid ;
            fstrSql = "select * from "+table+" where FID='" + fid+"'";
            fstrSql_count = "select count(1) from "+table+" where FID='" + fid+"'";
        }

        //20131107 lxl ��ѯ�ֿ��
        public CFTUserAppealClass(string fid, string db, string tb,string mark)
        {
            //fstrSql = "select Fid,FType,Fuin,FSubmitTime,FState,Fpicktime,FCheckTime,FCheckInfo,FCheckUser,FComment,Femail from t_tenpay_appeal_trans where FID=" + fid ;
            fstrSql = "select * from "+db+"."+tb+ " where FID='" + fid+"'";
            fstrSql_count = "select count(1) from " + db + "." + tb + " where FID='" + fid + "'";
        }

        public static DataSet GetLockList(DateTime BeginDate, DateTime EndDate, string fstate, string ftype, string QQType, string username, int Count,int SortType)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            DataSet ds = new DataSet();

            try
            {
                da.OpenConn();

                string strSql = "select * from t_tenpay_appeal_trans where FsubmitTime >= '" + BeginDate.ToString("yyyy-MM-dd 00:00:00") + "' and FsubmitTime <= '" + EndDate.ToString("yyyy-MM-dd 23:59:59") + "'";

                if (username == null || username == "")
                {
                    throw new Exception("�����쵥�˲�����Ϊ��!");
                }

                if (fstate == "99")
                {
                    strSql += " and Fstate in(0,3,4,5,6,8) ";		// 2012/4/25  ��Ϊ����8Ҳ�����ˣ���������Ӧ����Ӱɣ�
                }
                else if (fstate == "0" || fstate == "3" || fstate == "4" || fstate == "5" || fstate == "6" || fstate == "8")  //����8Ҳ������
                {
                    strSql += " and Fstate = " + fstate;
                }
                else
                {
                    throw new Exception("������״̬�����������쵥!");
                }
                if (ftype == "99")
                {
                    strSql += " and FType in(1,2,3,4,5,6,7,0,9,10)";//20130923 lxl �����Ƹ�ͨ����ֻ����ơ����������ơ����������쵥������
                }
                // 2012/4/2 �����ftype=��7�������������조����֤�����롱��
                else if (ftype == "1" || ftype == "2" || ftype == "3" || ftype == "4" || ftype == "5" || ftype == "6" || ftype == "7" || ftype == "0" || ftype == "9" || ftype == "10")
                {
                    strSql += " and FType = " + ftype;
                }
                else
                {
                    throw new Exception("���������Ͳ����������쵥!");
                }

                if (QQType == "0")    //"" ��������; "0" �ǻ�Ա; "1" ��ͨ��Ա; "2" VIP��Ա
                {
                    strSql += " and (FParameter not like '%vip_flag=1%') and (FParameter not like '%vip_flag=2%') ";
                }
                else if (QQType == "1")
                {
                    strSql += " and FParameter like '%vip_flag=1%' ";
                }
                else if (QQType == "2")
                {
                    strSql += " and FParameter like '%vip_flag=2%' ";
                }

                if (SortType != 99)
                {
                    if (SortType == 0)   //����ʱ��С����
                        strSql += " order by FSubmitTime asc ";
                    if (SortType == 1)   //����ʱ���С
                        strSql += " order by FSubmitTime desc ";
                }

                strSql += " limit " + Count;

                ds = da.dsGetTotalData(strSql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    HandleParameter(ds, true);

                    string WhereStr = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["IsPass"].ToString() == "Y")
                        {
                            dr.Delete();
                        }
                        else
                            WhereStr += "'" + dr["Fid"].ToString().Trim() + "',";
                    }
                    ds.AcceptChanges();

                    WhereStr = WhereStr.Substring(0, WhereStr.Length - 1);

                    strSql = " update t_tenpay_appeal_trans set FPickUser='" + username + "',FPickTime=now(),Fstate=8 where Fid in(" + WhereStr + ")";

                    da.ExecSql(strSql);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                da.Dispose();
            }

            return ds;
        }

        //�����쵥�����������;ɱ�����
        public CFTUserAppealClass(DateTime BeginDate, DateTime EndDate, string fstate, string ftype, string QQType, string username, int Count, int SortType)
        {
            fstrSql = "select '' as DBName, '' as tableName, Fid,FType,Fuin,FSubmitTime,FState,FCheckTime,FReCheckTime,Fpicktime,FReCheckUser,FCheckInfo,FCheckUser,FComment,Femail from t_tenpay_appeal_trans where FsubmitTime >= '" + BeginDate.ToString("yyyy-MM-dd 00:00:00") + "' and FsubmitTime <= '" + EndDate.ToString("yyyy-MM-dd 23:59:59") + "'";

            if (username == null || username == "")
            {
                throw new Exception("�����쵥�˲�����Ϊ��!");
            }

            if (fstate == "99")
            {
                fstrSql += " and Fstate in(0,3,4,5,6,8) ";		// 2012/4/25  ��Ϊ����8Ҳ�����ˣ���������Ӧ����Ӱɣ�
            }
            else if (fstate == "0" || fstate == "3" || fstate == "4" || fstate == "5" || fstate == "6" || fstate == "8")  //����8Ҳ������
            {
                fstrSql += " and Fstate = " + fstate;
            }
            else
            {
                throw new Exception("������״̬�����������쵥!");
            }
            if (ftype == "99")
            {
                fstrSql += " and FType in(1,2,3,4,5,6,7,0,9,10)";//20130923 lxl �����Ƹ�ͨ����ֻ����ơ����������ơ����������쵥������
            }
            // 2012/4/2 �����ftype=��7�������������조����֤�����롱��
            else if (ftype == "1" || ftype == "2" || ftype == "3" || ftype == "4" || ftype == "5" || ftype == "6" || ftype == "7" || ftype == "0" || ftype == "9" || ftype == "10")
            {
                fstrSql += " and FType = " + ftype;
            }
            else
            {
                throw new Exception("���������Ͳ����������쵥!");
            }

            if (QQType == "0")    //"" ��������; "0" �ǻ�Ա; "1" ��ͨ��Ա; "2" VIP��Ա
            {
                fstrSql += " and (FParameter not like '%vip_flag=1%') and (FParameter not like '%vip_flag=2%') ";
            }
            else if (QQType == "1")
            {
                fstrSql += " and FParameter like '%vip_flag=1%' ";
            }
            else if (QQType == "2")
            {
                fstrSql += " and FParameter like '%vip_flag=2%' ";
            }

            if (SortType != 99)
            {
                if (SortType == 0)   //����ʱ��С����
                    fstrSql += " order by FSubmitTime asc ";
                if (SortType == 1)   //����ʱ���С
                    fstrSql += " order by FSubmitTime desc ";
            }

            //    strSql += " limit " + Count;

            //ds = da.dsGetTotalData(strSql);

            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    HandleParameter(ds, true);

            //    string WhereStr = "";
            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        if (dr["IsPass"].ToString() == "Y")
            //        {
            //            dr.Delete();
            //        }
            //        else
            //            WhereStr += "'" + dr["Fid"].ToString().Trim() + "',";
            //    }
            //    ds.AcceptChanges();

            //    WhereStr = WhereStr.Substring(0, WhereStr.Length - 1);

            //    strSql = " update t_tenpay_appeal_trans set FPickUser='" + username + "',FPickTime=now(),Fstate=8 where Fid in(" + WhereStr + ")";

            //    da.ExecSql(strSql);

            //    fstrSql = "";
            //    fstrSql_count = "";
            //}


            // return ds;
        }

        //�����쵥�����������ͷֿ������
        public CFTUserAppealClass(DateTime BeginDate, DateTime EndDate, string fstate, string ftype, string QQType, string username, int Count, int SortType, bool mark)
        {
            string strWhere = " where FsubmitTime >= '" + BeginDate.ToString("yyyy-MM-dd 00:00:00") + "' and FsubmitTime <= '" + EndDate.ToString("yyyy-MM-dd 23:59:59") + "'";
            if (username == null || username == "")
            {
                throw new Exception("�����쵥�˲�����Ϊ��!");
            }

            if (fstate == "99")
            {
                strWhere += " and Fstate in(0,3,4,5,6,8) ";		// 2012/4/25  ��Ϊ����8Ҳ�����ˣ���������Ӧ����Ӱɣ�
            }
            else if (fstate == "0" || fstate == "3" || fstate == "4" || fstate == "5" || fstate == "6" || fstate == "8")  //����8Ҳ������
            {
                strWhere += " and Fstate = " + fstate;
            }
            else
            {
                throw new Exception("������״̬�����������쵥!");
            }
            if (ftype == "99")
            {
                strWhere += " and FType in(1,2,3,4,5,6,7,0,9,10)";//20130923 lxl �����Ƹ�ͨ����ֻ����ơ����������ơ����������쵥������
            }
            // 2012/4/2 �����ftype=��7�������������조����֤�����롱��
            else if (ftype == "1" || ftype == "2" || ftype == "3" || ftype == "4" || ftype == "5" || ftype == "6" || ftype == "7" || ftype == "0" || ftype == "9" || ftype == "10")
            {
                strWhere += " and FType = " + ftype;
            }
            else
            {
                throw new Exception("���������Ͳ����������쵥!");
            }

            if (QQType == "0")    //"" ��������; "0" �ǻ�Ա; "1" ��ͨ��Ա; "2" VIP��Ա
            {
                strWhere += " and FVipFlag<>1 and FVipFlag<>2 ";
            }
            else if (QQType == "1")
            {
                strWhere += " and FVipFlag=1 ";
            }
            else if (QQType == "2")
            {
                strWhere += " and FVipFlag=2 ";
            }

            fstrSql = "";
            fstrSql_count = "";
            string str = " Fid,FType,Fuin,FSubmitTime,FState,FCheckTime,FReCheckTime,Fpicktime,FReCheckUser,FCheckInfo,FCheckUser,FComment,Femail from db_appeal_";
            
            int yearBegin = BeginDate.Year;
            int monBegin = BeginDate.Month;
            int yearEnd = EndDate.Year;
            int monEnd = EndDate.Month;
            if (yearEnd < 2014)//���ݿ�������
            {
                fstrSql = "";
                fstrSql_count = "";
            }
            else
            {
                while (yearBegin <= yearEnd)
                {
                    if (yearBegin < 2014)
                    {
                        yearBegin++;
                        continue;
                    }
                    for (int i = 1; i <= 12; i++)//�·�
                    {
                        if (yearBegin == yearEnd && i > monEnd)//�鵽��������
                            break;
                        if (i < monBegin)//С�ڿ�ʼ�·ݵ����ݲ���
                            continue;
                        if (fstrSql != "")
                            fstrSql += " union all ";
                        if (i < 10)
                            fstrSql += " select 'db_appeal_" + yearBegin + "' as DBName,'t_tenpay_appeal_trans_" + "0" + i + "' as tableName," + str + yearBegin + ".t_tenpay_appeal_trans_" + "0" + i + strWhere;
                        else
                            fstrSql += " select 'db_appeal_" + yearBegin + "' as DBName,'t_tenpay_appeal_trans_" + i + "' as tableName," + str + yearBegin + ".t_tenpay_appeal_trans_" + i + strWhere;

                    }
                    yearBegin++;
                    monBegin = 0;//����ݺ󣬽���ʼ�·���Ϊ0
                }
            }
        }

        //�����쵥�����������ͷֿ������
        public CFTUserAppealClass(DateTime BeginDate, DateTime EndDate, string fstate, string ftype, string QQType, string username, int Count, int SortType, bool mark,string db,string tb)
        {
            string strWhere = " where FsubmitTime >= '" + BeginDate.ToString("yyyy-MM-dd 00:00:00") + "' and FsubmitTime <= '" + EndDate.ToString("yyyy-MM-dd 23:59:59") + "'";
            if (username == null || username == "")
            {
                throw new Exception("�����쵥�˲�����Ϊ��!");
            }

            if (fstate == "99")
            {
                strWhere += " and Fstate in(0,3,4,5,6,8) ";		// 2012/4/25  ��Ϊ����8Ҳ�����ˣ���������Ӧ����Ӱɣ�
            }
            else if (fstate == "0" || fstate == "3" || fstate == "4" || fstate == "5" || fstate == "6" || fstate == "8")  //����8Ҳ������
            {
                strWhere += " and Fstate = " + fstate;
            }
            else
            {
                throw new Exception("������״̬�����������쵥!");
            }
            if (ftype == "99")
            {
                strWhere += " and FType in(1,2,3,4,5,6,7,0,9,10)";//20130923 lxl �����Ƹ�ͨ����ֻ����ơ����������ơ����������쵥������
            }
            // 2012/4/2 �����ftype=��7�������������조����֤�����롱��
            else if (ftype == "1" || ftype == "2" || ftype == "3" || ftype == "4" || ftype == "5" || ftype == "6" || ftype == "7" || ftype == "0" || ftype == "9" || ftype == "10")
            {
                strWhere += " and FType = " + ftype;
            }
            else
            {
                throw new Exception("���������Ͳ����������쵥!");
            }

            if (QQType == "0")    //"" ��������; "0" �ǻ�Ա; "1" ��ͨ��Ա; "2" VIP��Ա
            {
                strWhere += " and FVipFlag<>1 and FVipFlag<>2 ";
            }
            else if (QQType == "1")
            {
                strWhere += " and FVipFlag=1 ";
            }
            else if (QQType == "2")
            {
                strWhere += " and FVipFlag=2 ";
            }

            fstrSql = "";
            fstrSql_count = "";
            string str = " Fid,FType,Fuin,FSubmitTime,FState,FCheckTime,FReCheckTime,Fpicktime,FReCheckUser,FCheckInfo,FCheckUser,FComment,Femail from db_appeal_";
            if (int.Parse(tb)< 10)
                fstrSql += " select 'db_appeal_" + db + "' as DBName,'t_tenpay_appeal_trans_" + "0" + tb + "' as tableName," + str + db + ".t_tenpay_appeal_trans_" + "0" + tb + strWhere;
            else
                fstrSql += " select 'db_appeal_" + db + "' as DBName,'t_tenpay_appeal_trans_" + tb + "' as tableName," + str + db + ".t_tenpay_appeal_trans_" + tb + strWhere;
        }


        public DataSet GetLockListDBTB(DateTime BeginDate, DateTime EndDate, string fstate, string ftype, string QQType, string username, int Count, int SortType)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            DataSet ds = new DataSet();

            try
            {
                da.OpenConn();

                string strSql = "select * from t_tenpay_appeal_trans where FsubmitTime >= '" + BeginDate.ToString("yyyy-MM-dd 00:00:00") + "' and FsubmitTime <= '" + EndDate.ToString("yyyy-MM-dd 23:59:59") + "'";

                if (username == null || username == "")
                {
                    throw new Exception("�����쵥�˲�����Ϊ��!");
                }

                if (fstate == "99")
                {
                    strSql += " and Fstate in(0,3,4,5,6,8) ";		// 2012/4/25  ��Ϊ����8Ҳ�����ˣ���������Ӧ����Ӱɣ�
                }
                else if (fstate == "0" || fstate == "3" || fstate == "4" || fstate == "5" || fstate == "6" || fstate == "8")  //����8Ҳ������
                {
                    strSql += " and Fstate = " + fstate;
                }
                else
                {
                    throw new Exception("������״̬�����������쵥!");
                }
                if (ftype == "99")
                {
                    strSql += " and FType in(1,2,3,4,5,6,7,0,9,10)";//20130923 lxl �����Ƹ�ͨ����ֻ����ơ����������ơ����������쵥������
                }
                // 2012/4/2 �����ftype=��7�������������조����֤�����롱��
                else if (ftype == "1" || ftype == "2" || ftype == "3" || ftype == "4" || ftype == "5" || ftype == "6" || ftype == "7" || ftype == "0" || ftype == "9" || ftype == "10")
                {
                    strSql += " and FType = " + ftype;
                }
                else
                {
                    throw new Exception("���������Ͳ����������쵥!");
                }

                if (QQType == "0")    //"" ��������; "0" �ǻ�Ա; "1" ��ͨ��Ա; "2" VIP��Ա
                {
                    strSql += " and (FParameter not like '%vip_flag=1%') and (FParameter not like '%vip_flag=2%') ";
                }
                else if (QQType == "1")
                {
                    strSql += " and FParameter like '%vip_flag=1%' ";
                }
                else if (QQType == "2")
                {
                    strSql += " and FParameter like '%vip_flag=2%' ";
                }

                if (SortType != 99)
                {
                    if (SortType == 0)   //����ʱ��С����
                        strSql += " order by FSubmitTime asc ";
                    if (SortType == 1)   //����ʱ���С
                        strSql += " order by FSubmitTime desc ";
                }

                strSql += " limit " + Count;

                ds = da.dsGetTotalData(strSql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    HandleParameter(ds, true);

                    string WhereStr = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["IsPass"].ToString() == "Y")
                        {
                            dr.Delete();
                        }
                        else
                            WhereStr += "'" + dr["Fid"].ToString().Trim() + "',";
                    }
                    ds.AcceptChanges();

                    WhereStr = WhereStr.Substring(0, WhereStr.Length - 1);

                    strSql = " update t_tenpay_appeal_trans set FPickUser='" + username + "',FPickTime=now(),Fstate=8 where Fid in(" + WhereStr + ")";

                    da.ExecSql(strSql);

                    fstrSql = "";
                    fstrSql_count = ds.Tables[0].Rows.Count.ToString();//�������¼����
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                da.Dispose();
            }

            return ds;
        }

        //������t_tenpay_appeal_kf_total
        /*��     ����      ����ɹ�  ����ʧ��   ����
        User OperationDay SuccessNum  FailNum  OtherNum
        ����ɹ�:���ɹ��ʼ�
        ����:�����ܶ�(ת��̨������)
        */
        public static bool ConfirmAppeal(int fid, string Fcomment, string user, string userIP, out string msg)
        {
            msg = "";

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                da.OpenConn();

                if (Fcomment != null && Fcomment.Trim() != "")
                {
                    Fcomment = PublicRes.replaceMStr(Fcomment);
                }

                //��ȡ��һЩ��Ҫ����Ϣ ��ǰ״̬,�޸����, QQ��,email
                //�޸�������Ҫ������. ȡ��֧��������Ҫclear_pps, ע������Ҫ���ණ��
                string strSql = "select * from t_tenpay_appeal_trans where Fid=" + fid;
                DataSet ds = da.dsGetTotalData(strSql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    HandleParameter(ds, true);

                    DataRow dr = ds.Tables[0].Rows[0];
                    int fstate = Int32.Parse(dr["FState"].ToString());
                    int ftype = Int32.Parse(dr["FType"].ToString());
                    string fuin = dr["FUin"].ToString();
                    string email = dr["Femail"].ToString();
                    string question1 = dr["question1"].ToString();
                    string answer1 = dr["answer1"].ToString();
                    string cont_type = dr["cont_type"].ToString();
                    string mobile_no = dr["mobile_no"].ToString();
                    string client_ip = dr["client_ip"].ToString();
                    string certno = dr["cre_id"].ToString();

                    if (fstate == 1 || fstate == 7)
                    {
                        msg = "ԭ��¼��״̬����ȷ";
                        return false;
                    }

                    string nowtime = PublicRes.strNowTimeStander;
                    string new_name = "";
                    int clear_pps = 0;

                    string submittime = DateTime.Parse(dr["FSubmitTime"].ToString()).ToString("yyyy��MM��dd��");

                    //��ȡ���ڵ��û�����furion 20060902
                    string username = PublicRes.GetNameFromQQ(fuin);
                    //��ע���û����ﴦ����
                    if ((username == null || username.Trim() == "") && ftype == 6)
                    {
                        username = "�װ��ĲƸ�ͨ�ͻ�";
                    }
                    if (username == null || username.Trim() == "")
                    {
                        msg = "ȡԭ���û���ʱ����";
                        //return false;
                    }

                    string p3 = "";
                    string p4 = "";

                    //					if (!PublicRes.ReleaseCache(fuin,"qqid"))
                    //					{
                    //						Common.CommLib.commRes.sendLog4Log("QUeryInfo.ConfirmAppeal","ͨ����������ʱ��Ԥ����û�"+ fuin + "cacheʧ�ܣ����飡");
                    //						msg = "ͨ����������ʱ��Ԥ����û�"+ fuin + "cacheʧ�ܣ����飡";
                    //						return false;
                    //					}
                    //���д���.	 //�����ʼ�
                    if (ftype == 0)
                    {
                        string Fuid = PublicRes.ConvertToFuid(fuin);
                        if (Fuid.Length < 3)
                        {
                            msg = "ȡ�ڲ�IDʧ��";
                            return false;
                        }

                        /*
                        string inmsg = "uid=" + Fuid;
                        inmsg += "&optype=3";
                        inmsg += "&time_stamp=" + TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(nowtime);
                        inmsg += "&server_ip=" + userIP;
                        inmsg += "&client_ip=" + client_ip;
                        inmsg += "&sign=" + System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Fuid + "3" + TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(nowtime) + "cft_uk_key","md5").ToLower();
                        */

                        //md5( uid + optype + time_stamp + watch_word)
                        string watchWord = System.Configuration.ConfigurationManager.AppSettings["watchword"].ToLower();
                        string inmsg = "watchword=" + watchWord;
                        inmsg += "&uid=" + Fuid;
                        inmsg += "&optype=3";
                        inmsg += "&time_stamp=" + TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(nowtime);
                        inmsg += "&server_ip=" + userIP;
                        inmsg += "&client_ip=" + client_ip;
                        inmsg += "&sign=" + System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Fuid + "3" + TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(nowtime) + watchWord, "md5").ToLower();


                        string reply;
                        short sresult;

                        if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("ra_apeal_service", inmsg, true, out reply, out sresult, out msg))
                        {
                            if (sresult != 0)
                            {
                                msg = "ra_apeal_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                                return false;
                            }
                            else
                            {
                                if (reply.IndexOf("result=0") > -1)   //&0=298751028&res_info=ok&result=0 ����ֵ
                                {
                                }
                                else
                                {
                                    msg = "ra_apeal_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            msg = "ra_apeal_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                            return false;
                        }
                    }
                    else if (ftype == 1)
                    {
                        //�һ�����
                        clear_pps = Int32.Parse(dr["clear_pps"].ToString());

                        bool IsNew; //�Ƿ�������
                        if (cont_type != "") //cont_typeΪ�¼��ֶΣ����Ϊ�վ�˵����������
                        {
                            IsNew = true;
                            bool IsMobile = false;
                            bool IsMail = false;
                            //cont_type=1ʱ,mobile_no��Ч cont_type=2ʱ,femail��Ч

                            if (cont_type == "1")
                                IsMobile = true;
                            else if (cont_type == "2")
                                IsMail = true;

                            if (cont_type == "1" || cont_type == "2")//�����
                            {
                                //������������û�������Ҫ�����䣨Ĭ�ϰ�������Ǹ��ʺţ�
                                if (IsMail && fuin.IndexOf("@") > -1)
                                {
                                }
                                else
                                {
                                    string BindMail = "";
                                    Query_Service qs = new Query_Service();
                                    if (!qs.BindMsgNotify(fuin, IsMobile, mobile_no, IsMail, email, client_ip,certno, out BindMail, out msg)) return false;

                                    email = BindMail;
                                }
                            }
                        }
                        else
                        {
                            IsNew = false;
                        }

                        if (!GetNewPwd(fuin, clear_pps, userIP, nowtime, question1, answer1, IsNew, out msg)) return false;

                        string warmTips = @"<br/><br/>��ܰ��ʾ��<br/>��������˺Ż�δ�󶨹����ֻ������������������󶨡��󶨳ɹ������ȿɸ��ݹ����ֻ�������֤�����ʵʱ���޸�����֧�����롣";

                        p3 = msg;
                        if (clear_pps == 1)
                            //p4 = "�������뱣�������ѱ���գ����½���������������뱣�����⣡"; 
                            p4 = "�����ܱ������Ѿ����³ɹ�����ʹ�ø��µ��ܱ��𰸲������ĲƸ�ͨ�˻���" + warmTips;
                        else
                            p4 = warmTips;
                    }
                    else if (ftype == 2)
                    {
                        //�޸��û�����
                        new_name = QueryInfo.GetString(dr["new_name"]);
                        /*�ɽӿ�
                        if(!UpdateUserName(fuin,new_name,false,userIP,nowtime, out msg))
                            return false;
                        */
                        //�½ӿ�
                        string Fuid = PublicRes.ConvertToFuid(fuin);
                        if (Fuid.Length < 3)
                        {
                            msg = "ȡ�ڲ�IDʧ��";
                            return false;
                        }

                        //������MD5����������Ŷ����Ϊ�������һ��
                        System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                        byte[] outbuff = GB2312.GetBytes(Fuid + "|" + fuin + "|" + new_name + "||5416d7cd6ef195a0f7622a9c56b55e84");
                        System.Security.Cryptography.MD5CryptoServiceProvider get_md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                        byte[] hash_byte = get_md5.ComputeHash(outbuff);

                        string inmsg = "uin=" + fuin;
                        inmsg += "&true_name=" + new_name;
                        inmsg += "&vali_type=100";
                        inmsg += "&token=" + System.BitConverter.ToString(hash_byte).Replace("-", "").ToLower();

                        string reply;
                        short sresult;

                        if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("ui_modify_usernameid_service", inmsg, true, out reply, out sresult, out msg))
                        {
                            if (sresult != 0)
                            {
                                msg = "ui_modify_usernameid_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                                return false;
                            }
                            else
                            {
                                if (reply.StartsWith("result=0"))
                                {
                                }
                                else
                                {
                                    msg = "ui_modify_usernameid_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            msg = "ui_modify_usernameid_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                            return false;
                        }

                        p3 = new_name;
                    }
                    else if (ftype == 3)
                    {
                        //�޸Ĺ�˾��
                        new_name = QueryInfo.GetString(dr["new_company"]);

                        if (!UpdateUserName(fuin, new_name, true, userIP, nowtime, out msg))
                            return false;

                        p3 = new_name;
                    }
                    else if (ftype == 4)
                    {
                        //ע���ʺ�
                        if (!DelUser(fuin, email, Fcomment, user, userIP, nowtime, out msg))
                            return false;

                        //��Ϊ������ʱ��֧��ע���󶨹�ϵ furion 20060902
                        //p3 = "������ͬʱע���˸��ʻ�ԭ���п��ź����֤����İ󶨹�ϵ��лл";
                        p3 = "";
                    }
                    else if (ftype == 5)//�����ʺŵ������һ��ֻ��ģ����ͨ����ͷ�ϵͳ������һ�������䡣 andrew 20110419
                    {

                        //�һ�����
                        clear_pps = Int32.Parse(dr["clear_pps"].ToString());

                        bool IsNew; //�Ƿ�������
                        if (cont_type != "") //cont_typeΪ�¼��ֶΣ����Ϊ�վ�˵����������
                        {
                            IsNew = true;
                            bool IsMobile = false;
                            bool IsMail = false;
                            //cont_type=1ʱ,mobile_no��Ч cont_type=2ʱ,femail��Ч

                            if (cont_type == "1")
                                IsMobile = true;
                            else if (cont_type == "2")
                                IsMail = true;

                            if (cont_type == "1" || cont_type == "2")
                            {
                                //������������û�������Ҫ�����䣨Ĭ�ϰ�������Ǹ��ʺţ�
                                if (IsMail && fuin.IndexOf("@") > -1)
                                {
                                }
                                else
                                {
                                    string BindMail = "";
                                    Query_Service qs = new Query_Service();
                                    if (!qs.BindMsgNotify(fuin, IsMobile, mobile_no, IsMail, email, client_ip,certno, out BindMail, out msg)) return false;

                                    email = BindMail;
                                }
                            }
                        }
                        else
                        {
                            IsNew = false;
                        }

                        if (clear_pps == 1)
                        {
                            //ֻ�滻�ܱ�����ʹ�
                            if (!CheckQuestion(fuin, userIP, nowtime, question1, answer1, IsNew, out msg)) return false;
                        }

                        if (dr["from"].ToString() == "TENPAY_MOBILE")
                        {
                            string Fuid = PublicRes.ConvertToFuid(fuin);
                            string ret = "";
                            string appealtime = TimeTransfer.GetTickFromTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("GB2312");
                            string cgi = System.Configuration.ConfigurationManager.AppSettings["MobileCgi"].ToString();
                            cgi += "?uin=" + fuin;
                            cgi += "&bargainor_id=1000000000";
                            cgi += "&uid=" + Fuid;
                            cgi += "&appealtime=" + appealtime;
                            cgi += "&status=0";
                            cgi += "&cmd=update";
                            cgi += "&newmobile=" + mobile_no;
                            cgi += "&chv=9";
                            cgi += "&sign=" + System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("appealtime=" + appealtime + "&bargainor_id=1000000000" +
                                "&chv=9" + "&cmd=update" + "&newmobile=" + mobile_no + "&status=0" + "&uid=" + Fuid + "&uin=" + fuin + "&key=Adf^#KK12D", "md5").ToLower();

                            SunLibrary.LoggerFactory.Get("KF_Service QueryInfo").Info("MobileCgi send req:" + cgi);
                            
                            System.Net.HttpWebRequest webrequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(cgi);
                            System.Net.HttpWebResponse webresponse = (System.Net.HttpWebResponse)webrequest.GetResponse();
                            Stream stream = webresponse.GetResponseStream();
                            StreamReader streamReader = new StreamReader(stream, encoding);
                            ret = streamReader.ReadToEnd();

                            SunLibrary.LoggerFactory.Get("KF_Service QueryInfo").Info("MobileCgi return:" + ret);

                            webresponse.Close();
                            streamReader.Close();
                        }

                        //���������ֻ�,������ʲô����,ֱ�ӷ����ʼ��Ϳ�����.
                        string url = ConfigurationManager.AppSettings["ChangeMobileUrl"].Trim();

                        // ���������ֻ��Ļ�����Ҫ���WarmTips
                        string warmTips = "<br/> ��ܰ��ʾ��<br/>�����ӵ�ַ��Ч��Ϊ72Сʱ����������Ч����������ֻ��ʺŵİ󶨣�������޷����������ӵ�ַ������ʹ��IE�������¼<a href='www.tenpay.com'>�Ƹ�ͨ��վ</a>�������µ���ʼ����ӵ�ַ����������ֱ�ӽ������������ӵ�ַ���Ƶ��������ַ���д򿪲�������,лл��";

                        p3 = url + fid;

                        if (clear_pps == 1)
                            p4 = p3 + "<br/>�����ܱ������Ѿ����³ɹ�����ʹ��������ʱ�ɹ��޸ĵ��ܱ��𰸼��ɣ�" + warmTips;
                        else
                            p4 = p3 + warmTips;
                    }
                    else if (ftype == 6) //andrew 20110419
                    {
                        //���������ֻ�,������ʲô����,ֱ�ӷ����ʼ��Ϳ�����.
                        string url = ConfigurationManager.AppSettings["ChangeMobileUrl"].Trim();

                        // ���������ֻ��Ļ�����Ҫ���WarmTips
                        string warmTips = "<br/> ��ܰ��ʾ��<br/>�����ӵ�ַ��Ч��Ϊ72Сʱ����������Ч����������ֻ��ʺŵİ󶨣�������޷����������ӵ�ַ������ʹ��IE�������¼<a href='www.tenpay.com'>�Ƹ�ͨ��վ</a>�������µ���ʼ����ӵ�ַ����������ֱ�ӽ������������ӵ�ַ���Ƶ��������ַ���д򿪲�������,лл��";

                        p3 = url + fid;
                        p4 = p3 + warmTips;
                    }
                    else if (ftype == 7)
                    {
                        string new_cre_id = dr["new_cre_id"].ToString();
                        //�޸����֤��
                        /*�ɽӿ�
                        if(!UpdateCreid(fuin,new_cre_id,userIP,nowtime, out msg))
                            return false;
                        */
                        //�½ӿ�
                        string Fuid = PublicRes.ConvertToFuid(fuin);
                        if (Fuid.Length < 3)
                        {
                            msg = "ȡ�ڲ�IDʧ��";
                            return false;
                        }
                        string inmsg = "uin=" + fuin;
                        inmsg += "&cre_type=" + "1";
                        inmsg += "&cre_id=" + new_cre_id;
                        inmsg += "&vali_type=100";
                        inmsg += "&token=" + System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Fuid + "|" + fuin + "||" + new_cre_id + "|5416d7cd6ef195a0f7622a9c56b55e84", "md5").ToLower();

                        string reply;
                        short sresult;

                        if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("ui_modify_usernameid_service", inmsg, true, out reply, out sresult, out msg))
                        {
                            if (sresult != 0)
                            {
                                msg = "ui_modify_usernameid_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                                return false;
                            }
                            else
                            {
                                if (reply.StartsWith("result=0"))
                                {
                                }
                                else
                                {
                                    msg = "ui_modify_usernameid_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            msg = "ui_modify_usernameid_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                            return false;
                        }
                        p3 = new_cre_id;
                    }
                    else if (ftype == 9) //bruceliao 20121106
                    {
                        string Key = ConfigurationManager.AppSettings["Mbtoken_Unbind_Key"].Trim();
                        string Fuid = PublicRes.ConvertToFuid(fuin);
                        string time_stamp = TimeTransfer.GetTickFromTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        string inmsg = "uin=" + fuin;
                        inmsg += "&optype=" + "1";
                        inmsg += "&uid=" + Fuid;
                        inmsg += "&time_stamp=" + time_stamp;
                        inmsg += "&sign=" + System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Fuid + "1" + time_stamp + Key, "md5").ToLower();

                        string reply;
                        short sresult;

                        if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("mbtoken_unbind_service", inmsg, true, out reply, out sresult, out msg))
                        {
                            if (sresult != 0)
                            {
                                msg = "mbtoken_unbind_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                                return false;
                            }
                            else
                            {
                                if (reply.IndexOf("result=0") > -1)
                                {
                                }
                                else
                                {
                                    msg = "mbtoken_unbind_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            msg = "mbtoken_unbind_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                            return false;
                        }
                    }
                    else if (ftype == 10) //�������͵���������
                    {
                        string ip = ConfigurationManager.AppSettings["TCPMobileTokenIP"].Trim();
                        int portNumber;
                        int.TryParse(ConfigurationManager.AppSettings["TCPMobileTokenPORT"].Trim(),out portNumber);//�ַ�����ʽportת����int��ʽ
                        string Key = ConfigurationManager.AppSettings["Mbtoken_Unbind_Key"].Trim();
                        string Fuid = PublicRes.ConvertToFuid(fuin);
                        string time_stamp = TimeTransfer.GetTickFromTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        string request_type = "8161";
                        string inmsg = "request_type=" + request_type;
                        inmsg += "&optype=" + "1";
                        inmsg += "&uid=" + Fuid;
                        inmsg += "&ver=" + "1";
                        inmsg += "&head_u=" + Fuid;
                        inmsg += "&time_stamp=" + time_stamp;
                        inmsg += "&sign=" + System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Fuid + "1" + time_stamp + Key, "md5").ToLower();
                        var parameters = System.Text.Encoding.Default.GetBytes(inmsg);
                        var length = new byte[4];
                        length = BitConverter.GetBytes(parameters.Length);
                        List<byte> bufferIn = new List<byte>();
                        bufferIn.AddRange(length);
                        bufferIn.AddRange(parameters);


                        string token_seq;
                        string sresult;

                        TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTCPReply(inmsg,bufferIn.ToArray(), ip, portNumber, out msg, "sdyb_mbtoken_itg_srv", out sresult, out token_seq);
                        if (!("0".Equals(sresult)))//sresult="0"��ʾ���ɹ�
                        {
                            msg = "sdyb_mbtoken_itg_srv�ӿڷ���ʧ��Ӧ��result=" + sresult + "��msg=" + msg + "��token_seq=" + token_seq;
                            return false;
                        }
                    }
                    else
                    {
                        msg = "�������Ͳ���ȷ";
                        return false;
                    }

                    if (!SendAppealMail(email, ftype, true, username, submittime, p3, p4, "", "", fuin, out msg))
                    {
                        msg = "�����ʼ�ʧ�ܣ�" + msg;
                        return false;
                    }
                    else
                    {

                        if (fstate == 0)
                        {
                            strSql = "update t_tenpay_appeal_trans set FState=1,"
                                + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=0";
                        }
                        else if (fstate == 2)
                        {
                            strSql = "update t_tenpay_appeal_trans set FState=1,"
                                + " Fcomment='�������,�ܾ�ת���ͨ��." + Fcomment + "', FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=2";
                        }
                        else if (fstate == 3)
                        {
                            strSql = "update t_tenpay_appeal_trans set FState=1,"
                                + " Fcomment='" + Fcomment + "', FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=3";
                        }
                        else if (fstate == 4)
                        {
                            strSql = " update t_tenpay_appeal_trans set FState=1,"
                                + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=4";
                        }
                        else if (fstate == 5)
                        {
                            strSql = "update t_tenpay_appeal_trans set FState=1,"
                                + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=5";
                        }
                        else if (fstate == 6)
                        {
                            strSql = " update t_tenpay_appeal_trans set FState=1,"
                                + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=6";
                        }
                        else if (fstate == 8)
                        {
                            strSql = " update t_tenpay_appeal_trans set FState=1,"
                                + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=8";
                        }
                        // 2012/4/25 �������ͨ�����ų���״̬�����ߣ����������Ӧ�ķ��֪ͨ��
                        else if (fstate == 9)
                        {
                            strSql = " update t_tenpay_appeal_trans set FState=1,"
                                + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=9";
                        }
                        else
                        {
                            msg = "����ԭ�м�¼����,�˼�¼ԭʼ״̬����ȷ";
                            return false;
                        }
                        if (da.ExecSqlNum(strSql) == 1)
                        {
                            InputAppealNumber(user, "Success", "appeal");

                            string IsSendAppeal = ConfigurationManager.AppSettings["IsSendAppeal"].ToString();
                            if (IsSendAppeal == "Yes")
                            {
                                try
                                {
                                    if (ftype == 1 || ftype == 5 || ftype == 6)//�������ݸ����ĿǰֻҪ���루1���͸��������ֻ���5�� 2012/4/28 ����fstate=9��ftype=6֪ͨ���2��ͨ�������ų���״̬��������
                                    {
                                        fuin = System.Web.HttpUtility.UrlEncode(fuin, System.Text.Encoding.GetEncoding("GB2312"));
                                        string ftypestr;
                                        //2014/07/11 xiuling��type=6δ����ز��޸�
                                        //if (fstate == 9)
                                        //    ftypestr = System.Web.HttpUtility.UrlEncode("6", System.Text.Encoding.GetEncoding("GB2312"));
                                        //else
                                            ftypestr = System.Web.HttpUtility.UrlEncode(ftype.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        //string fstatestr = System.Web.HttpUtility.UrlEncode(fstate.ToString(),System.Text.Encoding.GetEncoding("GB2312"));
                                        string cre_id = System.Web.HttpUtility.UrlEncode(dr["cre_id"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string clear_ppsstr = System.Web.HttpUtility.UrlEncode(dr["clear_pps"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string is_answer = System.Web.HttpUtility.UrlEncode(dr["labIsAnswer"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        mobile_no = System.Web.HttpUtility.UrlEncode(mobile_no, System.Text.Encoding.GetEncoding("GB2312"));
                                        email = System.Web.HttpUtility.UrlEncode(email, System.Text.Encoding.GetEncoding("GB2312"));
                                        string score = System.Web.HttpUtility.UrlEncode(dr["score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string is_pass = System.Web.HttpUtility.UrlEncode(dr["IsPass"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        client_ip = System.Web.HttpUtility.UrlEncode(client_ip, System.Text.Encoding.GetEncoding("GB2312"));
                                        string reason = System.Web.HttpUtility.UrlEncode(dr["reason"].ToString().Trim(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string fcomment = System.Web.HttpUtility.UrlEncode(Fcomment, System.Text.Encoding.GetEncoding("GB2312")); //��ע
                                        string fcheck_info = System.Web.HttpUtility.UrlEncode("", System.Text.Encoding.GetEncoding("GB2312"));   //�ܾ�ԭ��
                                        string fsubmittime = System.Web.HttpUtility.UrlEncode(dr["FSubmitTime"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string fchecktime = System.Web.HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Text.Encoding.GetEncoding("GB2312"));

                                        string Data = "fuin=" + fuin + "&ftype=" + ftypestr + "&fstate=1&cre_id=" + cre_id + "&clear_pps=" + clear_ppsstr +
                                            "&is_answer=" + is_answer + "&mobile_no=" + mobile_no + "&email=" + email + "&score=" + score + "&is_pass=" + is_pass +
                                            "&client_ip=" + client_ip + "&fsubmittime=" + fsubmittime + "&fchecktime=" + fchecktime + "&reason=" + reason + "&fcomment=" + fcomment + "&fcheck_info=" + fcheck_info;
                                        Data = System.Web.HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                                        Data = "protocol=user_appeal_notify&version=1.0&data=" + Data;

                                        SunLibrary.LoggerFactory.Get("KF_Service QueryInfo").Info("user_appeal_notify send:" + Data);


                                        System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                                        byte[] sendBytes = GB2312.GetBytes(Data);

                                        string IP = ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                                        string PORT = ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                                        TENCENT.OSS.CFT.KF.Common.UDP.GetUDPReplyNoReturnNotReturn(sendBytes, IP, Int32.Parse(PORT));
                                    }
                                }
                                catch
                                {
                                }
                            }
                            return true;
                        }
                        else
                        {
                            msg = "����ԭ�м�¼����,�˼�¼�����ڻ�ԭʼ״̬����ȷ";
                            return false;
                        }
                    }
                }
                else
                {
                    msg = "���Ҽ�¼ʧ��.";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public static bool ConfirmAppealDBTB(string fid, string db,string tb,string Fcomment, string user, string userIP, out string msg)
        {
            msg = "";

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFTNEW"));
            try
            {
                da.OpenConn();

                if (Fcomment != null && Fcomment.Trim() != "")
                {
                    Fcomment = PublicRes.replaceMStr(Fcomment);
                }

                //��ȡ��һЩ��Ҫ����Ϣ ��ǰ״̬,�޸����, QQ��,email
                //�޸�������Ҫ������. ȡ��֧��������Ҫclear_pps, ע������Ҫ���ණ��
                string strSql = "select * from " + db + "." + tb + " where FID='" + fid+"'";
                DataSet ds = da.dsGetTotalData(strSql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    #region
                    HandleParameterByDBTB(ds, true);

                    DataRow dr = ds.Tables[0].Rows[0];
                    int fstate = Int32.Parse(dr["FState"].ToString());
                    int ftype = Int32.Parse(dr["FType"].ToString());
                    string fuin = dr["FUin"].ToString();
                    string email = dr["Femail"].ToString();
                    string question1 = dr["question1"].ToString();
                    string answer1 = dr["answer1"].ToString();
                    string cont_type = dr["cont_type"].ToString();
                    string mobile_no = dr["mobile_no"].ToString();
                    string client_ip = dr["client_ip"].ToString();
                  //  string mobile_bind = dr["FMobile"].ToString();//���ֻ���,ȡ���������
                    string certno = dr["cre_id"].ToString();

                    if (fstate == 1 || fstate == 7)
                    {
                        msg = "ԭ��¼��״̬����ȷ";
                        return false;
                    }

                    string nowtime = PublicRes.strNowTimeStander;
                    string new_name = "";
                    int clear_pps = 0;

                    string submittime = DateTime.Parse(dr["FSubmitTime"].ToString()).ToString("yyyy��MM��dd��");

                    //��ȡ���ڵ��û�����furion 20060902
                    string username = PublicRes.GetNameFromQQ(fuin);
                    //��ע���û����ﴦ����
                    if ((username == null || username.Trim() == "") && ftype == 6)
                    {
                        username = "�װ��ĲƸ�ͨ�ͻ�";
                    }
                    if (username == null || username.Trim() == "")
                    {
                        msg = "ȡԭ���û���ʱ����";
                        //return false;
                    }

                    string p3 = "";
                    string p4 = "";

                    //					if (!PublicRes.ReleaseCache(fuin,"qqid"))
                    //					{
                    //						Common.CommLib.commRes.sendLog4Log("QUeryInfo.ConfirmAppeal","ͨ����������ʱ��Ԥ����û�"+ fuin + "cacheʧ�ܣ����飡");
                    //						msg = "ͨ����������ʱ��Ԥ����û�"+ fuin + "cacheʧ�ܣ����飡";
                    //						return false;
                    //					}
                    //���д���.	 //�����ʼ�
                    if (ftype == 1)
                    {
                        //�һ�����
                        clear_pps = Int32.Parse(dr["clear_pps"].ToString());

                        bool IsNew; //�Ƿ�������
                        if (cont_type != "") //cont_typeΪ�¼��ֶΣ����Ϊ�վ�˵����������
                        {
                            IsNew = true;

                            if (cont_type == "1")//ǰ�˻ᴫ��1����3,3��ʱ����Ҫ�ͷ���
                            {
                                string Fuid = PublicRes.ConvertToFuid(fuin);
                                string client_id = ConfigurationManager.AppSettings["client_id"].ToString();
                                string singed = PublicRes.SingedByService("client_id=" + client_id + "&cmd=update_mobile&mobile=" + mobile_no + "&uid=" + Fuid);

                                //1.�һ����롢�������򻯰�����ֻ���Ҫ�� user_appeal_notify ֪ͨ
                                //2.�漰���󶨡������ֻ���Ҫ����֤��֪ͨ
                               string old_mobile = "";
                               Query_Service qs = new Query_Service();
                               if (!qs.BindOrChangeMobile(Fuid, fuin, old_mobile, mobile_no, client_ip, certno,singed, out msg))
                                      return false;
                                
                            }
                        }
                        else
                        {
                            IsNew = false;
                        }

                        if (clear_pps == 1)
                        {
                            //�滻�ܱ�����ʹ�
                            if (!CheckQuestion(fuin, userIP, nowtime, question1, answer1, true, out msg)) return false;
                        }

                        string url = ConfigurationManager.AppSettings["GetPassWKeyEmailUrl"].ToString();
                        string urlQQtips = ConfigurationManager.AppSettings["GetPassWKeyTipsUrl"].ToString();
                        string urlMessage = ConfigurationManager.AppSettings["GetPassWKeyMesUrl"].ToString();
                        //���ʼ������š�QQTips (string qqid, int ftype, string url, bool issucc, out string msg)
                        if (!SendAppealMailNew(email, ftype, true, username, url, url, url, "", "", fuin, out msg) )
                        {
                            msg = "�����ʼ�ʧ�ܣ�" + msg;
                            return false;
                        }
                        if (!SendAppealQQTips(fuin, ftype, urlQQtips, true, out msg))
                        {
                            msg = "����QQTipsʧ�ܣ�" + msg;
                            return false;
                        }
                        if (!SendAppealMessage(mobile_no, ftype, urlMessage, true, "", out msg))
                        {
                            msg = "���Ͷ���ʧ�ܣ�" + msg;
                            return false;
                        }
                    }

                    else if (ftype == 5 || ftype == 6)
                    {
                        //�޸��ܱ�
                        clear_pps = Int32.Parse(dr["clear_pps"].ToString());
                      
                            //ftype=5,�̶�cont_type=3
                            if (cont_type == "3")
                            {
                                string Fuid = PublicRes.ConvertToFuid(fuin);
                                string client_id = ConfigurationManager.AppSettings["client_id"].ToString();
                                string singed = PublicRes.SingedByService("client_id=" + client_id + "&cmd=update_mobile&mobile=" + mobile_no + "&uid=" + Fuid);
                             
                                //1.�һ����롢�������򻯰�����ֻ���Ҫ�� user_appeal_notify ֪ͨ
                                //2.�漰���󶨡������ֻ���Ҫ����֤��֪ͨ

                                Query_Service qs = new Query_Service();
                                string old_mobile = qs.GetOldBindMobile(Fuid,out msg);
                                if (msg!="")
                                    return false;
                                if (string.IsNullOrEmpty(old_mobile.Trim())){
                                    msg = "old_mobileΪ��";
                                    return false;
                                }
                                if (!qs.BindOrChangeMobile(Fuid, fuin, old_mobile, mobile_no, client_ip, certno, singed, out msg))
                                    return false;
                            }
                            else
                            {
                                msg = "cont_type��Ϊ3";
                                return false;
                            }

                        if (clear_pps == 1)
                        {
                            //�滻�ܱ�����ʹ�  lxl 20131226 ֻ����������޸�֧�������и��ܱ�
                            if (!CheckQuestion(fuin, userIP, nowtime, question1, answer1, true, out msg)) return false;
                        }

                        //���ʼ�������(string qqid, int ftype, string url, bool issucc, out string msg) 
                        if (mobile_no == "" || mobile_no.Length != 11)
                        {
                            msg = "mobile�����Ϲ淶��mobile=" + mobile_no;
                            return false;
                        }
                        string noticeMobile = mobile_no.Substring(0, 3) + "******" + mobile_no.Substring(9,2);
                        if (!SendAppealMailNew(email, ftype, true, username, noticeMobile, "", "", "", "", fuin, out msg))
                        {
                            msg = "�����ʼ�ʧ�ܣ�" + msg;
                            return false;
                        }
                        if ( !SendAppealMessage(mobile_no, ftype, "", true, "", out msg))
                        {
                            msg = "���Ͷ���ʧ�ܣ�" + msg;
                            return false;
                        }
                    }
                    else
                    {
                        msg = "�������Ͳ���ȷ";
                        return false;
                    }

                    #region
                    if (fstate == 0)
                    {
                        strSql = "update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=0";
                    }
                    else if (fstate == 2)
                    {
                        strSql = "update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='�������,�ܾ�ת���ͨ��." + Fcomment + "', FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=2";
                    }
                    else if (fstate == 3)
                    {
                        strSql = "update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "', FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=3";
                    }
                    else if (fstate == 4)
                    {
                        strSql = " update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=4";
                    }
                    else if (fstate == 5)
                    {
                        strSql = "update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=5";
                    }
                    else if (fstate == 6)
                    {
                        strSql = " update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=6";
                    }
                    else if (fstate == 8)
                    {
                        strSql = " update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=8";
                    }
                    // 2012/4/25 �������ͨ�����ų���״̬�����ߣ����������Ӧ�ķ��֪ͨ��
                    else if (fstate == 9)
                    {
                        strSql = " update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=9";
                    }
                    else
                    {
                        msg = "����ԭ�м�¼����,�˼�¼ԭʼ״̬����ȷ";
                        return false;
                    }
                    if (da.ExecSqlNum(strSql) == 1)
                    {
                        InputAppealNumber(user, "Success", "appeal");

                        string IsSendAppeal = ConfigurationManager.AppSettings["IsSendAppeal"].ToString();
                        if (IsSendAppeal == "Yes")
                        {
                            try
                            {
                                if (ftype == 1 || ftype == 5 || ftype == 6)//�������ݸ����ĿǰֻҪ���루1���͸��������ֻ���5�� 2012/4/28 ����fstate=9��ftype=6֪ͨ���2��ͨ�������ų���״̬��������
                                {
                                    fuin = System.Web.HttpUtility.UrlEncode(fuin, System.Text.Encoding.GetEncoding("GB2312"));
                                    string ftypestr;
                                    //if (fstate == 9)
                                    //    ftypestr = System.Web.HttpUtility.UrlEncode("6", System.Text.Encoding.GetEncoding("GB2312"));
                                    //else
                                        ftypestr = System.Web.HttpUtility.UrlEncode(ftype.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    //string fstatestr = System.Web.HttpUtility.UrlEncode(fstate.ToString(),System.Text.Encoding.GetEncoding("GB2312"));
                                    string cre_id = System.Web.HttpUtility.UrlEncode(dr["cre_id"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string clear_ppsstr = System.Web.HttpUtility.UrlEncode(dr["clear_pps"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string is_answer = System.Web.HttpUtility.UrlEncode(dr["labIsAnswer"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    mobile_no = System.Web.HttpUtility.UrlEncode(mobile_no, System.Text.Encoding.GetEncoding("GB2312"));
                                    email = System.Web.HttpUtility.UrlEncode(email, System.Text.Encoding.GetEncoding("GB2312"));
                                    string score = System.Web.HttpUtility.UrlEncode(dr["score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string is_pass = System.Web.HttpUtility.UrlEncode(dr["IsPass"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    client_ip = System.Web.HttpUtility.UrlEncode(client_ip, System.Text.Encoding.GetEncoding("GB2312"));
                                    string reason = System.Web.HttpUtility.UrlEncode(dr["reason"].ToString().Trim(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string fcomment = System.Web.HttpUtility.UrlEncode(Fcomment, System.Text.Encoding.GetEncoding("GB2312")); //��ע
                                    string fcheck_info = System.Web.HttpUtility.UrlEncode("", System.Text.Encoding.GetEncoding("GB2312"));   //�ܾ�ԭ��
                                    string fsubmittime = System.Web.HttpUtility.UrlEncode(dr["FSubmitTime"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string fchecktime = System.Web.HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Text.Encoding.GetEncoding("GB2312"));

                                    string Data = "fuin=" + fuin + "&ftype=" + ftypestr + "&fstate=1&cre_id=" + cre_id + "&clear_pps=" + clear_ppsstr +
                                        "&is_answer=" + is_answer + "&mobile_no=" + mobile_no + "&email=" + email + "&score=" + score + "&is_pass=" + is_pass +
                                        "&client_ip=" + client_ip + "&fsubmittime=" + fsubmittime + "&fchecktime=" + fchecktime + "&reason=" + reason + "&fcomment=" + fcomment + "&fcheck_info=" + fcheck_info;
                                    Data = System.Web.HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                                    Data = "protocol=user_appeal_notify&version=1.0&data=" + Data;

                                    SunLibrary.LoggerFactory.Get("KF_Service QueryInfo").Info("user_appeal_notify send:" + Data);

                                    System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                                    byte[] sendBytes = GB2312.GetBytes(Data);

                                    string IP = ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                                    string PORT = ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                                    TENCENT.OSS.CFT.KF.Common.UDP.GetUDPReplyNoReturnNotReturn(sendBytes, IP, Int32.Parse(PORT));
                                }
                            }
                            catch
                            {
                            }
                        }
                        return true;
                    }
                    else
                    {
                        msg = "����ԭ�м�¼����,�˼�¼�����ڻ�ԭʼ״̬����ȷ";
                        return false;
                    }
                    #endregion

                    #endregion
                }
                else
                {
                    msg = "���Ҽ�¼ʧ��.";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

      

        public static bool CancelAppeal(int fid, string reason, string OtherReason, string Fcomment, string user, string userIP, out string msg)
        {
            if (reason != null && reason.Trim() != "")
            {
                reason = PublicRes.replaceMStr(reason);
            }


            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                da.OpenConn();
                if (OtherReason != null && OtherReason.Trim() != "")
                {
                    OtherReason = PublicRes.replaceMStr(OtherReason);
                }
                if (Fcomment != null && Fcomment.Trim() != "")
                {
                    Fcomment = PublicRes.replaceMStr(Fcomment);
                }

                //�޸�������Ҫ������. ȡ��֧��������Ҫclear_pps, ע������Ҫ���ණ��
                string strSql = "select * from t_tenpay_appeal_trans where Fid=" + fid;
                DataSet ds = da.dsGetTotalData(strSql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    HandleParameter(ds, true);

                    DataRow dr = ds.Tables[0].Rows[0];
                    int fstate = Int32.Parse(dr["FState"].ToString());
                    int ftype = Int32.Parse(dr["FType"].ToString());
                    string fuin = dr["FUin"].ToString();
                    string email = dr["Femail"].ToString();
                    string submittime = DateTime.Parse(dr["FSubmitTime"].ToString()).ToString("yyyy��MM��dd��");

                    //��ȡ���ڵ��û�����furion 20060902
                    string username = PublicRes.GetNameFromQQ(fuin);
                    //��ע���û����ﴦ����
                    if ((username == null || username.Trim() == "") && ftype == 6)
                    {
                        username = "�װ��ĲƸ�ͨ�ͻ�";
                    }

                    if (username == null || username.Trim() == "")
                    {
                        msg = "ȡԭ���û���ʱ����";
                        username = fuin;
                        return false;
                    }
                    string EmailReason = reason;

                    if (ftype == 1)   //�������У����ڰ��ֻ����û��ͷ�����ԭ���󶨵����䣬���ڰ�������û��ͷ���¼�������
                    {
                        //�һ�����
                        string cont_type = dr["cont_type"].ToString();

                        if (cont_type == "1") //cont_typeΪ�¼��ֶΣ������Ϊ�վ�˵����������,cont_type=1ʱ,mobile_no��Ч cont_type=2ʱ,femail��Ч
                        {
                            string Fuid = PublicRes.ConvertToFuid(fuin);

                            if (Fuid == null || Fuid == "")
                            {
                                msg = "���ʺŵ��ڲ�IDΪ��,��ѯ������ʧ��!";
                                return false;
                            }

                            string sql = " select * from msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_" + Fuid.Substring(Fuid.Length - 1, 1) + " where Fuid = '" + Fuid + "'";
                            DataSet emailds = da.dsGetTotalData(sql);
                            if (emailds == null || emailds.Tables.Count == 0 || emailds.Tables[0].Rows.Count == 0)
                            {
                                msg = "���ʺ�û�а�����!";
                                return false;
                            }

                            email = emailds.Tables[0].Rows[0]["Femail"].ToString();
                        }

                        EmailReason = reason + "<br/> ��ܰ��ʾ��<br/> ��������˺��Ѿ������ֻ����룬������Ч������֤�룬������ֱ�ӽ���<a href='https://www.tenpay.com/v2.0/main/getPwdByMobile_index.shtml'>�ֻ����һ�֧�������ַ</a>ͨ�����ֻ������һ�����֧�����롣&";
                    }
                    //����ʧ���ʼ� ʧ�ܵ�һ��û�����Ĳ���
                    if (!SendAppealMail(email, ftype, false, username, submittime, "", "", EmailReason, OtherReason, fuin, out msg))
                    {
                        msg = "�����ʼ�ʧ�ܣ�" + msg;
                        return false;
                    }
                    else
                    {
                        if (fstate == 0)
                        {
                            strSql = "update t_tenpay_appeal_trans set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=0";
                        }
                        else if (fstate == 3)
                        {
                            strSql = "update t_tenpay_appeal_trans set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=3";
                        }
                        else if (fstate == 4)
                        {
                            strSql = " update t_tenpay_appeal_trans set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=4";
                        }
                        else if (fstate == 5)
                        {
                            strSql = "update t_tenpay_appeal_trans set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=5";
                        }
                        else if (fstate == 6)
                        {
                            strSql = " update t_tenpay_appeal_trans set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=6";
                        }
                        else if (fstate == 8)
                        {
                            strSql = " update t_tenpay_appeal_trans set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=8";
                        }
                        else
                        {
                            msg = "����ԭ�м�¼����,�˼�¼ԭʼ״̬����ȷ";
                            return false;
                        }
                        if (da.ExecSqlNum(strSql) == 1)
                        {
                            InputAppealNumber(user, "Fail", "appeal");
                            string IsSendAppeal = ConfigurationManager.AppSettings["IsSendAppeal"].ToString();
                            if (IsSendAppeal == "Yes")
                            {
                                try
                                {
                                    if (ftype == 1 || ftype == 5)//�������ݸ����ĿǰֻҪ���루1���͸��������ֻ���5��
                                    {
                                        fuin = System.Web.HttpUtility.UrlEncode(fuin, System.Text.Encoding.GetEncoding("GB2312"));
                                        string ftypestr = System.Web.HttpUtility.UrlEncode(ftype.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        //string fstatestr = System.Web.HttpUtility.UrlEncode(fstate.ToString(),System.Text.Encoding.GetEncoding("GB2312"));
                                        string cre_id = System.Web.HttpUtility.UrlEncode(dr["cre_id"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string clear_ppsstr = System.Web.HttpUtility.UrlEncode(dr["clear_pps"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string is_answer = System.Web.HttpUtility.UrlEncode(dr["labIsAnswer"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string mobile_no = System.Web.HttpUtility.UrlEncode(dr["mobile_no"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        email = System.Web.HttpUtility.UrlEncode(email, System.Text.Encoding.GetEncoding("GB2312"));
                                        string score = System.Web.HttpUtility.UrlEncode(dr["score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string is_pass = System.Web.HttpUtility.UrlEncode(dr["IsPass"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string client_ip = System.Web.HttpUtility.UrlEncode(dr["client_ip"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string reasonstr = System.Web.HttpUtility.UrlEncode(dr["reason"].ToString().Trim(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string fcomment = System.Web.HttpUtility.UrlEncode(Fcomment, System.Text.Encoding.GetEncoding("GB2312")); //��ע
                                        string fcheck_info = System.Web.HttpUtility.UrlEncode(reason + OtherReason, System.Text.Encoding.GetEncoding("GB2312"));   //�ܾ�ԭ��
                                        string fsubmittime = System.Web.HttpUtility.UrlEncode(dr["FSubmitTime"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string fchecktime = System.Web.HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Text.Encoding.GetEncoding("GB2312"));

                                        string Data = "fuin=" + fuin + "&ftype=" + ftypestr + "&fstate=2&cre_id=" + cre_id + "&clear_pps=" + clear_ppsstr +
                                            "&is_answer=" + is_answer + "&mobile_no=" + mobile_no + "&email=" + email + "&score=" + score + "&is_pass=" + is_pass +
                                            "&client_ip=" + client_ip + "&fsubmittime=" + fsubmittime + "&fchecktime=" + fchecktime + "&reason=" + reasonstr + "&fcomment=" + fcomment + "&fcheck_info=" + fcheck_info;
                                        Data = System.Web.HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                                        Data = "protocol=user_appeal_notify&version=1.0&data=" + Data;

                                        SunLibrary.LoggerFactory.Get("KF_Service QueryInfo").Info("user_appeal_notify send:" + Data);

                                        System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                                        byte[] sendBytes = GB2312.GetBytes(Data);

                                        string IP = ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                                        string PORT = ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                                        TENCENT.OSS.CFT.KF.Common.UDP.GetUDPReplyNoReturnNotReturn(sendBytes, IP, Int32.Parse(PORT));
                                    }
                                }
                                catch
                                {
                                }
                            }
                            return true;
                        }
                        else
                        {
                            msg = "����ԭ�м�¼����,�˼�¼�����ڻ�ԭʼ״̬����ȷ";
                            return false;
                        }
                    }
                }
                else
                {
                    msg = "���Ҽ�¼ʧ��.";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public static bool CancelAppealDBTB(string fid, string db, string tb, string reason, string OtherReason, string Fcomment, string user, string userIP, out string msg)
        {
            if (reason != null && reason.Trim() != "")
            {
                reason = PublicRes.replaceMStr(reason);
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFTNEW"));
            MySqlAccess da2 = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                da.OpenConn();
                da2.OpenConn();
                if (OtherReason != null && OtherReason.Trim() != "")
                {
                    OtherReason = PublicRes.replaceMStr(OtherReason);
                }
                if (Fcomment != null && Fcomment.Trim() != "")
                {
                    Fcomment = PublicRes.replaceMStr(Fcomment);
                }

                //�޸�������Ҫ������. ȡ��֧��������Ҫclear_pps, ע������Ҫ���ණ��
                string strSql = "select * from " + db + "." + tb + " where FID='" + fid+"'";
                DataSet ds = da.dsGetTotalData(strSql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    HandleParameterByDBTB(ds, true);

                    DataRow dr = ds.Tables[0].Rows[0];
                    int fstate = Int32.Parse(dr["FState"].ToString());
                    int ftype = Int32.Parse(dr["FType"].ToString());
                    string fuin = dr["FUin"].ToString();
                    string email = dr["Femail"].ToString();
                    string submittime = DateTime.Parse(dr["FSubmitTime"].ToString()).ToString("yyyy��MM��dd��");
                    string mobile = dr["mobile_no"].ToString();
                    string mobile_bind = dr["FMobile"].ToString();//���ֻ���,ȡ���������

                    //��ȡ���ڵ��û�����furion 20060902
                    string username = PublicRes.GetNameFromQQ(fuin);
                    //��ע���û����ﴦ����
                    if ((username == null || username.Trim() == "") && ftype == 6)
                    {
                        username = "�װ��ĲƸ�ͨ�ͻ�";
                    }

                    if (username == null || username.Trim() == "")
                    {
                        msg = "ȡԭ���û���ʱ����";
                        username = fuin;
                        return false;
                    }
                    string EmailReason = reason;
                    string mesReason = "";
                    mesReason = reason + "&" + OtherReason;
                    if (ftype == 1)
                    {
                        EmailReason = reason + "<br>��ܰ��ʾ��</br>��������˺��Ѿ������ֻ����룬������Ч������֤�룬������ֱ�ӽ���<a href='https://www.tenpay.com/v2.0/main/getPwdByMobile_index.shtml'>�ֻ����һ�֧�������ַ</a>ͨ�����ֻ������һ�����֧�����롣<br>";
                    }

                    if (!SendAppealMessage(mobile, ftype, "", false, mesReason, out msg))
                    {
                        msg = "���Ͷ���ʧ�ܣ�" + msg;
                        return false;
                    }
                    //����ʧ���ʼ� ʧ�ܵ�һ��û�����Ĳ���
                     if (!SendAppealMail(email, ftype, false, username, submittime, "", "", EmailReason, OtherReason, fuin, out msg))
                    {
                        msg = "�����ʼ�ʧ�ܣ�" + msg;
                        return false;
                    }
                    else
                    {
                        if (fstate == 0)
                        {
                            strSql = "update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=0";
                        }
                        else if (fstate == 3)
                        {
                            strSql = "update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=3";
                        }
                        else if (fstate == 4)
                        {
                            strSql = " update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=4";
                        }
                        else if (fstate == 5)
                        {
                            strSql = "update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=5";
                        }
                        else if (fstate == 6)
                        {
                            strSql = " update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=6";
                        }
                        else if (fstate == 8)
                        {
                            strSql = " update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=8";
                        }
                        else
                        {
                            msg = "����ԭ�м�¼����,�˼�¼ԭʼ״̬����ȷ";
                            return false;
                        }
                        if (da.ExecSqlNum(strSql) == 1)
                        {
                            InputAppealNumber(user, "Fail", "appeal");
                            string IsSendAppeal = ConfigurationManager.AppSettings["IsSendAppeal"].ToString();
                            if (IsSendAppeal == "Yes")
                            {
                                try
                                {
                                    if (ftype == 1 || ftype == 5)//�������ݸ����ĿǰֻҪ���루1���͸��������ֻ���5��
                                    {
                                        fuin = System.Web.HttpUtility.UrlEncode(fuin, System.Text.Encoding.GetEncoding("GB2312"));
                                        string ftypestr = System.Web.HttpUtility.UrlEncode(ftype.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        //string fstatestr = System.Web.HttpUtility.UrlEncode(fstate.ToString(),System.Text.Encoding.GetEncoding("GB2312"));
                                        string cre_id = System.Web.HttpUtility.UrlEncode(dr["cre_id"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string clear_ppsstr = System.Web.HttpUtility.UrlEncode(dr["clear_pps"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string is_answer = System.Web.HttpUtility.UrlEncode(dr["labIsAnswer"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        mobile_bind = System.Web.HttpUtility.UrlEncode(mobile_bind, System.Text.Encoding.GetEncoding("GB2312"));
                                        email = System.Web.HttpUtility.UrlEncode(email, System.Text.Encoding.GetEncoding("GB2312"));
                                        string score = System.Web.HttpUtility.UrlEncode(dr["score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string is_pass = System.Web.HttpUtility.UrlEncode(dr["IsPass"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string client_ip = System.Web.HttpUtility.UrlEncode(dr["client_ip"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string reasonstr = System.Web.HttpUtility.UrlEncode(dr["reason"].ToString().Trim(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string fcomment = System.Web.HttpUtility.UrlEncode(Fcomment, System.Text.Encoding.GetEncoding("GB2312")); //��ע
                                        string fcheck_info = System.Web.HttpUtility.UrlEncode(reason + OtherReason, System.Text.Encoding.GetEncoding("GB2312"));   //�ܾ�ԭ��
                                        string fsubmittime = System.Web.HttpUtility.UrlEncode(dr["FSubmitTime"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string fchecktime = System.Web.HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Text.Encoding.GetEncoding("GB2312"));

                                        string Data = "fuin=" + fuin + "&ftype=" + ftypestr + "&fstate=2&cre_id=" + cre_id + "&clear_pps=" + clear_ppsstr +
                                            "&is_answer=" + is_answer + "&mobile_no=" + mobile_bind + "&email=" + email + "&score=" + score + "&is_pass=" + is_pass +
                                            "&client_ip=" + client_ip + "&fsubmittime=" + fsubmittime + "&fchecktime=" + fchecktime + "&reason=" + reasonstr + "&fcomment=" + fcomment + "&fcheck_info=" + fcheck_info;
                                        Data = System.Web.HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                                        Data = "protocol=user_appeal_notify&version=1.0&data=" + Data;

                                        SunLibrary.LoggerFactory.Get("KF_Service QueryInfo").Info("user_appeal_notify send:" + Data);

                                        System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                                        byte[] sendBytes = GB2312.GetBytes(Data);

                                        string IP = ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                                        string PORT = ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                                        TENCENT.OSS.CFT.KF.Common.UDP.GetUDPReplyNoReturnNotReturn(sendBytes, IP, Int32.Parse(PORT));
                                    }
                                }
                                catch
                                {
                                }
                            }
                            return true;
                        }
                        else
                        {
                            msg = "����ԭ�м�¼����,�˼�¼�����ڻ�ԭʼ״̬����ȷ";
                            return false;
                        }
                    }
                }
                else
                {
                    msg = "���Ҽ�¼ʧ��.";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
                da2.Dispose();
            }
        }

        public static bool DelAppeal(int fid, string Fcomment, string user, string userIP, out string msg)
        {
            msg = "";

            if (Fcomment != null && Fcomment.Trim() != "")
            {
                Fcomment = PublicRes.replaceMStr(Fcomment);
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                da.OpenConn();

                string strSql = " select * from t_tenpay_appeal_trans where Fid=" + fid;
                DataSet ds = da.dsGetTotalData(strSql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    HandleParameter(ds, true);

                    DataRow dr = ds.Tables[0].Rows[0];
                    int fstate = Int32.Parse(dr["FState"].ToString());
                    int ftype = Int32.Parse(dr["Ftype"].ToString());

                    if (fstate == 0)
                    {
                        strSql = "update t_tenpay_appeal_trans set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid=" + fid + " and Fstate=0";
                    }
                    else if (fstate == 3)
                    {
                        strSql = " update t_tenpay_appeal_trans set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid=" + fid + " and Fstate=3";
                    }
                    else if (fstate == 4)
                    {
                        strSql = " update t_tenpay_appeal_trans set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid=" + fid + " and Fstate=4";
                    }
                    else if (fstate == 5)
                    {
                        strSql = "update t_tenpay_appeal_trans set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid=" + fid + " and Fstate=5";
                    }
                    else if (fstate == 6)
                    {
                        strSql = " update t_tenpay_appeal_trans set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid=" + fid + " and Fstate=6";
                    }
                    else if (fstate == 8)
                    {
                        strSql = " update t_tenpay_appeal_trans set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid=" + fid + " and Fstate=8";
                    }
                    else
                    {
                        msg = "����ԭ�м�¼����,�˼�¼ԭʼ״̬����ȷ";
                        return false;
                    }

                    if (da.ExecSqlNum(strSql) == 1)
                    {
                        InputAppealNumber(user, "Delete", "appeal");
                        string IsSendAppeal = ConfigurationManager.AppSettings["IsSendAppeal"].ToString();
                        if (IsSendAppeal == "Yes")
                        {
                            try
                            {
                                if (ftype == 1 || ftype == 5)//�������ݸ����ĿǰֻҪ���루1���͸��������ֻ���5��
                                {
                                    string fuin = System.Web.HttpUtility.UrlEncode(dr["fuin"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string ftypestr = System.Web.HttpUtility.UrlEncode(ftype.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string fstatestr = System.Web.HttpUtility.UrlEncode(fstate.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string cre_id = System.Web.HttpUtility.UrlEncode(dr["cre_id"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string clear_ppsstr = System.Web.HttpUtility.UrlEncode(dr["clear_pps"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string is_answer = System.Web.HttpUtility.UrlEncode(dr["labIsAnswer"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string mobile_no = System.Web.HttpUtility.UrlEncode(dr["mobile_no"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string email = System.Web.HttpUtility.UrlEncode(dr["email"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string score = System.Web.HttpUtility.UrlEncode(dr["score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string is_pass = System.Web.HttpUtility.UrlEncode(dr["IsPass"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string client_ip = System.Web.HttpUtility.UrlEncode(dr["client_ip"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string reason = System.Web.HttpUtility.UrlEncode(dr["reason"].ToString().Trim(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string fcomment = System.Web.HttpUtility.UrlEncode(Fcomment, System.Text.Encoding.GetEncoding("GB2312")); //��ע
                                    string fcheck_info = System.Web.HttpUtility.UrlEncode("", System.Text.Encoding.GetEncoding("GB2312"));   //�ܾ�ԭ��
                                    string fsubmittime = System.Web.HttpUtility.UrlEncode(dr["FSubmitTime"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string fchecktime = System.Web.HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Text.Encoding.GetEncoding("GB2312"));

                                    string Data = "fuin=" + fuin + "&ftype=" + ftypestr + "&fstate=3&cre_id=" + cre_id + "&clear_pps=" + clear_ppsstr +
                                        "&is_answer=" + is_answer + "&mobile_no=" + mobile_no + "&email=" + email + "&score=" + score + "&is_pass=" + is_pass +
                                        "&client_ip=" + client_ip + "&fsubmittime=" + fsubmittime + "&fchecktime=" + fchecktime + "&reason=" + reason + "&fcomment=" + fcomment + "&fcheck_info=" + fcheck_info;
                                    Data = System.Web.HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                                    Data = "protocol=user_appeal_notify&version=1.0&data=" + Data;

                                    SunLibrary.LoggerFactory.Get("KF_Service QueryInfo").Info("user_appeal_notify send:" + Data);

                                    System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                                    byte[] sendBytes = GB2312.GetBytes(Data);

                                    string IP = ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                                    string PORT = ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                                    TENCENT.OSS.CFT.KF.Common.UDP.GetUDPReplyNoReturnNotReturn(sendBytes, IP, Int32.Parse(PORT));
                                }
                            }
                            catch
                            {
                            }
                        }
                        return true;
                    }
                    else
                    {
                        msg = "����ԭ�м�¼����,�˼�¼�����ڻ�ԭʼ״̬����ȷ";
                        return false;
                    }
                }
                else
                {
                    msg = "���Ҽ�¼ʧ��.";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public static bool DelAppealDBTB(string fid, string db, string tb, string Fcomment, string user, string userIP, out string msg)
        {
            msg = "";

            if (Fcomment != null && Fcomment.Trim() != "")
            {
                Fcomment = PublicRes.replaceMStr(Fcomment);
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFTNEW"));
            try
            {
                da.OpenConn();

                string strSql = "select * from " + db + "." + tb + " where FID='" + fid+"'";
                DataSet ds = da.dsGetTotalData(strSql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                   HandleParameterByDBTB(ds, true);

                    DataRow dr = ds.Tables[0].Rows[0];
                    int fstate = Int32.Parse(dr["FState"].ToString());
                    int ftype = Int32.Parse(dr["Ftype"].ToString());

                    if (fstate == 0)
                    {
                        strSql = "update " + db + "." + tb + " set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=0";
                    }
                    else if (fstate == 3)
                    {
                        strSql = " update " + db + "." + tb + " set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=3";
                    }
                    else if (fstate == 4)
                    {
                        strSql = " update " + db + "." + tb + " set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=4";
                    }
                    else if (fstate == 5)
                    {
                        strSql = "update " + db + "." + tb + " set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=5";
                    }
                    else if (fstate == 6)
                    {
                        strSql = " update " + db + "." + tb + " set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=6";
                    }
                    else if (fstate == 8)
                    {
                        strSql = " update " + db + "." + tb + " set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=8";
                    }
                    else
                    {
                        msg = "����ԭ�м�¼����,�˼�¼ԭʼ״̬����ȷ";
                        return false;
                    }

                    if (da.ExecSqlNum(strSql) == 1)
                    {
                        InputAppealNumber(user, "Delete", "appeal");
                        string IsSendAppeal = ConfigurationManager.AppSettings["IsSendAppeal"].ToString();
                        if (IsSendAppeal == "Yes")
                        {
                            try
                            {
                                if (ftype == 1 || ftype == 5)//�������ݸ����ĿǰֻҪ���루1���͸��������ֻ���5��
                                {
                                    string fuin = System.Web.HttpUtility.UrlEncode(dr["fuin"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string ftypestr = System.Web.HttpUtility.UrlEncode(ftype.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string fstatestr = System.Web.HttpUtility.UrlEncode(fstate.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string cre_id = System.Web.HttpUtility.UrlEncode(dr["cre_id"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string clear_ppsstr = System.Web.HttpUtility.UrlEncode(dr["clear_pps"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string is_answer = System.Web.HttpUtility.UrlEncode(dr["labIsAnswer"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string mobile_no = System.Web.HttpUtility.UrlEncode(dr["mobile_no"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string email = System.Web.HttpUtility.UrlEncode(dr["email"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string score = System.Web.HttpUtility.UrlEncode(dr["score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string is_pass = System.Web.HttpUtility.UrlEncode(dr["IsPass"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string client_ip = System.Web.HttpUtility.UrlEncode(dr["client_ip"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string reason = System.Web.HttpUtility.UrlEncode(dr["reason"].ToString().Trim(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string fcomment = System.Web.HttpUtility.UrlEncode(Fcomment, System.Text.Encoding.GetEncoding("GB2312")); //��ע
                                    string fcheck_info = System.Web.HttpUtility.UrlEncode("", System.Text.Encoding.GetEncoding("GB2312"));   //�ܾ�ԭ��
                                    string fsubmittime = System.Web.HttpUtility.UrlEncode(dr["FSubmitTime"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string fchecktime = System.Web.HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Text.Encoding.GetEncoding("GB2312"));

                                    string Data = "fuin=" + fuin + "&ftype=" + ftypestr + "&fstate=3&cre_id=" + cre_id + "&clear_pps=" + clear_ppsstr +
                                        "&is_answer=" + is_answer + "&mobile_no=" + mobile_no + "&email=" + email + "&score=" + score + "&is_pass=" + is_pass +
                                        "&client_ip=" + client_ip + "&fsubmittime=" + fsubmittime + "&fchecktime=" + fchecktime + "&reason=" + reason + "&fcomment=" + fcomment + "&fcheck_info=" + fcheck_info;
                                    Data = System.Web.HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                                    Data = "protocol=user_appeal_notify&version=1.0&data=" + Data;

                                    SunLibrary.LoggerFactory.Get("KF_Service QueryInfo").Info("user_appeal_notify send:" + Data);

                                    System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                                    byte[] sendBytes = GB2312.GetBytes(Data);

                                    string IP = ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                                    string PORT = ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                                    TENCENT.OSS.CFT.KF.Common.UDP.GetUDPReplyNoReturnNotReturn(sendBytes, IP, Int32.Parse(PORT));
                                }
                            }
                            catch
                            {
                            }
                        }
                        return true;
                    }
                    else
                    {
                        msg = "����ԭ�м�¼����,�˼�¼�����ڻ�ԭʼ״̬����ȷ";
                        return false;
                    }
                }
                else
                {
                    msg = "���Ҽ�¼ʧ��.";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

    }



    #endregion

    #region �޸�QQ��ѯ��

    public class ChangeQQQueryClass : Query_BaseForNET
    {
        public ChangeQQQueryClass(string userid, string qq)
        {
            string strWhere = " where 1=1 ";

            if (userid != null && userid.Trim() != "")
            {
                strWhere += " and FUserID='" + userid + "' ";
            }

            if (qq != null && qq.Trim() != "")
            {
                strWhere += " and (FOldQQ='" + qq + "' or FNewQQ='" + qq + "') ";
            }

            fstrSql = " select * from c2c_fmdb.t_changeqq_list " + strWhere + " order by FActionTime desc";

            fstrSql_count = " select count(*) from  c2c_fmdb.t_changeqq_list " + strWhere;

        }

    }

    #endregion

    #region �ֻ���ֵ����¼��ѯ
    public class FundCardQueryClass : Query_BaseForNET
    {
        public FundCardQueryClass(string u_ID, string fsupplylist, string Fcard_id)
        {
            string strwhere = " where 1=1 ";
            if (u_ID != "" && u_ID != null)
            {
                strwhere += "and Flistid='" + u_ID + "'";
            }

            if (fsupplylist != "" && fsupplylist != null)
            {
                strwhere += "  and  Fsupply_list='" + fsupplylist + "'";
            }

            if (Fcard_id != "" && Fcard_id != null)
            {
                strwhere += "  and  Fcard_id='" + Fcard_id + "'";
            }

            strwhere += "  order by  Fmodify_time  desc";

            fstrSql = "select * from charge_card_db.t_card_list " + strwhere;
            fstrSql_count = "select count(1) from charge_card_db.t_card_list  " + strwhere;
        }

    }
    #endregion

    #region ʵ����֤������

    public class UserClassClass : Query_BaseForNET
    {/*
		public static bool GetUserClassSumInfo(string user, DateTime begintime, DateTime endtime, out string msg)
		{
			msg = "";

			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("RU"));
			try
			{				
				da.OpenConn();

				string strSql = "select count(*) from authen_process_db.t_authening_info "
					+ " where Fstat=1 and Fcre_stat=2 and Fauthen_type=1 and Fcard_stat=1 " 
					//״̬Ϊ��������֤״̬Ϊ����֤����֤;��Ϊ�ͷ���֤�����п�״̬Ϊ����֤
					+ " and Fpickstate=0 "; //����δ�쵥

				int itmp = Int32.Parse(da.GetOneResult(strSql));
				msg = "����δ�������ߣ�[" + itmp + "];";

				strSql = "select count(*) from authen_process_db.t_authening_info where Fpickstate=1 and Fpickuser='" + user + "'"; //�쵥��				
				itmp = Int32.Parse(da.GetOneResult(strSql));
				msg += "�����쵥����[" + itmp + "];";

				strSql = "select count(*) from authen_process_db.t_authening_info where Fpickuser='" + user 
					+ "' and Fpickstate>1 and Fpicktime between '" + begintime.ToString("yyyy-MM-dd HH:mm:ss")
					+ "' and '" + endtime.ToString("yyyy-MM-dd HH:mm:ss") + "'"; //ָ��ʱ�����Ѵ�����

				itmp = Int32.Parse(da.GetOneResult(strSql));

				msg += "����" + begintime.ToString("yyyy-MM-dd") + "��������[" + itmp + "];";
				return true;
			}
			catch(Exception err)
			{
				msg = PublicRes.replaceMStr(err.Message);
				return false;
			}
			finally
			{
				da.Dispose();
			}
			
		}*/

        public static bool HandleParameterX(DataSet ds)
        {
            try
            {
                ds.Tables[0].Columns.Add("Fcre_typeName", typeof(String));
                ds.Tables[0].Columns.Add("FpickstateName", typeof(String));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    string tmp = dr["Fcre_type"].ToString();
                    if (tmp == "1")
                    {
                        dr["Fcre_typeName"] = "���֤";
                    }
                    else if (tmp == "2")
                    {
                        dr["Fcre_typeName"] = "����";
                    }
                    else if (tmp == "3")
                    {
                        dr["Fcre_typeName"] = "����֤";
                    }
                    else if (tmp == "100")
                    {
                        dr["Fcre_typeName"] = "�Թ���Ȩ";
                    }
                    else
                    {
                        dr["Fcre_typeName"] = "δ����";
                    }

                    tmp = dr["Fpickstate"].ToString();
                    if (tmp == "0")
                    {
                        dr["FpickstateName"] = "δ����";
                    }
                    else if (tmp == "1")
                    {
                        dr["FpickstateName"] = "���쵥";
                    }
                    else if (tmp == "2")
                    {
                        dr["FpickstateName"] = "��֤�ɹ�";
                    }
                    else if (tmp == "3")
                    {
                        dr["FpickstateName"] = "��֤ʧ��";
                    }
                    else
                    {
                        dr["FpickstateName"] = "δ����" + tmp;
                    }

                }
                return true;
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
        }

        private static bool SendAppealMail(string email, bool issucc, string param1, out string msg)
        {
            msg = "";

            if (PublicRes.IgnoreLimitCheck)
                return true;

            if (email == null || email.Trim() == "")
            {
                //furion 20060902 �Ƿ�֧�ֲ����ʼ�ȡ��������.Ҫô������,Ҫô���ؼ�
                return true;
            }

            string filename = ConfigurationManager.AppSettings["ServicePath"].Trim();
            if (!filename.EndsWith("\\"))
                filename += "\\";

            string title = ""; if (issucc) filename += "UserClassYes.htm"; else filename += "UserClassNo.htm";

            StreamReader sr = new StreamReader(filename, System.Text.Encoding.GetEncoding("GB2312"));
            try
            {
                string content = sr.ReadToEnd();

                content = String.Format(content, param1);

                //					MailMessage mail = new MailMessage();
                //					mail.From = "service@tenpay.com";        //������
                //					mail.To   = email;          //�ռ���;
                //					mail.BodyFormat = MailFormat.Html;
                //					mail.Body = content; //�ʼ�����
                //					mail.Subject  = title;           //�ʼ�����
                //	
                //					SmtpMail.SmtpServer = ConfigurationManager.AppSettings["OutSmtpServer"].ToString();
                //					SmtpMail.Send(mail);
                //					SmtpMail.SmtpServer = null;
                TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend newMail = new TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend();
                newMail.SendMail(email, "", title, content, true, null);

                return true;
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                sr.Close();
            }
        }



        private static string getCgiString(string instr)
        {
            if (instr == null || instr.Trim() == "")
                return "";

            //System.Text.Encoding enc = System.Text.Encoding.GetEncoding("GB2312");
            return System.Web.HttpContext.Current.Server.UrlDecode(instr).Replace("\r\n", "").Trim()
                .Replace("%3d", "=").Replace("%20", " ").Replace("%26", "&");
        }

        public static bool CommitAppeal(Finance_Header fh, string[] result, string dbstr, out string msg)
        {
            msg = "";
            bool flag = true;

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString(dbstr));
            try
            {
                if (result.Length == 0)
                {
                    return true;
                }

                da.OpenConn();
                string strSql = "";
                foreach (string strresult in result)
                {
                    //�ɹ���ʧ�ܣ�����Ҫ���ʼ����쳣��ת��̨ʱ���÷��ʼ���
                    //���⣬ʧ��ʱ���ʼ�ʧ��
                    string[] split = strresult.Split(';');
                    if (split.Length != 3)
                        continue;

                    long id = long.Parse(split[0].Trim());
                    int index = Int32.Parse(split[1].Trim());
                    string idcard = split[2].Trim().ToUpper();

                    //if(index == 0 && idcard.Length != 5)
                    if (index == -1 && idcard.Length != 5)
                        continue;

                    //�¼��ж�
                    if (idcard.Length == 5)
                        index = 0;
                    else
                        index += 1;

                    int emailflag = 0; //0���÷��ʼ���1���ɹ��ʼ���2��ʧ���ʼ���
                    string memo = "";

                    strSql = " select Fuid from authen_process_db.t_authening_info where Flist_id=" + id;
                    string fuin = da.GetOneResult(strSql);

                    if (fuin == null || fuin.Trim() == "")
                    {
                        msg += "��¼{" + id + "}���ʺŶ�ȡ�д�;";
                        flag = false;
                        continue;
                    }

                    //������֧��0�жϺ���λ�Ƿ�һ����1ֱ�Ӿܾ���2ת��̨����
                    if (index == 1)
                    {
                        strSql = " update authen_process_db.t_authening_info set Fpicktime=now(),Fmemo='�ύ֤�����ϸ�',Fpickstate=3"
                            + " where Flist_id=" + id + " and Fpickstate=1";
                        emailflag = 2;
                        memo = "�ύ֤�����ϸ�";
                    }
                    else if (index == 0)
                    {

                        // TODO: 1�ͻ���Ϣ��������
                        /*
                        strSql = " select Fcreid from " +  PublicRes.GetTName("t_user_info",fuin) + " where fuid=" + fuin;
                        //string allidcard = PublicRes.ExecuteOne(strSql,"YW");
                        string allidcard = PublicRes.ExecuteOne(strSql,"ZL").ToUpper();
                        */

                        string Msg = "";
                        strSql = "uid=" + fuin;
                        string allidcard = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fcreid", out Msg);

                        if (allidcard == null || allidcard.Trim() == "")
                        {
                            msg += "��¼{" + id + "}���ʺŶ�ȡ�д�;" + Msg;
                            flag = false;
                            continue;
                        }

                        if (allidcard.EndsWith(idcard))
                        {

                            //����Ż�ɹ�
                            strSql = " update authen_process_db.t_authening_info set Fpicktime=now(),Fmemo='֤���ϸ�',Fpickstate=2"
                                + " where Flist_id=" + id + " and Fpickstate=1";


                            emailflag = 1;
                            memo = "֤���ϸ�";

                        }
                        else //furion 20071106 �¼���������������ʺ��д����֣�����쳣ת��̨��
                        {
                            //ʧ�ܡ�
                            strSql = " update authen_process_db.t_authening_info set Fpicktime=now(),Fmemo='�ύ֤�����ϸ�"
                                + idcard + "',Fpickstate=3"
                                + " where Flist_id=" + id + " and Fpickstate=1";

                            emailflag = 2;

                        }//if allidcard
                    }//if index		

                    //���ýӿ�,���ʧ������,�����ʼ�.
                    string strInfoSql = "select Fqqid,Fpath,Fstandby3 from authen_process_db.t_authening_info where Flist_id=" + id;
                    DataTable dtinfo = da.GetTable(strInfoSql);

                    if (dtinfo == null || dtinfo.Rows.Count != 1)
                    {
                        msg += "��¼{" + id + "}����Ϣ��ȡ�д�;";
                        flag = false;
                        continue;
                    }

                    if (dtinfo.Rows[0]["Fstandby3"].ToString() != "")  //��mandy��������   opr_state��0δ���壬1ȷ�ϣ�2����
                    {
                        string inmsg = "uid=" + fuin;
                        inmsg += "&opr_state=" + emailflag;
                        inmsg += "&memo=" + memo;
                        inmsg += "&des_path=" + PublicRes.ICEEncode(dtinfo.Rows[0]["Fpath"].ToString().Trim());
                        inmsg += "&operator=" + fh.UserName;

                        string reply;
                        short sresult;

                        if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("au_sure_cre_check_service", inmsg, true, out reply, out sresult, out msg))
                        {
                            if (sresult != 0)
                            {
                                msg = "au_sure_cre_check_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                                flag = false;
                                continue;
                            }
                            else
                            {
                                if (reply.StartsWith("result=0"))
                                {

                                }
                                else
                                {
                                    msg = "au_sure_cre_check_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                                    flag = false;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            msg = "au_sure_cre_check_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                            flag = false;
                            continue;
                        }
                    }
                    else   //��������
                    {
                        string inmsg = "uin=" + dtinfo.Rows[0]["Fqqid"].ToString().Trim();
                        inmsg += "&opr_state=" + emailflag;
                        inmsg += "&opr_type=1";
                        inmsg += "&des_path=" + PublicRes.ICEEncode(dtinfo.Rows[0]["Fpath"].ToString().Trim());
                        inmsg += "&operator=" + fh.UserName;

                        string reply;
                        short sresult;

                        if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("au_sure_creinfo_service", inmsg, true, out reply, out sresult, out msg))
                        {
                            if (sresult != 0)
                            {
                                msg = "au_sure_creinfo_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                                flag = false;
                                continue;
                            }
                            else
                            {
                                if (reply.StartsWith("result=0"))
                                {

                                }
                                else
                                {
                                    msg = "au_sure_creinfo_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                                    flag = false;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            msg = "au_sure_creinfo_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                            flag = false;
                            continue;
                        }
                    }

                    int iresult = da.ExecSqlNum(strSql);
                    if (iresult != 1)
                    {
                        msg += "���¼�¼{" + id + "}ʱδ�ɹ�;";
                        flag = false;
                        continue;
                    }

                    //�����ʼ����ݳɹ���ʧ�ܣ������ʼ����������ʧ�ܣ���ת��̨����
                    //if(emailflag > 0)
                    if (emailflag < 0) //���ٷ��ʼ�
                    {
                        //ȡ�ô������ĸ���Ҫ��Ϣ.
                        strSql = " select Fuid,Ftruename from authen_process_db.t_authening_info where Flist_id=" + id;
                        DataSet ds = da.dsGetTotalData(strSql);

                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                        {
                            string username = ds.Tables[0].Rows[0]["Ftruename"].ToString();
                            //��ѯ����email

                            /*
                            strSql = "select Femail from " + PublicRes.GetTName("t_user_info",fuin) + " where Fuid=" + fuin;
                            string email = PublicRes.ExecuteOne(strSql,"ZL");
                            */

                            string Msg = "";
                            strSql = "uid=" + fuin;
                            string email = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Femail", out Msg);

                            if (email == null || email.Trim() == "")
                                continue;

                            bool resultfalg = false;
                            if (emailflag == 1)
                                resultfalg = true;

                            string tmpmsg = "";

                            if (!SendAppealMail(email, resultfalg, username, out tmpmsg))
                            {
                                msg += "�����ʼ�ʧ�ܣ�" + tmpmsg;
                                flag = false;
                                continue;
                            }
                        }// if ds.rows.count > 0
                    } //if emailflag > 0

                }//foreach

                return true;

            }//try
            finally
            {
                da.Dispose();
            }
        }

        public static void InputUserClasslNumber(string User, string Type, string OperationType)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                da.OpenConn();
                string sql = "select count(1) from t_tenpay_appeal_kf_total where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType = '" + OperationType + "'";
                if (da.GetOneResult(sql) == "1")
                {
                    if (Type == "Success")
                        sql = "update t_tenpay_appeal_kf_total set UserClassSuccessNum = UserClassSuccessNum + 1 where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType = '" + OperationType + "'";
                    else if (Type == "Fail")
                        sql = "update t_tenpay_appeal_kf_total set UserClassFailNum = UserClassFailNum + 1 where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType = '" + OperationType + "'";
                    else if (Type == "Other")
                        sql = "update t_tenpay_appeal_kf_total set UserClassOtherNum = UserClassOtherNum + 1 where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType = '" + OperationType + "'";
                    else
                    {
                        throw new Exception("û����������!");
                    }
                }
                else
                {
                    if (Type == "Success")
                        sql = "insert into t_tenpay_appeal_kf_total(User,OperationDay,UserClassSuccessNum,OperationType) values('" + User + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "',1,'" + OperationType + "')";
                    else if (Type == "Fail")
                        sql = "insert into t_tenpay_appeal_kf_total(User,OperationDay,UserClassFailNum,OperationType) values('" + User + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "',1,'" + OperationType + "')";
                    else if (Type == "Other")
                        sql = "insert into t_tenpay_appeal_kf_total(User,OperationDay,UserClassOtherNum,OperationType) values('" + User + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "',1,'" + OperationType + "')";
                    else
                    {
                        throw new Exception("û����������!");
                    }
                }

                da.ExecSqlNum(sql);
            }
            catch
            {
                throw new Exception("��¼����ͳ��ʧ�ܣ�");
            }
            finally
            {
                da.Dispose();
            }
        }

        public static bool UserClassConfirm(int flist_id, string dbstr, string user, out string msg)
        {
            msg = "";
            int emailflag = 1; //0���÷��ʼ���1���ɹ��ʼ���2��ʧ���ʼ���

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString(dbstr));
            try
            {
                // 2012/4/18 �Ķ�sql���������֤ʧ�ܵļ�¼���ж���ʵ����֤��
                da.OpenConn();
                //string strSql = " update authen_process_db.t_authening_info set Fpicktime=now(),Fmemo='֤���ϸ�',Fpickstate=2"
                //+ " where Flist_id=" + flist_id.ToString() + " and (Fpickstate=0 or Fpickstate=1)";

                string strSql = " update authen_process_db.t_authening_info set Fpicktime=now(),Fmemo='֤���ϸ�',Fpickstate=2"
                    + " where Flist_id=" + flist_id.ToString() + " and (Fpickstate=0 or Fpickstate=1 or Fpickstate=3)";

                //���ýӿ�,���ʧ������,�����ʼ�.
                string strInfoSql = "select Fuid,Fqqid,Fpath,Fstandby3 from authen_process_db.t_authening_info where Flist_id=" + flist_id.ToString();
                DataTable dtinfo = da.GetTable(strInfoSql);

                if (dtinfo == null || dtinfo.Rows.Count != 1)
                {
                    msg += "��¼{" + flist_id.ToString() + "}����Ϣ��ȡ�д�;";
                    return false;
                }

                if (dtinfo.Rows[0]["Fstandby3"].ToString() != "")  //��mandy��������   opr_state��0δ���壬1ȷ�ϣ�2����
                {
                    string inmsg = "uid=" + dtinfo.Rows[0]["Fuid"].ToString();
                    inmsg += "&opr_state=" + emailflag;
                    inmsg += "&memo=֤���ϸ�";
                    inmsg += "&des_path=" + PublicRes.ICEEncode(dtinfo.Rows[0]["Fpath"].ToString().Trim());
                    inmsg += "&operator=" + user;

                    string reply;
                    short sresult;

                    if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("au_sure_cre_check_service", inmsg, true, out reply, out sresult, out msg))
                    {
                        if (sresult != 0)
                        {
                            msg = "au_sure_cre_check_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                            return false;
                        }
                        else
                        {
                            if (reply.StartsWith("result=0"))
                            {

                            }
                            else
                            {
                                msg = "au_sure_cre_check_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        msg = "au_sure_cre_check_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                        return false;
                    }
                }
                else
                {
                    string inmsg = "uin=" + dtinfo.Rows[0]["Fqqid"].ToString().Trim();
                    inmsg += "&opr_state=" + emailflag;
                    inmsg += "&opr_type=1";
                    inmsg += "&des_path=" + PublicRes.ICEEncode(dtinfo.Rows[0]["Fpath"].ToString().Trim());
                    inmsg += "&operator=" + user;

                    string reply;
                    short sresult;

                    if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("au_sure_creinfo_service", inmsg, true, out reply, out sresult, out msg))
                    {
                        if (sresult != 0)
                        {
                            msg = "au_sure_creinfo_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                            return false;
                        }
                        else
                        {
                            if (reply.StartsWith("result=0"))
                            {

                            }
                            else
                            {
                                msg = "au_sure_creinfo_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                                InputUserClasslNumber(user, "Other", "appeal");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        msg = "au_sure_creinfo_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                        return false;
                    }
                }

                int iresult = da.ExecSqlNum(strSql);
                if (iresult != 1)
                {
                    msg += "���¼�¼{" + flist_id.ToString() + "}ʱδ�ɹ�;";
                    return false;
                }

                InputUserClasslNumber(user, "Success", "appeal");
                return true;
            }
            finally
            {
                da.Dispose();
            }
        }


        public static bool UserClassCancel(int flist_id, string reason, string OtherReason, string dbstr, string user, out string msg)
        {
            msg = "";
            int emailflag = 2; //0���÷��ʼ���1���ɹ��ʼ���2��ʧ���ʼ���

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString(dbstr));
            try
            {
                da.OpenConn();
                string strSql = " update authen_process_db.t_authening_info set Fpicktime=now(),Fmemo='" + reason + OtherReason + "',Fpickstate=3"
                    + " where Flist_id=" + flist_id.ToString() + " and (Fpickstate=0 or Fpickstate=1)";

                //���ýӿ�,���ʧ������,�����ʼ�.
                string strInfoSql = "select Fuid,Fqqid,Fpath,Fstandby3 from authen_process_db.t_authening_info where Flist_id=" + flist_id.ToString();
                DataTable dtinfo = da.GetTable(strInfoSql);

                if (dtinfo == null || dtinfo.Rows.Count != 1)
                {
                    msg += "��¼{" + flist_id.ToString() + "}����Ϣ��ȡ�д�;";
                    return false;
                }

                if (dtinfo.Rows[0]["Fstandby3"].ToString() != "")  //��mandy��������   opr_state��0δ���壬1ȷ�ϣ�2����
                {
                    string inmsg = "uid=" + dtinfo.Rows[0]["Fuid"].ToString();
                    inmsg += "&opr_state=" + emailflag;
                    inmsg += "&memo=" + reason + OtherReason;
                    inmsg += "&des_path=" + PublicRes.ICEEncode(dtinfo.Rows[0]["Fpath"].ToString().Trim());
                    inmsg += "&operator=" + user;

                    string reply;
                    short sresult;

                    if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("au_sure_cre_check_service", inmsg, true, out reply, out sresult, out msg))
                    {
                        if (sresult != 0)
                        {
                            msg = "au_sure_cre_check_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                            return false;
                        }
                        else
                        {
                            if (reply.StartsWith("result=0"))
                            {

                            }
                            else
                            {
                                msg = "au_sure_cre_check_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        msg = "au_sure_cre_check_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg + "��reply=" + reply;
                        return false;
                    }
                }
                else
                {
                    string inmsg = "uin=" + dtinfo.Rows[0]["Fqqid"].ToString().Trim();
                    inmsg += "&opr_state=" + emailflag;
                    inmsg += "&opr_type=1";
                    inmsg += "&des_path=" + PublicRes.ICEEncode(dtinfo.Rows[0]["Fpath"].ToString().Trim());
                    inmsg += "&operator=" + user;

                    string reply;
                    short sresult;

                    if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("au_sure_creinfo_service", inmsg, true, out reply, out sresult, out msg))
                    {
                        if (sresult != 0)
                        {
                            msg = "au_sure_creinfo_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg;
                            return false;
                        }
                        else
                        {
                            if (reply.StartsWith("result=0"))
                            {

                            }
                            else
                            {
                                msg = "au_sure_creinfo_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        msg = "au_sure_creinfo_service�ӿ�ʧ�ܣ�result=" + sresult + "��msg=" + msg;
                        return false;
                    }
                }

                int iresult = da.ExecSqlNum(strSql);
                if (iresult != 1)
                {
                    msg += "���¼�¼{" + flist_id.ToString() + "}ʱδ�ɹ�;";
                    return false;
                }

                InputUserClasslNumber(user, "Fail", "appeal");

                return true;
            }
            finally
            {
                da.Dispose();
            }
        }

        /*
                public static DataSet UserClassClassPickTJ(Finance_Header fh, string dbstr,DateTime begindate, DateTime enddate, out string msg)
                {
                    msg = "";
                    MySqlAccess da = new MySqlAccess(PublicRes.GetConnString(dbstr));
                    try
                    {
                        da.OpenConn();

                        string strSql = " select Fpickuser,Fpickstate,count(*) from authen_process_db.t_authening_info "
                            + " where Fpicktime>='" + begindate.ToString("yyyy-MM-dd HH:mm:ss")
                            //+ "' and FSubmitTime<='" + enddate.ToString("yyyy-MM-dd HH:mm:ss") + "' and FState >0 "
                            + "' and Fpicktime<='" + enddate.ToString("yyyy-MM-dd HH:mm:ss") + "' and FpickState>1 "
                            + " group by Fpickuser,Fpickstate ";

                        DataSet ds = new DataSet();
                        DataTable dtresult = new DataTable();
                        ds.Tables.Add(dtresult);
                        dtresult.Columns.Add("Fpickuser",typeof(System.String));
                        dtresult.Columns.Add("FCommitCount",typeof(System.String));
                        dtresult.Columns.Add("FCancelCount",typeof(System.String));
                        dtresult.Columns.Add("FTotalCount",typeof(System.String));

                        DataTable dt = da.GetTable(strSql);
                        if(dt == null || dt.Rows.Count == 0)
                            return ds;

				
                        int commitcount = 0;
                        int cancelcount = 0;
                        int totalcount = 0;

                        int rowindex = 0;
                        Hashtable ht = new Hashtable();
                        foreach(DataRow dr in dt.Rows)
                        {
                            string struser = "";
                            if(dr["Fpickuser"] != null)
                            {
                                struser = dr["Fpickuser"].ToString().Trim();
                            }
                            else
                                continue;

                            int state = Int32.Parse(dr["Fpickstate"].ToString());
                            int count = Int32.Parse(dr[2].ToString());

                            if(ht.ContainsKey(struser))
                            {
                                //���û��ļ�¼�Ѵ���
                                int currrowindex = Int32.Parse(ht[struser].ToString());
                                dtresult.Rows[currrowindex]["FTotalCount"] = Int32.Parse(dtresult.Rows[currrowindex]["FTotalCount"].ToString()) + count;
						
                                switch(state)
                                {
                                    case 2:
                                        dtresult.Rows[currrowindex]["FCommitCount"] = count;	
                                        commitcount += count;
                                        break;
                                    case 3:
                                        dtresult.Rows[currrowindex]["FCancelCount"] = count;	
                                        cancelcount += count;
                                        break;
                                    default:
                                        break;
                                }
                            }//if containskey
                            else
                            {
                                //���û���¼������
                                //�����û�
                                DataRow drresult = dtresult.NewRow();
                                drresult["Fpickuser"] = struser;
                                drresult["FTotalCount"] = count;
                                drresult["FCommitCount"] = 0;
                                drresult["FCancelCount"] = 0;

                                switch(state)
                                {
                                    case 2:
                                        drresult["FCommitCount"] = count;	
                                        commitcount += count;
                                        break;
                                    case 3:
                                        drresult["FCancelCount"] = count;			
                                        cancelcount += count;
                                        break;
                                    default:
                                        break;
                                }

                                dtresult.Rows.Add(drresult);

                                ht.Add(struser,rowindex);
                                rowindex ++;
                            }//if containskey else
                        } //for each

                        totalcount = commitcount + cancelcount ;
                        DataRow drtotal = dtresult.NewRow();
                        drtotal["Fpickuser"] = "�ϼ�";
                        drtotal["FCommitCount"] = commitcount;
                        drtotal["FCancelCount"] = cancelcount;	
                        drtotal["FTotalCount"] = totalcount;

                        dtresult.Rows.Add(drtotal);

                        return ds;
                    }
                    catch(Exception err)
                    {
                        msg = err.Message;
                        return null;
                    }
                    finally
                    {
                        da.Dispose();
                    }
			
                }

                public static DataSet GetPickList(string fuin, int imax, string dbstr, int flag)
                {
                    DataSet ds = new DataSet();
                    MySqlAccess da = new MySqlAccess(PublicRes.GetConnString(dbstr));
                    try
                    {
                        fuin = fuin.Replace("\\","\\\\").Replace("'","\\'").Trim();
                        da.OpenConn();
                        //ȡ��¼,�����ñ��. ��������������.
                        string strSql = "";
                        string begindate = DateTime.Today.AddDays(-3).ToString("yyyy-MM-dd 00:00:00");
                        string enddate = DateTime.Today.ToString("yyyy-MM-dd 23:59:59");

                        if(flag == 0)
                        {
                            strSql = " select count(*) from authen_process_db.t_authening_info where Fpickuser='" + fuin + "' and Fpickstate=1 "
                                + " and  Fstat=1 and Fcre_stat=2 and Fauthen_type=1 and Fcard_stat=1 and Fmodify_time between '" + begindate + "' and '" + enddate + "'";

                            int icurr = Int32.Parse(da.GetOneResult(strSql));
                            if(icurr < imax)
                            {
                                icurr = imax - icurr;
                                strSql = " update authen_process_db.t_authening_info set FPickUser='" + fuin + "',Fpickstate=1 "
                                    + " where Fstat=1 and Fcre_stat=2 and Fauthen_type=1 and Fcard_stat=1 " 
                                    //״̬Ϊ��������֤״̬Ϊ����֤����֤;��Ϊ�ͷ���֤�����п�״̬Ϊ����֤
                                    + " and Fpickstate=0 and Fmodify_time between '" + begindate + "' and '" + enddate + "' limit " + icurr;
                                da.ExecSql(strSql);
                            }

                            strSql = " select Flist_id,Fcre_type,Fpath,FPickState from authen_process_db.t_authening_info "
                                + " where FPickUser='" + fuin + "' and Fpickstate=1  "
                                + " and  Fstat=1 and Fcre_stat=2 and Fauthen_type=1 and Fcard_stat=1 and Fmodify_time between '" + begindate + "' and '" + enddate + "' limit " + imax;

                            ds = da.dsGetTotalData(strSql);
                        }
                        else if(flag == 1)
                        {
                            strSql = " select Flist_id,Fcre_type,Fpath,FPickState from authen_process_db.t_authening_info "
                                + " where Fstat=1 and Fcre_stat=2 and Fauthen_type=1 and Fcard_stat=1 " 
                                //״̬Ϊ��������֤״̬Ϊ����֤����֤;��Ϊ�ͷ���֤�����п�״̬Ϊ����֤
                                + " and Fpickstate=0  and Fmodify_time between '" + begindate + "' and '" + enddate + "' limit " + imax;
                            //where FState=0 and FPickUser is null and  FType<4  limit " + imax;

                            ds = da.dsGetTotalData(strSql);

                            if(ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                            {
                                foreach(DataRow dr in ds.Tables[0].Rows)
                                {
                                    string tmpfid = dr["Flist_id"].ToString().Trim();

                                    strSql = " update authen_process_db.t_authening_info set FPickUser='" + fuin + "',Fpickstate=1 "
                                        + " where Flist_id='" + tmpfid + "' and Fpickstate=0 ";

                                    da.ExecSql(strSql);
                                }
                            }
					
                        }
				
                    }
                    catch(Exception err)
                    {
                        throw new LogicException(err.Message);
                    }
                    finally
                    {
                        da.Dispose();
                    }

                    return ds;
                }

                */

        public UserClassClass(string u_BeginTime, string u_EndTime, string fuin, int fstate, string QQType, int SortType)
        {

            string strWhere = " where Fstat=1 and Fcre_stat=2 and Fauthen_type=1 and Fcard_stat=1 ";

            if (fuin != null && fuin != "")
            {
                strWhere += " and Fqqid = '" + fuin + "' ";
            }

            if (u_BeginTime != null && u_BeginTime != "")
            {
                strWhere += " and Fmodify_time between '" + u_BeginTime + "' and '" + u_EndTime + "' ";
            }

            if (fstate != 99)
            {
                strWhere += " and Fpickstate=" + fstate + "  ";
            }

            //"" ��������; "0" �ǻ�Ա; "1" ��ͨ��Ա; "2" VIP��Ա
            //�û��ȼ���vip��ʶ(Fstandby1)
            //��cft��Ա���ȼ��� 0-6
            //cft��Ա���ȼ��� 100-106
            //vip��Ա���ȼ��� 200-206
            //����1���²����������ͨ��Ա��ͬ�ǻ�Ա����400-406
            //0��Ĭ��ֵ
            if (QQType == "0")
            {
                strWhere += " and (Fstandby1 < 100 or Fstandby1 >= 300) ";
            }
            else if (QQType == "1")
            {
                strWhere += " and Fstandby1>=100 and Fstandby1<200 ";
            }
            else if (QQType == "2")
            {
                strWhere += " and Fstandby1>=200 and Fstandby1<300 ";
            }

            if (SortType != 99)
            {
                if (SortType == 0)   //����ʱ��С����
                    strWhere += " order by Fcreate_time asc ";
                if (SortType == 1)   //����ʱ���С
                    strWhere += " order by Fcreate_time desc ";
            }

            fstrSql = "select Fpickstate,Fpickuser,Fpicktime,Fpath,Fcre_type,Fqqid,Fmemo,Flist_id,Fcreate_time from authen_process_db.t_authening_info "
                + strWhere;
            fstrSql_count = "select count(1) from authen_process_db.t_authening_info " + strWhere;
        }

        public UserClassClass(string fuin, string Flag)
        {
            fstrSql = "select Fpickstate,Fpickuser,Fcard_stat,Fpicktime,Fpath,Fcre_type,Fqqid,Fmemo,Flist_id,Fcreate_time,Fcre_stat from authen_process_db.t_authening_info where Fqqid = '" + fuin + "' and Fauthen_type=1 " +
                "order by Fmodify_time ";
            fstrSql_count = "select count(1) from authen_process_db.t_authening_info where Fqqid = '" + fuin + "' and Fauthen_type=1 ";
        }

        public UserClassClass(int flist_id)
        {
            fstrSql = "select Fpickstate,Fpickuser,Fcard_stat,Fpicktime,Fpath,Fcre_type,Fqqid,Fmemo,Flist_id,Fcreate_time from authen_process_db.t_authening_info where flist_id=" + flist_id.ToString();
            fstrSql_count = "select count(1) from authen_process_db.t_authening_info where flist_id=" + flist_id.ToString();
        }

        public static DataSet GetLockList(DateTime BeginDate, DateTime EndDate, int fstate, string username, int Count)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("RU"));
            da.OpenConn();

            DataSet ds = new DataSet();

            try
            {
                string strWhere = "where Fstat=1 and Fcre_stat=2 and Fauthen_type=1 and Fcard_stat=1 " +
                    "and Fmodify_time between '" + BeginDate.ToString("yyyy-MM-dd 00:00:00") + "' and '" + EndDate.ToString("yyyy-MM-dd 23:59:59") + "' ";

                if (fstate == 99)
                    fstate = 0;

                if (fstate != 0 && fstate != 1)
                {
                    throw new Exception("��״̬�����������쵥!");
                }
                strWhere += " and Fpickstate=" + fstate + "  ";

                if (username == null || username == "")
                {
                    throw new Exception("�����쵥�˲�����Ϊ��!");
                }

                string strSql = "select Fuid,Flist_id,Fpickstate,Fpickuser,Fcard_stat,Fpicktime,Fpath,Fcre_type,Fqqid,Fmemo,Fcreate_time from authen_process_db.t_authening_info "
                    + strWhere + "  order by Fmodify_time limit " + Count;

                ds = da.dsGetTotalData(strSql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string WhereStr = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WhereStr += "'" + dr["Flist_id"].ToString().Trim() + "',";
                    }
                    WhereStr = WhereStr.Substring(0, WhereStr.Length - 1);

                    strSql = " update authen_process_db.t_authening_info set FPickUser='" + username + "',FPickTime=now(),Fpickstate=1 where Flist_id in(" + WhereStr + ")";

                    da.ExecSql(strSql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                da.Dispose();
            }

            return ds;

        }



        public static DataSet GetDeleteList(string Fqqid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("RU"));
            da.OpenConn();

            try
            {
                string strSql = "select Fauthen_operator,Fqqid,Fmemo,Fmodify_time from authen_process_db.t_authening_info " +
                    "where Fqqid='" + Fqqid + "' and Fmemo='�ͷ�ɾ��' order by Fmodify_time desc limit 0,100";

                return da.dsGetTotalData(strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                da.Dispose();
            }

        }

    }


    #endregion

    #region  ϵͳ������
    public class SysBulletinClass
    {
        public static bool GoPrior(string fid, string userip, out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                da.OpenConn();
                string strSql = "select FOrder from c2c_db_inc.t_bulletin_info where FID=" + fid
                    + " and Fstate=1 and Flist_state=1";
                int noworder = Int32.Parse(da.GetOneResult(strSql));

                strSql = "select FOrder from c2c_db_inc.t_bulletin_info where FSysID=(select FSysID "
                    + " from c2c_db_inc.t_bulletin_info where FID=" + fid + ") and FOrder>"
                    + noworder + " and Fstate=1 and Flist_state=1 order by FOrder limit 1";

                string obj = da.GetOneResult(strSql);

                if (obj != null)
                {
                    int maxorder = Int32.Parse(obj);

                    strSql = "update c2c_db_inc.t_bulletin_info set FOrder=" + noworder
                        + ",FModify_time=now(),FIP='" + userip + "' where FOrder=" + maxorder
                        + " and Fstate=1 and Flist_state=1";
                    if (da.ExecSqlNum(strSql) == 1)
                    {
                        strSql = "update c2c_db_inc.t_bulletin_info set FOrder=" + maxorder
                            + ",FModify_time=now(),FIP='" + userip + "' where FID=" + fid
                            + " and Fstate=1 and Flist_state=1";
                        if (da.ExecSqlNum(strSql) == 1)
                            return true;
                        else
                        {
                            msg = "��һ����¼���³���";

                            strSql = "update c2c_db_inc.t_bulletin_info set FOrder=" + maxorder
                                + ",FModify_time=now(),FIP='" + userip
                                + "' where FOrder=" + noworder + " and FID<>" + fid
                                + " and Fstate=1 and Flist_state=1";
                            da.ExecSql(strSql);

                            return false;
                        }
                    }
                    else
                        return true;
                }
                else
                    return true;
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }

        }

        public static bool GoNext(string fid, string userip, out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                da.OpenConn();
                string strSql = "select FOrder from c2c_db_inc.t_bulletin_info where FID="
                    + fid + " and Fstate=1 and Flist_state=1";

                int noworder = Int32.Parse(da.GetOneResult(strSql));

                strSql = "select FOrder from c2c_db_inc.t_bulletin_info where FSysID=(select FSysID "
                    + " from c2c_db_inc.t_bulletin_info where FID=" + fid + ") and FOrder<"
                    + noworder + " and Fstate=1 and Flist_state=1 order by FOrder desc limit 1";

                string obj = da.GetOneResult(strSql);

                if (obj != null)
                {
                    int minorder = Int32.Parse(obj);

                    strSql = "update c2c_db_inc.t_bulletin_info set FOrder=" + noworder
                        + ",FModify_time=now(),FIP='" + userip + "' where FOrder=" + minorder
                        + " and Fstate=1 and Flist_state=1";
                    if (da.ExecSqlNum(strSql) == 1)
                    {
                        strSql = "update c2c_db_inc.t_bulletin_info set FOrder=" + minorder
                            + ",FModify_time=now(),FIP='" + userip + "' where FID=" + fid
                            + " and Fstate=1 and Flist_state=1";

                        if (da.ExecSqlNum(strSql) == 1)
                            return true;
                        else
                        {
                            msg = "��һ����¼���³���";

                            strSql = "update c2c_db_inc.t_bulletin_info set FOrder=" + minorder
                                + ",FModify_time=now(),FIP='" + userip
                                + "' where FOrder=" + noworder + " and FID<>" + fid + " and Fstate=1 and Flist_state=1";
                            da.ExecSql(strSql);

                            return false;
                        }
                    }
                    else
                        return true;
                }
                else
                    return true;
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public static bool GoHistory(string fid, string userip, out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                da.OpenConn();
                string strSql = "select FSysID from c2c_db_inc.t_bulletin_info where FID=" + fid
                    + " and Flist_state=1 and Fstate=1";

                string obj = da.GetOneResult(strSql);

                if (obj != null)
                {
                    int sysid = Int32.Parse(obj);
                    if (sysid != 2)
                    {
                        msg = "ֻ�вƸ�ͨϵͳ��ҳ�������ת�Ƶ���ʷ";
                        return false;
                    }
                    else
                    {
                        strSql = "update c2c_db_inc.t_bulletin_info set FSysID=3,FModify_time=now(),FIP='" + userip + "' where FID=" + fid;
                        if (da.ExecSqlNum(strSql) == 1)
                        {
                            return true;
                        }
                        else
                        {
                            msg = "���¼�¼����";
                            return false;
                        }
                    }
                }
                else
                {
                    msg = "��������ȷ�ļ�¼";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public static bool Del(string fid, string userip, out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                da.OpenConn();
                string strSql = "update c2c_db_inc.t_bulletin_info set Flist_state=2,FModify_time=now(),FIP='" + userip + "' where FID=" + fid;

                da.ExecSql(strSql);
                return true;
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public static bool Issue(string sysid, out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                da.OpenConn();

                string strSql = "delete from c2c_db_inc.t_bulletin_info  where FSysID="
                    + sysid + " and Fstate=2";
                da.ExecSql(strSql);

                strSql = "insert c2c_db_inc.t_bulletin_info(FSysID,FOrder,FIsNew,FTitle,FUrl,FissueTime,FLastTime,FuserId,FModify_time,Fstate,Flist_state,Fip,FStandBy1)"
                    + " select FSysID,FOrder,FIsNew,FTitle,FUrl,FissueTime,FLastTime,FuserId,now(),2,1,Fip,FStandBy1"
                    + " from c2c_db_inc.t_bulletin_info where FSysID=" + sysid + " and Fstate=1 and Flist_state=1";

                da.ExecSql(strSql);

                return true;
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public int FID = 0;
        public int FSysID = 1;
        public int FIsNew = 1;
        public string FTitle = "";
        public string FUrl = "";
        public string FissueTime = "";
        public string FLastTime = "";
        public string FuserId = "";

        public int FIsRed = 1;

        private bool ValidParam(out string msg)
        {
            msg = "";
            //У�鿪ʼ
            //			if(FSysID <1 || FSysID > 4)
            //			{
            //				msg = "ϵͳID����ȷ";
            //				return false;
            //			}

            if (FIsNew < 1 || FIsNew > 2)
            {
                msg = "�Ƿ�Ϊ�����ֶ�����";
                return false;
            }

            if (FIsRed < 0 || FIsRed > 1)
            {
                msg = "�Ƿ�Ϊ�����ֶ�����";
                return false;
            }

            if (FTitle == null || FTitle.Trim() == "")
            {
                msg = "���������ı��⡣";
                return false;
            }

            try
            {
                FissueTime = DateTime.Parse(FissueTime.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                if (FLastTime != null && FLastTime.Trim() != "")
                {
                    FLastTime = DateTime.Parse(FLastTime.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            catch
            {
                msg = "�������ȷ��ʱ��";
                return false;
            }

            if (FUrl == null || FUrl.Trim() == "")
            {
                msg = "���������";
                return false;
            }

            if (FuserId == null || FuserId.Trim() == "")
            {
                msg = "�����������";
                return false;
            }
            return true;
        }

        public bool Change(string userip, out string msg)
        {
            msg = "";
            if (!ValidParam(out msg))
            {
                return false;
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                da.OpenConn();
                string strSql = "select count(*) from c2c_db_inc.t_bulletin_info where FID=" + FID
                    + "  and Fstate=1 and Flist_state=1";

                if (da.GetOneResult(strSql) != "1")
                {
                    msg = "�Ҳ���ԭ�м�¼";
                    return false;
                }

                if (FLastTime == null || FLastTime.Trim() == "")
                {
                    strSql = "update c2c_db_inc.t_bulletin_info set FIsNew={0},FTitle='{1}',FUrl='{2}',FissueTime='{3}',"
                        + "FuserId='{4}',Fip='{5}',FModify_time=now(),FStandBy1={6} where FID=" + FID;

                    strSql = String.Format(strSql, FIsNew, FTitle, FUrl, FissueTime, FuserId, userip, FIsRed);
                }
                else
                {
                    strSql = "update c2c_db_inc.t_bulletin_info set FIsNew={0},FTitle='{1}',FUrl='{2}',FissueTime='{3}',FLastTime='{4}',"
                        + "FuserId='{5}',Fip='{6}',FModify_time=now(),FStandBy1={7} where FID=" + FID;

                    strSql = String.Format(strSql, FIsNew, FTitle, FUrl, FissueTime, FLastTime, FuserId, userip, FIsRed);
                }

                if (da.ExecSqlNum(strSql) == 1)
                    return true;
                else
                {
                    msg = "���¼�¼ʧ��";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public bool CreateNew(string userip, out string msg)
        {
            msg = "";
            if (!ValidParam(out msg))
            {
                return false;
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                da.OpenConn();
                string strSql = "select ifnull(max(Forder),0) from c2c_db_inc.t_bulletin_info where"
                    + "   Fstate=1 and Flist_state=1";

                int maxorder = Int32.Parse(da.GetOneResult(strSql)) + 1;

                if (FLastTime == null || FLastTime.Trim() == "")
                {
                    strSql = "Insert c2c_db_inc.t_bulletin_info(FSysID,FOrder,FIsNew,FTitle,FUrl,FissueTime"
                        + ",FuserId,FModify_time,Fstate,Flist_state,Fip,FStandBy1) "
                        + " values({0},{1},{2},'{3}','{4}','{5}','{6}',now(),1,1,'{7}',{8})";

                    strSql = String.Format(strSql, FSysID, maxorder, FIsNew, FTitle, FUrl, FissueTime, FuserId, userip, FIsRed);
                }
                else
                {
                    strSql = "Insert c2c_db_inc.t_bulletin_info(FSysID,FOrder,FIsNew,FTitle,FUrl,FissueTime,FLastTime"
                        + ",FuserId,FModify_time,Fstate,Flist_state,Fip,FStandBy1) "
                        + " values({0},{1},{2},'{3}','{4}','{5}','{6}','{7}',now(),1,1,'{8}',{9})";

                    strSql = String.Format(strSql, FSysID, maxorder, FIsNew, FTitle, FUrl, FissueTime, FLastTime, FuserId, userip, FIsRed);
                }

                if (da.ExecSqlNum(strSql) == 1)
                    return true;
                else
                {
                    msg = "������¼ʧ��";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public SysBulletinClass()
        { }

        public SysBulletinClass(int fid)
        {
            //��ȡ��Ϣ��
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                da.OpenConn();
                string strSql = "select * from c2c_db_inc.t_bulletin_info where FID=" + fid
                    + " and Fstate=1 and Flist_state=1";
                DataTable dt = da.GetTable(strSql);

                if (dt == null || dt.Rows.Count != 1)
                {
                    throw new LogicException("��ȡ��������");
                }

                FID = fid;
                FSysID = Int32.Parse(dt.Rows[0]["FSysID"].ToString());
                FIsNew = Int32.Parse(dt.Rows[0]["FIsNew"].ToString());
                FIsRed = Int32.Parse(dt.Rows[0]["FStandBy1"].ToString());

                object obj = dt.Rows[0]["FTitle"];
                if (obj != null) FTitle = obj.ToString().Trim();
                obj = dt.Rows[0]["FUrl"];
                if (obj != null) FUrl = obj.ToString().Trim();
                obj = dt.Rows[0]["FIssueTime"];
                if (obj != null) FissueTime = obj.ToString().Trim();
                obj = dt.Rows[0]["FLastTime"];
                if (obj != null) FLastTime = obj.ToString().Trim();
                obj = dt.Rows[0]["FUserID"];
                if (obj != null) FuserId = obj.ToString().Trim();
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

    }
    #endregion


    #region ���п��Ų�ѯ��

    public class BankDataClass : Query_BaseForNET
    {
        public BankDataClass(string fpay_acc, string Date)
        {
            fstrSql = "select fbank_order,fpay_acc,Fbank_date,Famt from c2c_zwdb_" + Date + ".t_bankdata_list where fpay_acc='" + fpay_acc + "' order by date_format(Fbank_date,'%Y%m%d') ";
            fstrSql_count = "select count(1) from c2c_zwdb_" + Date + ".t_bankdata_list where fpay_acc='" + fpay_acc + "'";
        }
    }

    #endregion



    #region �ʴ�����ѯ��

    public class RemitQueryClass : Query_BaseForNET
    {
        public RemitQueryClass(string batchid, string tranType, string dataType, string remitType, string tranState, string spid, string remitRec, string listID)
        {
            string strWhere = " where 1=1 ";

            if (batchid != null && batchid != "")
            {
                strWhere += " and Fbatchid='" + batchid + "'";
            }

            if (spid != null && spid != "")
            {
                strWhere += " and Fspid='" + spid + "'";
            }

            if (tranType != "99")
            {
                strWhere += " and Ftran_type=" + tranType;
            }

            if (dataType != "99")
            {
                strWhere += " and Fdata_type=" + dataType;
            }

            if (remitType != "99")
            {
                strWhere += " and Fremit_type=" + remitType;
            }

            if (tranState != "99")
            {
                strWhere += " and Ftran_state='" + tranState + "'";
            }

            if (listID != null && listID.Trim() != "")
            {
                strWhere += " and Flistid='" + listID.Trim() + "' ";
            }

            if (remitRec != null && remitRec.Trim() != "")
            {
                strWhere += " and Fremit_rec='" + remitRec.Trim() + "' ";
            }

            fstrSql = "select * from c2c_zwdb.t_remit_order " + strWhere;
            fstrSql_count = "select count(*) from c2c_zwdb.t_remit_order " + strWhere;
        }
    }



    public class QueryRemitStateInfo : Query_BaseForNET
    {
        public QueryRemitStateInfo(string Ford_date, string Ford_ssn)
        {
            string strWhere = " where Ford_date='" + Ford_date + "' and Ford_ssn='" + Ford_ssn + "'";

            fstrSql = "select * from c2c_db_remit.t_remit_list " + strWhere;
        }
    }

    #endregion

    public class ZWDicClass
    {
        //��ȡ����������
        public static string GetZWDicValue(string dicKey, string strConn)
			{
				MySqlAccess dazw = new MySqlAccess(strConn);
				try
				{
					if(dicKey==null||dicKey=="")
						return "";
			
					string strSql="select FDicValue from c2c_zwdb.t_zwdic_info where FDicKey='"+dicKey+"'";
					dazw.OpenConn();
				
					string fvalue= dazw.GetOneResult(strSql);
					if(fvalue==null||fvalue=="")
					{
						throw new LogicException("δ��ѯ��"+dicKey+"��Ӧ�����õ�ֵ��");
					}
					else
					{
						return fvalue;
					}


				}
				catch(Exception ex)
				{
					log4net.ILog log = log4net.LogManager.GetLogger("��ȡ����������ʧ��");
					if(log.IsErrorEnabled) log.Error(ex.Message);
					return "";
				}
				finally
				{
					dazw.Dispose();
				
				}
			}

    }

}