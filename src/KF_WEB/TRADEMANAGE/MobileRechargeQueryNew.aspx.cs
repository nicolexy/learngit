using System;
using CFT.CSOMS.BLL.MobileModule;
using CFT.CSOMS.COMMLIB;
using Tencent.DotNet.Common.UI;
using System.Web.Services.Protocols;
using System.Configuration;
using CFT.Apollo.Logging;



namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
	/// <summary>
	/// B2CReturnQuery ��ժҪ˵����
	/// </summary>
    public partial class MobileRechargeQueryNew : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["uid"] != null)
            {
                this.Label_uid.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                int operid = Int32.Parse(Session["OperID"].ToString());

                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this))
                {
                    WebUtils.ShowMessageAndRedirect(this.Page, "��������ʵ�Ȩ�޲��㣬�޷����ʸ�ҳ�档����ϵ������������ӦȨ�޼��ɷ��ʡ�", "../login.aspx");

                }
            }
        }

        protected override void OnInit(EventArgs e)
        {

            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);

        }

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindData(e.NewPageIndex);
        }

        public void Button2_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.pager.CurrentPageIndex = 0;
                BindData(1);
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

        private void BindData(int index)
        {
            int pageSize = 20;
            this.dataGridMobileRecharge.PageSize = pageSize;
            this.pager.PageSize = pageSize;

            index = index < 1 ? 1 : index;
            index = index - 1;
            int start = pageSize * index;

            string msg = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(this.txtUserQQ.Text.Trim()) && string.IsNullOrEmpty(this.txtOrderId.Text.Trim()))
                {
                    WebUtils.ShowMessage(this.Page, "����������ʺ���Ϣ��֧�������Ų�ѯ��");
                    return;
                }

                DateTime begindate;
                DateTime enddate;
                int beginDateNum = 0;
                int endDateNum = 0;

                if (!string.IsNullOrEmpty(this.txtBeginDate.Text.Trim()))
                {
                    if (DateTime.TryParse(this.txtBeginDate.Text.Trim(), out begindate))
                    {
                        beginDateNum = CommUtil.ConvertDateTimeInt(begindate);
                    }
                    else
                    {
                        WebUtils.ShowMessage(this.Page, "��������ȷ��ʼ���ڸ�ʽ��");
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(this.txtEndDate.Text.Trim()) && !DateTime.TryParse(this.txtEndDate.Text.Trim(), out enddate))
                {
                    if (DateTime.TryParse(this.txtBeginDate.Text.Trim(), out enddate))
                    {
                        endDateNum = CommUtil.ConvertDateTimeInt(enddate);
                    }
                    else
                    {
                        WebUtils.ShowMessage(this.Page, "��������ȷ�������ڸ�ʽ��");
                        return;
                    }

                }


                string reqServerUrl = ConfigurationManager.AppSettings["MobileRechageQueryUrl"] ?? "http://chong.cm.com/index.php/api/recovercarddeal/getDealList?";
                string auth_cm_com_session_key = string.Empty;
                if (Session["SzKey"] != null)
                {
                    auth_cm_com_session_key = Session["SzKey"].ToString();
                }

                LogHelper.LogInfo("TENCENT.OSS.CFT.KF.KF_Web.TradeManage.MobileRechargeQueryNew  private void BindData(int index)  ��ȡ����Cookie:auth_cm_com_session_key=" + auth_cm_com_session_key);
                //��ȡ������
                MobileService mobileService = new MobileService();
                var resultData = mobileService.GetRechargeList(reqServerUrl, this.txtUserQQ.Text.Trim(), this.txtOrderId.Text.Trim(), beginDateNum, endDateNum, Session["uid"].ToString(), index, pageSize, auth_cm_com_session_key, ref msg);

                if (resultData != null && resultData.retCode == 0 && resultData.data != null && resultData.data.rows != null && resultData.data.rows.Count > 0)
                {
                    this.pager.RecordCount = resultData.total;

                    this.dataGridMobileRecharge.DataSource = resultData.data.rows;
                    this.dataGridMobileRecharge.DataBind();
                }
                else
                {
                    this.pager.RecordCount = 0;

                    this.dataGridMobileRecharge.DataSource = null;
                    this.dataGridMobileRecharge.DataBind();
                }
            }
            catch (Exception err)
            {
                LogHelper.LogInfo(string.Format("TENCENT.OSS.CFT.KF.KF_Web.TradeManage.MobileRechargeQueryNew  private void BindData(int index),  retmsg={0}�� �����쳣���飺{1}", msg, err.ToString()));

                msg = string.IsNullOrEmpty(msg) ? err.Message : msg;
                WebUtils.ShowMessage(this.Page, msg);
                return;
            }
        }
    }
}
