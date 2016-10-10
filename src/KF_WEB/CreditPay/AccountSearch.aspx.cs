﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CFT.CSOMS.BLL.CreditModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.CreditPay
{
    public partial class AccountSearch : System.Web.UI.Page
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
            if (actionName.Equals("SearchAccountInfo"))
            {
                SearchAccountInfo();//个人汇总报表
            }          
        }

        public void SearchAccountInfo()
        {
            StringBuilder builder = new StringBuilder();
            string returnResult = string.Empty;
            try
            {
                string accountNo = Request.Form["accountNo"] != null && !string.IsNullOrEmpty(Request.Form["accountNo"].ToString()) ? Request.Form["accountNo"].ToString() : string.Empty;
                int accountType = Request.Form["accountType"] != null && !string.IsNullOrEmpty(Request.Form["accountType"].ToString()) ? int.Parse(Request.Form["accountType"].ToString()) : 1;
                TencentCreditService tencentCreditService = new TencentCreditService();
                bool searchResult = false;
                string errorMessage = string.Empty;
                returnResult = tencentCreditService.SearchAccountInfo(accountNo, accountType, out searchResult,out errorMessage);
                if (searchResult)
                {
                    builder.Append(returnResult);
                }
                else 
                {
                    builder.Append("[");
                    builder.Append("{");
                    builder.Append("\"result\":");
                    builder.Append("\"" + searchResult + "\",");
                    builder.Append("\"message\":");
                    builder.Append("\"" + errorMessage + "\"");
                    builder.Append("}");
                    builder.Append("]");
                }
            }
            catch (Exception ex)
            {
                builder.Append("[");
                builder.Append("{");
                builder.Append("\"result\":");
                builder.Append("\"False\",");
                builder.Append("\"message\":");
                builder.Append("\"查询出错了!\"");
                builder.Append("}");
                builder.Append("]");
            }                        
            Response.Write(builder.ToString());
            Response.End();
        }
    }
}