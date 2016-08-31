using System;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using CFT.CSOMS.COMMLIB;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.AccountModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    public partial class logOnWxAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label_uid.Text = Session["uid"].ToString();
            if (!IsPostBack)
            {
                if (Request.Params["action"] != null && Request.Params["action"].ToString() != "")
                {
                    string action = Request.Params["action"].ToString();
                    switch (action)
                    {
                        case "CancelWx":
                            {
                                Response.ContentType = "application/json";
                                string wxid = Request.Params["wxid"];
                                string reason = Request.Params["reason"];
                                string ret = GetLogOnWxResult(wxid,reason);
                                Response.Write(ret);
                                Response.End();
                            }
                            break;                        
                    }
                }
            }
        }

        public string GetLogOnWxResult(string wxid, string reason)
        {
            string ret = string.Empty;
            string uin = string.Empty;
            string msg = string.Empty;
            string username = Session["uid"].ToString();
            string clientip = HttpContext.Current.Request.UserHostAddress == "::1" ? "127.0.0.1" : HttpContext.Current.Request.UserHostAddress;
            StringBuilder sb = new StringBuilder();          
            for (int i = 0; i < Page.Session.Count; i++)
            {
                sb.Append("Session"+Session.Keys[i]+":"+Session[i].ToString());
            }        
            for (int i = 0; i < Request.Cookies.Count; i++)
            {
                sb.Append("Cookie" + Request.Cookies.Keys[i] + ":" + Request.Cookies[i].Value.ToString());
            }
            SunLibrary.LogHelper.LogInfo("GetLogOnWxResult-Current_Cookie_Session:" + sb.ToString());
            HttpCookie cookie = Request.Cookies["TCOA_TICKET"];
            if (null == cookie)
            {
                return "{\"ret\":\"oa_ticket不合法！\"}"; 
            }            
            string oaticket = cookie.Value.ToString();    
            uin = WeChatHelper.GetUINByWxid(wxid, out msg);
            if(!string.IsNullOrEmpty(msg)){
                return "{\"ret\":\"微信号不合法！\"}"; 
            }            
            bool state = new AccountService().LogOnWxAccount(uin, username, oaticket, clientip, reason, out msg);
            ret = state ? "{\"ret\":\"注销成功！\"}" : "{\"ret\":\"注销失败【" + msg + "】\"}";
            return ret;
        }
    }
}