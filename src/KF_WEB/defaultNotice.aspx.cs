using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
                string requestUrl = Request.QueryString["requestUrl"] == null ? string.Empty : Request.QueryString["requestUrl"].ToString();
                //requestUrl = "/RefundManage/RefundRegistration.aspx";
                if (!string.IsNullOrEmpty(actionName))
                {
                    DoAction(actionName, requestUrl);
                }

            }
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
        }

        private void DoAction(string actionName, string requestUrl)
        {
            if (actionName.Equals("GetCookie"))
            {
                GetCookie();
            }
            else if (actionName.Equals("SetCookie"))
            {
                SetCookie(requestUrl);
            }
        }

        private void GetCookie()
        {
            bool result = false;
            try
            {
                HttpCookie cookie = Request.Cookies["TencentKFCFSystemloginStatus"];

                string loginStaus = cookie == null ? string.Empty : cookie.Value.ToString();
                if (!string.IsNullOrEmpty(loginStaus))
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }            
            Response.Write(result);
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
                builder.Append("[ ");
                builder.Append("{ ");
                builder.Append("\"cookie\": ");
                builder.Append(cookie.Value + ",");
                builder.Append("\"requestUrl\": ");
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
    }
}