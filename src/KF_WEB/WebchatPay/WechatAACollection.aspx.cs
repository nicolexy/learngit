using System;
using System.Data;
using System.Linq;
using System.Web.Services.Protocols;
using System.Web.UI.WebControls;
using CFT.CSOMS.COMMLIB;
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.WebchatPay
{
    public partial class WechatAACollection : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                var wechatName = txtWechatName.Text.Trim();

                if (string.IsNullOrEmpty(wechatName))
                {
                    WebUtils.ShowMessage(this.Page, "微信号输入有误");
                }

                var aaUin = WeChatHelper.GetAAUINFromWeChatName(wechatName);
                var acctUin = WeChatHelper.GetUINFromWeChatName(wechatName);

                #if DEBUG
                aaUin = "oE15ntxv6MEUrZOZTk3a8ksPaAwU@aa.tenpay.com";               
                #endif

                if (string.IsNullOrEmpty(aaUin))
                    throw new Exception("查询微信号对应的AA财付通账户失败");

                BindBasicInfo(aaUin, acctUin);

                BindAACollections(aaUin,0, 20);
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }
        }

        private void BindBasicInfo(string aaUin, string acctUin)
        {
            lblAAUin.Text = aaUin;

            lblacctUin.Text = acctUin;

            var qs = new Query_Service.Query_Service();
            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
            var ds = qs.GetUserAccount(aaUin, 1, 0, 20);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                lblBanlance.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fbalance"].ToString());
            }
            
        }

        private void BindAACollections(string uin, int pageIndex, int pageSize)
        {

            var qs = new Query_Service.Query_Service();
                        
            var dsAACollections = qs.GetAATradeList(uin, pageIndex*pageSize, pageSize);

            this.aaCollectionPager.RecordCount = 1000;

            if (dsAACollections != null && dsAACollections.Tables.Count > 0)
            {
                dsAACollections.Tables[0].Columns.Add("Ftotal_paid_amount_text", typeof(string));
                dsAACollections.Tables[0].Columns.Add("Fstatus_text", typeof(string));

                foreach (DataRow item in dsAACollections.Tables[0].Rows)
                {
                    var totalPaidAmountText = classLibrary.setConfig.FenToYuan(item["Ftotal_paid_amount"].ToString());
                    switch (item["Ftype"].ToString())
                    {
                        case "1":
                            item["Ftotal_paid_amount_text"] = string.Format("+{0}", totalPaidAmountText);
                            break;
                        case "2":
                            item["Ftotal_paid_amount_text"] = string.Format("-{0}", totalPaidAmountText);
                            break;
                        default:
                            item["Ftotal_paid_amount_text"] = string.Format("未知{0}", totalPaidAmountText);
                            break;
                    }
                    
                    

                    switch (item["Flstate"].ToString())
                    {

                        case "1":
                            item["Fstatus_text"] = "正常";
                            break;
                        case "2":
                            item["Fstatus_text"] = "作废";
                            break;
                        case "3":
                            item["Fstatus_text"] = "关闭";
                            break;
                        default:
                            item["Fstatus_text"] = "未知" + item["Flstate"].ToString();
                            break;
                    }
                }

                gvAACollections.DataSource = dsAACollections.Tables[0].DefaultView;
                gvAACollections.DataBind();
            }
        }


        protected void aaCollectionPager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                var aaUin = lblAAUin.Text.Trim();
                if (string.IsNullOrEmpty(aaUin))
                    throw new Exception("获取用户AA财付通帐号异常");
                
                int pageIndex = e.NewPageIndex;
                this.aaCollectionPager.CurrentPageIndex = e.NewPageIndex;
                BindAACollections(aaUin, pageIndex-1, 20);
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }
        }


        protected void aaCollectionDetails_pagechanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            int currentPageIndex = e.NewPageIndex;
            this.aaCollectionDetails.CurrentPageIndex = currentPageIndex;

            string collectionNo = hfCurrentCollectionNo.Value;
            DateTime createTime = DateTime.Now;

            DateTime.TryParse(hfCurrentCollectionCreateTime.Value, out createTime);

            BindAADetails(collectionNo, createTime, currentPageIndex - 1, 20);
            
        }

        protected void gvAACollections_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try{
                if (e.CommandName == "ViewDetail")
                {
                    DateTime createTime = DateTime.Now;

                    var commandArgs = e.CommandArgument.ToString().Split(',');
                
                    if(commandArgs.Count() != 2)
                    {
                        throw new Exception("详情参数错误");
                    }

                    this.hfCurrentCollectionNo.Value = commandArgs[0];
                    this.hfCurrentCollectionCreateTime.Value = commandArgs[1];

                    DateTime.TryParse(commandArgs[1], out createTime);

                    BindAADetails(commandArgs[0], createTime, 0, 20);
                }
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + errStr);
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + eSys.Message.ToString());
            }
        }

        private void BindAADetails(string collectionNo, DateTime createTime, int pageIndex, int pageSize)
        {
            this.aaCollectionDetails.RecordCount = 1000;

            var qs = new Query_Service.Query_Service();
            var ds = qs.GetAATradeDetailsSingleYear(collectionNo, createTime, pageIndex * pageSize, pageSize);

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("Fnum_text", typeof(string));
                ds.Tables[0].Columns.Add("Fstate_text", typeof(string));
                ds.Tables[0].Columns.Add("receive_aaopenid", typeof(string));
                ds.Tables[0].Columns.Add("receive_name", typeof(string));


                if (ds.Tables[0].Rows.Count <= 0)
                {
                    WebUtils.ShowMessage(this.Page, string.Format("当前AA收款单{0}查询不到分单明细", collectionNo));
                }
                else
                {
                    //查询收款方AA财付通账号，发起收款的人的aaopenid放在总单里面wx_aa_collection_00.t_collection_list_0
                    var dsAATotalTrade = qs.QueryAATotalTradeInfo(collectionNo);
                    string receive_aaopenid = "";
                    string receive_name = "";
                    if (dsAATotalTrade != null && dsAATotalTrade.Tables.Count > 0 && dsAATotalTrade.Tables[0].Rows.Count > 0)
                    {
                        receive_aaopenid = dsAATotalTrade.Tables[0].Rows[0]["Fopenid"].ToString();
                        receive_name = dsAATotalTrade.Tables[0].Rows[0]["Fname"].ToString();
                    }

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        item["receive_aaopenid"] = receive_aaopenid;
                        item["receive_name"] = receive_name;
                        item["Fnum_text"] = classLibrary.setConfig.FenToYuan(item["Fnum"].ToString());

                        switch (item["Fstate"].ToString())
                        {

                            case "1":
                                item["Fstate_text"] = "等待支付";
                                break;
                            case "2":
                                item["Fstate_text"] = "支付完成";
                                break;
                            case "3":
                                item["Fstate_text"] = "B2C转账完成";
                                break;
                            case "4":
                                item["Fstate_text"] = "退款";
                                break;
                            default:
                                item["Fstate_text"] = "未知" + item["Fstate"].ToString();
                                break;
                        }
                    }
                }

                gvaacollectiondetails.DataSource = ds.Tables[0].DefaultView;
                gvaacollectiondetails.DataBind();
            }
        }

    }
}