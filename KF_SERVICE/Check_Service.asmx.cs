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
    /// Check_Service 的摘要说明。
    /// </summary>
    [WebService(Namespace = "http://Tencent.com/OSS/C2C/Finance/Check_Service")]
    public class Check_Service : System.Web.Services.WebService
    {
        /// <summary>
        /// SOAP头
        /// </summary>
        public Finance_Header myHeader;

        public Check_Service()
        {
            //CODEGEN: 该调用是 ASP.NET Web 服务设计器所必需的
            InitializeComponent();
        }


        #region 组件设计器生成的代码

        //Web 服务设计器所必需的
        private IContainer components = null;

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
        }

        /// <summary>
        /// 清理所有正在使用的资源。
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
        /// 开始一个审批
        /// </summary>
        /// <param name="strMainID">操作关键字ID</param>
        /// <param name="strCheckType">审批类型</param>
        /// <param name="strMemo">详细描述</param>
        /// <param name="strLevelValue">传入的审批级别判断值</param>
        /// <param name="myParams">需要的参数列表</param>
        [WebMethod(Description = "提请一个审批")]
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
                    throw new LogicException("不正确的调用方法！");
                }
                rl.actionType = "提请一个审批";
                rl.ID = strMainID;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "StartCheck";
                rl.SzKey = myHeader.SzKey;
                rl.type = "发起审批";
                rl.RightString = myHeader.RightString;
                rl.UserID = myHeader.UserName.Trim().ToLower();
                rl.UserIP = myHeader.UserIP;

                //先检查权限 Ray 2005.10.20 所有的审批之前需要先判断权限 
                //是执行条件的前提校验
                //priorCheck pc = new priorCheck();
                IpriorCheck pc = priorCheck.GetHandler(strCheckType);

                //if (pc.doPriorCheck(strCheckType) != null) //如果为空 默认通过权限验证
                if (pc != null) //如果为空,也就是这种审批类型没有处理,就通过前提校验. 
                {
                    //bool exeSign = pc.doPriorCheck(strCheckType).checkRight(myParams,out Msg);
                    bool exeSign = pc.checkRight(myParams, out Msg);
                    if (exeSign == false)
                    {
                        //throw new LogicException("发起权限检查失败！" + Msg);
                        throw new LogicException("执行条件的检查失败！" + Msg);
                    }
                }

                Check_Class.CreateCheck(rl.UserID, strMainID, strCheckType, strMemo, strLevelValue, myParams, myHeader);

                rl.detail = "用户" + rl.UserID + "在" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "时间对ID为" + strMainID
                    + "的记录进行了" + strCheckType + "的审批，执行结果成功！";
            }
            catch (LogicException err)
            {
                rl.sign = 0;
                rl.detail = "用户" + rl.UserID + "在" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "时间对ID为" + strMainID
                    + "的记录进行了" + strCheckType + "的审批，执行结果失败！" + err.Message;
                throw;
            }
            catch (Exception err)
            {
                rl.sign = 0;
                rl.detail = "用户" + rl.UserID + "在" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "时间对ID为" + strMainID
                    + "的记录进行了" + strCheckType + "的审批，执行结果失败！" + err.Message;
                throw new LogicException("Service处理失败！");
            }
            finally
            {
                rl.WriteLog();
            }
        }



        /// <summary>
        /// 执行审批完成后的动作
        /// </summary>
        /// <param name="strCheckID">审批ID</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "执行审批完的动作")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public bool ExecuteCheck(string strCheckID)
        {
            RightAndLog rl = new RightAndLog();
            try
            {

                if (myHeader == null)
                {
                    throw new LogicException("不正确的调用方法！");
                }
                rl.actionType = "执行审批完的动作";
                rl.ID = strCheckID;
                rl.OperID = myHeader.OperID;
                rl.sign = 1;
                rl.strRightCode = "ExecuteCheck";
                rl.SzKey = myHeader.SzKey;
                rl.RightString = myHeader.RightString;
                rl.type = "执行审批";
                rl.UserID = myHeader.UserName.Trim().ToLower();
                rl.UserIP = myHeader.UserIP;
                //				if(!rl.CheckRight())
                //				{
                //					throw new LogicException("用户无权执行此操作！");
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
                throw new LogicException("失败！" + err.Message.ToString().Replace("'", "’"));
            }
            finally
            {
                rl.WriteLog();
            }
        }

        /// <summary>
        /// 读取审批日志
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "读取审批日志")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataTable GetCheckLog(string checkid,int iStartIndex, int iRecordCount)
        {
            DataTable dt = null;
             string strSql = "SELECT fid, fcheckid,fcheckTime,fcheckuser,fcheckmemo,"
                    //furion 20050105 处理审批SQL
                    //+"(case  fcheckresult when 0 then '同意' when 1 then '<font color = red>不同意</font>' end) as fcheckresult " 
                    + "fcheckresult as ifcheckresult "
                    + "FROM c2c_fmdb.t_check_list "
                    + " where FcheckID=" + checkid.Trim();

                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
                try
                {
                    da.OpenConn();
                    dt = da.GetTableByRange(strSql, iStartIndex, iRecordCount);

                    //furion 20050105 处理审批SQL
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
                                dr["fcheckresult"] = "同意";
                            }
                            else if (tmp == "1")
                            {
                                dr["fcheckresult"] = "<font color = red>不同意</font>";
                            }

                            dr.EndEdit();
                        }
                    }
                    //furion end
                    return dt;
                }
                    catch (Exception e)
                {
                    throw new Exception("读取审批日志失败！ 请联系管理员。");
                    return dt;
                }
               
                finally
                {
                    da.Dispose();
                }
           
            
        }

        /// <summary>
        /// 未审批数量查询
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "未审批数量查询")]
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
        /// 已审批数量查询
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "已审批数量查询")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public int GetFinishCheckCount(string strCheckType, string strUserID)
        {
            string strSql = "select count(1) "
                + "from c2c_fmdb.t_check_main A where  A.FStartUser='"
                //+ strUserID + "' and (A.FState='finish' or A.FState='finished' ) "; //按照审批通过时间排序
                //+ strUserID + "' and (A.FNewState>=2 and A.FNewState<=3 ) "; //按照审批通过时间排序
                + strUserID + "' and (A.FNewState>=2 and A.FNewState<=4 ) "; //按照审批通过时间排序

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
        /// 我发起的审批，数据绑定处理
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "数据绑定处理")]
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
                //+ ",(case A.fstate when 'start' then '开始' when 'check' then '审批中' when 'finish' then '<font color = red>审批完成</font>' when 'finished' then '已执行' end) FState "
                //+ ",(select U.FLevelName from c2c_fmdb.t_check_user U where U.FCheckType=A.FCheckType and U.FLevel=A.FCheckLevel) FCheckLevel,"
                //+ "(case A.FCurrLevel when A.FCheckLevel then '审批已结束' else (select CONCAT(U.FLevelName,':',U.FUserID) from c2c_fmdb.t_check_user U where U.FCheckType=A.FCheckType and U.FLevel= A.FCurrLevel+1) end) FCurrLevel,"
                //+ " ,(case A.FCheckResult when 0 then '同意' when 1 then '<font color = red>不同意</font>' when -1 then '正在审批'  end ) FCheckResult, "
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
                        //(case A.fstate when 'start' then '开始' when 'check' then '审批中' when 'finish' then '<font color = red>审批完成</font>' when 'finished' then '已执行' end) FState "
                        if (istate == "0")
                            dr["FState"] = "开始";
                        else if (istate == "1")
                            dr["FState"] = "审批中";
                        else if (istate == "2")
                            dr["FState"] = "<font color = red>审批完成</font>";
                        else if (istate == "3")
                            dr["FState"] = "已执行";
                        //(case A.FCheckResult when 0 then '同意' when 1 then '<font color = red>不同意</font>' when -1 then '正在审批'  end ) FCheckResult, "
                        string iFCheckResult = dr["iFCheckResult"].ToString().Trim();
                        if (iFCheckResult == "0")
                            dr["FCheckResult"] = "同意";
                        else if (iFCheckResult == "1")
                            dr["FCheckResult"] = "<font color = red>不同意</font>";
                        else if (iFCheckResult == "-1")
                            dr["FCheckResult"] = "正在审批";

                        //+ ",(select U.FLevelName from c2c_fmdb.t_check_user U where U.FCheckType=A.FCheckType and U.FLevel=A.FCheckLevel) FCheckLevel,"
                        //+ "(case A.FCurrLevel when A.FCheckLevel then '审批已结束' else (select CONCAT(U.FLevelName,':',U.FUserID) from c2c_fmdb.t_check_user U where U.FCheckType=A.FCheckType and U.FLevel= A.FCurrLevel+1) end) FCurrLevel,"
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
                                dr["FCurrLevel"] = "审批已结束";
                                dr["FCheckLevel"] = dt_checktype.Rows[0]["FLevelName"].ToString().Trim();
                            }
                            else
                            {
                                if (dt_checktype.Rows.Count == 2)
                                {
                                    dr["FCurrLevel"] = dt_checktype.Rows[0]["FLevelName"].ToString().Trim()
                                        + "：" + dt_checktype.Rows[0]["FUserID"].ToString().Trim();
                                    dr["FCheckLevel"] = dt_checktype.Rows[1]["FLevelName"].ToString().Trim();
                                }
                                else
                                {
                                    dr["FCurrLevel"] = dt_checktype.Rows[0]["FLevelName"].ToString().Trim()
                                        + "：" + dt_checktype.Rows[0]["FUserID"].ToString().Trim();
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
        /// 已审批，数据绑定处理
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "已审批数据绑定处理")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public DataTable GetFinishCheckData(string strCheckType, string strUserID, int iStartIndex, int iRecordCount, string strFid) //fid是得到审批任务的唯一标识号
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
                //+ ",(case A.fstate when 'start' then '开始' when 'check' then '审批中' when 'finish' then '<font color = red>审批完成</font>' when 'finished' then '已执行'end) FState "
                //+ ",(select U.FLevelName from c2c_fmdb.t_check_user U where U.FCheckType=A.FCheckType and U.FLevel=A.FCheckLevel) FCheckLevel,"
                //+ "(case A.FCurrLevel when A.FCheckLevel then '审批已结束' else (select CONCAT(U.FLevelName,':',U.FUserID) from c2c_fmdb.t_check_user U where U.FCheckType=A.FCheckType and U.FLevel= A.FCurrLevel+1) end) FCurrLevel,"
                //+ " A.FCheckCount,(case A.FCheckResult when 0 then '同意' when 1 then '<font color = red>不同意</font>' when -1 then '正在审批' end ) FCheckResult, "
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
                        //(case A.fstate when 'start' then '开始' when 'check' then '审批中' when 'finish' then '<font color = red>审批完成</font>' when 'finished' then '已执行' end) FState "
                        if (istate == "0")
                            dr["FState"] = "开始";
                        else if (istate == "1")
                            dr["FState"] = "审批中";
                        else if (istate == "2")
                            dr["FState"] = "<font color = red>审批完成</font>";
                        else if (istate == "3")
                            dr["FState"] = "已执行";
                        else if (istate == "4")
                            dr["FState"] = "已撤消";
                        else if (istate == "9")
                            dr["FState"] = "执行中";
                        //(case A.FCheckResult when 0 then '同意' when 1 then '<font color = red>不同意</font>' when -1 then '正在审批'  end ) FCheckResult, "
                        string iFCheckResult = dr["iFCheckResult"].ToString().Trim();
                        if (iFCheckResult == "0")
                            dr["FCheckResult"] = "同意";
                        else if (iFCheckResult == "1")
                            dr["FCheckResult"] = "<font color = red>不同意</font>";
                        else if (iFCheckResult == "-1")
                            dr["FCheckResult"] = "正在审批";

                        //+ ",(select U.FLevelName from c2c_fmdb.t_check_user U where U.FCheckType=A.FCheckType and U.FLevel=A.FCheckLevel) FCheckLevel,"
                        //+ "(case A.FCurrLevel when A.FCheckLevel then '审批已结束' else (select CONCAT(U.FLevelName,':',U.FUserID) from c2c_fmdb.t_check_user U where U.FCheckType=A.FCheckType and U.FLevel= A.FCurrLevel+1) end) FCurrLevel,"
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
                                dr["FCurrLevel"] = "审批已结束";
                                dr["FCheckLevel"] = dt_checktype.Rows[0]["FLevelName"].ToString().Trim();
                            }
                            else
                            {
                                if (dt_checktype.Rows.Count == 2)
                                {
                                    dr["FCurrLevel"] = dt_checktype.Rows[0]["FLevelName"].ToString().Trim()
                                        + "：" + dt_checktype.Rows[0]["FUserID"].ToString().Trim();
                                    dr["FCheckLevel"] = dt_checktype.Rows[1]["FLevelName"].ToString().Trim();
                                }
                                else
                                {
                                    dr["FCurrLevel"] = dt_checktype.Rows[0]["FLevelName"].ToString().Trim()
                                        + "：" + dt_checktype.Rows[0]["FUserID"].ToString().Trim();
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
        /// 件待执行数量
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "件待执行数量")]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public int GetToDoNum(string strCheckType, string strUserID)
        {
            string strSql = "select count(1) "
                + "from c2c_fmdb.t_check_main A where  A.FStartUser='"
                //+ strUserID + "' and (A.FState='finish') "; //按照审批通过时间排序
                + strUserID + "' and (A.FNewState=2) "; //按照审批通过时间排序

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
        /// 获得审批类型
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "获得审批类型")]
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
