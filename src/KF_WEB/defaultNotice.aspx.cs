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
                    try
                    {
                        requestUrl = Server.UrlDecode(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(requestUrl))); // !string.IsNullOrEmpty(IdCardManualReviewService.DecodeBase64(requestUrl)) ? IdCardManualReviewService.DecodeBase64(requestUrl).Replace("%2f", "/") : string.Empty;
                        //requestUrl = "/RefundManage/RefundRegistration.aspx";
                    }
                    catch (Exception ex)
                    {
                        requestUrl = Request.QueryString["requestUrl"] == null ? string.Empty : Request.QueryString["requestUrl"].ToString();
                    }
                    Default defaultAspx = new Default();
                    defaultAspx.requestUrl = requestUrl;
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
            else if (actionName.Equals("Redirect"))
            {
                Redirect(requestUrl);
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

        private void Redirect(string requestUrl)
        {
            if (!string.IsNullOrEmpty(requestUrl))
            {
                //requestUrl = !string.IsNullOrEmpty(IdCardManualReviewService.DecodeBase64(requestUrl)) ? IdCardManualReviewService.DecodeBase64(requestUrl).Replace("%2f","/") : string.Empty;// Server.UrlDecode(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(requestUrl)));
                string url = HttpContext.Current.Request.Url.Host; //取 域名： 
                Response.Redirect(IdCardManualReviewService.DecodeBase64(requestUrl).Replace("%2f", "/"));                
            }
        }

        private void test()
        {
            //测试代码
            #region 客服系统充值单状态
            //string pay_result = string.Empty;
            //try
            //{
            //    string getBankSyncStateResult = "result=131512015&res_info=[131512015]&res_info_origin=record not found in all db";
            //    Dictionary<string, string> resultToDictionary = CommQuery.StringToDictionary(getBankSyncStateResult, '&', '=');
            //    if (resultToDictionary != null && resultToDictionary.Count > 0)
            //    {
            //        if (resultToDictionary.ContainsKey("result"))
            //        {
            //            string result = resultToDictionary["result"].ToString();//0 调用成功； 131512015 记录不存在 ； 131515001 参数错误；  131512009 查询数据库错误                        
            //            string res_info = resultToDictionary["res_info"].ToString();//返回信息
            //            if (result.Equals("0"))
            //            {
            //                if (resultToDictionary["pay_result"].ToString().Equals("1"))
            //                {
            //                    pay_result = "支付结果未知";
            //                }
            //                else if (resultToDictionary["pay_result"].ToString().Equals("2"))
            //                {
            //                    pay_result = "银行扣款成功";
            //                }
            //            }
            //            else if (result.Equals("131512015"))
            //            {

            //                pay_result = "记录不存在";
            //            }
            //            else if (result.Equals("131515001"))
            //            {
                            
            //                pay_result = "参数错误";
            //            }
            //            else if (result.Equals("131512009"))
            //            {

            //                pay_result = "查询数据库错误";
            //            }
            //        }
            //    }
            //}
            //catch (Exception err)
            //{
            //    //throw new Exception("GetBankSyncState Service处理失败！" + err.Message);
            //    pay_result = string.Empty;
                
            //}

            #endregion

            #region  身份证审核接口

            //IdCardManualReviewService aaa = new IdCardManualReviewService();
            //string uin = "085e9858e1e04c9e0e785c35c@wx.tenpay.com";
            //string seq_no = "2147260162500000027";
            //string credit_spid = "40000101";
            //string front_image = "2-1-7-76-3-4-3108004981288";
            //string back_image = "2-1-7-10-3-4-3108010381312";
            //int audit_result = 2;
            //string audit_error_des = "image not clear";
            //string audit_operator = "v_xjsun";
            //string audit_time = "2016-09-02 17:12:00";
            //string mes = string.Empty;
            //bool result = aaa.Review(uin, seq_no, credit_spid, front_image, back_image, audit_result, audit_error_des, audit_operator, audit_time, out mes);

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


            //StringBuilder sb_reviewResult = new StringBuilder();
            ////sb_reviewResult.Append("{");
            ////sb_reviewResult.Append("\"PlatCode\":");
            ////sb_reviewResult.Append("\"180220000\",");
            ////sb_reviewResult.Append("\"PlatMsg\":");
            ////sb_reviewResult.Append("\"PlatSpid length is out of range[10,10].\",");
            ////sb_reviewResult.Append("\"SeqNo\":");
            ////sb_reviewResult.Append("\"e7b7d12b5b7e4997ac09b5d3c8231c\",");
            ////sb_reviewResult.Append("}");

            //sb_reviewResult.Append("{");
            //sb_reviewResult.Append("\"PlatCode\":");
            //sb_reviewResult.Append("\"0\",");
            //sb_reviewResult.Append("\"PlatMsg\":");
            //sb_reviewResult.Append("\"Request Accepted\",");
            //sb_reviewResult.Append("\"RetText\":");
            //sb_reviewResult.Append("\"eyJyZXN1bHQiOiIwIiwicmVzX2luZm8iOiJPSyJ9\",");
            //sb_reviewResult.Append("\"SeqNo\":");
            //sb_reviewResult.Append("\"2147260162500000027\",");
            //sb_reviewResult.Append("\"Sign\":");
            //sb_reviewResult.Append("\"FC1A867BF4BA253E5409642D13B0312E\"");
            //sb_reviewResult.Append("}");



            //bool result = false;
            //string msg = string.Empty;
            ////平台调用接口返回结果                
            //var reviewResultJson = Newtonsoft.Json.JsonConvert.DeserializeObject(sb_reviewResult.ToString()) as Newtonsoft.Json.Linq.JObject;
            //if (reviewResultJson != null && reviewResultJson.Count > 0)
            //{
            //    string platCode = reviewResultJson["PlatCode"].ToString();
            //    if (platCode.Equals("0"))
            //    {
            //        result = true;
            //        //调用接口返回结果
            //        string retText = reviewResultJson["RetText"].ToString();
            //        string retTextDecodeBase64 = IdCardManualReviewService.DecodeBase64(retText);

            //        var retTextDecodeBase64Json = Newtonsoft.Json.JsonConvert.DeserializeObject(retTextDecodeBase64) as Newtonsoft.Json.Linq.JObject;
            //        if (retTextDecodeBase64Json != null && retTextDecodeBase64Json.Count > 0)
            //        {
            //            string retTextResult = retTextDecodeBase64Json["result"].ToString();
            //            result = retTextResult.Equals("0") ? true : false;
            //            msg = retTextDecodeBase64Json["res_info"].ToString();
            //            string aaa = retTextDecodeBase64Json.ToString();
            //        }
            //    }
            //    else
            //    {
            //        result = false;
            //        msg = reviewResultJson["PlatMsg"].ToString();
            //    }
            //}
            #endregion
      
        }       
    }
}