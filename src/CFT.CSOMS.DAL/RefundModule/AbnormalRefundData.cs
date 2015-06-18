using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.CFT.KF.DataAccess;
using CFT.CSOMS.DAL.Infrastructure;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using System.Collections;

namespace CFT.CSOMS.DAL.RefundModule
{
    public class AbnormalRefundData
    {
      
        /*
        private enum eRCHK
        {
            First = 2,//同步订单状态从2开始要审批
            Second,
            Three,
            Four,
            Five,
            Six,
            Unkown
        };

        private enum eResult
        {
            OK = 1,
            Refund,
            ReturnCW,
            Unkown,
        }
        private string[] m_nOperator =
        {
            "yukini",
            "christyzeng",
            "yonghualiu",    
        };

        private string[] m_nUserInfo =
        {
            "空",
            "空",
            "Ffirstuser",
            "Fseconduser",
            "Fthreeuser",    
            "Nuknow",
        };


        private string m_nKfCheckName = "REFUND_CHECKU_SER";

        private static Hashtable m_htCheck;
       */
        private string GetOutputFields()
        {
            string strOut = "select FpayListid,FCardType,FbankListid,FbankName,FbankType,FcreateTime,FtrueName,FmodifyTime,FReturnAmt,FAmt,FbankAccNo,";
            strOut += " FbankTypeOld,FoldId,FrefundType,FUserEmail,FReturnstate,Fstate,FBuyBanktype,FcheckID,FuserFlag,FSPID ";
            return strOut;
        }
        public DataSet RequestRefundData(string strUid, string strBank, string strFSPID, string strBeginDate, string strEndDate, int iCheck, int iTrade,string strOldID,string strOperater,string strMin,string strMax) 
        {
            StringBuilder Sql = new StringBuilder(GetOutputFields()+" from c2c_zwdb.t_refund_KF where 1 = 1");
            bool bState = false;
           
            if(!string.IsNullOrEmpty(strUid))
            {
                bState = true;
                Sql.Append(" and FpayListid ='"+strUid+"'");
            }
            if (!string.IsNullOrEmpty(strBank))
            {
                bState = true;
                Sql.Append(" and FbankListid ='" + strBank + "'");
            }
            if (!string.IsNullOrEmpty(strFSPID))
            {
                bState = true;
                
                string[] arySpID = strFSPID.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (arySpID.Length < 1)
                {
                    bState = false;
                }
                for (int i=0;i<arySpID.Length;++i)
                {
                    if (i == 0)
                    {
                        Sql.Append(" and( ");
                    }
                    else
                    {
                        Sql.Append(" or");
                    }

                    Sql.Append(" FSPID ='" + arySpID[i] + "'");
                }
                if (bState)
                {
                    Sql.Append(" )");
                }
                
            }
            if (!string.IsNullOrEmpty(strBeginDate))
            {
                bState = true;
                Sql.Append(" and FcreateTime >= '" +strBeginDate+ "' ");               
            }
            if (!string.IsNullOrEmpty(strEndDate))
            {
                bState = true;
                Sql.Append(" and FcreateTime <= '" +strEndDate+ "' ");
            }
            if (iCheck != -1)
            {
                bState = true;
                Sql.Append(" and Fstate =" + iCheck);
            }
            if ( iTrade != 0)
            {
                bState = true;
                Sql.Append(" and FReturnstate =" + iTrade);
            }
            /////string strOldID,string strOperater,string strMin,string strMax
            if (!string.IsNullOrEmpty(strOldID))
            {
                bState = true;
                Sql.Append(" and FoldId = '" + strOldID + "' ");
            }
            if (!string.IsNullOrEmpty(strOperater))
            {
                bState = true;
                Sql.Append(" and FStandby1 like '" + strOperater + "%' ");
            }
            if (!string.IsNullOrEmpty(strMin))
            {
                bState = true;
                Sql.Append(" and FReturnAmt >='" + strMin +"'");
            }
            if (!string.IsNullOrEmpty(strMax))
            {
                bState = true;
                Sql.Append(" and FReturnAmt <='" + strMax + "'");
            }
            /////
            if (!bState)
            {
                //strOut = "查询条件为空，请输入任意查询条件！";
                return null;
            }
            using (var da = MySQLAccessFactory.GetMySQLAccess("RefundDB"))
            {
                da.OpenConn();
                
                DataSet ds = da.dsGetTotalData(Sql.ToString());

                return ds;
            }
            return null;
        }

