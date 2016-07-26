using System;
using System.Data;
using System.Linq;
using System.Web.Services.Protocols;
using System.Web.UI.WebControls;
using CFT.CSOMS.BLL.WechatPay;
using CFT.CSOMS.COMMLIB;
using Tencent.DotNet.Common.UI;
using CFT.CSOMS.BLL.HandQModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.HandQBusiness
{
    public partial class FindHandQRedPacket : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        private const int m_nPageSize = 10;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                textBoxBeginDate.Text = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                textBoxEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.receivePager.RecordCount = 1000;
                this.sendListPager.RecordCount = 1000;
                this.redPacketHBDetailPager.RecordCount = 1000;

                BindHandQRecvInfor(1, m_nPageSize);
                BindHandQSendInfor(1, m_nPageSize);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        private void showMsg(string msg)
        {
            Response.Write("<script language=javascript>alert('" + msg + "')</script>");
        }

        private void BindHandQRecvInfor(int pageIndex, int pageSize)
        {
            if (string.IsNullOrEmpty(txtUin.Text))
            {
                showMsg("财付通帐号不能为空");
                return;
            }

            string strBeginDate = null;
            string strEndDate = null;
            if (!string.IsNullOrEmpty(textBoxBeginDate.Text))
            {
                DateTime begindate = DateTime.Parse(textBoxBeginDate.Text.Trim());
                strBeginDate = begindate.ToString("yyyy-MM-dd");
            }
            if (!string.IsNullOrEmpty(textBoxEndDate.Text))
            {
                DateTime enddate = DateTime.Parse(textBoxEndDate.Text.Trim());
                strEndDate = enddate.AddDays(1).ToString("yyyy-MM-dd");
            }
            int max = pageSize;
            int start = max * (pageIndex - 1);
            if (start < 0)
            {
                start = 1;
            }
            receivePager.CurrentPageIndex = pageIndex;
            gvReceiveHQList.DataSource = null;
            gvReceiveHQList.DataBind();
            string strOutMsg = "收红信息反馈";
            try
            {
                DataSet ds = new HandQService().RecvRedPacketInfo(txtUin.Text, "", strBeginDate, strEndDate, start, max, out strOutMsg);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    gvReceiveHQList.DataSource = ds.Tables[0].DefaultView;
                    gvReceiveHQList.DataBind();
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(ex.Message.ToString()));
                log4net.ILog log = log4net.LogManager.GetLogger("BindHandQRecvInfor");
                log.ErrorFormat(string.Format("strOutMsg={0},ex.Message={1}", strOutMsg, ex.Message));
            }
        }

        private void BindHandQSendInfor(int pageIndex, int pageSize)
        {
            if (string.IsNullOrEmpty(txtUin.Text))
            {
                showMsg("财付通帐号不能为空");
                return;
            }

            string strBeginDate = null;
            string strEndDate = null;
            if (!string.IsNullOrEmpty(textBoxBeginDate.Text))
            {
                DateTime begindate = DateTime.Parse(textBoxBeginDate.Text.Trim());
                strBeginDate = begindate.ToString("yyyy-MM-dd");
            }
            if (!string.IsNullOrEmpty(textBoxEndDate.Text))
            {
                DateTime enddate = DateTime.Parse(textBoxEndDate.Text.Trim());
                strEndDate = enddate.AddDays(1).ToString("yyyy-MM-dd");
            }
            int max = pageSize;
            int start = max * (pageIndex - 1);
            if (start < 0)
            {
                start = 1;
            }
            sendListPager.CurrentPageIndex = pageIndex;
            gvSendHQList.DataSource = null;
            gvSendHQList.DataBind();
            string strOutMsg = "发红包信息反馈";
            try
            {
                DataSet ds = new HandQService().SendRedPacketInfo(txtUin.Text, "", strBeginDate, strEndDate, start, max, out strOutMsg);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    gvSendHQList.DataSource = ds.Tables[0].DefaultView;
                    gvSendHQList.DataBind();
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(ex.Message.ToString()));
                log4net.ILog log = log4net.LogManager.GetLogger("BindHandQSendInfor");
                log.ErrorFormat(string.Format("strOutMsg={0},ex.Message={1}", strOutMsg, ex.Message));
            }

        }
        //strSendList:红包总单号
        public void BindHandQDetail(string strSendList, int pageIndex, int pageSize)
        {
            string strOutMsg = null;
            try
            {
                string ip = Request.UserHostAddress.ToString();
                if (ip == "::1")
                {
                    ip = "127.0.0.1";
                }
                gvHQRedPacketDetail.DataSource = null;
                gvHQRedPacketDetail.DataBind();
                this.redPacketHBDetailPager.CurrentPageIndex = pageIndex;
                int max = pageSize;
                int start = max * (pageIndex - 1);
                if (start < 0)
                {
                    start = 1;
                }
                int nType = 1;

                DataSet ds = new HandQService().RequestHandQDetail(strSendList, nType, start, max, out strOutMsg);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    gvHQRedPacketDetail.DataSource = ds.Tables[0].DefaultView;
                    gvHQRedPacketDetail.DataBind();
                }
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(ex.Message.ToString()));
                log4net.ILog log = log4net.LogManager.GetLogger("BindHandQDetail");
                log.ErrorFormat(string.Format("strOutMsg={0},strSendList={1},eSys.Message={1}", strOutMsg, strSendList, ex.Message));
            }
        }

        protected void sendHQListPager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                int currentPage = e.NewPageIndex;
                BindHandQSendInfor(currentPage, m_nPageSize);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        protected void receiveHQPager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                int currentPage = e.NewPageIndex;
                BindHandQRecvInfor(currentPage, m_nPageSize);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        protected void gvReceiveHQList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewDetail")
                {
                    var commandArgs = e.CommandArgument.ToString().Split(',');

                    if (commandArgs.Count() != 2)
                    {
                        throw new Exception("详情参数错误");
                    }
                    hfSendListId.Value = commandArgs[0];
                    hfCreateTime.Value = commandArgs[1];

                    //112417524760201501059002423553 红包有人接 的测试数据
                    BindHandQDetail(commandArgs[0], 1, m_nPageSize);
                    //测试数据。红包没有人接数据
                    //BindHandQDetail("102417524760201501079002423813");
                }

            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        protected void gvSendHQList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewDetail")
                {
                    var commandArgs = e.CommandArgument.ToString().Split(',');

                    if (commandArgs.Count() != 2)
                    {
                        throw new Exception("详情参数错误");
                    }

                    hfSendListId.Value = commandArgs[0];
                    hfCreateTime.Value = commandArgs[1];

                    BindHandQDetail(commandArgs[0], 1, m_nPageSize);
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        protected void redPacketHBDetailPager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                var sendListId = hfSendListId.Value;
                int currentPage = e.NewPageIndex;
                BindHandQDetail(sendListId, currentPage, m_nPageSize);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

    }
}