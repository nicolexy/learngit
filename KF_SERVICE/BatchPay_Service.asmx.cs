using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.C2C.Finance.BankLib;


namespace TENCENT.OSS.CFT.KF.KF_Service
{
    /// <summary>
    /// BatchPay_Service ��ժҪ˵����
    /// </summary>
    [WebService(Namespace = "http://Tencent.com/OSS/C2C/Finance/BatchPay_Service")]
    public class BatchPay_Service : System.Web.Services.WebService
    {
        public BatchPay_Service()
        {
            //CODEGEN: �õ����� ASP.NET Web ����������������
            InitializeComponent();
        }



        #region �����������ɵĴ���

        //Web ����������������
        private IContainer components = null;

        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {
        }

        /// <summary>
        /// ������������ʹ�õ���Դ��
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        // WEB ����ʾ��
        // HelloWorld() ʾ�����񷵻��ַ��� Hello World
        // ��Ҫ���ɣ���ȡ��ע�������У�Ȼ�󱣴沢������Ŀ
        // ��Ҫ���Դ� Web �����밴 F5 ��

        //		[WebMethod]
        //		public string HelloWorld()
        //		{
        //			return "Hello World";
        //		}

        /// <summary>
        /// ��ȡָ�����κŵ��쳣�˵���¼
        /// </summary>
        /// <param name="strBatchID">���κ�</param>
        /// <returns>���ؽ��</returns>
        [WebMethod]
        public DataSet RefundOther_ShowData(string strBatchID)
        {
            DataTable dt;
            MySqlAccess dazw = new MySqlAccess(PublicRes.GetConnString("ZW"));
            try
            {
                //�������е���ת�ñ��20060615
                string strSql = "select * from c2c_zwdb.t_batchrefundother_rec where FbatchID='" + strBatchID + "'";
                dazw.OpenConn();
                dt = dazw.GetTable(strSql);
                TranBatchreFundOtherRec(dt);
            }
            finally
            {
                dazw.Dispose();
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            return ds;
        }

        [WebMethod]
        public Param[] GetRefundState_Other(string batchid)
        {
            return BankRefundIO.GetRefundState_Other(batchid, PublicRes.GetConnString("ZW"));
        }

        /// <summary>
        /// ȡ��ָ��ʱ���������е��˵��쳣�������
        /// </summary>
        /// <param name="WeekIndex">ָ��ʱ��</param>
        /// <returns>�������ݼ�</returns>
        [WebMethod]
        public DataSet RefundOther_InitGrid_R(string beginDate, string endDate, string banktype, int status, string proposer, string refundPath)
        {
            string strBeginDate = DateTime.Parse(beginDate).ToString("yyyyMMdd");
            string strEndDate = DateTime.Parse(endDate).ToString("yyyyMMdd");

            //�������е���ת�ñ��20060615
            string strSql = "select FBatchID,'0' FUrl,FBatchDay,FBankType,'' FBankTypeName,FPayCount,(FPaySum / 100) FPaySum1 ,FStatus,'' FMsg,FProposer ,'' Furl2,FApproveDate"
                + " from c2c_zwdb.t_batchrefundother_rec  ";
            string strSqlWhere = " where FbatchDay between '" + strBeginDate + "' and '" + strEndDate + "' ";
            if (banktype != "0000")
            {
                strSqlWhere += " and FbankType='" + banktype + "'";
            }
            if (status != 9999)
            {
                strSqlWhere += " and Fstatus='" + status + "'";
            }
            if (proposer != null && proposer != "")
            {
                strSqlWhere += " and Fproposer='" + commRes.replaceSqlStr(proposer) + "'";
            }
            if (refundPath != null && refundPath != "9999" && refundPath != "")
            {
                strSqlWhere += " and FBatchID like '__________" + commRes.replaceSqlStr(refundPath) + "_'";
            }

            strSql = strSql + strSqlWhere + "   order by Fbanktype ";

            DataSet ds = new DataSet();
            DataTable dt;
            MySqlAccess dazw = new MySqlAccess(PublicRes.GetConnString("ZW"));
            try
            {
                dazw.OpenConn();

                //�����˵���������Ҫ�����˿��������������״̬
                string strSql2 = "select FBatchID from c2c_zwdb.t_batchrefundother_rec where FBatchID like '__________1E_'  and FStatus in(4,96) order by FBatchDay desc";
                DataTable dt_batchid = dazw.GetTable(strSql2);
                if (dt_batchid != null && dt_batchid.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt_batchid.Rows)
                    {
                        string handleBatchid = dr["FBatchID"].ToString();
                        //string strTmp="select count(*) from c2c_zwdb.t_refund_total where FoldID in(select FoldID from c2c_zwdb.t_refund_other where FHandleBatchId ='"+handleBatchid+"') and Fstate =1";
                        string strTmp = "select count(*) from c2c_zwdb.t_refund_total t right join c2c_zwdb.t_refund_other o on t.foldid=o.foldid  where  o.fhandlebatchid='" + handleBatchid + "' and t.Fstate =1";
                        int stata1 = Convert.ToInt32(dazw.GetOneResult(strTmp));
                        if (stata1 > 0)
                        {
                            string upSql = "update c2c_zwdb.t_batchrefundother_rec set FStatus=96 where fbatchid='" + handleBatchid + "' and FStatus=4";
                            dazw.ExecSqlNum(upSql);
                            continue;
                        }
                        strTmp = "select count(*) from c2c_zwdb.t_refund_other where FHandleBatchId ='" + handleBatchid + "'";
                        int otherCount = Convert.ToInt32(dazw.GetOneResult(strTmp));
                        //strTmp="select count(*)  from c2c_zwdb.t_refund_total where FoldID in(select FoldID from c2c_zwdb.t_refund_other where FHandleBatchId ='"+handleBatchid+"') and Fstate in(2,3)";
                        strTmp = "select count(*) from c2c_zwdb.t_refund_total t right join c2c_zwdb.t_refund_other o on t.foldid=o.foldid  where  o.fhandlebatchid='" + handleBatchid + "' and t.Fstate in(2,3)";
                        int stata2 = Convert.ToInt32(dazw.GetOneResult(strTmp));
                        if (stata2 == otherCount)
                        {
                            //�������α�״̬
                            string upSql = "update c2c_zwdb.t_batchrefundother_rec set FStatus=99 where fbatchid='" + handleBatchid + "' and FStatus in(4,96)";
                            int bcount = Convert.ToInt32(dazw.ExecSqlNum(upSql));
                            //�����˿��쳣����״̬
                            //							if(bcount==1)
                            //							{
                            string upSql2 = "update c2c_zwdb.t_refund_other o,c2c_zwdb.t_refund_total t set o.FHandleType=3, o.Fmodify_Time=now(),o.FRefundMemo=concat(o.FRefundMemo,'|��" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "��ȡ���ٴζ����˿�ɹ�" + "')  where o.Foldid=t.Foldid and o.FHandleBatchId ='" + handleBatchid + "' and t.Fstate=2 and o.FHandleType=2";
                            dazw.ExecSqlNum(upSql2);
                            //							}
                            continue;
                        }
                    }
                }

                dt = dazw.GetTable(strSql);
                TranBatchreFundOtherRec(dt);
            }
            finally
            {
                dazw.Dispose();
            }

            ds.Tables.Add(dt);
            return ds;
        }