        public bool UpdateRefundData(string[] strOldId, string strIdentity, string strBankAccNoOld, string strUserEmail, string strNewBankAccNo,string strBankUserName,
            string strReason, string strImgCommitment, string strImgIdentity, string strImgBankWater, string strImgCancellation, string strBankName,string strOperater,
            int nOldBankType, int nNewBankType, int nUserFalg, int nCardType, int nState, out string outMsg)
        {
            outMsg = "";
            try
            {
                string strSql = "update c2c_zwdb.t_refund_KF set ";                
                if (!string.IsNullOrEmpty(strIdentity))
                {
                    strSql += "Fidentity='" + strIdentity + "',";
                }
                if (!string.IsNullOrEmpty(strBankAccNoOld))
                {                    
                    strSql += "FbankAccNoOld='" + strBankAccNoOld + "',";
                }
                if (!string.IsNullOrEmpty(strUserEmail))
                {                    
                    strSql += "FUserEmail='" + strUserEmail + "',";
                }
                if (!string.IsNullOrEmpty(strNewBankAccNo))
                {                
                    strSql += "FbankAccNo='" + strNewBankAccNo + "',";
                }
                if (!string.IsNullOrEmpty(strBankUserName))
                {                   
                    strSql += "FtrueName='" + strBankUserName + "',";
                }
                if (!string.IsNullOrEmpty(strReason))
                {                  
                    strSql += "Fkfremark='" + strReason + "',";
                }
                if (!string.IsNullOrEmpty(strImgCommitment))
                {                   
                    strSql += "FCommitmentFile='" + strImgCommitment + "',";
                }
                if (!string.IsNullOrEmpty(strImgIdentity))
                {                  
                    strSql += "FIdentityCardFile='" + strImgIdentity + "',";
                }
                if (!string.IsNullOrEmpty(strImgBankWater))
                {                   
                    strSql += "FBankWaterFile='" + strImgBankWater + "',";
                }
                if (!string.IsNullOrEmpty(strImgCancellation))
                {                  
                    strSql += "FCancellationFile='" + strImgCancellation + "',";
                }
                if (!string.IsNullOrEmpty(strBankName))
                {
                    strSql += "FbankName='" + strBankName + "',";
                }
                if (!string.IsNullOrEmpty(strOperater))
                {
                    strSql += "FStandby1='" + strOperater + "',";
                }

                if (nOldBankType != -1)
                {                  
                    strSql += "FbankTypeOld='" + nOldBankType + "',";
                }
                if (nNewBankType != -1)
                {                   
                    strSql += "FbankType='" + nNewBankType + "',";
                }
                if (nState >= 0 && nState <= 15)
                {                 
                    strSql += "Fstate= " + nState + ",";
                }
                strSql += "FuserFlag=" + nUserFalg + "," +"FcurType = 1"+","+"FCardType="+nCardType+",";
                strSql += "FmodifyTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'  where";
                for (int i=0; i< strOldId.Length;++i)
                {
                    if (i != 0)
                    {
                        strSql += " or ";
                    }

                    strSql += " FoldId='" + strOldId[i] + "'";
                }
                

                using (var da = MySQLAccessFactory.GetMySQLAccess("RefundDB"))
                {
                    da.OpenConn();
                    if (!da.ExecSql(strSql.ToString()))
                    {
                        outMsg = "执行补填SQL语句出错！";
                        return false;
                    }
                     return true;
                }
                
            }
            catch (Exception eStr)
            {                
                outMsg = "客服补填特殊退款资料插入失败！" + "[" + eStr.Message.ToString() + "]";
                return false;
            }
        }

        private string GetDetailFields()
        {
            string strOut = "select FpayListid,FbankListid, Fidentity,FbankAccNoOld,FbankNameOld,FbankTypeOld,FUserEmail,FbankAccNo,FbankName,FbankType,FHisCheckID";
            strOut += " ,Fstate,FcreateTime,FtrueName,Fkfremark,FIdentityCardFile,FCommitmentFile,FBankWaterFile,FCancellationFile,FcheckID,FuserFlag,FCardType,FbankName,FStandby1,FStandby2 ";
            return strOut;
        }

        public DataSet RequestDetailsData(string strRefundId, out string outMsg)
        {
            outMsg = null;
            try
            {
                
                string strSQL = GetDetailFields() + " from c2c_zwdb.t_refund_KF where FoldId='" + strRefundId +"'";
                using (var da = MySQLAccessFactory.GetMySQLAccess("RefundDB"))
                {
                    da.OpenConn();

                    DataSet ds = da.dsGetTotalData(strSQL.ToString());
                    
                    return ds;
                }
            }
            catch (Exception ex)
            {
                outMsg = "查询退款详细失败！" + "[" + ex.Message.ToString() + "]";
            }
            return null;
        }

