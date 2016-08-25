using CFT.Apollo.Common.Configuration;
using CFT.CSOMS.BLL.CFTAccountModule;
using CFT.CSOMS.BLL.FundModule;
using CFT.CSOMS.DAL.CheckModoule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.RefundManage;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
    public partial class GetFundRatePageRedeem : System.Web.UI.Page
    {
        CheckService checkService = new CheckService();
        string client_ip = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!ClassLib.ValidateRight("LCTQuery", this))
                {
                    Response.Redirect("../login.aspx?wh=1");
                }
                client_ip = Request.UserHostAddress.ToString();
                if (client_ip == "::1")
                {
                    client_ip = "127.0.0.1";
                }
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if (!IsPostBack)
            {
                string Redeem = Request.QueryString["RedeemType"];
                switch (Redeem)
                {
                    case "NonMonetaryFundRedeem": //非货币基金强赎
                        {
                            NonMonetaryFundRedeem();
                            plan_NonMonetaryFundRedeem.Visible = true;
                            break;
                        }
                    case "NonMonetaryFundRedeem_view"://非货币基金强赎-账务系统查看
                        {
                            NonMonetaryFundRedeem_view();
                            plan_NonMonetaryFundRedeem.Visible = true;
                            break;
                        }
                    case "MonetaryFundRedeem"://货币基金强赎
                        {
                            MonetaryFundRedeem();
                            plan_MonetaryFundRedeem.Visible = true;
                            break;
                        }
                    case "MonetaryFundRedeem_view"://货币基金强赎-账务系统查看
                        {
                            MonetaryFundRedeem_view();
                            plan_MonetaryFundRedeem.Visible = true;
                            break;
                        }

                }

            }
        }

        #region 非货币基金强赎
        /// <summary>
        /// 账务系统查看
        /// </summary>
        protected void NonMonetaryFundRedeem_view()
        {
            try
            {
                string objid = Request.QueryString["objid"];
                DataTable dt = checkService.GetCheckInfo(objid, "NonMonetaryFundRedeem");
                if (dt != null && dt.Rows.Count != 0)
                {
                    lbl_uin.Text = dt.Rows[0]["uin"].ToString();
                    lbl_spid.Text = dt.Rows[0]["spid"].ToString();
                    lbl_fund_code.Text = dt.Rows[0]["fund_code"].ToString();
                    lbl_total_fee.Text = dt.Rows[0]["total_fee"].ToString();
                    lbl_acct_type.Text = dt.Rows[0]["acct_type"].ToString();
                    lbl_channel_id.Text = dt.Rows[0]["channel_id"].ToString();
                    lbl_bind_serialno.Text = dt.Rows[0]["bind_serialno"].ToString();
                    lbl_card_tail.Text = dt.Rows[0]["card_tail"].ToString();
                    lbl_bank_type.Text = dt.Rows[0]["bank_type"].ToString();
                    lbl_client_ip.Text = dt.Rows[0]["client_ip"].ToString();
                    lbl_cur_type.Text = dt.Rows[0]["cur_type"].ToString();
                    lbl_close_id.Text = dt.Rows[0]["close_id"].ToString();
                    ddl_redem_type.SelectedValue = dt.Rows[0]["redem_type"].ToString();
                    lbl_opt_type.Text = "全部赎回";

                    imgNoN.ImageUrl = dt.Rows[0]["ImageUrl"].ToString();
                    btnNON.Visible = false;
                    upNON.Visible = false;
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "申请单查询失败！" + PublicRes.GetErrorMsg(eSys.ToString()));
                return;
            }
        }

        /// <summary>
        /// 生产申请单
        /// </summary>
        protected void NonMonetaryFundRedeem()
        {
            string uin = Request.QueryString["uin"];
            string spid = Request.QueryString["spid"];
            string fund_code = Request.QueryString["fund_code"];
            string fund_name = Request.QueryString["fund_name"];
            string total_fee = Request.QueryString["total_fee"];
            string acct_type = Request.QueryString["acct_type"];
            string channel_id = Request.QueryString["channel_id"];
            string bind_serialno = Request.QueryString["bind_serialno"];
            string card_tail = Request.QueryString["card_tail"];
            string bank_type = Request.QueryString["bank_type"];
            string cur_type = Request.QueryString["cur_type"];
            string close_id = Request.QueryString["close_id"];
            string Ftype = Request.QueryString["Ftype"];

            lbl_uin.Text = uin;
            lbl_spid.Text = spid;
            lbl_fund_code.Text = fund_code;
            lbl_total_fee.Text = total_fee;
            lbl_acct_type.Text = acct_type;
            lbl_channel_id.Text = channel_id;
            lbl_bind_serialno.Text = bind_serialno;
            lbl_card_tail.Text = card_tail;
            lbl_bank_type.Text = bank_type;
            lbl_client_ip.Text = client_ip;
            lbl_cur_type.Text ="1";
            lbl_close_id.Text = close_id;
            
            if (Ftype != "7")  //{"7", "投连险"}
            {
                title_close_id.Visible = false;
                lbl_close_id.Visible = false;
                title_opt_type.Visible = false;
                lbl_opt_type.Visible = false;
                lbl_opt_type.Text = "全部赎回";
            }
        }

        /// <summary>
        /// 提交审批单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNON_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["alPath"] == null)
                {
                    WebUtils.ShowMessage(this.Page, "请上传图片！");
                    return;
                }

                ZWCheck_Service.Check_Service checkService = new ZWCheck_Service.Check_Service();
                ZWCheck_Service.Param[] parameters = new ZWCheck_Service.Param[18];

                string objid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string ReturnUrl = "http://kf.cf.com/NewQueryInfoPages/GetFundRatePageRedeem.aspx?RedeemType=NonMonetaryFundRedeem_view&objid=" + objid;
                string uid = new AccountService().QQ2Uid(lbl_uin.Text);
