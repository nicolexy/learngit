using CFT.CSOMS.BLL.CFTAccountModule;
using CFT.CSOMS.COMMLIB;
using SunLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TENCENT.OSS.CFT.KF.Common;

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
           string dbstr = CommLib.DbConnectionString.Instance.GetConnectionString(strkey.Trim().ToUpper());

           LogHelper.LogInfo(" test.aspx  private void GetDBConnStr  strKey：" +strkey+",dbstr:"+ dbstr);

            string qq="404968099";
                if (Request["qq"] != null){
                    qq = Request["qq"].ToString();
                }
            this.Label1.Text = dbstr;

            bindshimingrzData(qq);

           Response.Write(dbstr);

        }


        private void bindshimingrzData(string qq)
        {
            string fuin = classLibrary.setConfig.replaceMStr(qq);

            int max = 20;
            int start = 1;

            DataSet ds = new AuthenInfoService().GetUserClassQueryListForThis(fuin, start, max);

            if (ds != null && ds.Tables.Count > 0)
            {
                this.GridView1.DataSource = ds.Tables[0].DefaultView;
                this.GridView1.DataBind();
            }
            else
            {
                throw new LogicException("bindshimingrzData没有找到记录！");
            }

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