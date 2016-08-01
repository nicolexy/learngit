using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                    DoAction(actionName);
                }
            }
        }

        private void DoAction(string actionName)
        {
            if (actionName.Equals("GetCookie"))
            {
                GetCookie();
            }
            else if (actionName.Equals("SetCookie"))
            {
                SetCookie();
            }
        }

        private void GetCookie()
        {
            bool result = false;
            try
            {
                HttpCookie cookie = Request.Cookies["TencentKFCFSystemloginStaus"];

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

        private void SetCookie()
        {
            try
            {
                // 创建一个HttpCookie对象
                HttpCookie cookie = new HttpCookie("TencentKFCFSystemloginStaus");
                //设定此cookie值
                cookie.Value = uid;
                DateTime dtnow = DateTime.Now;
                //TimeSpan tsminute = new TimeSpan(0, 1, 0, 0);
                TimeSpan ts = DateTime.Parse(dtnow.AddDays(1).ToString("yyyy-MM-dd 00:01:00")).Subtract(dtnow);
                cookie.Expires = dtnow + ts;
                //加入此cookie
                Response.Cookies.Add(cookie);
                Response.Write(cookie.Value);
                Response.End();
            }
            catch (Exception ex)
            { 
            
            }            
        }
    }
}