using CFT.CSOMS.BLL.ForeignCurrencyModule;
using CFT.CSOMS.COMMLIB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay
{
    public partial class RealNameInformationQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Query_Click(object sender, EventArgs e)
        {
            try 
            {
                bind();
            }
            catch(Exception ex)
            {
                WebUtils.ShowMessage(this.Page, HttpUtility.JavaScriptStringEncode(ex.ToString()));
            }
        }

        public void bind() 
        {
            string input = txt_input_id.Text.Trim();
            if (input.Length < 1)
            {
                WebUtils.ShowMessage(this.Page, "请输入查询条件!"); return;
            }

            var bll = new FCXGWallet();

            string ip = this.Page.Request.UserHostAddress.ToString();
            if (ip == "::1")
                ip = "127.0.0.1";

            string queryuid = "";
            string uin = "";

            if (checkUid.Checked)
            {
                queryuid = input;
                var dt_user = bll.QueryUserInfo(queryuid, ip);
                if (dt_user != null && dt_user.Columns.Count > 0 && dt_user.Columns.Contains("uin"))
                {
                    uin = dt_user.Rows[0]["uin"].ToString();
                }
            }
            else if (checkWeChatId.Checked)
            {
                uin = WeChatHelper.GetFCXGOpenIdFromWeChatName(input, Request.UserHostAddress) + "@wx.hkg";
            }
            else if (checkUin.Checked)
            {
                uin = input;
            }
#if DEBUG
            uin = "o5PXlsmuWeDZGyySqGzI-hEroCKA@wx.hkg";
#endif
            DataTable dt = new FCXGWallet().QueryRealNameInfo(uin, ip);

            panDetail.Visible = true;
            bool isRight = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("SensitiveRole", this);

            foreach (var con in panDetail.Controls)
            {
                if (con is Label)
                {
                    Label lable = con as Label;
                    if (lable.ID.StartsWith("lbl_"))
                    {
                        try
                        {
                            if (lable.ID == "lbl_name")
                            {
                                lable.Text = classLibrary.setConfig.ConvertName(dt.Rows[0][lable.ID.Replace("lbl_", "")].ToString(), isRight);
                            }
                            else if (lable.ID == "lbl_mobile")
                            {
                                lable.Text = classLibrary.setConfig.ConvertTelephoneNumber(dt.Rows[0][lable.ID.Replace("lbl_", "")].ToString(), isRight);
                            }
                            else if (lable.ID == "lbl_cre_id")
                            {
                                lable.Text = classLibrary.setConfig.IDCardNoSubstring(dt.Rows[0][lable.ID.Replace("lbl_", "")].ToString(), isRight);
                            }
                            else
                            {
                                lable.Text = dt.Rows[0][lable.ID.Replace("lbl_", "")].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            lable.Text = ex.Message;
                        }
                    }
                }
            }
        }
    }
}