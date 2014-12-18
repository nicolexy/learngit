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
    /// QueryYTTrade ��ժҪ˵����
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
                          strHtml +="<p>���ã�</p>";
                          strHtml +="<p>������ʷ�����������û�֧����״̬�������������˿�ʧ�ܡ�</p>";
                          strHtml +="<p>�������û�ȡ�����˻������æ��ʵ��</p>";
                    strHtml +="<p>1���ñʶ�����Ӧ��ԭ���ţ�����ѯ�¿�����Ϣ�Ƿ���ȷ��</p>";
                    strHtml +="<p>2���¾ɿ����Ƿ�Ϊͬһ�ˣ����֤�Ƿ���ͬ��лл!</p>";
                    strHtml +="<p>  �¿������ţ�"+ strNewBankAccNo+"</p>";
                    strHtml +="<p>  �������У�"+ strNewBankName+"</p>";   
                    strHtml +="<p>  �������ƣ�"+ strUserName+"</p>"; 
                    strHtml +="<table><tr bgColor='#cccccc'><th>�˿�����</th><th>����</th><th>��������</th><th>���ж�����</th><th>�˿���</th><th>���׽��</th></tr>";
                    strHtml += "<tr><td>strReturnDate</td><td>strUserName</td><td>strListTime</td><td>strBankListId</td><td>strReturnAmt</td><td>strAmt</td></tr>";
                    strHtml +="</table></body></html>";
                 

                    string strHtml = "���ã�\n\r";
                    strHtml += "������ʷ�����������û�֧����״̬�������������˿�ʧ�ܡ�\n\r";
                    strHtml += "�������û�ȡ�����˻������æ��ʵ��\n\r";
                    strHtml += "1���ñʶ�����Ӧ��ԭ���ţ�����ѯ�¿�����Ϣ�Ƿ���ȷ��\n\r";
                    strHtml += "2���¾ɿ����Ƿ�Ϊͬһ�ˣ����֤�Ƿ���ͬ��лл!\n\r";
                    strHtml += "  �¿������ţ�" + strNewBankAccNo + "\n\r";
                    strHtml += "  �������У�" + strNewBankName + "\n\r";
                    strHtml += "  �������ƣ�" + strUserName + "\n\r";
                    strHtml += "�˿�����\t ���� \t �������� \t\t ���ж����� \t �˿��� \t ���׽��\n\r";
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

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
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
                    throw new Exception("���ݿ��޷����¼");
                }

                DataGridGroup.DataSource = ds;
                DataGridGroup.DataBind();
                TableContacts.Visible = false;
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "�����ȡ����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString())); 
            }
        }

        protected void btnContacts_Click(object sender, System.EventArgs e)
        {
           /* if (string.IsNullOrEmpty(this.tbmaintext.Text.Trim()) || string.IsNullOrEmpty(this.tbdate.Text.Trim()))
            {
                WebUtils.ShowMessage(this.Page, "�������ʼ����ļ����ڣ�"); return;
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
                    group.Append(de.Value.ToString());//�����ʼ���ȫ������
                    group.Append(";");
                    listGroupId.Add(de.Key.ToString());//�����ʼ���ȫ����id
                }

                this.tbGroup.Text = group.ToString();
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "����ʼ�����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
            }
        }

        //�����ʼ�
        protected void btnSendMail_Click(object sender, System.EventArgs e)
        {
            try
            {
                //string maintext = this.tbmaintext.Text.Trim();
                string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //this.tbdate.Text.Trim();
                string strReturnAnt = RefundPublicFun.FenToYuan(ViewState["returnAmt"].ToString()) + "Ԫ";
                string strAmt = RefundPublicFun.FenToYuan(ViewState["amt"].ToString()) + "Ԫ";
                string emailMsg ="<html><head><title></title></head><body><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:1310px;line-height:22px; margin-left:10px; table-layout:fixed;\" align='left' ID='Table1'><tr><td style='width:1000px;'><p style='padding:10px 0;margin:0;'> "
                + "���ã�<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{0}<br><br><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                + "�Ƹ�֧ͨ���Ƽ����޹�˾<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                +"{1}</p></td></tr></table></body></html>";
                string strHtml = "<html><head><title></title></head><body>";
                //strHtml += "<p>���ã�</p>";          
                strHtml += "<p>������ʷ�����������û�֧����״̬�������������˿�ʧ�ܡ�</p>";
                strHtml += "<p>�������û�ȡ�����˻������æ��ʵ��</p>";
                strHtml += "<p>1���ñʶ�����Ӧ��ԭ���ţ�����ѯ�¿�����Ϣ�Ƿ���ȷ��</p>";
                strHtml += "<p>2���¾ɿ����Ƿ�Ϊͬһ�ˣ����֤�Ƿ���ͬ��лл!</p>";
                strHtml += "<p>  �¿������ţ�" + ViewState["newBankAccNo"] + "</p>";
                strHtml += "<p>  �������У�" + ViewState["newBankName"] + "</p>";
                strHtml += "<p>  �������ƣ�" + ViewState["trueName"] + "</p>";
                strHtml += "<p>��������</p>";
                strHtml += "<table><tr bgColor='#cccccc' align='center'><th Width='150px'>�˿����� </th><th Width='100px'>����</th><th Width='150px'>��������</th><th Width='200px'>���ж�����</th><th Width='100px' >�˿���</th><th Width='100px' >���׽��</th></tr>";
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
                            //string bccMail = row["Femail"].ToString();//�����ܼ������ַ
                            CommMailSend.SendInternalMailCanSecret("", "", row["Femail"].ToString(),"�˶���Ϣ", emailMsg.ToString(), true, null);
                        }
                    }
                }

                WebUtils.ShowMessage(this.Page, "�����ʼ��ɹ���");
                this.btnSendMail.Visible = false;
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "�����ʼ�ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
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
                case "QueryOneGroup": //�鿴��������ϵ��
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
                WebUtils.ShowMessage(this.Page, "��ȡ���Ա����ʧ�ܣ�" + PublicRes.GetErrorMsg(eSys.Message.ToString())); 
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