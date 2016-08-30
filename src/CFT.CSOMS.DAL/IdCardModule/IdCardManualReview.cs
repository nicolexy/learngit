using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CFT.Apollo.Logging;
using CFT.CSOMS.DAL.Infrastructure;

namespace CFT.CSOMS.DAL.IdCardModule
{
    /// <summary>
    /// 身份证影印件客服人工审核
    /// </summary>
    public class IdCardManualReview
    {

        /// <summary>
        /// 客服领取需要人工审核的单据
        /// </summary>
        /// <param name="uid">领单人</param>
        /// <param name="uin">用户帐号</param>
        /// <param name="yearMonths">年月</param>
        /// <returns></returns>
        public bool ReceiveNeedReviewIdCardData(string uid, string uin, List<string> yearMonths, out string message)
        {

            bool receiveResult = false;
            message = string.Empty;
            var fmda = MySQLAccessFactory.GetMySQLAccess("DataSource_ht");
            try
            {
                if (!string.IsNullOrEmpty(uin))
                {
                    //通过帐号分配任务                    
                    fmda.OpenConn();
                    fmda.StartTran();

                    if (yearMonths.Count > 0)
                    {
                        StringBuilder tableName = new StringBuilder();
                        tableName.Append("c2c_fmdb.t_check_identitycard_");
                        foreach (var yearMonth in yearMonths)
                        {
                            tableName.Append(yearMonth);
                            StringBuilder sb = new StringBuilder();
                            sb.AppendFormat("SELECT * FROM {0} WHERE Fuin={1} ", tableName, uin);
                            DataTable dt = new DataTable();
                            dt = fmda.GetTable(sb.ToString());
                            if (dt == null)
                            {
                                message = string.Format("用户帐号：{0}不存在", uin);
                            }
                            else
                            {
                                StringBuilder sb2 = new StringBuilder();
                                sb2.AppendFormat("SELECT * FROM {0} WHERE Fuin={1} AND Fstate>{2} AND Foperator IS NOT NULL ", tableName, uin, 1);
                                DataTable dt2 = new DataTable();
                                dt2 = fmda.GetTable(sb2.ToString());
                                if (dt2 != null && dt2.Rows.Count > 0)
                                {
                                    message = string.Format("用户帐号：{0}已被其他客服领单", uin);
                                }
                                else
                                {
                                    StringBuilder sb_Update = new StringBuilder();
                                    sb_Update.AppendFormat("UPDATE {0} SET Foperator='{1}',Fstate={2} WHERE Fuin='{3}'", tableName, uid, 2, uin);
                                    bool updateResult = fmda.ExecSql(sb_Update.ToString());
                                    if (updateResult)
                                    {
                                        message = string.Format("用户帐号：{0}领单成功", uid);
                                        receiveResult = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                receiveResult = false;
                LogHelper.LogInfo("IdCardManualReview.ReceiveNeedReviewIdCardData:" + ex.Message);
            }
            finally
            {
                fmda.Commit();
                fmda.CloseConn();
                fmda.Dispose();
            }
            return receiveResult;
        }

        /// <summary>
        ///  客服领取需要人工审核的单据
        /// </summary>
        /// <param name="uid">领单人</param>
        /// <param name="reviewCount">领单数量</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public bool ReceiveNeedReviewIdCardData2(string uid, int reviewCount, List<string> yearMonths, out string message)
        {
            DataSet ds = new DataSet();
            bool returnResult = false;
            message = string.Empty;
            var fmda = MySQLAccessFactory.GetMySQLAccess("DataSource_ht");
            try
            {
                fmda.OpenConn();
                fmda.StartTran();
                StringBuilder sb = new StringBuilder();
                if (yearMonths.Count > 0)
                {
                    StringBuilder tableName = new StringBuilder();
                    tableName.Append("c2c_fmdb.t_check_identitycard_");
                    foreach (var yearMonth in yearMonths)
                    {
                        tableName.Append(yearMonth);
                        sb.Append("UPDATE " + tableName + " ");
                        sb.Append("SET Foperator='" + uid + "',");
                        sb.Append("Fstate=2 ");
                        sb.Append("WHERE Fidentitycard IN(");

                        sb.Append("select tb1.Fidentitycard from(");
                        sb.Append("SELECT Fidentitycard FROM " + tableName + " WHERE Fstate=1 AND Foperator IS NULL ORDER BY Fcreate_time ASC  LIMIT 0," + reviewCount + " ");
                        sb.Append(") tb1");

                        sb.Append(")");
                        int updateResult = fmda.ExecSqlNum(sb.ToString());
                        if (updateResult == 0)
                        {
                            message = "领单失败";
                        }
                        else
                        {
                            message = string.Format("领单成功,本次领取了{0}单", updateResult);
                            returnResult = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnResult = false;
                LogHelper.LogInfo("IdCardManualReview.ReceiveNeedReviewIdCardData2:" + ex.Message);
            }
            finally
            {
                fmda.Commit();
                fmda.CloseConn();
                fmda.Dispose();
            }
            return returnResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="uin"></param>
        /// <param name="reviewStatus"></param>
        /// <param name="reviewResult"></param>
        /// <param name="yearMonths"></param>
        /// <returns></returns>
        public DataTable LoadReview(string uid, string uin, int reviewStatus, int reviewResult, List<string> yearMonths, int pageSize, int pageNumber, string order, ref int total)
        {
            DataTable dt = new DataTable();
            total = 0;
            var fmda = MySQLAccessFactory.GetMySQLAccess("DataSource_ht");
            try
            {
                //通过帐号分配任务                    
                fmda.OpenConn();
                fmda.StartTran();

                if (yearMonths.Count > 0)
                {
                    StringBuilder tableName = new StringBuilder();
                    tableName.Append("c2c_fmdb.t_check_identitycard_");
                    int startIndex = ((pageNumber - 1) * pageSize);
                    DataTable dtTotal = new DataTable();
                    foreach (var yearMonth in yearMonths)
                    {
                        tableName.Append(yearMonth);
                        StringBuilder sb = new StringBuilder();
                        sb.Append("SELECT *,'" + tableName + "' AS TableName FROM " + tableName + " ");
                        sb.Append("WHERE 1=1 ");
                        if (reviewStatus > 0)
                        {
                            sb.Append("AND Fstate='" + reviewStatus + "' ");
                            if (reviewStatus > 1)
                            {
                                sb.Append("AND Foperator='" + uid + "' ");
                            }
                        }
                        if (reviewResult > -1)
                        {
                            sb.Append("AND Fresult='" + reviewResult + "' ");
                        }
                        
                        if (!string.IsNullOrEmpty(uin))
                        {
                            sb.Append("AND Fuin='" + uin + "' ");
                        }
                        if (!string.IsNullOrEmpty(order))
                        {
                            sb.Append("ORDER BY " + order + " ");
                        }
                        dtTotal = fmda.GetTable(sb.ToString());
                        total += dtTotal.Rows.Count;

                        sb.Append("LIMIT " + startIndex + "," + pageSize + "  ");
                        dt = fmda.GetTable(sb.ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                dt = null;
            }
            finally
            {
                fmda.Commit();
                fmda.CloseConn();
                fmda.Dispose();
            }
            return dt;
        }

        /// <summary>
        /// 加载审核信息
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="fserial_numbe"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable LoadReview(int fid, string fserial_numbe, string tableName)
        {
            DataTable dt = new DataTable();
            var fmda = MySQLAccessFactory.GetMySQLAccess("DataSource_ht");
            try
            {
                //通过帐号分配任务                    
                fmda.OpenConn();
                fmda.StartTran();

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM " + tableName + " ");
                sb.Append("WHERE Fserial_number='" + fserial_numbe + "' ");
                sb.Append("AND Fid=" + fid + " ");
                sb.Append("AND Foperator IS NOT NULL  ");
                sb.Append("AND Fresult>0  ");
                sb.Append("LIMIT 0,10");
                dt = fmda.GetTable(sb.ToString());
            }
            catch (Exception ex)
            {
                dt = null;
            }
            finally
            {
                fmda.Commit();
                fmda.CloseConn();
                fmda.Dispose();
            }
            return dt;
        }
        

        public bool Update(string fserial_numbe, int fid, int fresult, string memo, string tableName, string foperator, out string message)
        {
            DataSet ds = new DataSet();
            bool returnResult = false;
            message = string.Empty;
            var fmda = MySQLAccessFactory.GetMySQLAccess("DataSource_ht");
            try
            {
                fmda.OpenConn();
                fmda.StartTran();
                StringBuilder sb = new StringBuilder();
                sb.Append("UPDATE " + tableName + " ");
                sb.Append("SET Fresult=" + fresult + ",");
                if (fresult == 2)
                {
                sb.Append("Fmemo='" + memo + "', ");
                }
                sb.Append("Foperator='" + foperator + "', ");
                sb.Append("Fmodify_time=sysdate() ");
                sb.Append("WHERE Fid=" + fid + " AND Fserial_number='" + fserial_numbe + "' ");
                int updateResult = fmda.ExecSqlNum(sb.ToString());
                if (updateResult == 0)
                {
                    message = "保存失败";
                }
                else
                {
                    message = string.Format("保存成功", updateResult);
                    returnResult = true;
                }
            }
            catch (Exception ex)
            {
                returnResult = false;
                LogHelper.LogInfo("IdCardManualReview.Update:" + ex.Message);
            }
            finally
            {
                fmda.Commit();
                fmda.CloseConn();
                fmda.Dispose();
            }
            return returnResult;
        }

        public bool UpdateFstate(string fserial_numbe, int fid, int fstate,string tableName, out string message)
        {
            DataSet ds = new DataSet();
            bool returnResult = false;
            message = string.Empty;
            var fmda = MySQLAccessFactory.GetMySQLAccess("DataSource_ht");
            try
            {
                fmda.OpenConn();
                fmda.StartTran();
                StringBuilder sb = new StringBuilder();
                sb.Append("UPDATE " + tableName + " ");
                sb.Append("SET Fstate=" + fstate + " ");                
                sb.Append("WHERE Fid=" + fid + " AND Fserial_number='" + fserial_numbe + "' ");
                int updateResult = fmda.ExecSqlNum(sb.ToString());
                if (updateResult == 0)
                {
                    message = "保存失败";
                }
                else
                {
                    message = string.Format("保存成功", updateResult);
                    returnResult = true;
                }
            }
            catch (Exception ex)
            {
                returnResult = false;
                LogHelper.LogInfo("IdCardManualReview.UpdateFstate:" + ex.Message);
            }
            finally
            {
                fmda.Commit();
                fmda.CloseConn();
                fmda.Dispose();
            }
            return returnResult;
        }

        public string Review(string reqStr)
        {

            StringBuilder sb_cgi = new StringBuilder();
            string msg = "";
            string res = string.Empty;
            //cgi = System.Configuration.ConfigurationManager.AppSettings["UserControlFinCgi"].ToString();
            sb_cgi.Append("http://10.123.7.25/finance_pay/kf_auth_ocr_audit.fcgi");//测试 cgi
            sb_cgi.Append("?");
            sb_cgi.Append(reqStr);
            // 测试 cgi = "http://check.cf.com/cgi-bin/v1.0/BauClrBan.cgi?uid=400061433&type=1009&sum=2850&opera=1100000000";
            // LogHelper.LogInfo("RemoveUserControlFin send req:" + cgi);                     
            try
            {
                res = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetFromCGI(sb_cgi.ToString(), "", out msg);
                if (msg != "")
                {
                    throw new Exception(msg);
                }
            }
            catch (Exception err)
            {
                LogHelper.LogInfo("IdCardManualReview.Review:" + err.Message);
                throw new Exception(string.Format("OCR客服审核接口:{0}", err.Message));
            }
            return res;
        }
    }
}