#if DEBUG
                uid = "123456789";
                lblHB_fund_code.Text = "990001";
                lblHB_spid.Text = "1230258701";
#endif
                parameters[0] = new ZWCheck_Service.Param() { ParamName = "uin", ParamValue = lbl_uin.Text };
                parameters[1] = new ZWCheck_Service.Param() { ParamName = "uid", ParamValue = uid };
                parameters[2] = new ZWCheck_Service.Param() { ParamName = "spid", ParamValue = lbl_spid.Text };
                parameters[3] = new ZWCheck_Service.Param() { ParamName = "fund_code", ParamValue = lbl_fund_code.Text };
                parameters[4] = new ZWCheck_Service.Param() { ParamName = "total_fee", ParamValue = lbl_total_fee.Text };
                parameters[5] = new ZWCheck_Service.Param() { ParamName = "acct_type", ParamValue = lbl_acct_type.Text };
                parameters[6] = new ZWCheck_Service.Param() { ParamName = "channel_id", ParamValue = lbl_channel_id.Text };
                parameters[7] = new ZWCheck_Service.Param() { ParamName = "bind_serialno", ParamValue = lbl_bind_serialno.Text };
                parameters[8] = new ZWCheck_Service.Param() { ParamName = "card_tail", ParamValue = lbl_card_tail.Text };
                parameters[9] = new ZWCheck_Service.Param() { ParamName = "bank_type", ParamValue = lbl_bank_type.Text };
                parameters[10] = new ZWCheck_Service.Param() { ParamName = "cur_type", ParamValue = "1" };
                parameters[11] = new ZWCheck_Service.Param() { ParamName = "redem_type", ParamValue = ddl_redem_type.SelectedValue };
                parameters[12] = new ZWCheck_Service.Param() { ParamName = "client_ip", ParamValue = lbl_client_ip.Text };
                parameters[13] = new ZWCheck_Service.Param() { ParamName = "close_id", ParamValue = lbl_close_id.Text };
                parameters[14] = new ZWCheck_Service.Param() { ParamName = "opt_type", ParamValue = "2" };
                parameters[15] = new ZWCheck_Service.Param() { ParamName = "ImageUrl", ParamValue = ViewState["alPath"].ToString().Trim() };
                parameters[16] = new ZWCheck_Service.Param() { ParamName = "operator", ParamValue = Session["uid"].ToString() };
                parameters[17] = new ZWCheck_Service.Param() { ParamName = "ReturnUrl", ParamValue = ReturnUrl };

                checkService.Finance_HeaderValue = RefundPublicFun.SetWebServiceHeader(this);
                checkService.StartCheck(objid, "NonMonetaryFundRedeem", "非货币基金强赎", "0", parameters);
                WebUtils.ShowMessage(this.Page, "提交成功！");
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "提交失败！" + PublicRes.GetErrorMsg(eSys.ToString()));
                return;
            }
        }


        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void upNON_Click(object sender, EventArgs e)
        {
            try
            {
                upImage(fileNON);
                imgNoN.ImageUrl = ViewState["kfPath"].ToString();
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);

            }
        }
       
        #endregion

        #region 货币基金强赎

        /// <summary>
        /// 生产审批单
        /// </summary>
        protected void MonetaryFundRedeem()
        {
            string uin = Request.QueryString["uin"];
            string spid = Request.QueryString["spid"];
            string fund_code = Request.QueryString["fund_code"];
            string fund_name = Request.QueryString["fund_name"];
            string total_fee = Request.QueryString["total_fee"];
            string acct_type = Request.QueryString["acct_type"];//
            string channel_id = Request.QueryString["channel_id"];
            string bind_serialno = Request.QueryString["bind_serialno"];
            string card_tail = Request.QueryString["card_tail"];
            string bank_type = Request.QueryString["bank_type"];
            string cur_type = Request.QueryString["cur_type"];

            lblHB_uin.Text = uin;
            lblHB_spid.Text = spid;
            lblHB_fund_code.Text = fund_code;
            lblHB_total_fee.Text = total_fee;
            lblHB_channel_id.Text = channel_id;
            lblHB_bind_serialno.Text = bind_serialno;
            lblHB_card_tail.Text = card_tail;
            lblHB_bank_type.Text = bank_type;
            lblHB_cur_type.Text = cur_type;
        }
      
        /// <summary>
        /// 提交审批单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnHB_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["alPath"] == null)
                {
                    WebUtils.ShowMessage(this.Page, "请上传图片！");
                    return;
                }


                ZWCheck_Service.Check_Service checkService = new ZWCheck_Service.Check_Service();
                ZWCheck_Service.Param[] parameters = new ZWCheck_Service.Param[20];

                string objid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string ReturnUrl = "http://kf.cf.com/NewQueryInfoPages/GetFundRatePageRedeem.aspx?RedeemType=MonetaryFundRedeem_view&objid=" + objid;
                string uid = new AccountService().QQ2Uid(lbl_uin.Text);
