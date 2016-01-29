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
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.Common;
using CFT.CSOMS.BLL.ForeignCurrencyModule;
using System.Linq;
using System.Collections;


namespace TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay
{
	/// <summary>
	/// OrderDetail 的摘要说明。
	/// </summary>
    public partial class FCXGHKWalletDetail : PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			try
			{
				labUid.Text = Session["uid"].ToString();
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
                string typeval = Request.QueryString["typeid"];
                string listid = Request.QueryString["listid"];
                string qry_time = Request.QueryString["qrytime"];

                if (string.IsNullOrEmpty(typeval) || string.IsNullOrEmpty(listid))
				{
                    WebUtils.ShowMessage(this.Page, "参数有误！");
                    return;
				}

				try
				{
                    var typeid = int.Parse(typeval);
                    DateTime tempdate = DateTime.Now;
                    if (typeid == 2 && (string.IsNullOrEmpty(qry_time) || !DateTime.TryParse(qry_time, out tempdate)))
                    {
                        WebUtils.ShowMessage(this.Page, "参数有误！");
                        return;
                    }

                    BindInfo(typeid, listid, qry_time);
				}
				catch(Exception eSys)
				{
                    LogError("ForeignCurrencyPay.FCXGHKWalletDetail", "BindInfo(typeid, listid, qry_time);", eSys);
					WebUtils.ShowMessage(this.Page,"获取数据失败！" + eSys.Message.ToString());
				}
			}
		}


		private void BindInfo(int typeid,string listid,string qry_time)
		{
            var bll = new FCXGWallet();
            var clientIP = Request.UserHostAddress == "::1" ? "127.0.0.1" : Request.UserHostAddress;
            var packageData = bll.QueryHKPackageDetail(typeid, listid, qry_time, clientIP);
            if (packageData == null )
            {
                WebUtils.ShowMessage(this.Page, "未找到记录");
            }

            //hb_order为发红包详情
            var hborder = packageData.hb_order;
            if (hborder != null)
            {
                var hbtypeStr = string.IsNullOrEmpty(hborder.hb_type) ? "" : hborder.hb_type == "2" ? "固定值红包" : hborder.hb_type == "1" ? "拼手气红包" : "";// 1拼手气，2固定值红包
                this.lb_hb_type.Text = hbtypeStr;
                this.lb_send_listid.Text = hborder.send_listid;
                this.lb_create_time.Text = hborder.create_time;

                string paystate = string.Empty;
                //hattiyzhang(张菲) 01-29 16:10:51
                //1 等待付款
                //2 已付款
                if (!string.IsNullOrEmpty(hborder.pay_state)) {
                    if (hborder.pay_state == "1") {
                        paystate = "等待付款";
                    }
                    else if (hborder.pay_state == "2")
                    {
                        paystate = "已付款";
                    }
                }
                this.lb_pay_state.Text = paystate;
                this.lb_card_type.Text = hborder.card_type;
                this.lb_total_amount.Text = MoneyTransfer.FenToYuan(hborder.total_amount) ;
                this.lb_total_num.Text = hborder.total_num;
                this.lb_fee_amount.Text = MoneyTransfer.FenToYuan(hborder.fee_amount);
                this.lb_refund_time.Text = hborder.refund_time;
                this.lb_refund_listid.Text = hborder.refund_listid;
                this.lb_send_account.Text = hborder.send_account;
                this.lb_send_name.Text = hborder.send_name;
                this.lb_pay_time.Text = hborder.pay_time;
                this.lb_pay_means.Text = string.IsNullOrEmpty(hborder.pay_means) ? "" : hborder.pay_means == "2" ? "余额" : hborder.pay_means == "1" ? "银行卡" : "";//1.银行卡,2.余额
                this.lb_card_num.Text = hborder.card_num;
                this.lb_recv_amount.Text = MoneyTransfer.FenToYuan(hborder.received_amount) ;
                this.lb_recv_num.Text = hborder.received_num;
                this.lb_invalid_time.Text = hborder.invalid_time;
                this.lb_refund_amount.Text = MoneyTransfer.FenToYuan(hborder.refund_amount) ;
            }
            else
            {
                throw new LogicException("未找到发红包详情记录！");
            }

            //hb_items为收红包详情
            var hbitems =  packageData.hb_items;
            if (hbitems!=null&& hbitems.ret_num>0)
			{
                this.dgReceivePackage.DataSource = hbitems.hb_list;
                this.dgReceivePackage.DataBind();
			}
			else
			{
                throw new LogicException("未找到收红包详情记录！");
			}
		}

        /// <summary>
        /// 获取类中的属性值
        /// </summary>
        /// <param name="FieldName"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string GetModelValue(string FieldName, object obj)
        {
            try
            {
                Type Ts = obj.GetType();
                object o = Ts.GetProperty(FieldName).GetValue(obj, null);
                string Value = Convert.ToString(o);
                if (string.IsNullOrEmpty(Value)) return string.Empty;
                return Value;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取备注信息
        /// </summary>
        /// <param name="objval"></param>
        /// <returns></returns>
        public string GetMemo(object objval)
        {
            ///备注那块：
            //2，表示最佳手气
            //1，表示不是最佳手所
            //文档上写着，只有2，最佳手气时才显示
            string memo = string.Empty;
            try
            {
                if (objval != null)
                {
                    var strv = objval.ToString();
                    if (strv == "2")
                    {
                        memo = "最佳手气";
                    }
                }
                return memo;
            }
            catch
            {
                return string.Empty;
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

	}
}
