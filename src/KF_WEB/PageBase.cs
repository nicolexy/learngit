﻿using CFT.Apollo.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

namespace TENCENT.OSS.CFT.KF.KF_Web
{
    /// <summary>
    /// 用户操作页面基类
    /// </summary>
    public class PageBase : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //this.Load += new System.EventHandler(PageBase_Load);
            this.Error += new System.EventHandler(PageBase_Error);
            string pageBackgroundSwitch = System.Configuration.ConfigurationManager.AppSettings["SetPageBackgroundSwitch"] ?? "1";
            if (pageBackgroundSwitch == "1")
            {
                this.Load += PageBase_Load;
            }

            //try
            //{
                if (Session["uid"] == null)
                {
                    Response.Write("<script>window.parent.location.href = '../login.aspx?returnurl="+Server.UrlEncode(Request.Url.PathAndQuery)+"';</script>");
                    Response.End();
                }

                //页面请求，日志记录
                string requestRecord = Session["uid"] + " " + Request.UserHostAddress + " " + Request.Path + " " + Request.QueryString+" "+Request.RequestType+" "+Request.Url.Authority;
                    
            LogHelper.LogInfo(requestRecord,"KFWebPageRequest");

            //}
            //catch(Exception ef)//Session为空，则跳转
            //{
            //    Response.Write("超时，请重新<a herf = '../login.aspx' target = 'parent'> 登录</a>！");
            //}
        
        }

        void PageBase_Load(object sender, EventArgs e)
        {
            string pageScriptBgStyle = @"function watermark(wrapClass, text) {
			    var wrap = document.createElement('div');
			    wrap.className = wrapClass;
			    var page_width = Math.max(document.body.scrollWidth, document.body.clientWidth);
			    var page_height = Math.max(document.body.scrollHeight, document.body.clientHeight);
			    for (var i = 20; i < page_width; i += 200) {
			        for (var j = 20; j < page_height; j += 150) {
			            var node = document.createElement('span');
			            node.style.top = j;
			            node.style.left = i;
			            node.textContent = node.innerText = text;
			            wrap.appendChild(node);
			        }
			    }
			    document.body.appendChild(wrap);
			    window.onresize = window.onload = function () {
			        wrap.parentElement.removeChild(wrap);
			        watermark(wrapClass, text);
			    };
			}
			
			watermark('watermark_wrap', '" + Session["uid"].ToString()+ "');";

            ClientScript.RegisterClientScriptBlock(this.GetType(), "PageBaseWater", "<script language='javascript'>" + pageScriptBgStyle + "</script>");
        }

        
        protected void PageBase_Error(object sender, System.EventArgs e)
        {
            string errMsg = GetRequestError(HttpContext.Current);

            LogHelper.LogError(errMsg);

            //HttpContext.Current.Response.Write(errMsg);
            Server.ClearError();

        }

        /// <summary>
        /// 异常日志记录
        /// </summary>
        /// <param name="page"></param>
        /// <param name="errMsg"></param>
        /// <param name="ex"></param>
        public void LogError(string strKey, string errMsg, Exception ex)
        {
            string errVal = "请求页面：" + Request.Url.AbsoluteUri + ", 错误标识：" + strKey + ", 描述信息：" + errMsg;

            if (ex!=null) {
                errVal += ", 异常详情：" + ex.ToString();
            }

            LogHelper.LogError(errVal);
        }

        /// <summary>
        /// 获取请求错误详细参数信息
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string GetRequestError(HttpContext httpContext)
        {
            string errMsg = string.Empty,
                ip = string.Empty,
                browser = string.Empty,
                rget = string.Empty,
                rpost = string.Empty;

            ip = httpContext.Request.UserHostAddress;
            if (!string.IsNullOrEmpty(ip))
                ip = "IP[" + ip + "]";

            NameValueCollection headers = httpContext.Request.Headers;
            if (headers != null && headers.Count > 0)
            {
                browser = "BROWSER[";
                foreach (string header in headers)
                {
                    foreach (string headervalue in headers.GetValues(header))
                    {
                        browser += header + ":" + headervalue + ";";
                    }
                }
                if (browser.EndsWith(";"))
                {
                    browser = browser.Remove(browser.Length - 1) + "]";
                }
            }

            string method = httpContext.Request.HttpMethod;
            if (method.ToLower() == "get")
                rget = GetGetInput();
            else if (method.ToLower() == "post")
                rpost = GetPostInput();

            Exception currentError = httpContext.Server.GetLastError();
            errMsg = string.Format(
                @"<h1>系统错误：</h1><hr/>
                    系统发生错误，请与联系管理员。<br/>
                    错误地址：{0}<br/>
                    错误信息：{1}<hr/>
                    <b>Stack Trace：</b><br/>{2}<hr/>
                    <br/>{3}
                    <br/>{4}
                    <br/>{5}
                    <br/>{6}",
                httpContext.Request.Url.ToString(), currentError.Message.ToString(), currentError.StackTrace, ip, browser, rget, rpost);

            return errMsg;
        }

        /// <summary>
        /// 获取GET方式的请求信息
        /// </summary>
        /// <returns></returns>
        private static string GetGetInput()
        {
            NameValueCollection querystring = HttpContext.Current.Request.QueryString;
            string _value = "GET[";
            foreach (string k in querystring.AllKeys)
            {
                foreach (string v in querystring.GetValues(k))
                {
                    _value = _value + k + ":" + v + ";";
                }
            }
            if (_value.EndsWith(";"))
            {
                _value = _value.Remove(_value.Length - 1);
            }
            return _value + "]";
        }

        /// <summary>
        /// 获取POST方式的请求信息
        /// </summary>
        /// <returns></returns>
        private static string GetPostInput()
        {
            string _value = "POST[";
            if (HttpContext.Current.Request.Headers.ToString().ToLower().Contains("gzip"))
                _value += "GZIP_DATA;";

            NameValueCollection form = HttpContext.Current.Request.Form;
            foreach (string k in form.AllKeys)
            {
                foreach (string v in form.GetValues(k))
                {
                    _value = _value + k + "=" + v + ";";
                }
            }
            if (_value.EndsWith(";"))
            {
                _value = _value.Remove(_value.Length - 1);
            }
            return _value + "]";
        }
    }
}