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
    /// CFTAppeal 的摘要说明。
    /// </summary>
    public partial class BatchFreezeAppeal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            string startTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00");
            string endTime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");

            if (Request.QueryString["proType"] == null)
            {
                this.showtxt.Text = "缺少proType参数！";
                return;
            }

            #region 默认查询时间段设置
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
                this.showtxt.Text = "proType参数不正确！";
                return;
            }
            #endregion

            #region 查询时间校验
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
                    this.showtxt.Text = "日期输入有误！";
                    return;
                }

                if (startdate.CompareTo(enddate) > 0)
                {
                    this.showtxt.Text = "终止日期小于起始日期！";
                    return;
                }

                if (startdate.Month != enddate.Month)
                {
                    this.showtxt.Text = "开始日期与结束日期不在同一个月！";
                    return;
                }

                //特殊找密未补充资料的单，批量结单只允许对60天之前的结单
                if (proType == "2")
                {
                    if (startdate.AddDays(60) > DateTime.Now || enddate.AddDays(60) > DateTime.Now)
                    {
                        this.showtxt.Text = "不能结单60天以内的单！请选择" + DateTime.Now.AddDays(-60).ToString("yyyy-MM-dd") + "之前的时间段（包括该日期）";
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

            this.showtxt.Text = "后台批量结单中，请稍后到“特殊申诉处理”中查询结果！";
        }

        private void HandleBatchFinishApeal()
        {
            UserAppealService userAppealService = new UserAppealService();
            userAppealService.BatchFinishAppeal(ViewState["startTime"].ToString(), ViewState["endTime"].ToString(), ViewState["proType"].ToString());
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
    }
}
