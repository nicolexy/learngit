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
    /// RefundInfo ��ժҪ˵����
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
                        labTitle.Text = "�����˿�Ǽ�";
                        statevisible.Visible = false;
                    }
                    else
                    {
                        labTitle.Text = "�޸��˿�Ǽ�";
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
                    //�󶨿�ʼ
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

                    //ddlRefundStatus.SelectedValue = sc.FSubmit_refund.ToString(); //�ύ�˿�
                    memo.Text = sc.FSam_no;
                    recycUser.Text = sc.FRecycle_user;
                    //bussNumber.ReadOnly = true;
                    tbAmount.Text = classLibrary.setConfig.FenToYuan(Convert.ToDouble(sc.FAmount));
                    tbRefundAmount.Text = classLibrary.setConfig.FenToYuan(Convert.ToDouble(sc.FRefund_amount));
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "δ��ȡ�����ݣ�");
                }
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
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

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
            if (Session["uid"] == null)
            {
                Response.Redirect("../login.aspx?wh=1");
            }
            try 
            {
                //�жϽ��׽��˿���
                string sAmount = tbAmount.Text;
                int amount = 0;
                if (!string.IsNullOrEmpty(sAmount)) 
                {
                    sAmount = sAmount.Replace("Ԫ", "");
                    amount = classLibrary.setConfig.YuanToFen(Convert.ToDouble(sAmount));
                    
                }
                
                string sRefundAmount = tbRefundAmount.Text;
                int refundAmount = 0;
                if (!string.IsNullOrEmpty(sRefundAmount))
                {
                    sRefundAmount = sRefundAmount.Replace("Ԫ", "");
                    refundAmount = classLibrary.setConfig.YuanToFen(Convert.ToDouble(sRefundAmount));
                }
                if (refundAmount <= 0) 
                {
                    WebUtils.ShowMessage(this.Page, "�˿���Ӧ����0��");
                    return;
                }
                if (refundAmount > amount) 
                {
                    WebUtils.ShowMessage(this.Page, "�˿���С�ڻ���ڶ�����");
                    return;
                }
                string orderId = cftOrderId.Text.Trim();
                if (!string.IsNullOrEmpty(orderId) && orderId.Length >= 10)
                {
                    if (!refundIdList.Contains(Convert.ToInt32(orderId.Substring(0, 10))))
                    {
                        WebUtils.ShowMessage(this.Page, "���̼ҵĶ����������������˿");
                        return;
                    }
                }

                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                Query_Service.RefundInfoClass cb = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.RefundInfoClass();
                cb.FOrderId = cftOrderId.Text.Trim();
                cb.FRefund_type = int.Parse(ddlRefundType.SelectedValue);
                cb.FSam_no = memo.Text.Trim(); //SAM������
                cb.FRecycle_user = recycUser.Text.Trim(); //��Ʒ������
                cb.FSubmit_user = Session["uid"].ToString(); //�Ǽ���
                cb.FRefund_amount = refundAmount.ToString(); //�˿���

                if (cb.FOrderId == "")
                {
                    WebUtils.ShowMessage(this.Page, "������Ƹ�ͨ������");
                    return;
                }

                //�޸�
                if (listid != "")
                {
                    //�޸ġ�
                    cb.FId = int.Parse(listid);
                    cb.FRefund_state = 1;
                    cb.FSubmit_refund = 3;//ʧЧ
                    qs.ChangeRefundInfo(cb);
                    WebUtils.ShowMessage(this.Page, "�޸ĳɹ�");
                }
                else
                {
                    //����
                    cb.FRefund_state = 1;
                    cb.FSubmit_refund = 3;//ʧЧ
                    qs.AddRefundInfo(cb);
                    WebUtils.ShowMessage(this.Page, "�����ɹ�");
                }
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "���÷������" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
            }
		}

        protected void btnAmount_Click(object sender, System.EventArgs e)
        {
            string listid = cftOrderId.Text.Trim();

            bool flag = false;
            try
            {
                DataSet ds =  new TradeService().GetPayByListid(listid); //��ѯ΢��ת��ҵ��
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    string wxTradeId = ds.Tables[0].Rows[0]["Fcoding"].ToString();//���˻�����������
                    if (wxTradeId.Contains("mkt") || wxTradeId.Contains("wxp"))
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                flag = true;
                LogHelper.LogError("��ѯ΢��ת��ҵ���쳣  protected void btnAmount_Click(object sender, System.EventArgs e) ��" + ex.ToString());
                WebUtils.ShowMessage(this.Page, "��ѯ΢��ת��ҵ���쳣��" + ex.Message + ex.StackTrace);
                return;
            }

            if (flag)
            {
                this.btnSave.Visible = false;
                WebUtils.ShowMessage(this.Page, "��ǰ����Ϊ΢�Ŵ󵥣���ֹ¼�룡");
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