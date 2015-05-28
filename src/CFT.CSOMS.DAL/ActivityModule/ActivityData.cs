using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.Apollo.Logging;


namespace CFT.CSOMS.DAL.ActivityModule
{
    public class ActivityData
    {
        //乐刷卡活动
        public DataSet QueryLeShuaKaActivity(string uin, string stime, string etime, int start, int count)
        {
            if (string.IsNullOrEmpty(uin))
            {
                throw new ArgumentNullException("必填参数：账号为空！");
            }

            int len = uin.Length;
            string tableName = "db_action_happy_center_" + uin.Substring(len - 2) + ".t_action_pri_" + uin.Substring(len - 3, 1);

            StringBuilder Sql = new StringBuilder("select * from " + tableName);
            Sql.Append(" where FUin=" + uin);
            if (!string.IsNullOrEmpty(stime))
            {
                Sql.Append(" AND FCreateTime >='" + stime + "'");
            }
            if (!string.IsNullOrEmpty(etime))
            {
                Sql.Append(" AND FCreateTime <='" + etime + "'");
            }
            Sql.Append(" limit " + start + "," + count);

            using (var da = MySQLAccessFactory.GetMySQLAccess("LSKActivity"))
            {
                da.OpenConn();

                DataSet ds = da.dsGetTotalData(Sql.ToString());

                return ds;
            }
        }

        //理财通活动
        public DataSet QueryLCTActivity(string payUin, string stime, string etime, int start, int count)
        {
            if (string.IsNullOrEmpty(payUin))
            {
                throw new ArgumentNullException("必填参数：账号为空！");
            }

            //先查询理财通活动表
            string act_str = "";
            string qrySql = "select Fact_id from c2c_fmdb.t_activity_kf where Fstate=2 ";
            using (var da = MySQLAccessFactory.GetMySQLAccess("RefundRegister"))
            {
                da.OpenConn();

                DataTable dt = da.GetTable(qrySql);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new Exception("请先添加活动！");
                }
                foreach (DataRow dr in dt.Rows)
                {
                    act_str += "'" + dr["Fact_id"].ToString() + "',";
                }
                act_str = act_str.Substring(0, act_str.Length - 1);
            }
            if (act_str == "")
            {
                throw new Exception("请先添加活动！");
            }

            string tableId = string.Empty;
            if (payUin.IndexOf("@wx.tenpay.com") > 0)
            {
                var uin = payUin.Substring(0, payUin.IndexOf("@wx.tenpay.com"));
                tableId = uin.Substring(uin.Length - 2, 2);
            }
            else
            {
                tableId = payUin.Substring(payUin.Length - 2, 2);
            }

            LogHelper.LogInfo("微信支付账号" + payUin + "获取tableid:" + tableId);
            if (string.IsNullOrEmpty(tableId))
            {
                throw new ArgumentNullException("tableId为空！");
            }
            if (tableId.Length == 1)
            {
                tableId = "0" + tableId;
            }

            string tableName1 = "db_action_licaitong.t_action_trans_" + tableId;
            string tableName2 = "db_action_licaitong.t_action_prize_" + tableId;

            StringBuilder Sql = new StringBuilder("");

            Sql.Append("SELECT FActionId,FUin,FPayUin,FSPID,FTransId,FStatus,FMoney,FExpiredTime,FStandby2,FActType from " + tableName1 + " where FActionId IN(" + act_str + ") AND FPayUin='");
            Sql.Append(payUin + "'");

            if (!string.IsNullOrEmpty(stime))
            {
                Sql.Append(" AND FCreateTime >='" + stime + "'");
            }
            if (!string.IsNullOrEmpty(etime))
            {
                Sql.Append(" AND FCreateTime <='" + etime + "'");
            }

            Sql.Append(" limit " + start + "," + count);

            using (var da = MySQLAccessFactory.GetMySQLAccess("LCTActivity"))
            {
                da.OpenConn();

                DataSet ds = da.dsGetTotalData(Sql.ToString());

                return ds;
            }
        }
        //属于理财通活动的中奖记录查询
        public DataSet QueryLCTPrize(string payUin, string actId)
        {
            string tableId = string.Empty;
            if (payUin.IndexOf("@wx.tenpay.com") > 0)
            {
                var uin = payUin.Substring(0, payUin.IndexOf("@wx.tenpay.com"));
                tableId = uin.Substring(uin.Length - 2, 2);
            }
            else
            {
                tableId = payUin.Substring(payUin.Length - 2, 2);
            }
            if (string.IsNullOrEmpty(tableId))
            {
                throw new ArgumentNullException("tableId为空！");
            }
            if (tableId.Length == 1)
            {
                tableId = "0" + tableId;
            }

            string tableName1 = "db_action_licaitong.t_action_trans_" + tableId;
            string tableName2 = "db_action_licaitong.t_action_prize_" + tableId;

            StringBuilder Sql = new StringBuilder("SELECT b.FPriTransId,b.FPrizeType, b.FStandby2 as FPrizeName,b.FPrizeExpiredTime,b.FStartDate,b.FPrizeDesc,b.FErrInfo,b.FSPID as FGiveSpid,b.FStatus as FGiveState,b.FMoney as FPrizeMoney,b.FTransIdB as FGivePosId,b.FCreateTime as FPrizeTime,b.FModifyTime as FPrizeModifyTime FROM ");
            Sql.Append(tableName2 + " b ");
            Sql.Append(" where b.FPayUin='" + payUin + "' and b.FActionId='" + actId + "'");

            using (var da = MySQLAccessFactory.GetMySQLAccess("LCTActivity"))
            {
                da.OpenConn();

                DataSet ds = da.dsGetTotalData(Sql.ToString());

                return ds;
            }
        }

