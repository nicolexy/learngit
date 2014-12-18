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
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using System.Xml;
using CFT.CSOMS.BLL.ForeignCurrencyModule;
using TENCENT.OSS.CFT.KF.KF_Web.InternetBank;
using CFT.CSOMS.BLL.RefundModule;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using System.Text;

namespace TENCENT.OSS.CFT.KF.KF_Web.RefundManage
{
	/// <summary>
    /// QueryYTTrade 的摘要说明。
	/// </summary>
    public partial class NoticeBankByEmail : System.Web.UI.Page
	{
        private string title;
        string strNewBankName;
        string strNewBankAccNo;
        string strUserName;
        string strReturnDate;
        string strOldBankName;
        string strListTime;
        string strBankListId;
        string strReturnAmt;
        string strAmt;
        private static List<string> listGroupId = new List<string>();
        Hashtable htGroup = new Hashtable();
        protected BankEmailService sysService = new BankEmailService();
        protected void Page_Load(object sender, System.EventArgs e)
		{

			try
			{
                if (!IsPostBack)
                {
                    if (!classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
                    this.pager.RecordCount = 1000;
                    this.pagerContacts.RecordCount = 1000;

                   /*   strNewBankName       = Request.QueryString["newBankName"].ToString();
                    strNewBankAccNo      = Request.QueryString["newBankAccNo"].ToString();
                     strUserName          = Request.QueryString["trueName"].ToString();
                     strReturnDate        = Request.QueryString["returnDate"].ToString();
                     strOldBankName       = Request.QueryString["bankType"].ToString();
                     strListTime          = Request.QueryString["createTime"].ToString();
                     strBankListId        = Request.QueryString["bankListID"].ToString();
                     strReturnAmt         = Request.QueryString["returnAmt"].ToString();
                     strAmt               = Request.QueryString["amt"].ToString();
                    // string strURL = "NoticeBankByEmail.aspx?newBankName=" +arrParam[0]+ "&newBankAccNo="+arrParam[1]+"&trueName="+arrParam[2]+"&returnDate="+arrParam[3]
               // + "&bankType=" + arrParam[4] + "&createTime=" + arrParam[5] + "&bankListID=" + arrParam[6] + "&returnAmt=" + arrParam[7] + "&amt=" + arrParam[8];
                    string strHtml = "<html><head><title></title></head><body>";
                          strHtml +="<p>您好！</p>";
                          strHtml +="<p>以下历史订单，由于用户支付卡状态不正常，导致退款失败。</p>";
                          strHtml +="<p>现已向用户取得新账户，请帮忙核实：</p>";
                    strHtml +="<p>1：该笔订单对应的原卡号，并查询新卡号信息是否正确。</p>";
                    strHtml +="<p>2：新旧卡号是否为同一人，身份证是否相同，谢谢!</p>";
                    strHtml +="<p>  新开户卡号："+ strNewBankAccNo+"</p>";
                    strHtml +="<p>  开户银行："+ strNewBankName+"</p>";   
                    strHtml +="<p>  开户名称："+ strUserName+"</p>"; 
                    strHtml +="<table><tr bgColor='#cccccc'><th>退款日期</th><th>银行</th><th>订单日期</th><th>银行订单号</th><th>退款金额</th><th>交易金额</th></tr>";
                    strHtml += "<tr><td>strReturnDate</td><td>strUserName</td><td>strListTime</td><td>strBankListId</td><td>strReturnAmt</td><td>strAmt</td></tr>";
                    strHtml +="</table></body></html>";
                 

                    string strHtml = "您好！\n\r";
                    strHtml += "以下历史订单，由于用户支付卡状态不正常，导致退款失败。\n\r";
                    strHtml += "现已向用户取得新账户，请帮忙核实：\n\r";
                    strHtml += "1：该笔订单对应的原卡号，并查询新卡号信息是否正确。\n\r";
                    strHtml += "2：新旧卡号是否为同一人，身份证是否相同，谢谢!\n\r";
                    strHtml += "  新开户卡号：" + strNewBankAccNo + "\n\r";
                    strHtml += "  开户银行：" + strNewBankName + "\n\r";
                    strHtml += "  开户名称：" + strUserName + "\n\r";
                    strHtml += "退款日期\t 银行 \t 订单日期 \t\t 银行订单号 \t 退款金额 \t 交易金额\n\r";
                    strHtml += strReturnDate+"\t"+strUserName+"\t"+strListTime+"\t\t"+strBankListId+"\t"+strReturnAmt+"\t"+strAmt+"\n\r";
                    
                    this.tbmaintext.Text = strHtml;
                    this.tbdate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); */
                }
			}
			catch
			{
				Response.Redirect("../login.aspx?wh=1");
			}
            TableGroup.Visible = false;
            TableContacts.Visible = false;
            if (!string.IsNullOrEmpty(Request.QueryString["title"]))
            {
                title = Request.QueryString["title"].Trim();
                ViewState["title"] = title;
            }
            ViewState["newBankName"] = Request.QueryString["newBankName"].ToString();
            ViewState["newBankAccNo"] = Request.QueryString["newBankAccNo"].ToString();
            ViewState["trueName"] = Request.QueryString["trueName"].ToString();
            ViewState["returnDate"] = Request.QueryString["returnDate"].ToString();
            ViewState["bankType"] = Request.QueryString["bankType"].ToString();
            ViewState["createTime"] = Request.QueryString["createTime"].ToString();
            ViewState["bankListID"] = Request.QueryString["bankListID"].ToString();
            ViewState["returnAmt"] = Request.QueryString["returnAmt"].ToString();
            ViewState["amt"] = Request.QueryString["amt"].ToString();
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
            this.DataGridGroup.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGridGroup_ItemCommand);
            this.pagerContacts.PageChanged += new Wuqi.Webdiyer.PageChangedEventHandler(this.ChangePagerContacts);
		}
		#endregion


