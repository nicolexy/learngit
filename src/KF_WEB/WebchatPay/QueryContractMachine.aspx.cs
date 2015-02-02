using System;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using CFT.CSOMS.BLL.ActivityModule;
using System.Collections;
using CFT.CSOMS.BLL.FundModule;
using CFT.CSOMS.COMMLIB;
using System.Configuration;
using System.Text;
using System.Net;
using System.Xml;
using System.IO;

namespace TENCENT.OSS.CFT.KF.KF_Web.WebchatPay
{
	/// <summary>
    /// QueryContractMachine ��ժҪ˵����
    /// ��Լ�������ѯ
	/// </summary>
    public partial class QueryContractMachine : System.Web.UI.Page
	{
        protected void Page_Load(object sender, System.EventArgs e)
		{
           
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    
                    
                }
                 
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
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
            this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
		}
		#endregion


		private void ValidateDate()
		{
            string cft_no = txtCftNo.Text.Trim();
            string uin = tbx_payAccount.Text.Trim();

            if (cft_no == "" && uin == "")
            {
                throw new Exception("�����Ż��˺Ų���Ϊ�գ�");
            }
		}

        public void btnQuery_Click(object sender, System.EventArgs e)
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
                QueryData();
			}
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"���÷������" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"��ȡ����ʧ�ܣ�" + eSys.Message.ToString());
			}
		}

        private void BindData(string listid)
		{
            DataTable dt = null;
            dt = new FundService().QueryContractMachineDetail(listid);

			if(dt != null  && dt.Rows.Count > 0)
			{
                lb_CftOrderId.Text = dt.Rows[0]["listid"].ToString();
                lb_FundOrderId.Text = dt.Rows[0]["fund_code"].ToString();
                lb_LtOrderId.Text = dt.Rows[0]["sp_billno"].ToString();
                lb_FreezeTradeId.Text = dt.Rows[0]["freeze_list"].ToString();
                lb_TradeState.Text = dt.Rows[0]["stateStr"].ToString();
                lb_FreezeTime.Text = dt.Rows[0]["freeze_time"].ToString();
                lb_FreezeAmt.Text = dt.Rows[0]["totalfeeStr"].ToString();  //������
                lb_UnFreezeTime.Text = dt.Rows[0]["unfreeze_time"].ToString(); //�ⶳʱ��
                //lb_UnFreezeNo.Text = dt.Rows[0]["listid"].ToString();  //�ⶳ����
                //lb_Default.Text = dt.Rows[0]["listid"].ToString();  //ΥԼ�ۿ�
                lb_Spid.Text = dt.Rows[0]["spid"].ToString();//�̻���
                lb_SpNo.Text = dt.Rows[0]["sp_billno"].ToString();//�̻�����
                //lb_Present.Text = dt.Rows[0]["listid"].ToString();//���ͷݶ�
                //lb_lstate.Text = dt.Rows[0]["listid"].ToString();//����״̬
                lb_Channel.Text = dt.Rows[0]["chanidStr"].ToString();//��������

                lb_Uin.Text = dt.Rows[0]["uin"].ToString(); //�û��˺�
                lb_OrderTime.Text = dt.Rows[0]["create_time"].ToString(); //�µ�ʱ��
                //lb_ModifyTime.Text = dt.Rows[0]["create_time"].ToString(); //����޸�ʱ��
                //lb_BuyType.Text = dt.Rows[0]["listid"].ToString(); //���뷽ʽ
                lb_UinFreezeTime.Text = dt.Rows[0]["freeze_time"].ToString(); //�û�����ʱ��

                //�ֻ�/����Ϣ
                lb_hyjType.Text = dt.Rows[0]["mtype"].ToString(); //��Լ������
                //lb_hyjColor.Text = dt.Rows[0]["listid"].ToString(); //��Լ����ɫ
                //lb_hyjMemory.Text = dt.Rows[0]["listid"].ToString(); //��Լ���ڴ��С
                

                //lb_FirstMonth.Text = dt.Rows[0]["listid"].ToString(); //�����ʷ�
                lb_Phone.Text = dt.Rows[0]["bind_mobile"].ToString(); //ѡ�����
                lb_PlanType.Text = dt.Rows[0]["plan_code"].ToString(); //ѡ���ײ�
                lb_PlanCont.Text = dt.Rows[0]["desc"].ToString(); //�ײ�����
                lb_creType.Text = dt.Rows[0]["bind_cre_type"].ToString(); //����֤��
                lb_creId.Text = dt.Rows[0]["bind_cre_id"].ToString(); //����֤��id
                lb_BindPhone.Text = dt.Rows[0]["cnee_mobile"].ToString(); //��ϵ�绰

                lb_addr.Text = dt.Rows[0]["cnee_address"].ToString(); //�ռ���ַ
                lb_Express.Text = dt.Rows[0]["express"].ToString(); //�������
                lb_expTicket.Text = dt.Rows[0]["exp_ticket"].ToString(); //��ݵ���

                DataTable dt2 = new FundService().QueryPhoneDetail(dt.Rows[0]["spid"].ToString(), dt.Rows[0]["bind_mobile"].ToString());
                if (dt2 != null && dt2.Rows.Count > 0) 
                {
                    lb_Area.Text = dt2.Rows[0]["Farea"].ToString(); //�ֻ���������
                    lb_PhoneState.Text = dt2.Rows[0]["FstateStr"].ToString(); //�ֻ��ŵ�ǰ״̬
                }
			}
			else
			{
                lb_CftOrderId.Text = "";
                lb_FundOrderId.Text = "";
                lb_LtOrderId.Text = "";
                lb_FreezeTradeId.Text = "";
                lb_TradeState.Text = "";
                lb_FreezeTime.Text = "";
                lb_FreezeAmt.Text = "";
                lb_UnFreezeTime.Text = ""; //�ⶳʱ��
                lb_UnFreezeNo.Text = "";  //�ⶳ����
                lb_Default.Text = "";  //ΥԼ�ۿ�
                lb_Spid.Text = "";//�̻���
                lb_SpNo.Text = "";//�̻�����
                lb_Present.Text = "";//���ͷݶ�
                lb_lstate.Text = "";//����״̬
                lb_Channel.Text = "";//��������

                lb_Uin.Text = ""; //�û��˺�
                lb_OrderTime.Text = ""; //�µ�ʱ��
                lb_ModifyTime.Text = ""; //����޸�ʱ��
                lb_BuyType.Text = ""; //���뷽ʽ
                lb_UinFreezeTime.Text = ""; //�û�����ʱ��

                //�ֻ�/����Ϣ
                lb_hyjType.Text = ""; //��Լ������
                lb_hyjColor.Text = ""; //��Լ����ɫ
                lb_hyjMemory.Text = ""; //��Լ���ڴ��С
                lb_Area.Text = ""; //�ֻ���������
                lb_PhoneState.Text = ""; //�ֻ��ŵ�ǰ״̬

                lb_FirstMonth.Text = ""; //�����ʷ�
                lb_Phone.Text = ""; //ѡ�����
                lb_PlanType.Text = ""; //ѡ���ײ�
                lb_PlanCont.Text = ""; //�ײ�����
                lb_creType.Text = ""; //����֤��
                lb_creId.Text = ""; //����֤��id
                lb_BindPhone.Text = ""; //��ϵ�绰

                lb_addr.Text = ""; //�ռ���ַ
                lb_Express.Text = ""; //�������
                lb_expTicket.Text = ""; //��ݵ���
			}
		}

        private void QueryData() 
        {
            string listid = txtCftNo.Text.Trim();
            string uin = tbx_payAccount.Text.Trim();
            if (uin != "")
            {
                uin = getQQID();
                ListTable.Visible = true;
                DataSet ds = new FundService().QueryListidFromUin(uin);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataGrid1.DataSource = ds.Tables[0].DefaultView;
                    DataGrid1.DataBind();
                }
                else 
                {
                    DataGrid1.DataSource = null;
                }
            }
            else if (listid != "") 
            {
                ListTable.Visible = false;
                BindData(listid);
            }
        }

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string fid = e.Item.Cells[0].Text.Trim(); //ID

            switch (e.CommandName)
            {
                case "DETAIL": //����
                    BindData(fid);
                    break;
            }
        }

        private string getQQID()
        {
            if (string.IsNullOrEmpty(this.tbx_payAccount.Text))
            {
                return "";
            }
            var id = this.tbx_payAccount.Text.Trim();
            if (this.WeChatCft.Checked)
            {
                return id;
            }
            else if (this.WeChatUid.Checked)
            {
                var qs = new Query_Service.Query_Service();
                return qs.Uid2QQ(id);
            }
            else if (this.WeChatQQ.Checked || this.WeChatMobile.Checked || this.WeChatEmail.Checked)
            {
                string queryType = string.Empty;
                if (this.WeChatQQ.Checked)
                {
                    queryType = "QQ";
                }
                else if (this.WeChatMobile.Checked)
                {
                    queryType = "Mobile";
                }
                else if (this.WeChatEmail.Checked)
                {
                    queryType = "Email";
                }

                string openID = string.Empty, errorMessage = string.Empty;
                int errorCode = 0;
                var IPList = ConfigurationManager.AppSettings["WeChat"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < IPList.Length; j++)
                {
                    if (getOpenIDFromWeChat(queryType, id, out openID, out errorCode, out errorMessage, IPList[j]))
                    {
                        break;
                    }
                }
                if (errorCode == 0)
                {
                    return openID + "@wx.tenpay.com";
                }
                else if (errorCode == 1)
                {
                    throw new Exception("û�д��û�");
                }
                else
                {
                    throw new Exception(errorCode + errorMessage);
                }
            }
            else if (this.WeChatId.Checked)
            {
                return WeChatHelper.GetUINFromWeChatName(id);
            }

            return id;
        }

        //ͨ��΢�Ű󶨵�QQ���ֻ���������Ϣ��ѯ��openID����Ӧ�ĲƸ�ͨ�˺ű���openID@wx.tenpay.com
        private bool getOpenIDFromWeChat(string queryType, string ID, out string openID, out int errorCode, out string errorMessage, string IP)
        {
            openID = errorMessage = string.Empty;
            errorCode = 0;
            try
            {
                string parameterString = "<Request>{0}<AppId>wx482cac0d58846383</AppId></Request>";
                string IDstring = string.Empty;
                string API;
                if (queryType == "QQ")
                {
                    IDstring = string.Format("<QQ>{0}</QQ>", ID);
                    API = "ConvertQQToOuterAcctId";
                }
                else if (queryType == "Mobile")
                {
                    IDstring = string.Format("<Mobile>{0}</Mobile>", ID);
                    API = "ConvertMobileToOuterAcctId";
                }
                else if (queryType == "Email")
                {
                    IDstring = string.Format("<Email>{0}</Email>", ID);
                    API = "ConvertEmailToOuterAcctId";
                }
                else
                {
                    errorCode = -1;
                    errorMessage = "��ѯ���Ͳ���ȷ";
                    return false;
                }
                parameterString = string.Format(parameterString, IDstring);
                var data = Encoding.Default.GetBytes(parameterString);
                var request = (HttpWebRequest)WebRequest.Create(string.Format("http://{0}:12137/cgi-bin/{1}?f=xml&appname=wx_tenpay", IP, API));
                request.Method = "POST";
                request.ContentType = "text/xml;charset=UTF-8";
                var parameter = request.GetRequestStream();
                parameter.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var myResponseStream = response.GetResponseStream();
                var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                var resultXml = new XmlDocument();
                resultXml.LoadXml(myStreamReader.ReadToEnd());
                myStreamReader.Close();
                myResponseStream.Close();
                var responseNode = resultXml.SelectSingleNode("Response");
                errorCode = Convert.ToInt32(responseNode.SelectSingleNode("error").SelectSingleNode("code").InnerText);
                errorMessage = responseNode.SelectSingleNode("error").SelectSingleNode("message").InnerText;
                openID = responseNode.SelectSingleNode("result").SelectSingleNode("OuterAcctId").InnerText;
                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }
        }

	}
}