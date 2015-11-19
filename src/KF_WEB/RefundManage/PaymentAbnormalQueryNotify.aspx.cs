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
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using CFT.CSOMS.BLL.RefundModule;
using System.Collections.Generic;

namespace TENCENT.OSS.CFT.KF.KF_Web.RefundManage
{
    public partial class PaymentAbnormalQueryNotify : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
          RefundService refundService=  new RefundService();
          
			
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				int operid = Int32.Parse(Session["OperID"].ToString());

				if(!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if(!IsPostBack)
			{
				TextBoxDate.Text = DateTime.Now.ToString("yyyy��MM��dd��");

                PublicRes.GetDropdownlist(RefundService.SubTypePay, ddlSubTypePay);
                PublicRes.GetDropdownlist(RefundService.typeht, ddltype);
                PublicRes.GetDropdownlist(RefundService.notifyStatusht, ddlNotityStatus);
                PublicRes.GetDropdownlist(RefundService.notifyResultht, ddlNotityResult);
                PublicRes.GetDropdownlist(RefundService.accht, ddlAccType);
                PublicRes.GetDropdownlist(RefundService.errht, ddlErrorType);

                //���������б�
                setConfig.GetAllBankListFromDic(ddlBankType);
                ddlBankType.Items.Insert(0, new ListItem("��������", ""));
				Table2.Visible = false;				
			}

            ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
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

		private void ValidateDate()
		{ 
			DateTime date;

			try
			{
				date = DateTime.Parse(TextBoxDate.Text);
			}
			catch
			{
				throw new Exception("������������");
			}
            if (string.IsNullOrEmpty(tbBatchID.Text.Trim()))
            {
                throw new Exception("���β���Ϊ�գ�");
            }
            ViewState["sTime"] = date.ToString("yyyy-MM-dd 00:00:00");
            ViewState["eTime"] = date.ToString("yyyy-MM-dd 23:59:59");
		}

        protected void ButtonQuery_Click(object sender, System.EventArgs e)
		{
			try
			{
				ValidateDate();
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

			try
			{
                ViewState["recordCount"] = -1;
				Table2.Visible = true;
				BindData(1);
                var count = (int)ViewState["recordCount"];
                pager.RecordCount = count;
                lb_conut.InnerText = count.ToString();
			}
			catch(Exception eSys)
            {
                string errStr = PublicRes.GetErrorMsg(eSys.Message.ToString());
                WebUtils.ShowMessage(this.Page, "��ȡ����ʧ�ܣ�" + errStr);
			}
		}

        //��ѯ��¼�б�
		private void BindData(int index)
		{
            string sTime = ViewState["sTime"].ToString();
            string eTime = ViewState["eTime"].ToString();

            string batchID = tbBatchID.Text.Trim();
            string packageID = tbPackageID.Text.Trim();
            string listid = tblistid.Text.Trim();
            string type = ddltype.SelectedValue;
            string subTypePay = ddlSubTypePay.SelectedValue;
            string notityStatus = ddlNotityStatus.SelectedValue;
            string notityResult = ddlNotityResult.SelectedValue;
            string bankType =ddlBankType.SelectedValue;
            string errorType =ddlErrorType.SelectedValue;
            string accType = ddlAccType.SelectedValue;

            ChooseRadio.SelectedValue = "1";//ÿ�β�ѯ��ѡ����ѡ

            this.pager.CurrentPageIndex = index;
			int max = pager.PageSize;
			int start = max * (index-1);

            var count = (int)ViewState["recordCount"];
            DataSet ds = new RefundService().QueryPaymenAbnormal(sTime, eTime, batchID, packageID,
                listid, type, subTypePay, notityStatus, notityResult, bankType, errorType, accType, start, max , ref count);

            ViewState["recordCount"] = count;

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("Fbank_type_str", typeof(String));
                classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0], "Fbank_type", "Fbank_type_str", "BANK_TYPE");
                
                SendBtnControl(false);

                DataGrid1.DataSource = ds.Tables[0].DefaultView;
                DataGrid1.DataBind();
            }
            else
            {
                SendBtnControl(true);
                DataGrid1.DataSource = null;
                DataGrid1.DataBind();
                showMsg("û���ҵ���¼");
            }
		}

        //����֪ͨ����
        protected void SendNotifies(string notifyType)
        {
            try
            {
                Session["AbnorListid"] = null;
                Session["sTime"] = ViewState["sTime"].ToString();
                Session["eTime"] = ViewState["eTime"].ToString();
                Session["batchID"] = tbBatchID.Text.Trim();
                Session["packageID"] = tbPackageID.Text.Trim();
                Session["listid"] = tblistid.Text.Trim();
                Session["type"] = ddltype.SelectedValue;
                Session["subTypePay"] = ddlSubTypePay.SelectedValue;
                Session["notityStatus"] = ddlNotityStatus.SelectedValue;
                Session["notityResult"] = ddlNotityResult.SelectedValue;
                Session["bankType"] = ddlBankType.SelectedValue;
                Session["errorType"] = ddlErrorType.SelectedValue;
                Session["accType"] = ddlAccType.SelectedValue;
                string ip = Request.UserHostAddress.ToString();
                if (ip == "::1")
                    ip = "127.0.0.1";
                Session["client_ip"] = ip;

                string by = "";//��ѯ��ʽ

                //ȫѡ���и�����������ѯ���м�¼����֪ͨ
                //�������ݵ���������֪ͨ
                if (this.ChooseRadio.SelectedValue == "3")//ȫѡ����
                {
                    by = "condition";
                }
                else
                {
                    ArrayList listid = new ArrayList();
                    try
                    {
                        listid = GetCheckData();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("��ȡ�ѹ�ѡ�����쳣��" + ex.Message);
                    }

                    if (listid == null || listid.Count == 0)
                    {
                        BindData(pager.CurrentPageIndex);
                        throw new Exception("����ѡ����Ҫ���������ݣ�");
                    }
                    by = "listid";
                    Session["AbnorListid"] = listid;

                }
                SendBtnControl(true);//���ذ�ť��������ύ

              //  Response.Write("<script language=javascript>window.showModalDialog('SendPaymentAbnorNotify.aspx?by=" + by + "&notifyType=" + notifyType + "','','dialogWidth:900px;DialogHeight=1000px;status:no');</script>");
                Response.Write("<script language=javascript>window.open('SendPaymentAbnorNotify.aspx?by=" + by + "&notifyType=" + notifyType + "');</script>");
            }                                              
            catch (Exception err)
            {
                string errStr = PublicRes.GetErrorMsg(err.Message.ToString());
                showMsg("����֪ͨ�쳣��" + errStr);
            }
        }

        #region ����֪ͨ��ť
        protected void SendWX_Click(object sender, System.EventArgs e)
        {
            SendNotifies("1");
        }

        protected void SendMES_Click(object sender, System.EventArgs e)
        {
            SendNotifies("4");
        }

        //protected void SendQQ_Click(object sender, System.EventArgs e)
        //{
        //    SendNotifies("2");
        //}
        //protected void SendEmail_Click(object sender, System.EventArgs e)
        //{
        //    SendNotifies("3");
        //}
        //protected void SendTips_Click(object sender, System.EventArgs e)
        //{
        //    SendNotifies("5");
        //}
        //protected void SendWallet_Click(object sender, System.EventArgs e)
        //{
        //    SendNotifies("6");
        //}
        #endregion

        protected void Choose_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.ChooseRadio.SelectedValue == "2")//ȫѡ��ҳ
            {
                ChooseData(true);
            }
            else
            {
                ChooseData(false);
            }
           
        }

        //ȫѡ��ҳ����
        protected void ChooseData(bool allPage)
        {

            int count = DataGrid1.Items.Count;
            if (count <= 0)
            {
                showMsg("û�п�ѡ��������");
                return;
            }

            for (int i = 0; i < count; i++)
            {
                System.Web.UI.Control obj = DataGrid1.Items[i].Cells[10].FindControl("CheckBox2");
                if (obj != null && obj.Visible)
                {
                    CheckBox cb = (CheckBox)obj;
                    if (allPage)
                    {
                        cb.Checked = true;
                    }
                    else
                    {
                        cb.Checked = false;
                    }
                }
            }
        }
        
        //��ȡ�ѹ�ѡ����
        private ArrayList GetCheckData()
        {
            ArrayList listid = new ArrayList();
            try
            {
                int count = DataGrid1.Items.Count;

                for (int i = 0; i < count; i++)
                {
                    System.Web.UI.Control obj = DataGrid1.Items[i].Cells[10].FindControl("CheckBox2");
                    if (obj != null && obj.Visible)
                    {
                        CheckBox cb = (CheckBox)obj;
                        if (cb.Checked)
                        {
                            listid.Add(DataGrid1.Items[i].Cells[4].Text.Trim());
                        }
                    }
                }
                return listid;
            }
            catch (Exception ex)
            {
                throw new Exception("��ѡ�쳣��"+ex.Message);
            }
        }

        private void SendBtnControl(bool hide)
        {
            string notityStatus = ddlNotityStatus.SelectedValue;
            this.btSendWX.Visible = true;
            this.btSendMES.Visible = true;
            //this.btSendQQ.Visible = true;
            //this.btSendEmail.Visible = true;
            //this.btSendTips.Visible = true;
            //this.btSendWallet.Visible = true;
            //�����͡������м�����״̬��������֪ͨ
            if (hide || string.IsNullOrEmpty(notityStatus) || notityStatus == "1" || notityStatus == "2")
            {
                this.btSendWX.Visible = false;
                this.btSendMES.Visible = false;
                //this.btSendQQ.Visible = false;
                //this.btSendEmail.Visible = false;
                //this.btSendTips.Visible = false;
                //this.btSendWallet.Visible = false;
            }
        }

        private void showMsg(string msg)
        {
            Response.Write("<script language=javascript>alert('" + msg + "')</script>");
        }
     
        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }
	}
}
