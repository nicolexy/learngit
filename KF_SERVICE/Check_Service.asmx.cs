using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.C2C.Finance.BankLib;

namespace TENCENT.OSS.CFT.KF.KF_Service
{
    /// <summary>
    /// Check_Service ��ժҪ˵����
    /// </summary>
    [WebService(Namespace = "http://Tencent.com/OSS/C2C/Finance/Check_Service")]
    public class Check_Service : System.Web.Services.WebService
    {
        /// <summary>
        /// SOAPͷ
        /// </summary>
        public Finance_Header myHeader;

        public Check_Service()
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


        [WebMethodAttribute]
        public bool SetSendLog(string operID)
        {
            return SendLogger.InitInstance(operID);
        }



        //		[WebMethodAttribute]
        //		public bool SendLog(bool stopCatchLog)
        //		{
        //			return SendLogger.GetInstance().SendLog(stopCatchLog);
        //		}




        /// <summary>
        /// ��ʼһ������
        /// </summary>
        /// <param name="strMainID">�����ؼ���ID</param>
        /// <param name="strCheckType">��������</param>
        /// <param name="strMemo">��ϸ����</param>
        /// <param name="strLevelValue">��������������ж�ֵ</param>
        /// <param name="myParams">��Ҫ�Ĳ����б�</param>
        [WebMethod(Description = "����һ������")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public void StartCheck(string strMainID, string strCheckType, string strMemo, string strLevelValue, Param[] myParams)
        {
            if (PublicRes.IgnoreLimitCheck)
            {
                using (MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht")))
                {
                    da.OpenConn();
                    string strSql = "select Fid,Fnewstate from c2c_fmdb.t_check_main where Fobjid='" + strMainID + "' and Fchecktype='" + strCheckType + "' order by Fid desc";
                    DataTable dt = da.GetTable(strSql);

                    if (dt == null || dt.Rows.Count != 1)
                        return;

                    if (dt.Rows[0]["Fnewstate"] != null && dt.Rows[0]["Fnewstate"].ToString().Trim() == "3")
                        ExecuteCheck(dt.Rows[0]["Fid"].ToString().Trim());
                    else
                        return;
                }
            }
            string Msg = null;
            RightAndLog rl = new RightAndLog();
            try
            {
                if (myHeader == null)
                {
                    throw new LogicException("����ȷ�ĵ��÷�����");
                }
                rl.actionType = "����һ������";
                rl.ID = strMainID;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "StartCheck";
                rl.SzKey = myHeader.SzKey;
                rl.type = "��������";
                rl.RightString = myHeader.RightString;
                rl.UserID = myHeader.UserName.Trim().ToLower();
                rl.UserIP = myHeader.UserIP;

                //�ȼ��Ȩ�� Ray 2005.10.20 ���е�����֮ǰ��Ҫ���ж�Ȩ�� 
                //��ִ��������ǰ��У��
                //priorCheck pc = new priorCheck();
                IpriorCheck pc = priorCheck.GetHandler(strCheckType);

                //if (pc.doPriorCheck(strCheckType) != null) //���Ϊ�� Ĭ��ͨ��Ȩ����֤
                if (pc != null) //���Ϊ��,Ҳ����������������û�д���,��ͨ��ǰ��У��. 
                {
                    //bool exeSign = pc.doPriorCheck(strCheckType).checkRight(myParams,out Msg);
                    bool exeSign = pc.checkRight(myParams, out Msg);
                    if (exeSign == false)
                    {
                        //throw new LogicException("����Ȩ�޼��ʧ�ܣ�" + Msg);
                        throw new LogicException("ִ�������ļ��ʧ�ܣ�" + Msg);
                    }
                }

                Check_Class.CreateCheck(rl.UserID, strMainID, strCheckType, strMemo, strLevelValue, myParams, myHeader);

                rl.detail = "�û�" + rl.UserID + "��" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "ʱ���IDΪ" + strMainID
                    + "�ļ�¼������" + strCheckType + "��������ִ�н���ɹ���";
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.detail = "�û�" + rl.UserID + "��" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "ʱ���IDΪ" + strMainID
                    + "�ļ�¼������" + strCheckType + "��������ִ�н��ʧ�ܣ�" + err.Message;
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.detail = "�û�" + rl.UserID + "��" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "ʱ���IDΪ" + strMainID
                    + "�ļ�¼������" + strCheckType + "��������ִ�н��ʧ�ܣ�" + err.Message;
                throw new LogicException("Service����ʧ�ܣ�");
            }
            finally
            {
                rl.WriteLog();
            }
        }



        /// <summary>
        /// ִ��������ɺ�Ķ���
        /// </summary>
        /// <param name="strCheckID">����ID</param>
        /// <returns>�Ƿ�ɹ�</returns>
        [WebMethod(Description = "ִ��������Ķ���")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool ExecuteCheck(string strCheckID)
        {
            RightAndLog rl = new RightAndLog();
            try
            {

                if (myHeader == null)
                {
                    throw new LogicException("����ȷ�ĵ��÷�����");
                }
                rl.actionType = "ִ��������Ķ���";
                rl.ID = strCheckID;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "ExecuteCheck";
                rl.SzKey = myHeader.SzKey;
                rl.RightString = myHeader.RightString;
                rl.type = "ִ������";
                rl.UserID = myHeader.UserName.Trim().ToLower();
                rl.UserIP = myHeader.UserIP;
                //				if(!rl.CheckRight())
                //				{
                //					throw new LogicException("�û���Ȩִ�д˲�����");
                //				}

                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
                try
                {
                    da.OpenConn();
                    return FinishCheck.ExecuteCheck(strCheckID, myHeader, da);
                }
                finally
                {
                    da.Dispose();
                }
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                throw new LogicException("ʧ�ܣ�" + err.Message.ToString().Replace("'", "��"));
            }
            finally
            {
                rl.WriteLog();
            }
        }

        /// <summary>
        /// ��ȡ������־
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "��ȡ������־")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataTable GetCheckLog(string checkid,int iStartIndex, int iRecordCount)
        {
            DataTable dt = null;
             string strSql = "SELECT fid, fcheckid,fcheckTime,fcheckuser,fcheckmemo,"
                    //furion 20050105 ��������SQL
                    //+"(case  fcheckresult when 0 then 'ͬ��' when 1 then '<font color = red>��ͬ��</font>' end) as fcheckresult " 
                    + "fcheckresult as ifcheckresult "
                    + "FROM c2c_fmdb.t_check_list "
                    + " where FcheckID=" + checkid.Trim();

                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
                try
                {
                    da.OpenConn();
                    dt = da.GetTableByRange(strSql, iStartIndex, iRecordCount);

                    //furion 20050105 ��������SQL
                    if (dt != null)
                        dt.Columns.Add("fcheckresult", typeof(System.String));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr.BeginEdit();
                            string tmp = dr["ifcheckresult"].ToString();
                            if (tmp == "0")
                            {
                                dr["fcheckresult"] = "ͬ��";
                            }
                            else if (tmp == "1")
                            {
                                dr["fcheckresult"] = "<font color = red>��ͬ��</font>";
                            }

                            dr.EndEdit();
                        }
                    }
                    //furion end
                    return dt;
                }
                    catch (Exception e)
                {
                    throw new Exception("��ȡ������־ʧ�ܣ� ����ϵ����Ա��");
                    return dt;
                }
               
                finally
                {
                    da.Dispose();
                }
           
            
        }

