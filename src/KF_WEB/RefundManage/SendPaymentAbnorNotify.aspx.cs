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
using System.Collections.Generic;
using System.Configuration;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using System.Text;
using CFT.CSOMS.BLL.RefundModule;
using TENCENT.OSS.C2C.Finance.BankLib;
using System.Threading;
using SunLibrary;

namespace TENCENT.OSS.CFT.KF.KF_Web.RefundManage
{
	/// <summary>
    /// QueryYTTrade 的摘要说明。
	/// </summary>
    public partial class SendPaymentAbnorNotify : System.Web.UI.Page
	{
        protected RefundService refundService = new RefundService();
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");

                    if (Request.QueryString["by"] != null && Request.QueryString["by"].Trim() != "")
                    {
                        string by = Request.QueryString["by"].Trim();
                        ViewState["by"] = by;

                        if (by == "listid")//通过单号发通知
                        {
                            ArrayList listid = (ArrayList)Session["AbnorListid"];
                            ViewState["AbnorListid"] = listid;
                            Session.Remove("AbnorListid");

                            if (listid == null || listid.Count == 0)
                            {
                                showMsg("未选中需要操作的数据！");
                            }
                        }
                    }
                    else
                        Response.Redirect("../login.aspx?wh=1");

                    if (Request.QueryString["notifyType"] != null && Request.QueryString["notifyType"].Trim() != "")
                    {
                        ViewState["notifyType"] = Request.QueryString["notifyType"].Trim();
                        this.tbNotifyType.Text = RefundService.notifyTypeht[ViewState["notifyType"].ToString()].ToString();
                    }
                    else
                        Response.Redirect("../login.aspx?wh=1");