#if DEBUG
                uid = "123456789";
                lblHB_fund_code.Text = "000397";
                lblHB_spid.Text = "1217608401";
#endif

                parameters[0] = new ZWCheck_Service.Param() { ParamName = "uin", ParamValue = lblHB_uin.Text };
                parameters[1] = new ZWCheck_Service.Param() { ParamName = "uid", ParamValue = uid };
                parameters[2] = new ZWCheck_Service.Param() { ParamName = "spid", ParamValue = lblHB_spid.Text };
                parameters[3] = new ZWCheck_Service.Param() { ParamName = "fund_code", ParamValue = lblHB_fund_code.Text };
                parameters[4] = new ZWCheck_Service.Param() { ParamName = "total_fee", ParamValue = lblHB_total_fee.Text };
                parameters[5] = new ZWCheck_Service.Param() { ParamName = "acct_type", ParamValue = "2" };
                parameters[6] = new ZWCheck_Service.Param() { ParamName = "channel_id", ParamValue = "68|fm_6_qs_1" };
                parameters[7] = new ZWCheck_Service.Param() { ParamName = "bind_serialno", ParamValue = lblHB_bind_serialno.Text };
                parameters[8] = new ZWCheck_Service.Param() { ParamName = "card_name", ParamValue = txtHB_card_name.Text };
                parameters[9] = new ZWCheck_Service.Param() { ParamName = "card_tail", ParamValue = lblHB_card_tail.Text };
                parameters[10] = new ZWCheck_Service.Param() { ParamName = "bank_type", ParamValue = lblHB_bank_type.Text };
                parameters[11] = new ZWCheck_Service.Param() { ParamName = "bank_name", ParamValue = txtHB_bank_name.Text.Trim() };
                parameters[12] = new ZWCheck_Service.Param() { ParamName = "cur_type", ParamValue = "1" };
                parameters[13] = new ZWCheck_Service.Param() { ParamName = "bank_area", ParamValue = txtHB_bank_area.Text.Trim() };
                parameters[14] = new ZWCheck_Service.Param() { ParamName = "bank_city", ParamValue = txtHB_bank_city.Text.Trim() };
                parameters[15] = new ZWCheck_Service.Param() { ParamName = "redem_type", ParamValue = "3"};
                parameters[16] = new ZWCheck_Service.Param() { ParamName = "client_ip", ParamValue = client_ip };
                //parameters[17] = new ZWCheck_Service.Param() { ParamName = "en_buf", ParamValue = "ABCDEFG" };
                parameters[17] = new ZWCheck_Service.Param() { ParamName = "ImageUrl", ParamValue = ViewState["alPath"].ToString().Trim() };
                parameters[18] = new ZWCheck_Service.Param() { ParamName = "operator", ParamValue = Session["uid"].ToString() };
                parameters[19] = new ZWCheck_Service.Param() { ParamName = "ReturnUrl", ParamValue = ReturnUrl };

                checkService.Finance_HeaderValue = RefundPublicFun.SetWebServiceHeader(this);
                checkService.StartCheck(objid, "MonetaryFundRedeem", "货币基金强赎", "0", parameters);
                WebUtils.ShowMessage(this.Page, "提交成功！");
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "提交失败！" + PublicRes.GetErrorMsg(eSys.ToString()));
            }
        }

        protected void upHB_Click(object sender, EventArgs e)
        {
            try
            {
                upImage(fileHB);
                imgHB.ImageUrl = ViewState["kfPath"].ToString();
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, ex.Message);

            }
        }

        /// <summary>
        /// 账务系统查看
        /// </summary>
        protected void MonetaryFundRedeem_view()
        {
            try
            {
                string objid = Request.QueryString["objid"];
                DataTable dt = checkService.GetCheckInfo(objid, "MonetaryFundRedeem");
                if (dt != null && dt.Rows.Count != 0)
                {
                    lblHB_uin.Text = dt.Rows[0]["uin"].ToString();
                    lblHB_spid.Text = dt.Rows[0]["spid"].ToString();
                    lblHB_fund_code.Text = dt.Rows[0]["fund_code"].ToString();
                    lblHB_total_fee.Text = dt.Rows[0]["total_fee"].ToString();
                    lblHB_bind_serialno.Text = dt.Rows[0]["bind_serialno"].ToString();
                    txtHB_card_name.Text = dt.Rows[0]["card_name"].ToString();
                    lblHB_card_tail.Text = dt.Rows[0]["card_tail"].ToString();
                    lblHB_bank_type.Text = dt.Rows[0]["bank_type"].ToString();
                    txtHB_bank_name.Text = dt.Rows[0]["bank_name"].ToString();
                    lblHB_cur_type.Text = dt.Rows[0]["cur_type"].ToString();
                    txtHB_bank_area.Text = dt.Rows[0]["bank_area"].ToString();
                    txtHB_bank_city.Text = dt.Rows[0]["bank_city"].ToString();
                    imgHB.ImageUrl = dt.Rows[0]["ImageUrl"].ToString();
                    btnHB.Visible = false;
                    upHB.Visible = false;
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "申请单查询失败！" + PublicRes.GetErrorMsg(eSys.ToString()));
            }
        }
       #endregion
       
      

        protected void upImage(HtmlInputFile file)
        {
            try
            {
                //上传需要的图片，并返回对应服务器上的地址
                //存放文件
                // string s1 = File1.Value;
                if (file.Value == "")
                {
                    throw new Exception("请上传图片");
                }
                string szTypeName = Path.GetExtension(file.Value);
                string upStr = null;


                if (szTypeName.ToLower() != ".jpg" && szTypeName.ToLower() != ".gif" && szTypeName.ToLower() != ".bmp")
                {
                    throw new Exception("上传的文件不正确，必须为jpg,gif,bmp");
                }

                string fileName = "s1" + DateTime.Now.ToString("yyyyMMddHHmmss") + szTypeName;
                upStr = "uploadfile/" + DateTime.Now.ToString("yyyyMMdd") + "/CSOMS/RedemptionFund";

                string targetPath = Path.Combine(Server.MapPath(Request.ApplicationPath), upStr.Replace("/", "\\"));
                PublicRes.CreateDirectory(targetPath);

                file.PostedFile.SaveAs(Path.Combine(targetPath, fileName));

                if (AppSettings.Get<bool>("FPSenabled", false))
                {
                    //前期 先保留 老的  图片存储方式
                    var result = commLib.FPSFileHelper.UploadFile(file.PostedFile.InputStream, "RedemptionFund/" + fileName);
                    ViewState["alPath"] = ViewState["kfPath"] = result.url;
                }
                else
                {
                    ViewState["kfPath"] = "/" + upStr + "/" + fileName;
                    ViewState["alPath"] = System.Configuration.ConfigurationManager.AppSettings["GetImageFromKf2Url"].ToString() + ViewState["kfPath"].ToString();
                }

            }
            catch (Exception eStr)
            {
                throw new Exception("上传文件失败！", eStr);
            }
        }
    }
}