using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CFT.CSOMS.BLL.CreditModule;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace TENCENT.OSS.CFT.KF.KF_Web.CreditPay
{
    public partial class DetailSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["uid"] == null)
            {
                if (Request.QueryString["getAction"] != null)
                {
                    StringBuilder builder = new StringBuilder();
                    //builder.Append("[");
                    builder.Append("{");
                    builder.Append("\"result\":");
                    builder.Append("\"False\",");
                    builder.Append("\"message\":");
                    builder.Append("\"NoRight\",");
                    builder.Append("\"loginPath\":");
                    builder.Append("\"../login.aspx?returnUrl=" + Server.UrlEncode(Request.Url.AbsolutePath) + "\"");
                    builder.Append("}");
                    //builder.Append("]");
                    Response.Write(builder.ToString());
                    Response.End();
                }
                else
                {
                    Response.Write("<script type='text/javascript'>window.parent.location.href = '../login.aspx?returnUrl=" + Server.UrlEncode(Request.Url.PathAndQuery) + "';</script>");
                    Response.End();
                }
            }
            else
            {
                try
                {
                    this.Label_uid.Text = Session["uid"] == null ? string.Empty : Session["uid"].ToString();
                    if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this))
                    {
                        Response.Redirect("../login.aspx?wh=1");
                    }
                }
                catch (Exception ex)  //如果没有登陆或者没有权限就跳出
                {
                    Response.Redirect("../login.aspx?wh=1");
                }
                string getAction = Request.QueryString["getAction"] != null ? Request.QueryString["getAction"].ToString() : string.Empty;
                if (!string.IsNullOrEmpty(getAction))
                {
                    Action(getAction);
                }
            }
        }

        public void Action(string actionName)
        {
            if (actionName.Equals("LoadPayList"))
            {
                LoadPayList();//支付明细查询
            }
            else if (actionName.Equals("LoadRepayList"))
            {
                LoadRepayList();//还款明细查询
            }
            else if (actionName.Equals("LoadRefundList"))
            {
                LoadRefundList();//退款单查询
            }
            else if (actionName.Equals("LoadRefundDetail"))
            {
                LoadRefundDetail();//退款详情
            }
            else if (actionName.Equals("LoadRefundQuXiangInfo"))
            {
                LoadRefundQuXiangInfo();//退款去向
            }
            
        }

        /// <summary>
        /// 支付明细查询
        /// </summary>
        public void LoadPayList()
        {
            StringBuilder builder = new StringBuilder();
            string returnResult = string.Empty;
            try
            {
                string orderStr = (base.Request.Form["order"] != null) ? base.Request.Form["order"].ToString() : "asc";
                //int pageNumber = (base.Request.Form["page"] != null) ? int.Parse(base.Request.Form["page"].ToString()) : 1;
                //int pageSize = (base.Request.Form["rows"] != null) ? int.Parse(base.Request.Form["rows"].ToString()) : 15;
                int pageNumber = 0; //(base.Request.Form["page"] != null) ? int.Parse(base.Request.Form["page"].ToString()) : 1;
                int pageSize = int.MaxValue;// (base.Request.Form["rows"] != null) ? int.Parse(base.Request.Form["rows"].ToString()) : 15;
                if (base.Request.Form["pageNumber"] != null && base.Request.Form["pageSize"] != null && base.Request.Form["pageNumber"].Length > 0 && base.Request.Form["pageSize"].Length > 0)
                {
                    pageNumber = Convert.ToInt32(base.Request.Form["pageNumber"].ToString());
                    pageSize = Convert.ToInt32(base.Request.Form["pageSize"].ToString());
                }
                string sortStr = (base.Request.Form["sort"] != null) ? base.Request.Form["sort"].ToString() : "Fcreate_time ";

                string accountNo = Request.Form["accountNo"] != null && !string.IsNullOrEmpty(Request.Form["accountNo"].ToString()) ? Request.Form["accountNo"].ToString() : string.Empty;
                int accountType = Request.Form["accountType"] != null && !string.IsNullOrEmpty(Request.Form["accountType"].ToString()) ? int.Parse(Request.Form["accountType"].ToString()) : 1;
                string trans_id = Request.Form["trans_id"] != null && !string.IsNullOrEmpty(Request.Form["trans_id"].ToString()) ? Request.Form["trans_id"].ToString() : string.Empty;
                //int billStatus = Request.Form["billStatus"] != null && !string.IsNullOrEmpty(Request.Form["billStatus"].ToString()) ? int.Parse(Request.Form["billStatus"].ToString()) : 1;
                string beginDate = Request.Form["beginDate"] != null && !string.IsNullOrEmpty(Request.Form["beginDate"].ToString()) ? Request.Form["beginDate"].ToString() : string.Empty;
                string endDate = Request.Form["endDate"] != null && !string.IsNullOrEmpty(Request.Form["endDate"].ToString()) ? Request.Form["endDate"].ToString() : string.Empty;
                string next_row_key = Request.Form["next_row_key"] != null && !string.IsNullOrEmpty(Request.Form["next_row_key"].ToString()) ? Request.Form["next_row_key"].ToString() : string.Empty;
                string nextpage_flg = Request.Form["nextpage_flg"] != null && !string.IsNullOrEmpty(Request.Form["nextpage_flg"].ToString()) ? Request.Form["nextpage_flg"].ToString() : string.Empty;
                string timeStamp = CommQuery.GetTimeStamp();
                int req_type = string.IsNullOrEmpty(trans_id) ? 0 : 1;      //查询方式：0=按时间段；1=按交易订单号+时间段          
                TencentCreditService tencentCreditService = new TencentCreditService();
                bool searchResult = false;
                string errorMessage = string.Empty;
                int total = 0;
               
                //if (bool.Parse(nextpage_flg) || nextpage_flg.Equals("1"))
                //{
                    if (classLibrary.getData.IsTestMode && !classLibrary.getData.IsNewSensitivePowerMode)
                    {
                        returnResult = tencentCreditService.LoadPayList(accountNo, accountType, req_type = 0, trans_id, beginDate, endDate, timeStamp, pageSize, pageNumber, next_row_key, sortStr + " " + orderStr, out searchResult, out errorMessage, ref total);
                    }
                    else
                    {
                        returnResult = tencentCreditService.LoadPayList(accountNo, accountType, req_type = 0, trans_id, beginDate, endDate, timeStamp, pageSize, pageNumber, next_row_key, sortStr + " " + orderStr, out searchResult, out errorMessage, ref total);
                    }
                    if (searchResult)
                    {
                        builder.Append(returnResult);
                    }
                    else
                    {
                        //builder.Append("[");
                        builder.Append("{");
                        builder.Append("\"result\":");
                        builder.Append("\"" + searchResult + "\",");
                        builder.Append("\"res_info\":");
                        builder.Append("\"" + errorMessage + "\"");
                        builder.Append("}");
                        //builder.Append("]");
                    }
                //}
                //else
                //{
                //    searchResult = false;
                //    errorMessage = "没有数据了";
                //    //builder.Append("[");
                //    builder.Append("{");
                //    builder.Append("\"result\":");
                //    builder.Append("\"" + searchResult + "\",");
                //    builder.Append("\"res_info\":");
                //    builder.Append("\"" + errorMessage + "\"");
                //    builder.Append("}");
                //    //builder.Append("]");
                //}
            }
            catch (Exception ex)
            {
                //builder.Append("[");
                builder.Append("{");
                builder.Append("\"result\":");
                builder.Append("\"False\",");
                builder.Append("\"res_info\":");
                builder.Append("\"查询出错了!\"");
                builder.Append("}");
                //builder.Append("]");
            }
            Response.Write(builder.ToString());
            Response.End();
        }

        /// <summary>
        /// 还款明细查询
        /// </summary>
        public void LoadRepayList()
        {
            StringBuilder builder = new StringBuilder();
            string returnResult = string.Empty;
            try
            {
                string orderStr = (base.Request.Form["order"] != null) ? base.Request.Form["order"].ToString() : "asc";
                //int pageNumber = (base.Request.Form["page"] != null) ? int.Parse(base.Request.Form["page"].ToString()) : 1;
                //int pageSize = (base.Request.Form["rows"] != null) ? int.Parse(base.Request.Form["rows"].ToString()) : 15;
                int pageNumber = 0; //(base.Request.Form["page"] != null) ? int.Parse(base.Request.Form["page"].ToString()) : 1;
                int pageSize = int.MaxValue;// (base.Request.Form["rows"] != null) ? int.Parse(base.Request.Form["rows"].ToString()) : 15;
                if (base.Request.Form["pageNumber"] != null && base.Request.Form["pageSize"] != null && base.Request.Form["pageNumber"].Length > 0 && base.Request.Form["pageSize"].Length > 0)
                {
                    pageNumber = Convert.ToInt32(base.Request.Form["pageNumber"].ToString());
                    pageSize = Convert.ToInt32(base.Request.Form["pageSize"].ToString());
                }
                string sortStr = (base.Request.Form["sort"] != null) ? base.Request.Form["sort"].ToString() : "Fcreate_time ";

                string accountNo = Request.Form["accountNo"] != null && !string.IsNullOrEmpty(Request.Form["accountNo"].ToString()) ? Request.Form["accountNo"].ToString() : string.Empty;
                int accountType = Request.Form["accountType"] != null && !string.IsNullOrEmpty(Request.Form["accountType"].ToString()) ? int.Parse(Request.Form["accountType"].ToString()) : 1;
                string trans_id = Request.Form["trans_id"] != null && !string.IsNullOrEmpty(Request.Form["trans_id"].ToString()) ? Request.Form["trans_id"].ToString() : string.Empty;
                //int billStatus = Request.Form["billStatus"] != null && !string.IsNullOrEmpty(Request.Form["billStatus"].ToString()) ? int.Parse(Request.Form["billStatus"].ToString()) : 1;
                string beginDate = Request.Form["beginDate"] != null && !string.IsNullOrEmpty(Request.Form["beginDate"].ToString()) ? Request.Form["beginDate"].ToString() : string.Empty;
                string endDate = Request.Form["endDate"] != null && !string.IsNullOrEmpty(Request.Form["endDate"].ToString()) ? Request.Form["endDate"].ToString() : string.Empty;
                string next_row_key = Request.Form["next_row_key"] != null && !string.IsNullOrEmpty(Request.Form["next_row_key"].ToString()) ? Request.Form["next_row_key"].ToString() : string.Empty;
                string nextpage_flg = Request.Form["nextpage_flg"] != null && !string.IsNullOrEmpty(Request.Form["nextpage_flg"].ToString()) ? Request.Form["nextpage_flg"].ToString() : string.Empty;
                string timeStamp = CommQuery.GetTimeStamp();
                int req_type = string.IsNullOrEmpty(trans_id) ? 0 : 1;      //查询方式：0=按时间段；1=按交易订单号+时间段    
                TencentCreditService tencentCreditService = new TencentCreditService();
                bool searchResult = false;
                string errorMessage = string.Empty;
                int total = 0;               

                //if (bool.Parse(nextpage_flg) || nextpage_flg.Equals("1"))
                //{
                    if (classLibrary.getData.IsTestMode && !classLibrary.getData.IsNewSensitivePowerMode)
                    {
                        returnResult = tencentCreditService.LoadRepayList(accountNo, accountType, req_type = 0, trans_id, beginDate, endDate, timeStamp, pageSize, pageNumber, next_row_key, sortStr + " " + orderStr, out searchResult, out errorMessage, ref total);
                    }
                    else
                    {
                        returnResult = tencentCreditService.LoadRepayList(accountNo, accountType, req_type = 0, trans_id, beginDate, endDate, timeStamp, pageSize, pageNumber, next_row_key, sortStr + " " + orderStr, out searchResult, out errorMessage, ref total);
                    }
                    if (searchResult)
                    {
                        builder.Append(returnResult);
                    }
                    else
                    {
                        //builder.Append("[");
                        builder.Append("{");
                        builder.Append("\"result\":");
                        builder.Append("\"" + searchResult + "\",");
                        builder.Append("\"res_info\":");
                        builder.Append("\"" + errorMessage + "\"");
                        builder.Append("}");
                        //builder.Append("]");
                    }
                //}
                //else
                //{
                //    searchResult = false;
                //    errorMessage = "没有数据了";
                //    //builder.Append("[");
                //    builder.Append("{");
                //    builder.Append("\"result\":");
                //    builder.Append("\"" + searchResult + "\",");
                //    builder.Append("\"res_info\":");
                //    builder.Append("\"" + errorMessage + "\"");
                //    builder.Append("}");
                //    //builder.Append("]");
                //}
            }
            catch (Exception ex)
            {
                //builder.Append("[");
                builder.Append("{");
                builder.Append("\"result\":");
                builder.Append("\"False\",");
                builder.Append("\"res_info\":");
                builder.Append("\"查询出错了!\"");
                builder.Append("}");
                //builder.Append("]");
            }
            Response.Write(builder.ToString());
            Response.End();
        }

        /// <summary>
        /// 退款单查询
        /// </summary>
        public void LoadRefundList()
        {
            StringBuilder builder = new StringBuilder();
            string returnResult = string.Empty;
            try
            {
                string orderStr = (base.Request.Form["order"] != null) ? base.Request.Form["order"].ToString() : "asc";
                int pageNumber = 0; //(base.Request.Form["page"] != null) ? int.Parse(base.Request.Form["page"].ToString()) : 1;
                int pageSize = int.MaxValue;// (base.Request.Form["rows"] != null) ? int.Parse(base.Request.Form["rows"].ToString()) : 15;
                if (base.Request.Form["pageNumber"] != null && base.Request.Form["pageSize"] != null && base.Request.Form["pageNumber"].Length > 0 && base.Request.Form["pageSize"].Length > 0)
                {
                    pageNumber = Convert.ToInt32(base.Request.Form["pageNumber"].ToString());
                    pageSize = Convert.ToInt32(base.Request.Form["pageSize"].ToString());
                }
                string sortStr = (base.Request.Form["sort"] != null) ? base.Request.Form["sort"].ToString() : "Fcreate_time ";

                string accountNo = Request.Form["accountNo"] != null && !string.IsNullOrEmpty(Request.Form["accountNo"].ToString()) ? Request.Form["accountNo"].ToString() : string.Empty;
                int accountType = Request.Form["accountType"] != null && !string.IsNullOrEmpty(Request.Form["accountType"].ToString()) ? int.Parse(Request.Form["accountType"].ToString()) : 1;
                string trans_id = Request.Form["trans_id"] != null && !string.IsNullOrEmpty(Request.Form["trans_id"].ToString()) ? Request.Form["trans_id"].ToString() : string.Empty;
                //int billStatus = Request.Form["billStatus"] != null && !string.IsNullOrEmpty(Request.Form["billStatus"].ToString()) ? int.Parse(Request.Form["billStatus"].ToString()) : 1;
                string beginDate = Request.Form["beginDate"] != null && !string.IsNullOrEmpty(Request.Form["beginDate"].ToString()) ? Request.Form["beginDate"].ToString() : string.Empty;
                string endDate = Request.Form["endDate"] != null && !string.IsNullOrEmpty(Request.Form["endDate"].ToString()) ? Request.Form["endDate"].ToString() : string.Empty;
                string next_row_key = Request.Form["next_row_key"] != null && !string.IsNullOrEmpty(Request.Form["next_row_key"].ToString()) ? Request.Form["next_row_key"].ToString() : string.Empty;//this.hid_RefundListnext_row_key.Value;
                string nextpage_flg = Request.Form["nextpage_flg"] != null && !string.IsNullOrEmpty(Request.Form["nextpage_flg"].ToString()) ? Request.Form["nextpage_flg"].ToString() : string.Empty;
                string timeStamp = CommQuery.GetTimeStamp();
                int req_type = string.IsNullOrEmpty(trans_id) ? 0 : 1;      //查询方式：0=按时间段；1=按交易订单号+时间段    
                TencentCreditService tencentCreditService = new TencentCreditService();
                bool searchResult = false;
                string errorMessage = string.Empty;
                int total = 0;
                //nextpage_flg 后续是否还有数据 ;0：没数据;1：有数据
                //if (bool.Parse(nextpage_flg) || nextpage_flg.Equals("1"))
                //{
                    if (classLibrary.getData.IsTestMode && !classLibrary.getData.IsNewSensitivePowerMode)
                    {
                        returnResult = tencentCreditService.LoadRefundList(accountNo, accountType, req_type = 0, trans_id, beginDate, endDate, timeStamp, pageSize, pageNumber, next_row_key, sortStr + " " + orderStr, out searchResult, out errorMessage, ref total);
                    }
                    else
                    {
                        returnResult = tencentCreditService.LoadRefundList(accountNo, accountType, req_type = 0, trans_id, beginDate, endDate, timeStamp, pageSize, pageNumber, next_row_key, sortStr + " " + orderStr, out searchResult, out errorMessage, ref total);
                    }
                    if (searchResult)
                    {
                        builder.Append(returnResult);
                    }
                    else
                    {
                        //builder.Append("[");
                        builder.Append("{");
                        builder.Append("\"result\":");
                        builder.Append("\"" + searchResult + "\",");
                        builder.Append("\"res_info\":");
                        builder.Append("\"" + errorMessage + "\"");
                        builder.Append("}");
                        //builder.Append("]");
                    }
                //}
                //else
                //{
                //    searchResult = false;
                //    errorMessage = "没有数据了";
                //    //builder.Append("[");
                //    builder.Append("{");
                //    builder.Append("\"result\":");
                //    builder.Append("\"" + searchResult + "\",");
                //    builder.Append("\"res_info\":");
                //    builder.Append("\"" + errorMessage + "\"");
                //    builder.Append("}");
                //    //builder.Append("]");
                //}
                
            }
            catch (Exception ex)
            {
                //builder.Append("[");
                builder.Append("{");
                builder.Append("\"result\":");
                builder.Append("\"False\",");
                builder.Append("\"res_info\":");
                builder.Append("\"查询出错了!\"");
                builder.Append("}");
                //builder.Append("]");
            }
            Response.Write(builder.ToString());
            Response.End();
        }
        
        /// <summary>
        /// 退款详情
        /// </summary>
        public void LoadRefundDetail()
        {
            StringBuilder builder = new StringBuilder();
            string returnResult = string.Empty;
            try
            {
                string orderStr = (base.Request.Form["order"] != null) ? base.Request.Form["order"].ToString() : "asc";
                int pageNumber = (base.Request.Form["page"] != null) ? int.Parse(base.Request.Form["page"].ToString()) : 1;
                int pageSize = (base.Request.Form["rows"] != null) ? int.Parse(base.Request.Form["rows"].ToString()) : 15;
                string sortStr = (base.Request.Form["sort"] != null) ? base.Request.Form["sort"].ToString() : "Fcreate_time ";

                string accountNo = Request.Form["accountNo"] != null && !string.IsNullOrEmpty(Request.Form["accountNo"].ToString()) ? Request.Form["accountNo"].ToString() : string.Empty;
                int accountType = Request.Form["accountType"] != null && !string.IsNullOrEmpty(Request.Form["accountType"].ToString()) ? int.Parse(Request.Form["accountType"].ToString()) : 1;
                string refund_flow_id =  Request.Form["refund_flow_id"] != null && !string.IsNullOrEmpty(Request.Form["refund_flow_id"].ToString()) ? Request.Form["refund_flow_id"].ToString() : string.Empty;
                string c_rchg_id = string.Empty;// Request.Form["c_rchg_id"] != null && !string.IsNullOrEmpty(Request.Form["c_rchg_id"].ToString()) ? Request.Form["c_rchg_id"].ToString() : string.Empty;
                //int billStatus = Request.Form["billStatus"] != null && !string.IsNullOrEmpty(Request.Form["billStatus"].ToString()) ? int.Parse(Request.Form["billStatus"].ToString()) : 1;
                string beginDate = Request.Form["beginDate"] != null && !string.IsNullOrEmpty(Request.Form["beginDate"].ToString()) ? Request.Form["beginDate"].ToString() : string.Empty;
                string endDate = Request.Form["endDate"] != null && !string.IsNullOrEmpty(Request.Form["endDate"].ToString()) ? Request.Form["endDate"].ToString() : string.Empty;
                string timeStamp = CommQuery.GetTimeStamp();
                //int req_type = 0;
                TencentCreditService tencentCreditService = new TencentCreditService();
                bool searchResult = false;
                string errorMessage = string.Empty;
                int total = 0;

                if (classLibrary.getData.IsTestMode && !classLibrary.getData.IsNewSensitivePowerMode)
                {
                    returnResult = tencentCreditService.LoadRefundDetail(accountNo, accountType, refund_flow_id,c_rchg_id, beginDate, endDate, timeStamp, pageSize, pageNumber, sortStr + " " + orderStr, out searchResult, out errorMessage, ref total);
                }
                else
                {
                    returnResult = tencentCreditService.LoadRefundDetail(accountNo, accountType, refund_flow_id, c_rchg_id,beginDate, endDate, timeStamp, pageSize, pageNumber, sortStr + " " + orderStr, out searchResult, out errorMessage, ref total);
                }
                if (searchResult)
                {
                    builder.Append(returnResult);
                }
                else
                {
                    //builder.Append("[");
                    builder.Append("{");
                    builder.Append("\"result\":");
                    builder.Append("\"" + searchResult + "\",");
                    builder.Append("\"res_info\":");
                    builder.Append("\"" + errorMessage + "\"");
                    builder.Append("}");
                    // builder.Append("]");
                }
            }
            catch (Exception ex)
            {
                //builder.Append("[");
                builder.Append("{");
                builder.Append("\"result\":");
                builder.Append("\"False\",");
                builder.Append("\"res_info\":");
                builder.Append("\"查询出错了!\"");
                builder.Append("}");
                //builder.Append("]");
            }
            Response.Write(builder.ToString());
            Response.End();
        }

        /// <summary>
        /// 退款去向
        /// </summary>
        public void LoadRefundQuXiangInfo()
        {
            StringBuilder builder = new StringBuilder();
            string returnResult = string.Empty;
            try
            {
                string orderStr = (base.Request.Form["order"] != null) ? base.Request.Form["order"].ToString() : "asc";
                int pageNumber = (base.Request.Form["page"] != null) ? int.Parse(base.Request.Form["page"].ToString()) : 1;
                int pageSize = (base.Request.Form["rows"] != null) ? int.Parse(base.Request.Form["rows"].ToString()) : 15;
                string sortStr = (base.Request.Form["sort"] != null) ? base.Request.Form["sort"].ToString() : "Fcreate_time ";

                string accountNo = Request.Form["accountNo"] != null && !string.IsNullOrEmpty(Request.Form["accountNo"].ToString()) ? Request.Form["accountNo"].ToString() : string.Empty;
                int accountType = Request.Form["accountType"] != null && !string.IsNullOrEmpty(Request.Form["accountType"].ToString()) ? int.Parse(Request.Form["accountType"].ToString()) : 1;
                string refund_flow_id = Request.Form["refund_flow_id"] != null && !string.IsNullOrEmpty(Request.Form["refund_flow_id"].ToString()) ? Request.Form["refund_flow_id"].ToString() : string.Empty;
                string c_rchg_id = string.Empty;// Request.Form["c_rchg_id"] != null && !string.IsNullOrEmpty(Request.Form["c_rchg_id"].ToString()) ? Request.Form["c_rchg_id"].ToString() : string.Empty;
                //int billStatus = Request.Form["billStatus"] != null && !string.IsNullOrEmpty(Request.Form["billStatus"].ToString()) ? int.Parse(Request.Form["billStatus"].ToString()) : 1;
                string beginDate = Request.Form["beginDate"] != null && !string.IsNullOrEmpty(Request.Form["beginDate"].ToString()) ? Request.Form["beginDate"].ToString() : string.Empty;
                string endDate = Request.Form["endDate"] != null && !string.IsNullOrEmpty(Request.Form["endDate"].ToString()) ? Request.Form["endDate"].ToString() : string.Empty;
                string timeStamp = CommQuery.GetTimeStamp(); 
                TencentCreditService tencentCreditService = new TencentCreditService();
                bool searchResult = false;
                string errorMessage = string.Empty;
                int total = 0;

                if (classLibrary.getData.IsTestMode && !classLibrary.getData.IsNewSensitivePowerMode)
                {
                    returnResult = tencentCreditService.LoadRefundQuXiangInfo(accountNo, accountType, refund_flow_id,c_rchg_id, beginDate, endDate, timeStamp, pageSize, pageNumber, sortStr + " " + orderStr, out searchResult, out errorMessage, ref total);
                }
                else
                {
                    returnResult = tencentCreditService.LoadRefundQuXiangInfo(accountNo, accountType, refund_flow_id, c_rchg_id, beginDate, endDate, timeStamp, pageSize, pageNumber, sortStr + " " + orderStr, out searchResult, out errorMessage, ref total);
                }
                if (searchResult)
                {
                    builder.Append(returnResult);
                }
                else
                {
                    //builder.Append("[");
                    builder.Append("{");
                    builder.Append("\"result\":");
                    builder.Append("\"" + searchResult + "\",");
                    builder.Append("\"res_info\":");
                    builder.Append("\"" + errorMessage + "\"");
                    builder.Append("}");
                    // builder.Append("]");
                }
            }
            catch (Exception ex)
            {
                //builder.Append("[");
                builder.Append("{");
                builder.Append("\"result\":");
                builder.Append("\"False\",");
                builder.Append("\"res_info\":");
                builder.Append("\"查询出错了!\"");
                builder.Append("}");
                //builder.Append("]");
            }
            Response.Write(builder.ToString());
            Response.End();
        }
    }
}