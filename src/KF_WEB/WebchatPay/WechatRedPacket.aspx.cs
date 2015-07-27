using System;
using System.Data;
using System.Linq;
using System.Web.Services.Protocols;
using System.Web.UI.WebControls;
using CFT.CSOMS.BLL.WechatPay;
using CFT.CSOMS.COMMLIB;
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.WebchatPay
{
    public partial class WechatRedPacket : System.Web.UI.Page
    {
        DateTime beginTime, endTime;
        string wechatName, hbUin, payListId;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TextBoxBeginDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy年MM月dd日");
                TextBoxEndDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
            }
        }


        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                #region 值处理
                this.receivePager.RecordCount = 1000;
                this.sendListPager.RecordCount = 1000;
                // this.redPacketDetailPager.RecordCount = 1000;
                wechatName = txtWechatName.Text.Trim();
                if (string.IsNullOrEmpty(wechatName))
                {
                    throw new Exception("微信号输入有误");
                }
                try
                {
                    beginTime = DateTime.Parse(TextBoxBeginDate.Text.Trim());
                    endTime = DateTime.Parse(TextBoxEndDate.Text.Trim());
                }
                catch
                {
                    throw new Exception("查询日期输入有误");
                }
                hbUin = WeChatHelper.GetHBUINFromWeChatName(wechatName);
                //#if DEBUG
                //hbUin = "oE15nt-ddlcBCbICy4VJcRXFG20Y@hb.tenpay.com";
                //#endif
                if (string.IsNullOrEmpty(hbUin))
                    throw new Exception("查询微信号对应的红包财付通账户失败");
                hfHBUin.Value = hbUin;
                #endregion
                try
                {
                    BindBasicInfo(hbUin);
                }
                catch { }//防止不能查询
                hbUin = hbUin.Replace("@hb.tenpay.com", "");
                ViewState["hbUin"] = hbUin;
                BindReceiveList(hbUin, 1, 10);
                BindSendList(hbUin, 1, 10);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        private void BindBasicInfo(string hbUin)
        {
            lblHongbaoUin.Text = hbUin;

            var qs = new Query_Service.Query_Service();
            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
            var ds = qs.GetUserAccount(hbUin, 1, 0, 20);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                lblBanlance.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fbalance"].ToString());
                lblFreezen.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fcon"].ToString());
            }
        }
        //绑定接受红包
        private void BindReceiveList(string hbUin, int pageIndex, int pageSize)
        {
            try
            {
                //var qs = new Query_Service.Query_Service();

                //var dsReceiveList = qs.GetReceiveRedPacketList(hbUin, beginTime, endTime, pageIndex * pageSize, pageSize);
                int max = pageSize;
                int start = max * (pageIndex - 1);
                receivePager.CurrentPageIndex = pageIndex;
                //string ip = Request.UserHostAddress.ToString();
                //if (ip == "::1")
                //    ip = "127.0.0.1";

                var dsReceiveList = new WechatPayService().QueryUserReceiveList(hbUin, beginTime, endTime, start, max);

                if (dsReceiveList != null && dsReceiveList.Tables.Count > 0)
                {
                    dsReceiveList.Tables[0].Columns.Add("Amount_text", typeof(string));
                    dsReceiveList.Tables[0].Columns.Add("Title", typeof(string));

                    foreach (DataRow item in dsReceiveList.Tables[0].Rows)
                    {
                        item["Amount_text"] = classLibrary.setConfig.FenToYuan(item["Amount"].ToString());
                        item["Title"] = string.Format("{0}发的红包", item["SendName"].ToString());
                    }
                    gvReceiveList.DataSource = dsReceiveList.Tables[0].DefaultView;
                }
                else
                {
                    gvReceiveList.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                gvReceiveList.DataSource = null;
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(ex.Message.ToString()));
            }
            gvReceiveList.DataBind();
        }
        //绑定发送红包
        private void BindSendList(string hbUin, int pageIndex, int pageSize)
        {
            try
            {
                //var qs = new Query_Service.Query_Service();

                //var dsSendList = qs.GetSendRedPacketList(hbUin, beginTime, endTime, pageIndex * pageSize, pageSize, payListId);

                int max = pageSize;
                int start = max * (pageIndex - 1);
                sendListPager.CurrentPageIndex = pageIndex;

                //string ip = Request.UserHostAddress.ToString();
                //if (ip == "::1")
                //    ip = "127.0.0.1";

                var dsSendList = new WechatPayService().QueryUserSendList(hbUin, beginTime, endTime, start, max);


                if (dsSendList != null && dsSendList.Tables.Count > 0)
                {
                    dsSendList.Tables[0].Columns.Add("Summary", typeof(string));
                    dsSendList.Tables[0].Columns.Add("State_text", typeof(string));
                    dsSendList.Tables[0].Columns.Add("Refund", typeof(string));
                    dsSendList.Tables[0].Columns.Add("TotalAmount_text", typeof(string));

                    foreach (DataRow item in dsSendList.Tables[0].Rows)
                    {
                        item["Summary"] = string.Format("{0}/{1},总计{2}元",
                                                        item["ReceivedNum"].ToString(),
                                                        item["TotalNum"].ToString(),
                                                        classLibrary.setConfig.FenToYuan(item["ReceivedAmount"].ToString()));

                        double refundAmount = double.Parse(item["TotalAmount"].ToString()) - double.Parse(item["ReceivedAmount"].ToString());

                        item["Refund"] = classLibrary.setConfig.FenToYuan(refundAmount);
                        item["TotalAmount_text"] = classLibrary.setConfig.FenToYuan(item["TotalAmount"].ToString());

                        switch (item["State"].ToString().Trim())
                        {
                            case "PREPAY": item["State_text"] = "等待支付"; break;              //1
                            case "PAYOK": item["State_text"] = "支付完成"; break;               //2
                            case "PARTRECEIVE": item["State_text"] = "部分领取"; break;         //3
                            case "ALLRECEIVE": item["State_text"] = "全部领取"; break;          //4
                            case "OVERTIMEREFUNDED": item["State_text"] = "过期退回"; break;    //5
                            case "EXCEPTIONREFUNDED": item["State_text"] = "异常退款"; break;   //6
                            case "REFUNDING": item["State_text"] = "退款中"; break;             //7
                            default: item["State_text"] = "未知" + item["State"].ToString(); break;
                        }
                    }
                    gvSendList.DataSource = dsSendList.Tables[0].DefaultView;
                }
                else
                {
                    gvSendList.DataSource = null;
                }
                this.sendListPager.RecordCount = 1000;
            }
            catch (Exception ex)
            {
                gvSendList.DataSource = null;
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(ex.Message.ToString()));
            }
            gvSendList.DataBind();
        }
        // 绑定红包详细 ,receive_id  如果不等于null 就表示是接受红包详情
        private void BindRedPacketDetailList(string sendListId, string receive_id = null)
        {
            //var qs = new Query_Service.Query_Service();

            //var dsDetailList = qs.GetRedPacketDetailList(sendListId, createTime, pageIndex * pageSize, pageSize);
            // this.redPacketDetailPager.CurrentPageIndex = pageIndex;
            //int max = pageSize;
            //int start = max * (pageIndex - 1);
            //string ip = Request.UserHostAddress.ToString();
            //if (ip == "::1")
            //    ip = "127.0.0.1";

            var bll = new WechatPayService();
            var dsDetailList = receive_id == null ? bll.QueryDetail(sendListId, 0) : bll.QueryReceiveHBInfoById(sendListId, receive_id, 1);

            if (dsDetailList != null && dsDetailList.Tables.Count > 0 && dsDetailList.Tables[0].Rows.Count > 0)
            {
                dsDetailList.Tables[0].Columns.Add("Amount_text", typeof(string));
                dsDetailList.Tables[0].Columns.Add("ReceiveOpenid_text", typeof(string));
                dsDetailList.Tables[0].Columns.Add("SendOpenid_text", typeof(string));
                //红包详情中Fsend_openid都一样
                string send_openid_tmp = string.Format("{0}@wx.tenpay.com", WeChatHelper.GetAcctIdFromOpenId(dsDetailList.Tables[0].Rows[0]["SendOpenid"].ToString()));
                foreach (DataRow item in dsDetailList.Tables[0].Rows)
                {
                    item["Amount_text"] = classLibrary.setConfig.FenToYuan(item["Amount"].ToString());
                    //  item["Freceive_openid_text"] = item["Freceive_openid"].ToString() + "@hb.tenpay.com";
                    //本来展示红包账号，改为展示微信账号
                    item["ReceiveOpenid_text"] = string.Format("{0}@wx.tenpay.com", WeChatHelper.GetAcctIdFromOpenId(item["ReceiveOpenid"].ToString()));
                    item["SendOpenid_text"] = send_openid_tmp;
                };
                gvRedPacketDetail.Columns[1].Visible = receive_id == null;  //订单号在接受者角度是没有的
                gvRedPacketDetail.DataSource = dsDetailList.Tables[0].DefaultView;
                gvRedPacketDetail.DataBind();
            }


        }
        //发送红包分页
        protected void sendListPager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                //  FetchInput();
                int currentPage = e.NewPageIndex;

                BindSendList(ViewState["hbUin"].ToString(), currentPage, 10);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }
        //接受红包分页
        protected void receivePager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                // FetchInput();
                int currentPage = e.NewPageIndex;

                BindReceiveList(ViewState["hbUin"].ToString(), currentPage, 10);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }
        //接受红包详细
        protected void gvReceiveList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewDetail")
                {
                    //   DateTime createTime = DateTime.Now;

                    var commandArgs = e.CommandArgument.ToString().Split(',');

                    //if (commandArgs.Count() != 2)
                    //{
                    //    throw new Exception("详情参数错误");
                    //}

                    hfSendListId.Value = commandArgs[0];
                    hfCreateTime.Value = commandArgs[1];


                    //    DateTime.TryParse(commandArgs[1], out createTime);

                    BindRedPacketDetailList(commandArgs[1], commandArgs[0]);
                }

            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }
        //发送红包详细
        protected void gvSendList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewDetail")
                {
                    //  DateTime createTime = DateTime.Now;

                    var commandArgs = e.CommandArgument.ToString().Split(',');

                    //if (commandArgs.Count() != 2)
                    //{
                    //    throw new Exception("详情参数错误");
                    //}

                    hfSendListId.Value = commandArgs[0];
                    hfCreateTime.Value = commandArgs[1];


                    //  DateTime.TryParse(commandArgs[1], out createTime);

                    BindRedPacketDetailList(commandArgs[0]);
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }
    }
}