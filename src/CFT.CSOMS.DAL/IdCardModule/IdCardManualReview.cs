﻿using System;
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
                        sb.Append("WHERE Fserial_number IN(");

                        sb.Append("select tb1.Fserial_number from(");
                        sb.Append("SELECT Fserial_number FROM " + tableName + " WHERE Fstate=1 AND Foperator IS NULL ORDER BY Fcreate_time ASC  LIMIT 0," + reviewCount + " ");
                        sb.Append(") tb1");

                        sb.Append(")");
                        LogHelper.LogInfo(string.Format("{0} 用户[{1}]执行查询操作,查询SQL:{2}",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),uid,sb.ToString() ));
                        int updateResult = fmda.ExecSqlNum(sb.ToString());
                        if (updateResult == 0)
                        {
                            message = "领单失败";
                            returnResult = false;
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
                message = "领单失败";
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
        public DataTable LoadReview(string uid, string uin, int reviewStatus, int reviewResult, List<string> yearMonths, string beginDate, string endDate, bool isHaveRightForSeeDetail,string modifyBeginDate,string modifyEndDate,string foperator,int fmemo, int pageSize, int pageNumber, string order, ref int total)
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
                        sb.Append("SELECT Fid,Fname,Fidentitycard,Fimage_path1,Fimage_path2,Fimage_file1,Fimage_file2,Fserial_number,Fspid,Fcreate_time,Fuin,Fmodify_time,Fstate,Fresult, ");
                        sb.Append("Foperator,Fmemo,Fstandby1 AS AgreeRemark,");
                        sb.Append("'" + tableName + "' AS TableName FROM " + tableName + " ");
                        sb.Append("WHERE 1=1 ");
                        if (reviewStatus > 0)
                        {
                            sb.Append("AND Fstate='" + reviewStatus + "' ");
                            if (reviewStatus == 2)//已领单
                            {
                                //当审核状态是已领单时，当前登录人没有【查看详情】的权限时只能查看自己领取的审核单
                                if (!isHaveRightForSeeDetail)
                                {
                                    sb.Append("AND Foperator='" + uid + "' ");
                                }
                            }
                           
                            //
                           
                            //if (reviewStatus > 1)
                            //{
                            //    sb.Append("AND Foperator='" + uid + "' ");
                            //}
                        }
                        if (reviewResult > -1)
                        {
                            sb.Append("AND Fresult='" + reviewResult + "' ");
                        }
                        
                        if (!string.IsNullOrEmpty(uin))
                        {
                            sb.Append("AND Fuin='" + uin + "' ");
                        }
                        if (!string.IsNullOrEmpty(beginDate))
                        {
                            sb.Append(" AND date_format(Fcreate_time, '%Y-%m-%d')>='" + beginDate + "' ");
                        }
                        if (!string.IsNullOrEmpty(endDate))
                        {
                            sb.Append(" AND date_format(Fcreate_time, '%Y-%m-%d')<='" + endDate + "' ");
                        }

                        if (!string.IsNullOrEmpty(modifyBeginDate))
                        {
                            sb.Append(" AND date_format(Fmodify_time, '%Y-%m-%d')>='" + modifyBeginDate + "' ");
                        }
                        if (!string.IsNullOrEmpty(modifyEndDate))
                        {
                            sb.Append(" AND date_format(Fmodify_time, '%Y-%m-%d')<='" + modifyEndDate + "' ");
                        }
                        if (!string.IsNullOrEmpty(foperator))
                        {
                            sb.Append("AND Foperator='" + foperator + "' ");
                        }
                        if (fmemo > 0)
                        {
                            sb.Append("AND Fmemo='" + fmemo + "' ");
                        }

                        if (!string.IsNullOrEmpty(order))
                        {
                            sb.Append("ORDER BY " + order + " ");
                        }
                        
                        dtTotal = fmda.GetTable(sb.ToString());
                        total += (dtTotal == null || dtTotal.Rows.Count < 1) ? 0 : dtTotal.Rows.Count;
                        sb.Append("LIMIT " + startIndex + "," + pageSize + "  ");
                        LogHelper.LogInfo(string.Format("{0} 用户[{1}]执行查询操作,查询SQL:{2}",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),uid,sb.ToString() ));
                        dt = fmda.GetTable(sb.ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                dt = null;
                string message = string.Format("方法LoadReview查询数据库出错:{0}", ex.ToString());
                LogHelper.LogInfo(message);
            }
            finally
            {
                fmda.Commit();
                fmda.CloseConn();
                fmda.Dispose();
            }
            return dt;
        }


        public DataTable LoadReviewForExport(string uid, string uin, int reviewStatus, int reviewResult, List<string> yearMonths, string beginDate, string endDate, string modifyBeginDate,string modifyEndDate,string foperator,int fmemo,string order)
        {
            DataTable dt = new DataTable();
            //total = 0;
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
                    //int startIndex = ((pageNumber - 1) * pageSize);
                    DataTable dtTotal = new DataTable();
                    foreach (var yearMonth in yearMonths)
                    {
                        tableName.Append(yearMonth);
                        StringBuilder sb = new StringBuilder();
                        sb.Append("SELECT Fserial_number AS '流水号',Fspid AS '商户号',Fcreate_time AS '申请时间',Fuin AS '用户帐号',Fmodify_time AS '审核时间' , ");
                        sb.Append("CASE Fstate WHEN 1 THEN '未领单' WHEN 2 THEN '已领单' WHEN 3 THEN '推送到实名系统失败' WHEN 4 THEN '推送成功' END AS '审核状态', ");
                        sb.Append("CASE Fresult WHEN 0 THEN '未处理' WHEN 1 THEN '通过' WHEN 2 THEN '驳回' END AS '审核结果', ");
                        sb.Append("Foperator AS '处理人',  ");
                        sb.Append("CASE Fmemo WHEN 1 THEN '未显示图片' WHEN 2 THEN '上传非身份证照片' WHEN 3 THEN '身份证不清晰不完整' WHEN 4 THEN '身份证证件号不一致' WHEN 5 THEN '其他原因' WHEN 6 THEN '身份证姓名和提供姓名不符' WHEN 7 THEN '身份证签发机关和地址不一致' WHEN 8 THEN '两张均为正面或者反面' WHEN 9 THEN '身份证证件虚假' WHEN 10 THEN '身份证已超过有效期' WHEN 11 THEN '身份证照片非原件' END AS '审核信息'  ");
                        //sb.Append("CASE Fstandby1 WHEN '2' THEN '系统可优化' ELSE '需要人工审核' END AS '通过备注' ");
                        sb.Append("FROM " + tableName + "  ");
                        sb.Append("WHERE 1=1 ");
                        if (reviewStatus > 0)
                        {
                            sb.Append("AND Fstate='" + reviewStatus + "' ");
                            //if (reviewStatus > 1)
                            //{
                            //    sb.Append("AND Foperator='" + uid + "' ");
                            //}
                        }
                        if (reviewResult > -1)
                        {
                            sb.Append("AND Fresult='" + reviewResult + "' ");
                        }
                        
                        if (!string.IsNullOrEmpty(uin))
                        {
                            sb.Append("AND Fuin='" + uin + "' ");
                        }
                        if (!string.IsNullOrEmpty(beginDate))
                        {
                            sb.Append(" AND date_format(Fcreate_time, '%Y-%m-%d')>='" + beginDate + "' ");
                        }
                        if (!string.IsNullOrEmpty(endDate))
                        {
                            sb.Append(" AND date_format(Fcreate_time, '%Y-%m-%d')<='" + endDate + "' ");
                        }

                        //if (!string.IsNullOrEmpty(modifyBeginDate))
                        //{
                        //    sb.Append(" AND date_format(Fmodify_time, '%Y-%m-%d')>='" + modifyBeginDate + "' ");
                        //}
                        //if (!string.IsNullOrEmpty(modifyEndDate))
                        //{
                        //    sb.Append(" AND date_format(Fmodify_time, '%Y-%m-%d')<='" + modifyEndDate + "' ");
                        //}
                        if (!string.IsNullOrEmpty(foperator))
                        {
                            sb.Append("AND Foperator='" + foperator + "' ");
                        }
                        if (fmemo > 0)
                        {
                            sb.Append("AND Fmemo='" + fmemo + "' ");
                        }
                        if (!string.IsNullOrEmpty(order))
                        {
                            sb.Append("ORDER BY " + order + " ");
                        }
                        
                        dtTotal = fmda.GetTable(sb.ToString());
                        //total += dtTotal.Rows.Count;
                        //sb.Append("LIMIT " + startIndex + "," + pageSize + "  ");
                        LogHelper.LogInfo(string.Format("{0} 用户[{1}]执行查询操作,查询SQL:{2}",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),uid,sb.ToString() ));
                        dt = fmda.GetTable(sb.ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                dt = null;
                string message = string.Format("方法LoadReviewForExport查询数据库出错:{0}", ex.ToString());
                LogHelper.LogInfo(message);
            }
            finally
            {
                fmda.Commit();
                fmda.CloseConn();
                fmda.Dispose();
            }
            return dt;
        }
        public DataTable LoadHZReport(string uid, List<string> yearMonths, string modifyBeginDate, string modifyEndDate, string foperator, int pageSize, int pageNumber, string order, ref int total)
        {
            DataTable dt = new DataTable();
            total = 0;
            var fmda = MySQLAccessFactory.GetMySQLAccess("DataSource_ht");
            try
            {
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
                        sb.Append("SELECT TB1.时间,  ");
                        sb.Append("0 AS '总工单量',  ");
                        sb.Append("0 AS '待审核总量', ");
                        sb.Append("SUM(TB1.进单量) AS '进单量',  ");
                        sb.Append("SUM(TB1.已处理量) AS '已处理量',  ");
                        sb.Append("SUM(TB1.审核通过量) AS '审核通过量',  ");
                        sb.Append("CAST((SUM(TB1.审核通过量)*100/SUM(TB1.已处理量)) AS DECIMAL(5,2)) AS '审核通过率',  ");
                        sb.Append("SUM(TB1.审核拒绝量) AS '审核拒绝量',  ");
                        sb.Append("CAST((SUM(TB1.审核拒绝量)*100/SUM(TB1.已处理量)) AS DECIMAL(5,2))  AS '审核拒绝率'  ");
                        sb.Append("FROM (  ");
                        sb.Append("SELECT   ");
                        sb.Append("(date_format(Fmodify_time, '%Y-%m-%d')) AS '时间',	  ");
                        sb.Append("0 AS '进单量',		  ");
                        sb.Append("SUM(CASE WHEN Fresult=1 THEN 1 ELSE 0 END )   AS '审核通过量',  ");
                        sb.Append("SUM(CASE WHEN Fresult=2 THEN 1 ELSE 0 END)   AS '审核拒绝量',  ");
                        sb.Append("COUNT(Fid) AS '已处理量'  ");
                        sb.Append("FROM " + tableName + "  ");
                        sb.Append("WHERE 1=1 AND Fresult!=0  ");
                        if (!string.IsNullOrEmpty(modifyBeginDate))
                        {
                            sb.Append(" AND date_format(Fmodify_time, '%Y-%m-%d')>='" + modifyBeginDate + "' ");
                        }
                        if (!string.IsNullOrEmpty(modifyEndDate))
                        {
                            sb.Append(" AND date_format(Fmodify_time, '%Y-%m-%d')<='" + modifyEndDate + "' ");
                        }
                        sb.Append("GROUP BY (date_format(Fmodify_time, '%Y-%m-%d'))  ");
                        sb.Append(" UNION  ");
                        sb.Append("SELECT   ");
                        sb.Append("(date_format(Fcreate_time, '%Y-%m-%d')) AS '时间',  ");
                        sb.Append("COUNT(Fserial_number) AS '进单量',		  ");
                        sb.Append("0 AS '审核通过量',  ");
                        sb.Append("0 AS '审核拒绝量',  ");
                        sb.Append("0 AS '已处理量'  ");
                        sb.Append("FROM " + tableName + "  ");
                        sb.Append("WHERE 1=1   ");
                        if (!string.IsNullOrEmpty(modifyBeginDate))
                        {
                            sb.Append(" AND date_format(Fcreate_time, '%Y-%m-%d')>='" + modifyBeginDate + "' ");
                        }
                        if (!string.IsNullOrEmpty(modifyEndDate))
                        {
                            sb.Append(" AND date_format(Fcreate_time, '%Y-%m-%d')<='" + modifyEndDate + "' ");
                        }
                        sb.Append("GROUP BY (date_format(Fcreate_time, '%Y-%m-%d'))	  ");
                        sb.Append(") AS TB1 GROUP BY TB1.`时间`  ");
                        sb.Append("ORDER BY TB1.`时间` ");

                        dtTotal = fmda.GetTable(sb.ToString());
                        total += (dtTotal == null || dtTotal.Rows.Count < 1) ? 0 : dtTotal.Rows.Count;
                        LogHelper.LogInfo("返回table行数:" + total.ToString());
                        sb.Append("LIMIT " + startIndex + "," + pageSize + "  ");
                        LogHelper.LogInfo(string.Format("{0} 用户[{1}]执行查询操作,查询SQL:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), uid, sb.ToString()));
                        dt = fmda.GetTable(sb.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                dt = null;
                string message = string.Format("方法LoadHZReport查询数据库出错:{0}", ex.ToString());
                LogHelper.LogInfo(message);
            }
            finally
            {
                fmda.Commit();
                fmda.CloseConn();
                fmda.Dispose();
            }
            return dt;
        }
        public DataTable LoadPersonalReviewReport(string uid, List<string> yearMonths, string modifyBeginDate, string modifyEndDate, string foperator, int pageSize, int pageNumber, string order, ref int total)
        {
            DataTable dt = new DataTable();
            total = 0;
            var fmda = MySQLAccessFactory.GetMySQLAccess("DataSource_ht");
            try
            {
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
                        sb.Append("SELECT TB1.`处理人`, ");
                        sb.Append("TB1.审核时间,  ");
                        sb.Append("SUM(TB1.通过) AS '通过', ");
                        sb.Append("SUM(TB1.拒绝) AS '拒绝',");
                        sb.Append("TB1.当天第一单处理时间,");
                        sb.Append("TB1.当天最后一单处理时间, ");
                        sb.Append("SUM(TB1.汇总) AS '汇总'  ");
                        sb.Append("FROM (");
                        sb.Append("SELECT Foperator AS '处理人', ");
                        sb.Append("(date_format(Fmodify_time, '%Y-%m-%d')) AS '审核时间',   ");
                        sb.Append("SUM(CASE WHEN Fresult=1 THEN 1 ELSE 0 END) AS '通过', ");
                        sb.Append("SUM(CASE WHEN Fresult=2 THEN 1 ELSE 0 END) AS '拒绝', ");
                        sb.Append("MIN(Fmodify_time) AS '当天第一单处理时间', ");
                        sb.Append("MAX(Fmodify_time) AS '当天最后一单处理时间',  ");
                        sb.Append("COUNT(Fid) AS '汇总' ");
                        sb.Append("FROM " + tableName + "  ");
                        sb.Append("WHERE 1=1 AND Fresult!=0 ");
                        if (!string.IsNullOrEmpty(modifyBeginDate))
                        {
                            sb.Append(" AND date_format(Fmodify_time, '%Y-%m-%d')>='" + modifyBeginDate + "' ");
                        }
                        if (!string.IsNullOrEmpty(modifyEndDate))
                        {
                            sb.Append(" AND date_format(Fmodify_time, '%Y-%m-%d')<='" + modifyEndDate + "' ");
                        }
                        if (!string.IsNullOrEmpty(foperator))
                        {
                            sb.Append("AND Foperator='" + foperator + "' ");
                        }
                        sb.Append("GROUP BY Foperator,(date_format(Fmodify_time, '%Y-%m-%d')) ");
                        sb.Append(") AS TB1 GROUP BY TB1.`处理人`,TB1.`审核时间` ");
                        sb.Append("ORDER BY TB1.`处理人` ASC,TB1.`审核时间` ASC ");

                        dtTotal = fmda.GetTable(sb.ToString());
                        
                        total += (dtTotal == null || dtTotal.Rows.Count < 1) ? 0 : dtTotal.Rows.Count;
                        LogHelper.LogInfo("返回table行数:" + total.ToString());
                        sb.Append("LIMIT " + startIndex + "," + pageSize + "  ");
                        LogHelper.LogInfo(string.Format("{0} 用户[{1}]执行查询操作,查询SQL:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), uid, sb.ToString()));
                        dt = fmda.GetTable(sb.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                dt = null;
                string message = string.Format("方法LoadPersonalReviewReport查询数据库出错:{0}", ex.ToString());
                LogHelper.LogInfo(message);
            }
            finally
            {
                fmda.Commit();
                fmda.CloseConn();
                fmda.Dispose();
            }
            return dt;
        }
        public DataTable LoadFailReasonReport(string uid, List<string> yearMonths, string modifyBeginDate, string modifyEndDate, string foperator, int pageSize, int pageNumber, string order, ref int total)
        {
            DataTable dt = new DataTable();
            total = 0;
            var fmda = MySQLAccessFactory.GetMySQLAccess("DataSource_ht");
            try
            {
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
                        sb.Append("SELECT TB1.审核时间,  ");
                        sb.Append("SUM(TB1.未显示图片) AS '未显示图片',  ");
                        sb.Append("SUM(TB1.上传非身份证照片) AS '上传非身份证照片',  ");
                        sb.Append("SUM(TB1.身份证不清晰不完整) AS '身份证不清晰不完整',  ");
                        sb.Append("SUM(TB1.身份证证件号不一致) AS '身份证证件号不一致',  ");
                        sb.Append("SUM(TB1.其他原因) AS '其他原因',  ");
                        sb.Append("SUM(TB1.身份证姓名和提供姓名不符) AS '身份证姓名和提供姓名不符',  ");
                        sb.Append("SUM(TB1.身份证签发机关和地址不一致) AS '身份证签发机关和地址不一致',  ");
                        sb.Append("SUM(TB1.两张均为正面或者反面) AS '两张均为正面或者反面',  ");
                        sb.Append("SUM(TB1.身份证证件虚假) AS '身份证证件虚假',  ");
                        sb.Append("SUM(TB1.身份证已超过有效期) AS '身份证已超过有效期',  ");
                        sb.Append("SUM(TB1.身份证照片非原件) AS '身份证照片非原件',  ");
                        sb.Append("SUM(TB1.汇总) AS '汇总'  ");
                        sb.Append("FROM (");
                        sb.Append("SELECT (date_format(Fmodify_time, '%Y-%m-%d')) AS '审核时间',  ");
                        sb.Append("SUM(CASE WHEN Fmemo='1' THEN 1 ELSE 0 END) AS '未显示图片',  ");
                        sb.Append("SUM(CASE WHEN Fmemo='2' THEN 1 ELSE 0 END) AS '上传非身份证照片',  ");
                        sb.Append("SUM(CASE WHEN Fmemo='3' THEN 1 ELSE 0 END) AS '身份证不清晰不完整',  ");
                        sb.Append("SUM(CASE WHEN Fmemo='4' THEN 1 ELSE 0 END) AS '身份证证件号不一致',  ");
                        sb.Append("SUM(CASE WHEN Fmemo='5' THEN 1 ELSE 0 END) AS '其他原因',  ");
                        sb.Append("SUM(CASE WHEN Fmemo='6' THEN 1 ELSE 0 END) AS '身份证姓名和提供姓名不符',  ");
                        sb.Append("SUM(CASE WHEN Fmemo='7' THEN 1 ELSE 0 END) AS '身份证签发机关和地址不一致',  ");
                        sb.Append("SUM(CASE WHEN Fmemo='8' THEN 1 ELSE 0 END) AS '两张均为正面或者反面',  ");
                        sb.Append("SUM(CASE WHEN Fmemo='9' THEN 1 ELSE 0 END) AS '身份证证件虚假',  ");
                        sb.Append("SUM(CASE WHEN Fmemo='10' THEN 1 ELSE 0 END) AS '身份证已超过有效期',  ");
                        sb.Append("SUM(CASE WHEN Fmemo='11' THEN 1 ELSE 0 END) AS '身份证照片非原件',  ");
                        sb.Append("COUNT(Fid) AS '汇总' ");
                        sb.Append("FROM " + tableName + "  ");
                        sb.Append("WHERE 1=1 ");
                        if (!string.IsNullOrEmpty(modifyBeginDate))
                        {
                            sb.Append(" AND date_format(Fmodify_time, '%Y-%m-%d')>='" + modifyBeginDate + "' ");
                        }
                        if (!string.IsNullOrEmpty(modifyEndDate))
                        {
                            sb.Append(" AND date_format(Fmodify_time, '%Y-%m-%d')<='" + modifyEndDate + "' ");
                        }
                        sb.Append("GROUP BY (date_format(Fmodify_time, '%Y-%m-%d')) ");
                        sb.Append(") AS TB1 GROUP BY TB1.`审核时间` ");

                        dtTotal = fmda.GetTable(sb.ToString());
                        total += (dtTotal == null || dtTotal.Rows.Count < 1) ? 0 : dtTotal.Rows.Count;
                        LogHelper.LogInfo("返回table行数:" + total.ToString());
                        sb.Append("LIMIT " + startIndex + "," + pageSize + "  ");
                        LogHelper.LogInfo(string.Format("{0} 用户[{1}]执行查询操作,查询SQL:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), uid, sb.ToString()));
                        dt = fmda.GetTable(sb.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                dt = null;
                string message = string.Format("方法LoadFailReasonReport查询数据库出错:{0}", ex.ToString());
                LogHelper.LogInfo(message);
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

        public DataTable LoadAllReview(string tableName,string endDate)
        {
            DataTable dt = new DataTable();
            var fmda = MySQLAccessFactory.GetMySQLAccess("DataSource_ht");
            try
            {                  
                fmda.OpenConn();
                fmda.StartTran();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COUNT(Fid) AS Total FROM " + tableName + " ");
                sb.Append("WHERE (date_format(Fcreate_time, '%Y-%m-%d'))<='" + endDate + "' ");
                sb.Append("LIMIT 0,1");
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
        public DataTable LoadWaitReview(string tableName, string endDate)
        {
            DataTable dt = new DataTable();
            var fmda = MySQLAccessFactory.GetMySQLAccess("DataSource_ht");
            try
            {                  
                fmda.OpenConn();
                fmda.StartTran();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COUNT(Fid) AS Total FROM " + tableName + " ");
                sb.Append("WHERE Fresult=0 AND  (date_format(Fcreate_time, '%Y-%m-%d'))<='" + endDate + "' ");
                sb.Append("LIMIT 0,1");
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

        public bool Update(string fserial_numbe, int fid, int fresult, string memo, string tableName, string foperator, int agreeRemark, out string message)
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
                if (fresult == 1)//通过
                {
                    sb.Append("Fstandby1=" + agreeRemark + ", ");
                }
                else if (fresult == 2)//驳回
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
                message = "保存失败";
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

        public bool UpdateFstate(string fserial_numbe, int fid, int fstate,string tableName)
        {
            DataSet ds = new DataSet();
            bool returnResult = false;
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
                    returnResult = false;
                }
                else
                {
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

        public bool Review(string reqStr,out string message)
        {
            bool result = true;
            StringBuilder sb_cgi = new StringBuilder();
            string errorMsg = "";
            message = string.Empty;            
            //10.49.130.221
            //10.49.130.211
            //10.49.130.133
            string kf_auth_ocr_audit_QueryUrl = System.Configuration.ConfigurationManager.AppSettings["kf_auth_ocr_audit_QueryUrl"].ToString();
            //LogHelper.LogInfo(string.Format("IdCardManualReview.Review(string reqStr),kf_auth_ocr_audit_QueryUrl:{0}", kf_auth_ocr_audit_QueryUrl));
            sb_cgi.Append(kf_auth_ocr_audit_QueryUrl);
            sb_cgi.Append(reqStr);
            string logMessage = string.Format("IdCardManualReview.Review,请求cji地址:{0}", sb_cgi.ToString());
            LogHelper.LogInfo(logMessage);
            
            try
            {
                message = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetFromCGI(sb_cgi.ToString(), "", out errorMsg);
                if (string.IsNullOrEmpty(message)||!string.IsNullOrEmpty(errorMsg))
                {
                    LogHelper.LogInfo("IdCardManualReview.Review,message = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetFromCGI,错误信息:" + errorMsg);
                    result = false;
                    message = errorMsg;
                }
            }
            catch (Exception err)
            {
                LogHelper.LogInfo(string.Format("IdCardManualReview.Review,请求cji地址:{0},错误信息:{1}", sb_cgi.ToString(), err.Message));
                result = false;
                message = err.Message.ToString();
                //throw new Exception(string.Format("OCR客服审核接口:{0}", err.Message));
            }
            return result;
        }



        public string ReviewByRelay(string reqStr)
        {
            string result = string.Empty;
            try
            {
                string relayip = System.Configuration.ConfigurationManager.AppSettings["kf_auth_ocr_audit_IP"];
                int relayport = int.Parse(System.Configuration.ConfigurationManager.AppSettings["kf_auth_ocr_audit_Por"]);
                string requesttpe = System.Configuration.ConfigurationManager.AppSettings["kf_auth_ocr_audit_request_type"];
                result = RelayAccessFactory.RelayInvoke(reqStr, requesttpe, false, false, relayip, relayport, "");
            }
            catch (Exception err)
            {                
                result = string.Empty;
                LogHelper.LogInfo(string.Format("IdCardManualReview类ReviewByRelay方法调用出错,参数reqStr={0},错误信息:{3}", reqStr, err.Message));
            }
            return result;
        }
    }
}
