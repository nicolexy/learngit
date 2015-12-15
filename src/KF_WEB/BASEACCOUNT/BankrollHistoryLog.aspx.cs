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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using CFT.CSOMS.BLL.CFTAccountModule;
using CFT.CSOMS.BLL.TransferMeaning;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// BankrollHistoryLog ��ժҪ˵����
	/// </summary>
	public partial class BankrollHistoryLog : System.Web.UI.Page
	{
		protected string iFramePath;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{

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

		protected void Button1_Click(object sender, System.EventArgs e)
		{
			try
			{
                if (string.IsNullOrEmpty(this.TextBox1_InputQQ.Text))
                {
                    WebUtils.ShowMessage(this.Page, "�������˺�!");
                    return;
                }
				DateTime begindate;
				DateTime enddate;
				try
				{
					begindate = DateTime.Parse(TextBoxBeginDate.Text);
					enddate = DateTime.Parse(TextBoxEndDate.Text);
				}
				catch
				{
					throw new Exception("������������");
				}

				if(begindate.CompareTo(enddate) > 0)
				{
					throw new Exception("��ֹ����С����ʼ���ڣ����������룡");
				}

				if(begindate.AddMonths(1) < enddate)
				{
					throw new Exception("��ѯʱ�䲻�ܴ���һ���£����������룡");
				}

                Session["QQID"] = getQQID();
                if ((this.InternalID.Checked) && (Session["QQID"] == null))//echo,�����ѯ����Ϊ�ڲ�id���Ҷ�ӦQQΪ�գ���ʱ�п�����ע�����˻���
                {
                    BindDataCancel(1, 1);   //��ѯע���˻���Ϣ
                }
                else
                {
                    if (!(Session["QQID"] == null))
                    {
                        BindData(1, 1);           //������
                    }
                }

				iFramePath = "bankrollLog.aspx?type=QQID&BeginDate=" + begindate.ToString("yyyy-MM-dd 00:00:00") + "&EndDate=" + enddate.ToString("yyyy-MM-dd 23:59:59");
			}
			catch(Exception ex)
			{
				WebUtils.ShowMessage(this.Page,"��ѯ����"+ ex.Message);
			}
		}
        
        protected string getQQID()
        {
            Session["fuid"] = "";
            if (string.IsNullOrEmpty(this.TextBox1_InputQQ.Text))
            {
                throw new Exception("������Ҫ��ѯ���˺�");
            }
            var id = this.TextBox1_InputQQ.Text.Trim();
            if (this.CFT.Checked)
            {
                return id;
            }
            else if (this.InternalID.Checked)
            {
                Session["fuid"] = id;  //ע���ʺŲ�ѯ�ʽ���ˮ
                var qs = new Query_Service.Query_Service();
                return qs.Uid2QQ(id);
            }
            return id;
        }

        //ע���˺���Ϣ��ѯ
        private void BindDataCancel(int istr, int imax)
        {
            Query_Service.Query_Service myService = new Query_Service.Query_Service();
            myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
            DataSet ds = new DataSet();
            ds = new AccountService().GetUserAccountCancel(this.TextBox1_InputQQ.Text.Trim(), 1, istr, imax);

            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                throw new Exception("���ݿ��޴˼�¼");
            }
            else
            {
                Session["QQID"] = ds.Tables[0].Rows[0]["Fqqid"].ToString();
                string fuid = ds.Tables[0].Rows[0]["fuid"].ToString().Trim();

                this.Label1_Acc.Text = ds.Tables[0].Rows[0]["Fqqid"].ToString();
                #region this.labQQstate
                if (Label1_Acc.Text != "")
                {
                    string testuid = new AccountService().QQ2Uid(Label1_Acc.Text);
                    if (testuid != null)
                    {
                        labQQstate.Text = "ע��δ����";
                        if (testuid.Trim() == fuid)
                        {
                            //���ж��Ƿ����Ѽ��� furion 20070226
                            string trueuid = new AccountService().QQ2UidX(Label1_Acc.Text);
                            if (trueuid != null)
                                labQQstate.Text = "�ѹ���";
                            else
                                labQQstate.Text = "�ѹ���δ����";
                        }
                    }
                    else
                        labQQstate.Text = "δע��";
                }
                else
                    labQQstate.Text = "";
                #endregion
                #region this.Label14_Ftruename
                // 2012/5/2 ��Ϊ��ҪQ_USER_INFO��ȡ׼ȷ���û���ʵ�������Ķ�
                try
                {
                    this.Label14_Ftruename.Text = ds.Tables[0].Rows[0]["UserRealName2"].ToString();
                }
                catch
                {
                    this.Label14_Ftruename.Text = ds.Tables[0].Rows[0]["Ftruename"].ToString();
                }
                #endregion

                //furion 20061116 email��¼�޸�
                this.labEmail.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Femail"]);
                #region this.labEmailState 
                if (labEmail.Text != "")
                {
                    string testuid = new AccountService().QQ2Uid(labEmail.Text);
                    if (testuid != null)
                    {
                        labEmailState.Text = "ע��δ����";
                        if (testuid.Trim() == fuid)
                        {
                            //���ж��Ƿ����Ѽ��� furion 20070226
                            string trueuid = new AccountService().QQ2UidX(labEmail.Text);
                            if (trueuid != null)
                                labEmailState.Text = "�ѹ���";
                            else
                                labEmailState.Text = "�ѹ���δ����";
                        }
                    }
                    else
                        labEmailState.Text = "δע��";
                }
                else
                    labEmailState.Text = "";
                #endregion
                this.lbInnerID.Text = ds.Tables[0].Rows[0]["Fuid"].ToString();

                this.labMobile.Text = PublicRes.GetString(ds.Tables[0].Rows[0]["Fmobile"]);
                #region this.labMobileState
                if (labMobile.Text != "")
                {
                    string testuid = new AccountService().QQ2Uid(labMobile.Text);
                    if (testuid != null)
                    {
                        labMobileState.Text = "ע��δ����";
                        if (testuid.Trim() == fuid)
                        {
                            //���ж��Ƿ����Ѽ��� furion 20070226
                            string trueuid = new AccountService().QQ2UidX(labMobile.Text);
                            if (trueuid != null)
                                labMobileState.Text = "�ѹ���";
                            else
                                labMobileState.Text = "�ѹ���δ����";
                        }
                    }
                    else
                        labMobileState.Text = "δע��";
                }
                else
                    labMobileState.Text = "";
                #endregion
                this.lbLeftPay.Text = Transfer.convertBPAY(ds.Tables[0].Rows[0]["Fbpay_state"].ToString().Trim());

                this.Label12_Fstate.Text = Transfer.accountState(ds.Tables[0].Rows[0]["Fstate"].ToString());
                this.Label13_Fuser_type.Text = Transfer.convertFuser_type(ds.Tables[0].Rows[0]["Fuser_type"].ToString());

                this.Label15_Useable.Text = classLibrary.setConfig.FenToYuan((long.Parse(ds.Tables[0].Rows[0]["Fbalance"].ToString()) - long.Parse(ds.Tables[0].Rows[0]["Fcon"].ToString())).ToString());  //�ʻ�����ȥ�������= �������
                #region this.Label4_Freeze
                string s_fz_amt = PublicRes.objectToString(ds.Tables[0], "Ffz_amt"); //���˶�����
                string s_cron = PublicRes.objectToString(ds.Tables[0], "Fcon");
                long l_fzamt = 0, l_cron = 0;
                if (s_fz_amt != "")
                {
                    l_fzamt = long.Parse(s_fz_amt);
                }
                if (s_cron != "")
                {
                    l_cron = long.Parse(s_cron);
                }
                this.Label4_Freeze.Text = classLibrary.setConfig.FenToYuan(l_fzamt + l_cron);//������=���˶�����+������
                #endregion

                this.Label5_YestodayLeft.Text = ds.Tables[0].Rows[0]["Fyday_balance"].ToString();		   //"10";
                this.Label3_LeftAcc.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fbalance"].ToString());//tu.u_Balance;				   //"3000";

                this.Label2_Type.Text = Transfer.convertMoney_type(ds.Tables[0].Rows[0]["Fcurtype"].ToString());//tu.u_CurType;				   //"����ȯ";
                this.lblLoginTime.Text = ds.Tables[0].Rows[0]["Fcreate_time"].ToString();

                this.Label16_Fapay.Text = ds.Tables[0].Rows[0]["Fapay"].ToString();
                this.Label7_SingleMax.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fquota"].ToString());		   //"2000";

                this.Label8_PerDayLmt.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fquota_pay"].ToString());			//"5000";
                this.lbFetchMoney.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Ffetch"].ToString().Trim());

                this.lbSave.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fsave"].ToString().Trim());
                this.Label9_LastSaveDate.Text = ds.Tables[0].Rows[0]["Fsave_time"].ToString();				//"2005-03-01";

                this.Label10_Drawing.Text = ds.Tables[0].Rows[0]["Ffetch_time"].ToString();              //"2005-04-15";
                this.Label17_Flogin_ip.Text = ds.Tables[0].Rows[0]["Flogin_ip"].ToString();

                this.Label6_LastModify.Text = ds.Tables[0].Rows[0]["Fmodify_time"].ToString();		   //"2005-05-01";
                #region this.Label18_Attid
                //2006-10-18 edwinyang ���Ӳ�Ʒ����
                int nAttid = int.Parse(ds.Tables[0].Rows[0]["Att_id"].ToString());
                this.Label18_Attid.Text = CheckBasicInfo(nAttid);
                #endregion

                this.Label11_Remark.Text = ds.Tables[0].Rows[0]["Fmemo"].ToString();					//"����һ������ʲô��û�����£�";                
            }

            if (Session["QQID"] == null && Session["QQID"] == "")
            {
                return;
            }
        }

		private void BindData(int istr,int imax)
		{
			//Query_Service.Query_Service myService = new Query_Service.Query_Service();
			//myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

			DataSet ds = new AccountService().GetUserAccount(Session["QQID"].ToString(),1,istr,imax);	

			if(ds == null || ds.Tables.Count<1 || ds.Tables[0].Rows.Count<1) 
			{
				throw new Exception("���ݿ��޴˼�¼");					
			}	

			this.Label1_Acc.Text			= PublicRes.objectToString(ds.Tables[0], "Fqqid");
            this.Label2_Type.Text           = Transfer.convertMoney_type(PublicRes.objectToString(ds.Tables[0], "Fcurtype"));//tu.u_CurType;				   //"����ȯ";
			this.Label3_LeftAcc.Text		= classLibrary.setConfig.FenToYuan(PublicRes.objectToString(ds.Tables[0],"Fbalance"));//tu.u_Balance;				   //"3000";
			this.Label4_Freeze.Text			= classLibrary.setConfig.FenToYuan(PublicRes.objectToString(ds.Tables[0],"Fcon"));                  //"1000";
            this.Label5_YestodayLeft.Text = PublicRes.objectToString(ds.Tables[0], "Fyday_balance"); 		   //"10";
			this.lblLoginTime.Text          = PublicRes.objectToString(ds.Tables[0],"Fcreate_time");
			this.Label6_LastModify.Text		= PublicRes.objectToString(ds.Tables[0],"Fmodify_time");		   //"2005-05-01";
			this.Label7_SingleMax.Text		= classLibrary.setConfig.FenToYuan(PublicRes.objectToString(ds.Tables[0],"Fquota"));		   //"2000";
			this.Label8_PerDayLmt.Text		= classLibrary.setConfig.FenToYuan(PublicRes.objectToString(ds.Tables[0],"Fquota_pay"));			//"5000";
			this.Label9_LastSaveDate.Text   = PublicRes.objectToString(ds.Tables[0],"Fsave_time");				//"2005-03-01";
			this.Label10_Drawing.Text		= PublicRes.objectToString(ds.Tables[0],"Ffetch_time");              //"2005-04-15";
			this.Label11_Remark.Text		= PublicRes.objectToString(ds.Tables[0],"Fmemo");					//"����һ������ʲô��û�����£�";
            this.Label12_Fstate.Text        = Transfer.accountState(PublicRes.objectToString(ds.Tables[0],"Fstate"));
            this.Label13_Fuser_type.Text    = Transfer.convertFuser_type(PublicRes.objectToString(ds.Tables[0],"Fuser_type"));
			
			try
			{
				// �Ķ�trueName��ֵΪuserInfo���trueName
				this.Label14_Ftruename.Text     = PublicRes.objectToString(ds.Tables[0],"UserRealName2");
			}
			catch
			{
				this.Label14_Ftruename.Text     = PublicRes.objectToString(ds.Tables[0],"Ftruename");
			}
			
			this.Label15_Useable.Text       = classLibrary.setConfig.FenToYuan((long.Parse(PublicRes.objectToString(ds.Tables[0],"Fbalance"))-long.Parse(PublicRes.objectToString(ds.Tables[0],"Fcon"))).ToString());  //�ʻ�����ȥ�������= �������
			this.Label16_Fapay.Text         = PublicRes.objectToString(ds.Tables[0],"Fapay");
			this.Label17_Flogin_ip.Text     = PublicRes.objectToString(ds.Tables[0],"Flogin_ip");

			//furion 20061116 email��¼�޸�
			this.labEmail.Text = PublicRes.GetString(PublicRes.objectToString(ds.Tables[0],"Femail"));
            this.labMobile.Text = PublicRes.GetString(PublicRes.objectToString(ds.Tables[0], "Fmobile"));

			//2006-10-18 edwinyang ���Ӳ�Ʒ����
            int nAttid = int.Parse(PublicRes.objectToString(ds.Tables[0], "Att_id"));
			this.Label18_Attid.Text			 = CheckBasicInfo(nAttid);
			this.lbInnerID.Text             = PublicRes.objectToString(ds.Tables[0],"fuid").Trim();
			this.lbFetchMoney.Text          = classLibrary.setConfig.FenToYuan(PublicRes.objectToString(ds.Tables[0],"Ffetch").Trim());
            this.lbLeftPay.Text             = Transfer.convertBPAY(PublicRes.objectToString(ds.Tables[0],"Fbpay_state").Trim());
			this.lbSave.Text                = classLibrary.setConfig.FenToYuan(PublicRes.objectToString(ds.Tables[0],"Fsave").Trim());
			string fuid = PublicRes.objectToString(ds.Tables[0],"fuid").Trim();

			if(Label1_Acc.Text != "")
			{
                string testuid = new AccountService().QQ2Uid(Label1_Acc.Text);

				if(testuid != null)
				{
					labQQstate.Text = "ע��δ����";

					if(testuid.Trim() == fuid)
					{
						//���ж��Ƿ����Ѽ��� furion 20070226
                        string trueuid = new AccountService().QQ2UidX(Label1_Acc.Text);

						if(trueuid != null)
							labQQstate.Text = "�ѹ���";
						else
							labQQstate.Text = "�ѹ���δ����";
					}
				}
				else
					labQQstate.Text = "δע��";
			}
			else
				labQQstate.Text = "";

			if(labEmail.Text != "")
			{
                string testuid = new AccountService().QQ2Uid(labEmail.Text);

				if(testuid != null)
				{
					labEmailState.Text = "ע��δ����";

					if(testuid.Trim() == fuid)
					{
						//���ж��Ƿ����Ѽ��� furion 20070226
                        string trueuid = new AccountService().QQ2UidX(labEmail.Text);

						if(trueuid != null)
							labEmailState.Text = "�ѹ���";
						else
							labEmailState.Text = "�ѹ���δ����";
					}
				}
				else
					labEmailState.Text = "δע��";
			}
			else
				labEmailState.Text = "";

			if(labMobile.Text != "")
			{
                string testuid = new AccountService().QQ2Uid(labMobile.Text);

				if(testuid != null)
				{
					labMobileState.Text = "ע��δ����";

					if(testuid.Trim() == fuid)
					{
						//���ж��Ƿ����Ѽ��� furion 20070226
                        string trueuid = new AccountService().QQ2UidX(labMobile.Text);

						if(trueuid != null)
							labMobileState.Text = "�ѹ���";
						else
							labMobileState.Text = "�ѹ���δ����";
					}
				}
				else
					labMobileState.Text = "δע��";
			}
			else
				labMobileState.Text = "";

		}

		private string CheckBasicInfo(int nAttid)
		{
			DataSet ds = PermitPara.QueryDicAccName();

			if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
			{
				return "";
			}

			DataTable dt = ds.Tables[0];

			foreach(DataRow dr in dt.Rows)
			{
				if (nAttid == int.Parse(dr["Value"].ToString().Trim()))
				{
					return dr["Text"].ToString().Trim();
				}
			}
			return "";
		}

        protected void lbtnQueryOld_Click(object sender, EventArgs e)
        {
            DateTime begindate = DateTime.Parse(TextBoxBeginDate.Text);
            DateTime enddate = DateTime.Parse(TextBoxEndDate.Text);
            lbtnQueryOld.Text = "�°�";
            iFramePath = "bankrollLog.aspx?type=QQID&BeginDate=" + begindate.ToString("yyyy-MM-dd 00:00:00") + "&EndDate=" + enddate.ToString("yyyy-MM-dd 23:59:59");
        }

	}
}
