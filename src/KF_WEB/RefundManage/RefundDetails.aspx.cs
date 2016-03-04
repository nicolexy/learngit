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
	/// fetchName 的摘要说明。
	/// </summary>
    public partial class RefundDetails : System.Web.UI.Page
	{
        private string m_nFrom = "1111";
        private string m_nRefundReason = "退款失败，财务转向客服处理.";

        private string[] m_arrCheckRecord = 
       {
            "客服审核",       
            "BG审核",
            "风控审核",
            "未定义审批"
            
        };

        public string[] m_arrUserFlag = 
        {
            "个人",
            "公司",
            "未知类型",

        };
        public string[] m_arrCardType = 
        {
            "借记卡",
            "信用卡",
            "未知类型卡",
        };
        private  static log4net.ILog s_log;
        private  static log4net.ILog log
        {
            get
            {
                if (s_log == null)
                {
                    s_log = log4net.LogManager.GetLogger("特殊退款单查询");
                }

                return s_log;
            }
        }
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			//验证权限
            //lbOperator.Text = Session["OperID"].ToString();           
              
              try
              {
                 /*
                  //测试用
                   if (!string.IsNullOrEmpty(RefundPublicFun.operatorName))
                  {
                      Session["uid"] = RefundPublicFun.operatorName;
                  }*/
                  //财务直接URL调用查询
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
                      log.ErrorFormat("特殊退款请求页面出错1： 退款号：{0} 程序出错原因:{1}", Request.QueryString["foldId"].ToString(), eSys.Message);                                 
                  }
                  else
                  {
                      log.ErrorFormat("特殊退款请求页面出错2：操作者：{0} 退款号：{1} 程序出错原因:{2}", Session["uid"].ToString(), Request.QueryString["foldId"].ToString(), eSys.Message);                                 
                  }
                  
              }
		}
		private void BindData()
		{
            string strMsg = null;
            if (Request.QueryString["foldId"] == null)
            {
                log.ErrorFormat("BindData： Request.QueryString[foldId] = null ");                                 
            }
            ViewState["foldId"] = Request.QueryString["foldId"].ToString();
            log.InfoFormat("ViewState[foldId]={0}", ViewState["foldId"].ToString());

            //主站那边收集的资料，根据状态，生成审批ID         
            DataSet ds = new RefundService().RequestDetailsData(ViewState["foldId"].ToString(), out strMsg);
            if (ds == null)
            {
                WebUtils.ShowMessage(this.Page, "查询退款详情失败" + strMsg);
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
                        ViewState["kfOperator"] = strAryOperator[0];    // 记录资料提交者  用户
                        kfOperator.Text = string.Format("资料提交者：{0} 提交时间:{1}", strAryOperator[0], strAryOperator[1]);
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

                //处理主站收集过资料并发起审批
                string strState = dr["Fstate"].ToString();
                string strFrom = dr["FStandby2"].ToString();
                string strHisCheckID = dr["FHisCheckID"].ToString();
                string strCheckId = dr["FcheckID"].ToString();
                if (strState.Trim() == "2" && strFrom.Trim() == m_nFrom.Trim() && string.IsNullOrEmpty(strCheckId))
                {
                    log.InfoFormat("进入主站发起单子：  退款号：{1} ", ViewState["foldId"].ToString());
                    //生成审批编号
                    ZWCheck_Service.Check_Service chkService = new ZWCheck_Service.Check_Service();
                    chkService.Finance_HeaderValue = RefundPublicFun.SetWebServiceHeader(this);

                    /*objectID组合规则 =   
                    目的：确保每次申请唯一*/
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

                //如果是从主站那边录入的信息
                if (strFrom.Trim() == m_nFrom.Trim())
                {
                    //拉图片cgi
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
                log.InfoFormat("显示存储路径图片：igCommitment = ：{0} igIdentity = ：{1} igBankWater = ：{2} igAccount = ：{3} strCheckId={4}", igCommitment.ImageUrl, igIdentity.ImageUrl, igBankWater.ImageUrl, igAccount.ImageUrl, strCheckId);                                                
                
                
                if (!string.IsNullOrEmpty(strCheckId.Trim()))
                {
                    log.InfoFormat("非空:strCheckId={0}", strCheckId);
                    ViewState["checkId"] = strCheckId.Trim();
                    ViewState["state"] = dr["Fstate"].ToString();
                    //当前审批记录
                    ShowCurCheckRecord();
                }
                else
                {                  
                    SetBtnEnabledState(false, 0);
                    lbKfCheckTime.Text = lbKfCheckName.Text = lbKfCheckReason.Text = null;
                    lbBgCheckTime.Text = lbBgCheckName.Text = lbBgCheckReason.Text = null;
                    lbFengCheckTime.Text = lbFengCheckName.Text = lbFengCheckReason.Text = null;
                    log.InfoFormat("为空:strCheckId={0}", strCheckId);
                }
                
                //历史审批记录
                ShowHistoryCheckRecord(strCheckId);
            }

            //后台数据
            if (!string.IsNullOrEmpty(lbUinID.Text))
            {
                /*step1:根据财付通订单号 求 财付通号
                  step2:财付通号 求 个人信息
                 */
                string msg = null;
                string strUid = new RefundService().GetBankCardBindInformation(lbUinID.Text, out msg);
                msg += "调用接口取得值是:"+strUid;
                log.InfoFormat("特殊退款后台数据：操作者：{0} 退款号：{1} 财付通订单:{2} 返回的结果信息:{3}", Session["uid"].ToString(), ViewState["foldId"].ToString(), lbUinID.Text, msg);
                Query_Service.Query_Service myService = new Query_Service.Query_Service();
                myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

                log.InfoFormat("操作者：{0} 头信息 SrcUrl={1},UserIP={2}, UserName={3},SessionID={4},SzKey={5},OperID={6},RightString={7}",
                    Session["uid"].ToString(), myService.Finance_HeaderValue.SrcUrl, myService.Finance_HeaderValue.UserIP, myService.Finance_HeaderValue.UserName,
                    myService.Finance_HeaderValue.SessionID, myService.Finance_HeaderValue.SzKey, myService.Finance_HeaderValue.OperID, myService.Finance_HeaderValue.RightString);  
                if (!string.IsNullOrEmpty(strUid))
                {
                    log.InfoFormat("求个人信息不为空：操作者：{0} ", Session["uid"].ToString());


                    DataSet tmpds = new AccountService().GetUserInfo(strUid.Trim(), 1, 1);
                    if (tmpds == null || tmpds.Tables.Count < 1 || tmpds.Tables[0].Rows.Count < 1)
                    {
                        msg += string.Format("根据财付通帐号：{0} 求个人信息为空:", strUid);
                        log.InfoFormat("求个人信息为空：操作者：{0} 退款号：{1} 财付通订单:{2} 返回的结果信息:{3}", Session["uid"].ToString(), ViewState["foldId"].ToString(), lbUinID.Text, msg);                 
                        return;
                    }
                    log.InfoFormat("最后得值为Ftruename={0},Fmobile={1},Fcreid={2}", tmpds.Tables[0].Rows[0]["Ftruename"].ToString(), tmpds.Tables[0].Rows[0]["Fmobile"].ToString(),classLibrary.setConfig.ConvertCreID(tmpds.Tables[0].Rows[0]["Fcreid"].ToString()));    
                    ltruename.Text = tmpds.Tables[0].Rows[0]["Ftruename"].ToString();    
                    lPhone.Text = tmpds.Tables[0].Rows[0]["Fmobile"].ToString();
                    lIdentity.Text = classLibrary.setConfig.ConvertCreID(tmpds.Tables[0].Rows[0]["Fcreid"].ToString());
                }
            }
        }
        //当前审批记录
        private void ShowCurCheckRecord()
        {
            log.InfoFormat("ShowCurCheckRecord进来了");
            if (ViewState["checkId"] == null || Session["uid"] == null || ViewState["foldId"] == null)
            {
                log.InfoFormat("当前审批ShowCurCheckRecord：ViewState[checkId] == null ||  Session[uid] == null|| ViewState[foldId] == null");
                return;
            }
            log.InfoFormat("开始调用权限判断接口 操作者：{0} 退款号：{1} checkId:{2}",Session["uid"].ToString(), ViewState["foldId"].ToString(),ViewState["checkId"].ToString());
            ZWCheck_Service.Check_Service chkService = new ZWCheck_Service.Check_Service();
            bool bState = false;
            try 
            {
                 bState = chkService.ValiCheckRight(Session["uid"].ToString(), ViewState["checkId"].ToString());
            }
            catch (Exception eSys)
            {
                log.ErrorFormat("权限判断异常：操作者：{0} 退款号：{1} 程序出错原因:{2}", Session["uid"].ToString(), ViewState["foldId"].ToString(), eSys.Message);                                 
            }
            SetBtnEnabledState(bState, Convert.ToInt32(ViewState["state"]));
            if (Session["uid"] != null && ViewState["foldId"] != null)
            {
                log.InfoFormat("权限判断：操作者：{0} 退款号：{1} 记录状态:{2} 权限判断返回结果:{3}", Session["uid"].ToString(), ViewState["foldId"].ToString(), Convert.ToInt32(ViewState["state"]), bState);                 
            }
           // chkService.Finance_HeaderValue = RefundPublicFun.SetWebServiceHeader(this);
           // log.InfoFormat("代理对象头对象具体信息值 UserIP:{0} UserName:{1} SzKey:{2} OperID:{3} RightString:{4}", chkService.Finance_HeaderValue.UserIP, chkService.Finance_HeaderValue.UserName, chkService.Finance_HeaderValue.SzKey, chkService.Finance_HeaderValue.OperID, chkService.Finance_HeaderValue.RightString);
            //显示当前审批记录与历史审批记录
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

        

        //历史审批记录
        private void ShowHistoryCheckRecord(string strCheckId)
        {
            string strHistory = null;
            if (ViewState["foldId"] == null)
            {
                return;
            }
            ZWCheck_Service.Check_Service chkService = new ZWCheck_Service.Check_Service();
            //财务转客服历史记录
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
                //财务审批结果与其它审批没有放在一起，需分别读取组合
                if (!string.IsNullOrEmpty(strZWCheckMemo))
                {
                    string[] arrZWCheckMemo = strZWCheckMemo.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int index = arrZWCheckMemo.Length-1; index >= 0; --index)
                    {
                        strHistory += string.Format("{0}\t{1}\t {2}", "财务审批", arrZWCheckMemo[index], m_nRefundReason) + "<br>";
                        if (arrHisCheckId != null && !string.IsNullOrEmpty(arrHisCheckId[index]))
                        {
                            ShowHistorySystle(arrHisCheckId[index], chkService,ref strHistory);
                        }
                    }                   
                }
            }
            //当前审批的历史数据
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
                        strHistory += "审批等级超出了范围" + dr["FLevel"].ToString();
                    }
                    strHistory += "<br>";
                }
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

        private void SetRefundCheckState(int nState,string strOldId)
        {
            try
            {
                new RefundService().SetRefundCheckState(nState, strOldId);
            }
            catch (Exception ex)
            {
                log.ErrorFormat("设置退款状态：操作者：{0} 退款号strOldId=：{1} 程序出错原因:{2}", Session["uid"].ToString(), strOldId, ex.Message); 
                throw new Exception(ex.Message);
            }
        }
        //通过
        protected void btnPass_Click(object sender, System.EventArgs e)
		{
            try
            {
                if (ViewState["foldId"] == null)
                {
                    WebUtils.ShowMessage(this.Page, "ViewState[foldId]为空。");
                    return;
                }
                if (ViewState["checkId"] == null)
                {
                    WebUtils.ShowMessage(this.Page, "checkId为空 null");
                    return;
                }
                string strCheckId = ViewState["checkId"].ToString();
                ZWCheck_Service.Check_Service chkService = new ZWCheck_Service.Check_Service();
                chkService.Finance_HeaderValue = RefundPublicFun.SetWebServiceHeader(this);
                
                if (!chkService.ValiCheckRight(Session["uid"].ToString(), strCheckId))
                {
                    log.InfoFormat("通过时权限判断没通过：操作者：{0} checkId：{1} 记录状态:{2} ", Session["uid"].ToString(), strCheckId, Convert.ToInt32(ViewState["state"]));                 
                    WebUtils.ShowMessage(this.Page, "审批权限不对！");
                    return;
                }           
                if (chkService.DoCheckNew(strCheckId, "同意", 0))
                {
                    int nState = Convert.ToInt32(ViewState["state"]) + 1;
                    SetRefundCheckState(nState, ViewState["foldId"].ToString());
                    SetBtnEnabledState(false, 0);
                    log.InfoFormat("审批通过：操作者：{0} checkId：{1} 记录状态:{2} ", Session["uid"].ToString(), strCheckId, nState); 
                    WebUtils.ShowMessage(this.Page, "审批已生效！");
                }
                else
                {
                    log.ErrorFormat("审批通过：操作者：{0} DoCheckNew返回结果为false", Session["uid"].ToString()); 
                    WebUtils.ShowMessage(this.Page, "同意审批结果返回false");
                }

            }
            catch (Exception ex)
            {
                log.ErrorFormat("审批通过：操作者：{0} 退款号：{1} 程序出错原因:{2}", Session["uid"].ToString(), ViewState["foldId"].ToString(), ex.Message); 
                //WebUtils.ShowMessage(this.Page, "审批失败！"+ex.Message);
            }

		}
        //拒绝
        protected void btnRefuse_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (ViewState["foldId"] == null)
                {
                    WebUtils.ShowMessage(this.Page, "ViewState[foldId]为空。");
                    return;
                }
                if (ViewState["checkId"] == null)
                {
                    WebUtils.ShowMessage(this.Page, "checkId为空。");
                    return;
                }
                if (string.IsNullOrEmpty(lbWriteReason.Text))
                {
                    WebUtils.ShowMessage(this.Page, "请选择拒绝原因或手动填写原因！");
                    return;
                }
                string strCheckId = ViewState["checkId"].ToString();
                ZWCheck_Service.Check_Service chkService = new ZWCheck_Service.Check_Service();
                chkService.Finance_HeaderValue = RefundPublicFun.SetWebServiceHeader(this);
                //Session["uid"].ToString()
                if (!chkService.ValiCheckRight(Session["uid"].ToString(), strCheckId))
                {
                    log.InfoFormat("通过时权限判断没通过：操作者：{0} checkId：{1} 记录状态:{2} ", Session["uid"].ToString(), strCheckId, Convert.ToInt32(ViewState["state"]));                 
                    WebUtils.ShowMessage(this.Page, "审批权限不对！");
                    return;
                }
                if (chkService.DoCheckNew(strCheckId, lbWriteReason.Text, 1))
                {
                    //审批重新开始
                    SetRefundCheckState(0, ViewState["foldId"].ToString());
                    SetBtnEnabledState(false, 0);
                    log.InfoFormat("审批拒绝成功：操作者：{0} checkId：{1} ", Session["uid"].ToString(), strCheckId); 
                    WebUtils.ShowMessage(this.Page, "审批已生效！");
                    RefusedSendEmail();
                }
                else
                {
                    log.ErrorFormat("审批拒绝：操作者：{0} DoCheckNew返回结果为false", Session["uid"].ToString()); 
                    WebUtils.ShowMessage(this.Page, "拒绝审批结果返回false");
                }

            }
            catch (Exception ex)
            {
                log.ErrorFormat("审批拒绝：操作者：{0} 退款号：{1} 程序出错原因:{2}", Session["uid"].ToString(), ViewState["foldId"].ToString(), ex.Message); 
               // WebUtils.ShowMessage(this.Page, "审批失败！" + ex.Message); 
            }

        }
        //转向财务
        protected void btnTransferCW_Click(object sender, System.EventArgs e)
        {
            if (ViewState["foldId"] == null )
            {
                WebUtils.ShowMessage(this.Page, "foldId为空。");
                return;
            }
            if (ViewState["checkId"] == null)
            {
                WebUtils.ShowMessage(this.Page, "checkId为空。");
                return;
            }

            string strCheckId = ViewState["checkId"].ToString();
            ZWCheck_Service.Check_Service chkService = new ZWCheck_Service.Check_Service();
            chkService.Finance_HeaderValue = RefundPublicFun.SetWebServiceHeader(this);
            
            if (!chkService.ValiCheckRight(Session["uid"].ToString(), strCheckId))
            {
                log.InfoFormat("通转向财务时权限判断没通过：操作者：{0} checkId：{1} ", Session["uid"].ToString(), strCheckId);                 
                WebUtils.ShowMessage(this.Page, "审批权限不对！");
                return;
            }
            if (!chkService.DoCheckNew(strCheckId, "同意并转向财务处理", 0))
            {
                log.ErrorFormat("转向财务审批通过：操作者：{0} DoCheckNew返回结果为false", Session["uid"].ToString());
                WebUtils.ShowMessage(this.Page, "同意审批结果返回false");
                return;
            }

            SetRefundCheckState(5, ViewState["foldId"].ToString());
            WebUtils.ShowMessage(this.Page, "转财务处理成功。");
            SetBtnEnabledState(false, 0);
            log.InfoFormat("特殊退款转向财务：操作者：{0} 将 退款号为:{1}转向财务处理", Session["uid"].ToString(), ViewState["foldId"].ToString());

 
        }
        private void SetBtnEnabledState(bool bState,int nState)
        {
            //只有在审批权限及当前记录允许审批时，才可使用按钮
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
                    WebUtils.ShowMessage(this.Page, "修改成功！");
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "修改失败" + strMsg);
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, PublicRes.GetErrorMsg("修改出错" + ex.Message));
            }
        }

        /// <summary>
        /// 拒绝后发送邮件 给 资料提交者
        /// </summary>
        private void RefusedSendEmail()
        {
            try
            {
                var uin = ViewState["kfOperator"] as string;
                if (uin != null)
                {
                    var listid = lbUinID.Text;
                    var mailBody = uin + "：\r\n" +
                        "您提交的特殊退款订单 " + listid + " 审批失败，请您及时关注！谢谢！";
                    TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendInternalMail(uin, "", "特殊退款订单审批失败通知", mailBody);
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("审批拒绝：操作者：{0} 退款号：{1} 发送邮件给资料提交者发送失败:{2}", Session["uid"].ToString(), ViewState["foldId"].ToString(), ex.Message); 
            }
        }
        /*
        //作废
        protected void btnTransferInvalid_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["foldId"] == null)
                {
                    WebUtils.ShowMessage(this.Page, "ViewState[foldId]为空。");
                    return;
                }
                if (ViewState["checkId"] == null)
                {
                    WebUtils.ShowMessage(this.Page, "checkId为空。");
                    return;
                }
                string strCheckId = ViewState["checkId"].ToString();
                ZWCheck_Service.Check_Service chkService = new ZWCheck_Service.Check_Service();
                chkService.Finance_HeaderValue = RefundPublicFun.SetWebServiceHeader(this);
                //Session["uid"].ToString()
                if (!chkService.ValiCheckRight(Session["uid"].ToString(), strCheckId))
                {
                    log.InfoFormat("作废时权限判断没通过：操作者：{0} checkId：{1} ", Session["uid"].ToString(), strCheckId);  
                    WebUtils.ShowMessage(this.Page, "审批权限不对！");
                    return;
                }
                if (chkService.DoCheckNew(strCheckId, "作废此单，无法处理", 1))
                {
                    //转向无法处理。
                    new RefundService().SetRefundCheckState(7, ViewState["foldId"].ToString());
                    SetBtnEnabledState(false, 0);
                    WebUtils.ShowMessage(this.Page, "作废此单已生效！");
                    log.InfoFormat("退款作废：操作者：{0} 将 退款号为:{1}转向无法处理", Session["uid"].ToString(), ViewState["foldId"].ToString());
                }
                else
                {
                    log.ErrorFormat("退款作废：操作者：{0} DoCheckNew返回结果为false", Session["uid"].ToString());
                    WebUtils.ShowMessage(this.Page, "拒绝审批结果返回false");
                }

            }
            catch (Exception ex)
            {
                log.ErrorFormat("退款作废：操作者：{0} 退款号：{1} 程序出错原因:{2}", Session["uid"].ToString(), ViewState["foldId"].ToString(), ex.Message);
                
            }

        }

        //还原
        protected void btnInitState_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["uid"].ToString().Trim() != RefundPublicFun.OPERATOR.Trim())
                {
                    WebUtils.ShowMessage(this.Page, "没有权限操作此项功能");
                    return;
                }

                if (ViewState["foldId"] == null)
                {
                    WebUtils.ShowMessage(this.Page, "ViewState[foldId]为空。");
                    return;
                }
                string strHisCheckID = "";
                if (string.IsNullOrEmpty(new RefundService().QueryAbnormalRefundCheckID(ViewState["foldId"].ToString(),ref strHisCheckID)))
                {
                    new RefundService().SetRefundCheckState(0, ViewState["foldId"].ToString());
                    WebUtils.ShowMessage(this.Page, "初始化成功。");
                }
                else
                {
                    WebUtils.ShowMessage(this.Page, "checkID不为空,不允许初始化操作。");
                    return;
                }

            }
            catch (Exception ex)
            {
                log.ErrorFormat("特殊退款初始化还原操作:操作者{0} 退款号：{1} 程序出错原因:{2}", Session["uid"].ToString(), ViewState["foldId"].ToString(), ex.Message);

            }

        }
         * */

	}
}
