using System;
using System.IO;
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
using CFT.CSOMS.BLL.RefundModule;
using CFT.CSOMS.BLL.TransferMeaning;
using CFT.CSOMS.BLL.CFTAccountModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.RefundManage
{
	/// <summary>
	/// fetchName ��ժҪ˵����
	/// </summary>
    public partial class RefundDetails : System.Web.UI.Page
	{
        private string m_nFrom = "1111";
        private string m_nRefundReason = "�˿�ʧ�ܣ�����ת��ͷ�����.";

        private string[] m_arrCheckRecord = 
       {
            "�ͷ����",       
            "BG���",
            "������",
            "δ��������"
            
        };

        public string[] m_arrUserFlag = 
        {
            "����",
            "��˾",
            "δ֪����",

        };
        public string[] m_arrCardType = 
        {
            "��ǿ�",
            "���ÿ�",
            "δ֪���Ϳ�",
        };
        private  static log4net.ILog s_log;
        private  static log4net.ILog log
        {
            get
            {
                if (s_log == null)
                {
                    s_log = log4net.LogManager.GetLogger("�����˿��ѯ");
                }

                return s_log;
            }
        }
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			//��֤Ȩ��
            //lbOperator.Text = Session["OperID"].ToString();           
              
              try
              {
                 /*
                  //������
                   if (!string.IsNullOrEmpty(RefundPublicFun.operatorName))
                  {
                      Session["uid"] = RefundPublicFun.operatorName;
                  }*/
                  //����ֱ��URL���ò�ѯ
                  if (Session["uid"] == null)
                  {
                      if (Request.QueryString["system"] != null && Request.QueryString["system"].ToString().Trim() == "admin")
                      {
                          Session["uid"] = "caiwu";
                      }
                      else 
                      {
                          Response.Redirect("../login.aspx?wh=1");
                      }
                  }
                
                  
                  if (!IsPostBack)
                  {
                      BindData();    
                  }
                  
              }
              catch (Exception eSys)
              {
                  if (Session["uid"] == null)
                  {
                      log.ErrorFormat("�����˿�����ҳ�����1�� �˿�ţ�{0} �������ԭ��:{1}", Request.QueryString["foldId"].ToString(), eSys.Message);                                 
                  }
                  else
                  {
                      log.ErrorFormat("�����˿�����ҳ�����2�������ߣ�{0} �˿�ţ�{1} �������ԭ��:{2}", Session["uid"].ToString(), Request.QueryString["foldId"].ToString(), eSys.Message);                                 
                  }
                  
              }
		}
		private void BindData()
		{
            string strMsg = null;
            if (Request.QueryString["foldId"] == null)
            {
                log.ErrorFormat("BindData�� Request.QueryString[foldId] = null ");                                 
            }
            ViewState["foldId"] = Request.QueryString["foldId"].ToString();
            log.InfoFormat("ViewState[foldId]={0}", ViewState["foldId"].ToString());

            //��վ�Ǳ��ռ������ϣ�����״̬����������ID         
            DataSet ds = new RefundService().RequestDetailsData(ViewState["foldId"].ToString(), out strMsg);
            if (ds == null)
            {
                WebUtils.ShowMessage(this.Page, "��ѯ�˿�����ʧ��" + strMsg);
                return;
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                lbUinID.Text = dr["FpayListid"].ToString();
                lbBankListID.Text = dr["FbankListid"].ToString();
                lbIdentity.Text = dr["Fidentity"].ToString();
                lbInitBank.Text = dr["FbankAccNoOld"].ToString();
                lbInitBankName.Text = Transfer.convertbankType(dr["FbankTypeOld"].ToString());
                lbMail.Text = dr["FUserEmail"].ToString();
                lbNewBank.Text = dr["FbankAccNo"].ToString();
                lbNewBankType.Text = Transfer.convertbankType(dr["FbankType"].ToString());
                lbUser.Text = dr["FtrueName"].ToString();
                txt_Reason.Text = dr["Fkfremark"].ToString();
                lbCreateTime.Text = dr["FcreateTime"].ToString();

                kfOperator.Text = "";
                string strOperater = dr["FStandby1"].ToString();
                if(!string.IsNullOrEmpty(strOperater))
                {
                    string[] strAryOperator = strOperater.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (strAryOperator.Length == 2)
                    {
                        ViewState["kfOperator"] = strAryOperator[0];    // ��¼�����ύ��  �û�
                        kfOperator.Text = string.Format("�����ύ�ߣ�{0} �ύʱ��:{1}", strAryOperator[0], strAryOperator[1]);
                    }
                    else
                    {
                        kfOperator.Text = strOperater;
                    }
                    
                }
                
                int nUserFlag = int.Parse(dr["FuserFlag"].ToString());
                if(nUserFlag <m_arrUserFlag.Length  && nUserFlag > 0)
                {
                    lbUserFlag.Text = m_arrUserFlag[nUserFlag - 1];
                }               
                int nCardType = int.Parse(dr["FCardType"].ToString());
                if (nCardType < m_arrCardType.Length && nCardType > 0)
                {
                    lbCardType.Text = m_arrCardType[nCardType - 1];
                }
                lbBankName.Text = dr["FbankName"].ToString();

                //������վ�ռ������ϲ���������
                string strState = dr["Fstate"].ToString();
                string strFrom = dr["FStandby2"].ToString();
                string strHisCheckID = dr["FHisCheckID"].ToString();
                string strCheckId = dr["FcheckID"].ToString();
                if (strState.Trim() == "2" && strFrom.Trim() == m_nFrom.Trim() && string.IsNullOrEmpty(strCheckId))
                {
                    log.InfoFormat("������վ�����ӣ�  �˿�ţ�{1} ", ViewState["foldId"].ToString());
                    //�����������
                    ZWCheck_Service.Check_Service chkService = new ZWCheck_Service.Check_Service();
                    chkService.Finance_HeaderValue = RefundPublicFun.SetWebServiceHeader(this);

                    /*objectID��Ϲ��� =   
                    Ŀ�ģ�ȷ��ÿ������Ψһ*/
                    string strObjectID = ViewState["foldId"].ToString();
                    if (!string.IsNullOrEmpty(strHisCheckID))
                    {
                        string[] arrHisCheckId = strHisCheckID.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < arrHisCheckId.Length; ++i)
                        {
                            strObjectID += arrHisCheckId[i];
                        }
                    }

                    strCheckId = chkService.StartCheckNew(strObjectID, RefundPublicFun.KFCHECKTYPE, RefundPublicFun.KFCHECKMEMO);
                    if (!string.IsNullOrEmpty(strCheckId))
                    {
                        new RefundService().SetAbnormalRefundListID(strCheckId, ViewState["foldId"].ToString());
                    }
                }

                //����Ǵ���վ�Ǳ�¼�����Ϣ
                if (strFrom.Trim() == m_nFrom.Trim())
                {
                    //��ͼƬcgi
                    string urlCGI = System.Configuration.ConfigurationManager.AppSettings["GetAppealImageCgi"].Trim();
                    igCommitment.ImageUrl = urlCGI + dr["FCommitmentFile"].ToString();
                    igIdentity.ImageUrl = urlCGI + dr["FIdentityCardFile"].ToString();
                    igBankWater.ImageUrl = urlCGI + dr["FBankWaterFile"].ToString();
                    igAccount.ImageUrl = urlCGI + dr["FCancellationFile"].ToString();
                }
                else
                {
                    string requestUrl = System.Configuration.ConfigurationManager.AppSettings["GetImageFromKf2Url"].ToString();
                    string strLocFile = System.Configuration.ConfigurationManager.AppSettings["KFWebSrc"].ToString();
                    string strLocCommitment = strLocFile + dr["FCommitmentFile"].ToString();
                    string strLocIdentity = strLocFile + dr["FIdentityCardFile"].ToString();
                    string strLocBankWater = strLocFile + dr["FBankWaterFile"].ToString();
                    string strLocAccount = strLocFile + dr["FCancellationFile"].ToString();

                    igCommitment.ImageUrl = File.Exists(strLocCommitment.Trim()) ? "../" + dr["FCommitmentFile"].ToString() : requestUrl + "/" + dr["FCommitmentFile"].ToString();
                    igIdentity.ImageUrl = File.Exists(strLocIdentity.Trim()) ? "../" + dr["FIdentityCardFile"].ToString() : requestUrl + "/" + dr["FIdentityCardFile"].ToString();
                    igBankWater.ImageUrl = File.Exists(strLocBankWater.Trim()) ? "../" + dr["FBankWaterFile"].ToString() : requestUrl + "/" + dr["FBankWaterFile"].ToString();
                    igAccount.ImageUrl = File.Exists(strLocAccount.Trim()) ? "../" + dr["FCancellationFile"].ToString() : requestUrl + "/" + dr["FCancellationFile"].ToString();
 
                }
                log.InfoFormat("��ʾ�洢·��ͼƬ��igCommitment = ��{0} igIdentity = ��{1} igBankWater = ��{2} igAccount = ��{3} strCheckId={4}", igCommitment.ImageUrl, igIdentity.ImageUrl, igBankWater.ImageUrl, igAccount.ImageUrl, strCheckId);                                                
                
                
                if (!string.IsNullOrEmpty(strCheckId.Trim()))
                {
                    log.InfoFormat("�ǿ�:strCheckId={0}", strCheckId);
                    ViewState["checkId"] = strCheckId.Trim();
                    ViewState["state"] = dr["Fstate"].ToString();
                    //��ǰ������¼
                    ShowCurCheckRecord();
                }
                else
                {                  
                    SetBtnEnabledState(false, 0);
                    lbKfCheckTime.Text = lbKfCheckName.Text = lbKfCheckReason.Text = null;
                    lbBgCheckTime.Text = lbBgCheckName.Text = lbBgCheckReason.Text = null;
                    lbFengCheckTime.Text = lbFengCheckName.Text = lbFengCheckReason.Text = null;
                    log.InfoFormat("Ϊ��:strCheckId={0}", strCheckId);
                }
                
                //��ʷ������¼
                ShowHistoryCheckRecord(strCheckId);
            }

            //��̨����
            if (!string.IsNullOrEmpty(lbUinID.Text))
            {
                /*step1:���ݲƸ�ͨ������ �� �Ƹ�ͨ��
                  step2:�Ƹ�ͨ�� �� ������Ϣ
                 */
                string msg = null;
                string strUid = new RefundService().GetBankCardBindInformation(lbUinID.Text, out msg);
                msg += "���ýӿ�ȡ��ֵ��:"+strUid;
                log.InfoFormat("�����˿��̨���ݣ������ߣ�{0} �˿�ţ�{1} �Ƹ�ͨ����:{2} ���صĽ����Ϣ:{3}", Session["uid"].ToString(), ViewState["foldId"].ToString(), lbUinID.Text, msg);
                Query_Service.Query_Service myService = new Query_Service.Query_Service();
                myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

                log.InfoFormat("�����ߣ�{0} ͷ��Ϣ SrcUrl={1},UserIP={2}, UserName={3},SessionID={4},SzKey={5},OperID={6},RightString={7}",
                    Session["uid"].ToString(), myService.Finance_HeaderValue.SrcUrl, myService.Finance_HeaderValue.UserIP, myService.Finance_HeaderValue.UserName,
                    myService.Finance_HeaderValue.SessionID, myService.Finance_HeaderValue.SzKey, myService.Finance_HeaderValue.OperID, myService.Finance_HeaderValue.RightString);  
                if (!string.IsNullOrEmpty(strUid))
                {
                    log.InfoFormat("�������Ϣ��Ϊ�գ������ߣ�{0} ", Session["uid"].ToString());


                    DataSet tmpds = new AccountService().GetUserInfo(strUid.Trim(), 1, 1);
                    if (tmpds == null || tmpds.Tables.Count < 1 || tmpds.Tables[0].Rows.Count < 1)
                    {
                        msg += string.Format("���ݲƸ�ͨ�ʺţ�{0} �������ϢΪ��:", strUid);
                        log.InfoFormat("�������ϢΪ�գ������ߣ�{0} �˿�ţ�{1} �Ƹ�ͨ����:{2} ���صĽ����Ϣ:{3}", Session["uid"].ToString(), ViewState["foldId"].ToString(), lbUinID.Text, msg);                 
                        return;
                    }
                    log.InfoFormat("����ֵΪFtruename={0},Fmobile={1},Fcreid={2}", tmpds.Tables[0].Rows[0]["Ftruename"].ToString(), tmpds.Tables[0].Rows[0]["Fmobile"].ToString(),classLibrary.setConfig.ConvertCreID(tmpds.Tables[0].Rows[0]["Fcreid"].ToString()));    
                    ltruename.Text = tmpds.Tables[0].Rows[0]["Ftruename"].ToString();    
                    lPhone.Text = tmpds.Tables[0].Rows[0]["Fmobile"].ToString();
                    lIdentity.Text = classLibrary.setConfig.ConvertCreID(tmpds.Tables[0].Rows[0]["Fcreid"].ToString());
                }
            }
        }
        //��ǰ������¼
        private void ShowCurCheckRecord()
        {
            log.InfoFormat("ShowCurCheckRecord������");
            if (ViewState["checkId"] == null || Session["uid"] == null || ViewState["foldId"] == null)
            {
                log.InfoFormat("��ǰ����ShowCurCheckRecord��ViewState[checkId] == null ||  Session[uid] == null|| ViewState[foldId] == null");
                return;
            }
            log.InfoFormat("��ʼ����Ȩ���жϽӿ� �����ߣ�{0} �˿�ţ�{1} checkId:{2}",Session["uid"].ToString(), ViewState["foldId"].ToString(),ViewState["checkId"].ToString());
            ZWCheck_Service.Check_Service chkService = new ZWCheck_Service.Check_Service();
            bool bState = false;
            try 
            {
                 bState = chkService.ValiCheckRight(Session["uid"].ToString(), ViewState["checkId"].ToString());
            }
            catch (Exception eSys)
            {
                log.ErrorFormat("Ȩ���ж��쳣�������ߣ�{0} �˿�ţ�{1} �������ԭ��:{2}", Session["uid"].ToString(), ViewState["foldId"].ToString(), eSys.Message);                                 
            }
            SetBtnEnabledState(bState, Convert.ToInt32(ViewState["state"]));
            if (Session["uid"] != null && ViewState["foldId"] != null)
            {
                log.InfoFormat("Ȩ���жϣ������ߣ�{0} �˿�ţ�{1} ��¼״̬:{2} Ȩ���жϷ��ؽ��:{3}", Session["uid"].ToString(), ViewState["foldId"].ToString(), Convert.ToInt32(ViewState["state"]), bState);                 
            }
           // chkService.Finance_HeaderValue = RefundPublicFun.SetWebServiceHeader(this);
           // log.InfoFormat("�������ͷ���������Ϣֵ UserIP:{0} UserName:{1} SzKey:{2} OperID:{3} RightString:{4}", chkService.Finance_HeaderValue.UserIP, chkService.Finance_HeaderValue.UserName, chkService.Finance_HeaderValue.SzKey, chkService.Finance_HeaderValue.OperID, chkService.Finance_HeaderValue.RightString);
            //��ʾ��ǰ������¼����ʷ������¼
            DataSet curDsData = chkService.GetCheckLogByType(ViewState["checkId"].ToString(), 1);
            if (curDsData != null && curDsData.Tables.Count > 0 && curDsData.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in curDsData.Tables[0].Rows)
                {
                    switch (dr["FLevel"].ToString().Trim())
                    {
                        case "1":
                            {
                                lbKfCheckTime.Text = dr["FCheckTime"].ToString();
                                lbKfCheckName.Text = dr["FCheckuser"].ToString();
                                lbKfCheckReason.Text = dr["FCheckMemo"].ToString();
                                break;
                            }
                        case "2":
                            {
                                lbBgCheckTime.Text = dr["FCheckTime"].ToString();
                                lbBgCheckName.Text = dr["FCheckuser"].ToString();
                                lbBgCheckReason.Text = dr["FCheckMemo"].ToString();
                                break;
                            }
                        case "3":
                            {
                                lbFengCheckTime.Text = dr["FCheckTime"].ToString();
                                lbFengCheckName.Text = dr["FCheckuser"].ToString();
                                lbFengCheckReason.Text = dr["FCheckMemo"].ToString();
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
        }

        

        //��ʷ������¼
        private void ShowHistoryCheckRecord(string strCheckId)
        {
            string strHistory = null;
            if (ViewState["foldId"] == null)
            {
                return;
            }
            ZWCheck_Service.Check_Service chkService = new ZWCheck_Service.Check_Service();
            //����ת�ͷ���ʷ��¼
            DataSet zwDs = new RefundService().CheckFZWCheckMemo(ViewState["foldId"].ToString());
            if (zwDs != null && zwDs.Tables.Count > 0 && zwDs.Tables[0].Rows.Count > 0)
            {
                string[] arrHisCheckId = null;
                string strHisCheckID = zwDs.Tables[0].Rows[0]["FHisCheckID"].ToString();
                if (!string.IsNullOrEmpty(strHisCheckID))
                {
                    arrHisCheckId = strHisCheckID.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                }
                
                string strZWCheckMemo = zwDs.Tables[0].Rows[0]["FZWCheckMemo"].ToString();
                //���������������������û�з���һ����ֱ��ȡ���
                if (!string.IsNullOrEmpty(strZWCheckMemo))
                {
                    string[] arrZWCheckMemo = strZWCheckMemo.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int index = arrZWCheckMemo.Length-1; index >= 0; --index)
                    {
                        strHistory += string.Format("{0}\t{1}\t {2}", "��������", arrZWCheckMemo[index], m_nRefundReason) + "<br>";
                        if (arrHisCheckId != null && !string.IsNullOrEmpty(arrHisCheckId[index]))
                        {
                            ShowHistorySystle(arrHisCheckId[index], chkService,ref strHistory);
                        }
                    }                   
                }
            }
            //��ǰ��������ʷ����
            if (ViewState["checkId"] != null)
            {
                DataSet hisDsData = chkService.GetCheckLogByType(ViewState["checkId"].ToString(), 2);
                ShowHistorySystle(ViewState["checkId"].ToString(), chkService, ref strHistory);
            }
            lbHistory1.Text = strHistory;
 
        }

        private void ShowHistorySystle(string strCheckId, ZWCheck_Service.Check_Service chkService, ref string strHistory)
        {
            if (string.IsNullOrEmpty(strCheckId))
            {
                return;
            }
            DataSet hisDsData = chkService.GetCheckLogByType(strCheckId, 2);
            if (hisDsData != null && hisDsData.Tables.Count > 0 && hisDsData.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in hisDsData.Tables[0].Rows)
                {
                    int nIndex = int.Parse(dr["FLevel"].ToString()) - 1;
                    if (nIndex >= 0 && nIndex < m_arrCheckRecord.Length - 1)
                    {
                        strHistory += string.Format("{0}\t{1}\t {2}\t {3}", m_arrCheckRecord[nIndex], dr["FCheckuser"].ToString(), dr["FCheckTime"].ToString(), dr["FCheckMemo"].ToString());
                    }
                    else
                    {
                        strHistory += "�����ȼ������˷�Χ" + dr["FLevel"].ToString();
                    }
                    strHistory += "<br>";
                }
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

        private void SetRefundCheckState(int nState,string strOldId)
        {
            try
            {
                new RefundService().SetRefundCheckState(nState, strOldId);
            }
            catch (Exception ex)
            {
                log.ErrorFormat("�����˿�״̬�������ߣ�{0} �˿��strOldId=��{1} �������ԭ��:{2}", Session["uid"].ToString(), strOldId, ex.Message); 
                throw new Exception(ex.Message);
            }
        }
        //ͨ��
        protected void btnPass_Click(object sender, System.EventArgs e)
		{
            try
            {
                if (ViewState["foldId"] == null)
                {
                    WebUtils.ShowMessage(this.Page, "ViewState[foldId]Ϊ�ա�");
                    return;
                }
                if (ViewState["checkId"] == null)
                {
                    WebUtils.ShowMessage(this.Page, "checkIdΪ�� null");
                    return;
                }
                string strCheckId = ViewState["checkId"].ToString();
                ZWCheck_Service.Check_Service chkService = new ZWCheck_Service.Check_Service();
                chkService.Finance_HeaderValue = RefundPublicFun.SetWebServiceHeader(this);
                
                if (!chkService.ValiCheckRight(Session["uid"].ToString(), strCheckId))
                {
                    log.InfoFormat("ͨ��ʱȨ���ж�ûͨ���������ߣ�{0} checkId��{1} ��¼״̬:{2} ", Session["uid"].ToString(), strCheckId, Convert.ToInt32(ViewState["state"]));                 
                    WebUtils.ShowMessage(this.Page, "����Ȩ�޲��ԣ�");
                    return;
                }           
                if (chkService.DoCheckNew(strCheckId, "ͬ��", 0))
                {
                    int nState = Convert.ToInt32(ViewState["state"]) + 1;
                    SetRefundCheckState(nState, ViewState["foldId"].ToString());
                    SetBtnEnabledState(false, 0);
                    log.InfoFormat("����ͨ���������ߣ�{0} checkId��{1} ��¼״̬:{2} ", Session["uid"].ToString(), strCheckId, nState); 
                    WebUtils.ShowMessage(this.Page, "��������Ч��");
                }
                else
                {
                    log.ErrorFormat("����ͨ���������ߣ�{0} DoCheckNew���ؽ��Ϊfalse", Session["uid"].ToString()); 
                    WebUtils.ShowMessage(this.Page, "ͬ�������������false");
                }

            }
            catch (Exception ex)
            {
                log.ErrorFormat("����ͨ���������ߣ�{0} �˿�ţ�{1} �������ԭ��:{2}", Session["uid"].ToString(), ViewState["foldId"].ToString(), ex.Message); 
                //WebUtils.ShowMessage(this.Page, "����ʧ�ܣ�"+ex.Message);
            }

		}
        //�ܾ�
        protected void btnRefuse_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (ViewState["foldId"] == null)
                {
                    WebUtils.ShowMessage(this.Page, "ViewState[foldId]Ϊ�ա�");
                    return;
                }
                if (ViewState["checkId"] == null)
                {
                    WebUtils.ShowMessage(this.Page, "checkIdΪ�ա�");
                    return;
                }
                if (string.IsNullOrEmpty(lbWriteReason.Text))
                {
                    WebUtils.ShowMessage(this.Page, "��ѡ��ܾ�ԭ����ֶ���дԭ��");
                    return;
                }
                string strCheckId = ViewState["checkId"].ToString();
                ZWCheck_Service.Check_Service chkService = new ZWCheck_Service.Check_Service();
                chkService.Finance_HeaderValue = RefundPublicFun.SetWebServiceHeader(this);
                //Session["uid"].ToString()
                if (!chkService.ValiCheckRight(Session["uid"].ToString(), strCheckId))
                {
                    log.InfoFormat("ͨ��ʱȨ���ж�ûͨ���������ߣ�{0} checkId��{1} ��¼״̬:{2} ", Session["uid"].ToString(), strCheckId, Convert.ToInt32(ViewState["state"]));                 
                    WebUtils.ShowMessage(this.Page, "����Ȩ�޲��ԣ�");
                    return;
                }
                if (chkService.DoCheckNew(strCheckId, lbWriteReason.Text, 1))
                {
                    //�������¿�ʼ
                    SetRefundCheckState(0, ViewState["foldId"].ToString());
                    SetBtnEnabledState(false, 0);
                    log.InfoFormat("�����ܾ��ɹ��������ߣ�{0} checkId��{1} ", Session["uid"].ToString(), strCheckId); 
                    WebUtils.ShowMessage(this.Page, "��������Ч��");
                    RefusedSendEmail();
                }
                else
                {
                    log.ErrorFormat("�����ܾ��������ߣ�{0} DoCheckNew���ؽ��Ϊfalse", Session["uid"].ToString()); 
                    WebUtils.ShowMessage(this.Page, "�ܾ������������false");
                }

            }
            catch (Exception ex)
            {
                log.ErrorFormat("�����ܾ��������ߣ�{0} �˿�ţ�{1} �������ԭ��:{2}", Session["uid"].ToString(), ViewState["foldId"].ToString(), ex.Message); 
               // WebUtils.ShowMessage(this.Page, "����ʧ�ܣ�" + ex.Message); 
            }

        }
        //ת�����
        protected void btnTransferCW_Click(object sender, System.EventArgs e)
        {
            if (ViewState["foldId"] == null )
            {
                WebUtils.ShowMessage(this.Page, "foldIdΪ�ա�");
                return;
            }
            if (ViewState["checkId"] == null)
            {
                WebUtils.ShowMessage(this.Page, "checkIdΪ�ա�");
                return;
            }

            string strCheckId = ViewState["checkId"].ToString();
            ZWCheck_Service.Check_Service chkService = new ZWCheck_Service.Check_Service();
            chkService.Finance_HeaderValue = RefundPublicFun.SetWebServiceHeader(this);
            
            if (!chkService.ValiCheckRight(Session["uid"].ToString(), strCheckId))
            {
                log.InfoFormat("ͨת�����ʱȨ���ж�ûͨ���������ߣ�{0} checkId��{1} ", Session["uid"].ToString(), strCheckId);                 
                WebUtils.ShowMessage(this.Page, "����Ȩ�޲��ԣ�");
                return;
            }
            if (!chkService.DoCheckNew(strCheckId, "ͬ�Ⲣת�������", 0))
            {
                log.ErrorFormat("ת���������ͨ���������ߣ�{0} DoCheckNew���ؽ��Ϊfalse", Session["uid"].ToString());
                WebUtils.ShowMessage(this.Page, "ͬ�������������false");
                return;
            }

            SetRefundCheckState(5, ViewState["foldId"].ToString());
            WebUtils.ShowMessage(this.Page, "ת������ɹ���");
            SetBtnEnabledState(false, 0);
            log.InfoFormat("�����˿�ת����񣺲����ߣ�{0} �� �˿��Ϊ:{1}ת�������", Session["uid"].ToString(), ViewState["foldId"].ToString());

 
        }
        private void SetBtnEnabledState(bool bState,int nState)
        {
            //ֻ��������Ȩ�޼���ǰ��¼��������ʱ���ſ�ʹ�ð�ť
            if (bState == true && (nState > 1 && nState < 6))
            {
                btnCommit.Enabled   = true;
                btnRefuse.Enabled   = true;
                btnCW.Enabled       = true;
                //btnInvalid.Enabled  = true;
                txt_Reason.Width = 480;
                txt_Reason.Enabled = true;
                Button1.Visible = true;
            }
            else
            {
                btnCommit.Enabled   = false;
                btnRefuse.Enabled   = false;
                btnCW.Enabled       = false;
                //btnInvalid.Enabled  = false;
                txt_Reason.Width = 560;
                txt_Reason.Enabled = false;
                Button1.Visible = false;
            }

        }

        protected void OnDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {          
            lbWriteReason.Text = dropReasonList.SelectedItem.Text;   
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                var foldId = ViewState["foldId"].ToString();
                string strMsg = "";
                var result = new RefundService().UpdateRefundData(new string[] { foldId }, null, null, null, null, null, txt_Reason.Text, null, null, null, null, null, null, -1, -1, null, null, -1, out strMsg);
                if (result)
                {
                    WebUtils.ShowMessage(this.Page, "�޸ĳɹ���");
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "�޸�ʧ��" + strMsg);
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, PublicRes.GetErrorMsg("�޸ĳ���" + ex.Message));
            }
        }

        /// <summary>
        /// �ܾ������ʼ� �� �����ύ��
        /// </summary>
        private void RefusedSendEmail()
        {
            try
            {
                var uin = ViewState["kfOperator"] as string;
                if (uin != null)
                {
                    var listid = lbUinID.Text;
                    var mailBody = uin + "��\r\n" +
                        "���ύ�������˿�� " + listid + " ����ʧ�ܣ�������ʱ��ע��лл��";
                    TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendInternalMail(uin, "", "�����˿������ʧ��֪ͨ", mailBody);
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("�����ܾ��������ߣ�{0} �˿�ţ�{1} �����ʼ��������ύ�߷���ʧ��:{2}", Session["uid"].ToString(), ViewState["foldId"].ToString(), ex.Message); 
            }
        }
        /*
        //����
        protected void btnTransferInvalid_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["foldId"] == null)
                {
                    WebUtils.ShowMessage(this.Page, "ViewState[foldId]Ϊ�ա�");
                    return;
                }
                if (ViewState["checkId"] == null)
                {
                    WebUtils.ShowMessage(this.Page, "checkIdΪ�ա�");
                    return;
                }
                string strCheckId = ViewState["checkId"].ToString();
                ZWCheck_Service.Check_Service chkService = new ZWCheck_Service.Check_Service();
                chkService.Finance_HeaderValue = RefundPublicFun.SetWebServiceHeader(this);
                //Session["uid"].ToString()
                if (!chkService.ValiCheckRight(Session["uid"].ToString(), strCheckId))
                {
                    log.InfoFormat("����ʱȨ���ж�ûͨ���������ߣ�{0} checkId��{1} ", Session["uid"].ToString(), strCheckId);  
                    WebUtils.ShowMessage(this.Page, "����Ȩ�޲��ԣ�");
                    return;
                }
                if (chkService.DoCheckNew(strCheckId, "���ϴ˵����޷�����", 1))
                {
                    //ת���޷�����
                    new RefundService().SetRefundCheckState(7, ViewState["foldId"].ToString());
                    SetBtnEnabledState(false, 0);
                    WebUtils.ShowMessage(this.Page, "���ϴ˵�����Ч��");
                    log.InfoFormat("�˿����ϣ������ߣ�{0} �� �˿��Ϊ:{1}ת���޷�����", Session["uid"].ToString(), ViewState["foldId"].ToString());
                }
                else
                {
                    log.ErrorFormat("�˿����ϣ������ߣ�{0} DoCheckNew���ؽ��Ϊfalse", Session["uid"].ToString());
                    WebUtils.ShowMessage(this.Page, "�ܾ������������false");
                }

            }
            catch (Exception ex)
            {
                log.ErrorFormat("�˿����ϣ������ߣ�{0} �˿�ţ�{1} �������ԭ��:{2}", Session["uid"].ToString(), ViewState["foldId"].ToString(), ex.Message);
                
            }

        }

        //��ԭ
        protected void btnInitState_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["uid"].ToString().Trim() != RefundPublicFun.OPERATOR.Trim())
                {
                    WebUtils.ShowMessage(this.Page, "û��Ȩ�޲��������");
                    return;
                }

                if (ViewState["foldId"] == null)
                {
                    WebUtils.ShowMessage(this.Page, "ViewState[foldId]Ϊ�ա�");
                    return;
                }
                string strHisCheckID = "";
                if (string.IsNullOrEmpty(new RefundService().QueryAbnormalRefundCheckID(ViewState["foldId"].ToString(),ref strHisCheckID)))
                {
                    new RefundService().SetRefundCheckState(0, ViewState["foldId"].ToString());
                    WebUtils.ShowMessage(this.Page, "��ʼ���ɹ���");
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "checkID��Ϊ��,�������ʼ��������");
                    return;
                }

            }
            catch (Exception ex)
            {
                log.ErrorFormat("�����˿��ʼ����ԭ����:������{0} �˿�ţ�{1} �������ԭ��:{2}", Session["uid"].ToString(), ViewState["foldId"].ToString(), ex.Message);

            }

        }
         * */

	}
}
