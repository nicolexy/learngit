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
       // DateTime beginTime, endTime;
        string wechatName,hbUin,payListId;


        protected void Page_Load(object sender, EventArgs e)
        {
            //ButtonBeginDate.Attributes.Add("onclick", "openModeBegin()");
            //ButtonEndDate.Attributes.Add("onclick", "openModeEnd()");

            if (!IsPostBack)
            {
                //TextBoxBeginDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy年MM月dd日");
                //TextBoxEndDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
            }
        }

        private void FetchInput()
        {
            wechatName = txtWechatName.Text.Trim();

            if (string.IsNullOrEmpty(wechatName))
            {
                WebUtils.ShowMessage(this.Page, "微信号输入有误");
            }

            //try
            //{
            //    beginTime = DateTime.Parse(TextBoxBeginDate.Text.Trim());
            //    endTime = DateTime.Parse(TextBoxEndDate.Text.Trim());
            //}
            //catch
            //{
            //    throw new Exception("查询日期输入有误");
            //}

            hbUin = hfHBUin.Value.Trim();

            if (string.IsNullOrEmpty(hbUin))
            {
                hbUin = WeChatHelper.GetHBUINFromWeChatName(wechatName);

                if (string.IsNullOrEmpty(hbUin))
                    throw new Exception("查询微信号对应的红包财付通账户失败");

//#if DEBUG
//                hbUin = "oE15nt-ddlcBCbICy4VJcRXFG20Y@hb.tenpay.com";
//#endif
                hfHBUin.Value = hbUin;
            }

           // payListId = txtPayListId.Text.Trim();
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.receivePager.RecordCount = 1000;
                this.sendListPager.RecordCount = 1000;
                this.redPacketDetailPager.RecordCount = 1000;
                FetchInput();

                try
                {
                    BindBasicInfo(hbUin);
                }
                catch { }//防止不能查询

                //测试接口
              //  hbUin = "onqOjjqoNArgAnXUC-g2QyPfL9fQ";
             //  hbUin = "onqOjjhRRpnwjceoYnAY2CrJd2JU";

                hbUin = hbUin.Replace("@hb.tenpay.com", "");
                ViewState["hbUin"] = hbUin;
                BindReceiveList(hbUin,  1, 10);

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

        private void BindReceiveList(string hbUin, int pageIndex, int pageSize)
        {
            //var qs = new Query_Service.Query_Service();

            //var dsReceiveList = qs.GetReceiveRedPacketList(hbUin, beginTime, endTime, pageIndex * pageSize, pageSize);
            int max = pageSize;
            int start = max * (pageIndex - 1);
            receivePager.CurrentPageIndex = pageIndex;
            string ip = Request.UserHostAddress.ToString();
            if (ip == "::1")
                ip = "127.0.0.1";

            var dsReceiveList = new WechatPayService().QueryWebchatHB(hbUin, 2, ip, start, max);

            if (dsReceiveList != null && dsReceiveList.Tables.Count > 0)
            {
                dsReceiveList.Tables[0].Columns.Add("Famount_text", typeof(string));
                dsReceiveList.Tables[0].Columns.Add("Title", typeof(string));

                foreach (DataRow item in dsReceiveList.Tables[0].Rows)
                {
                    item["Famount_text"] = classLibrary.setConfig.FenToYuan(item["Famount"].ToString());
                    item["Title"] = string.Format("{0}发的红包", item["Fsend_name"].ToString());
                }

                gvReceiveList.DataSource = dsReceiveList.Tables[0].DefaultView;
                gvReceiveList.DataBind();
            }

        }

        private void BindSendList(string hbUin,  int pageIndex, int pageSize)
        {
            //var qs = new Query_Service.Query_Service();

            //var dsSendList = qs.GetSendRedPacketList(hbUin, beginTime, endTime, pageIndex * pageSize, pageSize, payListId);

            int max = pageSize;
            int start = max * (pageIndex - 1);
            sendListPager.CurrentPageIndex = pageIndex;

            string ip = Request.UserHostAddress.ToString();
            if (ip == "::1")
                ip = "127.0.0.1";

            var dsSendList = new WechatPayService().QueryWebchatHB(hbUin, 1, ip, start, max);
           

            if (dsSendList != null && dsSendList.Tables.Count > 0)
            {
                dsSendList.Tables[0].Columns.Add("summary", typeof(string));
                dsSendList.Tables[0].Columns.Add("Fstate_text", typeof(string));
                dsSendList.Tables[0].Columns.Add("refund", typeof(string));
                dsSendList.Tables[0].Columns.Add("Ftotal_amount_text", typeof(string));

                foreach (DataRow item in dsSendList.Tables[0].Rows)
                {
                    item["summary"] = string.Format("{0}/{1},总计{2}元",
                                                    item["Freceived_num"].ToString(),
                                                    item["Ftotal_num"].ToString(),
                                                    classLibrary.setConfig.FenToYuan(item["Freceived_amount"].ToString()));

                    double refundAmount = double.Parse(item["Ftotal_amount"].ToString()) - double.Parse(item["Freceived_amount"].ToString());

                    item["refund"] = classLibrary.setConfig.FenToYuan(refundAmount);
                    item["Ftotal_amount_text"] = classLibrary.setConfig.FenToYuan(item["Ftotal_amount"].ToString());

                    switch (item["Fstate"].ToString())
                    {

                        case "1":
                            item["Fstate_text"] = "等待支付";
                            break;
                        case "2":
                            item["Fstate_text"] = "支付完成";
                            break;
                        case "3":
                            item["Fstate_text"] = "部分领取";
                            break;
                        case "4":
                            item["Fstate_text"] = "全部领取";
                            break;
                        case "5":
                            item["Fstate_text"] = "过期退回";
                            break;
                        default:
                            item["Fstate_text"] = "未知" + item["Flstate"].ToString();
                            break;
                    }
                }

                gvSendList.DataSource = dsSendList.Tables[0].DefaultView;
                gvSendList.DataBind();
            }

            this.sendListPager.RecordCount = 1000;
        }

        private void BindRedPacketDetailList(string sendListId, int pageIndex, int pageSize)
        {
            //var qs = new Query_Service.Query_Service();

            //var dsDetailList = qs.GetRedPacketDetailList(sendListId, createTime, pageIndex * pageSize, pageSize);
            this.redPacketDetailPager.CurrentPageIndex = pageIndex;
            int max = pageSize;
            int start = max * (pageIndex - 1);
            string ip = Request.UserHostAddress.ToString();
            if (ip == "::1")
                ip = "127.0.0.1";

            var dsDetailList = new WechatPayService().QueryWebchatHB(sendListId, 3, ip, start, max);


            if (dsDetailList != null && dsDetailList.Tables.Count > 0)
            {
                dsDetailList.Tables[0].Columns.Add("Famount_text", typeof(string));
                dsDetailList.Tables[0].Columns.Add("Freceive_openid_text", typeof(string));

                foreach (DataRow item in dsDetailList.Tables[0].Rows)
                {
                    item["Famount_text"] = classLibrary.setConfig.FenToYuan(item["Famount"].ToString());
                    item["Freceive_openid_text"] = item["Freceive_openid"].ToString() + "@hb.tenpay.com";
                }

                gvRedPacketDetail.DataSource = dsDetailList.Tables[0].DefaultView;
                gvRedPacketDetail.DataBind();
            }

           
        }

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

        protected void gvReceiveList_RowCommand(object sender, GridViewCommandEventArgs e)
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

                   
                //    DateTime.TryParse(commandArgs[1], out createTime);

                    BindRedPacketDetailList(commandArgs[0],1,10);
                }
                
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        protected void gvSendList_RowCommand(object sender, GridViewCommandEventArgs e)
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


                  //  DateTime.TryParse(commandArgs[1], out createTime);

                    BindRedPacketDetailList(commandArgs[0],  1, 10);
                }
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        protected void redPacketDetailPager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
              
                var sendListId = hfSendListId.Value;
                //DateTime createTime = DateTime.Now;
                //DateTime.TryParse(hfCreateTime.Value, out createTime);

                BindRedPacketDetailList(sendListId, e.NewPageIndex, 10);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }



        
    }
}