        //用户翻倍收益卡活动
        public DataSet QueryUserFBSYKActivity(string payUin, string stime, string etime, int start, int count)
        {
            string tableId = COMMLIB.CommUtil.GetLCTTableId(payUin);
            if (string.IsNullOrEmpty(tableId))
            {
                throw new ArgumentNullException("tableId为空！");
            }
            if (tableId.Length == 1)
            {
                tableId = "0" + tableId;
            }

            string tableName = "db_lct_profit_card.t_profit_card_" + tableId;

            string sql = string.Format(@"SELECT FUin,FType,FRate,FProfitDays,
                                                                    CASE FStatus WHEN 0 THEN '未使用' 
                                                                                         WHEN 1 THEN '使用中' 
                                                                                         WHEN 2 THEN '已使用' 
                                                                                         WHEN 3 THEN '拆卡中' 
                                                                                         WHEN 4 THEN '拆卡完毕' END AS FStatus,
                                                                    FDealTime,FExpiredTime,FProfitStartDay,FProfitEndDay,FProfitInfo 
                                                        FROM {0} WHERE FPayUin= '{1}' AND FCreateTime BETWEEN '{2}' AND '{3}'", tableName, payUin, stime, etime);
            using (var da = MySQLAccessFactory.GetMySQLAccess("UserfbsykActivity"))
            {
                da.OpenConn();
                DataSet ds = da.dsGetTotalData(sql);
                return ds;
            }
        }

        //微信支付活动
        public DataSet QueryWXZFActivity(string uin, string stime, string etime, int start, int count)
        {
            if (string.IsNullOrEmpty(uin))
            {
                throw new ArgumentNullException("必填参数：账号为空！");
            }

            int len = uin.Length;
            string tableName = "db_action_wx.t_pay_and_cdkey_" + uin.Substring(len - 2);
            // string sql = "SELECT a.*,b.FActName FROM " + tableName + " a, db_action_info.t_cft_action_info b  WHERE a.FActId=b.FActId  and a.FTransId= '" + uin + "'";
            string sql = "SELECT * FROM " + tableName + " WHERE FTransId= '" + uin + "'";
            if (!string.IsNullOrEmpty(stime))
            {
                sql += " AND FCreateTime>='" + stime + "'";
            }
            if (!string.IsNullOrEmpty(etime))
            {
                sql += " AND FCreateTime<='" + etime + "'";
            }

            sql += " LIMIT " + start + "," + count;

            using (var da = MySQLAccessFactory.GetMySQLAccess("WXZFActivity"))
            {
                da.OpenConn();

                DataSet ds = da.dsGetTotalData(sql);

                return ds;
            }
        }

        //心意卡活动
        public DataSet QueryXYKActivity(string uin, string stime, string etime, string flag, int start, int count, out string sendCount)
        {
            if (string.IsNullOrEmpty(uin))
            {
                throw new ArgumentNullException("必填参数：账号为空！");
            }
            sendCount = "0";
            string receiveCount = "";
            string cgi = "";
            string msg = "";
            cgi = System.Configuration.ConfigurationManager.AppSettings["QueryXYKActivityCgi"].ToString();
            cgi += "?AppName=wx_mmpay_kf&f=json&wxuin=" + uin;
            if (!string.IsNullOrEmpty(stime))
            {
                cgi += "&time=" + System.Web.HttpUtility.UrlEncode(stime, System.Text.Encoding.GetEncoding("gb2312"));
            }
            cgi += "&start=" + start;
            cgi += "&limit=" + count;
            cgi += "&flag=" + flag;

            //LogHelper.LogInfo("XYK send req:" + cgi);
            string res = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetFromCGI(cgi, "UTF-8", out msg);
            if (msg != "")
            {
                throw new Exception(msg);
            }
            //LogHelper.LogInfo("XYK return:" + res);

            DataSet ds = TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.ParseCgiJsonForXYKQuery(res, flag, out sendCount, out receiveCount, out msg);
            LogHelper.LogInfo("XYK parse:" + msg);

            return ds;
        }

        public DataSet QueryXYKSendDetail(string sendId)
        {
            if (string.IsNullOrEmpty(sendId))
            {
                throw new ArgumentNullException("必填参数：单号为空！");
            }

            string cgi = "";
            string msg = "";
            cgi = System.Configuration.ConfigurationManager.AppSettings["QueryXYKSendDetailCgi"].ToString();
            cgi += "?AppName=wx_mmpay_kf&f=json&sendid=" + sendId;
            //LogHelper.LogInfo("XYK send req:" + cgi);
            string res = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetFromCGI(cgi, "UTF-8", out msg);
            if (msg != "")
            {
                throw new Exception(msg);
            }
            //LogHelper.LogInfo("XYK return:" + res);

            DataSet ds = TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.ParseCgiJsonForXYKSendDetailQuery(res, out msg);
            LogHelper.LogInfo("XYK parse:" + msg);
            return ds;
        }

        //添加新活动
        public void AddLctActivity(string actNo, string actName, string opeUin)
        {
            if (string.IsNullOrEmpty(actNo))
            {
                throw new ArgumentNullException("必填参数：单号为空！");
            }
            string qrySql = "select Fact_id from c2c_fmdb.t_activity_kf where Fact_id='" + actNo + "'";
            using (var da = MySQLAccessFactory.GetMySQLAccess("RefundRegister"))
            {
                da.OpenConn();

                DataTable dt = da.GetTable(qrySql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    throw new Exception("活动号已存在！" + actNo);
                }
            }

            string sql = "insert into c2c_fmdb.t_activity_kf(Fact_id,Fact_name,Fope_acc,Fcreate_time,Fmodify_time) values('{0}','{1}','{2}',now(),now())";
            sql = String.Format(sql, actNo, actName, opeUin);

            using (var da = MySQLAccessFactory.GetMySQLAccess("RefundRegister"))
            {
                da.OpenConn();

                da.ExecSqlNum(sql);
            }
        }

        //删除新活动
        public void DelLctActivity(int fid)
        {

            string qrySql = "select Fact_id from c2c_fmdb.t_activity_kf where Fid=" + fid;
            using (var da = MySQLAccessFactory.GetMySQLAccess("RefundRegister"))
            {
                da.OpenConn();

                DataTable dt = da.GetTable(qrySql);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new Exception("活动不存在！");
                }
            }

            string sql = string.Format("update c2c_fmdb.t_activity_kf set Fstate=4, Fmodify_time=now() where Fid={0}", fid);

            using (var da = MySQLAccessFactory.GetMySQLAccess("RefundRegister"))
            {
                da.OpenConn();

                da.ExecSqlNum(sql);
            }
        }

