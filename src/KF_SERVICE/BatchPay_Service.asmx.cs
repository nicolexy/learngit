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
    /// BatchPay_Service 的摘要说明。
    /// </summary>
    [WebService(Namespace = "http://Tencent.com/OSS/C2C/Finance/BatchPay_Service")]
    public class BatchPay_Service : System.Web.Services.WebService
    {
        public BatchPay_Service()
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

        // WEB 服务示例
        // HelloWorld() 示例服务返回字符串 Hello World
        // 若要生成，请取消注释下列行，然后保存并生成项目
        // 若要测试此 Web 服务，请按 F5 键

        //		[WebMethod]
        //		public string HelloWorld()
        //		{
        //			return "Hello World";
        //		}

        /// <summary>
        /// 获取指定批次号的异常退单记录
        /// </summary>
        /// <param name="strBatchID">批次号</param>
        /// <returns>返回结果</returns>
        [WebMethod]
        public DataSet RefundOther_ShowData(string strBatchID)
        {
            DataTable dt;
            MySqlAccess dazw = new MySqlAccess(PublicRes.GetConnString("ZW"));
            try
            {
                //汇总银行调整转用标记20060615
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
        /// 取出指定时间所有银行的退单异常批次情况
        /// </summary>
        /// <param name="WeekIndex">指定时间</param>
        /// <returns>返回数据集</returns>
        [WebMethod]
        public DataSet RefundOther_InitGrid_R(string beginDate, string endDate, string banktype, int status, string proposer, string refundPath)
        {
            string strBeginDate = DateTime.Parse(beginDate).ToString("yyyyMMdd");
            string strEndDate = DateTime.Parse(endDate).ToString("yyyyMMdd");

            //汇总银行调整转用标记20060615
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

                //网银退单的批次需要根据退款情况来更新批次状态
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
                            //更新批次表状态
                            string upSql = "update c2c_zwdb.t_batchrefundother_rec set FStatus=99 where fbatchid='" + handleBatchid + "' and FStatus in(4,96)";
                            int bcount = Convert.ToInt32(dazw.ExecSqlNum(upSql));
                            //更新退款异常表中状态
                            //							if(bcount==1)
                            //							{
                            string upSql2 = "update c2c_zwdb.t_refund_other o,c2c_zwdb.t_refund_total t set o.FHandleType=3, o.Fmodify_Time=now(),o.FRefundMemo=concat(o.FRefundMemo,'|于" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "获取到再次订单退款成功" + "')  where o.Foldid=t.Foldid and o.FHandleBatchId ='" + handleBatchid + "' and t.Fstate=2 and o.FHandleType=2";
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
                        dr["FStatusName"] = "生成退单任务单";
                    }
                    else if (tmp == "1")
                    {
                        dr["FStatusName"] = "申请再次网银退款";
                        dr["FMsg"] = "等待审批通过";
                    }
                    else if (tmp == "2")
                    {
                        dr["FStatusName"] = "申请付款退款";
                        dr["FMsg"] = "等待审批通过";
                    }
                    else if (tmp == "3")
                    {
                        dr["FStatusName"] = "申请人工授权退款";
                        dr["FMsg"] = "等待审批通过";
                    }
                    else if (tmp == "4")
                    {
                        dr["FStatusName"] = "审批通过";
                        dr["FMsg"] = "等待统一汇总退款";
                    }
                    else if (tmp == "5")
                    {
                        dr["FStatusName"] = "审批通过";

                    }
                    else if (tmp == "6")
                    {
                        dr["FStatusName"] = "审批通过";
                        dr["FMsg"] = "请生成授权书开始退款";
                    }
                    else if (tmp == "7")
                    {
                        dr["FStatusName"] = "再次网银退款中";
                        dr["FMsg"] = "等待退款结果回导";
                    }
                    else if (tmp == "8")
                    {
                        dr["FStatusName"] = "付款退款退款中";
                    }
                    else if (tmp == "9")
                    {
                        dr["FStatusName"] = "人工授权退款中";
                        dr["FMsg"] = "等待授权退款结果";
                    }
                    else if (tmp == "10")
                    {
                        dr["FStatusName"] = "再次网银退款结果回导完成";
                        dr["FMsg"] = "请关注退款结果";
                    }
                    else if (tmp == "11")
                    {
                        dr["FStatusName"] = "出纳完成人工授权操作";
                        dr["FMsg"] = "可以手工生效退款结果";
                    }
                    else if (tmp == "12")
                    {
                        dr["FStatusName"] = "退款直接调整为成功审批中";
                        dr["FMsg"] = "等待审批通过";
                    }
                    else if (tmp == "13")
                    {
                        dr["FStatusName"] = "申请转入代发处理中";
                        dr["FMsg"] = "等待审批通过";
                    }
                    else if (tmp == "96")
                    {
                        dr["FStatusName"] = "再次网银退款中";
                        dr["FMsg"] = "";
                    }
                    else if (tmp == "97")
                    {
                        dr["FStatusName"] = "授权书新流程退款中";
                        dr["FMsg"] = "";
                    }
                    else if (tmp == "98")
                    {
                        dr["FStatusName"] = "人工授权书生成中";
                        dr["FMsg"] = "";
                    }

                    else if (tmp == "99")
                    {
                        dr["FStatusName"] = "批次处理完成";

                    }
                    else
                    {
                        dr["FStatusName"] = "未知状态" + tmp;
                    }

                    tmp = dr["FBatchDay"].ToString();
                    dr["FBatchDay"] = tmp.Substring(0, 4) + "年" + tmp.Substring(4, 2) + "月" + tmp.Substring(6, 2) + "日";

                    tmp = dr["FBatchID"].ToString();
                    if (tmp.Length == 13 && tmp.IndexOf("E") > -1)
                    {
                        tmp = tmp.Substring(10, 1);
                        if (tmp == "1")
                        {
                            dr["FRefundPath"] = "网银退单";
                        }
                        else if (tmp == "3")
                        {
                            dr["FRefundPath"] = "人工授权退款";
                        }
                        else if (tmp == "6")
                        {
                            dr["FRefundPath"] = "付款退款";
                        }
                        else if (tmp == "9")
                        {
                            dr["FRefundPath"] = "直接调整为退款成功";
                        }
                        else if (tmp == "7")
                        {
                            dr["FRefundPath"] = "转入代发";
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
