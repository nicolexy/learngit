using System;
using System.Data;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using CFT.CSOMS.BLL.ActivityModule;
using System.Collections;
using CFT.CSOMS.BLL.FundModule;
using CFT.CSOMS.COMMLIB;
using System.Configuration;
using System.Text;
using System.Net;
using System.Xml;
using System.IO;

namespace TENCENT.OSS.CFT.KF.KF_Web.WebchatPay
{
	/// <summary>
    /// QueryContractMachine 的摘要说明。
    /// 合约机详情查询
	/// </summary>
    public partial class QueryContractMachine : System.Web.UI.Page
	{
        protected void Page_Load(object sender, System.EventArgs e)
		{
           
			try
			{
				Label1.Text = Session["uid"].ToString();
				string szkey = Session["SzKey"].ToString();

                if (!IsPostBack)
                {
                    
                    
                }
                 
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}
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
            this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
		}
		#endregion


		private void ValidateDate()
		{
            string cft_no = txtCftNo.Text.Trim();
            string uin = tbx_payAccount.Text.Trim();

            if (cft_no == "" && uin == "")
            {
                throw new Exception("订单号或账号不能为空！");
            }
		}

        public void btnQuery_Click(object sender, System.EventArgs e)
		{
            try
			{
				ValidateDate();
			}
			catch(Exception err)
			{
				WebUtils.ShowMessage(this.Page,err.Message);
				return;
			}

			try
			{
                QueryData();
			}
			catch(SoapException eSoap) //捕获soap类异常
			{
				string errStr = PublicRes.GetErrorMsg(eSoap.Message.ToString());
				WebUtils.ShowMessage(this.Page,"调用服务出错：" + errStr);
			}
			catch(Exception eSys)
			{
				WebUtils.ShowMessage(this.Page,"读取数据失败！" + eSys.Message.ToString());
			}
		}