                    GetAndClearSession();
                    BindData(ViewState["notifyType"].ToString());
                }
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }
           
            btnSendNotify.Attributes["onClick"] = "if(!confirm('确定要发送通知吗？')) return false;";
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

        private void BindData(string notifyType)
        {
            try
            {
                this.tbDate.Text = DateTime.Parse(ViewState["eTime"].ToString()).ToString("yyyy-MM-dd");
                this.tbBatchID.Text = ViewState["batchID"].ToString();
                this.tbPackageID.Text = ViewState["packageID"].ToString();
                this.tblistid.Text = ViewState["listid"].ToString();
                this.tbtype.Text = RefundService.typeht[ViewState["type"].ToString()].ToString();
                this.tbSubTypePay.Text = RefundService.SubTypePay[ViewState["subTypePay"].ToString()].ToString();
                this.tbNotityStatus.Text = RefundService.notifyStatusht[ViewState["notityStatus"].ToString()].ToString();
                this.tbNotityResult.Text = RefundService.notifyResultht[ViewState["notityResult"].ToString()].ToString();
                this.tbErrorType.Text = RefundService.errht[ViewState["errorType"].ToString()].ToString();
                this.tbAccType.Text = RefundService.accht[ViewState["accType"].ToString()].ToString();
                this.tbBankType.Text = BankIO.QueryBankName(ViewState["bankType"].ToString());

                string imgUrl = "";
                if (notifyType == "1")
                    imgUrl = "../Images/template/WXNotifyTmp.jpg";
                else if (notifyType == "4")
                    imgUrl = "../Images/template/MESNotifyTmp.jpg";
                this.imgExample.ImageUrl = imgUrl;
            }
            catch (Exception eSys)
            {
                showMsg("读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        protected void btnSendNotify_Click(object sender, System.EventArgs e)
        {
            string delayReason=this.tbDelayReason.Text.Trim();
            string accTime = this.tbToAccTime.Text.Trim();
            if (string.IsNullOrEmpty(delayReason) || string.IsNullOrEmpty(accTime))
            {
                showMsg("请输入延迟理由及到账时间！"); 
                return;
            }

            if (delayReason.Length > 32)
            {
                showMsg("延迟理由不能超过32个字符！");
                return;
            }

            try
            {
                DateTime date = DateTime.Parse(accTime);
            }
            catch
            {
                showMsg("到账时间输入有误！"); 
                return;
            }

            showMsg("后台数据处理中，请稍后查询结果,不要连续发送相同条件的通知！");
            btnSendNotify.Visible = false;
            Thread t = new Thread(UpdatePaymenAbnormal);
            t.Start();
        }

        private void UpdatePaymenAbnormal()
        {
            try
            {
                #region 保存参数
                string delayReason = PublicRes.GetString(tbDelayReason.Text.Trim());
                string toAccTime = DateTime.Parse(PublicRes.GetString(tbToAccTime.Text.Trim())).ToString("yyyy-MM-dd HH:mm:ss");
                string batchID = tbBatchID.Text.Trim();
                string packageID = tbPackageID.Text.Trim();
                string listid = tblistid.Text.Trim();
                string sTime = ViewState["sTime"].ToString();
                string eTime = ViewState["eTime"].ToString();
                string type = ViewState["type"].ToString();
                string subTypePay = ViewState["subTypePay"].ToString();
                string notityStatus = ViewState["notityStatus"].ToString();
                string notityResult = ViewState["notityResult"].ToString();
                string bankType = ViewState["bankType"].ToString();
                string errorType = ViewState["errorType"].ToString();
                string accType = ViewState["accType"].ToString();
                string ip = ViewState["client_ip"].ToString();
                #endregion

                #region 需要更新的字段及数据
                Dictionary<string, string> dicData = new Dictionary<string, string>();
                dicData.Add("notify_detail", delayReason);
                dicData.Add("notify_type", ViewState["notifyType"].ToString());
                dicData.Add("pre_arrival_time", toAccTime);
                dicData.Add("notify_sender", ViewState["uid"].ToString());
                dicData.Add("client_ip", ip);

                #endregion

                if (ViewState["by"].ToString() == "listid")
                    refundService.UpdatePaymenAbnormalByListid(sTime, notityStatus, dicData, (ArrayList)ViewState["AbnorListid"]);
                else if (ViewState["by"].ToString() == "condition")
                {
                    #region 查询条件字段及数据
                    Dictionary<string, string> dicCondition = new Dictionary<string, string>();
                    dicCondition.Add("sTime", sTime);
                    dicCondition.Add("eTime", eTime);
                    dicCondition.Add("batchID", batchID);
                    dicCondition.Add("packageID", packageID);
                    dicCondition.Add("listid", listid);
                    dicCondition.Add("type", type);
                    dicCondition.Add("subTypePay", subTypePay);
                    dicCondition.Add("notityStatus", notityStatus);
                    dicCondition.Add("notityResult", notityResult);
                    dicCondition.Add("bankType", bankType);
                    dicCondition.Add("errorType", errorType);
                    dicCondition.Add("accType", accType);
                    #endregion

                    refundService.UpdatePaymenAbnormalByQuery(dicCondition, dicData);
                }

            }
            catch (Exception eSys)
            {
                LogHelper.LogInfo("发送通知失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        private void GetAndClearSession()
        {
            ViewState["eTime"] = Session["eTime"].ToString();
            ViewState["sTime"] = Session["sTime"].ToString();
            ViewState["batchID"] = Session["batchID"].ToString();
            ViewState["packageID"] = Session["packageID"].ToString();
            ViewState["listid"] = Session["listid"].ToString();
            ViewState["type"] = Session["type"].ToString();
            ViewState["subTypePay"] = Session["subTypePay"].ToString();
            ViewState["notityStatus"] = Session["notityStatus"].ToString();
            ViewState["notityResult"] = Session["notityResult"].ToString();
            ViewState["bankType"] = Session["bankType"].ToString();
            ViewState["errorType"] = Session["errorType"].ToString();
            ViewState["accType"] = Session["accType"].ToString();
            ViewState["client_ip"] = Session["client_ip"].ToString();
            ViewState["uid"] = Session["uid"].ToString();

            Session.Remove("eTime");
            Session.Remove("batchID");
            Session.Remove("packageID");
            Session.Remove("listid");
            Session.Remove("type");
            Session.Remove("subTypePay");
            Session.Remove("notityStatus");
            Session.Remove("notityResult");
            Session.Remove("bankType");
            Session.Remove("errorType");
            Session.Remove("accType");
            Session.Remove("client_ip");
        }

        private void showMsg(string msg)
        {
            HttpContext.Current.Response.Write("<script language=javascript>alert('" + msg + "')</script>");
        }
      
	}
}