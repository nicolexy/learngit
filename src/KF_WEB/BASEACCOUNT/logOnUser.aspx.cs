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


namespace TENCENT.OSS.C2C.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// logOnUser ��ժҪ˵����
	/// </summary>
	public partial class logOnUser : System.Web.UI.Page
	{
        protected BalaceService balaceService = new BalaceService();
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()"); 
			ButtonEndDate.Attributes.Add("onclick", "openModeEnd()"); 

			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();
				//int operid = Int32.Parse(Session["OperID"].ToString());

				//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"CancelAccount")) Response.Redirect("../login.aspx?wh=1");

                if (!ClassLib.ValidateRight("CancelAccount", this)) Response.Redirect("../login.aspx?wh=1");
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}

			if (!Page.IsPostBack)
			{
				BindHistoryInfo("","",DateTime.Parse("1970-01-01 00:00:00"),DateTime.Now,0,10);
				this.TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
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
			string qqid   = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceSqlStr(this.TextBox1_QQID.Text);
			string reason = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceSqlStr(this.txtReason.Text);
            bool emailCheck = EmailCheckBox.Checked;
            string emailAddr = TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceSqlStr(this.txtEmail.Text);

            if (emailCheck && TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.CheckEmail(emailAddr))
            {
                WebUtils.ShowMessage(this.Page, "��������ȷ��ʽ���û������ַ");
                return;
            }

			string memo   = "[ע��QQ����:" + qqid + "]ע��ԭ��:" + reason;

			string Msg = "";

			try
			{
				Finance_Manage fm = new Finance_Manage();
				if (!fm.checkUserReg(qqid,out Msg))
				{
					Msg = "�ʺŷǷ�����δע�ᣡ";
					WebUtils.ShowMessage(this.Page,Msg);
					return;
				}

				Check_Service cs = new Check_Service();
				TENCENT.OSS.CFT.KF.KF_Web.Check_WebService.Finance_Header fh = setConfig.setFH_CheckService(this);
				cs.Finance_HeaderValue = fh;

				Param[] myParams = new Param[4];

				string fqqid = this.txbConfirmQ.Text.Trim();
				myParams[0] = new Param();
				myParams[0].ParamName = "fqqid";
				myParams[0].ParamValue = fqqid;

				myParams[1] = new Param();
				myParams[1].ParamName = "Memo";
				myParams[1].ParamValue = memo;

				myParams[2] = new Param();
				myParams[2].ParamName = "returnUrl";
				myParams[2].ParamValue = "/BaseAccount/InfoCenter.aspx?id=" + qqid;

				myParams[3] = new Param();
				myParams[3].ParamName = "fuser";
				myParams[3].ParamValue = Session["uid"].ToString();

				string mainID     = DateTime.Now.ToString("yyyyMMdd") + qqid;
				string checkType  = "logonUser";

                Query_Service qs = new Query_Service();
                TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Finance_Header fh2 = setConfig.setFH(this);
                qs.Finance_HeaderValue = fh2;

                if (255 < reason.Length)
                {
                    WebUtils.ShowMessage(this.Page, "��ע�������ó���255���ַ�");
                    return;
                }

                //�Ƿ���δ��ɵĽ��׵�
                if (qs.LogOnUsercheckOrder(fqqid, "1"))
                {
                    WebUtils.ShowMessage(this.Page, "��δ��ɵĽ��׵�");
                    return;
                }
               
                if (qs.LogOnUserCheckYDT(fqqid, "1"))//�Ƿ�ͨһ��ͨ
                {
                    WebUtils.ShowMessage(this.Page, "��ͨ��һ��ͨ");
                    return;
                }

                //���������
               // 1:�жϽ����ڷ�ֵ,����һ������
               //2:���С��ʱ,����logonhistory��,���ýӿ�ע��,���������,�����䷢���ʼ�,������Ϣ��һ����Ա.

				long balance=0;//���ǽ��
				
				DataSet ds1 = qs.GetChildrenInfo(fqqid,"1");//���ʻ����
				if(ds1!=null && ds1.Tables.Count>0 && ds1.Tables[0].Rows.Count>0)
				{
					balance+=long.Parse(ds1.Tables[0].Rows[0]["Fbalance"].ToString().Trim());
				}
				ds1 = qs.GetChildrenInfo(fqqid,"80");//��Ϸ���ʻ�
				if(ds1!=null && ds1.Tables.Count>0 && ds1.Tables[0].Rows.Count>0)
				{
					balance+=long.Parse(ds1.Tables[0].Rows[0]["Fbalance"].ToString().Trim());
				}
				ds1 = qs.GetChildrenInfo(fqqid,"82");//ֱͨ�����ʻ�
				if(ds1!=null && ds1.Tables.Count>0 && ds1.Tables[0].Rows.Count>0)
				{
					balance+=long.Parse(ds1.Tables[0].Rows[0]["Fbalance"].ToString().Trim());
				}

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

                if (balance < 5000)//ϵͳ�Զ�ע��
                {
                    if (!qs.LogOnUserDeleteUser(qqid, reason, Label1.Text, "", out Msg))
                    {
                        throw new Exception(Msg);

                    }
                    //ϵͳ�Զ�ע���ɹ����û����ʼ�
                    if (emailCheck)
                    {
                     SendEmail(emailAddr,qqid,"ϵͳ�Զ�����");
                    }
                    WebUtils.ShowMessage(this.Page, "ϵͳ�Զ������ɹ���");
                    return;
                }
                else
                {
                    cs.StartCheck(mainID, checkType, memo, MoneyTransfer.FenToYuan(balance.ToString()), myParams);
                    WebUtils.ShowMessage(this.Page, "������������ɹ���");
                    return;
                }
			}
			catch(SoapException eSoap) //����soap���쳣
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"������������ʧ�ܣ�" + errStr);
				return;
			}
			catch(Exception err)
			{
				//Msg += "�������������쳣��" + Common.CommLib.commRes.replaceHtmlStr(err.Message);
				Msg += "�������������쳣��" + TENCENT.OSS.CFT.KF.KF_Web.classLibrary.setConfig.replaceHtmlStr(err.Message);
				WebUtils.ShowMessage(this.Page,Msg);
				return;
			}

			this.btLogOn.Enabled = false;
		}

        private bool SendEmail(string email, string qqid,string subject)
        {
            try
            {
                /*
                TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend newMail = new TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend();
                
                string pattern = "";
                string filename = Server.MapPath(Request.ApplicationPath).Trim().ToLower() + "\\Email\\DelUserYes.htm";

                StreamReader sr = new StreamReader(filename, System.Text.Encoding.GetEncoding("GB2312"));
                try
                {
                    pattern = sr.ReadToEnd();
                }
                finally
                {
                    sr.Close();
                }

                pattern = String.Format(pattern, qqid,DateTime.Now, "", "" ,"");
                newMail.SendMail(email, "", subject, pattern, true, null);
                */
                string str_params = "p_name=" + qqid + "&p_parm1=" + DateTime.Now + "&p_parm2=" + "" + "&p_parm3=" + "" + "&p_parm4=" + "ϵͳ�Զ�����";
                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(email, "2034", str_params);
                return true;
            }
            catch (Exception err)
            {
                throw new Exception("���û����ʼ�����"+err.Message);
            }
        }

		protected void btQuery_Click(object sender, System.EventArgs e)
		{
			DateTime bgTime;
			DateTime edTime;

			try
			{
				bgTime = DateTime.Parse(this.TextBoxBeginDate.Text);	
			}
			catch
			{
				WebUtils.ShowMessage(this.Page,"��ʼ���ڸ�ʽ����ȷ!Ĭ��Ϊ1970��1��1��");
				this.TextBoxBeginDate.Text = "1970-01-01";
				bgTime = DateTime.Parse("1970-01-01 00:00:00");
			}
			
			try
			{
				edTime = DateTime.Parse(this.TextBoxEndDate.Text);	
			}
			catch
			{
				WebUtils.ShowMessage(this.Page,"�������ڸ�ʽ����ȷ!Ĭ��Ϊ����");
				this.TextBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
				edTime = DateTime.Now;
			}
			
			string qqid   = this.TxbQueryQQ.Text.Trim();
			string handid = this.txbHandID.Text.Trim();
            
            //����΢����ز�ѯ yinhuang 2014/2/20
            string wx_qq = this.tbWxQQ.Text.Trim();
            string wx_email = this.tbWxEmail.Text.Trim();
            string wx_phone = this.tbWxPhone.Text.Trim();

            try
            {
                string queryType = string.Empty;
                string id = string.Empty;
                if (!string.IsNullOrEmpty(wx_qq))
                {
                    queryType = "QQ";
                    id = wx_qq;
                }
                else if (!string.IsNullOrEmpty(wx_phone))
                {
                    queryType = "Mobile";
                    id = wx_phone;
                }
                else if (!string.IsNullOrEmpty(wx_email))
                {
                    queryType = "Email";
                    id = wx_email;
                }

                if (!string.IsNullOrEmpty(queryType)) 
                {
                    string openID = string.Empty, errorMessage = string.Empty;
                    int errorCode = 0;
                    var IPList = ConfigurationManager.AppSettings["WeChat"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int j = 0; j < IPList.Length; j++)
                    {
                        if (PublicRes.getOpenIDFromWeChat(queryType, id, out openID, out errorCode, out errorMessage, IPList[j]))
                        {
                            break;
                        }
                    }
                    if (errorCode == 0)
                    {
                        qqid = openID + "@wx.tenpay.com";
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
            }
            catch (Exception err) 
            {
                WebUtils.ShowMessage(this.Page, err.Message);
                return;
            }
            

			BindHistoryInfo(qqid,handid,bgTime,edTime,0,1000); //Ĭ�ϲ�ѯ1000��
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

