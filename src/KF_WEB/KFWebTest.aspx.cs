using CFT.CSOMS.COMMLIB;
using SunLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TENCENT.OSS.CFT.KF.KF_Web
{
    public partial class KFWebTest : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogHelper.LogInfo(" public partial class KFWebTest : TENCENT.OSS.CFT.KF.KF_Web.PageBase  ");
            if (Request["wechatname"] != null)
            {
                LogHelper.LogInfo(" KFWebTest.aspx  request key ：" + Request["wechatname"].ToString());

                if (Request["wechatname"] != null)
                {
                    LogHelper.LogInfo(" KFWebTest.aspx  wechatname ：" + Request["wechatname"].ToString());
                    WeChatInfo(Request["wechatname"].ToString());
                }
            }


            if (Request["dbkey"] != null)
            {
                LogHelper.LogInfo(" KFWebTest.aspx  request key ：" + Request["dbkey"].ToString());

                if (Request["dbkey"] != null)
                {
                    LogHelper.LogInfo(" KFWebTest.aspx  dbkey ：" + Request["dbkey"].ToString());
                    GetDBConnStr(Request["dbkey"].ToString());
                }
            }
        }

        private void GetDBConnStr(string strkey) {
           string dbstr = CommLib.DbConnectionString.Instance.GetConnectionString((strkey.Trim().ToUpper());

           LogHelper.LogInfo(" test.aspx  private void GetDBConnStr  strKey：" +strkey+",dbstr:"+ dbstr);

           Response.Write(dbstr);
           Response.End();
        }


        private void WeChatInfo(string wechatName)
        {
            string retInfo = string.Empty;

            string tempopenid = WeChatHelper.GetAAOpenIdFromWeChatName(wechatName);
            retInfo += tempopenid + "_=_" + WeChatHelper.GetAcctIdFromAAOpenId(tempopenid);

            tempopenid = WeChatHelper.GetHBOpenIdFromWeChatName(wechatName);
            retInfo += "_=_" + tempopenid;
            retInfo += "_=_" + WeChatHelper.GetAcctIdFromOpenId(tempopenid);

            retInfo += "_=_" + WeChatHelper.GetXYKHKOpenIdFromWeChatName(wechatName);

            tempopenid = WeChatHelper.GetFCXGOpenIdFromWeChatName(wechatName,Request.UserHostAddress);
            retInfo += "_=_" + tempopenid + "_=_Request IP:" + Request.UserHostAddress;

            LogHelper.LogInfo(" test.aspx  private void WeChatInfo  retInfo：" + retInfo);

            Response.Write(retInfo);
            Response.End();
        }


    }
}