        public string RequestItemState(string strRefundId)
        {     
            try
            {
                string strSQL = "select Fstate from c2c_zwdb.t_refund_KF where FoldId='" + strRefundId + "'";
                using (var da = MySQLAccessFactory.GetMySQLAccess("RefundDB"))
                {
                    da.OpenConn();
                    DataSet ds = da.dsGetTotalData(strSQL.ToString());
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {                      
                        return ds.Tables[0].Rows[0]["Fstate"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        public string GetBankCardBindInformation(string listid, out string msg)
        {
            msg = "开始执行";
            try
            {               
                string striceWhere = "listid=" + listid;
                msg += striceWhere;
                string strout = "";
                //DataTable dtIce = CommQuery.GetTableFromICE(striceWhere, "query_order_service", out msg);
                DataSet ds = CommQuery.GetDataSetFromICE(striceWhere, CommQuery.QUERY_ORDER, out strout);
                msg += "通过ICE取值"+strout;
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    msg += "ds有值=" + ds.Tables[0].Rows[0]["Fbuyid"].ToString();
                    //买家帐户号码（财付通帐号）
                    return ds.Tables[0].Rows[0]["Fbuyid"].ToString();
                }
            }
            catch (Exception ex)
            {
                msg += ex.Message;
                
            }
    
            return null;
        }

        public void SetRefundCheckState(int nState, string strOldId)
        {
            try
            {
                if(string.IsNullOrEmpty(strOldId))
                {
                    return;
                }
                string strSql = "update c2c_zwdb.t_refund_KF set Fstate =" + nState + ",";
                strSql += "FmodifyTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where ";
                string[] aryOldID = strOldId.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i=0;i<aryOldID.Length;++i)
                {
                    if (i != 0)
                    {
                        strSql += "or";
                    }
                    if (!string.IsNullOrEmpty(aryOldID[i]))
                    {
                        strSql += "  FoldId='" + aryOldID[i] + "'";
                    }                
                }
                
                using (var da = MySQLAccessFactory.GetMySQLAccess("RefundDB"))
                {
                    da.OpenConn();
                    da.ExecSql(strSql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void SetAbnormalRefundListID(string strCheckId,string strOldId)
        {
            try
            {
                string strSql = "update c2c_zwdb.t_refund_KF set FcheckID = '" + strCheckId + "',";
                strSql += "FmodifyTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                strSql += " where  FoldId='" + strOldId + "'";
                using (var da = MySQLAccessFactory.GetMySQLAccess("RefundDB"))
                {
                    da.OpenConn();
                    da.ExecSql(strSql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteAbnormalRefundRecord(string strOldId)
        {
            try
            {
                string strSql = "update c2c_zwdb.t_refund_KF where FoldId='" + strOldId + "'";
                using (var da = MySQLAccessFactory.GetMySQLAccess("RefundDB"))
                {
                    da.OpenConn();
                    da.ExecSql(strSql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //查询审批编号
        public string  QueryAbnormalRefundCheckID(string strOldId ,ref string strHisCheckID)
        {
            try
            {
                string strSql = "select FcheckID,FHisCheckID from c2c_zwdb.t_refund_KF where FoldId='" + strOldId + "'";
                using (var da = MySQLAccessFactory.GetMySQLAccess("RefundDB"))
                {
                    da.OpenConn();
                    DataSet ds =  da.dsGetTotalData(strSql);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        strHisCheckID = ds.Tables[0].Rows[0]["FHisCheckID"].ToString();
                        return ds.Tables[0].Rows[0]["FcheckID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        //查财务转客服记录
        public DataSet CheckFZWCheckMemo(string strOldId)
        {
            try
            {
                string strSql = "select FHisCheckID,FZWCheckMemo  from c2c_zwdb.t_refund_KF where FoldId='" + strOldId + "'";
                using (var da = MySQLAccessFactory.GetMySQLAccess("RefundDB"))
                {
                    da.OpenConn();                     
                    return da.dsGetTotalData(strSql);                   
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        //
        public void RequestInfoChange(string strTxt, string strOperator)
        {
            try
            {
                if (string.IsNullOrEmpty(strTxt))
                {
                    return;
                }
                if (strOperator != "yonghualiu")
                {
                    return;
                }
 
                using (var da = MySQLAccessFactory.GetMySQLAccess("RefundDB"))
                {
                    da.OpenConn();
                    da.ExecSql(strTxt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /*        
         //  设置审批结果
         //  Fcheckresult:单次审批结果 1：同意 2，拒绝
         //  Fcheckstate:审批单的状态 1：当前审批记录 2：历史记录
         
        public bool SetRefundCheckState(string strRefundId, string strUser, int nState,string strMemo, int nResult)
        {
            if (nState >= 2 && nState <= 4)
            {
                return false;
            }
            if(!IsCheckOperator(strUser,nState))
            {
                return false;
            }
            string strSql = "select Fstate  from c2c_zwdb.t_refund_KF where FoldId='" + strRefundId + "'";
            using (var da = MySQLAccessFactory.GetMySQLAccess("RefundDB"))
            {
                da.OpenConn();
                DataSet ds = da.dsGetTotalData(strSql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //验证web传值，是否被恶意改动过
                    int nCurState = int.Parse(ds.Tables[0].Rows[0]["Fstate"].ToString());
                    if (nState != nCurState)
                    {
                        return false;
                    }
                    switch(nResult)
                    {
                        case (int)eResult.OK:
                            {

                                if (nCurState >= (int)eRCHK.First && nCurState <= (int)eRCHK.Four)
                                {
                                    int nSetState = nCurState + 1;
                                    string strStateSql = "updata c2c_zwdb.t_refund_KF set Fstate =" + nSetState + 
                                        ",FmodifyTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +"' where FoldId = '" + strRefundId + "'";
                                    da.ExecSql(strStateSql);
                                    //日志
                                    string strKey = "";
                                    string strValue = "";
                                    string strRecordSql = "insert into c2c_zwdb.t_refund_kfcheckinfo(";
                                    strKey = "Foldid,Fcheckresult,Fcheckremark,Fcheckuser,Fchecklevel,Fcheckstate,Frceatetime )";
                                    strValue = "values('" + strRefundId +"','1','" +strMemo+"','"+strUser+"'," +(nCurState-1)+",'1','"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')" ;
                                    string strExSql = strRecordSql + strKey + strValue;
                                    da.ExecSql(strExSql);

                                }

                                break;
                            }
                        case (int)eResult.Refund:
                                {
                                    int nSetState = 0;
                                    string strStateSql = "updata c2c_zwdb.t_refund_KF set Fstate =" + nSetState +
                                        ",FmodifyTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where FoldId = '" + strRefundId + "'";
                                    da.ExecSql(strStateSql);
                                    //日志
                                    string strKey = "";
                                    string strValue = "";
                                    string strRecordSql = "insert into c2c_zwdb.t_refund_kfcheckinfo(";
                                    strKey = "Foldid,Fcheckresult,Fcheckremark,Fcheckuser,Fchecklevel,Fcheckstate,Frceatetime )";
                                    strValue = "values('" + strRefundId + "','2','" + strMemo + "','" + strUser + "'," + (nCurState - 1) + ",'1','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                    string strExSql = strRecordSql + strKey + strValue;
                                    da.ExecSql(strExSql);

                                    //将当前审批日志，全改成历史日志标识
                                    string strRuSql = "updata into c2c_zwdb.t_refund_kfcheckinfo set Fcheckstate = 2 where Foldid='" + strRefundId + "'";
                                    da.ExecSql(strRuSql);

                                }
                            break;
                        case (int)eResult.ReturnCW:
                                {
                                    int nSetState = 5;
                                    string strStateSql = "updata c2c_zwdb.t_refund_KF set Fstate =" + nSetState +
                                        ",FmodifyTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where FoldId = '" + strRefundId + "'";
                                    da.ExecSql(strStateSql);
                                    //日志
                                    string strKey = "";
                                    string strValue = "";
                                    string strRecordSql = "insert into c2c_zwdb.t_refund_kfcheckinfo(";
                                    strKey = "Foldid,Fcheckresult,Fcheckremark,Fcheckuser,Fchecklevel,Fcheckstate,Frceatetime )";
                                    strValue = "values('" + strRefundId + "','1','" + strMemo + "','" + strUser + "'," + (nCurState - 1) + ",'1','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                    string strExSql = strRecordSql + strKey + strValue;
                                    da.ExecSql(strExSql);
                                }
                            break;
                        }
                       
                    }
            }


          return false;
        }

        
        //读取日志
        public DataTable ReadKFCheckInfo(string strRefundId, int nType)
        {
            //简单验证下
            if (nType != 1 || nType != 2)
            {
                return null;
            }
          
           using (var da = MySQLAccessFactory.GetMySQLAccess("RefundDB"))
           {
               string strSql = "select Fcheckuser,Fchecklevel,Frceatetime,Fcheckremark from c2c_zwdb.t_refund_kfcheckinfo where Fcheckresult=" + nType + "and  Fcheckstate =" + nType + "and Foldid='" + strRefundId + "'";
               if (nType == 2)
               {
                   strSql += " order by Frceatetime desc";
               }
               return da.GetTable(strSql);
           }
        }

        //初始化哈稀表
        private void InitHTChecek()
        {

                if (m_htCheck != null)
                {
                    return ;
                }
                m_htCheck = new Hashtable();
                string strSql = "select Ffirstuser,Fseconduser,Fthreeuser,Ffouruser,Ffiveuser from c2c_zwdb.t_refund_checkconfig where FName = '" + m_nKfCheckName +"'";
                using (var da = MySQLAccessFactory.GetMySQLAccess("RefundDB"))
                {
                    da.OpenConn();

                    DataSet ds = da.dsGetTotalData(strSql.ToString());
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        m_htCheck["Ffirstuser"]     = ds.Tables[0].Rows[0]["Ffirstuser"].ToString();
                        m_htCheck["Fseconduser"]    = ds.Tables[0].Rows[0]["Fseconduser"].ToString();
                        m_htCheck["Fthreeuser"]     = ds.Tables[0].Rows[0]["Fthreeuser"].ToString();
                        // m_htCheck["Ffouruser"]      = ds.Tables[0].Rows[0]["Ffouruser"].ToString();
                        //  m_htCheck["Ffiveuser"]      = ds.Tables[0].Rows[0]["Ffiveuser"].ToString();
                    }
                }
                                 
        }
        //判断是否有权限增加帅哥美女
        private bool IsKFOperator(string strOperator)
        {
            for (int i = 0; i < m_nOperator.Length; ++i)
            {
                if (m_nOperator[i].Trim() == strOperator.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        //设置审人组信息(可批量设置)
        public bool SetKFCheckUserInfo(string strUserInfo,string strUserLevel,string strOperator,int nType)
        {

            try
            {
                //权限验证
                if (!IsKFOperator(strOperator))
                {
                    return false;
                }
                
                string[] strArr = strUserInfo.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (strArr.Length <= 0)
                {
                    return false;
                }
                strUserLevel = strUserLevel.Trim();
                InitHTChecek();
                if (m_htCheck.Contains(strUserLevel))
                {
                    string strUser ="";
                    if (nType == -1)
                    {
                       string[] strArryUser = m_htCheck[strUserLevel].ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);              
                        for(int k=0;k<strArryUser.Length;++k)
                        {
                            bool bState = true;
                            for (int i = 0; i < strArr.Length; ++i)
                            {
                                if (strArr[i].Trim() == strArryUser[k].Trim())
                                {
                                    bState = false;
                                }
                            }
                            if (bState)
                            {

                                strUser += strArryUser[k].Trim() + '|';
                            }                           
                        }                
                    }
                    else
                    {
                        string strTemp = m_htCheck[strUserLevel].ToString();
                        if (string.IsNullOrEmpty(strTemp.Trim()))
                        {
                            strUser = strUserInfo;
                        }
                        else
                        {
                            strUser = (strTemp.Substring(strTemp.Length - 1, 1).ToString() == "|") ? strTemp + strUserInfo : strTemp + '|' + strUserInfo;
                        }
                        
                        
                    }
                    
                    string strSql = "update c2c_zwdb.t_refund_checkconfig set " + strUserLevel + "='" + strUser + " 'where FName = '" + m_nKfCheckName + "'";
                    using (var da = MySQLAccessFactory.GetMySQLAccess("RefundDB"))
                    {
                        da.OpenConn();
                        da.ExecSql(strSql);
                    }
                    //同步内存值
                    m_htCheck[strUserLevel] = strUser;
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
            
        }

        //当前同学是否有审批权限
        public bool IsCheckOperator(string strUser,int nState)
        {
                if(nState <2 && nState> 4)
                {
                    return false;
                }
                string strUserLevel = m_nUserInfo[nState];
                if (m_htCheck.Contains(strUserLevel))
                {
                    string[] strArryUser = m_htCheck[strUserLevel].ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < strArryUser.Length; ++i)
                    {
                        if (strArryUser[i].Trim() == strUser.Trim())
                        {
                            return true;
                        }
                    }
                }
                return false;
        }
     */
    }
}

        