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
    public partial class FindHandQRedPacket : System.Web.UI.Page
    {
        private const int m_nPageSize = 10;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                textBoxBeginDate.Text = DateTime.Now.AddDays(-7).ToString("yyyy年MM月dd日");
                textBoxEndDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
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
                strEndDate = enddate.ToString("yyyy-MM-dd");
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
                DataSet ds = new HandQService().QueryHandQInfor(txtUin.Text, "", strBeginDate, strEndDate, "2", start, max, out strOutMsg);
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].Columns.Add("amount_text", typeof(string));
                        ds.Tables[0].Columns.Add("Title", typeof(string));

                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            item["amount_text"] = classLibrary.setConfig.FenToYuan(item["amount"].ToString());
                            item["Title"] = string.Format("{0}发出的红包", item["send_name"].ToString());
                        }
                        gvReceiveHQList.DataSource = ds.Tables[0].DefaultView;
                        gvReceiveHQList.DataBind();
                    }
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
                strEndDate = enddate.ToString("yyyy-MM-dd");
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
                DataSet ds = new HandQService().QueryHandQInfor(txtUin.Text, "", strBeginDate, strEndDate, "1", start, max, out strOutMsg);
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        ds.Tables[0].Columns.Add("amount_text", typeof(string));
                        ds.Tables[0].Columns.Add("state_text", typeof(string));
                        ds.Tables[0].Columns.Add("Title", typeof(string));
                        ds.Tables[0].Columns.Add("summary", typeof(string));
                        ds.Tables[0].Columns.Add("refund", typeof(string));
                        ds.Tables[0].Columns.Add("send_listidex", typeof(string));

                        foreach (DataRow item in ds.Tables[0].Rows)
                        {

                            item["summary"] = string.Format("{0}/{1},总计{2}元",
                                               item["recv_num"].ToString(),
                                               item["total_num"].ToString(),
                                               classLibrary.setConfig.FenToYuan(item["recv_amount"].ToString()));
                            item["amount_text"] = classLibrary.setConfig.FenToYuan(item["total_amount"].ToString());
                            item["Title"] = string.Format("{0}发出的红包", item["send_name"].ToString());
                            item["refund"] = classLibrary.setConfig.FenToYuan((int.Parse(item["total_amount"].ToString()) - int.Parse(item["recv_amount"].ToString())).ToString());
                            //去掉前二位，听说是路由产生的
                            string strSendList = item["send_listid"].ToString();
                            item["send_listidex"] = strSendList.Substring(2);
                            switch (item["state"].ToString())
                            {

                                case "1":
                                    item["state_text"] = "支付成功";
                                    break;
                                case "2":
                                    item["state_text"] = "全部领取";
                                    break;
                                case "3":
                                    item["state_text"] = "已过期";
                                    break;
                                case "4":
                                    item["state_text"] = "过期退款";
                                    break;
                                default:
                                    item["state_text"] = "未知" + item["state"].ToString();
                                    break;
                            }
                        }

                        gvSendHQList.DataSource = ds.Tables[0].DefaultView;
                        gvSendHQList.DataBind();
                    }

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
        public void BindHandQDetail(string strSendList,int pageIndex, int pageSize)
        
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
               
                DataSet ds = new HandQService().RequestHandQDetail(strSendList,nType,start,max, out strOutMsg);
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].Columns.Add("amount_text", typeof(string));
                        ds.Tables[0].Columns.Add("Title", typeof(string));

                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
    
                            item["amount_text"] = string.IsNullOrEmpty(item["amount"].ToString())? "": classLibrary.setConfig.FenToYuan(item["amount"].ToString());
                            item["Title"] = string.IsNullOrEmpty(item["send_name"].ToString())?"":string.Format("发给{0}红包", item["send_name"].ToString());
                        }

                        gvHQRedPacketDetail.DataSource = ds.Tables[0].DefaultView;
                        gvHQRedPacketDetail.DataBind();
                    }                   
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
               // FetchInput();
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
                 //   DateTime createTime = DateTime.Now;

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
                  //  DateTime createTime = DateTime.Now;

                    var commandArgs = e.CommandArgument.ToString().Split(',');

                    if (commandArgs.Count() != 2)
                    {
                        throw new Exception("详情参数错误");
                    }

                    hfSendListId.Value = commandArgs[0];
                    hfCreateTime.Value = commandArgs[1];

                    BindHandQDetail(commandArgs[0], 1, m_nPageSize);
                  //  DateTime.TryParse(commandArgs[1], out createTime);

                   // BindRedPacketDetailList(commandArgs[0],  1, 10);
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
                //DateTime createTime = DateTime.Now;
                //DateTime.TryParse(hfCreateTime.Value, out createTime);
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