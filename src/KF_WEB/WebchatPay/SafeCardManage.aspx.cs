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
using System.Web.Services.Protocols;
using System.Linq;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.C2C.Finance.BankLib;
using CFT.CSOMS.BLL.FundModule;
using System.Configuration;
using CFT.CSOMS.COMMLIB;
using CFT.CSOMS.BLL.FundModule;
using CFT.CSOMS.BLL.CFTAccountModule;
namespace TENCENT.OSS.CFT.KF.KF_Web.WebchatPay
{
    using System.Configuration;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Xml;
    using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
    using System.Configuration;
    using System.Text;
    using System.Net;
    using System.Xml;
    using System.IO;
    using System.Collections.Generic;

    public partial class SafeCardManage : System.Web.UI.Page
    {
        protected Query_Service.Query_Service queryService = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
        private string fundSPId;
        protected FundService fundBLLService = new FundService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //绑定基金公司列表
                BindFundsList();
            }
            fundSPId = this.ddl_companyName.SelectedValue;
            queryService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);
        }

        private void BindFundsList()
        {
            try
            {
                var fundCorLists = FundService.GetAllFundInfo();

                if (fundCorLists.Count < 1)
                    throw new Exception("未拉取到基金公司记录表");

                this.ddl_companyName.DataSource = fundCorLists;
                this.ddl_companyName.DataTextField = "Name";
                this.ddl_companyName.DataValueField = "SPId";
                this.ddl_companyName.DataBind();
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, "基金公司列表拉取失败:" + HttpUtility.JavaScriptStringEncode(ex.ToString()));
            }
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                var qqId = getQQID();
                if (string.IsNullOrEmpty(qqId))
                {
                    WebUtils.ShowMessage(this, "帐号异常");
                    return;
                }

                if (!ClassLib.ValidateRight("BalanceControl", this))
                {
                    lblALLFinance.Text = "";
                }
                else 
                {
                    lblALLFinance.Text = BindALLFinance(qqId);
                }
                

                BindFundPayBankCardInfo(qqId);

                var bindBankCards = queryService.GetBindBankCard(qqId);

                if (bindBankCards != null && bindBankCards.Tables.Count > 0)
                {
                    gvBindBankCard.DataSource = FilterSupportBankCard(bindBankCards.Tables[0], qqId);
                    gvBindBankCard.DataBind();
                }

                this.tradeListPager.RecordCount = 1000;
                this.tradeLogPager.RecordCount = 1000;

                BindFundTradeList(qqId, fundSPId,1);
                FundTradeLog(qqId,1);
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + HttpUtility.JavaScriptStringEncode(eSoap.ToString()));
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取数据失败！" + HttpUtility.JavaScriptStringEncode(eSys.ToString()));
            }
        }

        private DataTable FilterSupportBankCard(DataTable dtBindBankCards, string qqId)
        {
            dtBindBankCards.Columns.Add("supportFund", typeof(String));
            dtBindBankCards.Columns.Add("safeCard", typeof(String));
            dtBindBankCards.Columns.Add("bankTypeName", typeof(string));

            string safeCardBankType = string.Empty;
            string safeCardTail = string.Empty;
            string safeCardId = string.Empty;
            var dtFundPayCard = fundBLLService.GetPayCardInfo(qqId);
            if (dtFundPayCard != null && dtFundPayCard.Rows.Count > 0)
            {
                safeCardBankType = dtFundPayCard.Rows[0]["Fbank_type"].ToString();
                safeCardTail = dtFundPayCard.Rows[0]["Fcard_tail"].ToString();
                safeCardId = dtFundPayCard.Rows[0]["Fbank_id"].ToString();
            }

            foreach (DataRow bindCardItem in dtBindBankCards.Rows)
            {

                bindCardItem["bankTypeName"] = BankIO.QueryBankName(bindCardItem["bank_type"].ToString());

                if (fundBLLService.GetFundSupportBank(bindCardItem["bank_type"].ToString()))
                {
                    bindCardItem["supportFund"] = "支持";
                }
                else
                {
                    bindCardItem["supportFund"] = "不支持";
                }

                if (bindCardItem["bank_type"].ToString() == safeCardBankType
                    && bindCardItem["card_tail"].ToString() == safeCardTail
                    && bindCardItem["bankid"].ToString() == safeCardId)
                {
                    bindCardItem["safeCard"] = "是";
                }
                else
                {
                    bindCardItem["safeCard"] = "否";
                }

            }

            return dtBindBankCards;
        }

        protected void gvBindBankCard_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ModifyFundPayCard")
                {
                    int rowIndex = int.Parse(e.CommandArgument.ToString());
                    var row = gvBindBankCard.Rows[rowIndex];

                    var newCardInfo = new SafeCardInfo()
                    {
                        bindSerialNo = gvBindBankCard.DataKeys[rowIndex].Value.ToString(),
                        bankType = row.Cells[10].Text,
                        bankId = row.Cells[7].Text,
                        mobile = row.Cells[4].Text,
                        cardTail = row.Cells[3].Text,
                        uin = row.Cells[0].Text,
                        uid = row.Cells[8].Text,
                        bindTime = DateTime.Parse(row.Cells[11].Text),
                    };

                    var checkResult = CheckOperationCondition(newCardInfo);
                    if (checkResult.Count > 0)
                        throw new Exception(checkResult.ToString("\n"));

                    ModifyFundPayBankCard(newCardInfo);

                }
            }
            catch (SoapException eSoap) //捕获soap类异常
            {
                string errStr = PublicRes.GetErrorMsg(eSoap.Message);
                WebUtils.ShowMessage(this.Page, "调用服务出错：" + HttpUtility.JavaScriptStringEncode(eSoap.ToString()));

            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "修改绑定安全卡异常！" + HttpUtility.JavaScriptStringEncode(eSys.ToString()));
            }
        }

        protected void BindFundPayBankCardInfo(string qqId)
        {
            //安全卡信息
            var safeCardInfo = fundBLLService.GetPayCardInfo(qqId);
            if (safeCardInfo != null && safeCardInfo.Rows.Count > 0)
            {
                lblBankType.Text = BankIO.QueryBankName(safeCardInfo.Rows[0]["Fbank_type"].ToString());
                lblBankCardTail.Text = safeCardInfo.Rows[0]["Fcard_tail"].ToString();
                lblCell.Text = safeCardInfo.Rows[0]["Fmobile"].ToString();
            }
            else
            {
                //clear viewstate
                lblBankType.Text = lblBankCardTail.Text = lblCell.Text = string.Empty;
            }
        }

        private string BindALLFinance(string qqId)
        {
            string LCTBalance = "0.00";
            decimal markValue = 0;
            try//理财通余额查询
            {
                DataTable subAccountInfoTable = new LCTBalanceService().QuerySubAccountInfo(qqId, 89);//理财通余额，币种89

                if (subAccountInfoTable == null || subAccountInfoTable.Rows.Count < 1)
                {
                    LCTBalance = "0.00";
                }
                else
                {
                    LCTBalance = MoneyTransfer.FenToYuan((subAccountInfoTable.Rows[0]["Fbalance"].ToString()));//分转元
                }
            }
            catch (Exception eSys)
            {
                throw new Exception("查询理财通余额失败！" + eSys.Message.ToString());
            }

            try//市值查询
            {
                //总市值
                markValue = new FundService().GetUserFundMarkValue(qqId);
            }
            catch (Exception eSys)
            {
                throw new Exception("查询理基金市值失败！" + eSys.Message.ToString());
            }

            return  (decimal.Parse(LCTBalance.ToString().Trim()) + markValue).ToString();
        }

        private void BindFundTradeList(string qqId, string spId,int pageIndex, int pageSize = 5)
        {
            this.tradeListPager.CurrentPageIndex = pageIndex;
            if (string.IsNullOrEmpty(spId))
                throw new Exception(string.Format("无法同时查询所有基金的流水信息，请选择指定的基金"));

            var fundInfo = FundService.GetAllFundInfo().Where(i => i.SPId == spId);

            if (fundInfo.Count() < 1)
                throw new Exception(string.Format("找不到{0}对应的基金信息", spId));
           // DataSet dsTradeList = queryService.GetFundTradeList(qqId, pageIndex, pageSize);//lxl需要开发修改
            DateTime date = DateTime.Parse("1900-01-01 00:00:00");
            DataTable dsTradeList = fundBLLService.GetFundRollList(qqId, date, date, fundInfo.First().CurrencyType.ToString(), this.tradeLogPager.PageSize * (pageIndex - 1), pageSize, -1);
           
            if (dsTradeList!=null)
            {
                dgFundTradeList.DataSource = FilterTradeList(dsTradeList).DefaultView;
            }
            else
            {
                //clear viewstate
                dgFundTradeList.DataSource = null;
            }


            dgFundTradeList.DataBind();

        }

        private void FundTradeLog(string qqId, int pageIndex, int pageSize = 5)
        {
            this.tradeLogPager.CurrentPageIndex = pageIndex;

            DataTable dsList = fundBLLService.GetFundTradeLog(qqId, this.tradeLogPager.PageSize * (pageIndex - 1), pageSize);

            if (dsList != null)
            {
                if(dsList.Rows.Count>0){
                    dsList.Columns.Add("Fbank_type_str", typeof(String));
                    foreach (DataRow dr in dsList.Rows)
                    {
                        dr["Fbank_type_str"] = getData.GetBankNameFromBankCode(dr["Fbank_type"].ToString());
                    }
                }
                dgFundTradeLog.DataSource = dsList.DefaultView;
            }
            else
            {
                dgFundTradeLog.DataSource = null;
            }

            dgFundTradeLog.DataBind();
        }

        protected void ModifyFundPayBankCard(SafeCardInfo newCardInfo)
        {
            try
            {
                var tradeId = string.Empty;

                var dtFundPayCard = fundBLLService.GetPayCardInfo(newCardInfo.uin);
                if (dtFundPayCard != null && dtFundPayCard.Rows.Count > 0)
                {
                    tradeId = dtFundPayCard.Rows[0]["Ftrade_id"].ToString();
                }


                var callResult = queryService.ModifyFundPayCard(newCardInfo.uin, tradeId, newCardInfo.uid, newCardInfo.bindSerialNo, newCardInfo.bankType,
                    newCardInfo.cardTail, newCardInfo.bankId, newCardInfo.mobile);

                if (callResult == true)
                    WebUtils.ShowMessage(this.Page, "修改理财通安全卡成功!");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("执行修改安全卡异常：{0}", ex.Message));
            }
        }

        /// <summary>
        /// 检查权限
        /// </summary>
        /// <returns></returns>
        protected OperatorRole GetOperatorRole()
        {
            if (classLibrary.ClassLib.ValidateRight("ModifyFundPayCard", this))
            {
                return OperatorRole.RC;
            }
            else if (classLibrary.ClassLib.ValidateRight("ModifyFundPayCardByCS", this))
            {
                return OperatorRole.CS;
            }
            else
            {
                return OperatorRole.Unknown;
            }
        }

        //检查操作前置条件
        protected List<string> CheckOperationCondition(SafeCardInfo newCardInfo)
        {
            var checkResult = new List<string>();

            switch (GetOperatorRole())
            {
                case OperatorRole.Unknown:
                    checkResult.Add("权限不足");
                    break;
                case OperatorRole.RC:
                    break;
                case OperatorRole.CS:
                    checkResult = CheckBusinessLogic(newCardInfo);
                    break;
                default:
                    checkResult.Add("权限不足");
                    break;
            }

            return checkResult;

        }

        //检查数据的业务逻辑
        protected List<string> CheckBusinessLogic(SafeCardInfo newCardInfo)
        {
            var checkResult = new List<string>();

            //配置到文件
            decimal totalBalanceStandard = decimal.Parse(System.Configuration.ConfigurationManager.AppSettings["SafeCardBS"].ToString());
            decimal ALLFinance = 0;
            ALLFinance = decimal.Parse(BindALLFinance(newCardInfo.uin));
            //if (totalBalance >= 5000000)
            if (ALLFinance >= totalBalanceStandard/100)
            {
                checkResult.Add("理财通总资产大于" + totalBalanceStandard/100);
                return checkResult;
            }

            //新银行卡是绑定的时间大于3天 
            //if (DateTime.Now.Subtract(newCardInfo.bindTime).Days < 3)
            //{
            //    checkResult.Add("新银行卡绑定的时间小于3天");
            //    return checkResult;
            //}

            var safeCardInfo = fundBLLService.GetPayCardInfo(newCardInfo.uin);

            if (safeCardInfo != null && safeCardInfo.Rows.Count > 0)
            {
                //旧银行卡（原安全卡）的绑定时间大于5天
                //string date = PublicRes.objectToString(safeCardInfo, "Fmodify_time");
                //if (DateTime.Now.Subtract(DateTime.Parse(date)).Days < 5)
                //{
                //    checkResult.Add("旧银行卡绑定的时间小于5天");
                //    return checkResult;
                //}

                //将上面的条件改为旧卡绑定快捷支付的时间，而不是旧卡成为安全卡的时间
                string bank_id = PublicRes.objectToString(safeCardInfo, "Fbank_id");
                string bind_serialno = PublicRes.objectToString(safeCardInfo, "Fbind_serialno");//绑定序列号
                Query_Service.Query_Service qs = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
                //与一点通查询一样
                //根据uin 卡号  绑定序列号 绑定状态所有 查询
                DataSet ds = qs.GetBankCardBindList_New(newCardInfo.uin, "", bank_id, "", "", "", "", "", "", "", 0, false, 99, bind_serialno, 0, 2);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    throw new Exception("未查询到旧卡绑定记录！");
                }
                else
                {
                    string date = ds.Tables[0].Rows[0]["Fbind_time_local"].ToString();
                    try
                    {
                        DateTime.Parse(date);
                    }
                    catch
                    {
                        throw new Exception("旧卡绑定时间格式有误！");
                    }
                    if (DateTime.Now.Subtract(DateTime.Parse(date)).Days < 5)
                    {
                        checkResult.Add("旧银行卡绑定的时间小于5天");
                        return checkResult;
                    }
                }

                if (ALLFinance >= 1000000)//lxl 20140729加
                {
                    //新旧银行卡是同一个银行的
                    if (PublicRes.objectToString(safeCardInfo, "Fbank_type") != newCardInfo.bankType)
                    {
                        checkResult.Add("新旧银行卡不是同一个银行");
                        return checkResult;
                    }

                    //新旧卡绑定的手机号码一样
                    if (PublicRes.objectToString(safeCardInfo, "Fmobile") != newCardInfo.mobile)
                    {
                        checkResult.Add("新旧卡绑定的手机号码不一样");
                        return checkResult;
                    }
                }
            }

            return checkResult;
        }

        protected void tradeListPager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                var qqId = txtQQId.Text.Trim();
                if (string.IsNullOrEmpty(qqId))
                    throw new Exception("理财通帐号为空");

                this.tradeListPager.CurrentPageIndex = e.NewPageIndex;

                BindFundTradeList(qqId,fundSPId,e.NewPageIndex);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, string.Format("翻页异常:{0}", ex.Message));
            }
        }

        protected void tradeLogPager_PageChanged(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            try
            {
                var qqId = txtQQId.Text.Trim();
                if (string.IsNullOrEmpty(qqId))
                    throw new Exception("理财通帐号为空");

                this.tradeLogPager.CurrentPageIndex = e.NewPageIndex;

                FundTradeLog(qqId,e.NewPageIndex);
            }
            catch (Exception ex)
            {
                WebUtils.ShowMessage(this, string.Format("翻页异常:{0}", ex.Message));
            }
        }

        private DataTable FilterTradeList(DataTable tradeList)
        {
            tradeList.Columns.Add("TradeType", typeof(String));
            tradeList.Columns.Add("PayAmountText", typeof(String));
            tradeList.Columns.Add("ChannelName", typeof(String));

            if (tradeList != null && tradeList.Rows.Count > 0)
            {
                foreach (DataRow tradeItem in tradeList.Rows)
                {
                    tradeItem["PayAmountText"] = classLibrary.setConfig.FenToYuan(tradeItem["Ftotal_fee"].ToString());

                    switch (tradeItem["Fchannel_id"].ToString())
                    {
                        case "1":
                            tradeItem["ChannelName"] = "财付通网站";
                            break;
                        case "2":
                            tradeItem["ChannelName"] = "微信";
                            break;
                        case "3":
                            tradeItem["ChannelName"] = "手Q";
                            break;
                        default:
                            tradeItem["ChannelName"] = "未知" + tradeItem["Fchannel_id"].ToString();
                            break;
                    }

                    switch (tradeItem["Fpur_type"].ToString())
                    {
                        case "1":
                            tradeItem["TradeType"] = "申购";
                            break;
                        case "2":
                            tradeItem["TradeType"] = "认购";
                            break;
                        case "3":
                            tradeItem["TradeType"] = "定投";
                            break;
                        case "4":
                            tradeItem["TradeType"] = "赎回";
                            break;
                        case "5":
                            tradeItem["TradeType"] = "撤销";
                            break;
                        case "6":
                            tradeItem["TradeType"] = "分红";
                            break;
                        case "7":
                            tradeItem["TradeType"] = "认申购失败";
                            break;
                        case "8":
                            tradeItem["TradeType"] = "比例确认退款";
                            break;
                        case "9":
                            tradeItem["TradeType"] = "赠送收益申购";
                            break;
                        case "10":
                            tradeItem["TradeType"] = "赠送份额申购";
                            break;
                        default:
                            tradeItem["TradeType"] = "未知" + tradeItem["Fpur_type"].ToString();
                            break;
                    }
                }
            }

            return tradeList;
        }

        #region need move to bll
        string getQQID()
        {
            var id = this.txtQQId.Text.Trim();

            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("请输入要查询的账号");
            }

            if (this.WeChatCft.Checked)
            {
                return id;
            }
            else if (this.WeChatUid.Checked)
            {
                var qs = new Query_Service.Query_Service();
                return qs.Uid2QQ(id);
            }
            else if (this.WeChatQQ.Checked || this.WeChatMobile.Checked || this.WeChatEmail.Checked)
            {
                string queryType = string.Empty;
                if (this.WeChatQQ.Checked)
                {
                    queryType = "QQ";
                }
                else if (this.WeChatMobile.Checked)
                {
                    queryType = "Mobile";
                }
                else if (this.WeChatEmail.Checked)
                {
                    queryType = "Email";
                }

                string openID = string.Empty, errorMessage = string.Empty;
                int errorCode = 0;
                var IPList = ConfigurationManager.AppSettings["WeChat"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < IPList.Length; j++)
                {
                    if (getOpenIDFromWeChat(queryType, id, out openID, out errorCode, out errorMessage, IPList[j]))
                    {
                        break;
                    }
                }
                if (errorCode == 0)
                {
                    return openID + "@wx.tenpay.com";
                }
                else if (errorCode == 1)
                {
                    throw new Exception("没有此用户");
                }
                else
                {
                    throw new Exception(errorCode + errorMessage);
                }
            }
            else if (this.WeChatId.Checked)
            {
                return WeChatHelper.GetUINFromWeChatName(id);
            }

            return id;
        }

        //通过微信绑定的QQ、手机或邮箱信息查询其openID，对应的财付通账号便是openID@wx.tenpay.com
        bool getOpenIDFromWeChat(string queryType, string ID, out string openID, out int errorCode, out string errorMessage, string IP)
        {
            openID = errorMessage = string.Empty;
            errorCode = 0;
            try
            {
                string parameterString = "<Request>{0}<AppId>wx482cac0d58846383</AppId></Request>";
                string IDstring = string.Empty;
                string API;
                if (queryType == "QQ")
                {
                    IDstring = string.Format("<QQ>{0}</QQ>", ID);
                    API = "ConvertQQToOuterAcctId";
                }
                else if (queryType == "Mobile")
                {
                    IDstring = string.Format("<Mobile>{0}</Mobile>", ID);
                    API = "ConvertMobileToOuterAcctId";
                }
                else if (queryType == "Email")
                {
                    IDstring = string.Format("<Email>{0}</Email>", ID);
                    API = "ConvertEmailToOuterAcctId";
                }
                else
                {
                    errorCode = -1;
                    errorMessage = "查询类型不正确";
                    return false;
                }
                parameterString = string.Format(parameterString, IDstring);
                var data = Encoding.Default.GetBytes(parameterString);
                var request = (HttpWebRequest)WebRequest.Create(string.Format("http://{0}:12137/cgi-bin/{1}?f=xml&appname=wx_tenpay", IP, API));
                request.Method = "POST";
                request.ContentType = "text/xml;charset=UTF-8";
                var parameter = request.GetRequestStream();
                parameter.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var myResponseStream = response.GetResponseStream();
                var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                var resultXml = new XmlDocument();
                resultXml.LoadXml(myStreamReader.ReadToEnd());
                myStreamReader.Close();
                myResponseStream.Close();
                var responseNode = resultXml.SelectSingleNode("Response");
                errorCode = Convert.ToInt32(responseNode.SelectSingleNode("error").SelectSingleNode("code").InnerText);
                errorMessage = responseNode.SelectSingleNode("error").SelectSingleNode("message").InnerText;
                openID = responseNode.SelectSingleNode("result").SelectSingleNode("OuterAcctId").InnerText;
                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }
        }

        #endregion
    }
    public enum OperatorRole
    {
        Unknown, //未知
        RC,      // 风控
        CS       //客服
    }

    public class SafeCardInfo
    {
        public string uid { get; set; }
        public string uin { get; set; }
        public string bindSerialNo { get; set; }
        public string bankType { get; set; }
        public string cardTail { get; set; }
        public string bankId { get; set; }
        public string mobile { get; set; }
        public DateTime bindTime { get; set; }
    }

}