        /// <summary>
        /// δ����������ѯ
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "δ����������ѯ")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public int GetStartCheckCount(string strCheckType, string strUserID)
        {
            string strSql = "select count(1) "
                + "from c2c_fmdb.t_check_main A where  A.FStartUser='"
                //+ strUserID + "' and A.FState<>'finish' and A.Fstate<>'finished'";
                + strUserID + "' and A.FNewState<2";

            if (strCheckType != "0")
            {
                strSql += " and A.FCheckType='" + strCheckType + "'";
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
            try
            {
                da.OpenConn();
                string Result = da.GetOneResult(strSql);
                return Int32.Parse(Result);
            }
            finally
            {
                da.Dispose();
            }
        }

        /// <summary>
        /// ������������ѯ
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "������������ѯ")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public int GetFinishCheckCount(string strCheckType, string strUserID)
        {
            string strSql = "select count(1) "
                + "from c2c_fmdb.t_check_main A where  A.FStartUser='"
                //+ strUserID + "' and (A.FState='finish' or A.FState='finished' ) "; //��������ͨ��ʱ������
                //+ strUserID + "' and (A.FNewState>=2 and A.FNewState<=3 ) "; //��������ͨ��ʱ������
                + strUserID + "' and (A.FNewState>=2 and A.FNewState<=4 ) "; //��������ͨ��ʱ������

            if (strCheckType != "0")
            {
                strSql += " and A.FCheckType='" + strCheckType + "'";
            }

            strSql += " Order by FEndTime DESC";

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
            try
            {
                da.OpenConn();
                string Result = da.GetOneResult(strSql);
                return Int32.Parse(Result);
            }
            finally
            {
                da.Dispose();
            }
        }

