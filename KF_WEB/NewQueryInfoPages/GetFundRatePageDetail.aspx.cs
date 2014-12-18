using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Xml;
using CFT.CSOMS.BLL.ForeignCurrencyModule;
using TENCENT.OSS.CFT.KF.KF_Web.InternetBank;
using CFT.CSOMS.BLL.SysManageModule;
using System.IO;
using CFT.CSOMS.DAL.CheckModoule;
using CFT.CSOMS.COMMLIB;

namespace TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages
{
	/// <summary>
    /// QueryYTTrade 的摘要说明。
	/// </summary>
    public partial class GetFundRatePageDetail : System.Web.UI.Page
	{
        private string close_flag, uin, spid, fund_code, total_fee, end_date, bind_serialno, card_tail, mobile, bank_type;
        CheckService checkService = new CheckService();
        static Hashtable images;
        protected void Page_Load(object sender, System.EventArgs e)
		{

			try
			{

                if (!IsPostBack)
                {
                    if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
                    if (Request.QueryString["opertype"] != null && Request.QueryString["opertype"].Trim() != "")
                    {
                        string opertype = Request.QueryString["opertype"].Trim();

                        if (opertype == "1")//客服系统提交审批
                        {
                            #region
                            if (!string.IsNullOrEmpty(Request.QueryString["close_flag"]) &&
                                !string.IsNullOrEmpty(Request.QueryString["uin"]) &&
                                !string.IsNullOrEmpty(Request.QueryString["spid"]) &&
                                !string.IsNullOrEmpty(Request.QueryString["fund_code"]) &&
                                !string.IsNullOrEmpty(Request.QueryString["total_fee"]))
                            {//必传参数
                                close_flag = Request.QueryString["close_flag"].Trim();
                                ViewState["close_flag"] = close_flag;
                                uin = Request.QueryString["uin"].Trim();
                                ViewState["uin"] = uin;
                                spid = Request.QueryString["spid"].Trim();
                                ViewState["spid"] = spid;
                                fund_code = Request.QueryString["fund_code"].Trim();
                                ViewState["fund_code"] = fund_code;
                                total_fee = Request.QueryString["total_fee"].Trim();
                                ViewState["total_fee"] = total_fee;
                            //   ViewState["total_fee"] = "0";
                            }
                            else
                            {
                                Response.Redirect("../login.aspx?wh=1");
                            }

                            if (Request.QueryString["close_flag"].ToString() == "2")//封闭
                            {
                                if (!string.IsNullOrEmpty(Request.QueryString["end_date"]))
                                {
                                    ViewState["end_date"] = Request.QueryString["end_date"].Trim();
                                }
                                else
                                {
                                    Response.Redirect("../login.aspx?wh=1");
                                }

                                this.tableCloseInput.Visible = true;
                                this.tableCloseApply.Visible = false;
                                this.tableUNCloseApply.Visible = false;
                            }

                            if (Request.QueryString["close_flag"].ToString() == "1")//不封闭
                            {
                                if (!string.IsNullOrEmpty(Request.QueryString["bind_serialno"]))
                                {
                                    ViewState["bind_serialno"]  = Request.QueryString["bind_serialno"].Trim();
                                }
                                else
                                {
                                    Response.Redirect("../login.aspx?wh=1");
                                }
                                if (!string.IsNullOrEmpty(Request.QueryString["card_tail"]))
                                {
                                    ViewState["card_tail"]  = Request.QueryString["card_tail"].Trim();
                                }
                                else
                                {
                                    Response.Redirect("../login.aspx?wh=1");
                                }
                                if (!string.IsNullOrEmpty(Request.QueryString["bank_type"]))
                                {
                                    ViewState["bank_type"]  = Request.QueryString["bank_type"].Trim();
                                }
                                else
                                {
                                    Response.Redirect("../login.aspx?wh=1");
                                }
                                  if (!string.IsNullOrEmpty(Request.QueryString["mobile"]))
                                {
                                    ViewState["mobile"] = Request.QueryString["mobile"].Trim();
                                }
                                else
                                {
                                    Response.Redirect("../login.aspx?wh=1");
                                }
                                this.tableCloseInput.Visible = false;
                                this.tableCloseApply.Visible = false;
                                CreateApplyUNClose();
                            }
                            #endregion
                        }
                        else//账务系统查看
                        {
                            tableCloseInput.Visible = false;
                           // tableUNCloseInput.Visible = false;
                            this.tableCloseApply.Visible = false;
                            if (Request.QueryString["objid"] != null && Request.QueryString["objid"].Trim() != "")
                            {
                                string objid = Request.QueryString["objid"].Trim();
                                ViewState["objid"] = objid;
                                if (Request.QueryString["close_flag"].ToString() == "2")//封闭
                                    bindCloseApply(objid);
                                if (Request.QueryString["close_flag"].ToString() == "1")//不封闭
                                    bindUNCloseApply(objid);
                            }
                        }
                    }
                    else
                        Response.Redirect("../login.aspx?wh=1");

                    images = new Hashtable();
                }
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}
            
           
		}

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion
        
