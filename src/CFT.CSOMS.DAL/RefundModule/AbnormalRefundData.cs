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
using System.Threading;
using CFT.Apollo.Logging;

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
            strOut += " FbankTypeOld,FoldId,FrefundType,FUserEmail,FReturnstate,Fstate,FBuyBanktype,FcheckID,FuserFlag,FSPID,fbuyid,FUserTEL ";
            return strOut;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strUid"></param>
        /// <param name="strBank"></param>
        /// <param name="strFSPID"></param>
        /// <param name="strBeginDate"></param>
        /// <param name="strEndDate"></param>
        /// <param name="iCheck"></param>
        /// <param name="iTrade">退款状态</param>
        /// <param name="strOldID"></param>
        /// <param name="strOperater"></param>
        /// <param name="strMin"></param>
        /// <param name="strMax"></param>      
        /// <param name="payScene">支付场景 0:全部 1:微信 2:非微信</param>
        /// <param name="payChannel">支付渠道 0:全部 1:快捷 2:非快捷</param>
        /// <returns></returns>
        public DataSet RequestRefundData(string strUid, string strBank, string strFSPID, string strBeginDate, string strEndDate, int iCheck, int iTrade, string strOldID, string strOperater, string strMin, string strMax, int payScene, Tuple<int, List<string>> payChannel) 
        {
            var term = new StringBuilder();
            if(!string.IsNullOrEmpty(strUid))
            {
                term.AppendFormat(" and FpayListid ='{0}'", strUid);
            }
            if (!string.IsNullOrEmpty(strBank))
            {
                term.AppendFormat(" and FbankListid ='{0}'", strBank);
            }
            if (!string.IsNullOrEmpty(strFSPID))
            {               
                string[] arySpID = strFSPID.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (arySpID.Length > 0)
                {
                    var inParam = string.Join("','", arySpID);
                    term.AppendFormat(" and FSPID in ('{0}')", inParam);
                }              
            }
            if (!string.IsNullOrEmpty(strBeginDate))
            {
                term.AppendFormat(" and FcreateTime >= '{0}'", strBeginDate);
            }
            if (!string.IsNullOrEmpty(strEndDate))
            {
                term.AppendFormat(" and FcreateTime <= '{0}'", strEndDate);
            }
            if (iCheck != -1)
            {
                term.AppendFormat(" and Fstate ={0}", iCheck.ToString());
            }
            /////string strOldID,string strOperater,string strMin,string strMax
            if (!string.IsNullOrEmpty(strOldID))
            {
                term.AppendFormat(" and FoldId = '{0}'", strOldID);
            }
            if (!string.IsNullOrEmpty(strOperater))
            {
                term.AppendFormat(" and FStandby1 like '{0}%'", strOperater);
            }
            if (!string.IsNullOrEmpty(strMin))
            {
                term.AppendFormat(" and FReturnAmt >='{0}'", strMin);
            }
            if (!string.IsNullOrEmpty(strMax))
            {
                term.AppendFormat(" and FReturnAmt <='{0}'", strMax);
            }
            if (payScene!=0)
            {
                var isNot = payScene == 1 ? "" : "not";
                term.AppendFormat(" and Fbuyid {0} like '%@wx.tenpay.com'", isNot);
            }
            if (payChannel.Item1 != 0)
            {
                var inParam =string.Join(",", payChannel.Item2);
                var isNot = payChannel.Item1 == 1 ? "" : "not";
                term.AppendFormat(" and FBuyBanktype {0} in({1})", isNot, inParam);
            }
            DataSet ds = null;
            if (term.Length > 0)
            {
                term.Remove(0, 4);  //去掉开始的" and"
                var querySQL = GetOutputFields() + " FROM c2c_zwdb.t_refund_KF WHERE" + term.ToString();
                using (var da = MySQLAccessFactory.GetMySQLAccess("RefundDB"))
                {
                    da.OpenConn();
                    ds = da.dsGetTotalData(querySQL);
                }

                #region 退款状态 查询 特殊处理
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var kf_dt = ds.Tables[0];
                    kf_dt.Columns.Add(new DataColumn("TotalDB_Fstate", typeof(int)) { DefaultValue = -1 });
                    kf_dt.Columns.Add("TotalDB_FCreateMemo", typeof(string));

                    var foldIds = kf_dt.AsEnumerable().Select(u => (string)u["FoldId"]).ToArray();  //获取 FoldId 的集合 去主表(t_refund_total) 查找 退款状态(fstate)
                    var foldIdParam = string.Join("','", foldIds);
                    var totalQuerySQL = string.Format("SELECT FoldId,Fstate as TotalDB_Fstate,FCreateMemo as TotalDB_FCreateMemo FROM c2c_zwdb.t_refund_total WHERE FoldId IN('{0}')", foldIdParam);

                    if (iTrade != 0)  // 如果查询条件中加入了 退款状态查询
                    {
                        totalQuerySQL += " and fstate =" + iTrade.ToString();
                    }

                    using (var totalda = MySQLAccessFactory.GetMySQLAccess("ZWTK"))
                    {
                        totalda.OpenConn();
                        DataSet totalds = totalda.dsGetTotalData(totalQuerySQL);

                        if (totalds != null && totalds.Tables.Count > 0 && totalds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < kf_dt.Rows.Count; i++)
                            {
                                var totalInfo = totalds.Tables[0].Select("FoldId = '" + (string)kf_dt.Rows[i]["FoldId"] + "'").FirstOrDefault();
                                if (totalInfo != null)
                                {
                                    kf_dt.Rows[i]["TotalDB_Fstate"] = totalInfo["TotalDB_Fstate"];
                                    kf_dt.Rows[i]["TotalDB_FCreateMemo"] = totalInfo["TotalDB_FCreateMemo"];
                                }
                                else if (iTrade != 0)
                                {
                                    kf_dt.Rows.RemoveAt(i--);
                                }
                            }
                        }
                    }
                } 
                #endregion
            }

            //strOut = "查询条件为空，请输入任意查询条件！";
            return ds;
        }

        public bool UpdateRefundData(string[] strOldId, string strIdentity, string strBankAccNoOld, string strUserEmail, string strNewBankAccNo,string strBankUserName,
            string strReason, string strImgCommitment, string strImgIdentity, string strImgBankWater, string strImgCancellation, string strBankName,string strOperater,
            int nOldBankType, int nNewBankType, int? nUserFalg, int? nCardType, int nState, out string outMsg)
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
                if (nUserFalg != null)
                {
                    strSql += "FuserFlag=" + nUserFalg + ",";
                }
                if (nCardType != null)
                {
                    strSql += "FCardType=" + nCardType + ",";
                }
                strSql += "FcurType = 1" + ",";
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

        /// <summary>
        /// 异常付款退款单明细查询
        /// </summary>
        /// <param name="sTime">异常写入时间 开始时间</param>
        /// <param name="eTime">异常写入时间 结束时间</param>
        /// <param name="batchID">批次</param>
        /// <param name="packageID">包ID</param>
        /// <param name="listid">业务单号</param>
        /// <param name="type">业务类型</param>
        /// <param name="product">产品标示  product+business_type 共同确定子业务类型</param>
        /// <param name="business_type"> 业务类型 </param>
        /// <param name="notity">是否已通知用户</param>
        /// <param name="bankType">银行类型</param>
        /// <param name="errorType">错误类型</param>
        /// <param name="accType">账户类型</param>
        /// <param name="start"></param>
        /// <param name="max"></param>
        /// <param name="count">记录总数 ref -1 时查询记录数并返回 否则不返回</param>
        /// <returns></returns>
        public DataSet QueryPaymenAbnormal(string sTime, 
            string eTime, string batchID, string packageID, string listid, string type,
            string product, string business_type, string notityStatus, string notityResult, string bankType, string errorType,
            string accType, int start, int max, ref int count)
        {
            string tbName = "c2c_zwdb.t_abnormal_roll_" + DateTime.Parse(sTime).ToString("yyyyMM");
            StringBuilder Sql = new StringBuilder("select Ftype,Fabnormal_time,FBatchID,FPackageID,Flistid,Fproduct,Fbusiness_type,Fbank_type,Ferror_type,Faccount_type,Fspid,Fnotify_type,Fnotify_result,Fnotify_status,Fnotify_history ");
            Sql.AppendFormat(" from {0}", tbName);
            StringBuilder whereStr = WhereStrAbnRefund(sTime, eTime, batchID, packageID, listid, type, product, business_type, notityStatus, notityResult, bankType, errorType, accType);

            Sql.Append(whereStr);
            Sql.Append(" order by Fabnormal_time desc");
            Sql.Append(" limit " + start + "," + max);

            using (var da = MySQLAccessFactory.GetMySQLAccess("PaymenAbnormal"))
            {
                da.OpenConn();

                DataSet ds = da.dsGetTotalData(Sql.ToString());

                if (count == -1)
                {
                    var countsql = string.Format("SELECT count(1) FROM {0} {1} ", tbName, whereStr.ToString());
                    count =int.Parse(da.GetOneResult(countsql));
                }

                return ds;
            }
        }


        /// <summary>
        /// 付款延迟异常数据查询
        /// </summary>
        /// <param name="flistid">业务单号</param>
        /// <param name="time"></param>
        /// <returns></returns>
        public DataTable GetPaymenAbnormalByFListId(string flistid, DateTime time)
        {
            DataTable dt = null;

            string tbName = "c2c_zwdb.t_abnormal_roll_" ;
            string sqlStr = @"
            select FBatchID,Flistid from {0} where Flistid = '{1}'";
            using (var da = MySQLAccessFactory.GetMySQLAccess("PaymenAbnormal"))
            {
                da.OpenConn();
                try
                {
                    dt = da.GetTable(string.Format(sqlStr, tbName + time.ToString("yyyyMM"), flistid));
                }
                catch (Exception ex)
                {
                    LogHelper.LogError("CFT.CSOMS.DAL.RefundModule.AbnormalRefundData  public DataTable GetPaymenAbnormalByFListId(string flistid, DateTime time),异常：" + ex.ToString());
                }
                if (dt == null || dt.Rows.Count == 0)
                {
                    dt = da.GetTable(string.Format(sqlStr, tbName + time.AddMonths(1).ToString("yyyyMM"), flistid));
                }
            }
            return dt;
        }

      
        /// <summary>
        /// 通过交易单号更新异常付款退款单明细
        /// 发送通知
        /// </summary>
        /// <param name="sTime">查询时间</param>
        /// <param name="dicData">更新字段及数据  包含后面注释的字段</param>
        /// <param name="appid">公众号appid 发微信要</param>
        /// <param name="template_id">消息模板ID 发邮件 短信等</param>
        /// <param name="Fnotify_detail">通知详情</param>
        /// <param name="Fnotify_type">通知类型</param>
        /// <param name="Fpre_arrival_time">预计到账时间</param>
        /// <param name="Fnotify_sender">通知发送者</param>
        /// <param name="Fclient_ip">消息发送者ip</param>
        /// <param name="Fnotify_history">历史通知方式</param>
        /// <param name="listids">交易单号列表</param>
        /// <returns></returns>
        public void UpdatePaymenAbnormalByListid(string sTime,Dictionary<string, string> dicData, ArrayList listids)
        {
            StringBuilder Sql = updateFiledAbnRefund(sTime, dicData["template_id"],
                dicData["notify_detail"], dicData["notify_type"],
                dicData["pre_arrival_time"], dicData["notify_sender"],
                dicData["client_ip"]);

            string allListid = "";
            if (listids == null || listids.Count == 0)
                throw new Exception("没有需要更新的交易单！");
            foreach (string id in listids)
            {
                allListid += "'" + id + "',";
            }
           allListid= allListid.Substring(0,allListid.Length-1);
            Sql.Append(" where Flistid in(" + allListid + ")");

            using (var da = MySQLAccessFactory.GetMySQLAccess("PaymenAbnormal"))
            {
                da.OpenConn();

                da.ExecSqlNum(Sql.ToString());
            }
        }

        /// <summary>
        /// 通过查询条件更新所有异常付款退款单明细
        /// 发送通知
        /// </summary>
        /// <param name="dicCondition">查询条件</param>
        /// <param name="dicData">更新字段及数据 </param>
        /// <returns></returns>
        public int UpdatePaymenAbnormalByQuery(Dictionary<string, string> dicCondition, Dictionary<string, string> dicData)
        {
            StringBuilder Sql = updateFiledAbnRefund(dicCondition["sTime"], dicData["template_id"],
                dicData["notify_detail"], dicData["notify_type"],
                dicData["pre_arrival_time"], dicData["notify_sender"],
                dicData["client_ip"]);

            StringBuilder whereStr = WhereStrAbnRefund(dicCondition["sTime"],
                dicCondition["eTime"], dicCondition["batchID"], dicCondition["packageID"],
                dicCondition["listid"], dicCondition["type"], dicCondition["product"],
                dicCondition["business_type"], dicCondition["notityStatus"], dicCondition["notityResult"],
                dicCondition["bankType"], dicCondition["errorType"], dicCondition["accType"]);
            whereStr.Append(" order by Fabnormal_time desc");
            whereStr.Append(" limit 50");

            Sql.Append(whereStr);

            using (var da = MySQLAccessFactory.GetMySQLAccess("PaymenAbnormal"))
            {
                da.OpenConn();

                return da.ExecSqlNum(Sql.ToString());
            }
        }

        /// <summary>
        /// 查询和更新条件
        /// </summary>
        /// <returns></returns>
        private static StringBuilder WhereStrAbnRefund(string sTime, string eTime, string batchID, string packageID, string listid, string type, string product, string business_type, string notityStatus, string notityResult, string bankType, string errorType, string accType)
        {
            StringBuilder whereStr = new StringBuilder(" where Fcreate_time between '" + sTime + "' and '" + eTime + "' and  FBatchID='" + batchID + "' ");
            if (!string.IsNullOrEmpty(packageID))
            {
                whereStr.Append(" and FPackageID ='" + packageID + "'");
            }
            if (!string.IsNullOrEmpty(listid))
            {
                whereStr.Append(" and Flistid ='" + listid + "'");
            }
            if (!string.IsNullOrEmpty(type))
            {
                whereStr.Append(" and Ftype ='" + type + "'");
            }
            if (!string.IsNullOrEmpty(product) && !string.IsNullOrEmpty(business_type))//确定子业务类型
            {
                whereStr.Append(" and Fproduct ='" + product + "'");
                whereStr.Append(" and Fbusiness_type ='" + business_type + "'");
            }
            if (!string.IsNullOrEmpty(notityStatus))
            {
                whereStr.Append(" and Fnotify_status ='" + notityStatus + "'");
            }
            if (!string.IsNullOrEmpty(notityResult))
            {
                whereStr.Append(" and Fnotify_result ='" + notityResult + "'");
            }
            if (!string.IsNullOrEmpty(bankType))
            {
                whereStr.Append(" and Fbank_type ='" + bankType + "'");
            }
            if (!string.IsNullOrEmpty(errorType))
            {
                whereStr.Append(" and Ferror_type ='" + errorType + "'");
            }
            if (!string.IsNullOrEmpty(accType))
            {
                whereStr.Append(" and Faccount_type ='" + accType + "' ");
            }

            return whereStr;
        }

        /// <summary>
        /// 更新字段及数据
        /// </summary>
        /// <returns></returns>
        private static StringBuilder updateFiledAbnRefund(string sTime, string template_id, string notify_detail, string notify_type, string pre_arrival_time, string notify_sender, string client_ip)
        {
            string tbName = "c2c_zwdb.t_abnormal_roll_" + DateTime.Parse(sTime).ToString("yyyyMM");
            StringBuilder Sql = new StringBuilder("update " + tbName + " set Fmodify_time=now()");
            Sql.Append(", Fnotify_status='1'");
            Sql.Append(", Fnotify_type='" + notify_type + "'");
            Sql.Append(", Fpre_arrival_time='" + pre_arrival_time + "'");
            Sql.Append(", Ftemplate_id ='" + template_id + "'");
            if (!string.IsNullOrEmpty(notify_detail))
            {
                Sql.Append(", Fnotify_detail ='" + notify_detail + "'");
            }
            if (!string.IsNullOrEmpty(notify_sender))
            {
                Sql.Append(", Fnotify_sender ='" + notify_sender + "'");
            }
            if (!string.IsNullOrEmpty(client_ip))
            {
                Sql.Append(", Fclient_ip ='" + client_ip + "'");
            }
            return Sql;
        }

    }
}

        