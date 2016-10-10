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

                //Response.Redirect("../login.aspx?returnurl=" + Server.UrlEncode(Request.Url.PathAndQuery));
            }
            else
            {
                try
                {
                    this.Label_uid.Text = Session["uid"] == null ? string.Empty : Session["uid"].ToString();
                    //hid_IdCaredServerPath.Value = System.Configuration.ConfigurationManager.AppSettings["IDCardManualReview_zx_sm_get_imagefcgi"].ToString();
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
                        dt.Columns.Add("审核时间");
                        dt.Columns.Add("总工单量");
                        dt.Columns.Add("待审核总量");
                        dt.Columns.Add("进单量");
                        dt.Columns.Add("已处理量");
                        dt.Columns.Add("审核通过量");
                        dt.Columns.Add("审核通过率");
                        dt.Columns.Add("审核拒绝量");
                        dt.Columns.Add("审核拒绝率");
                        for (int i = 0; i < 30; i++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["审核时间"] = DateTime.Parse(modifyBeginDate).AddDays(i).ToString("yyyy-MM-dd");
                            dr["总工单量"] = i+1000;
                            dr["待审核总量"] = i+500;
                            dr["进单量"] = i + 300;
                            dr["已处理量"] = i + 1000;
                            dr["审核通过量"] = i + 1000;
                            dr["审核通过率"] = 60;
                            dr["审核拒绝量"] = i + 1000;
                            dr["审核拒绝率"] = 40;
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
                        dt.Columns.Add("处理人");
                        dt.Columns.Add("审核时间");
                        dt.Columns.Add("未处理");
                        dt.Columns.Add("通过");
                        dt.Columns.Add("拒绝");
                        dt.Columns.Add("当天第一单处理时间");
                        dt.Columns.Add("当天最后一单处理时间");
                        dt.Columns.Add("汇总");
                        for (int i = 0; i < 30; i++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["处理人"] ="v_hjlong";
                            dr["审核时间"] = DateTime.Parse(modifyBeginDate).AddDays(i).ToString("yyyy-MM-dd");
                            dr["未处理"] = i + 1000;
                            dr["通过"] = i + 500;
                            dr["拒绝"] = i + 300;
                            dr["当天第一单处理时间"] = DateTime.Parse(modifyBeginDate).AddDays(i).ToString("yyyy-MM-dd");
                            dr["当天最后一单处理时间"] = DateTime.Parse(modifyBeginDate).AddDays(i).ToString("yyyy-MM-dd");
                            dr["汇总"] = i + 1000;
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
                        dt.Columns.Add("审核时间");
                        dt.Columns.Add("两张均为正面或负面");
                        dt.Columns.Add("身份证不清晰不完整");
                        dt.Columns.Add("身份证姓名和提供姓名不符");
                        dt.Columns.Add("身份证证件号不一致");
                        dt.Columns.Add("身份证签发机关和地址不一致");
                        dt.Columns.Add("身份证已超过有效期");
                        dt.Columns.Add("身份证照片非原件");
                        dt.Columns.Add("身份证证件虚假");
                        dt.Columns.Add("未显示图片");
                        dt.Columns.Add("上传非身份证照片");
                        dt.Columns.Add("其他原因");
                        for (int i = 0; i < 30; i++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["审核时间"] = DateTime.Parse(modifyBeginDate).AddDays(i).ToString("yyyy-MM-dd");
                            dr["两张均为正面或负面"] = i + 1000;
                            dr["身份证不清晰不完整"] = i + 500;
                            dr["身份证姓名和提供姓名不符"] = i + 300;
                            dr["身份证证件号不一致"] = i + 1000;
                            dr["身份证签发机关和地址不一致"] = i + 1000;
                            dr["身份证已超过有效期"] = 60;
                            dr["身份证照片非原件"] = i + 1000;
                            dr["身份证证件虚假"] = 40;
                            dr["未显示图片"] = 50;
                            dr["上传非身份证照片"] = 60;
                            dr["其他原因"] = 70;
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