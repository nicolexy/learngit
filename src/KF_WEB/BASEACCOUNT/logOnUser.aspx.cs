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
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Web.Services.Protocols;
using System.Xml.Schema;
using System.Xml;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.Check_WebService;
using TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService;
using System.IO;
using CFT.CSOMS.BLL.BalanceModule;
using System.Configuration;
using CFT.CSOMS.BLL.TradeModule;
using System.Text.RegularExpressions;
using CFT.Apollo.Logging;
using CFT.CSOMS.COMMLIB;
using CFT.CSOMS.BLL.CFTAccountModule;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace TENCENT.OSS.C2C.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// logOnUser ��ժҪ˵����
	/// </summary>
	public partial class logOnUser : PageBase
	{
        protected BalaceService balaceService = new BalaceService();
		protected void Page_Load(object sender, System.EventArgs e)
		{             
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"CancelAccount")) Response.Redirect("../login.aspx?wh=1");

                if (!ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if (!Page.IsPostBack)
			{
				BindHistoryInfo("","",DateTime.Parse("1970-01-01 00:00:00"),DateTime.Now,0,10);
                TextBoxBeginDate.Value = DateTime.Now.ToString("yyyy-01-01");
				this.TextBoxEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
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

		public bool BindHistoryInfo(string qqid,string handid,DateTime bgDateTime,DateTime edDateTime,int pageIndex,int pageSize)
		{
			string Msg    = "";
			try
			{
				TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService.Finance_Manage fm = new TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService.Finance_Manage();
				DataSet ds =  fm.logOnUserHistory(bgDateTime,edDateTime,qqid,handid,pageIndex,pageSize,out Msg);  //Ĭ����ʾ����20��
			
				this.dgInfo.DataSource = ds;
				this.dgInfo.DataBind();

				return true;
			}
			catch(Exception e)
            {
                LogError("BaseAccount.logOnUser.public bool BindHistoryInfo(string qqid,string handid,DateTime bgDateTime,DateTime edDateTime,int pageIndex,int pageSize)", "��ȡ������Ϣ�쳣", e);
				WebUtils.ShowMessage(this.Page,Msg + e.Message);
				return false;
			}
		}


		/// <summary>
		/// �ύ�������롣����֤�ʻ��Ƿ����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btLogOn_Click(object sender, System.EventArgs e)  //�ݲ�����ע������
        {      
               // qs1.ClosedBalancePaid(this.TextBox1_QQID.Text.Trim());//�����ùر����֧�����ܣ�Ϊ�˲�ѯ����
			string qqid   = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceSqlStr(this.TextBox1_QQID.Text).Trim();
            string wxid = this.TextBox2_WX.Text.Trim();
			string reason = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceSqlStr(this.txtReason.Text).Trim();
            bool emailCheck = EmailCheckBox.Checked;
            string emailAddr = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceSqlStr(this.txtEmail.Text).Trim();

            int wxFlag = 0;//�Ƿ���΢���˺�
            if (string.IsNullOrEmpty(TextBox1_QQID.Text) && string.IsNullOrEmpty(TextBox2_WX.Text))
            {
                ValidateID.Text = "��������Q�˺Ż��·�΢�ź�";
                return;
            }
            if (!string.IsNullOrEmpty(TextBox1_QQID.Text) && TextBox1_QQID.Text.Contains("@wx.tenpay.com"))
            {
                ValidateID.Text = "΢��֧���ʺ�������΢�ź�ע��!";
                return;
            }
            if (!string.IsNullOrEmpty(TextBox1_QQID.Text) && !string.IsNullOrEmpty(TextBox2_WX.Text))
            {
                ValidateID.Text = "����ͬʱ������Q�˺ź�΢���˺�!!!";
                return;
            }
            string wxUIN = "";
            //string wxHBUIN = "";
            if (string.IsNullOrEmpty(TextBox1_QQID.Text))
            {
                if (TextBox2_WX.Text != txbConfirmQ.Text)
                {
                    ValidateID.Text = "����������˺Ų���ͬ������������!!!";
                    return;
                }
                wxFlag = 1;
                wxUIN = WeChatHelper.GetUINFromWeChatName(wxid);
                //wxHBUIN = WeChatHelper.GetHBUINFromWeChatName(wxid);
                qqid = wxUIN;
            }
            else if (TextBox1_QQID.Text != txbConfirmQ.Text)
            {
                ValidateID.Text = "����������˺Ų���ͬ������������!!!";
                return;
            }
            if (emailCheck && TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.CheckEmail(emailAddr))
            {
                WebUtils.ShowMessage(this.Page, "��������ȷ��ʽ���û������ַ");
                return;
            }

			string Msg = "";

            try
            {
                Finance_Manage fm = new Finance_Manage();
                if (!fm.checkUserReg(qqid, out Msg))
                {
                    Msg = "�ʺŷǷ�����δע�ᣡ";
                    WebUtils.ShowMessage(this.Page, Msg);
                    return;
                }

                if (255 < reason.Length)
                {
                    WebUtils.ShowMessage(this.Page, "��ע�������ó���255���ַ�");
                    return;
                }


                Check_Service cs = new Check_Service();
                TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Finance_Header fh = setConfig.setFH_CheckService(this);
                cs.Finance_HeaderValue = fh;

                Query_Service qs = new Query_Service();
                TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Finance_Header fh2 = setConfig.setFH(this);
                qs.Finance_HeaderValue = fh2;

                string accountName = string.Empty;
                long balance = 0;//���ǽ��
                if (ViewState["Cached" + qqid] == null)
                {
                    //��Q�û�ת���С��˿��С�δ��ɵĶ�����ֹע��������ע��
                    if (Regex.IsMatch(qqid, @"^[1-9]\d*$"))
                    {
                        #region ��Q�����ж�

                        #region ת��
                        try
                        {
                            string mobileQTransferErrorMsg = "";
                            DataSet dsMobileQTransfer = new TradeService().GetUnfinishedMobileQTransfer(qqid, out mobileQTransferErrorMsg);
                            if (!string.IsNullOrEmpty(mobileQTransferErrorMsg))
                            {
                                WebUtils.ShowMessage(this.Page, "��ѯ��Qת�˼�¼ʧ��" + mobileQTransferErrorMsg);
                                return;
                            }
                            if (dsMobileQTransfer != null && dsMobileQTransfer.Tables.Count > 0 && dsMobileQTransfer.Tables[0].Rows.Count > 0 && dsMobileQTransfer.Tables[0].Rows[0]["result"].ToString() != "192720108")
                            {
                                WebUtils.ShowMessage(this.Page, "��Q�û�ת���С��˿��С�δ��ɵĶ�����ֹע��������ע��");
                                return;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            LogHelper.LogError("��ѯ��Qת�˼�¼ʧ��" + ex.Message, "LogOnUser");
                            WebUtils.ShowMessage(this.Page, "��ѯ��Qת�˼�¼ʧ��" + ex.Message);
                            return;
                        }
                        #endregion
                        //DataSet dsHandQ = new TradeService().QueryPaymentParty("", "1,2,12,4,6,7", "3", qqid);
                        //if (dsHandQ != null && dsHandQ.Tables.Count > 0 && dsHandQ.Tables[0].Rows.Count > 0 && dsHandQ.Tables[0].Rows[0]["result"].ToString() != "97420006")
                        //{
                        //    WebUtils.ShowMessage(this.Page, "��Q�û�ת���С��˿��С�δ��ɵĶ�����ֹע��������ע��");
                        //    return;
                        //}
                        #region ���
                        try
                        {
                            DataSet dsMobileQHB = new TradeService().GetUnfinishedMobileQHB(qqid);
                            if (dsMobileQHB.Tables[0].Columns.Contains("row_num"))
                            {
                                if (dsMobileQHB != null && dsMobileQHB.Tables.Count > 0 && dsMobileQHB.Tables[0].Rows.Count > 0 && int.Parse(dsMobileQHB.Tables[0].Rows[0]["row_num"].ToString()) > 0)
                                {
                                    WebUtils.ShowMessage(this.Page, "���˻�����δ��ɵ���Q������ף���ֹע��������ע��");
                                    return;
                                }
                            }
                            else
                            {
                                WebUtils.ShowMessage(this.Page, "��ѯ�Ƿ���δ�����Q�������ʧ��!");
                                return;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            LogHelper.LogError("��ѯ��Q���δ��ɽ���ʧ��" + ex.Message, "LogOnUser");
                            WebUtils.ShowMessage(this.Page, "��ѯ��Q���δ��ɽ���ʧ��" + ex.Message);
                            return;
                        }
                        #endregion                    

                        #region ΢����
                        try
                        {
                            if (new TradeService().HasUnfinishedWeiLibDai(qqid))
                            {
                                WebUtils.ShowMessage(this.Page, "����δ��ɵ�΢����Ƿ��,��ֹע��������ע��");
                                return;
                            }
                        }
                        catch (Exception)
                        {
                            WebUtils.ShowMessage(this.Page, "΢������ѯ,����");
                            return;
                        }
                        #endregion

                        #endregion
                    }

                    //��΢��֧����ת�ˡ����δ��ɵĽ�ֹע��������ע��
                    if (wxFlag == 1)
                    {
                        #region ΢�������ж�
                        try
                        {
                            var WXUnfinishedTrade = (new TradeService()).QueryWXUnfinishedTrade(qqid);
                            if (!WXUnfinishedTrade)
                            {
                                LogHelper.LogInfo(wxid + "���˺���δ���΢��֧��ת�ˣ���ֹע��!");
                                WebUtils.ShowMessage(this.Page, "���˺���δ���΢��֧��ת�ˣ���ֹע��!");
                                return;
                            }

                            var HasUnfinishedHB = (new TradeService()).QueryWXUnfinishedHB(qqid);
                            if (!HasUnfinishedHB)
                            {
                                LogHelper.LogInfo(wxid + "���˺���δ���΢�ź������ֹע��!");
                                WebUtils.ShowMessage(this.Page, "���˺���δ���΢�ź������ֹע��!");
                                return;
                            }
                            string resultCodeString = "";
                            var HasUnfinishedRepayment = new TradeService().QueryWXFCancelAsRepayMent(qqid, out resultCodeString);
                            if (HasUnfinishedRepayment)
                            {
                                LogHelper.LogInfo(wxid + "���˺���δ���΢�Ż����ֹע��!");
                                WebUtils.ShowMessage(this.Page, "���˺���δ���΢�Ż����ֹע��!" + resultCodeString);
                                return;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            LogError("BaseAccount.logOnUser.protected void btLogOn_Click(object sender, System.EventArgs e) ", "����������ѯ΢��֧����;��Ŀ����:", ex);
                            WebUtils.ShowMessage(this.Page, "����������ѯ΢��֧����;��Ŀ����" + ex.Message);
                            return;
                        }
                        #endregion
                    }

                    //�Ƿ���δ��ɵĽ��׵�
                    if (qs.LogOnUsercheckOrder(qqid, "1"))
                    {
                        WebUtils.ShowMessage(this.Page, "��δ��ɵĽ��׵�");
                        return;
                    }

                    if (qs.LogOnUserCheckYDT(qqid, "1"))//�Ƿ�ͨһ��ͨ
                    {
                        WebUtils.ShowMessage(this.Page, "��ͨ��һ��ͨ");
                        return;
                    }

                    if (qs.LogOnUserCheckYDT(qqid, "2"))//�Ƿ�ͨ�˿��֧��
                    {
                        WebUtils.ShowMessage(this.Page, "��ͨ�˿��֧��");
                        return;
                    }

                    #region ����ж�
                    //�ⲿ���߼���ע�͵���
                    //���û�û�й������֧���Ĺ���ʱ��û�д�Ҳû�йر�ʱ��zw_prodatt_query_service���ش���
                    //���ϴ����֧��Ҳ����ͨ�������ǲ�������
                    //string ip = fh2.UserIP;
                    //bool isOpen = balaceService.BalancePaidOrNotQuery(fqqid, ip);//��ѯ���֧�����ܹر����
                    //if (balance <= 200000 && !isOpen)//�����֧�������ѱ������С��2000Ԫ�����Զ����ӿڴ����֧���Ĺ���
                    //{
                    //    balaceService.OpenBalancePaid(qqid,ip);
                    //}
                    //if (balance > 200000 && !isOpen)//��Ϊ������������֧�����Ͳ���ע����Ϊ��ϵͳ�����ٶ�
                    //{
                    //    WebUtils.ShowMessage(this.Page, "������2000Ԫ�������֧��Ϊ�ر�״̬������������");
                    //    return;
                    //}

                    //���������
                    // 1:�жϽ����ڷ�ֵ,����һ������
                    //2:���С��ʱ,����logonhistory��,���ýӿ�ע��,���������,�����䷢���ʼ�,������Ϣ��һ����Ա.


                    var ds = qs.GetUserAccount(qqid, 1, 1, 1);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        accountName = ds.Tables[0].Rows[0]["UserRealName2"].ToString();
                        string s_fz_amt = PublicRes.objectToString(ds.Tables[0], "Ffz_amt"); //���˶�����
                        string s_cron = PublicRes.objectToString(ds.Tables[0], "Fcon");

                        var freezeMoney = 0L;
                        if (s_fz_amt != "")
                        {
                            freezeMoney += long.Parse(s_fz_amt);
                        }

                        if (s_cron != "")
                        {
                            freezeMoney += long.Parse(s_cron);
                        }

                        if (freezeMoney > 0)
                        {
                            WebUtils.ShowMessage(this.Page, "�˻��ж����ʽ�֧��ע����");
                            return;
                        }

                    }


                    DataSet ds1 = qs.GetChildrenInfo(qqid, "1");//���ʻ����
                    if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        balance += long.Parse(ds1.Tables[0].Rows[0]["Fbalance"].ToString().Trim());
                    }
                    ds1 = qs.GetChildrenInfo(qqid, "80");//��Ϸ���ʻ�
                    if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        balance += long.Parse(ds1.Tables[0].Rows[0]["Fbalance"].ToString().Trim());
                    }
                    ds1 = qs.GetChildrenInfo(qqid, "82");//ֱͨ�����ʻ�
                    if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        balance += long.Parse(ds1.Tables[0].Rows[0]["Fbalance"].ToString().Trim());
                    }
                    #endregion

                    ViewState["Cached" + qqid] = balance;
                    var yuan = MoneyTransfer.FenToYuan(balance);
                    Response.Write("<script type='text/javascript'>window.onload=function(){ confirm(' ���˻���" + yuan + "Ԫ \\r\\n һ��ע���ɹ��޷��ָ�����ȷ���Ƿ����ע����') && document.getElementById('btLogOn').click();}</script>");
                    return;
                }

                balance = Convert.ToInt64(ViewState["Cached" + qqid]);
                ViewState["Cached" + qqid] = null;

                #region ע������

                string mainID = DateTime.Now.ToString("yyyyMMdd") + qqid;
                string checkType = "logonUser";
                if (balance < 10 * 10 * 200)//ϵͳ�Զ�ע��
                {
                    if (!qs.LogOnUserDeleteUser(qqid, reason, Label1.Text, "", out Msg))
                    {
                        throw new Exception(Msg);

                    }
                    //ϵͳ�Զ�ע���ɹ����û����ʼ�
                    if (emailCheck)
                    {
                        SendEmail(emailAddr, qqid, "ϵͳ�Զ�����");
                    }
                    UnRegisterNotify(qqid, accountName, fh.UserIP);
                    WebUtils.ShowMessage(this.Page, "ϵͳ�Զ������ɹ���");
                    return;
                }
                else
                {
                    string memo = "[ע��QQ����:" + qqid + "]ע��ԭ��:" + reason;
                    Param[] myParams = new Param[4];

                    myParams[0] = new Param();
                    myParams[0].ParamName = "fqqid";
                    myParams[0].ParamValue = qqid;

                    myParams[1] = new Param();
                    myParams[1].ParamName = "Memo";
                    myParams[1].ParamValue = memo;

                    myParams[2] = new Param();
                    myParams[2].ParamName = "returnUrl";
                    myParams[2].ParamValue = "/BaseAccount/InfoCenter.aspx?id=" + qqid;

                    myParams[3] = new Param();
                    myParams[3].ParamName = "fuser";
                    myParams[3].ParamValue = Session["uid"].ToString();

                    cs.StartCheck(mainID, checkType, memo, MoneyTransfer.FenToYuan(balance.ToString()), myParams);
                    WebUtils.ShowMessage(this.Page, "������������ɹ���");
                    return;
                }

                #endregion
            }
            catch (SoapException eSoap) //����soap���쳣
            {
                LogError("BaseAccount.logOnUser.protected void btLogOn_Click(object sender, System.EventArgs e) ", "������������ʧ��,SoapException:", eSoap);
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "������������ʧ�ܣ�" + errStr + "��StackTrace��" + eSoap.StackTrace);
                return;
            }
            catch (Exception err)
            {
                LogError("BaseAccount.logOnUser.protected void btLogOn_Click(object sender, System.EventArgs e) ", "�������������쳣:", err);
                //Msg += "�������������쳣��" + Common.CommLib.commRes.replaceHtmlStr(err.Message);
                Msg += "�������������쳣��" + TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceHtmlStr(err.Message);
                WebUtils.ShowMessage(this.Page, Msg + "��StackTrace��" + err.StackTrace);
                return;
            }           
		}

        /// <summary>
        /// ע���ɹ�֪ͨ���
        /// </summary>
        /// <param name="account"></param>
        /// <param name="accountName"></param>
        /// <param name="executorip"></param>
        private void UnRegisterNotify(string account, string accountName, string executorip)
        {
            IPAddress ipAddr;
            string ip = ConfigurationManager.AppSettings["UnRegisterNotify_IP"].ToString();
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["UnRegisterNotify_Port"].ToString());
            if (!IPAddress.TryParse(ip, out ipAddr))
            {
                LogHelper.LogError("ip��ַ����" + ipAddr.ToString());
            }
            else
            {
                UDPConnection conn = new UDPConnection(ipAddr, port);
                UdpClient udpC = conn.Connection();
                if (udpC == null)
                {
                    LogHelper.LogError("ע���Ƹ�֪ͨͨunregister_notify���޷����ӵ���������");
                }
                else
                {
                    string dataStr = "channel_id=111";
                    dataStr += "&purchaser_id=" + account;
                    dataStr += "&client_ip=" + executorip;
                    dataStr += "&time_stamp=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dataStr += "&imei=";
                    dataStr += "&guid=";
                    dataStr += "&mobile_info=";
                    dataStr += "&QQRiskInfo=";
                    dataStr += "&certno_3des=";
                    dataStr += "&name=" + accountName;
                    dataStr += "&ftype=3";

                    string reqStr = "protocol=unregister_notify&version=1.0&data=" + CommUtil.URLEncode(dataStr);
                    LogHelper.LogInfo("unregister_notify send:" + reqStr);
                    byte[] sendbytes = Encoding.Default.GetBytes(reqStr);
                    udpC.Send(sendbytes, sendbytes.Length);
                }
            }
        }

        private bool SendEmail(string email, string qqid,string subject)
        {
            try
            {
                string str_params = "p_name=" + qqid + "&p_parm1=" + DateTime.Now + "&p_parm2=" + "" + "&p_parm3=" + "" + "&p_parm4=" + "ϵͳ�Զ�����";
                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(email, "2034", str_params);
                return true;
            }
            catch (Exception err)
            {
                LogError("BaseAccount.logOnUser.private bool SendEmail(string email, string qqid,string subject) ", "���û����ʼ�����:", err);
                throw new Exception("���û����ʼ�����"+err.Message);
            }
        }

        protected void btQuery_Click(object sender, System.EventArgs e)
        {
            DateTime bgTime, edTime;
            if (!DateTime.TryParse(this.TextBoxBeginDate.Value, out bgTime))
            {
                //WebUtils.ShowMessage(this.Page, "��ʼ���ڸ�ʽ����ȷ!Ĭ��Ϊ1970��1��1��");
                bgTime = new DateTime(DateTime.Now.Year, 1, 1);
                this.TextBoxBeginDate.Value = bgTime.ToString("yyyy-MM-dd");
            }

            if (!DateTime.TryParse(this.TextBoxEndDate.Value, out edTime))
            {
                //WebUtils.ShowMessage(this.Page, "�������ڸ�ʽ����ȷ!Ĭ��Ϊ����");
                edTime = DateTime.Today;
                this.TextBoxEndDate.Value = edTime.ToString("yyyy-MM-dd");
                
            }

            string qqid = this.TxbQueryQQ.Text.Trim();
            string handid = this.txbHandID.Text.Trim();

            //����΢����ز�ѯ yinhuang 2014/2/20
            string wx_qq = this.tbWxQQ.Text.Trim();
            string wx_email = this.tbWxEmail.Text.Trim();
            string wx_phone = this.tbWxPhone.Text.Trim();
            string tbWxNo = this.tbWxNo.Text.Trim();
            try
            {
                string queryType = string.Empty;
                string id = string.Empty;
                if (!string.IsNullOrEmpty(wx_qq))
                {
                    queryType = "WeChatQQ";
                    id = wx_qq;
                }
                else if (!string.IsNullOrEmpty(wx_phone))
                {
                    queryType = "WeChatMobile";
                    id = wx_phone;
                }
                else if (!string.IsNullOrEmpty(wx_email))
                {
                    queryType = "WeChatEmail";
                    id = wx_email;
                }
                else if (!string.IsNullOrEmpty(tbWxNo))
                {
                    queryType = "WeChatId";
                    id = tbWxNo;
                }
                if (!string.IsNullOrEmpty(queryType))
                {
                    qqid = AccountService.GetQQID(queryType, id);
                    if (qqid == null || qqid.IndexOf("@") == 0) //����Null  ���� @wx.tenpay.com ʱ��ʾ���� 
                    {
                        WebUtils.ShowMessage(this.Page, queryType + " ת���Ƹ�ͨ�˺�ʧ��"); return;
                    }
                }
            }
            catch (Exception err)
            {
                LogError("BaseAccount.logOnUser.protected void btQuery_Click(object sender, System.EventArgs e) ", "��ѯ��Ϣ�쳣:", err);
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }
            BindHistoryInfo(qqid, handid, bgTime, edTime, 0, 1000); //Ĭ�ϲ�ѯ1000��
        }

		protected void lkHistoryQuery_Click(object sender, System.EventArgs e)
		{
			this.TABLE3.Visible = true;
		}

        protected void CheckBox1_CheckedChanged(object sender, System.EventArgs e)
        {
           
        }

        
	}
}

