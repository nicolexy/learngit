using System;
using System.Data;


namespace CFT.CSOMS.DAL.FundModule
{
    using CFT.CSOMS.DAL.Infrastructure;
    using TENCENT.OSS.CFT.KF.Common;

    public class FundInfoData
    {
        string serverIp = System.Configuration.ConfigurationManager.AppSettings["FundRateIP"].ToString();
        int serverPort = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["FundRatePort"].ToString());
        public DataTable QueryAllFundInfo()
        {
            //using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            //{
            //    da.OpenConn();
            //    string Sql = " select Fsp_name,Fspid,Ffund_name,Ffund_code,Fcurtype from fund_db.t_fund_sp_config";
            //    DataSet ds = da.dsGetTotalData(Sql);

            //    return ds.Tables[0];
            //}



            //spid不是查询条件，没有意义；
            DataTable dt = null;
            string requestText = "reqid=658&flag=2&offset={0}&limit={1}&fields=spid:1234567890";
            DataSet ds = new DataSet();
            int pageindex = 0;
            int limit = 20;
            while (true)
            {
                string requestTextTemp = string.Format(requestText, pageindex * limit, limit);
                DataSet dsTemp = RelayAccessFactory.GetDSFromRelayFromXML(requestTextTemp, "100769", serverIp, serverPort);
                if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
                {
                    ds = PublicRes.ToOneDataset(dsTemp, ds);
                }
                else
                {
                    break;
                }
                pageindex++;
            }
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;

            
        }

        //通过商户号查询基金公司信息
        public DataTable QueryFundInfoBySpid(string spid)
        {
            //if (string.IsNullOrEmpty(spid))
            //    throw new ArgumentNullException("spid");
            //using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            //{
            //    da.OpenConn();
            //    string Sql = " select Fsp_name,Fspid,Ffund_name,Ffund_code,Fcurtype,Fclose_flag,Ftransfer_flag,Fbuy_valid from fund_db.t_fund_sp_config where Fspid='" + spid + "'";
            //    DataSet ds = da.dsGetTotalData(Sql);

            //    return ds.Tables[0];
            //}

            DataTable dt = null;
            string requestText = "reqid=659&flag=1&fields=spid:" + spid;
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }
       
        //判断是否重新申购基金
        public bool QueryIfAnewBoughtFund(string listid, DateTime time)
        {
            //using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            //{
            //    da.OpenConn();
            //    string Sql = " select count(1) from fund_db.t_fetch_list_" + time.ToString("yyyyMM") + " where Ffund_apply_id= '" + listid + "'";
            //    if (da.GetOneResult(Sql) == "1")  //存在表示重新申购的基金
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}