        /// <summary>
        /// �ҷ�������������ݰ󶨴���
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "���ݰ󶨴���")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataTable GetStartCheckData(string strCheckType, string strUserID, int iStartIndex, int iRecordCount, string strFid)
        {
            string strFidSql;

            if (strFid == "")
            {
                strFidSql = "";
            }
            else
            {
                strFidSql = " and A.fid=" + strFid + " ";
            }

            string strSql = "select A.FID,A.FObjID,"
                //+ "(select C.Fvalue from c2c_fmdb.t_check_param C where A.FID = C.FCheckID and C.Fkey = 'zwTime') as zwTime, "  
                //+ "(select C.Fvalue from c2c_fmdb.t_check_param C where A.FID = C.FCheckID and C.Fkey = 'returnUrl') as returnUrl, "
                //+ "(select C.Fvalue from c2c_fmdb.t_check_param C where A.FID = C.FCheckID and C.Fkey = 'czType') as czType, "  
                //+ "(Select C.Fvalue from c2c_fmdb.t_check_param C where C.FcheckID = A.FID  and C.Fkey = 'batLstID') as  batLstID, "
                + "T.FTypeName"
                + "  ,A.FStartUser,A.FCheckMemo ,A.FCheckMoney, A.FCheckType,A.FCheckCount,"
                + "A.FNewstate as iFstate,A.FCheckResult as iFCheckResult,A.FCheckLevel as iFCheckLevel,A.FCurrLevel+1 as iFCurrLevel,"
                //+ ",(case A.fstate when 'start' then '��ʼ' when 'check' then '������' when 'finish' then '<font color = red>�������</font>' when 'finished' then '��ִ��' end) FState "
                //+ ",(select U.FLevelName from c2c_fmdb.t_check_user U where U.FCheckType=A.FCheckType and U.FLevel=A.FCheckLevel) FCheckLevel,"
                //+ "(case A.FCurrLevel when A.FCheckLevel then '�����ѽ���' else (select CONCAT(U.FLevelName,':',U.FUserID) from c2c_fmdb.t_check_user U where U.FCheckType=A.FCheckType and U.FLevel= A.FCurrLevel+1) end) FCurrLevel,"
                //+ " ,(case A.FCheckResult when 0 then 'ͬ��' when 1 then '<font color = red>��ͬ��</font>' when -1 then '��������'  end ) FCheckResult, "
                + "A.FStartTime,A.FEndTime "
                + "from c2c_fmdb.t_check_main A,c2c_fmdb.t_check_type T  where T.FTypeID=A.FCheckType and A.FStartUser='"
                //+ strUserID + "' and A.FState<>'finish' and A.FState<>'finished' "
                + strUserID + "' and A.FNewState<2 "
                + strFidSql;

            if (strCheckType != "0")
            {
                strSql += " and A.FCheckType='" + strCheckType + "'";
            }

            strSql += "  order by A.FID desc ";

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
            try
            {
                da.OpenConn();
                DataTable dt = da.GetTableByRange(strSql, iStartIndex, iRecordCount);
                if (dt != null)
                {
                    dt.Columns.Add("zwTime", typeof(System.String));
                    dt.Columns.Add("returnUrl", typeof(System.String));
                    dt.Columns.Add("czType", typeof(System.String));
                    dt.Columns.Add("batLstID", typeof(System.String));
                    //dt.Columns.Add("FTypeName",typeof(System.String));
                    dt.Columns.Add("banktype", typeof(System.String));
                    dt.Columns.Add("FState", typeof(System.String));
                    dt.Columns.Add("FCheckLevel", typeof(System.String));
                    dt.Columns.Add("FCurrLevel", typeof(System.String));
                    dt.Columns.Add("FCheckResult", typeof(System.String));
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr.BeginEdit();
                        string fid = dr["FID"].ToString().Trim();
                        if (fid != "" && dt.Rows.Count == 1)
                        {
                            strSql = "select * from c2c_fmdb.t_check_param where FCheckID=" + fid;
                            DataTable dt_param = da.GetTable(strSql);

                            if (dt_param != null && dt_param.Rows.Count > 0)
                            {
                                foreach (DataRow dr_param in dt_param.Rows)
                                {
                                    //+ "(select C.Fvalue from c2c_fmdb.t_check_param C where A.FID = C.FCheckID and C.Fkey = 'zwTime') as zwTime, "  
                                    //+ "(select C.Fvalue from c2c_fmdb.t_check_param C where A.FID = C.FCheckID and C.Fkey = 'returnUrl') as returnUrl, "
                                    //+ "(select C.Fvalue from c2c_fmdb.t_check_param C where A.FID = C.FCheckID and C.Fkey = 'czType') as czType, "  
                                    //+ "(Select C.Fvalue from c2c_fmdb.t_check_param C where C.FcheckID = A.FID  and C.Fkey = 'batLstID') as  batLstID, "
                                    string fkey = dr_param["Fkey"].ToString().Trim().ToLower();
                                    if (fkey == "zwtime")
                                        dr["zwTime"] = dr_param["Fvalue"].ToString();
                                    else if (fkey == "returnurl")
                                        dr["returnUrl"] = dr_param["Fvalue"].ToString();
                                    else if (fkey == "cztype")
                                        dr["czType"] = dr_param["Fvalue"].ToString();
                                    else if (fkey == "batlstid")
                                        dr["batLstID"] = dr_param["Fvalue"].ToString();
                                    else if (fkey == "banktype")
                                        dr["banktype"] = dr_param["Fvalue"].ToString();
                                }
                            }
                        }

                        string istate = dr["iFState"].ToString().Trim();
                        //(case A.fstate when 'start' then '��ʼ' when 'check' then '������' when 'finish' then '<font color = red>�������</font>' when 'finished' then '��ִ��' end) FState "
                        if (istate == "0")
                            dr["FState"] = "��ʼ";
                        else if (istate == "1")
                            dr["FState"] = "������";
                        else if (istate == "2")
                            dr["FState"] = "<font color = red>�������</font>";
                        else if (istate == "3")
                            dr["FState"] = "��ִ��";
                        //(case A.FCheckResult when 0 then 'ͬ��' when 1 then '<font color = red>��ͬ��</font>' when -1 then '��������'  end ) FCheckResult, "
                        string iFCheckResult = dr["iFCheckResult"].ToString().Trim();
                        if (iFCheckResult == "0")
                            dr["FCheckResult"] = "ͬ��";
                        else if (iFCheckResult == "1")
                            dr["FCheckResult"] = "<font color = red>��ͬ��</font>";
                        else if (iFCheckResult == "-1")
                            dr["FCheckResult"] = "��������";

                        //+ ",(select U.FLevelName from c2c_fmdb.t_check_user U where U.FCheckType=A.FCheckType and U.FLevel=A.FCheckLevel) FCheckLevel,"
                        //+ "(case A.FCurrLevel when A.FCheckLevel then '�����ѽ���' else (select CONCAT(U.FLevelName,':',U.FUserID) from c2c_fmdb.t_check_user U where U.FCheckType=A.FCheckType and U.FLevel= A.FCurrLevel+1) end) FCurrLevel,"
                        //as iFCheckLevel,A.FCurrLevel as iFCurrLevel
                        string fchecktype = dr["FCheckType"].ToString().Trim();
                        string ifchecklevel = dr["iFCheckLevel"].ToString().Trim();
                        string ifcurrLevel = dr["iFCurrLevel"].ToString().Trim();
                        strSql = "select FLevelName,FUserID from c2c_fmdb.t_check_user where FCheckType='"
                            + fchecktype + "' and FLevel in (" + ifchecklevel + "," + ifcurrLevel + ") order by FLevel";

                        DataTable dt_checktype = da.GetTable(strSql);
                        if (dt_checktype != null && dt_checktype.Rows.Count > 0)
                        {
                            if (ifchecklevel == "0")
                            {
                                dr["FCurrLevel"] = "";
                                dr["FCheckLevel"] = "";
                            }
                            else if (Int32.Parse(ifchecklevel) == Int32.Parse(ifcurrLevel) - 1)
                            {
                                dr["FCurrLevel"] = "�����ѽ���";
                                dr["FCheckLevel"] = dt_checktype.Rows[0]["FLevelName"].ToString().Trim();
                            }
                            else
                            {
                                if (dt_checktype.Rows.Count == 2)
                                {
                                    dr["FCurrLevel"] = dt_checktype.Rows[0]["FLevelName"].ToString().Trim()
                                        + "��" + dt_checktype.Rows[0]["FUserID"].ToString().Trim();
                                    dr["FCheckLevel"] = dt_checktype.Rows[1]["FLevelName"].ToString().Trim();
                                }
                                else
                                {
                                    dr["FCurrLevel"] = dt_checktype.Rows[0]["FLevelName"].ToString().Trim()
                                        + "��" + dt_checktype.Rows[0]["FUserID"].ToString().Trim();
                                    dr["FCheckLevel"] = dt_checktype.Rows[0]["FLevelName"].ToString().Trim();
                                }
                            }
                        }

                        dr.EndEdit();
                    }
                }

                return dt;
            }
            finally
            {
                da.Dispose();
            }
        }

        /// <summary>
        /// �����������ݰ󶨴���
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "���������ݰ󶨴���")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataTable GetFinishCheckData(string strCheckType, string strUserID, int iStartIndex, int iRecordCount, string strFid) //fid�ǵõ����������Ψһ��ʶ��
        {

            string strFidSql;

            if (strFid == "")
            {
                strFidSql = "";
            }
            else
            {
                strFidSql = " and A.fid=" + strFid + " ";
            }

            string strSql = "select A.FID,A.FObjID,"

                //+ "(select C.Fvalue from c2c_fmdb.t_check_param C where A.FID = C.FCheckID and C.Fkey = 'zwTime') as zwTime, "  
                //+ "(select C.Fvalue from c2c_fmdb.t_check_param C where A.FID = C.FCheckID and C.Fkey = 'returnUrl') as returnUrl, "  
                //+ "(select C.Fvalue from c2c_fmdb.t_check_param C where A.FID = C.FCheckID and C.Fkey = 'czType') as czType, "  
                //+ "(Select C.Fvalue from c2c_fmdb.t_check_param C where C.FcheckID = A.FID  and C.Fkey = 'batLstID') as  batLstID, "
                + "T.FTypeName "
                + "  ,A.FStartUser,A.FCheckMemo ,A.FCheckMoney, A.FCheckType,A.FCheckCount,"
                + "A.FNewstate as iFstate,A.FCheckResult as iFCheckResult,A.FCheckLevel as iFCheckLevel,A.FCurrLevel+1 as iFCurrLevel,"
                //+ ",(case A.fstate when 'start' then '��ʼ' when 'check' then '������' when 'finish' then '<font color = red>�������</font>' when 'finished' then '��ִ��'end) FState "
                //+ ",(select U.FLevelName from c2c_fmdb.t_check_user U where U.FCheckType=A.FCheckType and U.FLevel=A.FCheckLevel) FCheckLevel,"
                //+ "(case A.FCurrLevel when A.FCheckLevel then '�����ѽ���' else (select CONCAT(U.FLevelName,':',U.FUserID) from c2c_fmdb.t_check_user U where U.FCheckType=A.FCheckType and U.FLevel= A.FCurrLevel+1) end) FCurrLevel,"
                //+ " A.FCheckCount,(case A.FCheckResult when 0 then 'ͬ��' when 1 then '<font color = red>��ͬ��</font>' when -1 then '��������' end ) FCheckResult, "
                + "A.FStartTime,A.FEndTime "
                + "from c2c_fmdb.t_check_main A,c2c_fmdb.t_check_type T where T.FTypeID=A.FCheckType and A.FStartUser='"
                //+ strUserID + "' and (A.FState='finish' or A.FState='finished')" 
                //+ strUserID + "' and (A.FNewState>=2 and A.FNewState<=3)" 
                + strUserID + "' and (A.FNewState>=2 and A.FNewState<=4)"
                + strFidSql;

            if (strCheckType != "0")
            {
                strSql += " and A.FCheckType='" + strCheckType + "'";
            }

            strSql += "  order by A.FID desc";

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
            try
            {
                da.OpenConn();
                DataTable dt = da.GetTableByRange(strSql, iStartIndex, iRecordCount);

                if (dt != null)
                {
                    dt.Columns.Add("zwTime", typeof(System.String));
                    dt.Columns.Add("returnUrl", typeof(System.String));
                    dt.Columns.Add("czType", typeof(System.String));
                    dt.Columns.Add("batLstID", typeof(System.String));
                    //dt.Columns.Add("FTypeName",typeof(System.String));
                    dt.Columns.Add("banktype", typeof(System.String));
                    dt.Columns.Add("FState", typeof(System.String));
                    dt.Columns.Add("FCheckLevel", typeof(System.String));
                    dt.Columns.Add("FCurrLevel", typeof(System.String));
                    dt.Columns.Add("FCheckResult", typeof(System.String));
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr.BeginEdit();
                        string fid = dr["FID"].ToString().Trim();
                        if (fid != "" && dt.Rows.Count == 1)
                        {
                            strSql = "select * from c2c_fmdb.t_check_param where FCheckID=" + fid;
                            DataTable dt_param = da.GetTable(strSql);

                            if (dt_param != null && dt_param.Rows.Count > 0)
                            {
                                foreach (DataRow dr_param in dt_param.Rows)
                                {
                                    //+ "(select C.Fvalue from c2c_fmdb.t_check_param C where A.FID = C.FCheckID and C.Fkey = 'zwTime') as zwTime, "  
                                    //+ "(select C.Fvalue from c2c_fmdb.t_check_param C where A.FID = C.FCheckID and C.Fkey = 'returnUrl') as returnUrl, "
                                    //+ "(select C.Fvalue from c2c_fmdb.t_check_param C where A.FID = C.FCheckID and C.Fkey = 'czType') as czType, "  
                                    //+ "(Select C.Fvalue from c2c_fmdb.t_check_param C where C.FcheckID = A.FID  and C.Fkey = 'batLstID') as  batLstID, "
                                    string fkey = dr_param["Fkey"].ToString().Trim().ToLower();
                                    if (fkey == "zwtime")
                                        dr["zwTime"] = dr_param["Fvalue"].ToString();
                                    else if (fkey == "returnurl")
                                        dr["returnUrl"] = dr_param["Fvalue"].ToString();
                                    else if (fkey == "cztype")
                                        dr["czType"] = dr_param["Fvalue"].ToString();
                                    else if (fkey == "batlstid")
                                        dr["batLstID"] = dr_param["Fvalue"].ToString();
                                    else if (fkey == "banktype")
                                        dr["banktype"] = dr_param["Fvalue"].ToString();
                                }
                            }
                        }

                        string istate = dr["iFState"].ToString().Trim();
                        //(case A.fstate when 'start' then '��ʼ' when 'check' then '������' when 'finish' then '<font color = red>�������</font>' when 'finished' then '��ִ��' end) FState "
                        if (istate == "0")
                            dr["FState"] = "��ʼ";
                        else if (istate == "1")
                            dr["FState"] = "������";
                        else if (istate == "2")
                            dr["FState"] = "<font color = red>�������</font>";
                        else if (istate == "3")
                            dr["FState"] = "��ִ��";
                        else if (istate == "4")
                            dr["FState"] = "�ѳ���";
                        else if (istate == "9")
                            dr["FState"] = "ִ����";
                        //(case A.FCheckResult when 0 then 'ͬ��' when 1 then '<font color = red>��ͬ��</font>' when -1 then '��������'  end ) FCheckResult, "
                        string iFCheckResult = dr["iFCheckResult"].ToString().Trim();
                        if (iFCheckResult == "0")
                            dr["FCheckResult"] = "ͬ��";
                        else if (iFCheckResult == "1")
                            dr["FCheckResult"] = "<font color = red>��ͬ��</font>";
                        else if (iFCheckResult == "-1")
                            dr["FCheckResult"] = "��������";

                        //+ ",(select U.FLevelName from c2c_fmdb.t_check_user U where U.FCheckType=A.FCheckType and U.FLevel=A.FCheckLevel) FCheckLevel,"
                        //+ "(case A.FCurrLevel when A.FCheckLevel then '�����ѽ���' else (select CONCAT(U.FLevelName,':',U.FUserID) from c2c_fmdb.t_check_user U where U.FCheckType=A.FCheckType and U.FLevel= A.FCurrLevel+1) end) FCurrLevel,"
                        //as iFCheckLevel,A.FCurrLevel as iFCurrLevel
                        string fchecktype = dr["FCheckType"].ToString().Trim();
                        string ifchecklevel = dr["iFCheckLevel"].ToString().Trim();
                        string ifcurrLevel = dr["iFCurrLevel"].ToString().Trim();
                        strSql = "select FLevelName,FUserID from c2c_fmdb.t_check_user where FCheckType='"
                            + fchecktype + "' and FLevel in (" + ifchecklevel + "," + ifcurrLevel + ") order by FLevel";

                        DataTable dt_checktype = da.GetTable(strSql);
                        if (dt_checktype != null && dt_checktype.Rows.Count > 0)
                        {
                            if (ifchecklevel == "0")
                            {
                                dr["FCurrLevel"] = "";
                                dr["FCheckLevel"] = "";
                            }
                            else if (Int32.Parse(ifchecklevel) == Int32.Parse(ifcurrLevel) - 1)
                            {
                                dr["FCurrLevel"] = "�����ѽ���";
                                dr["FCheckLevel"] = dt_checktype.Rows[0]["FLevelName"].ToString().Trim();
                            }
                            else
                            {
                                if (dt_checktype.Rows.Count == 2)
                                {
                                    dr["FCurrLevel"] = dt_checktype.Rows[0]["FLevelName"].ToString().Trim()
                                        + "��" + dt_checktype.Rows[0]["FUserID"].ToString().Trim();
                                    dr["FCheckLevel"] = dt_checktype.Rows[1]["FLevelName"].ToString().Trim();
                                }
                                else
                                {
                                    dr["FCurrLevel"] = dt_checktype.Rows[0]["FLevelName"].ToString().Trim()
                                        + "��" + dt_checktype.Rows[0]["FUserID"].ToString().Trim();
                                    dr["FCheckLevel"] = dt_checktype.Rows[0]["FLevelName"].ToString().Trim();
                                }
                            }
                        }

                        dr.EndEdit();
                    }
                }

                return dt;
            }
            finally
            {
                da.Dispose();
            }
        }


        /// <summary>
        /// ����ִ������
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "����ִ������")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public int GetToDoNum(string strCheckType, string strUserID)
        {
            string strSql = "select count(1) "
                + "from c2c_fmdb.t_check_main A where  A.FStartUser='"
                //+ strUserID + "' and (A.FState='finish') "; //��������ͨ��ʱ������
                + strUserID + "' and (A.FNewState=2) "; //��������ͨ��ʱ������

            if (strCheckType != "0")
            {
                strSql += " and A.FCheckType='" + strCheckType + "'";
            }

            strSql += " Order by FEndTime DESC";

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
            try
            {
                da.OpenConn();
                string Result = da.GetOneResult(strSql);
                return Int32.Parse(Result);
            }
            finally
            {
                da.Dispose();
            }
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "�����������")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataSet getCheckType()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
            try
            {
                da.OpenConn();
                string str = "SELECT ftypeid,ftypeName FROM c2c_fmdb.t_check_type  order by  ftypeName asc";
               // DataTable dt = da.GetTable(str);
                DataSet ds = da.dsGetTotalData(str);
               
                return ds;
            }
            finally
            {
                da.Dispose();
            }
        }
    }
    
}
