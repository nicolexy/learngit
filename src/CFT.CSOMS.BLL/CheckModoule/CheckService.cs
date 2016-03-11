

using System;
using System.Collections.Generic;
using System.Data;
using CFT.CSOMS.COMMLIB;
using CFT.CSOMS.DAL.Infrastructure;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.C2C.Finance.BankLib;
using CFT.CSOMS.BLL.CheckModoule;
namespace CFT.CSOMS.DAL.CheckModoule
{
    public class CheckService
    {
        //指数基金赎回类型：fRate_type
        public string DicfRate_type(string key)
        {
            Dictionary<string, string> dicfRate_type = new Dictionary<string, string>() 
            {   
                {"1238657101","T+2到账"},
                {"1241176901","T+2到账"},

                {"1239537001","T+3到账"},
                {"1249279401","T+3到账"},
                {"1249643101","T+3到账"},

                {"1250802101","T+7到账"}
           };
            if (dicfRate_type.ContainsKey(key))
                return dicfRate_type[key];
            return "t+0";
        }

        public DataTable GetCheckInfo(string objid, string checkType)
        {
            try
            {
                DataTable dt = new CheckData().GetCheckInfo(objid, checkType);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataTable dtR = new DataTable();
                    string col_name, col_value;
                    List<String> row = new List<String>();
                    string sql = "select ";
                    foreach (DataRow dr in dt.Rows)
                    {
                        col_name = dr["FKey"].ToString().Trim();
                        dtR.Columns.Add(col_name.ToLower());//列名
                        col_value = dr["FValue"].ToString().Trim();

                        sql += "'" + col_value + "' as " + col_name + " ,";
                    }
                    sql = sql.Substring(0, sql.Length - 1);
                    dtR = PublicRes.returnDSAll(sql, "ht_DB").Tables[0];
                    if (dtR != null && dtR.Rows.Count > 0)
                    {
                        dtR.Columns.Add("fRate_type_Str", typeof(string));
                        foreach (DataRow dr in dtR.Rows) 
                        {
                            dr["fRate_type_Str"] = DicfRate_type(dr["spid"].ToString().Trim());
                        }
                    }
                    return dtR;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception("service获取审批数据异常：" + ex.Message);
            }
        }


        /// <summary>
        /// 开始一个审批
        /// </summary>
        /// <param name="strMainID">操作关键字ID</param>
        /// <param name="strCheckType">审批类型</param>
        /// <param name="strMemo">详细描述</param>
        /// <param name="strLevelValue">传入的审批级别判断值</param>
        /// <param name="myParams">需要的参数列表</param>   
        public void StartCheck(string strMainID, string strCheckType, string strMemo, string strLevelValue, string user, string ip, Param[] myParams)
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
                        ExecuteCheck(dt.Rows[0]["Fid"].ToString().Trim(), user, ip);
                    else
                        return;
                }
            }
            string Msg = null;

            try
            {
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
                        throw new Exception("执行条件的检查失败！" + Msg);
                    }
                }

                Check_Class.CreateCheck(user, strMainID, strCheckType, strMemo, strLevelValue, myParams, user, ip);

                string strMsg = "用户" + user + "在" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "时间对ID为" + strMainID
                    + "的记录进行了" + strCheckType + "的审批，执行结果成功！";

                log4net.LogManager.GetLogger(strMsg);
            }
            catch (Exception err)
            {
                string strMsg = "用户" + user + "在" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "时间对ID为" + strMainID
                    + "的记录进行了" + strCheckType + "的审批，执行结果失败！" + err.Message;

                log4net.LogManager.GetLogger(strMsg);
                throw new Exception("Service处理失败！");
            }
        }

        /// <summary>
        /// 执行审批完成后的动作
        /// </summary>
        /// <param name="strCheckID">审批ID</param>
        /// <returns>是否成功</returns>    
        public bool ExecuteCheck(string strCheckID, string user, string ip)
        {
            try
            {
                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
                try
                {
                    da.OpenConn();
                    return FinishCheck.ExecuteCheck(strCheckID, user, ip, da);
                }
                finally
                {
                    da.Dispose();
                }
            }
            catch (Exception err)
            {
                throw new Exception("失败！" + err.Message.ToString().Replace("'", "’"));
            }
        }

    }
}
