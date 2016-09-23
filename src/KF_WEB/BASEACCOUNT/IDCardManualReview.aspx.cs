using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CFT.CSOMS.BLL.IdCardModule;
using commLib;
using SunLibrary;
namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    public partial class IDCardManualReview : System.Web.UI.Page//TENCENT.OSS.CFT.KF.KF_Web.PageBase
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
            else if (actionName.Equals("IsHaveRightForReviewCount"))
            {
                IsHaveRightForReviewCount();//获取是否有批量领取任务的权限
            }
            else if (actionName.Equals("IsHaveRightForSeeDetail"))
            {
                IsHaveRightForSeeDetail();//获取是否有查看详情的权限
            }
        }

        private void ReceiveReview()
        {
            bool result = false;
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
                    result = false;
                }
                else
                {
                    IdCardManualReviewService idCardManualReviewService = new IdCardManualReviewService();
                    bool receiveNeedReviewIdCardDataResult = idCardManualReviewService.ReceiveNeedReviewIdCardData(uid, uin, int.Parse(reviewCount), beginDate, endDate, out message);
                    result = receiveNeedReviewIdCardDataResult;
                }
            }
            catch (Exception ex)
            {
                message = "批量领单失败：" + ex.Message.ToString();
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            builder.Append("{");
            builder.Append("\"result\":");
            builder.Append("\"" + result + "\",");
            builder.Append("\"message\":");
            builder.Append("\"" + message + "\"");
            builder.Append("}");
            builder.Append("]");
            Response.Write(builder.ToString());
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
                    bool isHaveRightForSeeDetail = false;                    
                    try
                    {
                        isHaveRightForSeeDetail = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("IDCardManualReview_SeeDetail", this);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.LogInfo("IDCardManualReview.IsHaveRightForSeeDetail,获取查看详情权限失败:" + ex.ToString());
                    }

                    IdCardManualReviewService idCardManualReviewService = new IdCardManualReviewService();
                    int total = 0;
                    DataTable dt = new DataTable();
                    #region
#if DEBUG

                    dt.Columns.Add("Fid");
                    dt.Columns.Add("Fserial_number");
                    dt.Columns.Add("Fspid");
                    dt.Columns.Add("Fcreate_time");
                    dt.Columns.Add("Fuin");
                    dt.Columns.Add("Fname");
                    dt.Columns.Add("Fidentitycard");
                    dt.Columns.Add("Fmodify_time");
                    dt.Columns.Add("Fimage_path1");
                    dt.Columns.Add("Fimage_path2");
                    dt.Columns.Add("Fimage_file1");
                    dt.Columns.Add("Fimage_file2");
                    dt.Columns.Add("Fstate");
                    dt.Columns.Add("Fresult");
                    dt.Columns.Add("Fmemo");
                    dt.Columns.Add("Foperator");
                    dt.Columns.Add("Fstandby1");
                    dt.Columns.Add("Fstandby2");
                    dt.Columns.Add("Fstandby3");
                    dt.Columns.Add("Fstandby4");
                    dt.Columns.Add("Fstandby5");
                    dt.Columns.Add("TableName");
                    DataRow dr1 = dt.NewRow();
                    dr1["Fid"] = "12";
                    dr1["Fserial_number"] = "1147105501600000005";
                    dr1["Fspid"] = "2000000501";
                    dr1["Fcreate_time"] = "2016/8/13 9:12:24";
                    dr1["Fuin"] = "195806606";
                    dr1["Fname"] = "SEZyxazimzM=";
                    dr1["Fidentitycard"] = "TG/Jqoya4pb3/R3Qe2Vr0jSMNoV4R3Ju";
                    dr1["Fmodify_time"] = "2016/8/15 16:42:07";
                    dr1["Fimage_path1"] = "ent_no=10000003&req_data=FAD02DDB476FC071DDD0F0C4FB33885740C3B3181A70EDCD4EB5EADEAA88514643BB1F9AD403207242646194A636EB6AD5ED27BC462EF8063EDABCCB7C2DC624&seq_no=1147105501600000005";
                    dr1["Fimage_path2"] = "ent_no=10000003&req_data=FAD02DDB476FC071DDD0F0C4FB33885740C3B3181A70EDCD4EB5EADEAA88514643BB1F9AD403207242646194A636EB6AD5ED27BC462EF8063EDABCCB7C2DC624&seq_no=1147105501600000005";
                    dr1["Fimage_file1"] = "1-1-5-64-0-1-3016181900461";
                    dr1["Fimage_file2"] = "1-1-5-64-0-1-3016181900461";
                    dr1["Fstate"] = "2";
                    dr1["Fresult"] = "0";
                    dr1["Fmemo"] = "0";
                    dr1["Foperator"] = "1100000000";
                    dr1["Fstandby1"] = "";
                    dr1["Fstandby2"] = "";
                    dr1["Fstandby3"] = "";
                    dr1["Fstandby4"] = "";
                    dr1["Fstandby5"] = "";
                    dr1["TableName"] = "c2c_fmdb.t_check_identitycard_201608";
                    dt.Rows.Add(dr1);

                    DataRow dr2 = dt.NewRow();
                    dr2["Fid"] = "13";
                    dr2["Fserial_number"] = "1147105531500000006";
                    dr2["Fspid"] = "2000000501";
                    dr2["Fcreate_time"] = "2016/8/13 9:12:24";
                    dr2["Fuin"] = "195806606";
                    dr2["Fname"] = "SEZyxazimzM=";
                    dr2["Fidentitycard"] = "TG/Jqoya4pb3/R3Qe2Vr0jSMNoV4R3Ju";
                    dr2["Fmodify_time"] = "2016/8/15 16:42:07";
                    dr2["Fimage_path1"] = "ent_no=10000003&req_data=FAD02DDB476FC071DDD0F0C4FB33885740C3B3181A70EDCD4EB5EADEAA88514643BB1F9AD403207242646194A636EB6AD5ED27BC462EF8063EDABCCB7C2DC624&seq_no=1147105501600000005";
                    dr2["Fimage_path2"] = "ent_no=10000003&req_data=FAD02DDB476FC071DDD0F0C4FB33885740C3B3181A70EDCD4EB5EADEAA88514643BB1F9AD403207242646194A636EB6AD5ED27BC462EF8063EDABCCB7C2DC624&seq_no=1147105501600000005";
                    dr2["Fimage_file1"] = "1-1-5-64-0-1-3016181900461";
                    dr2["Fimage_file2"] = "1-1-5-64-0-1-3016181900461";
                    dr2["Fstate"] = "2";
                    dr2["Fresult"] = "0";
                    dr2["Fmemo"] = "0";
                    dr2["Foperator"] = "1100000000";
                    dr2["Fstandby1"] = "";
                    dr2["Fstandby2"] = "";
                    dr2["Fstandby3"] = "";
                    dr2["Fstandby4"] = "";
                    dr2["Fstandby5"] = "";
                    dr2["TableName"] = "c2c_fmdb.t_check_identitycard_201608";
                    dt.Rows.Add(dr2);

                    DataRow dr3 = dt.NewRow();
                    dr3["Fid"] = "12";
                    dr3["Fserial_number"] = "1147105531500000006";
                    dr3["Fspid"] = "2000000501";
                    dr3["Fcreate_time"] = "2016/8/13 9:12:24";
                    dr3["Fuin"] = "195806606";
                    dr3["Fname"] = "SEZyxazimzM=";
                    dr3["Fidentitycard"] = "TG/Jqoya4pb3/R3Qe2Vr0jSMNoV4R3Ju";
                    dr3["Fmodify_time"] = "2016/8/15 16:42:07";
                    dr3["Fimage_path1"] = "ent_no=10000003&req_data=FAD02DDB476FC071DDD0F0C4FB33885740C3B3181A70EDCD4EB5EADEAA88514643BB1F9AD403207242646194A636EB6AD5ED27BC462EF8063EDABCCB7C2DC624&seq_no=1147105501600000005";
                    dr3["Fimage_path2"] = "ent_no=10000003&req_data=FAD02DDB476FC071DDD0F0C4FB33885740C3B3181A70EDCD4EB5EADEAA88514643BB1F9AD403207242646194A636EB6AD5ED27BC462EF8063EDABCCB7C2DC624&seq_no=1147105501600000005";
                    dr3["Fimage_file1"] = "1-1-5-64-0-1-3016181900461";
                    dr3["Fimage_file2"] = "1-1-5-64-0-1-3016181900461";
                    dr3["Fstate"] = "2";
                    dr3["Fresult"] = "0";
                    dr3["Fmemo"] = "0";
                    dr3["Foperator"] = "1100000000";
                    dr3["Fstandby1"] = "";
                    dr3["Fstandby2"] = "";
                    dr3["Fstandby3"] = "";
                    dr3["Fstandby4"] = "";
                    dr3["Fstandby5"] = "";
                    dr3["TableName"] = "c2c_fmdb.t_check_identitycard_201608";
                    dt.Rows.Add(dr3);

#endif
                    #endregion
#if !DEBUG
                    dt = idCardManualReviewService.LoadReview(uid, uin, reviewStatus, reviewResult, beginDate, endDate,isHaveRightForSeeDetail, pageSize, pageNumber, str2 + " " + str, ref total);
#endif
                    message = JsonHelper.DataTableToJson(dt, total, true);
                    //message=" { "rows":[ { "Fid":"12","Fserial_number":"1147105501600000005","Fspid":"2000000501","Fcreate_time":"2016/8/13 9:12:24","Fuin":"195806606","Fname":"SEZyxazimzM=","Fidentitycard":"TG/Jqoya4pb3/R3Qe2Vr0jSMNoV4R3Ju","Fmodify_time":"2016/8/15 16:42:07","Fimage_path1":"ent_no=10000003&req_data=FAD02DDB476FC071DDD0F0C4FB33885740C3B3181A70EDCD4EB5EADEAA88514643BB1F9AD403207242646194A636EB6AD5ED27BC462EF8063EDABCCB7C2DC624&seq_no=1147105501600000005","Fimage_path2":"ent_no=10000003&req_data=FAD02DDB476FC071DDD0F0C4FB33885740C3B3181A70EDCD4EB5EADEAA88514643BB1F9AD403207242646194A636EB6AD5ED27BC462EF8063EDABCCB7C2DC624&seq_no=1147105501600000005","Fimage_file1":"1-1-5-64-0-1-3016181900461","Fimage_file2":"1-1-5-64-0-1-3016181900461","Fstate":"2","Fresult":"0","Fmemo":"0","Foperator":"1100000000","Fstandby1":"","Fstandby2":"","Fstandby3":"","Fstandby4":"","Fstandby5":"","TableName":"c2c_fmdb.t_check_identitycard_201608"}, { "Fid":"13","Fserial_number":"1147105531500000006","Fspid":"2000000501","Fcreate_time":"2016/8/13 9:12:24","Fuin":"195806606","Fname":"SEZyxazimzM=","Fidentitycard":"TG/Jqoya4pb3/R3Qe2Vr0jSMNoV4R3Ju","Fmodify_time":"2016/8/15 15:40:53","Fimage_path1":"ent_no=10000003&req_data=FAD02DDB476FC071DDD0F0C4FB33885740C3B3181A70EDCD4EB5EADEAA88514643BB1F9AD403207242646194A636EB6AD5ED27BC462EF8063EDABCCB7C2DC624&seq_no=1147105531500000006","Fimage_path2":"ent_no=10000003&req_data=FAD02DDB476FC071DDD0F0C4FB33885740C3B3181A70EDCD4EB5EADEAA88514643BB1F9AD403207242646194A636EB6AD5ED27BC462EF8063EDABCCB7C2DC624&seq_no=1147105531500000006","Fimage_file1":"1-1-5-64-0-1-3016181900461","Fimage_file2":"1-1-5-64-0-1-3016181900461","Fstate":"2","Fresult":"0","Fmemo":"4","Foperator":"1100000000","Fstandby1":"","Fstandby2":"","Fstandby3":"","Fstandby4":"","Fstandby5":"","TableName":"c2c_fmdb.t_check_identitycard_201608"}, { "Fid":"14","Fserial_number":"1234","Fspid":"1234567890","Fcreate_time":"2016/8/12 0:00:00","Fuin":"abcd@wx.tenpay.com","Fname":"guoyueqiang","Fidentitycard":"360726","Fmodify_time":"","Fimage_path1":"image_path1","Fimage_path2":"image_path2","Fimage_file1":"image_file1","Fimage_file2":"image_file2","Fstate":"1","Fresult":"0","Fmemo":"","Foperator":"","Fstandby1":"","Fstandby2":"","Fstandby3":"","Fstandby4":"","Fstandby5":"","TableName":"c2c_fmdb.t_check_identitycard_201608"} ],"total":3}";
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
            bool result = true;
            string message = string.Empty;
            try
            {

                string beginDate = Request.Form["beginDate"] != null || string.IsNullOrEmpty(Request.Form["beginDate"].ToString()) ? Request.Form["beginDate"].ToString() : string.Empty;
                string endDate = Request.Form["endDate"] != null || string.IsNullOrEmpty(Request.Form["endDate"].ToString()) ? Request.Form["endDate"].ToString() : string.Empty;
                int totalMonth = DateTime.Parse(endDate).Year * 12 + DateTime.Parse(endDate).Month - DateTime.Parse(beginDate).Year * 12 - DateTime.Parse(beginDate).Month;
                if (totalMonth >= 1)
                {
                    result = false;
                    message = string.Format("查询日期不能超过一个月");
                }
            }
            catch (Exception ex)
            {
                message = string.Format("查询日期输入出现错误:{0}", ex.Message.ToString());
                result = false;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            builder.Append("{");
            builder.Append("\"result\":");
            builder.Append("\"" + result + "\",");
            builder.Append("\"message\":");
            builder.Append("\"" + message + "\"");
            builder.Append("}");
            builder.Append("]");
            Response.Write(builder.ToString());
            Response.End();
        }

        private void Decryptor()
        {            
            bool result = false;
            string message = string.Empty;
            try
            {
                string decryptorStr = Request.Form["DecryptorStr"] != null || string.IsNullOrEmpty(Request.Form["DecryptorStr"].ToString()) ? Request.Form["DecryptorStr"].ToString() : string.Empty;
                if (!string.IsNullOrEmpty(decryptorStr))
                {
                    BankLib.BankIOX aa = new BankLib.BankIOX();
                    message = aa.Decryptor(decryptorStr, "PWMXbxkk98N62W1lxnixJtMy");//测试环境key:abcdefghjklmn12345678912
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                message = "解密出错,请联系相关工作人员。";
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            builder.Append("{");
            builder.Append("\"result\":");
            builder.Append("\"" + result + "\",");
            builder.Append("\"message\":");
            builder.Append("\"" + message + "\"");
            builder.Append("}");
            builder.Append("]");
            Response.Write(builder.ToString());
            Response.End();
        }

        private void SaveReview()
        {
            bool result = false;
            string returnMessage = string.Empty;
            string message = string.Empty;
            try
            {
                #region
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
                    LogHelper.LogInfo("IDCardManualReview.SaveReview,updateResult:更新成功");
                    DataTable dt = new DataTable();
                    dt = idCardManualReviewService.LoadReview(fid, fserial_number, tableName);
                    if (dt != null)
                    {
                        string uin = dt.Rows[0]["Fuin"] != null || dt.Rows[0]["Fuin"] != DBNull.Value ? dt.Rows[0]["Fuin"].ToString() : string.Empty;
                        //string uid = string.Empty;// dt.Rows[0]["Fuin"] != null || dt.Rows[0]["Fuin"] != DBNull.Value ? dt.Rows[0]["Fuin"].ToString() : string.Empty;                        
                        string seq_no = dt.Rows[0]["Fserial_number"] != null || dt.Rows[0]["Fserial_number"] != DBNull.Value ? dt.Rows[0]["Fserial_number"].ToString() : string.Empty;//Guid.NewGuid().ToString("N").Substring(0, 30);// "1147099249300000001";// 
                        string credit_spid = dt.Rows[0]["Fspid"] != null || dt.Rows[0]["Fspid"] != DBNull.Value ? dt.Rows[0]["Fspid"].ToString() : string.Empty;
                        string front_image = dt.Rows[0]["Fimage_file1"] != null || dt.Rows[0]["Fimage_file1"] != DBNull.Value ? dt.Rows[0]["Fimage_file1"].ToString() : string.Empty;
                        string back_image = dt.Rows[0]["Fimage_file2"] != null || dt.Rows[0]["Fimage_file2"] != DBNull.Value ? dt.Rows[0]["Fimage_file2"].ToString() : string.Empty;
                        int audit_result = dt.Rows[0]["Fresult"] != null || dt.Rows[0]["Fresult"] != DBNull.Value ? int.Parse(PublicRes.GetInt(dt.Rows[0]["Fresult"].ToString())) : 0;
                        string audit_error_des = dt.Rows[0]["Fmemo"] != null || dt.Rows[0]["Fmemo"] != DBNull.Value ? dt.Rows[0]["Fmemo"].ToString() : string.Empty;
                        string audit_operator = dt.Rows[0]["Foperator"] != null || dt.Rows[0]["Foperator"] != DBNull.Value ? dt.Rows[0]["Foperator"].ToString() : string.Empty;
                        string audit_time = dt.Rows[0]["Fmodify_time"] != null || dt.Rows[0]["Fmodify_time"] != DBNull.Value ? dt.Rows[0]["Fmodify_time"].ToString() : string.Empty;

                        string kf_auth_ocr_audit_LoadAPIType = System.Configuration.ConfigurationManager.AppSettings["kf_auth_ocr_audit_LoadAPIType"] != null ? System.Configuration.ConfigurationManager.AppSettings["kf_auth_ocr_audit_LoadAPIType"].ToString() : "cji";//调用接口的方式，cji方式或者relay 值为 cji,relay
                        if (!string.IsNullOrEmpty(kf_auth_ocr_audit_LoadAPIType))
                        {
                            bool sendResult = false;
                            if (kf_auth_ocr_audit_LoadAPIType.Equals("cji"))
                            {
                                sendResult = idCardManualReviewService.Review(uin, seq_no, credit_spid, front_image, back_image, audit_result, audit_error_des, audit_operator, audit_time, out message);
                            }
                            else if (kf_auth_ocr_audit_LoadAPIType.Equals("relay"))
                            {
                                sendResult = idCardManualReviewService.ReviewByRelay(uin, seq_no, credit_spid, front_image, back_image, audit_result, audit_error_des, audit_operator, audit_time, out message);
                            }
                            int fstate = sendResult == true ? 4 : 3;//3=推送到实名系统失败;4=推送成功
                            //接口调用后 更新该条数据的审核状态
                            bool updateState = idCardManualReviewService.UpdateFstate(fserial_number, fid, fstate, tableName);
                            result = updateState;
                            returnMessage = "身份证影印件审核" + (reviewResult == 1 ? "[通过]" : "[拒绝]") + (updateState == true ? "成功" : "失败") + "<br/>" + message;
                        }
                    }
                    else
                    {
                        result = false;
                        LogHelper.LogInfo("IdCardManualReviewService.Review,message:" + message);
                    }

                }
                #endregion

            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("IDCardManualReview.SaveReview,提交失败:" + ex.Message);
                result = false;
                message = "审核失败:" + ex.Message.ToString();
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            builder.Append("{");
            builder.Append("\"result\":");
            builder.Append("\"" + result + "\",");
            builder.Append("\"message\":");
            builder.Append("\"" + returnMessage + "\"");
            builder.Append("}");
            builder.Append("]");
            Response.Write(builder.ToString());
            Response.End();
        }

        private void ReSend()
        {
            bool result = false;
            string message = string.Empty;
            string returnMessage = string.Empty;
            try
            {
                #region
                string tableName = Request.Form["TableName"] != null || string.IsNullOrEmpty(Request.Form["TableName"].ToString()) ? Request.Form["TableName"].ToString() : string.Empty;
                string fserial_number = Request.Form["Fserial_number"] != null || string.IsNullOrEmpty(Request.Form["Fserial_number"].ToString()) ? Request.Form["Fserial_number"].ToString() : string.Empty;

                int fid = Request.Form["Fid"] != null || string.IsNullOrEmpty(Request.Form["Fid"].ToString()) ? int.Parse(Request.Form["Fid"].ToString()) : 0;
                int reviewResult = Request.Form["reviewResult"] != null || string.IsNullOrEmpty(Request.Form["reviewResult"].ToString()) ? int.Parse(Request.Form["reviewResult"].ToString()) : 0;
                IdCardManualReviewService idCardManualReviewService = new IdCardManualReviewService();
                DataTable dt = new DataTable();
                dt = idCardManualReviewService.LoadReview(fid, fserial_number, tableName);
                if (dt != null)
                {
                    string uin = dt.Rows[0]["Fuin"] != null || dt.Rows[0]["Fuin"] != DBNull.Value ? dt.Rows[0]["Fuin"].ToString() : string.Empty;
                    string seq_no = dt.Rows[0]["Fserial_number"] != null || dt.Rows[0]["Fserial_number"] != DBNull.Value ? dt.Rows[0]["Fserial_number"].ToString() : string.Empty;// "1147099249300000001";
                    string credit_spid = dt.Rows[0]["Fspid"] != null || dt.Rows[0]["Fspid"] != DBNull.Value ? dt.Rows[0]["Fspid"].ToString() : string.Empty;
                    string front_image = dt.Rows[0]["Fimage_file1"] != null || dt.Rows[0]["Fimage_file1"] != DBNull.Value ? dt.Rows[0]["Fimage_file1"].ToString() : string.Empty;
                    string back_image = dt.Rows[0]["Fimage_file2"] != null || dt.Rows[0]["Fimage_file2"] != DBNull.Value ? dt.Rows[0]["Fimage_file2"].ToString() : string.Empty;
                    int audit_result = dt.Rows[0]["Fresult"] != null || dt.Rows[0]["Fresult"] != DBNull.Value ? int.Parse(PublicRes.GetInt(dt.Rows[0]["Fresult"].ToString())) : 0;
                    string audit_error_des = dt.Rows[0]["Fmemo"] != null || dt.Rows[0]["Fmemo"] != DBNull.Value ? dt.Rows[0]["Fmemo"].ToString() : string.Empty;
                    string audit_operator = dt.Rows[0]["Foperator"] != null || dt.Rows[0]["Foperator"] != DBNull.Value ? dt.Rows[0]["Foperator"].ToString() : string.Empty;
                    string audit_time = dt.Rows[0]["Fmodify_time"] != null || dt.Rows[0]["Fmodify_time"] != DBNull.Value ? dt.Rows[0]["Fmodify_time"].ToString() : string.Empty;
                    string kf_auth_ocr_audit_LoadAPIType = System.Configuration.ConfigurationManager.AppSettings["kf_auth_ocr_audit_LoadAPIType"] != null ? System.Configuration.ConfigurationManager.AppSettings["kf_auth_ocr_audit_LoadAPIType"].ToString() : "cji";//调用接口的方式，cji方式或者relay 值为 cji,relay
                    if (!string.IsNullOrEmpty(kf_auth_ocr_audit_LoadAPIType))
                    {
                        bool sendResult = false;
                        if (kf_auth_ocr_audit_LoadAPIType.Equals("cji"))
                        {
                            sendResult = idCardManualReviewService.Review(uin, seq_no, credit_spid, front_image, back_image, audit_result, audit_error_des, audit_operator, audit_time, out message);
                        }
                        else if (kf_auth_ocr_audit_LoadAPIType.Equals("relay"))
                        {
                            sendResult = idCardManualReviewService.ReviewByRelay(uin, seq_no, credit_spid, front_image, back_image, audit_result, audit_error_des, audit_operator, audit_time, out message);
                        }
                        int fstate = sendResult == true ? 4 : 3;//3=推送到实名系统失败;4=推送成功
                        //接口调用后 更新该条数据的审核状态
                        bool updateState = idCardManualReviewService.UpdateFstate(fserial_number, fid, fstate, tableName);
                        result = updateState;
                        //message = message + "<br/>" + (sendResult == true ? "推送到实名系统成功" : "推送到实名系统失败");
                        returnMessage = "身份证影印件审核" + (reviewResult == 1 ? "[通过]" : "[拒绝]") + (updateState == true ? "成功" : "失败") + "<br/>" + message;
                    }
                }
                else
                {
                    result = false;
                    message = string.Format("系统中不存在流水号为[{0}]的身份证影印件待审数据", fserial_number);
                }
                #endregion

            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("IDCardManualReview.ReSend,重新提交失败:" + ex.Message);
                result = false;
                message = "审核失败:" + ex.Message.ToString();
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            builder.Append("{");
            builder.Append("\"result\":");
            builder.Append("\"" + result + "\",");
            builder.Append("\"message\":");
            builder.Append("\"" + returnMessage + "\"");
            builder.Append("}");
            builder.Append("]");
            Response.Write(builder.ToString());
            Response.End();
        }

        private void IsHaveRightForReviewCount()
        {
            bool result = false;
            string message = string.Empty;
            try
            {
                result = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("IDCardManualReview_ReviewCount", this);
            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("IDCardManualReview.IsHaveRightForReviewCount,获取批量领单权限失败:" + ex.ToString());
                result = false;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            builder.Append("{");
            builder.Append("\"result\":");
            builder.Append("\"" + result + "\",");
            builder.Append("\"message\":");
            builder.Append("\"" + message + "\"");
            builder.Append("}");
            builder.Append("]");
            Response.Write(builder.ToString());
            Response.End();
        }

        private void IsHaveRightForSeeDetail()
        {
            bool result = false;
            string message = string.Empty;
            try
            {
                result = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("IDCardManualReview_SeeDetail", this);
            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("IDCardManualReview.IsHaveRightForSeeDetail,获取查看详情权限失败:" + ex.ToString());
                result = false;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            builder.Append("{");
            builder.Append("\"result\":");
            builder.Append("\"" + result + "\",");
            builder.Append("\"message\":");
            builder.Append("\"" + message + "\"");
            builder.Append("}");
            builder.Append("]");
            Response.Write(builder.ToString());
            Response.End();
        }
    }
}