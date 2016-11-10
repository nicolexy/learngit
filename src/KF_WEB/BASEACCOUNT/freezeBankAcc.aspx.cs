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
using TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using CFT.CSOMS.BLL.FreezeModule;
using CFT.Apollo.Logging;
using CFT.CSOMS.BLL.CFTAccountModule;
using System.Collections.Generic;
using System.Linq;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// freezeBankAcc ��ժҪ˵����
	/// </summary>
	public partial class freezeBankAcc : PageBase
	{
        public List<FreezeChannel> FreezeChannels = new List<FreezeChannel>()
                    {
                      new FreezeChannel{Value="1",Text="��ض���",RightName="UnFreezeChannelFK"},
                      new FreezeChannel{Value="2",Text="���Ķ���",RightName="UnFreezeChannelPP"},
                      new FreezeChannel{Value="3",Text="�û�����",RightName="UnFreezeChannelYH"},
                      new FreezeChannel{Value="4",Text="�̻�����",RightName="UnFreezeChannelSH"},
                      new FreezeChannel{Value="5",Text="BG�ӿڶ���",RightName="UnFreezeChannelBG"},
                      new FreezeChannel{Value="6",Text="���ӿ��ɽ��׶���",RightName="UnFreezeChannelFK"},
                      new FreezeChannel{Value="10",Text="����������թƽ̨����",RightName=""},
                      new FreezeChannel{Value="11",Text="������ƭ����",RightName="UnFreezeChannelBDBP"},
                      new FreezeChannel{Value="12",Text="���ֶ���",RightName="UnFreezeChannelTX"},
                      new FreezeChannel{Value="13",Text="��Ķ���",RightName="UnFreezeChannelSD"}
                    };
        private string sign;
		protected System.Web.UI.WebControls.ImageButton ButtonBeginDate;
		private string uid;
	
		//christy:�ⶳ���ļ򻯰��ʺ�ʱ���û���������ϵ��ʽ�ǿհ��Ի��޷��ⶳ.���԰��û���������ϵ��ʽ��2����������ж�ȥ��
		protected void Page_Load(object sender, System.EventArgs e)
		{
            //furion 20050906 ���޸���ǰ���κι��ܣ�ֻ���������µĶ�����
            // �ڴ˴������û������Գ�ʼ��ҳ��
    
            this.cbx_showEndDate.CheckedChanged += new EventHandler(cbx_showEndDate_CheckedChanged);

			try
			{
				if(System.Configuration.ConfigurationManager.AppSettings["isTestingMode"].ToString().ToLower() == "false")
				{
					labUid.Text = Session["uid"].ToString();
					string szkey = Session["SzKey"].ToString();
					//int operid = Int32.Parse(Session["OperID"].ToString());
					//if (!AllUserRight.ValidRight(szkey, operid, PublicRes.GROUPID, "FreezeUser")) Response.Redirect("../login.aspx?wh=1");						
					if(!classLibrary.ClassLib.ValidateRight("FreezeUser",this)) Response.Redirect("../login.aspx?wh=1");
				}
			}
			catch  //���û�е�½����û��Ȩ�޾�����
			{
				Response.Redirect("../login.aspx?wh=1");
			} 

			if (!Page.IsPostBack)
			{
				ViewState["showEndDate"] = "0";

                if (System.Configuration.ConfigurationManager.AppSettings["isTestingMode"].ToString().ToLower() == "false")//����ʱfalse��Ϊtrue
				{
					sign   = Request.QueryString["id"].ToString();
					uid = Request.QueryString["uid"].ToString();
                    string iswechat = Request.QueryString["iswechat"].ToString();

					//furion 20050906 �������־ֲ��������������У�������ViewState��
					ViewState["sign"] = sign;
					ViewState["uid"] = uid;
					//furion end.

                    //yinhuang 2013/8/15 ���Ӷ�΢�ŵĴ���
                    ViewState["iswechat"] = iswechat;
                    ViewState["tuserName"] = "";
                    FreezeChannel();
                    BindInfo();
                }
			}
		}

        private void FreezeChannel()
        {
            ddlFreezeChannel.Items.Clear();
            foreach (FreezeChannel item in FreezeChannels)
            {
                ListItem listitem = new ListItem() { Value = item.Value, Text = item.Text };
                ddlFreezeChannel.Items.Add(listitem);
            }
        }

		private void BindInfo()
		{
			//����Ϣ
			sign = ViewState["sign"].ToString();
			uid = ViewState["uid"].ToString();
			
			if (sign.ToLower() == "true")  //����������ʻ������ж������
			{
				//�������
				Label1_state.Text     = "�����˻�";
				this.BT_F_Or_Not.Text = "�����˻�";

				//furion 20050906
				Label_listID.Text = uid;
				labReason.Text = "����ԭ��";            
				tbUserName.Text = "";
				tbUserName.Enabled = true;
				tbContact.Text = "";
				tbContact.Enabled = true;
                ddlFreezeChannel.Enabled = true;
				//furion end
			}
			else if (sign.ToLower() == "false")   //����Ƕ����˻������нⶳ����
			{
				//�ⶳ����
				Label1_state.Text     = "�ⶳ�ʻ�";
				this.BT_F_Or_Not.Text = "�ⶳ�ʻ�";

				//furion 20050906
				labReason.Text = "�ⶳԭ��"; 				
				tbUserName.Enabled = false;				
				tbContact.Enabled = false;
                ddlFreezeChannel.Enabled = false;
				Label_listID.Text = uid;

				//��ȡ��ԭ���ύ���û���������ϵ��ʽ���ʻ����롣
				Query_Service.Query_Service fm = new Query_Service.Query_Service();
				Query_Service.Finance_Header fh = classLibrary.setConfig.setFH(this);
				fm.Finance_HeaderValue = fh;
				
				try
				{
					//FFreezeType: 1Ϊ�����ʻ���2Ϊ��������
					Query_Service.FreezeInfo fi = fm.GetExistFreeze(uid,1);
                    if (fi != null)
                    { //���û�ж����¼�ģ�Ҳ�ܽⶳ
                        ViewState["fid"] = fi.fid;

                        tbUserName.Text = (string.IsNullOrEmpty(fi.username)) ? "<���û�����>" : fi.username;
                        tbContact.Text = (string.IsNullOrEmpty(fi.contact)) ? "<����ϵ��ʽ>" : fi.contact;
                        //ddlFreezeChannel.Text = (fi.FFreezeChannel == "" || fi.FFreezeChannel == null) ? "<�޽ⶳ����>" : fi.FFreezeChannel;
                        if (fi.FFreezeChannel == null || fi.FFreezeChannel == "" || fi.FFreezeChannel == "0")
                        {
                            ddlFreezeChannel.Items.Add(new ListItem("�޶�������", "0"));
                            ddlFreezeChannel.SelectedValue = "0";
                        }
                        else
                        {
                            ddlFreezeChannel.SelectedValue = fi.FFreezeChannel;
                        }
                        ViewState["tuserName"] = (string.IsNullOrEmpty(fi.username)) ? "" : fi.username;
                        ViewState["freezeChannel"] = ddlFreezeChannel.SelectedValue; //�ⶳ����
                    }
                    else 
                    {
                        ViewState["fid"] = ""; 
                        ViewState["tuserName"] = "";
                        ViewState["freezeChannel"] = "";
                    }
					
				}
				catch
				{
					ViewState["fid"] = "";   //������������쳣,˵����QQ��ע���
                    ViewState["tuserName"] = "";
                    ViewState["freezeChannel"] = "";
				}
				//furion end
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

		protected void BT_F_Or_Not_Click(object sender, System.EventArgs e)
		{
			
			if(Session["uid"] == null)
				Response.Redirect("../login.aspx?wh=1"); //���µ�½

			sign = ViewState["sign"].ToString();
			uid = ViewState["uid"].ToString();

			try
			{
				string strszkey = Session["SzKey"].ToString().Trim();
				int ioperid = Int32.Parse(Session["OperID"].ToString());
				int iserviceid = Common.AllUserRight.GetServiceID("FreezeUser") ;
				string struserdata = Session["uid"].ToString().Trim();
				string content = struserdata + "ִ����[����QQ�ʻ�]����,��������[" + uid
					+ "]ʱ��:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

				Common.AllUserRight.UpdateSession(strszkey,ioperid,PublicRes.GROUPID,iserviceid,struserdata,content);

				Finance_Manage fm = new Finance_Manage();
				Finance_ManageService.Finance_Header fh = classLibrary.setConfig.setFH_Finance(this);
				fm.Finance_HeaderValue = fh;
                string op_type = "";//���ͨ�˻�״̬��������

				//���ö�����߽ⶳ�ʻ���service
				if (sign.ToLower() == "true")  //����������ʻ������ж������
                {
                    #region
                    op_type = "1";
					bool exeSign = false;
					if (Request.QueryString["type"] == null)
					{
						exeSign = fm.freezeAccount(uid,1);	
					}
					else 
					{
                        string uname = "";
                        if (ViewState["tuserName"] != null && ViewState["tuserName"].ToString() != "")
                        {
                            uname = ViewState["tuserName"].ToString();
                        }
                        if (ViewState["iswechat"].ToString() == "true")
                        {
                            //΢�Ŵ������� ,yinhuang ʹ�ýӿ�ʵ��
                            //exeSign = fm.FreezePerAccountWechat(uid, 1);
                            exeSign = fm.FreezePerAccountWechat_New(uid, uname, ddlFreezeChannel.SelectedValue);
                        }
                        else {
                            //���� 1 ui_freeze_user_service
                            exeSign = fm.freezePerAccount(uid, 1, uname, ddlFreezeChannel.SelectedValue);
                        }
					}

					if (false == exeSign)
					{
						WebUtils.ShowMessage(this.Page,"�˻�����ʧ�ܣ����Ϊ�̻���QQ�ţ�û�ж���Ȩ�ޡ�����ϵϵͳ����Ա��");
						return;
					}
					else
					{
                        WebUtils.ShowMessage(this.Page,"�˻�����ɹ���");	
					}

					//furion 20050906 Ҫ�ȼ��빤�������ɹ�����������Ĺ�����
					Query_Service.Query_Service qs = new Query_Service.Query_Service();
					Query_Service.Finance_Header fhq = classLibrary.setConfig.setFH(this);
					qs.Finance_HeaderValue = fhq;

					try
					{
						Query_Service.FreezeInfo fi = new FreezeInfo();
						fi.FFreezeID = uid;
						fi.FFreezeType = 1;
						fi.username = tbUserName.Text.Trim();
						fi.contact = tbContact.Text.Trim();
						fi.FFreezeReason = tbMemo.Text.Trim();
                        fi.FFreezeChannel = ddlFreezeChannel.SelectedValue;

						if(this.cbx_showEndDate.Checked)
						{
							DateTime endDate;

							try
							{
								endDate = DateTime.Parse(this.tbx_FreezeEndDate.Text);
								fi.strFreezeEndDate = endDate.ToString("yyyyMMdd");
							}
							catch
							{
								WebUtils.ShowMessage(this.Page,"��������ȷ�����ڸ�ʽ");
								return;
							}
						}
						else
						{
							fi.strFreezeEndDate = "";
						}

						fi.username = classLibrary.setConfig.replaceMStr(fi.username);
						fi.contact = classLibrary.setConfig.replaceMStr(fi.contact);
						fi.FFreezeReason = classLibrary.setConfig.replaceMStr(fi.FFreezeReason);

						string log = classLibrary.SensitivePowerOperaLib.MakeLog("edit",struserdata,"[����QQ�ʻ�]",
							fhq.UserName,fhq.UserIP,fhq.OperID.ToString(),fhq.SzKey,fi.FFreezeID,fi.FFreezeType.ToString(),
							fi.username,fi.contact,fi.FFreezeReason);

						if(!classLibrary.SensitivePowerOperaLib.WriteOperationRecord("FreezeUser",log,this))
						{
							
						}
                        
						qs.CreateNewFreeze(fi);
					}
					catch(Exception ex)
                    {
                        LogError("ENCENT.OSS.CFT.KF.KF_Web.BaseAccount.freezeBankAcc", "protected void BT_F_Or_Not_Click(object sender, System.EventArgs e),�������Ṥ��ʱʧ��:", ex);
						WebUtils.ShowMessage(this.Page,"�������Ṥ��ʱʧ�ܣ�");
					//	return;
					}
					//furion end

                    if (exeSign) 
                    {
                        //����ɹ�������΢����Ϣ yinhuang 2014/09/23
                        //��΢�Ŷ�����Ϣ
                        if (uid.IndexOf("@wx.tenpay.com") > 0)
                        {
                            string reqsource = "bus_kf_freeze";
                            string accid = uid.Substring(0, uid.IndexOf("@wx.tenpay.com"));
                            string templateid = "Td2l1120f5TCN9Ap2R3yWLhVS7yy41U379MZudwmiH0";
                            string cont1 = "���΢��֧���˻��ѳɹ���������ģʽ���˻��ݲ����á�";
                            string cont2 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            string cont3 = "��������ָ�������ģʽ";
                            string msgtype = "freeze";
                            try
                            {
                                //Ϊ��Ӱ�����ϣ��ݲ������쳣
                                new FreezeService().SendWechatMsg(reqsource, accid, templateid, cont1, cont2, cont3, msgtype);
                            }
                            catch (Exception ef)
                            {
                                LogHelper.LogError("��΢�Ŷ�����Ϣ[new FreezeService().SendWechatMsg]�쳣��" + ef.ToString());
                                WebUtils.ShowMessage(this.Page, accid + "��΢�Ŷ�����Ϣ�쳣");
                            }
                        }
                    }
                    #endregion
                }
				else if (sign.ToLower() == "false")
                {
                    #region 
                    op_type = "2";
                    //�ⶳ��Ҫ���ݶ��������ж��Ƿ���Ȩ�� yinhuang 2013/12/9
                    string isChannel = "";
                    if (ViewState["freezeChannel"] != null && ViewState["freezeChannel"].ToString() != "") 
                    {
                        isChannel = ViewState["freezeChannel"].ToString();
                    }

                    string val = "";
                    string des = "";

                    //if (isChannel != "" && isChannel != "0")
                    //{
                    //    //���Ϊ��,����Ҫ����Ȩ���ж�;��Ϊ��,����Ҫ����Ȩ���ж�.
                    //    if (isChannel == "1" || isChannel == "6")
                    //    {
                    //        //�������
                    //        val = "UnFreezeChannelFK";
                    //        des = "��ض���";
                    //    }
                    //    else if (isChannel == "2")
                    //    {
                    //        //����
                    //        val = "UnFreezeChannelPP";
                    //        des = "���Ķ���";
                    //    }
                    //    else if (isChannel == "3")
                    //    {
                    //        //�û�
                    //        val = "UnFreezeChannelYH";
                    //        des = "�û�����";
                    //    }
                    //    else if (isChannel == "4")
                    //    {
                    //        //�̻�
                    //        val = "UnFreezeChannelSH";
                    //        des = "�̻�����";
                    //    }
                    //    else if (isChannel == "5")
                    //    {
                    //        //BG
                    //        val = "UnFreezeChannelBG";
                    //        des = "BG�ӿڶ���";
                    //    }
                    //}
                    //else 
                    //{
                    //    val = "UnFreezeChannelFK";
                    //    des = "��ض���";
                    //}
                    if (FreezeChannels.Exists(p => p.Value == isChannel))
                    {
                        val = FreezeChannels.Where(p => p.Value == isChannel).First().RightName;
                        des = FreezeChannels.Where(p => p.Value == isChannel).First().Text;
                    }
                    else
                    {
                        val = "UnFreezeChannelFK";
                        des = "��ض���";
                    }

                    if (val == "" || !classLibrary.ClassLib.ValidateRight(val, this))
                    {
                        //����Ȩ���ж�
                        WebUtils.ShowMessage(this.Page, "û�нⶳ��������Ϊ[" + des + "]��Ȩ�ޣ�");
                        return;
                    }

					Query_Service.Query_Service qs = new Query_Service.Query_Service();
					Query_Service.Finance_Header fhq = classLibrary.setConfig.setFH(this);
					qs.Finance_HeaderValue = fhq;

					//���ж����������������������ԭ���Ĵ������������ɻ��ڡ�
					long UNFreeze_BigMoney = long.Parse(System.Configuration.ConfigurationManager.AppSettings["UNFreeze_BigMoney"]);

					string Msg = "";
					long userbalance = 0;
                    if (ViewState["iswechat"].ToString() == "false")
                    {
                        userbalance = qs.GetUserBalance(uid, 1, out Msg);
                    }
                    
					if(userbalance < 0)
					{
						WebUtils.ShowMessage(this.Page,"��ѯ�û����ʧ�ܣ�" + classLibrary.setConfig.replaceHtmlStr(Msg));
						return;
					}

					if(userbalance >= UNFreeze_BigMoney)
					{
						//���������������
						Check_WebService.Check_Service cs = new Check_WebService.Check_Service();
						Check_WebService.Finance_Header fhc = classLibrary.setConfig.setFH_CheckService(this);

						cs.Finance_HeaderValue = fhc;

						Check_WebService.Param[] myparam = new Check_WebService.Param[8];

						myparam[0] = new Check_WebService.Param();
						myparam[0].ParamName = "uid";
						myparam[0].ParamValue = uid;

						myparam[1] = new Check_WebService.Param();
						myparam[1].ParamName = "mediflag";
						myparam[1].ParamValue = "false";

						myparam[2] = new Check_WebService.Param();
						myparam[2].ParamName = "username";
						myparam[2].ParamValue = fhc.UserName;

						myparam[3] = new Check_WebService.Param();
						myparam[3].ParamName = "userip";
						myparam[3].ParamValue = fhc.UserIP;

						myparam[4] = new Check_WebService.Param();
						myparam[4].ParamName = "type";
						myparam[4].ParamValue = "2";

						myparam[5] = new Check_WebService.Param();
						myparam[5].ParamName = "handleresult";
						myparam[5].ParamValue = classLibrary.setConfig.replaceSqlStr(tbMemo.Text.Trim());
						

						myparam[6] = new Check_WebService.Param();
						myparam[6].ParamName = "fid";
						myparam[6].ParamValue = ViewState["fid"].ToString();

						string returnUrl = "/BaseAccount/FreezeDetail.aspx?fid=" + ViewState["fid"].ToString();
						myparam[7] = new Check_WebService.Param();
						myparam[7].ParamName = "returnUrl";
						myparam[7].ParamValue = returnUrl;

						string fmemo = "�ⶳ�����û���" + uid + "�����Ϊ��" + MoneyTransfer.FenToYuan(userbalance);

						string mainId    = DateTime.Now.ToString("yyyyMMddHHmmssfff");
						cs.StartCheck(mainId,"UNFreezeCheck",fmemo,MoneyTransfer.FenToYuan(userbalance.ToString()),myparam);

						WebUtils.ShowMessage(this.Page,"�ⶳ�˻����ϴ󣬷��������ɹ���");
						this.Button1_back.Visible = true;
						this.BT_F_Or_Not.Visible = false;
						return;
					}

                    if (Request.QueryString["type"] == null)
                    {
                        fm.freezeAccount(uid, 2);
                    }
                    else
                    {
                        string uname = "";
                        if (ViewState["tuserName"] != null && ViewState["tuserName"].ToString() != "")
                        {
                            uname = ViewState["tuserName"].ToString();
                        }

                        bool refreeze = false;

                        if (ViewState["iswechat"].ToString() == "true")
                        {
                            //΢�Ŵ�������
                            //fm.FreezePerAccountWechat(uid, 2);
                            refreeze = fm.UnFreezePerAccountWechat_New(uid, uname);
                        }
                        else
                        {
                            //�ⶳ 2 ui_unfreeze_user_service
                            refreeze = fm.freezePerAccount(uid, 2, uname, "");
                        }

                        if (refreeze)
                        {
                            //�ⶳ�ɹ�������΢����Ϣ
                            //��΢�Žⶳ��Ϣ
                            if (uid.IndexOf("@wx.tenpay.com") > 0)
                            {
                                string reqsource = "bus_kf_unfreeze";//�ͷ��ⶳ
                                string accid = uid.Substring(0, uid.IndexOf("@wx.tenpay.com"));
                                string templateid = "DeNkYEfSBW7mVQET6QHwnilGWvG8cLssLSyRH0CSDk0";
                                string cont1 = "���΢��֧���˻����ų��˰�ȫ���ղ��ɱ���ģʽ�л�������ģʽ��";
                                string cont2 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                string cont3 = "��������鿴΢��֧����ȫ���Ͻ���";
                                string msgtype = "unfreeze";
                                try
                                {
                                    new FreezeService().SendWechatMsg(reqsource, accid, templateid, cont1, cont2, cont3, msgtype);
                                }
                                catch(Exception ef)
                                {
                                    LogHelper.LogError("��΢�Žⶳ��Ϣ[new FreezeService().SendWechatMsg]�쳣��" + ef.ToString());

                                    WebUtils.ShowMessage(this.Page, accid + "��΢�Ŷ�����Ϣ�쳣");
                                }
                            }
                        }
                    }


					if(ViewState["fid"] != null && ViewState["fid"].ToString() != "")  //������������쳣,Ϊ��˵����QQ��ע���,��������
					{
						try
						{
							Query_Service.FreezeInfo fi = new FreezeInfo();
							fi.fid = ViewState["fid"].ToString();
							fi.FHandleResult = tbMemo.Text.Trim();
							fi.FFreezeType = 1;
							fi.FHandleResult = classLibrary.setConfig.replaceMStr(fi.FHandleResult);
							qs.UpdateFreezeInfo(fi);
						}
						catch(Exception ex)
                        {
                            LogError("ENCENT.OSS.CFT.KF.KF_Web.BaseAccount.freezeBankAcc", "protected void BT_F_Or_Not_Click(object sender, System.EventArgs e),�����Ṥ��ʱʧ��:", ex);
							WebUtils.ShowMessage(this.Page,"�����Ṥ��ʱʧ�ܣ�");
						//	return;
						}
                    }
                    #endregion
                }

                //���ͨ�˻������ⶳ����
                try
                {
                    AccountService acc = new AccountService();
                    string ip = Request.UserHostAddress.ToString();
                    if (ip == "::1")
                        ip = "127.0.0.1";
                    Boolean state = acc.LCTAccStateOperator(uid, op_type, Session["uid"].ToString(), ip);
                }
                catch (Exception err)
                {
                    LogError("ENCENT.OSS.CFT.KF.KF_Web.BaseAccount.freezeBankAcc", "protected void BT_F_Or_Not_Click(object sender, System.EventArgs e),�������ͨ�˻�״̬ʧ��:", err);
                    string errStr = PublicRes.GetErrorMsg(err.Message.ToString());
                    WebUtils.ShowMessage(this.Page, "�������ͨ�˻�״̬ʧ�ܣ�" + errStr);
                    return;
                }

				this.Button1_back.Visible = true;
				this.BT_F_Or_Not.Visible = false;
			}
			catch(SoapException er) //����soap��
            {
                LogError("ENCENT.OSS.CFT.KF.KF_Web.BaseAccount.freezeBankAcc", "protected void BT_F_Or_Not_Click(object sender, System.EventArgs e),��ѯ����SoapException:", er);
				string str = PublicRes.GetErrorMsg(er.Message.ToString());
				WebUtils.ShowMessage(this.Page,"��ѯ����"+ str);
			}
			catch(Exception eSys)
            {
                LogError("ENCENT.OSS.CFT.KF.KF_Web.BaseAccount.freezeBankAcc", "protected void BT_F_Or_Not_Click(object sender, System.EventArgs e),end error:", eSys);
				WebUtils.ShowMessage(this.Page,"����ʧ�ܣ������ԣ�"+ eSys.Message.ToString());
			}	
		}

		protected void Button1_back_Click(object sender, System.EventArgs e)
		{
			if(Session["uid"] == null)
				Response.Redirect("../login.aspx?wh=1"); //���µ�½
			
			uid = ViewState["uid"].ToString();

			if (Request.QueryString["type"] == null)
				Response.Write("<script language=javascript>window.parent.WorkArea.location='UserBankInfoQuery.aspx?id=" + uid + " '</script>"); 
			else
				Response.Write("<script language=javascript>window.location='InfoCenter.aspx?id=" + uid + " '</script>"); 
		}

		private void cbx_showEndDate_CheckedChanged(object sender, EventArgs e)
		{
			
		}
	}
    public class FreezeChannel
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public string RightName { get; set; }
    }
}