        public DataTable QueryActivityList(string actNo, string actName, int offset, int limit)
        {
            string qrySql = "select * from c2c_fmdb.t_activity_kf where Fstate=2 ";
            if (actNo != null && actNo.Trim() != "")
            {
                qrySql += " AND Fact_id='" + actNo + "'";
            }
            if (actName != null && actName.Trim() != "")
            {
                qrySql += " AND Fact_name like'%" + actName + "%'";
            }
            qrySql += " LIMIT " + offset + "," + limit;
            using (var da = MySQLAccessFactory.GetMySQLAccess("RefundRegister"))
            {
                da.OpenConn();

                DataTable dt = da.GetTable(qrySql);

                return dt;
            }
        }

        //通过FUid找到用户参加活动的渠道号
        public string GetChannelIDByFUId(string uid)
        {
            if (string.IsNullOrEmpty(uid)) return string.Empty;

            var tableName = string.Format("fund_db_{0}.t_trade_user_fund_{1}", uid.Substring(uid.Length - 2), uid.Substring(uid.Length - 3, 1));

            string sql = string.Format("SELECT Fchannel_id FROM {0} WHERE Fuid = '{1}' ", tableName, uid);
            using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            {
                da.OpenConn();
                DataTable dt = da.GetTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["Fchannel_id"].ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
        }
        private string GetParamFields()
        {
            return "select Fuin,FActId,FTransId,Fuin,FState,FPrizeDesc,FPrizeInfo,FPayFee,FPayTime,FCreateTime,FErrInfo,FUsrState,FPrizeLv,FPrizeType,FStandby2 from  ";
        }

        //手Q活动
        public DataSet QueryHandQActivity(string strUin, string strBegingTime, string strEndTime, int nStart, int nCount)
        {
            if (string.IsNullOrEmpty(strUin))
            {
                throw new ArgumentNullException("请输入财付通帐号。");
            }

            int len = strUin.Length;
            string tableName = "db_action_mobileqq.t_pay_action_" + strUin.Substring(len - 2);

            StringBuilder Sql = new StringBuilder(GetParamFields() + tableName);
            Sql.Append(" where Fuin=" + strUin);
            if (!string.IsNullOrEmpty(strBegingTime))
            {
                Sql.Append(" AND FCreateTime >='" + strBegingTime + "'");
            }
            if (!string.IsNullOrEmpty(strEndTime))
            {
                Sql.Append(" AND FCreateTime <='" + strEndTime + "'");
            }
            Sql.Append(" limit " + nStart + "," + nCount);

            using (var da = MySQLAccessFactory.GetMySQLAccess("HandQInfo"))
            {
                da.OpenConn();

                DataSet ds = da.dsGetTotalData(Sql.ToString());

                return ds;
            }
        }

    }
}
