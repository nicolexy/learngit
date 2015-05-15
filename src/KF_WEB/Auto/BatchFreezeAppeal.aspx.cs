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
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.Common;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using Apollo = CFT.Apollo;
using System.Threading;
using CFT.CSOMS.BLL.UserAppealModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.Auto
{
    /// <summary>
    /// CFTAppeal ��ժҪ˵����
    /// </summary>
    public partial class BatchFreezeAppeal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            string startTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00");
            string endTime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");

            if (Request.QueryString["proType"] == null)
            {
                this.showtxt.Text = "ȱ��proType������";
                return;
            }

            #region Ĭ�ϲ�ѯʱ�������
            string proType = Request.QueryString["proType"].ToString().Trim();
            if (proType == "1")
            {
                startTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00");
                endTime = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
            }
            else if (proType == "2")
            {
                startTime = DateTime.Now.AddDays(-60).ToString("yyyy-MM-dd 00:00:00");
                endTime = DateTime.Now.AddDays(-60).ToString("yyyy-MM-dd 23:59:59");
            }
            else
            {
                this.showtxt.Text = "proType��������ȷ��";
                return;
            }
            #endregion

            #region ��ѯʱ��У��
            if (Request.QueryString["startDate"] != null && Request.QueryString["endDate"] != null)
            {

                DateTime startdate = new DateTime();
                DateTime enddate = new DateTime();
                try
                {
                    startdate = DateTime.Parse(Request.QueryString["startDate"].ToString());
                    enddate = DateTime.Parse(Request.QueryString["endDate"].ToString());
                    startTime = DateTime.Parse(startdate.ToString("yyyy-MM-dd 00:00:00")).ToString();
                    endTime = DateTime.Parse(enddate.ToString("yyyy-MM-dd 23:59:59")).ToString();
                }
                catch
                {
                    this.showtxt.Text = "������������";
                    return;
                }

                if (startdate.CompareTo(enddate) > 0)
                {
                    this.showtxt.Text = "��ֹ����С����ʼ���ڣ�";
                    return;
                }

                if (startdate.Month != enddate.Month)
                {
                    this.showtxt.Text = "��ʼ������������ڲ���ͬһ���£�";
                    return;
                }

                //��������δ�������ϵĵ��������ᵥֻ�����60��֮ǰ�Ľᵥ
                if (proType == "2")
                {
                    if (startdate.AddDays(60) > DateTime.Now || enddate.AddDays(60) > DateTime.Now)
                    {
                        this.showtxt.Text = "���ܽᵥ60�����ڵĵ�����ѡ��" + DateTime.Now.AddDays(-60).ToString("yyyy-MM-dd") + "֮ǰ��ʱ��Σ����������ڣ�";
                        return;
                    }
                }
            }
            #endregion

             ViewState["startTime"]  =startTime;
             ViewState["endTime"] = endTime;
             ViewState["proType"] = proType;

             Thread t = new Thread(HandleBatchFinishApeal);
             t.Start();

            this.showtxt.Text = "��̨�����ᵥ�У����Ժ󵽡��������ߴ����в�ѯ�����";
        }

        private void HandleBatchFinishApeal()
        {
            UserAppealService userAppealService = new UserAppealService();
            userAppealService.BatchFinishAppeal(ViewState["startTime"].ToString(), ViewState["endTime"].ToString(), ViewState["proType"].ToString());
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
    }
}
