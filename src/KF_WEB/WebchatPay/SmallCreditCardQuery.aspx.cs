using System;
using System.Data;
using System.Web.Services.Protocols;
using CFT.CSOMS.COMMLIB;
using Tencent.DotNet.Common.UI;

namespace TENCENT.OSS.CFT.KF.KF_Web.WebchatPay
{
    public partial class SmallCreditCardQuery : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            
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
            this.pager.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePage);
        }
        #endregion

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            int type = 13;
            if (ViewState["qType"] != null && ViewState["qType"].ToString() != "") 
            {
                type = int.Parse(ViewState["qType"].ToString());
            }
            BindData(e.NewPageIndex, type);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                var wechatName = txtWechatName.Text.Trim();
                string tradeId = txtTradeId.Text.Trim();

                this.pager.RecordCount = 1000;

                if (!string.IsNullOrEmpty(tradeId)) 
                {
                    BindTradeInfo(tradeId);
                }
                else if (!string.IsNullOrEmpty(wechatName))
                {
                    BindWechatInfo(wechatName);
                }
                else 
                {
                    throw new Exception("请输入微信号或交易单号！");
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

        private void BindWechatInfo(string wechatName)
        {
            clearAccInfo();
            //通过微信号查小额刷卡信息
            var acctUin = WeChatHelper.GetUINFromWeChatName(wechatName);
            if (string.IsNullOrEmpty(acctUin))
                throw new Exception("查询微信号对应的财付通账户失败");

            ViewState["acctUin"] = acctUin;
            ViewState["qType"] = 13;

            BindXeskInfo(acctUin);


            BindData(1,13);
            
        }

        private void BindXeskInfo(string uin) 
        {
            var qs = new Query_Service.Query_Service();
            qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
            var ds = qs.GetSmallCreditCardInfo(uin);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                lblCreateTime.Text = ds.Tables[0].Rows[0]["Fcreate_time"].ToString();
                lblAccountNo.Text = uin;
                string s_state = ds.Tables[0].Rows[0]["Fstate"].ToString();
                if (s_state == "1")
                {
                    lblAccountState.Text = "开通";
                }
                else if (s_state == "2")
                {
                    lblAccountState.Text = "冻结";
                }
                else if (s_state == "3")
                {
                    lblAccountState.Text = "解冻";
                }
                else
                {
                    lblAccountState.Text = "未知状态：" + s_state;
                }

                lblBanlance.Text = classLibrary.setConfig.FenToYuan(ds.Tables[0].Rows[0]["Fbalance"].ToString());
            }
        }

        private void BindTradeInfo(string tradeId) 
        {
            ViewState["tradeId"] = tradeId;
            ViewState["qType"] = 4;

            clearAccInfo();

            if (!string.IsNullOrEmpty(tradeId)) 
            {
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                qs.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
                DataSet ds = qs.GetPayList(tradeId, 4, DateTime.Now.AddDays(-30), DateTime.Now, 0, 1);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //首先过滤掉113记录
                    string channel_id = ds.Tables[0].Rows[0]["Fchannel_id"].ToString();
                    if (channel_id == "113") 
                    {
                        //通过买家账户号查小额刷卡信息
                        string uin = ds.Tables[0].Rows[0]["Fbuyid"].ToString();
                        if (!string.IsNullOrEmpty(uin))
                        {
                            BindXeskInfo(uin);
                        }

                        ds.Tables[0].Columns.Add("Fpaynum_str", typeof(String));//支付金额
                        ds.Tables[0].Columns.Add("Ftrade_state_str", typeof(String));//状态
                        ds.Tables[0].Columns.Add("Fbank_id_str", typeof(String));//银行卡号
                        ds.Tables[0].Columns.Add("Fbank_type_str", typeof(String));//银行类型

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            string s_bankid = dr["Fbuy_bankid"].ToString();
                            if (!string.IsNullOrEmpty(s_bankid))
                            {
                                dr["Fbank_id_str"] = s_bankid.Substring(s_bankid.Length - 4, 4);
                            }

                        }

                        classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0], "Fbuy_bank_type", "Fbank_type_str", "BANK_TYPE");
                        classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0], "Ftrade_state", "Ftrade_state_str", "PAY_STATE");
                        classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fpaynum", "Fpaynum_str");

                        DataGrid1.DataSource = ds.Tables[0].DefaultView;
                        DataGrid1.DataBind();
                    }
                }
            }
        }

        private void BindData(int index, int qType) 
        {
            if (qType == 4) 
            {
                //通过单号查询，无分页
                if (index == 1)
                {
                    BindTradeInfo(ViewState["tradeId"].ToString());
                }
                else 
                {
                    return;
                }
            }
            if (ViewState["acctUin"] != null) 
            {
                string uin = ViewState["acctUin"].ToString();

                int max = pager.PageSize;
                int start = max * (index - 1);

                var qs = new Query_Service.Query_Service();
                DataSet ds = qs.GetPayList(uin, qType, DateTime.Now.AddDays(-30), DateTime.Now, start, max);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) 
                {
                    ds.Tables[0].Columns.Add("Fpaynum_str", typeof(String));//支付金额
                    ds.Tables[0].Columns.Add("Ftrade_state_str", typeof(String));//状态
                    ds.Tables[0].Columns.Add("Fbank_id_str", typeof(String));//银行卡号
                    ds.Tables[0].Columns.Add("Fbank_type_str", typeof(String));//银行类型

                    foreach (DataRow dr in ds.Tables[0].Rows) 
                    {
                        string s_bankid = dr["Fbuy_bankid"].ToString();
                        if(!string.IsNullOrEmpty(s_bankid))
                        {
                            dr["Fbank_id_str"] = s_bankid.Substring(s_bankid.Length - 4, 4);
                        }
                        
                    }

                    classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0], "Fbuy_bank_type", "Fbank_type_str", "BANK_TYPE");
                    classLibrary.setConfig.GetColumnValueFromDic(ds.Tables[0], "Fstate", "Ftrade_state_str", "PAY_STATE");
                    classLibrary.setConfig.FenToYuan_Table(ds.Tables[0], "Fpaynum", "Fpaynum_str");

                    DataGrid1.DataSource = ds.Tables[0].DefaultView;
                    DataGrid1.DataBind();
                }
            }
        }

        private void clearAccInfo() 
        {
            lblCreateTime.Text = "";
            lblAccountNo.Text = "";
            lblAccountState.Text = "";
            lblBanlance.Text = "";
        }
    }
}