﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CFT.CSOMS.BLL.IdCardModule;
namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    public partial class IDCardManualReview : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    this.Label_uid.Text = Session["uid"].ToString();
                    hid_IdCaredServerPath.Value = System.Configuration.ConfigurationManager.AppSettings["IDCardManualReview_zx_sm_get_imagefcgi"].ToString();
                    string szkey = Session["SzKey"].ToString();
                    if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
                }
                catch  //如果没有登陆或者没有权限就跳出
                {
                    Response.Redirect("../login.aspx?wh=1");
                }
            }
            string getAction = Request.QueryString["getAction"] != null ? Request.QueryString["getAction"].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(getAction))
            {
                Action(getAction);
            }
        }
        public void Action(string actionName)
        {
            if (actionName.Equals("ReceiveReview"))
            {
                ReceiveReview();//批量领单
            }
            else if (actionName.Equals("LoadReview"))
            {
                LoadReview();//查询
            }
            else if (actionName.Equals("SaveReview"))
            {
                SaveReview();//人工审核
            }
            else if (actionName.Equals("ReSend"))
            {
                ReSend();
            }
            else if (actionName.Equals("CheckDate"))
            {
                CheckDate();
            }
            else if (actionName.Equals("Decryptor"))
            {
                Decryptor();
            }
        }

        private void ReceiveReview()
        {
            string message = string.Empty;
            try
            {
                string uid = this.Label_uid.Text;//Request.Form["uid"] != null || string.IsNullOrEmpty(Request.Form["uid"].ToString()) ? Request.Form["uid"].ToString() : string.Empty;
                string uin = Request.Form["uin"] != null || string.IsNullOrEmpty(Request.Form["uin"].ToString()) ? Request.Form["uin"].ToString() : string.Empty;
                string reviewCount = Request.Form["reviewCount"] != null || string.IsNullOrEmpty(Request.Form["reviewCount"].ToString()) ? Request.Form["reviewCount"].ToString() : "50";
                string beginDate = Request.Form["beginDate"] != null || string.IsNullOrEmpty(Request.Form["beginDate"].ToString()) ? Request.Form["beginDate"].ToString() : string.Empty;
                string endDate = Request.Form["endDate"] != null || string.IsNullOrEmpty(Request.Form["endDate"].ToString()) ? Request.Form["endDate"].ToString() : string.Empty;
                int totalMonth = DateTime.Parse(endDate).Year * 12 + DateTime.Parse(endDate).Month - DateTime.Parse(beginDate).Year * 12 - DateTime.Parse(beginDate).Month;
                if (totalMonth >= 1)
                {
                    message = string.Format("查询日期不能超过一个月");
                }
                else
                {
                    IdCardManualReviewService idCardManualReviewService = new IdCardManualReviewService();
                    bool aaa = idCardManualReviewService.ReceiveNeedReviewIdCardData(uid, uin, int.Parse(reviewCount), beginDate, endDate, out message);
                }
            }
            catch (Exception ex)
            {
                message = "批量领单失败：" + ex.Message.ToString();
            }
            Response.Write(message);
            Response.End();
        }

        private void LoadReview()
        {
            string message = string.Empty;
            try
            {
                string str = (base.Request.Form["order"] != null) ? base.Request.Form["order"].ToString() : "asc";
                int pageNumber = (base.Request.Form["page"] != null) ? int.Parse(base.Request.Form["page"].ToString()) : 1;
                int pageSize = (base.Request.Form["rows"] != null) ? int.Parse(base.Request.Form["rows"].ToString()) : 15;
                string str2 = (base.Request.Form["sort"] != null) ? base.Request.Form["sort"].ToString() : "Fcreate_time ";

                string uid = this.Label_uid.Text;// Request.Form["uid"] != null || string.IsNullOrEmpty(Request.Form["uid"].ToString()) ? Request.Form["uid"].ToString() : string.Empty;
                string uin = Request.Form["uin"] != null || string.IsNullOrEmpty(Request.Form["uin"].ToString()) ? Request.Form["uin"].ToString() : string.Empty;
                int reviewStatus = Request.Form["reviewStatus"] != null || string.IsNullOrEmpty(Request.Form["reviewStatus"].ToString()) ? int.Parse(Request.Form["reviewStatus"].ToString()) : 1;
                int reviewResult = Request.Form["reviewResult"] != null || string.IsNullOrEmpty(Request.Form["reviewResult"].ToString()) ? int.Parse(Request.Form["reviewResult"].ToString()) : 0;
                string beginDate = Request.Form["beginDate"] != null || string.IsNullOrEmpty(Request.Form["beginDate"].ToString()) ? Request.Form["beginDate"].ToString() : string.Empty;
                string endDate = Request.Form["endDate"] != null || string.IsNullOrEmpty(Request.Form["endDate"].ToString()) ? Request.Form["endDate"].ToString() : string.Empty;
                int totalMonth = DateTime.Parse(endDate).Year * 12 + DateTime.Parse(endDate).Month - DateTime.Parse(beginDate).Year * 12 - DateTime.Parse(beginDate).Month;
                if (totalMonth >= 1)
                {
                    message = string.Format("查询日期不能超过一个月");
                }
                else
                {
                    IdCardManualReviewService idCardManualReviewService = new IdCardManualReviewService();
                    int total = 0;
                    DataTable dt = new DataTable();
                    dt = idCardManualReviewService.LoadReview(uid, uin, reviewStatus, reviewResult, beginDate, endDate, pageSize, pageNumber, str2 + " " + str, ref total);
                    message = GetJsonFromDataTable(dt, total, true);

                }
            }
            catch (Exception ex)
            {

            }
            Response.Write(message);
            Response.End();
        }


        private void CheckDate()
        {
            string message = string.Empty;
            try
            {
               
                string beginDate = Request.Form["beginDate"] != null || string.IsNullOrEmpty(Request.Form["beginDate"].ToString()) ? Request.Form["beginDate"].ToString() : string.Empty;
                string endDate = Request.Form["endDate"] != null || string.IsNullOrEmpty(Request.Form["endDate"].ToString()) ? Request.Form["endDate"].ToString() : string.Empty;
                int totalMonth = DateTime.Parse(endDate).Year * 12 + DateTime.Parse(endDate).Month - DateTime.Parse(beginDate).Year * 12 - DateTime.Parse(beginDate).Month;
                if (totalMonth >= 1)
                {
                    message = string.Format("查询日期不能超过一个月");
                }               
            }
            catch (Exception ex)
            {
                message = string.Empty;
            }
            Response.Write(message);
            Response.End();
        }

        private void Decryptor()
        {
            string message = string.Empty;
            try
            {
                string decryptorStr = Request.Form["DecryptorStr"] != null || string.IsNullOrEmpty(Request.Form["DecryptorStr"].ToString()) ? Request.Form["DecryptorStr"].ToString() : string.Empty;
                if (!string.IsNullOrEmpty(decryptorStr))
                {
                    BankLib.BankIOX aa = new BankLib.BankIOX();
                    message = aa.Decryptor(decryptorStr, "PWMXbxkk98N62W1lxnixJtMy");//测试环境key:abcdefghjklmn12345678912
                }                
            }
            catch (Exception ex)
            {
                message = "<span style='color:red'>解密出错,请联系相关工作人员。</span>";
            }
            Response.Write(message);
            Response.End();
        }
        
        private void SaveReview()
        {
            string message = string.Empty;
            try
            {
                string foperator = this.Label_uid.Text;//
                string Fuin = Request.Form["Fuin"] != null || string.IsNullOrEmpty(Request.Form["Fuin"].ToString()) ? Request.Form["Fuin"].ToString() : string.Empty;
                string memo = Request.Form["Fmemo"] != null || string.IsNullOrEmpty(Request.Form["Fmemo"].ToString()) ? Request.Form["Fmemo"].ToString() : string.Empty;
                string tableName = Request.Form["TableName"] != null || string.IsNullOrEmpty(Request.Form["TableName"].ToString()) ? Request.Form["TableName"].ToString() : string.Empty;
                string fserial_number = Request.Form["Fserial_number"] != null || string.IsNullOrEmpty(Request.Form["Fserial_number"].ToString()) ? Request.Form["Fserial_number"].ToString() : string.Empty;

                int fid = Request.Form["Fid"] != null || string.IsNullOrEmpty(Request.Form["Fid"].ToString()) ? int.Parse(Request.Form["Fid"].ToString()) : 0;
                int reviewResult = Request.Form["reviewResult"] != null || string.IsNullOrEmpty(Request.Form["reviewResult"].ToString()) ? int.Parse(Request.Form["reviewResult"].ToString()) : 0;
                IdCardManualReviewService idCardManualReviewService = new IdCardManualReviewService();
                bool updateResult = idCardManualReviewService.Update(fserial_number, fid, reviewResult, memo, tableName, foperator, out  message);
                if (updateResult)
                {
                    DataTable dt = new DataTable();
                    dt = idCardManualReviewService.LoadReview(fid, fserial_number, tableName);
                    if (dt != null)
                    {
                        // IdCardManualReviewService aaa = new IdCardManualReviewService();
                        //string uin="201311079024139@wx.tenpay.com";
                        // uid="299708515";
                        // string seq_no="1147099249300000001";
                        // string credit_spid="10000003";
                        // string front_image="201604251427591702311";
                        // string back_image="201604251427591702312";
                        // int audit_result=1;
                        // string audit_error_des="image not clear";
                        // string audit_operator="heidizhang";
                        // string audit_time = "2016-08-11 10:00:00";
                        // string sign="edf3ea3fd7d7610188acb1a7fc1433f8";
                        // string result = aaa.Review( uin,uid,  seq_no,  credit_spid,  front_image,  back_image,  audit_result,  audit_error_des,  audit_operator,  audit_time);

                        string uin = dt.Rows[0]["Fuin"] != null || dt.Rows[0]["Fuin"] != DBNull.Value ? dt.Rows[0]["Fuin"].ToString() : string.Empty;
                        string uid = string.Empty;// dt.Rows[0]["Fuin"] != null || dt.Rows[0]["Fuin"] != DBNull.Value ? dt.Rows[0]["Fuin"].ToString() : string.Empty;                        
                        string seq_no = Guid.NewGuid().ToString("N").Substring(0, 30);// "1147099249300000001";// dt.Rows[0]["Fuin"] != null || dt.Rows[0]["Fuin"] != DBNull.Value ? dt.Rows[0]["Fuin"].ToString() : string.Empty;
                        string credit_spid = dt.Rows[0]["Fspid"] != null || dt.Rows[0]["Fspid"] != DBNull.Value ? dt.Rows[0]["Fspid"].ToString() : string.Empty;
                        string front_image = dt.Rows[0]["Fimage_file1"] != null || dt.Rows[0]["Fimage_file1"] != DBNull.Value ? dt.Rows[0]["Fimage_file1"].ToString() : string.Empty;
                        string back_image = dt.Rows[0]["Fimage_file2"] != null || dt.Rows[0]["Fimage_file2"] != DBNull.Value ? dt.Rows[0]["Fimage_file2"].ToString() : string.Empty;
                        int audit_result = dt.Rows[0]["Fresult"] != null || dt.Rows[0]["Fresult"] != DBNull.Value ? int.Parse(PublicRes.GetInt(dt.Rows[0]["Fresult"].ToString())) : 0;
                        string audit_error_des = dt.Rows[0]["Fmemo"] != null || dt.Rows[0]["Fmemo"] != DBNull.Value ? dt.Rows[0]["Fmemo"].ToString() : string.Empty;
                        string audit_operator = dt.Rows[0]["Foperator"] != null || dt.Rows[0]["Foperator"] != DBNull.Value ? dt.Rows[0]["Foperator"].ToString() : string.Empty;
                        string audit_time = dt.Rows[0]["Fmodify_time"] != null || dt.Rows[0]["Fmodify_time"] != DBNull.Value ? dt.Rows[0]["Fmodify_time"].ToString() : string.Empty;
                        bool sendResult = idCardManualReviewService.Review(uin, uid, seq_no, credit_spid, front_image, back_image, audit_result, audit_error_des, audit_operator, audit_time, out message);
                        if (!sendResult)
                        { 
                            //接口调用失败 向表中写入状态
                            bool updateState = idCardManualReviewService.UpdateFstate(fserial_number, fid, 3, tableName, out  message);                            
                        }
                    }
                    else
                    {
                        message = "没有该数据";
                    }

                }
            }
            catch (Exception ex)
            {

            }
            Response.Write(message);
            Response.End();
        }


        private void ReSend()
        {
            string message = string.Empty;
            try
            {
                //string foperator = this.Label_uid.Text;//
                //string Fuin = Request.Form["Fuin"] != null || string.IsNullOrEmpty(Request.Form["Fuin"].ToString()) ? Request.Form["Fuin"].ToString() : string.Empty;                
                string tableName = Request.Form["TableName"] != null || string.IsNullOrEmpty(Request.Form["TableName"].ToString()) ? Request.Form["TableName"].ToString() : string.Empty;
                string Fserial_number = Request.Form["Fserial_number"] != null || string.IsNullOrEmpty(Request.Form["Fserial_number"].ToString()) ? Request.Form["Fserial_number"].ToString() : string.Empty;

                int fid = Request.Form["Fid"] != null || string.IsNullOrEmpty(Request.Form["Fid"].ToString()) ? int.Parse(Request.Form["Fid"].ToString()) : 0;
                int reviewResult = Request.Form["reviewResult"] != null || string.IsNullOrEmpty(Request.Form["reviewResult"].ToString()) ? int.Parse(Request.Form["reviewResult"].ToString()) : 0;
                IdCardManualReviewService idCardManualReviewService = new IdCardManualReviewService();
                DataTable dt = new DataTable();
                dt = idCardManualReviewService.LoadReview(fid, Fserial_number, tableName);
                if (dt != null)
                {
                    // IdCardManualReviewService aaa = new IdCardManualReviewService();
                    //string uin="201311079024139@wx.tenpay.com";
                    // uid="299708515";
                    // string seq_no="1147099249300000001";
                    // string credit_spid="10000003";
                    // string front_image="201604251427591702311";
                    // string back_image="201604251427591702312";
                    // int audit_result=1;
                    // string audit_error_des="image not clear";
                    // string audit_operator="heidizhang";
                    // string audit_time = "2016-08-11 10:00:00";
                    // string sign="edf3ea3fd7d7610188acb1a7fc1433f8";
                    // string result = aaa.Review( uin,uid,  seq_no,  credit_spid,  front_image,  back_image,  audit_result,  audit_error_des,  audit_operator,  audit_time);

                    string uin = dt.Rows[0]["Fuin"] != null || dt.Rows[0]["Fuin"] != DBNull.Value ? dt.Rows[0]["Fuin"].ToString() : string.Empty;
                    string uid = string.Empty;// dt.Rows[0]["Fuin"] != null || dt.Rows[0]["Fuin"] != DBNull.Value ? dt.Rows[0]["Fuin"].ToString() : string.Empty;
                    string seq_no = Guid.NewGuid().ToString("N").Substring(0,30); //"1147099249300000001";// dt.Rows[0]["Fuin"] != null || dt.Rows[0]["Fuin"] != DBNull.Value ? dt.Rows[0]["Fuin"].ToString() : string.Empty;
                    string credit_spid = dt.Rows[0]["Fspid"] != null || dt.Rows[0]["Fspid"] != DBNull.Value ? dt.Rows[0]["Fspid"].ToString() : string.Empty;
                    string front_image = dt.Rows[0]["Fimage_file1"] != null || dt.Rows[0]["Fimage_file1"] != DBNull.Value ? dt.Rows[0]["Fimage_file1"].ToString() : string.Empty;
                    string back_image = dt.Rows[0]["Fimage_file2"] != null || dt.Rows[0]["Fimage_file2"] != DBNull.Value ? dt.Rows[0]["Fimage_file2"].ToString() : string.Empty;
                    int audit_result = dt.Rows[0]["Fresult"] != null || dt.Rows[0]["Fresult"] != DBNull.Value ? int.Parse(PublicRes.GetInt(dt.Rows[0]["Fresult"].ToString())) : 0;
                    string audit_error_des = dt.Rows[0]["Fmemo"] != null || dt.Rows[0]["Fmemo"] != DBNull.Value ? dt.Rows[0]["Fmemo"].ToString() : string.Empty;
                    string audit_operator = dt.Rows[0]["Foperator"] != null || dt.Rows[0]["Foperator"] != DBNull.Value ? dt.Rows[0]["Foperator"].ToString() : string.Empty;
                    string audit_time = dt.Rows[0]["Fmodify_time"] != null || dt.Rows[0]["Fmodify_time"] != DBNull.Value ? dt.Rows[0]["Fmodify_time"].ToString() : string.Empty;
                    bool sendResult = idCardManualReviewService.Review(uin, uid, seq_no, credit_spid, front_image, back_image, audit_result, audit_error_des, audit_operator, audit_time, out message);
                    if (sendResult)
                    {
                        //接口调用失败 向表中写入状态
                        bool updateState = idCardManualReviewService.UpdateFstate(Fserial_number, fid, 4, tableName, out  message);
                    }
                }
                else
                {
                    message = "没有该数据";
                }
            }
            catch (Exception ex)
            {
                message = "重新提交失败";
            }
            Response.Write(message);
            Response.End();
        }
     
        public string GetJsonFromDataTable(DataTable dt, int total, bool isshowtotal = true)
        {
            StringBuilder builder = new StringBuilder();
            if (dt == null || dt.Rows.Count == 0)
            {
                builder.Append("{ ");
                builder.Append("\"rows\":[ ");
                builder.Append("]");
                if (isshowtotal)
                {
                    builder.Append(",");
                    builder.Append("\"total\":");
                    builder.Append(total);
                }
                builder.Append("}");
                return builder.ToString();
            }
            builder.Append("{ ");
            builder.Append("\"rows\":[ ");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                builder.Append("{ ");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (j < (dt.Columns.Count - 1))
                    {
                        builder.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + this.JsonCharFilter(dt.Rows[i][j].ToString()) + "\",");
                    }
                    else if (j == (dt.Columns.Count - 1))
                    {
                        builder.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + this.JsonCharFilter(dt.Rows[i][j].ToString()) + "\"");
                    }
                }
                if (i == (dt.Rows.Count - 1))
                {
                    builder.Append("} ");
                }
                else
                {
                    builder.Append("}, ");
                }
            }
            builder.Append("]");
            if (isshowtotal)
            {
                builder.Append(",");
                builder.Append("\"total\":");
                builder.Append(total);
            }
            builder.Append("}");
            return builder.ToString();
        }



        public string JsonCharFilter(string sourceStr)
        {
            sourceStr = sourceStr.Replace(@"\", @"\\");
            sourceStr = sourceStr.Replace("\"", "\\\"");
            sourceStr = sourceStr.Replace("\b", @"\b");
            sourceStr = sourceStr.Replace("\t", @"\t");
            sourceStr = sourceStr.Replace("\n", @"\n");
            sourceStr = sourceStr.Replace("\f", @"\f");
            sourceStr = sourceStr.Replace("\r", @"\r");
            sourceStr = sourceStr.Replace("\r\n", "");
            sourceStr = sourceStr.Replace("（", "(");
            sourceStr = sourceStr.Replace("）", ")");
            return sourceStr;
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            IdCardManualReviewService idCardManualReviewService = new IdCardManualReviewService();
            try
            {
            }
            catch (Exception ex)
            {

            }

        }
    }
}