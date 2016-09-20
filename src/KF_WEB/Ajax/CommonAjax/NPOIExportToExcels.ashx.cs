using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using CFT.CSOMS.BLL.IdCardModule;
using commLib;

namespace TENCENT.OSS.CFT.KF.KF_Web.Ajax.CommonAjax
{
    /// <summary>
    /// Summary description for NPOIExportToExcels
    /// </summary>
    public class NPOIExportToExcels : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string getAction = context.Request.QueryString["getAction"] != null || string.IsNullOrEmpty(context.Request.QueryString["getAction"].ToString()) ? context.Request.QueryString["getAction"].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(getAction))
            {
                Action(getAction, context);
            }
        }

        public void Action(string actionName, HttpContext context)
        {
            if (actionName.Equals("ExportIDCardManualReviewDataToExcel"))
            {
                ExportIDCardManualReviewDataToExcel(context);
            }

        }
        private void ExportIDCardManualReviewDataToExcel(HttpContext context)
        {
            string message = string.Empty;
            try
            {
                string str = (context.Request.QueryString["order"] != null) ? context.Request.QueryString["order"].ToString() : "asc";
                //int pageNumber = (context.Request.QueryString["page"] != null) ? int.Parse(context.Request.QueryString["page"].ToString()) : 1;
                //int pageSize = (context.Request.QueryString["rows"] != null) ? int.Parse(context.Request.QueryString["rows"].ToString()) : 15;
                string str2 = (context.Request.QueryString["sort"] != null) ? context.Request.QueryString["sort"].ToString() : "Fcreate_time ";

                string uid = context.Request.QueryString["uid"] != null || string.IsNullOrEmpty(context.Request.QueryString["uid"].ToString()) ? context.Request.QueryString["uid"].ToString() : string.Empty;
                string uin = context.Request.QueryString["uin"] != null || string.IsNullOrEmpty(context.Request.QueryString["uin"].ToString()) ? context.Request.QueryString["uin"].ToString() : string.Empty;
                int reviewStatus = context.Request.QueryString["reviewStatus"] != null || string.IsNullOrEmpty(context.Request.QueryString["reviewStatus"].ToString()) ? int.Parse(context.Request.QueryString["reviewStatus"].ToString()) : 1;
                int reviewResult = context.Request.QueryString["reviewResult"] != null || string.IsNullOrEmpty(context.Request.QueryString["reviewResult"].ToString()) ? int.Parse(context.Request.QueryString["reviewResult"].ToString()) : 0;
                string beginDate = context.Request.QueryString["beginDate"] != null || string.IsNullOrEmpty(context.Request.QueryString["beginDate"].ToString()) ? context.Request.QueryString["beginDate"].ToString() : string.Empty;
                string endDate = context.Request.QueryString["endDate"] != null || string.IsNullOrEmpty(context.Request.QueryString["endDate"].ToString()) ? context.Request.QueryString["endDate"].ToString() : string.Empty;
                int totalMonth = DateTime.Parse(endDate).Year * 12 + DateTime.Parse(endDate).Month - DateTime.Parse(beginDate).Year * 12 - DateTime.Parse(beginDate).Month;
                if (totalMonth >= 1)
                {
                    bool result = false;
                    message = string.Format("查询日期不能超过一个月");
                    StringBuilder builder = new StringBuilder();
                    builder.Append("[");
                    builder.Append("{");
                    builder.Append("\"result\":");
                    builder.Append("\"" + result + "\",");
                    builder.Append("\"message\":");
                    builder.Append("\"" + message + "\"");
                    builder.Append("}");
                    builder.Append("]");
                    context.Response.Write(builder.ToString());
                    context.Response.End();
                }
                else
                {
                    IdCardManualReviewService idCardManualReviewService = new IdCardManualReviewService();
                    //int total = 0;
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
                     dt = idCardManualReviewService.LoadReviewForExport(uid, uin, reviewStatus, reviewResult, beginDate, endDate,  str2 + " " + str);
#endif

                    NPOIExportToExcel exportToExcel = new NPOIExportToExcel();
                    exportToExcel.dt = dt;
                    exportToExcel.lineTitle =string.Empty;
                    exportToExcel.excelName = "身份证影印件审核";
                    exportToExcel.sheetName = "身份证影印件审核";
                    exportToExcel.ExportExcel();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}