        private void BindData(string listid)
		{
            DataTable dt = null;
            dt = new FundService().QueryContractMachineDetail(listid);

			if(dt != null  && dt.Rows.Count > 0)
			{
                lb_CftOrderId.Text = dt.Rows[0]["listid"].ToString();
                lb_FundOrderId.Text = dt.Rows[0]["fund_code"].ToString();
                lb_LtOrderId.Text = dt.Rows[0]["sp_billno"].ToString();
                lb_FreezeTradeId.Text = dt.Rows[0]["freeze_list"].ToString();
                lb_TradeState.Text = dt.Rows[0]["stateStr"].ToString();
                lb_FreezeTime.Text = dt.Rows[0]["freeze_time"].ToString();
                lb_FreezeAmt.Text = dt.Rows[0]["totalfeeStr"].ToString();  //冻结金额
                lb_UnFreezeTime.Text = dt.Rows[0]["unfreeze_time"].ToString(); //解冻时间
                //lb_UnFreezeNo.Text = dt.Rows[0]["listid"].ToString();  //解冻单号
                //lb_Default.Text = dt.Rows[0]["listid"].ToString();  //违约扣款
                lb_Spid.Text = dt.Rows[0]["spid"].ToString();//商户号
                lb_SpNo.Text = dt.Rows[0]["sp_billno"].ToString();//商户订单
                //lb_Present.Text = dt.Rows[0]["listid"].ToString();//赠送份额
                //lb_lstate.Text = dt.Rows[0]["listid"].ToString();//物理状态
                lb_Channel.Text = dt.Rows[0]["chanidStr"].ToString();//购买渠道

                lb_Uin.Text = dt.Rows[0]["uin"].ToString(); //用户账号
                lb_OrderTime.Text = dt.Rows[0]["create_time"].ToString(); //下单时间
                //lb_ModifyTime.Text = dt.Rows[0]["create_time"].ToString(); //最后修改时间
                //lb_BuyType.Text = dt.Rows[0]["listid"].ToString(); //买入方式
                lb_UinFreezeTime.Text = dt.Rows[0]["freeze_time"].ToString(); //用户冻结时间

                //手机/卡信息
                lb_hyjType.Text = dt.Rows[0]["mtype"].ToString(); //合约机机型
                //lb_hyjColor.Text = dt.Rows[0]["listid"].ToString(); //合约机颜色
                //lb_hyjMemory.Text = dt.Rows[0]["listid"].ToString(); //合约机内存大小
                

                //lb_FirstMonth.Text = dt.Rows[0]["listid"].ToString(); //首月资费
                lb_Phone.Text = dt.Rows[0]["bind_mobile"].ToString(); //选择号码
                lb_PlanType.Text = dt.Rows[0]["plan_code"].ToString(); //选择套餐
                lb_PlanCont.Text = dt.Rows[0]["desc"].ToString(); //套餐内容
                lb_creType.Text = dt.Rows[0]["bind_cre_type"].ToString(); //入网证件
                lb_creId.Text = dt.Rows[0]["bind_cre_id"].ToString(); //入网证件id
                lb_BindPhone.Text = dt.Rows[0]["cnee_mobile"].ToString(); //联系电话

                lb_addr.Text = dt.Rows[0]["cnee_address"].ToString(); //收件地址
                lb_Express.Text = dt.Rows[0]["express"].ToString(); //发货快递
                lb_expTicket.Text = dt.Rows[0]["exp_ticket"].ToString(); //快递单号

                DataTable dt2 = new FundService().QueryPhoneDetail(dt.Rows[0]["spid"].ToString(), dt.Rows[0]["bind_mobile"].ToString());
                if (dt2 != null && dt2.Rows.Count > 0) 
                {
                    lb_Area.Text = dt2.Rows[0]["Farea"].ToString(); //手机卡归属地
                    lb_PhoneState.Text = dt2.Rows[0]["FstateStr"].ToString(); //手机号当前状态
                }
			}
			else
			{
                lb_CftOrderId.Text = "";
                lb_FundOrderId.Text = "";
                lb_LtOrderId.Text = "";
                lb_FreezeTradeId.Text = "";
                lb_TradeState.Text = "";
                lb_FreezeTime.Text = "";
                lb_FreezeAmt.Text = "";
                lb_UnFreezeTime.Text = ""; //解冻时间
                lb_UnFreezeNo.Text = "";  //解冻单号
                lb_Default.Text = "";  //违约扣款
                lb_Spid.Text = "";//商户号
                lb_SpNo.Text = "";//商户订单
                lb_Present.Text = "";//赠送份额
                lb_lstate.Text = "";//物理状态
                lb_Channel.Text = "";//购买渠道

                lb_Uin.Text = ""; //用户账号
                lb_OrderTime.Text = ""; //下单时间
                lb_ModifyTime.Text = ""; //最后修改时间
                lb_BuyType.Text = ""; //买入方式
                lb_UinFreezeTime.Text = ""; //用户冻结时间

                //手机/卡信息
                lb_hyjType.Text = ""; //合约机机型
                lb_hyjColor.Text = ""; //合约机颜色
                lb_hyjMemory.Text = ""; //合约机内存大小
                lb_Area.Text = ""; //手机卡归属地
                lb_PhoneState.Text = ""; //手机号当前状态

                lb_FirstMonth.Text = ""; //首月资费
                lb_Phone.Text = ""; //选择号码
                lb_PlanType.Text = ""; //选择套餐
                lb_PlanCont.Text = ""; //套餐内容
                lb_creType.Text = ""; //入网证件
                lb_creId.Text = ""; //入网证件id
                lb_BindPhone.Text = ""; //联系电话

                lb_addr.Text = ""; //收件地址
                lb_Express.Text = ""; //发货快递
                lb_expTicket.Text = ""; //快递单号
			}
		}

        private void QueryData() 
        {
            string listid = txtCftNo.Text.Trim();
            string uin = tbx_payAccount.Text.Trim();
            if (uin != "")
            {
                uin = getQQID();
                ListTable.Visible = true;
                DataSet ds = new FundService().QueryListidFromUin(uin);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataGrid1.DataSource = ds.Tables[0].DefaultView;
                    DataGrid1.DataBind();
                }
                else 
                {
                    DataGrid1.DataSource = null;
                }
            }
            else if (listid != "") 
            {
                ListTable.Visible = false;
                BindData(listid);
            }
        }

        private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string fid = e.Item.Cells[0].Text.Trim(); //ID

            switch (e.CommandName)
            {
                case "DETAIL": //详情
                    BindData(fid);
                    break;
            }
        }

        private string getQQID()
        {
            if (string.IsNullOrEmpty(this.tbx_payAccount.Text))
            {
                return "";
            }
            var id = this.tbx_payAccount.Text.Trim();
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
        private bool getOpenIDFromWeChat(string queryType, string ID, out string openID, out int errorCode, out string errorMessage, string IP)
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

	}
}