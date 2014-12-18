using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.RefundManage
{
    public class RefundPublicFun
    {
       public static string operatorName = null;
        //设置soap头信息
        public static ZWCheck_Service.Finance_Header SetWebServiceHeader(TemplateControl page)
        {

            ZWCheck_Service.Finance_Header header = new ZWCheck_Service.Finance_Header();
            //header.SrcUrl = page.Page.Request.Url.ToString();
            header.UserIP = page.Page.Request.UserHostAddress;
            header.UserName = (page.Page.Session["uid"] == null)? ""  : page.Page.Session["uid"].ToString();
            //header.SessionID = page.Page.Session.SessionID;
            header.SzKey = (page.Page.Session["SzKey"] == null) ? ""  : page.Page.Session["SzKey"].ToString();
            header.OperID = (page.Page.Session["OperID"]== null)? 0   :Int32.Parse(page.Page.Session["OperID"].ToString());
            header.RightString =(page.Page.Session["SzKey"]==null)?"" :page.Page.Session["SzKey"].ToString();
            return header;
        }

        public static string FenToYuan(string strfen)
        {
            if (strfen == "")
            {
                strfen = "0";
            }

            double yuan = (double)(Int64.Parse(strfen)) / 100;
            yuan = Math.Round(yuan, 2);

            string tmp = yuan.ToString();
            int iindex = tmp.IndexOf(".");
            if (iindex == -1) tmp += ".00";
            if (iindex == tmp.Length - 2) tmp += "0";

            return tmp;
        }
    }
}