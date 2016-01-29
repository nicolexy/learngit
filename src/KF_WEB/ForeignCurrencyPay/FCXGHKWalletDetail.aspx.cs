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
	/// OrderDetail ��ժҪ˵����
	/// </summary>
    public partial class FCXGHKWalletDetail : PageBase
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
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
                    WebUtils.ShowMessage(this.Page, "��������");
                    return;
				}

				try
				{
                    var typeid = int.Parse(typeval);
                    DateTime tempdate = DateTime.Now;
                    if (typeid == 2 && (string.IsNullOrEmpty(qry_time) || !DateTime.TryParse(qry_time, out tempdate)))
                    {
                        WebUtils.ShowMessage(this.Page, "��������");
                        return;
                    }

                    BindInfo(typeid, listid, qry_time);
				}
				catch(Exception eSys)
				{
                    LogError("ForeignCurrencyPay.FCXGHKWalletDetail", "BindInfo(typeid, listid, qry_time);", eSys);
					WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
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
                WebUtils.ShowMessage(this.Page, "δ�ҵ���¼");
            }

            //hb_orderΪ���������
            var hborder = packageData.hb_order;
            if (hborder != null)
            {
                var hbtypeStr = string.IsNullOrEmpty(hborder.hb_type) ? "" : hborder.hb_type == "2" ? "�̶�ֵ���" : hborder.hb_type == "1" ? "ƴ�������" : "";// 1ƴ������2�̶�ֵ���
                this.lb_hb_type.Text = hbtypeStr;
                this.lb_send_listid.Text = hborder.send_listid;
                this.lb_create_time.Text = hborder.create_time;

                string paystate = string.Empty;
                //hattiyzhang(�ŷ�) 01-29 16:10:51
                //1 �ȴ�����
                //2 �Ѹ���
                if (!string.IsNullOrEmpty(hborder.pay_state)) {
                    if (hborder.pay_state == "1") {
                        paystate = "�ȴ�����";
                    }
                    else if (hborder.pay_state == "2")
                    {
                        paystate = "�Ѹ���";
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
                this.lb_pay_means.Text = string.IsNullOrEmpty(hborder.pay_means) ? "" : hborder.pay_means == "2" ? "���" : hborder.pay_means == "1" ? "���п�" : "";//1.���п�,2.���
                this.lb_card_num.Text = hborder.card_num;
                this.lb_recv_amount.Text = MoneyTransfer.FenToYuan(hborder.received_amount) ;
                this.lb_recv_num.Text = hborder.received_num;
                this.lb_invalid_time.Text = hborder.invalid_time;
                this.lb_refund_amount.Text = MoneyTransfer.FenToYuan(hborder.refund_amount) ;
            }
            else
            {
                throw new LogicException("δ�ҵ�����������¼��");
            }

            //hb_itemsΪ�պ������
            var hbitems =  packageData.hb_items;
            if (hbitems!=null&& hbitems.ret_num>0)
			{
                this.dgReceivePackage.DataSource = hbitems.hb_list;
                this.dgReceivePackage.DataBind();
			}
			else
			{
                throw new LogicException("δ�ҵ��պ�������¼��");
			}
		}

        /// <summary>
        /// ��ȡ���е�����ֵ
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
        /// ��ȡ��ע��Ϣ
        /// </summary>
        /// <param name="objval"></param>
        /// <returns></returns>
        public string GetMemo(object objval)
        {
            ///��ע�ǿ飺
            //2����ʾ�������
            //1����ʾ�����������
            //�ĵ���д�ţ�ֻ��2���������ʱ����ʾ
            string memo = string.Empty;
            try
            {
                if (objval != null)
                {
                    var strv = objval.ToString();
                    if (strv == "2")
                    {
                        memo = "�������";
                    }
                }
                return memo;
            }
            catch
            {
                return string.Empty;
            }
        }

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

	}
}