            var serverIp = System.Configuration.ConfigurationManager.AppSettings["FundRateIP"].ToString();
            var serverPort = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["FundRatePort"].ToString());
            string requestText = "reqid=680&flag=1&fields=order_time:" + time.ToString("yyyy-MM-dd") + "|fund_apply_id:" + listid;
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        //通过商户号+订单号（若为提现单则是后18位）查询基金交易单
        public DataTable QueryTradeFundInfo(string spid, string listid)
        {
            //if (string.IsNullOrEmpty(spid.Trim()))
            //    throw new ArgumentNullException("spid");
            //if (string.IsNullOrEmpty(listid))
            //    throw new ArgumentNullException("listid");
            //using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            //{
            //    da.OpenConn();
            //    var table_name = string.Format("fund_db_{0}.t_trade_fund_{1}", listid.Substring(listid.Length - 2), listid.Substring(listid.Length - 3, 1));
            //    string Sql = string.Format(" select Ffetchid, Fspid,Ffund_name,Ffund_code,Fpur_type,Fcharge_fee from {0} where Fspid='{1}' and Flistid='{2}'", table_name, spid, listid);
            //    DataSet ds = da.dsGetTotalData(Sql);

            //    return ds.Tables[0];
            //}

           // listid可以唯一标识，不需要新增spid的判断
            DataTable dt = null;
            string requestText = "reqid=665&flag=1&fields=listid:" + listid;
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        //查询定期产品用户交易记录
        public DataTable QueryCloseFundRollList(string tradeId, string fundCode, string beginDateStr, string endDateStr, int currentPageIndex = 0, int pageSize = 10)
        {
            //if (string.IsNullOrEmpty(tradeId))
            //    throw new ArgumentNullException("tradeId");
            //if (string.IsNullOrEmpty(fundCode))
            //    throw new ArgumentNullException("fundCode");
            //using (var da = MySQLAccessFactory.GetMySQLAccess("Fund"))
            //{
            //    da.OpenConn();
            //    var table_name = string.Format("fund_db_{0}.t_fund_close_trans_{1}", tradeId.Substring(tradeId.Length - 2), tradeId.Substring(tradeId.Length - 3, 1));
            //    string Sql = string.Format(" select * from {0} where Ftrade_id='{1}' and Ffund_code='{2}' ", table_name, tradeId, fundCode);
            //    Sql = string.Format(Sql + " and Ftrans_date between '{0}' and '{1}' ", beginDateStr, endDateStr);
            //    Sql = string.Format(Sql + " limit {0},{1} ", currentPageIndex, pageSize);
            //    DataSet ds = da.dsGetTotalData(Sql);
            //    return ds.Tables[0];
            //}

            if (string.IsNullOrEmpty(tradeId))
                throw new ArgumentNullException("tradeId");
            if (string.IsNullOrEmpty(fundCode))
                throw new ArgumentNullException("fundCode");
            DataTable dt = null;
            string requestText = "reqid=681&flag=2&offset={0}&limit={1}&fields=trade_id:{2}|fund_code:{3}|begin_time:{4}|end_time:{5}";
            requestText = string.Format(requestText, currentPageIndex, pageSize, tradeId, fundCode, beginDateStr, endDateStr);
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];        
            }
            return dt;


        }
         

        //查询预估收益
        public DataTable QueryEstimateProfit(string tradeId, string fundCode, string beginDateStr, string endDateStr, int currentPageIndex = 0, int pageSize = 10)
        {
            if (string.IsNullOrEmpty(tradeId))
                throw new ArgumentNullException("tradeId");
            if (string.IsNullOrEmpty(fundCode))
                throw new ArgumentNullException("fundCode");
            DataTable dt = null;
            string requestText = "reqid=684&flag=2&offset={0}&limit={1}&fields=trade_id:{2}";
            requestText = string.Format(requestText, currentPageIndex, pageSize, tradeId);
            if (!string.IsNullOrEmpty(fundCode))
            {
                requestText += "|fund_code:" + fundCode;
            }
            if (!string.IsNullOrEmpty(beginDateStr))
            {
                requestText += "|begin_day:" + beginDateStr;
            }
            if (!string.IsNullOrEmpty(endDateStr))
            {
                requestText += "|end_day:" + endDateStr;
            }

            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;


        }

        #region 定投
        
        //定投计划表
        public DataTable Get_DT_fundBuyPlan(string uid, int offset, int limit)
        {
            string requestText = "reqid=712&flag=2&offset={0}&limit={1}&fields=uid:{2}";
            requestText = string.Format(requestText, offset, limit, uid);
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            else 
            {
                throw new LogicException("查询数据为空");
            }
            return dt;

        }
        //定投计划表 单个
        public DataTable Get_DT_fundBuyPlanByPlanid(string uid, string plan_id)
        {
            string requestText = "reqid=714&flag=1&fields=uid:{0}|plan_id:{1}";
            requestText = string.Format(requestText, uid, plan_id);

            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            else
            {
                throw new LogicException("查询数据为空");
            }
            return dt;
        }
        /// 定投交易单
        public DataTable Get_DT_PlanBuyOrder(string uid, string plan_id, int offset, int limit)
        {
            string requestText = "reqid=713&flag=2&offset={0}&limit={1}&fields=uid:{2}|plan_id:{3}";
            requestText = string.Format(requestText, offset, limit, uid, plan_id);
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            else
            {
                throw new LogicException("查询数据为空");
            }
            return dt;
        }
        #endregion

        #region 定赎
        public DataTable Get_HFD_FundFetchPlan(string uin, int offset, int limit)
        {
            string requestText = "reqid=717&flag=2&offset={0}&limit={1}&fields=uin:{2}";
            requestText = string.Format(requestText, offset, limit, uin);
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            else
            {
                throw new LogicException("查询数据为空");
            }
            return dt;
        }

        public DataTable Get_HFD_FundFetchPlanByPlanid(string uin, string plan_id)
        {
            string requestText = "reqid=715&flag=1&fields=uin:{0}|plan_id:{1}";
            requestText = string.Format(requestText, uin, plan_id);
             
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            else
            {
                throw new LogicException("查询数据为空");
            }
            return dt;
        }

        public DataTable Get_HFD_PlanFetchOrder(string uin, string plan_id, int offset, int limit)
        {
            string requestText = "reqid=718&flag=2&offset={0}&limit={1}&fields=uin:{2}|plan_id:" + plan_id;
            requestText = string.Format(requestText, offset, limit, uin);
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            else
            {
                throw new LogicException("查询数据为空");
            }
            return dt;
        }
        #endregion

        #region 梦想计划
        /// <summary>
        /// 计划列表
        /// </summary>
        /// <param name="uin"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public DataTable Get_DreamProject_Plan(string uin, int offset, int limit) 
        { 
            string requestText = "reqid=751&flag=2&offset={0}&limit={1}&fields=uin:{2}";
            requestText = string.Format(requestText, offset, limit, uin);
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            else
            {
                throw new LogicException("查询数据为空");
            }
            return dt;
        }
        /// <summary>
        /// 交易单列表
        /// </summary>
        /// <param name="plan_id"></param>
        /// <param name="trade_id"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public DataTable Get_DreamProject_trans(string plan_id, string trade_id, int offset, int limit)
        {
            string requestText = "reqid=753&flag=2&offset={0}&limit={1}&fields=trade_id:{2}|plan_id:{3}";
            requestText = string.Format(requestText, offset, limit, trade_id, plan_id);
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            else
            {
                throw new LogicException("查询数据为空");
            }
            return dt;
        }
        /// <summary>
        /// 资产列表
        /// </summary>
        /// <param name="plan_id"></param>
        /// <param name="trade_id"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public DataTable Get_DreamProject_asset(string plan_id, string trade_id, int offset, int limit)
        {
            string requestText = "reqid=755&flag=2&offset={0}&limit={1}&fields=trade_id:{2}|plan_id:{3}";
            requestText = string.Format(requestText, offset, limit, trade_id, plan_id);
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            else
            {
                throw new LogicException("查询数据为空");
            }
            return dt;
        }

        /// <summary>
        /// 受控资产列表
        /// </summary>
        /// <param name="plan_id"></param>
        /// <param name="trade_id"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public DataTable Get_DreamProject_controlasset(string trade_id, string spid, string fund_code)
        {
            string requestText = "reqid=756&flag=2&offset={0}&limit={1}&fields=trade_id:{2}|spid:{3}|fund_code:{4}";
            requestText = string.Format(requestText, 0, 1, trade_id, spid, fund_code);
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            else
            {
                throw new LogicException("查询数据为空");
            }
            return dt;
        }
        #endregion
        //预约买入
        public DataTable GetLCTReserveOrder(string trade_id, string listid, string stime, string etime, int offset, int limit)
        {
            //20151102870920001000
            string requestText = "reqid=710&flag=2&offset={0}&limit={1}&fields=trade_id:{2}";
            requestText = string.Format(requestText, offset, limit, trade_id);
            if (!string.IsNullOrEmpty(listid))
            {
                requestText += "|listid:" + listid;
            }
            if (!string.IsNullOrEmpty(stime))
            {
                requestText += "|reserve_begin_time:" + stime;
            }
            if (!string.IsNullOrEmpty(etime))
            {
                requestText += "|reserve_end_time:" + etime;
            }
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            else
            {
                throw new LogicException("查询数据为空");
            }
            return dt;
        }
        //理财通转投
        public DataTable GetLCTSwith(string trade_id, string buy_id, string redem_id, string change_id)
        {
            /* <request>
       <reqid>628</reqid>
       <flag>1</flag>
       <notes>查询转换记录</notes>
       <db_no>4405</db_no>
        <db_type>1</db_type>
        <db_field>trade_id</db_field>
        <sql>
       SELECT Fchange_id,Ftotal_fee,Ftrade_id,Fori_spid,Fnew_spid,Fori_fund_code, \
       Fnew_fund_code,Fbuy_id,Fredem_id,Fstate,Facc_time \
       FROM  fund_db_$AA$.t_change_sp_record_$B$ \
       WHERE Fbuy_id='$buy_id$' \
       OR Fredem_id='$redem_id$' \
       OR Fchange_id='$change_id$' \
      AND Ftrade_id='$trade_id$' \
        </sql>
 
             </request>*/

            string requestText = "reqid=628&flag=1&fields=trade_id:" + trade_id;
            if (!string.IsNullOrEmpty(buy_id))
            {
                requestText += "|buy_id:" + buy_id;
            }
            if (!string.IsNullOrEmpty(redem_id))
            {
                requestText += "|redem_id:" + redem_id;
            }
            if (!string.IsNullOrEmpty(change_id))
            {
                requestText += "|change_id:" + change_id;
            }
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            else
            {
                throw new LogicException("查询数据为空");
            }
            return dt;
        }
        public DataTable Query_QuotationTransaction(string trade_id, DateTime Fdue_date, int offset, int limit)
        {

//            <request>
//    <reqid>759</reqid>
//    <flag>1</flag>
//    <notes>报价交易详情</notes>
//    <db_no>4405</db_no>
//    <db_type>1</db_type>
//    <needauth>false</needauth>
//    <needtradeid>true</needtradeid>
//    <db_field>trade_id</db_field>
//    <sql>
//        SELECT Fid,Fissue,Ftrade_id,Fspid,Ffund_code, \
//        Ftrans_date,Fvalue_date,Fduration,Fdue_date, \
//        Fprofit_recon_date,Ffetch_arrive_date,Fredem_type, \
//        Fprofit_rate,Fterminate_profit_rate, \
//        Ftotal_fee,Fcreate_time,Fmodify_time,Fstate \
//        Fend_transfer_spid,Fend_transfer_fundcode,Fend_redem_listid \
//        FROM fund_db_$AA$.t_quote_trans_$B$   \
//        WHERE Ftrade_id='$trade_id$'  \
//        AND Fdue_date='$due_date$' \     
//    </sql>
//</request>
            //request:&fields=trade_id:20150908000028213|due_date:20160408&flag=2&limit=2&offset=0&reqid=759

            string due_date = Fdue_date.ToString("yyyyMMdd");
            string requestText = "reqid=759&flag=2&offset=" + offset +
                "&limit=" + limit + "&fields=trade_id:" + trade_id + "|due_date:" + due_date;
           
            requestText += "&MSG_NO=100769759" + DateTime.Now.ToString("yyyyMMddHHmmss");
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "100769", serverIp, serverPort);
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            else
            {
                throw new LogicException("查询数据为空");
            }
            return dt;

        }
    }
}
