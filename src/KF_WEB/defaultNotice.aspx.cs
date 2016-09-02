using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CFT.CSOMS.BLL.BankCardBindModule;
using CFT.CSOMS.BLL.IdCardModule;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
namespace TENCENT.OSS.CFT.KF.KF_Web
{
    public partial class defaultNotice : System.Web.UI.Page
    {
        string uid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            uid = Session["uid"] == null ? string.Empty : Session["uid"].ToString();
            if (!IsPostBack)
            {
                string actionName = Request.QueryString["getAction"] == null ? string.Empty : Request.QueryString["getAction"].ToString();
                if (!string.IsNullOrEmpty(actionName))
                {
                    string requestUrl = Request.QueryString["requestUrl"] == null ? string.Empty : Request.QueryString["requestUrl"].ToString();
                    requestUrl = Server.UrlDecode(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(requestUrl)));
                    //requestUrl = "/RefundManage/RefundRegistration.aspx";

                    DoAction(actionName, requestUrl);
                }
            }
            //test();
        }

        private void DoAction(string actionName, string requestUrl)
        {
            if (actionName.Equals("GetCookie"))
            {
                GetCookie(requestUrl);
            }
            else if (actionName.Equals("SetCookie"))
            {
                SetCookie(requestUrl);
            }
        }

        private void GetCookie(string requestUrl)
        {
            string cookieName = string.Empty;
            try
            {
                HttpCookie cookie = Request.Cookies["TencentKFCFSystemloginStatus"];
                cookieName = cookie == null ? string.Empty : cookie.Value.ToString();
            }
            catch (Exception ex)
            {
                cookieName = string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            builder.Append("{");
            builder.Append("\"cookie\":");
            builder.Append("\"" + cookieName + "\",");
            builder.Append("\"requestUrl\":");
            builder.Append("\"" + requestUrl + "\"");
            builder.Append("}");
            builder.Append("]");
            Response.Write(builder.ToString());
            Response.End();
        }

        private void SetCookie(string requestUrl)
        {
            try
            {
                // 创建一个HttpCookie对象
                HttpCookie cookie = new HttpCookie("TencentKFCFSystemloginStatus");
                //设定此cookie值
                cookie.Value = uid;
                DateTime dtnow = DateTime.Now;
                //TimeSpan tsminute = new TimeSpan(0, 1, 0, 0);
                TimeSpan ts = DateTime.Parse(dtnow.AddDays(1).ToString("yyyy-MM-dd 00:01:00")).Subtract(dtnow);
                cookie.Expires = dtnow + ts;
                //加入此cookie
                Response.Cookies.Add(cookie);
                StringBuilder builder = new StringBuilder();
                builder.Append("[");
                builder.Append("{");
                builder.Append("\"cookie\":");
                builder.Append("\"" + cookie.Value + "\",");
                builder.Append("\"requestUrl\":");
                builder.Append("\"" + requestUrl + "\"");
                builder.Append("}");
                builder.Append("]");
                //Response.Write(cookie.Value);
                Response.Write(builder.ToString());
                Response.End();
            }
            catch (Exception ex)
            {

            }
        }

        private void test()
        {
            //测试代码
            //IdCardManualReviewService aaa = new IdCardManualReviewService();
            //string uin = "201311079024139@wx.tenpay.com";
            //uid = "299708515";
            //string seq_no = "1147099249300000001";
            //string credit_spid = "10000003";
            //string front_image = "201604251427591702311";
            //string back_image = "201604251427591702312";
            //int audit_result = 1;
            //string audit_error_des = "image not clear";
            //string audit_operator = "heidizhang";
            //string audit_time = "2016-08-11 10:00:00";
            //string sign = "edf3ea3fd7d7610188acb1a7fc1433f8";
            //string result = aaa.Review(uin, uid, seq_no, credit_spid, front_image, back_image, audit_result, audit_error_des, audit_operator, audit_time);

            //测试代码
            // string retTextDecodeBase64 = DecodeBase64("eyJyZXN1bHQiOiIxOTQ5MDIwMDA0IiwicmVzX2luZm8iOiJbMTk0OTAyMDAwNF3mgqjnmoTmk43kvZzlt7Lmj5DkuqTvvIzor7fnoa7orqTmmK/lkKblt7LnlJ/mlYjjgIIifQ==");
            // string msg = string.Empty;
            // bool result =false;
            // Dictionary<string, string> dic_RetText = CommQuery.StringToDictionary(retTextDecodeBase64.TrimStart('{').TrimEnd('}').Trim('"').Trim('"'), ',', ':', out msg);
            //var json= Newtonsoft.Json.JsonConvert.DeserializeObject(retTextDecodeBase64) as Newtonsoft.Json.Linq.JObject;
            //var ret= json["result"];
            // if (dic_RetText != null && dic_RetText.Count > 0)
            // {
            //     if (dic_RetText.Keys.Contains("result"))
            //     {
            //         string retTextResult = dic_RetText["result"].ToString();
            //         result = retTextResult.Equals("0") ? true : false;
            //         msg = dic_RetText["res_info"].ToString();
            //     }
            // }   

            //string answer = "result=131512015&res_info=[131512015]&res_info_origin=record not found in all db";
            //string Msg = string.Empty;
            ////解析
            ////DataSet ds = CommQuery.ParseRelayPageMethod1(answer, out Msg);
            //DataSet ds =CommQuery.StringToDataTable(answer,'&','=', out Msg );
            //BankCardBindService aaa = new BankCardBindService();
            //string bbb = aaa.GetBankSyncState(0, string.Empty, string.Empty);
            //string seq_no = Guid.NewGuid().ToString("N").Substring(0, 30);
            //string kf_auth_ocr_audit_QueryUrl = System.Configuration.ConfigurationManager.AppSettings["kf_auth_ocr_audit_QueryUrl"];

            #region  身份证审核接口            
            StringBuilder sb_reviewResult = new StringBuilder();
            //sb_reviewResult.Append("{");
            //sb_reviewResult.Append("\"PlatCode\":");
            //sb_reviewResult.Append("\"180220000\",");
            //sb_reviewResult.Append("\"PlatMsg\":");
            //sb_reviewResult.Append("\"PlatSpid length is out of range[10,10].\",");
            //sb_reviewResult.Append("\"SeqNo\":");
            //sb_reviewResult.Append("\"e7b7d12b5b7e4997ac09b5d3c8231c\",");
            //sb_reviewResult.Append("}");

            sb_reviewResult.Append("{");
            sb_reviewResult.Append("\"PlatCode\":");
            sb_reviewResult.Append("\"0\",");
            sb_reviewResult.Append("\"PlatMsg\":");
            sb_reviewResult.Append("\"Request Accepted\",");
            sb_reviewResult.Append("\"RetText\":");
            sb_reviewResult.Append("\"eyJyZXN1bHQiOiIxOTQ5MDIwMDA0IiwicmVzX2luZm8iOiJbMTk0OTAyMDAwNF3mgqjnmoTmk43kvZzlt7Lmj5DkuqTvvIzor7fnoa7orqTmmK/lkKblt7LnlJ/mlYjjgIIifQ==\",");
            sb_reviewResult.Append("\"SeqNo\":");
            sb_reviewResult.Append("\"d1614fbc8b994ce48ae48d1f1b1604\",");
            sb_reviewResult.Append("\"Sign\":");
            sb_reviewResult.Append("\"368A4978CB5F509E830F5D4D7D79CF6C\"");
            sb_reviewResult.Append("}");



            bool result = false;
            string msg = string.Empty;
            //平台调用接口返回结果                
            var reviewResultJson = Newtonsoft.Json.JsonConvert.DeserializeObject(sb_reviewResult.ToString()) as Newtonsoft.Json.Linq.JObject;
            if (reviewResultJson != null && reviewResultJson.Count > 0)
            {
                string platCode = reviewResultJson["PlatCode"].ToString();
                if (platCode.Equals("0"))
                {
                    result = true;
                    //调用接口返回结果
                    string retText = reviewResultJson["RetText"].ToString();
                    string retTextDecodeBase64 = IdCardManualReviewService.DecodeBase64(retText);

                    var retTextDecodeBase64Json = Newtonsoft.Json.JsonConvert.DeserializeObject(retTextDecodeBase64) as Newtonsoft.Json.Linq.JObject;
                    if (retTextDecodeBase64Json != null && retTextDecodeBase64Json.Count > 0)
                    {
                        string retTextResult = retTextDecodeBase64Json["result"].ToString();
                        result = retTextResult.Equals("0") ? true : false;
                        msg = retTextDecodeBase64Json["res_info"].ToString();
                        string aaa = retTextDecodeBase64Json.ToString();
                    }
                }
                else
                {
                    result = false;
                    msg = reviewResultJson["PlatMsg"].ToString();
                }
            }
            #endregion
        }
    }
}