        private void BindDataGroup(int index)
        {
            try
            {
                TableGroup.Visible = true;
                TableContacts.Visible = false;
                this.pager.CurrentPageIndex = index;
                int max = pager.PageSize;
                int start = max * (index - 1);
                DataSet ds= new DataSet();
                ds = sysService.QueryAllContactsGroup(start, max);
                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    DataGridGroup.DataSource = null;
                    DataGridGroup.DataBind();
                    throw new Exception("数据库无分组记录");
                }

                DataGridGroup.DataSource = ds;
                DataGridGroup.DataBind();
                TableContacts.Visible = false;
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "分组读取数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString())); 
            }
        }

        protected void btnContacts_Click(object sender, System.EventArgs e)
        {
           /* if (string.IsNullOrEmpty(this.tbmaintext.Text.Trim()) || string.IsNullOrEmpty(this.tbdate.Text.Trim()))
            {
                WebUtils.ShowMessage(this.Page, "请输入邮件正文及日期！"); return;
            }*/
            BindDataGroup(1);
            this.btnSendMail.Visible = true;
        }


        protected void btnSubmitGroup_Click(object sender, System.EventArgs e)
        {
            try
            {
                listGroupId.Clear();
                System.Web.UI.WebControls.CheckBox CBox;
                string groupId = "";
                string groupName = "";
                foreach (DataGridItem DgItem in DataGridGroup.Items)
                {
                    CBox = (CheckBox)DgItem.FindControl("selGroup");
                    if (CBox.Checked)
                    {
                        groupId = ((HtmlInputHidden)DgItem.FindControl("selectGroupId")).Value;
                        groupName = ((HtmlInputHidden)DgItem.FindControl("selectGroupName")).Value;
                        htGroup.Add(groupId, groupName);
                    }
                }

                StringBuilder group = new StringBuilder();
                foreach (DictionaryEntry de in htGroup)
                {
                    group.Append(de.Value.ToString());//发送邮件的全部组名
                    group.Append(";");
                    listGroupId.Add(de.Key.ToString());//发送邮件的全部组id
                }

                this.tbGroup.Text = group.ToString();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "添加邮件分组失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        //发送邮件
        protected void btnSendMail_Click(object sender, System.EventArgs e)
        {
            try
            {
                //string maintext = this.tbmaintext.Text.Trim();
                string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //this.tbdate.Text.Trim();
                string strReturnAnt = RefundPublicFun.FenToYuan(ViewState["returnAmt"].ToString()) + "元";
                string strAmt = RefundPublicFun.FenToYuan(ViewState["amt"].ToString()) + "元";
                string emailMsg ="<html><head><title></title></head><body><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:1310px;line-height:22px; margin-left:10px; table-layout:fixed;\" align='left' ID='Table1'><tr><td style='width:1000px;'><p style='padding:10px 0;margin:0;'> "
                + "您好！<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{0}<br><br><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                + "财付通支付科技有限公司<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                +"{1}</p></td></tr></table></body></html>";
                string strHtml = "<html><head><title></title></head><body>";
                //strHtml += "<p>您好！</p>";          
                strHtml += "<p>以下历史订单，由于用户支付卡状态不正常，导致退款失败。</p>";
                strHtml += "<p>现已向用户取得新账户，请帮忙核实：</p>";
                strHtml += "<p>1：该笔订单对应的原卡号，并查询新卡号信息是否正确。</p>";
                strHtml += "<p>2：新旧卡号是否为同一人，身份证是否相同，谢谢!</p>";
                strHtml += "<p>  新开户卡号：" + ViewState["newBankAccNo"] + "</p>";
                strHtml += "<p>  开户银行：" + ViewState["newBankName"] + "</p>";
                strHtml += "<p>  开户名称：" + ViewState["trueName"] + "</p>";
                strHtml += "<p>订单详情</p>";
                strHtml += "<table><tr bgColor='#cccccc' align='center'><th Width='150px'>退款日期 </th><th Width='100px'>银行</th><th Width='150px'>订单日期</th><th Width='200px'>银行订单号</th><th Width='100px' >退款金额</th><th Width='100px' >交易金额</th></tr>";
                strHtml += "<tr align='center'><td>" + ViewState["returnDate"] + "</td><td>" + ViewState["trueName"] + "</td><td>" + ViewState["createTime"] + "</td><td>" + ViewState["bankListID"] + "</td><td>" + strReturnAnt + "</td><td>" + strAmt + "</td></tr>";
                strHtml += "</table></body></html>";
                emailMsg = string.Format(emailMsg, strHtml, date);
                StringBuilder emailList = new StringBuilder();
                foreach (string id in listGroupId)
                {
                    DataSet ds = new DataSet();
                    ds = sysService.QueryOneGroupContacts(id, 0, 1000);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {  
                            //string bccMail = row["Femail"].ToString();//发送密件邮箱地址
                            CommMailSend.SendInternalMailCanSecret("", "", row["Femail"].ToString(),"核对信息", emailMsg.ToString(), true, null);
                        }
                    }
                }

                WebUtils.ShowMessage(this.Page, "发送邮件成功！");
                this.btnSendMail.Visible = false;
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "发送邮件失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        private void DataGridGroup_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string fid = e.Item.Cells[0].Text.Trim();
            string groupName = e.Item.Cells[1].Text.Trim();
            ViewState["groupId"] = fid;
            ViewState["groupName"] = groupName;

            switch (e.CommandName)
            {
                case "QueryOneGroup": //查看分组中联系人
                    QueryOneGroup(fid);
                    break;
            }
        }


        private void QueryOneGroup(string fid,int index=1)
        {

            try
            {
                TableGroup.Visible = true;
                TableContacts.Visible = true;
                this.conGroupName.Text = ViewState["groupName"].ToString();
                this.pagerContacts.CurrentPageIndex = index;
                int max = pagerContacts.PageSize;
                int start = max * (index - 1);
                DataSet ds = new DataSet();
                ds = sysService.QueryOneGroupContacts(fid,start, max);
                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    DataGridContacts.DataSource = null;
                    DataGridContacts.DataBind();
                }

                DataGridContacts.DataSource = ds;
                DataGridContacts.DataBind();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "读取组成员数据失败！" + PublicRes.GetErrorMsg(eSys.Message.ToString())); 
            }
        }

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            BindDataGroup(e.NewPageIndex);
        }

        public void ChangePagerContacts(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            pagerContacts.CurrentPageIndex = e.NewPageIndex;
            QueryOneGroup(ViewState["groupId"].ToString(), e.NewPageIndex);
        }

	}
}