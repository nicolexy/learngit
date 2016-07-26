using System;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.RefundModule;
using CFT.CSOMS.BLL.InternetBank;
using System.Data;
using System.Collections.Generic;
using CFT.CSOMS.BLL.WechatPay;
using CFT.Apollo.Logging;
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.InternetBank
{
	/// <summary>
    /// RefundInfo 的摘要说明。
	/// </summary>
    public partial class RefundInfo : TENCENT.OSS.CFT.KF.KF_Web.PageBase
	{
        private string listid;
        protected static List<int> refundIdList = new List<int>();
        
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
                cftOrderId.Attributes["onblur"] = ClientScript.GetPostBackEventReference(btnAmount, null);

                if (!IsPostBack)
                {
                    refundIdList.Clear();
                    DataSet ds = new InternetBankService().GetRefundByFrefundId(0, "", "", 0, 0);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                refundIdList.Add(Convert.ToInt32(item["Frefund_id"]));
                            }
                        }
                    }
                    
                    if (Request.QueryString["listid"] != null && Request.QueryString["listid"].Trim() != "")
                    {
                        listid = Request.QueryString["listid"].Trim();
                        ViewState["listid"] = listid;
                    }
                    else
                    {
                        listid = "";
                        ViewState["listid"] = listid;
                    }


                    if (listid == "")
                    {
                        if (!classLibrary.ClassLib.ValidateRight("RefundCheck", this))
                        {
                            Response.Redirect("../login.aspx?wh=1");
                        }
                        labTitle.Text = "新增退款登记";
                        statevisible.Visible = false;
                    }
                    else
                    {
                        labTitle.Text = "修改退款登记";
                        statevisible.Visible = false;
                        BindData(listid);
                    }
                }
                else
                {
                    listid = ViewState["listid"].ToString();
                }
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}
		}

        private void BindData(string listid)
		{
            try 
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();

                Query_Service.RefundInfoClass sc = qs.GetRefundDetail(listid);
                if (sc != null)
                {
                    //绑定开始
                    cftOrderId.Text = sc.FOrderId;
                    string refund_type = sc.FRefund_type.ToString();
                    if (refund_type == "1" || refund_type == "2" || refund_type == "3" || refund_type == "4" || refund_type == "5")
                    {
                        ddlRefundType.SelectedValue = "10";
                    }
                    else 
                    {
                        ddlRefundType.SelectedValue = refund_type;
                    }

                    //ddlRefundStatus.SelectedValue = sc.FSubmit_refund.ToString(); //提交退款
                    memo.Text = sc.FSam_no;
                    recycUser.Text = sc.FRecycle_user;
                    //bussNumber.ReadOnly = true;
                    tbAmount.Text = classLibrary.setConfig.FenToYuan(Convert.ToDouble(sc.FAmount));
                    tbRefundAmount.Text = classLibrary.setConfig.FenToYuan(Convert.ToDouble(sc.FRefund_amount));
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "未读取到数据！");
                }
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
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

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
            if (Session["uid"] == null)
            {
                Response.Redirect("../login.aspx?wh=1");
            }
            try 
            {
                //判断交易金额，退款金额
                string sAmount = tbAmount.Text;
                int amount = 0;
                if (!string.IsNullOrEmpty(sAmount)) 
                {
                    sAmount = sAmount.Replace("元", "");
                    amount = classLibrary.setConfig.YuanToFen(Convert.ToDouble(sAmount));
                    
                }
                
                string sRefundAmount = tbRefundAmount.Text;
                int refundAmount = 0;
                if (!string.IsNullOrEmpty(sRefundAmount))
                {
                    sRefundAmount = sRefundAmount.Replace("元", "");
                    refundAmount = classLibrary.setConfig.YuanToFen(Convert.ToDouble(sRefundAmount));
                }
                if (refundAmount <= 0) 
                {
                    WebUtils.ShowMessage(this.Page, "退款金额应大于0！");
                    return;
                }
                if (refundAmount > amount) 
                {
                    WebUtils.ShowMessage(this.Page, "退款金额小于或等于订单金额！");
                    return;
                }
                string orderId = cftOrderId.Text.Trim();
                if (!string.IsNullOrEmpty(orderId) && orderId.Length >= 10)
                {
                    if (!refundIdList.Contains(Convert.ToInt32(orderId.Substring(0, 10))))
                    {
                        WebUtils.ShowMessage(this.Page, "该商家的订单不允许走网银退款。");
                        return;
                    }
                }

                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                Query_Service.RefundInfoClass cb = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.RefundInfoClass();
                cb.FOrderId = cftOrderId.Text.Trim();
                cb.FRefund_type = int.Parse(ddlRefundType.SelectedValue);
                cb.FSam_no = memo.Text.Trim(); //SAM工单号
                cb.FRecycle_user = recycUser.Text.Trim(); //物品回收人
                cb.FSubmit_user = Session["uid"].ToString(); //登记人
                cb.FRefund_amount = refundAmount.ToString(); //退款金额

                if (cb.FOrderId == "")
                {
                    WebUtils.ShowMessage(this.Page, "请输入财付通订单号");
                    return;
                }

                //修改
                if (listid != "")
                {
                    //修改。
                    cb.FId = int.Parse(listid);
                    cb.FRefund_state = 1;
                    cb.FSubmit_refund = 3;//失效
                    qs.ChangeRefundInfo(cb);
                    WebUtils.ShowMessage(this.Page, "修改成功");
                }
                else
                {
                    //新增
                    cb.FRefund_state = 1;
                    cb.FSubmit_refund = 3;//失效
                    qs.AddRefundInfo(cb);
                    WebUtils.ShowMessage(this.Page, "新增成功");
                }
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }
		}

        protected void btnAmount_Click(object sender, System.EventArgs e)
        {
            string listid = cftOrderId.Text.Trim();

            bool flag = false;
            try
            {
                DataSet ds =  new TradeService().GetPayByListid(listid); //查询微信转账业务
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    string wxTradeId = ds.Tables[0].Rows[0]["Fcoding"].ToString();//子账户关联订单号
                    if (wxTradeId.Contains("mkt") || wxTradeId.Contains("wxp"))
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                flag = true;
                LogHelper.LogError("查询微信转账业务异常  protected void btnAmount_Click(object sender, System.EventArgs e) ：" + ex.ToString());
                WebUtils.ShowMessage(this.Page, "查询微信转账业务异常：" + ex.Message + ex.StackTrace);
                return;
            }

            if (flag)
            {
                this.btnSave.Visible = false;
                WebUtils.ShowMessage(this.Page, "当前订单为微信大单，禁止录入！");
            }
            else
                this.btnSave.Visible = true;

            if (string.IsNullOrEmpty(listid))
            {
                return;
            }
            string amount = new RefundRegisterService().QueryTradeAmount(listid);
            this.tbAmount.Text = classLibrary.setConfig.FenToYuan(amount);
        }
	}
}