        public void TranBatchreFundOtherRec(DataTable dt)
        {
            try
            {
                dt.Columns.Add("FStatusName", typeof(System.String));
                dt.Columns.Add("FRefundPath", typeof(System.String));
                foreach (DataRow dr in dt.Rows)
                {
                    string tmp = dr["FStatus"].ToString();
                    if (tmp == "0")
                    {
                        dr["FStatusName"] = "�����˵�����";
                    }
                    else if (tmp == "1")
                    {
                        dr["FStatusName"] = "�����ٴ������˿�";
                        dr["FMsg"] = "�ȴ�����ͨ��";
                    }
                    else if (tmp == "2")
                    {
                        dr["FStatusName"] = "���븶���˿�";
                        dr["FMsg"] = "�ȴ�����ͨ��";
                    }
                    else if (tmp == "3")
                    {
                        dr["FStatusName"] = "�����˹���Ȩ�˿�";
                        dr["FMsg"] = "�ȴ�����ͨ��";
                    }
                    else if (tmp == "4")
                    {
                        dr["FStatusName"] = "����ͨ��";
                        dr["FMsg"] = "�ȴ�ͳһ�����˿�";
                    }
                    else if (tmp == "5")
                    {
                        dr["FStatusName"] = "����ͨ��";

                    }
                    else if (tmp == "6")
                    {
                        dr["FStatusName"] = "����ͨ��";
                        dr["FMsg"] = "��������Ȩ�鿪ʼ�˿�";
                    }
                    else if (tmp == "7")
                    {
                        dr["FStatusName"] = "�ٴ������˿���";
                        dr["FMsg"] = "�ȴ��˿����ص�";
                    }
                    else if (tmp == "8")
                    {
                        dr["FStatusName"] = "�����˿��˿���";
                    }
                    else if (tmp == "9")
                    {
                        dr["FStatusName"] = "�˹���Ȩ�˿���";
                        dr["FMsg"] = "�ȴ���Ȩ�˿���";
                    }
                    else if (tmp == "10")
                    {
                        dr["FStatusName"] = "�ٴ������˿����ص����";
                        dr["FMsg"] = "���ע�˿���";
                    }
                    else if (tmp == "11")
                    {
                        dr["FStatusName"] = "��������˹���Ȩ����";
                        dr["FMsg"] = "�����ֹ���Ч�˿���";
                    }
                    else if (tmp == "12")
                    {
                        dr["FStatusName"] = "�˿�ֱ�ӵ���Ϊ�ɹ�������";
                        dr["FMsg"] = "�ȴ�����ͨ��";
                    }
                    else if (tmp == "13")
                    {
                        dr["FStatusName"] = "����ת�����������";
                        dr["FMsg"] = "�ȴ�����ͨ��";
                    }
                    else if (tmp == "96")
                    {
                        dr["FStatusName"] = "�ٴ������˿���";
                        dr["FMsg"] = "";
                    }
                    else if (tmp == "97")
                    {
                        dr["FStatusName"] = "��Ȩ���������˿���";
                        dr["FMsg"] = "";
                    }
                    else if (tmp == "98")
                    {
                        dr["FStatusName"] = "�˹���Ȩ��������";
                        dr["FMsg"] = "";
                    }

                    else if (tmp == "99")
                    {
                        dr["FStatusName"] = "���δ������";

                    }
                    else
                    {
                        dr["FStatusName"] = "δ֪״̬" + tmp;
                    }

                    tmp = dr["FBatchDay"].ToString();
                    dr["FBatchDay"] = tmp.Substring(0, 4) + "��" + tmp.Substring(4, 2) + "��" + tmp.Substring(6, 2) + "��";

                    tmp = dr["FBatchID"].ToString();
                    if (tmp.Length == 13 && tmp.IndexOf("E") > -1)
                    {
                        tmp = tmp.Substring(10, 1);
                        if (tmp == "1")
                        {
                            dr["FRefundPath"] = "�����˵�";
                        }
                        else if (tmp == "3")
                        {
                            dr["FRefundPath"] = "�˹���Ȩ�˿�";
                        }
                        else if (tmp == "6")
                        {
                            dr["FRefundPath"] = "�����˿�";
                        }
                        else if (tmp == "9")
                        {
                            dr["FRefundPath"] = "ֱ�ӵ���Ϊ�˿�ɹ�";
                        }
                        else if (tmp == "7")
                        {
                            dr["FRefundPath"] = "ת�����";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        [WebMethod]
        public string GetZWDicValueByKey(string key)
        {
            return ZWDicClass.GetZWDicValue(key, PublicRes.GetConnString("ZW"));
        }
    }
}