        //封闭 产生申请单
        public void btnCreateApplyClose_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                {
                    throw new Exception("截止日期为空！");
                }
                    upImage(FileClose);

                tb_Cuin.Text = ViewState["uin"].ToString();
                tb_Cspid.Text = ViewState["spid"].ToString();
                tb_Cfund_code.Text = ViewState["fund_code"].ToString();
                tb_Ctotal_fee.Text = ViewState["total_fee"].ToString();
                tb_Cend_date.Text = ViewState["end_date"].ToString();
                tb_Cend_dateHand.Text = txtEndDate.Text.Trim();
                string ip = Request.UserHostAddress.ToString();
                if (ip == "::1")
                    ip = "127.0.0.1";
                tb_Cclient_ip.Text = ip;
                this.ImageC.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["GetImageFromKf2Url"].ToString() + ViewState["alPath"].ToString();


                this.tableCloseApply.Visible = true;
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "生成申请单失败！" + eSys.Message.ToString());
                return;
            }
        }

        //封闭 提交申请单
        public void btnSubmitCloseApply_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (tb_Cend_date.Text.Trim() != tb_Cend_dateHand.Text.Trim())
                {
                    WebUtils.ShowMessage(this.Page, "客服手工输入截止日期 与 基金截至日期 不一致！");
                    return;
                }

                string objid = System.DateTime.Now.ToString("yyyyMMddHHmmss");
                string ReturnUrl = "http://kf.cf.com/NewQueryInfoPages/GetFundRatePageDetail.aspx?close_flag=2&objid=" + objid+ "&opertype=0";
                string[,] param = new string[,] {  { "close_flag", ViewState["close_flag"].ToString() },
                                { "uin", ViewState["uin"].ToString() }, 
                                { "spid", ViewState["spid"].ToString() },
                                { "fund_code", ViewState["fund_code"].ToString() },
                                { "total_fee", ViewState["total_fee"].ToString() },
                                { "acct_type", "2" },
                                { "channel_id","stat_type=68|fm_6_qs_1"},//透传
                                { "redem_type", "3" },
                                { "end_date", tb_Cend_date.Text.Trim() },
                                { "end_date_hand", tb_Cend_dateHand.Text.Trim() },//客服手工输入截止日期
                                { "client_ip",tb_Cclient_ip.Text.Trim() },
                                { "ImageUrl",ImageC.ImageUrl.Trim() },
                                { "operator",Session["uid"].ToString() },
                                { "ReturnUrl",ReturnUrl},
                              };

                Check_WebService.Param[] pa = PublicRes.ToParamArray(param);
                string memo = "客服系统提交强赎审批信息，close_flag:2|uin:" + ViewState["uin"].ToString() 
                             + "|spid:" + ViewState["spid"].ToString()
                             + "|fund_code:" + ViewState["fund_code"].ToString()
                             + "|total_fee:" + ViewState["total_fee"].ToString()
                             + "|end_date:" + tb_Cend_date.Text.Trim();
                SunLibrary.LoggerFactory.Get("GetFundRatePageDetail").Info(memo);
                PublicRes.CreateCheckService(this).StartCheck(objid, "CloseRedemFundType", memo, "0", pa);
              //  WebUtils.ShowMessage(this.Page, "已提交，请等待审批！");
                this.ShowMsg("已提交，请等待审批！");

            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "提交申请单失败：" + eSys.Message.ToString());
                return;
            }
        }

        //封闭 账务系统查看
        public void bindCloseApply(string objid)
        {
            try
            {
                DataTable dt = checkService.GetCheckInfo(objid, "CloseRedemFundType");
               if (dt != null && dt.Rows.Count != 0)
               {
                   tb_Cuin.Text = dt.Rows[0]["uin"].ToString();
                   tb_Cspid.Text = dt.Rows[0]["spid"].ToString();
                   tb_Cfund_code.Text = dt.Rows[0]["fund_code"].ToString();
                   tb_Ctotal_fee.Text = dt.Rows[0]["total_fee"].ToString();
                   tb_Cend_date.Text = dt.Rows[0]["end_date"].ToString();
                   tb_Cend_dateHand.Text = dt.Rows[0]["end_date_hand"].ToString();
                   tb_Cclient_ip.Text = dt.Rows[0]["client_ip"].ToString();
                   this.ImageC.ImageUrl = dt.Rows[0]["ImageUrl"].ToString();
                  this.tableCloseApply.Visible = true;
                //  this.tableUNCloseApply.Visible = false;
                  this.ButtonSubmitClose.Visible = false;
               }
               else
               {
                   WebUtils.ShowMessage(this.Page, "查询出错！");
                   return;
               }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "申请单查询失败！" + eSys.Message.ToString());
                return;
            }
        }

      
        ////不封闭 产生申请单
        //public void btnCreateApplyUNClose_Click(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(tbtrue_name_new.Text.Trim()))
        //        {
        //            throw new Exception("开户新姓名为空");
        //        }
        //        if (string.IsNullOrEmpty(tbbank_id_new.Text.Trim()))
        //        {
        //            throw new Exception("新银行账号为空");
        //        }
        //        if (string.IsNullOrEmpty(tbaddress.Text.Trim()))
        //        {
        //            throw new Exception("所在城市（省、市）为空");
        //        }
        //        if (string.IsNullOrEmpty(tbbank_type_new.Text.Trim()))
        //        {
        //            throw new Exception("新开户银行和支行名称为空");
        //        }
        //        if (string.IsNullOrEmpty(tbcert_id.Text.Trim()))
        //        {
        //            throw new Exception("身份证号码为空");
        //        }

        //        if (images.Count==0)
        //        {
        //            images.Add("FileUNClose1","");
        //            images.Add("FileUNClose2", "");
        //            images.Add("FileUNClose3", "");
        //            images.Add("FileUNClose4", "");
        //        }
        //        if (images["FileUNClose1"] == "")
        //        {
        //            upImage(FileUNClose1);
        //            images["FileUNClose1"] = ViewState["alPath"].ToString();
        //        }
        //        if (images["FileUNClose2"] == "")
        //        {
        //            upImage(FileUNClose2);
        //            images["FileUNClose2"] = ViewState["alPath"].ToString();
        //        }
        //        //银行扫描件可不上传
        //        if (FileUNClose3.Value != "")
        //        {
        //            upImage(FileUNClose3);
        //            images["FileUNClose3"] = ViewState["alPath"].ToString();
        //        }

        //        if (images["FileUNClose4"] == "")
        //        {
        //            upImage(FileUNClose4);
        //            images["FileUNClose4"] = ViewState["alPath"].ToString();
        //        }

        //        tb_UNCuin.Text = ViewState["uin"].ToString();
        //        tb_UNCspid.Text = ViewState["spid"].ToString();
        //        tb_UNCfund_code.Text = ViewState["fund_code"].ToString();
        //        tb_UNCtotal_fee.Text = ViewState["total_fee"].ToString();
        //        tb_UNCtrue_name.Text = ViewState["true_name"].ToString();
        //        tb_UNCbank_id.Text = ViewState["bank_id"].ToString();
        //        tb_UNCtrue_name_new.Text = tbtrue_name_new.Text.Trim();
        //        tb_UNCbank_id_new.Text = tbbank_id_new.Text.Trim();
        //        tb_UNCbank_type.Text = ViewState["bank_type"].ToString();
        //        tb_UNCaddress.Text = tbaddress.Text.Trim();
        //        tb_UNCbank_type_new.Text = tbbank_type_new.Text.Trim();
        //        tb_UNCcert_id.Text = tbcert_id.Text.Trim();
        //        this.ImageUNC1.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["KFWebUrl"].ToString() + images["FileUNClose1"];
        //        this.ImageUNC2.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["KFWebUrl"].ToString() + images["FileUNClose2"];
        //        if (images["FileUNClose3"].ToString()!="")
        //            this.ImageUNC3.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["KFWebUrl"].ToString() + images["FileUNClose3"];
        //        this.ImageUNC4.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["KFWebUrl"].ToString() + images["FileUNClose4"];

        //        this.tableUNCloseApply.Visible = true;
        //    }
        //    catch (Exception eSys)
        //    {
        //        WebUtils.ShowMessage(this.Page, "生成申请单失败！" + eSys.Message.ToString());
        //        return;
        //    }
        //}

        //不封闭 产生申请单
        public void CreateApplyUNClose()
        {
            try
            {
                tb_UNCuin.Text = ViewState["uin"].ToString();
                tb_UNCspid.Text = ViewState["spid"].ToString();
                tb_UNCfund_code.Text = ViewState["fund_code"].ToString();
                tb_UNCtotal_fee.Text = ViewState["total_fee"].ToString();
                tb_UNCbind_serialno.Text = ViewState["bind_serialno"].ToString();
                tb_UNCcard_tail.Text = ViewState["card_tail"].ToString();
                tb_UNCbank_type.Text = ViewState["bank_type"].ToString();
                tb_UNmobile.Text = ViewState["mobile"].ToString();
                string ip = Request.UserHostAddress.ToString();
                if (ip == "::1")
                    ip = "127.0.0.1";
                this.tb_UNCclient_ip.Text = ip;
                this.tableUNCloseApply.Visible = true;
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "生成申请单失败！" + eSys.Message.ToString());
                return;
            }
        }

        /// <summary>
        /// 不封闭 提交申请单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnSubmitUNCloseApply_Click(object sender, System.EventArgs e)
        {
            try
            {
                string objid = System.DateTime.Now.ToString("yyyyMMddHHmmss");
                string ReturnUrl = "http://kf.cf.com/NewQueryInfoPages/GetFundRatePageDetail.aspx?close_flag=1&objid=" + objid + "&opertype=0";

        //        string bankID_Encode = CommUtil.EncryptZerosPadding(tb_UNCbank_id_new.Text.Trim());
                string[,] param = new string[,] {  { "close_flag", ViewState["close_flag"].ToString() },
                                { "uin", ViewState["uin"].ToString() }, 
                                { "spid", ViewState["spid"].ToString() },
                                { "total_fee", ViewState["total_fee"].ToString() },
                                { "fund_code", ViewState["fund_code"].ToString() },
                                { "channel_id","stat_type=68|fm_6_qs_2"},
                                { "bind_serialno", ViewState["bind_serialno"].ToString() },
                                { "card_tail", ViewState["card_tail"].ToString() },
                                { "bank_type",ViewState["bank_type"].ToString() },
                                { "cur_type","1" },
                                { "redem_type", "1" },//赎回类型 1 t+0  2 t+1
                                { "client_ip", this.tb_UNCclient_ip.Text.Trim()},
                                { "mobile",ViewState["mobile"].ToString() },
                                { "operator",Session["uid"].ToString() },
                                { "ReturnUrl",ReturnUrl},
                               };

                Check_WebService.Param[] pa = PublicRes.ToParamArray(param);
                string memo = "客服系统提交强赎审批信息，close_flag:1|uin:" + ViewState["uin"].ToString()
                             + "|spid:" + ViewState["spid"].ToString()
                             + "|fund_code:" + ViewState["fund_code"].ToString()
                             + "|total_fee:" + ViewState["total_fee"].ToString()
                             + "|bind_serialno:" + ViewState["bind_serialno"].ToString()
                             + "|card_tail:" + ViewState["card_tail"].ToString()
                             + "|bank_type:" + ViewState["bank_type"].ToString()
                             + "|mobile:" + ViewState["mobile"].ToString();
                SunLibrary.LoggerFactory.Get("GetFundRatePageDetail").Info(memo);
                PublicRes.CreateCheckService(this).StartCheck(objid, "UnCloseRedemFundType", memo, "0", pa);
                // WebUtils.ShowMessage(this.Page, "已提交，请等待审批！");
                this.ShowMsg("已提交，请等待审批！");
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "提交申请单失败：" + eSys.Message.ToString());
                return;
            }
        }

        ////不封闭 账务系统查看
        public void bindUNCloseApply(string objid)
        {
            try
            {
                DataTable dt = checkService.GetCheckInfo(objid, "UnCloseRedemFundType");
                if (dt != null && dt.Rows.Count != 0)
                {
                    tb_UNCuin.Text = dt.Rows[0]["uin"].ToString();
                    tb_UNCspid.Text = dt.Rows[0]["spid"].ToString();
                    tb_UNCfund_code.Text = dt.Rows[0]["fund_code"].ToString();
                    tb_UNCtotal_fee.Text = dt.Rows[0]["total_fee"].ToString();
                    tb_UNCbind_serialno.Text = dt.Rows[0]["bind_serialno"].ToString();
                    tb_UNCcard_tail.Text = dt.Rows[0]["card_tail"].ToString();
                    tb_UNmobile.Text = dt.Rows[0]["mobile"].ToString();
                    tb_UNCbank_type.Text = dt.Rows[0]["bank_type"].ToString();
                    tb_UNCclient_ip.Text = dt.Rows[0]["client_ip"].ToString();
                    this.tableCloseApply.Visible = false;
                    this.tableUNCloseApply.Visible = true;
                    this.btnSubmitUNClose.Visible = false;
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "查询出错！");
                    return;
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "申请单查询失败！" + eSys.Message.ToString());
                return;
            }
        }
        protected void upImage(HtmlInputFile file)
        {
            try
            {
                //上传需要的图片，并返回对应服务器上的地址
                //存放文件
               // string s1 = File1.Value;
                string s1 = file.Value;
                if (s1 == "")
                {
                    throw new Exception("请上传图片");
                }
                string szTypeName = s1.Substring(s1.Length - 4, 4);
                string alPath;
                HtmlInputFile inputFile = file;
                string upStr = null;


                if (szTypeName.ToLower() != ".jpg" && szTypeName.ToLower() != ".gif" && szTypeName.ToLower() != ".bmp")
                {
                    throw new Exception("上传的文件不正确，必须为jpg,gif,bmp");
                }

                if (inputFile.Value != "")
                {
                    string fileName = "s1" + DateTime.Now.ToString("yyyyMMddHHmmss") + szTypeName; //
                 //   upStr = "uploadfile/RedemptionFund";//System.Configuration.ConfigurationManager.AppSettings["uploadPath"].ToString();
                    upStr = "uploadfile/" + System.DateTime.Now.ToString("yyyyMMdd") + "/CSOMS/RedemptionFund";//System.Configuration.ConfigurationManager.AppSettings["uploadPath"].ToString();
                    string targetPath = Server.MapPath(Request.ApplicationPath) + "\\" + upStr;
                    PublicRes.CreateDirectory(targetPath);

                    string path = Server.MapPath(Request.ApplicationPath) + "\\" + upStr + "\\" + fileName;
                    inputFile.PostedFile.SaveAs(path);

                    //alPath.Add(upStr+ "/" +fileName);	
                    alPath = upStr + "/" + fileName;
                    ViewState["alPath"] = alPath;
                }
                else
                {
                    throw new Exception("请上传正确的图片");
                }
            }
            catch (Exception eStr)
            {
                string errMsg = "上传文件失败！" + eStr.Message.ToString().Replace("'", "’");
                throw new Exception(errMsg);
            }
        }

        private void ShowMsg(string msg)
        {
            Response.Write("<script language=javascript>window.operner=null;alert('" + msg + "');window.close();</script>");
        }
	}
}