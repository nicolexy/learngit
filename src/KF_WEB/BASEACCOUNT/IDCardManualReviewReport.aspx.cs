using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CFT.CSOMS.BLL.CFTAccountModule;
using CFT.CSOMS.BLL.IdCardModule;
using commLib;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    public partial class IDCardManualReviewReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["uid"] == null)
            {
                if (Request.QueryString["getAction"] != null)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("[");
                    builder.Append("{");
                    builder.Append("\"result\":");
                    builder.Append("\"False\",");
                    builder.Append("\"message\":");
                    builder.Append("\"NoRight\",");
                    builder.Append("\"loginPath\":");
                    builder.Append("\"../login.aspx?returnUrl=" + Server.UrlEncode(Request.Url.AbsolutePath) + "\"");
                    builder.Append("}");
                    builder.Append("]");
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
            if (actionName.Equals("LoadHZReport"))
            {
                LoadHZReport();//个人汇总报表
            }
            else if (actionName.Equals("LoadPersonalReviewReport"))
            {
                LoadPersonalReviewReport();//个人审核情况报表
            }
            else if (actionName.Equals("LoadFailReasonReport"))
            {
                LoadFailReasonReport();//失败原因报表
            }
            else if (actionName.Equals("CheckDate"))
            {
                CheckDate();
            }
        }

        private void LoadHZReport()
        {
            string message = string.Empty;
            try
            {
                string order = (base.Request.Form["order"] != null) ? base.Request.Form["order"].ToString() : "asc";
                int pageNumber = (base.Request.Form["page"] != null) ? int.Parse(base.Request.Form["page"].ToString()) : 1;
                int pageSize = (base.Request.Form["rows"] != null) ? int.Parse(base.Request.Form["rows"].ToString()) : 15;
                string sort = (base.Request.Form["sort"] != null) ? base.Request.Form["sort"].ToString() : "Fcreate_time ";

                string uid = this.Label_uid.Text;// Request.Form["uid"] != null || string.IsNullOrEmpty(Request.Form["uid"].ToString()) ? Request.Form["uid"].ToString() : string.Empty;
                
                string modifyBeginDate = Request.Form["modifyBeginDate"] != null && !string.IsNullOrEmpty(Request.Form["modifyBeginDate"].ToString()) ? Request.Form["modifyBeginDate"].ToString() : string.Empty;
                string modifyEndDate = Request.Form["modifyEndDate"] != null && !string.IsNullOrEmpty(Request.Form["modifyEndDate"].ToString()) ? Request.Form["modifyEndDate"].ToString() : string.Empty;
                string foperator = Request.Form["foperator"] != null && !string.IsNullOrEmpty(Request.Form["foperator"].ToString()) ? Request.Form["foperator"].ToString() : string.Empty;
                int totalMonth = DateTime.Parse(modifyEndDate).Year * 12 + DateTime.Parse(modifyEndDate).Month - DateTime.Parse(modifyBeginDate).Year * 12 - DateTime.Parse(modifyBeginDate).Month;
                if (totalMonth >= 1)
                {
                    message = string.Format("查询日期不能超过一个月");
                }
                else
                {
                

                    IdCardManualReviewService idCardManualReviewService = new IdCardManualReviewService();
                    int total = 0;
                    DataTable dt = new DataTable();
                    if (classLibrary.getData.IsTestMode && !classLibrary.getData.IsNewSensitivePowerMode)
                    {
                        #region
                        dt.Columns.Add("Date");
                        dt.Columns.Add("ZongGongDanLiang");
                        dt.Columns.Add("DaiShenHeZongLiang");
                        dt.Columns.Add("JinDanLiang");
                        dt.Columns.Add("YiChuLiLiang");
                        dt.Columns.Add("ShenHeTongGuoLiang");
                        dt.Columns.Add("ShenHeTongGuoLv");
                        dt.Columns.Add("ShenHeJuJueLiang");
                        dt.Columns.Add("ShenHeJuJueLv");
                        for (int i = 0; i < 30; i++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["Date"] = DateTime.Parse(modifyBeginDate).AddDays(i).ToString("yyyy-MM-dd");
                            dr["ZongGongDanLiang"] = i + 1000;
                            dr["DaiShenHeZongLiang"] = i + 500;
                            dr["JinDanLiang"] = i + 300;
                            dr["YiChuLiLiang"] = i + 1000;
                            dr["ShenHeTongGuoLiang"] = i + 1000;
                            dr["ShenHeTongGuoLv"] = 60;
                            dr["ShenHeJuJueLiang"] = i + 1000;
                            dr["ShenHeJuJueLv"] = 40;
                            dt.Rows.Add(dr);
                        }                        
                        #endregion
                    }
                    else
                    {
                        dt = idCardManualReviewService.LoadHZReport(uid, modifyBeginDate, modifyEndDate, foperator, pageSize, pageNumber, sort + " " + order, ref total);
                    }
                    //                    #region
                    //#if DEBUG



                    //#endif
                    //                    #endregion
                    //#if !DEBUG

                    //#endif

                    //message = JsonHelper.DataTableToJson(dt, total, true);
                    message = idCardManualReviewService.DataTableToJsonForHZReport(dt, total, true);
                    //message=" { "rows":[ { "Fid":"12","Fserial_number":"1147105501600000005","Fspid":"2000000501","Fcreate_time":"2016/8/13 9:12:24","Fuin":"195806606","Fname":"SEZyxazimzM=","Fidentitycard":"TG/Jqoya4pb3/R3Qe2Vr0jSMNoV4R3Ju","Fmodify_time":"2016/8/15 16:42:07","Fimage_path1":"ent_no=10000003&req_data=FAD02DDB476FC071DDD0F0C4FB33885740C3B3181A70EDCD4EB5EADEAA88514643BB1F9AD403207242646194A636EB6AD5ED27BC462EF8063EDABCCB7C2DC624&seq_no=1147105501600000005","Fimage_path2":"ent_no=10000003&req_data=FAD02DDB476FC071DDD0F0C4FB33885740C3B3181A70EDCD4EB5EADEAA88514643BB1F9AD403207242646194A636EB6AD5ED27BC462EF8063EDABCCB7C2DC624&seq_no=1147105501600000005","Fimage_file1":"1-1-5-64-0-1-3016181900461","Fimage_file2":"1-1-5-64-0-1-3016181900461","Fstate":"2","Fresult":"0","Fmemo":"0","Foperator":"1100000000","Fstandby1":"","Fstandby2":"","Fstandby3":"","Fstandby4":"","Fstandby5":"","TableName":"c2c_fmdb.t_check_identitycard_201608"}, { "Fid":"13","Fserial_number":"1147105531500000006","Fspid":"2000000501","Fcreate_time":"2016/8/13 9:12:24","Fuin":"195806606","Fname":"SEZyxazimzM=","Fidentitycard":"TG/Jqoya4pb3/R3Qe2Vr0jSMNoV4R3Ju","Fmodify_time":"2016/8/15 15:40:53","Fimage_path1":"ent_no=10000003&req_data=FAD02DDB476FC071DDD0F0C4FB33885740C3B3181A70EDCD4EB5EADEAA88514643BB1F9AD403207242646194A636EB6AD5ED27BC462EF8063EDABCCB7C2DC624&seq_no=1147105531500000006","Fimage_path2":"ent_no=10000003&req_data=FAD02DDB476FC071DDD0F0C4FB33885740C3B3181A70EDCD4EB5EADEAA88514643BB1F9AD403207242646194A636EB6AD5ED27BC462EF8063EDABCCB7C2DC624&seq_no=1147105531500000006","Fimage_file1":"1-1-5-64-0-1-3016181900461","Fimage_file2":"1-1-5-64-0-1-3016181900461","Fstate":"2","Fresult":"0","Fmemo":"4","Foperator":"1100000000","Fstandby1":"","Fstandby2":"","Fstandby3":"","Fstandby4":"","Fstandby5":"","TableName":"c2c_fmdb.t_check_identitycard_201608"}, { "Fid":"14","Fserial_number":"1234","Fspid":"1234567890","Fcreate_time":"2016/8/12 0:00:00","Fuin":"abcd@wx.tenpay.com","Fname":"guoyueqiang","Fidentitycard":"360726","Fmodify_time":"","Fimage_path1":"image_path1","Fimage_path2":"image_path2","Fimage_file1":"image_file1","Fimage_file2":"image_file2","Fstate":"1","Fresult":"0","Fmemo":"","Foperator":"","Fstandby1":"","Fstandby2":"","Fstandby3":"","Fstandby4":"","Fstandby5":"","TableName":"c2c_fmdb.t_check_identitycard_201608"} ],"total":3}";
                }
            }
            catch (Exception ex)
            {

            }
            Response.Write(message);
            Response.End();
        }
        private void LoadPersonalReviewReport()
        {
            string message = string.Empty;
            try
            {
                string order = (base.Request.Form["order"] != null) ? base.Request.Form["order"].ToString() : "asc";
                int pageNumber = (base.Request.Form["page"] != null) ? int.Parse(base.Request.Form["page"].ToString()) : 1;
                int pageSize = (base.Request.Form["rows"] != null) ? int.Parse(base.Request.Form["rows"].ToString()) : 15;
                string sort = (base.Request.Form["sort"] != null) ? base.Request.Form["sort"].ToString() : "Fcreate_time ";

                string uid = this.Label_uid.Text;// Request.Form["uid"] != null || string.IsNullOrEmpty(Request.Form["uid"].ToString()) ? Request.Form["uid"].ToString() : string.Empty;

                string modifyBeginDate = Request.Form["modifyBeginDate"] != null && !string.IsNullOrEmpty(Request.Form["modifyBeginDate"].ToString()) ? Request.Form["modifyBeginDate"].ToString() : string.Empty;
                string modifyEndDate = Request.Form["modifyEndDate"] != null && !string.IsNullOrEmpty(Request.Form["modifyEndDate"].ToString()) ? Request.Form["modifyEndDate"].ToString() : string.Empty;
                string foperator = Request.Form["foperator"] != null && !string.IsNullOrEmpty(Request.Form["foperator"].ToString()) ? Request.Form["foperator"].ToString() : string.Empty;
                int totalMonth = DateTime.Parse(modifyEndDate).Year * 12 + DateTime.Parse(modifyEndDate).Month - DateTime.Parse(modifyBeginDate).Year * 12 - DateTime.Parse(modifyBeginDate).Month;
                if (totalMonth >= 1)
                {
                    message = string.Format("查询日期不能超过一个月");
                }
                else
                {


                    IdCardManualReviewService idCardManualReviewService = new IdCardManualReviewService();
                    int total = 0;
                    DataTable dt = new DataTable();
                    if (classLibrary.getData.IsTestMode && !classLibrary.getData.IsNewSensitivePowerMode)
                    {
                        #region
                        dt.Columns.Add("Foperator");
                        dt.Columns.Add("Fmodify_time");
                        dt.Columns.Add("未处理");
                        dt.Columns.Add("Agree");
                        dt.Columns.Add("Refuse");
                        dt.Columns.Add("CurrentDayFirstFmodify_time");
                        dt.Columns.Add("CurrentDayLastFmodify_time");
                        dt.Columns.Add("Total");
                        for (int i = 0; i < 30; i++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["Foperator"] = "v_hjlong";
                            dr["Fmodify_time"] = DateTime.Parse(modifyBeginDate).AddDays(i).ToString("yyyy-MM-dd");
                            dr["未处理"] = i + 1000;
                            dr["Agree"] = i + 500;
                            dr["Refuse"] = i + 300;
                            dr["CurrentDayFirstFmodify_time"] = DateTime.Parse(modifyBeginDate).AddDays(i).ToString("yyyy-MM-dd");
                            dr["CurrentDayLastFmodify_time"] = DateTime.Parse(modifyBeginDate).AddDays(i).ToString("yyyy-MM-dd");
                            dr["Total"] = i + 1000;
                            dt.Rows.Add(dr);
                        }
                        #endregion
                    }
                    else
                    {
                        dt = idCardManualReviewService.LoadPersonalReviewReport(uid, modifyBeginDate, modifyEndDate, foperator, pageSize, pageNumber, sort + " " + order, ref total);
                    }
                    //                    #region
                    //#if DEBUG



                    //#endif
                    //                    #endregion
                    //#if !DEBUG

                    //#endif

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
        private void LoadFailReasonReport()
        {
            string message = string.Empty;
            try
            {
                string order = (base.Request.Form["order"] != null) ? base.Request.Form["order"].ToString() : "asc";
                int pageNumber = (base.Request.Form["page"] != null) ? int.Parse(base.Request.Form["page"].ToString()) : 1;
                int pageSize = (base.Request.Form["rows"] != null) ? int.Parse(base.Request.Form["rows"].ToString()) : 15;
                string sort = (base.Request.Form["sort"] != null) ? base.Request.Form["sort"].ToString() : "Fcreate_time ";

                string uid = this.Label_uid.Text;// Request.Form["uid"] != null || string.IsNullOrEmpty(Request.Form["uid"].ToString()) ? Request.Form["uid"].ToString() : string.Empty;

                string modifyBeginDate = Request.Form["modifyBeginDate"] != null && !string.IsNullOrEmpty(Request.Form["modifyBeginDate"].ToString()) ? Request.Form["modifyBeginDate"].ToString() : string.Empty;
                string modifyEndDate = Request.Form["modifyEndDate"] != null && !string.IsNullOrEmpty(Request.Form["modifyEndDate"].ToString()) ? Request.Form["modifyEndDate"].ToString() : string.Empty;
                string foperator = Request.Form["foperator"] != null && !string.IsNullOrEmpty(Request.Form["foperator"].ToString()) ? Request.Form["foperator"].ToString() : string.Empty;
                int totalMonth = DateTime.Parse(modifyEndDate).Year * 12 + DateTime.Parse(modifyEndDate).Month - DateTime.Parse(modifyBeginDate).Year * 12 - DateTime.Parse(modifyBeginDate).Month;
                if (totalMonth >= 1)
                {
                    message = string.Format("查询日期不能超过一个月");
                }
                else
                {


                    IdCardManualReviewService idCardManualReviewService = new IdCardManualReviewService();
                    int total = 0;
                    DataTable dt = new DataTable();
                    if (classLibrary.getData.IsTestMode && !classLibrary.getData.IsNewSensitivePowerMode)
                    {
                        #region
                        dt.Columns.Add("Fmodify_time");
                        dt.Columns.Add("Fmemo8");
                        dt.Columns.Add("Fmemo3");
                        dt.Columns.Add("Fmemo6");
                        dt.Columns.Add("Fmemo4");
                        dt.Columns.Add("Fmemo7");
                        dt.Columns.Add("Fmemo10");
                        dt.Columns.Add("Fmemo11");
                        dt.Columns.Add("Fmemo9");
                        dt.Columns.Add("Fmemo1");
                        dt.Columns.Add("Fmemo2");
                        dt.Columns.Add("Fmemo5");
                        dt.Columns.Add("Total");
                        for (int i = 0; i < 30; i++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["Fmodify_time"] = DateTime.Parse(modifyBeginDate).AddDays(i).ToString("yyyy-MM-dd");
                            dr["Fmemo8"] = i + 1000;
                            dr["Fmemo3"] = i + 500;
                            dr["Fmemo6"] = i + 300;
                            dr["Fmemo4"] = i + 1000;
                            dr["Fmemo7"] = i + 1000;
                            dr["Fmemo10"] = 60;
                            dr["Fmemo11"] = i + 1000;
                            dr["Fmemo9"] = 40;
                            dr["Fmemo1"] = 50;
                            dr["Fmemo2"] = 60;
                            dr["Fmemo5"] = 70;
                            dr["Total"] = 50000;
                            dt.Rows.Add(dr);
                        }
                        #endregion
                    }
                    else
                    {
                        dt = idCardManualReviewService.LoadFailReasonReport(uid, modifyBeginDate, modifyEndDate, foperator, pageSize, pageNumber, sort + " " + order, ref total);
                    }
                    //                    #region
                    //#if DEBUG



                    //#endif
                    //                    #endregion
                    //#if !DEBUG

                    //#endif

                    //message = JsonHelper.DataTableToJson(dt, total, true);
                    message = idCardManualReviewService.DataTableToJsonForFailReasonReport(dt, total, true);
                    
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

                string beginDate = Request.Form["beginDate"] != null && !string.IsNullOrEmpty(Request.Form["beginDate"].ToString()) ? Request.Form["beginDate"].ToString() : string.Empty;
                string endDate = Request.Form["endDate"] != null && !string.IsNullOrEmpty(Request.Form["endDate"].ToString()) ? Request.Form["endDate"].ToString() : string.Empty;
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
    }
}